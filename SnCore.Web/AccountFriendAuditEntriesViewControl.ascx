<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFriendAuditEntriesViewControl.ascx.cs"
 Inherits="FriendAuditEntriesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<table class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <% Response.Write(Renderer.Render(Title)); %>
   </div>
  </td>
  <td align="right" valign="middle">
   <SnCore:RssLink ID="linkFriendsActivity" runat="server" Title="Activity RSS"
    NavigateUrl="AccountFriendAuditEntriesRss.aspx" />
  </td>
 </tr>
</table>
<asp:UpdatePanel runat="server" ID="panelAuditEntries" UpdateMode="Always" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridFriends" 
   AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
   CssClass="sncore_inner_table" ShowHeader="false">
   <PagerStyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
    prevpagetext="Prev" horizontalalign="Center" />
   <ItemTemplate>
    <span class="sncore_message_tr_td_span">
     <div class="sncore_message">
      <div class="sncore_person" style="margin-top: 0px;">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>&width=75&height=75" />
       </a>
      </div>
      <div class='sncore_content' style='width: <%# base.OuterWidth %>px;'>
       <div class='sncore_message_body_halftable' style="padding: 10px 10px 10px 10px;">
        <%# RenderEx(Eval("Description")) %>
       </div>
       <div class="sncore_footer">
        <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Updated")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
         &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Updated")) %>
        </span>
        &#187; <a href='<%# Eval("Url") %>'>click</a>
        <%# (bool) Eval("IsBroadcast") ? "<img src='images/account/broadcast.gif' valign='absmiddle'>" : string.Empty %>
        <asp:LinkButton ID="linkForward" OnCommand="linkForward_Command" CommandArgument='<%# Eval("Id") %>' 
         CommandName="Forward" Text='<%# (bool) Eval("IsBroadcast") ? "&#187; forward" : "&#187; broadcast" %>' runat="server"
         Visible='<%# ! (bool) Eval("IsBroadcast") || ((int) Eval("AccountId") != SessionManager.AccountId) %>' OnClientClick="this.disabled = 'disabled';" />
        <asp:LinkButton ID="linkDelete" OnCommand="linkDelete_Command" CommandArgument='<%# Eval("Id") %>' 
         CommandName="Delete" Text='&#187; delete' runat="server"
         Visible='<%# (bool) Eval("IsBroadcast") && (((int) Eval("AccountId") == SessionManager.AccountId) || SessionManager.IsAdministrator) %>'
         OnClientClick="return confirm('Are you sure you want to delete this broadcast?')" />
       </div>
      </div>
     </div>
    </span>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>


