<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<TTWSAirports>
			<xsl:apply-templates select="TTWSAirports/Airport"/>
		</TTWSAirports>
	</xsl:template>
	
	<xsl:template match="Airport">
		<Airport>
			<Code><xsl:value-of select="Code"/></Code>
			<Name>
				<xsl:variable name="country" select="Other/@Country"/>
				<xsl:choose>
					<!-- Apalachicola, FL, US -->
					<xsl:when test="contains(substring-after(Name,','),',')">
						<xsl:variable name="state" select="substring(substring-after(Name,', '),1,4)"/>
						<xsl:value-of select="concat(substring-before(Name,','),', ',$state,$country)"/>
					</xsl:when>
					<xsl:otherwise>
					<!-- Al Arish, EG -->
						<xsl:value-of select="concat(substring-before(Name,','),', ',$country)"/>
					</xsl:otherwise>
				</xsl:choose>
			</Name>
			<xsl:copy-of select="TimeZone"/>
			<xsl:copy-of select="Other"/>
		</Airport>
	</xsl:template>
</xsl:stylesheet>
