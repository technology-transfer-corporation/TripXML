<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_LowFarePlusRS2.xsl 														-->
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
		<PricedItinerary>
			<xsl:apply-templates select="AirItinerary"/>
			<xsl:copy-of select="AirItineraryPricingInfo"/>
			<xsl:copy-of select="TicketingInfo"/>
		</PricedItinerary>
	</xsl:template>
	
	<xsl:template match="AirItinerary">
		<AirItinerary>
			<xsl:attribute name="DirectionInd"><xsl:value-of select="@DirectionInd"/></xsl:attribute>
			<xsl:apply-templates select="OriginDestinationOptions"/>
		</AirItinerary>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOptions">
		<OriginDestinationOptions>
			<xsl:variable name="ods" select="."/>
			<xsl:variable name="nn"></xsl:variable>
			<xsl:call-template name="sortfl">
				<xsl:with-param name="currod">1</xsl:with-param>
				<xsl:with-param name="totod"><xsl:value-of select="count(OriginDestinationOption) + 1"/></xsl:with-param>
				<xsl:with-param name="ods" select="$ods"/>
				<xsl:with-param name="newod" select="$nn"/>
			</xsl:call-template>
		</OriginDestinationOptions>
	</xsl:template>
	
	<xsl:template name="sortfl">
		<xsl:param name="currod"/>
		<xsl:param name="totod"/>
		<xsl:param name="ods"/>
		<xsl:param name="newod"/>
		<xsl:choose>
			<xsl:when test="$currod &lt; $totod">
				<xsl:for-each select="$ods/OriginDestinationOption[position()=$currod]/Flight">
					<xsl:variable name="newod1">
						<xsl:if test="$newod != ''">
							<xsl:copy-of select="$newod"/>
						</xsl:if>
						<xsl:copy-of select="."/>
					</xsl:variable>
					<xsl:call-template name="sortfl">
						<xsl:with-param name="currod"><xsl:value-of select="$currod + 1"/></xsl:with-param>
						<xsl:with-param name="totod"><xsl:value-of select="$totod"/></xsl:with-param>
						<xsl:with-param name="ods" select="$ods"/>
						<xsl:with-param name="newod">
							<xsl:copy-of select="$newod1"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<NewOD>
					<xsl:copy-of select="$newod"/>
				</NewOD>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
			
</xsl:stylesheet>
