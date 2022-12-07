#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật thiết bị
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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class CnDoiTuong : DotNetNuke.Entities.Modules.UserModuleBase
    {
        // thiss is command
	// thiss is command
        #region Properties
        int vDOITUONG_ID;
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
                    vDOITUONG_ID = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vDOITUONG_ID, false);
                   
                }
            }
            catch (Exception ex)
            {
                //ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
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
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
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
                CapNhat(vDOITUONG_ID);
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
        /// <param name="pDOITUONG_ID"></param>
        public void SetFormInfo(int pDOITUONG_ID, bool IsChonDoiTuong)
        {
            try
            {
                string TitleBreadcrumb = "";
                if (pDOITUONG_ID == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                    TiepDanCardTool.Visible = false;
                    DonThuCardTool.Visible = false;
                    List<CANHAN> cANHANs = new List<CANHAN>();
                    CANHAN cANHAN = new CANHAN();
                    cANHANs.Add(cANHAN);
                    cANHAN.CANHAN_ID = 0;
                    cANHAN.CANHAN_GIOITINH = false;
                    ListViewDoiTuong.DataSource = cANHANs;
                    ListViewDoiTuong.DataBind();
                    TitleBreadcrumb = TitleBreadcrumb + "Thêm mới tiếp dân";
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
                    int MaxSTT = Int32.Parse(vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT).ToString());
                    lblSTT.Text = (MaxSTT + 1) + "";
                    lblNgayTiepDan.Text = "Ngày " + DateTime.Now.ToString("dd/MM/yyyy").ToString();
                    
                    
                    Loadbtn_ThemNguoiDaiDien();
                }
                else
                {
                    DOITUONG dOITUONG = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == pDOITUONG_ID).FirstOrDefault(); 
                   
                    if (dOITUONG != null)
                    {
                        txtSoNguoi.Text = dOITUONG.DOITUONG_SONGUOI.ToString();
                        
                        hdfDoiTuongID.Value = dOITUONG.DOITUONG_ID.ToString();
                        drpDOITUONG.SelectedValue = dOITUONG.DOITUONG_LOAI.ToString();
                        txtTenCoQuanToChuc.Text = dOITUONG.DOITUONG_TEN;
                        txtDiaChiDoiTuong.Text = dOITUONG.DOITUONG_DIACHI;
                        drpDOITUONG_SelectedIndexChanged(drpDOITUONG, null);
                       
                        ListViewDoiTuong.DataSource = dOITUONG.CANHANs;
                        ListViewDoiTuong.DataBind();
                        for (int i = 0; i < dOITUONG.CANHANs.Count(); i++)
                        {

                            TitleBreadcrumb = TitleBreadcrumb + " " + dOITUONG.CANHANs[i].CANHAN_HOTEN;
                            ListViewDataItem  listViewDataItem = ListViewDoiTuong.Items[i];
                            TextBox txtCaNhanID = ((TextBox)ListViewDoiTuong.Items[i].FindControl("txtCaNhanID"));
                            if(txtCaNhanID.Text!="")
                            {
                                int vCANHAN_ID = Int32.Parse(txtCaNhanID.Text);
                                CANHAN cANHAN = dOITUONG.CANHANs.Where(x => x.CANHAN_ID == vCANHAN_ID).FirstOrDefault();
                                if(cANHAN!=null)
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
                        lblBreadcrumbTitle.Text = TitleBreadcrumb;
                        txtSoNguoiDaiDien.Text = dOITUONG.CANHANs.Count().ToString();
                       
                        SetEnableForm(false);
                        Loadbtn_ThemNguoiDaiDien();

                        int TiepDan_Count = vDataContext.TIEPDANs.Where(x => x.DOITUONG_ID == vDOITUONG_ID).ToList().Count();
                        if (TiepDan_Count > 0)
                        {
                            lableSoLuongTiepDan.Text = TiepDan_Count.ToString();
                            TiepDanCardTool.Visible = true;
                        }
                        else
                        {
                            TiepDanCardTool.Visible = false;
                        }    
                        int DonThu_Count = vDataContext.DONTHUs.Where(x => x.DOITUONG_ID == vDOITUONG_ID).ToList().Count();
                        if (DonThu_Count > 0)
                        {
                            lableSoLuongDonThu.Text = DonThu_Count.ToString();
                            DonThuCardTool.Visible = true;
                        }
                        else
                        {
                            DonThuCardTool.Visible = false;
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
            btnChonNguoiDaiDien.Visible = pEnableStatus;
            txtSoNguoi.Enabled = pEnableStatus;
            txtSoNguoiDaiDien.Enabled = pEnableStatus;
            txtTenCoQuanToChuc.Enabled = pEnableStatus;
            txtDiaChiDoiTuong.Enabled = pEnableStatus;
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

            // don thu
           
        }
        /// <summary>
        /// Cập nhật thông tin thiết bị
        /// </summary>
        /// <param name="pDOITUONG_ID"></param>
        public void CapNhat(int pDOITUONG_ID)
        {
            try
            {
                string oErrorMessage = "";

                if (pDOITUONG_ID == 0)//Thêm mới 
                {
                    
                    int vLOAIDOITUONG = Int32.Parse(drpDOITUONG.SelectedValue);
                  
                    // Đối tượng
                    if(hdfIsCoppy.Value!= "false")
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
                            cANHAN_Insert.CANHAN_DIACHI_DAYDU = (cANHAN_Insert.CANHAN_DIACHI != "" ? (cANHAN_Insert.CANHAN_DIACHI + ", ") : "") + vDiaChi;
                            cANHAN_Insert.DOITUONG_ID = DOITUONGInfo.DOITUONG_ID;
                            cANHAN_Insert.NGAYTAO = DateTime.Now;
                            cANHAN_Insert.NGUOITAO = _currentUser.UserID;
                            vDataContext.CANHANs.InsertOnSubmit(cANHAN_Insert);
                            vDataContext.SubmitChanges();
                        }
                        if (DOITUONGInfo.DOITUONG_ID > 0)
                        {
                            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin đối tượng", "id=" + DOITUONGInfo.DOITUONG_ID);
                            Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới đối tượng thành công";
                            Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                            Response.Redirect(vUrl);
                        }
                        else
                        {
                            ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        }
                    }
                }
                else //Cập nhật thông tin đối tượng
                {
                    var DOITUONGInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == pDOITUONG_ID).SingleOrDefault();
                    
                    if (DOITUONGInfo != null)
                    {
                        // Set thông tin tiếp dân
                        int vLOAIDOITUONG = Int32.Parse(drpDOITUONG.SelectedValue);
                        
                        
                     
                        if (vLOAIDOITUONG==1)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Cá Nhân";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1 ;
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 1;
                        }   
                        else if(vLOAIDOITUONG==2)
                        {
                            DOITUONGInfo.DOITUONG_TEN = "Nhóm đông người";
                            DOITUONGInfo.DOITUONG_DIACHI = "";
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = Int32.Parse(txtSoNguoiDaiDien.Text);
                            DOITUONGInfo.DOITUONG_SONGUOI = Int32.Parse(txtSoNguoi.Text);
                            DOITUONGInfo.DOITUONG_LOAI = 2;
                        }
                        else
                        {
                            DOITUONGInfo.DOITUONG_TEN= ClassCommon.ClearHTML(txtTenCoQuanToChuc.Text.Trim());
                            DOITUONGInfo.DOITUONG_DIACHI = ClassCommon.ClearHTML(txtDiaChiDoiTuong.Text.Trim());
                            DOITUONGInfo.DOITUONG_SONGUOIDAIDIEN = 1;
                            DOITUONGInfo.DOITUONG_SONGUOI = 1;
                            DOITUONGInfo.DOITUONG_LOAI = 3;
                        }
                        DOITUONGInfo.DOITUONG_NACDANH = false;
                        DOITUONGInfo.NGAYCAPNHAT = DateTime.Now;
                        DOITUONGInfo.NGUOICAPNHAT = _currentUser.UserID;
                       
                        // Danh sách cá nhân cũ
                        List<CANHAN> cANHANs= new List<CANHAN>();
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
                            if(cANHAN_UPDATE.DP_ID>0)
                            {
                                DIAPHUONG DIAPHUONG = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == cANHAN_UPDATE.DP_ID).FirstOrDefault();
                                if(DIAPHUONG.DP_ID_CHA>0)
                                {
                                    DIAPHUONG DIAPHUONG_CAP_CHA = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG.DP_ID_CHA).FirstOrDefault();
                                    if (DIAPHUONG_CAP_CHA.DP_ID_CHA > 0)
                                    {
                                        DIAPHUONG DIAPHUONG_CAP_CHA_2 = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == DIAPHUONG_CAP_CHA.DP_ID_CHA).FirstOrDefault();
                                        vDiaChi =", "+ DIAPHUONG.DP_TEN + ", " + DIAPHUONG_CAP_CHA.DP_TEN + ", " + DIAPHUONG_CAP_CHA_2.DP_TEN;
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
                            cANHAN_UPDATE.CANHAN_DIACHI_DAYDU = (cANHAN_UPDATE.CANHAN_DIACHI != "" ? (cANHAN_UPDATE.CANHAN_DIACHI + ", ") : "") + vDiaChi;
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
                        
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin tiếp dân thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;
                        btnSua.Visible = true;
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
            catch(Exception Ex)
            {


            }
            
        }
        public CANHAN GetThongTinCaNhan (ListViewDataItem listViewDataItem_CaNhan)
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
            if(txtNgayCap.Text!="")
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
            if(pDropDownListXa.SelectedValue!="")
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

               
                if(item.DataItemIndex==0)
                {
                    HtmlAnchor html = (HtmlAnchor)e.Item.FindControl("Xoa_CaNhan");
                    html.Visible = false;
                }    

                HtmlInputRadioButton rdoNam = ((HtmlInputRadioButton)e.Item.FindControl("rdoNam"));
                HtmlGenericControl lblFoNam = ((HtmlGenericControl)e.Item.FindControl("lblforrdoNam"));

                lblFoNam.Attributes.Add("for", ListViewDoiTuong.ClientID + "_" + rdoNam.ClientID.Replace("ctrl"+ item.DataItemIndex+"_", ""));

                HtmlInputRadioButton rdoNu = ((HtmlInputRadioButton)e.Item.FindControl("rdoNu"));
                HtmlGenericControl lblFoNu = ((HtmlGenericControl)e.Item.FindControl("lblforrdoNu"));
                lblFoNu.Attributes.Add("for", ListViewDoiTuong.ClientID+"_"+ rdoNu.ClientID.Replace("ctrl" + item.DataItemIndex + "_", ""));




            }
            catch (Exception Ex)
            {

            }
        }
        public void LoadDiaPhuong(int vDP_ID, DropDownList pDropDownListXa, DropDownList pDropDownListHuyen, DropDownList pDropDownListThanhpho)
        {
            dIAPHUONGs = vDataContext.DIAPHUONGs.ToList();
            if (vDP_ID==-1)
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
                if(dIAPHUONG!=null)
                {
                    if(dIAPHUONG.CapDo==3)
                    {
                        List<DIAPHUONG> DIAPHUONGXas =  dIAPHUONGs.Where(x => x.DP_ID_CHA == dIAPHUONG.DP_ID_CHA).ToList();
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
                    else if(dIAPHUONG.CapDo == 2)
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
            if(pdrlDanToc.Items.FindByValue(vDANTOCP_ID.ToString())==null)
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
            catch(Exception Ex)
            { }

        }        
        protected void drpDOITUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loadbtn_ThemNguoiDaiDien();
            drpDOITUONG.Focus();
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
            else if (drpDOITUONG.SelectedValue == "3"  )
            {
                PanelDoiTuong.Visible = true;
                panelSoNguoiDaiDien.Visible = false;
                panelSoNguoi.Visible = false;
            }
            if(drpDOITUONG.SelectedValue == "1" || drpDOITUONG.SelectedValue == "3")
            {
                if (ListViewDoiTuong.Items.Count >= 2)
                {
                    for (int i = 1; i < ListViewDoiTuong.Items.Count; i++)
                    {
                        HtmlAnchor HtmlAnchor_Xoa_CaNhan = (HtmlAnchor)ListViewDoiTuong.Items[i].FindControl("Xoa_CaNhan");
                        string vId = HtmlAnchor_Xoa_CaNhan.HRef;
                        XoaCaNhan_by_ID(vId);
                    }
                }
            }
        }
        protected void txtSoNguoiDaiDien_TextChanged(object sender, EventArgs e)
        {
            Loadbtn_ThemNguoiDaiDien();
        }
       
        public void LoadLoaiDonThu(int vLoaiDonThu_ID, bool IsSelectedIndexChanged, DropDownList pdrlLoaiTiepDan,DropDownList pdrlLoaiKieuNai,DropDownList pdrlLoaiChiTiet,string pName)
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
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại "+ pName, ""));
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
                       
                        if(!IsSelectedIndexChanged)
                        {
                            List<LOAIDONTHU> objLOAIDONTHUCap2List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).ToList();
                            LOAIDONTHU objLOAIDONTHUCap1 = lOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objLOAIDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                            pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                            pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                            pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                            pdrlLoaiTiepDan.DataBind();
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại "+ pName, ""));
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
                            pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại "+ pName, ""));
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
                if(!IsSelectedIndexChanged)
                {
                    List<LOAIDONTHU> objLOAIDONTHUCap1List = lOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    pdrlLoaiTiepDan.DataSource = objLOAIDONTHUCap1List;
                    pdrlLoaiTiepDan.DataTextField = "LOAIDONTHU_TEN";
                    pdrlLoaiTiepDan.DataValueField = "LOAIDONTHU_ID";
                    pdrlLoaiTiepDan.DataBind();
                    pdrlLoaiTiepDan.Items.Insert(0, new ListItem("Chọn loại "+ pName, ""));
                    pdrlLoaiTiepDan.SelectedValue = "";

                    pdrlLoaiKieuNai.Items.Clear();
                    pdrlLoaiKieuNai.Items.Insert(0, new ListItem("Chọn loại", ""));
                    pdrlLoaiKieuNai.SelectedValue = "";

                    pdrlLoaiChiTiet.Items.Clear();
                    pdrlLoaiChiTiet.Items.Insert(0, new ListItem("Chọn chi tiết loại", ""));
                    pdrlLoaiChiTiet.SelectedValue = "";
                }
               
                

            }
        }
          
        protected void Xoa_CaNhan_ServerClick(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                string vId = html.HRef;
                XoaCaNhan_by_ID(vId);

            }
            catch (Exception Ex)
            {

            }
            
        }

        private void XoaCaNhan_by_ID(string vId)
        {
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

        protected void TiepDanCardTool_Click(object sender, EventArgs e)
        {
            ClassCommon common = new ClassCommon();
            string param = "&DOITUONG_ID=" + vDOITUONG_ID;
            string URL = common.GET_URL_MODULE(this.PortalId, ClassParameter.v_ModuleByDefinition_TIEPDAN,param);
            Response.Redirect(URL,true);
        }

        protected void DonThuCardTool_Click(object sender, EventArgs e)
        {
            ClassCommon common = new ClassCommon();
            string param = "&DOITUONG_ID=" + vDOITUONG_ID;
            string URL = common.GET_URL_MODULE(this.PortalId, ClassParameter.v_ModuleByDefinition_DONTHU, param);
            Response.Redirect(URL, false);
        }
    }
}
