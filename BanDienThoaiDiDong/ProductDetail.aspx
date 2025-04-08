<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="BanDienThoaiDiDong.ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
    Chi tiết sản phẩm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="./assets/css/productDetail.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Breadcrumb -->
    <div class="breadcrumb">
        <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/">Trang chủ</asp:HyperLink>
        <span class="separator">/</span>
        <span class="current"><asp:Label ID="lblProductName" runat="server" /></span>
    </div>

    <div class="product-detail">
        <!-- Gallery bên trái -->
        <div class="product-gallery">
            <div class="main-image">
                <asp:Image ID="imgMainProduct" runat="server" />
            </div>
        </div>

        <!-- Thông tin bên phải -->
        <div class="product-info">
            <div class="brand-name">
                <asp:Label ID="lblBrand" runat="server" />
            </div>
            <h1 class="product-title">
                <asp:Label ID="lblTitle" runat="server" />
            </h1>
            
            <div class="product-price">
                <span class="current-price">
                    <asp:Label ID="lblCurrentPrice" runat="server" />
                </span>
                <span class="original-price">
                    <asp:Label ID="lblOriginalPrice" runat="server" />
                </span>
                <span class="discount-badge">
                    <asp:Label ID="lblDiscount" runat="server" />
                </span>
            </div>

            <!-- Các tùy chọn -->
            <div class="variant-sections">
                <div class="variant-section">
                    <h3 class="variant-title">Dung lượng</h3>
                    <div class="variant-options">
                        <asp:Repeater ID="rptStorage" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnStorage" runat="server" 
                                    CssClass='<%# Eval("IsSelected").ToString() == "True" ? "variant-btn active" : "variant-btn" %>'
                                    OnClick="StorageButton_Click" 
                                    CommandArgument='<%# Eval("Value") %>'>
                                    <%# Eval("Text") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

                <div class="variant-section">
                    <h3 class="variant-title">Màu sắc</h3>
                    <div class="variant-options">
                        <asp:Repeater ID="rptColors" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnColor" runat="server" 
                                    CssClass='<%# Eval("IsSelected").ToString() == "True" ? "color-btn active" : "color-btn" %>'
                                    OnClick="ColorButton_Click" 
                                    CommandArgument='<%# Eval("Value") %>'>
                                    <span class="color-circle" style='background-color: <%# Eval("ColorCode") %>'></span>
                                    <%# Eval("Text") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="quantity-selector">
                <label>Số lượng:</label>
                <div class="quantity-controls">
                    <asp:LinkButton ID="btnDecrease" runat="server"
                        CssClass="quantity-btn"
                        OnClick="QuantityButton_Click"
                        CommandName="decrease">
                        <span class="fas fa-minus quantity-btn-style">-</span>
                    </asp:LinkButton>

                    <asp:TextBox ID="txtQuantity" runat="server"
                        CssClass="quantity-input"
                        Text="1"
                        AutoPostBack="true"
                        OnTextChanged="txtQuantity_TextChanged" />

                    <asp:LinkButton ID="btnIncrease" runat="server"
                        CssClass="quantity-btn"
                        OnClick="QuantityButton_Click"
                        CommandName="increase">
            <span class="fas fa-plus quantity-btn-style">+</span>
                    </asp:LinkButton>
                </div>
            </div>

            <!-- Buttons -->
            <div class="product-actions">
                <asp:Button ID="btnBuyNow" runat="server" Text="Mua ngay" CssClass="buy-now-btn" OnClick="BuyNow_Click" />
                <asp:Button ID="btnAddToCart" runat="server" Text="Thêm vào giỏ" CssClass="add-to-cart-btn" OnClick="AddToCart_Click" />
            </div>
        </div>
    </div>

    <!-- Mô tả và thông số -->
    <div class="product-details">
        <div class="product-description">
            <h2 class="section-title">Mô tả sản phẩm</h2>
            <div class="description-content">
                <asp:Literal ID="litDescription" runat="server" />
            </div>
        </div>

        <div class="product-specs">
            <h2 class="section-title">Thông số kỹ thuật</h2>
            <asp:GridView ID="gvSpecs" runat="server" CssClass="specs-table" 
                AutoGenerateColumns="false" ShowHeader="false">
                <Columns>
                    <asp:BoundField DataField="SpecName" ItemStyle-CssClass="spec-name" />
                    <asp:BoundField DataField="SpecValue" ItemStyle-CssClass="spec-value" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>