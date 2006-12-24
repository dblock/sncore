<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemAccountRedirectsManage.aspx.cs"
 Inherits="SystemAccountRedirectsManage" Title="System | Account Redirects" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleRedirects" Text="All Redirects" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Permanent redirects profiles and blogs.
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink Text="&#187; Add New" CssClass="sncore_createnew" NavigateUrl="AccountRedirectEdit.aspx"
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
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/redirect.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Redirect">
      <itemtemplate>
       <%# Renderer.GetLink(Renderer.Render(Eval("SourceUri")), Renderer.Render(Eval("SourceUri")))%>
       <div class="sncore_description">
        <%# Renderer.GetLink(Renderer.Render(Eval("TargetUri")), Renderer.Render(Eval("TargetUri"))) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Account">
      <itemtemplate>
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>"><%# Renderer.Render(Eval("AccountName")) %></a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountRedirectEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
