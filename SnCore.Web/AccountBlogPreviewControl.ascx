<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogPreviewControl.ascx.cs"
 Inherits="AccountBlogPreviewControl" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<table width="100%">
 <tr>
  <td width="*" align="left">
   <asp:Label CssClass="sncore_h2" ID="blogName" runat="server" />
  </td>
  <td align="center" width="*">
   <asp:Label CssClass="sncore_description" ID="blogDescription" runat="server" />
  </td>
  <td width="140" align="center">
   <SnCore:RssLink ID="linkRelRss" runat="server" />
  </td>
 </tr>
</table>
<div class="sncore_createnew">
 <asp:HyperLink ID="linkRead" runat="server" Text="&#187; Read" />
 <span class="sncore_link">
  <asp:Label ID="labelPosts" runat="server" />
 </span>
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
   RepeatRows="3" ShowHeader="false" AllowCustomPaging="true" CssClass="sncore_inner_table" 
   BorderWidth="0" BorderColor="White">
   <PagerStyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
    prevpagetext="Prev" horizontalalign="Center" />
   <ItemTemplate>
    <div>
     <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
      <%# Renderer.Render(Renderer.RemoveHtml(Eval("Title"))) %>
     </a>
     <span>
      <%# ((bool) Eval("Sticky")) ? "<img src='images/buttons/sticky.gif' valign='absmiddle'>" : "" %>
     </span>
    </div>
    <div class="sncore_link_description">
     &#187; posted by 
     <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
     <span class='<%# (DateTime.UtcNow.Subtract(((DateTime) Eval("Created"))).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
       <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
     </span>
    </div>
    <div style="margin-top: 10px; font-size: smaller;">
     <%# Renderer.RenderEx(Renderer.CleanHtml(Eval("Body"))) %>
    </div>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
