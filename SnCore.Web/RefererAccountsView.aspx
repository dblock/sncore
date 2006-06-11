<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="RefererAccountsView.aspx.cs"
 Inherits="RefererAccountsView" Title="Top Traffickers" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Top Traffickers
    </div>
    <div class="sncore_h2sub">
     <a href="AccountsView.aspx">&#187; All People</a>
     <a href="AccountInvitationsManage.aspx">&#187; Invite a Friend</a>
    </div>
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" RepeatColumns="4" RepeatRows="4" RepeatDirection="Horizontal"
  CssClass="sncore_table" ShowHeader="false">
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
 <table class="sncore_table">
  <tr>
   <td>
    <b>We need your help to grow!</b> Please add a link to our website and
    <asp:LinkButton ID="linkAdministrator" runat="server" Text="send an e-mail" />
    to let us know.
   </td>
  </tr>
 </table>
</asp:Content>
