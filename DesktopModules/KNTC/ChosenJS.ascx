<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChosenJS.ascx.cs" Inherits="HOPKHONGGIAY.ChosenJS" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnJsInclude ID="ChosenJquery" runat="server" FilePath="/DesktopModules/KNTC/Scripts/chosen.jquery.js" AddTag="false" />
<dnn:DnnJsInclude ID="Chosen" runat="server" FilePath="/DesktopModules/KNTC/Scripts/chosen.js" AddTag="false" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/KNTC/Css/chosen.css" />

<%--<link href="/DesktopModules/HOPKHONGGIAY/Css/chosen.css" type="text/css" rel="stylesheet"> 
<script src="/DesktopModules/HOPKHONGGIAY/Scripts/chosen.jquery.js" type="text/javascript"></script>
<script src="/DesktopModules/HOPKHONGGIAY/Scripts/chosen.js" type="text/javascript"></script>--%>


<script type="text/javascript">
    function initchosen() {
        $(".chosen-select").chosen({ disable_search_threshold: 2, no_results_text: "Không tìm thấy kết quả phù hợp: ", allow_single_deselect: false, display_selected_options: false });
    }   
  </script>