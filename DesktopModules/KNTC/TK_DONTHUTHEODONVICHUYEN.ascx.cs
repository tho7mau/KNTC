using DotNetNuke.Entities.Users;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_DONTHUTHEODONVICHUYEN : DotNetNuke.Entities.Modules.UserModuleBase
    {
        KNTCDataContext vDC = new KNTCDataContext();
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        BaoCaoController baoCaoController = new BaoCaoController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadDanhSachDonVi();
            }    
        }
        /// <summary>
        /// Load danh sách đơn vị vào dropdown
        /// </summary>
        public void LoadDanhSachDonVi()
        {
            try
            {
                var vDonViInfos = vDC.DONVIs.OrderBy(x => x.TENDONVI).ToList();
                if (vDonViInfos.Count > 0)
                {

                    ddlistCoQuanTiepNhan.Items.Clear();
                    ddlistCoQuanTiepNhan.DataSource = vDonViInfos;
                    ddlistCoQuanTiepNhan.DataTextField = "TENDONVI";
                    ddlistCoQuanTiepNhan.DataValueField = "DONVI_ID";
                    ddlistCoQuanTiepNhan.DataBind();
                    ddlistCoQuanTiepNhan.Items.Insert(0, new ListItem("Tất cả cơ quan", ""));

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnXuatExel_Click(object sender, EventArgs e)
        {         
            if (Page.IsValid)
            {
                try
                {
                    string NhanDonTuNgay = "01/01/1990";
                    string NhanDonDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrEmpty(textNhanDonTuNgay.Text))
                    {
                        NhanDonTuNgay = textNhanDonTuNgay.Text;
                    }
                    if (!string.IsNullOrEmpty(textNhanDonDenNgay.Text))
                    {
                        NhanDonDenNgay = textNhanDonDenNgay.Text;
                    }



                    string DonChuyenTuNgay = "01/01/1990";
                    string DonChuyenDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrEmpty(textDonChuyenTuNgay.Text))
                    {
                        DonChuyenTuNgay = textDonChuyenTuNgay.Text;
                    }
                    if (!string.IsNullOrEmpty(textDonChuyenDenNgay.Text))
                    {
                        DonChuyenDenNgay = textDonChuyenDenNgay.Text;
                    }

                    string HanGiaiQuyetTuNgay = "01/01/1990"; ;
                    string HanGiaiQuyetDenNgay = DateTime.Now.ToString("dd/MM/yyyy");

                    if (!string.IsNullOrEmpty(textHanGiaiQuyetTuNgay.Text))
                    {
                        HanGiaiQuyetTuNgay = textHanGiaiQuyetTuNgay.Text;
                    }
                    if (!string.IsNullOrEmpty(textHanGiaiQuyetDenNgay.Text))
                    {
                        HanGiaiQuyetDenNgay = textHanGiaiQuyetDenNgay.Text;
                    }

                    if (Convert.ToDateTime(NhanDonTuNgay) > Convert.ToDateTime(NhanDonDenNgay))
                    {
                        ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Vui lòng chọn nhận đơn từ ngày lớn hơn nhận đơn đến ngày.", "Thông báo", "warning");
                    }
                    else if (Convert.ToDateTime(DonChuyenTuNgay) > Convert.ToDateTime(DonChuyenDenNgay))
                    {
                        ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Vui lòng chọn chuyển đơn từ ngày lớn hơn chuyển đơn đến ngày.", "Thông báo", "warning");
                    }
                    else if (Convert.ToDateTime(HanGiaiQuyetTuNgay) > Convert.ToDateTime(HanGiaiQuyetDenNgay))
                    {
                        ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Vui lòng chọn hạn giải quyết từ ngày lớn hơn hạn giải quyết đến ngày.", "Thông báo", "warning");
                    }
                    else
                    {
                        Byte[] fileBytes = baoCaoController.TK_DONTHUTHEODONVICHUYEN(NhanDonTuNgay, NhanDonDenNgay, DonChuyenTuNgay, DonChuyenDenNgay, HanGiaiQuyetTuNgay, HanGiaiQuyetDenNgay, ddlistCoQuanTiepNhan.SelectedValue, ddlistTrangThaiGiaiQuyet.SelectedValue);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_DONTHUTHEODONVICHUYEN_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
                                     + DateTime.Now.Year + ".xlsx");
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";
                            StringWriter sw = new StringWriter();
                            Response.BinaryWrite(fileBytes);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Không có dữ liệu xuất thống kê.", "Thông báo", "error");
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, Vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
                }
            }
        }
    }
}
