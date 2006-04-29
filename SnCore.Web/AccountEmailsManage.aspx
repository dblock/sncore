<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEmailsManage.aspx.cs"
 Inherits="AccountEmailsManage" Title="Account | EMails" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <table class="sncore_table">
   <tr>
    <td valign="top" width="150">
     <SnCore:AccountMenu runat="server" ID="menu" />
    </td>
    <td valign="top" width="*">
     <div class="sncore_h2">
      My Email Addresses
     </div>
     <SnCoreWebControls:PagedGrid CellPadding="4" OnItemDataBound="gridManage_ItemDataBound"
      OnItemCommand="gridManage_ItemCommand" runat="server" ID="gridManage" AutoGenerateColumns="false"
      CssClass="sncore_account_table">
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:BoundColumn DataField="Address" Visible="false" />
       <asp:BoundColumn DataField="Verified" Visible="false" />
       <asp:BoundColumn DataField="Principal" Visible="false" />
       <asp:BoundColumn ReadOnly="True" DataFormatString="<img src='images/AccountEmailVerified{0}.gif'>"
        DataField="Verified"></asp:BoundColumn>
       <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Email Address">
        <itemtemplate>
         <a href="mailto:<%# base.Render(Eval("Address")) %>">
          <%# base.Render(Eval("Address")) %>
         </a>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
       <asp:ButtonColumn Visible="true" ButtonType="LinkButton" CommandName="Resend" DataTextField="Id"
        DataTextFormatString="Resend" />
       <asp:ButtonColumn Visible="true" ButtonType="LinkButton" CommandName="SetPrincipal"
        DataTextField="Id" DataTextFormatString="Set Principal" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
     <div class="sncore_h2">
      Add New</div>
     <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
      ShowSummary="true" />
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        e-mail address:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
        <asp:RequiredFieldValidator ID="inputEmailAddressRequired" runat="server" ControlToValidate="inputEmailAddress"
         CssClass="sncore_form_validator" ErrorMessage="e-mail address is required" Display="Dynamic" /><asp:RegularExpressionValidator
          ID="inputEmailAddressRegexRequired" runat="server" ControlToValidate="inputEmailAddress"
          ValidationExpression=".*@.*\..*" CssClass="sncore_form_validator" ErrorMessage="e-mail address is invalid"
          Display="Dynamic" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Add" CausesValidation="true"
         CssClass="sncore_form_button" OnClick="save_Click" />
       </td>
      </tr>
     </table>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
    </td>
   </tr>
  </table>
</asp:Content>
