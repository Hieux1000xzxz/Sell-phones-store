using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class ProductEdit : System.Web.UI.Page
    {
        private int? productId = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int pid))
            {
                productId = pid;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                if (productId.HasValue)
                {
                    LoadProductData();
                    ltTitle.Text = "Chỉnh sửa sản phẩm";
                }
                else
                {
                    ltTitle.Text = "Thêm sản phẩm mới";
                    // Khởi tạo DataTable trống cho variants khi thêm mới
                    DataTable dt = new DataTable();
                    dt.Columns.Add("VariantID", typeof(int));
                    dt.Columns.Add("Color", typeof(string));
                    dt.Columns.Add("ColorCode", typeof(string));
                    dt.Columns.Add("Storage", typeof(string));
                    dt.Columns.Add("Price", typeof(decimal));
                    dt.Columns.Add("Stock", typeof(int));
                    dt.Columns.Add("VarImageUrl", typeof(string));
                    dt.Rows.Add(0, "", "#000000", "", 0, 0, "");
                    rptVariants.DataSource = dt;
                    rptVariants.DataBind();
                }
            }
        }

        private void LoadCategories()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlCategory.Items.Clear();
                        ddlCategory.Items.Add(new ListItem("-- Chọn danh mục --", ""));
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

        private void LoadProductData()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                // Load product info
                string sql = "SELECT * FROM Products WHERE ProductID = @ProductID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtProductName.Text = reader["ProductName"].ToString();
                            ddlCategory.SelectedValue = reader["CategoryID"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            txtOriginalPrice.Text = reader["OriginalPrice"].ToString();
                            txtSalePrice.Text = reader["SalePrice"].ToString();
                            txtStock.Text = reader["Stock"].ToString();
                            chkIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);

                            if (!string.IsNullOrEmpty(reader["DefaultImageUrl"].ToString()))
                            {
                                imgMain.ImageUrl = reader["DefaultImageUrl"].ToString();
                                imgMain.Visible = true;
                            }
                        }
                    }
                }

                // Load specs
                sql = "SELECT * FROM ProductSpecs WHERE ProductID = @ProductID";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    DataTable dtSpecs = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dtSpecs);
                    }
                    if (dtSpecs.Rows.Count == 0)
                    {
                        dtSpecs.Rows.Add(dtSpecs.NewRow());
                    }
                    rptSpecs.DataSource = dtSpecs;
                    rptSpecs.DataBind();
                }

                // Load variants - Chỉ load các variant có IsActive = 1
                if (productId.HasValue)
                {
                    string variantsSql = @"SELECT * FROM ProductVariants 
                                         WHERE ProductID = @ProductID 
                                         AND IsActive = 1 
                                         ORDER BY Storage, Color";
                    using (MySqlCommand cmd = new MySqlCommand(variantsSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            rptVariants.DataSource = reader;
                            rptVariants.DataBind();
                        }
                    }
                }
            }
        }

        private void LoadEmptyRepeaters()
        {
            // Khởi tạo repeater specs với 1 item trống
            DataTable dtSpecs = new DataTable();
            dtSpecs.Columns.Add("SpecID", typeof(int));
            dtSpecs.Columns.Add("SpecName", typeof(string));
            dtSpecs.Columns.Add("SpecValue", typeof(string));
            dtSpecs.Rows.Add(DBNull.Value, "", "");
            rptSpecs.DataSource = dtSpecs;
            rptSpecs.DataBind();

            // Khởi tạo repeater variants với 1 item trống
            DataTable dtVariants = new DataTable();
            dtVariants.Columns.Add("VariantID", typeof(int));
            dtVariants.Columns.Add("Color", typeof(string));
            dtVariants.Columns.Add("Storage", typeof(string));
            dtVariants.Columns.Add("Price", typeof(decimal));
            dtVariants.Columns.Add("Stock", typeof(int));
            dtVariants.Columns.Add("ImageUrl", typeof(string));
            dtVariants.Rows.Add(DBNull.Value, "", "", 0, 0, "");
            rptVariants.DataSource = dtVariants;
            rptVariants.DataBind();
        }

        protected void btnAddSpec_Click(object sender, EventArgs e)
        {
            LoadEmptyRepeaters();
            DataTable dt = new DataTable();
            dt.Columns.Add("SpecID", typeof(int));
            dt.Columns.Add("SpecName", typeof(string));
            dt.Columns.Add("SpecValue", typeof(string));

            foreach (RepeaterItem item in rptSpecs.Items)
            {
                TextBox txtSpecName = (TextBox)item.FindControl("txtSpecName");
                TextBox txtSpecValue = (TextBox)item.FindControl("txtSpecValue");
                dt.Rows.Add(DBNull.Value, txtSpecName.Text, txtSpecValue.Text);
            }

            dt.Rows.Add(DBNull.Value, "", "");
            rptSpecs.DataSource = dt;
            rptSpecs.DataBind();
        }

        protected void btnRemoveSpec_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int specId;

            if (!int.TryParse(btn.CommandArgument, out specId))
            {
                ShowError("Không tìm thấy thông số cần xóa!");
                return;
            }

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "DELETE FROM ProductSpecs WHERE SpecID = @SpecID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@SpecID", specId);
                        cmd.ExecuteNonQuery();
                    }

                    ShowSuccess("Đã xóa thông số thành công!");
                    LoadProductData(); // Tải lại dữ liệu
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi xóa thông số: " + ex.Message);
            }
        }

        protected void btnAddVariant_Click(object sender, EventArgs e)
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
                            string sql = @"INSERT INTO ProductVariants 
                                (ProductID, Color, ColorCode, Storage, Price, Stock, IsActive) 
                                VALUES 
                                (@ProductID, @Color, @ColorCode, @Storage, @Price, @Stock, 1)";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", productId);
                                cmd.Parameters.AddWithValue("@Color", "Màu mới");
                                cmd.Parameters.AddWithValue("@ColorCode", "#000000");
                                cmd.Parameters.AddWithValue("@Storage", "Dung lượng mới");
                                cmd.Parameters.AddWithValue("@Price", 0);
                                cmd.Parameters.AddWithValue("@Stock", 0);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            LoadProductData(); // Tải lại dữ liệu để hiển thị variant mới
                            ShowSuccess("Đã thêm biến thể mới!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ShowError("Lỗi khi thêm biến thể: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi: " + ex.Message);
            }
        }

        protected void btnRemoveVariant_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.CommandArgument);

            DataTable dt = new DataTable();
            dt.Columns.Add("VariantID", typeof(int));
            dt.Columns.Add("Color", typeof(string));
            dt.Columns.Add("Storage", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("VarImageUrl", typeof(string));
            dt.Columns.Add("ColorCode", typeof(string));
            int currentIndex = 0;
            foreach (RepeaterItem item in rptVariants.Items)
            {
                if (currentIndex != index)
                {
                    TextBox txtColor = (TextBox)item.FindControl("txtColor");
                    TextBox txtStorage = (TextBox)item.FindControl("txtStorage");
                    TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
                    TextBox txtVariantStock = (TextBox)item.FindControl("txtVariantStock");
                    Image imgVariant = (Image)item.FindControl("imgVariant");

                    dt.Rows.Add(
                        DBNull.Value,
                        txtColor.Text,
                        txtStorage.Text,
                        Convert.ToDecimal(string.IsNullOrEmpty(txtPrice.Text) ? "0" : txtPrice.Text),
                        Convert.ToInt32(string.IsNullOrEmpty(txtVariantStock.Text) ? "0" : txtVariantStock.Text),
                        imgVariant.Visible ? imgVariant.ImageUrl : ""
                    );
                }
                currentIndex++;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(DBNull.Value, "", "", 0, 0, "");
            }

            rptVariants.DataSource = dt;
            rptVariants.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
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
                            string imageUrl = null;
                            if (fuMainImage.HasFile)
                            {
                                imageUrl = UploadImage(fuMainImage);
                                if (imageUrl == null) return;
                            }
                            // Thêm/Cập nhật sản phẩm
                            string sql;
                            if (!productId.HasValue)
                            {
                                sql = @"INSERT INTO Products (CategoryID, ProductName, Description, 
                                        OriginalPrice, SalePrice, DefaultImageUrl, Stock, IsActive, CreatedAt, UpdatedAt)
                                        VALUES (@CategoryID, @ProductName, @Description, @OriginalPrice,
                                        @SalePrice, @DefaultImageUrl, 0, @IsActive, GETDATE(), GETDATE());
                                        SELECT SCOPE_IDENTITY();";
                            }
                            else
                            {
                                sql = @"UPDATE Products SET 
                                        CategoryID = @CategoryID,
                                        ProductName = @ProductName,
                                        Description = @Description,
                                        OriginalPrice = @OriginalPrice,
                                        SalePrice = @SalePrice,
                                        DefaultImageUrl = ISNULL(@DefaultImageUrl, DefaultImageUrl),
                                        Stock = @Stock,
                                        IsActive = @IsActive,
                                        UpdatedAt = GETDATE()
                                        WHERE ProductID = @ProductID;
                                        SELECT @ProductID;";
                            }

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                                cmd.Parameters.AddWithValue("@OriginalPrice", Convert.ToDecimal(txtOriginalPrice.Text));
                                cmd.Parameters.AddWithValue("@SalePrice", Convert.ToDecimal(txtSalePrice.Text));
                                cmd.Parameters.AddWithValue("@DefaultImageUrl", (object)imageUrl ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Stock", Convert.ToInt32(txtStock.Text));
                                cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                                if (productId.HasValue)
                                {
                                    cmd.Parameters.AddWithValue("@ProductID", productId.Value);
                                }

                                productId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // Lưu thông số kỹ thuật
                            sql = "DELETE FROM ProductSpecs WHERE ProductID = @ProductID";
                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", productId);
                                cmd.ExecuteNonQuery();
                            }

                            foreach (RepeaterItem item in rptSpecs.Items)
                            {
                                TextBox txtSpecName = (TextBox)item.FindControl("txtSpecName");
                                TextBox txtSpecValue = (TextBox)item.FindControl("txtSpecValue");

                                if (!string.IsNullOrEmpty(txtSpecName.Text) && !string.IsNullOrEmpty(txtSpecValue.Text))
                                {
                                    sql = "INSERT INTO ProductSpecs (ProductID, SpecName, SpecValue) VALUES (@ProductID, @SpecName, @SpecValue)";
                                    using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@ProductID", productId);
                                        cmd.Parameters.AddWithValue("@SpecName", txtSpecName.Text.Trim());
                                        cmd.Parameters.AddWithValue("@SpecValue", txtSpecValue.Text.Trim());
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            // Lưu biến thể sản phẩm


                            // Xử lý từng variant trong form
                            foreach (RepeaterItem item in rptVariants.Items)
                            {
                                TextBox txtColorCode = (TextBox)item.FindControl("txtColorCode");
                                TextBox txtColor = (TextBox)item.FindControl("txtColor");
                                TextBox txtStorage = (TextBox)item.FindControl("txtStorage");
                                TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
                                TextBox txtVariantStock = (TextBox)item.FindControl("txtVariantStock");
                                FileUpload fuVariantImage = (FileUpload)item.FindControl("fuVariantImage");
                                HiddenField hdnVariantId = (HiddenField)item.FindControl("hdnVariantId");

                                if (!string.IsNullOrEmpty(txtColor.Text) && !string.IsNullOrEmpty(txtStorage.Text))
                                {
                                    string variantImageUrl = null;
                                    if (fuVariantImage.HasFile)
                                    {
                                        variantImageUrl = UploadImage(fuVariantImage);
                                        if (variantImageUrl == null) return;
                                    }

                                    int variantId;
                                    if (int.TryParse(hdnVariantId?.Value, out variantId) && variantId > 0)
                                    {
                                        // Update existing variant
                                        sql = @"UPDATE ProductVariants 
                   SET ColorCode = @ColorCode,
                       Color = @Color,
                       Storage = @Storage,
                       Price = @Price,
                       Stock = @Stock";

                                        if (variantImageUrl != null)
                                        {
                                            sql += ", VarImageUrl = @ImageUrl";
                                        }

                                        sql += " WHERE VariantID = @VariantID";

                                        using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@VariantID", variantId);
                                            cmd.Parameters.AddWithValue("@ColorCode", txtColorCode.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Color", txtColor.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Storage", txtStorage.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                                            cmd.Parameters.AddWithValue("@Stock", Convert.ToInt32(txtVariantStock.Text));
                                            if (variantImageUrl != null)
                                            {
                                                cmd.Parameters.AddWithValue("@ImageUrl", variantImageUrl);
                                            }
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // Insert new variant
                                        sql = @"INSERT INTO ProductVariants 
                   (ProductID, ColorCode, Color, Storage, Price, Stock, VarImageUrl) 
                   VALUES 
                   (@ProductID, @ColorCode, @Color, @Storage, @Price, @Stock, @ImageUrl)";

                                        using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@ProductID", productId);
                                            cmd.Parameters.AddWithValue("@ColorCode", txtColorCode.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Color", txtColor.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Storage", txtStorage.Text.Trim());
                                            cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                                            cmd.Parameters.AddWithValue("@Stock", Convert.ToInt32(txtVariantStock.Text));
                                            cmd.Parameters.AddWithValue("@ImageUrl", (object)variantImageUrl ?? DBNull.Value);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            // Cập nhật tổng stock
                            sql = @"UPDATE Products 
        SET Stock = (SELECT ISNULL(SUM(Stock), 0) FROM ProductVariants WHERE ProductID = @ProductID)
        WHERE ProductID = @ProductID";
                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", productId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            Response.Redirect("~/Admin/ProductManagement.aspx");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ShowError("Lỗi khi lưu sản phẩm: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi kết nối database: " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ProductManagement.aspx");
        }

        private string UploadImage(FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(fileName);

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        ShowError("Chỉ chấp nhận file ảnh có định dạng: " + string.Join(", ", allowedExtensions));
                        return null;
                    }

                    if (fileUpload.FileBytes.Length > 5 * 1024 * 1024)
                    {
                        ShowError("Kích thước file không được vượt quá 5MB");
                        return null;
                    }

                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string uploadPath = Path.Combine(Server.MapPath("~"), "assets", "images");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string fullPath = Path.Combine(uploadPath, newFileName);
                    fileUpload.SaveAs(fullPath);
                    return "/assets/images/" + newFileName;
                }
                return null;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải lên hình ảnh: " + ex.Message);
                return null;
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        protected void btnSaveVariant_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            int variantId = Convert.ToInt32(btn.CommandArgument);

            TextBox txtColorCode = (TextBox)item.FindControl("txtColorCode");
            TextBox txtColor = (TextBox)item.FindControl("txtColor");
            TextBox txtStorage = (TextBox)item.FindControl("txtStorage");
            TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
            TextBox txtVariantStock = (TextBox)item.FindControl("txtVariantStock");
            FileUpload fuVariantImage = (FileUpload)item.FindControl("fuVariantImage");

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string variantImageUrl = null;
                            if (fuVariantImage.HasFile)
                            {
                                variantImageUrl = UploadImage(fuVariantImage);
                                if (variantImageUrl == null) return;
                            }

                            string sql = @"UPDATE ProductVariants 
                                 SET ColorCode = @ColorCode,
                                     Color = @Color,
                                     Storage = @Storage,
                                     Price = @Price,
                                     Stock = @Stock";

                            if (variantImageUrl != null)
                            {
                                sql += ", VarImageUrl = @ImageUrl";
                            }

                            sql += " WHERE VariantID = @VariantID";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@VariantID", variantId);
                                cmd.Parameters.AddWithValue("@ColorCode", txtColorCode.Text.Trim());
                                cmd.Parameters.AddWithValue("@Color", txtColor.Text.Trim());
                                cmd.Parameters.AddWithValue("@Storage", txtStorage.Text.Trim());
                                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                                cmd.Parameters.AddWithValue("@Stock", Convert.ToInt32(txtVariantStock.Text));
                                if (variantImageUrl != null)
                                {
                                    cmd.Parameters.AddWithValue("@ImageUrl", variantImageUrl);
                                }
                                cmd.ExecuteNonQuery();
                            }

                            // Cập nhật tổng stock
                            UpdateProductStock(conn, transaction);

                            transaction.Commit();
                            ShowSuccess("Đã cập nhật biến thể thành công!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ShowError("Lỗi khi cập nhật biến thể: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi: " + ex.Message);
            }
        }

        protected void btnDeleteVariant_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int variantId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Cập nhật IsActive = 0 và Stock = 0
                            string sql = @"UPDATE ProductVariants 
                                 SET IsActive = 0, Stock = 0 
                                 WHERE VariantID = @VariantID";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@VariantID", variantId);
                                cmd.ExecuteNonQuery();
                            }

                            // Trigger sẽ tự động cập nhật Stock của Products

                            transaction.Commit();

                            // Ẩn variant trên giao diện bằng JavaScript
                            string script = $"document.getElementById('variant-{variantId}').style.display = 'none';";
                            ScriptManager.RegisterStartupScript(this, GetType(), $"hideVariant_{variantId}", script, true);

                            ShowSuccess("Đã ẩn bi���n thể thành công!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ShowError("Lỗi khi ẩn biến thể: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi: " + ex.Message);
            }
        }

        private void UpdateProductStock(MySqlConnection conn, MySqlTransaction transaction)
        {
            string sql = @"UPDATE Products 
                  SET Stock = (SELECT ISNULL(SUM(Stock), 0) 
                             FROM ProductVariants 
                             WHERE ProductID = @ProductID AND IsActive = 1)
                  WHERE ProductID = @ProductID";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.ExecuteNonQuery();
            }
        }

        private void ShowSuccess(string message)
        {
            lblError.Text = message;
            lblError.CssClass = "success-message";
            lblError.Visible = true;
        }

        protected void btnSaveBasic_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string imageUrl = null;
                    if (fuMainImage.HasFile)
                    {
                        imageUrl = UploadImage(fuMainImage);
                        if (imageUrl == null) return;
                    }

                    string sql = @"UPDATE Products SET 
                        CategoryID = @CategoryID,
                        ProductName = @ProductName,
                        Description = @Description,
                        OriginalPrice = @OriginalPrice,
                        SalePrice = @SalePrice,
                        DefaultImageUrl = ISNULL(@DefaultImageUrl, DefaultImageUrl),
                        IsActive = @IsActive,
                        UpdatedAt = GETDATE()
                        WHERE ProductID = @ProductID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@OriginalPrice", Convert.ToDecimal(txtOriginalPrice.Text));
                        cmd.Parameters.AddWithValue("@SalePrice", Convert.ToDecimal(txtSalePrice.Text));
                        cmd.Parameters.AddWithValue("@DefaultImageUrl", (object)imageUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                    ShowSuccess("Đã cập nhật thông tin cơ bản!");
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi: " + ex.Message);
            }
        }

        protected void btnSaveSpec_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            int specId;

            // Lấy các control trong repeater item
            HiddenField hdnSpecId = (HiddenField)item.FindControl("hdnSpecId");
            TextBox txtSpecName = (TextBox)item.FindControl("txtSpecName");
            TextBox txtSpecValue = (TextBox)item.FindControl("txtSpecValue");

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtSpecName.Text) || string.IsNullOrWhiteSpace(txtSpecValue.Text))
            {
                ShowError("Vui lòng nhập đầy đủ thông tin thông số!");
                return;
            }

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql;

                    // Nếu có SpecID thì update, không thì insert
                    if (int.TryParse(hdnSpecId.Value, out specId) && specId > 0)
                    {
                        sql = @"UPDATE ProductSpecs 
                               SET SpecName = @SpecName, 
                                   SpecValue = @SpecValue
                               WHERE SpecID = @SpecID";
                    }
                    else
                    {
                        sql = @"INSERT INTO ProductSpecs 
                               (ProductID, SpecName, SpecValue) 
                               VALUES 
                               (@ProductID, @SpecName, @SpecValue)";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (specId > 0)
                        {
                            cmd.Parameters.AddWithValue("@SpecID", specId);
                        }
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@SpecName", txtSpecName.Text.Trim());
                        cmd.Parameters.AddWithValue("@SpecValue", txtSpecValue.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    ShowSuccess("Đã lưu thông số kỹ thuật thành công!");
                    LoadProductData(); // Tải lại dữ liệu để cập nhật ID cho các spec mới
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi lưu thông số: " + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ProductManagement.aspx");
        }
    }
}