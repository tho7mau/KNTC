using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_TRUNGDON_NEW : DotNetNuke.Entities.Modules.UserModuleBase
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
            if (!Page.IsPostBack)
            {
                Get_Cache();
                SetDate();                   
                LoadDanhSach(_StartPage, _EndPage);
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
                        Byte[] fileBytes = baoCaoController.TK_DONTHU_TRUNG_NEW(date_tu.Text, date_den.Text, ddlistLoaiDoiTuong.SelectedValue);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_TRUNGDON_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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

        protected Boolean ValidateForm()
        {
            Boolean vResult = true;
            string vToastrMessage = "";
            if (string.IsNullOrEmpty(date_tu.Text))
            {               
                 vToastrMessage += "- Vui lòng chọn Từ ngày<br/>";              
                 vResult = false;
            }
            // Địa phương
            if (string.IsNullOrEmpty(date_den.Text))
            {
                vToastrMessage += "- Vui lòng chọn Đến ngày<br/>";
                vResult = false;
            }
            if(vResult == true)
            {
                if (Convert.ToDateTime(date_tu.Text) > Convert.ToDateTime(date_den.Text))
                {
                    vToastrMessage += "- Vui lòng chọn từ ngày lớn hơn đến ngày";
                    vResult = false;
                }
            }
            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, vToastrMessage, "Thông báo", "error");
            }
            return vResult;
        }
        protected void SetDate()
        {
            date_tu.Text = "01/01/" + DateTime.Now.Year;
            date_den.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                int pLoaiDoiTuong = Convert.ToInt32(ddlistLoaiDoiTuong.SelectedValue);
                List<DONTHU> lstDonThuTrung = new List<DONTHU>();
                List<DONTHU> objDT = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Date > Convert.ToDateTime(date_tu.Text))
                                                                            && (x.NGAYTAO.Value.Date < Convert.ToDateTime(date_den.Text))
                                                                            && (pLoaiDoiTuong == 0 || x.DOITUONG.DOITUONG_LOAI == pLoaiDoiTuong)
                                                                             ).OrderByDescending(x=>x.DONTHU_STT).ToList();

                List<long> DONTHU_REMOVE_ID = new List<long>();
                // Kiểm tra đơn thư trùng 
                foreach (var item in objDT)
                {
                    if (DONTHU_REMOVE_ID.Count == 0 || (DONTHU_REMOVE_ID.Count > 0 && !DONTHU_REMOVE_ID.Contains(item.DONTHU_ID)))
                    {
                        var objTRung = objDT.Where(x => x.DONTHU_ID != item.DONTHU_ID && x.DONTHU_NOIDUNG == item.DONTHU_NOIDUNG && x.DOITUONG_ID == item.DOITUONG_ID).ToList();
                        if (objTRung.Count > 0)
                        {
                            lstDonThuTrung.Add(item);
                            lstDonThuTrung.AddRange(objTRung);
                            DONTHU_REMOVE_ID.AddRange(objTRung.Select(x => x.DONTHU_ID).ToList());
                        }
                    }
                }

                int TotalRow = 0;
                if (lstDonThuTrung.Count > 0)
                {
                    TotalRow = lstDonThuTrung.Count();
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
                lstDonThuTrung = lstDonThuTrung.Skip(v_start - 1).Take(v_end - (v_start - 1)).ToList();
                dgDanhSach.DataSource = lstDonThuTrung;
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