<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FacebookConnect.aspx.cs" Inherits="FacebookConnect" Title="Facebook Connect" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2sub">
  Please wait ...
 </div>
 <div id="fb-root"></div>
 <script src="http://connect.facebook.net/en_US/all.js"></script>
 <script type="text/javascript">
  var facebookAPIKey = "<% Response.Write(SessionManager.GetCachedConfiguration("Facebook.APIKey", "")); %>";
  FB.init({appId: facebookAPIKey, status: true, cookie: true, xfbml: true});
  window.location="<% Response.Write(ReturnUrl); %>";
 </script>
</asp:Content>
