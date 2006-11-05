<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemImgsView.aspx.cs"
 Inherits="AccountFeedItemImgsView" Title="Pictures" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titlePictures" Text="Pictures" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         A picture is worth a thousand words. These pictures are extracted from 
         <a href="AccountFeedsView.aspx">syndicated blogs</a> and are updated several times a day.
         Click on a picture to see the full blog post.
        </div>
        <div class="sncore_title_paragraph">
         Do you have a blog or a FlickR account? You can <a href="AccountFeedWizard.aspx">syndicate yours here</a>. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountFeedItemsView.aspx">&#187; Blogs</a>
       <a href="AccountFeedItemsView.aspx">&#187; Content</a>
       <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
       <asp:LinkButton ID="linkEdit" runat="server" OnClick="linkEdit_Click" Text="&#187; Edit" />
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountFeedItemImgsRss.aspx" />
      <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
       href="AccountFeedItemImgsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnItemCommand="gridManage_ItemCommand"
    OnDataBinding="gridManage_DataBinding">
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
      <asp:UpdatePanel ID='panelShowHide' UpdateMode="Conditional" runat="Server">
       <ContentTemplate>
        <asp:LinkButton Text='<%# (bool) Eval("Visible") ? "&#187; Hide" : "&#187; Show" %>' ID="linkToggleVisible" runat="server"
         Visible='<%# base.IsEditing %>' CommandName="Toggle" CommandArgument='<%# Eval("Id") %>' />
       </ContentTemplate>
      </asp:UpdatePanel>
     </div>
     <br />
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
