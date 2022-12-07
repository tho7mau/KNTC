using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HuongXuLyChiTiet
/// </summary>
public class HuongXuLyChiTiet
{
    public int HUONGXYLY_ID { get; set; }
    public string HUONGXYLY_TEN { get; set; }
    public string HXL_COQUAN { get; set; }
    public string HXL_THOIHANGQ { get; set; }
    public string HXL_THAMQUYENGQ { get; set; }
    public string HXL_NGAYCHUYEN { get; set; }
    public string HXL_NGUOITIEPNHAN { get; set; }
    public string HXL_NGUOIDUYET { get; set; }
    public string HXL_CHUCVU { get; set; }
    public string HXL_YKIENXL { get; set; }
    public string HXL_GHICHU { get; set; }
    public List<HoSo> HoSo { get; set; }
}