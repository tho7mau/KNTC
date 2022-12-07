<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtBieuQuyet.ascx.cs" Inherits="HOPKHONGGIAY.CtBieuQuyet" %>
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
<asp:UpdatePanel runat="server" ID="updatePN">
    <ContentTemplate>
        <div class="row line-g">
            <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">                                
                <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btnBoQua_Click" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>
            </div>
        </div>
        <div class="pd-20">
            <div class="panel panel-default">
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

                    <div class="row">
                        <div class="form-horizontal pd-0">
                            <div class="col-sm-12 col-md-12 col-lg-12 pd-t10 pd-0">
                                <div>
                                    <div class="heading-chitiet">BIỂU QUYẾT</div>
                                    <div class="pd-l30 pd-r30 pd-b7">
                                        <div class="form-group mr-t10">
                                            <asp:Literal runat="server" ID="textNoiDungBieuQuyet" Text="Không có biểu quyết"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
      
    
      <!-- Modal chi tiết biểu quyết-->
        <div>

            <div class="modal fade bs-example-modal-lg" id="modalNguoiChon" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel2">CHI TIẾT BIỂU QUYẾT</h4>
                        </div>

                        <div class="modal-body" id="pPrint" runat="server">
                            <asp:Literal runat="server" ID="labelNoiDungChiTietBieuQuyet"> </asp:Literal>
                        </div>
                        <div class="modal-footer">
                            <a class="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100" data-dismiss="modal">&nbsp; Đóng </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
           <asp:HiddenField runat="server" ID="lbDapAnId"></asp:HiddenField>
        <asp:Button ID="buttonLoadDsBieuQuyet" Text="Submit" runat="server" OnClick="LoadChiTietBieuQuyet" Style="display: none" />
        <!--End Modal-->
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

    function OpenModalBieuQuyet() {
        $('#modalNguoiChon').modal('show');
    }

    function HienThiDanhSachNguoiChon(a) {
        document.getElementById('<%=lbDapAnId.ClientID %>').value = a.getAttribute("data");
          document.getElementById("<%=buttonLoadDsBieuQuyet.ClientID%>").click();

          //$('#modalNguoiChon').modal('show');
          //$('#modalPopUp').modal('show');
          //Reload_TabBieuQuyet();
      }

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
