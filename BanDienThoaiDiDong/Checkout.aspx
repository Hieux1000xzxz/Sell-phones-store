<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="BanDienThoaiDiDong.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
    Thanh toán
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="./assets/css/Checkout.css" rel="stylesheet" />
    <script type="text/javascript">
        function showOrderSuccess() {
            Toastify({
                text: "Đặt hàng thành công!",
                duration: 3000,
                gravity: "top",
                position: "right",
                backgroundColor: "#4CAF50",
                stopOnFocus: true
            }).showToast();

            // Cập nhật số lượng giỏ hàng về 0
            var cartCount = document.querySelector('.cart-count');
            if (cartCount) {
                cartCount.textContent = '0';
            }
        }
    </script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/toastify-js/src/toastify.min.css">
    <script src="https://cdn.jsdelivr.net/npm/toastify-js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="checkout-container">
        <h1 class="checkout-title">Thanh toán</h1>

        <div class="checkout-content">
            <div class="checkout-form">
                <div class="form-section">
                    <h2 class="section-title">Thông tin giao hàng</h2>
                    <div class="form-group">
                        <label for="fullname">Họ và tên</label>
                        <asp:TextBox ID="txtFullName" runat="server" placeholder="Nhập họ và tên" />
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                            ControlToValidate="txtFullName"
                            ErrorMessage="Vui lòng nhập họ tên"
                            CssClass="error-message" />
                    </div>
                    <div class="form-group">
                        <label for="phone">Số điện thoại</label>
                        <asp:TextBox ID="txtPhone" runat="server" placeholder="Nhập số điện thoại" />
                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                            ControlToValidate="txtPhone"
                            ErrorMessage="Vui lòng nhập số điện thoại"
                            CssClass="error-message" />
                    </div>
                    <div class="form-group">
                        <label for="email">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Nhập email" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                            ControlToValidate="txtEmail"
                            ErrorMessage="Vui lòng nhập email"
                            CssClass="error-message" />
                    </div>
                    <div class="form-group">
                        <label for="address">Địa chỉ</label>
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" 
                            placeholder="Nhập địa chỉ chi tiết" />
                        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" 
                            ControlToValidate="txtAddress"
                            ErrorMessage="Vui lòng nhập địa chỉ"
                            CssClass="error-message" />
                    </div>
                    <div class="form-group">
                        <label for="note">Ghi chú</label>
                        <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" 
                            placeholder="Ghi chú về đơn hàng" />
                    </div>
                </div>

                <div class="form-section">
                    <h2 class="section-title">Phương thức thanh toán</h2>
                    <div class="payment-methods">
                        <asp:RadioButtonList ID="rblPaymentMethod" runat="server" CssClass="payment-method-list">
                            <asp:ListItem Value="cod" Selected="True">
                                <div class="payment-method">
                                    <img src="./assets/icons/cash.svg" alt="COD">
                                    <span>Thanh toán khi nhận hàng (COD)</span>
                                </div>
                            </asp:ListItem>
                            <asp:ListItem Value="bank">
                                <div class="payment-method">
                                    <img src="./assets/icons/credit-card.svg" alt="Bank">
                                    <span>Chuyển khoản ngân hàng</span>
                                </div>
                            </asp:ListItem>
                            <asp:ListItem Value="momo">
                                <div class="payment-method">
                                    <img src="./assets/icons/momo.svg" alt="Momo">
                                    <span>Ví MoMo</span>
                                </div>
                            </asp:ListItem>
                            <asp:ListItem Value="zalopay">
                                <div class="payment-method">
                                    <img src="./assets/icons/zalopay.svg" alt="ZaloPay">
                                    <span>ZaloPay</span>
                                </div>
                            </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>

            <div class="order-summary">
                <h2 class="summary-title">Đơn hàng của bạn</h2>
                <div class="order-items">
                    <asp:Repeater ID="rptOrderItems" runat="server">
                        <ItemTemplate>
                            <div class="order-item">
                                <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>'>
                                <div class="item-info">
                                    <h3><%# Eval("ProductName") %></h3>
                                    <p><%# Eval("Variant") %></p>    
                                    <p>Số lượng: <%# Eval("Quantity") %></p>
                                    
                                    <p class="item-price"><%# String.Format("{0:N0}₫", Eval("CurrentPrice")) %></p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="price-summary">
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
                </div>

                <asp:Button ID="btnPlaceOrder" runat="server" 
                    Text="Đặt hàng" 
                    CssClass="place-order-btn"
                    OnClick="btnPlaceOrder_Click" 
                    OnClientClick="return confirm('Bạn có chắc chắn muốn đặt hàng?');" />

                <!-- Thêm ValidationSummary để hiển thị tất cả lỗi -->
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    HeaderText="Vui lòng kiểm tra các thông tin sau:"
                    ShowMessageBox="true"
                    ShowSummary="false" />
            </div>
        </div>
    </div>
</asp:Content>