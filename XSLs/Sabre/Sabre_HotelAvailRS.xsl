<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_HotelAvailRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_HotelAvailRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_HotelAvailRS">
		<OTA_HotelAvailRS>
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
				<!--Sometimes Sabre sends back an empty RoomStay tag if you send invalid RQ File -->
				<xsl:when test="not(RoomStays/RoomStay) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<!--This is for the OTA_RoomRates message -->
				<xsl:when test="RoomStays/RoomStay/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax  != ''">
					<Success></Success>
					<RoomStays>
						<RoomStay>
							<RoomRates>
								<xsl:apply-templates select="RoomStays/RoomStay/RoomRates/RoomRate" mode="RoomRate" />
							</RoomRates>
							<GuestCounts>
								<GuestCount>
									<xsl:attribute name="Count">
										<xsl:value-of select="RoomStays/RoomStay/GuestCounts/GuestCount/@Count" />
									</xsl:attribute>
								</GuestCount>
							</GuestCounts>
							<BasicPropertyInfo>
								<xsl:attribute name="ChainCode">
									<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@ChainCode" />
								</xsl:attribute>
								<xsl:attribute name="HotelCityCode">
									<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@HotelCityCode" />
								</xsl:attribute>
								<xsl:attribute name="HotelCode">
									<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@HotelCode" />
								</xsl:attribute>
								<xsl:attribute name="HotelName">
									<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@HotelName" />
								</xsl:attribute>
								<xsl:attribute name="ChainName"></xsl:attribute>
								<Address>
									<AddressLine>
										<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '1']" />
									</AddressLine>
									<AddressLine>
										<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/AddressLine[position() = '2']" />
									</AddressLine>
									<CountryName>
										<xsl:attribute name="Code">
											<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/BasicPropertyInfoAddlInfo/Address/CountryName/@Code" />
										</xsl:attribute>
									</CountryName>
								</Address>
								<ContactNumbers>
									<ContactNumber>
										<xsl:attribute name="PhoneNumber">
											<xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/BasicPropertyInfoAddlInfo/ContactNumbers/ContactNumber/@PhoneNumber" />
										</xsl:attribute>
									</ContactNumber>
								</ContactNumbers>
							</BasicPropertyInfo>
						</RoomStay>
					</RoomStays>
				</xsl:when>
				<!--This is for the OTA_HotelAvail message -->
				<xsl:otherwise>
					<Success></Success>
					<RoomStays>
						<xsl:attribute name="MoreIndicator">
							<xsl:value-of select="RoomStays/@MoreIndicator" />
						</xsl:attribute>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="HotelAvail" />
					</RoomStays>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomRate" mode="RoomRate">
		<xsl:variable name="NumDec">
			<xsl:value-of select="substring-after(Rates/Rate/Base/@AmountBeforeTax,'.')" />
		</xsl:variable>
		<xsl:variable name="CountNumDec">
			<xsl:value-of select="string-length($NumDec)" />
		</xsl:variable>
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
							<xsl:value-of select="translate(Rates/Rate/Base/@AmountBeforeTax,'.','')" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Rates/Rate/Base/@CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$CountNumDec" />
						</xsl:attribute>
					</Base>
					<RateDescription>
						<Text>
							<xsl:value-of select="RoomRateAddlInfo/RoomRateDescription/Text" />
						</Text>
					</RateDescription>
					<xsl:if test="TPA_Extensions/GDIndicator != ''">
						<TPA_Extensions>
							<GDIndicator>
								<xsl:value-of select="Rates/Rate/TPA_Extensions/GDIndicator" />
							</GDIndicator>
						</TPA_Extensions>
					</xsl:if>
				</Rate>
			</Rates>
		</RoomRate>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomStay" mode="HotelAvail">
		<RoomStay>
			<RatePlans>
				<RatePlan>
					<AdditionalDetails>
						<xsl:variable name="MinRate">
							<xsl:value-of select="translate(string(BasicPropertyInfo/BasicPropertyInfoAddlInfo/TPA_Extensions/MinRate),'.','')" />
						</xsl:variable>
						<!--xsl:variable name="NoDeci">
							<xsl:value-of select="string-length($Deci)"/>
						</xsl:variable-->
						<xsl:variable name="MaxRate">
							<xsl:value-of select="translate(string(BasicPropertyInfo/BasicPropertyInfoAddlInfo/TPA_Extensions/MaxRate),'.','')" />
						</xsl:variable>
						<AdditionalDetail>
							<xsl:attribute name="Code">MinimumRate</xsl:attribute>
							<xsl:attribute name="Amount">
								<xsl:value-of select="$MinRate" />
							</xsl:attribute>
						</AdditionalDetail>
						<AdditionalDetail>
							<xsl:attribute name="Code">MaximumRate</xsl:attribute>
							<xsl:attribute name="Amount">
								<xsl:value-of select="$MaxRate" />
							</xsl:attribute>
						</AdditionalDetail>
					</AdditionalDetails>
				</RatePlan>
			</RatePlans>
			<Total>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="BasicPropertyInfo/BasicPropertyInfoAddlInfo/TPA_Extensions/CurrencyCode" />
				</xsl:attribute>
				<xsl:variable name="Deci">
					<xsl:value-of select="substring-after(string(BasicPropertyInfo/BasicPropertyInfoAddlInfo/TPA_Extensions/MinRate),'.')" />
				</xsl:variable>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="string-length($Deci)" />
				</xsl:attribute>
			</Total>
			<BasicPropertyInfo>
				<xsl:attribute name="ChainCode">
					<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelCityCode">
					<xsl:value-of select="BasicPropertyInfo/@HotelCityCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelCode">
					<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelName">
					<xsl:value-of select="BasicPropertyInfo/@HotelName" />
				</xsl:attribute>
				<xsl:attribute name="ChainName"></xsl:attribute>
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
			</BasicPropertyInfo>
		</RoomStay>
	</xsl:template>
	<!--*************************************************************-->
</xsl:stylesheet>
