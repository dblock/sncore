<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountDeclineInvitation.aspx.cs" Inherits="AccountDeclineInvitation" Title="SignUp" %>

<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Decline Invitation
 </div>
 <div class="sncore_h2sub">
  <a href="Default.aspx">&#187; Home Page</a>
 </div>
 <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <asp:Panel ID="panelDecline" runat="server">
  <table class="sncore_table">
   <tr>
    <td>
     Are you sure you want to decline <asp:HyperLink ID="linkAccount" runat="server" />'s invitation?     
     <p style="margin: 10px;">
      <SnCoreWebControls:Button ID="inputDecline" runat="server" Text="Decline" CausesValidation="true"
       CssClass="sncore_form_button" OnClick="decline_Click" />
     </p>
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
