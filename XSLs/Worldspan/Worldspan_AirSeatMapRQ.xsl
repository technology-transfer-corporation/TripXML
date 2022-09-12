<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_AirSeatMapRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 08 Mar 2009 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirSeatMapRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_AirSeatMapRQ">
		<OTA_AirSeatMapRQ Version="1.001">
			<xsl:copy-of select="SeatMapRequests"/>
		</OTA_AirSeatMapRQ>
	</xsl:template>
</xsl:stylesheet>