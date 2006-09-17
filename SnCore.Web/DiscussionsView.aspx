<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionsView.aspx.cs" Inherits="DiscussionsView" Title="Forums" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Forums
 </div> 
 <div class="sncore_h2sub">
  <a href="DiscussionTopOfThreadsView.aspx">&#187; New Threads</a>
  <a href="DiscussionThreadsView.aspx">&#187; New Posts</a>
  <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    search:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
    <asp:RequiredFieldValidator ID="inputSearchRequired" runat="server" ControlToValidate="inputSearch"
     CssClass="sncore_form_validator" ErrorMessage="search string is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table>
 <atlas:UpdatePanel runat="server" ID="panelGridManage" Mode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" AutoGenerateColumns="false"
    CssClass="sncore_table" AllowPaging="true" AllowCustomPaging="true" PageSize="10">
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/discussions.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Forum">
      <itemtemplate>
       <a href='DiscussionView.aspx?id=<%# Eval("Id") %>'>
        <%# base.Render(Eval("Name")) %>
       </a>
       <span style="font-size: smaller;">
        <a href="DiscussionPost.aspx?did=<%# Eval("Id") %>&ReturnUrl=<%# 
         Renderer.UrlEncode(Request.Url.PathAndQuery) %>"><%# base.SessionManager.IsLoggedIn ? "&#187; post new" : ""%></a>
       </span>
       <div class="sncore_description">
       <%# base.Render(Eval("Description")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Threads">
      <itemtemplate>
       <%# Eval("ThreadCount") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Posts">
      <itemtemplate>
       <%# Eval("PostCount") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='DiscussionRss.aspx?id=<%# Eval("Id") %>'><img 
        border="0" alt="Rss" src="images/rss.gif" /></a>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </atlas:UpdatePanel>
</asp:Content>
