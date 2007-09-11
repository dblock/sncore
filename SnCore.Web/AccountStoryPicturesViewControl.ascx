<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountStoryPicturesViewControl.ascx.cs"
 Inherits="AccountStoryPicturesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="panelPictures">
 <ContentTemplate>
  <SnCoreWebControls:PagedList runat="server" RepeatDirection="Horizontal"
   ID="picturesView" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
   ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="4" RepeatRows="1" AllowCustomPaging="true">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
   <ItemStyle VerticalAlign="Middle" />
   <ItemTemplate>
    <a href='<%# string.Format("AccountStoryPictureView.aspx?id={0}", Eval("Id")) %>'>
     <img border="0" src='<%# string.Format("AccountStoryPictureThumbnail.aspx?id={0}", Eval("Id")) %>'
      alt='<%# base.Render(Eval("Name")) %>' />
     <div style="font-size: smaller;">
      <%# GetCommentCount((int) Eval("CommentCount")) %>
     </div>
    </a>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
