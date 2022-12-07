#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật phòng ban
/// Ngày tạo        :08/04/2020
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
using Telerik.Web.UI;

namespace KNTC
{
    public partial class CnPhongBan : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhongBanId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        string vBenhVienMaSo = ClassParameter.msbv;
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        PhongBanController vPhongBanControllerInfo = new PhongBanController();
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
                    vPhongBanId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachDonVi();
                    SetFormInfo(vPhongBanId);
                    textTenPhongBan.Focus();
                }
                //Edit Title
                if (vPhongBanId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phòng ban</a> / Thêm mới";
                }
                else
                {
                    var vPhoingBanInfo = vPhongBanControllerInfo.GetPhongBanTheoId(vPhongBanId);
                    if (vPhoingBanInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phòng ban</a> / " + vPhoingBanInfo.TENPHONGBAN;
                    }

                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Load danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public void LoadDanhSachDonVi()
        {
            try
            {
                List<DONVI> vDonViInfos = new List<DONVI>();
                vDonViInfos = vDataContext.DONVIs.OrderBy(x => x.TENDONVI).ToList();
                ddlistDonVi.Items.Clear();
                ddlistDonVi.DataSource = vDonViInfos;
                ddlistDonVi.DataTextField = "TENDONVI";
                ddlistDonVi.DataValueField = "DONVI_ID";
                ddlistDonVi.DataBind();
                ddlistDonVi.Items.Insert(0, new ListItem("Chọn đơn vị", ""));
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
                CapNhat(vPhongBanId);
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
            string vToastrMessage = "Vui lòng nhập ";
            string oErrorMessage = "";
            string vToastrMessage_Trung = "";
            if (textTenPhongBan.Text == "")
            {
                textTenPhongBan.CssClass += " vld";
                textTenPhongBan.Focus();
                labelTenPhongBan.Attributes["class"] += " vld";
                vToastrMessage = "Tên phòng ban, ";
                vResult = false;
            }
            else
            {
                textTenPhongBan.CssClass = textTenPhongBan.CssClass.Replace("vld", "").Trim();
                labelTenPhongBan.Attributes.Add("class", labelTenPhongBan.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (ddlistDonVi.SelectedValue == "")
            {
                ddlistDonVi.CssClass += " vld";
                ddlistDonVi.Focus();
                labelDonVi.Attributes["class"] += " vld";
                vToastrMessage += "Đơn vị, ";
                vResult = false;
            }
            else
            {
                ddlistDonVi.CssClass = ddlistDonVi.CssClass.Replace("vld", "").Trim();
                labelDonVi.Attributes.Add("class", labelDonVi.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (ddlistDonVi.SelectedValue != "" && vPhongBanControllerInfo.KiemTraTrungTenPhongBan(vPhongBanId, int.Parse(ddlistDonVi.SelectedValue), textTenPhongBan.Text.Trim(), out oErrorMessage))
            {
                textTenPhongBan.CssClass += " vld";
                textTenPhongBan.Focus();
                labelTenPhongBan.Attributes["class"] += " vld";
                vToastrMessage = "Tên phòng ban đã tồn tại.  ";
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
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới phòng ban", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pPhongBanId"></param>
        public void SetFormInfo(int pPhongBanId)
        {
            try
            {
                if (pPhongBanId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    var vPhongBanInfo = vDataContext.PhongBans.Where(x => x.PB_ID == pPhongBanId).FirstOrDefault();
                    if (vPhongBanInfo != null)
                    {
                        labelTen.Text = vPhongBanInfo.TENPHONGBAN;
                        textTenPhongBan.Text = vPhongBanInfo.TENPHONGBAN;
                        ddlistDonVi.SelectedValue = vPhongBanInfo.DONVI_ID.ToString();
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
            textTenPhongBan.Enabled = pEnableStatus;
            ddlistDonVi.Enabled = pEnableStatus;
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

                if (pPhongBanId == 0)//Thêm mới
                {
                    PhongBan vPhongBanInfo = new PhongBan();
                    vPhongBanInfo.TENPHONGBAN = ClassCommon.ClearHTML(textTenPhongBan.Text.Trim());
                    vPhongBanInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                    vPhongBanInfo.TENDONVI = ddlistDonVi.SelectedItem.Text;
                    int oPhongBanId = 0;
                    vPhongBanControllerInfo.ThemMoiPhongBan(vPhongBanInfo, out oPhongBanId, out oErrorMessage);
                    if (oPhongBanId > 0)
                    {
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới đơn vị thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin đơn vị", "id=" + oPhongBanId);
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật thông tin phòng ban
                {
                    var vPhongBanInfo = vDataContext.PhongBans.Where(x => x.PB_ID == pPhongBanId).SingleOrDefault();
                    if (vPhongBanInfo != null)
                    {
                        vPhongBanInfo.TENPHONGBAN = ClassCommon.ClearHTML(textTenPhongBan.Text.Trim());
                        vPhongBanInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                        vPhongBanInfo.TENDONVI = ddlistDonVi.SelectedItem.Text;
                        vDataContext.SubmitChanges();
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin phòng ban thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenPhongBan.Focus();
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

    }
}
