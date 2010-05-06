<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountCreateWelcome.aspx.cs"
 Inherits="AccountCreateWelcome" Title="Welcome" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Welcome!
 </div>
 <div class="sncore_h2sub">
  <a href="Default.aspx">&#187; Return to HomePage</a>
  <a href="AccountManage.aspx">&#187; Go to Me Me</a>
  <a href="AccountPicturesManage.aspx">&#187; Upload a Picture</a>
 </div>
 <table class="sncore_notice_info">
  <tr>
   <td class="sncore_notice_tr_td">
    <p>
     Dear <asp:Label ID="labelAccountName" runat="server" />. Your account has been created and you are now logged in.
    </p>
   </td>
  </tr>
 </table>
 <asp:Panel id="panelEmailNotConfirmed" runat="server">
  <table class="sncore_account_table">
   <tr>
    <td>
     <p>
      But before you can post anything you will have to confirm your e-mail address. We have sent you an e-mail with instructions 
      to do so. It may take a few minutes to arrive. Also Junk Mail filters may treat the confirmation e-mail as unwanted spam. 
      Please make sure to double-check your Junk Mail folder.
     </p>
     <p>
      While you wait for that confirmation e-mail <a href="AccountPicturesManage.aspx">upload a profile picture</a> and 
      <a href="AccountPreferencesManage.aspx">adjust your preferences</a>.  
     </p>   
    </td>
   </tr>
  </table>
 </asp:Panel>
 <asp:Panel id="panelEmailConfirmed" runat="server">
  <table class="sncore_account_table">
   <tr>
    <td>
     <p>
      You can now <a href="AccountPicturesManage.aspx">upload a profile picture</a> and 
      <a href="AccountPreferencesManage.aspx">adjust your preferences</a>.
     </p>   
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
