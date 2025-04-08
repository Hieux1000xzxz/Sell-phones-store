using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadUsers();
        }

        private void LoadUsers()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT UserID, FullName, Email, Phone, Role 
                     FROM Users 
                     WHERE @Search = '' OR 
                      FullName LIKE CONCAT('%', @Search, '%') OR 
                      Email LIKE CONCAT('%', @Search, '%') OR 
                      Phone LIKE CONCAT('%', @Search, '%')";


                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());
                    DataTable dt = new DataTable();
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e) => LoadUsers();
        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvUsers.Rows[e.RowIndex];
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                string fullName = (row.FindControl("txtFullName") as TextBox).Text.Trim();
                string email = (row.FindControl("txtEmail") as TextBox).Text.Trim();
                string phone = (row.FindControl("txtPhone") as TextBox).Text.Trim();

                using (MySqlConnection conn = Database.GetConnection())
                {
                    string sql = @"UPDATE Users 
                                 SET FullName = @FullName, Email = @Email, Phone = @Phone 
                                 WHERE UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                gvUsers.EditIndex = -1;
                LoadUsers();
                ShowMessage("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message);
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string[] deleteQueries = {
                                "DELETE FROM CartItems WHERE UserID = @UserID",
                                "DELETE FROM OrderDetails WHERE OrderID IN (SELECT OrderID FROM Orders WHERE UserID = @UserID)",
                                "DELETE FROM Orders WHERE UserID = @UserID",
                                "DELETE FROM Users WHERE UserID = @UserID AND Role != 'Admin'"
                            };

                            foreach (string sql in deleteQueries)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(sql, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@UserID", userId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            LoadUsers();
                            ShowMessage("Xóa tài khoản thành công!");
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message);
            }
        }

        private void ShowMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }
}