using DotNetNuke.Entities.Users;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_DONTHU_NEW : DotNetNuke.Entities.Modules.UserModuleBase
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
                Get_Cache();
                SetInfoSearch();
                LoadDanhSach(_StartPage, _EndPage);
            }
        }
        /// <summary>
        /// Load danh sách đơn vị vào dropdown
        /// </summary>
       
        protected void btnXuatExel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (ValidateForm() == true)
                    {
                        List<DONTHU> lstDonThu = new List<DONTHU>();
                        List<int> _DP_ID = new List<int>();
                        int _NguonDon = Convert.ToInt32(ddlistNguon.SelectedValue);
                        int _LoaiDon = Convert.ToInt32(drlLoaiDon.SelectedValue);
                        int _DoiTuong = Convert.ToInt32(ddlistDoiTuong.SelectedValue);
                        int _HuongXuLy = Convert.ToInt32(ddlistHuongXuLy.SelectedValue);
                        int _TrangThaiGiaiQuyet = Convert.ToInt32(ddlistTrangThaiGiaiQuyet.SelectedValue);
                        List<DONTHU> objDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(textNhanDonTuNgay.Text) 
                                                          && x.NGAYTAO <= Convert.ToDateTime(textNhanDonDenNgay.Text)
                                                          && (_NguonDon == -1 || x.NGUONDON_LOAI_CHITIET ==_NguonDon)
                                                          && (_LoaiDon ==-1 || x.LOAIDONTHU_CHA_ID == _LoaiDon)
                                                          && (_DoiTuong == -1 || x.DOITUONG.DOITUONG_LOAI == _DoiTuong)
                                                          && (_HuongXuLy ==-1 || x.HUONGXULY_ID == _HuongXuLy)
                                                          && (_TrangThaiGiaiQuyet ==-1 || (x.KETQUA_XYLY ==true && _TrangThaiGiaiQuyet ==0) || ((x.KETQUA_XYLY ==null || x.KETQUA_XYLY ==false) && _TrangThaiGiaiQuyet ==1))
                                                          ).ToList();

                        // Lọc theo địa phương
                        if (drlTinhThanhPho.SelectedValue !="-1")
                        {
                            if (drlXa.SelectedValue !="-1")
                            {
                                _DP_ID.Add(Convert.ToInt32(drlXa.SelectedValue));
                            }
                            else if(drlQuanHuyen.SelectedValue != "-1")
                            {
                                _DP_ID.Add(Convert.ToInt32(drlQuanHuyen.SelectedValue));
                                var lstXa_ID = vDC.DIAPHUONGs.Where(x => x.DP_ID_CHA == Convert.ToInt32(drlQuanHuyen.SelectedValue)).Select(x => x.DP_ID).ToList();
                                _DP_ID.AddRange(lstXa_ID);
                            }
                            else
                            {
                                string Index =  drlTinhThanhPho.SelectedValue + ".";
                                //_DP_ID.Add(Convert.ToInt32(drlTinhThanhPho.SelectedValue));
                                var lstDP_ID = vDC.DIAPHUONGs.Where(x => x.INDEX_ID.Contains(Index)).Select(x => x.DP_ID).ToList();

                                _DP_ID.AddRange(lstDP_ID);
                            }
                            foreach(var it in objDonThu)
                            {
                                var objCaNhan = it.DOITUONG.CANHANs.Where(x=>_DP_ID.Contains((int)x.DP_ID)).ToList();
                               if(objCaNhan.Count > 0)
                                {
                                    lstDonThu.Add(it);
                                }
                            }
                        }
                        else
                        {
                            lstDonThu.AddRange(objDonThu);
                        }

                        Byte[] fileBytes = baoCaoController.TK_DONTHU_NEW(textNhanDonTuNgay.Text,textNhanDonDenNgay.Text,lstDonThu);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_DONTHU_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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

        public void SetInfoSearch()
        {
            textNhanDonTuNgay.Text = "01/01/" + DateTime.Now.Year;
            textNhanDonDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            loadHuongXuLy(0);
            LoadTinh();
            drlTinhThanhPho.SelectedValue = ClassParameter.vDiaPhuongDefault.ToString();
            LoadHuyen( ClassParameter.vDiaPhuongDefault);
            LoadXa(-1);
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
            if (!string.IsNullOrEmpty(textNhanDonTuNgay.Text) && !string.IsNullOrEmpty(textNhanDonDenNgay.Text) && (Convert.ToDateTime(textNhanDonTuNgay.Text) > Convert.ToDateTime(textNhanDonDenNgay.Text)))
            {
                vToastrMessage += "- Vui lòng chọn Nhận đơn từ ngày lớn hơn Nhận đơn đến ngày <br/>";
                vResult = false;
            }           
            if (vResult == false)
            {
                ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, vToastrMessage, "Thông báo", "error");
            }
            return vResult;
        }

        public void loadHuongXuLy(int vLoaiDonThuID)
        {
            // Khiếu nại ID = 1
            // Tố cáo  ID = 2004
            // Kiến nghị phản ánh ID =4035
            // Đơn có nhiều nội dung 4051         
            ddlistHuongXuLy.Items.Clear();
            ddlistHuongXuLy.Items.Insert(0, new ListItem("--Chọn hướng xử lý--", "-1"));
            if (vLoaiDonThuID == ClassParameter.vKhieuNai_ID)
            {
               
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Hướng dẫn", "2"));
                // Chưa có 
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Ra thông báo thụ lý", "5"));

                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra văn bản đôn đốc", "4"));

                // Chưa có 
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra công văn giao đơn vị xử lý", "12"));

                ddlistHuongXuLy.Items.Insert(6, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(7, new ListItem("Từ chối thụ lý", "8"));
                ddlistHuongXuLy.Items.Insert(8, new ListItem("Trả đơn", "7"));
            }
            else if (vLoaiDonThuID == ClassParameter.vToCao_ID)
            {
               
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Chuyển đơn", "3"));
                // Chưa có 
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Ra thông báo thụ lý", "5"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra văn bản đôn đốc", "4"));
                // Chưa có 
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra công văn giao đơn vị xử lý", "12"));
            }
            else if (vLoaiDonThuID == ClassParameter.vPAKN_ID)
            {             
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Chuyển đơn", "3"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Hướng dẫn - không có văn bản", "9"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Trả đơn", "7"));
            }
            else if (vLoaiDonThuID == ClassParameter.vNhieuNoiDung_ID)
            {              
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Hướng dẫn", "2"));
            }
            else
            {              
                ddlistHuongXuLy.Items.Insert(1, new ListItem("Thụ lý giải quyết", "1"));
                ddlistHuongXuLy.Items.Insert(2, new ListItem("Hướng dẫn", "2"));
                ddlistHuongXuLy.Items.Insert(3, new ListItem("Chuyển đơn", "3"));
                ddlistHuongXuLy.Items.Insert(4, new ListItem("Ra văn bản đôn đốc", "4"));
                ddlistHuongXuLy.Items.Insert(5, new ListItem("Ra thông báo thụ lý", "5"));
                ddlistHuongXuLy.Items.Insert(6, new ListItem("Lưu và theo dõi", "6"));
                ddlistHuongXuLy.Items.Insert(7, new ListItem("Trả đơn", "7"));
                ddlistHuongXuLy.Items.Insert(8, new ListItem("Từ chối thụ lý", "8"));
                ddlistHuongXuLy.Items.Insert(9, new ListItem("Hướng dẫn - không có văn bản", "9"));
                ddlistHuongXuLy.Items.Insert(10, new ListItem("Khác", "10"));
                ddlistHuongXuLy.Items.Insert(11, new ListItem("Ra công văn giao đơn vị xử lý", "12"));
            }
        }
       public void LoadTinh()
        {           
            var objTinh = vDC.DIAPHUONGs.Where(x => x.DP_ID_CHA == 0).ToList();
            drlTinhThanhPho.Items.Clear();
            drlTinhThanhPho.DataSource = objTinh;
            drlTinhThanhPho.DataTextField = "DP_TEN";
            drlTinhThanhPho.DataValueField = "DP_ID";
            drlTinhThanhPho.DataBind();
            drlTinhThanhPho.Items.Insert(0, new ListItem("--Chọn Tỉnh/Thành phố--", "-1"));
        }
        public void LoadHuyen(int _DP_ID)
        {
            var objHuyen = vDC.DIAPHUONGs.Where(x => x.DP_ID_CHA == _DP_ID).ToList();
            drlQuanHuyen.Items.Clear();
            drlQuanHuyen.DataSource = objHuyen;
            drlQuanHuyen.DataTextField = "DP_TEN";
            drlQuanHuyen.DataValueField = "DP_ID";
            drlQuanHuyen.DataBind();
            drlQuanHuyen.Items.Insert(0, new ListItem("--Chọn Quận/Huyện--", "-1"));
        }
        public void LoadXa(int _DP_ID)
        {
            var objXa = vDC.DIAPHUONGs.Where(x => x.DP_ID_CHA == _DP_ID).ToList();
            drlXa.Items.Clear();
            drlXa.DataSource = objXa;
            drlXa.DataTextField = "DP_TEN";
            drlXa.DataValueField = "DP_ID";
            drlXa.DataBind();
            drlXa.Items.Insert(0, new ListItem("--Chọn Phường/Xã--", "-1"));
        }

        protected void drlTinhThanhPho_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadHuyen(Convert.ToInt32(drlTinhThanhPho.SelectedValue));
            LoadXa(-1);
        }

        protected void drlQuanHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadXa(Convert.ToInt32(drlQuanHuyen.SelectedValue));
        }


        protected void drlLoaiDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadHuongXuLy(Convert.ToInt32(drlLoaiDon.SelectedValue));
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
                List<DONTHU> lstDonThu = new List<DONTHU>();
                List<int> _DP_ID = new List<int>();
                int _NguonDon = Convert.ToInt32(ddlistNguon.SelectedValue);
                int _LoaiDon = Convert.ToInt32(drlLoaiDon.SelectedValue);
                int _DoiTuong = Convert.ToInt32(ddlistDoiTuong.SelectedValue);
                int _HuongXuLy = Convert.ToInt32(ddlistHuongXuLy.SelectedValue);
                int _TrangThaiGiaiQuyet = Convert.ToInt32(ddlistTrangThaiGiaiQuyet.SelectedValue);
                List<DONTHU> objDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(textNhanDonTuNgay.Text)
                                                  && x.NGAYTAO <= Convert.ToDateTime(textNhanDonDenNgay.Text)
                                                  && (_NguonDon == -1 || x.NGUONDON_LOAI_CHITIET == _NguonDon)
                                                  && (_LoaiDon == -1 || x.LOAIDONTHU_CHA_ID == _LoaiDon)
                                                  && (_DoiTuong == -1 || x.DOITUONG.DOITUONG_LOAI == _DoiTuong)
                                                  && (_HuongXuLy == -1 || x.HUONGXULY_ID == _HuongXuLy)
                                                  && (_TrangThaiGiaiQuyet == -1 || (x.KETQUA_XYLY == true && _TrangThaiGiaiQuyet == 0) || ((x.KETQUA_XYLY == null || x.KETQUA_XYLY == false) && _TrangThaiGiaiQuyet == 1))
                                                  ).ToList();

                // Lọc theo địa phương
                if (drlTinhThanhPho.SelectedValue != "-1")
                {
                    if (drlXa.SelectedValue != "-1")
                    {
                        _DP_ID.Add(Convert.ToInt32(drlXa.SelectedValue));
                    }
                    else if (drlQuanHuyen.SelectedValue != "-1")
                    {
                        _DP_ID.Add(Convert.ToInt32(drlQuanHuyen.SelectedValue));
                        var lstXa_ID = vDC.DIAPHUONGs.Where(x => x.DP_ID_CHA == Convert.ToInt32(drlQuanHuyen.SelectedValue)).Select(x => x.DP_ID).ToList();
                        _DP_ID.AddRange(lstXa_ID);
                    }
                    else
                    {
                        string Index = drlTinhThanhPho.SelectedValue + ".";
                        //_DP_ID.Add(Convert.ToInt32(drlTinhThanhPho.SelectedValue));
                        var lstDP_ID = vDC.DIAPHUONGs.Where(x => x.INDEX_ID.Contains(Index)).Select(x => x.DP_ID).ToList();

                        _DP_ID.AddRange(lstDP_ID);
                    }
                    foreach (var it in objDonThu)
                    {
                        var objCaNhan = it.DOITUONG.CANHANs.Where(x => _DP_ID.Contains((int)x.DP_ID)).ToList();
                        if (objCaNhan.Count > 0)
                        {
                            lstDonThu.Add(it);
                        }
                    }
                }
                else
                {
                    lstDonThu.AddRange(objDonThu);
                }

                int TotalRow = 0;
                if (lstDonThu.Count > 0)
                {
                    TotalRow = lstDonThu.Count();
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
                lstDonThu = lstDonThu.OrderByDescending(x => x.DONTHU_STT).ToList();
                lstDonThu = lstDonThu.Skip(v_start - 1).Take(v_end - (v_start - 1)).ToList();
                dgDanhSach.DataSource = lstDonThu;
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
