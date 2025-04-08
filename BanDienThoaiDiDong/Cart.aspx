<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="BanDienThoaiDiDong.Cart" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
    Giỏ hàng
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./assets/css/cart.css">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cart-container">
        <h1 class="cart-title">Giỏ hàng của bạn</h1>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="cart-content">
                    <div class="cart-items">
                        <asp:Repeater ID="rptCartItems" runat="server">
                            <ItemTemplate>
                                <div class="cart-item">
                                    <div class="item-image">
                                        <asp:Image ID="imgProduct" runat="server" 
                                            ImageUrl='<%# Eval("ImageUrl") %>' 
                                            AlternateText='<%# Eval("ProductName") %>' />
                                    </div>
                                    <div class="item-details">
                                        <h3 class="item-name"><%# Eval("ProductName") %></h3>
                                        <p class="item-variant"><%# Eval("Variant") %></p>
                                    </div>
                                    <div class="item-quantity">
                                        <asp:LinkButton ID="btnMinus" runat="server" 
                                            CssClass="quantity-btn minus"
                                            CommandName="DecreaseQuantity"
                                            CommandArgument='<%# Eval("VariantID") %>'
                                            OnCommand="UpdateQuantity_Command">-</asp:LinkButton>
                                        
                                        <asp:TextBox ID="txtQuantity" runat="server" 
                                            CssClass="quantity-input"
                                            Text='<%# Eval("Quantity") %>'
                                            AutoPostBack="true"
                                            ReadOnly ="true"
                                            OnTextChanged="txtQuantity_TextChanged" />
                                        
                                        <asp:LinkButton ID="btnPlus" runat="server" 
                                            CssClass="quantity-btn plus"
                                            CommandName="IncreaseQuantity"
                                            CommandArgument='<%# Eval("VariantID") %>'
                                            OnCommand="UpdateQuantity_Command">+</asp:LinkButton>
                                    </div>
                                    <div class="item-price">
                                        <p class="current-price"><%# String.Format("{0:N0}₫", Eval("CurrentPrice")) %></p>
                                        <p class="original-price"><%# String.Format("{0:N0}₫", Eval("OriginalPrice")) %></p>
                                    </div>
                                    <asp:LinkButton ID="btnRemove" runat="server" 
                                        CssClass="remove-item"
                                        CommandName="RemoveItem"
                                        CommandArgument='<%# Eval("VariantID") %>'
                                        OnCommand="RemoveItem_Command">
                                        <img src="./assets/icons/trash.svg" alt="Remove" />
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="cart-summary">
                        <h2 class="summary-title">Tổng giỏ hàng</h2>
                        <div class="summary-row">
                            <span>Tạm tính:</span>
                            <asp:Label ID="lblSubTotal" runat="server" />
                        </div>
                        <div class="summary-row">
                            <span>Giảm giá:</span>
                            <asp:Label ID="lblDiscount" runat="server" />
                        </div>
                        <div class="summary-row shipping">
                            <span>Phí vận chuyển:</span>
                            <asp:Label ID="lblShipping" runat="server" Text="Miễn phí" />
                        </div>
                        <div class="summary-total">
                            <span>Tổng cộng:</span>
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                        <div class="cart-actions">
                            <asp:Button ID="btnCheckout" runat="server" 
                                Text="Tiến hành thanh toán" 
                                CssClass="checkout-btn"
                                OnClick="btnCheck" />
                            <asp:HyperLink ID="lnkContinueShopping" runat="server" 
                                NavigateUrl="~/" 
                                CssClass="continue-shopping">
                                Tiếp tục mua sắm
                            </asp:HyperLink>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>