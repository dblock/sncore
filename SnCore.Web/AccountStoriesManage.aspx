<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoriesManage.aspx.cs" Inherits="AccountStoriesManage" Title="Account | Stories" %>

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
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     My Stories
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Tell a Story" CssClass="sncore_createnew"
     NavigateUrl="AccountStoryEdit.aspx" runat="server" />
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
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Story" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
       <itemtemplate>
     <a href="AccountStoryEdit.aspx?id=<%# Eval("Id") %>">
      <%# base.Render(Eval("Name")) %>
     </a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
     <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">View</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
     <a href="AccountStoryEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
