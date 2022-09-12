<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_HotelSearchRS.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 08 Jan 2013 - Rastko - corrected error handling							-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_HotelSearchRS>
			<xsl:attribute name="Version">3.14</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Hotel_ListReply/messageErrorList and not(Hotel_ListReply/propertyList)">
					<Errors>
						<xsl:apply-templates select="Hotel_ListReply/messageErrorList" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="Hotel_ListReply" />
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelSearchRS>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="Hotel_ListReply">
		<Success />
		<Properties>
			<xsl:apply-templates select="propertyList" />
		</Properties>
	</xsl:template>
	<!--    **************************************************************   -->
	<xsl:template match="propertyList">
		<Property>
			<xsl:attribute name="ChainCode">
				<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelCode">
				<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelCityCode">
				<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/cityCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelName">
				<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyName" />
			</xsl:attribute>
			<xsl:attribute name="HotelCodeContext">
				<xsl:choose>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'A'">Airport</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'D'">Downtown</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'E'">East</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'N'">North</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'R'">Resort</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'S'">South</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/areaLocationCode = 'W'">West</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ChainName">
				<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainName" />
			</xsl:attribute>
			<xsl:attribute name="BrandName">
				<xsl:choose>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CA'">Complete Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CP'">Complete Access Plus</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'DY'">Dynamic Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'IA'">Independent Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'SA'">Standard Access</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:if test="hotelProductInfo/pointOfReferenceDetails/porLatitude != ''">
				<Position>
					<xsl:attribute name="Latitude">
						<xsl:value-of select="hotelProductInfo/pointOfReferenceDetails/porLatitude" />
					</xsl:attribute>
					<xsl:attribute name="Longitude">
						<xsl:value-of select="hotelProductInfo/pointOfReferenceDetails/porLongitude" />
					</xsl:attribute>
				</Position>
			</xsl:if>
			<xsl:if test="hotelProductInfo/propertyHeaderDetails/stateCode !='' or hotelProductInfo/propertyHeaderDetails/countryCode !=''">
				<Address>
					<xsl:if test="hotelProductInfo/propertyHeaderDetails/stateCode !=''">
						<StateProv>
							<xsl:attribute name="StateCode">
								<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/stateCode" />
							</xsl:attribute>
						</StateProv>
					</xsl:if>
					<CountryName>
						<xsl:attribute name="Code">
							<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/countryCode" />
						</xsl:attribute>
					</CountryName>
				</Address>
			</xsl:if>
			<!--Note;  haven't been able to get a HotelSearchRQ to work yet with a Point of Reference code - once I do, come back to this section to see how it needs to be changed -->
			<xsl:if test="hotelProductInfo/pointOfReferenceDetails/distance != '' or  hotelProductInfo/pointOfReferenceDetails/direction != ''">
				<RelativePosition>
					<xsl:if test="hotelProductInfo/pointOfReferenceDetails/direction != ''">
						<xsl:attribute name="Direction">
							<xsl:value-of select="hotelProductInfo/pointOfReferenceDetails/direction" />
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="hotelProductInfo/pointOfReferenceDetails/distance != ''">
						<xsl:attribute name="Distance">
							<xsl:value-of select="hotelProductInfo/pointOfReferenceDetails/distance" />
						</xsl:attribute>
					</xsl:if>
				</RelativePosition>
			</xsl:if>
		</Property>
	</xsl:template>
	<!--    **************************************************************   -->
	<xsl:template match="messageErrorList">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="messageErrorInformation/errorDetails>/errorCode" />
			</xsl:attribute>
			<xsl:value-of select="errorFreeText/freetext" />
		</Error>
	</xsl:template>
	<!--    **************************************************************   -->
</xsl:stylesheet>
