#region Thông tin chung
/// Mục đích        :Controller đối tượng
/// Ngày tại        :26/03/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DoiTuongController
/// </summary>
namespace KNTC
{
    
    public class DoiTuongController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion
        /// <summary>
        ///  Get đơn vị theo ID - backend
        /// </summary>
        /// <param name="pDoiTuongId"></param>
        /// <returns></returns>
        public DOITUONG GetDoiTuongTheoId(int pDoiTuongId)
        {
            try
            {
                DOITUONG vDoiTuongInfo = new DOITUONG();
                vDoiTuongInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == pDoiTuongId).FirstOrDefault();
                if (vDoiTuongInfo != null)
                {
                    return vDoiTuongInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Thêm mới đơn vị - backend
        /// </summary>
        /// <param name="pDonviInfo"></param>
        /// <param name="oDoiTuongId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiDoiTuong(DOITUONG pDonviInfo, out long oDoiTuongId, out string oErrorMessage)
        {
            try
            {
                vDataContext.DOITUONGs.InsertOnSubmit(pDonviInfo);
                vDataContext.SubmitChanges();

                oDoiTuongId = vDataContext.DOITUONGs.OrderByDescending(x => x.DOITUONG_ID).FirstOrDefault().DOITUONG_ID;
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oDoiTuongId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backend
        /// </summary>
        /// <param name="pThietBi_ID"></param>
        /// <param name="pThietBiInfo"></param>
        /// <param name="oErrorMessage"></param>
        //public void CapNhatDoiTuong(int pDoiTuongId, DOITUONG pDoiTuongInfo, out string oErrorMessage)
        //{
        //    try
        //    {
        //        DOITUONG vDoiTuongInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == pDoiTuongId).SingleOrDefault();
        //        if (vDoiTuongInfo != null)
        //        {
        //            vDoiTuongInfo.TENDOITUONG = pDoiTuongInfo.TENDOITUONG;
        //            vDoiTuongInfo.TENVIETTAT = pDoiTuongInfo.TENVIETTAT;
        //            vDoiTuongInfo.MOTA = pDoiTuongInfo.MOTA;
        //            vDoiTuongInfo.TRANGTHAI = pDoiTuongInfo.TRANGTHAI;
        //            vDataContext.SubmitChanges();
        //        }
        //        oErrorMessage = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //    }
        //}

        /// <summary>
        /// Xóa đơn vị - backend
        /// </summary>
        /// <param name="pDoiTuongId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaDoiTuong(int pDoiTuongId, out string oErrorMessage)
        {
            try
            {
                DOITUONG vDoiTuongInfo = vDataContext.DOITUONGs.Where(x => x.DOITUONG_ID == pDoiTuongId).SingleOrDefault();

                if (vDoiTuongInfo != null)
                {
                    vDataContext.DOITUONGs.DeleteOnSubmit(vDoiTuongInfo);
                    vDataContext.SubmitChanges();
                }
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Kiểm tra đơn vị đang sử dụng - backend
        /// </summary>
        /// <param name="pDoiTuongId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraDoiTuongDaPhatSinhDuLieu(int pDoiTuongId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountDoiTuong_PhongBan = vDataContext.PhongBans.Where(x => x.DOITUONG_ID == pDoiTuongId).Count();
        //        int vCountDoiTuong_PhienHop = vDataContext.PHIENHOPs.Where(x => x.DOITUONG_ID == pDoiTuongId).Count();
        //        int vCountDoiTuong_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.DOITUONG_ID == pDoiTuongId).Count();
        //        int vCountDoiTuong_KhachMoi = vDataContext.KHACHMOIs.Where(x => x.DOITUONG_ID == pDoiTuongId).Count();
        //        oErrorMessage = "";
        //        if (vCountDoiTuong_PhongBan > 0 || vCountDoiTuong_PhienHop > 0 || vCountDoiTuong_NguoiDung > 0 || vCountDoiTuong_KhachMoi > 0)
        //            return true;//Đã được sử dụng
        //        else
        //            return false;//Chưa được sử dụng
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //        return true;
        //    }

        //}

        /// <summary>
        /// Kiểm tra trùng tên đơn vị - backend
        /// </summary>
        /// <param name="pThietBi_ID"></param>
        /// <param name="pTenThietBi"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraTrungTenDoiTuong(int pDoiTuongId, string pTenDoiTuong, out string oErrorMessage)
        //{
        //    try
        //    {
        //        bool vResult = false;
        //        var vDoiTuongInfos = (from vDoiTuongInfo in vDataContext.DOITUONGs
        //                           where vDoiTuongInfo.TENDOITUONG == pTenDoiTuong && vDoiTuongInfo.DOITUONG_ID != pDoiTuongId
        //                           select vDoiTuongInfo).ToList();
        //        if (vDoiTuongInfos.Count() > 0)
        //            vResult = true; //trùng
        //        else
        //            vResult = false;//không trùng
        //        oErrorMessage = "";
        //        return vResult;

        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //        return true;
        //    }
        //}

        public DoiTuongController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
