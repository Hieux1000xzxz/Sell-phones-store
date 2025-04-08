<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BanDienThoaiDiDong.Default" %>
<asp:Content ID="Content0" ContentPlaceHolderID="dynamicTitle" runat="server">
    PhoneShop
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content">
        <div class="categories-and-banner">
            <!-- Category sidebar -->
            <div class="category-sidebar">
                <h3 class="category-title">Danh mục sản phẩm</h3>
                <asp:Repeater ID="rptCategories" runat="server">
                    <HeaderTemplate>
                        <ul class="category-list">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="category-item">
                            <asp:HyperLink ID="lnkCategory" runat="server"
                                NavigateUrl='<%# "Products.aspx?category=" + Eval("CategoryID") %>'
                                CssClass="category-link">
                                
                                <asp:Label ID="lblCategoryName" runat="server"
                                    Text='<%# Eval("CategoryName") %>'></asp:Label>
                            </asp:HyperLink>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <!-- Main banner - Giữ nguyên vì đã xử lý bằng JS -->
            <div class="main-banner">
                <div class="slider">
                    <div class="slider-container">
                        <div class="slide">
                            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSWDlXhTUhdiO6JCcDEX3Eh3PmLmb5StuFF9A&s" alt="Banner 1">
                        </div>
                        <div class="slide">
                            <img src="https://png.pngtree.com/thumb_back/fh260/back_our/20190621/ourmid/pngtree-cool-new-mobile-phone-promotion-purple-banner-image_183067.jpg" alt="Banner 2">
                        </div>
                        <div class="slide">
                            <img src="https://d1csarkz8obe9u.cloudfront.net/posterpreviews/smart-phone-banner-design-template-caa98978d25e965873a22b01acb99ba7_screen.jpg?ts=1718877755" alt="Banner 3">
                        </div>
                    </div>
                    <button class="slider-btn prev-btn" type="button">
                        <img src="./assets/icons/chevron-left.svg" alt="Previous">
                    </button>
                    <button class="slider-btn next-btn" type="button">
                        <img src="./assets/icons/chevron-right.svg" alt="Next">
                    </button>
                    <div class="slider-dots"></div>
                </div>
            </div>
        </div>

        <!-- Products Grid -->
        <div class="products-grid">
            <h2 class="section-title">Sản phẩm nổi bật</h2>
            <asp:Repeater ID="rptFeaturedProducts" runat="server">
                <HeaderTemplate>
                    <div class="products-container">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="product-card">
                        <asp:HyperLink ID="lnkProduct" runat="server"
                            NavigateUrl='<%# "ProductDetail.aspx?id=" + Eval("ProductId") %>'
                            CssClass="product-link">
                            <div class="product-image">
                                <asp:Image ID="imgProduct" runat="server"
                                    ImageUrl='<%# Eval("ImageUrl") %>'
                                    AlternateText='<%# Eval("ProductName") %>' />
                                   <div class="discount-badge">
                                        <%# GetDiscountPercentage(Eval("OriginalPrice"), Eval("SalePrice")) %>%
                                    </div>
                            </div>
                            <div class="product-info">
                                <h3 class="product-name"><%# Eval("ProductName") %></h3>
                                <div class="price-info">
                                    <div class="original-price"><%# String.Format("{0:N0}₫", Eval("OriginalPrice")) %></div>
                                    <div class="product-price"><%# String.Format("{0:N0}₫", Eval("SalePrice")) %></div>
                                </div>
                            </div>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div class="products-grid best-sellers">
            <h2 class="section-title">Sản phẩm bán chạy</h2>
            <asp:Repeater ID="rptBestSellers" runat="server">
                <HeaderTemplate>
                    <div class="products-container">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="product-card">
                        <asp:HyperLink ID="lnkProduct" runat="server"
                            NavigateUrl='<%# "ProductDetail.aspx?id=" + Eval("ProductId") %>'
                            CssClass="product-link">
                            <div class="product-image">
                                <asp:Image ID="imgProduct" runat="server"
                                    ImageUrl='<%# Eval("ImageUrl") %>'
                                    AlternateText='<%# Eval("ProductName") %>' />
                                   <div class="discount-badge">
                                        <%# GetDiscountPercentage(Eval("OriginalPrice"), Eval("SalePrice")) %>%
                                    </div>
                                <div class="rank-badge">Top <%# Container.ItemIndex + 1 %></div>
                            </div>
                            <div class="product-info">
                                <h3 class="product-name"><%# Eval("ProductName") %></h3>
                                <div class="price-info">
                                    <div class="original-price"><%# String.Format("{0:N0}₫", Eval("OriginalPrice")) %></div>
                                    <div class="product-price"><%# String.Format("{0:N0}₫", Eval("SalePrice")) %></div>
                                </div>
                            </div>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div class="products-grid new-arrivals">
            <h2 class="section-title">Sản phẩm mới</h2>
            <asp:Repeater ID="rptNewProducts" runat="server">
                <HeaderTemplate>
                    <div class="products-container">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="product-card">
                        <asp:HyperLink ID="lnkProduct" runat="server"
                            NavigateUrl='<%# "ProductDetail.aspx?id=" + Eval("ProductId") %>'
                            CssClass="product-link">
                            <div class="product-image">
                                <asp:Image ID="imgProduct" runat="server"
                                    ImageUrl='<%# Eval("ImageUrl") %>'
                                    AlternateText='<%# Eval("ProductName") %>' />
                                   <div class="discount-badge">
                                        <%# GetDiscountPercentage(Eval("OriginalPrice"), Eval("SalePrice")) %>%
                                    </div>
                                <div class="new-badge">Mới</div>
                            </div>
                            <div class="product-info">
                                <h3 class="product-name"><%# Eval("ProductName") %></h3>
                                <div class="price-info">
                                    <div class="original-price"><%# String.Format("{0:N0}₫", Eval("OriginalPrice")) %></div>
                                    <div class="product-price"><%# String.Format("{0:N0}₫", Eval("SalePrice")) %></div>
                                </div>
                            </div>
                        </asp:HyperLink>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
