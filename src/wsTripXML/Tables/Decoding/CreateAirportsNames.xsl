<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<TTWSAirports>
			<xsl:apply-templates select="AmadeusAirports/Code"/>
		</TTWSAirports>
	</xsl:template>
	
	<xsl:template match="Code">
		<xsl:variable name="country" select="city/@country"/>
		<Airport>
			<Code><xsl:value-of select="ref/@code"/></Code>
			<Name>
				<xsl:value-of select="city"/>
				<xsl:variable name="airport" select="ref"/>
				<xsl:if test="city!=$airport and $airport!=''">
					<xsl:value-of select="concat('-',$airport)"/>
				</xsl:if>
				<xsl:value-of select="', '"/>
				<xsl:if test="city/@state!=''">
					<xsl:value-of select="concat(city/@state,', ')"/>
				</xsl:if>
				<xsl:variable name="countryname" select="../TTWSCountries/Country[Code=$country]/Name"/>
				<xsl:call-template name="buildName">
					<xsl:with-param name="name"><xsl:value-of select="$countryname"/></xsl:with-param>
					<xsl:with-param name="finalname"></xsl:with-param>
				</xsl:call-template>
			</Name>
		</Airport>
	</xsl:template>
	
	<xsl:template name="buildName">
		<xsl:param name="name"/>
		<xsl:param name="words"/>
		<xsl:param name="finalname"/>
		<xsl:variable name="name1">
			<xsl:choose>
				<xsl:when test="contains($name,' ')">
					<xsl:value-of select="substring-before($name,' ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$name"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$name1 != ''">
				<xsl:variable name="name2">
					<xsl:value-of select="substring($name1,1,1)"/>
					<xsl:if test="string-length($name1) > 1">
						<xsl:value-of select="translate(substring($name1,2),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="name3">
					<xsl:choose>
						<xsl:when test="$finalname = ''">
							<xsl:value-of select="$name2"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($finalname,' ',$name2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:call-template name="buildName">
					<xsl:with-param name="name"><xsl:value-of select="substring-after($name,' ')"/></xsl:with-param>
					<xsl:with-param name="finalname"><xsl:value-of select="$name3"/></xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$finalname"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
