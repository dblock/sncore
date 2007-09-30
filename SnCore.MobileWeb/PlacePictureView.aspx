<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacePictureView.aspx.cs"
 Inherits="PlacePictureView" Title="Place | Picture" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <h3>
  <asp:Label ID="labelPlaceName" runat="server" Text="Place" />
 </h3>
 <div>
  <img runat="server" id="inputPicture" border="0" src="PlacePictureThumbnail.aspx?id=0" />
 </div>
 <div class="sncore_description">
  <b>
   <asp:Label ID="inputName" runat="server" />
  </b>
 </div>
 <div class="sncore_description">
  <asp:Label ID="inputDescription" runat="server" />
 </div>
 <div class="sncore_description">
  uploaded
  <asp:Label ID="inputCreated" runat="server" />
 </div>
 <div class="sncore_description">
  <asp:Label ID="inputCounter" runat="server" /> clicks
 </div>
</asp:Content>
