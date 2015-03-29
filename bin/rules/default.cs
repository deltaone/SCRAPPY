// Debug.Assert(false, "Debug point!");
// MAIN.DumpObject(rows[0].Clean(), "row");
// MAIN.DumpObject(table[0]["description"], "description");

<[*www.elite-games.ru*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.elite-games.ru/
	v["PageTitle"] = "Elite Games"; 
	v["PageDescription"] = "Elite Games - Новости";
	
	var rows = GetRowsByXPath(html.DocumentNode, ".//td[@bgcolor='#CCCCFF']/../..");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"date", 		"./tr[1]/td[1]/b/font",	"InnerText",	"$1"},
	{	"title",		"./tr[1]/td[1]/b",		"InnerText",	"$1"},
	{	"description",	"./tr[3]/td[1]",		"InnerHtml",	"$1"},
	});
}

<[*www.psjailbreak.ru*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.psjailbreak.ru/
	v["PageTitle"] = "PSJailBreak";
	v["PageDescription"] = "Cвободу Playstation 3!";
	
	var rows = GetRowsByXPath(html.DocumentNode, ".//div[@class='bitchpost']/div[3]/..");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		".//img",				"src",			"$1"},
	{	"date", 		"./div[2]/ul/li[3]",	"InnerText",	"$1"},
	{	"link",			"./div[1]/h3/a",		"href",			"$1"},
	{	"title",		"./div[1]/h3/a",		"InnerText",	"$1"},
	{	"description",	"./div[3]/div",			"InnerText",	"$1"},
	});
}

<[*u3d.at.ua*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://u3d.at.ua/load/
	v["PageTitle"] = "u3d.at.ua";
	v["PageDescription"] = "Всё для unity3d";
	
	var rows = GetRowsByXPath(html.DocumentNode, ".//div[starts-with(@id, 'entryID')]");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"./div/div[1]/a/img",								"src",			"$1"},
	{	"date", 		"./div/div[2]/table/tbody/tr[2]/td/div/div[1]",		"InnerText",	"$1"},
	{	"link",			"./div/div[2]/div[2]/a",							"href",			"$1"},
	{	"title",		"./div/div[2]/div[2]/a",							"InnerText",	"$1"},
	{	"description",	"./div/div[2]/table/tbody/tr[1]/td/div",			"InnerHtml",	"$1"},
	});
}

<[*unitplayer.ru*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://unitplayer.ru/
	v["PageTitle"] = "UnitPlayer.ru";
	v["PageDescription"] = "Всё для unity3d";
	
	var rows = GetRowsByXPath(html.DocumentNode, ".//div[starts-with(@id, 'entryID')]");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"./div/div[1]/div[1]/center/a/img",		"src",			"$1"},
	{	"date", 		"./div/div[2]/div",						"InnerText",	"$1"},
	{	"link",			"./div/div[1]/h3/a",					"href",			"$1"},
	{	"title",		"./div/div[1]/h3/a",					"InnerText",	"$1"},
	{	"description",	"./div/div[1]/p[@class=\"message\"]",	"InnerHtml",	"$1"},
	});
}

<[*1c.ru*release*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://1c.ru/rus/support/release/categ.jsp?GroupID=88
	v["PageTitle"] = "Текущие релизы программ фирмы '1С'";
	v["PageDescription"] = "Конфигурации для России";
	
	var rows = GetRowsByXPath(html.DocumentNode, @".//table[@class=""content""]/tbody/tr/td[2]/..");
	var table = ExtractToHashtables(rows, new string[,] {
	{	"date", 		"./td[1]/span",		"InnerText",		"$1"},
	{	"title",		"./td[2]/span",		"InnerText",		"$1"},
	{	"release",		"./td[3]/span",		"InnerText",		"$1"},	
	});	
	foreach(var r in table) {
		r["title"] = "[" + r["date"] + "] " + r["title"] + " (" + r["release"] + ")";		
	}
	v["PageTable"] = table;
}

<[*www.klerk.ru*articles*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.klerk.ru/soft/articles/page/1/
	v["PageTitle"] = "Статьи программисту";
	v["PageDescription"] = "Клерк.ру";

	var rows = GetRowsByXPath(html.DocumentNode, ".//article/header/..");
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"date", 		"./header/p/span",		"InnerText",		"$1"},
	{	"link",			"./header/h1/a",		"href",				"$1"},
	{	"title",		"./header/h1/a",		"InnerText",		"$1"},
	{	"description",	"./a",					"InnerHtml",		"$1"}, // <p> not closed
	});
}

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
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		"<img src=\"(.*?)\" class=", 											"$1"},
	{	"date", 		"class=\"post-date\">(.*?)</p>", 										"$1"},
	{	"link",			"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$1"},
	{	"title",		"<h2 class=\"post-title\">\\s+<a href=\"(.*?)\" .*?title=\"(.*?)\"",	"$2"},
	{	"description",	"entry excerpt\">.*<p>(.*?)</p>\\s*</div>", 							"$1"},
	});
}

<[*www.pockettactics.com*]>	
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // http://www.pockettactics.com/
	v["PageTitle"] = "Pocket Tactics";
	v["PageDescription"] = "The home of proper games on iPhone, iPad and Android: strategy, RPGs, sims, and more.";

	var rows = GetRowsByRegex(pageText, "(<article id=\"post-.*?</article>)", "$1");	
	v["PageTable"] = ExtractToHashtables(rows, new string[,] {
	{	"image",		@"wp-image.*?src=""(.*?)""",								"$1"},
	{	"date", 		@"pubdate>(.*?)</time>", 									"$1"},
	{	"link",			@"<h1><a href=""(.*?)"".*?title=""(.*?)"">",				"$1"},
	{	"title",		@"<h1><a href=""(.*?)"".*?title=""(.*?)"">",				"$2"},
	{	"description",	@"<p class=""wp-caption-text.*?(<p>.*?</p>)\s*</section>",	"$1"},
	});
}

<[*www.avito.ru*bytovaya_elektronika*]>	
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{ 	// https://www.avito.ru/moskva/bytovaya_elektronika?view=list
	v["PageTitle"] = "Авито - электроника";
	v["PageDescription"] = "Бесплатные объявления раздела бытовой электроники";
	
	var rows = GetRowsByRegex(pageText, "(<div class=\"price\">.*?</span>\\s+</div>\\s+</div>)", "$1");
	var table = ExtractToHashtables(rows, new string[,] {
	{	"date", 		"<span class=\"date\">(.*?)</span>", 					"$1"},
	{	"link",			"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"$1"},
	{	"title",		"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"$2"},
	{	"category",		"<span class=\"c.*?\">(.*?)</span>", 					"$1"},
	{	"cost", 		"^.*?<p>\\s+(.*?\\.)\\s+<", 							"$1"}
	});
	foreach(var r in table) {
		r["description"] = "<b>" + r["title"] + "</b><br>Категория: " +  r["category"] + "<br><i>Цена: {0}</i>".Place((string)r["cost"]);
		r["title"] = r["title"] + " (" + r["category"] + ") " + r["cost"];
	}
	v["PageTable"] = table;
}

<[*www.avito.ru*bytovaya_elektronika*]>
public static void Process(string pageText, HtmlDocument html, Hashtable v)
{   // https://www.avito.ru/moskva/bytovaya_elektronika?view=list
	v["TEMPLATE"] = "table.st";
	v["PageTitle"] = "Авито - электроника";
	v["PageText"] = "При генерации страницы был использован 'SCRAPPY' ...";
	v["PageTableTitle"] = "Бесплатные объявления раздела бытовой электроники";
	v["PageTextBottom"] = "Generated: " + v["PageDate"] + "<br>@ 2015 de1ta0ne";
	
	var rows = GetRowsByRegex(pageText, "(<div class=\"price\">.*?</span>\\s+</div>\\s+</div>)", "$1");
	v["PageTable"] = ExtractToArrays(rows, new string[,] {
	{	"Дата", 			"<span class=\"date\">(.*?)</span>", 					"<span class=\"keywords\">$1</span>"},
	{	"Наименование", 	"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"<a href=\"$1\">$2</a>" 			},
	{	"Цена", 			"^.*?<p>\\s+(.*?\\.)\\s+<", 							"<div align=\"right\">$1</div>" 	},
	{	"Категория",		"<span class=\"c.*?\">(.*?)</span>", 					"<span class=\"keywords\">$1</span>"}
	});
}
