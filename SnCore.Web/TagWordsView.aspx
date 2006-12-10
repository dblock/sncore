<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="TagWordsView.aspx.cs"
 Inherits="TagWordsView" Title="Tags" %>

<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Tags
    </div>
   </td>
  </tr>
  <tr>
   <td>
    <table class="sncore_table">
     <tr>
      <td>
       <asp:Repeater runat="server" ID="tagwords">
        <ItemTemplate>
         <span style='vertical-align: middle; font-size: <%# base.GetFontSize((int) Eval("Frequency")) %>px;'><a 
          href='TagWordAccountsView.aspx?id=<%# Eval("Id") %>'><%# base.Render(Eval("Word")) %></a></span>
        </ItemTemplate>
       </asp:Repeater>
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
