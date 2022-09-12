<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_HotelAvailRQ.xsl 														-->
	<!-- ================================================================== -->
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
					<PoweredHotel_SingleAvailability>
						<scrollingInformation>
							<displayRequest>050</displayRequest>
							<maxNumberItems>30</maxNumberItems>
							<nextItemReference></nextItemReference>
						</scrollingInformation>
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="single" />
					</PoweredHotel_SingleAvailability>
					<PoweredHotel_StructuredPricing>
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="pricing" />
					</PoweredHotel_StructuredPricing>
				</PricingDetails>
			</xsl:when>
			<xsl:when test="AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef/@HotelCode != ''">
				<PoweredHotel_SingleAvailability>
					<scrollingInformation>
						<displayRequest>050</displayRequest>
						<maxNumberItems>
							<xsl:choose>
								<xsl:when test="@MaxResponses!=''">
									<xsl:choose>
										<xsl:when test="@MaxResponses &lt; 31"><xsl:value-of select="@MaxResponses"/></xsl:when>
										<xsl:otherwise>30</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>30</xsl:otherwise>
							</xsl:choose>
						</maxNumberItems>
					</scrollingInformation>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="single" />
				</PoweredHotel_SingleAvailability>
			</xsl:when>
			<xsl:otherwise>
				<PoweredHotel_AvailabilityMultiProperties>
					<scrollingInformation>
						<displayRequest>050</displayRequest>
						<maxNumberItems>
							<xsl:choose>
								<xsl:when test="@MaxResponses!=''">
									<xsl:choose>
										<xsl:when test="@MaxResponses &lt; 101"><xsl:value-of select="@MaxResponses"/></xsl:when>
										<xsl:otherwise>100</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>30</xsl:otherwise>
							</xsl:choose>
						</maxNumberItems>
						<nextItemReference></nextItemReference>
					</scrollingInformation>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="multi" />
				</PoweredHotel_AvailabilityMultiProperties>
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
			<bookingCode><xsl:value-of select="RatePlanCandidates/RatePlanCandidate/@RatePlanID"/></bookingCode>
		</roomInformation>
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
			<xsl:if test="//@RequestedCurrency !=' ' or RateRange/@MaxRate !='' or RateRange/@MinRate !=''">
				<rateDetails>
					<xsl:if test="//@RequestedCurrency !=''">
						<currency>
							<xsl:value-of select="@RequestedCurrency" />
						</currency>
					</xsl:if>
					<rangeMax>
						<xsl:value-of select="RateRange/@MaxRate" />
					</rangeMax>
					<rangeMin>
						<xsl:value-of select="RateRange/@MinRate" />
					</rangeMin>
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
			<xsl:if test="//@RequestedCurrency !=' ' or @MaxRate !='' or @MinRate !=''">
				<rateDetails>
					<xsl:if test="//@RequestedCurrency !=''">
						<currency>
							<xsl:value-of select="@RequestedCurrency" />
						</currency>
					</xsl:if>
					<rangeMax>
						<xsl:value-of select="@MaxRate" />
					</rangeMax>
					<rangeMin>
						<xsl:value-of select="@MinRate" />
					</rangeMin>
				</rateDetails>
			</xsl:if>
			<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="RatePlanCode" />
		</hotelPropertySelection>
		<hotelLocationPrefInfo>
			<locationSelection>
				<cityCode>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
				</cityCode>
			</locationSelection>
			<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="ChainCode" />
			<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelName != ''">
				<hotelNameSelection>
					<nameSearch>
						<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelName" />
					</nameSearch>
				</hotelNameSelection>
			</xsl:if>
		</hotelLocationPrefInfo>
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
</xsl:stylesheet>
