<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountsView.aspx.cs"
 Inherits="AccountsView" Title="Foodies" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Cities ID="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
 <sncorewebcontrols:pagedlist cellpadding="4" runat="server" id="gridManage" 
  allowcustompaging="true" repeatcolumns="1" repeatrows="7" repeatdirection="Horizontal"
  cssclass="sncore_table" showheader="false">
  <PagerStyle PageButtonCount="5" cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" />
  <ItemTemplate>
   <div>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
    </a>
   </div>
   <div>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
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
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name"
  AutoPostBack="true" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
  DataTextField="Name" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
  DataValueField="Name" runat="server" AutoPostBack="true" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputNeighborhood" DataTextField="Name"
  DataValueField="Name" runat="server" Visible="false" />
 <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
  Text="show people with pictures only" Checked="false" Visible="false" />
</asp:Content>
