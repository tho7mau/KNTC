<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DsPhienHopDiemDanh.ascx.cs" Inherits="HOPKHONGGIAY.DsPhienHopDiemDanh" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<script type="text/javascript" src="<%=vPathCommonJS %>"></script>
<script type="text/javascript">
    function Remove_tab() {
        $(".paping").each(function () {
            var $this = $(this);
            $this.html($this.html().replace(/&nbsp;/g, ''));
        });
    }
</script>

<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
            <div id="modalprogress">
                <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/DesktopModules/HOPKHONGGIAY/Images/ajax-loader.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel runat="server" ID="updatePN" >
    <ContentTemplate>
        <div class="pd-20">
        <div class="panel panel-default PSW">
    <div class="panel-body" id="ContentPane" runat="server">
   
        <script type="text/javascript" language="javascript">
            Sys.Application.add_load(Remove_tab);
        </script>
        <div>
            <div id="modalPopUp" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">
                                <span id="spnTitle"></span>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                <span id="spnMsg"></span>.
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Quay lại</button>
                            <button type="button" id="btnConfirm" class="btn btn-danger">
                                Có</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlFormDanhSach" runat="server" CssClass="form mr-b10" DefaultButton="buttonSearch">
            <div class="form-inline">
                <div class="col-right mr-b6">
                    <asp:TextBox CssClass="form-control btn-sm" OnTextChanged="btnSearch_Click" Width="400" AutoPostBack="true" ID="textSearchContent" placeholder="Nhập từ khóa..." runat="server"></asp:TextBox>
                    <asp:DropDownList Visible="true" ID="ddlistDonVi" runat="server" OnSelectedIndexChanged="btnSearch_Click" AutoPostBack="true" CssClass="form-control slPhongBan" AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                    </asp:LinkButton>
                </div>
                <div class="col-left" runat="server">
                    <asp:LinkButton Visible="false" ID="btn_ThemMoi" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="icofont-plus"></i> Thêm mới</asp:LinkButton>                                
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
            <asp:Panel CssClass="baoloi" runat="server" ID="Panel1" Visible="false">
                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
            </asp:Panel>
            <asp:DataGrid DataKeyField="ID" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="True" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
                AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound">
                <HeaderStyle CssClass="tieude" />
                <Columns>
                    <asp:TemplateColumn HeaderText="STT" Visible="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" onclick="handle_checked_delete_all_rows(this,'divShowBtnXoa');" />
                        </HeaderTemplate>
                        <HeaderStyle Width="3%" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Width="3%" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRow" runat="server" onclick="handle_checked_delete_row(this,'divShowBtnXoa');" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn HeaderText="ID" DataField="ID" Visible="false">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle />
                    </asp:BoundColumn>

                    <asp:TemplateColumn HeaderText="Tên đại biểu" HeaderStyle-HorizontalAlign="Left" SortExpression="TEN">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <a id="dgChiTiet" onserverclick="DiemDanh" title="Thông tin câu hỏi" href='<%# Eval("ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.TEN") %></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                     <asp:TemplateColumn HeaderText="Đơn vị" HeaderStyle-HorizontalAlign="Left" SortExpression="DONVI">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# Eval("TENDONVI").ToString() %>                             
                        </ItemTemplate>
                    </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="Chức vụ" HeaderStyle-HorizontalAlign="Left" SortExpression="CHUCVU">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# Eval("TENCHUCVU").ToString() %>                             
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Đại biểu / Khách mời" HeaderStyle-HorizontalAlign="Left" SortExpression="LOAI">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# Eval("LOAI").ToString() == "daibieu" ? "Đại biểu" : "Khách mời"%>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Có mặt" Visible="true">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a onserverclick="DiemDanh" title="Điểm danh" href='<%# Eval("ID").ToString() + "/"+ Eval("LOAI").ToString() %>' oncontextmenu="return false" runat="server">
                               <%# DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA") != null ?  (Boolean.Parse(DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA").ToString()) == true ? "<span class='glyphicon glyphicon-ok' style='color:#008000;'></span>":"<span style='color:red;' class='glyphicon glyphicon-minus-sign'></span>") : "<span style='color:red;' class='glyphicon glyphicon-minus-sign'></span>" %> 
                            </a>                           
                        </ItemTemplate>
                    </asp:TemplateColumn>
                
                </Columns>
                <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999"></PagerStyle>
            </asp:DataGrid>
        </asp:Panel>
         </div>
</div></div>
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
