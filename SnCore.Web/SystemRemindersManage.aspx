<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemRemindersManage.aspx.cs" Inherits="SystemRemindersManage" Title="Reminders" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Reminders
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemReminderEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
  OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table">
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="center" />
  <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="center" />
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
  <img src="images/Item.gif" />
 </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Url" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
    <itemtemplate>
    <a href='<%# base.Render(Eval("Url")) %>'>Link</a>
    <div style="color: Red; font-size: smaller;">
     <%# base.Render(Eval("LastRunError")) %>
    </div>
   </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Sent">
    <itemtemplate>
     <%# Eval("ReminderEventCount") %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Last Run">
    <itemtemplate>
     <%# base.Adjust(Eval("LastRun")) %>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-Font-Bold="true">
    <itemtemplate>
     <a href='SystemReminderEdit.aspx?id=<%# Eval("Id") %>'>
      Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
