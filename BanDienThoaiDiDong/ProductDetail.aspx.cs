using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        private static Product CurrentProduct;
        private static string CurrentStorage;
        private static string CurrentColor;
        private int CurrentQuantity = 1;
        private static int CurrentVariantID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string productId = Request.QueryString["id"];
                if (string.IsNullOrEmpty(productId))
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                LoadProductData(productId);
                if (CurrentProduct != null)
                {
                    BindProductData();
                    Database.LoadCartItemsCount(this);
                    CurrentQuantity = 1;
                    txtQuantity.Text = "1";
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                if (int.TryParse(txtQuantity.Text, out int quantity))
                {
                    CurrentQuantity = quantity;
                }
            }
        }

        private void LoadProductData(string productId)
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    CurrentProduct = new Product();

                    // Load thông tin cơ bản, variant đầu tiên và thông số kỹ thuật
                    string sql = @"
                        WITH FirstVariant AS (
                            SELECT TOP 1 v.Storage, v.Color, v.Price, v.Stock, v.VarImageUrl, v.VariantID
                            FROM ProductVariants v 
                            WHERE v.ProductID = @ProductID AND v.Stock > 0
                            ORDER BY v.Storage, v.Color
                        )
                        SELECT p.*, c.CategoryName as Brand,
                               v.Storage, v.Color, v.Price as VariantPrice, 
                               v.VarImageUrl, v.VariantID
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                        LEFT JOIN FirstVariant v ON 1=1
                        WHERE p.ProductID = @ProductID AND p.IsActive = 1;

                        SELECT DISTINCT Storage, Color, ColorCode
                        FROM ProductVariants 
                        WHERE ProductID = @ProductID AND Stock > 0
                        ORDER BY Storage, Color;

                        SELECT SpecName, SpecValue 
                        FROM ProductSpecs 
                        WHERE ProductID = @ProductID;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Đọc thông tin cơ bản và variant đầu tiên
                            if (!reader.Read()) return;

                            LoadBasicInfo(reader);

                            // Đọc variants
                            reader.NextResult();
                            LoadVariants(reader);

                            // Đọc thông số kỹ thuật
                            reader.NextResult();
                            LoadSpecifications(reader);
                        }
                    }
                }
            }
            catch (Exception)
            {
                CurrentProduct = null;
            }
        }

        private void LoadBasicInfo(MySqlDataReader reader)
        {
            CurrentProduct.ProductId = (int)reader["ProductID"];
            CurrentProduct.ProductName = reader["ProductName"].ToString();
            CurrentProduct.Brand = reader["Brand"].ToString();
            CurrentProduct.Description = reader["Description"].ToString();
            CurrentProduct.OriginalPrice = (decimal)reader["OriginalPrice"];
            CurrentProduct.ImageUrl = reader["DefaultImageUrl"].ToString();

            // Lưu thông tin variant đầu tiên
            CurrentStorage = reader["Storage"].ToString();
            CurrentColor = reader["Color"].ToString();
            CurrentVariantID = reader.GetInt32(reader.GetOrdinal("VariantID"));
            CurrentProduct.SalePrice = reader.GetDecimal(reader.GetOrdinal("VariantPrice"));

            // Set ảnh mặc định từ variant
            string variantImage = reader["VarImageUrl"].ToString();
            imgMainProduct.ImageUrl = !string.IsNullOrEmpty(variantImage) ? variantImage : CurrentProduct.ImageUrl;
        }

        private void LoadVariants(MySqlDataReader reader)
        {
            CurrentProduct.StorageOptions = new List<ProductVariant>();
            CurrentProduct.ColorOptions = new List<ProductVariant>();
            HashSet<string> storages = new HashSet<string>();
            HashSet<string> colors = new HashSet<string>();

            while (reader.Read())
            {
                string storage = reader["Storage"].ToString();
                string color = reader["Color"].ToString();

                if (!storages.Contains(storage))
                {
                    storages.Add(storage);
                    CurrentProduct.StorageOptions.Add(new ProductVariant
                    {
                        Value = storage,
                        Text = storage + "GB",
                        IsSelected = storage == CurrentStorage
                    });
                }

                if (!colors.Contains(color))
                {
                    colors.Add(color);
                    CurrentProduct.ColorOptions.Add(new ProductVariant
                    {
                        Value = color,
                        Text = color,
                        ColorCode = reader["ColorCode"].ToString(),
                        IsSelected = color == CurrentColor
                    });
                }
            }
        }

        private void LoadSpecifications(MySqlDataReader reader)
        {
            CurrentProduct.Specifications = new List<ProductSpec>();
            while (reader.Read())
            {
                CurrentProduct.Specifications.Add(new ProductSpec
                {
                    SpecName = reader["SpecName"].ToString(),
                    SpecValue = reader["SpecValue"].ToString()
                });
            }
        }

        private void BindProductData()
        {
            lblProductName.Text = lblTitle.Text = CurrentProduct.ProductName;
            lblBrand.Text = CurrentProduct.Brand;
            lblCurrentPrice.Text = $"{CurrentProduct.SalePrice:N0}₫";
            lblOriginalPrice.Text = $"{CurrentProduct.OriginalPrice:N0}₫";

            if (CurrentProduct.OriginalPrice > CurrentProduct.SalePrice)
            {
                decimal discount = ((CurrentProduct.OriginalPrice - CurrentProduct.SalePrice) * 100) / CurrentProduct.OriginalPrice;
                lblDiscount.Text = $"-{Math.Round(discount)}%";
            }

            rptStorage.DataSource = CurrentProduct.StorageOptions;
            rptColors.DataSource = CurrentProduct.ColorOptions;
            rptStorage.DataBind();
            rptColors.DataBind();

            litDescription.Text = CurrentProduct.Description;
            gvSpecs.DataSource = CurrentProduct.Specifications;
            gvSpecs.DataBind();
        }

        protected void StorageButton_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            CurrentStorage = btn.CommandArgument;

            foreach (var option in CurrentProduct.StorageOptions)
            {
                option.IsSelected = option.Value == CurrentStorage;
            }

            UpdateVariantInfo();
            rptStorage.DataSource = CurrentProduct.StorageOptions;
            rptStorage.DataBind();
        }

        protected void ColorButton_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            CurrentColor = btn.CommandArgument;

            foreach (var option in CurrentProduct.ColorOptions)
            {
                option.IsSelected = option.Value == CurrentColor;
            }

            UpdateVariantInfo();
            rptColors.DataSource = CurrentProduct.ColorOptions;
            rptColors.DataBind();
        }

        private void UpdateVariantInfo()
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT v.VariantID, v.Price, v.Stock, v.VarImageUrl
                        FROM ProductVariants v
                        WHERE v.ProductID = @ProductID 
                        AND v.Storage = @Storage 
                        AND v.Color = @Color";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", CurrentProduct.ProductId);
                        cmd.Parameters.AddWithValue("@Storage", CurrentStorage);
                        cmd.Parameters.AddWithValue("@Color", CurrentColor);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CurrentVariantID = reader.GetInt32(0);
                                CurrentProduct.SalePrice = reader.GetDecimal(1);
                                lblCurrentPrice.Text = $"{CurrentProduct.SalePrice:N0}₫";

                                string variantImage = reader["VarImageUrl"].ToString();
                                imgMainProduct.ImageUrl = !string.IsNullOrEmpty(variantImage) 
                                    ? variantImage 
                                    : CurrentProduct.ImageUrl;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        protected void QuantityButton_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            int quantity = CurrentQuantity;

            if (btn.CommandName == "increase" && quantity < 99)
            {
                quantity++;
            }
            else if (btn.CommandName == "decrease" && quantity > 1)
            {
                quantity--;
            }

            UpdateQuantity(quantity);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int quantity))
            {
                if (quantity < 1) quantity = 1;
                if (quantity > 99) quantity = 99;
                UpdateQuantity(quantity);
            }
            else
            {
                UpdateQuantity(1);
            }
        }

        private void UpdateQuantity(int quantity)
        {
            CurrentQuantity = quantity;
            txtQuantity.Text = quantity.ToString();
        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Session["ReturnUrl"] = Request.Url.ToString();
                Response.Redirect("~/Login.aspx");
                return;
            }

            AddToCart(CurrentProduct.ProductId, CurrentVariantID, CurrentQuantity);
            Database.LoadCartItemsCount(this);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "showAddToCartAlert();", true);
        }

        protected void BuyNow_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Session["ReturnUrl"] = Request.Url.ToString();
                Response.Redirect("~/Login.aspx");
                return;
            }

            var singleItemCart = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = CurrentProduct.ProductId,
                    ProductName = CurrentProduct.ProductName,
                    ImageUrl = imgMainProduct.ImageUrl,
                    Variant = $"{CurrentStorage}GB - {CurrentColor}",
                    Quantity = CurrentQuantity,
                    CurrentPrice = CurrentProduct.SalePrice,
                    OriginalPrice = CurrentProduct.OriginalPrice,
                    VariantId = CurrentVariantID
                }
            };

            Session["BuyNowCart"] = singleItemCart;
            Response.Redirect("~/Checkout.aspx?mode=buynow");
        }

        private void AddToCart(int productId, int variantId, int quantity)
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"
                    MERGE CartItems AS target
                    USING (VALUES (@UserID, @ProductID, @VariantID, @Quantity)) 
                        AS source (UserID, ProductID, VariantID, Quantity)
                    ON target.UserID = source.UserID 
                        AND target.ProductID = source.ProductID
                        AND target.VariantID = source.VariantID
                    WHEN MATCHED THEN
                        UPDATE SET Quantity = target.Quantity + source.Quantity
                    WHEN NOT MATCHED THEN
                        INSERT (UserID, ProductID, VariantID, Quantity)
                        VALUES (source.UserID, source.ProductID, source.VariantID, source.Quantity);";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@VariantID", variantId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
