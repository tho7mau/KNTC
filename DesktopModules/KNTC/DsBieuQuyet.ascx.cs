#region Thông tin chung
/// Mục đích        :Hiển thị danh sách phiên họp
/// Ngày tại        :03/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    /// <summary>
    /// Xử lý dữ liệu danh sách phiên họp
    /// </summary>

    public partial class DsBieuQuyet : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        NguoiDungController vNguoiDungControllerInfo = new NguoiDungController();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        BieuQuyetController vBieuQuyetControllerInfo = new BieuQuyetController();
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        string vMacAddress = ClassCommon.GetMacAddress();
        #endregion
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Kiem tra quyen dang nhap, phan quyen
                //CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Get PageSize, Current Page
                Get_Cache();
                //Edit Title
                this.ModuleConfiguration.ModuleTitle = "Quản lý biểu quyết";
                if (!IsPostBack)
                {
                    try
                    {
                        //Session.RemoveAll();
                        LoadDanhSachPhienHop();
                        SetFormInfo(Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"]);
                        LoadDanhSach(0);
                        //Load info search

                    }
                    catch (Exception ex)
                    {
                        ClassCommon.ShowToastr(Page, ex + "", "Thông báo lỗi", "error");
                        //log.Error("", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xữ lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
                //log.Error("", ex);
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
        protected void dgDanhSach_CauHoi(object sender, EventArgs e)
        {
            try{
                string vHTML_CauHoi = "";
                HtmlAnchor html = (HtmlAnchor)sender;
                int BQ_ID = Convert.ToInt32(html.HRef.ToString());
                var objCauHoi = vDataContext.CAUHOIs.Where(x => x.BIEUQUYET_ID == BQ_ID).ToList();
                if (objCauHoi.Count > 0)
                {
                    foreach (var it in objCauHoi)
                    {
                        vHTML_CauHoi += "<p style='margin-left:30px'>" + "&#9830; &nbsp;" + it.NOIDUNG + " </p>";
                    }
                }
                if (vHTML_CauHoi != "")
                {
                    literal_modal_cauhoi.Text = vHTML_CauHoi;
                }
                else
                {
                    literal_modal_cauhoi.Text = "<p style='margin-left:30px'> Chưa có câu hỏi nào cho biểu quyết này.</p>";
                }               
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Pop", "openModal();", true);
            }
            catch(Exception ex)
            {
                
            }                           
        }
        protected void dgDanhSach_KetQua(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vId = html.HRef;
            string vUrl = Globals.NavigateURL("ketqua", "mid=" + this.ModuleId, "title=Xem kết quả biểu quyết", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
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
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin biểu quyết", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
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
                List<int> vBieuQuyetIds = new List<int>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vBieuQuyetIds.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                //Xóa danh sách phiên họp đã chọn
                int vCountBieuQuyetDaXoa = 0;
                string vErrorMessage = "";
                foreach (int vBieuQuyetId in vBieuQuyetIds)
                {
                    if (!vBieuQuyetControllerInfo.KiemTraBieuQuyetDangDuocSuDung(vBieuQuyetId, out vErrorMessage))
                    {
                        vBieuQuyetControllerInfo.XoaBieuQuyet(vBieuQuyetId, out vErrorMessage);
                        vCountBieuQuyetDaXoa += 1;
                    }
                }
                ClassCommon.ShowToastr(Page, "Xóa thành công " + vCountBieuQuyetDaXoa + " biểu quyết!", "Thông báo", "success");
                LoadDanhSach(vCurentPage);
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }
      


        /// <summary>        
        /// Event Xoa phiên họp
        /// Ngô Hoài Hận
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        protected void dgDanhSach_Xoa(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int vBieuQuyetId = int.Parse(html.HRef);
            string vErrorMessage = "";
            UserInfo _User = new UserInfo();
            try
            {
                //List<PSW_XACTHUC_LENH> lstResult = nvController.GetXTLenh(vId);
                if (vBieuQuyetControllerInfo.KiemTraBieuQuyetDangDuocSuDung(vBieuQuyetId, out vErrorMessage))//phiên họp đã phát sinh dữ liệu
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa không thành công, biểu quyết đã phát sinh dữ liệu!", "Thông báo", "error");
                }
                else
                {
                    vBieuQuyetControllerInfo.XoaBieuQuyet(vBieuQuyetId, out vErrorMessage);
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa biểu quyết thành công!", "Thông báo", "success");
                    LoadDanhSach(vCurentPage);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
                //log.Error("", ex);
            }
        }

        /// <summary>
        /// Event them moi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới biểu quyết", "id=0");
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Event Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string vSearchContent = ClassCommon.RemoveSpace(textSearchContent.Text.Trim());
            //int vSearch_DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
            int vSearch_PhienHop = int.Parse(ddlistPhienHop.SelectedValue);
            Dictionary<string, string> dictSearch = null;
            //Khoi tao dictionary
            if (vSearchContent != "" || vSearch_PhienHop != -1)
            {
                dictSearch = new Dictionary<string, string>();
                dictSearch.Add("KeyWord", vSearchContent);
                dictSearch.Add("PhienHop", vSearch_PhienHop.ToString());
            }
            if (vSearchContent == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("KeyWord");
            }
            if (vSearch_PhienHop == -1)
            {
                if (dictSearch != null)
                    dictSearch.Remove("PhienHop");
            }
            //Gan danh sach Search into Session
            Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] = dictSearch;
            LoadDanhSach(0);
        }

        #endregion
        #region Methods
        /// <summary>
        /// Set thong tin Search
        /// </summary>
        /// <param name="pObjSearch"></param>
        public void SetFormInfo(Object pObjSearch)
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

        /// <summary>
        /// Đánh số thứ tự auto trên record dataGrid
        /// </summary>
        /// <returns></returns>
        public string STT()
        {
            return ((dgDanhSach.CurrentPageIndex * vPageSize) + vStt++).ToString();
        }

        /// <summary>
        ///  Load danh sách co phan trang
        ///  Kết luân = 0 or null : Chưa Kết luận
        ///           = 1 Thông qua
        ///           = 2 Không thông qua
        /// </summary>
        /// <param name="pCurentPage"></param>
        protected void LoadDanhSach(int pCurentPage)
        {
            try
            {
                string vContentSearch = textSearchContent.Text.Trim();
                //int vDONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                //int vCV_ID = int.Parse(ddlistChucVu.SelectedValue);
                int vPhienHop = int.Parse(ddlistPhienHop.SelectedValue);

                string vErrorMessage = "";


                if (Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] != null)
                {
                    Dictionary<string, string> vDictSearch = (Dictionary<string, string>)Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"];
                    if (ClassCommon.ExistKey("KeyWord", vDictSearch))
                    {
                        vContentSearch = vDictSearch["KeyWord"].ToLower();
                    }
                    if (ClassCommon.ExistKey("PhienHop", vDictSearch))
                    {
                        vPhienHop = int.Parse(vDictSearch["PhienHop"]);
                    }                 
                }

                var vBieuQuyetInfo = vBieuQuyetControllerInfo.GetDanhSachBieuQuyet(vContentSearch, vPhienHop);
                           
                if (ViewState["keysort_bieuquyet"] != null && ViewState["typesort_bieuquyet"] != null)
                {
                    string key = ViewState["keysort_bieuquyet"].ToString();
                    string type = ViewState["typesort_bieuquyet"].ToString();

                    if (key == "NOIDUNGBIEUQUYET" && type == "ASC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderBy(x => x.NOIDUNGBIEUQUYET).ToList();
                    }
                    if (key == "NOIDUNGBIEUQUYET" && type == "DESC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderByDescending(x => x.NOIDUNGBIEUQUYET).ToList();
                    }           
                    
                    if (key == "DIENGIAI" && type == "DESC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderByDescending(x => x.DIENGIAI).ToList();
                    }
                    if (key == "DIENGIAI" && type == "ASC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderBy(x => x.DIENGIAI).ToList();
                    }
                    if (key == "TIEUDEPHIENHOP" && type == "DESC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderByDescending(x => x.PHIENHOP.TIEUDE).ToList();
                    }
                    if (key == "TIEUDEPHIENHOP" && type == "ASC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderBy(x => x.PHIENHOP.TIEUDE).ToList();
                    }
                    if(key == "THOIGIANBATDAU" && type == "DESC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderByDescending(x => x.THOIGIANBATDAU).ToList();
                    }
                    if (key == "THOIGIANBATDAU" && type == "ASC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderBy(x => x.THOIGIANBATDAU).ToList();
                    }
                    if (key == "THOIGIANKETTHUC" && type == "DESC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderByDescending(x => x.THOIGIANKETTHUC).ToList();
                    }
                    if (key == "THOIGIANKETTHUC" && type == "ASC")
                    {
                        vBieuQuyetInfo = vBieuQuyetInfo.OrderBy(x => x.THOIGIANKETTHUC).ToList();
                    }

                }
                if (vBieuQuyetInfo != null)
                {
                    dgDanhSach.VirtualItemCount = vBieuQuyetInfo.Count;
                    dgDanhSach.DataSource = vBieuQuyetInfo;
                    dgDanhSach.PageSize = vPageSize;
                    dgDanhSach.CurrentPageIndex = pCurentPage;
                    dgDanhSach.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ trị", "Thông báo lỗi", "error");
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
                Response.Redirect(Globals.NavigateURL());
            }
            else
            {
                btn_ThemMoi.Visible = false;
            }
        }

        /// <summary>
        /// Get PageSize, Current Page
        /// </summary>
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
        protected void ChonNgay(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            LoadDanhSach(0);
        }
        protected void dateTuNgay_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            LoadDanhSach(0);
        }
           
        /// <summary>
        /// Sort
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgDanhSach_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortDirection = GetSortDirection(e.SortExpression);
            ViewState["keysort_bieuquyet"] = e.SortExpression;
            ViewState["typesort_bieuquyet"] = sortDirection;
            LoadDanhSach(0);
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
                for (int i = 2; i <= 4; i++)
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
                ddlistPhienHop.Items.Insert(0, new ListItem("Tất cả phiên họp", "0"));
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        #region Trạng thái
        protected void dgDuThao(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int BIEUQUYET_ID = Convert.ToInt32(html.HRef.ToString());
            DoiTrangThai(BIEUQUYET_ID,0);
            ClassCommon.ShowToastr(Page, "Cập nhật trạng thái thành công!", "Thông báo", "success");
        }
        protected void dgDangBieuQuyet(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int BIEUQUYET_ID = Convert.ToInt32(html.HRef.ToString());
            DoiTrangThai(BIEUQUYET_ID, 1);
            ClassCommon.ShowToastr(Page, "Cập nhật trạng thái thành công!", "Thông báo", "success");
        }
        protected void dgDaBieuQuyet(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int BIEUQUYET_ID = Convert.ToInt32(html.HRef.ToString());
            DoiTrangThai(BIEUQUYET_ID, 2);
            ClassCommon.ShowToastr(Page, "Cập nhật trạng thái thành công!", "Thông báo", "success");
        }

        public void DoiTrangThai(int pBIEUQUYET_ID, int pTRANGTHAI)
        {
            var objBIEUQUYET = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == pBIEUQUYET_ID).FirstOrDefault();
            if(objBIEUQUYET != null)
            {
                objBIEUQUYET.TRANGTHAI = pTRANGTHAI;
                vDataContext.SubmitChanges();
                LoadDanhSach(0);
            }
        }
        #endregion 

    }
}
