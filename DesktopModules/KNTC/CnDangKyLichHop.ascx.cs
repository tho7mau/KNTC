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
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CnDangKyLichHop : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        DuyetPhienHopController vDuyetPhienHopController = new DuyetPhienHopController();
        PhongHopController vPhongHopControllerInfo = new PhongHopController();

        string vMacAddress = ClassCommon.GetMacAddress();

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
                    var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                    dtpickerThoiGianBatDau.SelectedDate = vPhienHopInfo.THOIGIANBATDAU;
                    dtpickerThoiGianKetThuc.SelectedDate = vPhienHopInfo.THOIGIANKETTHUC;
                    SetFormInfo(vPhienHopId);
                    LoadDropDownThietBi();
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
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / Cập nhật thông tin / " + vPhienHopInfo.TIEUDE;
                    }

                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
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
            Boolean vResult_Trung = true;
            string vToastrMessage = "";
            string oErrorMessage = "";
            string vToastrMessage_Trung = "";

            if (textSoNguoi.Text == "")
            {
                textSoNguoi.CssClass += " vld";
                textSoNguoi.Focus();
                labelSoNguoi.Attributes["class"] += " vld";
                vToastrMessage += "Số người , ";
                vResult = false;
            }
            else
            {
                textSoNguoi.CssClass = textSoNguoi.CssClass.Replace("vld", "").Trim();
                labelSoNguoi.Attributes.Add("class", labelSoNguoi.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vResult == false && vResult_Trung == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Vui lòng nhập " + vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ". <br/>" + vToastrMessage_Trung, "Thông báo", "error");
            }
            else
            {
                if (vResult == false && vResult_Trung == true)
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Vui lòng nhập " + vToastrMessage.Substring(0, vToastrMessage.Length - 2) + "." + "Thông báo", "error");
                }
                else
                {
                    if (vResult == true && vResult_Trung == false)
                    {
                        ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessage_Trung, "Thông báo", "error");
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
                var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).FirstOrDefault();
                if (vPhienHopInfo != null)
                {
                    textSoNguoi.Text = vDuyetPhienHopController.GetKhachThamDu(pPhienHopId).ToString();
                    dtpickerThoiGianBatDau.SelectedDate = vPhienHopInfo.THOIGIANBATDAU;
                    dtpickerThoiGianKetThuc.SelectedDate = vPhienHopInfo.THOIGIANKETTHUC;
                    var objPhienHop_PhongHop = vDataContext.PHIENHOP_PHONGHOPs.Where(x => x.PHIENHOP_ID == pPhienHopId).FirstOrDefault();
                    if (objPhienHop_PhongHop != null)
                    {
                        pnlPhongDaChon.Visible = true;
                        textTenPhongHop.Text = objPhienHop_PhongHop.PHONGHOP.TENPHONGHOP;
                        textSucChua.Text = objPhienHop_PhongHop.PHONGHOP.SUCCHUA.ToString();
                        textThietBi.Text = vPhongHopControllerInfo.GetDanhSachTenThietBi(objPhienHop_PhongHop.PHONGHOP_ID);
                    }
                    else
                    {
                        pnlPhongDaChon.Visible = false;
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
                if (pPhienHopId == 0)//Thêm mới phiên họp
                {
                    PHIENHOP vPhienHopInfo = new PHIENHOP();

                    vPhienHopInfo.UserId_TAO = _currentUser.UserID;
                    vPhienHopInfo.NGAYTAO = DateTime.Now;
                    vPhienHopInfo.UserId_CAPNHAT = _currentUser.UserID;
                    vPhienHopInfo.NGAYCAPNHAT = DateTime.Now;
                    int oPhienHopId = 0;
                    vPhienHopControllerInfo.ThemMoiPhienHop(vPhienHopInfo, out oPhienHopId, out vErrorMessage);
                    if (oPhienHopId > 0)
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Tạo phiên họp thành công!  Vui lòng cập nhật thông tin cho phiên họp";
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


        #endregion
        #region dropdown thiết bị
        protected void ddlistThietBi_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {

        }
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
        #endregion       
        protected void buttonTimKiem_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == true)
            {
                //CapNhat(vPhienHopId);
                TimPhongHop();
            }
        }
        public void TimPhongHop()
        {
            try
            {
                List<PHONGHOP> lstPHONGHOP = new List<PHONGHOP>();
                List<HKG_PHONGHOP_THIETBIResult> hKG_PHONGHOP_s = new List<HKG_PHONGHOP_THIETBIResult>();
                string vThietBiID = "";
                int countTB = 0;
                int SucChua = Convert.ToInt32(textSoNguoi.Text);
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
                //  Trường hợp có chọn thiết bị
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
                // Phòng hợp thỏa điều kiện thiết bị và sức chứa ==> tìm theo thời gian 
                List<int> vPhongHopIds_DaCoLich = new List<int>();
                if (lstPHONGHOP.Count > 0)
                {
                    List<HKG_PHONGHOP_PHIENHOPResult> ListPH = vDataContext.HKG_PHONGHOP_PHIENHOP(dtpickerThoiGianBatDau.SelectedDate, dtpickerThoiGianKetThuc.SelectedDate).ToList();
                    if (ListPH.Count > 0)
                    {
                        //lstPHONGHOP = lstPHONGHOP.Where(x => !ListPH.Select(z => z.PHONGHOP_ID).Contains(x.PHONGHOP_ID)).ToList();
                        vPhongHopIds_DaCoLich = lstPHONGHOP.Where(x => ListPH.Select(z => z.PHONGHOP_ID).Contains(x.PHONGHOP_ID)).Select(x => x.PHONGHOP_ID).ToList();
                    }
                }

                DataTable vDataTableInfo = new DataTable();
                vDataTableInfo.Columns.Add("PHONGHOP_ID");
                vDataTableInfo.Columns.Add("SUCCHUA");
                vDataTableInfo.Columns.Add("TENPHONGHOP");
                vDataTableInfo.Columns.Add("DANGKY");
                vDataTableInfo.Columns.Add("LICHHOP");
                var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                if (lstPHONGHOP.Count > 0)
                {
                    pnlThongBao.Visible = false;
                    pnlKetQuaTimKiem.Visible = true;
                    foreach (var vPhongHopInfo in lstPHONGHOP)
                    {
                        DataRow row = vDataTableInfo.NewRow();
                        row["PHONGHOP_ID"] = vPhongHopInfo.PHONGHOP_ID;
                        row["SUCCHUA"] = vPhongHopInfo.SUCCHUA;
                        row["TENPHONGHOP"] = vPhongHopInfo.TENPHONGHOP;

                        if (vPhongHopIds_DaCoLich.Contains(vPhongHopInfo.PHONGHOP_ID))// && ((vPhienHopInfo.THOIGIANBATDAU <= dtpickerThoiGianBatDau.SelectedDate && dtpickerThoiGianBatDau.SelectedDate >= vPhienHopInfo.THOIGIANBATDAU)))
                        {
                            row["DANGKY"] = "visibility: hidden";

                            var vPhienHopInfos = (from PhienHop_PhongHop in vDataContext.PHIENHOP_PHONGHOPs
                                                  join PhienHop in vDataContext.PHIENHOPs on PhienHop_PhongHop.PHONGHOP_ID equals PhienHop.PHONGHOP_ID
                                                  where PhienHop_PhongHop.PHONGHOP_ID == vPhongHopInfo.PHONGHOP_ID && ((dtpickerThoiGianBatDau.SelectedDate <= PhienHop.THOIGIANBATDAU && dtpickerThoiGianKetThuc.SelectedDate >= PhienHop.THOIGIANBATDAU) || (dtpickerThoiGianBatDau.SelectedDate <= PhienHop.THOIGIANKETTHUC && dtpickerThoiGianKetThuc.SelectedDate >= PhienHop.THOIGIANKETTHUC)) && PhienHop.TRANGTHAI == 2
                                                  select PhienHop).Distinct().ToList();
                            if (vPhienHopInfos.Count > 0)
                            {
                                string vLichHopTiepTheo = "";
                                foreach (var vPhienHop in vPhienHopInfos)
                                {
                                    vLichHopTiepTheo += "• " + vPhienHop.TIEUDE + " <span style='color:red;'>(" + String.Format("{0:hh:mm dd/MM}", vPhienHop.THOIGIANBATDAU) + " - " + String.Format("{0: hh:mm dd/MM}", vPhienHop.THOIGIANKETTHUC) + ")</span><br/>";
                                }
                                row["LICHHOP"] = vLichHopTiepTheo;
                            }
                        }
                        else
                            row["DANGKY"] = "";

                        if (((dtpickerThoiGianBatDau.SelectedDate < vPhienHopInfo.THOIGIANBATDAU || dtpickerThoiGianBatDau.SelectedDate > vPhienHopInfo.THOIGIANKETTHUC)
                             || (dtpickerThoiGianKetThuc.SelectedDate < vPhienHopInfo.THOIGIANBATDAU || dtpickerThoiGianKetThuc.SelectedDate > vPhienHopInfo.THOIGIANKETTHUC)))
                        {
                            row["DANGKY"] = "visibility: hidden";
                            row["LICHHOP"] = "Phòng trống - cập nhật thời gian họp để đăng ký phòng";
                        }

                        vDataTableInfo.Rows.Add(row);
                    }
                    //ListView_PHONG.DataSource = lstPHONGHOP;
                    ListView_PHONG.DataSource = vDataTableInfo;
                    ListView_PHONG.DataBind();
                }
                else
                {
                    pnlThongBao.Visible = true;
                    lblThongBao.Text = "Không tìm thấy phòng hợp nào phù hợp yêu cầu.";
                    pnlKetQuaTimKiem.Visible = false;
                }

            }
            catch (Exception ex)
            {

            }
        }
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
        /// Đăng ký phòng cho phiên họp          
        protected void DangKyPhong(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vPhongID = html.HRef;

            var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
            if (vPhienHopInfo != null)
            {
                vPhienHopInfo.PHONGHOP_ID = int.Parse(vPhongID);
                vDataContext.SubmitChanges();
            }

            var vPhongHopInfo = vPhongHopControllerInfo.GetPhongHopTheoId(int.Parse(vPhongID));

            vDuyetPhienHopController.XoaPhongDaChon(vPhienHopId);
            PHIENHOP_PHONGHOP pHIENHOP_PHONGHOP = new PHIENHOP_PHONGHOP();
            pHIENHOP_PHONGHOP.PHIENHOP_ID = vPhienHopId;
            pHIENHOP_PHONGHOP.PHONGHOP_ID = Convert.ToInt32(vPhongID);
            pHIENHOP_PHONGHOP.SODO_FILE = vPhongHopInfo.SODO_FILE;
            pHIENHOP_PHONGHOP.SODO_Text = vPhongHopInfo.SODO_Text; ;
            vDataContext.PHIENHOP_PHONGHOPs.InsertOnSubmit(pHIENHOP_PHONGHOP);
            vDataContext.SubmitChanges();
            //Session[vMacAddress + TabId.ToString() + "_Message"] = "Đăng ký phòng thành công!";
            //Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
            ClassCommon.ShowToastr(Page, "Đăng ký phòng thành công!", "Thông báo", "success");
            SetFormInfo(vPhienHopId);
            TimPhongHop();
            //Response.Redirect(Globals.NavigateURL());

        }

        /// <summary>
        /// Xóa phòng đã chọn    
        protected void XoaPhong(object sender, EventArgs e)
        {
            vDuyetPhienHopController.XoaPhongDaChon(vPhienHopId);
            ClassCommon.ShowToastr(Page, "Xóa phòng đã chọn cho phiên họp thành công!", "Thông báo", "success");
            var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
            if (vPhienHopInfo != null)
            {
                vPhienHopInfo.PHONGHOP_ID = null;
                vDataContext.SubmitChanges();
            }
            SetFormInfo(vPhienHopId);
            TimPhongHop();
        }
    }
}
