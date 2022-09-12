<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- BL_LowFareNoCountryRS.xsl 													                   -->
<!-- ================================================================== -->
<!-- Date: 02 Oct 2012 - Rastko - added support for LowOfferMatrix and LowOfferSearch	       -->
<!-- Date: 16 Dec 2011 - Rastko	- new file											       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="am" select="OTA_AirLowFareSearchRS/NoCountry | OTA_AirLowFareSearchPlusRS/NoCountry | OTA_AirLowFareSearchMatrixRS/NoCountry | OTA_AirLowOfferMatrixRS/ProviderBL/NoCountry | OTA_AirLowOfferSearchRS/ProviderBL/NoCountry"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRS | OTA_AirLowFareSearchPlusRS | OTA_AirLowFareSearchMatrixRS | OTA_AirLowOfferMatrixRS | OTA_AirLowOfferSearchRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchRS">
		<OTA_AirLowFareSearchRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:if test="SearchPromotionsResponse">
				<xsl:copy-of select="SearchPromotionsResponse"/>
			</xsl:if>
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
			<xsl:if test="SearchPromotionsResponse">
				<xsl:copy-of select="SearchPromotionsResponse"/>
			</xsl:if>
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
			<xsl:if test="SearchPromotionsResponse">
				<xsl:copy-of select="SearchPromotionsResponse"/>
			</xsl:if>
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
		<xsl:variable name="countrysearch">
			<xsl:for-each select="$am/Country">
				<xsl:variable name="country"><xsl:value-of select="@Code"/></xsl:variable>
				<xsl:variable name="from"><xsl:value-of select="@From"/></xsl:variable>
				<xsl:variable name="to"><xsl:value-of select="@To"/></xsl:variable>
					<xsl:variable name="dep">
						<xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport"/>
					</xsl:variable>
					<xsl:variable name="depCountry">
						<xsl:value-of select="substring($dep,string-length($dep) - 1)"/>
					</xsl:variable>
					<xsl:variable name="arr">
						<xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport"/>
					</xsl:variable>
					<xsl:variable name="arrCountry">
						<xsl:value-of select="substring($arr,string-length($arr) - 1)"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$country = $depCountry and $from='Y'">1</xsl:when>
						<xsl:when test="$country = $arrCountry and $to='Y'">1</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($countrysearch,'1')"></xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>
