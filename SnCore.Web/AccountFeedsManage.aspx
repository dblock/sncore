<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountFeedsManage.aspx.cs" Inherits="AccountFeedsManage" Title="Account | Syndication" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="My Syndicated Content" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Really Simple Syndication (RSS) is a lightweight XML format designed for sharing headlines and other Web content. 
    In particular, it allows you to syndicate content from your blog. This means that when you write
    a new post on your blog it will appear on this site as well, automatically, within a short period of time. 
   </div>
   <div class="sncore_title_paragraph">
    Because we have so many users, syndicating will bring you more readers. You still own all your content
    and can even <a href="AccountLicenseEdit.aspx">add a creative license</a> for it. Use the 
    <a href="AccountFeedWizard.aspx">Wizard</a> to make it happen.
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink ID="HyperLink1" Text="&#187; Syndicate Wizard" CssClass="sncore_createnew"
  NavigateUrl="AccountFeedWizard.aspx" runat="server" />
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
       <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
       <div style="color: Red; font-size: smaller;">
        <%# base.Render(Eval("LastError")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Last Update">
      <itemtemplate>
       <%# base.Adjust(Eval("Updated")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">View</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountFeedEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="Update" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
