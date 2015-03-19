<html>
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8">
<title>$PageTitle</title>
#include("style.st")
</head>
<body>

<center>
<a href="$PageURL">$PageTitle</a>
</center>
<table width=100%>

#if($PageText)
<tr><td align=center>
	<span class="keywords">$PageText</span><br>
</td></tr>
#end

#if($PageTableTitle)
<tr><td align=center>
	<span class="header">$PageTableTitle</span><br>
</td></tr>
#end

<tr><td align=center>
	<table border=0 cellpadding=5 cellspacing=0 style="border: 2px solid #a9a9a9">
#foreach($row in $PageTable)
	#odd
	<tr bgcolor=#f0f0ca>
	#even
	<tr bgcolor=#fdfdca>
	#each
		#if($velocityCount == 1)
			#foreach($col in $row)<th>$col</th>
			#end
		#else	
			#foreach($col in $row)<td>$col</td>
			#end
		#end
	#after
	</tr>
#end	
	</table>
</td></tr>

#if($PageTextBottom)
<tr><td align=center>
	<span class="keywords">$PageTextBottom</span>		
</td></tr>
#end

</table>
</body>
</html>