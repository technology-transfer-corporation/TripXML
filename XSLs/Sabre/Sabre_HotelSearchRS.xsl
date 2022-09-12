<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_HotelSearchRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_HotelSearchRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_HotelAvailRS">
		<OTA_HotelSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<!--Sometimes Sabre sends back an empty RoomStay tag if you send in invalid city code -->
				<xsl:when test="not(RoomStays/RoomStay) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<!--This is for the OTA_HotelAvail message -->
				<xsl:otherwise>
					<Success></Success>
					<Properties>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="HotelAvail" />
					</Properties>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelSearchRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomStay" mode="HotelAvail">
		<Property>
			<xsl:attribute name="ChainCode">
				<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelCode">
				<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelCityCode">
				<xsl:value-of select="BasicPropertyInfo/@HotelCityCode" />
			</xsl:attribute>
			<xsl:attribute name="HotelName">
				<xsl:value-of select="BasicPropertyInfo/@HotelName" />
			</xsl:attribute>
			<xsl:if test="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Latitude != '' or BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Longitude != ''">
				<Position>
					<xsl:if test="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Latitude != ''">
						<xsl:attribute name="Latitude">
							<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Latitude" />
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Longitude != ''">
						<xsl:attribute name="Longitude">
							<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Position/@Longitude" />
						</xsl:attribute>
					</xsl:if>
				</Position>
			</xsl:if>
			<Address>
				<AddressLine>
					<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '1']" />
				</AddressLine>
				<AddressLine>
					<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '2']" />
				</AddressLine>
			</Address>
			<ContactNumbers>
				<ContactNumber>
					<xsl:attribute name="PhoneNumber">
						<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/ContactNumbers/ContactNumber/@PhoneNumber" />
					</xsl:attribute>
				</ContactNumber>
			</ContactNumbers>
		</Property>
	</xsl:template>
	<!--*************************************************************-->
</xsl:stylesheet>
