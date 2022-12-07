<%@ Control AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="ICON" Src="~/Admin/Containers/Icon.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="VISIBILITY" Src="~/Admin/Containers/Visibility.ascx" %>

<!-- lien ket css file can thiet - container.css 
<link href="container.css" rel="stylesheet" type="text/css" />-->

<!-- xay dung cac thanh fan cho container -->
<div class="Cont_Table_Login">
    <div class="row">
        <div class="Header">
            
            <dnn:TITLE runat="server" ID="dnnTITLE" CssClass="Header_Title" />
            
        </div>
    </div>
    <div class="Content">
        <!-- thanh fan ContentPane chua cac noi dung - buoc fai ton tai -->
        <div id="ContentPane" runat="server"></div>
    </div>
    <!-- REGION: ACCTION BOTTON: PRINT, EDIT TEXT, ... -->
    <div class="row">
        

        <!-- END REGION: ACCTION BOTTON: PRINT, EDIT TEXT, ... -->
        <!-- REGION: MENU BOTTOM -->

        <!-- END REGION: MENU BOTTOM -->
    </div>
</div>
<!-- REGION: MENU SEPARATOR -->

<!-- END REGION: MENU SEPARATOR -->
