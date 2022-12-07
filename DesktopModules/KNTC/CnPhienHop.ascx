<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnPhienHop.ascx.cs" Inherits="HOPKHONGGIAY.CnPhienHop" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%--<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>--%>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>

<chosen:chosenjs runat="server" />

<dnn:DnnJsInclude ID="PathCommonJS" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/common.js' AddTag="false" />
<dnn:DnnJsInclude ID="JavascriptMask" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/Mask/jquery.metadata.js' AddTag="false" />
<dnn:DnnJsInclude ID="FancytreeUIDeps" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery-ui-dependencies/jquery.fancytree.ui-deps.js" AddTag="false" />
<dnn:DnnJsInclude ID="Fancytree" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.js" AddTag="false" />
<dnn:DnnJsInclude ID="FancytreeFilter" runat="server" FilePath="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.filter.js" AddTag="false" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/skin-win8/ui.fancytree.css" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/custom-fanytree.css" />

<%--<%=vJavascriptMask %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<script type="text/javascript" src="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery-ui-dependencies/jquery.fancytree.ui-deps.js"></script>
<script type="text/javascript" src="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.js"></script>
<script type="text/javascript" src="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/jquery.fancytree.filter.js"></script>
<link href="/DesktopModules/HOPKHONGGIAY/lib/fancytree-master/src/skin-win8/ui.fancytree.css" type="text/css" rel="stylesheet"> 
<link href="/DesktopModules/HOPKHONGGIAY/Css/chosen.css" type="text/css" rel="stylesheet"> --%>
<script>
    function Isdisplay() {
        document.getElementById("tailieu-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_File = document.getElementById("tailieu");
        content_ChuongTrinhHop.className = content_File.className.replace("active", "");
        content_File.className = content_File.className.replace("tab-pane fade", "tab-pane fade active in");
    }

    function Isdisplay_ChonPhong() {
        document.getElementById("phonghop-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_PhongHop = document.getElementById("phonghop");
        content_ChuongTrinhHop.className = content_PhongHop.className.replace("active", "");
        content_PhongHop.className = content_PhongHop.className.replace("tab-pane fade", "tab-pane fade active in");

    }
    function Isdisplay_DaiBieu() {
        document.getElementById("tailieu-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_daibieu = document.getElementById("daibieu");
        content_ChuongTrinhHop.className = content_daibieu.className.replace("active", "");
        content_daibieu.className = content_File.className.replace("tab-pane fade", "tab-pane fade active in");
    }
    function pageLoad(sender, args) {
        initchosen();

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
            $("#treedaibieu").fancytree({
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
        }
        else {
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
                tree = $.ui.fancytree.getTree("#treedaibieu");
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
                            <a id="" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
                        </div>
                        <div id="treedaibieu" name="selNodes">
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
                            <a id="" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
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
                            <a id="" class="btnResetSearch btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100">&times;</a>
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

                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>

                    <asp:LinkButton ID="btnSua" Visible="true" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" Visible="false" runat="server" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                    <asp:LinkButton ID="buttonGoiDuyet" runat="server" CommandName="GoiDuyet" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-send-mail fz17"></i>  Gửi duyệt</asp:LinkButton>
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
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTieu">Tiêu đề</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9">
                                        <asp:TextBox ID="textTieuDe" runat="server" CssClass="form-control requirements" MaxLength="200" />
                                    </div>
                                </div>

                                <div class="form-group mr-t10">
                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="labelDonVi">Đơn vị</label>
                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                        <asp:DropDownList ID="ddlistDonVi" runat="server" CssClass="form-control requirements slPhongBan">
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
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group mr-t10">
                                    <label runat="server" id="labelThoiGianKetThuc" class="col-sm-3 control-label pd-r0">Thời gian kết thúc </label>
                                    <div class="col-sm-9 col-md-9 col-lg-9" style="text-align: center">
                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianKetThuc" TimeView-HeaderText="Giờ kết thúc" Width="100%"
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
            <div class="row box-body" runat="server" id="divDetail">
                <div class="row ">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <ul class="nav nav-tabs" id="myTab" role="tablist">
                                <li class='nav-item  <%=(TabActive.ToString()==""?"active":"")%>'>
                                    <a class='nav-link>' id="chuongtrinhhop-tab" data-toggle="tab" href="#chuongtrinhhop" role="tab" aria-controls="chuongtrinhhop" aria-selected='<%=(TabActive.ToString()==""?"true":"false") %>'><i class="icofont-list"></i>Chương trình họp</a><%--aria-expanded="true"--%>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="THAMDU"?"active":"") %>">
                                    <a class='nav-link ' id="daibieu-tab" data-toggle="tab" href="#daibieu" role="tab" aria-controls="daibieu" aria-selected='<%=(TabActive.ToString()=="THAMDU"?"true":"false") %>'><i class="icofont-users-alt-3"></i>Thành phần</a>
                                </li>

                                <li class="nav-item <%=(TabActive.ToString()=="PHONGHOP"?"active":"") %>">
                                    <a class="nav-link" id="phonghop-tab" data-toggle="tab" href="#phonghop" role="tab" aria-controls="phonghop" aria-selected="<%=(TabActive.ToString()=="PHONGHOP"?"true":"false") %>"><i class="icofont-black-board fz18"></i>Phòng họp</a>
                                </li>

                                <li class="nav-item  <%=(TabActive.ToString()=="TAILIEU"?"active":"") %>">
                                    <a class="nav-link" id="tailieu-tab" data-toggle="tab" href="#tailieu" role="tab" aria-controls="tailieu" aria-selected="<%=(TabActive.ToString()=="TAILIEU"?"true":"false") %>"><i class="icofont-attachment"></i>Tài liệu họp</a>
                                </li>
                                <%--  <li class="nav-item">
                                    <a class="nav-link" id="bienbanhop-tab" data-toggle="tab" href="#bienbanhop" role="tab" aria-controls="bienbanhop" aria-selected="false">Biên bản họp</a>
                                </li>--%>
                            </ul>
                            <div class="tab-content row box9" id="myTabContent">
                                <div class="tab-pane fade  <%=(TabActive.ToString()==""?" active in":"") %>" id="chuongtrinhhop" role="tabpanel" aria-labelledby="chuongtrinhhop-tab">
                                    <div class="row" runat="server" visible="true" id="divChuongTrinhHopCapNhat">
                                        <%--<dnn:TextEditor runat="server" ID="textChuongTrinhHop" Width="100%" Height="100%" />--%>
                                        <asp:TextBox ID="textChuongTrinhHop" runat="server" CssClass="form-control" Rows="20" TextMode="MultiLine" />
                                    </div>
                                    <div class="row" runat="server" visible="true" id="divChuongTrinhHopChiTiet">
                                        <asp:Label runat="server" ID="labelChuongTrinhHop"></asp:Label>
                                    </div>
                                </div>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="THAMDU"?" active in":"") %>" id="daibieu" role="tabpanel" aria-labelledby="daibieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                    <%-- Start - Chọn nhiều chủ trì--%>
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelChuTri">Chủ trì</label>
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
                                                            <asp:Label runat="server" ID="lblKhachmoisPhienHop" CssClass="hidden"></asp:Label>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- <div class="tab-pane fade" id="khachmoi" role="tabpanel" aria-labelledby="khachmoi-tab">
                                    <div class="row">
                                        khách mời
                                    </div>
                                </div>--%>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="TAILIEU"?" active in":"") %>" id="tailieu" role="tabpanel" aria-labelledby="tailieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal" runat="server" id="divFile">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
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
                                                            <asp:TextBox ID="textMotaFile" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" />
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
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label2"></label>
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
                                                            <asp:Panel ID="pnlTapTin" runat="server" Visible="false">
                                                                <asp:Label ID="lblTapTin" runat="server"></asp:Label>
                                                                <asp:LinkButton ID="btnXoa_tapTin" runat="server" Visible="false" OnClick="btnXoa_tapTin_Click"><i class="icon-xoa"></i></asp:LinkButton>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>

                                                    <div class="form-group mr-b0 mr-t10 offset-md-3" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label6"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:LinkButton ID="btn_TL" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btn_TL_Click"><i class="fa fa-upload" aria-hidden="true"></i> Tải lên</asp:LinkButton>
                                                            <asp:LinkButton ID="buttonCapNhatTaiLieu" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100" OnClick="buttonCapNhatTaiLieu_Click"><i class="icofont-save"></i> Cập nhật</asp:LinkButton>
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
                                                        <ItemStyle HorizontalAlign="Left" />
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

                                                    <asp:TemplateField HeaderText="Phổ biến/Không phổ biến" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiDoMat" runat="server">
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

                                                    <asp:TemplateField HeaderText="Trạng thái" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("TRANGTHAI").ToString() == "0" ? "Dự thảo" : "Đã duyệt"%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFile+"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sửa" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkSuaTaiLieu" onserverclick="SuaTaiLieu" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' runat="server">
                                                                <i class="icofont-ui-edit"></i>
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Xoá" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="deletebtn" runat="server" CommandName="Delete" CssClass="btnXoaTaiLieu"
                                                                OnClientClick="return confirm('Bạn muốn xoá tài liệu này?');" Text=""><i class="glyphicon glyphicon-remove red"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="tab-pane fade <%=(TabActive.ToString()=="PHONGHOP"?" active in":"") %>" id="phonghop" visible="false" role="tabpanel" aria-labelledby="phonghop-tab">
                                    <div class="form-horizontal" runat="server" id="divChonPhong">
                                        <div class="col-sm-12 col-md-12 col-lg-12">
                                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                <div class="form-group mr-b0 mr-t10" runat="server">
                                                    <label class="col-sm-3 control-label pd-r0" runat="server" id="label5">Thời gian bắt đầu</label>
                                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianBatDau_ChonPhong" TimeView-HeaderText="Giờ bắt đầu" Width="100%" Calendar-EnableTheming="true"
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
                                                    <label runat="server" id="label7" class="col-sm-3 control-label pd-r0">Thời gian kết thúc </label>
                                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8">
                                                        <telerik:RadDateTimePicker Culture="vi-VN" ID="dtpickerThoiGianKetThuc_ChonPhong" TimeView-HeaderText="Giờ kết thúc" Width="100%" Calendar-EnableTheming="true"
                                                            TimeView-Interval="01:00:00" TimeView-StartTime="07:00:00" TimeView-EndTime="17:00:01" TimeView-Columns="2" placeholder="00:00"
                                                            DateInput-DateFormat="dd/MM/yyyy HH:mm" TimeView-TimeFormat="HH:mm" runat="server" AutoPostBack="true" CssClass="form-control requirements slPhongBan">
                                                        </telerik:RadDateTimePicker>
                                                    </div>
                                                </div>

                                                <div class="form-group mr-b0 mr-t10" runat="server">
                                                    <label runat="server" id="labelBenhNhan" class="col-sm-3 control-label pd-r0">Chọn thiết bị</label>
                                                    <div class="col-sm-9 col-md-9 col-lg-9 pd-t8  focused">
                                                        <telerik:RadComboBox Skin="Simple" ID="ddlistThietBi" Filter="Contains" CssClass="custom-radcombox"
                                                            InputCssClass="form-control requirements" runat="server" Width="100%" Height="200px" EnableCheckAllItemsCheckBox="true"
                                                            EmptyMessage="-- Chọn thiết bị --" ShowWhileLoading="true" CheckBoxes="true" CausesValidation="false" EnableLoadOnDemand="true"
                                                            Localization-ItemsCheckedString="thiết bị được chọn" Localization-AllItemsCheckedString="Chọn tất cả" Localization-CheckAllString="Tất cả thiết bị"
                                                            LoadingMessage="Đang tải..." Localization-NoMatches="Không tìm thấy" OnCheckAllCheck="">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-12 col-md-12 col-lg-12 pd-l30 pd-r30 pd-b text-center">
                                                <asp:LinkButton ID="buttonTimKiem" runat="server" OnClick="buttonTimKiem_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-search"></i> Tìm kiếm</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-12 col-md-12 col-lg-12 pd-t15">
                                        <asp:Label runat="server" ID="labelThongBaoPhongHop" Text="" Visible="false"></asp:Label>
                                        <asp:Panel runat="server" ID="pnlKetQuaTimKiem" Visible="true">
                                            <asp:ListView ID="ListView_PHONG" runat="server">
                                                <ItemTemplate>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <div class="body_Phong" style='<%# DataBinder.Eval(Container, "DataItem.DACHON").ToString()==""?"background: #ffebdd;": "" %>'>
                                                            <div class="item_phong">
                                                                <p style="font-weight: bold;"><i class="icofont-black-board fz18" style="color: #ff6600"></i><%# Eval("TENPHONGHOP")%></p>
                                                                <p><i class="icofont-users-social fz18" style="color: #ff6600"></i>Sức chứa:  <%# Eval("SUCCHUA")%></p>
                                                                <p><i class="icofont-imac fz18" style="color: #ff6600"></i>Thiết bị:  <%# GetDanhSachTenThietBi(int.Parse(DataBinder.Eval(Container, "DataItem.PHONGHOP_ID").ToString())) %></p>
                                                                <asp:Literal runat="server" ID="labelLichTrung" Text='<%# Eval("LICHHOP").ToString() %>'></asp:Literal>
                                                            </div>
                                                            <div class="item_phong text-right" style="width: 75px">
                                                                <a onserverclick="DangKyPhong" title="Chọn phòng cho phiên họp" href='<%# Eval("PHONGHOP_ID").ToString() %>' oncontextmenu="return false" runat="server" class='btn btn-primary waves-effect none-radius none-shadow' style='<%# Eval("DANGKY").ToString() + ";" %>'>Đăng ký</a>
                                                                <a onserverclick="XoaPhong" title="Bỏ phòng cho phiên họp" onclick="return getConfirmation(this, 'BỎ CHỌN PHÒNG','Bạn có muốn bỏ chọn phòng này?');" href='<%# Eval("PHONGHOP_ID").ToString() %>' oncontextmenu="return false" runat="server" class='btn btn-danger waves-effect none-radius none-shadow' style='<%# Eval("DACHON").ToString() + ";" %>'>Bỏ chọn</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr style='border-top: 0px; margin-top: 0px; margin-bottom: 0px; <%# (int.Parse(DataBinder.Eval(Container, "DataItem.STT").ToString())%3==0)?"display:block;": "display:none;" %>'></hr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <%--  <div class="tab-pane fade" id="bienbanhop" visible="false" role="tabpanel" aria-labelledby="bienbanhop-tab">
                                    <div class="row" runat="server" id="divBienBanHopChinhSua">
                                        <dnn:TextEditor runat="server" ID="textBienBanHop" Width="100%" Height="100%" />
                                    </div>
                                    <div class="row" runat="server" visible="true" id="divBienBanHopChiTiet">
                                        <asp:Literal runat="server" ID="textBienBanHopChiTiet"></asp:Literal>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_TL" />        
    </Triggers>
</asp:UpdatePanel>
<style>
    .form_radiobuttonlist label {
        margin-top: 5px;
    }

    .form_radiobuttonlist label {
        margin-top: 5px;
    }

    .title_KetQua {
        /*margin-top:50px;*/
        font-size: 16px;
        float: left;
        margin-bottom: 15px;
    }

    .btnXoaTaiLieu {
        background: transparent;
        border: none;
    }

    .body_Phong {
        padding: 6px;
        /*box-shadow: 0 2px 8px -2px black;*/
        margin-bottom: 15px;
        width: 100%;
        border: 1px solid #7b7b7b70;
        display: table;
        border-radius: 10px;
    }

    .item_phong {
        display: table-cell;
        vertical-align: middle;
    }

        .item_phong p {
            line-height: 25px;
        }

        .item_phong i {
            padding-right: 15px;
        }

    .phong_container:first-child {
        padding-left: 0px;
        padding-right: 15px;
    }

    .phong_container:last-child {
        padding-left: 15px;
        padding-right: 0px;
    }

    .body_PhongDaChon {
        padding-top: 30px;
        float: left;
    }

    .active-result > input, .result-selected > input {
        display: inline-block;
        vertical-align: middle;
    }

        .active-result > input[type=checkbox], .result-selected > input[type=checkbox] {
            top: 0px !important;
        }

    div.custom-radcombox > table {
        width: 100% !important
    }

    #modalChonDaiBieu .modal-body {
        max-height: 600px;
        overflow: auto;
    }
</style>

<script>
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }
    function ConfirmDelete() {
        if (confirm("Bạn có muốn xóa không?")) {
            return true;
        }
        else {
            return false;
        }
    }
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function getWidthSoDo() {
        return (screen.width * 60) / 100;
    }
</script>
