<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreate.aspx.cs"
 Inherits="AccountCreate" Title="Join" %>

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
     &#187; <a href="AccountCreateOpenId.aspx">join here</a> if you have an <a href="http://www.videntity.org"
      target="_blank">open-id</a>
    </td>
    <td align="right">
     already have an account? &#187; <a href="AccountLogin.aspx">login</a>
    </td>
   </tr>
  </table>
 </div>
 <table class="sncore_notice_error">
  <tr>
   <td>
    If you are using Safari or Opera browsers you will run into many known issues. Clicking on some buttons
    will appear to hang. Please switch to Internet Explorer or Firefox. We are working on resolving this.
   </td>
  </tr>
 </table>
 <atlas:UpdatePanel ID="panelJoin" runat="server" Mode="Always">
  <ContentTemplate>
   <asp:Panel ID="panelCreate" runat="server">
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
       birthday:
      </td>
      <td class="sncore_form_value">
       <SnCore:SelectDate ID="inputBirthday" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <div class="sncore_description">
        your birthday is required for your protection when resetting passwords
       </div>
      </td>
     </tr>
    </table>
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       e-mail address:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <div class="sncore_description">
        use your e-mail address to login, it will not be published to other users
        <br />
        <font color='red'>please double-check your e-mail address</font>
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       password:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword"
        runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       re-type password:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="Password" CssClass="sncore_form_textbox" ID="inputPassword2"
        runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <div class="sncore_description">
        the system stores a password hash, not your clear-text password
       </div>
      </td>
     </tr>
    </table>
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="inputCreate" runat="server" Text="Join!" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="create_Click" />
      </td>
     </tr>
    </table>
   </asp:Panel>
  </ContentTemplate>
 </atlas:UpdatePanel>
 <table class="sncore_notice_warning">
  <tr>
   <td>
    Before you can post anything you will have to confirm your e-mail address. We will
    send you an e-mail to do so. Sometimes Junk Mail filters treat the confirmation
    e-mail as unwanted spam. Please make sure to double-check your Junk Mail folder.
   </td>
  </tr>
 </table>
</asp:Content>
