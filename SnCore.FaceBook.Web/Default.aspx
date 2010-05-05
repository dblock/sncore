<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="Default.aspx.cs" Inherits="_Default" Title="SnCore FaceBook Application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <a href="AccountFeedItemImgsView.aspx">&#187; images</a>
 
 Hello, <%= Api.Users.GetInfo().name %> 
</asp:Content>
