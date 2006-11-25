<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemCityEdit.aspx.cs" Inherits="SystemCityEdit" Title="System | City" %>

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
     City
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemCitiesManage.aspx"
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
      <td class="sncore_form_label">
       tag:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputTag" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country:</td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCountry" DataTextField="Name"
        DataValueField="Name" runat="server" AutoPostBack="true" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged" />
       <asp:RequiredFieldValidator ID="inputCountryRequired" runat="server" ControlToValidate="inputCountry"
        CssClass="sncore_form_validator" ErrorMessage="country is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       state:</td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputState" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="save_Click" />
      </td>
     </tr>
    </table>
    <asp:Panel ID="panelMerge" runat="server">
     <div class="sncore_h2">
      Merge
     </div>
     <asp:UpdatePanel id="panelMergeUpdate" runat="server" UpdateMode="Always">
      <ContentTemplate>
       <table class="sncore_account_table">
        <tr>
         <td class="sncore_form_label">
          what:
         </td>
         <td class="sncore_form_value">
          <asp:TextBox CssClass="sncore_form_textbox" ID="inputMergeWhat" runat="server" />
         </td>
        </tr>
        <tr>
         <td>
         </td>
         <td class="sncore_form_value">
          <SnCoreWebControls:Button ID="mergeLookup" runat="server" Text="Lookup" CausesValidation="true" 
           CssClass="sncore_form_button" OnClick="mergeLookup_Click" />
         </td>
        </tr>
       </table>
       <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridMergeLookup_ItemCommand"
        runat="server" ID="gridMergeLookup" AutoGenerateColumns="false" CssClass="sncore_account_table"
        AllowPaging="False">
        <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
        <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
        <Columns>
         <asp:BoundColumn DataField="Id" Visible="false" />
         <asp:BoundColumn DataField="Name" Visible="false" />
         <asp:TemplateColumn>
          <itemtemplate>
           <img src="images/Item.gif" />
          </itemtemplate>
         </asp:TemplateColumn>
         <asp:TemplateColumn HeaderText="City" ItemStyle-HorizontalAlign="Left">
          <itemtemplate>
           <%# base.Render(Eval("Name")) %>
           <div class="sncore_description">
            <%# base.Render(Eval("Country")) %>
            <%# base.Render(Eval("State")) %>
           </div>
          </itemtemplate>
         </asp:TemplateColumn>
         <asp:TemplateColumn>
          <ItemTemplate>
           <asp:LinkButton runat="server" id="merge" CommandName="Merge" 
            Text="Merge" CommandArgument='<%# Eval("Id") %>' />
          </ItemTemplate>          
         </asp:TemplateColumn>
        </Columns>
       </SnCoreWebControls:PagedGrid>     
      </ContentTemplate>
     </asp:UpdatePanel>
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
