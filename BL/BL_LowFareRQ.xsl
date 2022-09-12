<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- BL_LowFareRQ.xsl 																            -->
	<!-- ================================================================== -->
	<!-- Date: 02 Oct 2012 - Rastko - added support for LowOfferMatrix and LowOfferSearch	  	-->
	<!-- Date: 21 May 2012 - Rastko - added Air option S for FlightSite				-->
	<!-- Date: 18 Jul 2011 - Rastko	- added support for LowFareMatrix					-->
	<!-- Date: 18 May 2009 - Rastko													            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="bl" select="OTA_AirLowFareSearchRQ/ProviderBL/Preferences | OTA_AirLowFareSearchPlusRQ/ProviderBL/Preferences | OTA_AirLowFareSearchScheduleRQ/ProviderBL/Preferences | OTA_AirLowFareSearchMatrixRQ/ProviderBL/Preferences | OTA_AirLowOfferMatrixRQ/ProviderBL/Preferences | OTA_AirLowOfferSearchRQ/ProviderBL/Preferences"/>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRQ">
				<OTA_AirLowFareSearchRQ>
					<xsl:apply-templates select="OTA_AirLowFareSearchRQ"/>
				</OTA_AirLowFareSearchRQ>
			</xsl:when>
			<xsl:when test="OTA_AirLowFareSearchScheduleRQ">
				<OTA_AirLowFareSearchScheduleRQ>
					<xsl:apply-templates select="OTA_AirLowFareSearchScheduleRQ"/>
				</OTA_AirLowFareSearchScheduleRQ>
			</xsl:when>
			<xsl:when test="OTA_AirLowFareSearchMatrixRQ">
				<OTA_AirLowFareSearchMatrixRQ>
					<xsl:apply-templates select="OTA_AirLowFareSearchMatrixRQ"/>
				</OTA_AirLowFareSearchMatrixRQ>
			</xsl:when>
			<xsl:when test="OTA_AirLowOfferMatrixRQ">
				<OTA_AirLowOfferMatrixRQ>
					<xsl:apply-templates select="OTA_AirLowOfferMatrixRQ"/>
				</OTA_AirLowOfferMatrixRQ>
			</xsl:when>
			<xsl:when test="OTA_AirLowOfferSearchRQ">
				<OTA_AirLowOfferSearchRQ>
					<xsl:apply-templates select="OTA_AirLowOfferSearchRQ"/>
				</OTA_AirLowOfferSearchRQ>
			</xsl:when>
			<xsl:otherwise>
				<OTA_AirLowFareSearchPlusRQ>
					<xsl:if test="OTA_AirLowFareSearchPlusRQ/@MaxResponses != ''">
						<xsl:attribute name="MaxResponses">
							<xsl:value-of select="OTA_AirLowFareSearchPlusRQ/@MaxResponses"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ"/>
				</OTA_AirLowFareSearchPlusRQ>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchRQ | OTA_AirLowFareSearchPlusRQ | OTA_AirLowFareSearchScheduleRQ | OTA_AirLowFareSearchMatrixRQ | OTA_AirLowOfferMatrixRQ | OTA_AirLowOfferSearchRQ">
		<xsl:variable name="msg" select="name()"/>
		<xsl:copy-of select="POS"/>
		<xsl:variable name="depDate">
			<xsl:value-of select="translate(substring(OriginDestinationInformation[1]/DepartureDateTime,1,10),'-','')"/>
		</xsl:variable>
		<xsl:variable name="origin">
			<xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode"/>
		</xsl:variable>
		<xsl:variable name="arrival">
			<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/>
		</xsl:variable>
		<xsl:variable name="air" select="$bl/Airline[@StartDate &lt;= $depDate and @EndDate >= $depDate][Route/@Depart='***' or Route/@Depart=$origin][Route/@Arrival='***' or Route/@Arrival=$arrival]"/>
		<xsl:copy-of select="OriginDestinationInformation"/>
		<TravelPreferences>
			<xsl:choose>
				<xsl:when test="$air/@Code != '' and TravelPreferences/VendorPref">
					<xsl:choose>
						<xsl:when test="$air[@Level='I']">
							<xsl:for-each select="$air[@Level='I']">
								<VendorPref>
									<xsl:attribute name="Code"><xsl:value-of select="@Code"/></xsl:attribute>
								</VendorPref>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="TravelPreferences/VendorPref[not(@PreferLevel = 'Unacceptable')] and $air[@Level='E']">
							<xsl:copy-of select="TravelPreferences/VendorPref[not(@PreferLevel = 'Unacceptable')]"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select="$air/@Code">
								<VendorPref>
									<xsl:attribute name="Code"><xsl:value-of select="."/></xsl:attribute>
									<xsl:if test="../@Level = 'E'">
										<xsl:attribute name="PreferLevel">Unacceptable</xsl:attribute>
									</xsl:if>
								</VendorPref>
							</xsl:for-each>
							<xsl:copy-of select="TravelPreferences/VendorPref"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$air/@Code != ''">
					<xsl:for-each select="$air/@Code">
						<VendorPref>
							<xsl:attribute name="Code"><xsl:value-of select="."/></xsl:attribute>
							<xsl:choose>
								<xsl:when test="../@Level = 'E' or ($msg='OTA_AirLowFareSearchPlusRQ' and ../@Level = 'S')">
									<xsl:attribute name="PreferLevel">Unacceptable</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="PreferLevel">Only</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</VendorPref>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="TravelPreferences/VendorPref"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:copy-of select="TravelPreferences/FlightTypePref"/>
			<xsl:copy-of select="TravelPreferences/FareRestrictPref"/>
			<xsl:copy-of select="TravelPreferences/CabinPref"/>
		</TravelPreferences>
		<TravelerInfoSummary>
			<xsl:copy-of select="TravelerInfoSummary/SeatsRequested"/>
			<xsl:copy-of select="TravelerInfoSummary/AirTravelerAvail"/>
			<xsl:choose>
				<xsl:when test="$bl/PrivateFare/@Code != '' and (TravelerInfoSummary/PriceRequestInformation/@PricingSource = 'Both' or TravelerInfoSummary/PriceRequestInformation/@PricingSource = 'Private')">
					<PriceRequestInformation>
						<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@NegotiatedFaresOnly != ''">
							<xsl:attribute name="NegotiatedFaresOnly"><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@NegotiatedFaresOnly"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="PricingSource"><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@PricingSource"/></xsl:attribute>
						<NegotiatedFareCode>
							<xsl:attribute name="Code"><xsl:value-of select="$bl/PrivateFare/@Code"/></xsl:attribute>
						</NegotiatedFareCode>
					</PriceRequestInformation>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="TravelerInfoSummary/PriceRequestInformation"/>
				</xsl:otherwise>
			</xsl:choose>
		</TravelerInfoSummary>
	</xsl:template>
</xsl:stylesheet>
