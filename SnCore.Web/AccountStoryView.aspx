<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoryView.aspx.cs"
 Inherits="AccountStoryView" Title="Story" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="CounterView" Src="CounterViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PicturesView" Src="AccountStoryPicturesViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="storyName" runat="server" />
    </div>
    <!-- NOEMAIL-START -->
    <div class="sncore_h2sub">
     <asp:HyperLink ID="linkAuthor" runat="server" />
     <a href="AccountStoriesView.aspx">&#187; All Stories</a>
     <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
    </div>
    <asp:Panel ID="panelOwner" runat="server">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td align="right">
        <div>
         <asp:HyperLink runat="server" ID="linkEdit" Text="&#187; Edit Story" />
        </div>
        <div>
         <asp:HyperLink runat="server" ID="linkAddPhotos" Text="&#187; Add Photos" />
        </div>
       </td>
      </tr>
     </table>
    </asp:Panel>
    <!-- NOEMAIL-END -->
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_table_tr_td">
       <asp:Label ID="storySummary" runat="server" />
      </td>
     </tr>
    </table>
    <!-- NOEMAIL-START -->
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_table_tr_td" align="center">
       <SnCore:PicturesView id="picturesView" runat="server" />
      </td>     
     </tr>
    </table>
    <!-- NOEMAIL-END -->
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td>
       <SnCore:LicenseView runat="server" ID="licenseView" />       
      </td>
      <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
       socially bookmark this story:
      </td>
      <td class="sncore_table_tr_td">      
       <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
      </td>
      <!-- NOEMAIL-START -->
      <td class="sncore_table_tr_td">
       <div class="sncore_description">
        views: <SnCore:CounterView ID="counterProfileViews" runat="server" />
       </div>
      </td>
      <!-- NOEMAIL-END -->
     </tr>
    </table>
    <!-- NOEMAIL-START -->
    <a name="comments"></a>
    <SnCore:DiscussionFullView runat="server" ID="storyComments" OuterWidth="624" PostNewText="&#187; Post a Comment" />
    <!-- NOEMAIL-END -->
   </td>
  </tr>
 </table>
</asp:Content>
