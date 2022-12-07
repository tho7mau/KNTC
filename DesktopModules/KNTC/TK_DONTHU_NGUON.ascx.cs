using DotNetNuke.Entities.Users;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
namespace KNTC
{
    public partial class TK_DONTHU_NGUON : DotNetNuke.Entities.Modules.UserModuleBase
    {       
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        BaoCaoController baoCaoController = new BaoCaoController();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnXuatExel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string vTuNgay = "01/01/1990";
                    string vDenNgay = DateTime.Now.ToString("dd/MM/yyyy");
                    if (!String.IsNullOrEmpty(date_tu.Text))
                    {
                        vTuNgay = date_tu.Text;
                    }
                    if (!String.IsNullOrEmpty(date_den.Text))
                    {
                        vDenNgay = date_den.Text;
                    }

                    if (Convert.ToDateTime(vTuNgay) > Convert.ToDateTime(vDenNgay))
                    {
                        ClassCommon.ShowToastr(Page, "Vui lòng chọn từ ngày lớn hơn đến ngày", "Thông báo lỗi", "error");
                    }
                    else
                    {
                        Byte[] fileBytes = baoCaoController.TK_DONTHU_NGUON(vTuNgay,vDenNgay, ddlistNguonDon.SelectedValue);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_DONTHU_NGUON_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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