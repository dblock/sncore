<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFeedTest.aspx.cs" Inherits="AccountFeedTest" Title="Feed" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AutoScroll" Src="AutoScrollControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RedirectView" Src="AccountRedirectViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelAll" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td align="center" width="150">
     <a runat="server" id="linkAccount" href="AccountView.aspx">
      <img border="0" src="AccountPictureThumbnail.aspx" runat="server" id="imageAccount" />
      <div>
       <asp:Label ID="labelAccountName" runat="server" />
      </div>
     </a>
    </td>
    <td valign="top" width="*">
     <div class="sncore_h2">
      <asp:HyperLink Target="_blank" ID="labelFeed" runat="server" Text="Feed" />
     </div>
     <div class="sncore_h2sub">
      <asp:Label ID="labelFeedDescription" runat="server" />
     </div>
    </td>
   </tr>
  </table>
  <asp:UpdatePanel runat="server" UpdateMode="Always" ID="panelGrid">
   <ContentTemplate>
    <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
     RepeatRows="5" CssClass="sncore_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="false">
     <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
      prevpagetext="Prev" horizontalalign="Center" />
     <ItemTemplate>
      <div class="sncore_h2left">
       <a href='#'>
        <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
       </a>
      </div>
      <div class="sncore_h2sub" style="font-size: smaller;">
       &#187; <%# base.Adjust(Eval("Created")) %>
       <a href='<%# base.Render(Eval("Link")) %>' target="_blank">
        &#187; x-posted
       </a>
      </div>
      <div>
       <%# GetDescription((string) Eval("Description")) %>
      </div>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>
   </ContentTemplate>
  </asp:UpdatePanel>
  <SnCore:AutoScroll runat="server" ID="autoScrollToForm" />
 </asp:Panel>
</asp:Content>
