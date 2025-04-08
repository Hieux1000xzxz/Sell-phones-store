<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchingResult.aspx.cs" Inherits="BanDienThoaiDiDong.SearchingResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
    Kết quả tìm kiếm
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="./assets/css/searchingResult.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search-results-container">
        <!-- Breadcrumb -->
        <div class="breadcrumb">
            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/">Trang chủ</asp:HyperLink>
            <span class="separator">/</span>
            <span class="current-page">Kết quả tìm kiếm: "<asp:Label ID="lblSearchTerm" runat="server" />"</span>
        </div>

        <!-- Filter Section -->
        <div class="filter-section">
            <div class="filter-group">
                <label>Sắp xếp theo:</label>
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="sort-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                    <asp:ListItem Value="relevance" Text="Độ phù hợp" />
                    <asp:ListItem Value="price-asc" Text="Giá tăng dần" />
                    <asp:ListItem Value="price-desc" Text="Giá giảm dần" />
                    <asp:ListItem Value="name-asc" Text="Tên A-Z" />
                    <asp:ListItem Value="name-desc" Text="Tên Z-A" />
                </asp:DropDownList>
            </div>

            <div class="filter-group">
                <label>Giá:</label>
                <asp:DropDownList ID="ddlPriceRange" runat="server" CssClass="price-range-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPriceRange_SelectedIndexChanged">
                    <asp:ListItem Value="all" Text="Tất cả" />
                    <asp:ListItem Value="0-5000000" Text="Dưới 5 triệu" />
                    <asp:ListItem Value="5000000-10000000" Text="5 - 10 triệu" />
                    <asp:ListItem Value="10000000-20000000" Text="10 - 20 triệu" />
                    <asp:ListItem Value="20000000" Text="Trên 20 triệu" />
                </asp:DropDownList>
            </div>
        </div>

        <!-- Results Info -->
        <div class="results-info">
            <p>Tìm thấy <strong><asp:Label ID="lblResultCount" runat="server" /></strong> sản phẩm</p>
        </div>

        <!-- Products Grid -->
        <div class="products-grid">
            <div class="products-container">
                <asp:Repeater ID="rptProducts" runat="server">
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
                </asp:Repeater>
            </div>
            
            <asp:Button ID="btnLoadMore" runat="server" 
                Text="Xem thêm sản phẩm" 
                CssClass="load-more-btn" 
                OnClick="btnLoadMore_Click" 
                Visible="false" />
        </div>
    </div>
</asp:Content>