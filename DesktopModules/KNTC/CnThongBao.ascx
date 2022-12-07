<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnThongBao.ascx.cs" Inherits="HOPKHONGGIAY.CnThongBao" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>

<dnn:DnnJsInclude ID="FancytreeUIDeps" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery-ui-dependencies/jquery.fancytree.ui-deps.js" AddTag="false" />
<dnn:DnnJsInclude ID="Fancytree" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.js" AddTag="false" />
<dnn:DnnJsInclude ID="FancytreeFilter" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.filter.js" AddTag="false" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/skin-win8/ui.fancytree.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/custom-fanytree.css" />

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
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }

    function pageLoad(sender, args) {
        initchosen();
        $(".Chonnguoinhan").click(function () {
            $(":ui-fancytree").fancytree("destroy");
            Intfancytree("nguoinhan");
            $('#modalChonNguoiNhanThongBao').modal('show');
        });
    }
</script>

<script type="text/javascript">
    function Intfancytree(type) {
        var domainname = window.location.origin;
        var url_source = domainname + "/DesktopModules/HOPKHONGGIAY/API/ServiceHopKhongGiay/";
        if (type == "nguoinhan") {
            var NguoiNhans = $('#<%=lblNguoiNhanThongBao.ClientID %>').html();
            url_source = url_source + "GetNguoiNhanThongBao?nguoinhan=" + NguoiNhans;
            $("#treenguoinhan").fancytree({
                extensions: ["filter"],
                quicksearch: true,
                checkbox: true,
                selectMode: 3,
                source: {
                    url: url_source
                },
                filter: {
                    autoApply: true,   // Re-apply last filter if lazy data is loaded
                    autoExpand: true, // Expand all branches that contain matches while filtered
                    counter: true,     // Show a badge with number of matching child nodes near parent icons
                    fuzzy: false,      // Match single characters in order, e.g. 'fb' will match 'FooBar'
                    hideExpandedCounter: false,  // Hide counter badge if parent is expanded
                    hideExpanders: false,       // Hide expanders if all child nodes are hidden by filter
                    highlight: true,   // Highlight matches by wrapping inside <mark> tags
                    leavesOnly: false, // Match end nodes only
                    nodata: true,      // Display a 'no data' status node if result is empty
                    mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
                },
                select: function (event, data) {
                    var selKeys = $.map(data.tree.getSelectedNodes(), function (node) {
                        return node.key;
                    });
                    $('#<%=hiddenfiendNguoiNhan.ClientID %>').val(selKeys.join(","));
                },
            });
        }
        var tree;
        $("input[name=search]").on("keyup", function (e) {

            tree = $.ui.fancytree.getTree("#treenguoinhan");

            if ($(this).val() != "") {
                var n,
                    args = "autoApply autoExpand fuzzy hideExpanders highlight leavesOnly nodata".split(" "),
                    opts = {},
                    filterFunc = $("#branchMode").is(":checked") ? tree.filterBranches : tree.filterNodes,
                    match = $(this).val();
                $.each(args, function (i, o) {
                    opts[o] = $("#" + o).is(":checked");
                });
                opts.mode = $("#hideMode").is(":checked") ? "hide" : "dimm";

                if (e && e.which === $.ui.keyCode.ESCAPE || $.trim(match) === "") {
                    $("a#btnResetSearch").click();
                    return;
                }
                if ($("#regex").is(":checked")) {
                    // Pass function to perform match
                    n = filterFunc.call(tree, function (node) {
                        return new RegExp(match, "i").test(node.title);
                    }); //, opts
                } else {
                    // Pass a string to perform case insensitive matching
                    n = filterFunc.call(tree, match); //, opts
                }
                $("a.btnResetSearch").attr("disabled", false);
                $("span#matches").text("(" + n + " matches)");
            }
            else {
                var tree = $.ui.fancytree.getTree();
                $("span#matches").text("");
                tree.clearFilter();
                $("a.btnResetSearch").attr("disabled", true);
            }

        }).focus();
        $("a.btnResetSearch").click(function (e) {
            var tree = $.ui.fancytree.getTree();
            $("input[name=search]").val("");
            $("span#matches").text("");
            tree.clearFilter();
        }).attr("disabled", true);

        $("fieldset input:checkbox").change(function (e) {
            var id = $(this).attr("id"),
                flag = $(this).is(":checked");

            // Some options can only be set with general filter options (not method args):
            switch (id) {
                case "counter":
                case "hideExpandedCounter":
                    tree.options.filter[id] = flag;
                    break;
            }
            tree.clearFilter();
            $("input[name=search]").keyup();
        });
    }
    $(function () {
    });
    function HideModal() {
        $('#modalChonNguoiNhanThongBao').modal('hide');

    };
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

        <%--Modal chọn nhiều người nhận thông báo--%>
        <div id="modalChonNguoiNhanThongBao" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <span>Chọn người nhận thông báo</span>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form " style="position: relative;">
                            <div class="form-group mr-t10">
                                <input name="search" data-type="chutri" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." autocomplete="off" />
                            </div>
                            <a id="" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="treenguoinhan" name="selNodes">
                            <input id="hiddenfiendNguoiNhan" type="hidden" runat="server" value="" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <asp:Button Text="Chọn" ID="btnConfirmChonNguoiNhan" OnClientClick="HideModal()" CssClass="btn btn-danger" runat="server" OnClick="btnConfirmChonNguoiNhan_Click"></asp:Button>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--End modal chọn nhiều chủ trì--%>

        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonThemmoi" Visible="false" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="false" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" Visible="false" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="glyphicon glyphicon-send"></i> Gửi thông báo</asp:LinkButton>
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

                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group mr-t10" runat="server" id="divGoiNgay" visible="false">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="lblTaiKhoan">Gửi ngay</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:CheckBox ID="checkboxGoiNgay" Checked="false" runat="server" CssClass="mr-l10" Text=" " AutoPostBack="true" OnCheckedChanged="checkboxGoiNgay_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="form-group mr-t10" runat="server" id="divNgayGoi" visible="true">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNgayGoi">Thời gian gửi</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianGoi" TimeView-HeaderText="Giờ gửi thông báo" Width="100%" Calendar-EnableTheming="true"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="00:00:00" TimeView-EndTime="23:00:01" TimeView-Columns="3" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </telerik:RadDateTimePicker>

                                    </div>
                                </div>

                                <div class="form-group mr-t10" runat="server" id="divNguoiNhan" visible="false">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNguoiNhan">Người nhận</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <a runat="server" id="Chonnguoinhan" class="mr-b10 Chonnguoinhan btn btn_orange  waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked  mr-r5"></i>Chọn người nhận</a>
                                        <asp:DataList ID="lboxNguoiNhan" runat="server" DataKeyField="NGUOIDUNG_ID"
                                            CssClass="select-tag">
                                            <ItemTemplate>
                                                <div class="item-tag">
                                                    <a href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>' id="XoaThuKy" onserverclick="XoaNguoiNhan" runat="server" visible='<%#textTieuDe.Enabled%>'><i class="icofont-close-line"></i></a>
                                                    <%#  DataBinder.Eval(Container,"DataItem.TENVIETTAT")==null?"":(DataBinder.Eval(Container,"DataItem.TENVIETTAT")+" - ") %>  <%# DataBinder.Eval(Container,"DataItem.TENNGUOIDUNG") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <asp:Label runat="server" ID="lblNguoiNhanThongBao" CssClass="hidden"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row box-body" runat="server" id="divNguoiDung">
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
                                        <asp:LinkButton ID="btn_ThemMoi" Visible="false" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                                        <div id="divShowBtnXoa" style="display: none;" class="fleft">
                                            <button type="button" class="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100" onclick="confirm_delete_rows_update('<%=btn_GuiLaiThongBao.ClientID%>')"><i class="glyphicon glyphicon-send"></i><span>Gửi lại thông báo</span></button>
                                        </div>
                                        <div style="display: none;">
                                            <asp:LinkButton ID="btn_GuiLaiThongBao" OnClientClick="return getConfirmation(this, 'THÔNG TIN','Bạn muốn gửi lại thông báo cho những người đã chọn?');" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="btn_GuiLaiThongBao_Click"><i class="glyphicon glyphicon-send pd-r5"></i> &nbsp; Gửi lại thông báo</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
                                <asp:Panel CssClass="baoloi" runat="server" ID="Panel1" Visible="false">
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:DataGrid DataKeyField="NGUOIDUNG_ID" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="True" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
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
                                        <asp:BoundColumn HeaderText="NGUOIDUNG_ID" DataField="NGUOIDUNG_ID" Visible="false">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle />
                                        </asp:BoundColumn>

                                        <asp:TemplateColumn HeaderText="Người nhận" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%--<a id="dgChiTiet" onserverclick="dgDanhSach_Sua" title="Thông tin câu hỏi" href='<%# Eval("NGUOIDUNG_ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.DoiTuong") %></a>--%>
                                                <%# DataBinder.Eval(Container, "DataItem.DoiTuong") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <asp:TemplateColumn HeaderText="Trạng thái gửi" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("TrangThaiGui").ToString() == "True"? "Đã gửi":"Chưa gửi" %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <asp:TemplateColumn HeaderText="Trạng thái xem" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("TrangThaiXem").ToString() == "True"? "Đã xem":"Chưa xem" %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <%--                                        <asp:TemplateColumn HeaderText="Sửa" Visible="false">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a onserverclick="dgDanhSach_Sua" style='<%#kiemtra(int.Parse(DataBinder.Eval(Container, "DataItem.NGUOIDUNG_ID").ToString())).ToString()=="True"?"display:none": "" %>' title="Cập nhật thông tin câu hỏi" class="icon-sua" href='<%# Eval("CAUHOI_ID").ToString() %>' oncontextmenu="return false" runat="server"></a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>

                                        <asp:TemplateColumn HeaderText="Xóa" Visible="false">
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a onserverclick="dgDanhSach_Xoa" title="Xóa câu hỏi" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa câu hỏi này?');" href='<%# Eval("NGUOIDUNG_ID").ToString()%>' oncontextmenu="return false" runat="server"></a>
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
