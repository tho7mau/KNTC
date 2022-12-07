#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật chức vụ aa
/// Ngày tạo        :03/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Telerik.Web.UI;

namespace KNTC
{
    public partial class CnChucVu : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vChucVuId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        string vBenhVienMaSo = ClassParameter.msbv;
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        ChucVuController vChucVuControllerInfo = new ChucVuController();
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
                    vChucVuId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vChucVuId);
                    textTenChucVu.Focus();
                }
                //Edit Title
                if (vChucVuId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý chức vụ</a> / Thêm mới";
                }
                else
                {
                    var vChucVuInfo = vChucVuControllerInfo.GetChucVuTheoId(vChucVuId);
                    if (vChucVuInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý chức vụ</a> / " + vChucVuInfo.TENCHUCVU;
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
        protected void buttonCapNhat_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == true)
            {
                CapNhat(vChucVuId);
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
            if (textTenChucVu.Text == "")
            {
                textTenChucVu.CssClass += " vld";
                textTenChucVu.Focus();
                labelTenChucVu.Attributes["class"] += " vld";
                vToastrMessage = "Vui lòng nhập Tên chức vụ, ";
                vResult = false;
            }
            else
            {
                textTenChucVu.CssClass = textTenChucVu.CssClass.Replace("vld", "").Trim();
                labelTenChucVu.Attributes.Add("class", labelTenChucVu.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (vChucVuControllerInfo.KiemTraTrungTenChucVu(vChucVuId, textTenChucVu.Text.Trim(), out oErrorMessage))
            {
                textTenChucVu.CssClass += " vld";
                textTenChucVu.Focus();
                labelTenChucVu.Attributes["class"] += " vld";
                vToastrMessage = "Tên chức vụ đã tồn tại.  ";
                vResult = false;
            }

            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
            }
            return vResult;
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới chức vụ", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pChucVuId"></param>
        public void SetFormInfo(int pChucVuId)
        {
            try
            {
                if (pChucVuId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    var vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).FirstOrDefault();
                    if (vChucVuInfo != null)
                    {
                        labelTen.Text = vChucVuInfo.TENCHUCVU; 
                        textTenChucVu.Text = vChucVuInfo.TENCHUCVU;
                        textMoTa.Text = vChucVuInfo.MOTA;
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
            textTenChucVu.Enabled = pEnableStatus;
            textMoTa.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin chức vụ
        /// </summary>
        /// <param name="pChucVuId"></param>
        public void CapNhat(int pChucVuId)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string oErrorMessage = "";

                if (pChucVuId == 0)//Thêm mới
                {
                    CHUCVU vChucVuInfo = new CHUCVU();
                    vChucVuInfo.TENCHUCVU = ClassCommon.ClearHTML(textTenChucVu.Text.Trim());
                    vChucVuInfo.MOTA = ClassCommon.ClearHTML(textMoTa.Text.Trim());
                    int oChucVuId = 0;
                    vChucVuControllerInfo.ThemMoiChucVu(vChucVuInfo, out oChucVuId, out oErrorMessage);
                    if (oChucVuId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin chức vụ", "id=" + oChucVuId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới chức vụ thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật thông tin chức vụ
                {
                    var vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).SingleOrDefault();
                    if (vChucVuInfo != null)
                    {
                        vChucVuInfo.TENCHUCVU = ClassCommon.ClearHTML(textTenChucVu.Text.Trim());
                        vChucVuInfo.MOTA = ClassCommon.ClearHTML(textMoTa.Text.Trim());
                        vDataContext.SubmitChanges();
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin chức vụ thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenChucVu.Focus();
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

        protected void buttonSua_Click(object sender, EventArgs e)
        {
            buttonSua.Visible = false;
            buttonCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            SetEnableForm(true);
        }
        #endregion


    }
}
