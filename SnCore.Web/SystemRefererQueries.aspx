<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemRefererQueries.aspx.cs"
 Inherits="SystemRefererQueries" Title="Referer Queries" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Referer Queries
    </div>
    <atlas:UpdatePanel runat="server" Mode="Always" ID="panelReferers">
     <ContentTemplate>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkRefererCounters" runat="server" Text="&#187; Hit Stats" NavigateUrl="SystemStatsHits.aspx" />
       <asp:HyperLink ID="linkRefererHosts" runat="server" Text="&#187; Referer Hosts" NavigateUrl="SystemRefererHosts.aspx" />
       <asp:HyperLink ID="linkRefererQueries" runat="server" Text="&#187; Referer Queries" Enabled="False" />
      </div>
      <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
       AllowPaging="true" AutoGenerateColumns="false" AllowCustomPaging="True" CssClass="sncore_account_table">
       <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
       <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
        PrevPageText="Prev" HorizontalAlign="Center" />
       <Columns>
        <asp:BoundColumn DataField="Id" Visible="false" />
        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
         <itemtemplate>
          <img src='images/item.gif'>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Total">
         <itemtemplate>
          <%# base.Render(Eval("Total")) %>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Keywords">
         <itemtemplate>
          <%# base.Render(Eval("Keywords")) %>
         </itemtemplate>
        </asp:TemplateColumn>
       </Columns>
      </SnCoreWebControls:PagedGrid>
     </ContentTemplate>
    </atlas:UpdatePanel>
   </td>
  </tr>
 </table>
</asp:Content>
