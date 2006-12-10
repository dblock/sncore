<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountWebsitesManage.aspx.cs"
 Inherits="AccountWebsitesManage" Title="Account | Websites" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleWebsites" Text="My Websites" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Link to your website. It will appear on your member profile.
   </div>
   <div class="sncore_title_paragraph">
    If you have a blog, <a href="AccountFeedWizard.aspx">syndicate it here</a> instead.
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink Text="&#187; Add New" CssClass="sncore_createnew" NavigateUrl="AccountWebsiteEdit.aspx"
  runat="server" />
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
    AllowPaging="true" AllowCustomPaging="true" PageSize="5">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/websites.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Website">
      <itemtemplate>
       <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Name"))) %>
       <div class="sncore_description">
        <%# Renderer.GetLink(Renderer.Render(Eval("Url")), Renderer.Render(Eval("Url"))) %>
       </div>
       <div class="sncore_description">
        <%# base.Render(Eval("Description")) %>
       </div>        
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountWebsiteEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
