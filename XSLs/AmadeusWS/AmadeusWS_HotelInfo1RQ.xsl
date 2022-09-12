<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_HotelInfo1RQ.xsl 													-->
<!-- ================================================================== -->
<!-- Date: 19 Jun 2014 - Rastko - get rate info for all rates							-->
<!-- Date: 14 Apr 2014 - Rastko - added mapping of OTA_HotelRs response		-->
<!-- Date: 19 Oct 2013 - Rastko - call HotelDescriptiveInfo with WithParsing in all cases	-->
<!-- Date: 13 Dec 2012 - Rastko - added mapping for requested currency 			-->
<!-- Date: 13 Oct 2012 - Rastko - added mapping for StructuredPricing			-->
<!-- Date: 08 Aug 2012 - Rastko - new implementation								-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<HtlMed>
			<xsl:apply-templates select="Hotel_AvailabilityMultiPropertiesReply" />
			<xsl:apply-templates select="Hotel_SingleAvailabilityReply" />
			<xsl:apply-templates select="OTA_HotelAvailRS" />
		</HtlMed>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="Hotel_SingleAvailabilityReply">
		<OTA_HotelDescriptiveInfoRQ EchoToken="WithParsing" PrimaryLangID="en" Version="6.001">
			<HotelDescriptiveInfos>
				<xsl:apply-templates select="singleAvailabilityDetails/propertyAvailability/hotelProductInfo/propertyHeaderDetails"/>
			</HotelDescriptiveInfos>
		</OTA_HotelDescriptiveInfoRQ>
		<xsl:for-each select="singleAvailabilityDetails/propertyAvailability/rateInformation">
			<Hotel_StructuredPricing>
				<hotelPropertyInfo>
					<hotelReference>
						<chainCode><xsl:value-of select="../hotelProductInfo/propertyHeaderDetails/chainCode"/></chainCode>
						<cityCode><xsl:value-of select="../hotelProductInfo/propertyHeaderDetails/cityCode"/></cityCode>
						<hotelCode><xsl:value-of select="../hotelProductInfo/propertyHeaderDetails/propertyCode"/></hotelCode>
					</hotelReference>
				</hotelPropertyInfo>
				<bookingPeriod>
					<businessSemantic>CHK</businessSemantic>
					<beginDateTime>
						<year>
							<xsl:value-of select="'20'"/>
							<xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkInDate,5)"/>
						</year>
						<month><xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkInDate,3,2)"/></month>
						<day><xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkInDate,1,2)"/></day>
					</beginDateTime>
					<endDateTime>
						<year>
							<xsl:value-of select="'20'"/>
							<xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkOutDate,5)"/>
						</year>
						<month><xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkOutDate,3,2)"/></month>
						<day><xsl:value-of select="substring(../hotelProductInfo/propertyHeaderDetails/checkOutDate,1,2)"/></day>
					</endDateTime>
				</bookingPeriod>
				<roomInformation>
					<bookingCode><xsl:value-of select="rateReferenceDetails/providerBookingCode"/></bookingCode>
				</roomInformation>
				<xsl:if test="../hotelProductInfo/otherHotelInformation/currencyRequested!=''">
					<foreignCurrencyInformation>
						<conversionRateDetails>
							<conversionType>ALL</conversionType>
							<currency><xsl:value-of select="../hotelProductInfo/otherHotelInformation/currencyRequested"/></currency>
						</conversionRateDetails>
				    </foreignCurrencyInformation>
			   </xsl:if>
			</Hotel_StructuredPricing>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="Hotel_AvailabilityMultiPropertiesReply">
		<OTA_HotelDescriptiveInfoRQ EchoToken="WithParsing" PrimaryLangID="en" Version="6.001">
			<HotelDescriptiveInfos>
				<xsl:apply-templates select="propertyAvailabilityList/hotelProductInfo/propertyHeaderDetails"/>
			</HotelDescriptiveInfos>
		</OTA_HotelDescriptiveInfoRQ>
	</xsl:template>
	
	<xsl:template match="OTA_HotelAvailRS">
		<OTA_HotelDescriptiveInfoRQ EchoToken="WithParsing" PrimaryLangID="en" Version="6.001">
			<HotelDescriptiveInfos>
				<xsl:apply-templates select="HotelStays/HotelStay"/>
			</HotelDescriptiveInfos>
		</OTA_HotelDescriptiveInfoRQ>
	</xsl:template>
	
	<xsl:template match="propertyHeaderDetails">
		<HotelDescriptiveInfo>
			<xsl:attribute name="HotelCode"><xsl:value-of select="concat(chainCode,cityCode,propertyCode)"/></xsl:attribute>
			<HotelInfo SendData="true"/>
			<FacilityInfo SendGuestRooms="true" SendMeetingRooms="true" SendRestaurants="true"/>
			<Policies SendPolicies="true"/>
			<AreaInfo SendAttractions="true" SendRecreations="true" SendRefPoints="true"/>
			<AffiliationInfo SendAwards="true" SendLoyalPrograms="false"/>
			<ContactInfo SendData="true"/>
			<MultimediaObjects SendData="true"/>
			<ContentInfos>
				<ContentInfo Name="SecureMultimediaURLs"/>
			</ContentInfos>
		</HotelDescriptiveInfo>	
	</xsl:template>
	
	<xsl:template match="HotelStay">
		<HotelDescriptiveInfo>
			<xsl:attribute name="HotelCode"><xsl:value-of select="BasicPropertyInfo/@HotelCode"/></xsl:attribute>
			<HotelInfo SendData="true"/>
			<FacilityInfo SendGuestRooms="true" SendMeetingRooms="true" SendRestaurants="true"/>
			<Policies SendPolicies="true"/>
			<AreaInfo SendAttractions="true" SendRecreations="true" SendRefPoints="true"/>
			<AffiliationInfo SendAwards="true" SendLoyalPrograms="false"/>
			<ContactInfo SendData="true"/>
			<MultimediaObjects SendData="true"/>
			<ContentInfos>
				<ContentInfo Name="SecureMultimediaURLs"/>
			</ContentInfos>
		</HotelDescriptiveInfo>	
	</xsl:template>

</xsl:stylesheet>