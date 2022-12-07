using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TiepDanDS
{
    public long TIEPDAN_ID { get; set; }
    public long TIEPDAN_STT { get; set; }
    public string TIEPDAN_NGAYTAO { get; set; }
    public string TIEPDAN_NOIDUNG { get; set; }
    public int DONTHU_ID { get; set; }
    public int DOITUONG_LOAI { get; set; }
    public int LOAIDONTHU_ID { get; set; }
    public string LOAIDONTHU_TEN { get; set; }
    public List<CaNhan_Short> caNhan_Shorts { get; set; }
    public string DOITUONG_DIACHI { get; set; }

}