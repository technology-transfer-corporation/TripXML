<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_CancelVirtualCardLoadRQ.xsl									-->
<!-- ================================================================== -->
<!-- Date: 16 Jan 2019 - Rastko - new file											-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="CancelVirtualCardLoadRQ"/>
	</xsl:template>
	<xsl:template match="CancelVirtualCardLoadRQ">
		<PAY_CancelVirtualCardLoadRQ>
			<xsl:copy-of select="."/>
		</PAY_CancelVirtualCardLoadRQ>
	</xsl:template>
</xsl:stylesheet>
