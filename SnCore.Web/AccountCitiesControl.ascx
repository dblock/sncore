<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountCitiesControl.ascx.cs"
 Inherits="AccountCitiesControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:UpdatePanel ID="panelAccountCities" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
  <div class="sncore_table_cities">
   <asp:DataList RepeatLayout="Flow" ID="listCities" runat="server" RepeatColumns="8"
    RepeatDirection="Horizontal">
    <ItemTemplate>
     <asp:LinkButton ID="linkCity" runat="server" Text='<%# string.Format("&#187; {0}", Renderer.Render(Eval("Name"))) %>'
      OnCommand="link_Command" CommandName="Select" 
      CommandArgument='<%# string.Format("city={0}&state={1}&country={2}", Renderer.Render(Eval("Name")), Renderer.Render(Eval("State")), Renderer.Render(Eval("Country"))) %>' />
    </ItemTemplate>
   </asp:DataList>
   <a href="AccountCitiesView.aspx">&#187; more ...</a>
  </div>
 </ContentTemplate>
</asp:UpdatePanel>
