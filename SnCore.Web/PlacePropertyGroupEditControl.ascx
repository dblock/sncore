<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacePropertyGroupEditControl.ascx.cs" Inherits="PlacePropertyGroupEditControl" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:DataGrid CellPadding="4" runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table" 
 AllowPaging="false" AllowCustomPaging="false" ShowHeader="false">
 <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" BorderColor="White" BorderWidth="0" />
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn ItemStyle-CssClass="sncore_form_label">
   <ItemTemplate>
    <asp:HiddenField ID="Id" runat="server" Value='<%# Eval("Id") %>' />
    <asp:HiddenField ID="propertyId" runat="server" Value='<%# Eval("PlacePropertyId") %>' />
    <%# Render(Eval("PlacePropertyName")) %> :
   </ItemTemplate>
  </asp:TemplateColumn>
  <asp:TemplateColumn ItemStyle-CssClass="sncore_form_value">
   <ItemTemplate>
    <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="array_value" 
     Visible='<%# ((string) Eval("PlacePropertyTypeName")) == "System.Array" %>' Text='<%# Eval("Value").ToString().Trim("\"".ToCharArray()).Replace("\"\"", ", ") %>' />
    <asp:TextBox TextMode="MultiLine" Rows="5" CssClass="sncore_form_textbox" runat="server" ID="text_value" 
     Visible='<%# ((string) Eval("PlacePropertyTypeName")) == "System.Text.StringBuilder" %>' Text='<%# Eval("Value").ToString() %>' />
    <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="string_value" 
     Visible='<%# ((string) Eval("PlacePropertyTypeName")) == "System.String" %>' Text='<%# Eval("Value").ToString() %>' />
    <asp:TextBox CssClass="sncore_form_textbox" runat="server" ID="int_value" 
     Visible='<%# ((string) Eval("PlacePropertyTypeName")) == "System.Int32" %>' Text='<%# Eval("Value").ToString() %>' />
    <asp:CheckBox CssClass="sncore_form_checkbox" runat="server" ID="bool_value" 
     Visible='<%# ((string) Eval("PlacePropertyTypeName")) == "System.Boolean" %>' Checked='<%# ((string) Eval("Value")) == "True" %>' />
    <div class="sncore_description">
     <%# Render(Eval("PlacePropertyDescription")) %>
    </div>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
