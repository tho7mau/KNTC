<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnKhachMoi_DoiMatKhau.ascx.cs" Inherits="HOPKHONGGIAY.CnKhachMoi_DoiMatKhau" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<%=vJavascriptMask %>
<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/DesktopModules/HOPKHONGGIAY/Images/ajax-loader.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upn" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="glyphicon glyphicon-floppy-disk"></i> Thêm mới</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Cập nhật</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btnBoQua_Click" CausesValidation="false"><i class='icofont-undo'></i> Bỏ qua</asp:LinkButton>
                </div>
            </div>
            <div class="row bl">
                <div class="col-md-10">
                    <div class="col-sm-offset-4 col-sm-6 col-md-7 col-lg-6 pd-l30 pd-r30">
                        <asp:ValidationSummary ID="ValidationSummary1" CssClass="baoloi" runat="server" EnableClientScript="true" />
                        <asp:Panel CssClass="baoloi" runat="server" ID="pnlThongBao" Visible="false">
                            <asp:Label ID="lblThongBao" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div class="row box-body">
                <div class="row box9">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTenDangNhap">Tên đăng nhập </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textTenDangNhap" runat="server" Enabled="false" CssClass="form-control requirements" MaxLength="50" />
                                    </div>
                                </div>

                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelHoTen">Họ và tên</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textHoTenNguoiDung" runat="server" Enabled="false" CssClass="form-control requirements" MaxLength="50" />
                                    </div>
                                </div>
                               
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                 <div class="form-group mr-t10">
                                    <label runat="server" id="labelMatKhau" class="col-sm-3 control-label pd-r0">Mật khẩu mới</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 focused">
                                        <asp:TextBox ID="textMatKhau" TextMode="Password" runat="server" CssClass="form-control requirements" MaxLength="20" />
                                    </div>
                                </div>

                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelXacNhanMatKhau" class="col-sm-3 control-label pd-r0">Xác nhận mật khẩu mới</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 focused">
                                        <asp:TextBox ID="textXacNhanMatKhau" TextMode="Password" runat="server" CssClass="form-control requirements" MaxLength="20" />
                                    </div>
                                </div>

                                <%--<div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelDonVi">Đơn vị</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <asp:DropDownList ID="ddlistDonVi" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelChucVu" class="col-sm-3 control-label pd-r0">Chức vụ </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9" style="text-align: center">
                                        <asp:DropDownList ID="ddlistChucVu" runat="server" CssClass="form-control requirements slPhongBan">
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<style>
    .form_radiobuttonlist label {
        margin-top: 5px;
    }
</style>
<script>
    //function pageLoad(sender, args) {
    //       if (args._isPartialLoad) { // postback
    //           $('input.auto').autoNumeric('update');
    //       }
    //       else { // not postback
    //           $('input.auto').autoNumeric();
    //       }
    //   }
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
