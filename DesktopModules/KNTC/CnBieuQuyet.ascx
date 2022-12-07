<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnBieuQuyet.ascx.cs" Inherits="HOPKHONGGIAY.CnBieuQuyet" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>

<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>
<chosen:chosenjs runat="server" />

<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<%=vJavascriptMask %>
<script>
    function pageLoad(sender, args) {
        initchosen();
    }
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
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="true" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" Visible="false" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
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
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNoiDung">Nội dung biểu quyết</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textNoiDung" runat="server" CssClass="form-control requirements" TextMode="MultiLine" Rows="3" MaxLength="500" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTGBatDau">Thời gian bắt đầu</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerTGBatDau" TimeView-HeaderText="Giờ bắt đầu" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control slPhongBan">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelPhienHop">Phiên họp</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                      <%--  <asp:DropDownList ID="ddlistPhienHop"  runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </asp:DropDownList>--%>

                                        <asp:DropDownList ID="ddlistPhienHop" runat="server" AutoPostBack="true" SelectionMode="Single" data-placeholder="Chọn phiên họp" CssClass="form-control requirements chosen-select slPhongBan">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelDienGiai">Diễn giải</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textDienGiai" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTGKetThuc">Thời gian kết thúc</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerTGKetThuc" TimeView-HeaderText="Giờ kết thúc" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control slPhongBan">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row box-body" runat="server" id="divCauHoi">
                <div class="row box9">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:Panel ID="pnlFormDanhSach" runat="server" CssClass="form mr-b10" DefaultButton="buttonSearch">
                                <div class="form-inline">
                                    <div class="col-right mr-b6">
                                        <asp:TextBox CssClass="form-control btn-sm" OnTextChanged="btnSearch_Click" Width="400" AutoPostBack="true" ID="textSearchContent" placeholder="Nhập từ khóa..." runat="server"></asp:TextBox>

                                        <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                                               <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                                        </asp:LinkButton>
                                    </div>
                                    <div class="col-left" runat="server">
                                        <asp:LinkButton ID="btn_ThemMoi" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                                        <div id="divShowBtnXoa" style="display: none;" class="fleft">
                                            <button type="button" class="btn-default btn-warning-delete btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" onclick="confirm_delete_rows_update('<%=btn_Xoa.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                                        </div>
                                        <div style="display: none;">
                                            <asp:LinkButton ID="btn_Xoa" OnClientClick="return getConfirmation(this, 'THÔNG TIN','Bạn chắc chắn muốn xóa những câu hỏi đã chọn?');" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="btn_Xoa_Click"><i class=""></i> Xóa</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>


                            <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
                                <asp:Panel CssClass="baoloi" runat="server" ID="Panel1" Visible="false">
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:DataGrid DataKeyField="CAUHOI_ID" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="True" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
                                    AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound">
                                    <HeaderStyle CssClass="tieude" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="STT" Visible="true">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="handle_checked_delete_all_rows(this,'divShowBtnXoa');" />
                                            </HeaderTemplate>
                                            <HeaderStyle Width="3%" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" Width="3%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" onclick="handle_checked_delete_row(this,'divShowBtnXoa');" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn HeaderText="CAUHOI_ID" DataField="CAUHOI_ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle />
                                        </asp:BoundColumn>


                                        <asp:TemplateColumn HeaderText="Câu hỏi" HeaderStyle-HorizontalAlign="Left" SortExpression="NOIDUNG">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <a id="dgChiTiet" onserverclick="dgDanhSach_Sua" title="Thông tin câu hỏi" href='<%# Eval("CAUHOI_ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.NOIDUNG") %></a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <asp:TemplateColumn HeaderText="Sửa" Visible="false">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a onserverclick="dgDanhSach_Sua" style='<%#kiemtra(int.Parse(DataBinder.Eval(Container, "DataItem.CAUHOI_ID").ToString())).ToString()=="True"?"display:none": "" %>' title="Cập nhật thông tin câu hỏi" class="icon-sua" href='<%# Eval("CAUHOI_ID").ToString() %>' oncontextmenu="return false" runat="server"></a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <asp:TemplateColumn HeaderText="Xóa" Visible="false">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a onserverclick="dgDanhSach_Xoa" title="Xóa câu hỏi" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa câu hỏi này?');" href='<%# Eval("CAUHOI_ID").ToString()%>' oncontextmenu="return false" runat="server"></a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999"></PagerStyle>
                                </asp:DataGrid>
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
</style>
<script>  
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
