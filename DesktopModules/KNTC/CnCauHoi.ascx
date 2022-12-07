<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CnCauHoi.ascx.cs" Inherits="HOPKHONGGIAY.CnCauHoi" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" src="<%=vPathCommonJS%>"></script>
<%=vJavascriptMask %>
<asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
    <ProgressTemplate>
        <div id="overlay">
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
                    <asp:LinkButton ID="buttonThemmoi" runat="server" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" Visible="false"  runat="server" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Lưu</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="true" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
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
                            <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-2 control-label pd-r0" runat="server" id="labelCauHoi">Câu hỏi</label>
                                    <div class="col-sm-10 col-md-10 col-lg-10 ">
                                        <asp:TextBox ID="textCauhoi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control requirements" MaxLength="500" />
                                    </div>
                                </div>                                                                                                         
                            </div>

                            <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30" runat="server" Visible="false">                              
                                <div class="form-group mr-b0 mr-t10" runat="server">
                                   <label class="col-sm-2 control-label pd-r0" runat="server" id="labelThuTu">Thứ tự </label>
                                    <div class="col-sm-10 col-md-10 col-lg-10">
                                        <asp:TextBox ID="textSoThuTu" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" MaxLength="3" />
                                    </div>
                                </div>                                
                            </div>
                              
                              <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-2 control-label pd-r0" runat="server" id="labelDapAnMoi">Đáp án mới</label>
                                    <div class="col-sm-10 col-md-10 col-lg-10 ">
                                        <asp:TextBox ID="textDapAnMoi" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control requirements" MaxLength="500" />
                                    </div>
                                </div>                                                                                                         
                            </div>
                              <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-2 control-label pd-r0" runat="server" id="labelDapAn"></label>
                                    <div class="col-sm-10 col-md-10 col-lg-10 ">
                                   <asp:LinkButton ID="btnAddDapAn" OnClick="btnAddDapAn_Click" runat="server" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-plus"></i> Thêm đáp án</asp:LinkButton>
                                    </div>
                                </div>                                                                                                         
                            </div>
                           <%--  <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-2 control-label pd-r0" runat="server" id="label2">Đáp án</label>
                                    <div class="col-sm-10 col-md-10 col-lg-10 ">
                                         <asp:ListBox ID="listBoxDapAn" runat="server" Rows="10" Width="100%"></asp:ListBox>

                                       <div class="buton">
                                        <asp:ImageButton ID="cmdUp" CommandName="cmdUp" runat="server" AlternateText="Di chuyển lên" ImageUrl="/images/up.gif"
                                            OnClick="btn_cmdUp" />
                                        <asp:ImageButton ID="cmdDown" CommandName="cmdDown" runat="server" AlternateText="Di chuyển xuống"
                                            ImageUrl="/images/dn.gif" OnClick="btn_cmdDown" />
                                        <asp:ImageButton ID="cmdDeleteOption" CommandName="cmdDelOption" runat="server" AlternateText="Xóa"
                                            ImageUrl="/images/delete.gif" OnClick="btn_cmdDeleteOption" />
                                    </div>                                 
                                    </div>
                                </div>                                                                                                         
                            </div>--%>

                              <div class="col-sm-8 col-md-8 col-lg-8 col-md-offset-2 pd-l30 pd-r30">
                                <div class="form-group mr-t10">
                                    <label class="col-sm-2 control-label pd-r0" runat="server" id="label2">Đáp án</label>
                                    <div class="col-sm-10 col-md-10 col-lg-10 ">
                                         <asp:ListBox ID="listBoxDapAn" runat="server" Rows="10" Width="100%"></asp:ListBox>

                                                                 
                                    </div>
                                </div>                                                                                                         
                            </div>
                             <div class="col-sm-1 col-md-1 col-lg-1 pd-l0 pd-r50">
                                  <div class="form-group mr-t10">

                                       <div class="butonlistBox">
                                        <asp:ImageButton  ID="cmdUp" CommandName="cmdUp" runat="server" AlternateText="Di chuyển lên" ImageUrl="/images/up.gif"
                                            OnClick="btn_cmdUp" />
                                        <asp:ImageButton ID="cmdDown" CommandName="cmdDown" runat="server" AlternateText="Di chuyển xuống"
                                            ImageUrl="/images/dn.gif" OnClick="btn_cmdDown" />
                                        <asp:ImageButton ID="cmdDeleteOption" CommandName="cmdDelOption" runat="server" AlternateText="Xóa"
                                            ImageUrl="/images/delete.gif" OnClick="btn_cmdDeleteOption" />
                                    </div>      
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
        margin-bottom:20px;
    }
    .butonlistBox input
    {
        display:inherit;
        margin-bottom:10px;
    }
</style>
<script>

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
