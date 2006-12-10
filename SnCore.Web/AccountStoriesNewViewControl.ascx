<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountStoriesNewViewControl.ascx.cs"
 Inherits="AccountStoriesNewViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountStoriesRss.aspx" ButtonVisible="false"
 Title="New Stories" />
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <div class="sncore_h2">
    <a href='AccountStoriesView.aspx'>
      New Stories
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <div class="sncore_createnew">
    <div class="sncore_link">
     <a href="AccountStoriesView.aspx">&#187; all</a>
     <a href="AccountStoryEdit.aspx">&#187; tell a story</a>
     <a href="AccountStoriesRss.aspx">&#187; rss</a>
    </div>
   </div>
  </td>
 </tr>
</table>
<asp:DataGrid CellPadding="4" ShowHeader="false" runat="server" ID="storiesView"
 AutoGenerateColumns="false" CssClass="sncore_half_table">
 <ItemStyle CssClass="sncore_table_tr_td" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn>
   <ItemTemplate>
    <div class="sncore_title">
     <a href='AccountStoryView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name"))%>
     </a>
     <span class="sncore_link">
      <a href='AccountStoryView.aspx?id=<%# Eval("Id") %>'>
       &#187; <%# GetComments((int) Eval("CommentCount")) %>
      </a>
     </span>     
    </div>
    <!--
    <div style="font-size: smaller;">
     by 
     <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
      <%# base.Render(Eval("AccountName"))%>
     </a>
     on
     <%# base.Adjust(Eval("Created")).ToString("d") %>
    </div>
    -->
    <div class="sncore_description">
     <%# base.GetDescription((string) Eval("Summary"))%>
    </div>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
