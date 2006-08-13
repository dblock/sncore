<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountEventFeaturedViewControl.ascx.cs"
 Inherits="AccountEventFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountEventsRss.aspx" />
 <table cellpadding="2" cellspacing="0" class="sncore_half_table" style="width: 95%;">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <div class="sncore_h2">
     <asp:HyperLink ID="linkFeature" runat="server" Text="Featured Event" />
     <img src="images/site/right.gif" border="0" />
    </div>
    <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
     <div class="sncore_link">
      <a href="AccountEventsView.aspx">&#187; all</a>
      <a href="FeaturedAccountEventsView.aspx">&#187; previously featured</a>
      <a href="AccountEventsRss.aspx">&#187; rss</a> 
     </div>
     <div class="sncore_link">     
      <a href="AccountEventEdit.aspx">&#187; submit an event</a>
     </div>
    </asp:Panel>
   </td>
   <td width="150px" align="center" valign="top">    
    <a runat="server" id="linkFeature2">
     <img border="0" id="imgFeature" runat="server" />
    </a>
    <div class="sncore_description">
     <a runat="server" id="linkFeature3">
      <asp:Label ID="labelFeatureName" runat="server" />
     </a>
    </div>
    <div class="sncore_description">
     <asp:Label ID="labelFeaturePlaceName" runat="server" />
    </div>
    <div class="sncore_description">
     <asp:Label ID="labelFeaturePlaceCity" runat="server" />
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
