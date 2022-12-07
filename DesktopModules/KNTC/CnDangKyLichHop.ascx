<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnDangKyLichHop.ascx.cs" Inherits="HOPKHONGGIAY.CnDangKyLichHop" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>
<chosen:chosenjs runat="server" />
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

        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="buttonTimKiem">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonTimKiem" runat="server" OnClick="buttonTimKiem_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-search"></i> Tìm kiếm</asp:LinkButton>
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
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelThoiGianBatDau">Thời gian bắt đầu</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianBatDau" TimeView-HeaderText="Giờ bắt đầu" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelSoNguoi" class="col-sm-3 control-label pd-r0">Số người tham dự</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8 focused">
                                        <asp:TextBox ID="textSoNguoi" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control requirements" MaxLength="3" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">

                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelThoiGianKetThuc" class="col-sm-3 control-label pd-r0">Thời gian kết thúc </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianKetThuc" TimeView-HeaderText="Giờ kết thúc" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>

                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label runat="server" id="labelBenhNhan" class="col-sm-3 control-label pd-r0">Chọn thiết bị</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8  focused">
                                        <telerik:RadComboBox Skin="Simple" ID="ddlistThietBi" Filter="Contains" CssClass="custom-radcombox" AutoPostBack="true"
                                            InputCssClass="form-control requirements" runat="server" Width="100%" Height="200px" EnableCheckAllItemsCheckBox="true"
                                            EmptyMessage="-- Chọn thiết bị --" ShowWhileLoading="true" CheckBoxes="true" CausesValidation="false" EnableLoadOnDemand="true" OnItemDataBound="ddlistThietBi_ItemDataBound"
                                            Localization-ItemsCheckedString="thiết bị được chọn" Localization-AllItemsCheckedString="Chọn tất cả" Localization-CheckAllString="Tất cả thiết bị"
                                            LoadingMessage="Đang tải..." Localization-NoMatches="Không tìm thấy" OnCheckAllCheck="">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlPhongDaChon" runat="server" Visible="true" CssClass="body_PhongDaChon">
                                 <div class="col-sm-12 pd-l30 title_KetQua">
                                   <span>PHÒNG ĐÃ CHỌN</span>
                                </div>
                              
                                <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                    <div class="body_Phong">
                                        <div class="col-md-4">
                                            <asp:Image runat="server" ImageUrl="/DesktopModules/HOPKHONGGIAY/Images/icon_home.png" Style="width: 80%;"></asp:Image>
                                        </div>
                                        <div class="col-md-8">
                                           <p> <asp:Label runat="server" ID="textTenPhongHop" Font-Size="16px" /> </p>
                                           <p> Sức chứa : <asp:Label runat="server" ID="textSucChua" /></p>
                                          <p>Thiết bị :  <asp:Label runat="server" ID="textThietBi" /></p>
                                        </div>
                                        <a  onserverclick="XoaPhong" title="Xóa phòng đã chọn" onclick="return getConfirmation(this, 'XÓA PHÒNG','Bạn muốn xóa dữ liệu này?');" oncontextmenu="return false" runat="server" class="btn btn-danger waves-effect none-radius none-shadow" style="width: 100%">Xóa</a>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>

                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <%--   <asp:Label ID="lblKetQua" runat="server" Text="Không tìm thấy phòng hợp nào phù hợp yêu cầu" style="color:red" Visible="false"/>--%>
                            <asp:Panel runat="server" ID="pnlKetQuaTimKiem" Style="margin-top: 50px" Visible="false">
                                <div class="col-sm-12 pd-l30 title_KetQua">
                                    <span>KẾT QUẢ TÌM KIẾM</span>
                                </div>
                                <asp:ListView ID="ListView_PHONG" runat="server">
                                    <ItemTemplate>
                                        <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                            <div class="body_Phong">
                                                <div class="col-md-4">
                                                    <asp:Image runat="server" ImageUrl="/DesktopModules/HOPKHONGGIAY/Images/icon_home.png" Style="width: 80%;"></asp:Image>
                                                </div>
                                                <div class="col-md-8">
                                                    <p style="font-size: 16px"><%# Eval("TENPHONGHOP")%></p>
                                                    <p>Sức chứa:  <%# Eval("SUCCHUA")%></p>
                                                    <p>Thiết bị:  <%# GetDanhSachTenThietBi(int.Parse(DataBinder.Eval(Container, "DataItem.PHONGHOP_ID").ToString())) %></p>
                                                    <asp:Literal runat="server" ID="labelLichTrung" text='<%# Eval("LICHHOP").ToString() %>'></asp:Literal>
                                                </div>
                                                <a  onserverclick="DangKyPhong" title="Chọn phòng cho phiên họp" href='<%# Eval("PHONGHOP_ID").ToString() %>' oncontextmenu="return false" runat="server" class='btn btn-primary waves-effect none-radius none-shadow' style='<%# Eval("DANGKY").ToString() + "; width: 100%" %>'>Đăng ký</a>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
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

    .title_KetQua {
        /*margin-top:50px;*/
        font-size: 16px;
        float: left;
        margin-bottom: 15px;
    }

    .body_Phong {
        padding: 20px 20px 20px 20px;
        box-shadow: 0 2px 8px -2px black;
        margin-bottom: 20px;
    }
    .body_PhongDaChon{
        padding-top:30px;
        float:left;
    }
</style>

<script type="text/javascript">
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }
    function ConfirmDelete() {
        return confirm("Bạn muốn xóa dữ liệu này ?");
    }
   
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
