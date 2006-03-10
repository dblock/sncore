<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemDiscussionsManage.aspx.cs" Inherits="SystemDiscussionsManage" Title="System | Discussions" %>

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
     Discussions
    </div>
    <asp:HyperLink ID="HyperLink1" Text="Create New" CssClass="sncore_createnew" NavigateUrl="SystemDiscussionEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand" runat="server"
     ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_account_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:BoundColumn DataField="Name" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
       HeaderText="Discussion">
       <itemtemplate>
     <b>
      <%# base.Render(Eval("Name")) %>
     </b>
     <br />
     <%# base.Render(Eval("Description")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th">
       <itemtemplate>
     <a href="SystemDiscussionEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete">
      </asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
