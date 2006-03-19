<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogsViewControl.ascx.cs"
 Inherits="AccountBlogsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BlogPreview" Src="AccountBlogPreviewControl.ascx" %>
<SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountBlogs"
 CssClass="sncore_inner_table" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false"
 Width="95%">
 <Columns>
  <asp:TemplateColumn>
   <itemtemplate>
    <div class="sncore_h2left">
     <a href='AccountBlogView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name")) %>
     </a>
    </div>
    <SnCore:BlogPreview runat="server" BlogId='<%# Eval("Id") %>' />
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>