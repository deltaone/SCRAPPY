# SCRAPPY
Command line regex scraper engine based on CS-Script and NVelocity (designed for WebSite-Watcher http://aignes.com)

# Features:
  * Create RSS feed / TABLE view from any website
  * Transform website to custom design through templates
  * Cachable precompiled processor for each bookmark
  * Easily extend scrap functionlity through adding custom functions
  * Full featured C# scripts in scraper routines (http://www.csscript.net)
  * Rich template syntax (http://www.castleproject.org/download)

# How to use:
  1. Create folder 'scrappy' in root of WebSite-Watcher
  2. Copy content of 'bin' folder into newly created 'scrappy'
  3. Create new bookmark in WebSite-Watcher
  4. Paste content of 'bin\wsw-plugin' into bookmark plugin
  5. Edit content 'WebSite-Watcher\scrappy\rules\default.cs' or create new .cs file into same folder
  6. Look into 'bin\readme' for additional info

# Example sites:
rules for these sites defined into 'rules\default.cs'
  * http://www.spacegamejunkie.com
  * https://www.avito.ru/moskva/bytovaya_elektronika?view=list

# Example rule:
<[*www.spacegamejunkie.com*]>	
public static Hashtable Process(string pageText, Hashtable v)
{   // http://www.spacegamejunkie.com/
	v["PageTitle"] = "Space Game Junkie";
	v["PageDescription"] = "Playing Through Space Gaming's Past, Present and Future";

	var rows = GetStringsByRegex(pageText, "(<article id=\"post-.*?</article>)", "$1");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"<img src=\"(.*?)\" class=", 											"$1"},	
	{	"date", 		"class=\"post-date\">(.*?)</p>", 										"$1"},	
	{	"link",			"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$1"},
	{	"title",		"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$2"},
	{	"description",	"entry excerpt\">\\s*(.*?)\\s*</div>", 									"$1"},	
	});
	return(v);
}

# Screenshot:
![Alt text](/result.png?raw=true "Feel the difference")
