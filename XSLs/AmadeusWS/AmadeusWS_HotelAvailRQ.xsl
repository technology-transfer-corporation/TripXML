<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_HotelAvailRQ.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date : 24 Mar 2014 - Rastko - correct currency mapping in hotel pricing part		 -->
	<!-- Date : 17 Mar 2014 - Rastko - added currency mapping in hotel pricing part			 -->
	<!-- Date : 09 Jan 2013 - Rastko - added additional search parameters				 -->
	<!-- Date : 13 Dec 2012 - Rastko - corrected mapping of currency conversion			 -->
	<!-- Date : 19 Jul 2012 - Shashin - change for Hotel_AvailabilityMultiProperties request using rate plan -->
	<!-- Date: 07 Jul 2009 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRQ" />
	</xsl:template>
	<xsl:template match="OTA_HotelAvailRQ">
		<xsl:choose>
			<xsl:when test="AvailRequestSegments/AvailRequestSegment/@AvailReqType = 'PricingDetails'">
				<PricingDetails>
					<Hotel_SingleAvailability>
						<scrollingInformation>
							<displayRequest>050</displayRequest>
							<maxNumberItems>32</maxNumberItems>
							<nextItemReference></nextItemReference>
						</scrollingInformation>
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="single" />
					</Hotel_SingleAvailability>
					<Hotel_StructuredPricing>
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="pricing" />
					</Hotel_StructuredPricing>
				</PricingDetails>
			</xsl:when>
			<xsl:when test="AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef/@HotelCode != ''">
				<Hotel_SingleAvailability>
					<scrollingInformation>
						<displayRequest>050</displayRequest>
						<maxNumberItems>
							<xsl:choose>
								<xsl:when test="@MaxResponses!=''">
									<xsl:value-of select="@MaxResponses"/>
								</xsl:when>
								<xsl:otherwise>30</xsl:otherwise>
							</xsl:choose>
						</maxNumberItems>
						<nextItemReference></nextItemReference>
					</scrollingInformation>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="single" />
				</Hotel_SingleAvailability>
			</xsl:when>
			<xsl:otherwise>
				<Hotel_AvailabilityMultiProperties>
					<scrollingInformation>
						<displayRequest>050</displayRequest>
						<maxNumberItems>
							<xsl:choose>
								<xsl:when test="@MaxResponses!=''">
									<xsl:value-of select="@MaxResponses"/>
								</xsl:when>
								<xsl:otherwise>30</xsl:otherwise>
							</xsl:choose>
						</maxNumberItems>
						<nextItemReference></nextItemReference>
					</scrollingInformation>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="multi" />
				</Hotel_AvailabilityMultiProperties>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="AvailRequestSegment" mode="pricing">
		<hotelPropertyInfo>
			<hotelReference>
				<chainCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
				</chainCode>
				<cityCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
				</cityCode>
				<hotelCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode" />
				</hotelCode>
			</hotelReference>
		</hotelPropertyInfo>
		<bookingPeriod>
			<businessSemantic>CHK</businessSemantic>
			<beginDateTime>
				<year><xsl:value-of select="substring(string(StayDateRange/@Start),1,4)" /></year>
				<month><xsl:value-of select="substring(string(StayDateRange/@Start),6,2)" /></month>
				<day><xsl:value-of select="substring(string(StayDateRange/@Start),9,2)" /></day>
			</beginDateTime>
			<endDateTime>
				<year><xsl:value-of select="substring(string(StayDateRange/@End),1,4)" /></year>
				<month><xsl:value-of select="substring(string(StayDateRange/@End),6,2)" /></month>
				<day><xsl:value-of select="substring(string(StayDateRange/@End),9,2)" /></day>
			</endDateTime>
		</bookingPeriod>
		<roomInformation>
			<bookingCode>
				<xsl:value-of select="RatePlanCandidates/RatePlanCandidate/@RatePlanID"/>
			</bookingCode>
		</roomInformation>
		<xsl:if test="//@RequestedCurrency !=' ' or ../../POS/Source/@ISOCurrency!=''">
			<foreignCurrencyInformation>
				<conversionRateDetails>
					<conversionType>ALL</conversionType>
					<xsl:choose>
						<xsl:when test="../../POS/Source/@ISOCurrency!=''">
							<currency>
								<xsl:value-of select="../../POS/Source/@ISOCurrency" />
							</currency>
						</xsl:when>
						<xsl:when test="//@RequestedCurrency !=''">
							<currency>
								<xsl:value-of select="@RequestedCurrency" />
							</currency>
						</xsl:when>
					</xsl:choose>
				</conversionRateDetails>
			</foreignCurrencyInformation>
		</xsl:if>
	</xsl:template>

	<xsl:template match="AvailRequestSegment" mode="single">
		<!--Note: Couldn't find any amenity codes on Amadeus to map OTA Amenity code to -->
		<hotelPropertySelection>
			<propertyInformation>
				<chainCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
				</chainCode>
				<cityCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
				</cityCode>
				<propertyCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode" />
				</propertyCode>
			</propertyInformation>
			<checkInDate>
				<xsl:value-of select="substring(string(StayDateRange/@Start),9,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@Start),6,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@Start),3,2)" />
			</checkInDate>
			<checkOutDate>
				<xsl:value-of select="substring(string(StayDateRange/@End),9,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@End),6,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@End),3,2)" />
			</checkOutDate>
			<roomDetails>
				<availabilityStatus>A</availabilityStatus>
				<xsl:if test="RoomStayCandidates/RoomStayCandidate/GuestCounts/GuestCount/@Count != ''">
					<occupancy>
						<xsl:value-of select="RoomStayCandidates/RoomStayCandidate/GuestCounts/GuestCount/@Count"/>
					</occupancy>
				</xsl:if>
				<xsl:if test="RoomStayCandidates/RoomStayCandidate/@RoomType !=''">
					<roomType>
						<xsl:value-of select="RoomStayCandidates/RoomStayCandidate/@RoomType" />
					</roomType>
				</xsl:if>
			</roomDetails>
			<xsl:if test="//@RequestedCurrency !=' ' or RateRange/@MaxRate !='' or RateRange/@MinRate !='' or ../../POS/Source/@ISOCurrency!=''">
				<rateDetails>
					<xsl:choose>
						<xsl:when test="../../POS/Source/@ISOCurrency!=''">
							<currency>
								<xsl:value-of select="../../POS/Source/@ISOCurrency" />
							</currency>
						</xsl:when>
						<xsl:when test="//@RequestedCurrency !=''">
							<currency>
								<xsl:value-of select="@RequestedCurrency" />
							</currency>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="RateRange/@MaxRate !=''">
						<rangeMax>
							<xsl:value-of select="RateRange/@MaxRate" />
						</rangeMax>
					</xsl:if>
					<xsl:if test="RateRange/@MinRate !=''">
						<rangeMin>
							<xsl:value-of select="RateRange/@MinRate" />
						</rangeMin>
					</xsl:if>
				</rateDetails>
			</xsl:if>
			<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="RatePlanCode" />
		</hotelPropertySelection>
	</xsl:template>
	<xsl:template match="AvailRequestSegment" mode="multi">
		<!--Note: Couldn't find any amenity codes on Amadeus to map OTA Amenity code to -->
		<hotelPropertySelection>
			<checkInDate>
				<xsl:value-of select="substring(string(StayDateRange/@Start),9,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@Start),6,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@Start),3,2)" />
			</checkInDate>
			<checkOutDate>
				<xsl:value-of select="substring(string(StayDateRange/@End),9,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@End),6,2)" />
				<xsl:value-of select="substring(string(StayDateRange/@End),3,2)" />
			</checkOutDate>
			<roomDetails>
				<availabilityStatus>A</availabilityStatus>
				<xsl:if test="RoomStayCandidates/RoomStayCandidate/GuestCounts/GuestCount/@Count != ''">
					<occupancy>
						<xsl:value-of select="RoomStayCandidates/RoomStayCandidate/GuestCounts/GuestCount/@Count"/>
					</occupancy>
				</xsl:if>
			</roomDetails>
			<xsl:if test="//@RequestedCurrency !=' ' or @MaxRate !='' or @MinRate !='' or ../../POS/Source/@ISOCurrency!=''">
				<rateDetails>
					<xsl:choose>
						<xsl:when test="../../POS/Source/@ISOCurrency!=''">
							<currency>
								<xsl:value-of select="../../POS/Source/@ISOCurrency" />
							</currency>
						</xsl:when>
						<xsl:when test="//@RequestedCurrency !=''">
							<currency>
								<xsl:value-of select="@RequestedCurrency" />
							</currency>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="@MaxRate !=''">
						<rangeMax>
							<xsl:value-of select="@MaxRate" />
						</rangeMax>
					</xsl:if>
					<xsl:if test="@MinRate !=''">
						<rangeMin>
							<xsl:value-of select="@MinRate" />
						</rangeMin>
					</xsl:if>
				</rateDetails>
			</xsl:if>
			<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="multi" />
		</hotelPropertySelection>
		<hotelLocationPrefInfo>
			<locationSelection>
				<xsl:if test="HotelSearchCriteria/Criterion/Address/CountryName/@Code !=''">
					<countryCode>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Address/CountryName/@Code" />
					</countryCode>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Address/StateProv/@StateCode !=''">
					<stateCode>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Address/StateProv/@StateCode" />
					</stateCode>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode !=''">
					<cityCode>
						<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
					</cityCode>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Address/CityName !=''">
					<cityName>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Address/CityName" />
					</cityName>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Address/AddressLine !=''">
					<streetAddress>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Address/AddressLine" />
					</streetAddress>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Telephone/@PhoneNumber !=''">
					<phoneNumber>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Telephone/@PhoneNumber" />
					</phoneNumber>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Address/PostalCode !=''">
					<zipCode>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Address/PostalCode" />
					</zipCode>
				</xsl:if>
				<xsl:if test="HotelSearchCriteria/Criterion/Radius/@Direction !=''">
					<areaCode>
						<xsl:value-of select="HotelSearchCriteria/Criterion/Radius/@Direction" />
					</areaCode>
				</xsl:if>
			</locationSelection>
			<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="ChainCode" />
			<xsl:if test="HotelSearchCriteria/Criterion/HotelAmenity">
				<facilitySelection>
					<xsl:for-each select="HotelSearchCriteria/Criterion/HotelAmenity">
						<facilityCode><xsl:value-of select="."/></facilityCode>
					</xsl:for-each>
				</facilitySelection> 
			</xsl:if>
			<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelName != ''">
				<hotelNameSelection>
					<nameSearch>
						<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelName" />
					</nameSearch>
				</hotelNameSelection>
			</xsl:if>
		</hotelLocationPrefInfo>
		<xsl:if test="HotelSearchCriteria/Criterion/RefPoint  != '' or HotelSearchCriteria/Criterion/HotelCityCode != '' or HotelSearchCriteria/Criterion/Position/@Longitude != '' or HotelSearchCriteria/Criterion/Position/@Latitude != '' or HotelSearchCriteria/Criterion/Radius/@Distance != ''">
			<porInfo>
				<hotelRefPointInfo>
					<xsl:if test="HotelSearchCriteria/Criterion/RefPoint">
						<porIdentification>
							<porName>
								<xsl:value-of select="HotelSearchCriteria/Criterion/RefPoint" />
							</porName>
						</porIdentification>
					</xsl:if>
					<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode">
						<cityIATACode>
							<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
						</cityIATACode>
					</xsl:if>
					<xsl:if test="HotelSearchCriteria/Criterion/Position">
						<porGeocode>
							<porLongitude>
								<xsl:value-of select="HotelSearchCriteria/Criterion/Position/@Longitude" />
							</porLongitude>
							<porLatitude>
								<xsl:value-of select="HotelSearchCriteria/Criterion/Position/@Latitude" />
							</porLatitude>
						</porGeocode>
					</xsl:if>
					<xsl:if test="HotelSearchCriteria/Criterion/Radius/@Distance">
						<radiusSearch>
							<xsl:value-of select="HotelSearchCriteria/Criterion/Radius/@Distance" />
						</radiusSearch>
						<distanceDecimalPoint>0</distanceDecimalPoint>
						<measureUnit>
							<xsl:choose>
								<xsl:when test="HotelSearchCriteria/Criterion/Radius/@DistanceMeasure = 'M'">M</xsl:when>
								<xsl:when test="HotelSearchCriteria/Criterion/Radius/@DistanceMeasure = 'K'">G</xsl:when>
							</xsl:choose>
						</measureUnit>
					</xsl:if>
				</hotelRefPointInfo>
			</porInfo>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Criterion" mode="ChainCode">
		<providerSelection>
			<chainCode>
				<xsl:value-of select="HotelRef/@ChainCode" />
			</chainCode>
		</providerSelection>
	</xsl:template>
	<xsl:template match="RatePlanCandidate" mode="RatePlanCode">
		<xsl:if test="@RatePlanCode != ''">
			<negotiatedRateSelection>
				<rateCode>
					<xsl:value-of select="@RatePlanCode" />
				</rateCode>
			</negotiatedRateSelection>
		</xsl:if>
	</xsl:template>
	<xsl:template match="RatePlanCandidate" mode="multi">
		<xsl:if test="@RatePlanCode != ''">
			<negotiatedRateSelection>
				<rateCodeRequested>
					<xsl:value-of select="@RatePlanCode" />
				</rateCodeRequested>
			</negotiatedRateSelection>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
