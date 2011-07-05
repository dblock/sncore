// By use of this code snippet, I agree to the Brightcove Publisher T and C 
// found at http://www.brightcove.com/publishertermsandconditions.html. 

var config = new Array();

config["videoId"] = null; //the default video loaded into the player
config["videoRef"] = null; //the default video loaded into the player by ref id specified in console
config["lineupId"] = null; //the default lineup loaded into the player
config["playerTag"] = null; //player tag used for identifying this page in brightcove reporting
config["autoStart"] = false; //tells the player to start playing video on load
config["preloadBackColor"] = "#FFFFFF"; //background color while loading the player

/* do not edit these config items */
config["playerId"] = "AvVjSwktsNX%2BSjZaaDrunoPlTYprfRUmY";
config["width"] = 520;
config["height"] = 587;

createExperience(config);
