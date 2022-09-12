<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Portal_SearchPromotionsRQ.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 05 Apr 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">		
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ"/>	
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OTA_AirLowFareSearchRQ | OTA_AirLowFareSearchPlusRQ">
		 <!--SearchPromotions xmlns="http://odyssey.com/webservices/"-->
		 <SearchPromotions>
			<promoSearch>
				<exclusiveOnly>false</exclusiveOnly>
				<PromotionID>0</PromotionID>
				<NoRulesRestrictions>false</NoRulesRestrictions>
				<ExclusiveOnly>false</ExclusiveOnly>
				<AdultCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</AdultCount>
				<ChildCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</ChildCount>
				<SeniorCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</SeniorCount>
				<InfantCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</InfantCount>
				<SeatedInfantCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</SeatedInfantCount>
				<PromotionTypeID>0</PromotionTypeID>
				<ProductTypeID>2</ProductTypeID>
				<Suppliers/>
				<Brands></Brands>
				<FareType></FareType>
				<ClassTypes>1</ClassTypes>
				<Destinations></Destinations>
        			<Countries></Countries>
        			<States></States>
        			<DepartureCities></DepartureCities>
				<DepartureStartDate>
					<xsl:value-of select="OriginDestinationInformation/DepartureDateTime"/>
				</DepartureStartDate>
				<DepartureEndDate>
					<xsl:choose>
						<xsl:when test="OriginDestinationInformation[position() > 1 and position()=last()]/DepartureDateTime!=''">
							<xsl:value-of select="OriginDestinationInformation[position()=last()]/DepartureDateTime"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="OriginDestinationInformation[1]/DepartureDateTime"/>
						</xsl:otherwise>
					</xsl:choose>
				</DepartureEndDate>
				<BookingDate>2009-01-23T00:00:00.0000000-05:00</BookingDate>
				<StartDate>2009-01-23T00:00:00.0000000-05:00</StartDate>
				<EndDate>2009-01-23T00:00:00.0000000-05:00</EndDate>
				<FromCity></FromCity>
				<ToCity></ToCity>
				<FromCountry></FromCountry>
				<ToCountry></ToCountry>
				<JourneyType></JourneyType>
				<OfficeID>
					<xsl:choose>
						<xsl:when test="POS/TPA_Extensions/Provider/Name[. != 'Portal']/@PseudoCityCode!=''">
							<xsl:for-each select="POS/TPA_Extensions/Provider/Name[. != 'Portal']">
								<string><xsl:value-of select="@PseudoCityCode"/></string>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="POS/Source/@PseudoCityCode"/></xsl:otherwise>
					</xsl:choose>	
				</OfficeID>
				<Affiliate><xsl:value-of select="POS/TPA_Extensions/Provider/Name[. = 'Portal']/@PseudoCityCode"/></Affiliate>
				<RoutingCities/>
				<ApplicationType></ApplicationType>
				<BookingClass/>
				<FlighNumbers/>
			</promoSearch>
			<languageId>1</languageId>
      		<currencyId>
      			<xsl:choose>
					<xsl:when test="POS/Source/@ISOCurrency!=''">
						<xsl:value-of select="POS/Source/@ISOCurrency"/>
					</xsl:when>
					<xsl:otherwise>USD</xsl:otherwise>
				</xsl:choose>
      		</currencyId>
      		<catalog>BBTCustomers</catalog>
		</SearchPromotions>
	</xsl:template>	
</xsl:stylesheet>
