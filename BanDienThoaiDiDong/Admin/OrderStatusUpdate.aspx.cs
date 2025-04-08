using System;
using System.Data.SqlClient;

namespace BanDienThoaiDiDong
{
    public partial class OrderStatusUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int orderId = Convert.ToInt32(Request.QueryString["id"]);
                    LoadOrderInfo(orderId);
                }
                else
                {
                    Response.Redirect("~/Admin/OrderManagement.aspx");
                }
            }
        }

        private void LoadOrderInfo(int orderId)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT o.*, u.FullName, u.Phone 
                            FROM Orders o
                            INNER JOIN Users u ON o.UserID = u.UserID
                            WHERE o.OrderID = @OrderID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblOrderId.Text = orderId.ToString();
                            lblCustomerName.Text = reader["FullName"].ToString();
                            lblPhone.Text = reader["Phone"].ToString();
                            lblTotalAmount.Text = string.Format("{0:N0}₫", reader["TotalAmount"]);
                            lblCurrentStatus.Text = reader["Status"].ToString();
                            ddlNewStatus.SelectedValue = reader["Status"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(Request.QueryString["id"]);
            string newStatus = ddlNewStatus.SelectedValue;
            string currentStatus = lblCurrentStatus.Text;

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update order status
                        string updateOrderSql = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                        using (SqlCommand cmd = new SqlCommand(updateOrderSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Status", newStatus);
                            cmd.Parameters.AddWithValue("@OrderID", orderId);
                            cmd.ExecuteNonQuery();
                        }


                        // Xử lý các trường hợp thay đổi trạng thái
                        if (currentStatus == "Chờ xác nhận")
                        {
                            if (newStatus == "Đã giao" || newStatus == "Đang giao")
                            {
                                // Trừ stock khi khôi phục đơn từ trạng thái hủy
                                UpdateVariantQuantities(conn, transaction, orderId, false);
                            } 
                            else if (newStatus == "Đã hủy")
                            {
                                // Hoàn lại stock khi hủy đơn
                                UpdateVariantQuantities(conn, transaction, orderId, true);
                            }
                        }
                        else if (currentStatus == "Đã hủy")
                        {
                            if (newStatus == "Chờ xác nhận" || newStatus == "Đang giao")
                            {
                                // Trừ stock khi khôi phục đơn từ trạng thái hủy
                                UpdateVariantQuantities(conn, transaction, orderId, false);
                            }
                        }
                        else if (currentStatus=="Đang giao" || currentStatus =="Đã giao")
                        {
                            if (newStatus == "Đã hủy")
                            {
                                // Hoàn lại stock khi hủy đơn
                                UpdateVariantQuantities(conn, transaction, orderId, true);
                            }
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

            Response.Redirect("~/Admin/OrderManagement.aspx");
        }

        private void UpdateVariantQuantities(SqlConnection conn, SqlTransaction transaction, int orderId, bool isCancel)
        {
            string sql = @"
                        UPDATE pv
                        SET 
                            pv.Stock = pv.Stock + CASE WHEN @IsCancel = 1 THEN od.Quantity ELSE -od.Quantity END,
                            pv.Sold = pv.Sold + CASE WHEN @IsCancel = 1 THEN -od.Quantity ELSE od.Quantity END
                        FROM ProductVariants pv
                        INNER JOIN OrderDetails od ON pv.VariantID = od.VariantID
                        WHERE od.OrderID = @OrderID";

            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@IsCancel", isCancel);
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/OrderManagement.aspx");
        }
    }
}