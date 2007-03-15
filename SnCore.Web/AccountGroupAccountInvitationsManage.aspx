<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountGroupAccountInvitationsManage.aspx.cs"
 Inherits="AccountGroupAccountInvitationsManage" Title="Group | Membership Invitations" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Pending Invitations
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="listPending" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="listPending_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="1"
    RepeatRows="2" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <table cellpadding="4" cellspacing="0">
      <tr>
       <td align="center">
        <a href="AccountGroupView.aspx?id=<%# Eval("AccountGroupId") %>">
         <img border="0" src="AccountGroupPictureThumbnail.aspx?id=<%# Eval("AccountGroupPictureId") %>" />
         <div style="font-size: smaller;">
          <%# base.Render(Eval("AccountGroupName")) %>
         </div>
        </a>
        <div class="sncore_description">
         <%# base.Adjust(Eval("Created")).ToString() %>
        </div>
       </td>
       <td align="left" valign="top">
        <div style="border-bottom: solid 1px black; margin-bottom: 10px; padding-bottom: 10px;">
         <a href="AccountView.aspx?id=<%# Eval("RequesterId") %>">
          <%# base.Render(Eval("RequesterName")) %>
         </a>
         invited you to join
         <a href="AccountGroupView.aspx?id=<%# Eval("AccountGroupId") %>">
          <%# base.Render(Eval("AccountGroupName")) %>
         </a>
        </div>
        <div>
         <%# base.RenderEx(Eval("Message")) %>
        </div>
        <div style="border-top: solid 1px black; margin-top: 10px; padding-top: 10px;">
         <div>
          <asp:LinkButton Text="&#187; Accept Request" ID="linkAccept" runat="server"
           CommandName="Accept" CommandArgument='<%# Eval("Id") %>' />
         </div>
         <div>
          <asp:LinkButton Text="&#187; Reject Request" ID="linkReject" runat="server" 
           OnClientClick="return confirm('Are you sure you want to reject this request?')"
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
      optional note:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" TextMode="MultiLine" Rows="5" ID="inputReason"
       runat="server" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
