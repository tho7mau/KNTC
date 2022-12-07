<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtPhienHop.ascx.cs" Inherits="HOPKHONGGIAY.CtPhienHop" %>
<%@ Register TagPrefix="dnnsc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register Src="chosenJS.ascx" TagName="chosenjs" TagPrefix="chosen" %>
<chosen:chosenjs runat="server" />
<dnn:DnnJsInclude ID="PathCommonJS" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/common.js' AddTag="false" />
<dnn:DnnJsInclude ID="JavascriptMask" runat="server" FilePath='/DesktopModules/HOPKHONGGIAY/Scripts/Mask/jquery.metadata.js' AddTag="false" />

<script>
    function Isdisplay() {
        document.getElementById("tailieu-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_File = document.getElementById("tailieu");
        content_ChuongTrinhHop.className = content_ChuongTrinhHop.className.replace("active", "");
        content_File.className = content_File.className.replace("tab-pane fade", "tab-pane fade active in");
    }
    function Reload_TabBieuQuyet() {
        document.getElementById("bieuquyet-tab").click();
        var content_ChuongTrinhHop = document.getElementById("chuongtrinhhop");
        var content_BieuQuyet = document.getElementById("bieuquyet");
        content_ChuongTrinhHop.className = content_ChuongTrinhHop.className.replace("tab-pane fade active in", "tab-pane fade");
        content_BieuQuyet.className = content_BieuQuyet.className.replace("tab-pane fade", "tab-pane fade active in");
    }

    function pageLoad(sender, args) {
        initchosen();
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
<asp:UpdatePanel ID="upn" runat="server">
    <ContentTemplate>

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


        <asp:Panel ID="pnlCN" CssClass="form" runat="server" DefaultButton="btnCapNhat">
            <div class="row line-g">
                <div class="col-sm-12 col-lg-11 mr-10 pd-0" style="text-align: left">
                    <asp:LinkButton ID="buttonSapXepViTri" runat="server" Visible="false" OnClick="buttonSapXepViTri_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-company fz17"></i> Sơ đồ vị trí</asp:LinkButton>
                    <asp:LinkButton ID="buttonDiemDanh" runat="server" Visible="false" OnClick="buttonDiemDanh_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-checked fz17"></i> Điểm danh</asp:LinkButton>
                    <asp:LinkButton ID="buttonGoiThongBao" runat="server" Visible="true" OnClick="buttonGoiThongBao_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="glyphicon glyphicon-send"></i> Gửi thông báo</asp:LinkButton>
                    <asp:LinkButton ID="buttonThemmoi" runat="server" Visible="false" OnClick="buttonThemmoi_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="glyphicon glyphicon-floppy-disk"></i> Thêm mới</asp:LinkButton>
                    <asp:LinkButton ID="btnCapNhat" runat="server" Visible="false" CausesValidation="false" OnClick="btnCapNhat_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-save"></i> Cập nhật</asp:LinkButton>
                    <asp:LinkButton ID="btnSua" Visible="true" runat="server" CausesValidation="false" OnClick="btnSua_Click" CssClass="btn btn-primary waves-effect none-radius none-shadow btn-sm min-width-100"><i class="icofont-ui-edit"></i>  Sửa</asp:LinkButton>
                    <asp:LinkButton ID="btnBoQua" runat="server" CssClass="btn btn-sm btn-default waves-effect none-radius none-shadow min-width-100" OnClick="btnBoQua_Click" CausesValidation="false"><i class='icofont-undo'></i> Trở về</asp:LinkButton>

                    <span runat="server" id="spanSetTrangThai" visible="false">
                        <asp:LinkButton ID="btnDaHop" runat="server" CssClass="fright buttonTrangThai" OnClick="btnDaHop_Click" CausesValidation="false"> Đã họp</asp:LinkButton>
                        <i class="icofont-thin-right fz18 fright"></i>
                        <asp:LinkButton ID="btnDangHop" runat="server" CssClass="fright buttonTrangThai" OnClick="btnDangHop_Click" CausesValidation="false"> Đang họp</asp:LinkButton>
                        <i class="icofont-thin-right fz18 fright"></i>
                        <asp:LinkButton ID="btnCongBo" runat="server" CssClass="fright buttonTrangThai" OnClick="btnCongBo_Click" CausesValidation="false"> Công bố</asp:LinkButton>
                    </span>
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
            <br />
            <div class="row box-body" style="overflow-y: auto">
                <div class="row " style="min-height: 300px">
                    <div class="form-horizontal">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <ul class="nav nav-tabs" id="myTab" role="tablist">
                                <li class="nav-item  <%=(TabActive.ToString()==""?"active":"") %>" id="liChuongTrinhHop">
                                    <a class="nav-link" id="chuongtrinhhop-tab" data-toggle="tab" href="#chuongtrinhhop" role="tab" aria-controls="chuongtrinhhop" aria-selected="true" aria-expanded="true"><i class="icofont-list"></i>Chương trình họp</a>
                                </li>

                                <li class="nav-item  <%=(TabActive.ToString()=="TAILIEU"?"active":"") %>" id="liTaiLieuHop">
                                    <a class="nav-link" id="tailieu-tab" data-toggle="tab" href="#tailieu" role="tab" aria-controls="tailieu" aria-selected="false"><i class="icofont-attachment"></i>Tài liệu</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="BIENBAN"?"active":"") %>" id="liBienBanHop">
                                    <a class="nav-link" id="bienbanhop-tab" data-toggle="tab" href="#bienbanhop" role="tab" aria-controls="bienbanhop" aria-selected="false"><i class="icofont-law-document"></i>Biên bản</a>
                                </li>
                                <li class="nav-item <%=(TabActive.ToString()=="KETLUAN"?"active":"") %>" id="liKetLuan">
                                    <a class="nav-link" id="ketluan-tab" data-toggle="tab" href="#ketluan" role="tab" aria-controls="ketluan" aria-selected="false"><i class="icofont-file-document"></i>Kết luận</a>
                                </li>

                                <li class="nav-item   <%=(TabActive.ToString()=="BIEUQUYET"?"active":"") %>" id="liBieuQuyet">
                                    <a class="nav-link" id="bieuquyet-tab" data-toggle="tab" href="#bieuquyet" role="tab" aria-controls="bieuquyet" aria-selected="false"><i class="icofont-numbered"></i>Biểu quyết</a>
                                </li>

                                <li class="nav-item <%=(TabActive.ToString()=="SODO"?"active":"") %>" id="liSoDoPhong">
                                    <a class="nav-link" id="sodophong-tab" data-toggle="tab" href="#sodophong" role="tab" aria-controls="sodophong" aria-selected='false'><i class="icofont-company fz17"></i>Sơ đồ</a>
                                </li>

                                <li class="nav-item <%=(TabActive.ToString()=="DIEMDANH"?"active":"") %>" id="liDiemDanh">
                                    <a class="nav-link" id="diemdanh-tab" data-toggle="tab" href="#diemdanh" role="tab" aria-controls="diemdanh" aria-selected='false'><i class="icofont-checked"></i>Điểm danh</a>
                                </li>

                                <%-- <li class="nav-item" id="liDiemDanh">
                                    <a class="nav-link" id="diemdanh-tab" data-toggle="tab" href="#diemdanh" role="tab" aria-controls="diemdanh" aria-selected="false"><i class="icofont-checked"></i>Điểm danh</a>
                                </li>--%>

                                <li class="nav-item <%=(TabActive.ToString()=="PHATBIEU"?"active":"") %>" id="liPhatBieu">
                                    <a class="nav-link" id="phatbieu-tab" data-toggle="tab" href="#phatbieu" role="tab" aria-controls="phatbieu" aria-selected="false"><i class="icofont-hand"></i>Phát biểu</a>
                                </li>
                            </ul>

                            <%--Tab chương trình họp--%>
                            <div class="tab-content " id="myTabContent">
                                <div class="tab-pane fade <%=(TabActive.ToString()==""?"active in":"")%>" id="chuongtrinhhop" role="tabpanel" aria-labelledby="chuongtrinhhop-tab">
                                    <div class="col-sm-12 col-md-12 col-lg-12 pd-t10 pd-0">
                                        <div class="col-sm-12 col-md-12 col-lg-12 pd-0 ">
                                            <div class="block_chitiet pd-b10">
                                                <div class="heading-chitiet">THÔNG TIN PHIÊN HỌP</div>
                                                <div class="pd-l30 pd-r30 row">
                                                    <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-2 control-label pd-r0 pd-l0" runat="server" id="labelThoiGian">Thời gian</label>
                                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                                <asp:TextBox ID="textThoiGian" runat="server" ReadOnly="true" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-2 control-label pd-r0 pd-l0" runat="server" id="label5">Địa điểm</label>
                                                            <div class="col-sm-10 col-md-10 col-lg-10 ">
                                                                <asp:TextBox ID="textDiaDiem" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-2 control-label pd-r0 pd-l0" runat="server" id="labelDonVi">Đơn vị</label>
                                                            <div class="col-sm-10 col-md-10 col-lg-10 pd-t8">
                                                                <asp:TextBox ID="textDonVi" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label runat="server" id="labelThoiGianKetThuc" class="col-sm-2 control-label pd-r0 pd-l0">Chủ trì </label>
                                                            <div class="col-sm-10 col-md-10 col-lg-10" style="text-align: center">
                                                                <asp:TextBox ID="textChuTri" TextMode="MultiLine" Rows="4" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="block_chitiet">
                                                <div class="heading-chitiet">NỘI DUNG HỌP</div>
                                                <div class="pd-l30 pd-r30 ">
                                                    <div class="form-group mr-t10">
                                                        <asp:Label ID="textChuongTrinhHop" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div runat="server" id="divGhiChu" visible="false">
                                                    <div class="heading-chitiet">GHI CHÚ</div>
                                                    <div class="pd-l30 pd-r30 ">
                                                        <div class="form-group mr-t10">
                                                            <asp:Label ID="labelGhiChu" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-12 col-md-12 pd-l10 pd-0 mr-l0">

                                            <div class="pd-b10 pd-r10 col-sm-12 col-md-6 pd-l0">
                                                <div class="block_chitiet">
                                                    <div class="heading-chitiet">ĐẠI BIỂU</div>
                                                    <div class="pd-l30 pd-r30 ">
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0" runat="server" id="label7">Tổng số đại biểu</label>
                                                            <div class="col-sm-8 col-md-8 col-lg-8 ">
                                                                <asp:TextBox ID="textTongSoDaiBieu" ReadOnly="true" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0 color-thamdu" runat="server" id="label9">Tham dự</label>
                                                            <div class="col-sm-8 col-md-18 col-lg-8 ">
                                                                <asp:TextBox ID="textSoLuongDaiBieuThamDu" ReadOnly="true" runat="server" CssClass="form-control color-thamdu" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0 color-vangmat" runat="server" id="label8">Vắng mặt</label>
                                                            <div class="col-sm-8 col-md-8 col-lg-8 ">
                                                                <asp:TextBox ID="textSoLuongDaiBieuVangMat" ReadOnly="true" runat="server" CssClass="form-control color-vangmat" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <asp:Panel ID="pnlDanhSach" runat="server" CssClass="danhsach">
                                                                <asp:DataGrid runat="server" ID="dgDanhSach" AutoGenerateColumns="False" CssClass="" ShowHeader="false">
                                                                    <HeaderStyle CssClass="tieude" />
                                                                    <Columns>
                                                                        <%--<asp:BoundColumn ItemStyle-Width="15%" HeaderText="Đại biểu" DataField="NGUOIDUNG_ID" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>--%>
                                                                        <asp:TemplateColumn HeaderText="Đại biểu" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <span title='<%#Eval("TENNGUOIDUNG") %>' class='<%# (Eval("THAMDU") != null && Eval("THAMDU").ToString() == "True") ? "color-thamdu" : "color-vangmat" %>'><%#Eval("TENNGUOIDUNG").ToString() + " - " + Eval("TENCHUCVU").ToString() + " " + Eval("TENDONVI").ToString() %></span>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class=" pd-b10 pd-l10 col-sm-12 col-md-6 pd-r0">
                                                <div class="block_chitiet">
                                                    <div class="heading-chitiet">KHÁCH MỜI</div>
                                                    <div class="pd-l30 pd-r30 ">
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0" runat="server" id="label10">Tổng số khách mời</label>
                                                            <div class="col-sm-8 col-md-8 col-lg-8 ">
                                                                <asp:TextBox ID="textTongSoKhachMoi" ReadOnly="true" runat="server" CssClass="form-control" Text="3" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0 color-thamdu" runat="server" id="label11">Tham dự</label>
                                                            <div class="col-sm-8 col-md-18 col-lg-8 ">
                                                                <asp:TextBox ID="textSoLuongKhachMoiThamDu" ReadOnly="true" runat="server" CssClass="form-control color-thamdu" Text="2" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <label class="col-sm-4 control-label pd-r0 pd-l0 color-vangmat" runat="server" id="label12">Vắng mặt</label>
                                                            <div class="col-sm-8 col-md-8 col-lg-8 ">
                                                                <asp:TextBox ID="textSoLuongKhachMoiVangMat" ReadOnly="true" runat="server" CssClass="form-control color-vangmat" Text="1" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group mr-t10">
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="danhsach">
                                                                <asp:DataGrid runat="server" ID="dgDanhSachKhachMoi" AutoGenerateColumns="False" CssClass="" ShowHeader="false">
                                                                    <HeaderStyle CssClass="tieude" />
                                                                    <Columns>
                                                                        <%--<asp:BoundColumn ItemStyle-Width="15%" HeaderText="Đại biểu" DataField="NGUOIDUNG_ID" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>--%>
                                                                        <asp:TemplateColumn HeaderText="Đại biểu" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <span class='<%# (Eval("THAMDU") != null && Eval("THAMDU").ToString() == "True") ? "color-thamdu" : "color-vangmat" %>'><%#Eval("KHACHMOI") %> </span>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--End tab chương trình họp--%>
                                <%-- <div class="tab-pane fade" id="khachmoi" role="tabpanel" aria-labelledby="khachmoi-tab">
                                    <div class="row">
                                        khách mời
                                    </div>
                                </div>--%>
                                <%--Tab tài liệu họp--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="TAILIEU"?"active in":"")%>" id="tailieu" role="tabpanel" aria-labelledby="tailieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal hidden">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="col-sm-6 col-md-6 col-lg-6 pd-l30 pd-r30">
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelTenTaiLieu">Tên tài liệu</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:TextBox ID="textTenTaiLieu" runat="server" CssClass="form-control requirements" MaxLength="200" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group mr-t10">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelMota">Mô tả</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:TextBox ID="textMotaFile" runat="server" CssClass="form-control requirements" MaxLength="200" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group mr-b0 mr-t10" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="labelFile">Chọn tài liệu</label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:FileUpload ID="f_file" runat="server" CssClass="" AllowMultiple="true" Style="float: left;" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group mr-b0 mr-t10 offset-md-3" runat="server">
                                                        <label class="col-sm-3 control-label pd-r0" runat="server" id="label6"></label>
                                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                                            <asp:LinkButton ID="btn_TL" runat="server" UseSubmitBehavior="false" CausesValidation="false" CssClass="btn btn-sm btn-default none-radius shadow-btn-sm min-width-100" OnClick="btn_TL_Click"><i class="fa fa-upload" aria-hidden="true"></i>Tải lên</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="danhsach pd-t10" Style="padding: 0px">
                                            <asp:Label runat="server" ID="lblDanhSachFile" CssClass="none-noidung" Visible="false">Chưa có tài liệu cho phiên họp</asp:Label>
                                            <asp:GridView ID="dgDanhSach_File" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tên tài liệu" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Nhóm" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("NHOM")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quyền xem" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("QUYEN")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Độ phổ biến" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#(DataBinder.Eval(Container,"DataItem.DOMAT").ToString().Equals("Mật"))?"Phổ biến":"Không phổ biến" %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  <asp:TemplateField HeaderText="Tên file" Visible="false">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("MOTA")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <%#Eval("TRANGTHAI").ToString() == "0" ? "Dự thảo" : "Đã duyệt"%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFile+"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ItemStyle-Width="10%" Visible="false" HeaderText="Xóa" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove'></i>" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <%--End tab tài liệu họp--%>
                                <%--Tab kết luận--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="KETLUAN"?"active in":"")%>" id="ketluan" role="tabpanel" aria-labelledby="ketluan-tab">
                                    <div class="row">
                                        <asp:Panel ID="Panel4" runat="server" CssClass="danhsach" Style="padding: 0px">
                                            <asp:Label runat="server" ID="lblKetLuan" CssClass="none-noidung" Visible="false">Chưa có nội dung kết luận cuộc họp</asp:Label>
                                            <asp:GridView ID="dgDanhSach_FileKetLuan" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach_KetLuan" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tài liệu kết luận" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                   
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiTrangThai" runat="server">
                                                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("1"))?"<span class='glyphicon glyphicon-ok' title='Đã duyệt' style='color:#008000;'></span>":"<span style='color:red;' title='Chờ duyệt' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFileKetLuan +"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ItemStyle-Width="10%" HeaderText="Xóa" Visible="false" ShowDeleteButton="False" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove' onclick='alert(abc);'></i>" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <%--end tab kết luận--%>


                                <%--Tab biên bản họp--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="BIENBAN"?"active in":"")%>" id="bienbanhop" role="tabpanel" aria-labelledby="bienbanhop-tab">
                                    <div class="row">
                                        <asp:Panel ID="Panel2" runat="server" CssClass="danhsach" Style="padding: 0px">
                                            <asp:Label runat="server" ID="lblPhienBanHop" CssClass="none-noidung" Visible="false">Chưa có nội dung biên bản họp</asp:Label>
                                            <asp:GridView ID="dgDanhSach_FileBienBan" CssClass="Grid" runat="server" OnRowDeleting="XoaFileKhoiDanhSach_BienBan" Width="100%" DataKeyNames="HA_ID, HA_FILE_PATH"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="tieude" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tên tài liệu" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Eval("TENTAILIEU")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                   
                                                    <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="lnkKichHoat" href='<%# DataBinder.Eval(Container,"DataItem.HA_ID") %>' onserverclick="ThayDoiTrangThai" runat="server">
                                                                <%#(DataBinder.Eval(Container,"DataItem.TRANGTHAI").ToString().Equals("1"))?"<span class='glyphicon glyphicon-ok' title='Đã duyệt' style='color:#008000;'></span>":"<span style='color:red;' title='Chờ duyệt' class='glyphicon glyphicon-minus-sign'></span>" %>                                                         
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Tải về" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a oncontextmenu="return false" target="_blank" href='<%#vPathCommonUploadFileBienBan+"/" +Eval("HA_FILE_PATH")%>' runat="server"><i class="glyphicon glyphicon-download-alt"></i></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField ItemStyle-Width="10%" Visible="false" HeaderText="Xóa" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center" DeleteText="<i class='glyphicon glyphicon-remove' onclick='alert(abc);'></i>" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <%--end tab biên bản họp--%>


                                <%--Tab biểu quyết--%>
                                <div class="tab-pane fade <%=(TabActive.ToString()=="BIEUQUYET"?"active in":"")%>" id="bieuquyet" role="tabpanel" aria-labelledby="bieuquyet-tab">
                                    <div class="row">
                                        <div class="form-horizontal pd-0">
                                            <div class="col-sm-12 col-md-12 col-lg-12 pd-t10 pd-0">
                                                <div class="block_chitiet">
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
                                <%--End tab biểu quyết--%>

                                <%--Tab phát biểu--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="PHATBIEU"?"active in":"")%>" id="phatbieu" role="tabpanel" aria-labelledby="phatbieu-tab">
                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12 pd-t10">
                                                <asp:Panel ID="pnDanhSachPhatBieu" runat="server" CssClass="danhsach">

                                                    <asp:Label runat="server" ID="lblPhatBieu" Visible="false" CssClass="none-noidung">Chưa có đại biểu nào đăng ký phát biểu </asp:Label>
                                                    <asp:DataGrid runat="server" ID="dgDanhSachPhatBieu" AutoGenerateColumns="False" CssClass="">
                                                        <HeaderStyle CssClass="tieude" />
                                                        <Columns>
                                                            <%--<asp:BoundColumn ItemStyle-Width="15%" HeaderText="Đại biểu" DataField="NGUOIDUNG_ID" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>--%>
                                                            <%-- <asp:TemplateColumn HeaderText="Đại biểu" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <span title='<%#Eval("TENNGUOIDUNG") %>' class='<%# (Eval("THAMDU") != null && Eval("THAMDU").ToString() == "True") ? "color-thamdu" : "color-vangmat" %>'><%#Eval("TENNGUOIDUNG").ToString() + " - " + Eval("TENCHUCVU").ToString() + " " + Eval("TENDONVI").ToString() %></span>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>--%>
                                                            <asp:TemplateColumn HeaderText="Nội dung phát biểu" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("NOIDUNGDANGKY") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Người phát biểu" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%#Eval("NGUOIDUNG.TENNGUOIDUNG") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Thời gian phát biểu" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# String.Format("{0:HH:MM dd/MM/yyyy}",Eval("THOIGIANDANGKY")) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# (Eval("MOIPHATBIEU").ToString() == "False" && Eval("TRANGTHAI").ToString() == "False") ? "Chưa phát biểu" : 
                                                                            (Eval("MOIPHATBIEU").ToString() == "False" && Eval("TRANGTHAI").ToString() == "True") ? "Từ chối" : "Đã phát biểu" %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--End tab phát biểu--%>

                                <%--Tab điểm danh--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="DIEMDANH"?"active in":"")%>" id="diemdanh" role="tabpanel" aria-labelledby="diemdanh-tab">
                                    <div class="row">
                                        <div class="col-sm-12 col-md-12 col-lg-12">
                                            <asp:Panel ID="pnlFormDanhSach" runat="server" CssClass="form mr-b10" DefaultButton="buttonSearch">
                                                <div class="form-inline">
                                                    <div class="col-right mr-b6">
                                                        <asp:TextBox CssClass="form-control btn-sm" OnTextChanged="btnSearch_Click" Width="400" AutoPostBack="true" ID="textSearchContent" placeholder="Nhập từ khóa..." runat="server"></asp:TextBox>

                                                        <asp:LinkButton ID="buttonSearch" runat="server" Visible="false" CssClass="btn btn-primary btn-sm btn-search" OnClick="btnSearch_Click">
                                                        <i class="glyphicon glyphicon-search"></i>&nbsp;Tìm
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-left" runat="server">
                                                        <div id="divShowBtnXoa" style="display: none;" class="fleft">
                                                            <button type="button" class="btn-default btn-warning-delete btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" onclick="confirm_delete_rows_update('<%=btn_DiemDanh.ClientID%>')"><i class="icofont-ui-delete"></i>Xóa</button>
                                                        </div>
                                                        <div style="display: none;">
                                                            <asp:LinkButton ID="btn_DiemDanh" CausesValidation="false" Visible="true" runat="server" CssClass="btn-default btn-sm none-radius  btn min-width-100 mr-t3 mr-b3" OnClick="DiemDanhNhieuDaiBieu"><i class=""></i> Điểm danh</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>


                                            <asp:Panel ID="Panel5" runat="server" CssClass="danhsach">
                                                <asp:DataGrid DataKeyField="ID" runat="server" ID="dgDanhSach_DiemDanh" OnInit="dgDanhSach_Init" AutoGenerateColumns="False" OnPageIndexChanged="dgDanhSach_PageIndexChanged" AllowPaging="False" AllowCustomPaging="False" OnItemCreated="dgDanhSach_ItemCreated"
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
                                                                <%# DataBinder.Eval(Container, "DataItem.TEN") %>
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

                                                        <asp:TemplateColumn HeaderText="Dự thay/Lý do vắng" Visible="true">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                            <ItemTemplate>
                                                                <%# Eval("NGUOIDUTHAY") != null ? Eval("NGUOIDUTHAY") :(Eval("LYDOVANG") != null ? Eval("LYDOVANG").ToString() : "") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Có mặt" Visible="true">
                                                            <HeaderStyle />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA") != null ?  (Boolean.Parse(DataBinder.Eval(Container, "DataItem.XACNHANTHAMGIA").ToString()) == true ? "<span  style='color: green;'>Tham dự</span>":"<span style='color:red;'>Vắng mặt</span>") : "<span style='color:red;'>Vắng mặt</span>" %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <%-- <asp:TemplateColumn HeaderText="Sửa" Visible="false">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <a onserverclick="dgDanhSach_Sua" style='<%#kiemtra(int.Parse(DataBinder.Eval(Container, "DataItem.CAUHOI_ID").ToString())).ToString()=="True"?"display:none": "" %>' title="Cập nhật thông tin câu hỏi" class="icon-sua" href='<%# Eval("CAUHOI_ID").ToString() %>' oncontextmenu="return false" runat="server"></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Xóa" Visible="false">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <a onserverclick="dgDanhSach_Xoa" title="Xóa câu hỏi" class="icon-xoa" onclick="return getConfirmation(this, 'THÔNG BÁO','Bạn muốn xóa câu hỏi này?');" href='<%# Eval("CAUHOI_ID").ToString()%>' oncontextmenu="return false" runat="server"></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>--%>
                                                    </Columns>
                                                    <PagerStyle Mode="NumericPages" CssClass="paping" PageButtonCount="9999"></PagerStyle>
                                                </asp:DataGrid>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <%--End tab điểm danh--%>

                                <%--Tab sơ đồ phòng--%>
                                <div class="tab-pane fade row box9 <%=(TabActive.ToString()=="SODO"?"active in":"")%>" id="sodophong" role="tabpanel" aria-labelledby="sodophong-tab">

                                    <div class="row">
                                        <div class="form-horizontal">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="text-center">
                                                    <asp:Label runat="server" ID="lblSoDoPhong" Visible="false" CssClass="none-noidung">Chưa chọn phòng cho phiên họp</asp:Label>
                                                    <label runat="server" visible="true" style="width: 90%" id="lblImage"></label>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--End tab sơ đồ phòng--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>



    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btn_TL" />
    </Triggers>
</asp:UpdatePanel>
<style>
    .form_radiobuttonlist label {
        margin-top: 5px;
    }

    .buttonTrangThai {
        color: #393939 !important;
    }

    .activeTrangThai {
        color: #0182c6 !important;
        font-weight: bold;
    }

    .none-noidung {
        font-size: 13px;
        /* font-weight:bold;     */
    }
</style>
<script>
    function HienThiDanhSachNguoiChon(a) {
        document.getElementById('<%=lbDapAnId.ClientID %>').value = a.getAttribute("data");
        document.getElementById("<%=buttonLoadDsBieuQuyet.ClientID%>").click();

        //$('#modalNguoiChon').modal('show');
        //$('#modalPopUp').modal('show');
        //Reload_TabBieuQuyet();
    }

    function OpenModalBieuQuyet() {
        $('#modalNguoiChon').modal('show');
    }
    function getConfirmation(sender, title, message) {
        $("#spnTitle").text(title);
        $("#spnMsg").text(message);
        $('#modalPopUp').modal('show');
        $('#btnConfirm').attr('onclick', "$('#modalPopUp').modal('hide');setTimeout(function(){" + $(sender).prop('href') + "}, 50);");
        return false;
    }

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
