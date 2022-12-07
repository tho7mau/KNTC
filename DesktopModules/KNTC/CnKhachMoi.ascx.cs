#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật khách mời
/// Ngày tại        :08/03/2019
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
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CnKhachMoi : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vKhachMoiId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        KhachMoiController vKhachMoiControllerInfo = new KhachMoiController();

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
                    vKhachMoiId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachDonVi();
                    LoadDanhSachChucVu();
                    SetFormInfo(vKhachMoiId);
                    textTenKhachMoi.Focus();
                   
                }
                //Edit Title
                if (vKhachMoiId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý khách mời</a> / Thêm mới";
                }
                else
                {
                    var vKhachMoiInfo = vKhachMoiControllerInfo.GetKhachMoiTheoId(vKhachMoiId);
                    if(vKhachMoiInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý khách mời</a> / " + vKhachMoiInfo.TENNGUOIDUNG;
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
                CapNhat(vKhachMoiId);
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
            Boolean vResult_Password = true;
            string vToastrMessage = "";
            string vToastrMessagePassword = "";
            string oErrorMessage = "";
            NguoiDungController vNguoiDungControllerInfo = new NguoiDungController();
            if (checkboxTaiKhoan.Checked)
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


                if (textMatKhau.Text == "" && vKhachMoiId == 0)
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


                if (textXacNhanMatKhau.Text == "" && vKhachMoiId == 0)
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

                if ((textMatKhau.Text != "" && textXacNhanMatKhau.Text != "") && textXacNhanMatKhau.Text != textMatKhau.Text)
                {
                    textXacNhanMatKhau.CssClass += " vld";
                    textXacNhanMatKhau.Focus();
                    labelXacNhanMatKhau.Attributes["class"] += " vld";
                    vToastrMessagePassword = "Mật khẩu xác nhận không chính xác ";
                    vResult_Password = false;
                }
                
                if (vNguoiDungControllerInfo.KiemTraTrungUsername( ClassCommon.ClearHTML(textTenDangNhap.Text.Trim()), out oErrorMessage))
                {
                    textTenDangNhap.CssClass += " vld";
                    textTenDangNhap.Focus();
                    labelTenDangNhap.Attributes["class"] += " vld";
                    vToastrMessagePassword = "Tên đăng nhập đã tồn tại ";
                    vResult_Password = false;
                }

            }
            if (textTenKhachMoi.Text == "")
            {
                textTenKhachMoi.CssClass += " vld";
                textTenKhachMoi.Focus();
                labelTenKhachMoi.Attributes["class"] += " vld";
                vToastrMessage += "Tên khách mời, ";
                vResult = false;
            }
            else
            {
                textTenKhachMoi.CssClass = textTenKhachMoi.CssClass.Replace("vld", "").Trim();
                labelTenKhachMoi.Attributes.Add("class", labelTenKhachMoi.Attributes["class"].ToString().Replace("vld", ""));
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
           
            //if (vNguoiDungControllerInfo.KiemTra (vKhachMoiId, ClassCommon.ClearHTML(textTenKhachMoi.Text.Trim()), out oErrorMessage))
            //{
            //    textTenKhachMoi.CssClass += " vld";
            //    textTenKhachMoi.Focus();
            //    labelTenKhachMoi.Attributes["class"] += " vld";
            //    vToastrMessagePassword = "Tên khách mời đã tồn tại ";
            //    vResult_Password = false;
            //}           

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

        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            buttonThemmoi.Visible = false;
            btnCapNhat.Visible = true;
            SetEnableForm(true);
            //checkboxTaiKhoan.Visible = false;
            if (Request.QueryString["id"] != null)
            {
                vKhachMoiId = int.Parse(Request.QueryString["id"]);
                if (vKhachMoiId != 0)
                {
                    var vKhachMoiInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == vKhachMoiId).FirstOrDefault();
                    if (vKhachMoiInfo.UserId != null)
                    {
                        checkboxTaiKhoan.Visible = false;
                        textTenDangNhap.Enabled = false;
                    }
                    else
                    {
                        checkboxTaiKhoan.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pKhachMoiId"></param>
        public void SetFormInfo(int pKhachMoiId)
        {
            try
            {
                if (pKhachMoiId == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    buttonThemmoi.Visible = false;
                    btnCapNhat.Visible = true;
                }
                else // Cập nhật
                {
                    SetEnableForm(false);
                    checkboxTaiKhoan.Visible = false;
                    var vKhachMoiInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pKhachMoiId).FirstOrDefault();
                    if (vKhachMoiInfo != null)
                    {
                        textTenKhachMoi.Text = vKhachMoiInfo.TENNGUOIDUNG;
                        textEmail.Text = vKhachMoiInfo.EMAIL;
                        textSoDienThoai.Text = vKhachMoiInfo.SODIENTHOAI;
                        ddlistDonVi.SelectedValue = vKhachMoiInfo.DONVI_ID.ToString();
                        
                        LoadDanhSachPhongBan();
                        if (vKhachMoiInfo.PB_ID == null)
                        {
                            ddlistPhongBan.SelectedValue = "";
                        }
                        else
                        {
                            ddlistPhongBan.SelectedValue = vKhachMoiInfo.PB_ID.ToString();
                        }
                        ddlistChucVu.SelectedValue = vKhachMoiInfo.CV_ID.ToString();
                        if(vKhachMoiInfo.UserId==null)
                        {
                            panelTaiKhoan.Visible = false;
                            textTenDangNhap.Enabled = true;
                        }
                        else
                        {
                            textTenDangNhap.Enabled = false;
                            textTenDangNhap.Text = vKhachMoiInfo.Username;
                            checkboxTaiKhoan.Checked = true;
                            checkboxTaiKhoan.Visible = false;
                            panelTaiKhoan.Visible = true;
                            panelMatKhau.Visible = false;
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
            
            textTenKhachMoi.Enabled = pEnableStatus;
            textEmail.Enabled = pEnableStatus;
            textSoDienThoai.Enabled = pEnableStatus;
            ddlistChucVu.Enabled = pEnableStatus;
            ddlistDonVi.Enabled = pEnableStatus;
            ddlistPhongBan.Enabled = pEnableStatus;
            textTenDangNhap.Enabled = pEnableStatus;


        }

        /// <summary>
        /// Cập nhật thông tin khách mời
        /// </summary>
        /// <param name="pKhachMoiId"></param>
        public void CapNhat(int pKhachMoiId)
        {
            try
            {
                var cultureInfo = new CultureInfo("vi-VN");
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                string vErrorMessage = "";
                if (pKhachMoiId == 0)//Thêm mới khách mời
                {                                                        
                    NGUOIDUNG vKhachMoiInfo = new NGUOIDUNG();
                    vKhachMoiInfo.TENNGUOIDUNG = ClassCommon.ClearHTML(textTenKhachMoi.Text.Trim());
                    vKhachMoiInfo.EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                    vKhachMoiInfo.SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                    vKhachMoiInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                    vKhachMoiInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                    vKhachMoiInfo.TRANGTHAI = true;
                    vKhachMoiInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.KhachMoi;
                    vKhachMoiInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);
                    if(checkboxTaiKhoan.Checked)
                    {
                        vKhachMoiInfo.Username = textTenDangNhap.Text;
                    }
                    int oKhachMoiId = 0;
                    string vMatkhau = textMatKhau.Text;
                    vKhachMoiControllerInfo.ThemMoiKhachMoi(vKhachMoiInfo,  PortalId, vMatkhau, out oKhachMoiId, out vErrorMessage);   
                    if(oKhachMoiId > 0)
                    {
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin khách mời", "id=" + oKhachMoiId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới khách mời thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin khách mời
                {
                    var vKhachMoiInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pKhachMoiId).SingleOrDefault();
                    if (vKhachMoiInfo != null)
                    {
                        vKhachMoiInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.KhachMoi;
                        vKhachMoiInfo.TENNGUOIDUNG = ClassCommon.ClearHTML(textTenKhachMoi.Text.Trim());
                        vKhachMoiInfo.EMAIL = ClassCommon.ClearHTML(textEmail.Text.Trim());
                        vKhachMoiInfo.SODIENTHOAI = ClassCommon.ClearHTML(textSoDienThoai.Text.Trim());
                        vKhachMoiInfo.CV_ID = int.Parse(ddlistChucVu.SelectedValue);
                        vKhachMoiInfo.PB_ID = int.Parse(ddlistPhongBan.SelectedValue);
                        vKhachMoiInfo.DONVI_ID = int.Parse(ddlistDonVi.SelectedValue);
                        
                        string vMatkhau = textMatKhau.Text;
                        if(textTenDangNhap.Text!=""&&textMatKhau.Text!=""&& vKhachMoiInfo.UserId==null && vKhachMoiInfo.Username==null)
                        {
                            int oUserId = 0;
                            vKhachMoiInfo.Username = textTenDangNhap.Text;
                            vKhachMoiControllerInfo.TaoTaiKhoanKhachMoi(vKhachMoiInfo,  PortalId, vMatkhau, out oUserId, out vErrorMessage);

                            vKhachMoiInfo.UserId = oUserId;
                        }
                        vDataContext.SubmitChanges();
                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin khách mời thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        panelMatKhau.Visible = false;
                        checkboxTaiKhoan.Visible = false;
                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;
                        btnSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textTenKhachMoi.Focus();
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
                vDonViInfos = vDataContext.DONVIs.OrderBy(x=>x.TENDONVI).ToList();
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
                vChucVuInfos = vDataContext.CHUCVUs.OrderBy(x=>x.TENCHUCVU).ToList();
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
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới khách mời", "id=0");
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

        protected void checkboxTaiKhoan_CheckedChanged(object sender, EventArgs e)
        {
            if(checkboxTaiKhoan.Checked)
            {
                panelTaiKhoan.Visible = true;
            }
            else
            {
                panelTaiKhoan.Visible = false;
            }
        }
    }
}
