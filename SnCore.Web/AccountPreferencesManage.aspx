<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountPreferencesManage.aspx.cs"
 Inherits="AccountPreferencesManage" Title="Account | Preferences" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectTimeZone" Src="SelectTimeZoneControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountPropertyGroups" Src="AccountPropertyGroupsControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <table cellspacing="0" cellpadding="4" class="sncore_account_table">
     <tr>
      <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
       <a runat="server" id="linkPictures" href="AccountView.aspx">
        <img border="0" src="images/AccountThumbnail.gif" runat="server" id="accountImage" />
       </a>
      </td>
      <td valign="top" width="*">
       <asp:Label CssClass="sncore_account_name" ID="accountName" runat="server" />
       <br />
       <br />
       <div style="padding: 10px;">
        <asp:HyperLink ID="accountFirstDegree" NavigateUrl="AccountFriendsView.aspx" runat="server" />
        <br />
        <asp:Label ID="accountSecondDegree" runat="server" />
        <br />
        <asp:HyperLink ID="accountAllDegrees" NavigateUrl="AccountsView.aspx" runat="server" />
        <br />
        <asp:HyperLink ID="accountDiscussionThreads" NavigateUrl="AccountDiscussionThreadsView.aspx"
         runat="server" />
       </div>
      </td>
     </tr>
    </table>
    <div class="sncore_h2">
     My Preferences
    </div>
    <asp:ValidationSummary runat="server" ID="inputValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" MaxLength="128" runat="server" />
       <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
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
      <td class="sncore_form_label">
       city:</td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" MaxLength="64" runat="server" /></td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country and state:
      </td>
      <td class="sncore_form_value">
       <atlas:UpdatePanel runat="server" ID="panelCountryState" Mode="Conditional">
        <ContentTemplate>     
        <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
         ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
         runat="server" />
        <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
         DataValueField="Name" runat="server" />
        </ContentTemplate>
       </atlas:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       timezone:</td>
      <td class="sncore_form_value">
       <SnCore:SelectTimeZone runat="server" ID="inputTimeZone" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       signature:</td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSignature" TextMode="MultiLine"
        Rows="3" MaxLength="128" runat="server" /></td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageSave" runat="server" Text="Save" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
      </td>
     </tr>
    </table>
    <SnCore:AccountPropertyGroups ID="groups" runat="server" />    
    <div class="sncore_h2">
     Security
    </div>
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       <img alt="" src="images/Item.gif" />
      </td>
      <td class="sncore_table_tr_td">
       <a href="AccountChangePassword.aspx">Change Password</a>
      </td>
     </tr>
    </table>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
