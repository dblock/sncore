function ads_position()
{
 var content = document.getElementById("content")
 var ads = document.getElementById("ads");
 if (ads == null || content == null) return; 
 ads.style.position = "absolute";
 ads.style.display = "";
 ads.style.top = 2;
 ads.style.left = content.offsetLeft + 835;
}

ads_position();