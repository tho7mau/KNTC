using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TiepDanDetails
/// </summary>
public class TiepDanDetails
{
    public long TIEPDAN_ID { get; set; }
	public string TIEPDAN_NOIDUNG { get; set; }
	public string TIEPDAN_KETQUA { get; set; }
	public long TIEPDAN_STT { get; set; }
	public string TIEPDAN_THOGIAN { get; set; }
	public int DONTHU_ID { get; set; }
	public int NGUOITAO { get; set; }
	public int NGUOICAPNHAT { get; set; }
	public string NGAYTAO { get; set; }
	public string NGAYCAPNHAT { get; set; }
	public int TIEPDAN_LANTIEP { get; set; }
	public int TIEPDAN_CANBO_TIEP_ID { get; set; }
	public long DOITUONG_ID { get; set; }
	public string TIEPDAN_LOAI { get; set; }
	public string TIEPDAN_LOAI_LV0 { get; set; }
	public string TIEPDAN_LOAI_LV1 { get; set; }
	public List<CaNhan> CaNhan { get; set; }
	


}