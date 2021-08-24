<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TTWSAirports>
		<xsl:apply-templates select="TTWSAirports/Airport" mode="a">
			<xsl:sort order="ascending" data-type="text" select="Country"/>
		</xsl:apply-templates>
	</TTWSAirports>
</xsl:template>

<xsl:template match="Airport" mode="a">
	<xsl:copy-of select="."/> 
</xsl:template>

<xsl:template match="Airport">
	<Airport>
		<xsl:copy-of select="Code"/>
		<Name><xsl:value-of select="substring-before(Name,',')"/></Name>
		<xsl:variable name="country">
			<xsl:choose>
				<xsl:when test="contains(substring-after(Name,','),',')">
					<xsl:value-of select="substring-after(substring-after(Name,', '),', ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-after(Name,', ')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="contains(substring-after(Name,','),',')">
			<State>
				<xsl:value-of select="substring-before(substring-after(Name,', '),',')"/>
			</State>
		</xsl:if>
		<xsl:if test="string-length($country)>2 and contains($country,'- USA')">
			<State>
				<xsl:value-of select="substring-before($country,' ')"/>
			</State>
		</xsl:if>
		<xsl:if test="string-length($country)>2 and contains($country,' Canada')">
			<State>
				<xsl:value-of select="substring-before($country,' ')"/>
			</State>
		</xsl:if>
		<Country>
			<xsl:choose>
				<xsl:when test="string-length($country)>2">
					<xsl:choose>
						<xsl:when test="contains($country,'- USA')">U.S.A</xsl:when>
						<xsl:when test="contains($country,' Canada')">CANADA</xsl:when>
						<xsl:otherwise><xsl:value-of select="$country"/></xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="../TTWSCountries/Country[Code=$country]/Name"/>
				</xsl:otherwise>
			</xsl:choose>
		</Country>
	</Airport>
</xsl:template>  

</xsl:stylesheet>