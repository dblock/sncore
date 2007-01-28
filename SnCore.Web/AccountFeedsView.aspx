<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedsView.aspx.cs"
 Inherits="AccountFeedsView" Title="Blogs" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleBlogs" Text="Blogs" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         Do you have time to read two hundred blogs? These are <a href="AccountFeedsView.aspx">syndicated blogs</a>.
         You can read the combined blog posts <a href="AccountFeedItemsView.aspx">here</a>. It's a convenient way to 
         keep up with all this information. Blogs are updated several times a day. We also 
         extract and publish <a href="AccountFeedItemImgsView.aspx">pictures</a>,
         <a href="AccountFeedItemMediasView.aspx">podcasts and videos</a> from all posts.
        </div>
        <div class="sncore_title_paragraph">
         Do you have a blog? You can <a href="AccountFeedWizard.aspx">syndicate yours here</a>. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountFeedItemsView.aspx">&#187; Blog Posts</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
       <a href="AccountFeedItemMediasView.aspx">&#187; Podcasts &amp; Videos</a>
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountFeedsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
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
 </asp:UpdatePanel>   
</asp:Content>
