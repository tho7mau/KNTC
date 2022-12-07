<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnCanBo.ascx.cs" Inherits="KNTC.CnCanBo" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<%--<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/DesktopModules/HOPKHONGGIAY/Images/ajax-loader.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<asp:UpdatePanel ID="upn" runat="server">
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnCanNhat" DefaultButton="buttonCapNhat">
            <%--<body class="sidebar-mini layout-footer-fixed layout-navbar-fixed layout-fixed">--%>
            <!-- Content Wrapper. Contains page content -->
            <!-- Content Header (Page header) -->
            <div class="wrapper-form">
                <section class="content-header sticky">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <ol class="breadcrumb">
                                    <li class="breadcrumb-item"><a runat="server" id="linkBackToList" onserverclick="btnBoQua_Click">Quản lý cán bộ</a></li>
                                    <li class="breadcrumb-item active">
                                        <asp:Literal runat="server" ID="labelTen"></asp:Literal></li>
                                </ol>
                            </div>
                        </div>
                    </div>
                    <!-- /.container-fluid -->
                    <div class="form-tools">
                        <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i>  Thêm</asp:LinkButton>
                        <asp:LinkButton ID="buttonCapNhat" Visible="false" OnClick="btnCapNhat_Click" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                        <asp:LinkButton ID="buttonSua" OnClick="btnSua_Click" Visible="true" runat="server" CausesValidation="false" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                        <asp:LinkButton ID="buttonBoQua" OnClick="btnBoQua_Click" runat="server" CssClass="btn btn-md btn-default btn-flat text-primary tool-left" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>
                    </div>
                </section>

                <!-- Main content -->
                <section class="content">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <!-- /.card-header -->
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="labelUsername">Tên đăng nhập</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="textUsername" runat="server" CssClass="form-control requirement float-left" MaxLength="200" />
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" ID="panelMatKhau">
                                                    <div class="form-group row">
                                                        <label class="col-lg-3 col-form-label" runat="server" id="labelPassword">Mật khẩu</label>
                                                        <div class="col-lg-9">
                                                            <asp:TextBox ID="textMatKhau" TextMode="Password" runat="server" CssClass="form-control requirement float-left" MaxLength="200" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group row">
                                                        <label class="col-lg-3 col-form-label" runat="server" id="labelRepassword">Xác nhận mật khẩu</label>
                                                        <div class="col-lg-9">
                                                            <asp:TextBox ID="textXacNhanMatKhau" TextMode="Password" runat="server" CssClass="form-control requirement float-left" MaxLength="200" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="labelTenCanBo">Tên cán bộ</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="textTenCanBo" runat="server" CssClass="form-control requirement float-left" MaxLength="200" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="labelSoDienThoai">Số điện thoại</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="textSoDienThoai" runat="server" CssClass="form-control float-left" MaxLength="200" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="labelEmail">Email</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="textEmail" runat="server" CssClass="form-control float-left" MaxLength="200" />
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="label1">Lãnh đạo</label>
                                                    <div class="col-lg-9" style="margin-bottom: auto;margin-top: auto;">
                                                        <asp:CheckBox runat="server" ID="cboxLanhDao" CssClass="cbCustom" AutoPostBack="true" Text="" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelDonVi">Đơn vị</label>
                                                    <div class="col-lg-9">
                                                        <asp:DropDownList ID="ddlistDonVi" runat="server" AutoPostBack="true" CssClass="form-control requirement select2bs4" OnSelectedIndexChanged="ddlistDonVi_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelPhongBan">Phòng ban</label>
                                                    <div class="col-lg-9">
                                                        <asp:DropDownList ID="ddlistPhongBan" runat="server" AutoPostBack="true" CssClass="form-control requirement select2bs4">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                 <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label" runat="server" id="labelChucVu">Chức vụ</label>
                                                    <div class="col-lg-9">
                                                        <asp:DropDownList ID="ddlistChucVu" runat="server" AutoPostBack="true" CssClass="form-control requirement select2bs4">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Mô tả</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="textMoTa" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control float-left" MaxLength="500" />
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
            <!-- ./wrapper -->
            <!-- jQuery -->

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

            </script>
        </asp:Panel>
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

                        //Initialize Select2 Elements
                        $('.select2bs4').select2({
                            theme: 'bootstrap4'
                        })

                    }
                    function isNumberKey(evt) {
                        var charCode = (evt.which) ? evt.which : evt.keyCode;
                        if (charCode > 31 && (charCode < 48 || charCode > 57))
                            return false;
                        return true;
                    }
</script>
