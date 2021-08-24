<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TTWSAirports>
		<xsl:variable name="airports">
			<xsl:apply-templates select="TTWSAirports/Airport" mode="thomalex">
				<xsl:sort order="ascending" data-type="text" select="Code"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="TTWSAirports/Airports/Airport" mode="jetcombo">
				<xsl:sort order="ascending" data-type="text" select="@iata"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:apply-templates select="msxsl:node-set($airports)/Airport" mode="all">
			<xsl:sort order="ascending" data-type="text" select="Code"/>
		</xsl:apply-templates>
	</TTWSAirports>
</xsl:template>

<xsl:template match="Airport" mode="thomalex">
	<Airport>
		<Code><xsl:value-of select="Code"/></Code>
		<Name>
			<xsl:value-of select="Name"/>
		</Name>
		<xsl:copy-of select="TimeZone"/>
		<xsl:variable name="code"><xsl:value-of select="Code"/></xsl:variable>
		<xsl:if test="../Airports/Airport[@iata=$code]">
			<Other>
				<xsl:attribute name="iso2"><xsl:value-of select="../Airports/Airport[@iata=$code]/@iso2"/></xsl:attribute>
				<xsl:attribute name="iso3"><xsl:value-of select="../Airports/Airport[@iata=$code]/@iso3"/></xsl:attribute>
				<xsl:attribute name="StateCode">
					<xsl:if test="contains(substring-after(Name,','),',')">
						<xsl:value-of select="substring-before(substring-after(Name,', '),',')"/>
					</xsl:if>
				</xsl:attribute>
				<xsl:attribute name="Country"><xsl:value-of select="../Airports/Airport[@iata=$code]/@Country"/></xsl:attribute>
				<xsl:attribute name="GeoZone"><xsl:value-of select="../Airports/Airport[@iata=$code]/@GeoZone"/></xsl:attribute>
				<xsl:attribute name="Latitude"><xsl:value-of select="../Airports/Airport[@iata=$code]/@Latitude"/></xsl:attribute>
				<xsl:attribute name="Longitude"><xsl:value-of select="../Airports/Airport[@iata=$code]/@Longitude"/></xsl:attribute>
			</Other>
		</xsl:if>
	</Airport>
</xsl:template>  

<xsl:template match="Airport" mode="jetcombo">
	<xsl:variable name="code"><xsl:value-of select="@iata"/></xsl:variable>
	<xsl:choose>
		<xsl:when test="../../Airport[Code=$code]"></xsl:when>
		<xsl:otherwise>
			<Airport>
				<Code><xsl:value-of select="@iata"/></Code>
				<Name><xsl:value-of select="@Name"/></Name>
				<TimeZone>
					<StandardTime>0</StandardTime>
				</TimeZone>
				<Other>
					<xsl:attribute name="iso2"><xsl:value-of select="@iso2"/></xsl:attribute>
					<xsl:attribute name="iso3"><xsl:value-of select="@iso3"/></xsl:attribute>
					<xsl:attribute name="StateCode"><xsl:value-of select="@StateCode"/></xsl:attribute>
					<xsl:attribute name="Country"><xsl:value-of select="@Country"/></xsl:attribute>
					<xsl:attribute name="GeoZone"><xsl:value-of select="@GeoZone"/></xsl:attribute>
					<xsl:attribute name="Latitude"><xsl:value-of select="@Latitude"/></xsl:attribute>
					<xsl:attribute name="Longitude"><xsl:value-of select="@Longitude"/></xsl:attribute>
				</Other>
			</Airport>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="Airport" mode="all">
	<xsl:copy-of select="."/>
</xsl:template>

</xsl:stylesheet>