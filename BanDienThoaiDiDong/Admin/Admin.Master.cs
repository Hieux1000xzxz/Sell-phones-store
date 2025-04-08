using System;
using System.IO;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminAccess();
            if (!IsPostBack)
            {
                LoadAdminInfo();
                SetActiveMenu();
            }
        }

        private void SetActiveMenu()
        {
            string currentPage = Path.GetFileName(Request.Path).ToLower();
            lnkDashboard.CssClass = "menu-item" + (currentPage == "admindashboard.aspx" ? " active" : "");
            lnkProducts.CssClass = "menu-item" + (currentPage == "productmanagement.aspx" ? " active" : "");
            lnkOrders.CssClass = "menu-item" + (currentPage == "ordermanagement.aspx" ? " active" : "");
            lnkUsers.CssClass = "menu-item" + (currentPage == "usermanagement.aspx" ? " active" : "");
        }

        private void CheckAdminAccess()
        {
            // Kiểm tra session và quyền admin
            if (Session["UserID"] == null || !IsAdmin())
            {
                // Lưu URL hiện tại để redirect sau khi đăng nhập
                Session["ReturnUrl"] = Request.Url.PathAndQuery;
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        private void LoadAdminInfo()
        {
            if (Session["FullName"] != null)
            {
                lblAdminName.Text = Session["FullName"].ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Ghi log đăng xuất
                LogAdminAction("Logout");

                // Xóa session
                Session.Clear();
                Session.Abandon();

                // Chuyển về trang đăng nhập
                Response.Redirect("~/Login.aspx");
            }
            catch (Exception ex)
            {
                // Log lỗi và hiển thị thông báo
                ShowErrorMessage("Có lỗi xảy ra khi đăng xuất: " + ex.Message);
            }
        }

        private bool IsAdmin()
        {
            if (Session["UserID"] == null)
                return false;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT Role FROM Users 
                                WHERE UserID = @UserID AND IsActive = 1";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                        string role = cmd.ExecuteScalar()?.ToString();
                        return role == "Admin";
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void LogAdminAction(string action)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"INSERT INTO AdminLogs (UserID, Action, ActionDate, IPAddress)
                                VALUES (@UserID, @Action, NOW(), @IPAddress)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                        cmd.Parameters.AddWithValue("@Action", action);
                        cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi logging
            }
        }

        private void ShowErrorMessage(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "error", script, true);
        }
    }
}