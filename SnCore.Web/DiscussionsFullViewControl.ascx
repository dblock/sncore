<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionsFullViewControl.ascx.cs"
 Inherits="DiscussionsFullViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Repeater ID="listDiscussions" runat="server">
 <ItemTemplate>
  <div>
   <%# Eval("Id") %>
  </div>
 </ItemTemplate>
</asp:Repeater>
