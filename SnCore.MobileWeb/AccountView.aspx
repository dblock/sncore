<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountView.aspx.cs"
 Inherits="AccountView" Title="Account | View" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <h3>
  <asp:Label ID="accountName" runat="server" />
 </h3>
 <div class="sncore_description">
  <div>
   <asp:Label ID="accountId" CssClass="sncore_account_id" runat="server" />
  </div>
  <div>
   <asp:Label ID="accountLastLogin" runat="server" />
  </div>
  <div>
   <asp:Label ID="accountCity" CssClass="sncore_account_locations" runat="server" />
   <asp:Label ID="accountState" CssClass="sncore_account_locations" runat="server" />
  </div>
  <div>
   <asp:Label ID="accountCountry" CssClass="sncore_account_locations" runat="server" />
  </div>
 </div>
</asp:Content>
