<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemImgsView.aspx.cs"
 Inherits="AccountFeedItemImgsView" Title="People" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Pictures
    </div>
    <div class="sncore_h2sub">
     <a href="AccountFeedItemsView.aspx">&#187; Feeds</a>
     <a href="AccountFeedItemsView.aspx">&#187; Content</a>
    </div>
   </td>
   <td>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountFeedItemImgsRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountFeedItemImgsRss.aspx" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
  AllowCustomPaging="true" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal"
  CssClass="sncore_table" ShowHeader="false" OnItemCommand="gridManage_ItemCommand">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
  <ItemTemplate>
   <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
    <img border="0" src="AccountFeedItemImgThumbnail.aspx?id=<%# Eval("Id") %>" alt='<%# base.Render(Eval("Description")) %>' />
   </a>
   <div>
    x-posted in 
    <a href="AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>">
     <%# base.Render(Eval("AccountFeedName")) %>
    </a>
   </div>
   <div>
    <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
     <%# base.Render(Eval("AccountFeedItemTitle")) %>
    </a>    
   </div>
   <div>
    <atlas:UpdatePanel ID='panelShowHide' Mode="Conditional" runat="Server">
     <ContentTemplate>
      <asp:LinkButton Text='<%# (bool) Eval("Visible") ? "&#187; Hide" : "&#187; Show" %>' ID="linkToggleVisible" runat="server"
       Visible='<%# base.SessionManager.IsAdministrator %>' CommandName="Toggle" CommandArgument='<%# Eval("Id") %>' />
     </ContentTemplate>
    </atlas:UpdatePanel>
   </div>
   <br />
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
 </asp:Content>
