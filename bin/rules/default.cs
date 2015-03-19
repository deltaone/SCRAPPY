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

<[*www.avito.ru*bytovaya_elektronika*]>	
public static Hashtable Process(string pageText, Hashtable v)
{ 	// https://www.avito.ru/moskva/bytovaya_elektronika?view=list
	v["PageTitle"] = "Авито - электроника";
	v["PageDescription"] = "Бесплатные объявления раздела бытовой электроники";	
	
	var rows = GetStringsByRegex(pageText, "(<div class=\"price\">.*?</span>\\s+</div>\\s+</div>)", "$1");
	var table = ExtractToHashtables(rows, new string[,] {
	{	"date", 		"<span class=\"date\">(.*?)</span>", 					"$1"},
	{	"link",			"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"$1"},
	{	"title",		"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"$2"},
	{	"category",		"<span class=\"c.*?\">(.*?)</span>", 					"$1"},
	{	"cost", 		"^.*?<p>\\s+(.*?\\.)\\s+<", 							"$1"}
	});		
	foreach(var r in table) {
		r["description"] = "<b>" + r["title"] + "</b><br>Категория: " +  r["category"] + "<br><i>Цена: {0}</i>".Place((string)r["cost"]);
		r["title"] = (r["title"] + " (" + r["category"] + ") " + r["cost"]).XmlEscape();
	}
	v["PageTable"] = table;
	return(v);
}

<[*www.avito.ru*bytovaya_elektronika*]>
public static Hashtable Process(string pageText, Hashtable v)
{   // https://www.avito.ru/moskva/bytovaya_elektronika?view=list
	v["TEMPLATE"] = "table.st";
	v["PageTitle"] = "Авито - электроника";
	v["PageText"] = "first line + second line + third line";
	v["PageTableTitle"] = "Бесплатные объявления раздела бытовой электроники";
	v["PageTextBottom"] = "Generated: " + v["PageDate"] + "<br>@ 2015 de1ta0ne";
	
	var rows = GetStringsByRegex(pageText, "(<div class=\"price\">.*?</span>\\s+</div>\\s+</div>)", "$1");
	v["PageTable"] = ExtractToArrays(rows, new string[,] {
	{	"Дата", 			"<span class=\"date\">(.*?)</span>", 					"<span class=\"keywords\">$1</span>"},
	{	"Наименование", 	"class=\"h3\">.*?href=\"(.*?)\".*?\">(.*?)</a>", 		"<a href=\"$1\">$2</a>"},
	{	"Цена", 			"^.*?<p>\\s+(.*?\\.)\\s+<", 							"<div align=\"right\">$1</div>"},
	{	"Категория",		"<span class=\"c.*?\">(.*?)</span>", 					"<span class=\"keywords\">$1</span>"}
	});
	return(v);
}
