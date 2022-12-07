#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật phòng họp
/// Ngày tại        :05/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using Aspose.Svg;
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Web.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.com.hisoftware.api2;

namespace HOPKHONGGIAY
{
    public partial class CnPhongHop : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhongHopId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        PhongHopController vPhongHopControllerInfo = new PhongHopController();
        PhongHopThietBiController vPhongHopThietBiControllerInfo = new PhongHopThietBiController();
        ThietBiController vThietBiControllerInfo = new ThietBiController();
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
                //string dataDir = RunExamples.GetDataDir_Data();
                //using (var document = new SVGDocument(Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "/sodophong_a3.svg"))
                //{
                //    // do some actions over the document here... 
                //    string text = File.ReadAllText(Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "/sodophong_a3.svg");
                //    int i = 0;
                //}
                //Kiem tra quyen dang nhap
                CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vPhongHopId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDropDownThietBi();
                    SetFormInfo(vPhongHopId);
                    textTenPhongHop.Focus();
                }
                //Edit Title
                if (vPhongHopId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phòng họp</a> / Thêm mới";
                }
                else
                {
                    var vPhongHopInfo = vPhongHopControllerInfo.GetPhongHopTheoId(vPhongHopId);
                    if (vPhongHopInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phòng họp</a> / " + vPhongHopInfo.TENPHONGHOP;
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
                CapNhat(vPhongHopId);
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
            if (textTenPhongHop.Text == "")
            {
                textTenPhongHop.CssClass += " vld";
                textTenPhongHop.Focus();
                labelTenPhongHop.Attributes["class"] += " vld";
                vToastrMessage += "Tên phòng họp, ";
                vResult = false;
            }
            else
            {
                textTenPhongHop.CssClass = textTenPhongHop.CssClass.Replace("vld", "").Trim();
                labelTenPhongHop.Attributes.Add("class", labelTenPhongHop.Attributes["class"].ToString().Replace("vld", ""));
            }



            if (textSucChua.Text == "")
            {
                textSucChua.CssClass += " vld";
                textSucChua.Focus();
                labelSucChua.Attributes["class"] += " vld";
                vToastrMessage += "Sức chứa, ";
                vResult = false;
            }
            else
            {
                textSucChua.CssClass = textSucChua.CssClass.Replace("vld", "").Trim();
                labelSucChua.Attributes.Add("class", labelSucChua.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vPhongHopControllerInfo.KiemTraTrungTenPhongHop(vPhongHopId, textTenPhongHop.Text.Trim(), out oErrorMessage))
            {
                textTenPhongHop.CssClass += " vld";
                textTenPhongHop.Focus();
                labelTenPhongHop.Attributes["class"] += " vld";
                vToastrMessage_Trung += "Tên phòng họp đã tồn tại.";
                vResult_Trung = false;
            }
            else
            {
                textTenPhongHop.CssClass = textTenPhongHop.CssClass.Replace("vld", "").Trim();
                labelTenPhongHop.Attributes.Add("class", labelTenPhongHop.Attributes["class"].ToString().Replace("vld", ""));
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

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới phòng họp", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pPhongHopId"></param>
        public void SetFormInfo(int pPhongHopId)
        {
            try
            {
                if (pPhongHopId == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    buttonThemmoi.Visible = false;
                    btnCapNhat.Visible = true;
                }
                else
                {
                    SetEnableForm(false);
                    var vPhongHopInfo = vDataContext.PHONGHOPs.Where(x => x.PHONGHOP_ID == pPhongHopId).FirstOrDefault();
                    if (vPhongHopInfo != null)
                    {
                        
                        textTenPhongHop.Text = vPhongHopInfo.TENPHONGHOP;
                        textSucChua.Text = vPhongHopInfo.SUCCHUA.ToString();
                        textDienGiai.Text = vPhongHopInfo.DIENGIAI;
                        var vThietBiPhongHopIds = vDataContext.PHONGHOP_THIETBIs.Where(x => x.PHONGHOP_ID == vPhongHopInfo.PHONGHOP_ID).Select(x => x.THIETBI_ID).ToList();
                        if (vThietBiPhongHopIds.Count > 0)
                        {
                            foreach (RadComboBoxItem cb in ddlistThietBi.Items)
                            {
                                if (vThietBiPhongHopIds.Contains(int.Parse(cb.Value)))
                                {
                                    cb.Checked = true;
                                }
                            }
                        }
                        if(vPhongHopInfo.SODO_FILE != null && vPhongHopInfo.SODO_FILE != "")
                        {
                            ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "function", "setSrcImage('" + ClassParameter.vPathCommonUploadPhongHop + "/" + vPhongHopInfo.SODO_FILE + "');", true);
//                            buttonXoaHinhAnh.Visible = true;
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
            textTenPhongHop.Enabled = pEnableStatus;
            textSucChua.Enabled = pEnableStatus;
            textDienGiai.Enabled = pEnableStatus;
            ddlistThietBi.Enabled = pEnableStatus;            
            divSoDo.Visible = pEnableStatus;
            buttonXoaHinhAnh.Enabled = pEnableStatus;          
        }


        public bool KiemTraPhongHopCoSoDo(int pPhongHopId)
        {
            try
            {
                bool vResult = false;
                var vPhongHopInfo = vPhongHopControllerInfo.GetPhongHopTheoId(pPhongHopId);
                if(vPhongHopInfo != null)
                {
                    if(vPhongHopInfo.SODO_FILE != null && vPhongHopInfo.SODO_FILE != "")
                    {
                        vResult = true;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void LoadDropDownThietBi()
        {
            try
            {
                string oErrorMessage = "";
                List<THIETBI> vDonViInfos = vThietBiControllerInfo.GetDanhSachThietBi("", 1, out oErrorMessage);
                ddlistThietBi.DataSource = vDonViInfos;
                //ddlistThietBi.Items.Insert(0, new ListItem("Tất cả đơn vị", "-1"));
                ddlistThietBi.DataTextField = "TENTHIETBI";
                ddlistThietBi.DataValueField = "THIETBI_ID";
                ddlistThietBi.DataBind();
            }
            catch (Exception ex)
            {
                ClassCommon.ShowToastr(Page, "Có lỗi xãy ra vui lòng liên hệ quản trị", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Cập nhật thông tin phòng họp
        /// </summary>
        /// <param name="pPhongHopID"></param>
        public void CapNhat(int pPhongHopID)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string oErrorMessage = "";

                if (pPhongHopID == 0)//Thêm mới phòng họp
                {
                    PHONGHOP vPhongHopInfo = new PHONGHOP();
                    vPhongHopInfo.TENPHONGHOP = ClassCommon.ClearHTML(textTenPhongHop.Text.Trim());
                    vPhongHopInfo.DIENGIAI = ClassCommon.ClearHTML(textDienGiai.Text.Trim());
                    vPhongHopInfo.SUCCHUA = int.Parse(ClassCommon.ClearHTML(textSucChua.Text.Trim()));
                    int oPhongHopID = 0;
                    if (f_SoDoPhongHop.HasFile)
                    {
                        string filepath = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop);
                        HttpFileCollection uploadedFiles = Request.Files;
                        HttpPostedFile userPostedFile = uploadedFiles[0];
                        try
                        {
                            if (userPostedFile.ContentType == "image/svg+xml")
                            {                              
                                ClassCommon.UploadFile(userPostedFile, filepath, userPostedFile.FileName, "");
                                vPhongHopInfo.SODO_FILE = userPostedFile.FileName;                               
                                vPhongHopInfo.SODO_Text = File.ReadAllText(filepath + "/" + userPostedFile.FileName).ToString();
                            }
                            else
                            {
                                ClassCommon.ShowToastr(Page, "Vui lòng tải lên file sơ đồ định dạng .svg", "Thông báo lỗi", "error");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }    
                    
                    vPhongHopControllerInfo.ThemMoiPhongHop(vPhongHopInfo, out oPhongHopID, out oErrorMessage);
                    List<PHONGHOP_THIETBI> vPhongHopThietBiInfos = new List<PHONGHOP_THIETBI>();
                    if (oPhongHopID > 0)
                    {
                        if (ddlistThietBi.CheckedItems != null)
                        {
                            foreach (var vThietBi in ddlistThietBi.CheckedItems)
                            {
                                PHONGHOP_THIETBI vPhongHopThietBiInfo = new PHONGHOP_THIETBI();
                                vPhongHopThietBiInfo.PHONGHOP_ID = oPhongHopID;
                                vPhongHopThietBiInfo.THIETBI_ID = int.Parse(vThietBi.Value);
                                vPhongHopThietBiInfos.Add(vPhongHopThietBiInfo);
                            }
                            if (vPhongHopThietBiInfos.Count > 0)
                            {
                                vPhongHopThietBiControllerInfo.ThemNhieuPhongHopThietBi(vPhongHopThietBiInfos);
                            }
                        }
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin phòng họp", "id=" + oPhongHopID);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới phòng họp thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật thông tin phòng họp
                {
                    var vPhongHopInfo = vDataContext.PHONGHOPs.Where(x => x.PHONGHOP_ID == pPhongHopID).SingleOrDefault();
                    if (vPhongHopInfo != null)
                    {
                        vPhongHopInfo.TENPHONGHOP = ClassCommon.ClearHTML(textTenPhongHop.Text.Trim());
                        vPhongHopInfo.DIENGIAI = ClassCommon.ClearHTML(textDienGiai.Text.Trim());
                        vPhongHopInfo.SUCCHUA = int.Parse(ClassCommon.ClearHTML(textSucChua.Text.Trim()));
                        if (f_SoDoPhongHop.HasFile)
                        {
                            string filepath = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop);
                            HttpFileCollection uploadedFiles = Request.Files;
                            HttpPostedFile userPostedFile = uploadedFiles[0];
                            try
                            {
                                if (userPostedFile.ContentType == "image/svg+xml")
                                {
                                    if(vPhongHopInfo.SODO_FILE != null && vPhongHopInfo.SODO_FILE != "")
                                    {
                                        File.Delete(Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "/" + vPhongHopInfo.SODO_FILE);
                                    }
                                    ClassCommon.UploadFile(userPostedFile, filepath, userPostedFile.FileName, "");
                                    vPhongHopInfo.SODO_FILE = userPostedFile.FileName;
                                    vPhongHopInfo.SODO_Text = File.ReadAllText(filepath + "/" + userPostedFile.FileName).ToString();
                                }
                                else
                                {
                                    ClassCommon.ShowToastr(Page, "Vui lòng tải lên file sơ đồ định dạng .svg", "Thông báo lỗi", "error");
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        vDataContext.SubmitChanges();

                        if (ddlistThietBi.CheckedItems != null)
                        {
                            vPhongHopThietBiControllerInfo.XoaAllPhongHopThietBi(vPhongHopId);
                            List<PHONGHOP_THIETBI> vPhongHopThietBiInfos = new List<PHONGHOP_THIETBI>();
                            foreach (var vThietBi in ddlistThietBi.CheckedItems)
                            {
                                PHONGHOP_THIETBI vPhongHopThietBiInfo = new PHONGHOP_THIETBI();
                                vPhongHopThietBiInfo.PHONGHOP_ID = vPhongHopId;
                                vPhongHopThietBiInfo.THIETBI_ID = int.Parse(vThietBi.Value);
                                vPhongHopThietBiInfos.Add(vPhongHopThietBiInfo);
                            }
                            if (vPhongHopThietBiInfos.Count > 0)
                            {
                                vPhongHopThietBiControllerInfo.ThemNhieuPhongHopThietBi(vPhongHopThietBiInfos);
                            }
                        }
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin phòng họp thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;                        
                        btnSua.Visible = true;
                        buttonXoaHinhAnh.Visible = false;
                        ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "function", "setSrcImage('" + ClassParameter.vPathCommonUploadPhongHop + "/" + vPhongHopInfo.SODO_FILE + "');", true);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenPhongHop.Focus();
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
        #endregion

        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            buttonXoaHinhAnh.Visible = true;
            if (KiemTraPhongHopCoSoDo(vPhongHopId))
                buttonXoaHinhAnh.Visible = true;
            else
                buttonXoaHinhAnh.Visible = false;


            SetFormInfo(vPhongHopId);
            SetEnableForm(true);
        }

        protected void ddlistThietBi_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
        {
            try
            {
                if (vPhongHopId == 0)
                {
                    e.Item.Checked = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void XoaHinhAnh(object sender, EventArgs e)
        {            
            var vPhongHopInfo = vPhongHopControllerInfo.GetPhongHopTheoId(vPhongHopId);
            if(vPhongHopInfo != null)
            {
                ScriptManager.RegisterStartupScript(upn.Page, upn.GetType(), "function", "setEmptyImage();", true);
                File.Delete(Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "/" + vPhongHopInfo.SODO_FILE);
                buttonXoaHinhAnh.Visible = false;
            }               
        }
    }
}
