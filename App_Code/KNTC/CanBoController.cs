#region Thông tin chung
/// Mục đích        :Controller cán bộ
/// Ngày tại        :08/04/2020
/// Người tạo       :Ngô Hoài Hận
/// Phiên bản       :Release 1.0.0
/// Bản quyền       : LIINK .LTD
#endregion
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for CanBoController
/// </summary>
namespace KNTC
{
    
    public class CanBoController
    {
        #region Khai báo chung
        KNTCDataContext vDataContext = new KNTCDataContext();
        RoleController vRoleControllerInfo = new RoleController();
        #endregion

        /// <summary>
        /// Get cán bộ theo ID
        /// </summary>
        /// <param name="pCanBoId"></param>
        /// <returns></returns>
        public CANBO GetCanBoTheoId(int pCanBoId)
        {
            try
            {
                CANBO vDanTocInfo = new CANBO();
                vDanTocInfo = vDataContext.CANBOs.Where(x => x.CANBO_ID == pCanBoId).FirstOrDefault();
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
        ///  Thêm mới cán bộ - backend
        /// </summary>
        /// <param name="pCanBoInfo"></param>
        /// <param name="pPortalSetting"></param>
        /// <param name="pPortalId"></param>
        /// <param name="pPassWord"></param>
        /// <param name="oCanBoID"></param>
        public void ThemMoiCanBo(CANBO pCanBoInfo, PortalSettings pPortalSetting, int pPortalId, string pPassWord, out int oCanBoID)
        {
            try
            {
                UserInfo vUserInfo = new UserInfo();
                vUserInfo.PortalID = pPortalId;
                vUserInfo.IsSuperUser = false;
                vUserInfo.FirstName = "";
                vUserInfo.LastName = "";
                vUserInfo.DisplayName = pCanBoInfo.CANBO_TEN;
                vUserInfo.Email = pCanBoInfo.CANBO_EMAIL;
                vUserInfo.Username = pCanBoInfo.Username;
                //Nạp giá trị vào objMembership
                UserMembership objMembership = new UserMembership();
                objMembership.Approved = true;
                objMembership.CreatedDate = DateTime.Now;
                objMembership.Password = pPassWord;
                vUserInfo.Membership = objMembership;
                 
                //Thêm user và trả đối tượng user vừa thêm
                UserCreateStatus result = UserController.CreateUser(ref vUserInfo);

                if (result == UserCreateStatus.Success)
                {
                    //RoleInfo vRoleInfo = vRoleControllerInfo.GetRoleByName(pPortalId, "CANBO"); //Add người dùng vào role CANBO
                    //RoleController.AddUserRole(vUserInfo, vRoleInfo, pPortalSetting, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
                    CANBO vCanBoInfo = new CANBO();
                    vCanBoInfo.Username = vUserInfo.Username;
                    vCanBoInfo.CANBO_TEN = vUserInfo.DisplayName;
                    vCanBoInfo.CANBO_EMAIL = vUserInfo.Email;
                    vCanBoInfo.CANBO_SODIENTHOAI = pCanBoInfo.CANBO_SODIENTHOAI;
                    vCanBoInfo.CV_ID = pCanBoInfo.CV_ID;
                    vCanBoInfo.DONVI_ID = pCanBoInfo.DONVI_ID;
                    vCanBoInfo.PB_ID = pCanBoInfo.PB_ID;
                    vCanBoInfo.UserId = vUserInfo.UserID;
                    vCanBoInfo.TRANGTHAI = true;
                    vDataContext.CANBOs.InsertOnSubmit(vCanBoInfo);
                    vDataContext.SubmitChanges();
                    oCanBoID = vDataContext.CANBOs.OrderByDescending(x => x.CANBO_ID).FirstOrDefault().CANBO_ID;
                }
                else
                {
                    oCanBoID = -1;
                }
            }
            catch (Exception ex)
            {
                oCanBoID = -1;
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn vị - backed
        /// </summary>
        /// <param name="pDanTocId"></param>
        /// <param name="pDanTocInfo"></param>
        /// <param name="oErrorMessage"></param>
        public void CapNhatDanToc(int pDanTocId, CANBO pDanTocInfo, out string oErrorMessage)
        {
            try
            {
                var vDanTocInfo = vDataContext.CANBOs.Where(x => x.CANBO_ID == pDanTocId).SingleOrDefault();
                if(vDanTocInfo != null)
                {
                    //vDanTocInfo.CANBO_TEN = pDanTocInfo.CANBO_TEN;
                    //vDanTocInfo.CANBO_MOTA = pDanTocInfo.CANBO_MOTA;
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
        public void XoaCanBo(int pDanTocId, out string oErrorMessage)
        {
            try
            {
                CANBO vDanTocInfo = vDataContext.CANBOs.Where(x => x.CANBO_ID == pDanTocId).SingleOrDefault();
              
                if(vDanTocInfo != null)
                {
                    vDataContext.CANBOs.DeleteOnSubmit(vDanTocInfo);
                    vDataContext.SubmitChanges();
                    UserController objUserDNN = new UserController();
                    var objUserInfo = objUserDNN.GetUser(0, (int)vDanTocInfo.UserId);
                    UserController.DeleteUser(ref objUserInfo, false, false);
                    UserController.RemoveUser(objUserInfo);
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
        //        int vCountDanToc_NguoiDung = vDataContext.NGUOIDUNGs.Where(x => x.CANBO_ID == pDanTocId).Count();
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
        /// Kiểm tra trùng username - backend
        /// </summary>
        /// <param name="pUsername"></param>
        /// <param name="oErrorMessage"></param>
        /// <returns></returns>
        public bool KiemTraTrungUsername(string pUsername, int pUserId, out string oErrorMessage)
        {
            try
            {
                UserDataContext userDataContext = new UserDataContext();
                bool vResult = false;
                var vNguoiDungInfos = userDataContext.Users.Where(x => x.Username == pUsername && x.UserID != pUserId).ToList();

                if (vNguoiDungInfos.Count() > 0)
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
        public CanBoController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
