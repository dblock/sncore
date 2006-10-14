<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="Welcome" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsNewView" Src="AccountsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesNewView" Src="PlacesNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsActiveView" Src="AccountsActiveViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionPostsNewView" Src="DiscussionPostsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedItemsNewView" Src="AccountFeedItemsNewViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SearchDefault" Src="SearchDefaultControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeaturedView" Src="AccountFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFeaturedView" Src="PlaceFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountFeedFeaturedView" Src="AccountFeedFeaturedViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BlogView" Src="AccountBlogViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountEventFeaturedView" Src="AccountEventFeaturedViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table width="100%" cellpadding="0" cellspacing="0">
  <tr>
   <td valign="top" width="*">
    <SnCore:BlogView ID="websiteBlog" runat="server">
     <ContentLinkIds>
      <asp:ListItem>SnCore.PressContentGroup.Id</asp:ListItem>
      <asp:ListItem>SnCore.AddContentGroup.Id</asp:ListItem>
     </ContentLinkIds>
    </SnCore:BlogView>
    <table width="100%" cellpadding="0" cellspacing="0"> 
     <tr>
      <td colspan="2">
       <SnCore:Title ID="featuredTitle" Text="Featured" runat="server">  
        <Template>
         <div class="sncore_title_paragraph">
          Do you want to be featured on 
          <%# Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")) %>?          
          <br />
          <asp:LinkButton ID="linkAdministrator" runat="server" Text="Send us an e-mail"
           OnClientClick="<%# LinkMailToAdministrator %>" /> and tell us why.
         </div>
        </Template>
       </SnCore:Title>
       <div class="sncore_h2sub">
        <a href="TellAFriend.aspx?Url=<% Response.Write(Renderer.UrlEncode(Request.Url.PathAndQuery)); %>&Subject=<% Response.Write(Renderer.UrlEncode(Title)); %>">&#187; Tell a Friend</a>     
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
   </td>
   <td valign="top" width="333">
    <div id="panelRightFront">
     <!-- NOEMAIL-START -->
     <SnCore:SearchDefault runat="server" ID="searchDefault" />
     <!-- NOEMAIL-END -->
     <SnCore:PlacesNewView ID="placesNewMain" runat="server" Count="2" />
     <SnCore:AccountsNewView ID="accountsNewMain" runat="server" Count="2" />
     <SnCore:DiscussionPostsNewView ID="discussionsNewMain" runat="server" />
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
