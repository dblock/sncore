<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemStatsHits.aspx.cs"
 Inherits="SystemStatsHits" Title="System Statistics - Hits" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <sncore:accountmenu runat="server" id="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     <asp:Label ID="labelChartType" runat="server" Text="Daily" />
    </div>
    <div class="sncore_h2sub">
     <asp:LinkButton OnClick="linkHourly_Click" ID="linkHourly" runat="server" Text="&#187; Hourly" />
     <asp:LinkButton OnClick="linkDaily_Click" ID="linkDaily" runat="server" Text="&#187; Daily" />
     <asp:LinkButton OnClick="linkWeekly_Click" ID="linkWeekly" runat="server" Text="&#187; Weekly" />
     <asp:LinkButton OnClick="linkMonthly_Click" ID="linkMonthly" runat="server" Text="&#187; Monthly" />
     <asp:LinkButton OnClick="linkYearly_Click" ID="linkYearly" runat="server" Text="&#187; Yearly" />
     <asp:LinkButton OnClick="linkDailyUnique_Click" ID="linkDailyUnique" runat="server" Text="&#187; Unique" />
     <asp:HyperLink ID="linkRefererHosts" runat="server" Text="&#187; Referer Hosts" NavigateUrl="SystemRefererHosts.aspx" />
     <asp:HyperLink ID="linkRefererQueries" runat="server" Text="&#187; Referer Queries" NavigateUrl="SystemRefererQueries.aspx" />
    </div>
    <table class="sncore_account_table">
     <tr>
      <td>
       <img runat="server" id="imageStats" src="SystemStatsChart.aspx?type=Daily" />
      </td>
     </tr>
    </table>
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_description">
       note: all counter times are UTC
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
