<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnPhienHop_ViTri.ascx.cs" Inherits="HOPKHONGGIAY.CnPhienHop_ViTri" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<%=vJavascriptMask %>
<script>
    //function pageLoad(sender, args) {

    //    if (args._isPartialLoad) { // postback
    //        //$('input.auto').autoNumeric('update');         
    //    }
    //    else { // not postback
    //        //$('input.auto').autoNumeric();
    //    }
    //}
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay"
            <div id="modalprogress">
                <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/DesktopModules/HOPKHONGGIAY/Images/ajax-loader.gif" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upn" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonThemmoi" runat="server" Visible="false" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" Visible="true" runat="server" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="false" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btnBoQua_Click" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>
                </div>
            </div>
            <div class="row bl">
                <div class="col-md-10">
                    <div class="col-sm-offset-4 col-sm-6 col-md-7 col-lg-6 pd-l30 pd-r30">
                        <asp:ValidationSummary ID="ValidationSummary1" CssClass="baoloi" runat="server" EnableClientScript="true" />
                        <asp:Panel CssClass="baoloi" runat="server" ID="pnlThongBao" Visible="false">
                            <asp:Label ID="lblThongBao" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div class="row box-body">
                <div class="row box9">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="col-sm-6 col-md-6 col-lg-6 pd-l15 pd-r15">
                                <asp:Panel ID="pnlFormDanhSach" runat="server" CssClass="form mr-b10" DefaultButton="buttonSearch">
                                    <div class="form-inline">
                                        <div class="col-right mr-b6">
                                            <asp:TextBox CssClass="form-control btn-sm" OnTextChanged="btnSearch_Click" Width="300" AutoPostBack="true" ID="textSearchContent" placeholder="Nhập từ khóa..." runat="server"></asp:TextBox>                                          
                                            <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                                                <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                                            </asp:LinkButton>
                                        </div>                                       
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach" Style="margin-bottom: 15px;">
                                    <asp:DataGrid runat="server" ID="dgDanhSach" AutoGenerateColumns="False" AllowPaging="false" OnItemDataBound="dgDanhSach_ItemDataBound" ><%--AutoPostBack="true"--%>
                                        <Columns>
                                            <asp:BoundColumn DataField="TEN" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Họ và tên"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Chức Vụ - Đơn vị" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# Eval("TENCHUCVU").ToString() + " - " +  Eval("TENDONVI").ToString() %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Mã ghế" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>                                                     
                                                    <asp:TextBox tooltip='<%# Eval("ID").ToString() + "|" +  Eval("LOAI").ToString() %>'    runat="server" AutoPostBack="true" OnTextChanged="NhapMaGhe" ID="textMaGhe" MaxLength="500" Style="height: 34px; margin-bottom: 3px" Text=''></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle CssClass="tieude" />
                                    </asp:DataGrid>
                                </asp:Panel>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <label runat="server" id="lblImage"></label>
                                <%--<object data="/DesktopModules/HOPKHONGGIAY/sodophong_a3.svg" type="image/svg+xml" width="600" id="blah">--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<style>
    .form_radiobuttonlist label {
        margin-top: 5px;
    }
</style>

