<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BanDienThoaiDiDong.Register" %>
<asp:Content ID="Content0" ContentPlaceHolderID="dynamicTitle" runat="server">
    Đăng ký - THT
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="auth-wrapper">
    <div class="auth-container">
        <div class="auth-header">
            <h2>Đăng ký tài khoản</h2>
            <p>Tạo tài khoản để mua sắm dễ dàng hơn</p>
        </div>
        <div class="auth-form">
            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtFullName">Họ và tên</asp:Label>
                <asp:TextBox ID="txtFullName" runat="server" 
                    placeholder="Nhập họ và tên" 
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                    ControlToValidate="txtFullName"
                    ErrorMessage="Vui lòng nhập họ tên"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtPhone">Số điện thoại</asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" TextMode="Phone" 
                    placeholder="Nhập số điện thoại" 
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                    ControlToValidate="txtPhone"
                    ErrorMessage="Vui lòng nhập số điện thoại"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPhone" runat="server"
                    ControlToValidate="txtPhone"
                    ValidationExpression="^(0|84)(2(0[3-9]|1[0-6|8|9]|2[0-2|5-9]|3[2-9]|4[0-9]|5[1|2|4-9]|6[0-3|9]|7[0-7]|8[0-9]|9[0-4|6|7|9])|3[2-9]|5[5|6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])([0-9]{7})$"
                    ErrorMessage="Số điện thoại không hợp lệ"
                    Display="Dynamic"
                    CssClass="error-message">
                </asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtEmail">Email</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" 
                    placeholder="Nhập email" 
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail"
                    ErrorMessage="Vui lòng nhập email"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ErrorMessage="Email không hợp lệ"
                    Display="Dynamic"
                    CssClass="error-message">
                </asp:RegularExpressionValidator>
            </div>

            <!-- Thêm trường ngày sinh -->
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtBirthday">Ngày sinh</asp:Label>
                <asp:TextBox ID="txtBirthday" runat="server" TextMode="Date" 
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <!-- Thêm trường giới tính -->
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlGender">Giới tính</asp:Label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                    <asp:ListItem Value="male">Nam</asp:ListItem>
                    <asp:ListItem Value="female">Nữ</asp:ListItem>
                    <asp:ListItem Value="other">Khác</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Thêm trường địa chỉ -->
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtAddress">Địa chỉ</asp:Label>
                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" 
                    placeholder="Nhập địa chỉ" 
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtPassword">Mật khẩu</asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                    placeholder="Nhập mật khẩu" 
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword"
                    ErrorMessage="Vui lòng nhập mật khẩu"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ValidationExpression=".{8,}"
                    ErrorMessage="Mật khẩu phải có ít nhất 8 ký tự"
                    Display="Dynamic"
                    CssClass="error-message">
                </asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtConfirmPassword">Xác nhận mật khẩu</asp:Label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                    placeholder="Nhập lại mật khẩu" 
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword"
                    ErrorMessage="Vui lòng xác nhận mật khẩu"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtPassword"
                    ErrorMessage="Mật khẩu không khớp"
                    Display="Dynamic" 
                    CssClass="error-message">
                </asp:CompareValidator>
            </div>

            <asp:Button ID="btnRegister" runat="server" 
                Text="Đăng ký" 
                CssClass="submit-btn"
                OnClick="btnRegister_Click" />
        </div>

        <div class="auth-links">
            <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" CssClass="switch-auth">
                Đã có tài khoản? Đăng nhập ngay
            </asp:HyperLink>
            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/Default.aspx" CssClass="back-home">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                </svg>
                Quay về trang chủ
            </asp:HyperLink>
        </div>
    </div>
    </div>
</asp:Content>