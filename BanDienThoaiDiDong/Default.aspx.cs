using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadFeaturedProducts();
                LoadBestSellers();
                LoadNewProducts();
                Database.LoadCartItemsCount(this);
            }
        }
        private void LoadCategories()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    rptCategories.DataSource = cmd.ExecuteReader();
                    rptCategories.DataBind();
                }
            }
        }       
        private void LoadFeaturedProducts()
        {
            string query = @"SELECT 
                                    ProductID,
                                    ProductName,
                                    DefaultImageUrl,
                                    OriginalPrice,
                                    SalePrice
                                FROM Products
                                WHERE IsActive = 1
                                ORDER BY ((OriginalPrice - SalePrice) * 100.0 / OriginalPrice) DESC
                                LIMIT 5";
            rptFeaturedProducts.DataSource = Database.GetProductsByQuery(query);
            rptFeaturedProducts.DataBind();
        }
        private void LoadBestSellers()
        {
            string query = @"SELECT 
                                ProductID,
                                ProductName,
                                DefaultImageUrl,
                                OriginalPrice,
                                SalePrice
                            FROM Products
                            WHERE IsActive = 1
                            ORDER BY Sold DESC
                            LIMIT 5";
            rptBestSellers.DataSource = Database.GetProductsByQuery(query);
            rptBestSellers.DataBind();
        }
        private void LoadNewProducts()
        {
            string query = @"SELECT 
                                    ProductID,
                                    ProductName,
                                    DefaultImageUrl,
                                    OriginalPrice,
                                    SalePrice
                                FROM Products 
                                WHERE IsActive = 1
                                ORDER BY CreatedAt DESC
                                LIMIT 5";
            rptNewProducts.DataSource = Database.GetProductsByQuery(query);
            rptNewProducts.DataBind();
        }
        protected string GetDiscountPercentage(object originalPrice, object currentPrice)
        {
            if (originalPrice == null || currentPrice == null) return "0";
            decimal orig = Convert.ToDecimal(originalPrice);
            decimal curr = Convert.ToDecimal(currentPrice);
            if (orig <= 0) return "0";
            int discount = (int)((1 - curr / orig) * 100);
            return discount.ToString();
        }
    }
}