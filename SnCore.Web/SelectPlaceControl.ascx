<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectPlaceControl.ascx.cs"
 Inherits="SelectPlaceControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>

<asp:UpdatePanel ID="panelSelectPlace" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <table class="sncore_account_table">
   <tr>
    <td colspan="2">
     <div class="sncore_link" style="font-weight: bold;" id="panelWorking" runat="server">
     </div>
     <asp:Panel ID="panelButtons" CssClass="sncore_link" runat="server">
      <asp:LinkButton CausesValidation="false" ID="lookupPlace" runat="server" Text="&#187; lookup an existing location"
       OnClick="lookupPlace_Click" Enabled="false" />
      <asp:LinkButton CausesValidation="false" ID="addPlace" runat="server" Text="&#187; add a new location"
       OnClick="addPlace_Click" />
      </asp:Panel>
    </td>
   </tr>
  </table>
  <asp:Panel ID="panelPlace" runat="server">
   <asp:DataGrid AutoGenerateColumns="false" CssClass="sncore_account_table" ID="chosenPlace"
    runat="server">
    <HeaderStyle CssClass="sncore_table_tr_th" />
    <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <Columns>
     <asp:TemplateColumn>
      <ItemTemplate>
       <img src="images/Item.gif" />
      </ItemTemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="chosen location" ItemStyle-HorizontalAlign="Left">
      <ItemTemplate>
       <a href='PlaceView.aspx?id=<%# Eval("Id") %>' target="_blank">
        <%# base.Render(Eval("Name")) %>
       </a>
       <div class="sncore_description">
        <%# base.Render(Eval("Neighborhood")) %>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Street")) %>
        <%# base.Render(Eval("Zip")) %>
        <%# base.Render(Eval("City")) %>
       </div>
       <div class="sncore_description">     
        <%# base.Render(Eval("State")) %>
        <%# base.Render(Eval("Country")) %>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Phone")) %>
       </div>
      </ItemTemplate>
     </asp:TemplateColumn>
    </Columns>
   </asp:DataGrid>
   <SnCoreWebControls:PersistentPanel ID="panelLookup" runat="server">
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       existing location:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputLookupName" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <div style="color: red; font-weight: bold; font-size: smaller; margin-top: 2px; margin-bottom: 5px;">
        <asp:Label EnableViewState="false" ID="labelLookup" runat="server" />
       </div>
       <SnCoreWebControls:Button ID="buttonLookup" runat="server" Text="Lookup"
        CssClass="sncore_form_button" CausesValidation="false" OnClick="buttonLookup_Click" />
      </td>
     </tr>
    </table>
    <asp:DataList RepeatColumns="2" CssClass="sncore_account_table" ID="gridLookupPlaces"
     runat="server">
     <ItemStyle VerticalAlign="Top" CssClass="sncore_table_tr_td" Width="50%" />
     <ItemTemplate>
      <li>
       <asp:LinkButton runat="server" ID="lookupChoose" CausesValidation="false" OnCommand="lookupChoose_Command"
        Text='<%# base.Render(Eval("Name")) %>' CommandArgument='<%# Eval("Id") %>' />
       <span class="sncore_link">
        <asp:LinkButton runat="server" ID="lookupChoose2" CausesValidation="false" OnCommand="lookupChoose_Command"
         Text='&#187; choose' CommandArgument='<%# Eval("Id") %>' />
       </span>
       <div class="sncore_li_description">
        <%# base.Render(Eval("Neighborhood")) %>
       </div>
       <div class="sncore_li_description">
        <%# base.Render(Eval("Street")) %>
        <%# base.Render(Eval("Zip")) %>
        <%# base.Render(Eval("City")) %>
       </div>
       <div class="sncore_li_description">     
        <%# base.Render(Eval("State")) %>
        <%# base.Render(Eval("Country")) %>
       </div>
       <div class="sncore_li_description">
        <%# base.Render(Eval("Phone")) %>
       </div>
     </ItemTemplate>
    </asp:DataList>
   </SnCoreWebControls:PersistentPanel>
   <SnCoreWebControls:PersistentPanel ID="panelAdd" runat="server" Visible="false">
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectType" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country and state:</td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelSelectCountryState" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
          CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name" DataValueField="Name"
          runat="server" />
         <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
          DataValueField="Name" runat="server" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:</td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       neighborhood:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputNeighborhood" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       address:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputStreet" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       zip:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputZip" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       cross-street:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCrossStreet" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       phone:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPhone" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       fax:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputFax" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       e-mail:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmail" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       website:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputWebsite" runat="server" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </asp:Panel>
 </ContentTemplate>
</asp:UpdatePanel>
