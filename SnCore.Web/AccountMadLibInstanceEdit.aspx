<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountMadLibInstanceEdit.aspx.cs"
 Inherits="AccountMadLibInstanceEdit" Title="Mad Lib" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="MadLibInstanceEdit" Src="MadLibInstanceEditControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <SnCore:Title ID="titleNewMadLib" Text="Post a Mad Lib" runat="server">  
   <Template>
    <div class="sncore_title_paragraph">
     <b>Mad Libs</b> is the name of a word game that uses word substitutions for humorous effect. They are especially popular with children 
     and frequently played as a party game or as a pastime. Mad Libs consist of a book that has a short story on each page, but with 
     many of the key words are replaced with blanks. Beneath each blank is specified a lexical or other category, such as noun, verb,
     place, or a part of the body. The result is usually comic, surreal and somewhat nonsensical.
    </div>
   </Template>
  </SnCore:Title>
  <asp:HyperLink ID="linkCancel" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
  <atlas:UpdatePanel ID="panelPost" runat="server">
   <ContentTemplate>
    <table class="sncore_table">
     <tr>
      <td class="sncore_table_tr_td_madlib">
       <SnCore:MadLibInstanceEdit ID="madLibInstance" runat="server" />
      </td>
     </tr>
    </table>
    <table class="sncore_table">    
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="post" runat="server" Text="Go Mad!" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="post_Click" />
      </td>
     </tr>
    </table>
   </ContentTemplate>
  </atlas:UpdatePanel>
</asp:Content>
