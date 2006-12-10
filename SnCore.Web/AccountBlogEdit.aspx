<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountBlogEdit.aspx.cs"
 Inherits="AccountBlogEdit" Title="Blog" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Blog
 </div>
 <div>
  <asp:HyperLink ID="linkPreview" Text="&#187; Preview" CssClass="sncore_cancel" NavigateUrl="AccountBlogView.aspx"
   runat="server" />
 </div>
 <div>
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountBlogsManage.aspx"
   runat="server" />
 </div>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
    <asp:RequiredFieldValidator ID="inputNameValidator" runat="server" ControlToValidate="inputName"
     CssClass="sncore_form_validator" ErrorMessage="blog name is required" Display="Dynamic" />
    <div class="sncore_link_small">
     name of your blog
    </div>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox TextMode="MultiLine" Rows="3" ID="inputDescription" runat="server" CssClass="sncore_form_textbox" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td>
    <SnCoreWebControls:Button ID="linkSave" CssClass="sncore_form_button" OnClick="save"
     runat="server" Text="Save" />
   </td>
  </tr>
 </table>
 <asp:Panel runat="server" ID="panelEntries">
  <div class="sncore_h2">
   Posts
  </div>
  <asp:HyperLink ID="linkNew" Text="&#187; Post New" CssClass="sncore_cancel" NavigateUrl="AccountBlogPost.aspx"
   runat="server" />
  <asp:UpdatePanel ID="panelPosts" runat="server" UpdateMode="Conditional">
   <ContentTemplate>
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManagePosts" AllowPaging="true" AllowCustomPaging="true"
     OnItemCommand="gridManagePosts_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table">
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
      <asp:TemplateColumn HeaderText="Entry" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
       <a href="AccountBlogPostView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Title")) %>
       </a>
       <div class="sncore_description">
        posted by 
        <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
         <%# base.Render(Eval("AccountName")) %>
        </a>
        on 
        <%# base.Adjust(Eval("Created")) %>
       </div>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
       <a href="AccountBlogPost.aspx?bid=<%# Eval("AccountBlogId") %>&id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">Edit</a>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </ContentTemplate>
  </asp:UpdatePanel>
  <div class="sncore_h2">
   Authors</div>
  <asp:HyperLink ID="linkNewAuthor" Text="&#187; New Author" CssClass="sncore_cancel"
   NavigateUrl="AccountBlogAuthorEdit.aspx" runat="server" />
  <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="panelAuthors">
   <ContentTemplate>
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManageAuthors" AllowCustomPaging="true"
     OnItemCommand="gridManageAuthors_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table"
     PageSize="10" AllowPaging="true">
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
      <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <%# base.Render(Eval("AccountName")) %>
       </a>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Post">
       <itemtemplate>
       <%# base.Render(Eval("AllowPost").ToString()) %>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Edit">
       <itemtemplate>
       <%# base.Render(Eval("AllowEdit").ToString()) %>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Delete">
       <itemtemplate>
       <%# base.Render(Eval("AllowDelete").ToString()) %>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
       <a href="AccountBlogAuthorEdit.aspx?bid=<%# Eval("AccountBlogId") %>&id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </ContentTemplate>
  </asp:UpdatePanel>
 </asp:Panel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
