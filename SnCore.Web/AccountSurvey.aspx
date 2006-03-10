<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountSurvey.aspx.cs" Inherits="AccountSurvey" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     <asp:Label ID="surveyName" runat="server" Text="Survey" />
    </div>
    <asp:Wizard ID="surveyWizard" runat="server" CssClass="sncore_account_table" OnActiveStepChanged="surveyWizard_ActiveStepChanged">
     <NavigationButtonStyle CssClass="sncore_form_button" />
     <NavigationStyle HorizontalAlign="Left" CssClass="sncore_table_tr_td" />
     <StepStyle CssClass="sncore_table_tr_td" />
     <SideBarStyle CssClass="sncore_table_tr_td" VerticalAlign="top" />
     <WizardSteps>
      <asp:WizardStep StepType="Complete">
       <div style="padding-left: 20px; color: Green; font-weight: bold;">
        Thanks! You can edit these answers <a href="AccountSurveysManage.aspx">here</a>
        and take new surveys about exciting tasty things!
       </div>
      </asp:WizardStep>
     </WizardSteps>
    </asp:Wizard>
   </td>
  </tr>
 </table>
</asp:Content>
