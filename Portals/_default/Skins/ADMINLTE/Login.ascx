<!--khai bao file user control-->
<%@ Control Language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DNNLINK" Src="~/Admin/Skins/DnnLink.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKTOMOBILE" Src="~/Admin/Skins/LinkToMobileSite.ascx" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MENU" Src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<meta name="viewport" content="width=device-width,initial-scale=1">
<link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet">
<dnn:STYLES runat="server" ID="bootstrapminStyles" IsFirst="False" Name="bootstrapmin" StyleSheet="bootstrap/css/bootstrap.min.css" UseSkinPath="True" />
<%--<script src="<%=SkinPath %>plugins/jQuery/jQuery-2.2.0.min.js"></script>--%>
<div>
    <div class="">
        <div class="content-login" >
             <img class="logo" src="<%=SkinPath %>images/quochuy.png" />
            <div id="ContentPane" class="contentPane" runat="server">
                <h1 class="title-name">
                   <%# DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalName.ToString() %>
                    </h1>
                <h1 class="title-sub">Đăng nhập</h1>
            </div>
        </div>
    </div>
    <div class="mid-content-login"><div class="taiungdung">Tải ứng dụng  tại<a href="" target="_blank"><img src="<%=SkinPath %>images/googleplay_icon.png" /></a><a href="#"><img src="<%=SkinPath %>images/app_store_icon.png" /></a></div></div>
    <%--<div class="gioithieu"><a href="#">Giới thiệu</a></div>--%>
    <div class="content-login-bg" >
    </div>
   <%-- <div class="content-login-bg-img" >
        <img src="<%=SkinPath %>images/bg2.jpg" />
    </div>--%>
    <div class="copy-right-login">
        <span class="Copyright">Copyright © 2021 <%# DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalName.ToString() %></span> 
    </div>
</div>
<script src="<%=SkinPath %>bootstrap/js/bootstrap.min.js"></script>
<style>
    .mid-content-login
    {
        text-align:center;
        padding-top:20px;
        /*text-shadow: 1px 1px #9a9999;*/
    }
    .mid-content-login .gioithieu,
    .mid-content-login .taiungdung
    {
        text-align:center;
        display:inline-block;
        padding:20px;
        color: #6b6b6b;
        font-size: 17px;
        font-family: tahoma;
    }
    .mid-content-login .gioithieu a,
    .mid-content-login .taiungdung a
    {
        color: #6b6b6b;            
    }
    
    .mid-content-login .taiungdung a img
    {
        height:35px;
        margin-left:15px;
    }
    .content-login img.logo
    {
        width: 100px;
        margin-top: 20px;
        padding: 20px;
    }
    .content-login *
    {
        font-family: Tahoma !important;
    }
    .content-login-bg
    {
            position: absolute;
    top: 0px;
    left: 0px;
    height: 100%;
    width: 100%;
    background: #D3CCE3;
    background: -webkit-linear-gradient(to right, #f1f3ff, #abb0cc);
    background: linear-gradient(to right, #f1f3ff, #abb0cc);
    z-index: -2;
    }
    .content-login-bg-img
    {
            position: absolute;
            top: 0px;
            left: 0px;
            height: 100%;
            width: 100%;
            z-index: -3;
     
    }
    .content-login-bg-img img
    {
            position: absolute;
            top: 0px;
            left: 0px;
            height: 100%;
            width: 100%;
                object-fit: cover;
    }
    .content-login
    {
        z-index:1;
        width:100%;
        text-align:center;
    }
    .content-login .Cont_Table_Login
    {
        margin-top: 1% !important;
        width: 360px;
        margin: 0 auto;
        margin-bottom: 0px;
        padding: 0px;
        display: inline-block;
    }
    .content-login .Cont_Table_Login .Header
    {
        margin: 0 auto;
        padding: 17px;
        text-align: center;
        color: #ff6600;
    }
    .content-login .Cont_Table_Login .Header span
    {
        font-size:25px;
    }
    .content-login .Cont_Table_Login .Content
    {
        background:rgba(255, 255, 255, 0.8);
        border-radius:4px;
        padding: 30px 30px 0px 30px !important;
            position: relative
    }
    /*.content-login .Cont_Table_Login .Content::after
     {
        position:absolute;
        width:40px;
        height:40px;
        background:#000;
        content:"";
     }*/
    .content-login .Cont_Table_Login .Content table
    {
        width:100%;
    }
    .content-login .Cont_Table_Login .Content table .SubHead img
    {
        display:none;
    }
    .content-login .Cont_Table_Login .Content table .SubHead label span
    {
        font-size: 14px;
        font-weight: 200;
        color: #333;
    }
    .content-login .Cont_Table_Login .Content input.NormalTextBox.form-control
    {
        width:100% !important;
        border-radius: 3px !important;
        border-color: #ececec;
        box-shadow: none;
        height: 38px;
        padding-left: 34px;
        position:relative;
    }
    .content-login .Cont_Table_Login .Content input.NormalTextBox.form-control[type="text"]
    {
        background-image:url(Portals/_default/Skins/DATTOCHUC/images/username.png);
        background-repeat:no-repeat;
    }
    .content-login .Cont_Table_Login .Content input.NormalTextBox.form-control[type="password"]
    {
        background-image:url(Portals/_default/Skins/DATTOCHUC/images/password.png);
        background-repeat:no-repeat;
    }
    .content-login .Cont_Table_Login .Content input.btn
    {
        margin-top:20px;
        width:100px !important;
        float:right;
        background:#078544;
        border-radius:3px !important;
        box-shadow: 2px 4px 6px rgba(7, 133, 68, 0.4);
        height:36px;

    }
    .content-login .Cont_Table_Login  .Content  table[summary="SignIn Design Table"] span.Normal
    {
        position: relative;
        top: -27px;
    }
    .content-login .Cont_Table_Login .Content > div>div>div>table[summary="SignIn Design Table"] > tbody > tr:nth-child(2)
    {
        position:relative;
        display:inline-block;
        height:15px !important;
    }
    .content-login .Cont_Table_Login .Content span.Normal input[type=checkbox]
    {
        margin-right: 10px;
        position: relative;
        top: 2px;
    }
    .content-login .Cont_Table_Login .Content span.Normal  label
    {
        font-size:14px;
        color:#333;
        font-weight: 200;
    }
    .content-login .Cont_Table_Login .Content a.CommandButton 
    {
        font-size: 14px;
        color: #078544;
        font-weight: 200;
        width: 100%;
        display: inline-block;
        position: relative;
        /* top: -20px; */
        padding-top: 16px;
        border-top: 1px solid #eee;
    }
    .copy-right-login
    {
        text-align: center;
        color: #6b6b6b;
        font-family: Tahoma;
        padding-top: 15px;
        position: absolute;
        bottom: 15px;
        /* right: 0px; */
        width: 100%;
    }
    
    .content-login .LoginPanel
    {
        width:auto !important;
            padding-right: 0px;
    }
    .content-login .LoginPanel .dnnFormItem .dnnLabel
    {
        display: inline-block;
        float: none;
        width: 100%;
        text-align: left;
    }
    .content-login .LoginPanel .dnnFormItem  input[type="text"], 
    .content-login .LoginPanel .dnnFormItem  input[type="password"]
    {
        display: inline-block;
        float: none;
        width: 100%;
        text-align: left;
        border: 1px solid #f5c3a2;
        font-size: 14px;
    font-family: Tahoma !important;
    }
    .content-login .LoginPanel .dnnFormItem .dnnFormLabel
    {
            width: auto;
        MARGIN: 0PX;
        font-weight: normal;
        font-family: Tahoma !important;
        padding-bottom: 10px;
    }
    .content-login .LoginPanel .dnnFormItem .dnnBoxLabel
    {
        font-weight: normal;
        font-family: Tahoma !important;
    }
    .content-login .LoginPanel .dnnForm  .dnnFormItem:nth-child(5)
    {
        display:none;
    }
    .content-login .LoginPanel .dnnForm  .dnnFormItem:nth-child(3) .dnnSecondaryAction
    {
        display:none;
    }
    .content-login .LoginPanel .dnnForm  .dnnFormItem:nth-child(3),
    .content-login .LoginPanel .dnnForm  .dnnFormItem:nth-child(4)
    {
        float:left;
        width:50%;
        clear: none;
       margin-top: 10px;
    } .content-login .LoginPanel .dnnForm  .dnnFormItem a.dnnPrimaryAction
      {
            color: #fff;
            background-color: #ff6600 !important;
            border-color: #ff6d40;
            background: no-repeat;
            font-size: 14px;
            box-shadow: 0 2px 2px 0 rgba(233,30,99,.14), 0 3px 1px -2px rgba(233,30,99,.2), 0 1px 5px 0 rgba(233,30,99,.12);
            font-weight: normal;
            padding: 8px 35px;
            border-radius: 20px;
            float: right;

      }
      .content-login .LoginPanel .dnnSecondaryAction
      {
          display:none;
      }
      .title-name
      {
        color: #545454;
        font-size: 23px;
        padding-top: 0px;
        text-transform: uppercase;
      }
      .title-sub
      {
          font-size: 22px;
        color: #f60;
        text-transform: uppercase;
      }
      html .contentpane {
    
     height: auto !important;
    
}
</style>