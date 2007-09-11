<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountEventPicturesView.aspx.cs" Inherits="AccountEventPicturesView" Title="AccountEvent | Pictures" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Event Pictures
 </div>
 <div class="sncore_h2sub">
  <a href='AccountEventPicturesManage.aspx?id=<% Response.Write(base.RequestId); %>'>&#187; Upload a Picture</a>
 </div>
 <asp:DataList RepeatColumns="4" runat="server" ID="listView" CssClass="sncore_table">
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <ItemTemplate>
   <a href="AccountEventPictureView.aspx?id=<%# Eval("Id") %>">
    <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("Id") %>"
     alt="<%# base.Render(Eval("Name")) %>" />
    <div>
     <%# base.Render(Eval("Description")) %>
    </div>
    <div style="font-size: smaller;">
     <%# GetCommentCount((int) Eval("CommentCount")) %>
    </div>
   </a>
  </ItemTemplate>
 </asp:DataList>
</asp:Content>
