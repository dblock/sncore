<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="DiscussionsView.aspx.cs" Inherits="DiscussionsView" Title="Discussions" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <sncorewebcontrols:pagedlist cellpadding="4" runat="server" id="gridManage" 
  allowcustompaging="true" repeatcolumns="1" repeatrows="7" repeatdirection="Horizontal"
  cssclass="sncore_table" showheader="false">
  <PagerStyle PageButtonCount="10" cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" />
  <ItemTemplate>
    <div>
     <a href='DiscussionView.aspx?id=<%# Eval("Id") %>'>
      <%# Renderer.Render(Eval("Name")) %>
     </a>
    </div>
    <div class="sncore_description">
    <!--
     <a href="DiscussionPost.aspx?did=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(Request.Url.PathAndQuery) %>">
      &#187; post new
     </a>
     -->
     <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Modified")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
      &#187; last post <%# SessionManager.ToAdjustedString((DateTime) Eval("Modified")) %>
     </span>
    </div>
  </ItemTemplate>
 </sncorewebcontrols:pagedlist>
</asp:Content>
