<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountGroupFeaturedViewControl.ascx.cs"
 Inherits="AccountGroupFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="FeaturedAccountGroupsRss.aspx" 
 Title="Featured Groups" ButtonVisible="false" />
<div id="divTitle" class="sncore_h2" runat="server">
 <a href='AccountGroupsView.aspx'>
  Featured Groups
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:Panel ID="panelLinks" runat="server" CssClass="sncore_createnew">
 <div id="divLinks" class="sncore_link" runat="server">
  <span id="spanLinkViewGroup" runat="server">
   <a href='AccountGroupsView.aspx'>
    &#187; all groups
   </a>
   <a href='FeaturedAccountGroupsRss.aspx'>
    &#187; rss
   </a>
  </span>
 </div>
</asp:Panel>
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="2" cellspacing="0" class="sncore_half_table">
  <tr>
   <td valign="top">
    <a runat="server" id="linkFeature2">
     <img border="0" id="imgFeature" runat="server" />
    </a>
   </td>
   <td width="*" valign="top">
    <div class="sncore_title">
     <a runat="server" id="linkFeature3">
      <asp:Label ID="labelFeatureName" runat="server" />
     </a>
    </div>
    <div class="sncore_description">
     <asp:Label ID="labelFeatureDescription" runat="server" />
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
