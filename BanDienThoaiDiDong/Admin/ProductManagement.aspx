<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductManagement.aspx.cs" Inherits="BanDienThoaiDiDong.ProductManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/productManagement.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="product-management">
        <div class="page-header">
            <h1>Quản lý sản phẩm</h1>
            <asp:Button ID="btnAddNew" runat="server" Text="Thêm sản phẩm mới" CssClass="btn-add" OnClick="btnAddNew_Click" />
        </div>

        <div class="filter-section">
            <div class="search-box">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Tìm kiếm sản phẩm..." />
                <asp:Button ID="btnSearch" runat="server" CssClass="my-search-product-btn" OnClick="btnSearch_Click" Text="Tìm kiếm"/>
            </div>
            <div class="filter-box">
                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                    <asp:ListItem Text="Tất cả danh mục" Value="" />
                </asp:DropDownList>
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    <asp:ListItem Text="Tất cả trạng thái" Value="" />
                    <asp:ListItem Text="Đang bán" Value="1" />
                    <asp:ListItem Text="Ngừng bán" Value="0" />
                </asp:DropDownList>
            </div>
        </div>

        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" 
            CssClass="grid-view" AllowPaging="True" PageSize="10"
            OnPageIndexChanging="gvProducts_PageIndexChanging"
            OnRowCommand="gvProducts_RowCommand"
            DataKeyNames="ProductID">
            <Columns>
                <asp:BoundField DataField="ProductID" HeaderText="ID" />
                <asp:TemplateField HeaderText="Hình ảnh">
                    <ItemTemplate>
                        <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("DefaultImageUrl") %>' CssClass="product-img" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm" />
                <asp:BoundField DataField="CategoryName" HeaderText="Danh mục" />
                <asp:BoundField DataField="OriginalPrice" HeaderText="Giá gốc" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="SalePrice" HeaderText="Giá bán" DataFormatString="{0:N0}₫" />
                <asp:BoundField DataField="Stock" HeaderText="Tồn kho" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="Sold" HeaderText="Đã bán" />
                <asp:TemplateField HeaderText="Trạng thái">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" 
                            Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Đang bán" : "Ngừng bán" %>'
                            CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "status-active" : "status-inactive" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <div class="action-buttons">
                            <asp:Button ID="btnEdit" runat="server" CssClass="btn-edit"
                                CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>'
                                Text="Sửa" />
                            <asp:Button ID="btnDelete" runat="server" CssClass="btn-delete"
                                CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>'
                                Text="Xóa"
                                OnClientClick="return confirm('Bạn có chắc muốn xóa sản phẩm này?');" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>