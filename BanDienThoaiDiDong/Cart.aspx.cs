using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["UserID"] == null)
                {
                    // Nếu chưa đăng nhập, giữ giỏ hàng trong Session
                    LoadCartFromSession();
                }
                else
                {
                    // Nếu đã đăng nhập, tải giỏ hàng từ database

                    LoadCartItemsCount();
                    LoadCartFromDatabase();
                }
            }
        }
        public void LoadCartItemsCount()
        {
            if (Session["UserID"] != null)
            {
                Database.slitems = GetCartItemsCount();
                var masterPage = this.Master as Site1;
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
        public int GetCartItemsCount()
        {
            if (Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
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
        private void LoadCartFromSession()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }
            var cart = Session["Cart"] as List<CartItem>;
            rptCartItems.DataSource = cart;
            rptCartItems.DataBind();
            UpdateCartSummary();
        }

        private void LoadCartFromDatabase()
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return;
                }

                int userId = Convert.ToInt32(Session["UserID"]);
                var cart = new List<CartItem>();

                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT 
                        c.CartItemID,
                        c.ProductID,
                        c.Quantity,
                        c.VariantID,
                        p.ProductName,
                        p.DefaultImageUrl,
                        ISNULL(pv.Price, p.SalePrice) as CurrentPrice,
                        p.OriginalPrice,
                        CONCAT(pv.Storage, 'GB - Màu: ', pv.Color) as Variant
                    FROM CartItems c
                    INNER JOIN Products p ON c.ProductID = p.ProductID
                    LEFT JOIN ProductVariants pv ON c.VariantID = pv.VariantID
                    WHERE c.UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cart.Add(new CartItem
                                {
                                    ProductId = Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    ImageUrl = reader["DefaultImageUrl"].ToString(),
                                    Variant = reader["Variant"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    CurrentPrice = Convert.ToDecimal(reader["CurrentPrice"]),
                                    OriginalPrice = Convert.ToDecimal(reader["OriginalPrice"]),
                                    VariantId = reader["VariantID"] != DBNull.Value ?
                                              Convert.ToInt32(reader["VariantID"]) : (int?)null
                                });
                            }
                        }
                    }
                }
                Session["Cart"] = cart;

                // Bind dữ liệu vào Repeater
                rptCartItems.DataSource = Session["Cart"];
                rptCartItems.DataBind();

                // Cập nhật tổng tiền
                UpdateCartSummary();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                System.Diagnostics.Debug.WriteLine("LoadCartFromDatabase Error: " + ex.Message);
            }
        }

        protected void UpdateQuantity_Command(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            int change = e.CommandName == "IncreaseQuantity" ? 1 : -1;

            if (Session["UserID"] != null)
            {
                // Cập nhật trong database
                UpdateQuantityInDatabase(productId, change);
                LoadCartFromDatabase();
                LoadCartItemsCount();
            }
            else
            {
                // Cập nhật trong session
                var cart = Session["Cart"] as List<CartItem>;
                var item = cart?.FirstOrDefault(x => x.ProductId == productId);
                if (item != null)
                {
                    item.Quantity = Math.Max(1, item.Quantity + change);
                    Session["Cart"] = cart;
                    LoadCartFromSession();
                }
            }

            // Cập nhật số lượng trong giỏ hàng trên master page
            if (Master is Site1 masterPage)
            {
                masterPage.UpdateCartCount();
            }
        }

        private void UpdateQuantityInDatabase(int variantId, int change)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE CartItems 
                             SET Quantity = CASE 
                                WHEN Quantity + @Change < 1 THEN 1 
                                ELSE Quantity + @Change 
                             END,
                             DateAdded = GETDATE()
                             WHERE UserID = @UserID AND VariantID = @VariantID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@VariantID", variantId);
                    cmd.Parameters.AddWithValue("@Change", change);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            TextBox txtQuantity = (TextBox)sender;
            RepeaterItem item = (RepeaterItem)txtQuantity.NamingContainer;
            int productId = Convert.ToInt32(((HiddenField)item.FindControl("hfProductId")).Value);

            if (int.TryParse(txtQuantity.Text, out int newQuantity))
            {
                newQuantity = Math.Max(1, newQuantity);
                if (Session["UserID"] != null)
                {
                    // Cập nhật trong database
                    SetQuantityInDatabase(productId, newQuantity);
                    LoadCartFromDatabase();
                }
                else
                {
                    // Cập nhật trong session
                    var cart = Session["Cart"] as List<CartItem>;
                    var cartItem = cart?.FirstOrDefault(x => x.ProductId == productId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = newQuantity;
                        Session["Cart"] = cart;
                        LoadCartFromSession();
                    }
                }

                // Cập nhật số lượng trong giỏ hàng trên master page
                if (Master is Site1 masterPage)
                {
                    masterPage.UpdateCartCount();
                }
            }
        }

        private void SetQuantityInDatabase(int variantID, int quantity)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE CartItems 
                             SET Quantity = @Quantity,
                             DadeAdded = GETDATE()
                             WHERE UserID = @UserID AND VariantID = @VariantID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@VariantID", variantID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        protected void RemoveItem_Command(object sender, CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            if (Session["UserID"] != null)
            {
                // Xóa từ database
                RemoveItemFromDatabase(productId);
                LoadCartFromDatabase();
                LoadCartItemsCount();
            }
            else
            {
                // Xóa từ session
                var cart = Session["Cart"] as List<CartItem>;
                var item = cart?.FirstOrDefault(x => x.ProductId == productId);
                if (item != null)
                {
                    cart.Remove(item);
                    Session["Cart"] = cart;
                    LoadCartFromSession();
                }
            }

            // Cập nhật số lượng trong giỏ hàng trên master page
            if (Master is Site1 masterPage)
            {
                masterPage.UpdateCartCount();
            }
        }

        private void RemoveItemFromDatabase(int VariantId)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM CartItems WHERE UserID = @UserID AND VariantID = @VariantID";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@VariantID", VariantId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateCartSummary()
        {
            decimal subTotal = 0;
            decimal discount = 0;

            if (Session["UserID"] != null)
            {
                // Tính từ database
                int userId = Convert.ToInt32(Session["UserID"]);
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT SUM(c.Quantity * pv.Price) as SubTotal,
                                 SUM(c.Quantity * (p.OriginalPrice - pv.Price)) as Discount
                                 FROM CartItems c
                                 INNER JOIN Products p ON c.ProductID = p.ProductID
                                 LEFT JOIN ProductVariants pv ON c.VariantID = pv.VariantID
                                 WHERE c.UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subTotal = reader["SubTotal"] != DBNull.Value ? Convert.ToDecimal(reader["SubTotal"]) : 0;
                                discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : 0;
                            }
                        }
                    }
                }
            }
            else
            {
                // Tính từ session
                var cart = Session["Cart"] as List<CartItem>;
                if (cart != null)
                {
                    subTotal = cart.Sum(x => x.CurrentPrice * x.Quantity);
                    discount = cart.Sum(x => (x.OriginalPrice - x.CurrentPrice) * x.Quantity);
                }
            }

            lblSubTotal.Text = String.Format("{0:N0}₫", subTotal);
            lblDiscount.Text = String.Format("-{0:N0}₫", discount);
            lblTotal.Text = String.Format("{0:N0}₫", subTotal);
        }
        public List<CartItem> GetAllCartItems()
        {
            var cartItems = new List<CartItem>();

            if (Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                try
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();
                        string sql = @"
                                        SELECT 
                                            c.CartItemID,
                                            c.ProductID,
                                            c.Quantity,
                                            c.VariantID,
                                            p.ProductName,
                                            p.DefaultImageUrl,
                                            ISNULL(pv.Price, p.SalePrice) as CurrentPrice,
                                            p.OriginalPrice,
                                            p.CategoryID,
                                            CONCAT(pv.Storage, 'GB - Màu: ', pv.Color) as Variant
                                        FROM CartItems c
                                        INNER JOIN Products p ON c.ProductID = p.ProductID
                                        LEFT JOIN ProductVariants pv ON c.VariantID = pv.VariantID
                                        WHERE c.UserID = @UserID
                                        ORDER BY c.DateAdded DESC";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    cartItems.Add(new CartItem
                                    {
                                        ProductId = Convert.ToInt32(reader["ProductID"]),
                                        ProductName = reader["ProductName"].ToString(),
                                        ImageUrl = reader["DefaultImageUrl"].ToString(),
                                        Variant = reader["Variant"].ToString(),
                                        Quantity = Convert.ToInt32(reader["Quantity"]),
                                        CurrentPrice = Convert.ToDecimal(reader["CurrentPrice"]),
                                        OriginalPrice = Convert.ToDecimal(reader["OriginalPrice"]),
                                        VariantId = reader["VariantID"] != DBNull.Value ?
                                            Convert.ToInt32(reader["VariantID"]) : (int?)null
                                    });
                                }
                            }
                        }
                    }
                    Session["Cart"] = cartItems;
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi
                    System.Diagnostics.Debug.WriteLine($"Error in GetAllCartItems: {ex.Message}");
                    throw;
                }
            }
            else
            {
                // Nếu chưa đăng nhập, lấy từ Session
                cartItems = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            }

            return cartItems;
        }
        protected void btnCheck(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Session["ReturnUrl"] = Request.Url.ToString();
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Chuyển hướng đến trang checkout
            Response.Redirect("~/Checkout.aspx?mode=checkout");
        }
    }
   
}