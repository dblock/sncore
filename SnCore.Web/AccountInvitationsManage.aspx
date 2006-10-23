<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountInvitationsManage.aspx.cs"
 Inherits="AccountInvitationsManage" Title="Account | Invitations" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <asp:Panel ID="panelInvite" runat="server">
     <div class="sncore_h2">
      Invite Friends
     </div>
     <asp:UpdatePanel runat="server" ID="panelGridManage" UpdateMode="Always" RenderMode="Inline">
      <ContentTemplate>
       <SnCore:Notice ID="noticeManage" runat="server" />
       <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
        ShowSummary="true" />
       <table class="sncore_account_table">
        <tr>
         <td class="sncore_form_label">
          e-mail address(es):
         </td>
         <td class="sncore_form_value">
          <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" TextMode="MultiLine"
           Rows="3" runat="server" />
          <asp:RequiredFieldValidator ID="inputEmailAddressRequired" runat="server" ControlToValidate="inputEmailAddress"
           CssClass="sncore_form_validator" Display="None" ErrorMessage="at least one e-mail address is required" />
          <div class="sncore_description">
           one per line or separated with semicolons
          </div>
         </td>
        </tr>
        <tr>
         <td class="sncore_form_label">
          optional message:
         </td>
         <td class="sncore_form_value">
          <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" TextMode="MultiLine"
           Rows="5" runat="server" />
         </td>
        </tr>
        <tr>
         <td>
         </td>
         <td class="sncore_form_value">
          <SnCoreWebControls:Button ID="invite" runat="server" Text="Invite" CausesValidation="true"
           CssClass="sncore_form_button" OnClick="invite_Click" />
         </td>
        </tr>
       </table>
       <div class="sncore_h2">
        Pending Invitations
       </div>
       <SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" PageSize="10" runat="server"
        ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table" 
        OnItemCommand="gridManage_ItemCommand" AllowCustomPaging="true">
        <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
         PrevPageText="Prev" HorizontalAlign="Center" />
        <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
        <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
        <Columns>
         <asp:BoundColumn DataField="Id" Visible="false" />
         <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
          <itemtemplate>
           <img src="images/Item.gif" />
          </itemtemplate>
         </asp:TemplateColumn>
         <asp:TemplateColumn HeaderText="Email Address">
          <itemtemplate>
           <%# base.Render(Eval("Email")) %>
          </itemtemplate>
         </asp:TemplateColumn>
         <asp:TemplateColumn HeaderText="Sent">
          <itemtemplate>
           <%# base.Adjust(Eval("Created")).ToString("d") %>
          </itemtemplate>
         </asp:TemplateColumn>
         <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
        </Columns>
       </SnCoreWebControls:PagedGrid>
      </ContentTemplate>
     </asp:UpdatePanel>
     <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
