<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="BanDienThoaiDiDong.OrderDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
    Chi tiết đơn hàng
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="assets/css/orderDetail.css" rel="stylesheet" />
    <script type="text/javascript">
        function confirmCancel() {
            return confirm('Bạn có chắc chắn muốn hủy đơn hàng này không?');
        }

        function showMessage(message, isSuccess) {
            Toastify({
                text: message,
                duration: 3000,
                gravity: "top",
                position: "right",
                backgroundColor: isSuccess ? "#4CAF50" : "#f44336",
                stopOnFocus: true
            }).showToast();
        }
    </script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css">
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-details-container">
        <div class="order-header">
            <div class="order-status-container">
                <h1>Chi tiết đơn hàng #<asp:Label ID="lblOrderId" runat="server" /></h1>
                <asp:Label ID="lblOrderStatus" runat="server" CssClass="order-status" />
            </div>
            
            <div class="order-info">
                <div class="info-group">
                    <h3><i class="fas fa-user"></i> Thông tin khách hàng</h3>
                    <p>Họ tên: <asp:Label ID="lblCustomerName" runat="server" /></p>
                    <p>Email: <asp:Label ID="lblEmail" runat="server" /></p>
                    <p>Điện thoại: <asp:Label ID="lblPhone" runat="server" /></p>
                </div>

                <div class="info-group">
                    <h3><i class="fas fa-map-marker-alt"></i> Địa chỉ giao hàng</h3>
                    <p><asp:Label ID="lblShippingAddress" runat="server" /></p>
                </div>

                <div class="info-group">
                    <h3><i class="fas fa-info-circle"></i> Thông tin đơn hàng</h3>
                    <p>Ngày đặt: <asp:Label ID="lblOrderDate" runat="server" /></p>
                    <p>Ghi chú: <asp:Label ID="lblNote" runat="server" /></p>
                </div>
            </div>
        </div>

        <div class="order-items">
            <h2><i class="fas fa-shopping-bag"></i> Sản phẩm đã đặt</h2>
            <asp:Repeater ID="rptOrderDetails" runat="server">
                <ItemTemplate>
                    <div class="item">
                        <div class="item-image-container">
                            <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="item-image">
                        </div>
                        <div class="item-details">
                            <h3><%# Eval("ProductName") %></h3>
                            <div class="item-specs">
                                <p><i class="fas fa-palette"></i> Màu sắc: <%# Eval("Color") %></p>
                                <p><i class="fas fa-memory"></i> Dung lượng: <%# Eval("Storage") %>GB</p>
                                <p><i class="fas fa-shopping-basket"></i> Số lượng: <%# Eval("Quantity") %></p>
                            </div>
                        </div>
                        <div class="item-price">
                            <p class="price"><%# String.Format("{0:N0}₫", Eval("Price")) %></p>
                            <p class="item-total">Thành tiền: <%# String.Format("{0:N0}₫", Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))) %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="order-summary">
            <h2><i class="fas fa-file-invoice-dollar"></i> Tổng quan đơn hàng</h2>
            <div class="summary-row">
                <span>Tạm tính:</span>
                <span><asp:Label ID="lblSubTotal" runat="server" /></span>
            </div>
            <div class="summary-row">
                <span>Phí vận chuyển:</span>
                <span><asp:Label ID="lblShippingFee" runat="server" /></span>
            </div>
            <div class="summary-row total-row">
                <span>Tổng cộng:</span>
                <span><asp:Label ID="lblTotal" runat="server" /></span>
            </div>
        </div>

        <div class="order-actions">
            <asp:Button ID="btnCancelOrder" runat="server"
                Text="Hủy đơn hàng"
                CssClass="btn-cancel-order"
                OnClick="btnCancelOrder_Click"
                OnClientClick="return confirmCancel();"
                Visible="false" />
        </div>
    </div>
</asp:Content>