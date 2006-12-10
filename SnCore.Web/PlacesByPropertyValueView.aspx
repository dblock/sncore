<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacesByPropertyValueView.aspx.cs"
 Inherits="PlacesByPropertyValueView" Title="Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacePropertyValuesView" Src="PlacePropertyValuesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       <asp:Label id="titlePlacesByProperty" runat="server" />
      </div>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkAll" runat="server" NavigateUrl="PlacesView.aspx" Text="&#187; All Places" />
       <a href="PlaceEdit.aspx">&#187; Suggest a Place</a>
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
      </div>
     </td>
     <td align="right" valign="middle">
     <!--
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="PlacesByPropertyValueViewRss.aspx" />
       -->
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel> 
 <table class="sncore_table">
  <tr>
   <td>
    <SnCore:PlacePropertyValuesView id="ppvs" runat="server" /> 
   </td>
  </tr>
 </table> 
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true" 
    CssClass="sncore_table" ShowHeader="false" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal" 
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
