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
		<a href="$row.get_Item('image')"><img style="float: left; margin: 0 20px 10px 0; width: 250px" src="$row.get_Item('image')"></a>
		$row.get_Item('description')
	  ]]>
	#else
      <![CDATA[$row.get_Item('description')]]>
    #end
	
	</content>
	#after
  </entry>
#end
  
</feed>