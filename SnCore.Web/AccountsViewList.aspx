<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountsViewList.aspx.cs"
 Inherits="AccountsViewList" Title="People" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Site Map - People
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
   <a href="AccountView.aspx?id=<%# Eval("Id") %>">
    <%# base.Render(Eval("Name")) %>
   </a>
   <div class="sncore_li_description">
    last activity:
    <%# base.Adjust(Eval("LastLogin")).ToString("d") %>
   </div>
   <div class="sncore_li_description">
    <%# base.Render(Eval("City")) %>
    <%# base.Render(Eval("State")) %>
    <%# base.Render(Eval("Country")) %>
   </div>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
