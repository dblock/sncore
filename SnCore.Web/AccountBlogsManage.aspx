<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountBlogsManage.aspx.cs"
 Inherits="AccountBlogsManage" Title="Account | Blogs" %>

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
     My Blogs
    </div>
    <asp:HyperLink ID="HyperLink2" Text="&#187; i already have a blog" CssClass="sncore_createnew"
     NavigateUrl="AccountFeedEdit.aspx" runat="server" />
    <br />
    <asp:HyperLink ID="HyperLink1" Text="&#187; create a new blog" CssClass="sncore_createnew"
     NavigateUrl="AccountBlogEdit.aspx" runat="server" />
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
      <asp:TemplateColumn HeaderText="Blog" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
       <itemtemplate>
        <a href="AccountBlogView.aspx?id=<%# Eval("Id") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountBlogPost.aspx?bid=<%# Eval("Id") %>">Post</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountBlogEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
    <div class="sncore_h2">
     Contributing Blogs
    </div>
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManageAuthor"
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
      <asp:TemplateColumn HeaderText="Blog" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
       <itemtemplate>
        <a href="AccountBlogView.aspx?id=<%# Eval("AccountBlogId") %>">
         <%# base.Render(Eval("AccountBlogName")) %>
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountBlogPost.aspx?bid=<%# Eval("AccountBlogId") %>">Post</a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountBlogEdit.aspx?id=<%# Eval("AccountBlogId") %>">Edit</a>
       </itemtemplate>
      </asp:TemplateColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
