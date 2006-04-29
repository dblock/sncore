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
     Friend Requests
    </div>
    <asp:Panel ID="panelPending" runat="server">
     <div class="sncore_h2">
      Pending
     </div>
     <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridPending" HeaderStyle-CssClass="sncore_table_tr_th"
      AutoGenerateColumns="false" CssClass="sncore_account_table" ShowHeader="false" OnItemCommand="gridPending_ItemCommand">
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn>
        <itemtemplate>
         <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
          <img border="0" id="imageAccount" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
          <br />
          <%# base.Render(Eval("AccountName")) %>
         </a>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
        <itemtemplate>
         <%# base.RenderEx(Eval("Message")) %>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn>
        <itemtemplate>
         <%# base.Adjust(Eval("Created")).ToString() %>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Accept" Text="Accept" />
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Reject" Text="Reject" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
     <table runat="server" id="reasonTable" class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        reason:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" TextMode="MultiLine" Rows="5" ID="inputReason"
         runat="server" />
        <br />
        note: if you don't supply a reason while accepting or rejecting requests,
        <br />
        no e-mail will be sent
       </td>
      </tr>
     </table>
    </asp:Panel>
    <asp:Panel ID="panelSent" runat="server">
     <div class="sncore_h2">
      Sent
     </div>
     <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridSent" AutoGenerateColumns="false"
      CssClass="sncore_account_table" ShowHeader="false" OnItemCommand="gridSent_ItemCommand">
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn>
        <itemtemplate>
         <a href="AccountView.aspx?id=<%# Eval("KeenId") %>">
          <img border="0" id="imageAccount" src="AccountPictureThumbnail.aspx?id=<%# Eval("KeenPictureId") %>" />
          <br />
          <%# base.Render(Eval("KeenName")) %>
         </a>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn>
        <itemtemplate>
         <%# base.Adjust(Eval("Created")).ToString() %>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Cancel" Text="&#187; Cancel" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
    </asp:Panel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
