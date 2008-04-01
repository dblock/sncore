<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemConfigurationsManage.aspx.cs"
 Inherits="SystemConfigurationsManage" Title="System | Configurations" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Configuration Settings
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemConfigurationEdit.aspx"
  runat="server" />
 <a href="SystemConfigurationEmailEdit.aspx">&#187; E-Mail Subsystem</a>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src='images/<%# (int) Eval("Id") != 0 ? "Item" : "Question" %>.gif' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Name">
      <itemtemplate>
       <a href='SystemConfigurationEdit.aspx?<%# (int) Eval("Id") == 0 ? string.Format("name={0}", Renderer.UrlEncode(Eval("Name"))) : string.Format("id={0}", Eval("Id")) %>'>
        <%# Renderer.Render(Eval("Name")) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Value" ItemStyle-HorizontalAlign="Left">
      <itemtemplate>
       <%# GetValue((bool) Eval("Password"), (string) Eval("Value")) %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='SystemConfigurationEdit.aspx?<%# (int) Eval("Id") == 0 ? string.Format("name={0}", Renderer.UrlEncode(Eval("Name"))) : string.Format("id={0}", Eval("Id")) %>'>
        Edit
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <asp:LinkButton id="delete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' Text="Delete"
        Visible='<%# (int) Eval("Id") != 0 %>' OnClientClick="return confirm('Are you sure you want to delete this setting?')" />
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
