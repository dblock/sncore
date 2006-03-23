<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemSurveyEdit.aspx.cs"
 Inherits="SystemSurveyEdit" Title="System | Survey" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Survey
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemSurveysManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
       <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
      </td>
     </tr>
    </table>
    <asp:Panel ID="panelQuestions" runat="server">
     <div class="sncore_h2">
      Survey Questions
     </div>
     <asp:HyperLink ID="linkNewQuestion" Text="&#187; Create New" CssClass="sncore_createnew"
      NavigateUrl="SystemSurveyQuestionEdit.aspx" runat="server" />
     <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
      runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn>
        <itemtemplate>
         <img src="images/Item.gif" />
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="Question" ItemStyle-HorizontalAlign="Left">
        <itemtemplate>
         <%# base.Render(Eval("Question")) %>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Edit" Text="Edit" />
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
