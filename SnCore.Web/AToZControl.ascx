<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AToZControl.ascx.cs" Inherits="AToZControl" %>
<asp:UpdatePanel ID="panelAToZ" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
  <div class="sncore_table_atoz" runat="server" id="divatoz" />
 </ContentTemplate>
</asp:UpdatePanel>
