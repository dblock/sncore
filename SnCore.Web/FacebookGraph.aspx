<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="FacebookGraph.aspx.cs"
 Inherits="FacebookGraph" Title="Facebook Graph" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="xacc" Namespace="Xacc" Assembly="xacc.propertygrid" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Facebook Graph - User
 </div>
 <div class="sncore_h2sub">
  <xacc:propertygrid id="facebookUser" runat="server" ShowHelp="false" Width="750" />
 </div>
 <div class="sncore_h2">
  Hometown
 </div>
 <div class="sncore_h2sub">
  <xacc:propertygrid id="facebookUserHometownLocation" runat="server" ShowHelp="false" Width="750" />
 </div>
 <div class="sncore_h2">
  Current Location
 </div>
 <div class="sncore_h2sub">
  <xacc:propertygrid id="facebookUserCurrentLocation" runat="server" ShowHelp="false" Width="750" />
 </div>
 <div class="sncore_h2">
  Status
 </div>
 <div class="sncore_h2sub">
  <xacc:propertygrid id="facebookUserStatus" runat="server" ShowHelp="false" Width="750" />
 </div>
</asp:Content>
