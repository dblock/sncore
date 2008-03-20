<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectCultureControl.ascx.cs"
 Inherits="SelectCultureControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Repeater ID="listCultures" runat="server">
 <ItemTemplate>
  <asp:ImageButton ImageAlign="AbsMiddle" ImageUrl='<%# string.Format("images/culture/{0}.png", Eval("LCID")) %>' 
   ID="selectCulture" runat="server" CommandArgument='<%# Eval("LCID") %>' CommandName="SelectCulture" 
   AlternateText='<%# Eval("NativeName") %>' Width="16" Height="16" OnCommand="listCultures_ItemCommand" />
 </ItemTemplate>
</asp:Repeater>
