<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventView.aspx.cs"
 Inherits="AccountEventView" Title="Event | View" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountTimeZone" Src="AccountTimeZoneControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PicturesView" Src="AccountEventPicturesViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel CssClass="panel" ID="pnlAccountEvent" runat="server">
  <table cellspacing="0" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td_images">
     <SnCore:PicturesView id="picturesView" runat="server" />
    </td>
    <td valign="top" width="*">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label CssClass="sncore_event_name" ID="eventName" runat="server" />
        <div class="sncore_description">
         <asp:HyperLink ID="eventType" runat="server" />
        </div>
        <div class="sncore_link">
         <div>
          <asp:HyperLink ID="eventCity" runat="server" />
          <asp:HyperLink ID="eventState" runat="server" />
         </div>
         <div>
          <asp:HyperLink ID="eventCountry" runat="server" />
         </div>
         <div>
          <asp:Label ID="eventPhone" runat="server" />
         </div>
         <div>
          <asp:HyperLink ID="eventWebsite" runat="server" Text="&#187; website" />
          <asp:HyperLink ID="eventEmail" runat="server" Text="&#187; e-mail" />
         </div>
         <div style="font-weight: bold;">
          <asp:Label ID="eventCost" runat="server" />
         </div>
        </div>
       </td>
       <td class="sncore_table_tr_td" valign="top" align="right">
        <asp:Label ID="eventId" CssClass="sncore_event_id" runat="server" />
       </td>
      </tr>
     </table>
     <asp:Panel ID="panelDetails" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td align="right">
         <div>
          <a href='AccountEventPicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>
           &#187; Upload a Picture</a>
         </div>
         <div>
          <asp:LinkButton ID="linkExportVCalendar" runat="server" OnClick="linkExportVCalendar_Click" 
           Text="&#187; Export to Outlook" />
         </div>
         <!-- NOEMAIL-START -->
         <div>
          <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
         </div>
         <asp:Panel ID="panelOwner" runat="server">
          <div>
           <asp:HyperLink runat="server" ID="linkEdit" Text="&#187; Edit Event" />
          </div>
          <div>
           <asp:LinkButton runat="server" ID="linkDelete" Text="&#187; Delete Event" OnClick="linkDelete_Click" 
            OnClientClick="return confirm('Are you sure you want to delete this event?')"/>           
          </div>
         </asp:Panel>
         <asp:Panel ID="panelAdmin" runat="server">
          <div>
           <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="&#187; Feature" />
          </div>
          <div>
           <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures"
            Text="&#187; Delete Features" />
          </div>
         </asp:Panel>
         <!-- NOEMAIL-END -->
        </td>
       </tr>
      </table>
     </asp:Panel>
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
        socially bookmark this event:
       </td>
       <td class="sncore_table_tr_td">
        <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
       </td>
      </tr>
     </table>
     <asp:Panel ID="panelViews" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td style="font-size: smaller; text-align: right;">
         <% Response.Write(AddedBy); %>
        </td>
        <td style="font-size: smaller; text-align: right;">
         <a href="AccountEventEdit.aspx">&#187; post an event</a>
        </td>
       </tr>
      </table>
     </asp:Panel>
     <asp:Panel ID="panelDescription" runat="server">
      <div class="sncore_h2">
       What
      </div>
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_table_tr_td">
         <asp:Label runat="server" ID="labelDescription" />
        </td>
       </tr>
      </table>
     </asp:Panel>
     <div class="sncore_h2">
      When
     </div>
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label runat="server" ID="labelSchedule" />
       </td>
      </tr>
     </table>
     <SnCore:AccountTimeZone CssClass="sncore_inner_table" Width="95%" runat="server" ID="timezone" />
     <div class="sncore_h2">
      Where
     </div>
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <a id="placeLink" runat="server">
         <img border="0" runat="server" id="placeImage" />
        </a>
       </td>
       <td class="sncore_table_tr_td">
        <a class="sncore_place_name" id="placeLink2" runat="server">
         <asp:Label ID="placeName" runat="server" />
        </a>
        <div class="sncore_description">
         <asp:Label ID="placeNeighborhood" runat="server" />
         <asp:Label ID="placeCity" runat="server" />
         <asp:Label ID="placeState" runat="server" />
         <asp:Label ID="placeCountry" runat="server" />
        </div>
       </td>
      </tr>
     </table>
     <!-- NOEMAIL-START -->
     <a name="Comments" />
     <SnCore:DiscussionFullView runat="server" ID="discussionAccountEvents" Text="Reviews"
      OuterWidth="472" PostNewText="&#187; Post a Review" />
     <!-- NOEMAIL-END -->
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
