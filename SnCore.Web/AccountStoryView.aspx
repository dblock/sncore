<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoryView.aspx.cs"
 Inherits="AccountStoryView" Title="Story" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkAccountStory" Text="Story" runat="server" />
 </div>
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 200px;">
    <asp:Panel CssClass="sncore_nopicture_table" ID="storyNoPicture" runat="server" Visible="false">
     <img border="0" src="images/AccountThumbnail.gif" />
    </asp:Panel>
    <asp:DataList ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px"
     runat="server" ID="listPictures" ShowHeader="false">
     <ItemTemplate>
      <a href='AccountStoryPictureView.aspx?id=<%# Eval("Id") %>'>
       <img border="0" alt='<%# Eval("Name") %>' src='AccountStoryPictureThumbnail.aspx?id=<%# Eval("Id") %>' />
      </a>
     </ItemTemplate>
    </asp:DataList>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="storyName" runat="server" />
    </div>
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_table_tr_td">
       <asp:Label ID="storySummary" runat="server" />
      </td>
     </tr>
    </table>
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
       socially bookmark this story:
      </td>
      <td class="sncore_table_tr_td">      
       <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
      </td>
     </tr>
    </table>
    <a name="comments"></a>
    <SnCore:DiscussionFullView runat="server" ID="storyComments" PostNewText="&#187; Post a Comment" />
   </td>
  </tr>
 </table>
</asp:Content>
