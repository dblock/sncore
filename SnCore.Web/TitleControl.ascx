<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TitleControl.ascx.cs"
 Inherits="TitleControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
 TargetControlID="panelTitle" Collapsed="true" CollapsedSize="42" ExpandedSize="150"
 ExpandControlID="imageHelp" CollapseControlID="imageHelp" SuppressPostBack="true">
</ajaxtoolkit:CollapsiblePanelExtender>
<asp:Panel ID="panelTitle" runat="server">
 <div class="sncore_h2">
  <asp:Label ID="labelText" runat="server" Text="Untitled" />
  <asp:ImageButton CausesValidation="false" ID="imageHelp" runat="server" ImageUrl="images/site/help.gif"
   ImageAlign="AbsMiddle" AlternateText="Click here for help ..." />
 </div>
 <div class="sncore_h2sub" id="divHelp" runat="server">
 </div>
</asp:Panel>
