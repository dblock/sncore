<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemMediasView.aspx.cs"
 Inherits="AccountFeedItemMediasView" Title="Podcasts & Videos" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleMedia" Text="Podcasts & Videos" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         A podcast is worth a thousand words. A video is worth a million pictures. Rich media is extracted from 
         <a href="AccountFeedsView.aspx">syndicated blogs</a> and updated several times a day.
        </div>
        <div class="sncore_title_paragraph">
         Do you have a blog? You can <a href="AccountFeedWizard.aspx">syndicate yours here</a>. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountFeedsView.aspx">&#187; Blog Directory</a>
       <a href="AccountFeedItemsView.aspx">&#187; Content</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
       <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
       <asp:LinkButton ID="linkEdit" runat="server" OnClick="linkEdit_Click" Text="&#187; Edit" />
      </div>
     </td>
     <td width="250">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountFeedItemMediasRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="3" RepeatRows="2" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnItemCommand="gridManage_ItemCommand"
    OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
      <%# Renderer.CleanHtml(Eval("EmbeddedHtml")) %>
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
