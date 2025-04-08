using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT UserID, FullName, Email, Phone, Address, Birthday, Gender, Role, IsActive
                                FROM Users 
                                WHERE Email = @Email AND Password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lưu thông tin user vào session
                                Session["UserID"] = reader["UserID"];
                                Session["FullName"] = reader["FullName"];
                                Session["Email"] = reader["Email"];
                                Session["Phone"] = reader["Phone"];
                                Session["Address"] = reader["Address"] ?? "";
                                Session["Birthday"] = reader["Birthday"] ?? DBNull.Value;
                                Session["Gender"] = reader["Gender"] ?? "";
                                Session["Role"] = reader["Role"].ToString();
                                Session["Role"] = reader["Role"]?.ToString() ?? "User";
                                if (Session["Role"].ToString() == "Admin")
                                {
                                    Response.Redirect("~/Admin/AdminDashboard.aspx");
                                    return;
                                }
                                // Cập nhật master page
                                var masterPage = this.Master as Site1;
                                if (masterPage != null)
                                {
                                    masterPage.UpdateLoginStatus();
                                }
                                string returnUrl = Session["ReturnUrl"] as string;
                                MergeCartWithDatabase();
                                if (!string.IsNullOrEmpty(returnUrl))
                                {
                                    Session["ReturnUrl"] = null; // Xóa URL đã lưu
                                    Response.Redirect(returnUrl);
                                }
                                else
                                {
                                    Response.Redirect("~/Default.aspx");
                                }
                            }
                            else
                            {
                                ShowError("Email hoặc mật khẩu không đúng");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Đã có lỗi xảy ra. Vui lòng thử lại sau.");
            }
        }

        private void ShowError(string message)
        {
            Label lblError = FindControl("lblError") as Label;
            if (lblError != null)
            {
                lblError.Text = message;
                lblError.Visible = true;
            }
        }
        private void MergeCartWithDatabase()
        {
            var sessionCart = Session["Cart"] as List<CartItem>;
            if (sessionCart != null && sessionCart.Any())
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Lấy giỏ hàng hiện tại từ database
                            string selectSql = @"SELECT ProductID, Quantity FROM CartItems 
                                       WHERE UserID = @UserID";

                            Dictionary<int, int> dbCart = new Dictionary<int, int>();
                            using (MySqlCommand cmd = new MySqlCommand(selectSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserID", userId);
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        dbCart[reader.GetInt32(0)] = reader.GetInt32(1);
                                    }
                                }
                            }

                            // Merge items
                            foreach (var item in sessionCart)
                            {
                                if (dbCart.ContainsKey(item.ProductId))
                                {
                                    // Update quantity if product exists
                                    string updateSql = @"UPDATE CartItems 
                                               SET Quantity = Quantity + @Quantity,
                                                   UpdatedAt = GETDATE()
                                               WHERE UserID = @UserID 
                                               AND ProductID = @ProductID";

                                    using (MySqlCommand cmd = new MySqlCommand(updateSql, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@UserID", userId);
                                        cmd.Parameters.AddWithValue("@ProductID", item.ProductId);
                                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Insert new item if product doesn't exist
                                    string insertSql = @"INSERT INTO CartItems 
                                               (UserID, ProductID, VariantID, Quantity)
                                               VALUES (@UserID, @ProductID, @VariantID, @Quantity)";

                                    using (MySqlCommand cmd = new MySqlCommand(insertSql, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@UserID", userId);
                                        cmd.Parameters.AddWithValue("@ProductID", item.ProductId);
                                        cmd.Parameters.AddWithValue("@VariantID", item.VariantId ?? (object)DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // Clear session cart after merging
                Session["Cart"] = null;
            }
        }
    }
}
