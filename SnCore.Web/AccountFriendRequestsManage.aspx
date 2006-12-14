<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFriendRequestsManage.aspx.cs"
 Inherits="AccountFriendRequestsManage" Title="Account | Friend Requests" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Pending Friend Requests
 </div>
 <div class="sncore_h2sub">
  <a href="AccountFriendRequestsSentManage.aspx">&#187; Sent Requests</a>
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="listPending" Width="0px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="listPending_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="1"
    RepeatRows="2" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <table cellpadding="4" cellspacing="0">
      <tr>
       <td>
        <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
         <img border="0" 
          src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
         <div style="font-size: smaller;">
          <%# base.Render(Eval("AccountName")) %>
         </div>
        </a>
        <div class="sncore_description">
         <%# base.Adjust(Eval("Created")).ToString() %>
        </div>
       </td>
       <td align="left" valign="top">
        <div>
         <%# base.RenderEx(Eval("Message")) %>
        </div>
        <div style="border-top: solid 1px black; margin-top: 10px; padding-top: 10px;">
         <div>
          <asp:LinkButton Text="&#187; Accept Request" ID="linkAccept" runat="server"
           CommandName="Accept" CommandArgument='<%# Eval("Id") %>' />
         </div>
         <div>
          <asp:LinkButton Text="&#187; Reject Request" ID="linkReject" runat="server" OnClientClick="return confirm('Are you sure you want to reject this request?')"
           CommandName="Reject" CommandArgument='<%# Eval("Id") %>' />
         </div>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
    <table runat="server" cellpadding="4" id="reasonTable" class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       message:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" TextMode="MultiLine" Rows="5" ID="inputReason"
        runat="server" />
       <div class="sncore_description">
        if you don't enter a reason while accepting or rejecting requests,
        no e-mail will be sent
       </div>
      </td>
     </tr>
    </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
