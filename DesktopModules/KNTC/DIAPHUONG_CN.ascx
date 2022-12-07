<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DIAPHUONG_CN.ascx.cs" Inherits="KNTC.DIAPHUONG_CN" %>
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
                                    <li class="breadcrumb-item"><a runat="server" id="linkBackToList" onserverclick="btnBoQua_Click">Quản lý đơn vị hành chính</a></li>
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
                <section class="content mr-t10">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <!-- /.card-header -->
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label class="col-lg-3 col-form-label" runat="server" id="labelTenDiaPhuong">Tên đơn vị hành chính</label>
                                                    <div class="col-lg-9">
                                                        <asp:TextBox ID="txtTenDiaPhuong" runat="server" CssClass="form-control requirement float-left" MaxLength="500" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-3 col-form-label">Đơn vị cha</label>
                                                    <div class="col-lg-9">
                                                        <asp:DropDownList ID="ddlistDIAPHUONG_CapTren" runat="server" CssClass="form-control select2bs4">
                                                        </asp:DropDownList>
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
