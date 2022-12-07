#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật đơn vị
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
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class CnDonVi : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vDonViId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        DonViController vDonViController = new DonViController();
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
                //CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Lay ID tu Form DS
                if (Request.QueryString["id"] != null)
                {
                    vDonViId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vDonViId);
                    //textTenThietBi.Focus();                
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }


        protected void btnSua_Click(object sender, EventArgs e)
        {
            buttonSua.Visible = false;
            buttonCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            SetEnableForm(true);
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
                CapNhat(vDonViId);
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
            string vToastrMessage = "Vui lòng nhập ";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";
            if (textTenDonVi.Text == "")
            {
                textTenDonVi.CssClass += " vld";
                textTenDonVi.Focus();
                labelTenDonVi.Attributes["class"] += " vld";
                vToastrMessage = " Tên đơn vị, ";
                vResult = false;
            }
            else
            {
                textTenDonVi.CssClass = textTenDonVi.CssClass.Replace("vld", "").Trim();
                labelTenDonVi.Attributes.Add("class", labelTenDonVi.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vDonViController.KiemTraTrungTenDonVi(vDonViId, textTenDonVi.Text.Trim(), out oErrorMessage))
            {
                textTenDonVi.CssClass += " vld";
                textTenDonVi.Focus();
                labelTenDonVi.Attributes["class"] += " vld";
                vToastrMessage += "Tên đơn vị đã tồn tại. ";
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới thiết bị", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        ///  Set thông tin cho form
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        public void SetFormInfo(int pLoaiDonThuId)
        {
            try
            {
                if (pLoaiDonThuId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    var vLoaiDonThuInfo = vDataContext.DONVIs.Where(x => x.DONVI_ID == pLoaiDonThuId).FirstOrDefault();
                    if (vLoaiDonThuInfo != null)
                    {
                        labelTen.Text = vLoaiDonThuInfo.TENDONVI;
                        textTenDonVi.Text = vLoaiDonThuInfo.TENDONVI;
                        textTenVietTat.Text = vLoaiDonThuInfo.TENVIETTAT;
                        textMoTaDonVi.Text = vLoaiDonThuInfo.MOTA;
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
            textTenDonVi.Enabled = pEnableStatus;
            textTenVietTat.Enabled = pEnableStatus;
            textMoTaDonVi.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin thiết bị
        /// </summary>
        /// <param name="pThietBiId"></param>
        public void CapNhat(int pDonViID)
        {
            try
            {
                string oErrorMessage = "";

                if (pDonViID == 0)//Thêm mới thiết bị
                {

                    DONVI vDonViInfo = new DONVI();
                    vDonViInfo.TENVIETTAT = ClassCommon.ClearHTML(textTenVietTat.Text.Trim());
                    vDonViInfo.TENDONVI = ClassCommon.ClearHTML(textTenDonVi.Text.Trim());
                    vDonViInfo.MOTA = ClassCommon.ClearHTML(textMoTaDonVi.Text.Trim());
                    vDonViInfo.TRANGTHAI = 1;
                  
                    int oDonViId = 0;
                    vDonViController.ThemMoiDonVi(vDonViInfo, out oDonViId, out oErrorMessage);
                    if (oDonViId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin đơn vị", "id=" + oDonViId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới đơn vị thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin đơn vị
                {
                    var vDonViInfo = vDataContext.DONVIs.Where(x => x.DONVI_ID == vDonViId).FirstOrDefault();
                    if (vDonViInfo != null)
                    {
                        vDonViInfo.TENVIETTAT = ClassCommon.ClearHTML(textTenVietTat.Text.Trim());
                        vDonViInfo.TENDONVI = ClassCommon.ClearHTML(textTenDonVi.Text.Trim());
                        vDonViInfo.MOTA = ClassCommon.ClearHTML(textMoTaDonVi.Text.Trim());
                        vDataContext.SubmitChanges();

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin thiết bị thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenDonVi.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
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



    }
}
