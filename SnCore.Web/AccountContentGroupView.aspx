<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountContentGroupView.aspx.cs" Inherits="AccountContentGroupView" Title="Content Group" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label ID="labelName" runat="server" />
    </div>
    <div class="sncore_h2sub">
     <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
    </div>
   </td>
   <td>
    <asp:Label ID="labelDescription" CssClass="sncore_description" runat="server" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountContentGroupViewRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountContentGroupViewRss.aspx" />
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage"
    AutoGenerateColumns="false" CssClass="sncore_table" AllowPaging="true" AllowCustomPaging="true" 
    PageSize="5" BorderWidth="0" ShowHeader="false">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
      <ItemTemplate>
       <div class="sncore_message_table">
        <div class="sncore_message_body">
         <%# ((bool) Eval("AccountContentGroupTrusted")) ? Eval("Text") : base.RenderEx(Eval("Text"))  %>
        </div>
       </div>
      </ItemTemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
