<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlaceView.aspx.cs"
 Inherits="PlaceView" Title="Place | View" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div>  
  <a id="placeLinkPictures" href="PlacePicturesView.aspx" runat="server">
   <asp:Image ID="placeImage" runat="server" BorderColor="0" />
  </a>
 </div>
 <div class="sncore_description">
  <div>
   <asp:Label ID="placeType" runat="server" />
  </div>
  <div>
   <asp:Label ID="placeStreet" runat="server" />
  </div>
  <div>
   <asp:Label ID="placeCrossStreet" runat="server" />
  </div>
  <div>
   <asp:Label ID="placeNeighborhood" runat="server" />
  </div>
  <div>
   <asp:Label ID="placeZip" runat="server" />
   <asp:Label ID="placeCity" runat="server" />
   <asp:Label ID="placeState" runat="server" />
  </div>
  <div>
   <asp:Label ID="placeCountry" runat="server" />
  </div>
 </div>
 <div>
  <ul class="links">
   <li><asp:HyperLink ID="linkPictures" runat="server" Text="&#187; Pictures" /></li>
   <li><asp:HyperLink ID="linkReviews" runat="server" Text="&#187; Reviews" /></li>
  </ul>
 </div>
</asp:Content>
