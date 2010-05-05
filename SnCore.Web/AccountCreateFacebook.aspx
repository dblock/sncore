<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreateFacebook.aspx.cs"
 Inherits="AccountCreateFacebook" Title="Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <div class="sncore_h2sub">
  Already have an account? <a href="AccountLogin.aspx">&#187; Login</a>
  <a href="AccountCreate.aspx">&#187; Join w/o Facebook</a>
 </div>
 <asp:UpdatePanel ID="panelJoin" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <asp:Panel ID="panelCreateFacebook" runat="server">
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
    <asp:Panel ID="panelFacebook" runat="server">
     <div class="sncore_h2">
      Login to Facebook
     </div>
     <table class="sncore_table">
      <tr>
       <td>
       </td>
       <td>
        <a href="<% Response.Write(FacebookLoginUri); %>">
         <img border="0" src="images/login/facebook.jpg" alt="Login with Facebook" />
        </a>
       </td>
      </table>
    </asp:Panel>
    <asp:Panel ID="panelIdentity" runat="server">
     <div class="sncore_h2">
      Almost Done
     </div>
     <div class="sncore_h2sub">
      To establish your identity, we're asking you to supply the following additional information.
     </div>
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
         a valid e-mail address is required to activate your account 
         <br />
         <font color='red'>please double-check your e-mail address</font>
        </div>
       </td>
      </tr>
     </table>
     <table class="sncore_table">
      <tr>
       <td class="sncore_form_label">
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="inputCreateFacebook" runat="server" Text="Join!" CausesValidation="true"
         CssClass="sncore_form_button" OnClick="CreateFacebook_Click" />
       </td>
       <td class="sncore_link">
        By joining, you automatically accept the <a href="TermsOfUse.aspx">Terms of Use</a>.
       </td>
      </tr>
     </table>
    </asp:Panel>
   </asp:Panel>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
