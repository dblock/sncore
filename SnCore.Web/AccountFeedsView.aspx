<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedsView.aspx.cs"
 Inherits="AccountFeedsView" Title="Feeds" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <atlas:UpdatePanel ID="panelLinks" Mode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       Feeds
      </div>
      <div class="sncore_h2sub">
       <a href="AccountFeedItemsView.aspx">&#187; Content</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountFeedsRss.aspx" />
      <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
       href="AccountFeedsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </atlas:UpdatePanel>
 <atlas:UpdatePanel runat="server" ID="panelGrid" Mode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="2" RepeatRows="4" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <table width="100%">
      <tr>
       <td width="150px">
        <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
         <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
        </a>
        <div class="sncore_link">
         <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
          <%# base.Render(Eval("AccountName")) %>
         </a>
        </div>
       </td>
       <td width="*" align="left">
        <div class="sncore_h2">
         <a href='AccountFeedView.aspx?id=<%# Eval("Id") %>'>
          <%# base.Render(Eval("Name")) %>
         </a>
         <span style="font-size: xx-small">
          <%# Renderer.GetLink(Renderer.Render(Eval("LinkUrl")), "&#187; x-posted") %>
         </span>
        </div>      
        <div class="sncore_h2sub">
         <%# base.Render(Eval("Description")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </atlas:UpdatePanel>   
</asp:Content>
