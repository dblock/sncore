<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionView.aspx.cs"
 Inherits="DiscussionView" Title="Discussion" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:DiscussionView runat="server" ID="discussionMain" DefaultViewRows="7" ViewText="" 
  PostNewText="&#187; Post New" />
 <table class="sncore_half_inner_table">
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
