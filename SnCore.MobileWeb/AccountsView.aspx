<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountsView.aspx.cs"
 Inherits="AccountsView" Title="People" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Cities ID="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
 <div class="sncore_search">
  search:
  <asp:TextBox ID="inputSearch" runat="server" CssClass="sncore_form_textbox" />
  <SnCoreWebControls:Button id="search" runat="server" text="Search" causesvalidation="true"
   cssclass="sncore_form_button" onclick="search_Click" />
 </div>
 <sncorewebcontrols:pagedlist cellpadding="4" runat="server" id="gridManage" 
  allowcustompaging="true" repeatcolumns="1" repeatrows="7" repeatdirection="Horizontal"
  cssclass="sncore_table" showheader="false">
  <PagerStyle PageButtonCount="5" cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemTemplate>
   <div>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>">
     <%# Renderer.Render(Eval("Name")) %>
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("City")) %>
    <%# Renderer.Render(Eval("State"))%>
    <%# Renderer.Render(Eval("Country"))%>
   </div>
  </ItemTemplate>
 </sncorewebcontrols:pagedlist>
 <div style="display: none;">
  <div>
   country:
   <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name"
    AutoPostBack="true" DataValueField="Name" runat="server" />
  </div>
  <div>
   state:
   <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
    DataTextField="Name" DataValueField="Name" runat="server" />
  </div>
  <div>
   city:
   <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
    DataValueField="Name" runat="server" AutoPostBack="true" />
  </div>
  <div>
   neighborhood:
   <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputNeighborhood" DataTextField="Name"
    DataValueField="Name" runat="server" />
  </div>
  <div>
   type:
   <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputType" DataTextField="Name"
    DataValueField="Name" runat="server" />
  </div>
  <div>
   <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
    Text="show places with pictures only" Checked="false" />
  </div>
 </div>
</asp:Content>
