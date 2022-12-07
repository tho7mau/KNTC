<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_DONTHUTHEODONVICHUYEN.ascx.cs" Inherits="KNTC.TK_DONTHUTHEODONVICHUYEN" %>
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
                        <h3 class="card-title">Thống kê đơn thư theo đơn vị chuyển</h3>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Nhận đơn từ ngày</label>
                                            <div class="col-lg-9">
                                                <div class='input-group datecontrol date reservationdateNhanDonTuNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textNhanDonTuNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateNhanDonTuNgay' data-toggle="datetimepicker">
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
                                                <div class='input-group datecontrol date reservationdateNhanDonDenNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textNhanDonDenNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateNhanDonDenNgay' data-toggle="datetimepicker">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Chuyển đơn từ ngày</label>
                                            <div class="col-lg-9">
                                                <div class='input-group datecontrol date reservationdateDonChuyenTuNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textDonChuyenTuNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateDonChuyenTuNgay' data-toggle="datetimepicker">
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
                                                <div class='input-group datecontrol date reservationdateonChuyenDenNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textDonChuyenDenNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateonChuyenDenNgay' data-toggle="datetimepicker">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Hạn giải quyết từ ngày</label>
                                            <div class="col-lg-9">
                                                <div class='input-group datecontrol date reservationdateHanGiaiQuyetTuNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textHanGiaiQuyetTuNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateHanGiaiQuyetTuNgay' data-toggle="datetimepicker">
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
                                                <div class='input-group datecontrol date reservationdateHanGiaiQuyetDenNgay' data-target-input="nearest">
                                                    <asp:TextBox Style="width: 50% !important" ID="textHanGiaiQuyetDenNgay" runat="server" CssClass="form-control datetimepicker-input " placeholder="" />
                                                    <div class="input-group-append" data-target='.reservationdateHanGiaiQuyetDenNgay' data-toggle="datetimepicker">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Cơ quan tiếp nhận</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistCoQuanTiepNhan"  runat="server" CssClass="form-control select2bs4"  >
                                                 </asp:DropDownList> <%-- multiple--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Trạng thái giải quyết</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistTrangThaiGiaiQuyet" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Value="" Text="Tất cả trạng thái"></asp:ListItem>
                                                    <asp:ListItem Value="True" Text="Đã giải quyết"></asp:ListItem>
                                                    <asp:ListItem Value="False" Text="Chưa giải quyết"></asp:ListItem>
                                                </asp:DropDownList>
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
