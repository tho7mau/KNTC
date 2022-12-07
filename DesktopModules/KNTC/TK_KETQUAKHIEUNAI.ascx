<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_KETQUAKHIEUNAI.ascx.cs" Inherits="KNTC.TK_KETQUAKHIEUNAI" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<script type="text/javascript">
    $(document).ready(function () {
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
                        <h3 class="card-title">Thống kê kết quả xử lý đơn khiếu nại</h3>
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
                                    <%--    Thêm mới kỳ báo cáo--%>
                                <br />
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label class="col-lg-4 col-form-label">Chọn kỳ báo cáo</label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="drpKyBaoCao" AutoPostBack="true" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drpKyBaoCao_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="Chọn kỳ báo cáo"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Tháng 1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Tháng 2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Tháng 3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Tháng 4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Tháng 5"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="Tháng 6"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="Tháng 7"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="Tháng 8"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="Tháng 9"></asp:ListItem>
                                                    <asp:ListItem Value="10" Text="Tháng 10"></asp:ListItem>
                                                    <asp:ListItem Value="11" Text="Tháng 11"></asp:ListItem>
                                                    <asp:ListItem Value="12" Text="Tháng 12"></asp:ListItem>
                                                     <asp:ListItem Value="q1" Text="Quý 1"></asp:ListItem>
                                                     <asp:ListItem Value="q2" Text="Quý 2"></asp:ListItem>
                                                     <asp:ListItem Value="q2" Text="Quý 3"></asp:ListItem>
                                                     <asp:ListItem Value="q4" Text="Quý 4"></asp:ListItem>
                                                     <asp:ListItem Value="t6" Text="6 Tháng"></asp:ListItem>
                                                     <asp:ListItem Value="t9" Text="9 Tháng"></asp:ListItem>                                                  
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label class="col-lg-4 col-form-label">Chọn năm</label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="drpNam" AutoPostBack="true" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drpNam_SelectedIndexChanged">                                                
                                                </asp:DropDownList>
                                                  <asp:Label runat="server" Visible="false" ID="lblKyBaoCao"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                              <%--  end kỳ báo cáo--%>
                                <br />
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label class="col-lg-4 col-form-label">Từ ngày</label>
                                            <div class="col-lg-8">
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
                                            <label class="col-lg-4 col-form-label">Đến ngày</label>
                                            <div class="col-lg-8">
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
