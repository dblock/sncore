<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionView.aspx.cs"
 Inherits="DiscussionView" Title="Discussion" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="FacebookLike" Src="FacebookLikeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:DiscussionView runat="server" ID="discussionMain" DefaultViewRows="7"
  PostNewText="&#187; Post New" />
 <table class="sncore_half_inner_table">
  <tr>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    bookmark:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    <SnCore:FacebookLike ID="facebookLike" runat="server" />
   </td>
  </tr>
 </table>
</asp:Content>
