<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_HotelSearchRQ.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 08 Jan 2013 - Rastko - added correct mapping for area codes				-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelSearchRQ" />
	</xsl:template>
	<xsl:template match="OTA_HotelSearchRQ">
		<Hotel_List>
			<scrollingInformation>
				<displayRequest>050</displayRequest>
			</scrollingInformation>
			<xsl:apply-templates select="Criteria/Criterion" />
		</Hotel_List>
	</xsl:template>
	<xsl:template match="Criterion">
		<hotelLocationPrefInfo>
			<xsl:if test="Address != '' or HotelRef/@HotelCityCode != '' or Telephone/@PhoneNumber != '' or  Telephone/@AreaCityCode  != '' or Radius/@Direction != ''">
				<locationSelection>
					<xsl:if test="Address/CountryName/@Code !=''">
						<countryCode>
							<xsl:value-of select="Address/CountryName/@Code" />
						</countryCode>
					</xsl:if>
					<xsl:if test="Address/StateProv/@StateCode !=''">
						<stateCode>
							<xsl:value-of select="Address/StateProv/@StateCode" />
						</stateCode>
					</xsl:if>
					<xsl:if test="HotelRef/@HotelCityCode !=''">
						<cityCode>
							<xsl:value-of select="HotelRef/@HotelCityCode" />
						</cityCode>
					</xsl:if>
					<!--alternateCitiesOption></alternateCitiesOption-->
					<xsl:if test="Address/CityName !=''">
						<cityName>
							<xsl:value-of select="Address/CityName" />
						</cityName>
					</xsl:if>
					<xsl:if test="Address/AddressLine !=''">
						<streetAddress>
							<xsl:value-of select="Address/AddressLine" />
						</streetAddress>
					</xsl:if>
					<xsl:if test="Telephone/@PhoneNumber !=''">
						<phoneNumber>
							<xsl:value-of select="Telephone/@PhoneNumber" />
						</phoneNumber>
					</xsl:if>
					<xsl:if test="Address/PostalCode !=''">
						<zipCode>
							<xsl:value-of select="Address/PostalCode" />
						</zipCode>
					</xsl:if>
					<xsl:if test="Radius/@Direction !=''">
						<areaCode>
							<xsl:value-of select="Radius/@Direction" />
						</areaCode>
					</xsl:if>
				</locationSelection>
			</xsl:if>
			<xsl:if test="HotelRef/@ChainCode != ''">
				<providerSelection>
					<chainCode>
						<xsl:value-of select="HotelRef/@ChainCode" />
					</chainCode>
				</providerSelection>
			</xsl:if>
			<xsl:if test="Award/@Rating != ''">
				<categorySelection>
					<ratingCode>
						<xsl:value-of select="Award/@Rating" />
					</ratingCode>
				</categorySelection>
			</xsl:if>
			<xsl:if test="HotelRef/@HotelName != ''">
				<hotelNameSelection>
					<nameSearch>
						<xsl:value-of select="HotelRef/@HotelName" />
					</nameSearch>
				</hotelNameSelection>
			</xsl:if>
		</hotelLocationPrefInfo>
		<xsl:if test="RefPoint  != '' or HotelCityCode != '' or Position/@Longitude != '' or Position/@Latitude != '' or Radius/@Distance != ''">
			<porInfo>
				<hotelRefPointInfo>
					<xsl:if test="RefPoint">
						<porIdentification>
							<porName>
								<xsl:value-of select="RefPoint" />
							</porName>
						</porIdentification>
					</xsl:if>
					<xsl:if test="HotelRef/@HotelCityCode">
						<cityIATACode>
							<xsl:value-of select="HotelRef/@HotelCityCode" />
						</cityIATACode>
					</xsl:if>
					<xsl:if test="Position">
						<porGeocode>
							<porLongitude>
								<xsl:value-of select="Position/@Longitude" />
							</porLongitude>
							<porLatitude>
								<xsl:value-of select="Position/@Latitude" />
							</porLatitude>
						</porGeocode>
					</xsl:if>
					<xsl:if test="Radius/@Distance">
						<radiusSearch>
							<xsl:value-of select="Radius/@Distance" />
						</radiusSearch>
						<distanceDecimalPoint>0</distanceDecimalPoint>
						<measureUnit>
							<xsl:choose>
								<xsl:when test="Radius/@DistanceMeasure = 'M'">M</xsl:when>
								<xsl:when test="Radius/@DistanceMeasure = 'K'">G</xsl:when>
							</xsl:choose>
						</measureUnit>
					</xsl:if>
				</hotelRefPointInfo>
			</porInfo>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
