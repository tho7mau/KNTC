
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KNTC
{
    public partial class TK_SOTIEPDAN : DotNetNuke.Entities.Modules.UserModuleBase
    {
        KNTCDataContext vDC = new KNTCDataContext();      
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
            }          
        }
        protected void ExportToWord()
        {
            //Get the data from database into datatable
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
                string vLoaiDoiTuong = ddlistLoaiDoiTuong.SelectedValue; //vLoaiDoiTuong = 0 tất cả ; 1 cá nhân ; 2 đoàn đông người ; 3 cơ quan 
                string vLoai = ddlistLoai.SelectedValue; // vLoai == 0 tất cả  = 1 không đơn ; vLoai = 2 có đơn
                var objTiepDan = vDC.TIEPDANs.Where(x => (x.TIEPDAN_THOGIAN >= Convert.ToDateTime(vTuNgay).Date && x.TIEPDAN_THOGIAN <= Convert.ToDateTime(vDenNgay).Date) 
                                                                               && (vLoaiDoiTuong =="0" || x.DOITUONG.DOITUONG_LOAI ==Convert.ToInt32(vLoaiDoiTuong))
                                                                               &&( vLoai=="0" || (vLoai =="1" && x.DONTHU_ID == null) || (vLoai=="2" && x.DONTHU_ID !=null )) ).ToList();

                var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                var objHINHTHUCGIAIQUYET = vDC.HINHTHUCGIAIQUYETs.ToList();
                var objDONVI = vDC.DONVIs.ToList();
                var objCANBO = vDC.CANBOs.ToList();
                DataTable dt = new DataTable();
                BaoCaoController baoCaoController = new BaoCaoController();
                dt = baoCaoController.Get_SoTiepDan_DataTable(objTiepDan);


                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.RowDataBound += GridView1_RowDataBound;
                GridView1.DataSource = dt;
                GridView1.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SoTiepDan.doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-word ";
                Response.Write("<html>");
                Response.Write("<head>");
                Response.Write("<META HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=UTF-8'>");
                Response.Write("<meta name=ProgId content=Word.Document>");
                Response.Write("<meta name=Generator content='Microsoft Word 9'>");
                Response.Write("<meta name=Originator content='Microsoft Word 9'>");
                Response.Write("<style>");
                Response.Write("@page Section1 {size:595.45pt 841.7pt; margin:0.5in 0.5in 0.5in 0.5in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
                Response.Write("div.Section1 {page:Section1;}");
                Response.Write("@page Section2 {size:841.7pt 595.45pt;mso-page-orientation:landscape;margin:0.5in 0.5in 0.5in 0.5in;mso-header-margin:.5in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}");
                Response.Write("div.Section2 {page:Section2;}");
                Response.Write("</style>");
                Response.Write("</head>");
                Response.Write("<body>");
                Response.Write("<div class=Section2>");
                
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                string vHeader = "<div style='text-align:center; width:100%'><b >SỔ TIẾP DÂN</b></div><br/>";
                sw.Write(Server.HtmlDecode(vHeader));
                GridView1.AllowPaging = false;
                GridView1.DataBind();
                GridView1.RenderControl(hw);
                Response.Write(sw.ToString());
                Response.Write("</div>");
                Response.Write("</body>");
                Response.Write("</html>");
                Response.Flush();
                Response.End();


                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition",
                //    "attachment;filename=SoTiepDan.doc");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-word ";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //string vHeader = "<div style='text-align:center; width:100%'><b >SỔ TIẾP DÂN</b></div><br/>";
                //sw.Write(Server.HtmlDecode(vHeader));
                //GridView1.RenderControl(hw);
               
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();
            }                   
        }

        private void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\n", "<br />");
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\b", "<b>");
                    row.Cells[i].Text = row.Cells[i].Text.Replace("\a", "</b>");
                }
            }
            
            
        }

        protected void btnXuatExel_Click(object sender, EventArgs e)
        {           
            ExportToWord();
        }      
    }
}
