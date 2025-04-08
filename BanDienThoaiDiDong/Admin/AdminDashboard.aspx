<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="BanDienThoaiDiDong.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard">
        <div class="page-header">
            <h1>Tổng quan hệ thống</h1>
            <p class="timestamp">Cập nhật lúc: <asp:Label ID="lblLastUpdate" runat="server" /></p>
        </div>

        <div class="stats-grid">
            <div class="stat-card revenue">
                <div class="stat-icon">
                    <i class="fas fa-dollar-sign"></i>
                </div>
                <div class="stat-info">
                    <h3>Tổng doanh thu</h3>
                    <asp:Label ID="lblTotalRevenue" runat="server" CssClass="stat-value" />
                </div>
            </div>

            <div class="stat-card orders">
                <div class="stat-icon">
                    <i class="fas fa-shopping-bag"></i>
                </div>
                <div class="stat-info">
                    <h3>Đơn hàng mới</h3>
                    <asp:Label ID="lblNewOrders" runat="server" CssClass="stat-value" />
                </div>
            </div>

            <div class="stat-card products">
                <div class="stat-icon">
                    <i class="fas fa-box"></i>
                </div>
                <div class="stat-info">
                    <h3>Sản phẩm tồn kho</h3>
                    <asp:Label ID="lblTotalStock" runat="server" CssClass="stat-value" />
                </div>
            </div>

            <div class="stat-card users">
                <div class="stat-icon">
                    <i class="fas fa-users"></i>
                </div>
                <div class="stat-info">
                    <h3>Tổng người dùng</h3>
                    <asp:Label ID="lblTotalUsers" runat="server" CssClass="stat-value" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>