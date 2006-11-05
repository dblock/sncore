<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoScrollControl.ascx.cs"
 Inherits="AutoScrollControl" %>

<script type="text/xml-script">
  <page xmlns:script="http://schemas.microsoft.com/xml-script/2005">
    <components>
      <application load="onAutoScrollControlLoad" />
    </components>
  </page>
</script>

<script type="text/javascript">
  function onAutoScrollControlLoad() 
  {
    window.location.href = '#<% Response.Write(ScrollLocation); %>';
  }
</script>

