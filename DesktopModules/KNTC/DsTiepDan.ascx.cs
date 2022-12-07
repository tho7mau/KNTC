#region Thông tin chung
/// Mục đích        :Hiển thị danh sách tiếp dân
/// Ngày tại        :03/04/2021
/// Người tạo       :Nguyễn Hoàng Tấn Tài
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
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    /// <summary>
    /// Xử lý dữ liệu danh sách tiếp dân
    /// </summary>

    public partial class DsTiepDan : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = 10;//ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        int _StartPage = 1;
        int _EndPage = 10;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        TiepDanController vTiepDanController = new TiepDanController();
        string vMacAddress = ClassCommon.GetMacAddress();
        KNTCDataContext vDC = new KNTCDataContext();
        public ClassCommon vClassCommon = new ClassCommon();
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {             
                ShowMessage();
                //Get PageSize, Current Page
                Get_Cache();
                //Edit Title
                this.ModuleConfiguration.ModuleTitle = "Quản lý tiếp dân";
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
        protected void dgDanhSach_Sua(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            string vId = html.HRef;
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin tiếp dân", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }

        ///// <summary>
        ///// Xóa nhiều tiếp dân
        ///// Ngô Hoài Hận
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy danh sách ID tiếp dân trên danh sách
                List<Int64> vTIEPDAN_IDs = new List<Int64>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vTIEPDAN_IDs.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                //Xóa danh sách tiếp dân đã chọn              
                int STT_NHONHAT = 0;
                var objTD = vDC.TIEPDANs.Where(x => vTIEPDAN_IDs.Contains(x.TIEPDAN_ID)).ToList();
                if (objTD.Count > 0)
                {
                     STT_NHONHAT = (int)objTD.OrderBy(x => x.TIEPDAN_STT).Select(x => x.TIEPDAN_STT).First();
                    vDC.TIEPDANs.DeleteAllOnSubmit(objTD);
                    vDC.SubmitChanges();

                    // Cập nhật lại số thứ tự tiếp dân
                    var objIEPDAN = vDC.TIEPDANs.Where(x => x.TIEPDAN_STT > STT_NHONHAT).OrderBy(x=>x.TIEPDAN_STT).ToList();
                    foreach (var it in objIEPDAN)
                    {
                        it.TIEPDAN_STT = STT_NHONHAT;
                        STT_NHONHAT++;

                    }
                    vDC.SubmitChanges();

                    ClassCommon.ShowToastr(Page, "Xóa thành công " + vTIEPDAN_IDs.Count() + " tiếp dân!", "Thông báo", "success");
                }
              
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới tiếp dân", "id=0");
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDanhSach(1, vPageSize);
            SessionDefault();
        }

        #endregion
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
                    vSearchOption = "|TIEPDAN.TIEPDAN_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|TIEPDAN.TIEPDAN_NOIDUNG,normal,,";
                    if (Request.QueryString["DOITUONG_ID"] != null)
                    {
                        vSearchOption += "|TIEPDAN.DOITUONG_ID,equal,=" + Request.QueryString["DOITUONG_ID"].ToString() + ",";
                    }
                }
                DataSet ds = objCommonController.GetPage(PortalId, ModuleId, "TiepDan_GetPage", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                List<TIEPDAN_DOITUONG> vTIEPDANs = vTiepDanController.getList(vContentSearch);
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
      
        private void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\n", "<br />");
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\b", "<b>");
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\a", "</b>");
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

        protected void OptionSearch_TIEPDAN_STT_Click(object sender, EventArgs e)
        {
            //literal.Text = Literal_OptionSearch.Text;
        }
        public string getThongTinDoiTuong(int pTDOITUONG_ID)
        {
            string strDoiTuong = "";
            List<CANHAN> objCANHANs = vDC.CANHANs.Where(x => x.DOITUONG_ID == pTDOITUONG_ID).ToList();
            if (objCANHANs.Count > 0)
            {
                for (int i = 0; i < objCANHANs.Count; i++)
                {
                    string vSubName = "";
                    bool IsGetSubName =  GetSubCharacterName(objCANHANs[i].CANHAN_HOTEN, out vSubName);
                    strDoiTuong = strDoiTuong + "<div>";
                    if (IsGetSubName)
                    {
                        strDoiTuong = strDoiTuong + "<button type='button' class='btn btn-block btn-primary btn -lg' style='width:40px; padding:5px; height:40px; border-radius:50%;float:left;margin-right:10px;margin-top:2px;margin-bottom:10px;'>";
                        strDoiTuong = strDoiTuong + vSubName + "</button>";
                    }
                    strDoiTuong = strDoiTuong + "<h6><b>"+ objCANHANs[i].CANHAN_HOTEN + "</b></h6>";
                    strDoiTuong = strDoiTuong + "<p>"+ objCANHANs[i].CANHAN_DIACHI_DAYDU + "</p>";
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

        #region So tiep dan
        protected void btn_SoTiepDan_Click(object sender, EventArgs e)
        {
            try
            {

                var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                var objHINHTHUCGIAIQUYET = vDC.HINHTHUCGIAIQUYETs.ToList();
                var objDONVI = vDC.DONVIs.ToList();
                var objCANBO = vDC.CANBOs.ToList();
                string vKeySort = "";
                string vTypeSort = "";

                int v_start = 1;
                int v_end = 999999;
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
                    vSearchOption = "|TIEPDAN.TIEPDAN_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|TIEPDAN.TIEPDAN_NOIDUNG,normal,,";
                }
                DataSet ds = objCommonController.GetPage(PortalId, ModuleId, "TiepDan_GetPage", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                DataTable ScoresTable = ds.Tables[0];

                var ids = ScoresTable.AsEnumerable().Select(r => r.Field<long>("TIEPDAN_ID")).ToList();

                var objTiepDan = vDC.TIEPDANs.Where(x => ids.Contains(x.TIEPDAN_ID)).OrderByDescending(x=>x.TIEPDAN_ID).ToList();

                #region Xuất word từ dataGrid
                //DataTable dt = new DataTable();
                //BaoCaoController baoCaoController = new BaoCaoController();
                //dt = baoCaoController.Get_SoTiepDan_DataTable(objTiepDan);
                ////Create a dummy GridView
                //GridView GridView1 = new GridView();
                //GridView1.AllowPaging = false;
                //GridView1.RowDataBound += GridView1_RowDataBound;
                //GridView1.DataSource = dt;
                //GridView1.DataBind();


                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=SoTiepDan.doc");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-word ";
                //Response.Write("<html>");
                //Response.Write("<head>");
                //Response.Write("<META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=UTF-8'>");
                //Response.Write("<meta name=ProgId content=Word.Document>");
                //Response.Write("<meta name=Generator content='Microsoft Word 9'>");
                //Response.Write("<meta name=Originator content='Microsoft Word 9'>");
                //Response.Write("<style>");
                //Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:0.5in 0.5in 0.5in 0.5in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
                //Response.Write("div.Section1 {page:Section1;}");
                //Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:landscape;margin:0.5in 0.5in 0.5in 0.5in;mso-header-margin:.5in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
                //Response.Write("div.Section2 {page:Section2;}");
                //Response.Write("</style>");
                //Response.Write("</head>");
                //Response.Write("<body>");
                //Response.Write("<div class=Section2>");

                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //string vHeader = "<div style='text-align:center; width:100%'><b >SỔ TIẾP DÂN</b></div><br/>";
                //sw.Write(Server.HtmlDecode(vHeader));
                //GridView1.AllowPaging = false;
                //GridView1.DataBind();
                //GridView1.RenderControl(hw);
                //Response.Write(sw.ToString());
                //Response.Write("</div>");
                //Response.Write("</body>");
                //Response.Write("</html>");
                //Response.Flush();
                //Response.End();
                #endregion

                #region Xuất word bằng Key              
                int _userID = _currentUser.UserID;
                string sourceFile = Server.MapPath(ClassParameter.vPathDataBieuMau) + "\\Giaybiennhan.docx";
                string sourceFilePatch = Server.MapPath(ClassParameter.vPathDataBieuMau);
                string vPathBieuMau_MapPath = Server.MapPath(ClassParameter.vPathDataBieuMau);
                XUATWORDController objOUATWORDController = new XUATWORDController();
                List<byte[]> allData = null;
                List<string> ResponseFileNames = new List<string>();

                List<SoTiepDanInfo> soTiepDanInfos = GetSoTiepDan(objTiepDan);
                allData = objOUATWORDController.XuatSoTiepDan(soTiepDanInfos, vPathBieuMau_MapPath, sourceFile, sourceFilePatch, out ResponseFileNames);

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
                #endregion
            }
            catch (Exception Ex)
            {
            }
        }
        public List<SoTiepDanInfo> GetSoTiepDan(List<TIEPDAN> objTiepDans)
        {
            List<SoTiepDanInfo> lstSoTiepDan = new List<SoTiepDanInfo>();

            try
            {                                        
                var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                var objHINHTHUCGIAIQUYET = vDC.HINHTHUCGIAIQUYETs.ToList();
                var objDONVI = vDC.DONVIs.ToList();
                var objCANBO = vDC.CANBOs.ToList();
                                    
                if (objTiepDans.Count > 0)
                {
                    int _STT = 0;
                    foreach (var it in objTiepDans)
                    {
                        _STT++;
                        string _LOAIDON = "";
                        string _CANHAN = "";
                        string _CMND = "";
                        string _SONGUOI = "";
                        string _COQUANGIAIQUYET = "";                      
                        SoTiepDanInfo objSoTiepDan = new SoTiepDanInfo();                      
                        objSoTiepDan.STT = _STT;
                        objSoTiepDan.NGAYTIEP = it.TIEPDAN_THOGIAN == null ? "" : Convert.ToDateTime(it.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        //if (it.DONTHU_ID == null)
                        //{
                        //    row["Nội dung Khiếu nại - Tố cáo"] = "Không đơn: " + it.TIEPDAN_NOIDUNG;
                        //}
                        //else
                        //{
                        //    DONTHU objDONTHU = vDC.DONTHUs.Where(x => x.DONTHU_ID == it.DONTHU_ID).FirstOrDefault();
                        //    row["Nội dung Khiếu nại - Tố cáo"] = "Có đơn: " + objDONTHU.DONTHU_NOIDUNG;
                        //}

                        var objCANHAN = vDC.CANHANs.Where(x => x.DOITUONG_ID == it.DOITUONG_ID).FirstOrDefault();
                       
                        //foreach (var cn in objCANHAN)
                        //{
                        //    if (canhan == "")
                        //    {
                        //        canhan += cn.CANHAN_HOTEN + "\n";
                        //        canhan += cn.CANHAN_DIACHI_DAYDU;
                        //    }
                        //    else
                        //    {
                        //        canhan += "\n" + cn.CANHAN_HOTEN + "\n";
                        //        canhan += cn.CANHAN_DIACHI_DAYDU;
                        //    }

                        //}
                        if (objCANHAN !=null)
                        {
                            _CANHAN += objCANHAN.CANHAN_HOTEN ;
                            if (!string.IsNullOrEmpty(objCANHAN.CANHAN_DIACHI_DAYDU))
                            {
                                _CANHAN += "\r\n Địa chỉ:" + objCANHAN.CANHAN_DIACHI_DAYDU;
                            }
                          
                            _CMND = objCANHAN.CANHAN_CMDN;
                        }
                        objSoTiepDan.HOTEN_DIACHI = _CANHAN;
                        objSoTiepDan.CMND = _CMND;
                        objSoTiepDan.NOIDUNG = it.TIEPDAN_NOIDUNG;
                     

                        _SONGUOI = it.DOITUONG.DOITUONG_SONGUOI ==null? "": it.DOITUONG.DOITUONG_SONGUOI.ToString();
                                           
                        // Kiểm tra tiếp dân có đơn hoặc ko đơn      
                        //Không đơn
                                            
                        if (it.DONTHU_ID == null)
                        {
                            var objLDT = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == it.TIEPDAN_LOAI).FirstOrDefault();
                            if (objLDT != null)
                            {
                                string[] arr = objLDT.LOAIDONTHU_INDEX.Split('.');
                                var objLDT_LV0 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == Convert.ToInt32(arr[0])).FirstOrDefault();
                               _LOAIDON = objLDT_LV0.LOAIDONTHU_TEN ;
                            }
                            //row["Kết quả xử lý (Tiếp nhận hoặc giải thích hướng dẫn)"] = it.TIEPDAN_KETQUA;
                        }                       
                        //Có đơn 
                        else
                        {
                            var objDT = vDC.DONTHUs.Where(x => x.DONTHU_ID == it.DONTHU_ID).FirstOrDefault();
                            if (objDT != null)
                            {
                                if (objDT.LOAIDONTHU_CHA_ID != null)
                                {
                                    var objLoai = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == objDT.LOAIDONTHU_CHA_ID).FirstOrDefault();
                                    if (objLoai != null)
                                    {
                                        _LOAIDON = objLoai.LOAIDONTHU_TEN;
                                    }
                                }

                                // Tiếp dân có đơn => Đơn thư cơ quan giải quyết = DAGIAIQUYET_DONVI_ID                      
                                if (objDT.DAGIAIQUYET_DONVI_ID != null)
                                {
                                    var objDV = objDONVI.Where(x => x.DONVI_ID == objDT.DAGIAIQUYET_DONVI_ID).FirstOrDefault();
                                    if (objDV != null)
                                    {
                                        _COQUANGIAIQUYET += objDV.TENDONVI ;
                                        objSoTiepDan.COQUANGIAIQUYET = _COQUANGIAIQUYET;
                                    }
                                }
                                //hình thức giải quyết xác định bằng DAGIAIQUYET_HTGQ_ID
                                //if (objDT.DAGIAIQUYET_HTGQ_ID != null)
                                //{
                                //    var objHTGQ = objHINHTHUCGIAIQUYET.Where(x => x.HTGQ_ID == objDT.DAGIAIQUYET_HTGQ_ID).FirstOrDefault();
                                //    if (objHTGQ != null)
                                //    {
                                //        vCoquan_HinhThuc += objHTGQ.HTGQ_TEN;
                                //    }
                                //}

                                // HUONGXULY_ID = 1  Thụ lý giải quyết
                                // HUONGXULY_ID = 3  Chuyển đơn
                                // HUONGXULY_ID = 7  Trả đơn
                                if(objDT.HUONGXULY_ID == 1)
                                {
                                    objSoTiepDan.HXL_THULY = "x";
                                }
                                else if (objDT.HUONGXULY_ID == 3)
                                {
                                    objSoTiepDan.HXL_CHUYENDON = "x";
                                }
                                else if (objDT.HUONGXULY_ID == 7)
                                {
                                    objSoTiepDan.HXL_TRADON = "x";
                                }

                                objSoTiepDan.THEODOIKETQUA = objDT.KETQUA_NOIDUNG;
                                objSoTiepDan.GHICHU = objDT.DONTHU_GHICHU;

                            }
                        }
                        if (!string.IsNullOrEmpty(_SONGUOI))
                        {
                            _SONGUOI = "\r\n, Số người:" + _SONGUOI;
                        }
                        objSoTiepDan.LOAIDON_SONGUOI = _LOAIDON  + _SONGUOI;
                        lstSoTiepDan.Add(objSoTiepDan);
                    }
                }                          
            }
            catch (Exception)
            {              
            }
            return lstSoTiepDan;
        }
        public void SessionDefault()
        {
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = 10;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = 1;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = 10;
        }
        #endregion
    }
}

