<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountBlogView.aspx.cs" Inherits="AccountBlogView" Title="Blog" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccount" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="imageAccount" />
     <div>
      <asp:Label ID="labelAccountName" runat="server" />
     </div>
    </a>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="labelBlog" runat="server" Text="Blog" />
    </div>
    <div class="sncore_h2sub">
     <asp:Label ID="labelBlogDescription" runat="server" />
    </div>
    <asp:Panel ID="panelAdmin" runat="server" HorizontalAlign="Right">
     <div>
      <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="Feature" />
     </div>
     <div>
      <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="Delete Features" />
     </div>
    </asp:Panel>
    <asp:Panel ID="panelOwner" runat="server" HorizontalAlign="Right">
     <div>
      <asp:HyperLink ID="linkEdit" Text="&#187; Edit Blog" runat="server" />
     </div>
    </asp:Panel>
   </td>   
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountBlogRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountBlogRss.aspx" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    socially bookmark this blog:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
  RepeatRows="5" CssClass="sncore_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="true">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemTemplate>
   <div class="sncore_h2left">
    <a href='AccountBlogPostView.aspx?id=<%# base.Render(Eval("Id")) %>'>
     <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
    </a>
   </div>
   <div class="sncore_h2sub" style="font-size: smaller;">
    &#187; 
    by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
     <%# base.Render(Eval("AccountName")) %>
    </a>
    on 
    <%# base.Adjust(Eval("Created")) %>
    <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>
     &#187; <%# GetComments((int) Eval("CommentCount"))%></a>
   </div>
   <div>
    <%# base.RenderEx(Eval("Body")) %>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
