using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BanDienThoaiDiDong
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateCartCount();
                UpdateLoginStatus();
            }
        }

        public void UpdateLoginStatus()
        {
            if (Session["UserID"] != null)
            {
                // Đã đăng nhập - hiển thị tên người dùng
                loginLink.NavigateUrl = "~/Profile.aspx";
                lblAccount.Text = Session["FullName"].ToString();
                loginLink.ToolTip = Session["Email"].ToString();
            }
            else
            {
                // Chưa đăng nhập
                loginLink.NavigateUrl = "~/Login.aspx";
                lblAccount.Text = "Tài khoản";
                loginLink.ToolTip = "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = searchBar.Text.Trim(); // Lấy từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Chuyển hướng người dùng đến trang SearchingResult.aspx với từ khóa tìm kiếm
                Response.Redirect("~/SearchingResult.aspx?searchTerm=" + HttpUtility.UrlEncode(searchTerm));
            }
        }


        protected void cartButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cart.aspx");
        }
        public void UpdateCartCount()
        {
           lblCartCount.Text = Database.slitems.ToString();
        }
    }
}