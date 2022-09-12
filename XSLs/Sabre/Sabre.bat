@echo off
rem set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools;%path%

xsltc /settings:document+,script+ Sabre_AirAvailRQ.xsl
xsltc /settings:document+,script+ Sabre_AirAvailRS.xsl
xsltc /settings:document+,script+ Sabre_AirFlifoRQ.xsl
xsltc /settings:document+,script+ Sabre_AirFlifoRS.xsl
xsltc /settings:document+,script+ Sabre_AirPriceRQ.xsl
xsltc /settings:document+,script+ Sabre_AirPriceRS.xsl
xsltc /settings:document+,script+ Sabre_AirRulesRQ.xsl
xsltc /settings:document+,script+ Sabre_AirRulesRS.xsl
xsltc /settings:document+,script+ Sabre_CarAvailRQ.xsl
xsltc /settings:document+,script+ Sabre_CarAvailRS.xsl
xsltc /settings:document+,script+ Sabre_CarInfoRQ.xsl
xsltc /settings:document+,script+ Sabre_CarInfoRS.xsl
xsltc /settings:document+,script+ Sabre_CCValidRQ.xsl
xsltc /settings:document+,script+ Sabre_CCValidRS.xsl
xsltc /settings:document+,script+ Sabre_CrypticRQ.xsl
xsltc /settings:document+,script+ Sabre_CrypticRS.xsl
xsltc /settings:document+,script+ Sabre_CurConvRQ.xsl
xsltc /settings:document+,script+ Sabre_CurConvRS.xsl
xsltc /settings:document+,script+ Sabre_FareDisplayRQ.xsl
xsltc /settings:document+,script+ Sabre_FareDisplayRS.xsl
xsltc /settings:document+,script+ Sabre_HotelAvailRQ.xsl
xsltc /settings:document+,script+ Sabre_HotelAvailRS.xsl
xsltc /settings:document+,script+ Sabre_HotelInfoRQ.xsl
xsltc /settings:document+,script+ Sabre_HotelInfoRS.xsl
xsltc /settings:document+,script+ Sabre_HotelSearchRQ.xsl
xsltc /settings:document+,script+ Sabre_HotelSearchRS.xsl
xsltc /settings:document+,script+ Sabre_IssueTicketRQ.xsl
xsltc /settings:document+,script+ Sabre_IssueTicketSessionedRQ.xsl
xsltc /settings:document+,script+ Sabre_IssueTicketRS.xsl
xsltc /settings:document+,script+ Sabre_LowFarePlusRQ.xsl
xsltc /settings:document+,script+ Sabre_LowFarePlusRS.xsl
xsltc /settings:document+,script+ Sabre_LowFarePlusRQ_Air.xsl
xsltc /settings:document+,script+ Sabre_LowFarePlusRQ_FS.xsl
xsltc /settings:document+,script+ Sabre_LowFarePlusRS_FS.xsl
xsltc /settings:document+,script+ Sabre_LowFareRQ.xsl
xsltc /settings:document+,script+ Sabre_LowFareRS.xsl
xsltc /settings:document+,script+ Sabre_LowFareSchedule2RS.xsl
xsltc /settings:document+,script+ Sabre_LowFareScheduleRS.xsl
xsltc /settings:document+,script+ Sabre_LowFareScheduleRQ.xsl
xsltc /settings:document+,script+ Sabre_PNRCancelRQ.xsl
xsltc /settings:document+,script+ Sabre_PNRCancelRS.xsl
xsltc /settings:document+,script+ Sabre_PNRReadRQ.xsl
xsltc /settings:document+,script+ Sabre_PNRReadRS.xsl
xsltc /settings:document+,script+ Sabre_PNRRepriceRQ.xsl
xsltc /settings:document+,script+ Sabre_PNRRepriceRS.xsl
xsltc /settings:document+,script+ Sabre_QueueReadRQ.xsl
xsltc /settings:document+,script+ Sabre_QueueRQ.xsl
xsltc /settings:document+,script+ Sabre_QueueRS.xsl
xsltc /settings:document+,script+ Sabre_ShowMileageRQ.xsl
xsltc /settings:document+,script+ Sabre_ShowMileageRS.xsl
xsltc /settings:document+,script+ Sabre_TB_Errors.xsl
xsltc /settings:document+,script+ Sabre_TBErrors.xsl
xsltc /settings:document+,script+ Sabre_TimeDiffRQ.xsl
xsltc /settings:document+,script+ Sabre_TimeDiffRS.xsl
xsltc /settings:document+,script+ Sabre_TravelBuildRQ.xsl
xsltc /settings:document+,script+ Sabre_UpdateInsertRQ.xsl
xsltc /settings:document+,script+ v03_Sabre_AirPriceRQ.xsl
xsltc /settings:document+,script+ v03_Sabre_AirPriceRS.xsl
xsltc /settings:document+,script+ v03_Sabre_LowFarePlusRQ.xsl
xsltc /settings:document+,script+ v03_Sabre_LowFarePlusRS.xsl
xsltc /settings:document+,script+ v03_Sabre_PNRReadRQ.xsl
xsltc /settings:document+,script+ v03_Sabre_PNRReadRS.xsl
xsltc /settings:document+,script+ v03_Sabre_TB_Errors.xsl
xsltc /settings:document+,script+ v03_Sabre_TravelBuildRQ.xsl
xsltc /settings:document+,script+ v04_Sabre_TravelBuildRQ.xsl
xsltc /settings:document+,script+ v04_Sabre_PNRReadRQ.xsl
xsltc /settings:document+,script+ v04_Sabre_PNRReadRS.xsl

xsltc /settings:document+,script+ Sabre_SalesReportRQ.xsl
xsltc /settings:document+,script+ Sabre_SalesReportRS.xsl

xsltc /settings:document+,script+ Sabre_ProfileReadRQ.xsl
xsltc /settings:document+,script+ Sabre_ProfileReadRS.xsl
xsltc /settings:document+,script+ Sabre_ProfileCreateRQ.xsl
xsltc /settings:document+,script+ Sabre_ProfileCreateRS.xsl
xsltc /settings:document+,script+ Sabre_ProfileUpdateRQ.xsl
xsltc /settings:document+,script+ Sabre_ProfileUpdateRS.xsl

rem xsltc /settings:document+,script+ Sabre_TicketCouponRQ.xsl
rem xsltc /settings:document+,script+ Sabre_TicketCouponRS.xsl

xcopy "*.dll"  "..\..\wsTripXML\bin"  /c /d /i /y
xcopy "*.dll"  "..\..\TripXMLMain\Xsl\Sabre"  /c /d /i /y
xcopy "*.dll"  "C:\TripXML\Xsl\Sabre"  /c /d /i /y

del "*.dll" 
pause