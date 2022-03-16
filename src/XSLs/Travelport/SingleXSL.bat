@echo off
rem set /p id="Enter XSLT Name: "
rem set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%
xsltc /settings:document+,script+ %~1%

xcopy "*.dll"  "..\..\TripXMLMain\Xsl\Travelport"  /c /d /i /y
xcopy "%id%.dll"  "..\..\wsTripXML\Xsl\Travelport"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\Travelport"  /c /d /i /y

del "*.dll" 