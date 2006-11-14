<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPlaceQueueManage.aspx.cs" Inherits="AccountPlaceQueueManage" Title="Queued Places" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="100%">
    <SnCore:Title ID="titleQueuedPlaces" Text="My Queued Places" runat="server">  
     <Template>
      <div class="sncore_title_paragraph">
       You want to go places. <a href="PlacesView.aspx">Browse</a> and add your queue. 
       Your friends also want to go places. You can see your combined friends queue 
       <a href="PlaceFriendsQueueView.aspx">here</a>.
      </div>
     </Template>
    </SnCore:Title>
    <div class="sncore_h2sub">
     <a href="PlaceFriendsQueueView.aspx">&#187; My Friends Queue</a>
    </div>
    <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
     <ContentTemplate>
      <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
       ID="queue" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
       OnItemCommand="queue_Command" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
       RepeatRows="4" AllowCustomPaging="true">
       <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
        prevpagetext="Prev" horizontalalign="Center" />
       <ItemTemplate>
         <a href="PlaceView.aspx?id=<%# Eval("Place.Id") %>">
          <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("Place.PictureId") %>" />
          <div style="font-size: smaller;">
           <%# base.Render(Eval("Place.Name")) %>
          </div>
         </a>
         <div style="font-size: smaller;">
          <asp:LinkButton Text="&#187; Delete" ID="deleteQueued" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
           CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
         </div>
       </ItemTemplate>
      </SnCoreWebControls:PagedList>
     </ContentTemplate>
    </asp:UpdatePanel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
