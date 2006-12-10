<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacesView.aspx.cs"
 Inherits="PlacesView" Title="Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceNeighborhoodsView" Src="PlaceNeighborhoodsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" class="sncore_table_noborder">
    <tr>
     <td>
      <SnCore:Title ID="titlePlaces" Text="Places" runat="server" ExpandedSize="100">  
       <Template>
        <div class="sncore_title_paragraph">
         These are places in your city, contributed by other users. Participate! Click on a place, upload 
         a picture, leave a Mad Lib or write a review. Then, <a href="PlaceEdit.aspx">Suggest a New Place</a>.
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All Places" />
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; All Local Places" />
       <a href="PlaceEdit.aspx">&#187; Suggest a Place</a>
       <a href="AccountPlaceQueueManage.aspx">&#187; My Queue</a>
       <a href="PlaceFriendsQueueView.aspx">&#187; My Friends Queue</a>
       <a href="PlacesFavoritesView.aspx">&#187; Favorites</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
      </div>
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="PlacesRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelSearch" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server" EnableViewState="true">
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
       <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectSortOrder" runat="server">
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
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
        Text="show places with pictures only" Checked="true" />
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
   <asp:UpdatePanel runat="server" ID="panelNeighborhoods" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
     <table class="sncore_table_noborder">
      <tr>
       <td class="sncore_link">
        <SnCore:PlaceNeighborhoodsView ID="neighborhoods" runat="server" />
       </td>
      </tr>
     </table>
    </ContentTemplate>
   </asp:UpdatePanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" CssClass="sncore_table"
    ShowHeader="false" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal" 
    OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <ItemTemplate>
     <div class="sncore_link">
      <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
       <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
      </a>
     </div>
     <div class="sncore_link">
      <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
     </div>
     <div class="sncore_link">
      <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
       &#187; read and review
      </a>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("Neighborhood")) %>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("Country")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
