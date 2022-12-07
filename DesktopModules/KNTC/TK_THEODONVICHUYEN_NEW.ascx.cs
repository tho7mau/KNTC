using DotNetNuke.Entities.Users;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_THEODONVICHUYEN_NEW : DotNetNuke.Entities.Modules.UserModuleBase
    {
        int vPageSize = 10;
        int vCurentPage = 0;
        int _StartPage = 1;
        int _EndPage = 10;
        KNTCDataContext vDC = new KNTCDataContext();
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        BaoCaoController baoCaoController = new BaoCaoController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInfoDateTime();
                LoadDanhSachDonVi();
            }
        }
        /// <summary>
        /// Load danh sách đơn vị vào dropdown
        /// </summary>
        public void LoadDanhSachDonVi()
        {
            try
            {
                var vDonViInfos = vDC.DONVIs.OrderBy(x => x.DONVI_ID).ToList();
                if (vDonViInfos.Count > 0)
                {

                    ddlistCoQuanTiepNhan.Items.Clear();
                    ddlistCoQuanTiepNhan.DataSource = vDonViInfos;
                    ddlistCoQuanTiepNhan.DataTextField = "TENDONVI";
                    ddlistCoQuanTiepNhan.DataValueField = "DONVI_ID";
                    ddlistCoQuanTiepNhan.DataBind();                

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnXuatExel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {


                    if (ValidateForm() == true)
                    {
                        Byte[] fileBytes = baoCaoController.TK_DONVICHUYEN_NEW(textNhanDonTuNgay.Text, textNhanDonDenNgay.Text, textDonChuyenTuNgay.Text, textDonChuyenDenNgay.Text, textHanGiaiQuyetTuNgay.Text, textHanGiaiQuyetDenNgay.Text, ddlistCoQuanTiepNhan.SelectedValue, ddlistTrangThaiGiaiQuyet.SelectedValue);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_DONTHUTHEODONVICHUYEN_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
                                     + DateTime.Now.Year + ".xlsx");
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";
                            StringWriter sw = new StringWriter();
                            Response.BinaryWrite(fileBytes);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Không có dữ liệu xuất thống kê.", "Thông báo", "error");
                        }
                    }                  
                }
                catch (Exception ex)
                {
                    ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, Vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
                }

            }
        }

        public void SetInfoDateTime()
        {
            textNhanDonTuNgay.Text = "01/01/" + DateTime.Now.Year;
            textNhanDonDenNgay.Text =  DateTime.Now.ToString("dd/MM/yyyy");
            textDonChuyenTuNgay.Text = "01/01/" + DateTime.Now.Year;
            textDonChuyenDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textHanGiaiQuyetTuNgay.Text = "01/01/" + DateTime.Now.Year;
            textHanGiaiQuyetDenNgay.Text = "31/12/" + DateTime.Now.Year; 

        }
        protected Boolean ValidateForm()
        {
            Boolean vResult = true;
            string vToastrMessage = "";
            if (string.IsNullOrEmpty(textNhanDonTuNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Nhận đơn từ ngày <br/>";
                vResult = false;
            }
            if (string.IsNullOrEmpty(textNhanDonDenNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Nhận đơn đến ngày <br/>";
                vResult = false;
            }

            if (string.IsNullOrEmpty(textDonChuyenTuNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Chuyển đơn từ ngày <br/>";
                vResult = false;
            }
            if (string.IsNullOrEmpty(textDonChuyenDenNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Chuyển đơn đến ngày <br/>";
                vResult = false;
            }
            if (string.IsNullOrEmpty(textHanGiaiQuyetTuNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Hạn giải quyết từ ngày <br/>";
                vResult = false;
            }
            if (string.IsNullOrEmpty(textHanGiaiQuyetDenNgay.Text))
            {
                vToastrMessage += "- Vui lòng chọn Hạn giải quyết đến ngày <br/>";
                vResult = false;
            }

            if ( !string.IsNullOrEmpty(textNhanDonTuNgay.Text) && !string.IsNullOrEmpty(textNhanDonDenNgay.Text) && (Convert.ToDateTime(textNhanDonTuNgay.Text) > Convert.ToDateTime(textNhanDonDenNgay.Text)))
            {
                vToastrMessage += "- Vui lòng chọn Nhận đơn từ ngày lớn hơn Nhận đơn đến ngày <br/>";
                vResult = false;
            }
            if (!string.IsNullOrEmpty(textDonChuyenTuNgay.Text) && !string.IsNullOrEmpty(textDonChuyenDenNgay.Text) && (Convert.ToDateTime(textDonChuyenTuNgay.Text) > Convert.ToDateTime(textDonChuyenDenNgay.Text)))
            {
                vToastrMessage += "- Vui lòng chọn Chuyển đơn từ ngày lớn hơn Chuyển đơn đến ngày <br/>";
                vResult = false;
            }
            if (!string.IsNullOrEmpty(textHanGiaiQuyetTuNgay.Text) && !string.IsNullOrEmpty(textHanGiaiQuyetDenNgay.Text) && (Convert.ToDateTime(textHanGiaiQuyetTuNgay.Text) > Convert.ToDateTime(textHanGiaiQuyetDenNgay.Text)))
            {
                vToastrMessage += "- Vui lòng chọn Hạn giải quyết từ ngày lớn hơn Hạn giải quyết đến ngày <br/>";
                vResult = false;
            }

            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, vToastrMessage, "Thông báo", "error");
            }
            return vResult;
        }
        #region Phan trang
        ///// <summary>
        ///// Get PageSize, Current Page
        ///// </summary>
        protected void Get_Cache()
        {

            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] != null)
            {
                vPageSize = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"].ToString());
            }
            if (Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"] != null)
            {
                vCurentPage = Int32.Parse(Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_CurrenPage"].ToString());
            }
        }
        protected void dgDanhSach_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }
        protected void txtRecordStartEnd_TextChanged(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, false, false);
            txtRecordStartEnd.Text = valueStartEnd;

            vPageSize = o_PageSize;
            Session[PortalSettings.ActiveTab.TabID + _currentUser.UserID + "_PageSize"] = o_PageSize;
            LoadDanhSach(start, end);


        }
        protected void LinkButtonPrevious_Click(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, true, false);
            txtRecordStartEnd.Text = valueStartEnd;
            vPageSize = o_PageSize;
            LoadDanhSach(start, end);
        }

        protected void LinkButtonLast_Click(object sender, EventArgs e)
        {
            int o_PageSize;
            int start;
            int end;
            string valueStartEnd;
            ClassCommon.checkEnterStartEnd(txtRecordStartEnd.Text, lblTotalRecords.Text, vPageSize, out o_PageSize, out start, out end, out valueStartEnd, false, true);
            txtRecordStartEnd.Text = valueStartEnd;
            vPageSize = o_PageSize;
            LoadDanhSach(start, end);
        }
        public void LoadDanhSach(int v_start, int v_end)
        {
            if (ValidateForm() == true)
            {
                List<int> DONVI_ID_TIEPNHANs = new List<int>();

                string DONVI_IDs_CHUYEN = ddlistCoQuanTiepNhan.SelectedValue;
                string NhanDonTuNgay = textNhanDonTuNgay.Text;
                string NhanDonDenNgay = textNhanDonDenNgay.Text;
                string DonChuyenTuNgay = textDonChuyenTuNgay.Text;
                string DonChuyenDenNgay = textDonChuyenDenNgay.Text;
                string HanGiaiQuyetTuNgay = textHanGiaiQuyetTuNgay.Text;
                string HanGiaiQuyetDenNgay = textHanGiaiQuyetDenNgay.Text;
                string GIAIQUYETTRANGTHAI_ID = ddlistTrangThaiGiaiQuyet.SelectedValue;

                //Đơn thư có hướng xử lý là chuyển đơn
                var objDonThu = vDC.DONTHUs.Where(x => x.HUONGXULY_ID == 3 && x.HUONGXULY_DONVI_ID == Convert.ToInt32(DONVI_IDs_CHUYEN)).ToList();
                // Ngày nhận đơn
                if (NhanDonTuNgay != "" && NhanDonDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateTime.ParseExact(NhanDonTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.NGAYTAO <= DateTime.ParseExact(NhanDonDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                // Ngày chuyển đơn
                if (DonChuyenTuNgay != "" && DonChuyenDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.HUONGXULY_NGAYCHUYEN >= DateTime.ParseExact(DonChuyenTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.HUONGXULY_NGAYCHUYEN <= DateTime.ParseExact(DonChuyenDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                // Thời hạn giải quyết

                if (HanGiaiQuyetTuNgay != "" && HanGiaiQuyetDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.HUONGXULY_THOIHANGIAIQUET >= DateTime.ParseExact(HanGiaiQuyetTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.HUONGXULY_THOIHANGIAIQUET <= DateTime.ParseExact(HanGiaiQuyetDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                if (GIAIQUYETTRANGTHAI_ID == "0")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == false || x.KETQUA_XYLY == null).ToList();
                }
                // Đã có kết quả giải quyết
                else if (GIAIQUYETTRANGTHAI_ID == "1")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == true).ToList();
                }

                int TotalRow = 0;
                if (objDonThu.Count > 0)
                {
                    TotalRow = objDonThu.Count();
                }
                else
                {
                    TotalRow = 0;
                    v_start = 0;
                    v_end = 0;
                }
                if (!ClassCommon.CheckAfterLoad_DataGrird(txtRecordStartEnd, LinkButtonPrevious, LinkButtonLast, TotalRow, v_start, v_end))
                {
                    v_start = TotalRow;
                    v_end = TotalRow;
                }
                objDonThu = objDonThu.OrderByDescending(x => x.DONTHU_STT).ToList();
                objDonThu = objDonThu.Skip(v_start - 1).Take(v_end - (v_start - 1)).ToList();
                dgDanhSach.DataSource = objDonThu;
                dgDanhSach.DataBind();
                lblTotalRecords.Text = TotalRow + "";
            }
        }

        public string getThongTinDoiTuong(int pTDOITUONG_ID)
        {
            try
            {
                string strDoiTuong = "";
                List<CANHAN> objCANHANs = vDC.CANHANs.Where(x => x.DOITUONG_ID == pTDOITUONG_ID).ToList();
                if (objCANHANs.Count > 0)
                {
                    for (int i = 0; i < objCANHANs.Count; i++)
                    {
                        string vSubName = "";
                        bool IsGetSubName = GetSubCharacterName(objCANHANs[i].CANHAN_HOTEN, out vSubName);
                        strDoiTuong = strDoiTuong + "<div>";
                        if (IsGetSubName)
                        {
                            strDoiTuong = strDoiTuong + "<button type='button' class='btn btn-block btn-primary btn -lg' style='width:40px; padding:5px; height:40px; border-radius:50%;float:left;margin-right:10px;margin-top:2px;margin-bottom:10px;'>";
                            strDoiTuong = strDoiTuong + vSubName + "</button>";
                        }
                        strDoiTuong = strDoiTuong + "<h6><b>" + objCANHANs[i].CANHAN_HOTEN + "</b></h6>";
                        strDoiTuong = strDoiTuong + "<p>" + objCANHANs[i].CANHAN_DIACHI_DAYDU + "</p>";
                        strDoiTuong = strDoiTuong + "</div>";
                    }
                }
                return strDoiTuong;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public bool GetSubCharacterName(string pName, out string oSubCharracter)
        {
            string vSubCharracter = "";

            if (pName.Contains(" "))
            {
                pName = pName.Trim();
                var vSubCharracter_arr = pName.Split(' ');
                if (vSubCharracter_arr.Count() >= 2)
                {
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[vSubCharracter_arr.Count() - 2].Substring(0, 1);
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[vSubCharracter_arr.Count() - 1].Substring(0, 1);

                    oSubCharracter = vSubCharracter;
                    return true;
                }
                else if (vSubCharracter_arr.Count() == 1)
                {
                    vSubCharracter = vSubCharracter + vSubCharracter_arr[0].Substring(0, 1);
                    oSubCharracter = vSubCharracter;
                    return true;
                }
                else
                {
                    oSubCharracter = vSubCharracter;
                    return false;
                }
            }
            else if (pName.Length >= 1)
            {
                vSubCharracter = pName.Substring(0, 1);
                oSubCharracter = vSubCharracter;
                return true;
            }
            else
            {
                oSubCharracter = vSubCharracter;
                return false;
            }
        }
        public string GetTenLoaiDonThuById(string pLOAIDONTHU_ID)
        {
            try
            {
                string vTenLoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == int.Parse(pLOAIDONTHU_ID)).FirstOrDefault().LOAIDONTHU_TEN;
                return vTenLoaiDonThu;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string GetCoQuanChuyenDon(string vCoQuanId)
        {
            try
            {
                string vTenCoQuan = "";
                vTenCoQuan = vDC.DONVIs.Where(x => x.DONVI_ID == int.Parse(vCoQuanId)).FirstOrDefault().TENDONVI;
                return vTenCoQuan;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDanhSach(1, vPageSize);
        }
        #endregion
    }
}
