using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
                lblLastUpdate.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT 
                                IFNULL(SUM(CASE WHEN o.Status = 'Đã giao' THEN o.TotalAmount ELSE 0 END), 0) as TotalRevenue,
                                COUNT(CASE WHEN o.Status = 'Chờ xác nhận' THEN 1 END) as NewOrders,
                                (SELECT SUM(Stock) FROM ProductVariants) as TotalStock,
                                (SELECT COUNT(*) FROM Users WHERE IsActive = 1) as TotalUsers
                            FROM Orders o";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal totalRevenue = reader.GetDecimal(0);
                                lblTotalRevenue.Text = string.Format("{0:N0}₫", totalRevenue);
                                lblNewOrders.Text = reader["NewOrders"].ToString();
                                lblTotalStock.Text = string.Format("{0:N0}", reader["TotalStock"]);
                                lblTotalUsers.Text = string.Format("{0:N0}", reader["TotalUsers"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Không thể tải dữ liệu tổng quan: " + ex.Message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "error", script, true);
        }
    }
}