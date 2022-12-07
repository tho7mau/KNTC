#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật người dùng
/// Ngày tại        :15/08/2019
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CnNguoiDung : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vNguoiDungID;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        NguoiDungController vNguoiDungControllerInfo = new NguoiDungController();
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
                    LoadDanhSachDonVi();
                    LoadDanhSachChucVu();
                    SetFormInfo(vNguoiDungID);
                    textTenDangNhap.Focus();

                }
                //Edit Title
                if (vNguoiDungID == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý người dùng</a> / Thêm mới";
                }
                else
                {
                    var vNguoiDungInfo = vNguoiDungControllerInfo.GetNguoiDungTheoID(vNguoiDungID);
                    if (vNguoiDungInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý người dùng</a> / " + vNguoiDungInfo.TENNGUOIDUNG;
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
                CapNhat(vNguoiDungID);
            }
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


        /// <summary>       
        /// Validate form và hiện thông báo lỗi
        /// </summary>       
        /// <returns>true: Form không có lỗi; flase: form có trường lỗi</returns>        
        /// <returns></returns>
        protected Boolean ValidateForm()
        {
            Boolean vResult = true;
            Boolean vResult_Username = true;
            Boolean vResult_Password = true;
            string vToastrMessage = "";
            string vToastrMessageUsername = "";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";
            if (vNguoiDungID == 0)//Thêm mới người dùng
            {
                if (textTenDangNhap.Text == "")
                {
                    textTenDangNhap.CssClass += " vld";
                    textTenDangNhap.Focus();
                    labelTenDangNhap.Attributes["class"] += " vld";
                    vToastrMessage += "Tên đăng nhập, ";
                    vResult = false;
                }
                else
                {
                    textTenDangNhap.CssClass = textTenDangNhap.CssClass.Replace("vld", "").Trim();
                    labelTenDangNhap.Attributes.Add("class", labelTenDangNhap.Attributes["class"].ToString().Replace("vld", ""));
                }

                if (textTenDangNhap.Text.Length < 5)
                {
                    textTenDangNhap.CssClass += " vld";
                    textTenDangNhap.Focus();
                    labelTenDangNhap.Attributes["class"] += " vld";
                    vToastrMessage += "Tên đăng nhập lớn hơn 4 kí tự, ";
                    vResult = false;
                }
            }


            if (textMatKhau.Text == "" && vNguoiDungID == 0)
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


            if (textXacNhanMatKhau.Text == "" && vNguoiDungID == 0)
            {
                textXacNhanMatKhau.CssClass += " vld";
                textXacNhanMatKhau.Focus();
                labelXacNhanMatKhau.Attributes["class"] += " vld";
                vToastrMessage += "Xác nhận mật khẩu, ";
                vResult = false;
            }
            else
            {
                textXacNhanMatKhau.CssClass = textXacNhanMatKhau.CssClass.Replace("vld", "").Trim();
                labelXacNhanMatKhau.Attributes.Add("class", labelXacNhanMatKhau.Attributes["class"].ToString().Replace("vld", ""));
            }


            if (textHoTenNguoiDung.Text == "")
            {
                textHoTenNguoiDung.CssClass += " vld";
                textHoTenNguoiDung.Focus();
                labelHoTen.Attributes["class"] += " vld";
                vToastrMessage += "Họ và tên, ";
                vResult = false;
            }
            else
            {
                textHoTenNguoiDung.CssClass = textHoTenNguoiDung.CssClass.Replace("vld", "").Trim();
                labelHoTen.Attributes.Add("class", labelHoTen.Attributes["class"].ToString().Replace("vld", ""));
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

            if (vNguoiDungID == 0)//Thêm mới người dùng
            {
                if ((textMatKhau.Text != "" && textXacNhanMatKhau.Text != "") && textXacNhanMatKhau.Text != textMatKhau.Text)
                {
                    textXacNhanMatKhau.CssClass += " vld";
                    textXacNhanMatKhau.Focus();
                    labelXacNhanMatKhau.Attributes["class"] += " vld";
                    vToastrMessagePassword = "Mật khẩu xác nhận không chính xác ";
                    vResult_Password = false;
                }


                if (vNguoiDungControllerInfo.KiemTraTrungUsername(ClassCommon.ClearHTML(textTenDangNhap.Text.Trim()), out oErrorMessage))
                {
                    textTenDangNhap.CssClass += " vld";
                    textTenDangNhap.Focus();
                    labelTenDangNhap.Attributes["class"] += " vld";
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
                if (pNguoiDungId == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    panelMatKhau.Visible = false;
                    var vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDungId).FirstOrDefault();
                    if (vNguoiDungInfo != null)
                    {
                        textTenDangNhap.Text = vNguoiDungInfo.Username;
                        textHoTenNguoiDung.Text = vNguoiDungInfo.TENNGUOIDUNG;
                        textEmail.Text = vNguoiDungInfo.EMAIL;
                        textSoDienThoai.Text = vNguoiDungInfo.SODIENTHOAI;
                        ddlistDonVi.SelectedValue = vNguoiDungInfo.DONVI_ID.ToString();
                        LoadDanhSachPhongBan();
                        if (vNguoiDungInfo.PB_ID == null)
                        {
                            ddlistPhongBan.SelectedValue = "";
                        }
                        else
                        {
                            ddlistPhongBan.SelectedValue = vNguoiDungInfo.PB_ID.ToString();
                        }

                        ddlistChucVu.SelectedValue = vNguoiDungInfo.CV_ID.ToString();
                        RoleController vRoleControllerInfo = new RoleController();
                        var vUserDnnInfo = UserController.GetUserById(this.PortalId, vNguoiDungInfo.UserId ?? 0);
                        if (vUserDnnInfo != null)
                        {
                            if (vUserDnnInfo.IsInRole("CHUYENVIEN"))
                            {
                                checkboxQuyenChuyenVien.Checked = true;
                            }
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
            panelMatKhau.Visible = false;
            if (vNguoiDungID != 0)
            {
                textTenDangNhap.Enabled = false;
            }
            else
            {
                textTenDangNhap.Enabled = pEnableStatus;
            }

            textMatKhau.Enabled = pEnableStatus;
            textXacNhanMatKhau.Enabled = pEnableStatus;
            textHoTenNguoiDung.Enabled = pEnableStatus;
            textEmail.Enabled = pEnableStatus;
            textSoDienThoai.Enabled = pEnableStatus;
            ddlistChucVu.Enabled = pEnableStatus;
            ddlistPhongBan.Enabled = pEnableStatus;
            ddlistDonVi.Enabled = pEnableStatus;
            checkboxQuyenChuyenVien.Enabled = pEnableStatus;
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

                if (pNguoiDungID == 0)//Thêm mới người dùng
                {
                    NGUOIDUNG vNguoiDungInfo = new NGUOIDUNG();

                    vNguoiDungInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.DaiBieu;

                    vNguoiDungInfo.Username = ClassCommon.ClearHTML(textTenDangNhap.Text.Trim());
                    vNguoiDungInfo.TENNGUOIDUNG = ClassCommon.ClearHTML(textHoTenNguoiDung.Text.Trim());
                    vNguoiDungInfo.EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                    vNguoiDungInfo.SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                    vNguoiDungInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                    vNguoiDungInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);

                    vNguoiDungInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.DaiBieu;
                    vNguoiDungInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                    int oNguoiDungID = 0;
                    vNguoiDungControllerInfo.ThemMoiNguoiDung(vNguoiDungInfo, PortalSettings, this.PortalId, ClassCommon.ClearHTML(textMatKhau.Text.Trim()), checkboxQuyenChuyenVien.Checked, out oNguoiDungID);
                    if (oNguoiDungID > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin đơn vị", "id=" + oNguoiDungID);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới người dùng thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);

                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật thông tin người dùng
                {
                    var vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDungID).SingleOrDefault();
                    if (vNguoiDungInfo != null)
                    {
                        //Cập nhật bảng người dùng
                        vNguoiDungInfo.Username = ClassCommon.ClearHTML(textTenDangNhap.Text.Trim());
                        vNguoiDungInfo.TENNGUOIDUNG = ClassCommon.ClearHTML(textHoTenNguoiDung.Text.Trim());
                        vNguoiDungInfo.EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                        vNguoiDungInfo.SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                        vNguoiDungInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                        vNguoiDungInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);
                        vNguoiDungInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                        vDataContext.SubmitChanges();
                        RoleController vRoleControllerInfo = new RoleController();
                        //Kiểm tra cập nhật lại quyền chuyên viên
                        var vUserDnnInfo = UserController.GetUserById(this.PortalId, vNguoiDungInfo.UserId ?? 0);
                        if (checkboxQuyenChuyenVien.Checked)
                        {
                            if (vUserDnnInfo != null)
                            {
                                if (!vUserDnnInfo.IsInRole("CHUYENVIEN"))
                                {
                                    RoleInfo vRoleInfo_ChuyenVien = vRoleControllerInfo.GetRoleByName(this.PortalId, "CHUYENVIEN"); //Add người dùng vào role CHUYENVIEN - RoleGroup: HOPKHONGGIAY
                                    RoleController.AddUserRole(vUserDnnInfo, vRoleInfo_ChuyenVien, this.PortalSettings, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
                                }
                            }
                        }
                        else
                        {
                            vUserDnnInfo = UserController.GetUserById(this.PortalId, vNguoiDungInfo.UserId ?? 0);
                            if (vUserDnnInfo != null)
                            {
                                if (vUserDnnInfo.IsInRole("CHUYENVIEN"))
                                {
                                    RoleInfo vRoleInfo_ChuyenVien = vRoleControllerInfo.GetRoleByName(this.PortalId, "CHUYENVIEN"); //Xóa người dùng vào role CHUYENVIEN - RoleGroup: HOPKHONGGIAY
                                    RoleController.DeleteUserRole(vUserDnnInfo, vRoleInfo_ChuyenVien, this.PortalSettings, false);
                                }
                            }
                        }
                        //Cập nhật username
                        UserController.ChangeUsername(vNguoiDungInfo.UserId ?? 0, ClassCommon.ClearHTML(textTenDangNhap.Text.Trim()));
                        //Cập nhật Display name bảng User dnn
                        var vUserInfo = vUserDataContextInfo.Users.Where(x => x.UserID == vNguoiDungInfo.UserId).SingleOrDefault();
                        vUserInfo.DisplayName = vNguoiDungInfo.TENNGUOIDUNG;
                        vUserDataContextInfo.SubmitChanges();

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin người dùng thành công", "Thông báo", "success");
                        SetEnableForm(false);

                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;
                        btnCapNhat.Visible = false;
                        btnSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenDangNhap.Focus();
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
                LoadDanhSachPhongBan();


            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
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
        #endregion

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("Edit", "mid=" + this.ModuleId, "title=Thêm mới người dùng", "id=0");
            Response.Redirect(vUrl);
        }



        protected void ddlistDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachPhongBan();
            string vMatKhau = textMatKhau.Text;
            string vXacNhanMatKhau = textXacNhanMatKhau.Text;
            textMatKhau.Attributes.Add("value", vMatKhau);
            textXacNhanMatKhau.Attributes.Add("value", vXacNhanMatKhau);
        }
    }
}
