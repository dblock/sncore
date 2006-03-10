<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountStoriesControl.ascx.cs"
 Inherits="SearchAccountStoriesControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelStoriesResults" runat="server">
 <div class="sncore_h2">
  Stories
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="10" AllowCustomPaging="true"
  AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table" ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Top">
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
      <div>
       <%# base.Render(Eval("AccountName")) %>
      </div>
     </a>
     <div style="font-size: smaller; font-weight: bold;">
       <%# GetComments((int) Eval("CommentCount")) %>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
      <div class="sncore_h2">
       <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
      </div>
      <blockquote>
       <div style="font-size: smaller; font-weight: bold;">
         posted by 
         <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
          <%# base.Render(Eval("AccountName")) %>
         </a>
         on 
         <%# base.Adjust(Eval("Created")).ToString("d") %>     
       </div>
       <br />
       <div style="font-size: smaller;">
         <%# GetSummary((string) Eval("Summary")) %>
       </div>  
      </blockquote>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
