using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HOPKHONGGIAY
{
    /// <summary>
    /// Summary description for TaiLieuController
    /// </summary>
    public class TaiLieuController
    {
        //#region Khai báo chung
        //HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        //#endregion

        //public TaiLieuController()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}

        //public int InsertTaiLieu(TAILIEU pTaiLieu)
        //{
        //    try
        //    {
        //        vDataContext.TAILIEUs.InsertOnSubmit(pTaiLieu);
        //        vDataContext.SubmitChanges();

        //        return pTaiLieu.TAILIEU_ID;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}

        //public bool RemoveTaiLieu(int pTaiLieuId, int pUserId)
        //{
        //    try
        //    {
        //        TAILIEU vTaiLieu = vDataContext.TAILIEUs.FirstOrDefault(tl => tl.TAILIEU_ID == pTaiLieuId);
        //        if (vTaiLieu != null)
        //        {
        //            if(pUserId == vTaiLieu.DAIBIEU_ID)
        //            {
        //                //Xóa góp ý
        //                List<GOPY> vLstGopY = (from a in vDataContext.GOPies
        //                                       where a.TAILIEU_ID == pTaiLieuId
        //                                       select a).ToList();
        //                vDataContext.GOPies.DeleteAllOnSubmit(vLstGopY);
        //                vDataContext.SubmitChanges();

        //                //Xóa ghi chú
        //                List<GHICHU> vLstGhiChu = (from a in vDataContext.GHICHUs
        //                                           where a.TAILIEU_ID == pTaiLieuId
        //                                           select a).ToList();
        //                vDataContext.GHICHUs.DeleteAllOnSubmit(vLstGhiChu);
        //                vDataContext.SubmitChanges();

        //                vDataContext.TAILIEUs.DeleteOnSubmit(vTaiLieu);
        //                vDataContext.SubmitChanges();
        //            }
        //            else
        //            {
        //                ////Xóa góp ý
        //                //List<GOPY> vLstGopY = (from a in vDataContext.GOPies
        //                //                       where a.TAILIEU_ID == pTaiLieuId && a.NGUOIDUNG_ID == pUserId
        //                //                       select a).ToList();
        //                //vDataContext.GOPies.DeleteAllOnSubmit(vLstGopY);
        //                //vDataContext.SubmitChanges();

        //                ////Xóa ghi chú
        //                //List<GHICHU> vLstGhiChu = (from a in vDataContext.GHICHUs
        //                //                           where a.TAILIEU_ID == pTaiLieuId && a.NGUOIDUNG_ID == pUserId
        //                //                           select a).ToList();
        //                //vDataContext.GHICHUs.DeleteAllOnSubmit(vLstGhiChu);
        //                //vDataContext.SubmitChanges();

        //                //Tìm tài liệu cá nhân
        //                TAILIEU vTaiLieuCaNhan = vDataContext.TAILIEUs.FirstOrDefault(tl => tl.TAILIEU_ID_CHA == vTaiLieu.TAILIEU_ID && tl.DAIBIEU_ID == pUserId);

        //                //Xóa góp ý
        //                List<GOPY> vLstGopYCaNhan = (from a in vDataContext.GOPies
        //                                       where a.TAILIEU_ID == vTaiLieuCaNhan.TAILIEU_ID
        //                                       select a).ToList();
        //                vDataContext.GOPies.DeleteAllOnSubmit(vLstGopYCaNhan);
        //                vDataContext.SubmitChanges();

        //                //Xóa ghi chú
        //                List<GHICHU> vLstGhiChuCaNhan = (from a in vDataContext.GHICHUs
        //                                           where a.TAILIEU_ID == vTaiLieuCaNhan.TAILIEU_ID
        //                                           select a).ToList();
        //                vDataContext.GHICHUs.DeleteAllOnSubmit(vLstGhiChuCaNhan);
        //                vDataContext.SubmitChanges();

        //                vDataContext.TAILIEUs.DeleteOnSubmit(vTaiLieuCaNhan);
        //                vDataContext.SubmitChanges();
        //            }
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public List<TaiLieuInfo> GetTaiLieuCaNhan(int pPHIENHOPID, int pLOAI, int pUser_id, string pDomain)
        //{
        //    //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân
        //    //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //    List<TaiLieuInfo> vLstTaiLieu = (from a in vDataContext.TAILIEUs
        //                                     where (pPHIENHOPID == -1 ? true : a.PHIENHOP_ID == pPHIENHOPID)
        //                                     && (pLOAI == -1 ? true : a.LOAITAILIEU == pLOAI)
        //                                     && a.TRANGTHAI == 1
        //                                     && (pUser_id == -1 ? true : a.DAIBIEU_ID == pUser_id)
        //                                     select new TaiLieuInfo()
        //                                     {
        //                                         TAILIEU_ID = a.TAILIEU_ID,
        //                                         TEN = a.TEN,
        //                                         MOTA = a.MOTA,
        //                                         TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                         TRANGTHAI = a.TRANGTHAI,
        //                                         PHIENHOP_ID = a.PHIENHOP_ID,
        //                                         DAIBIEU_ID = a.DAIBIEU_ID,
        //                                         DOMAT = a.DOMAT,
        //                                         LOAITAILIEU = a.LOAITAILIEU,
        //                                         NGAYTAO = a.NGAYTAO,
        //                                         NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                         UserId = a.UserId,
        //                                         FILE_MOTA = a.FILE_MOTA,
        //                                         FILE_EXT = a.FILE_EXT,
        //                                         FILE_SIZE = a.FILE_SIZE,
        //                                         FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                         FILE_NAME = a.FILE_NAME,
        //                                         OBJECT_LOAI = a.OBJECT_LOAI,
        //                                         OBJECT_ID = a.OBJECT_ID,
        //                                         FILE_URL = pDomain + ClassParameter.vPathCommonUploadTaiLieuHop + "/" + a.FILE_NAME
        //                                     }).ToList();

        //    return vLstTaiLieu;
        //}

        //public List<NhomTaiLieuInfo> GetTaiLieuCaNhanGroupByNhom(int pPHIENHOPID, int pLOAI, int pUser_id, string pDomain)
        //{
        //    //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân
        //    //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //    List<TaiLieuInfo> vLstTaiLieu = (from a in vDataContext.TAILIEUs
        //                                     where (pPHIENHOPID == -1 ? true : a.PHIENHOP_ID == pPHIENHOPID)
        //                                     && (pLOAI == -1 ? true : a.LOAITAILIEU == pLOAI)
        //                                     && a.TRANGTHAI == 1
        //                                     && (pUser_id == -1 ? true : a.DAIBIEU_ID == pUser_id)
        //                                     select new TaiLieuInfo()
        //                                     {
        //                                         TAILIEU_ID = a.TAILIEU_ID,
        //                                         TEN = a.TEN,
        //                                         MOTA = a.MOTA,
        //                                         TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                         TRANGTHAI = a.TRANGTHAI,
        //                                         PHIENHOP_ID = a.PHIENHOP_ID,
        //                                         DAIBIEU_ID = a.DAIBIEU_ID,
        //                                         DOMAT = a.DOMAT,
        //                                         LOAITAILIEU = a.LOAITAILIEU,
        //                                         NGAYTAO = a.NGAYTAO,
        //                                         NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                         UserId = a.UserId,
        //                                         FILE_MOTA = a.FILE_MOTA,
        //                                         FILE_EXT = a.FILE_EXT,
        //                                         FILE_SIZE = a.FILE_SIZE,
        //                                         FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                         FILE_NAME = a.FILE_NAME,
        //                                         OBJECT_LOAI = a.OBJECT_LOAI,
        //                                         OBJECT_ID = a.OBJECT_ID,
        //                                         FILE_URL = pDomain + ClassParameter.vPathCommonUploadTaiLieuHop + "/" + a.FILE_NAME,
        //                                         TL_NHOM = a.TL_NHOM
        //                                     }).ToList();

        //    var objNhoms = vLstTaiLieu.Select(a => a.TL_NHOM).Distinct().ToList();

        //    List<NhomTaiLieuInfo> vLstNhomTaiLieus = new List<NhomTaiLieuInfo>();
        //    foreach (var it in objNhoms)
        //    {
        //        NhomTaiLieuInfo obj = new NhomTaiLieuInfo();
        //        if (it == null || it == "")
        //        {
        //            obj.NHOM_TAILIEU = "Tài liệu khác";
        //        }
        //        else
        //        {
        //            obj.NHOM_TAILIEU = it;
        //        }

        //        obj.LstTaiLieuInfos = new List<TaiLieuInfo>();
        //        obj.LstTaiLieuInfos = vLstTaiLieu.Where(a => a.TL_NHOM == it).ToList();
        //        vLstNhomTaiLieus.Add(obj);
        //    }

        //    return vLstNhomTaiLieus;
        //}

        //public List<TaiLieuInfo> GetTaiLieuCuocHop(int pPHIENHOPID, int pUserId, string pDomain)
        //{
        //    //Tìm quyền của người dùng này
        //    PHIENHOP_NGUOIDUNG objNguoiDung = vDataContext.PHIENHOP_NGUOIDUNGs.FirstOrDefault(a => a.NGUOIDUNG_ID == pUserId && a.PHIENHOP_ID == pPHIENHOPID);

        //    if (objNguoiDung != null)
        //    {
        //        //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân
        //        //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //        List<TaiLieuInfo> vLstTaiLieu = (from a in vDataContext.TAILIEUs
        //                                         where (pPHIENHOPID == -1 ? true : a.PHIENHOP_ID == pPHIENHOPID)
        //                                         && (a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuHop || a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuNghienCuu)
        //                                         && a.TRANGTHAI == 1
        //                                         join phien in vDataContext.PHIENHOPs on a.PHIENHOP_ID equals phien.PHIENHOP_ID
        //                                         join b in vDataContext.QUYEN_TAILIEUs on a.TAILIEU_ID equals b.TAILIEU_ID
        //                                         where b.QUYEN_ID == objNguoiDung.LOAI_DAIBIEU
        //                                         select new TaiLieuInfo()
        //                                         {
        //                                             TAILIEU_ID = a.TAILIEU_ID,
        //                                             TEN = a.TEN,
        //                                             MOTA = a.MOTA,
        //                                             TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                             TRANGTHAI = a.TRANGTHAI,
        //                                             PHIENHOP_ID = a.PHIENHOP_ID,
        //                                             DAIBIEU_ID = a.DAIBIEU_ID,
        //                                             DOMAT = a.DOMAT,
        //                                             LOAITAILIEU = a.LOAITAILIEU,
        //                                             NGAYTAO = a.NGAYTAO,
        //                                             NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                             UserId = a.UserId,
        //                                             FILE_MOTA = a.FILE_MOTA,
        //                                             FILE_EXT = a.FILE_EXT,
        //                                             FILE_SIZE = a.FILE_SIZE,
        //                                             FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                             FILE_NAME = a.FILE_NAME,
        //                                             OBJECT_LOAI = a.OBJECT_LOAI,
        //                                             OBJECT_ID = a.OBJECT_ID,
        //                                             FILE_URL = pDomain + ClassParameter.vPathCommonUploadTaiLieuHop + "/" + a.FILE_NAME,
        //                                             TrangThaiPhienHop = phien.TRANGTHAI,
        //                                             TL_NHOM = a.TL_NHOM
        //                                         }).ToList();

        //        //Kiểm tra tài liệu này người dùng đã save hay chưa
        //        foreach (var it in vLstTaiLieu)
        //        {
        //            int count = (from a in vDataContext.TAILIEUs
        //                         where a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuLuu
        //                         && a.TEN == it.TEN && a.PHIENHOP_ID == it.PHIENHOP_ID
        //                         && a.DAIBIEU_ID == pUserId
        //                         select a).Count();
        //            if (count == 0)
        //            {
        //                it.DaLuu = 0;
        //            }
        //            else
        //            {
        //                it.DaLuu = 1;
        //            }
        //        }

        //        //string json = new JavaScriptSerializer().Serialize(vLstTaiLieu);
        //        //json = json.Replace("null", "\"\"");
        //        //return json;

        //        return vLstTaiLieu;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public List<NhomTaiLieuInfo> GetTaiLieuCuocHopGroupByNhom(int pPHIENHOPID, int pUserId, string pDomain)
        //{
        //    //Tìm quyền của người dùng này
        //    PHIENHOP_NGUOIDUNG objNguoiDung = vDataContext.PHIENHOP_NGUOIDUNGs.FirstOrDefault(a => a.NGUOIDUNG_ID == pUserId && a.PHIENHOP_ID == pPHIENHOPID);

        //    if (objNguoiDung != null)
        //    {
        //        //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân
        //        //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //        List<TaiLieuInfo> vLstTaiLieu = (from a in vDataContext.TAILIEUs
        //                                         where (pPHIENHOPID == -1 ? true : a.PHIENHOP_ID == pPHIENHOPID)
        //                                         && (a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuHop || a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuNghienCuu)
        //                                         && a.TRANGTHAI == 1
        //                                         join phien in vDataContext.PHIENHOPs on a.PHIENHOP_ID equals phien.PHIENHOP_ID
        //                                         join b in vDataContext.QUYEN_TAILIEUs on a.TAILIEU_ID equals b.TAILIEU_ID
        //                                         where b.QUYEN_ID == objNguoiDung.LOAI_DAIBIEU
        //                                         select new TaiLieuInfo()
        //                                         {
        //                                             TAILIEU_ID = a.TAILIEU_ID,
        //                                             TEN = a.TEN,
        //                                             MOTA = a.MOTA,
        //                                             TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                             TRANGTHAI = a.TRANGTHAI,
        //                                             PHIENHOP_ID = a.PHIENHOP_ID,
        //                                             DAIBIEU_ID = a.DAIBIEU_ID,
        //                                             DOMAT = a.DOMAT,
        //                                             LOAITAILIEU = a.LOAITAILIEU,
        //                                             NGAYTAO = a.NGAYTAO,
        //                                             NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                             UserId = a.UserId,
        //                                             FILE_MOTA = a.FILE_MOTA,
        //                                             FILE_EXT = a.FILE_EXT,
        //                                             FILE_SIZE = a.FILE_SIZE,
        //                                             FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                             FILE_NAME = a.FILE_NAME,
        //                                             OBJECT_LOAI = a.OBJECT_LOAI,
        //                                             OBJECT_ID = a.OBJECT_ID,
        //                                             FILE_URL = pDomain + ClassParameter.vPathCommonUploadTaiLieuHop + "/" + a.FILE_NAME,
        //                                             TrangThaiPhienHop = phien.TRANGTHAI,
        //                                             TL_NHOM = a.TL_NHOM
        //                                         }).ToList();

        //        //Kiểm tra tài liệu này người dùng đã save hay chưa
        //        foreach (var it in vLstTaiLieu)
        //        {
        //            int count = (from a in vDataContext.TAILIEUs
        //                         where a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuLuu
        //                         && a.TEN == it.TEN && a.PHIENHOP_ID == it.PHIENHOP_ID
        //                         && a.DAIBIEU_ID == pUserId
        //                         select a).Count();
        //            if (count == 0)
        //            {
        //                it.DaLuu = 0;
        //            }
        //            else
        //            {
        //                it.DaLuu = 1;
        //            }
        //        }

        //        var objNhoms = vLstTaiLieu.Select(a => a.TL_NHOM).Distinct().ToList();

        //        List<NhomTaiLieuInfo> vLstNhomTaiLieus = new List<NhomTaiLieuInfo>();
        //        foreach (var it in objNhoms)
        //        {
        //            NhomTaiLieuInfo obj = new NhomTaiLieuInfo();
        //            if (it == null || it == "")
        //            {
        //                obj.NHOM_TAILIEU = "Tài liệu khác";
        //            }
        //            else
        //            {
        //                obj.NHOM_TAILIEU = it;
        //            }

        //            obj.LstTaiLieuInfos = new List<TaiLieuInfo>();
        //            obj.LstTaiLieuInfos = vLstTaiLieu.Where(a => a.TL_NHOM == it).ToList();
        //            vLstNhomTaiLieus.Add(obj);
        //        }

        //        return vLstNhomTaiLieus;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public List<TaiLieuInfo> GetTaiLieuKetLuanCuocHop(int pPHIENHOPID, string pDomain)
        //{
        //    //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân
        //    //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //    List<TaiLieuInfo> vLstTaiLieu = (from a in vDataContext.TAILIEUs
        //                                     where (pPHIENHOPID == -1 ? true : a.PHIENHOP_ID == pPHIENHOPID)
        //                                     && (a.LOAITAILIEU == (int)CommonEnum.TapTinObjectLoai.TaiLieuKetLuan)
        //                                     && a.TRANGTHAI == 1
        //                                     select new TaiLieuInfo()
        //                                     {
        //                                         TAILIEU_ID = a.TAILIEU_ID,
        //                                         TEN = a.TEN,
        //                                         MOTA = a.MOTA,
        //                                         TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                         TRANGTHAI = a.TRANGTHAI,
        //                                         PHIENHOP_ID = a.PHIENHOP_ID,
        //                                         DAIBIEU_ID = a.DAIBIEU_ID,
        //                                         DOMAT = a.DOMAT,
        //                                         LOAITAILIEU = a.LOAITAILIEU,
        //                                         NGAYTAO = a.NGAYTAO,
        //                                         NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                         UserId = a.UserId,
        //                                         FILE_MOTA = a.FILE_MOTA,
        //                                         FILE_EXT = a.FILE_EXT,
        //                                         FILE_SIZE = a.FILE_SIZE,
        //                                         FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                         FILE_NAME = a.FILE_NAME,
        //                                         OBJECT_LOAI = a.OBJECT_LOAI,
        //                                         OBJECT_ID = a.OBJECT_ID,
        //                                         FILE_URL = pDomain + ClassParameter.vPathCommonUploadKetLuan + "/" + a.FILE_NAME,
        //                                         TL_NHOM = a.TL_NHOM
        //                                     }).ToList();

        //    //string json = new JavaScriptSerializer().Serialize(vLstTaiLieu);
        //    //json = json.Replace("null", "\"\"");
        //    //return json;

        //    return vLstTaiLieu;
        //}

        //public List<TaiLieuInfo> GetChiTietTaiLieu(int pTaiLieuId, int pUserId, string pDomain)
        //{
        //    try
        //    {
        //        //Tìm quyền của người dùng này
        //        int count = (from tailieu in vDataContext.TAILIEUs
        //                     where tailieu.TAILIEU_ID == pTaiLieuId
        //                     join tailieu_quyen in vDataContext.QUYEN_TAILIEUs on tailieu.TAILIEU_ID equals tailieu_quyen.TAILIEU_ID
        //                     join phienhop_nguoidung in vDataContext.PHIENHOP_NGUOIDUNGs on tailieu.PHIENHOP_ID equals phienhop_nguoidung.PHIENHOP_ID
        //                     where phienhop_nguoidung.NGUOIDUNG_ID == pUserId && phienhop_nguoidung.LOAI_DAIBIEU == tailieu_quyen.QUYEN_ID
        //                     select tailieu).Count();
        //        if (count > 0)
        //        {
        //            //Loại tài liệu 1: Tài liệu họp, 2: Tài liệu cá nhân, 3: Tài liệu copy ra
        //            //Trạng thái tài liệu 0: Chưa duyệt, 1: Đã duyệt
        //            List<TaiLieuInfo> vTaiLieu = (from a in vDataContext.TAILIEUs
        //                                          where (pTaiLieuId == -1 ? true : pTaiLieuId == a.TAILIEU_ID)
        //                                          && a.TRANGTHAI == 1
        //                                          select new TaiLieuInfo()
        //                                          {
        //                                              TAILIEU_ID = a.TAILIEU_ID,
        //                                              TEN = a.TEN,
        //                                              MOTA = a.MOTA,
        //                                              TAILIEUCHUNG = a.TAILIEUCHUNG,
        //                                              TRANGTHAI = a.TRANGTHAI,
        //                                              PHIENHOP_ID = a.PHIENHOP_ID,
        //                                              DAIBIEU_ID = a.DAIBIEU_ID,
        //                                              DOMAT = a.DOMAT,
        //                                              LOAITAILIEU = a.LOAITAILIEU,
        //                                              NGAYTAO = a.NGAYTAO,
        //                                              NGAYCAPNHAT = a.NGAYCAPNHAT,
        //                                              UserId = a.UserId,
        //                                              FILE_MOTA = a.FILE_MOTA,
        //                                              FILE_EXT = a.FILE_EXT,
        //                                              FILE_SIZE = a.FILE_SIZE,
        //                                              FILE_USERID_CAPNHAT = a.FILE_USERID_CAPNHAT,
        //                                              FILE_NAME = a.FILE_NAME,
        //                                              OBJECT_LOAI = a.OBJECT_LOAI,
        //                                              OBJECT_ID = a.OBJECT_ID,
        //                                              FILE_URL = pDomain + ClassParameter.vPathCommonUploadTaiLieuHop + "/" + a.FILE_NAME,
        //                                              TL_NHOM = a.TL_NHOM
        //                                          }).ToList();
        //            foreach (var it in vTaiLieu)
        //            {
        //                //Get thông tin góp ý
        //                it.vLstGopYInfos = new List<GopYInfo>();
        //                it.vLstGopYInfos = (from a in vDataContext.GOPies
        //                                    where a.TAILIEU_ID == pTaiLieuId
        //                                    && (pUserId == -1 ? true : pUserId == a.NGUOIDUNG_ID)
        //                                    select new GopYInfo()
        //                                    {
        //                                        GOPY_ID = a.GOPY_ID,
        //                                        NGUOIDUNG_ID = a.NGUOIDUNG_ID,
        //                                        NGAYGOPY = a.NGAYGOPY,
        //                                        TAILIEU_ID = a.TAILIEU_ID,
        //                                        GOPY_NOIDUNG = a.GOPY_NOIDUNG,
        //                                    }).ToList();

        //                //Get thông tin ghi chú
        //                it.vLstGhiChuInfos = new List<GhiChuInfo>();
        //                it.vLstGhiChuInfos = (from a in vDataContext.GHICHUs
        //                                      where a.TAILIEU_ID == pTaiLieuId
        //                                      && (pUserId == -1 ? true : pUserId == a.NGUOIDUNG_ID)
        //                                      select new GhiChuInfo()
        //                                      {
        //                                          GHICHU_ID = a.GHICHU_ID,
        //                                          NOIDUNG = a.NOIDUNG.Replace("\n", "<br>"),
        //                                          LOAIGHICHU = a.LOAIGHICHU,
        //                                          PHIENHOP_ID = a.PHIENHOP_ID,
        //                                          NGUOIDUNG_ID = a.NGUOIDUNG_ID,
        //                                          TAILIEU_ID = a.TAILIEU_ID,
        //                                          THOIGIANGHICHU = a.THOIGIANGHICHU,
        //                                          USER_ID = a.USER_ID,
        //                                          TRANGTHAI = a.TRANGTHAI
        //                                      }).ToList();
        //            }

        //            //string json = new JavaScriptSerializer().Serialize(vTaiLieu);
        //            //json = json.Replace("null", "\"\"");
        //            //return json;

        //            return vTaiLieu;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public int SaveTaiLieuHop(int pTaiLieuId, int pUserId, string pToken)
        //{
        //    DeviceAppController vDeviceAppController = new DeviceAppController();

        //    int vTaiLieuId = -1;
        //    if (vDeviceAppController.CheckTokenDevice(pToken))
        //    {
        //        bool check = false;
        //        try
        //        {
        //            //Lấy thông tin tài liệu
        //            TAILIEU vTaiLieu = vDataContext.TAILIEUs.FirstOrDefault(a => a.TAILIEU_ID == pTaiLieuId);

        //            if (vTaiLieu != null && pUserId != 0)
        //            {
        //                TAILIEU vTaiLieu_New = new TAILIEU();
        //                vTaiLieu_New.TEN = vTaiLieu.TEN;
        //                vTaiLieu_New.MOTA = vTaiLieu.MOTA;
        //                vTaiLieu_New.TAILIEUCHUNG = false;
        //                vTaiLieu_New.TRANGTHAI = 1;
        //                vTaiLieu_New.PHIENHOP_ID = vTaiLieu.PHIENHOP_ID;
        //                vTaiLieu_New.DAIBIEU_ID = pUserId;
        //                vTaiLieu_New.DOMAT = vTaiLieu.DOMAT;
        //                vTaiLieu_New.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuLuu;
        //                vTaiLieu_New.NGAYTAO = DateTime.Now;
        //                vTaiLieu_New.NGAYCAPNHAT = DateTime.Now;
        //                vTaiLieu_New.UserId = vTaiLieu.UserId;
        //                vTaiLieu_New.FILE_NAME = vTaiLieu.FILE_NAME;
        //                vTaiLieu_New.FILE_MOTA = vTaiLieu.FILE_MOTA;
        //                vTaiLieu_New.FILE_EXT = vTaiLieu.FILE_EXT;
        //                vTaiLieu_New.FILE_SIZE = vTaiLieu.FILE_SIZE;
        //                vTaiLieu_New.FILE_USERID_CAPNHAT = vTaiLieu.FILE_USERID_CAPNHAT;
        //                vTaiLieu_New.OBJECT_LOAI = vTaiLieu.OBJECT_LOAI;
        //                vTaiLieu_New.OBJECT_ID = vTaiLieu.OBJECT_ID;
        //                vTaiLieu_New.TAILIEU_ID_CHA = pTaiLieuId;
        //                vTaiLieu_New.TL_NHOM = vTaiLieu.TL_NHOM;

        //                vTaiLieuId = InsertTaiLieu(vTaiLieu_New);
        //            }
        //        }
        //        catch
        //        {
        //        }

        //    }

        //    return vTaiLieuId;
        //}

        //public int UploadTaiLieu(int pPhienHopId, int pUserId, string pData, string pFileName, string pPath)
        //{
        //    try
        //    {
        //        var vTemp = pData.Split(',');
        //        string vBase64String = vTemp[vTemp.Count() - 1];
        //        byte[] bytes = Convert.FromBase64String(vBase64String);
        //        string vPath = pPath + "/" + pFileName;
        //        File.WriteAllBytes(vPath, bytes);

        //        //Lấy thông tin phiên họp
        //        PHIENHOP objPhienHop = vDataContext.PHIENHOPs.FirstOrDefault(a => a.PHIENHOP_ID == pPhienHopId);

        //        TAILIEU vTaiLieu_New = new TAILIEU();
        //        vTaiLieu_New.TEN = objPhienHop.TIEUDE;
        //        vTaiLieu_New.MOTA = "";
        //        vTaiLieu_New.TAILIEUCHUNG = false;
        //        vTaiLieu_New.TRANGTHAI = 1;
        //        vTaiLieu_New.PHIENHOP_ID = pPhienHopId;
        //        vTaiLieu_New.DAIBIEU_ID = pUserId;
        //        vTaiLieu_New.DOMAT = false;
        //        vTaiLieu_New.LOAITAILIEU = (int)CommonEnum.TapTinObjectLoai.TaiLieuCaNhan;
        //        vTaiLieu_New.NGAYTAO = DateTime.Now;
        //        vTaiLieu_New.NGAYCAPNHAT = DateTime.Now;
        //        vTaiLieu_New.UserId = pUserId;
        //        vTaiLieu_New.FILE_NAME = pFileName;
        //        vTaiLieu_New.FILE_MOTA = "";
        //        vTaiLieu_New.FILE_EXT = pFileName.Split('.').LastOrDefault();
        //        vTaiLieu_New.FILE_SIZE = bytes.Length;
        //        vTaiLieu_New.FILE_USERID_CAPNHAT = pUserId;
        //        vTaiLieu_New.OBJECT_LOAI = 1;
        //        vTaiLieu_New.OBJECT_ID = pPhienHopId;

        //        return InsertTaiLieu(vTaiLieu_New);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //}


    }
}