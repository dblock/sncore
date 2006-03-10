<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookmarksViewControl.ascx.cs" Inherits="BookmarksViewControl" %>
<%@ Import Namespace="SnCore.Services" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:DataList runat="server" ID="bookmarksView" CssClass="sncore_bookmarks_table">
 <ItemStyle HorizontalAlign="Center" CssClass="sncore_bookmarks_table_tr_td" />
 <ItemTemplate>
  <a target="_blank" href="<%# GetUrl(Eval("Url").ToString()) %>">
   <img border="0" src="SystemBookmark.aspx?id=<%# Eval("Id") %>&ShowThumbnail=<%# base.ShowThumbnail %>"
    alt="<%# base.Render(Eval("Name")) %>
<%# base.Render(Eval("Description")) %>" />
  </a>
 </ItemTemplate>
</asp:DataList>
