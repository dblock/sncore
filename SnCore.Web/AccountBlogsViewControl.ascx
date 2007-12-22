<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogsViewControl.ascx.cs"
 Inherits="AccountBlogsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BlogPreview" Src="AccountBlogViewControl.ascx" %>
<asp:Repeater ID="accountBlogs" runat="server">
 <ItemTemplate>
  <SnCore:BlogPreview ID="blogPreview" runat="server" BlogId='<%# Eval("Id") %>' />
 </ItemTemplate>
</asp:Repeater>
