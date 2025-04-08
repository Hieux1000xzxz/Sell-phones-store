<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="BanDienThoaiDiDong.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="dynamicTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="./assets/css/profile.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="profile-container">
        <!-- Sidebar Menu -->
        <div class="profile-sidebar">
            <div class="user-info">
                <asp:Image ID="imgAvatar" runat="server" CssClass="user-avatar" ImageUrl="~/assets/icons/square-user.svg" />
                <div class="user-details">
                    <h3 class="user-name">
                        <asp:Literal ID="litUsername" runat="server"></asp:Literal></h3>
                    <p class="user-email">
                        <asp:Literal ID="litEmail" runat="server"></asp:Literal></p>
                </div>
            </div>

            <nav class="profile-nav">
                <asp:LinkButton ID="lnkProfile" runat="server" CssClass="nav-item active" OnClick="lnkProfile_Click">
                    <img src="assets/icons/user.svg" alt="Profile" />
                    Thông tin tài khoản
                </asp:LinkButton>
                <asp:LinkButton ID="lnkOrders" runat="server" CssClass="nav-item" OnClick="lnkOrders_Click">
                    <img src="assets/icons/shopping-bag.svg" alt="Orders" />
                    Đơn hàng của tôi
                </asp:LinkButton>
                <asp:LinkButton ID="lnkLogout" runat="server" CssClass="nav-item logout" OnClick="lnkLogout_Click">
                    <img src="assets/icons/log-out.svg" alt="Logout" />
                    Đăng xuất
                </asp:LinkButton>
            </nav>
        </div>

        <!-- Main Content Area -->
        <div class="profile-content">
            <asp:MultiView ID="mvProfile" runat="server" ActiveViewIndex="0">
                <!-- Profile View -->
                <asp:View ID="viewProfile" runat="server">
                    <h2 class="section-title">Thông tin tài khoản</h2>
                    <div class="profile-form">
                        <!-- Thông tin cá nhân -->
                        <div class="form-section">
                            <h3 class="form-section-title">Thông tin cá nhân</h3>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtFullName">Họ và tên</asp:Label>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName"
                                    ErrorMessage="Vui lòng nhập họ tên" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtEmail">Email</asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Vui lòng nhập email" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Email không hợp lệ" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtPhone">Số điện thoại</asp:Label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="Số điện thoại không hợp lệ" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="form-row">
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtBirthday">Ngày sinh</asp:Label>
                                    <asp:TextBox ID="txtBirthday" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="ddlGender">Giới tính</asp:Label>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="male">Nam</asp:ListItem>
                                        <asp:ListItem Value="female">Nữ</asp:ListItem>
                                        <asp:ListItem Value="other">Khác</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtAddress">Địa chỉ</asp:Label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-actions">
                            <asp:Button ID="btnCancel" runat="server" Text="Hủy thay đổi" CssClass="cancel-btn"
                                OnClick="btnCancel_Click" CausesValidation="false" />
                            <asp:Button ID="btnSave" runat="server" Text="Lưu thay đổi" CssClass="save-btn"
                                OnClick="btnSave_Click" />
                        </div>
                    </div>
                </asp:View>

                <!-- Orders View -->
                <asp:View ID="viewOrders" runat="server">
                    <h2 class="section-title">Đơn hàng của tôi</h2>
                    
                    <!-- Order Tabs -->
                    <div class="order-tabs">
                        <asp:LinkButton ID="btnAllOrders" runat="server" CssClass="tab-btn active" OnClick="OrderTab_Click" >
                            Tất cả
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnPendingOrders" runat="server" CssClass="tab-btn" OnClick="OrderTab_Click">
                            Chờ xác nhận
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnShippingOrders" runat="server" CssClass="tab-btn" OnClick="OrderTab_Click">
                            Đang giao
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDeliveredOrders" runat="server" CssClass="tab-btn" OnClick="OrderTab_Click">
                            Đã giao
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnCancelledOrders" runat="server" CssClass="tab-btn" OnClick="OrderTab_Click">
                            Đã hủy
                        </asp:LinkButton>
                    </div>

                    <!-- Order List -->
                    <div class="order-list">
                        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
                            <ItemTemplate>
                                <div class="order-item">
                                    <div class="order-header">
                                        <div class="order-id">Mã đơn hàng #<%# Eval("OrderID") %></div>
                                        <h5>Thời gian tạo đơn: <%# Eval("OrderDate") %></h5>
                                        <div class="order-status <%# GetStatusClass(Eval("Status").ToString()) %>">
                                            <%# Eval("Status") %>
                                        </div>
                                    </div>
                                    <div class="order-products">
                                        <asp:Repeater ID="rptOrderItems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                                            <ItemTemplate>
                                                <div class="product-item">
                                                    <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>'>
                                                    <div class="product-info">
                                                        <h4><%# Eval("ProductName") %></h4>
                                                        <p>Số lượng: <%# Eval("Quantity") %></p>
                                                        <p class="product-price"><%# FormatPrice(Eval("Price")) %></p>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="order-footer">
                                        <div class="order-total">
                                            <span>Tổng tiền:</span>
                                            <span class="total-price"><%# FormatPrice(Eval("TotalAmount")) %></span>
                                        </div>
                                        <div class="order-actions">
                                            <asp:LinkButton runat="server" CssClass="action-btn" 
                                                CommandName="ViewDetail" CommandArgument='<%# Eval("OrderID") %>'>
                                                Chi tiết
                                            </asp:LinkButton>
                               
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>