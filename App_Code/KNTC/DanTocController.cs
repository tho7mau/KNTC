#region Thông tin chung
/// Mục đích        :Controller dân tộc
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
    
    public class DanTocController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion
        
        /// <summary>
        /// Get phòng ban theo ID - backend
        /// </summary>
        /// <param name="pDanTocId"></param>
        /// <returns></returns>
        public DANTOC GetDanTocTheoId(int pDanTocId)
        {
            try
            {
                DANTOC vDanTocInfo = new DANTOC();
                vDanTocInfo = vDataContext.DANTOCs.Where(x => x.DANTOC_ID == pDanTocId).FirstOrDefault();
                if (vDanTocInfo != null)
                {
                    return vDanTocInfo;
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
        /// <param name="pDanTocInfo"></param>
        /// <param name="oDanTocId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiDanToc(DANTOC pDanTocInfo, out int oDanTocId,  out string oErrorMessage)
        {
            try
            {             
                vDataContext.DANTOCs.InsertOnSubmit(pDanTocInfo);
                vDataContext.SubmitChanges();

                oDanTocId = vDataContext.DANTOCs.OrderByDescending(x => x.DANTOC_ID).FirstOrDefault().DANTOC_ID;
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oDanTocId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backed
        /// </summary>
        /// <param name="pDanTocId"></param>
        /// <param name="pDanTocInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatDanToc(int pDanTocId, DANTOC pDanTocInfo, out string oErrorMessage)
        {
            try
            {
                var vDanTocInfo = vDataContext.DANTOCs.Where(x => x.DANTOC_ID == pDanTocId).SingleOrDefault();
                if(vDanTocInfo != null)
                {
                    vDanTocInfo.DANTOC_TEN = pDanTocInfo.DANTOC_TEN;
                    vDanTocInfo.DANTOC_MOTA = pDanTocInfo.DANTOC_MOTA;
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
        /// <param name="pDanTocId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaDanToc(int pDanTocId, out string oErrorMessage)
        {
            try
            {
                DANTOC vDanTocInfo = vDataContext.DANTOCs.Where(x => x.DANTOC_ID == pDanTocId).SingleOrDefault();
              
                if(vDanTocInfo != null)
                {
                    vDataContext.DANTOCs.DeleteOnSubmit(vDanTocInfo);
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
        /// <param name="pDanTocId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraphongBanDangDuocSuDung(int pDanTocId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountDanToc_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.DANTOC_ID == pDanTocId).Count();
        //        oErrorMessage = "";
        //        if (vCountDanToc_NguoiDung > 0 || vCountDanToc_KhachMoi > 0)
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
        /// <param name="pDanTocId"></param>
        /// <param name="pTenDanToc"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        public bool KiemTraTrungTenDanToc(int pDanTocId,  string pTenDanToc, out string oErrorMessage)
        {
            try
            {
                bool vResult = false;
                var vDanTocInfos = (from vDanTocInfo in vDataContext.DANTOCs
                          where vDanTocInfo.DANTOC_TEN == pTenDanToc && vDanTocInfo.DANTOC_ID != pDanTocId
                                      select vDanTocInfo).ToList();
                if (vDanTocInfos.Count() > 0)
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
        public DanTocController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
