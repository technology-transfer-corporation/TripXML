<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_FareDisplayRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 14 Feb 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<FarePlus_DisplayFaresForCityPair_Query>
			<xsl:apply-templates select="OTA_AirFareDisplayRQ" />
		</FarePlus_DisplayFaresForCityPair_Query>
	</xsl:template>
	
	<xsl:template match="OTA_AirFareDisplayRQ">
		<OriginCity>
			<xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode" />
		</OriginCity>
		<DestinationCity>
			<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode" />
		</DestinationCity>
		<xsl:if test="TravelPreferences/VendorPref/@Code!=''">
			<Airline1>
				<xsl:value-of select="TravelPreferences/VendorPref[1]/@Code" />
			</Airline1>
			<xsl:if test="TravelPreferences/VendorPref[position()=2]/@Code!=''">
				<Airline2>
					<xsl:value-of select="TravelPreferences/VendorPref[position()=2]/@Code" />
				</Airline2>
			</xsl:if>
			<xsl:if test="TravelPreferences/VendorPref[position()=3]/@Code!=''">
				<Airline3>
					<xsl:value-of select="TravelPreferences/VendorPref[position()=3]/@Code" />
				</Airline3>
			</xsl:if>
		</xsl:if>
		<CommonFares>F</CommonFares>
		<DateOutbound>
			<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,9,2)" />
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
			<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,1,4)" />
		</DateOutbound>
		<xsl:if test="OriginDestinationInformation[2]">
			<DateInbound>
				<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,9,2)" />
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
				<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,1,4)" />
			</DateInbound>
			<DateType>E</DateType>
		</xsl:if>
		<FareType1>
			<xsl:choose>
				<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[1]/@Code!=''">
					<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[1]/@Code"/>
				</xsl:when>
				<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[1]/@Code"/></xsl:otherwise>
			</xsl:choose>
		</FareType1>
		<xsl:if test="TravelerInfoSummary/PassengerTypeQuantity[position()=2]/@Code!='' or TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=2]/@Code!=''">
			<FareType2>
				<xsl:choose>
					<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=2]/@Code!=''">
						<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[2]/@Code"/>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[2]/@Code"/></xsl:otherwise>
				</xsl:choose>
			</FareType2>
		</xsl:if>
		<xsl:if test="TravelerInfoSummary/PassengerTypeQuantity[position()=3]/@Code!='' or TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=3]/@Code!=''">
			<FareType3>
				<xsl:choose>
					<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=3]/@Code!=''">
						<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[3]/@Code"/>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[3]/@Code"/></xsl:otherwise>
				</xsl:choose>
			</FareType3>
		</xsl:if>
		<xsl:if test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode/@Code!=''">
			<CorporateContractNumber>
				<xsl:for-each select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode">
					<xsl:if test="position() > 1">-</xsl:if>
					<xsl:value-of select="@Code"/>
				</xsl:for-each>
			</CorporateContractNumber>
		</xsl:if>
		<xsl:if test="TravelPreferences/CabinPref/@Cabin!=''">
			<CabinPosition>
				<xsl:choose>
					<xsl:when test="TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
					<xsl:when test="TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
					<xsl:otherwise>Y</xsl:otherwise>
				</xsl:choose>
			</CabinPosition>
		</xsl:if>
		<xsl:if test="TravelPreferences/FareTypePref/@FareType='Private' or TravelPreferences/FareTypePref/@FareType='Both' or TravelPreferences/FareTypePref/@FareType='CorporateCode'">
		 	<xsl:choose>
				<xsl:when test="TravelPreferences/FareTypePref/@FareType='CorporateCode'">
					<CAPI_TicketingIndicatorsPlus> 
						<PricingTicketingIndicator>RW</PricingTicketingIndicator>
					</CAPI_TicketingIndicatorsPlus>
				</xsl:when>
				<xsl:when test="TravelPreferences/FareTypePref/@FareType='Both'">
					<CAPI_TicketingIndicatorsPlus> 
						<PricingTicketingIndicator>RU</PricingTicketingIndicator>
					</CAPI_TicketingIndicatorsPlus>
					<CAPI_TicketingIndicatorsPlus> 
						<PricingTicketingIndicator>RP</PricingTicketingIndicator>
					</CAPI_TicketingIndicatorsPlus>
				</xsl:when>
				<xsl:otherwise>
					<CAPI_TicketingIndicatorsPlus> 
						<PricingTicketingIndicator>RU</PricingTicketingIndicator>
					</CAPI_TicketingIndicatorsPlus>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="month">
		<xsl:param name="month" />
		<xsl:choose>
			<xsl:when test="$month = '01'">JAN</xsl:when>
			<xsl:when test="$month = '02'">FEB</xsl:when>
			<xsl:when test="$month = '03'">MAR</xsl:when>
			<xsl:when test="$month = '04'">APR</xsl:when>
			<xsl:when test="$month = '05'">MAY</xsl:when>
			<xsl:when test="$month = '06'">JUN</xsl:when>
			<xsl:when test="$month = '07'">JUL</xsl:when>
			<xsl:when test="$month = '08'">AUG</xsl:when>
			<xsl:when test="$month = '09'">SEP</xsl:when>
			<xsl:when test="$month = '10'">OCT</xsl:when>
			<xsl:when test="$month = '11'">NOV</xsl:when>
			<xsl:when test="$month = '12'">DEC</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>