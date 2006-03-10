<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountsView.aspx.cs" Inherits="AccountsView" Title="People" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     People
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountsRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountsRss.aspx" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    e-mail address:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    sort by:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectSortOrder"
     runat="server">
     <asp:ListItem Selected="True" Text="Last Activity" Value="LastLogin" />
     <asp:ListItem Text="Name" Value="Name" />
     <asp:ListItem Text="Date Joined" Value="Created" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    order by:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectOrderBy" runat="server"
     AutoPostBack="True">
     <asp:ListItem Selected="True" Text="Descending" Value="false" />
     <asp:ListItem Text="Ascending" Value="true" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    country and state:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList CssClass="sncore_form_dropdown_small" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
     ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
     runat="server" />
    <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" AutoPostBack="true"
     DataTextField="Name" DataValueField="Name" runat="server" /></td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    city:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
     Text="show people with pictures only" Checked="false" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <div class="sncore_account_name">
      <a href="AccountView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
     </div>
     <div class="sncore_description">
      last activity: <%# base.Adjust(Eval("LastLogin")).ToString("d") %>
     </div>
     <div class="sncore_description">
      <%# base.Render(Eval("City")) %>
      <%# base.Render(Eval("State")) %>
      <%# base.Render(Eval("Country")) %>
     </div>
     <br />
     <div>
      <a href='AccountPicturesView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewPictures((int) Eval("NewPictures")) %>
      </a>
     </div>
     <div>
      <a href='AccountStoryView.aspx?id=<%# GetAccountStoryId((TransitAccountStory) Eval("LatestStory")) %>'>
       <%# GetAccountStory((TransitAccountStory)Eval("LatestStory"))%>
      </a>
     </div>
     <div>
      <a href='AccountSurveyView.aspx?aid=<%# Eval("Id") %>&id=<%# GetSurveyId((TransitSurvey) Eval("LatestSurvey")) %>'>
       <%# GetSurvey((TransitSurvey)Eval("LatestSurvey"))%>
      </a>
     </div>
     <div>
      <a href='AccountDiscussionThreadsView.aspx?id=<%# Eval("Id") %>'>
       <%# GetNewDiscussionPosts((int) Eval("NewDiscussionPosts")) %>
      </a>
     </div>
     <div>
       <a href='AccountView.aspx?id=<%# Eval("Id") %>'>
        <%# GetNewSyndicatedContent((int) Eval("NewSyndicatedContent")) %>
       </a>
     </div>    
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
