<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MadLibInstancesViewControl.ascx.cs"
 Inherits="MadLibInstancesViewControl" %>
 
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<SnCore:Title ID="titleNewMadLib" Text="Mad Libs" runat="server">  
 <Template>
  <div class="sncore_title_paragraph">
   <b>Mad Libs</b> is the name of a word game that uses word substitutions for humorous effect. They are especially popular with children 
   and frequently played as a party game or as a pastime. Mad Libs consist of a book that has a short story on each page, but with 
   many of the key words are replaced with blanks. Beneath each blank is specified a lexical or other category, such as noun, verb,
   place, or a part of the body. The result is usually comic, surreal and somewhat nonsensical.
  </div>
 </Template>
</SnCore:Title>
<div class="sncore_cancel">
 <asp:HyperLink ID="linkNew" runat="server" Text="&#187; Go Mad!" />
</div>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="madlibs" RepeatRows="3" RepeatColumns="1" 
 RepeatDirection="Vertical" CssClass="sncore_inner_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="true"
 OnItemCommand="madlibs_ItemCommand">
 <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemStyle CssClass="sncore_message_tr_td" />
 <ItemTemplate>
  <div class="sncore_message">
   <div class="sncore_person">
    <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
     <img border="0" width="50px" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
    </a>
   </div>
   <div class="sncore_header">
    posted by
    <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
     <%# base.Render(Eval("AccountName")) %>
    </a>
    on
    <%# base.Adjust(Eval("Created")).ToString() %>
   </div>
   <div class="sncore_content_madlib">
    <div class="sncore_message_body">
     <%# RenderMadLib(Renderer.Render((string)Eval("Text")))%>
    </div>
   </div>
   <div class="sncore_footer">
    <asp:HyperLink ID="linkEdit" runat="server" Text="&#187; edit"
     NavigateUrl='<%# GetEditUrl((int) Eval("Id"), (int) Eval("MadLibId")) %>' 
     Visible='<%# SessionManager.IsLoggedIn && ((int) Eval("AccountId")) == SessionManager.Account.Id || SessionManager.IsAdministrator %>' />
    <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" 
     OnClientClick="return confirm('Are you sure you want to do this?')" CommandArgument='<%# Eval("Id") %>'
     Visible='<%# SessionManager.IsLoggedIn && ((int) Eval("AccountId")) == SessionManager.Account.Id || SessionManager.IsAdministrator %>' />
   </div>
  </div> 
 </ItemTemplate>
</SnCoreWebControls:PagedList>