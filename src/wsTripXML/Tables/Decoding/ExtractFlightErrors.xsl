<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<Flights>
		<xsl:apply-templates select="Bookings/OTA_TravelItineraryRS"/>
	</Flights>
</xsl:template>
<xsl:template match="OTA_TravelItineraryRS">
	<xsl:for-each select="Errors/Error">
		<Flight>
			<xsl:value-of select="substring(substring-after(.,'Flight '),1,2)"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="substring-before(substring(substring-after(.,'Flight '),3),' ')"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="substring(substring-after(.,'Class '),1,1)"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="substring(substring-after(.,'Date '),1,10)"/>
		</Flight>
	</xsl:for-each>
</xsl:template>
</xsl:stylesheet>