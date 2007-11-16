<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountFriendAuditEntriesView.aspx.cs" Inherits="AccountFriendAuditEntriesView" Title="Friends Activity" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label id="labelName" runat="server" Text="Friends Activity" />
    </div>
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" ID="panelFriends" RenderMode="Inline" UpdateMode="Conditional">
  <ContentTemplate> 
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridFriends" 
    AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <%# base.RenderEx(Eval("Description")) %>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel> 
</asp:Content>
