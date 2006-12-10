<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventsViewList.aspx.cs"
 Inherits="AccountEventsViewList" Title="Events" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Site Map - Events
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" align="left">
    <a id="linkPrev" runat="server">Prev</a>
   </td>
   <td class="sncore_table_tr_td" align="center">
    <a href="SiteMap.aspx">Site Map</a>
   </td>
   <td class="sncore_table_tr_td" align="right">
    <a id="linkNext" runat="server">Next</a>
   </td>
  </tr>
 </table>
 <asp:DataList RepeatColumns="2" CssClass="sncore_table" ID="gridManage" runat="server">
  <ItemStyle CssClass="sncore_table_tr_td" Width="50%" />
  <ItemTemplate>
   <li>
   <a href="AccountEventView.aspx?id=<%# Eval("Id") %>">
    <%# base.Render(Eval("Name")) %>
   </a>
   <div class="sncore_li_description">
    <%# base.Render(Eval("Schedule")) %>
   </div>
   <div class="sncore_li_description">
    <a href='PlaceView.aspx?id=<%# Eval("PlaceId") %>'>
     <%# base.Render(Eval("PlaceName")) %>
    </a>
   </div>
   <div class="sncore_li_description">
    <%# base.Render(Eval("PlaceNeighborhood")) %>
    <%# base.Render(Eval("PlaceCity")) %>
    <%# base.Render(Eval("PlaceState")) %>
    <%# base.Render(Eval("PlaceCountry")) %>
   </div>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
