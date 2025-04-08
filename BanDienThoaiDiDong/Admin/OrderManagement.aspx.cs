using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }
        private void UpdateVariantQuantities(MySqlConnection conn, MySqlTransaction transaction, int orderId, bool isCancel)
        {
            string sql = @"
                        UPDATE ProductVariants pv
                        INNER JOIN OrderDetails od ON pv.VariantID = od.VariantID
                        SET 
                            pv.Stock = pv.Stock + CASE WHEN @IsCancel = 1 THEN od.Quantity ELSE -od.Quantity END,
                            pv.Sold = pv.Sold + CASE WHEN @IsCancel = 1 THEN -od.Quantity ELSE od.Quantity END
                        WHERE od.OrderID = @OrderID";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@IsCancel", isCancel);
                cmd.ExecuteNonQuery();
            }
        }
        private void LoadOrders()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT o.OrderID, u.FullName as CustomerName, u.Phone, 
                    o.TotalAmount, o.OrderDate, o.Status, o.ShippingAddress
                    FROM Orders o
                    INNER JOIN Users u ON o.UserID = u.UserID
                    WHERE (@Status = '' OR o.Status = @Status)
                    AND (@Search = '' OR o.OrderID LIKE CONCAT('%', @Search, '%'))
                    AND (@DateFilter = '' 
                        OR (@DateFilter = 'today' AND DATE(o.OrderDate) = CURDATE())
                        OR (@DateFilter = 'week' AND o.OrderDate >= DATE_SUB(CURDATE(), INTERVAL 7 DAY))
                        OR (@DateFilter = 'month' AND o.OrderDate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)))
                    ORDER BY o.OrderDate DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateFilter", ddlDateFilter.SelectedValue);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvOrders.DataSource = dt;
                        gvOrders.DataBind();
                    }
                }
            }
        }

        protected string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Chờ xác nhận": return "pending";
                case "Đang giao": return "shipping";
                case "Đã giao": return "delivered";
                case "Đã hủy": return "cancelled";
                default: return "unknown";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void ddlDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrders();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int orderId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "ViewOrder":
                    Response.Redirect($"~/Admin/OrderDetailAdmin.aspx?id={orderId}");
                    break;

                case "EditStatus":
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    Panel pnlStatus = (Panel)row.FindControl("pnlStatus");
                    Label lblStatus = (Label)pnlStatus.FindControl("lblStatus");
                    DropDownList ddlStatus = (DropDownList)pnlStatus.FindControl("ddlOrderStatus");

                    // Thêm trạng thái hiện tại vào DropDownList nếu chưa có
                    string currentStatus = lblStatus.Text;
                    if (!ddlStatus.Items.Contains(new ListItem(currentStatus)))
                    {
                        ddlStatus.Items.Insert(0, new ListItem(currentStatus));
                    }

                    // Hiển thị dropdown và ẩn label
                    lblStatus.Visible = false;
                    ddlStatus.Visible = true;
                    ddlStatus.SelectedValue = currentStatus;
                    break;
            }
        }

        protected void ddlOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int orderId = Convert.ToInt32(gvOrders.DataKeys[row.RowIndex].Value);
            string newStatus = ddl.SelectedValue;

            // Cập nhật trạng thái trong database
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update order status
                        string updateOrderSql = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                        using (MySqlCommand cmd = new MySqlCommand(updateOrderSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Status", newStatus);
                            cmd.Parameters.AddWithValue("@OrderID", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // Xử lý các trường hợp thay đổi trạng thái
                        if (newStatus == "Đang giao" || newStatus == "Đã giao")
                        {
                            UpdateVariantQuantities(conn, transaction, orderId, false);
                        }
                        else if (newStatus == "Hủy")
                        {
                            UpdateVariantQuantities(conn, transaction, orderId, true);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            // Hiển thị lại label và ẩn dropdown
            Panel pnlStatus = (Panel)row.FindControl("pnlStatus");
            Label lblStatus = (Label)pnlStatus.FindControl("lblStatus");
            lblStatus.Text = newStatus;
            lblStatus.Visible = true;
            ddl.Visible = false;

            // Cập nhật lại CSS class cho label
            lblStatus.CssClass = "status-" + GetStatusClass(newStatus);
        }
    }
}