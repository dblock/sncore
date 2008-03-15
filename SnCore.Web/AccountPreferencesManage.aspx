<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountPreferencesManage.aspx.cs"
 Inherits="AccountPreferencesManage" Title="Account | Preferences" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectTimeZone" Src="SelectTimeZoneControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountPropertyGroups" Src="AccountPropertyGroupsControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SystemMenu" Src="SystemMenuControl.ascx" %>
<%@ Register tagprefix="SnCore" tagname="Notice" src="NoticeControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titlePreferences" Text="Preferences" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    This is your opportunity to choose your <b>name</b> and <b>birthday</b>. You should set your birthday date to a
    true one because it is used for password reminders and is not visible to other members of the site. 
   </div>
   <div class="sncore_title_paragraph">       
    Enter the <b>city</b>, <b>country</b> and <b>state</b> you live in. When you check out 
    <a href="AccountsView.aspx">members</a> and <a href="PlacesView.aspx">places</a> local ones will
    appear first.
   </div>
   <div>       
    You don't have to pick a timezone, especially if you travel a lot - the system will pickup your browser's time. 
    And your signature will appear at the bottom of all your <a href="DiscussionsView.aspx">discussion posts</a>.
   </div>
  </Template>
 </SnCore:Title>
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
    <asp:UpdatePanel runat="server" ID="panelCity" RenderMode="Inline" UpdateMode="Conditional">
     <ContentTemplate>
      <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoCompleteCity" TargetControlID="inputCity"
       ServiceMethod="GetCitiesCompletionList" ServicePath="ScriptServices.asmx" MinimumPrefixLength="0" 
       CompletionInterval="500" EnableCaching="true" CompletionSetCount="25" UseContextKey="true" />
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" MaxLength="64" runat="server" />
     </ContentTemplate>
    </asp:UpdatePanel>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    country and state:
   </td>
   <td class="sncore_form_value">
    <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
     <ContentTemplate>     
     <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name" 
      AutoPostBack="true" DataValueField="Name" runat="server" />
     <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
      DataValueField="Name" runat="server" AutoPostBack="true" />
     </ContentTemplate>
    </asp:UpdatePanel>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    language &amp; region:
   </td>
   <td class="sncore_form_value">
    <asp:UpdatePanel runat="server" ID="panelLocale" UpdateMode="Conditional">
     <ContentTemplate>
     <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputLocale" 
      DataTextField="NativeName" AutoPostBack="true" DataValueField="LCID" runat="server" />
     </ContentTemplate>
    </asp:UpdatePanel>
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
 <SnCore:AccountRedirectEdit id="accountredirect" runat="server" />
 <div class="sncore_h2">
  Content Licensing
 </div>
 <div class="sncore_h2sub">
   <a href="AccountLicenseEdit.aspx">&#187; Choose License</a>
 </div>
 <div class="sncore_h2">
  Security
 </div>
 <div class="sncore_h2sub">
   <a href="AccountChangePassword.aspx">&#187; Change Password</a>
 </div>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
