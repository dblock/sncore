<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreate.aspx.cs"
 Inherits="AccountCreate" Title="Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Join!
 </div>
 <div class="sncore_h2sub">
  Three ways to join this site! &#187; Already have an account? <a href="AccountLogin.aspx">&#187; Login</a>
 </div>
 <asp:UpdatePanel ID="panelJoin" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <table class="sncore_table">
    <tr>
     <td align="center">
      <a href="AccountCreateEmail.aspx"><img border="0" src="images/signup/email.jpg" alt="E-Mail" /></a>
      <br />
      <a href="AccountCreateEmail.aspx">e-mail</a>
     </td>
     <td align="center">
      <a href="AccountCreateFacebook.aspx"><img border="0" src="images/signup/facebook.jpg" alt="Facebook" /></a>
      <br />
      <a href="AccountCreateFacebook.aspx">facebook</a>
     </td>
     <td align="center">
      <a href="AccountCreateOpenId.aspx"><img border="0" src="images/signup/openid.jpg" alt="Open-Id" /></a>
      <br />
      <a href="AccountCreateOpenId.aspx">open-id</a>
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
