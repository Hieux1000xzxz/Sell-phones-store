using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }
        private void LoadCategories()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = "SELECT CategoryID, CategoryName, Description FROM Categories ORDER BY CategoryName";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    gvCategories.DataSource = dt;
                    gvCategories.DataBind();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCategoryName.Text.Trim();
                string desc = txtDescription.Text.Trim();

                if (string.IsNullOrEmpty(name))
                {
                    ShowMessage("Vui lòng nhập tên danh mục!");
                    return;
                }

                using (MySqlConnection conn = Database.GetConnection())
                {
                    string sql = "INSERT INTO Categories (CategoryName, Description) VALUES (@Name, @Desc)";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Desc", desc);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                txtCategoryName.Text = "";
                txtDescription.Text = "";
                LoadCategories();
                ShowMessage("Thêm danh mục thành công!");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message);
            }
        }

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            LoadCategories();
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            LoadCategories();
        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvCategories.Rows[e.RowIndex];
                int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                string name = (row.FindControl("txtEditName") as TextBox).Text.Trim();
                string desc = (row.FindControl("txtEditDesc") as TextBox).Text.Trim();

                using (MySqlConnection conn = Database.GetConnection())
                {
                    string sql = "UPDATE Categories SET CategoryName = @Name, Description = @Desc WHERE CategoryID = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", categoryId);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Desc", desc);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                gvCategories.EditIndex = -1;
                LoadCategories();
                ShowMessage("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message);
            }
        }

        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                using (MySqlConnection conn = Database.GetConnection())
                {
                    string sql = "DELETE FROM Categories WHERE CategoryID = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", categoryId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadCategories();
                ShowMessage("Xóa danh mục thành công!");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message);
            }
        }

        private void ShowMessage(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }
    }
}