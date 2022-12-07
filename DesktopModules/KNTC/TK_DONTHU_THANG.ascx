<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_DONTHU_THANG.ascx.cs" Inherits="KNTC.TK_DONTHU_THANG" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<script type="text/javascript">
    function pageLoad(sender, args) {
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })
        $('[data-mask]').inputmask();
        //$('.datecontrol').datetimepicker({
        //    format: 'DD/MM/YYYY'
        //});
        //$('.datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
        //var modal = $('.modal-backdrop.show');
        //for (var i = 0; i < modal.length; i++) {
        //    $('#modal-default').modal('hide');
        //    $('#modal-default').modal('show')
        //}
        //initSearch();
        //InitSticky();
        //$('.datepicker').datetimepicker({
        //    format: 'L'
        //});

    }
</script>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header sticky">
                        <h3 class="card-title">Thống kê đơn thư theo tháng</h3>
                        <asp:Panel runat="server" CssClass="card-filter">
                        </asp:Panel>
                        <div class="card-tools">
                            <div class="tool-right">
                                <div class="card-filter-advance">
                                </div>
                                <div class="card-list-type">
                                </div>


                                <div class="card-pagination">
                                </div>

                                <div class="tool-left">
                                </div>
                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body p-0">
                            <div class="container-fluid">
                                <br />

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-4 col-form-label">Chọn năm</label>
                                                    <div class="col-lg-8">
                                                        <asp:DropDownList ID="ddlistNam" runat="server" CssClass="form-control select2bs4">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-4 col-form-label">Nguồn đơn</label>
                                                    <div class="col-lg-8">
                                                        <asp:DropDownList ID="ddlistNguonDon" AutoPostBack="true" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="ddlistNguonDon_SelectedIndexChanged">
                                                            <asp:ListItem Value="-1" Text="Chọn nguồn đơn"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="Trực tiếp"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Gián tiếp"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Cơ quan khác chuyển tới"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-lg-4">

                                                <div class="form-group row">
                                                    <label for="inputPhone" class="col-lg-4 col-form-label">Đơn vị</label>
                                                    <div class="col-lg-8">
                                                        <asp:DropDownList ID="ddlistDonVi" runat="server" CssClass="form-control select2bs4">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 text-center">
                                        <asp:LinkButton class="btn bg-gradient-primary btn-flat tool-left" OnClick="btnXuatExel_Click" ID="btn_XuatExcel" runat="server"> <i class="icofont-download-alt"></i>  Xuất dữ liệu  </asp:LinkButton>
                                    </div>
                                </div>
                                <br />
                                <!-- /.row -->
                            </div>
                        </div>
                    </div>
                </div>
        </section>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_XuatExcel" />
    </Triggers>
</asp:UpdatePanel>
