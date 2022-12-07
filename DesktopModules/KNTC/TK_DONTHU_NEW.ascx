<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_DONTHU_NEW.ascx.cs" Inherits="KNTC.TK_DONTHU_NEW" %>
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
    }
</script>

<%--<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header sticky">
                        <h3 class="card-title">Thống kê đơn thư</h3>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Nguồn đơn</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistNguon" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Chọn nguồn đơn--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Trực tiếp" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Bưu chính" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Cơ quan khác chuyển tới" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Hộp thư góp ý" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Loại đơn</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlLoaiDon" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlLoaiDon_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Chọn loại đơn--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Khiếu nại" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Tố cáo" Value="2004"></asp:ListItem>
                                                    <asp:ListItem Text="Kiến nghị, phản ánh" Value="4035"></asp:ListItem>
                                                    <asp:ListItem Text="Đơn có nhiều nội dung" Value="4051"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Đối tượng</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistDoiTuong" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Tất cả đối tượng--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Cá nhân" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Nhóm đông người" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Cơ quan tổ chức" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh/thành phố</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlTinhThanhPho_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
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
                                                <asp:DropDownList ID="drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlQuanHuyen_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Hướng xử lý</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistHuongXuLy" runat="server" CssClass="form-control select2bs4"  ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tình trạng</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistTrangThaiGiaiQuyet" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Value="-1" Text="Tất cả tình trạng"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Đã có kết quả giải quyết"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Chưa có kết quả giải quyết"></asp:ListItem>
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
</asp:UpdatePanel>--%>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header ">
                        <h3 class="card-title">Thống kê đơn thư</h3>
                        <asp:Panel runat="server" CssClass="card-search" Style="padding-top: 20px">
                            <div class="col-lg-12">
                                <br />
                                <div class="row" style="width: 100%">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Nguồn đơn</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistNguon" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Chọn nguồn đơn--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Trực tiếp" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Bưu chính" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Cơ quan khác chuyển tới" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Hộp thư góp ý" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Loại đơn</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlLoaiDon" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlLoaiDon_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Chọn loại đơn--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Khiếu nại" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Tố cáo" Value="2004"></asp:ListItem>
                                                    <asp:ListItem Text="Kiến nghị, phản ánh" Value="4035"></asp:ListItem>
                                                    <asp:ListItem Text="Đơn có nhiều nội dung" Value="4051"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Đối tượng</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistDoiTuong" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Tất cả đối tượng--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Cá nhân" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Nhóm đông người" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Cơ quan tổ chức" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh/thành phố</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlTinhThanhPho_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
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
                                                <asp:DropDownList ID="drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlQuanHuyen_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Hướng xử lý</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistHuongXuLy" runat="server" CssClass="form-control select2bs4" ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tình trạng</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddlistTrangThaiGiaiQuyet" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Value="-1" Text="Tất cả tình trạng"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Đã có kết quả giải quyết"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Chưa có kết quả giải quyết"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 text-center">
                                        <asp:LinkButton class="btn bg-gradient-primary btn-flat tool-left" OnClick="btnSearch_Click" ID="btnSearch" runat="server"> <i class="icofont-search"></i>  Tìm kiếm  </asp:LinkButton>
                                        <asp:LinkButton class="btn bg-gradient-primary btn-flat tool-left" OnClick="btnXuatExel_Click" ID="btn_XuatExcel" runat="server"> <i class="icofont-download-alt"></i>  Xuất dữ liệu  </asp:LinkButton>

                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-tools">
                            <div class="tool-right">
                                <div class="card-filter-advance">
                                </div>
                                <div class="card-list-type">
                                </div>
                                <div class="card-pagination">
                                    <div class="btn-group tool-right pagination-group">
                                        <span class="pagination-title">
                                            <asp:TextBox runat="server" ID="txtRecordStartEnd" AutoPostBack="true" OnTextChanged="txtRecordStartEnd_TextChanged" CssClass="form-control float-left text-right" Width="50" placeholder=""></asp:TextBox>
                                            /
                                            <asp:Label runat="server" ID="lblTotalRecords" Text="" />
                                        </span>
                                        <asp:LinkButton ID="LinkButtonPrevious" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonPrevious_Click">&lt;</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonLast" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonLast_Click">&gt;</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="tool-left">
                                </div>
                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body p-0">
                            <div class="container-fluid">

                                <!-- /.row -->
                                <!-- /.card-header -->
                                <div class="card-body p-0">
                                    <!--style="height: 500px;"-->
                                    <div class="table-content p-0">
                                        <asp:DataGrid runat="server" ID="dgDanhSach" AutoGenerateColumns="False" BorderWidth="0" AutoPostback="True"
                                            OnItemDataBound="dgDanhSach_ItemDataBound" CssClass="table vertical-align-middle">
                                            <%--text-nowrap--%>
                                            <HeaderStyle CssClass="table-header" />
                                            <Columns>
                                                <asp:BoundColumn HeaderText="DONTHU_ID" DataField="DONTHU_ID" Visible="false">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" SortExpression="DONTHU_STT">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.DONTHU_STT") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Tên chủ đơn" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>


                                                        <%# getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Nguồn đơn" HeaderStyle-HorizontalAlign="Left" SortExpression="NGUONDON_LOAI_CHITIET">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%# Eval("NGUONDON_LOAI_CHITIET").ToString() == "0"?"Trực tiếp":Eval("NGUONDON_LOAI_CHITIET").ToString() == "1"?"Bưu chính":Eval("NGUONDON_LOAI_CHITIET").ToString() == "3"?"Họp thư góp ý":GetCoQuanChuyenDon(Eval("NGUONDON_DONVI_ID").ToString()) %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Loại đơn" HeaderStyle-HorizontalAlign="Left" SortExpression="LOAIDONTHU_ID">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#GetTenLoaiDonThuById(DataBinder.Eval(Container, "DataItem.LOAIDONTHU_CHA_ID").ToString()) %>
                                                        <%--<%#  Eval("DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG_TEN").ToString()) %>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Nội dung vụ việc" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_NOIDUNG">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.DONTHU_NOIDUNG") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn HeaderText="Tình trạng" HeaderStyle-HorizontalAlign="Left" SortExpression="TinhTrang">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%# Eval("TRANGTHAI_DONTHUKHONGDUDIEUKIEN") !=null && Eval("TRANGTHAI_DONTHUKHONGDUDIEUKIEN").ToString() =="True" ?"Lưu đơn":
                                                    Eval("HUONGXULY_ID")== null?"Chưa xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="0"?"Chưa xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="1"?"Đang xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="2"?"Đơn thư kết thúc":
                                                    Eval("HUONGXULY_ID").ToString() =="3"?"Đang xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="4"?"Đang xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="5"?"Đang xử lý":
                                                    Eval("HUONGXULY_ID").ToString() =="6"?"Đơn thư kết thúc":
                                                    Eval("HUONGXULY_ID").ToString() =="7"?"Đơn thư kết thúc":
                                                    Eval("HUONGXULY_ID").ToString() =="8"?"Đơn thư kết thúc":
                                                    Eval("HUONGXULY_ID").ToString() =="9"?"Đơn thư kết thúc":
                                                    Eval("HUONGXULY_ID").ToString() =="10"?"Đang xử lý":"Chưa xử lý" %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Hướng/Nội dung xử lý" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_NOIDUNG">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%#Eval("TRANGTHAI_DONTHUKHONGDUDIEUKIEN") !=null && Eval("TRANGTHAI_DONTHUKHONGDUDIEUKIEN").ToString() =="True"?"": DataBinder.Eval(Container, "DataItem.HUONGXULY_ID") != null ? 
                                                    ("<b>" +   Eval("HUONGXULY_TEN").ToString() + "</b> <br/>" +  Eval("HUONGXULY_YKIEN_XULY").ToString())
                                                    :
                                                    ""%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Ngày đề đơn" HeaderStyle-HorizontalAlign="Center" SortExpression="NGUONDON_NGAYDEDON" HeaderStyle-Width="10%">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.NGUONDON_NGAYDEDON")) %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Ngày tiếp nhận" HeaderStyle-HorizontalAlign="Center" SortExpression="NGAYTAO" HeaderStyle-Width="10%">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.NGAYTAO")) %>
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
                        </div>
                    </div>
                </div>
        </section>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_XuatExcel" />
    </Triggers>
</asp:UpdatePanel>
