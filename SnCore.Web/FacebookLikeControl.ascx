<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FacebookLikeControl.ascx.cs" Inherits="FacebookLikeControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<iframe src="http://www.facebook.com/widgets/like.php?href=<% Response.Write(Renderer.UrlEncode(Request.Url)); %>" scrolling="no" frameborder="0" style="border:none; width:350px; height:25px"></iframe>