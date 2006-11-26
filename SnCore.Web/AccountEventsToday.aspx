<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventsToday.aspx.cs"
 Inherits="AccountEventsToday" Title="Events This Week" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountTimeZone" Src="AccountTimeZoneControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="panelLinks" runat="server" UpdateMode="Conditional" EnableViewState="true">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleEvents" Text="Events This Week" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         Use the calendar <b>&#187;</b> arrows to choose a week or a month. You can also choose events in a country,
         city and/or state.
        </div>
        <div class="sncore_title_paragraph">
         <a href="AccountEventEdit.aspx">Click here</a> to submit an event. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <asp:LinkButton ID="linkShowAll" runat="server" Text="&#187; All Events This Week" OnClick="linkShowAll_Click" />
       <a href="AccountEventsView.aspx">&#187; All Events</a>
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; Local Events" />
       <a href="AccountEventEdit.aspx">&#187; Suggest an Event</a>
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
      </div>
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel ID="panelSearch" runat="server" UpdateMode="Conditional" EnableViewState="true">
  <ContentTemplate>
   <table class="sncore_table">
    <tr>
     <td valign="top">
      <table>
       <tr>
        <td class="sncore_form_label">
         country and state:
        </td>
        <td class="sncore_form_value">
         <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
          <ContentTemplate>
           <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
            ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
            runat="server" />
           <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" OnSelectedIndexChanged="inputState_SelectedIndexChanged"
            AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" /></td>
           </ContentTemplate>
          </asp:UpdatePanel>
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label" style="height: 25px">
         city:
        </td>
        <td class="sncore_form_value" style="height: 25px">
         <asp:UpdatePanel runat="server" ID="panelCity" UpdateMode="Conditional">
          <ContentTemplate>
           <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
            DataValueField="Name" runat="server" AutoPostBack="true" OnSelectedIndexChanged="inputCity_SelectedIndexChanged" />
          </ContentTemplate>
         </asp:UpdatePanel>
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label" style="height: 25px">
         neighborhood:
        </td>
        <td class="sncore_form_value" style="height: 25px">
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
     </td>
     <td>
      <asp:Calendar ID="calendarEvents" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66"
       BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
       ForeColor="#663399" Height="200px" ShowGridLines="True" Width="220px" NextMonthText="&#187;" 
       OnSelectionChanged="calendarEvents_SelectionChanged" PrevMonthText="&#171;" SelectionMode="DayWeekMonth" 
       SelectMonthText="&#187;&#187;" SelectWeekText="&#187;" FirstDayOfWeek="Monday">
       <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
       <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
       <SelectorStyle BackColor="#FFCC66" />
       <OtherMonthDayStyle ForeColor="#CC9966" />
       <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
       <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
       <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
      </asp:Calendar>
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
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
       <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
        <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
      <itemtemplate>
       <div>
        <a class="sncore_event_name" href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
        <span style="font-size: smaller;">
         <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>"> 
          &#187; event details
         </a>
        </span>
       </div>
       <div class="sncore_description">
        at 
        <a href='PlaceView.aspx?id=<%# Eval("PlaceId") %>'><%# base.Render(Eval("PlaceName")) %></a>      
       </div>
       <div class="sncore_description">
        Starts: <%# base.Adjust(Eval("StartDateTime")).ToString("f") %>
       </div>
       <div class="sncore_description">
        Ends: <%# base.Adjust(Eval("EndDateTime")).ToString("f") %>
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
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
