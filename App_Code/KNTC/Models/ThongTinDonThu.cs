using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ThongTinDonThu
/// </summary>
public class ThongTinDonThu
{
    public long DONTHU_ID { get; set; }
    public int LOAIDONTHU_ID { get; set; }
    public string DONTHU_TRANGTHAI{ get; set; }
    public string LOAIDONTHU_GOC { get; set; }
    public string LOAIDONTHU_TEN { get; set; }
    public string LOAIDONTHU_CHITIET { get; set; }
    public string DONTHU_NOIDUNG { get; set; }
    public string DONTHU_COQUANGQ { get; set; }
    public string DONTHU_LANGQ { get; set; }
    public string DONTHU_NGAYBANHANHQD { get; set; }
    public string DONTHU_HINHTHUCGQ { get; set; }
    public string DONTHU_KETQUAGQ { get; set; }
    public List<HoSo> HoSo { get; set; }
}