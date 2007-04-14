<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountRssWatchsManage.aspx.cs" Inherits="AccountRssWatchsManage" Title="Account | Subscriptions" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="My Subscriptions" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Manage your content subscriptions via RSS.
    Really Simple Syndication (RSS) is a lightweight XML format designed for sharing headlines and other Web content.    
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink ID="linkNew" Text="&#187; New Subscription" CssClass="sncore_createnew"
  NavigateUrl="AccountRssWatchEdit.aspx" runat="server" />
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" OnItemCommand="gridManage_ItemCommand"
    AutoGenerateColumns="false" CssClass="sncore_account_table" PageSize="10" AllowCustomPaging="true" AllowPaging="true">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src='images/<%# string.IsNullOrEmpty((string) Eval("LastError")) ? "Item.gif" : "Question.gif" %>'
        alt="<%# base.Render(Eval("LastError")) %>" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Feed" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
      <itemtemplate>
       <a href="<%# base.Render(Eval("Url")) %>">
        <%# base.Render(Eval("Name")) %>
       </a>
       <div style="color: Red; font-size: smaller;">
        <%# base.Render(Eval("LastError")) %>
       </div>
       <div class="sncore_description">
        Last Sent: <%# base.Adjust(Eval("Sent")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountRssWatchEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountRssWatchView.aspx?id=<%# Eval("Id") %>">Preview</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
