<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPictureView.aspx.cs" Inherits="AccountPictureView" Title="Account | Picture" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionFullView" Src="DiscussionFullViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel runat="server" ID="panelPicture" UpdateMode="Always">
  <ContentTemplate>
   <div class="sncore_h2">
    <asp:Label ID="labelAccountName" runat="server" Text="Account" />
   </div>
   <div class="sncore_h2sub">
    <asp:HyperLink ID="linkBack" runat="server" Text="&#187; Back" />
    <asp:HyperLink ID="linkComments" runat="server" NavigateUrl="#comments" />
   </div>
   <table cellspacing="0" cellpadding="4" class="sncore_table">
    <tr>
     <td class="sncore_table_tr_td_images">
      <SnCoreWebControls:PagedList OnItemCommand="picturesView_ItemCommand" runat="server" ID="picturesView"
       RepeatColumns="1" RepeatRows="5" AllowCustomPaging="true">
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="&#187;"
        PrevPageText="&#171;" HorizontalAlign="Center" PageButtonCount="4" />
       <ItemTemplate>
        <asp:ImageButton ID="pictureThumbnail" runat="server" CommandName="Picture" CommandArgument='<%# Eval("Id") %>' 
         ImageUrl='<%# string.Format("AccountPictureThumbnail.aspx?id={0}", Eval("Id")) %>' AlternateText='<%# base.Render(Eval("Name")) %>' />
        <div style="font-size: smaller;">
         <asp:LinkButton runat="server" ID="pictureThumbnailLink" Text='<%# GetCommentCount((int) Eval("CommentCount")) %>' 
          CommandName="Picture" CommandArgument='<%# Eval("Id") %>' />
        </div>
       </ItemTemplate>
      </SnCoreWebControls:PagedList>
     </td>
     <td valign="top" width="*">
      <asp:Panel runat="server" id="panelNavigator" CssClass="sncore_link" style="text-align: center; margin: 10px;">
       <asp:LinkButton ID="linkPrev" Text="&#171; Prev" runat="server" OnCommand="picturesView_ItemCommand" CommandName="Picture" /> |
       <asp:Label ID="labelIndex" runat="server" />
       | <asp:LinkButton ID="linkNext" Text="Next &#187;" runat="server" OnCommand="picturesView_ItemCommand" CommandName="Picture" />        
      </asp:Panel>
      <div style="text-align: center; width: 100%;">
       <a runat="server" id="downloadPicture">
        <img runat="server" id="inputPicture" border="0" src="AccountPictureThumbnail.aspx?id=0" />
       </a>
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
         description:
        </td>
        <td class="sncore_form_value">
         <asp:Label ID="inputDescription" runat="server" />
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
      <SnCore:DiscussionFullView runat="server" ID="discussionComments" Text="Comments" 
       OuterWidth="472" PostNewText="&#187; Post a Comment" />  
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
