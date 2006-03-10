<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoriesView.aspx.cs" Inherits="AccountStoriesView" Title="Stories" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Stories
    </div>
    <div class="sncore_h2sub">
     <a href="AccountStoryEdit.aspx">&#187; Tell a Story</a>
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountStoriesRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountStoriesRss.aspx" />
   </td>
  </tr>
 </table> 
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    search:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
    <asp:RequiredFieldValidator ID="inputSearchRequired" runat="server" ControlToValidate="inputSearch"
     CssClass="sncore_form_validator" ErrorMessage="search string is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table> 
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10" AllowCustomPaging="true"
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
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
      <div>
       <a class="sncore_story_name" href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
       <span style="font-size: smaller;">
        <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
         &#187; read and comment
        </a>
       </span>
      </div>
      <div class="sncore_description">
        posted by 
        <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
         <%# base.Render(Eval("AccountName")) %>
        </a>
        on 
        <%# base.Adjust(Eval("Created")).ToString("d") %>     
      </div>
      <div class="sncore_summary">
       <%# GetSummary((string) Eval("Summary")) %>
      </div>  
      </blockquote>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
