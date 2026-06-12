@echo off
set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;%path%

xsltc /settings:document+,script+ Portal_LowFareRQ.xsl
xsltc /settings:document+,script+ Portal_TravelBuildRQ.xsl
xsltc /settings:document+,script+ Portal_SearchPromotionsRQ.xsl
xsltc /settings:document+,script+ Portal_LowFarePlusRS.xsl
xsltc /settings:document+,script+ Portal_LowFareRS.xsl
xsltc /settings:document+,script+ Portal_LowFarePlusRQ.xsl