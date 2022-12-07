#region Thông tin chung
/// Mục đích        :Cập nhật thông báo trong phiên họp
/// Ngày tại        :21/07/2020
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
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    public partial class CnPhienHop_GoiThongBao : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        CauHoiController vCauHoiControllerInfo = new CauHoiController();
        PhienHopController vPhienHopControllerInfo = new PhienHopController();
        BieuQuyetController vBieuQuyetControllerInfo = new BieuQuyetController();
        ThongBaoController vThongBaoControllerInfo = new ThongBaoController();
        DuyetPhienHopController vDuyetPhienHopControllerInfo = new DuyetPhienHopController();
        PhienHopController vPhienHopController = new PhienHopController();

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
                    vPhienHopId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo();

                }
                //Edit Title

                this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / Gửi thông báo cuộc họp";
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }
        /// <summary>
        /// Thêm mới biểu quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thêm mới phiên họp", "id=0");
            Response.Redirect(vUrl);
        }

        /// <summary>
        /// Set trạng thái visible form
        /// </summary>
        /// <param name="pEnableStatus"></param>
        public void SetEnableForm(bool pEnableStatus)
        {
            textTieuDe.Enabled = pEnableStatus;
            textNoidung.Enabled = pEnableStatus;
            dtpickerThoiGianGoi.Enabled = pEnableStatus;
            //btn_ThemMoi.Visible = pEnableStatus;
            //dgDanhSach.Columns[0].Visible = pEnableStatus;                 
        }

        /// <summary>
        /// Sửa thông tin biểu quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
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
            string vUrl = Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Chi tiết phiên họp", "id=" + vPhienHopId);
            Response.Redirect(vUrl);
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

            if (textTieuDe.Text == "")
            {
                textTieuDe.CssClass += " vld";
                textTieuDe.Focus();
                labelTieuDe.Attributes["class"] += " vld";
                vToastrMessage += "Tiêu đề, ";
                vResult = false;
            }
            else
            {
                textTieuDe.CssClass = textTieuDe.CssClass.Replace("vld", "").Trim();
                labelTieuDe.Attributes.Add("class", labelTieuDe.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (textNoidung.Text == "")
            {
                textNoidung.CssClass += " vld";
                textNoidung.Focus();
                labelNoiDung.Attributes["class"] += " vld";
                vToastrMessage += "Nội dung, ";
                vResult = false;
            }
            else
            {
                textNoidung.CssClass = textNoidung.CssClass.Replace("vld", "").Trim();
                labelNoiDung.Attributes.Add("class", labelNoiDung.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (checkboxGoiNgay.Checked == false && dtpickerThoiGianGoi.SelectedDate == null)
            {
                dtpickerThoiGianGoi.CssClass += " vld";
                dtpickerThoiGianGoi.Focus();
                labelNgayGoi.Attributes["class"] += " vld";
                vToastrMessage += "Thời gian gửi, ";
                vResult = false;
            }
            else
            {
                dtpickerThoiGianGoi.CssClass = dtpickerThoiGianGoi.CssClass.Replace("vld", "").Trim();
                labelNgayGoi.Attributes.Add("class", labelNgayGoi.Attributes["class"].ToString().Replace("vld", ""));
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
        public void SetFormInfo()
        {
            dtpickerThoiGianGoi.SelectedDate = DateTime.Now;
        }

        public void SetEmptyForm()
        {
            textTieuDe.Text = "";
            textNoidung.Text = "";
        }
        /// <summary>
        /// Cập nhật thông tin phiên họp
        /// </summary>
        /// <param name="pPhienHopId"></param>
        public void CapNhat()
        {
            try
            {
                List<int> vNguoiDungIds_NhanThongBao = new List<int>();              
                vNguoiDungIds_NhanThongBao = vDuyetPhienHopControllerInfo.GetDanhSachNguoiDungIdByPhienHopId(vPhienHopId);
                if (vNguoiDungIds_NhanThongBao.Count > 0)
                {
                    //Gởi thông báo Push - Notification
                    HKG_THONGBAO vThongBaoInfo = new HKG_THONGBAO();
                    vThongBaoInfo.Title = ClassCommon.ClearHTML(textTieuDe.Text.Trim());
                    vThongBaoInfo.Content = ClassCommon.ClearHTML(textNoidung.Text.Trim());
                    vThongBaoInfo.CreateDate = DateTime.Now;
                    var vSendDate = checkboxGoiNgay.Checked ? DateTime.Now : dtpickerThoiGianGoi.SelectedDate;
                    vThongBaoInfo.SendDate = vSendDate;
                    vThongBaoInfo.Type = checkboxGoiNgay.Checked ? (int)CommonEnum.KieuThongBao.TucThoi : (int)CommonEnum.KieuThongBao.DinhThoi;
                    vThongBaoInfo.Kind = (int)CommonEnum.LoaiThongBao.ThongBaoHeThong;
                    vThongBaoInfo.PhienHop_Id = vPhienHopId;
                    bool vResultGoiThongBao = false;

                    var vTieuDePhienHop = vPhienHopController.GetPhienHopTheoId(vPhienHopId);


                    string vData = "\"data\": {\"PhienHopId\": "  + vPhienHopId +  ", \"Loai\": \"phienhop\", \"TieuDePhienHop\": \"" + vTieuDePhienHop.TIEUDE + "\"},";

                    vResultGoiThongBao = vThongBaoControllerInfo.SendNotifications(vNguoiDungIds_NhanThongBao,
                                                                                    checkboxGoiNgay.Checked ? "" : String.Format("{0: yyyy-MM-dd HH:mm:ss}", dtpickerThoiGianGoi.SelectedDate),
                                                                                    vThongBaoInfo.Content, vData, "", "");

                    int vThongBaoId = 0;
                    if (vResultGoiThongBao)
                    {
                        vThongBaoInfo.Status = (int)CommonEnum.TrangThaiThongBao.DaGui;
                        vThongBaoId = vThongBaoControllerInfo.InsertThongBao(vThongBaoInfo);
                        if (vThongBaoId > 0)
                        {
                            List<THONGBAO_NGUOIDUNG> vThongBaoNguoiDungInfos = new List<THONGBAO_NGUOIDUNG>();
                            foreach (var NguoiDungId in vNguoiDungIds_NhanThongBao)
                            {
                                THONGBAO_NGUOIDUNG vThongBaoNguoiDungInfo = new THONGBAO_NGUOIDUNG();
                                vThongBaoNguoiDungInfo.NguoiDungId = NguoiDungId;
                                vThongBaoNguoiDungInfo.ThongBaoId = vThongBaoId;
                                vThongBaoNguoiDungInfo.TrangThaiGuiThongBao = true;
                                vThongBaoNguoiDungInfo.DaXemNoiDung = false;
                                vThongBaoNguoiDungInfos.Add(vThongBaoNguoiDungInfo);
                            }
                            if (vThongBaoNguoiDungInfos.Count > 0)
                            {
                                vThongBaoControllerInfo.InsertThongBao_NguoiDungs(vThongBaoNguoiDungInfos);
                            }
                            Session[vMacAddress + TabId.ToString() + "_Message"] = "Gửi thông báo thành công";
                            Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                            string vUrl = Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Chi tiết phiên họp", "id=" + vPhienHopId);
                            Response.Redirect(vUrl);
                        }
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Gửi thông báo không thành công.", "Thông báo", "error");
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
                Response.Redirect(Globals.NavigateURL(), false);
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
        /// Load danh sách đại biểu
        /// </summary>
        /// <returns></returns>

        #endregion       

        protected void CheckboxGoiNgay_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxGoiNgay.Checked)
            {
                divNgayGoi.Visible = false;
            }
            else
            {
                divNgayGoi.Visible = true;
            }
        }
    }
}
