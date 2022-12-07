using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Web.Api;

namespace KNTC
{
    /// <summary>
    /// 0: HỒ SƠ ĐƠN THƯ, 1: HỒ SƠ NGƯỜI ĐẠI DIỆN, ỦY QUYỀN: 2 HỒ SƠ KẾT QUẢ GIẢI QUYẾT, 3: HỒ SƠ ĐƠN THƯ ĐƯỢC CHUYỂN TỚI; 4: HỒ SƠ HƯỚNG XỬ LÝ
    /// </summary>
    public class ServiceKNTCController : DnnApiController
    {
        KNTCDataContext vDC = new KNTCDataContext();
        BaoCaoController baoCaoController = new BaoCaoController();
        string vPathFile = "http://192.168.1.29:9971/DesktopModules/KNTC/";
        string vDomain = "http://192.168.1.29:9971/";

        public ServiceKNTCController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        #region Tiep dan API
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TiepDanDashBoardGet(string year)
        {
            try
            {
                var objTiepDanOld = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN.Value.Year == (Convert.ToInt32(year) - 1)).ToList();
                List<TiepDan> objTiepDan = (from a in vDC.TIEPDANs
                                            join b in vDC.DOITUONGs
                                            on a.DOITUONG_ID equals b.DOITUONG_ID
                                            join c in vDC.LOAIDONTHUs

                                            on a.TIEPDAN_LOAI equals c.LOAIDONTHU_ID
                                            where a.TIEPDAN_THOGIAN.Value.Year == Convert.ToInt32(year)
                                            select new TiepDan
                                            {
                                                TIEPDAN_ID = a.TIEPDAN_ID,
                                                DONTHU_ID = a.DONTHU_ID != null ? (int)a.DONTHU_ID : 0,
                                                Month = a.TIEPDAN_THOGIAN != null ? Convert.ToDateTime(a.TIEPDAN_THOGIAN).Month : 0,
                                                DOITUONG_LOAI = b.DOITUONG_LOAI != null ? (int)b.DOITUONG_LOAI : 0,
                                                LOAIDONTHU_ID = a.TIEPDAN_LOAI_CHA_ID != null ? (int)a.TIEPDAN_LOAI_CHA_ID : 0,
                                                LOAIDONTHU_TEN = c.LOAIDONTHU_TEN
                                            }).ToList();
                // Lấy đơn thư theo tháng 

                TiepDanDashboard dashBoard = new TiepDanDashboard();
                if (objTiepDan.Count > 0)
                {
                  
                    List<TiepDanChart> tiepDanCharts = new List<TiepDanChart>();
                    for (int i = 1; i <= DateTime.Now.Month; i++)
                    {
                        TiepDanChart tiepDans = new TiepDanChart();
                        tiepDans.Month = i;
                        tiepDans.CD_Count = objTiepDan.Where(x => x.DONTHU_ID != 0 && x.Month == i).Count();
                        tiepDans.KD_Count = objTiepDan.Where(x => x.DONTHU_ID == 0 && x.Month == i).Count();
                        tiepDanCharts.Add(tiepDans);
                    }

                    dashBoard.tiepDanCharts = tiepDanCharts;
                    dashBoard.CaNhan = objTiepDan.Where(x => x.DOITUONG_LOAI == 1).Count();
                    dashBoard.ToChuc = objTiepDan.Where(x => x.DOITUONG_LOAI == 2).Count();
                    dashBoard.CoQuan = objTiepDan.Where(x => x.DOITUONG_LOAI == 3).Count();

                    int vLastYeard = Int32.Parse(year) - 1;
                    DateTime vLastYeard_FormDate = new DateTime(vLastYeard, 1, 1);
                    DateTime vLastYeard_ToDate = new DateTime(vLastYeard, DateTime.Now.Month, DateTime.Now.Day);
                    var objLoai = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).Select(y => new TD_LoaiDonThu

                    {
                        LOAIDONTHU_ID = y.LOAIDONTHU_ID,
                        LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                        LOAIDONTHU_COLOR = y.LOAIDONTHU_COLOR == null ? "" : y.LOAIDONTHU_COLOR,
                        LOAIDONTHU_COLOR_CLASS = y.LOAIDONTHU_COLOR_CLASS == null ? "" : y.LOAIDONTHU_COLOR_CLASS,
                        LOAIDONTHU_ICONNAME = y.LOAIDONTHU_ICONNAME == null ? "" : y.LOAIDONTHU_ICONNAME,
                        LOAIDONTHU_ICONFILE = y.LOAIDONTHU_ICONFILE == null ? "" : y.LOAIDONTHU_ICONFILE,
                        //Count = objTiepDanOld.Where(x => x.TIEPDAN_LOAI_CHA_ID == y.LOAIDONTHU_ID).Count(),
                        //Count_oldYear = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= vLastYeard_FormDate && x.TIEPDAN_THOGIAN <= vLastYeard_ToDate && x.TIEPDAN_LOAI_CHA_ID == y.LOAIDONTHU_ID).ToList().Count()
                        //Count_oldYear = objTiepDanOld.Where(x => x.TIEPDAN_LOAI_CHA_ID == y.LOAIDONTHU_ID).Count()
                    }).ToList();
                    foreach (var item in objLoai)
                    {
                        item.Count = objTiepDan.Where(x => x.LOAIDONTHU_ID == item.LOAIDONTHU_ID).Count();
                        item.Count_oldYear = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= vLastYeard_FormDate && x.TIEPDAN_THOGIAN <= vLastYeard_ToDate && x.TIEPDAN_LOAI_CHA_ID == item.LOAIDONTHU_ID).ToList().Count();
                    }
                    dashBoard.TD_LoaiDonThu = objLoai;
                }


                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(dashBoard.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }
        // Get chi tiết thông tin tiếp dân
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TiepDanDetailsGet(string tiepdanid)
        {
            TiepDanChiTiet tiepDanChiTiet = new TiepDanChiTiet();
            TiepDanDetails tiepDanDetails = new TiepDanDetails();
            //DonThuChiTiet donThuChiTiet = new DonThuChiTiet();
            try
            {

                var objTiepDan = vDC.TIEPDANs.Where(x => x.TIEPDAN_ID == Convert.ToInt64(tiepdanid)).FirstOrDefault();
                if (objTiepDan !=null)
                {
                    if(objTiepDan.DONTHU_ID != null)
                    {
                        DonThuChiTiet donThuChiTiet = GetDonThuChiTiet((long)objTiepDan.DONTHU_ID);
                        if (donThuChiTiet !=null)
                        {
                            //tiepDanChiTiet.DonThu_Doituong = donThuChiTiet.DonThu_Doituong;
                            tiepDanChiTiet.TTNguoiBiToCao = donThuChiTiet.TTNguoiBiToCao;
                            tiepDanChiTiet.ThongTinDonThu = donThuChiTiet.ThongTinDonThu;
                            tiepDanChiTiet.NguonDon = donThuChiTiet.NguonDon;
                            tiepDanChiTiet.HuongXuLy = donThuChiTiet.HuongXuLy;
                        }
                    }

                    tiepDanDetails.TIEPDAN_ID = objTiepDan.TIEPDAN_ID;
                    tiepDanDetails.TIEPDAN_NOIDUNG = objTiepDan.TIEPDAN_NOIDUNG;
                    tiepDanDetails.TIEPDAN_KETQUA = objTiepDan.TIEPDAN_KETQUA;
                    tiepDanDetails.TIEPDAN_STT = (long)objTiepDan.TIEPDAN_STT;
                    tiepDanDetails.TIEPDAN_THOGIAN = objTiepDan.TIEPDAN_THOGIAN !=null ? Convert.ToDateTime(objTiepDan.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy"):"";
                    tiepDanDetails.DONTHU_ID = objTiepDan.DONTHU_ID != null ? (int)objTiepDan.DONTHU_ID : 0;
                    tiepDanDetails.NGUOITAO = objTiepDan.NGUOITAO !=null ? (int) objTiepDan.NGUOITAO:0;
                    tiepDanDetails.NGUOICAPNHAT = objTiepDan.NGUOICAPNHAT != null ? (int)objTiepDan.NGUOICAPNHAT : 0;

                    tiepDanDetails.NGAYTAO = objTiepDan.NGAYTAO != null ? Convert.ToDateTime(objTiepDan.NGAYTAO).ToString("dd/MM/yyyy") :"";
                    tiepDanDetails.NGAYCAPNHAT = objTiepDan.NGAYCAPNHAT != null ? Convert.ToDateTime(objTiepDan.NGAYCAPNHAT).ToString("dd/MM/yyyy") : "";
                    tiepDanDetails.TIEPDAN_LANTIEP = objTiepDan.TIEPDAN_LANTIEP != null ? (int)objTiepDan.TIEPDAN_LANTIEP : 0; ;
                    tiepDanDetails.TIEPDAN_CANBO_TIEP_ID = objTiepDan.TIEPDAN_CANBO_TIEP_ID != null ? (int)objTiepDan.TIEPDAN_CANBO_TIEP_ID : 0;
                    tiepDanDetails.DOITUONG_ID = objTiepDan.DOITUONG_ID != null ? (int)objTiepDan.DOITUONG_ID : 0;
                    // Lấy loại tiếp dân
                    if(objTiepDan.DONTHU_ID == null)
                    {
                        tiepDanDetails.TIEPDAN_LOAI = objTiepDan.TIEPDAN_LOAI == null ? "" :baoCaoController.GetLoaiTiepDan((int)objTiepDan.TIEPDAN_LOAI);
                        tiepDanDetails.TIEPDAN_LOAI_LV0 = objTiepDan.TIEPDAN_LOAI == null ? "" : baoCaoController.GetLoaiTiepDan_lv0((int)objTiepDan.TIEPDAN_LOAI);
                        tiepDanDetails.TIEPDAN_LOAI_LV1 = objTiepDan.TIEPDAN_LOAI == null ? "" : baoCaoController.GetLoaiTiepDan_lv1((int)objTiepDan.TIEPDAN_LOAI);
                    }
                    else
                    {
                        tiepDanDetails.TIEPDAN_LOAI = tiepDanChiTiet.ThongTinDonThu ==null?"": tiepDanChiTiet.ThongTinDonThu.LOAIDONTHU_CHITIET;
                        tiepDanDetails.TIEPDAN_LOAI_LV0 = tiepDanChiTiet.ThongTinDonThu == null ? "" : tiepDanChiTiet.ThongTinDonThu.LOAIDONTHU_GOC;
                        tiepDanDetails.TIEPDAN_LOAI_LV1 = tiepDanChiTiet.ThongTinDonThu == null ? "" : tiepDanChiTiet.ThongTinDonThu.LOAIDONTHU_TEN;

                    }
                   



                    List<CaNhan> lstCaNhan = new List<CaNhan>();
                    if (objTiepDan.DOITUONG_ID !=null)
                    {
                        var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objTiepDan.DOITUONG_ID).ToList();
                        if(objCaNhan.Count > 0)
                        {
                          
                            foreach(var it in objCaNhan)
                            {
                                CaNhan caNhan = new CaNhan();
                                caNhan.CANHAN_ID = it.CANHAN_ID;
                                caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                                caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                                caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                                caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                                caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                                if(it.QUOCTICH_ID != null)
                                {
                                   var objQT = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault();
                                    if (objQT !=null)
                                    {
                                        caNhan.QUOCTICH = objQT.QUOCTICH_TEN;
                                    }
                                    
                                }
                                if (it.DANTOC_ID != null)
                                {
                                    var objCN = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault();
                                    if (objCN != null)
                                    {
                                        caNhan.DANTOC = objCN.DANTOC_TEN;
                                    }                                 
                                }
                                caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                                caNhan.DOITUONG_LOAI = caNhan.DOITUONG_LOAI = objTiepDan.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objTiepDan.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                                caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                                lstCaNhan.Add(caNhan);
                            }
                        }
                    }
                    tiepDanDetails.CaNhan = lstCaNhan;

                    tiepDanChiTiet.TiepDanDetails = tiepDanDetails;
                }


                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(tiepDanChiTiet.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }
       
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TiepDanDS(string dt_loai)
        {
            //dt_loai == 0 Lấy tất cả
            //dt_loai == 1 Cá nhân
            //dt_loai == 2 nhóm đông người
            //dt_loai == 3 cơ quan tổ chức
            try
            {               
                List<TiepDanDS> lstTiepDan = new List<TiepDanDS>();
                var objTIEPDAN = vDC.KNTC_TIEPDAN_DOITUONG_LOAIs.Where(x=> dt_loai=="0" ||x.DOITUONG_LOAI == Convert.ToInt32(dt_loai)).OrderByDescending(x=>x.TIEPDAN_STT).ToList();
               
                if(objTIEPDAN.Count >0)
                {
                  
                    lstTiepDan = objTIEPDAN.Select(y => new TiepDanDS
                    {
                        LOAIDONTHU_ID = y.LOAIDONTHU_ID,
                        LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                        TIEPDAN_ID = y.TIEPDAN_ID,
                        TIEPDAN_NGAYTAO = y.NGAYTAO ==null?"":Convert.ToDateTime(y.NGAYTAO).ToString("dd/MM/yyyy"),
                        TIEPDAN_NOIDUNG = y.TIEPDAN_NOIDUNG,
                        DOITUONG_LOAI = y.DOITUONG_LOAI==null?0:(int)y.DOITUONG_LOAI,
                        TIEPDAN_STT = y.TIEPDAN_STT ==null?0:(long)y.TIEPDAN_STT,
                        caNhan_Shorts = vDC.CANHANs.Where(x => x.DOITUONG_ID == y.DOITUONG_ID).Select(z => new CaNhan_Short{
                            CANHAN_ID = z.CANHAN_ID,
                            CANHAN_HOTEN = z.CANHAN_HOTEN

                        }).ToList()                    
                    }).ToList();                
                }
               
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstTiepDan.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TiepDanNangCao(string hoten,string diachi,string noidung,string tinhtrang)
        {
            //tinhtrang == 0 Lấy tất cả
            //tinhtrang == 1 Có đơn
            //tinhtrang == 2 Không đơn
            string k_hoten = string.IsNullOrEmpty(hoten) == true ? "" : hoten.ToLower();
            string k_diachi = string.IsNullOrEmpty(diachi) == true ? "" : diachi.ToLower();
            string k_noidung = string.IsNullOrEmpty(noidung) == true ? "" : noidung.ToLower(); ;
            try
            {
                List<TiepDanDS> lstTiepDan = new List<TiepDanDS>();
                var objTIEPDAN = vDC.KNTC_TIEPDAN_DOITUONG_LOAIs.Where(x => (tinhtrang =="0" ||(tinhtrang =="1" && x.DONTHU_ID != null) || (tinhtrang =="2" && x.DONTHU_ID == null))  ).ToList();
                if (objTIEPDAN.Count > 0)
                {
                    lstTiepDan = objTIEPDAN.Select(y => new TiepDanDS
                    {
                        LOAIDONTHU_ID = y.LOAIDONTHU_ID,
                        LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                        TIEPDAN_ID = y.TIEPDAN_ID,
                        TIEPDAN_NGAYTAO = y.NGAYTAO == null ? "" : Convert.ToDateTime(y.NGAYTAO).ToString("dd/MM/yyyy"),
                        TIEPDAN_NOIDUNG = y.TIEPDAN_NOIDUNG,
                        DOITUONG_LOAI = y.DOITUONG_LOAI == null ? 0 : (int)y.DOITUONG_LOAI,
                        TIEPDAN_STT = y.TIEPDAN_STT == null ? 0 : (long)y.TIEPDAN_STT,
                        DOITUONG_DIACHI =y.DOITUONG_DIACHI,
                        caNhan_Shorts = vDC.CANHANs.Where(x => x.DOITUONG_ID == y.DOITUONG_ID).Select(z => new CaNhan_Short
                        {
                            CANHAN_ID = z.CANHAN_ID,
                            CANHAN_HOTEN = z.CANHAN_HOTEN,
                            CANHAN_DIACHI = z.CANHAN_DIACHI_DAYDU

                        }).ToList()
                    }).Where(x => (x.TIEPDAN_NOIDUNG != null && x.TIEPDAN_NOIDUNG.ToLower().Contains(k_noidung.ToLower()))
                                                      || x.caNhan_Shorts.Where(cn => (cn.CANHAN_HOTEN != null && cn.CANHAN_HOTEN.ToLower().Contains(k_hoten.ToLower()))
                                                      || (cn.CANHAN_DIACHI != null && cn.CANHAN_DIACHI.ToLower().Contains(k_diachi.ToLower()))).FirstOrDefault() != null
                                                      || (x.DOITUONG_DIACHI != null && x.DOITUONG_DIACHI.ToLower().Contains(k_diachi.ToLower()))
                                                      || (x.TIEPDAN_STT.ToString().ToLower() == k_noidung)).ToList();

                    //lstTiepDan = lstTiepDan.Where(x =>(x.TIEPDAN_NOIDUNG != null && x.TIEPDAN_NOIDUNG.ToLower().Contains(k_noidung.ToLower())) 
                    //                                  || x.caNhan_Shorts.Where(cn =>(cn.CANHAN_HOTEN !=null && cn.CANHAN_HOTEN.ToLower().Contains(k_hoten.ToLower())) 
                    //                                  || (cn.CANHAN_DIACHI !=null && cn.CANHAN_DIACHI.ToLower().Contains(k_diachi.ToLower()))).FirstOrDefault() != null 
                    //                                  ||(x.DOITUONG_DIACHI != null && x.DOITUONG_DIACHI.ToLower().Contains(k_diachi.ToLower()))
                    //                                  || (x.TIEPDAN_STT.ToString().ToLower() == k_noidung)).ToList();
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstTiepDan.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TiepDan_DoiTuong_Loai()
        {
            List<DoiTuongLoai> lstDTL = new List<DoiTuongLoai>();
            try
            {
                var objTiepDan = vDC.KNTC_TIEPDAN_DOITUONG_LOAIs.ToList();
                for (int i = 0; i < 4; i++)
                {
                    DoiTuongLoai dtl = new DoiTuongLoai();
                    dtl.DOITUONGLOAI_ID = i;
                    dtl.DOITUONGLOAI_TEN = i == 0 ? "Tất cả đối tượng" : i == 1 ? "Cá nhân" : i == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                    dtl.DOITUONGLOAI_COUNT = objTiepDan.Where(x => i == 0 || x.DOITUONG_LOAI == i).Count();
                    lstDTL.Add(dtl);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstDTL.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }
        #endregion

        #region Don thu API
        [AllowAnonymous]
        [HttpGet]

        public HttpResponseMessage DonThuDashBoardGet(string year)
        {
            try
            {
                List<DonThu> objDonThu = (from a in vDC.DONTHUs
                                          join b in vDC.DOITUONGs
                                          on a.DOITUONG_ID equals b.DOITUONG_ID
                                          where a.NGAYTAO.Value.Year == Convert.ToInt32(year)
                                          select new DonThu
                                          {
                                              DONTHU_ID = a.DONTHU_ID,
                                              Month = a.NGAYTAO != null ? Convert.ToDateTime(a.NGAYTAO).Month : 0,
                                              DOITUONG_LOAI = b.DOITUONG_LOAI != null ? (int)b.DOITUONG_LOAI : 0,
                                              LOAIDONTHU_ID = a.LOAIDONTHU_CHA_ID != null ? (int)a.LOAIDONTHU_CHA_ID : 0,
                                              HUONGXYLY_ID = a.HUONGXULY_ID != null ? (int)a.HUONGXULY_ID : 0
                                          }).ToList();
                // Lấy đơn thư theo tháng 
                DonThuDashboard dashBoard = new DonThuDashboard();
                if (objDonThu.Count > 0)
                {
                    dashBoard.CaNhan = objDonThu.Where(x => x.DOITUONG_LOAI == 1).Count();
                    dashBoard.ToChuc = objDonThu.Where(x => x.DOITUONG_LOAI == 2).Count();
                    dashBoard.CoQuan = objDonThu.Where(x => x.DOITUONG_LOAI == 3).Count();

                    var objLoai = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    List<LoaiDonThu> LstLoaiDonThu = new List<LoaiDonThu>();
                    if (objLoai.Count > 0)
                    {
                        LstLoaiDonThu = objLoai.Select(y => new LoaiDonThu
                        {
                            LOAIDONTHU_ID = y.LOAIDONTHU_ID,
                            LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                            T1 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 1).Count(),
                            T2 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 2).Count(),
                            T3 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 3).Count(),
                            T4 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 4).Count(),
                            T5 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 5).Count(),
                            T6 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 6).Count(),
                            T7 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 7).Count(),
                            T8 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 8).Count(),
                            T9 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 9).Count(),
                            T10 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 10).Count(),
                            T11 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 11).Count(),
                            T12 = objDonThu.Where(x => x.LOAIDONTHU_ID == y.LOAIDONTHU_ID && x.Month == 12).Count()
                        }).ToList();

                    }
                    dashBoard.LoaiDonThu = LstLoaiDonThu;
                }

                // Count huong xu ly don thu
                List<HuongXuLy> lstHuongXuLy = new List<HuongXuLy>();
                var objHuongXuLy = vDC.HUONGXYLies.ToList();
                int vLastYeard = Int32.Parse(year) - 1;
                DateTime vLastYeard_FormDate = new DateTime(vLastYeard, 1, 1);
                DateTime vLastYeard_ToDate = new DateTime(vLastYeard, DateTime.Now.Month, DateTime.Now.Day);

                foreach (var it in objHuongXuLy)
                {
                    HuongXuLy huongXuLy = new HuongXuLy();
                    huongXuLy.HUONGXYLY_ID = it.HUONGXYLY_ID;
                    huongXuLy.HUONGXYLY_TEN = it.HUONGXYLY_TEN;
                    huongXuLy.HUONGXULY_ICONNAME = it.HUONGXULY_ICONNAME == null ? "" : it.HUONGXULY_ICONNAME;
                    huongXuLy.HUONGXULY_COLOR = it.HUONGXULY_COLOR == null ? "" : it.HUONGXULY_COLOR;
                    huongXuLy.HUONGXULY_COLOR_CLASS = it.HUONGXULY_COLOR_CLASS == null ? "" : it.HUONGXULY_COLOR_CLASS;
                    huongXuLy.HUONGXULY_ICONFILE = it.HUONGXULY_ICONFILE == null ? "" : it.HUONGXULY_ICONFILE;
                    huongXuLy.HUONGXYLY_Count = objDonThu.Where(x => x.HUONGXYLY_ID == it.HUONGXYLY_ID).Count();
                    huongXuLy.HUONGXYLY_Count_OldYear = vDC.DONTHUs.Where(x => x.NGAYTAO >= vLastYeard_FormDate && x.NGAYTAO <= vLastYeard_ToDate && x.HUONGXULY_ID == it.HUONGXYLY_ID).ToList().Count();
                    lstHuongXuLy.Add(huongXuLy);
                }
                dashBoard.HuongXuLy = lstHuongXuLy;
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(dashBoard.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThuDS(string dt_loai , string trangthai)
        {
            //dt_loai == 0 Lấy tất cả
            //dt_loai == 1 Cá nhân
            //dt_loai == 2 nhóm đông người
            //dt_loai == 3 cơ quan tổ chức

            // trangthai == 0 Lấy đơn thư chưa có hướng xử lý
            // trangthai  != 0  Lấy tất cả đơn thư
            try
            {              
                List<DonThuDS> lstTDonThu = new List<DonThuDS>();
                var objDONTHU = vDC.KNTC_DONTHU_DOITUONG_LOAIs.Where(x => (dt_loai == "0" || x.DOITUONG_LOAI == Convert.ToInt32(dt_loai))
                                                                          && ((trangthai == "0" && x.DONTHU_TRANGTHAI == Convert.ToInt32(trangthai))|| trangthai != "0")).OrderByDescending(x=>x.DONTHU_STT).ToList();
                if (objDONTHU.Count > 0)
                {
                    lstTDonThu = objDONTHU.Select(y => new DonThuDS
                    {
                        LOAIDONTHU_ID = y.LOAIDONTHU_ID == null ? 0 : (int)y.LOAIDONTHU_ID,
                        LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                        LOAIDONTHU_CHA_TEN = y.LOAIDONTHU_CHA_TEN,
                        DONTHU_ID = y.DONTHU_ID,
                        DONTHU_NGUONDON = y.NGUONDON_LOAI_CHITIET ==null?"": y.NGUONDON_LOAI_CHITIET ==0?"Trực tiếp": y.NGUONDON_LOAI_CHITIET ==1?"Bưu chính": y.NGUONDON_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)y.NGUONDON_DONVI_ID),
                        DONTHU_NOIDUNG = y.DONTHU_NOIDUNG,
                        DONTHU_NGAYTAO = y.NGAYTAO == null ? "" : Convert.ToDateTime(y.NGAYTAO).ToString("dd/MM/yyyy"),
                        DOITUONG_LOAI = y.DOITUONG_LOAI == null ? 0 : (int)y.DOITUONG_LOAI,
                        HUONGXULY_ID = y.HUONGXYLY_ID == null ? 0 : (int)y.HUONGXYLY_ID,
                        HUONGXULY_TEN = y.HUONGXYLY_TEN,
                        DONVI_ID = y.DONVI_ID == null ? 0 : (int)y.DONVI_ID,
                        DONVI_TEN = y.TENDONVI,
                        DONTHU_STT = y.DONTHU_STT == null ? 0 : (long)y.DONTHU_STT,
                        caNhan_Shorts = vDC.CANHANs.Where(x => x.DOITUONG_ID == y.DOITUONG_ID).Select(z => new CaNhan_Short
                        {
                            CANHAN_ID = z.CANHAN_ID,
                            CANHAN_HOTEN = z.CANHAN_HOTEN

                        }).ToList()
                    }).ToList();
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstTDonThu.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }
      
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThu_DoiTuong(string donthuid)
        {
            List<CaNhan> lstCaNhan = new List<CaNhan>();
            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if(objDonThu !=null && objDonThu.DOITUONG_ID != null)
                {
                    var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_ID).ToList();
                    if (objCaNhan.Count > 0)
                    {

                        foreach (var it in objCaNhan)
                        {
                            CaNhan caNhan = new CaNhan();
                            caNhan.CANHAN_ID = it.CANHAN_ID;
                            caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                            caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                            caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                            caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                            caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                            if (it.QUOCTICH_ID != null)
                            {
                                caNhan.QUOCTICH = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault().QUOCTICH_TEN;
                            }
                            if (it.DANTOC_ID != null)
                            {
                                caNhan.DANTOC = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault().DANTOC_TEN;
                            }
                            caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                            caNhan.DOITUONG_LOAI = caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objDonThu.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan";
                            caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                            lstCaNhan.Add(caNhan);
                        }
                    }
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstCaNhan.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }

        /// <summary>
        /// 0: HỒ SƠ ĐƠN THƯ, 1: HỒ SƠ NGƯỜI ĐẠI DIỆN, ỦY QUYỀN: 2 HỒ SƠ KẾT QUẢ GIẢI QUYẾT, 3: HỒ SƠ ĐƠN THƯ ĐƯỢC CHUYỂN TỚI; 4: HỒ SƠ HƯỚNG XỬ LÝ
        /// </summary>
       
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage ThongTinDonThu(string donthuid)
        {
            ThongTinDonThu thongTinDon = new ThongTinDonThu();
            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if (objDonThu != null)
                {
                    thongTinDon.DONTHU_ID = objDonThu.DONTHU_ID;
                    thongTinDon.LOAIDONTHU_ID = objDonThu.LOAIDONTHU_ID == null ? 0 : (int)objDonThu.LOAIDONTHU_ID;
                    thongTinDon.DONTHU_TRANGTHAI = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN == true ? "Không đủ điều kiện" : "Đủ điều kiện";
                    if (objDonThu.LOAIDONTHU_CHA_ID != null)
                    {
                        thongTinDon.LOAIDONTHU_GOC = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDonThu.LOAIDONTHU_CHA_ID).FirstOrDefault().LOAIDONTHU_TEN;

                    }
                    if (objDonThu.LOAIDONTHU_ID != null)
                    {
                        var objLoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDonThu.LOAIDONTHU_ID).FirstOrDefault();
                        thongTinDon.LOAIDONTHU_CHITIET = objLoaiDonThu.LOAIDONTHU_TEN;
                        thongTinDon.LOAIDONTHU_TEN = objLoaiDonThu.LOAIDONTHU_CHA_ID == 0 ? objLoaiDonThu.LOAIDONTHU_TEN : objLoaiDonThu.LOAIDONTHU_CHA_TEN;
                    }
                    thongTinDon.DONTHU_NOIDUNG = objDonThu.DONTHU_NOIDUNG;
                    if (objDonThu.DAGIAIQUYET_DONVI_ID != null)
                    {
                        thongTinDon.DONTHU_COQUANGQ = vDC.DONVIs.Where(x => x.DONVI_ID == objDonThu.DAGIAIQUYET_DONVI_ID).FirstOrDefault().TENDONVI;
                    }
                    thongTinDon.DONTHU_HINHTHUCGQ = objDonThu.HUONGXULY_TEN;
                    thongTinDon.DONTHU_KETQUAGQ = objDonThu.DAGIAIQUYET_KETQUA_CQ;
                    thongTinDon.DONTHU_LANGQ = objDonThu.DAGIAIQUYET_LAN.ToString();
                    thongTinDon.DONTHU_NGAYBANHANHQD = objDonThu.DAGIAIQUYET_NGAYBANHANH_QD !=null?Convert.ToDateTime(objDonThu.DAGIAIQUYET_NGAYBANHANH_QD).ToString("dd/MM/yyyy"):"";

                    thongTinDon.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID, 0);
                        
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(thongTinDon.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThu_DoiTuongBiKNTC(string donthuid)
        {
            List<CaNhan> lstCaNhan = new List<CaNhan>();
            TTNguoiBiToCao objBiKNTC = new TTNguoiBiToCao();
            
            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if (objDonThu != null && objDonThu.DOITUONG_BI_KNTC_ID != null)
                {
                    var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_BI_KNTC_ID).ToList();
                    if (objCaNhan.Count > 0)
                    {

                        foreach (var it in objCaNhan)
                        {
                            CaNhan caNhan = new CaNhan();
                            caNhan.CANHAN_ID = it.CANHAN_ID;
                            caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                            caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                            caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                            caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                            caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                            if (it.QUOCTICH_ID != null)
                            {
                                caNhan.QUOCTICH = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault().QUOCTICH_TEN;
                            }
                            if (it.DANTOC_ID != null)
                            {
                                caNhan.DANTOC = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault().DANTOC_TEN;
                            }
                            caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                            caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objDonThu.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                            caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                            lstCaNhan.Add(caNhan);
                        }
                    }
                    objBiKNTC.caNhans = lstCaNhan;
                    objBiKNTC.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID,1);
                    if (objDonThu.NGUOIUYQUYEN_CANHAN_ID != null)
                    {
                        var obj = vDC.CANHANs.Where(x => x.CANHAN_ID == objDonThu.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();

                        objBiKNTC.NGUOIUYQUYEN_TEN = obj.CANHAN_HOTEN;
                        objBiKNTC.NGUOIUYQUYEN_DIACHI = obj.CANHAN_DIACHI_DAYDU;
                        objBiKNTC.NGUOIUYQUYEN_GIOITINH = obj.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                       
                    }
                  
                }
               
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(objBiKNTC.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThu_HuongXuLy(string donthuid)
        {
          
            HuongXuLyChiTiet objHuongXuLy = new HuongXuLyChiTiet();

            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if (objDonThu != null )
                {
                    objHuongXuLy.HUONGXYLY_ID = objDonThu.HUONGXULY_ID ==null ?0:(int)objDonThu.HUONGXULY_ID;
                    objHuongXuLy.HUONGXYLY_TEN = objDonThu.HUONGXULY_TEN;
                    objHuongXuLy.HXL_COQUAN = objDonThu.HUONGXULY_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.HUONGXULY_DONVI_ID);
                    objHuongXuLy.HXL_THOIHANGQ = objDonThu.HUONGXULY_THOIHANGIAIQUET == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_THOIHANGIAIQUET).ToString("dd/MM/yyyy");
                    objHuongXuLy.HXL_THAMQUYENGQ = objDonThu.HUONGXULY_THAMQUYENGIAIQUYET_TEN;
                    objHuongXuLy.HXL_NGAYCHUYEN = objDonThu.HUONGXULY_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_NGAYCHUYEN).ToString("dd/MM/yyyy");
                    objHuongXuLy.HXL_NGUOITIEPNHAN = objDonThu.HUONGXULY_CANBO == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_CANBO);
                    objHuongXuLy.HXL_NGUOIDUYET = objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID);
                    objHuongXuLy.HXL_CHUCVU = objDonThu.HUONGXULY_CHUCVU_TEN;
                    objHuongXuLy.HXL_YKIENXL = objDonThu.HUONGXULY_YKIEN_XULY;
                    objHuongXuLy.HXL_GHICHU = objDonThu.DONTHU_GHICHU;
                    objHuongXuLy.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID,4);
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(objHuongXuLy.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThu_NguonDon(string donthuid)
        {
            NguonDon objNguonDon = new NguonDon();
            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if (objDonThu != null)
                {
                    objNguonDon.DONTHU_ID = objDonThu.DONTHU_ID;
                    objNguonDon.NGUONDON_DONVI = objDonThu.NGUONDON_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.NGUONDON_DONVI_ID);
                    objNguonDon.NGUONDON_LOAI = (bool)objDonThu.NGUONDON_LOAI;
                    objNguonDon.NGUONDON_LOAI_CHITIET = objDonThu.NGUONDON_LOAI_CHITIET ==null ?"": objDonThu.NGUONDON_LOAI_CHITIET ==0?"Trực tiếp": objDonThu.NGUONDON_LOAI_CHITIET ==1?"Bưu chính" : objNguonDon.NGUONDON_DONVI;
                    objNguonDon.NGUONDON_SOVANBANCHUYEN = objDonThu.NGUONDON_SOVANBANCHUYEN;
                    objNguonDon.NGUONDON_NGAYNHANDON = objDonThu.NGAYTAO == null ? "" : Convert.ToDateTime(objDonThu.NGAYTAO).ToString("dd/MM/yyyy");
                    objNguonDon.NGUONDON_NGAYCHUYEN = objDonThu.NGUONDON_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYCHUYEN).ToString("dd/MM/yyyy");
                    objNguonDon.NGUONDON_NGAYDEDON = objDonThu.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(objNguonDon.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }

       
        ///
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThuChiTiet(string donthuid)
        {
            DonThuChiTiet donThuChiTiet = new DonThuChiTiet();
            List<CaNhan> lstCaNhanGuiDonThu = new List<CaNhan>();
            ThongTinDonThu thongTinDon = new ThongTinDonThu();
            NguonDon objNguonDon = new NguonDon();
            HuongXuLyChiTiet objHuongXuLy = new HuongXuLyChiTiet();
            List<CaNhan> lstCaNhan_NguoiBiKNTC = new List<CaNhan>();
            TTNguoiBiToCao objBiKNTC = new TTNguoiBiToCao();
            try
            {
                var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == Convert.ToInt64(donthuid)).FirstOrDefault();
                if (objDonThu != null)
                { 

                    // Lấy list đối tượng gửi đơn thư *****************************
                    if(objDonThu.DOITUONG_ID != null)
                    {
                        var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_ID).ToList();
                        if (objCaNhan.Count > 0)
                        {

                            foreach (var it in objCaNhan)
                            {
                                CaNhan caNhan = new CaNhan();
                                caNhan.CANHAN_ID = it.CANHAN_ID;
                                caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                                caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                                caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                                caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                                caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                                if (it.QUOCTICH_ID != null)
                                {
                                    var objQT = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault();
                                    if (objQT !=null)
                                    {
                                        caNhan.QUOCTICH = objQT.QUOCTICH_TEN;
                                    }
                                   
                                }
                                if (it.DANTOC_ID != null)
                                {
                                   var DT = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault();
                                    if (DT !=null)
                                    {
                                        caNhan.DANTOC = DT.DANTOC_TEN;
                                    }
                                   
                                }
                                caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                                caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI ==1 ?"Cá nhân":objDonThu.DOITUONG.DOITUONG_LOAI == 2?"Đoàn đông người":"Cơ quan tổ chức";
                                caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                                lstCaNhanGuiDonThu.Add(caNhan);
                            }
                        }
                    }
                    donThuChiTiet.DonThu_Doituong = lstCaNhanGuiDonThu;
                    // end get đối tượng gửi đơn thư **************************

                    // Lấy thông tin đơn thư **********************************
                    thongTinDon.DONTHU_ID = objDonThu.DONTHU_ID;
                    thongTinDon.LOAIDONTHU_ID = objDonThu.LOAIDONTHU_ID == null ? 0 : (int)objDonThu.LOAIDONTHU_ID;
                    thongTinDon.DONTHU_TRANGTHAI = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN == true ? "Không đủ điều kiện" : "Đủ điều kiện";
                    if (objDonThu.LOAIDONTHU_CHA_ID != null)
                    {
                        var objLDT = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDonThu.LOAIDONTHU_CHA_ID).FirstOrDefault();
                        if (objLDT !=null)
                        {
                            thongTinDon.LOAIDONTHU_GOC = objLDT.LOAIDONTHU_TEN;
                        }
                      

                    }
                    if (objDonThu.LOAIDONTHU_ID != null)
                    {
                        var objLoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDonThu.LOAIDONTHU_ID).FirstOrDefault();
                        thongTinDon.LOAIDONTHU_CHITIET = objLoaiDonThu.LOAIDONTHU_TEN;
                        thongTinDon.LOAIDONTHU_TEN = objLoaiDonThu.LOAIDONTHU_CHA_ID == 0 ? objLoaiDonThu.LOAIDONTHU_TEN : objLoaiDonThu.LOAIDONTHU_CHA_TEN;
                    }
                    thongTinDon.DONTHU_NOIDUNG = objDonThu.DONTHU_NOIDUNG;
                    if (objDonThu.DAGIAIQUYET_DONVI_ID != null)
                    {
                        var objDV = vDC.DONVIs.Where(x => x.DONVI_ID == objDonThu.DAGIAIQUYET_DONVI_ID).FirstOrDefault();
                        if (objDV !=null)
                        {
                            thongTinDon.DONTHU_COQUANGQ = objDV.TENDONVI;
                        }
                      
                    }
                    thongTinDon.DONTHU_HINHTHUCGQ = objDonThu.HUONGXULY_TEN;
                    thongTinDon.DONTHU_KETQUAGQ = objDonThu.DAGIAIQUYET_KETQUA_CQ;
                    thongTinDon.DONTHU_LANGQ = objDonThu.DAGIAIQUYET_LAN.ToString();
                    thongTinDon.DONTHU_NGAYBANHANHQD = objDonThu.DAGIAIQUYET_NGAYBANHANH_QD != null ? Convert.ToDateTime(objDonThu.DAGIAIQUYET_NGAYBANHANH_QD).ToString("dd/MM/yyyy") : "";
                    thongTinDon.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID,0);

                    donThuChiTiet.ThongTinDonThu = thongTinDon;
                    // End Lấy thông tin đơn thư ******************************

                    // Lấy thông tin người bị khiếu nại tố cáo

                    if (objDonThu.LOAIDONTHU_ID != null)
                    {
                        if (objDonThu != null && objDonThu.DOITUONG_BI_KNTC_ID != null)
                        {
                            var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_BI_KNTC_ID).ToList();
                            if (objCaNhan.Count > 0)
                            {

                                foreach (var it in objCaNhan)
                                {
                                    CaNhan caNhan = new CaNhan();
                                    caNhan.CANHAN_ID = it.CANHAN_ID;
                                    caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                                    caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                                    caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";                                 
                                    caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                                    caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                                    if (it.QUOCTICH_ID != null)
                                    {
                                        var objQT = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault();
                                        if (objQT != null)
                                        {
                                            // caNhan.QUOCTICH = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault().QUOCTICH_TEN;
                                            caNhan.QUOCTICH = objQT.QUOCTICH_TEN;
                                        }
                                       
                                    }
                                    if (it.DANTOC_ID != null)
                                    {
                                        var objDT = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault();
                                        if (objDT !=null)
                                        {
                                            caNhan.DANTOC = objDT.DANTOC_TEN;
                                        }
                                       
                                    }
                                    caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                                    caNhan.DOITUONG_LOAI = caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objDonThu.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                                    caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                                    lstCaNhan_NguoiBiKNTC.Add(caNhan);
                                }
                            }
                            objBiKNTC.caNhans = lstCaNhan_NguoiBiKNTC;
                            objBiKNTC.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID,1);
                            if (objDonThu.NGUOIUYQUYEN_CANHAN_ID != null)
                            {
                                var obj = vDC.CANHANs.Where(x => x.CANHAN_ID == objDonThu.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();

                                objBiKNTC.NGUOIUYQUYEN_TEN = obj.CANHAN_HOTEN;
                                objBiKNTC.NGUOIUYQUYEN_DIACHI = obj.CANHAN_DIACHI_DAYDU;
                                objBiKNTC.NGUOIUYQUYEN_GIOITINH = obj.CANHAN_GIOITINH == false ? "Nam" : "Nữ";

                            }

                        }
                    }

                    donThuChiTiet.TTNguoiBiToCao = objBiKNTC;
                    // End thông tin người bị khiếu nại tố cáo

                    // Lấy thông tin nguồn đơn
                    objNguonDon.DONTHU_ID = objDonThu.DONTHU_ID;
                    objNguonDon.NGUONDON_DONVI = objDonThu.NGUONDON_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.NGUONDON_DONVI_ID);
                    objNguonDon.NGUONDON_LOAI = objDonThu.NGUONDON_LOAI == null ?false:(bool)objDonThu.NGUONDON_LOAI;
                    objNguonDon.NGUONDON_LOAI_CHITIET = objDonThu.NGUONDON_LOAI_CHITIET == null ? "" : objDonThu.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : objDonThu.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" :objNguonDon.NGUONDON_DONVI;
                    objNguonDon.NGUONDON_SOVANBANCHUYEN = objDonThu.NGUONDON_SOVANBANCHUYEN;
                    objNguonDon.NGUONDON_NGAYNHANDON = objDonThu.NGAYTAO == null ? "" : Convert.ToDateTime(objDonThu.NGAYTAO).ToString("dd/MM/yyyy");
                    objNguonDon.NGUONDON_NGAYCHUYEN = objDonThu.NGUONDON_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYCHUYEN).ToString("dd/MM/yyyy");
                    objNguonDon.NGUONDON_NGAYDEDON = objDonThu.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                    donThuChiTiet.NguonDon = objNguonDon;
                    // End lấy thông tin nguồn đơn

                    // Đơn thư hướng xử lý
                    objHuongXuLy.HUONGXYLY_ID = objDonThu.HUONGXULY_ID == null ? 0 : (int)objDonThu.HUONGXULY_ID;
                    objHuongXuLy.HUONGXYLY_TEN = objDonThu.HUONGXULY_TEN;                 
                    objHuongXuLy.HXL_COQUAN = objDonThu.HUONGXULY_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.HUONGXULY_DONVI_ID);
                    objHuongXuLy.HXL_THOIHANGQ = objDonThu.HUONGXULY_THOIHANGIAIQUET == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_THOIHANGIAIQUET).ToString("dd/MM/yyyy");
                    objHuongXuLy.HXL_THAMQUYENGQ = objDonThu.HUONGXULY_THAMQUYENGIAIQUYET_TEN;
                    objHuongXuLy.HXL_NGAYCHUYEN = objDonThu.HUONGXULY_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_NGAYCHUYEN).ToString("dd/MM/yyyy");
                    objHuongXuLy.HXL_NGUOITIEPNHAN = objDonThu.HUONGXULY_CANBO == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_CANBO);
                    objHuongXuLy.HXL_NGUOIDUYET = objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID);
                    objHuongXuLy.HXL_CHUCVU = objDonThu.HUONGXULY_CHUCVU_TEN;
                    objHuongXuLy.HXL_YKIENXL = objDonThu.HUONGXULY_YKIEN_XULY;
                    objHuongXuLy.HXL_GHICHU = objDonThu.DONTHU_GHICHU;
                    // objHuongXuLy.HXL_URLFILE = System.Web.HttpContext.Current.Server.MapPath(vPathFile) + "Files/FileMau.pdf";
                    objHuongXuLy.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID,4);
                   
                    donThuChiTiet.HuongXuLy = objHuongXuLy;
                    // End Đơn thư hướng xử lý
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(donThuChiTiet.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message,new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        // Hiển thị danh sách đơn thư đã tiếp nhận được sắp xếp theo số tứ tự đơn và cho phép tìm kiếm theo thông tin chủ đơn, nguồn đơn, loại đơn, nội dung đơn, trình trạng đơn thư.

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThuNangCao(string hoten, string diachi, string noidung, string trangthai)
        {

            // DONTHU_TRANGTHAI 0 : chưa xử lý, 1: đã có hướng xử lý, 2: gửi giải quyết đơn thư, 3 đã có kết quả giải quyết, 4 Kết thúc đơn
            // huongxuly ==0 => Lấy đơn thư chưa có hướng xử lý
            // huongxuly !=0 => lấy tất cả đơn thư

            string k_hoten = string.IsNullOrEmpty(hoten) ==true ? "":hoten.ToLower();
            string k_diachi = string.IsNullOrEmpty(diachi) == true ? "" : diachi.ToLower();
            string k_noidung = string.IsNullOrEmpty(noidung) == true ? "" : noidung.ToLower(); ;

            try
            {
                List<DonThuDS> lstTDonThu = new List<DonThuDS>();
                var objDONTHU = vDC.KNTC_DONTHU_DOITUONG_LOAIs.Where(x=> x.DONTHU_TRANGTHAI == Convert.ToInt32(trangthai)).OrderByDescending(x => x.DONTHU_STT).ToList();

                if (objDONTHU.Count > 0)
                {
                    lstTDonThu = objDONTHU.Select(y => new DonThuDS
                    {
                        LOAIDONTHU_ID = y.LOAIDONTHU_ID == null ? 0 : (int)y.LOAIDONTHU_ID,
                        LOAIDONTHU_TEN = y.LOAIDONTHU_TEN,
                        LOAIDONTHU_CHA_TEN = y.LOAIDONTHU_CHA_TEN,
                        DONTHU_ID = y.DONTHU_ID,
                        DONTHU_NGUONDON = y.NGUONDON_LOAI_CHITIET == null ? "" : y.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : y.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" : y.NGUONDON_DONVI_ID ==null?"": baoCaoController.GetTenDonVi((int)y.NGUONDON_DONVI_ID),
                        DONTHU_NOIDUNG = y.DONTHU_NOIDUNG,
                        DONTHU_NGAYTAO = y.NGAYTAO == null ? "" : Convert.ToDateTime(y.NGAYTAO).ToString("dd/MM/yyyy"),
                        DOITUONG_LOAI = y.DOITUONG_LOAI == null ? 0 : (int)y.DOITUONG_LOAI,
                        HUONGXULY_ID = y.HUONGXYLY_ID == null ? 0 : (int)y.HUONGXYLY_ID,
                        HUONGXULY_TEN = y.HUONGXYLY_TEN,
                        DONVI_ID = y.DONVI_ID == null ? 0 : (int)y.DONVI_ID,
                        DONVI_TEN = y.TENDONVI,
                        DONTHU_STT = y.DONTHU_STT == null ? 0 : (long)y.DONTHU_STT,
                        DOITUONG_DIACHI = y.DOITUONG_DIACHI,                      
                        caNhan_Shorts = vDC.CANHANs.Where(x => x.DOITUONG_ID == y.DOITUONG_ID).Select(z => new CaNhan_Short
                        {
                            CANHAN_ID = z.CANHAN_ID,
                            CANHAN_HOTEN = z.CANHAN_HOTEN,
                            CANHAN_DIACHI = z.CANHAN_DIACHI_DAYDU

                        }).ToList()
                    }).Where(x => (x.DONTHU_NOIDUNG != null && x.DONTHU_NOIDUNG.ToLower().Contains(k_noidung))
                                                     || x.caNhan_Shorts.Where(cn => (cn.CANHAN_HOTEN != null && cn.CANHAN_HOTEN.ToLower().Contains(k_hoten))
                                                     || (cn.CANHAN_DIACHI != null && cn.CANHAN_DIACHI.ToLower().Contains(k_diachi))).FirstOrDefault() != null
                                                     || (x.DOITUONG_DIACHI != null && x.DOITUONG_DIACHI.ToLower().Contains(k_diachi))
                                                     || (x.DONTHU_STT.ToString().ToLower() == k_noidung)).ToList();

                    //lstTDonThu = lstTDonThu.Where(x =>(x.DONTHU_NOIDUNG !=null && x.DONTHU_NOIDUNG.ToLower().Contains(k_noidung)) 
                    //                                    || x.caNhan_Shorts.Where(cn =>(cn.CANHAN_HOTEN !=null && cn.CANHAN_HOTEN.ToLower().Contains(k_hoten)) 
                    //                                    || (cn.CANHAN_DIACHI !=null && cn.CANHAN_DIACHI.ToLower().Contains(k_diachi))).FirstOrDefault() != null 
                    //                                    ||(x.DOITUONG_DIACHI !=null && x.DOITUONG_DIACHI.ToLower().Contains(k_diachi))
                    //                                    ||  x.DONTHU_STT.ToString().ToLower() ==k_noidung).ToList();
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstTDonThu.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {              
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

        }
      
        #endregion

        #region Login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Login([FromBody] LoginEntity model)
        {
            try
            {
                NguoiDungInfo nguoiDungInfo = new NguoiDungInfo();

                UserInfo vUserInfo = UserController.GetUserByName(model.username);

                bool vIsValidLogin = System.Web.Security.Membership.ValidateUser(vUserInfo.Username, model.password);

                if (vIsValidLogin == true)
                {
                    if (vUserInfo.IsInRole("CHUYENVIEN") || vUserInfo.IsInRole("LANHDAO"))
                    {
                        nguoiDungInfo.USER_ID = vUserInfo.UserID;
                        nguoiDungInfo.USER_HOTEN = vUserInfo.DisplayName;
                        if (vUserInfo.IsInRole("CHUYENVIEN") == true)
                        {
                            nguoiDungInfo.USER_ROLE = "CHUYENVIEN";
                        }
                        else
                        {
                            nguoiDungInfo.USER_ROLE = "LANHDAO";
                        }
                        nguoiDungInfo.USER_TOKEN = ClassCommon.GetGuid();
                    }                  
                }

                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(nguoiDungInfo.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        public DonThuChiTiet GetDonThuChiTiet(long donthuid)
        {
            DonThuChiTiet donThuChiTiet = new DonThuChiTiet();
            List<CaNhan> lstCaNhanGuiDonThu = new List<CaNhan>();
            ThongTinDonThu thongTinDon = new ThongTinDonThu();
            NguonDon objNguonDon = new NguonDon();
            HuongXuLyChiTiet objHuongXuLy = new HuongXuLyChiTiet();
            List<CaNhan> lstCaNhan_NguoiBiKNTC = new List<CaNhan>();
            TTNguoiBiToCao objBiKNTC = new TTNguoiBiToCao();

            var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == donthuid).FirstOrDefault();
            if (objDonThu != null)
            {
                // Lấy list đối tượng gửi đơn thư *****************************
                if (objDonThu.DOITUONG_ID != null)
                {
                    var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_ID).ToList();
                    if (objCaNhan.Count > 0)
                    {

                        foreach (var it in objCaNhan)
                        {
                            CaNhan caNhan = new CaNhan();
                            caNhan.CANHAN_ID = it.CANHAN_ID;
                            caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                            caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                            caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                            caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                            caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                            if (it.QUOCTICH_ID != null)
                            {
                                var objQT = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault();
                                if (objQT != null)
                                {
                                    caNhan.QUOCTICH = objQT.QUOCTICH_TEN;
                                }

                            }
                            if (it.DANTOC_ID != null)
                            {
                                var DT = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault();
                                if (DT != null)
                                {
                                    caNhan.DANTOC = DT.DANTOC_TEN;
                                }

                            }
                            caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                            caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objDonThu.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                            caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                            lstCaNhanGuiDonThu.Add(caNhan);
                        }
                    }
                }
                donThuChiTiet.DonThu_Doituong = lstCaNhanGuiDonThu;
                // end get đối tượng gửi đơn thư **************************

                // Lấy thông tin đơn thư **********************************
                thongTinDon.DONTHU_ID = objDonThu.DONTHU_ID;
                thongTinDon.LOAIDONTHU_ID = objDonThu.LOAIDONTHU_ID == null ? 0 : (int)objDonThu.LOAIDONTHU_ID;
                thongTinDon.DONTHU_TRANGTHAI = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN == true ? "Không đủ điều kiện" : "Đủ điều kiện";

                thongTinDon.LOAIDONTHU_GOC = objDonThu.LOAIDONTHU_ID ==null ?"": baoCaoController.GetLoaiTiepDan_lv0((int)objDonThu.LOAIDONTHU_ID);
                thongTinDon.LOAIDONTHU_TEN = objDonThu.LOAIDONTHU_ID == null ? "" : baoCaoController.GetLoaiTiepDan_lv1((int)objDonThu.LOAIDONTHU_ID);
                thongTinDon.LOAIDONTHU_CHITIET = objDonThu.LOAIDONTHU_ID == null ? "" : baoCaoController.GetLoaiTiepDan((int)objDonThu.LOAIDONTHU_ID);
              

               
                thongTinDon.DONTHU_NOIDUNG = objDonThu.DONTHU_NOIDUNG;
                if (objDonThu.DAGIAIQUYET_DONVI_ID != null)
                {
                    var objDV = vDC.DONVIs.Where(x => x.DONVI_ID == objDonThu.DAGIAIQUYET_DONVI_ID).FirstOrDefault();
                    if (objDV != null)
                    {
                        thongTinDon.DONTHU_COQUANGQ = objDV.TENDONVI;
                    }

                }
                thongTinDon.DONTHU_HINHTHUCGQ = objDonThu.HUONGXULY_TEN;
                thongTinDon.DONTHU_KETQUAGQ = objDonThu.DAGIAIQUYET_KETQUA_CQ;
                thongTinDon.DONTHU_LANGQ = objDonThu.DAGIAIQUYET_LAN.ToString();
                thongTinDon.DONTHU_NGAYBANHANHQD = objDonThu.DAGIAIQUYET_NGAYBANHANH_QD != null ? Convert.ToDateTime(objDonThu.DAGIAIQUYET_NGAYBANHANH_QD).ToString("dd/MM/yyyy") : "";
                thongTinDon.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID, 0);

                donThuChiTiet.ThongTinDonThu = thongTinDon;
                // End Lấy thông tin đơn thư ******************************

                // Lấy thông tin người bị khiếu nại tố cáo

                if (objDonThu.LOAIDONTHU_ID != null)
                {
                    if (objDonThu != null && objDonThu.DOITUONG_BI_KNTC_ID != null)
                    {
                        var objCaNhan = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDonThu.DOITUONG_BI_KNTC_ID).ToList();
                        if (objCaNhan.Count > 0)
                        {

                            foreach (var it in objCaNhan)
                            {
                                CaNhan caNhan = new CaNhan();
                                caNhan.CANHAN_ID = it.CANHAN_ID;
                                caNhan.CANHAN_HOTEN = it.CANHAN_HOTEN;
                                caNhan.CANHAN_CMDN = it.CANHAN_CMDN;
                                caNhan.CANHAN_CMDN_NGAYCAP = it.CANHAN_CMDN_NGAYCAP != null ? Convert.ToDateTime(it.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy") : "";
                                caNhan.CANHAN_NOICAP = it.CANHAN_NOICAP;
                                caNhan.CANHAN_DIACHI_DAYDU = it.CANHAN_DIACHI_DAYDU;
                                if (it.QUOCTICH_ID != null)
                                {
                                    var objQT = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault();
                                    if (objQT != null)
                                    {
                                        // caNhan.QUOCTICH = vDC.QUOCTICHes.Where(x => x.QUOCTICH_ID == it.QUOCTICH_ID).FirstOrDefault().QUOCTICH_TEN;
                                        caNhan.QUOCTICH = objQT.QUOCTICH_TEN;
                                    }

                                }
                                if (it.DANTOC_ID != null)
                                {
                                    var objDT = vDC.DANTOCs.Where(x => x.DANTOC_ID == it.DANTOC_ID).FirstOrDefault();
                                    if (objDT != null)
                                    {
                                        caNhan.DANTOC = objDT.DANTOC_TEN;
                                    }

                                }
                                caNhan.DOITUONG_ID = (int)it.DOITUONG_ID;
                                caNhan.DOITUONG_LOAI = caNhan.DOITUONG_LOAI = objDonThu.DOITUONG.DOITUONG_LOAI == 1 ? "Cá nhân" : objDonThu.DOITUONG.DOITUONG_LOAI == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                                caNhan.CANHAN_GIOITINH = it.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                                lstCaNhan_NguoiBiKNTC.Add(caNhan);
                            }
                        }
                        objBiKNTC.caNhans = lstCaNhan_NguoiBiKNTC;
                        objBiKNTC.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID, 1);
                        if (objDonThu.NGUOIUYQUYEN_CANHAN_ID != null)
                        {
                            var obj = vDC.CANHANs.Where(x => x.CANHAN_ID == objDonThu.NGUOIUYQUYEN_CANHAN_ID).FirstOrDefault();

                            objBiKNTC.NGUOIUYQUYEN_TEN = obj.CANHAN_HOTEN;
                            objBiKNTC.NGUOIUYQUYEN_DIACHI = obj.CANHAN_DIACHI_DAYDU;
                            objBiKNTC.NGUOIUYQUYEN_GIOITINH = obj.CANHAN_GIOITINH == false ? "Nam" : "Nữ";
                        }

                    }
                }

                donThuChiTiet.TTNguoiBiToCao = objBiKNTC;
                // End thông tin người bị khiếu nại tố cáo

                // Lấy thông tin nguồn đơn
                objNguonDon.DONTHU_ID = objDonThu.DONTHU_ID;
                objNguonDon.NGUONDON_DONVI = objDonThu.NGUONDON_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.NGUONDON_DONVI_ID);
                objNguonDon.NGUONDON_LOAI = objDonThu.NGUONDON_LOAI == null ? false : (bool)objDonThu.NGUONDON_LOAI;
                objNguonDon.NGUONDON_LOAI_CHITIET = objDonThu.NGUONDON_LOAI_CHITIET == null ? "" : objDonThu.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : objDonThu.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" : objNguonDon.NGUONDON_DONVI;
                objNguonDon.NGUONDON_SOVANBANCHUYEN = objDonThu.NGUONDON_SOVANBANCHUYEN;
                objNguonDon.NGUONDON_NGAYNHANDON = objDonThu.NGAYTAO == null ? "" : Convert.ToDateTime(objDonThu.NGAYTAO).ToString("dd/MM/yyyy");
                objNguonDon.NGUONDON_NGAYCHUYEN = objDonThu.NGUONDON_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYCHUYEN).ToString("dd/MM/yyyy");
                objNguonDon.NGUONDON_NGAYDEDON = objDonThu.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(objDonThu.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                donThuChiTiet.NguonDon = objNguonDon;
                // End lấy thông tin nguồn đơn

                // Đơn thư hướng xử lý
                objHuongXuLy.HUONGXYLY_ID = objDonThu.HUONGXULY_ID == null ? 0 : (int)objDonThu.HUONGXULY_ID;
                objHuongXuLy.HUONGXYLY_TEN = objDonThu.HUONGXULY_TEN;
                objHuongXuLy.HXL_COQUAN = objDonThu.HUONGXULY_DONVI_ID == null ? "" : baoCaoController.GetTenDonVi((int)objDonThu.HUONGXULY_DONVI_ID);
                objHuongXuLy.HXL_THOIHANGQ = objDonThu.HUONGXULY_THOIHANGIAIQUET == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_THOIHANGIAIQUET).ToString("dd/MM/yyyy");
                objHuongXuLy.HXL_THAMQUYENGQ = objDonThu.HUONGXULY_THAMQUYENGIAIQUYET_TEN;
                objHuongXuLy.HXL_NGAYCHUYEN = objDonThu.HUONGXULY_NGAYCHUYEN == null ? "" : Convert.ToDateTime(objDonThu.HUONGXULY_NGAYCHUYEN).ToString("dd/MM/yyyy");
                objHuongXuLy.HXL_NGUOITIEPNHAN = objDonThu.HUONGXULY_CANBO == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_CANBO);
                objHuongXuLy.HXL_NGUOIDUYET = objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID == null ? "" : baoCaoController.GetTenCanBo((int)objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID);
                objHuongXuLy.HXL_CHUCVU = objDonThu.HUONGXULY_CHUCVU_TEN;
                objHuongXuLy.HXL_YKIENXL = objDonThu.HUONGXULY_YKIEN_XULY;
                objHuongXuLy.HXL_GHICHU = objDonThu.DONTHU_GHICHU;
                // objHuongXuLy.HXL_URLFILE = System.Web.HttpContext.Current.Server.MapPath(vPathFile) + "Files/FileMau.pdf";
                objHuongXuLy.HoSo = baoCaoController.GetUrlFile(objDonThu.DONTHU_ID, 4);

                donThuChiTiet.HuongXuLy = objHuongXuLy;
                // End Đơn thư hướng xử lý
            }

            return donThuChiTiet;
        }
        #endregion

        #region Thống kê
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage KQGQKHIEUNAITOCAO(string TuNgay, string DenNgay)
        {
            try
            {
                UrlInfo urlInfo = new UrlInfo();
                string url = "";
                 string vTuNgay = "01/01/1990";
                string vDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                if (!string.IsNullOrEmpty(TuNgay))
                {
                    vTuNgay = TuNgay;
                }
                if (!string.IsNullOrEmpty(DenNgay))
                {
                    vDenNgay = DenNgay;
                }
                Byte[] fileBytes = baoCaoController.KQGQ_KHIEUNAITOCAO(TuNgay, DenNgay);

                if (fileBytes != null)
                {
                    url = @"DesktopModules/KNTC/bieumau_api/THKQ_XULYDON_KNTC_" + ClassCommon.GetUploadDateTime().ToString() + ".xlsx";
                    System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + url, fileBytes);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                urlInfo.Url = vDomain + url;
                res.Content = new StringContent(urlInfo.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage KQTIEPCONGDAN(string TuNgay, string DenNgay)
        {
            try
            {
                UrlInfo urlInfo = new UrlInfo();
                string url = "";
                string vTuNgay = "01/01/1990";
                string vDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                if (!string.IsNullOrEmpty(TuNgay))
                {
                    vTuNgay = TuNgay;
                }
                if (!string.IsNullOrEmpty(DenNgay))
                {
                    vDenNgay = DenNgay;
                }
                Byte[] fileBytes = baoCaoController.KQ_TIEPCONGDAN(vTuNgay, vDenNgay);

                if (fileBytes != null)
                {
                    url = @"DesktopModules/KNTC/bieumau_api/KQ_TIEPCONGDAN_" + ClassCommon.GetUploadDateTime().ToString() + ".xlsx";
                    System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + url, fileBytes);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                urlInfo.Url = vDomain + url;
                res.Content = new StringContent(urlInfo.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TKDONTRONGTHANG(string Nam, string NguonDon, string DonViID)
        {
            try
            {
                UrlInfo urlInfo = new UrlInfo();
                string url = "";               
                Byte[] fileBytes = baoCaoController.TK_DONTHU_THANG(Nam, NguonDon, DonViID);

                if (fileBytes != null)
                {
                  
                    url = @"DesktopModules/KNTC/bieumau_api/TK_DONTHUTRONGTHANG_" + ClassCommon.GetUploadDateTime().ToString() + ".xlsx";
                    System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + url, fileBytes);

                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                urlInfo.Url = vDomain + url;
                res.Content = new StringContent(urlInfo.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TKDONTHEONGUON(string TuNgay, string DenNgay, string NguonDon)
        {
            try
            {
                UrlInfo urlInfo = new UrlInfo();
                string url = "";
                string vTuNgay = "01/01/1990";
                string vDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                if (!string.IsNullOrEmpty(TuNgay))
                {
                    vTuNgay = TuNgay;
                }
                if (!string.IsNullOrEmpty(DenNgay))
                {
                    vDenNgay = DenNgay;
                }
                Byte[] fileBytes = baoCaoController.TK_DONTHU_NGUON(vTuNgay, vDenNgay, NguonDon);

                if (fileBytes != null)
                {

                    url = @"DesktopModules/KNTC/bieumau_api/TK_DONTHUTHEONGUONDON_" + ClassCommon.GetUploadDateTime().ToString() + ".xlsx";
                    System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + url, fileBytes);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                urlInfo.Url = vDomain + url;
                res.Content = new StringContent(urlInfo.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage TKTRUNGDON(string pTuNgay, string pDenNgay, string pLoaiDoiTuong)
        {
            try
            {
                string vTuNgay = "01/01/1990";
                string vDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                if (!string.IsNullOrEmpty(pTuNgay))
                {
                    vTuNgay = pTuNgay;
                }
                if (!string.IsNullOrEmpty(pDenNgay))
                {
                    vDenNgay = pDenNgay;
                }

                string url = "";
                Byte[] fileBytes = baoCaoController.TK_DONTHU_TRUNG(vTuNgay, vDenNgay, pLoaiDoiTuong);

                if (fileBytes != null)
                {
                    url =  @"DesktopModules/KNTC/bieumau_api/TK_TRUNGDON_" + ClassCommon.GetUploadDateTime().ToString() + ".xlsx";
                    System.IO.File.WriteAllBytes(HttpContext.Current.Request.PhysicalApplicationPath + url, fileBytes);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent((vDomain + url).ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }
        #endregion

        #region DanhMuc
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DonThu_DoiTuong_Loai()
        {
            List<DoiTuongLoai> lstDTL = new List<DoiTuongLoai>();
            try
            {

                var objDonThu = vDC.KNTC_DONTHU_DOITUONG_LOAIs.ToList();
                for (int i = 0; i < 4; i++)
                {
                    DoiTuongLoai dtl = new DoiTuongLoai();
                    dtl.DOITUONGLOAI_ID = i;
                    dtl.DOITUONGLOAI_TEN = i == 0 ? "Tất cả đối tượng" : i == 1 ? "Cá nhân" : i == 2 ? "Đoàn đông người" : "Cơ quan tổ chức";
                    dtl.DOITUONGLOAI_COUNT = objDonThu.Where(x => i == 0 || x.DOITUONG_LOAI == i).Count();
                    lstDTL.Add(dtl);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstDTL.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DMNguonDon()
        {
            List<DMNguonDon> lstND = new List<DMNguonDon>();
            try
            {             
                for (int i = -1; i < 3; i++)
                {
                    DMNguonDon objNguonDon = new DMNguonDon();
                    objNguonDon.ND_ID = i.ToString();
                    objNguonDon.ND_TEN = i == -1 ? "Tất cả" : i == 0 ? "Trực tiếp" : i == 1 ? "Bưu chính" : "Cơ quan khác chuyển tới";

                    lstND.Add(objNguonDon);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstND.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DMDonVi()
        {
            List<DMDonVi> lstDV = new List<DMDonVi>();
            try
            {
                var objDonVi = vDC.DONVIs.ToList();


                DMDonVi objTATCA = new DMDonVi();
                objTATCA.DV_ID = "0";
                objTATCA.DV_TEN = "Tất cả đơn vị";
                lstDV.Add(objTATCA);

                foreach (var dv in objDonVi)
                {
                    DMDonVi objDV = new DMDonVi();
                    objDV.DV_TEN = dv.TENDONVI;
                    objDV.DV_ID = dv.DONVI_ID.ToString();

                    lstDV.Add(objDV);
                }
                var res = Request.CreateResponse(HttpStatusCode.OK);
                res.Content = new StringContent(lstDV.ToJson(), System.Text.Encoding.UTF8, "application/json");
                return res;
            }
            catch (Exception Ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }
        }
        #endregion

        public class RouteMapper : IServiceRouteMapper
        {
            public void RegisterRoutes(IMapRoute mapRouteManager)
            {
                mapRouteManager.MapHttpRoute("KNTC", "default", "{controller}/{action}", new[] { "KNTC" });
            }
        }      
    }
}