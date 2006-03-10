<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchDiscussionPostsControl.ascx.cs"
 Inherits="SearchDiscussionPostsControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelDiscussionPostsResults" runat="server">
 <div class="sncore_h2">
  Discussion Posts
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
      <table width="100%">
       <tr>
        <td width="150" align="center" valign="top">
         <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
          <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
          <br />
          <b>
           <%# base.Render(Eval("AccountName")) %>
          </b></a>
        </td>
        <td align="left" valign="top" width="*">
         <b>discussion:</b>
         <a href='DiscussionView.aspx?id=<%# Eval("DiscussionId") %>'>
          <%# base.Render(Eval("DiscussionName"))%>
         </a>
         <br />
         <b>subject:</b>
         <a href='DiscussionThreadView.aspx?id=<%# Eval("DiscussionThreadId") %>'>
          <%# base.Render(Eval("Subject"))%>
         </a>
         <br />
         <b>posted:</b>
         <%# base.Adjust(Eval("Created")).ToString() %>
         <br />
         <br />
         <%# base.RenderEx(Eval("Body"))%>
        </td>
       </tr>
      </table>
     </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
