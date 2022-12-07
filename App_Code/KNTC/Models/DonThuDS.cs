using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonThuDS
/// </summary>
public class DonThuDS
{
    public long DONTHU_ID { get; set; }
    public long DONTHU_STT { get; set; }
    public string DONTHU_NGAYTAO { get; set; }
    public string DONTHU_NGUONDON { get; set; }
    public string DONTHU_NOIDUNG { get; set; }
   
    public int DOITUONG_LOAI { get; set; }
    public int LOAIDONTHU_ID { get; set; }
    public string LOAIDONTHU_TEN { get; set; }
    public string LOAIDONTHU_CHA_TEN { get; set; }
    public int HUONGXULY_ID { get; set; }
    public string HUONGXULY_TEN { get; set; }
    public int DONVI_ID { get; set; }
    public string DONVI_TEN { get; set; }
    public List<CaNhan_Short> caNhan_Shorts { get; set; }
    public string DOITUONG_DIACHI { get; set; }
}