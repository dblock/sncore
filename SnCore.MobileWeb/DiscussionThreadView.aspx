<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionThreadView.aspx.cs"
 Inherits="DiscussionThreadView" Title="Discussion Thread" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_description">
  <asp:Label CssClass="sncore_description" ID="discussionDescription" runat="server" />
 </div>
 <SnCoreWebControls:PagedGrid BorderColor="White" ShowHeader="false" runat="server"
  ID="discussionThreadView" AutoGenerateColumns="false" CssClass="sncore_table" BorderWidth="0"
  AllowPaging="false" AllowCustomPaging="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" PageButtonCount="5" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="CanEdit" Visible="false" />
   <asp:BoundColumn DataField="CanDelete" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
     <span class="sncore_message_tr_td_span" style='margin-left: <%# (int) Eval("Level") * 10 %>px'>
      <div class="sncore_message">
       <div class="sncore_message_subject" style='<%# (int) Eval("Level") != 0 ? "display: none;" : ""%>'>
        <%# Renderer.Render(Eval("Subject")) %>
       </div>
       <div class="sncore_header">
        posted 
        by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'><%# Renderer.Render(Eval("AccountName")) %></a>
        <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
         &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
        </span>
       </div>
       <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_content_recent" : "sncore_content" %>'
        style='width: <%# base.OuterWidth - (int) Eval("Level") * 5 %>px'>
        <div class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_message_body_recent" : "sncore_message_body" %>'>
         <%# SessionManager.RenderMarkups(Renderer.RenderEx(Eval("Body"))) %>
        </div>
       </div>
      </div>      
     </span>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
