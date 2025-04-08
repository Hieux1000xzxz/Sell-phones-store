using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class ProductManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
            }
        }

        private void LoadCategories()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlCategory.Items.Clear();
                        ddlCategory.Items.Add(new ListItem("Tất cả danh mục", ""));
                        while (reader.Read())
                        {
                            ddlCategory.Items.Add(new ListItem(
                                reader["CategoryName"].ToString(),
                                reader["CategoryID"].ToString()
                            ));
                        }
                    }
                }
            }
        }

        private void LoadProducts()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT p.*, c.CategoryName,
                        (SELECT IFNULL(SUM(pv.Stock), 0) FROM ProductVariants pv WHERE pv.ProductID = p.ProductID) as Stock,
                        (SELECT IFNULL(SUM(pv.Sold), 0) FROM ProductVariants pv WHERE pv.ProductID = p.ProductID) as Sold
                        FROM Products p
                        LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                        WHERE (@CategoryID = '' OR p.CategoryID = @CategoryID)
                        AND (@Status = '' OR p.IsActive = @Status)
                        AND (@Search = '' OR p.ProductName LIKE CONCAT('%', @Search, '%'))
                        ORDER BY p.ProductID DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvProducts.DataSource = dt;
                        gvProducts.DataBind();
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "EditProduct":
                    Response.Redirect($"~/Admin/ProductEdit.aspx?id={productId}");
                    break;

                case "DeleteProduct":
                    DeleteProduct(productId);
                    break;
            }
        }

        private void DeleteProduct(int productId)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "UPDATE Products SET IsActive = 0 WHERE ProductID = @ProductID";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadProducts();
                ShowMessage("Xóa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        private void ShowMessage(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
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
                            string categoryId;
                            using (MySqlCommand cmdCategory = new MySqlCommand(
                                "SELECT CategoryID FROM Categories WHERE IsActive = 1 ORDER BY RAND() LIMIT 1",
                                conn,
                                transaction))
                            {
                                categoryId = cmdCategory.ExecuteScalar()?.ToString();
                            }

                            string sql = @"INSERT INTO Products 
                        (CategoryID, ProductName, Description, OriginalPrice, 
                         SalePrice, Stock, Sold, IsActive, DefaultImageUrl, CreatedAt, UpdatedAt)
                        VALUES 
                        (@CategoryID, 'Sản phẩm mới', 'Mô tả sản phẩm', 1000000, 
                         1000000, 1, 0, 1, '~/assets/images/default-product.png', NOW(), NOW())";

                            long newProductId;
                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                                cmd.ExecuteNonQuery();
                                newProductId = cmd.LastInsertedId;
                            }

                            sql = @"INSERT INTO ProductVariants 
                        (ProductID, Color, ColorCode, Storage, Price, Stock, Sold, VarImageUrl, IsActive)
                        VALUES 
                        (@ProductID, 'Mặc định', '#000000', 'Mặc định', 1000000, 1, 0, '~/assets/images/default-product.png', 1)";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", newProductId);
                                cmd.ExecuteNonQuery();
                            }

                            sql = @"INSERT INTO ProductSpecs 
                        (ProductID, SpecName, SpecValue)
                        VALUES 
                        (@ProductID, 'Màn hình', 'Chưa cập nhật'),
                        (@ProductID, 'CPU', 'Chưa cập nhật'),
                        (@ProductID, 'RAM', 'Chưa cập nhật'),
                        (@ProductID, 'Bộ nhớ trong', 'Chưa cập nhật')";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", newProductId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            LoadProducts();
                            Response.Redirect($"~/Admin/ProductEdit.aspx?id={newProductId}");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ShowMessage("Lỗi khi tạo sản phẩm mới: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}