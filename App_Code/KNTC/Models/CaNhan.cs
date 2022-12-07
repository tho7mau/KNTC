using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CaNhan
/// </summary>
public class CaNhan
{
	public long CANHAN_ID { get; set; }
	public string CANHAN_HOTEN { get; set; }
	public string CANHAN_CMDN { get; set; }
	public string CANHAN_CMDN_NGAYCAP { get; set; }
	public string CANHAN_NOICAP { get; set; }
	//public int DP_ID { get; set; }
	//public string CANHAN_DIACHI { get; set; }
	public string CANHAN_DIACHI_DAYDU { get; set; }
	public string QUOCTICH{ get; set; }
	public string DANTOC { get; set; }
	//public int NGUOITAO { get; set; }
	//public int NGUOICAPNHAT { get; set; }
	//public DateTime NGAYTAO { get; set; }
	public long DOITUONG_ID { get; set; }
	//public DateTime NGAYCAPNHAT { get; set; }
	public string DOITUONG_LOAI { get; set; }
	public string CANHAN_GIOITINH { get; set; }
}