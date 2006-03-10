<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountInvitationsManage.aspx.cs" Inherits="AccountInvitationsManage"
 Title="Account | Invitations" %>

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
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     Invite Friends
    </div>
    <SnCore:Notice ID="noticeManage" runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       e-mail address(es):
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" TextMode="MultiLine" Rows="3" runat="server" />
       <asp:RequiredFieldValidator ID="inputEmailAddressRequired" runat="server" ControlToValidate="inputEmailAddress"
        CssClass="sncore_form_validator" Display="None" ErrorMessage="at least one e-mail address is required" />
       one per line or separated with semicolons
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       message:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" TextMode="MultiLine"
        Rows="10" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="invite" runat="server" Text="Invite" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="invite_Click" />
      </td>
     </tr>
    </table>
    <div class="sncore_h2">
     Pending Invitations
    </div>
    <SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" PageSize="15" runat="server" ID="gridManage"
     AutoGenerateColumns="false" CssClass="sncore_account_table" OnItemCommand="gridManage_ItemCommand">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
       HeaderText="EMail">
       <itemtemplate>
      <%# base.Render(Eval("Email")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th"
       HeaderText="Sent">
       <itemtemplate>
      <%# base.Adjust(Eval("Created")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
