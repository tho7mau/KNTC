<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnDoiTuong.ascx.cs" Inherits="KNTC.CnDoiTuong" %>
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
                                    <asp:LinkButton ID="btnBreadcrumbBack" runat="server" OnClick="btnBreadcrumbBack_Click">Quản lý đối tượng</asp:LinkButton>
                                    </a></li>
                                <li class="breadcrumb-item active">
                                    <asp:Label ID="lblBreadcrumbTitle" runat="server"></asp:Label></li>
                            </ol>
                        </div>
                    </div>
                </div>
                <!-- /.container-fluid -->
                <div class="form-tools">
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" OnClick="btnCapNhat_Click" Visible="false" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="true" OnClick="btnSua_Click" CausesValidation="true" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-md btn-default btn-flat text-primary tool-left" CausesValidation="false" OnClick="btnBoQua_Click"><i class='icofont-undo' ></i> Trở về</asp:LinkButton>
                </div>
            </section>

            <!-- Main content -->
            <section class="content mr-t10">

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header ">
                                    <div class="card-tools">
                                        <asp:LinkButton  CssClass="btn btn-app" runat="server" ID="TiepDanCardTool" OnClick="TiepDanCardTool_Click">
                                            <asp:Label runat="server" ID="lableSoLuongTiepDan"  CssClass="badge bg-primary"></asp:Label>
                                            <i class="icon icofont-users-alt-3"></i>Tiếp dân
                                        </asp:LinkButton>
                                        <asp:LinkButton CssClass="btn btn-app" runat="server" ID="DonThuCardTool" OnClick="DonThuCardTool_Click">
                                            <asp:Label runat="server" ID="lableSoLuongDonThu" CssClass="badge bg-danger"></asp:Label>
                                            <i class="icon icofont-file-document"></i>Đơn thư
                                        </asp:LinkButton>
                                        <%--<a class="btn btn-app">
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
                                        </a>--%>
                                        <button type="button" class="btn btn-tool d-none d-lg-inline-flex  d-xl-inline-flex" data-card-widget="maximize">
                                            <i class="fas fa-expand"></i>
                                        </button>
                                        <%--<div class="btn-group  d-none d-lg-inline-flex d-xl-inline-flex">
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
                                        </div>--%>
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
                                                <h4>Thông tin đối tượng</h4>
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
                                                        <asp:LinkButton ID="btnChonNguoiDaiDien" Visible="false" runat="server" CssClass="btn btn-sm  btn-flat text-primary float-left" data-toggle="modal" data-target="#modal-default"><i class="icofont-maximize h4"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6" runat="server" id="panelSoNguoi">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Số người</label>
                                                    <div class="col-lg-3">
                                                        <asp:TextBox type="text" class="form-control requirement" ID="txtSoNguoi" data-mask runat="server" placeholder="" data-inputmask="'mask': '[9]'" inputmode="numeric"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-lg-6" runat="server" id="panelSoNguoiDaiDien">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Số người đại diện</label>
                                                    <div class="col-lg-3">
                                                        <asp:TextBox type="text" class="form-control requirement" ID="txtSoNguoiDaiDien" data-mask runat="server" placeholder="" data-inputmask="'mask': '[9]'" inputmode="numeric" AutoPostBack="true" OnTextChanged="txtSoNguoiDaiDien_TextChanged" Text="1"></asp:TextBox>
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
                                                            <label for="inputPhone" class="col-lg-3 col-form-label">CMTND/ Thẻ căn cước</label>
                                                            <div class="col-lg-9">
                                                                <asp:TextBox ID="txtCMND" runat="server" CssClass="form-control " MaxLength="500" Text='<%# Eval("CANHAN_CMDN")!=null?Eval("CANHAN_CMDN").ToString():""  %>' />

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
