<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountView.aspx.cs"
 Inherits="AccountView" Title="Account | View" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <h3>
  <asp:Label ID="accountName" runat="server" />
 </h3>
 <div>  
  <a id="accountLinkPictures" href="AccountPicturesView.aspx" runat="server">
   <asp:Image ID="accountImage" runat="server" BorderColor="0" />
  </a>
 </div>
 <div class="sncore_description">
  <div>
   <asp:Label ID="accountLastLogin" runat="server" />
  </div>
  <div>
   <asp:Label ID="accountCity" runat="server" />
   <asp:Label ID="accountState" runat="server" />
  </div>
  <div>
   <asp:Label ID="accountCountry" runat="server" />
  </div>
 </div>
</asp:Content>
