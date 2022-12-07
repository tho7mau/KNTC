#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật loại đơn thư
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
    public partial class CnLoaiDonThu : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vLoaiDonThuId;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();
        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        KNTCDataContext vDataContext = new KNTCDataContext();
        LoaiDonThuController vLoaiDonThuController = new LoaiDonThuController();
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
                    vLoaiDonThuId = int.Parse(Request.QueryString["id"]);
                }
                if (!IsPostBack)
                {
                    LoadDanhSachLoaiDonViCapCha();
                    SetFormInfo(vLoaiDonThuId);
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
                CapNhat(vLoaiDonThuId);
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
                List<LOAIDONTHU> vLoaiDonThuInfos = new List<LOAIDONTHU>();
                if(vLoaiDonThuId > 0)
                {
                    vLoaiDonThuInfos = vDataContext.LOAIDONTHUs.Where(x=>x.LOAIDONTHU_ID != vLoaiDonThuId).OrderBy(x => x.LOAIDONTHU_INDEX).ToList();
                }
                else
                {
                    vLoaiDonThuInfos = vDataContext.LOAIDONTHUs.OrderBy(x => x.LOAIDONTHU_INDEX).ToList();
                }
                
                if(vLoaiDonThuInfos.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("LOAIDONTHU_ID");
                    dt.Columns.Add("LOAIDONTHU_TEN");

                    foreach (var ldt in vLoaiDonThuInfos)
                    {
                        DataRow row = dt.NewRow();
                        row["LOAIDONTHU_ID"] = ldt.LOAIDONTHU_ID;
                        if (ldt.LOAIDONTHU_CAP == 2)
                        {
                            row["LOAIDONTHU_TEN"] = "__" + ldt.LOAIDONTHU_TEN;
                        }
                        else
                        {
                            if(ldt.LOAIDONTHU_CAP == 3)
                            {
                                row["LOAIDONTHU_TEN"] = "_____" + ldt.LOAIDONTHU_TEN;
                            }
                            else
                            {
                                row["LOAIDONTHU_TEN"] =  ldt.LOAIDONTHU_TEN;

                            }
                        }
                        dt.Rows.Add(row);
                    }

                    ddlistLoaiDonThu_CapTren.Items.Clear();
                    ddlistLoaiDonThu_CapTren.DataSource = dt;
                    ddlistLoaiDonThu_CapTren.DataTextField = "LOAIDONTHU_TEN";
                    ddlistLoaiDonThu_CapTren.DataValueField = "LOAIDONTHU_ID";
                    ddlistLoaiDonThu_CapTren.DataBind();
                    ddlistLoaiDonThu_CapTren.Items.Insert(0, new ListItem("Không có", "0"));
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
            //if (textMaLoaiDonThu.Text == "")
            //{
            //    textMaLoaiDonThu.CssClass += " vld";
            //    textMaLoaiDonThu.Focus();
            //    labelMaLoaiDonThu.Attributes["class"] += " vld";
            //    vToastrMessage = " Mã loại đơn thư, ";
            //    vResult = false;
            //}
            //else
            //{
            //    textMaLoaiDonThu.CssClass = textMaLoaiDonThu.CssClass.Replace("vld", "").Trim();
            //    labelMaLoaiDonThu.Attributes.Add("class", labelMaLoaiDonThu.Attributes["class"].ToString().Replace("vld", ""));
            //}

            if (textTenLoaiDonThu.Text == "")
            {
                textTenLoaiDonThu.CssClass += " vld";
                textTenLoaiDonThu.Focus();
                labelTenLoaiDonThu.Attributes["class"] += " vld";
                vToastrMessage += " Tên loại đơn thư, ";
                vResult = false;
            }
            else
            {
                textTenLoaiDonThu.CssClass = textTenLoaiDonThu.CssClass.Replace("vld", "").Trim();
                labelTenLoaiDonThu.Attributes.Add("class", labelTenLoaiDonThu.Attributes["class"].ToString().Replace("vld", ""));
            }

            
            //if (vLoaiDonThuController.KiemTraTrungMaLoaiDonThu(vLoaiDonThuId,textMaLoaiDonThu.Text.Trim(), out oErrorMessage))
            //{
            //    textMaLoaiDonThu.CssClass += " vld";
            //    textMaLoaiDonThu.Focus();
            //    labelMaLoaiDonThu.Attributes["class"] += " vld";
            //    vToastrMessage = "Mã loại đơn thư đã tồn tại";
            //    vResult = false;
            //}
            //else
            //{
            //    textMaLoaiDonThu.CssClass = textMaLoaiDonThu.CssClass.Replace("vld", "").Trim();
            //    labelMaLoaiDonThu.Attributes.Add("class", labelMaLoaiDonThu.Attributes["class"].ToString().Replace("vld", ""));
            //}
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
                    var vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).FirstOrDefault();
                    if (vLoaiDonThuInfo != null)
                    {
                        labelTen.Text = vLoaiDonThuInfo.LOAIDONTHU_TEN;
                        //textMaLoaiDonThu.Text = vLoaiDonThuInfo.LOAIDONTHU_MA;
                        textTenLoaiDonThu.Text = vLoaiDonThuInfo.LOAIDONTHU_TEN;
                        textMoTaLoaiDonThu.Text = vLoaiDonThuInfo.LOAIDONTHU_MOTA;
                        ddlistLoaiDonThu_CapTren.SelectedValue = vLoaiDonThuInfo.LOAIDONTHU_CHA_ID.ToString();
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
            //textMaLoaiDonThu.Enabled = pEnableStatus;
            textTenLoaiDonThu.Enabled = pEnableStatus;
            ddlistLoaiDonThu_CapTren.Enabled = pEnableStatus;
            textMoTaLoaiDonThu.Enabled = pEnableStatus;
        }

        /// <summary>
        /// Cập nhật thông tin thiết bị
        /// </summary>
        /// <param name="pThietBiId"></param>
        public void CapNhat(int pThietBiId)
        {
            try
            {                
                string oErrorMessage = "";

                if (pThietBiId == 0)//Thêm mới thiết bị
                {
                    int vLoaiDonThuId_Cha = int.Parse(ddlistLoaiDonThu_CapTren.SelectedValue);
                 
                    LOAIDONTHU vLoaiDonThuInfo = new LOAIDONTHU();
                    //vLoaiDonThuInfo.LOAIDONTHU_MA = ClassCommon.ClearHTML(textMaLoaiDonThu.Text.Trim());
                   
                    vLoaiDonThuInfo.LOAIDONTHU_TEN = ClassCommon.ClearHTML(textTenLoaiDonThu.Text.Trim());
                    vLoaiDonThuInfo.LOAIDONTHU_MOTA = ClassCommon.ClearHTML(textMoTaLoaiDonThu.Text.Trim());
                    vLoaiDonThuInfo.NGAYTAO = DateTime.Now;
                    vLoaiDonThuInfo.NGUOITAO = _currentUser.UserID;

                    vLoaiDonThuInfo.NGAYCAPNHAT = DateTime.Now;
                    vLoaiDonThuInfo.NGUOICAPNHAT = _currentUser.UserID;

                    if (vLoaiDonThuId_Cha > 0)
                    {
                        LOAIDONTHU vLoaiDonThu_Cha = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == int.Parse(ddlistLoaiDonThu_CapTren.SelectedValue)).FirstOrDefault();
                        vLoaiDonThuInfo.LOAIDONTHU_CHA_ID = vLoaiDonThu_Cha.LOAIDONTHU_ID;
                        vLoaiDonThuInfo.LOAIDONTHU_CHA_TEN = vLoaiDonThu_Cha.LOAIDONTHU_TEN;
                        vLoaiDonThuInfo.LOAIDONTHU_CAP = vLoaiDonThu_Cha.LOAIDONTHU_CAP + 1;
                    }
                    else
                    {
                        vLoaiDonThuInfo.LOAIDONTHU_CHA_ID = 0;
                        vLoaiDonThuInfo.LOAIDONTHU_CAP = 1;
                        vLoaiDonThuInfo.LOAIDONTHU_CHA_TEN = "Không có";
                    }
                    int pLoaiDonThuId = 0;
                    vLoaiDonThuController.ThemMoiLoaiDonThu(vLoaiDonThuInfo, out pLoaiDonThuId, out oErrorMessage);
                    if (pLoaiDonThuId > 0)
                    {                        
                        string vUrl = Globals.NavigateURL("edit", "mid=" + this.ModuleId, "title=Cập nhật thông tin loại đơn thư", "id=" + pLoaiDonThuId);
                        Session[vMacAddress + TabId.ToString() + "_Message"] = "Thêm mới loại đơn thư thành công";
                        Session[vMacAddress + TabId.ToString() + "_Type"] = "success";
                        Response.Redirect(vUrl);
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                    }
                }
                else //Cập nhật thông tin thiết bị
                {
                    var vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == vLoaiDonThuId).FirstOrDefault();
                    if (vLoaiDonThuInfo != null)
                    {
                        int vLoaiDonThuId_Cha = int.Parse(ddlistLoaiDonThu_CapTren.SelectedValue);
                        //vLoaiDonThuInfo.LOAIDONTHU_MA = ClassCommon.ClearHTML(textMaLoaiDonThu.Text.Trim());
                        vLoaiDonThuInfo.LOAIDONTHU_TEN = ClassCommon.ClearHTML(textTenLoaiDonThu.Text.Trim());
                        vLoaiDonThuInfo.LOAIDONTHU_MOTA = ClassCommon.ClearHTML(textMoTaLoaiDonThu.Text.Trim());
                        vLoaiDonThuInfo.NGAYCAPNHAT = DateTime.Now;
                        vLoaiDonThuInfo.NGUOICAPNHAT = _currentUser.UserID;
                        if (vLoaiDonThuId_Cha > 0)
                        {
                            LOAIDONTHU vLoaiDonThu_Cha = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == int.Parse(ddlistLoaiDonThu_CapTren.SelectedValue)).FirstOrDefault();
                            vLoaiDonThuInfo.LOAIDONTHU_CHA_ID = vLoaiDonThu_Cha.LOAIDONTHU_ID;
                            vLoaiDonThuInfo.LOAIDONTHU_CHA_TEN = vLoaiDonThu_Cha.LOAIDONTHU_TEN;
                            vLoaiDonThuInfo.LOAIDONTHU_CAP = vLoaiDonThu_Cha.LOAIDONTHU_CAP + 1;
                        }
                        else
                        {
                            vLoaiDonThuInfo.LOAIDONTHU_CHA_ID = 0;
                            vLoaiDonThuInfo.LOAIDONTHU_CAP = 1;
                            vLoaiDonThuInfo.LOAIDONTHU_CHA_TEN = "Không có";
                        }
                        if(vLoaiDonThuInfo.LOAIDONTHU_CHA_ID > 0)
                        {
                            var vLoaiDonThu_CapTren = vLoaiDonThuController.GetLoaiDonThuTheoId(vLoaiDonThuInfo.LOAIDONTHU_CHA_ID ?? 0);
                            if(vLoaiDonThu_CapTren != null)
                            {
                                vLoaiDonThuInfo.LOAIDONTHU_INDEX = vLoaiDonThu_CapTren.LOAIDONTHU_INDEX + vLoaiDonThuId.ToString()+ ".";
                            }
                        }
                        vLoaiDonThuController.CapNhatLoaiDonThu(vLoaiDonThuId, vLoaiDonThuInfo, out oErrorMessage);

                        ClassCommon.ShowToastr(Page, "Cập nhật thông tin thiết bị thành công", "Thông báo", "success");
                        SetEnableForm(false);
                        buttonThemmoi.Visible = true;
                        buttonCapNhat.Visible = false;
                        buttonSua.Visible = true;
                    }
                    else
                    {
                        ClassCommon.ShowToastr(Page, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo", "error");
                        //textMaLoaiDonThu.Focus();
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
