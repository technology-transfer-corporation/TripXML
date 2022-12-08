set path=C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;%path%
xsltc /settings:document+,script+ BL_LowFareNoFareTypeRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoTktRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoMixRS.xsl
xsltc /settings:document+,script+ BL_LowFareNoCountryRS.xsl
xsltc /settings:document+,script+ BL_LowFareRQ.xsl
xsltc /settings:document+,script+ BL_LowFareRS.xsl