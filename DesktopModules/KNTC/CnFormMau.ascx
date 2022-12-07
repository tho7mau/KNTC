<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnFormMau.ascx.cs" Inherits="KNTC.CnFormMau" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<script type="text/javascript" src="<%=vPathCommonJS%>"></script>--%>
<%--<%=vJavascriptMask %>--%>
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
        <%--<body class="sidebar-mini layout-footer-fixed layout-navbar-fixed layout-fixed">--%>
        <!-- Content Wrapper. Contains page content -->
        <!-- Content Header (Page header) -->
        <section class="content-header sticky">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="/pages/liink/simple.html">Quản lý loại đơn thư</a></li>
                            <li class="breadcrumb-item active">Khiếu nại tố cáo </li>
                        </ol>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
            <div class="form-tools">
                <asp:LinkButton ID="buttonThemmoi" runat="server" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Thêm</asp:LinkButton>
                <asp:LinkButton ID="btnCapNhat" Visible="false" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                <asp:LinkButton ID="btnSua" Visible="true" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-md btn-default btn-flat text-primary tool-left" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>
            </div>
        </section>

        <!-- Main content -->
        <section class="content">

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
                            <form class="form-horizontal">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <div class="form-group row">
                                                <label class="col-lg-3 col-form-label" runat="server" id="labelMaLoaiDonThu">Mã loại đơn thư</label>
                                                <div class="col-lg-9">
                                                    <asp:TextBox ID="textMaLoaiDonThu" runat="server" CssClass="form-control requirement float-left" MaxLength="500" />
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-lg-3 col-form-label" runat="server" id="labelTenLoaiDonThu">Tên loại đơn thư</label>
                                                <div class="col-lg-9">
                                                    <asp:TextBox ID="textTenLoaiDonThu" runat="server" CssClass="form-control requirement float-left" MaxLength="500" />
                                                </div>
                                            </div>

                                             <div class="form-group row">
                                                <label for="inputHoTen" class="col-lg-3 col-form-label">Ngày đặt hàng</label>
                                                <div class="col-lg-9">
                                                    <div class="input-group date" id="reservationdate" data-target-input="nearest">
                                                        <input type="text" class="form-control datetimepicker-input" value="08/20/2020" data-target="#reservationdate" />
                                                        <div class="input-group-append" data-target="#reservationdate" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                        </div>                                                        
                                                        <telerik:RadDatePicker runat="server" ID="dateDemo"></telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="form-group row">
                                                <label for="inputPhone" class="col-lg-3 col-form-label">Loại cấp trên</label>
                                                <div class="col-lg-9">
                                                    <asp:DropDownList ID="ddlistLoaiDonThu_CapTren" runat="server" CssClass="form-control select2bs4">
                                                        <asp:ListItem Text="Đơn thu" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="Khiếu nại" Value="B"></asp:ListItem>
                                                        <asp:ListItem Text="Tố cáo" Value="C"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label for="inputPhone" class="col-lg-3 col-form-label">Mô tả</label>
                                                <div class="col-lg-9">
                                                    <asp:TextBox ID="textMoTaLoaiDonThu" runat="server" CssClass="form-control float-left" MaxLength="500" />
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label for="inputPhone" class="col-lg-3 col-form-label"></label>
                                                <div class="col-lg-9">
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
  <!-- ./wrapper -->
            <!-- jQuery -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/jquery/jquery.min.js"></script>
            <!-- Bootstrap 4 -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
            <!-- Select2 -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/select2/js/select2.full.min.js"></script>
            <!-- Bootstrap4 Duallistbox -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
            <!-- InputMask -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/moment/moment.min.js"></script>
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
            <!-- date-range-picker -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/daterangepicker/daterangepicker.js"></script>
            <!-- overlayScrollbars -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
            <!-- bootstrap color picker -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
            <!-- Tempusdominus Bootstrap 4 -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
            <!-- Bootstrap Switch -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
            <!-- bs-custom-file-input -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/plugins/bs-custom-file-input/bs-custom-file-input.min.js"></script>
            <!-- AdminLTE App -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/dist/js/adminlte.min.js"></script>
            <!-- AdminLTE for demo purposes -->
            <script src="../Portals/_default/Skins/ADMINLTE/lib/AdminLTE/dist/js/demo.js"></script>
            <script type="text/javascript">
                $(document).ready(function () {
                    //bsCustomFileInput.init();                   
                });
            </script>
            <script>
                $(function () {
                    //Initialize Select2 Elements
                    $('.select2bs4').select2({
                        theme: 'bootstrap4'
                    })

                    //Datemask dd/mm/yyyy
                    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
                    //Datemask2 mm/dd/yyyy
                    $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
                    //Money Euro
                    $('[data-mask]').inputmask()

                    //Date range picker
                    $('#reservationdate').datetimepicker({
                        format: 'L'
                    });
                    //Date range picker
                    $('#reservation').daterangepicker()
                    //Date range picker with time picker
                    $('#reservationtime').daterangepicker({
                        timePicker: true,
                        timePickerIncrement: 30,
                        locale: {
                            format: 'MM/DD/YYYY hh:mm A'
                        }
                    })
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
    function pageLoad(sender, args) {
        $('.select2').select2({

        })

        //Initialize Select2 Elements
        $('.select2bs4').select2({
            //theme: 'bootstrap4'
        })

    }
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
