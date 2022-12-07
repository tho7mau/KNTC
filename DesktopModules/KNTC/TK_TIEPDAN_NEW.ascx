<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TK_TIEPDAN_NEW.ascx.cs" Inherits="KNTC.TK_TIEPDAN_NEW" %>
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

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header ">
                        <h3 class="card-title">Tổng hợp kết quả tiếp công dân</h3>
                        <asp:Panel runat="server" CssClass="card-search" Style="padding-top: 20px">
                            <div class="col-lg-12">
                                <br />
                               <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tiếp dân từ ngày</label>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tiếp dân đến ngày</label>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Loại tiếp dân</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddloaiTiepDan" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Chọn loại tiếp dân--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Có đơn" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Không đơn" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh/thành phố</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlTinhThanhPho_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quận/huyện</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlQuanHuyen_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
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
                                
                                    <asp:BoundColumn HeaderText="TIEPDAN_ID" DataField="TIEPDAN_ID" Visible="false" >
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                      <%# DataBinder.Eval(Container, "DataItem.TIEPDAN_STT") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Họ tên" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>                                                                     
                                                <%# getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>  
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Loại tiếp dân" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%#  Eval("DOITUONG.DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG.DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG.DOITUONG_TEN").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Tình trạng" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_ID" HeaderStyle-Width="8%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.DONTHU_ID")==null?"Không đơn":"Có đơn" %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# vClassCommon.GioiHanChu_Biding((DataBinder.Eval(Container, "DataItem.DONTHU_ID") ==null ?DataBinder.Eval(Container, "DataItem.TIEPDAN_NOIDUNG"):GetNoiDungDonThu(DataBinder.Eval(Container, "DataItem.DONTHU_ID").ToString())),50) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Kết quả" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_KETQUA" HeaderStyle-Width="25%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# vClassCommon.GioiHanChu_Biding((DataBinder.Eval(Container, "DataItem.DONTHU_ID") ==null?DataBinder.Eval(Container, "DataItem.TIEPDAN_KETQUA"):Get_Ykien_Xuly_DonThu(DataBinder.Eval(Container, "DataItem.DONTHU_ID").ToString())),50) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Lần tiếp" HeaderStyle-HorizontalAlign="Right" SortExpression="TIEPDAN_LANTIEP"  HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.TIEPDAN_LANTIEP") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Thời gian tiếp" HeaderStyle-HorizontalAlign="Center" SortExpression="TIEPDAN_THOGIAN" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                           <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container, "DataItem.TIEPDAN_THOGIAN")) %>
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



<%--<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header sticky">
                        <h3 class="card-title">Tổng hợp kết quả tiếp công dân</h3>
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
                        <div class="card-body p-0">
                            <div class="container-fluid">
                                <br />
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tiếp dân từ ngày</label>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tiếp dân đến ngày</label>
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
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Loại tiếp dân</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="ddloaiTiepDan" runat="server" CssClass="form-control select2bs4">
                                                    <asp:ListItem Text="--Chọn loại tiếp dân--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Có đơn" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Không đơn" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Tỉnh/thành phố</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlTinhThanhPho" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlTinhThanhPho_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group row">
                                            <label for="inputPhone" class="col-lg-3 col-form-label">Quận/huyện</label>
                                            <div class="col-lg-9">
                                                <asp:DropDownList ID="drlQuanHuyen" runat="server" CssClass="form-control select2bs4" OnSelectedIndexChanged="drlQuanHuyen_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="AutoID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
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
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 text-center">
                                        <asp:LinkButton class="btn bg-gradient-primary btn-flat tool-left" OnClick="btnXuatExel_Click" ID="btn_XuatExcel" runat="server"> <i class="icofont-download-alt"></i>  Xuất dữ liệu  </asp:LinkButton>
                                    </div>
                                </div>
                                <br />                              
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
