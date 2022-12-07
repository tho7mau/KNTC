#region Thông tin chung
/// Mục đích        :Hiển thị danh sách đơn vị
/// Ngày tạ0        :09/04/2021
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
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    /// <summary>
    /// Xử lý dữ liệu danh sách đơn vị
    /// </summary>

    public partial class DsPhongBan : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = 10;//ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        int _StartPage = 1;
        int _EndPage = 10;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        PhongBanController vPhongBanController = new PhongBanController();
        string vMacAddress = ClassCommon.GetMacAddress();
        KNTCDataContext vDC = new KNTCDataContext();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Literal_OptionSearch.Text = literal.Text;
                //Kiem tra quyen dang nhap, phan quyen
                //CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Get PageSize, Current Page
                Get_Cache();
                //Edit Title
                this.ModuleConfiguration.ModuleTitle = "Quản lý phòng ban";
                if (!IsPostBack)
                {
                    SetFormInfo(Session[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent"]);
                    LoadDanhSach(_StartPage, _EndPage);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

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

        ///// <summary>
        ///// Cập nhật thông tin đơn vị (chuyển đến trang Edit)
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void dgDanhSach_Sua(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vId = html.HRef;
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin phòng ban", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }

        ///// <summary>
        ///// Xóa nhiều loại đơn thư
        ///// Ngô Hoài Hận
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy danh sách ID đơn vị trên danh sách
                List<int> vIds = new List<int>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vIds.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                //Xóa danh sách đơn vị đã chọn
                int vCountDaXoa = 0;
                string oErrorMessage = "";
                foreach (int vId in vIds)
                {
                    vPhongBanController.XoaPhongBan(vId, out oErrorMessage);
                    vCountDaXoa += 1;
                }
                ClassCommon.ShowToastr(Page, "Xóa thành công " + vCountDaXoa + " phòng ban!", "Thông báo", "success");
                LoadDanhSach(1, vPageSize);
                SessionDefault();
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có vấn đề xãy ra trong quá trình xóa dữ liệu. Vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }

      
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới phòng ban", "id=0");
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }   
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDanhSach(1, vPageSize);
            SessionDefault();
        }

        #endregion

        //#region Methods
        ///// <summary>
        ///// Set thong tin Search
        ///// </summary>
        ///// <param name="pObjSearch"></param>
        public void SetFormInfo(Object pObjSearch)
        {


            if (pObjSearch != null)
            {
                Dictionary<string, string> dictSearch = (Dictionary<string, string>)pObjSearch;
                if (ClassCommon.ExistKey("KeyWord", dictSearch))
                {
                    textSearchContent.Text = dictSearch["KeyWord"];
                }
            }
            
        }


        ///// <summary>
        /////  Load danh sách co phan trang
        ///// </summary>
        ///// <param name="pCurentPage"></param>
        protected void LoadDanhSach(int v_start, int v_end)
        {
            try
            {
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
                    vSearchOption = "|PHONGBAN.TENPHONGBAN,normal,,|PHONGBAN.TENDONVI,normal,,";
                }
                DataSet ds = objCommonController.GetPage(PortalId, ModuleId, "PhongBan_GetPage", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                //List<TIEPDAN_DOITUONG> vTIEPDANs = vTiepDanController.getList(vContentSearch);
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
        ///// Check Account Login
        ///// </summary>
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
            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] != null)
            {
                _StartPage = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"].ToString());
            }
            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] != null)
            {
                _EndPage = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"].ToString());
            }
        }
    
        public void SessionDestroy(string[] pArr)
        {
            for (int i = 0; i < pArr.Count(); i++)
            {
                Session.Remove(pArr[i]);
            }
        }
        protected void dgDanhSach_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortDirection = GetSortDirection(e.SortExpression);
            ViewState["keysort"] = e.SortExpression;
            ViewState["typesort"] = sortDirection;
            LoadDanhSach(1, vPageSize);
            SessionDefault();
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
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = start;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = end;
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
            _StartPage = start;
            _EndPage = end;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = start;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = end;
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
            _StartPage = start;
            _EndPage = end;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = start;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = end;
            LoadDanhSach(start, end);
        }    
        public void SessionDefault()
        {
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = 10;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = 1;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = 10;
        }
    }
}

