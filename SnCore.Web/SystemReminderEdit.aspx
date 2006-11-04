<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemReminderEdit.aspx.cs" Inherits="SystemReminderEdit" Title="Reminder" %>

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
     Reminder
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemRemindersManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       url:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputUrl" runat="server" />
       <asp:RequiredFieldValidator ID="inputUrlRequired" runat="server" ControlToValidate="inputUrl"
        CssClass="sncore_form_validator" ErrorMessage="Url is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       delta (hours):
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDeltaHours" runat="server">
        <asp:ListItem Text="Every 1 Hour" Value="1" />
        <asp:ListItem Text="Every 6 Hours" Value="6" />
        <asp:ListItem Text="Every 12 Hours" Value="12" />
        <asp:ListItem Text="Every 24 Hours" Value="24" />
        <asp:ListItem Text="Every 48 Hours" Value="48" />
        <asp:ListItem Text="Every 72 Hours" Value="72" />
        <asp:ListItem Text="Once a Week" Value="168" Selected="true" />
        <asp:ListItem Text="Twice a Month" Value="336" />
        <asp:ListItem Text="Every 3 Weeks" Value="504" />
        <asp:ListItem Text="Once a Month" Value="672" />
       </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       watch:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDataObject" OnSelectedIndexChanged="inputDataObject_SelectedIndexChanged"
        DataTextField="Name" DataValueField="Id" AutoPostBack="True" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       field:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDataObjectField" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputRecurrent" runat="server"
        Text="Recurrent" Checked="True" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputEnabled" runat="server"
        Text="Enabled" Checked="False" />
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
    <asp:UpdatePanel runat="server" id="panelAccountProperties">
     <ContentTemplate>     
      <div class="sncore_h2">
       Account Properties
      </div>
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         group:
        </td>
        <td class="sncore_form_value">
         <asp:DropDownList ID="inputAccountPropertyGroup" runat="server" DataValueField="Id" DataTextField="Name" CssClass="sncore_form_dropdown" 
          OnSelectedIndexChanged="inputAccountPropertyGroup_SelectedIndexChanged" AutoPostBack="true" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         property:
        </td>
        <td class="sncore_form_value">
         <asp:DropDownList ID="inputAccountProperty" runat="server" DataValueField="Id" DataTextField="Name" CssClass="sncore_form_dropdown" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         value:
        </td>
        <td class="sncore_form_value">
         <asp:TextBox ID="inputAccountPropertyValue" runat="server" CssClass="sncore_form_textbox" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
        </td>
        <td class="sncore_form_value">
         <asp:CheckBox ID="inputAccountPropertyEmpty" runat="server" CssClass="sncore_form_checkbox" Checked="true" Text="include unset" />
        </td>
       </tr>
       <tr>
        <td>
        </td>
        <td class="sncore_form_value">
         <SnCoreWebControls:Button ID="addAccountProperty" runat="server" Text="Add" CausesValidation="true" CssClass="sncore_form_button"
          OnClick="addAccountProperty_Click" />
        </td>
       </tr>
      </table>      
      <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManageReminderAccountProperties" PageSize="15" 
       AllowPaging="true" AllowCustomPaging="true" OnItemCommand="gridManageReminderAccountProperties_ItemCommand" 
       AutoGenerateColumns="false" CssClass="sncore_account_table">
       <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="center" />
       <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="center" />
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
        PrevPageText="Prev" HorizontalAlign="Center" />
       <Columns>
        <asp:BoundColumn DataField="Id" Visible="false" />
        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
         <itemtemplate>
          <img src="images/Item.gif" />
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Name">
         <itemtemplate>
          <%# base.Render(Eval("AccountPropertyName")) %>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Value">
         <itemtemplate>
          <%# base.Render(Eval("Value")) %>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="UnSet">
         <itemtemplate>
          <%# Eval("UnSet") %>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
       </Columns>
      </SnCoreWebControls:PagedGrid>     
      <div class="sncore_h2">
       Test Reminder
      </div>
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         account id:
        </td>
        <td class="sncore_form_value">
         <asp:TextBox id="inputTestAccountId" runat="server" CssClass="sncore_form_textbox" />         
        </td>
       </tr>
       <tr>
        <td>
        </td>
        <td class="sncore_form_value">
         <SnCoreWebControls:Button ID="inputTest" runat="server" Text="Test" CausesValidation="true" CssClass="sncore_form_button"
          OnClick="inputTest_Click" />
        </td>
       </tr>
      </table>    
     </ContentTemplate>
    </asp:UpdatePanel>    
   </td>
  </tr>
 </table>
</asp:Content>
