<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TTWSCities>
		<xsl:apply-templates select="TTWSCities/Cities[Airport != City]/Airport">
			<xsl:sort order="ascending" data-type="text" select="."/>
		</xsl:apply-templates>
	</TTWSCities>
</xsl:template>

<xsl:template match="Airport">
	<Cities>
		<Airport><xsl:value-of select="."/></Airport>
		<City><xsl:value-of select="../City"/></City>
	</Cities>
	<xsl:variable name="city"><xsl:value-of select="../City"/></xsl:variable>
	<xsl:variable name="airport"><xsl:value-of select="."/></xsl:variable>
	<xsl:apply-templates select="../../Cities[City = $city][Airport != $airport]">
		<xsl:with-param name="airport"><xsl:value-of select="$airport"/></xsl:with-param>
	</xsl:apply-templates>
</xsl:template>  

<xsl:template match="Cities">
	<xsl:param name="airport"/>
	<Cities>
		<Airport><xsl:value-of select="Airport"/></Airport>
		<City><xsl:value-of select="$airport"/></City>
	</Cities>
</xsl:template>  

</xsl:stylesheet>