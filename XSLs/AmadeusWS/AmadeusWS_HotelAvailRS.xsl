<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_HotelAvailRS.xsl 												       -->
	<!-- ================================================================== -->
	<!-- Date: 27 Mar 2014 - Rastko - corrected total amount mapping for RAT rates		       -->
	<!-- Date: 17 Mar 2014 - Rastko - mapped currency exchange in structured pricing response -->
	<!-- Date: 05 Mar 2014 - Rastko - mapped structured pricing error condition		       -->
	<!-- Date: 08 Dec 2013 - Rastko - added hotel fees from structured pricing		       -->
	<!-- Date: 25 Oct 2013 - Rastko - added HotelAmenity tag to BasicPropertyInfo element	       -->
	<!-- Date: 19 Oct 2013 - Rastko - added Amenity mapping for hotel multi avail response	       -->
	<!-- Date: 09 Jan 2013 - Rastko - corrected error handling						       -->
	<!-- Date: 08 Jan 2013 - Rastko - corrected policy text and check in and check out times       -->
	<!-- Date: 22 Dec 2012 - Rastko - corrected error mapping in multi avail response	       -->
	<!-- Date: 13 Dec 2012 - Rastko - added mapping of alternate currency 			       -->
	<!-- Date: 12 Dec 2012 - Rastko - mapped avail error list to display correct error		       -->
	<!-- Date: 07 Dec 2012 - Rastko - mapped DC CC to DN CC					       -->
	<!-- Date: 04 Dec 2012 - Rastko - corrected code to match production multi media	       -->
	<!-- Date: 26 Nov 2012 - Rastko - mapped AM CC to AX CC					       -->
	<!-- Date: 15 Oct 2012 - Rastko - added checkin and checkout times			       -->
	<!-- Date: 13 Oct 2012 - Rastko - added rate breakdown for 5 first rates			       -->
	<!-- Date: 24 Sep 2012 - Rastko - grouped hotel main text under one Text tag		       -->
	<!-- Date: 13 Sep 2012 - Rastko - corrected credit card vendor mapping			       -->
	<!-- Date: 11 Sep 2012 - Rastko - added SubSection title in multi availability reply	       -->
	<!-- Date: 10 Sep 2012 - Rastko - added suport for Error tag					       -->
	<!-- Date: 08 Sep 2012 - Rastko - added code to return fake images in test		       -->
	<!-- Date: 04 Sep 2012 - Rastko - corrected room amenity mapping			       -->
	<!-- Date: 02 Sep 2012 - Rastko - complete integration of hotel pricing info		       -->
	<!-- Date: 31 Aug 2012 - Rastko - complete integration of hotel descriptive info		       -->
	<!-- Date: 30 Aug 2012 - Rastko - complete integration of hotel descriptive info		       -->
	<!-- Date: 28 Aug 2012 - Rastko - integrated hotel descriptive info				       -->
	<!-- Date: 07 Jul 2009 - Rastko														       -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="Hotel_SingleAvailabilityReply">
				<xsl:apply-templates select="Hotel_SingleAvailabilityReply"/>
			</xsl:when>
			<xsl:when test="Hotel_AvailabilityMultiPropertiesReply">
				<xsl:apply-templates select="Hotel_AvailabilityMultiPropertiesReply"/>
			</xsl:when>
			<xsl:when test="Hotel_StructuredPricingReply">
				<xsl:apply-templates select="Hotel_StructuredPricingReply"/>
			</xsl:when>
			<xsl:otherwise>
				<OTA_HotelAvailRS>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<Errors>
						<xsl:apply-templates select="//Errors/Error"/>
					</Errors>
				</OTA_HotelAvailRS>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="Hotel_SingleAvailabilityReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages"/>
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList">
					<Errors>
						<xsl:apply-templates select="messageErrorList"/>
					</Errors>
				</xsl:when>
				<xsl:when test="Hotel_StructuredPricingReply/errorInformation">
					<Errors>
						<xsl:apply-templates select="Hotel_StructuredPricingReply/errorInformation"/>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<RoomStays>
						<xsl:apply-templates select="singleAvailabilityDetails[1]/propertyAvailability"/>
					</RoomStays>
					<xsl:if test="OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent">
						<xsl:variable name="htldesc" select="OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent"/>
						<xsl:if test="$htldesc/AreaInfo/Recreations/Recreation/@Code!=''">
							<Areas>
								<xsl:for-each select="$htldesc/AreaInfo/Recreations/Recreation">
									<Area>
										<xsl:attribute name="AreaID"><xsl:value-of select="@Code"/></xsl:attribute>
										<AreaDescription>
											<Text></Text>
										</AreaDescription>
									</Area>
								</xsl:for-each>
							</Areas>
						</xsl:if>
						<!--xsl:if test="$htldesc/HotelInfo/Services/Service">
							<Criteria>
								<Criterion>
									<xsl:for-each select="$htldesc/HotelInfo/Services/Service">
										<xsl:choose>
											<xsl:when test="Features/Feature">
												<xsl:for-each select="Features/Feature">
													<HotelAmenity>
														<xsl:attribute name="Code">
															<xsl:choose>
																<xsl:when test="@AccessibleCode!=''"><xsl:value-of select="concat('PHY',@AccessibleCode)"/></xsl:when>
																<xsl:when test="@SecurityCode!=''"><xsl:value-of select="concat('SEC',@SecurityCode)"/></xsl:when>
																<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
															</xsl:choose>
														</xsl:attribute>
													</HotelAmenity>
												</xsl:for-each>
											</xsl:when>
											<xsl:when test="@BusinessServiceCode!=''">
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="concat('BUS',@BusinessServiceCode)"/>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:when>
											<xsl:when test="@Code!=''">
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="concat('HAC',@Code)"/>
														<xsl:if test="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description!=''">
															<xsl:for-each select="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
																<xsl:value-of select="concat(' ',.)"/>
															</xsl:for-each>
														</xsl:if>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:when>
											<xsl:otherwise>
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="."/>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</Criterion>
							</Criteria>
						</xsl:if-->
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="propertyAvailability">
		<xsl:variable name="hoteldesc">
			<xsl:value-of select="concat(hotelProductInfo/propertyHeaderDetails/chainCode,hotelProductInfo/propertyHeaderDetails/cityCode,hotelProductInfo/propertyHeaderDetails/propertyCode)"/>
		</xsl:variable>
		<RoomStay AvailabilityStatus="AvailableForSale">
			<xsl:if test="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
				<xsl:variable name="htldesc" select="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
				<xsl:if test="$htldesc/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity">
					<RoomTypes>
						<RoomType>
							<Amenities>
								<xsl:for-each select="$htldesc/FacilityInfo/GuestRooms/GuestRoom[1]/Amenities/Amenity">
									<Amenity>
										<xsl:attribute name="RoomAmenity"><xsl:value-of select="concat('RMA',@RoomAmenityCode)"/></xsl:attribute>
									</Amenity>
								</xsl:for-each>
							</Amenities>
						</RoomType>
					</RoomTypes>
				</xsl:if>
			</xsl:if>
			<RatePlans>
				<xsl:apply-templates select="rateInformation" mode="One"/>
				<xsl:apply-templates select="../../singleAvailabilityDetails[position() > 1]/propertyAvailability/rateInformation" mode="One"/>
			</RatePlans>
			<xsl:if test="../../Hotel_StructuredPricingReply">
				<RoomRates>
					<xsl:for-each select="../../Hotel_StructuredPricingReply">
						<RoomRate>
							<xsl:attribute name="BookingCode">
								<xsl:value-of select="hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/roomRateInfoSection/roomInformation/bookingCode"/>
							</xsl:attribute>
							<Rates>
								<xsl:variable name="baserate">
									<xsl:choose>
										<xsl:when test="hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount!=''">
											<xsl:value-of select="sum(hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="sum(hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:for-each select="hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateChangeSection">
									<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
									<Rate>
										<xsl:attribute name="EffectiveDate">
											<xsl:value-of select="concat(rateChangePeriodInformation/beginDateTime/year,'-')"/>
											<xsl:value-of select="concat(format-number(rateChangePeriodInformation/beginDateTime/month,'00'),'-')"/>
											<xsl:value-of select="format-number(rateChangePeriodInformation/beginDateTime/day,'00')"/>
										</xsl:attribute>
										<xsl:attribute name="ExpireDate">
											<xsl:value-of select="concat(rateChangePeriodInformation/endDateTime/year,'-')"/>
											<xsl:value-of select="concat(format-number(rateChangePeriodInformation/endDateTime/month,'00'),'-')"/>
											<xsl:value-of select="format-number(rateChangePeriodInformation/endDateTime/day,'00')"/>
										</xsl:attribute>
										<xsl:attribute name="RateTimeUnit"><xsl:value-of select="'Day'"/></xsl:attribute>
										<Base>
											<xsl:choose>
												<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
													<xsl:attribute name="AmountBeforeTax">
														<xsl:value-of select="translate(chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount,'.','')"/>
													</xsl:attribute>
													<xsl:attribute name="CurrencyCode"><xsl:value-of select="chgOfRateConvertedAmnt/conversionRateDetails/currency"/></xsl:attribute>
													<xsl:attribute name="DecimalPlaces">
														<xsl:choose>
															<xsl:when test="contains(chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount,'.')"><xsl:value-of select="'2'"/></xsl:when>
															<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>
												</xsl:when>
												<xsl:otherwise>
													<xsl:attribute name="AmountBeforeTax">
														<xsl:value-of select="translate(rateChangeAmountInformation/monetaryDetails/amount,'.','')"/>
													</xsl:attribute>
													<xsl:attribute name="CurrencyCode"><xsl:value-of select="rateChangeAmountInformation/monetaryDetails/currency"/></xsl:attribute>
													<xsl:attribute name="DecimalPlaces">
														<xsl:choose>
															<xsl:when test="contains(rateChangeAmountInformation/monetaryDetails/amount,'.')"><xsl:value-of select="'2'"/></xsl:when>
															<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>
												</xsl:otherwise>
											</xsl:choose>
										</Base>
										<xsl:if test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/infoMsgAndCancelPolicies/freeText!='' and $pos=1">
											<CancelPolicies>
												<CancelPenalty>
													<xsl:if test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime[businessSemantic='CAN']">
														<Deadline>
															<xsl:attribute name="AbsoluteDeadline">
																<xsl:value-of select="../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/year"/>
																<xsl:text>-</xsl:text>
																<xsl:value-of select="format-number(../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/month,'00')"/>
																<xsl:text>-</xsl:text>
																<xsl:value-of select="format-number(../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/day,'00')"/>
																<xsl:text>T</xsl:text>
																<xsl:value-of select="format-number(../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/hour,'00')"/>
																<xsl:text>:</xsl:text>
																<xsl:value-of select="format-number(../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/minutes,'00')"/>
																<xsl:text>:00</xsl:text>
															</xsl:attribute>
														</Deadline>
													</xsl:if>
													<PenaltyDescription>
														<xsl:attribute name="Name"><xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/pricingCategory/itemDescription/description"/></xsl:attribute>
														<Text>
															<xsl:for-each select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/infoMsgAndCancelPolicies/freeText">
																<xsl:value-of select="concat(.,' ')"/>
															</xsl:for-each>
														</Text>
													</PenaltyDescription>
												</CancelPenalty>
											</CancelPolicies>
										</xsl:if>
										<xsl:if test="$pos=1">
											<Total>
												<xsl:variable name="totalrate">
													<xsl:choose>
														<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
															<xsl:choose>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount/conversionRateDetails/conversionType='U'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount/conversionRateDetails/convertedValueAmount"/>
																</xsl:when>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount"/>
																</xsl:when>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:choose>
																		<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount">
																			<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount"/>
																		</xsl:when>
																		<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount">
																			<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount"/>
																		</xsl:when>
																		<xsl:otherwise>
																			<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='X']/conversionRateDetails/convertedValueAmount"/>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:when>
														<xsl:otherwise>
															<xsl:choose>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/typeQualifier='712'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/amount"/>
																</xsl:when>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount"/>
																</xsl:when>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/amount"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:choose>
													<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
														<xsl:choose>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount/conversionRateDetails/conversionType='U'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection	[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount/conversionRateDetails/convertedValueAmount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:otherwise>
																<xsl:attribute name="AmountBeforeTax">
																	<xsl:choose>
																		<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount">
																			<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount,'.','')"/>
																		</xsl:when>
																		<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount">
																			<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount,'.','')"/>
																		</xsl:when>
																		<xsl:otherwise>
																			<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='X']/conversionRateDetails/convertedValueAmount,'.','')"/>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:attribute>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:when>
													<xsl:otherwise>
														<xsl:choose>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/typeQualifier='712'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/amount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
																<xsl:attribute name="AmountAfterTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')"/>
																</xsl:attribute>
															</xsl:when>
															<xsl:otherwise>
																<xsl:attribute name="AmountBeforeTax">
																	<xsl:value-of select="translate(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/amount,'.','')"/>
																</xsl:attribute>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:otherwise>
												</xsl:choose>
												<xsl:attribute name="CurrencyCode">
													<xsl:choose>
														<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
															<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V' or conversionRateDetails/conversionType='W' or conversionRateDetails/conversionType='X']/conversionRateDetails/currency"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/currency"/>
														</xsl:otherwise>
													</xsl:choose>	
												</xsl:attribute>
												<xsl:variable name="dec">
													<xsl:choose>
														<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
															<xsl:choose>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount">
																	<xsl:value-of select="string-length(substring-after(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V']/conversionRateDetails/convertedValueAmount,'.'))"/>
																</xsl:when>
																<xsl:when test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount">
																	<xsl:value-of select="string-length(substring-after(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='W']/conversionRateDetails/convertedValueAmount,'.'))"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="string-length(substring-after(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='X']/conversionRateDetails/convertedValueAmount,'.'))"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="string-length(substring-after(../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.'))"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="$dec"/>
												</xsl:attribute>
												<xsl:if test="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/category='TAX' 
												               or ../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/category='SCH' 
												               or ../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/category='TAX'">
													<Taxes>
														<xsl:attribute name="Amount">
															<xsl:variable name="tot"><xsl:value-of select="$totalrate - $baserate"/></xsl:variable>
															<xsl:choose>
																<xsl:when test="$dec=2">
																	<xsl:value-of select="translate(format-number($tot,'0.00'),'.','')"/>
																</xsl:when>
																<xsl:otherwise><xsl:value-of select="$tot"/></xsl:otherwise>
															</xsl:choose>
														</xsl:attribute>
														<xsl:attribute name="CurrencyCode">
															<xsl:choose>
																<xsl:when test="chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount!=''">
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts[conversionRateDetails/conversionType='V' or conversionRateDetails/conversionType='W' or conversionRateDetails/conversionType='X']/conversionRateDetails/currency"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="../../../hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/currency"/>
																</xsl:otherwise>
															</xsl:choose>	
														</xsl:attribute>
														<xsl:attribute name="DecimalPlaces">
															<xsl:value-of select="$dec"/>
														</xsl:attribute>
													</Taxes>
												</xsl:if>
											</Total>
										</xsl:if>
									</Rate>
								</xsl:for-each>
							</Rates>
						</RoomRate>
					</xsl:for-each>
				</RoomRates>
			</xsl:if>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count"><xsl:value-of select="hotelProductInfo/roomDetails/occupancy"/></xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),5,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),3,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),1,2)"/>
				</xsl:variable>
				<xsl:variable name="end">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),5,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),3,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),1,2)"/>
				</xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<!--xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration">
					<xsl:value-of select="$v1"/>
				</xsl:attribute-->
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<xsl:if test="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
				<xsl:variable name="htldesc" select="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
				<xsl:if test="$htldesc/Policies/Policy/StayRequirements/StayRequirement/Description/Text!='' or $htldesc/Policies/Policy/GuaranteePaymentPolicy/GuaranteePayment/Description/Text!=''">
					<Guarantee>
						<xsl:if test="$htldesc/Policies/Policy/StayRequirements/StayRequirement/Description/Text!=''">
							<GuaranteeDescription Name="StayRequirement">
								<xsl:for-each select="$htldesc/Policies/Policy/StayRequirements/StayRequirement/Description/Text">
									<Text><xsl:value-of select="."/></Text>
								</xsl:for-each>
							</GuaranteeDescription>
						</xsl:if>
						<xsl:if test="$htldesc/Policies/Policy/GuaranteePaymentPolicy/GuaranteePayment/Description/Text!=''">
							<GuaranteeDescription Name="GuaranteePayment">
								<xsl:for-each select="$htldesc/Policies/Policy/GuaranteePaymentPolicy/GuaranteePayment/Description/Text">
									<Text><xsl:value-of select="."/></Text>
								</xsl:for-each>
							</GuaranteeDescription>
						</xsl:if>
				</Guarantee>
				</xsl:if>
			</xsl:if>
			<Total>
				<xsl:choose>
					<xsl:when test="hotelProductInfo/otherHotelInformation/currencyRequested!=''">
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="hotelProductInfo/otherHotelInformation/currencyRequested"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="rateInformation/rateAmountDetails/displayDecimalPoint"/></xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="hotelProductInfo/otherHotelInformation/propertyCurrencyCode"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="rateInformation/rateAmountDetails/loadedDecimalPoint"/></xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</Total>
			<BasicPropertyInfo>
				<xsl:variable name="chcode">
					<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode"/>
				</xsl:variable>
				<xsl:attribute name="ChainCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode"/></xsl:attribute>
				<xsl:attribute name="HotelCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyCode"/></xsl:attribute>
				<xsl:attribute name="HotelCityCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/cityCode"/></xsl:attribute>
				<xsl:attribute name="HotelName"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyName"/></xsl:attribute>
				<xsl:attribute name="ChainName"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainName"/></xsl:attribute>
				<xsl:attribute name="HotelCodeContext"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/accessQualifier"/></xsl:attribute>
				<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]"/>
				<xsl:if test="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
					<xsl:variable name="htldesc" select="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
					<VendorMessages>
						<VendorMessage>
							<xsl:attribute name="InfoType"><xsl:value-of select="'Image'"/></xsl:attribute>
							<SubSection SubTitle="HotelMainImage">
								<Paragraph Name="{hotelProductInfo/propertyHeaderDetails/propertyName}">
									<URL>
										<xsl:choose>
											<xsl:when test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']">
												<xsl:value-of select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/ImageItems/ImageItem/ImageFormat[@DimensionCategory='C']/URL"/>
											</xsl:when>
											<xsl:when test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem[1]/ImageFormat[@DimensionCategory='C']/URL!=''">
												<xsl:value-of select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem[1]/ImageFormat[@DimensionCategory='C']/URL"/>
											</xsl:when>
											<!--xsl:otherwise>
												<xsl:value-of select="'http://multimediarepository.amadeus.com/cmr/retrieve/hotel/0A85F9543AFB49F49D0C30B7C7471706/C'"/>
											</xsl:otherwise-->
										</xsl:choose>
									</URL>
								</Paragraph>
							</SubSection>
						</VendorMessage>
						<VendorMessage>
							<xsl:attribute name="InfoType"><xsl:value-of select="'Image'"/></xsl:attribute>
							<SubSection SubTitle="HotelImages">
								<xsl:if test="$htldesc/HotelInfo/Descriptions//MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem[@Category='15']/ImageFormat">
									<Paragraph Name="Logo">
										<URL>
											<xsl:value-of select="$htldesc/HotelInfo/Descriptions//MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem[@Category='15']/ImageFormat[position()=last()]/URL"/>
										</URL>
									</Paragraph>
								</xsl:if>
								<xsl:for-each select="$htldesc/HotelInfo/CategoryCodes/GuestRoomInfo/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem">
									<Paragraph>
										<xsl:attribute name="Name">
											<xsl:choose>
												<xsl:when test="Description!=''"><xsl:value-of select="Description"/></xsl:when>
												<xsl:otherwise><xsl:value-of select="Description/@Caption"/></xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<URL>
											<xsl:value-of select="ImageFormat[position()=last()]/URL"/>
										</URL>
									</Paragraph>
								</xsl:for-each>
								<xsl:for-each select="$htldesc/HotelInfo/Descriptions//MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem[@Category!='15']">
									<Paragraph>
										<xsl:attribute name="Name">
											<xsl:choose>
												<xsl:when test="Description!=''"><xsl:value-of select="Description"/></xsl:when>
												<xsl:otherwise><xsl:value-of select="Description/@Caption"/></xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<URL>
											<xsl:value-of select="ImageFormat[position()=last()]/URL"/>
										</URL>
									</Paragraph>
								</xsl:for-each>
								<xsl:for-each select="$htldesc/HotelInfo/Services/Service/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem">
									<Paragraph>
										<xsl:attribute name="Name">
											<xsl:choose>
												<xsl:when test="Description!=''"><xsl:value-of select="Description"/></xsl:when>
												<xsl:otherwise><xsl:value-of select="Description/@Caption"/></xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<URL>
											<xsl:value-of select="ImageFormat[position()=last()]/URL"/>
										</URL>
									</Paragraph>
								</xsl:for-each>
								<xsl:for-each select="$htldesc/AreaInfo/Recreations/Recreation/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem">
									<Paragraph>
										<xsl:attribute name="Name">
											<xsl:choose>
												<xsl:when test="Description!=''"><xsl:value-of select="Description"/></xsl:when>
												<xsl:otherwise><xsl:value-of select="Description/@Caption"/></xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<URL>
											<xsl:value-of select="ImageFormat[position()=last()]/URL"/>
										</URL>
									</Paragraph>
								</xsl:for-each>
							</SubSection>
						</VendorMessage>
						<VendorMessage>
							<xsl:attribute name="InfoType"><xsl:value-of select="'Text'"/></xsl:attribute>
							<SubSection SubTitle="HotelMainText">
								<Paragraph>
									<xsl:choose>
										<xsl:when test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/TextItems/TextItem/Description!=''">
											<Text>
												<xsl:for-each select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/TextItems/TextItem/Description">
													<xsl:value-of select="concat(.,' ')"/>
												</xsl:for-each>
											</Text>
										</xsl:when>
										<xsl:otherwise>
											<Text>
												<xsl:value-of select="concat('A ',hotelProductInfo/propertyHeaderDetails/chainName,' property')"/>
											</Text>
										</xsl:otherwise>
									</xsl:choose>
								</Paragraph>
							</SubSection>
							<xsl:if test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@AdditionalDetailCode='3']/TextItems/TextItem/Description">
								<SubSection SubTitle="Location">
									<Paragraph>
										<xsl:for-each select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@AdditionalDetailCode='3']/TextItems/TextItem/Description">
											<Text>
												<xsl:value-of select="."/>
											</Text>
										</xsl:for-each>
									</Paragraph>
								</SubSection>
							</xsl:if>
							<xsl:if test="$htldesc/HotelInfo/CategoryCodes/GuestRoomInfo/MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
								<SubSection SubTitle="Guest room info">
									<Paragraph>
										<xsl:for-each select="$htldesc/HotelInfo/CategoryCodes/GuestRoomInfo/MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
											<Text>
												<xsl:value-of select="."/>
											</Text>
										</xsl:for-each>
									</Paragraph>
								</SubSection>
							</xsl:if>
							<xsl:for-each select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@AdditionalDetailCode!='3' or not(@AdditionalDetailCode)][not(ImageItems)]">
								<SubSection>
									<xsl:attribute name="SubTitle"></xsl:attribute>
									<xsl:attribute name="SubCode">
										<xsl:choose>
											<xsl:when test="@AdditionalDetailCode!=''"><xsl:value-of select="concat('ADT',@AdditionalDetailCode)"/></xsl:when>
											<xsl:otherwise><xsl:value-of select="concat('INF',@InfoCode)"/></xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<Paragraph>
										<xsl:for-each select="TextItems/TextItem/Description">
											<Text>
												<xsl:value-of select="."/>
											</Text>
										</xsl:for-each>
									</Paragraph>
								</SubSection>
							</xsl:for-each>
						</VendorMessage>
						<xsl:choose>
							<xsl:when test="$htldesc/Policies/Policy/PolicyInfo/@CheckInTime!=''">
								<VendorMessage>
									<SubSection>
										<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-in Time'"/></xsl:attribute>
										<Paragraph>
											<Text>
												<xsl:value-of select="$htldesc/Policies/Policy/PolicyInfo/@CheckInTime"/>
											</Text>
										</Paragraph>
									</SubSection>
									<xsl:if test="$htldesc/Policies/Policy/PolicyInfo/@CheckOutTime!=''">
										<SubSection>
											<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-out Time'"/></xsl:attribute>
											<Paragraph>
												<Text>
													<xsl:value-of select="$htldesc/Policies/Policy/PolicyInfo/@CheckOutTime"/>
												</Text>
											</Paragraph>
										</SubSection>
									</xsl:if>
								</VendorMessage>
							</xsl:when>
							<xsl:when test="../../Hotel_StructuredPricingReply/hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/otherInfoSection/checkInOutTimeAndExpressInfo">
								<xsl:variable name="check" select="../../Hotel_StructuredPricingReply/hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/otherInfoSection/checkInOutTimeAndExpressInfo"/>
								<VendorMessage>
									<xsl:if test="$check/expressCheckIn='1'">
										<SubSection>
											<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-in Time'"/></xsl:attribute>
											<Paragraph>
												<Text>
													<xsl:value-of select="format-number($check/checkInTimeLimitation/hour,'00')"/>
													<xsl:value-of select="concat(':',format-number($check/checkInTimeLimitation/minutes,'00'))"/>
												</Text>
											</Paragraph>
										</SubSection>
									</xsl:if>
									<xsl:if test="$check/expressCheckOut='1'">
										<SubSection>
											<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-out Time'"/></xsl:attribute>
											<Paragraph>
												<Text>
													<xsl:value-of select="format-number($check/checkOutTimeLimitation/hour,'00')"/>
													<xsl:value-of select="concat(':',format-number($check/checkOutTimeLimitation/minutes,'00'))"/>
												</Text>
											</Paragraph>
										</SubSection>
									</xsl:if>
								</VendorMessage>
							</xsl:when>
							<xsl:when test="../../Hotel_StructuredPricingReply/hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/infoMsgAndCancelPolicies/freeText">
								<xsl:variable name="check">
									<xsl:value-of select="../../Hotel_StructuredPricingReply/hotelPricingSection/hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/infoMsgAndCancelPolicies/freeText"/>
								</xsl:variable>
								<VendorMessage>
									<xsl:if test="contains($check,'CHECK IN ')">
										<SubSection>
											<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-in Time'"/></xsl:attribute>
											<Paragraph>
												<Text>
													<xsl:value-of select="substring-before(substring-after($check,'CHECK IN '),' ')"/>
												</Text>
											</Paragraph>
										</SubSection>
									</xsl:if>
									<xsl:if test="contains($check,'CHECK OUT')">
										<SubSection>
											<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-out Time'"/></xsl:attribute>
											<Paragraph>
												<Text>
													<xsl:value-of select="substring-after($check,'CHECK OUT')"/>
												</Text>
											</Paragraph>
										</SubSection>
									</xsl:if>
								</VendorMessage>
							</xsl:when>
						</xsl:choose>
					</VendorMessages>
					<xsl:copy-of select="$htldesc/ContactInfos/ContactInfo[1]/Addresses/Address[1]"/>
					<ContactNumbers>
						<xsl:for-each select="$htldesc/ContactInfos/ContactInfo[1]/Phones/Phone">
							<ContactNumber>
								<xsl:attribute name="PhoneTechType"><xsl:value-of select="@PhoneTechType"/></xsl:attribute>
								<xsl:attribute name="PhoneNumber"><xsl:value-of select="@PhoneNumber"/></xsl:attribute>
							</ContactNumber>
						</xsl:for-each>
					</ContactNumbers>
					<xsl:copy-of select="$htldesc/AffiliationInfo/Awards/Award"/>
				</xsl:if>
				<RelativePosition>
					<Transportations>
						<xsl:if test="hotelProductInfo/propertyHeaderDetails/bestTransportation !=''">
							<Transportation>
								<xsl:attribute name="TransportationCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/bestTransportation"/></xsl:attribute>
							</Transportation>
						</xsl:if>
						<xsl:if test="//HotelsRS/Hotel_FeaturesReply">
							<Transportation>
								<Descriptions>
									<Description Name="Description">
										<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='1']/description"/>
										<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11A']/description"/>
										<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11']/description"/>
									</Description>
									<Description Name="Facilities">
										<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4']/description"/>
										<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4A']/description"/>
									</Description>
								</Descriptions>
							</Transportation>
						</xsl:if>
						<xsl:if test="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
							<xsl:variable name="htldesc" select="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
							<xsl:if test="$htldesc/AreaInfo/RefPoints/RefPoint/Transportations/Transportation/MultimediaDescriptions/MultimediaDescription/TextItems/TextItem[@Title='Transportation']/Description!=''">
								<Transportation>
									<Descriptions>
										<Description Name="Transportation">
											<xsl:for-each select="$htldesc/AreaInfo/RefPoints/RefPoint/Transportations/Transportation/MultimediaDescriptions/MultimediaDescription/TextItems/TextItem[@Title='Transportation']/Description">
												<Text><xsl:value-of select="."/></Text>
											</xsl:for-each>
										</Description>
									</Descriptions>
								</Transportation>
							</xsl:if>
						</xsl:if>
					</Transportations>
				</RelativePosition>
				<xsl:if test="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
					<xsl:variable name="htldesc" select="../../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
					<xsl:if test="$htldesc/HotelInfo/Services/Service">
						<xsl:for-each select="$htldesc/HotelInfo/Services/Service">
							<xsl:choose>
								<xsl:when test="Features/Feature">
									<xsl:for-each select="Features/Feature">
										<HotelAmenity>
											<xsl:attribute name="Code">
												<xsl:choose>
													<xsl:when test="@AccessibleCode!=''"><xsl:value-of select="concat('PHY',@AccessibleCode)"/></xsl:when>
													<xsl:when test="@SecurityCode!=''"><xsl:value-of select="concat('SEC',@SecurityCode)"/></xsl:when>
													<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</HotelAmenity>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="@BusinessServiceCode!=''">
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="concat('BUS',@BusinessServiceCode)"/>
										</xsl:attribute>
									</HotelAmenity>
								</xsl:when>
								<xsl:when test="@Code!=''">
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="concat('HAC',@Code)"/>
											<!--xsl:if test="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description!=''">
												<xsl:for-each select="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
													<xsl:value-of select="concat(' ',.)"/>
												</xsl:for-each>
											</xsl:if-->
										</xsl:attribute>
									</HotelAmenity>
								</xsl:when>
								<xsl:otherwise>
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="."/>
										</xsl:attribute>
									</HotelAmenity>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:if>
				</xsl:if>
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
			<xsl:attribute name="BookingCode"><xsl:choose><xsl:when test="rateReferenceDetails/providerBookingCode!= ''"><xsl:value-of select="rateReferenceDetails/providerBookingCode"/></xsl:when><xsl:otherwise><xsl:value-of select="roomInformation/roomType"/><xsl:value-of select="roomInformation/rateCode"/></xsl:otherwise></xsl:choose></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="roomInformation/rateCode"/></xsl:attribute>
			<xsl:if test="roomInformation/rateCategoryCode != ''">
				<xsl:attribute name="RatePlanType"><xsl:value-of select="roomInformation/rateCategoryCode"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="rateIndicators/rateChangeIndicator = 'Y'">
				<xsl:attribute name="RateIndicator">ChangeDuringStay</xsl:attribute>
			</xsl:if>
			<!--xsl:if test="rateReferenceDetails/providerBookingCode!= ''">
				<xsl:attribute name="RatePlanID"><xsl:value-of select="rateReferenceDetails/providerBookingCode"/></xsl:attribute>
			</xsl:if-->
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
						<xsl:attribute name="HoldTime"><xsl:value-of select="rateIndicators/guaranteeRequired + 12"/>:00:00</xsl:attribute>
					</Guarantee>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="rateIndicators/mealIncluded='B'">
				<MealsIncluded>
					<xsl:attribute name="Breakfast"><xsl:value-of select="'true'"/></xsl:attribute>
				</MealsIncluded>
			</xsl:if>
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
						<xsl:choose>
							<xsl:when test="rateAmountDetails/displayValue!=''">
								<xsl:value-of select="rateAmountDetails/displayValue"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="rateAmountDetails/loadedValue"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:if test="rateDescriptionDetails/rateLineDescription!=''">
						<DetailDescription>
							<xsl:for-each select="rateDescriptionDetails/rateLineDescription">
								<Text>
									<xsl:value-of select="."/>
								</Text>
							</xsl:for-each>
						</DetailDescription>
					</xsl:if>
				</AdditionalDetail>
				<xsl:variable name="cat">
					<xsl:value-of select="substring(roomInformation/roomType,1,1)"/>
				</xsl:variable>
				<xsl:variable name="nip">
					<xsl:value-of select="substring(roomInformation/roomType,2,1)"/>
				</xsl:variable>
				<xsl:variable name="bed">
					<xsl:value-of select="substring(roomInformation/roomType,3,1)"/>
				</xsl:variable>
				<AdditionalDetail Type="Category">
					<DetailDescription>
						<xsl:attribute name="Name">
							<xsl:choose>
								<xsl:when test="$cat='A'">
									<xsl:value-of select="'Superior'"/>
								</xsl:when>
								<xsl:when test="$cat='B'">
									<xsl:value-of select="'Moderate'"/>
								</xsl:when>
								<xsl:when test="$cat='C'">
									<xsl:value-of select="'Standard'"/>
								</xsl:when>
								<xsl:when test="$cat='D'">
									<xsl:value-of select="'Minimum'"/>
								</xsl:when>
								<xsl:when test="$cat='E'">
									<xsl:value-of select="'Superior'"/>
								</xsl:when>
								<xsl:when test="$cat='F'">
									<xsl:value-of select="'Moderate'"/>
								</xsl:when>
								<xsl:when test="$cat='G'">
									<xsl:value-of select="'Standard'"/>
								</xsl:when>
								<xsl:when test="$cat='H'">
									<xsl:value-of select="'Minimum'"/>
								</xsl:when>
								<xsl:when test="$cat='I'">
									<xsl:value-of select="'Moderate'"/>
								</xsl:when>
								<xsl:when test="$cat='J'">
									<xsl:value-of select="'Standard'"/>
								</xsl:when>
								<xsl:when test="$cat='K'">
									<xsl:value-of select="'Minimum'"/>
								</xsl:when>
								<xsl:when test="$cat='P'">
									<xsl:value-of select="'Executive'"/>
								</xsl:when>
								<xsl:when test="$cat='S'">
									<xsl:value-of select="'Moderate'"/>
								</xsl:when>
								<xsl:when test="$cat='T'">
									<xsl:value-of select="'Standard'"/>
								</xsl:when>
								<xsl:when test="$cat='U'">
									<xsl:value-of select="'Minimum'"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'Any'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</DetailDescription>
				</AdditionalDetail>
				<AdditionalDetail Type="NumberOfBeds">
					<DetailDescription>
						<xsl:attribute name="Name">
							<xsl:choose>
								<xsl:when test="translate($nip,'1234567890','')!=''"><xsl:value-of select="'Any'"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="$nip"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</DetailDescription>
				</AdditionalDetail>
				<AdditionalDetail Type="BedType">
					<DetailDescription>
						<xsl:attribute name="Name">
							<xsl:choose>
								<xsl:when test="$bed='D'">
									<xsl:value-of select="'Double'"/>
								</xsl:when>
								<xsl:when test="$bed='K'">
									<xsl:value-of select="'King'"/>
								</xsl:when>
								<xsl:when test="$bed='P'">
									<xsl:value-of select="'Pullout'"/>
								</xsl:when>
								<xsl:when test="$bed='Q'">
									<xsl:value-of select="'Queen'"/>
								</xsl:when>
								<xsl:when test="$bed='S'">
									<xsl:value-of select="'Single'"/>
								</xsl:when>
								<xsl:when test="$bed='T'">
									<xsl:value-of select="'Twin'"/>
								</xsl:when>
								<xsl:when test="$bed='W'">
									<xsl:value-of select="'Water Bed'"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'Any'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</DetailDescription>
				</AdditionalDetail>
			</AdditionalDetails>
		</RatePlan>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="Hotel_AvailabilityMultiPropertiesReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages"/>
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList and not(propertyAvailabilityList)">
					<Errors>
						<xsl:apply-templates select="messageErrorList"/>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<RoomStays>
						<xsl:apply-templates select="propertyAvailabilityList"/>
					</RoomStays>
					<!--xsl:if test="OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent">
						<xsl:variable name="htldesc" select="OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent"/>
						<xsl:if test="$htldesc/HotelInfo/Services/Service">
							<Criteria>
								<Criterion>
									<xsl:for-each select="$htldesc/HotelInfo/Services/Service">
										<xsl:choose>
											<xsl:when test="Features/Feature">
												<xsl:for-each select="Features/Feature">
													<HotelAmenity>
														<xsl:attribute name="Code">
															<xsl:choose>
																<xsl:when test="@AccessibleCode!=''"><xsl:value-of select="concat('PHY',@AccessibleCode)"/></xsl:when>
																<xsl:when test="@SecurityCode!=''"><xsl:value-of select="concat('SEC',@SecurityCode)"/></xsl:when>
																<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
															</xsl:choose>
														</xsl:attribute>
													</HotelAmenity>
												</xsl:for-each>
											</xsl:when>
											<xsl:when test="@BusinessServiceCode!=''">
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="concat('BUS',@BusinessServiceCode)"/>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:when>
											<xsl:when test="@Code!=''">
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="concat('HAC',@Code)"/>
														<xsl:if test="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description!=''">
															<xsl:for-each select="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
																<xsl:value-of select="concat(' ',.)"/>
															</xsl:for-each>
														</xsl:if>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:when>
											<xsl:otherwise>
												<HotelAmenity>
													<xsl:attribute name="Code">
														<xsl:value-of select="."/>
													</xsl:attribute>
												</HotelAmenity>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</Criterion>
							</Criteria>
						</xsl:if>
					</xsl:if-->
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="propertyAvailabilityList">
		<xsl:variable name="hoteldesc">
			<xsl:value-of select="concat(hotelProductInfo/propertyHeaderDetails/chainCode,hotelProductInfo/propertyHeaderDetails/cityCode,hotelProductInfo/propertyHeaderDetails/propertyCode)"/>
		</xsl:variable>
		<RoomStay AvailabilityStatus="AvailableForSale">
			<xsl:if test="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
				<xsl:variable name="htldesc" select="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
				<xsl:if test="$htldesc/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity">
					<RoomTypes>
						<RoomType>
							<Amenities>
								<xsl:for-each select="$htldesc/FacilityInfo/GuestRooms/GuestRoom[1]/Amenities/Amenity">
									<Amenity>
										<xsl:attribute name="RoomAmenity"><xsl:value-of select="concat('RMA',@RoomAmenityCode)"/></xsl:attribute>
									</Amenity>
								</xsl:for-each>
							</Amenities>
						</RoomType>
					</RoomTypes>
				</xsl:if>
			</xsl:if>
			<RatePlans>
				<RatePlan>
					<AdditionalDetails>
						<xsl:apply-templates select="rateRangeInformation/minRate"/>
						<xsl:apply-templates select="rateRangeInformation/maxRate"/>
					</AdditionalDetails>
				</RatePlan>
			</RatePlans>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count"><xsl:value-of select="hotelProductInfo/roomDetails/occupancy"/></xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),5,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),3,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkInDate),1,2)"/>
				</xsl:variable>
				<xsl:variable name="end">20<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),5,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),3,2)"/>-<xsl:value-of select="substring(string(hotelProductInfo/propertyHeaderDetails/checkOutDate),1,2)"/>
				</xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<!--xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration"><xsl:value-of select="$v1"/></xsl:attribute-->
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<Total>
				<xsl:choose>
					<xsl:when test="hotelProductInfo/otherHotelInformation/currencyRequested!=''">
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="hotelProductInfo/otherHotelInformation/currencyRequested"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="rateRangeInformation/minRate/displayDecimalPoint"/></xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="hotelProductInfo/otherHotelInformation/propertyCurrencyCode"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="rateRangeInformation/minRate/loadedDecimalPoint"/></xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</Total>
			<BasicPropertyInfo>
				<xsl:variable name="chcode">
					<xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainCode"/>
				</xsl:variable>
				<xsl:attribute name="ChainCode"><xsl:value-of select="$chcode"/></xsl:attribute>
				<xsl:attribute name="HotelCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyCode"/></xsl:attribute>
				<xsl:attribute name="HotelCityCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/cityCode"/></xsl:attribute>
				<xsl:attribute name="HotelName"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/propertyName"/></xsl:attribute>
				<xsl:attribute name="ChainName"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/chainName"/></xsl:attribute>
				<xsl:attribute name="HotelCodeContext"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/accessQualifier"/></xsl:attribute>
				<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]"/>
				<xsl:if test="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
					<xsl:variable name="htldesc" select="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
					<VendorMessages>
						<VendorMessage>
							<xsl:attribute name="InfoType"><xsl:value-of select="'Image'"/></xsl:attribute>
							<SubSection SubTitle="HotelMainImage">
								<Paragraph Name="{hotelProductInfo/propertyHeaderDetails/propertyName}">
									<URL>
										<xsl:choose>
											<xsl:when test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/ImageItems/ImageItem/ImageFormat[@DimensionCategory='C']/URL!=''">
												<xsl:value-of select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/ImageItems/ImageItem/ImageFormat[@DimensionCategory='C']/URL"/>
											</xsl:when>
											<xsl:when test="$htldesc/HotelInfo/CategoryCodes/GuestRoomInfo/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem/ImageFormat[@DimensionCategory='C']/URL!=''">
												<xsl:value-of select="$htldesc/HotelInfo/CategoryCodes/GuestRoomInfo/MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem/ImageFormat[@DimensionCategory='C']/URL"/>
											</xsl:when>
											<!--xsl:otherwise>
												<xsl:value-of select="'http://multimediarepository.amadeus.com/cmr/retrieve/hotel/0A85F9543AFB49F49D0C30B7C7471706/C'"/>
											</xsl:otherwise-->
										</xsl:choose>
									</URL>
								</Paragraph>
							</SubSection>
						</VendorMessage>
						<VendorMessage>
							<xsl:attribute name="InfoType"><xsl:value-of select="'Text'"/></xsl:attribute>
							<SubSection SubTitle="HotelMainText">
								<Paragraph>
									<xsl:choose>
										<xsl:when test="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/TextItems/TextItem/Description!=''">
											<Text>
												<xsl:for-each select="$htldesc/HotelInfo/Descriptions/MultimediaDescriptions/MultimediaDescription[@InfoCode='1']/TextItems/TextItem/Description">																								<xsl:value-of select="concat(.,' ')"/>
												</xsl:for-each>
											</Text>
										</xsl:when>
										<xsl:otherwise>
											<Text>
												<xsl:value-of select="concat('A ',hotelProductInfo/propertyHeaderDetails/chainName,' property')"/>
											</Text>
										</xsl:otherwise>
									</xsl:choose>
								</Paragraph>
							</SubSection>
						</VendorMessage>
					</VendorMessages>
					<xsl:copy-of select="$htldesc/ContactInfos/ContactInfo[1]/Addresses/Address[1]"/>
					<ContactNumbers>
						<xsl:for-each select="$htldesc/ContactInfos/ContactInfo[1]/Phones/Phone">
							<ContactNumber>
								<xsl:attribute name="PhoneTechType"><xsl:value-of select="@PhoneTechType"/></xsl:attribute>
								<xsl:attribute name="PhoneNumber"><xsl:value-of select="@PhoneNumber"/></xsl:attribute>
							</ContactNumber>
						</xsl:for-each>
					</ContactNumbers>
					<xsl:copy-of select="$htldesc/AffiliationInfo/Awards/Award"/>
				</xsl:if>
				<RelativePosition>
					<Transportations>
						<xsl:if test="hotelProductInfo/propertyHeaderDetails/bestTransportation !=''">
							<Transportation>
								<xsl:attribute name="TransportationCode"><xsl:value-of select="hotelProductInfo/propertyHeaderDetails/bestTransportation"/></xsl:attribute>
							</Transportation>
						</xsl:if>
						<Transportation>
							<Descriptions>
								<Description Name="Description">
									<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='1']/description"/>
									<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11A']/description"/>
									<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='11']/description"/>
								</Description>
								<Description Name="Facilities">
									<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4']/description"/>
									<xsl:apply-templates select="//HotelsRS/Hotel_FeaturesReply[hotelProductInfo/propertyHeaderDetails/chainCode = $chcode]/hotelFeaturesTerms/featuresTerms[category='4A']/description"/>
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
				<xsl:if test="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]">
					<xsl:variable name="htldesc" select="../OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent[@HotelCode=$hoteldesc]"/>
					<xsl:if test="$htldesc/HotelInfo/Services/Service">
						<xsl:for-each select="$htldesc/HotelInfo/Services/Service">
							<xsl:choose>
								<xsl:when test="Features/Feature">
									<xsl:for-each select="Features/Feature">
										<HotelAmenity>
											<xsl:attribute name="Code">
												<xsl:choose>
													<xsl:when test="@AccessibleCode!=''"><xsl:value-of select="concat('PHY',@AccessibleCode)"/></xsl:when>
													<xsl:when test="@SecurityCode!=''"><xsl:value-of select="concat('SEC',@SecurityCode)"/></xsl:when>
													<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</HotelAmenity>
									</xsl:for-each>
								</xsl:when>
								<xsl:when test="@BusinessServiceCode!=''">
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="concat('BUS',@BusinessServiceCode)"/>
										</xsl:attribute>
									</HotelAmenity>
								</xsl:when>
								<xsl:when test="@Code!=''">
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="concat('HAC',@Code)"/>
											<!--xsl:if test="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description!=''">
												<xsl:for-each select="MultimediaDescriptions/MultimediaDescription/TextItems/TextItem/Description">
													<xsl:value-of select="concat(' ',.)"/>
												</xsl:for-each>
											</xsl:if-->
										</xsl:attribute>
									</HotelAmenity>
								</xsl:when>
								<xsl:otherwise>
									<HotelAmenity>
										<xsl:attribute name="Code">
											<xsl:value-of select="."/>
										</xsl:attribute>
									</HotelAmenity>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:if>
				</xsl:if>
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
	<xsl:template match="Hotel_StructuredPricingReply">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//CAPI_Messages/LineType='E'">
					<Errors>
						<xsl:apply-templates select="//CAPI_Messages"/>
					</Errors>
				</xsl:when>
				<xsl:when test="messageErrorList">
					<Errors>
						<xsl:apply-templates select="messageErrorList"/>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<RoomStays>
						<xsl:apply-templates select="hotelPricingSection"/>
					</RoomStays>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelAvailRS>
	</xsl:template>
	<xsl:template match="hotelPricingSection">
		<RoomStay AvailabilityStatus="AvailableForSale">
			<RatePlans>
				<xsl:apply-templates select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateChangeSection"/>
			</RatePlans>
			<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='DES']/roomRateInfoSection">
				<xsl:variable name="roomrate" select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='DES']/roomRateInfoSection"/>
				<RoomRates>
					<RoomRate>
						<xsl:attribute name="BookingCode"><xsl:value-of select="$roomrate/roomInformation/bookingCode"/></xsl:attribute>
						<xsl:attribute name="RoomTypeCode"><xsl:value-of select="$roomrate/roomInformation/roomRateIdentifier/roomType"/></xsl:attribute>
						<xsl:attribute name="RatePlanCode"><xsl:value-of select="$roomrate/roomInformation/roomRateIdentifier/ratePlanCode"/></xsl:attribute>
						<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='DES']/totalAmountInformation or hotelPricingCategorySection/taxSection">
							<Rates>
								<Rate>
									<xsl:if test="hotelPricingCategorySection/taxSection">
										<Fees>
											<xsl:for-each select="hotelPricingCategorySection/taxSection/taxFeeInformation">
												<Fee>
													<xsl:attribute name="Type">
														<xsl:choose>
															<xsl:when test="includedInAmount='I'">Inclusive</xsl:when>
															<xsl:otherwise>Exclusive</xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>
													<xsl:if test="percentage!=''">
														<xsl:attribute name="Percent">
															<xsl:value-of select="percentage"/>
														</xsl:attribute>
													</xsl:if>
													<xsl:choose>
														<xsl:when test="following-sibling::convertedTaxAmount[1]/conversionRateDetails/convertedValueAmount!=''">
															<xsl:attribute name="Amount">
																<xsl:value-of select="following-sibling::convertedTaxAmount[1]/conversionRateDetails/convertedValueAmount"/>
															</xsl:attribute>
														</xsl:when>
														<xsl:when test="amount!=''">
															<xsl:attribute name="Amount">
																<xsl:value-of select="amount"/>
															</xsl:attribute>
														</xsl:when>
													</xsl:choose>
													<xsl:choose>
														<xsl:when test="following-sibling::convertedTaxAmount[1]/conversionRateDetails/currency!=''">
															<xsl:attribute name="CurrencyCode">
																<xsl:value-of select="following-sibling::convertedTaxAmount[1]/conversionRateDetails/currency"/>
															</xsl:attribute>
														</xsl:when>
														<xsl:when test="currencyCode!=''">
															<xsl:attribute name="CurrencyCode">
																<xsl:value-of select="currencyCode"/>
															</xsl:attribute>
														</xsl:when>
													</xsl:choose>
													<Description>
														<Text>
															<xsl:value-of select="longName"/>
														</Text>
														<xsl:if test="perPerson='PER'">
															<Text><xsl:value-of select="'Per Person'"/></Text>
														</xsl:if>
														<xsl:if test="perPerson='ROO'">
															<Text><xsl:value-of select="'Per Room'"/></Text>
														</xsl:if>
														<xsl:if test="timeUnit='DY'">
															<Text><xsl:value-of select="'Daily'"/></Text>
														</xsl:if>
														<xsl:if test="timeUnit='PER'">
															<Text><xsl:value-of select="'Per period'"/></Text>
														</xsl:if>
														<xsl:if test="timeUnit='4'">
															<Text><xsl:value-of select="'Package'"/></Text>
														</xsl:if>
													</Description>
												</Fee>
											</xsl:for-each>
										</Fees>
									</xsl:if>
								</Rate>
							</Rates>
						</xsl:if>
						<RoomRateDescription>
							<xsl:for-each select="$roomrate/roomRateDescription/freeText">
								<Text><xsl:value-of select="."/></Text>
							</xsl:for-each>
						</RoomRateDescription>
					</RoomRate>
				</RoomRates>
			</xsl:if>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count"><xsl:value-of select="occupancyLevel/quantityDetails/numberOfUnit"/></xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:variable name="start">
					<xsl:value-of select="bookingPeriod/beginDateTime/year"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/beginDateTime/month,'00')"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/beginDateTime/day,'00')"/>
				</xsl:variable>
				<xsl:variable name="end">
					<xsl:value-of select="bookingPeriod/endDateTime/year"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/endDateTime/month,'00')"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(bookingPeriod/endDateTime/day,'00')"/>
				</xsl:variable>
				<xsl:attribute name="Start"><xsl:value-of select="$start"/></xsl:attribute>
				<!--xsl:variable name="v1" select="ttVB:FctDateDuration(string($start),string($end))"/>
				<xsl:attribute name="Duration"><xsl:value-of select="$v1"/></xsl:attribute-->
				<xsl:attribute name="End"><xsl:value-of select="$end"/></xsl:attribute>
			</TimeSpan>
			<xsl:choose>
				<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/guaranteeDepositStatusInfo/statusDetails[indicator='GUA'][action='1']">
					<Guarantee>
						<GuaranteesAccepted>
							<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/creditCardInformation">
								<GuaranteeAccepted>
									<PaymentCard>
										<xsl:attribute name="CardCode">
											<xsl:choose>
												<xsl:when test="formOfPayment/vendorCode='CA'"><xsl:value-of select="'MC'"/></xsl:when>
												<xsl:when test="formOfPayment/vendorCode='MA'"><xsl:value-of select="'MC'"/></xsl:when>
												<xsl:when test="formOfPayment/vendorCode='DC'"><xsl:value-of select="'DN'"/></xsl:when>
												<xsl:when test="formOfPayment/vendorCode='AM'"><xsl:value-of select="'AX'"/></xsl:when>
												<xsl:otherwise><xsl:value-of select="formOfPayment/vendorCode"/></xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentCard>
								</GuaranteeAccepted>
							</xsl:for-each>
						</GuaranteesAccepted>
						<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/formOfPaymentDescription/freeText">
							<GuaranteeDescription>
								<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/formOfPaymentDescription/freeText">
									<Text><xsl:value-of select="."/></Text>
								</xsl:for-each>
							</GuaranteeDescription>
						</xsl:if>
					</Guarantee>
				</xsl:when>
				<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/guaranteeDepositStatusInfo/statusDetails[indicator='DP'][action='1']">
					<DepositPayments>
						<RequiredPayment>
							<AcceptedPayments>
								<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/creditCardInformation">
									<AcceptedPayment>
										<PaymentCard>
											<xsl:attribute name="CardCode">
												<xsl:choose>
													<xsl:when test="formOfPayment/vendorCode='CA'"><xsl:value-of select="'MC'"/></xsl:when>
													<xsl:when test="formOfPayment/vendorCode='MA'"><xsl:value-of select="'MC'"/></xsl:when>
													<xsl:when test="formOfPayment/vendorCode='DC'"><xsl:value-of select="'DN'"/></xsl:when>
													<xsl:when test="formOfPayment/vendorCode='AM'"><xsl:value-of select="'AX'"/></xsl:when>
													<xsl:otherwise><xsl:value-of select="formOfPayment/vendorCode"/></xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</PaymentCard>
									</AcceptedPayment>
								</xsl:for-each>
							</AcceptedPayments>
							<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/rulesSection/rulesInformation/ruleDetails[type='6']">
								<Deadline>
									<xsl:attribute name="OffsetTimeUnit">
										<xsl:variable name="offset">
											<xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/rulesSection/rulesInformation/ruleDetails[type='6']/quantityUnit"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$offset='2'"><xsl:value-of select="'Hour'"/></xsl:when>
											<xsl:when test="$offset='3'"><xsl:value-of select="'Day'"/></xsl:when>
											<xsl:when test="$offset='4'"><xsl:value-of select="'Week'"/></xsl:when>
											<xsl:when test="$offset='5'"><xsl:value-of select="'Month'"/></xsl:when>
										</xsl:choose>
									</xsl:attribute>
									<xsl:attribute name="OffsetUnitMultiplier">
										<xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/rulesSection/rulesInformation/ruleDetails[type='6']/quantity"/>
									</xsl:attribute>
								</Deadline>
							</xsl:if>
							<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/formOfPaymentDescription/freeText">
								<PaymentDescription>
									<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='BOO']/bookingRequirementsSection/formOfPaymentDescription/freeText">
										<Text><xsl:value-of select="."/></Text>
									</xsl:for-each>
								</PaymentDescription>
							</xsl:if>
						</RequiredPayment>
					</DepositPayments>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/infoMsgAndCancelPolicies/freeText!=''">
				<CancelPenalties>
					<CancelPenalty>
						<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime[businessSemantic='CAN']">
							<Deadline>
								<xsl:attribute name="AbsoluteDeadline">
									<xsl:value-of select="hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/year"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="format-number(hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/month,'00')"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="format-number(hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/day,'00')"/>
									<xsl:text>T</xsl:text>
									<xsl:value-of select="format-number(hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/hour,'00')"/>
									<xsl:text>:</xsl:text>
									<xsl:value-of select="format-number(hotelPricingCategorySection	[pricingCategory/itemDescriptionType='CXL']/cancellationPoliciesSection/cancellationDateTime	[businessSemantic='CAN']/dateTime/minutes,'00')"/>
									<xsl:text>:00</xsl:text>
								</xsl:attribute>
							</Deadline>
						</xsl:if>
						<PenaltyDescription>
							<xsl:attribute name="Name"><xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/pricingCategory/itemDescription/description"/></xsl:attribute>
							<Text>
								<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='CXL']/infoMsgAndCancelPolicies/freeText">
									<xsl:value-of select="concat(.,' ')"/>
								</xsl:for-each>
							</Text>
						</PenaltyDescription>
					</CancelPenalty>
				</CancelPenalties>
			</xsl:if>
			<Total>
				<xsl:choose>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/typeQualifier='712'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:choose>
								<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount">
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/convertedTotalAmount/conversionRateDetails/convertedValueAmount,'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/totalAmountInformation/monetaryDetails/amount,'.','')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/includedInAmount='I'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:choose>
								<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts">
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts/conversionRateDetails/convertedValueAmount,'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')"/>										</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/includedInAmount='I'">
						<xsl:attribute name="AmountAfterTax">
							<xsl:choose>
								<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/convertedMonetaryAmounts">
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts/conversionRateDetails/convertedValueAmount,'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.','')"/>										</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="AmountBeforeTax">
							<xsl:choose>
								<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts">
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts/conversionRateDetails/convertedValueAmount,'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/amount,'.','')"/>											</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts">
							<xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts/conversionRateDetails/currency"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/currency"/>											</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:choose>
						<xsl:when test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts">
							<xsl:value-of select="string-length(substring-after(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/convertedMonetaryAmounts/conversionRateDetails/convertedValueAmount,'.'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="string-length(substring-after(hotelPricingCategorySection[pricingCategory/itemDescriptionType='RAT']/rateInformationSection/rateAmountInformation/tariffInfo/totalAmount,'.'))"/>						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection/taxFeeInformation/category='TAX' or hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/category='SCH' or hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection/taxFeeInformation/category='TAX'">
					<Taxes>
						<xsl:for-each select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='INC']/taxSection[taxFeeInformation/category='TAX'] | hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection[taxFeeInformation/category='SCH'] | hotelPricingCategorySection[pricingCategory/itemDescriptionType='TTX']/taxSection[taxFeeInformation/category='TAX']">
							<Tax>
								<xsl:attribute name="Type"><xsl:choose><xsl:when test="../totalAmountInformation/monetaryDetails/typeQualifier='712'">Inclusive</xsl:when><xsl:when test="taxFeeInformation/includedInAmount='I'">Inclusive</xsl:when><xsl:otherwise>Exclusive</xsl:otherwise></xsl:choose></xsl:attribute>
								<xsl:attribute name="Code"><xsl:value-of select="taxFeeInformation/code"/></xsl:attribute>
								<xsl:choose>
									<xsl:when test="taxFeeInformation/percentage!=''">
										<xsl:attribute name="Percent"><xsl:value-of select="taxFeeInformation/percentage"/></xsl:attribute>
									</xsl:when>
									<xsl:when test="taxFeeInformation/amount!=''">
										<xsl:attribute name="Amount"><xsl:value-of select="translate(taxFeeInformation/amount,'.','')"/></xsl:attribute>
									</xsl:when>
								</xsl:choose>
								<xsl:if test="taxFeeInformation/longName!=''">
									<TaxDescription>
										<Text>
											<xsl:value-of select="taxFeeInformation/longName"/>
										</Text>
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
				<xsl:attribute name="ChainCode"><xsl:value-of select="hotelPropertyInfo/hotelReference/chainCode"/></xsl:attribute>
				<xsl:attribute name="HotelCode"><xsl:value-of select="hotelPropertyInfo/hotelReference/hotelCode"/></xsl:attribute>
				<xsl:attribute name="HotelCityCode"><xsl:value-of select="hotelPropertyInfo/hotelReference/cityCode"/></xsl:attribute>
				<xsl:attribute name="HotelName"><xsl:value-of select="hotelPropertyInfo/hotelName"/></xsl:attribute>
				<xsl:attribute name="ChainName"><xsl:value-of select="hotelChainInformation/companyName"/></xsl:attribute>
				<xsl:attribute name="HotelCodeContext"><xsl:value-of select="hotelChainInformation/accessLevel"/></xsl:attribute>
				<xsl:if test="hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/otherInfoSection/checkInOutTimeAndExpressInfo or hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/infoMsgAndCancelPolicies/freeText">
					<xsl:variable name="check" select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/otherInfoSection/checkInOutTimeAndExpressInfo"/>
					<xsl:variable name="other" select="hotelPricingCategorySection[pricingCategory/itemDescriptionType='OTH']/infoMsgAndCancelPolicies"/>
					<VendorMessages>
						<VendorMessage>
							<xsl:if test="$check/expressCheckIn='1'">
								<SubSection>
									<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-in Time'"/></xsl:attribute>
									<Paragraph>
										<Text>
											<xsl:value-of select="format-number($check/checkInTimeLimitation/hour,'00')"/>
											<xsl:value-of select="concat(':',format-number($check/checkInTimeLimitation/minutes,'00'))"/>
										</Text>
									</Paragraph>
								</SubSection>
							</xsl:if>
							<xsl:if test="$check/expressCheckOut='1'">
								<SubSection>
									<xsl:attribute name="SubTitle"><xsl:value-of select="'Check-out Time'"/></xsl:attribute>
									<Paragraph>
										<Text>
											<xsl:value-of select="format-number($check/checkOutTimeLimitation/hour,'00')"/>
											<xsl:value-of select="concat(':',format-number($check/checkOutTimeLimitation/minutes,'00'))"/>
										</Text>
									</Paragraph>
								</SubSection>
							</xsl:if>
							<xsl:if test="$other/freeText!=''">
								<SubSection>
									<xsl:attribute name="SubTitle"><xsl:value-of select="'Other Information'"/></xsl:attribute>
									<Paragraph>
										<xsl:for-each select="$other/freeText">
											<Text>
												<xsl:value-of select="."/>
											</Text>
										</xsl:for-each>
									</Paragraph>
								</SubSection>
							</xsl:if>
						</VendorMessage>
					</VendorMessages>
				</xsl:if>
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
			<xsl:attribute name="EffectiveDate"><xsl:value-of select="rateChangePeriodInformation/beginDateTime/year"/><xsl:text>-</xsl:text><xsl:value-of select="format-number(rateChangePeriodInformation/beginDateTime/month,'00')"/><xsl:text>-</xsl:text><xsl:value-of select="format-number(rateChangePeriodInformation/beginDateTime/day,'00')"/></xsl:attribute>
			<xsl:attribute name="ExpireDate"><xsl:value-of select="rateChangePeriodInformation/endDateTime/year"/><xsl:text>-</xsl:text><xsl:value-of select="format-number(rateChangePeriodInformation/endDateTime/month,'00')"/><xsl:text>-</xsl:text><xsl:value-of select="format-number(rateChangePeriodInformation/endDateTime/day,'00')"/></xsl:attribute>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="chgOfRateConvertedAmnt">
								<xsl:value-of select="translate(chgOfRateConvertedAmnt/conversionRateDetails/convertedValueAmount,'.','')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="translate(rateChangeAmountInformation/monetaryDetails/amount,'.','')"/>
							</xsl:otherwise>
						</xsl:choose>
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
				<xsl:choose>
					<xsl:when test="displayValue!=''">
						<xsl:value-of select="displayValue"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="loadedValue"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</AdditionalDetail>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="maxRate">
		<AdditionalDetail>
			<xsl:attribute name="Code">MaximumRate</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:choose>
					<xsl:when test="displayValue!=''">
						<xsl:value-of select="displayValue"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="loadedValue"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</AdditionalDetail>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="Error">
		<Error Type="Amadeus">
			<xsl:value-of select="."/>
		</Error>
	</xsl:template>

	<xsl:template match="messageErrorList">
		<Error Type="Amadeus">
			<xsl:value-of select="errorFreeText/freeText"/>
			<xsl:value-of select="errorFreeText/freetext"/>
		</Error>
	</xsl:template>
	
	<xsl:template match="errorInformation">
		<Error Type="Amadeus">
			<xsl:value-of select="errorDescription/freeText"/>
		</Error>
	</xsl:template>
	<!-- *********************************************************************************************************  -->
	<xsl:template match="Hotel_FeaturesReply">
		<Address>
			<AddressLine>
				<xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1A']/description"/>
			</AddressLine>
			<CityName>
				<xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1C']/description"/>
			</CityName>
			<PostalCode>
				<xsl:value-of select="hotelFeaturesTerms/featuresTerms[category='1F']/description"/>
			</PostalCode>
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
	<msxsl:script language="VisualBasic" implements-prefix="ttVB"><![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]></msxsl:script>
</xsl:stylesheet>
