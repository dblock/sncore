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
 <ItemTemplate>
   <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
     <td align="left" valign="top" width="*" class='<%# GetCssClass((DateTime) Eval("Created")) %>_left_border'>
      <div class="sncore_message_header">
       <div class="sncore_description">
        posted <%# base.Adjust(Eval("Created")).ToString() %>
       </div>
       <div class="sncore_description">
        <asp:HyperLink ID="linkEdit" runat="server" Text="&#187; edit"
         NavigateUrl='<%# GetEditUrl((int) Eval("Id"), (int) Eval("MadLibId")) %>' 
         Visible='<%# SessionManager.IsLoggedIn && ((int) Eval("AccountId")) == SessionManager.Account.Id || SessionManager.IsAdministrator %>' />
        <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" 
         OnClientClick="return confirm('Are you sure you want to do this?')" CommandArgument='<%# Eval("Id") %>'
         Visible='<%# SessionManager.IsLoggedIn && ((int) Eval("AccountId")) == SessionManager.Account.Id || SessionManager.IsAdministrator %>' />
       </div>
       <div class="sncore_message_body">
        <%# RenderMadLib(Renderer.Render((string)Eval("Text")))%>
       </div>
      </div>
     </td>
     <td width="150" align="center" valign="top" class='<%# GetCssClass((DateTime) Eval("Created")) %>_right_border'>
      <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
       <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
      </a>
      <div class="sncore_link_description">
       <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
        <%# base.Render(Eval("AccountName")) %>
       </a>
      </div>
     </td>
    </tr>
  </table>
 </ItemTemplate>
</SnCoreWebControls:PagedList>