<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_FareDisplayRQ.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 20 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<Fare_DisplayFaresForCityPair>
			<xsl:apply-templates select="OTA_AirFareDisplayRQ" />
		</Fare_DisplayFaresForCityPair>
	</xsl:template>
	
	<xsl:template match="OTA_AirFareDisplayRQ">
		<msgType>
		     <messageFunctionDetails>
		     	<messageFunction>711</messageFunction>
		     </messageFunctionDetails> 
		</msgType>
		<xsl:if test="TravelPreferences/CabinPref/@Cabin!=''">
			<availCabinConf>
				<productDetailsQualifier>SC</productDetailsQualifier>
				<bookingClassDetails>
					<designator>
						<xsl:choose>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
							<xsl:otherwise>Y</xsl:otherwise>
						</xsl:choose>
					</designator>
				</bookingClassDetails>
			</availCabinConf>
		</xsl:if>
		<xsl:if test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode/@Code!=''">
			<multiCorporate>
				<xsl:for-each select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode">
					<corporateId>
						<corporateQualifier>RU</corporateQualifier>
						<identity><xsl:value-of select="@Code"/></identity>
					</corporateId>
				</xsl:for-each>
			</multiCorporate>
		</xsl:if>
		<pricingTickInfo>
			<priceTicketDetails>
				<xsl:choose>
					<xsl:when test="RuleReqInfo/SubSection[@SubTitle='CorporateCode']">
						<indicators>RW</indicators>
					</xsl:when>
					<xsl:when test="TravelPreferences/FareTypePref/@FareType='Private'">
						<indicators>RU</indicators>
					</xsl:when>
					<xsl:when test="TravelPreferences/FareTypePref/@FareType='Both'">
						<indicators>RU</indicators>
						<indicators>RP</indicators>
					</xsl:when>
					<xsl:otherwise>
						<indicators>RP</indicators>
					</xsl:otherwise>
				</xsl:choose>
			</priceTicketDetails>
		</pricingTickInfo>
		<flightQualification>
			<fareCategories>
				<fareType1>
					<xsl:choose>
						<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[1]/@Code!=''">
							<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[1]/@Code"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[1]/@Code"/></xsl:otherwise>
					</xsl:choose>
				</fareType1>
				<xsl:if test="TravelerInfoSummary/PassengerTypeQuantity[position()=2]/@Code!='' or TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=2]/@Code!=''">
					<fareType2>
						<xsl:choose>
							<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=2]/@Code!=''">
								<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[2]/@Code"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[2]/@Code"/></xsl:otherwise>
						</xsl:choose>
					</fareType2>
				</xsl:if>
				<xsl:if test="TravelerInfoSummary/PassengerTypeQuantity[position()=3]/@Code!='' or TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=3]/@Code!=''">
					<fareType3>
						<xsl:choose>
							<xsl:when test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[position()=3]/@Code!=''">
								<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode[3]/@Code"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="TravelerInfoSummary/PassengerTypeQuantity[3]/@Code"/></xsl:otherwise>
						</xsl:choose>
					</fareType3>
				</xsl:if>
			</fareCategories>
		</flightQualification>
		<xsl:if test="TravelPreferences/VendorPref/@Code!=''">
			<transportInformation>
		  		<transportService>
		  			<companyIdentification>
		  				<marketingCompany><xsl:value-of select="TravelPreferences/VendorPref[1]/@Code"/></marketingCompany>
		  			</companyIdentification> 
		  		</transportService>
			</transportInformation> 
		</xsl:if>
		<tripDescription>
			<origDest>
			     <origin>
			     	<xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode" />
			     </origin>
			     <destination>
			     	<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode" />
			     </destination> 
			</origDest>
			<dateFlightMovement>
				<dateAndTimeDetails>
					<qualifier>O</qualifier>
					<date>
						<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,9,2)" />
						<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,6,2)" />
						<xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,3,2)" />
					</date>
				</dateAndTimeDetails>
				<xsl:if test="OriginDestinationInformation[2]">
					<dateAndTimeDetails>
						<qualifier>I</qualifier>
						<date>
							<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,9,2)" />
							<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,6,2)" />
							<xsl:value-of select="substring(OriginDestinationInformation[2]/DepartureDateTime,3,2)" />
						</date>
					</dateAndTimeDetails>
				</xsl:if>
			</dateFlightMovement>
		</tripDescription>
	</xsl:template>
</xsl:stylesheet>