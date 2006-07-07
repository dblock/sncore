
<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlaceView.aspx.cs"
 Inherits="PlaceView" Title="Place | View" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceAccountsView" Src="PlaceAccountsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlaceFavoriteAccountsView" Src="PlaceFavoriteAccountsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkCountry" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkState" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkCity" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkType" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkPlace" runat="server" />
 </div>
 <asp:Panel CssClass="panel" ID="pnlPlace" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 200px;">
     <asp:Panel ID="panelNoPicture" CssClass="sncore_nopicture_table" runat="server" Visible="false">
      <img border="0" src="images/PlaceThumbnail.gif" />
     </asp:Panel>
     <asp:DataList runat="server" ID="picturesView">
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <ItemTemplate>
       <a href="PlacePictureView.aspx?id=<%# Eval("Id") %>">
        <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("Id") %>" alt="<%# base.Render(Eval("Name")) %>" />
        <div style="font-size: smaller;">
        <%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
         " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>
        </div>
       </a>
      </ItemTemplate>
     </asp:DataList>
    </td>
    <td valign="top" width="*">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label CssClass="sncore_place_name" ID="placeName" runat="server" />
        <div>
         <asp:Label ID="placeType" CssClass="sncore_account_lastlogin" runat="server" />
        </div>
        <div>
         <asp:Label ID="placeCity" CssClass="sncore_place_location" runat="server" />
         <asp:Label ID="placeState" CssClass="sncore_place_location" runat="server" />
        </div>
        <div>
         <asp:Label ID="placeCountry" CssClass="sncore_place_location" runat="server" />
        </div>
        <div style="font-size: smaller;">
         <asp:Label ID="placePhone2" runat="server" />
         <asp:ImageButton Visible="False" ImageUrl="images/account/inbox.gif" ImageAlign="AbsMiddle" runat="server" ID="imageEmail" />
        </div>
        <div style="font-size: smaller;">
         <asp:HyperLink ID="placeWebsite" runat="server" />
        </div>
       </td>
       <td class="sncore_table_tr_td" valign="top" align="right">
        <asp:Label ID="placeId" CssClass="sncore_place_id" runat="server" />
       </td>
      </tr>
     </table>
     <asp:Panel ID="panelDetails" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td colspan="2" align="right">
         <div>
          <asp:LinkButton ID="linkAddToFavorites" OnClick="linkAddToFavorites_Click" runat="server"
           Text="&#187; Add to Favorites" />
         </div>
         <div>
          <a href='PlacePicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>&#187; Upload
           a Picture</a>
         </div>
         <div>
          <a href='AccountPlaceRequestEdit.aspx?pid=<% Response.Write(base.RequestId); %>'>&#187; 
           Claim Ownership</a>
         </div>
         <asp:Panel ID="panelAdmin" runat="server">
          <div>
           <asp:HyperLink runat="server" ID="linkAdminEdit" Text="&#187; Edit Content" />
          </div>
          <div>
           <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="&#187; Feature" />
          </div>
          <div>
           <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="&#187; Delete Features" />
          </div>
         </asp:Panel>
        </td>
       </tr>
      </table>
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
         socially bookmark this place:
        </td>
        <td class="sncore_table_tr_td">
         <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
        </td>
       </tr>
      </table>
     </asp:Panel>
     <asp:Panel ID="panelViews" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_table_tr_td" style="font-size: smaller;">
         <a id="linkDetails" runat="server" href="#" onclick="showDetails();">&#187; details</a>
         <a id="linkMap" runat="server" href="#" onclick="showMap();">&#187; map</a> 
         <a id="linkDirections" runat="server" target="_blank">&#187; directions</a>
        </td>
        <td align="right" style="font-size: smaller;">
         <% Response.Write(SuggestedBy); %>
        </td>
       </tr>
      </table>
     </asp:Panel>
     <div id="divDetails">
      <asp:Panel ID="panelDescription" runat="server">
       <div class="sncore_h2">
        About
       </div>
       <table class="sncore_inner_table" width="95%">
        <tr>
         <td class="sncore_table_tr_td">
          <asp:Label runat="server" ID="labelDescription" />
         </td>
        </tr>
       </table>
      </asp:Panel>
      <asp:Panel ID="panelAddress" runat="server">
       <table class="sncore_inner_table" width="95%">
        <tr>
         <td class="sncore_form_label">
          address:
         </td>
         <td class="sncore_form_value">
          <asp:Label ID="placeAddress" runat="server" />
         </td>
        </tr>
        <tr>
         <td class="sncore_form_label">
          zip:
         </td>
         <td class="sncore_form_value">
          <asp:Label ID="placeZip" runat="server" />
         </td>
        </tr>
        <tr>
         <td class="sncore_form_label">
          cross-street:
         </td>
         <td class="sncore_form_value">
          <asp:Label ID="placeCrossStreet" runat="server" />
         </td>
        </tr>
        <tr>
         <td class="sncore_form_label">
          phone:
         </td>
         <td class="sncore_form_value">
          <asp:Label ID="placePhone" runat="server" />
         </td>
        </tr>
        <tr>
         <td class="sncore_form_label">
          fax:
         </td>
         <td class="sncore_form_value">
          <asp:Label ID="placeFax" runat="server" />
         </td>
        </tr>
       </table>
      </asp:Panel>
      <asp:Panel ID="panelOwners" runat="server">
       <SnCore:PlaceAccountsView ID="placeAccounts" runat="server" />
      </asp:Panel>
      <asp:Panel ID="panelFriends" runat="server">
       <SnCore:PlaceFavoriteAccountsView ID="placeFriends" runat="server" />
      </asp:Panel>
     </div>
     <div id="divMap" style="display: none;">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_table_tr_td">
         <div id="mapContainer" style="width: 500px; height: 400px;">
         </div>
        </td>
       </tr>
      </table>

      <script type="text/javascript" src="http://api.maps.yahoo.com/ajaxymap?v=2.0&appid=SnCore"></script>

     </div>
     <a name="Comments" />
     <SnCore:DiscussionFullView runat="server" ID="discussionPlaces" Text="Reviews" PostNewText="&#187; Post a Review" />
     <asp:Panel ID="panelSubmit" Visible="False" runat="server">
      <div class="sncore_notice_info">
       <table cellspacing="0" cellpadding="4" class="sncore_inner_table" width="95%">
        <tr>
         <td>
          This place is not in the system.<br />
          Please take a moment to submit it by
          <asp:HyperLink ID="linkEdit" runat="server" Text="clicking here" />.
         </td>
        </tr>
       </table>
      </div>
     </asp:Panel>
    </td>
   </tr>
  </table>
 </asp:Panel>

 <script type="text/javascript">
  //<![CDATA[
   var map = null;
  
   function createYahooMarker() 
   { 
     var myImage = new YImage();
     myImage.src = 'http://us.i1.yimg.com/us.yimg.com/i/us/map/gr/mt_ic_c.gif';
     myImage.size = new YSize(20, 20);
     myImage.offsetSmartWindow = new YCoordPoint(0, 0);
     var marker = new YMarker("<% Response.Write(base.Render(base.Address)); %>", myImage, "2"); 
     marker.addLabel("<img src='images/account/star.gif'>");
     YEvent.Capture(marker, EventsList.MouseClick, function() { marker.openSmartWindow("<% Response.Write(MarkerText); %>") }); 
     return marker; 
   } 
  
   function showMap()
   {
     if (map == null)
     {     
       map = new YMap(document.getElementById('mapContainer'));
       map.drawZoomAndCenter("<% Response.Write(base.Render(base.Address)); %>", 3);
       
       map.addPanControl();
       map.addZoomLong();

       var marker = createYahooMarker(); 
       map.addOverlay(marker);
     }
     
     document.getElementById("divDetails").style.display = "none";
     document.getElementById("divMap").style.display = "";
   }
   
   function showDetails()
   {
     document.getElementById("divDetails").style.display = "";
     document.getElementById("divMap").style.display = "none";
   }
  //]]>
 </script>

</asp:Content>
