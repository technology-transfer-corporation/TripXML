<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- BL_LowFareNoMixRS.xsl 														                   -->
<!-- ================================================================== -->
<!-- Date: 02 Oct 2012 - Rastko - added support for LowOfferMatrix and LowOfferSearch	       -->
<!-- Date: 18 Jul 2011 - Rastko	- added support for LowFareMatrix					       -->
<!-- Date: 08 Apr 2010 - Rastko - return error if all flights filtered out				       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="am" select="OTA_AirLowFareSearchRS/NoMixAirline | OTA_AirLowFareSearchPlusRS/NoMixAirline | OTA_AirLowFareSearchMatrixRS/NoMixAirline | OTA_AirLowOfferMatrixRS/ProviderBL/NoMixAirline | OTA_AirLowOfferSearchRS/ProviderBL/NoMixAirline"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRS | OTA_AirLowFareSearchPlusRS | OTA_AirLowFareSearchMatrixRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchRS">
		<OTA_AirLowFareSearchRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:variable name="PricedItinerary">
				<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="Errors">
					<xsl:copy-of select="Errors"/>
				</xsl:when>
				<xsl:when test="$PricedItinerary!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:copy-of select="$PricedItinerary"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error>
							<xsl:value-of select="'ALL FLIGHTS HAVE BEEN FILTERED OUT BY BUSINESS LOGIC'"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowFareSearchPlusRS">
		<OTA_AirLowFareSearchPlusRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:variable name="PricedItinerary">
				<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="Errors">
					<xsl:copy-of select="Errors"/>
				</xsl:when>
				<xsl:when test="$PricedItinerary!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:copy-of select="$PricedItinerary"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error>
							<xsl:value-of select="'ALL FLIGHTS HAVE BEEN FILTERED OUT BY BUSINESS LOGIC'"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchPlusRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowFareSearchMatrixRS">
		<OTA_AirLowFareSearchMatrixRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:variable name="PricedItinerary">
				<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="Errors">
					<xsl:copy-of select="Errors"/>
				</xsl:when>
				<xsl:when test="$PricedItinerary!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:copy-of select="$PricedItinerary"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error>
							<xsl:value-of select="'ALL FLIGHTS HAVE BEEN FILTERED OUT BY BUSINESS LOGIC'"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchMatrixRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowOfferMatrixRS">
		<OTA_AirLowOfferMatrixRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:variable name="PricedItinerary">
				<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="Errors">
					<xsl:copy-of select="Errors"/>
				</xsl:when>
				<xsl:when test="$PricedItinerary!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:copy-of select="$PricedItinerary"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error>
							<xsl:value-of select="'ALL FLIGHTS HAVE BEEN FILTERED OUT BY BUSINESS LOGIC'"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowOfferMatrixRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowOfferSearchRS">
		<OTA_AirLowOfferSearchRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:variable name="PricedItinerary">
				<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="Errors">
					<xsl:copy-of select="Errors"/>
				</xsl:when>
				<xsl:when test="$PricedItinerary!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:copy-of select="$PricedItinerary"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error>
							<xsl:value-of select="'ALL FLIGHTS HAVE BEEN FILTERED OUT BY BUSINESS LOGIC'"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowOfferSearchRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<xsl:variable name="ods" select="AirItinerary/OriginDestinationOptions"/>
		<xsl:variable name="allairc">
			<xsl:value-of select="count($ods/OriginDestinationOption/FlightSegment)"/>
		</xsl:variable>
		<xsl:variable name="airsearch">
			<xsl:for-each select="$am/Airline">
				<xsl:variable name="air"><xsl:value-of select="@Code"/></xsl:variable>
				<xsl:choose>
					<xsl:when test="$air='**'">
						<xsl:variable name="air1">
							<xsl:value-of select="$ods/OriginDestinationOption/FlightSegment[1]/MarketingAirline/@Code"/>
						</xsl:variable>
						<xsl:variable name="airc">
							<xsl:value-of select="count($ods/OriginDestinationOption/FlightSegment[MarketingAirline/@Code = $air1])"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$allairc != $airc">1</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="airc">
							<xsl:value-of select="count($ods/OriginDestinationOption/FlightSegment[MarketingAirline/@Code = $air])"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$airc = 0">0</xsl:when>
							<xsl:when test="$allairc != $airc">1</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($airsearch,'1')"></xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>
