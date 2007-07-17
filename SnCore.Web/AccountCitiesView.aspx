<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCitiesView.aspx.cs"
 Inherits="AccountCitiesView" Title="Top Cities" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titlePeople" Text="Top Cities" runat="server" ExpandedSize="100">
       <Template>
        <div class="sncore_title_paragraph">
         This page shows all cities sorted by popularity.
        </div>
       </Template>
      </SnCore:Title>      
      <div class="sncore_h2sub">
       <a href="AccountsView.aspx">&#187; All People</a>
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" RenderMode="Inline" UpdateMode="Conditional">
  <ContentTemplate> 
    <SnCoreWebControls:PagedList runat="server" ID="gridManage" BorderWidth="0" AllowCustomPaging="true" 
     RepeatRows="25" RepeatDirection="Vertical" RepeatColumns="3" ShowHeader="false" CssClass="sncore_table">
     <ItemTemplate>
     <div>
      <a href='AccountsView.aspx?city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>&pictures=false'>
       <%# base.Render(Eval("Name")) %>
      </a>
      (<%# base.Render(Eval("Total")) %>)
      <a href='AccountsView.aspx?city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>&pictures=false'>
       <img src="images/account/friends.gif" style="border: none; vertical-align: middle;" alt="People" />
      </a>
      <a href='PlacesView.aspx?city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>&pictures=false'>
       <img src="images/account/places.gif" style="border: none; vertical-align: middle;" alt="Places" />
      </a>
      <a href='AccountEventsView.aspx?city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>&pictures=false'>
       <img src="images/account/events.gif" style="border: none; vertical-align: middle;" alt="Places" />
      </a>
      <a href='AccountFeedsView.aspx?city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>'>
       <img src="images/account/syndication.gif" style="border: none; vertical-align: middle;" alt="Blogs" />
      </a>
      <span class="sncore_link" style='<%# SessionManager.IsAdministrator ? "" : "display: none;" %>'>
       <a href='SystemCityEdit.aspx?id=<%# Eval("Id") %>&city=<%# base.Render(Eval("Name")) %>&state=<%# base.Render(Eval("State")) %>&country=<%# base.Render(Eval("Country")) %>'>
        &#187; edit
       </a>
      </span>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>