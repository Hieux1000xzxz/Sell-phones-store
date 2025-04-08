using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear form fields
                txtFullName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtBirthday.Text = string.Empty;
                ddlGender.SelectedIndex = 0;
                txtAddress.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string fullName = txtFullName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string email = txtEmail.Text.Trim();
                DateTime? birthday = null;
                if (!string.IsNullOrEmpty(txtBirthday.Text))
                {
                    birthday = Convert.ToDateTime(txtBirthday.Text);
                }
                string gender = ddlGender.SelectedValue;
                string address = txtAddress.Text.Trim();
                string password = txtPassword.Text;

                try
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();

                        // Kiểm tra email và số điện thoại tồn tại
                        string checkExisting = @"SELECT 
                            CASE 
                                WHEN EXISTS(SELECT 1 FROM Users WHERE Email = @Email) THEN 1 
                                WHEN EXISTS(SELECT 1 FROM Users WHERE Phone = @Phone) THEN 2
                                ELSE 0 
                            END";

                        using (MySqlCommand cmd = new MySqlCommand(checkExisting, conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Phone", phone);
                            int result = (int)cmd.ExecuteScalar();

                            if (result == 1)
                            {
                                ShowError("Email đã được sử dụng");
                                return;
                            }
                            else if (result == 2)
                            {
                                ShowError("Số điện thoại đã được sử dụng");
                                return;
                            }
                        }

                        // Thêm user mới với các trường bổ sung
                        string sql = @"INSERT INTO Users (FullName, Email, Phone, Password, Birthday, Gender, Address, CreatedAt, UpdatedAt) 
                                     VALUES (@FullName, @Email, @Phone, @Password, @Birthday, @Gender, @Address, GETDATE(), GETDATE())";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Phone", phone);
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@Birthday", birthday ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Gender", gender);
                            cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);

                            cmd.ExecuteNonQuery();
                        }

                        // Chuyển hướng đến trang đăng nhập
                        Response.Redirect("~/Login.aspx");
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}