<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemStatsHits.aspx.cs"
 Inherits="SystemStatsHits" Title="System Statistics - Hits" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <asp:UpdatePanel runat="server" ID="panelStats" UpdateMode="Always">
  <ContentTemplate>
   <div class="sncore_h2">
    <asp:Label ID="labelChartType" runat="server" Text="Daily" />
   </div>
   <div class="sncore_h2sub">
    <asp:LinkButton OnClick="linkHourly_Click" ID="linkHourly" runat="server" Text="&#187; Hits Hourly" />
    <asp:LinkButton OnClick="linkDaily_Click" ID="linkDaily" runat="server" Text="&#187; Daily" />
    <asp:LinkButton OnClick="linkWeekly_Click" ID="linkWeekly" runat="server" Text="&#187; Weekly" />
    <asp:LinkButton OnClick="linkMonthly_Click" ID="linkMonthly" runat="server" Text="&#187; Monthly" />
    <asp:LinkButton OnClick="linkYearly_Click" ID="linkYearly" runat="server" Text="&#187; Yearly" />
    <asp:LinkButton OnClick="linkDailyNew_Click" ID="linkDailyNew" runat="server" Text="&#187; New Daily" />
    <asp:LinkButton OnClick="linkDailyReturning_Click" ID="linkDailyReturning" runat="server" Text="&#187; Returning Daily" />
    <br />
    <asp:LinkButton OnClick="linkAccountDaily_Click" ID="linkAccountDaily" runat="server" Text="&#187; New Accounts Daily" />
    <asp:LinkButton OnClick="linkAccountWeekly_Click" ID="linkAccountWeekly" runat="server" Text="&#187; Weekly" />
    <asp:LinkButton OnClick="linkAccountMonthly_Click" ID="linkAccountMonthly" runat="server" Text="&#187; Monthly" />
    <asp:LinkButton OnClick="linkAccountYearly_Click" ID="linkAccountYearly" runat="server" Text="&#187; Yearly" />
    <br />
    <asp:HyperLink ID="linkRefererHosts" runat="server" Text="&#187; Referrer Hosts" NavigateUrl="SystemRefererHosts.aspx" />
    <asp:HyperLink ID="linkRefererQueries" runat="server" Text="&#187; Referrer Queries" NavigateUrl="SystemRefererQueries.aspx" />
    <asp:HyperLink ID="linkCache" runat="server" Text="&#187; Cache" NavigateUrl="SystemStatsCache.aspx" />
   </div>
   <table class="sncore_account_table">
    <tr>
     <td>
      <img runat="server" id="imageStats" src="SystemStatsChart.aspx?type=Daily" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_description">
    note: all counter times are UTC
   </td>
  </tr>
 </table>
</asp:Content>
