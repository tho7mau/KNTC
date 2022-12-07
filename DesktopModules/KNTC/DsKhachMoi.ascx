<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DsKhachMoi.ascx.cs" Inherits="HOPKHONGGIAY.DsKhachMoi" %>
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
                            <button type="button" class="btn btn-default" data-dismiss="modal">Không</button>
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
                    <asp:DropDownList Visible="true" ID="ddlistChucVu" runat="server" OnSelectedIndexChanged="btnSearch_Click" AutoPostBack="true" CssClass="form-control slPhongBan" AppendDataBoundItems="true">
                        <asp:ListItem Value="-1" Text="Tất cả chức vụ" Selected="True" />
                    </asp:DropDownList>
                    <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                    </asp:LinkButton>
                </div>
                <div class="col-left" runat="server">
                    <asp:LinkButton ID="btn_ThemMoi" runat="server" OnClick="btnThemMoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm mr-r3 min-width-100 mr-t3 mr-b6 fleft"><i class="icofont-plus"></i> Thêm mới</asp:LinkButton>
                    <div id="divShowBtnXoa" style="display: none;" class="fleft">
                        <button type="button" class="btn-default btn-warning-delete btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" onclick="confirm_delete_rows_update('<%=btn_Xoa.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                    </div>
                    <div style="display: none;">
                        <asp:LinkButton ID="btn_Xoa" OnClientClick="return getConfirmation(this, 'THÔNG BÁO','Bạn chắc chắn muốn xóa những khách mời đã chọn?');" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="btn_Xoa_Click"><i class=""></i> Xóa</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
            <asp:Panel CssClass="baoloi" runat="server" ID="pnlThongBao" Visible="false">
                <asp:Label ID="lblThongBao" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <asp:DataGrid DataKeyField="NGUOIDUNG_ID" runat="server" ID="dgDanhSach" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="True" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
                AllowSorting="True" OnSortCommand="dgDanhSach_SortCommand" OnItemDataBound="dgDanhSach_ItemDataBound">
                <HeaderStyle CssClass="tieude" />
                <Columns>
                    <asp:TemplateColumn HeaderText="STT" Visible="true">
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
                    <asp:BoundColumn HeaderText="NGUOIDUNG_ID" DataField="NGUOIDUNG_ID" Visible="false">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle />
                    </asp:BoundColumn>
                    <%-- <asp:TemplateColumn HeaderText="TT">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# STT() %>
                        </ItemTemplate>
                    </asp:TemplateColumn>--%>
                    <asp:TemplateColumn HeaderText="Tên khách mời" HeaderStyle-HorizontalAlign="Left" SortExpression="TenKhachMoi">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <a id="dgChiTiet" onserverclick="dgDanhSach_Sua" title="Cập nhật thông tin khách mời" href='<%# Eval("NGUOIDUNG_ID").ToString() %>' oncontextmenu="return false" runat="server"><%# DataBinder.Eval(Container, "DataItem.TENNGUOIDUNG") %></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                     <asp:TemplateColumn HeaderText="Tên đăng nhập" HeaderStyle-HorizontalAlign="Left" SortExpression="USERNAME">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Username") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Email" HeaderStyle-HorizontalAlign="Left" SortExpression="EMAIL">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EMAIL") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Điện thoại" HeaderStyle-HorizontalAlign="Left" SortExpression="SODIENTHOAI">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.SODIENTHOAI") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Đơn vị" HeaderStyle-HorizontalAlign="Left" SortExpression="DONVI">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.DONVI.TENDONVI") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                     <asp:TemplateColumn HeaderText="Phòng Ban" HeaderStyle-HorizontalAlign="Left" SortExpression="PHONGBAN">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.PhongBan.TENPHONGBAN") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Chức vụ" HeaderStyle-HorizontalAlign="Left" SortExpression="CHUCVU">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.CHUCVU.TENCHUCVU") %>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Đổi mật khẩu" Visible="true">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a onserverclick="DoiMatKhau" title="Đổi mật khẩu khách mời" href='<%# Eval("NGUOIDUNG_ID").ToString() %>' oncontextmenu="return false" runat="server" style='<%# (Eval("Username") == null || Eval("Username").ToString() == "" ) ? "display:none" : ""%>'><i class="icofont-ui-password fz18"></i></a>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                  
                    <asp:TemplateColumn HeaderText="Hoạt động" Visible="true" SortExpression="TRANGTHAI"> 
                        <HeaderStyle  />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                           <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.NGUOIDUNG_ID") %>'  onserverclick="ThayDoiTrangThai" runat="server">
                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("True"))?"<span class='glyphicon glyphicon-ok' style='color:#008000;'></span>":"<span style='color:red;' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                            </a>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:TemplateColumn HeaderText="Xóa" Visible="false">
                        <HeaderStyle Width="5%" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a onserverclick="dgDanhSach_Xoa" title="Xóa khách mời" class="icon-xoa" onclick="return getConfirmation(this, 'XÓA KHÁCH MỜI','Bạn muốn xóa khách mời này?');" href='<%# Eval("NGUOIDUNG_ID").ToString()%>' oncontextmenu="return false" runat="server"></a>
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
