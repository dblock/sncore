<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountDiscussionThreadsView.aspx.cs" Inherits="AcountDiscussionThreadsView"
 Title="Discussion Threads" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <link rel="alternate" type="application/rss+xml" title="Rss" href="DiscussionRss.aspx?id=<% Response.Write(RequestId); %>">
 <div class="sncore_navigate">
  <asp:HyperLink NavigateUrl="AccountView.aspx" CssClass="sncore_navigate_item"
   ID="linkAccount" Text="Account" runat="server" />
 </div>
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <asp:Label ID="labelHeader" runat="server" Text="Discussion Posts" CssClass="sncore_h2" />
   </td>
   <td align="right">
    <asp:HyperLink ImageUrl="images/rss.gif" runat="server" ToolTip="Rss" ID="linkRss" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td>
    <asp:CheckBox AutoPostBack="true" OnCheckedChanged="showTopLevel_CheckedChanged"
     ID="showTopLevel" Checked="True" runat="server" Text="only show threads started by this user" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" AllowCustomPaging="True"
  PageSize="25" ShowHeader="true" runat="server" ID="discussionView" AutoGenerateColumns="false"
  CssClass="sncore_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
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
   <asp:TemplateColumn HeaderText="Subject" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
    <itemtemplate>
     <a href="DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&id=<%# Eval("DiscussionThreadId") %>&ReturnUrl=<%# SnCore.Tools.Web.Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
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
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
