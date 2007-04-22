<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoScrollControl.ascx.cs"
 Inherits="AutoScrollControl" %>

<script type="text/javascript">
  function onAutoScrollControlLoad() 
  {
    window.scrollTo(0,0);
  }
  
  Sys.Application.add_load(function() { onAutoScrollControlLoad(); }); 
</script>

