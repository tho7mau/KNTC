<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnPhienHop_GoiThongBao.ascx.cs" Inherits="HOPKHONGGIAY.CnPhienHop_GoiThongBao" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>

<script type="text/javascript" src="<%=vPathCommonJS%>"></script>

<%=vJavascriptMask %>
<script>
    function Isdisplay() {
        document.getElementById("filedk-tab").click();
        var content_CBM = document.getElementById("cbm");
        var content_File = document.getElementById("filedk");
        content_CBM.className = content_CBM.className.replace("active", "");
        content_File.className = content_File.className.replace("tab-pane fade", "tab-pane fade active in");
    }
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }
</script>

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
        <div>
            <div id="modalPopUp" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">
                                <span id="spnTitle"></span>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                <span id="spnMsg"></span>.
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <button type="button" id="btnConfirm" class="btn btn-danger">
                                Có</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    

        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonThemmoi" Visible="false" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="false" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" Visible="true" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="glyphicon glyphicon-send"></i> Gửi thông báo</asp:LinkButton>
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
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTieuDe">Tiêu đề </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textTieuDe" runat="server" CssClass="form-control requirements" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNoiDung">Nội dung</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textNoidung" runat="server" CssClass="form-control requirements" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6 col-md-6 col-lg-6" >
                                  <div class="form-group mr-t10" runat="server" id="divGoiNgay" visible="true">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="lblTaiKhoan">Gửi ngay</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:CheckBox ID="checkboxGoiNgay" Checked="false" runat="server" CssClass="mr-l10" Text=" " AutoPostBack="true" OnCheckedChanged="CheckboxGoiNgay_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10" runat="server" id="divNgayGoi" visible="true">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNgayGoi">Thời gian Gửi</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">  
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianGoi" TimeView-HeaderText="Giờ gửi thông báo" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="00:00:00" TimeView-EndTime="23:00:01" TimeView-Columns="3" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </telerik:RadDateTimePicker>

                                    </div>
                                </div>                                
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

    button.btn-primary {
        color: #ffffff;
        background-color: #ff6600 !important;
        border-color: #ff6600 !important;
        border-radius: 4px !important;
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
