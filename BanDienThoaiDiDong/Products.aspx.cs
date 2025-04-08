using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string searchTerm = Request.QueryString["category"];
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    lblSearchTerm.Text = getCategoryName(searchTerm); // Hiển thị từ khóa tìm kiếm trên trang
                    PerformSearch(searchTerm); // Thực hiện tìm kiếm
                }
            }
        }

        private void PerformSearch(string searchTerm, string sortBy = "relevance", string priceRange = "all")
        {
            var searchResults = Database.GetProductsByCategory(searchTerm, sortBy, priceRange);

            rptProducts.DataSource = searchResults;
            rptProducts.DataBind();

            lblResultCount.Text = searchResults.Count.ToString();
            btnLoadMore.Visible = searchResults.Count >= 12;
        }
        protected string getCategoryName(string searchTerm)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    string query = @"SELECT CategoryName FROM Categories WHERE CategoryID= @CategoryID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CategoryId", searchTerm);
                    conn.Open();
                    object result = cmd.ExecuteScalar().ToString();
                    return result.ToString();
                }

                catch (Exception ex)
                {
                    // Ghi log hoặc xử lý ngoại lệ
                    throw new InvalidOperationException("Lỗi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
            }
        }
        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortBy = ddlSort.SelectedValue;
            string searchTerm = lblSearchTerm.Text;
            string priceRange = ddlPriceRange.SelectedValue;

            PerformSearch(searchTerm, sortBy, priceRange);
        }

        protected void ddlPriceRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortBy = ddlSort.SelectedValue;
            string searchTerm = lblSearchTerm.Text;
            string priceRange = ddlPriceRange.SelectedValue;

            PerformSearch(searchTerm, sortBy, priceRange);
        }

        // Thêm hỗ trợ phân trang
        private const int PageSize = 12;
        private int CurrentPage = 1;
        protected void btnLoadMore_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            string searchTerm = lblSearchTerm.Text;
            string sortBy = ddlSort.SelectedValue;
            string priceRange = ddlPriceRange.SelectedValue;

            var searchResults = Database.SearchProducts(searchTerm, sortBy, priceRange)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (searchResults.Count < PageSize)
            {
                btnLoadMore.Visible = false;
            }

            // Nếu không phải là lần đầu tiên, thêm các sản phẩm mới vào danh sách hiện tại
            var currentProducts = rptProducts.DataSource as List<Product> ?? new List<Product>();
            currentProducts.AddRange(searchResults);

            rptProducts.DataSource = currentProducts;
            rptProducts.DataBind();
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
