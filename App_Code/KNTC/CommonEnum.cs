using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace KNTC
{
    /// <summary>
    /// Class contain common enums in project
    /// </summary>
    public class CommonEnum
    {
        public enum TenBieuMau
        {
            [Description("ChuyenDon")]
            ChuyenDon = 0,
            [Description("TraDon")]
            TraDon = 1,
            [Description("HuongDan")]
            HuongDan = 1,

        }


    }
}