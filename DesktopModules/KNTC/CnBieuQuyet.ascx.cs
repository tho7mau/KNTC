#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật phiên họp
/// Ngày tạo        :08/03/2020
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CnBieuQuyet : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vBQId;
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        CauHoiController vCauHoiControllerInfo = new CauHoiController();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        BieuQuyetController vBieuQuyetControllerInfo = new BieuQuyetController();

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
                    vBQId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachPhienHop();
                    SetFormInfo_DS(Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi" + vBQId]);
                    SetFormInfo(vBQId);
                    
                    //textTenKhachMoi.Focus();                   
                }
                //Edit Title
                if (vBQId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý biểu quyết</a> / Thêm mới";
                }
                else
                {
                    var vBieuQuyetInfo = vBieuQuyetControllerInfo.GetBieuQuyetTheoId(vBQId);
                    if (vBieuQuyetInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý biểu quyết</a> / " + vBieuQuyetInfo.NOIDUNGBIEUQUYET;
                    }

                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }
        /// <summary>
        /// Thêm mới biểu quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới phiên họp", "id=0");
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Set trạng thái visible form
        /// </summary>
        /// <param name="pEnableStatus"></param>
        public void SetEnableForm(bool pEnableStatus)
        {
            textNoiDung.Enabled = pEnableStatus;
            textDienGiai.Enabled = pEnableStatus;
            ddlistPhienHop.Enabled = pEnableStatus;
            dtpickerTGBatDau.Enabled = pEnableStatus;
            dtpickerTGKetThuc.Enabled = pEnableStatus;
            btn_ThemMoi.Visible = pEnableStatus;
            dgDanhSach.Columns[0].Visible = pEnableStatus;                 
        }

        /// <summary>
        /// Sửa thông tin biểu quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            SetEnableForm(true);
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
                CapNhat(vBQId);
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
            Boolean vResult_Password = true;
            string vToastrMessage = "";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";

            if (textNoiDung.Text == "")
            {
                textNoiDung.CssClass += " vld";
                textNoiDung.Focus();
                labelNoiDung.Attributes["class"] += " vld";
                vToastrMessage += "Nội dung biểu quyết, ";
                vResult = false;
            }
            else
            {
                textNoiDung.CssClass = textNoiDung.CssClass.Replace("vld", "").Trim();
                labelNoiDung.Attributes.Add("class", labelNoiDung.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (String.IsNullOrEmpty(ddlistPhienHop.SelectedValue))
            {
                ddlistPhienHop.CssClass += " vld";
                ddlistPhienHop.Focus();
                labelPhienHop.Attributes["class"] += " vld";
                vToastrMessage += "Phiên họp, ";
                vResult = false;
            }
            else
            {
                ddlistPhienHop.CssClass = ddlistPhienHop.CssClass.Replace("vld", "").Trim();
                labelPhienHop.Attributes.Add("class", labelPhienHop.Attributes["class"].ToString().Replace("vld", ""));
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
        /// <param name="pBieuQuyetId"></param>
        public void SetFormInfo(int pBieuQuyetId)
        {
            if (pBieuQuyetId > 0) // Cập nhật
            {
                var vBieuQuyetInfo = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == pBieuQuyetId).FirstOrDefault();
                if (vBieuQuyetInfo != null)
                {                    
                    SetEnableForm(false);
                    if (vBieuQuyetControllerInfo.KiemTraBieuQuyetDaThucHien(pBieuQuyetId))
                    {
                        btn_ThemMoi.Visible = false;
                    }
                    LoadDanhSach(0, pBieuQuyetId);
                    textNoiDung.Text = vBieuQuyetInfo.NOIDUNGBIEUQUYET;
                    textDienGiai.Text = vBieuQuyetInfo.DIENGIAI;                 
                    ddlistPhienHop.SelectedValue = vBieuQuyetInfo.PHIENHOP_ID.ToString();
                    if (vBieuQuyetInfo.THOIGIANBATDAU !=null)
                    {
                        dtpickerTGBatDau.SelectedDate = vBieuQuyetInfo.THOIGIANBATDAU;
                    }
                    if (vBieuQuyetInfo.THOIGIANKETTHUC != null)
                    {
                        dtpickerTGKetThuc.SelectedDate = vBieuQuyetInfo.THOIGIANKETTHUC;
                    }
                }
            }
            else
            {
                btnSua.Visible = false;
                btnCapNhat.Visible = true;
                divCauHoi.Visible = false;
                buttonThemmoi.Visible = false;
            }
        }


        /// <summary>
        /// Set thong tin Search
        /// </summary>
        /// <param name="pObjSearch"></param>
        public void SetFormInfo_DS(Object pObjSearch)
        {
            if (pObjSearch != null)
            {
                Dictionary<string, string> dictSearch = (Dictionary<string, string>)pObjSearch;
                if (ClassCommon.ExistKey("KeyWord", dictSearch))
                {
                    textSearchContent.Text = dictSearch["KeyWord"];
                }
                if (ClassCommon.ExistKey("PhienHop", dictSearch))
                {
                    ddlistPhienHop.SelectedValue = dictSearch["PhienHop"];
                }
            }
            else
            {
                //txtSearchContent.Text = "";
                //slChuyenKhoa.SelectedItem.Value = "0";
            }
        }
        public void SetEmptyForm()
        {
            textNoiDung.Text ="";
            textDienGiai.Text ="";
            ddlistPhienHop.SelectedIndex = 0;
        }
        /// <summary>
        /// Cập nhật thông tin phiên họp
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void CapNhat(int pBQId)
        {
            try
            {          
                string vErrorMessage = "";
                if (pBQId == 0)//Thêm mới biểu quyết
                {
                    BIEUQUYET vBieuQuyetInfo = new BIEUQUYET();
                    vBieuQuyetInfo.NOIDUNGBIEUQUYET = ClassCommon.ClearHTML(textNoiDung.Text.Trim());
                    vBieuQuyetInfo.DIENGIAI = ClassCommon.ClearHTML(textDienGiai.Text.Trim());
                    vBieuQuyetInfo.TRANGTHAI = 0;
                    vBieuQuyetInfo.PHIENHOP_ID = int.Parse(ddlistPhienHop.SelectedValue);
                    int oBieuQuyetId = 0;
                    vBieuQuyetInfo.THOIGIANBATDAU = dtpickerTGBatDau.SelectedDate;
                    vBieuQuyetInfo.THOIGIANKETTHUC = dtpickerTGKetThuc.SelectedDate;
                    vBieuQuyetControllerInfo.ThemMoiBieuQuyet(vBieuQuyetInfo, out oBieuQuyetId, out vErrorMessage);
                    if (oBieuQuyetId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin biểu quyết", "id=" + oBieuQuyetId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới biểu quyết thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật thông tin biểu quyết
                {
                    var vBieuQuyetInfo = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == pBQId).SingleOrDefault();
                    if (vBieuQuyetInfo != null)
                    {
                        vBieuQuyetInfo.NOIDUNGBIEUQUYET = ClassCommon.ClearHTML(textNoiDung.Text.Trim());
                        vBieuQuyetInfo.DIENGIAI = ClassCommon.ClearHTML(textDienGiai.Text.Trim());
                        vBieuQuyetInfo.TRANGTHAI = 0;
                        vBieuQuyetInfo.PHIENHOP_ID = int.Parse(ddlistPhienHop.SelectedValue);
                        vBieuQuyetInfo.THOIGIANBATDAU = dtpickerTGBatDau.SelectedDate;
                        vBieuQuyetInfo.THOIGIANKETTHUC = dtpickerTGKetThuc.SelectedDate;
                        vDataContext.SubmitChanges();
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin biểu quyết thành công", "Thông báo", "success");
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
        public void LoadDanhSachPhienHop()
        {
            try
            {
                List<PHIENHOP> vPhienHopInfos = new List<PHIENHOP>();
                vPhienHopInfos = vDataContext.PHIENHOPs.OrderByDescending(x=>x.PHIENHOP_ID).ToList();
                ddlistPhienHop.Items.Clear();
                ddlistPhienHop.DataSource = vPhienHopInfos;
                ddlistPhienHop.DataTextField = "TIEUDE";
                ddlistPhienHop.DataValueField = "PHIENHOP_ID";
                ddlistPhienHop.DataBind();
                ddlistPhienHop.Items.Insert(0, new ListItem("Chọn phiên họp", String.Empty));
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

        #endregion

        #region Câu hỏi
        /// <summary>
        /// Event Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string vSearchContent = ClassCommon.RemoveSpace(textSearchContent.Text.Trim());
            int vSearch_PhienHopID = int.Parse(ddlistPhienHop.SelectedValue);
            Dictionary<string, string> dictSearch = null;
            //Khoi tao dictionary
            if (vSearchContent != "" || vSearch_PhienHopID != 0)
            {
                dictSearch = new Dictionary<string, string>();
                dictSearch.Add("KeyWord", vSearchContent);

                dictSearch.Add("PhienHop", vSearch_PhienHopID.ToString());
            }
            if (vSearchContent == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("KeyWord");
            }
            if (vSearch_PhienHopID.ToString() == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("PhienHop");
            }
            //Gan danh sach Search into Session
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi" + vBQId] = dictSearch;
            LoadDanhSach(0, vBQId);
        }

        /// <summary>
        ///  Load danh sách co phan trang
        /// </summary>
        /// <param name="pCurentPage"></param>
        protected void LoadDanhSach(int pCurentPage, int pBieuQuyetId)
        {
            try
            {
                string vContentSearch = textSearchContent.Text.Trim();                
                string vErrorMessage = "";

                if (Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi" + vBQId] != null)
                {
                    Dictionary<string, string> vDictSearch = (Dictionary<string, string>)Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi"+ vBQId];
                    if (ClassCommon.ExistKey("KeyWord", vDictSearch))
                    {
                        vContentSearch = vDictSearch["KeyWord"].ToLower();
                    }
                }
                var demo = vDataContext.CAUHOIs.OrderByDescending(x => x.CAUHOI_ID).ToList();

                var vCauHoiInfos = vCauHoiControllerInfo.GetDanhSachCauHoi(vContentSearch, 0, pBieuQuyetId);

                if (ViewState["keysort_cauhoi"] != null && ViewState["typesort_cauhoi"] != null)
                {
                    string key = ViewState["keysort_cauhoi"].ToString();
                    string type = ViewState["typesort_cauhoi"].ToString();

                    if (key == "NOIDUNG" && type == "ASC")
                    {
                        vCauHoiInfos = vCauHoiInfos.OrderBy(x => x.NOIDUNG).ToList();
                    }

                    if (key == "NOIDUNG" && type == "DESC")
                    {
                        vCauHoiInfos = vCauHoiInfos.OrderByDescending(x => x.NOIDUNG).ToList();
                    }
                }
                int Count = vCauHoiInfos.Count;
                vCauHoiInfos = vCauHoiInfos.Skip((pCurentPage) * vPageSize).Take(vPageSize).ToList();
                dgDanhSach.DataSource = vCauHoiInfos;
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
        /// Cập nhật thông tin phiên họp (chuyển đến trang Edit)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDanhSach_Sua(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vId = html.HRef;
            string vUrl = Globals.NavigateURL("edit_cauhoi", "mid=" + this.ModuleId, "title=Cập nhật thông tin câu hỏi", "id=" + vId,"bieuquyetid="+ vBQId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi" + vBQId });
            Response.Redirect(vUrl);
        }

        /// <summary> 
        /// Session Destroy
        /// </summary>
        /// <param name="pArr"></param>
        public void SessionDestroy(string[] pArr)
        {
            for (int i = 0; i < pArr.Count(); i++)
            {
                Session.Remove(pArr[i]);
            }
        }

        /// <summary>
        /// Event them moi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit_cauhoi", "mid=" + this.ModuleId, "title=Thêm mới câu hỏi", "id=0","bieuquyetid=" + vBQId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent_CauHoi" + vBQId });
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Xóa phiên họp      
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy danh sách ID phiên họp đã chọn trên danh sách
                List<int> vCauHoiIds = new List<int>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vCauHoiIds.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                //Xóa danh sách phiên họp đã chọn
                int vCountCauHoiDaXoa = 0;
                string vErrorMessage = "";
                foreach (int vCauHoiId in vCauHoiIds)
                {
                    if (!vCauHoiControllerInfo.KiemTraCauHoiSuDung(vCauHoiId, out vErrorMessage))
                    {
                        vCauHoiControllerInfo.XoaCauHoi(vCauHoiId, out vErrorMessage);
                        vCountCauHoiDaXoa += 1;
                    }
                }
                ClassCommon.ShowToastr(Page, "Xóa thành công " + vCountCauHoiDaXoa + " Câu hỏi!", "Thông báo", "success");
                LoadDanhSach(vCurentPage, vBQId);
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }
        /// <summary>
        /// Kiểm tra câu hỏi sử dụng
        /// </summary>
        /// <param name="CauHoiID"></param>
        /// <returns></returns>
        public bool kiemtra(int CauHoiID)
        {
            string vErrorMessage = "";
            return vCauHoiControllerInfo.KiemTraCauHoiSuDung(CauHoiID, out vErrorMessage);
        }

        /// <summary>        
        /// Event Xoa phiên họp     
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        protected void dgDanhSach_Xoa(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int vCauHoiId = int.Parse(html.HRef);
            string vErrorMessage = "";
            UserInfo _User = new UserInfo();
            try
            {
                //List<PSW_XACTHUC_LENH> lstResult = nvController.GetXTLenh(vId);
                if (vCauHoiControllerInfo.KiemTraCauHoiSuDung(vCauHoiId, out vErrorMessage))//phiên họp đã phát sinh dữ liệu
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa không thành công, Câu hỏi đã phát sinh dữ liệu!", "Thông báo", "error");
                }
                else
                {
                    vCauHoiControllerInfo.XoaCauHoi(vCauHoiId, out vErrorMessage);
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa câu hỏi thành công!", "Thông báo", "success");
                    LoadDanhSach(vCurentPage, vBQId);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
                //log.Error("", ex);
            }
        }


        /// <summary>
        /// Sort
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgDanhSach_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortDirection = GetSortDirection(e.SortExpression);
            ViewState["keysort_phienhop"] = e.SortExpression;
            ViewState["typesort_phienhop"] = sortDirection;
            LoadDanhSach(0, vBQId);
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


        public bool KiemTraCauHoiDaThucHienBieuQuyet(int CauHoiID)
        {
            string vErrorMessage = "";
            return vCauHoiControllerInfo.KiemTraCauHoiSuDung(CauHoiID, out vErrorMessage);
        }
        #endregion

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
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = Int16.Parse(e.NewPageIndex.ToString());
            vCurentPage = Int16.Parse(e.NewPageIndex.ToString());
            LoadDanhSach(vCurentPage, vBQId);
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
            LoadDanhSach(dgDanhSach.PageCount - 1, vBQId);
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = (dgDanhSach.PageCount - 1);
        }
        void lbNextPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach.CurrentPageIndex < (dgDanhSach.PageCount - 1))
            {
                LoadDanhSach(dgDanhSach.CurrentPageIndex + 1, vBQId);
                Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = (dgDanhSach.CurrentPageIndex);
            }
        }
        void lbPreviousPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (dgDanhSach.CurrentPageIndex > 0)
            {
                LoadDanhSach(dgDanhSach.CurrentPageIndex - 1, vBQId);
                Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = (dgDanhSach.CurrentPageIndex);
            }
        }
        void lbFirstPage_Click(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            LoadDanhSach(0, vBQId);
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = 0;
        }
        void DdlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();

            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize_CauHoi"] = Int16.Parse(ddlPageSize.SelectedValue);
            vPageSize = Int16.Parse(ddlPageSize.SelectedValue);
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage_CauHoi"] = 0;
            vCurentPage = 0;
            LoadDanhSach(vCurentPage, vBQId);
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


    }
}
