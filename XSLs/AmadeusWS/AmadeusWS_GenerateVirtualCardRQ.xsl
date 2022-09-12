<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_GenerateVirtualCardRQ.xsl									-->
<!-- ================================================================== -->
<!-- Date: 16 Jan 2019 - Rastko - new file											-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PAY_GenerateVirtualCardRQ"/>
	</xsl:template>
	<xsl:template match="PAY_GenerateVirtualCardRQ">
    <AMA_PAY_GenerateVirtualCardRQ Version="2.0">
		  <xsl:if test="VirtualCard">
		    <xsl:copy-of select="VirtualCard"/>
		  </xsl:if>
		  <xsl:if test="ReportingInfo">
		    <xsl:copy-of select="ReportingInfo"/>
		  </xsl:if>
    </AMA_PAY_GenerateVirtualCardRQ>
	</xsl:template>
</xsl:stylesheet>
