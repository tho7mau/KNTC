#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật dân tộc
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class CnCanBo : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vCanBoId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        string a ;
        string b;
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        CanBoController vCanBoController = new CanBoController();
        UserDataContext vUserDataContextInfo = new UserDataContext();
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
                    vCanBoId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachDonVi();
                    LoadDanhSachChucVu();
                    SetFormInfo(vCanBoId);
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
            Boolean vResult = true;
            string vToastrMessage = "Vui lòng nhập ";
            string vToastrMessagePassword = "";
            Boolean vResult_Password = true;
            string oErrorMessage = "";
            if (textUsername.Text == "")
            {
                textUsername.CssClass += " vld";
                textUsername.Focus();
                labelUsername.Attributes["class"] += " vld";
                vToastrMessage += "Tên đăng nhập, ";
                vResult = false;
            }
            else
            {
                textUsername.CssClass = textUsername.CssClass.Replace("vld", "").Trim();
                labelUsername.Attributes.Add("class", labelUsername.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (textUsername.Text.Length < 5)
            {
                textUsername.CssClass += " vld";
                textUsername.Focus();
                labelUsername.Attributes["class"] += " vld";
                vToastrMessage += "Tên đăng nhập lớn hơn 4 kí tự, ";
                vResult = false;
            }

            if (textMatKhau.Text == "" && vCanBoId == 0)
            {
                textMatKhau.CssClass += " vld";
                textMatKhau.Focus();
                labelPassword.Attributes["class"] += " vld";
                vToastrMessage += "Mật khẩu, ";
                vResult = false;
            }
            else
            {
                textMatKhau.CssClass = textMatKhau.CssClass.Replace("vld", "").Trim();
                labelPassword.Attributes.Add("class", labelPassword.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (textXacNhanMatKhau.Text == "" && vCanBoId == 0)
            {
                textXacNhanMatKhau.CssClass += " vld";
                textXacNhanMatKhau.Focus();
                labelRepassword.Attributes["class"] += " vld";
                vToastrMessage += "Xác nhận mật khẩu, ";
                vResult = false;
            }
            else
            {
                textXacNhanMatKhau.CssClass = textXacNhanMatKhau.CssClass.Replace("vld", "").Trim();
                labelRepassword.Attributes.Add("class", labelRepassword.Attributes["class"].ToString().Replace("vld", ""));
            }
            if (textTenCanBo.Text == "")
            {
                textTenCanBo.CssClass += " vld";
                textTenCanBo.Focus();
                labelTenCanBo.Attributes["class"] += " vld";
                vToastrMessage += "Họ và tên, ";
                vResult = false;
            }
            else
            {
                textTenCanBo.CssClass = textTenCanBo.CssClass.Replace("vld", "").Trim();
                labelTenCanBo.Attributes.Add("class", labelTenCanBo.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (textEmail.Text.Trim() != "" && !ClassCommon.KiemTraDinhDangEmail(textEmail.Text.Trim()))
            {
                textEmail.CssClass += " vld";
                textEmail.Focus();
                labelEmail.Attributes["class"] += " vld";
                vToastrMessage += "Email đúng định dạng, ";
                vResult = false;
            }
            else
            {
                textEmail.CssClass = textEmail.CssClass.Replace("vld", "").Trim();
                labelEmail.Attributes.Add("class", labelEmail.Attributes["class"].ToString().Replace("vld", ""));
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

            if (ddlistPhongBan.SelectedValue == "")
            {
                ddlistPhongBan.CssClass += " vld";
                ddlistPhongBan.Focus();
                labelPhongBan.Attributes["class"] += " vld";
                vToastrMessage += "Phòng ban, ";
                vResult = false;
            }
            else
            {
                ddlistPhongBan.CssClass = ddlistPhongBan.CssClass.Replace("vld", "").Trim();
                labelPhongBan.Attributes.Add("class", labelPhongBan.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (ddlistChucVu.SelectedValue == "")
            {
                ddlistChucVu.CssClass += " vld";
                ddlistChucVu.Focus();
                labelChucVu.Attributes["class"] += " vld";
                vToastrMessage += "Chức vụ, ";
                vResult = false;
            }
            else
            {
                ddlistChucVu.CssClass = ddlistChucVu.CssClass.Replace("vld", "").Trim();
                labelChucVu.Attributes.Add("class", labelChucVu.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vCanBoId == 0)//Thêm mới cán bộ
            {
                if ((textMatKhau.Text != "" && textXacNhanMatKhau.Text != "") && textXacNhanMatKhau.Text != textMatKhau.Text)
                {
                    textXacNhanMatKhau.CssClass += " vld";
                    textXacNhanMatKhau.Focus();
                    labelRepassword.Attributes["class"] += " vld";
                    vToastrMessagePassword = "Mật khẩu xác nhận không chính xác ";
                    vResult_Password = false;
                }               
            }
            int vUserId = 0;
            if(vCanBoId > 0)
            {
                var objUser = vDataContext.CANBOs.Where(x => x.CANBO_ID == vCanBoId).FirstOrDefault();
                vUserId = objUser.UserId ?? 0;
                if (vCanBoController.KiemTraTrungUsername(ClassCommon.ClearHTML(textUsername.Text.Trim()), vCanBoId, out oErrorMessage))
                {
                    if (objUser.Username != textUsername.Text)
                    {
                        textUsername.CssClass += " vld";
                        textUsername.Focus();
                        labelUsername.Attributes["class"] += " vld";
                        vToastrMessagePassword = "Tên đăng nhập đã tồn tại ";
                        vResult_Password = false;
                    }               
                }
            }
            else
            {
                if (vCanBoController.KiemTraTrungUsername(ClassCommon.ClearHTML(textUsername.Text.Trim()), vCanBoId, out oErrorMessage))
                {
                    textUsername.CssClass += " vld";
                    textUsername.Focus();
                    labelUsername.Attributes["class"] += " vld";
                    vToastrMessagePassword = "Tên đăng nhập đã tồn tại ";
                    vResult_Password = false;
                }
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
        /// <param name="pCanBoId"></param>
        public void SetFormInfo(int pCanBoId)
        {
            try
            {
                if (pCanBoId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);

                    panelMatKhau.Visible = false;
                    var vCanBoInfo = vDataContext.CANBOs.Where(x => x.CANBO_ID == pCanBoId).FirstOrDefault();
                    if (vCanBoInfo != null)
                    {
                        labelTen.Text = vCanBoInfo.CANBO_TEN;
                        textUsername.Text = vCanBoInfo.Username;
                        textTenCanBo.Text = vCanBoInfo.CANBO_TEN;
                        textEmail.Text = vCanBoInfo.CANBO_EMAIL;
                        textSoDienThoai.Text = vCanBoInfo.CANBO_SODIENTHOAI;
                        textMoTa.Text = vCanBoInfo.GHICHU;
                        cboxLanhDao.Checked = vCanBoInfo.LANHDAO??false;
                        ddlistDonVi.SelectedValue = vCanBoInfo.DONVI_ID.ToString();
                        ddlistChucVu.SelectedValue = vCanBoInfo.CV_ID.ToString();
                        LoadDanhSachPhongBan();
                        if (vCanBoInfo.PB_ID == null)
                        {
                            ddlistPhongBan.SelectedValue = "";
                        }
                        else
                        {
                            ddlistPhongBan.SelectedValue = vCanBoInfo.PB_ID.ToString();
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
            textUsername.Enabled = pEnableStatus;
            textTenCanBo.Enabled = pEnableStatus;
            textEmail.Enabled = pEnableStatus;
            textSoDienThoai.Enabled = pEnableStatus;
            ddlistDonVi.Enabled = pEnableStatus;
            ddlistChucVu.Enabled = pEnableStatus;
            ddlistPhongBan.Enabled = pEnableStatus;
            textMoTa.Enabled = pEnableStatus;
            cboxLanhDao.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin cán bộ
        /// </summary>
        /// <param name="pThietBiId"></param>
        public void CapNhat(int pCanBoId)
        {
            try
            {
                string oErrorMessage = "";

                if (pCanBoId == 0)//Thêm mới cán bộ
                {
                    CANBO vCanBoInfo = new CANBO();

                    vCanBoInfo.Username = ClassCommon.ClearHTML(textUsername.Text.Trim());
                    vCanBoInfo.CANBO_TEN = ClassCommon.ClearHTML(textTenCanBo.Text.Trim());
                    vCanBoInfo.CANBO_EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                    vCanBoInfo.CANBO_SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                    vCanBoInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                    vCanBoInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);
                    vCanBoInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                    vCanBoInfo.LANHDAO = cboxLanhDao.Checked;

                    int oCanBoId = 0;
                    vCanBoController.ThemMoiCanBo(vCanBoInfo, PortalSettings, this.PortalId, ClassCommon.ClearHTML(textMatKhau.Text.Trim()), out oCanBoId);
                    if (oCanBoId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin cán bộ", "id=" + oCanBoId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới cán bộ thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin cán bộ
                {
                    var vCanBoInfo = vDataContext.CANBOs.Where(x => x.CANBO_ID == vCanBoId).SingleOrDefault();
                    if (vCanBoInfo != null)
                    {
                        //Cập nhật bảng cán bộ
                        vCanBoInfo.Username = ClassCommon.ClearHTML(textUsername.Text.Trim());
                        vCanBoInfo.CANBO_TEN = ClassCommon.ClearHTML(textTenCanBo.Text.Trim());
                        vCanBoInfo.CANBO_EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                        vCanBoInfo.CANBO_SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                        vCanBoInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                        vCanBoInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);
                        vCanBoInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                        vCanBoInfo.LANHDAO = cboxLanhDao.Checked;
                        vDataContext.SubmitChanges();                        
                        //Cập nhật username
                        UserController.ChangeUsername(vCanBoInfo.UserId ?? 0, ClassCommon.ClearHTML(textUsername.Text.Trim()));
                        //Cập nhật Display name bảng User dnn
                        var vUserInfo = vUserDataContextInfo.Users.Where(x => x.UserID == vCanBoInfo.UserId).SingleOrDefault();
                        vUserInfo.DisplayName = vCanBoInfo.CANBO_TEN;
                        vUserDataContextInfo.SubmitChanges();

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin cán bộ thành công", "Thông báo", "success");
                        SetEnableForm(false);

                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textUsername.Focus();
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

        protected void ddlistDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachPhongBan();
            string vMatKhau = textMatKhau.Text;
            string vXacNhanMatKhau = textXacNhanMatKhau.Text;
            textMatKhau.Attributes.Add("value", vMatKhau);
            textXacNhanMatKhau.Attributes.Add("value", vXacNhanMatKhau);
        }

        /// <summary>
        /// Load danh sách Phòng ban
        /// </summary>
        /// <returns></returns>
        public void LoadDanhSachPhongBan()
        {
            try
            {
                List<PhongBan> vPhongBanInfos = new List<PhongBan>();
                ddlistPhongBan.Items.Clear();
                if (ddlistDonVi.SelectedValue != "" && ddlistDonVi.SelectedValue != null)
                {
                    int vDonViSelected = Int32.Parse(ddlistDonVi.SelectedValue);
                    vPhongBanInfos = vDataContext.PhongBans.Where(x => x.DONVI_ID == vDonViSelected).OrderBy(x => x.TENPHONGBAN).ToList();
                    ddlistPhongBan.DataSource = vPhongBanInfos;
                    ddlistPhongBan.DataTextField = "TENPHONGBAN";
                    ddlistPhongBan.DataValueField = "PB_ID";
                    ddlistPhongBan.DataBind();
                }

                ddlistPhongBan.Items.Insert(0, new ListItem("Chọn phòng ban", ""));

            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        /// <summary>
        /// Load danh sách chức vụ
        /// </summary>
        public void LoadDanhSachChucVu()
        {
            try
            {
                List<CHUCVU> vChucVuInfos = new List<CHUCVU>();
                vChucVuInfos = vDataContext.CHUCVUs.OrderBy(x => x.TENCHUCVU).ToList();
                ddlistChucVu.Items.Clear();
                ddlistChucVu.DataSource = vChucVuInfos;
                ddlistChucVu.DataTextField = "TENCHUCVU";
                ddlistChucVu.DataValueField = "CV_ID";
                ddlistChucVu.DataBind();
                ddlistChucVu.Items.Insert(0, new ListItem("Chọn chức vụ", ""));
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
                List<DONVI> vDonViInfos = vDataContext.DONVIs.OrderBy(x => x.TENDONVI).ToList();
                ddlistDonVi.Items.Clear();
                ddlistDonVi.DataSource = vDonViInfos;
                ddlistDonVi.DataTextField = "TENDONVI";
                ddlistDonVi.DataValueField = "DONVI_ID";
                ddlistDonVi.DataBind();
                ddlistDonVi.Items.Insert(0, new ListItem("Chọn đơn vị", ""));
                LoadDanhSachPhongBan();
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }


       

    }
}
