<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionViewControl.ascx.cs"
 Inherits="DiscussionViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<br />
<asp:Label CssClass="sncore_h2" ID="discussionLabel" runat="server" />
<br />
<asp:Label CssClass="sncore_h2sub" ID="discussionDescription" runat="server" />
<br />
<asp:HyperLink ID="postNew" Text="Post New" CssClass="sncore_createnew" runat="server" />
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
   <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true"
    CssClass="sncore_form_button" OnClick="search_Click" />
  </td>
 </tr>
</table>
<SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" AllowCustomPaging="True"
 PageSize="25" ShowHeader="true" runat="server" ID="discussionView" AutoGenerateColumns="false"
 CssClass="sncore_table" OnItemDataBound="discussionView_ItemDataBound" OnItemCommand="discussionView_ItemCommand">
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
 <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:BoundColumn DataField="CanEdit" Visible="false" />
  <asp:BoundColumn DataField="CanDelete" Visible="false" />
  <asp:TemplateColumn>
   <itemtemplate>
    <img src="images/Item.gif" />
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn HeaderText="Subject" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
   <itemtemplate>
     <a href="DiscussionThreadView.aspx?did=<%# base.DiscussionId %>&id=<%# Eval("DiscussionThreadId") %>&ReturnUrl=<%# SnCore.Tools.Web.Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
      <%# base.Render(Eval("Subject"))%>
     </a>
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn HeaderText="Posts">
   <itemtemplate>
    <%# Eval("DiscussionThreadCount") %>
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn>
   <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <%# base.Render(Eval("AccountName").ToString())%>
     </a>
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn HeaderText="Last Post">
   <itemtemplate>
     <%# base.Adjust(Eval("DiscussionThreadModified")).ToString()%>     
   </itemtemplate>
  </asp:TemplateColumn>
  <asp:ButtonColumn CommandName="Delete" Text="Delete" />
 </Columns>
</SnCoreWebControls:PagedGrid>
