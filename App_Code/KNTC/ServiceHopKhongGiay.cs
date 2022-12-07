using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Web.Api;

namespace KNTC
{

    /// <summary>
    /// Summary description for ServiceHopKhongGiay
    /// </summary>
    public class ServiceHopKhongGiayController : DnnApiController
    {
        //    HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();

        //    public ServiceHopKhongGiayController()
        //    {
        //        //
        //        // TODO: Add constructor logic here
        //        //
        //    }

        //    [AllowAnonymous]
        //    [HttpGet]
        //    public HttpResponseMessage HelloWorld()
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        //    }

        //    [AllowAnonymous]
        //    [System.Web.Http.HttpPost]
        //    public HttpResponseMessage Login(string pUserName, string pPassword, string pPlayerId)
        //    {
        //        PhienHopController vPhienHopController = new PhienHopController();

        //        var res = Request.CreateResponse(HttpStatusCode.OK);

        //        res.Headers.Add("Access-Control-Allow-Origin", "http://hop.liink.vn");

        //        res.Content = new StringContent(vPhienHopController.Login(pUserName, pPassword, pPlayerId).ToJson(), System.Text.Encoding.UTF8, "application/json");
        //        return res;
        //    }

        //    /// <summary>
        //    /// Get đại biểu ẩn những đại biểu đã chọn 
        //    /// </summary>
        //    /// <param name="daibieu"></param>
        //    /// <returns></returns>
        //    [AllowAnonymous]
        //    [HttpGet]
        //    public HttpResponseMessage GetDaiBieu(string daibieu, string an)
        //    {
        //        try
        //        {
        //            if (daibieu == null)
        //            {
        //                daibieu = "";
        //            }
        //            var obj = from DONVI in vDataContext.DONVIs
        //                      select new
        //                      {
        //                          key = DONVI.DONVI_ID, 
        //                          title = DONVI.TENDONVI,
        //                          expanded = true,
        //                          children = (from PhongBan in DONVI.PhongBans
        //                                      select new
        //                                      {
        //                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID,
        //                                          title = PhongBan.TENPHONGBAN,
        //                                          unselectable = Check_Enable(DONVI.DONVI_ID + "_" + PhongBan.PB_ID, an),
        //                                          children = (from NguoiDung in PhongBan.NGUOIDUNGs
        //                                                      where NguoiDung.LOAI == (int)CommonEnum.LoaiNguoiDung.DaiBieu && NguoiDung.TRANGTHAI == true
        //                                                      select new
        //                                                      {
        //                                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID,
        //                                                          title = NguoiDung.TENNGUOIDUNG,
        //                                                          selected = Getvalue(DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID, daibieu),                                                              
        //                                                          unselectable = Check_Enable(DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID, an)
        //                                                      })
        //                                      })
        //                                      .Distinct()
        //                      };
        //            obj = obj.Where(x => x.children.Where(y => y.children.Count() > 0).Count() > 0);

        //            var res = Request.CreateResponse(HttpStatusCode.OK);

        //            //res.Headers.Add("Access-Control-Allow-Origin", "http://hop.liink.vn");

        //            res.Content = new StringContent(obj.ToJson(), System.Text.Encoding.UTF8, "application/json");
        //            return res;
        //        }
        //        catch (Exception Ex)
        //        {
        //            return null;
        //        }
        //    }



        //    /// <summary>
        //    /// Get đại biểu ẩn những đại biểu đã chọn 
        //    /// </summary>
        //    /// <param name="daibieu"></param>

        //    /// <returns></returns>
        //    [AllowAnonymous]
        //    [HttpGet]
        //    public HttpResponseMessage GetNguoiNhanThongBao(string nguoinhan)
        //    {
        //        try
        //        {
        //            if (nguoinhan == null)
        //            {
        //                nguoinhan = "";
        //            }
        //            var obj = from DONVI in vDataContext.DONVIs
        //                      select new
        //                      {
        //                          key = DONVI.DONVI_ID,
        //                          title = DONVI.TENDONVI,
        //                          expanded = true,
        //                          children = (from PhongBan in DONVI.PhongBans
        //                                      select new
        //                                      {
        //                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID,
        //                                          title = PhongBan.TENPHONGBAN,
        //                                          children = (from NguoiDung in PhongBan.NGUOIDUNGs      
        //                                                      where NguoiDung.UserId != null && NguoiDung.TRANGTHAI == true
        //                                                      select new
        //                                                      {
        //                                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID,
        //                                                          title = NguoiDung.TENNGUOIDUNG,
        //                                                          selected = Getvalue(DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID, nguoinhan),
        //                                                      })
        //                                      })
        //                                      .Distinct()
        //                      };
        //            obj = obj.Where(x => x.children.Where(y => y.children.Count() > 0).Count() > 0);

        //            var res = Request.CreateResponse(HttpStatusCode.OK);

        //            //res.Headers.Add("Access-Control-Allow-Origin", "http://hop.liink.vn");

        //            res.Content = new StringContent(obj.ToJson(), System.Text.Encoding.UTF8, "application/json");
        //            return res;
        //        }
        //        catch (Exception Ex)
        //        {
        //            return null;
        //        }
        //    }


        //    [AllowAnonymous]
        //    [HttpGet]
        //    public HttpResponseMessage GetKhachMoi(string khachmoi)
        //    {
        //        try
        //        {
        //            if (khachmoi == null)
        //            {
        //                khachmoi = "";
        //            }
        //            var obj = from DONVI in vDataContext.DONVIs
        //                      select new
        //                      {
        //                          key = DONVI.DONVI_ID,
        //                          title = DONVI.TENDONVI,
        //                          expanded = true,
        //                          children = (from PhongBan in DONVI.PhongBans
        //                                      select new
        //                                      {
        //                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID,
        //                                          title = PhongBan.TENPHONGBAN,
        //                                          children = (from NguoiDung in PhongBan.NGUOIDUNGs
        //                                                      where NguoiDung.LOAI == (int)CommonEnum.LoaiNguoiDung.KhachMoi && NguoiDung.TRANGTHAI == true
        //                                                      select new
        //                                                      {
        //                                                          key = DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID,
        //                                                          title = NguoiDung.TENNGUOIDUNG,
        //                                                          selected = Getvalue(DONVI.DONVI_ID + "_" + PhongBan.PB_ID + "_" + NguoiDung.NGUOIDUNG_ID, khachmoi),                                                              
        //                                                      })
        //                                      })
        //                                      .Distinct()
        //                      };
        //            obj = obj.Where(x => x.children.Where(y => y.children.Count() > 0).Count() > 0);

        //            var res = Request.CreateResponse(HttpStatusCode.OK);

        //            //res.Headers.Add("Access-Control-Allow-Origin", "http://hop.liink.vn");

        //            res.Content = new StringContent(obj.ToJson(), System.Text.Encoding.UTF8, "application/json");
        //            return res;
        //        }
        //        catch (Exception Ex)
        //        {
        //            return null;
        //        }
        //    }      

        //    /// <summary>
        //    /// Không cho đại biểu theo danh sách id
        //    /// </summary>
        //    /// <param name="pID"></param>
        //    /// <param name="pParramIDs"></param>
        //    /// <returns></returns>
        //    public bool Check_Enable(string pID, string pParramIDs)
        //    {
        //        if (pParramIDs.Contains(pID))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }      



        //    public bool Getvalue(string pID, string pParramIDs)
        //    {
        //        if(pParramIDs.Contains(pID))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }

        //    /// <summary>
        //    /// Authur NHTTAI
        //    /// Get số lượng thông báo chưa đọc
        //    /// </summary>
        //    /// <param name="pDomain">Tên miền hệ thống</param>
        //    /// <returns></returns>
        //    [AllowAnonymous]
        //    [HttpGet]
        //    public DonViTrienKhaiInfo CheckDonViTrienKhai(string pDomain)
        //    {
        //        if (pDomain != "")
        //        {
        //            if (pDomain == "https://hopkhonggiay.vinhlong.gov.vn")
        //            {
        //                DonViTrienKhaiInfo donViTrienKhaiInfo = new DonViTrienKhaiInfo();
        //                donViTrienKhaiInfo.DONVI_TK_ID = 1;
        //                donViTrienKhaiInfo.DONVI_TK_TEN = "UBND TỈNH VĨNH LONG";
        //                donViTrienKhaiInfo.DONVI_TK_TENHETHONG = "HỆ THỐNG HỌP KHÔNG GIẤY TỈNH VĨNH LONG";
        //                donViTrienKhaiInfo.DONVI_TK_TRANGTHAI = 1;
        //                donViTrienKhaiInfo.DONVI_TK_API = "https://hopkhonggiay.vinhlong.gov.vn/DesktopModules/HOPKHONGGIAY/Services/HopKhongGiayService.asmx";
        //                return donViTrienKhaiInfo;
        //            }
        //            else if (pDomain == "http://hop.liink.vn")
        //            {
        //                DonViTrienKhaiInfo donViTrienKhaiInfo = new DonViTrienKhaiInfo();
        //                donViTrienKhaiInfo.DONVI_TK_ID = 2;
        //                donViTrienKhaiInfo.DONVI_TK_TEN = "CÔNG TY TNHH PHẦN MỀM VÀ DỊCH VỤ CÔNG NGHỆ THÔNG TIN LIINK";
        //                donViTrienKhaiInfo.DONVI_TK_TENHETHONG = "PHẦN MỀM HỌP KHÔNG GIẤY";
        //                donViTrienKhaiInfo.DONVI_TK_TRANGTHAI = 1;
        //                donViTrienKhaiInfo.DONVI_TK_API = "http://hop.liink.vn/DesktopModules/HOPKHONGGIAY/Services/HopKhongGiayService.asmx";
        //                return donViTrienKhaiInfo;
        //            }
        //            else if (pDomain == "local_test")
        //            {
        //                DonViTrienKhaiInfo donViTrienKhaiInfo = new DonViTrienKhaiInfo();
        //                donViTrienKhaiInfo.DONVI_TK_ID = 3;
        //                donViTrienKhaiInfo.DONVI_TK_TEN = "local_test";
        //                donViTrienKhaiInfo.DONVI_TK_TENHETHONG = "local_test";
        //                donViTrienKhaiInfo.DONVI_TK_TRANGTHAI = 1;
        //                donViTrienKhaiInfo.DONVI_TK_API = "http://192.168.1.24:9951/DesktopModules/HOPKHONGGIAY/Services/HopKhongGiayService.asmx";
        //                return donViTrienKhaiInfo;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }


        //}


        public class RouteMapper : IServiceRouteMapper
        {
            public void RegisterRoutes(IMapRoute mapRouteManager)
            {
                mapRouteManager.MapHttpRoute("HOPKHONGGIAY", "default", "{controller}/{action}", new[] { "HOPKHONGGIAY" });
            }
        }
    }
}