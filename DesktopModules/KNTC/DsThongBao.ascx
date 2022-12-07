<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DsThongBao.ascx.cs" Inherits="HOPKHONGGIAY.DsThongBao" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
<asp:UpdatePanel runat="server" ID="updatePN">
    <ContentTemplate>
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

                    <%--     <asp:DropDownList Visible="true" ID="ddlistDonVi" runat="server" OnSelectedIndexChanged="btnSearch_Click" AutoPostBack="true" CssClass="form-control slPhongBan" AppendDataBoundItems="true">

                    </asp:DropDownList>--%>
                    <asp:DropDownList Visible="false" ID="ddlistTrangThai"  runat="server" OnSelectedIndexChanged="btnSearch_Click" AutoPostBack="true" CssClass="form-control slPhongBan" AppendDataBoundItems="true">
                        <asp:ListItem Selected="True" Value="-1" Text="Tất cả trạng thái" />
                        <asp:ListItem Value="1" Text="Chưa gửi" />
                        <asp:ListItem Value="2" Text="Đã gửi" />
                        <asp:ListItem Value="3" Text="Huỷ" />
                    </asp:DropDownList>
                    <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                    </asp:LinkButton>
                </div>
                <div class="col-left" runat="server">
                    <asp:LinkButton ID="btn_ThemMoi" Visible="true" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="glyphicon glyphicon-send"></i>  Gửi thông báo hệ thống</asp:LinkButton>
                     <asp:LinkButton ID="buttonThietLap"  runat="server" OnClick="buttonThietLap_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="icofont-ui-settings fz18"></i> Thiết lập</asp:LinkButton>
                    <div id="divShowBtnXoa" style="display: none;" class="fleft">
                        <button type="button" class="btn-default btn-warning-delete btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" onclick="confirm_delete_rows_update('<%=btn_Xoa.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                    </div>
                    <div style="display: none;">
                        <asp:LinkButton ID="btn_Xoa" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="btn_Xoa_Click"><i class=""></i> Xóa</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
            <asp:Panel CssClass="baoloi" runat="server" ID="pnlThongBao" Visible="false">
                <asp:Label ID="lblThongBao" runat="server" Text=""></asp:Label>
            </asp:Panel>
            <asp:DataGrid DataKeyField="Id" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="True" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
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
                    <asp:BoundColumn HeaderText="Id" DataField="Id" Visible="false">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle />
                    </asp:BoundColumn>                                        
                    <asp:TemplateColumn HeaderText="Tiêu đề" HeaderStyle-HorizontalAlign="Left" SortExpression="TIEUDE">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                          <a id="dgChitiet" onserverclick="dgDanhSach_Sua" title="Thông tin phiên họp" href='<%# Eval("Id").ToString() %>' oncontextmenu="return false" runat="server">  <%# DataBinder.Eval(Container, "DataItem.Title") %></a>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                      <asp:TemplateColumn HeaderText="Nội dung" HeaderStyle-HorizontalAlign="Left" SortExpression="NOIDUNG">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                         <%# DataBinder.Eval(Container, "DataItem.Content") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                                     
                    <asp:BoundColumn SortExpression="SendDate" HeaderText="Thời gian gửi" DataField="SendDate" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                      <asp:TemplateColumn  HeaderText="Loại thông báo" Visible="true" HeaderStyle-HorizontalAlign="Left">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Kind").ToString() == "0" ? "Thông báo":
                                    (DataBinder.Eval(Container, "DataItem.Kind").ToString() == "1" ? "Thông báo cuộc họp" :
                                    (DataBinder.Eval(Container, "DataItem.Kind").ToString() == "2"? "Thông báo hệ thống" : "")) %>
                        </ItemTemplate>
                    </asp:TemplateColumn> 
                    
                    <asp:TemplateColumn HeaderText="Sửa" Visible="false">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a onserverclick="dgDanhSach_Sua" title="Thông tin thông báo" class="icon-sua" href='<%# Eval("Id").ToString() %>' oncontextmenu="return false" runat="server"></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Xóa" Visible="false">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a onserverclick="dgDanhSach_Xoa" title="Xóa phiên họp" class="icon-xoa" onclick="return getConfirmation(this, 'XÓA PHIÊN HỌP','Bạn muốn xóa phiên họp này?');" href='<%# Eval("Id").ToString()%>' oncontextmenu="return false" runat="server"></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999"></PagerStyle>
            </asp:DataGrid>
        </asp:Panel>
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
