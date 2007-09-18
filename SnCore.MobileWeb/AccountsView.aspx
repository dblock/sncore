<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountsView.aspx.cs"
 Inherits="AccountsView" Title="People" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <sncorewebcontrols:pagedlist cellpadding="4" runat="server" id="gridManage" 
  allowcustompaging="true" repeatcolumns="1" repeatrows="10" repeatdirection="Horizontal"
  cssclass="sncore_table" showheader="false">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemTemplate>
   <div>
    <a href="AccountView.aspx?id=<%# Eval("Id") %>">
     <%# Renderer.Render(Eval("Name")) %>
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("City")) %>
    <%# Renderer.Render(Eval("State"))%>
    <%# Renderer.Render(Eval("Country"))%>
   </div>
  </ItemTemplate>
 </sncorewebcontrols:pagedlist>
</asp:Content>
