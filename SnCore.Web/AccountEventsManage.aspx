<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventsManage.aspx.cs"
 Inherits="AccountEventsManage" Title="Account | Events" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     My Events
    </div>
    <asp:HyperLink Text="&#187; Post New" CssClass="sncore_createnew" NavigateUrl="AccountEventEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
     runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:BoundColumn DataField="Name" Visible="false" />
      <asp:TemplateColumn>
       <itemtemplate>
        <img src="images/account/events.gif" />
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Event">
       <itemtemplate>
        <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
         at
        <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
         <%# base.Render(Eval("PlaceName")) %>
        </a>
        <div class="sncore_table_tr_th">
         <%# base.Render(Eval("Schedule"))%>
        </div>
        <div class="sncore_description">
         <%# base.Render(Eval("Description")) %>
        </div>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">View</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountEventEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
