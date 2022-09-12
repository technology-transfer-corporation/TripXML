<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_HotelAvailRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 07 Jul 2009 - Rastko															       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="PoweredHotel_SingleAvailabilityReply">
				<xsl:apply-templates select="PoweredHotel_SingleAvailabilityReply" />
			</xsl:when>
			<xsl:when test="PoweredHotel_AvailabilityMultiPropertiesReply">
				<xsl:apply-templates select="PoweredHotel_AvailabilityMultiPropertiesReply" />
			</xsl:when>
			<xsl:when test="PoweredHotel_StructuredPricingReply">
				<xsl:apply-templates select="PoweredHotel_StructuredPricingReply" />
			</xsl:when>
			<xsl:otherwise>
				<OTA_HotelAvailRS>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<Errors>
						<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" />
					</Errors>
				</OTA_HotelAvailRS>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="PoweredHotel_SingleAvailabilityReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages" />
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList">
					<Errors>
						<xsl:apply-templates select="messageErrorList" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<RoomStays>
						<xsl:apply-templates select="singleAvailabilityDetails/propertyAvailability" />
					</RoomStays>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="propertyAvailability">
		<RoomStay AvailabilityStatus="AvailableForSale">
			<RatePlans>
				<xsl:apply-templates select="rateInformation" mode="One" />
			</RatePlans>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count">
						<xsl:value-of select="hotelProductInfo/roomDetails/occupancy" />
					</xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),5,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),3,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),1,2)" /></xsl:variable>
				<xsl:variable name="end">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),5,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),3,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),1,2)" /></xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration">
					<xsl:value-of select="$v1"/>
				</xsl:attribute>
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<Total>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="hotelProductInfo/otherHotelInformation/propertyCurrencyCode" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="rateInformation/rateAmountDetails/loadedDecimalPoint" />
				</xsl:attribute>
			</Total>
			<BasicPropertyInfo>
				<xsl:variable name="chcode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode" /></xsl:variable>
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
				<xsl:attribute name="ChainName"></xsl:attribute>
				<xsl:attribute name="HotelCodeContext">
					<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/accessQualifier" />
				</xsl:attribute>
				<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]"></xsl:apply-templates>
				<RelativePosition>
					<Transportations>
						<xsl:if test="hotelProductInfo/propertyHeaderDetails/bestTransportation !=''">
							<Transportation>
								<xsl:attribute name="TransportationCode">
									<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/bestTransportation" />
								</xsl:attribute>
							</Transportation>
						</xsl:if>
						<Transportation>
							<Descriptions>
								<Description Name="Description">
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='1']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11A']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11']/description"/>
								</Description>
								<Description Name="Facilities">
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4A']/description"/>
								</Description>
							</Descriptions>
						</Transportation>
					</Transportations>
				</RelativePosition>
			</BasicPropertyInfo>
			<AlternateInfo>
				<xsl:choose>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CA'">Complete Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CP'">Complete Access Plus</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'DY'">Dynamic Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'IA'">Independent Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'SA'">Standard Access</xsl:when>
				</xsl:choose>
			</AlternateInfo>
		</RoomStay>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="rateInformation" mode="One">
		<RatePlan>
			<xsl:attribute name="BookingCode"><xsl:value-of select="roomInformation/roomType"/><xsl:value-of select="roomInformation/rateCode"/></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="roomInformation/rateCode"/></xsl:attribute>
			<xsl:if test="roomInformation/rateCategoryCode != ''">
				<xsl:attribute name="RatePlanType"><xsl:value-of select="roomInformation/rateCategoryCode"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="rateIndicators/rateChangeIndicator = 'Y'">
				<xsl:attribute name="RateIndicator">ChangeDuringStay</xsl:attribute>
			</xsl:if>
			<xsl:if test="rateReferenceDetails/providerBookingCode!= ''">
				<xsl:attribute name="RatePlanID"><xsl:value-of select="rateReferenceDetails/providerBookingCode"/></xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="rateIndicators/guaranteeRequired = 'G'">
					<Guarantee>
						<xsl:attribute name="GuaranteeType">GuaranteeRequired</xsl:attribute>
					</Guarantee>
				</xsl:when>
				<xsl:when test="rateIndicators/guaranteeRequired = 'D'">
					<Guarantee>
						<xsl:attribute name="GuaranteeType">Deposit</xsl:attribute>
					</Guarantee>
				</xsl:when>
				<xsl:when test="rateIndicators/guaranteeRequired != ''">
					<Guarantee>
						<xsl:attribute name="HoldTime"><xsl:value-of select="rateIndicators/guaranteeRequired + 12" />:00:00</xsl:attribute>
					</Guarantee>
				</xsl:when>
			</xsl:choose>
			<RestrictionStatus>
				<xsl:choose>
					<xsl:when test="rateIndicators/roomAvailabilityStatus = 'A' or not(rateIndicators/roomAvailabilityStatus)">
						<xsl:attribute name="Status">Open</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="Status">Close</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</RestrictionStatus>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount">
						<xsl:value-of select="rateAmountDetails/loadedValue" />
					</xsl:attribute>
					<xsl:if test="rateDescriptionDetails/rateLineDescription!=''">
						<DetailDescription>
							<xsl:for-each select="rateDescriptionDetails/rateLineDescription">
								<Text><xsl:value-of select="."/></Text>
							</xsl:for-each>
						</DetailDescription>
					</xsl:if>
				</AdditionalDetail>
			</AdditionalDetails>
		</RatePlan>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="PoweredHotel_AvailabilityMultiPropertiesReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages" />
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList">
					<Errors>
						<xsl:apply-templates select="messageErrorList" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<RoomStays>
						<xsl:apply-templates select="propertyAvailabilityList" />
					</RoomStays>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="propertyAvailabilityList">
		<RoomStay>
			<RatePlans>
				<RatePlan>
					<AdditionalDetails>
						<xsl:apply-templates select="rateRangeInformation/minRate" />
						<xsl:apply-templates select="rateRangeInformation/maxRate" />
					</AdditionalDetails>
				</RatePlan>
			</RatePlans>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count">
						<xsl:value-of select="hotelProductInfo/roomDetails/occupancy" />
					</xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),5,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),3,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),1,2)" /></xsl:variable>
				<xsl:variable name="end">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),5,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),3,2)" />-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),1,2)" /></xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration">
					<xsl:value-of select="$v1"/>
				</xsl:attribute>
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<Total>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="hotelProductInfo/otherHotelInformation/propertyCurrencyCode" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="rateRangeInformation/minRate/loadedDecimalPoint" />
				</xsl:attribute>
			</Total>
			<BasicPropertyInfo>
				<xsl:variable name="chcode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode" /></xsl:variable>
				<xsl:attribute name="ChainCode">
					<xsl:value-of select="$chcode" />
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
				<xsl:attribute name="ChainName"></xsl:attribute>
				<xsl:attribute name="HotelCodeContext">
					<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/accessQualifier" />
				</xsl:attribute>
				<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]"></xsl:apply-templates>
				<RelativePosition>
					<Transportations>
						<xsl:if test="hotelProductInfo/propertyHeaderDetails/bestTransportation !=''">
							<Transportation>
								<xsl:attribute name="TransportationCode">
									<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/bestTransportation" />
								</xsl:attribute>
							</Transportation>
						</xsl:if>
						<Transportation>
							<Descriptions>
								<Description Name="Description">
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='1']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11A']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11']/description"/>
								</Description>
								<Description Name="Facilities">
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4']/description"/>
									<xsl:apply-templates select="//PoweredHotelsRS/PoweredHotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4A']/description"/>
								</Description>
							</Descriptions>
						</Transportation>
						<!--Transportation>
							<Descriptions>
								<Description Name="Photo">
									<URL>
										<xsl:text>http://amadeustest.leonardo.com/Content/</xsl:text>
										<xsl:value-of select="$chcode" />/<xsl:value-of select="$chcode" />
										<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/cityCode" />
										<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyCode" />/<xsl:value-of select="$chcode" />
										<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/cityCode" />
										<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyCode" />
										<xsl:text>_EXT_01_C.jpg?Referrer=API&amp;CorpId= Elleipsis &amp;UserId=LAS1S210D</xsl:text>
									</URL>
								</Description>
								<Description Name="Caption">
									<Text>
										<xsl:value-of select="@Caption"/>
									</Text>
								</Description>
								<Description Name="Height">
									<Text>
										<xsl:value-of select="@Height"/>
									</Text>
								</Description>
								<Description Name="Width">
									<Text>
										<xsl:value-of select="@Width"/>
									</Text>
								</Description>
							</Descriptions>
						</Transportation-->
					</Transportations>
				</RelativePosition>
			</BasicPropertyInfo>
			<AlternateInfo>
				<xsl:choose>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CA'">Complete Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'CP'">Complete Access Plus</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'DY'">Dynamic Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'IA'">Independent Access</xsl:when>
					<xsl:when test="hotelProductInfo/propertyHeaderDetails/accessQualifier = 'SA'">Standard Access</xsl:when>
				</xsl:choose>
			</AlternateInfo>
		</RoomStay>
	</xsl:template>
	
	<xsl:template match="PoweredHotel_StructuredPricingReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages" />
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList">
					<Errors>
						<xsl:apply-templates select="messageErrorList" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<RoomStays>
						<xsl:apply-templates select="hotelPricingSection" />
					</RoomStays>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	
	<xsl:template match="hotelPricingSection">
		<RoomStay AvailabilityStatus="AvailableForSale">
			<RatePlans>
				<xsl:apply-templates select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateChangeSection" />
			</RatePlans>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count">
						<xsl:value-of select="occupancyLevel/quantityDetails/numberOfUnit" />
					</xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">
					<xsl:value-of select="bookingPeriod/beginDateTime/year" />
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/beginDateTime/month,'00')" />
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/beginDateTime/day,'00')" />
				</xsl:variable>
				<xsl:variable name="end">
					<xsl:value-of select="bookingPeriod/endDateTime/year" />
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/endDateTime/month,'00')" />
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/endDateTime/day,'00')" />
				</xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration">
					<xsl:value-of select="$v1"/>
				</xsl:attribute>
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<Total>
				<xsl:choose>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/typeQualifier='712'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/amount,'.','')" />
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')" />
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')" />
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="AmountBeforeTax">
							<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')" />
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/currency" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="string-length(substring-after(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.'))" />
				</xsl:attribute>
				<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/category='TAX' or hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/category='SCH'">
					<Taxes>
						<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection[taxFeeInformation/category='TAX'] | hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection[taxFeeInformation/category='SCH']">
							<Tax>
								<xsl:attribute name="Type">
									<xsl:choose>
										<xsl:when test="../totalAmountInformation/monetaryDetails/typeQualifier='712'">Inclusive</xsl:when>
										<xsl:when test="taxFeeInformation/includedInAmount='I'">Inclusive</xsl:when>
										<xsl:otherwise>Exclusive</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="Code"><xsl:value-of select="taxFeeInformation/code"/></xsl:attribute>
								<xsl:choose>
									<xsl:when test="taxFeeInformation/percentage!=''">
										<xsl:attribute name="Percent"><xsl:value-of select="taxFeeInformation/percentage"/></xsl:attribute>
									</xsl:when>
									<xsl:when test="taxFeeInformation/amount!=''">
										<xsl:attribute name="Amount"><xsl:value-of select="taxFeeInformation/amount"/></xsl:attribute>
									</xsl:when>
								</xsl:choose>
								<xsl:if test="taxFeeInformation/longName!=''">
									<TaxDescription>
										<Text><xsl:value-of select="taxFeeInformation/longName"/></Text>
									</TaxDescription>
								</xsl:if>
								<xsl:if test="taxFeeInformation/timeUnit!=''">
									<TaxDescription>
										<Text>
											<xsl:choose>
												<xsl:when test="taxFeeInformation/timeUnit='DY'">DAILY</xsl:when>
												<xsl:when test="taxFeeInformation/timeUnit='PER'">PERIOD</xsl:when>
												<xsl:otherwise>PACKAGE</xsl:otherwise>
											</xsl:choose>
										</Text>
									</TaxDescription>
								</xsl:if>
								<xsl:if test="taxFeeInformation/perPerson!=''">
									<TaxDescription>
										<Text>
											<xsl:choose>
												<xsl:when test="taxFeeInformation/perPerson='ROO'">PER ROOM</xsl:when>
												<xsl:when test="taxFeeInformation/perPerson='PER'">PER PERSON</xsl:when>
												<xsl:otherwise>UNKNOWN</xsl:otherwise>
											</xsl:choose>
										</Text>
									</TaxDescription>
								</xsl:if>
							</Tax>
						</xsl:for-each>
					</Taxes>
				</xsl:if>
			</Total>
			<BasicPropertyInfo>
				<xsl:attribute name="ChainCode">
					<xsl:value-of select="hotelPropertyInfo/hotelReference/chainCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelCode">
					<xsl:value-of select="hotelPropertyInfo/hotelReference/hotelCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelCityCode">
					<xsl:value-of select="hotelPropertyInfo/hotelReference/cityCode" />
				</xsl:attribute>
				<xsl:attribute name="HotelName">
					<xsl:value-of select="hotelPropertyInfo/hotelName" />
				</xsl:attribute>
				<xsl:attribute name="ChainName">
					<xsl:value-of select="hotelChainInformation/companyName" />
				</xsl:attribute>
				<xsl:attribute name="HotelCodeContext">
					<xsl:value-of select="hotelChainInformation/accessLevel" />
				</xsl:attribute>
			</BasicPropertyInfo>
			<AlternateInfo>
				<xsl:choose>
					<xsl:when test="hotelChainInformation/accessLevel = 'CA'">Complete Access</xsl:when>
					<xsl:when test="hotelChainInformation/accessLevel = 'CP'">Complete Access Plus</xsl:when>
					<xsl:when test="hotelChainInformation/accessLevel = 'DY'">Dynamic Access</xsl:when>
					<xsl:when test="hotelChainInformation/accessLevel = 'IA'">Independent Access</xsl:when>
					<xsl:when test="hotelChainInformation/accessLevel = 'SA'">Standard Access</xsl:when>
				</xsl:choose>
			</AlternateInfo>
		</RoomStay>
	</xsl:template>
	
	<xsl:template match="rateChangeSection">
		<RatePlan>
			<xsl:attribute name="EffectiveDate">
				<xsl:value-of select="rateChangePeriodInformation/beginDateTime/year" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(rateChangePeriodInformation/beginDateTime/month,'00')" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(rateChangePeriodInformation/beginDateTime/day,'00')" />
			</xsl:attribute>
			<xsl:attribute name="ExpireDate">
				<xsl:value-of select="rateChangePeriodInformation/endDateTime/year" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(rateChangePeriodInformation/endDateTime/month,'00')" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(rateChangePeriodInformation/endDateTime/day,'00')" />
			</xsl:attribute>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(rateChangeAmountInformation/monetaryDetails/amount,'.','')" />
					</xsl:attribute>
					<DetailDescription>
						<Text>Daily</Text>
					</DetailDescription>
				</AdditionalDetail>
			</AdditionalDetails>
		</RatePlan>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="minRate">
		<AdditionalDetail>
			<xsl:attribute name="Code">MinimumRate</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="loadedValue" />
			</xsl:attribute>
		</AdditionalDetail>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="maxRate">
		<AdditionalDetail>
			<xsl:attribute name="Code">MaximumRate</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="loadedValue" />
			</xsl:attribute>
		</AdditionalDetail>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="CAPI_Messages">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="messageErrorList">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="messageErrorInformation/errorDetails/errorCode" />
			</xsl:attribute>
			<xsl:value-of select="errorFreeText/freetext" />
			<xsl:value-of select="errorFreeText/freeText" />
		</Error>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="PoweredHotel_FeaturesReply">
		<Address>
			<AddressLine><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1A']/description"/></AddressLine>
			<CityName><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1C']/description"/></CityName>
			<PostalCode><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1F']/description"/></PostalCode>
			<xsl:if test="hotelFeaturesTerms/featuresTerms[category='1D']/description != ''">
				<StateProv>
					<xsl:attribute name="StateCode"><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1D']/description"/></xsl:attribute>
				</StateProv>
			</xsl:if>
			<CountryName>
				<xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1E']/description"/>
			</CountryName>
		</Address>
		<ContactNumbers>
			<ContactNumber>
				<xsl:attribute name="PhoneTechType">Telephone</xsl:attribute>
				<xsl:attribute name="PhoneNumber"><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1G']/description"/></xsl:attribute>
			</ContactNumber>
			<ContactNumber>
				<xsl:attribute name="PhoneTechType">Fax</xsl:attribute>
				<xsl:attribute name="PhoneNumber"><xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1I']/description"/></xsl:attribute>
			</ContactNumber>
		</ContactNumbers>
	</xsl:template>
	
	<xsl:template match="description">
		<xsl:if test="string(.) != ''">
			<Text>
				<xsl:value-of select="."/>
			</Text>
		</xsl:if>
	</xsl:template>

<msxsl:script language="VisualBasic" implements-prefix="ttVB">
<![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]>
</msxsl:script>

</xsl:stylesheet>
