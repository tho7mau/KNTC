<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnPhongHop.ascx.cs" Inherits="HOPKHONGGIAY.CnPhongHop" %>
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
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" Visible="false" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btnBoQua_Click" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>
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
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTenPhongHop">Tên phòng họp </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textTenPhongHop" runat="server" CssClass="form-control requirements" MaxLength="500" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelSucChua" class="col-sm-3 control-label pd-r0">Sức chứa</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 focused">
                                        <asp:TextBox ID="textSucChua" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control requirements" MaxLength="3" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label runat="server" id="labelBenhNhan" class="col-sm-3 control-label pd-r0">Chọn thiết bị</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9  focused">
                                        <%--<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>--%>
                                        <telerik:RadComboBox Skin="Simple" ID="ddlistThietBi" Filter="Contains" CssClass="custom-radcombox" AutoPostBack="true"
                                            InputCssClass="form-control requirements" runat="server" Width="100%" Height="200px" EnableCheckAllItemsCheckBox="true"
                                            EmptyMessage="-- Chọn thiết bị --" ShowWhileLoading="true" CheckBoxes="true" CausesValidation="false" EnableLoadOnDemand="true" OnItemDataBound="ddlistThietBi_ItemDataBound"
                                            Localization-ItemsCheckedString="thiết bị được chọn" Localization-AllItemsCheckedString="Chọn tất cả" Localization-CheckAllString="Tất cả thiết bị"
                                            LoadingMessage="Đang tải..." Localization-NoMatches="Không tìm thấy" OnCheckAllCheck="">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="label1">Diễn giải </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textDienGiai" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" MaxLength="1000" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30" runat="server" id="divSoDo">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="label2">Sơ đồ phòng họp </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:FileUpload ID="f_SoDoPhongHop" Enabled="true" onchange="readURL(this);" runat="server" CssClass="" accept=".svg" AllowMultiple="false" Style="float: left;" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12 pd-l30 pd-r30" style="overflow-x:auto">
                                <asp:LinkButton ID="buttonXoaHinhAnh" runat="server" Visible="false" CausesValidation="false" OnClick="XoaHinhAnh" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-delete"></i> Xoá</asp:LinkButton>
                               <%-- <img id="blah" src="#" alt="img" style="display: none;"  width="500"/>--%>
                                <object  data="/DesktopModules/HOPKHONGGIAY/sodophong_a3.svg" type="image/svg+xml"  style="display: none; width:100%"   id="blah">
                                 <%--<img id="blah" src="/DesktopModules/HOPKHONGGIAY/Upload/PHONGHOP/sodophong.svg" alt="img" style="display: none;"  width="500"  width="500"/>--%>
                                <%--<img id="blah2" src="/DesktopModules/HOPKHONGGIAY/Upload/PHONGHOP/bvtimmachangiang.jpg" alt="img" />--%>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnCapNhat" />
    </Triggers>
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
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#blah')
                    .attr('data', e.target.result)
                    .attr('style', 'display:block')
                    .width((screen.width * 60) / 100);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }

    function setSrcImage(a) {
        $('#blah').attr("data", a);
        $('#blah').attr("style", "display: block");
    }

    function setEmptyImage() {
        $('#blah').attr("data", "");
        $('#blah').attr("style", "display: none");
    }


</script>
