<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="PlaceFriendsQueueView.aspx.cs" Inherits="PlaceFriendsQueueView" Title="My Friends' Queue" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Import Namespace="SnCore.Services" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleQueuedPlaces" Text="My Friends Queue" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Your friends want to go places. This is the combined friends queue. <a href="PlacesView.aspx">Browse places</a> and add them 
    to <a href="AccountPlaceQueueManage.aspx">your personal queue</a>. Only your friends can see it.
   </div>
  </Template>
 </SnCore:Title>
 <div class="sncore_h2sub">
  <a href="AccountPlaceQueueManage.aspx">&#187; My Queue</a>
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="queue" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    ItemStyle-CssClass="sncore_table_tr_td" PageSize="5" AllowCustomPaging="true" AutoGenerateColumns="false"
    ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <ItemTemplate>
       <table border="0" bordercolor="white">
        <tr>
         <td width="150" align="center">
          <a href="PlaceView.aspx?id=<%# Eval("Place.Id") %>">
           <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("Place.PictureId") %>" />
           <div style="font-size: smaller;">
            <%# base.Render(Eval("Place.Name")) %>
           </div>
          </a>
         </td>
         <td width="400" align="center">
          <%# RenderAccounts((TransitAccount[]) Eval("Accounts")) %>
         </td>
        </tr>
       </table>
      </ItemTemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
