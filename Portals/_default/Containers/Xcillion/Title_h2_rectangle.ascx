<%@ Control AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<div class="DNNContainer_Title_h2 SpacingBottom">
    <div class="heading_wrapper">
        <div class="lms_heading_6">
            <h2 class="lms_heading_title"><dnn:TITLE runat="server" id="dnnTITLE" CssClass="TitleH2-v6" /></h2>
		    <div class="rectangle"></div>
        </div>
    </div>
    <div id="ContentPane" runat="server"></div>
	<div class="clear"></div>
</div>

