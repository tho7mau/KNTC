using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_DONTHU_THANG : DotNetNuke.Entities.Modules.UserModuleBase
    {
        KNTCDataContext vDC = new KNTCDataContext();
        UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
        BaoCaoController baoCaoController = new BaoCaoController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadNam();
                LoadDonVi("-1");
            }
        }
        public void LoadNam()
        {
            int StartYear = vDC.DONTHUs.OrderBy(x => x.NGAYTAO).First().NGAYTAO.Value.Year;
            int EndYear =DateTime.Now.Year;
           
            int row = 0;
            for (int i = EndYear; i >= StartYear; i--)
            {
                ddlistNam.Items.Insert(row, new ListItem(i.ToString(), i.ToString()));
                row++;
            }
        }

        public void LoadDonVi(string vND)
        {
           List<DONVI>  objDonVi = new List<DONVI>();          
            if (vND == "2")
            {
                objDonVi = vDC.DONVIs.ToList();               
            }
            ddlistDonVi.Items.Clear();
            ddlistDonVi.DataSource = objDonVi;
            ddlistDonVi.DataValueField = "DONVI_ID";
            ddlistDonVi.DataTextField = "TENDONVI";
            ddlistDonVi.DataBind();
            ddlistDonVi.Items.Insert(0, new ListItem("Tất cả đơn vị", "0"));
        }

        protected void btnXuatExel_Click(object sender, EventArgs e)
        {          
            if (Page.IsValid)
            {
                try
                {
                    Byte[] fileBytes = baoCaoController.TK_DONTHU_THANG(ddlistNam.SelectedValue, ddlistNguonDon.SelectedValue, ddlistDonVi.SelectedValue);
                    if (fileBytes != null)
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=" + "TK_DONTHU_THANG_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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
                catch (Exception ex)
                {
                    ClassCommon.THONGBAO_TOASTR(Page, ex, _currentUser, "Có lỗi trong quá trình xử lý, Vui lòng liên hệ với quản trị!", "Thông báo lỗi", "error");
                }
            }
        }

        protected void ddlistNguonDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDonVi(ddlistNguonDon.SelectedValue);
        }
    }
}
