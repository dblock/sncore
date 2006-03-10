<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TagWordsViewControl.ascx.cs"
 Inherits="TagWordsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<div class="sncore_h2">
 Tags
</div>
<table class="sncore_half_table" align="center" width="95%">
 <tr>
  <td class="sncore_table_tr_td" align="center">
   <asp:Repeater runat="server" ID="tagwords">
    <ItemTemplate>
     <span style='vertical-align: middle; font-size: <%# base.GetFontSize((int) Eval("Frequency")) %>px;'><a 
      href='TagWordAccountsView.aspx?id=<%# Eval("Id") %>'><%# base.Render(Eval("Word")) %></a></span>
    </ItemTemplate>
   </asp:Repeater>
  </td>
 </tr>
</table>
