using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonThuChiTiet
/// </summary>
public class DonThuChiTiet
{
    public List<CaNhan> DonThu_Doituong { get; set; }
    public TTNguoiBiToCao TTNguoiBiToCao { get; set; }

    public ThongTinDonThu ThongTinDonThu { get; set; }
    public NguonDon NguonDon { get; set; }
    public HuongXuLyChiTiet HuongXuLy { get; set; }
}