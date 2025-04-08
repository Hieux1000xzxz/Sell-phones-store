<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BanDienThoaiDiDong.Login" %>
<asp:Content ID="Content0" ContentPlaceHolderID="dynamicTitle" runat="server">
    Đăng nhập - THT
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="auth-wrapper">
    <div class="auth-container">
        <div class="auth-header">
            <h2>Đăng nhập</h2>
            <p>Chào mừng bạn quay trở lại!</p>
        </div>

        <div class="auth-form">
            <div class="form-group">
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail">Email</asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" 
                    TextMode="Email" 
                    placeholder="Nhập email của bạn"
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail"
                    ErrorMessage="Vui lòng nhập email"
                    Display="Dynamic"
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword">Mật khẩu</asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" 
                    TextMode="Password" 
                    placeholder="Nhập mật khẩu"
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword"
                    ErrorMessage="Vui lòng nhập mật khẩu"
                    Display="Dynamic"
                    CssClass="error-message">
                </asp:RequiredFieldValidator>
            </div>

            <asp:Button ID="btnLogin" runat="server" 
                Text="Đăng nhập" 
                CssClass="submit-btn"
                OnClick="btnLogin_Click"/>
        </div>

        <div class="auth-links">
            <asp:HyperLink ID="lnkRegister" runat="server" 
                NavigateUrl="~/Register.aspx" 
                CssClass="switch-auth">
                Chưa có tài khoản? Đăng ký ngay
            </asp:HyperLink>
            
            <asp:HyperLink ID="lnkHome" runat="server" 
                NavigateUrl="~/Default.aspx" 
                CssClass="back-home">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                </svg>
                Quay về trang chủ
            </asp:HyperLink>
        </div>
    </div>
    </div>
</asp:Content>