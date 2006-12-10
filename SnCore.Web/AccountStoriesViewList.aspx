<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoriesViewList.aspx.cs"
 Inherits="AccountStoriesViewList" Title="Stories" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Site Map - Stories
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
   <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
    <%# base.Render(Eval("Name")) %>
   </a>
   <div class="sncore_li_description">
    posted by 
    <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
     <%# base.Render(Eval("AccountName")) %> 
    </a>
    on 
    <%# base.Adjust(Eval("Created")).ToString("d") %>
   </div>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
