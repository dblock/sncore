<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFeedsManage.aspx.cs" Inherits="AccountFeedsManage" Title="Account | Feeds" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     My Syndicated Content
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Syndicate Wizard" CssClass="sncore_createnew"
     NavigateUrl="AccountFeedWizard.aspx" runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" OnItemCommand="gridManage_ItemCommand"
     AutoGenerateColumns="false" CssClass="sncore_account_table">
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
        <img src='images/<%# string.IsNullOrEmpty((string) Eval("LastError")) ? "Item.gif" : "Question.gif" %>'
         alt="<%# base.Render(Eval("LastError")) %>" />
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Feed" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
       <itemtemplate>
        <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
        <div style="color: Red; font-size: smaller;">
         <%# base.Render(Eval("LastError")) %>
        </div>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Last Update">
       <itemtemplate>
        <%# base.Adjust(Eval("Updated")) %>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">View</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountFeedEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="Update" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
