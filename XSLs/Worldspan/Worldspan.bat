@echo off
rem set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%

xsltc /settings:document+,script+ Worldspan_AirPriceRQ.xsl
xsltc /settings:document+,script+ Worldspan_AirPriceRS.xsl
xsltc /settings:document+,script+ Worldspan_AirRulesRQ.xsl
xsltc /settings:document+,script+ Worldspan_AirRulesRS.xsl
xsltc /settings:document+,script+ Worldspan_AirSeatMapRQ.xsl
xsltc /settings:document+,script+ Worldspan_AirSeatMapRS.xsl
xsltc /settings:document+,script+ Worldspan_AuthorizationRQ.xsl
xsltc /settings:document+,script+ Worldspan_AuthorizationRS.xsl
xsltc /settings:document+,script+ Worldspan_CrypticRQ.xsl
xsltc /settings:document+,script+ Worldspan_CrypticRS.xsl
xsltc /settings:document+,script+ Worldspan_FareDisplayRQ.xsl
xsltc /settings:document+,script+ Worldspan_IssueTicketSessionedRQ.xsl
xsltc /settings:document+,script+ Worldspan_IssueTicketSessionedRS.xsl
xsltc /settings:document+,script+ Worldspan_FareDisplayRS.xsl
xsltc /settings:document+,script+ Worldspan_LowFarePlusRQ.xsl
xsltc /settings:document+,script+ Worldspan_LowFarePlusRS.xsl
xsltc /settings:document+,script+ Worldspan_LowFareRQ.xsl
xsltc /settings:document+,script+ Worldspan_LowFareRS.xsl
xsltc /settings:document+,script+ Worldspan_PNRCancelRQ.xsl
xsltc /settings:document+,script+ Worldspan_PNRCancelRS.xsl
xsltc /settings:document+,script+ Worldspan_PNRReadRQ.xsl
xsltc /settings:document+,script+ Worldspan_PNRReadRS.xsl
xsltc /settings:document+,script+ Worldspan_PNRRepriceRQ.xsl
xsltc /settings:document+,script+ Worldspan_PNRRepriceRS.xsl
xsltc /settings:document+,script+ Worldspan_QueueRQ.xsl
xsltc /settings:document+,script+ Worldspan_QueueRS.xsl
xsltc /settings:document+,script+ Worldspan_TravelBuildRQ.xsl
xsltc /settings:document+,script+ Worldspan_UpdateDeleteRQ.xsl
xsltc /settings:document+,script+ Worldspan_UpdateInsertRQ.xsl
xsltc /settings:document+,script+ v03_Worldspan_PNRReadRQ.xsl
xsltc /settings:document+,script+ v03_Worldspan_PNRReadRS.xsl
xsltc /settings:document+,script+ v03_Worldspan_LowFarePlusRQ.xsl
xsltc /settings:document+,script+ v03_Worldspan_LowFarePlusRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\Xsl\Worldspan"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\Worldspan"  /c /d /i /y

del "*.dll" 
pause