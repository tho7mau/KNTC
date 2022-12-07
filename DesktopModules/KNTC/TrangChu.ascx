
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TrangChu.ascx.cs" Inherits="KNTC.TrangChu" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>--%>
<script type="text/javascript" src="<%=vPathCommonJS %>"></script>
<dnn:DnnJsInclude ID="Chosen" runat="server" FilePath="/DesktopModules/KNTC/Scripts/search.js" AddTag="false" />
<dnn:DnnJsInclude ID="Trangchujs" runat="server" FilePath="/DesktopModules/KNTC/Scripts/trangchu.js" AddTag="false" />

<script type="text/javascript">

    function pageLoad(sender, args) {
        initSearch();
        /*Init();*/
        $('.datepicker').datetimepicker({
            format: 'L'
        });
    }
</script>

<style>
    .info-box .info-box-content .info-box-text h6{
        margin-top:2px;
    }
    </style>

<!-- Main content -->
<div class="content">
    <div class="container-fluid">
        <div class="row">
            <!-- /.col-md-9 -->
            <div class="col-lg-9 daskboard_flex">
                <div class="card mr-t15">
                    <div class="card-header border-0">
                        <div class="d-flex justify-content-between">
                            <h3 class="card-title">Tiếp dân</h3>
                           <%-- <a href="javascript:void(0);">View Report</a>--%>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="position-relative mb-4">
                            <canvas id="tiepdan-chart" height="400"></canvas>
                        </div>

                        <div class="d-flex flex-row justify-content-end">
                            <span class="mr-10">
                                <i class="fas fa-square text-primary"></i> Có đơn
                            </span>

                            <span class="mr-10">
                                <i class="fas fa-square text-gray"></i> Không đơn
                            </span>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-lg-3 daskboard_flex">
                <div class="card mr-t15">
                    <div class="card-header  border-0">
                        <h3 class="card-title">Loại đối tượng tiếp dân</h3>

                        <div class="card-tools">
                            <%-- <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i>
                  </button>--%>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body pd-10">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="chart-responsive">
                                    <canvas id="pieChart" height="220"></canvas>
                                </div>
                                <!-- ./chart-responsive -->
                            </div>
                            <!-- /.col -->
                            <div class="col-md-12 pd-0">
                                <ul class="chart-legend chart-legend-horizontal clearfix">
                                    <li><i class="icofont-user-alt-7 text-primary icofont-2x"></i> <span class="chart-legend-title" >Cá nhân </span><span class="total-td-cn-per char-per text-primary "></span> <span class="total-td-cn char-num"></span> </li>
                                    <li><i class="icofont-bank-alt text-danger icofont-2x"></i><span class="chart-legend-title" >Cơ quan tổ chức</span> <span class="total-td-cqtc-per char-per text-danger"></span><span class="total-td-cqtc char-num"></span></li>
                                    <li><i class="icofont-users-alt-3 text-warning icofont-2x"></i><span class="chart-legend-title" >Đoàn đông người </span><span class="total-td-ddn-per char-per text-danger"></span><span class="total-td-ddn char-num"></span></li>
                                </ul>
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.row -->
                    </div>
                  
                </div>
            </div>
            <!-- /.col-md-3 -->
        </div>
        <div class="row LoaiTiepDan_Content">
          
        </div>
        <div class="row">
            <!-- /.col-md-9 -->
            <div class="col-lg-9 daskboard_flex">
                <div class="card ChartDonThu_content">
                    <div class="card-header border-0">
                        <div class="d-flex justify-content-between">
                            <h3 class="card-title">Đơn thư</h3>
                           <%-- <a href="javascript:void(0);">View Report</a>--%>
                        </div>
                    </div>
                    <div class="card-body">
                        <%--<div class="d-flex">
                            <p class="d-flex flex-column">
                                <span class="text-bold text-lg">$18,230.00</span>
                                <span>Sales Over Time</span>
                            </p>
                            <p class="ml-auto d-flex flex-column text-right">
                                <span class="text-success">
                                    <i class="fas fa-arrow-up"></i>33.1%
                                </span>
                                <span class="text-muted">Since last month</span>
                            </p>
                        </div>--%>
                        <!-- /.d-flex -->

                        <div class="position-relative mb-4">
                            <canvas id="donthu-chart" height="400"></canvas>
                        </div>

                        <div class="d-flex flex-row justify-content-end ChartDonThu_title">
                            <%--<span class="mr-2">
                                <i class="fas fa-square text-primary"></i>Có đơn
                            </span>

                            <span>
                                <i class="fas fa-square text-gray"></i>Không đơn
                            </span>--%>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-lg-3 daskboard_flex">
                <div class="card ">
                    <div class="card-header  border-0">
                        <h3 class="card-title">Loại đối tượng đơn thư</h3>

                        <div class="card-tools">
                            <%-- <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i>
                  </button>--%>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body pd-10">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="chart-responsive">
                                    <canvas id="pieChartDonThu" height="220"></canvas>
                                </div>
                                <!-- ./chart-responsive -->
                            </div>
                            <!-- /.col -->
                            <div class="col-md-12 pd-0">
                                <ul class="chart-legend chart-legend-horizontal clearfix">
                                    <li><i class="icofont-user-alt-7 text-primary icofont-2x"></i><span class="chart-legend-title" >Cá nhân</span> <span class="total-dt-cn-per char-per text-primary "></span> <span class="total-dt-cn char-num"></span> </li>
                                    <li><i class="icofont-bank-alt text-danger icofont-2x"></i><span class="chart-legend-title" >Cơ quan tổ chức</span> <span class="total-dt-cqtc-per char-per text-danger"></span><span class="total-dt-cqtc char-num"></span></li>
                                    <li><i class="icofont-users-alt-3 text-warning icofont-2x"></i><span class="chart-legend-title" >Đoàn đông người</span> <span class="total-dt-ddn-per char-per text-danger"></span><span class="total-dt-ddn char-num"></span></li>
                                </ul>
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.row -->
                    </div>

                </div>
            </div>
            <!-- /.col-md-3 -->
        </div>
         <div class="row LoaiDonThu_Content">

        </div>
        <!-- /.row -->
    </div>
    <!-- /.container-fluid -->
</div>
<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
    </ProgressTemplate>
</asp:UpdateProgress>
<div class="float-right OptionSearch OptionSearchSave" style="display: none;"></div>

<asp:UpdatePanel runat="server" ID="updatePN" Visible="true">
    <ContentTemplate>
        <section class="content ">
            <div class="container-fluid">
                <div class="card card-table-master">
                    <div class="card-header ">
                        <h3 class="card-title">Danh sách đơn thư mới</h3>
                        <asp:Panel runat="server" DefaultButton="buttonSearch" CssClass="card-filter">
                            <%--<div class="card-filter">--%>
                            <!-- Text search -->
                            <div class="input-group input-group tool-right">
                                <asp:Literal runat="server" ID="Literal_OptionSearch">
                            <div class="float-right OptionSearch OptionSearchDisplay"  data-save="false"></div>
                                </asp:Literal>
                                <asp:TextBox runat="server" ID="textSearchContent_HiddenField" CssClass="form-control float-right textSearchContent_HiddenField" placeholder="Nhập từ khóa tìm kiếm" Style="min-width: 100px;"></asp:TextBox>
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

                            <div class="tool-left">
                            
                            </div>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body p-0">
                        <!--style="height: 500px;"-->
                        <div class="table-content p-0">
                            <asp:DataGrid DataKeyField="DONTHU_ID" Visible="true" runat="server" ID="dgDanhSach" AutoGenerateColumns="False" BorderWidth="0"
                                AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound" CssClass="table vertical-align-middle">
                                <%--text-nowrap--%>
                                <HeaderStyle CssClass="table-header" />
                                <Columns>
                                  
                                    <asp:BoundColumn HeaderText="DONTHU_ID" DataField="DONTHU_ID" Visible="false" HeaderStyle-CssClass="">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="STT" HeaderStyle-HorizontalAlign="Center" SortExpression="DONTHU_STT" HeaderStyle-CssClass="" HeaderStyle-Width="4%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <a title="Xem thông tin đơn thư" href="<%# objClassCommon.GET_URL_MODULE(this.PortalId,v_ModuleByDefinition_DONTHU,"&ctl=edit&mid="+v_ModuleByDefinition_DONTHU_KeyID+"&id="+ DataBinder.Eval(Container,"DataItem.DONTHU_ID"))%>" ><%# DataBinder.Eval(Container, "DataItem.DONTHU_STT") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Tên chủ đơn" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="" ItemStyle-Width="20%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>

                                            <a title="Xem thông tin đơn thư" href="<%# objClassCommon.GET_URL_MODULE(this.PortalId,v_ModuleByDefinition_DONTHU,"&ctl=edit&mid="+v_ModuleByDefinition_DONTHU_KeyID+"&id="+ DataBinder.Eval(Container,"DataItem.DONTHU_ID"))%>" >
                                                <%--<div>
                                                    <button type="button" class="btn btn-block btn-primary btn-lg" style="width: 40px; padding: 5px; height: 40px; border-radius: 50%; float: left; margin-right: 10px; margin-top: 10px; margin-bottom: 10px;">
                                                        TT</button>
                                                    <h6><b>Nguyễn Hoàng Tấn Tài</b></h6>
                                                    <p>A9-6, đường số 2, KDC Nam Long, P. Hưng Thạnh, Q. Cái Răng, TP. Cần Thơ</p>
                                                </div>
                                                <div>
                                                    <button type="button" class="btn btn-block btn-primary btn-lg" style="width: 40px; padding: 5px; height: 40px; border-radius: 50%; float: left; margin-right: 10px; margin-top: 10px; margin-bottom: 10px;">
                                                        TT</button>
                                                    <h6><b>Nguyễn Hoàng Tấn Tài</b></h6>
                                                    <p>A9-6, đường số 2, KDC Nam Long, P. Hưng Thạnh, Q. Cái Răng, TP. Cần Thơ</p>
                                                </div>--%>

                                                <%# getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>  </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Loại đối tượng" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-CssClass="" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%#  Eval("DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG_TEN").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nguồn đơn" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-CssClass="" HeaderStyle-Width="8%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                           <%#  Eval("NGUONDON_LOAI").ToString()=="0"?"Trực tiếp":"Gián tiếp" %><br />
                                            <%#  Eval("NGUONDON_LOAI").ToString()=="0"?"Ban tiếp công dân":(Eval("NGUONDON_LOAI_CHITIET").ToString()=="0"?"Trung ương":Eval("NGUONDON_LOAI_CHITIET").ToString()=="1"?"Địa phương":"Bưu chính") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Loại đơn" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-CssClass="" HeaderStyle-Width="8%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# getLoaiDonThu(int.Parse(Eval("LOAIDONTHU_CHA_ID").ToString())) %>  </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" SortExpression="DONTHU_NOIDUNG" HeaderStyle-CssClass="" HeaderStyle-Width="25%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <%# vClassCommon.GioiHanChu_Biding(DataBinder.Eval(Container, "DataItem.DONTHU_NOIDUNG"),50) %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="Thời gian nhận" HeaderStyle-HorizontalAlign="Center" SortExpression="NGAYTAO" HeaderStyle-CssClass="" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.NGAYTAO") %>
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
<style>
    .container-fluid .daskboard_flex 
    {
            display: flex;
    }
    .container-fluid .daskboard_flex .card
    {
        width: 100%;
    }
    ul.chart-legend.chart-legend-horizontal
    {
        padding-top:10px;
    }
    ul.chart-legend.chart-legend-horizontal li
    {
        padding-top:10px;
        padding-bottom:10px;

    }
    ul.chart-legend.chart-legend-horizontal li i
    {
        top: 3px;
        position: relative;
        padding-right: 10px;
    }
    .char-num
    {
        font-size: 19px;
        float: right;
        font-weight: 500;
        min-width: 50px;
        text-align: center;
    }
    .char-per
    {
        font-size: 19px;
        float: right;
        font-weight: 500;
        margin-left: 30px;
        min-width: 50px;
        text-align: center;
    }
    @media(max-width:1400px)
    {
        ul.chart-legend.chart-legend-horizontal li i {
           font-size: 1.5em;
        }
        .char-num {
            font-size: 16px;
            float: right;
            font-weight: 500;
            min-width: 10px;
            text-align: center;
        }
        .char-per {
            font-size: 14px;
            margin-left: 5px;
        }
        .chart-legend-title
        {

        }
    }
</style>