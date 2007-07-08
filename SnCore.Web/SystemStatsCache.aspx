<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemStatsCache.aspx.cs"
 Inherits="SystemStatsCache" Title="System Statistics - Cache" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <asp:UpdatePanel runat="server" ID="panelCache" UpdateMode="Always">
  <ContentTemplate>
   <div class="sncore_h2">
    Session Cache
   </div>
   <div class="sncore_h2sub">
    <asp:Label id="labelCacheDescription" runat="server" />
    <asp:LinkButton id="linkFlush" runat="server" OnClick="linkFlush_Click" Text="&#187; flush" />
   </div>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridCache" PageSize="10"
    AllowPaging="true" AutoGenerateColumns="false" AllowCustomPaging="false" CssClass="sncore_account_table">
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src='images/item.gif'>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Name">
      <itemtemplate>
       <%# base.Render(Eval("Name")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Type">
      <itemtemplate>
       <%# base.Render(Eval("Type")) %>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
   <div class="sncore_h2">
    Rolled-Up Cache
   </div>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridRolledUpCache" PageSize="10"
    AllowPaging="true" AutoGenerateColumns="false" AllowCustomPaging="false" CssClass="sncore_account_table">
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src='images/item.gif'>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Type">
      <itemtemplate>
       <%# base.Render(Eval("Type")) %>
       <div class="sncore_description">
        <%# base.Render(Eval("Name")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Size">
      <itemtemplate>
       <%# base.Render(Eval("Size")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Count">
      <itemtemplate>
       <%# base.Render(Eval("Count")) %>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
