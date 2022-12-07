#region Thông tin chung
/// Mục đích        :Cập nhật vị trí ngồi trong phiên họp
/// Ngày tạo        :01/06/2020
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
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace HOPKHONGGIAY
{
    public partial class CnPhienHop_ViTri : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        string vBenhVienMaSo = ClassParameter.msbv;
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        PhongHopController vPhongHopControllerInfo = new PhongHopController();
        PhienHopNguoiDungController vPienHopNguoiDungControllerInfo = new PhienHopNguoiDungController();

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
                    SetFormInfo(vPhienHopId);
                    SetInfoSoDo(vPhienHopId, false);
                    LoadDanhSach();
                }
                //Edit Title
                if (vPhienHopId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> ";
                }
                else
                {
                    var vPhienHopInfo = vPhienHopControllerInfo.GetPhienHopTheoId(vPhienHopId);
                    if (vPhienHopInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phòng ban</a> / " + vPhienHopInfo.TIEUDE;
                    }
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }


        /// <summary>
        /// Event Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDanhSach();
        }

        /// <summary>
        ///  Set thông tin sơ đồ khi chọn ghế
        /// </summary>
        /// <param name="pPhienHopId"></param>
        /// <param name="pCapNhatFile">true: Cập nhật file phòng họp;</param>
        public void SetInfoSoDo(int pPhienHopId, bool pCapNhatFile)
        {
            try
            {
                SoDoPhongHopInfo objSoDo = (from a in vDataContext.PHIENHOP_PHONGHOPs
                                            where a.PHIENHOP_ID == pPhienHopId
                                            join phong in vDataContext.PHONGHOPs on a.PHONGHOP_ID equals phong.PHONGHOP_ID
                                            select new SoDoPhongHopInfo()
                                            {
                                                PHIENHOP_ID = a.PHIENHOP_ID,
                                                PHONGHOP_ID = a.PHONGHOP_ID,
                                                SODO_FILE = phong.SODO_FILE,
                                                SODO_TEXT = a.SODO_Text == null ? "" : Server.HtmlDecode(a.SODO_Text).Replace("\n", ""),
                                            }).FirstOrDefault();

                if (objSoDo != null)
                {

                    //Tìm danh sách phiên họp vị trí
                    var objViTris = vDataContext.PRO_PHIENHOP_VITRI(pPhienHopId).ToList();
                    

                    string vSource = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\" + objSoDo.SODO_FILE;
                    string vDes = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg";

                    string readText = File.ReadAllText(vSource);
                    //LOAI: 
                    foreach (var it in objViTris)
                    {
                        if(!String.IsNullOrEmpty(it.MAGHE))
                        {
                            int index = readText.LastIndexOf(it.MAGHE);

                            string vStr = readText.Substring(0, index);

                            int index_X = vStr.LastIndexOf("tspan x=\"");
                            index_X += ("tspan x=\"").Length;
                            string vPosition = "";

                            for (int i = index_X; i < index; i++)
                            {
                                if (vStr[i] == '"')
                                {
                                    break;
                                }
                                else
                                {
                                    vPosition += vStr[i].ToString();
                                }
                            }

                            var vNameArr = it.TEN.Trim().Split(' ');
                            string vText = "<tspan dy='0'>" + vNameArr[0] + "</tspan>";

                            for (int i = 1; i < vNameArr.Length; i++)
                            {
                                vText += "<tspan x='" + vPosition + "' dy='1.4em' >" + vNameArr[i] + "</tspan>";
                            }
                            readText = readText.Replace(it.MAGHE, vText);
                        }                     
                    }                   
                    File.WriteAllText(vDes, readText);
                    if (pCapNhatFile)
                    {
                        PHIENHOP_PHONGHOP vPhienHopPhongHopInfo = vDataContext.PHIENHOP_PHONGHOPs.Where(x => x.PHIENHOP_ID == vPhienHopId).FirstOrDefault();
                        string vPhienHopPhongHopFileName = "phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg";
                        if (vPhienHopPhongHopInfo != null)
                        {
                            vPhienHopPhongHopInfo.SODO_FILE = vPhienHopPhongHopFileName;
                            vPhienHopPhongHopInfo.SODO_Text = File.ReadAllText(vDes);
                        }
                    }

                    vDataContext.SubmitChanges();
                    lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\phien_" + pPhienHopId + "_phong_" + objSoDo.PHONGHOP_ID + ".svg" + "\" type=\"image/svg+xml\" width=\"600\">";
                }
            }
            catch (Exception ex)
            {

            }
        }



        /// <summary>
        /// Event button Cap nhat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            CapNhat(vPhienHopId);
        }

        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            SetEnableForm(true);
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


        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới phòng ban", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods

        public void LoadDanhSach()
        {
            try
            {
                string vKeyWord = textSearchContent.Text.Trim().ToLower();
                DataTable dtTable;
                dtTable = new DataTable();
                dtTable.Columns.Add("ID");
                dtTable.Columns.Add("LOAI");
                dtTable.Columns.Add("TEN");
                dtTable.Columns.Add("TENDONVI");
                dtTable.Columns.Add("TENCHUCVU");

                var vThanhPhanThamDuInfos = vDataContext.HKG_PHIENHOP_DIEMDANH(vPhienHopId).ToList();
                vThanhPhanThamDuInfos = vThanhPhanThamDuInfos.Where(x => x.TEN.ToLower().Contains(vKeyWord) || x.TENCHUCVU.ToLower().Contains(vKeyWord) || x.TENDONVI.ToLower().Contains(vKeyWord)).ToList();
                if (vThanhPhanThamDuInfos != null && vThanhPhanThamDuInfos.Count > 0)
                {
                    foreach (var vThanhPhan in vThanhPhanThamDuInfos)
                    {
                        DataRow row = dtTable.NewRow();
                        row["ID"] = vThanhPhan.ID;
                        row["LOAI"] = vThanhPhan.LOAI;
                        row["TEN"] = vThanhPhan.TEN;
                        row["TENDONVI"] = vThanhPhan.TENDONVI;
                        row["TENCHUCVU"] = vThanhPhan.TENCHUCVU;
                        dtTable.Rows.Add(row);
                    }
                }
                dgDanhSach.DataSource = dtTable;
                dgDanhSach.PageSize = 9999;
                dgDanhSach.CurrentPageIndex = 0;
                dgDanhSach.VirtualItemCount = dtTable.Rows.Count;
                dgDanhSach.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void SetFormInfo(int pPhienHopId)
        {
            try
            {
                if (vPhienHopId > 0)
                {
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] == null)
                    {
                        var vPhienHopNguoiDungCoViTri = vDataContext.PRO_PHIENHOP_VITRI(pPhienHopId);
                        var vPhienHopVTInfo = vPhienHopNguoiDungCoViTri.Where(x => x.MAGHE != null && x.MAGHE != "").ToList();
                        if (vPhienHopVTInfo.Count() > 0)
                        {
                            List<string> KeyKey = new List<string>();
                            foreach (var vPHVT in vPhienHopVTInfo)
                            {
                                var vString = vPHVT.ID.ToString() + "|" + vPHVT.MAGHE + "|" + vPHVT.LOAI;
                                KeyKey.Add(vString);
                            }
                            ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] = KeyKey;
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Set trạng thái visible form
        /// </summary>
        /// <param name="pEnableStatus"></param>
        public void SetEnableForm(bool pEnableStatus)
        {

        }

        /// <summary>
        /// Cập nhật thông tin phòng ban
        /// </summary>
        /// <param name="pPhongBanId"></param>
        public void CapNhat(int pPhongBanId)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string oErrorMessage = "";
                if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] != null)
                {
                    List<string> KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"];
                    //Clear vị trí chổ ngồi cũ
                    var vPhienHopNguoiDungInfos = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.PHIENHOP_ID == vPhienHopId).ToList();
                    var vPhienHopKhachMoiInfos = vDataContext.PHIENHOP_KHACHMOIs.Where(x => x.PHIENHOP_ID == vPhienHopId).ToList();
                    if (vPhienHopNguoiDungInfos != null && vPhienHopNguoiDungInfos.Count > 0)
                    {
                        foreach (var vPhienHopNguoiDungInfo in vPhienHopNguoiDungInfos)
                        {
                            vPhienHopNguoiDungInfo.MAGHE = "";
                        }
                        vDataContext.SubmitChanges();
                    }
                    if (vPhienHopKhachMoiInfos != null && vPhienHopKhachMoiInfos.Count > 0)
                    {
                        foreach (var vPhienHopKhachMoiInfo in vPhienHopKhachMoiInfos)
                        {
                            vPhienHopKhachMoiInfo.MAGHE = "";
                        }
                        vDataContext.SubmitChanges();
                    }

                    //Set lại vị trí chổ ngồi mới
                    for (int i = 0; i < KeyKey.Count; i++)
                    {
                        var vContent = KeyKey[i].Split('|');
                        int vId = int.Parse(vContent[0]);
                        if (vContent[2] == "daibieu")
                        {
                            var vPhienHopNguoiDungInfo = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vId && x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
                            vPhienHopNguoiDungInfo.MAGHE = vContent[1];
                            vDataContext.SubmitChanges();
                        }
                        else
                        {
                            if (vContent[2] == "khachmoi")
                            {
                                var vPhienHopNguoiDungInfo = vDataContext.PHIENHOP_KHACHMOIs.Where(x => x.KHACHMOI_ID == vId && x.PHIENHOP_ID == vPhienHopId).SingleOrDefault();
                                vPhienHopNguoiDungInfo.MAGHE = vContent[1];
                                vDataContext.SubmitChanges();
                            }
                        }
                    }
                    SetInfoSoDo(vPhienHopId, true);
                    ClassCommon.ShowToastr(Page, "Cập nhật vị trí chổ ngồi thành công", "Thông báo", "success");
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
                Response.Redirect(Globals.NavigateURL());
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

        protected void NhapMaGhe(object sender, EventArgs e)
        {
            try
            {
                var UserID = _currentUser.UserID;
                TextBox textTextBox = ((TextBox)sender);
                var vKey = textTextBox.ToolTip.Split('|');
                string vMaGhe = ClassCommon.ClearHTML(textTextBox.Text.Trim());
                string vContent = vKey[0] + "|" + vMaGhe + "|" + vKey[1];
                bool check = true;


                if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null && ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"].ToString() != "")
                {
                    List<string> KeyKey = new List<string>();
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null)
                    {
                        KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"];
                    }
                    if (KeyKey.Count() > 0)
                    {
                        foreach (var drr in KeyKey)
                        {
                            if (drr.Split('|')[0] == vContent.Split('|')[0].ToLower() && drr.Split('|')[2] == vKey[1])
                            {
                                KeyKey.Remove(drr);
                                KeyKey.Add(vContent);
                                ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                                LoadDanhSach();
                                break;
                            }
                            else
                            {
                                KeyKey.Add(vContent);
                                ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                                LoadDanhSach();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    List<string> KeyKey = new List<string>();
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] != null)
                    {
                        KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"];
                    }
                    if (KeyKey.Count() >= 0)
                    {
                        KeyKey.Add(vContent);
                        ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + UserID + "MaGhe"] = KeyKey;
                        LoadDanhSach();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void dgDanhSach_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                TextBox textMaGhe = (TextBox)e.Item.FindControl("textMaGhe");
                List<string> KeyKey = (List<string>)ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"];
                if (textMaGhe != null)
                {
                    if (ViewState[vMacAddress + PortalSettings.ActiveTab.TabID + _currentUser.UserID + "MaGhe"] != null)
                    {
                        {
                            for (int i = 0; i < KeyKey.Count; i++)
                            {
                                var vKey = KeyKey[i].Split('|');
                                if (textMaGhe.ToolTip.Split('|')[0] == vKey[0] && textMaGhe.ToolTip.Split('|')[1] == vKey[2])
                                {
                                    textMaGhe.Text = KeyKey[i].Split('|')[1];
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

        #endregion



    }
}
