<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoryPictureView.aspx.cs" Inherits="AccountStoryPictureView" Title="AccountStoryPicture | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel runat="server" ID="panelPicture" UpdateMode="Always">
  <ContentTemplate>
   <div class="sncore_navigate">
    <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
    <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountStory" Text="Story" runat="server" />
    <asp:Label CssClass="sncore_navigate_item" ID="labelPicture" Text="Picture" runat="server" />
   </div>
   <div class="sncore_h2">
    <asp:Label ID="labelAccountStoryName" runat="server" Text="AccountStory" />
   </div>
   <div class="sncore_h2sub">
    <asp:HyperLink ID="linkBack" runat="server" Text="&#187; Back" />
    <asp:HyperLink ID="linkComments" runat="server" NavigateUrl="#comments" />
   </div>
   <table cellspacing="0" cellpadding="4" class="sncore_table">
    <tr>
     <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 200px;">
      <SnCoreWebControls:PagedList OnItemCommand="picturesView_ItemCommand" runat="server" ID="picturesView"
       RepeatColumns="1" RepeatRows="5" AllowCustomPaging="true">
       <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="&#187;"
        prevpagetext="&#171;" horizontalalign="Center" />
       <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
       <ItemTemplate>
        <asp:ImageButton ID="pictureThumbnail" runat="server" CommandName="Picture" CommandArgument='<%# Eval("Id") %>' 
         ImageUrl='<%# string.Format("AccountStoryPictureThumbnail.aspx?id={0}", Eval("Id")) %>' AlternateText='<%# base.Render(Eval("Name")) %>' />
        <div style="font-size: smaller;">
         <asp:LinkButton runat="server" ID="pictureThumbnailLink" Text='<%# ((int) Eval("CommentCount") >= 1) ? Eval("CommentCount").ToString() + 
          " comment" + (((int) Eval("CommentCount") == 1) ? "" : "s") : "" %>' CommandName="Picture" CommandArgument='<%# Eval("Id") %>' />
        </div>
       </ItemTemplate>
      </SnCoreWebControls:PagedList>
     </td>
     <td valign="top" width="*">
      <div class="sncore_link" style="text-align: center; margin: 10px;">
       <asp:LinkButton ID="linkPrev" Text="&#171; Prev" runat="server" OnCommand="picturesView_ItemCommand" CommandName="Picture" /> |
       <asp:Label ID="labelIndex" runat="server" />
       | <asp:LinkButton ID="linkNext" Text="Next &#187;" runat="server" OnCommand="picturesView_ItemCommand" CommandName="Picture" />        
      </div>
      <div style="text-align: center; width: 100%;">
       <img runat="server" id="inputPicture" src="AccountStoryPictureThumbnail.aspx?id=0" />
      </div>
      <table class="sncore_inner_table" width="95%">
       <tr>
        <td class="sncore_form_label">
         name:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="inputName" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         uploaded:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="inputCreated" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         views:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="inputCounter" runat="server" />
        </td>
       </tr>
      </table>
      <a name="comments"></a>
      <SnCore:DiscussionFullView runat="server" ID="discussionComments" Text="Comments" PostNewText="&#187; Post a Comment" />  
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
