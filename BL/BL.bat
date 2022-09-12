@echo off
rem setlocal
set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%

xsltc /settings:document+,script+ BL_LowFareNoFareTypeRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoTktRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoMixRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoCountryRS.xsl
xsltc /settings:document+,script+ BL_LowFareRQ.xsl
xsltc /settings:document+,script+ BL_LowFareRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\Xsl\BL"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\BL"  /c /d /i /y

del "*.dll" 
pause