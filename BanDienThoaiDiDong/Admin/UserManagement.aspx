<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="BanDienThoaiDiDong.UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/userManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="user-management">
        <h1>Quản lý tài khoản</h1>
        
        <div class="search-box">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Tìm kiếm theo tên, email hoặc số điện thoại..." />
            <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" OnClick="btnSearch_Click" CssClass="btn-search" />
        </div>

        <asp:GridView ID="gvUsers" runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="grid-view" 
                        AllowPaging="True" 
                        PageSize="10"
                        OnPageIndexChanging="gvUsers_PageIndexChanging"
                        OnRowEditing="gvUsers_RowEditing"
                        OnRowUpdating="gvUsers_RowUpdating"
                        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                        OnRowDeleting="gvUsers_RowDeleting"
                        DataKeyNames="UserID">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="true" />
                <asp:TemplateField HeaderText="Họ tên">
                    <ItemTemplate><%# Eval("FullName") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFullName" runat="server" Text='<%# Bind("FullName") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Số điện thoại">
                    <ItemTemplate><%# Eval("Phone") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPhone" runat="server" Text='<%# Bind("Phone") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Role" HeaderText="Vai trò" ReadOnly="true" />
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Sửa" CommandName="Edit" CssClass="btn-edit" />
                        <asp:Button ID="btnDelete" runat="server" Text="Xóa" CommandName="Delete" 
                            CssClass="btn-delete" OnClientClick="return confirm('Xóa tài khoản này?');"
                            Visible='<%# Eval("Role").ToString() != "Admin" %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnUpdate" runat="server" Text="Lưu" CommandName="Update" CssClass="btn-save" />
                        <asp:Button ID="btnCancel" runat="server" Text="Hủy" CommandName="Cancel" CssClass="btn-cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>