#region Thông tin chung
/// Mục đích        :Controller đơn vị (back-end)
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
/// Summary description for DonViController
/// </summary>
namespace KNTC
{
    
    public class DonViController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion
        /// <summary>
        ///  Get đơn vị theo ID - backend
        /// </summary>
        /// <param name="pDonViId"></param>
        /// <returns></returns>
        public DONVI GetDonViTheoID(int pDonViId)
        {
            try
            {
                DONVI vDonViInfo = new DONVI();
                vDonViInfo = vDataContext.DONVIs.Where(x => x.DONVI_ID == pDonViId).FirstOrDefault();
                if (vDonViInfo != null)
                {
                    return vDonViInfo;
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
        /// <param name="oDonViId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiDonVi(DONVI pDonviInfo, out int oDonViId, out string oErrorMessage)
        {
            try
            {
                vDataContext.DONVIs.InsertOnSubmit(pDonviInfo);
                vDataContext.SubmitChanges();

                oDonViId = vDataContext.DONVIs.OrderByDescending(x => x.DONVI_ID).FirstOrDefault().DONVI_ID;
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oDonViId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backend
        /// </summary>
        /// <param name="pThietBi_ID"></param>
        /// <param name="pThietBiInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatDonVi(int pDonViId, DONVI pDonViInfo, out string oErrorMessage)
        {
            try
            {
                DONVI vDonViInfo = vDataContext.DONVIs.Where(x => x.DONVI_ID == pDonViId).SingleOrDefault();
                if (vDonViInfo != null)
                {
                    vDonViInfo.TENDONVI = pDonViInfo.TENDONVI;
                    vDonViInfo.TENVIETTAT = pDonViInfo.TENVIETTAT;
                    vDonViInfo.MOTA = pDonViInfo.MOTA;
                    vDonViInfo.TRANGTHAI = pDonViInfo.TRANGTHAI;
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
        /// Xóa đơn vị - backend
        /// </summary>
        /// <param name="pDonViId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaDonVi(int pDonViId, out string oErrorMessage)
        {
            try
            {
                DONVI vDonViInfo = vDataContext.DONVIs.Where(x => x.DONVI_ID == pDonViId).SingleOrDefault();

                if (vDonViInfo != null)
                {
                    vDataContext.DONVIs.DeleteOnSubmit(vDonViInfo);
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
        /// <param name="pDonViId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraDonViDaPhatSinhDuLieu(int pDonViId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountDonVi_PhongBan = vDataContext.PhongBans.Where(x => x.DONVI_ID == pDonViId).Count();
        //        int vCountDonVi_PhienHop = vDataContext.PHIENHOPs.Where(x => x.DONVI_ID == pDonViId).Count();
        //        int vCountDonVi_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.DONVI_ID == pDonViId).Count();
        //        int vCountDonVi_KhachMoi = vDataContext.KHACHMOIs.Where(x => x.DONVI_ID == pDonViId).Count();
        //        oErrorMessage = "";
        //        if (vCountDonVi_PhongBan > 0 || vCountDonVi_PhienHop > 0 || vCountDonVi_NguoiDung > 0 || vCountDonVi_KhachMoi > 0)
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
        public bool KiemTraTrungTenDonVi(int pDonViId, string pTenDonVi, out string oErrorMessage)
        {
            try
            {
                bool vResult = false;
                var vDonViInfos = (from vDonViInfo in vDataContext.DONVIs
                                   where vDonViInfo.TENDONVI == pTenDonVi && vDonViInfo.DONVI_ID != pDonViId
                                   select vDonViInfo).ToList();
                if (vDonViInfos.Count() > 0)
                    vResult = true; //trùng
                else
                    vResult = false;//không trùng
                oErrorMessage = "";
                return vResult;

            }
            catch (Exception ex)
            {
                oErrorMessage = ex.Message;
                return true;
            }
        }

        public DonViController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
