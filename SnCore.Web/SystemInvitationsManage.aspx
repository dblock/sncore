<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemInvitationsManage.aspx.cs" Inherits="SystemInvitationsManage" Title="System | Invitations" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Pending Invitations
    </div>
    <SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" PageSize="15" runat="server" ID="gridManage"
     AutoGenerateColumns="false" CssClass="sncore_account_table" OnItemCommand="gridManage_ItemCommand">
     <PagerStyle CssClass="sncore_account_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn>
       <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="EMail" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
      <%# base.Render(Eval("Email")) %>
      <div style="font-size: smaller">
       <%# base.Render(Eval("Message")) %>
      </div>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Invited By">
       <itemtemplate>
      <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
       <%# base.Render(Eval("AccountName")) %>
      </a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Sent">
       <itemtemplate>
      <%# base.Adjust(Eval("Created")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
