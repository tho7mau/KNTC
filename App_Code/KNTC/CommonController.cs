#region
    // File name    :   CommonController.cs
    // Purpose      :   Controller chung
    // Create date  :   10/10/2007
    // Author       :   DVTIN
    // Version      :   1.0
    // Author : Liink
#endregion

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace KNTC
{
    public class CommonController
    {
        public CommonController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetPage(int pPortalid, int pModuleID, string pProcedure, string pOptionSeach, string pKeyword, string pOptiopnSort, int pOffset, int pFetch)
        {
            IDataReader dr = DataProvider.Instance().GetPage( pPortalid,  pModuleID,  pProcedure,  pOptionSeach,  pKeyword,  pOptiopnSort,  pOffset,  pFetch);
            return ClassCommon.ConvertDataReadertoDataSet(dr);
        }
    }
}
