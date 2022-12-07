#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật dân tộc
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
    public partial class CnDanToc : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vDanTocId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        DanTocController vDanTocController = new DanTocController();
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
                    vDanTocId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vDanTocId);
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
                CapNhat(vDanTocId);
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
            if (textTenDanToc.Text == "")
            {
                textTenDanToc.CssClass += " vld";
                textTenDanToc.Focus();
                labelTenDanToc.Attributes["class"] += " vld";
                vToastrMessage = " Tên dân tộc, ";
                vResult = false;
            }
            else
            {
                textTenDanToc.CssClass = textTenDanToc.CssClass.Replace("vld", "").Trim();
                labelTenDanToc.Attributes.Add("class", labelTenDanToc.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vDanTocController.KiemTraTrungTenDanToc(vDanTocId, textTenDanToc.Text.Trim(), out oErrorMessage))
            {
                textTenDanToc.CssClass += " vld";
                textTenDanToc.Focus();
                labelTenDanToc.Attributes["class"] += " vld";
                vToastrMessage += "Tên dân tộc đã tồn tại. ";
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới dân tộc", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        ///  Set thông tin cho form
        /// </summary>
        /// <param name="pDanTocId"></param>
        public void SetFormInfo(int pDanTocId)
        {
            try
            {
                if (pDanTocId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    var vLoaiDonThuInfo = vDataContext.DANTOCs.Where(x => x.DANTOC_ID == pDanTocId).FirstOrDefault();
                    if (vLoaiDonThuInfo != null)
                    {
                        labelTen.Text = vLoaiDonThuInfo.DANTOC_TEN;
                        textTenDanToc.Text = vLoaiDonThuInfo.DANTOC_TEN;
                        textMoTa.Text = vLoaiDonThuInfo.DANTOC_MOTA;
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
            textTenDanToc.Enabled = pEnableStatus;
            textMoTa.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin dân tộc
        /// </summary>
        /// <param name="pThietBiId"></param>
        public void CapNhat(int pDanTocID)
        {
            try
            {
                string oErrorMessage = "";

                if (pDanTocID == 0)//Thêm mới dân tộc
                {

                    DANTOC vDanTocInfo = new DANTOC();
                    vDanTocInfo.DANTOC_TEN = ClassCommon.ClearHTML(textTenDanToc.Text.Trim());
                    vDanTocInfo.DANTOC_MOTA = ClassCommon.ClearHTML(textMoTa.Text.Trim());
                  
                    int oDanTocId = 0;
                    vDanTocController.ThemMoiDanToc(vDanTocInfo, out oDanTocId, out oErrorMessage);
                    if (oDanTocId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin dân tộc", "id=" + oDanTocId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới dân tộc thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin dân tộc
                {
                    var vDanTocInfo = vDataContext.DANTOCs.Where(x => x.DANTOC_ID == vDanTocId).FirstOrDefault();
                    if (vDanTocInfo != null)
                    {
                        vDanTocInfo.DANTOC_TEN = ClassCommon.ClearHTML(textTenDanToc.Text.Trim());
                        vDanTocInfo.DANTOC_MOTA = ClassCommon.ClearHTML(textMoTa.Text.Trim());
                        vDataContext.SubmitChanges();

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin dân tộc thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenDanToc.Focus();
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
