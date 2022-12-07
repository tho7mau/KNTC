<!--khai bao file user control-->
<%@ Control Language="c#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
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


<!-- Google Font: Source Sans Pro -->
<link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
<style type="text/css">
    ul.nav.nav-pills.nav-sidebar.flex-column > li > a.nav-link > p {
    white-space: nowrap;
    }
</style>

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        // Set page title by template [page name] - [site name]
        DotNetNuke.Framework.CDefault tp = ((DotNetNuke.Framework.CDefault)this.Page);
        StringBuilder title = new StringBuilder();
        if (!string.IsNullOrEmpty(PortalSettings.ActiveTab.Title))
        {
            title.Append(PortalSettings.ActiveTab.Title);
        }
        else
        {
            DotNetNuke.Entities.Tabs.TabInfo tab =
                (DotNetNuke.Entities.Tabs.TabInfo)PortalSettings.ActiveTab.BreadCrumbs[PortalSettings.ActiveTab.BreadCrumbs.Count - 1];
            title.Append(tab.TabName);
        }
        title.Append(string.Concat(" - ", PortalSettings.PortalName));
        tp.Title = title.ToString();
       
    }
</script>

<%--<dnn:DnnJsInclude ID="bootstrapJS" runat="server" FilePath="bootstrap/js/bootstrap.min.js" PathNameAlias="SkinPath" AddTag="false" />--%>
<meta name="viewport" content="width=device-width, initial-scale=1">
<div class="wrapper">
    <nav class="main-header navbar navbar-expand navbar-dark navbar-primary ">
      <!-- Left navbar links -->
      <ul class="navbar-nav">
        <li class="nav-item">
          <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
        </li>
        <li class="nav-item d-none d-sm-inline-block app-name">
          <a href="/" class="nav-link"> <img src="/Portals/0/Vinhlong_Province_min.png" alt="User Avatar" class="float-left"> <h5 class="app-title">Hệ thống quản lý hồ sơ khiếu nại tố cáo</h5></a>
        </li>
        <%--<li class="nav-item d-none d-sm-inline-block">
          <a href="#" class="nav-link">Contact</a>
        </li>--%>
      </ul>

      <!-- SEARCH FORM -->
      <%--<form class="form-inline ml-3">
        <div class="input-group input-group-sm">
          <input class="form-control form-control-navbar" type="search" placeholder="Search" aria-label="Search">
          <div class="input-group-append">
            <button class="btn btn-navbar" type="submit">
              <i class="fas fa-search"></i>
            </button>
          </div>
        </div>
      </form>--%>

      <!-- Right navbar links -->
      <ul class="navbar-nav ml-auto">
        <!-- Messages Dropdown Menu -->
        <%--<li class="nav-item dropdown">
          <a class="nav-link" data-toggle="dropdown" href="#">
            <i class="far fa-comments"></i>
            <span class="badge badge-danger navbar-badge">3</span>
          </a>
          <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
            <a href="#" class="dropdown-item">
              <!-- Message Start -->
              <div class="media">
                <img src="../../dist/img/user1-128x128.jpg" alt="User Avatar" class="img-size-50 mr-3 img-circle">
                <div class="media-body">
                  <h3 class="dropdown-item-title">
                    Brad Diesel
                    <span class="float-right text-sm text-danger"><i class="fas fa-star"></i></span>
                  </h3>
                  <p class="text-sm">Call me whenever you can...</p>
                  <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                </div>
              </div>
              <!-- Message End -->
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item">
              <!-- Message Start -->
              <div class="media">
                <img src="../../dist/img/user8-128x128.jpg" alt="User Avatar" class="img-size-50 img-circle mr-3">
                <div class="media-body">
                  <h3 class="dropdown-item-title">
                    John Pierce
                    <span class="float-right text-sm text-muted"><i class="fas fa-star"></i></span>
                  </h3>
                  <p class="text-sm">I got your message bro</p>
                  <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                </div>
              </div>
              <!-- Message End -->
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item">
              <!-- Message Start -->
              <div class="media">
                <img src="../../dist/img/user3-128x128.jpg" alt="User Avatar" class="img-size-50 img-circle mr-3">
                <div class="media-body">
                  <h3 class="dropdown-item-title">
                    Nora Silvester
                    <span class="float-right text-sm text-warning"><i class="fas fa-star"></i></span>
                  </h3>
                  <p class="text-sm">The subject goes here</p>
                  <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
                </div>
              </div>
              <!-- Message End -->
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item dropdown-footer">See All Messages</a>
          </div>
        </li>--%>
        <!-- Notifications Dropdown Menu -->
        <%--<li class="nav-item dropdown">
          <a class="nav-link" data-toggle="dropdown" href="#">
            <i class="far fa-bell"></i>
            <span class="badge badge-warning navbar-badge">15</span>
          </a>
          <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
            <span class="dropdown-item dropdown-header">15 Notifications</span>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item">
              <i class="fas fa-envelope mr-2"></i> 4 new messages
              <span class="float-right text-muted text-sm">3 mins</span>
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item">
              <i class="fas fa-users mr-2"></i> 8 friend requests
              <span class="float-right text-muted text-sm">12 hours</span>
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item">
              <i class="fas fa-file mr-2"></i> 3 new reports
              <span class="float-right text-muted text-sm">2 days</span>
            </a>
            <div class="dropdown-divider"></div>
            <a href="#" class="dropdown-item dropdown-footer">See All Notifications</a>
          </div>
        </li>
        <li class="nav-item">
          <a class="nav-link" data-widget="control-sidebar" data-slide="true" href="#" role="button">
            <i class="fas fa-th-large"></i>
          </a>
        </li>--%>
      </ul>
    </nav>
    <aside class="main-sidebar  elevation-4 sidebar-light-primary">
      <!-- Brand Logo -->
        <%
            UserInfo _currentUser = UserController.Instance.GetCurrentUserInfo();
            if (_currentUser != null) 
            {
                %>
              <a href="/" class="brand-link">
                  <img  src="/DnnImageHandler.ashx?mode=profilepic&amp;userId=<%=_currentUser.UserID %>&amp;h=33&amp;w=33" alt="KNTC Vĩnh Long"
                     class="brand-image img-circle elevation-3"
                     style="opacity: .8">
                <%--<img src="../../dist/img/AdminLTELogo.png"
                     alt="KNTC Vĩnh Long"
                     class="brand-image img-circle elevation-3"
                     style="opacity: .8">--%>
                <span class="brand-text font-weight-light"><%= _currentUser.DisplayName  %></span>
              </a>
         <%}
                %>
      <!-- Sidebar -->
      <div class="sidebar">
        <!-- Sidebar user (optional) -->
        <%--<div class="user-panel mt-3 pb-3 mb-3 d-flex">
          <div class="image">
            <img src="../../dist/img/user2-160x160.jpg" class="img-circle elevation-2" alt="User Image">
          </div>
          <div class="info">
            <a href="#" class="d-block">Alexander Pierce</a>
          </div>
        </div>--%>

        <!-- Sidebar Menu -->
          
        <nav class="mt-2">
            <dnn:MENU ID="MENU" MenuStyle="BootstrapMenu" runat="server"></dnn:MENU>
         <%-- <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
            <!-- Add icons to the links using the .nav-icon class
             with font-awesome or any other icon font library -->
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-tachometer-alt"></i>
                <p>
                  Dashboard
                  <i class="right fas fa-angle-left"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../../index.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Dashboard v1</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../../index2.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Dashboard v2</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../../index3.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Dashboard v3</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item">
              <a href="../widgets.html" class="nav-link">
                <i class="nav-icon fas fa-th"></i>
                <p>
                  Widgets
                  <span class="right badge badge-danger">New</span>
                </p>
              </a>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-copy"></i>
                <p>
                  Layout Options
                  <i class="fas fa-angle-left right"></i>
                  <span class="badge badge-info right">6</span>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../layout/top-nav.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Top Navigation</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/top-nav-sidebar.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Top Navigation + Sidebar</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/boxed.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Boxed</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/fixed-sidebar.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Fixed Sidebar</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/fixed-topnav.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Fixed Navbar</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/fixed-footer.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Fixed Footer</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../layout/collapsed-sidebar.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Collapsed Sidebar</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-chart-pie"></i>
                <p>
                  Charts
                  <i class="right fas fa-angle-left"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../charts/chartjs.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>ChartJS</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../charts/flot.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Flot</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../charts/inline.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Inline</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-tree"></i>
                <p>
                  UI Elements
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../UI/general.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>General</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/icons.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Icons</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/buttons.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Buttons</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/sliders.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Sliders</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/modals.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Modals & Alerts</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/navbar.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Navbar & Tabs</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/timeline.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Timeline</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../UI/ribbons.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Ribbons</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-edit"></i>
                <p>
                  Forms
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../forms/general.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>General Elements</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../forms/advanced.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Advanced Elements</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../forms/editors.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Editors</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../forms/validation.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Validation</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview menu-open">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-table"></i>
                <p>
                  Tables
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../tables/simple.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Simple Tables</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../tables/data.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>DataTables</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../tables/jsgrid.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>jsGrid</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-header">EXAMPLES</li>
            <li class="nav-item">
              <a href="../calendar.html" class="nav-link">
                <i class="nav-icon far fa-calendar-alt"></i>
                <p>
                  Calendar
                  <span class="badge badge-info right">2</span>
                </p>
              </a>
            </li>
            <li class="nav-item">
              <a href="../gallery.html" class="nav-link">
                <i class="nav-icon far fa-image"></i>
                <p>
                  Gallery
                </p>
              </a>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon far fa-envelope"></i>
                <p>
                  Mailbox
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../mailbox/mailbox.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Inbox</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../mailbox/compose.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Compose</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../mailbox/read-mail.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Read</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-book"></i>
                <p>
                  Pages
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../examples/invoice.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Invoice</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/profile.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Profile</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/e-commerce.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>E-commerce</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/projects.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Projects</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/project-add.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Project Add</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/project-edit.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Project Edit</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/project-detail.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Project Detail</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/contacts.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Contacts</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon far fa-plus-square"></i>
                <p>
                  Extras
                  <i class="fas fa-angle-left right"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="../examples/login.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Login</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/register.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Register</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/forgot-password.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Forgot Password</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/recover-password.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Recover Password</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/lockscreen.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Lockscreen</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/legacy-user-menu.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Legacy User Menu</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/language-menu.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Language Menu</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/404.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Error 404</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/500.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Error 500</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/pace.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Pace</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../examples/blank.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Blank Page</p>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="../../starter.html" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Starter Page</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-header">LIINK</li>
          <li class="nav-item">
            <a href="/pages/liink/simple.html" class="nav-link active">
              <i class="nav-icon icofont-table"></i>
              <p>List </p>
            </a>
          </li>
          <li class="nav-item">
            <a href="/pages/liink/form.html" class="nav-link">
              <i class="nav-icon icofont-edit"></i>
              <p>Form </p>
            </a>
          </li>
          <li class="nav-item">
            <a href="/pages/liink/detail.html" class="nav-link">
              <i class="nav-icon icofont-file-document"></i>
              <p>Detail </p>
            </a>
          </li>
            <li class="nav-header">MISCELLANEOUS</li>
            <li class="nav-item">
              <a href="https://adminlte.io/docs/3.0" class="nav-link">
                <i class="nav-icon fas fa-file"></i>
                <p>Documentation</p>
              </a>
            </li>
            <li class="nav-header">MULTI LEVEL EXAMPLE</li>
            <li class="nav-item">
              <a href="#" class="nav-link">
                <i class="fas fa-circle nav-icon"></i>
                <p>Level 1</p>
              </a>
            </li>
            <li class="nav-item has-treeview">
              <a href="#" class="nav-link">
                <i class="nav-icon fas fa-circle"></i>
                <p>
                  Level 1
                  <i class="right fas fa-angle-left"></i>
                </p>
              </a>
              <ul class="nav nav-treeview">
                <li class="nav-item">
                  <a href="#" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Level 2</p>
                  </a>
                </li>
                <li class="nav-item has-treeview">
                  <a href="#" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>
                      Level 2
                      <i class="right fas fa-angle-left"></i>
                    </p>
                  </a>
                  <ul class="nav nav-treeview">
                    <li class="nav-item">
                      <a href="#" class="nav-link">
                        <i class="far fa-dot-circle nav-icon"></i>
                        <p>Level 3</p>
                      </a>
                    </li>
                    <li class="nav-item">
                      <a href="#" class="nav-link">
                        <i class="far fa-dot-circle nav-icon"></i>
                        <p>Level 3</p>
                      </a>
                    </li>
                    <li class="nav-item">
                      <a href="#" class="nav-link">
                        <i class="far fa-dot-circle nav-icon"></i>
                        <p>Level 3</p>
                      </a>
                    </li>
                  </ul>
                </li>
                <li class="nav-item">
                  <a href="#" class="nav-link">
                    <i class="far fa-circle nav-icon"></i>
                    <p>Level 2</p>
                  </a>
                </li>
              </ul>
            </li>
            <li class="nav-item">
              <a href="#" class="nav-link">
                <i class="fas fa-circle nav-icon"></i>
                <p>Level 1</p>
              </a>
            </li>
            <li class="nav-header">LABELS</li>
            <li class="nav-item">
              <a href="#" class="nav-link">
                <i class="nav-icon far fa-circle text-danger"></i>
                <p class="text">Important</p>
              </a>
            </li>
            <li class="nav-item">
              <a href="#" class="nav-link">
                <i class="nav-icon far fa-circle text-warning"></i>
                <p>Warning</p>
              </a>
            </li>
            <li class="nav-item">
              <a href="#" class="nav-link">
                <i class="nav-icon far fa-circle text-info"></i>
                <p>Informational</p>
              </a>
            </li>
          </ul>--%>
        </nav>
        <!-- /.sidebar-menu -->
      </div>
      <!-- /.sidebar -->
    </aside>
        <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper wrapper-list">
      <!-- Content Header (Page header) -->
      <!--<section class="content-header">
        <div class="container-fluid">
          <div class="row mb-2">
            <div class="col-sm-6">
              <h1>Simple Tables</h1>
            </div>
            <div class="col-sm-6">
              <ol class="breadcrumb float-sm-right">
                <li class="breadcrumb-item"><a href="#">Home</a></li>
                <li class="breadcrumb-item active">Simple Tables</li>
              </ol>
            </div>
          </div>
        </div>
      </section>-->
      <!-- Main content -->
        <div id="ContentPane" class="contentPane" runat="server"></div>
      <%--<section class="content ">
        <div class="container-fluid">
          <div class="card card-table-master">
            <div class="card-header sticky">
              <h3 class="card-title">Đơn hàng</h3>
              <div class="card-filter">
                <!-- Text search -->
                <div class="input-group input-group tool-right">
                  <input type="text" name="table_search" class="form-control float-right" placeholder="Nhập từ khóa tìm kiếm">
                  <div class="input-group-append d-none filter-toggler">
                    <button class="navbar-toggler btn btn-default btn-flat" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                      <i class="icofont-filter"></i>
                    </button>
                  </div>
                  <div class="input-group-append ">
                    <button type="submit" class="btn btn-default btn-flat"><i class="fas fa-search"></i></button>

                  </div>

                </div>
              </div>
              <div class="card-tools">
                <div class="tool-right">
                  <div class="card-filter-advance">
                    <nav class="navbar navbar-expand-lg ">
                      <div class="collapse navbar-collapse" id="navbarNavDropdown">
                        <ul class="navbar-nav">
                          <li class="nav-item dropdown dropdown-filter">
                            <button type="button" class="btn btn-tool dropdown-toggle" aria-expanded="false" data-toggle="dropdown">
                              <i class="icofont-filter"></i> Bộ lọc
                            </button>
                            <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Mã đơn hàng</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Họ tên</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Địa chỉ</a>
                              <div class="dropdown-divider"></div>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Thời gian</a>
                            </div>
                          </li>
                          <li class="nav-item dropdown dropdown-filter">
                            <button type="button" class="btn btn-tool dropdown-toggle" aria-expanded="false" data-toggle="dropdown">
                              <i class="icofont-listing-number"></i> Nhóm
                            </button>
                            <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Họ tên</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Phương thức thanh toán</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Dịch vụ vận chuyển</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Trạng thái</a>
                            </div>
                          </li>
                          <li class="nav-item dropdown dropdown-filter">
                            <button type="button" class="btn btn-tool dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                              <i class="icofont-checked"></i> Trạng thái
                            </button>
                            <div class="dropdown-menu dropdown-menu-right" role="menu" style="">
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Đã thanh toán</a>
                              <a href="#" class="dropdown-item"><i class="icofont-check"></i>Chưa thanh toán</a>
                            </div>
                          </li>
                          <!--<li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="navbarDropdownMenuLink" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              Dropdown link
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                              <a class="dropdown-item" href="#">Action</a>
                              <a class="dropdown-item" href="#">Another action</a>
                              <a class="dropdown-item" href="#">Something else here</a>
                            </div>
                          </li>-->
                        </ul>
                      </div>
                    </nav>
                  </div>
                  <div class="card-list-type">
                    <div class="btn-group">
                      <button type="button" class="btn btn-default btn-sm btn-flat"><i class="fa fa-th-large" aria-hidden="true"></i></button>
                      <button type="button" class="btn btn-default btn-sm btn-flat active "><i class="fa fa-list" aria-hidden="true"></i></button>
                      <button type="button" class="btn btn-default btn-sm btn-flat"><i class="fa fa-th" aria-hidden="true"></i></button>
                    </div>
                  </div>


                  <div class="card-pagination">
                    <div class="btn-group tool-right pagination-group">
                      <span class="pagination-title">1-20 / 50 </span>
                      <button type="button" class="btn btn-default btn-sm disabled btn-flat"><</button>
                      <button type="button" class="btn btn-default btn-sm btn-flat">></button>
                    </div>
                  </div>
                </div>

                <!--<div class="tool-right">

                </div>-->
                <div class="tool-left">
                  <a href="form.html" class="btn  bg-gradient-primary btn-flat tool-left"><i class="icofont-plus"></i> THÊM MỚI</a>
                  <a class="btn btn-default btn-flat tool-left text-primary d-none d-lg-inline-block d-xl-inline-block"><i class="icofont-upload "></i> NHẬP</a>
                  <a class="btn btn-default btn-flat tool-left text-primary" id="BtnXoa" style="display:none;"><i class="icofont-ui-delete"></i> XÓA</a>
                </div>
              </div>
            </div>
            <!-- /.card-header -->
            <div class="card-body p-0">
              <!--style="height: 500px;"-->
              <div class="table-content p-0">
                <table class="table text-nowrap">
                  <!--table-head-fixed-->
                  <thead>
                    <tr class=" ">
                      <th class=" sticky"><input class="check_all" type="checkbox" onclick="handle_checked_delete_all_rows(this,'BtnXoa');"></th>
                      <th class=" sticky">Mã</th>
                      <th class=" sticky">Họ tên</th>
                      <th class=" sticky">Ngày tạo</th>
                      <th class=" sticky">Phương thức thanh toán</th>
                      <th class=" sticky">Dịch vụ vận chuyển</th>
                      <th class=" sticky">Trạng thái</th>
                      <th class=" sticky text-right">Thành tiền</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>183</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>182</td>
                      <td>Ngô Hoài Hận</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>181</td>
                      <td>Cao Thanh Thi</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>180</td>
                      <td>Nguyễn Huỳnh Khánh</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>179</td>
                      <td>Thái Đức Thịnh</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>178</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>177</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>176</td>
                      <td>Ngô Hoài Hận</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>175</td>
                      <td>Cao Thanh Thi</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>174</td>
                      <td>Nguyễn Huỳnh Khánh</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>173</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>172</td>
                      <td>Thái Đức Thịnh</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>171</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>170</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>172</td>
                      <td>Thái Đức Thịnh</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>171</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>170</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>171</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>170</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>171</td>
                      <td>Nguyễn Hoàng Tấn Tài</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                    <tr>
                      <td><input class=" check_item" type="checkbox"></td>
                      <td>170</td>
                      <td>Lê Huy Trường Giang</td>
                      <td>17/02/2020</td>
                      <td>COD</td>
                      <td>CPN</td>
                      <td><span class="badge bg-danger">Chưa xử lý</span></td>
                      <td class="text-right">130.000.000đ</td>
                    </tr>
                  </tbody>
                </table>


              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->

          </div><!-- /.container-fluid -->
      </section>--%>
      <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
      <div class="float-right d-none d-sm-block">
        <b>Version</b> 1.0.0
      </div>
      <strong>Copyright &copy; 2021 <a href="#">HỆ THỐNG QUẢN LÝ HỒ SƠ KHIẾU NẠI TỐ CÁO</a>.</strong> All rights
      reserved.
    </footer>

    <!-- Control Sidebar -->
    <aside class="control-sidebar control-sidebar-dark">
      <!-- Control sidebar content goes here -->
    </aside>
    <!-- /.control-sidebar -->
  </div>

<%--<div class="row">
    <div class="main-header" style="display: <%#Request.IsAuthenticated?"":"none !important" %>">
        <!-- Logo -->
        <div class="logo hidden-xs">
            <!-- mini logo for sidebar mini 50x50 pixels -->
            <span class="logo-mini waves-effect">HỌP KHÔNG GIẤY
            </span>
            <!-- logo for regular state and mobile devices -->
            <span class="logo-lg waves-effect">HỌP KHÔNG GIẤY
            <!-- <img src="Portals/_default/Skins/PSW/images/logo_title.png" alt="Công ty cổ phần Phân bón và Hóa chất Dầu khí Tây Nam Bộ"> -->
            </span>
        </div>
        <!-- Header Navbar: style can be found in header.less -->
        <nav class="navbar ">
            <!-- Sidebar toggle button-->
            <a href="#" class="sidebar-toggle" data-toggle="offcanvas" role="button" id="sidebar-toggle">
                <span class="sr-only">MENU</span>
            </a>
            <div class="navbar-custom-menu">
                <ul class="nav navbar-nav">
                    <li>
                        <a href="<%#Request.IsAuthenticated?DotNetNuke.Common.Globals.ApplicationPath+"/Default.aspx?tabid=56":DotNetNuke.Common.Globals.ApplicationPath+"/Login.aspx"%>">
                            <span><%#Request.IsAuthenticated?UserController.GetCurrentUserInfo().DisplayName.ToString():"Đăng Nhập" %></span>
                        </a>
                    </li>
                    <li class="treeview"><a href="<%#Request.IsAuthenticated?DotNetNuke.Common.Globals.ApplicationPath+"/logoff.aspx":"#"%>" style="display: <%#Request.IsAuthenticated?"block":"none" %>"><%#Request.IsAuthenticated?"Thoát":"" %> </a>
                    </li>
                </ul>
            </div>           
        </nav>

    </div>
   <aside class="main-sidebar han" style="width: <%#Request.IsAuthenticated?"":"0px !important" %>">
        <!-- sidebar: style can be found in sidebar.less -->
        <section class="sidebar">
            <dnn:MENU ID="MENU" MenuStyle="BootstrapMenu" runat="server"></dnn:MENU>
            <ul class="sidebar-menu">
                
            </ul>
        </section>
        <!-- /.sidebar -->
    </aside>
    <div class="content-wrapper" style="overflow-y: hidden; overflow-x: auto; min-width: 100px; margin-left: <%#Request.IsAuthenticated?"":"0px ! important;" %>">
        <div id="ContentPane" class="contentPane" runat="server"></div>
        <div class="row">
            <div runat="server" class="col-md-6" id="ContentLeft"></div>
            <div runat="server" class="col-md-6" id="ContentRight"></div>
        </div>
    </div>
    <div id="footer" class="main-footer" style="display: <%#Request.IsAuthenticated?"":"none ! important" %>">
        <div id="footer-content">
            <div class="bottom_left" style="">
                <div class="footer-infomation">
                    © Copyright 2020 <%# DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalName.ToString() %>
                </div>
            </div>
           
        </div>
    </div>
</div>--%>

<%--<dnn:DnnJsInclude ID="jquery" runat="server" FilePath="lib/AdminLTE/plugins/jquery/jquery.min.js" PathNameAlias="SkinPath" AddTag="false" />--%>
<dnn:DnnJsInclude ID="bootstrap" runat="server" FilePath="lib/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="overlayScrollbars" runat="server" FilePath="lib/AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="adminlte" runat="server" FilePath="lib/AdminLTE/dist/js/adminlte.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="daterangepicker" runat="server" FilePath="lib/AdminLTE/plugins/daterangepicker/daterangepicker.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="select2" runat="server" FilePath="lib/AdminLTE/plugins/select2/js/select2.full.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="duallistbox" runat="server" FilePath="lib/AdminLTE/plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="moment" runat="server" FilePath="lib/AdminLTE/plugins/moment/moment.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="DnnJsInclude2" runat="server" FilePath="lib/AdminLTE/plugins/moment/locales.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="DnnJsInclude3" runat="server" FilePath="lib/AdminLTE/plugins/moment//locale/vi.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="inputmask" runat="server" FilePath="lib/AdminLTE/plugins/inputmask/min/jquery.inputmask.bundle.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="colorpicker" runat="server" FilePath="lib/AdminLTE/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="tempusdominus" runat="server" FilePath="lib/AdminLTE/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="bootstrapswitch" runat="server" FilePath="lib/AdminLTE/plugins/bootstrap-switch/js/bootstrap-switch.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="bscustomfileinput" runat="server" FilePath="lib/AdminLTE/plugins/bs-custom-file-input/bs-custom-file-input.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="DnnJsInclude1" runat="server" FilePath="lib/AdminLTE/plugins/chart.js/Chart.min.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="demo" runat="server" FilePath="lib/AdminLTE/dist/js/common.js" PathNameAlias="SkinPath" AddTag="false" />
<script type="text/javascript">
    $(document).ready(function () {
        bsCustomFileInput.init();
        InitSticky();
    });
</script>
<!-- date-range-picker -->
  <%--<script src="../../plugins/daterangepicker/daterangepicker.js"></script>--%>
<dnn:DnnJsInclude ID="toastrJS" runat="server" FilePath="js/toastr.js" PathNameAlias="SkinPath" AddTag="false" />
<%--<dnn:DnnJsInclude ID="adminJS" runat="server" FilePath="js/admin.js" PathNameAlias="SkinPath" AddTag="false" />
<dnn:DnnJsInclude ID="wavesJS" runat="server" FilePath="js/waves.js" PathNameAlias="SkinPath" AddTag="false" />--%>
<%--<dnn:DnnJsInclude ID="customJS" runat="server" FilePath="js/js.js" PathNameAlias="SkinPath" AddTag="false" />--%>

<%--<!-- jQuery -->
  <script src="lib/AdminLTE/plugins/jquery/jquery.min.js"></script>
  <!-- Bootstrap 4 -->
  <script src="lib/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
  <!-- overlayScrollbars -->
  <script src="lib/AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
  <!-- AdminLTE App -->
  <script src="lib/AdminLTE/dist/js/adminlte.min.js"></script>
  <!-- AdminLTE for demo purposes -->
  <script src="lib/AdminLTE/dist/js/demo.js"></script>--%>
  <script>
    //$(document).on('click.bs.dropdown.data-api', '.dropdown.dropdown-filter', function (e) {
    //  e.stopPropagation();
    //});
    //$('li.dropdown.dropdown-filter .dropdown-item').on('click', function (event) {
    //  $(this).toggleClass('check');
    //});
  </script>
  <script>


//    /*Xử lý checkRow và checkAll*/
//    var count = 0;
//    function confirm_delete_rows(id) {
//      if (count > 0) {
//        if (confirm("Bạn có muốn xóa không?")) {
//          count = 0;
//          document.getElementById(id).click();
//          document.getElementById('BtnXoa').style.display = "none";
//        }
//      }

//    }
//    function confirm_delete_rows_update(id) {
//      document.getElementById(id).click();
//      document.getElementById('BtnXoa').style.display = "none";
//    }

//    function handle_checked_delete_row(obj, id) {
//      count = 0;
//      //Get the Row based on checkbox
//      var row = obj.parentNode.parentNode;

//      //Get the reference of GridView
//      var GridView = row.parentNode;
//      //Get all input elements in Gridview
//      var inputList = GridView.getElementsByTagName("input");
//      var checked = true;
//      for (var i = 0; i < inputList.length; i++) {
//        //The First element is the Header Checkbox
//        var headerCheckBox = inputList[0];
//        //Based on all or none checkboxes are checked check/uncheck Header Checkbox

//        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
//          if (!inputList[i].checked) {
//            checked = false;
//          }
//          else {
//            count++;
//          }
//        }
//      }

//      headerCheckBox.checked = checked;
//      if (count == 0) {
//        document.getElementById("divShowBtnXoa").style.display = "none";
//        //$("#divShowBtnXoa")[0].style.display = "none";
//      }
//      else {
//        //$("#divShowBtnXoa")[0].style.display = "inline-block";
//        document.getElementById("divShowBtnXoa").style.display = "inline-block";
//      }
//    }

//    function handle_checked_delete_all_rows(obj, id) {
//      count = 0;
//      var GridView = obj.parentNode.parentNode.parentNode.parentNode;
//      var inputList = GridView.getElementsByTagName("input");

//      for (var i = 0; i < inputList.length; i++) {
//        //Get the Cell To find out ColumnIndex
//        var row = inputList[i].parentNode.parentNode;
//        if (inputList[i].type == "checkbox" && obj != inputList[i]) {
//          if (obj.checked) {
//            //If the header checkbox is checked check all checkboxes
//            //and highlight all rows
//            count++;
//            inputList[i].checked = true;
//          }
//          else {
//            inputList[i].checked = false;
//          }
//        }
//      }
//      if (count == 0) {
//        document.getElementById("BtnXoa").style.display = "none";
//      }
//      else {
//        document.getElementById("BtnXoa").style.display = "initial";
//      }
//    }
///*Kết thúc checkRow và checkAll*/
  </script>




