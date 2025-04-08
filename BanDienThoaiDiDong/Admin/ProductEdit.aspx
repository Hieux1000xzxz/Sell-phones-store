<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="BanDienThoaiDiDong.ProductEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/productEdit.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="product-edit">
        <div class="page-header">
            <h1><asp:Literal ID="ltTitle" runat="server" /></h1>
            <asp:Button ID="btnBack" runat="server" Text="Quay lại" CssClass="btn-back" OnClick="btnBack_Click" CausesValidation="false" />
        </div>

        <div class="edit-form">
            <!-- Thông tin cơ bản -->
            <div class="section-title">Thông tin cơ bản</div>
            <div class="form-group">
                <label for="txtProductName">Tên sản phẩm:</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                    ControlToValidate="txtProductName" 
                    ErrorMessage="Vui lòng nhập tên sản phẩm" 
                    Display="Dynamic" CssClass="error-message" />
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label for="ddlCategory">Danh mục:</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server"
                        ControlToValidate="ddlCategory"
                        ErrorMessage="Vui lòng chọn danh mục"
                        Display="Dynamic" CssClass="error-message" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label for="txtOriginalPrice">Giá gốc:</label>
                    <asp:TextBox ID="txtOriginalPrice" runat="server" CssClass="form-control" TextMode="Number" />
                    <asp:RequiredFieldValidator ID="rfvOriginalPrice" runat="server"
                        ControlToValidate="txtOriginalPrice"
                        ErrorMessage="Vui lòng nhập giá gốc"
                        Display="Dynamic" CssClass="error-message" />
                </div>

                <div class="form-group">
                    <label for="txtSalePrice">Giá bán:</label>
                    <asp:TextBox ID="txtSalePrice" runat="server" CssClass="form-control" TextMode="Number" />
                    <asp:RequiredFieldValidator ID="rfvSalePrice" runat="server"
                        ControlToValidate="txtSalePrice"
                        ErrorMessage="Vui lòng nhập giá bán"
                        Display="Dynamic" CssClass="error-message" />
                </div>

                <div class="form-group" style="display: none;">
                    <label for="txtStock">Số lượng tồn:</label>
                    <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" />
                    <asp:RequiredFieldValidator ID="rfvStock" runat="server"
                        ControlToValidate="txtStock"
                        ErrorMessage="Vui lòng nhập số lượng"
                        Display="Dynamic" CssClass="error-message" />
                </div>
            </div>

            <div class="form-group">
                <label for="txtDescription">Mô tả sản phẩm:</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
            </div>

            <div class="form-actions">
                <asp:Button ID="btnSaveBasic" runat="server" Text="Lưu thông tin" CssClass="btn-save" OnClick="btnSaveBasic_Click" />
            </div>

            <!-- Thông số kỹ thuật -->
            <div class="section-title">Thông số kỹ thuật</div>
            <div class="specs-container">
                <asp:Repeater ID="rptSpecs" runat="server">
                    <ItemTemplate>
                        <div class="spec-item">
                            <asp:HiddenField ID="hdnSpecId" runat="server" Value='<%# Eval("SpecID") %>' />
                            <asp:TextBox ID="txtSpecName" runat="server" CssClass="form-control" placeholder="Tên thông số" Text='<%# Eval("SpecName") %>' />
                            <asp:TextBox ID="txtSpecValue" runat="server" CssClass="form-control" placeholder="Giá trị" Text='<%# Eval("SpecValue") %>' />
                            <asp:Button ID="btnSaveSpec" runat="server" Text="Lưu" CssClass="btn-save" OnClick="btnSaveSpec_Click" CommandArgument='<%# Eval("SpecID") %>' />
                            <asp:Button ID="btnRemoveSpec" runat="server" Text="Xóa" CssClass="btn-remove" OnClick="btnRemoveSpec_Click" CommandArgument='<%# Eval("SpecID") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Button ID="btnAddSpec" runat="server" Text="Thêm thông số" CssClass="btn-add-spec" OnClick="btnAddSpec_Click" CausesValidation="false"/>
            </div>

            <!-- Biến thể sản phẩm -->
            <div class="section-title">Biến thể sản phẩm</div>
            <div class="variants-container">
                <asp:Repeater ID="rptVariants" runat="server">
                    <ItemTemplate>
                        <div class="variant-item" id='<%# "variant-" + Eval("VariantID") %>'>
                            <asp:HiddenField ID="hdnVariantId" runat="server" Value='<%# Eval("VariantID") %>' />
                            <div class="form-row">
                                <div class="form-group">
                                    <label>Màu sắc:</label>
                                    <div class="color-input-group">
                                        <asp:TextBox ID="txtColor" runat="server" CssClass="form-control" Text='<%# Eval("Color") %>' />
                                        <asp:TextBox ID="txtColorCode" runat="server" CssClass="form-control color-picker"
                                            Text='<%# Eval("ColorCode") %>' type="color" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Dung lượng:</label>
                                    <asp:TextBox ID="txtStorage" runat="server" CssClass="form-control" Text='<%# Eval("Storage") %>' />
                                </div>
                                <div class="form-group">
                                    <label>Giá:</label>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Eval("Price") %>' />
                                </div>
                                <div class="form-group">
                                    <label>Số lượng:</label>
                                    <asp:TextBox ID="txtVariantStock" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Eval("Stock") %>' />
                                </div>
                                <div class="form-group">
                                    <label>Hình ảnh:</label>
                                    <asp:FileUpload ID="fuVariantImage" runat="server" CssClass="form-control" />
                                    <asp:Image ID="imgVariant" runat="server" CssClass="preview-image" 
                                                ImageUrl='<%# Eval("VarImageUrl")?.ToString() ?? "" %>' 
                                                Visible='<%# !string.IsNullOrEmpty(Eval("VarImageUrl")?.ToString()) %>' />
                                </div>
                                <div class="variant-actions">
                                    <asp:Button ID="btnSaveVariant" runat="server" Text="Lưu biến thể" 
                                        CssClass="btn-save" OnClick="btnSaveVariant_Click" 
                                        CommandArgument='<%# Eval("VariantID") %>' />
                                    <asp:Button ID="btnDeleteVariant" runat="server" Text="Xóa biến thể" 
                                        CssClass="btn-remove" OnClick="btnDeleteVariant_Click"
                                        CommandArgument='<%# Eval("VariantID") %>'
                                        OnClientClick="return confirm('Bạn có chắc muốn xóa biến thể này?');" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Button ID="btnAddVariant" runat="server" Text="Thêm biến thể" CssClass="btn-add-variant" OnClick="btnAddVariant_Click" CausesValidation="false"/>
            </div>

            <!-- Hình ảnh sản phẩm -->
            <div class="section-title">Hình ảnh sản phẩm</div>
            <div class="form-group">
                <label for="fuMainImage">Hình ảnh chính:</label>
                <asp:FileUpload ID="fuMainImage" runat="server" CssClass="form-control" />
                <asp:Image ID="imgMain" runat="server" CssClass="preview-image" Visible="false" />
            </div>

            <div class="form-group">
                <label>Trạng thái:</label>
                <asp:CheckBox ID="chkIsActive" runat="server" Text="Đang bán" Checked="true" />
            </div>

            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
        </div>
    </div>
</asp:Content>