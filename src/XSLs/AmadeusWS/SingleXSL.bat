@echo off
rem set /p id="Enter XSLT Name: "
set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;%path%
xsltc /settings:document+,script+ %~1%

xcopy "*.dll"  "..\..\TripXMLMain\Xsl\AmadeusWS"  /c /d /i /y
xcopy "*.dll"  "..\..\wsTripXML\Xsl\AmadeusWS"  /c /d /i /y
xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\AmadeusWS"  /c /d /i /y

del "*.dll" 
