<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DsDoiTuong.ascx.cs" Inherits="KNTC.DsDoiTuong" %>
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
            format: 'L'
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
                        <h3 class="card-title">Danh sách đối tượng</h3>
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
                                <div class="card-filter-advance">
                                    <nav class="navbar navbar-expand-lg ">
                                        <div class="collapse navbar-collapse" id="navbarNavDropdown">
                                            <ul class="navbar-nav">
                                                <li class="nav-item dropdown dropdown-filter">
                                                    <button type="button" class="btn btn-tool dropdown-toggle" aria-expanded="false" data-toggle="dropdown">
                                                        <i class="icofont-filter"></i>Bộ lọc
                                                    </button>
                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">

                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.DOITUONG_ID" data-type="normal" data-title="Mã đối tượng"><i class="icofont-check"></i>Mã đối tượng</a>
                                                        <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_HOTEN" data-type="normal"><i class="icofont-check"></i>Họ tên</a>
                                                        <a href="#" class="dropdown-item" data-option="CANHAN.CANHAN_DIACHI_DAYDU" data-type="normal"><i class="icofont-check"></i>Địa chỉ người đại diện</a>
                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.DOITUONG_DIACHI" data-type="normal"><i class="icofont-check"></i>Địa chỉ cơ quan tổ chức</a>
                                                        <div class="dropdown-divider"></div>
                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.NGAYTAO" data-type="datetime"><i class="icofont-check"></i>Thời gian</a>
                                                        <div class="datetime-content" data-option="DOITUONG.NGAYTAO"></div>
                                                        
                                                    </div>
                                                </li>

                                                <li class="nav-item dropdown dropdown-filter">
                                                    <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                                        <i class="icofont-checked"></i>Trạng thái
                                                    </button>
                                                    <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.DOITUONG_LOAI" data-type="equal" data-value="=1"><i class="icofont-check"></i>Cá nhân</a>
                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.DOITUONG_LOAI" data-type="equal" data-value="=2"><i class="icofont-check"></i>Nhóm đông người</a>
                                                        <a href="#" class="dropdown-item" data-option="DOITUONG.DOITUONG_LOAI" data-type="equal" data-value="=3"><i class="icofont-check"></i>Cơ quan, tổ chức</a>
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
                            <div class="tool-left">
                                <asp:LinkButton ID="btn_ThemMoi" runat="server" OnClick="btnThemMoi_Click" CssClass="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i> THÊM MỚI</asp:LinkButton>
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
                            <asp:DataGrid DataKeyField="DOITUONG_ID" runat="server" ID="dgDanhSach" AutoGenerateColumns="False" BorderWidth="0"
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
                                    <asp:BoundColumn HeaderText="DOITUONG_ID" DataField="DOITUONG_ID" Visible="false" HeaderStyle-CssClass="sticky">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Mã đối tượng" HeaderStyle-HorizontalAlign="Center" SortExpression="DOITUONG_ID" HeaderStyle-CssClass="sticky" HeaderStyle-Width="7%">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <a onserverclick="dgDanhSach_Sua" title="Cập nhật thông tin tiếp dân" href='<%# Eval("DOITUONG_ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.DOITUONG_ID") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Họ tên" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="sticky" ItemStyle-Width="20%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <a onserverclick="dgDanhSach_Sua" title="Cập nhật thông tin tiếp dân" href='<%# Eval("DOITUONG_ID").ToString() %>' oncontextmenu="return false" runat="server">                                         
                                                <%# getThongTinDoiTuong(int.Parse(Eval("DOITUONG_ID").ToString())) %>  </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Loại đối tượng" HeaderStyle-HorizontalAlign="Left" SortExpression="DOITUONG_LOAI" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                           <b> <%#Eval("DOITUONG_LOAI").ToString()=="1"?"Cá Nhân":(Eval("DOITUONG_LOAI").ToString()=="2"?"Nhóm đông người":Eval("DOITUONG_TEN").ToString()) %></b>
                                              <%#Eval("DOITUONG_LOAI").ToString()=="3"?("<p>"+Eval("DOITUONG_DIACHI").ToString()+"<p/>"):"" %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                              
                                    <asp:TemplateColumn HeaderText="Số người" HeaderStyle-HorizontalAlign="Right" SortExpression="DOITUONG_SONGUOI" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container, "DataItem.DOITUONG_SONGUOI") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Đối tượng bị KNTC" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                       <%#DataBinder.Eval(Container, "DataItem.DOITUONG_BIKNTC").ToString()=="True" ?"Đối tượng bị KNTC":""%>
                                            <%--<%#DataBinder.Eval(Container, "DataItem.DOITUONG_BIKNTC")!=null?(Boolean.Parse( DataBinder.Eval(Container, "DataItem.DOITUONG_BIKNTC").ToString()) == true ? "Đối tượng bị KNTC":""):"" %>--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Tiếp dân" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <a href="<%# objClassCommon.GET_URL_MODULE(this.PortalId,v_ModuleByDefinition_TIEPDAN,"&DOITUONG_ID="+ DataBinder.Eval(Container,"DataItem.DOITUONG_ID") )%>">
                                                <%#getTiepDanCount(int.Parse(Eval("DOITUONG_ID").ToString()))%>
                                            </a><%-- Eval("DOITUONG_ID").ToString()--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Đơn thư" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                          <a href="<%# objClassCommon.GET_URL_MODULE(this.PortalId,v_ModuleByDefinition_DONTHU,"&DOITUONG_ID="+ DataBinder.Eval(Container,"DataItem.DOITUONG_ID"))%>"><%# getDonThuCount(int.Parse(Eval("DOITUONG_ID").ToString()))%></a><%--+ Eval("DOITUONG_ID").ToString() --%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Thời gian tạo" HeaderStyle-HorizontalAlign="Center" SortExpression="NGAYTAO" HeaderStyle-CssClass="sticky" HeaderStyle-Width="10%">
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
