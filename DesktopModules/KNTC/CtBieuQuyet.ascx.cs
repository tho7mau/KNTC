#region Thông tin chung
/// Mục đích        :Kết quả biểu quyết
/// Ngày tại        :02/07/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HOPKHONGGIAY
{
    /// <summary>
    /// Xử lý dữ liệu danh sách câu hỏi
    /// </summary>

    public partial class CtBieuQuyet : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPageSize = ClassParameter.vPageSize;
        public string vPathCommonJS = ClassParameter.vPathCommonJavascript;
        int vStt = 1;
        int vCurentPage = 0;
        int vBieuQuyetId;
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        PhienHopNguoiDungController vPhienHopNguoiDungControllerInfo = new PhienHopNguoiDungController();
        string vMacAddress = ClassCommon.GetMacAddress();
        HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        #endregion
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Kiem tra quyen dang nhap, phan quyen
                //CheckAccountLogin();
                //Hien thong bao neu co loi xay ra
                ShowMessage();
                //Get PageSize, Current Page
                //Edit Title
                //this.ModuleConfiguration.ModuleTitle = "Quản lý câu hỏi";

                if (Request.QueryString["id"] != null)
                {
                    vBieuQuyetId = int.Parse(Request.QueryString["id"]);
                }
                var vBieuQuyetInfo = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == vBieuQuyetId).FirstOrDefault();
                if (vBieuQuyetInfo != null)
                {
                    this.ModuleConfiguration.ContainerPath = "/Portals/_default/Containers/DLK_Container/";
                    this.ModuleConfiguration.ContainerSrc = "/Portals/_default/Containers/DLK_Container/Default.ascx";
                    this.ModuleConfiguration.ModuleControl.ControlTitle = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý biểu quyết</a> / " + vBieuQuyetInfo.NOIDUNGBIEUQUYET + " / Kết quả biểu quyết";
                    //dnnTITLE. = "<a href='" + Globals.NavigateURL() + "' class='title-link'>Quản lý phiên họp</a> / " + "<a href='" + Globals.NavigateURL("chitiet", "mid=" + this.ModuleId, "title=Thông tin phiên họp", "id=" + vPhienHopId) + "' class='title-link'>" + vPhienHopInfo.TIEUDE + "</a> / Điểm danh";
                }

                if (!IsPostBack)
                {
                    try
                    {
                        SetFormInfo(vBieuQuyetId);
                    }
                    catch (Exception ex)
                    {
                        ClassCommon.ShowToastr(Page, ex + "", "Thông báo lỗi", "error");
                        //log.Error("", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xữ lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
                //log.Error("", ex);
            }
        }

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
        #region Methods      
        /// <summary>
        /// Session Destroy
        /// </summary>
        /// <param name="pArr"></param>
        public void SetFormInfo(int pBieuQuyetId)
        {
            try
            {
                //Tab biểu quyết
                string vBieuQuyetHTML = "";
                var vBieuQuyetInfo = vDataContext.BIEUQUYETs.Where(x => x.BIEUQUYET_ID == pBieuQuyetId).FirstOrDefault();
                if (vBieuQuyetInfo != null)
                {
                    //Danh sách biểu quyết
                    //foreach (var vBieuQuyetInfo in vBieuQuyetInfos)
                    //{
                    vBieuQuyetHTML += "<div class='panel panel-white panel-collapse-heading'>";
                    vBieuQuyetHTML += "<div class='panel-heading pd-8' role='tab' id='heading'>";
                    string vCollapeName = "collapse" + vBieuQuyetInfo.BIEUQUYET_ID.ToString();
                    vBieuQuyetHTML += "<a role='button' tabindex='-1' data-toggle='collapse' href='#" + vCollapeName + "' id='tt' aria-expanded='false' aria-controls='" + vCollapeName + "' class='btnCollapse'>";
                    vBieuQuyetHTML += "<h4 class='panel-title'>" + vBieuQuyetInfo.NOIDUNGBIEUQUYET + "</h4>";
                    vBieuQuyetHTML += "</a>";
                    vBieuQuyetHTML += "</div>";
                    vBieuQuyetHTML += "</div>";

                    vBieuQuyetHTML += "<div id='" + vCollapeName + "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='heading2' aria-expanded='false'>";
                    vBieuQuyetHTML += " <div class='form-group pd-l10'>";

                    var vCauHoiInfos = vDataContext.CAUHOIs.Where(x => x.BIEUQUYET_ID == vBieuQuyetInfo.BIEUQUYET_ID).ToList();
                    if (vCauHoiInfos.Count > 0)
                    {
                        //Danh sách câu hỏi
                        foreach (var vCauHoiInfo in vCauHoiInfos)
                        {
                            vBieuQuyetHTML += "<div class='form-group mr-l10 mr-r20 pd-t10 pd-b10' style='border: 1px solid #f1f1f1'>";
                            vBieuQuyetHTML += "<div class='col-sm-6'>";
                            vBieuQuyetHTML += "<label class='control-label pd-r0'>" + vCauHoiInfo.NOIDUNG + "</label>";
                            vBieuQuyetHTML += "</div>";

                            var vDapAnInfos = vDataContext.DAPANCAUHOIs.Where(x => x.CAUHOI_ID == vCauHoiInfo.CAUHOI_ID).ToList();
                            if (vDapAnInfos.Count > 0)
                            {
                                //Danh sách đáp án
                                vBieuQuyetHTML += "<div class='col-sm-6'>";
                                var vDapAnBieuQuyetInfos = vDataContext.DAIBIEU_BIEUQUYETs.Where(x => x.PHIENHOP_ID == vBieuQuyetInfo.PHIENHOP_ID && x.DAPANCAUHOI.CAUHOI_ID == vCauHoiInfo.CAUHOI_ID).ToList();
                                double vTongSoDaiBieuThamGiaBieuQuyet = vDapAnBieuQuyetInfos.Count();
                                if (vTongSoDaiBieuThamGiaBieuQuyet > 0)
                                {
                                    foreach (var vDapAnInfo in vDapAnInfos)
                                    {
                                        float vSoNguoiChon = vDapAnBieuQuyetInfos.Where(x => x.DAPANTRALOI_ID == vDapAnInfo.DAPANCAUHOI_ID).Count();
                                        double vTiLeChon = Math.Round((vSoNguoiChon / vTongSoDaiBieuThamGiaBieuQuyet) * 100);
                                        vBieuQuyetHTML += "<div class='col-sm-12 form-group'>";
                                        vBieuQuyetHTML += "<a data='" + vDapAnInfo.DAPANCAUHOI_ID + "' onclick='return HienThiDanhSachNguoiChon(this);'>";

                                        vBieuQuyetHTML += "<label class='pd-r0 col-sm-6' style='font-weight: normal'>" + vDapAnInfo.NOIDUNG + " (" + vSoNguoiChon + "/" + vTongSoDaiBieuThamGiaBieuQuyet + ") " + "</label>";

                                        vBieuQuyetHTML += "<div class='progress col-sm-6 pd-l0 pd-r0'>";
                                        vBieuQuyetHTML += " <div class='progress-bar' role='progressbar' aria-valuenow='" + vTiLeChon + "' aria-valuemin='0' aria-valuemax='100' style='min-width:2em; width: " + vTiLeChon + "%;'>";
                                        vBieuQuyetHTML += vTiLeChon + "% ";
                                        vBieuQuyetHTML += "</div>";
                                        vBieuQuyetHTML += "</div>";
                                        vBieuQuyetHTML += "</a>";
                                        vBieuQuyetHTML += "</div>";
                                       
                                    }
                                }
                                else
                                {
                                    foreach (var vDapAnInfo in vDapAnInfos)
                                    {
                                        vBieuQuyetHTML += "<div class='progress'>";
                                        vBieuQuyetHTML += " <div class='progress-bar pd-l10' role='progressbar' aria-valuenow='0' min-width='20' aria-valuemin='0' aria-valuemax='100' style='width: 100%; text-align:left'>";
                                        vBieuQuyetHTML += "&nbsp; " + vDapAnInfo.NOIDUNG + " -  Chưa biểu quyết";
                                        vBieuQuyetHTML += "</div>";
                                        vBieuQuyetHTML += "</div>";
                                    }
                                }

                                vBieuQuyetHTML += "</div>";
                            }
                            else
                            {
                                vBieuQuyetHTML += "Câu hỏi chưa có đáp án";
                            }
                            vBieuQuyetHTML += "</div>";

                        }
                    }
                    else
                    {
                        vBieuQuyetHTML += "Biểu quyết chưa có câu hỏi";
                    }

                    vBieuQuyetHTML += "</div>";
                    vBieuQuyetHTML += "</div>";
                    //}

                    textNoiDungBieuQuyet.Text = vBieuQuyetHTML;
                }
                else
                {
                    vBieuQuyetHTML = "Phiên họp không có biểu quyết";
                }
                //End tab biểu quyết
            }
            catch (Exception ex)
            {

            }
        }


        protected void LoadChiTietBieuQuyet(object sender, EventArgs e)
        {
            try
            {
                int vDapAnId = 0;
                if (lbDapAnId.Value != "")
                {
                    vDapAnId = int.Parse(lbDapAnId.Value);
                }
                if (vDapAnId > 0)
                {
                    int vStt = 1;
                    string vResultHtml = "";
                    var vDanhSachDaiBieuChonDapAn = (from DaiBieu_BQ in vDataContext.DAIBIEU_BIEUQUYETs
                                                     join DaiBieu in vDataContext.NGUOIDUNGs on DaiBieu_BQ.DAIBIEU_ID equals DaiBieu.NGUOIDUNG_ID
                                                     where DaiBieu_BQ.DAPANTRALOI_ID == vDapAnId
                                                     select DaiBieu).ToList();
                    var vDapAnInfo = vDataContext.DAPANCAUHOIs.Where(x => x.DAPANCAUHOI_ID == vDapAnId).SingleOrDefault();
                    if (vDanhSachDaiBieuChonDapAn.Count > 0)
                    {
                        vResultHtml += "<h5>Danh sách Đại biểu lựa chọn <b>" + vDapAnInfo.NOIDUNG + "</b> cho câu hỏi <b>" + vDapAnInfo.CAUHOI.NOIDUNG + ": </b></h5><br/>";
                        foreach (var DaiBieu in vDanhSachDaiBieuChonDapAn)
                        {

                            vResultHtml +=  vStt + ". " + "<b>" + DaiBieu.TENNGUOIDUNG + "</b>" + " - " + DaiBieu.CHUCVU.TENCHUCVU + " " + DaiBieu.DONVI.TENDONVI + "<br/>";
                            vStt++;
                        }
                    }
                    else
                    {
                        vResultHtml += "<h5>Không có đại biểu nào chọn đáp án này.</h5>";
                    }
                    labelNoiDungChiTietBieuQuyet.Text = vResultHtml;

                }
                if (ViewState["OpenModalBieuQuyet"] == null)
                {
                    ViewState["OpenModalBieuQuyet"] = "Open";
                }
                ViewState["Tab"] = "BIEUQUYET";
                OpenModalBieuQuyet();
                //Reload_TabBieuQuyet();


            }
            catch (Exception ex)
            {

            }
        }

        public void OpenModalBieuQuyet()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Reload", "OpenModalBieuQuyet();", true);
        }

        #endregion


        #region Button

        /// <summary>
        /// Event nhan button Bo Qua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBoQua_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
        }

        #endregion

    }
}
