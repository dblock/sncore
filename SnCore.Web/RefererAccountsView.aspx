<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="RefererAccountsView.aspx.cs"
 Inherits="RefererAccountsView" Title="Top Traffickers" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleTopTraffickers" Text="Top Traffickers" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    This page shows all members that link back to 
    <%# Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")) %>,
    most active referrers first.
   </div>
   <div class="sncore_title_paragraph">
    We need your help to grow! Please link us on your website and
    <asp:LinkButton ID="linkAdministrator" runat="server" Text="send us an e-mail"
     OnClientClick="<%# LinkMailToAdministrator %>" /> to be added to 
     Top Traffickers.
   </div>
  </Template>
 </SnCore:Title>
 <div class="sncore_h2sub">
  <a href="AccountsView.aspx">&#187; All People</a>
  <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>
 </div>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
    AllowCustomPaging="true" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a target="_blank" href='<%# base.Render(Eval("RefererHostLastRefererUri")) %>'>
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
     </a>
     <div>
      <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
       <%# base.Render(Eval("AccountName")) %>
      </a>
     (<%# Eval("RefererHostTotal") %>)
     </div>
     <div>
      <a target="_blank" href='<%# base.Render(Eval("RefererHostLastRefererUri")) %>'>
       <%# base.Render(Eval("RefererHostName")) %>
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
