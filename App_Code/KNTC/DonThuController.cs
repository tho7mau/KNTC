#region Thông tin chung
/// Mục đích        :Controller đơn thư
/// Ngày tại        :26/03/2021
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
    
    public class DonThuController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion

        /// <summary>
        /// Get danh sách tiếp dân
        /// </summary>
        /// <param name="pKeyWord"></param>
        /// <returns></returns>
        public List<DONTHU> GetDanhSachDonThu(string pKeyWord)
        {
            try
            {
                List<DONTHU> vDONTHUInfo = new List<DONTHU>();
                vDONTHUInfo = vDataContext.DONTHUs.ToList();/* Where(x => SqlMethods.Like(x., "%" + pKeyWord + "%") || SqlMethods.Like(x.MOTA, "%" + pKeyWord + "%")).OrderByDescending(x => x.CV_ID)*/
                return vDONTHUInfo;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public List<V_DONTHU_DOITUONG> getList(string pKeyWord)
        {
            try
            {
                List<V_DONTHU_DOITUONG> vDONTHU_DOITUONGs = new List<V_DONTHU_DOITUONG>();
                vDONTHU_DOITUONGs = vDataContext.V_DONTHU_DOITUONGs.ToList();/* Where(x => SqlMethods.Like(x., "%" + pKeyWord + "%") || SqlMethods.Like(x.MOTA, "%" + pKeyWord + "%")).OrderByDescending(x => x.CV_ID)*/
                return vDONTHU_DOITUONGs;
            }
            catch (Exception ex)
            {
                return null;

            }
        }


        /// <summary>
        /// Get chức vụ theo ID
        /// </summary>
        /// <param name="pDONTHU_ID"></param>
        /// <returns></returns>
        public DONTHU GetAll_Info_DONTHU_ById(int pDONTHU_ID )
        {
            try
            {
                DONTHU vDONTHUInfo = new DONTHU();
                vDONTHUInfo = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == pDONTHU_ID).FirstOrDefault();
                if (vDONTHUInfo != null)
                {                    
                    return vDONTHUInfo;
                }
                else
                {
                    return new DONTHU();
                }
            }
            catch (Exception ex)
            {
                return new DONTHU();
            }
        }

        ///// <summary>
        ///// Thêm mới chức vụ
        ///// </summary>
        ///// <param name="pChucVuInfo"></param>
        ///// <param name="oChucVuId"></param>
        ///// <param name="oErrorMessage"></param>
        //  public void ThemMoiChucVu(CHUCVU pChucVuInfo, out int oChucVuId,  out string oErrorMessage)
        //  {
        //      try
        //      {             
        //          vDataContext.CHUCVUs.InsertOnSubmit(pChucVuInfo);
        //          vDataContext.SubmitChanges();

        //          oChucVuId = vDataContext.CHUCVUs.OrderByDescending(x => x.CV_ID).FirstOrDefault().CV_ID;
        //          oErrorMessage = "";
        //      }
        //      catch (Exception ex)
        //      {
        //          oChucVuId = -1;
        //          oErrorMessage = ex.Message;
        //      }
        //  }
        //  /// <summary>
        //  /// Cập nhật thông tin chúc vụ
        //  /// </summary>
        //  /// <param name="pChucVuId"></param>
        //  /// <param name="pChucVuInfo"></param>
        //  /// <param name="oErrorMessage"></param>
        //  public void CapNhatChucVu(int pChucVuId, CHUCVU pChucVuInfo , out string oErrorMessage)
        //  {
        //      try
        //      {
        //          var vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).SingleOrDefault();
        //          if(vChucVuInfo != null)
        //          {
        //              vChucVuInfo.TENCHUCVU = pChucVuInfo.TENCHUCVU;
        //              vChucVuInfo.MOTA = pChucVuInfo.MOTA;
        //              vDataContext.SubmitChanges();
        //          }                                          
        //          oErrorMessage = "";
        //      }
        //      catch (Exception ex)
        //      {              
        //          oErrorMessage = ex.Message;
        //      }
        //  }

        //  /// <summary>
        //  /// Xóa chức vụ
        //  /// </summary>
        //  /// <param name="pChucVuId"></param>
        //  /// <param name="oErrorMessage"></param>
        //  public void XoaChucVu(int pChucVuId, out string oErrorMessage)
        //  {
        //      try
        //      {
        //          CHUCVU vChucVuInfo = vDataContext.CHUCVUs.Where(x => x.CV_ID == pChucVuId).SingleOrDefault();


        //          if(vChucVuInfo != null)
        //          {
        //              vDataContext.CHUCVUs.DeleteOnSubmit(vChucVuInfo);
        //              vDataContext.SubmitChanges();
        //          }                                              
        //          oErrorMessage = "";
        //      }
        //      catch (Exception ex)
        //      {
        //          oErrorMessage = ex.Message;
        //      }
        //  }

        //  /// <summary>
        //  /// Kiểm tra chức vụ đang sử dụng
        //  /// </summary>
        //  /// <param name="pChucVuId"></param>
        //  /// <param name="oErrorMessage"></param>
        //  /// <returns></returns>
        //  public bool KiemTraChucVuDangDuocSuDung(int pChucVuId, out string oErrorMessage)
        //  {
        //      try
        //      {
        //          int vCountChucVu_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.CV_ID == pChucVuId).Count();
        //          int vCountChucVu_KhachMoi = vDataContext.KHACHMOIs.Where(x => x.CV_ID == pChucVuId).Count();
        //          oErrorMessage = "";
        //          if (vCountChucVu_NguoiDung > 0 || vCountChucVu_KhachMoi > 0)
        //              return true;//Đã được sử dụng
        //          else
        //              return false;//Chưa được sử dụng
        //      }
        //      catch (Exception ex)
        //      {
        //          oErrorMessage = ex.Message;
        //          return true;
        //      }
        //  }



        //  /// <summary>
        //  ///  Kiểm tra trùng tên chức vụ
        //  /// </summary>
        //  /// <param name="pChucVuId"></param>
        //  /// <param name="pTenChucVu"></param>
        //  /// <param name="oErrorMessage"></param>
        //  /// <returns></returns>
        //  public bool KiemTraTrungTenChucVu(int pChucVuId, string pTenChucVu, out string oErrorMessage)
        //  {
        //      try
        //      {
        //          bool vResult = false;
        //          var vChucVuInfos = (from vChucVuInfo in vDataContext.CHUCVUs
        //                    where vChucVuInfo.TENCHUCVU == pTenChucVu && vChucVuInfo.CV_ID != pChucVuId
        //                              select vChucVuInfo).ToList();
        //          if (vChucVuInfos.Count() > 0)
        //              vResult = true; //trùng
        //          else
        //              vResult =  false;//không trùng
        //          oErrorMessage = "";
        //          return vResult;

        //      }
        //      catch (Exception ex)
        //      {
        //          oErrorMessage = ex.Message;
        //          return true;                
        //      }
        //  }
        //  public ChucVuController()
        //  {
        //      //
        //      // TODO: Add constructor logic here
        //      //
        //  }

        #region Khanh - Nhân bản đơn thư
        public long NhanBan(int _DonThuID)
        {
            long _DonThuID_Return = 0 ;
            try
            {
                var objDonThu = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == _DonThuID).FirstOrDefault();
                if (objDonThu != null)
                {
                    DONTHU _NhanBan = new DONTHU();
                    int MaxSTT = int.Parse((vDataContext.DONTHUs.Max(x => x.DONTHU_STT) ?? 0).ToString());
                     _NhanBan.DONTHU_STT = MaxSTT + 1;
                  
                    _NhanBan.NGUONDON_LOAI = objDonThu.NGUONDON_LOAI;
                    _NhanBan.NGUONDON_LOAI_CHITIET = objDonThu.NGUONDON_LOAI_CHITIET;
                    _NhanBan.NGUONDON_DONVI_ID = objDonThu.NGUONDON_DONVI_ID;

                    _NhanBan.NGUONDON_SOVANBANCHUYEN = objDonThu.NGUONDON_SOVANBANCHUYEN;
                    _NhanBan.NGUONDON_NGAYDEDON = objDonThu.NGUONDON_NGAYDEDON; //
                    _NhanBan.NGUONDON_NGAYCHUYEN = objDonThu.NGUONDON_NGAYCHUYEN;//

                    _NhanBan.LOAIDONTHU_ID = objDonThu.LOAIDONTHU_ID;
                    _NhanBan.DONTHU_NOIDUNG = objDonThu.DONTHU_NOIDUNG;
                    _NhanBan.DAGIAIQUYET_DONTHU = objDonThu.DAGIAIQUYET_DONTHU;

                    _NhanBan.DAGIAIQUYET_DONVI_ID = objDonThu.DAGIAIQUYET_DONVI_ID;
                    _NhanBan.DAGIAIQUYET_LAN = objDonThu.DAGIAIQUYET_LAN;
                    _NhanBan.DAGIAIQUYET_HTGQ_ID = objDonThu.DAGIAIQUYET_HTGQ_ID;

                    _NhanBan.DAGIAIQUYET_NGAYBANHANH_QD = objDonThu.DAGIAIQUYET_NGAYBANHANH_QD; //
                    _NhanBan.DAGIAIQUYET_KETQUA_CQ = objDonThu.DAGIAIQUYET_KETQUA_CQ;
                    _NhanBan.DONTHU_KHONGDUDDIEUKIEN = objDonThu.DONTHU_KHONGDUDDIEUKIEN;

                    _NhanBan.DOITUONG_BI_KNTC_ID = objDonThu.DOITUONG_BI_KNTC_ID;
                    _NhanBan.NGUOIUYQUYEN_CANHAN_ID = objDonThu.NGUOIUYQUYEN_CANHAN_ID;
                    _NhanBan.NGUOIUYQUYEN_LOAI = objDonThu.NGUOIUYQUYEN_LOAI;

                    _NhanBan.DONTHU_TRANGTHAI = objDonThu.DONTHU_TRANGTHAI;

                    //_NhanBan.HUONGXULY_ID = objDonThu.HUONGXULY_ID;
                    //_NhanBan.HUONGXULY_TEN = objDonThu.HUONGXULY_TEN;
                    //_NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_ID = objDonThu.HUONGXULY_THAMQUYENGIAIQUYET_ID;
                    //_NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_TEN = objDonThu.HUONGXULY_THAMQUYENGIAIQUYET_TEN;
                    //_NhanBan.HUONGXULY_DONVI_ID = objDonThu.HUONGXULY_DONVI_ID;

                    //_NhanBan.HUONGXULY_CANBO = objDonThu.HUONGXULY_CANBO;
                    //_NhanBan.HUONGXULY_NGAYCHUYEN = objDonThu.HUONGXULY_NGAYCHUYEN;  //
                    //_NhanBan.HUONGXULY_SOHIEUVB_DI = objDonThu.HUONGXULY_SOHIEUVB_DI;

                    //_NhanBan.HUONGXULY_NGUOIDUYET_CANHAN_ID = objDonThu.HUONGXULY_NGUOIDUYET_CANHAN_ID;
                    //_NhanBan.HUONGXULY_YKIEN_XULY = objDonThu.HUONGXULY_YKIEN_XULY;

                    //_NhanBan.HUONGXULY_CHUCVU_TEN = objDonThu.HUONGXULY_CHUCVU_TEN;
                    //_NhanBan.HUONGXULY_THOIHANGIAIQUET = objDonThu.HUONGXULY_THOIHANGIAIQUET;
                    //_NhanBan.KETQUA_XYLY = objDonThu.KETQUA_XYLY;

                    //_NhanBan.KETQUA_NOIDUNG = objDonThu.KETQUA_NOIDUNG;
                    //_NhanBan.KETQUA_NGAY = objDonThu.KETQUA_NGAY;
                    //_NhanBan.KETQUA_NGAYCAPNHAT = objDonThu.KETQUA_NGAYCAPNHAT;

                    //*********** Đặt lại hướng xử lý và kết quả xử lý null khi nhân bản ************** 
                    _NhanBan.HUONGXULY_ID = null;
                    _NhanBan.HUONGXULY_TEN = null;
                    _NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_ID = null;
                    _NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_TEN = null;
                    _NhanBan.HUONGXULY_DONVI_ID = null;

                    _NhanBan.HUONGXULY_CANBO = null;
                    _NhanBan.HUONGXULY_NGAYCHUYEN = null;  //
                    _NhanBan.HUONGXULY_SOHIEUVB_DI = null;

                    _NhanBan.HUONGXULY_NGUOIDUYET_CANHAN_ID = null;
                    _NhanBan.HUONGXULY_YKIEN_XULY = null;

                    _NhanBan.HUONGXULY_CHUCVU_TEN = null;
                    _NhanBan.HUONGXULY_THOIHANGIAIQUET = null;
                    _NhanBan.KETQUA_XYLY = null;

                    _NhanBan.KETQUA_NOIDUNG = null;
                    _NhanBan.KETQUA_NGAY = null;
                    _NhanBan.KETQUA_NGAYCAPNHAT = null;
                    //*********** End ************** 

                    _NhanBan.DONTHU_GHICHU = objDonThu.DONTHU_GHICHU;
                    _NhanBan.NGAYTAO = objDonThu.NGAYTAO;
                    _NhanBan.NGUOICAPNHAT = objDonThu.NGUOICAPNHAT;

                    _NhanBan.NGAYCAPNHAT = objDonThu.NGAYCAPNHAT;
                    _NhanBan.NGUOITAO = objDonThu.NGUOITAO;
                    _NhanBan.DOITUONG_ID = objDonThu.DOITUONG_ID;

                    _NhanBan.LOAIDONTHU_CHA_ID = objDonThu.LOAIDONTHU_CHA_ID;
                    _NhanBan.TRANGTHAI_DONTHUKHONGDUDIEUKIEN = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN;
                    _NhanBan.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = objDonThu.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET;

                    _NhanBan.LOAIDONTHU_CHA_ID = objDonThu.LOAIDONTHU_CHA_ID;
                    _NhanBan.TRANGTHAI_DONTHUKHONGDUDIEUKIEN = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN;
                    _NhanBan.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = objDonThu.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET;

                    _NhanBan.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = objDonThu.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO;
                    _NhanBan.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = objDonThu.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN;
                    _NhanBan.DONTHU_NACDANH = objDonThu.DONTHU_NACDANH;


                    _NhanBan.DONTHU_CU = objDonThu.DONTHU_CU;
                    vDataContext.DONTHUs.InsertOnSubmit(_NhanBan);
                    vDataContext.SubmitChanges();
                    _DonThuID_Return = _NhanBan.DONTHU_ID;
                }
                return _DonThuID_Return;
            }
            catch(Exception ex)
            {
                return _DonThuID_Return;
            }
        
        }

        #endregion
    }
}
