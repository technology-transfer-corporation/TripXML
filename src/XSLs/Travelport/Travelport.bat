@echo off
rem set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%
xsltc /settings:document+,script+ Travelport_CrypticRQ.xsl
xsltc /settings:document+,script+ Travelport_CrypticRS.xsl
xsltc /settings:document+,script+ Travelport_PNRRepriceRQ.xsl
xsltc /settings:document+,script+ Travelport_PNRRepriceRS.xsl
xsltc /settings:document+,script+ Travelport_QueueRQ.xsl
xsltc /settings:document+,script+ Travelport_QueueRS.xsl
xsltc /settings:document+,script+ Travelport_UpdateInsertRQ.xsl
xsltc /settings:document+,script+ v03_Travelport_PNRReadRQ.xsl
xsltc /settings:document+,script+ v03_Travelport_PNRReadRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\Xsl\Travelport"  /c /d /i /y
xcopy "*.dll"  "..\..\wsTripXML\Xsl\Travelport"  /c /d /i /y

del "*.dll" 
pause