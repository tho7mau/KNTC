#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật đơn thư
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class CnGiaiQuyetDonThu : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vDonThuId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        DonThuController vDonThuController = new DonThuController();
        DoiTuongController vDoiTuongController = new DoiTuongController();
        HoSoController vHoSoController = new HoSoController();
        List<DIAPHUONG> vDiaPhuongInfos = new List<DIAPHUONG>();
        List<DANTOC> vDanTocInfos = new List<DANTOC>();
        List<QUOCTICH> vQuocTichInfos = new List<QUOCTICH>();
        List<LOAIDONTHU> vLoaiDonThuInfos = new List<LOAIDONTHU>();
        public string vPathCommonUploadHoSo = ClassParameter.vPathCommonUploadHoSo;
        string vMacAddress = ClassCommon.GetMacAddress();
        int vPageSize = 20;
        int vCurentPage = 0;
        #endregion

        #region Events
        /// <summary>
        /// Event page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //dIAPHUONGs = vDataContext.DIAPHUONGs.ToList();
            try
            {
                // set tiêu đề chức năng trên Breadcrum
                //Kiem tra quyen dang nhap
                //CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vDonThuId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] != null)
                    {
                        XoaSessionFile();
                    }
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] != null)
                    {
                        XoaSessionFile_HuongXuLy();
                    }
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] != null)
                    {
                        XoaSessionFile_HoSoNguoiDaiDienUyQuyen();
                    }
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] != null)
                    {
                        XoaSessionFile_KetQuaGiaiQuyet();
                    }
                    hdfieldLayThongTin.Value = "true";
                    LoadDanhSachDonVi();
                    LoadDanhSachCanBo();
                    LoadDanhSachHinhThucGiaiQuyet();
                    LoadDanToc(-1, ddlistDanToc);
                    LoadQuocTich(-1, ddlistQuocTich);
                    LoadQuocTich(-1, ddlistQuocTichNguoiDaiDienUyQuyen);
                    SetFormInfo(vDonThuId);
                    LoadDanhSachHoSoDonThu(vDonThuId);
                    LoadDanhSachHoSoHuongXuLy(vDonThuId);
                    LoadDanhSachHoSoNguoiDaiDienUyQuyen(vDonThuId);
                    LoadDanhSachHoSoKetQuaGiaiQuyet(vDonThuId);
                    LoadDanhSach(1, vPageSize);
                    LoadBSModal();
                    //textTenThietBi.Focus();                
                }

            }
            catch (Exception ex)
            {
                //ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        protected void ChonDiaPhuongNguoiBiKhieuNaiToCao(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlistSelect = (DropDownList)sender;
                LoadDiaPhuong(Int32.Parse(ddlistSelect.SelectedValue), ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);
            }
            catch (Exception Ex)
            { }
        }

        protected void ChonDiaPhuongNguoiDaiDienUyQuyen(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlistSelect = (DropDownList)sender;
                LoadDiaPhuong(Int32.Parse(ddlistSelect.SelectedValue), ddlistXaPhuongNguoiDaiDienUyQuyen, ddlistQuanHuyenNguoiDaiDienUyQuyen, ddlistTinhThanhNguoiDaiDienUyQuyen);
            }
            catch (Exception Ex)
            { }
        }
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            textNoiDungBaoCao.Focus();

            SetEnableForm(true);
            Loadbtn_ThemNguoiDaiDien();
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
                CapNhat(vDonThuId);
            }
        }

        /// <summary>
        /// Chọn cơ quan tiếp nhận hướng xử lý, load danh sách cán bộ thuộc cơ quan đó
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlistCoQuanTiepNhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlistCoQuanTiepNhan.SelectedValue))
                {
                    var vCanBoTiepNhanInfos = vDataContext.CANBOs.Where(x => x.DONVI_ID == int.Parse(ddlistCoQuanTiepNhan.SelectedValue)).OrderBy(x => x.CANBO_TEN).ToList();
                    if (vCanBoTiepNhanInfos != null)
                    {
                        ddlistNguoiTiepNhan.DataSource = vCanBoTiepNhanInfos;
                        ddlistNguoiTiepNhan.DataTextField = "CANBO_TEN";
                        ddlistNguoiTiepNhan.DataValueField = "CANBO_ID";
                        ddlistNguoiTiepNhan.DataBind();
                        ddlistNguoiTiepNhan.Items.Insert(0, new ListItem("Chọn người tiếp nhận", ""));
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
            // return false;
            Boolean vResult = true;
            string vToastrMessage = "Vui lòng ";
            string oErrorMessage = "";

            if (cboxDaXuLy.Checked)
            {
                if (textNoiDungBaoCao.Text.Trim() == "")
                {
                    textNoiDungBaoCao.CssClass += " vld";
                    textNoiDungBaoCao.Focus();
                    labelNoiDungBaoCao.Attributes["class"] += " vld";
                    vToastrMessage += "nhập nội dung báo cáo, ";
                    vResult = false;
                }
                else
                {
                    textNoiDungBaoCao.CssClass = textNoiDungBaoCao.CssClass.Replace("vld", "").Trim();
                    labelNoiDungBaoCao.Attributes.Add("class", labelNoiDungBaoCao.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (dateNgayCapNhatKetQua.Text.Trim() == "")
                {
                    dateNgayCapNhatKetQua.CssClass += " vld";
                    dateNgayCapNhatKetQua.Focus();
                    labelNgayCapNhatKetQuaXuly.Attributes["class"] += " vld";
                    vToastrMessage += "chọn Ngày cập nhật kết quả, ";
                    vResult = false;
                }
                else
                {
                    dateNgayCapNhatKetQua.CssClass = dateNgayCapNhatKetQua.CssClass.Replace("vld", "").Trim();
                    labelNgayCapNhatKetQuaXuly.Attributes.Add("class", labelNgayCapNhatKetQuaXuly.Attributes["class"].ToString().Replace("vld", ""));
                }
                if (dateNgayCoKetQua.Text.Trim() == "")
                {
                    dateNgayCoKetQua.CssClass += " vld";
                    dateNgayCoKetQua.Focus();
                    labelNgayCapNhatKetQuaXuly.Attributes["class"] += " vld";
                    vToastrMessage += "chọn Ngày có kết quả, ";
                    vResult = false;
                }
                else
                {
                    dateNgayCoKetQua.CssClass = dateNgayCoKetQua.CssClass.Replace("vld", "").Trim();
                    labelNgayCapNhatKetQuaXuly.Attributes.Add("class", labelNgayCapNhatKetQuaXuly.Attributes["class"].ToString().Replace("vld", ""));
                }
            }

            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
            }
            return vResult;
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới thiết bị", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion

        #region Methods
        public void LoadLoaiDonThu(int vLoaiDonThu_ID, bool IsSelectedIndexChanged)
        {
            vLoaiDonThuInfos = vDataContext.LOAIDONTHUs.ToList();
            LOAIDONTHU objLOAIDONTHU = new LOAIDONTHU();
            if (vLoaiDonThu_ID > 0)
            {
                objLOAIDONTHU = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_ID == vLoaiDonThu_ID).FirstOrDefault();

                if (objLOAIDONTHU != null)
                {
                    if (objLOAIDONTHU.LOAIDONTHU_CAP == 3)
                    {
                        List<LOAIDONTHU> objLOAIDONTHUCap3List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).ToList();
                        LOAIDONTHU objLOAIDONTHUCap2 = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();

                        if (objLOAIDONTHUCap2 != null)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap2List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID).ToList();
                            LOAIDONTHU objLOAIDONTHUCap1 = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CAP == 1).ToList();

                            ddlistLoaDonThu.DataSource = objLOAIDONTHUCap1List;
                            ddlistLoaDonThu.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaDonThu.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaDonThu.DataBind();
                            ddlistLoaDonThu.Items.Insert(0, new ListItem("Chọn loại đơn thư", ""));
                            ddlistLoaDonThu.SelectedValue = objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID.ToString();


                            ddlistLoaiKhieuNai.DataSource = objLOAIDONTHUCap2List;
                            ddlistLoaiKhieuNai.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaiKhieuNai.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaiKhieuNai.DataBind();
                            ddlistLoaiKhieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHUCap1.LOAIDONTHU_TEN, ""));
                            ddlistLoaiKhieuNai.SelectedValue = objLOAIDONTHUCap2.LOAIDONTHU_ID.ToString();


                            ddlistLoaiKhieuNaiChiTiet.DataSource = objLOAIDONTHUCap3List;
                            ddlistLoaiKhieuNaiChiTiet.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaiKhieuNaiChiTiet.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaiKhieuNaiChiTiet.DataBind();
                            ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                            ddlistLoaiKhieuNaiChiTiet.SelectedValue = vLoaiDonThu_ID.ToString();
                        }
                    }
                    else if (objLOAIDONTHU.LOAIDONTHU_CAP == 2)
                    {

                        if (!IsSelectedIndexChanged)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap2List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).ToList();
                            LOAIDONTHU objLOAIDONTHUCap1 = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                            ddlistLoaDonThu.DataSource = objLOAIDONTHUCap1List;
                            ddlistLoaDonThu.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaDonThu.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaDonThu.DataBind();
                            ddlistLoaDonThu.Items.Insert(0, new ListItem("Chọn loại đơn thư", ""));
                            ddlistLoaDonThu.SelectedValue = objLOAIDONTHU.LOAIDONTHU_CHA_ID.ToString();


                            ddlistLoaiKhieuNai.DataSource = objLOAIDONTHUCap2List;
                            ddlistLoaiKhieuNai.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaiKhieuNai.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaiKhieuNai.DataBind();
                            ddlistLoaiKhieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHUCap1.LOAIDONTHU_TEN, ""));
                            ddlistLoaiKhieuNai.SelectedValue = vLoaiDonThu_ID.ToString();
                        }
                        List<LOAIDONTHU> objLOAIDONTHUCap3List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_ID).ToList();

                        ddlistLoaiKhieuNaiChiTiet.DataSource = objLOAIDONTHUCap3List;
                        ddlistLoaiKhieuNaiChiTiet.DataTextField = "LOAIDONTHU_TEN";
                        ddlistLoaiKhieuNaiChiTiet.DataValueField = "LOAIDONTHU_ID";
                        ddlistLoaiKhieuNaiChiTiet.DataBind();
                        ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";
                    }
                    else if (objLOAIDONTHU.LOAIDONTHU_CAP == 1)
                    {
                        if (!IsSelectedIndexChanged)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                            ddlistLoaDonThu.DataSource = objLOAIDONTHUCap1List;
                            ddlistLoaDonThu.DataTextField = "LOAIDONTHU_TEN";
                            ddlistLoaDonThu.DataValueField = "LOAIDONTHU_ID";
                            ddlistLoaDonThu.DataBind();
                            ddlistLoaDonThu.Items.Insert(0, new ListItem("Chọn loại đơn thư", ""));
                            ddlistLoaDonThu.SelectedValue = objLOAIDONTHU.LOAIDONTHU_ID.ToString();
                        }

                        List<LOAIDONTHU> objLOAIDONTHUCap2List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_ID).ToList();
                        ddlistLoaiKhieuNai.DataSource = objLOAIDONTHUCap2List;
                        ddlistLoaiKhieuNai.DataTextField = "LOAIDONTHU_TEN";
                        ddlistLoaiKhieuNai.DataValueField = "LOAIDONTHU_ID";
                        ddlistLoaiKhieuNai.DataBind();
                        ddlistLoaiKhieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHU.LOAIDONTHU_TEN, ""));
                        ddlistLoaiKhieuNai.SelectedValue = "";

                        ddlistLoaiKhieuNaiChiTiet.Items.Clear();
                        ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";
                    }
                }
            }
            else
            {
                if (!IsSelectedIndexChanged)
                {
                    List<LOAIDONTHU> objLOAIDONTHUCap1List = vLoaiDonThuInfos.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    ddlistLoaDonThu.DataSource = objLOAIDONTHUCap1List;
                    ddlistLoaDonThu.DataTextField = "LOAIDONTHU_TEN";
                    ddlistLoaDonThu.DataValueField = "LOAIDONTHU_ID";
                    ddlistLoaDonThu.DataBind();
                    ddlistLoaDonThu.Items.Insert(0, new ListItem("Chọn loại đơn thư", ""));
                    ddlistLoaDonThu.SelectedValue = "";

                    ddlistLoaiKhieuNai.Items.Clear();
                    ddlistLoaiKhieuNai.Items.Insert(0, new ListItem("Chọn loại", ""));
                    ddlistLoaiKhieuNai.SelectedValue = "";

                    ddlistLoaiKhieuNaiChiTiet.Items.Clear();
                    ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                    ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";
                }
                else
                {
                    if (ddlistLoaDonThu.SelectedValue != "")
                    {
                        ddlistLoaiKhieuNaiChiTiet.Items.Clear();
                        ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";
                    }
                    else
                    {
                        ddlistLoaiKhieuNaiChiTiet.Items.Clear();
                        ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn loại", ""));
                        ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";

                        ddlistLoaiKhieuNaiChiTiet.Items.Clear();
                        ddlistLoaiKhieuNaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        ddlistLoaiKhieuNaiChiTiet.SelectedValue = "";
                    }
                }
            }
        }


        /// <summary>
        ///  Set thông tin cho form
        /// </summary>
        /// <param name="pDONTHU_ID"></param>
        public void SetFormInfo(int pDONTHU_ID)
        {
            try
            {
                string TitleBreadcrumb = "";
                if (pDONTHU_ID == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                    btn_XuatPhieu.Visible = false;
                    List<CANHAN> cANHANs = new List<CANHAN>();
                    CANHAN cANHAN = new CANHAN();
                    cANHANs.Add(cANHAN);
                    cANHAN.CANHAN_ID = 0;
                    cANHAN.CANHAN_GIOITINH = false;
                    ListViewDoiTuong.DataSource = cANHANs;
                    textNgayNhanDon.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textNgayChuyen.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textNgayDeDon.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textThoiHanGiaiQuyet.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                    ListViewDoiTuong.DataBind();
                    TitleBreadcrumb = TitleBreadcrumb + "Thêm mới đơn thư";
                    lblBreadcrumbTitle.Text = TitleBreadcrumb;
                    for (int i = 0; i < ListViewDoiTuong.Items.Count(); i++)
                    {
                        ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                        DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                        DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                        DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));
                        DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                        DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));
                        LoadDiaPhuong(-1, pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
                        LoadDanToc(-1, drlDanToc);
                        LoadQuocTich(-1, drlQuocTich);
                    }
                    LoadDiaPhuong(-1, ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);
                    LoadDiaPhuong(-1, ddlistXaPhuongNguoiDaiDienUyQuyen, ddlistQuanHuyenNguoiDaiDienUyQuyen, ddlistTinhThanhNguoiDaiDienUyQuyen);

                    LoadLoaiDonThu(0, false);
                    textSoNguoi.Text = "1";
                    textSoNguoiDaiDien.Enabled = false;
                    Loadbtn_ThemNguoiDaiDien();

                    int MaxSTT = Int32.Parse(vDataContext.DONTHUs.Max(x => x.DONTHU_STT).ToString());
                    //lblSTT.Text = (MaxSTT + 1) + "";
                    textNoiDungDonThu.Text = "";
                }
                else //Cập nhật đơn thư
                {
                    DONTHU vDonThuInfo = vDonThuController.GetAll_Info_DONTHU_ById(pDONTHU_ID);
                    if (vDonThuInfo != null)
                    {
                        ddlistNguon.SelectedValue = vDonThuInfo.NGUONDON_LOAI_CHITIET.ToString() ?? "0";
                        textNgayNhanDon.Text = DateTime.Parse(vDonThuInfo.NGAYTAO.ToString()).ToString("dd/MM/yyyy");
                        textNgayDeDon.Text = DateTime.Parse(vDonThuInfo.NGUONDON_NGAYDEDON.ToString()).ToString("dd/MM/yyyy");
                        if (vDonThuInfo.NGUONDON_LOAI_CHITIET == 2)
                        {
                            divCoQuanChuyenDon.Visible = true;
                            ddlistCoQuanDaChuyen.SelectedValue = vDonThuInfo.NGUONDON_DONVI_ID.ToString() ?? "";
                            textNgayChuyen.Text = DateTime.Parse(vDonThuInfo.NGUONDON_NGAYCHUYEN.ToString()).ToString("dd/MM/yyyy");

                            textSoVanBanChuyen.Text = vDonThuInfo.NGUONDON_SOVANBANCHUYEN;
                        }
                        else
                        {
                            divCoQuanChuyenDon.Visible = false;
                        }

                        ddlistDoiTuong.SelectedValue = vDonThuInfo.DOITUONG_ID.ToString() ?? "0";
                        textSoNguoi.Text = vDonThuInfo.DOITUONG.CANHANs.Count().ToString();
                        //Load đơn thư không đủ điều kiện
                        if (vDonThuInfo.TRANGTHAI_DONTHUKHONGDUDIEUKIEN ?? false)
                        {
                            cboxDonThuKhongDuDieuKien.Checked = true;
                            divDonThuKhongDuDieuKien.Visible = true;
                            divHuongXuLy.Visible = false;
                        }
                        else
                        {
                            cboxDonThuKhongDuDieuKien.Checked = false;
                            divDonThuKhongDuDieuKien.Visible = false;
                            divHuongXuLy.Visible = true;
                        }

                        //Load checkbox nặc danh
                        if (vDonThuInfo.NGUONDON_LOAI_CHITIET == 0)
                        {
                            divNacDanh.Visible = false;
                        }
                        else
                        {
                            if (vDonThuInfo.DOITUONG_ID != 1)
                            {
                                divNacDanh.Visible = false;
                            }
                            else
                            {
                                divNacDanh.Visible = true;
                            }
                        }

                        if (vDonThuInfo.DONTHU_NACDANH == true)
                        {
                            cboxDonThuNacDanh.Checked = vDonThuInfo.DONTHU_NACDANH ?? false;
                            divDonThuKhongDuDieuKien.Visible = false;
                        }
                        else
                        {
                            divDonThuKhongDuDieuKien.Visible = true;
                            cboxDonThuNacDanh.Checked = vDonThuInfo.DONTHU_NACDANH ?? false;
                        }


                        textNgayNhanDon.Text = DateTime.Parse(vDonThuInfo.NGAYTAO.ToString()).ToString("dd/MM/yyyy");
                        ddlistNguon.SelectedValue = vDonThuInfo.NGUONDON_LOAI_CHITIET.ToString() ?? "0";
                        textSoNguoi.Text = "1";
                        TitleBreadcrumb = "";
                        lblSTT_ThoiHanGiaiQuyet.Text = (vDonThuInfo.DONTHU_STT) + "";
                        lblThoiHanGiaiQuyet.Text = String.Format("{0:dd/MM/yyyy}", vDonThuInfo.HUONGXULY_THOIHANGIAIQUET);
                        //Load danh sách đối tượng
                        ListViewDoiTuong.DataSource = vDonThuInfo.DOITUONG.CANHANs;
                        ListViewDoiTuong.DataBind();
                        for (int i = 0; i < vDonThuInfo.DOITUONG.CANHANs.Count(); i++)
                        {
                            TitleBreadcrumb = TitleBreadcrumb + "-" + vDonThuInfo.DOITUONG.CANHANs[i].CANHAN_HOTEN;
                            ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                            TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                            if (txtCaNhanID.Text != "")
                            {
                                int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);
                                CANHAN cANHAN = vDonThuInfo.DOITUONG.CANHANs.Where(x => x.CANHAN_ID == vCANHAN_ID).FirstOrDefault();
                                if (cANHAN != null)
                                {
                                    DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                                    DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                                    DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));
                                    DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                                    DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));


                                    LoadDiaPhuong((int)(cANHAN.DP_ID ?? -1), pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
                                    LoadDanToc((int)(cANHAN.DANTOC_ID ?? -1), drlDanToc);
                                    LoadQuocTich((int)(cANHAN.QUOCTICH_ID ?? -1), drlQuocTich);
                                }
                            }
                        }
                        //Thông tin cơ quan đã giải quyết
                        if (vDonThuInfo.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET ?? false)
                        {
                            cboxCoQuanDaGiaiQuyet.Checked = true;
                            divCoQuanDaGiaiQuyet.Visible = true;
                            if (vDonThuInfo.DAGIAIQUYET_DONVI_ID != null)
                            {
                                ddlistCoQuanDaGiaiQuyet.SelectedValue = vDonThuInfo.DAGIAIQUYET_DONVI_ID.ToString();
                            }
                            textLanGiaiQuyet.Text = vDonThuInfo.DAGIAIQUYET_LAN.ToString();
                            textNgayBanHanhQuyetDinh.Text = DateTime.Parse(vDonThuInfo.DAGIAIQUYET_NGAYBANHANH_QD.ToString()).ToString("dd/MM/yyyy");
                            if (vDonThuInfo.DAGIAIQUYET_HTGQ_ID != null)
                            {
                                ddlistHinhThucGiaiQuyet.SelectedValue = vDonThuInfo.DAGIAIQUYET_HTGQ_ID.ToString();
                            }
                            textKetQuaCuaCoQuanGiaiQuyet.Text = vDonThuInfo.DAGIAIQUYET_KETQUA_CQ;
                        }
                        else
                        {
                            cboxCoQuanDaGiaiQuyet.Checked = false;
                            divCoQuanDaGiaiQuyet.Visible = false;
                        }

                        //Bổ sung thông tin người bị khiếu nại, tố cáo
                        if (vDonThuInfo.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO ?? false)
                        {
                            cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked = true;
                            divThongTinNguoiBiKhieuNaiToCao.Visible = true;
                            var vDoiTuongBiKhieuNaiToCaoInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == vDonThuInfo.DOITUONG_BI_KNTC_ID).FirstOrDefault();

                            if (vDoiTuongBiKhieuNaiToCaoInfo != null)
                            {
                                ddlistDoiTuongBoSung.SelectedValue = vDoiTuongBiKhieuNaiToCaoInfo.DOITUONG_LOAI.ToString();
                                if (vDoiTuongBiKhieuNaiToCaoInfo.DOITUONG_LOAI == 1)
                                {
                                    divTenCoQuanBiKhieuNaiToCao.Visible = false;
                                    divQuocTichDanToc_NguoiBiKhieuNaiToCao.Visible = true;

                                    divTenCaNhan_BiKhieuNaiToCao.Visible = true;
                                    if (vDoiTuongBiKhieuNaiToCaoInfo.CANHANs.Count > 0)
                                    {

                                        var vCaNhanInfo = vDoiTuongBiKhieuNaiToCaoInfo.CANHANs.FirstOrDefault();
                                        if (vCaNhanInfo != null)
                                        {
                                            textHoTen_NguoiBiKhieuNaiToCao.Text = vCaNhanInfo.CANHAN_HOTEN;
                                            if (vCaNhanInfo.CANHAN_GIOITINH ?? false)
                                            {
                                                rdoDoiTuongNam.Checked = true;
                                                rdoDoiTuongNu.Checked = false;
                                            }
                                            else
                                            {
                                                rdoDoiTuongNam.Checked = false;
                                                rdoDoiTuongNu.Checked = true;
                                            }
                                            textNoiCongTac.Text = vCaNhanInfo.NOICONGTAC;
                                            textChucVu.Text = vCaNhanInfo.CHUCVU;
                                            textDiaChiDoiTuongBiKhieuNaiToCao.Text = vCaNhanInfo.CANHAN_DIACHI;
                                            LoadDiaPhuong(vCaNhanInfo.DP_ID ?? 0, ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);
                                            ddlistDanToc.SelectedValue = vCaNhanInfo.DANTOC_ID.ToString() ?? "";
                                            ddlistQuocTich.SelectedValue = vCaNhanInfo.QUOCTICH_ID.ToString() ?? "";
                                        }
                                    }
                                }
                                else
                                {
                                    if (vDoiTuongBiKhieuNaiToCaoInfo.DOITUONG_LOAI == 3)
                                    {
                                        divTenCoQuanBiKhieuNaiToCao.Visible = true;
                                        divQuocTichDanToc_NguoiBiKhieuNaiToCao.Visible = false;
                                        divTenCaNhan_BiKhieuNaiToCao.Visible = false;
                                        textTenCoQuanToChuc_BiKhieuNaiToCao.Text = vDoiTuongBiKhieuNaiToCaoInfo.DOITUONG_TEN;
                                        textDiaChi.Text = vDoiTuongBiKhieuNaiToCaoInfo.DOITUONG_DIACHI;
                                        LoadDiaPhuong(vDoiTuongBiKhieuNaiToCaoInfo.DP_ID ?? 0, ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);

                                    }
                                }
                            }
                        }
                        else
                        {
                            cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked = false;
                            divThongTinNguoiBiKhieuNaiToCao.Visible = false;
                        }
                        //Bổ sung thông tin người đại diện, ủy quyền
                        if (vDonThuInfo.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN ?? false)
                        {
                            cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked = true;
                            divThongTinNguoiDaiDienUyQuyen.Visible = true;
                            var vCaNhanDaiDienUyQuyenInfo = vDataContext.CANHANs.Where(x => x.CANHAN_ID == vDonThuInfo.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();
                            if (vCaNhanDaiDienUyQuyenInfo != null)
                            {
                                textHoTenNguoiDaiDien.Text = vCaNhanDaiDienUyQuyenInfo.CANHAN_HOTEN;
                                if (vCaNhanDaiDienUyQuyenInfo.CANHAN_GIOITINH ?? false)
                                {
                                    rdoDaiDienUyQuyenNam.Checked = true;
                                    rdoDaiDienUyQuyenNu.Checked = false;
                                }
                                else
                                {
                                    rdoDaiDienUyQuyenNam.Checked = false;
                                    rdoDaiDienUyQuyenNu.Checked = true;
                                }
                                textDiaChiNguoiDaiDienUyQuyen.Text = vCaNhanDaiDienUyQuyenInfo.CANHAN_DIACHI;
                                LoadDiaPhuong(vCaNhanDaiDienUyQuyenInfo.DP_ID ?? 0, ddlistXaPhuongNguoiDaiDienUyQuyen, ddlistQuanHuyenNguoiDaiDienUyQuyen, ddlistTinhThanhNguoiDaiDienUyQuyen);
                            }
                        }
                        else
                        {
                            cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked = false;
                            divThongTinNguoiDaiDienUyQuyen.Visible = false;
                        }

                        //Load thông tin hướng xử lý
                        if (vDonThuInfo.HUONGXULY_ID != null)
                        {
                            ddlistHuongXuLy.SelectedValue = vDonThuInfo.HUONGXULY_ID.ToString();
                            if (vDonThuInfo.HUONGXULY_ID == 2 || vDonThuInfo.HUONGXULY_ID == 3 || vDonThuInfo.HUONGXULY_ID == 7 || vDonThuInfo.HUONGXULY_ID == 1)
                            {
                                btn_XuatPhieu.Visible = true;
                            }
                            else
                            {
                                btn_XuatPhieu.Visible = false;
                            }
                        }
                        if (vDonThuInfo.HUONGXULY_THAMQUYENGIAIQUYET_ID != null)
                        {
                            ddlistThamQuyenGiaiQuyet.SelectedValue = vDonThuInfo.HUONGXULY_THAMQUYENGIAIQUYET_ID.ToString();
                        }

                        if (vDonThuInfo.HUONGXULY_DONVI_ID != null)
                        {
                            ddlistCoQuanTiepNhan.SelectedValue = vDonThuInfo.HUONGXULY_DONVI_ID.ToString();
                            ddlistCoQuanTiepNhan_SelectedIndexChanged(ddlistCoQuanTiepNhan, new EventArgs());
                        }
                        if (vDonThuInfo.HUONGXULY_NGUOIDUYET_CANHAN_ID != null)
                        {
                            ddlistNguoiDuyet.SelectedValue = vDonThuInfo.HUONGXULY_NGUOIDUYET_CANHAN_ID.ToString();
                        }
                        if (vDonThuInfo.HUONGXULY_CANBO != null)
                        {
                            ddlistNguoiTiepNhan.SelectedValue = vDonThuInfo.HUONGXULY_CANBO.ToString();
                        }
                        if (vDonThuInfo.HUONGXULY_NGAYCHUYEN != null)
                        {
                            textNgayChuyen_HuongXuLy.Text = DateTime.Parse(vDonThuInfo.HUONGXULY_NGAYCHUYEN.ToString()).ToString("dd/MM/yyyy");
                        }

                        textSoHieuVanBanDi.Text = vDonThuInfo.HUONGXULY_SOHIEUVB_DI;
                        textChucVu_HuongXuLy.Text = vDonThuInfo.HUONGXULY_CHUCVU_TEN;
                        if (vDonThuInfo.HUONGXULY_THOIHANGIAIQUET != null)
                        {
                            textThoiHanGiaiQuyet.Text = DateTime.Parse(vDonThuInfo.HUONGXULY_THOIHANGIAIQUET.ToString()).ToString("dd/MM/yyyy");
                        }
                        if (vDonThuInfo.HUONGXULY_ID == 1 ||
                           vDonThuInfo.HUONGXULY_ID == 5 ||
                           vDonThuInfo.HUONGXULY_ID == 6 ||
                           vDonThuInfo.HUONGXULY_ID == 7 ||
                           vDonThuInfo.HUONGXULY_ID == 8 ||
                           vDonThuInfo.HUONGXULY_ID == 9 ||
                           vDonThuInfo.HUONGXULY_ID == 10)
                        {
                            divCoQuanTiepNhan_HuongXuLy.Visible = false;
                        }
                        else
                        {
                            divCoQuanTiepNhan_HuongXuLy.Visible = true;
                            if ((vDonThuInfo.HUONGXULY_ID == 2) || (vDonThuInfo.HUONGXULY_ID == 3))
                            {
                                divNgayChuyen_HuongXuLy.Visible = true;

                            }
                            else
                            {
                                divNgayChuyen_HuongXuLy.Visible = false;
                            }
                        }

                        textNoiDungDonThu.Text = vDonThuInfo.DONTHU_NOIDUNG;

                        textYKienXuLy.Text = vDonThuInfo.HUONGXULY_YKIEN_XULY;
                        textGhiChu.Text = vDonThuInfo.DONTHU_GHICHU;
                        cboxDaXuLy.Checked = vDonThuInfo.KETQUA_XYLY ?? false;
                        divKetQuaXuLy.Visible = vDonThuInfo.KETQUA_XYLY ?? false ? true : false;

                        textNoiDungBaoCao.Text = vDonThuInfo.KETQUA_NOIDUNG;
                        if(vDonThuInfo.KETQUA_NGAYCAPNHAT!=null)
                        {
                            dateNgayCapNhatKetQua.Text = ((DateTime)vDonThuInfo.KETQUA_NGAYCAPNHAT).ToString("dd/MM/yyyy");
                        }
                        if(vDonThuInfo.KETQUA_NGAY!=null)
                        {
                            dateNgayCoKetQua.Text = ((DateTime)vDonThuInfo.KETQUA_NGAY).ToString("dd/MM/yyyy");
                        }    
                        

                        TitleBreadcrumb = TitleBreadcrumb.Remove(0, 1);
                        lblBreadcrumbTitle.Text = TitleBreadcrumb + " (" + vDonThuInfo.DONTHU_STT + " - " + DateTime.Parse(vDonThuInfo.NGAYTAO.ToString()).ToString("dd/MM/yyyy") + ")";
                        SetEnableForm(false);
                        Loadbtn_ThemNguoiDaiDien();
                        if (vDonThuInfo.LOAIDONTHU_ID != null)
                        {
                            LoadLoaiDonThu((int)(vDonThuInfo.LOAIDONTHU_ID), false);
                        }
                        else
                        {
                            LoadLoaiDonThu(0, false);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }
        /// <summary>
        /// Load danh sách đơn vị vào dropdown
        /// </summary>
        public void LoadDanhSachDonVi()
        {
            try
            {
                var vDonViInfos = vDataContext.DONVIs.OrderBy(x => x.TENDONVI).ToList();
                if (vDonViInfos.Count > 0)
                {
                    ddlistCoQuanDaChuyen.Items.Clear();
                    ddlistCoQuanDaChuyen.DataSource = vDonViInfos;
                    ddlistCoQuanDaChuyen.DataTextField = "TENDONVI";
                    ddlistCoQuanDaChuyen.DataValueField = "DONVI_ID";
                    ddlistCoQuanDaChuyen.DataBind();
                    ddlistCoQuanDaChuyen.Items.Insert(0, new ListItem("Chọn cơ quan", ""));

                    ddlistCoQuanDaGiaiQuyet.Items.Clear();
                    ddlistCoQuanDaGiaiQuyet.DataSource = vDonViInfos;
                    ddlistCoQuanDaGiaiQuyet.DataTextField = "TENDONVI";
                    ddlistCoQuanDaGiaiQuyet.DataValueField = "DONVI_ID";
                    ddlistCoQuanDaGiaiQuyet.DataBind();
                    ddlistCoQuanDaGiaiQuyet.Items.Insert(0, new ListItem("Chọn cơ quan", ""));

                    ddlistCoQuanTiepNhan.Items.Clear();
                    ddlistCoQuanTiepNhan.DataSource = vDonViInfos;
                    ddlistCoQuanTiepNhan.DataTextField = "TENDONVI";
                    ddlistCoQuanTiepNhan.DataValueField = "DONVI_ID";
                    ddlistCoQuanTiepNhan.DataBind();
                    ddlistCoQuanTiepNhan.Items.Insert(0, new ListItem("Chọn cơ quan", ""));
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Load sanh sách cán bộ duyệt
        /// </summary>
        public void LoadDanhSachCanBo()
        {
            try
            {
                var vCanBoInfos = vDataContext.CANBOs.Where(x => x.LANHDAO == true).OrderBy(x => x.CANBO_TEN).ToList();
                if (vCanBoInfos.Count > 0)
                {
                    ddlistNguoiDuyet.Items.Clear();
                    ddlistNguoiDuyet.DataSource = vCanBoInfos;
                    ddlistNguoiDuyet.DataTextField = "CANBO_TEN";
                    ddlistNguoiDuyet.DataValueField = "CANBO_ID";
                    ddlistNguoiDuyet.DataBind();
                    ddlistNguoiDuyet.Items.Insert(0, new ListItem("Chọn người duyệt", ""));
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Load danh sách hình thức giải quyết
        /// </summary>
        public void LoadDanhSachHinhThucGiaiQuyet()
        {
            try
            {
                var vCanBoInfos = vDataContext.HINHTHUCGIAIQUYETs.OrderBy(x => x.HTGQ_ID).ToList();
                if (vCanBoInfos.Count > 0)
                {
                    ddlistHinhThucGiaiQuyet.Items.Clear();
                    ddlistHinhThucGiaiQuyet.DataSource = vCanBoInfos;
                    ddlistHinhThucGiaiQuyet.DataTextField = "HTGQ_TEN";
                    ddlistHinhThucGiaiQuyet.DataValueField = "HTGQ_ID";
                    ddlistHinhThucGiaiQuyet.DataBind();
                    ddlistHinhThucGiaiQuyet.Items.Insert(0, new ListItem("Chọn hình thức giải quyết", ""));
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Set trạng thái visible form
        /// </summary>
        /// <param name="pEnableStatus"></param>
        public void SetEnableForm(bool pEnableStatus)
        {
            //Thông tin nguồn đơn
            ddlistNguon.Enabled = false;
            textNgayDeDon.Enabled = false;
            ddlistDoiTuong.Enabled = false;
            ddlistCoQuanDaChuyen.Enabled = false;
            textNgayNhanDon.Enabled = false;
            textSoVanBanChuyen.Enabled = false;
            textSoNguoi.Enabled = false;

            //Thông tin người đại diện
            ddlistDoiTuong.Enabled = false;
            textSoNguoiDaiDien.Enabled = false;
            cboxDonThuNacDanh.Enabled = false;

            foreach (var item in ListViewDoiTuong.Items)
            {
                TextBox txtHoTen = ((TextBox)item.FindControl("txtHoTen"));
                //TextBox txtLanTiep = ((TextBox)item.FindControl("txtLanTiep"));
                TextBox txtCMND = ((TextBox)item.FindControl("txtCMND"));
                HtmlInputRadioButton rdoNam = ((HtmlInputRadioButton)item.FindControl("rdoNam"));
                HtmlInputRadioButton rdoNu = ((HtmlInputRadioButton)item.FindControl("rdoNu"));
                TextBox txtNgayCap = ((TextBox)item.FindControl("txtNgayCap"));
                TextBox txtNoiCap = ((TextBox)item.FindControl("txtNoiCap"));
                TextBox txtDiaChi = ((TextBox)item.FindControl("txtDiaChi"));

                DropDownList pDropDownListXa = ((DropDownList)item.FindControl("drlXa"));
                DropDownList pDropDownListHuyen = ((DropDownList)item.FindControl("drlQuanHuyen"));
                DropDownList pDropDownListThanhpho = ((DropDownList)item.FindControl("drlTinhThanhPho"));
                DropDownList drlQuocTich = ((DropDownList)item.FindControl("drlQuocTich"));
                DropDownList drlDanToc = ((DropDownList)item.FindControl("drlDanToc"));

                if (item.DataItemIndex != 0)
                {
                    HtmlAnchor buttonXoaCaNhan = ((HtmlAnchor)item.FindControl("Xoa_CaNhan"));
                    buttonXoaCaNhan.Visible = false;
                }

                txtHoTen.Enabled = false;
                txtCMND.Enabled = false;
                rdoNam.Disabled = !false;
                rdoNu.Disabled = !false;

                txtNgayCap.Enabled = false;
                txtNoiCap.Enabled = false;
                txtDiaChi.Enabled = false;
                pDropDownListThanhpho.Enabled = false;
                pDropDownListHuyen.Enabled = false;
                pDropDownListXa.Enabled = false;
                drlQuocTich.Enabled = false;
                drlDanToc.Enabled = false;
            }

            //Thông tin đơn thư
            cboxDonThuKhongDuDieuKien.Enabled = false;
            ddlistLoaDonThu.Enabled = false;
            ddlistLoaiKhieuNai.Enabled = false;
            ddlistLoaiKhieuNaiChiTiet.Enabled = false;
            textNoiDungDonThu.Enabled = false;

            cboxCoQuanDaGiaiQuyet.Enabled = false;
            ddlistCoQuanDaGiaiQuyet.Enabled = false;
            textLanGiaiQuyet.Enabled = false;
            textNgayBanHanhQuyetDinh.Enabled = false;
            ddlistHinhThucGiaiQuyet.Enabled = false;
            textKetQuaCuaCoQuanGiaiQuyet.Enabled = false;

            cboxBoSungThongTinNguoiBiKhieuNaiToCao.Enabled = false;
            ddlistDoiTuongBoSung.Enabled = false;
            textTenCoQuanToChuc_BiKhieuNaiToCao.Enabled = false;
            textHoTen_NguoiBiKhieuNaiToCao.Enabled = false;
            textNoiCongTac.Enabled = false;
            rdoDoiTuongNam.Disabled = !false;
            rdoDoiTuongNu.Disabled = !false;
            textDiaChiDoiTuongBiKhieuNaiToCao.Enabled = false;
            ddlistTinhThanh.Enabled = false;
            ddlistXaPhuong.Enabled = false;
            ddlistQuanHuyen.Enabled = false;
            ddlistQuocTich.Enabled = false;
            ddlistDanToc.Enabled = false;


            cboxBoSungThongTinNguoiDaiDienUyQuyen.Enabled = false;
            textHoTenNguoiDaiDien.Enabled = false;
            rdoDaiDienUyQuyenNam.Disabled = !false;
            rdoDaiDienUyQuyenNu.Disabled = !false;
            textDiaChiNguoiDaiDienUyQuyen.Enabled = false;
            ddlistTinhThanhNguoiDaiDienUyQuyen.Enabled = false;
            ddlistXaPhuongNguoiDaiDienUyQuyen.Enabled = false;
            ddlistQuanHuyenNguoiDaiDienUyQuyen.Enabled = false;
            ddlistQuocTichNguoiDaiDienUyQuyen.Enabled = false;

            //Hướng xử lý
            ddlistHuongXuLy.Enabled = false;
            ddlistThamQuyenGiaiQuyet.Enabled = false;
            ddlistCoQuanTiepNhan.Enabled = false;
            textNgayChuyen_HuongXuLy.Enabled = false;
            textSoHieuVanBanDi.Enabled = false;
            ddlistNguoiDuyet.Enabled = false;
            ddlistNguoiTiepNhan.Enabled = false;
            textChucVu_HuongXuLy.Enabled = false;
            textThoiHanGiaiQuyet.Enabled = false;
            textYKienXuLy.Enabled = false;
            textGhiChu.Enabled = false;

            textNoiDungBaoCao.Enabled = pEnableStatus;
            dateNgayCapNhatKetQua.Enabled = pEnableStatus;
            dateNgayCoKetQua.Enabled = pEnableStatus;
            cboxDaXuLy.Enabled = pEnableStatus;
            //Hồ sơ đơn thư
            dgDanhSach_File_HoSoDonThu.Columns[4].Visible = false;
            buttonThemTaiLieu.Visible = false;

            //Hồ sơ đơn thư
            dgDanhSach_File_HuongXuLy.Columns[4].Visible = false;
            buttonThemHoSoHuongXuLy.Visible = false;

            //Hồ sơ đơn thư
            dgDanhSach_File_NguoiDaiDienUyQuyen.Columns[4].Visible = false;
            buttonThemTapTinNguoiDaiDienUyQuyen.Visible = false;

            //Hồ sơ kết quả giải quyết
            dgDanhSach_File_KetQuaGiaiQuyet.Columns[4].Visible = pEnableStatus;
            buttonThemHoSoKetQuaXuLy.Visible = pEnableStatus;



            //Thêm đối tượng
            btnChonNguoiDaiDien.Visible = false;
        }

        /// <summary>
        /// Cập nhật thông tin thiết bị
        /// </summary>
        /// <param name="pDonThuId"></param>
        public void CapNhat(int pDonThuId)
        {
            try
            {
                var vDonThuInfo = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == vDonThuId).FirstOrDefault();
                vDonThuInfo.KETQUA_XYLY = cboxDaXuLy.Checked;
                if (cboxDaXuLy.Checked)
                {
                    
                    vDonThuInfo.KETQUA_NOIDUNG = ClassCommon.ClearHTML(textNoiDungBaoCao.Text.Trim());
                    DateTime oNgayCapNhatKetQua;
                    if (DateTime.TryParse(dateNgayCapNhatKetQua.Text.Trim(), out oNgayCapNhatKetQua))
                    {
                        vDonThuInfo.KETQUA_NGAYCAPNHAT = oNgayCapNhatKetQua;
                    }
                    DateTime oNgayCoKetQua;
                    if (DateTime.TryParse(dateNgayCoKetQua.Text.Trim(), out oNgayCoKetQua))
                    {
                        vDonThuInfo.KETQUA_NGAY = oNgayCoKetQua;
                    }
                    vDonThuInfo.DONTHU_TRANGTHAI = 3;
                }
                else
                {
                    vDonThuInfo.DONTHU_TRANGTHAI = 2;
                }
                vDataContext.SubmitChanges();
                ClassCommon.ShowToastr(Page, "Cập nhật giải quyết đơn thư thành công", "Thông báo", "success");
                SetEnableForm(false);
                buttonThemmoi.Visible = false;
                btnCapNhat.Visible = false;
                btnSua.Visible = true;
            }
            catch (Exception ex)
            {
                //ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Check Account Login
        /// </summary>
        public void CheckAccountLogin()
        {

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
        #endregion

        #region Xử lý đối tượng
        protected void btnBreadcrumbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
        }

        protected void btn_ThemMoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới đơn thư", "id=0");
            Response.Redirect(vUrl);
        }

        protected void btn_ThemNguoiDaiDien_Click(object sender, EventArgs e)
        {
            try
            {
                List<CANHAN> cANHANs = new List<CANHAN>();
                foreach (var item in ListViewDoiTuong.Items)
                {
                    CANHAN cANHAN = new CANHAN();
                    cANHAN = GetThongTinCaNhan(item);
                    cANHANs.Add(cANHAN);
                }

                CANHAN objCANHANAppend = new CANHAN();
                long MinID = cANHANs.Min(x => x.CANHAN_ID);
                if (MinID > 0)
                {
                    objCANHANAppend.CANHAN_ID = -1;
                }
                else
                {
                    objCANHANAppend.CANHAN_ID = MinID - 1;
                }
                objCANHANAppend.DP_ID = -1;
                objCANHANAppend.QUOCTICH_ID = -1;
                objCANHANAppend.DANTOC_ID = -1;

                objCANHANAppend.CANHAN_GIOITINH = false;
                cANHANs.Add(objCANHANAppend);


                ListViewDoiTuong.DataSource = cANHANs;
                ListViewDoiTuong.DataBind();

                for (int i = 0; i < cANHANs.Count(); i++)
                {
                    ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                    TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                    if (txtCaNhanID.Text != "")
                    {
                        int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);

                        DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                        DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                        DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));

                        DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                        DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));

                        LoadDiaPhuong((int)cANHANs[i].DP_ID, pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
                        LoadDanToc((int)cANHANs[i].DANTOC_ID, drlDanToc);
                        LoadQuocTich((int)cANHANs[i].QUOCTICH_ID, drlQuocTich);
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }
        public CANHAN GetThongTinCaNhan(ListViewDataItem listViewDataItem_CaNhan)
        {
            TextBox txtHoTen = ((TextBox)listViewDataItem_CaNhan.FindControl("txtHoTen"));
            TextBox txtCaNhanID = ((TextBox)listViewDataItem_CaNhan.FindControl("txtCaNhanID"));
            TextBox txtLanTiep = ((TextBox)listViewDataItem_CaNhan.FindControl("txtLanTiep"));
            TextBox txtCMND = ((TextBox)listViewDataItem_CaNhan.FindControl("txtCMND"));
            TextBox txtNgayCap = ((TextBox)listViewDataItem_CaNhan.FindControl("txtNgayCap"));
            TextBox txtNoiCap = ((TextBox)listViewDataItem_CaNhan.FindControl("txtNoiCap"));
            TextBox txtDiaChi = ((TextBox)listViewDataItem_CaNhan.FindControl("txtDiaChi"));

            HtmlInputRadioButton rdoNam = ((HtmlInputRadioButton)listViewDataItem_CaNhan.FindControl("rdoNam"));
            HtmlInputRadioButton rdoNu = ((HtmlInputRadioButton)listViewDataItem_CaNhan.FindControl("rdoNu"));

            DropDownList pDropDownListXa = ((DropDownList)listViewDataItem_CaNhan.FindControl("drlXa"));
            DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem_CaNhan.FindControl("drlQuanHuyen"));
            DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem_CaNhan.FindControl("drlTinhThanhPho"));
            DropDownList drlQuocTich = ((DropDownList)listViewDataItem_CaNhan.FindControl("drlQuocTich"));
            DropDownList drlDanToc = ((DropDownList)listViewDataItem_CaNhan.FindControl("drlDanToc"));
            CANHAN objCANHAN = new CANHAN();
            objCANHAN.CANHAN_ID = Int32.Parse(txtCaNhanID.Text);
            objCANHAN.CANHAN_HOTEN = txtHoTen.Text;
            //objCANHAN.lan = Int32.Parse(txtLanTiep.Text);
            objCANHAN.CANHAN_CMDN = txtCMND.Text;
            if (txtNgayCap.Text != "")
            {
                objCANHAN.CANHAN_CMDN_NGAYCAP = DateTime.Parse(txtNgayCap.Text);
            }
            else
            {
                objCANHAN.CANHAN_CMDN_NGAYCAP = null;
            }

            objCANHAN.CANHAN_NOICAP = txtNoiCap.Text;
            objCANHAN.CANHAN_DIACHI = txtDiaChi.Text;
            objCANHAN.CANHAN_GIOITINH = !rdoNam.Checked;
            if (pDropDownListXa.SelectedValue != "")
            {
                objCANHAN.DP_ID = Int32.Parse(pDropDownListXa.SelectedValue);
            }
            else if (pDropDownListHuyen.SelectedValue != "")
            {
                objCANHAN.DP_ID = Int32.Parse(pDropDownListHuyen.SelectedValue);
            }
            else if (pDropDownListThanhpho.SelectedValue != "")
            {
                objCANHAN.DP_ID = Int32.Parse(pDropDownListThanhpho.SelectedValue);
            }
            else
            {
                objCANHAN.DP_ID = -1;
            }
            if (drlQuocTich.SelectedValue != "")
            {
                objCANHAN.QUOCTICH_ID = Int32.Parse(drlQuocTich.SelectedValue);
            }
            else
            {
                objCANHAN.QUOCTICH_ID = -1;
            }
            if (drlDanToc.SelectedValue != "")
            {
                objCANHAN.DANTOC_ID = Int32.Parse(drlDanToc.SelectedValue);
            }
            else
            {
                objCANHAN.DANTOC_ID = -1;
            }
            return objCANHAN;
        }


        protected void ListViewDoiTuong_LayoutCreated(object sender, EventArgs e)
        {

        }

        protected void ListViewDoiTuong_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            try
            {
                ListView lv = sender as ListView;
                ListViewDataItem item = e.Item as ListViewDataItem;

                if (item.DataItemIndex == 0)
                {
                    HtmlAnchor html = (HtmlAnchor)e.Item.FindControl("Xoa_CaNhan");
                    html.Visible = false;
                }

                HtmlInputRadioButton rdoNam = ((HtmlInputRadioButton)e.Item.FindControl("rdoNam"));
                HtmlGenericControl lblFoNam = ((HtmlGenericControl)e.Item.FindControl("lblforrdoNam"));

                lblFoNam.Attributes.Add("for", ListViewDoiTuong.ClientID + "_" + rdoNam.ClientID.Replace("ctrl" + item.DataItemIndex + "_", ""));

                HtmlInputRadioButton rdoNu = ((HtmlInputRadioButton)e.Item.FindControl("rdoNu"));
                HtmlGenericControl lblFoNu = ((HtmlGenericControl)e.Item.FindControl("lblforrdoNu"));
                lblFoNu.Attributes.Add("for", ListViewDoiTuong.ClientID + "_" + rdoNu.ClientID.Replace("ctrl" + item.DataItemIndex + "_", ""));
            }
            catch (Exception Ex)
            {
            }
        }
        public void LoadDiaPhuong(int vDP_ID, DropDownList pDropDownListXa, DropDownList pDropDownListHuyen, DropDownList pDropDownListThanhpho)
        {
            try
            {
                vDiaPhuongInfos = vDataContext.DIAPHUONGs.ToList();
                if (vDP_ID == -1)
                {
                    List<DIAPHUONG> DIAPHUONGTinhs = vDiaPhuongInfos.Where(x => x.CapDo == 1).ToList();
                    pDropDownListThanhpho.DataSource = DIAPHUONGTinhs;
                    pDropDownListThanhpho.DataTextField = "DP_TEN";
                    pDropDownListThanhpho.DataValueField = "DP_ID";
                    pDropDownListThanhpho.DataBind();
                    pDropDownListThanhpho.Items.Insert(0, new ListItem("Chọn Tỉnh/thành phố", "-1"));
                    pDropDownListThanhpho.SelectedValue = "-1";

                    pDropDownListHuyen.DataBind();
                    pDropDownListHuyen.Items.Insert(0, new ListItem("Chọn Quận/huyện", "-1"));
                    pDropDownListHuyen.SelectedValue = "";

                    pDropDownListXa.Items.Clear();
                    pDropDownListXa.Items.Insert(0, new ListItem("Chọn Xã/Phường", "-1"));
                    pDropDownListXa.SelectedValue = "-1";
                }
                else
                {
                    DIAPHUONG dIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                    if (dIAPHUONG != null)
                    {
                        if (dIAPHUONG.CapDo == 3)
                        {
                            List<DIAPHUONG> DIAPHUONGXas = vDiaPhuongInfos.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID_CHA).ToList();
                            DIAPHUONG DIAPHUONGHuyen = vDiaPhuongInfos.Where(x => x.DP_ID == dIAPHUONG.DP_ID_CHA).FirstOrDefault();
                            List<DIAPHUONG> DIAPHUONGHuyens = vDiaPhuongInfos.Where(x => x.DP_ID_CHA == DIAPHUONGHuyen.DP_ID_CHA).ToList();
                            DIAPHUONG DIAPHUONGTinh = vDiaPhuongInfos.Where(x => x.DP_ID == DIAPHUONGHuyen.DP_ID_CHA).FirstOrDefault();
                            List<DIAPHUONG> DIAPHUONGTinhs = vDiaPhuongInfos.Where(x => x.CapDo == 1).ToList();
                            pDropDownListXa.DataSource = DIAPHUONGXas;
                            pDropDownListXa.DataTextField = "DP_TEN";
                            pDropDownListXa.DataValueField = "DP_ID";
                            pDropDownListXa.DataBind();
                            pDropDownListHuyen.DataSource = DIAPHUONGHuyens;
                            pDropDownListHuyen.DataTextField = "DP_TEN";
                            pDropDownListHuyen.DataValueField = "DP_ID";
                            pDropDownListHuyen.DataBind();
                            pDropDownListThanhpho.DataSource = DIAPHUONGTinhs;
                            pDropDownListThanhpho.DataTextField = "DP_TEN";
                            pDropDownListThanhpho.DataValueField = "DP_ID";
                            pDropDownListThanhpho.DataBind();

                            pDropDownListXa.SelectedValue = vDP_ID.ToString();
                            pDropDownListHuyen.SelectedValue = DIAPHUONGHuyen.DP_ID.ToString();
                            pDropDownListThanhpho.SelectedValue = DIAPHUONGTinh.DP_ID.ToString();
                        }
                        else if (dIAPHUONG.CapDo == 2)
                        {
                            List<DIAPHUONG> DIAPHUONGXas = vDiaPhuongInfos.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID).ToList();
                            pDropDownListXa.DataSource = DIAPHUONGXas;
                            pDropDownListXa.DataTextField = "DP_TEN";
                            pDropDownListXa.DataValueField = "DP_ID";
                            pDropDownListXa.DataBind();
                            pDropDownListXa.Items.Insert(0, new ListItem("Chọn Xã/Phường", "-1"));
                            pDropDownListXa.SelectedValue = "-1";

                        }
                        else if (dIAPHUONG.CapDo == 1)
                        {
                            pDropDownListHuyen.Items.Clear();

                            List<DIAPHUONG> DIAPHUONGHuyens = vDiaPhuongInfos.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID).ToList();
                            pDropDownListHuyen.DataSource = DIAPHUONGHuyens;
                            pDropDownListHuyen.DataTextField = "DP_TEN";
                            pDropDownListHuyen.DataValueField = "DP_ID";
                            pDropDownListHuyen.DataBind();
                            pDropDownListHuyen.Items.Insert(0, new ListItem("Chọn Quận/huyện", "-1"));
                            pDropDownListHuyen.SelectedValue = "-1";

                            pDropDownListXa.Items.Clear();
                            pDropDownListXa.Items.Insert(0, new ListItem("Chọn Xã/Phường", "-1"));
                            pDropDownListXa.SelectedValue = "-1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void LoadDanToc(int vDANTOCP_ID, DropDownList pdrlDanToc)
        {
            try
            {
                vDanTocInfos = vDataContext.DANTOCs.ToList(); ;
                pdrlDanToc.DataSource = vDanTocInfos;
                pdrlDanToc.DataTextField = "DANTOC_TEN";
                pdrlDanToc.DataValueField = "DANTOC_ID";
                pdrlDanToc.DataBind();
                pdrlDanToc.Items.Insert(0, new ListItem("Chọn Dân tộc", "-1"));
                pdrlDanToc.SelectedValue = vDANTOCP_ID.ToString();
            }
            catch (Exception ex)
            {

            }
        }
        public void LoadQuocTich(int vQUOCTICH_ID, DropDownList pdrlQuocTich)
        {
            try
            {
                vQuocTichInfos = vDataContext.QUOCTICHes.ToList(); ;
                pdrlQuocTich.DataSource = vQuocTichInfos;
                pdrlQuocTich.DataTextField = "QUOCTICH_TEN";
                pdrlQuocTich.DataValueField = "QUOCTICH_ID";
                pdrlQuocTich.DataBind();
                pdrlQuocTich.Items.Insert(0, new ListItem("Chọn Quốc tịch", "-1"));
                pdrlQuocTich.SelectedValue = vQUOCTICH_ID.ToString();
            }
            catch (Exception ex)
            {

            }
        }


        protected void drlDIAPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList drlTinhThanhPho = (DropDownList)sender;
                ListViewItem item = (ListViewItem)drlTinhThanhPho.NamingContainer;

                DropDownList pDropDownListXa = ((DropDownList)item.FindControl("drlXa"));
                DropDownList pDropDownListHuyen = ((DropDownList)item.FindControl("drlQuanHuyen"));
                DropDownList pDropDownListThanhpho = ((DropDownList)item.FindControl("drlTinhThanhPho"));
                LoadDiaPhuong(Int32.Parse(drlTinhThanhPho.SelectedValue), pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
            }
            catch (Exception Ex)
            { }

            //dIAPHUONGs = vDataContext.DIAPHUONGs.ToList();
            //drlLoaiTiepDan.DataSource = dIAPHUONGs;
            //drlLoaiTiepDan.DataTextField = "DP_TEN";
            //drlLoaiTiepDan.DataValueField = "DP_ID";
            //drlLoaiTiepDan.DataBind();
        }



        protected void textSoNguoi_TextChanged(object sender, EventArgs e)
        {
            Loadbtn_ThemNguoiDaiDien();
        }

        protected void drlLoaiKieuNai_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList DropDownListLoai = (DropDownList)sender;
                if (DropDownListLoai.SelectedValue == "")
                {
                    LoadLoaiDonThu(0, true);
                }
                else
                {
                    LoadLoaiDonThu(Int32.Parse(DropDownListLoai.SelectedValue), true);
                }
            }
            catch (Exception ex)
            {

            }
        }



        protected void Xoa_CaNhan_ServerClick(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                string vId = html.HRef;

                List<CANHAN> cANHANs = new List<CANHAN>();
                foreach (var item in ListViewDoiTuong.Items)
                {
                    CANHAN cANHAN = new CANHAN();
                    cANHAN = GetThongTinCaNhan(item);
                    cANHANs.Add(cANHAN);
                }
                cANHANs.Remove(cANHANs.Where(x => x.CANHAN_ID == Int32.Parse(vId)).FirstOrDefault());
                ListViewDoiTuong.DataSource = cANHANs;
                ListViewDoiTuong.DataBind();

                for (int i = 0; i < cANHANs.Count(); i++)
                {

                    ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                    TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                    if (txtCaNhanID.Text != "")
                    {
                        int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);

                        DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                        DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                        DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));

                        DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                        DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));

                        LoadDiaPhuong((int)cANHANs[i].DP_ID, pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
                        LoadDanToc((int)cANHANs[i].DANTOC_ID, drlDanToc);
                        LoadQuocTich((int)cANHANs[i].QUOCTICH_ID, drlQuocTich);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
        }
        #endregion

        #region Xử lý hiển thị / ẩn form 
        /// <summary>
        /// Load chức vụ khi chọn người duyệt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChonNguoiDuyet(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlistNguoiDuyet.SelectedValue))
                {
                    var vChucVuInFo = vDataContext.CANBOs.Where(x => x.CANBO_ID == int.Parse(ddlistNguoiDuyet.SelectedValue)).Select(x => x.CHUCVU).FirstOrDefault();
                    if (vChucVuInFo != null)
                    {
                        textChucVu_HuongXuLy.Text = vChucVuInFo.TENCHUCVU;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Chọn nguồn đơn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChonNguonDon(object sender, EventArgs e)
        {
            try
            {
                if (ddlistNguon.SelectedValue == "2")
                {
                    divCoQuanChuyenDon.Visible = true;
                }
                else
                {
                    divCoQuanChuyenDon.Visible = false;
                }

                if (ddlistNguon.SelectedValue == "0")
                {
                    divNacDanh.Visible = false;
                }
                else
                {
                    if (ddlistDoiTuong.SelectedValue == "1")
                    {
                        divNacDanh.Visible = true;
                    }
                    else
                    {
                        divNacDanh.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Chọn đối tượng của đơn thư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChonDoiTuong(object sender, EventArgs e)
        {
            try
            {
                Loadbtn_ThemNguoiDaiDien();
                if (ddlistDoiTuong.SelectedValue == "3")
                {
                    divDoiTuong.Visible = true;
                    textSoNguoi.Enabled = false;
                    textSoNguoiDaiDien.Enabled = false;
                }
                else
                {
                    divDoiTuong.Visible = false;
                }
                if (ddlistDoiTuong.SelectedValue == "1")
                {
                    textSoNguoi.Enabled = true;
                    textSoNguoiDaiDien.Enabled = false;
                    if (ddlistNguon.SelectedValue != "0")
                    {
                        divNacDanh.Visible = true;
                    }
                    else
                    {
                        divNacDanh.Visible = false;
                    }
                }
                else
                {
                    textSoNguoi.Enabled = true;
                    textSoNguoi.Enabled = true;
                    divNacDanh.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void Loadbtn_ThemNguoiDaiDien()
        {
            try
            {
                int oSoNguoi;
                int oSoNguoiDaiDien;

                if (int.TryParse(textSoNguoi.Text.Trim(), out oSoNguoi) && int.TryParse(textSoNguoiDaiDien.Text.Trim(), out oSoNguoiDaiDien))
                {
                    if (oSoNguoiDaiDien > oSoNguoi)
                    {
                        ClassCommon.ShowToastr(Page, "Số người đại diện không được lớn hơn số người", "Thông báo", "error");
                        textSoNguoiDaiDien.Text = textSoNguoi.Text.Trim();
                    }

                }
                if (ddlistDoiTuong.SelectedValue == "1")
                {
                    btn_ThemNguoiDaiDien.Visible = false;
                    textSoNguoiDaiDien.Text = "1";
                    textSoNguoiDaiDien.Enabled = false;
                }
                else if (ddlistDoiTuong.SelectedValue == "2")
                {

                    if (Int16.Parse(textSoNguoiDaiDien.Text) > 1)
                    {
                        btn_ThemNguoiDaiDien.Visible = true;
                    }
                    textSoNguoiDaiDien.Enabled = true;
                }
                else if (ddlistDoiTuong.SelectedValue == "3")
                {
                    if (Int16.Parse(textSoNguoiDaiDien.Text) > 1)
                    {
                        btn_ThemNguoiDaiDien.Visible = true;
                    }

                    textSoNguoiDaiDien.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Check đơn thư nặc danh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboxDonThuNacDanh_ServerChange(object sender, EventArgs e)
        {
            try
            {
                if (cboxDonThuNacDanh.Checked)
                {
                    divDonThuKhongDuDieuKien.Visible = false;
                    List<CANHAN> cANHANs = new List<CANHAN>();
                    CANHAN cANHAN = new CANHAN();
                    cANHANs.Add(cANHAN);
                    cANHAN.CANHAN_ID = 0;
                    cANHAN.CANHAN_GIOITINH = false;
                    cANHAN.CANHAN_HOTEN = "Đơn thư nặc danh";
                    ListViewDoiTuong.DataSource = cANHANs;
                    ListViewDoiTuong.DataBind();
                    //txtHoTen.Text = "Đơn thư nặc danh";
                }
                else
                {
                    divDonThuKhongDuDieuKien.Visible = true;
                    textHoTen_NguoiBiKhieuNaiToCao.Text = "";
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Check đơn thư không đủ điều kiện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboxDonThuKhongDuDieuKien_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxDonThuKhongDuDieuKien.Checked)
                {
                    divHuongXuLy.Visible = false;
                }
                else
                {
                    divHuongXuLy.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Check bổ sung thông tin cơ quan đã giải quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbCoQuanDaGiaiQuyet_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxCoQuanDaGiaiQuyet.Checked)
                {
                    divCoQuanDaGiaiQuyet.Visible = true;
                }
                else
                {
                    divCoQuanDaGiaiQuyet.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Check bổ sung thông tin người bị khiếu nại, tố cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbBoSungThongTinNguoiBiKhieuNaiToCao_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked)
                {
                    divThongTinNguoiBiKhieuNaiToCao.Visible = true;
                }
                else
                {
                    divThongTinNguoiBiKhieuNaiToCao.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Check bổ sung thông tin người đại diện, ủy quyền
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbBoSungThongTinNguoiDaiDienUyQuyem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked)
                {
                    divThongTinNguoiDaiDienUyQuyen.Visible = true;
                }
                else
                {
                    divThongTinNguoiDaiDienUyQuyen.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Chọn hướng xử lý, ẩn hiện control trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChonHuongXuLy(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlistHuongXuLy.SelectedValue))
                {
                    if (ddlistHuongXuLy.SelectedValue == "1" ||
                        ddlistHuongXuLy.SelectedValue == "5" ||
                        ddlistHuongXuLy.SelectedValue == "6" ||
                        ddlistHuongXuLy.SelectedValue == "7" ||
                        ddlistHuongXuLy.SelectedValue == "8" ||
                        ddlistHuongXuLy.SelectedValue == "9" ||
                        ddlistHuongXuLy.SelectedValue == "10")
                    {
                        divCoQuanTiepNhan_HuongXuLy.Visible = false;
                    }
                    else
                    {
                        divCoQuanTiepNhan_HuongXuLy.Visible = true;
                        if (ddlistHuongXuLy.SelectedValue == "2" || ddlistHuongXuLy.SelectedValue == "3")
                        {
                            divNgayChuyen_HuongXuLy.Visible = true;
                        }
                        else
                        {
                            divNgayChuyen_HuongXuLy.Visible = false;
                        }
                    }
                }
                else
                {
                    divCoQuanTiepNhan_HuongXuLy.Visible = false;
                    divNgayChuyen_HuongXuLy.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Chọn loại đối tượng bị khiếu nại, tố cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChonLoaiDoiTuongBiKhieuNaoToCao(object sender, EventArgs e)
        {
            try
            {
                if (ddlistDoiTuongBoSung.SelectedValue == "1")
                {
                    divTenCoQuanBiKhieuNaiToCao.Visible = false;
                    divQuocTichDanToc_NguoiBiKhieuNaiToCao.Visible = true;
                    divTenCaNhan_BiKhieuNaiToCao.Visible = true;
                }
                else
                {
                    divTenCoQuanBiKhieuNaiToCao.Visible = true;
                    divQuocTichDanToc_NguoiBiKhieuNaiToCao.Visible = false;
                    divTenCaNhan_BiKhieuNaiToCao.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Xử lý tập tin hồ sơ đơn thư
        protected void LuuHoSo(object sender, EventArgs e)
        {
            try
            {
                if (!fileHoSoDinhKem.HasFile)
                {
                    ClassCommon.ShowToastr(Page, "Vui lòng chọn hồ sơ đính kèm", "Thông báo", "error");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosodonthu').modal('show');", true);
                }
                else
                {
                    if (textTenHoSo.Text.Trim() == "")
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng nhập Tên hồ sơ", "Thông báo", "error");
                        textTenHoSo.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosodonthu').modal('show');", true);
                    }
                    else
                    {

                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;

                        List<HOSO> vHoSoInfos = new List<HOSO>();

                        //for (int i = 0; i < uploadedFiles.Count; i++)
                        //{
                        HttpPostedFile userPostedFile = uploadedFiles[0];//Thứ tự control file từ trên xuống
                                                                         //File cho phép: .jpg, jpng, .jpeg, .doc, .docx, .xls, .xlsx, pdf
                        if (userPostedFile.ContentType == "image/jpg" || userPostedFile.ContentType == "image/png" || userPostedFile.ContentType == "image/jpeg" || userPostedFile.ContentType == "application/msword" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || userPostedFile.ContentType == "application/pdf" || userPostedFile.ContentType == "application/vnd.ms-excel" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            if (userPostedFile.ContentLength < 1048576 * 50)//100MB
                            {
                                string filename = userPostedFile.FileName;
                                string extension = System.IO.Path.GetExtension(filename);
                                string vFilename = filename.Substring(0, filename.Length - extension.Length) + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + extension;
                                ClassCommon.UploadFile(userPostedFile, filepath, vFilename, "");
                                HOSO vHoSoInfo = new HOSO();
                                vHoSoInfo.HOSO_TEN = ClassCommon.ClearHTML(textTenHoSo.Text.Trim());
                                vHoSoInfo.HOSO_TOMTAT = ClassCommon.ClearHTML(textNoiDungTomTat.Text.Trim());
                                vHoSoInfo.NGAYTAO = DateTime.Now;
                                vHoSoInfo.NGUOITAO = _currentUser.UserID;
                                vHoSoInfo.NGAYCAPNHAT = DateTime.Now;
                                vHoSoInfo.NGUOICAPNHAT = _currentUser.UserID;
                                if (!String.IsNullOrEmpty(ddlistLoaiHoSo.SelectedValue))
                                {
                                    vHoSoInfo.LOAIHOSO_ID = int.Parse(ddlistLoaiHoSo.SelectedValue);
                                }
                                else
                                {
                                    vHoSoInfo.LOAIHOSO_ID = 2; // Loại khác
                                }
                                vHoSoInfo.HOSO_FILE = vFilename;
                                vHoSoInfos.Add(vHoSoInfo);
                                //vHoSoController.ThemMoiHoSo(vHoSoInfo, out oHoSoId, out oErrorMessage);
                                //if (oHoSoId > 0)
                                //{
                                //    DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                //    vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                //    vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                //    vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư
                                //    vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                //    //Load lại danh sách đơn thư
                                //}
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng chọn file kích thước nhỏ hơn 50MB", "Thông báo", "error");
                            }
                        }
                        else
                        {
                            ClassCommon.ShowToastr(Page, "Vui lòng chọn file đúng định dạng", "Thông báo", "error");
                        }
                        //}//End for
                        if (vHoSoInfos.Count > 0)
                        {
                            if (vDonThuId > 0)
                            {

                                foreach (var vHoSoInfo in vHoSoInfos)
                                {
                                    long oHoSoId = 0;
                                    string oErrorMessage = "";
                                    HOSO vHoSoInfo_New = new HOSO();
                                    vHoSoInfo_New.HOSO_TEN = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.HOSO_TOMTAT = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.NGAYTAO = vHoSoInfo.NGAYTAO;
                                    vHoSoInfo_New.NGUOITAO = vHoSoInfo.NGUOITAO;
                                    vHoSoInfo_New.NGAYCAPNHAT = vHoSoInfo.NGAYCAPNHAT;
                                    vHoSoInfo_New.NGUOICAPNHAT = vHoSoInfo.NGUOICAPNHAT;
                                    vHoSoInfo_New.HOSO_FILE = vHoSoInfo.HOSO_FILE;
                                    vHoSoInfo_New.LOAIHOSO_ID = vHoSoInfo.LOAIHOSO_ID;
                                    vHoSoController.ThemMoiHoSo(vHoSoInfo_New, out oHoSoId, out oErrorMessage);
                                    if (oHoSoId > 0)
                                    {
                                        DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                        vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] == null)
                                {
                                    Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] = new List<HOSO>();
                                }
                                List<HOSO> vDonThuInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"];
                                foreach (var item in vHoSoInfos)
                                {
                                    Int32 vID_TEMP = Int32.Parse(DateTime.Now.ToString("ddHHmmss"));
                                    item.HOSO_ID = vID_TEMP;
                                    vDonThuInfo_Session.Insert(0, item);
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] = vDonThuInfo_Session;
                            }

                            //textTenTaiLieu.Text = "";
                            //textMotaFile.Text = "";
                        }
                        LoadDanhSachHoSoDonThu(vDonThuId);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Load danh sách hồ sơ đơn thư
        /// </summary>
        /// <param name="pDonThuId"></param>
        public void LoadDanhSachHoSoDonThu(int pDonThuId)
        {
            List<HOSO> vHoSoInfos = new List<HOSO>();
            if (pDonThuId > 0)
            {
                vHoSoInfos = (from vHoSo in vDataContext.HOSOs
                              join vDonThuHoSo in vDataContext.DONTHU_HOSOs on vHoSo.HOSO_ID equals vDonThuHoSo.HOSO_ID
                              where vDonThuHoSo.DONTHU_ID == pDonThuId && vDonThuHoSo.LOAI_HS_DONTHU == 0 //Loại hồ sơ đơn thư
                              select vHoSo).ToList();

            }
            else
            {
                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] != null)
                {
                    vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"];
                }
            }

            dgDanhSach_File_HoSoDonThu.DataSource = vHoSoInfos;
            dgDanhSach_File_HoSoDonThu.DataBind();
            if (vHoSoInfos.Count > 0)
            {
                dgDanhSach_File_HoSoDonThu.Visible = true;
            }
            else
            {
                dgDanhSach_File_HoSoDonThu.Visible = false;
            }
        }

        /// <summary>
        /// Xóa hồ sơ đơn thư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaHoSoDonThu(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vHoSoId = int.Parse(html.HRef);
                if (vDonThuId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] != null)
                    {
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"];
                        HOSO vHoSoInfo_Delete = vHoSoInfos.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        if (vHoSoInfo_Delete != null)
                        {
                            File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                            vHoSoInfos.Remove(vHoSoInfo_Delete);
                            Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] = vHoSoInfos;
                        }
                    }
                }
                else
                {
                    HOSO vHoSoInfo_Delete = vDataContext.HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                    if (vHoSoInfo_Delete != null)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                        DONTHU_HOSO vHoSoDonThuInfos = vDataContext.DONTHU_HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        vDataContext.DONTHU_HOSOs.DeleteOnSubmit(vHoSoDonThuInfos);
                        vDataContext.HOSOs.DeleteOnSubmit(vHoSoInfo_Delete);
                        vDataContext.SubmitChanges();
                    }
                }
                LoadDanhSachHoSoDonThu(vDonThuId);
                ClassCommon.ShowToastr(Page, "Xóa hồ sơ đơn thư thành công", "Thông báo", "success");
            }
            catch (Exception ex)
            {

            }
        }

        public void XoaSessionFile()
        {
            try
            {
                List<HOSO> vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"];
                if (vHoSoInfos.Count > 0)
                {
                    foreach (var vHoSoInfo in vHoSoInfos)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo.HOSO_FILE);
                    }
                }
                Session.Remove(PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Xử lý tập tin hồ sơ hướng xử lý
        protected void LuuHoSoHuongXuLy(object sender, EventArgs e)
        {
            try
            {
                if (!fileHoSoDinhKem_HuongXuLy.HasFile)
                {
                    ClassCommon.ShowToastr(Page, "Vui lòng chọn hồ sơ đính kèm", "Thông báo", "error");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosohuongxuly').modal('show');", true);
                }
                else
                {
                    if (textTenHoSo_HuongXuLy.Text.Trim() == "")
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng nhập Tên hồ sơ", "Thông báo", "error");
                        textTenHoSo_HuongXuLy.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosohuongxuly').modal('show');", true);
                    }
                    else
                    {
                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;

                        List<HOSO> vHoSoInfos = new List<HOSO>();

                        //for (int i = 0; i < uploadedFiles.Count; i++)
                        //{
                        HttpPostedFile userPostedFile = uploadedFiles[1];//Thứ tự control file từ trên xuống
                                                                         //File cho phép: .jpg, jpng, .jpeg, .doc, .docx, .xls, .xlsx, pdf
                        if (userPostedFile.ContentType == "image/jpg" || userPostedFile.ContentType == "image/png" || userPostedFile.ContentType == "image/jpeg" || userPostedFile.ContentType == "application/msword" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || userPostedFile.ContentType == "application/pdf" || userPostedFile.ContentType == "application/vnd.ms-excel" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            if (userPostedFile.ContentLength < 1048576 * 50)//100MB
                            {
                                string filename = userPostedFile.FileName;
                                string extension = System.IO.Path.GetExtension(filename);
                                string vFilename = filename.Substring(0, filename.Length - extension.Length) + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + extension;
                                ClassCommon.UploadFile(userPostedFile, filepath, vFilename, "");
                                HOSO vHoSoInfo = new HOSO();
                                vHoSoInfo.HOSO_TEN = ClassCommon.ClearHTML(textTenHoSo_HuongXuLy.Text.Trim());
                                vHoSoInfo.HOSO_TOMTAT = ClassCommon.ClearHTML(textNoiDungTomTat_HuongXuLy.Text.Trim());
                                vHoSoInfo.NGAYTAO = DateTime.Now;
                                vHoSoInfo.NGUOITAO = _currentUser.UserID;
                                vHoSoInfo.NGAYCAPNHAT = DateTime.Now;
                                vHoSoInfo.NGUOICAPNHAT = _currentUser.UserID;
                                if (!String.IsNullOrEmpty(ddlistLoaiHoSo_HuongXuLy.SelectedValue))
                                {
                                    vHoSoInfo.LOAIHOSO_ID = int.Parse(ddlistLoaiHoSo_HuongXuLy.SelectedValue);
                                }
                                else
                                {
                                    vHoSoInfo.LOAIHOSO_ID = 2; // Loại khác
                                }
                                vHoSoInfo.HOSO_FILE = vFilename;
                                vHoSoInfos.Add(vHoSoInfo);
                                //vHoSoController.ThemMoiHoSo(vHoSoInfo, out oHoSoId, out oErrorMessage);
                                //if (oHoSoId > 0)
                                //{
                                //    DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                //    vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                //    vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                //    vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư
                                //    vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                //    //Load lại danh sách đơn thư
                                //}
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng chọn file kích thước nhỏ hơn 50MB", "Thông báo", "error");
                            }
                        }
                        else
                        {
                            ClassCommon.ShowToastr(Page, "Vui lòng chọn file đúng định dạng", "Thông báo", "error");
                        }
                        //}//End for
                        if (vHoSoInfos.Count > 0)
                        {
                            if (vDonThuId > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfos)
                                {
                                    long oHoSoId = 0;
                                    string oErrorMessage = "";
                                    HOSO vHoSoInfo_New = new HOSO();
                                    vHoSoInfo_New.HOSO_TEN = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.HOSO_TOMTAT = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.NGAYTAO = vHoSoInfo.NGAYTAO;
                                    vHoSoInfo_New.NGUOITAO = vHoSoInfo.NGUOITAO;
                                    vHoSoInfo_New.NGAYCAPNHAT = vHoSoInfo.NGAYCAPNHAT;
                                    vHoSoInfo_New.NGUOICAPNHAT = vHoSoInfo.NGUOICAPNHAT;
                                    vHoSoInfo_New.HOSO_FILE = vHoSoInfo.HOSO_FILE;
                                    vHoSoInfo_New.LOAIHOSO_ID = vHoSoInfo.LOAIHOSO_ID;
                                    vHoSoController.ThemMoiHoSo(vHoSoInfo_New, out oHoSoId, out oErrorMessage);
                                    if (oHoSoId > 0)
                                    {
                                        DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                        vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 4;//Hồ sơ đơn thư
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] == null)
                                {
                                    Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] = new List<HOSO>();
                                }
                                List<HOSO> vDonThuInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"];
                                foreach (var item in vHoSoInfos)
                                {
                                    Int32 vID_TEMP = Int32.Parse(DateTime.Now.ToString("ddHHmmss"));
                                    item.HOSO_ID = vID_TEMP;
                                    vDonThuInfo_Session.Insert(0, item);
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] = vDonThuInfo_Session;
                            }

                            //textTenTaiLieu.Text = "";
                            //textMotaFile.Text = "";
                        }
                        LoadDanhSachHoSoHuongXuLy(vDonThuId);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Load danh sách hồ sơ đơn thư
        /// </summary>
        /// <param name="pDonThuId"></param>
        public void LoadDanhSachHoSoHuongXuLy(int pDonThuId)
        {
            List<HOSO> vHoSoInfos = new List<HOSO>();
            if (pDonThuId > 0)
            {
                vHoSoInfos = (from vHoSo in vDataContext.HOSOs
                              join vDonThuHoSo in vDataContext.DONTHU_HOSOs on vHoSo.HOSO_ID equals vDonThuHoSo.HOSO_ID
                              where vDonThuHoSo.DONTHU_ID == pDonThuId && vDonThuHoSo.LOAI_HS_DONTHU == 4//Loại hướng xử lý
                              select vHoSo).ToList();

            }
            else
            {
                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] != null)
                {
                    vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"];
                }
            }

            dgDanhSach_File_HuongXuLy.DataSource = vHoSoInfos;
            dgDanhSach_File_HuongXuLy.DataBind();
            if (vHoSoInfos.Count > 0)
            {
                dgDanhSach_File_HuongXuLy.Visible = true;
            }
            else
            {
                dgDanhSach_File_HuongXuLy.Visible = false;
            }
        }

        /// <summary>
        /// Xóa hồ sơ đơn thư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaHoSoDonThu_HuongXuLy(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vHoSoId = int.Parse(html.HRef);
                if (vDonThuId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] != null)
                    {
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"];
                        HOSO vHoSoInfo_Delete = vHoSoInfos.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        if (vHoSoInfo_Delete != null)
                        {
                            File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                            vHoSoInfos.Remove(vHoSoInfo_Delete);
                            Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] = vHoSoInfos;
                        }
                    }
                }
                else
                {
                    HOSO vHoSoInfo_Delete = vDataContext.HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                    if (vHoSoInfo_Delete != null)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                        DONTHU_HOSO vHoSoDonThuInfos = vDataContext.DONTHU_HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        vDataContext.DONTHU_HOSOs.DeleteOnSubmit(vHoSoDonThuInfos);
                        vDataContext.HOSOs.DeleteOnSubmit(vHoSoInfo_Delete);
                        vDataContext.SubmitChanges();
                    }
                }
                LoadDanhSachHoSoHuongXuLy(vDonThuId);
                ClassCommon.ShowToastr(Page, "Xóa hồ sơ hướng xử lý thành công", "Thông báo", "success");
            }
            catch (Exception ex)
            {

            }
        }

        public void XoaSessionFile_HuongXuLy()
        {
            try
            {
                List<HOSO> vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"];
                if (vHoSoInfos.Count > 0)
                {
                    foreach (var vHoSoInfo in vHoSoInfos)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo.HOSO_FILE);
                    }
                }
                Session.Remove(PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Hồ sơ người đại diện, ủy quyền        
        protected void LuuHoSoNguoiDaiDienUyQuyen(object sender, EventArgs e)
        {
            try
            {
                if (!fileThongTinNguoiDaiDienUyQuyen.HasFile)
                {
                    ClassCommon.ShowToastr(Page, "Vui lòng chọn hồ sơ đính kèm", "Thông báo", "error");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosonguoidaidienuyquyen').modal('show');", true);
                }
                else
                {
                    if (textTenHoSoNguoiDaiDienUyQuyen.Text.Trim() == "")
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng nhập Tên hồ sơ", "Thông báo", "error");
                        textTenHoSoNguoiDaiDienUyQuyen.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosonguoidaidienuyquyen').modal('show');", true);
                    }
                    else
                    {
                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;

                        List<HOSO> vHoSoInfos = new List<HOSO>();

                        //for (int i = 0; i < uploadedFiles.Count; i++)
                        //{
                        HttpPostedFile userPostedFile = uploadedFiles[2];//Thứ tự control file từ trên xuống
                                                                         //File cho phép: .jpg, jpng, .jpeg, .doc, .docx, .xls, .xlsx, pdf
                        if (userPostedFile.ContentType == "image/jpg" || userPostedFile.ContentType == "image/png" || userPostedFile.ContentType == "image/jpeg" || userPostedFile.ContentType == "application/msword" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || userPostedFile.ContentType == "application/pdf" || userPostedFile.ContentType == "application/vnd.ms-excel" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            if (userPostedFile.ContentLength < 1048576 * 50)//100MB
                            {
                                string filename = userPostedFile.FileName;
                                string extension = System.IO.Path.GetExtension(filename);
                                string vFilename = filename.Substring(0, filename.Length - extension.Length) + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + extension;
                                ClassCommon.UploadFile(userPostedFile, filepath, vFilename, "");
                                HOSO vHoSoInfo = new HOSO();
                                vHoSoInfo.HOSO_TEN = ClassCommon.ClearHTML(textTenHoSoNguoiDaiDienUyQuyen.Text.Trim());
                                vHoSoInfo.HOSO_TOMTAT = ClassCommon.ClearHTML(textNoiDungTomTatFileNguoiDaiDienUyQuyen.Text.Trim());
                                vHoSoInfo.NGAYTAO = DateTime.Now;
                                vHoSoInfo.NGUOITAO = _currentUser.UserID;
                                vHoSoInfo.NGAYCAPNHAT = DateTime.Now;
                                vHoSoInfo.NGUOICAPNHAT = _currentUser.UserID;
                                if (!String.IsNullOrEmpty(ddlistLoaiHoSoThongTinNguoiDaiDienUyQuyen.SelectedValue))
                                {
                                    vHoSoInfo.LOAIHOSO_ID = int.Parse(ddlistLoaiHoSoThongTinNguoiDaiDienUyQuyen.SelectedValue);
                                }
                                else
                                {
                                    vHoSoInfo.LOAIHOSO_ID = 2; // Loại khác
                                }
                                vHoSoInfo.HOSO_FILE = vFilename;
                                vHoSoInfos.Add(vHoSoInfo);
                                //vHoSoController.ThemMoiHoSo(vHoSoInfo, out oHoSoId, out oErrorMessage);
                                //if (oHoSoId > 0)
                                //{
                                //    DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                //    vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                //    vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                //    vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư
                                //    vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                //    //Load lại danh sách đơn thư
                                //}
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng chọn file kích thước nhỏ hơn 50MB", "Thông báo", "error");
                            }
                        }
                        else
                        {
                            ClassCommon.ShowToastr(Page, "Vui lòng chọn file đúng định dạng", "Thông báo", "error");
                        }
                        //}//End for
                        if (vHoSoInfos.Count > 0)
                        {
                            if (vDonThuId > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfos)
                                {
                                    long oHoSoId = 0;
                                    string oErrorMessage = "";
                                    HOSO vHoSoInfo_New = new HOSO();
                                    vHoSoInfo_New.HOSO_TEN = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.HOSO_TOMTAT = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.NGAYTAO = vHoSoInfo.NGAYTAO;
                                    vHoSoInfo_New.NGUOITAO = vHoSoInfo.NGUOITAO;
                                    vHoSoInfo_New.NGAYCAPNHAT = vHoSoInfo.NGAYCAPNHAT;
                                    vHoSoInfo_New.NGUOICAPNHAT = vHoSoInfo.NGUOICAPNHAT;
                                    vHoSoInfo_New.HOSO_FILE = vHoSoInfo.HOSO_FILE;
                                    vHoSoInfo_New.LOAIHOSO_ID = vHoSoInfo.LOAIHOSO_ID;
                                    vHoSoController.ThemMoiHoSo(vHoSoInfo_New, out oHoSoId, out oErrorMessage);
                                    if (oHoSoId > 0)
                                    {
                                        DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                        vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 1;//Hồ sơ người đại diện, ủy quyền
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] == null)
                                {
                                    Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] = new List<HOSO>();
                                }
                                List<HOSO> vDonThuInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"];
                                foreach (var item in vHoSoInfos)
                                {
                                    Int32 vID_TEMP = Int32.Parse(DateTime.Now.ToString("ddHHmmss"));
                                    item.HOSO_ID = vID_TEMP;
                                    vDonThuInfo_Session.Insert(0, item);
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] = vDonThuInfo_Session;
                            }

                            //textTenTaiLieu.Text = "";
                            //textMotaFile.Text = "";
                        }
                        LoadDanhSachHoSoNguoiDaiDienUyQuyen(vDonThuId);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Load danh sách hồ sơ đơn thư
        /// </summary>
        /// <param name="pDonThuId"></param>
        public void LoadDanhSachHoSoNguoiDaiDienUyQuyen(int pDonThuId)
        {
            List<HOSO> vHoSoInfos = new List<HOSO>();
            if (pDonThuId > 0)
            {
                vHoSoInfos = (from vHoSo in vDataContext.HOSOs
                              join vDonThuHoSo in vDataContext.DONTHU_HOSOs on vHoSo.HOSO_ID equals vDonThuHoSo.HOSO_ID
                              where vDonThuHoSo.DONTHU_ID == pDonThuId && vDonThuHoSo.LOAI_HS_DONTHU == 1//Hồ sơ người đại diện, ủy quyền
                              select vHoSo).ToList();
            }
            else
            {
                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] != null)
                {
                    vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"];
                }
            }

            dgDanhSach_File_NguoiDaiDienUyQuyen.DataSource = vHoSoInfos;
            dgDanhSach_File_NguoiDaiDienUyQuyen.DataBind();
            if (vHoSoInfos.Count > 0)
            {
                dgDanhSach_File_NguoiDaiDienUyQuyen.Visible = true;
            }
            else
            {
                dgDanhSach_File_NguoiDaiDienUyQuyen.Visible = false;
            }
        }

        /// <summary>
        /// Xóa hồ sơ đơn thư
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaHoSoDonThu_NguoiDaiDienUyQuyen(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vHoSoId = int.Parse(html.HRef);
                if (vDonThuId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] != null)
                    {
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"];
                        HOSO vHoSoInfo_Delete = vHoSoInfos.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        if (vHoSoInfo_Delete != null)
                        {
                            File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                            vHoSoInfos.Remove(vHoSoInfo_Delete);
                            Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] = vHoSoInfos;
                        }
                    }
                }
                else
                {
                    HOSO vHoSoInfo_Delete = vDataContext.HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                    if (vHoSoInfo_Delete != null)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                        DONTHU_HOSO vHoSoDonThuInfos = vDataContext.DONTHU_HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        vDataContext.DONTHU_HOSOs.DeleteOnSubmit(vHoSoDonThuInfos);
                        vDataContext.HOSOs.DeleteOnSubmit(vHoSoInfo_Delete);
                        vDataContext.SubmitChanges();
                    }
                }
                LoadDanhSachHoSoNguoiDaiDienUyQuyen(vDonThuId);
                ClassCommon.ShowToastr(Page, "Xóa hồ sơ người đại diện, ủy quyền thành công", "Thông báo", "success");
            }
            catch (Exception ex)
            {

            }
        }

        public void XoaSessionFile_HoSoNguoiDaiDienUyQuyen()
        {
            try
            {
                List<HOSO> vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"];
                if (vHoSoInfos.Count > 0)
                {
                    foreach (var vHoSoInfo in vHoSoInfos)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo.HOSO_FILE);
                    }
                }
                Session.Remove(PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Hồ sơ đính kèm kết quả   
        protected void LuuHoSoKetQuaGiaiQuyet(object sender, EventArgs e)
        {
            try
            {
                if (!fileKetQuaGiaiQuyet.HasFile)
                {
                    ClassCommon.ShowToastr(Page, "Vui lòng chọn hồ sơ đính kèm", "Thông báo", "error");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosoketqua').modal('show');", true);
                }
                else
                {
                    if (textTenHoSoKetQuaGiaiQuyet.Text.Trim() == "")
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng nhập Tên hồ sơ", "Thông báo", "error");
                        textTenHoSoKetQuaGiaiQuyet.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "$('#modal-default-hosoketqua').modal('show');", true);
                    }
                    else
                    {
                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;

                        List<HOSO> vHoSoInfos = new List<HOSO>();

                        //for (int i = 0; i < uploadedFiles.Count; i++)
                        //{
                        HttpPostedFile userPostedFile = uploadedFiles[3];//Thứ tự control file từ trên xuống
                                                                         //File cho phép: .jpg, jpng, .jpeg, .doc, .docx, .xls, .xlsx, pdf
                        if (userPostedFile.ContentType == "image/jpg" || userPostedFile.ContentType == "image/png" || userPostedFile.ContentType == "image/jpeg" || userPostedFile.ContentType == "application/msword" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || userPostedFile.ContentType == "application/pdf" || userPostedFile.ContentType == "application/vnd.ms-excel" || userPostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            if (userPostedFile.ContentLength < 1048576 * 50)//100MB
                            {
                                string filename = userPostedFile.FileName;
                                string extension = System.IO.Path.GetExtension(filename);
                                string vFilename = filename.Substring(0, filename.Length - extension.Length) + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + extension;
                                ClassCommon.UploadFile(userPostedFile, filepath, vFilename, "");
                                HOSO vHoSoInfo = new HOSO();
                                vHoSoInfo.HOSO_TEN = ClassCommon.ClearHTML(textTenHoSoKetQuaGiaiQuyet.Text.Trim());
                                vHoSoInfo.HOSO_TOMTAT = ClassCommon.ClearHTML(textNoiDungTomTatKetQua.Text.Trim());
                                vHoSoInfo.NGAYTAO = DateTime.Now;
                                vHoSoInfo.NGUOITAO = _currentUser.UserID;
                                vHoSoInfo.NGAYCAPNHAT = DateTime.Now;
                                vHoSoInfo.NGUOICAPNHAT = _currentUser.UserID;
                                if (!String.IsNullOrEmpty(ddlistLoaiHoSoKetQua.SelectedValue))
                                {
                                    vHoSoInfo.LOAIHOSO_ID = int.Parse(ddlistLoaiHoSoKetQua.SelectedValue);
                                }
                                else
                                {
                                    vHoSoInfo.LOAIHOSO_ID = 2; // Loại khác
                                }
                                vHoSoInfo.HOSO_FILE = vFilename;
                                vHoSoInfos.Add(vHoSoInfo);
                                //vHoSoController.ThemMoiHoSo(vHoSoInfo, out oHoSoId, out oErrorMessage);
                                //if (oHoSoId > 0)
                                //{
                                //    DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                //    vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                //    vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                //    vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư
                                //    vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                //    //Load lại danh sách đơn thư
                                //}
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng chọn file kích thước nhỏ hơn 50MB", "Thông báo", "error");
                            }
                        }
                        else
                        {
                            ClassCommon.ShowToastr(Page, "Vui lòng chọn file đúng định dạng", "Thông báo", "error");
                        }
                        //}//End for
                        if (vHoSoInfos.Count > 0)
                        {
                            if (vDonThuId > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfos)
                                {
                                    long oHoSoId = 0;
                                    string oErrorMessage = "";
                                    HOSO vHoSoInfo_New = new HOSO();
                                    vHoSoInfo_New.HOSO_TEN = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.HOSO_TOMTAT = vHoSoInfo.HOSO_TEN;
                                    vHoSoInfo_New.NGAYTAO = vHoSoInfo.NGAYTAO;
                                    vHoSoInfo_New.NGUOITAO = vHoSoInfo.NGUOITAO;
                                    vHoSoInfo_New.NGAYCAPNHAT = vHoSoInfo.NGAYCAPNHAT;
                                    vHoSoInfo_New.NGUOICAPNHAT = vHoSoInfo.NGUOICAPNHAT;
                                    vHoSoInfo_New.HOSO_FILE = vHoSoInfo.HOSO_FILE;
                                    vHoSoInfo_New.LOAIHOSO_ID = vHoSoInfo.LOAIHOSO_ID;
                                    vHoSoController.ThemMoiHoSo(vHoSoInfo_New, out oHoSoId, out oErrorMessage);
                                    if (oHoSoId > 0)
                                    {
                                        DONTHU_HOSO vDonThuHoSoInfo = new DONTHU_HOSO();
                                        vDonThuHoSoInfo.DONTHU_ID = vDonThuId;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 2;//Hồ sơ kết quả giải quyết
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] == null)
                                {
                                    Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] = new List<HOSO>();
                                }
                                List<HOSO> vDonThuInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"];
                                foreach (var item in vHoSoInfos)
                                {
                                    Int32 vID_TEMP = Int32.Parse(DateTime.Now.ToString("ddHHmmss"));
                                    item.HOSO_ID = vID_TEMP;
                                    vDonThuInfo_Session.Insert(0, item);
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] = vDonThuInfo_Session;
                            }

                            //textTenTaiLieu.Text = "";
                            //textMotaFile.Text = "";
                        }
                        LoadDanhSachHoSoKetQuaGiaiQuyet(vDonThuId);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Load danh sách hồ sơ kết quả giải quyết
        /// </summary>
        /// <param name="pDonThuId"></param>
        public void LoadDanhSachHoSoKetQuaGiaiQuyet(int pDonThuId)
        {
            List<HOSO> vHoSoInfos = new List<HOSO>();
            if (pDonThuId > 0)
            {
                vHoSoInfos = (from vHoSo in vDataContext.HOSOs
                              join vDonThuHoSo in vDataContext.DONTHU_HOSOs on vHoSo.HOSO_ID equals vDonThuHoSo.HOSO_ID
                              where vDonThuHoSo.DONTHU_ID == pDonThuId && vDonThuHoSo.LOAI_HS_DONTHU == 2//Hồ sơ kết quả giải quyết
                              select vHoSo).ToList();
            }
            else
            {
                if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] != null)
                {
                    vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"];
                }
            }

            dgDanhSach_File_KetQuaGiaiQuyet.DataSource = vHoSoInfos;
            dgDanhSach_File_KetQuaGiaiQuyet.DataBind();
            if (vHoSoInfos.Count > 0)
            {
                dgDanhSach_File_KetQuaGiaiQuyet.Visible = true;
            }
            else
            {
                dgDanhSach_File_KetQuaGiaiQuyet.Visible = false;
            }
        }

        /// <summary>
        /// Xóa hồ sơ kết quả giải quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XoaHoSoDonThu_KetQua(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vHoSoId = int.Parse(html.HRef);
                if (vDonThuId == 0)
                {
                    if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] != null)
                    {
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"];
                        HOSO vHoSoInfo_Delete = vHoSoInfos.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        if (vHoSoInfo_Delete != null)
                        {
                            File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                            vHoSoInfos.Remove(vHoSoInfo_Delete);
                            Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"] = vHoSoInfos;
                        }
                    }
                }
                else
                {
                    HOSO vHoSoInfo_Delete = vDataContext.HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                    if (vHoSoInfo_Delete != null)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo_Delete.HOSO_FILE);
                        DONTHU_HOSO vHoSoDonThuInfos = vDataContext.DONTHU_HOSOs.Where(x => x.HOSO_ID == vHoSoId).FirstOrDefault();
                        vDataContext.DONTHU_HOSOs.DeleteOnSubmit(vHoSoDonThuInfos);
                        vDataContext.HOSOs.DeleteOnSubmit(vHoSoInfo_Delete);
                        vDataContext.SubmitChanges();
                    }
                }
                LoadDanhSachHoSoKetQuaGiaiQuyet(vDonThuId);
                ClassCommon.ShowToastr(Page, "Xóa hồ sơ đính kèm kết quả giải quyết thành công", "Thông báo", "success");
            }
            catch (Exception ex)
            {

            }
        }

        public void XoaSessionFile_KetQuaGiaiQuyet()
        {
            try
            {
                List<HOSO> vHoSoInfos = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos"];
                if (vHoSoInfos.Count > 0)
                {
                    foreach (var vHoSoInfo in vHoSoInfos)
                    {
                        File.Delete(Server.MapPath(vPathCommonUploadHoSo) + "/" + vHoSoInfo.HOSO_FILE);
                    }
                }
                Session.Remove(PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoKetQuaGiaiQuyetInfos");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Modal
        ///// <summary>
        /////  Load danh sách co phan trang
        ///// </summary>
        ///// <param name="pCurentPage"></param>
        protected void LoadDanhSach(int v_start, int v_end)
        {
            try
            {
                int vDP_ID = 0;
                if (BS_drlXa.SelectedValue != "-1" && BS_drlXa.SelectedValue != "")
                {
                    vDP_ID = Int32.Parse(BS_drlXa.SelectedValue);
                }
                else if (BS_drlQuanHuyen.SelectedValue != "-1" && BS_drlQuanHuyen.SelectedValue != "")
                {
                    vDP_ID = Int32.Parse(BS_drlQuanHuyen.SelectedValue);
                }
                else if (BS_drlTinhThanhPho.SelectedValue != "-1" && BS_drlTinhThanhPho.SelectedValue != "")
                {
                    vDP_ID = Int32.Parse(BS_drlTinhThanhPho.SelectedValue);
                }
                else
                {
                    vDP_ID = 0;
                }


                string vKeySort = "";
                string vTypeSort = "";
                string vContentSearch = textSearchContent.Text.Trim();
                if (ViewState["keysort"] != null && ViewState["typesort"] != null)
                {
                    vKeySort = ViewState["keysort"].ToString();
                    vTypeSort = ViewState["typesort"].ToString();

                }

                CommonController objCommonController = new CommonController();
                string vSearchOption = textSearchContent_HiddenField.Text;


                DataSet ds = new DataSet();
                // trường hợp có đơn 
                bool vFlag = true;
                if (vFlag == true)
                {
                    if (vSearchOption == "")
                    {
                        vSearchOption = "|DONTHU.DONTHU_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|DONTHU.DONTHU_NOIDUNG,normal,,";
                    }
                    if (vDP_ID != 0)
                    {
                        DIAPHUONG objDIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                        if (objDIAPHUONG != null)
                        {
                            vSearchOption = vSearchOption + "|DIAPHUONG.INDEX_ID,equal,like '" + objDIAPHUONG.INDEX_ID + "%' ,";
                        }
                    }
                    ds = objCommonController.GetPage(PortalId, ModuleId, "DonThu_GetPage_Popup", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                    dgDanhSach.DataKeyField = "DONTHU_ID";
                    // Tiếp dân
                    dgDanhSach.Columns[3].Visible = false;
                    dgDanhSach.Columns[7].Visible = false;
                    dgDanhSach.Columns[9].Visible = false;

                    dgDanhSach.Columns[11].Visible = false;
                    dgDanhSach.Columns[12].Visible = false;
                    dgDanhSach.Columns[15].Visible = false;
                    // Đơn thư
                    dgDanhSach.Columns[4].Visible = true;
                    dgDanhSach.Columns[8].Visible = true;
                    dgDanhSach.Columns[10].Visible = true;
                    dgDanhSach.Columns[13].Visible = true;
                    dgDanhSach.Columns[14].Visible = true;
                    dgDanhSach.Columns[16].Visible = true;
                    //dgDanhSach.Columns[9].Visible = true;
                    //dgDanhSach.Columns[10].Visible = true;
                    //dgDanhSach.Columns[11].Visible = true;
                }
                // trường hợp không có đơn có đơn 
                else
                {
                    if (vSearchOption == "")
                    {
                        vSearchOption = "|TIEPDAN.TIEPDAN_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|TIEPDAN.TIEPDAN_NOIDUNG,normal,,";
                    }
                    if (vDP_ID != 0)
                    {
                        DIAPHUONG objDIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                        if (objDIAPHUONG != null)
                        {
                            vSearchOption = vSearchOption + "|DIAPHUONG.INDEX_ID,equal,like '" + objDIAPHUONG.INDEX_ID + "%' ,";
                        }
                    }
                    ds = objCommonController.GetPage(PortalId, ModuleId, "TiepDan_GetPage_Popup", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                    dgDanhSach.DataKeyField = "TIEPDAN_ID";

                    // Tiếp dân
                    dgDanhSach.Columns[3].Visible = true;
                    dgDanhSach.Columns[7].Visible = true;
                    dgDanhSach.Columns[9].Visible = true;
                    dgDanhSach.Columns[11].Visible = true;
                    dgDanhSach.Columns[12].Visible = true;
                    dgDanhSach.Columns[15].Visible = true;
                    // Đơn thư
                    dgDanhSach.Columns[4].Visible = false;
                    dgDanhSach.Columns[8].Visible = false;
                    dgDanhSach.Columns[10].Visible = false;
                    dgDanhSach.Columns[13].Visible = false;
                    dgDanhSach.Columns[14].Visible = false;
                    dgDanhSach.Columns[16].Visible = false;
                    //dgDanhSach.Columns[9].Visible = false;
                    //dgDanhSach.Columns[10].Visible = false;
                    //dgDanhSach.Columns[11].Visible = false;
                }
                int TotalRow = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalRow = Int32.Parse(ds.Tables[0].Rows[0]["TotalRow"].ToString());
                }
                else
                {
                    TotalRow = 0;
                    v_start = 0;
                    v_end = 0;
                }
                if (!ClassCommon.CheckAfterLoad_DataGrird(txtRecordStartEnd, LinkButtonPrevious, LinkButtonLast, TotalRow, v_start, v_end))
                {
                    v_start = TotalRow;
                    v_end = TotalRow;

                }
                dgDanhSach.DataSource = ds.Tables[0];
                dgDanhSach.DataBind();
                lblTotalRecords.Text = TotalRow + "";
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ trị", "Thông báo lỗi", "error");
            }
        }
        ///// <summary>
        ///// Event Search Click
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDanhSach(1, vPageSize);


            }
            catch (Exception Ex)
            {

            }
        }
        ///// <summary>
        ///// Get PageSize, Current Page
        ///// </summary>
        protected void Get_Cache()
        {

            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] != null)
            {
                vPageSize = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"].ToString());
            }
            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] != null)
            {
                vCurentPage = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"].ToString());
            }
        }
        ///// <summary>
        ///// Sort
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="e"></param>
        protected void dgDanhSach_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortDirection = GetSortDirection(e.SortExpression);
            ViewState["keysort"] = e.SortExpression;
            ViewState["typesort"] = sortDirection;
            LoadDanhSach(1, vPageSize);
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

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                for (int i = 2; i <= 3; i++)
                {
                    e.Item.Cells[i].Attributes.Add("onclick", "javascript:__doPostBack" +
                 "('" + e.Item.UniqueID + "$dgChiTiet' , '')");
                }
            }

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

                            image.ImageUrl = "/DesktopModules/KNTC/Images/sort_both.png";

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
                                        image.ImageUrl = "/DesktopModules/KNTC/Images/sort_asc.png";
                                        image.CssClass = "sort_img";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "/DesktopModules/KNTC/Images/sort_desc.png";
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
        protected void txtRecordStartEnd_TextChanged(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, false, false);
            txtRecordStartEnd.Text = valueStartEnd;

            vPageSize = o_PageSize;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = o_PageSize;
            LoadDanhSach(start, end);


        }
        protected void LinkButtonPrevious_Click(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, true, false);
            txtRecordStartEnd.Text = valueStartEnd;
            vPageSize = o_PageSize;
            LoadDanhSach(start, end);
        }
        protected void LinkButtonLast_Click(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, false, true);
            txtRecordStartEnd.Text = valueStartEnd;
            vPageSize = o_PageSize;
            LoadDanhSach(start, end);
        }
        public string getThongTinDoiTuong(int pTDOITUONG_ID)
        {
            string strDoiTuong = "";
            List<CANHAN> objCANHANs = vDataContext.CANHANs.Where(x => x.DOITUONG_ID == pTDOITUONG_ID).ToList();
            if (objCANHANs.Count > 0)
            {
                for (int i = 0; i < objCANHANs.Count; i++)
                {
                    string vSubName = "";
                    bool IsGetSubName = GetSubCharacterName(objCANHANs[i].CANHAN_HOTEN, out vSubName);
                    strDoiTuong = strDoiTuong + "<div>";
                    if (IsGetSubName)
                    {
                        strDoiTuong = strDoiTuong + "<button type='button' class='btn btn-block btn-primary btn -lg' style='width:40px; padding:5px; height:40px; border-radius:50%;float:left;margin-right:10px;margin-top:2px;margin-bottom:10px;'>";
                        strDoiTuong = strDoiTuong + vSubName + "</button>";
                    }
                    strDoiTuong = strDoiTuong + "<h6><b>" + objCANHANs[i].CANHAN_HOTEN + "</b></h6>";
                    strDoiTuong = strDoiTuong + "<p>" + objCANHANs[i].CANHAN_DIACHI_DAYDU + "</p>";
                    strDoiTuong = strDoiTuong + "</div>";
                }
            }
            return strDoiTuong;

        }
        public bool GetSubCharacterName(string pName, out string oSubCharracter)
        {
            string vSubCharracter = "";

            if (pName.Contains(" "))
            {
                var vSubCharracter_arr = pName.Trim().Split(' ');
                if (vSubCharracter_arr.Count() >= 2)
                {
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[vSubCharracter_arr.Count() - 2].Substring(0, 1);
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[vSubCharracter_arr.Count() - 1].Substring(0, 1);

                    oSubCharracter = vSubCharracter;
                    return true;
                }
                else if (vSubCharracter_arr.Count() == 1)
                {
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[0].Substring(0, 1);
                    oSubCharracter = vSubCharracter;
                    return true;
                }
                else
                {
                    oSubCharracter = vSubCharracter;
                    return false;
                }
            }
            else if (pName.Length >= 1)
            {
                vSubCharracter = pName.Substring(0, 1);
                oSubCharracter = vSubCharracter;
                return true;
            }
            else
            {
                oSubCharracter = vSubCharracter;
                return false;
            }
        }
        private void LoadBSModal()
        {
            LoadDiaPhuong(-1, BS_drlXa, BS_drlQuanHuyen, BS_drlTinhThanhPho);
        }
        protected void BS_drlDIAPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList drlSelected = (DropDownList)sender;
                LoadDiaPhuong(Int32.Parse(drlSelected.SelectedValue), BS_drlXa, BS_drlQuanHuyen, BS_drlTinhThanhPho);
                LoadDanhSach(1, vPageSize);
            }
            catch (Exception Ex)
            { }


        }
        public string GetLoaiDonThu(string p_str_LoaiDonThuID)
        {
            try
            {
                string v_TenLoai = "";
                int p_LoaiDonThuID = Int32.Parse(p_str_LoaiDonThuID);
                LOAIDONTHU objLOAIDONTHU = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == p_LoaiDonThuID).FirstOrDefault();
                if (objLOAIDONTHU != null)
                {
                    if (objLOAIDONTHU.LOAIDONTHU_CHA_ID != 0)
                    {
                        LOAIDONTHU objLOAIDONTHU_CHA1 = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                        if (objLOAIDONTHU_CHA1 != null)
                        {
                            if (objLOAIDONTHU_CHA1.LOAIDONTHU_CHA_ID != 0)
                            {
                                LOAIDONTHU objLOAIDONTHU_CHA2 = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                                if (objLOAIDONTHU_CHA2 != null)
                                {
                                    v_TenLoai = objLOAIDONTHU_CHA2.LOAIDONTHU_TEN + " / " + objLOAIDONTHU_CHA1.LOAIDONTHU_TEN + " / " + objLOAIDONTHU.LOAIDONTHU_TEN;
                                }
                                else
                                {
                                    v_TenLoai = objLOAIDONTHU_CHA1.LOAIDONTHU_TEN + " / " + objLOAIDONTHU.LOAIDONTHU_TEN;
                                }
                            }
                            else
                            {
                                v_TenLoai = objLOAIDONTHU_CHA1.LOAIDONTHU_TEN + " / " + objLOAIDONTHU.LOAIDONTHU_TEN;
                            }
                        }
                        else
                        {
                            v_TenLoai = objLOAIDONTHU.LOAIDONTHU_TEN;
                        }
                    }
                    else
                    {
                        v_TenLoai = objLOAIDONTHU.LOAIDONTHU_TEN;
                    }
                }
                else
                {
                    v_TenLoai = "";
                }
                return v_TenLoai;
            }
            catch (Exception Ex)
            {
                return "";
            }
        }
        protected void btnChonDoiTuong_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                //Lấy danh sách ID tiếp dân trên danh sách
                int vID = -1;

                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vID = ((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }

                var vDoiTuongInfo = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == vID).Select(x => x.DOITUONG).FirstOrDefault();
                hdfieldDoiTuongId.Value = vDoiTuongInfo.DOITUONG_ID.ToString();
                if (vDoiTuongInfo != null)
                {
                    if (btn.CommandArgument == "ChonDoiTuong")
                    {
                        textSoNguoi.Enabled = false;
                    }
                    else
                    {
                        textSoNguoi.Enabled = true;
                    }
                    textSoNguoi.Text = vDoiTuongInfo.DOITUONG_SONGUOI.ToString();
                    textSoNguoiDaiDien.Text = vDoiTuongInfo.DOITUONG_SONGUOIDAIDIEN.ToString();
                    //Load danh sách đối tượng

                    ListViewDoiTuong.DataSource = vDoiTuongInfo.CANHANs;
                    ListViewDoiTuong.DataBind();

                    for (int i = 0; i < vDoiTuongInfo.CANHANs.Count(); i++)
                    {
                        //TitleBreadcrumb = TitleBreadcrumb + "-" + vDonThuInfo.DOITUONG.CANHANs[i].CANHAN_HOTEN;
                        ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                        TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                        if (txtCaNhanID.Text != "")
                        {
                            int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);
                            CANHAN cANHAN = vDoiTuongInfo.CANHANs.Where(x => x.CANHAN_ID == vCANHAN_ID).FirstOrDefault();
                            if (cANHAN != null)
                            {
                                DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                                DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                                DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));
                                DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                                DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));

                                LoadDiaPhuong((int)cANHAN.DP_ID, pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho);
                                LoadDanToc((int)cANHAN.DANTOC_ID, drlDanToc);
                                LoadQuocTich((int)cANHAN.QUOCTICH_ID, drlQuocTich);
                            }
                        }
                    }
                    foreach (var item in ListViewDoiTuong.Items)
                    {

                        TextBox txtHoTen = ((TextBox)item.FindControl("txtHoTen"));
                        //TextBox txtLanTiep = ((TextBox)item.FindControl("txtLanTiep"));
                        TextBox txtCMND = ((TextBox)item.FindControl("txtCMND"));
                        HtmlInputRadioButton rdoNam = ((HtmlInputRadioButton)item.FindControl("rdoNam"));
                        HtmlInputRadioButton rdoNu = ((HtmlInputRadioButton)item.FindControl("rdoNu"));
                        TextBox txtNgayCap = ((TextBox)item.FindControl("txtNgayCap"));
                        TextBox txtNoiCap = ((TextBox)item.FindControl("txtNoiCap"));
                        TextBox txtDiaChi = ((TextBox)item.FindControl("txtDiaChi"));

                        DropDownList pDropDownListXa = ((DropDownList)item.FindControl("drlXa"));
                        DropDownList pDropDownListHuyen = ((DropDownList)item.FindControl("drlQuanHuyen"));
                        DropDownList pDropDownListThanhpho = ((DropDownList)item.FindControl("drlTinhThanhPho"));
                        DropDownList drlQuocTich = ((DropDownList)item.FindControl("drlQuocTich"));
                        DropDownList drlDanToc = ((DropDownList)item.FindControl("drlDanToc"));
                        if (btn.CommandArgument == "ChonDoiTuong")
                        {
                            txtHoTen.Enabled = false;
                            //txtLanTiep.Enabled = pEnableStatus;
                            txtCMND.Enabled = false;
                            rdoNam.Disabled = true;
                            rdoNu.Disabled = true;

                            txtNgayCap.Enabled = false;
                            txtNoiCap.Enabled = false;
                            txtDiaChi.Enabled = false;
                            pDropDownListThanhpho.Enabled = false;
                            pDropDownListHuyen.Enabled = false;
                            pDropDownListXa.Enabled = false;
                            drlQuocTich.Enabled = false;
                            drlDanToc.Enabled = false;
                        }
                        else
                        {
                            txtHoTen.Enabled = true;
                            txtCMND.Enabled = true;
                            rdoNam.Disabled = false;
                            rdoNu.Disabled = false;

                            txtNgayCap.Enabled = true;
                            txtNoiCap.Enabled = true;
                            txtDiaChi.Enabled = true;
                            pDropDownListThanhpho.Enabled = true;
                            pDropDownListHuyen.Enabled = true;
                            pDropDownListXa.Enabled = true;
                            drlQuocTich.Enabled = true;
                            drlDanToc.Enabled = true;
                        }
                    }
                }

                //SetFormInfo(vID, true);

                if (btn.CommandArgument == "ChonDoiTuong")
                {
                    hdfieldLayThongTin.Value = "false";
                    //txtLanTiep.Enabled = true;
                    //txtLanTiep.Text = (Int32.Parse(txtLanTiep.Text) + 1).ToString();
                    //txtNoiDungTiepDan.Enabled = true;
                    //txtKetQua.Enabled = true;
                    //btnChonNguoiDaiDien.Visible = true;
                }
                else
                {
                    hdfieldLayThongTin.Value = "true";
                    SetEnableForm(true);
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "hideModal()", true);

            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }
        #endregion


        protected void btn_XuatPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                vDonThuId = int.Parse(Request.QueryString["id"]);
                List<int> vDONTHU_IDs = new List<int>();
                vDONTHU_IDs.Add(vDonThuId);
                string sourceFile = Server.MapPath(ClassParameter.vPathDataBieuMau) + "\\Chuyendon_mau_01.docx";
                string sourceFilePatch = Server.MapPath(ClassParameter.vPathDataBieuMau);
                string vPathBieuMau_MapPath = Server.MapPath(ClassParameter.vPathDataBieuMau);
                XUATWORDController objOUATWORDController = new XUATWORDController();
                List<byte[]> allData = null;
                List<string> ResponseFileNames = new List<string>();
                allData = objOUATWORDController.XuatPhieuDonThu(vDONTHU_IDs, vPathBieuMau_MapPath, sourceFile, sourceFilePatch, out ResponseFileNames);

                string gui = ClassCommon.GetGuid();
                string src = HttpContext.Current.Request.PhysicalApplicationPath + @"\DesktopModules\KNTC\bieumau\Export\" + ResponseFileNames[0];
                File.WriteAllBytes(src, allData[0]);
                byte[] byteArray = File.ReadAllBytes(src);
                File.Delete(src);
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + ResponseFileNames[0]);
                Response.Charset = "";
                Response.ContentType = "application/octet-stream";
                if (byteArray != null) Response.BinaryWrite(byteArray);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception Ex)
            {

            }
        }

        protected void cboxDaXuLy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxDaXuLy.Checked)
                {
                    divKetQuaXuLy.Visible = true;
                }
                else
                {
                    divKetQuaXuLy.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
