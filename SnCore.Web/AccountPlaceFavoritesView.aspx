<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPlaceFavoritesView.aspx.cs" Inherits="AccountPlaceFavoritesView" Title="Favorite Places" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       <asp:Label id="labelName" runat="server" />
      </div>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkAccount" runat="server" Text="&#187; Back" />
       <a href="PlaceEdit.aspx">&#187; Add New</a>
       <a href="PlacesView.aspx">&#187; All Places</a>
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" 
    RepeatDirection="Horizontal" CssClass="sncore_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
      <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" />
     </a>
     <div>
      <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
       <%# base.Render(Eval("PlaceName")) %>
      </a>
     </div>
     <div>
      <%# base.Render(Eval("PlaceCity")) %>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList> 
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
