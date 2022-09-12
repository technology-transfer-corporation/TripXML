<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Worldspan_CrypticRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 27 Jan 2105 - Rastko		- new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CrypticRQ" />
	</xsl:template>
	<xsl:template match="CrypticRQ">
		<OTA_ScreenTextRQ Version="1" xmlns="http://www.opentravel.org/OTA/2003/05">
			<ScreenEntry><xsl:value-of select="Entry" /></ScreenEntry>
		</OTA_ScreenTextRQ>
	</xsl:template>
</xsl:stylesheet>
