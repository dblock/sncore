<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacesView.aspx.cs"
 Inherits="PlacesView" Title="Places" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceNeighborhoodsView" Src="PlaceNeighborhoodsViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Cities ID="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
 <SnCore:PlaceNeighborhoodsView ID="neighborhoods" runat="server" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true"
  RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal" CssClass="sncore_table"
  ShowHeader="false">
  <pagerstyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" PageButtonCount="5" />
  <ItemTemplate>
   <div>
    <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
     <%# Renderer.Render(Eval("Name")) %>
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("Neighborhood")) %>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("City")) %>
    <%# Renderer.Render(Eval("State"))%>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("Country"))%>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
 <asp:TextBox ID="inputName" CssClass="sncore_form_textbox" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name"
  AutoPostBack="true" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
  DataTextField="Name" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
  DataValueField="Name" runat="server" AutoPostBack="true" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputNeighborhood" DataTextField="Name"
  DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputType" DataTextField="Name"
  DataValueField="Name" runat="server" Visible="false" />
 <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
  Text="show places with pictures only" Checked="false" Visible="false" />
</asp:Content>
