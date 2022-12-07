#region
// File name    :   ClassParameter.cs
// Purpose      :   Cac tham so su dung chung cho toan du an
// Create date  :   10/10/2020
// Author       :   NHHAN
// Version      :   v1.0
// Copyright    :   LIINK
#endregion
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DotNetNuke;
using DotNetNuke.Common;


namespace KNTC
{
    public class ClassParameter
    {       
        public static string vKhongTinThayDuLieu = "Không tìm thấy dữ liệu";
        public static int vSizeFile = (5242880 * 2); // 5Mb        
        public static int vPageSizeTimKiem = 10;
        public static int vKyTuNoiDung = 60000;
        public static string vPathCommonUploadHoSo = "/DesktopModules/KNTC/Upload/HOSO";
        public static string vPathDataBieuMau = "/DesktopModules/KNTC/bieumau";
        public static string vPathBieuMau = DotNetNuke.Common.Globals.ApplicationPath + "/DesktopModules/KNTC/bieumau/";

        public static string vPathCommonJS = DotNetNuke.Common.Globals.ApplicationPath + "/DesktopModules/KNTC/Scripts/common.js";
        public static string vPathCommonToastJS = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/js/toastr.js";
        public static string vPathCommonToastCSS = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/css/toastr.css";
        public static string vPathIQueryJavascript = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/Scripts/jquery/";
        public static string vPathCommonJavascript = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/Scripts/common.js";
        public static string vPathCommonJavascriptwz_ToolTip = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/Scripts/wz_tooltip/";
        public static string vPathCommonSettingXML = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/setting.xml";
        public static string vPathCommonSettingDefauttXML = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/default.xml";
        public static string vPathPrototypeJavascript = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/Scripts/prototype.js";
        public static string vPathCommonImages = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/images/";
        public static string vPathCommonConfigXML =  "/DesktopModules/QLVB/Config/Config.xml";
        public static int vMaxLengthFCK = 80000;
        public static string vPathCommon = DotNetNuke.Common.Globals.ApplicationPath.ToString();       
        public static string vPathCommonQLVB = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/";
        public static string vPathMudim = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/QLVB/Scripts/Mudim.js";
        public static string[] vLoaiFileHopLe = { @"application/msword", @"application/excel", @"application/pdf", @"application/vnd.ms-excel", @"application/msexcel" };
        public static string vKhongDuQuyen = "Không được phép truy cập";
        public static string vKhongHopLe = "Dữ liệu không hợp lệ";      
        public static string vFileUploadKhongHopLe = "Vui lòng tải lên tập tin có định dạng (.doc, .xls)";
        public static string vFileUploadQuaLon = "Tập tin tải lên quá lớn";
        public static int vPageSize = 30;

        public static string vSmsApiUsername = "smsbrand_ubndvlg";
        public static string vSmsApiPassword = "Khdn@2019";
        public static string vSmsApiCpCode = "UBNDVLG";
        public static string vSmsApiRequestID = "1";
        public static string vSmsApiServiceID = "UBND_VLG";
        public static string vSmsApiCommandCode = "bulksms";
        public static string vSmsApiGoiCoDau = "1";
        public static string vSmsApiGoiKhongDau = "0";

        public static string UrlServiceR = "http://localhost:9987/api/";
        public static string msbv = "BV001";
        public static string UrlServiceN = "http://localhost:9988/api/";
        public static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwOTY0ODQzMzg4IiwianRpIjoiMzM1NWQxNDYtMTFlMS00MDQ1LWFhNDAtNzU4MjYwNzlmN2JjIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI1ZDk1NzVkMDdjNzgwOTE5OTgyYWM3NGMiLCJleHAiOjE1Nzg0NzIxNDIsImlzcyI6IkxpaW5rIiwiYXVkIjoiTGlpbmsifQ.hbmrCPHBKRhNsLnoEthk8VJd_JX0_8Xqn9JEfjunZ34";
        public static string vJavascriptMaskNumber = "<script type='text/javascript' src='" + DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/HOPKHONGGIAY/Scripts/Mask/jquery.metadata.js'></script>"
       + "<script type='text/javascript' src='" + DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/HOPKHONGGIAY/Scripts/Mask/autoNumeric-1.6.2.js'></script>";
        public static string vJavascriptMask = "<script type='text/javascript' src='" + DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/HOPKHONGGIAY/Scripts/Mask/jquery.metadata.js'></script>";

        public static int vSoLuongSlotGioTrongBuoi = 2;
        public static int vSoLuongLuotKham = 10;
        public static string vPathCommonHopKhongGiay = DotNetNuke.Common.Globals.ApplicationPath.ToString() + "/DesktopModules/KNTC/";

        public static string OneSignalAppID = "7573fff2-376c-4c37-953c-56aa5e485fdc";
        public static string OneSignalRestAPIKey = "OTAyYWM3NjItMWZmYS00NTlmLWE1ZDctNGNmMDRjZTMzNDI2";

        public static int vDiaPhuongDefault = 55; // -1: không load mặt định
        public static int vDanTocDefault = 1; // -1: không load mặt định
        public static int vQuocTichDefault = 1234; // -1: không load mặt định

        public static string v_ModuleByDefinition_TIEPDAN = "Quản lý tiếp dân";
        public static string v_ModuleByDefinition_DONTHU = "Quản lý đơn thư"; // KNTC Quản lý đơn thư

        public static int vKhieuNai_ID = 1;
        public static int vToCao_ID = 2004;
        public static int vPAKN_ID = 4035;
        public static int vNhieuNoiDung_ID = 4051;
    }
}
