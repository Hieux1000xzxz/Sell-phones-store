using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace BanDienThoaiDiDong
{
    public partial class Profile : System.Web.UI.Page
    {
     
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập ngay từ đầu
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUserInfo();
                LoadOrders();
                // Set view mặc định
                mvProfile.SetActiveView(viewProfile);
                SetActiveTab(lnkProfile);
            }
        }
        
        private void LoadUserInfo()
        {
            try
            {
                // Kiểm tra và gán giá trị an toàn cho các control
                litUsername.Text = Session["Username"]?.ToString() ?? "";
                litEmail.Text = Session["Email"]?.ToString() ?? "";
                txtFullName.Text = Session["FullName"]?.ToString() ?? "";
                txtEmail.Text = Session["Email"]?.ToString() ?? "";
                txtPhone.Text = Session["Phone"]?.ToString() ?? "";
                txtAddress.Text = Session["Address"]?.ToString() ?? "";

                // Xử lý ngày sinh
                if (DateTime.TryParse(Session["Birthday"]?.ToString(), out DateTime birthday))
                {
                    txtBirthday.Text = birthday.ToString("yyyy-MM-dd");
                }

                // Xử lý giới tính
                string gender = Session["Gender"]?.ToString()?.ToLower() ?? "male";
                if (ddlGender.Items.FindByValue(gender) != null)
                {
                    ddlGender.SelectedValue = gender;
                }
                else
                {
                    ddlGender.SelectedValue = "male"; // Giá trị mặc định
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Có lỗi khi tải thông tin người dùng: " + ex.Message + "');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    int userId = Convert.ToInt32(Session["UserID"]);

                    string updateQuery = @"UPDATE Users 
                                 SET FullName = @FullName,
                                     Email = @Email,
                                     Phone = @Phone,
                                     Address = @Address,
                                     Birthday = @Birthday,
                                     Gender = @Gender,
                                     UpdatedAt = GETDATE()
                                 WHERE UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        // Thêm các tham số với kiểm tra null
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@FullName", string.IsNullOrEmpty(txtFullName.Text) ? (object)DBNull.Value : txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim());

                        // Xử lý ngày sinh
                        if (DateTime.TryParse(txtBirthday.Text, out DateTime birthday))
                        {
                            cmd.Parameters.AddWithValue("@Birthday", birthday);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Birthday", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật lại Session
                    Session["FullName"] = txtFullName.Text.Trim();
                    Session["Email"] = txtEmail.Text.Trim();
                    Session["Phone"] = txtPhone.Text.Trim();
                    Session["Address"] = txtAddress.Text.Trim();
                    Session["Birthday"] = txtBirthday.Text;
                    Session["Gender"] = ddlGender.SelectedValue;

                    // Cập nhật lại thông tin hiển thị
                    LoadUserInfo();

                    // Hiển thị thông báo thành công
                    ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSuccess",
                        "alert('Cập nhật thông tin thành công');", true);

                    // Cập nhật lại tên hiển thị trên master page
                    var masterPage = this.Master as Site1;
                    if (masterPage != null)
                    {
                        masterPage.UpdateLoginStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateError",
                    $"alert('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Tải lại thông tin từ database
            LoadUserInfo();
        }

        protected void lnkProfile_Click(object sender, EventArgs e)
        {
            mvProfile.SetActiveView(viewProfile);
            SetActiveTab(lnkProfile);
        }

        protected void lnkOrders_Click(object sender, EventArgs e)
        {
            mvProfile.SetActiveView(viewOrders);
            SetActiveTab(lnkOrders);
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        private void SetActiveTab(LinkButton activeButton)
        {
            lnkProfile.CssClass = "nav-item";
            lnkOrders.CssClass = "nav-item";
            activeButton.CssClass = "nav-item active";
        }

        protected void OrderTab_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton clickedButton = (LinkButton)sender;

                // Reset tất cả các tab
                btnAllOrders.CssClass = "tab-btn";
                btnPendingOrders.CssClass = "tab-btn";
                btnShippingOrders.CssClass = "tab-btn";
                btnDeliveredOrders.CssClass = "tab-btn";
                btnCancelledOrders.CssClass = "tab-btn";

                // Set active cho tab được click
                clickedButton.CssClass = "tab-btn active";

                // Debug để kiểm tra ID của button được click
                System.Diagnostics.Debug.WriteLine($"Clicked button ID: {clickedButton.ID}");

                LoadOrders(clickedButton.ID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('Lỗi khi chuyển tab: {ex.Message}');", true);
            }
        }

        private void LoadOrders(string buttonId = null)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    int userId = Convert.ToInt32(Session["UserID"]);
                    string orderQuery = "SELECT * FROM Orders WHERE UserID = @UserID";

                    // Xác định trạng thái từ ID của button
                    string status = null;
                    if (!string.IsNullOrEmpty(buttonId))
                    {
                        switch (buttonId)
                        {
                            case "btnPendingOrders":
                                status = "Chờ xác nhận";
                                break;
                            case "btnShippingOrders":
                                status = "Đang giao";
                                break;
                            case "btnDeliveredOrders":
                                status = "Đã giao";
                                break;
                            case "btnCancelledOrders":
                                status = "Đã hủy";
                                break;
                                // Trường hợp btnAllOrders hoặc default: không thêm điều kiện status
                        }

                        if (!string.IsNullOrEmpty(status))
                        {
                            orderQuery += " AND Status = @Status";
                        }
                    }

                    orderQuery += " ORDER BY OrderDate DESC";

                    using (MySqlCommand cmd = new MySqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        if (!string.IsNullOrEmpty(status))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Order> orders = new List<Order>();
                            while (reader.Read())
                            {
                                int x;
                                var order = new Order
                                {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    Status = reader["Status"].ToString(),
                                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),                          

                            };
                              
                                orders.Add(order);
                            }

                            rptOrders.DataSource = orders;
                            rptOrders.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error",
                    $"alert('Lỗi khi tải đơn hàng: {ex.Message}');", true);
            }
        }
        private List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        p.ProductName,
                        od.Quantity,
                        od.Price,
                        COALESCE(pv.VarImageUrl, p.DefaultImageUrl) as ImageUrl,
                        pv.Color,
                        pv.Storage
                    FROM OrderDetails od
                    INNER JOIN Products p ON od.ProductID = p.ProductID
                    LEFT JOIN ProductVariants pv ON od.VariantID = pv.VariantID
                    WHERE od.OrderID = @OrderID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new OrderItem
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    ImageUrl = reader["ImageUrl"].ToString()
                                };

                                // Thêm thông tin biến thể nếu có
                                string color = reader["Color"]?.ToString();
                                string storage = reader["Storage"]?.ToString();
                                if (!string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(storage))
                                {
                                    item.ProductName += " - " +
                                        (string.IsNullOrEmpty(color) ? "" : color) +
                                        (string.IsNullOrEmpty(storage) ? "" : $" {storage}GB");
                                }

                                orderItems.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine($"Error in GetOrderItems: {ex.Message}");
                throw; // Ném lại exception để xử lý ở tầng trên
            }

            return orderItems;
        }


        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetail")
            {
                // Handle view order detail
                string orderId = e.CommandArgument.ToString();
                Response.Redirect($"OrderDetail.aspx?OrderID={orderId}");
                // TODO: Implement order detail view logic
            }
            else if (e.CommandName == "CancelOrder")
            {
                // Handle order cancellation
                string orderId = e.CommandArgument.ToString();
                // TODO: Implement order cancellation logic
            }
        }
       
        protected string GetStatusClass(string status)
        {
            switch (status.ToLower())
            {
                case "Chờ xác nhận":
                    return "Chờ xác nhận";
                case "Đang giao":
                    return "Đang giao";
                case "Đã giao":
                    return "Đã giao";
                case "Đã hủy":
                    return "Đã hủy";
                default:
                    return "";
            }
        }

        protected string ShowCancelButton(string status)
        {
            if (status == "Chờ xác nhận")
            {
                return $@"<asp:LinkButton runat='server' CssClass='action-btn cancel-btn' 
                         CommandName='CancelOrder' CommandArgument='<%# Eval(""OrderID"") %>'>
                         Hủy đơn
                         </asp:LinkButton>";
            }
            return string.Empty;
        }

        protected string FormatPrice(object price)
        {
            if (decimal.TryParse(price.ToString(), out decimal value))
            {
                return value.ToString("#,##0 đ");
            }
            return "0 đ";
        }
    }
}

