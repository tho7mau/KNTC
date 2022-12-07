#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật phiên họp
/// Ngày tại        :08/03/2019
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Collections;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.Data.Extensions;
using Telerik.Web.UI;

namespace HOPKHONGGIAY
{
    public partial class CnPhienHop : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        DataTable dtTable;


        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        public string vPathCommonUploadFile = ClassParameter.vPathCommonUploadTaiLieuHop;

        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();

        PhienHopNguoiDungController vPhienHopNguoiDungControllerInfo = new PhienHopNguoiDungController();
        PhienHopKhachMoiController vPhienHopKhachMoiControllerInfo = new PhienHopKhachMoiController();
        PhongHopController vPhongHopControllerInfo = new PhongHopController();
        DuyetPhienHopController vDuyetPhienHopController = new DuyetPhienHopController();
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
                //Kiem tra quyen dang nhap
                CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vPhienHopId = int.Parse(Request.QueryString["id"]);
                }
                //Edit Title
                if (vPhienHopId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / Thêm mới";
                }
                else
                {
                    var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                    dtpickerThoiGianBatDau_ChonPhong.SelectedDate = vPhienHopInfo.THOIGIANBATDAU;
                    dtpickerThoiGianKetThuc_ChonPhong.SelectedDate = vPhienHopInfo.THOIGIANKETTHUC;
                    textSoNguoi.Text = vDuyetPhienHopController.GetKhachThamDu(vPhienHopId).ToString();
                    if (vPhienHopInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / " + vPhienHopInfo.TIEUDE;
                    }
                }
                if (!IsPostBack)
                {
                    LoadDanhSachDonVi();
                    LoadDropDownThietBi();
                    SetFormInfo(vPhienHopId);

                    if (vPhienHopId == 0)
                    {
                        SetEnableForm(true);
                        LoadDanhSachPhongHop(true);
                    }
                    else
                    {
                        SetEnableForm(false);
                        LoadDanhSachPhongHop(false);
                    }

                    LoadDSFile(vPhienHopId);
                }
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }
        void Page_LoadComplete(object sender, EventArgs e)
        {
            // call your download function
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
            if (ValidateForm() == true)
            {
                LinkButton vButton = sender as LinkButton;
                string vAction = vButton.CommandName;
                bool vGoiDuyet = false;
                if (vAction == "GoiDuyet")
                {
                    vGoiDuyet = true;
                }
                CapNhat(vPhienHopId, vGoiDuyet);
            }
        }


        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            buttonGoiDuyet.Text = "<i class='icofont-send-mail fz17'></i>  Lưu & Gửi duyệt";
            SetEnableForm(true);

            LoadDanhSachPhongHop(true);
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


        /// <summary>       
        /// Validate form và hiện thông báo lỗi
        /// </summary>       
        /// <returns>true: Form không có lỗi; flase: form có trường lỗi</returns>        
        /// <returns></returns>
        protected Boolean ValidateForm()
        {
            Boolean vResult = true;
            Boolean vResult_Password = true;
            string vToastrMessage = "";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";

            if (textTieuDe.Text == "")
            {
                textTieuDe.CssClass += " vld";
                textTieuDe.Focus();
                labelTieu.Attributes["class"] += " vld";
                vToastrMessage += "Tiêu đề, ";
                vResult = false;
            }
            else
            {
                textTieuDe.CssClass = textTieuDe.CssClass.Replace("vld", "").Trim();
                labelTieu.Attributes.Add("class", labelTieu.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (ddlistDonVi.SelectedValue == "")
            {
                ddlistDonVi.CssClass += " vld";
                ddlistDonVi.Focus();
                labelDonVi.Attributes["class"] += " vld";
                vToastrMessage += "Đơn vị, ";
                vResult = false;
            }
            else
            {
                ddlistDonVi.CssClass = ddlistDonVi.CssClass.Replace("vld", "").Trim();
                labelDonVi.Attributes.Add("class", labelDonVi.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (dtpickerThoiGianBatDau.SelectedDate == null)
            {
                dtpickerThoiGianBatDau.CssClass += " vld";
                dtpickerThoiGianBatDau.Focus();
                labelThoiGianBatDau.Attributes["class"] += " vld";
                vToastrMessage += "Thời gian bắt đầu, ";
                vResult = false;
            }
            else
            {
                dtpickerThoiGianBatDau.CssClass = dtpickerThoiGianBatDau.CssClass.Replace("vld", "").Trim();
                labelThoiGianBatDau.Attributes.Add("class", labelThoiGianBatDau.Attributes["class"].ToString().Replace("vld", ""));
            }

            //if (dtpickerThoiGianKetThuc.SelectedDate == null)
            //{
            //    dtpickerThoiGianKetThuc.CssClass += " vld";
            //    dtpickerThoiGianKetThuc.Focus();
            //    labelThoiGianKetThuc.Attributes["class"] += " vld";
            //    vToastrMessage += "Thời gian kết thúc, ";
            //    vResult = false;
            //}
            //else
            //{
            //    dtpickerThoiGianKetThuc.CssClass = dtpickerThoiGianKetThuc.CssClass.Replace("vld", "").Trim();
            //    labelThoiGianKetThuc.Attributes.Add("class", labelThoiGianKetThuc.Attributes["class"].ToString().Replace("vld", ""));
            //}

            if (lblChuTrisPhienHop.Text == "")
            {
                lboxChuTri.CssClass += " vld";
                lboxChuTri.Focus();
                labelChuTri.Attributes["class"] += " vld";
                vToastrMessage += "Chủ trì, ";
                vResult = false;
            }
            else
            {
                lboxChuTri.CssClass = lboxChuTri.CssClass.Replace("vld", "").Trim();
                labelChuTri.Attributes.Add("class", labelChuTri.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vResult == false && vResult_Password == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Vui lòng nhập " + vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ". <br/>" + vToastrMessagePassword, "Thông báo", "error");
            }
            else
            {
                if (vResult == false && vResult_Password == true)
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Vui lòng nhập " + vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
                }
                else
                {
                    if (vResult == true && vResult_Password == false)
                    {
                        ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessagePassword, "Thông báo", "error");
                    }
                }
            }
            return vResult;
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
                buttonCapNhatTaiLieu.Visible = false;
                if (pPhienHopId == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                    buttonGoiDuyet.Visible = false;
                    dtpickerThoiGianBatDau.SelectedDate = DateTime.Now;
                    dtpickerThoiGianKetThuc.SelectedDate = DateTime.Now;
                    dtpickerThoiGianBatDau_ChonPhong.SelectedDate = DateTime.Now;
                    dtpickerThoiGianKetThuc_ChonPhong.SelectedDate = DateTime.Now;
                    buttonCapNhatTaiLieu.Visible = false;
                }
                else // Cập nhật
                {
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).FirstOrDefault();
                    if (vPhienHopInfo != null)
                    {
                        textTieuDe.Text = vPhienHopInfo.TIEUDE;
                        textChuongTrinhHop.Text = ClassCommon.RemoveJavascript(HttpUtility.HtmlDecode(vPhienHopInfo.NOIDUNG)).ToString();
                        string vTest = Server.HtmlDecode(vPhienHopInfo.NOIDUNG).ToString();
                        labelChuongTrinhHop.Text = vPhienHopInfo.NOIDUNG.Replace("\n", "<br/>");//Server.HtmlDecode(vTest);
                        //labelCTH.Text = vTest;
                        ddlistDonVi.SelectedValue = vPhienHopInfo.DONVI_ID.ToString();
                        dtpickerThoiGianBatDau.SelectedDate = vPhienHopInfo.THOIGIANBATDAU;
                        dtpickerThoiGianKetThuc.SelectedDate = vPhienHopInfo.THOIGIANKETTHUC;
                        textGhiChu.Text = vPhienHopInfo.GHICHU;

                        int vChuTriId = 0;
                        vChuTriId = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.LOAI_DAIBIEU == 1 && x.PHIENHOP_ID == pPhienHopId).FirstOrDefault().NGUOIDUNG_ID;
                        // Authur NHHAN
                        //int vThuKyId = 0;
                        //vThuKyId = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.LOAI_DAIBIEU == 3 && x.PHIENHOP_ID == pPhienHopId).FirstOrDefault().NGUOIDUNG_ID;
                        //if (vThuKyId > 0)
                        //    ddlistThuKy.SelectedValue = vThuKyId.ToString();

                        //NHHAN - Lấy danh sách chủ trì 
                        List<HKG_DAIBIEU> vChuTriInfos = vDataContext.HKG_DAIBIEUs.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri && x.PHIENHOP_ID == pPhienHopId && x.LOAI == (int)CommonEnum.LoaiNguoiDung.DaiBieu).ToList();
                        if (vChuTriInfos.Count > 0)
                        {
                            lboxChuTri.DataSource = null;
                            lboxChuTri.DataSource = vChuTriInfos;

                            lboxChuTri.DataBind();
                            string vID_ChuTris = "";
                            foreach (var vChuTriInfo in vChuTriInfos)
                            {
                                if (vID_ChuTris != "")
                                {
                                    vID_ChuTris = vID_ChuTris + ",";
                                }
                                vID_ChuTris = vID_ChuTris + vChuTriInfo.DONVI_ID + "_" + vChuTriInfo.PB_ID + "_" + vChuTriInfo.NGUOIDUNG_ID;
                            }
                            lblChuTrisPhienHop.Text = vID_ChuTris;
                        }


                        // Authur NHTTAI
                        //Lấy danh sách thư ký đã chọn trong phiên họp

                        List<HKG_DAIBIEU> HKG_THUKYs = vDataContext.HKG_DAIBIEUs.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy && x.PHIENHOP_ID == pPhienHopId && x.LOAI == (int)CommonEnum.LoaiNguoiDung.DaiBieu).ToList();
                        if (HKG_THUKYs.Count > 0)
                        {
                            lboxThuKy.DataSource = null;
                            lboxThuKy.DataSource = HKG_THUKYs;

                            lboxThuKy.DataBind();
                            string vID_THUKYS = "";
                            foreach (var HKG_THUKY in HKG_THUKYs)
                            {
                                if (vID_THUKYS != "")
                                {
                                    vID_THUKYS = vID_THUKYS + ",";
                                }
                                vID_THUKYS = vID_THUKYS + HKG_THUKY.DONVI_ID + "_" + HKG_THUKY.PB_ID + "_" + HKG_THUKY.NGUOIDUNG_ID;
                            }
                            lblThuKysPhienHop.Text = vID_THUKYS;
                        }

                        //Lấy danh sách đại biểu đã chọn trong phiên họp
                        //Authur NHTTAI
                        List<HKG_DAIBIEU> HKG_DAIBIEUs = vDataContext.HKG_DAIBIEUs.Where(x => x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu && x.PHIENHOP_ID == pPhienHopId && x.LOAI == (int)CommonEnum.LoaiNguoiDung.DaiBieu).ToList();
                        if (HKG_DAIBIEUs.Count > 0)
                        {
                            lboxDaiBieu.DataSource = null;
                            lboxDaiBieu.DataSource = HKG_DAIBIEUs;

                            lboxDaiBieu.DataBind();
                            string vID_DAIBIEUS = "";
                            foreach (var HKG_DAIBIEU in HKG_DAIBIEUs)
                            {
                                if (vID_DAIBIEUS != "")
                                {
                                    vID_DAIBIEUS = vID_DAIBIEUS + ",";
                                }
                                vID_DAIBIEUS = vID_DAIBIEUS + HKG_DAIBIEU.DONVI_ID + "_" + HKG_DAIBIEU.PB_ID + "_" + HKG_DAIBIEU.NGUOIDUNG_ID;
                            }
                            lblDaiBieusPhienHop.Text = vID_DAIBIEUS;
                        }
                        //Lấy danh sách khách mời đã chọn trong phiên họp
                        //Authur NHHAN
                        List<HKG_KHACHMOI> HKG_KHACHMOIs = vDataContext.HKG_KHACHMOIs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI == (int)CommonEnum.LoaiNguoiDung.KhachMoi && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).ToList();

                        if (HKG_KHACHMOIs.Count() > 0)
                        {
                            lboxKhachMoi.DataSource = null;
                            lboxKhachMoi.DataSource = HKG_KHACHMOIs;

                            lboxKhachMoi.DataBind();
                            string vID_KHACHMOIS = "";
                            foreach (var HKG_KHACHMOI in HKG_KHACHMOIs)
                            {
                                if (vID_KHACHMOIS != "")
                                {
                                    vID_KHACHMOIS = vID_KHACHMOIS + ",";
                                }
                                vID_KHACHMOIS = vID_KHACHMOIS + HKG_KHACHMOI.DONVI_ID + "_" + HKG_KHACHMOI.PB_ID + "_" + HKG_KHACHMOI.NGUOIDUNG_ID;
                            }
                            lblKhachmoisPhienHop.Text = vID_KHACHMOIS;
                        }
                        //SetEnableForm(false);
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
            textTieuDe.Enabled = pEnableStatus;
            ddlistDonVi.Enabled = pEnableStatus;
            dtpickerThoiGianBatDau.Enabled = pEnableStatus;
            dtpickerThoiGianKetThuc.Enabled = pEnableStatus;
            textGhiChu.Enabled = pEnableStatus;

            divChuongTrinhHopCapNhat.Visible = pEnableStatus;
            divChuongTrinhHopChiTiet.Visible = !pEnableStatus;

            lboxChuTri.Enabled = pEnableStatus;
            //ddlistThuKy.Enabled = pEnableStatus;
            lboxDaiBieu.Enabled = pEnableStatus;

            lboxKhachMoi.Enabled = pEnableStatus;

            textTenTaiLieu.Enabled = pEnableStatus;
            textMotaFile.Enabled = pEnableStatus;
            btn_TL.Enabled = pEnableStatus;
            f_file.Enabled = pEnableStatus;
            divFile.Visible = pEnableStatus;
            //dgDanhSach_File.Columns[1].Visible = pEnableStatus;
            dgDanhSach_File.Columns[6].Visible = pEnableStatus;
            divChonPhong.Visible = pEnableStatus;

            //Phòng họp
            dtpickerThoiGianBatDau_ChonPhong.Enabled = pEnableStatus;
            dtpickerThoiGianKetThuc_ChonPhong.Enabled = pEnableStatus;
            textSoNguoi.Enabled = false;
            ddlistThietBi.Enabled = pEnableStatus;
            buttonTimKiem.Visible = pEnableStatus;
            Chonchutri.Visible = pEnableStatus;
            Chondaibieu.Visible = pEnableStatus;
            Chonkhachmoi.Visible = pEnableStatus;
            Chonthuky.Visible = pEnableStatus;
            SetFormInfo(vPhienHopId);
        }

        /// <summary>
        /// Cập nhật thông tin phiên họp
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void CapNhat(int pPhienHopId, bool vGoiDuyet)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string vErrorMessage = "";
                if (pPhienHopId == 0)//Thêm mới phiên họp
                {
                    PHIENHOP vPhienHopInfo = new PHIENHOP();
                    vPhienHopInfo.TIEUDE = ClassCommon.ClearHTML(textTieuDe.Text.Trim());

                    vPhienHopInfo.THOIGIANBATDAU = dtpickerThoiGianBatDau.SelectedDate ?? DateTime.Now;
                    if (dtpickerThoiGianKetThuc.SelectedDate != null)
                    {
                        vPhienHopInfo.THOIGIANKETTHUC = dtpickerThoiGianKetThuc.SelectedDate ?? DateTime.Now;
                    }

                    vPhienHopInfo.TRANGTHAI = 0;
                    if (vGoiDuyet)
                    {
                        vPhienHopInfo.TRANGTHAI = 3;//Trạng thái gởi duyệt
                    }
                    vPhienHopInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                    vPhienHopInfo.NOIDUNG = ClassCommon.RemoveJavascript(textChuongTrinhHop.Text);

                    vPhienHopInfo.UserId_TAO = _currentUser.UserID;
                    vPhienHopInfo.NGAYTAO = DateTime.Now;
                    vPhienHopInfo.UserId_CAPNHAT = _currentUser.UserID;
                    vPhienHopInfo.NGAYCAPNHAT = DateTime.Now;
                    vPhienHopInfo.GHICHU = ClassCommon.ClearHTML(textGhiChu.Text.Trim());
                    int oPhienHopId = 0;
                    vPhienHopControllerInfo.ThemMoiPhienHop(vPhienHopInfo, out oPhienHopId, out vErrorMessage);
                    if (oPhienHopId > 0)
                    {
                        if (ViewState["PHONG_SELECTED"] != null)
                        {
                            int v_Phong_Selected = Int32.Parse(ViewState["PHONG_SELECTED"].ToString());
                            Insert_Phong_By_Phien(v_Phong_Selected, oPhienHopId);

                        }
                        if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                        {
                            List<TAILIEU> objTAILIEUs_Session = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = null;
                            objTAILIEUs_Session.ToList().ForEach(c => c.PHIENHOP_ID = oPhienHopId);
                            objTAILIEUs_Session.ToList().ForEach(c => c.OBJECT_ID = oPhienHopId);

                            //objTAILIEUs_Session.ToList().ForEach(c => c.TAILIEU_ID =);
                            List<TAILIEU> vTaiLieuInfos_New = new List<TAILIEU>();
                            foreach (var TaiLieu in objTAILIEUs_Session)
                            {
                                TAILIEU vTaiLieuInfo = new TAILIEU();
                                vTaiLieuInfo.FILE_NAME = TaiLieu.FILE_NAME;
                                vTaiLieuInfo.FILE_MOTA = TaiLieu.FILE_MOTA;
                                vTaiLieuInfo.FILE_EXT = TaiLieu.FILE_EXT;
                                vTaiLieuInfo.FILE_SIZE = TaiLieu.FILE_SIZE;
                                vTaiLieuInfo.NGAYCAPNHAT = TaiLieu.NGAYCAPNHAT;
                                vTaiLieuInfo.NGAYTAO = TaiLieu.NGAYTAO;
                                vTaiLieuInfo.OBJECT_LOAI = TaiLieu.OBJECT_LOAI;
                                vTaiLieuInfo.OBJECT_ID = TaiLieu.OBJECT_ID;


                                vTaiLieuInfo.TL_NHOM = TaiLieu.TL_NHOM;
                                vTaiLieuInfo.PHIENHOP_ID = TaiLieu.PHIENHOP_ID;
                                vTaiLieuInfo.TEN = TaiLieu.TEN;
                                vTaiLieuInfo.MOTA = TaiLieu.MOTA;
                                vTaiLieuInfo.LOAITAILIEU = TaiLieu.LOAITAILIEU;
                                vTaiLieuInfo.TRANGTHAI = TaiLieu.TRANGTHAI;
                                vTaiLieuInfo.TAILIEUCHUNG = TaiLieu.TAILIEUCHUNG;
                                vTaiLieuInfo.DOMAT = TaiLieu.DOMAT;
                                vTaiLieuInfo.UserId = TaiLieu.UserId;

                                vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                                vDataContext.SubmitChanges();
                                var vTaiLieuId_News = vDataContext.TAILIEUs.OrderByDescending(x => x.TAILIEU_ID).Select(x => x.TAILIEU_ID).FirstOrDefault();
                                if (vTaiLieuId_News > 0)
                                {
                                    List<QUYEN_TAILIEU> vQuyenTLInfos = new List<QUYEN_TAILIEU>();
                                    foreach (var vQTL in TaiLieu.QUYEN_TAILIEUs)
                                    {
                                        QUYEN_TAILIEU vQuyenTLInfo = new QUYEN_TAILIEU();
                                        vQuyenTLInfo.QUYEN_ID = vQTL.QUYEN_ID;
                                        vQuyenTLInfo.TAILIEU_ID = vTaiLieuId_News;
                                        vQuyenTLInfos.Add(vQuyenTLInfo);
                                    }
                                    vDataContext.QUYEN_TAILIEUs.InsertAllOnSubmit(vQuyenTLInfos);
                                    vDataContext.SubmitChanges();
                                }
                            }
                            //vDataContext.TAILIEUs.InsertAllOnSubmit(objTAILIEUs_Session);
                            //vDataContext.SubmitChanges();
                        }
                        Update_ThanhPhanThamDu(oPhienHopId);

                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Tạo phiên họp thành công!";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới phiên họp", "id=" + oPhienHopId);
                        Response.Redirect(vUrl, false);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin phiên họp
                {
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).SingleOrDefault();
                    if (vPhienHopInfo != null)
                    {
                        //vPhienHopNguoiDungControllerInfo.XoaAllPhienHopNguoiDung(pPhienHopId);
                        //vPhienHopKhachMoiControllerInfo.XoaAllPhienHopKhachMoi(pPhienHopId);

                        vPhienHopInfo.NOIDUNG = ClassCommon.RemoveJavascript(textChuongTrinhHop.Text);
                        vPhienHopInfo.UserId_CAPNHAT = _currentUser.UserID;
                        vPhienHopInfo.NGAYCAPNHAT = DateTime.Now;
                        vPhienHopInfo.TIEUDE = ClassCommon.ClearHTML(textTieuDe.Text.Trim());
                        vPhienHopInfo.THOIGIANBATDAU = dtpickerThoiGianBatDau.SelectedDate ?? DateTime.Now;
                        vPhienHopInfo.GHICHU = ClassCommon.ClearHTML(textGhiChu.Text.Trim());
                        if (dtpickerThoiGianKetThuc.SelectedDate != null)
                        {
                            vPhienHopInfo.THOIGIANKETTHUC = dtpickerThoiGianKetThuc.SelectedDate ?? DateTime.Now;
                        }
                        else
                        {
                            vPhienHopInfo.THOIGIANKETTHUC = null;
                        }
                        vPhienHopInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                        if (vGoiDuyet)
                        {
                            vPhienHopInfo.TRANGTHAI = 3;//Trạng thái gởi duyệt
                        }
                        vDataContext.SubmitChanges();
                        Update_ThanhPhanThamDu(pPhienHopId);

                        if (vGoiDuyet)
                        {
                            Session[vMacAddress + TabId.ToString() + "_Message"] = "Gửi duyệt phiên họp: " + vPhienHopInfo.TIEUDE + " thành công";
                            Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                            Response.Redirect(Globals.NavigateURL());
                        }
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Cập nhật thông tin phiên họp thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        //ClassCommon.ShowToastr(Page, "Cập nhật thông tin phiên họp thành công", "Thông báo", "success");
                        Response.Redirect(Request.RawUrl);
                        // NHTAI COMMAND
                        //SetEnableForm(false);
                        //SetFormInfo(pPhienHopId);
                        //buttonThemmoi.Visible = true;
                        //btnCapNhat.Visible = false;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        private void Update_ThanhPhanThamDu(int pPhienHopId)
        {
            //Chủ trì
            try
            {
                //Thêm mới nhiều chủ trì                
                //Authur NHHAN
                List<Int32> vIdChuTri = new List<int>();
                if (lboxChuTri.DataKeys.Count > 0)
                {
                    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    for (int i = 0; i < lboxChuTri.DataKeys.Count; i++)
                    {
                        vIdChuTri.Add(Int32.Parse(lboxChuTri.DataKeys[i].ToString()));
                        int vId = Int32.Parse(lboxChuTri.DataKeys[i].ToString());
                        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);
                    }

                    // ************* Mới***************               
                    // Get chủ trì ID
                    List<int> Lst_TK_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).Select(x => x.NGUOIDUNG_ID).ToList();

                    // List người dùng có trong Database nhưng không được chọn  Delete
                    List<int> lst_TK_FromlboxChuTri = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    // List người dùng cần xóa
                    //List<int> Lst_ND_CANXOA = Lst_ND_ID.Intersect(lst_ND_FromlboxDaiBieu).ToList();
                    List<int> Lst_TK_CANXOA = Lst_TK_ID.Except(lst_TK_FromlboxChuTri).ToList();
                    var obj_TK_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_TK_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).ToList();
                    if (obj_TK_XOA.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_TK_XOA);
                        vDataContext.SubmitChanges();
                    }

                    // List người dùng không tồn tại trong database Insert
                    List<PHIENHOP_NGUOIDUNG> objChuTri_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_TK_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    if (objChuTri_OUTDATABASE.Count > 0)
                    {
                        foreach (var obj in objChuTri_OUTDATABASE)
                        {
                            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                            if (objNguoiDungPhienHop != null)
                            {
                                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                                vDataContext.SubmitChanges();
                                objChuTri_OUTDATABASE.Remove(obj);
                            }
                        }

                        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objChuTri_OUTDATABASE);
                    }

                    //************** End **************
                }
                else
                {
                    var vPhienHopChuTriInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).ToList();
                    if (vPhienHopChuTriInfos != null && vPhienHopChuTriInfos.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopChuTriInfos);
                        vDataContext.SubmitChanges();
                    }
                }



                //Thư ký
                //Authur NHTTAI
                List<Int32> vID_THUKYs = new List<int>();
                if (lboxThuKy.DataKeys.Count > 0)
                {
                    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    for (int i = 0; i < lboxThuKy.DataKeys.Count; i++)
                    {
                        vID_THUKYs.Add(Int32.Parse(lboxThuKy.DataKeys[i].ToString()));
                        int vId = Int32.Parse(lboxThuKy.DataKeys[i].ToString());
                        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);
                    }

                    // ************* Mới***************               
                    // Get thư ký ID
                    List<int> Lst_TK_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).Select(x => x.NGUOIDUNG_ID).ToList();

                    // List người dùng có trong Database nhưng không được chọn  Delete
                    List<int> lst_TK_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    // List người dùng cần xóa
                    List<int> Lst_TK_CANXOA = Lst_TK_ID.Except(lst_TK_FromlboxDaiBieu).ToList();
                    var obj_TK_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_TK_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).ToList();
                    if (obj_TK_XOA.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_TK_XOA);
                        vDataContext.SubmitChanges();
                    }

                    // List người dùng không tồn tại trong database Insert
                    List<PHIENHOP_NGUOIDUNG> objTHUKY_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_TK_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    if (objTHUKY_OUTDATABASE.Count > 0)
                    {
                        foreach (var obj in objTHUKY_OUTDATABASE)
                        {
                            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                            if (objNguoiDungPhienHop != null)
                            {
                                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                                vDataContext.SubmitChanges();
                                objTHUKY_OUTDATABASE.Remove(obj);
                            }
                        }
                        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objTHUKY_OUTDATABASE);
                    }

                    //************** End **************

                }
                else
                {
                    var vPhienHopThuKyInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).ToList();
                    if (vPhienHopThuKyInfos != null && vPhienHopThuKyInfos.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopThuKyInfos);
                        vDataContext.SubmitChanges();
                    }
                }
                //Đại biểu
                if (lboxDaiBieu.DataKeys.Count > 0)
                {
                    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    for (int i = 0; i < lboxDaiBieu.DataKeys.Count; i++)
                    {
                        int vId = Int32.Parse(lboxDaiBieu.DataKeys[i].ToString());

                        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);

                    }

                    // ************* Mới***************               
                    // Get người dùng ID
                    List<int> Lst_ND_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).Select(x => x.NGUOIDUNG_ID).ToList();

                    // List người dùng có trong Database nhưng không được chọn  Delete
                    List<int> lst_ND_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    // List người dùng cần xóa
                    List<int> Lst_ND_CANXOA = Lst_ND_ID.Except(lst_ND_FromlboxDaiBieu).ToList();
                    var obj_ND_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_ND_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).ToList();
                    if (obj_ND_XOA.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_ND_XOA);
                        vDataContext.SubmitChanges();
                    }

                    // List người dùng không tồn tại trong database Insert
                    List<PHIENHOP_NGUOIDUNG> objNGUOIDUNG_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_ND_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    if (objNGUOIDUNG_OUTDATABASE.Count > 0)
                    {
                        foreach (var obj in objNGUOIDUNG_OUTDATABASE)
                        {
                            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                            if (objNguoiDungPhienHop != null)
                            {
                                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                                vDataContext.SubmitChanges();
                                objNGUOIDUNG_OUTDATABASE.Remove(obj);
                            }
                        }

                        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objNGUOIDUNG_OUTDATABASE);
                    }
                    //************** End **************
                }
                else
                {
                    var vPhienHopDaiBieuInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).ToList();
                    if (vPhienHopDaiBieuInfos != null && vPhienHopDaiBieuInfos.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopDaiBieuInfos);
                        vDataContext.SubmitChanges();
                    }
                }


                //Khách mời new
                if (lboxKhachMoi.DataKeys.Count > 0)
                {
                    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    for (int i = 0; i < lboxKhachMoi.DataKeys.Count; i++)
                    {
                        int vId = Int32.Parse(lboxKhachMoi.DataKeys[i].ToString());

                        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.KhachMoi;
                        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);

                    }

                    // ************* Mới***************               
                    // Get người dùng ID
                    List<int> Lst_ND_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).Select(x => x.NGUOIDUNG_ID).ToList();

                    // List người dùng có trong Database nhưng không được chọn  Delete
                    List<int> lst_ND_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    // List người dùng cần xóa
                    List<int> Lst_ND_CANXOA = Lst_ND_ID.Except(lst_ND_FromlboxDaiBieu).ToList();
                    var obj_ND_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_ND_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).ToList();
                    if (obj_ND_XOA.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_ND_XOA);
                        vDataContext.SubmitChanges();
                    }

                    // List người dùng không tồn tại trong database Insert
                    List<PHIENHOP_NGUOIDUNG> objNGUOIDUNG_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_ND_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    if (objNGUOIDUNG_OUTDATABASE.Count > 0)
                    {
                        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objNGUOIDUNG_OUTDATABASE);
                    }

                    //************** End **************                
                }
                else
                {
                    var vPhienHopDaiBieuInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).ToList();
                    if (vPhienHopDaiBieuInfos != null && vPhienHopDaiBieuInfos.Count > 0)
                    {
                        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopDaiBieuInfos);
                        vDataContext.SubmitChanges();
                    }
                }
            }
            catch (Exception Ex)
            {

            }
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

        /// <summary>
        /// Load danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public void LoadDanhSachDonVi()
        {
            try
            {
                List<DONVI> vDonViInfos = new List<DONVI>();
                vDonViInfos = vDataContext.DONVIs.OrderBy(x => x.TENDONVI).ToList();
                ddlistDonVi.Items.Clear();
                ddlistDonVi.DataSource = vDonViInfos;
                ddlistDonVi.DataTextField = "TENDONVI";
                ddlistDonVi.DataValueField = "DONVI_ID";
                ddlistDonVi.DataBind();
                ddlistDonVi.Items.Insert(0, new ListItem("Chọn đơn vị", ""));

                //ddlistDonVi_ChonDaiBieu.DataSource = vDonViInfos;
                //ddlistDonVi_ChonDaiBieu.DataTextField = "TENDONVI";
                //ddlistDonVi_ChonDaiBieu.DataValueField = "DONVI_ID";
                //ddlistDonVi_ChonDaiBieu.DataBind();
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới phiên họp", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion

        #region Xử lý file       

        public void LoadDSFile(int pPhienHopId = 0)
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("TENTAILIEU");
            dtTable.Columns.Add("TRANGTHAI");
            dtTable.Columns.Add("MOTA");
            dtTable.Columns.Add("NHOM");
            dtTable.Columns.Add("DOMAT");
            dtTable.Columns.Add("QUYEN");
            dtTable.Columns.Add("HA_FILE_PATH");
            dtTable.Columns.Add("HA_ID");
            dtTable.Columns.Add("HA_TENFILE");
            dtTable.Columns.Add("HA_EXT");
            dtTable.Columns.Add("HA_SIZE");
            // Trường họp cập nhật
            List<TAILIEU> objTAILIEU = new List<TAILIEU>();
            if (pPhienHopId != 0)
            {
                objTAILIEU = (from p in vDataContext.TAILIEUs
                              where p.OBJECT_ID == pPhienHopId && p.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuHop
                              orderby p.TAILIEU_ID descending
                              select p).ToList();
            }
            else
            {
                if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                {
                    objTAILIEU = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                }
            }
            var vQuyenInfos = vDataContext.QUYENs.ToList();
            if (objTAILIEU.Count > 0)
            {
                foreach (var it in objTAILIEU)
                {
                    DataRow row = dtTable.NewRow();
                    row["TENTAILIEU"] = (it.TEN);
                    row["TRANGTHAI"] = (it.TRANGTHAI);
                    row["MOTA"] = (it.MOTA);
                    row["NHOM"] = (it.TL_NHOM);
                    row["DOMAT"] = (it.DOMAT == true ? "Mật" : "Không");
                    row["HA_FILE_PATH"] = (it.FILE_NAME);
                    string vQuyenXemStr = "";
                    var QuyenTaiLieuInfos = it.QUYEN_TAILIEUs.ToList();
                    if (vPhienHopId > 0)
                    {
                        QuyenTaiLieuInfos = vDataContext.QUYEN_TAILIEUs.Where(x => x.TAILIEU_ID == it.TAILIEU_ID).ToList();
                    }

                    if (QuyenTaiLieuInfos.Count > 0)
                    {
                        if (QuyenTaiLieuInfos.Count >= 4)
                        {
                            vQuyenXemStr = "Tất cả, ";
                        }
                        else
                        {
                            foreach (var vQuyenXem in QuyenTaiLieuInfos)
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

                    row["HA_ID"] = (it.TAILIEU_ID);
                    row["HA_TENFILE"] = it.FILE_MOTA;
                    row["HA_EXT"] = it.FILE_EXT;
                    row["HA_SIZE"] = it.FILE_SIZE;
                    dtTable.Rows.Add(row);
                }
            }

            dgDanhSach_File.DataSource = dtTable;
            dgDanhSach_File.DataBind();
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
                if (vPhienHopId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                    {
                        List<TAILIEU> objTAILIEUs = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                        vTaiLieuInfo = objTAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault();
                        int vIndex = objTAILIEUs.IndexOf(vTaiLieuInfo);
                        objTAILIEUs.Remove(vTaiLieuInfo);
                        if (vTaiLieuInfo.DOMAT ?? true)
                        {
                            vTaiLieuInfo.DOMAT = false;
                        }
                        else if (!vTaiLieuInfo.DOMAT ?? false)
                        {
                            vTaiLieuInfo.DOMAT = true;
                        }
                        objTAILIEUs.Insert(vIndex, vTaiLieuInfo);
                        Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = objTAILIEUs;
                    }
                }
                else
                {
                    vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault();
                    if (vTaiLieuInfo.DOMAT ?? true)
                    {
                        vTaiLieuInfo.DOMAT = false;
                    }
                    else if (!vTaiLieuInfo.DOMAT ?? false)
                    {
                        vTaiLieuInfo.DOMAT = true;
                    }
                    vDataContext.SubmitChanges();
                }
                ClassCommon.ShowToastr(Page, "Cập nhật mức độ phổ biến của tài liệu: " + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
                callModalScript();
                LoadDSFile(vPhienHopId);
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
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                TableCell cell = dgDanhSach_File.Rows[e.RowIndex].Cells[0];
                string vFile_Name = dgDanhSach_File.DataKeys[e.RowIndex]["HA_FILE_PATH"].ToString();
                File.Delete(Server.MapPath(vPathCommonUploadFile) + "/" + vFile_Name);
                int vFile_ID = int.Parse(dgDanhSach_File.DataKeys[e.RowIndex].Value.ToString());
                if (vPhienHopId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                    {
                        List<TAILIEU> objTAILIEUs = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                        TAILIEU vTaiLieuInfo = objTAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).FirstOrDefault();
                        objTAILIEUs.Remove(vTaiLieuInfo);
                        Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = objTAILIEUs;
                    }
                }
                else
                {
                    var vQuyenTaiLieuInfos = vDataContext.QUYEN_TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).ToList();
                    vDataContext.QUYEN_TAILIEUs.DeleteAllOnSubmit(vQuyenTaiLieuInfos);
                    vDataContext.SubmitChanges();
                    var vTapTinInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).SingleOrDefault();
                    vDataContext.TAILIEUs.DeleteOnSubmit(vTapTinInfo);
                    vDataContext.SubmitChanges();
                }


                LoadDSFile(vPhienHopId);
                ClassCommon.ShowToastr(Page, "Xoá tài liệu họp thành công", "Thông báo", "success");
                callModalScript();
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Xóa tập tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnXoa_tapTin_Click(object sender, EventArgs e)
        {
            lblTapTin.Text = "";
            pnlTapTin.Visible = false;
            f_file.Visible = true;
            btn_TL.Visible = true;
            buttonCapNhatTaiLieu.Visible = false;
        }

        /// <summary>
        /// Chọn sửa tài liệu từ dánh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SuaTaiLieu(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vTaiLieuId = int.Parse(html.HRef);
                TAILIEU vTaiLieuInfo = new TAILIEU();
                string oErrorMessage = "";
                pnlTapTin.Visible = true;
                f_file.Visible = false;
                if (vPhienHopId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                    {
                        List<TAILIEU> objTAILIEUs = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                        vTaiLieuInfo = objTAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault();
                        if (vTaiLieuInfo != null)
                        {
                            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"] = vTaiLieuInfo;
                            textTenTaiLieu.Text = vTaiLieuInfo.TEN;
                            textNhomTaiLieu.Text = vTaiLieuInfo.TL_NHOM;
                            textMotaFile.Text = textMotaFile.Text;
                            lblTapTin.Text = "<a href='" + "/" + vTaiLieuInfo.FILE_NAME + "' target='_blank'>" + vTaiLieuInfo.FILE_MOTA + "</a>";
                            if (vTaiLieuInfo.QUYEN_TAILIEUs != null)
                            {
                                foreach (var Quyen in vTaiLieuInfo.QUYEN_TAILIEUs)
                                {
                                    if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.ChuTri)
                                    {
                                        checkboxChuTri.Checked = true;
                                    }
                                    else
                                    {
                                        checkboxChuTri.Checked = true;
                                    }
                                    if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.DaiBieu)
                                    {
                                        checkboxDaiBieu.Checked = true;
                                    }
                                    else
                                    {
                                        checkboxDaiBieu.Checked = false;
                                    }
                                    if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.ThuKy)
                                    {
                                        checkboxThuKy.Checked = true;
                                    }
                                    else
                                    {
                                        checkboxThuKy.Checked = false;
                                    }
                                    if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.KhachMoi)
                                    {
                                        checkboxKhachMoi.Checked = true;
                                    }
                                    else
                                    {
                                        checkboxKhachMoi.Checked = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuId).FirstOrDefault();                 
                    textTenTaiLieu.Text = vTaiLieuInfo.TEN;
                    textNhomTaiLieu.Text = vTaiLieuInfo.TL_NHOM;
                    textMotaFile.Text = vTaiLieuInfo.MOTA;
                    lblTapTin.Text = "<a href='" + Server.MapPath(vPathCommonUploadFile) + "/" + vTaiLieuInfo.FILE_NAME + "' target='_blank'>" + vTaiLieuInfo.FILE_MOTA + "</a>";
                    if (vTaiLieuInfo.QUYEN_TAILIEUs != null)
                    {
                        foreach (var Quyen in vTaiLieuInfo.QUYEN_TAILIEUs)
                        {
                            if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.ChuTri)
                            {
                                checkboxChuTri.Checked = true;
                            }
                            else
                            {
                                checkboxChuTri.Checked = true;
                            }
                            if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.DaiBieu)
                            {
                                checkboxDaiBieu.Checked = true;
                            }
                            else
                            {
                                checkboxDaiBieu.Checked = false;
                            }
                            if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.ThuKy)
                            {
                                checkboxThuKy.Checked = true;
                            }
                            else
                            {
                                checkboxThuKy.Checked = false;
                            }
                            if (Quyen.QUYEN_ID == (int)CommonEnum.LoaiDaiBieu.KhachMoi)
                            {
                                checkboxKhachMoi.Checked = true;
                            }
                            else
                            {
                                checkboxKhachMoi.Checked = false;
                            }
                        }
                    }
                    Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"] = vTaiLieuInfo;
                }
                //ClassCommon.ShowToastr(Page, "Cập nhật mức độ phổ biến của tài liệu: " + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
                //callModalScript();
                //LoadDSFile(vPhienHopId);
                
                buttonCapNhatTaiLieu.Visible = true;
                btn_TL.Visible = false;
                ViewState["Tab"] = "TAILIEU";
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Xử lý cập nhật tài liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonCapNhatTaiLieu_Click(object sender, EventArgs e)
        {
            try
            {
                if (vPhienHopId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] != null)
                    {
                        if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"] != null)
                        {
                            var vTaiLieuInfo_SS = (TAILIEU)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"];
                            if (vTaiLieuInfo_SS != null)
                            {
                                List<TAILIEU> objTAILIEUs = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                                TAILIEU vTaiLieuInfo = objTAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuInfo_SS.TAILIEU_ID).FirstOrDefault();
                                objTAILIEUs.Remove(vTaiLieuInfo);
                                vTaiLieuInfo.TL_NHOM = ClassCommon.ClearHTML(textNhomTaiLieu.Text.Trim());
                                vTaiLieuInfo.TEN = ClassCommon.ClearHTML(textTenTaiLieu.Text.Trim());
                                vTaiLieuInfo.MOTA = ClassCommon.ClearHTML(textMotaFile.Text.Trim());
                                EntitySet<QUYEN_TAILIEU> vQuyenTaiLieuInfos = new EntitySet<QUYEN_TAILIEU>();
                                if (vTaiLieuInfo.QUYEN_TAILIEUs.Count > 0)
                                {
                                    foreach (var Quyen_TL in vTaiLieuInfo.QUYEN_TAILIEUs)
                                    {
                                        if (checkboxChuTri.Checked)
                                        {
                                            var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                            vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                                            vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                        }
                                        if (checkboxThuKy.Checked)
                                        {
                                            var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                            vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                                            vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                        }
                                        if (checkboxDaiBieu.Checked)
                                        {
                                            var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                            vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                                            vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                        }
                                        if (checkboxKhachMoi.Checked)
                                        {
                                            var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                            vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.KhachMoi;
                                            vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                        }
                                    }
                                    vTaiLieuInfo.QUYEN_TAILIEUs = vQuyenTaiLieuInfos;
                                    objTAILIEUs.Add(vTaiLieuInfo);
                                    Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = objTAILIEUs;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"] != null)
                    {
                        var vTaiLieuInfo_SS = (TAILIEU)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEU_CapNhat"];
                        if (vTaiLieuInfo_SS != null)
                        {
                            var vTaiLieuInfo = vDataContext.TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuInfo_SS.TAILIEU_ID).SingleOrDefault();
                            if (vTaiLieuInfo != null)
                            {
                                vTaiLieuInfo.TL_NHOM = ClassCommon.ClearHTML(textNhomTaiLieu.Text.Trim());
                                vTaiLieuInfo.TEN = ClassCommon.ClearHTML(textTenTaiLieu.Text.Trim());
                                vTaiLieuInfo.MOTA = ClassCommon.ClearHTML(textMotaFile.Text.Trim());
                                vDataContext.SubmitChanges();
                                List<QUYEN_TAILIEU> vQuyenTaiLieuInfos_old = new List<QUYEN_TAILIEU>();
                                if (vTaiLieuInfo.QUYEN_TAILIEUs.Count > 0)
                                {
                                    vQuyenTaiLieuInfos_old = vDataContext.QUYEN_TAILIEUs.Where(x => x.TAILIEU_ID == vTaiLieuInfo_SS.TAILIEU_ID).ToList();
                                    vDataContext.QUYEN_TAILIEUs.DeleteAllOnSubmit(vQuyenTaiLieuInfos_old);
                                    vDataContext.SubmitChanges();
                                }
                                List<QUYEN_TAILIEU> vQuyenTaiLieuInfos = new List<QUYEN_TAILIEU>();

                                if (checkboxChuTri.Checked)
                                {
                                    QUYEN_TAILIEU vQuyenTaiLieuInfoChuTri = new QUYEN_TAILIEU();
                                    vQuyenTaiLieuInfoChuTri.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                                    vQuyenTaiLieuInfoChuTri.TAILIEU_ID = vTaiLieuInfo.TAILIEU_ID;
                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfoChuTri);
                                }
                                if (checkboxThuKy.Checked)
                                {
                                    QUYEN_TAILIEU vQuyenTaiLieuInfoThuKy = new QUYEN_TAILIEU();
                                    vQuyenTaiLieuInfoThuKy.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                                    vQuyenTaiLieuInfoThuKy.TAILIEU_ID = vTaiLieuInfo.TAILIEU_ID;
                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfoThuKy);
                                }
                                if (checkboxDaiBieu.Checked)
                                {
                                    QUYEN_TAILIEU vQuyenTaiLieuInfoDaiBieu = new QUYEN_TAILIEU();
                                    vQuyenTaiLieuInfoDaiBieu.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                                    vQuyenTaiLieuInfoDaiBieu.TAILIEU_ID = vTaiLieuInfo.TAILIEU_ID;
                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfoDaiBieu);
                                }
                                if (checkboxKhachMoi.Checked)
                                {
                                    QUYEN_TAILIEU vQuyenTaiLieuInfoDaiBieu = new QUYEN_TAILIEU();
                                    vQuyenTaiLieuInfoDaiBieu.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.KhachMoi;
                                    vQuyenTaiLieuInfoDaiBieu.TAILIEU_ID = vTaiLieuInfo.TAILIEU_ID;
                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfoDaiBieu);
                                }
                                vDataContext.QUYEN_TAILIEUs.InsertAllOnSubmit(vQuyenTaiLieuInfos);
                                vDataContext.SubmitChanges();
                            }                            
                        }
                    }
                }
                lblTapTin.Visible = false;
                f_file.Visible = true;
                textTenTaiLieu.Text = "";
                textMotaFile.Text = "";
                buttonCapNhatTaiLieu.Visible = false;
                f_file.Visible = true;
                checkboxChuTri.Checked = true;
                checkboxDaiBieu.Checked = true;
                checkboxThuKy.Checked = true;
                checkboxKhachMoi.Checked = true;

                LoadDSFile(vPhienHopId);

                ClassCommon.ShowToastr(Page, "Cập nhật thông tin tài liệu thành công", "Thông báo", "success");
                callModalScript();
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
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
            try
            {
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
                    if (textNhomTaiLieu.Text.Trim() == "")
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng nhập tên Nhóm tài liệu", "Thông báo lỗi", "error");

                        textNhomTaiLieu.CssClass += " vld";
                        textNhomTaiLieu.Focus();
                        labelNhomTaiLieu.Attributes["class"] += " vld";
                    }
                    else
                    {
                        textNhomTaiLieu.CssClass = textNhomTaiLieu.CssClass.Replace("vld", "").Trim();
                        labelNhomTaiLieu.Attributes.Add("class", labelNhomTaiLieu.Attributes["class"].ToString().Replace("vld", ""));
                        if (!checkboxChuTri.Checked && !checkboxDaiBieu.Checked && !checkboxKhachMoi.Checked && !checkboxThuKy.Checked)
                        {
                            ClassCommon.ShowToastr(Page, "Vui lòng chọn quyền xem tài liệu", "Thông báo lỗi", "error");
                            labelQuyenXem.Attributes["class"] += " vld";
                        }
                        else
                        {
                            labelQuyenXem.Attributes.Add("class", labelQuyenXem.Attributes["class"].ToString().Replace("vld", ""));
                            if (!f_file.HasFile && buttonCapNhatTaiLieu.Visible == false)
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
                                List<TAILIEU> objTAILIEUs = new List<TAILIEU>();
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

                                                string vFilename = filename.Substring(0, filename.Length - extension.Length) + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + extension;
                                                ClassCommon.UploadFile(userPostedFile, filepath, vFilename, "");

                                                TAILIEU vTaiLieuInfo = new TAILIEU();
                                                vTaiLieuInfo.FILE_NAME = vFilename;
                                                vTaiLieuInfo.FILE_MOTA = filename.Substring(0, filename.Length - extension.Length);
                                                vTaiLieuInfo.FILE_EXT = extension;
                                                vTaiLieuInfo.FILE_SIZE = Int32.Parse(userPostedFile.ContentLength.ToString());
                                                vTaiLieuInfo.NGAYCAPNHAT = DateTime.Now;
                                                vTaiLieuInfo.NGAYTAO = DateTime.Now;
                                                vTaiLieuInfo.OBJECT_LOAI = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                                vTaiLieuInfo.OBJECT_ID = vPhienHopId;


                                                vTaiLieuInfo.TL_NHOM = ClassCommon.ClearHTML(textNhomTaiLieu.Text.Trim());
                                                vTaiLieuInfo.PHIENHOP_ID = vPhienHopId;
                                                vTaiLieuInfo.TEN = ClassCommon.ClearHTML(textTenTaiLieu.Text.Trim());
                                                vTaiLieuInfo.MOTA = ClassCommon.ClearHTML(textMotaFile.Text.Trim());
                                                vTaiLieuInfo.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                                vTaiLieuInfo.TRANGTHAI = 1;
                                                vTaiLieuInfo.TAILIEUCHUNG = true;
                                                vTaiLieuInfo.DOMAT = false;
                                                vTaiLieuInfo.UserId = _currentUser.UserID;
                                                EntitySet<QUYEN_TAILIEU> vQuyenTaiLieuInfos = new EntitySet<QUYEN_TAILIEU>();
                                                //Kiểm tra quyền chủ trì được xem
                                                if (checkboxChuTri.Checked)
                                                {
                                                    var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                                    vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                                }
                                                //Kiểm tra quyền đại biểu được xem
                                                if (checkboxDaiBieu.Checked)
                                                {
                                                    var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                                    vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                                }
                                                //Kiểm tra quyền thư ký được xem
                                                if (checkboxThuKy.Checked)
                                                {
                                                    var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                                    vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                                }
                                                //Kiểm tra quyền khách mời được xem
                                                if (checkboxKhachMoi.Checked)
                                                {
                                                    var vQuyenTaiLieuInfo = new QUYEN_TAILIEU();
                                                    vQuyenTaiLieuInfo.QUYEN_ID = (int)CommonEnum.LoaiDaiBieu.KhachMoi;

                                                    vQuyenTaiLieuInfos.Add(vQuyenTaiLieuInfo);
                                                }
                                                vTaiLieuInfo.QUYEN_TAILIEUs = vQuyenTaiLieuInfos;
                                                objTAILIEUs.Add(vTaiLieuInfo);
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
                                    catch (Exception ex)
                                    {

                                    }
                                }// End for
                                if (vPhienHopId != 0)
                                {
                                    if (objTAILIEUs.Count > 0)
                                    {
                                        foreach (var TaiLieu in objTAILIEUs)
                                        {
                                            TAILIEU vTaiLieuInfo = new TAILIEU();
                                            vTaiLieuInfo.FILE_NAME = TaiLieu.FILE_NAME;
                                            vTaiLieuInfo.FILE_MOTA = TaiLieu.FILE_MOTA;
                                            vTaiLieuInfo.FILE_EXT = TaiLieu.FILE_EXT;
                                            vTaiLieuInfo.FILE_SIZE = TaiLieu.FILE_SIZE;
                                            vTaiLieuInfo.NGAYCAPNHAT = TaiLieu.NGAYCAPNHAT;
                                            vTaiLieuInfo.NGAYTAO = TaiLieu.NGAYTAO;
                                            vTaiLieuInfo.OBJECT_LOAI = TaiLieu.OBJECT_LOAI;
                                            vTaiLieuInfo.OBJECT_ID = TaiLieu.OBJECT_ID;

                                            vTaiLieuInfo.TL_NHOM = TaiLieu.TL_NHOM;
                                            vTaiLieuInfo.PHIENHOP_ID = TaiLieu.PHIENHOP_ID;
                                            vTaiLieuInfo.TEN = TaiLieu.TEN;
                                            vTaiLieuInfo.MOTA = TaiLieu.MOTA;
                                            vTaiLieuInfo.LOAITAILIEU = TaiLieu.LOAITAILIEU;
                                            vTaiLieuInfo.TRANGTHAI = TaiLieu.TRANGTHAI;
                                            vTaiLieuInfo.TAILIEUCHUNG = TaiLieu.TAILIEUCHUNG;
                                            vTaiLieuInfo.DOMAT = TaiLieu.DOMAT;
                                            vTaiLieuInfo.UserId = TaiLieu.UserId;

                                            vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                                            vDataContext.SubmitChanges();
                                            var vTaiLieuId_News = vDataContext.TAILIEUs.OrderByDescending(x => x.TAILIEU_ID).Select(x => x.TAILIEU_ID).FirstOrDefault();
                                            if (vTaiLieuId_News > 0)
                                            {
                                                List<QUYEN_TAILIEU> vQuyenTLInfos = new List<QUYEN_TAILIEU>();
                                                foreach (var vQTL in TaiLieu.QUYEN_TAILIEUs)
                                                {
                                                    QUYEN_TAILIEU vQuyenTLInfo = new QUYEN_TAILIEU();
                                                    vQuyenTLInfo.QUYEN_ID = vQTL.QUYEN_ID;
                                                    vQuyenTLInfo.TAILIEU_ID = vTaiLieuId_News;
                                                    vQuyenTLInfos.Add(vQuyenTLInfo);
                                                }
                                                vDataContext.QUYEN_TAILIEUs.InsertAllOnSubmit(vQuyenTLInfos);
                                                vDataContext.SubmitChanges();
                                            }
                                        }
                                    }

                                    //vDataContext.TAILIEUs.InsertAllOnSubmit(objTAILIEUs);
                                    //vDataContext.SubmitChanges();
                                }
                                else
                                {
                                    if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] == null)
                                    {
                                        Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = new List<TAILIEU>();
                                    }

                                    List<TAILIEU> objTAILIEUs_Session = (List<TAILIEU>)Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"];
                                    foreach (var item in objTAILIEUs)
                                    {
                                        Int32 vID_TEMP = Int32.Parse(DateTime.Now.ToString("ddHHmmss"));
                                        item.TAILIEU_ID = vID_TEMP;
                                        objTAILIEUs_Session.Insert(0, item);
                                    }
                                    Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_" + "objTAILIEUs"] = objTAILIEUs_Session;
                                }
                                LoadDSFile(vPhienHopId);
                                textTenTaiLieu.Text = "";
                                textMotaFile.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            callModalScript();
        }

        public void callModalScript()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay();", true);
        }
        #endregion

        #region Tab chọn phòng họp
        /// <summary>
        /// Get danh sách tên thiết bị
        /// </summary>
        /// <param name="pThietBiId"></param>
        /// <returns></returns>
        public string GetDanhSachTenThietBi(int pThietBiId)
        {

            try
            {
                return vPhongHopControllerInfo.GetDanhSachTenThietBi(pThietBiId);

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// Load danh sách phòng họp
        /// </summary>
        public void LoadDanhSachPhongHop(bool p_is_edit)
        {
            try
            {
                List<PHONGHOP> lstPHONGHOP = new List<PHONGHOP>();
                List<HKG_PHONGHOP_THIETBIResult> hKG_PHONGHOP_s = new List<HKG_PHONGHOP_THIETBIResult>();
                string vThietBiID = "";
                int countTB = 0;
                int SucChua = 0;
                if (textSoNguoi.Text != "")
                {
                    SucChua = Convert.ToInt32(textSoNguoi.Text);
                }
                if (ddlistThietBi.CheckedItems != null)
                {
                    int vCount = ddlistThietBi.CheckedItems.Count;
                    foreach (var vThietBi in ddlistThietBi.CheckedItems)
                    {
                        countTB++;
                        if (countTB == vCount)
                        {
                            vThietBiID += vThietBi.Value;
                        }
                        else
                        {
                            vThietBiID += vThietBi.Value + ",";
                        }
                    }
                }
                // Lấy thông tin phòng đã chọn cho phiên họp
                var objPhienHop_PhongHop_Select = vDataContext.PHIENHOP_PHONGHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                //Trường họp view
                if (!p_is_edit)
                {
                    if (objPhienHop_PhongHop_Select != null)
                    {
                        lstPHONGHOP = vDataContext.PHONGHOPs.Where(x => x.PHONGHOP_ID == objPhienHop_PhongHop_Select.PHONGHOP_ID).ToList();
                    }
                    else
                    {
                        labelThongBaoPhongHop.Text = "Chưa chọn phòng họp.";
                        labelThongBaoPhongHop.Visible = true;
                    }
                }
                else
                {
                    if (countTB > 0)
                    {
                        hKG_PHONGHOP_s = vDataContext.HKG_PHONGHOP_THIETBI(vThietBiID, countTB, SucChua).ToList();
                        if (hKG_PHONGHOP_s.Count > 0)
                        {
                            lstPHONGHOP = vDataContext.PHONGHOPs.Where(x => hKG_PHONGHOP_s.Select(z => z.PHONGHOP_ID).Contains(x.PHONGHOP_ID)).ToList();
                        }
                    }
                    // Trường hợp không chọn thiết bị
                    else
                    {
                        lstPHONGHOP = vDataContext.PHONGHOPs.Where(x => x.SUCCHUA >= SucChua).ToList();
                    }
                }
                //  Trường hợp có chọn thiết bị
                // Phòng hợp thỏa điều kiện thiết bị và sức chứa ==> tìm theo thời gian 
                //List<int> vPhongHopIds_DaCoLich = new List<int>();
                List<HKG_PHONGHOP_PHIENHOPResult> HKG_PHONGHOP_PHIENHOPResults = new List<HKG_PHONGHOP_PHIENHOPResult>();
                if (lstPHONGHOP.Count > 0)
                {
                    HKG_PHONGHOP_PHIENHOPResults = vDataContext.HKG_PHONGHOP_PHIENHOP(dtpickerThoiGianBatDau_ChonPhong.SelectedDate, dtpickerThoiGianKetThuc_ChonPhong.SelectedDate).ToList();

                }
                DataTable vDataTableInfo = new DataTable();
                vDataTableInfo.Columns.Add("STT");
                vDataTableInfo.Columns.Add("PHONGHOP_ID");
                vDataTableInfo.Columns.Add("SUCCHUA");
                vDataTableInfo.Columns.Add("TENPHONGHOP");
                vDataTableInfo.Columns.Add("DANGKY");
                vDataTableInfo.Columns.Add("LICHHOP");
                vDataTableInfo.Columns.Add("DACHON");
                var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                if (lstPHONGHOP.Count > 0)
                {
                    pnlThongBao.Visible = false;
                    pnlKetQuaTimKiem.Visible = true;
                    int STT = 0;
                    foreach (var vPhongHopInfo in lstPHONGHOP)
                    {
                        STT = STT + 1;
                        DataRow row = vDataTableInfo.NewRow();
                        row["STT"] = STT;
                        row["PHONGHOP_ID"] = vPhongHopInfo.PHONGHOP_ID;
                        row["SUCCHUA"] = vPhongHopInfo.SUCCHUA;
                        row["TENPHONGHOP"] = vPhongHopInfo.TENPHONGHOP;
                        if (HKG_PHONGHOP_PHIENHOPResults.Count > 0)
                        {
                            if (HKG_PHONGHOP_PHIENHOPResults.Select(x => x.PHONGHOP_ID).Distinct().Contains(vPhongHopInfo.PHONGHOP_ID))// && ((vPhienHopInfo.THOIGIANBATDAU <= dtpickerThoiGianBatDau.SelectedDate && dtpickerThoiGianBatDau.SelectedDate >= vPhienHopInfo.THOIGIANBATDAU)))
                            {
                                row["DANGKY"] = "display: none;";

                                var HKG_PHONGHOP_PHIENHOPResults_BY_ID = HKG_PHONGHOP_PHIENHOPResults.Where(x => x.PHONGHOP_ID == vPhongHopInfo.PHONGHOP_ID).ToList();
                                if (HKG_PHONGHOP_PHIENHOPResults_BY_ID.Count > 0)
                                {
                                    string vLichHopTiepTheo = "";
                                    foreach (var vPhienHop in HKG_PHONGHOP_PHIENHOPResults_BY_ID)
                                    {
                                        vLichHopTiepTheo = vLichHopTiepTheo + "<i class='icofont-calendar fz15' style='color: #ff6600'></i> <span><b>" + vPhienHop.TIEUDE + " </b> (" + String.Format("{0:hh:mm dd/MM}", vPhienHop.THOIGIANBATDAU) + " - " + String.Format("{0: hh:mm dd/MM}", vPhienHop.THOIGIANKETTHUC) + ")</span><br/>";
                                    }
                                    row["LICHHOP"] = vLichHopTiepTheo;
                                }
                            }
                            else
                            {
                                row["DANGKY"] = "";
                            }
                        }
                        else
                        {
                            row["DANGKY"] = "";
                        }
                        if (vPhienHopId != 0)
                        {
                            if (((dtpickerThoiGianBatDau_ChonPhong.SelectedDate < vPhienHopInfo.THOIGIANBATDAU || dtpickerThoiGianBatDau_ChonPhong.SelectedDate > vPhienHopInfo.THOIGIANKETTHUC)
                             || (dtpickerThoiGianKetThuc_ChonPhong.SelectedDate < vPhienHopInfo.THOIGIANBATDAU || dtpickerThoiGianKetThuc_ChonPhong.SelectedDate > vPhienHopInfo.THOIGIANKETTHUC)))
                            {
                                row["DANGKY"] = "display: none;";
                                row["LICHHOP"] = "Phòng trống - cập nhật thời gian họp để đăng ký phòng";
                            }
                        }

                        if (vPhienHopId == 0)
                        {
                            if (ViewState["PHONG_SELECTED"] != null)
                            {
                                int v_PHONG_SELECTED = Int32.Parse(ViewState["PHONG_SELECTED"].ToString());
                                if (vPhongHopInfo.PHONGHOP_ID == v_PHONG_SELECTED)
                                {
                                    row["DACHON"] = "";
                                    row["DANGKY"] = "display: none;";

                                }
                                else
                                {
                                    row["DACHON"] = "display: none;";
                                }
                            }
                            else
                            {
                                row["DACHON"] = "display: none;";
                            }
                        }
                        else
                        {
                            if (objPhienHop_PhongHop_Select != null)
                            {
                                if (vPhongHopInfo.PHONGHOP_ID == objPhienHop_PhongHop_Select.PHONGHOP_ID)
                                {
                                    row["DACHON"] = "";
                                    row["DANGKY"] = "display: none;";
                                    // xem chi tiết ẩn button bỏ chọn
                                    if (!p_is_edit)
                                    {
                                        row["DACHON"] = "display: none;";
                                    }
                                }
                                else
                                {
                                    row["DACHON"] = "display: none;";
                                }
                            }
                            else
                            {
                                row["DACHON"] = "display: none;";
                            }
                        }



                        vDataTableInfo.Rows.Add(row);
                    }
                    //ListView_PHONG.DataSource = lstPHONGHOP;
                    labelThongBaoPhongHop.Text = "";
                    labelThongBaoPhongHop.Visible = false;
                }
                else
                {
                    labelThongBaoPhongHop.Text = "Không tìm thấy thông tin được yêu cầu.";
                    labelThongBaoPhongHop.Visible = true;
                }
                ListView_PHONG.DataSource = vDataTableInfo;
                ListView_PHONG.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Xóa phòng đã chọn    
        protected void XoaPhong(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vPhongID = html.HRef;
            vDuyetPhienHopController.XoaPhongDaChon(vPhienHopId);
            ClassCommon.ShowToastr(Page, "Xóa phòng đã chọn cho phiên họp thành công!", "Thông báo", "success");
            var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
            if (vPhienHopInfo != null)
            {
                vPhienHopInfo.PHONGHOP_ID = null;
                vDataContext.SubmitChanges();
            }
            LoadDanhSachPhongHop(true);
            ActiveTabChonPhong();
        }


        /// <summary>
        /// Đăng ký phòng cho phiên họp        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DangKyPhong(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vPhongID = html.HRef;

            if (vPhienHopId == 0)
            {
                ViewState["Tab"] = "PHONGHOP";
                ViewState["PHONG_SELECTED"] = vPhongID;
                ClassCommon.ShowToastr(Page, "Đăng ký phòng thành công!", "Thông báo", "success");
                LoadDanhSachPhongHop(true);


            }
            else
            {
                Insert_Phong_By_Phien(Int32.Parse(vPhongID), vPhienHopId);
                ClassCommon.ShowToastr(Page, "Đăng ký phòng thành công!", "Thông báo", "success");
                LoadDanhSachPhongHop(true);
                ViewState["Tab"] = "PHONGHOP";
            }
        }

        private void Insert_Phong_By_Phien(int pPhongID, int pPhienHopID)
        {
            var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopID).SingleOrDefault();
            if (vPhienHopInfo != null)
            {
                vPhienHopInfo.PHONGHOP_ID = pPhongID;
                vDataContext.SubmitChanges();
            }
            var vPhongHopInfo = vPhongHopControllerInfo.GetPhongHopTheoId(pPhongID);
            vDuyetPhienHopController.XoaPhongDaChon(vPhienHopId);
            PHIENHOP_PHONGHOP pHIENHOP_PHONGHOP = new PHIENHOP_PHONGHOP();
            pHIENHOP_PHONGHOP.PHIENHOP_ID = pPhienHopID;
            pHIENHOP_PHONGHOP.PHONGHOP_ID = pPhongID;
            pHIENHOP_PHONGHOP.SODO_FILE = vPhongHopInfo.SODO_FILE;
            pHIENHOP_PHONGHOP.SODO_Text = vPhongHopInfo.SODO_Text;
            vDataContext.PHIENHOP_PHONGHOPs.InsertOnSubmit(pHIENHOP_PHONGHOP);
            vDataContext.SubmitChanges();
        }

        /// <summary>
        /// Load danh sách phiên họp đại biểu
        /// </summary>
        public void LoadDropDownThietBi()
        {
            ThietBiController vThietBiControllerInfo = new ThietBiController();
            try
            {
                string oErrorMessage = "";
                List<THIETBI> vDonViInfos = vThietBiControllerInfo.GetDanhSachThietBi("", 1, out oErrorMessage);
                ddlistThietBi.DataSource = vDonViInfos;
                ddlistThietBi.DataTextField = "TENTHIETBI";
                ddlistThietBi.DataValueField = "THIETBI_ID";


                ddlistThietBi.DataBind();
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Tìm kiếm phòng cho phiên họp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonTimKiem_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                vPhienHopId = int.Parse(Request.QueryString["id"]);
            }
            LoadDanhSachPhongHop(true);
            //ActiveTabChonPhong();
            ViewState["Tab"] = "PHONGHOP";

        }
        public void ActiveTabChonPhong()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay_ChonPhong();", true);
        }
        #endregion

        #region Thành phần tham dự Authur NHTTAI
        //Start - Phần chủ trì phiên họp - NHHAN bổ sung

        /// <summary>
        /// Chọn nhiều chủ trì cho phiên họp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmChonChuTri_Click(object sender, EventArgs e)
        {
            try
            {
                string vChuTris = hiddenfiendChonChuTri.Value;
                if (hiddenfiendChonChuTri.Value != "")
                {
                    lblChuTrisPhienHop.Text = vChuTris;
                    var vDaiBieus_Arr = vChuTris.Split(',');
                    List<int> ChuTri_ID_Select = new List<int>();
                    for (int i = 0; i < vDaiBieus_Arr.Count(); i++)
                    {
                        var CHUTRI = vDaiBieus_Arr[i].Split('_');
                        if (CHUTRI.Count() < 3)
                        {
                            continue;
                        }
                        else
                        {
                            ChuTri_ID_Select.Add(Int32.Parse(CHUTRI[2].ToString()));
                        }
                    }
                    List<HKG_THONGTINGUOIDUNG> NGUOIDUNGs = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => ChuTri_ID_Select.Contains(x.NGUOIDUNG_ID)).ToList();
                    if (NGUOIDUNGs.Count > 0)
                    {
                        lboxChuTri.DataSource = NGUOIDUNGs;
                        lboxChuTri.DataBind();
                    }
                    else
                    {
                        lboxChuTri.DataSource = null;
                        lboxChuTri.DataBind();
                        lblChuTrisPhienHop.Text = "";
                    }
                }
                else
                {
                    lboxChuTri.DataSource = null;
                    lboxChuTri.DataBind();
                    lblChuTrisPhienHop.Text = "";
                }
                ViewState["Tab"] = "THAMDU";
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Xoá chủ trì trong danh sách đã chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaChuTri(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vCHUTRIId = int.Parse(html.HRef);

                var NGUOIDUNG = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vCHUTRIId).FirstOrDefault();
                if (NGUOIDUNG != null)
                {
                    string vChuTri = lblChuTrisPhienHop.Text;
                    string key = NGUOIDUNG.DONVI_ID + "_" + NGUOIDUNG.PB_ID + "_" + NGUOIDUNG.NGUOIDUNG_ID;
                    vChuTri = vChuTri.Replace(key, "");
                    vChuTri = vChuTri.Replace(",,", ",");
                    if (vChuTri != "")
                    {
                        if (vChuTri[(vChuTri.Length - 1)] == ',')
                        {
                            vChuTri = vChuTri.Substring(0, (vChuTri.Length - 1));
                        }
                    }
                    lblChuTrisPhienHop.Text = vChuTri;
                    hiddenfiendChonChuTri.Value = vChuTri;
                    btnConfirmChonChuTri_Click(btnConfirmChonChuTri, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        //End - phần chủ trì phiên họp

        protected void btnConfirmChonDaiBieu_Click(object sender, EventArgs e)
        {
            string vDaiBieus = hiddenfieldChonDaiBieu.Value;
            if (hiddenfieldChonDaiBieu.Value != "")
            {
                lblDaiBieusPhienHop.Text = vDaiBieus;
                var vDaiBieus_Arr = vDaiBieus.Split(',');
                List<int> DaiBieu_ID_Select = new List<int>();
                for (int i = 0; i < vDaiBieus_Arr.Count(); i++)
                {
                    var DAIBIEU = vDaiBieus_Arr[i].Split('_');
                    if (DAIBIEU.Count() < 3)
                    {
                        continue;
                    }
                    else
                    {
                        DaiBieu_ID_Select.Add(Int32.Parse(DAIBIEU[2].ToString()));

                    }
                }
                List<HKG_THONGTINGUOIDUNG> NGUOIDUNGs = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => DaiBieu_ID_Select.Contains(x.NGUOIDUNG_ID)).ToList();
                if (NGUOIDUNGs.Count > 0)
                {
                    lboxDaiBieu.DataSource = NGUOIDUNGs;
                    lboxDaiBieu.DataBind();
                }
                else
                {
                    lboxDaiBieu.DataSource = null;
                    lboxDaiBieu.DataBind();
                    lblDaiBieusPhienHop.Text = "";
                }
            }
            else
            {
                lboxDaiBieu.DataSource = null;
                lboxDaiBieu.DataBind();
                lblDaiBieusPhienHop.Text = "";
            }
            ViewState["Tab"] = "THAMDU";
        }
        protected void btnConfirmChonThuKy_Click(object sender, EventArgs e)
        {
            string vThuKys = hiddenfieldChonThuKy.Value;
            if (hiddenfieldChonThuKy.Value != "")
            {
                lblThuKysPhienHop.Text = vThuKys;
                var vThuKys_Arr = vThuKys.Split(',');
                List<int> ThuKy_ID_Select = new List<int>();
                for (int i = 0; i < vThuKys_Arr.Count(); i++)
                {
                    var THUKY = vThuKys_Arr[i].Split('_');
                    if (THUKY.Count() < 3)
                    {
                        continue;
                    }
                    else
                    {
                        ThuKy_ID_Select.Add(Int32.Parse(THUKY[2].ToString()));

                    }
                }
                List<HKG_THONGTINGUOIDUNG> NGUOIDUNGs = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => ThuKy_ID_Select.Contains(x.NGUOIDUNG_ID)).ToList();
                if (NGUOIDUNGs.Count > 0)
                {
                    lboxThuKy.DataSource = NGUOIDUNGs;
                    lboxThuKy.DataBind();
                }
                else
                {
                    lboxThuKy.DataSource = null;
                    lboxThuKy.DataBind();
                    lblThuKysPhienHop.Text = "";
                }
            }
            else
            {
                lboxThuKy.DataSource = null;
                lboxThuKy.DataBind();
                lblThuKysPhienHop.Text = "";
            }
            ViewState["Tab"] = "THAMDU";


        }

        protected void btnConfirmChonKhachMoi_Click(object sender, EventArgs e)
        {
            string vKhachMois = hiddenfieldChonKhachMoi.Value;
            if (hiddenfieldChonKhachMoi.Value != "")
            {
                lblKhachmoisPhienHop.Text = vKhachMois;
                var vKhachMois_Arr = vKhachMois.Split(',');
                List<int> KhachMoi_ID_Select = new List<int>();
                for (int i = 0; i < vKhachMois_Arr.Count(); i++)
                {
                    var KHACHMOI = vKhachMois_Arr[i].Split('_');
                    if (KHACHMOI.Count() < 3)
                    {
                        continue;
                    }
                    else
                    {
                        KhachMoi_ID_Select.Add(Int32.Parse(KHACHMOI[2].ToString()));

                    }
                }
                List<HKG_THONGTINKHACHMOI> KHAHMOIs = vDataContext.HKG_THONGTINKHACHMOIs.Where(x => KhachMoi_ID_Select.Contains(x.NGUOIDUNG_ID)).ToList();
                if (KHAHMOIs.Count > 0)
                {
                    lboxKhachMoi.DataSource = KHAHMOIs;
                    lboxKhachMoi.DataBind();
                }
                else
                {
                    lboxKhachMoi.DataSource = null;
                    lboxKhachMoi.DataBind();
                }
            }
            else
            {
                lboxKhachMoi.DataSource = null;
                lboxKhachMoi.DataBind();
            }
            ViewState["Tab"] = "THAMDU";

        }


        /// <summary>
        /// Xóa đại biểu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaDaiBieu(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vDAIBIEUId = int.Parse(html.HRef);

                var NGUOIDUNG = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vDAIBIEUId).FirstOrDefault();
                if (NGUOIDUNG != null)
                {
                    string vDaiBieus = lblDaiBieusPhienHop.Text;
                    string key = NGUOIDUNG.DONVI_ID + "_" + NGUOIDUNG.PB_ID + "_" + NGUOIDUNG.NGUOIDUNG_ID;
                    vDaiBieus = vDaiBieus.Replace(key, "");
                    vDaiBieus = vDaiBieus.Replace(",,", ",");
                    if (vDaiBieus != "")
                    {
                        if (vDaiBieus[(vDaiBieus.Length - 1)] == ',')
                        {
                            vDaiBieus = vDaiBieus.Substring(0, (vDaiBieus.Length - 1));
                        }
                    }

                    lblDaiBieusPhienHop.Text = vDaiBieus;
                    hiddenfieldChonDaiBieu.Value = vDaiBieus;
                    btnConfirmChonDaiBieu_Click(btnConfirmChonDaiBieu, new EventArgs());
                }


            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }

        }


        /// <summary>
        /// Xóa thư ký
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaThuKy(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vTHUKYId = int.Parse(html.HRef);
                var NGUOIDUNG = vDataContext.HKG_THONGTINGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vTHUKYId).FirstOrDefault();
                if (NGUOIDUNG != null)
                {
                    string vThuKys = lblThuKysPhienHop.Text;
                    string key = NGUOIDUNG.DONVI_ID + "_" + NGUOIDUNG.PB_ID + "_" + NGUOIDUNG.NGUOIDUNG_ID;
                    vThuKys = vThuKys.Replace(key, "");
                    vThuKys = vThuKys.Replace(",,", ",");
                    if (vThuKys != "")
                    {
                        if (vThuKys[(vThuKys.Length - 1)] == ',')
                        {
                            vThuKys = vThuKys.Substring(0, (vThuKys.Length - 1));
                        }
                    }
                    lblThuKysPhienHop.Text = vThuKys;
                    hiddenfieldChonThuKy.Value = vThuKys;
                    btnConfirmChonThuKy_Click(btnConfirmChonThuKy, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }

        }


        /// <summary>
        /// Xóa khách mời
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaKhachMoi(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vKHACHMOIId = int.Parse(html.HRef);
                var KHACHMOI = vDataContext.HKG_THONGTINKHACHMOIs.Where(x => x.NGUOIDUNG_ID == vKHACHMOIId).FirstOrDefault();
                if (KHACHMOI != null)
                {
                    string vKhachMois = lblKhachmoisPhienHop.Text;
                    string key = KHACHMOI.DONVI_ID + "_" + KHACHMOI.PB_ID + "_" + KHACHMOI.NGUOIDUNG_ID;
                    vKhachMois = vKhachMois.Replace(key, "");
                    vKhachMois = vKhachMois.Replace(",,", ",");
                    if (vKhachMois[(vKhachMois.Length - 1)] == ',')
                    {
                        vKhachMois = vKhachMois.Substring(0, (vKhachMois.Length - 1));
                    }
                    lblKhachmoisPhienHop.Text = vKhachMois;
                    hiddenfieldChonKhachMoi.Value = vKhachMois;
                    btnConfirmChonKhachMoi_Click(btnConfirmChonKhachMoi, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }

        }
        #endregion


    }
}
