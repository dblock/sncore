<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacesView.aspx.cs"
 Inherits="PlacesView" Title="Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <atlas:UpdatePanel ID="panelLinks" Mode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       Places
      </div>
      <div class="sncore_h2sub">
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All Places" />
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; All Local Places" />
       <a href="PlaceEdit.aspx">&#187; Suggest a Place</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
      </div>
     </td>
     <td align="right" valign="middle">
      <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="PlacesRss.aspx" />
      <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
       href="PlacesRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </atlas:UpdatePanel>
 <atlas:UpdatePanel runat="server" ID="panelSearch" Mode="Conditional">
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
       <atlas:UpdatePanel runat="server" ID="panelCountryState" Mode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
          ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
          runat="server" />
         <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" OnSelectedIndexChanged="inputState_SelectedIndexChanged"
          AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" /></td>
        </ContentTemplate>
       </atlas:UpdatePanel>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:
      </td>
      <td class="sncore_form_value">
       <atlas:UpdatePanel runat="server" ID="panelCity" Mode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
          DataValueField="Name" runat="server" />
        </ContentTemplate>
       </atlas:UpdatePanel>
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
  </ContentTemplate>
 </atlas:UpdatePanel>
 <atlas:UpdatePanel runat="server" ID="panelGrid" Mode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
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
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("Country")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </atlas:UpdatePanel>   
</asp:Content>
