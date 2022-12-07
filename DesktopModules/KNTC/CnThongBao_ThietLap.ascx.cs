#region Thông tin chung
/// Mục đích        :Thiết lập gởi thông báo
/// Ngày tạo        :14/07/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       :LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Telerik.Web.UI;

namespace HOPKHONGGIAY
{
    public partial class CnThongBao_ThietLap : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        ThongBaoController vThongBaoConTrollerInfo = new ThongBaoController();
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
                if (!IsPostBack)
                {
                    SetFormInfo();
                }
                //Edit Title
                this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý thông báo</a> / Thiết lập";

            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }



        /// <summary>       
        /// Validate form và hiện thông báo lỗi
        /// </summary>       
        /// <returns>true: Form không có lỗi; flase: form có trường lỗi</returns>        
        /// <returns></returns>
        protected Boolean ValidateForm()
        {
            Boolean vResult = true;
            string vToastrMessage = "Vui lòng nhập ";

            if (cboxThongBao.Checked)
            {
                if (textOneSignalAppId.Text == "")
                {
                    textOneSignalAppId.CssClass += " vld";
                    textOneSignalAppId.Focus();
                    labelOneSignalAppID.Attributes["class"] += " vld";
                    vToastrMessage += "OneSignal App ID, ";
                    vResult = false;
                }
                else
                {
                    textOneSignalAppId.CssClass = textOneSignalAppId.CssClass.Replace("vld", "").Trim();
                    labelOneSignalAppID.Attributes.Add("class", labelOneSignalAppID.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textOneSignalRestApiKey.Text == "")
                {
                    textOneSignalRestApiKey.CssClass += " vld";
                    textOneSignalRestApiKey.Focus();
                    labelOneSignalRestApiKey.Attributes["class"] += " vld";
                    vToastrMessage += "OneSignal App ID, ";
                    vResult = false;
                }
                else
                {
                    textOneSignalRestApiKey.CssClass = textOneSignalRestApiKey.CssClass.Replace("vld", "").Trim();
                    labelOneSignalRestApiKey.Attributes.Add("class", labelOneSignalRestApiKey.Attributes["class"].ToString().Replace("vld", ""));
                }
            }

            if (cboxSMS.Checked)
            {
                if (textApiUser.Text == "")
                {
                    textApiUser.CssClass += " vld";
                    textApiUser.Focus();
                    labelApiUser.Attributes["class"] += " vld";
                    vToastrMessage += "Api User, ";
                    vResult = false;
                }
                else
                {
                    textApiUser.CssClass = textApiUser.CssClass.Replace("vld", "").Trim();
                    labelApiUser.Attributes.Add("class", labelApiUser.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textApiPassword.Text == "")
                {
                    textApiPassword.CssClass += " vld";
                    textApiPassword.Focus();
                    labelApiPassword.Attributes["class"] += " vld";
                    vToastrMessage += "Api Password, ";
                    vResult = false;
                }
                else
                {
                    textApiPassword.CssClass = textApiPassword.CssClass.Replace("vld", "").Trim();
                    labelApiPassword.Attributes.Add("class", labelApiPassword.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textApiCode.Text == "")
                {
                    textApiCode.CssClass += " vld";
                    textApiCode.Focus();
                    labelApiCode.Attributes["class"] += " vld";
                    vToastrMessage += "Api CPCode, ";
                    vResult = false;
                }
                else
                {
                    textApiCode.CssClass = textApiCode.CssClass.Replace("vld", "").Trim();
                    labelApiCode.Attributes.Add("class", labelApiCode.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textRequestID.Text == "")
                {
                    textRequestID.CssClass += " vld";
                    textRequestID.Focus();
                    labelRequestID.Attributes["class"] += " vld";
                    vToastrMessage += "Api RequestID, ";
                    vResult = false;
                }
                else
                {
                    textRequestID.CssClass = textRequestID.CssClass.Replace("vld", "").Trim();
                    labelRequestID.Attributes.Add("class", labelRequestID.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textServiceID.Text == "")
                {
                    textServiceID.CssClass += " vld";
                    textServiceID.Focus();
                    labelServiceID.Attributes["class"] += " vld";
                    vToastrMessage += "Api RequestID, ";
                    vResult = false;
                }
                else
                {
                    textServiceID.CssClass = textServiceID.CssClass.Replace("vld", "").Trim();
                    labelServiceID.Attributes.Add("class", labelServiceID.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textCommandCode.Text == "")
                {
                    textCommandCode.CssClass += " vld";
                    textCommandCode.Focus();
                    labelCommandCode.Attributes["class"] += " vld";
                    vToastrMessage += "Api CommandCode, ";
                    vResult = false;
                }
                else
                {
                    textCommandCode.CssClass = textCommandCode.CssClass.Replace("vld", "").Trim();
                    labelCommandCode.Attributes.Add("class", labelCommandCode.Attributes["class"].ToString().Replace("vld", ""));
                }

              
               
            }
            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
            }

            return vResult;
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
                CapNhat();
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
        public void SetFormInfo()
        {
            try
            {
                {                    
                    var vThietLapInfo = vDataContext.THONGBAO_THIETLAPs.Where(x => x.Id == 1).FirstOrDefault();
                    if (vThietLapInfo != null)
                    {
                        if (vThietLapInfo.TrangThaiGoiSms)
                        {
                            cboxSMS.Checked = true;                            
                        }
                        else
                        {
                            cboxSMS.Checked = false;                            
                        }

                        if (vThietLapInfo.TrangThaiGoiThongBao)
                        {
                            cboxThongBao.Checked = true;
                        }
                        else
                        {
                            cboxThongBao.Checked = false;
                        }

                        textOneSignalAppId.Text = vThietLapInfo.PUSH_OneSignalAppId;
                        textOneSignalRestApiKey.Text = vThietLapInfo.PUSH_OneSignalRestAPIKey;
                        textApiUser.Text = vThietLapInfo.SMS_ApiUsername;
                        textApiPassword.Text = vThietLapInfo.SMS_ApiPassword;
                        textApiCode.Text = vThietLapInfo.SMS_ApiCode;
                        textRequestID.Text = vThietLapInfo.SMS_ApiRequestID;
                        textServiceID.Text = vThietLapInfo.SMS_ApiServiceID;
                        textCommandCode.Text = vThietLapInfo.SMS_ApiCommandCode;
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
            //textTenChucVu.Enabled = pEnableStatus;
            //textMota.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin chức vụ
        /// </summary>
        public void CapNhat()
        {
            try
            {              
                var vThongBaoInfo = vDataContext.THONGBAO_THIETLAPs.Where(x=>x.Id == 1).FirstOrDefault();
                if(vThongBaoInfo != null)
                {
                    vThongBaoInfo.TrangThaiGoiSms = cboxSMS.Checked;
                    vThongBaoInfo.TrangThaiGoiThongBao = cboxThongBao.Checked;

                    vThongBaoInfo.SMS_ApiUsername = ClassCommon.ClearHTML(textApiUser.Text.Trim());
                    vThongBaoInfo.SMS_ApiPassword = ClassCommon.ClearHTML(textApiPassword.Text.Trim());
                    vThongBaoInfo.SMS_ApiCode = ClassCommon.ClearHTML(textApiCode.Text.Trim());
                    vThongBaoInfo.SMS_ApiRequestID = ClassCommon.ClearHTML(textRequestID.Text.Trim());
                    vThongBaoInfo.SMS_ApiServiceID = ClassCommon.ClearHTML(textServiceID.Text.Trim());
                    vThongBaoInfo.SMS_ApiCommandCode = ClassCommon.ClearHTML(textCommandCode.Text.Trim());

                    vThongBaoInfo.PUSH_OneSignalAppId = ClassCommon.ClearHTML(textOneSignalAppId.Text.Trim());
                    vThongBaoInfo.PUSH_OneSignalRestAPIKey = ClassCommon.ClearHTML(textOneSignalRestApiKey.Text.Trim());

                    vDataContext.SubmitChanges();
                    Session[vMacAddress + TabId.ToString() + "_Message"] = "Thay đổi thiết lập thông báo thành công";
                    Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                    Response.Redirect(Globals.NavigateURL(),true);
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



        protected void cboxSMS_ServerChange(object sender, EventArgs e)
        {
            try
            {
                if (cboxSMS.Checked)
                {
                    divSMS.Visible = true;
                }
                else
                {
                    divSMS.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
