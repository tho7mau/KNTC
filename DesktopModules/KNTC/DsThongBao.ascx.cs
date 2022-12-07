#region Thông tin chung
/// Mục đích        :Hiển thị danh sách thông báo
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

    public partial class DsThongBao : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        NguoiDungController vNguoiDungControllerInfo = new NguoiDungController();
        ThongBaoController vThongBaoControllerInfo = new ThongBaoController();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
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
                this.ModuleConfiguration.ModuleTitle = "Quản lý thông báo";
                if (!IsPostBack)
                {
                    try
                    {                     
                        //Session.RemoveAll();
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


        /// <summary>
        /// Cập nhật thông tin người dùng (chuyển đến trang Edit)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDanhSach_Sua(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vId = html.HRef;
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thông tin thông báo", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Load danh sách chức vụ
        /// </summary>
        //public void LoadDropDownChucVu()
        //{
        //    try
        //    {
        //        List<CHUCVU> vChucVuInfo = vChucVuControllerInfo.GetDanhSachChucVu();
        //        ddlistChucVu.DataSource = vChucVuInfo;
        //        ddlistChucVu.Items.Insert(0, new ListItem("Tất cả chức vụ", "-1"));
        //        ddlistChucVu.DataTextField = "TENCHUCVU";
        //        ddlistChucVu.DataValueField = "CV_ID";
        //        ddlistChucVu.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
        //    }

        //}
      

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
                List<int> vPhienHopIds = new List<int>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vPhienHopIds.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                //Xóa danh sách phiên họp đã chọn
                int vCountPhienHopDaXoa = 0;
                string vErrorMessage = "";
                foreach (int vPhienHopId in vPhienHopIds)
                {
                    if (!vPhienHopControllerInfo.KiemTraPhienHopDangDuocSuDung(vPhienHopId, out vErrorMessage))
                    {
                        vPhienHopControllerInfo.XoaPhienHop(vPhienHopId, out vErrorMessage);
                        vCountPhienHopDaXoa += 1;
                    }
                }
                ClassCommon.ShowToastr(Page, "Xóa thành công " + vCountPhienHopDaXoa + " phiên họp!", "Thông báo", "success");
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
            int vPhienHopId = int.Parse(html.HRef);
            string vErrorMessage = "";
            UserInfo _User = new UserInfo();
            try
            {
                //List<PSW_XACTHUC_LENH> lstResult = nvController.GetXTLenh(vId);
                if (vPhienHopControllerInfo.KiemTraPhienHopDangDuocSuDung(vPhienHopId, out vErrorMessage))//phiên họp đã phát sinh dữ liệu
                {
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa không thành công, phiên họp đã phát sinh dữ liệu!", "Thông báo", "error");
                }
                else
                {
                    vPhienHopControllerInfo.XoaPhienHop(vPhienHopId, out vErrorMessage);
                    ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Xóa phiên họp thành công!", "Thông báo", "success");
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới thông báo", "id=0");
            SessionDestroy(new string[] { vMacAddress+  PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
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
            int vSearch_TrangThai = int.Parse(ddlistTrangThai.SelectedValue);
            Dictionary<string, string> dictSearch = null;
            //Khoi tao dictionary
            if (vSearchContent != "" || vSearch_TrangThai != -1)
            {
                dictSearch = new Dictionary<string, string>();
                dictSearch.Add("KeyWord", vSearchContent);
                dictSearch.Add("TrangThai", vSearch_TrangThai.ToString());               
            }
            if (vSearchContent == "")
            {
                if (dictSearch != null)
                    dictSearch.Remove("KeyWord");
            }
            if (vSearch_TrangThai == -1)
            {
                if (dictSearch != null)
                    dictSearch.Remove("TrangThai");
            }             
            //Gan danh sach Search into Session
            Session[vMacAddress+ PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] = dictSearch;
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
                if (ClassCommon.ExistKey("TrangThai", dictSearch))
                {
                    ddlistTrangThai.SelectedValue = dictSearch["TrangThai"];
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
                //int vDONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                //int vCV_ID = int.Parse(ddlistChucVu.SelectedValue);
                int vTrangThai = int.Parse(ddlistTrangThai.SelectedValue);

                string vErrorMessage = "";


                if (Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"] != null)
                {
                    Dictionary<string, string> vDictSearch = (Dictionary<string, string>)Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"];
                    if (ClassCommon.ExistKey("KeyWord", vDictSearch))
                    {
                        vContentSearch = vDictSearch["KeyWord"].ToLower();
                    }
                    if (ClassCommon.ExistKey("TrangThai", vDictSearch))
                    {
                        vTrangThai = int.Parse(vDictSearch["TrangThai"]);
                    }
                    ////if (ClassCommon.ExistKey("CV_ID", vDictSearch))
                    ////{
                    ////    vDONVI_ID = int.Parse(vDictSearch["CV_ID"]);
                    ////}
                }

                List<HKG_THONGBAO> vThongBaoInfos = vThongBaoControllerInfo.GetDanhSachThongBao(vContentSearch);

                //vThongBaoInfos = vThongBaoInfos.OrderByDescending(x => x.SendDate).ToList();
                if (ViewState["keysort_phienhop"] != null && ViewState["typesort_phienhop"] != null)
                {
                    string key = ViewState["keysort_phienhop"].ToString();
                    string type = ViewState["typesort_phienhop"].ToString();

                    //if (key == "TIEUDE" && type == "ASC")
                    //    vThongBaoInfos = vThongBaoInfos.OrderBy(x => x.Title).ToList();
                    //if (key == "TIEUDE" && type == "DESC")
                    //    vThongBaoInfos = vThongBaoInfos.OrderByDescending(x => x.Title).ToList();                
                }

                if (vThongBaoInfos != null)
                {
                    dgDanhSach.VirtualItemCount = vThongBaoInfos.Count;
                    dgDanhSach.DataSource = vThongBaoInfos;
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
        /// Thay đổi trạng thái sử dụng thiết bị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThayDoiTrangThai(object sender, EventArgs e)
        {
            try
            {
                HtmlAnchor html = (HtmlAnchor)sender;
                int vPhienHopId = int.Parse(html.HRef);
                PHIENHOP vPhienHopInfo = new PHIENHOP();
                string oErrorMessage = "";
                vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                if (vPhienHopInfo != null)
                {
                    if (vPhienHopInfo.TRANGTHAI == 0)
                    {
                        vPhienHopInfo.TRANGTHAI = 3;//Trạng thái chờ duyệt
                    }                   
                    vPhienHopControllerInfo.CapNhatPhienHop(vPhienHopId, vPhienHopInfo, out oErrorMessage);
                    ClassCommon.ShowToastr(Page, "Cập nhật trạng thái thiết bị thành công", "Thông báo", "Success");
                    LoadDanhSach(vCurentPage);
                }

            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
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
                for (int i = 2; i <= 5; i++)
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



        protected void buttonThietLap_Click(object sender, EventArgs e)
        {
            try
            {             
                string vUrl = Globals.NavigateURL("thietlap", "mid=" + this.ModuleId, "title=Thiết lập thông báo");
                Response.Redirect(vUrl);
            } 
            catch (Exception ex)
            {

            }
        }
    }
}
