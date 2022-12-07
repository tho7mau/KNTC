#region Thông tin chung
/// Mục đích        :Đổi mật khẩu
/// Ngày tạo        :03/04/2021
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
    public partial class CnCanBo_DoiMatKhau : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vCanBoId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        CanBoController vCanBoController = new CanBoController();
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
                    vCanBoId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vCanBoId);
                }
                //Edit Title

                var vCanBoInfo = vCanBoController.GetCanBoTheoId(vCanBoId);
                if (vCanBoInfo != null)
                {
                    labelTen.Text = vCanBoInfo.CANBO_TEN;
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
                CapNhat(vCanBoId);
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
            Boolean vResult_Total = true;
            Boolean vResult = true;
            Boolean vResult_Password = true;
            string vToastrMessage = "";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";


            if (textMatKhau.Text == "")
            {
                textMatKhau.CssClass += " vld";
                textMatKhau.Focus();
                labelMatKhau.Attributes["class"] += " vld";
                vToastrMessage += "Mật khẩu, ";
                vResult = false;
            }
            else
            {
                textMatKhau.CssClass = textMatKhau.CssClass.Replace("vld", "").Trim();
                labelMatKhau.Attributes.Add("class", labelMatKhau.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (textXacNhanMatKhau.Text == "")
            {
                textXacNhanMatKhau.CssClass += " vld";
                textXacNhanMatKhau.Focus();
                labelXacNhanMatKhau.Attributes["class"] += " vld";
                vToastrMessage += "Xác nhận mật khẩu, ";
                vResult = false;
                vResult_Total = false;
            }
            else
            {
                textXacNhanMatKhau.CssClass = textXacNhanMatKhau.CssClass.Replace("vld", "").Trim();
                labelXacNhanMatKhau.Attributes.Add("class", labelXacNhanMatKhau.Attributes["class"].ToString().Replace("vld", ""));
            }

            if ((textMatKhau.Text != "" && textXacNhanMatKhau.Text != "") && textXacNhanMatKhau.Text != textMatKhau.Text)
            {
                textXacNhanMatKhau.CssClass += " vld";
                textXacNhanMatKhau.Focus();
                labelXacNhanMatKhau.Attributes["class"] += " vld";
                vToastrMessagePassword = "Mật khẩu xác nhận không chính xác ";
                vResult_Password = false;
                vResult_Total = false;
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
            return vResult_Total;
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới đổi mật khẩu", "id=0");
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
                buttonThemmoi.Visible = false;
                buttonCapNhat.Visible = true;
                buttonSua.Visible = false;
                SetEnableForm(false);
                var vCanBoInfo = vCanBoController.GetCanBoTheoId(vCanBoId);
                if (vCanBoInfo != null)
                {
                    textTenCanBo.Text = vCanBoInfo.CANBO_TEN;
                    textTenDangNhap.Text = vCanBoInfo.Username;
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
            textTenCanBo.Enabled = pEnableStatus;
            textTenDangNhap.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin đổi mật khẩu
        /// </summary>
        /// <param name="pChucVuId"></param>
        public void CapNhat(int pChucVuId)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                var vCanBoInfo = vCanBoController.GetCanBoTheoId(vCanBoId);
                if (vCanBoInfo != null)
                {
                    UserInfo vUserInfo = UserController.GetUserById(this.PortalId, vCanBoInfo.UserId ?? 0);
                    string vOldPassword = UserController.ResetPassword(vUserInfo, vUserInfo.Membership.PasswordAnswer);
                    if (UserController.ChangePassword(vUserInfo, vOldPassword, textMatKhau.Text.Trim()) == true)
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Đổi mật khẩu cán bộ thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(Globals.NavigateURL());
                    }
                    else
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Đổi mật khẩu cán bộ không thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "error";
                        Response.Redirect(Globals.NavigateURL());
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
