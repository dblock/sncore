<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountPropertyGroupEdit.aspx.cs" Inherits="AccountPropertyGroupEdit" Title="Account | Profile" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <table width="100%">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label ID="labelName" runat="server" />
    </div>
   </td>
   <td>
    <div class="sncore_description">
     <asp:Label ID="labelDescription" runat="server" />
    </div>
   </td>
  </tr>
 </table>
 <div class="sncore_cancel">
  <asp:HyperLink runat="server" NavigateUrl="~/AccountPreferencesManage.aspx" ID="linkBack" Text="&#187; Cancel" />
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <asp:DataGrid CellPadding="4" runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table" 
    AllowPaging="false" AllowCustomPaging="false">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn ItemStyle-CssClass="sncore_form_label">
      <ItemTemplate>
       <asp:HiddenField ID="Id" runat="server" Value='<%# Eval("Id") %>' />
       <asp:HiddenField ID="propertyId" runat="server" Value='<%# Eval("AccountProperty.Id") %>' />
       <%# Render(Eval("AccountProperty.Name")) %> :
      </ItemTemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-CssClass="sncore_form_value">
      <ItemTemplate>
       <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="array_value" 
        Visible='<%# ((Type) Eval("AccountProperty.Type")).ToString() == "System.Array" %>' Text='<%# Eval("Value").ToString().Trim("\"".ToCharArray()).Replace("\"\"", ", ") %>' />
       <asp:TextBox TextMode="MultiLine" Rows="5" CssClass="sncore_form_textbox" runat="server" ID="text_value" 
        Visible='<%# ((Type) Eval("AccountProperty.Type")).ToString() == "System.Text.StringBuilder" %>' Text='<%# Eval("Value").ToString() %>' />
       <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="string_value" 
        Visible='<%# ((Type) Eval("AccountProperty.Type")).ToString() == "System.String" %>' Text='<%# Eval("Value").ToString() %>' />
       <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="int_value" 
        Visible='<%# ((Type) Eval("AccountProperty.Type")).ToString() == "System.Int32" %>' Text='<%# Eval("Value").ToString() %>' />
       <asp:CheckBox CssClass="sncore_form_checkbox" runat="server" ID="bool_value" 
        Visible='<%# ((Type) Eval("AccountProperty.Type")).ToString() == "System.Boolean" %>' Checked='<%# ((string) Eval("Value")) == "True" %>' />
       <div class="sncore_description">
        <%# Render(Eval("AccountProperty.Description")) %>
       </div>
      </ItemTemplate>
     </asp:TemplateColumn>
    </Columns>
   </asp:DataGrid>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      <SnCoreWebControls:Button ID="save" runat="server" Text="Save" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
