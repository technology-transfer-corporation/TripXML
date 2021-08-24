<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TTWSCities>
		<xsl:apply-templates select="TTWSCities/Cities">
			<xsl:sort order="ascending" data-type="text" select="City"/>
		</xsl:apply-templates>
	</TTWSCities>
</xsl:template>

<xsl:template match="Cities">
	<CityAirport><xsl:value-of select="City"/><xsl:value-of select="Airport"/></CityAirport> 
</xsl:template>  

</xsl:stylesheet>