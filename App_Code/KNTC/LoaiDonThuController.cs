#region Thông tin chung
/// Mục đích        :Controller Loại đơn thư (back-end)
/// Ngày tại        :25/03/2021
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
/// Summary description for LoaiDonThuController
/// </summary>
namespace KNTC
{

    public class LoaiDonThuController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion

        /// <summary>
        /// Get danh sách loại đơn thư
        /// </summary>
        /// <param name="pKeyWord"></param>
        /// <returns></returns>
        public List<LOAIDONTHU> GetDanhSachLoaiDonThu(string pKeyWord)
        {
            try
            {
                List<LOAIDONTHU> vLoaiDonThuInfos = new List<LOAIDONTHU>();
                vLoaiDonThuInfos = vDataContext.LOAIDONTHUs.Where(x =>
                SqlMethods.Like(x.LOAIDONTHU_TEN, "%" + pKeyWord + "%") ||
                SqlMethods.Like(x.LOAIDONTHU_CHA_TEN, "%" + pKeyWord + "%") ||
                SqlMethods.Like(x.LOAIDONTHU_MOTA, "%" + pKeyWord + "%") ||
                 SqlMethods.Like(x.LOAIDONTHU_MA, "%" + pKeyWord + "%")).OrderByDescending(x => x.LOAIDONTHU_ID).ToList();
                return vLoaiDonThuInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get loại đơn thư theo ID
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <returns></returns>
        public LOAIDONTHU GetLoaiDonThuTheoId(int pLoaiDonThuId)
        {
            try
            {
                LOAIDONTHU vLoaiDonThuInfo = new LOAIDONTHU();
                vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).FirstOrDefault();
                if (vLoaiDonThuInfo != null)
                {
                    return vLoaiDonThuInfo;
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
        /// Thêm mới loại đơn thư
        /// </summary>
        /// <param name="pLoaiDonThuInfos"></param>
        /// <param name="oLoaiDonThuId"></param>
        /// <param name="oErrorMessage"></param>
        public void ThemMoiLoaiDonThu(LOAIDONTHU pLoaiDonThuInfos, out int oLoaiDonThuId, out string oErrorMessage)
        {
            try
            {
                vDataContext.LOAIDONTHUs.InsertOnSubmit(pLoaiDonThuInfos);
                vDataContext.SubmitChanges();

                oLoaiDonThuId = vDataContext.LOAIDONTHUs.OrderByDescending(x => x.LOAIDONTHU_ID).FirstOrDefault().LOAIDONTHU_ID;
                oErrorMessage = "";
                var vLoaiDonThuNew = GetLoaiDonThuTheoId(oLoaiDonThuId);
                if (vLoaiDonThuNew.LOAIDONTHU_CHA_ID > 0) //Có loại cấp trên
                {
                    var vLoaiDonThuNew_CapTren = GetLoaiDonThuTheoId(vLoaiDonThuNew.LOAIDONTHU_CHA_ID ?? 0);
                    if (vLoaiDonThuNew_CapTren != null)
                    {
                        vLoaiDonThuNew.LOAIDONTHU_INDEX = vLoaiDonThuNew_CapTren.LOAIDONTHU_INDEX + vLoaiDonThuNew.LOAIDONTHU_ID + ".";
                    }
                }
                else
                {
                    vLoaiDonThuNew.LOAIDONTHU_INDEX = vLoaiDonThuNew.LOAIDONTHU_ID.ToString() + ".";
                }
                vDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                oLoaiDonThuId = -1;
                oErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// Cập nhật thông tin chúc vụ
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <param name="pLoaiDonThuInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatLoaiDonThu(int pLoaiDonThuId, LOAIDONTHU pLoaiDonThuInfo, out string oErrorMessage)
        {
            try
            {
                var vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).SingleOrDefault();
                if (vLoaiDonThuInfo != null)
                {
                    vLoaiDonThuInfo.LOAIDONTHU_MA = pLoaiDonThuInfo.LOAIDONTHU_MA;
                    vLoaiDonThuInfo.LOAIDONTHU_TEN = pLoaiDonThuInfo.LOAIDONTHU_TEN;
                    vLoaiDonThuInfo.LOAIDONTHU_MOTA = pLoaiDonThuInfo.LOAIDONTHU_MOTA;
                    vLoaiDonThuInfo.LOAIDONTHU_CAP = pLoaiDonThuInfo.LOAIDONTHU_CAP;
                    vLoaiDonThuInfo.LOAIDONTHU_CHA_ID = pLoaiDonThuInfo.LOAIDONTHU_CHA_ID;
                    vLoaiDonThuInfo.LOAIDONTHU_CHA_TEN = pLoaiDonThuInfo.LOAIDONTHU_CHA_TEN;
                    vLoaiDonThuInfo.LOAIDONTHU_INDEX = pLoaiDonThuInfo.LOAIDONTHU_INDEX;

                    vLoaiDonThuInfo.NGAYCAPNHAT = pLoaiDonThuInfo.NGAYCAPNHAT;
                    vLoaiDonThuInfo.NGUOICAPNHAT = pLoaiDonThuInfo.NGUOICAPNHAT;
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
        /// Xóa loại đơn thư
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <param name="oErrorMessage"></param>
        /// 

        public void XoaLoaiDonThu(int pLoaiDonThuId, out int oCountXoa ,out string oErrorMessage)
        {
            oCountXoa = 0;
            try
            {
                LOAIDONTHU vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).SingleOrDefault();


                if (vLoaiDonThuInfo != null)
                {
                    var objLoaiDonthu_Xoa = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_INDEX.StartsWith(vLoaiDonThuInfo.LOAIDONTHU_INDEX)).ToList();
                    if(objLoaiDonthu_Xoa != null)
                    {
                      var objLoaiDonthu_Xoa_Id = objLoaiDonthu_Xoa.Select(x => x.LOAIDONTHU_ID).ToList();
                        // Kiểm tra loại đơn thư có được sử dụng trong đơn thư
                        var objDonThu = vDataContext.DONTHUs.Where(x => objLoaiDonthu_Xoa_Id.Contains((int)x.LOAIDONTHU_ID)).ToList();
                        if(objDonThu.Count == 0) // Loại đơn thư chưa được sử dụng trong đơn thư
                        {
                            // Kiểm tra loại đơn thư có được sử dụng trong tiếp dân
                            var objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_LOAI != null && objLoaiDonthu_Xoa_Id.Contains((int)x.TIEPDAN_LOAI)).ToList();
                            if (objTiepDan.Count == 0)// Loại đơn thư chưa được sử dụng trong tiếp dân
                            {
                                oCountXoa = objLoaiDonthu_Xoa.Count;
                                vDataContext.LOAIDONTHUs.DeleteAllOnSubmit(objLoaiDonthu_Xoa);
                                vDataContext.SubmitChanges();
                              
                            }
                        }
                       
                    }
                }
                oErrorMessage = "";
            }
            catch (Exception ex)
            {
                oErrorMessage = ex.Message;
            }
        }

        //public void XoaLoaiDonThu(int pLoaiDonThuId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        LOAIDONTHU vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).SingleOrDefault();


        //        if (vLoaiDonThuInfo != null)
        //        {
        //            //vDataContext.LOAIDONTHUs.DeleteOnSubmit(vLoaiDonThuInfo);
        //            //vDataContext.SubmitChanges();\
        //            XoaLoaiDonThuCapCon(vLoaiDonThuInfo.LOAIDONTHU_ID);
        //        }
        //        oErrorMessage = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //    }
        //}
        /// <summary>
        ///  Xóa loại đơn thư
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <returns></returns>
        //public void XoaLoaiDonThuCapCon(int pLoaiDonThuId)
        //{
        //    try
        //    {
        //        //Kiểm tra xem có tồn tại cấp dưới không
        //        var vLoaiDonThuCapDuoiInfos = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == pLoaiDonThuId).Select(x => x.LOAIDONTHU_ID).ToList();

        //        if (vLoaiDonThuCapDuoiInfos.Count > 0)//Không tồn tại cấp dưới xóa
        //        {
        //            foreach (var vLoaiDonThuCapDuoiInfo in vLoaiDonThuCapDuoiInfos)
        //            {
        //                XoaLoaiDonThuCapCon(vLoaiDonThuCapDuoiInfo);
        //            }                   
        //        }
        //        var vLoaiDonThuInfo = vDataContext.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pLoaiDonThuId).FirstOrDefault();
        //        vDataContext.LOAIDONTHUs.DeleteOnSubmit(vLoaiDonThuInfo);
        //        vDataContext.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// Kiểm tra loại đơn thư đang sử dụng
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        //public bool KiemTraLoaiDonThuDangDuocSuDung(int pLoaiDonThuId, out string oErrorMessage)
        //{
        //    try
        //    {
        //        int vCountLoaiDonThu_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.CV_ID == pLoaiDonThuId).Count();
        //        int vCountLoaiDonThu_KhachMoi = vDataContext.KHACHMOIs.Where(x => x.CV_ID == pLoaiDonThuId).Count();
        //        oErrorMessage = "";
        //        if (vCountLoaiDonThu_NguoiDung > 0 || vCountLoaiDonThu_KhachMoi > 0)
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
        ///  Kiểm tra trùng tên loại đơn thư
        /// </summary>
        /// <param name="pLoaiDonThuId"></param>
        /// <param name="pMaLoaiDonThu"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        public bool KiemTraTrungMaLoaiDonThu(int pLoaiDonThuId, string pMaLoaiDonThu, out string oErrorMessage)
            {
                try
                {
                    bool vResult = false;
                    var vLoaiDonThuInfos = (from vLoaiDonThuInfo in vDataContext.LOAIDONTHUs
                                            where vLoaiDonThuInfo.LOAIDONTHU_MA == pMaLoaiDonThu && vLoaiDonThuInfo.LOAIDONTHU_ID != pLoaiDonThuId
                                            select vLoaiDonThuInfo).ToList();
                    if (vLoaiDonThuInfos.Count() > 0)
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
            public LoaiDonThuController()
            {
                //
                // TODO: Add constructor logic here
                //
            }
        }
    }
