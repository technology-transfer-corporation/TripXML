@echo off
set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;%path%

xsltc /settings:document+,script+ PortalXML_LowFarePlusRQ.xsl
xsltc /settings:document+,script+ PortalXML_LowFarePlusRS.xsl
xsltc /settings:document+,script+ PortalXML_LowFareRQ.xsl
xsltc /settings:document+,script+ PortalXML_LowFareRS.xsl
