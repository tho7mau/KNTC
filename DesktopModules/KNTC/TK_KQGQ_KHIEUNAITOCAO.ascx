<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_KQGQ_KHIEUNAITOCAO.ascx.cs" Inherits="KNTC.TK_KQGQ_KHIEUNAITOCAO" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

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


</script>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header sticky">
                        <h3 class="card-title">Tổng hợp kết quả xử lý đơn khiếu nại tố cáo</h3>
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
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Từ ngày</label>
                                            <div class="col-lg-9">
                                                <div class='input-group datecontrol date reservationdateTuNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="date_tu" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateTuNgay' data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Đến ngày</label>
                                            <div class="col-lg-9">
                                                <div class='input-group datecontrol date reservationdateDenNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="date_den" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateDenNgay' data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="icofont-ui-calendar h5 mb-0"></i></div>
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

