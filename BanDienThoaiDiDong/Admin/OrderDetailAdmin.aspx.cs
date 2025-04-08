using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class OrderDetailAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int orderId = Convert.ToInt32(Request.QueryString["id"]);
                    LoadOrderInfo(orderId);
                    LoadOrderItems(orderId);
                }
                else
                {
                    Response.Redirect("~/Admin/OrderManagement.aspx");
                }
            }
        }

        private void LoadOrderInfo(int orderId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT o.*, u.FullName, u.Email, u.Phone 
                    FROM Orders o
                    INNER JOIN Users u ON o.UserID = u.UserID
                    WHERE o.OrderID = @OrderID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblOrderId.Text = orderId.ToString();
                            lblCustomerName.Text = reader["FullName"].ToString();
                            lblEmail.Text = reader["Email"].ToString();
                            lblPhone.Text = reader["Phone"].ToString();
                            lblAddress.Text = reader["ShippingAddress"].ToString();
                            lblNote.Text = reader["Note"] != DBNull.Value ? reader["Note"].ToString() : "";
                            lblStatus.Text = reader["Status"].ToString();
                            lblStatus.CssClass = $"order-status status-{GetStatusClass(reader["Status"].ToString())}";
                            lblTotalAmount.Text = string.Format("{0:N0}₫", reader["TotalAmount"]);

                            decimal shippingFee = reader["ShippingFee"] != DBNull.Value ?
                                Convert.ToDecimal(reader["ShippingFee"]) : 0;
                            lblShippingFee.Text = string.Format("{0:N0}₫", shippingFee);

                            decimal totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            decimal subTotal = totalAmount - shippingFee;
                            lblSubTotal.Text = string.Format("{0:N0}₫", subTotal);
                        }
                    }
                }
            }
        }

        private void LoadOrderItems(int orderId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT od.*, p.ProductName, 
                        IFNULL(p.DefaultImageUrl, '~/assets/images/default-product.png') as ImageUrl,
                        IFNULL(pv.Color, 'Không rõ') as Color, 
                        IFNULL(pv.Storage, 'Không rõ') as Storage,
                        (od.Quantity * od.Price) as SubTotal
                    FROM OrderDetails od
                    INNER JOIN Products p ON od.ProductID = p.ProductID
                    INNER JOIN ProductVariants pv ON od.VariantID = pv.VariantID
                    WHERE od.OrderID = @OrderID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvOrderItems.DataSource = dt;
                        gvOrderItems.DataBind();
                    }
                }
            }
        }

        protected string GetStatusClass(string status)
        {
            switch (status)
            {
                case "Chờ xác nhận": return "pending";
                case "Đã xác nhận": return "confirmed";
                case "Đang giao": return "shipping";
                case "Đã giao": return "delivered";
                case "Đã hủy": return "cancelled";
                default: return "unknown";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/OrderManagement.aspx");
        }
    }
}