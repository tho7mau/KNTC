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
    
    public class TiepDanController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        #endregion

        /// <summary>
        /// Get danh sách tiếp dân
        /// </summary>
        /// <param name="pKeyWord"></param>
        /// <returns></returns>
        public List<TIEPDAN> GetDanhSachTiepDan(string pKeyWord)
        {
            try
            {
                List<TIEPDAN> vTIEPDANInfo = new List<TIEPDAN>();
                vTIEPDANInfo = vDataContext.TIEPDANs.ToList();/* Where(x => SqlMethods.Like(x., "%" + pKeyWord + "%") || SqlMethods.Like(x.MOTA, "%" + pKeyWord + "%")).OrderByDescending(x => x.CV_ID)*/
                return vTIEPDANInfo;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public List<TIEPDAN_DOITUONG> getList(string pKeyWord)
        {
            try
            {
                List<TIEPDAN_DOITUONG> vTIEPDAN_DOITUONGs = new List<TIEPDAN_DOITUONG>();
                vTIEPDAN_DOITUONGs = vDataContext.TIEPDAN_DOITUONGs.ToList();/* Where(x => SqlMethods.Like(x., "%" + pKeyWord + "%") || SqlMethods.Like(x.MOTA, "%" + pKeyWord + "%")).OrderByDescending(x => x.CV_ID)*/
                return vTIEPDAN_DOITUONGs;
            }
            catch (Exception ex)
            {
                return null;

            }
        }


        /// <summary>
        /// Get chức vụ theo ID
        /// </summary>
        /// <param name="pTIEPDAN_ID"></param>
        /// <returns></returns>
        public TIEPDAN GetAll_Info_TIAPDAN_ById(int pTIEPDAN_ID )
        {
            try
            {
                TIEPDAN vTIEPDANInfo = new TIEPDAN();
                vTIEPDANInfo = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == pTIEPDAN_ID).FirstOrDefault();
                if (vTIEPDANInfo != null)
                {

                    
                    return vTIEPDANInfo;
                }
                else
                {
                    return new TIEPDAN();
                }
            }
            catch (Exception ex)
            {
                return new TIEPDAN();
            }
        }

        #region Nhân bản tiếp dân
        public long NhanBan(int TD_ID)
        {
            long _TiepDan_Return = 0;
            try
            {

                var objTiepDan = vDataContext.TIEPDANs.Where(x => x.TIEPDAN_ID == TD_ID).FirstOrDefault();
                if (objTiepDan != null)
                {
                    TIEPDAN _tiepDan = new TIEPDAN();
                    int MaxSTT = int.Parse((vDataContext.TIEPDANs.Max(x => x.TIEPDAN_STT) ?? 0).ToString());
                    _tiepDan.TIEPDAN_STT = MaxSTT + 1;
                    _tiepDan.TIEPDAN_NOIDUNG = objTiepDan.TIEPDAN_NOIDUNG;
                    _tiepDan.TIEPDAN_KETQUA = objTiepDan.TIEPDAN_KETQUA;
                    _tiepDan.TIEPDAN_THOGIAN = objTiepDan.TIEPDAN_THOGIAN;
                 
                    _tiepDan.NGUOITAO = objTiepDan.NGUOITAO;
                    _tiepDan.NGUOICAPNHAT = objTiepDan.NGUOICAPNHAT;
                    _tiepDan.NGAYTAO = objTiepDan.NGAYTAO;
                    _tiepDan.NGAYCAPNHAT = objTiepDan.NGAYCAPNHAT;
                    _tiepDan.TIEPDAN_LANTIEP = objTiepDan.TIEPDAN_LANTIEP;
                    _tiepDan.TIEPDAN_CANBO_TIEP_ID = objTiepDan.TIEPDAN_CANBO_TIEP_ID;
                    _tiepDan.DOITUONG_ID = objTiepDan.DOITUONG_ID;
                    _tiepDan.TIEPDAN_LOAI = objTiepDan.TIEPDAN_LOAI;
                    _tiepDan.TIEPDAN_LOAI_CHA_ID = objTiepDan.TIEPDAN_LOAI_CHA_ID;
                    _tiepDan.TIEPDAN_BTD = objTiepDan.TIEPDAN_BTD;
                    _tiepDan.TIEPDAN_CU = objTiepDan.TIEPDAN_CU;                 
                    // Thêm tiếp đơn thư nếu tiếp dân là có đơn
                    DonThuController donThuController = new DonThuController();
                    if (objTiepDan.DONTHU_ID != null)
                    {
                      long _DonThuID =  donThuController.NhanBan((int)objTiepDan.DONTHU_ID);
                       _tiepDan.DONTHU_ID = _DonThuID;
                    }
                    vDataContext.TIEPDANs.InsertOnSubmit(_tiepDan);
                    vDataContext.SubmitChanges();
                    _TiepDan_Return = _tiepDan.TIEPDAN_ID;
                }

                //var objDonThu = vDataContext.DONTHUs.Where(x => x.DONTHU_ID == _DonThuID).FirstOrDefault();
                //if (objDonThu != null)
                //{
                //    DONTHU _NhanBan = new DONTHU();
                //    int MaxSTT = int.Parse((vDataContext.DONTHUs.Max(x => x.DONTHU_STT) ?? 0).ToString());
                //    _NhanBan.DONTHU_STT = MaxSTT + 1;

                //    _NhanBan.NGUONDON_LOAI = objDonThu.NGUONDON_LOAI;
                //    _NhanBan.NGUONDON_LOAI_CHITIET = objDonThu.NGUONDON_LOAI_CHITIET;
                //    _NhanBan.NGUONDON_DONVI_ID = objDonThu.NGUONDON_DONVI_ID;

                //    _NhanBan.NGUONDON_SOVANBANCHUYEN = objDonThu.NGUONDON_SOVANBANCHUYEN;
                //    _NhanBan.NGUONDON_NGAYDEDON = objDonThu.NGUONDON_NGAYDEDON; //
                //    _NhanBan.NGUONDON_NGAYCHUYEN = objDonThu.NGUONDON_NGAYCHUYEN;//

                //    _NhanBan.LOAIDONTHU_ID = objDonThu.LOAIDONTHU_ID;
                //    _NhanBan.DONTHU_NOIDUNG = objDonThu.DONTHU_NOIDUNG;
                //    _NhanBan.DAGIAIQUYET_DONTHU = objDonThu.DAGIAIQUYET_DONTHU;

                //    _NhanBan.DAGIAIQUYET_DONVI_ID = objDonThu.DAGIAIQUYET_DONVI_ID;
                //    _NhanBan.DAGIAIQUYET_LAN = objDonThu.DAGIAIQUYET_LAN;
                //    _NhanBan.DAGIAIQUYET_HTGQ_ID = objDonThu.DAGIAIQUYET_HTGQ_ID;

                //    _NhanBan.DAGIAIQUYET_NGAYBANHANH_QD = objDonThu.DAGIAIQUYET_NGAYBANHANH_QD; //
                //    _NhanBan.DAGIAIQUYET_KETQUA_CQ = objDonThu.DAGIAIQUYET_KETQUA_CQ;
                //    _NhanBan.DONTHU_KHONGDUDDIEUKIEN = objDonThu.DONTHU_KHONGDUDDIEUKIEN;

                //    _NhanBan.DOITUONG_BI_KNTC_ID = objDonThu.DOITUONG_BI_KNTC_ID;
                //    _NhanBan.NGUOIUYQUYEN_CANHAN_ID = objDonThu.NGUOIUYQUYEN_CANHAN_ID;
                //    _NhanBan.NGUOIUYQUYEN_LOAI = objDonThu.NGUOIUYQUYEN_LOAI;

                //    _NhanBan.DONTHU_TRANGTHAI = objDonThu.DONTHU_TRANGTHAI;

                //    //*********** Đặt lại hướng xử lý và kết quả xử lý null khi nhân bản ************** 
                //    _NhanBan.HUONGXULY_ID = null;
                //    _NhanBan.HUONGXULY_TEN = null;
                //    _NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_ID = null;
                //    _NhanBan.HUONGXULY_THAMQUYENGIAIQUYET_TEN = null;
                //    _NhanBan.HUONGXULY_DONVI_ID = null;

                //    _NhanBan.HUONGXULY_CANBO = null;
                //    _NhanBan.HUONGXULY_NGAYCHUYEN = null;  //
                //    _NhanBan.HUONGXULY_SOHIEUVB_DI = null;

                //    _NhanBan.HUONGXULY_NGUOIDUYET_CANHAN_ID = null;
                //    _NhanBan.HUONGXULY_YKIEN_XULY = null;

                //    _NhanBan.HUONGXULY_CHUCVU_TEN = null;
                //    _NhanBan.HUONGXULY_THOIHANGIAIQUET = null;
                //    _NhanBan.KETQUA_XYLY = null;

                //    _NhanBan.KETQUA_NOIDUNG = null;
                //    _NhanBan.KETQUA_NGAY = null;
                //    _NhanBan.KETQUA_NGAYCAPNHAT = null;
                //    //*********** End ************** 

                //    _NhanBan.DONTHU_GHICHU = objDonThu.DONTHU_GHICHU;
                //    _NhanBan.NGAYTAO = objDonThu.NGAYTAO;
                //    _NhanBan.NGUOICAPNHAT = objDonThu.NGUOICAPNHAT;

                //    _NhanBan.NGAYCAPNHAT = objDonThu.NGAYCAPNHAT;
                //    _NhanBan.NGUOITAO = objDonThu.NGUOITAO;
                //    _NhanBan.DOITUONG_ID = objDonThu.DOITUONG_ID;

                //    _NhanBan.LOAIDONTHU_CHA_ID = objDonThu.LOAIDONTHU_CHA_ID;
                //    _NhanBan.TRANGTHAI_DONTHUKHONGDUDIEUKIEN = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN;
                //    _NhanBan.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = objDonThu.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET;

                //    _NhanBan.LOAIDONTHU_CHA_ID = objDonThu.LOAIDONTHU_CHA_ID;
                //    _NhanBan.TRANGTHAI_DONTHUKHONGDUDIEUKIEN = objDonThu.TRANGTHAI_DONTHUKHONGDUDIEUKIEN;
                //    _NhanBan.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET = objDonThu.TRANGTHAI_THONGTINCOQUANDAGIAIQUYET;

                //    _NhanBan.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO = objDonThu.TRANGTHAI_BOSUNGTHONGTINNGUOIBIKHIEUNAITOCAO;
                //    _NhanBan.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN = objDonThu.TRANGTHAI_BOSUNGTHONGTINNGUOIDAIDIENUYQUYEN;
                //    _NhanBan.DONTHU_NACDANH = objDonThu.DONTHU_NACDANH;


                //    _NhanBan.DONTHU_CU = objDonThu.DONTHU_CU;
                //    vDataContext.DONTHUs.InsertOnSubmit(_NhanBan);
                //    vDataContext.SubmitChanges();
                //    _TiepDan_Return = _NhanBan.DONTHU_ID;
                //}

                return _TiepDan_Return;
            }
            catch (Exception ex)
            {
                return _TiepDan_Return;
            }

        }
        #endregion
    }
}
