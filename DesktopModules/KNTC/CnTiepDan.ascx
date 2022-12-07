<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnTiepDan.ascx.cs" Inherits="KNTC.CnTiepDan" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<dnn:DnnJsInclude ID="Chosen" runat="server" FilePath="/DesktopModules/KNTC/Scripts/search.js" AddTag="false" />
<%--<%=vJavascriptMask %>--%>
<script type="text/javascript">
    function pageLoad(sender, args) {
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })
        $('[data-mask]').inputmask();
        $('.datecontrol').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('.datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
        var modal = $('.modal-backdrop.show');
        //for (var i = 0; i < modal.length; i++) {
        //    $('#modal-default').modal('hide');
        //    $('#modal-default').modal('show')
        //}
        initSearch();
        InitSticky();
        $('.datepicker').datetimepicker({
            format: 'L'
        });
    }
    function hideModal() {
        // alert("aaaa");
        $('#modal-default').modal('hide');
    }


</script>
<div class="modal" id="modal-default-hosodonthu" data-backdrop="static">
    <div class="modal-dialog" style="width: 60%; max-width: 60%;">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Thêm hồ sơ đính kèm đơn thư</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label6">Chọn tập tin </label>
                                    <div class="col-lg-9">
                                        <asp:FileUpload runat="server" ID="fileHoSoDinhKem" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label3">Tên văn bản </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textTenHoSo" runat="server" CssClass="form-control float-left requirement" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label5">Loại hồ sơ </label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="ddlistLoaiHoSo" runat="server" CssClass="form-control select2bs4">
                                            <asp:ListItem Value="" Text="Chọn loại hồ sơ"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Đơn kiến nghị, phản ánh"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Phiếu chuyển đơn"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Đơn tố cáo"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Đơn khiếu nại"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Loại khác"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelNoiDungTomTat">Nội dung tóm tắt </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textNoiDungTomTat" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control float-left" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-tools mr-b10 text-center">
                            <asp:LinkButton ID="buttonLuuHoSo_Modal" OnClick="LuuHoSo" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Thêm</asp:LinkButton>
                            <button class="btn  btn-flat btn-md btn-outline-danger tool-left" causesvalidation="false" data-dismiss="modal"><i class="icofont-ui-close"></i>Đóng</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="buttonLuuHoSo_Modal" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<div class="modal" id="modal-default-hosohuongxuly" data-backdrop="static">
    <div class="modal-dialog" style="width: 60%; max-width: 60%;">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Thêm hồ sơ đính kèm hướng xử lý</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="upn2" runat="server" UpdateMode="Always">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label7">Chọn tập tin </label>
                                    <div class="col-lg-9">
                                        <asp:FileUpload runat="server" ID="fileHoSoDinhKem_HuongXuLy" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label8">Tên hồ sơ </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textTenHoSo_HuongXuLy" runat="server" CssClass="form-control float-left requirement" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label9">Loại hồ sơ </label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="ddlistLoaiHoSo_HuongXuLy" runat="server" CssClass="form-control select2bs4">
                                            <asp:ListItem Value="" Text="Chọn loại hồ sơ"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Đơn kiến nghị, phản ánh"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Phiếu chuyển đơn"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Đơn tố cáo"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Đơn khiếu nại"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Loại khác"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label10">Nội dung tóm tắt </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textNoiDungTomTat_HuongXuLy" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control float-left" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-tools mr-b10 text-center">
                            <asp:LinkButton ID="buttonLuuHoSo_HuongXuLy" OnClick="LuuHoSoHuongXuLy" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Lưu</asp:LinkButton>
                            <button class="btn  btn-flat btn-md btn-outline-danger tool-left" causesvalidation="false" data-dismiss="modal"><i class="icofont-ui-close"></i>Đóng</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="buttonLuuHoSo_HuongXuLy" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<div class="modal" id="modal-default-hosonguoidaidienuyquyen" data-backdrop="static">
    <div class="modal-dialog" style="width: 60%; max-width: 60%;">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Thêm hồ sơ đính kèm thông tin người đại diện, ủy quyền</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label13">Chọn tập tin </label>
                                    <div class="col-lg-9">
                                        <asp:FileUpload runat="server" ID="fileThongTinNguoiDaiDienUyQuyen" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label14">Tên hồ sơ </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textTenHoSoNguoiDaiDienUyQuyen" runat="server" CssClass="form-control float-left requirement" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label15">Loại hồ sơ </label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="ddlistLoaiHoSoThongTinNguoiDaiDienUyQuyen" runat="server" CssClass="form-control select2bs4">
                                            <asp:ListItem Value="" Text="Chọn loại hồ sơ"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Đơn kiến nghị, phản ánh"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Phiếu chuyển đơn"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Đơn tố cáo"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Đơn khiếu nại"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Loại khác"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group row">
                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label16">Nội dung tóm tắt </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="textNoiDungTomTatFileNguoiDaiDienUyQuyen" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control float-left" MaxLength="500" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-tools mr-b10 text-center">
                            <asp:LinkButton ID="buttonLuuHoSoNguoiDaiDienUyQuyen" OnClick="LuuHoSoNguoiDaiDienUyQuyen" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Lưu</asp:LinkButton>
                            <button class="btn  btn-flat btn-md btn-outline-danger tool-left" causesvalidation="false" data-dismiss="modal"><i class="icofont-ui-close"></i>Đóng</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="buttonLuuHoSoNguoiDaiDienUyQuyen" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>

<%--<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/DesktopModules/HOPKHONGGIAY/Images/ajax-loader.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<div class="float-right OptionSearch OptionSearchSave" style="display: none;"></div>
<div class="modal fade" id="modal-default" data-backdrop="static">
    <div class="modal-dialog" style="width: 90%; max-width: 90%;">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Chọn đối tượng</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <section class="content ">
                            <div class="container-fluid">
                                <div class="card card-table-master">
                                    <div class="card-header sticky sticky-popup" style="top: 0px !important">
                                        <%-- <h3 class="card-title">Danh sách tiếp dân</h3>--%>
                                        <asp:Panel runat="server" DefaultButton="buttonSearch" CssClass="card-filter card-filter-full">
                                            <%--<div class="card-filter">--%>
                                            <!-- Text search -->
                                            <div class="form-group row">

                                                <div class="col-lg-4">
                                                    <asp:DropDownList ID="BS_drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="BS_drlDIAPHUONG_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:DropDownList ID="BS_drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="BS_drlDIAPHUONG_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:DropDownList ID="BS_drlXa" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="BS_drlDIAPHUONG_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-lg-12">
                                                <div class="input-group input-group tool-right">

                                                    <asp:Literal runat="server" ID="Literal_OptionSearch">
                                                    <div class="float-right OptionSearch OptionSearchDisplay"  data-save="false"></div>
                                                    </asp:Literal>
                                                    <asp:TextBox runat="server" ID="textSearchContent_HiddenField" CssClass="form-control float-right textSearchContent_HiddenField" placeholder="Nhập từ khóa tìm kiếm" Style="min-width: 100px;"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="textSearchContent" CssClass="form-control float-right" placeholder="Nhập từ khóa tìm kiếm" Style="min-width: 100px;"></asp:TextBox>

                                                    <div class="input-group-append d-none filter-toggler">
                                                        <button class="navbar-toggler btn btn-default btn-flat" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                                                            <i class="icofont-filter"></i>
                                                        </button>
                                                    </div>
                                                    <div class="input-group-append ">
                                                        <asp:LinkButton ID="buttonSearch" runat="server" CssClass="btn btn-default btn-flat" OnClick="btnSearch_Click"> <i class="fas fa-search"></i></asp:LinkButton>
                                                        <%----%>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--</div>--%>
                                        </asp:Panel>
                                        <div class="card-tools">
                                            <div class="tool-right">
                                                <div class="card-filter-advance">
                                                    <nav class="navbar navbar-expand-lg ">
                                                        <div class="collapse navbar-collapse" id="navbarNavDropdown">
                                                            <ul class="navbar-nav">
                                                                <li class="nav-item dropdown dropdown-filter">
                                                                    <button type="button" class="btn btn-tool dropdown-toggle" aria-expanded="false" data-toggle="dropdown">
                                                                        <i class="icofont-filter"></i>Bộ lọc
                                                                    </button>
                                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                                        <div runat="server" id="FilterTiepDan">
                                                                            <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_STT" data-type="normal" data-title="Số thứ tự"><i class="icofont-check"></i>Số thứ tự tiếp dân</a>
                                                                            <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_HOTEN" data-type="normal"><i class="icofont-check"></i>Họ tên</a>
                                                                            <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_DIACHI_DAYDU" data-type="normal"><i class="icofont-check"></i>Địa chỉ</a>
                                                                            <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_NOIDUNG" data-type="normal"><i class="icofont-check"></i>Nội dung tiếp dân</a>
                                                                            <div class="dropdown-divider"></div>
                                                                            <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_THOGIAN" data-type="datetime"><i class="icofont-check"></i>Thời gian</a>
                                                                            <div class="datetime-content" data-option="TIEPDAN.TIEPDAN_THOGIAN"></div>
                                                                            <a href="#" class="dropdown-item" data-option="TIEPDAN.NGAYTAO" data-type="datetime"><i class="icofont-check"></i>Ngày tạo</a>
                                                                            <div class="datetime-content" data-option="TIEPDAN.NGAYTAO"></div>
                                                                        </div>
                                                                        <div runat="server" id="FilterDonThu">
                                                                            <a href="#" class="dropdown-item" data-option="DONTHU.DONTHU_STT" data-type="normal" data-title="Số thứ tự đơn thư"><i class="icofont-check"></i>Số thứ tự đơn thư</a>
                                                                            <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_HOTEN" data-type="normal"><i class="icofont-check"></i>Họ tên</a>
                                                                            <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_DIACHI_DAYDU" data-type="normal"><i class="icofont-check"></i>Địa chỉ</a>
                                                                            <a href="#" class="dropdown-item" data-option="DONTHU.DONTHU_NOIDUNG" data-type="normal"><i class="icofont-check"></i>Nội dung đơn thư</a>
                                                                            <div class="dropdown-divider"></div>
                                                                            <a href="#" class="dropdown-item" data-option="DONTHU.NGUONDON_NGAYDEDON" data-type="datetime"><i class="icofont-check"></i>Ngày đề đơn</a>
                                                                            <div class="datetime-content" data-option="DONTHU.NGUONDON_NGAYDEDON"></div>
                                                                            <a href="#" class="dropdown-item" data-option="DONTHU.NGAYTAO" data-type="datetime"><i class="icofont-check"></i>Ngày tạo</a>
                                                                            <div class="datetime-content" data-option="DONTHU.NGAYTAO"></div>
                                                                        </div>
                                                                    </div>
                                                                </li>

                                                                <%-- <li class="nav-item dropdown dropdown-filter">
                                                                    <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                                                        <i class="icofont-checked"></i>Trạng thái
                                                                    </button>
                                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                                        <a href="#" id="codon" class="dropdown-item" data-option="TIEPDAN.DONTHU_ID" data-type="equal" data-value="is not null"><i class="icofont-check"></i>Có đơn</a>
                                                                        <a href="#" id="khongdon" class="dropdown-item" data-option="TIEPDAN.DONTHU_ID" data-type="equal" data-value="is null"><i class="icofont-check"></i>Không đơn</a>
                                                                    </div>
                                                                </li>--%>
                                                            </ul>
                                                        </div>
                                                    </nav>
                                                </div>
                                                <div class="card-list-type">
                                                    <div id="divShowBtnXoa" style="display: none;">
                                                        <asp:LinkButton ID="btnChonDoiTuong" Visible="true" runat="server" OnClick="btnChonDoiTuong_Click" CommandArgument="ChonDoiTuong" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-checked"></i> Chọn</asp:LinkButton>
                                                        <asp:LinkButton ID="btnLayThongTinDoiTuong" Visible="true" CausesValidation="true" runat="server" OnClick="btnChonDoiTuong_Click" CommandArgument="LayThongTinDoiTuong" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-copy"></i> Lấy thông tin</asp:LinkButton>
                                                    </div>
                                                    <button class="btn  btn-flat btn-md btn-outline-danger tool-left" causesvalidation="false" data-dismiss="modal"><i class="icofont-close-line-circled"></i>Đóng</button>
                                                    <%--<div class="btn-group">
                                                        <button type="button" class="btn btn-default btn-sm btn-flat"><i class="fa fa-th-large" aria-hidden="true"></i></button>
                                                        <button type="button" class="btn btn-default btn-sm btn-flat active "><i class="fa fa-list" aria-hidden="true"></i></button>
                                                        <button type="button" class="btn btn-default btn-sm btn-flat"><i class="fa fa-th" aria-hidden="true"></i></button>
                                                    </div>--%>
                                                </div>


                                                <div class="card-pagination">
                                                    <div class="btn-group tool-right pagination-group">
                                                        <span class="pagination-title">
                                                            <asp:TextBox runat="server" ID="txtRecordStartEnd" AutoPostBack="true" OnTextChanged="txtRecordStartEnd_TextChanged" CssClass="form-control float-left text-right form-control-sm" Width="50" placeholder=""></asp:TextBox>
                                                            /
                                            <asp:Label runat="server" ID="lblTotalRecords" Text="" />
                                                        </span>
                                                        <asp:LinkButton ID="LinkButtonPrevious" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonPrevious_Click">&lt;</asp:LinkButton>
                                                        <asp:LinkButton ID="LinkButtonLast" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonLast_Click">&gt;</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--<div class="tool-right">
                                            </div>-->
                                            <div class="tool-left">
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.card-header -->
                                    <div class="card-body p-0">
                                        <div class="table-content p-0">
                                            <asp:DataGrid runat="server" ID="dgDanhSach" AutoGenerateColumns="False" BorderWidth="0"
                                                AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound" CssClass="table vertical-align-middle">
                                                <HeaderStyle CssClass="table-header" />
                                                <Columns>
                                                    <asp:TemplateColumn Visible="true" HeaderStyle-CssClass="">
                                                        <HeaderTemplate>
                                                            Chọn
                                                        </HeaderTemplate>
                                                        <HeaderStyle Width="3%" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" Width="3%" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" onclick="handle_checked_one_row(this,'divShowBtnXoa');" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="TIEPDAN_ID" HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_ID")!=null?Eval("TIEPDAN_ID").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="DONTHU_ID" HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("DONTHU_ID")!=null?Eval("DONTHU_ID").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" SortExpression="TIEPDAN_STT" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a title="Cập nhật thông tin tiếp dân" href='<%# Eval("TIEPDAN_ID")!=null?Eval("TIEPDAN_ID").ToString():"" %>' oncontextmenu="return false" runat="server"><%# Eval("TIEPDAN_STT")!=null?Eval("TIEPDAN_STT").ToString():"" %> </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" SortExpression="DONTHU_STT" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a title="Cập nhật thông tin tiếp dân" href='<%# Eval("DONTHU_ID")!=null?Eval("DONTHU_ID").ToString():"" %>' oncontextmenu="return false" runat="server"><%# Eval("DONTHU_STT")!=null?Eval("DONTHU_STT").ToString():"" %> </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Họ tên" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="" ItemStyle-Width="25%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Loại đối tượng" HeaderStyle-HorizontalAlign="Left" SortExpression="DOITUONG_LOAI" HeaderStyle-CssClass="" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#  Eval("DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG_TEN").ToString()) %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Loại tiếp dân" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_LOAI")!=null?GetLoaiDonThu(Eval("TIEPDAN_LOAI").ToString()) :"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Loại đơn thư" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="" HeaderStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("LOAIDONTHU_ID")!=null?GetLoaiDonThu(Eval("LOAIDONTHU_ID").ToString()) :"" %> <%--(!=null?int.Parse(Eval("LOAIDONTHU_ID").ToString()):0))--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_NOIDUNG" HeaderStyle-CssClass="" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_NOIDUNG")!=null?Eval("TIEPDAN_NOIDUNG").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_NOIDUNG" HeaderStyle-CssClass="" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("DONTHU_NOIDUNG")!=null?Eval("DONTHU_NOIDUNG").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Kết quả tiếp dân" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_KETQUA" HeaderStyle-CssClass="" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_KETQUA")!=null?Eval("TIEPDAN_KETQUA").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Lần tiếp " HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_KETQUA" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_LANTIEP")!=null?Eval("TIEPDAN_LANTIEP").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <%--<asp:TemplateColumn HeaderText="CB nhập đơn/CB xử lý" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Cơ quan GQ tiếp/ Người nhận GQ tiếp" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>--%>
                                                    <asp:TemplateColumn HeaderText="Hướng xử lý" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("HUONGXULY_TEN")!=null?Eval("HUONGXULY_TEN").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Số hiệu văn bản đi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%# Eval("HUONGXULY_SOHIEUVB_DI")!=null?Eval("HUONGXULY_SOHIEUVB_DI").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Thời gian tiếp" HeaderStyle-HorizontalAlign="Center" SortExpression="TIEPDAN_THOGIAN" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("TIEPDAN_THOGIAN")!=null?Eval("TIEPDAN_THOGIAN").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Ngày đề đơn" HeaderStyle-HorizontalAlign="Center" SortExpression="NGUONDON_NGAYDEDON" HeaderStyle-CssClass="">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%# Eval("NGUONDON_NGAYDEDON")!=null?Eval("NGUONDON_NGAYDEDON_FOTMAT").ToString():"" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999" Visible="false"></PagerStyle>
                                            </asp:DataGrid>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>
                                <!-- /.container-fluid -->
                            </div>
                        </section>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>


<asp:UpdatePanel ID="upn" runat="server" UpdateMode="Always">
    <ContentTemplate>

        <!-- /.modal -->
        <div class="wrapper-form">
            <%--<body class="sidebar-mini layout-footer-fixed layout-navbar-fixed layout-fixed">--%>
            <!-- Content Wrapper. Contains page content -->
            <!-- Content Header (Page header) -->
            <section class="content-header sticky">
                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-6">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item">
                                    <asp:LinkButton ID="btnBreadcrumbBack" runat="server" OnClick="btnBreadcrumbBack_Click">Quản lý tiếp dân</asp:LinkButton>
                                    </a></li>
                                <li class="breadcrumb-item active">
                                    <asp:Label ID="lblBreadcrumbTitle" runat="server"></asp:Label></li>
                            </ol>
                        </div>
                    </div>
                </div>
                <!-- /.container-fluid -->
                <div class="form-tools">
                    <asp:LinkButton ID="btnSua" Visible="true" OnClick="btnSua_Click" CausesValidation="true" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" OnClick="btnCapNhat_Click" Visible="false" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                     <asp:LinkButton ID="btnNhanBan" Visible="true" OnClick="btnNhanBan_Click" CausesValidation="true" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Nhân bản</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-md btn-default btn-flat text-primary tool-left" CausesValidation="false" OnClick="btnBoQua_Click"><i class='icofont-undo' ></i> Trở về</asp:LinkButton>
                </div>
            </section>

            <!-- Main content -->
            <section class="content mr-t10">

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header hidden">
                                    <div class="card-tools">
                                        <a class="btn btn-app">
                                            <span class="badge bg-warning">3</span>
                                            <i class="fas fa-bullhorn"></i>Thông báo
                                        </a>
                                        <a class="btn btn-app">
                                            <span class="badge bg-success">2</span>
                                            <i class="fas fa-barcode"></i>Sản phẩm
                                        </a>
                                        <a class="btn btn-app">
                                            <!-- <span class="badge bg-purple">891</span> -->
                                            <i class="icon icofont-ui-user"></i>Tài khoản KH
                                        </a>
                                        <a class="btn btn-app">
                                            <span class="badge bg-teal">1</span>
                                            <i class="icon icofont-credit-card"></i>Thanh toán
                                        </a>
                                        <a class="btn btn-app">
                                            <span class="badge bg-info">2</span>
                                            <i class="icon icofont-dropbox"></i>Đóng gói
                                        </a>
                                        <a class="btn btn-app">
                                            <span class="badge bg-danger">1</span>
                                            <i class="icon icofont-retweet"></i>Đổi trả
                                        </a>
                                        <button type="button" class="btn btn-tool d-none d-lg-inline-flex  d-xl-inline-flex" data-card-widget="maximize">
                                            <i class="fas fa-expand"></i>
                                        </button>
                                        <div class="btn-group  d-none d-lg-inline-flex d-xl-inline-flex">
                                            <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                                <i class="fas fa-wrench"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                <a href="#" class="dropdown-item">Action</a>
                                                <a href="#" class="dropdown-item">Another action</a>
                                                <a href="#" class="dropdown-item">Something else here</a>
                                                <a class="dropdown-divider"></a>
                                                <a href="#" class="dropdown-item">Separated link</a>
                                            </div>
                                        </div>
                                        <!--<button type="button" class="btn btn-tool" data-card-widget="remove">
                          <i class="fas fa-times"></i>
                    </button>-->
                                    </div>
                                </div>
                                <!-- /.card-header -->
                                <div class="form-horizontal">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <h4>Thông tin tiếp dân</h4>
                                            </div>
                                            <div class="col-lg-6 text-right">
                                                <h2><b>
                                                    <asp:Label runat="server" ID="lblSTT" CssClass="text-primary"> </asp:Label></b></h2>
                                                <h6>
                                                    <asp:Label runat="server" ID="lblNgayTiepDan" CssClass="text-primary"> </asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Đơn thư</label>
                                                    <div class="col-lg-9">
                                                        <asp:RadioButton class="" ID="rdoCoDon" runat="server" GroupName="grdoDonThu" Text="Có đơn" AutoPostBack="true" OnCheckedChanged="rdoDon_CheckedChanged" />
                                                        <asp:RadioButton class="" ID="rdoKhongDon" runat="server" GroupName="grdoDonThu" Text="Không đơn" AutoPostBack="true" OnCheckedChanged="rdoDon_CheckedChanged"></asp:RadioButton>

                                                        <%--<div class="custom-control custom-radio">
                                                            <input class="custom-control-input" type="radio" id="rdoCoDon" runat="server" checked name="customRadioDonThu">
                                                            
                                                            <label for='<%=rdoCoDon.ClientID%>' class="custom-control-label">Có đơn</label>
                                                        </div>
                                                        <div class="custom-control custom-radio">
                                                           
                                                            <input class="custom-control-input" type="radio" id="rdoKhongDon" runat="server" checked name="customRadioDonThu" AutoPostBack="true" onserverchange="rdoKhongDon_ServerChange">
                                                            <asplabel for='<%=rdoKhongDon.ClientID %>' class="custom-control-label">Không đơn</asplabel>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label id="lbltxtNgayTiepDan" runat="server" class="col-lg-3 col-form-label">Ngày tiếp</label>
                                                    <div class="col-lg-5">
                                                        <div class='input-group datecontrol date reservationdate_ngaytiepdan' data-target-input="nearest">
                                                            <asp:TextBox ID="txtNgayTiepDan" runat="server" CssClass="form-control datetimepicker-input requirement " placeholder="" />
                                                            <div class="input-group-append" data-target='.reservationdate_ngaytiepdan' data-toggle="datetimepicker">
                                                                <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                    </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Đối tượng</label>
                                                    <div class="col-lg-9">
                                                        <asp:HiddenField runat="server" ID="hdfDoiTuongID" />
                                                        <asp:HiddenField runat="server" ID="hdfTiepDanID" />
                                                        <asp:HiddenField runat="server" ID="hdfDonThuID" />
                                                        <asp:HiddenField runat="server" ID="hdfBiKhieuNaiToCao" />
                                                        <asp:HiddenField runat="server" ID="hdfCanNhanDaiDienUyQuyen" />
                                                        <asp:HiddenField runat="server" ID="hdfIsCoppy" />
                                                        <asp:DropDownList ID="drpDOITUONG" runat="server" CssClass="form-control select2bs4 float-left" AutoPostBack="true" OnSelectedIndexChanged="drpDOITUONG_SelectedIndexChanged">
                                                            <asp:ListItem Text="Cá nhân" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Nhóm đông người" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Cơ quan tổ chức" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:LinkButton ID="btnChonNguoiDaiDien" runat="server" CssClass="btn btn-sm  btn-flat text-primary float-left" data-toggle="modal" data-target="#modal-default"><i class="icofont-maximize h4"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6" runat="server" id="panelSoNguoi">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Số người</label>
                                                    <div class="col-lg-3">
                                                        <asp:TextBox type="text" class="form-control requirement" ID="txtSoNguoi" data-mask runat="server" placeholder="" data-inputmask="'mask':'[9][9]'" inputmode="numeric"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-lg-6" runat="server" id="panelSoNguoiDaiDien">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Số người đại diện</label>
                                                    <div class="col-lg-3">
                                                        <asp:TextBox type="text" class="form-control requirement" ID="txtSoNguoiDaiDien" data-mask runat="server" placeholder="" data-inputmask="'mask': '[9][9]'" inputmode="numeric" AutoPostBack="true" OnTextChanged="txtSoNguoiDaiDien_TextChanged" Text="1"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-5">
                                                        <asp:LinkButton ID="btn_ThemNguoiDaiDien" runat="server" OnClick="btn_ThemNguoiDaiDien_Click" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i> THÊM NGƯỜI ĐẠI DIỆN</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="PanelDoiTuong" runat="server" visible="false">
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Tên cơ quan, tổ chức</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="txtTenCoQuanToChuc" runat="server" CssClass="form-control requirement float-left" MaxLength="500" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Địa chỉ</label>

                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="txtDiaChiDoiTuong" runat="server" CssClass="form-control  float-left" MaxLength="500" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:ListView runat="server" ID="ListViewDoiTuong" OnItemCreated="ListViewDoiTuong_ItemCreated" DataKeyNames="">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <h5>Thông tin người đại diện</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelHoTen">Họ tên</label>
                                                            <div class="col-lg-9">
                                                                <%--<input type="text" class="form-control requirement float-left" id="inputHoTen" value="Nguyễn Hoàng Tấn Tài" placeholder="Họ tên">--%>
                                                                <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control requirement float-left" MaxLength="500" Text='<%# Eval("CANHAN_HOTEN")!=null?Eval("CANHAN_HOTEN").ToString():"" %>' />
                                                                <asp:TextBox ID="txtCaNhanID" runat="server" Visible="false" CssClass="form-control  float-left" MaxLength="500" Text='<%# Eval("CANHAN_ID").ToString() %>' />


                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelKetQua"></label>
                                                            <div class="col-lg-3">
                                                            </div>
                                                            <div class="col-lg-3">
                                                                <a onserverclick="Xoa_CaNhan_ServerClick" id="Xoa_CaNhan" href='<%# Eval("CANHAN_ID").ToString() %>' oncontextmenu="return false" runat="server">Xóa</a>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">CMND/ Thẻ căn cước</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="txtCMND" runat="server" CssClass="form-control" data-mask  data-inputmask="'mask':'[9][9][9][9][9][9][9][9][9][9][9][9]'" inputmode="numeric" MaxLength="500" Text='<%# Eval("CANHAN_CMDN")!=null?Eval("CANHAN_CMDN").ToString():""  %>' />

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Giới tính</label>
                                                            <div class="col-lg-9">
                                                                <div class="custom-control custom-radio">
                                                                    <input class="custom-control-input" type="radio" id="rdoNam" runat="server" groupname="grdoGioiTinh" name="customRadio" checked='<%# !bool.Parse(Eval("CANHAN_GIOITINH").ToString()) %>'><%--<%# !bool.Parse(Eval("CANHAN_GIOITINH").ToString()).ToString() %>--%>
                                                                    <label class="custom-control-label" runat="server" id="lblforrdoNam">Nam</label>
                                                                </div>
                                                                <div class="custom-control custom-radio">
                                                                    <input class="custom-control-input rdoNu" type="radio" id="rdoNu" runat="server" name="customRadio" checked='<%# bool.Parse(Eval("CANHAN_GIOITINH").ToString()) %>'><%--checked='<%# bool.Parse(Eval("CANHAN_GIOITINH").ToString()) %>'--%>
                                                                    <label class="custom-control-label" runat="server" id="lblforrdoNu">Nữ</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Ngày cấp</label>
                                                            <div class="col-lg-9">
                                                                <div class='input-group datecontrol date reservationdate<%# Eval("CANHAN_ID").ToString() %>' data-target-input="nearest">
                                                                    <asp:TextBox ID="txtNgayCap" runat="server" CssClass="form-control datetimepicker-input" placeholder="" Text='<%# Eval("CANHAN_CMDN_NGAYCAP")!=null?DateTime.Parse(Eval("CANHAN_CMDN_NGAYCAP").ToString()).ToString("dd/MM/yyyy"):"" %>' />
                                                                    <div class="input-group-append" data-target='.reservationdate<%# Eval("CANHAN_ID").ToString() %>' data-toggle="datetimepicker">
                                                                        <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Nơi cấp</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="txtNoiCap" runat="server" CssClass="form-control " MaxLength="500" placeholder="" Text='<%# Eval("CANHAN_NOICAP")!=null?Eval("CANHAN_NOICAP").ToString():""   %>' />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Địa chỉ</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-control " MaxLength="500" placeholder="" Text='<%# Eval("CANHAN_DIACHI")!=null?Eval("CANHAN_DIACHI").ToString():""    %>' />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh/thành phố</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlDIAPHUONG_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quận/huyện</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlDIAPHUONG_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Xã/Phường</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="drlXa" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quốc tịch</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="drlQuocTich" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Dân tộc</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="drlDanToc" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>

                                        <div runat="server" id="contentTiepDan">
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" runat="server" id="lbldrlLoaiTiepDan" class="col-lg-3 col-form-label">Loại tiếp dân</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="drlLoaiTiepDan" runat="server" CssClass="form-control select2bs4 requirement" OnSelectedIndexChanged="drlLoaiKieuNai_TiepDan_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label">Loại khiếu nại</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="drlLoaiKieuNai" runat="server" CssClass="form-control select2bs4 " OnSelectedIndexChanged="drlLoaiKieuNai_TiepDan_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label">Chi tiết loại khiếu nại</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="drlLoaiChiTiet" runat="server" CssClass="form-control select2bs4 ">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label">Lần tiếp</label>
                                                        <div class="col-lg-3">
                                                            <asp:TextBox type="text" class="form-control requirement" ID="txtLanTiep" Text="1" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelNoiDungTiepDan">Nội dung tiếp dân</label>
                                                        <div class="col-lg-9">
                                                            <%--calc((100% * 2) + (100% * 0.36) - 50px) !important--%>
                                                            <asp:TextBox ID="txtNoiDungTiepDan" runat="server" CssClass="form-control  requirement" TextMode="MultiLine" Rows="5" AutoPostBack="true">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelKetQua">Kết quả tiếp dân</label>
                                                        <div class="col-lg-9">
                                                            <%--calc((100% * 2) + (100% * 0.36) - 50px) !important--%>
                                                            <asp:TextBox ID="txtKetQua" runat="server" CssClass="form-control  requirement" TextMode="MultiLine" Rows="5">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div runat="server" id="contentDonThu">
                                            <div class="row" style="padding-top: 15px">
                                                <div class="col-lg-6">
                                                    <h4>Thông tin đơn thư</h4>
                                                </div>
                                                <div class="col-lg-6 text-right">
                                                    <h1><b>
                                                        <asp:Label runat="server" ID="label1" CssClass="text-primary"> </asp:Label></b></h1>
                                                </div>
                                            </div>
                                            <div runat="server" id="div"></div>
                                            <div class="row" runat="server" id="divDonThuKhongDuDieuKien">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label"></label>
                                                        <div class="col-lg-9">
                                                            <asp:CheckBox runat="server" ID="cboxDonThuKhongDuDieuKien" CssClass="cbCustom" OnCheckedChanged="cboxDonThuKhongDuDieuKien_CheckedChanged" AutoPostBack="true" Text="Đơn thư không đủ điều kiện" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <%--<label for="inputPhone" class="col-lg-3 col-form-label">Đơn cũ</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="ddlistDonCu" runat="server" CssClass="form-control select2bs4 requirement" >
                                                            </asp:DropDownList>
                                                        </div>--%>
                                                        <asp:Label runat="server" ID="lblNgayNhanDon" for="" class="col-lg-3 col-form-label">Ngày nhận đơn</asp:Label>
                                                        <div class="col-lg-9">
                                                            <div class='input-group datecontrol date reservationdate_ngaynhandon' data-target-input="nearest">
                                                                <asp:TextBox ID="txtNgayNhanDon" runat="server" CssClass="form-control datetimepicker-input requirement " placeholder="" />
                                                                <div class="input-group-append" data-target='.reservationdate_ngaynhandon' data-toggle="datetimepicker">
                                                                    <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                         <asp:Label runat="server" ID="lblNgayDeDon" for="" class="col-lg-3 col-form-label">Ngày đề đơn</asp:Label>                                                     
                                                        <div class="col-lg-9">
                                                            <div class='input-group datecontrol date reservationdate_ngaydedon' data-target-input="nearest">
                                                                <asp:TextBox ID="txtNgayDeDon" runat="server" CssClass="form-control datetimepicker-input requirement" placeholder="" />
                                                                <div class="input-group-append" data-target='.reservationdate_ngaydedon' data-toggle="datetimepicker">
                                                                    <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <asp:Label runat="server" ID="lbllistLoaDonThu" for="" class="col-lg-3 col-form-label">Loại đơn thư</asp:Label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="ddlistLoaDonThu" runat="server" CssClass="form-control select2bs4 requirement" OnSelectedIndexChanged="drlLoaiKieuNai_DonThu_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label">Loại khiếu nại</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="ddlistLoaiKhieuNai" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlLoaiKieuNai_DonThu_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label">Chi tiết loại khiếu nại</label>
                                                        <div class="col-lg-9">
                                                            <asp:DropDownList ID="ddlistLoaiKhieuNaiChiTiet" runat="server" CssClass="form-control select2bs4">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <asp:Label runat="server" ID="lblNoiDungDonThu" for="" class="col-lg-3 col-form-label">Nội dung đơn thư</asp:Label>
                                                        <div class="col-lg-9">
                                                            <%--calc((100% * 2) + (100% * 0.36) - 50px) !important--%>
                                                            <asp:TextBox ID="textNoiDungDonThu" runat="server" CssClass="form-control  requirement" TextMode="MultiLine" Rows="5">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label2"></label>
                                                        <div class="col-lg-9">
                                                            <asp:LinkButton ID="buttonThemTaiLieu" runat="server" CssClass="btn btn-default btn-flat" data-toggle="modal" data-target="#modal-default-hosodonthu"> <i class="icofont-plus"></i> Thêm tập tin đính kèm</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--<div class="card-body p-0">
                                            <div class="table-content p-0">--%>
                                            <div class="row text-center">
                                                <div class="col-lg-9" style="margin-left: auto; margin-right: auto;">
                                                    <asp:DataGrid DataKeyField="HOSO_ID" runat="server" ID="dgDanhSach_File_HoSoDonThu" AutoGenerateColumns="False" CssClass="table vertical-align-middle">
                                                        <HeaderStyle CssClass="table-header" />
                                                        <Columns>
                                                            <asp:BoundColumn HeaderText="HOSO_ID" DataField="HOSO_ID" Visible="false"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Tên hồ sơ" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TEN" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Nội dung tóm tắt" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TOMTAT" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="Tải về" Visible="true">
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadHoSo+"/" +Eval("HOSO_FILE")%>' runat="server"><i class="icofont-download"></i></a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="Xóa" Visible="true">
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a onserverclick="XoaHoSoDonThu" title="Xóa tài liệu" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa câu hỏi này?');" href='<%# Eval("HOSO_ID").ToString()%>' oncontextmenu="return false" runat="server">
                                                                        <i class="icofont-ui-delete"></i>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </div>
                                            </div>
                                            <br />
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <label for="inputPhone" class="col-lg-3 col-form-label"></label>
                                                        <div class="col-lg-9">
                                                            <asp:CheckBox runat="server" ID="cboxCoQuanDaGiaiQuyet" CssClass="cbCustom" AutoPostBack="true" Text="Thông tin cơ quan đã giải quyết" OnCheckedChanged="cbCoQuanDaGiaiQuyet_CheckedChanged" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="divCoQuanDaGiaiQuyet" visible="false">
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="lbllistCoQuanDaGiaiQuyet">Cơ quan đã giải quyết</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistCoQuanDaGiaiQuyet" runat="server" CssClass="form-control select2bs4 requirement" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Lần giải quyết</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textLanGiaiQuyet" runat="server" CssClass="form-control" Style="width: 40% !important">
                                                                </asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Ngày ban hành quyết định</label>
                                                            <div class="col-lg-9">
                                                                <div class='input-group datecontrol date reservationdateNgayBanHanhQD' data-target-input="nearest">
                                                                    <asp:TextBox ID="textNgayBanHanhQuyetDinh" runat="server" CssClass="form-control datetimepicker-input" placeholder="" />
                                                                    <div class="input-group-append" data-target='.reservationdateNgayBanHanhQD' data-toggle="datetimepicker">
                                                                        <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Hình thức giải quyết</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistHinhThucGiaiQuyet" runat="server" CssClass="form-control select2bs4">
                                                                    <asp:ListItem Value="" Text="Chọn hình thức giải quyết"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="Chưa có quyết định giải quyết"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Quyết định lần 1"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Quyết định lần 2"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="Quyết định cuối cùng"></asp:ListItem>
                                                                    <asp:ListItem Value="4" Text="Bản án Sơ thẩm"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="Bản án Phúc thẩm"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="Bản án Tái thẩm"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="Quyết định giám đốc thẩm"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="Trả lời bằng văn bản"></asp:ListItem>
                                                                    <asp:ListItem Value="9" Text="Kết luận, thông báo, báo cáo"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Kết quả của cơ quan giải quyết</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textKetQuaCuaCoQuanGiaiQuyet" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tệp đính kèm</label>
                                                            <div class="col-lg-9">
                                                                <asp:FileUpload runat="server" ID="fileCoQuanDaGiaiQuyet" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                    </div>
                                                </div>

                                                <br />
                                                <br />
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <div class="col-lg-3"></div>
                                                        <div class="col-lg-9">
                                                            <asp:CheckBox runat="server" ID="cboxBoSungThongTinNguoiBiKhieuNaiToCao" CssClass="cbCustom" OnCheckedChanged="cbBoSungThongTinNguoiBiKhieuNaiToCao_CheckedChanged" AutoPostBack="true" Text="Bổ sung thông tin người bị khiếu nại; tố cáo " />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="divThongTinNguoiBiKhieuNaiToCao" visible="false">
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Đối tượng</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistDoiTuongBoSung" runat="server" OnSelectedIndexChanged="ChonLoaiDoiTuongBiKhieuNaoToCao" CssClass="form-control select2bs4 requirement" AutoPostBack="true">
                                                                    <asp:ListItem Value="1" Text="Cá nhân"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="Cơ quan/tổ chức"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" runat="server" id="divTenCoQuanBiKhieuNaiToCao" visible="false">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="lblTenCoQuanToChuc_BiKhieuNaiToCao">Tên cơ quan/tổ chức</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textTenCoQuanToChuc_BiKhieuNaiToCao" runat="server" CssClass="form-control requirement">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div runat="server" id="divTenCaNhan_BiKhieuNaiToCao">
                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="lblHoTen_NguoiBiKhieuNaiToCao">Họ tên</label>
                                                                <div class="col-lg-9">
                                                                    <asp:TextBox ID="textHoTen_NguoiBiKhieuNaiToCao" runat="server" CssClass="form-control requirement">
                                                                    </asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Giới tính</label>
                                                                <div class="col-lg-9">
                                                                    <div class="custom-control custom-radio">
                                                                        <input class="custom-control-input rdoDoiTuongNam" type="radio" id="rdoDoiTuongNam" runat="server" groupname="grdoGioiTinh1111" name="customRadio1" checked="true"><%--<%# !bool.Parse(Eval("CANHAN_GIOITINH").ToString()).ToString() %>--%>
                                                                        <label class="custom-control-label" id="lblforrdoDoiTuongNam" for='<%= rdoDoiTuongNam.ClientID%>'>Nam</label>
                                                                    </div>
                                                                    <div class="custom-control custom-radio">
                                                                        <input class="custom-control-input rdoDoiTuongNu" type="radio" id="rdoDoiTuongNu" runat="server" name="customRadio1"><%--checked='<%# bool.Parse(Eval("CANHAN_GIOITINH").ToString()) %>'--%>
                                                                        <label class="custom-control-label" id="lblforrdoDoiTuongNu" for='<%=rdoDoiTuongNu.ClientID %>'>Nữ</label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Nơi công tác</label>
                                                                <div class="col-lg-9">
                                                                    <asp:TextBox ID="textNoiCongTac" runat="server" CssClass="form-control">
                                                                    </asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Chức vụ</label>
                                                                <div class="col-lg-9">
                                                                    <asp:TextBox ID="textChucVu" runat="server" CssClass="form-control">
                                                                    </asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Địa chỉ</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textDiaChi" runat="server" CssClass="form-control">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh thành</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistTinhThanh" runat="server" OnSelectedIndexChanged="ChonDiaPhuongNguoiBiKhieuNaiToCao" CssClass="form-control select2bs4" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quận/huyện</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistQuanHuyen" runat="server" OnSelectedIndexChanged="ChonDiaPhuongNguoiBiKhieuNaiToCao" CssClass="form-control select2bs4" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Xã phường</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistXaPhuong" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" runat="server" id="divQuocTichDanToc_NguoiBiKhieuNaiToCao">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quốc tịch</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistQuocTich" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Dân tộc</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistDanToc" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <br />
                                                <br />
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <div class="form-group row">
                                                        <div class="col-lg-3"></div>
                                                        <div class="col-lg-9">
                                                            <asp:CheckBox runat="server" ID="cboxBoSungThongTinNguoiDaiDienUyQuyen" OnCheckedChanged="cbBoSungThongTinNguoiDaiDienUyQuyem_CheckedChanged" CssClass="cbCustom" AutoPostBack="true" Text="Bổ sung thông tin người đại diện, ủy quyền" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="divThongTinNguoiDaiDienUyQuyen" visible="false">
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="lblHoTenNguoiDaiDien">Họ tên</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textHoTenNguoiDaiDien" runat="server" CssClass="form-control requirement">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Giới tính</label>
                                                            <div class="col-lg-9">
                                                                <div class="custom-control custom-radio">
                                                                    <input class="custom-control-input" type="radio" id="rdoDaiDienUyQuyenNam" runat="server" groupname="grdoDaiDienUyQuyenGioiTinh" name="customRadio" checked='true'><%--<%# !bool.Parse(Eval("CANHAN_GIOITINH").ToString()).ToString() %>--%>
                                                                    <label class="custom-control-label" id="labelforrdoDaiDienUyQuyenNam" for="<%=rdoDaiDienUyQuyenNam.ClientID %>">Nam</label>
                                                                </div>
                                                                <div class="custom-control custom-radio">
                                                                    <input class="custom-control-input rdoNu" type="radio" id="rdoDaiDienUyQuyenNu" runat="server" name="customRadio"><%--checked='<%# bool.Parse(Eval("CANHAN_GIOITINH").ToString()) %>'--%>
                                                                    <label class="custom-control-label" id="labelforrdoDaiDienUyQuyenNu" for="<%=rdoDaiDienUyQuyenNu.ClientID %>">Nữ</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Địa chỉ</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textDiaChiNguoiDaiDienUyQuyen" runat="server" CssClass="form-control">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh thành</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistTinhThanhNguoiDaiDienUyQuyen" OnSelectedIndexChanged="ChonDiaPhuongNguoiDaiDienUyQuyen" runat="server" CssClass="form-control select2bs4" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quận/huyện</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistQuanHuyenNguoiDaiDienUyQuyen" OnSelectedIndexChanged="ChonDiaPhuongNguoiDaiDienUyQuyen" runat="server" CssClass="form-control select2bs4" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Xã phường</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistXaPhuongNguoiDaiDienUyQuyen" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quốc tịch</label>
                                                            <div class="col-lg-9">
                                                                <asp:DropDownList ID="ddlistQuocTichNguoiDaiDienUyQuyen" runat="server" CssClass="form-control select2bs4">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label12"></label>
                                                            <div class="col-lg-9">
                                                                <asp:LinkButton ID="buttonThemTapTinNguoiDaiDienUyQuyen" runat="server" CssClass="btn btn-default btn-flat" data-toggle="modal" data-target="#modal-default-hosonguoidaidienuyquyen"> <i class="icofont-plus"></i> Thêm tập tin đính kèm</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row text-center">
                                                    <div class="col-lg-9" style="margin-left: auto; margin-right: auto;">
                                                        <asp:DataGrid DataKeyField="HOSO_ID" runat="server" ID="dgDanhSach_File_NguoiDaiDienUyQuyen" AutoGenerateColumns="False" CssClass="table vertical-align-middle">
                                                            <HeaderStyle CssClass="table-header" />
                                                            <Columns>
                                                                <asp:BoundColumn HeaderText="HOSO_ID" DataField="HOSO_ID" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Tên hồ sơ" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TEN" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Nội dung tóm tắt" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TOMTAT" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="Tải về" Visible="true">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadHoSo+"/" +Eval("HOSO_FILE")%>' runat="server"><i class="icofont-download"></i></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:TemplateColumn HeaderText="Xóa" Visible="true">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a onserverclick="XoaHoSoDonThu_NguoiDaiDienUyQuyen" title="Xóa tài liệu" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa câu hỏi này?');" href='<%# Eval("HOSO_ID").ToString()%>' oncontextmenu="return false" runat="server">
                                                                            <i class="icofont-ui-delete"></i>
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </div>
                                                </div>
                                                <br />
                                                <br />
                                                <div class="row" style="padding-top: 15px">
                                                    <div class="col-lg-6">
                                                        <h4>Hướng xử lý</h4>
                                                    </div>
                                                    <div class="col-lg-6 text-right">
                                                        <h1><b>
                                                            <asp:Label runat="server" ID="label4" CssClass="text-primary"> </asp:Label></b></h1>
                                                    </div>
                                                </div>
                                                <div runat="server" id="divHuongXuLy">
                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Hướng xử lý</label>
                                                                <div class="col-lg-9">
                                                                    <asp:DropDownList ID="ddlistHuongXuLy" runat="server" CssClass="form-control select2bs4 requirement" AutoPostBack="true" OnSelectedIndexChanged="ChonHuongXuLy">
                                                             <%--           <asp:ListItem Value="" Text="Chọn hướng xử lý"></asp:ListItem>
                                                                        <asp:ListItem Value="1" Text="Thụ lý giải quyết"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="Hướng dẫn"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="Chuyển đơn"></asp:ListItem>
                                                                        <asp:ListItem Value="4" Text="Ra văn bản đôn đốc"></asp:ListItem>
                                                                        <asp:ListItem Value="5" Text="Ra văn bản thông báo"></asp:ListItem>
                                                                        <asp:ListItem Value="6" Text="Lưu và theo dõi"></asp:ListItem>
                                                                        <asp:ListItem Value="7" Text="Trả đơn"></asp:ListItem>
                                                                        <asp:ListItem Value="8" Text="Từ chối thụ lý"></asp:ListItem>
                                                                        <asp:ListItem Value="9" Text="Hướng dẫn - không có văn bản"></asp:ListItem>
                                                                        <asp:ListItem Value="10" Text="Khác"></asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Thẩm quyền giải quyết</label>
                                                                <div class="col-lg-9">
                                                                    <asp:DropDownList ID="ddlistThamQuyenGiaiQuyet" runat="server" CssClass="form-control select2bs4">
                                                                        <asp:ListItem Value="" Text="Chọn thẩm quyền giải quyết"></asp:ListItem>
                                                                        <asp:ListItem Value="1" Text="Cơ quan hành chính các cấp"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="Cơ quan tư pháp các cấp"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="Cơ quan Đảng"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div runat="server" id="divCoQuanTiepNhan_HuongXuLy" visible="false">
                                                        <div class="row">
                                                            <div class="col-lg-6">
                                                                <div class="form-group row">
                                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Cơ quan tiếp nhận</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:DropDownList ID="ddlistCoQuanTiepNhan" OnSelectedIndexChanged="ddlistCoQuanTiepNhan_SelectedIndexChanged" runat="server" CssClass="form-control select2bs4" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6">
                                                                <div class="form-group row">
                                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Người tiếp nhận</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:DropDownList ID="ddlistNguoiTiepNhan" runat="server" CssClass="form-control select2bs4">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-lg-6" runat="server" id="divNgayChuyen_HuongXuLy">
                                                                <div class="form-group row">
                                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Ngày chuyển</label>
                                                                    <div class="col-lg-9">
                                                                        <div class='input-group datecontrol date reservationdateNgayChuyen' data-target-input="nearest">
                                                                            <asp:TextBox ID="textNgayChuyen_HuongXuLy" runat="server" CssClass="form-control datetimepicker-input" placeholder="" />
                                                                            <div class="input-group-append" data-target='.reservationdateNgayChuyen' data-toggle="datetimepicker">
                                                                                <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-6">
                                                                <div class="form-group row">
                                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Số hiệu văn bản đi</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:TextBox ID="textSoHieuVanBanDi" runat="server" CssClass="form-control">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Người duyệt</label>
                                                                <div class="col-lg-9">
                                                                    <asp:DropDownList ID="ddlistNguoiDuyet" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="ddlistNguoiDuyet_SelectedIndexChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Chức vụ</label>
                                                                <div class="col-lg-9">
                                                                    <asp:TextBox ID="textChucVu_HuongXuLy" runat="server" CssClass="form-control">
                                                                    </asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-lg-6">
                                                            <div class="form-group row">
                                                                <label for="inputPhone" class="col-lg-3 col-form-label">Thời hạn giải quyết</label>
                                                                <div class="col-lg-9">
                                                                    <div class='input-group datecontrol date reservationdateThoiHanGiaiQuyet' data-target-input="nearest">
                                                                        <asp:TextBox Style="width: 50% !important" ID="textThoiHanGiaiQuyet" runat="server" CssClass="form-control datetimepicker-input" placeholder="" />
                                                                        <div class="input-group-append" data-target='.reservationdateThoiHanGiaiQuyet' data-toggle="datetimepicker">
                                                                            <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="lblYKienXuLy">Ý kiến xử lý</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textYKienXuLy" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control requirement">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">Ghi chú</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="textGhiChu" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control">
                                                                </asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="form-group row">
                                                            <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="label11"></label>
                                                            <div class="col-lg-9">
                                                                <asp:LinkButton ID="buttonThemHoSoHuongXuLy" runat="server" CssClass="btn btn-default btn-flat" data-toggle="modal" data-target="#modal-default-hosohuongxuly"> <i class="icofont-plus"></i> Thêm tập tin đính kèm</asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="card-body p-0">
                                                <div class="table-content p-0">--%>
                                                <div class="row text-center">
                                                    <div class="col-lg-9" style="margin-left: auto; margin-right: auto;">
                                                        <asp:DataGrid DataKeyField="HOSO_ID" runat="server" ID="dgDanhSach_File_HuongXuLy" AutoGenerateColumns="False" CssClass="table vertical-align-middle">
                                                            <HeaderStyle CssClass="table-header" />
                                                            <Columns>
                                                                <asp:BoundColumn HeaderText="HOSO_ID" DataField="HOSO_ID" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Tên hồ sơ" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TEN" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Nội dung tóm tắt" HeaderStyle-HorizontalAlign="Left" DataField="HOSO_TOMTAT" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="Tải về" Visible="true">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadHoSo+"/" +Eval("HOSO_FILE")%>' runat="server"><i class="icofont-download"></i></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:TemplateColumn HeaderText="Xóa" Visible="true">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a onserverclick="XoaHoSoDonThu_HuongXuLy" title="Xóa tài liệu" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa hồ sơ hướng xử lý này?');" href='<%# Eval("HOSO_ID").ToString()%>' oncontextmenu="return false" runat="server">
                                                                            <i class="icofont-ui-delete"></i>
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </div>
        </div>
        </div>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- Content Wrapper. Contains page content -->
                    <!-- /.content-wrapper -->
                    <!-- Control Sidebar -->
                    <aside class="control-sidebar control-sidebar-dark">
                        <!-- Control sidebar content goes here -->
                    </aside>
                    <!-- /.control-sidebar -->
                    </setion>
                </div>



                <script>
                    $(function () {
                        //Initialize Select2 Elements
                        //$('.select2').select2({

                        //})

                        //Initialize Select2 Elements


                        ////Datemask dd/mm/yyyy

                        ////Datemask2 mm/dd/yyyy
                        //$('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
                        ////Money Euro


                        //Date range picker

                        ////Date range picker
                        //$('#reservation').daterangepicker()
                        ////Date range picker with time picker
                        //$('#reservationtime').daterangepicker({
                        //    timePicker: true,
                        //    timePickerIncrement: 30,
                        //    locale: {
                        //        format: 'MM/DD/YYYY hh:mm A'
                        //    }
                        //})
                        //Date range as a button
                        $('#daterange-btn').daterangepicker(
                            {
                                ranges: {
                                    'Today': [moment(), moment()],
                                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                                },
                                startDate: moment().subtract(29, 'days'),
                                endDate: moment()
                            },
                            function (start, end) {
                                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                            }
                        )

                        //Timepicker
                        $('#timepicker').datetimepicker({
                            format: 'LT'
                        })

                        //Bootstrap Duallistbox
                        $('.duallistbox').bootstrapDualListbox()

                        //Colorpicker
                        $('.my-colorpicker1').colorpicker()
                        //color picker with addon
                        $('.my-colorpicker2').colorpicker()

                        $('.my-colorpicker2').on('colorpickerChange', function (event) {
                            $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
                        });

                        $("input[data-bootstrap-switch]").each(function () {
                            $(this).bootstrapSwitch('state', $(this).prop('checked'));
                        });

                    })
                </script>
                <%--</body>--%>
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
<!-- Large modal -->
<!-- Button trigger modal -->
