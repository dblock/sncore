<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountEventsManage.aspx.cs"
 Inherits="AccountEventsManage" Title="Account | Events" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleEvents" Text="My Events" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    <a href="AccountEventEdit.aspx">Post an event</a> to the website calendar. You can create one-time and
    recurrent events.
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink Text="&#187; Post New" CssClass="sncore_createnew" NavigateUrl="AccountEventEdit.aspx"
  runat="server" />
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table" 
    PageSize="10" AllowPaging="true" AllowCustomPaging="true">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/events.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Event">
      <itemtemplate>
       <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
        at
       <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
        <%# base.Render(Eval("PlaceName")) %>
       </a>
       <div class="sncore_table_tr_th">
        <%# base.Render(Eval("Schedule"))%>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">View</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountEventEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
