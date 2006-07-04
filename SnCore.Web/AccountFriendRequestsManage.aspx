<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFriendRequestsManage.aspx.cs"
 Inherits="AccountFriendRequestsManage" Title="Account | Friend Requests" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:Notice ID="noticeManage" runat="server" />
    <div class="sncore_h2">
     Pending Friend Requests
    </div>
    <div class="sncore_h2sub">
     <a href="AccountFriendRequestsSentManage.aspx">&#187; Sent</a>
    </div>
    <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
     ID="listPending" Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
     OnItemCommand="listPending_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
     RepeatRows="2" AllowCustomPaging="true">
     <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
      prevpagetext="Prev" horizontalalign="Center" />
     <ItemTemplate>
      <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
       <img alt="<%# base.Render(Eval("Message")) %>" border="0" 
        src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
       <div style="font-size: smaller;">
        <%# base.Render(Eval("AccountName")) %>
       </div>
      </a>
      <div class="sncore_description">
       <%# base.Adjust(Eval("Created")).ToString() %>
      </div>
      <div style="font-size: smaller;">
       <div>
        <asp:LinkButton Text="&#187; accept" ID="linkAccept" runat="server"
         CommandName="Accept" CommandArgument='<%# Eval("Id") %>' />
       </div>
       <div>
        <asp:LinkButton Text="&#187; reject" ID="linkReject" runat="server" OnClientClick="return confirm('Are you sure you want to reject this request?')"
         CommandName="Reject" CommandArgument='<%# Eval("Id") %>' />
       </div>
      </div>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>     
    <table runat="server" id="reasonTable" class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       reason:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" TextMode="MultiLine" Rows="5" ID="inputReason"
        runat="server" />
       <div class="sncore_description">
        note: if you don't supply a reason while accepting or rejecting requests,
        no e-mail will be sent
       </div>
      </td>
     </tr>
    </table>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
