<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- TXML_GetDealsRS.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 31 Aug 2010 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<TXML_GetDealsRS Version="1.0">
			<Success /> 
			<Deals>
				<xsl:apply-templates select="TXML_GetDealsRS/Deals/Deal" />
			</Deals>
		</TXML_GetDealsRS >
	</xsl:template>
	<xsl:template match="Deal">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="dd" select="OriginDestinationInformation/DepartureDate"/>
		<xsl:variable name="rd" select="OriginDestinationInformation/ReturnDate"/>
		<xsl:variable name="ol" select="OriginDestinationInformation/OriginLocation/@LocationCode"/>
		<xsl:variable name="dl" select="OriginDestinationInformation/DestinationLocation/@LocationCode"/>
		<xsl:variable name="ma" select="OriginDestinationInformation/MarketingAirline/@Code"/>
		<xsl:variable name="ft" select="FareInfo/@FareType"/>
		<xsl:variable name="tot" select="FareInfo/TotalAmount"/>
		<xsl:variable name="mkp" select="FareInfo/IncludedMarkup"/>
		<xsl:if test="$pos=1 or preceding-sibling::Deal[1][OriginDestinationInformation/DepartureDate!=$dd or OriginDestinationInformation/ReturnDate!=$rd or OriginDestinationInformation/OriginLocation/@LocationCode!=$ol or OriginDestinationInformation/DestinationLocation/@LocationCode!=$dl or OriginDestinationInformation/MarketingAirline/@Code!=$ma or FareInfo/@FareType!=$ft or FareInfo/TotalAmount!=$tot or FareInfo/IncludedMarkup!=$mkp]">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
