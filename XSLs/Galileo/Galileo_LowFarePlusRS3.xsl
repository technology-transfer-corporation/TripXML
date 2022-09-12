<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_LowFarePlusRS3.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 15 Dec 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchPlusRS">
		<OTA_AirLowFareSearchPlusRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchPlusRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<xsl:variable name="nump">
			<xsl:value-of select="count(preceding-sibling::PricedItinerary/AirItinerary/OriginDestinationOptions/NewOD)"/>
		</xsl:variable>
		<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/NewOD">
			<xsl:with-param name="nump"><xsl:value-of select="$nump"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="NewOD">
		<xsl:param name="nump"/>
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="$nump + $pos" />
			</xsl:attribute>
			<AirItinerary>
				<xsl:attribute name="DirectionInd"><xsl:value-of select="../../../AirItinerary/@DirectionInd"/></xsl:attribute>
				<OriginDestinationOptions>
					<xsl:apply-templates select="Flight"/>
				</OriginDestinationOptions>
			</AirItinerary>
			<xsl:copy-of select="../../../AirItineraryPricingInfo"/>
			<xsl:copy-of select="../../../TicketingInfo"/>
		</PricedItinerary>
	</xsl:template>
	
	<xsl:template match="Flight">
		<OriginDestinationOption>
			<xsl:copy-of select="FlightSegment"/>
		</OriginDestinationOption>
	</xsl:template>
</xsl:stylesheet>






