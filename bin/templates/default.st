<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <title><![CDATA[$PageTitle]]></title>
  <link href="$PageURL"/>
  <id><![CDATA[$PageURL]]></id>
  <updated><![CDATA[$PageDate]]></updated>

#foreach($row in $PageTable)
	#before
  <entry>
	#each
	<title><![CDATA[$row.get_Item('title')]]></title>
	#if($row.get_Item('link'))
		<link href="$row.get_Item('link')"/>
	#else
		<link href="/"/>
	#end
	<id><![CDATA[$row.get_Item('title')]]></id>
	#if($row.get_Item('date'))	
		<updated><![CDATA[$row.get_Item('date')]]></updated>
	#else
		<updated>Today</updated>
	#end
    <content type="html">	
		<![CDATA[
		#if($row.get_Item('image'))
			<a href="$row.get_Item('image')"><img style="float: left; margin: 0 20px 10px 0; width: 250px" src="$row.get_Item('image')"></a>
		#end  
		#if($row.get_Item('description'))
			$row.get_Item('description')
		#end    
		]]>
	</content>
	#after
  </entry>
#end

</feed>