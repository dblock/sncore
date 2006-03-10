<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionView.aspx.cs"
 Inherits="DiscussionView" Title="Discussion" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="DiscussionRss.aspx?id=<% Response.Write(RequestId); %>">
 <div class="sncore_navigate">
  <asp:HyperLink NavigateUrl="~/DiscussionsView.aspx" CssClass="sncore_navigate_item"
   ID="linkDiscussions" Text="Discussions" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkDiscussion" Text="Discussion"
   runat="server" />
  <asp:HyperLink ImageUrl="images/rss.gif" runat="server" ToolTip="Rss" ID="linkRss" />
 </div>
 <SnCore:DiscussionView runat="server" ID="discussionMain" PostNewText="&#187; Post New" />
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    socially bookmark this discussion:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
  </tr>
 </table>
</asp:Content>
