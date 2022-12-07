#region Thông tin chung
/// Mục đích        :Hiển thị danh sách đơn thư
/// Ngày tại        :03/04/2021
/// Người tạo       :NGÔ HOÀI HẬN
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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    /// <summary>
    /// Xử lý dữ liệu danh sách đơn thư
    /// </summary>

    public partial class DsDonThu : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = 10;//ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        public string vPathCommonUploadHoSo = ClassParameter.vPathCommonUploadHoSo;
        int vStt = 1;
        int vCurentPage = 0;
        int _StartPage = 1;
        int _EndPage = 10;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        DonThuController vDonThuController = new DonThuController();
        string vMacAddress = ClassCommon.GetMacAddress();
        KNTCDataContext vDC = new KNTCDataContext();
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
                this.ModuleConfiguration.ModuleTitle = "Quản lý đơn thư";
                if (!IsPostBack)
                {
                    SetFormInfo();
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
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin đơn thư", "id=" + vId);
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }
        ///// <summary>
        ///// Xóa nhiều đơn thư
        ///// Ngô Hoài Hận
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy danh sách ID tiếp dân trên danh sách
                List<Int64> vDONTHU_IDs = new List<Int64>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    CheckBox vCheckBox = (CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vDONTHU_IDs.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }
                int STT_NHONHAT = 0;
                var objDONTHU = vDC.DONTHUs.Where(x => vDONTHU_IDs.Contains(x.DONTHU_ID)).ToList();
                if (objDONTHU.Count > 0)
                {
                        // xóa đơn thư có STT = null
                       //var objDONTHU_STT_NULL = vDC.DONTHUs.Where(x => x.DONTHU_STT == null).ToList();
                       // if (objDONTHU_STT_NULL.Count >0 )
                       // {
                        STT_NHONHAT = (int)objDONTHU.OrderBy(x => x.DONTHU_STT).Select(x => x.DONTHU_STT).First();
                        vDC.DONTHUs.DeleteAllOnSubmit(objDONTHU);
                        vDC.SubmitChanges();
                        //vDC.DONTHUs.DeleteAllOnSubmit(objDONTHU_STT_NULL);
                        //vDC.SubmitChanges();
                    //}
                    //objDONTHU = objDONTHU.Where(x => x.DONTHU_STT != null).ToList();
                  

                    // Cập nhật lại số thứ tự tiếp dân
                    var objDT = vDC.DONTHUs.Where(x => x.DONTHU_STT > STT_NHONHAT).OrderBy(x => x.DONTHU_STT).ToList();
                    foreach (var it in objDT)
                    {
                       
                        it.DONTHU_STT = STT_NHONHAT;
                        STT_NHONHAT++;
                        vDC.SubmitChanges();
                    }
                   

                    ClassCommon.ShowToastr(Page, "Xóa thành công " + vDONTHU_IDs.Count() + " đơn thư !", "Thông báo", "success");
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới đơn thư", "id=0");
            SessionDestroy(new string[] { vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_SearchContent" });
            Response.Redirect(vUrl);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
            LoadDanhSach(1, vPageSize);
            SessionDefault();
        }

        #endregion

        #region Methods
       
        public void SetFormInfo()
        {
            //if (pObjSearch != null)
            //{
            //    Dictionary<string, string> dictSearch = (Dictionary<string, string>)pObjSearch;
            //    if (ClassCommon.ExistKey("KeyWord", dictSearch))
            //    {
            //        textSearchContent.Text = dictSearch["KeyWord"];
            //    }
            //}
            //else
            //{
            //    //txtSearchContent.Text = "";
            //    //slChuyenKhoa.SelectedItem.Value = "0";
            //}
        }
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
                if (vKeySort == "" && vTypeSort == "")
                {
                    vKeySort = "DONTHU_STT";
                    vTypeSort = "DESC";
                }
                CommonController objCommonController = new CommonController();

                string vSearchOption = "|DONTHU.DONTHU_STT,normal,,|CANHAN.CANHAN_HOTEN,normal,,|CANHAN.CANHAN_DIACHI_DAYDU,normal,,|DONTHU.DONTHU_NOIDUNG,normal,,";
                //string vSearchOption = "";
                if (Request.QueryString["DOITUONG_ID"] != null)
                {
                    vSearchOption += "|DONTHU.DOITUONG_ID,equal,=" + Request.QueryString["DOITUONG_ID"].ToString() + ",";
                }

                if (textSearchContent_HiddenField.Text != "")
                {
                    vSearchOption = textSearchContent_HiddenField.Text;
                }
                    DataSet ds = objCommonController.GetPage(PortalId, ModuleId, "DonThu_GetPage", vSearchOption, textSearchContent.Text, vKeySort + " " + vTypeSort, v_start - 1, v_end);
                List<V_DONTHU_DOITUONG> vDONTHUs = vDonThuController.getList(vContentSearch);
                int TotalRow = 0;
                if (1 > 0)
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
        public string GetCoQuanChuyenDon(string vCoQuanId)
        {
            try
            {
                string vTenCoQuan = "";
                vTenCoQuan = vDC.DONVIs.Where(x => x.DONVI_ID == int.Parse(vCoQuanId)).FirstOrDefault().TENDONVI;
                return vTenCoQuan;
            }
            catch (Exception ex)
            {
                return "";
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
        
            if (e.Item.ItemType == ListItemType.Header)
            {
                LinkButton btnSort;
                Image image;
                //iterate through all the header cells
                int dem = 0;
                foreach (System.Web.UI.WebControls.TableCell cell in e.Item.Cells)
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
        protected void dgDanhSach_XuatPhieu(object sender, EventArgs e)
        {
            HtmlAnchor html = (HtmlAnchor)sender;
            int vId = int.Parse(html.HRef);
            try
            {
                List<int> vDONTHU_IDs = new List<int>();
                vDONTHU_IDs.Add(vId);
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
        public string GetTenLoaiDonThuById(string pLOAIDONTHU_ID)
        {
            try
            {
                string vTenLoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == int.Parse(pLOAIDONTHU_ID)).FirstOrDefault().LOAIDONTHU_TEN;
                return vTenLoaiDonThu;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string getThongTinDoiTuong(int pTDOITUONG_ID)
        {
            try
            {
                string strDoiTuong = "";
                List<CANHAN> objCANHANs = vDC.CANHANs.Where(x => x.DOITUONG_ID == pTDOITUONG_ID).ToList();
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
            catch (Exception ex)
            {
                return "";
            }

        }
        public bool GetSubCharacterName(string pName, out string oSubCharracter)
        {
            string vSubCharracter = "";

            if (pName.Contains(" "))
            {
                pName = pName.Trim();
                var vSubCharracter_arr = pName.Split(' ');
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

        protected void buttonGiaiQuyet_Click(object sender, EventArgs e)
        {
            try
            {
                //Lấy danh sách ID đơn thư trên danh sách
                List<int> vDonThuIds = new List<int>();
                foreach (DataGridItem GridItem in dgDanhSach.Items)
                {
                    System.Web.UI.WebControls.CheckBox vCheckBox = (System.Web.UI.WebControls.CheckBox)GridItem.Cells[0].Controls[1];
                    if (vCheckBox.Checked == true)
                    {
                        vDonThuIds.Add((int.Parse(dgDanhSach.DataKeys[GridItem.ItemIndex].ToString())));
                    }
                }

                if (vDonThuIds.Count > 0)
                {
                    foreach (var vId in vDonThuIds)
                    {
                        var vDonThuInfo = vDC.DONTHUs.Where(x => x.DONTHU_ID == vId).FirstOrDefault();

                        if (vDonThuInfo != null)
                        {
                            if (vDonThuInfo.HUONGXULY_ID == 1 || vDonThuInfo.HUONGXULY_ID == 3 || vDonThuInfo.HUONGXULY_ID == 4 || vDonThuInfo.HUONGXULY_ID == 5)
                            {
                                vDonThuInfo.DONTHU_TRANGTHAI = 2;//Trạng thái gửi xử lý đơn thư
                            }
                            else
                            {
                                if (vDonThuInfo.HUONGXULY_ID == 2 || vDonThuInfo.HUONGXULY_ID == 9 || vDonThuInfo.HUONGXULY_ID == 10)
                                {
                                    vDonThuInfo.DONTHU_TRANGTHAI = 1;//Đã có hướng giải quyết
                                }
                                else
                                {
                                    vDonThuInfo.DONTHU_TRANGTHAI = 4;//Kết thúc đơn
                                }
                            }
                            vDC.SubmitChanges();
                        }
                    }
                    LoadDanhSach(_StartPage, _EndPage);
                    ClassCommon.ShowToastr(Page, "Đã giải quyết thành công những đơn thư đã chọn", "Thông báo", "success");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void SessionDefault()
        {
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = 10;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_StartPage"] = 1;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_EndPage"] = 10;
        }
        #endregion
        #region Giai quyet don thu

        public string KiemTraNgayGiaiQuyet(string pDonThuId)
        {
            try
            {
                string vResult = "";
                var vDonThuInfo = vDC.DONTHUs.Where(x => x.DONTHU_ID == int.Parse(pDonThuId)).FirstOrDefault();
                if (vDonThuInfo != null)
                {
                    if (vDonThuInfo.KETQUA_NGAY != null)
                    {
                        if (vDonThuInfo.KETQUA_NGAY > vDonThuInfo.HUONGXULY_THOIHANGIAIQUET)
                        {
                            vResult = "<span style='color:red'>" + String.Format("{0:dd/MM/yyyy}", vDonThuInfo.KETQUA_NGAY) + " </span>";
                        }
                        else
                        {
                            vResult = String.Format("{0:dd/MM/yyyy}", vDonThuInfo.KETQUA_NGAY);
                        }
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string GetFileVanBanGiaiQuyet(string pDonThuID)
        {
            try
            {
                string vResult = "";
                var vHoSoInfos = (from vHoSo in vDC.HOSOs
                                  join vDonThuHoSo in vDC.DONTHU_HOSOs on vHoSo.HOSO_ID equals vDonThuHoSo.HOSO_ID
                                  where vDonThuHoSo.DONTHU_ID == int.Parse(pDonThuID) && vDonThuHoSo.LOAI_HS_DONTHU == 2//Hồ sơ kết quả giải quyết
                                  select vHoSo).ToList();
                if (vHoSoInfos.Count > 0)
                {

                    foreach (var vHoSoInfo in vHoSoInfos)
                    {
                        vResult += "<a href='" + vPathCommonUploadHoSo + "/" + vHoSoInfo.HOSO_FILE + "' target='_blank'><i class='icofont-download' style='color:blue'></i> " + vHoSoInfo.HOSO_TEN + " </a></br>";
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
    }
}

