<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DsTiepDan.ascx.cs" Inherits="KNTC.DsTiepDan" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>--%>

<script type="text/javascript" src="<%=vPathCommonJS %>"></script>
<dnn:DnnJsInclude ID="Chosen" runat="server" FilePath="/DesktopModules/KNTC/Scripts/search.js" AddTag="false" />

<script type="text/javascript">

    function pageLoad(sender, args) {
        initSearch();
        InitSticky();
        $('.datepicker').datetimepicker({
            format: 'L',
            locale: 'vn'
        });
    }
</script>

<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
    </ProgressTemplate>
</asp:UpdateProgress>
<div class="float-right OptionSearch OptionSearchSave" style="display: none;"></div>

<asp:UpdatePanel runat="server" ID="updatePN">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header sticky">
                        <h3 class="card-title">Danh sách tiếp dân</h3>
                        <asp:Panel runat="server" DefaultButton="buttonSearch" CssClass="card-filter">
                            <%--<div class="card-filter">--%>
                            <!-- Text search -->
                            <div class="input-group input-group tool-right">
                                <asp:Literal runat="server" ID="Literal_OptionSearch">
                            <div class="float-right OptionSearch OptionSearchDisplay"  data-save="false"></div>
                                </asp:Literal>
                                <asp:TextBox runat="server" ID="textSearchContent_HiddenField" CssClass="form-control float-right textSearchContent_HiddenField" placeholder="Nhập từ khóa tìm kiếm" Style="min-width: 100px;"></asp:TextBox><%----%>
                                <asp:TextBox runat="server" ID="textSearchContent" CssClass="form-control float-right" placeholder="Nhập từ khóa tìm kiếm" Style="min-width: 100px;"></asp:TextBox>

                                <div class="input-group-append d-none filter-toggler">
                                    <button class="navbar-toggler btn btn-default btn-flat" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                                        <i class="icofont-filter"></i>
                                    </button>
                                </div>
                                <div class="input-group-append ">

                                    <asp:LinkButton ID="buttonSearch" runat="server" CssClass="btn btn-default btn-flat" OnClick="btnSearch_Click"> <i class="fas fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                            <%--</div>--%>
                        </asp:Panel>
                        <div class="card-tools">
                            <div class="tool-right">
                                <div class="card-filter-advance">
                                    <nav class="navbar navbar-expand-lg ">
                                        <div class="collapse navbar-collapse" id="navbarNavDropdown">
                                            <ul class="navbar-nav">
                                                <li class="nav-item dropdown dropdown-filter">
                                                    <button type="button" class="btn btn-tool dropdown-toggle" aria-expanded="false" data-toggle="dropdown">
                                                        <i class="icofont-filter"></i>Bộ lọc
                                                    </button>
                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_STT" data-type="normal" data-title="Số thứ tự tiếp dân"><i class="icofont-check"></i>Số thứ tự tiếp dân</a>
                                                        <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_HOTEN" data-type="normal"><i class="icofont-check"></i>Họ tên</a>
                                                        <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_DIACHI_DAYDU" data-type="normal"><i class="icofont-check"></i>Địa chỉ</a>
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_NOIDUNG" data-type="normal"><i class="icofont-check"></i>Nội dung</a>
                                                        <div class="dropdown-divider"></div>
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.TIEPDAN_THOGIAN" data-type="datetime"><i class="icofont-check"></i>Thời gian</a>
                                                        <div class="datetime-content" data-option="TIEPDAN.TIEPDAN_THOGIAN"></div>
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.NGAYTAO" data-type="datetime"><i class="icofont-check"></i>Ngày tạo</a>
                                                        <div class="datetime-content" data-option="TIEPDAN.NGAYTAO"></div>
                                                    </div>
                                                </li>

                                                <li class="nav-item dropdown dropdown-filter">
                                                    <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                                        <i class="icofont-checked"></i>Trạng thái
                                                    </button>
                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.DONTHU_ID " data-type="equal" data-value="is not null"><i class="icofont-check"></i>Có đơn</a>
                                                        <a href="#" class="dropdown-item" data-option="TIEPDAN.DONTHU_ID" data-type="equal" data-value="is null"><i class="icofont-check"></i>Không đơn</a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </nav>
                                </div>
                                <div class="card-list-type">                                 
                                </div>


                                <div class="card-pagination">
                                    <div class="btn-group tool-right pagination-group">
                                        <span class="pagination-title">
                                            <asp:TextBox runat="server" ID="txtRecordStartEnd" AutoPostBack="true" OnTextChanged="txtRecordStartEnd_TextChanged" CssClass="form-control float-left text-right form-control-sm" Width="50" placeholder=""></asp:TextBox>
                                            /
                                            <asp:Label runat="server" ID="lblTotalRecords" Text="" />
                                        </span>

                                        <asp:LinkButton ID="LinkButtonPrevious" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonPrevious_Click">&lt;</asp:LinkButton>
                                        <asp:LinkButton ID="LinkButtonLast" runat="server" CssClass="btn btn-default btn-sm btn-flat" OnClick="LinkButtonLast_Click">&gt;</asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <!--<div class="tool-right">
                                </div>-->
                            <div class="tool-left">
                                <asp:LinkButton ID="btn_ThemMoi" runat="server" OnClick="btnThemMoi_Click" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i> THÊM MỚI</asp:LinkButton>
                                <%--<asp:LinkButton ID="btn_Nhap" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-default btn-flat tool-left text-primary"><i class="icofont-upload"></i> NHẬP</asp:LinkButton>--%>
                                <asp:LinkButton ID="btn_SoTiepDan" runat="server" OnClick="btn_SoTiepDan_Click" CssClass="btn btn-default btn-flat tool-left text-primary"><i class="icofont-file-excel"></i> SỔ TIẾP DÂN </asp:LinkButton>
                                <%--<asp:LinkButton ID="btn_Xoa" OnClientClick="return getConfirmation(this, 'THÔNG BÁO','Bạn chắc chắn muốn xóa dữ liệu đã chọn?');" CausesValidation="false" runat="server" CssClass="btn btn-default btn-flat tool-left text-primary " OnClick="btn_Xoa_Click" Style="display: none;"><i class=""></i> Xóa</asp:LinkButton>--%>
                                <div id="divShowBtnXoa" style="display: none;" >
                                    <button type="button" class="btn btn-default btn-flat tool-left text-primary  btn-warning-delete " onclick="confirm_delete_rows('<%=btn_Xoa.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                                </div>
                                <div style="display: none;">
                                    <asp:LinkButton ID="btn_Xoa" CausesValidation="false" Visible="true" runat="server" CssClass="btn btn-default btn-flat tool-left text-primary  btn min-width-100 mr-t3 mr-b3" OnClick="btn_Xoa_Click"><i class=""></i> Xóa</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body p-0">
                        <!--style="height: 500px;"-->
                        <div class="table-content p-0">
                            <asp:DataGrid DataKeyField="TIEPDAN_ID" runat="server" ID="dgDanhSach" AutoGenerateColumns="False" BorderWidth="0"
                                AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound" CssClass="table vertical-align-middle">
                                <%--text-nowrap--%>
                                <HeaderStyle CssClass="table-header" />
                                <Columns>
                                    <asp:TemplateColumn Visible="true" HeaderStyle-CssClass="sticky">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick="handle_checked_delete_all_rows(this,'divShowBtnXoa');" />
                                        </HeaderTemplate>
                                        <HeaderStyle Width="3%" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Width="3%" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" onclick="handle_checked_delete_row(this,'divShowBtnXoa');" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="TIEPDAN_ID" DataField="TIEPDAN_ID" Visible="false" HeaderStyle-CssClass="sticky">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" SortExpression="TIEPDAN_STT" HeaderStyle-CssClass="sticky" HeaderStyle-Width="4%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <a onserverclick="dgDanhSach_Sua" title="Cập nhật thông tin tiếp dân" href='<%# Eval("TIEPDAN_ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.TIEPDAN_STT") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Họ tên" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="sticky" ItemStyle-Width="20%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>

                                            <a onserverclick="dgDanhSach_Sua" title="Cập nhật thông tin tiếp dân" href='<%# Eval("TIEPDAN_ID").ToString() %>' oncontextmenu="return false" runat="server">                                        
                                                <%# getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>  </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Loại tiếp dân" HeaderStyle-HorizontalAlign="Left" SortExpression="DOITUONG_LOAI" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%#  Eval("DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG_TEN").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Tình trạng" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_ID" HeaderStyle-CssClass="sticky" HeaderStyle-Width="8%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.DONTHU_ID").ToString()==""?"Không đơn":"Có đơn" %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_NOIDUNG" HeaderStyle-CssClass="sticky" HeaderStyle-Width="25%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# vClassCommon.GioiHanChu_Biding((DataBinder.Eval(Container, "DataItem.DONTHU_ID").ToString()==""?DataBinder.Eval(Container, "DataItem.TIEPDAN_NOIDUNG"):DataBinder.Eval(Container, "DataItem.DONTHU_NOIDUNG")),50) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Kết quả" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEPDAN_KETQUA" HeaderStyle-CssClass="sticky" HeaderStyle-Width="25%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# vClassCommon.GioiHanChu_Biding((DataBinder.Eval(Container, "DataItem.DONTHU_ID").ToString()==""?DataBinder.Eval(Container, "DataItem.TIEPDAN_KETQUA"):DataBinder.Eval(Container, "DataItem.HUONGXULY_YKIEN_XULY")),50) %>
                                            <%--<%# vClassCommon.GioiHanChu_Biding(DataBinder.Eval(Container, "DataItem.TIEPDAN_KETQUA"),50) %>--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Lần tiếp" HeaderStyle-HorizontalAlign="Right" SortExpression="TIEPDAN_LANTIEP" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.TIEPDAN_LANTIEP") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Thời gian tiếp" HeaderStyle-HorizontalAlign="Center" SortExpression="TIEPDAN_THOGIAN" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
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
                <!-- /.container-fluid -->
            </div>
        </section>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_SoTiepDan" />
    </Triggers>
</asp:UpdatePanel>

<style>
    .form .form-inline .col-left {
        float: left;
    }

    .danhsach table tbody tr.paping td select {
        float: right;
    }
</style>
<script type="text/javascript">
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }
    function ConfirmDelete() {
        return confirm("Bạn muốn xóa dữ liệu này ?");
    }
    function ConfirmActive() {
        return confirm("Bạn muốn thay đổi trạng thái quốc gia?");
    }
    function ConfirmDelete() {
        return confirm("Bạn muốn xóa dữ liệu này ?");
    }     
</script>
