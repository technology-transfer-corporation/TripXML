@echo off
set /p id="Enter XSLT Name: "

set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%
xsltc /settings:document+,script+ %id%


xcopy "%id%.dll"  "..\..\wsTripXML\Xsl\Travelport"  /c /d /i /y
del "%id%.dll" 

pause