<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedsView.aspx.cs"
 Inherits="AccountFeedsView" Title="Blog Directory" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:Cities id="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
  AllowCustomPaging="true" RepeatRows="7" RepeatDirection="Vertical"
  CssClass="sncore_table" ShowHeader="false">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" PageButtonCount="5" />
  <ItemStyle CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <div>
    <a href='AccountFeedView.aspx?id=<%# Eval("Id") %>'>
     <%# Renderer.Render(Eval("Name")) %>
    </a>
    <span style="font-size: xx-small">
     <%# Renderer.GetLink(Renderer.Render(Eval("LinkUrl")), "&#187; x-posted") %>
    </span>
   </div>      
   <div class="sncore_description">
    <%# Renderer.GetSummary((string) Eval("Description")) %>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name"
  AutoPostBack="true" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
  DataTextField="Name" DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
  DataValueField="Name" runat="server" AutoPostBack="true" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputNeighborhood" DataTextField="Name"
  DataValueField="Name" runat="server" Visible="false" />
 <asp:DropDownList CssClass="sncore_form_dropdown" ID="DropDownList1" DataTextField="Name"
  DataValueField="Name" runat="server" Visible="false" />
</asp:Content>
