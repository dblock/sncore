<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountEventView.aspx.cs"
 Inherits="AccountEventView" Title="Event | View" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkCountry" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkState" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkCity" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkType" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountEvent" runat="server" />
 </div>
 <asp:Panel CssClass="panel" ID="pnlAccountEvent" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 200px;">
     <asp:Panel ID="panelNoPicture" CssClass="sncore_nopicture_table" runat="server" Visible="false">
      <img border="0" src="images/AccountEventThumbnail.gif" />
     </asp:Panel>
     <asp:DataList runat="server" ID="picturesView">
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <ItemTemplate>
       <a href="AccountEventPictureView.aspx?id=<%# Eval("Id") %>">
        <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("Id") %>" alt="<%# base.Render(Eval("Name")) %>" />
        <div>
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
        <asp:Label CssClass="sncore_event_name" ID="AccountEventName" runat="server" />
        <div>
         <asp:Label ID="AccountEventType" CssClass="sncore_account_lastlogin" runat="server" />
        </div>
        <div>
         <asp:Label ID="AccountEventCity" CssClass="sncore_event_location" runat="server" />
         <asp:Label ID="AccountEventState" CssClass="sncore_event_location" runat="server" />
        </div>
        <div>
         <asp:Label ID="AccountEventCountry" CssClass="sncore_event_location" runat="server" />
        </div>
       </td>
       <td class="sncore_table_tr_td" valign="top" align="right">
        <asp:Label ID="AccountEventId" CssClass="sncore_event_id" runat="server" />
       </td>
      </tr>
     </table>
     <asp:Panel ID="panelDetails" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td colspan="2" align="right">
         <div>
          <a href='AccountEventPicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>
           &#187; Upload a Picture</a>
         </div>
         <asp:Panel ID="panelOwner" runat="server">
          <div>
           <asp:HyperLink runat="server" ID="linkEdit" Text="&#187; Edit Content" />
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
        </td>
       </tr>
      </table>
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
     </asp:Panel>
     <asp:Panel ID="panelViews" runat="server">
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td align="right" style="font-size: smaller;">
         <% Response.Write(SuggestedBy); %>
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
          <asp:Label ID="placeCity" runat="server" />
          <asp:Label ID="placeState" runat="server" />
          <asp:Label ID="placeCountry" runat="server" />
         </div>
         <div class="sncore_description">
          <asp:Label ID="placeDescription" runat="server" />
         </div>
        </td>
       </tr>
      </table>
      <div class="sncore_h2">
       Contact
      </div>
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_form_label">
         website:
        </td>
        <td class="sncore_form_value">
         <asp:HyperLink ID="AccountEventWebsite" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         contact phone:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="AccountEventPhone" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         contact e-mail:
        </td>
        <td class="sncore_form_value">
         <asp:LinkButton ID="AccountEventEmail" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         cost to attend:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="AccountEventCost" runat="server" />
        </td>
       </tr>
      </table>
     </asp:Panel>
     <a name="Comments" />
     <SnCore:DiscussionFullView runat="server" ID="discussionAccountEvents" Text="Reviews"
      PostNewText="&#187; Post a Review" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>