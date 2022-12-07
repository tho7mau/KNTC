#region
// File name    :   DIAPHUONG_CN.ascx.cs
// Purpose      :   Cập nhật, Thêm mới 
// Create date  :   09/05/2016
// Update date  :   09/05/2016
// Author       :   MVBAO
// Version      :   v1.0
// Copyright    :   LIINK
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace KNTC
{
    public partial class DIAPHUONG_CN : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vDiaPhuongID;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        DIAPHUONGController vDIAPHUONGController = new DIAPHUONGController();
        DIAPHUONGController DIAPHUONGController = new DIAPHUONGController();
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
                    vDiaPhuongID = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachLoaiDonViCapCha();
                    SetFormInfo(vDiaPhuongID);
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
                CapNhat(vDiaPhuongID);
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
        /// Load danh sách đơn vị cấp cha
        /// </summary>
        /// <returns></returns>
        public void LoadDanhSachLoaiDonViCapCha()
        {
            try
            {
                List<DIAPHUONG> vDIAPHUONGInfos = new List<DIAPHUONG>();
                if (vDiaPhuongID > 0)
                {
                    vDIAPHUONGInfos = vDataContext.DIAPHUONGs.Where(x => x.DP_ID != vDiaPhuongID).OrderBy(x => x.INDEX_ID).ToList();
                }
                else
                {
                    vDIAPHUONGInfos = vDataContext.DIAPHUONGs.OrderBy(x => x.INDEX_ID).ToList();
                }

                if (vDIAPHUONGInfos.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("DP_ID");
                    dt.Columns.Add("DP_TEN");

                    foreach (var ldt in vDIAPHUONGInfos)
                    {
                        DataRow row = dt.NewRow();
                        row["DP_ID"] = ldt.DP_ID;
                        if (ldt.CapDo == 2)
                        {
                            row["DP_TEN"] = "__" + ldt.DP_TEN;
                        }
                        else
                        {
                            if (ldt.CapDo == 3)
                            {
                                row["DP_TEN"] = "_____" + ldt.DP_TEN;
                            }
                            else
                            {
                                row["DP_TEN"] = ldt.DP_TEN;

                            }
                        }
                        dt.Rows.Add(row);
                    }

                    ddlistDIAPHUONG_CapTren.Items.Clear();
                    ddlistDIAPHUONG_CapTren.DataSource = dt;
                    ddlistDIAPHUONG_CapTren.DataTextField = "DP_TEN";
                    ddlistDIAPHUONG_CapTren.DataValueField = "DP_ID";
                    ddlistDIAPHUONG_CapTren.DataBind();
                    ddlistDIAPHUONG_CapTren.Items.Insert(0, new ListItem("Không có", "0"));
                }
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
            string vToastrMessagePassword = "";
            string oErrorMessage = "";
            if (txtTenDiaPhuong.Text == "")
            {
                txtTenDiaPhuong.CssClass += " vld";
                txtTenDiaPhuong.Focus();
                labelTenDiaPhuong.Attributes["class"] += " vld";
                vToastrMessage += " Tên đơn vị hành chính, ";
                vResult = false;
            }
            else
            {
                txtTenDiaPhuong.CssClass = txtTenDiaPhuong.CssClass.Replace("vld", "").Trim();
                labelTenDiaPhuong.Attributes.Add("class", labelTenDiaPhuong.Attributes["class"].ToString().Replace("vld", ""));
            }
            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _User, vToastrMessage.Substring(0, vToastrMessage.Length - 2) + ".", "Thông báo", "error");
            }
            return vResult;
        }

        protected void buttonThemmoi_Click(object sender, EventArgs e)
        {
            string vUrl = Globals.NavigateURL("create_update", "mid=" + this.ModuleId, "title=Thêm mới đơn vị hành chính", "id=0");
            Response.Redirect(vUrl);
        }
        #endregion


        #region Methods
        /// <summary>
        ///  Set thông tin cho form
        /// </summary>
        /// <param name="pDIAPHUONGId"></param>
        public void SetFormInfo(int pDIAPHUONGId)
        {
            try
            {
                if (pDIAPHUONGId == 0)//Thêm mới
                {
                    labelTen.Text = "Thêm mới";
                    buttonSua.Visible = false;
                    buttonCapNhat.Visible = true;
                    buttonThemmoi.Visible = false;
                }
                else
                {
                    SetEnableForm(false);
                    var vDIAPHUONGInfo = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == pDIAPHUONGId).FirstOrDefault();
                    if (vDIAPHUONGInfo != null)
                    {
                        labelTen.Text = vDIAPHUONGInfo.DP_TEN;
                        txtTenDiaPhuong.Text = vDIAPHUONGInfo.DP_TEN;
                        ddlistDIAPHUONG_CapTren.SelectedValue = vDIAPHUONGInfo.DP_ID_CHA.ToString();
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
            txtTenDiaPhuong.Enabled = pEnableStatus;
            ddlistDIAPHUONG_CapTren.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin địa phương
        /// </summary>
        /// <param name="pThietBiId"></param>
        public void CapNhat(int pThietBiId)
        {
            try
            {
                string oErrorMessage = "";

                if (pThietBiId == 0)//Thêm mới địa phương
                {
                    int vDiaPhuongID_Cha = int.Parse(ddlistDIAPHUONG_CapTren.SelectedValue);

                    DIAPHUONG vDIAPHUONGInfo = new DIAPHUONG();

                    vDIAPHUONGInfo.DP_TEN = ClassCommon.ClearHTML(txtTenDiaPhuong.Text.Trim());

                    if (vDiaPhuongID_Cha > 0)
                    {
                        DIAPHUONG vDIAPHUONG_Cha = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == int.Parse(ddlistDIAPHUONG_CapTren.SelectedValue)).FirstOrDefault();
                        vDIAPHUONGInfo.DP_ID_CHA = vDIAPHUONG_Cha.DP_ID;
                        vDIAPHUONGInfo.CapDo = vDIAPHUONG_Cha.CapDo + 1;
                    }
                    else
                    {
                        vDIAPHUONGInfo.DP_ID_CHA = 0;
                        vDIAPHUONGInfo.CapDo = 1;
                    }
                    int pDIAPHUONGId = 0;
                    vDIAPHUONGController.ThemMoiDIAPHUONG(vDIAPHUONGInfo, out pDIAPHUONGId, out oErrorMessage);
                    if (pDIAPHUONGId > 0)
                    {
                        string vUrl = Globals.NavigateURL("create_update", "mid=" + this.ModuleId, "title=Cập nhật thông tin đơn vị hành chính", "id=" + pDIAPHUONGId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới đơn vị hành chính thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin
                {
                    var vDIAPHUONGInfo = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == vDiaPhuongID).FirstOrDefault();
                    if (vDIAPHUONGInfo != null)
                    {
                        int vDiaPhuongID_Cha = int.Parse(ddlistDIAPHUONG_CapTren.SelectedValue);
                        vDIAPHUONGInfo.DP_TEN = ClassCommon.ClearHTML(txtTenDiaPhuong.Text.Trim());
                        if (vDiaPhuongID_Cha > 0)
                        {
                            DIAPHUONG vDIAPHUONG_Cha = vDataContext.DIAPHUONGs.Where(x => x.DP_ID == int.Parse(ddlistDIAPHUONG_CapTren.SelectedValue)).FirstOrDefault();
                            vDIAPHUONGInfo.DP_ID_CHA = vDIAPHUONG_Cha.DP_ID;
                            vDIAPHUONGInfo.CapDo = vDIAPHUONG_Cha.CapDo + 1;
                        }
                        else
                        {
                            vDIAPHUONGInfo.DP_ID_CHA = 0;
                            vDIAPHUONGInfo.CapDo = 1; 
                        }
                        if (vDIAPHUONGInfo.DP_ID_CHA > 0)
                        {
                            var vDIAPHUONG_CapTren = vDIAPHUONGController.GetDIAPHUONG_By_ID(vDIAPHUONGInfo.DP_ID_CHA);
                            if (vDIAPHUONG_CapTren != null)
                            {
                                vDIAPHUONGInfo.INDEX_ID = vDIAPHUONG_CapTren.INDEX_ID + vDiaPhuongID.ToString() + ".";
                            }
                        }
                        vDIAPHUONGController.CAPNHAT_DP(vDIAPHUONGInfo);

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin đơn vị hành chính thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        txtTenDiaPhuong.Focus();
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