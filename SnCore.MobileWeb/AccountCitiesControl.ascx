<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountCitiesControl.ascx.cs"
 Inherits="AccountCitiesControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<div class="sncore_table_cities">
 <asp:DataList RepeatLayout="Flow" ID="listCities" runat="server" RepeatColumns="8"
  RepeatDirection="Horizontal">
  <ItemTemplate>
   <asp:LinkButton ID="linkCity" runat="server" Text='<%# string.Format("&#187; {0}", Renderer.Render(Eval("Name"))) %>'
    OnCommand="link_Command" CommandName="Select" CommandArgument='<%# string.Format("country={0}&state={1}&city={2}", Renderer.UrlEncode(Eval("Country")), Renderer.UrlEncode(Eval("State")), Renderer.UrlEncode(Eval("Name"))) %>' />
  </ItemTemplate>
 </asp:DataList>
 <!--<a href="AccountCitiesView.aspx">&#187; more ...</a>-->
</div>
