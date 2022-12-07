using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TiepDanDashboard
/// </summary>
public class TiepDanDashboard
{

    //public List<LoaiTiepDan> LoaiTiepDan { get; set; }
    public List<TiepDanChart> tiepDanCharts { get; set; }
    public int CaNhan { get; set; }
    public int CoQuan { get; set; }
    public int ToChuc { get; set; }
    public List<TD_LoaiDonThu> TD_LoaiDonThu { get; set; }
}