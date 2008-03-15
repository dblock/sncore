<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SystemMenuControl.ascx.cs" Inherits="SystemMenuControl" %>
<table class="sncore_accountmenu_table">
 <tr><td width="30" align="center"><img src="images/account/settings.gif" /></td><td width="*"><a href="SystemPreferencesManage.aspx"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Links, Manage %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/star.gif" /></td><td width="*"><a href="SystemStatsHits.aspx"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Links, HitStats %>" /></a></td></tr>
 <tr><td width="30" align="center"><img src="images/account/prefs.gif" /></td><td width="*"><a href="BugProjectsManage.aspx"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Links, Bugs %>" /></a></td></tr>
</table>
