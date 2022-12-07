#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật phiên họp
/// Ngày tại        :08/03/2019
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace HOPKHONGGIAY
{
    public partial class CnDuyetPhienHop : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        int vPageSize = ClassParameter.vPageSize;
        DataTable dtTable;
        int vCurentPage = 0;
        CauHoiController vCauHoiControllerInfo = new CauHoiController();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        public string vPathCommonUploadFile = ClassParameter.vPathCommonUploadTaiLieuHop;
        public string vPathCommonUploadFileBienBan = ClassParameter.vPathCommonUploadBienBanHop;
        public string vPathCommonUploadFileKetLuan = ClassParameter.vPathCommonUploadKetLuan;

        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();

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
                    LoadDanhSachChuTri();
                    LoadDanhSachDonVi();
                    LoadDanhSach(vCurentPage);
                    SetFormInfo(vPhienHopId);
                    SetInfoSoDo(vPhienHopId, false);
                    LoadDSFile(vPhienHopId);
                    LoadDanhSachGhe();
                    LoadDSFile_BIENBAN(vPhienHopId);
                    LoadDSFile_KetLuan(vPhienHopId);
                    //textTenKhachMoi.Focus();                   
                }
                //Edit Title
                if (vPhienHopId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý duyệt phiên họp</a> / Thêm mới";
                }
                else
                {
                    var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                    if (vPhienHopInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý duyệt phiên họp</a> / " + vPhienHopInfo.TIEUDE;
                    }

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
                CapNhat(vPhienHopId);
            }
        }


        /// <summary>
        /// Event nhan button Bo Qua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBoQua_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Chi tiết phiên họp", "id=" + vPhienHopId, "caching="+ DateTime.Now.ToString());
            Response.Redirect(vUrl);
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


            if (ddlistChuTri.SelectedValue == "")
            {
                ddlistChuTri.CssClass += " vld";
                ddlistChuTri.Focus();
                labelChuTri.Attributes["class"] += " vld";
                vToastrMessage += "Chủ trì, ";
                vResult = false;
            }
            else
            {
                ddlistChuTri.CssClass = ddlistChuTri.CssClass.Replace("vld", "").Trim();
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
                if (pPhienHopId > 0)//Cập nhật
                {
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).FirstOrDefault();
                    if (vPhienHopInfo != null)
                    {
                        textTieuDe.Text = vPhienHopInfo.TIEUDE;
                        textChuongTrinhHop.Text = vPhienHopInfo.NOIDUNG;//.Replace("\n", "<br/>");//Server.HtmlDecode();

                        ddlistDonVi.SelectedValue = vPhienHopInfo.DONVI_ID.ToString();
                        dtpickerThoiGianBatDau.SelectedDate = vPhienHopInfo.THOIGIANBATDAU;
                        dtpickerThoiGianKetThuc.SelectedDate = vPhienHopInfo.THOIGIANKETTHUC;
                        textGhiChu.Text = vPhienHopInfo.GHICHU;
                        int vChuTriId = 0;
                        vChuTriId = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.LOAI_DAIBIEU == 1 && x.PHIENHOP_ID == pPhienHopId).FirstOrDefault().NGUOIDUNG_ID;
                        if (vChuTriId > 0)
                            ddlistChuTri.SelectedValue = vChuTriId.ToString();
                        // Authur NHHAN
                        //int vThuKyId = 0;
                        //vThuKyId = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.LOAI_DAIBIEU == 3 && x.PHIENHOP_ID == pPhienHopId).FirstOrDefault().NGUOIDUNG_ID;
                        //if (vChuTriId > 0)
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
                        List<HKG_DAIBIEU> HKG_THUKYs = vDataContext.HKG_DAIBIEUs.Where(x => x.LOAI_DAIBIEU == 3 && x.PHIENHOP_ID == pPhienHopId).ToList();
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
                        List<HKG_DAIBIEU> HKG_DAIBIEUs = vDataContext.HKG_DAIBIEUs.Where(x => x.LOAI_DAIBIEU == 2 && x.PHIENHOP_ID == pPhienHopId).ToList();
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
                        //Authur NHTTAI
                        List<HKG_KHACHMOI> HKG_KHACHMOIs = vDataContext.HKG_KHACHMOIs.Where(x => x.PHIENHOP_ID == pPhienHopId).ToList();

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
                    }

                    // Nếu trường hợp đã thêm phòng họp cho phiên họp thì không chọn lại thời gian
                    var objPHIENHOP_PHONGHOP = vDataContext.PHIENHOP_PHONGHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).ToList();
                    if (objPHIENHOP_PHONGHOP.Count > 0)
                    {
                        dtpickerThoiGianBatDau.Enabled = false;
                        dtpickerThoiGianKetThuc.Enabled = false;
                    }
                    else
                    {
                        dtpickerThoiGianBatDau.Enabled = true;
                        dtpickerThoiGianKetThuc.Enabled = true;
                    }

                    if (vPhienHopInfo.TRANGTHAI != 4 && vPhienHopInfo.TRANGTHAI != 5)
                    {
                        //liBienBanHop.Visible = false;
                        //liKetLuan.Visible = false;                      
                    }
                    else
                    {
                        textTieuDe.Enabled = false;
                        ddlistDonVi.Enabled = false;
                        dtpickerThoiGianBatDau.Enabled = false;
                        dtpickerThoiGianKetThuc.Enabled = false;
                        //liThanhPhan.Visible = false;
                        //liChuongTrinhHop.Visible = false;
                        //liTaiLieuHop.Visible = false;
                    }
                    //Sơ đồ vị trí

                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] == null)
                    {
                        var vPhienHopNguoiDungCoViTri = vDataContext.PRO_PHIENHOP_VITRI_WEB(pPhienHopId);
                        var vPhienHopVTInfo = vPhienHopNguoiDungCoViTri.Where(x => x.MAGHE != null && x.MAGHE != "").ToList();
                        if (vPhienHopVTInfo.Count() > 0)
                        {
                            List<string> KeyKey = new List<string>();
                            foreach (var vPHVT in vPhienHopVTInfo)
                            {
                                var vString = vPHVT.ID.ToString() + "|" + vPHVT.MAGHE + "|" + vPHVT.LOAI;
                                KeyKey.Add(vString);
                            }
                            ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] = KeyKey;
                        }
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception Ex)
            {

            }
        }

        /// <summary>
        /// Cập nhật thông tin phiên họp
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void CapNhat(int pPhienHopId)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string vErrorMessage = "";
                var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).SingleOrDefault();
                if (vPhienHopInfo != null)
                {
                    //Cập nhật phiên họp
                    vPhienHopInfo.NOIDUNG = ClassCommon.RemoveJavascript(HttpUtility.HtmlDecode(textChuongTrinhHop.Text));

                    if (textChuongTrinhHop.Text != null && textChuongTrinhHop.Text != "")
                    {
                        vPhienHopInfo.THOIGIANKETLUAN = DateTime.Now;
                    }
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
                    vDataContext.SubmitChanges();

                    Update_ThanhPhanThamDu(vPhienHopId);

                    #region Thành phần than dự old
                    //Chủ trì
                    //Thêm mới nhiều chủ trì                
                    //Authur NHHAN
                    //List<Int32> vIdChuTri = new List<int>();
                    //if (lboxChuTri.DataKeys.Count > 0)
                    //{
                    //    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    //    for (int i = 0; i < lboxChuTri.DataKeys.Count; i++)
                    //    {
                    //        vIdChuTri.Add(Int32.Parse(lboxChuTri.DataKeys[i].ToString()));
                    //        int vId = Int32.Parse(lboxChuTri.DataKeys[i].ToString());
                    //        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                    //        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ChuTri;//1: Chủ trì; 2: Đại biểu; 3: Thư ký
                    //        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                    //        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                    //        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                    //        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);
                    //    }

                    //    // ************* Mới***************               
                    //    // Get chủ trì ID
                    //    List<int> Lst_TK_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).Select(x => x.NGUOIDUNG_ID).ToList();

                    //    // List người dùng có trong Database nhưng không được chọn  Delete
                    //    List<int> lst_TK_FromlboxChuTri = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    //    // List người dùng cần xóa
                    //    List<int> Lst_TK_CANXOA = Lst_TK_ID.Except(lst_TK_FromlboxChuTri).ToList();
                    //    var obj_TK_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_TK_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).ToList();
                    //    if (obj_TK_XOA.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_TK_XOA);
                    //        vDataContext.SubmitChanges();
                    //    }

                    //    // List người dùng không tồn tại trong database Insert
                    //    List<PHIENHOP_NGUOIDUNG> objChuTri_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_TK_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    //    if (objChuTri_OUTDATABASE.Count > 0)
                    //    {
                    //        foreach (var obj in objChuTri_OUTDATABASE)
                    //        {
                    //            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                    //            if (objNguoiDungPhienHop != null)
                    //            {
                    //                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ChuTri;
                    //                vDataContext.SubmitChanges();
                    //                objChuTri_OUTDATABASE.Remove(obj);
                    //            }
                    //        }
                    //        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objChuTri_OUTDATABASE);
                    //    }
                    //    //************** End **************

                    //}
                    //else
                    //{
                    //    var vPhienHopChuTriInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri).ToList();
                    //    if (vPhienHopChuTriInfos != null && vPhienHopChuTriInfos.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopChuTriInfos);
                    //        vDataContext.SubmitChanges();
                    //    }
                    //}

                    ////Authur NHTTAI
                    //List<Int32> vID_THUKYs = new List<int>();
                    //if (lboxThuKy.DataKeys.Count > 0)
                    //{
                    //    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    //    for (int i = 0; i < lboxThuKy.DataKeys.Count; i++)
                    //    {
                    //        vID_THUKYs.Add(Int32.Parse(lboxThuKy.DataKeys[i].ToString()));
                    //        int vId = Int32.Parse(lboxThuKy.DataKeys[i].ToString());

                    //        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                    //        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = 3;//1: Chủ trì; 2: Đại biểu; 3: Thư ký
                    //        vPhienHop_NguoiDungInfo.PHIENHOP_ID = vPhienHopId;
                    //        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                    //        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                    //        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);
                    //    }


                    //    // ************* Mới***************               
                    //    // Get thư ký ID
                    //    List<int> Lst_TK_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).Select(x => x.NGUOIDUNG_ID).ToList();

                    //    // List người dùng có trong Database nhưng không được chọn  Delete
                    //    List<int> lst_TK_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    //    // List người dùng cần xóa
                    //    List<int> Lst_TK_CANXOA = Lst_TK_ID.Except(lst_TK_FromlboxDaiBieu).ToList();
                    //    var obj_TK_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_TK_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).ToList();
                    //    if (obj_TK_XOA.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_TK_XOA);
                    //        vDataContext.SubmitChanges();
                    //    }                    

                    //    // List người dùng không tồn tại trong database Insert
                    //    List<PHIENHOP_NGUOIDUNG> objTHUKY_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_TK_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    //    if (objTHUKY_OUTDATABASE.Count > 0)
                    //    {
                    //        foreach (var obj in objTHUKY_OUTDATABASE)
                    //        {
                    //            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                    //            if (objNguoiDungPhienHop != null)
                    //            {
                    //                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.ThuKy;
                    //                vDataContext.SubmitChanges();
                    //                objTHUKY_OUTDATABASE.Remove(obj);
                    //            }
                    //        }
                    //        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objTHUKY_OUTDATABASE);
                    //    }
                    //    //************** End **************


                    //}
                    //else
                    //{
                    //    var vPhienHopThuKyInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ThuKy).ToList();
                    //    if (vPhienHopThuKyInfos != null && vPhienHopThuKyInfos.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopThuKyInfos);
                    //        vDataContext.SubmitChanges();
                    //    }

                    //}
                    ////Đại biểu
                    //if (lboxDaiBieu.DataKeys.Count > 0)
                    //{
                    //    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    //    for (int i = 0; i < lboxDaiBieu.DataKeys.Count; i++)
                    //    {
                    //        int vId = Int32.Parse(lboxDaiBieu.DataKeys[i].ToString());

                    //        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                    //        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = 2;//1: Chủ trì; 2: Đại biểu; 3: Thư ký
                    //        vPhienHop_NguoiDungInfo.PHIENHOP_ID = vPhienHopId;
                    //        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                    //        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                    //        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);

                    //    }

                    //    // ************* Mới***************               
                    //    // Get người dùng ID
                    //    List<int> Lst_ND_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).Select(x => x.NGUOIDUNG_ID).ToList();

                    //    // List người dùng có trong Database nhưng không được chọn  Delete
                    //    List<int> lst_ND_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    //    // List người dùng cần xóa
                    //    List<int> Lst_ND_CANXOA = Lst_ND_ID.Except(lst_ND_FromlboxDaiBieu).ToList();
                    //    var obj_ND_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_ND_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).ToList();
                    //    if (obj_ND_XOA.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_ND_XOA);
                    //        vDataContext.SubmitChanges();
                    //    }

                    //    // List người dùng không tồn tại trong database Insert
                    //    List<PHIENHOP_NGUOIDUNG> objNGUOIDUNG_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_ND_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    //    if (objNGUOIDUNG_OUTDATABASE.Count > 0)
                    //    {
                    //        foreach (var obj in objNGUOIDUNG_OUTDATABASE)
                    //        {
                    //            var objNguoiDungPhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.NGUOIDUNG_ID == obj.NGUOIDUNG_ID).FirstOrDefault();
                    //            if (objNguoiDungPhienHop != null)
                    //            {
                    //                objNguoiDungPhienHop.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.DaiBieu;
                    //                vDataContext.SubmitChanges();
                    //                objNGUOIDUNG_OUTDATABASE.Remove(obj);
                    //            }
                    //        }
                    //        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objNGUOIDUNG_OUTDATABASE);
                    //    }
                    //    //************** End **************                     
                    //}
                    //else //Xoá tất cả đại biểu trong phiên họp - Hận
                    //{
                    //    var vPhienHopNguoiDungInfos_Delete = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.DaiBieu).ToList();
                    //    if (vPhienHopNguoiDungInfos_Delete != null && vPhienHopNguoiDungInfos_Delete.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopNguoiDungInfos_Delete);
                    //        vDataContext.SubmitChanges();
                    //    }
                    //}

                    ////Khách mời new
                    //if (lboxKhachMoi.DataKeys.Count > 0)
                    //{
                    //    List<PHIENHOP_NGUOIDUNG> vPhienHop_NguoiDungInfos = new List<PHIENHOP_NGUOIDUNG>();
                    //    for (int i = 0; i < lboxKhachMoi.DataKeys.Count; i++)
                    //    {
                    //        int vId = Int32.Parse(lboxKhachMoi.DataKeys[i].ToString());

                    //        PHIENHOP_NGUOIDUNG vPhienHop_NguoiDungInfo = new PHIENHOP_NGUOIDUNG();
                    //        vPhienHop_NguoiDungInfo.LOAI_DAIBIEU = (int)CommonEnum.LoaiDaiBieu.KhachMoi;//1: Chủ trì; 2: Đại biểu; 3: Thư ký; 4: Khách mời
                    //        vPhienHop_NguoiDungInfo.PHIENHOP_ID = pPhienHopId;
                    //        vPhienHop_NguoiDungInfo.NGUOIDUNG_ID = vId;
                    //        vPhienHop_NguoiDungInfo.NHANTHONGBAO = true;
                    //        vPhienHop_NguoiDungInfos.Add(vPhienHop_NguoiDungInfo);

                    //    }

                    //    // ************* Mới***************               
                    //    // Get người dùng ID
                    //    List<int> Lst_ND_ID = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).Select(x => x.NGUOIDUNG_ID).ToList();

                    //    // List người dùng có trong Database nhưng không được chọn  Delete
                    //    List<int> lst_ND_FromlboxDaiBieu = vPhienHop_NguoiDungInfos.Select(x => x.NGUOIDUNG_ID).ToList();
                    //    // List người dùng cần xóa
                    //    //List<int> Lst_ND_CANXOA = Lst_ND_ID.Intersect(lst_ND_FromlboxDaiBieu).ToList();
                    //    List<int> Lst_ND_CANXOA = Lst_ND_ID.Except(lst_ND_FromlboxDaiBieu).ToList();
                    //    var obj_ND_XOA = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && Lst_ND_CANXOA.Contains(x.NGUOIDUNG_ID) && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).ToList();
                    //    if (obj_ND_XOA.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(obj_ND_XOA);
                    //        vDataContext.SubmitChanges();
                    //    }

                    //    // List người dùng không tồn tại trong database Insert
                    //    List<PHIENHOP_NGUOIDUNG> objNGUOIDUNG_OUTDATABASE = vPhienHop_NguoiDungInfos.Where(x => !Lst_ND_ID.Contains(x.NGUOIDUNG_ID)).ToList();
                    //    if (objNGUOIDUNG_OUTDATABASE.Count > 0)
                    //    {
                    //        vPhienHopNguoiDungControllerInfo.ThemNhieuPhienHopNguoiDung(objNGUOIDUNG_OUTDATABASE);
                    //    }

                    //    //************** End **************                
                    //}
                    //else
                    //{
                    //    var vPhienHopDaiBieuInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == pPhienHopId && x.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.KhachMoi).ToList();
                    //    if (vPhienHopDaiBieuInfos != null && vPhienHopDaiBieuInfos.Count > 0)
                    //    {
                    //        vDataContext.PHIENHOP_NGUOIDUNGs.DeleteAllOnSubmit(vPhienHopDaiBieuInfos);
                    //        vDataContext.SubmitChanges();
                    //    }
                    //}
                    #endregion


                    //Cập nhật vị trí chổ ngồi
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] != null)
                    {
                        List<string> KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"];
                        //Clear vị trí chổ ngồi cũ
                        var vPhienHopNguoiDungInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == vPhienHopId).ToList();
                        //var vPhienHopKhachMoiInfos = vDataContext.PHIENHOP_KHACHMOIs.Where(x => x.PHIENHOP_ID == vPhienHopId).ToList();
                        if (vPhienHopNguoiDungInfos != null && vPhienHopNguoiDungInfos.Count > 0)
                        {
                            foreach (var vPhienHopNguoiDungInfo in vPhienHopNguoiDungInfos)
                            {
                                vPhienHopNguoiDungInfo.MAGHE = "";
                            }
                            vDataContext.SubmitChanges();
                        }                       

                        //Set lại vị trí chổ ngồi mới
                        for (int i = 0; i < KeyKey.Count; i++)
                        {
                            var vContent = KeyKey[i].Split('|');
                            int vId = int.Parse(vContent[0]);
                            if (vContent[2] == "daibieu")
                            {
                                var vPhienHopNguoiDungInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
                                vPhienHopNguoiDungInfo.MAGHE = vContent[1];
                                vDataContext.SubmitChanges();
                            }
                            else
                            {
                                if (vContent[2] == "khachmoi")
                                {
                                    var vPhienHopNguoiDungInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
                                    vPhienHopNguoiDungInfo.MAGHE = vContent[1];
                                    vDataContext.SubmitChanges();
                                }
                            }
                        }
                        SetInfoSoDo(vPhienHopId, true);
                        ClassCommon.ShowToastr(Page, "Cập nhật vị trí chổ ngồi thành công", "Thông báo", "success");
                    }


                    
                    Session[vMacAddress + TabId.ToString() + "_Message"] = "Cập nhật thông tin phiên họp thành công";
                    Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                    //ClassCommon.ShowToastr(Page, "Cập nhật thông tin phiên họp thành công", "Thông báo", "success");
                    string vUrl = Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Chi tiết phiên họp", "id=" + vPhienHopId);
                    Response.Redirect(vUrl,false);
                }
                else
                {
                    ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
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
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Load danh sách đại biểu
        /// </summary>
        /// <returns></returns>
        public void LoadDanhSachChuTri()
        {
            try
            {
                List<NGUOIDUNG> vNguoiDungInfos = new List<NGUOIDUNG>();
                vNguoiDungInfos = vDataContext.NGUOIDUNGs.ToList();
                DataTable vDataTable = new DataTable();
                vDataTable.Columns.Add("NGUOIDUNG_ID");
                vDataTable.Columns.Add("TENNGUOIDUNG");
                if (vNguoiDungInfos != null)
                {
                    foreach (var vNguoiDungInfo in vNguoiDungInfos)
                    {
                        DataRow vDataRow = vDataTable.NewRow();
                        vDataRow["NGUOIDUNG_ID"] = vNguoiDungInfo.NGUOIDUNG_ID;
                        vDataRow["TENNGUOIDUNG"] = vNguoiDungInfo.DONVI.TENVIETTAT + " - " + vNguoiDungInfo.TENNGUOIDUNG;
                        vDataTable.Rows.Add(vDataRow);
                    }


                    ddlistChuTri.Items.Clear();
                    ddlistChuTri.DataSource = vDataTable;
                    ddlistChuTri.DataTextField = "TENNGUOIDUNG";
                    ddlistChuTri.DataValueField = "NGUOIDUNG_ID";
                    ddlistChuTri.DataBind();
                    ddlistChuTri.Items.Insert(0, new ListItem("Chọn chủ trì", ""));

                }
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
                        row["DOMAT"] = (it.DOMAT == true ? "Mật" : "Không");
                        row["NHOM"] = (it.TL_NHOM);
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
                        row["HA_FILE_PATH"] = (it.FILE_NAME);
                        row["HA_ID"] = (it.TAILIEU_ID);
                        row["HA_TENFILE"] = it.FILE_MOTA;
                        row["HA_EXT"] = it.FILE_EXT;
                        row["HA_SIZE"] = it.FILE_SIZE;
                        dtTable.Rows.Add(row);
                    }
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
        protected void ThayDoiTrangThai(object sender, EventArgs e)
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
                    ClassCommon.ShowToastr(Page, "Cập nhật mức độ phổ biến của tài liệu: " + vTaiLieuInfo.TEN + " thành công", "Thông báo", "Success");
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
                var vQuyenTaiLieuInfos = vDataContext.QUYEN_TAILIEUs.Where(x => x.TAILIEU_ID == vFile_ID).ToList();
                vDataContext.QUYEN_TAILIEUs.DeleteAllOnSubmit(vQuyenTaiLieuInfos);
                vDataContext.SubmitChanges();
                vDataContext.TAILIEUs.DeleteOnSubmit(vTapTinInfo);
                vDataContext.SubmitChanges();
                LoadDSFile(vPhienHopId);
                ClassCommon.ShowToastr(Page, "Xoá tài liệu họp thành công", "Thông báo", "success");
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
                    //for (int i = 0; i < uploadedFiles.Count; i++)
                    //{
                    HttpPostedFile userPostedFile = uploadedFiles[2];
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
                                vTaiLieuInfo.TL_NHOM = ClassCommon.ClearHTML(textNhomTaiLieu.Text.Trim());
                                vTaiLieuInfo.NGAYTAO = DateTime.Now;
                                vTaiLieuInfo.OBJECT_LOAI = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                vTaiLieuInfo.OBJECT_ID = vPhienHopId;

                                vTaiLieuInfo.PHIENHOP_ID = vPhienHopId;
                                vTaiLieuInfo.TEN = ClassCommon.ClearHTML(textTenTaiLieu.Text.Trim());
                                vTaiLieuInfo.MOTA = ClassCommon.ClearHTML(textMotaFile.Text.Trim());
                                vTaiLieuInfo.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuHop;
                                vTaiLieuInfo.TRANGTHAI = 1;
                                vTaiLieuInfo.TAILIEUCHUNG = true;
                                vTaiLieuInfo.DOMAT = false;
                                vTaiLieuInfo.UserId = _currentUser.UserID;

                                vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                                vDataContext.SubmitChanges();

                                List<QUYEN_TAILIEU> vQuyenTaiLieuInfos = new List<QUYEN_TAILIEU>();
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

                                var vTaiLieuId_New =  vDataContext.TAILIEUs.OrderByDescending(x => x.TAILIEU_ID).Select(x => x.TAILIEU_ID).FirstOrDefault();
                                if(vTaiLieuId_New > 0)
                                {
                                    vQuyenTaiLieuInfos.ForEach(x => x.TAILIEU_ID = vTaiLieuId_New);
                                    vDataContext.QUYEN_TAILIEUs.InsertAllOnSubmit(vQuyenTaiLieuInfos);
                                    vDataContext.SubmitChanges();
                                }

                                textTenTaiLieu.Text = "";
                                textMotaFile.Text = "";

                                LoadDSFile(vPhienHopId);
                                ClassCommon.ShowToastr(Page, "Tải lên tài liệu họp thành công", "Thông báo", "success");
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
                    //}
                }
            }
            callModalScript();


        }



        public void callModalScript()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay();", true);
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
                else
                {


                }
            }
            dgDanhSach_FileBienBan.DataSource = dtTable;
            dgDanhSach_FileBienBan.DataBind();
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


        protected void TaiLenBienBan(object sender, EventArgs e)
        {
            //if (dgDanhSach.Rows.Count < 1)
            //{

            if (!f_File_BienBan.HasFile)
            {
                ClassCommon.ShowToastr(Page, "Vui lòng chọn tài liệu", "Thông báo lỗi", "error");
                f_File_BienBan.CssClass += " vld";
                f_File_BienBan.Focus();
                labelFileBienBan.Attributes["class"] += " vld";
            }
            else
            {
                f_File_BienBan.CssClass = f_File_BienBan.CssClass.Replace("vld", "").Trim();
                labelFileBienBan.Attributes.Add("class", labelFileBienBan.Attributes["class"].ToString().Replace("vld", ""));
                string filepath = Server.MapPath(vPathCommonUploadFileBienBan);
                HttpFileCollection uploadedFiles = Request.Files;
                //for (int i = 0; i < uploadedFiles.Count; i++)
                //{
                HttpPostedFile userPostedFile = uploadedFiles[0];
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
                            vTaiLieuInfo.OBJECT_LOAI = (int)CommonEnum.TapTinObjectLoai.TaiLieuBienBanHop;
                            vTaiLieuInfo.OBJECT_ID = vPhienHopId;

                            vTaiLieuInfo.PHIENHOP_ID = vPhienHopId;
                            vTaiLieuInfo.TEN = filename;
                            vTaiLieuInfo.MOTA = filename;
                            vTaiLieuInfo.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuBienBanHop;
                            vTaiLieuInfo.TRANGTHAI = 1;
                            vTaiLieuInfo.TAILIEUCHUNG = true;
                            vTaiLieuInfo.DOMAT = false;
                            vTaiLieuInfo.UserId = _currentUser.UserID;

                            vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                            vDataContext.SubmitChanges();
                            textTenTaiLieu.Text = "";
                            textMotaFile.Text = "";

                            LoadDSFile_BIENBAN(vPhienHopId);
                            ClassCommon.ShowToastr(Page, "Tài lên biên bản họp thành công", "Thông báo lỗi", "success");
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
                //}

            }
            callModalScript_BienBan();
        }


        public void callModalScript_BienBan()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay_BienBan();", true);
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
            dgDanhSach_FileKetLuan.DataSource = dtTable;
            dgDanhSach_FileKetLuan.DataBind();
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

        /// <summary>
        /// Tài lên tài liệu kết luận
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TaiLenTaiLieuKetLuan(object sender, EventArgs e)
        {
            //if (dgDanhSach.Rows.Count < 1)
            //{

            if (!f_File_KetLuan.HasFile)
            {
                ClassCommon.ShowToastr(Page, "Vui lòng chọn tài liệu", "Thông báo lỗi", "error");
                f_File_KetLuan.CssClass += " vld";
                f_File_KetLuan.Focus();
                labelFileKetLuan.Attributes["class"] += " vld";
            }
            else
            {
                f_File_KetLuan.CssClass = f_File_KetLuan.CssClass.Replace("vld", "").Trim();
                labelFileKetLuan.Attributes.Add("class", labelFileKetLuan.Attributes["class"].ToString().Replace("vld", ""));
                string filepath = Server.MapPath(vPathCommonUploadFileKetLuan);
                HttpFileCollection uploadedFiles = Request.Files;
                //for (int i = 0; i < uploadedFiles.Count; i++)
                //{
                HttpPostedFile userPostedFile = uploadedFiles[1];
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
                            vTaiLieuInfo.OBJECT_LOAI = (int)CommonEnum.TapTinObjectLoai.TaiLieuKetLuan;
                            vTaiLieuInfo.OBJECT_ID = vPhienHopId;

                            vTaiLieuInfo.PHIENHOP_ID = vPhienHopId;
                            vTaiLieuInfo.TEN = filename;
                            vTaiLieuInfo.MOTA = filename;
                            vTaiLieuInfo.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuKetLuan;
                            vTaiLieuInfo.TRANGTHAI = 1;
                            vTaiLieuInfo.TAILIEUCHUNG = true;
                            vTaiLieuInfo.DOMAT = false;
                            vTaiLieuInfo.UserId = _currentUser.UserID;

                            vDataContext.TAILIEUs.InsertOnSubmit(vTaiLieuInfo);
                            vDataContext.SubmitChanges();
                            textTenTaiLieu.Text = "";
                            textMotaFile.Text = "";

                            LoadDSFile_KetLuan(vPhienHopId);
                            ClassCommon.ShowToastr(Page, "Tải lên tài liệu kết luận cuộc họp thành công", "Thông báo lỗi", "success");
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
                //}
            }
            callModalScript_KetLuan();
        }


        public void callModalScript_KetLuan()
        {
            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "alert", "Isdisplay_KetLuan();", true);
        }

        #endregion

        #region Điểm danh


        protected void LoadDanhSach(int pCurentPage)
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
                vDiemDanhInfo = vDiemDanhInfo.Where(x => (x.TEN.ToLower().Contains(vContentSearch) || x.TENDONVI.ToLower().Contains(vContentSearch) || x.TENCHUCVU.ToLower().Contains(vContentSearch) || x.LOAI.ToLower().Contains(vContentSearch))).ToList();
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
                dgDanhSach.DataSource = vDiemDanhInfo;
                dgDanhSach.VirtualItemCount = Count;
                dgDanhSach.PageSize = vPageSize;
                dgDanhSach.CurrentPageIndex = pCurentPage;
                dgDanhSach.DataBind();
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ trị", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Điểm danh đại biểu hoặc khách mời
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DiemDanh(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                string[] vData = html.HRef.Split('/');
                int vId = int.Parse(vData[0]);
                string vLoai = vData[1];
                if (vLoai == "daibieu")
                {
                    var vDaiBieuInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                    if (vDaiBieuInfo != null)
                    {
                        if (vDaiBieuInfo.XACNHANTHAMGIA == null)
                        {
                            vDaiBieuInfo.XACNHANTHAMGIA = true;
                            vDaiBieuInfo.THAMDU = true;
                            vDataContext.SubmitChanges();
                            ClassCommon.ShowToastr(Page, "Điểm danh cho đại biểu " + vDaiBieuInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                            LoadDanhSach(vCurentPage);
                        }
                        else
                        {
                            if (vDaiBieuInfo.XACNHANTHAMGIA == false)
                            {
                                vDaiBieuInfo.XACNHANTHAMGIA = true;
                                vDaiBieuInfo.THAMDU = true;
                                vDataContext.SubmitChanges();
                                ClassCommon.ShowToastr(Page, "Điểm danh cho đại biểu " + vDaiBieuInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                                LoadDanhSach(vCurentPage);
                            }
                            else
                            {
                                vDaiBieuInfo.XACNHANTHAMGIA = false;
                                vDaiBieuInfo.THAMDU = false;
                                vDataContext.SubmitChanges();
                                ClassCommon.ShowToastr(Page, "Bỏ điểm danh cho đại biểu " + vDaiBieuInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                                LoadDanhSach(vCurentPage);
                            }
                        }
                        ViewState["Tab"] = "DIEMDANH";
                    }

                }
                else
                {
                    if (vLoai == "khachmoi")
                    {
                        var vKhachMoiInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                        if (vKhachMoiInfo != null)
                        {
                            if (vKhachMoiInfo.THAMDU == null)
                            {
                                vKhachMoiInfo.THAMDU = true;
                                vDataContext.SubmitChanges();
                                ClassCommon.ShowToastr(Page, "Điểm danh cho khách mời " + vKhachMoiInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                                LoadDanhSach(vCurentPage);
                            }
                            else
                            {
                                if (vKhachMoiInfo.THAMDU == false)
                                {
                                    vKhachMoiInfo.THAMDU = true;
                                    vDataContext.SubmitChanges();
                                    ClassCommon.ShowToastr(Page, "Điểm danh cho khách mời " + vKhachMoiInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                                    LoadDanhSach(vCurentPage);
                                }
                                else
                                {
                                    vKhachMoiInfo.THAMDU = false;
                                    vDataContext.SubmitChanges();
                                    ClassCommon.ShowToastr(Page, "Bỏ điểm danh cho khách mời " + vKhachMoiInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                                    LoadDanhSach(vCurentPage);
                                }
                            }
                            ViewState["Tab"] = "DIEMDANH";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
            LoadDanhSach(0);
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
            LoadDanhSach(0);
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
            dgDanhSach.CurrentPageIndex = e.NewPageIndex;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = Int16.Parse(e.NewPageIndex.ToString());
            vCurentPage = Int16.Parse(e.NewPageIndex.ToString());
            LoadDanhSach(vCurentPage);
        }
        protected void dgDanhSach_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            Custom_Paging(sender, e, dgDanhSach.CurrentPageIndex, dgDanhSach.VirtualItemCount, dgDanhSach.PageCount);
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
            LoadDanhSach(dgDanhSach.PageCount - 1);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach.PageCount - 1);
        }
        void lbNextPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach.CurrentPageIndex < (dgDanhSach.PageCount - 1))
            {
                LoadDanhSach(dgDanhSach.CurrentPageIndex + 1);
                Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach.CurrentPageIndex);
            }
        }
        void lbPreviousPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach.CurrentPageIndex > 0)
            {
                LoadDanhSach(dgDanhSach.CurrentPageIndex - 1);
                Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = (dgDanhSach.CurrentPageIndex);
            }
        }
        void lbFirstPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            LoadDanhSach(0);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = 0;
        }
        void DdlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();

            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = Int16.Parse(ddlPageSize.SelectedValue);
            vPageSize = Int16.Parse(ddlPageSize.SelectedValue);
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] = 0;
            vCurentPage = 0;
            LoadDanhSach(vCurentPage);
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
                    if (dgDanhSach.VirtualItemCount != 0)
                    {
                        lblCurentViewerRecord.Text = " " + ((dgDanhSach.CurrentPageIndex * dgDanhSach.PageSize) + 1).ToString() + " - " + (dgDanhSach.CurrentPageIndex + 1 == dgDanhSach.PageCount ? dgDanhSach.VirtualItemCount.ToString() : ((dgDanhSach.CurrentPageIndex + 1) * dgDanhSach.PageSize).ToString()).ToString();
                        lblCurentViewerRecord.Text += " trong tổng số " + dgDanhSach.VirtualItemCount + "";
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
                    lblKhachmoisPhienHop.Text = "";
                }
            }
            else
            {
                lboxKhachMoi.DataSource = null;
                lboxKhachMoi.DataBind();
                lblKhachmoisPhienHop.Text = "";
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
                    if (vKhachMois != "")
                    {
                        if (vKhachMois[(vKhachMois.Length - 1)] == ',')
                        {
                            vKhachMois = vKhachMois.Substring(0, (vKhachMois.Length - 1));
                        }
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

        #region Sơ đồ vị trí
        protected void NhapMaGhe(object sender, EventArgs e)
        {
            try
            {
                var UserID = _currentUser.UserID;
                TextBox textTextBox = ((TextBox)sender);
                var vKey = textTextBox.ToolTip.Split('|');
                string vMaGhe = ClassCommon.ClearHTML(textTextBox.Text.Trim());
                string vContent = vKey[0] + "|" + vMaGhe + "|" + vKey[1];
                bool check = true;


                if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null && ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"].ToString() != "")
                {
                    List<string> KeyKey = new List<string>();
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null)
                    {
                        KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"];
                    }
                    if (KeyKey.Count() > 0)
                    {
                        foreach (var drr in KeyKey)
                        {
                            if (drr.Split('|')[0] == vContent.Split('|')[0].ToLower() && drr.Split('|')[2] == vKey[1])
                            {
                                KeyKey.Remove(drr);
                                KeyKey.Add(vContent);
                                ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                                LoadDanhSachGhe();
                                break;
                            }
                            else
                            {
                                KeyKey.Add(vContent);
                                ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                                LoadDanhSachGhe();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    List<string> KeyKey = new List<string>();
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null)
                    {
                        KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"];
                    }
                    if (KeyKey.Count() >= 0)
                    {
                        KeyKey.Add(vContent);
                        ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                        LoadDanhSachGhe();
                    }
                }
                ViewState["Tab"] = "SODO";
            }
            catch (Exception ex)
            {

            }
        }

        public void LoadDanhSachGhe()
        {
            try
            {
                string vKeyWord = textSearchMaGhe.Text.Trim().ToLower();
                DataTable dtTable;
                dtTable = new DataTable();
                dtTable.Columns.Add("ID");
                dtTable.Columns.Add("LOAI");
                dtTable.Columns.Add("TEN");
                dtTable.Columns.Add("TENDONVI");
                dtTable.Columns.Add("TENCHUCVU");

                var vThanhPhanThamDuInfos = vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId).ToList();                
                vThanhPhanThamDuInfos = vThanhPhanThamDuInfos.Where(x => x.TEN.ToLower().Contains(vKeyWord) || x.TENCHUCVU.ToLower().Contains(vKeyWord) || x.TENDONVI.ToLower().Contains(vKeyWord)).ToList();
                if (vThanhPhanThamDuInfos != null && vThanhPhanThamDuInfos.Count > 0)
                {
                    foreach (var vThanhPhan in vThanhPhanThamDuInfos)
                    {
                        DataRow row = dtTable.NewRow();
                        row["ID"] = vThanhPhan.ID;
                        row["LOAI"] = vThanhPhan.LOAI;
                        row["TEN"] = vThanhPhan.TEN;
                        row["TENDONVI"] = vThanhPhan.TENDONVI;
                        row["TENCHUCVU"] = vThanhPhan.TENCHUCVU;
                        dtTable.Rows.Add(row);
                    }
                }
                dgDanhSachGhe.DataSource = dtTable;
                dgDanhSachGhe.PageSize = 9999;
                dgDanhSachGhe.CurrentPageIndex = 0;
                dgDanhSachGhe.VirtualItemCount = dtTable.Rows.Count;
                dgDanhSachGhe.DataBind();              
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        ///  Set thông tin sơ đồ khi chọn ghế
        /// </summary>
        /// <param name="pPhienHopId"></param>
        /// <param name="pCapNhatFile">true: Cập nhật file phòng họp;</param>
        public void SetInfoSoDo(int pPhienHopId, bool pCapNhatFile)
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
                                                SODO_FILE = phong.SODO_FILE,
                                                SODO_TEXT = a.SODO_Text == null ? "" : Server.HtmlDecode(a.SODO_Text).Replace("\n", ""),
                                            }).FirstOrDefault();

                if (objSoDo != null)
                {

                    //Tìm danh sách phiên họp vị trí
                    var objViTris = vDataContext.PRO_PHIENHOP_VITRI_WEB(pPhienHopId).ToList();


                    string vSource = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\" + objSoDo.SODO_FILE;
                    string vDes = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg";

                    string readText = File.ReadAllText(vSource);
                    //LOAI: 
                    foreach (var it in objViTris)
                    {
                        if (!String.IsNullOrEmpty(it.MAGHE))
                        {
                            int index = readText.LastIndexOf(it.MAGHE);

                            string vStr = readText.Substring(0, index);

                            int index_X = vStr.LastIndexOf("tspan x=\"");
                            index_X += ("tspan x=\"").Length;
                            string vPosition = "";

                            for (int i = index_X; i < index; i++)
                            {
                                if (vStr[i] == '"')
                                {
                                    break;
                                }
                                else
                                {
                                    vPosition += vStr[i].ToString();
                                }
                            }

                            var vNameArr = it.TEN.Trim().Split(' ');
                            string vText = "<tspan dy='0'>" + vNameArr[0] + "</tspan>";

                            for (int i = 1; i < vNameArr.Length; i++)
                            {
                                vText += "<tspan x='" + vPosition + "' dy='1.4em' >" + vNameArr[i] + "</tspan>";
                            }
                            readText = readText.Replace(it.MAGHE, vText);
                        }
                    }

                    File.WriteAllText(vDes, readText);
                    if (pCapNhatFile)
                    {
                        PHIENHOP_PHONGHOP vPhienHopPhongHopInfo = vDataContext.PHIENHOP_PHONGHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                        string vPhienHopPhongHopFileName = "phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg";
                        if (vPhienHopPhongHopInfo != null)
                        {
                            vPhienHopPhongHopInfo.SODO_FILE = vPhienHopPhongHopFileName;
                            vPhienHopPhongHopInfo.SODO_Text = File.ReadAllText(vDes);
                        }
                    }

                    vDataContext.SubmitChanges();
                    lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\" + objSoDo.SODO_FILE + "\" type=\"image/svg+xml\" style='width: 60em'>";
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void dgDanhSachGhe_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                TextBox textMaGhe = (TextBox)e.Item.FindControl("textMaGhe");
                List<string> KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"];
                if (textMaGhe != null)
                {
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] != null)
                    {
                        {
                            for (int i = 0; i < KeyKey.Count; i++)
                            {
                                var vKey = KeyKey[i].Split('|');
                                if (textMaGhe.ToolTip.Split('|')[0] == vKey[0] && textMaGhe.ToolTip.Split('|')[1] == vKey[2])
                                {
                                    textMaGhe.Text = KeyKey[i].Split('|')[1];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void textSearchMaGhe_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachGhe();
            ViewState["Tab"] = "SODO";
        }

        #endregion


    }
}
