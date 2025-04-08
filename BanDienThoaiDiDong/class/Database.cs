using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.UI;

namespace BanDienThoaiDiDong
{
    public static class Database
    {

        public static int slitems;
        public static MySqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BanDienThoaiDiDong"].ConnectionString;
            return new MySqlConnection(connectionString);
        }
        public static List<Product> SearchProducts(string searchTerm,
        string sortBy = "relevance",
        string priceRange = "all")
        {
            var searchResults = new List<Product>();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    string query = @"
                        SELECT p.ProductId, p.ProductName, p.OriginalPrice, p.SalePrice, 
                               ISNULL(p.DefaultImageUrl, '') as DefaultImageUrl
                        FROM Products p 
                        WHERE p.ProductName LIKE @searchTerm
                        AND p.IsActive = 1";

                    // Điều kiện lọc theo giá
                    switch (priceRange)
                    {
                        case "0-5000000":
                            query += " AND p.SalePrice < 5000000";
                            break;
                        case "5000000-10000000":
                            query += " AND p.SalePrice BETWEEN 5000000 AND 10000000";
                            break;
                        case "10000000-20000000":
                            query += " AND p.SalePrice BETWEEN 10000000 AND 20000000";
                            break;
                        case "20000000":
                            query += " AND p.SalePrice > 20000000";
                            break;
                    }

                    // Điều kiện sắp xếp
                    switch (sortBy)
                    {
                        case "price-asc":
                            query += " ORDER BY p.SalePrice ASC";
                            break;
                        case "price-desc":
                            query += " ORDER BY p.SalePrice DESC";
                            break;
                        case "name-asc":
                            query += " ORDER BY p.ProductName ASC";
                            break;
                        case "name-desc":
                            query += " ORDER BY p.ProductName DESC";
                            break;
                        case "relevance":
                        default:
                            query += " ORDER BY CASE WHEN p.ProductName LIKE @exactTerm THEN 0 ELSE 1 END, p.ProductName";
                            break;
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    cmd.Parameters.AddWithValue("@exactTerm", searchTerm + "%");

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            var product = new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("DefaultImageUrl")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),
                                SalePrice = reader.GetDecimal(reader.GetOrdinal("SalePrice")),
                            };

                            searchResults.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi hoặc xử lý ngoại lệ
                    throw new InvalidOperationException("Lỗi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
            }

            return searchResults;
        }
        public static List<Product> GetProductsByQuery(string query)
        {
            var products = new List<Product>();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("DefaultImageUrl")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),
                                SalePrice = reader.GetDecimal(reader.GetOrdinal("SalePrice")),
                            };

                            products.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log hoặc xử lý ngoại lệ
                    throw new InvalidOperationException("Lỗi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
            }

            return products;
        }
        public static List<Product> GetProductsByCategory(string categoryId,
       string sortBy = "relevance",
       string priceRange = "all")
        {
            var searchResults = new List<Product>();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    string query = @"
                SELECT p.ProductId, p.ProductName, p.OriginalPrice, p.SalePrice, 
                       ISNULL(p.DefaultImageUrl, '') as DefaultImageUrl
                FROM Products p
                WHERE p.CategoryID = @CategoryID
                AND p.IsActive = 1";

                    // Điều kiện lọc theo giá
                    switch (priceRange)
                    {
                        case "0-5000000":
                            query += " AND p.SalePrice < 5000000";
                            break;
                        case "5000000-10000000":
                            query += " AND p.SalePrice BETWEEN 5000000 AND 10000000";
                            break;
                        case "10000000-20000000":
                            query += " AND p.SalePrice BETWEEN 10000000 AND 20000000";
                            break;
                        case "20000000":
                            query += " AND p.SalePrice > 20000000";
                            break;
                    }

                    // Điều kiện sắp xếp
                    switch (sortBy)
                    {
                        case "price-asc":
                            query += " ORDER BY p.SalePrice ASC";
                            break;
                        case "price-desc":
                            query += " ORDER BY p.SalePrice DESC";
                            break;
                        case "name-asc":
                            query += " ORDER BY p.ProductName ASC";
                            break;
                        case "name-desc":
                            query += " ORDER BY p.ProductName DESC";
                            break;
                        default:
                            query += " ORDER BY p.ProductName";
                            break;
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("DefaultImageUrl")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),
                                SalePrice = reader.GetDecimal(reader.GetOrdinal("SalePrice")),
                            };

                            searchResults.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi hoặc xử lý ngoại lệ
                    throw new InvalidOperationException("Lỗi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
            }

            return searchResults;
        }
        public static void LoadCartItemsCount(Page page)
        {
            if (page.Session["UserID"] != null)
            {
                Database.slitems = GetCartItemsCount(page);
                var masterPage = page.Master as Site1;
                if (masterPage != null)
                {
                    masterPage.UpdateCartCount();
                }
            }
            else
            {
                Database.slitems = 0;
            }
        }

        private static int GetCartItemsCount(Page page)
        {
            if (page.Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(page.Session["UserID"]);
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT SUM(Quantity) AS TotalQuantity
                        FROM CartItems
                        WHERE UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        var result = cmd.ExecuteScalar();
                        return result == DBNull.Value ? 0 : Convert.ToInt32(result);
                    }
                }
            }
            else
            {
                Database.slitems = 0;
                return Database.slitems;
            }
        }
    }
}
