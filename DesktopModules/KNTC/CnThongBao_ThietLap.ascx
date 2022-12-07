<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnThongBao_ThietLap.ascx.cs" Inherits="HOPKHONGGIAY.CnThongBao_ThietLap" %>
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
                    <asp:LinkButton ID="btnCapNhat" runat="server" CausesValidation="false" Visible="true" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
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
                            <div class="col-sm-9 col-md-9 col-lg-9 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-6 control-label pd-r0" runat="server" id="labelTenChucVu">Gửi thông báo ứng dụng</label>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <%--<asp:TextBox ID="textTenChucVu" runat="server" CssClass="form-control requirements" MaxLength="500" />--%>
                                        <label class="switch">
                                            <input type="checkbox" class="pushclass" runat="server" id="cboxThongBao" onchange="PushProcess()">
                                            <span class="slider round"></span>
                                        </label>

                                    </div>

                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-6 control-label pd-r0" runat="server" id="label1">Gửi tin nhắn SMS </label>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <label class="switch">
                                            <input type="checkbox" class="smsclass" runat="server" id="cboxSMS" onchange="SMSProcess()">
                                            <span class="slider round"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row box9 divpush" runat="server" id="divPushNotification">
                    <div class="heading-chitiet">CẤU HÌNH PUSH NOTIFICATION</div>
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelOneSignalAppID">OneSignal App ID</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textOneSignalAppId" runat="server" CssClass="form-control requirements" TextMode="MultiLine" Rows="2"/>
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelOneSignalRestApiKey">OneSignal Rest API Key</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textOneSignalRestApiKey" runat="server" CssClass="form-control requirements" TextMode="MultiLine" Rows="2" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row box9 divsms" runat="server" id="divSMS">
                    <div class="heading-chitiet">CẤU HÌNH SMS</div>
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelApiUser">Api User</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textApiUser" runat="server" CssClass="form-control requirements" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelApiPassword">Api Password</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textApiPassword" runat="server" CssClass="form-control requirements"  />
                                    </div>

                                </div>

                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelApiCode">Api CPCode</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textApiCode" runat="server" CssClass="form-control requirements" MaxLength="500" />
                                    </div>

                                </div>

                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">

                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelRequestID">Api RequestID </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textRequestID" runat="server" CssClass="form-control requirements" />
                                    </div>
                                </div>

                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelServiceID">Api ServiceID </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textServiceID" runat="server" CssClass="form-control requirements" MaxLength="500" />
                                    </div>
                                </div>

                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelCommandCode">Api CommandCode </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textCommandCode" runat="server" CssClass="form-control requirements" MaxLength="500" />
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

    .switch {
        position: relative;
        display: inline-block;
        width: 50px;
        height: 25px;
    }

        .switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }

    .slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        -webkit-transition: .4s;
        transition: .4s;
    }

        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 4px;
            bottom: 4px;
            background-color: white;
            -webkit-transition: .4s;
            transition: .4s;
        }

    input:checked + .slider {
        background-color: #ff6600;
    }

    input:focus + .slider {
        box-shadow: 0 0 1px #ff6600;
    }

    input:checked + .slider:before {
        -webkit-transform: translateX(26px);
        -ms-transform: translateX(26px);
        transform: translateX(26px);
    }

    /* Rounded sliders */
    .slider.round {
        border-radius: 34px;
    }

        .slider.round:before {
            border-radius: 50%;
        }
</style>
<script>
    function pageLoad(sender, args) {
        PushProcess();
        SMSProcess();
    }
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
    function SMSProcess() {
        if ($('.smsclass').is(':checked')) {
            $('.divsms').show();
        }
        else {
            $('.divsms').hide();
        }
    }
    function PushProcess() {
        if ($('.pushclass').is(':checked')) {
            $('.divpush').show();
        }
        else {
            $('.divpush').hide();
        }
    }  
</script>
