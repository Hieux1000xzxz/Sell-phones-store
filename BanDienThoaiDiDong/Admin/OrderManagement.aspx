<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="BanDienThoaiDiDong.OrderManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/orderManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-management">
        <div class="page-header">
            <h1>Quản lý đơn hàng</h1>
        </div>

        <div class="filter-section">
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Tìm kiếm theo mã đơn hàng..." />
                <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" CssClass="btn-search" />
            </div>
            <div class="filter-box">
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    <asp:ListItem Text="Tất cả trạng thái" Value="" />
                    <asp:ListItem Text="Chờ xác nhận" Value="Chờ xác nhận" />
                    <asp:ListItem Text="Đã xác nhận" Value="Đã xác nhận" />
                    <asp:ListItem Text="Đang giao" Value="Đang giao" />
                    <asp:ListItem Text="Đã giao" Value="Đã giao" />
                    <asp:ListItem Text="Đã hủy" Value="Đã hủy" />
                </asp:DropDownList>
                <asp:DropDownList ID="ddlDateFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDateFilter_SelectedIndexChanged">
                    <asp:ListItem Text="Tất cả thời gian" Value="" />
                    <asp:ListItem Text="Hôm nay" Value="today" />
                    <asp:ListItem Text="7 ngày qua" Value="week" />
                    <asp:ListItem Text="30 ngày qua" Value="month" />
                </asp:DropDownList>
            </div>
        </div>
        
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False"
            CssClass="grid-view" AllowPaging="True" PageSize="10"
            OnPageIndexChanging="gvOrders_PageIndexChanging"
            OnRowCommand="gvOrders_RowCommand"
            DataKeyNames="OrderID">
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Mã đơn" />
                <asp:BoundField DataField="CustomerName" HeaderText="Khách hàng" />
                <asp:BoundField DataField="Phone" HeaderText="Số điện thoại" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Tổng tiền" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="OrderDate" HeaderText="Ngày đặt" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:TemplateField HeaderText="Trạng thái">
                    <ItemTemplate>
                        <asp:Panel ID="pnlStatus" runat="server" CssClass="status-panel">
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                CssClass='<%# "status-" + GetStatusClass(Eval("Status").ToString()) %>' />
                            <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="status-dropdown" Visible="false"
                                OnSelectedIndexChanged="ddlOrderStatus_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Đã xác nhận" Value="Đã xác nhận" />
                                <asp:ListItem Text="Đang giao" Value="Đang giao" />
                                <asp:ListItem Text="Đã giao" Value="Đã giao" />
                                <asp:ListItem Text="Đã hủy" Value="Đã hủy" />
                            </asp:DropDownList>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnView" runat="server" Text="Xem chi tiết"
                                CommandName="ViewOrder" CommandArgument='<%# Eval("OrderID") %>'
                                CssClass="btn-view" />
                            <asp:Button ID="btnEdit" runat="server" Text="Sửa trạng thái"
                                CommandName="EditStatus" CommandArgument='<%# Eval("OrderID") %>'
                                CssClass="btn-edit" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>