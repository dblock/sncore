<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FeaturedViewControl.ascx.cs"
 Inherits="FeaturedViewControl" %>

<%@ Register TagPrefix="SnCore" TagName="AccountFeaturedView" Src="AccountFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFeaturedView" Src="PlaceFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedFeaturedView" Src="AccountFeedFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountEventFeaturedView" Src="AccountEventFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<table width="100%" cellpadding="0" cellspacing="0"> 
 <tr>
  <td colspan="2">
   <SnCore:Title ID="featuredTitle" Text="Featured" runat="server" ExpandedSize="100">  
    <Template>
     <div class="sncore_title_paragraph">
      Do you want to be featured on 
      <%# Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")) %>?          
      <br />
      <asp:LinkButton ID="linkAdministrator" runat="server" Text="Send us an e-mail"
       OnClientClick="<%# GetLinkMailToAdministrator() %>" /> and tell us why.
     </div>
    </Template>
   </SnCore:Title>
   <div class="sncore_h2sub">
    <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
   </div>
  </td>
 </tr>
 <tr>
  <td valign="top">
   <SnCore:AccountFeedFeaturedView ID="accountfeedFeatured" runat="server" />
  </td>
  <td valign="top">
   <SnCore:AccountFeaturedView ID="accountFeatured" runat="server" />
  </td>
 </tr>
 <tr>
  <td valign="top">
   <SnCore:PlaceFeaturedView ID="PlaceFeaturedView" runat="server" />
  </td>
  <td valign="top">
   <SnCore:AccountEventFeaturedView ID="accounteventsFeatured" runat="server" />
  </td>
 </tr>
</table>
