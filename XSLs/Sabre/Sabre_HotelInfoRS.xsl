<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_HotelDescriptiveInfoRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_HotelDescriptiveInfoRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_HotelAvailRS">
		<OTA_HotelDescriptiveInfoRS>
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
				<xsl:otherwise>
					<Success></Success>
					<HotelDescriptiveContents>
						<xsl:apply-templates select="RoomStays/RoomStay" />
					</HotelDescriptiveContents>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelDescriptiveInfoRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomRate" mode="RoomRate">
		<RoomRate>
			<xsl:attribute name="RoomTypeCode">
				<xsl:value-of select="RoomRateAddlInfo/@RoomTypeCode" />
			</xsl:attribute>
			<xsl:attribute name="RatePlanCode">
				<xsl:value-of select="RoomRateAddlInfo/@RatePlanCode" />
			</xsl:attribute>
			<Rates>
				<Rate>
					<Base>
						<xsl:attribute name="AmountBeforeTax">
							<xsl:value-of select="Rates/Rate/Base/@AmountBeforeTax" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Rates/Rate/Base/@CurrencyCode" />
						</xsl:attribute>
					</Base>
					<RateDescription>
						<Text>
							<xsl:value-of select="RoomRateAddlInfo/RoomRateDescription/Text" />
						</Text>
					</RateDescription>
					<TPA_Extensions>
						<GDIndicator>
							<xsl:value-of select="Rates/Rate/TPA_Extensions/GDIndicator" />
						</GDIndicator>
					</TPA_Extensions>
				</Rate>
			</Rates>
		</RoomRate>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomStay">
		<HotelDescriptiveContent LanguageCode="en-us">
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
			<HotelInfo>
				<xsl:if test="BasicPropertyInfo/BasicPropertyInfoAddlInfo/VendorMessages/VendorMessage/SubSection/Paragraph !=''">
					<Services>
						<Service>
							<Features>
								<xsl:apply-templates select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/VendorMessages/VendorMessage/SubSection"
									mode="CodeDetail" />
							</Features>
						</Service>
					</Services>
				</xsl:if>
			</HotelInfo>
			<ContactInfos>
				<ContactInfo>
					<Addresses>
						<Address>
							<AddressLine>
								<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '1']" />
							</AddressLine>
							<AddressLine>
								<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '2']" />
							</AddressLine>
							<CountryName>
								<xsl:attribute name="Code">
									<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/CountryName/@Code" />
								</xsl:attribute>
							</CountryName>
						</Address>
					</Addresses>
					<Phones>
						<Phone>
							<xsl:attribute name="PhoneNumber">
								<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/ContactNumbers/ContactNumber/@PhoneNumber" />
							</xsl:attribute>
							<xsl:attribute name="PhoneUseType">PHN</xsl:attribute>
						</Phone>
					</Phones>
				</ContactInfo>
			</ContactInfos>
		</HotelDescriptiveContent>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="SubSection" mode="CodeDetail">
		<Feature>
			<xsl:attribute name="CodeDetail">
				<xsl:choose>
					<xsl:when test="@SubTitle = 'Policies'">Policy</xsl:when>
					<xsl:when test="@SubTitle = 'Rooms'">Room Description</xsl:when>
					<xsl:when test="@SubTitle = 'PropertyTypes'">Hotel Category</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@SubTitle" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<Description>
				<!--xsl:when test="position()='1'">
						<xsl:apply-templates select="Paragraph/Text/following-sibling::Paragraph/Text" mode="NextLine"/>
					</xsl:when-->
				<xsl:apply-templates select="Paragraph/Text" mode="NextLine" />
			</Description>
		</Feature>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="Text" mode="NextLine">
		<Text>
			<xsl:value-of select="translate(.,'&#164;',' ')" />
		</Text>
	</xsl:template>
	<!--    **************************************************************   -->
</xsl:stylesheet>
