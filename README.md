# SCRAPPY
Command line regex/xpath scraper engine based on CS-Script/HtmlAgilityPack/NVelocity (can be used with WebSite-Watcher http://aignes.com)

# Description
Is designed to extract and post-processing of any information from the pages through the regexp/xpath and output the  received information through templates (thx template engine NVelocity). At the moment the output templates is RSS and simple table. Configuration data extraction is made through subprogramm is written in c # (used runtime compiler CS-Script). In principle, it turned out that you can implement your algorithm for extracting data from any page, the structure of the configuration file allows you to add your own functions and procedures.

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
  * http://www.elite-games.ru/
  * http://1c.ru/rus/support/release/categ.jsp?GroupID=88
  * http://www.klerk.ru/soft/articles/page/1/
  * http://www.spacegamejunkie.com/
  * http://www.pockettactics.com/
  * https://www.avito.ru/moskva/bytovaya_elektronika?view=list

# Example rule (regex and xpath for same site):
```csharp
<[*www.spacegamejunkie.com*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.spacegamejunkie.com/
	v["PageTitle"] = "Space Game Junkie";
	v["PageDescription"] = "Playing Through Space Gaming's Past, Present and Future";

	var rows = GetRowsByXPath(html.DocumentNode, ".//article");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"./div/div[1]/a[1]/img",	"src",				"$1"},
	{	"date", 		"./div/div[2]/p[2]",		"InnerText",		"$1"},
	{	"link",			"./div/h2/a",				"href",				"$1"},
	{	"title",		"./div/h2/a",				"title",			"$1"},
	{	"description",	"./div/div[3]",				"InnerHtml",		"$1"},
	});
}

<[*www.spacegamejunkie.com*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.spacegamejunkie.com/
	v["PageTitle"] = "Space Game Junkie";
	v["PageDescription"] = "Playing Through Space Gaming's Past, Present and Future";

	var rows = GetRowsByRegex(pageText, "(<article id=\"post-.*?</article>)", "$1");
	MAIN.DumpObject(rows[0], "row");	
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"<img src=\"(.*?)\" class=", 											"$1"},
	{	"date", 		"class=\"post-date\">(.*?)</p>", 										"$1"},
	{	"link",			"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$1"},
	{	"title",		"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$2"},
	{	"description",	"entry excerpt\">.*<p>(.*?)</p>\\s*</div>", 							"$1"},
	});
}
```

# Tips:
  * For easy extracting datarows from websites use Firefox addon "FireBug"
  * To help extract data from row using regexp try https://regex101.com/
  * To help extract data from row using xpath try Firefox addon "FirePath"
  * When write rules look into debug folder, it contains intermediate data (set debug=1 into scrappy.ini for that)

# Screenshot:
![Alt text](/result.png?raw=true "Feel the difference")

<p align="center"> <img src="https://komarev.com/ghpvc/?username=deltaone-SCRAPPY&label=Repository%20views&color=ce9927&style=flat" /> </p>
