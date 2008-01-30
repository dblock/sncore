<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="NewContent.aspx.cs"
 Inherits="NewContent" Title="New Content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table_noborder">
  <tr>
   <td valign="top" width="*">
    <h3>Featured People</h3>
    <asp:DataList runat="server" ID="accounts" RepeatColumns="6" RepeatDirection="Horizontal"
     CellPadding="4" ItemStyle-HorizontalAlign="Center" Width="100%">    
     <ItemTemplate>
      <div>
       <a href="AccountView.aspx?id=<%# Eval("DataRowId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# base.GetAccount((int) Eval("DataRowId")).PictureId %>&width=75&height=75" />
       </a>
      </div>
      <div style="font-size: smaller;">
       <a href="AccountView.aspx?id=<%# Eval("DataRowId") %>">
        <%# base.Render(base.GetAccount((int) Eval("DataRowId")).Name) %>
       </a>
      </div>
      <div style="font-size: smaller;">
       <%# base.Render(base.GetAccount((int) Eval("DataRowId")).City) %>
      </div>
     </ItemTemplate>
    </asp:DataList>
    <h3>Featured Places</h3>
    <asp:DataList runat="server" ID="places" RepeatColumns="6" RepeatDirection="Horizontal"
     CellPadding="4" ItemStyle-HorizontalAlign="Center" Width="100%">
     <ItemTemplate>
      <div>
       <a href="PlaceView.aspx?id=<%# Eval("DataRowId") %>">
        <img border="0" src="PlacePictureThumbnail.aspx?id=<%# base.GetPlace((int) Eval("DataRowId")).PictureId %>&width=75&height=75" />
       </a>
      </div>
      <div style="font-size: smaller;">
       <a href="PlaceView.aspx?id=<%# Eval("DataRowId") %>">
        <%# base.Render(base.GetPlace((int) Eval("DataRowId")).Name) %>
       </a>
      </div>
      <div style="font-size: smaller;">
       <%# base.Render(base.GetPlace((int) Eval("DataRowId")).City) %>
      </div>
     </ItemTemplate>
    </asp:DataList>
    <h3>Syndicated Blogs</h3>
    <asp:DataList runat="server" ID="accountfeeds" RepeatColumns="6" RepeatDirection="Horizontal"
     CellPadding="4" ItemStyle-HorizontalAlign="Center" Width="100%">
     <ItemTemplate>
      <div>
       <a href="AccountFeedView.aspx?id=<%# Eval("DataRowId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# base.GetAccountFeed((int) Eval("DataRowId")).AccountPictureId %>&width=75&height=75" />
       </a>
      </div>
      <div style="font-size: smaller;">
       <a href="AccountFeedView.aspx?id=<%# Eval("DataRowId") %>">
        <%# base.Render(base.GetAccountFeed((int) Eval("DataRowId")).Name) %>
       </a>
      </div>
     </ItemTemplate>
    </asp:DataList>
    <h3>Upcoming Events</h3>
    <asp:DataList runat="server" ID="accountevents" RepeatColumns="6" RepeatDirection="Horizontal"
     CellPadding="4" ItemStyle-HorizontalAlign="Center" Width="100%">
     <ItemTemplate>
      <div>
       <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
        <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("PictureId") %>&width=75&height=75" />
       </a>
      </div>
      <div style="font-size: smaller;">
       <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
      </div>
      <div style="font-size: smaller;">
       <%# base.Render(Eval("PlaceName")) %>,
       <%# base.Render(Eval("PlaceCity")) %>
      </div>
     </ItemTemplate>
    </asp:DataList>
   </td>
  </tr>
 </table>
</asp:Content>
