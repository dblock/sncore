function ads_position()
{
 var content = document.getElementById("content");
 var ads = document.getElementById("ads");
 if (ads == null || content == null) return; 
 ads.style.display = "";
 ads.style.position = "absolute";
 ads.style.width = "115px";
 ads.style.top = "2px";
 ads.style.left = (content.offsetLeft + 835) + "px";
}

window.onresize = ads_position;
Sys.Application.add_load(function() { ads_position(); }); 
