<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_HotelAvailRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 20 Feb 2011 - Rastko - added corporate discount mapping				-->
	<!-- Date: 29 May 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:apply-templates select="HotelCompleteAvailability_9_0_2"/>
			<xsl:apply-templates select="HotelAvailability_11_0_2"/>
			<xsl:apply-templates select="GalileoError"/>
		</OTA_HotelAvailRS>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelCompleteAvailability_9_0_2">
		<xsl:choose>
			<xsl:when test="HotelCompleteAvailability/HotelError">
				<Errors>
					<Error Type="Galileo">
						<xsl:attribute name="Code"><xsl:value-of select="HotelCompleteAvailability/HotelError/ErrMsgAry/ErrMsg/Num"/></xsl:attribute>
						<xsl:value-of select="HotelCompleteAvailability/HotelError/ErrMsgAry/ErrMsg/Msg"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<xsl:apply-templates select="HotelCompleteAvailability"/>
			</xsl:otherwise>
		</xsl:choose>
		<!--xsl:apply-templates select="HotelCompleteAvailability/NoErrQual" mode="more"/-->
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelAvailability_11_0_2">
		<xsl:choose>
			<xsl:when test="HostApplicationError/ErrorCode !=''">
				<Errors>
					<Error Type="Galileo">
						<xsl:attribute name="Code"><xsl:value-of select="HostApplicationError/ErrorCode"/></xsl:attribute>
						<xsl:value-of select="HostApplicationError/Text"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="TransactionErrorCode/Code !=''">
				<Errors>
					<Error Type="Galileo">
						<xsl:attribute name="Code"><xsl:value-of select="TransactionErrorCode/Code"/></xsl:attribute>
						<xsl:value-of select="TransactionErrorCode/Domain"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="HotelAvailability/HotelError and not(HotelAvailability/HotelPropHeader)">
				<Errors>
					<Error Type="Galileo">
						<xsl:attribute name="Code"><xsl:value-of select="HotelAvailability/HotelError/ErrMsgAry/ErrMsg/Num"/></xsl:attribute>
						<xsl:value-of select="HotelAvailability/HotelError/ErrMsgAry/ErrMsg/Msg"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="HotelSimilarNames">
				<Success/>
				<Warnings>
					<Warning Type="Galileo">REFERENCE POINT SIMILAR NAME LIST</Warning>
				</Warnings>
				<Criteria>
					<xsl:apply-templates select="HotelSimilarNames/SmlrNameAry/SmlrName"/>
				</Criteria>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<RoomStays>
					<xsl:if test="HotelAvailability[position()=last()]/HotelGetMore/MoreInd = 'Y'">
						<xsl:apply-templates select="HotelAvailability[position()=last()]/HotelGetMore"/>
					</xsl:if>
					<xsl:apply-templates select="HotelAvailability"/>
				</RoomStays>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- **********************************************************************************************************************-->
	<!-- ********************************************************************************************************************* -->
	<!-- From this point down, these template matches are for the HotelCompleteAvailability_9_0_2 message -->
	<!-- ********************************************************************************************************************* -->
	<!-- ********************************************************************************************************************* -->
	<xsl:template match="HotelCompleteAvailability">
		<RoomStays>
			<RoomStay>
				<RatePlans>
					<xsl:apply-templates select="HotelRate"/>
				</RatePlans>
				<GuestCounts IsPerRoom="1">
					<GuestCount>
						<xsl:attribute name="Count"><xsl:value-of select="HtlAvailHeader/NumPersons"/></xsl:attribute>
					</GuestCount>
				</GuestCounts>
				<TimeSpan>
					<xsl:attribute name="Start"><xsl:value-of select="substring(string(HtlAvailHeader/StartDt),1,4)"/>-<xsl:value-of select="substring(string(HtlAvailHeader/StartDt),5,2)"/>-<xsl:value-of select="substring(string(HtlAvailHeader/StartDt),7,2)"/></xsl:attribute>
					<xsl:attribute name="Duration"><xsl:value-of select="HtlAvailHeader/NumNights"/></xsl:attribute>
					<xsl:attribute name="End"><xsl:value-of select="substring(string(HtlAvailHeader/EndDt),1,4)"/>-<xsl:value-of select="substring(string(HtlAvailHeader/EndDt),5,2)"/>-<xsl:value-of select="substring(string(HtlAvailHeader/EndDt),7,2)"/></xsl:attribute>
				</TimeSpan>
				<xsl:if test="HotelCorporateDiscount[1]/CorporateDiscCodeAry">
					<Discount>
						<xsl:attribute name="DiscountCode">
							<xsl:value-of select="HotelCorporateDiscount[1]/CorporateDiscCodeAry/CorporateDiscCodeItem[1]/CorporateDiscCode"/>
						</xsl:attribute>
						<DiscountReason>
							<xsl:element name="Text">
								<xsl:value-of select="HotelCorporateDiscount[1]/CorporateDiscCodeAry/CorporateDiscCodeItem[1]/MultiLevelRateCode"/>
							</xsl:element>
						</DiscountReason>
					</Discount>
				</xsl:if>
				<Total>
					<xsl:choose>
						<xsl:when test="HotelAlternateRates[1]/AltCrncy!=''">
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="HotelAlternateRates[1]/AltCrncy" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="HotelAlternateRates[1]/NumDec" />
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="HotelRate[1]/StoredCrncy" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="HotelRate[1]/StoredNumDec" />
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</Total>
				<BasicPropertyInfo>
					<xsl:attribute name="ChainCode"><xsl:value-of select="HotelPropertyRecord/Vnd"/></xsl:attribute>
					<xsl:attribute name="HotelCode"><xsl:value-of select="HotelPropertyRecord/RMID"/></xsl:attribute>
					<xsl:attribute name="HotelCityCode"><xsl:value-of select="HotelPropHeader/City"/></xsl:attribute>
					<xsl:attribute name="HotelName"><xsl:value-of select="HotelPropertyRecord/PropName"/></xsl:attribute>
					<xsl:attribute name="HotelCodeContext"><xsl:value-of select="HotelType/HtlTypeInd"/></xsl:attribute>
					<xsl:if test="HotelVendorMarketing/VMsg!=''">
						<VendorMessages>
							<VendorMessage>
								<xsl:attribute name="Title"><xsl:value-of select="HotelVendorMarketing/VMsg"/></xsl:attribute>
							</VendorMessage>
						</VendorMessages>
					</xsl:if>
					<xsl:if test="HotelPropertyRecord/ShortAddr!='' or HotelPropHeader/State!=''">
						<Address>
							<AddressLine>
								<xsl:value-of select="HotelPropertyRecord/ShortAddr"/>
							</AddressLine>
							<xsl:if test="HotelPropHeader/State!=''">
								<StateProv>
									<xsl:attribute name="StateCode"><xsl:value-of select="HotelPropHeader/State"/></xsl:attribute>
								</StateProv>
							</xsl:if>
						</Address>
					</xsl:if>
					<xsl:if test="HotelPropertyRecord/Dir!='' or HotelPropertyRecord/Dist!='' or HotelPropertyRecord/Locn!=''">
						<RelativePosition>
							<xsl:attribute name="Direction"><xsl:value-of select="HotelPropertyRecord/Dir"/></xsl:attribute>
							<xsl:attribute name="Distance"><xsl:value-of select="HotelPropertyRecord/Dist"/><xsl:value-of select="HotelPropHeader/MileKmInd"/></xsl:attribute>
							<xsl:attribute name="IndexPointCode"><xsl:choose><xsl:when test="HotelPropertyRecord/Locn='S'">U</xsl:when><xsl:when test="HotelPropertyRecord/Locn=''">A</xsl:when><xsl:otherwise><xsl:value-of select="HotelPropertyRecord/Locn"/></xsl:otherwise></xsl:choose></xsl:attribute>
							<xsl:if test="HotelPropertyRecord/Transportation!=''">
								<Transportations>
									<Transportation>
										<xsl:attribute name="TransportationCode"><xsl:value-of select="HotelPropertyRecord/Transportation"/></xsl:attribute>
									</Transportation>
								</Transportations>
							</xsl:if>
						</RelativePosition>
					</xsl:if>
				</BasicPropertyInfo>
			</RoomStay>
		</RoomStays>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="ErrQual">
		<Errors>
			<!--Come back and see what valid values are for @Type -->
			<Error Type="H">
				<xsl:attribute name="Code"><xsl:value-of select="Num"/></xsl:attribute>
				<xsl:value-of select="Msg"/>
			</Error>
		</Errors>
	</xsl:template>

	<xsl:template match="GalileoError">
		<Errors>
			<Error Type="Galileo">
				<xsl:attribute name="Code">9000</xsl:attribute>
				<xsl:value-of select="."/>
			</Error>
		</Errors>
	</xsl:template>

	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelRate">
		<RatePlan>
			<xsl:attribute name="BookingCode"><xsl:value-of select="BIC"/></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="DispRate"/></xsl:attribute>
			<xsl:if test="RateCatInd != ''">
				<xsl:attribute name="RatePlanType"><xsl:value-of select="RateCatInd"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="RateChgInd = 'Y'">
				<xsl:attribute name="RateIndicator"><xsl:text>ChangeDuringStay</xsl:text></xsl:attribute>
			</xsl:if>
			<xsl:if test="RateGuarInd !=''">
				<Guarantee>
					<xsl:choose>
						<xsl:when test="RateGuarInd='Y'">
							<xsl:attribute name="GuaranteeType">Rate is guaranteed</xsl:attribute>
						</xsl:when>
					</xsl:choose>
				</Guarantee>
			</xsl:if>
			<RatePlanDescription>
				<Text><xsl:value-of select="following-sibling::HotelRateDescription[1]/Desc"/></Text>
				<xsl:if test="following-sibling::HotelRateDescription[position() = 2]">
					<Text><xsl:value-of select="following-sibling::HotelRateDescription[position() = 2]/Desc"/></Text>
				</xsl:if>
				<xsl:if test="following-sibling::HotelRateDescription[position() = 3]">
					<Text><xsl:value-of select="following-sibling::HotelRateDescription[position() = 3]/Desc"/></Text>
				</xsl:if>
			</RatePlanDescription>
			<RestrictionStatus>
				<xsl:attribute name="Status">
					<xsl:choose>
						<xsl:when test="AvailNeedInd ='N'">Close</xsl:when>
						<xsl:otherwise>Open</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</RestrictionStatus>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="following-sibling::HotelAlternateRates[1]/AltCrncy!=''">
								<xsl:value-of select="following-sibling::HotelAlternateRates[1]/AltRateAmt"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="VStoredRateAmt"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</AdditionalDetail>
			</AdditionalDetails>
		</RatePlan>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="BICInfo1" mode="Standard">
		<RatePlan>
			<xsl:attribute name="BookingCode"><xsl:value-of select="BIC"/></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="substring(string(BIC),3,3)"/></xsl:attribute>
			<xsl:if test="RateChgInd ='Y'">
				<xsl:attribute name="RateIndicator">ChangeDuringStay</xsl:attribute>
			</xsl:if>
			<xsl:if test="../../../HotelCompleteAvailability/NoErrQual/DepInd!=''">
				<Guarantee>
					<xsl:choose>
						<xsl:when test="../../../HotelCompleteAvailability/NoErrQual/DepInd='D'">
							<xsl:attribute name="GuaranteeType">Deposit</xsl:attribute>
						</xsl:when>
						<xsl:when test="../../../HotelCompleteAvailability/NoErrQual/DepInd='G'">
							<xsl:attribute name="GuaranteeType">GuaranteeRequired</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="HoldTime"><xsl:value-of select="../../../HotelCompleteAvailability/NoErrQual/DepInd"/>:00:00</xsl:attribute>						
						</xsl:otherwise>
					</xsl:choose>
				</Guarantee>
			</xsl:if>
			<!--I think the RoomDescription i.e. King, Double etc  should be in RoomStay/RoomTypes/RoomType/RoomDescription/@Name but because of the way the schema is, it would not make sense to put it there. RoomTypes can occur multiple times and so can RatePlans (Booking Code is under RatePlan, as well as other items such as MealsIncluded, RestrictionStatus, RatePlanCode ) -->
			<!--Also, ask about putting the RoomType description like it is in RoomTypes lookup table -->
			<Commission>
				<xsl:attribute name="StatusType"><xsl:choose><xsl:when test="CommissionInd='Y'">Full</xsl:when><xsl:otherwise>Non-paying</xsl:otherwise></xsl:choose></xsl:attribute>
			</Commission>
			<!--Ask about Meals - OTA only has option for Breakfast, Lunch or Dinner - I am assuming Breakfast -->
			<xsl:if test="MealPlanInd='M'">
				<MealsIncluded>
					<xsl:attribute name="Breakfast">1</xsl:attribute>
				</MealsIncluded>
			</xsl:if>
			<RestrictionStatus>
				<xsl:attribute name="Status"><xsl:choose><xsl:when test="AvailNeedInd='N'">Close</xsl:when><xsl:otherwise>Open</xsl:otherwise></xsl:choose></xsl:attribute>
			</RestrictionStatus>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount"><xsl:value-of select="RateAmt"/></xsl:attribute>
				</AdditionalDetail>
			</AdditionalDetails>
			<!--RateCategory>!func:Decode(GxsHotelRateCategoryR,<xsl:value-of select="RateCatInd"/>,<xsl:value-of select="RateCatInd"/>)</RateCategory>
			<RoomTypeCode>
				<xsl:value-of select="substring(string(BIC),1,3)"/>
			</RoomTypeCode>
			<RoomTypeDescription>!func:Decode(RoomTypes,<xsl:value-of select="substring(string(BIC),1,3)"/>)</RoomTypeDescription-->
			<!--I didn't know what to map this to since -->
			<!--xsl:if test="CancelPolicyInd='P'">
				<CancelPolicy>Y</CancelPolicy>
			</xsl:if-->
		</RatePlan>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="BICInfo" mode="Inside">
		<RatePlan>
			<xsl:attribute name="BookingCode"><xsl:value-of select="BIC"/></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="substring(string(BIC),4,3)"/></xsl:attribute>
			<xsl:if test="RateChgInd ='Y'">
				<xsl:attribute name="RateIndicator"><xsl:value-of select="ChangeDuringStay"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="RateDescAry/RateDesc[position()=2]/Desc!=''">
				<xsl:apply-templates select="RateDescAry/RateDesc[position()=2]/Desc" mode="RC"/>
			</xsl:if>
			<xsl:if test="//DepInd!=''">
				<Guarantee>
					<xsl:choose>
						<xsl:when test="//DepInd='D'">
							<xsl:attribute name="GuaranteeType">Deposit</xsl:attribute>
						</xsl:when>
						<xsl:when test="//DepInd='G'">
							<xsl:attribute name="GuaranteeType">GuaranteeRequired</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="HoldTime"><xsl:value-of select="../../../HotelCompleteAvailability/NoErrQual/DepInd"/>:00:00</xsl:attribute>		
						</xsl:otherwise>
					</xsl:choose>
				</Guarantee>
			</xsl:if>
			<RestrictionStatus>
				<xsl:attribute name="Status">
					<xsl:choose>
						<xsl:when test="RoomTypeReqInd='Y'">Close</xsl:when>
						<xsl:otherwise>Open</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</RestrictionStatus>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount"><xsl:value-of select="StoredRateAmt"/></xsl:attribute>
				</AdditionalDetail>
			</AdditionalDetails>
			<!--RoomTypeCode>
				<xsl:value-of select="substring(string(BIC),1,3)"/>
			</RoomTypeCode-->
			<!--xsl:if test="RateDescAry/RateDesc[position()=1]/Desc!=''">
				<RoomTypeDescription>
					<xsl:value-of select="RateDescAry/RateDesc[position()=1]/Desc"/>
				</RoomTypeDescription>
			</xsl:if-->
			<!--Couldn't find somewher to put AltRateAmt -->
			<!--xsl:if test="AltRateAmt!=''">
				<AlternateCurrencyAmount>
					<xsl:value-of select="AltRateAmt"/>
				</AlternateCurrencyAmount>
			</xsl:if-->
		</RatePlan>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="Desc" mode="RC">
		<xsl:choose>
			<xsl:when test="contains(string(.),'RACK')">
				<xsl:attribute name="RatePlanID">RAC</xsl:attribute>
			</xsl:when>
			<xsl:when test="contains(string(.),'CORPORATE')">
				<xsl:attribute name="RatePlanID">COR</xsl:attribute>
			</xsl:when>
			<xsl:when test="contains(string(.),'WEEKEND')">
				<xsl:attribute name="RatePlanID">WKD</xsl:attribute>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="NoErrQual" mode="more">
		<xsl:if test="MoreInd = 'Y'">
			<RoomStays>
				<xsl:attribute name="MoreIndicator"><xsl:value-of select="MoreType"/><xsl:value-of select="VKeys"/></xsl:attribute>
			</RoomStays>
		</xsl:if>
	</xsl:template>
	<!-- **********************************************************************************************************************-->
	<!-- **********************************************************************************************************************-->
	<!-- ********************************************************************************************************************* -->
	<!-- From this point down, these template matches are for the HotelAvailability_11_0_2 message                -->
	<!-- This is the response for availability for MULTIPLE hotels (i.e. Hotel Search )			          -->
	<!-- ********************************************************************************************************************* -->
	<!-- ********************************************************************************************************************* -->
	<xsl:template match="HotelAvailability">
			<xsl:apply-templates select="HotelInsideShopProperty[PropInd='A']" mode="Main"/>
	</xsl:template>
	<!-- *********************************************************************************************************** -->

	<xsl:template match="HotelGetMore">
		<xsl:attribute name="MoreIndicator">
			<xsl:value-of select="TotNumProps"/>
			<xsl:text>A</xsl:text>
			<xsl:value-of select="StartDBKey"/>
			<xsl:text>B</xsl:text>
			<xsl:value-of select="EndDBKey"/>
			<xsl:text>C</xsl:text>
			<xsl:value-of select="RefPtDBKey"/>
			<xsl:text>D</xsl:text>
			<xsl:value-of select="BitMap"/>
			<xsl:text>E</xsl:text>
			<xsl:value-of select="SubtotalNumPropsRet"/>
			<xsl:text>F</xsl:text>
			<xsl:value-of select="DBPropTok"/>
			<xsl:text>G</xsl:text>
			<xsl:value-of select="DBPropKey"/>
		</xsl:attribute>
	</xsl:template>
	
	<xsl:template match="TypeEQual">
		<Errors>
			<Error Type="Hotel">
				<xsl:attribute name="Code"><xsl:value-of select="Num"/></xsl:attribute>
				<xsl:value-of select="Msg"/>
			</Error>
		</Errors>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="HotelInsideShopProperty" mode="Main">
		<RoomStay>
			<RatePlans>
				<RatePlan>
					<xsl:if test="DepInd!=''">
						<Guarantee>
							<xsl:attribute name="GuaranteeType">
								<xsl:choose>
									<xsl:when test="DepInd='G'">Guarantee required</xsl:when>
									<xsl:when test="DepInd='D'">Deposit required</xsl:when>
									<xsl:otherwise>
										<xsl:text>Hold time </xsl:text>
										<xsl:value-of select="DepInd"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</Guarantee>
					</xsl:if>
					<AdditionalDetails>
						  <AdditionalDetail Code="MinimumRate">
						  	<xsl:choose>
								<xsl:when test="following-sibling::HotelAlternateCurrency[1]/AltCurrency!=''">
									<xsl:attribute name="Amount"><xsl:value-of select="following-sibling::HotelAlternateCurrency[1]/LowRate"/></xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount"><xsl:value-of select="following-sibling::HotelInsideShopRate[1]/LowRateAmt"/></xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						  </AdditionalDetail>
						  <AdditionalDetail Code="MaximumRate">
						  	<xsl:choose>
								<xsl:when test="following-sibling::HotelAlternateCurrency[1]/AltCurrency!=''">
									<xsl:attribute name="Amount"><xsl:value-of select="following-sibling::HotelAlternateCurrency[1]/HighRate"/></xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount"><xsl:value-of select="following-sibling::HotelInsideShopRate[1]/HighRateAmt"/></xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>	  	
						  </AdditionalDetail>
					</AdditionalDetails>
				</RatePlan>			
			</RatePlans>
			<GuestCounts IsPerRoom="1">
				<GuestCount>
					<xsl:attribute name="Count"><xsl:value-of select="../HtlAvailHeader/NumPersons"/></xsl:attribute>
				</GuestCount>
			</GuestCounts>
			<TimeSpan>
				<xsl:attribute name="Start"><xsl:value-of select="substring(string(../HtlAvailHeader/StartDt),1,4)"/>-<xsl:value-of select="substring(string(../HtlAvailHeader/StartDt),5,2)"/>-<xsl:value-of select="substring(string(../HtlAvailHeader/StartDt),7,2)"/></xsl:attribute>
				<xsl:attribute name="Duration"><xsl:value-of select="../HtlAvailHeader/NumNights"/></xsl:attribute>
				<xsl:attribute name="End"><xsl:value-of select="substring(string(../HtlAvailHeader/EndDt),1,4)"/>-<xsl:value-of select="substring(string(../HtlAvailHeader/EndDt),5,2)"/>-<xsl:value-of select="substring(string(../HtlAvailHeader/EndDt),7,2)"/></xsl:attribute>
			</TimeSpan>
			<xsl:if test="following-sibling::HotelCorporateDiscount[1]/CorporateDiscCodeAry">
				<Discount>
					<xsl:attribute name="DiscountCode">
						<xsl:value-of select="following-sibling::HotelCorporateDiscount[1]/CorporateDiscCodeAry/CorporateDiscCodeItem[1]/CorporateDiscCode"/>
					</xsl:attribute>
					<DiscountReason>
						<xsl:element name="Text">
							<xsl:value-of select="following-sibling::HotelCorporateDiscount[1]/CorporateDiscCodeAry/CorporateDiscCodeItem[1]/MultiLevelRateCode"/>
						</xsl:element>
					</DiscountReason>
				</Discount>
			</xsl:if>
			<Total>
				<xsl:choose>
					<xsl:when test="following-sibling::HotelAlternateCurrency[1]/AltCurrency!=''">
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="following-sibling::HotelAlternateCurrency[1]/AltCurrency"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="following-sibling::HotelAlternateCurrency[1]/NumDec"/></xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="following-sibling::HotelInsideShopRate[1]/Crncy"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="following-sibling::HotelInsideShopRate[1]/NumDec"/></xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</Total>
			<BasicPropertyInfo>
				<xsl:attribute name="ChainCode"><xsl:value-of select="Vnd"/></xsl:attribute>
				<xsl:attribute name="HotelCode"><xsl:value-of select="PropNum"/></xsl:attribute>
				<xsl:attribute name="HotelCityCode"><xsl:value-of select="City"/></xsl:attribute>
				<xsl:variable name="chcode">
					<xsl:if test="string-length(PropNum) = 4">0</xsl:if>
					<xsl:value-of select="PropNum"/>
				</xsl:variable>
				<xsl:attribute name="HotelName">
					<xsl:choose>
						<xsl:when test="//ImageSetResponse/Property[@Id = $chcode]/Description/GeneralInformation/HotelName != ''">
							<xsl:value-of select="//ImageSetResponse/Property[@Id = $chcode]/Description/GeneralInformation/HotelName"/>
						</xsl:when>
						<xsl:when test="PropFullName != ''">
							<xsl:value-of select="PropFullName"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="PropName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="ChainName"></xsl:attribute>
				<VendorMessages>
					<VendorMessage Title="Link Indicator">
						<xsl:attribute name="InfoType">
							<xsl:choose>
								<xsl:when test="LinkInd='I'">Inside Availability Link</xsl:when>
								<xsl:when test="LinkInd='L'">Link Partner</xsl:when>
								<xsl:when test="LinkInd='R'">RoomMaster Only Participant</xsl:when>
								<xsl:when test="LinkInd='S'">Inside Shopper Participant</xsl:when>
							</xsl:choose>
						</xsl:attribute>
					</VendorMessage>
					<VendorMessage Title="Inside shopper indicator">
						<xsl:attribute name="InfoType"><xsl:value-of select="InsideShopInd"/></xsl:attribute>
					</VendorMessage>
					<VendorMessage Title="Spectrum advertiser participant">
						<xsl:attribute name="InfoType"><xsl:value-of select="SpectrumAdvertInd"/></xsl:attribute>
					</VendorMessage>
				</VendorMessages>
				<xsl:apply-templates select="//ImageSetResponse/Property[@Id = $chcode]/Description/GeneralInformation"></xsl:apply-templates>
				<xsl:if test="AAARating!=''">
					<Award Provider="AAA">
						<xsl:attribute name="Rating"><xsl:value-of select="AAARating"/></xsl:attribute>
					</Award>
				</xsl:if>
				<RelativePosition>
					<xsl:attribute name="Direction"><xsl:value-of select="Dir"/></xsl:attribute>
					<xsl:attribute name="Distance"><xsl:value-of select="Dist"/><xsl:value-of select="../HotelPropHeader/MileKmInd"/></xsl:attribute>							
					<Transportations>
						<Transportation>
							<xsl:attribute name="TransportationCode"><xsl:value-of select="TransportInd"/></xsl:attribute>
						</Transportation>
						<Transportation>
							<Descriptions>
								<Description Name="Description">
									<xsl:apply-templates select="//ImageSetResponse/Property[@Id = $chcode]/Description/KeywordsInformation/Keyword[@Description='HOTEL DESCRIPTION']/KeywordInformation/Text"/>
								</Description>
								<Description Name="Facilities">
									<xsl:apply-templates select="//ImageSetResponse/Property[@Id = $chcode]/Description/KeywordsInformation/Keyword[@Description='FACILITIES']/KeywordInformation/Text"/>
								</Description>
							</Descriptions>
						</Transportation>
						<xsl:apply-templates select="//ImageSetResponse/Property[@Id = $chcode]/Photos/Photo"/>
					</Transportations>
				</RelativePosition>
			</BasicPropertyInfo>
		</RoomStay>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	
	<xsl:template match="Photo">
		<Transportation>
			<Descriptions>
				<Description Name="Photo">
					<URL><xsl:value-of select="URL"/></URL>
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
		</Transportation>	
	</xsl:template>

	<xsl:template match="Text">
		<xsl:if test="string(.) != ''">
			<Text>
				<xsl:value-of select="."/>
			</Text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="GeneralInformation">
		<Address>
			<AddressLine>
				<xsl:choose>
					<xsl:when test="contains(Address/Line[1],'@')"><xsl:value-of select="substring-before(Address/Line[1],'@')"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="Address/Line[1]"/></xsl:otherwise>
				</xsl:choose>
			</AddressLine>
			<AddressLine><xsl:value-of select="Address/Line[2]"/></AddressLine>
			<xsl:if test="Address/Line[3] != ''">
				<AddressLine><xsl:value-of select="Address/Line[3]"/></AddressLine>
			</xsl:if>
			<!--CityName><xsl:value-of select="substring-before(Address/Line[2],'@')"/></CityName>
			<xsl:variable name="line2">
				<xsl:value-of select="substring-after(Line[2],'@')"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="substring-after($line2,'@') != ''">
					<PostalCode><xsl:value-of select="substring-before(substring-after($line2,'@'),'@')"/></PostalCode>
					<StateProv>
						<xsl:attribute name="StateCode"><xsl:value-of select="substring-before($line2,'@')"/></xsl:attribute>
					</StateProv>
				</xsl:when>
				<xsl:otherwise>
					<PostalCode><xsl:value-of select="substring-before($line2,'@')"/></PostalCode>
				</xsl:otherwise>
			</xsl:choose>
			<CountryName>
				<xsl:attribute name="Code"><xsl:value-of select="substring-before(Line[3],'@')"/></xsl:attribute>
			</CountryName-->
		</Address>
		<ContactNumbers>
			<ContactNumber>
				<xsl:attribute name="PhoneTechType">Telephone</xsl:attribute>
				<xsl:attribute name="PhoneNumber"><xsl:value-of select="Phone"/></xsl:attribute>
			</ContactNumber>
			<ContactNumber>
				<xsl:attribute name="PhoneTechType">Fax</xsl:attribute>
				<xsl:attribute name="PhoneNumber"><xsl:value-of select="Fax"/></xsl:attribute>
			</ContactNumber>
		</ContactNumbers>
	</xsl:template>
	
	<xsl:template match="BICInfo">
		<RatePlan>
			<xsl:attribute name="BookingCode"><xsl:value-of select="BIC"/></xsl:attribute>
			<xsl:attribute name="RatePlanCode"><xsl:value-of select="substring(string(BIC),4,3)"/></xsl:attribute>
			<xsl:if test="RateChgInd ='Y'">
				<xsl:attribute name="RateIndicator">ChangeDuringStay</xsl:attribute>
			</xsl:if>
			<xsl:if test="RateDescAry/RateDesc[position()=2]/Desc!=''">
				<xsl:apply-templates select="RateDescAry/RateDesc[position()=2]/Desc" mode="RC"/>
			</xsl:if>
			<xsl:if test="//DepInd!=''">
				<Guarantee>
					<xsl:choose>
						<xsl:when test="//DepInd='D'">
							<xsl:attribute name="GuaranteeType">Deposit</xsl:attribute>
						</xsl:when>
						<xsl:when test="//DepInd='G'">
							<xsl:attribute name="GuaranteeType">GuaranteeRequired</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="hold">
								<xsl:value-of select="//DepInd"/>
							</xsl:variable>
							<xsl:attribute name="HoldTime"><xsl:value-of select="$hold + 12"/></xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</Guarantee>
			</xsl:if>
			<RestrictionStatus>
				<xsl:attribute name="Status"><xsl:choose><xsl:when test="RoomTypeReqInd='Y'">Close</xsl:when><xsl:otherwise>Open</xsl:otherwise></xsl:choose></xsl:attribute>
			</RestrictionStatus>
			<AdditionalDetails>
				<AdditionalDetail>
					<xsl:attribute name="Amount"><xsl:value-of select="VStoredRateAmt"/></xsl:attribute>
				</AdditionalDetail>
			</AdditionalDetails>
			<!--RoomTypeCode>
				<xsl:value-of select="substring(string(BIC),1,3)"/>
			</RoomTypeCode-->
			<!--xsl:if test="RateDescAry/RateDesc[position()=1]/Desc!=''">
				<RoomTypeDescription>
					<xsl:value-of select="RateDescAry/RateDesc[position()=1]/Desc"/>
				</RoomTypeDescription>
			</xsl:if-->
			<!--Couldn't find somewher to put AltRateAmt -->
			<!--xsl:if test="AltRateAmt!=''">
				<AlternateCurrencyAmount>
					<xsl:value-of select="AltRateAmt"/>
				</AlternateCurrencyAmount>
			</xsl:if-->
		</RatePlan>
	</xsl:template>
	
	<xsl:template match="SmlrName">
		<Criterion>
			<Address>
				<CityName><xsl:value-of select="City"/></CityName>
				<StateProv><xsl:value-of select="State"/></StateProv>
			</Address>
			<RefPoint><xsl:value-of select="PropName"/></RefPoint>
		</Criterion>	
	</xsl:template>
	<!-- *********************************************************************************************************** -->
</xsl:stylesheet>
