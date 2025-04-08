<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="OrderStatusUpdate.aspx.cs" Inherits="BanDienThoaiDiDong.OrderStatusUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/orderManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-status-update">
        <h2>Cập nhật trạng thái đơn hàng #<asp:Label ID="lblOrderId" runat="server" /></h2>
        
        <div class="order-info">
            <div class="info-group">
                <label>Khách hàng:</label>
                <asp:Label ID="lblCustomerName" runat="server" />
            </div>
            <div class="info-group">
                <label>Số điện thoại:</label>
                <asp:Label ID="lblPhone" runat="server" />
            </div>
            <div class="info-group">
                <label>Tổng tiền:</label>
                <asp:Label ID="lblTotalAmount" runat="server" />
            </div>
        </div>

        <div class="status-update-form">
            <div class="form-group">
                <label>Trạng thái hiện tại:</label>
                <asp:Label ID="lblCurrentStatus" runat="server" CssClass="current-status" />
            </div>
            <div class="form-group">
                <label>Trạng thái mới:</label>
                <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Chờ xác nhận" Value="Chờ xác nhận" />
                    <asp:ListItem Text="Đang giao" Value="Đang giao" />
                    <asp:ListItem Text="Đã giao" Value="Đã giao" />
                    <asp:ListItem Text="Đã hủy" Value="Đã hủy" />
                </asp:DropDownList>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" CssClass="btn-update" />
                <asp:Button ID="btnCancel" runat="server" Text="Quay lại" OnClick="btnCancel_Click" CssClass="btn-cancel" />
            </div>
        </div>
    </div>
</asp:Content>