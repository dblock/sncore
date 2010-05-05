<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FacebookConnect.aspx.cs" Inherits="FacebookConnect" Title="Facebook Connect" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
 <script type="text/javascript">
  var facebookAPIKey = "<% Response.Write(SessionManager.GetCachedConfiguration("Facebook.APIKey", "")); %>";
  FB.init(facebookAPIKey, "AccountLogin.aspx");
 </script>
</asp:Content>
