<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountContentGroupsManage.aspx.cs"
 Inherits="AccountContentGroupsManage" Title="Account | ContentGroups" %>

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
     My Content Groups
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Create a Content Group" CssClass="sncore_createnew"
     NavigateUrl="AccountContentGroupEdit.aspx" runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" OnItemCommand="gridManage_ItemCommand"
     AutoGenerateColumns="false" CssClass="sncore_account_table" AllowPaging="true" AllowCustomPaging="true" PageSize="10">
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
        <img src="images/account/contentgroups.gif" />
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Content Group" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
       <itemtemplate>
        <a href="AccountContentGroupView.aspx?id=<%# Eval("Id") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountContentEdit.aspx?gid=<%# Eval("Id") %>">Create Content</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountContentGroupEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
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
