<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountPropertyGroupsViewControl.ascx.cs"
 Inherits="AccountPropertyGroupsViewControl" %>
 
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:DataGrid runat="server" ID="groups" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false"
 OnItemCreated="groups_ItemCreated" BorderColor="White">
 <ItemStyle HorizontalAlign="Left" />
 <Columns>
  <asp:TemplateColumn>
   <ItemTemplate>
    <div class="sncore_h2" id="title" runat="server">
     <%# Renderer.Render(Eval("Name")) %>
    </div>
    <asp:DataGrid runat="server" ID="values" BorderWidth="0" ShowHeader="false" AutoGenerateColumns="false"
     CssClass="sncore_account_table" BorderColor="White">
     <ItemStyle HorizontalAlign="Left" CssClass="sncore_table_tr_td" />
     <Columns>
      <asp:TemplateColumn>
       <ItemTemplate>
        <div class="sncore_h3">
         <%# Render(Eval("AccountPropertyName")) %>
        </div>
        <div class="sncore_h2sub">
         <%# RenderEx(Eval("Value")) %>
        </div>
       </ItemTemplate>
      </asp:TemplateColumn>
     </Columns>
    </asp:DataGrid>    
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
