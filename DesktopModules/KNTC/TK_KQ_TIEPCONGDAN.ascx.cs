using DotNetNuke.Entities.Users;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
namespace KNTC
{
    public partial class TK_KQ_TIEPCONGDAN : DotNetNuke.Entities.Modules.UserModuleBase
    {
        KNTCDataContext vDC = new KNTCDataContext();
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
                        //var objTiepDan = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= Convert.ToDateTime(date_tu.Text).Date && x.TIEPDAN_THOGIAN <= Convert.ToDateTime(date_den.Text).Date).ToList();
                        //if (objTiepDan.Count == 0)
                        //{
                        //    ClassCommon.THONGBAO_TOASTR(Page, null, _currentUser, "Không có dữ liệu xuất thống kê.", "Thông báo", "error");
                        //}
                        //else
                        //{
                        //    var ExistFile = Server.MapPath("DesktopModules/KNTC/bieumau/THKQ_TIEPCONGDAN.xlsx");
                        //    var File = new FileInfo(ExistFile);
                        //    using (ExcelPackage pck = new ExcelPackage(File))
                        //    {
                        //        ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                        //        #region Form 1 "form tĩnh "
                        //        int rowf1 = 13;
                        //        // Đếm số lượng Tiếp dân thường xuyên qua ban tiếp công dân

                        //        var objTD_BTDTX = objTiepDan.Where(x => x.TIEPDAN_BTD == true).ToList();

                        //        // Tiếp dân vụ việc 
                        //        var objTD_BTDTX_VV = objTD_BTDTX.Where(x => x.DOITUONG.DOITUONG_LOAI != 2).ToList();

                        //        //  Tiếp dân thường xuyên Vụ việc lượt
                        //        ws.Cells[rowf1, 2].Value = objTD_BTDTX_VV.Count() ;
                        //        //  Tiếp dân thường xuyên Vụ việc người
                        //        ws.Cells[rowf1, 3].Value = objTD_BTDTX_VV.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Count();
                        //        //  Tiếp dân thường xuyên Vụ việc mới phát sinh
                        //        ws.Cells[rowf1, 4].Value = objTD_BTDTX_VV.Where(x => x.TIEPDAN_CU == null).Count();
                        //        //  Tiếp dân thường xuyên Vụ việc củ
                        //        ws.Cells[rowf1, 5].Value = objTD_BTDTX_VV.Where(x => x.TIEPDAN_CU != null).Count();

                        //        // Tiếp dân thường xuyên đoàn đông người
                        //        var objTD_BTDTX_DDN =  objTD_BTDTX.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                        //        ws.Cells[rowf1, 6].Value = objTD_BTDTX_DDN.Count();

                        //        // Tiếp dân thường xuyên đoàn đông người -> số người
                        //        ws.Cells[rowf1, 7].Value = objTD_BTDTX_DDN.Select(x=>x.DOITUONG.DOITUONG_SONGUOI).Count();

                        //        //Tiếp dân thường xuyên đoàn đông người mới phát sinh
                        //        ws.Cells[rowf1, 8].Value = objTD_BTDTX_DDN.Where(x => x.TIEPDAN_CU == null).Count();

                        //        //Tiếp dân thường xuyên đoàn đông người củ
                        //        ws.Cells[rowf1, 9].Value = objTD_BTDTX_DDN.Where(x => x.TIEPDAN_CU != null).Count();



                        //        // Đếm số lượng Tiếp dân đột xuất của lãnh đạo tỉnh

                        //        var objTD_LDT = objTiepDan.Where(x => x.TIEPDAN_BTD == false).ToList();

                        //        // Tiếp dân vụ việc 
                        //        var objTD_LDT_VV = objTD_LDT.Where(x => x.DOITUONG.DOITUONG_LOAI != 2).ToList();

                        //        //  Tiếp dân thường xuyên Vụ việc lượt
                        //        ws.Cells[rowf1, 10].Value = objTD_LDT_VV.Count();
                        //        //  Tiếp dân thường xuyên Vụ việc người
                        //        ws.Cells[rowf1, 11].Value = objTD_LDT_VV.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Count();
                        //        //  Tiếp dân thường xuyên Vụ việc mới phát sinh
                        //        ws.Cells[rowf1, 12].Value = objTD_LDT_VV.Where(x => x.TIEPDAN_CU == null).Count();
                        //        //  Tiếp dân thường xuyên Vụ việc củ
                        //        ws.Cells[rowf1, 13].Value = objTD_LDT_VV.Where(x => x.TIEPDAN_CU != null).Count();

                        //        // Tiếp dân thường xuyên đoàn đông người
                        //        var objTD_LDT_DDN = objTD_LDT.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                        //        ws.Cells[rowf1, 14].Value = objTD_LDT_DDN.Count();

                        //        // Tiếp dân thường xuyên đoàn đông người -> số người
                        //        ws.Cells[rowf1, 15].Value = objTD_LDT_DDN.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Count();

                        //        //Tiếp dân thường xuyên đoàn đông người mới phát sinh
                        //        ws.Cells[rowf1, 16].Value = objTD_LDT_DDN.Where(x => x.TIEPDAN_CU == null).Count();

                        //        //Tiếp dân thường xuyên đoàn đông người củ
                        //        ws.Cells[rowf1, 17].Value = objTD_LDT_DDN.Where(x => x.TIEPDAN_CU != null).Count();

                        //        #endregion

                        //        #region Form 2
                        //        // Excel form 2

                        //        // Tiếp dân có đơn 
                        //        var objTiepDan_CoDon = objTiepDan.Where(x => x.DONTHU_ID != null).Select(x=>x.DONTHU_ID).ToList();
                        //        var objDONTHU_TIEPDAN = vDC.DONTHUs.Where(x => objTiepDan_CoDon.Contains(x.DONTHU_ID)).Select(x=>x.LOAIDONTHU_ID).ToList();

                        //        //Tiếp dân không có đơn 
                        //        var objTiepDan_KoDon = objTiepDan.Where(x => x.DONTHU_ID == null).ToList();


                        //        // Nội dung tiếp công dân (vụ việc)
                        //        var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                        //        var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                        //        foreach (var it in objLOAIDONTHU_GOC)
                        //        {
                        //            //Loại đơn thư LV2
                        //            var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                        //            c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                        //            // Loại đơn thư LV3
                        //            if (c_LDTLV2 > 0)
                        //            {
                        //                foreach (var it2 in objLOAIDONTHU_LV2)
                        //                {
                        //                    CountRow_lv1 = 0;
                        //                    var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                        //                    c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                        //                    if (c_LDTLV3 > 0)
                        //                    {
                        //                        CountRow_LV2 = countRow_Lv3;
                        //                        foreach (var it3 in objLOAIDONTHU_LV3)
                        //                        {
                        //                            //Loại đơn thư lv3 excel
                        //                            vCount++;
                        //                            countRow_Lv3++;
                        //                            ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                        //                            ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //                            // Đếm số lượng đơn thư Loại đơn lv3

                        //                            ws.Cells[row + 5, countRow_Lv3].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it3.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x=>x == it3.LOAIDONTHU_ID).Count();
                        //                            ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //                        }
                        //                        //Loại đơn thư lv2 exccel
                        //                        ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                        //                        ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                        //                        ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //                        //Loại đơn thư lv1 excel
                        //                        CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                        //                    }
                        //                    else
                        //                    {
                        //                        vCount++;
                        //                        countRow_Lv3++;
                        //                        ws.Cells[row + 2, countRow_Lv3, row + 2, countRow_Lv3].Merge = true;
                        //                        ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                        //                        ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //                        // Đếm số lượng đơn thư Loại đơn lv2
                        //                        ws.Cells[row + 5, countRow_Lv3].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it2.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x => x == it2.LOAIDONTHU_ID).Count();
                        //                        ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                        //                    }
                        //                }

                        //                // Loại đơn thư LV1
                        //                ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Merge = true;
                        //                ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                        //                ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //                countRow = CountRow_lv1 + 1;
                        //            }
                        //            else
                        //            {
                        //                // Loại đơn thư lv1 không có loại con                             
                        //                ws.Cells[row + 1, countRow, row + 3, countRow].Merge = true;
                        //                ws.Cells[row + 1, countRow, row + 3, countRow].Value = it.LOAIDONTHU_TEN;
                        //                ws.Cells[row + 1, countRow, row + 3, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //                // Đếm số lượng đơn thư Loại đơn lv2
                        //                ws.Cells[row + 5, countRow].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x => x == it.LOAIDONTHU_ID).Count();
                        //                ws.Cells[row + 5, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //                countRow++;
                        //                vCount++;
                        //            }



                        //        }
                        //        ws.Cells[row, 1, row, vCount].Merge = true;
                        //        ws.Cells[row, 1].Value = "Nội dung tiếp công dân (Số vụ việc)";
                        //        ws.Cells[row, 1, row, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                        //        // Kết quả tiếp công dân *******************************************

                        //        ws.Cells[row, vCount + 1, row, vCount + 4].Merge = true;
                        //        ws.Cells[row, vCount + 1].Value = "Kết quả tiếp công dân (Số vụ việc)";
                        //        ws.Cells[row, vCount + 1, row, vCount + 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //        //Chưa được giải quyết
                        //        ws.Cells[row + 1, vCount + 1, row + 3, vCount + 1].Merge = true;
                        //        ws.Cells[row + 1, vCount + 1].Value = "Chưa được giải quyết";
                        //        ws.Cells[row + 1, vCount + 1, row + 3, vCount + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        //Đã được giải quyết
                        //        ws.Cells[row + 1, vCount + 2, row + 1, vCount + 4].Merge = true;
                        //        ws.Cells[row + 1, vCount + 2].Value = "Đã được giải quyết";
                        //        ws.Cells[row + 1, vCount + 2, row + 1, vCount + 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        // Chưa có quyết định giải quyết  
                        //        ws.Cells[row + 2, vCount + 2, row + 3, vCount + 2].Merge = true;
                        //        ws.Cells[row + 2, vCount + 2].Value = "Chưa có QD giải quyết";
                        //        ws.Cells[row + 2, vCount + 2, row + 3, vCount + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        // Đã có quyết định giải quyết lần 1,2...cuối cùng  
                        //        ws.Cells[row + 2, vCount + 3, row + 3, vCount + 3].Merge = true;
                        //        ws.Cells[row + 2, vCount + 3].Value = "Chưa có QD giải quyết (lần 1,2,cuối cùng)";
                        //        ws.Cells[row + 2, vCount + 3, row + 3, vCount + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        // Đã có bản án của tòa  
                        //        ws.Cells[row + 2, vCount + 4, row + 3, vCount + 4].Merge = true;
                        //        ws.Cells[row + 2, vCount + 4].Value = "Đã có bản án của tòa";
                        //        ws.Cells[row + 2, vCount + 4, row + 3, vCount + 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        // Ghi chú
                        //        ws.Cells[row, vCount + 5, row + 3, vCount + 5].Merge = true;
                        //        ws.Cells[row, vCount + 5].Value = "Ghi chú";
                        //        ws.Cells[row, vCount + 5, row + 3, vCount + 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //        // Số thứ tự
                        //        for (int i = 1; i < vCount + 6; i++)
                        //        {
                        //            ws.Cells[row + 4, i].Value = i + 16;
                        //            ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //        }

                        //        #endregion
                        //        #region Form 3 vụ đông người

                        //        // Lấy tiếp dân thuộc đối tượng nhóm đông người
                        //        var objTIEPDAN_VDN = objTiepDan.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                        //        string vtext = objTIEPDAN_VDN.Count.ToString() + " lượt " + objTIEPDAN_VDN.Count.ToString() + " vụ với " + objTIEPDAN_VDN.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Count() +" người. Cụ thể:";
                        //        ws.Cells[row + 7, 3].Value = vtext;

                        //        int num = 0;
                        //        foreach (var it in objTIEPDAN_VDN)
                        //        {
                        //            num++;
                        //            //Số thứ tự
                        //            ws.Cells[row + 8 + num, 1].Value = num;
                        //            ws.Cells[row + 8 + num, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //            //Ngày
                        //            ws.Cells[row + 8 + num, 2].Value = Convert.ToDateTime(it.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        //            ws.Cells[row + 8 + num, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //            //Nội dung địa chỉ
                        //            ws.Cells[row + 8 + num, 3, row + 8 + num, 9].Merge = true;
                        //            ws.Cells[row + 8 + num, 3].Value = it.DOITUONG.DOITUONG_DIACHI + ",Nội dung: " + it.TIEPDAN_NOIDUNG;
                        //            ws.Cells[row + 8 + num, 3, row + 8 + num, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //            // Số người đến trực tiếp                             
                        //            ws.Cells[row + 8 + num, 10].Value = it.DOITUONG.DOITUONG_SONGUOIDAIDIEN;
                        //            ws.Cells[row + 8 + num, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //            //Hình thức xử lý
                        //            ws.Cells[row + 8 + num, 11, row + 8 + num, 16].Merge = true;
                        //            ws.Cells[row + 8 + num, 11].Value = it.TIEPDAN_KETQUA;
                        //            ws.Cells[row + 8 + num, 11, row + 8 + num, 16].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //            // Ghi chú
                        //            ws.Cells[row + 8 + num, 17].Value ="Ghi chú";
                        //            ws.Cells[row + 8 + num, 17].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //        }
                        //        #endregion


                        Byte[] fileBytes = baoCaoController.KQ_TIEPCONGDAN(vTuNgay,vDenNgay);
                        if (fileBytes !=null)
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment;filename=" + "THKQ_TIEPCONGDAN_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_"
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
                       
                        //    }
                        //}
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
