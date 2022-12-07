using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TiepDanChart
/// </summary>
public class TiepDan
{   
        public long TIEPDAN_ID { get; set; }
        public int DONTHU_ID { get; set; }
        public int Month { get; set; }
        public int DOITUONG_LOAI { get; set; }
        public int LOAIDONTHU_ID { get; set; }
        public string LOAIDONTHU_TEN { get; set; }
}