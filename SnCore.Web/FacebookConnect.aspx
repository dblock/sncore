<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FacebookConnect.aspx.cs" Inherits="FacebookConnect" Title="Facebook Connect" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2sub">
  Please wait ...
 </div>
 <div id="fb-root"></div>
 <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
 <script type="text/javascript">
  var facebookAPIKey = "<% Response.Write(SessionManager.GetCachedConfiguration("Facebook.APIKey", "")); %>";
  FB.init(facebookAPIKey);
  FB.ensureInit(function() {
   FB.Connect.get_status().waitUntilReady( function( status ) {
      switch ( status ) {
      case FB.ConnectState.connected:
         window.location="<% Response.Write(ReturnUrl); %>";
         break;
      case FB.ConnectState.appNotAuthorized:
      case FB.ConnectState.userNotLoggedIn:
         FB.Connect.requireSession();
	 alert('There was an error logging in.');
      }
   });
  });
 </script>
</asp:Content>
