<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
	<!-- Amadeus_UpdateTSTRS.xsl													  -->
	<!-- ================================================================== -->
	<!-- Date: 07 April 2008															  -->
	<!-- ================================================================== -->
		
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="PoweredTicket_UpdateTSTReply"/>
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_StoredFareUpdateRS>
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_StoredFareUpdateRS>
</xsl:template>
	
<xsl:template match="CAPI_Messages">
	<Error>
		<xsl:attribute name="Code">
			<xsl:value-of select="ErrorCode" />
		</xsl:attribute>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="Text" />
	</Error>
</xsl:template>

<xsl:template match="PoweredTicket_UpdateTSTReply">
	<OTA_StoredFareUpdateRS>
		<xsl:choose>
			<xsl:when test="fareList">
				<Success/>
				<xsl:if test="Error or Warning">
					<Warnings>
						<xsl:apply-templates select="Error" mode="warning"/>
						<xsl:apply-templates select="Warning" mode="warning"/>
					</Warnings>
				</xsl:if>
				<xsl:apply-templates select="scrollingInformation"/>
				<xsl:apply-templates select="fareList"/>
			</xsl:when>
			<xsl:when test="Error">
				<Errors>
					<xsl:apply-templates select="Error" mode="error"/>
				</Errors>
			</xsl:when>
		</xsl:choose>			
	</OTA_StoredFareUpdateRS>		
</xsl:template>	

<xsl:template match="Error" mode="error">
	<Error Type="Amadeus">
		<xsl:value-of select="."/>
	</Error>
</xsl:template>

<xsl:template match="Text">
	<Text>
		<xsl:value-of select="."/>
	</Text>
</xsl:template>

<xsl:template match="Error | Warning" mode="warning">
	<Warning Type="Amadeus">
		<xsl:value-of select="."/>
	</Warning>
</xsl:template>

<xsl:template match="scrollingInformation">
	<MoreIndicator>
		<xsl:attribute name="NbOfRemainingItems"><xsl:value-of select="nextListInformation/remainingInformation"/></xsl:attribute>
		<xsl:attribute name="LastItemReference"><xsl:value-of select="nextListInformation/remainingReference"/></xsl:attribute>
	</MoreIndicator>
</xsl:template>

<xsl:template match="fareList">
	<StoredFareInformation>
		<xsl:attribute name="SalesIndicator"><xsl:value-of select="pricingInformation/salesIndicator"/></xsl:attribute>
		<xsl:attribute name="FareIndicator"><xsl:value-of select="pricingInformation/tstInformation/tstIndicator"/></xsl:attribute>
		<xsl:attribute name="FareCalculationMode"><xsl:value-of select="pricingInformation/fcmi"/></xsl:attribute>
		<xsl:attribute name="ValidatingCarrierCode"><xsl:value-of select="validatingCarrier/carrierInformation/carrierCode"/></xsl:attribute>
		<StoredFareReference>
			<xsl:attribute name="Qualifier"><xsl:value-of select="fareReference/referenceType"/></xsl:attribute>
			<xsl:attribute name="ID"><xsl:value-of select="fareReference/iDDescription/iDSequenceNumber"/></xsl:attribute>
			<xsl:attribute name="UniqueRef"><xsl:value-of select="fareReference/uniqueReference"/></xsl:attribute>
		</StoredFareReference>
	</StoredFareInformation>
	<xsl:apply-templates select="lastTktDate"/>
	<xsl:apply-templates select="paxSegReference/refDetails"/>
	<xsl:apply-templates select="fareDataInformation/fareDataMainInformation"/>
	<xsl:apply-templates select="fareDataInformation/fareDataSupInformation"/>
	<xsl:apply-templates select="taxInformation"/>
	<xsl:apply-templates select="bankerRates/firstRateDetail"/>
	<xsl:apply-templates select="bankerRates/secondRateDetail"/>
	<xsl:apply-templates select="passengerInformation[penDisInformation/infoQualifier='700']"/>
	<xsl:apply-templates select="passengerInformation[penDisInformation/infoQualifier='701']"/>
	<xsl:apply-templates select="originDestination/cityCode"/>
	<xsl:apply-templates select="segmentInformation"/>
	<xsl:apply-templates select="otherPricingInfo"/>
	<xsl:apply-templates select="statusInformation"/>
	<xsl:apply-templates select="carrierFeesGroup"/>
	<xsl:apply-templates select="contextualFop/formOfPayment"/>
	<xsl:apply-templates select="automaticReissueInfo"/>
	<xsl:apply-templates select="mileage"/>
</xsl:template>



<xsl:template match="lastTktDate">
	<FareValidity>
		<xsl:attribute name="ValidityReason"><xsl:value-of select="businessSemantic"/></xsl:attribute>
		<xsl:attribute name="ValidityDate">
			<xsl:value-of select="dateTime/year"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="dateTime/month"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="dateTime/day"/>		
		</xsl:attribute>
	</FareValidity>
</xsl:template>

<xsl:template match="refDetails">
	<TravelerReference>
		<UniqueID>
			<xsl:attribute name="Qualifier"><xsl:value-of select="refQualifier"/></xsl:attribute>
			<xsl:attribute name="ID"><xsl:value-of select="refNumber"/></xsl:attribute>
		</UniqueID>
	</TravelerReference>
</xsl:template>

<xsl:template match="fareDataMainInformation">
	<Information>
		<xsl:attribute name="Qualifier"><xsl:value-of select="fareDataQualifier"/></xsl:attribute>
		<xsl:attribute name="Location"><xsl:value-of select="fareLocation"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="fareCurrency"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="Amount"><xsl:value-of select="fareAmount"/></xsl:attribute>
		</Price>
	</Information>
</xsl:template>

<xsl:template match="fareDataSupInformation">
	<Information>
		<xsl:attribute name="Qualifier"><xsl:value-of select="fareDataQualifier"/></xsl:attribute>
		<xsl:attribute name="Location"><xsl:value-of select="fareLocation"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="fareCurrency"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="Amount"><xsl:value-of select="fareAmount"/></xsl:attribute>
		</Price>
	</Information>
</xsl:template>

<xsl:template match="taxInformation">
	<Taxes>
		<xsl:attribute name="Qualifier"><xsl:value-of select="taxDetails/taxQualifier"/></xsl:attribute>
		<xsl:attribute name="Identifier"><xsl:value-of select="taxDetails/taxIdentification/taxIdentifier"/></xsl:attribute>
		<xsl:attribute name="CountryCode"><xsl:value-of select="taxDetails/taxType/isoCountry"/></xsl:attribute>
		<xsl:attribute name="Nature"><xsl:value-of select="taxDetails/taxNature"/></xsl:attribute>
		<xsl:attribute name="ExemptIndicator"><xsl:value-of select="taxDetails/taxExempt"/></xsl:attribute>
		<xsl:apply-templates select="amountDetails/fareDataMainInformation"/>
		<xsl:apply-templates select="amountDetails/fareDataSupInformation"/>
	</Taxes>
</xsl:template>

<xsl:template match="bankerRates/firstRateDetail">
	<BankerRates>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstRateDetail/currencyCode"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="PriceTypeCode"><xsl:value-of select="('FirstRate')"/></xsl:attribute>
			<xsl:attribute name="Amount"><xsl:value-of select="firstRateDetail/amount"/></xsl:attribute>
		</Price>
	</BankerRates>
</xsl:template>

<xsl:template match="bankerRates/secondRateDetail">
	<BankerRates>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondRateDetail/currencyCode"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="PriceTypeCode"><xsl:value-of select="('SecondRate')"/></xsl:attribute>
			<xsl:attribute name="Amount"><xsl:value-of select="secondRateDetail/amount"/></xsl:attribute>
		</Price>
	</BankerRates>
</xsl:template>

<xsl:template match="passengerInformation[penDisInformation/infoQualifier='700']">
	<PenaltyInformation>
		<xsl:attribute name="Type"><xsl:value-of select="penDisInformation/penDisData/penaltyType"/></xsl:attribute>
		<xsl:attribute name="Qualifier"><xsl:value-of select="penDisInformation/penDisData/penaltyQualifier"/></xsl:attribute>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="penDisInformation/penDisData/penaltyCurrency"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="Amount"><xsl:value-of select="penDisInformation/penDisData/penaltyAmount"/></xsl:attribute>
		</Price>
		<xsl:apply-templates select="passengerReference"/>
	</PenaltyInformation>
</xsl:template>

<xsl:template match="passengerInformation[penDisInformation/infoQualifier='701']">
	<DiscountInformation>
		<xsl:attribute name="Type"><xsl:value-of select="penDisInformation/penDisData/penaltyType"/></xsl:attribute>
		<xsl:attribute name="Qualifier"><xsl:value-of select="penDisInformation/penDisData/penaltyQualifier"/></xsl:attribute>
		<xsl:attribute name="Code"><xsl:value-of select="penDisInformation/penDisData/discountCode"/></xsl:attribute>
		<Currency>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="penDisInformation/penDisData/penaltyCurrency"/></xsl:attribute>
		</Currency>
		<Price>
			<xsl:attribute name="Amount"><xsl:value-of select="penDisInformation/penDisData/penaltyAmount"/></xsl:attribute>
		</Price>
		<xsl:apply-templates select="passengerReference"/>
	</DiscountInformation>
</xsl:template>

<xsl:template match="passengerReference">
	<TravelerReference>
		<UniqueID>
			<xsl:attribute name="Qualifier"><xsl:value-of select="refDetails/refQualifier"/></xsl:attribute>
			<xsl:attribute name="ID"><xsl:value-of select="refDetails/refNumber"/></xsl:attribute>
		</UniqueID>
	</TravelerReference>
</xsl:template>

<xsl:template match="cityCode">
	<CityPair>
		<xsl:attribute name="Code"><xsl:value-of select="."/></xsl:attribute>
	</CityPair>
</xsl:template>

<xsl:template match="segmentInformation">
	<Segments>
		<xsl:attribute name="SequenceID"><xsl:value-of select="sequenceInformation/sequenceSection/sequenceNumber"/></xsl:attribute>
		<xsl:attribute name="CarrierCode"><xsl:value-of select="segDetails/airlineDetail/carrierCode"/></xsl:attribute>
		<xsl:attribute name="ClassOfService"><xsl:value-of select="segDetails/segmentDetail/classOfService"/></xsl:attribute>
		<xsl:attribute name="TicketingStatus"><xsl:value-of select="segDetails/ticketingStatus"/></xsl:attribute>
		<xsl:attribute name="SegmentIdentification"><xsl:value-of select="segDetails/segmentDetail/identification"/></xsl:attribute>
		<Connection>
			<xsl:attribute name="Type"><xsl:value-of select="connexInformation/connecDetails/connexType"/></xsl:attribute>
			<xsl:attribute name="Information"><xsl:value-of select="connexInformation/connecDetails/routingInformation"/></xsl:attribute>
		</Connection>
		<xsl:apply-templates select="fareQualifier"/>
		<xsl:apply-templates select="validityInformation"/>
		<xsl:apply-templates select="bagAllowanceInformation"/>
		<CityPair>
			<xsl:attribute name="Qualifier"><xsl:value-of select="('Origin')"/></xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="segDetails/departureCity/cityCode"/></xsl:attribute>
		</CityPair>
		<CityPair>
			<xsl:attribute name="Qualifier"><xsl:value-of select="('Destination')"/></xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="segDetails/arrivalCity/cityCode"/></xsl:attribute>
		</CityPair>
		<xsl:apply-templates select="segmentReference/refDetails"/>
	</Segments>
</xsl:template>

<xsl:template match="fareQualifier">
	<FareBasis>
		<xsl:attribute name="MovementType"><xsl:value-of select="movementType"/></xsl:attribute>
		<xsl:attribute name="PrimaryCode"><xsl:value-of select="fareBasisDetails/primaryCode"/></xsl:attribute>
		<xsl:attribute name="FareBasisCode"><xsl:value-of select="fareBasisDetails/fareBasisCode"/></xsl:attribute>
		<xsl:attribute name="TicketDesignator"><xsl:value-of select="fareBasisDetails/ticketDesignator"/></xsl:attribute>
		<xsl:attribute name="DiscountTicketDesignator"><xsl:value-of select="fareBasisDetails/discTktDesignator"/></xsl:attribute>
		<xsl:attribute name="DiscountType"><xsl:value-of select="zapOffDetails/zapOffType"/></xsl:attribute>
		<xsl:attribute name="DiscountAmount"><xsl:value-of select="zapOffDetails/zapOffAmount"/></xsl:attribute>
		<xsl:attribute name="DiscountPercentage"><xsl:value-of select="zapOffDetails/zapOffPercentage"/></xsl:attribute>
	</FareBasis>
</xsl:template>

<xsl:template match="validityInformation">
	<FareValidity>
		<xsl:attribute name="ValidityReason"><xsl:value-of select="businessSemantic"/></xsl:attribute>
		<xsl:attribute name="ValidityDate">
			<xsl:value-of select="dateTime/year"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="dateTime/month"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="dateTime/day"/>		
		</xsl:attribute>
	</FareValidity>
</xsl:template>

<xsl:template match="bagAllowanceInformation">
	<BagAllowance>
		<xsl:attribute name="Quantity"><xsl:value-of select="bagAllowanceDetails/baggageQuantity"/></xsl:attribute>
		<xsl:attribute name="Weight"><xsl:value-of select="bagAllowanceDetails/baggageWeight"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="bagAllowanceDetails/baggageType"/></xsl:attribute>
		<xsl:attribute name="Unit"><xsl:value-of select="bagAllowanceDetails/measureUnit"/></xsl:attribute>
	</BagAllowance>
</xsl:template>

<xsl:template match="otherPricingInfo">
	<OtherFareInformation>
		<xsl:attribute name="Type"><xsl:value-of select="attributeDetails/attributeType"/></xsl:attribute>
		<xsl:attribute name="Description"><xsl:value-of select="attributeDetails/attributeDescription"/></xsl:attribute>
	</OtherFareInformation>
</xsl:template>

<xsl:template match="statusInformation">
	<Status>
		<xsl:attribute name="Type"><xsl:value-of select="('FirstStatus')"/></xsl:attribute>
		<xsl:attribute name="StatusCode"><xsl:value-of select="firstStatusDetails/tstFlag"/></xsl:attribute>
	</Status>
	<Status>
		<xsl:attribute name="Type"><xsl:value-of select="('OtherStatus')"/></xsl:attribute>
		<xsl:attribute name="StatusCode"><xsl:value-of select="otherStatusDetails/tstFlag"/></xsl:attribute>
	</Status>
</xsl:template>

<xsl:template match="carrierFeesGroup">
	<AirlineFees>
		<xsl:attribute name="FeeType"><xsl:value-of select="carrierFeeType"/></xsl:attribute>
		<xsl:apply-templates select="carrierFeeInfo"/>
	</AirlineFees>
</xsl:template>

<xsl:template match="carrierFeeInfo">
	<FeeInformation>
		<FeeProperties>
			<xsl:attribute name="Type"><xsl:value-of select="carrierFeeSubcode/dataTypeInformation/type"/></xsl:attribute>
			<xsl:apply-templates select="carrierFeeSubcode/dataInformation"/>
		</FeeProperties>
		<FeeName>
			<xsl:attribute name="Qualifier"><xsl:value-of select="commercialName/freeTextQualification/textSubjectQualifier"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="commercialName/freeText"/></xsl:attribute>
		</FeeName>
		<Amount>
			<xsl:attribute name="Qualifier"><xsl:value-of select="feeAmount/monetaryDetails/typeQualifier"/></xsl:attribute>
			<Currency>
				<xsl:attribute name="CurrencyCode"><xsl:value-of select="feeAmount/monetaryDetails/currency"/></xsl:attribute>
			</Currency>
			<Price>
				<xsl:attribute name="Amount"><xsl:value-of select="feeAmount/monetaryDetails/amount"/></xsl:attribute>
			</Price>
		</Amount>
		<xsl:apply-templates select="feeTax"/>		
	</FeeInformation>
</xsl:template>

<xsl:template match="dataInformation">
	<FeeApplication>
		<xsl:attribute name="Code"><xsl:value-of select="indicator"/></xsl:attribute>
	</FeeApplication>
</xsl:template>

<xsl:template match="feeTax">
	<Taxes>
		<xsl:attribute name="TaxCategory"><xsl:value-of select="taxCategory"/></xsl:attribute>
		<xsl:apply-templates select="feeTaxDetails"/>
	</Taxes>
</xsl:template>

<xsl:template match="feeTaxDetails">
	<Details>
		<xsl:attribute name="Rate"><xsl:value-of select="rate"/></xsl:attribute>
		<xsl:attribute name="CurrencyCode"><xsl:value-of select="currencyCode"/></xsl:attribute>
		<xsl:attribute name="Type"><xsl:value-of select="type"/></xsl:attribute>
	</Details>
</xsl:template>

<xsl:template match="formOfPayment">
	<FormOfPayment>
		<xsl:attribute name="Type"><xsl:value-of select="type"/></xsl:attribute>
		<xsl:attribute name="CreditCardNumber"><xsl:value-of select="creditCardNumber"/></xsl:attribute>
		<xsl:attribute name="ChargedAmount"><xsl:value-of select="chargedAmount"/></xsl:attribute>
	</FormOfPayment>
</xsl:template>

<xsl:template match="automaticReissueInfo">
	<TicketingInformation>
		<TicketInfo>
			<xsl:attribute name="Type"><xsl:value-of select="ticketInfo/documentDetails/type"/></xsl:attribute>
			<xsl:attribute name="Number"><xsl:value-of select="ticketInfo/documentDetails/number"/></xsl:attribute>
		</TicketInfo>
		<CouponInfo>
			<xsl:attribute name="Qualifier"><xsl:value-of select="('MAIN')"/></xsl:attribute>
			<xsl:attribute name="Number"><xsl:value-of select="couponInfo/couponDetails/cpnNumber"/></xsl:attribute>
		</CouponInfo>
		<xsl:if test="couponInfo/otherCouponDetails">
			<CouponInfo>
				<xsl:attribute name="Qualifier"><xsl:value-of select="('OTHER')"/></xsl:attribute>
				<xsl:attribute name="Number"><xsl:value-of select="couponInfo/otherCouponDetails/cpnNumber"/></xsl:attribute>
			</CouponInfo>
		</xsl:if>
		<xsl:apply-templates select="paperCouponRange"/>
		<BaseFareInfo>
			<Information>
				<xsl:attribute name="Qualifier"><xsl:value-of select="baseFareInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
				<xsl:attribute name="Location"><xsl:value-of select="baseFareInfo/monetaryDetails/location"/></xsl:attribute>
				<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
				<Currency>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="baseFareInfo/monetaryDetails/currency"/></xsl:attribute>
				</Currency>
				<Price>
					<xsl:attribute name="Amount"><xsl:value-of select="baseFareInfo/monetaryDetails/amount"/></xsl:attribute>
				</Price>
			</Information>
			<xsl:if test="baseFareInfo/otherMonetaryDetails">
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="baseFareInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="baseFareInfo/otherMonetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="baseFareInfo/otherMonetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="baseFareInfo/otherMonetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
			</xsl:if>
		</BaseFareInfo>
		<FirstDPIGroup>
			<PenaltyInformation>
				<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/reIssuePenalty/penDisData/penaltyQualifier"/></xsl:attribute>
				<Currency>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/reIssuePenalty/penDisData/penaltyCurrency"/></xsl:attribute>
				</Currency>
				<Price>
					<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/reIssuePenalty/penDisData/penaltyAmount"/></xsl:attribute>
				</Price>
			</PenaltyInformation>
			<ReissueInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/reissueInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/reissueInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/reissueInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/reissueInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="firstDpiGroup/reissueInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/reissueInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/reissueInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/reissueInfo/otherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/reissueInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</ReissueInfo>
			<OldTaxInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/oldTaxInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/oldTaxInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/oldTaxInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/oldTaxInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="firstDpiGroup/oldTaxInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/oldTaxInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/oldTaxInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/oldTaxInfo/otherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/oldTaxInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</OldTaxInfo>
			<ReissueBalanceInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="firstDpiGroup/reissueBalanceInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/otherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="firstDpiGroup/reissueBalanceInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</ReissueBalanceInfo>
		</FirstDPIGroup>
		<SecondDPIGroup>
			<PenaltyInformation>
				<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/reIssuePenalty/penDisData/penaltyQualifier"/></xsl:attribute>
				<Currency>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/reIssuePenalty/penDisData/penaltyCurrency"/></xsl:attribute>
				</Currency>
				<Price>
					<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/reIssuePenalty/penDisData/penaltyAmount"/></xsl:attribute>
				</Price>
			</PenaltyInformation>
			<ResidualValueInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/residualValueInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/residualValueInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/residualValueInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/residualValueInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="secondDpiGroup/residualValueInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/residualValueInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/residualValueInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/residualValueInfo/otherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/residualValueInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</ResidualValueInfo>
			<OldTaxInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/oldTaxInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/oldTaxInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/oldTaxInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/oldTaxInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="secondDpiGroup/oldTaxInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/oldTaxInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/oldTaxInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/oldTaxInfo/otherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/oldTaxInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</OldTaxInfo>
			<IssueBalanceInfo>
				<Information>
					<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/monetaryDetails/typeQualifier"/></xsl:attribute>
					<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/monetaryDetails/location"/></xsl:attribute>
					<xsl:attribute name="Type"><xsl:value-of select="('Main')"/></xsl:attribute>
					<Currency>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/monetaryDetails/currency"/></xsl:attribute>
					</Currency>
					<Price>
						<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/monetaryDetails/amount"/></xsl:attribute>
					</Price>
				</Information>
				<xsl:if test="secondDpiGroup/issueBalanceInfo/otherMonetaryDetails">
					<Information>
						<xsl:attribute name="Qualifier"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/otherMonetaryDetails/typeQualifier"/></xsl:attribute>
						<xsl:attribute name="Location"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/otherMonetaryDetails/location"/></xsl:attribute>
						<xsl:attribute name="Type"><xsl:value-of select="('Supplementary')"/></xsl:attribute>
						<Currency>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="secondDpiGroup/issueBalanceInfootherMonetaryDetails/currency"/></xsl:attribute>
						</Currency>
						<Price>
							<xsl:attribute name="Amount"><xsl:value-of select="secondDpiGroup/issueBalanceInfo/otherMonetaryDetails/amount"/></xsl:attribute>
						</Price>
					</Information>
				</xsl:if>
			</IssueBalanceInfo>
		</SecondDPIGroup>		
	</TicketingInformation>
</xsl:template>

<xsl:template match="paperCouponRange">
	<PaperCouponRange>
		<TicketInfo>
			<xsl:attribute name="Type"><xsl:value-of select="ticketInfo/documentDetails/type"/></xsl:attribute>
			<xsl:attribute name="Number"><xsl:value-of select="ticketInfo/documentDetails/number"/></xsl:attribute>
		</TicketInfo>
		<CouponInfo>
			<xsl:attribute name="Qualifier"><xsl:value-of select="('MAIN')"/></xsl:attribute>
			<xsl:attribute name="Number"><xsl:value-of select="couponInfo/couponDetails/cpnNumber"/></xsl:attribute>
		</CouponInfo>
		<xsl:if test="couponInfo/otherCouponDetails">
			<CouponInfo>
				<xsl:attribute name="Qualifier"><xsl:value-of select="('OTHER')"/></xsl:attribute>
				<xsl:attribute name="Number"><xsl:value-of select="couponInfo/otherCouponDetails/cpnNumber"/></xsl:attribute>
			</CouponInfo>
		</xsl:if>
	</PaperCouponRange>	
</xsl:template>

<xsl:template match="mileage">
	<Mileage>
		<xsl:value-of select="mileageTimeDetails/totalMileage"/>
	</Mileage>	
</xsl:template>


</xsl:stylesheet>

