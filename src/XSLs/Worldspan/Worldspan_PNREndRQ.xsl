<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
  ==================================================================
   Worldspan_PNREndRQ.xsl															
  ================================================================== 
   Date: 17 Dec 2024 - Kobelev - New file											
  ================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_PNREndRQ"/>
	</xsl:template>
	<xsl:template match="OTA_PNREndRQ">
		<ScreenEntry>6.TRIPXML</ScreenEntry>
		<ScreenEntry>ER</ScreenEntry>
	</xsl:template>
</xsl:stylesheet>