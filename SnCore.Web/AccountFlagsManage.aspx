<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFlagsManage.aspx.cs"
 Inherits="AccountFlagsManage" Title="Account | Flags" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Reported Abuse
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
    AllowPaging="true" AllowCustomPaging="true" PageSize="5">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/flags.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Member">
      <itemtemplate>
       <a target="_blank" href='AccountView.aspx?id=<%# Eval("FlaggedAccountId") %>'>
        <%# Eval("FlaggedAccountName") %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Reported">
      <itemtemplate>
       <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Type">
      <itemtemplate>
       <%# Eval("AccountFlagType") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountFlagView.aspx?id=<%# Eval("Id") %>">View</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
