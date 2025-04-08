using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class OrderDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int orderId;
                if (int.TryParse(Request.QueryString["OrderId"], out orderId))
                {
                    LoadOrderInfo(orderId);
                    LoadOrderDetails(orderId);
                }
            }
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            int orderId;
            if (int.TryParse(Request.QueryString["OrderId"], out orderId))
            {
                try
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();
                        using (MySqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // Cập nhật trạng thái đơn hàng
                                string updateOrderSql = "UPDATE Orders SET Status = N'Đã hủy' WHERE OrderID = @OrderID AND Status = N'Chờ xác nhận'";
                                using (MySqlCommand cmd = new MySqlCommand(updateOrderSql, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Hoàn lại số lượng tồn kho
                                        string updateStockSql = @"
                                            UPDATE pv
                                            SET pv.Stock = pv.Stock + od.Quantity
                                            FROM ProductVariants pv
                                            JOIN OrderDetails od ON pv.VariantID = od.VariantID
                                            WHERE od.OrderID = @OrderID";

                                        using (MySqlCommand updateStockCmd = new MySqlCommand(updateStockSql, conn, transaction))
                                        {
                                            updateStockCmd.Parameters.AddWithValue("@OrderID", orderId);
                                            updateStockCmd.ExecuteNonQuery();
                                        }

                                        transaction.Commit();

                                        // Cập nhật UI
                                        lblOrderStatus.Text = "Đã hủy";
                                        lblOrderStatus.CssClass = "order-status status-cancelled";
                                        btnCancelOrder.Visible = false;

                                        // Hiển thị thông báo thành công
                                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowMessage",
                                            "showMessage('Đơn hàng đã được hủy thành công', true);", true);
                                    }
                                    else
                                    {
                                        throw new Exception("Không thể hủy đơn hàng này");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hiển thị thông báo lỗi
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowMessage",
                        $"showMessage('Có lỗi xảy ra: {ex.Message}', false);", true);
                }
            }
        }

        private void LoadOrderInfo(int orderId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT o.*, u.FullName, u.Email
                    FROM Orders o
                    JOIN Users u ON o.UserID = u.UserID
                    WHERE o.OrderID = @OrderID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblOrderId.Text = orderId.ToString();
                            lblOrderDate.Text = Convert.ToDateTime(reader["OrderDate"]).ToString("dd/MM/yyyy HH:mm");
                            lblCustomerName.Text = reader["FullName"].ToString();
                            lblEmail.Text = reader["Email"].ToString();
                            lblPhone.Text = reader["ShippingPhone"].ToString();
                            lblShippingAddress.Text = reader["ShippingAddress"].ToString();
                            lblNote.Text = reader["Note"]?.ToString() ?? "Không có ghi chú";

                            string status = reader["Status"].ToString();
                            lblOrderStatus.Text = status;
                            lblOrderStatus.CssClass = $"order-status status-{GetStatusClass(status)}";
                            btnCancelOrder.Visible = status == "Chờ xác nhận";

                            decimal totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            lblTotal.Text = String.Format("{0:N0}₫", totalAmount);
                            lblSubTotal.Text = String.Format("{0:N0}₫", totalAmount);
                            lblShippingFee.Text = "Miễn phí";
                        }
                    }
                }
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            var orderDetails = new List<OrderDetailItem>();
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        od.Quantity, 
                        od.Price,
                        p.ProductName,
                        p.DefaultImageUrl,
                        pv.Color,
                        pv.Storage
                    FROM OrderDetails od
                    JOIN Products p ON od.ProductID = p.ProductID
                    JOIN ProductVariants pv ON od.VariantID = pv.VariantID
                    WHERE od.OrderID = @OrderID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderDetails.Add(new OrderDetailItem
                            {
                                ProductName = reader["ProductName"].ToString(),
                                Color = reader["Color"].ToString(),
                                Storage = reader["Storage"].ToString(),
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"],
                                ImageUrl = reader["DefaultImageUrl"].ToString()
                            });
                        }
                    }
                }
            }
            rptOrderDetails.DataSource = orderDetails;
            rptOrderDetails.DataBind();
        }

        private string GetStatusClass(string status)
        {
            switch (status.ToLower())
            {
                case "chờ xác nhận": return "pending";
                case "đã xác nhận": return "confirmed";
                case "đang giao": return "shipping";
                case "đã giao": return "delivered";
                case "đã hủy": return "cancelled";
                default: return "pending";
            }
        }
    }

    public class OrderDetailItem
    {
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string Storage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}