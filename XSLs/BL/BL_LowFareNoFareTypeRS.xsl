<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- BL_LowFareNoFareTypeRS.xsl 												                   -->
<!-- ================================================================== -->
<!-- Date: 02 Oct 2012 - Rastko - added support for LowOfferMatrix and LowOfferSearch	       -->
<!-- Date: 18 Jul 2011 - Rastko	- added support for LowFareMatrix					       -->
<!-- Date: 13 Apr 2010 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="am" select="OTA_AirLowFareSearchRS/NoFareType | OTA_AirLowFareSearchPlusRS/NoFareType | OTA_AirLowFareSearchMatrixRS/NoFareType | OTA_AirLowOfferMatrixRS/ProviderBL/NoFareType | OTA_AirLowOfferSearchRS/ProviderBL/NoFareType"/>
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
		<xsl:choose>
			<xsl:when test="$am/Fare/@Type!=''">
				<xsl:variable name="ft"><xsl:value-of select="$am/Fare/@Type"/></xsl:variable>
				<xsl:choose>
					<xsl:when test="AirItineraryPricingInfo/@PricingSource = $ft">
						<xsl:variable name="ods" select="AirItinerary/OriginDestinationOptions"/>
						<xsl:variable name="airsearch">
							<xsl:for-each select="$am/Fare[@Type=$ft]/Airline">
								<xsl:variable name="air">
									<xsl:value-of select="@Code"/>
								</xsl:variable>
								<xsl:variable name="airc">
									<xsl:value-of select="count($ods/OriginDestinationOption/FlightSegment[MarketingAirline/@Code = $air])"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test="$airc = 0">0</xsl:when>
									<xsl:otherwise>1</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="contains($airsearch,'1')"></xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise><xsl:copy-of select="."/></xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$am/Airline/FB!=''">
				<xsl:variable name="ods" select="AirItinerary/OriginDestinationOptions"/>
				<xsl:variable name="airsearch">
					<xsl:for-each select="$am/Airline">
						<xsl:variable name="air">
							<xsl:value-of select="@Code"/>
						</xsl:variable>
						<xsl:variable name="airc">
							<xsl:value-of select="count($ods/OriginDestinationOption/FlightSegment[MarketingAirline/@Code = $air])"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$airc = 0">0</xsl:when>
							<xsl:otherwise>1</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($airsearch,'1')">
						<xsl:variable name="ptc" select="AirItineraryPricingInfo/PTC_FareBreakdowns"/>
						<xsl:variable name="fbsearch">
							<xsl:for-each select="$am/Airline">
								<xsl:for-each select="FB">
									<xsl:variable name="fb">
										<xsl:value-of select="."/>
									</xsl:variable>
									<xsl:variable name="fbc">
										<xsl:value-of select="count($ptc/PTC_FareBreakdown/FareBasisCodes/FareBasisCode[. = $fb])"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$fbc = 0">0</xsl:when>
										<xsl:otherwise>1</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="contains($fbsearch,'1')"></xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>
