<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="TagWordAccountsView.aspx.cs" Inherits="TagWordAccountsView" Title="Users" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkPeople" Text="People" runat="server" />
 </div>
 <div class="sncore_h2">
  Tag
 </div>
 <asp:Label ID="tagSubtitle" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10" AllowPaging="true"
  AutoGenerateColumns="false" CssClass="sncore_table" ShowHeader="false">
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
</asp:Content>
