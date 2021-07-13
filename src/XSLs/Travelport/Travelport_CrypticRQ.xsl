<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CrypticRQ" />
	</xsl:template>
	<xsl:template match="CrypticRQ">
		<xsl:value-of select="Entry" />
	</xsl:template>
</xsl:stylesheet>
