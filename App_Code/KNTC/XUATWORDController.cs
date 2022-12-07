using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DotNetNuke.Instrumentation;
using DocumentFormat.OpenXml;
using System.Globalization;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace KNTC
{
    public class XUATWORDController : DotNetNuke.Entities.Modules.UserModuleBase
    {
        #region Khai Bao Doi Tuong
        private readonly ILog log = LoggerSource.Instance.GetLogger(typeof(XUATWORDController).FullName);
        DonThuController vDonThuController = new DonThuController();
        KNTCDataContext vDC = new KNTCDataContext();
        int _LoaiDonNhieuNoiDung = 4051;
        #endregion
        public XUATWORDController()
        {
        }  
        public List<byte[]> XuatPhieuDonThu(List<int> DONTHU_IDs, string PathBieuMau_MapPath, string sourceFile, string sourceFilePatch, out List<string> ResponseFileNames)
        {
            ResponseFileNames = new List<string>();
            List<byte[]> phieu = new List<byte[]>();
            int count = DONTHU_IDs.Count;
            for (int DONTHU_ID_item = 0; DONTHU_ID_item < DONTHU_IDs.Count; DONTHU_ID_item++)
            {
                //int vLien = 1;
                int vDONTHU_ID = DONTHU_IDs[DONTHU_ID_item];
                string ResponseFileName = "";
                var objDONTHU_BY_ID = vDC.DONTHUs.Where(x => x.DONTHU_ID == vDONTHU_ID).SingleOrDefault();
                byte[] byteArray = null;
                // Chuyển đơn
                if (objDONTHU_BY_ID.HUONGXULY_ID == 3)
                {
                    if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID ==ClassParameter.vToCao_ID) //Chuyển đơn tố cáo
                    {
                        sourceFilePatch = sourceFilePatch + "\\ChuyenDon_ToCao.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                      
                    }
                    else if(objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vPAKN_ID) // Chuyển đơn phản ánh kiến nghị
                    {
                        sourceFilePatch = sourceFilePatch + "\\ChuyenDon_PAKN.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }
                    else
                    {
                        sourceFilePatch = sourceFilePatch + "\\Chuyendon_mau_01.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }                
                }
                // Hướng dẫn
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 2)
                {
                    // Hướng dẫn đơn thư có nhiều nội dung
                    if(objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == _LoaiDonNhieuNoiDung)
                    {
                        sourceFilePatch = sourceFilePatch + "\\HuongDan_NhieuNoiDung.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }
                    else
                    {
                        sourceFilePatch = sourceFilePatch + "\\Huongdandon_mau_01.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }
                  
                }
                // Thụ lý giải quyết
                else if(objDONTHU_BY_ID.HUONGXULY_ID == 1)
                {
                    sourceFilePatch = sourceFilePatch + "\\PhieuDeXuat_ThuLyDon.docx";
                    byteArray = File.ReadAllBytes(sourceFilePatch);
                }
                // Trả đơn
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 7)
                {
                    sourceFilePatch = sourceFilePatch + "\\Tradon_mau_01.docx";
                    byteArray = File.ReadAllBytes(sourceFilePatch);
                }
                // Ra thông báo thụ lý
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 5)
                {
                    if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vToCao_ID) //Thụ lý đơn tố cáo
                    {
                        sourceFilePatch = sourceFilePatch + "\\ThuLyToCao.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }
                    else if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vKhieuNai_ID) // thụ lý đơn khiếu nại
                    {
                        sourceFilePatch = sourceFilePatch + "\\ThuLyKhieuNai.docx";
                        byteArray = File.ReadAllBytes(sourceFilePatch);
                    }
                }
                // Từ chối thụ lý
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 8)
                {
                    sourceFilePatch = sourceFilePatch + "\\TuChoiThuLy.docx";
                    byteArray = File.ReadAllBytes(sourceFilePatch);
                }
                // Ra văn bản đôn đốc
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 4)
                {
                    sourceFilePatch = sourceFilePatch + "\\VanBanDonDoc.docx";
                    byteArray = File.ReadAllBytes(sourceFilePatch);
                }
                // Ra công văn giao đơn vị xử lý
                else if (objDONTHU_BY_ID.HUONGXULY_ID == 12)
                {
                    sourceFilePatch = sourceFilePatch + "\\CongVanGiao.docx";
                    byteArray = File.ReadAllBytes(sourceFilePatch);
                }

                MemoryStream mem = new MemoryStream();
                mem.Write(byteArray, 0, (int)byteArray.Length);

                using (var wordDoc = WordprocessingDocument.Open(mem, true))
                {
                    var mergeFields = wordDoc.MainDocumentPart.RootElement.Descendants<FieldCode>();                  
                    var DictionaryKeyWWord = new Dictionary<string, string>();
                    // Chuyển đơn
                    if (objDONTHU_BY_ID.HUONGXULY_ID==3)
                    {
                        if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vToCao_ID) //Chuyển đơn tố cáo
                        {
                            DictionaryKeyWWord = getDictionaryPhieuChuyen_ToCao(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "ChuyenDon";
                        }
                        else if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vPAKN_ID) // Chuyển đơn phản ánh kiến nghị
                        {
                            DictionaryKeyWWord = getDictionaryPhieuChuyen_PAKN(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "ChuyenDon";
                        }
                        else
                        {
                            DictionaryKeyWWord = getDictionaryPhieuChuyen(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "ChuyenDon";
                        }                      
                    }
                    // Hướng dẫn
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 2)
                    {
                        if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == _LoaiDonNhieuNoiDung)
                        {
                            DictionaryKeyWWord = getDictionaryPhieuHuongDan_NhieuNoiDung(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "Huongdandon";
                            fillNhieuNoiDung(wordDoc, (int)objDONTHU_BY_ID.DONTHU_ID);
                        }
                        else
                        {
                            DictionaryKeyWWord = getDictionaryPhieuHuongDan(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "Huongdandon";
                        }                   
                    }
                    // Thụ lý giải quyết
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 1)
                    {
                        DictionaryKeyWWord = getDictionaryPhieuDeXuat_ThuLyDon(objDONTHU_BY_ID);
                        ResponseFileName = ResponseFileName + "ThuLyGiaiQuyet";
                    }
                    // Ra thông báo thụ lý
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 5)
                    {
                        if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vToCao_ID) //Thụ lý đơn tố cáo
                        {
                            DictionaryKeyWWord = getDictionary_ThuLyToCao(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "ThuLyDonToCao";                        
                        }
                        else if (objDONTHU_BY_ID.LOAIDONTHU_CHA_ID == ClassParameter.vKhieuNai_ID) // thụ lý đơn khiếu nại
                        {
                            DictionaryKeyWWord = getDictionary_ThuLyKhieuNai(objDONTHU_BY_ID);
                            ResponseFileName = ResponseFileName + "ThuLyKhieuNai";                       
                        }
                    }
                    // Trả đơn
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 7)
                    {
                        DictionaryKeyWWord = getDictionaryPhieuTradon(objDONTHU_BY_ID);
                        ResponseFileName = ResponseFileName + "Tradon";
                    }
                    // Từ chối thụ lý
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 8)
                    {
                        DictionaryKeyWWord = getDictionary_TuChoiThuLy(objDONTHU_BY_ID);
                        
                        ResponseFileName = ResponseFileName + "TuChoiThuLy";
                    }
                    // Ra văn bản đôn đốc
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 4)
                    {
                        DictionaryKeyWWord = getDictionary_VanBanDonDoc(objDONTHU_BY_ID);

                        ResponseFileName = ResponseFileName + "VanBanDonDoc";
                    }
                    // Ra công văn giao đơn vị xử lý
                    else if (objDONTHU_BY_ID.HUONGXULY_ID == 12)
                    {
                        DictionaryKeyWWord = getDictionary_RaCongVanGiao(objDONTHU_BY_ID);

                        ResponseFileName = ResponseFileName + "RaCongVanGiao";
                    }
                    foreach (var item in objDONTHU_BY_ID.DOITUONG.CANHANs)
                    {
                        ResponseFileName = ResponseFileName + ClassCommon.BoDauTiengViet(item.CANHAN_HOTEN).Replace(" ", "");
                    }
                    ResponseFileName = ResponseFileName + ".docx";
                    ResponseFileNames.Add(ResponseFileName);

                    foreach (var key in DictionaryKeyWWord)
                    {
                        ReplaceMergeFieldWithText(mergeFields, key.Key, key.Value);
                    }            
                    string sourceFile_SaveAs = PathBieuMau_MapPath+"\\Export\\"+ ResponseFileName ;
                    File.WriteAllBytes(sourceFile_SaveAs, mem.ToArray());

                    //wordDoc.MainDocumentPart.Document.Save();
                    wordDoc.Close();
                }
                phieu.Add(mem.ToArray());
                //}
                break;
            }
            //byte[] allData = OpenAndCombine(phieu);
            return phieu;
        }

        private Dictionary<string, string> getDictionaryPhieuChuyen(DONTHU objDONTHU)
        {
            try
            {              
                DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH==true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú "+ CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                }
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length-2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length-2);

                if (COQUAN_CHUYEN!=null)
                {
                    var keyValues = new Dictionary<string, string> {
                            //{ @"QR_CODE", fileQR_code},
                            //{ @"PXN_SO", SecurityElement.Escape(((int)objPSW_PHIEUXUATNHAP.PXN_SO_GIAODICH).ToString("0000000")) },//pxn_so
                            
                            { @"VANBAN_SO", SecurityElement.Escape("") },//SỐ VĂN BẢN
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"COQUAN_CHUYEN_TEN",SecurityElement.Escape(COQUAN_CHUYEN.TENDONVI) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DONTHU",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },
                            { @"DONTHU_NGAYDEDON",SecurityElement.Escape(objDONTHU.NGUONDON_NGAYDEDON !=null ?(DateTime.Parse(objDONTHU.NGUONDON_NGAYDEDON.ToString()).ToString("dd/MM/yyyy")):"") },
                            { @"CANHAN_HOTEN",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                        };
                    return keyValues;
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

        // Phiếu chuyển đơn tố cáo
        private Dictionary<string, string> getDictionaryPhieuChuyen_ToCao(DONTHU objDONTHU)
        {
            try
            {
                DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string NgayDeDon = "";
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
                if (objDONTHU.NGUONDON_NGAYDEDON != null)
                {
                    NgayDeDon = objDONTHU.NGUONDON_NGAYDEDON.Value.Day + " tháng " + objDONTHU.NGUONDON_NGAYDEDON.Value.Month + " năm " + objDONTHU.NGUONDON_NGAYDEDON.Value.Year;
                }
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    //ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    //ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                    ThongTinDoiTuong += ", " + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;
                }
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);

                if (COQUAN_CHUYEN != null)
                {
                    var keyValues = new Dictionary<string, string> {                        
                            { @"VANBAN_SO", SecurityElement.Escape(objDONTHU.HUONGXULY_SOHIEUVB_DI) },//SỐ VĂN BẢN
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"COQUAN",SecurityElement.Escape(COQUAN_CHUYEN.TENDONVI) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DONTHU_NOIDUNG",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },
                            { @"NGAYDEDON",SecurityElement.Escape(NgayDeDon) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },                         
                            { @"CANHAN",SecurityElement.Escape(ThongTinDoiTuongHoTen) }                            
                        };
                    return keyValues;
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

        // Phiếu chuyển đơn phản ánh kiến nghị
        private Dictionary<string, string> getDictionaryPhieuChuyen_PAKN(DONTHU objDONTHU)
        {
            try
            {
                DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string NgayDeDon = "";
                if(objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
                if (objDONTHU.NGUONDON_NGAYDEDON != null)
                {
                    NgayDeDon = objDONTHU.NGUONDON_NGAYDEDON.Value.Day + " tháng " + objDONTHU.NGUONDON_NGAYDEDON.Value.Month + " năm " + objDONTHU.NGUONDON_NGAYDEDON.Value.Year;
                }

                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    //ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuong += ", "+ CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;
                }
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);

                if (COQUAN_CHUYEN != null)
                {
                    var keyValues = new Dictionary<string, string> {
                            { @"VANBAN_SO", SecurityElement.Escape(objDONTHU.HUONGXULY_SOHIEUVB_DI) },//SỐ VĂN BẢN
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"COQUANGIAIQUYET",SecurityElement.Escape(COQUAN_CHUYEN.TENDONVI) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DONTHU_NOIDUNG",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },
                            { @"NGAYDEDON",SecurityElement.Escape(NgayDeDon) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"HOTEN",SecurityElement.Escape(ThongTinDoiTuongHoTen) }
                        };
                    return keyValues;
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
        private Dictionary<string, string> getDictionaryPhieuDeXuat_ThuLyDon(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";
                //DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                LOAIDONTHU lOAIDONTHU = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string DiaChi = "";
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
               
                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                   
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    ThongTinDoiTuong += ", " + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;
                    //ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    //ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                }
                if (objDONTHU.DOITUONG.CANHANs.Count >1)
                {
                    DiaChi = objDONTHU.DOITUONG.DOITUONG_DIACHI;
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x=>x.CANHAN_DAIDIENUYQUYEN ==true).FirstOrDefault();
                    if (obj != null )
                    {
                        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                    }
                    else
                    {
                        DiaChi = objDONTHU.DOITUONG.CANHANs.FirstOrDefault().CANHAN_DIACHI_DAYDU;
                    }
                }
                else
                {
                    DiaChi = objDONTHU.DOITUONG.CANHANs.FirstOrDefault().CANHAN_DIACHI_DAYDU;
                }
               

                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);
                var keyValues = new Dictionary<string, string> {
                            { @"VANBAN_SO", SecurityElement.Escape("") },//SỐ VĂN BẢN
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"GIAMDOC",SecurityElement.Escape(NGUOIDUYET) },
                            { @"NOIDUNG",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },                           
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"TENDOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"DOITUONG_DIACHI",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"LOAIDONTHU",SecurityElement.Escape(lOAIDONTHU.LOAIDONTHU_TEN) },
                            { @"LOAI_UP",SecurityElement.Escape(lOAIDONTHU.LOAIDONTHU_TEN.ToUpper()) },
                            { @"DIACHI",SecurityElement.Escape(DiaChi) }
            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Dictionary<string, string> getDictionaryPhieuHuongDan(DONTHU objDONTHU)
        {
            try
            {
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";            
                string ThongTinNguonDon = "";                         
                string CoQuanTiepNhan = "";
                string NgayTao = "";
                string NgayDeDon = "";
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
                if (objDONTHU.NGUONDON_NGAYDEDON != null)
                {
                    NgayDeDon = objDONTHU.NGUONDON_NGAYDEDON.Value.Day + " tháng " + objDONTHU.NGUONDON_NGAYDEDON.Value.Month + " năm " + objDONTHU.NGUONDON_NGAYDEDON.Value.Year;
                }

                DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                if (COQUAN_CHUYEN !=null)
                {
                    CoQuanTiepNhan = COQUAN_CHUYEN.TENDONVI;
                }
                LOAIDONTHU lOAIDONTHU_cha = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                if (  objDONTHU.NGUONDON_DONVI_ID!=null)
                {
                    DONVI COQUAN_DA_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.NGUONDON_DONVI_ID).SingleOrDefault();
                    ThongTinNguonDon = ThongTinNguonDon + "thông qua văn bản chuyển đơn";
                    if(objDONTHU.NGUONDON_SOVANBANCHUYEN!=""&& objDONTHU.NGUONDON_SOVANBANCHUYEN != null)
                    {
                        ThongTinNguonDon = ThongTinNguonDon + " số "+ objDONTHU.NGUONDON_SOVANBANCHUYEN;
                    }
                    if ( objDONTHU.NGUONDON_NGAYCHUYEN != null)
                    {
                        ThongTinNguonDon = ThongTinNguonDon + ", ngày " + ((DateTime) objDONTHU.NGUONDON_NGAYCHUYEN).ToString("dd/MM/yyyyy");
                    }
                    ThongTinNguonDon = ThongTinNguonDon + " của " + COQUAN_DA_CHUYEN.TENDONVI;
                  
                }    
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    //ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    //ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                    ThongTinDoiTuong += ", " +  CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;
                }
            
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);

                var keyValues = new Dictionary<string, string> {                          
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },                            
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"CANHAN_HOTEN",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"LOAI_DONTHU_TEN",SecurityElement.Escape(lOAIDONTHU_cha.LOAIDONTHU_TEN.ToLower()) },
                            { @"THONG_TIN_NGUONDON",SecurityElement.Escape(ThongTinNguonDon) },
                            { @"DONTHU_NOIDUNG",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },
                            { @"NGAYDEDON",SecurityElement.Escape(NgayDeDon) },                        
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao)},
                            { @"TEN_NAMSINH",SecurityElement.Escape(ThongTinDoiTuongHoTen)},
                            { @"COQUAN",SecurityElement.Escape(CoQuanTiepNhan)},
                        };
                return keyValues;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Dictionary<string, string> getDictionaryPhieuHuongDan_NhieuNoiDung(DONTHU objDONTHU)
        {
            try
            {
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";                                                    
                string NgayTao = "";
                string NgayDeDon = "";
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
                if (objDONTHU.NGUONDON_NGAYDEDON != null)
                {
                    NgayDeDon = objDONTHU.NGUONDON_NGAYDEDON.Value.Day + " tháng " + objDONTHU.NGUONDON_NGAYDEDON.Value.Month + " năm " + objDONTHU.NGUONDON_NGAYDEDON.Value.Year;
                }
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    //ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    //ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                    ThongTinDoiTuong += ", " +  CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + CANHAN_item.CANHAN_HOTEN;                  
                }
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);

                var keyValues = new Dictionary<string, string> {                          
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"CANHAN_HOTEN",SecurityElement.Escape(ThongTinDoiTuongHoTen) },                                                                               
                            { @"NGAYDEDON",SecurityElement.Escape(NgayDeDon)},
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao)},
                            { @"CANHAN",SecurityElement.Escape(ThongTinDoiTuongHoTen)}
                           
                        };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private Dictionary<string, string> getDictionaryPhieuTradon(DONTHU objDONTHU)
        {
            try
            {
                LOAIDONTHU lOAIDONTHU_cha = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                {
                    ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "Bà " : "Ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                }
                ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);
                var keyValues = new Dictionary<string, string> {
                            { @"VANBAN_SO", SecurityElement.Escape("") },//SỐ VĂN BẢN
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"LOAI_DONTHU_TEN",SecurityElement.Escape(lOAIDONTHU_cha.LOAIDONTHU_TEN.ToLower())},
                            { @"THONGTIN_DOITUONG",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"CANHAN_HOTEN",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"DONTHU_NOIDUNG",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG.Replace("\n","\n\r")) },
                            { @"HUONGXULY_NOIDUNG",SecurityElement.Escape(objDONTHU.HUONGXULY_YKIEN_XULY) },
                            { @"DONTHU_NGAYDEDON",SecurityElement.Escape(((DateTime)objDONTHU.NGUONDON_NGAYDEDON).ToString("dd/MM/yyyy")) }
                        };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Dictionary<string, string> getDictionary_ThuLyKhieuNai(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";                      
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string DiaChi = "";
                string LanTiep = "lần 1";
                string CMND = "";
                string NgayCap = "";
                string NoiCap = "";
                var objTiepDan = vDC.TIEPDANs.Where(x => x.DONTHU_ID == objDONTHU.DONTHU_ID).FirstOrDefault();
                if (objTiepDan !=null)
                {
                    LanTiep = objTiepDan.TIEPDAN_LANTIEP ==null? "lần 1":"lần "+ objTiepDan.TIEPDAN_LANTIEP.ToString();
                }

                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }

                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }
             
                //Nhóm đông người cơ quan tổ chức lấy người đại diện
                if (objDONTHU.DOITUONG.CANHANs.Count > 1)
                {
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                    if (obj != null)
                    {
                        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong =  obj.CANHAN_HOTEN + ", thường trú " + obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = obj.CANHAN_HOTEN;
                        CMND = obj.CANHAN_CMDN;
                        NoiCap = obj.CANHAN_NOICAP;
                        NgayCap = obj.CANHAN_CMDN_NGAYCAP ==null? "" : Convert.ToDateTime(obj.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        var objDefault = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                        DiaChi = objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = objDefault.CANHAN_HOTEN + ", thường trú " + objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = objDefault.CANHAN_HOTEN;
                        CMND = objDefault.CANHAN_CMDN;
                        NoiCap = objDefault.CANHAN_NOICAP;
                        NgayCap = objDefault.CANHAN_CMDN_NGAYCAP == null ? "" : Convert.ToDateTime(objDefault.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                    }
                }
                else if (objDONTHU.DOITUONG.CANHANs.Count ==1)
                {
                    var objCaNhan = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                    DiaChi = objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuong = objCaNhan.CANHAN_HOTEN + ", thường trú " + objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen = objCaNhan.CANHAN_HOTEN;
                    CMND = objCaNhan.CANHAN_CMDN;
                    NoiCap = objCaNhan.CANHAN_NOICAP;
                    NgayCap = objCaNhan.CANHAN_CMDN_NGAYCAP == null ? "" : Convert.ToDateTime(objCaNhan.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                }
                //Cá nhân
                var keyValues = new Dictionary<string, string> {                        
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"NOIDUNGDON",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"HOTEN_DIACHI",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DIACHI",SecurityElement.Escape(DiaChi) },
                            { @"LAN",SecurityElement.Escape(LanTiep) },
                            { @"CMND",SecurityElement.Escape(CMND) },
                            { @"NGAYCAP",SecurityElement.Escape(NgayCap) },
                            { @"NOICAP",SecurityElement.Escape(NoiCap) },
            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Dictionary<string, string> getDictionary_ThuLyToCao(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";           
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string NGUOIBITOCAO = "";
                string NOIDUNGGIAIQUYET = objDONTHU.HUONGXULY_YKIEN_XULY;

                if (objDONTHU.DOITUONG_BI_KNTC_ID != null)
                {
                    var objCANHAN_BITOCAO = vDC.CANHANs.Where(x => x.DOITUONG_ID == objDONTHU.DOITUONG_BI_KNTC_ID).FirstOrDefault();
                    if(objCANHAN_BITOCAO != null)
                    {
                        NGUOIBITOCAO = objCANHAN_BITOCAO.CANHAN_HOTEN;
                    }
                }

                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }

                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }

                //Nhóm đông người cơ quan tổ chức lấy người đại diện
                if (objDONTHU.DOITUONG.CANHANs.Count > 1)
                {
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                    if (obj != null)
                    {                      
                        ThongTinDoiTuongHoTen = obj.CANHAN_HOTEN;    
                    }
                    else
                    {
                        var objDefault = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                        ThongTinDoiTuongHoTen = objDefault.CANHAN_HOTEN;
                    }
                }
                else if (objDONTHU.DOITUONG.CANHANs.Count == 1)
                {
                    var objCaNhan = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();                   
                    ThongTinDoiTuongHoTen = objCaNhan.CANHAN_HOTEN;
                }
                //Cá nhân
                var keyValues = new Dictionary<string, string> {                          
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },                         
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"NGUOIBITOCAO",SecurityElement.Escape(NGUOIBITOCAO) },
                            { @"CANBO",SecurityElement.Escape(NGUOIDUYET) },
                            { @"NOIDUNGGIAIQUYET",SecurityElement.Escape(NOIDUNGGIAIQUYET) }

            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Dictionary<string, string> getDictionary_TuChoiThuLy(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string DiaChi = "";             
                string CMND = "";
                string NgayCap = "";
                string NoiCap = "";
                string LyDo = "";
               
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }

                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }

                LyDo = objDONTHU.HUONGXULY_YKIEN_XULY;
                //Nhóm đông người cơ quan tổ chức lấy người đại diện
                if (objDONTHU.DOITUONG.CANHANs.Count > 1)
                {
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                    if (obj != null)
                    {
                        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = obj.CANHAN_HOTEN + ", thường trú " + obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = obj.CANHAN_HOTEN;
                        CMND = obj.CANHAN_CMDN;
                        NoiCap = obj.CANHAN_NOICAP;
                        NgayCap = obj.CANHAN_CMDN_NGAYCAP == null ? "" : Convert.ToDateTime(obj.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        var objDefault = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                        DiaChi = objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = objDefault.CANHAN_HOTEN + ", thường trú " + objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = objDefault.CANHAN_HOTEN;
                        CMND = objDefault.CANHAN_CMDN;
                        NoiCap = objDefault.CANHAN_NOICAP;
                        NgayCap = objDefault.CANHAN_CMDN_NGAYCAP == null ? "" : Convert.ToDateTime(objDefault.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                    }
                }
                else if (objDONTHU.DOITUONG.CANHANs.Count == 1)
                {
                    var objCaNhan = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                    DiaChi = objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuong = objCaNhan.CANHAN_HOTEN + ", thường trú " + objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen = objCaNhan.CANHAN_HOTEN;
                    CMND = objCaNhan.CANHAN_CMDN;
                    NoiCap = objCaNhan.CANHAN_NOICAP;
                    NgayCap = objCaNhan.CANHAN_CMDN_NGAYCAP == null ? "" : Convert.ToDateTime(objCaNhan.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                }
                //Cá nhân
                var keyValues = new Dictionary<string, string> {                          
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"NOIDUNGDON",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"HOTEN_DIACHI",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DIACHI",SecurityElement.Escape(DiaChi) },                           
                            { @"CMND",SecurityElement.Escape(CMND) },
                            { @"NGAYCAP",SecurityElement.Escape(NgayCap) },
                            { @"NOICAP",SecurityElement.Escape(NoiCap) },
                            { @"LYDO",SecurityElement.Escape(LyDo) }
            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Dictionary<string, string> getDictionary_VanBanDonDoc(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string DiaChi = "";
                string ThoiHan = "..../..../20..";
                LOAIDONTHU lOAIDONTHU = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();

                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }

                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }

              
                if (objDONTHU.DOITUONG.CANHANs.Count > 1)
                {
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                    if (obj != null)
                    {
                        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = obj.CANHAN_HOTEN + ", thường trú " + obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = obj.CANHAN_HOTEN;
                      
                    }
                    else
                    {
                        var objDefault = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                        DiaChi = objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = objDefault.CANHAN_HOTEN + ", thường trú " + objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = objDefault.CANHAN_HOTEN;                   
                    }
                }
                else if (objDONTHU.DOITUONG.CANHANs.Count == 1)
                {
                    var objCaNhan = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                    DiaChi = objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuong = objCaNhan.CANHAN_HOTEN + ", thường trú " + objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen = objCaNhan.CANHAN_HOTEN;             
                }

                if(objDONTHU.HUONGXULY_THOIHANGIAIQUET != null)
                {
                    ThoiHan = Convert.ToDateTime(objDONTHU.HUONGXULY_THOIHANGIAIQUET).ToString("dd/MM/yyyy");
                }
                //Cá nhân
                var keyValues = new Dictionary<string, string> {
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"NOIDUNGDON",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"HOTEN_DIACHI",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DIACHI",SecurityElement.Escape(DiaChi) },
                            { @"CANBO",SecurityElement.Escape(NGUOIDUYET) },
                            { @"LOAIDONTHU",SecurityElement.Escape(lOAIDONTHU.LOAIDONTHU_TEN) },
                            { @"THOIHAN",SecurityElement.Escape(ThoiHan) }
                            

            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Dictionary<string, string> getDictionary_RaCongVanGiao(DONTHU objDONTHU)
        {
            try
            {
                string NGUOIDUYET = "";
                string ThongTinDoiTuong = "";
                string ThongTinDoiTuongHoTen = "";
                string NgayTao = "";
                string DiaChi = "";
                string CoQuanTiepNhan = "";             
                if (objDONTHU.NGAYTAO != null)
                {
                    NgayTao = objDONTHU.NGAYTAO.Value.Day + " tháng " + objDONTHU.NGAYTAO.Value.Month + " năm " + objDONTHU.NGAYTAO.Value.Year;
                }
                LOAIDONTHU lOAIDONTHU = vDC.LOAIDONTHUs.Where(x => x.LOAIDONTHU_ID == objDONTHU.LOAIDONTHU_CHA_ID).FirstOrDefault();
                var objNGUOIDUYET = vDC.CANBOs.Where(x => x.CANBO_ID == objDONTHU.HUONGXULY_NGUOIDUYET_CANHAN_ID).FirstOrDefault();
                if (objNGUOIDUYET != null)
                {
                    NGUOIDUYET = objNGUOIDUYET.CANBO_TEN;
                }
                DONVI COQUAN_CHUYEN = vDC.DONVIs.Where(x => x.DONVI_ID == objDONTHU.HUONGXULY_DONVI_ID).SingleOrDefault();
                if (COQUAN_CHUYEN != null)
                {
                    CoQuanTiepNhan = COQUAN_CHUYEN.TENDONVI;
                }
                //Nhóm đông người cơ quan tổ chức lấy người đại diện
                if (objDONTHU.DOITUONG.CANHANs.Count > 1)
                {
                    var obj = objDONTHU.DOITUONG.CANHANs.Where(x => x.CANHAN_DAIDIENUYQUYEN == true).FirstOrDefault();
                    if (obj != null)
                    {
                        DiaChi = obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = obj.CANHAN_HOTEN + ", thường trú " + obj.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = obj.CANHAN_HOTEN;
                       
                    }
                    else
                    {
                        var objDefault = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                        DiaChi = objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuong = objDefault.CANHAN_HOTEN + ", thường trú " + objDefault.CANHAN_DIACHI_DAYDU;
                        ThongTinDoiTuongHoTen = objDefault.CANHAN_HOTEN;
                    
                    }
                }
                else if (objDONTHU.DOITUONG.CANHANs.Count == 1)
                {
                    var objCaNhan = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                    DiaChi = objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuong = objCaNhan.CANHAN_HOTEN + ", thường trú " + objCaNhan.CANHAN_DIACHI_DAYDU;
                    ThongTinDoiTuongHoTen = objCaNhan.CANHAN_HOTEN;                 
                }
                //Cá nhân
                var keyValues = new Dictionary<string, string> {
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"NOIDUNGDON",SecurityElement.Escape(objDONTHU.DONTHU_NOIDUNG) },
                            { @"NGAYTAO",SecurityElement.Escape(NgayTao) },
                            { @"DOITUONG",SecurityElement.Escape(ThongTinDoiTuongHoTen) },
                            { @"HOTEN_DIACHI",SecurityElement.Escape(ThongTinDoiTuong) },
                            { @"DIACHI",SecurityElement.Escape(DiaChi) },
                            { @"COQUAN",SecurityElement.Escape(CoQuanTiepNhan) },
                            { @"LOAIDON",SecurityElement.Escape(lOAIDONTHU.LOAIDONTHU_TEN) }

            };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void ReplaceMergeFieldWithText(IEnumerable<FieldCode> fields, string mergeFieldName, string replacementText)
        {
            var field = fields
                .Where(f => f.InnerText.Contains(mergeFieldName))
                .ToList();

            if (field != null)
            {
                foreach (var item in field)
                {

                    Run rFldCode = (Run)item.Parent;
                    Run rBegin = rFldCode.PreviousSibling<Run>();
                    Run rSep = rFldCode.NextSibling<Run>();
                    Run rText = rSep.NextSibling<Run>();
                    Run rEnd = rText.NextSibling<Run>();
                    rFldCode.Remove();
                    rBegin.Remove();
                    rSep.Remove();
                    rEnd.Remove();
                    Text t = rText.GetFirstChild<Text>();
                    if (item.InnerText.Contains("QR_CODE"))
                    {
                        var element = new Drawing(
                            new DW.Inline(
                            new DW.Extent() { Cx = 990000L, Cy = 792000L },
                            new DW.EffectExtent()
                            {
                                LeftEdge = 0L,
                                TopEdge = 0L,
                                RightEdge = 0L,
                                BottomEdge = 0L
                            },
                            new DW.DocProperties()
                            {
                                Id = (UInt32Value)1U,
                                Name = "Picture 1"
                            },
                            new DW.NonVisualGraphicFrameDrawingProperties(
                                new A.GraphicFrameLocks() { NoChangeAspect = true }),
                            new A.Graphic(
                                new A.GraphicData(
                                    new PIC.Picture(
                                        new PIC.NonVisualPictureProperties(
                                            new PIC.NonVisualDrawingProperties()
                                            {
                                                Id = (UInt32Value)0U,
                                                Name = "QRCODE.jpg"
                                            },
                                            new PIC.NonVisualPictureDrawingProperties()),
                                        new PIC.BlipFill(
                                            new A.Blip(
                                                new A.BlipExtensionList(
                                                    new A.BlipExtension()
                                                    {
                                                        Uri =
                                                          "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                    })
                                            )
                                            {
                                                Embed = "dc6d57975c57",
                                                CompressionState =
                                                A.BlipCompressionValues.Print
                                            },
                                            new A.Stretch(
                                                new A.FillRectangle())),
                                        new PIC.ShapeProperties(
                                            new A.Transform2D(
                                                new A.Offset() { X = 0L, Y = 0L },
                                                new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                            new A.PresetGeometry(
                                                new A.AdjustValueList()
                                            )
                                            { Preset = A.ShapeTypeValues.Rectangle }))
                                )
                                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                        )
                            {
                                DistanceFromTop = (UInt32Value)0U,
                                DistanceFromBottom = (UInt32Value)0U,
                                DistanceFromLeft = (UInt32Value)0U,
                                DistanceFromRight = (UInt32Value)0U,
                                EditId = "50D07946"
                            });

                        var parent = t.Parent;

                        if (!(parent is Run))  // Parent should be a run element.
                        {
                            Console.Out.WriteLine("Parent is not run");
                        }
                        else
                        {
                            // Insert image (the image created with your function) after text place holder.        
                            t.Parent.InsertAfter<Drawing>(element, t);
                            // Remove text place holder.
                            t.Remove();
                        }
                    }
                    else
                    {
                        if (replacementText == "")
                        {
                            t.Text = "";
                        }
                        else
                        {
                            if (t != null)
                            {
                                t.Text = (replacementText != null) ? replacementText : string.Empty;
                            }
                        }
                    }
                }
            }
        }
        private void ApplyFooter(WordprocessingDocument doc)
        {
            MainDocumentPart mainDocPart = doc.MainDocumentPart;
            FooterPart footerPart1 = mainDocPart.AddNewPart<FooterPart>("r98");
            Footer footer1 = new Footer();
            Paragraph paragraph1 = new Paragraph() { };
            Run run1 = new Run();
            Text text1 = new Text();
            var fontSize = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "19" }; // font-size * 2

            //paragraph1.AppendChild(new Italic());
            RunProperties rp4 = new RunProperties();
            rp4.Italic = new Italic();
            rp4.Color = new Color() { Val = "8c8c8c" };
            rp4.AppendChild(fontSize);
            text1.Text = DateTime.Now.ToString(@"dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            //text1.Text = DateTime.Now.Hour.ToString(    ) + ":" + DateTime.Now.Minute.ToString() + " " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            run1.Append(rp4);
            run1.Append(text1);

            paragraph1.Append(run1);
            footer1.Append(paragraph1);
            footerPart1.Footer = footer1;
            SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
            if (sectionProperties1 == null)
            {
                sectionProperties1 = new SectionProperties() { };
                mainDocPart.Document.Body.Append(sectionProperties1);
            }
            FooterReference footerReference1 = new FooterReference() { Type = DocumentFormat.OpenXml.Wordprocessing.HeaderFooterValues.Default, Id = "r98" };
            sectionProperties1.InsertAt(footerReference1, 0);
        }

        #region Xuất biên nhận
        // Xuất giấy biên nhận
        public List<byte[]> XuatGiayBienNhan(List<int> DONTHU_IDs,int pUserID,string PathBieuMau_MapPath, string sourceFile, string sourceFilePatch, out List<string> ResponseFileNames)
        {
            ResponseFileNames = new List<string>();
            List<byte[]> phieu = new List<byte[]>();
            int count = DONTHU_IDs.Count;          
            for (int DONTHU_ID_item = 0; DONTHU_ID_item < DONTHU_IDs.Count; DONTHU_ID_item++)
            {
                //int vLien = 1;
                int vDONTHU_ID = DONTHU_IDs[DONTHU_ID_item];
                string ResponseFileName = "";
                var objDONTHU_BY_ID = vDC.DONTHUs.Where(x => x.DONTHU_ID == vDONTHU_ID).SingleOrDefault();
                byte[] byteArray = null;
                sourceFilePatch = sourceFilePatch + "\\Giaybiennhan.docx";
                byteArray = File.ReadAllBytes(sourceFilePatch);

                MemoryStream mem = new MemoryStream();
                mem.Write(byteArray, 0, (int)byteArray.Length);

                using (var wordDoc = WordprocessingDocument.Open(mem, true))
                {
                    var mergeFields = wordDoc.MainDocumentPart.RootElement.Descendants<FieldCode>();
                    var DictionaryKeyWWord = new Dictionary<string, string>();
                    DictionaryKeyWWord = getDictionaryGiayBienNhan(objDONTHU_BY_ID,pUserID);
                    ResponseFileName = ResponseFileName + "Giaybiennhan";
                
                    ResponseFileName = ResponseFileName + ".docx";
                    ResponseFileNames.Add(ResponseFileName);

                    foreach (var key in DictionaryKeyWWord)
                    {
                        ReplaceMergeFieldWithText(mergeFields, key.Key, key.Value);
                    }
                    fillTableHoSo(wordDoc, (int)objDONTHU_BY_ID.DONTHU_ID);


                    string sourceFile_SaveAs = PathBieuMau_MapPath + "\\Export\\" + ResponseFileName;
                    File.WriteAllBytes(sourceFile_SaveAs, mem.ToArray());

                    //wordDoc.MainDocumentPart.Document.Save();
                    wordDoc.Close();
                }
                phieu.Add(mem.ToArray());
                //}
                break;
            }
            //byte[] allData = OpenAndCombine(phieu);
            return phieu;
        }

        private Dictionary<string, string> getDictionaryGiayBienNhan(DONTHU objDONTHU,int pUserID)
        {
            try
            {
                var objCANBO = vDC.CANBOs.Where(x => x.UserId == pUserID).FirstOrDefault();
                string CANBO_TEN = "...........................................";
                string TENCHUCVU = "...........................................";              
                if (objCANBO !=null)
                {
                    CANBO_TEN = objCANBO.CANBO_TEN;
                    TENCHUCVU = objCANBO.CHUCVU.TENCHUCVU;
                }                           
                var objCANHAN = objDONTHU.DOITUONG.CANHANs.FirstOrDefault();
                string CANHAN_CMDN_NGAYCAP = objCANHAN.CANHAN_CMDN_NGAYCAP == null ? "..........................................." : Convert.ToDateTime(objCANHAN.CANHAN_CMDN_NGAYCAP).ToString("dd/MM/yyyy");
                string CANHAN_CMDN = string.IsNullOrEmpty(objCANHAN.CANHAN_CMDN) ==true? "..........................................." : objCANHAN.CANHAN_CMDN;
                string CANHAN_NOICAP = string.IsNullOrEmpty(objCANHAN.CANHAN_NOICAP) == true ? "..........................................." : objCANHAN.CANHAN_NOICAP;
                string CANHAN_DIACHI = string.IsNullOrEmpty(objCANHAN.CANHAN_DIACHI) == true ? "............................................" : objCANHAN.CANHAN_DIACHI;
             
                //foreach (var CANHAN_item in objDONTHU.DOITUONG.CANHANs)
                //{
                //    ThongTinDoiTuong += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "Bà " : "Ông ") + CANHAN_item.CANHAN_HOTEN + ", thường trú " + CANHAN_item.CANHAN_DIACHI_DAYDU;
                //    ThongTinDoiTuongHoTen += ", " + (CANHAN_item.CANHAN_GIOITINH == true ? "bà " : "ông ") + CANHAN_item.CANHAN_HOTEN;
                //}
                //ThongTinDoiTuong = ThongTinDoiTuong.Substring(2, ThongTinDoiTuong.Length - 2);
                //ThongTinDoiTuongHoTen = ThongTinDoiTuongHoTen.Substring(2, ThongTinDoiTuongHoTen.Length - 2);
                var keyValues = new Dictionary<string, string> {                        
                            { @"dd", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"hh_t", SecurityElement.Escape( DateTime.Now.ToString("hh")) },
                            { @"dd_t", SecurityElement.Escape( DateTime.Now.ToString("dd")) },
                            { @"MM_T", SecurityElement.Escape( DateTime.Now.ToString("MM")) },
                            { @"yyyy_t", SecurityElement.Escape( DateTime.Now.ToString("yyyy")) },
                            { @"CANBO_TEN",SecurityElement.Escape(CANBO_TEN)},
                            { @"TENCHUCVU",SecurityElement.Escape(TENCHUCVU)},
                            { @"CANHAN_HOTEN",SecurityElement.Escape(objCANHAN.CANHAN_HOTEN) },
                            { @"CANHAN_CMDN",SecurityElement.Escape(CANHAN_CMDN) },
                            { @"CANHAN_CMDN_NGAYCAP",SecurityElement.Escape(CANHAN_CMDN_NGAYCAP) },
                            { @"CANHAN_NOICAP",SecurityElement.Escape(CANHAN_NOICAP) },
                            { @"CANHAN_DIACHI",SecurityElement.Escape(objCANHAN.CANHAN_DIACHI_DAYDU) }                         
                        };
                return keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Xuất sổ tiếp dân
        public List<byte[]> XuatSoTiepDan(List<SoTiepDanInfo> _SoTiepDanInfo, string PathBieuMau_MapPath, string sourceFile, string sourceFilePatch, out List<string> ResponseFileNames)
        {
            ResponseFileNames = new List<string>();
            List<byte[]> phieu = new List<byte[]>();
            //int vLien = 1;
            string ResponseFileName = "";         
            byte[] byteArray = null;
            sourceFilePatch = sourceFilePatch + "\\Sotiepdan.docx";
            byteArray = File.ReadAllBytes(sourceFilePatch);

            MemoryStream mem = new MemoryStream();
            mem.Write(byteArray, 0, (int)byteArray.Length);

            using (var wordDoc = WordprocessingDocument.Open(mem, true))
            {
                fillTableSoTiepDan(wordDoc, _SoTiepDanInfo);
                var mergeFields = wordDoc.MainDocumentPart.RootElement.Descendants<FieldCode>();
                var DictionaryKeyWWord = new List<Dictionary<string, string>>();
                DictionaryKeyWWord = getDictionarySoTiepDan(_SoTiepDanInfo);
                ResponseFileName = ResponseFileName + "Sotiepdan";
         
                ResponseFileName = ResponseFileName + ".docx";
                ResponseFileNames.Add(ResponseFileName);
        
                string sourceFile_SaveAs = PathBieuMau_MapPath + "\\Export\\" + ResponseFileName;
                File.WriteAllBytes(sourceFile_SaveAs, mem.ToArray());
                wordDoc.MainDocumentPart.Document.Save();
                wordDoc.Close();
                phieu.Add(mem.ToArray());
            }
          
            //byte[] allData = OpenAndCombine(phieu);
            return phieu;
        }

        private List<Dictionary<string, string>> getDictionarySoTiepDan( List<SoTiepDanInfo> _soTiepDan)
        {
            try
            {
                List<Dictionary<string, string>> List_keyValues = new List<Dictionary<string, string>>();

                foreach (var it in _soTiepDan)
                {
                    var keyValues = new Dictionary<string, string> {                         
                            { @"A_1", SecurityElement.Escape(it.STT.ToString()) },
                            { @"B_2", SecurityElement.Escape(it.NGAYTIEP)},
                            { @"C_3", SecurityElement.Escape( it.HOTEN_DIACHI) },
                            { @"D_4", SecurityElement.Escape( it.CMND) },
                            { @"E_5", SecurityElement.Escape(it.NOIDUNG) },
                            { @"F_6", SecurityElement.Escape(it.LOAIDON_SONGUOI) },
                            { @"G_7", SecurityElement.Escape(it.COQUANGIAIQUYET) },
                            { @"H_8",SecurityElement.Escape(it.HXL_THULY)},
                            { @"I_9",SecurityElement.Escape(it.HXL_TRADON)},
                            { @"J_10",SecurityElement.Escape(it.HXL_CHUYENDON) },
                            { @"K_11",SecurityElement.Escape(it.THEODOIKETQUA) },
                            { @"L_12",SecurityElement.Escape(it.GHICHU) }
                        };
                    List_keyValues.Add(keyValues);
                }                                         
                return List_keyValues;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void fillTableSoTiepDan(WordprocessingDocument wordDoc, List<SoTiepDanInfo> _soTiepDan)
        {
            //List<int> KeyKey = new List<int>();
            _soTiepDan = _soTiepDan.ToList();
            var indexRowInTable = 3;
            DataTable dt_SoTiepDan= new DataTable();
            dt_SoTiepDan.Columns.Add("STT");
            dt_SoTiepDan.Columns.Add("NGAYTIEP");
            dt_SoTiepDan.Columns.Add("HOTEN_DIACHI");
            dt_SoTiepDan.Columns.Add("CMND");
            dt_SoTiepDan.Columns.Add("NOIDUNG");
            dt_SoTiepDan.Columns.Add("LOAIDON_SONGUOI");
            dt_SoTiepDan.Columns.Add("COQUANGIAIQUYET");
            dt_SoTiepDan.Columns.Add("HXL_THULY");
            dt_SoTiepDan.Columns.Add("HXL_TRADON");
            dt_SoTiepDan.Columns.Add("HXL_CHUYENDON");
            dt_SoTiepDan.Columns.Add("THEODOIKETQUA");
            dt_SoTiepDan.Columns.Add("GHICHU");
            foreach(var it in _soTiepDan)
            {
                DataRow dr = dt_SoTiepDan.NewRow();
                dr["STT"] = it.STT;
                dr["NGAYTIEP"] = it.NGAYTIEP;
                dr["HOTEN_DIACHI"] = it.HOTEN_DIACHI;
                dr["CMND"] = it.CMND;
                dr["NOIDUNG"] = it.NOIDUNG;
                dr["LOAIDON_SONGUOI"] = it.LOAIDON_SONGUOI;
                dr["COQUANGIAIQUYET"] = it.COQUANGIAIQUYET;
                dr["HXL_THULY"] = it.HXL_THULY;
                dr["HXL_TRADON"] = it.HXL_TRADON;
                dr["HXL_CHUYENDON"] = it.HXL_CHUYENDON;
                dr["THEODOIKETQUA"] = it.THEODOIKETQUA;
                dr["GHICHU"] = it.GHICHU;
                dt_SoTiepDan.Rows.Add(dr);
            }
           
            var tables = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ToList();
            List<Table> table_ = tables.ToList();
            foreach (var item in table_)
            {
                int index_tong = indexRowInTable;
                var rows = item.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(indexRowInTable);
                int count_row = dt_SoTiepDan.Rows.Count;
                foreach (DataRow rowData in dt_SoTiepDan.Rows)
                {
                    var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    tr.CloneNode(true);
                    var countCell = rows.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().Count();                  
                    foreach (DataColumn name in dt_SoTiepDan.Columns)
                    {
                        var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var paragraphColor = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        var text = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        var cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var runProperties = run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.RunProperties());
                        var fontSize = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "24" };                   
                        runProperties.AppendChild(fontSize);
                        RunProperties runProp = new RunProperties();
                        RunFonts runFont = new RunFonts();

                        string uuuuu = name.ColumnName;
                        if (uuuuu =="STT")
                        {
                            paragraph.ParagraphProperties.Justification = new DocumentFormat.OpenXml.Wordprocessing.Justification
                            {
                                Val = DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Center
                            };
                        }
                        else
                        {
                            paragraph.ParagraphProperties.Justification = new DocumentFormat.OpenXml.Wordprocessing.Justification
                            {
                                Val = DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Left
                            };
                        }
                      
                        paragraph.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId()
                        {
                            Val = "No Spacing"
                        };
                        paragraph.ParagraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                        {
                            After = "0",
                            Before = "0"
                        };
                        text.Text = rowData[uuuuu].ToString() != null ? rowData[uuuuu].ToString() : string.Empty;
                   
                        TableCellProperties tcp = new TableCellProperties();
                        TableCellVerticalAlignment tcVA = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };
                        //TableCellWidth tcW = new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "100" };
                        //append the vertical alignment and cell width objects to the TableCellProperties
                        //tcp.Append(tcW);
                        tcp.Append(tcVA);
                        cell.Append(tcp);
                        runFont.Ascii = "Times New Roman";
                        runProp.Append(runFont);
                        run.PrependChild<RunProperties>(runProp);
                        run.AppendChild(text);
                        paragraph.AppendChild(run);
                        paragraph.ParagraphProperties.Indentation = new DocumentFormat.OpenXml.Wordprocessing.Indentation { FirstLine = "0" };
                        cell.AppendChild(paragraph);

                        tr.AppendChild(cell);
                    }
                    index_tong++;
                    item.InsertBefore(tr, rows);

                    count_row--;
                }
                item.RemoveChild(rows);
            }
        }

        private void fillTableHoSo(WordprocessingDocument wordDoc,int pDonThuID)
        {
            List<HOSO> objHoSo = new List<HOSO>();
            var objHoSo_DonThu = vDC.DONTHU_HOSOs.Where(x => x.DONTHU_ID == pDonThuID && x.LOAI_HS_DONTHU == 0).ToList();
            if (objHoSo_DonThu.Count > 0)
            {
                var objHoSo_ID = objHoSo_DonThu.Select(x => x.HOSO_ID).ToList();
                objHoSo = vDC.HOSOs.Where(x => objHoSo_ID.Contains(x.HOSO_ID)).ToList();               
            }         
            var indexRowInTable = 0;
            DataTable dt_HOSO = new DataTable();

            dt_HOSO.Columns.Add("HOSO_TEN");
            int count = 0;
            foreach (var it in objHoSo)
            {
                count++;
                DataRow dr = dt_HOSO.NewRow();               
                dr["HOSO_TEN"] = count + ". "+ it.HOSO_TEN;
                dt_HOSO.Rows.Add(dr);
            }
            var tables = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ToList();
            List<Table> table_ = tables.Skip(3).Take(1).ToList();         
            foreach (var item in table_)
            {
                int index_tong = indexRowInTable;
                var rows = item.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(indexRowInTable);
                int count_row = dt_HOSO.Rows.Count;
                foreach (DataRow rowData in dt_HOSO.Rows)
                {
                    var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    tr.CloneNode(true);
                    var countCell = rows.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().Count();
                    foreach (DataColumn name in dt_HOSO.Columns)
                    {
                        var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var paragraphColor = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        var text = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        var cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var runProperties = run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.RunProperties());
                        var fontSize = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "28" };
                        runProperties.AppendChild(fontSize);
                        TopBorder topBorder = new DocumentFormat.OpenXml.Wordprocessing.TopBorder();
                        topBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        runProperties.AppendChild(topBorder);
                        BottomBorder bottomBorder = new DocumentFormat.OpenXml.Wordprocessing.BottomBorder();
                        bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        runProperties.AppendChild(bottomBorder);
                        RightBorder rightBorder = new DocumentFormat.OpenXml.Wordprocessing.RightBorder();
                        rightBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        runProperties.AppendChild(rightBorder);
                        LeftBorder leftBorder = new DocumentFormat.OpenXml.Wordprocessing.LeftBorder();
                        leftBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        runProperties.AppendChild(leftBorder);


                        RunProperties runProp = new RunProperties();
                        RunFonts runFont = new RunFonts();

                        string uuuuu = name.ColumnName;
                        paragraph.ParagraphProperties.Justification = new DocumentFormat.OpenXml.Wordprocessing.Justification
                        {
                            Val = DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Left

                        };
                        paragraph.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId()
                        {
                            Val = "No Spacing"
                        };
                        paragraph.ParagraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                        {
                            After = "0",
                            Before = "0"
                        };
                        text.Text = rowData[uuuuu].ToString() != null ? rowData[uuuuu].ToString() : string.Empty;

                        TableCellProperties tcp = new TableCellProperties();
                        TableCellVerticalAlignment tcVA = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };                   
                        tcp.Append(tcVA);

                        cell.Append(tcp);
                        runFont.Ascii = "Times New Roman";
                        runProp.Append(runFont);
                        run.PrependChild<RunProperties>(runProp);
                        run.AppendChild(text);
                        paragraph.AppendChild(run);
                        paragraph.ParagraphProperties.Indentation = new DocumentFormat.OpenXml.Wordprocessing.Indentation { FirstLine = "0" };
                        cell.AppendChild(paragraph);

                        tr.AppendChild(cell);                      
                    }
                    index_tong++;
                    item.InsertBefore(tr, rows);

                    count_row--;
                }
                item.RemoveChild(rows);
            }
        }

        private void fillNhieuNoiDung(WordprocessingDocument wordDoc, int pDonThuID)
        {
            var objDonThu = vDC.DONTHU_NHIEUNOIDUNGs.Where(x => x.DONTHU_ID == pDonThuID).ToList();
            var indexRowInTable = 0;
            DataTable dt_NoiDung = new DataTable();

            dt_NoiDung.Columns.Add("NOIDUNG_THAMQUYEN");
            int count = 0;
            foreach (var it in objDonThu)
            {
                count++;
                DataRow dr = dt_NoiDung.NewRow();
                dr["NOIDUNG_THAMQUYEN"] =  count + ". " + it.NOIDUNG + " thuộc thẩm quyền giải quyết của "+ it.TENDONVI;
                dt_NoiDung.Rows.Add(dr);
            }
            var tables = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().ToList();
            List<Table> table_ = tables.Skip(1).Take(1).ToList();
            foreach (var item in table_)
            {
                int index_tong = indexRowInTable;
                var rows = item.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ElementAt(indexRowInTable);
                int count_row = dt_NoiDung.Rows.Count;
                foreach (DataRow rowData in dt_NoiDung.Rows)
                {
                    var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    tr.CloneNode(true);
                    var countCell = rows.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>().Count();
                    foreach (DataColumn name in dt_NoiDung.Columns)
                    {
                        var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var paragraphColor = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties());
                        var run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        var text = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        var cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                        var runProperties = run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.RunProperties());
                        //var fontSize = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "28" };
                        //runProperties.AppendChild(fontSize);
                        //TopBorder topBorder = new DocumentFormat.OpenXml.Wordprocessing.TopBorder();
                        //topBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        //runProperties.AppendChild(topBorder);
                        //BottomBorder bottomBorder = new DocumentFormat.OpenXml.Wordprocessing.BottomBorder();
                        //bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        //runProperties.AppendChild(bottomBorder);
                        //RightBorder rightBorder = new DocumentFormat.OpenXml.Wordprocessing.RightBorder();
                        //rightBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        //runProperties.AppendChild(rightBorder);
                        //LeftBorder leftBorder = new DocumentFormat.OpenXml.Wordprocessing.LeftBorder();
                        //leftBorder.Val = new EnumValue<BorderValues>(BorderValues.None);
                        //runProperties.AppendChild(leftBorder);


                        RunProperties runProp = new RunProperties();
                        RunFonts runFont = new RunFonts();

                        string uuuuu = name.ColumnName;
                        paragraph.ParagraphProperties.Justification = new DocumentFormat.OpenXml.Wordprocessing.Justification
                        {
                            Val = DocumentFormat.OpenXml.Wordprocessing.JustificationValues.ThaiDistribute

                        };
                        paragraph.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId()
                        {
                            Val = "No Spacing"
                        };
                        paragraph.ParagraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                        {
                            After = "0",
                            Before = "0"
                        };
                        text.Text =rowData[uuuuu].ToString() != null ? rowData[uuuuu].ToString() : string.Empty;

                        TableCellProperties tcp = new TableCellProperties();
                        TableCellVerticalAlignment tcVA = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };
                        tcp.Append(tcVA);

                        //cell.Append(tcp);
                        runFont.Ascii = "Times New Roman";
                        runProp.Append(runFont);
                        run.PrependChild<RunProperties>(runProp);
                        run.AppendChild(text);
                        paragraph.AppendChild(run);
                        paragraph.ParagraphProperties.Indentation = new DocumentFormat.OpenXml.Wordprocessing.Indentation { FirstLine = "0" };
                        cell.AppendChild(paragraph);
                        tr.AppendChild(cell);
                    }
                    index_tong++;
                    item.InsertBefore(tr, rows);

                    count_row--;
                }
                item.RemoveChild(rows);
            }
        }
        #endregion
    }
}