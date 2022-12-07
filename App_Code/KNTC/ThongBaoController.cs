using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using DotNetNuke.Services.Log.EventLog;

namespace HOPKHONGGIAY
{
    /// <summary>
    /// Summary description for TaiLieuController
    /// </summary>
    public class ThongBaoController
    {
        //#region Khai báo chung
        //HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        //#endregion

        //#region Web area
        ///// <summary>
        ///// Get danh sách tất cả thông báo
        ///// </summary>
        ///// <returns></returns>
        //public List<HKG_THONGBAO> GetDanhSachThongBao()
        //{
        //    try
        //    {
        //        List<HKG_THONGBAO> vThongBaoInfos = vDataContext.HKG_THONGBAOs.ToList(); ;
        //        return vThongBaoInfos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}



        ///// <summary>
        ///// Get danh sách thông báo - web
        ///// </summary>
        ///// <param name="pKeyWord"></param>
        ///// <param name="pTuNgay"></param>
        ///// <param name="pDenNgay"></param>
        ///// <param name="pTrangTrai"></param>
        ///// <returns></returns>
        //public List<HKG_THONGBAO> GetDanhSachThongBao(string pKeyWord)
        //{
        //    try
        //    {
        //        List<HKG_THONGBAO> vThongBaoInfos = vDataContext.HKG_THONGBAOs.Where(x => (SqlMethods.Like(x.Title, "%" + pKeyWord + "%") || SqlMethods.Like(x.Content, "%" + pKeyWord + "%"))).OrderByDescending(x => x.SendDate).ToList();
        //        return vThongBaoInfos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        ///// <summary>
        ///// Get danh sách thông báo - web
        ///// </summary>
        ///// <param name="pKeyWord"></param>
        ///// <param name="pTuNgay"></param>
        ///// <param name="pDenNgay"></param>
        ///// <param name="pTrangTrai"></param>
        ///// <returns></returns>
        //public List<HKG_THONGBAO> GetDanhSachThongBao(string pKeyWord, DateTime pTuNgay, DateTime pDenNgay, int pTrangTrai)
        //{
        //    try
        //    {
        //        List<HKG_THONGBAO> vThongBaoInfos = vDataContext.HKG_THONGBAOs.Where(x => (SqlMethods.Like(x.Title, "%" + pKeyWord + "%") || SqlMethods.Like(x.Content, "%" + pKeyWord + "%")) && (x.SendDate >= pTuNgay && x.SendDate <= pDenNgay) && (x.Status == pTrangTrai || pTrangTrai == -1)).ToList();
        //        return vThongBaoInfos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Get danh sách thông báo theo Id
        ///// </summary>
        ///// <param name="pPhienHopId"></param>
        ///// <returns></returns>
        //public List<HKG_THONGBAO> GetDanhSachThongBaoByPhienHopId(int pPhienHopId)
        //{
        //    try
        //    {
        //        var vThongBaoInfos = vDataContext.HKG_THONGBAOs.Where(x => x.PhienHop_Id == pPhienHopId).ToList(); ;
        //        return vThongBaoInfos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Thêm người dùng được gởi thông báo vào CSDL
        ///// </summary>
        ///// <param name="pThongBaoNguoiDungInfo"></param>
        //public void InsertThongBao_NguoiDung(THONGBAO_NGUOIDUNG pThongBaoNguoiDungInfo)
        //{
        //    try
        //    {
        //        vDataContext.THONGBAO_NGUOIDUNGs.InsertOnSubmit(pThongBaoNguoiDungInfo);
        //        vDataContext.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        ///// <summary>
        ///// Thêm nhiều người dùng được gởi thông báo
        ///// </summary>
        ///// <param name="pThongBaoNguoiDungInfos"></param>
        //public void InsertThongBao_NguoiDungs(List<THONGBAO_NGUOIDUNG> pThongBaoNguoiDungInfos)
        //{
        //    try
        //    {
        //        vDataContext.THONGBAO_NGUOIDUNGs.InsertAllOnSubmit(pThongBaoNguoiDungInfos);
        //        vDataContext.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        ///// One Signal
        ///// Chức năng gửi thông báo 
        ///// pUserIDs: Danh sách User ID gửi thông báo, nếu trường hợp list null sẽ gửi tất cả người dùng
        ///// pContent: Nội dung thông báo
        ///// pData: Dữ liệu truyền vào string json Ex: pData = "\"data\": {\"page\": \"PhieuKham_ChiTiet\", \"PhieuKhamID\": \"" + ThongBaoLichKham.dangkykhamid + "\"}," (page: Tên trang dưới ứng dụng).
        ///// pAppURL: Địa chỉ ứng dụng
        ///// pWebURL Địa chỉ web URL khi click chọn thông báo
        ///// Update: Ngô Hoài Hận   
        //public bool SendNotifications(List<int> pNguoiDungId, string pSendDateTime, string pContent, string pData, string pAppURL, string pWebURL)
        //{
        //    string vConditionUser = "";
        //    var vThietLapInfo = vDataContext.THONGBAO_THIETLAPs.FirstOrDefault();
        //    if (pNguoiDungId != null && pNguoiDungId.Count > 0)
        //    {
        //        string vPlayerIDs = "";
        //        List<string> devices = (from a in vDataContext.NGUOIDUNG_DEVICEs
        //                                          where pNguoiDungId.Contains(a.NGUOIDUNG_ID) && a.EnableThongBao == true && a.PlayerId.Length > 5
        //                                          select a.PlayerId).Distinct().ToList();
        //        if (devices.Count > 0)
        //        {
        //            for (int i = 0; i < devices.Count; i++)
        //            {
        //                vPlayerIDs = (vPlayerIDs + (vPlayerIDs != "" ? "," : "") + "\"" + devices[i] + "\"");
        //            }
        //            vConditionUser = "\"include_player_ids\": [" + vPlayerIDs + "]";
        //        }
        //        else
        //        {
        //            vConditionUser = "\"included_segments\": []";//  vConditionUser = "\"included_segments\": [\"Subscribed Users\"]";
        //        }
        //    }
        //    else
        //    {
        //        vConditionUser = "\"included_segments\": []";
        //    }
        //    //pSendDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    string vDeliverySendAfter = "";
        //    if (pSendDateTime != null && pSendDateTime != "")
        //    {
        //        vDeliverySendAfter = "\"send_after\": \"" + pSendDateTime + " GMT+0700" + "\",";//"2015-09-24 14:00:00 GMT-0700"
        //    }
        //    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
        //    request.KeepAlive = true;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; charset=utf-8";

        //    request.Headers.Add("authorization", "Basic " + vThietLapInfo.PUSH_OneSignalRestAPIKey);

        //    byte[] byteArray = Encoding.UTF8.GetBytes("{"
        //                                            + "\"app_id\": \"" + vThietLapInfo.PUSH_OneSignalAppId + "\","
        //                                            + "\"contents\": {\"en\": \"" + pContent + "\"},"
        //                                            + pData
        //                                            + vDeliverySendAfter
        //                                            + vConditionUser + "}");
        //    string responseContent = null;

        //    try
        //    {
        //        using (var writer = request.GetRequestStream())
        //        {
        //            writer.Write(byteArray, 0, byteArray.Length);
        //        }

        //        using (var response = request.GetResponse() as HttpWebResponse)
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = reader.ReadToEnd();
        //            }
        //        }
        //        return true;
        //    }
        //    catch (WebException ex)
        //    {
        //        var objEventLog = new EventLogController();
        //        objEventLog.AddLog("Notification Exception", ex.Message + ": " + ex.ToString(), EventLogController.EventLogType.ADMIN_ALERT);
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
        //        return false;
        //    }
        //    System.Diagnostics.Debug.WriteLine(responseContent);
        //}

        //#endregion

        //public ThongBaoController()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

        //public HKG_THONGBAO GetThongBaoById(int pThongBaoId)
        //{
        //    HKG_THONGBAO obj = vDataContext.HKG_THONGBAOs.FirstOrDefault(a => a.Id == pThongBaoId);

        //    return obj;
        //}

        //public string GetPlayerIdsByPhienHopId(int pPhienHopId, List<int> pDaiBieuId)
        //{
        //    List<string> vLstIDs = (from a in vDataContext.PHIENHOP_NGUOIDUNGs
        //                            where a.PHIENHOP_ID == pPhienHopId
        //                            && (pDaiBieuId.Count == 0 ? true : pDaiBieuId.Contains(a.NGUOIDUNG_ID))
        //                            join nguoidung in vDataContext.NGUOIDUNGs on a.NGUOIDUNG_ID equals nguoidung.NGUOIDUNG_ID
        //                            join b in vDataContext.Devices_Apps on nguoidung.UserId equals b.UserId
        //                            select b.PlayerId).ToList();

        //    string combindedString = string.Join(",", vLstIDs);

        //    return combindedString;
        //}

        //public List<int> GetChuToaCuocHop(int pPhienHopId)
        //{
        //    List<int> pChuToaIds = (from a in vDataContext.PHIENHOP_NGUOIDUNGs
        //                            where a.PHIENHOP_ID == pPhienHopId && a.LOAI_DAIBIEU == (int)CommonEnum.LoaiDaiBieu.ChuTri
        //                            select a.NGUOIDUNG_ID).ToList();

        //    return pChuToaIds;
        //}

        //public int InsertThongBao(HKG_THONGBAO pThongBao, int pNguoiDungId)
        //{
        //    vDataContext.HKG_THONGBAOs.InsertOnSubmit(pThongBao);
        //    vDataContext.SubmitChanges();

        //    var vID = vDataContext.HKG_THONGBAOs.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

        //    THONGBAO_NGUOIDUNG objThongBaoNguoiDung = new THONGBAO_NGUOIDUNG();
        //    objThongBaoNguoiDung.ThongBaoId = pThongBao.Id;
        //    objThongBaoNguoiDung.NguoiDungId = pNguoiDungId;
        //    objThongBaoNguoiDung.TrangThaiGuiThongBao = true;
        //    objThongBaoNguoiDung.DaXemNoiDung = false;
        //    vDataContext.THONGBAO_NGUOIDUNGs.InsertOnSubmit(objThongBaoNguoiDung);
        //    vDataContext.SubmitChanges();

        //    return pThongBao.Id;
        //}

        ///// <summary>
        ///// Insert thông báo - web
        ///// </summary>
        ///// <param name="pThongBao"></param>
        ///// <returns></returns>
        //public int InsertThongBao(HKG_THONGBAO pThongBao)
        //{
        //    vDataContext.HKG_THONGBAOs.InsertOnSubmit(pThongBao);
        //    vDataContext.SubmitChanges();
        //    var vID = vDataContext.HKG_THONGBAOs.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
        //    return vID;
        //}

        //public bool UpdateThongBao(int pId, HKG_THONGBAO pThongBao)
        //{
        //    try
        //    {
        //        HKG_THONGBAO obj = vDataContext.HKG_THONGBAOs.SingleOrDefault(a => a.Id == pId);
        //        if (obj != null)
        //        {
        //            obj.Type = pThongBao.Type;
        //            obj.Kind = pThongBao.Kind;
        //            obj.Title = pThongBao.Title;
        //            obj.Content = pThongBao.Content;
        //            obj.CreateDate = pThongBao.CreateDate;
        //            obj.SendDate = pThongBao.SendDate;
        //            obj.Status = pThongBao.Status;
        //            obj.PhienHop_Id = pThongBao.PhienHop_Id;
        //            vDataContext.SubmitChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool CheckDaDocAll(int pUserId)
        //{
        //    try
        //    {
        //        //Tìm danh sách các thông báo mà người này chưa đọc
        //        List<THONGBAO_NGUOIDUNG> vLstThongBao = (from a in vDataContext.THONGBAO_NGUOIDUNGs
        //                                                 where (a.DaXemNoiDung == false || a.DaXemNoiDung == null)
        //                                                 && a.NguoiDungId == pUserId
        //                                                 select a).ToList();

        //        foreach (var it in vLstThongBao)
        //        {
        //            THONGBAO_NGUOIDUNG obj = vDataContext.THONGBAO_NGUOIDUNGs.SingleOrDefault(a => a.ThongBaoId == it.ThongBaoId && a.NguoiDungId == it.NguoiDungId);
        //            obj.DaXemNoiDung = true;
        //            vDataContext.SubmitChanges();
        //        }

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool CheckDaDoc(int pUserId, int pThongBaoId)
        //{
        //    try
        //    {
        //        THONGBAO_NGUOIDUNG obj = vDataContext.THONGBAO_NGUOIDUNGs.SingleOrDefault(a => a.ThongBaoId == pThongBaoId && a.NguoiDungId == pUserId);
        //        obj.DaXemNoiDung = true;
        //        vDataContext.SubmitChanges();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public int GetSoLuongThongBaoChuaDoc(int pUserId)
        //{
        //    //int count = (from a in vDataContext.THONGBAO_NGUOIDUNGs
        //    //             where (a.DaXemNoiDung == false || a.DaXemNoiDung == null)
        //    //             && a.NguoiDungId == pUserId
        //    //             select a).Count();

        //    int count = (from a in vDataContext.HKG_THONGBAO_NGUOIDUNG_VIEWs
        //                 where a.NGUOINHANTHONGBAO_ID == pUserId && (a.DAXEM_THONGBAO == false || a.DAXEM_THONGBAO == null)
        //                 orderby a.THONGBAO_ID descending
        //                 select new ThongBaoInfo()
        //                 {
        //                     THONGBAO_ID = a.THONGBAO_ID,
        //                     PHIENHOP_ID = a.PHIENHOP_ID,
        //                     NGUOINHANTHONGBAO_ID = a.NGUOINHANTHONGBAO_ID,
        //                     TIEUDE_THONGBAO = a.TIEUDE_THONGBAO,
        //                     NOIDUNG_THONGBAO = a.NOIDUNG_THONGBAO,
        //                     DAXEM_THONGBAO = a.DAXEM_THONGBAO,
        //                     TIEUDE_PHIENHOP = a.TIEUDE_PHIENHOP,
        //                     THOIGIANBATDAU_PHIENHOP = a.THOIGIANBATDAU_PHIENHOP,
        //                     CHUTRI_ID = a.CHUTRI_ID,
        //                     CHUTRI_TEN = a.CHUTRI_TEN,
        //                     PHONGHOP_ID = a.PHONGHOP_ID,
        //                     TENPHONGHOP = a.TENPHONGHOP,
        //                     THOIGIAN_GUI = a.THOIGIAN_GUI
        //                 }).Count();

        //    return count;
        //}

        //public bool CheckEnableSendThongBao(int pUserId)
        //{
        //    NGUOIDUNG_DEVICE objNguoiDungDevice = (from a in vDataContext.NGUOIDUNG_DEVICEs
        //                                           where a.NGUOIDUNG_ID == pUserId
        //                                           select a).FirstOrDefault();

        //    if (objNguoiDungDevice != null)
        //    {
        //        return objNguoiDungDevice.EnableThongBao == true ? true : false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool CheckEnableAmTinNhan(int pUserId)
        //{
        //    NGUOIDUNG_DEVICE objNguoiDungDevice = (from a in vDataContext.NGUOIDUNG_DEVICEs
        //                                           where a.NGUOIDUNG_ID == pUserId
        //                                           select a).FirstOrDefault();

        //    if (objNguoiDungDevice != null)
        //    {
        //        return objNguoiDungDevice.EnableAmTinNhan == true ? true : false;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        /////
        ///// One Signal
        ///// Chức năng gửi thông báo 
        ///// pUserIDs: Danh sách User ID gửi thông báo, nếu trường hợp list null sẽ gửi tất cả người dùng
        ///// pContent: Nội dung thông báo
        ///// pData: Dữ liệu truyền vào string json Ex: pData = "\"data\": {\"page\": \"PhieuKham_ChiTiet\", \"PhieuKhamID\": \"" + ThongBaoLichKham.dangkykhamid + "\"}," (page: Tên trang dưới ứng dụng).
        ///// pAppURL: Địa chỉ ứng dụng
        ///// pWebURL Địa chỉ web URL khi click chọn thông báo
        ///// Update: Ngo
        //#region One Signal
        ////public bool SendNotifications(List<int> pUserIDs, string pSendDateTime, string pContent, string pData, string pAppURL, string pWebURL)
        ////{
        ////    string vConditionUser = "";
        ////    if (pUserIDs != null && pUserIDs.Count > 0)
        ////    {
        ////        string vPlayerIDs = "";
        ////        List<Devices_App> devices = (from a in vDataContext.Devices_Apps
        ////                                     where pUserIDs.Contains(a.UserId.Value)
        ////                                     select a).ToList();
        ////        if (devices.Count > 0)
        ////        {
        ////            for (int i = 0; i < devices.Count; i++)
        ////            {
        ////                vPlayerIDs = (vPlayerIDs + (vPlayerIDs != "" ? "," : "") + "\"" + devices[i].PlayerId + "\"");
        ////            }
        ////            vConditionUser = "\"include_player_ids\": [" + vPlayerIDs + "]";
        ////        }
        ////        else
        ////        {
        ////            vConditionUser = "\"included_segments\": [\"Subscribed Users\"]";
        ////        }
        ////    }
        ////    else
        ////    {
        ////        vConditionUser = "\"included_segments\": [\"Subscribed Users\"]";
        ////    }
        ////    //pSendDateTime.ToString("yyyy-MM-dd HH:mm") 
        ////    string vDeliverySendAfter = "";
        ////    if (pSendDateTime != null && pSendDateTime != "")
        ////    {
        ////        vDeliverySendAfter = "\"send_after\": \"" + pSendDateTime + ":00 GMT+0700" + "\",";//"2015-09-24 14:00:00 GMT-0700"
        ////    }
        ////    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
        ////    request.KeepAlive = true;
        ////    request.Method = "POST";
        ////    request.ContentType = "application/json; charset=utf-8";

        ////    request.Headers.Add("authorization", "Basic " + ClassParameter.OneSignalRestAPIKey);

        ////    byte[] byteArray = Encoding.UTF8.GetBytes("{"
        ////                                            + "\"app_id\": \"" + ClassParameter.OneSignalAppID + "\","
        ////                                            + "\"contents\": {\"en\": \"" + pContent + "\"},"
        ////                                            + pData
        ////                                            + vDeliverySendAfter
        ////                                            + vConditionUser + "}");

        ////    string responseContent = null;

        ////    try
        ////    {
        ////        using (var writer = request.GetRequestStream())
        ////        {
        ////            writer.Write(byteArray, 0, byteArray.Length);
        ////        }

        ////        using (var response = request.GetResponse() as HttpWebResponse)
        ////        {
        ////            using (var reader = new StreamReader(response.GetResponseStream()))
        ////            {
        ////                responseContent = reader.ReadToEnd();
        ////            }
        ////        }
        ////        return true;
        ////    }
        ////    catch (WebException ex)
        ////    {
        ////        System.Diagnostics.Debug.WriteLine(ex.Message);
        ////        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
        ////        return false;
        ////    }

        ////    System.Diagnostics.Debug.WriteLine(responseContent);
        ////}
        //#endregion

        //public List<ThongBaoInfo> GetThongBao(int pUserId)
        //{
        //    List<ThongBaoInfo> objThongBaos = (from a in vDataContext.HKG_THONGBAO_NGUOIDUNG_VIEWs
        //                                       where a.NGUOINHANTHONGBAO_ID == pUserId
        //                                       orderby a.THONGBAO_ID descending
        //                                       select new ThongBaoInfo()
        //                                       {
        //                                           THONGBAO_ID = a.THONGBAO_ID,
        //                                           PHIENHOP_ID = a.PHIENHOP_ID,
        //                                           NGUOINHANTHONGBAO_ID = a.NGUOINHANTHONGBAO_ID,
        //                                           TIEUDE_THONGBAO = a.TIEUDE_THONGBAO,
        //                                           NOIDUNG_THONGBAO = a.NOIDUNG_THONGBAO,
        //                                           DAXEM_THONGBAO = a.DAXEM_THONGBAO,
        //                                           TIEUDE_PHIENHOP = a.TIEUDE_PHIENHOP,
        //                                           THOIGIANBATDAU_PHIENHOP = a.THOIGIANBATDAU_PHIENHOP,
        //                                           CHUTRI_ID = a.CHUTRI_ID,
        //                                           CHUTRI_TEN = a.CHUTRI_TEN,
        //                                           PHONGHOP_ID = a.PHONGHOP_ID,
        //                                           TENPHONGHOP = a.TENPHONGHOP,
        //                                           THOIGIAN_GUI = a.THOIGIAN_GUI
        //                                       }).ToList();
        //    return objThongBaos;
        //}

        //public bool SetEnableThongBao(int pUserId, int pStatus)
        //{
        //    try
        //    {
        //        //Tìm thông tin người dùng
        //        NGUOIDUNG objNguoiDung = vDataContext.NGUOIDUNGs.SingleOrDefault(a => a.NGUOIDUNG_ID == pUserId);

        //        List<Devices_App> objDeviceApp = vDataContext.Devices_Apps.Where(a => a.UserId == objNguoiDung.UserId).ToList();

        //        foreach (var it in objDeviceApp)
        //        {
        //            it.EnableThongBao = pStatus == 1 ? true : false;
        //        }

        //        List<NGUOIDUNG_DEVICE> objNguoiDungDevice = vDataContext.NGUOIDUNG_DEVICEs.Where(a => a.NGUOIDUNG_ID == pUserId).ToList();

        //        foreach (var it in objNguoiDungDevice)
        //        {
        //            it.EnableThongBao = pStatus == 1 ? true : false;
        //        }

        //        vDataContext.SubmitChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public bool SetEnableAmTinNhan(int pUserId, int pStatus)
        //{
        //    try
        //    {
        //        //Tìm thông tin người dùng
        //        NGUOIDUNG objNguoiDung = vDataContext.NGUOIDUNGs.SingleOrDefault(a => a.NGUOIDUNG_ID == pUserId);

        //        List<Devices_App> objDeviceApp = vDataContext.Devices_Apps.Where(a => a.UserId == objNguoiDung.UserId).ToList();

        //        foreach (var it in objDeviceApp)
        //        {
        //            it.EnableAmTinNhan = pStatus == 1 ? true : false;
        //        }

        //        List<NGUOIDUNG_DEVICE> objNguoiDungDevice = vDataContext.NGUOIDUNG_DEVICEs.Where(a => a.NGUOIDUNG_ID == pUserId).ToList();

        //        foreach (var it in objNguoiDungDevice)
        //        {
        //            it.EnableAmTinNhan = pStatus == 1 ? true : false;
        //        }

        //        vDataContext.SubmitChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

    }
}