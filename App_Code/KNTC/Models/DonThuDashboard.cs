using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonThuDashboard
/// </summary>
public class DonThuDashboard
{
    public int CaNhan { get; set; }
    public int CoQuan { get; set; }
    public int ToChuc { get; set; }
    public List<LoaiDonThu> LoaiDonThu { get; set; }
    public List<HuongXuLy> HuongXuLy { get; set; }
}