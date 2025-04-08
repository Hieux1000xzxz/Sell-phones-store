using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace BanDienThoaiDiDong
{
    public partial class Checkout : System.Web.UI.Page
    {
        private bool IsBuyNowMode => Request.QueryString["mode"] == "buynow";
        private List<CartItem> CartItems => IsBuyNowMode ? (List<CartItem>)Session["BuyNowCart"] : (List<CartItem>)Session["Cart"];
        private int UserId => Convert.ToInt32(Session["UserId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Session["ReturnUrl"] = Request.Url.ToString();
                    Response.Redirect("~/Login.aspx");
                }
                LoadUserInfo();
                UpdateOrderSummary();
                rptOrderItems.DataSource = CartItems;
                rptOrderItems.DataBind();
            }
        }

        private void LoadUserInfo()
        {
            txtFullName.Text = Session["FullName"]?.ToString() ?? "";
            txtEmail.Text = Session["Email"]?.ToString() ?? "";
            txtPhone.Text = Session["Phone"]?.ToString() ?? "";
            txtAddress.Text = Session["Address"]?.ToString() ?? "";
        }

        private void UpdateOrderSummary()
        {
            if (CartItems == null) return;

            decimal subTotal = CartItems.Sum(x => x.CurrentPrice * x.Quantity);
            lblSubTotal.Text = $"{subTotal:N0}₫";
            lblDiscount.Text = $"-{CartItems.Sum(x => (x.OriginalPrice - x.CurrentPrice) * x.Quantity):N0}₫";
            lblTotal.Text = $"{subTotal:N0}₫";
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SaveOrder();
                    ShowSuccessMessage();
                    if (IsBuyNowMode == false) {
                        ClearCartFromDatabase();
                    }
                  
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void SaveOrder()
        {
            var order = new Order
            {
                CustomerName = txtFullName.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                Note = txtNote.Text,
                PaymentMethod = rblPaymentMethod.SelectedValue,
                CartItems = CartItems,
                OrderDate = DateTime.Now,
                UserId = UserId,
                TotalAmount = CartItems.Sum(x => x.CurrentPrice * x.Quantity)
            };
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    int orderId = InsertOrder(order, conn, transaction);
                    InsertOrderDetails(orderId, conn, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi khi lưu đơn hàng: " + ex.Message);
                }
            }
        }
        private int InsertOrder(Order order, MySqlConnection conn, MySqlTransaction transaction)
        {
            string insertOrderQuery = @"
                INSERT INTO Orders (UserID, OrderDate, ShippingAddress, ShippingPhone, TotalAmount, Note)
                OUTPUT INSERTED.OrderID
                VALUES (@UserID, @OrderDate, @ShippingAddress, @ShippingPhone, @TotalAmount, @Note)";

            using (var cmd = new MySqlCommand(insertOrderQuery, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@ShippingPhone", order.Phone);
                cmd.Parameters.AddWithValue("@ShippingAddress", order.Address);
                cmd.Parameters.AddWithValue("@Note", string.IsNullOrEmpty(order.Note) ? DBNull.Value : (object)order.Note);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                return (int)cmd.ExecuteScalar();
            }
        }
        private void InsertOrderDetails(int orderId, MySqlConnection conn, MySqlTransaction transaction)
        {
            foreach (var item in CartItems)
            {
                string insertOrderDetailQuery = @"
                    INSERT INTO OrderDetails (OrderID, ProductID, VariantID, Quantity, Price)
                    VALUES (@OrderID, @ProductID, @VariantID, @Quantity, @Price)";

                using (var cmd = new MySqlCommand(insertOrderDetailQuery, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@ProductID", item.ProductId);
                    cmd.Parameters.AddWithValue("@VariantID", item.VariantId);
                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@Price", item.CurrentPrice);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void ClearCartFromDatabase()
        {
            if (Session["UserID"] == null) return;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "DELETE FROM CartItems WHERE UserID = @UserID";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", UserId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in ClearCartFromDatabase: " + ex.Message);
            }
        }
        private void ShowSuccessMessage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "OrderSuccess", "showOrderSuccess(); setTimeout(function() { window.location='Default.aspx'; }, 1500);", true);
        }
        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "OrderError", $"alert('Có lỗi xảy ra: {message}');", true);
        }
    }
}
