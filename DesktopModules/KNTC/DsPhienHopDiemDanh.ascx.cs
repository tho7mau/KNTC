#region Thông tin chung
/// Mục đích        :Hiển thị danh sách câu hỏi
/// Ngày tại        :03/04/2020
/// Người tạo       :Nguyễn Huỳnh Khánh
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    /// <summary>
    /// Xử lý dữ liệu danh sách câu hỏi
    /// </summary>

    public partial class DsPhienHopDiemDanh : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        int vPhienHopId;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        PhienHopNguoiDungController vPhienHopNguoiDungControllerInfo = new PhienHopNguoiDungController();
        string vMacAddress = ClassCommon.GetMacAddress();
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
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
                //this.ModuleConfiguration.ModuleTitle = "Quản lý câu hỏi";
                
                if (Request.QueryString["id"] != null)
                {
                    vPhienHopId = int.Parse(Request.QueryString["id"]);
                }
                var vPhienHopInfo = vDataContext.PHIENHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                if(vPhienHopInfo != null)
                {
                    this.ModuleConfiguration.ContainerPath = "/Portals/_default/Containers/DLK_Container/";
                    this.ModuleConfiguration.ContainerSrc = "/Portals/_default/Containers/DLK_Container/Default.ascx";
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / " + "<a href='" + Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Thông tin phiên họp", "id=" + vPhienHopId) + "' class='title-link'>" + vPhienHopInfo.TIEUDE + "</a> / Điểm danh";
                    //dnnTITLE. = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / " + "<a href='" + Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Thông tin phiên họp", "id=" + vPhienHopId) + "' class='title-link'>" + vPhienHopInfo.TIEUDE + "</a> / Điểm danh";
                }

                if (!IsPostBack)
                {                   
                    try
                    {
                        //Session.RemoveAll();                       
                        loadDonVi();
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
            ViewState["keysort_diemdanh"] = e.SortExpression;
            ViewState["typesort_diemdanh"] = sortDirection;
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
        /// Cập nhật thông tin phiên họp (chuyển đến trang Edit)
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
                if(vLoai == "daibieu")
                {
                    var vDaiBieuInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                    if(vDaiBieuInfo != null)
                    {
                        if(vDaiBieuInfo.XACNHANTHAMGIA == null)
                        {
                            vDaiBieuInfo.XACNHANTHAMGIA = true;
                            vDaiBieuInfo.THAMDU = true;
                            vDataContext.SubmitChanges();
                            ClassCommon.ShowToastr(Page, "Điểm danh cho đại biểu " + vDaiBieuInfo.NGUOIDUNG.TENNGUOIDUNG + " thành công", "Thông báo", "success");
                            LoadDanhSach(vCurentPage);
                        }
                        else
                        {
                            if(vDaiBieuInfo.XACNHANTHAMGIA == false)
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
                    }

                }
                else
                {
                    if (vLoai == "khachmoi")
                    {
                        var vKhachMoiInfo = vDataContext.PHIENHOP_KHACHMOIs.Where(x => x.KHACHMOI_ID == vId && x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                        if (vKhachMoiInfo != null)
                        {                            
                            if (vKhachMoiInfo.THAMDU == null)
                            {
                                vKhachMoiInfo.THAMDU = true;
                                vDataContext.SubmitChanges();
                                ClassCommon.ShowToastr(Page, "Điểm danh cho khách mời " + vKhachMoiInfo.KHACHMOI.TENKHACHMOI + " thành công", "Thông báo", "success");
                                LoadDanhSach(vCurentPage);
                            }
                            else
                            {
                                if (vKhachMoiInfo.THAMDU == false)
                                {
                                    vKhachMoiInfo.THAMDU = true;
                                    vDataContext.SubmitChanges();
                                    ClassCommon.ShowToastr(Page, "Điểm danh cho khách mời " + vKhachMoiInfo.KHACHMOI.TENKHACHMOI + " thành công", "Thông báo", "success");
                                    LoadDanhSach(vCurentPage);
                                }
                                else
                                {
                                    vKhachMoiInfo.THAMDU = false;
                                    vDataContext.SubmitChanges();
                                    ClassCommon.ShowToastr(Page, "Bỏ điểm danh cho khách mời " + vKhachMoiInfo.KHACHMOI.TENKHACHMOI + " thành công", "Thông báo", "success");
                                    LoadDanhSach(vCurentPage);
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

      

        /// <summary>
        /// Event them moi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới câu hỏi", "id=0");
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
            int vSearch_DonVi = int.Parse(ddlistDonVi.SelectedValue);           
            Dictionary<string, string> dictSearch = null;
            //Khoi tao dictionary
            if (vSearchContent != "" || vSearch_DonVi != 0)
            {
                dictSearch = new Dictionary<string, string>();
                dictSearch.Add("KeyWord", vSearchContent);
               
                dictSearch.Add("DonVi", vSearch_DonVi.ToString());
            }
            if (vSearchContent == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("KeyWord");
            } 
            if(vSearch_DonVi.ToString() == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("DonVi");
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
                if (ClassCommon.ExistKey("DonVi", dictSearch))
                {
                    ddlistDonVi.SelectedValue = dictSearch["DonVi"];
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
        /// </summary>
        /// <param name="pCurentPage"></param>
        protected void LoadDanhSach(int pCurentPage)
        {
            try
            {
                string vContentSearch = textSearchContent.Text.Trim();
                int vDonViId = int.Parse(ddlistDonVi.SelectedValue);
                string vErrorMessage = "";

                if (Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] != null)
                {
                    Dictionary<string, string> vDictSearch = (Dictionary<string, string>)Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"];
                    if (ClassCommon.ExistKey("KeyWord", vDictSearch))
                    {
                        vContentSearch = vDictSearch["KeyWord"].ToLower();
                    } 
                    if (ClassCommon.ExistKey("DonVi", vDictSearch))
                    {
                        vDonViId = int.Parse(vDictSearch["DonVi"]);
                    }
                }
                //var vDiemDanhInfo = vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId).ToList();
                var vDiemDanhInfo = (from vDiemDanh in vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId)
                                     where ((vDiemDanh.TEN.ToLower().Contains(vContentSearch.ToLower()) || vDiemDanh.TENCHUCVU.ToLower().Contains(vContentSearch.ToLower()) || vDiemDanh.TENDONVI.ToLower().Contains(vContentSearch.ToLower()))
                                     && (vDiemDanh.DONVI_ID == vDonViId || vDonViId == 0))
                                     select vDiemDanh).ToList();

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
                vDiemDanhInfo = vDiemDanhInfo.ToList();
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

        
        #region Button
        public void loadDonVi()
        {
            try
            {
                var vListDiemDanh = vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId).ToList();
                var vDonViInfos = (from vDonVi in vListDiemDanh
                                group vDonVi by new { vDonVi.DONVI_ID, vDonVi.TENDONVI } into g
                                select new
                                {
                                    g.Key.DONVI_ID,
                                    g.Key.TENDONVI
                                }).ToList();
                ddlistDonVi.Items.Clear();
                ddlistDonVi.DataSource = vDonViInfos;
                ddlistDonVi.DataTextField = "TENDONVI";
                ddlistDonVi.DataValueField = "DONVI_ID";
                ddlistDonVi.DataBind();
                ddlistDonVi.Items.Insert(0, new ListItem("Tất cả đơn vị", "0"));
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }            
        #endregion
        
    }
}
