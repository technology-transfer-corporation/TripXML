<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelSearchRQ" />
	</xsl:template>
	<xsl:template match="OTA_HotelSearchRQ">
		<PoweredHotel_List>
			<scrollingInformation>
				<displayRequest>050</displayRequest>
        <maxNumberItems>099</maxNumberItems>
			</scrollingInformation>
			<xsl:apply-templates select="Criteria/Criterion" />
		</PoweredHotel_List>
	</xsl:template>
	<xsl:template match="Criterion">
		<!--Note: Couldn't find any amenity codes on Amadeus to map OTA Amenity code to -->
		<hotelLocationPrefInfo>
			<xsl:if test="Address != ' ' or HotelRef/@HotelCityCode != ' ' or Telephone/@PhoneNumber != ' ' or  Telephone/@AreaCityCode  != ' ' ">
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
					<xsl:if test="Telephone/@AreaCityCode !=''">
						<areaCode>
							<xsl:value-of select="Telephone/@AreaCityCode" />
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
		<xsl:if test="RefPoint  != '' or HotelCityCode != '' or Position/@Longitude != '' or Position/@Latitude != '' or Radius/@Direction != '' or Radius/@Distance != ''">
			<hotelRefPointInfo>
				<porCategory></porCategory>
				<porIdentification>
					<porName>
						<xsl:value-of select="RefPoint" />
					</porName>
					<!--language></language-->
				</porIdentification>
				<cityIATACode>
					<xsl:value-of select="HotelRef/@HotelCityCode" />
				</cityIATACode>
				<porGeocode>
					<porLongitude>
						<xsl:value-of select="Position/@Longitude" />
					</porLongitude>
					<porLatitude>
						<xsl:value-of select="Position/@Latitude" />
					</porLatitude>
				</porGeocode>
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
			</hotelRefPointInfo>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
