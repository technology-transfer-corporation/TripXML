@echo off

xsltc /settings:document+,script+ TXML_GetDealsRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\Xsl\TCML"  /c /d /i /y
xcopy "*.dll"  "..\..\wsTripXML\Xsl\TXML"  /c /d /i /y

del "*.dll" 
pause












