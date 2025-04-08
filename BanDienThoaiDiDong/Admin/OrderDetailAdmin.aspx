<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetailAdmin.aspx.cs" Inherits="BanDienThoaiDiDong.OrderDetailAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/orderManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-detail-admin">
        <div class="page-header">
            <div class="header-content">
                <h2>Chi tiết đơn hàng #<asp:Label ID="lblOrderId" runat="server" /></h2>
                <div class="status-badge">
                    <asp:Label ID="lblStatus" runat="server" CssClass="order-status" />
                </div>
            </div>
            <asp:Button ID="btnBack" runat="server" Text="Quay lại" OnClick="btnBack_Click" CssClass="btn-back" />
        </div>

        <div class="order-sections">
            <div class="order-info-section">
                <div class="section-box customer-info">
                    <h3>Thông tin khách hàng</h3>
                    <div class="info-row">
                        <span class="label">Họ tên:</span>
                        <asp:Label ID="lblCustomerName" runat="server" CssClass="value" />
                    </div>
                    <div class="info-row">
                        <span class="label">Email:</span>
                        <asp:Label ID="lblEmail" runat="server" CssClass="value" />
                    </div>
                    <div class="info-row">
                        <span class="label">Số điện thoại:</span>
                        <asp:Label ID="lblPhone" runat="server" CssClass="value" />
                    </div>
                </div>

                <div class="section-box shipping-info">
                    <h3>Thông tin giao hàng</h3>
                    <div class="info-row">
                        <span class="label">Địa chỉ:</span>
                        <asp:Label ID="lblAddress" runat="server" CssClass="value" />
                    </div>
                    <div class="info-row">
                        <span class="label">Ghi chú:</span>
                        <asp:Label ID="lblNote" runat="server" CssClass="value" />
                    </div>
                </div>
            </div>

            <div class="order-items-section">
                <h3>Chi tiết sản phẩm</h3>
                <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" 
                    CssClass="items-grid" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Sản phẩm">
                            <ItemTemplate>
                                <div class="product-info">
                                    <asp:Image ID="imgProduct" runat="server" 
                                        ImageUrl='<%# Eval("ImageUrl") %>' 
                                        CssClass="product-image" />
                                    <div class="product-details">
                                        <div class="product-name"><%# Eval("ProductName") %></div>
                                        <div class="product-variant">
                                            Phiên bản: <%# Eval("Color") %> - <%# Eval("Storage") %>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Price" HeaderText="Đơn giá" 
                            DataFormatString="{0:N0}₫" ItemStyle-CssClass="price-column" />
                        <asp:BoundField DataField="Quantity" HeaderText="Số lượng" 
                            ItemStyle-CssClass="quantity-column" />
                        <asp:BoundField DataField="SubTotal" HeaderText="Thành tiền" 
                            DataFormatString="{0:N0}₫" ItemStyle-CssClass="subtotal-column" />
                    </Columns>
                </asp:GridView>

                <div class="order-summary">
                    <div class="summary-row">
                        <span class="label">Tổng tiền hàng:</span>
                        <asp:Label ID="lblSubTotal" runat="server" CssClass="value" />
                    </div>
                    <div class="summary-row">
                        <span class="label">Phí vận chuyển:</span>
                        <asp:Label ID="lblShippingFee" runat="server" CssClass="value" />
                    </div>
                    <div class="summary-row total">
                        <span class="label">Tổng thanh toán:</span>
                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="value" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>