<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruisePriceBookingRS.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 14 Dec 2007 - Rastko								        		       			       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="CruiseByPass_PriceBookingReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruisePriceBookingRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruisePriceBookingRS>
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

<xsl:template match="CruiseByPass_PriceBookingReply">
	<OTA_CruisePriceBookingRS Version="1.000">
		<xsl:choose>
			<xsl:when test="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription"/>
				</Errors>
			</xsl:when>
			<!--xsl:when test="not(advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType)">
				<Errors>
					<xsl:apply-templates select="advisoryGroup/advisoryDetailsPerPax/advisoryMessageDetails"/>
				</Errors>
			</xsl:when-->
			<xsl:when test="generalErrors/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="generalErrors/advisoryMessageDetails"/>
				</Errors>
			</xsl:when>
			<xsl:when test="generalErrors/errorQualifierDescription/messageType=('4')">
				<Errors>
					<xsl:apply-templates select="generalErrors/advisoryMessageDetails"/>
				</Errors>
			</xsl:when>
			<xsl:when test="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('4')">
				<Success/>
				<Warnings>
					<xsl:apply-templates select="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription"/>
				</Warnings>
			</xsl:when>
			<xsl:when test="not(advisoryGroup)">
				<Success/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryGroup) and not(generalErrors)) or (advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('4'))">
			<SailingInfo>
				<SelectedSailing>
					<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNumber"/>
					<xsl:attribute name="Start">
						<xsl:if test="sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate != ''">
							<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>	
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring (sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:attribute name="Duration">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingDateTime/sailingDuration"/>	
					</xsl:attribute>
					<xsl:attribute name="VendorCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/>											</xsl:attribute>
					<xsl:attribute name="VendorName"></xsl:attribute>
					<xsl:attribute name="ShipCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
					</xsl:attribute>
					<xsl:attribute name="ShipName"/>
				</SelectedSailing>
				<xsl:apply-templates select="sailingGroup/currencyInfo/currencyList/currencyIsoCode"/>
				<xsl:choose>
					<xsl:when test="sailingGroup/categoryInfo/categoryId/berthedCategory != ''">
						<SelectedCategory>
							<xsl:attribute name="BerthedCategoryCode">
								<xsl:value-of select="sailingGroup/categoryInfo/categoryId/berthedCategory"/>	
							</xsl:attribute>
							<xsl:attribute name="PricedCategoryCode">
								<xsl:value-of select="sailingGroup/categoryInfo/categoryId/pricedCategory"/>	
							</xsl:attribute>
						</SelectedCategory>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="SelectedCategory"/>
					</xsl:otherwise>
				</xsl:choose>
			</SailingInfo>
			<BookingPayment>
				<BookingPrices>
					<xsl:apply-templates select="sailingGroup/generalPricingGroup"/>
				</BookingPrices>
				<PaymentSchedule>
					<xsl:apply-templates select="sailingGroup/dateTime/dateTimeDescription"/>
				</PaymentSchedule>
				<xsl:if test="sailingGroup/travellerGroup">
					<GuestPrices>
						<xsl:variable name="tgroup" select="sailingGroup/travellerGroup[1]"/>
						<xsl:apply-templates select="sailingGroup/travellerGroup">
							<xsl:with-param name="tgroup" select="$tgroup"/>
						</xsl:apply-templates>
					</GuestPrices>
				</xsl:if>
			</BookingPayment>
		</xsl:if>
	</OTA_CruisePriceBookingRS >
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('2')]">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="advisoryMessageDetails">
	<Error>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="messageDescription"/>
	</Error>
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('4')]">
	<Warning>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:if test="messageQualifier=('1')">
			<xsl:attribute name="Code">
				<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</Warning>
</xsl:template>

<xsl:template match="cruiseVoyageNumber">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="currencyIsoCode">
	<Currency>
		<xsl:attribute name="CurrencyCode">
			<xsl:value-of select="."/>	
		</xsl:attribute>
		<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
	</Currency>
</xsl:template>

<xsl:template match="generalPricingGroup">
	<xsl:variable name="pricetype"><xsl:value-of select="generalPriceInfo/amountDetail/amountQualifierCode"/></xsl:variable>
	<xsl:if test="not(preceding-sibling::generalPricingGroup/generalPriceInfo/amountDetail[amountQualifierCode = $pricetype])">
		<BookingPrice>
			<xsl:attribute name="PriceTypeCode">
				<xsl:choose>
					<xsl:when test="generalPriceInfo/amountDetail/amountQualifierCode = '000' and contains(generalPriceInfo/amountDetail/priceDescription,'TRANSFERS')">PKG</xsl:when>
					<xsl:when test="generalPriceInfo/amountDetail/amountQualifierCode = '000' and contains(generalPriceInfo/amountDetail/priceDescription,'PACKAGES')">PKG</xsl:when>
					<xsl:when test="generalPriceInfo/amountDetail/amountQualifierCode = '000' and generalPriceInfo/amountDetail/priceDescription = 'ONBOARD CREDIT'">POB</xsl:when>
					<xsl:when test="generalPriceInfo/amountDetail/amountQualifierCode = '000' and generalPriceInfo/amountDetail/priceDescription = 'COMMISSION AMOUNT'">CCC</xsl:when>
					<xsl:otherwise><xsl:value-of select="generalPriceInfo/amountDetail/amountQualifierCode"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="sum(../generalPricingGroup/generalPriceInfo/amountDetail[amountQualifierCode = $pricetype]/amount)"/>
			</xsl:attribute>
			<xsl:apply-templates select="generalPriceInfo/priceStatus"/>
			<xsl:value-of select="generalPriceInfo/amountDetail/priceDescription"/>
		</BookingPrice>
	</xsl:if>
</xsl:template>

<xsl:template match="priceStatus">
	<xsl:attribute name="RestrictedIndicator">
		<xsl:choose>
			<xsl:when test=". = 'Y'">true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="dateTimeDescription">
	<Payment>
		<xsl:attribute name="PaymentNumber">
			<xsl:choose>
				<xsl:when test="dateTimeQualifier=('107')">1</xsl:when>
				<xsl:when test="dateTimeQualifier=('170')">2</xsl:when>
				<xsl:otherwise><xsl:value-of select="dateTimeQualifier"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="DueDate">
			<xsl:value-of select="substring(dateTimeDetails,5,4)"/>	
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(dateTimeDetails,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(dateTimeDetails,1,2)"/>
			<xsl:if test="substring(dateTimeDetails,9,2) != ''">
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring(dateTimeDetails,9,2)"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring(dateTimeDetails,11,2)"/>
				<xsl:text>:00</xsl:text>
			</xsl:if>
		</xsl:attribute>
	</Payment>
</xsl:template>

<xsl:template match="travellerGroup">
	<xsl:param name="tgroup"/>
	<GuestPrice>
		<xsl:attribute name="GuestRPH">
			<xsl:value-of select="passengerDetails/travellerId/lastName"/>
		</xsl:attribute>
		<xsl:apply-templates select="modeOfTransportation"/>
		<xsl:if test="pricingGroup">
			<PriceInfos>
				<xsl:variable name="tgroupcur" select="."/>
				<xsl:apply-templates select="$tgroup/pricingGroup">
					<xsl:with-param name="tgroupcur" select="$tgroupcur"/>
				</xsl:apply-templates>
			</PriceInfos>
		</xsl:if>
	</GuestPrice>
</xsl:template>

<xsl:template match="modeOfTransportation">
	<GuestTransportation>
		<xsl:attribute name="TransportationMode">
			<xsl:value-of select="transportationInfo/modeOfTransport"/>
		</xsl:attribute>
		<xsl:attribute name="TransportationStatus">
			<xsl:choose>
				<xsl:when test="motStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="motStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="motStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="not(motStatus)">Available</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="motStatus"/>	
				</xsl:otherwise>
			</xsl:choose>	
		</xsl:attribute>
		<GatewayCity>
			<xsl:value-of select="transportationInfo/motCity"/>
		</GatewayCity>
	</GuestTransportation>
</xsl:template>

<xsl:template match="pricingGroup">
	<xsl:param name="tgroupcur"/>
	<xsl:variable name="aqc"><xsl:value-of select="passengerPriceDetails/amountDetail/amountQualifierCode"/></xsl:variable>
	<xsl:choose>
		<xsl:when test="$tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]">
			<PriceInfo>
				<xsl:attribute name="PriceTypeCode">
					<xsl:choose>
						<xsl:when test="$aqc = '000' and contains($tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/priceDescription,'TRANSFERS')">PKG</xsl:when>
						<xsl:when test="$aqc = '000' and contains($tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/priceDescription,'PACKAGES')">PKG</xsl:when>
						<xsl:when test="$aqc = '000' and $tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/priceDescription = 'ONBOARD CREDIT'">POB</xsl:when>
						<xsl:when test="$aqc = '000' and $tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/priceDescription = 'COMMISSION AMOUNT'">CCC	</xsl:when>
						<xsl:otherwise><xsl:value-of select="$aqc"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Amount">
					<xsl:value-of select="$tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/amount"/>
				</xsl:attribute>
				<xsl:apply-templates select="$tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/priceStatus"/>
				<xsl:value-of select="$tgroupcur/pricingGroup[passengerPriceDetails/amountDetail/amountQualifierCode = $aqc]/passengerPriceDetails/amountDetail/priceDescription"/>
			</PriceInfo>
		</xsl:when>
		<xsl:otherwise>
			<PriceInfo>
				<xsl:attribute name="PriceTypeCode">
					<xsl:choose>
						<xsl:when test="passengerPriceDetails/amountDetail/amountQualifierCode = '000' and contains(passengerPriceDetails/amountDetail/priceDescription,'TRANSFERS')">PKG</xsl:when>
						<xsl:when test="passengerPriceDetails/amountDetail/amountQualifierCode = '000' and contains(passengerPriceDetails/amountDetail/priceDescription,'PACKAGES')">PKG</xsl:when>
						<xsl:when test="passengerPriceDetails/amountDetail/amountQualifierCode = '000' and passengerPriceDetails/amountDetail/priceDescription = 'ONBOARD CREDIT'">POB</xsl:when>
						<xsl:when test="passengerPriceDetails/amountDetail/amountQualifierCode = '000' and passengerPriceDetails/amountDetail/priceDescription = 'COMMISSION AMOUNT'">CCC	</xsl:when>
						<xsl:otherwise><xsl:value-of select="passengerPriceDetails/amountDetail/amountQualifierCode"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Amount">0</xsl:attribute>
			</PriceInfo>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>


</xsl:stylesheet>