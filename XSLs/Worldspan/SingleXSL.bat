@echo off
rem set /p id="Enter XSLT Name: "
rem set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%
xsltc /settings:document %~1

xcopy "*.dll"  "..\..\TripXMLMain\Xsl\Worldspan"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\Worldspan"  /c /d /i /y

del "*.dll" 