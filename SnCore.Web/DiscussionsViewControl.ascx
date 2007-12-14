<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionsViewControl.ascx.cs"
 Inherits="DiscussionsViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<asp:Repeater ID="listDiscussions" runat="server">
 <ItemTemplate>
  <SnCore:DiscussionView id="dicussionView" runat="server" DiscussionId='<%# Eval("Id") %>'
   OuterWidth='<%# OuterWidth %>' PostNewText='<%# PostNewText %>' CssClass='<%# CssClass %>'
   ReadText='<%# ((int) Eval("DefaultViewRows") == 0) ? "&#187; Read" : string.Empty %>' />
 </ItemTemplate>
</asp:Repeater>
