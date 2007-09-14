<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountAddressesManage.aspx.cs"
 Inherits="AccountAddressesManage" Title="Account | Addresses" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Account Addresses</div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="AccountAddressEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
  runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
  ShowHeader="False">
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="Name" Visible="false" />
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
  <img src="images/Item.gif" />
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
  <b>
   <%# base.Render(Eval("Name")) %>
  </b>
  <br />
  <%# base.Render(Eval("Street")) %>
  <%# base.Render(Eval("Apt")) %>
  <br />
  <%# base.Render(Eval("Zip")) %>
  <%# base.Render(Eval("City")) %>
  <%# base.Render(Eval("State")) %>
  <br />
  <%# base.Render(Eval("Country")) %>
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
  <a href="AccountAddressEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
