<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountGroupView.aspx.cs"
 Inherits="AccountGroupView" Title="Account Group | View" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="CounterView" Src="CounterViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RedirectView" Src="AccountRedirectViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountsView" Src="AccountGroupAccountsViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacesView" Src="AccountGroupPlacesViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionView" Src="DiscussionViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="BlogView" Src="AccountBlogViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PicturesView" Src="AccountGroupPicturesViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:Panel CssClass="panel" ID="pnlAccountGroup" runat="server">
  <table cellspacing="0" cellpadding="4" class="sncore_table">
   <tr>
    <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
     <SnCore:PicturesView id="picturesView" runat="server" />
    </td>
    <td valign="top" width="*">
     <table class="sncore_inner_table" width="95%">
      <tr>
       <td class="sncore_table_tr_td">
        <asp:Label CssClass="sncore_account_name" ID="accountgroupName" runat="server" />
        <div>
         <SnCore:RedirectView id="redirect" runat="server" />
        </div>
        <div class="sncore_description">
         <asp:Label ID="accountgroupDescription" runat="server" />
        </div>
        <!-- NOEMAIL-START -->
         <div class="sncore_description">
          group views: <SnCore:CounterView ID="counterAccountGroupViews" runat="server" />
         </div>
        <!-- NOEMAIL-END -->
       </td>
       <td align="right" valign="middle">
        <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountGroupRss.aspx" />
       </td>
      </tr>
      <tr>
       <td colspan="2" class="sncore_table_tr_td" style="text-align: right;">
        <div>
         <asp:LinkButton ID="linkRequest" runat="server" Text="&#187; Join this Group" OnClick="linkRequest_Click" />
        </div>
        <div>
         <asp:LinkButton ID="linkLeave" runat="server" Text="&#187; Leave this Group" OnClick="linkLeave_Click" 
          OnClientClick="return confirm('Are you sure you want to leave this group?');" />
        </div>
        <div>
         <asp:HyperLink ID="linkInviteFriend" runat="server" Text="&#187; Invite a Friend" NavigateUrl="AccountGroupAccountInvitationEdit.aspx" />
        </div>
        <div>
         <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
        </div>
        <div>
         <asp:HyperLink ID="linkRequests" runat="server" Text="&#187; Membership Requests" 
          NavigateUrl="AccountGroupAccountRequestsManage.aspx" />
        </div>
        <div>
         <asp:HyperLink ID="linkMembers" runat="server" Text="&#187; Group Members" 
          NavigateUrl="AccountGroupAccountsView.aspx" />
        </div>
        <asp:Panel ID="panelGroupAdmin" runat="server">
         <div>
          <asp:HyperLink ID="linkPictures" runat="server" Text="&#187; Upload a Picture" 
           NavigateUrl="AccountGroupPicturesManage.aspx" />
         </div>
         <div>
          <asp:LinkButton ID="linkDelete" runat="server" Text="&#187; Delete this Group" OnClick="linkDelete_Click" 
           OnClientClick="return confirm('Are you sure you want to delete this group?');" />
         </div>
         <div>
          <asp:HyperLink ID="linkEditGroup" runat="server" Text="&#187; Edit Group" />
         </div>
         <div>
          <asp:HyperLink ID="linkEditGroupDiscussion" runat="server" Text="&#187; Edit Group Discussion" />
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
        socially bookmark this group:
       </td>
       <td class="sncore_table_tr_td">
        <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
       </td>
      </tr>
     </table>
     <a name="discuss"></a>
     <SnCore:BlogView runat="server" ID="blogView" />
     <SnCore:DiscussionView runat="server" ID="discussionView" PostNewText="&#187; Post New" 
      OuterWidth="472" CssClass="sncore_account_table" />
     <a name="members"></a>
     <SnCore:AccountsView runat="server" ID="accountsView" />
     <SnCore:PlacesView runat="server" ID="placesView" />
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
