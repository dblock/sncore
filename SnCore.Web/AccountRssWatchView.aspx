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
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label ID="labelTitle" runat="server" />
    </div>
    <div class="sncore_h2sub">
     <asp:HyperLink ID="linkChannel" runat="server" />
     <asp:HyperLink ID="linkEditRssWatch" runat="server" Text="&#187; Edit Subscription" />
    </div>
   </td>
   <td>
    <div class="sncore_description">
     <asp:Label ID="labelSince" runat="server" />
    </div>
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" UpdateMode="Always" ID="panelGrid">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="10" CssClass="sncore_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <div style="font-weight: bold; font-size:larger; border-bottom: solid 1px silver;">
      <a href='<%# Renderer.Render(Eval("Link").ToString()) %>'>
       <%# Renderer.Render((string) Eval("Title")) %>
      </a>
     </div>
     <div style="font-size: smaller; color: Silver;">
      <%# Renderer.Render(((DateTime) Eval("PubDate")).ToString("d")) %> | <%# Renderer.RenderEx((string) Eval("Author")) %>
     </div>
     <div style="margin-top: 5px;">
      <%# Renderer.RenderEx((string) Eval("Description")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AutoScroll runat="server" ID="autoScrollToForm" />
</asp:Content>
