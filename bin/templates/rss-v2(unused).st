<?xml version="1.0" encoding="utf-8" ?>
<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">

<channel>
  <title>$PageTitle</title>
  <atom:link href="$PageURL" rel="self" type="application/rss+xml" /> 
  <link>$PageURL</link>
  <description>$PageDescription</description>
  <pubDate>$PageDate</pubDate>
  <lastBuildDate>$PageDate</lastBuildDate>
  
#foreach($row in $PageTable)
	#before
  <item>
	#each
	<pubDate>$row.get_Item('date')</pubDate>
	<guid isPermaLink="true">$row.get_Item('link')</guid> 
    <link>$row.get_Item('link')</link>	
	<title>$row.get_Item('title')</title>    
	<description>
	$row.get_Item('description')
	</description>
	#after
  </item>
#end
  
</channel>
</rss>