<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionsView.aspx.cs" Inherits="DiscussionsView" Title="Discussions" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <SnCore:Title ID="titleDiscussions" Text="Discussions" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Don't be shy. Exchange views, opinions and essential information with other members. 
    Create controversy. Post to a discussion.
   </div>
   <div class="sncore_title_paragraph">
    Everyone can read your posts, but one must be a registered member with a verified e-mail address to start a new topic
    or reply. Click <a href="AccountCreate.aspx">here to join</a> or <a href="AccountLogin.aspx">here to login</a> if you're
    already a member.
   </div>
  </Template>
 </SnCore:Title>
 <div class="sncore_h2sub">
  <a href="DiscussionTopOfThreadsView.aspx">&#187; New Threads</a>
  <a href="DiscussionThreadsView.aspx">&#187; New Posts</a>
  <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
  <asp:LinkButton ID="linkSearch" runat="server" Text="&#187; Search" CausesValidation="false" />
 </div>
 <ajaxtoolkit:CollapsiblePanelExtender ID="panelSearchExtender" runat="server"
  TargetControlID="panelSearch" Collapsed="true" ExpandedSize="75"
  ExpandControlID="linkSearch" CollapseControlID="linkSearch" SuppressPostBack="true">
 </ajaxtoolkit:CollapsiblePanelExtender>
 <asp:Panel ID="panelSearch" runat="server" CssClass="sncore_collapsed_div">
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
 </asp:Panel>
 <asp:UpdatePanel runat="server" ID="panelGridManage" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" AutoGenerateColumns="false"
    CssClass="sncore_table" AllowPaging="true" AllowCustomPaging="true" PageSize="20">
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='DiscussionView.aspx?id=<%# Eval("Id") %>'>
        <img border="0" src="images/account/discussions.gif" />
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Discussion">
      <itemtemplate>
       <span class="sncore_message_subject">
        <a href='DiscussionView.aspx?id=<%# Eval("Id") %>'>
         <%# base.Render(Eval("Name")) %>
        </a>
       </span>
       <span class="sncore_link">
        <a href="DiscussionPost.aspx?did=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
         &#187; post new
        </a>
       </span>
       <div class="sncore_description">
       <%# base.Render(Eval("Description")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Threads" ItemStyle-Width="75">      
      <itemtemplate>
       <%# Eval("ThreadCount") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Posts" ItemStyle-Width="75">
      <itemtemplate>
       <%# Eval("PostCount") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-Width="75">
      <itemtemplate>
       <a href='DiscussionRss.aspx?id=<%# Eval("Id") %>'><img 
        border="0" alt="Rss" src="images/rss.gif" /></a>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
