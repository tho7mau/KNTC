#region Thông tin chung
/// Mục đích        :Thêm mới + cập nhật đơn vị
/// Ngày tạo        :03/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Permissions;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using Telerik.Web.UI;

namespace HOPKHONGGIAY
{
    public partial class SoDo : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Properties
        int vPhienHopId;
        int vUserId;
        string vLoai;
        string vToken;

        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        UserInfo _User = new UserInfo();

        public string vJavascriptMask = ClassParameter.vJavascriptMask;
        public string vPathCommonJS = ClassParameter.vPathCommonJS;
        HopKhongGiayDataContext vDC = new HopKhongGiayDataContext();
        string vMacAddress = ClassCommon.GetMacAddress();

        #endregion


        #region Events
        /// <summary>
        /// Event page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["PhienHopId"] != null)
                {
                    vPhienHopId = int.Parse(Request.QueryString["PhienHopId"]);
                }

                if (Request.QueryString["UserId"] != null)
                {
                    vUserId = int.Parse(Request.QueryString["UserId"]);
                }

                if(Request.QueryString["Loai"] != null)
                {
                    vLoai = Request.QueryString["Loai"];
                }

                if (Request.QueryString["Token"] != null)
                {
                    vToken = Request.QueryString["Token"];
                }

                if (!IsPostBack)
                {
                    SetInfo(vPhienHopId, vUserId, vLoai, vToken);
                }
            }
            catch (Exception ex)
            {
                ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
            }
        }

        public void SetInfo(int pPHIENHOPID, int pUserId, string pLoai, string pToken)
        {
            try
            {
                SoDoPhongHopInfo objSoDo = (from a in vDC.PHIENHOP_PHONGHOPs
                                            where a.PHIENHOP_ID == pPHIENHOPID
                                            join phong in vDC.PHONGHOPs on a.PHONGHOP_ID equals phong.PHONGHOP_ID
                                            select new SoDoPhongHopInfo()
                                            {
                                                PHIENHOP_ID = a.PHIENHOP_ID,
                                                PHONGHOP_ID = a.PHONGHOP_ID,
                                                SODO_FILE = a.SODO_FILE,
                                                SODO_TEXT = a.SODO_Text == null ? "" : Server.HtmlDecode(a.SODO_Text).Replace("\n", ""),
                                            }).FirstOrDefault();

                if(objSoDo != null)
                {

                    //Tìm danh sách phiên họp vị trí
                    var objViTris = vDC.PRO_PHIENHOP_VITRI(pPHIENHOPID);

                    string vSource = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\" + objSoDo.SODO_FILE;
                    string vDes = Server.MapPath(ClassParameter.vPathCommonUploadPhongHop) + "\\" + pPHIENHOPID + "_" + pUserId + ".svg";

                    string readText = File.ReadAllText(vSource);
                    //LOAI: 
                    foreach(var it in objViTris)
                    {
                        if (it.MAGHE == null)
                        {
                            continue;
                        }

                        if (it.LOAI == pLoai && it.ID == pUserId)
                        {
                            //Tìm vị trí đầu tiên tìm thấy mã ghế
                            int index = readText.LastIndexOf(it.MAGHE);

                            string vStr = readText.Substring(0, index);

                            int index_X = vStr.LastIndexOf("tspan x=\"");
                            index_X += ("tspan x=\"").Length;
                            string vPosition = "";

                            for (int i = index_X; i < index; i++)
                            {
                                if (vStr[i] == '"')
                                {
                                    break;
                                }
                                else
                                {
                                    vPosition += vStr[i].ToString();
                                }
                            }

                            var vNameArr = it.TEN.Trim().Split(' ');
                            string vText = "<tspan fill='red' dy='0'>" + vNameArr[0] + "</tspan>";

                            for (int i = 1; i < vNameArr.Length; i++)
                            {
                                vText += "<tspan fill='red' x='" + vPosition + "' dy='1.4em' >" + vNameArr[i] + "</tspan>";
                            }

                            readText = readText.Replace(it.MAGHE, vText);
                        }
                        else
                        {
                            //Tìm vị trí đầu tiên tìm thấy mã ghế
                            int index = readText.LastIndexOf(it.MAGHE);

                            string vStr = readText.Substring(0, index);

                            int index_X = vStr.LastIndexOf("tspan x=\"");
                            index_X += ("tspan x=\"").Length;
                            string vPosition = "";

                            for(int i= index_X; i < index; i++)
                            {
                                if(vStr[i] == '"')
                                {
                                    break;   
                                }
                                else
                                {
                                    vPosition += vStr[i].ToString();
                                }
                            }

                            var vNameArr = it.TEN.Trim().Split(' ');
                            string vText = "<tspan dy='0'>" + vNameArr[0] + "</tspan>";

                            for (int i = 1; i < vNameArr.Length; i++)
                            {
                                vText += "<tspan x='" + vPosition + "' dy='1.4em' >" + vNameArr[i] + "</tspan>";
                            }  

                            readText = readText.Replace(it.MAGHE, vText);
                        }

                    }

                    File.WriteAllText(vDes, readText);

                    lblImage.InnerHtml = "<object data=\"" + ClassParameter.vPathCommonUploadPhongHop + "\\" + pPHIENHOPID + "_" + pUserId + ".svg" + "\" type=\"image/svg+xml\" width=\"600\">";
                }
            }
            catch(Exception ex)
            {
                var a = ex;
            }
            

        }

        #endregion

    }
}
