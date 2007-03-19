<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountEventWizard.aspx.cs"
 Inherits="AccountEventWizard" Title="Event Wizard" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="Event Import Wizard" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    The event wizard allows you to import events from sites that support iCalendar, a standard for calendar data exchange. 
    The standard is sometimes referred to as "iCal". Popular sites implementing iCal include
    <a href="http://www.eventful.com">Eventful</a>.
   </div>
   <div>
    The page that you enter below must have a meta link of <b>text/calendar</b> type, eg. 
    &lt;link rel="alternate" type="text/calendar" title="iCalendar" 
     href="/ical/events/E0-001-002992261-4/E0-001-002992261-4.ics" /&gt;
   </div>
  </Template>
 </SnCore:Title>
 <asp:UpdatePanel runat="server" ID="panelMain">
  <ContentTemplate>
   <div class="sncore_cancel">
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" runat="server" NavigateUrl="AccountEventsToday.aspx" />
    <asp:HyperLink ID="linkPostNew" Text="&#187; Don't Import: Post New" NavigateUrl="AccountEventEdit.aspx"
     runat="server" />
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      event address:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox ID="inputLinkUrl" runat="server" CssClass="sncore_form_textbox" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
      <div class="sncore_link_small">
       eg. http://eventful.com/events/E0-001-00000001-4
      </div>
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td>
      <sncorewebcontrols:button id="linkDiscover" cssclass="sncore_form_button" onclick="discover_Click"
       runat="server" text="Discover" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridEvents" AutoGenerateColumns="false" 
    CssClass="sncore_account_table">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Website" Visible="false" />
     <asp:TemplateColumn HeaderText="Event" ItemStyle-HorizontalAlign="Left" 
      HeaderStyle-HorizontalAlign="Left">
      <itemtemplate>
       <div class="sncore_h2">
        <%# base.Render(Eval("Name")) %>
       </div>
       <div class="sncore_h2sub">
        <b><%# base.Render(Eval("StartDateTime")) %></b>
        <a href='<%# string.Format("AccountEventEdit.aspx?ical={0}&ReturnUrl={1}", Renderer.UrlEncode(Eval("Website")), Renderer.UrlEncode(Request.Url.PathAndQuery)) %>'>&#187; Choose</a>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Description")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
