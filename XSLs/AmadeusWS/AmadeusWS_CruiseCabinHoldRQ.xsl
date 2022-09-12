<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinHoldRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruiseCabinHoldRQ">
	<Cruise_HoldCabin>
		<agentEnvironment>
			<agentTerminalId>
				<xsl:value-of select="('12345678')"/>
			</agentTerminalId>
		</agentEnvironment>
		<numberOfUnitsDescription>
			<nbrOfUnitsDetails>
				<unitValue>
					<xsl:value-of select="sum(GuestCounts/GuestCount/@Quantity)"/>
				</unitValue>
				<unitQualifier>
					<xsl:value-of select="('NI')"/>
				</unitQualifier>
			</nbrOfUnitsDetails>
		</numberOfUnitsDescription>
		<xsl:if test="Guest/@Age">
			<xsl:apply-templates select="Guest/@Age"/>
			<unitQualifier>
				<xsl:value-of select="('AG')"/>
			</unitQualifier>
		</xsl:if>
		<sailingGroup>
			<sailingDescription>
				<providerDetails>
					<shipCode>
						<xsl:value-of select="SelectedSailing/@ShipCode"/>
					</shipCode>
					<cruiselineCode>
						<xsl:value-of select="SelectedSailing/@VendorCode"/>
					</cruiselineCode>
				</providerDetails>
				<sailingDateTime>
					<sailingDepartureDate>
						<xsl:value-of select="substring(SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SelectedSailing/@Start,1,4)"/>
					</sailingDepartureDate>
					<sailingDuration>
						<xsl:value-of select="SelectedSailing/@Duration"/>
					</sailingDuration>
				</sailingDateTime>
				<xsl:apply-templates select="SelectedSailing/@VoyageID"/>
			</sailingDescription>
			<xsl:apply-templates select="SelectedSailing/InclusivePackageOption"/>
			<xsl:if test="SelectedSailing/@VendorCode != 'RCC' and SelectedSailing/@VendorCode != 'CEL'">
				<xsl:choose>
					<xsl:when test="not(Guest[1]/GuestTransportation/GuestCity/@LocationCode) or (Guest	[1]/GuestTransportation/GuestCity/@LocationCode='')">
						<xsl:element name="addressInfo">
							<xsl:element name="cityName">
								<xsl:value-of select="Guest/GuestTransportation/GatewayCity/@LocationCode"/>
							</xsl:element>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="Guest[1]/GuestTransportation/GuestCity"/>	
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<modeOfTransportation>
				<transportationInfo>
					<modeOfTransport>
						<xsl:choose>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '29'">C</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '32'">A</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '33'">O</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '30'">F</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '31'">T</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '3'">B</xsl:when>
							<xsl:when test="Guest/GuestTransportation/@TransportationMode = '21'">R</xsl:when>
							<xsl:otherwise><xsl:value-of select="Guest/GuestTransportation/@TransportationMode"/></xsl:otherwise>
						</xsl:choose>
					</modeOfTransport>
					<motCity>
						<xsl:value-of select="Guest/GuestTransportation/GatewayCity/@LocationCode"/>
					</motCity>
				</transportationInfo>
			</modeOfTransportation>
			<currencyInfo>
				<currencyList>
					<currencyQualifier>
						<xsl:value-of select="('5')"/>
					</currencyQualifier>
					<currencyIsoCode>
						<xsl:choose>
							<xsl:when test="Currency/@CurrencyCode != ''">
								<xsl:value-of select="Currency/@CurrencyCode"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</currencyIsoCode>
				</currencyList>
			</currencyInfo>			
			<fareGroup>
				<fareCode>
					<fareCodeId>
						<cruiseFareCode>
							<xsl:value-of select="SelectedSailing/SelectedFare/@FareCode"/>
						</cruiseFareCode>
					</fareCodeId>
				</fareCode>
				<xsl:apply-templates select="SelectedSailing/SelectedFare/@GroupCode"/>
				<categoryGroup>
					<categoryInfo>
						<categoryId>
							<pricedCategory>
								<xsl:value-of select="SelectedSailing/SelectedCategory/@PricedCategoryCode"/>
							</pricedCategory>
							<berthedCategory>
								<xsl:value-of select="SelectedSailing/SelectedCategory/@BerthedCategoryCode"/>
							</berthedCategory>
						</categoryId>
					</categoryInfo>
					<xsl:apply-templates select="SelectedSailing/SelectedCategory/SelectedCabin"/>
				</categoryGroup>
			</fareGroup>
		</sailingGroup>		
	</Cruise_HoldCabin>
</xsl:template>

<xsl:template match="@Age">
	<nbrOfUnitsDetails>
		<unitValue>
			<xsl:value-of select="."/>
		</unitValue>
	</nbrOfUnitsDetails>
</xsl:template>

<xsl:template match="@VoyageID">
	<sailingId>
		<cruiseVoyageNbr>
			<xsl:value-of select="."/>
		</cruiseVoyageNbr>
	</sailingId>
</xsl:template>

<xsl:template match="GuestCity">
	<addressInfo>
		<cityName>
			<xsl:value-of select="@LocationCode"/>
		</cityName>
	</addressInfo>
</xsl:template>

<xsl:template match="InclusivePackageOption">
	<packageDescription>
		<packageType>
			<xsl:value-of select="('I')"/>
		</packageType>
		<packageDetails>
			<packageCode>
				<xsl:value-of select="@CruisePackageCode"/>
			</packageCode>
		</packageDetails>
		<packageDateTime>
			<packageStartDate>
				<xsl:value-of select="@StartDate"/>
			</packageStartDate>
		</packageDateTime>
	</packageDescription>
</xsl:template>

<xsl:template match="@GroupCode">
	<passengerGroupId>
		<passengerGroupInfoId>
			<groupCode>
				<xsl:value-of select="."/>
			</groupCode>
		</passengerGroupInfoId>
	</passengerGroupId>
</xsl:template>

<xsl:template match="SelectedCabin">
	<cabinInfo>
		<cabinDetails>
			<cabinNbr>
				<xsl:value-of select="@CabinNumber"/>
			</cabinNbr>						
		</cabinDetails>
		<!--xsl:apply-templates select="../@CabinFilters"/-->
		<xsl:apply-templates select="../@DeckName"/>
	</cabinInfo>
</xsl:template>

<!--xsl:template match="CabinFilters">
	<xsl:element name="cabinFilters">
		<xsl:value-of select="."/>
	</xsl:element>
</xsl:template-->

<xsl:template match="DeckName">
	<xsl:element name="deckPlanName">
		<xsl:element name="deckId">
			<xsl:value-of select="."/>
			</xsl:element>
	</xsl:element>
</xsl:template>


</xsl:stylesheet>
