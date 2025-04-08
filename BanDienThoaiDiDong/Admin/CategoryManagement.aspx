<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CategoryManagement.aspx.cs" Inherits="BanDienThoaiDiDong.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/categoryManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="category-management">
        <h1>Quản lý danh mục</h1>

        <div class="add-category">
            <div class="form-group">
                <label for="txtCategoryName">Tên danh mục:</label>
                <asp:TextBox ID="txtCategoryName" runat="server" placeholder="Nhập tên danh mục"/>
            </div>
            <div class="form-group">
                <label for="txtDescription">Mô tả:</label>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" placeholder="Nhập mô tả" />
            </div>
            <div class="button-group">
                <asp:Button ID="btnAdd" runat="server" Text="Thêm danh mục" OnClick="btnAdd_Click" CssClass="btn-add" />
            </div>
        </div>

        <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False"
            CssClass="grid-view" DataKeyNames="CategoryID"
            OnRowEditing="gvCategories_RowEditing"
            OnRowUpdating="gvCategories_RowUpdating"
            OnRowCancelingEdit="gvCategories_RowCancelingEdit"
            OnRowDeleting="gvCategories_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CategoryID" HeaderText="ID" ReadOnly="true" />
                <asp:TemplateField HeaderText="Tên danh mục">
                    <ItemTemplate><%# Eval("CategoryName") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("CategoryName") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mô tả">
                    <ItemTemplate><%# Eval("Description") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditDesc" runat="server" Text='<%# Bind("Description") %>' TextMode="MultiLine" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Sửa" CommandName="Edit" CssClass="btn-edit" />
                        <asp:Button ID="btnDelete" runat="server" Text="Xóa" CommandName="Delete"
                            CssClass="btn-delete" OnClientClick="return confirm('Xóa danh mục này?');" />
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