<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <title>$PageTitle</title>
  <link href="$PageURL"/>
  <id>$PageURL</id>
  <updated>$PageDate</updated>
  
#foreach($row in $PageTable)
	#before
  <entry>
	#each
	<title>$row.get_Item('title')</title>    
    <link href="$row.get_Item('link')"/>
	<id>$row.get_Item('link')</id>
	<updated>$row.get_Item('date')</updated>
    <content type="html">
	
	#if($row.get_Item('image'))
	  <![CDATA[   
		<table border=0 cellpadding=5 cellspacing=0>
		<tr>
		<td><img src="$row.get_Item('image')"></td>
		<td style="font: 80% Verdana, Arial, sans-serif;" valign="top">$row.get_Item('description')</td>
		</tr>
		</table>
	  ]]>
	#else
      <![CDATA[$row.get_Item('description')]]>
    #end
	
	</content>
	#after
  </entry>
#end
  
</feed>