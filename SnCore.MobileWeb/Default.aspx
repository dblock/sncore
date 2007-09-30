<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="Welcome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <ul>
  <li><a href="AccountsView.aspx">People</a></li>
  <li><a href="PlacesView.aspx">Places</a></li>
  <li><asp:LinkButton ID="linkLoginLogout" runat="server" OnClick="linkLoginLogout_Click" /></li>
 </ul>
</asp:Content>
