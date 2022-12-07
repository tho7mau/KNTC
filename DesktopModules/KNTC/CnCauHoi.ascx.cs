#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật câu hỏi
/// Ngày tạo        :03/04/2020
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

namespace HOPKHONGGIAY
{
    public partial class CnCauHoi : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vCauHoiId;
        int vBieuQuyetId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        ChucVuController vChucVuControllerInfo = new ChucVuController();
        CauHoiController vCauHoiControllerInfo = new CauHoiController();
        string vMacAddress = ClassCommon.GetMacAddress();
        string objListItem;
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
                    vCauHoiId = int.Parse(Request.QueryString["id"]);
                }
                if (Request.QueryString["bieuquyetid"] != null)
                {
                    vBieuQuyetId = int.Parse(Request.QueryString["bieuquyetid"]);
                }
                if (!IsPostBack)
                {
                    SetFormInfo(vCauHoiId);
                    textCauhoi.Focus();
                }
                //Edit Title
                var vBieuQuyetInfo = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == vBieuQuyetId).SingleOrDefault();

                if (vCauHoiId == 0)//Them moi
                {
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý biểu quyết</a> / " + "<a href='" + Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thông tin biểu quyết", "id=" + vBieuQuyetId) + "' class='title-link'>" + vBieuQuyetInfo.NOIDUNGBIEUQUYET + " </a> / Thêm mới câu hỏi";
                }
                else
                {
                    var vCauHoiInfo = vCauHoiControllerInfo.GetCauHoiTheoId(vCauHoiId);
                    if (vCauHoiInfo != null)
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý biểu quyết</a> / " + "<a href='" + Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thông tin biểu quyết", "id=" + vBieuQuyetId) + "' class='title-link'>" + vBieuQuyetInfo.NOIDUNGBIEUQUYET + "</a> / Thông tin câu hỏi";
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
                CapNhat(vCauHoiId);
            }
        }


        /// <summary>
        /// Event nhan button Bo Qua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBoQua_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Thông tin biểu quyết", "id=" + vBieuQuyetId);
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
            string vToastrMessage = "";
            if (textCauhoi.Text == "")
            {
                textCauhoi.CssClass += " vld";
                textCauhoi.Focus();
                labelCauHoi.Attributes["class"] += " vld";
                vToastrMessage += "nhập Câu hỏi, ";
                vResult = false;
            }
            else
            {
                textCauhoi.CssClass = textCauhoi.CssClass.Replace("vld", "").Trim();
                labelCauHoi.Attributes.Add("class", labelCauHoi.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (listBoxDapAn.Items.Count == 0)
            {
                textDapAnMoi.CssClass += " vld";
                textDapAnMoi.Focus();
                labelDapAnMoi.Attributes["class"] += " vld";
                vToastrMessage += "thêm đáp án cho Câu hỏi , ";
                vResult = false;
            }
            else
            {
                textDapAnMoi.CssClass = textDapAnMoi.CssClass.Replace("vld", "").Trim();
                textDapAnMoi.Attributes.Add("class", labelDapAnMoi.Attributes["class"].ToString().Replace("vld", ""));
            }

            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, "Vui lòng " + vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
            }
            return vResult;
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("edit_cauhoi", "mid=" + this.ModuleId, "title=Thêm mới câu hỏi", "id=0", "bieuquyetid=" + vBieuQuyetId);
            Response.Redirect(vUrl);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set thông tin cho form
        /// </summary>
        /// <param name="pChucVuId"></param>
        public void SetFormInfo(int pCauHoiId)
        {
            try
            {
                if (pCauHoiId == 0)//Thêm mới
                {
                    btnSua.Visible = false;
                    btnCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    if (KiemTraCauHoiDaThucHienBieuQuyet(vCauHoiId))
                    {
                        buttonThemmoi.Visible = false;
                        btnSua.Visible = false;
                    }
                    var vCauHoiInfo = vCauHoiControllerInfo.GetCauHoiTheoId(vCauHoiId);
                    if (vCauHoiInfo != null)
                    {
                        textCauhoi.Text = vCauHoiInfo.NOIDUNG;
                        textSoThuTu.Text = vCauHoiInfo.THUTU.ToString();
                        var lisdapan = vCauHoiControllerInfo.GetDapAnCauHoi(vCauHoiId);
                        foreach (var it in lisdapan)
                        {
                            listBoxDapAn.Items.Add(new ListItem(it.NOIDUNG, listBoxDapAn.Items.Count.ToString()));
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
            textCauhoi.Enabled = pEnableStatus;
            textSoThuTu.Enabled = pEnableStatus;
            textDapAnMoi.Enabled = pEnableStatus;
            listBoxDapAn.Enabled = pEnableStatus;
            btnAddDapAn.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin chức vụ
        /// </summary>
        /// <param name="pChucVuId"></param>
        public void CapNhat(int pCauHoiId)
        {
            try
            {
                string oErrorMessage = "";
                if (pCauHoiId == 0)//Thêm mới
                {
                    CAUHOI vCauHoiInfo = new CAUHOI();
                    vCauHoiInfo.NOIDUNG = ClassCommon.ClearHTML(textCauhoi.Text.Trim());
                    //vCauHoiInfo.THUTU = Convert.ToInt32(ClassCommon.ClearHTML(textSoThuTu.Text.Trim()));
                    vCauHoiInfo.BIEUQUYET_ID = vBieuQuyetId;
                    vCauHoiInfo.PHIENHOP_ID = (int)vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == vCauHoiInfo.BIEUQUYET_ID).FirstOrDefault().PHIENHOP_ID;
                    int oCauhoiId = 0;
                    vCauHoiControllerInfo.ThemMoiCauHoi(vCauHoiInfo, out oCauhoiId, out oErrorMessage);

                    if (oCauhoiId > 0)
                    {
                        // thêm đáp án cho câu hỏi
                        List<DAPANCAUHOI> LstDapAn = new List<DAPANCAUHOI>();
                        int ida = 0;
                        if (listBoxDapAn.Items.Count > 0)
                        {
                            foreach (ListItem item in listBoxDapAn.Items)
                            {
                                ida++;
                                DAPANCAUHOI objDapAn = new DAPANCAUHOI();

                                objDapAn.CAUHOI_ID = vDataContext.CAUHOIs.OrderByDescending(x => x.CAUHOI_ID).First().CAUHOI_ID;
                                // objOtion.ViewOrder = Int32.Parse(vieworder.Text);
                                objDapAn.BIEUQUYET_ID = vBieuQuyetId;
                                objDapAn.NOIDUNG = ClassCommon.ClearHTML(item.Text.Trim());
                                objDapAn.DAPAN_THUTU = ida;
                                LstDapAn.Add(objDapAn);
                            }
                            vDataContext.DAPANCAUHOIs.InsertAllOnSubmit(LstDapAn);
                            vDataContext.SubmitChanges();
                        }
                        //else
                        //{
                        //    pnlThongBao.Visible = true;
                        //    lblThongBao.Text = "Vui lòng nhập đáp án cho câu hỏi";
                        //}
                        // end thêm đáp án cho câu hỏi

                        string vUrl = Globals.NavigateURL("edit_cauhoi", "mid=" + this.ModuleId, "title=Thông tin câu hỏi", "id=" + oCauhoiId, "bieuquyetid=" + vBieuQuyetId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới đơn vị thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }

                }
                else //Cập nhật
                {
                    var vCauHoiInfo = vCauHoiControllerInfo.GetCauHoiTheoId(vCauHoiId);
                    if (vCauHoiInfo != null)
                    {
                        vCauHoiInfo.NOIDUNG = ClassCommon.ClearHTML(textCauhoi.Text.Trim());
                        //vCauHoiInfo.THUTU = Convert.ToInt32(ClassCommon.ClearHTML(textSoThuTu.Text.Trim()));
                        vCauHoiInfo.BIEUQUYET_ID = vBieuQuyetId;
                        vCauHoiInfo.PHIENHOP_ID = (int)vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == vCauHoiInfo.BIEUQUYET_ID).FirstOrDefault().PHIENHOP_ID;
                        vDataContext.SubmitChanges();
                        // thêm đáp án cho câu hỏi

                        // Xóa đáp án hiện tại
                        //var objDACH = vCauHoiControllerInfo.GetDapAnCauHoi(vCauHoiId);
                        string vErrorMessage = "";
                        vCauHoiControllerInfo.XoaDapAn(vCauHoiId, out vErrorMessage);
                        List<DAPANCAUHOI> LstDapAn = new List<DAPANCAUHOI>();
                        int ida = 0;
                        if (listBoxDapAn.Items.Count > 0)
                        {
                            foreach (ListItem item in listBoxDapAn.Items)
                            {
                                ida++;
                                DAPANCAUHOI objDapAn = new DAPANCAUHOI();

                                objDapAn.CAUHOI_ID = vCauHoiId;
                                objDapAn.BIEUQUYET_ID = vBieuQuyetId;
                                objDapAn.NOIDUNG = ClassCommon.ClearHTML(item.Text.Trim());
                                objDapAn.DAPAN_THUTU = ida;
                                LstDapAn.Add(objDapAn);
                            }
                            vDataContext.DAPANCAUHOIs.InsertAllOnSubmit(LstDapAn);
                            vDataContext.SubmitChanges();
                        }
                        //else
                        //{
                        //    pnlThongBao.Visible = true;
                        //    lblThongBao.Text = "Vui lòng nhập đáp án cho câu hỏi";
                        //}
                        // end thêm đáp án cho câu hỏi

                        ClassCommon.ShowToastr(Page, "Cập nhật câu hỏi thành công", "Thông báo", "success");
                        SetEnableForm(false);

                        buttonThemmoi.Visible = true;
                        btnCapNhat.Visible = false;
                        btnSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        textCauhoi.Focus();
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

        #region Button       
        #endregion

        #region Đáp án câu hỏi
        protected void btnAddDapAn_Click(object sender, EventArgs e)
        {
            if (textDapAnMoi.Text != "")
            {
                textDapAnMoi.CssClass = textDapAnMoi.CssClass.Replace("vld", "").Trim();
                textDapAnMoi.Attributes.Add("class", labelDapAnMoi.Attributes["class"].ToString().Replace("vld", ""));
                bool isUnique = true;
                for (int item = 0; item < listBoxDapAn.Items.Count - 1; item++)
                {
                    if (listBoxDapAn.Items[item].ToString().ToLower() == textDapAnMoi.Text.ToLower())
                    {
                        isUnique = false;
                    }
                }

                if (isUnique)
                {
                    string strOption = textDapAnMoi.Text;
                    listBoxDapAn.Items.Add(new ListItem(strOption, listBoxDapAn.Items.Count.ToString()));

                }
                textDapAnMoi.Text = "";
            }
            else
            {
                ClassCommon.ShowToastr(Page, "Vui lòng nhập nội dung đáp án", "Thông báo", "error");
                textDapAnMoi.CssClass += " vld";
                textDapAnMoi.Focus();
                labelDapAnMoi.Attributes["class"] += " vld";
            }
        }
        /// <summary>
        /// sửa thông tin câu hỏi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Visible = false;
            btnCapNhat.Visible = true;
            buttonThemmoi.Visible = false;
            SetEnableForm(true);
        }
        protected void btn_cmdUp(object sender, EventArgs e)
        {
            if (listBoxDapAn.SelectedIndex > 0)
            {
                int intSelectedIndex = listBoxDapAn.SelectedIndex;
                objListItem = listBoxDapAn.Items[listBoxDapAn.SelectedIndex].ToString();
                //listBoxDapAn.Items.Remove(objListItem);
                listBoxDapAn.Items.RemoveAt(listBoxDapAn.SelectedIndex);
                listBoxDapAn.Items.Insert(intSelectedIndex - 1, objListItem);
            }
        }

        protected void btn_cmdDown(object sender, EventArgs e)
        {
            if (listBoxDapAn.SelectedIndex < listBoxDapAn.Items.Count - 1)
            {
                int intSelectedIndex = listBoxDapAn.SelectedIndex;
                objListItem = listBoxDapAn.Items[listBoxDapAn.SelectedIndex].ToString();
                //listBoxDapAn.Items.Remove(objListItem);
                listBoxDapAn.Items.RemoveAt(listBoxDapAn.SelectedIndex);
                listBoxDapAn.Items.Insert(intSelectedIndex + 1, objListItem);
            }
        }

        protected void btn_cmdDeleteOption(object sender, EventArgs e)
        {
            if (listBoxDapAn.SelectedIndex != -1)
            {
                listBoxDapAn.Items.RemoveAt(listBoxDapAn.SelectedIndex);
            }
        }

        public bool KiemTraCauHoiDaThucHienBieuQuyet(int CauHoiID)
        {
            string vErrorMessage = "";
            return vCauHoiControllerInfo.KiemTraCauHoiSuDung(CauHoiID, out vErrorMessage);
        }
        #endregion
    }
}
