<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemNeighborhoodEdit.aspx.cs" Inherits="SystemNeighborhoodEdit" Title="System | Neighborhood" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Neighborhood
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemNeighborhoodsManage.aspx"
  runat="server" />
 <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
  ShowSummary="true" />
 <asp:UpdatePanel id="panelNeighborhood" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <table class="sncore_account_table">
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
       DataValueField="Name" runat="server" OnSelectedIndexChanged="inputState_SelectedIndexChanged"
       AutoPostBack="true" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      city:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
       DataValueField="Name" runat="server" />
     </td>
    </tr>
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
      <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
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
      <asp:TemplateColumn HeaderText="Neighborhood" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
        <%# base.Render(Eval("Name")) %>
        <div class="sncore_description">
         <%# base.Render(Eval("Country")) %>
         <%# base.Render(Eval("State")) %>
         <%# base.Render(Eval("City")) %>
        </div>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <ItemTemplate>
        <asp:LinkButton runat="server" id="mergeThis" CommandName="MergeThis" 
         Text="Merge This" CommandArgument='<%# Eval("Id") %>' />
       </ItemTemplate>          
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <ItemTemplate>
        <asp:LinkButton runat="server" id="mergeTo" CommandName="MergeTo" 
         Text="Merge To" CommandArgument='<%# Eval("Id") %>' />
       </ItemTemplate>          
      </asp:TemplateColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>     
   </ContentTemplate>
  </asp:UpdatePanel>
 </asp:Panel>
</asp:Content>
