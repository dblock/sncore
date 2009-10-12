<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountEventsView.aspx.cs" Inherits="AccountEventsView" Title="All Events" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountTimeZone" Src="AccountTimeZoneControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleEvents" Text="Events" runat="server" ExpandedSize="100">  
       <Template>
        <div class="sncore_title_paragraph">
         These are events in your city, submitted by other users. Participate! Attend an event, upload 
         a picture or write a review. Then, <a href="AccountEventWizard.aspx">Add an Event</a>.
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountEventWizard.aspx">&#187; Add an Event</a>
       <a href="AccountEventsToday.aspx">&#187; Events This Week</a>
       <asp:LinkButton ID="linkShowAll" runat="server" Text="&#187; All Events" OnClick="linkShowAll_Click" />
       <asp:LinkButton ID="linkSearch" runat="server" Text="&#187; Search" OnClick="linkSearch_Click" />
       <asp:Hyperlink id="linkPermalink" NavigateUrl="PlacesView.aspx" runat="server" Text="&#187; Permalink" />
      </div>
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountEventsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel ID="panelSearch" runat="server" UpdateMode="Conditional" EnableViewState="true">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel ID="panelSearchInternal" Visible="False" runat="server" EnableViewState="True">
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
       country and state:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown_small"
          ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
          runat="server" />
         <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState"
          AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCity" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
          DataValueField="Name" runat="server" AutoPostBack="true" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       neighborhood:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelNeighborhood" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputNeighborhood" DataTextField="Name"
          DataValueField="Name" runat="server" />
        </ContentTemplate>
       </asp:UpdatePanel>
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
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:Cities id="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
 <SnCore:AccountTimeZone runat="server" ID="timezone" />
 <asp:UpdatePanel runat="server" ID="panelGrid" RenderMode="Inline" UpdateMode="Conditional">
  <ContentTemplate> 
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="5"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
    ShowHeader="false" OnDataBinding="gridManage_DataBinding">
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
        <%# base.Render(Eval("PlaceNeighborhood")) %>
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
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
