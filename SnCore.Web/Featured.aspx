<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Featured.aspx.cs"
 Inherits="Featured" Title="Untitled Page" %>

<%@ Register TagPrefix="SnCore" TagName="AccountFeaturedView" Src="AccountFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFeaturedView" Src="PlaceFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedFeaturedView" Src="AccountFeedFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountEventFeaturedView" Src="AccountEventFeaturedViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Featured
 </div>
 <div class="sncore_h2sub">
  Think you deserve to be featured?
  <asp:LinkButton ID="linkAdministrator" runat="server" Text="Tell us why!" CausesValidation="false" />
 </div>
 <table>
  <tr>
   <td valign="top">
    <div class="sncore_h2">
     <a href="FeaturedAccountFeedsView.aspx">Featured Blog</a>
     <img src="images/site/right.gif" border="0" />
    </div>
    <SnCore:AccountFeedFeaturedView ID="accountfeedFeatured" runat="server" />
   </td>
   <td>
    <div class="sncore_link">
     <a href="AccountFeedsView.aspx">&#187; directory</a>
     <a href="AccountFeedItemImgsView.aspx">&#187; pictures</a>
     <a href="AccountFeedItemMediasView.aspx">&#187; podcasts &amp; videos</a>
    </div>
    <div class="sncore_link">
     <a href="FeaturedAccountFeedsView.aspx">&#187; previously featured</a>
    </div>
    <div class="sncore_link">
     <a href="AccountFeedEdit.aspx">&#187; syndicate a blog</a>
     <a href="AccountFeedsRss.aspx">&#187; rss</a> 
    </div>
   </td>
   <td valign="top">
    <div class="sncore_h2">
     <a href="FeaturedAccountsView.aspx">Featured Person</a>
     <img src="images/site/right.gif" border="0" />
    </div>
    <SnCore:AccountFeaturedView ID="accountFeatured" runat="server" />
   </td>
   <td>
    <div class="sncore_link">
     <a href="AccountsView.aspx">&#187; all</a> 
     <a href="AccountInvitationsManage.aspx">&#187; invite a friend</a>
    </div>
    <div class="sncore_link">
     <a href="FeaturedAccountsView.aspx">&#187; previously featured</a>
    </div>
    <div class="sncore_link">
     <a href="RefererAccountsView.aspx">&#187; top traffickers</a>
    </div>
    <div class="sncore_link">
     <a href="TagWordsView.aspx">&#187; tag cloud</a>
     <a href="AccountsRss.aspx">&#187; rss</a>
    </div>
   </td>
  </tr>
  <tr>
   <td valign="top">
    <div class="sncore_h2">
     <a href="FeaturedPlacesView.aspx">Featured Place</a>
     <img src="images/site/right.gif" border="0" />
    </div>
    <SnCore:PlaceFeaturedView ID="PlaceFeaturedView" runat="server" />
   </td>
   <td>
    <div class="sncore_link">
     <a href="PlacesView.aspx">&#187; all places</a> 
     <a href="PlaceEdit.aspx">&#187; add a new place</a> 
    </div>
    <div class="sncore_link">
     <a href="FeaturedPlacesView.aspx">&#187; previously featured</a>
     <a href="PlacesRss.aspx">&#187; rss</a>
    </div>
   </td>
   <td valign="top">
    <div class="sncore_h2">
     <a href="FeaturedAccountEventsView.aspx">Featured Event</a>
     <img src="images/site/right.gif" border="0" />
    </div>
    <SnCore:AccountEventFeaturedView ID="accounteventsFeatured" runat="server" />
   </td>
   <td>
    <div class="sncore_link">
     <a href="AccountEventsView.aspx">&#187; all</a>
     <a href="FeaturedAccountEventsView.aspx">&#187; previously featured</a>
    </div>
    <div class="sncore_link">
     <a href="AccountEventWizard.aspx">&#187; add an event</a>
     <a href="AccountEventsRss.aspx">&#187; rss</a> 
    </div>
   </td>
  </tr>
 </table>
 
</asp:Content>
