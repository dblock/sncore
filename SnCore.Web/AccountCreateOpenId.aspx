<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreateOpenId.aspx.cs"
 Inherits="AccountCreateOpenId" Title="Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <div class="sncore_h2sub">
  <table width="100%">
   <tr>
    <td align="left">
     &#187; <a href="AccountCreate.aspx">join here</a> if you don't have an <a href="http://www.videntity.org"
      target="_blank">open-id</a>
    </td>
    <td align="right">
     already have an account? &#187; <a href="AccountLogin.aspx">login</a>
    </td>
   </tr>
  </table>
 </div>
 <asp:UpdatePanel ID="panelJoin" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <asp:Panel ID="panelCreateOpenId" runat="server">
    <asp:Panel ID="panelBeta" runat="server">
     <table class="sncore_table">
      <tr>
       <td>
        This site is in <b>beta</b>. We would like to control the size of the user population
        until it reaches critical mass. If you don't have the beta password and wish to
        join, please do
        <asp:LinkButton ID="linkAdministrator" runat="server" Text="e-mail the administrator"
         CausesValidation="false" />
        for an invite.
       </td>
      </tr>
     </table>
     <table class="sncore_table">
      <tr>
       <td class="sncore_form_label" style="color: Red; font-weight: bold;">
        beta password:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputBetaPassword"
         runat="server" />
       </td>
      </tr>
     </table>
    </asp:Panel>
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       your name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <div class="sncore_description">
        this is how other users will see you
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       <a href="http://www.videntity.org" target="_blank">your open-id</a>
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_openid_textbox" ID="inputOpenId" runat="server" />
      </td>
     </tr>
    </table>
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="inputCreateOpenId" runat="server" Text="Join!" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="CreateOpenId_Click" />
      </td>
      <td class="sncore_link">
       By joining, you automatically accept the <a href="TermsOfUse.aspx">Terms of Use</a>.
      </td>
     </tr>
    </table>
   </asp:Panel>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
