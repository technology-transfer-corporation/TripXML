@echo off
rem set /p id="Enter XSLT Name: "
xsltc /settings:document+,script+ %~1

xcopy "*.dll"  "..\..\TripXMLMain\Xsl\AmadeusWS"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\AmadeusWS"  /c /d /i /y
xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y

del "*.dll" 
pause