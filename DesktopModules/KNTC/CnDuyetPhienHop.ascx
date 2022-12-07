<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnDuyetPhienHop.ascx.cs" Inherits="HOPKHONGGIAY.CnDuyetPhienHop" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>
<chosen:chosenjs runat="server" />
<dnn:DnnJsInclude ID="PathCommonJS" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/common.js' AddTag="false" />
<dnn:DnnJsInclude ID="JavascriptMask" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/Mask/jquery.metadata.js' AddTag="false" />
<dnn:DnnJsInclude ID="FancytreeUIDeps" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery-ui-dependencies/jquery.fancytree.ui-deps.js" AddTag="false" />
<dnn:DnnJsInclude ID="Fancytree" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.js" AddTag="false" />
<dnn:DnnJsInclude ID="FancytreeFilter" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.filter.js" AddTag="false" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/skin-win8/ui.fancytree.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/custom-fanytree.css" />
<script>
    function Isdisplay() {
        document.getElementById("tailieu-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_File = document.getElementById("tailieu");
        content_ChuongTrinhHop.className = content_File.className.replace("active", "");
        content_File.className = content_File.className.replace("tab-pane fade", "tab-pane fade active in");
    }

    function Isdisplay_BienBan() {
        document.getElementById("bienbanhop-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_FileBienBan = document.getElementById("bienbanhop");
        content_ChuongTrinhHop.className = content_FileBienBan.className.replace("active", "");
        content_FileBienBan.className = content_FileBienBan.className.replace("tab-pane fade", "tab-pane fade active in");
    }
    function Isdisplay_KetLuan() {
        document.getElementById("ketluan-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_FileKetLuan = document.getElementById("ketluan");
        content_ChuongTrinhHop.className = content_FileKetLuan.className.replace("active", "");
        content_FileKetLuan.className = content_FileKetLuan.className.replace("tab-pane fade", "tab-pane fade active in");
    }
    function pageLoad(sender, args) {
        initchosen();
         //$('<%=lboxDaiBieu.ClientID%>').chosen().chosenReadonly(true);

        $(".Chonchutri").click(function () {
            $(":ui-fancytree").fancytree("destroy");
            Intfancytree("chutri");
            $('#modalChonChuTri').modal('show');
        });

        $(".Chondaibieu").click(function () {
            $(":ui-fancytree").fancytree("destroy");
            Intfancytree("daibieu");
            $('#modalChonDaiBieu').modal('show');
        });

        $(".Chonkhachmoi").click(function () {
            $(":ui-fancytree").fancytree("destroy");
            Intfancytree("khachmoi");
            $('#modalChonKhachMoi').modal('show');
        });
        $(".Chonthuky").click(function () {
            $(":ui-fancytree").fancytree("destroy");
            Intfancytree("thuky");
            $('#modalChonThuKy').modal('show');
        });
    }

</script>
<script type="text/javascript">
    function Intfancytree(type) {
        var domainname = window.location.origin;
        var url_source = domainname + "/DesktopModules/HOPKHONGGIAY/API/ServiceHopKhongGiay/";
        if (type == "khachmoi") {
            var KhachMois = $('#<%=lblKhachmoisPhienHop.ClientID %>').html();
            url_source = url_source + "GetKhachMoi?khachmoi=" + KhachMois;
            $("#treekhachmoi").fancytree({
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
                    $('#<%=hiddenfieldChonKhachMoi.ClientID %>').val(selKeys.join(","));
                },
            });
        }
        else if (type == "daibieu") {
            var DaiBieus = $('#<%=lblDaiBieusPhienHop.ClientID %>').html();
            var ChuTris = $('#<%=lblChuTrisPhienHop.ClientID %>').html();
            var ThuKys = $('#<%=lblThuKysPhienHop.ClientID %>').html();
            url_source = url_source + "GetDaiBieu?daibieu=" + DaiBieus + "&an=" + ChuTris + "," + ThuKys;
            $("#tree").fancytree({
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
                    $('#<%=hiddenfieldChonDaiBieu.ClientID %>').val(selKeys.join(","));
                },
            });
        }
        else if (type == "thuky") {
            var ThuKys = $('#<%=lblThuKysPhienHop.ClientID %>').html();
            var ChuTris = $('#<%=lblChuTrisPhienHop.ClientID %>').html();
            var DaiBieus = $('#<%=lblDaiBieusPhienHop.ClientID %>').html();
            url_source = url_source + "GetDaiBieu?daibieu=" + ThuKys + "&an=" + ChuTris + "," + DaiBieus;
            $("#treethuky").fancytree({
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
                    $('#<%=hiddenfieldChonThuKy.ClientID %>').val(selKeys.join(","));
                },
            });
        } else {
            var ChuTris = $('#<%=lblChuTrisPhienHop.ClientID %>').html();
            var ThuKys = $('#<%=lblThuKysPhienHop.ClientID %>').html();
            var DaiBieus = $('#<%=lblDaiBieusPhienHop.ClientID %>').html();
            url_source = url_source + "GetDaiBieu?daibieu=" + ChuTris + "&an=" + ThuKys + "," + DaiBieus;
            $("#treechutri").fancytree({
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
                    $('#<%=hiddenfiendChonChuTri.ClientID %>').val(selKeys.join(","));
                }
            });
        }
        var tree;
        $("input[name=search]").on("keyup", function (e) {
            //alert($(this).val());
            if ($(this).data('type') == "daibieu") {
                tree = $.ui.fancytree.getTree("#tree");
            }
            else if ($(this).data('type') == "khachmoi") {

                tree = $.ui.fancytree.getTree("#treekhachmoi");
            }
            else if ($(this).data('type') == "thuky") {
                tree = $.ui.fancytree.getTree("#treethuky");
            }
            else if ($(this).data('type') == "chutri") {

                tree = $.ui.fancytree.getTree("#treechutri");
            }

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
        $('#modalChonChuTri').modal('hide');
        $('#modalChonDaiBieu').modal('hide');
        $('#modalChonKhachMoi').modal('hide');
        $('#modalChonThuKy').modal('hide');
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
        
        <%--Modal chọn nhiều chủ trì NHHAN--%>
         <div id="modalChonChuTri" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <span>Chọn chủ trì</span>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form " style="position: relative;">
                            <div class="form-group mr-t10">
                                <input name="search" data-type="chutri" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." autocomplete="off" />
                            </div>
                            <a id="" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="treechutri" name="selNodes">
                            <input id="hiddenfiendChonChuTri" type="hidden" runat="server" value="" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <asp:Button Text="Chọn" ID="btnConfirmChonChuTri" OnClientClick="HideModal()" CssClass="btn btn-danger" runat="server" OnClick="btnConfirmChonChuTri_Click"></asp:Button>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--End modal chọn nhiều chủ trì--%>
        <div id="modalChonDaiBieu" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <span>Chọn đại biểu</span>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form " style="position: relative;">
                            <div class="form-group mr-t10">
                                <input name="search" data-type="daibieu" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." autocomplete="off" />
                            </div>
                            <a id="btnResetSearch" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="tree" name="selNodes">
                            <input id="hiddenfieldChonDaiBieu" type="hidden" runat="server" value="" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <asp:Button Text="Chọn" ID="btnConfirmChonDaiBieu" OnClientClick="HideModal()" CssClass="btn btn-danger" runat="server" OnClick="btnConfirmChonDaiBieu_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modalChonThuKy" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <span>Chọn thư ký</span>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form " style="position: relative;">
                            <div class="form-group mr-t10">
                                <input name="search" data-type="thuky" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." autocomplete="off" />
                            </div>
                            <a id="btnResetSearch" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="treethuky" name="selNodes">
                            <input id="hiddenfieldChonThuKy" type="hidden" runat="server" value="" />

                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <asp:Button Text="Chọn" ID="btnConfirmChonThuKy" OnClientClick="HideModal()" CssClass="btn btn-danger" runat="server" OnClick="btnConfirmChonThuKy_Click"></asp:Button>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modalChonKhachMoi" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">
                            <span>Chọn khách mời</span>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="form " style="position: relative;">
                            <div class="form-group mr-t10">
                                <input name="search" data-type="khachmoi" class="form-control" placeholder="Nhập từ khóa tìm kiếm..." autocomplete="off" />
                            </div>
                            <a id="btnResetSearch" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="treekhachmoi" name="selNodes">
                            <input id="hiddenfieldChonKhachMoi" type="hidden" runat="server" value="" />

                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <asp:Button Text="Chọn" ID="btnConfirmChonKhachMoi" OnClientClick="HideModal()" CssClass="btn btn-danger" runat="server" OnClick="btnConfirmChonKhachMoi_Click"></asp:Button>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="btnCapNhat" runat="server" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
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
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTieu">Tiêu đề</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textTieuDe" runat="server" CssClass="form-control requirements" MaxLength="200" />
                                    </div>
                                </div>

                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelDonVi">Đơn vị</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <asp:DropDownList ID="ddlistDonVi" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                  <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelGhiChu">Ghi chú</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textGhiChu" runat="server" CssClass="form-control requirements" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
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
                                    <label runat="server" id="labelThoiGianKetThuc" class="col-sm-3 control-label pd-r0">Thời gian kết thúc </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9" style="text-align: center">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianKetThuc" TimeView-HeaderText="Giờ kết thúc" Width="100%"
                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row box-body">
                <div class="row ">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <ul class="nav nav-tabs" id="myTab" role="tablist">
                                <li class="nav-item <%=(TabActive.ToString()==""?"active":"") %>" id="liChuongTrinhHop">
                                    <a class="nav-link" id="chuongtrinhhop-tab" data-toggle="tab" href="#chuongtrinhhop" role="tab" aria-controls="chuongtrinhhop" aria-selected='<%=(TabActive.ToString()==""?"true":"false") %>'><i class="icofont-list"></i>Chương trình họp</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="THAMDU"?"active":"") %>" id="liThanhPhan">
                                    <a class="nav-link" id="daibieu-tab" data-toggle="tab" href="#daibieu" role="tab" aria-controls="daibieu" aria-selected='<%=(TabActive.ToString()=="THAMDU"?"true":"false") %>'><i class="icofont-users-alt-3"></i>Thành phần</a>
                                </li>

                                <li class="nav-item <%=(TabActive.ToString()=="TAILIEU"?"active":"") %>" id="liTaiLieuHop">
                                    <a class="nav-link" id="tailieu-tab" data-toggle="tab" href="#tailieu" role="tab" aria-controls="tailieu" aria-selected='<%=(TabActive.ToString()=="TAILIEU"?"true":"false") %>'><i class="icofont-attachment"></i>Tài liệu</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="BIENBANHOP"?"active":"") %>" id="liBienBanHop">
                                    <a class="nav-link" id="bienbanhop-tab" data-toggle="tab" href="#bienbanhop" role="tab" aria-controls="bienbanhop" aria-selected='<%=(TabActive.ToString()=="BIENBANHOP"?"true":"false") %>'><i class="icofont-law-document"></i>Biên bản</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="KETLUAN"?"active":"") %>" id="liKetLuan">
                                    <a class="nav-link" id="ketluan-tab" data-toggle="tab" href="#ketluan" role="tab" aria-controls="ketluan" aria-selected='<%=(TabActive.ToString()=="KETLUAN"?"true":"false") %>'><i class="icofont-file-document"></i>Kết luận</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="SODO"?"active":"") %>" id="liSoDoPhong" visible="false">
                                    <a class="nav-link" id="sodophong-tab" data-toggle="tab" href="#sodophong" role="tab" aria-controls="sodophong" aria-selected='<%=(TabActive.ToString()=="SODO"?"true":"false") %>'><i class="icofont-company fz17"></i>Sơ đồ</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="DIEMDANH"?"active":"") %>" id="liDiemDanh" visible="false">
                                    <a class="nav-link" id="diemdanh-tab" data-toggle="tab" href="#diemdanh" role="tab" aria-controls="diemdanh" aria-selected='<%=(TabActive.ToString()=="DIEMDANH"?"true":"false") %>'><i class="icofont-checked"></i>Điểm danh</a>
                                </li>
                            </ul>
                            <div class="tab-content row box9" id="myTabContent">
                                <div class="tab-pane fade <%=(TabActive.ToString()==""?" active in":"") %>" id="chuongtrinhhop" role="tabpanel" aria-labelledby="chuongtrinhhop-tab">
                                    <div class="row">
                                        <asp:TextBox ID="textChuongTrinhHop" runat="server" CssClass="form-control" Rows="20" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="THAMDU"?"active in":"")%>" id="daibieu" role="tabpanel" aria-labelledby="daibieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">

                                                    <div class="form-group mr-t10" runat="server" visible="false">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelChuTri">Chủ trì phiên họp</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                            <asp:DropDownList ID="ddlistChuTri" runat="server" SelectionMode="Single" data-placeholder="Chọn người chủ trì" CssClass="form-control chosen-select slPhongBan">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                      <%-- Start - Chọn nhiều chủ trì--%>
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label2">Chủ trì</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                            <a runat="server" id="Chonchutri" class="mr-b10 Chonchutri btn btn_orange  waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked  mr-r5"></i>Chọn chủ trì</a>
                                                            <asp:DataList ID="lboxChuTri" runat="server" DataKeyField="NGUOIDUNG_ID"
                                                                CssClass="select-tag">
                                                                <ItemTemplate>
                                                                    <div class="item-tag">
                                                                        <a href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>' id="XoaChuTri" onserverclick="XoaChuTri" runat="server" visible='<%#textTieuDe.Enabled%>'><i class="icofont-close-line"></i></a>
                                                                        <%#  DataBinder.Eval(Container,"DataItem.TENVIETTAT")==null?"":(DataBinder.Eval(Container,"DataItem.TENVIETTAT")+" - ") %>  <%# DataBinder.Eval(Container,"DataItem.TENNGUOIDUNG") %>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                            <asp:Label runat="server" ID="lblChuTrisPhienHop" CssClass="hidden"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <%--End - Chọn nhiều chủ trì--%>

                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label1">Đại biểu</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                            <a runat="server" id="Chondaibieu" class="mr-b10 Chondaibieu btn btn_orange  waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked  mr-r5"></i>Chọn đại biểu</a>
                                                            <asp:DataList ID="lboxDaiBieu" runat="server" DataKeyField="NGUOIDUNG_ID"
                                                                CssClass="select-tag">
                                                                <ItemTemplate>
                                                                    <div class="item-tag">
                                                                        <a href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>' id="XoaDaiBieu" onserverclick="XoaDaiBieu" runat="server" visible='<%#textTieuDe.Enabled%>'><i class="icofont-close-line"></i></a>
                                                                        <%#  DataBinder.Eval(Container,"DataItem.TENVIETTAT")==null?"":(DataBinder.Eval(Container,"DataItem.TENVIETTAT")+" - ") %>  <%# DataBinder.Eval(Container,"DataItem.TENNGUOIDUNG") %>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                            <asp:Label runat="server" ID="lblDaiBieusPhienHop" CssClass="hidden"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label3">Thư ký</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                            <a runat="server" id="Chonthuky" class="mr-b10 Chonthuky btn btn_orange  waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked  mr-r5"></i>Chọn thư ký</a>
                                                            <asp:DataList ID="lboxThuKy" runat="server" DataKeyField="NGUOIDUNG_ID"
                                                                CssClass="select-tag">
                                                                <ItemTemplate>
                                                                    <div class="item-tag">
                                                                        <a href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>' id="XoaThuKy" onserverclick="XoaThuKy" runat="server" visible='<%#textTieuDe.Enabled%>'><i class="icofont-close-line"></i></a>
                                                                        <%#  DataBinder.Eval(Container,"DataItem.TENVIETTAT")==null?"":(DataBinder.Eval(Container,"DataItem.TENVIETTAT")+" - ") %>  <%# DataBinder.Eval(Container,"DataItem.TENNGUOIDUNG") %>
                                                                    </div>
                                                                </ItemTemplate>

                                                            </asp:DataList>
                                                            <asp:Label runat="server" ID="lblThuKysPhienHop" CssClass="hidden"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label4">Khách mời</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">

                                                            <a runat="server" id="Chonkhachmoi" class="mr-b10 Chonkhachmoi btn btn_orange  waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked  mr-r5"></i>Chọn khách mời</a>
                                                            <asp:DataList ID="lboxKhachMoi" runat="server" DataKeyField="NGUOIDUNG_ID"
                                                                CssClass="select-tag">
                                                                <ItemTemplate>
                                                                    <div class="item-tag">
                                                                        <a href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>' id="XoaKhachMoi" onserverclick="XoaKhachMoi" runat="server" visible='<%#textTieuDe.Enabled%>'><i class="icofont-close-line"></i></a>
                                                                        <%#  DataBinder.Eval(Container,"DataItem.TENVIETTAT")==null?"":(DataBinder.Eval(Container,"DataItem.TENVIETTAT")+" - ") %>  <%# DataBinder.Eval(Container,"DataItem.TENNGUOIDUNG") %>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                            <asp:Label runat="server" ID="lblKhachmoisPhienHop"  CssClass="hidden"> </asp:Label><%----%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="BIENBANHOP"?"active in":"") %>" id="bienbanhop" role="tabpanel" aria-labelledby="bienbanhop-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12" runat="server" visible="true">
                                                <div class="col-md-offset-3 col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelFileBienBan">Chọn biên bản họp</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:FileUpload ID="f_File_BienBan" runat="server" CssClass="" AllowMultiple="true" Style="float: left;" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10 offset-md-3" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label10"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:LinkButton ID="buttonTailen_BienBan" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-sm btn-default none-radius shadow-btn-sm min-width-100" OnClick="TaiLenBienBan"><i class="fa fa-upload" aria-hidden="true"></i>Tải lên</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="danhsach" Style="padding: 0px">
                                            <asp:GridView ID="dgDanhSach_FileBienBan" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach_BienBan" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Biên bản họp" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                   
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiTrangThai" runat="server">
                                                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("1"))?"<span class='glyphicon glyphicon-ok' title='Đã duyệt' style='color:#008000;'></span>":"<span style='color:red;' title='Chờ duyệt' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFileBienBan+"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:CommandField ItemStyle-Width="10%" HeaderText="Xóa" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove' onclick='alert(abc);'></i>" />--%>
                                                     <asp:TemplateField  HeaderText="Xoá" HeaderStyle-HorizontalAlign="Center">
                                                       <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate >
                                                            <asp:LinkButton ID="deletebtn" runat="server" CommandName="Delete"  CssClass="btnXoaTaiLieu"
                                                                OnClientClick="return confirm('Bạn muốn xoá tài liệu này?');"  Text=""><i class="glyphicon glyphicon-remove red"></i></asp:LinkButton>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="KETLUAN"?"active in":"") %>" id="ketluan" role="tabpanel" aria-labelledby="ketluan-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12" runat="server" visible="true">
                                                <div class="col-md-offset-3 col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelFileKetLuan">Chọn tài liệu kết luận</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:FileUpload ID="f_File_KetLuan" runat="server" CssClass="" AllowMultiple="true" Style="float: left;" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10 offset-md-3" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label8"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:LinkButton ID="buttonTaiLenKetLuan" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-sm btn-default none-radius shadow-btn-sm min-width-100" OnClick="TaiLenTaiLieuKetLuan"><i class="fa fa-upload" aria-hidden="true"></i>Tải lên</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="danhsach" Style="padding: 0px">
                                            <asp:GridView ID="dgDanhSach_FileKetLuan" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach_KetLuan" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tài liệu kết luận" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                   
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiTrangThai" runat="server">
                                                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("1"))?"<span class='glyphicon glyphicon-ok' title='Đã duyệt' style='color:#008000;'></span>":"<span style='color:red;' title='Chờ duyệt' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFileKetLuan +"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:CommandField ItemStyle-Width="10%" HeaderText="Xóa" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove' onclick='alert(abc);'></i>" />--%>
                                                     <asp:TemplateField  HeaderText="Xoá" HeaderStyle-HorizontalAlign="Center">
                                                       <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate >
                                                            <asp:LinkButton ID="deletebtn" runat="server" CommandName="Delete"  CssClass="btnXoaTaiLieu"
                                                                OnClientClick="return confirm('Bạn muốn xoá tài liệu này?');"  Text=""><i class="glyphicon glyphicon-remove red"></i></asp:LinkButton>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="TAILIEU"?"active in":"") %>" id="tailieu" role="tabpanel" aria-labelledby="tailieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12" runat="server" visible="true">
                                                <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTenTaiLieu">Tên tài liệu</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:TextBox ID="textTenTaiLieu" runat="server" CssClass="form-control requirements" />
                                                        </div>
                                                    </div>
                                                      <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelNhomTaiLieu">Nhóm</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:TextBox ID="textNhomTaiLieu" runat="server" CssClass="form-control requirements" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelMota">Mô tả</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:TextBox ID="textMotaFile" runat="server" CssClass="form-control"/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">

                                                     <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelQuyenXem">Quyền xem</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9 mr-t10">
                                                            <asp:CheckBox ID="checkboxChuTri" Checked="true" runat="server" CssClass="mr-t10" Text=" Chủ trì" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label11"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:CheckBox ID="checkboxDaiBieu" Checked="true" runat="server" Text=" Đại biểu" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label7"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:CheckBox ID="checkboxThuKy" Checked="true" runat="server" Text=" Thư ký" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label32"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:CheckBox ID="checkboxKhachMoi" Checked="true" runat="server" Text=" Khách mời" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelFile">Chọn tài liệu</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:FileUpload ID="f_file" runat="server" CssClass="" AllowMultiple="true" Style="float: left;" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10 offset-md-3" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label6"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:LinkButton ID="btn_TL" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-sm btn-default none-radius shadow-btn-sm min-width-100" OnClick="btn_TL_Click"><i class="fa fa-upload" aria-hidden="true"></i>Tải lên</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="danhsach" Style="padding: 0px">
                                            <asp:GridView ID="dgDanhSach_File" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tên tài liệu" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Nhóm" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("NHOM")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Quyền xem" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("QUYEN")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Độ phổ biến" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkThayDoiDoMat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiDoMat" runat="server">
                                                                    <%#(DataBinder.Eval(Container,"DataItem.DOMAT").ToString().Equals("Mật"))?"<span> Phổ biến </span>":"<span> Không phổ biến </span>" %>   
                                                            </a>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  <asp:TemplateField HeaderText="Tên file" Visible="false">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("MOTA")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiTrangThai" runat="server">
                                                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("1"))?"<span class='glyphicon glyphicon-ok' title='Đã duyệt' style='color:#008000;'></span>":"<span style='color:red;' title='Chờ duyệt' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFile+"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:CommandField ItemStyle-Width="10%" HeaderText="Xóa" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove' onclick='alert(abc);'></i>" />--%>
                                                     <asp:TemplateField  HeaderText="Xoá" HeaderStyle-HorizontalAlign="Center">
                                                       <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate >
                                                            <asp:LinkButton ID="deletebtn" runat="server" CommandName="Delete"  CssClass="btnXoaTaiLieu"
                                                                OnClientClick="return confirm('Bạn muốn xoá tài liệu này?');"  Text=""><i class="glyphicon glyphicon-remove red"></i></asp:LinkButton>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="DIEMDANH"?"active in":"") %>" id="diemdanh" role="tabpanel" aria-labelledby="diemdanh-tab">
                                    <div class="row">
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
                                                        <div id="divShowBtnXoa" style="display: none;" class="fleft">
                                                            <button type="button" class="btn-default btn-warning-delete btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" onclick="confirm_delete_rows_update('<%=btn_DiemDanh.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                                                        </div>
                                                        <div style="display: none;">
                                                            <asp:LinkButton ID="btn_DiemDanh" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="DiemDanhNhieuDaiBieu"><i class=""></i> Điểm danh</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>


                                            <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
                                                <asp:Panel CssClass="baoloi" runat="server" ID="Panel1" Visible="false">
                                                    <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                                                </asp:Panel>
                                                <asp:DataGrid DataKeyField="ID" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="False" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
                                                    AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound">
                                                    <HeaderStyle CssClass="tieude" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="STT" Visible="false">
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
                                                        <asp:BoundColumn HeaderText="ID" DataField="ID" Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle />
                                                        </asp:BoundColumn>

                                                        <asp:TemplateColumn HeaderText="Tên đại biểu" HeaderStyle-HorizontalAlign="Left" SortExpression="TEN">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container, "DataItem.TEN") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="Đơn vị" HeaderStyle-HorizontalAlign="Left" SortExpression="DONVI">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <%# Eval("TENDONVI").ToString() %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Chức vụ" HeaderStyle-HorizontalAlign="Left" SortExpression="CHUCVU">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <%# Eval("TENCHUCVU").ToString() %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Đại biểu / Khách mời" HeaderStyle-HorizontalAlign="Left" SortExpression="LOAI">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <%# Eval("LOAI").ToString() == "daibieu" ? "Đại biểu" : "Khách mời"%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="Có mặt" Visible="true">
                                                            <HeaderStyle />
                                                            <ItemStyle  HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <a onserverclick="DiemDanh" title="Điểm danh" href='<%# Eval("ID").ToString() + "/"+ Eval("LOAI").ToString() %>' oncontextmenu="return false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA") != null ?  (Boolean.Parse(DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA").ToString()) == true ? "<span class='glyphicon glyphicon-ok' style='color:#008000;'></span>":"<span style='color:red;' class='glyphicon glyphicon-minus-sign'></span>") : "<span style='color:red;' class='glyphicon glyphicon-minus-sign'></span>" %> 
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <%-- <asp:TemplateColumn HeaderText="Sửa" Visible="false">
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
                                                        </asp:TemplateColumn>--%>
                                                    </Columns>
                                                    <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999"></PagerStyle>
                                                </asp:DataGrid>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="SODO"?" active in":"") %>" id="sodophong" role="tabpanel" aria-labelledby="sodophong-tab">
                                    <div class="row">

                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="col-sm-4 col-md-4 col-lg-4 pd-l15 pd-r15">
                                                    <asp:Panel ID="Panel5" runat="server" CssClass="form mr-b10" DefaultButton="buttonSearch">
                                                        <div class="form-inline">
                                                            <div class="col-right mr-b6">
                                                                <asp:TextBox CssClass="form-control btn-sm"  OnTextChanged="textSearchMaGhe_TextChanged" Width="300" AutoPostBack="true" ID="textSearchMaGhe" placeholder="Nhập từ khóa..." runat="server"></asp:TextBox>
                                                                <asp:LinkButton ID="btnSearchGhe" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="textSearchMaGhe_TextChanged">
                                                <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="Panel6" runat="server" CssClass="danhsach" Style="margin-bottom: 15px;">
                                                        <asp:DataGrid runat="server" ID="dgDanhSachGhe" AutoGenerateColumns="False"  AllowPaging="false" OnItemDataBound="dgDanhSachGhe_ItemDataBound">
                                                            <%--AutoPostBack="true"--%>
                                                            <Columns>
                                                                <asp:BoundColumn DataField="TEN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Họ và tên"></asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="Chức Vụ - Đơn vị" ItemStyle-HorizontalAlign="Left" Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <%# Eval("TENCHUCVU").ToString() + " - " +  Eval("TENDONVI").ToString() %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Mã ghế" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ToolTip='<%# Eval("ID").ToString() + "|" +  Eval("LOAI").ToString() %>' runat="server" AutoPostBack="true" OnTextChanged="NhapMaGhe" ID="textMaGhe" MaxLength="500" Style="height: 34px; margin-bottom: 3px" Text=''></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <HeaderStyle CssClass="tieude" />
                                                        </asp:DataGrid>
                                                    </asp:Panel>
                                                </div>
                                                <div class="col-sm-8 col-md-8 col-lg-8" style="overflow-y: auto">
                                                    <label runat="server" id="lblImage" style="width: 100%; "></label>
                                                    <%--<object data="/DesktopModules/HOPKHONGGIAY/sodophong_a3.svg" type="image/svg+xml" width="600" id="blah">--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_TL" />
        <asp:PostBackTrigger ControlID="buttonTailen_BienBan" />
        <asp:PostBackTrigger ControlID="buttonTaiLenKetLuan" />
    </Triggers>
</asp:UpdatePanel>
<style>
    .form_radiobuttonlist label {
        margin-top: 5px;
    }
    .btnXoaTaiLieu
    {
        background: transparent;
        border: none;
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

    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
