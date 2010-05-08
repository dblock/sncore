<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FacebookShareControl.ascx.cs" Inherits="FacebookShareControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share" type="text/javascript"></script>
<a name="fb_share" type="box_count" share_url="<% Response.Write(Renderer.UrlEncode(Request.Url)); %>">Share!</a>
