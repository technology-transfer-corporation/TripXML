<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<a>
			<xsl:apply-templates select="a/Table/Airport"/>
		</a>
	</xsl:template>
	
	<xsl:template match="Airport">
		<xsl:variable name="code"><xsl:value-of select="Code"/></xsl:variable>
		<xsl:if test="not(../../TTWSAirports/Airport[Code=$code])">
			<Airport>
				<Code><xsl:value-of select="Code"/></Code>
				<Name><xsl:value-of select="concat(Name,', FR')"/></Name>
				<TimeZone>
					<StandardTime>1</StandardTime>
					<DaylightSaving StartDate="2004-03-28" EndDate="2004-10-31">2</DaylightSaving>
				</TimeZone>
			</Airport>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
