<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountBlogsManage.aspx.cs"
 Inherits="AccountBlogsManage" Title="Account | Blogs" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <SnCore:Title ID="titleMyBlogs" Text="My Blogs" runat="server">  
     <Template>
      <div class="sncore_title_paragraph">
       You can <a href="AccountBlogEdit.aspx">create a new blog</a> and start blogging. You are the editor, owner and
       manager of your blog, and you can also let your friends contribute. Your blog appears on 
       <a href="AccountView.aspx">your profile</a> and is automatically 
       <a href="AccountFeedItemsView.aspx">syndicated</a> for everyone to see.
      </div>
      <div class="sncore_title_paragraph">
       If you already have a blog, <a href="AccountFeedWizard.aspx">Syndicate It</a> instead.
      </div>
     </Template>
    </SnCore:Title>
    <asp:HyperLink ID="linkSyndicate" Text="&#187; I Already Have a Blog" CssClass="sncore_createnew"
     NavigateUrl="AccountFeedWizard.aspx" runat="server" />
    <br />
    <asp:HyperLink ID="HyperLink1" Text="&#187; Start Blogging" CssClass="sncore_createnew"
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
        <img src="images/account/blogs.gif" />
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
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
