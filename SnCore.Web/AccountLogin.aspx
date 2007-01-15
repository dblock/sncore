<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountLogin.aspx.cs" Inherits="AccountLogin" Title="Login" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountLogin" Src="AccountLoginControl.ascx" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <SnCore:AccountLogin runat="server" ID="loginMain" />
</asp:Content>
