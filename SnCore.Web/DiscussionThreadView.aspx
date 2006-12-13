<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionThreadView.aspx.cs"
 Inherits="DiscussionThreadView" Title="Discussion Thread" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionThreadView" Src="DiscussionThreadViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:DiscussionThreadView runat="server" ID="discussionMain" />
 <table class="sncore_half_inner_table">
  <tr>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    socially bookmark this thread:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
  </tr>
 </table>
</asp:Content>
