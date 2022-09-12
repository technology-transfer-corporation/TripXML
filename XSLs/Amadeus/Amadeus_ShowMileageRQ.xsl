<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_ShowMileageRQ" />
	</xsl:template>
	<xsl:template match="OTA_ShowMileageRQ">
		<Cryptic_GetScreen_Query>
			<Command>
				<xsl:text>FQM</xsl:text>
				<xsl:value-of select="FromCity" />
				<xsl:apply-templates select="ToCity" />
			</Command>
		</Cryptic_GetScreen_Query>
	</xsl:template>
	<xsl:template match="ToCity">
		<xsl:value-of select="." />
	</xsl:template>
</xsl:stylesheet>
