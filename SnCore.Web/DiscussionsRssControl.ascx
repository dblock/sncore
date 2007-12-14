<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionsRssControl.ascx.cs"
 Inherits="DiscussionsRssControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionRss" Src="DiscussionRssControl.ascx" %>
<asp:Repeater ID="listDiscussions" runat="server">
 <ItemTemplate>
  <SnCore:DiscussionRss id="dicussionRss" runat="server" DiscussionId='<%# Eval("Id") %>' />
 </ItemTemplate>
</asp:Repeater>
