<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountsControl.ascx.cs"
 Inherits="SearchAccountsControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelAccountsResults" runat="server">
 <div class="sncore_h2">
  People
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="10"
  AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table" ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <%# base.Render(Eval("Name")) %>
     </a>
     <font style="font-size: .8em">
     <br />
     Location: 
     <b>
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
      <%# base.Render(Eval("Country")) %>
     </b>
     <br />
     Last activity: 
     <b>
      <%# base.Adjust(Eval("LastLogin")).ToString() %>
     </b>
     </font>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
