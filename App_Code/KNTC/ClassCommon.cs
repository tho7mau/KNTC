using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Log.EventLog;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Threading;
using DotNetNuke;
using DotNetNuke.Framework;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Web.Configuration;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System.Net.NetworkInformation;

/// <summary>
/// Summary description for ClassCommon
/// </summary>
/// 
namespace KNTC
{
    public class ClassCommon
    {
        HttpContext context;

        public ClassCommon(HttpContext Context)
        {
            context = Context;
        }

        public ClassCommon()
        {

        }


        /// <summary>
        /// Get địa chỉ MAC
        /// </summary>
        /// <returns></returns>        
        public static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;

        }
      
        public static void ShowToastr(Page page, string message, string title, string type = "info")
        {
            UserInfo _User = new UserInfo();
            THONGBAO_TOASTR(page, null, _User, message, title, type);
        }
        /// <summary>
        /// Get URL MODULE BY PORTAL_ID, DEFINITION
        /// </summary>
        /// <param name="PoralId"></param>
        /// <param name="ModuleByDefinition Name"></param>
        /// <param name="Param"></param>
        /// <author>NHTTAI</author>
        public string GET_URL_MODULE(int p_PoralId, string p_ModuleByDefinition, string p_Param)
        {
            ModuleInfo objModuleInfo = new ModuleInfo();
            ModuleController objModuleController = new ModuleController();
            objModuleInfo = objModuleController.GetModuleByDefinition(p_PoralId, p_ModuleByDefinition);
            string DomainName =HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            if (objModuleInfo != null)
            {
                return DomainName + "/Default.aspx?tabid=" + objModuleInfo.TabID.ToString() + p_Param;
            }
            else
            {
                return "";
            }
        }

        // <summary>
        /// Giới hạn chữ hiện thị chữ
        /// </summary>
        /// <param name="VanBan">Văn bản cần giới hạn</param>
        /// <param name="SoChu">Số chữ cần hiện thị</param>
        public static string GioiHanChu(string VanBan, int SoChu)
        {
            string[] Words = VanBan.Split(' ');
            string _return = string.Empty;
            if (Words.Length <= SoChu)
            {
                _return = VanBan;
            }
            else
            {
                for (int i = 0; i < SoChu; i++)
                {
                    _return += Words.GetValue(i).ToString() + " ";
                }
                _return += "...";
            }
            return _return.ToString();
        }

        /// <summary>
        /// Giới hạn chữ hiện thị chữ
        /// </summary>
        /// <param name="VanBan">Văn bản cần giới hạn</param>
        /// <param name="SoChu">Số chữ cần hiện thị</param>
        public string GioiHanChu_Biding(object objVanBan, int SoChu)
        {
            try
            {

                if (objVanBan != null)
                {
                    string VanBan = objVanBan.ToString();
                    string[] Words = VanBan.Split(' ');
                    string _return = string.Empty;
                    if (Words.Length <= SoChu)
                    {
                        _return = VanBan;
                    }
                    else
                    {
                        for (int i = 0; i < SoChu; i++)
                        {
                            _return += Words.GetValue(i).ToString() + " ";
                        }
                        _return += "...";
                    }
                    return _return.ToString();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {
                return "";
            }




        }



        /// <summary>
        /// Xoa khoang cach thua
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static String RemoveSpace(String pStr)
        {
            if (pStr != "")
            {
                //pStr = pStr.Replace("  ", " ");
                string strPattern = @"[\s]+";
                Regex rgx = new Regex(strPattern);
                pStr = rgx.Replace(pStr, " ");
            }
            return pStr;
        }
        /// <summary>
        /// Check Key have Exist In Dictionary
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pDict"></param>
        /// <returns></returns>
        public static bool ExistKey(string pKey, Dictionary<string, string> pDict)
        {
            bool b = false;
            foreach (string key in pDict.Keys)
            {
                if (key == pKey)
                    b = true;
            }
            return b;
        }
        /// <summary>
        /// Xóa khoảng trắng
        /// </summary>
        /// <param name="arrstr">Mảng đã cắt kí tự xuống dòng</param>
        /// <returns>Mảng các chuỗi chỉ có 1 khoảng trắng</returns>
        public static string[] XoaKhoangTrang(string[] arrstr)
        {
            if (arrstr.Length > 0)
            {
                for (int i = 0; i < arrstr.Length; i++)
                {
                    arrstr[i] = arrstr[i].Replace("  ", " ");
                }
            }
            return arrstr;
        }

        // <summary>
        /// Kiem tra user hien tai chi co quyen view
        /// </summary>
        /// <param name="pUserInfo">UserInfo</param>
        public static bool checkViewRole(DotNetNuke.Entities.Users.UserInfo pUserInfo)
        {
            if (pUserInfo.IsSuperUser)
            {
                return false;
            }
            return pUserInfo.IsInRole(ConfigurationManager.AppSettings["Role_DC"]);
        }
        /// <summary>
        /// thông báo toastr
        /// </summary>
        /// <param name="p_Page">Page</param>
        /// <param name="p_Ex">Ngoại lệ</param>
        /// <param name="p_User">UserInfo</param>
        /// <param name="p_Message">Nội dung thông báo nếu có</param>
        /// <param name="p_Title">Tiêu đề thông báo</param>
        /// <param name="p_Type">Loại thông báo</param>
        public static void THONGBAO_TOASTR(Page p_Page, Exception p_Ex, UserInfo p_User, string p_Message, string p_Title, string p_Type = "info")
        {
            if (p_Type == "error")
            {
                if (p_User.IsSuperUser == true && p_Ex != null)
                {
                    ScriptManager.RegisterStartupScript(p_Page, p_Page.GetType(), "toastr_message",
                    String.Format("javascript:toastr.{0}('{1}','{2}');", p_Type.ToLower(), p_Ex.Message, ""), addScriptTags: true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(p_Page, p_Page.GetType(), "toastr_message",
                        String.Format("javascript:toastr.{0}('{1}', '{2}');", p_Type.ToLower(), p_Message, ""), addScriptTags: true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(p_Page, p_Page.GetType(), "toastr_message",
                        String.Format("javascript:toastr.{0}('{1}', '{2}');", p_Type.ToLower(), p_Message, ""), addScriptTags: true);
            }
        }

        /// <summary>
        /// thông báo ngoại lệ
        /// </summary>
        /// <param name="pLblThongBao">label hiển thị</param>
        /// <param name="p_NoiDungThongBao">Nội dung thông báo nếu có</param>
        /// <param name="ex">Ngoại lệ</param>
        /// <param name="user">UserInfo</param>
        public static void ThongBaoNgoaiLe(Label pLblThongBao, string p_NoiDungThongBao, Exception ex, UserInfo user)
        {
            if (user.IsSuperUser == true)
            {
                pLblThongBao.Text = ex.ToString();
            }
            else
            {
                if (p_NoiDungThongBao == null && p_NoiDungThongBao == "")
                {
                    pLblThongBao.Text = "Lỗi trong quá trình xử lý. Vui lòng liên hệ quản trị.";
                }
                else
                {
                    pLblThongBao.Text = p_NoiDungThongBao;
                }
            }
            return;
        }

        /// <summary>
        /// thông báo ngoại lệ
        /// </summary>
        /// <param name="ppnThongBao">Panel thông báo</param>
        /// <param name="pLblThongBao">label hiển thị</param>
        /// <param name="p_NoiDungThongBao">Nội dung thông báo nếu có</param>
        /// <param name="ex">Ngoại lệ</param>
        /// <param name="user">UserInfo</param>
        public static void ThongBaoNgoaiLe(Panel ppnThongBao, Label pLblThongBao, string p_NoiDungThongBao, Exception ex, UserInfo user)
        {
            if (user.IsSuperUser == true && ex != null)
            {
                ppnThongBao.Visible = true;
                pLblThongBao.Text = ex.ToString();
            }
            else
            {

                ppnThongBao.Visible = true;
                if (p_NoiDungThongBao == null && p_NoiDungThongBao == "")
                {
                    pLblThongBao.Text = "Lỗi trong quá trình xử lý. Vui lòng liên hệ uản trị.";
                }
                else
                {
                    pLblThongBao.Text = p_NoiDungThongBao;
                }
            }
            return;
        }

        public static string DinhDangNgayViet(string thangNgayNam)
        {
            string ngayTraVe = "";
            CultureInfo MyCul = new CultureInfo("");
            DateTime dtNgay = Convert.ToDateTime(thangNgayNam);
            ngayTraVe = ((dtNgay.Day.ToString().Length == 1) ? "0" + dtNgay.Day.ToString() : dtNgay.Day.ToString()) + "/" + ((dtNgay.Month.ToString().Length == 1) ? "0" + dtNgay.Month.ToString() : dtNgay.Month.ToString()) + "/" + dtNgay.Year.ToString();
            return ngayTraVe;
        }

        /// <summary>
        /// Hien thi Ngay/Thang/Nam theo dinh dang "25/04/2007"
        /// </summary>
        /// <param name="value">Gia tri kieu DateTime</param>
        /// <returns>Chuoi duoc dinh dang theo "dd/MM/yyyy"</returns>
        public static string HienThiNgayThangNam(DateTime value)
        {
            string doiValue = String.Format("{0:dd/MM/yyyy}", value);
            return doiValue;
        }

        public static DataSet ConvertDataReadertoDataSet(IDataReader reader)
        {
            DataSet ds = new DataSet();
            DataTable dataTable = new DataTable();

            DataTable schemaTable = reader.GetSchemaTable();

            DataRow row;
            string columnName;
            DataColumn column;

            int count = schemaTable.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                row = schemaTable.Rows[i];
                columnName = (string)row["ColumnName"];
                column = new DataColumn(columnName, (Type)row["DataType"]);
                dataTable.Columns.Add(column);
            }
            ds.Tables.Add(dataTable);

            object[] values = new object[count];

            try
            {
                dataTable.BeginLoadData();
                while (reader.Read())
                {
                    reader.GetValues(values);
                    dataTable.LoadDataRow(values, true);
                }

            }
            finally
            {
                dataTable.EndLoadData();
                reader.Close();
            }

            return ds;
        }

        /// <summary>
        /// Buoc dinh danh cot kinh phi co dang 123.456.789 theo kieu tien te
        /// </summary>
        /// <author>DVTIN</author>
        public static void DinhDangTienTe()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";

            //Thread.CurrentThread.CurrentCulture.LCID = 0x409;
        }

        /// <summary>
        /// Chèn tiêu đề phân trang dùng chung
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>NVKIET</author>
        public static void ChenTieuDePhanTrang(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Pager)
            {
                TableCell Pager = (TableCell)e.Item.Controls[0];

                Label lblPagerText = new Label();
                lblPagerText.ID = "lblPagerText";
                lblPagerText.Text = "Trang: ";
                Pager.Controls.AddAt(0, lblPagerText);

                for (int i = 1; i <= Pager.Controls.Count; i += 2)
                {
                    object pgNumbers = Pager.Controls[i];
                    int endPagingIndex = Pager.Controls.Count - 1;

                    if (pgNumbers.GetType().Name == "DataGridLinkButton")
                    {
                        LinkButton lb = (LinkButton)pgNumbers;
                        if (lb.Text == "...")
                        {
                            if (i == 1)
                            {
                                lb.Text = "Trước";
                            }
                            else if (i == endPagingIndex)
                            {
                                lb.Text = "Tiếp";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check Email
        /// </summary>
        /// <param name="btnSubmit"></param>
        /// <param name="csm"></param>
        /// <author>NVKIET</author>
        public static bool IsValidEmailAddress(string email)
        {
            try
            {
                var emailChecked = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ngan submit nhieu lan
        /// </summary>
        /// <param name="btnSubmit"></param>
        /// <param name="csm"></param>
        /// <author>NVKIET</author>
        public static void PreventDoublePost(Page pPage)
        {
            string script =
                  @"<script>
                    function  btnSubmit(name)
                    {
                        if (typeof(Page_ClientValidate) == 'function')
                        { 
                            if (Page_ClientValidate() == false)
                            { 
                                return false; 
                            }
                        } 
                        var inputList = " + pPage.Form.Name + @".getElementsByTagName('input');
                        for (var i = 0; i < inputList.length; i++)
                        {
                            if (inputList[i].type == 'submit')
                            {
                                inputList[i].disabled = true;
                            }
                        }"
                        +
                        //btnSubmit.Page.ClientScript.GetPostBackEventReference(btnSubmit, string.Empty)
                        "__doPostBack(name,'');"
                        + @";}
                    </script>";
            pPage.ClientScript.RegisterClientScriptBlock(typeof(string), "", script);
            //btnSubmit.Attributes["onclick"] = "btnSubmit(this.name);";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btnSubmit"></param>
        public static void AddPreventDoublePost(ref Button btnSubmit)
        {
            btnSubmit.Attributes["onclick"] = "btnSubmit(this.name);";
        }

        /// <summary>
        /// Chuyen  thành HTML chuoi
        /// </summary>
        /// <param name="strHTML">string HTML</param>
        /// <returns></returns>
        public static string ClearHTML(string strHTML)
        {
            strHTML = strHTML.Replace("<", "&lt;");
            strHTML = strHTML.Replace(">", "&gt;");
            return strHTML;
        }
        /// <summary>
        /// Ký tự đặt biết của keyword
        /// </summary>
        /// <param name="strHTML">string HTML</param>
        /// <returns></returns>
        public static string ClearKeyword(string str)
        {
            str = str.Replace("'", "''");
            str = str.Replace("_", "/_");
            str = str.Replace("%", "/%");
            str = str.Replace("[", "/[");
            str = str.Replace("/", "//");
            return str;
        }
        /// <summary>
        /// Chuyen chuoi thanh HTML
        /// </summary>
        /// <param name="strHTML">Chuoi</param>
        /// <returns></returns>
        public static string HTML(string strHTML)
        {

            strHTML = strHTML.Replace("&lt;", "<");
            strHTML = strHTML.Replace("&gt;", ">");
            return strHTML;
        }

        /// <summary>
        /// Base64 Encode
        /// </summary>
        /// <param name="pData">string</param>
        /// <returns>bool</returns>
        public static string base64Encode(string pData)
        {
            try
            {
                byte[] encData_byte = new byte[pData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(pData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        /// <summary>
        /// Base64 Decode
        /// </summary>
        /// <param name="pData">string</param>
        /// <returns>bool</returns>
        public static string base64Decode(string pData)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(pData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        /// <summary>
        /// Dieu chinh page number cua data grid
        /// </summary>
        /// <param name="pRowCount">tong so row cua data</param>
        /// <param name="pPageSize">so row hien thi tren 1 page</param>
        /// <param name="pCurrentPageIndex">so trang hien tai</param>
        /// <returns>so trang du'ng nhat sau khi phan tich</returns>
        /// <author>NHTIN</author>
        public static int CorrectDGPageIndex(int pRowCount, int pPageSize, int pCurrentPageIndex)
        {
            int vMaxPage = Math.Max((int)Math.Ceiling((double)pRowCount / (double)pPageSize) - 1, 0);

            return vMaxPage > pCurrentPageIndex ? pCurrentPageIndex : vMaxPage;
        }


        /// <summary>
        /// Kiem tra dang nhap
        /// </summary>
        /// <param name="pPage">current page</param>
        /// <author>NHTIN</author>
        public static void MustLogin(DotNetNuke.Entities.Modules.PortalModuleBase pPage)
        {
            if (Null.IsNull(pPage.UserId))
            {
                pPage.Response.Redirect(Globals.NavigateURL("Login", new string[] { "returnurl=" + HttpUtility.UrlEncode(pPage.Request.Url.AbsoluteUri) }), true);
            }
        }

        public static string GetFileName(string pFullPath)
        {
            int x = pFullPath.LastIndexOf(@"\");

            if (x != -1)
            {
                x += 1;
                return pFullPath.Substring(x, pFullPath.Length - x);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Loai HTML ra khoi chuoi
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        /// <author>NHTIN</author>
        public static string StripHTML(string pHtmlString)
        {
            string vPattern = @"(<|&lt;)(.|\n)*?(>|&gt;)";
            return Regex.Replace(pHtmlString, vPattern, string.Empty);
        }

        /// <summary>
        /// <script></script>
        /// </summary>
        /// <param name="pHtmlString"></param>
        /// <returns></returns>
        /// <author>NHTIN</author>
        public static string StripScriptBlock(string pHtmlString)
        {
            //string vPattern = "(<|&lt;)script.*/*(>|&gt;)|(<|&lt;)/script(>|&gt;)|(<|&lt;)[a-z]+[^(>|&gt;)]*=('|\"|&quot;)+javascript:\\w+.*('|\"|&quot;)+(>|&gt;)|(<|&lt;)\\w+[^(>|&gt;)]*\\son\\w+\\s*=(.|\\n)*(>|&gt;)|(<|&lt;)iframe.*/*(>|&gt;)(<|&lt;)/iframe(>|&gt;)|(<|&lt;)object.*/*(>|&gt;)|(<|&lt;)/object(>|&gt;)";
            string vPattern = "(<|&lt;)script(.|\n)*?(>|&gt;)|(<|&lt;)\\/script(>|&gt;)";
            return Regex.Replace(pHtmlString, vPattern, string.Empty, RegexOptions.IgnoreCase);
        }

        public static string StringSafe(string pString, bool pIsStripAllHTML)
        {
            //strip <script> block
            pString = StripScriptBlock(pString);

            //strip all html
            if (pIsStripAllHTML)
            {
                pString = StripHTML(pString);
            }

            return pString;
        }

        /// <summary>
        /// Validator cho StringSafe
        /// </summary>
        /// <param name="pSource">CustomValidator object</param>
        /// <param name="args">ServerValidateEventArgs</param>
        /// <param name="pIsStripAllHTML">tùy chọn bỏ hết html hay ko</param>
        /// <author>NHTIN</author>
        public static void StringSafeValidate(ref object pSource, ref System.Web.UI.WebControls.ServerValidateEventArgs pArgs, bool pIsStripAllHTML)
        {
            CustomValidator cv = (CustomValidator)pSource;
            Control con = cv.FindControl(cv.ControlToValidate);

            string vValue = pArgs.Value.Trim() + " ";
            vValue = StringSafe(vValue, pIsStripAllHTML);

            if (vValue.Trim() != "")
            {
                pArgs.IsValid = true;
            }
            else
            {
                pArgs.IsValid = false;
            }

            if (typeof(TextBox) == con.GetType())
            {
                ((TextBox)con).Text = vValue.Trim();
            }

            {
                try
                {
                    ((DotNetNuke.UI.UserControls.TextEditor)con).Text = vValue.Trim();
                }
                catch { }
            }
        }

        /// <summary>
        /// Kiểm tra filesize và loại file được upload lên
        /// </summary>
        /// <param name="pSource">CustomValidator object</param>
        /// <param name="pFile">đối tượng HttpPostedFile</param>
        /// <param name="pFileType">mảng mime filetype</param>
        /// <returns></returns>
        public static bool KiemTraFileUpload(ref object pSource, HttpPostedFile pFile, string[] pFileType)
        {
            //CustomValidator cv = (CustomValidator)pSource;

            //if (pFile.ContentLength > ClassParameter.vSizeFile)
            //{
            //    cv.ErrorMessage = ClassParameter.vFileUploadQuaLon;
            //    return false;
            //}
            //else if (!KiemTraLoaiFile(pFile, pFileType))
            //{
            //    cv.ErrorMessage = ClassParameter.vFileUploadKhongHopLe;
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Upload file moi, xoa cu
        /// </summary>
        /// <param name="pUploadFile">HttpPostedFile</param>
        /// <param name="pSavePath">Duong dan</param>
        /// <param name="pFileName">Tên file</param>
        /// <returns>bool: true upload thành công, false không thành công</returns>
        /// <author>Châu Đức Toàn</author>
        public static bool UploadFile(HttpPostedFile pUploadFile, string pSavePath, string pFileName, string pOldFileName)
        {
            if (pUploadFile != null && pUploadFile.ContentLength > 0)
            {
                try
                {
                    XoaFile(pSavePath, pFileName);
                    pUploadFile.SaveAs(pSavePath + "\\" + pFileName);

                    //Resize---------------------------------------------------------------
                    //System.Drawing.Image myimg = System.Drawing.Image.FromFile(pSavePath + "\\" + pFileName);

                    //myimg = myimg.GetThumbnailImage(100, 100, null, IntPtr.Zero);

                    //myimg.Save(pSavePath + "\\" + pFileName.Replace(".jpg","_") + "resize.jpg", myimg.RawFormat);
                    //-----------------------------------------------------------------

                    if (pOldFileName != "")
                    {
                        XoaFile(pSavePath, pOldFileName);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiem tra file
        /// </summary>
        /// <param name="pUploadFile">HttpPostedFile</param>
        /// <param name="pFileTypeList">Danh sách file hop le</param>
        /// <returns>bool</returns>
        /// <author>Châu Đức Toàn</author>
        public static bool KiemTraLoaiFile(HttpPostedFile pUploadFile, string[] pFileTypeList)
        {
            bool vHopLe = false;

            if (pUploadFile != null && pUploadFile.ContentLength > 0 && pUploadFile.ContentLength < ClassParameter.vSizeFile)
            {
                for (int i = 0; i < pFileTypeList.Length; i++)
                {
                    if (pUploadFile.ContentType == pFileTypeList[i])
                    {
                        vHopLe = true;
                        break;
                    }
                }
            }

            return vHopLe;
        }

        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="pSavePath"></param>
        /// <param name="pFileName">Tên file can xóa</param>
        /// <returns>bool: true xóa thành công, false không thành</returns>
        /// <author>Châu Đức Toàn</author>
        public static bool XoaFile(string pSavePath, string pFileName)
        {
            FileInfo fi = new FileInfo(pSavePath + "\\" + pFileName);
            if (fi.Exists)
            {
                try
                {
                    fi.Delete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region Toan

        /// <summary>
        /// Chuyển ký tự Unicode thành dấu ?
        /// </summary>
        /// <param name="unicodeString">Chuổi có dấu</param>
        /// <returns>Chuổi không dấu</returns>
        /// <author>CDTOAN</author>
        public static string UnicodeToAscii(string unicodeString)
        {
            // Create two different encodings.
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            //Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            // Convert the byte[] into a string
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            return (asciiString);
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt
        /// </summary>
        /// <param name="s">Chuổi có dấu</param>
        /// <returns>Chuổi không dấu</returns>
        /// <author>CDToan</author>
        public static string BoDauTiengViet(string s)
        {
            char[] arr = s.ToCharArray();

            char[] codau = { 'ầ', 'à', 'á', 'ạ', 'ả', 'ã', 'â', 'ă', 'ấ', 'ậ', 'ẩ', 'ẫ', 'ă', 'ằ', 'ắ', 'ặ', 'ẳ', 'ẵ', 'è', 'é', 'ẹ', 'ẻ', 'ẽ', 'ê', 'ề', 'ế', 'ệ', 'ể', 'ễ', 'ì', 'í', 'ị', 'ỉ', 'ĩ', 'ò', 'ó', 'ọ', 'ỏ', 'õ', 'ô', 'ồ', 'ố', 'ộ', 'ổ', 'ỗ', 'ơ', 'ờ', 'ớ', 'ợ', 'ở', 'ỡ', 'ù', 'ú', 'ụ', 'ủ', 'ũ', 'ư', 'ừ', 'ứ', 'ự', 'ử', 'ữ', 'ỳ', 'ý', 'ỵ', 'ỷ', 'ỹ', 'đ', 'À', 'Á', 'Ạ', 'Ả', 'Ã', 'Â', 'Ầ', 'Ấ', 'Ậ', 'Ẩ', 'Ẫ', 'Ă', 'Ằ', 'Ắ', 'Ặ', 'Ẳ', 'Ẵ', 'È', 'É', 'Ẹ', 'Ẻ', 'Ẽ', 'Ê', 'Ề', 'Ế', 'Ệ', 'Ể', 'Ễ', 'Ì', 'Í', 'Ị', 'Ỉ', 'Ĩ', 'Ò', 'Ó', 'Ọ', 'Ỏ', 'Õ', 'Ô', 'Ồ', 'Ố', 'Ộ', 'Ổ', 'Ỗ', 'Ơ', 'Ờ', 'Ớ', 'Ợ', 'Ở', 'Ỡ', 'Ù', 'Ú', 'Ụ', 'Ủ', 'Ũ', 'Ư', 'Ừ', 'Ứ', 'Ự', 'Ử', 'Ữ', 'Ỳ', 'Ý', 'Ỵ', 'Ỷ', 'Ỹ', 'Đ' };

            char[] khongdau = { 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'i', 'i', 'i', 'i', 'i', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'u', 'y', 'y', 'y', 'y', 'y', 'd', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'I', 'I', 'I', 'I', 'I', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'U', 'Y', 'Y', 'Y', 'Y', 'Y', 'D' };

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < codau.Length; j++)
                {
                    if (arr[i] == codau[j])
                    {
                        arr[i] = khongdau[j];
                        break;
                    }
                }
            }



            string str = new string(arr);
            return str;
        }

        public static string secureString(string pStr)
        {
            string s = pStr.Trim();
            string p = @"'+";
            s = Regex.Replace(s, p, "");

            return s;
        }

        public static void ChenTieuDePhanTrangs(object sender, DataGridItemEventArgs e, int pRows)
        {
            StringBuilder vHLTM = new StringBuilder();
            if (e.Item.ItemType == ListItemType.Pager)
            {
                TableCell Pager = (TableCell)e.Item.Controls[0];

                Label lblPagerText = new Label();
                lblPagerText.ID = "lblPagerText";
                if (pRows == 0)
                {
                    vHLTM.Append("<table bgcolor='white' width='100%'><tr><td align='center' style='color:Red'>" + ClassParameter.vKhongTinThayDuLieu + "</td></tr></table>");
                }
                lblPagerText.Text = vHLTM.ToString() + "Trang: ";
                Pager.Controls.AddAt(0, lblPagerText);

                for (int i = 1; i <= Pager.Controls.Count; i += 2)
                {
                    object pgNumbers = Pager.Controls[i];
                    int endPagingIndex = Pager.Controls.Count - 1;

                    if (pgNumbers.GetType().Name == "DataGridLinkButton")
                    {
                        LinkButton lb = (LinkButton)pgNumbers;
                        if (lb.Text == "...")
                        {
                            if (i == 1)
                            {
                                lb.Text = "Trước";
                            }
                            else if (i == endPagingIndex)
                            {
                                lb.Text = "Tiếp";
                            }
                        }
                    }
                }
            }

            #region Doi mau tung dong khi mouse on
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.SelectedItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='beige';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
            }
            #endregion
        }
        #endregion

        public static void HandleEndRequest(UpdatePanel upd, string sc)
        {
            ScriptManager.RegisterClientScriptBlock(upd, upd.GetType(), Guid.NewGuid().ToString() + GetRandom(), sc, true);
            upd.Update();
        }

        public static void HandleEndRequestNoUpdate(UpdatePanel upd, string sc)
        {
            ScriptManager.RegisterClientScriptBlock(upd, upd.GetType(), Guid.NewGuid().ToString() + GetRandom(), sc, true);
        }

        public static string GetRandom()
        {
            int n = DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
            return n + "" + (new Random(n)).Next(10000);
        }

        public static void ThongBaoNgoaiLe(Label pLblThongBao, Exception ex, UserInfo user)
        {
            if (user.IsSuperUser == true)
            {
                pLblThongBao.Text = ex.ToString();
            }
            else
            {
                pLblThongBao.Text = "Lỗi trong quá trình xử lý. Vui lòng liên hệ quản trị.";
            }
            return;
        }
        /// <summary>
        /// thông báo ngoại lệ
        /// </summary>
        /// <param name="ppnThongBao">Panel thông báo</param>
        /// <param name="pLblThongBao">label hiển thị</param>
        /// <param name="ex">Ngoại lệ</param>
        /// <param name="user">UserInfo</param>
        public static void ThongBaoNgoaiLe(Panel ppnThongBao, Label pLblThongBao, Exception ex, UserInfo user)
        {
            if (user.IsSuperUser == true && ex != null)
            {
                ppnThongBao.Visible = true;
                pLblThongBao.Text = ex.ToString();
            }
            else
            {
                ppnThongBao.Visible = true;
                pLblThongBao.Text = "Lỗi trong quá trình xử lý. Vui lòng liên hệ quản trị.";
            }
            return;
        }

        /// <summary>
        /// thông báo  ngoại lệ
        /// </summary>
        /// <param name="ppnThongBao">Panel thông báo</param>
        /// <param name="pLblThongBao">label hiển thị</param>
        /// <param name="user">UserInfo</param>
        public static void ThongBaoNgoaiLe(Panel ppnThongBao, Label pLblThongBao, string pMessage)
        {
            ppnThongBao.Visible = true;
            pLblThongBao.Text = pMessage;

            return;
        }

        /// <summary>
        /// Get description của giá trị enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute =
                (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            if (attribute != null)
            {
                return attribute.Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetGuid()
        {
            try
            {
                return Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                ex.Data["Error"] = string.Empty;
                return string.Empty;
            }
        }

        public static string DocSo(decimal number)
        {
            string str = "";
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;

            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            str = str.Trim();
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Kiểm tra email đúng định dạng
        /// </summary>
        /// <param name="pEmail"></param>
        /// <returns>true: email đúng định dạng; false: Email sai định dạng</returns>
        public static bool KiemTraDinhDangEmail(string pEmail)
        {
            long n;
            bool result = false;
            //bool flag = false;           
            bool IsNumeric = long.TryParse(pEmail, out n);
            string str = @"\b[a-zA-Z0-9._%\-+']+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,4}\b";
            Regex re = new Regex(str);
            if (re.IsMatch(pEmail))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            if (pEmail == "") result = true;
            return result;
        }
        /// <summary>
        /// Xoa javascript
        /// </summary>
        /// <param name="pChuoi"></param>
        /// <returns></returns>
        public static string RemoveJavascript(string pChuoi)
        {
            string vChuoi = "";
            vChuoi = Regex.Replace(pChuoi, "<script", "&lt;script");
            vChuoi = Regex.Replace(vChuoi, "</script>", "&lt;/script&gt;");
            return vChuoi;
        }

        /// <summary>        
        /// Kiểm tra, set giá trị phân trang sau khi load danh sách
        /// Nguyễn Hoàng Tấn Tài
        /// </summary>
        /// <param name = "v_StartEnd" >Giá trị Textbox nhập Start-End</ param >
        /// <param name = "TotalRecord" >Giá trị Textbox tổng số lượng dòng</ param >
        /// <param name = "v_PageSize" >v_PageSize giá trị Page Size mặt định trên trang </ param >
        /// <param name = "o_PageSize" >o_PageSize giá trị Page Size được trả về</ param >
        /// <param name = "start" >Giá trị bắt đầu được trả về</ param >
        /// <param name = "end" >Giá trị kết thúc được trả về</ param >
        /// <param name = "valueStartEnd" >Giá trị xác định StartEnd được trả về cho textbox</ param >
        /// <param name = "Previous" >Is Previous Click</ param >
        /// <param name = "Last" >Is Last Click</ param >
        /// <returns></returns>
        public static bool checkEnterStartEnd(string v_StartEnd, string TotalRecord, int v_PageSize, out int o_PageSize, out int start, out int end, out string valueStartEnd, bool Previous, bool Last)
        {
            try
            {
                int numberTotalRecord = 0;
                bool successTotalRecord = Int32.TryParse(TotalRecord, out numberTotalRecord);



                if (v_StartEnd.Contains("-"))
                {
                    string[] arrStartEnd = v_StartEnd.Split('-');
                    if (arrStartEnd.Length == 2)
                    {
                        int numberStart;
                        bool successnumberStart = Int32.TryParse(arrStartEnd[0], out numberStart);
                        if (successnumberStart)
                        {
                            if (numberStart == 0)
                            {
                                numberStart = 1;
                            }
                        }
                        else
                        {
                            numberStart = 1;
                        }
                        int numberEnd;
                        bool successnumberEnd = Int32.TryParse(arrStartEnd[1], out numberEnd);
                        if (successnumberEnd)
                        {
                            if (numberEnd == 0)
                            {
                                numberEnd = 1;
                            }
                        }
                        else
                        {
                            numberEnd = 1;
                        }


                        //if (numberStart == 1 && numberEnd == 1)
                        //{
                        //    start = numberStart;
                        //    end = numberStart;
                        //    valueStartEnd = numberStart.ToString();
                        //    o_PageSize = v_PageSize;
                        //    return true;
                        //}

                        if (numberStart >= numberEnd)
                        {
                            start = numberStart;
                            end = numberStart;
                            //valueStartEnd = numberStart.ToString();
                            o_PageSize = 1;
                        }
                        else
                        {
                            start = numberStart;
                            end = numberEnd;
                            //valueStartEnd = start.ToString() + "-" + end.ToString();
                            o_PageSize = (numberEnd - numberStart) + 1;
                        }
                    }
                    else if (arrStartEnd.Length == 1)
                    {
                        int number;
                        bool success = Int32.TryParse(arrStartEnd[0], out number);
                        if (success)
                        {
                            if (number == 0)
                            {
                                start = 1;
                                end = v_PageSize;
                                //valueStartEnd = "1-" + v_PageSize;
                                o_PageSize = v_PageSize;
                            }
                            else
                            {
                                start = number;
                                end = number;
                                // valueStartEnd = number.ToString();
                                o_PageSize = 1;
                            }
                        }
                        else
                        {
                            start = 1;
                            end = v_PageSize;
                            //valueStartEnd = "1-" + v_PageSize;
                            o_PageSize = v_PageSize;
                        }
                    }
                    else
                    {
                        start = 1;
                        end = v_PageSize;
                        //valueStartEnd = "1-" + v_PageSize;
                        o_PageSize = v_PageSize;
                    }
                }
                else
                {
                    int number;
                    bool success = Int32.TryParse(v_StartEnd, out number);
                    if (success)
                    {
                        if (number == 0)
                        {
                            start = 1;
                            end = 1;
                            //valueStartEnd = "1";
                            o_PageSize = 1;
                        }
                        else
                        {
                            start = number;
                            end = number;
                            //valueStartEnd = number.ToString();
                            o_PageSize = 1;
                        }
                    }
                    else
                    {
                        start = 1;
                        end = 1;
                        //valueStartEnd = "1";
                        o_PageSize = 1;
                    }
                }
                int newstart = 0;
                int newend = 0;
                //ex 2-3 -> 1
                if (Previous == true)
                {
                    if (end != start)
                    {
                        if ((start - (end - start) - 1) <= 0) //2-(3-2) = 1
                        {
                            newstart = 1;
                            newend = start - 1;
                        }
                        else
                        {
                            // Code củ **********************************************************
                            //newstart = (start - (end - start)-1); //start = 2 - (3 - 2) = 1
                            //newend = start - 1; // 2-1

                            // **************** Khanh *******************************************
                            if (end == Convert.ToInt32(TotalRecord))
                            {
                                newstart = (start - v_PageSize);
                                newend = start - 1;
                            }
                            else
                            {
                                newstart = (start - (end - start) - 1);
                                newend = start - 1;
                            }

                        }
                    }
                    else
                    {

                        // Code củ ********************************************
                        //if ((start - 1) <= 0)
                        //{
                        //    newstart = 1;
                        //    newend = 1;
                        //}
                        //else
                        //{
                        //    newstart = start - 1;
                        //    newend = start-1;
                        //}

                        // **************** Khanh ***********************
                        if (end == Convert.ToInt32(TotalRecord))
                        {
                            newstart = (start - v_PageSize);
                            newend = start - 1;
                        }
                        else
                        {
                            if ((start - 1) <= 0)
                            {
                                newstart = 1;
                                newend = 1;
                            }
                            else
                            {
                                newstart = start - 1;
                                newend = start - 1;
                            }
                        }
                    }
                    end = newend;
                    start = newstart;
                }

                //ex  1-2 -> 3
                if (Last == true)
                {

                    if (end != start)
                    {
                        if ((end + (end - start)) >= numberTotalRecord) //2+(2-1) = 3
                        {
                            newend = numberTotalRecord;
                            newstart = end + 1;
                        }
                        else
                        {
                            newstart = end + 1;
                            newend = (end + (end - start)) + 1;

                        }
                    }
                    else
                    {
                        newstart = start + 1;
                        newend = end + 1;
                    }
                    //else
                    //{
                    //    if ((start - 1) <= 0)
                    //    {
                    //        start = 1;
                    //        end = 1;
                    //    }
                    //    else
                    //    {
                    //        start = start - 1;
                    //        end = start;
                    //    }
                    //}


                    end = newend;
                    start = newstart;
                }
                if (start == end)
                {
                    valueStartEnd = start + "";
                }
                else
                {
                    valueStartEnd = start + "-" + end;
                }
                return true;

            }
            catch (Exception Ex)
            {
                start = 1;
                end = 1;
                valueStartEnd = "1";
                o_PageSize = 1;
                return false;
            }

        }
        /// <summary>        
        /// Kiểm tra, set giá trị phân trang sau khi load danh sách
        /// Nguyễn Hoàng Tấn Tài
        /// </summary>
        /// <param name = "p_txtRecordStartEnd" >Textbox nhập Start-End</ param >
        /// <param name = "p_LinkButtonPrevious" >LinkButton Previous</ param >
        /// <param name = "p_LinkButtonLast" >LinkButton Last</ param >
        /// <param name = "p_TotalCount" >Tổng số records</ param >
        /// <param name = "p_Start" >Giá trị bắt đầu</ param >
        /// <param name = "p_End" >Giá trị kết thúc</ param >
        /// <returns>False:giá trị bắt đầu, kết thúc vượt số lượng records; True: giá trị bắt đầu, kết thúc trong số lượng records</returns>
        public static bool CheckAfterLoad_DataGrird(TextBox p_txtRecordStartEnd, LinkButton p_LinkButtonPrevious, LinkButton p_LinkButtonLast, int p_TotalCount, int p_Start, int p_End)
        {

            //if (p_txtRecordStartEnd.Text == "")
            //{
            //    p_txtRecordStartEnd.Text = "" + p_Start + "-" + (p_TotalCount < p_End ? p_TotalCount : p_End);
            //}
            // Khánh 
            if (p_End > p_TotalCount)
            {
                // set mặc định
                p_Start = 1;
                p_End = 10;
            }
            //End

            p_txtRecordStartEnd.Text = "" + p_Start + "-" + (p_TotalCount < p_End ? p_TotalCount : p_End);
            if (p_Start == 1)
            {
                p_LinkButtonPrevious.Enabled = false;
            }
            else
            {
                p_LinkButtonPrevious.Enabled = true;
            }

            if (p_End >= p_TotalCount)
            {
                p_LinkButtonLast.Enabled = false;
            }
            else
            {
                p_LinkButtonLast.Enabled = true;
            }

            if (p_Start > p_TotalCount && p_End > p_TotalCount)
            {
                p_txtRecordStartEnd.Text = p_TotalCount.ToString();
                return false;
            }
            else
            {
                return true;
            }
        }


        // Convert dataTable to json
        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lay ngay gio upload file
        /// </summary>
        /// <returns>string chuoi ngày gio</returns>
        /// <author>Châu Đức Toàn</author>
        public static string GetUploadDateTime()
        {
            string vNam = DateTime.Now.Year.ToString();
            string vThang = DateTime.Now.Month.ToString();
            string vNgay = DateTime.Now.Day.ToString();
            string vGio = DateTime.Now.Hour.ToString();
            string vPhut = DateTime.Now.Minute.ToString();
            string vGiay = DateTime.Now.Second.ToString();
            return vNam + vThang + vNgay + vGio + vPhut + vGiay;
        }
    }
    
}