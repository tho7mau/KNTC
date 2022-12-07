#region Thông tin chung
/// Mục đích        :Controller phòng ban (back-end)
/// Ngày tại        :08/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for ChucVuController
/// </summary>
namespace KNTC
{
    
    public class PhongBanController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion
        
        /// <summary>
        /// Get phòng ban theo ID - backend
        /// </summary>
        /// <param name="pPhongBanId"></param>
        /// <returns></returns>
        public PhongBan GetPhongBanTheoId(int pPhongBanId)
        {
            try
            {
                PhongBan vPhongBanInfo = new PhongBan();
                vPhongBanInfo = vDataContext.PhongBans.Where(x => x.PB_ID == pPhongBanId).FirstOrDefault();
                if (vPhongBanInfo != null)
                {
                    return vPhongBanInfo;
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
        /// Thêm mới phòng ban - backend
        /// </summary>
        /// <param name="pPhongBanInfo"></param>
        /// <param name="oPhongBanId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiPhongBan(PhongBan pPhongBanInfo, out int oPhongBanId,  out string oErrorMessage)
        {
            try
            {             
                vDataContext.PhongBans.InsertOnSubmit(pPhongBanInfo);
                vDataContext.SubmitChanges();

                oPhongBanId = vDataContext.PhongBans.OrderByDescending(x => x.PB_ID).FirstOrDefault().PB_ID;
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oPhongBanId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backed
        /// </summary>
        /// <param name="pPhongBanId"></param>
        /// <param name="pPhongBanInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatPhongBan(int pPhongBanId, PhongBan pPhongBanInfo, out string oErrorMessage)
        {
            try
            {
                var vPhongBanInfo = vDataContext.PhongBans.Where(x => x.PB_ID == pPhongBanId).SingleOrDefault();
                if(vPhongBanInfo != null)
                {
                    vPhongBanInfo.TENPHONGBAN = pPhongBanInfo.TENPHONGBAN;
                    vPhongBanInfo.DONVI_ID = pPhongBanInfo.DONVI_ID;
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
        /// Xóa phòng ban - backend
        /// </summary>
        /// <param name="pPhongBanId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaPhongBan(int pPhongBanId, out string oErrorMessage)
        {
            try
            {
                PhongBan vPhongBanInfo = vDataContext.PhongBans.Where(x => x.PB_ID == pPhongBanId).SingleOrDefault();
              
                if(vPhongBanInfo != null)
                {
                    vDataContext.PhongBans.DeleteOnSubmit(vPhongBanInfo);
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
        /// Kiểm tra phòng ban đang được sử dụng - backend
        /// </summary>
        /// <param name="pPhongBanId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraphongBanDangDuocSuDung(int pPhongBanId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountPhongBan_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.PB_ID == pPhongBanId).Count();
        //        oErrorMessage = "";
        //        if (vCountPhongBan_NguoiDung > 0 || vCountPhongBan_KhachMoi > 0)
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
        ///  Kiểm tra trùng tên phòng ban - backend
        /// </summary>
        /// <param name="pPhongBanId"></param>
        /// <param name="pTenPhongBan"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        public bool KiemTraTrungTenPhongBan(int pPhongBanId, int pDonViId, string pTenPhongBan, out string oErrorMessage)
        {
            try
            {
                bool vResult = false;
                var vPhongBanInfos = (from vPhongBanInfo in vDataContext.PhongBans
                          where vPhongBanInfo.TENPHONGBAN == pTenPhongBan && vPhongBanInfo.PB_ID != pPhongBanId && pDonViId == vPhongBanInfo.DONVI_ID
                                      select vPhongBanInfo).ToList();
                if (vPhongBanInfos.Count() > 0)
                    vResult = true; //trùng
                else
                    vResult =  false;//không trùng
                oErrorMessage = "";
                return vResult;              
            }
            catch (Exception ex)
            {
                oErrorMessage = ex.Message;
                return true;                
            }
        }
        public PhongBanController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
