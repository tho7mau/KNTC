#region Thông tin chung
/// Mục đích        :Chi tiết phiên họp
/// Ngày tại        :20/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using Microsoft.Web.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CtPhienHop : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        DataTable dtTable;
        int vPageSize = ClassParameter.vPageSize;
        int vCurentPage = 0;

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        public string vPathCommonUploadFile = ClassParameter.vPathCommonUploadTaiLieuHop;

        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        DuyetPhienHopController vDuyetPhienHopControllerInfo = new DuyetPhienHopController();
        public string vPathCommonUploadFileKetLuan = ClassParameter.vPathCommonUploadKetLuan;
        public string vPathCommonUploadFileBienBan = ClassParameter.vPathCommonUploadBienBanHop;

        PhienHopNguoiDungController vPhienHopNguoiDungControllerInfo = new PhienHopNguoiDungController();
        PhienHopKhachMoiController vPhienHopKhachMoiControllerInfo = new PhienHopKhachMoiController();
        string vMacAddress = ClassCommon.GetMacAddress();
        public String TabActive = "";

        #endregion

        #region Events
        /// <summary>
        /// Event page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClearCacheItems();
                //Kiem tra quyen dang nhap
                CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vPhienHopId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vPhienHopId);
                    SetInfoSoDo(vPhienHopId);
                    LoadDanhSach_DiemDanh(0);
                    LoadDSFile(vPhienHopId);
                    LoadDSFile_BIENBAN(vPhienHopId);
                    LoadDSFile_KetLuan(vPhienHopId);
                    //textTenKhachMoi.Focus();                   
                }
                //Edit Title
                if (vPhienHopId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / Thêm mới";
                }
                else
                {
                    var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                    if (vPhienHopInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / " + vPhienHopInfo.TIEUDE;
                    }
                }
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Clear cache 
        /// </summary>
        public void ClearCacheItems()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            for (int i = 0; i < keys.Count; i++)
                Cache.Remove(keys[i]);
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            // call your download function
            //if (ViewState["OpenModalBieuQuyet"] != null)
            //{
            //    ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "ReloadTabBieuQuyet", "Reload_TabBieuQuyet();", true);
            //    //Reload_TabBieuQuyet();
            //    if (ViewState["OpenModalBieuQuyet"].ToString() == "Open")
            //    {
            //        ViewState["OpenModalBieuQuyet"] = null;
            //        OpenModalBieuQuyet();
            //    }
            //}
            if (ViewState["Tab"] != null)
            {
                TabActive = ViewState["Tab"].ToString();
            }
        }

        /// <summary>
        /// Event button Cap nhat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            //if (ValidateForm() == true)
            //{
            //    CapNhat(vPhienHopId);
            //}
        }


        protected void LoadChiTietBieuQuyet(object sender, EventArgs e)
        {
            try
            {
                int vDapAnId = 0;
                if (lbDapAnId.Value != "")
                {
                    vDapAnId = int.Parse(lbDapAnId.Value);
                }
                if (vDapAnId > 0)
                {
                    int vStt = 1;
                    string vResultHtml = "";
                    var vDanhSachDaiBieuChonDapAn = (from DaiBieu_BQ in vDataContext.DAIBIEU_BIEUQUYETs
                                                     join DaiBieu in vDataContext.NGUOIDUNGs on DaiBieu_BQ.DAIBIEU_ID equals DaiBieu.NGUOIDUNG_ID
                                                     where DaiBieu_BQ.DAPANTRALOI_ID == vDapAnId
                                                     select DaiBieu).ToList();
                    var vDapAnInfo = vDataContext.DAPANCAUHOIs.Where(x => x.DAPANCAUHOI_ID == vDapAnId).SingleOrDefault();
                    if (vDanhSachDaiBieuChonDapAn.Count > 0)
                    {
                        vResultHtml += "<h5>Danh sách Đại biểu lựa chọn <b>" + vDapAnInfo.NOIDUNG + "</b> cho câu hỏi <b>" + vDapAnInfo.CAUHOI.NOIDUNG + ": </b></h5><br/>";
                        foreach (var DaiBieu in vDanhSachDaiBieuChonDapAn)
                        {

                            vResultHtml += vStt + ". " + "<b>" + DaiBieu.TENNGUOIDUNG + "</b>" + " - " + DaiBieu.CHUCVU.TENCHUCVU + " " + DaiBieu.DONVI.TENDONVI + "<br/>";
                            vStt++;
                        }
                    }
                    else
                    {
                        vResultHtml += "<h5>Không có đại biểu nào chọn đáp án này.</h5>";
                    }
                    labelNoiDungChiTietBieuQuyet.Text = vResultHtml;

                }
                if (ViewState["OpenModalBieuQuyet"] == null)
                {
                    ViewState["OpenModalBieuQuyet"] = "Open";
                }
                ViewState["Tab"] = "BIEUQUYET";                
                OpenModalBieuQuyet();
                //Reload_TabBieuQuyet();


            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCongBo_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                    if (vPhienHopInfo != null)
                    {
                        vPhienHopInfo.TRANGTHAI = 2;
                        vDataContext.SubmitChanges();
                        btnCongBo.CssClass += " activeTrangThai";
                        btnDangHop.CssClass = btnDangHop.CssClass.Replace("activeTrangThai", "").Trim();
                        btnDaHop.CssClass = btnDaHop.CssClass.Replace("activeTrangThai", "").Trim();
                        ClassCommon.ShowToastr(Page, "Chuyển trạng thái phiên họp thành công. Chuyển về trạng thái công bố", "Thông báo", "success");
                    }
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnDangHop_Click(object sender, EventArgs e)
        {
            try
            {
                var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                if (vPhienHopInfo != null)
                {
                    vPhienHopInfo.TRANGTHAI = 4;
                    vDataContext.SubmitChanges();
                    btnDangHop.CssClass += " activeTrangThai";
                    btnCongBo.CssClass = btnCongBo.CssClass.Replace("activeTrangThai", "").Trim();
                    btnDaHop.CssClass = btnDaHop.CssClass.Replace("activeTrangThai", "").Trim();

                    ClassCommon.ShowToastr(Page, "Chuyển trạng thái phiên họp thành công. Phiên họp đã được bắt đầu", "Thông báo", "success");
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void btnDaHop_Click(object sender, EventArgs e)
        {
            try
            {
                var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                if (vPhienHopInfo != null)
                {
                    vPhienHopInfo.TRANGTHAI = 5;

                    btnDaHop.CssClass += " activeTrangThai";
                    btnDangHop.CssClass = btnDangHop.CssClass.Replace("activeTrangThai", "").Trim();
                    btnCongBo.CssClass = btnCongBo.CssClass.Replace("activeTrangThai", "").Trim();

                    vDataContext.SubmitChanges();
                    ClassCommon.ShowToastr(Page, "Chuyển trạng thái phiên họp thành công. Phiên họp đã được kết thúc", "Thông báo", "success");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void buttonSapXepViTri_Click(object sender, EventArgs e)
        {
            try
            {
                string vUrl = Globals.NavigateURL("sodo", "mid=" + this.ModuleId, "title=Sơ đồ vị trí", "id=" + vPhienHopId);
                Response.Redirect(vUrl);
            }
            catch (Exception ex)
            {

            }
        }

        protected void buttonDiemDanh_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("diemdanh", "mid=" + this.ModuleId, "title=Điểm danh", "id=" + vPhienHopId);
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Event nhan button Bo Qua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBoQua_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void SetFormInfo(int pPhienHopId)
        {
            try
            {
                if (pPhienHopId == 0)//Thêm mới
                {

                }
                else // Cập nhật
                {
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).FirstOrDefault();
                    if (vPhienHopInfo != null)
                    {
                        //Set span trạng thái
                        if (vPhienHopInfo.TRANGTHAI == 2 || vPhienHopInfo.TRANGTHAI == 4 || vPhienHopInfo.TRANGTHAI == 5)
                        {
                            spanSetTrangThai.Visible = true;
                            if (vPhienHopInfo.TRANGTHAI == 2)
                            {

                                btnCongBo.CssClass += " activeTrangThai";
                            }
                            if (vPhienHopInfo.TRANGTHAI == 4)
                            {

                                btnDangHop.CssClass += " activeTrangThai";
                            }
                            if (vPhienHopInfo.TRANGTHAI == 5)
                            {

                                btnDaHop.CssClass += " activeTrangThai";
                            }
                        }

                        if (!String.IsNullOrEmpty(vPhienHopInfo.GHICHU))
                        {
                            divGhiChu.Visible =true;
                            labelGhiChu.Text = vPhienHopInfo.GHICHU;
                        }

                        string vThoiGianBatDau = String.Format("{0:hh:mm dd/MM/yyyy}", vPhienHopInfo.THOIGIANBATDAU);
                        string vThoiGianKetThuc = vPhienHopInfo.THOIGIANKETTHUC != null ? (" - " + String.Format("{0:hh:mm dd/MM/yyyy}", vPhienHopInfo.THOIGIANKETTHUC)) : "";

                        textThoiGian.Text = vThoiGianBatDau + vThoiGianKetThuc;
                        textDiaDiem.Text = vDuyetPhienHopControllerInfo.GetPhongHop(pPhienHopId);
                        var vChuTriInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).ToList();
                        if (vChuTriInfos.Count > 0)
                        {
                            string vContentChuTri = "";
                            foreach (var vChuTriInfo in vChuTriInfos)
                            {
                                vContentChuTri += vChuTriInfo.NGUOIDUNG.TENNGUOIDUNG + " - " + vChuTriInfo.NGUOIDUNG.CHUCVU.TENCHUCVU + " " + vChuTriInfo.NGUOIDUNG.DONVI.TENDONVI + "\n";
                            }
                            textChuTri.Text = vContentChuTri;
                        }

                        textChuongTrinhHop.Text = vPhienHopInfo.NOIDUNG.Replace("\n", "<br/>");//Server.HtmlDecode(vPhienHopInfo.NOIDUNG);

                        textDonVi.Text = vPhienHopInfo.DONVI.TENDONVI;
                        //Đại biểu
                        var vPhienHopNguoiDungInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU != (int)CommonEnum.LoaiDaiBieu.KhachMoi).Select(x => new
                        {
                            x.LOAI_DAIBIEU,
                            x.NGUOIDUNG_ID,
                            x.PHIENHOP_ID,
                            x.NGUOIDUNG.TENNGUOIDUNG,
                            x.NGUOIDUNG.DONVI.TENDONVI,
                            x.NGUOIDUNG.CHUCVU.TENCHUCVU,
                            x.THAMDU,
                            x.VANGMAT,
                            x.XACNHANTHAMGIA,
                        }).OrderBy(x => x.LOAI_DAIBIEU).ToList();

                        int vTongSoDaiBieu = vPhienHopNguoiDungInfos.Count;
                        int vSoLuongDaiBieuThamDu = vPhienHopNguoiDungInfos.Where(x => x.THAMDU == true).Count();
                        int vSoLuongDaiBieuVangMat = vTongSoDaiBieu - vSoLuongDaiBieuThamDu;

                        textTongSoDaiBieu.Text = vTongSoDaiBieu.ToString();
                        textSoLuongDaiBieuThamDu.Text = vSoLuongDaiBieuThamDu.ToString();
                        textSoLuongDaiBieuVangMat.Text = vSoLuongDaiBieuVangMat.ToString();


                        int vChuTriId = 0;
                        vChuTriId = vPhienHopNguoiDungInfos.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).FirstOrDefault().NGUOIDUNG_ID;


                        int vThuKyId = 0;
                        vThuKyId = vPhienHopNguoiDungInfos.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).FirstOrDefault().NGUOIDUNG_ID;

                        var vPhienHopDaiBieuInfo = vPhienHopNguoiDungInfos.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).ToList().Select(x => x.NGUOIDUNG_ID.ToString());


                        if (vPhienHopNguoiDungInfos.Count > 0)
                        {
                            dgDanhSach.DataSource = vPhienHopNguoiDungInfos;
                            dgDanhSach.DataBind();
                        }

                        //Khách mời
                        var vKhachMoiInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).Select(x => new
                        {
                            KHACHMOI = x.NGUOIDUNG.TENNGUOIDUNG + " - " + x.NGUOIDUNG.CHUCVU.TENCHUCVU + " " + x.NGUOIDUNG.DONVI.TENDONVI,
                            x.THAMDU
                        }).ToList();

                        int vTongSoKhachMoi = vKhachMoiInfos.Count();
                        int vSoLuongKhachMoiThamGia = vKhachMoiInfos.Where(x => x.THAMDU == true).Count();
                        int vSoLuongKhachMoiVangMat = vTongSoKhachMoi - vSoLuongKhachMoiThamGia;

                        textTongSoKhachMoi.Text = vTongSoKhachMoi.ToString();
                        textSoLuongKhachMoiThamDu.Text = vSoLuongKhachMoiThamGia.ToString();
                        textSoLuongKhachMoiVangMat.Text = vSoLuongKhachMoiVangMat.ToString();

                        if (vKhachMoiInfos.Count > 0)
                        {
                            dgDanhSachKhachMoi.DataSource = vKhachMoiInfos;
                            dgDanhSachKhachMoi.DataBind();
                        }

                        var vPhienHopKhachMoiInfo = vDataContext.PHIENHOP_KHACHMOIs.Where(x => x.PHIENHOP_ID == pPhienHopId).ToList().Select(x => x.KHACHMOI_ID.ToString());
                        //Tab danh sách phát biểu
                        var vPhatBieuInfos = vDataContext.PHATBIEUs.Where(x => x.PHIENHOP_ID == pPhienHopId).ToList();
                        //vPhatBieuInfos.First().THOIGIANDANGKY;
                        if (vPhatBieuInfos.Count > 0)
                        {
                            dgDanhSachPhatBieu.DataSource = vPhatBieuInfos;
                            dgDanhSachPhatBieu.DataBind();
                            lblPhatBieu.Visible = false;
                        }
                        else
                        {
                            lblPhatBieu.Visible = true;
                        }

                        //Tab biểu quyết
                        string vBieuQuyetHTML = "";
                        var vBieuQuyetInfos = vDataContext.BIEUQUYETs.Where(x => x.PHIENHOP_ID == pPhienHopId).ToList();
                        if (vBieuQuyetInfos.Count > 0)
                        {
                            //Danh sách biểu quyết
                            foreach (var vBieuQuyetInfo in vBieuQuyetInfos)
                            {
                                vBieuQuyetHTML += "<div class='panel panel-white panel-collapse-heading'>";
                                vBieuQuyetHTML += "<div class='panel-heading pd-8' role='tab' id='heading'>";
                                string vCollapeName = "collapse" + vBieuQuyetInfo.BIEUQUYET_ID.ToString();
                                vBieuQuyetHTML += "<a role='button' tabindex='-1' data-toggle='collapse' href='#" + vCollapeName + "' id='tt' aria-expanded='false' aria-controls='" + vCollapeName + "' class='btnCollapse'>";
                                vBieuQuyetHTML += "<h4 class='panel-title'>" + vBieuQuyetInfo.NOIDUNGBIEUQUYET + "</h4>";
                                vBieuQuyetHTML += "</a>";
                                vBieuQuyetHTML += "</div>";
                                vBieuQuyetHTML += "</div>";

                                vBieuQuyetHTML += "<div id='" + vCollapeName + "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='heading2' aria-expanded='false'>";
                                vBieuQuyetHTML += " <div class='form-group pd-l10'>";

                                var vCauHoiInfos = vDataContext.CAUHOIs.Where(x => x.BIEUQUYET_ID == vBieuQuyetInfo.BIEUQUYET_ID).ToList();
                                if (vCauHoiInfos.Count > 0)
                                {
                                    //Danh sách câu hỏi
                                    foreach (var vCauHoiInfo in vCauHoiInfos)
                                    {
                                        vBieuQuyetHTML += "<div class='form-group mr-l10 mr-r20 pd-t10 pd-b10' style='border: 1px solid #f1f1f1'>";
                                        vBieuQuyetHTML += "<div class='col-sm-6'>";
                                        vBieuQuyetHTML += "<label class='control-label pd-r0'>" + vCauHoiInfo.NOIDUNG + "</label>";
                                        vBieuQuyetHTML += "</div>";

                                        var vDapAnInfos = vDataContext.DAPANCAUHOIs.Where(x => x.CAUHOI_ID == vCauHoiInfo.CAUHOI_ID).ToList();
                                        if (vDapAnInfos.Count > 0)
                                        {
                                            //Danh sách đáp án
                                            vBieuQuyetHTML += "<div class='col-sm-6'>";
                                            var vDapAnBieuQuyetInfos = vDataContext.DAIBIEU_BIEUQUYETs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.DAPANCAUHOI.CAUHOI_ID == vCauHoiInfo.CAUHOI_ID).ToList();
                                            double vTongSoDaiBieuThamGiaBieuQuyet = vDapAnBieuQuyetInfos.Count();
                                            if (vTongSoDaiBieuThamGiaBieuQuyet > 0)
                                            {
                                                foreach (var vDapAnInfo in vDapAnInfos)
                                                {
                                                    float vSoNguoiChon = vDapAnBieuQuyetInfos.Where(x => x.DAPANTRALOI_ID == vDapAnInfo.DAPANCAUHOI_ID).Count();
                                                    double vTiLeChon = Math.Round((vSoNguoiChon / vTongSoDaiBieuThamGiaBieuQuyet) * 100);
                                                    vBieuQuyetHTML += "<div class='col-sm-12 form-group'>";
                                                    vBieuQuyetHTML += "<a data='" + vDapAnInfo.DAPANCAUHOI_ID + "' onclick='return HienThiDanhSachNguoiChon(this);'>";
                                                    vBieuQuyetHTML += "<label class='pd-r0 col-sm-6' style='font-weight: normal'>" + vDapAnInfo.NOIDUNG + " (" + vSoNguoiChon + "/" + vTongSoDaiBieuThamGiaBieuQuyet + ") " + "</label>";
                                                    vBieuQuyetHTML += "<div class='progress col-sm-6 pd-l0 pd-r0'>";
                                                    vBieuQuyetHTML += " <div class='progress-bar' role='progressbar' aria-valuenow='" + vTiLeChon + "' aria-valuemin='0' aria-valuemax='100' style='min-width:2em; width: " + vTiLeChon + "%;'>";
                                                    vBieuQuyetHTML += vTiLeChon + "% ";
                                                    vBieuQuyetHTML += "</div>";
                                                    vBieuQuyetHTML += "</div>";
                                                    vBieuQuyetHTML += "</a>";
                                                    vBieuQuyetHTML += "</div>";
                                                }
                                            }
                                            else
                                            {
                                                foreach (var vDapAnInfo in vDapAnInfos)
                                                {
                                                    vBieuQuyetHTML += "<div class='progress'>";
                                                    vBieuQuyetHTML += " <div class='progress-bar pd-l10' role='progressbar' aria-valuenow='0' min-width='20' aria-valuemin='0' aria-valuemax='100' style='width: 100%; text-align:left'>";
                                                    vBieuQuyetHTML += "&nbsp; " + vDapAnInfo.NOIDUNG + " -  Chưa biểu quyết";
                                                    vBieuQuyetHTML += "</div>";
                                                    vBieuQuyetHTML += "</div>";
                                                }
                                            }
                                            vBieuQuyetHTML += "</div>";
                                        }
                                        else
                                        {
                                            vBieuQuyetHTML += "Câu hỏi chưa có đáp án";
                                        }
                                        vBieuQuyetHTML += "</div>";
                                    }
                                }
                                else
                                {
                                    vBieuQuyetHTML += "Biểu quyết chưa có câu hỏi";
                                }
                                vBieuQuyetHTML += "</div>";
                                vBieuQuyetHTML += "</div>";
                            }
                            textNoiDungBieuQuyet.Text = vBieuQuyetHTML;
                        }
                        else
                        {
                            vBieuQuyetHTML = "Phiên họp không có biểu quyết";
                        }
                        //End tab biểu quyết

                        //if (vPhienHopInfo.TRANGTHAI != 4 && vPhienHopInfo.TRANGTHAI != 5)
                        //{
                        //    liBienBanHop.Visible = false;
                        //    liKetLuan.Visible = false;
                        //}
                    }
                }
            }
            catch (Exception Ex)
            {

            }
        }


        /// <summary>
        /// Set trạng thái visible form
        /// </summary>
        /// <param name="pEnableStatus"></param>
        public void SetEnableForm(bool pEnableStatus)
        {
            //textTenKhachMoi.Enabled = pEnableStatus;
            //textEmail.Enabled = pEnableStatus;
            //textSoDienThoai.Enabled = pEnableStatus;
            //ddlistChucVu.Enabled = pEnableStatus;
            //ddlistDonVi.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Check Account Login
        /// </summary>
        public void CheckAccountLogin()
        {
            //KT account quan tri dang nhap
            if (ModulePermissionController.CanAdminModule(this.ModuleConfiguration) == false)
            {
                Response.Redirect(Globals.NavigateURL(), false);
            }
        }

        /// <summary>
        /// Show Message
        /// </summary>
        public void ShowMessage()
        {
            //KT thong bao
            if (!String.IsNullOrEmpty(Session[vMacAddress + TabId.ToString() + "_Message"] as string) && !String.IsNullOrEmpty(Session[vMacAddress + TabId.ToString() + "_Type"] as string))
            {
                if (Session[vMacAddress + TabId.ToString() + "_Message"].ToString() != "" && Session[vMacAddress + TabId.ToString() + "_Type"].ToString() != "")
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, Session[vMacAddress + TabId.ToString() + "_Message"].ToString(), "Thông báo", Session[vMacAddress + TabId.ToString() + "_Type"].ToString());
                }
                Session[vMacAddress + TabId.ToString() + "_Message"] = "";
                Session[vMacAddress + TabId.ToString() + "_Type"] = "";
            }
        }


        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới phiên họp", "id=0");
            Response.Redirect(vUrl);
        }

        protected void btnSua_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin kết luận", "id=" + vPhienHopId);
            Response.Redirect(vUrl);
        }
        #endregion

        #region Xử lý file

        public void LoadDSFile(int pId = 0)
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("TENTAILIEU");
            dtTable.Columns.Add("TRANGTHAI");
            dtTable.Columns.Add("MOTA");
            dtTable.Columns.Add("DOMAT");
            dtTable.Columns.Add("NHOM");
            dtTable.Columns.Add("QUYEN");
            dtTable.Columns.Add("HA_FILE_PATH");
            dtTable.Columns.Add("HA_ID");
            dtTable.Columns.Add("HA_TENFILE");
            dtTable.Columns.Add("HA_EXT");
            dtTable.Columns.Add("HA_SIZE");
            if (pId != 0)
            {
                var temp = (from p in vDataContext.TAILIEUs
                            where p.OBJECT_ID == pId && p.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuHop
                            orderby p.TAILIEU_ID descending
                            select p).ToList();

                var vQuyenInfos = vDataContext.QUYENs.ToList();
                if (temp.Count > 0)
                {
                    foreach (var it in temp)
                    {
                        DataRow row = dtTable.NewRow();
                        row["TENTAILIEU"] = (it.TEN);
                        row["TRANGTHAI"] = (it.TRANGTHAI);
                        row["MOTA"] = (it.MOTA);
                        row["DOMAT"] = it.DOMAT;
                        row["NHOM"] = (it.TL_NHOM);
                        string vQuyenXemStr = "";
                        if (it.QUYEN_TAILIEUs.Count > 0)
                        {
                            if (it.QUYEN_TAILIEUs.Count >= 4)
                            {
                                vQuyenXemStr = "Tất cả, ";
                            }
                            else
                            {
                                foreach (var vQuyenXem in it.QUYEN_TAILIEUs)
                                {
                                    var vQuyen = vQuyenInfos.Where(x => x.QUYEN_ID == vQuyenXem.QUYEN_ID).Select(x => x.QUYEN_TEN).FirstOrDefault();
                                    if (!String.IsNullOrEmpty(vQuyen))
                                    {
                                        vQuyenXemStr += vQuyen + ", ";
                                    }
                                }
                            }

                            vQuyenXemStr = vQuyenXemStr.Substring(0, vQuyenXemStr.Length - 2);
                            row["QUYEN"] = vQuyenXemStr;
                        }
                        row["HA_FILE_PATH"] = (it.FILE_NAME);
                        row["HA_ID"] = (it.TAILIEU_ID);
                        row["HA_TENFILE"] = it.FILE_MOTA;
                        row["HA_EXT"] = it.FILE_EXT;
                        row["HA_SIZE"] = it.FILE_SIZE;
                        dtTable.Rows.Add(row);
                    }
                }
            }
            if (dtTable.Rows.Count > 0)
            {
                dgDanhSach_File.DataSource = dtTable;
                dgDanhSach_File.DataBind();
                lblDanhSachFile.Visible = false;
            }
            else
            {
                lblDanhSachFile.Visible = true;
            }         
        }

        /// <summary>
        /// Thay đổi trạng thái sử dụng thiết bị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThayDoiDoMat(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vTaiLieuId = int.Parse(html.HRef);
                TAILIEU vTaiLieuInfo = new TAILIEU();
                string oErrorMessage = "";
                vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault(); ;
                if (vTaiLieuInfo != null)
                {
                    if (vTaiLieuInfo.DOMAT ?? true)
                    {
                        vTaiLieuInfo.DOMAT = false;
                    }
                    else if (!vTaiLieuInfo.DOMAT ?? false)
                    {
                        vTaiLieuInfo.DOMAT = true;
                    }
                    vDataContext.SubmitChanges();
                    ClassCommon.ShowToastr(Page, "Cập nhật độ mật của tài liệu: " + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
                    callModalScript();
                    LoadDSFile(vPhienHopId);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }



        /// <summary>
        /// Xóa file khỏi danh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaFileKhoiDanhSach(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            TableCell cell = dgDanhSach_File.Rows[e.RowIndex].Cells[0];
            string vFile_Name = dgDanhSach_File.DataKeys[e.RowIndex]["HA_FILE_PATH"].ToString();
            File.Delete(Server.MapPath(vPathCommonUploadFile) + "/" + vFile_Name);
            int vFile_ID = int.Parse(dgDanhSach_File.DataKeys[e.RowIndex].Value.ToString());
            var vTapTinInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).SingleOrDefault();

            if (vTapTinInfo != null)
            {
                vDataContext.TAILIEUs.DeleteOnSubmit(vTapTinInfo);
                vDataContext.SubmitChanges();
                LoadDSFile(vPhienHopId);
            }
            callModalScript();            
        }
        /// <summary>
        /// Binding dữ liệu file
        /// </summary>
        protected void BindGrid_File()
        {
            dgDanhSach_File.DataSource = Session["File" + vMacAddress + vPhienHopId];
            dgDanhSach_File.DataBind();
        }


        protected void btn_TL_Click(object sender, EventArgs e)
        {
            //if (dgDanhSach.Rows.Count < 1)
            //{
            if (textTenTaiLieu.Text.Trim() == "")
            {
                ClassCommon.ShowToastr(Page, "Vui lòng nhập tên tài liệu", "Thông báo lỗi", "error");

                textTenTaiLieu.CssClass += " vld";
                textTenTaiLieu.Focus();
                labelTenTaiLieu.Attributes["class"] += " vld";
            }
            else
            {
                textTenTaiLieu.CssClass = textTenTaiLieu.CssClass.Replace("vld", "").Trim();
                labelTenTaiLieu.Attributes.Add("class", labelTenTaiLieu.Attributes["class"].ToString().Replace("vld", ""));
                if (!f_file.HasFile)
                {
                    ClassCommon.ShowToastr(Page, "Vui lòng chọn tài liệu", "Thông báo lỗi", "error");
                    f_file.CssClass += " vld";
                    f_file.Focus();
                    labelFile.Attributes["class"] += " vld";
                }
                else
                {
                    f_file.CssClass = f_file.CssClass.Replace("vld", "").Trim();
                    labelFile.Attributes.Add("class", labelFile.Attributes["class"].ToString().Replace("vld", ""));
                    string filepath = Server.MapPath(vPathCommonUploadFile);
                    HttpFileCollection uploadedFiles = Request.Files;
                    for (int i = 0; i < uploadedFiles.Count; i++)
                    {
                        HttpPostedFile userPostedFile = uploadedFiles[i];
                        try
                        {
                            //File cho phép: .jpg, jpng, .jpeg, .doc, .docx, .xls, .xlsx, .zip
                            if (userPostedFile.ContentType == "image/jpg" || userPostedFile.ContentType == "image/png" || userPostedFile.ContentType == "image/jpeg" || userPostedFile.ContentType == "application/msword" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || userPostedFile.ContentType == "application/pdf" || userPostedFile.ContentType == "application/vnd.ms-excel" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || userPostedFile.ContentType == "application/x-zip-compressed" || userPostedFile.ContentType == "application/octet-stream")
                            {
                                if (userPostedFile.ContentLength < 1048576 * 100)//100MB
                                {
                                    string filename = userPostedFile.FileName;
                                    string extension = System.IO.Path.GetExtension(filename);
                                    //string result = ClassCommon.GetGuid() + extension;
                                    //string result = filename + extension;
                                    ClassCommon.UploadFile(userPostedFile, filepath, filename, "");

                                    TAILIEU vTaiLieuInfo = new TAILIEU();
                                    vTaiLieuInfo.FILE_NAME = filename;
                                    vTaiLieuInfo.FILE_MOTA = filename.Substring(0, filename.Length - extension.Length);
                                    vTaiLieuInfo.FILE_EXT = extension;
                                    vTaiLieuInfo.FILE_SIZE = Int32.Parse(userPostedFile.ContentLength.ToString());
                                    vTaiLieuInfo.NGAYCAPNHAT = DateTime.Now;
                                    vTaiLieuInfo.NGAYTAO = DateTime.Now;
                                    vTaiLieuInfo.OBJECT_LOAI = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                    vTaiLieuInfo.OBJECT_ID = vPhienHopId;

                                    vTaiLieuInfo.PHIENHOP_ID = vPhienHopId;
                                    vTaiLieuInfo.TEN = ClassCommon.ClearHTML(textTenTaiLieu.Text.Trim());
                                    vTaiLieuInfo.MOTA = ClassCommon.ClearHTML(textMotaFile.Text.Trim());
                                    vTaiLieuInfo.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                    vTaiLieuInfo.TRANGTHAI = 0;
                                    vTaiLieuInfo.TAILIEUCHUNG = true;
                                    vTaiLieuInfo.DOMAT = false;
                                    vTaiLieuInfo.UserId = _currentUser.UserID;

                                    vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                                    vDataContext.SubmitChanges();
                                    textTenTaiLieu.Text = "";
                                    textMotaFile.Text = "";

                                    LoadDSFile(vPhienHopId);
                                }
                                else
                                {
                                    ClassCommon.ShowToastr(Page, "Vui lòng chọn file có kích thước file nhỏ hơn 100 MB", "Thông báo lỗi", "error");
                                }
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng chọn file đúng định dạng", "Thông báo lỗi", "error");
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            callModalScript();

        }
        public void callModalScript()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay();", true);
        }

        public void Reload_TabBieuQuyet()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "ReloadTabBieuQuyet", "Reload_TabBieuQuyet();", true);
        }

        public void OpenModalBieuQuyet()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "Reload", "OpenModalBieuQuyet();", true);
        }
        #endregion


        #region Xử lý file KetLuan

        public void LoadDSFile_KetLuan(int pId = 0)
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("TENTAILIEU");
            dtTable.Columns.Add("TRANGTHAI");
            dtTable.Columns.Add("MOTA");
            dtTable.Columns.Add("DOMAT");
            dtTable.Columns.Add("HA_FILE_PATH");
            dtTable.Columns.Add("HA_ID");
            dtTable.Columns.Add("HA_TENFILE");
            dtTable.Columns.Add("HA_EXT");
            dtTable.Columns.Add("HA_SIZE");
            if (pId != 0)
            {
                var temp = (from p in vDataContext.TAILIEUs
                            where p.OBJECT_ID == pId && p.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuKetLuan
                            orderby p.TAILIEU_ID descending
                            select p).ToList();

                if (temp.Count > 0)
                {
                    foreach (var it in temp)
                    {
                        DataRow row = dtTable.NewRow();
                        row["TENTAILIEU"] = (it.TEN);
                        row["TRANGTHAI"] = (it.TRANGTHAI);
                        row["MOTA"] = (it.MOTA);
                        row["DOMAT"] = (it.DOMAT == true ? "Mật" : "Không");
                        row["HA_FILE_PATH"] = (it.FILE_NAME);
                        row["HA_ID"] = (it.TAILIEU_ID);
                        row["HA_TENFILE"] = it.FILE_MOTA;
                        row["HA_EXT"] = it.FILE_EXT;
                        row["HA_SIZE"] = it.FILE_SIZE;
                        dtTable.Rows.Add(row);
                    }
                }
            }
            if (dtTable.Rows.Count >0)
            {
                dgDanhSach_FileKetLuan.DataSource = dtTable;
                dgDanhSach_FileKetLuan.DataBind();
                lblKetLuan.Visible = false;
            }
            else
            {
                lblKetLuan.Visible = true;
            }        
        }

        /// <summary>
        /// Thay đổi trạng thái sử dụng thiết bị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThayDoiTrangThai_KetLuan(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vTaiLieuId = int.Parse(html.HRef);
                TAILIEU vTaiLieuInfo = new TAILIEU();
                string oErrorMessage = "";
                string vMessage = "";
                vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault(); ;
                if (vTaiLieuInfo != null)
                {
                    if (vTaiLieuInfo.TRANGTHAI == 0)
                    {
                        vTaiLieuInfo.TRANGTHAI = 1;
                        vMessage = "Duyệt tài liệu: ";
                    }
                    else if (vTaiLieuInfo.TRANGTHAI == 1)
                    {
                        vTaiLieuInfo.TRANGTHAI = 0;
                        vMessage = "Bỏ duyệt tài liệu: ";
                    }
                    vDataContext.SubmitChanges();
                    ClassCommon.ShowToastr(Page, vMessage + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
                    callModalScript_KetLuan();
                    LoadDSFile_KetLuan(vPhienHopId);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Xóa file khỏi danh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaFileKhoiDanhSach_KetLuan(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            TableCell cell = dgDanhSach_FileKetLuan.Rows[e.RowIndex].Cells[0];
            string vFile_Name = dgDanhSach_FileKetLuan.DataKeys[e.RowIndex]["HA_FILE_PATH"].ToString();
            File.Delete(Server.MapPath(vPathCommonUploadFileKetLuan) + "/" + vFile_Name);
            int vFile_ID = int.Parse(dgDanhSach_FileKetLuan.DataKeys[e.RowIndex].Value.ToString());
            var vTapTinInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).SingleOrDefault();

            if (vTapTinInfo != null)
            {
                vDataContext.TAILIEUs.DeleteOnSubmit(vTapTinInfo);
                vDataContext.SubmitChanges();
                LoadDSFile_KetLuan(vPhienHopId);
            }
            callModalScript_KetLuan();
            ClassCommon.ShowToastr(Page, "Xoá tài liệu kết luận cuộc họp thành công", "Thông báo", "success");
        }
        /// <summary>
        /// Binding dữ liệu file
        /// </summary>
        protected void BindGrid_File_KetLuan()
        {
            dgDanhSach_FileKetLuan.DataSource = Session["FileKetLuan" + vMacAddress + vPhienHopId];
            dgDanhSach_FileKetLuan.DataBind();
        }



        public void callModalScript_KetLuan()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay_KetLuan();", true);
        }

        #endregion

        #region Xử lý file BienBan

        public void LoadDSFile_BIENBAN(int pId = 0)
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("TENTAILIEU");
            dtTable.Columns.Add("TRANGTHAI");
            dtTable.Columns.Add("MOTA");
            dtTable.Columns.Add("DOMAT");
            dtTable.Columns.Add("HA_FILE_PATH");
            dtTable.Columns.Add("HA_ID");
            dtTable.Columns.Add("HA_TENFILE");
            dtTable.Columns.Add("HA_EXT");
            dtTable.Columns.Add("HA_SIZE");
            if (pId != 0)
            {
                var temp = (from p in vDataContext.TAILIEUs
                            where p.OBJECT_ID == pId && p.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuBienBanHop
                            orderby p.TAILIEU_ID descending
                            select p).ToList();

                if (temp.Count > 0)
                {
                    foreach (var it in temp)
                    {
                        DataRow row = dtTable.NewRow();
                        row["TENTAILIEU"] = (it.TEN);
                        row["TRANGTHAI"] = (it.TRANGTHAI);
                        row["MOTA"] = (it.MOTA);
                        row["DOMAT"] = (it.DOMAT == true ? "Mật" : "Không");
                        row["HA_FILE_PATH"] = (it.FILE_NAME);
                        row["HA_ID"] = (it.TAILIEU_ID);
                        row["HA_TENFILE"] = it.FILE_MOTA;
                        row["HA_EXT"] = it.FILE_EXT;
                        row["HA_SIZE"] = it.FILE_SIZE;
                        dtTable.Rows.Add(row);
                    }
                }
            }
            if(dtTable.Rows.Count > 0)
            {
                dgDanhSach_FileBienBan.DataSource = dtTable;
                dgDanhSach_FileBienBan.DataBind();
                lblPhienBanHop.Visible = false;
            }
            else
            {
                lblPhienBanHop.Visible = true;
            }          
        }

        /// <summary>
        /// Thay đổi trạng thái sử dụng thiết bị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThayDoiTrangThai_BienBan(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vTaiLieuId = int.Parse(html.HRef);
                TAILIEU vTaiLieuInfo = new TAILIEU();
                string oErrorMessage = "";
                string vMessage = "";
                vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault(); ;
                if (vTaiLieuInfo != null)
                {
                    if (vTaiLieuInfo.TRANGTHAI == 0)
                    {
                        vTaiLieuInfo.TRANGTHAI = 1;
                        vMessage = "Duyệt tài liệu: ";
                    }
                    else if (vTaiLieuInfo.TRANGTHAI == 1)
                    {
                        vTaiLieuInfo.TRANGTHAI = 0;
                        vMessage = "Bỏ duyệt tài liệu: ";
                    }
                    vDataContext.SubmitChanges();
                    ClassCommon.ShowToastr(Page, vMessage + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
                    callModalScript_BienBan();
                    LoadDSFile_BIENBAN(vPhienHopId);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Xóa file khỏi danh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaFileKhoiDanhSach_BienBan(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            TableCell cell = dgDanhSach_FileBienBan.Rows[e.RowIndex].Cells[0];
            string vFile_Name = dgDanhSach_FileBienBan.DataKeys[e.RowIndex]["HA_FILE_PATH"].ToString();
            File.Delete(Server.MapPath(vPathCommonUploadFileBienBan) + "/" + vFile_Name);
            int vFile_ID = int.Parse(dgDanhSach_FileBienBan.DataKeys[e.RowIndex].Value.ToString());
            var vTapTinInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).SingleOrDefault();

            if (vTapTinInfo != null)
            {
                vDataContext.TAILIEUs.DeleteOnSubmit(vTapTinInfo);
                vDataContext.SubmitChanges();
                LoadDSFile_BIENBAN(vPhienHopId);
            }
            callModalScript_BienBan();
            ClassCommon.ShowToastr(Page, "Xoá biên bản họp thành công", "Thông báo", "success");
        }
        /// <summary>
        /// Binding dữ liệu file
        /// </summary>
        protected void BindGrid_File_BienBan()
        {
            dgDanhSach_FileBienBan.DataSource = Session["FileBienBan" + vMacAddress + vPhienHopId];
            dgDanhSach_FileBienBan.DataBind();
        }



        public void callModalScript_BienBan()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay_BienBan();", true);
        }

        #endregion

        #region Sơ đồ vị trí
        /// <summary>
        ///  Set thông tin sơ đồ khi chọn ghế
        /// </summary>
        /// <param name="pPhienHopId"></param>
        /// <param name="pCapNhatFile">true: Cập nhật file phòng họp;</param>
        public void SetInfoSoDo(int pPhienHopId)
        {
            try
            {
                SoDoPhongHopInfo objSoDo = (from a in vDataContext.PHIENHOP_PHONGHOPs
                                            where a.PHIENHOP_ID == pPhienHopId
                                            join phong in vDataContext.PHONGHOPs on a.PHONGHOP_ID equals phong.PHONGHOP_ID
                                            select new SoDoPhongHopInfo()
                                            {
                                                PHIENHOP_ID = a.PHIENHOP_ID,
                                                PHONGHOP_ID = a.PHONGHOP_ID,
                                                SODO_FILE = a.SODO_FILE,
                                                SODO_TEXT = a.SODO_Text == null ? "" : Server.HtmlDecode(a.SODO_Text).Replace("\n", ""),
                                            }).FirstOrDefault();

                if (objSoDo != null)
                {
                    //Tìm danh sách phiên họp vị trí
                    var objViTris = vDataContext.PRO_PHIENHOP_VITRI_WEB(pPhienHopId).ToList();


                    string vSource = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\" + objSoDo.SODO_FILE;
                    string vDes = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg?cache=" + new Random().Next(100000000,999999999);
                    if (File.Exists(vDes))
                    {
                        //lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg" + "\" type=\"image/svg+xml\" width=\"600\">";
                        lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\" + objSoDo.SODO_FILE  + "?cache="+ new Random().Next(100000000, 999999999) + "\" type=\"image/svg+xml\" style='width: 100%'>";
                    }
                    else
                    {
                        lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg?cache="+  new Random().Next(100000000,999999999) + "\" type=\"image/svg+xml\" style='width: 70%'>";
                        //lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\" + objSoDo.SODO_FILE  + "\" type=\"image/svg+xml\" width=\"600\">";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Điểm danh


        protected void LoadDanhSach_DiemDanh(int pCurentPage)
        {
            try
            {
                string vContentSearch = textSearchContent.Text.Trim().ToLower();
                if (vContentSearch == "khách mời")
                {
                    vContentSearch = "khachmoi";
                }

                if (vContentSearch == "đại biểu")
                {
                    vContentSearch = "daibieu";
                }

                string vErrorMessage = "";

                if (Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] != null)
                {
                    Dictionary<string, string> vDictSearch = (Dictionary<string, string>)Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"];
                    if (ClassCommon.ExistKey("KeyWord", vDictSearch))
                    {
                        vContentSearch = vDictSearch["KeyWord"].ToLower();
                    }
                }

                var vDiemDanhInfo = vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId).ToList();
                vDiemDanhInfo = vDiemDanhInfo.Where(x => (x.TEN.ToLower().Contains(vContentSearch) || 
                (x.NGUOIDUTHAY != null ? x.NGUOIDUTHAY.ToLower().Contains(vContentSearch) : false) ||
                x.TENDONVI.ToLower().Contains(vContentSearch) ||
                x.TENCHUCVU.ToLower().Contains(vContentSearch) || 
                x.LOAI.ToLower().Contains(vContentSearch))).ToList();
                vDiemDanhInfo = vDiemDanhInfo.OrderBy(x => x.LOAI).ThenBy(y => y.TEN).ToList();
                if (ViewState["keysort_diemdanh"] != null && ViewState["typesort_diemdanh"] != null)
                {
                    string key = ViewState["keysort_diemdanh"].ToString();
                    string type = ViewState["typesort_diemdanh"].ToString();

                    if (key == "TEN" && type == "ASC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderBy(x => x.TEN).ToList();
                    }

                    if (key == "TEN" && type == "DESC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderByDescending(x => x.TEN).ToList();
                    }

                    if (key == "DONVI" && type == "ASC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderBy(x => x.TENDONVI).ToList();
                    }

                    if (key == "DONVI" && type == "DESC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderByDescending(x => x.TENDONVI).ToList();
                    }
                    if (key == "CHUCVU" && type == "ASC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderBy(x => x.TENCHUCVU).ToList();
                    }

                    if (key == "CHUCVU" && type == "DESC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderByDescending(x => x.TENCHUCVU).ToList();
                    }
                    if (key == "LOAI" && type == "ASC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderBy(x => x.LOAI).ToList();
                    }

                    if (key == "LOAI" && type == "DESC")
                    {
                        vDiemDanhInfo = vDiemDanhInfo.OrderByDescending(x => x.LOAI).ToList();
                    }
                }
                int Count = vDiemDanhInfo.Count;
                dgDanhSach_DiemDanh.DataSource = vDiemDanhInfo;
                dgDanhSach_DiemDanh.VirtualItemCount = Count;
                dgDanhSach_DiemDanh.PageSize = vPageSize;
                dgDanhSach_DiemDanh.CurrentPageIndex = pCurentPage;
                dgDanhSach_DiemDanh.DataBind();
              
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ trị", "Thông báo lỗi", "error");
            }
        }

        


        /// <summary>
        /// Event Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string vSearchContent = ClassCommon.RemoveSpace(textSearchContent.Text.Trim());
            Dictionary<string, string> dictSearch = null;
            //Khoi tao dictionary
            if (vSearchContent != "")
            {
                dictSearch = new Dictionary<string, string>();
                dictSearch.Add("TEN", vSearchContent);

                //dictSearch.Add("PhienHop", vSearch_PhienHopID.ToString());
            }
            if (vSearchContent == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("TEN");
            }
            //if (vSearch_PhienHopID.ToString() == "")
            //{
            //    if (dictSearch != null)
            //        dictSearch.Remove("PhienHop");
            //}
            //Gan danh sach Search into Session
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] = dictSearch;
            LoadDanhSach_DiemDanh(0);
            ViewState["Tab"] = "DIEMDANH";
        }


        protected void DiemDanhNhieuDaiBieu(object sender, EventArgs e)
        {

        }

        protected void dgDanhSach_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortDirection = GetSortDirection(e.SortExpression);
            ViewState["keysort_diemdanh"] = e.SortExpression;
            ViewState["typesort_diemdanh"] = sortDirection;
            LoadDanhSach_DiemDanh(0);
            ViewState["Tab"] = "DIEMDANH";
        }


        private string GetSortDirection(string column)
        {
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }


        protected void dgDanhSach_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                LinkButton btnSort;
                Image image;
                //iterate through all the header cells
                int dem = 0;
                foreach (TableCell cell in e.Item.Cells)
                {
                    dem++;
                    if (dem > 1)
                    {
                        //check if the header cell has any child controls
                        if (cell.HasControls())
                        {
                            //get reference to the button column
                            btnSort = (LinkButton)cell.Controls[0];
                            image = new Image();

                            image.ImageUrl = "/DesktopModules/HOPKHONGGIAY/Images/sort_both.png";

                            cell.Controls.Add(image);

                            if (ViewState["SortExpression"] != null)
                            {
                                //see if the button user clicked on and the sortexpression in the viewstate are same
                                //this check is needed to figure out whether to add the image to this header column nor not
                                if (btnSort.CommandArgument == ViewState["SortExpression"].ToString())
                                {
                                    //following snippet figure out whether to add the up or down arrow
                                    //based on the sortdirection
                                    string temp = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortDirection"].ToString() == "ASC")
                                    {
                                        image.ImageUrl = "/DesktopModules/HOPKHONGGIAY/Images/sort_asc.png";
                                        image.CssClass = "sort_img";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "/DesktopModules/HOPKHONGGIAY/Images/sort_desc.png";
                                        image.CssClass = "sort_img";
                                    }
                                    cell.Controls.Add(image);
                                }
                            }
                        }
                    }
                }
            }
        }



        #region Phương thức phân trang

        LinkButton lbFirstPage = null;
        LinkButton lbPreviousPage = null;
        LinkButton lbNextPage = null;
        LinkButton lbLastPage = null;
        LinkButton lblToltalRecord = null;
        LinkButton lblCurentViewerRecord = null;
        LinkButton lblPageSize = null;
        DropDownList ddlPageSize = null;
        protected void dgDanhSach_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            dgDanhSach_DiemDanh.CurrentPageIndex = e.NewPageIndex;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = Int16.Parse(e.NewPageIndex.ToString());
            vCurentPage = Int16.Parse(e.NewPageIndex.ToString());
            LoadDanhSach_DiemDanh(vCurentPage);
        }
        protected void dgDanhSach_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            Custom_Paging(sender, e, dgDanhSach_DiemDanh.CurrentPageIndex, dgDanhSach_DiemDanh.VirtualItemCount, dgDanhSach_DiemDanh.PageCount);
        }
        protected void dgDanhSach_Init(object sender, EventArgs e)
        {
            lbFirstPage = new LinkButton();
            lbFirstPage.ID = "lbFirstPage";
            lbFirstPage.Text = "<<";
            lbFirstPage.CssClass = "paging_btn btn_first";
            lbFirstPage.Click += new EventHandler(lbFirstPage_Click);

            lbPreviousPage = new LinkButton();
            lbPreviousPage.ID = "lbPreviousPage";
            lbPreviousPage.Text = "<";
            lbPreviousPage.CssClass = "paging_btn btn_previous";
            lbPreviousPage.Click += new EventHandler(lbPreviousPage_Click);

            lbNextPage = new LinkButton();
            lbNextPage.ID = "lbNextPage";
            lbNextPage.Text = ">";
            lbNextPage.CssClass = "paging_btn btn_next";
            lbNextPage.Click += new EventHandler(lbNextPage_Click);

            lbLastPage = new LinkButton();
            lbLastPage.ID = "lbLastPage";
            lbLastPage.Text = ">>";
            lbLastPage.CssClass = "paging_btn btn_last";
            lbLastPage.Click += new EventHandler(lbLastPage_Click);

            lblToltalRecord = new LinkButton();
            lblToltalRecord.ID = "lblToltalRecord";
            lblToltalRecord.CssClass = "paging_label fright";

            lblCurentViewerRecord = new LinkButton();
            lblCurentViewerRecord.ID = "lblZ";
            lblCurentViewerRecord.CssClass = "paging_label fright";

            lblPageSize = new LinkButton();
            lblPageSize.ID = "lblPageSize";
            lblPageSize.CssClass = "fright paging_label";

            ddlPageSize = new DropDownList();
            ddlPageSize.ID = "ddlPageSize";

            //ddlPageSize.CssClass = "fright form-control input-sm ddl_pagesize";
            ddlPageSize.CssClass = "input-sm";
            ddlPageSize.AutoPostBack = true;
            ListItem vPageSize2 = new ListItem("5", "5");
            ListItem vPageSize10 = new ListItem("10", "10");
            ListItem vPageSize20 = new ListItem("20", "20");
            ListItem vPageSize30 = new ListItem("30", "30");
            ListItem vPageSize50 = new ListItem("50", "50");
            ListItem vPageSize100 = new ListItem("100", "100");
            ListItem vPageSize200 = new ListItem("200", "200");
            ListItem vPageSize9999 = new ListItem("Tất cả", "9999");
            ddlPageSize.Items.Add(vPageSize2);
            ddlPageSize.Items.Add(vPageSize10);
            ddlPageSize.Items.Add(vPageSize20);
            ddlPageSize.Items.Add(vPageSize30);
            ddlPageSize.Items.Add(vPageSize50);
            ddlPageSize.Items.Add(vPageSize100);
            ddlPageSize.Items.Add(vPageSize200);
            ddlPageSize.Items.Add(vPageSize9999);

            ddlPageSize.SelectedIndexChanged += DdlPageSize_SelectedIndexChanged;
        }
        void lbLastPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            LoadDanhSach_DiemDanh(dgDanhSach_DiemDanh.PageCount - 1);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach_DiemDanh.PageCount - 1);
        }
        void lbNextPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach_DiemDanh.CurrentPageIndex < (dgDanhSach_DiemDanh.PageCount - 1))
            {
                LoadDanhSach_DiemDanh(dgDanhSach_DiemDanh.CurrentPageIndex + 1);
                Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach_DiemDanh.CurrentPageIndex);
            }
        }
        void lbPreviousPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach_DiemDanh.CurrentPageIndex > 0)
            {
                LoadDanhSach_DiemDanh(dgDanhSach_DiemDanh.CurrentPageIndex - 1);
                Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach_DiemDanh.CurrentPageIndex);
            }
        }
        void lbFirstPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            LoadDanhSach_DiemDanh(0);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = 0;
        }
        void DdlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();

            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = Int16.Parse(ddlPageSize.SelectedValue);
            vPageSize = Int16.Parse(ddlPageSize.SelectedValue);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = 0;
            vCurentPage = 0;
            LoadDanhSach_DiemDanh(vCurentPage);
        }
        protected void Custom_Paging(object sender, DataGridItemEventArgs e, int vCurrentPageIndex, int vVirtualItemCount, int vPageCount)
        {
            if (vCurrentPageIndex == 0)
            {
                lbPreviousPage.Enabled = false;
                lbFirstPage.Enabled = false;
            }
            else
            {
                lbPreviousPage.Enabled = true;
                lbFirstPage.Enabled = true;
            }
            if (vCurrentPageIndex + 1 == vPageCount)
            {
                lbLastPage.Enabled = false;
                lbNextPage.Enabled = false;
            }
            else
            {
                lbLastPage.Enabled = true;
                lbNextPage.Enabled = true;
            }
            if (e.Item.ItemType == ListItemType.Pager)
            {
                e.Item.Cells[0].Text.Replace("&nbsp;", "");
                TableCell Pager = (TableCell)e.Item.Controls[0];
                for (int i = 0; i < Pager.Controls.Count; i++)
                {
                    try
                    {

                        object pgNumbers = Pager.Controls[i];
                        int endPagingIndex = Pager.Controls.Count - 1;
                        string Typea = pgNumbers.GetType().Name;
                        if (pgNumbers.GetType().Name == "DataGridLinkButton")
                        {
                            LinkButton lb = (LinkButton)pgNumbers;
                            lb.CssClass = "paging_item";
                            if (lb.Text == "...")
                            {
                                lb.Visible = false;
                            }
                            if (vPageCount > 5)
                            {
                                if (vCurrentPageIndex >= 2 && vPageCount > (vCurrentPageIndex + 2))
                                {
                                    if (Int32.Parse(lb.Text) > (vCurrentPageIndex + 3))
                                    {
                                        lb.Visible = false;
                                    }
                                    if (Int32.Parse(lb.Text) < (vCurrentPageIndex - 1))
                                    {
                                        lb.Visible = false;
                                    }
                                }
                                else if (vCurrentPageIndex < 2)
                                {
                                    if (Int32.Parse(lb.Text) > 5)
                                    {
                                        lb.Visible = false;
                                    }
                                }
                                else
                                {
                                    if (Int32.Parse(lb.Text) < (vPageCount - 4))
                                    {
                                        lb.Visible = false;
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi xảy ra vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
                        //log.Error("", ex);
                    }

                }
                // add the previous page link
                if (e.Item.Cells[0].FindControl("lbPreviousPage") == null)
                {
                    e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                    e.Item.Cells[0].Controls.AddAt(0, lbPreviousPage);
                }
                if (e.Item.Cells[0].FindControl("lbFirstPage") == null)
                {
                    e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                    e.Item.Cells[0].Controls.AddAt(0, lbFirstPage);
                }
                if (e.Item.Cells[0].FindControl("lbNextPage") == null)
                {
                    e.Item.Cells[0].Controls.Add(new LiteralControl(""));
                    e.Item.Cells[0].Controls.Add(lbNextPage);
                }
                if (e.Item.Cells[0].FindControl("lbLastPage") == null)
                {
                    e.Item.Cells[0].Controls.Add(new LiteralControl(""));
                    e.Item.Cells[0].Controls.Add(lbLastPage);
                }
                if (e.Item.Cells[0].FindControl("lblToltalRecord") == null)
                {
                    e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                    e.Item.Cells[0].Controls.AddAt(0, lbPreviousPage);
                }

                // Add total record in paging

                if (e.Item.Cells[0].FindControl("ddlPageSize") == null)
                {
                    e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                    e.Item.Cells[0].Controls.AddAt(0, lblPageSize);
                    lblPageSize.Text = "Số dòng hiển thị: ";
                }
                if (e.Item.Cells[0].FindControl("ddlPageSize") == null)
                {
                    e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                    ddlPageSize.SelectedValue = vPageSize.ToString();
                    e.Item.Cells[0].Controls.AddAt(0, ddlPageSize);
                }
                if (e.Item.Cells[0].FindControl("lblZ") == null)
                {
                    if (dgDanhSach_DiemDanh.VirtualItemCount != 0)
                    {
                        lblCurentViewerRecord.Text = " " + ((dgDanhSach_DiemDanh.CurrentPageIndex * dgDanhSach_DiemDanh.PageSize) + 1).ToString() + " - " + (dgDanhSach_DiemDanh.CurrentPageIndex + 1 == dgDanhSach_DiemDanh.PageCount ? dgDanhSach_DiemDanh.VirtualItemCount.ToString() : ((dgDanhSach_DiemDanh.CurrentPageIndex + 1) * dgDanhSach_DiemDanh.PageSize).ToString()).ToString();
                        lblCurentViewerRecord.Text += " trong tổng số " + dgDanhSach_DiemDanh.VirtualItemCount + "";
                        e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                        e.Item.Cells[0].Controls.AddAt(0, lblCurentViewerRecord);
                    }
                    else
                    {
                        lblCurentViewerRecord.Text = "Chưa có dữ liệu.";
                        e.Item.Cells[0].Controls.AddAt(0, new LiteralControl(""));
                        e.Item.Cells[0].Controls.AddAt(0, lblCurentViewerRecord);
                    }
                }
            }
        }
        #endregion



        #endregion

        #region Thông báo phiên họp
        protected void buttonGoiThongBao_Click(object sender, EventArgs e)
        {
            try
            {
                string vUrl = Globals.NavigateURL("thongbao", "mid=" + this.ModuleId, "title=Thông báo", "id=" + vPhienHopId);
                Response.Redirect(vUrl);
            } 
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
