xsltc /settings:document+,script+ Travelport_CrypticRQ.xsl
xsltc /settings:document+,script+ Travelport_CrypticRS.xsl
xsltc /settings:document+,script+ Travelport_PNRRepriceRQ.xsl
xsltc /settings:document+,script+ Travelport_PNRRepriceRS.xsl
xsltc /settings:document+,script+ Travelport_QueueRQ.xsl
xsltc /settings:document+,script+ Travelport_QueueRS.xsl
xsltc /settings:document+,script+ Travelport_UpdateInsertRQ.xsl
xsltc /settings:document+,script+ v03_Travelport_PNRReadRQ.xsl
xsltc /settings:document+,script+ v03_Travelport_PNRReadRS.xsl

xcopy "*.dll" "..\DLLs" /c /d /i /y
del "*.dll" 
