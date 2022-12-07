#region Thông tin chung
/// Mục đích        :Controller hồ sơ (back-end)
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
    
    public class HoSoController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion
        
        /// <summary>
        /// Get hồ sơ theo ID - backend
        /// </summary>
        /// <param name="pHoSoId"></param>
        /// <returns></returns>
        public HOSO GetHoSoTheoId(int pHoSoId)
        {
            try
            {
                HOSO vHoSoInfo = new HOSO();
                vHoSoInfo = vDataContext.HOSOs.Where(x => x.HOSO_ID == pHoSoId).FirstOrDefault();
                if (vHoSoInfo != null)
                {
                    return vHoSoInfo;
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
        /// Get danh sách hồ sơ theo objectId
        /// </summary>
        /// <param name="pHoSoId"></param>
        /// <returns></returns>
        public List<HOSO> GetDanhSachHoSoTheoObjectId(int pHoSoId)
        {
            try
            {
                var vHoSoInfos = vDataContext.HOSOs.Where(x => x.HOSO_ID == pHoSoId).ToList();
                return vHoSoInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Thêm mới hồ sơ - backend
        /// </summary>
        /// <param name="pHoSoInfo"></param>
        /// <param name="oHoSoId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiHoSo(HOSO pHoSoInfo, out long oHoSoId,  out string oErrorMessage)
        {
            try
            {             
                vDataContext.HOSOs.InsertOnSubmit(pHoSoInfo);
                vDataContext.SubmitChanges();

                oHoSoId = vDataContext.HOSOs.OrderByDescending(x => x.HOSO_ID).FirstOrDefault().HOSO_ID;
                
                
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oHoSoId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backed
        /// </summary>
        /// <param name="pHoSoId"></param>
        /// <param name="pHoSoInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatHoSo(int pHoSoId, HOSO pHoSoInfo, out string oErrorMessage)
        {
            try
            {
                var vHoSoInfo = vDataContext.HOSOs.Where(x => x.HOSO_ID == pHoSoId).SingleOrDefault();
                if(vHoSoInfo != null)
                {
                    //vHoSoInfo.TENPHONGBAN = pHoSoInfo.TENPHONGBAN;
                    //vHoSoInfo.DONVI_ID = pHoSoInfo.DONVI_ID;
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
        /// Xóa hồ sơ - backend
        /// </summary>
        /// <param name="pHoSoId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaHoSo(int pHoSoId, out string oErrorMessage)
        {
            try
            {
                HOSO vHoSoInfo = vDataContext.HOSOs.Where(x => x.HOSO_ID == pHoSoId).SingleOrDefault();
              
                if(vHoSoInfo != null)
                {
                    vDataContext.HOSOs.DeleteOnSubmit(vHoSoInfo);
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
        /// Kiểm tra hồ sơ đang được sử dụng - backend
        /// </summary>
        /// <param name="pHoSoId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraphongBanDangDuocSuDung(int pHoSoId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountHoSo_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.PB_ID == pHoSoId).Count();
        //        oErrorMessage = "";
        //        if (vCountHoSo_NguoiDung > 0 || vCountHoSo_KhachMoi > 0)
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
       
        public HoSoController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
