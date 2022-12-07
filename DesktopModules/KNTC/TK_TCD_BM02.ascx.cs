using DotNetNuke.Entities.Users;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_TCD_BM02 : DotNetNuke.Entities.Modules.UserModuleBase
    {
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        BaoCaoController baoCaoController = new BaoCaoController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadNam();
            }
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
                        Byte[] fileBytes = baoCaoController.KQ_PHANLOAI_XULY_TIEPDAN(vTuNgay, vDenNgay, lblKyBaoCao.Text);
                        if (fileBytes != null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "TK_BM02_TCD" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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

        #region Kỳ báo cáo
        public void LoadNam()
        {
            int j = 1;
            int maxNam = DateTime.Now.Year;
            int minNam = 2015;
            drpNam.Items.Insert(0, new ListItem("-- Chọn năm -- ", "0"));
            for (var i = maxNam; i >= minNam; i--)
            {
                drpNam.Items.Insert(j, new ListItem("Năm " + i.ToString(), i.ToString()));
                j++;
            }
        }
        public void SetThoiGian()
        {
            string _tuNgay = "";
            string _denNgay = "";
            if (drpNam.SelectedValue !="0" && drpKyBaoCao.SelectedValue !="0")
            {
                if (drpKyBaoCao.SelectedValue == "q1")
                {
                    _tuNgay = "15/12/" + (Convert.ToInt32(drpNam.SelectedValue) - 1);
                    _denNgay = "14/3/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "QÚY I/" + drpNam.SelectedValue;
                }
                else if (drpKyBaoCao.SelectedValue == "q2")
                {
                    _tuNgay = "15/3/" + drpNam.SelectedValue;
                    _denNgay = "14/6/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "QÚY 2/" + drpNam.SelectedValue;
                }
                else if (drpKyBaoCao.SelectedValue == "q3")
                {
                    _tuNgay = "15/6/" + drpNam.SelectedValue;
                    _denNgay = "14/9/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "QÚY 2/" + drpNam.SelectedValue;
                }
                else if (drpKyBaoCao.SelectedValue == "q4")
                {
                    _tuNgay = "15/9/" + drpNam.SelectedValue;
                    _denNgay = "14/12/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "QÚY 4/" + drpNam.SelectedValue;
                }
                else if (drpKyBaoCao.SelectedValue == "t6")
                {
                    _tuNgay = "15/12/" + (Convert.ToInt32(drpNam.SelectedValue) - 1);
                    _denNgay = "14/6/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "06 THÁNG ĐẦU NĂM " + drpNam.SelectedValue;
                }
                else if (drpKyBaoCao.SelectedValue == "t9")
                {
                    _tuNgay = "15/12/" + (Convert.ToInt32(drpNam.SelectedValue) - 1);
                    _denNgay = "14/9/" + drpNam.SelectedValue;
                    lblKyBaoCao.Text = "09 THÁNG ĐẦU NĂM " + drpNam.SelectedValue;
                }
                else
                {
                    if (drpKyBaoCao.SelectedValue == "1")
                    {
                        _tuNgay = "15/12/" + (Convert.ToInt32(drpNam.SelectedValue) - 1);
                        _denNgay = "14/1/" + drpNam.SelectedValue;
                    }
                    else
                    {
                        _tuNgay = "15/" + (Convert.ToInt32(drpKyBaoCao.SelectedValue) - 1) + "/" + drpNam.SelectedValue;
                        _denNgay = "14/" + drpKyBaoCao.SelectedValue + "/" + drpNam.SelectedValue;
                    }
                    lblKyBaoCao.Text ="THÁNG "+ drpKyBaoCao.SelectedValue +"/" + drpNam.SelectedValue;
                }                           
            }
            date_tu.Text = _tuNgay;
            date_den.Text = _denNgay;
        }

        protected void drpKyBaoCao_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetThoiGian();
        }

        protected void drpNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetThoiGian();
        }
        #endregion
    }
}