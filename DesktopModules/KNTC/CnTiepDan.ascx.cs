#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật thiết bị
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class CnTiepDan : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vTIEPDAN_ID;
        int vPageSize = 10;//ClassParameter.vPageSize;
        int vCurentPage = 0;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        //public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        TiepDanController tiepDanController = new TiepDanController();
        List<DIAPHUONG> dIAPHUONGs = new List<DIAPHUONG>();
        List<DANTOC> dANTOCs = new List<DANTOC>();
        List<QUOCTICH> qUOCTICHes = new List<QUOCTICH>();
        List<LOAIDONTHU> lOAIDONTHUs = new List<LOAIDONTHU>();
        TiepDanController vTiepDanController = new TiepDanController();
        string vMacAddress = ClassCommon.GetMacAddress();
        public string vPathCommonUploadHoSo = ClassParameter.vPathCommonUploadHoSo;
        HoSoController vHoSoController = new HoSoController();
        //string vMacAddress = ClassCommon.GetMacAddress();
        //ThietBiController vThietBiControllerInfo = new ThietBiController();
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
                ShowMessage();
                Get_Cache();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vTIEPDAN_ID = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    loadHuongXuLy(0);
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

                    SetFormInfo(vTIEPDAN_ID, false);
                    LoadDanhSach(1, vPageSize);
                    LoadBSModal();
                    EnableSoNguoi(vTIEPDAN_ID);
                }
            }
            catch (Exception ex)
            {
                //ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnNhanBan.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
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
                CapNhat(vTIEPDAN_ID);
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
            Boolean vResult = true;
            string vToastrMessage = "Vui lòng ";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";
            if (txtNgayTiepDan.Text == "")
            {
                txtNgayTiepDan.CssClass += " vld";
                txtNgayTiepDan.Focus();
                lbltxtNgayTiepDan.Attributes["class"] += " vld";
                vToastrMessage += "nhập Ngày tiếp dân, ";
                vResult = false;
            }
            else
            {
                txtNgayTiepDan.CssClass = txtNgayTiepDan.CssClass.Replace("vld", "").Trim();
                lbltxtNgayTiepDan.Attributes.Add("class", lbltxtNgayTiepDan.Attributes["class"].ToString().Replace("vld", ""));

            }
            foreach (var item in ListViewDoiTuong.Items)
            {

                TextBox txtHoTen = ((TextBox)item.FindControl("txtHoTen"));
                if (txtHoTen.Text == "")
                {
                    txtHoTen.CssClass += " vld";
                    txtHoTen.Focus();
                    //labelNoiDungTiepDan.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Họ tên, ";
                    vResult = false;
                }
                else
                {
                    txtHoTen.CssClass = txtHoTen.CssClass.Replace("vld", "").Trim();
                    //labelNoiDungTiepDan.Attributes.Add("class", labelNoiDungTiepDan.Attributes["class"].ToString().Replace("vld", ""));
                }
                break;

            }
            if (rdoCoDon.Checked)
            {
                if (txtNgayNhanDon.Text == "")
                {
                    txtNgayNhanDon.CssClass += " vld";
                    txtNgayNhanDon.Focus();
                    lblNgayNhanDon.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Ngày nhận đơn, ";
                    vResult = false;
                }
                else
                {
                    txtNgayNhanDon.CssClass = txtNgayNhanDon.CssClass.Replace("vld", "").Trim();
                    lblNgayNhanDon.Attributes.Add("class", lblNgayNhanDon.Attributes["class"].ToString().Replace("vld", ""));

                }
                if (txtNgayDeDon.Text == "")
                {
                    txtNgayDeDon.CssClass += " vld";
                    txtNgayDeDon.Focus();
                    lblNgayDeDon.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Ngày đề đơn, ";
                    vResult = false;
                }
                else
                {
                    txtNgayDeDon.CssClass = txtNgayDeDon.CssClass.Replace("vld", "").Trim();
                    lblNgayDeDon.Attributes.Add("class", lblNgayDeDon.Attributes["class"].ToString().Replace("vld", ""));

                }

                if (ddlistLoaDonThu.SelectedValue == "" || ddlistLoaDonThu.SelectedValue == "-1")
                {
                    ddlistLoaDonThu.CssClass += " vld";
                    ddlistLoaDonThu.Focus();
                    lbllistLoaDonThu.Attributes["class"] += " vld";
                    vToastrMessage += "chọn Loại đơn thư, ";
                    vResult = false;
                }
                else
                {
                    ddlistLoaDonThu.CssClass = ddlistLoaDonThu.CssClass.Replace("vld", "").Trim();
                    lbllistLoaDonThu.Attributes.Add("class", lbllistLoaDonThu.Attributes["class"].ToString().Replace("vld", ""));

                }
                if (textNoiDungDonThu.Text.Trim() == "")
                {
                    textNoiDungDonThu.CssClass += " vld";
                    textNoiDungDonThu.Focus();
                    lblNoiDungDonThu.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Nội dung đơn thư, ";
                    vResult = false;
                }
                else
                {
                    textNoiDungDonThu.CssClass = textNoiDungDonThu.CssClass.Replace("vld", "").Trim();
                    lblNoiDungDonThu.Attributes.Add("class", lblNoiDungDonThu.Attributes["class"].ToString().Replace("vld", ""));

                }
                if (cboxCoQuanDaGiaiQuyet.Checked)
                {
                    if (ddlistCoQuanDaGiaiQuyet.SelectedValue == "" || ddlistCoQuanDaGiaiQuyet.SelectedValue == "-1")
                    {
                        ddlistCoQuanDaGiaiQuyet.CssClass += " vld";
                        ddlistCoQuanDaGiaiQuyet.Focus();
                        lbllistCoQuanDaGiaiQuyet.Attributes["class"] += " vld";
                        vToastrMessage += "chọn Cơ quan đã giải quyết, ";
                        vResult = false;
                    }
                    else
                    {
                        ddlistCoQuanDaGiaiQuyet.CssClass = ddlistCoQuanDaGiaiQuyet.CssClass.Replace("vld", "").Trim();
                        lbllistCoQuanDaGiaiQuyet.Attributes.Add("class", lbllistCoQuanDaGiaiQuyet.Attributes["class"].ToString().Replace("vld", ""));

                    }
                }

                if (cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked)
                {
                    if (ddlistDoiTuongBoSung.SelectedValue == "1")
                    {
                        if (textHoTen_NguoiBiKhieuNaiToCao.Text.Trim() == "")
                        {
                            textHoTen_NguoiBiKhieuNaiToCao.CssClass += " vld";
                            textHoTen_NguoiBiKhieuNaiToCao.Focus();
                            lbllistCoQuanDaGiaiQuyet.Attributes["class"] += " vld";
                            vToastrMessage += "nhập Họ tên người bị khiếu nại; tố cáo, ";
                            vResult = false;
                        }
                        else
                        {
                            textHoTen_NguoiBiKhieuNaiToCao.CssClass = textHoTen_NguoiBiKhieuNaiToCao.CssClass.Replace("vld", "").Trim();
                            lbllistCoQuanDaGiaiQuyet.Attributes.Add("class", lbllistCoQuanDaGiaiQuyet.Attributes["class"].ToString().Replace("vld", ""));

                        }
                    }
                    else
                    {
                        if (textTenCoQuanToChuc_BiKhieuNaiToCao.Text.Trim() == "")
                        {
                            textTenCoQuanToChuc_BiKhieuNaiToCao.CssClass += " vld";
                            textTenCoQuanToChuc_BiKhieuNaiToCao.Focus();
                            lblTenCoQuanToChuc_BiKhieuNaiToCao.Attributes["class"] += " vld";
                            vToastrMessage += "nhập Họ tên người bị khiếu nại; tố cáo, ";
                            vResult = false;
                        }
                        else
                        {
                            textTenCoQuanToChuc_BiKhieuNaiToCao.CssClass = textTenCoQuanToChuc_BiKhieuNaiToCao.CssClass.Replace("vld", "").Trim();
                            lblTenCoQuanToChuc_BiKhieuNaiToCao.Attributes.Add("class", lblTenCoQuanToChuc_BiKhieuNaiToCao.Attributes["class"].ToString().Replace("vld", ""));

                        }
                    }


                }
                if (cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked)
                {
                    if (textHoTenNguoiDaiDien.Text.Trim() == "")
                    {
                        textHoTenNguoiDaiDien.CssClass += " vld";
                        textHoTenNguoiDaiDien.Focus();
                        lblHoTenNguoiDaiDien.Attributes["class"] += " vld";
                        vToastrMessage += "nhập Họ tên người đại diện, ủy quyền, ";
                        vResult = false;
                    }
                    else
                    {
                        textHoTenNguoiDaiDien.CssClass = textHoTenNguoiDaiDien.CssClass.Replace("vld", "").Trim();
                        lblHoTenNguoiDaiDien.Attributes.Add("class", lblHoTenNguoiDaiDien.Attributes["class"].ToString().Replace("vld", ""));

                    }
                }
                if (ddlistHuongXuLy.SelectedValue != "")
                {
                    if (textYKienXuLy.Text.Trim() == "")
                    {
                        textYKienXuLy.CssClass += " vld";
                        textYKienXuLy.Focus();
                        lblYKienXuLy.Attributes["class"] += " vld";
                        vToastrMessage += "nhập Ý kiến xử lý, ";
                        vResult = false;
                    }
                    else
                    {
                        textYKienXuLy.CssClass = textYKienXuLy.CssClass.Replace("vld", "").Trim();
                        lblYKienXuLy.Attributes.Add("class", lblYKienXuLy.Attributes["class"].ToString().Replace("vld", ""));

                    }
                }


            }
            else
            {
                if (drlLoaiTiepDan.SelectedValue == "" || drlLoaiTiepDan.SelectedValue == "-1")
                {
                    drlLoaiTiepDan.CssClass += " vld";
                    drlLoaiTiepDan.Focus();
                    lbldrlLoaiTiepDan.Attributes["class"] += " vld";
                    vToastrMessage += "chọn Loại tiếp dân, ";
                    vResult = false;
                }
                else
                {
                    drlLoaiTiepDan.CssClass = drlLoaiTiepDan.CssClass.Replace("vld", "").Trim();
                    lbldrlLoaiTiepDan.Attributes.Add("class", lbldrlLoaiTiepDan.Attributes["class"].ToString().Replace("vld", ""));

                }
                if (txtNoiDungTiepDan.Text == "")
                {
                    txtNoiDungTiepDan.CssClass += " vld";
                    txtNoiDungTiepDan.Focus();
                    labelNoiDungTiepDan.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Nội dung tiếp dân, ";
                    vResult = false;
                }
                else
                {
                    txtNoiDungTiepDan.CssClass = txtNoiDungTiepDan.CssClass.Replace("vld", "").Trim();
                    labelNoiDungTiepDan.Attributes.Add("class", labelNoiDungTiepDan.Attributes["class"].ToString().Replace("vld", ""));
                }
                if (txtKetQua.Text == "")
                {
                    txtKetQua.CssClass += " vld";
                    txtKetQua.Focus();
                    labelKetQua.Attributes["class"] += " vld";
                    vToastrMessage += "nhập Kết quả tiếp dân, ";
                    vResult = false;
                }
                else
                {
                    txtKetQua.CssClass = txtKetQua.CssClass.Replace("vld", "").Trim();
                    labelKetQua.Attributes.Add("class", labelKetQua.Attributes["class"].ToString().Replace("vld", ""));
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới tiếp dân", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion
        #region Methods
        /// <summary>
        ///  Set thông tin cho form
        /// </summary>
        /// <param name="pTIEPDAN_ID"></param>
        public void SetFormInfo(int pTIEPDAN_ID,bool IsChonDoiTuong)
        {
            try
            {
                string TitleBreadcrumb = "";
                if (pTIEPDAN_ID == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnNhanBan.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                    List<CANHAN> cANHANs = new List<CANHAN>();
                    CANHAN cANHAN = new CANHAN();
                    cANHANs.Add(cANHAN);
                    cANHAN.CANHAN_ID = 0;
                    cANHAN.CANHAN_GIOITINH = false;
                    ListViewDoiTuong.DataSource = cANHANs;
                    ListViewDoiTuong.DataBind();
                    TitleBreadcrumb = TitleBreadcrumb + "Thêm mới tiếp dân";
                    lblBreadcrumbTitle.Text = TitleBreadcrumb;
                    contentDonThu.Visible = false;
                    contentTiepDan.Visible = true;
                    rdoKhongDon.Checked = true;
                    for (int i = 0; i < ListViewDoiTuong.Items.Count(); i++)
                    {
                        ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                        DropDownList pDropDownListXa = ((DropDownList)ListViewDoiTuong.Items[i].FindControl("drlXa"));
                        DropDownList pDropDownListHuyen = ((DropDownList)listViewDataItem.FindControl("drlQuanHuyen"));
                        DropDownList pDropDownListThanhpho = ((DropDownList)listViewDataItem.FindControl("drlTinhThanhPho"));
                        DropDownList drlQuocTich = ((DropDownList)listViewDataItem.FindControl("drlQuocTich"));
                        DropDownList drlDanToc = ((DropDownList)listViewDataItem.FindControl("drlDanToc"));
                        LoadDiaPhuong(ClassParameter.vDiaPhuongDefault, pDropDownListXa, pDropDownListHuyen, pDropDownListThanhpho); // -1: không load mặt định
                        LoadDanToc(ClassParameter.vDanTocDefault, drlDanToc);
                        LoadQuocTich(ClassParameter.vQuocTichDefault, drlQuocTich);

                    }
                    LoadLoaiDonThu(0, false, drlLoaiTiepDan, drlLoaiKieuNai, drlLoaiChiTiet, "tiếp dân");
                    int MaxSTT = 0;
                    if (vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT) == null)
                    {
                        MaxSTT = 0;
                    }
                    else
                    {
                        MaxSTT = (int)vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT);
                    }

                    lblSTT.Text = (MaxSTT + 1) + "";
                    lblNgayTiepDan.Text = "Ngày " + DateTime.Now.ToString("dd/MM/yyyy").ToString();
                    txtNgayTiepDan.Text = DateTime.Now.ToString("dd/MM/yyyy").ToString();
                    txtNoiDungTiepDan.Text = "";
                    txtKetQua.Text = "";

                    Loadbtn_ThemNguoiDaiDien();
                }
                else
                {
                    TIEPDAN tIEPDAN;
                    if (IsChonDoiTuong)
                    {
                        if (rdoCoDon.Checked)
                        {
                            tIEPDAN = vDataContext.TIEPDANs.Where(x => x.DONTHU_ID == pTIEPDAN_ID).FirstOrDefault();
                            DONTHU objDONTHU_Selected = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == pTIEPDAN_ID).FirstOrDefault();
                            if (tIEPDAN == null)
                            {
                                tIEPDAN = new TIEPDAN();
                                tIEPDAN.TIEPDAN_LANTIEP = 1;
                                tIEPDAN.DOITUONG = objDONTHU_Selected.DOITUONG;
                                tIEPDAN.DONTHU_ID = objDONTHU_Selected.DONTHU_ID;
                                tIEPDAN.TIEPDAN_ID = -1;
                            }
                        }
                        else
                        {
                            tIEPDAN = tiepDanController.GetAll_Info_TIAPDAN_ById(pTIEPDAN_ID);
                        }
                    }
                    else
                    {
                        tIEPDAN = tiepDanController.GetAll_Info_TIAPDAN_ById(pTIEPDAN_ID);
                    }
                    if (tIEPDAN != null)
                    {
                        txtSoNguoi.Text = tIEPDAN.DOITUONG.DOITUONG_SONGUOI.ToString();
                        if (!IsChonDoiTuong)
                        {
                            lblSTT.Text = (tIEPDAN.TIEPDAN_STT) + "";
                            lblNgayTiepDan.Text = "Ngày " + ((DateTime)tIEPDAN.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                            txtNgayTiepDan.Text = ((DateTime)tIEPDAN.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        }
                        hdfTiepDanID.Value = tIEPDAN.TIEPDAN_ID.ToString();
                        hdfDoiTuongID.Value = tIEPDAN.DOITUONG_ID.ToString();
                        drpDOITUONG.SelectedValue = tIEPDAN.DOITUONG.DOITUONG_LOAI.ToString();
                        txtTenCoQuanToChuc.Text = tIEPDAN.DOITUONG.DOITUONG_TEN;
                        txtDiaChiDoiTuong.Text = tIEPDAN.DOITUONG.DOITUONG_DIACHI;
                        drpDOITUONG_SelectedIndexChanged(drpDOITUONG, null);

                        txtLanTiep.Text = tIEPDAN.TIEPDAN_LANTIEP.ToString();
                        txtNoiDungTiepDan.Text = tIEPDAN.TIEPDAN_NOIDUNG;
                        txtKetQua.Text = tIEPDAN.TIEPDAN_KETQUA;
                        ListViewDoiTuong.DataSource = tIEPDAN.DOITUONG.CANHANs;
                        ListViewDoiTuong.DataBind();
                        for (int i = 0; i < tIEPDAN.DOITUONG.CANHANs.Count(); i++)
                        {

                            TitleBreadcrumb = TitleBreadcrumb + "-" + tIEPDAN.DOITUONG.CANHANs[i].CANHAN_HOTEN;
                            ListViewDataItem listViewDataItem = ListViewDoiTuong.Items[i];
                            TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                            if (txtCaNhanID.Text != "")
                            {
                                int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);
                                CANHAN cANHAN = tIEPDAN.DOITUONG.CANHANs.Where(x => x.CANHAN_ID == vCANHAN_ID).FirstOrDefault();
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
                        txtSoNguoiDaiDien.Text = tIEPDAN.DOITUONG.CANHANs.Count().ToString();
                        if (!IsChonDoiTuong)
                        {
                            TitleBreadcrumb = TitleBreadcrumb.Remove(0, 1);
                            lblBreadcrumbTitle.Text = TitleBreadcrumb + " (" + tIEPDAN.TIEPDAN_STT + " - " + DateTime.Parse(tIEPDAN.TIEPDAN_THOGIAN.ToString()).ToString("dd/MM/yyyy") + ")";
                        }
                        SetEnableForm(false);
                        Loadbtn_ThemNguoiDaiDien();
                        if (tIEPDAN.DONTHU_ID != null)
                        {


                            DONTHU objDONTHU = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == tIEPDAN.DONTHU_ID).FirstOrDefault();
                            if (objDONTHU != null)
                            {
                                if (objDONTHU.LOAIDONTHU_ID != null)
                                {
                                    LoadLoaiDonThu((int)(objDONTHU.LOAIDONTHU_ID), false, ddlistLoaDonThu, ddlistLoaiKhieuNai, ddlistLoaiKhieuNaiChiTiet, "đơn thư");
                                }
                                else
                                {
                                    LoadLoaiDonThu(0, false, ddlistLoaDonThu, ddlistLoaiKhieuNai, ddlistLoaiKhieuNaiChiTiet, "đơn thư");
                                }
                                if (!IsChonDoiTuong)
                                {
                                    LoadDanhSachHoSoDonThu(objDONTHU.DONTHU_ID);
                                    LoadDanhSachHoSoHuongXuLy(objDONTHU.DONTHU_ID);
                                    LoadDanhSachHoSoNguoiDaiDienUyQuyen(objDONTHU.DONTHU_ID);
                                }

                            }
                            rdoCoDon.Checked = true;
                            rdoDon_CheckedChanged(rdoCoDon, new EventArgs());
                            if (objDONTHU.DONTHU_KHONGDUDDIEUKIEN != null)
                            {
                                cboxDonThuKhongDuDieuKien.Checked = bool.Parse(objDONTHU.DONTHU_KHONGDUDDIEUKIEN.ToString());
                                cboxDonThuKhongDuDieuKien_CheckedChanged(cboxDonThuKhongDuDieuKien, new EventArgs());
                            }

                            if (objDONTHU.NGAYTAO != null)
                            {
                                txtNgayNhanDon.Text = (DateTime.Parse(objDONTHU.NGAYTAO.ToString())).ToString("dd/MM/yyyy");
                            }
                            if (objDONTHU.NGUONDON_NGAYDEDON != null)
                            {
                                txtNgayDeDon.Text = (DateTime.Parse(objDONTHU.NGUONDON_NGAYDEDON.ToString())).ToString("dd/MM/yyyy");
                            }
                            textNoiDungDonThu.Text = objDONTHU.DONTHU_NOIDUNG.ToString();
                            if (bool.Parse(objDONTHU.DAGIAIQUYET_DONTHU.ToString()))
                            {
                                cboxCoQuanDaGiaiQuyet.Checked = bool.Parse(objDONTHU.DAGIAIQUYET_DONTHU.ToString());
                                cboxCoQuanDaGiaiQuyet.Checked = true;
                                cbCoQuanDaGiaiQuyet_CheckedChanged(cboxCoQuanDaGiaiQuyet, new EventArgs());
                                ddlistCoQuanDaGiaiQuyet.SelectedValue = objDONTHU.DAGIAIQUYET_DONVI_ID.ToString();
                                textLanGiaiQuyet.Text = objDONTHU.DAGIAIQUYET_LAN + "";
                                if (objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD != null)
                                {
                                    textNgayBanHanhQuyetDinh.Text = (DateTime.Parse(objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD.ToString())).ToString("dd/MM/yyyy");
                                }
                                if (objDONTHU.DAGIAIQUYET_HTGQ_ID != null)
                                {
                                    ddlistHinhThucGiaiQuyet.SelectedValue = objDONTHU.DAGIAIQUYET_HTGQ_ID.ToString();
                                }
                                textKetQuaCuaCoQuanGiaiQuyet.Text = objDONTHU.DAGIAIQUYET_KETQUA_CQ + "";
                                ///
                                ///Tiệp đính kèm 
                                ///
                            }
                            if (objDONTHU.DOITUONG_BI_KNTC_ID != null && objDONTHU.DOITUONG_BI_KNTC_ID != -1)
                            {
                                DOITUONG DOITUONG_BIKNTC = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == objDONTHU.DOITUONG_BI_KNTC_ID).FirstOrDefault();
                                cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked = true;
                                cbBoSungThongTinNguoiBiKhieuNaiToCao_CheckedChanged(cboxBoSungThongTinNguoiBiKhieuNaiToCao, new EventArgs());

                                if (DOITUONG_BIKNTC != null)
                                {
                                    ddlistDoiTuongBoSung.SelectedValue = DOITUONG_BIKNTC.DOITUONG_LOAI.ToString();
                                    ChonLoaiDoiTuongBiKhieuNaoToCao(ddlistDoiTuongBoSung, new EventArgs());

                                    if (DOITUONG_BIKNTC.CANHANs.Count > 0)
                                    {
                                        if (DOITUONG_BIKNTC.DOITUONG_LOAI == 1)
                                        {
                                            textHoTen_NguoiBiKhieuNaiToCao.Text = DOITUONG_BIKNTC.CANHANs[0].CANHAN_HOTEN.ToString();
                                            rdoDoiTuongNam.Checked = !Boolean.Parse(DOITUONG_BIKNTC.CANHANs[0].CANHAN_GIOITINH.ToString());
                                            rdoDoiTuongNu.Checked = Boolean.Parse(DOITUONG_BIKNTC.CANHANs[0].CANHAN_GIOITINH.ToString());
                                            textNoiCongTac.Text = DOITUONG_BIKNTC.CANHANs[0].NOICONGTAC + "";
                                            textChucVu.Text = DOITUONG_BIKNTC.CANHANs[0].CHUCVU + "";
                                        }
                                        else
                                        {
                                            textTenCoQuanToChuc_BiKhieuNaiToCao.Text = DOITUONG_BIKNTC.DOITUONG_TEN.ToString();
                                        }

                                        textDiaChi.Text = DOITUONG_BIKNTC.CANHANs[0].CANHAN_DIACHI + "";
                                        LoadDiaPhuong((int)DOITUONG_BIKNTC.CANHANs[0].DP_ID, ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);
                                        LoadDanToc((int)DOITUONG_BIKNTC.CANHANs[0].DANTOC_ID, ddlistDanToc);
                                        LoadQuocTich((int)DOITUONG_BIKNTC.CANHANs[0].QUOCTICH_ID, ddlistQuocTich);


                                    }
                                }
                            }
                            if (objDONTHU.NGUOIUYQUYEN_CANHAN_ID != null && objDONTHU.NGUOIUYQUYEN_CANHAN_ID != -1)
                            {
                                cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked = true;
                                cbBoSungThongTinNguoiDaiDienUyQuyem_CheckedChanged(cboxBoSungThongTinNguoiDaiDienUyQuyen, new EventArgs());
                                CANHAN CANHANUYQUYEN = vDataContext.CANHANs.Where(x => x.CANHAN_ID == objDONTHU.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();
                                if (CANHANUYQUYEN != null)
                                {
                                    textHoTenNguoiDaiDien.Text = CANHANUYQUYEN.CANHAN_HOTEN;
                                    rdoDaiDienUyQuyenNam.Checked = !Boolean.Parse(CANHANUYQUYEN.CANHAN_GIOITINH.ToString());
                                    rdoDaiDienUyQuyenNu.Checked = Boolean.Parse(CANHANUYQUYEN.CANHAN_GIOITINH.ToString());
                                    textDiaChiNguoiDaiDienUyQuyen.Text = CANHANUYQUYEN.CANHAN_DIACHI;
                                    LoadDiaPhuong((int)CANHANUYQUYEN.DP_ID, ddlistXaPhuongNguoiDaiDienUyQuyen, ddlistQuanHuyenNguoiDaiDienUyQuyen, ddlistTinhThanhNguoiDaiDienUyQuyen);
                                    LoadQuocTich((int)CANHANUYQUYEN.QUOCTICH_ID, ddlistQuocTichNguoiDaiDienUyQuyen);
                                }
                            }
                            if (objDONTHU.HUONGXULY_ID != null && objDONTHU.HUONGXULY_ID != -1)
                            {
                                ddlistHuongXuLy.SelectedValue = objDONTHU.HUONGXULY_ID + "";
                                ChonHuongXuLy(ddlistHuongXuLy, new EventArgs());
                            }
                            if (objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID != null && objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID != -1)
                            {
                                ddlistThamQuyenGiaiQuyet.SelectedValue = objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID + "";
                            }
                            if (objDONTHU.HUONGXULY_DONVI_ID != null && objDONTHU.HUONGXULY_DONVI_ID != -1)
                            {
                                ddlistCoQuanTiepNhan.SelectedValue = objDONTHU.HUONGXULY_DONVI_ID + "";
                                ddlistCoQuanTiepNhan_SelectedIndexChanged(ddlistCoQuanTiepNhan, new EventArgs());
                            }
                            if (objDONTHU.HUONGXULY_CANBO != null && objDONTHU.HUONGXULY_CANBO != -1)
                            {

                                CANBO CANBO_TIEPNHAN = vDataContext.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_CANBO).FirstOrDefault();
                                if (CANBO_TIEPNHAN != null)
                                {

                                    ddlistNguoiTiepNhan.SelectedValue = CANBO_TIEPNHAN.CANBO_ID + "";
                                }
                            }
                            if (objDONTHU.HUONGXULY_NGAYCHUYEN != null)
                            {
                                textNgayChuyen_HuongXuLy.Text = (DateTime.Parse(objDONTHU.HUONGXULY_NGAYCHUYEN.ToString())).ToString("dd/MM/yyyy");
                            }
                            textSoHieuVanBanDi.Text = objDONTHU.HUONGXULY_SOHIEUVB_DI;

                            if (objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID != null)
                            {
                                CANBO CANBO_DUYET = vDataContext.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                                if (CANBO_DUYET != null)
                                {
                                    ddlistNguoiDuyet.SelectedValue = objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID + "";
                                    textChucVu_HuongXuLy.Text = CANBO_DUYET.CHUCVU.TENCHUCVU;
                                }
                            }
                            if (objDONTHU.HUONGXULY_THOIHANGIAIQUET != null)
                            {
                                textThoiHanGiaiQuyet.Text = (DateTime.Parse(objDONTHU.HUONGXULY_THOIHANGIAIQUET.ToString())).ToString("dd/MM/yyyy");
                            }
                            textYKienXuLy.Text = objDONTHU.HUONGXULY_YKIEN_XULY;
                            textGhiChu.Text = objDONTHU.DONTHU_GHICHU;
                        }
                        else
                        {
                            rdoKhongDon.Checked = true;
                            if (tIEPDAN.TIEPDAN_LOAI != null)
                            {
                                LoadLoaiDonThu((int)(tIEPDAN.TIEPDAN_LOAI), false, drlLoaiTiepDan, drlLoaiKieuNai, drlLoaiChiTiet, "tiếp dân");
                            }
                            else
                            {
                                LoadLoaiDonThu(0, false, drlLoaiTiepDan, drlLoaiKieuNai, drlLoaiChiTiet, "tiếp dân");
                            }
                            contentDonThu.Visible = false;
                            contentTiepDan.Visible = true;
                        }
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
            drpDOITUONG.Enabled = pEnableStatus;
            txtNgayTiepDan.Enabled = pEnableStatus;
            btnChonNguoiDaiDien.Visible = pEnableStatus;
            txtSoNguoiDaiDien.Enabled = pEnableStatus;
            txtLanTiep.Enabled = pEnableStatus;
            txtTenCoQuanToChuc.Enabled = pEnableStatus;
            txtDiaChiDoiTuong.Enabled = pEnableStatus;

            //txtSoNguoi.Enabled = pEnableStatus;
            if (drpDOITUONG.SelectedValue != "1")
            {
                txtSoNguoi.Enabled = pEnableStatus;
            }
            else
            {
                txtSoNguoi.Text = "1";
                txtSoNguoi.Enabled = false;
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

                txtHoTen.Enabled = pEnableStatus;
                //txtLanTiep.Enabled = pEnableStatus;
                txtCMND.Enabled = pEnableStatus;
                rdoNam.Disabled = !pEnableStatus;
                rdoNu.Disabled = !pEnableStatus;

                txtNgayCap.Enabled = pEnableStatus;
                txtNoiCap.Enabled = pEnableStatus;
                txtDiaChi.Enabled = pEnableStatus;
                pDropDownListThanhpho.Enabled = pEnableStatus;
                pDropDownListHuyen.Enabled = pEnableStatus;
                pDropDownListXa.Enabled = pEnableStatus;
                drlQuocTich.Enabled = pEnableStatus;
                drlDanToc.Enabled = pEnableStatus;
            }

            rdoCoDon.Enabled = pEnableStatus;
            rdoKhongDon.Enabled = pEnableStatus;
            drlLoaiTiepDan.Enabled = pEnableStatus;
            drlLoaiKieuNai.Enabled = pEnableStatus;
            drlLoaiChiTiet.Enabled = pEnableStatus;
            txtNoiDungTiepDan.Enabled = pEnableStatus;
            txtKetQua.Enabled = pEnableStatus;
            // don thu
            cboxDonThuKhongDuDieuKien.Enabled = pEnableStatus;
            txtNgayNhanDon.Enabled = pEnableStatus;
            txtNgayDeDon.Enabled = pEnableStatus;
            ddlistLoaDonThu.Enabled = pEnableStatus;
            ddlistLoaiKhieuNaiChiTiet.Enabled = pEnableStatus;
            ddlistLoaiKhieuNai.Enabled = pEnableStatus;
            textNoiDungDonThu.Enabled = pEnableStatus;
            cboxCoQuanDaGiaiQuyet.Enabled = pEnableStatus;
            ddlistCoQuanDaGiaiQuyet.Enabled = pEnableStatus;
            textLanGiaiQuyet.Enabled = pEnableStatus;
            textNgayBanHanhQuyetDinh.Enabled = pEnableStatus;
            ddlistHinhThucGiaiQuyet.Enabled = pEnableStatus;
            textKetQuaCuaCoQuanGiaiQuyet.Enabled = pEnableStatus;
            fileCoQuanDaGiaiQuyet.Visible = pEnableStatus;
            cboxBoSungThongTinNguoiBiKhieuNaiToCao.Enabled = pEnableStatus;
            ddlistDoiTuongBoSung.Enabled = pEnableStatus;
            textTenCoQuanToChuc_BiKhieuNaiToCao.Enabled = pEnableStatus;
            textKetQuaCuaCoQuanGiaiQuyet.Enabled = pEnableStatus;
            textHoTen_NguoiBiKhieuNaiToCao.Enabled = pEnableStatus;
            rdoDoiTuongNam.Disabled = !pEnableStatus;
            rdoDoiTuongNu.Disabled = !pEnableStatus;
            textNoiCongTac.Enabled = pEnableStatus;
            textChucVu.Enabled = pEnableStatus;
            textDiaChi.Enabled = pEnableStatus;
            ddlistTinhThanh.Enabled = pEnableStatus;
            ddlistQuanHuyen.Enabled = pEnableStatus;
            ddlistXaPhuong.Enabled = pEnableStatus;
            ddlistQuocTich.Enabled = pEnableStatus;
            ddlistDanToc.Enabled = pEnableStatus;
            cboxBoSungThongTinNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            textHoTenNguoiDaiDien.Enabled = pEnableStatus;
            rdoDaiDienUyQuyenNam.Disabled = !pEnableStatus;
            rdoDaiDienUyQuyenNu.Disabled = !pEnableStatus;
            textDiaChiNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            ddlistTinhThanhNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            ddlistQuanHuyenNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            ddlistXaPhuongNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            ddlistQuocTichNguoiDaiDienUyQuyen.Enabled = pEnableStatus;
            ddlistHuongXuLy.Enabled = pEnableStatus;
            ddlistThamQuyenGiaiQuyet.Enabled = pEnableStatus;

            //Hồ sơ đơn thư
            dgDanhSach_File_HoSoDonThu.Columns[4].Visible = pEnableStatus;
            buttonThemTaiLieu.Visible = pEnableStatus;

            //Hồ sơ đơn thư
            dgDanhSach_File_HuongXuLy.Columns[4].Visible = pEnableStatus;
            buttonThemHoSoHuongXuLy.Visible = pEnableStatus;

            //Hồ sơ đơn thư
            dgDanhSach_File_NguoiDaiDienUyQuyen.Columns[4].Visible = pEnableStatus;
            buttonThemTapTinNguoiDaiDienUyQuyen.Visible = pEnableStatus;



            ddlistCoQuanTiepNhan.Enabled = pEnableStatus;
            ddlistNguoiTiepNhan.Enabled = pEnableStatus;
            textNgayChuyen_HuongXuLy.Enabled = pEnableStatus;
            textSoHieuVanBanDi.Enabled = pEnableStatus;
            ddlistNguoiDuyet.Enabled = pEnableStatus;
            textChucVu_HuongXuLy.Enabled = false;
            textThoiHanGiaiQuyet.Enabled = pEnableStatus;
            textYKienXuLy.Enabled = pEnableStatus;
            textGhiChu.Enabled = pEnableStatus;
        }
        /// <summary>
        /// Cập nhật thông tin thiết bị
        /// </summary>
        /// <param name="pTIEPDAN_ID"></param>
        public void CapNhat(int pTIEPDAN_ID)
        {
            try
            {
                string oErrorMessage = "";

                if (pTIEPDAN_ID == 0)//Thêm mới tiếp dân
                {
                    TIEPDAN TIEPDANInfo = new TIEPDAN();
                    int vLOAIDOITUONG = Int32.Parse(drpDOITUONG.SelectedValue);
                    if (GetLoaiDonThu() != null)
                    {
                        TIEPDANInfo.TIEPDAN_LOAI = Int32.Parse(GetLoaiDonThu());
                    }
                    TIEPDANInfo.TIEPDAN_NOIDUNG = ClassCommon.ClearHTML(txtNoiDungTiepDan.Text.Trim());
                    TIEPDANInfo.TIEPDAN_KETQUA = ClassCommon.ClearHTML(txtKetQua.Text.Trim());
                    TIEPDANInfo.TIEPDAN_THOGIAN = DateTime.Now;
                    TIEPDANInfo.TIEPDAN_KETQUA = ClassCommon.ClearHTML(txtKetQua.Text.Trim());
                    TIEPDANInfo.TIEPDAN_THOGIAN = DateTime.Parse(txtNgayTiepDan.Text);
                    TIEPDANInfo.NGAYTAO = DateTime.Now;
                    TIEPDANInfo.NGUOITAO = _currentUser.UserID;
                    TIEPDANInfo.TIEPDAN_BTD = 0;
                    int MaxSTT = 0;
                    if (vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT) != null)
                    {
                        TIEPDANInfo.TIEPDAN_STT = (vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT) + 1);
                    }
                    else
                    {
                        TIEPDANInfo.TIEPDAN_STT = 1;
                    }


                    TIEPDANInfo.TIEPDAN_LANTIEP = Int32.Parse(txtLanTiep.Text.Trim());
                    // Đối tượng
                    if (hdfIsCoppy.Value != "false")
                    {
                        DOITUONG DOITUONGInfo = new DOITUONG();
                        if (vLOAIDOITUONG == 1)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Cá Nhân";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1;
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 1;
                        }
                        else if (vLOAIDOITUONG == 2)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Nhóm đông người";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = Int32.Parse(txtSoNguoiDaiDien.Text);
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 2;
                        }
                        else
                        {
                            DOITUONGInfo.DOITUONG_TEN = ClassCommon.ClearHTML(txtTenCoQuanToChuc.Text.Trim());
                            DOITUONGInfo.DOITUONG_DIACHI = ClassCommon.ClearHTML(txtDiaChiDoiTuong.Text.Trim());
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1;
                            DOITUONGInfo.DOITUONG_SONGUOI = 1;
                            DOITUONGInfo.DOITUONG_LOAI = 3;
                        }
                        DOITUONGInfo.DOITUONG_NACDANH = false;
                        DOITUONGInfo.NGAYTAO = DateTime.Now;
                        DOITUONGInfo.NGUOITAO = _currentUser.UserID;
                        vDataContext.DOITUONGs.InsertOnSubmit(DOITUONGInfo);
                        vDataContext.SubmitChanges();

                        List<CANHAN> cANHAN_Lists = new List<CANHAN>();
                        foreach (var item in ListViewDoiTuong.Items)
                        {
                            CANHAN cANHAN = new CANHAN();
                            cANHAN = GetThongTinCaNhan(item);
                            cANHAN_Lists.Add(cANHAN);
                        }
                        for (int i = 0; i < cANHAN_Lists.Count; i++)
                        {
                            CANHAN cANHAN_Insert = cANHAN_Lists[i];
                            //SET THÔNG TIN CÁ NHÂN                      
                            string vDiaChi = "";
                            if (cANHAN_Insert.DP_ID > 0)
                            {
                                DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == cANHAN_Insert.DP_ID).FirstOrDefault();
                                if (DIAPHUONG.DP_ID_CHA > 0)
                                {
                                    DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                    if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                        vDiaChi = DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                    }
                                    else
                                    {
                                        vDiaChi = DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                    }
                                }
                                else
                                {
                                    vDiaChi = DIAPHUONG.DP_TEN;
                                }
                            }
                            //cANHAN_Insert.CANHAN_DIACHI_DAYDU = (cANHAN_Insert.CANHAN_DIACHI != "" ? (cANHAN_Insert.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            if (vDiaChi != "")
                            {
                                cANHAN_Insert.CANHAN_DIACHI_DAYDU = (cANHAN_Insert.CANHAN_DIACHI != "" ? (cANHAN_Insert.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            }
                            else
                            {
                                cANHAN_Insert.CANHAN_DIACHI_DAYDU = cANHAN_Insert.CANHAN_DIACHI;
                            }
                            cANHAN_Insert.DOITUONG_ID = DOITUONGInfo.DOITUONG_ID;
                            cANHAN_Insert.NGAYTAO = DateTime.Now;
                            cANHAN_Insert.NGUOITAO = _currentUser.UserID;
                            vDataContext.CANHANs.InsertOnSubmit(cANHAN_Insert);
                            vDataContext.SubmitChanges();
                        }
                        TIEPDANInfo.DOITUONG_ID = DOITUONGInfo.DOITUONG_ID;
                    }
                    else
                    {
                        TIEPDANInfo.DOITUONG_ID = Int64.Parse(hdfDoiTuongID.Value);
                    }
                    if (rdoCoDon.Checked)
                    {

                        #region Xử lý đơn thư
                        DONTHU objDONTHU = new DONTHU();
                        objDONTHU.DONTHU_STT = (vDataContext.DONTHUs.Max(x => x.DONTHU_STT) + 1);
                        objDONTHU.DOITUONG_ID = TIEPDANInfo.DOITUONG_ID;
                        objDONTHU.NGUONDON_LOAI = false;// Trực tiếp;
                        objDONTHU.NGUONDON_LOAI_CHITIET = 0;
                        objDONTHU.DONTHU_KHONGDUDDIEUKIEN = cboxDonThuKhongDuDieuKien.Checked;
                        if (ddlistLoaiKhieuNaiChiTiet.SelectedValue != "-1" && ddlistLoaiKhieuNaiChiTiet.SelectedValue != "")
                        {
                            objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaiKhieuNaiChiTiet.SelectedValue);
                        }
                        else if (ddlistLoaiKhieuNai.SelectedValue != "-1" && ddlistLoaiKhieuNai.SelectedValue != "")
                        {
                            objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaiKhieuNai.SelectedValue);
                        }
                        else if (ddlistLoaDonThu.SelectedValue != "-1" && ddlistLoaDonThu.SelectedValue != "")
                        {
                            objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaDonThu.SelectedValue);
                        }
                        objDONTHU.LOAIDONTHU_CHA_ID = int.Parse(ddlistLoaDonThu.SelectedValue);

                        objDONTHU.DONTHU_NOIDUNG = ClassCommon.ClearHTML(textNoiDungDonThu.Text.Trim());

                        if (txtNgayNhanDon.Text != "")
                        {
                            objDONTHU.NGAYTAO = DateTime.Parse(txtNgayNhanDon.Text);
                        }
                        if (txtNgayDeDon.Text != "")
                        {
                            objDONTHU.NGUONDON_NGAYDEDON = DateTime.Parse(txtNgayDeDon.Text);
                        }
                        #region Thông tin cơ quan đã giải quyết
                        objDONTHU.DAGIAIQUYET_DONTHU = cboxCoQuanDaGiaiQuyet.Checked;
                        if (cboxCoQuanDaGiaiQuyet.Checked)
                        {
                            objDONTHU.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = true;
                            if (ddlistCoQuanDaGiaiQuyet.SelectedValue != "")
                            {
                                objDONTHU.DAGIAIQUYET_DONVI_ID = Int32.Parse(ddlistCoQuanDaGiaiQuyet.SelectedValue);
                            }
                            if (textLanGiaiQuyet.Text != "")
                            {
                                objDONTHU.DAGIAIQUYET_LAN = Int32.Parse(textLanGiaiQuyet.Text);
                            }
                            if (textNgayBanHanhQuyetDinh.Text != "")
                            {
                                objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = DateTime.Parse(textNgayBanHanhQuyetDinh.Text);
                            }
                            if (textNgayBanHanhQuyetDinh.Text != "")
                            {
                                objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = DateTime.Parse(textNgayBanHanhQuyetDinh.Text);
                            }
                            objDONTHU.DAGIAIQUYET_HTGQ_ID = Int32.Parse(ddlistHinhThucGiaiQuyet.SelectedValue);
                            objDONTHU.DAGIAIQUYET_KETQUA_CQ = ClassCommon.ClearHTML(textKetQuaCuaCoQuanGiaiQuyet.Text.Trim());

                        }
                        else
                        {
                            objDONTHU.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = false;
                            objDONTHU.DAGIAIQUYET_DONVI_ID = null;
                            objDONTHU.DAGIAIQUYET_LAN = null;
                            objDONTHU.DAGIAIQUYET_LAN = null;
                            objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = null;
                            objDONTHU.DAGIAIQUYET_HTGQ_ID = null;
                            objDONTHU.DAGIAIQUYET_KETQUA_CQ = "";
                        }
                        #endregion
                        #region Bổ sung thông tin người bị khiếu nại; tố cáo
                        if (cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked)
                        {
                            objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = true;
                            DOITUONG DOITUONG_BI_KNTC = new DOITUONG();
                            DOITUONG_BI_KNTC.DOITUONG_BIKNTC = true;
                            CANHAN CANHAN_BI_KNTC = new CANHAN();
                            if (ddlistDoiTuongBoSung.SelectedValue == "1")
                            {
                                DOITUONG_BI_KNTC.DOITUONG_TEN = "Cá Nhân";
                                DOITUONG_BI_KNTC.DOITUONG_DIACHI = "";
                                DOITUONG_BI_KNTC.DOITUONG_SONGUOIDAIDIEN = 1;
                                DOITUONG_BI_KNTC.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                                DOITUONG_BI_KNTC.DOITUONG_LOAI = 1;
                            }
                            else if (ddlistDoiTuongBoSung.SelectedValue == "3")
                            {
                                DOITUONG_BI_KNTC.DOITUONG_TEN = ClassCommon.ClearHTML(textTenCoQuanToChuc_BiKhieuNaiToCao.Text.Trim());
                                DOITUONG_BI_KNTC.DOITUONG_DIACHI = "";
                                DOITUONG_BI_KNTC.DOITUONG_SONGUOIDAIDIEN = 1;
                                DOITUONG_BI_KNTC.DOITUONG_SONGUOI = 1;
                                DOITUONG_BI_KNTC.DOITUONG_LOAI = 3;
                            }

                            CANHAN_BI_KNTC.CANHAN_HOTEN = ClassCommon.ClearHTML(textHoTen_NguoiBiKhieuNaiToCao.Text.Trim());
                            CANHAN_BI_KNTC.CANHAN_GIOITINH = rdoDoiTuongNam.Checked;
                            CANHAN_BI_KNTC.NOICONGTAC = ClassCommon.ClearHTML(textNoiCongTac.Text.Trim());
                            CANHAN_BI_KNTC.CHUCVU = ClassCommon.ClearHTML(textChucVu.Text.Trim());
                            CANHAN_BI_KNTC.CANHAN_DIACHI = ClassCommon.ClearHTML(textDiaChi.Text.Trim());

                            #region Lấy thông tin địa chỉ, địa phương người bị khiếu nại; tố cáo
                            int DP_ID_CANHAN_BI_KNTC = -1;
                            if (ddlistXaPhuong.SelectedValue != "-1" && ddlistXaPhuong.SelectedValue != "")
                            {
                                DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistXaPhuong.SelectedValue);
                            }
                            else if (ddlistQuanHuyen.SelectedValue != "-1" && ddlistQuanHuyen.SelectedValue != "")
                            {
                                DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistQuanHuyen.SelectedValue);
                            }
                            else if (ddlistTinhThanh.SelectedValue != "-1" && ddlistTinhThanh.SelectedValue != "")
                            {
                                DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistTinhThanh.SelectedValue);
                            }
                            string vDiaChi = "";
                            if (DP_ID_CANHAN_BI_KNTC != -1)
                            {
                                CANHAN_BI_KNTC.DP_ID = DP_ID_CANHAN_BI_KNTC;
                                DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DP_ID_CANHAN_BI_KNTC).FirstOrDefault();
                                if (DIAPHUONG.DP_ID_CHA > 0)
                                {
                                    DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                    if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                    }
                                    else
                                    {
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                    }
                                }
                                else
                                {
                                    vDiaChi = ", " + DIAPHUONG.DP_TEN;
                                }
                                CANHAN_BI_KNTC.CANHAN_DIACHI_DAYDU = (CANHAN_BI_KNTC.CANHAN_DIACHI != "" ? (CANHAN_BI_KNTC.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            }
                            else
                            {
                                CANHAN_BI_KNTC.CANHAN_DIACHI_DAYDU = CANHAN_BI_KNTC.CANHAN_DIACHI;
                            }
                            #endregion
                            #region Lấy thông tin quốc tịch, dân tộc người bị khiếu nại; tố cáo
                            if (ddlistQuocTich.SelectedValue != "")
                            {
                                CANHAN_BI_KNTC.QUOCTICH_ID = Int32.Parse(ddlistQuocTich.SelectedValue);
                            }
                            else
                            {
                                CANHAN_BI_KNTC.QUOCTICH_ID = null;
                            }
                            if (ddlistDanToc.SelectedValue != "")
                            {
                                CANHAN_BI_KNTC.DANTOC_ID = Int32.Parse(ddlistDanToc.SelectedValue);
                            }
                            else
                            {
                                CANHAN_BI_KNTC.DANTOC_ID = null;
                            }
                            #endregion
                            vDataContext.DOITUONGs.InsertOnSubmit(DOITUONG_BI_KNTC);
                            vDataContext.SubmitChanges();
                            CANHAN_BI_KNTC.DOITUONG_ID = DOITUONG_BI_KNTC.DOITUONG_ID;
                            vDataContext.CANHANs.InsertOnSubmit(CANHAN_BI_KNTC);
                            vDataContext.SubmitChanges();
                            objDONTHU.DOITUONG_BI_KNTC_ID = DOITUONG_BI_KNTC.DOITUONG_ID;
                        }
                        else
                        {
                            objDONTHU.DOITUONG_BI_KNTC_ID = null;
                            objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = false;
                        }
                        #endregion
                        #region Bổ sung thông tin người đại diện, ủy quyền
                        if (cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked)
                        {
                            objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = true;
                            CANHAN CANHAN_DD_UYQUYEN = new CANHAN();
                            CANHAN_DD_UYQUYEN.CANHAN_HOTEN = ClassCommon.ClearHTML(textHoTenNguoiDaiDien.Text.Trim());
                            CANHAN_DD_UYQUYEN.CANHAN_GIOITINH = rdoDaiDienUyQuyenNam.Checked;
                            CANHAN_DD_UYQUYEN.CANHAN_DIACHI = ClassCommon.ClearHTML(textDiaChiNguoiDaiDienUyQuyen.Text.Trim());
                            #region Lấy thông tin địa chỉ, địa phương người đại diện, ủy quyền
                            int DP_ID_CANHAN_DD_UYQUYEN = -1;
                            if (ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue != "")
                            {
                                DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue);
                            }
                            else if (ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue != "")
                            {
                                DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue);
                            }
                            else if (ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue != "")
                            {
                                DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue);
                            }
                            string vDiaChi = "";
                            if (DP_ID_CANHAN_DD_UYQUYEN != -1)
                            {
                                CANHAN_DD_UYQUYEN.DP_ID = DP_ID_CANHAN_DD_UYQUYEN;
                                DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DP_ID_CANHAN_DD_UYQUYEN).FirstOrDefault();
                                if (DIAPHUONG.DP_ID_CHA > 0)
                                {
                                    DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                    if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                    }
                                    else
                                    {
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                    }
                                }
                                else
                                {
                                    vDiaChi = ", " + DIAPHUONG.DP_TEN;
                                }

                                CANHAN_DD_UYQUYEN.CANHAN_DIACHI_DAYDU = (CANHAN_DD_UYQUYEN.CANHAN_DIACHI != "" ? (CANHAN_DD_UYQUYEN.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            }
                            else
                            {
                                CANHAN_DD_UYQUYEN.CANHAN_DIACHI_DAYDU = CANHAN_DD_UYQUYEN.CANHAN_DIACHI;
                            }
                            #endregion
                            #region Lấy thông tin quốc tịch, dân tộc phương người đại diện, ủy quyền
                            if (ddlistQuocTich.SelectedValue != "")
                            {
                                CANHAN_DD_UYQUYEN.QUOCTICH_ID = Int32.Parse(ddlistQuocTichNguoiDaiDienUyQuyen.SelectedValue);
                            }
                            else
                            {
                                CANHAN_DD_UYQUYEN.QUOCTICH_ID = -1;
                            }

                            #endregion

                            vDataContext.CANHANs.InsertOnSubmit(CANHAN_DD_UYQUYEN);
                            vDataContext.SubmitChanges();
                            objDONTHU.NGUOIUYQUYEN_CANHAN_ID = CANHAN_DD_UYQUYEN.CANHAN_ID;
                        }
                        else
                        {
                            objDONTHU.NGUOIUYQUYEN_CANHAN_ID = null;
                            objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = false;
                        }
                        #endregion
                        #region Hướng xử lý
                        if (ddlistHuongXuLy.SelectedValue != "")
                        {
                            objDONTHU.HUONGXULY_ID = Int32.Parse(ddlistHuongXuLy.SelectedValue);
                            objDONTHU.DONTHU_TRANGTHAI = 1;
                            objDONTHU.HUONGXULY_TEN = ddlistHuongXuLy.SelectedItem.Text;
                        }
                        else
                        {
                            objDONTHU.HUONGXULY_ID = null;
                            objDONTHU.DONTHU_TRANGTHAI = 0;
                            objDONTHU.HUONGXULY_TEN = null;
                        }
                        if (ddlistThamQuyenGiaiQuyet.SelectedValue != "")
                        {
                            objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID = Int32.Parse(ddlistThamQuyenGiaiQuyet.SelectedValue);
                            objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_TEN = ddlistThamQuyenGiaiQuyet.SelectedItem.Text;
                        }
                        else
                        {
                            objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID = null;
                            objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_TEN = null;
                        }
                        if (ddlistCoQuanTiepNhan.SelectedValue != "")
                        {
                            objDONTHU.HUONGXULY_DONVI_ID = Int32.Parse(ddlistCoQuanTiepNhan.SelectedValue);
                        }
                        if (ddlistNguoiTiepNhan.SelectedValue != "")
                        {
                            objDONTHU.HUONGXULY_CANBO = int.Parse(ddlistNguoiTiepNhan.SelectedValue);
                        }

                        if (textNgayChuyen_HuongXuLy.Text != "")
                        {
                            objDONTHU.HUONGXULY_NGAYCHUYEN = DateTime.Parse(textNgayChuyen_HuongXuLy.Text);
                        }
                        objDONTHU.HUONGXULY_SOHIEUVB_DI = ClassCommon.ClearHTML(textSoHieuVanBanDi.Text.Trim());
                        if (ddlistNguoiDuyet.SelectedValue != "")
                        {
                            objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID = Int32.Parse(ddlistNguoiDuyet.SelectedValue);
                        }

                        objDONTHU.HUONGXULY_CHUCVU_TEN = ClassCommon.ClearHTML(textChucVu_HuongXuLy.Text.Trim());
                        if (textThoiHanGiaiQuyet.Text != "")
                        {
                            objDONTHU.HUONGXULY_THOIHANGIAIQUET = DateTime.Parse(textThoiHanGiaiQuyet.Text);
                        }
                        objDONTHU.HUONGXULY_YKIEN_XULY = ClassCommon.ClearHTML(textYKienXuLy.Text.Trim());
                        objDONTHU.DONTHU_GHICHU = ClassCommon.ClearHTML(textGhiChu.Text.Trim());
                        #endregion

                        vDataContext.DONTHUs.InsertOnSubmit(objDONTHU);
                        vDataContext.SubmitChanges();
                        TIEPDANInfo.DONTHU_ID = objDONTHU.DONTHU_ID;
                        //Thêm mới hồ sơ đơn thư vào CSDL
                        if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] != null)
                        {
                            List<HOSO> vHoSoInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"];
                            if (vHoSoInfo_Session.Count > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfo_Session)
                                {
                                    long oHoSoId = 0;
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
                                        vDonThuHoSoInfo.DONTHU_ID = objDONTHU.DONTHU_ID;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 0;//Hồ sơ đơn thư 
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoDonThuInfos"] = null;
                            }
                        }

                        //Thêm mới hồ sơ hướng xử lý vào CSDL
                        if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"] != null)
                        {
                            List<HOSO> vHoSoInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoHuongXuLyInfos"];
                            if (vHoSoInfo_Session.Count > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfo_Session)
                                {
                                    long oHoSoId = 0;
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
                                        vDonThuHoSoInfo.DONTHU_ID = objDONTHU.DONTHU_ID;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 4;//Hồ sơ hướng xử lý
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                            }
                        }

                        //Thêm mới hồ sơ người đại diện ủy quyền
                        if (Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] != null)
                        {
                            List<HOSO> vHoSoInfo_Session = (List<HOSO>)Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"];
                            if (vHoSoInfo_Session.Count > 0)
                            {
                                foreach (var vHoSoInfo in vHoSoInfo_Session)
                                {
                                    long oHoSoId = 0;
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
                                        vDonThuHoSoInfo.DONTHU_ID = objDONTHU.DONTHU_ID;
                                        vDonThuHoSoInfo.HOSO_ID = oHoSoId;
                                        vDonThuHoSoInfo.LOAI_HS_DONTHU = 1;//Hồ sơ người đại diện ủy quyền
                                        vDataContext.DONTHU_HOSOs.InsertOnSubmit(vDonThuHoSoInfo);
                                        vDataContext.SubmitChanges();
                                    }
                                }
                                Session[PortalSettings.ActiveTab.TabID + vMacAddress + _currentUser.UserID + "_CurrenPage_" + "vHoSoNguoiDaiDienUyQuyenInfos"] = null;
                            }
                        }

                        #endregion

                    }
                    else
                    {
                        TIEPDANInfo.DONTHU_ID = null;

                    }

                    vDataContext.TIEPDANs.InsertOnSubmit(TIEPDANInfo);
                    vDataContext.SubmitChanges();

                    if (TIEPDANInfo.TIEPDAN_ID > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin tiếp dân", "id=" + TIEPDANInfo.TIEPDAN_ID);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới tiếp dân thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin tiếp dân
                {
                    var TIEPDANInfo = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == pTIEPDAN_ID).SingleOrDefault();
                    if (TIEPDANInfo != null)
                    {
                        // Set thông tin tiếp dân
                        int vLOAIDOITUONG = Int32.Parse(drpDOITUONG.SelectedValue);
                        TIEPDANInfo.TIEPDAN_LOAI = Int32.Parse(GetLoaiDonThu());
                        TIEPDANInfo.TIEPDAN_NOIDUNG = ClassCommon.ClearHTML(txtNoiDungTiepDan.Text.Trim());
                        TIEPDANInfo.TIEPDAN_KETQUA = ClassCommon.ClearHTML(txtKetQua.Text.Trim());
                        TIEPDANInfo.TIEPDAN_LANTIEP = Int32.Parse(txtLanTiep.Text.Trim());
                        TIEPDANInfo.TIEPDAN_THOGIAN = DateTime.Parse(txtNgayTiepDan.Text);
                        var DOITUONGInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == TIEPDANInfo.DOITUONG_ID).SingleOrDefault();
                        if (vLOAIDOITUONG == 1)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Cá Nhân";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1;
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 1;
                        }
                        else if (vLOAIDOITUONG == 2)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Nhóm đông người";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = Int32.Parse(txtSoNguoiDaiDien.Text);
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 2;
                        }
                        else
                        {
                            DOITUONGInfo.DOITUONG_TEN = ClassCommon.ClearHTML(txtTenCoQuanToChuc.Text.Trim());
                            DOITUONGInfo.DOITUONG_DIACHI = ClassCommon.ClearHTML(txtDiaChiDoiTuong.Text.Trim());
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1;
                            DOITUONGInfo.DOITUONG_SONGUOI = 1;
                            DOITUONGInfo.DOITUONG_LOAI = 3;
                        }
                        DOITUONGInfo.DOITUONG_NACDANH = false;
                        DOITUONGInfo.NGAYCAPNHAT = DateTime.Now;
                        DOITUONGInfo.NGUOICAPNHAT = _currentUser.UserID;

                        // Danh sách cá nhân cũ
                        List<CANHAN> cANHANs = new List<CANHAN>();
                        cANHANs = vDataContext.CANHANs.Where(x => x.DOITUONG_ID == DOITUONGInfo.DOITUONG_ID).ToList();


                        List<CANHAN> cANHAN_Lists = new List<CANHAN>();
                        foreach (var item in ListViewDoiTuong.Items)
                        {
                            CANHAN cANHAN = new CANHAN();
                            cANHAN = GetThongTinCaNhan(item);
                            cANHAN_Lists.Add(cANHAN);
                        }
                        // Update
                        List<CANHAN> cANHAN_Update = cANHAN_Lists.Where(x => x.CANHAN_ID > 0).ToList();
                        List<Int64> cANHAN_Update_ID = cANHAN_Update.Select(x => x.CANHAN_ID).ToList();

                        for (int i = 0; i < cANHAN_Update.Count; i++)
                        {

                            CANHAN cANHAN_UPDATE = vDataContext.CANHANs.Where(x => x.CANHAN_ID == cANHAN_Update[i].CANHAN_ID).FirstOrDefault();
                            //SET THÔNG TIN CÁ NHÂN
                            cANHAN_UPDATE.CANHAN_CMDN = cANHAN_Update[i].CANHAN_CMDN;
                            cANHAN_UPDATE.CANHAN_CMDN_NGAYCAP = cANHAN_Update[i].CANHAN_CMDN_NGAYCAP;
                            cANHAN_UPDATE.CANHAN_DIACHI = cANHAN_Update[i].CANHAN_DIACHI;
                            cANHAN_UPDATE.DP_ID = cANHAN_Update[i].DP_ID;
                            string vDiaChi = "";
                            if (cANHAN_UPDATE.DP_ID > 0)
                            {
                                DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == cANHAN_UPDATE.DP_ID).FirstOrDefault();
                                if (DIAPHUONG.DP_ID_CHA > 0)
                                {
                                    DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                    if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                    }
                                    else
                                    {
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                    }

                                }
                                else
                                {
                                    vDiaChi = ", " + DIAPHUONG.DP_TEN;
                                }

                            }
                            //cANHAN_UPDATE.CANHAN_DIACHI_DAYDU = (cANHAN_UPDATE.CANHAN_DIACHI != "" ? (cANHAN_UPDATE.CANHAN_DIACHI +", " ) : "") + vDiaChi;
                            if (vDiaChi != "")
                            {
                                cANHAN_UPDATE.CANHAN_DIACHI_DAYDU = (cANHAN_UPDATE.CANHAN_DIACHI != "" ? (cANHAN_UPDATE.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            }
                            else
                            {
                                cANHAN_UPDATE.CANHAN_DIACHI_DAYDU = cANHAN_UPDATE.CANHAN_DIACHI;
                            }
                            cANHAN_UPDATE.CANHAN_GIOITINH = cANHAN_Update[i].CANHAN_GIOITINH;
                            cANHAN_UPDATE.CANHAN_HOTEN = cANHAN_Update[i].CANHAN_HOTEN;
                            cANHAN_UPDATE.CANHAN_NOICAP = cANHAN_Update[i].CANHAN_NOICAP;
                            cANHAN_UPDATE.DANTOC_ID = cANHAN_Update[i].DANTOC_ID;

                            cANHAN_UPDATE.QUOCTICH_ID = cANHAN_Update[i].QUOCTICH_ID;
                            cANHAN_UPDATE.NGAYCAPNHAT = DateTime.Now;
                            cANHAN_UPDATE.NGUOICAPNHAT = _currentUser.UserID;
                            vDataContext.SubmitChanges();
                        }
                        List<CANHAN> cANHAN_Inserts = cANHAN_Lists.Where(x => x.CANHAN_ID < 0).ToList();
                        for (int i = 0; i < cANHAN_Inserts.Count; i++)
                        {

                            CANHAN cANHAN_Insert = cANHAN_Inserts[i];

                            cANHAN_Insert.DOITUONG_ID = DOITUONGInfo.DOITUONG_ID;
                            cANHAN_Insert.NGAYTAO = DateTime.Now;
                            cANHAN_Insert.NGUOITAO = _currentUser.UserID;
                            vDataContext.CANHANs.InsertOnSubmit(cANHAN_Insert);
                            vDataContext.SubmitChanges();
                        }
                        List<CANHAN> cANHAN_Delete = cANHANs.Where(x => !cANHAN_Update_ID.Contains(x.CANHAN_ID)).ToList();
                        vDataContext.CANHANs.DeleteAllOnSubmit(cANHAN_Delete);
                        vDataContext.SubmitChanges();
                        //
                        //Đơn thư
                        //
                        if (rdoCoDon.Checked)
                        {
                            #region Xử lý đơn thư
                            DONTHU objDONTHU;
                            bool Is_InsertDonThu = false;
                            // trường hợp đã có đơn thư
                            if (TIEPDANInfo.DONTHU_ID != null)
                            {
                                objDONTHU = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == TIEPDANInfo.DONTHU_ID).FirstOrDefault();
                                if (objDONTHU == null)
                                {
                                    Is_InsertDonThu = true;
                                    objDONTHU = new DONTHU();
                                }
                            }
                            else
                            {
                                Is_InsertDonThu = true;
                                objDONTHU = new DONTHU();
                            }
                            if (Is_InsertDonThu)
                            {
                                objDONTHU.DONTHU_STT = (vDataContext.DONTHUs.Where(x => ((DateTime)x.NGUONDON_NGAYDEDON).Year == DateTime.Now.Year).ToList().Max(x => x.DONTHU_STT) + 1);
                                objDONTHU.NGUONDON_LOAI = false;// Trực tiếp;
                            }
                            objDONTHU.DOITUONG_ID = TIEPDANInfo.DOITUONG_ID;
                            objDONTHU.DONTHU_KHONGDUDDIEUKIEN = cboxDonThuKhongDuDieuKien.Checked;
                            objDONTHU.NGUONDON_LOAI = false;// Trực tiếp;
                            objDONTHU.NGUONDON_LOAI_CHITIET = 0;
                            if (ddlistLoaiKhieuNaiChiTiet.SelectedValue != "-1" && ddlistLoaiKhieuNaiChiTiet.SelectedValue != "")
                            {
                                objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaiKhieuNaiChiTiet.SelectedValue);
                            }
                            else if (ddlistLoaiKhieuNai.SelectedValue != "-1" && ddlistLoaiKhieuNai.SelectedValue != "")
                            {
                                objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaiKhieuNai.SelectedValue);
                            }
                            else if (ddlistLoaDonThu.SelectedValue != "-1" && ddlistLoaDonThu.SelectedValue != "")
                            {
                                objDONTHU.LOAIDONTHU_ID = int.Parse(ddlistLoaDonThu.SelectedValue);
                            }
                            objDONTHU.LOAIDONTHU_CHA_ID = int.Parse(ddlistLoaDonThu.SelectedValue);

                            objDONTHU.DONTHU_NOIDUNG = ClassCommon.ClearHTML(textNoiDungDonThu.Text.Trim());
                            if (txtNgayNhanDon.Text != "")
                            {
                                objDONTHU.NGAYTAO = DateTime.Parse(txtNgayNhanDon.Text);
                            }
                            else
                            {
                                objDONTHU.NGAYTAO = null;
                            }
                            if (txtNgayDeDon.Text != "")
                            {
                                objDONTHU.NGUONDON_NGAYDEDON = DateTime.Parse(txtNgayDeDon.Text);
                            }
                            else
                            {
                                objDONTHU.NGUONDON_NGAYDEDON = null;
                            }
                            #region Thông tin cơ quan đã giải quyết
                            objDONTHU.DAGIAIQUYET_DONTHU = cboxCoQuanDaGiaiQuyet.Checked;
                            if (cboxCoQuanDaGiaiQuyet.Checked)
                            {
                                objDONTHU.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = true;
                                if (ddlistCoQuanDaGiaiQuyet.SelectedValue != "")
                                {
                                    objDONTHU.DAGIAIQUYET_DONVI_ID = Int32.Parse(ddlistCoQuanDaGiaiQuyet.SelectedValue);
                                }
                                else
                                {
                                    objDONTHU.DAGIAIQUYET_DONVI_ID = null;
                                }
                                if (textLanGiaiQuyet.Text != "")
                                {
                                    objDONTHU.DAGIAIQUYET_LAN = Int32.Parse(textLanGiaiQuyet.Text);
                                }
                                else
                                {
                                    objDONTHU.DAGIAIQUYET_LAN = null;
                                }
                                if (textNgayBanHanhQuyetDinh.Text != "")
                                {
                                    objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = DateTime.Parse(textNgayBanHanhQuyetDinh.Text);
                                }
                                else
                                {
                                    objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = null;
                                }

                                if (ddlistHinhThucGiaiQuyet.SelectedValue != "")
                                {
                                    objDONTHU.DAGIAIQUYET_HTGQ_ID = Int32.Parse(ddlistHinhThucGiaiQuyet.SelectedValue);
                                }
                                else
                                {
                                    objDONTHU.DAGIAIQUYET_HTGQ_ID = null;
                                }


                                objDONTHU.DAGIAIQUYET_KETQUA_CQ = ClassCommon.ClearHTML(textKetQuaCuaCoQuanGiaiQuyet.Text.Trim());
                            }
                            else
                            {
                                objDONTHU.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = false;
                                objDONTHU.DAGIAIQUYET_DONVI_ID = null;
                                objDONTHU.DAGIAIQUYET_LAN = null;
                                objDONTHU.DAGIAIQUYET_LAN = null;
                                objDONTHU.DAGIAIQUYET_NGAYBANHANH_QD = null;
                                objDONTHU.DAGIAIQUYET_HTGQ_ID = null;
                                objDONTHU.DAGIAIQUYET_KETQUA_CQ = "";
                            }
                            #endregion
                            #region Bổ sung thông tin người bị khiếu nại; tố cáo
                            if (cboxBoSungThongTinNguoiBiKhieuNaiToCao.Checked)
                            {
                                objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = true;
                                DOITUONG DOITUONG_BI_KNTC = new DOITUONG();
                                CANHAN CANHAN_BI_KNTC = new CANHAN();
                                bool Is_InsertDOITUONG_BI_KNTC = false;
                                if (objDONTHU.DOITUONG_BI_KNTC_ID != null)
                                {
                                    DOITUONG_BI_KNTC = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == objDONTHU.DOITUONG_BI_KNTC_ID).FirstOrDefault();
                                    if (DOITUONG_BI_KNTC != null)
                                    {
                                        CANHAN_BI_KNTC = vDataContext.CANHANs.Where(x => x.DOITUONG_ID == objDONTHU.DOITUONG_BI_KNTC_ID).FirstOrDefault();
                                        if (CANHAN_BI_KNTC != null)
                                        {

                                        }
                                        else
                                        {
                                            DOITUONG_BI_KNTC = new DOITUONG();
                                            CANHAN_BI_KNTC = new CANHAN();
                                            Is_InsertDOITUONG_BI_KNTC = true;
                                        }
                                    }
                                    else
                                    {
                                        DOITUONG_BI_KNTC = new DOITUONG();
                                        CANHAN_BI_KNTC = new CANHAN();
                                        Is_InsertDOITUONG_BI_KNTC = true;
                                    }
                                }
                                else
                                {
                                    Is_InsertDOITUONG_BI_KNTC = true;
                                }
                                DOITUONG_BI_KNTC.DOITUONG_BIKNTC = true;

                                if (ddlistDoiTuongBoSung.SelectedValue == "1")
                                {
                                    DOITUONG_BI_KNTC.DOITUONG_TEN = "Cá Nhân";
                                    DOITUONG_BI_KNTC.DOITUONG_DIACHI = "";
                                    DOITUONG_BI_KNTC.DOITUONG_SONGUOIDAIDIEN = 1;
                                    DOITUONG_BI_KNTC.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                                    DOITUONG_BI_KNTC.DOITUONG_LOAI = 1;
                                }
                                else if (ddlistDoiTuongBoSung.SelectedValue == "3")
                                {
                                    DOITUONG_BI_KNTC.DOITUONG_TEN = ClassCommon.ClearHTML(textTenCoQuanToChuc_BiKhieuNaiToCao.Text.Trim());
                                    DOITUONG_BI_KNTC.DOITUONG_DIACHI = "";
                                    DOITUONG_BI_KNTC.DOITUONG_SONGUOIDAIDIEN = 1;
                                    DOITUONG_BI_KNTC.DOITUONG_SONGUOI = 1;
                                    DOITUONG_BI_KNTC.DOITUONG_LOAI = 3;
                                }

                                CANHAN_BI_KNTC.CANHAN_HOTEN = ClassCommon.ClearHTML(textHoTen_NguoiBiKhieuNaiToCao.Text.Trim());
                                CANHAN_BI_KNTC.CANHAN_GIOITINH = rdoDoiTuongNam.Checked;
                                CANHAN_BI_KNTC.NOICONGTAC = ClassCommon.ClearHTML(textNoiCongTac.Text.Trim());
                                CANHAN_BI_KNTC.CHUCVU = ClassCommon.ClearHTML(textChucVu.Text.Trim());
                                CANHAN_BI_KNTC.CANHAN_DIACHI = ClassCommon.ClearHTML(textDiaChi.Text.Trim());

                                #region Lấy thông tin địa chỉ, địa phương người bị khiếu nại; tố cáo
                                int DP_ID_CANHAN_BI_KNTC = -1;
                                if (ddlistXaPhuong.SelectedValue != "-1" && ddlistXaPhuong.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistXaPhuong.SelectedValue);
                                }
                                else if (ddlistQuanHuyen.SelectedValue != "-1" && ddlistQuanHuyen.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistQuanHuyen.SelectedValue);
                                }
                                else if (ddlistTinhThanh.SelectedValue != "-1" && ddlistTinhThanh.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_BI_KNTC = int.Parse(ddlistTinhThanh.SelectedValue);
                                }
                                string vDiaChi = "";
                                if (DP_ID_CANHAN_BI_KNTC != -1)
                                {
                                    CANHAN_BI_KNTC.DP_ID = DP_ID_CANHAN_BI_KNTC;
                                    DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DP_ID_CANHAN_BI_KNTC).FirstOrDefault();
                                    if (DIAPHUONG.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                        if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                        {
                                            DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                            vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                        }
                                        else
                                        {
                                            vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                        }
                                    }
                                    else
                                    {
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN;
                                    }
                                    CANHAN_BI_KNTC.CANHAN_DIACHI_DAYDU = (CANHAN_BI_KNTC.CANHAN_DIACHI != "" ? (CANHAN_BI_KNTC.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                                }
                                else
                                {
                                    CANHAN_BI_KNTC.DP_ID = null;
                                    CANHAN_BI_KNTC.CANHAN_DIACHI_DAYDU = CANHAN_BI_KNTC.CANHAN_DIACHI;
                                }
                                #endregion
                                #region Lấy thông tin quốc tịch, dân tộc người bị khiếu nại; tố cáo
                                if (ddlistQuocTich.SelectedValue != "")
                                {
                                    CANHAN_BI_KNTC.QUOCTICH_ID = Int32.Parse(ddlistQuocTich.SelectedValue);
                                }
                                else
                                {
                                    CANHAN_BI_KNTC.QUOCTICH_ID = null;
                                }
                                if (ddlistDanToc.SelectedValue != "")
                                {
                                    CANHAN_BI_KNTC.DANTOC_ID = Int32.Parse(ddlistDanToc.SelectedValue);
                                }
                                else
                                {
                                    CANHAN_BI_KNTC.DANTOC_ID = null;
                                }
                                #endregion
                                if (Is_InsertDOITUONG_BI_KNTC)
                                {
                                    vDataContext.DOITUONGs.InsertOnSubmit(DOITUONG_BI_KNTC);
                                    vDataContext.SubmitChanges();
                                    CANHAN_BI_KNTC.DOITUONG_ID = DOITUONG_BI_KNTC.DOITUONG_ID;
                                    vDataContext.CANHANs.InsertOnSubmit(CANHAN_BI_KNTC);
                                    vDataContext.SubmitChanges();
                                    objDONTHU.DOITUONG_BI_KNTC_ID = DOITUONG_BI_KNTC.DOITUONG_ID;
                                }
                                else
                                {
                                    vDataContext.SubmitChanges();
                                }
                            }
                            else
                            {
                                objDONTHU.DOITUONG_BI_KNTC_ID = null;
                                objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = false;
                            }
                            #endregion
                            #region Bổ sung thông tin người đại diện, ủy quyền
                            if (cboxBoSungThongTinNguoiDaiDienUyQuyen.Checked)
                            {
                                objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = true;
                                CANHAN CANHAN_DD_UYQUYEN = new CANHAN();
                                bool Is_InsertNGUOIUYQUYEN = false;
                                if (objDONTHU.NGUOIUYQUYEN_CANHAN_ID != null)
                                {
                                    CANHAN_DD_UYQUYEN = vDataContext.CANHANs.Where(x => x.CANHAN_ID == objDONTHU.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();
                                    if (CANHAN_DD_UYQUYEN == null)
                                    {
                                        Is_InsertNGUOIUYQUYEN = true;
                                    }
                                }
                                else
                                {
                                    Is_InsertNGUOIUYQUYEN = true;
                                }
                                CANHAN_DD_UYQUYEN.CANHAN_HOTEN = ClassCommon.ClearHTML(textHoTenNguoiDaiDien.Text.Trim());
                                CANHAN_DD_UYQUYEN.CANHAN_GIOITINH = rdoDaiDienUyQuyenNam.Checked;
                                CANHAN_DD_UYQUYEN.CANHAN_DIACHI = ClassCommon.ClearHTML(textDiaChiNguoiDaiDienUyQuyen.Text.Trim());
                                #region Lấy thông tin địa chỉ, địa phương người đại diện, ủy quyền
                                int DP_ID_CANHAN_DD_UYQUYEN = -1;
                                if (ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistXaPhuongNguoiDaiDienUyQuyen.SelectedValue);
                                }
                                else if (ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistQuanHuyenNguoiDaiDienUyQuyen.SelectedValue);
                                }
                                else if (ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue != "-1" && ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue != "")
                                {
                                    DP_ID_CANHAN_DD_UYQUYEN = int.Parse(ddlistTinhThanhNguoiDaiDienUyQuyen.SelectedValue);
                                }
                                string vDiaChi = "";
                                if (DP_ID_CANHAN_DD_UYQUYEN != -1)
                                {
                                    CANHAN_DD_UYQUYEN.DP_ID = DP_ID_CANHAN_DD_UYQUYEN;
                                    DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DP_ID_CANHAN_DD_UYQUYEN).FirstOrDefault();
                                    if (DIAPHUONG.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                        if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                        {
                                            DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                            vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
                                        }
                                        else
                                        {
                                            vDiaChi = ", " + DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN;
                                        }
                                    }
                                    else
                                    {
                                        vDiaChi = ", " + DIAPHUONG.DP_TEN;
                                    }

                                    CANHAN_DD_UYQUYEN.CANHAN_DIACHI_DAYDU = (CANHAN_DD_UYQUYEN.CANHAN_DIACHI != "" ? (CANHAN_DD_UYQUYEN.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                                }
                                else
                                {
                                    CANHAN_DD_UYQUYEN.CANHAN_DIACHI_DAYDU = CANHAN_DD_UYQUYEN.CANHAN_DIACHI;
                                    CANHAN_DD_UYQUYEN.DP_ID = null;
                                }
                                #endregion
                                #region Lấy thông tin quốc tịch, dân tộc phương người đại diện, ủy quyền
                                if (ddlistQuocTich.SelectedValue != "")
                                {
                                    CANHAN_DD_UYQUYEN.QUOCTICH_ID = Int32.Parse(ddlistQuocTichNguoiDaiDienUyQuyen.SelectedValue);
                                }
                                else
                                {
                                    CANHAN_DD_UYQUYEN.QUOCTICH_ID = null;
                                }
                                #endregion
                                if (Is_InsertNGUOIUYQUYEN)
                                {
                                    vDataContext.CANHANs.InsertOnSubmit(CANHAN_DD_UYQUYEN);
                                    vDataContext.SubmitChanges();
                                    objDONTHU.NGUOIUYQUYEN_CANHAN_ID = CANHAN_DD_UYQUYEN.CANHAN_ID;
                                }
                                else
                                {
                                    vDataContext.SubmitChanges();
                                }
                            }
                            else
                            {
                                objDONTHU.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = false;
                                objDONTHU.NGUOIUYQUYEN_CANHAN_ID = null;
                            }
                            #endregion
                            #region Hướng xử lý
                            if (ddlistHuongXuLy.SelectedValue != "")
                            {
                                objDONTHU.HUONGXULY_ID = Int32.Parse(ddlistHuongXuLy.SelectedValue);
                                objDONTHU.DONTHU_TRANGTHAI = 1;
                                objDONTHU.HUONGXULY_TEN = ddlistHuongXuLy.SelectedItem.Text;
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_ID = null;
                                objDONTHU.DONTHU_TRANGTHAI = 0;
                                objDONTHU.HUONGXULY_TEN = null;
                            }
                            if (ddlistThamQuyenGiaiQuyet.SelectedValue != "")
                            {
                                objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID = Int32.Parse(ddlistThamQuyenGiaiQuyet.SelectedValue);
                                objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_TEN = ddlistThamQuyenGiaiQuyet.SelectedItem.Text;
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_ID = null;
                                objDONTHU.HUONGXULY_THAMQUYENGIAIQUYET_TEN = null;

                            }
                            if (ddlistCoQuanTiepNhan.SelectedValue != "")
                            {
                                objDONTHU.HUONGXULY_DONVI_ID = Int32.Parse(ddlistCoQuanTiepNhan.SelectedValue);
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_DONVI_ID = null;
                            }
                            if (ddlistNguoiTiepNhan.SelectedValue != "")
                            {
                                objDONTHU.HUONGXULY_CANBO = int.Parse(ddlistNguoiTiepNhan.SelectedValue);
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_CANBO = null;
                            }
                            if (textNgayChuyen_HuongXuLy.Text != "")
                            {
                                objDONTHU.HUONGXULY_NGAYCHUYEN = DateTime.Parse(textNgayChuyen_HuongXuLy.Text);
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_NGAYCHUYEN = null;
                            }
                            objDONTHU.HUONGXULY_SOHIEUVB_DI = ClassCommon.ClearHTML(textSoHieuVanBanDi.Text.Trim());
                            if (ddlistNguoiDuyet.SelectedValue != "")
                            {
                                objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID = Int32.Parse(ddlistNguoiDuyet.SelectedValue);
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID = null;
                            }
                            objDONTHU.HUONGXULY_CHUCVU_TEN = ClassCommon.ClearHTML(textChucVu_HuongXuLy.Text.Trim());
                            if (textThoiHanGiaiQuyet.Text != "")
                            {
                                objDONTHU.HUONGXULY_THOIHANGIAIQUET = DateTime.Parse(textThoiHanGiaiQuyet.Text);
                            }
                            else
                            {
                                objDONTHU.HUONGXULY_THOIHANGIAIQUET = null;
                            }

                            objDONTHU.HUONGXULY_YKIEN_XULY = ClassCommon.ClearHTML(textYKienXuLy.Text.Trim());
                            objDONTHU.DONTHU_GHICHU = ClassCommon.ClearHTML(textGhiChu.Text.Trim());

                            #endregion
                            if (Is_InsertDonThu)
                            {
                                vDataContext.DONTHUs.InsertOnSubmit(objDONTHU);
                            }
                            vDataContext.SubmitChanges();
                            TIEPDANInfo.DONTHU_ID = objDONTHU.DONTHU_ID;
                            vDataContext.SubmitChanges();
                            #endregion
                        }
                        else
                        {
                            TIEPDANInfo.DONTHU_ID = null;
                            vDataContext.SubmitChanges();
                        }
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin tiếp dân thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;
                        btnSua.Visible = true;
                        btnNhanBan.Visible = true;
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
        protected void btnBreadcrumbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
        }
        protected void btn_ThemMoi_Click(object sender, EventArgs e)
        {

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
                objCANHANAppend.DP_ID = ClassParameter.vDiaPhuongDefault;
                objCANHANAppend.QUOCTICH_ID = ClassParameter.vQuocTichDefault;
                objCANHANAppend.DANTOC_ID = ClassParameter.vDanTocDefault;

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
            if (pDropDownListXa.SelectedValue != "" && pDropDownListXa.SelectedValue != "-1")
            {
                objCANHAN.DP_ID = Int32.Parse(pDropDownListXa.SelectedValue);
            }
            else if (pDropDownListHuyen.SelectedValue != "" && pDropDownListHuyen.SelectedValue != "-1")
            {
                objCANHAN.DP_ID = Int32.Parse(pDropDownListHuyen.SelectedValue);
            }
            else if (pDropDownListThanhpho.SelectedValue != "" && pDropDownListThanhpho.SelectedValue != "-1")
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
            dIAPHUONGs = vDataContext.DIAPHUONGs.ToList();
            if (vDP_ID == -1)
            {
                List<DIAPHUONG> DIAPHUONGTinhs = dIAPHUONGs.Where(x => x.CapDo == 1).ToList();
                pDropDownListThanhpho.DataSource = DIAPHUONGTinhs;
                pDropDownListThanhpho.DataTextField = "DP_TEN";
                pDropDownListThanhpho.DataValueField = "DP_ID";
                pDropDownListThanhpho.DataBind();
                pDropDownListThanhpho.Items.Insert(0, new ListItem("Chọn Tỉnh/thành phố", "-1"));
                //pDropDownListThanhpho.SelectedValue = "-1";

                pDropDownListHuyen.DataBind();
                pDropDownListHuyen.Items.Insert(0, new ListItem("Chọn Quận/huyện", "-1"));
                //pDropDownListHuyen.SelectedValue = "";

                pDropDownListXa.Items.Clear();
                pDropDownListXa.Items.Insert(0, new ListItem("Chọn Xã/Phường", "-1"));
                //pDropDownListXa.SelectedValue = "-1";
            }
            else
            {
                DIAPHUONG dIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                if (dIAPHUONG != null)
                {
                    if (dIAPHUONG.CapDo == 3)
                    {
                        List<DIAPHUONG> DIAPHUONGXas = dIAPHUONGs.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID_CHA).ToList();
                        DIAPHUONG DIAPHUONGHuyen = dIAPHUONGs.Where(x => x.DP_ID == dIAPHUONG.DP_ID_CHA).FirstOrDefault();
                        List<DIAPHUONG> DIAPHUONGHuyens = dIAPHUONGs.Where(x => x.DP_ID_CHA == DIAPHUONGHuyen.DP_ID_CHA).ToList();
                        DIAPHUONG DIAPHUONGTinh = dIAPHUONGs.Where(x => x.DP_ID == DIAPHUONGHuyen.DP_ID_CHA).FirstOrDefault();
                        List<DIAPHUONG> DIAPHUONGTinhs = dIAPHUONGs.Where(x => x.CapDo == 1).ToList();
                        // pDropDownListXa.SelectedValue = null;
                        pDropDownListXa.DataSource = DIAPHUONGXas;
                        pDropDownListXa.DataTextField = "DP_TEN";
                        pDropDownListXa.DataValueField = "DP_ID";
                        pDropDownListXa.DataBind();

                        //pDropDownListHuyen.Items.Clear();
                        pDropDownListHuyen.DataSource = DIAPHUONGHuyens;
                        pDropDownListHuyen.DataTextField = "DP_TEN";
                        pDropDownListHuyen.DataValueField = "DP_ID";
                        pDropDownListHuyen.DataBind();

                        // pDropDownListThanhpho.Items.Clear();
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
                        List<DIAPHUONG> DIAPHUONGXas = dIAPHUONGs.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID).ToList();
                        pDropDownListXa.Items.Clear();
                        pDropDownListXa.DataSource = DIAPHUONGXas;
                        pDropDownListXa.DataTextField = "DP_TEN";
                        pDropDownListXa.DataValueField = "DP_ID";
                        pDropDownListXa.DataBind();
                        pDropDownListXa.Items.Insert(0, new ListItem("Chọn Xã/Phường", "-1"));
                        pDropDownListXa.SelectedValue = "-1";

                    }
                    else if (dIAPHUONG.CapDo == 1)
                    {
                        if (pDropDownListThanhpho.Items.Count <= 1)
                        {
                            List<DIAPHUONG> DIAPHUONGTinhs = dIAPHUONGs.Where(x => x.CapDo == 1).ToList();
                            pDropDownListThanhpho.DataSource = DIAPHUONGTinhs;
                            pDropDownListThanhpho.DataTextField = "DP_TEN";
                            pDropDownListThanhpho.DataValueField = "DP_ID";
                            pDropDownListThanhpho.DataBind();
                            pDropDownListThanhpho.Items.Insert(0, new ListItem("Chọn Tỉnh/thành phố", "-1"));
                            pDropDownListThanhpho.SelectedValue = vDP_ID.ToString();
                        }
                        pDropDownListHuyen.Items.Clear();
                        List<DIAPHUONG> DIAPHUONGHuyens = dIAPHUONGs.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID).ToList();
                        pDropDownListHuyen.Items.Clear();
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
        public void LoadDanToc(int vDANTOCP_ID, DropDownList pdrlDanToc)
        {
            if (pdrlDanToc.Items.FindByValue(vDANTOCP_ID.ToString()) == null)
            {
                dANTOCs = vDataContext.DANTOCs.ToList(); ;
                pdrlDanToc.DataSource = dANTOCs;
                pdrlDanToc.DataTextField = "DANTOC_TEN";
                pdrlDanToc.DataValueField = "DANTOC_ID";
                pdrlDanToc.DataBind();
                pdrlDanToc.Items.Insert(0, new ListItem("Chọn Dân tộc", "-1"));
            }

            pdrlDanToc.SelectedValue = vDANTOCP_ID.ToString();

        }
        public void LoadQuocTich(int vQUOCTICH_ID, DropDownList pdrlQuocTich)
        {
            if (pdrlQuocTich.Items.FindByValue(vQUOCTICH_ID.ToString()) == null)
            {
                qUOCTICHes = vDataContext.QUOCTICHes.ToList(); ;
                pdrlQuocTich.DataSource = qUOCTICHes;
                pdrlQuocTich.DataTextField = "QUOCTICH_TEN";
                pdrlQuocTich.DataValueField = "QUOCTICH_ID";
                pdrlQuocTich.DataBind();
                pdrlQuocTich.Items.Insert(0, new ListItem("Chọn Quốc tịch", "-1"));

            }
            pdrlQuocTich.SelectedValue = vQUOCTICH_ID.ToString();

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
                drlTinhThanhPho.Focus();
            }
            catch (Exception Ex)
            { }

        }
        protected void drpDOITUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loadbtn_ThemNguoiDaiDien();
            drpDOITUONG.Focus();
            // Đối tượng cá nhân không đc nhập số người
            if (drpDOITUONG.SelectedValue == "1")
            {
                txtSoNguoi.Text = "1";
                txtSoNguoi.Enabled = false;
            }
            else
            {
                txtSoNguoi.Enabled = true;
            }
        }
        public void Loadbtn_ThemNguoiDaiDien()
        {
            if (drpDOITUONG.SelectedValue == "1")
            {
                btn_ThemNguoiDaiDien.Visible = false;
                txtSoNguoi.Text = "1";
                panelSoNguoiDaiDien.Visible = false;
                panelSoNguoi.Visible = true;
                PanelDoiTuong.Visible = false;
            }
            else if (drpDOITUONG.SelectedValue == "2")
            {

                if (Int16.Parse(txtSoNguoiDaiDien.Text) > ListViewDoiTuong.Items.Count())
                {
                    btn_ThemNguoiDaiDien.Visible = true;
                }
                else
                {
                    btn_ThemNguoiDaiDien.Visible = false;
                }
                panelSoNguoi.Visible = true;
                panelSoNguoiDaiDien.Visible = true;
                PanelDoiTuong.Visible = false;
            }
            else if (drpDOITUONG.SelectedValue == "3")
            {
                PanelDoiTuong.Visible = true;
                panelSoNguoiDaiDien.Visible = false;
                panelSoNguoi.Visible = false;
            }
        }
        protected void txtSoNguoiDaiDien_TextChanged(object sender, EventArgs e)
        {
            Loadbtn_ThemNguoiDaiDien();
        }
        protected void drlLoaiKieuNai_TiepDan_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListLoai = (DropDownList)sender;
            if (DropDownListLoai.SelectedValue == "")
            {
                LoadLoaiDonThu(0, true, drlLoaiTiepDan, drlLoaiKieuNai, drlLoaiChiTiet, "tiếp dân");
            }
            else
            {
                LoadLoaiDonThu(Int32.Parse(DropDownListLoai.SelectedValue), true, drlLoaiTiepDan, drlLoaiKieuNai, drlLoaiChiTiet, "tiếp dân");
            }
            DropDownListLoai.Focus();


        }
        public void LoadLoaiDonThu(int vLoaiDonThu_ID, bool IsSelectedIndexChanged, DropDownList pdrlLoaiTiepDan, DropDownList pdrlLoaiKieuNai, DropDownList pdrlLoaiChiTiet, string pName)
        {
            lOAIDONTHUs = vDataContext.LOAIDONTHUs.ToList();
            LOAIDONTHU objLOAIDONTHU = new LOAIDONTHU();
            if (vLoaiDonThu_ID > 0)
            {
                objLOAIDONTHU = lOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == vLoaiDonThu_ID).FirstOrDefault();

                if (objLOAIDONTHU != null)
                {

                    if (objLOAIDONTHU.LOAIDONTHU_CAP == 3)
                    {
                        List<LOAIDONTHU> objLOAIDONTHUCap3List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).ToList();
                        LOAIDONTHU objLOAIDONTHUCap2 = lOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();

                        if (objLOAIDONTHUCap2 != null)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap2List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID).ToList();
                            LOAIDONTHU objLOAIDONTHUCap1 = lOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();


                            pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                            pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiTiepDan.DataBind();
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại " + pName, ""));
                            pdrlLoaiTiepDan.SelectedValue = objLOAIDONTHUCap2.LOAIDONTHU_CHA_ID.ToString();


                            pdrlLoaiKieuNai.DataSource = objLOAIDONTHUCap2List;
                            pdrlLoaiKieuNai.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiKieuNai.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiKieuNai.DataBind();
                            pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHUCap1.LOAIDONTHU_TEN, ""));
                            pdrlLoaiKieuNai.SelectedValue = objLOAIDONTHUCap2.LOAIDONTHU_ID.ToString();


                            pdrlLoaiChiTiet.DataSource = objLOAIDONTHUCap3List;
                            pdrlLoaiChiTiet.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiChiTiet.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiChiTiet.DataBind();
                            pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                            pdrlLoaiChiTiet.SelectedValue = vLoaiDonThu_ID.ToString();
                        }
                    }
                    else if (objLOAIDONTHU.LOAIDONTHU_CAP == 2)
                    {

                        if (!IsSelectedIndexChanged)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap2List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).ToList();
                            LOAIDONTHU objLOAIDONTHUCap1 = lOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                            pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                            pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiTiepDan.DataBind();
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại " + pName, ""));
                            pdrlLoaiTiepDan.SelectedValue = objLOAIDONTHU.LOAIDONTHU_CHA_ID.ToString();

                            pdrlLoaiKieuNai.DataSource = objLOAIDONTHUCap2List;
                            pdrlLoaiKieuNai.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiKieuNai.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiKieuNai.DataBind();
                            pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHUCap1.LOAIDONTHU_TEN, ""));
                            pdrlLoaiKieuNai.SelectedValue = vLoaiDonThu_ID.ToString();
                        }

                        List<LOAIDONTHU> objLOAIDONTHUCap3List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_ID).ToList();

                        pdrlLoaiChiTiet.DataSource = objLOAIDONTHUCap3List;
                        pdrlLoaiChiTiet.DataTextField = "LOAIDONTHU_TEN";
                        pdrlLoaiChiTiet.DataValueField = "LOAIDONTHU_ID";
                        pdrlLoaiChiTiet.DataBind();
                        pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        pdrlLoaiChiTiet.SelectedValue = "";
                    }
                    else if (objLOAIDONTHU.LOAIDONTHU_CAP == 1)
                    {
                        if (!IsSelectedIndexChanged)
                        {

                            List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                            pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                            pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiTiepDan.DataBind();
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại " + pName, ""));
                            pdrlLoaiTiepDan.SelectedValue = objLOAIDONTHU.LOAIDONTHU_ID.ToString();
                        }

                        List<LOAIDONTHU> objLOAIDONTHUCap2List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_ID).ToList();
                        pdrlLoaiKieuNai.DataSource = objLOAIDONTHUCap2List;
                        pdrlLoaiKieuNai.DataTextField = "LOAIDONTHU_TEN";
                        pdrlLoaiKieuNai.DataValueField = "LOAIDONTHU_ID";
                        pdrlLoaiKieuNai.DataBind();
                        pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại " + objLOAIDONTHU.LOAIDONTHU_TEN, ""));
                        pdrlLoaiKieuNai.SelectedValue = "";

                        pdrlLoaiChiTiet.Items.Clear();
                        pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        pdrlLoaiChiTiet.SelectedValue = "";
                    }
                }
            }
            else
            {
                if (!IsSelectedIndexChanged)
                {
                    List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                    pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                    pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                    pdrlLoaiTiepDan.DataBind();
                    pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại " + pName, ""));
                    pdrlLoaiTiepDan.SelectedValue = "";

                    pdrlLoaiKieuNai.Items.Clear();
                    pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại", ""));
                    pdrlLoaiKieuNai.SelectedValue = "";

                    pdrlLoaiChiTiet.Items.Clear();
                    pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                    pdrlLoaiChiTiet.SelectedValue = "";
                }
                else
                {
                    if (drlLoaiTiepDan.SelectedValue != "")
                    {
                        pdrlLoaiChiTiet.Items.Clear();
                        pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        pdrlLoaiChiTiet.SelectedValue = "";
                    }
                    else
                    {
                        pdrlLoaiKieuNai.Items.Clear();
                        pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại", ""));
                        pdrlLoaiKieuNai.SelectedValue = "";

                        pdrlLoaiChiTiet.Items.Clear();
                        pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                        pdrlLoaiChiTiet.SelectedValue = "";
                    }
                }


            }
        }
        public string GetLoaiDonThu()
        {
            if (rdoCoDon.Checked)
            {
                if (ddlistLoaiKhieuNaiChiTiet.SelectedValue != "" && ddlistLoaiKhieuNaiChiTiet.SelectedValue != "")
                {
                    return ddlistLoaiKhieuNaiChiTiet.SelectedValue;
                }
                else if (ddlistLoaiKhieuNai.SelectedValue != "" && ddlistLoaiKhieuNai.SelectedValue != "")
                {
                    return ddlistLoaiKhieuNai.SelectedValue;
                }
                else if (ddlistLoaDonThu.SelectedValue != "" && ddlistLoaDonThu.SelectedValue != "")
                {
                    return ddlistLoaDonThu.SelectedValue;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (drlLoaiChiTiet.SelectedValue != "" && drlLoaiChiTiet.SelectedValue != "")
                {
                    return drlLoaiChiTiet.SelectedValue;
                }
                else if (drlLoaiKieuNai.SelectedValue != "" && drlLoaiKieuNai.SelectedValue != "")
                {
                    return drlLoaiKieuNai.SelectedValue;
                }
                else if (drlLoaiTiepDan.SelectedValue != "" && drlLoaiTiepDan.SelectedValue != "")
                {
                    return drlLoaiTiepDan.SelectedValue;
                }
                else
                {
                    return null;
                }
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



                if (vSearchOption == "")
                {
                    vSearchOption = "|DOITUONG.DOITUONG_ID,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|DOITUONG.DOITUONG_DIACHI,normal,,";
                }
                if (vDP_ID != 0)
                {
                    DIAPHUONG objDIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                    if (objDIAPHUONG != null)
                    {
                        vSearchOption = vSearchOption + "|DIAPHUONG.INDEX_ID,equal,like '" + objDIAPHUONG.INDEX_ID + "%' ,";
                    }
                }

                DataSet ds = objCommonController.GetPage(PortalId, ModuleId, "DoiTuong_GetPage", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                TiepDanController vTiepDanController = new TiepDanController();
                List<TIEPDAN_DOITUONG> vTIEPDANs = vTiepDanController.getList(vContentSearch);

                //DataSet ds = new DataSet();
                //// trường hợp có đơn 
                //bool vFlag = true;
                //if (vFlag == true)
                //{
                //    if (vSearchOption == "")
                //    {
                //        vSearchOption = "|DONTHU.DONTHU_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|DONTHU.DONTHU_NOIDUNG,normal,,";
                //    }
                //    if (vDP_ID != 0)
                //    {
                //        DIAPHUONG objDIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                //        if (objDIAPHUONG != null)
                //        {
                //            vSearchOption = vSearchOption + "|DIAPHUONG.INDEX_ID,equal,like '" + objDIAPHUONG.INDEX_ID + "%' ,";
                //        }
                //    }
                //    ds = objCommonController.GetPage(PortalId, ModuleId, "DonThu_GetPage_Popup", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                //    dgDanhSach.DataKeyField = "DONTHU_ID";
                //    // Tiếp dân
                //    dgDanhSach.Columns[3].Visible = false;
                //    dgDanhSach.Columns[7].Visible = false;
                //    dgDanhSach.Columns[9].Visible = false;

                //    dgDanhSach.Columns[11].Visible = false;
                //    dgDanhSach.Columns[12].Visible = false;
                //    dgDanhSach.Columns[15].Visible = false;
                //    // Đơn thư
                //    dgDanhSach.Columns[4].Visible = false;
                //    dgDanhSach.Columns[8].Visible = false;
                //    dgDanhSach.Columns[10].Visible = false;
                //    dgDanhSach.Columns[13].Visible = false;
                //    dgDanhSach.Columns[14].Visible = false;
                //    dgDanhSach.Columns[16].Visible = false;
                //    //dgDanhSach.Columns[9].Visible = true;
                //    //dgDanhSach.Columns[10].Visible = true;
                //    //dgDanhSach.Columns[11].Visible = true;
                //}
                //// trường hợp không có đơn có đơn 
                //else
                //{
                //    if (vSearchOption == "")
                //    {
                //        vSearchOption = "|TIEPDAN.TIEPDAN_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|TIEPDAN.TIEPDAN_NOIDUNG,normal,,";
                //    }
                //    if (vDP_ID != 0)
                //    {
                //        DIAPHUONG objDIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDP_ID).FirstOrDefault();
                //        if (objDIAPHUONG != null)
                //        {
                //            vSearchOption = vSearchOption + "|DIAPHUONG.INDEX_ID,equal,like '" + objDIAPHUONG.INDEX_ID + "%' ,";
                //        }
                //    }
                //    ds = objCommonController.GetPage(PortalId, ModuleId, "TiepDan_GetPage_Popup", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                //    dgDanhSach.DataKeyField = "TIEPDAN_ID";

                //    // Tiếp dân
                //    dgDanhSach.Columns[3].Visible = true;
                //    dgDanhSach.Columns[7].Visible = false;
                //    dgDanhSach.Columns[9].Visible = false;
                //    dgDanhSach.Columns[11].Visible = false;
                //    dgDanhSach.Columns[12].Visible = false;
                //    dgDanhSach.Columns[15].Visible = false;
                //    // Đơn thư
                //    dgDanhSach.Columns[4].Visible = false;
                //    dgDanhSach.Columns[8].Visible = false;
                //    dgDanhSach.Columns[10].Visible = false;
                //    dgDanhSach.Columns[13].Visible = false;
                //    dgDanhSach.Columns[14].Visible = false;
                //    dgDanhSach.Columns[16].Visible = false;
                //    //dgDanhSach.Columns[9].Visible = false;
                //    //dgDanhSach.Columns[10].Visible = false;
                //    //dgDanhSach.Columns[11].Visible = false;
                //}
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

                var vDoiTuongInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == vID).FirstOrDefault();
                
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

                //if (btn.CommandArgument == "ChonDoiTuong")
                //{
                //    //hdfIsCoppy.Value = "false";
                //    //txtLanTiep.Enabled = true;
                //    //txtLanTiep.Text = (Int32.Parse(txtLanTiep.Text) + 1).ToString();
                //    //txtNoiDungTiepDan.Enabled = true;
                //    //txtKetQua.Enabled = true;
                //    //btnChonNguoiDaiDien.Visible = true;
                //    //txtNgayTiepDan.Enabled = true;
                //    SetEnableForm(false);
                //}
                //else
                //{
                //    hdfIsCoppy.Value = "true";
                //    SetEnableForm(true);
                //}

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "hideModal()", true);

            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }



        #endregion
        protected void bsdrpDOITUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModelScript", "alert('AAAAA');", true);
            updatePN.Update();

        }
        protected void rdoKhongDon_ServerChange(object sender, EventArgs e)
        {

        }
        protected void rdoDon_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCoDon.Checked)
            {
            //    FilterTiepDan.Visible = false;
            //    FilterDonThu.Visible = true;
                contentTiepDan.Visible = false;
                contentDonThu.Visible = true;
                if (ddlistLoaDonThu.Items.Count <= 1)
                {
                    LoadLoaiDonThu(0, false, ddlistLoaDonThu, ddlistLoaiKhieuNai, ddlistLoaiKhieuNaiChiTiet, "đơn thư");
                }

                LoadDanhSachDonVi();
                LoadDanhSachCanBo();
                LoadDiaPhuong(-1, ddlistXaPhuong, ddlistQuanHuyen, ddlistTinhThanh);
                LoadDanToc(-1, ddlistDanToc);
                LoadQuocTich(-1, ddlistQuocTich);

            }
            else
            {
                //FilterTiepDan.Visible = true;
                //FilterDonThu.Visible = false;
                contentTiepDan.Visible = true;
                contentDonThu.Visible = false;
            }
            LoadDanhSach(1, vPageSize);
        }
        #region ĐƠN THU
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
        protected void drlLoaiKieuNai_DonThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DropDownListLoai = (DropDownList)sender;
            if (DropDownListLoai.SelectedValue == "")
            {
                LoadLoaiDonThu(0, true, ddlistLoaDonThu, ddlistLoaiKhieuNai, ddlistLoaiKhieuNaiChiTiet, "đơn thư");
            }
            else
            {
                LoadLoaiDonThu(Int32.Parse(DropDownListLoai.SelectedValue), true, ddlistLoaDonThu, ddlistLoaiKhieuNai, ddlistLoaiKhieuNaiChiTiet, "đơn thư");
            }
            // Load hướng xử lý theo loại đơn thư
            if (DropDownListLoai.SelectedValue == ClassParameter.vPAKN_ID.ToString() || DropDownListLoai.SelectedValue == ClassParameter.vToCao_ID.ToString() || DropDownListLoai.SelectedValue == ClassParameter.vKhieuNai_ID.ToString() || DropDownListLoai.SelectedValue == ClassParameter.vNhieuNoiDung_ID.ToString())
            {
                loadHuongXuLy(Convert.ToInt32(DropDownListLoai.SelectedValue));
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
                LoadDiaPhuong(-1, ddlistXaPhuongNguoiDaiDienUyQuyen, ddlistQuanHuyenNguoiDaiDienUyQuyen, ddlistTinhThanhNguoiDaiDienUyQuyen);
                LoadQuocTich(-1, ddlistQuocTichNguoiDaiDienUyQuyen);
            }
            catch (Exception ex)
            {
            }
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
                        ddlistHuongXuLy.SelectedValue == "10" ||
                        ddlistHuongXuLy.SelectedValue == "12")
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
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

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

        // <summary>
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

        protected void ddlistNguoiDuyet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlistNguoiDuyet.SelectedValue))
                {
                    var vCanBoDuyet = vDataContext.CANBOs.Where(x => x.CANBO_ID == int.Parse(ddlistNguoiDuyet.SelectedValue)).Select(x => x.CHUCVU).FirstOrDefault();
                    if (vCanBoDuyet != null)
                    {
                        textChucVu_HuongXuLy.Text = vCanBoDuyet.TENCHUCVU;
                    }

                }
                else
                {
                    textChucVu_HuongXuLy.Text = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

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
                        int vDonThuId;
                        if (vTIEPDAN_ID == 0)
                        {
                            vDonThuId = 0;
                        }
                        else
                        {
                            TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                            if (objTiepDan.DONTHU_ID != null)
                            {
                                vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                            }
                            else
                            {
                                vDonThuId = 0;
                            }
                        }
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
        public void LoadDanhSachHoSoDonThu(long pDonThuId)
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
                int vDonThuId;
                TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                if (objTiepDan.DONTHU_ID != null)
                {
                    vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                }
                else
                {
                    vDonThuId = 0;
                }
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
                        int vDonThuId;
                        if (vTIEPDAN_ID == 0)
                        {
                            vDonThuId = 0;
                        }
                        else
                        {
                            TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                            if (objTiepDan.DONTHU_ID != null)
                            {
                                vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                            }
                            else
                            {
                                vDonThuId = 0;
                            }
                        }
                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        HttpPostedFile userPostedFile = uploadedFiles[1];
                        //Thứ tự control file từ trên xuống
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
        public void LoadDanhSachHoSoHuongXuLy(long pDonThuId)
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
                int vDonThuId;
                TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                if (objTiepDan.DONTHU_ID != null)
                {
                    vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                }
                else
                {
                    vDonThuId = 0;
                }

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
                        int vDonThuId;
                        if (vTIEPDAN_ID == 0)
                        {
                            vDonThuId = 0;
                        }
                        else
                        {
                            TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                            if (objTiepDan.DONTHU_ID != null)
                            {
                                vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                            }
                            else
                            {
                                vDonThuId = 0;
                            }
                        }
                        string filepath = Server.MapPath(vPathCommonUploadHoSo);
                        HttpFileCollection uploadedFiles = Request.Files;
                        List<HOSO> vHoSoInfos = new List<HOSO>();
                        HttpPostedFile userPostedFile = uploadedFiles[2];
                        //Thứ tự control file từ trên xuống
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
        public void LoadDanhSachHoSoNguoiDaiDienUyQuyen(long pDonThuId)
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
                int vDonThuId;
                TIEPDAN objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == vTIEPDAN_ID).FirstOrDefault();
                if (objTiepDan.DONTHU_ID != null)
                {
                    vDonThuId = int.Parse(objTiepDan.DONTHU_ID.ToString());
                }
                else
                {
                    vDonThuId = 0;
                }
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
        public void EnableSoNguoi(int TD_ID)
        {
            if (TD_ID == 0)
            {
                txtSoNguoi.Text = "1";
                txtSoNguoi.Enabled = false;
            }
        }
        public void loadHuongXuLy(int vLoaiDonThuID)
        {
            // Khiếu nại ID = 1
            // Tố cáo  ID = 2004
            // Kiến nghị phản ánh ID =4035
            // Đơn có nhiều nội dung 4051         
            ddlistHuongXuLy.Items.Clear();
            if (vLoaiDonThuID == ClassParameter.vKhieuNai_ID)
            {
                ddlistHuongXuLy.Items.Insert(0, new ListItem("Chọn hướng xử lý", ""));
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Hướng dẫn", "2"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Chuyển đơn", "3"));
                // Chưa có 
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra thông báo thụ lý", "5"));

                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra văn bản đôn đốc", "4"));

                // Chưa có 
                ddlistHuongXuLy.Items.Insert(6, new ListItem("Ra công văn giao đơn vị xử lý", "12"));

                ddlistHuongXuLy.Items.Insert(7, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(8, new ListItem("Từ chối thụ lý", "8"));
                ddlistHuongXuLy.Items.Insert(9, new ListItem("Trả đơn", "7"));
            }
            else if (vLoaiDonThuID == ClassParameter.vToCao_ID)
            {
                ddlistHuongXuLy.Items.Insert(0, new ListItem("Chọn hướng xử lý", ""));
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Chuyển đơn", "3"));

                // Chưa có 
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Ra thông báo thụ lý", "5"));

                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra văn bản đôn đốc", "4"));

                // Chưa có 
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra công văn giao đơn vị xử lý", "12"));
                ddlistHuongXuLy.Items.Insert(6, new ListItem("Trả đơn", "7"));
            }
            else if (vLoaiDonThuID == ClassParameter.vPAKN_ID)
            {
                ddlistHuongXuLy.Items.Insert(0, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Chọn hướng xử lý", ""));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Chuyển đơn", "3"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Hướng dẫn - không có văn bản", "9"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Trả đơn", "7"));
            }
            else if (vLoaiDonThuID == ClassParameter.vNhieuNoiDung_ID)
            {
                ddlistHuongXuLy.Items.Insert(0, new ListItem("Chọn hướng xử lý", ""));
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Chuyển đơn", "3"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Hướng dẫn", "2"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Trả đơn", "7"));
            }
            else
            {
                ddlistHuongXuLy.Items.Insert(0, new ListItem("Chọn hướng xử lý", ""));
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Hướng dẫn", "2"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Chuyển đơn", "3"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra văn bản đôn đốc", "4"));
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra thông báo thụ lý", "5"));
                ddlistHuongXuLy.Items.Insert(6, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(7, new ListItem("Trả đơn", "7"));
                ddlistHuongXuLy.Items.Insert(8, new ListItem("Từ chối thụ lý", "8"));
                ddlistHuongXuLy.Items.Insert(9, new ListItem("Hướng dẫn - không có văn bản", "9"));
                ddlistHuongXuLy.Items.Insert(10, new ListItem("Ra công văn giao đơn vị xử lý", "12"));
                ddlistHuongXuLy.Items.Insert(11, new ListItem("Khác", "10"));
            }
        }
        #endregion    

        protected void btnNhanBan_Click(object sender, EventArgs e)
        {
            TiepDanController tiepDanController = new TiepDanController();
            long result = tiepDanController.NhanBan(vTIEPDAN_ID);
            if (result != 0)
            {
                Session[vMacAddress + TabId.ToString() + "_Message"] = "Nhân bản tiếp dân thành công";
                Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin tiếp dân", "id=" + result.ToString());
                Response.Redirect(vUrl);
            }
            else
            {
                ClassCommon.ShowToastr(Page, "Nhân bản tiếp dân không thành công", "Thông báo", "error");
            }
        }
    }
}
