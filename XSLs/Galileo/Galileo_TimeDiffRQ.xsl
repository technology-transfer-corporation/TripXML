<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_TimeDiffRQ" />
	</xsl:template>
	<xsl:template match="OTA_TimeDiffRQ">
		<LocalDateTimeCT_6_0>
			<LocalDateTimeMods>
				<ReqCity>
					<xsl:value-of select="LocalInfo/CityCode" />
				</ReqCity>
			</LocalDateTimeMods>
		</LocalDateTimeCT_6_0>
	</xsl:template>
</xsl:stylesheet>
