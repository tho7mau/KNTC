#region Thông tin chung
/// Mục đích        :BulkApi SmsBrandName Controller
/// Ngày tại        :14/07/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common.Controls;
using SmsBrandName;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Telerik.Web.UI.AsyncUpload;
using Telerik.Web.UI.PivotGrid.Core;

/// <summary>
/// Summary description for DonViController
/// </summary>
namespace KNTC
{
    
    public class SmsController
    {
        #region Khai báo chung
        //HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        
        #endregion

        #region Method
        public bool SendSmsThongBaoPhienHop()
        {
            try
            {
                CcApiClient vCcApiClientInfo = new CcApiClient();                          
                result vResult = vCcApiClientInfo.wsCpMt(ClassParameter.vSmsApiUsername,
                                                        ClassParameter.vSmsApiPassword,
                                                        ClassParameter.vSmsApiCpCode, 
                                                        "1", "84907037607",
                                                        "84907037607",
                                                        ClassParameter.vSmsApiServiceID,
                                                        ClassParameter.vSmsApiCommandCode,
                                                        "UBND_VLG: Test Sms Brandname",
                                                        "0");
                if(vResult.result1 == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Xử lý số điện thoại gởi sms: Thêm mã vùng Việt Nam
        /// </summary>
        /// <param name="pPhoneNumber"></param>
        /// <returns></returns>
        public string ProcessPhoneNumber(string pPhoneNumber)
        {
            string vPhoneNumberResule = "";
            string vTemp = pPhoneNumber;
            if (pPhoneNumber[0] == '0')
            {
                vTemp = pPhoneNumber.Substring(1, pPhoneNumber.Length - 1);
            }
            vPhoneNumberResule = "84" + vTemp;
            return vPhoneNumberResule;
        }

        /// <summary>
        /// Gởi sms cho đại biểu trong phiên họp
        /// </summary>
        /// <param name="pPhienHopInfo"></param>
        /// <param name="pNoiDungGoi"></param>
        /// <returns></returns>
        //public bool SendSmsThongBaoPhienHop(PHIENHOP pPhienHopInfo, string pNoiDungGoi)
        //{
        //    try
        //    {
        //        int vCountSuccess = 0;
        //        int vCountFailure = 0;
        //        CcApiClient vCcApiClientInfo = new CcApiClient();
        //        var vSoDienThoais = (from NguoiDung in vDataContext.NGUOIDUNGs join
        //                                          NguoiDungPhienHop in vDataContext.PHIENHOP_NGUOIDUNGs on NguoiDung.NGUOIDUNG_ID equals NguoiDungPhienHop.NGUOIDUNG_ID
        //                                          where NguoiDungPhienHop.PHIENHOP_ID == pPhienHopInfo.PHIENHOP_ID && NguoiDung.SODIENTHOAI != ""
        //                                     select NguoiDung.SODIENTHOAI).ToArray();
        //        string vNoiDungGoi = ClassParameter.vSmsApiServiceID + ": " + pNoiDungGoi;
        //        if (vSoDienThoais.Count() > 0)
        //        {
        //            //Xử lý sdt
        //            var vThietLapThongGoiThongBao = vDataContext.THONGBAO_THIETLAPs.Where(x => x.Id == 1).FirstOrDefault();
        //            foreach (var sdt in vSoDienThoais)
        //            {
        //                var vSoDienThoaiGoi = ProcessPhoneNumber(sdt);
        //                result vResult = vCcApiClientInfo.wsCpMt(vThietLapThongGoiThongBao.SMS_ApiUsername,
        //                                                vThietLapThongGoiThongBao.SMS_ApiPassword,
        //                                                vThietLapThongGoiThongBao.SMS_ApiCode,
        //                                                vThietLapThongGoiThongBao.SMS_ApiRequestID, 
        //                                                vSoDienThoaiGoi,
        //                                                vSoDienThoaiGoi,
        //                                                vThietLapThongGoiThongBao.SMS_ApiServiceID,
        //                                                vThietLapThongGoiThongBao.SMS_ApiCommandCode,
        //                                               vNoiDungGoi,
        //                                                ClassParameter.vSmsApiGoiCoDau);
        //                if(vResult.result1 == 1)//Gởi thành công
        //                {
        //                    vCountSuccess -= -1;
        //                }
        //                else //Gởi thất bại
        //                {
        //                    vCountFailure = vCountFailure - -1;
        //                }
        //            }
        //            if(vCountSuccess > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }  
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion


        public SmsController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
