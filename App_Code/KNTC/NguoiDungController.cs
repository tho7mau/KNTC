#region Thông tin chung
/// Mục đích        :Controller người dùng (back-end)
/// Ngày tại        :26/03/2020
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
using System.Web;
using Telerik.Web.UI.AsyncUpload;

/// <summary>
/// Summary description for NguoiDungController
/// </summary>
namespace HOPKHONGGIAY
{
    
    public class NguoiDungController
    {
        //#region Khai báo chung
        //HopKhongGiayDataContext vDataContext = new HopKhongGiayDataContext();
        //RoleController vRoleControllerInfo = new RoleController();
        //#endregion


        ///// <summary>
        ///// Get danh sách người dùng - backend
        ///// </summary>
        ///// <param name="pKeyWord"></param>
        ///// <param name="pDONVI_ID"></param>
        ///// <param name="pCV_ID"></param>
        ///// <returns></returns>
        //public List<NGUOIDUNG> GetDanhSachNguoiDung(string pKeyWord, int pDONVI_ID, int pCV_ID)
        //{
        //    try
        //    {
        //        List<NGUOIDUNG> vNguoiDungInfos = new List<NGUOIDUNG>();
        //        vNguoiDungInfos = (from vNguoiDungInfo in vDataContext.NGUOIDUNGs                                      
        //                           where  (SqlMethods.Like(vNguoiDungInfo.Username, "%" + pKeyWord + "%") ||
        //                           SqlMethods.Like(vNguoiDungInfo.TENNGUOIDUNG, "%" + pKeyWord + "%") ||
        //                                   SqlMethods.Like(vNguoiDungInfo.SODIENTHOAI, "%" + pKeyWord + "%") ||
        //                                   SqlMethods.Like(vNguoiDungInfo.EMAIL, "%" + pKeyWord + "%") ||
        //                                   SqlMethods.Like(vNguoiDungInfo.DONVI.TENDONVI, "%" + pKeyWord + "%") ||
        //                                   SqlMethods.Like(vNguoiDungInfo.CHUCVU.TENCHUCVU, "%" + pKeyWord + "%")) && 
        //                                   (vNguoiDungInfo.CV_ID == pCV_ID || pCV_ID == -1) &&
        //                                   (vNguoiDungInfo.DONVI_ID == pDONVI_ID || pDONVI_ID == -1)
        //                                   && vNguoiDungInfo.LOAI == (int)CommonEnum.LoaiNguoiDung.DaiBieu
        //                           select vNguoiDungInfo).OrderByDescending(x => x.NGUOIDUNG_ID).ToList();

        //        return vNguoiDungInfos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;                
        //    }
        //}

        ///// <summary>
        ///// Get người dùng theo NGUOIDUNG_ID - backend
        ///// </summary>
        ///// <param name="pNGUOIDUNG_ID"></param>
        ///// <returns></returns>
        //public NGUOIDUNG GetNguoiDungTheoID(int pNGUOIDUNG_ID)
        //{
        //    try
        //    {
        //        NGUOIDUNG vNguoiDungInfo = new NGUOIDUNG();
        //        vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNGUOIDUNG_ID).FirstOrDefault();
        //        if (vNguoiDungInfo != null)
        //        {
        //            return vNguoiDungInfo;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        /////  Thêm mới người dùng - backend
        ///// </summary>
        ///// <param name="pNguoiDungInfo"></param>
        ///// <param name="pPortalSetting"></param>
        ///// <param name="pPortalId"></param>
        ///// <param name="pPassWord"></param>
        ///// <param name="oNguoiDungID"></param>
        //public void ThemMoiNguoiDung(NGUOIDUNG pNguoiDungInfo, PortalSettings pPortalSetting, int pPortalId, string pPassWord, out int oNguoiDungID)
        //{
        //    try
        //    {
        //        UserInfo vUserInfo = new UserInfo();
        //        vUserInfo.PortalID = pPortalId;
        //        vUserInfo.IsSuperUser = false;
        //        vUserInfo.FirstName = "";
        //        vUserInfo.LastName = "";
        //        vUserInfo.DisplayName = pNguoiDungInfo.TENNGUOIDUNG;
        //        vUserInfo.Email = pNguoiDungInfo.EMAIL;
        //        vUserInfo.Username = pNguoiDungInfo.Username;
        //        //Nạp giá trị vào objMembership
        //        UserMembership objMembership = new UserMembership();
        //        objMembership.Approved = true;
        //        objMembership.CreatedDate = DateTime.Now;
        //        objMembership.Password = pPassWord;
        //        vUserInfo.Membership = objMembership;

        //        //Thêm user và trả đối tượng user vừa thêm
        //        UserCreateStatus result = UserController.CreateUser(ref vUserInfo);

        //        if (result == UserCreateStatus.Success)
        //        {
        //            RoleInfo vRoleInfo = vRoleControllerInfo.GetRoleByName(pPortalId, "NGUOIDUNG"); //Add người dùng vào role NGUOIDUNG - RoleGroup: HOPKHONGGIAY
        //            RoleController.AddUserRole(vUserInfo, vRoleInfo, pPortalSetting, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //            NGUOIDUNG vNguoiDungInfo = new NGUOIDUNG();
        //            vNguoiDungInfo.Username = vUserInfo.Username;
        //            vNguoiDungInfo.TENNGUOIDUNG = vUserInfo.DisplayName;
        //            vNguoiDungInfo.EMAIL = vUserInfo.Email;
        //            vNguoiDungInfo.SODIENTHOAI = pNguoiDungInfo.SODIENTHOAI;
        //            vNguoiDungInfo.CV_ID = pNguoiDungInfo.CV_ID;        
        //            vNguoiDungInfo.DONVI_ID = pNguoiDungInfo.DONVI_ID;
        //            vNguoiDungInfo.PB_ID = pNguoiDungInfo.PB_ID;
        //            vNguoiDungInfo.UserId = vUserInfo.UserID;
        //            vNguoiDungInfo.TRANGTHAI = true;
        //            vNguoiDungInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.DaiBieu;
        //            vDataContext.NGUOIDUNGs.InsertOnSubmit(vNguoiDungInfo);
        //            vDataContext.SubmitChanges();                  
        //            oNguoiDungID = vDataContext.NGUOIDUNGs.OrderByDescending(x => x.NGUOIDUNG_ID).FirstOrDefault().NGUOIDUNG_ID;
        //        }
        //        else
        //        {
        //            oNguoiDungID = -1;
        //        }
               
        //    }
        //    catch (Exception ex)
        //    {
        //        oNguoiDungID = -1;               
        //    }
        //}


        ///// <summary>
        /////  Thêm mới người dùng - backend
        ///// </summary>
        ///// <param name="pNguoiDungInfo"></param>
        ///// <param name="pPortalSetting"></param>
        ///// <param name="pPortalId"></param>
        ///// <param name="pPassWord"></param>
        ///// <param name="oNguoiDungID"></param>
        //public void ThemMoiNguoiDung(NGUOIDUNG pNguoiDungInfo, PortalSettings pPortalSetting, int pPortalId, string pPassWord, bool pQuyenChuyenVien, out int oNguoiDungID)
        //{
        //    try
        //    {
        //        UserInfo vUserInfo = new UserInfo();
        //        vUserInfo.PortalID = pPortalId;
        //        vUserInfo.IsSuperUser = false;
        //        vUserInfo.FirstName = "";
        //        vUserInfo.LastName = "";
        //        vUserInfo.DisplayName = pNguoiDungInfo.TENNGUOIDUNG;
        //        vUserInfo.Email = pNguoiDungInfo.EMAIL;
        //        vUserInfo.Username = pNguoiDungInfo.Username;
        //        //Nạp giá trị vào objMembership
        //        UserMembership objMembership = new UserMembership();
        //        objMembership.Approved = true;
        //        objMembership.CreatedDate = DateTime.Now;
        //        objMembership.Password = pPassWord;
        //        vUserInfo.Membership = objMembership;

        //        //Thêm user và trả đối tượng user vừa thêm
        //        UserCreateStatus result = UserController.CreateUser(ref vUserInfo);

        //        if (result == UserCreateStatus.Success)
        //        {
        //            RoleInfo vRoleInfo = vRoleControllerInfo.GetRoleByName(pPortalId, "NGUOIDUNG"); //Add người dùng vào role NGUOIDUNG - RoleGroup: HOPKHONGGIAY
        //            RoleController.AddUserRole(vUserInfo, vRoleInfo, pPortalSetting, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //            NGUOIDUNG vNguoiDungInfo = new NGUOIDUNG();

        //            if (pQuyenChuyenVien)
        //            {
        //                RoleInfo vRoleInfo_ChuyenVien = vRoleControllerInfo.GetRoleByName(pPortalId, "CHUYENVIEN"); //Add người dùng vào role CHUYENVIEN - RoleGroup: HOPKHONGGIAY
        //                RoleController.AddUserRole(vUserInfo, vRoleInfo_ChuyenVien, pPortalSetting, RoleStatus.Approved, Null.NullDate, Null.NullDate, false, false);
        //            }
        //            vNguoiDungInfo.Username = vUserInfo.Username;
        //            vNguoiDungInfo.TENNGUOIDUNG = vUserInfo.DisplayName;
        //            vNguoiDungInfo.EMAIL = vUserInfo.Email;
        //            vNguoiDungInfo.SODIENTHOAI = pNguoiDungInfo.SODIENTHOAI;
        //            vNguoiDungInfo.CV_ID = pNguoiDungInfo.CV_ID;
        //            vNguoiDungInfo.DONVI_ID = pNguoiDungInfo.DONVI_ID;
        //            vNguoiDungInfo.PB_ID = pNguoiDungInfo.PB_ID;
        //            vNguoiDungInfo.UserId = vUserInfo.UserID;
        //            vNguoiDungInfo.LOAI = (int)CommonEnum.LoaiNguoiDung.DaiBieu;
        //            vNguoiDungInfo.TRANGTHAI = true;
        //            vDataContext.NGUOIDUNGs.InsertOnSubmit(vNguoiDungInfo);
        //            vDataContext.SubmitChanges();
        //            oNguoiDungID = vDataContext.NGUOIDUNGs.OrderByDescending(x => x.NGUOIDUNG_ID).FirstOrDefault().NGUOIDUNG_ID;
        //        }
        //        else
        //        {
        //            oNguoiDungID = -1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oNguoiDungID = -1;
        //    }
        //}

        ///// <summary>
        ///// Cập nhật thông tin người dùng - backend
        ///// </summary>
        ///// <param name="pNguoiDungID"></param>
        ///// <param name="pNguoiDungInfo"></param>
        //public void CapNhatThongTinNguoiDung(int pNguoiDungID, NGUOIDUNG pNguoiDungInfo)
        //{
        //    try
        //    {
        //        NGUOIDUNG vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDungID).SingleOrDefault();
        //        if(vNguoiDungInfo != null)
        //        {
        //            vNguoiDungInfo.Username = pNguoiDungInfo.Username;
        //            vNguoiDungInfo.TENNGUOIDUNG = pNguoiDungInfo.TENNGUOIDUNG;
        //            vNguoiDungInfo.EMAIL = pNguoiDungInfo.EMAIL;
        //            vNguoiDungInfo.SODIENTHOAI = pNguoiDungInfo.SODIENTHOAI;
        //            vNguoiDungInfo.CV_ID = pNguoiDungInfo.CV_ID;
        //            vNguoiDungInfo.DONVI_ID = pNguoiDungInfo.DONVI_ID;
        //            vDataContext.SubmitChanges();
        //        }                                                         
        //    }
        //    catch (Exception ex)
        //    {              
                
        //    }
        //}


        ///// <summary>
        ///// Vô hiệu hóa người dùng - backend
        ///// </summary>
        ///// <param name="pNguoiDung_ID"></param>
        ///// <param name="pPortalId"></param>
        ///// <param name="oErrorMessage"></param>
        //public void CapNhatTrangThaiNguoiDung(int pNguoiDung_ID, int pPortalId, bool pTrangThai)
        //{
        //    try
        //    {
        //        NGUOIDUNG vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDung_ID).SingleOrDefault();
        //        if (vNguoiDungInfo != null)
        //        {
        //            //Set trạng thái người dùng người dùng
        //            vNguoiDungInfo.TRANGTHAI = pTrangThai;                    
        //            vDataContext.SubmitChanges();
        //            //Set trạng thái User dnn
        //            if(vNguoiDungInfo.UserId != null)
        //            {
        //                var vUserInfo = UserController.GetUserById(pPortalId, vNguoiDungInfo.UserId ?? 0);

        //                if (vUserInfo != null)
        //                {
        //                    if (pTrangThai == true)
        //                    {
        //                        UserController.RestoreUser(ref vUserInfo);
        //                    }
        //                    else
        //                    {
        //                        UserController.DeleteUser(ref vUserInfo, false, false);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
              
        //    }
        //}



        ///// <summary>
        ///// Xóa người dùng - backend
        ///// </summary>
        ///// <param name="pNguoiDung_ID"></param>
        ///// <param name="pPortalId"></param>
        ///// <param name="oErrorMessage"></param>
        //public void XoaNguoiDung(int pNguoiDung_ID, int pPortalId, out string oErrorMessage)
        //{
        //    oErrorMessage = "";
        //    try
        //    {
        //        NGUOIDUNG vNguoiDungInfo = vDataContext.NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDung_ID).SingleOrDefault();
        //        if (vNguoiDungInfo != null)
        //        {
        //            //Xóa người dùng
        //            vDataContext.NGUOIDUNGs.DeleteOnSubmit(vNguoiDungInfo);
        //            vDataContext.SubmitChanges();
        //            //Xóa User dnn
        //            UserInfo vUserInfo = UserController.GetUserById(pPortalId, vNguoiDungInfo.UserId ?? 0);
        //            UserController.DeleteUser(ref vUserInfo, false, false);
        //            UserController.RemoveUser(vUserInfo);                    
        //        }
        //        oErrorMessage = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //    }
        //}




        ///// <summary>
        ///// Kiểm tra thiết bị đang sử dụng - backend
        ///// </summary>
        ///// <param name="pThietBi_ID"></param>
        ///// <returns></returns>
        //public bool KiemTraNguoiDungDaPhatSinhDuLieu(int pNguoiDung_ID)
        //{
        //    try
        //    {
        //        int vCountNguoiDung_PhienHop = vDataContext.PHIENHOP_NGUOIDUNGs.Where(x => x.NGUOIDUNG_ID == pNguoiDung_ID).Count();
        //        int vCountNguoiDung_GhiChu = vDataContext.GHICHUs.Where(x => x.NGUOIDUNG_ID == pNguoiDung_ID).Count();
        //        int vCountNguoiDung_GopY = vDataContext.GOPies.Where(x => x.NGUOIDUNG_ID == pNguoiDung_ID).Count();
              
        //        if (vCountNguoiDung_PhienHop > 0 || vCountNguoiDung_GhiChu > 0 || vCountNguoiDung_GopY > 0)
        //            return true;//Đã được sử dụng
        //        else
        //            return false;//Chưa được sử dụng
        //    }
        //    catch (Exception ex)
        //    {
        //        return true;
        //    }

        //}

        ///// <summary>
        ///// Kiểm tra trùng username - backend
        ///// </summary>
        ///// <param name="pNguoiDungID"></param>
        ///// <param name="pUsername"></param>
        ///// <param name="oErrorMessage"></param>
        ///// <returns></returns>
        //public bool KiemTraTrungUsername( string pUsername, out string oErrorMessage)
        //{
        //    try
        //    {
        //        UserDataContext userDataContext = new UserDataContext();
        //        bool vResult = false;
        //        var vNguoiDungInfos = userDataContext.Users.Where(x=>x.Username== pUsername).ToList();

        //        if (vNguoiDungInfos.Count() > 0)
        //            vResult = true; //trùng
        //        else
        //            vResult =  false;//không trùng
        //        oErrorMessage = "";
        //        return vResult;
              
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrorMessage = ex.Message;
        //        return true;                
        //    }
        //}
       

        public NguoiDungController()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
