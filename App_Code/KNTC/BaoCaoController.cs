using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

/// <summary>
/// Summary description for BaoCaoController
/// </summary>

namespace KNTC
{
    public class BaoCaoController
    {
        KNTCDataContext vDC = new KNTCDataContext();
        string vPathFile = "http://192.168.1.29:9971/DesktopModules/KNTC/Upload/HOSO/";

        #region Thống kê kết quả giải quyết khiếu nại tố cáo
        public Byte[] KQGQ_KHIEUNAITOCAO(string pTuNgay, string pDenNgay)
        {
            int row = 10;
            int vCount = 0;
            int c_LDTLV2 = 0;
            int c_LDTLV3 = 0;
            int CountRow_LV2 = 0;
            int CountRow_lv1 = 0;
            int countRow_Lv3 = 7;
            int countRow = 8;

            Byte[] fileBytes = null;

            var objDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(pTuNgay).Date && x.NGAYTAO <= Convert.ToDateTime(pDenNgay).Date).ToList();

            if (objDonThu.Count > 0)
            {
                //var ExistFile = Server.MapPath("DesktopModules/KNTC/bieumau/THKQ_XULYDON_KNTC.xlsx");
                //var ExistFile = ClassParameter.vPathCommon + "/DesktopModules/KNTC/bieumau/THKQ_XULYDON_KNTC.xlsx";
                //var ExistFile = "E:\\ProjectDNN\\KNTC\\3-Coding\\Trunk\\DesktopModules\\KNTC\\bieumau\\THKQ_XULYDON_KNTC.xlsx";
                //var ExistFile = System.Web.Hosting.HostingEnvironment.MapPath(ClassParameter.vPathBieuMau) + "/DesktopModules/KNTC/bieumau/THKQ_XULYDON_KNTC.xlsx";
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "THKQ_XULYDON_KNTC.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    #region Form tĩnh
                    // Đơn thư đối tượng
                    //var objDONTHU_DOITUONG =from D in objDonThu
                    //                         join T in vDC.DOITUONGs
                    //                         on D.d

                    ws.Cells[6, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;


                    //Tổng số đơn
                    ws.Cells[row + 5, 2].Value = objDonThu.Count();

                    //Đơn trong kỳ nhiều người đứng tên
                    ws.Cells[row + 5, 3].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //Đơn trong kỳ một người đứng tên
                    ws.Cells[row + 5, 4].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //Đơn kỳ trước chuyển sang nhiều người đứng tên
                    var objToTalDonThu = vDC.DONTHUs.ToList();

                    ws.Cells[row + 5, 5].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //Đơn kỳ trước chuyển sang một người đứng tên
                    ws.Cells[row + 5, 6].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //Đơn đủ điều kiện xử lý
                    ws.Cells[row + 5, 7].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    #endregion

                    #region Form 2
                    // Excel form 2
                    // Nội dung tiếp công dân (vụ việc)
                    var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                    var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    foreach (var it in objLOAIDONTHU_GOC)
                    {
                        //Loại đơn thư LV2
                        var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                        c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                        // Loại đơn thư LV3
                        if (c_LDTLV2 > 0)
                        {
                            foreach (var it2 in objLOAIDONTHU_LV2)
                            {
                                CountRow_lv1 = 0;
                                var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                                c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                                if (c_LDTLV3 > 0)
                                {
                                    CountRow_LV2 = countRow_Lv3;
                                    //Tổng
                                    if (c_LDTLV3 > 1)
                                    {
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = "Tổng";
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        //Tính số đơn thư
                                        var objLOAIDONTHU_LV3_ID = objLOAIDONTHU_LV3.Select(x => x.LOAIDONTHU_ID).ToList();
                                        ws.Cells[row + 5, countRow_Lv3].Value = objDonThu.Where(x => objLOAIDONTHU_LV3_ID.Contains((int)x.LOAIDONTHU_ID)).Count();
                                        ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        c_LDTLV3++;
                                        // Tính MS                                            
                                        string vTong = "";
                                        for (int i = 1; i < c_LDTLV3; i++)
                                        {
                                            if (i < c_LDTLV3 - 1)
                                            {
                                                vTong += (vCount + 6 + i).ToString() + "+";
                                            }
                                            else
                                            {
                                                vTong += (vCount + 6 + i).ToString();
                                            }

                                        }
                                        ws.Cells[row + 4, countRow_Lv3].Value = (vCount + 6).ToString() + "=" + vTong;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }

                                    foreach (var it3 in objLOAIDONTHU_LV3)
                                    {
                                        //Loại đơn thư lv3 excel
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        //MS
                                        ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        // Đếm số lượng đơn thư Loại đơn lv3

                                        ws.Cells[row + 5, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID).Count();
                                        ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    }
                                    //Loại đơn thư lv2 exccel
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                                    //Loại đơn thư lv1 excel
                                    CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                                }
                                else
                                {
                                    vCount++;
                                    countRow_Lv3++;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Merge = true;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    //MS
                                    ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                    ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                                    // Đếm số lượng đơn thư

                                    ws.Cells[row + 5, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID).Count();
                                    ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    // ######################################################################################################################################################################################
                                    CountRow_lv1 = countRow_Lv3;
                                }
                            }

                            // Loại đơn thư LV1
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Merge = true;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            countRow = CountRow_lv1 + 1;
                        }
                        else
                        {
                            // Loại đơn thư lv1 không có loại con                             
                            ws.Cells[row + 1, countRow, row + 3, countRow].Merge = true;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            vCount++;
                            //MS
                            ws.Cells[row + 4, countRow].Value = vCount + 6;
                            ws.Cells[row + 4, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            // Đếm số lượng đơn thư

                            ws.Cells[row + 5, countRow].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_ID).Count();
                            ws.Cells[row + 5, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            countRow++;
                        }
                    }

                    int vColumn = 7;

                    ws.Cells[row, 8, row, vCount + vColumn].Merge = true;
                    ws.Cells[row, 8].Value = "Theo nội dung";
                    ws.Cells[row, 8, row, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Merge = true;
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Value = "Phân loại đơn khiếu nại tố cáo (số đơn)";
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;



                    //Thẩm quyền giải quyết
                    // 1 : Cơ quan hành chính các cấp
                    // 2 : Cơ quan tư pháp các cấp
                    // 3 : Cơ quan Đảng


                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Thẩm quyền giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                    // Các cơ quan hành chính các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan hành chính các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;

                    // Các cơ quan tư pháp các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan tư pháp các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;

                    // Của cơ quan đảng  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Của cơ quan đảng";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;


                    //Theo trình tự giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Theo trình tự giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Chưa được giải quyết
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Chưa được giải quyết";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;

                    //Đã được giải quyết lần đầu
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết lần đầu";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;

                    //Đã được giải quyết nhiều lần
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết nhiều lần";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;

                    // Đơn khác, phản ánh, kiến nghị, đơn nặc danh
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đơn khác, phản ánh, kiến nghị, đơn nặc danh";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true).Count();
                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh

                    var objHinhThucXuLy = vDC.HUONGXYLies.ToList();
                    foreach (var it in objHinhThucXuLy)
                    {
                        vColumn++;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Value = it.HUONGXYLY_TEN;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_ID == it.HUONGXYLY_ID).Count();
                        ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    }
                    // Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh
                    vColumn++;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Value = "Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh";
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    // Ghi chú

                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Ghi chú";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Số thứ tự
                    //for (int i = vCount; i <= vCount + vColumn; i++)
                    //{
                    //    ws.Cells[row + 4, i].Value = i -1;
                    //    ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}
                    for (int i = vCount + 7; i <= vCount + vColumn; i++)
                    {
                        ws.Cells[row + 4, i].Value = i;
                        ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    fileBytes = pck.GetAsByteArray();
                }
                #endregion
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê kết quả tiếp công dân
        public Byte[] KQ_TIEPCONGDAN(string pTuNgay, string pDenNgay)
        {
            int row = 7;
            int vCount = 0;
            int c_LDTLV2 = 0;
            int c_LDTLV3 = 0;
            int CountRow_LV2 = 0;
            int CountRow_lv1 = 0;
            int countRow_Lv3 = 17;
            int countRow = 18;
            int col = 18;
            Byte[] fileBytes = null;
            var objTiepDan = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= Convert.ToDateTime(pTuNgay).Date && x.TIEPDAN_THOGIAN <= Convert.ToDateTime(pDenNgay).Date).ToList();

            if (objTiepDan.Count > 0)
            {
                //var ExistFile = Server.MapPath("DesktopModules/KNTC/bieumau/THKQ_TIEPCONGDAN.xlsx");
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "THKQ_TIEPCONGDAN.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();


                    ws.Cells[5, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                    #region Form 1 "form tĩnh "
                    int rowf1 = row + 5;
                    // Đếm số lượng Tiếp dân thường xuyên qua ban tiếp công dân

                    var objTD_BTDTX = objTiepDan.Where(x => x.TIEPDAN_BTD == null || x.TIEPDAN_BTD == 0).ToList();

                    // Tiếp dân vụ việc 
                    var objTD_BTDTX_VV = objTD_BTDTX.Where(x => x.DOITUONG.DOITUONG_LOAI != 2).ToList();

                    //  Tiếp dân thường xuyên Vụ việc lượt
                    ws.Cells[rowf1, 2].Value = objTD_BTDTX_VV.Count();
                    //  Tiếp dân thường xuyên Vụ việc người
                    ws.Cells[rowf1, 3].Value = objTD_BTDTX_VV.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();
                    //  Tiếp dân thường xuyên Vụ việc mới phát sinh
                    ws.Cells[rowf1, 4].Value = objTD_BTDTX_VV.Where(x => x.TIEPDAN_CU == null).Count();
                    //  Tiếp dân thường xuyên Vụ việc củ
                    ws.Cells[rowf1, 5].Value = objTD_BTDTX_VV.Where(x => x.TIEPDAN_CU != null).Count();

                    // Tiếp dân thường xuyên đoàn đông người
                    var objTD_BTDTX_DDN = objTD_BTDTX.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                    ws.Cells[rowf1, 6].Value = objTD_BTDTX_DDN.Count();

                    // Tiếp dân thường xuyên đoàn đông người -> số người
                    ws.Cells[rowf1, 7].Value = objTD_BTDTX_DDN.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    //Tiếp dân thường xuyên đoàn đông người mới phát sinh
                    ws.Cells[rowf1, 8].Value = objTD_BTDTX_DDN.Where(x => x.TIEPDAN_CU == null).Count();

                    //Tiếp dân thường xuyên đoàn đông người củ
                    ws.Cells[rowf1, 9].Value = objTD_BTDTX_DDN.Where(x => x.TIEPDAN_CU != null).Count();

                    // Đếm số lượng Tiếp dân đột xuất của lãnh đạo tỉnh

                    var objTD_LDT = objTiepDan.Where(x => x.TIEPDAN_BTD != null && x.TIEPDAN_BTD != 0).ToList();

                    // Tiếp dân vụ việc 
                    var objTD_LDT_VV = objTD_LDT.Where(x => x.DOITUONG.DOITUONG_LOAI != 2).ToList();

                    //  Tiếp dân thường xuyên Vụ việc lượt
                    ws.Cells[rowf1, 10].Value = objTD_LDT_VV.Count();
                    //  Tiếp dân thường xuyên Vụ việc người
                    ws.Cells[rowf1, 11].Value = objTD_LDT_VV.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();
                    //  Tiếp dân thường xuyên Vụ việc mới phát sinh
                    ws.Cells[rowf1, 12].Value = objTD_LDT_VV.Where(x => x.TIEPDAN_CU == null).Count();
                    //  Tiếp dân thường xuyên Vụ việc củ
                    ws.Cells[rowf1, 13].Value = objTD_LDT_VV.Where(x => x.TIEPDAN_CU != null).Count();

                    // Tiếp dân thường xuyên đoàn đông người
                    var objTD_LDT_DDN = objTD_LDT.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                    ws.Cells[rowf1, 14].Value = objTD_LDT_DDN.Count();

                    // Tiếp dân thường xuyên đoàn đông người -> số người
                    ws.Cells[rowf1, 15].Value = objTD_LDT_DDN.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    //Tiếp dân thường xuyên đoàn đông người mới phát sinh
                    ws.Cells[rowf1, 16].Value = objTD_LDT_DDN.Where(x => x.TIEPDAN_CU == null).Count();

                    //Tiếp dân thường xuyên đoàn đông người củ
                    ws.Cells[rowf1, 17].Value = objTD_LDT_DDN.Where(x => x.TIEPDAN_CU != null).Count();

                    #endregion

                    #region Form 2
                    // Excel form 2

                    // Tiếp dân có đơn 
                    var objTiepDan_CoDon = objTiepDan.Where(x => x.DONTHU_ID != null).Select(x => x.DONTHU_ID).ToList();
                    var objDONTHU_TIEPDAN = vDC.DONTHUs.Where(x => objTiepDan_CoDon.Contains(x.DONTHU_ID)).Select(x => x.LOAIDONTHU_ID).ToList();

                    //Tiếp dân không có đơn 
                    var objTiepDan_KoDon = objTiepDan.Where(x => x.DONTHU_ID == null).ToList();


                    // Nội dung tiếp công dân (vụ việc)
                    var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                    var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    foreach (var it in objLOAIDONTHU_GOC)
                    {
                        //Loại đơn thư LV2
                        var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                        c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                        // Loại đơn thư LV3
                        if (c_LDTLV2 > 0)
                        {
                            foreach (var it2 in objLOAIDONTHU_LV2)
                            {
                                CountRow_lv1 = 0;
                                var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                                c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                                if (c_LDTLV3 > 0)
                                {
                                    CountRow_LV2 = countRow_Lv3;
                                    foreach (var it3 in objLOAIDONTHU_LV3)
                                    {
                                        //Loại đơn thư lv3 excel
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        // Đếm số lượng đơn thư Loại đơn lv3

                                        ws.Cells[row + 5, countRow_Lv3].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it3.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x => x == it3.LOAIDONTHU_ID).Count();
                                        ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    //Loại đơn thư lv2 exccel
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    //Loại đơn thư lv1 excel
                                    CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                                }
                                else
                                {

                                    vCount++;
                                    countRow_Lv3++;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Merge = true;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    // Đếm số lượng đơn thư Loại đơn lv2
                                    ws.Cells[row + 5, countRow_Lv3].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it2.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x => x == it2.LOAIDONTHU_ID).Count();
                                    ws.Cells[row + 5, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    // ######################################################################################################################################################################################
                                    CountRow_lv1 = countRow_Lv3;

                                }
                            }

                            // Loại đơn thư LV1
                            //countRow = countRow + 5;

                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Merge = true;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            countRow = CountRow_lv1 + 1;
                        }
                        else
                        {
                            // Loại đơn thư lv1 không có loại con                             
                            ws.Cells[row + 1, countRow, row + 3, countRow].Merge = true;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            // Đếm số lượng đơn thư Loại đơn lv2
                            ws.Cells[row + 5, countRow].Value = objTiepDan_KoDon.Where(x => x.TIEPDAN_LOAI == it.LOAIDONTHU_ID).Count() + objDONTHU_TIEPDAN.Where(x => x == it.LOAIDONTHU_ID).Count();
                            ws.Cells[row + 5, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            countRow++;
                            vCount++;
                        }
                    }

                    ws.Cells[row, countRow - vCount, row, countRow - 1].Merge = true;
                    ws.Cells[row, countRow - vCount, row, countRow - 1].Value = "Nội dung tiếp công dân (Số vụ việc)";
                    ws.Cells[row, countRow - vCount, row, countRow - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                    //// Kết quả tiếp công dân *******************************************
                    ///
                    vCount = col + vCount;

                    ws.Cells[row, vCount, row, vCount + 3].Merge = true;
                    ws.Cells[row, vCount, row, vCount + 3].Value = "Kết quả tiếp công dân (Số vụ việc)";
                    ws.Cells[row, vCount, row, vCount + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Chưa được giải quyết
                    ws.Cells[row + 1, vCount, row + 3, vCount].Merge = true;
                    ws.Cells[row + 1, vCount, row + 3, vCount].Value = "Chưa được giải quyết";
                    ws.Cells[row + 1, vCount, row + 3, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vCount++;
                    //Đã được giải quyết
                    ws.Cells[row + 1, vCount, row + 1, vCount + 2].Merge = true;
                    ws.Cells[row + 1, vCount, row + 1, vCount + 2].Value = "Đã được giải quyết";
                    ws.Cells[row + 1, vCount, row + 1, vCount + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Chưa có quyết định giải quyết  
                    ws.Cells[row + 2, vCount, row + 3, vCount].Merge = true;
                    ws.Cells[row + 2, vCount, row + 3, vCount].Value = "Chưa có QD giải quyết";
                    ws.Cells[row + 2, vCount, row + 3, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vCount++;
                    // Đã có quyết định giải quyết lần 1,2...cuối cùng  

                    ws.Cells[row + 2, vCount, row + 3, vCount].Merge = true;
                    ws.Cells[row + 2, vCount, row + 3, vCount].Value = "Đã có QD giải quyết (lần 1,2,cuối cùng)";
                    ws.Cells[row + 2, vCount, row + 3, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vCount++;
                    // Đã có bản án của tòa  
                    ws.Cells[row + 2, vCount, row + 3, vCount].Merge = true;
                    ws.Cells[row + 2, vCount, row + 3, vCount].Value = "Đã có bản án của tòa";
                    ws.Cells[row + 2, vCount, row + 3, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vCount++;
                    // Ghi chú
                    ws.Cells[row, vCount, row + 3, vCount].Merge = true;
                    ws.Cells[row, vCount, row + 3, vCount].Value = "Ghi chú";
                    ws.Cells[row, vCount, row + 3, vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vCount++;
                    // Số thứ tự
                    for (int i = col; i < vCount; i++)
                    {
                        ws.Cells[row + 4, i].Value = i - 1;
                        ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    #endregion

                    #region Form 3 vụ đông người

                    // Lấy tiếp dân thuộc đối tượng nhóm đông người
                    var objTIEPDAN_VDN = objTiepDan.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();
                    string vtext = objTIEPDAN_VDN.Count.ToString() + " lượt " + objTIEPDAN_VDN.Count.ToString() + " vụ với " + objTIEPDAN_VDN.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum() + " người. Cụ thể:";
                    ws.Cells[row + 7, 3].Value = vtext;

                    int num = 0;
                    foreach (var it in objTIEPDAN_VDN)
                    {
                        num++;
                        //Số thứ tự
                        ws.Cells[row + 8 + num, 1].Value = num;
                        ws.Cells[row + 8 + num, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Ngày
                        ws.Cells[row + 8 + num, 2].Value = Convert.ToDateTime(it.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        ws.Cells[row + 8 + num, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Nội dung địa chỉ
                        ws.Cells[row + 8 + num, 3, row + 8 + num, 9].Merge = true;
                        ws.Cells[row + 8 + num, 3].Value = it.DOITUONG.DOITUONG_DIACHI + ",Nội dung: " + it.TIEPDAN_NOIDUNG;
                        ws.Cells[row + 8 + num, 3, row + 8 + num, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        // Số người đến trực tiếp                             
                        ws.Cells[row + 8 + num, 10].Value = it.DOITUONG.DOITUONG_SONGUOIDAIDIEN;
                        ws.Cells[row + 8 + num, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Hình thức xử lý
                        ws.Cells[row + 8 + num, 11, row + 8 + num, 16].Merge = true;
                        ws.Cells[row + 8 + num, 11].Value = it.TIEPDAN_KETQUA;
                        ws.Cells[row + 8 + num, 11, row + 8 + num, 16].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        // Ghi chú
                        ws.Cells[row + 8 + num, 17].Value = "Ghi chú";
                        ws.Cells[row + 8 + num, 17].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    #endregion
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê đơn thư theo tháng
        public Byte[] TK_DONTHU_THANG(string pNam, string pNguonDon, string pDonViID)
        {
            List<DONTHU> objDonThu = new List<DONTHU>();
            int vThangHT = DateTime.Now.Month;
            int row = 10;
            int vCount = 0;
            int c_LDTLV2 = 0;
            int c_LDTLV3 = 0;
            int CountRow_LV2 = 0;
            int CountRow_lv1 = 0;
            int countRow_Lv3 = 7;
            int countRow = 8;
            Byte[] fileBytes = null;

            // objDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO.Value.Year == Convert.ToInt32(pNam) && (pND_ID == "-1" || x.NGUONDON_LOAI_CHITIET == Convert.ToInt32(pND_ID))).ToList();         
            objDonThu = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Year == Convert.ToInt32(pNam))
                                           && (pNguonDon == "-1" || x.NGUONDON_LOAI_CHITIET == Convert.ToInt32(pNguonDon))
                                           && (pDonViID == "0" || x.NGUONDON_DONVI_ID == Convert.ToInt32(pDonViID))).ToList();


            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_DONTHU_THANG.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells[6, 1].Value = "Số liệu năm " + pNam;

                    #region Form tĩnh                   

                    //Tổng số đơn

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        // Tháng 
                        ws.Cells[row + 4 + i, 1].Value = "Tháng " + i;
                        ws.Cells[row + 4 + i, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                        ws.Cells[row + 4 + i, 2].Value = objDonThu.Where(x => x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn trong kỳ nhiều người đứng tên
                        ws.Cells[row + 4 + i, 3].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn trong kỳ một người đứng tên
                        ws.Cells[row + 4 + i, 4].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn kỳ trước chuyển sang nhiều người đứng tên
                        var objToTalDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO.Value.Year <= Convert.ToInt32(pNam)).ToList();

                        ws.Cells[row + 4 + i, 5].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn kỳ trước chuyển sang một người đứng tên
                        ws.Cells[row + 4 + i, 6].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn đủ điều kiện xử lý
                        ws.Cells[row + 4 + i, 7].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    #endregion

                    #region Form 2
                    // Excel form 2
                    // Nội dung tiếp công dân (vụ việc)
                    var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                    var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    foreach (var it in objLOAIDONTHU_GOC)
                    {
                        //Loại đơn thư LV2
                        var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                        c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                        // Loại đơn thư LV3
                        if (c_LDTLV2 > 0)
                        {
                            foreach (var it2 in objLOAIDONTHU_LV2)
                            {
                                CountRow_lv1 = 0;
                                var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                                c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                                if (c_LDTLV3 > 0)
                                {
                                    CountRow_LV2 = countRow_Lv3;
                                    //Tổng
                                    if (c_LDTLV3 > 1)
                                    {
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = "Tổng";
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        //Tính số đơn thư
                                        var objLOAIDONTHU_LV3_ID = objLOAIDONTHU_LV3.Select(x => x.LOAIDONTHU_ID).ToList();
                                        for (int i = 1; i <= vThangHT; i++)
                                        {
                                            ws.Cells[row + 4 + i, countRow_Lv3].Value = objDonThu.Where(x => (objLOAIDONTHU_LV3_ID.Contains((int)x.LOAIDONTHU_ID)) && x.NGAYTAO.Value.Month == i).Count();
                                            ws.Cells[row + 4 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }

                                        c_LDTLV3++;
                                        // Tính MS                                            
                                        string vTong = "";
                                        for (int i = 1; i < c_LDTLV3; i++)
                                        {
                                            if (i < c_LDTLV3 - 1)
                                            {
                                                vTong += (vCount + 6 + i).ToString() + "+";
                                            }
                                            else
                                            {
                                                vTong += (vCount + 6 + i).ToString();
                                            }
                                        }
                                        ws.Cells[row + 4, countRow_Lv3].Value = (vCount + 6).ToString() + "=" + vTong;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }

                                    foreach (var it3 in objLOAIDONTHU_LV3)
                                    {
                                        //Loại đơn thư lv3 excel
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        //MS
                                        ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        // Đếm số lượng đơn thư Loại đơn lv3
                                        for (int i = 1; i <= vThangHT; i++)
                                        {
                                            ws.Cells[row + 4 + i, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                                            ws.Cells[row + 4 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }
                                    }
                                    //Loại đơn thư lv2 exccel
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                                    //Loại đơn thư lv1 excel
                                    CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                                }
                                else
                                {
                                    vCount++;
                                    countRow_Lv3++;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Merge = true;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    //MS
                                    ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                    ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    // Đếm số lượng đơn thư
                                    for (int i = 1; i <= vThangHT; i++)
                                    {
                                        ws.Cells[row + 4 + i, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                                        ws.Cells[row + 4 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    // ######################################################################################################################################################################################
                                    CountRow_lv1 = countRow_Lv3;
                                }
                            }

                            // Loại đơn thư LV1
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Merge = true;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            countRow = CountRow_lv1 + 1;
                        }
                        else
                        {
                            // Loại đơn thư lv1 không có loại con                             
                            ws.Cells[row + 1, countRow, row + 3, countRow].Merge = true;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            vCount++;
                            //MS
                            ws.Cells[row + 4, countRow].Value = vCount + 6;
                            ws.Cells[row + 4, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            // Đếm số lượng đơn thư
                            for (int i = 1; i <= vThangHT; i++)
                            {
                                ws.Cells[row + 4 + i, countRow].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                                ws.Cells[row + 4 + i, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                            countRow++;

                        }
                    }
                    int vColumn = 7;


                    ws.Cells[row, 8, row, vCount + vColumn].Merge = true;
                    ws.Cells[row, 8].Value = "Theo nội dung";
                    ws.Cells[row, 8, row, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Merge = true;
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Value = "Phân loại đơn khiếu nại tố cáo (số đơn)";
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;



                    //Thẩm quyền giải quyết
                    // 1 : Cơ quan hành chính các cấp
                    // 2 : Cơ quan tư pháp các cấp
                    // 3 : Cơ quan Đảng


                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Thẩm quyền giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                    // Các cơ quan hành chính các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan hành chính các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    // Các cơ quan tư pháp các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan tư pháp các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    // Của cơ quan đảng  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Của cơ quan đảng";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;


                    //Theo trình tự giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Theo trình tự giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Chưa được giải quyết
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Chưa được giải quyết";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0) && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    //Đã được giải quyết lần đầu
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết lần đầu";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    //Đã được giải quyết nhiều lần
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết nhiều lần";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1 && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    // Đơn khác, phản ánh, kiến nghị, đơn nặc danh
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đơn khác, phản ánh, kiến nghị, đơn nặc danh";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true && x.NGAYTAO.Value.Month == i).Count();
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    //Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh

                    var objHinhThucXuLy = vDC.HUONGXYLies.ToList();
                    foreach (var it in objHinhThucXuLy)
                    {
                        vColumn++;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Value = it.HUONGXYLY_TEN;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        for (int i = 1; i <= vThangHT; i++)
                        {
                            ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_ID == it.HUONGXYLY_ID && x.NGAYTAO.Value.Month == i).Count();
                            ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    // Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh
                    vColumn++;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Value = "Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh";
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    // Đơn thuộc thẩm quyền giải quyết 
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Value = "Đơn thuộc thẩm quyền";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Khiếu nại 
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Khiếu nại";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;

                    //Tố cáo
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Tố cáo";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    vColumn++;


                    // Ghi chú
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Ghi chú";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    for (int i = 1; i <= vThangHT; i++)
                    {
                        ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }


                    // Số thứ tự                  
                    for (int i = vCount + 7; i <= vCount + vColumn; i++)
                    {
                        ws.Cells[row + 4, i].Value = i;
                        ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    fileBytes = pck.GetAsByteArray();
                }
                #endregion
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê đơn thư theo nguồn
        public Byte[] TK_DONTHU_NGUON(string pTuNgay, string pDenNgay, string pNguonDon)
        {
            List<DONTHU> objDonThu = new List<DONTHU>();
            int vThangHT = DateTime.Now.Month;
            int row = 10;
            int vCount = 0;
            int c_LDTLV2 = 0;
            int c_LDTLV3 = 0;
            int CountRow_LV2 = 0;
            int CountRow_lv1 = 0;
            int countRow_Lv3 = 7;
            int countRow = 8;
            Byte[] fileBytes = null;

            // objDonThu = vDC.DONTHUs.Where(x => x.NGAYTAO.Value.Year == Convert.ToInt32(pNam) && (pND_ID == "-1" || x.NGUONDON_LOAI_CHITIET == Convert.ToInt32(pND_ID))).ToList();         
            objDonThu = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Date > Convert.ToDateTime(pTuNgay))
                                           && (x.NGAYTAO.Value.Date < Convert.ToDateTime(pDenNgay))
                                           && (pNguonDon == "-1" || x.NGUONDON_LOAI_CHITIET == Convert.ToInt32(pNguonDon))).ToList();

            var objCoQuan = vDC.DONVIs.ToList();
            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_DONTHU_NGUON.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    ws.Cells[6, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                    #region Form tĩnh                   

                    //Tổng số đơn                  
                    for (int i = 0; i <= 2; i++)
                    {
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            int row_nguon = pNguonDon == "-1" ? i : 0;
                            // Nguồn đơn
                            ws.Cells[row + 5 + row_nguon, 1].Value = i == 0 ? "Trực tiếp" : i == 1 ? "Gián tiếp" : "Cơ quan khác chuyển tới";
                            ws.Cells[row + 5 + row_nguon, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[row + 5 + row_nguon, 1].Style.Font.Bold = true;

                            ws.Cells[row + 5 + row_nguon, 2].Value = "222";
                            ws.Cells[row + 5 + row_nguon, 2].Value = objDonThu.Where(x => x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ nhiều người đứng tên
                            ws.Cells[row + 5 + row_nguon, 3].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ một người đứng tên
                            ws.Cells[row + 5 + row_nguon, 4].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn kỳ trước chuyển sang nhiều người đứng tên
                            var objToTalDonThu = vDC.DONTHUs.ToList();

                            ws.Cells[row + 5 + row_nguon, 5].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn kỳ trước chuyển sang một người đứng tên
                            ws.Cells[row + 5 + row_nguon, 6].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn đủ điều kiện xử lý
                            ws.Cells[row + 5 + row_nguon, 7].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    // Theo Cơ quan 
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_cq = pNguonDon == "-1" ? 2 : 0;
                        foreach (var cq in objCoQuan)
                        {
                            row_cq++;
                            // Tháng 
                            ws.Cells[row + 5 + row_cq, 1].Value = cq.TENDONVI;
                            ws.Cells[row + 5 + row_cq, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                            ws.Cells[row + 5 + row_cq, 2].Value = objDonThu.Where(x => x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ nhiều người đứng tên
                            ws.Cells[row + 5 + row_cq, 3].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ một người đứng tên
                            ws.Cells[row + 5 + row_cq, 4].Value = objDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn kỳ trước chuyển sang nhiều người đứng tên
                            var objToTalDonThu = vDC.DONTHUs.ToList();

                            ws.Cells[row + 5 + row_cq, 5].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1 && x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn kỳ trước chuyển sang một người đứng tên
                            ws.Cells[row + 5 + row_cq, 6].Value = objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1 && x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn đủ điều kiện xử lý
                            ws.Cells[row + 5 + row_cq, 7].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false && x.NGUONDON_DONVI_ID == cq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cq, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    #endregion

                    #region Form 2
                    // Excel form 2
                    // Nội dung tiếp công dân (vụ việc)
                    var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                    var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                    foreach (var it in objLOAIDONTHU_GOC)
                    {
                        //Loại đơn thư LV2
                        var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                        c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                        // Loại đơn thư LV3
                        if (c_LDTLV2 > 0)
                        {
                            foreach (var it2 in objLOAIDONTHU_LV2)
                            {
                                CountRow_lv1 = 0;
                                var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                                c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                                if (c_LDTLV3 > 0)
                                {
                                    CountRow_LV2 = countRow_Lv3;
                                    //Tổng
                                    if (c_LDTLV3 > 1)
                                    {
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = "Tổng";
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        //Tính số đơn thư
                                        var objLOAIDONTHU_LV3_ID = objLOAIDONTHU_LV3.Select(x => x.LOAIDONTHU_ID).ToList();

                                        // nguồn đơn loại chi tiết

                                        for (int i = 0; i <= 2; i++)
                                        {
                                            int row_nguon = pNguonDon == "-1" ? i : 0;
                                            if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                                            {
                                                ws.Cells[row + 5 + row_nguon, countRow_Lv3].Value = objDonThu.Where(x => (objLOAIDONTHU_LV3_ID.Contains((int)x.LOAIDONTHU_ID)) && x.NGUONDON_LOAI_CHITIET == i).Count();
                                                ws.Cells[row + 5 + row_nguon, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            }
                                        }
                                        if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                                        {
                                            //nguồn đơn theo đơn vị
                                            int row_tong = pNguonDon == "-1" ? 2 : 0;
                                            foreach (var nd_tong in objCoQuan)
                                            {
                                                row_tong++;
                                                ws.Cells[row + 5 + row_tong, countRow_Lv3].Value = objDonThu.Where(x => (objLOAIDONTHU_LV3_ID.Contains((int)x.LOAIDONTHU_ID)) && x.NGUONDON_DONVI_ID == nd_tong.DONVI_ID).Count();
                                                ws.Cells[row + 5 + row_tong, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            }
                                        }

                                        c_LDTLV3++;
                                        // Tính MS                                            
                                        string vTong = "";
                                        for (int i = 1; i < c_LDTLV3; i++)
                                        {
                                            if (i < c_LDTLV3 - 1)
                                            {
                                                vTong += (vCount + 6 + i).ToString() + "+";
                                            }
                                            else
                                            {
                                                vTong += (vCount + 6 + i).ToString();
                                            }
                                        }
                                        ws.Cells[row + 4, countRow_Lv3].Value = (vCount + 6).ToString() + "=" + vTong;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }

                                    foreach (var it3 in objLOAIDONTHU_LV3)
                                    {
                                        //Loại đơn thư lv3 excel
                                        vCount++;
                                        countRow_Lv3++;
                                        ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                                        ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        //MS
                                        ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                        ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        //// Đếm số lượng đơn thư Loại đơn lv3
                                        //for (int i = 1; i <= vThangHT; i++)
                                        //{
                                        //    ws.Cells[row + 4 + i, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                                        //    ws.Cells[row + 4 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        //}

                                        // nguồn đơn loại chi tiết
                                        for (int i = 0; i <= 2; i++)
                                        {
                                            if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                                            {
                                                int row_nguon = pNguonDon == "-1" ? i : 0;
                                                ws.Cells[row + 5 + row_nguon, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID && x.NGUONDON_LOAI_CHITIET == i).Count();
                                                ws.Cells[row + 5 + row_nguon, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            }
                                        }
                                        //nguồn đơn theo đơn vị
                                        if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                                        {
                                            int row_lv3 = pNguonDon == "-1" ? 2 : 0;
                                            foreach (var nd_lv3 in objCoQuan)
                                            {
                                                row_lv3++;
                                                ws.Cells[row + 5 + row_lv3, countRow_Lv3].Value = objDonThu.Where(x => (x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID) && x.NGUONDON_DONVI_ID == nd_lv3.DONVI_ID).Count();
                                                ws.Cells[row + 5 + row_lv3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            }
                                        }
                                    }
                                    //Loại đơn thư lv2 exccel
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                                    //Loại đơn thư lv1 excel
                                    CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                                }
                                else
                                {
                                    vCount++;
                                    countRow_Lv3++;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Merge = true;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                                    ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    //MS
                                    ws.Cells[row + 4, countRow_Lv3].Value = vCount + 6;
                                    ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    // ######################################################################################################################################################################################
                                    CountRow_lv1 = countRow_Lv3;

                                    //// Đếm số lượng đơn thư
                                    //for (int i = 1; i <= vThangHT; i++)
                                    //{
                                    //    ws.Cells[row + 4 + i, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                                    //    ws.Cells[row + 4 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    //}

                                    // Đếm loại đơn thư lv2
                                    // nguồn đơn loại chi tiết
                                    for (int i = 0; i <= 2; i++)
                                    {
                                        int row_nguon = pNguonDon == "-1" ? i : 0;
                                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                                        {
                                            ws.Cells[row + 5 + row_nguon, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID && x.NGUONDON_LOAI_CHITIET == i).Count();
                                            ws.Cells[row + 5 + row_nguon, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }
                                    }
                                    //nguồn đơn theo đơn vị
                                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                                    {
                                        int row_lv2 = pNguonDon == "-1" ? 2 : 0;
                                        foreach (var nd_lv2 in objCoQuan)
                                        {
                                            row_lv2++;
                                            ws.Cells[row + 5 + row_lv2, countRow_Lv3].Value = objDonThu.Where(x => (x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID) && x.NGUONDON_DONVI_ID == nd_lv2.DONVI_ID).Count();
                                            ws.Cells[row + 5 + row_lv2, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        }
                                    }
                                }
                            }

                            // Loại đơn thư LV1
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Merge = true;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            countRow = CountRow_lv1 + 1;
                        }
                        else
                        {
                            // Loại đơn thư lv1 không có loại con                             
                            ws.Cells[row + 1, countRow, row + 3, countRow].Merge = true;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Value = it.LOAIDONTHU_TEN;
                            ws.Cells[row + 1, countRow, row + 3, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            vCount++;
                            //MS
                            ws.Cells[row + 4, countRow].Value = vCount + 6;
                            ws.Cells[row + 4, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //// Đếm số lượng đơn thư
                            //for (int i = 1; i <= vThangHT; i++)
                            //{
                            //    ws.Cells[row + 4 + i, countRow].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_ID && x.NGAYTAO.Value.Month == i).Count();
                            //    ws.Cells[row + 4 + i, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //}


                            // Đếm loại đơn thư lv1
                            // nguồn đơn loại chi tiết
                            for (int i = 0; i <= 2; i++)
                            {
                                if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                                {
                                    int row_nguon = pNguonDon == "-1" ? i : 0;
                                    ws.Cells[row + 5 + row_nguon, countRow].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_ID && x.NGUONDON_LOAI_CHITIET == i).Count();
                                    ws.Cells[row + 5 + row_nguon, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                }
                            }
                            //nguồn đơn theo đơn vị
                            if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                            {
                                int row_lv1 = pNguonDon == "-1" ? 2 : 0;
                                foreach (var nd_lv1 in objCoQuan)
                                {
                                    row_lv1++;
                                    ws.Cells[row + 5 + row_lv1, countRow].Value = objDonThu.Where(x => (x.LOAIDONTHU_ID == it.LOAIDONTHU_ID) && x.NGUONDON_DONVI_ID == nd_lv1.DONVI_ID).Count();
                                    ws.Cells[row + 5 + row_lv1, countRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                }
                            }
                            countRow++;
                        }
                    }
                    int vColumn = 7;


                    ws.Cells[row, 8, row, vCount + vColumn].Merge = true;
                    ws.Cells[row, 8].Value = "Theo nội dung";
                    ws.Cells[row, 8, row, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Merge = true;
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Value = "Phân loại đơn khiếu nại tố cáo (số đơn)";
                    ws.Cells[row - 1, 8, row - 1, vCount + vColumn + 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    vColumn++;



                    //Thẩm quyền giải quyết
                    // 1 : Cơ quan hành chính các cấp
                    // 2 : Cơ quan tư pháp các cấp
                    // 3 : Cơ quan Đảng


                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //Thẩm quyền giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                    // Các cơ quan hành chính các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan hành chính các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1 && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Cơ quan hành chính các cấp
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_cqhc = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_cqhc in objCoQuan)
                        {
                            row_cqhc++;
                            ws.Cells[row + 5 + row_cqhc, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1) && x.NGUONDON_DONVI_ID == nd_cqhc.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cqhc, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    vColumn++;

                    // Các cơ quan tư pháp các câp  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan tư pháp các cấp";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2 && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Cơ quan tư pháp các cấp
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_cqtp = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_cqtp in objCoQuan)
                        {
                            row_cqtp++;
                            ws.Cells[row + 5 + row_cqtp, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2) && x.NGUONDON_DONVI_ID == nd_cqtp.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cqtp, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    vColumn++;

                    // Của cơ quan đảng  
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Của cơ quan đảng";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3 && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Cơ quan đảng
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3 && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_cqd = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_cqd in objCoQuan)
                        {
                            row_cqd++;
                            ws.Cells[row + 5 + row_cqd, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3) && x.NGUONDON_DONVI_ID == nd_cqd.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cqd, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    //Theo trình tự giải quyết
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Theo trình tự giải quyết";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Chưa được giải quyết
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Chưa được giải quyết";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0) && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Chưa được giải quyết
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_cgq = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_cgq in objCoQuan)
                        {
                            row_cgq++;
                            ws.Cells[row + 5 + row_cgq, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0) && x.NGUONDON_DONVI_ID == nd_cgq.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_cgq, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    //Đã được giải quyết lần đầu
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết lần đầu";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1 && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Đã được giải quyết lần đầu
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == 1) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_l1 = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_l1 in objCoQuan)
                        {
                            row_l1++;
                            ws.Cells[row + 5 + row_l1, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == 1) && x.NGUONDON_DONVI_ID == nd_l1.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_l1, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    //Đã được giải quyết nhiều lần
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết nhiều lần";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1 && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Đã được giải quyết nhiều lần
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN > 1) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_nl = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_nl in objCoQuan)
                        {
                            row_nl++;
                            ws.Cells[row + 5 + row_nl, vCount + vColumn].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN > 1) && x.NGUONDON_DONVI_ID == nd_nl.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_nl, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    // Đơn khác, phản ánh, kiến nghị, đơn nặc danh
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đơn khác, phản ánh, kiến nghị, đơn nặc danh";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true && x.NGAYTAO.Value.Month == i).Count();
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}

                    // Đơn khác, phản ánh, kiến nghị, đơn nặc danh
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => (x.DONTHU_NACDANH == true) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_nd = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_nd in objCoQuan)
                        {
                            row_nd++;
                            ws.Cells[row + 5 + row_nd, vCount + vColumn].Value = objDonThu.Where(x => (x.DONTHU_NACDANH == true) && x.NGUONDON_DONVI_ID == nd_nd.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_nd, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    //Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh

                    var objHinhThucXuLy = vDC.HUONGXYLies.ToList();
                    foreach (var it in objHinhThucXuLy)
                    {
                        vColumn++;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Value = it.HUONGXYLY_TEN;
                        ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //for (int i = 1; i <= vThangHT; i++)
                        //{
                        //    ws.Cells[row + 4 + i, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_ID == it.HUONGXYLY_ID && x.NGAYTAO.Value.Month == i).Count();
                        //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //}

                        // nguồn đơn loại chi tiết
                        for (int i = 0; i <= 2; i++)
                        {
                            int row_nguon = pNguonDon == "-1" ? i : 0;
                            if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                            {
                                ws.Cells[row + 5 + row_nguon, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_LOAI_CHITIET == i).Count();
                                ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                        }
                        //nguồn đơn theo đơn vị
                        if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                        {
                            int row_htxl = pNguonDon == "-1" ? 2 : 0;
                            foreach (var nd_htxl in objCoQuan)
                            {
                                row_htxl++;
                                ws.Cells[row + 5 + row_htxl, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_DONVI_ID == nd_htxl.DONVI_ID).Count();
                                ws.Cells[row + 5 + row_htxl, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                        }
                    }
                    // Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh
                    vColumn++;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Value = "Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh";
                    ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                    // Đơn thuộc thẩm quyền giải quyết 
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Merge = true;
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Value = "Đơn thuộc thẩm quyền";
                    ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Khiếu nại 
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Khiếu nại";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            //ws.Cells[row + 5 + i, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_kn = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_kn in objCoQuan)
                        {
                            row_kn++;
                            //ws.Cells[row + 5 + row_kn, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_DONVI_ID == nd_htxl.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_kn, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    //Tố cáo
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Tố cáo";
                    ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            //ws.Cells[row + 5 + i, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị
                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_tc = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_tc in objCoQuan)
                        {
                            row_tc++;
                            //ws.Cells[row + 5 + row_tc, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_DONVI_ID == nd_tc.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_tc, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    vColumn++;

                    // Ghi chú
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Ghi chú";
                    ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //for (int i = 1; i <= vThangHT; i++)
                    //{
                    //    ws.Cells[row + 4 + i, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //}
                    // nguồn đơn loại chi tiết
                    for (int i = 0; i <= 2; i++)
                    {
                        int row_nguon = pNguonDon == "-1" ? i : 0;
                        if (pNguonDon == "-1" || Convert.ToInt32(pNguonDon) == i)
                        {
                            //ws.Cells[row + 5 + i, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_LOAI_CHITIET == i).Count();
                            ws.Cells[row + 5 + row_nguon, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    //nguồn đơn theo đơn vị

                    if (pNguonDon == "2" || pNguonDon == "-1")// Cơ quan khác chuyển tới
                    {
                        int row_gc = pNguonDon == "-1" ? 2 : 0;
                        foreach (var nd_gc in objCoQuan)
                        {
                            row_gc++;
                            //ws.Cells[row + 5 + row_gc, vCount + vColumn].Value = objDonThu.Where(x => (x.HUONGXULY_ID == it.HUONGXYLY_ID) && x.NGUONDON_DONVI_ID == nd_gc.DONVI_ID).Count();
                            ws.Cells[row + 5 + row_gc, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    // Số thứ tự                  
                    for (int i = vCount + 7; i <= vCount + vColumn; i++)
                    {
                        ws.Cells[row + 4, i].Value = i;
                        ws.Cells[row + 4, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    fileBytes = pck.GetAsByteArray();
                }
                #endregion
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê trùng đơn
        public Byte[] TK_DONTHU_TRUNG(string pTuNgay, string pDenNgay, string pLoaiDoiTuong)
        {
            //List<DONTHU> objDonThu = new List<DONTHU>();
            int row = 10;

            Byte[] fileBytes = null;


            List<long> objDONTHU_GOCID = new List<long>();
            List<DONTHU> objDT = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Date > Convert.ToDateTime(pTuNgay))
                                                                        && (x.NGAYTAO.Value.Date < Convert.ToDateTime(pDenNgay))
                                                                        && (pLoaiDoiTuong == "0" || x.DOITUONG.DOITUONG_LOAI == Convert.ToInt32(pLoaiDoiTuong))
                                                                         ).ToList();

            List<long> DONTHU_REMOVE_ID = new List<long>();
            // Kiểm tra đơn thư trùng 
            foreach (var item in objDT)
            {
                if (DONTHU_REMOVE_ID.Count == 0 || (DONTHU_REMOVE_ID.Count > 0 && !DONTHU_REMOVE_ID.Contains(item.DONTHU_ID)))
                {
                    var objTRung = objDT.Where(x => x.DONTHU_ID != item.DONTHU_ID && x.DONTHU_NOIDUNG == item.DONTHU_NOIDUNG && x.DOITUONG_ID == item.DOITUONG_ID).ToList();
                    if (objTRung.Count > 0)
                    {
                        objDONTHU_GOCID.Add(item.DONTHU_ID);
                        DONTHU_REMOVE_ID.AddRange(objTRung.Select(x => x.DONTHU_ID).ToList());
                        //objDT_TEMP.RemoveAll(x=>objTRung.Contains(x));
                        //objDT_TEMP.AddRange(objTRung);
                    }
                }
            }

            //var objDONTHU_GOCID = vDC.DONTHUs.Where(x => x.DONTHU_CU != null).Select(x => x.DONTHU_CU).ToList();
            if (objDONTHU_GOCID.Count > 0)
            {
                var objDONTHU_GOCID_DISTINCT = objDONTHU_GOCID.Distinct().ToList();
                var objDonThu = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Date > Convert.ToDateTime(pTuNgay))
                                                                       && (x.NGAYTAO.Value.Date < Convert.ToDateTime(pDenNgay))
                                                                       && (pLoaiDoiTuong == "0" || x.DOITUONG.DOITUONG_LOAI == Convert.ToInt32(pLoaiDoiTuong))
                                                                       && (objDONTHU_GOCID_DISTINCT.Contains((int)x.DONTHU_ID))
                                                                        ).ToList();

                if (objDonThu.Count > 0)
                {
                    var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_TRUNGDON.xlsx";
                    var File = new FileInfo(ExistFile);
                    using (ExcelPackage pck = new ExcelPackage(File))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                        ws.Cells[6, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                        foreach (var it in objDonThu)
                        {
                            ws.Cells[row, 1].Value = row - 9;
                            ws.Cells[row, 2].Value = it.DONTHU_STT;
                            ws.Cells[row, 3].Value = getThongTinDoiTuong((int)it.DOITUONG_ID);
                            ws.Cells[row, 4].Value = it.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : it.NGUONDON_LOAI_CHITIET == 1 ? "Gián tiếp" : GetCoQuanChuyenDon(it.NGUONDON_DONVI_ID.ToString());
                            ws.Cells[row, 5].Value = GetTenLoaiDonThuById(it.LOAIDONTHU_ID.ToString());
                            ws.Cells[row, 6].Value = it.DONTHU_NOIDUNG;
                            ws.Cells[row, 7].Value = it.DONTHU_TRANGTHAI == 0 ? "Chưa xử lý" : it.DONTHU_TRANGTHAI == 1 ? "Đã có hướng xử lý" : it.DONTHU_TRANGTHAI == 2 ? "Gửi giải quyết đơn thư" : it.DONTHU_TRANGTHAI == 3 ? "Đã có kết quả giải quyết" : "Đơn thư đã kết thúc";
                            ws.Cells[row, 8].Value = it.HUONGXULY_TEN + "\n" + it.HUONGXULY_YKIEN_XULY;
                            if (it.NGUONDON_NGAYDEDON != null)
                            {
                                ws.Cells[row, 9].Value = Convert.ToDateTime(it.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                            }
                            ws.Cells[row, 10].Value = Convert.ToDateTime(it.NGAYTAO).ToString("dd/MM/yyyy");
                            ws.Cells[row, 11].Value = objDONTHU_GOCID.Where(x => x == it.DONTHU_ID).Count();

                            for (int i = 1; i < 12; i++)
                            {
                                ws.Cells[row, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                            row++;
                        }

                        fileBytes = pck.GetAsByteArray();
                    }
                }
            }

            return fileBytes;
        }
        #endregion

        #region Thống kê đơn thư theo đơn vị chuyển
        public Byte[] TK_DONTHUTHEODONVICHUYEN(string NhanDonTuNgay, string NhanDonDenNgay, string DonChuyenTuNgay, string DonChuyenDenNgay, string HanGiaiQuyetTuNgay, string HanGiaiQuyetDenNgay, string DONVI_IDs_CHUYEN, string GIAIQUYETTRANGTHAI_ID)
        {
            try
            {
                int row = 10;
                int vCount = 0;
                int c_LDTLV2 = 0;
                int c_LDTLV3 = 0;
                int CountRow_LV2 = 0;
                int CountRow_lv1 = 0;
                int countRow_Lv3 = 5;
                int countCol = 6;
                Byte[] fileBytes = null;
                List<int> DONVI_ID_TIEPNHANs = new List<int>();
                DateTime DateNhanDonTuNgay;
                DateTime DateNhanDonDenNgay;
                var objDonThu = vDC.DONTHUs.Where(x => x.HUONGXULY_ID == 3).ToList();
                // Ngày nhận đơn
                if (NhanDonTuNgay != "" && NhanDonDenNgay != "")
                {
                    if (TryParseExact_DateTime(NhanDonTuNgay, out DateNhanDonTuNgay) && TryParseExact_DateTime(NhanDonDenNgay, out DateNhanDonDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateNhanDonTuNgay && x.NGAYTAO <= DateNhanDonDenNgay).ToList();

                    }
                }
                else if (NhanDonTuNgay != "")
                {
                    if (TryParseExact_DateTime(NhanDonTuNgay, out DateNhanDonTuNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateNhanDonTuNgay).ToList();
                    }
                }
                else if (NhanDonDenNgay != "")
                {
                    if (TryParseExact_DateTime(NhanDonDenNgay, out DateNhanDonDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateNhanDonDenNgay).ToList();
                    }

                }

                // Ngày chuyển đơn
                DateTime DateDonChuyenTuNgay;
                DateTime DateDonChuyenDenNgay;
                if (DonChuyenTuNgay != "" && DonChuyenDenNgay != "")
                {
                    if (TryParseExact_DateTime(DonChuyenTuNgay, out DateDonChuyenTuNgay) && TryParseExact_DateTime(DonChuyenDenNgay, out DateDonChuyenDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateDonChuyenTuNgay && x.NGAYTAO <= DateDonChuyenDenNgay).ToList();
                    }
                }
                else if (DonChuyenTuNgay != "")
                {
                    if (TryParseExact_DateTime(DonChuyenTuNgay, out DateDonChuyenTuNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateDonChuyenTuNgay).ToList();
                    }
                }
                else if (DonChuyenDenNgay != "")
                {
                    if (TryParseExact_DateTime(DonChuyenDenNgay, out DateDonChuyenDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateDonChuyenDenNgay).ToList();
                    }
                }

                // Thời hạn giải quyết
                DateTime DateHanGiaiQuyetTuNgay;
                DateTime DateHanGiaiQuyetDenNgay;
                if (HanGiaiQuyetTuNgay != "" && HanGiaiQuyetDenNgay != "")
                {
                    if (TryParseExact_DateTime(HanGiaiQuyetTuNgay, out DateHanGiaiQuyetTuNgay) && TryParseExact_DateTime(HanGiaiQuyetDenNgay, out DateHanGiaiQuyetDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateHanGiaiQuyetTuNgay && x.NGAYTAO <= DateHanGiaiQuyetDenNgay).ToList();
                    }
                }
                else if (HanGiaiQuyetTuNgay != "")
                {
                    if (TryParseExact_DateTime(HanGiaiQuyetTuNgay, out DateHanGiaiQuyetTuNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateHanGiaiQuyetTuNgay).ToList();
                    }
                }
                else if (HanGiaiQuyetDenNgay != "")
                {
                    if (TryParseExact_DateTime(HanGiaiQuyetDenNgay, out DateHanGiaiQuyetDenNgay))
                    {
                        objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateHanGiaiQuyetDenNgay).ToList();
                    }
                }

                if (GIAIQUYETTRANGTHAI_ID != "0")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == false || x.KETQUA_XYLY == null).ToList();
                }
                // Đã có kết quả giải quyết
                else if (GIAIQUYETTRANGTHAI_ID == "1")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == true).ToList();
                }

                if (DONVI_IDs_CHUYEN != "")
                {
                    List<int> DONVI_IDs = new List<int>();
                    List<string> DONVI_IDs_List = DONVI_IDs_CHUYEN.Split(',').ToList();
                    for (int i = 0; i < DONVI_IDs_List.Count(); i++)
                    {
                        DONVI_IDs.Add(Int32.Parse(DONVI_IDs_List[i]));
                    }
                }
                DONVI_ID_TIEPNHANs = objDonThu.Select(x => Int32.Parse(x.HUONGXULY_DONVI_ID.ToString())).Distinct().ToList();
                var DONVIs = vDC.DONVIs.ToList();
                // Chưa có kết quả

                if (objDonThu.Count > 0)
                {
                    var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_CHUYENDON.xlsx";
                    var File = new FileInfo(ExistFile);
                    using (ExcelPackage pck = new ExcelPackage(File))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                        //ws.Cells[6, 1].Value = "Số liệu tính từ ngày " + NhanDonTuNgay + " đến ngày " + NhanDonDenNgay;
                        #region Form tĩnh
                        // Đơn thư đối tượng
                        //var objDONTHU_DOITUONG =from D in objDonThu
                        //                         join T in vDC.DOITUONGs
                        //                         on D.d
                        int TOTAL_TONGSODON = 0;
                        int TOTAL_DONTRONGKY_NHIEUNGUOI = 0;
                        int TOTAL_DONTRONGKY_MOTNGUOI = 0;
                        int TOTAL_DUDIEUKEN_XULY = 0;
                        for (int i = 0; i < DONVI_ID_TIEPNHANs.Count; i++)
                        {


                            ws.Cells[row + 5 + i, 1].Value = DONVIs.Where(x => x.DONVI_ID == DONVI_ID_TIEPNHANs[i]).FirstOrDefault().TENDONVI;
                            ws.Cells[row + 5 + i, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Tổng số đơn
                            int TONGSODON = objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i]).Count();
                            TOTAL_TONGSODON += TONGSODON;
                            ws.Cells[row + 5 + i, 2].Value = TONGSODON;
                            ws.Cells[row + 5 + i, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ nhiều người đứng tên
                            int DONTRONGKY_NHIEUNGUOI = objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i] && x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();
                            TOTAL_DONTRONGKY_NHIEUNGUOI += DONTRONGKY_NHIEUNGUOI;
                            ws.Cells[row + 5 + i, 3].Value = DONTRONGKY_NHIEUNGUOI;
                            ws.Cells[row + 5 + i, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn trong kỳ một người đứng tên
                            int DONTRONGKY_MOTNGUOI = objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i] && x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();
                            TOTAL_DONTRONGKY_MOTNGUOI += DONTRONGKY_MOTNGUOI;
                            ws.Cells[row + 5 + i, 4].Value = DONTRONGKY_MOTNGUOI;
                            ws.Cells[row + 5 + i, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //-> Không xử lý đơn trong kỳ vì tìm theo nhiều tiêu chí thời gian khác nhau
                            ////Đơn kỳ trước chuyển sang nhiều người đứng tên                   
                            //ws.Cells[row + 5 + i, 5].Value = (objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count()- DONTRONGKY_NHIEUNGUOI);
                            //ws.Cells[row + 5 + i, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            ////Đơn kỳ trước chuyển sang một người đứng tên
                            //ws.Cells[row + 5 + i, 6].Value =( objToTalDonThu.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count()- DONTRONGKY_MOTNGUOI);
                            //ws.Cells[row + 5 + i, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            //Đơn đủ điều kiện xử lý
                            int DUDIEUKEN_XULY = objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i] && x.DONTHU_KHONGDUDDIEUKIEN == false).Count();
                            TOTAL_DUDIEUKEN_XULY += DUDIEUKEN_XULY;
                            ws.Cells[row + 5 + i, 5].Value = DUDIEUKEN_XULY;
                            ws.Cells[row + 5 + i, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                            #endregion
                        }

                        #region Form 2
                        // Excel form 2
                        // Nội dung tiếp công dân (vụ việc)
                        var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                        var objLOAIDONTHU_GOC = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 1).ToList();
                        for (int i = 0; i < DONVI_ID_TIEPNHANs.Count; i++)
                        {
                            int v_Total_Count_LV3_By_DV_ID = 0;

                            countRow_Lv3 = 5;
                            foreach (var it in objLOAIDONTHU_GOC)
                            {
                                //Loại đơn thư LV2
                                var objLOAIDONTHU_LV2 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 2 && x.LOAIDONTHU_CHA_ID == it.LOAIDONTHU_ID).ToList();
                                c_LDTLV2 = objLOAIDONTHU_LV2.Count;

                                // Loại đơn thư LV3
                                if (c_LDTLV2 > 0)
                                {
                                    foreach (var it2 in objLOAIDONTHU_LV2)
                                    {
                                        CountRow_lv1 = 0;
                                        var objLOAIDONTHU_LV3 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_CAP == 3 && x.LOAIDONTHU_CHA_ID == it2.LOAIDONTHU_ID).ToList();
                                        c_LDTLV3 = objLOAIDONTHU_LV3.Count;

                                        if (c_LDTLV3 > 0)
                                        {
                                            int v_Count_LV3_By_DV_ID = 0;
                                            CountRow_LV2 = countRow_Lv3;
                                            //Tổng
                                            if (c_LDTLV3 > 1)
                                            {
                                                vCount++;
                                                countRow_Lv3++;
                                                if (i == 0)
                                                {

                                                    ws.Cells[row + 3, countRow_Lv3].Value = "Tổng";
                                                    ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                    c_LDTLV3++;
                                                    // Tính MS                                            
                                                    string vTong = "";
                                                    for (int j = 1; j < c_LDTLV3; j++)
                                                    {
                                                        if (j < c_LDTLV3 - 1)
                                                        {
                                                            vTong += (vCount + 4 + j).ToString() + "+";
                                                        }
                                                        else
                                                        {
                                                            vTong += (vCount + 4 + j).ToString();
                                                        }

                                                    }
                                                    ws.Cells[row + 4, countRow_Lv3].Value = (vCount + 4).ToString() + "=" + vTong;
                                                    ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                }

                                            }
                                            foreach (var it3 in objLOAIDONTHU_LV3)
                                            {
                                                //Loại đơn thư lv3 excel
                                                vCount++;
                                                countRow_Lv3++;
                                                if (i == 0)
                                                {
                                                    ws.Cells[row + 3, countRow_Lv3].Value = it3.LOAIDONTHU_TEN;
                                                    ws.Cells[row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                    //MS
                                                    ws.Cells[row + 4, countRow_Lv3].Value = vCount + 4;
                                                    ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                }
                                                ws.Cells[row + 5 + i, countRow_Lv3].Value = objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i] && x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID).Count();
                                                ws.Cells[row + 5 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                v_Count_LV3_By_DV_ID = v_Count_LV3_By_DV_ID + objDonThu.Where(x => x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i] && x.LOAIDONTHU_ID == it3.LOAIDONTHU_ID).Count();

                                                v_Total_Count_LV3_By_DV_ID = v_Total_Count_LV3_By_DV_ID + v_Count_LV3_By_DV_ID;

                                            }
                                            if (i == 0)
                                            {
                                                //Loại đơn thư lv2 exccel
                                                ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Merge = true;
                                                ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Value = it2.LOAIDONTHU_TEN;
                                                ws.Cells[row + 2, CountRow_LV2 + 1, row + 2, CountRow_LV2 + c_LDTLV3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                //Loại đơn thư lv1 excel
                                                CountRow_lv1 += CountRow_LV2 + c_LDTLV3;
                                            }
                                            if (c_LDTLV3 > 1)
                                            {
                                                ws.Cells[row + 5 + i, (countRow_Lv3 - objLOAIDONTHU_LV3.Count)].Value = v_Count_LV3_By_DV_ID;
                                                ws.Cells[row + 5 + i, (countRow_Lv3 - objLOAIDONTHU_LV3.Count)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                //if (i == (DONVI_ID_TIEPNHANs.Count - 1))
                                                //{
                                                //    ws.Cells[row + 5 + i + 1, (countRow_Lv3 - objLOAIDONTHU_LV3.Count)].Value = v_Total_Count_LV3_By_DV_ID;
                                                //    ws.Cells[row + 5 + i + 1, (countRow_Lv3 - objLOAIDONTHU_LV3.Count)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                //}
                                            }

                                        }
                                        else
                                        {
                                            countRow_Lv3++;
                                            if (i == 0)
                                            {
                                                ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Merge = true;
                                                ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Value = it2.LOAIDONTHU_TEN;
                                                ws.Cells[row + 2, countRow_Lv3, row + 3, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                vCount++;
                                                //MS
                                                ws.Cells[row + 4, countRow_Lv3].Value = vCount + 4;
                                                ws.Cells[row + 4, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            }
                                            // Đếm số lượng đơn thư
                                            ws.Cells[row + 5 + i, countRow_Lv3].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it2.LOAIDONTHU_ID).Count();
                                            ws.Cells[row + 5 + i, countRow_Lv3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                            // ######################################################################################################################################################################################
                                            CountRow_lv1 = countRow_Lv3;
                                        }
                                    }
                                    if (i == 0)
                                    {
                                        // Loại đơn thư LV1
                                        ws.Cells[row + 1, countCol, row + 1, CountRow_lv1].Merge = true;
                                        ws.Cells[row + 1, countCol, row + 1, CountRow_lv1].Value = it.LOAIDONTHU_TEN;
                                        ws.Cells[row + 1, countCol, row + 1, CountRow_lv1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        countCol = CountRow_lv1 + 1;
                                    }
                                }
                                else
                                {
                                    countRow_Lv3++;
                                    if (i == 0)
                                    {
                                        // Loại đơn thư lv1 không có loại con                             
                                        ws.Cells[row + 1, countCol, row + 3, countCol].Merge = true;
                                        ws.Cells[row + 1, countCol, row + 3, countCol].Value = it.LOAIDONTHU_TEN;
                                        ws.Cells[row + 1, countCol, row + 3, countCol].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        vCount++;
                                        //MS
                                        ws.Cells[row + 4, countCol].Value = vCount + 4;
                                        ws.Cells[row + 4, countCol].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    }
                                    // Đếm số lượng đơn thư

                                    ws.Cells[row + 5 + i, countCol].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_ID && x.HUONGXULY_DONVI_ID == DONVI_ID_TIEPNHANs[i]).Count();
                                    ws.Cells[row + 5 + i, countCol].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    //countCol++;
                                }
                            }
                            if (i == (DONVI_ID_TIEPNHANs.Count - 1))
                            {

                                for (int y = 6; y <= countRow_Lv3; y++)
                                {
                                    int total = 0;
                                    for (int j = 0; j < DONVI_ID_TIEPNHANs.Count; j++)
                                    {
                                        total = total + int.Parse(ws.Cells[row + 5 + j, y].Value.ToString());
                                    }
                                    ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, y].Value = total;
                                    ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, y].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                }
                                vCount = countRow_Lv3;


                            }
                        }
                        int vColumn = 0;

                        ws.Cells[row, 6, row, vCount + vColumn].Merge = true;
                        ws.Cells[row, 6].Value = "Theo nội dung";
                        ws.Cells[row, 6, row, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[row - 1, 6, row - 1, vCount + vColumn + 6].Merge = true;
                        ws.Cells[row - 1, 6, row - 1, vCount + vColumn + 6].Value = "Phân loại đơn khiếu nại tố cáo (số đơn)";
                        ws.Cells[row - 1, 6, row - 1, vCount + vColumn + 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;



                        ////Thẩm quyền giải quyết
                        //// 1 : Cơ quan hành chính các cấp
                        //// 2 : Cơ quan tư pháp các cấp
                        //// 3 : Cơ quan Đảng


                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Thẩm quyền giải quyết
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Thẩm quyền giải quyết";
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                        // Các cơ quan hành chính các câp  
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan hành chính các cấp";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;

                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        vColumn++;

                        // Các cơ quan tư pháp các câp  
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Cơ quan tư pháp các cấp";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;

                        // Của cơ quan đảng  
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Của cơ quan đảng";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;


                        //Theo trình tự giải quyết
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Merge = true;
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Value = "Theo trình tự giải quyết";
                        ws.Cells[row, vCount + vColumn, row, vCount + vColumn + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        // Chưa được giải quyết
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Chưa được giải quyết";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;

                        //Đã được giải quyết lần đầu
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết lần đầu";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;

                        //Đã được giải quyết nhiều lần
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đã được giải quyết nhiều lần";
                        ws.Cells[row + 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        vColumn++;

                        // Đơn khác, phản ánh, kiến nghị, đơn nặc danh
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Đơn khác, phản ánh, kiến nghị, đơn nặc danh";
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[row + 4, vCount + vColumn].Value = Int32.Parse(ws.Cells[row + 4, (vCount + vColumn - 1)].Value.ToString()) + 1;
                        ws.Cells[row + 4, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true).Count();
                        //ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh

                        //var objHinhThucXuLy = vDC.HUONGXYLies.ToList();
                        //foreach (var it in objHinhThucXuLy)
                        //{
                        //    vColumn++;
                        //    ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        //    ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Value = it.HUONGXYLY_TEN;
                        //    ws.Cells[row, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //    ws.Cells[row + 5, vCount + vColumn].Value = objDonThu.Where(x => x.HUONGXULY_ID == it.HUONGXYLY_ID).Count();
                        //    ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //}
                        //// Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh
                        //vColumn++;
                        //ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Merge = true;
                        //ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Value = "Kết quả xử lý đơn khiếu nại, tố cáo, kiến nghị, phản ánh";
                        //ws.Cells[row - 1, vCount + vColumn - objHinhThucXuLy.Count, row - 1, vCount + vColumn - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        for (int i = 0; i < DONVI_ID_TIEPNHANs.Count; i++)
                        {
                            int v_Col = 0;

                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1 && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2 && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3 && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0) && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1 && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, v_Col + vCount].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1 && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, v_Col + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            v_Col++;
                            ws.Cells[row + 5 + i, vCount + v_Col].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true && DONVI_ID_TIEPNHANs[i] == x.HUONGXULY_DONVI_ID).Count();
                            ws.Cells[row + 5 + i, vCount + v_Col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            // ghi chú
                            v_Col++;
                            ws.Cells[row + 5 + i, vCount + v_Col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            if (i == DONVI_ID_TIEPNHANs.Count - 1)
                            {
                                int v_Col_total = 0;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 1).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 2).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => x.HUONGXULY_THAMQUYENGIAIQUYET_ID == 3).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => (x.DAGIAIQUYET_LAN == null || x.DAGIAIQUYET_LAN == 0)).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, v_Col_total + vCount].Value = objDonThu.Where(x => x.DONTHU_NACDANH == true).Count();
                                ws.Cells[row + 5 + i + 1, vCount + v_Col_total].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                v_Col_total++;
                                ws.Cells[row + 5 + i + 1, v_Col_total + vCount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                        }

                        // Ghi chú
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Merge = true;
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Value = "Ghi chú";
                        ws.Cells[row - 1, vCount + vColumn, row + 3, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[row + 5, vCount + vColumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        // Số thứ tự
                        //for (int j = vCount; j <= vCount + vColumn; j++)
                        //{
                        //    ws.Cells[row + 4, j].Value = i + vCount + 6;
                        //    ws.Cells[row + 4, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        //}


                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 1].Value = "Tổng";
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Tổng số đơn
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 2].Value = TOTAL_TONGSODON;
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn trong kỳ nhiều người đứng tên
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 3].Value = TOTAL_DONTRONGKY_NHIEUNGUOI;
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn trong kỳ một người đứng tên
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 4].Value = TOTAL_DONTRONGKY_MOTNGUOI;
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //Đơn đủ điều kiện xử lý
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 5].Value = TOTAL_DUDIEUKEN_XULY;
                        ws.Cells[row + 5 + DONVI_ID_TIEPNHANs.Count, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);



                        #endregion
                        fileBytes = pck.GetAsByteArray();

                    }

                }
                return fileBytes;
            }
            catch (Exception Ex)
            {
                return null;
            }

        }

        #endregion

        #region Thống kê đơn thư theo đơn vị chuyển new 
        public Byte[] TK_DONVICHUYEN_NEW(string NhanDonTuNgay, string NhanDonDenNgay, string DonChuyenTuNgay, string DonChuyenDenNgay, string HanGiaiQuyetTuNgay, string HanGiaiQuyetDenNgay, string DONVI_IDs_CHUYEN, string GIAIQUYETTRANGTHAI_ID)
        {
            try
            {
                Byte[] fileBytes = null;
                List<int> DONVI_ID_TIEPNHANs = new List<int>();

                //Đơn thư có hướng xử lý là chuyển đơn
                var objDonThu = vDC.DONTHUs.Where(x => x.HUONGXULY_ID == 3 && x.HUONGXULY_DONVI_ID == Convert.ToInt32(DONVI_IDs_CHUYEN)).ToList();
                // Ngày nhận đơn
                if (NhanDonTuNgay != "" && NhanDonDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.NGAYTAO >= DateTime.ParseExact(NhanDonTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.NGAYTAO <= DateTime.ParseExact(NhanDonDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                // Ngày chuyển đơn
                if (DonChuyenTuNgay != "" && DonChuyenDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.HUONGXULY_NGAYCHUYEN >= DateTime.ParseExact(DonChuyenTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.HUONGXULY_NGAYCHUYEN <= DateTime.ParseExact(DonChuyenDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                // Thời hạn giải quyết

                if (HanGiaiQuyetTuNgay != "" && HanGiaiQuyetDenNgay != "")
                {
                    objDonThu = objDonThu.Where(x => x.HUONGXULY_THOIHANGIAIQUET >= DateTime.ParseExact(HanGiaiQuyetTuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.HUONGXULY_THOIHANGIAIQUET <= DateTime.ParseExact(HanGiaiQuyetDenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                }

                if (GIAIQUYETTRANGTHAI_ID == "0")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == false || x.KETQUA_XYLY == null).ToList();
                }
                // Đã có kết quả giải quyết
                else if (GIAIQUYETTRANGTHAI_ID == "1")
                {
                    objDonThu = objDonThu.Where(x => x.KETQUA_XYLY == true).ToList();
                }
                //if (DONVI_IDs_CHUYEN != "")
                //{
                //    List<int> DONVI_IDs = new List<int>();
                //    List<string> DONVI_IDs_List = DONVI_IDs_CHUYEN.Split(',').ToList();
                //    for (int i = 0; i < DONVI_IDs_List.Count(); i++)
                //    {
                //        DONVI_IDs.Add(Int32.Parse(DONVI_IDs_List[i]));
                //    }
                //}
                //DONVI_ID_TIEPNHANs = objDonThu.Select(x => Int32.Parse(x.HUONGXULY_DONVI_ID.ToString())).Distinct().ToList();
                //var DONVIs = vDC.DONVIs.ToList();
                // Chưa có kết quả

                if (objDonThu.Count > 0)
                {
                    var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_DONVICHUYEN.xlsx";
                    var File = new FileInfo(ExistFile);
                    using (ExcelPackage pck = new ExcelPackage(File))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                        var objDonVi = vDC.DONVIs.Where(x => x.DONVI_ID == Convert.ToInt32(DONVI_IDs_CHUYEN)).FirstOrDefault();
                        if (objDonVi != null)
                        {
                            ws.Cells[5, 1].Value = "THỐNG KÊ ĐƠN THƯ CHUYỂN ĐẾN " + objDonVi.TENDONVI.ToUpper();
                        }
                        else
                        {
                            ws.Cells[5, 1].Value = "THỐNG KÊ ĐƠN THƯ THEO ĐƠN VỊ CHUYỂN";
                        }

                        ws.Cells[7, 1].Value = "Số liệu tính từ ngày " + NhanDonTuNgay + " đến ngày " + NhanDonDenNgay;
                        int vIndexRow = 11;
                        foreach (var item in objDonThu)
                        {
                            //string ThongTinDoiTuongHoTen = "";
                            //string DiaChi = "";
                            //foreach (var CANHAN_item in item.DOITUONG.CANHANs)
                            //{
                            //    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;
                            //}
                            //ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);
                            //if (item.DOITUONG.CANHANs.Count > 1)
                            //{
                            //    DiaChi = item.DOITUONG.DOITUONG_DIACHI;
                            //    var obj = item.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                            //    if (obj != null)
                            //    {
                            //        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                            //    }
                            //    else
                            //    {
                            //        DiaChi = item.DOITUONG.CANHANs.FirstOrDefault().CANHAN_DIACHI_DAYDU;
                            //    }
                            //}
                            //else
                            //{
                            //    DiaChi = item.DOITUONG.CANHANs.FirstOrDefault().CANHAN_DIACHI_DAYDU;
                            //}

                            var LoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == item.LOAIDONTHU_CHA_ID).FirstOrDefault();
                            var objCANHAN = item.DOITUONG.CANHANs;
                            ws.Cells[vIndexRow, 1].Value = vIndexRow - 10;
                            ws.Cells[vIndexRow, 2].Value = item.DONTHU_STT;
                            ws.Cells[vIndexRow, 3].Value = getThongTinDoiTuong((int)item.DOITUONG_ID);
                            ws.Cells[vIndexRow, 4].Value = item.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : item.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" : item.NGUONDON_LOAI_CHITIET == 1 ? "Cơ quan khác chuyển tới" : "Hộp thư góp ý";
                            ws.Cells[vIndexRow, 5].Value = LoaiDonThu != null ? LoaiDonThu.LOAIDONTHU_TEN : "";
                            ws.Cells[vIndexRow, 6].Value = item.DONTHU_NOIDUNG;
                            ws.Cells[vIndexRow, 7].Value = "Đang xử lý";  // Chuyển đơn ==> trạng thái đang xử lý
                            ws.Cells[vIndexRow, 8].Value = item.HUONGXULY_TEN;
                            ws.Cells[vIndexRow, 9].Value = item.HUONGXULY_YKIEN_XULY;
                            ws.Cells[vIndexRow, 10].Value = item.NGAYTAO == null ? "" : Convert.ToDateTime(item.NGAYTAO).ToString("dd/MM/yyyy");
                            ws.Cells[vIndexRow, 11].Value = item.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(item.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");

                            vIndexRow++;
                            ws.InsertRow(vIndexRow, 1);
                        }

                        fileBytes = pck.GetAsByteArray();

                    }

                }
                return fileBytes;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        #endregion
        #region Thống kê trùng đơn new
        public Byte[] TK_DONTHU_TRUNG_NEW(string pTuNgay, string pDenNgay, string pLoaiDoiTuong)
        {
            int row = 11;
            Byte[] fileBytes = null;       
            List<DONTHU> lstDonThuTrung = new List<DONTHU>();
            List<DONTHU> objDT = vDC.DONTHUs.Where(x => (x.NGAYTAO.Value.Date > Convert.ToDateTime(pTuNgay))
                                                                        && (x.NGAYTAO.Value.Date < Convert.ToDateTime(pDenNgay))
                                                                        && (pLoaiDoiTuong == "0" || x.DOITUONG.DOITUONG_LOAI == Convert.ToInt32(pLoaiDoiTuong))
                                                                         ).OrderByDescending(x => x.DONTHU_STT).ToList();

            List<long> DONTHU_REMOVE_ID = new List<long>();
            // Kiểm tra đơn thư trùng 
            foreach (var item in objDT)
            {
                if (DONTHU_REMOVE_ID.Count == 0 || (DONTHU_REMOVE_ID.Count > 0 && !DONTHU_REMOVE_ID.Contains(item.DONTHU_ID)))
                {
                    var objTRung = objDT.Where(x => x.DONTHU_ID != item.DONTHU_ID && x.DONTHU_NOIDUNG == item.DONTHU_NOIDUNG && x.DOITUONG_ID == item.DOITUONG_ID).ToList();
                    if (objTRung.Count > 0)
                    {
                        lstDonThuTrung.Add(item);
                        lstDonThuTrung.AddRange(objTRung);
                        DONTHU_REMOVE_ID.AddRange(objTRung.Select(x => x.DONTHU_ID).ToList());
                    }
                }
            }
            if (lstDonThuTrung.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_TRUNGDONTHU.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells[7, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                    var LoaiDonThu_LV0 = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == 0).ToList();
                    foreach (var it in lstDonThuTrung)
                    {
                        var LoaiDonThu = LoaiDonThu_LV0.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_CHA_ID).FirstOrDefault();
                        ws.Cells[row, 1].Value = row - 10;
                        ws.Cells[row, 2].Value = it.DONTHU_STT;
                        ws.Cells[row, 3].Value = getThongTinDoiTuong((int)it.DOITUONG_ID);
                        ws.Cells[row, 4].Value = it.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : it.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" : it.NGUONDON_LOAI_CHITIET == 1 ? "Cơ quan khác chuyển tới" : "Hộp thư góp ý";
                        ws.Cells[row, 5].Value = LoaiDonThu != null ? LoaiDonThu.LOAIDONTHU_TEN : "";
                        ws.Cells[row, 6].Value = it.DONTHU_NOIDUNG;
                        //("TRANGTHAI_DONTHUKHONGDUDIEUKIEN").ToString()=="True" ?"Lưu đơn":
                        ws.Cells[row, 7].Value = it.TRANGTHAI_DONTHUKHONGDUDIEUKIEN == true ? "Lưu đơn" : it.HUONGXULY_ID == null ? "Chưa xử lý" : GetTrangThai((int)it.HUONGXULY_ID);
                        ws.Cells[row, 8].Value = it.HUONGXULY_TEN;
                        ws.Cells[row, 9].Value = it.HUONGXULY_YKIEN_XULY;
                        ws.Cells[row, 10].Value = it.NGAYTAO == null ? "" : Convert.ToDateTime(it.NGAYTAO).ToString("dd/MM/yyyy");
                        ws.Cells[row, 11].Value = it.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(it.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                        //ws.Cells[row, 11].Value = objDONTHU_GOCID.Where(x => x == it.DONTHU_ID).Count();

                        for (int i = 1; i < 12; i++)
                        {
                            ws.Cells[row, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        row++;
                    }
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê trùng đơn new
        public Byte[] TK_DONTHU_NEW(string pTuNgay, string pDenNgay,List<DONTHU> objDONTHU)
        {
            int row = 11;
            Byte[] fileBytes = null;          
            if (objDONTHU.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_DONTHU.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells[7, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                    var LoaiDonThu_LV0 = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_CHA_ID == 0).ToList();
                    foreach (var it in objDONTHU)
                    {
                        var LoaiDonThu = LoaiDonThu_LV0.Where(x => x.LOAIDONTHU_ID == it.LOAIDONTHU_CHA_ID).FirstOrDefault();
                        ws.Cells[row, 1].Value = row - 10;
                        ws.Cells[row, 2].Value = it.DONTHU_STT;
                        ws.Cells[row, 3].Value = getThongTinDoiTuong((int)it.DOITUONG_ID);
                        ws.Cells[row, 4].Value = it.NGUONDON_LOAI_CHITIET == 0 ? "Trực tiếp" : it.NGUONDON_LOAI_CHITIET == 1 ? "Bưu chính" : it.NGUONDON_LOAI_CHITIET == 1 ? "Cơ quan khác chuyển tới" : "Hộp thư góp ý";
                        ws.Cells[row, 5].Value = LoaiDonThu != null ? LoaiDonThu.LOAIDONTHU_TEN : "";
                        ws.Cells[row, 6].Value = it.DONTHU_NOIDUNG;
                        //("TRANGTHAI_DONTHUKHONGDUDIEUKIEN").ToString()=="True" ?"Lưu đơn":
                        ws.Cells[row, 7].Value = it.TRANGTHAI_DONTHUKHONGDUDIEUKIEN == true ? "Lưu đơn" : it.HUONGXULY_ID == null ? "Chưa xử lý" : GetTrangThai((int)it.HUONGXULY_ID);
                        ws.Cells[row, 8].Value = it.HUONGXULY_TEN;
                        ws.Cells[row, 9].Value = it.HUONGXULY_YKIEN_XULY;
                        ws.Cells[row, 10].Value = it.NGAYTAO == null ? "" : Convert.ToDateTime(it.NGAYTAO).ToString("dd/MM/yyyy");
                        ws.Cells[row, 11].Value = it.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(it.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                        //ws.Cells[row, 11].Value = objDONTHU_GOCID.Where(x => x == it.DONTHU_ID).Count();

                        for (int i = 1; i < 12; i++)
                        {
                            ws.Cells[row, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        row++;
                    }
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion
        #region Thống kê trùng đơn new
        public Byte[] TK_TIEPDAN_NEW(string pTuNgay, string pDenNgay, List<TIEPDAN> objTIEPDAN)
        {
            int row = 11;
            Byte[] fileBytes = null;
            if (objTIEPDAN.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "TK_TIEPDAN.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells[7, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;
                   
                    foreach (var it in objTIEPDAN)
                    {

                        ws.Cells[row, 1].Value = row - 10;
                        ws.Cells[row, 2].Value = it.TIEPDAN_STT;
                        ws.Cells[row, 3].Value = getThongTinDoiTuong((int)it.DOITUONG_ID);
                        ws.Cells[row, 4].Value = it.DOITUONG.DOITUONG_LOAI == 1 ? "Cá Nhân" : it.DOITUONG.DOITUONG_LOAI == 2 ? "Nhóm đông người"  :it.DOITUONG.DOITUONG_TEN;
                        ws.Cells[row, 5].Value = it.DONTHU_ID == null ?"Không đơn" : "Có đơn";
                        ws.Cells[row, 6].Value = it.TIEPDAN_NOIDUNG;               
                        ws.Cells[row, 7].Value = it.DONTHU_ID == null ?it.TIEPDAN_KETQUA : GetYKienXuLy((int)it.DONTHU_ID);
                        ws.Cells[row, 8].Value = it.TIEPDAN_LANTIEP;
                        ws.Cells[row, 9].Value = it.TIEPDAN_THOGIAN == null ? "" : Convert.ToDateTime(it.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        //ws.Cells[row, 11].Value = it.NGUONDON_NGAYDEDON == null ? "" : Convert.ToDateTime(it.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy");
                        for (int i = 1; i < 10; i++)
                        {
                            ws.Cells[row, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        row++;
                    }
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Sổ tiếp dân
        public DataTable Get_SoTiepDan_DataTable(List<TIEPDAN> objTiepDans)
        {
            try
            {
                var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();
                var objHINHTHUCGIAIQUYET = vDC.HINHTHUCGIAIQUYETs.ToList();
                var objDONVI = vDC.DONVIs.ToList();
                var objCANBO = vDC.CANBOs.ToList();

                DataTable dt = new DataTable();

                dt.Columns.Add("Số TT");
                dt.Columns.Add("Ngày tháng");
                dt.Columns.Add("Họ tên, địa chỉ người khiếu nại-tố cáo");
                dt.Columns.Add("Nội dung Khiếu nại - Tố cáo");
                dt.Columns.Add("Số người tham gia");
                dt.Columns.Add("Công dân trực tiếp đến");
                //dt.Columns.Add("Cơ quan khác chuyển đến");
                dt.Columns.Add("Phân loại nội dung khiếu nại tố cáo");
                dt.Columns.Add("Cơ quan đã giải quyết; hình thức giải quyết");
                dt.Columns.Add("Tên cơ quan có thẩm quyền giải quyết tiếp theo");
                dt.Columns.Add("Kết quả xử lý (Tiếp nhận hoặc giải thích hướng dẫn)");
                dt.Columns.Add("Cán bộ tiếp dân xử lý đơn");
                dt.Columns.Add("Ghi chú");

                int rowCount = 0;
                if (objTiepDans.Count > 0)
                {
                    foreach (var it in objTiepDans)
                    {
                        string vCoquan_HinhThuc = "";
                        rowCount++;
                        DataRow row = dt.NewRow();
                        row["Số TT"] = rowCount;
                        row["Ngày tháng"] = it.TIEPDAN_THOGIAN == null ? "" : Convert.ToDateTime(it.TIEPDAN_THOGIAN).ToString("dd/MM/yyyy");
                        if (it.DONTHU_ID == null)
                        {
                            row["Nội dung Khiếu nại - Tố cáo"] = "Không đơn: " + it.TIEPDAN_NOIDUNG;
                        }
                        else
                        {
                            DONTHU objDONTHU = vDC.DONTHUs.Where(x => x.DONTHU_ID == it.DONTHU_ID).FirstOrDefault();
                            row["Nội dung Khiếu nại - Tố cáo"] = "Có đơn: " + objDONTHU.DONTHU_NOIDUNG;
                        }

                        var objCANHAN = vDC.CANHANs.Where(x => x.DOITUONG_ID == it.DOITUONG_ID).ToList();
                        string canhan = "";
                        foreach (var cn in objCANHAN)
                        {
                            if (canhan == "")
                            {
                                canhan += cn.CANHAN_HOTEN + "\n";
                                canhan += cn.CANHAN_DIACHI_DAYDU;
                            }
                            else
                            {
                                canhan += "\n" + cn.CANHAN_HOTEN + "\n";
                                canhan += cn.CANHAN_DIACHI_DAYDU;
                            }

                        }

                        row["Họ tên, địa chỉ người khiếu nại-tố cáo"] = canhan;
                        row["Số người tham gia"] = it.DOITUONG.DOITUONG_SONGUOI;

                        row["Công dân trực tiếp đến"] = objCANHAN.Count;

                        //row["Cơ quan khác chuyển đến"] = "";

                        // Kiểm tra tiếp dân có đơn hoặc ko đơn      
                        //Không đơn
                        if (it.DONTHU_ID == null)
                        {
                            var objLDT = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == it.TIEPDAN_LOAI).FirstOrDefault();
                            if (objLDT != null)
                            {
                                string[] arr = objLDT.LOAIDONTHU_INDEX.Split('.');
                                var objLDT_LV0 = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == Convert.ToInt32(arr[0])).FirstOrDefault();
                                row["Phân loại nội dung khiếu nại tố cáo"] = objLDT_LV0.LOAIDONTHU_TEN;
                            }
                            row["Kết quả xử lý (Tiếp nhận hoặc giải thích hướng dẫn)"] = it.TIEPDAN_KETQUA;
                        }
                        //Có đơn 
                        else
                        {
                            var objDT = vDC.DONTHUs.Where(x => x.DONTHU_ID == it.DONTHU_ID).FirstOrDefault();
                            if (objDT != null)
                            {
                                if (objDT.LOAIDONTHU_CHA_ID != null)
                                {
                                    var objLoai = objLOAIDONTHU.Where(x => x.LOAIDONTHU_ID == objDT.LOAIDONTHU_CHA_ID).FirstOrDefault();
                                    if (objLoai != null)
                                    {
                                        row["Phân loại nội dung khiếu nại tố cáo"] = objLoai.LOAIDONTHU_TEN;
                                    }
                                }

                                // Tiếp dân có đơn => Đơn thư cơ quan giải quyết = DAGIAIQUYET_DONVI_ID                      
                                if (objDT.DAGIAIQUYET_DONVI_ID != null)
                                {
                                    var objDV = objDONVI.Where(x => x.DONVI_ID == objDT.DAGIAIQUYET_DONVI_ID).FirstOrDefault();
                                    if (objDV != null)
                                    {
                                        vCoquan_HinhThuc += objDV.TENDONVI + ", ";
                                    }
                                }
                                //hình thức giải quyết xác định bằng DAGIAIQUYET_HTGQ_ID
                                if (objDT.DAGIAIQUYET_HTGQ_ID != null)
                                {
                                    var objHTGQ = objHINHTHUCGIAIQUYET.Where(x => x.HTGQ_ID == objDT.DAGIAIQUYET_HTGQ_ID).FirstOrDefault();
                                    if (objHTGQ != null)
                                    {
                                        vCoquan_HinhThuc += objHTGQ.HTGQ_TEN;
                                    }
                                }

                                // Cơ quan có thẩm quyền giải quyết tiếp theo  => Hướng xử lý Id = 5 chuyển đơn =>  HUONGXULY_DONVI_ID                     
                                if (objDT.HUONGXULY_DONVI_ID != null)
                                {
                                    var objDV = objDONVI.Where(x => x.DONVI_ID == objDT.HUONGXULY_DONVI_ID).FirstOrDefault();
                                    if (objDV != null)
                                    {
                                        row["Tên cơ quan có thẩm quyền giải quyết tiếp theo"] = objDV.TENDONVI;
                                    }
                                }
                                row["Kết quả xử lý (Tiếp nhận hoặc giải thích hướng dẫn)"] = objDT.HUONGXULY_YKIEN_XULY;

                                if (objDT.HUONGXULY_CANBO != null)
                                {
                                    var objCB = objCANBO.Where(x => x.CANBO_ID == objDT.HUONGXULY_CANBO).FirstOrDefault();
                                    if (objCB != null)
                                    {
                                        row["Cán bộ tiếp dân xử lý đơn"] = objCB.CANBO_TEN;
                                    }
                                }
                            }
                        }
                        dt.Rows.Add(row);
                    }
                }
                else
                {
                    // Set form rổng
                    DataRow row = dt.NewRow();
                    row["Số TT"] = "";
                    row["Ngày tháng"] = "";
                    row["Họ tên, địa chỉ người khiếu nại-tố cáo"] = "";
                    row["Nội dung Khiếu nại - Tố cáo"] = "";
                    row["Số người tham gia"] = "";
                    row["Công dân trực tiếp đến"] = "";
                    //dt.Columns.Add("Cơ quan khác chuyển đến");
                    row["Phân loại nội dung khiếu nại tố cáo"] = "";
                    row["Cơ quan đã giải quyết; hình thức giải quyết"] = "";
                    row["Tên cơ quan có thẩm quyền giải quyết tiếp theo"] = "";
                    row["Kết quả xử lý (Tiếp nhận hoặc giải thích hướng dẫn)"] = "";
                    row["Cán bộ tiếp dân xử lý đơn"] = "";
                    row["Ghi chú"] = "";
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region Get dữ liệu
        public string getThongTinDoiTuong(int pTDOITUONG_ID)
        {
            try
            {
                string strDoiTuong = "";
                List<CANHAN> objCANHANs = vDC.CANHANs.Where(x => x.DOITUONG_ID == pTDOITUONG_ID).ToList();
                if (objCANHANs.Count > 0)
                {
                    for (int i = 0; i < objCANHANs.Count; i++)
                    {
                        strDoiTuong = strDoiTuong + objCANHANs[i].CANHAN_HOTEN;
                        strDoiTuong = strDoiTuong + (objCANHANs[i].CANHAN_DIACHI_DAYDU == "" ? "" : " Địa chỉ: " + objCANHANs[i].CANHAN_DIACHI_DAYDU + "\n");
                    }
                }
                return strDoiTuong;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public string GetCoQuanChuyenDon(string vCoQuanId)
        {
            try
            {
                string vTenCoQuan = "";
                vTenCoQuan = vDC.DONVIs.Where(x => x.DONVI_ID == int.Parse(vCoQuanId)).FirstOrDefault().TENDONVI;
                return vTenCoQuan;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string GetTenLoaiDonThuById(string pLOAIDONTHU_ID)
        {
            try
            {
                string vTenLoaiDonThu = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == int.Parse(pLOAIDONTHU_ID)).FirstOrDefault().LOAIDONTHU_TEN;
                return vTenLoaiDonThu;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public List<HoSo> GetUrlFile(long pDONTHU_ID, int pLoai_HS_DonThu)
        {
            List<HoSo> lstHoSo = new List<HoSo>();
            try
            {
                lstHoSo = vDC.DONTHU_HOSOs.Where(x => x.DONTHU_ID == pDONTHU_ID && x.LOAI_HS_DONTHU == pLoai_HS_DonThu).Select(y => new HoSo
                {
                    HOSO_TEN = y.HOSO.HOSO_TEN,
                    HOSO_FILE = vPathFile + y.HOSO.HOSO_FILE
                }).ToList();

                return lstHoSo;
            }
            catch (Exception ex)
            {
                return lstHoSo;
            }
        }
        public string GetTenDonVi(int pDonViID)
        {
            string TenDonVi = "";
            var objDonVi = vDC.DONVIs.Where(x => x.DONVI_ID == pDonViID).FirstOrDefault();
            if (objDonVi != null)
            {
                TenDonVi = objDonVi.TENDONVI;
            }

            return TenDonVi;
        }

        public string GetTenCanBo(int pCanBoID)
        {
            string TenCanBo = "";
            var objCanBo = vDC.CANBOs.Where(x => x.CANBO_ID == pCanBoID).FirstOrDefault();
            if (objCanBo != null)
            {
                TenCanBo = objCanBo.CANBO_TEN;
            }

            return TenCanBo;
        }

        public string GetLoaiTiepDan(int pID)
        {
            string TenLoai = "";
            var objLoai = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pID).FirstOrDefault();
            if (objLoai != null)
            {
                TenLoai = objLoai.LOAIDONTHU_TEN;
            }
            return TenLoai;
        }
        public string GetLoaiTiepDan_lv0(int pID)
        {
            string TenLoai = "";
            var objLDT = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pID).FirstOrDefault();
            if (objLDT != null)
            {
                string[] arr = objLDT.LOAIDONTHU_INDEX.Split('.');
                var objLDT_LV0 = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == Convert.ToInt32(arr[0])).FirstOrDefault();
                TenLoai = objLDT_LV0.LOAIDONTHU_TEN;
            }
            return TenLoai;
        }

        public string GetLoaiTiepDan_lv1(int pID)
        {
            string TenLoai = "";
            var objLDT = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == pID).FirstOrDefault();
            if (objLDT != null)
            {
                if (objLDT.LOAIDONTHU_CHA_ID != 0)
                {
                    TenLoai = objLDT.LOAIDONTHU_CHA_TEN;
                }
                else
                {
                    TenLoai = objLDT.LOAIDONTHU_TEN;
                }

            }
            return TenLoai;
        }
        public bool TryParseExact_DateTime(string dateString, out DateTime oDatetimeParse)
        {
            try
            {
                if (dateString == "")
                {
                    oDatetimeParse = new DateTime();
                    return false;
                }
                else
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    bool isSuccess = DateTime.TryParseExact(dateString, "dd/MM/yyyy", provider, DateTimeStyles.None, out oDatetimeParse);
                    return true;
                }

            }
            catch (Exception Ex)
            {
                oDatetimeParse = new DateTime();
                return false;
            }


        }
        #endregion

        // Bổ sung 2022
        #region Thống kê kết quả xử lý đơn
        public Byte[] KQ_XULYDON(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;

            Byte[] fileBytes = null;
            List<DONTHU> objDonThu = new List<DONTHU>();

            // Tổng đơn kỳ trước chuyển sang
            var objDonThu_TrongKy = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(pTuNgay).Date && x.NGAYTAO <= Convert.ToDateTime(pDenNgay).Date).ToList();

            // DONTHU_TRANGTHAI == 3 đã có kết quả xử lý, DONTHU_TRANGTHAI == 4 Kết thúc đơn;
            var objDonThu_KyTruoc = vDC.DONTHUs.Where(x => (x.NGAYTAO < Convert.ToDateTime(pTuNgay).Date && x.DONTHU_TRANGTHAI != 3 && x.DONTHU_TRANGTHAI != 4)
                                                          || (x.DONTHU_TRANGTHAI == 3 && x.KETQUA_NGAY >= Convert.ToDateTime(pTuNgay).Date && x.NGAYTAO <= Convert.ToDateTime(pTuNgay).Date)).ToList();
            // Tổng đơn kỳ trước chuyển sang

            objDonThu.AddRange(objDonThu_TrongKy);
            objDonThu.AddRange(objDonThu_KyTruoc);
            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "01_XLD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    #region Form tĩnh
                    // Header
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    //2 Tổng số đơn
                    ws.Cells[row, 2].Value = objDonThu.Count();

                    //3. Kỳ trước chuyển sang nhiều người đứng tên
                    ws.Cells[row, 3].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //4. Kỳ trước chuyển sang một người đứng tên
                    ws.Cells[row, 4].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //5. Kỳ trước chuyển sang đơn khác
                    ws.Cells[row, 5].Value = objDonThu_KyTruoc.Where(x => x.DONTHU_NACDANH == true).Count();

                    //6 Tiếp nhận trong kỳ nhiều người đứng tên
                    ws.Cells[row, 6].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //7 Tiếp nhận trong kỳ một người đứng tên
                    ws.Cells[row, 7].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //8 Tiếp nhận trong kỳ đơn khác
                    ws.Cells[row, 8].Value = objDonThu_TrongKy.Where(x => x.DONTHU_NACDANH == true).Count();

                    // 9 Số đơn đã xử lý
                    ws.Cells[row, 9].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    // Lấy đơn thư đủ điều kiện xử lý
                    objDonThu = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();
                    objDonThu_TrongKy = objDonThu_TrongKy.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();
                    objDonThu_KyTruoc = objDonThu_KyTruoc.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();


                    //10 Đơn đủ điều kiện xử lý số đơn
                    ws.Cells[row, 10].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    //11 Đơn đủ điều kiện xử lý số vụ việc
                    ws.Cells[row, 11].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    // Loại đơn thư
                    var objLOAIDONTHU = vDC.LOAIDONTHUs.ToList();

                    //12 Phân loại đơn khiếu nại
                    // LOAIDONTHU_INDEX = "1." => Loại khiếu nại
                    var objLOAIDONTHU_KHIEUNAI = objLOAIDONTHU.Where(x => x.LOAIDONTHU_INDEX.StartsWith("1.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    ws.Cells[row, 12].Value = objDonThu.Where(x => objLOAIDONTHU_KHIEUNAI.Contains((int)x.LOAIDONTHU_ID)).Count();

                    //13 Phân loại đơn tố cáo
                    // LOAIDONTHU_INDEX = "2004." => Loại khiếu nại
                    var objLOAIDONTHU_TOCAO = objLOAIDONTHU.Where(x => x.LOAIDONTHU_INDEX.StartsWith("2004.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    ws.Cells[row, 13].Value = objDonThu.Where(x => objLOAIDONTHU_TOCAO.Contains((int)x.LOAIDONTHU_ID)).Count();

                    //14 Phân loại đơn kiến nghị phản ánh
                    // LOAIDONTHU_INDEX = "4035." => Loại kiến nghị phản ánh
                    var objLOAIDONTHU_PAKN = objLOAIDONTHU.Where(x => x.LOAIDONTHU_INDEX.StartsWith("4035.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    ws.Cells[row, 14].Value = objDonThu.Where(x => objLOAIDONTHU_PAKN.Contains((int)x.LOAIDONTHU_ID)).Count();

                    //15 Tình trạng đã giải quyết lần đầu
                    ws.Cells[row, 15].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1).Count();

                    //16 Tình trạng đã giải quyết nhiều lần
                    ws.Cells[row, 16].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1).Count();

                    //17 Tình trạng đang giải quyết
                    ws.Cells[row, 17].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 1 || x.DONTHU_TRANGTHAI == 2).Count();

                    //18 Tình trạng chưa giải quyết
                    ws.Cells[row, 18].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 0).Count();

                    //19 Đơn thuộc thẩm quyền tổng số
                    ws.Cells[row, 19].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1).Count();

                    //20 Đơn thuộc thẩm quyền khiếu nại
                    ws.Cells[row, 20].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.LOAIDONTHU_ID == 1).Count();

                    //21 Đơn thuộc thẩm quyền tố cáo
                    ws.Cells[row, 21].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.LOAIDONTHU_ID == 2004).Count();

                    //22 Đơn thuộc thẩm quyền kiến nghị phản ánh
                    ws.Cells[row, 22].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.LOAIDONTHU_ID == 4035).Count();

                    //23 Đơn không thuộc thẩm quyền tổng số
                    ws.Cells[row, 23].Value = objDonThu.Where(x => x.HUONGXULY_ID != 1 && x.LOAIDONTHU_ID == 1).Count();

                    //24 Đơn không thuộc thẩm quyền hướng dẫn
                    ws.Cells[row, 24].Value = objDonThu.Where(x => x.HUONGXULY_ID == 2).Count();

                    //25 Đơn không thuộc thẩm quyền chuyển đơn
                    ws.Cells[row, 25].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3).Count();

                    //26 Đơn không thuộc thẩm quyền đôn đốc giải quyết
                    ws.Cells[row, 26].Value = objDonThu.Where(x => x.HUONGXULY_ID == 4).Count();

                    //27 Số văn bản phúc đáp nhận được do chuyển đơn
                    ws.Cells[row, 27].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    #endregion                                
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê kết quả xử lý đơn khiếu nại
        public Byte[] KQ_XULYDON_KHIEUNAI(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;
            Byte[] fileBytes = null;
            DateTime _TuNgay = Convert.ToDateTime(pTuNgay);
            DateTime _DenNgay = Convert.ToDateTime(pDenNgay);
            // LOAIDONTHU_ID = "1" => Loại khiếu nại       
            List<DONTHU> objDonThu = new List<DONTHU>();
            var objDonThu_TrongKy = vDC.DONTHUs.Where(x => x.NGAYTAO >= _TuNgay.Date
                                                && x.NGAYTAO <= _DenNgay.Date
                                                && x.LOAIDONTHU_CHA_ID == 1
                                                ).ToList();
            // Tổng đơn kỳ trước chuyển sang
            var objDonThu_KyTruoc = vDC.DONTHUs.Where(x => (x.NGAYTAO < _TuNgay.Date && x.DONTHU_TRANGTHAI != 3 && x.DONTHU_TRANGTHAI != 4)
                                                        || (x.DONTHU_TRANGTHAI == 3 && x.KETQUA_NGAY > _TuNgay.Date && x.NGAYTAO < _TuNgay.Date)).ToList();
            objDonThu_KyTruoc = objDonThu_KyTruoc.Where(x => x.LOAIDONTHU_CHA_ID == 1).ToList();

            objDonThu.AddRange(objDonThu_TrongKy);
            objDonThu.AddRange(objDonThu_KyTruoc);

            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "02_XLD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    // Đơn thư đối tượng                
                    // Số liệu báo cáo từ ngày đến ngày
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    //2 Tổng số đơn
                    ws.Cells[row, 2].Value = objDonThu.Count();

                    //3. Kỳ trước chuyển sang nhiều người đứng tên
                    ws.Cells[row, 3].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //4. Kỳ trước chuyển sang một người đứng tên
                    ws.Cells[row, 4].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //5 Tiếp nhận trong kỳ nhiều người đứng tên
                    ws.Cells[row, 5].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //6 Tiếp nhận trong kỳ một người đứng tên
                    ws.Cells[row, 6].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();


                    //0: chưa xử lý, 1: đã có hướng xử lý, 2: gửi giải quyết đơn thư, 3 đã có kết quả giải quyết, 4 Kết thúc đơn
                    // 7 Số đơn đã xử lý tổng
                    ws.Cells[row, 7].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //8 Số đơn đã xử lý kỳ trước chuyển sang
                    ws.Cells[row, 8].Value = objDonThu_KyTruoc.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //9 Số đơn đã xử lý trong kỳ
                    ws.Cells[row, 9].Value = objDonThu_TrongKy.Where(x => x.DONTHU_TRANGTHAI == 3).Count();


                    objDonThu_KyTruoc = objDonThu_KyTruoc.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();
                    objDonThu_TrongKy = objDonThu_TrongKy.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();
                    objDonThu = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).ToList();

                    //10 Đơn đủ điều kiện xử lý số đơn
                    ws.Cells[row, 10].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();


                    //11 Đơn đủ điều kiện xử lý số vụ việc

                    ws.Cells[row, 11].Value = objDonThu.Count();


                    //||||| Phân loại vụ việc theo nội dung
                    // Loại đơn thư phản ánh kiến nghị
                    // LOAIDONTHU_INDEX = "1." => Loại khiếu nại
                    var objLOAIDONTHU_KHIEUNAI = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_INDEX.StartsWith("1.")).ToList();

                    //12 Phân loại đơn khiếu nại lĩnh vực hành chính -> tổng
                    // LOAIDONTHU_INDEX = "1.2." => Loại khiếu nại lĩnh vực hành chính
                    var objLOAIDONTHU_KHIEUNAI_HANHCHINH = objLOAIDONTHU_KHIEUNAI.Where(x => x.LOAIDONTHU_INDEX.StartsWith("1.2.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    int _HC_TONG = objDonThu.Where(x => objLOAIDONTHU_KHIEUNAI_HANHCHINH.Contains((int)x.LOAIDONTHU_ID)).Count();
                    ws.Cells[row, 12].Value = _HC_TONG;

                    //13 Phân loại đơn khiếu nại lĩnh vực hành chính -> Chế độ chính sách
                    // LOAIDONTHU_ID = "2002." => khiếu nại lĩnh vực hành chính -> Chế độ chính sách
                    int _CDCS = objDonThu.Where(x => x.LOAIDONTHU_ID == 2002).Count();
                    ws.Cells[row, 13].Value = _CDCS;

                    //14 Phân loại đơn khiếu nại lĩnh vực hành chính -> Đất đai nhà cửa
                    // LOAIDONTHU_ID = "2003." => khiếu nại lĩnh vực hành chính -> Đất đai nhà cửa
                    int _DDNC = objDonThu.Where(x => x.LOAIDONTHU_ID == 2003).Count();
                    ws.Cells[row, 14].Value = _DDNC;

                    //15 Phân loại đơn khiếu nại lĩnh vực hành chính -> khác
                    ws.Cells[row, 15].Value = _HC_TONG - (_CDCS + _DDNC);

                    //16 Phân loại đơn khiếu nại lĩnh vực tư pháp
                    // LOAIDONTHU_INDEX = "1.4016." => Loại khiếu nại lĩnh vực tư pháp
                    var objLOAIDONTHU_KHIEUNAI_TUPHAP = objLOAIDONTHU_KHIEUNAI.Where(x => x.LOAIDONTHU_INDEX.StartsWith("1.4016.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    int _TUPHAP = objDonThu.Where(x => objLOAIDONTHU_KHIEUNAI_TUPHAP.Contains((int)x.LOAIDONTHU_ID)).Count();
                    ws.Cells[row, 16].Value = _TUPHAP;

                    //17 Phân loại đơn khiếu nại lĩnh vực đảng đoàn thể
                    // LOAIDONTHU_INDEX = "1.4022." => Loại khiếu nại lĩnh vực  đảng đoàn thể
                    var objLOAIDONTHU_KHIEUNAI_DANG = objLOAIDONTHU_KHIEUNAI.Where(x => x.LOAIDONTHU_INDEX.StartsWith("1.4022.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    int _DANG = objDonThu.Where(x => objLOAIDONTHU_KHIEUNAI_DANG.Contains((int)x.LOAIDONTHU_ID)).Count();
                    ws.Cells[row, 17].Value = _DANG;

                    //18 Phân loại đơn khiếu nại lĩnh vực khác
                    ws.Cells[row, 18].Value = objDonThu.Count() - (_HC_TONG + _TUPHAP + _DANG);

                    //19 Tình trạng đã giải quyết lần đầu
                    ws.Cells[row, 19].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN == 1).Count();

                    //20 Tình trạng đã giải quyết nhiều lần
                    ws.Cells[row, 20].Value = objDonThu.Where(x => x.DAGIAIQUYET_LAN > 1).Count();

                    //21 Tình trạng đã có bản án của tòa
                    ws.Cells[row, 21].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //22 Tình trạng đang giải quyết
                    ws.Cells[row, 22].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 2 || x.DONTHU_TRANGTHAI == 1).Count();

                    //23 Tình trạng chưa giải quyết
                    ws.Cells[row, 23].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 0).Count();

                    //24 KQXL Thuộc thẩm quyền tổng
                    // HUONGXULY_ID = 1 thụ lý giải quyết 
                    ws.Cells[row, 24].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1).Count();

                    //25 KQXL Thuộc thẩm quyền lần đầu
                    ws.Cells[row, 25].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.DAGIAIQUYET_LAN == 1).Count();

                    //26 KQXL Thuộc thẩm quyền lần 2
                    ws.Cells[row, 26].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.DAGIAIQUYET_LAN > 1).Count();

                    //27 KQXL không Thuộc thẩm quyền tổng
                    ws.Cells[row, 27].Value = objDonThu.Where(x => x.HUONGXULY_ID != 1).Count();


                    //28 KQXL không Thuộc thẩm quyền hướng dẫn
                    // HUONGXULY_ID = 2  hướng dẫn 
                    ws.Cells[row, 28].Value = objDonThu.Where(x => x.HUONGXULY_ID == 2).Count();

                    //29 KQXL không Thuộc thẩm quyền chuyển đơn
                    // HUONGXULY_ID = 3 chuyển đơn 
                    ws.Cells[row, 29].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3).Count();

                    //30 KQXL không Thuộc thẩm quyền đôn đốc giải quyết
                    // HUONGXULY_ID = 4 đôn đốc giải quyết 
                    ws.Cells[row, 30].Value = objDonThu.Where(x => x.HUONGXULY_ID == 4).Count();

                    //31 Số văn bản phúc đáp nhận được do chuyển đơn
                    ws.Cells[row, 31].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3 && x.KETQUA_XYLY == true).Count();
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê kết quả xử lý đơn tố cáo
        public Byte[] KQ_XULYDON_TOCAO(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;
            Byte[] fileBytes = null;
            // LOAIDONTHU_ID = "2004" => Loại Tố cáo       

            List<DONTHU> objDonThu = new List<DONTHU>();
            var objDonThu_TrongKy = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(pTuNgay).Date
                                                && x.NGAYTAO <= Convert.ToDateTime(pDenNgay).Date
                                                && x.LOAIDONTHU_CHA_ID == 2004
                                                ).ToList();
            // Tổng đơn kỳ trước chuyển sang
            var objDonThu_KyTruoc = vDC.DONTHUs.Where(x => (x.NGAYTAO < Convert.ToDateTime(pTuNgay).Date && x.DONTHU_TRANGTHAI != 3 && x.DONTHU_TRANGTHAI != 4)
                                                        || (x.DONTHU_TRANGTHAI == 3 && x.KETQUA_NGAY >= Convert.ToDateTime(pTuNgay).Date && x.NGAYTAO <= Convert.ToDateTime(pTuNgay).Date)).ToList();
            objDonThu_KyTruoc = objDonThu_KyTruoc.Where(x => x.LOAIDONTHU_CHA_ID == 2004).ToList();

            objDonThu.AddRange(objDonThu_TrongKy);
            objDonThu.AddRange(objDonThu_KyTruoc);

            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "03_XLD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    // Đơn thư đối tượng                
                    // Số liệu báo cáo từ ngày đến ngày
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    //2 Tổng số đơn
                    ws.Cells[row, 2].Value = objDonThu.Count();

                    //3. Kỳ trước chuyển sang nhiều người đứng tên
                    ws.Cells[row, 3].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //4. Kỳ trước chuyển sang một người đứng tên
                    ws.Cells[row, 4].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //5. Kỳ trước chuyển sang đơn khác
                    ws.Cells[row, 5].Value = objDonThu_KyTruoc.Where(x => x.DONTHU_NACDANH == true).Count();
                    //6 Tiếp nhận trong kỳ nhiều người đứng tên
                    ws.Cells[row, 6].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //7 Tiếp nhận trong kỳ một người đứng tên
                    ws.Cells[row, 7].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //8 Tiếp nhận trong kỳ đơn khác
                    ws.Cells[row, 8].Value = objDonThu_TrongKy.Where(x => x.DONTHU_NACDANH == true).Count();

                    //0: chưa xử lý, 1: đã có hướng xử lý, 2: gửi giải quyết đơn thư, 3 đã có kết quả giải quyết, 4 Kết thúc đơn
                    // 9 Số đơn đã xử lý tổng
                    ws.Cells[row, 9].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    // 10 Số đơn đã xử lý kỳ trước chuyển sang
                    ws.Cells[row, 10].Value = objDonThu_KyTruoc.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    // 11 Số đơn đã xử lý trong kỳ
                    ws.Cells[row, 11].Value = objDonThu_TrongKy.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //12 Đơn đủ điều kiện xử lý số đơn
                    ws.Cells[row, 12].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();


                    //13 Đơn đủ điều kiện xử lý số vụ việc
                    ws.Cells[row, 13].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    // |||||| Phân loại vụ việc theo nội dung |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                    // Loại đơn thư tố cáo
                    // LOAIDONTHU_INDEX = "2004." => Loại khiếu nại
                    var objLOAIDONTHU_TOCAO = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_INDEX.StartsWith("2004.")).ToList();

                    //14 Phân loại đơn tố cáo lĩnh vực hành chính -> tổng
                    // LOAIDONTHU_INDEX = "2004.4026." => Loại khiếu nại lĩnh vực hành chính
                    var objLOAIDONTHU_TOCAO_HANHCHINH = objLOAIDONTHU_TOCAO.Where(x => x.LOAIDONTHU_INDEX.StartsWith("2004.4026.")).Select(x => x.LOAIDONTHU_ID).ToList();
                    ws.Cells[row, 14].Value = objDonThu.Where(x => objLOAIDONTHU_TOCAO_HANHCHINH.Contains((int)x.LOAIDONTHU_ID)).Count();

                    //15 Phân loại đơn tố cáo lĩnh vực hành chính -> Chế độ chính sách 
                    // LOAIDONTHU_ID = "4027." => khiếu nại lĩnh vực hành chính -> Chế độ chính sách

                    ws.Cells[row, 15].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4027).Count();
                    //16 Phân loại đơn tố cáo lĩnh vực hành chính -> Đất đai nhà cửa 
                    // LOAIDONTHU_ID = "4028." => khiếu nại lĩnh vực hành chính -> Đất đai nhà cửa

                    ws.Cells[row, 16].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4028).Count();

                    //17 Phân loại đơn tố cáo lĩnh vực hành chính -> Công chức công vụ 
                    // LOAIDONTHU_ID = "4025." => Tố cáo lĩnh vực hành chính -> Công chức công vụ                
                    ws.Cells[row, 17].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4025).Count();

                    //18 Phân loại đơn tố cáo lĩnh vực hành chính -> khác
                    ws.Cells[row, 18].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4029).Count(); ;

                    //19 Phân loại tố cáo tham nhũng
                    // LOAIDONTHU_INDEX = "2006" => Loại tố cáo tham nhũng  

                    ws.Cells[row, 19].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 2006).Count(); ;

                    //20 Phân loại tố cáo nại lĩnh vực tư pháp
                    // LOAIDONTHU_ID = "2005." => Loại tố cáo lĩnh vực tư pháp

                    ws.Cells[row, 20].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 2005).Count(); ;

                    //21 Phân loại đơn khiếu nại lĩnh vực đảng đoàn thể
                    // LOAIDONTHU_INDEX = "2004.4030." => Loại khiếu nại lĩnh vực đảng đoàn thể

                    ws.Cells[row, 21].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4030).Count();

                    //22 Phân loại đơn tố cáo lĩnh vực khác
                    ws.Cells[row, 22].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4033).Count();



                    // |||||||||Phâ loại vụ việc theo tình trạng giải quyết ||||||||||||||||||||||||||||||||||||||||||||||||||||||||

                    //23 chưa giải quyết trong hạn
                    ws.Cells[row, 23].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 0).Count();

                    //24 đang giải quyết
                    ws.Cells[row, 24].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 1 || x.DONTHU_TRANGTHAI == 2).Count();

                    //25 quá hạn chưa giải quyết
                    ws.Cells[row, 25].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 0).Count();

                    //26 đã có kết luận giải quyết
                    ws.Cells[row, 26].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();


                    // ||||| Kết quả xử lý ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                    //27 KQXL Thuộc thẩm quyền tổng
                    // HUONGXULY_ID = 1 thụ lý giải quyết 
                    ws.Cells[row, 27].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1).Count();

                    //28 KQXL Thuộc thẩm quyền lần đầu
                    ws.Cells[row, 28].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.DAGIAIQUYET_LAN == 1).Count();

                    //29 KQXL Thuộc thẩm quyền lần tiếp
                    ws.Cells[row, 29].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1 && x.DAGIAIQUYET_LAN > 1).Count();

                    //30 KQXL không Thuộc thẩm quyền tổng                   
                    ws.Cells[row, 30].Value = objDonThu.Where(x => x.HUONGXULY_ID != 1).Count();

                    //31 KQXL không Thuộc thẩm quyền hướng dẫn
                    // HUONGXULY_ID = 2  hướng dẫn 
                    ws.Cells[row, 31].Value = objDonThu.Where(x => x.HUONGXULY_ID == 2).Count();

                    //32 KQXL không Thuộc thẩm quyền chuyển đơn
                    // HUONGXULY_ID = 3 chuyển đơn 
                    ws.Cells[row, 32].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3).Count();

                    //33 KQXL không Thuộc thẩm quyền đôn đốc giải quyết
                    // HUONGXULY_ID = 4 đôn đốc giải quyết 
                    ws.Cells[row, 33].Value = objDonThu.Where(x => x.HUONGXULY_ID == 4).Count();

                    //34 Số văn bản phúc đáp nhận được do chuyển đơn
                    ws.Cells[row, 34].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3 && x.KETQUA_XYLY == true).Count();
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê kết quả xử lý đơn phản ánh kiến nghị
        public Byte[] KQ_XULYDON_PAKN(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;
            Byte[] fileBytes = null;
            // LOAIDONTHU_ID = "4035" => Kiến nghị phản ánh       
            List<DONTHU> objDonThu = new List<DONTHU>();
            var objDonThu_TrongKy = vDC.DONTHUs.Where(x => x.NGAYTAO >= Convert.ToDateTime(pTuNgay).Date
                                                && x.NGAYTAO <= Convert.ToDateTime(pDenNgay).Date
                                                && x.LOAIDONTHU_CHA_ID == 4035
                                                ).ToList();
            // Tổng đơn kỳ trước chuyển sang
            var objDonThu_KyTruoc = vDC.DONTHUs.Where(x => (x.NGAYTAO < Convert.ToDateTime(pTuNgay).Date && x.DONTHU_TRANGTHAI != 3 && x.DONTHU_TRANGTHAI != 4)
                                                        || (x.DONTHU_TRANGTHAI == 3 && x.KETQUA_NGAY >= Convert.ToDateTime(pTuNgay).Date && x.NGAYTAO <= Convert.ToDateTime(pTuNgay).Date)).ToList();
            objDonThu_KyTruoc = objDonThu_KyTruoc.Where(x => x.LOAIDONTHU_CHA_ID == 4035).ToList();

            objDonThu.AddRange(objDonThu_TrongKy);
            objDonThu.AddRange(objDonThu_KyTruoc);

            if (objDonThu.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "04_XLD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    // Số liệu báo cáo từ ngày đến ngày
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    // Đơn thư đối tượng                
                    //2 Tổng số đơn
                    ws.Cells[row, 2].Value = objDonThu.Count();

                    //3. Kỳ trước chuyển sang nhiều người đứng tên
                    ws.Cells[row, 3].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //4. Kỳ trước chuyển sang một người đứng tên
                    ws.Cells[row, 4].Value = objDonThu_KyTruoc.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //5. Kỳ trước chuyển sang đơn khác
                    ws.Cells[row, 5].Value = objDonThu_KyTruoc.Count();

                    //6 Tiếp nhận trong kỳ nhiều người đứng tên
                    ws.Cells[row, 6].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI > 1).Count();

                    //7 Tiếp nhận trong kỳ một người đứng tên
                    ws.Cells[row, 7].Value = objDonThu_TrongKy.Where(x => x.DOITUONG != null && x.DOITUONG.DOITUONG_SONGUOI == 1).Count();

                    //8 Tiếp nhận trong kỳ đơn khác
                    ws.Cells[row, 8].Value = objDonThu_TrongKy.Count();

                    //0: chưa xử lý, 1: đã có hướng xử lý, 2: gửi giải quyết đơn thư, 3 đã có kết quả giải quyết, 4 Kết thúc đơn
                    // 9 Số đơn đã xử lý tổng
                    ws.Cells[row, 9].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    // 10 Số đơn đã xử lý kỳ trước chuyển sang
                    ws.Cells[row, 10].Value = objDonThu_KyTruoc.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    // 11 Số đơn đã xử lý trong kỳ
                    ws.Cells[row, 11].Value = objDonThu_TrongKy.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //12 Đơn đủ điều kiện xử lý số đơn
                    ws.Cells[row, 12].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();


                    //13 Đơn đủ điều kiện xử lý số vụ việc
                    ws.Cells[row, 13].Value = objDonThu.Where(x => x.DONTHU_KHONGDUDDIEUKIEN == false).Count();

                    //14 Phân loại đơn thư phản ánh kiến nghị -> Chế độ chính sách
                    // LOAIDONTHU_ID = 4036 => Chế độ chính sách

                    ws.Cells[row, 14].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4036).Count();

                    //15 Phân loại đơn thư Phản ánh kiến nghị  -> đất đai 
                    // LOAIDONTHU_ID = "4045." => Phản ánh kiến nghị -> đất đai                  
                    ws.Cells[row, 15].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4045).Count();

                    //16 Phân loại đơn thư phản ánh kiến nghị -> tư pháp
                    // LOAIDONTHU_ID = "4043." =>  phản ánh kiến nghị -> tư pháp                  
                    ws.Cells[row, 16].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4043).Count();

                    //17 Phân loại đơn thư phản ánh kiến nghị -> khác
                    // LOAIDONTHU_ID = "4050." => phản ánh kiến nghị -> khác               
                    ws.Cells[row, 17].Value = objDonThu.Where(x => x.LOAIDONTHU_ID == 4050).Count();

                    //18 Tình trạng đã được giải quyết
                    ws.Cells[row, 18].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 3).Count();

                    //19 Tình trạng đang được giải quyết
                    ws.Cells[row, 19].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 2 || x.DONTHU_TRANGTHAI == 1).Count();

                    //20 Tình trạng chưa được giải quyết
                    ws.Cells[row, 20].Value = objDonThu.Where(x => x.DONTHU_TRANGTHAI == 0).Count();

                    //21 KQXL Thuộc thẩm quyền tổng
                    // HUONGXULY_ID = 1 thụ lý giải quyết 
                    ws.Cells[row, 21].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1).Count();


                    //22 Không thuộc thẩm quyền tổng
                    ws.Cells[row, 22].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3 || x.HUONGXULY_ID == 4).Count();
                    //23 KQXL không Thuộc thẩm quyền chuyển đơn
                    // HUONGXULY_ID = 3 chuyển đơn 
                    ws.Cells[row, 23].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3).Count();

                    //24 KQXL không Thuộc thẩm quyền đôn đốc giải quyết
                    // HUONGXULY_ID = 24 đôn đốc giải quyết 
                    ws.Cells[row, 24].Value = objDonThu.Where(x => x.HUONGXULY_ID == 4).Count();

                    //25 Số văn bản phúc đáp nhận được do chuyển đơn
                    ws.Cells[row, 25].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3 && x.KETQUA_XYLY == true).Count();
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region Thống kê kết quả phân loại, xử lý đơn qua tiếp công dân
        public Byte[] KQ_PHANLOAI_XULY_TIEPDAN(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;
            Byte[] fileBytes = null;
            List<TIEPDAN> objTiepDan = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= Convert.ToDateTime(pTuNgay).Date
                                                && x.TIEPDAN_THOGIAN <= Convert.ToDateTime(pDenNgay).Date
                                                ).ToList();

            var objDONTHU_ID = objTiepDan.Where(x => x.DONTHU_ID != null).Select(x => x.DONTHU_ID).ToList();
            List<DONTHU> objDonThu = vDC.DONTHUs.Where(x => objDONTHU_ID.Contains(x.DONTHU_ID)).ToList();
            if (objTiepDan.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "02_TCD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    // Đơn thư đối tượng                

                    // Số liệu báo cáo từ ngày đến ngày
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    //2 Tổng số đơn nhận được qua tiếp công dân
                    ws.Cells[row, 2].Value = objDonThu.Count();

                    //3. Tổng số vụ việc được tiếp
                    ws.Cells[row, 3].Value = objTiepDan.Count();

                    // ***************************** Phân loại theo nội dung*********************************************
                    //4. Khiếu nại số đơn
                    ws.Cells[row, 4].Value = objDonThu.Where(x => x.LOAIDONTHU_CHA_ID == 1).Count();

                    //5. Khiếu nại số vụ việc
                    ws.Cells[row, 5].Value = objTiepDan.Where(x => x.TIEPDAN_LOAI_CHA_ID == 1).Count();

                    //6 Tố cáo số đơn
                    ws.Cells[row, 6].Value = objDonThu.Where(x => x.LOAIDONTHU_CHA_ID == 2004).Count();

                    //7 Tố cáo số vụ việc
                    ws.Cells[row, 7].Value = objTiepDan.Where(x => x.TIEPDAN_LOAI_CHA_ID == 2004).Count();

                    //8 Phản ánh kiến nghị số đơn
                    ws.Cells[row, 8].Value = objDonThu.Where(x => x.LOAIDONTHU_CHA_ID == 4035).Count();

                    // 9 Phản ánh kiến nghị số vụ việc
                    ws.Cells[row, 9].Value = objTiepDan.Where(x => x.TIEPDAN_LOAI_CHA_ID == 4035).Count();

                    //***************** Phân loại theo thẩm quyền *************************************
                    //0: chưa xử lý, 1: đã có hướng xử lý, 2: gửi giải quyết đơn thư, 3 đã có kết quả giải quyết, 4 Kết thúc đơn
                    // 10 Thuộc thẩm quyền số đơn
                    ws.Cells[row, 10].Value = objDonThu.Where(x => x.HUONGXULY_ID == 1).Count();

                    // 11 Thuộc thẩm quyền số vụ việc
                    int _SODON_KHONGTHUOCTHAMQUYEN = objDonThu.Where(x => x.HUONGXULY_ID != 1).Count();
                    ws.Cells[row, 11].Value = objTiepDan.Count() - _SODON_KHONGTHUOCTHAMQUYEN;

                    //12 Không thuộc thẩm quyền số đơn
                    ws.Cells[row, 12].Value = _SODON_KHONGTHUOCTHAMQUYEN;

                    //13 Không thuộc thẩm quyền số vụ việc tổng                 
                    ws.Cells[row, 13].Value = _SODON_KHONGTHUOCTHAMQUYEN;

                    //14 Không thuộc thẩm quyền số vụ việc hướng dẫn
                    // LOAIDONTHU_ID = 4036 => Chế độ chính sách

                    ws.Cells[row, 14].Value = objDonThu.Where(x => x.HUONGXULY_ID == 2).Count();

                    //15 Không thuộc thẩm quyền số vụ việc chuyển đơn                                   
                    ws.Cells[row, 15].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3).Count();

                    //16 Không thuộc thẩm quyền số vụ việc đôn đốc giải quyết                                
                    ws.Cells[row, 16].Value = objDonThu.Where(x => x.HUONGXULY_ID == 4).Count();

                    //17 Số văn bản phúc đáp nhận được do chuyển đơn
                    ws.Cells[row, 17].Value = objDonThu.Where(x => x.HUONGXULY_ID == 3 && x.KETQUA_XYLY == true).Count();

                    //18 Ghi chú
                    ws.Cells[row, 18].Value = "";

                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion
        #region Thống kê tổng hợp kết quả tiếp công dân thường xuyên, định kỳ và đột xuất
        public Byte[] KQ_TIEPDAN(string pTuNgay, string pDenNgay, string pKyBaoCao)
        {
            int row = 16;
            Byte[] fileBytes = null;
            List<TIEPDAN> objTiepDan = vDC.TIEPDANs.Where(x => x.TIEPDAN_THOGIAN >= Convert.ToDateTime(pTuNgay).Date
                                                && x.TIEPDAN_THOGIAN <= Convert.ToDateTime(pDenNgay).Date
                                                ).ToList();

            //var objDONTHU_ID = objTiepDan.Where(x => x.DONTHU_ID != null).Select(x => x.DONTHU_ID).ToList();
            //List<DONTHU> objDonThu = vDC.DONTHUs.Where(x => objDONTHU_ID.Contains(x.DONTHU_ID)).ToList();
            if (objTiepDan.Count > 0)
            {
                var ExistFile = System.Web.HttpContext.Current.Server.MapPath(ClassParameter.vPathBieuMau) + "01_TCD.xlsx";
                var File = new FileInfo(ExistFile);
                using (ExcelPackage pck = new ExcelPackage(File))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                    // Đơn thư đối tượng                

                    // Số liệu báo cáo từ ngày đến ngày
                    ws.Cells[6, 1].Value = pKyBaoCao;
                    ws.Cells[8, 1].Value = "Số liệu tính từ ngày " + pTuNgay + " đến ngày " + pDenNgay;

                    //2 Tổng số lượt tiếp
                    ws.Cells[row, 2].Value = objTiepDan.Select(x => x.TIEPDAN_LANTIEP).Sum();

                    //3. Tổng số người được tiếp
                    ws.Cells[row, 3].Value = objTiepDan.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();


                    //4. Tổng số vụ việc tiếp
                    ws.Cells[row, 4].Value = objTiepDan.Count();

                    // ***************************** Tiếp dân thường xuyên qua ban tiếp công dân*********************************************
                    // Tiếp dân thường xuyên qua ban tiếp công dân
                    var objTiepDan_ThuongXuyen = objTiepDan.Where(x => x.TIEPDAN_BTD == null || x.TIEPDAN_BTD == 0).ToList();


                    //5. Tiếp dân thường xuyên số lượt tiếp
                    ws.Cells[row, 5].Value = objTiepDan_ThuongXuyen.Select(x => x.TIEPDAN_LANTIEP).Sum();

                    //6 Tiếp dân thường xuyên số người được tiếp
                    ws.Cells[row, 6].Value = objTiepDan_ThuongXuyen.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    //7 Số vụ việc tiếp lần đầu
                    ws.Cells[row, 7].Value = objTiepDan_ThuongXuyen.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //8 Số vụ việc tiếp nhiều lần
                    ws.Cells[row, 8].Value = objTiepDan_ThuongXuyen.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    // 9 Đoàn đông người số đoàn được tiếp
                    // Danh sách tiếp dân đoàn đông người
                    var objTiepDan_ThuongXuyen_DoanDongNguoi = objTiepDan_ThuongXuyen.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();

                    ws.Cells[row, 9].Value = objTiepDan_ThuongXuyen_DoanDongNguoi.Count();

                    // 10 Đoàn đông người số người được tiếp
                    ws.Cells[row, 10].Value = objTiepDan_ThuongXuyen_DoanDongNguoi.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    // 11 Đoàn đông người số người tiếp lần đầu                   
                    ws.Cells[row, 11].Value = objTiepDan_ThuongXuyen_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //12 Đoàn đông người số người tiếp nhiều lần  
                    ws.Cells[row, 12].Value = objTiepDan_ThuongXuyen_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    // ***************************** Thủ trưởng tiếp*********************************************
                    // Thủ trưởng tiếp công dân
                    var objTiepDan_ThuTruong = objTiepDan.Where(x => x.TIEPDAN_BTD == 1).ToList();
                    //13. Thủ trưởng tiếp công dân số kỳ tiếp
                    ws.Cells[row, 13].Value = objTiepDan_ThuTruong.Count();

                    //14. Thủ trưởng tiếp công dân số lượt tiếp
                    ws.Cells[row, 14].Value = objTiepDan_ThuTruong.Select(x => x.TIEPDAN_LANTIEP).Sum();

                    //15 Thủ trưởng tiếp công dân số người được tiếp
                    ws.Cells[row, 15].Value = objTiepDan_ThuTruong.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    //16 Số vụ việc tiếp lần đầu
                    ws.Cells[row, 16].Value = objTiepDan_ThuTruong.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //17 Số vụ việc tiếp nhiều lần
                    ws.Cells[row, 17].Value = objTiepDan_ThuTruong.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    // 18 Đoàn đông người số đoàn được tiếp
                    // Danh sách tiếp dân đoàn đông người
                    var objTiepDan_ThuTruong_DoanDongNguoi = objTiepDan_ThuTruong.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();

                    ws.Cells[row, 18].Value = objTiepDan_ThuTruong_DoanDongNguoi.Count();

                    // 19 Đoàn đông người số người được tiếp
                    ws.Cells[row, 19].Value = objTiepDan_ThuTruong_DoanDongNguoi.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    // 20 Đoàn đông người số người tiếp lần đầu                   
                    ws.Cells[row, 20].Value = objTiepDan_ThuTruong_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //21 Đoàn đông người số người tiếp nhiều lần  
                    ws.Cells[row, 21].Value = objTiepDan_ThuTruong_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    // ***************************** Ủy quyền tiếp*********************************************
                    // Ủy quyền tiếp
                    var objTiepDan_UyQuyen = objTiepDan.Where(x => x.TIEPDAN_BTD == 1).ToList();

                    //22. Ủy quyền tiếp số kỳ tiếp
                    ws.Cells[row, 22].Value = objTiepDan_UyQuyen.Count;

                    //23. Ủy quyền tiếp số lượt tiếp
                    ws.Cells[row, 23].Value = objTiepDan_UyQuyen.Select(x => x.TIEPDAN_LANTIEP).Sum();

                    //24 Ủy quyền tiếp số người được tiếp
                    ws.Cells[row, 24].Value = objTiepDan_UyQuyen.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    //25 Số vụ việc tiếp lần đầu
                    ws.Cells[row, 25].Value = objTiepDan_UyQuyen.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //26 Số vụ việc tiếp nhiều lần
                    ws.Cells[row, 26].Value = objTiepDan_UyQuyen.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    // 27 Đoàn đông người số đoàn được tiếp
                    // Danh sách tiếp dân đoàn đông người
                    var objTiepDan_UyQuyen_DoanDongNguoi = objTiepDan_UyQuyen.Where(x => x.DOITUONG.DOITUONG_LOAI == 2).ToList();

                    ws.Cells[row, 27].Value = objTiepDan_UyQuyen_DoanDongNguoi.Count();

                    // 28 Đoàn đông người số người được tiếp
                    ws.Cells[row, 28].Value = objTiepDan_UyQuyen_DoanDongNguoi.Select(x => x.DOITUONG.DOITUONG_SONGUOI).Sum();

                    // 29 Đoàn đông người số người tiếp lần đầu                   
                    ws.Cells[row, 29].Value = objTiepDan_UyQuyen_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP == 1).Count();

                    //30 Đoàn đông người số người tiếp nhiều lần  
                    ws.Cells[row, 30].Value = objTiepDan_UyQuyen_DoanDongNguoi.Where(x => x.TIEPDAN_LANTIEP > 1).Count();

                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion
        public string GetTrangThai(int HXL_ID)
        {
            string _TrangThai = "";
           
            if  (HXL_ID ==1 || HXL_ID ==3 || HXL_ID ==4 || HXL_ID ==5 || HXL_ID ==10 || HXL_ID == 12)
            {
                _TrangThai = "Đang xử lý";
            }else if (HXL_ID == 2 || HXL_ID == 6 || HXL_ID == 7 || HXL_ID == 8 || HXL_ID == 9)
            {
                _TrangThai = "Đơn thư kết thúc";
            }
            else
            {
                _TrangThai = "Chưa xử lý";
            }
        
       
           return _TrangThai;
        }

        public string GetYKienXuLy(int DT_ID)
        {
            string _Re = "";
            var objDonThu = vDC.DONTHUs.Where(x => x.DONTHU_ID == DT_ID).FirstOrDefault();
            if (objDonThu !=null)
            {
                _Re = objDonThu.HUONGXULY_YKIEN_XULY;
            }
            return _Re;

        }
    }
}
