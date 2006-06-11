<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountEventsView.aspx.cs" Inherits="AccountEventsView" Title="Events" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Events
    </div>
    <div class="sncore_h2sub">
     <a href="AccountEventsToday.aspx">&#187; Events This Week</a>
     <asp:LinkButton ID="linkShowAll" runat="server" Text="&#187; All Events" OnClick="linkShowAll_Click" />
     <a href="AccountEventEdit.aspx">&#187; Suggest an Event</a>
    </div>
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountEventsRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountEventsRss.aspx" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
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
    sort by:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectSortOrder"
     runat="server">
     <asp:ListItem Text="Name" Value="Name" />
     <asp:ListItem Text="Date Created" Selected="True" Value="Created" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    order by:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectOrderBy" runat="server">
     <asp:ListItem Selected="True" Text="Descending" Value="false" />
     <asp:ListItem Text="Ascending" Value="true" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    country and state:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
     ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
     runat="server" />
    <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" OnSelectedIndexChanged="inputState_SelectedIndexChanged"
     AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" />
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
    type:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputType" DataTextField="Name"
     DataValueField="Name" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
     OnClick="search_Click" EnableViewState="false" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td style="font-size: smaller;">
    your current timezone is
    <% Response.Write(base.SessionManager.TimeZone); %>
    <a href="AccountPreferencesManage.aspx" target="_blank">&#187; change</a>
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
    <itemtemplate>
     <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div>
      <a class="sncore_event_name" href="AccountEventView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
      <span style="font-size: smaller;">
       <a href="AccountEventView.aspx?id=<%# Eval("Id") %>"> 
        &#187; event details
       </a>
      </span>
     </div>
     <div class="sncore_description">
      at 
      <a href='PlaceView.aspx?id=<%# Eval("PlaceId") %>'><%# base.Render(Eval("PlaceName")) %></a>      
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("Schedule")) %>      
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("PlaceCity")) %>
      <%# base.Render(Eval("PlaceState")) %>
      <%# base.Render(Eval("PlaceCountry")) %>
     </div>
     <div class="sncore_summary">
      <%# base.GetSummary((string)Eval("Description"))%>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
