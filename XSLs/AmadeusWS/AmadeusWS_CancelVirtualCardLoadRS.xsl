<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_CancelVirtualCardLoadRS.xsl									-->
<!-- ================================================================== -->
<!-- Date: 16 Jan 2019 - Rastko - new file											-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="CancelVirtualCardLoadRS"/>
	</xsl:template>
	<xsl:template match="CancelVirtualCardLoadRS">
		<PAY_CancelVirtualCardLoadRS>
			<xsl:copy-of select="."/>
		</PAY_CancelVirtualCardLoadRS>
	</xsl:template>
</xsl:stylesheet>
