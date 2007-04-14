<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountRssWatchView.aspx.cs" Inherits="AccountRssWatchView" Title="RssWatch" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AutoScroll" Src="AutoScrollControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RedirectView" Src="AccountRedirectViewControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="labelTitle" runat="server" />
 </div>
 <div class="sncore_h2sub">
  <asp:HyperLink ID="linkEditRssWatch" runat="server" Text="&#187; Edit Subscription" />
 </div>
 <asp:UpdatePanel runat="server" UpdateMode="Always" ID="panelGrid">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="10" CssClass="sncore_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <div>
      <%# Renderer.RenderEx((string) Eval("Description")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AutoScroll runat="server" ID="autoScrollToForm" ScrollLocation="form" />
</asp:Content>
