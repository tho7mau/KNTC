#region Thông tin chung
/// Mục đích        :Controller Chức vụ (back-end)
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
/// Summary description for ChucVuController
/// </summary>
namespace KNTC
{
    
    public class ChucVuController
    {
      //  #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        //  #endregion



        /// <summary>
        /// Get chức vụ theo ID
        /// </summary>
        /// <param name="pChucVuId"></param>
        /// <returns></returns>
        public CHUCVU GetChucVuTheoId(int pChucVuId)
        {
            try
            {
                CHUCVU vChucVuInfo = new CHUCVU();
                vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).FirstOrDefault();
                if (vChucVuInfo != null)
                {
                    return vChucVuInfo;
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
        /// Thêm mới chức vụ
        /// </summary>
        /// <param name="pChucVuInfo"></param>
        /// <param name="oChucVuId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiChucVu(CHUCVU pChucVuInfo, out int oChucVuId, out string oErrorMessage)
        {
            try
            {
                vDataContext.CHUCVUs.InsertOnSubmit(pChucVuInfo);
                vDataContext.SubmitChanges();

                oChucVuId = vDataContext.CHUCVUs.OrderByDescending(x => x.CV_ID).FirstOrDefault().CV_ID;
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oChucVuId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin chúc vụ
        /// </summary>
        /// <param name="pChucVuId"></param>
        /// <param name="pChucVuInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatChucVu(int pChucVuId, CHUCVU pChucVuInfo, out string oErrorMessage)
        {
            try
            {
                var vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).SingleOrDefault();
                if (vChucVuInfo != null)
                {
                    vChucVuInfo.TENCHUCVU = pChucVuInfo.TENCHUCVU;
                    vChucVuInfo.MOTA = pChucVuInfo.MOTA;
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
        /// Xóa chức vụ
        /// </summary>
        /// <param name="pChucVuId"></param>
        /// <param name="oErrorMessage"></param>
        public void XoaChucVu(int pChucVuId, out string oErrorMessage)
        {
            try
            {
                CHUCVU vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).SingleOrDefault();


                if (vChucVuInfo != null)
                {
                    vDataContext.CHUCVUs.DeleteOnSubmit(vChucVuInfo);
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
        ///  Kiểm tra trùng tên chức vụ
        /// </summary>
        /// <param name="pChucVuId"></param>
        /// <param name="pTenChucVu"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        public bool KiemTraTrungTenChucVu(int pChucVuId, string pTenChucVu, out string oErrorMessage)
        {
            try
            {
                bool vResult = false;
                var vChucVuInfos = (from vChucVuInfo in vDataContext.CHUCVUs
                                    where vChucVuInfo.TENCHUCVU == pTenChucVu && vChucVuInfo.CV_ID != pChucVuId
                                    select vChucVuInfo).ToList();
                if (vChucVuInfos.Count() > 0)
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
        public ChucVuController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
