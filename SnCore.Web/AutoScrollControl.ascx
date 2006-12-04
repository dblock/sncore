<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoScrollControl.ascx.cs"
 Inherits="AutoScrollControl" %>

<script type="text/javascript">
  function onAutoScrollControlLoad() 
  {
    window.location.href = '#<% Response.Write(ScrollLocation); %>';
  }
  
  Sys.Application.add_load(function() { onAutoScrollControlLoad(); }); 
</script>

