<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_PNRReadRQ.xsl 					     								       -->
<!-- ================================================================== -->
<!-- Date: 22 Jun 2008 - Rastko									       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<DPC8>
		<xsl:apply-templates select="OTA_ReadRQ"/>
	</DPC8>
</xsl:template>

<xsl:template match="OTA_ReadRQ">
	<MSG_VERSION>8</MSG_VERSION>
	<REC_LOC><xsl:value-of select="UniqueID/@ID"/></REC_LOC>
	<ETR_INF>Y</ETR_INF> 
  	<ALL_PNR_INF>Y</ALL_PNR_INF> 
  	<PRC_INF>Y</PRC_INF>
</xsl:template>

</xsl:stylesheet>