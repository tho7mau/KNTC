using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DIAPHUONGController
/// </summary>
/// 
namespace KNTC
{
    public class DIAPHUONGController
    {
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        KNTCDataContext vDC = new KNTCDataContext();
        public DIAPHUONGController()
        {}
        /// <summary>
        /// Lấy danh sách địa phương
        /// </summary>
        public List<DIAPHUONG> DS_DIAPHUONG()
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA != 0
                      orderby p.DP_ID descending
                      select p).ToList();
            return it;
        }

        /// <summary>
        /// Lấy danh sách địa phương theo id
        /// </summary>
        /// <param name="pDP_ID">id địa phương</param>
        public DIAPHUONG GetDIAPHUONG_By_ID(int pDP_ID)
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID == pDP_ID
                      select p).FirstOrDefault();
            return it;
        }

        /// <summary>
        /// Lấy danh sách địa phương cha
        /// </summary>
        public List<DIAPHUONG> DS_DIAPHUONG_CHA()
        {
            int dp_id = _currentUser.Profile.City != "" ? Convert.ToInt32(_currentUser.Profile.City) : 0;
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA == 0
                      orderby p.DP_TEN ascending
                      select p).ToList();
            if (dp_id > 0)
            {
                it = it.Where(x => x.DP_ID == dp_id).ToList();
            }
            return it;
        }

        /// <summary>
        /// Lấy danh sách địa phương con
        /// </summary>
        public List<DIAPHUONG> DS_DIAPHUONG_CON()
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA != 0
                      orderby p.DP_ID ascending
                      select p).ToList();
            return it;
        }
        public DIAPHUONG DIAPHUONG_CON(int DP_ID_CHA)
        {
            var it = (from p in vDC.DIAPHUONGs
                      where  p.DP_ID== DP_ID_CHA
                      orderby p.DP_ID ascending
                      select p).FirstOrDefault();
            return it;
        }

        //Kiem tra id co ton tai hay khong
        public bool kiemTraIdDP(int DP_ID)
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID == DP_ID
                      select p).ToList();
            if (it.Count > 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Kiểm tra tên địa phương khi thêm
        /// </summary>
        /// <param name="name">Tên địa phương</param>
        public bool kiemTraDIAPHUONG_TrungTen(string name)
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_TEN == name
                      select p).ToList();
            if (it.Count > 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Cập nhật địa phương
        /// </summary>
        /// <param name="QLCC_DIAPHUONG">Đơn vị</param>
        public void CAPNHAT_DP(DIAPHUONG obj_DIAPHUONG)
        {
            var DIAPHUONG = (from p in vDC.DIAPHUONGs
                             where p.DP_ID == obj_DIAPHUONG.DP_ID
                             select p).FirstOrDefault();
            DIAPHUONG.DP_TEN = obj_DIAPHUONG.DP_TEN;
            DIAPHUONG.DP_ID_CHA = obj_DIAPHUONG.DP_ID_CHA;
            DIAPHUONG.ID_KHUVUC = obj_DIAPHUONG.ID_KHUVUC;
            DIAPHUONG.TEN_KHUVUC = obj_DIAPHUONG.TEN_KHUVUC;
            DIAPHUONG.CapDo = obj_DIAPHUONG.CapDo;
            DIAPHUONG.INDEX_ID = obj_DIAPHUONG.INDEX_ID;
            vDC.SubmitChanges();
        }

        /// <summary>
        /// Them địa phương
        /// </summary>
        /// <param name="QLCC_DIAPHUONG">Địa phương</param>
        public void THEM_DP(DIAPHUONG obj_DIAPHUONG)
        {
            vDC.DIAPHUONGs.InsertOnSubmit(obj_DIAPHUONG);
            vDC.SubmitChanges();
        }

        /// <summary>
        /// Lấy danh sách địa phương cha thứ 1 với id
        /// </summary>
        /// <param name="ID_CHA">id địa phương cha</param>
        public List<DIAPHUONG> DS_DIAPHUONG_CHA1(int ID_CHA)
        {
            int dp_id = _currentUser.Profile.City != "" ? Convert.ToInt32(_currentUser.Profile.City) : 0;
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA == 0 && p.DP_ID != ID_CHA
                      orderby p.DP_TEN ascending
                      select p).ToList();
            if (dp_id > 0)
            {
                it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID == dp_id
                      orderby p.DP_TEN ascending
                      select p).ToList();
            }
            return it;
        }

        /// <summary>
        /// Lấy danh sách địa phương cha thứ 2 với id
        /// </summary>
        public List<DIAPHUONG> DS_DIAPHUONG_CHA2()
        {
            int dp_id = _currentUser.Profile.City != "" ? Convert.ToInt32(_currentUser.Profile.City) : 0;
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA == 0
                      orderby p.DP_TEN ascending
                      select p).ToList();
            if (dp_id > 0)
            {
                it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID == dp_id
                      orderby p.DP_TEN ascending
                      select p).ToList();
            }
            return it;
        }

        public List<DIAPHUONG> Gets_DiaPhuong_Cha()
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA == 0
                      orderby p.DP_TEN ascending
                      select p).ToList();
            return it;
        }
        public List<DIAPHUONG> Gets_Con_By_DiaPhuong_Cha(int ID)
        {
            var it = (from p in vDC.DIAPHUONGs
                      where p.DP_ID_CHA == ID
                      orderby p.DP_TEN
                      select p).ToList();
            return it;
        }

        public bool IsUsed(int dp_id)
        {
            return false;
        }
        
        public bool HasChild(int dp_id)
        {
            return vDC.DIAPHUONGs.Any(Diaphuong => Diaphuong.DP_ID_CHA == dp_id);
        }
        public void ThemMoiDIAPHUONG(DIAPHUONG pDIAPHUONGInfo, out int oDIAPHUONGID, out string oErrorMessage)
        {
            try
            {
                vDC.DIAPHUONGs.InsertOnSubmit(pDIAPHUONGInfo);
                vDC.SubmitChanges();

                oDIAPHUONGID = pDIAPHUONGInfo.DP_ID;
                oErrorMessage = "";
                var vDIAPHUONGNew = GetDIAPHUONG_By_ID(oDIAPHUONGID);
                if (vDIAPHUONGNew.DP_ID_CHA > 0) //Có loại cấp trên
                {
                    var vDIAPHUONGNew_CapTren = GetDIAPHUONG_By_ID(vDIAPHUONGNew.DP_ID_CHA);
                    if (vDIAPHUONGNew_CapTren != null)
                    {
                        vDIAPHUONGNew.INDEX_ID = vDIAPHUONGNew_CapTren.INDEX_ID + vDIAPHUONGNew.DP_ID + ".";
                    }
                }
                else
                {
                    vDIAPHUONGNew.INDEX_ID = vDIAPHUONGNew.DP_ID.ToString() + ".";
                }
                vDC.SubmitChanges();
            }
            catch (Exception ex)
            {
                oDIAPHUONGID = -1;
                oErrorMessage = ex.Message;
            }
        }

        public void XoaDiaPhuong(int pDIaPhuongId, out int oCountXoa, out string oErrorMessage)
        {
            oCountXoa = 0;
            try
            {
                DIAPHUONG vDiaPhuongInfo = vDC.DIAPHUONGs.Where(x => x.DP_ID == pDIaPhuongId).SingleOrDefault();


                if (vDiaPhuongInfo != null)
                {
                    var objDiaPhuong_Xoa = vDC.DIAPHUONGs.Where(x => x.INDEX_ID.StartsWith(vDiaPhuongInfo.INDEX_ID)).ToList();
                    if (objDiaPhuong_Xoa != null)
                    {
                        var objDiaPhuong_Xoa_Id = objDiaPhuong_Xoa.Select(x => x.DP_ID).ToList();
                        // Kiểm tra địa phương có được sử dụng trong danh mục cá nhân không
                        var objCaNhan = vDC.CANHANs.Where(x =>x.DP_ID !=null && objDiaPhuong_Xoa_Id.Contains((int)x.DP_ID)).ToList();
                        if (objCaNhan.Count == 0) // Địa phương chưa được sử dụng
                        {
                            oCountXoa = objDiaPhuong_Xoa.Count;
                            vDC.DIAPHUONGs.DeleteAllOnSubmit(objDiaPhuong_Xoa);
                            vDC.SubmitChanges();
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

    }
}
