#region Thông tin chung
/// Mục đích        :Đổi mật khẩu người dùng
/// Ngày tại        :03/04/2020
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

namespace HOPKHONGGIAY
{
    public partial class CnNguoiDung_DoiMatKhau : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vNguoiDungID;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        NguoiDungController vNguoiDungControllerInfo = new NguoiDungController();
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
                    vNguoiDungID = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vNguoiDungID);
                    textMatKhau.Focus();

                }
                //Edit Title

                var vNguoiDungInfo = vNguoiDungControllerInfo.GetNguoiDungTheoID(vNguoiDungID);
                if (vNguoiDungInfo != null)
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý người dùng</a> / Đổi mật khẩu / " + vNguoiDungInfo.TENNGUOIDUNG;
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
                CapNhat(vNguoiDungID);
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
        #endregion


        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pNguoiDungId"></param>
        public void SetFormInfo(int pNguoiDungId)
        {
            try
            {
                buttonThemmoi.Visible = false;
                var vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDungId).FirstOrDefault();
                if (vNguoiDungInfo != null)
                {
                    textTenDangNhap.Text = vNguoiDungInfo.Username;
                    textHoTenNguoiDung.Text = vNguoiDungInfo.TENNGUOIDUNG;
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
            textTenDangNhap.Enabled = pEnableStatus;
            textMatKhau.Enabled = pEnableStatus;
            textXacNhanMatKhau.Enabled = pEnableStatus;
            textHoTenNguoiDung.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        /// <param name="pNguoiDungID"></param>
        public void CapNhat(int pNguoiDungID)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

                var vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDungID).SingleOrDefault();
                if (vNguoiDungInfo != null)
                {
                    UserInfo vUserInfo = UserController.GetUserById(this.PortalId, vNguoiDungInfo.UserId ?? 0);
                    string vOldPassword = UserController.ResetPassword(vUserInfo, vUserInfo.Membership.PasswordAnswer);
                    if (UserController.ChangePassword(vUserInfo, vOldPassword, textMatKhau.Text.Trim()) == true)
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Đổi mật khẩu người dùng thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(Globals.NavigateURL());
                    }
                    else
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Đổi mật khẩu người dùng không thành công";
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


        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới người dùng", "id=0");
            Response.Redirect(vUrl);
        }

        #endregion


    }
}
