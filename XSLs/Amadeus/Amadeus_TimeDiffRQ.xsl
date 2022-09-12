<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_TimeDiffRQ" />
	</xsl:template>
	<xsl:template match="OTA_TimeDiffRQ">
		<Cryptic_GetScreen_Query>
			<Command>DDMIA<xsl:value-of select="substring(translate(LocalInfo/Time,':',''),1,4)" />/<xsl:value-of select="LocalInfo/CityCode" /></Command>
		</Cryptic_GetScreen_Query>
	</xsl:template>
</xsl:stylesheet>
