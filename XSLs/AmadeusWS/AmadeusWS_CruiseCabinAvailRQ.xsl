<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinAvailRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruiseCabinAvailRQ">
	<xsl:element name="Cruise_RequestCabinAvailability">
		<xsl:element name="processingInfo">
			<xsl:element name="processingDetails">
				<xsl:element name="businessType">
					<xsl:value-of select="('5')"/>
				</xsl:element>
				<xsl:element name="function">
					<xsl:value-of select="('56')"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
		<xsl:element name="agentEnvironment">
			<xsl:element name="agentTerminalId">
				<xsl:value-of select="('12345678')"/>
			</xsl:element>
		</xsl:element>
		<xsl:element name="numberOfUnitsDescription">
			<xsl:element name="nbrOfUnitsDetails">
				<xsl:element name="unitValue">
					<xsl:value-of select="sum(GuestCounts/GuestCount/@Quantity)"/>
				</xsl:element>
				<xsl:element name="unitQualifier">
					<xsl:value-of select="('NI')"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
		<xsl:element name="sailingGroup">
			<xsl:element name="sailingDescription">
				<xsl:element name="providerDetails">
					<xsl:element name="shipCode">
					<xsl:value-of select="SailingInfo/SelectedSailing/@ShipCode"/>
					</xsl:element>
					<xsl:element name="cruiselineCode">
						<xsl:value-of select="SailingInfo/SelectedSailing/@VendorCode"/>
					</xsl:element>
				</xsl:element>
				<xsl:element name="sailingDateTime">
					<xsl:element name="sailingDepartureDate">
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,1,4)"/>
					</xsl:element>
					<xsl:element name="sailingDuration">
						<xsl:value-of select="SailingInfo/SelectedSailing/@Duration"/>
					</xsl:element>
				</xsl:element>
				<xsl:apply-templates select="SailingInfo/SelectedSailing/@VoyageID"/>
			</xsl:element>
			<xsl:element name="modeOfTransportation">
				<xsl:element name="transportationInfo">
					<xsl:element name="modeOfTransport">
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
					</xsl:element>
					<xsl:element name="motCity">
						<xsl:value-of select="Guest/GuestTransportation/GatewayCity/@LocationCode"/>
					</xsl:element>
				</xsl:element>
				<xsl:choose>
					<xsl:when test="Guest/GuestTransportation/@TransportationStatus != ''">
						<xsl:element name="motStatus">
							<xsl:choose>
								<xsl:when test="Guest/GuestTransportation/@TransportationStatus = 'Available'">AVL</xsl:when>
								<xsl:when test="Guest/GuestTransportation/@TransportationStatus = 'OnRequest'">ONR</xsl:when>
								<xsl:when test="Guest/GuestTransportation/@TransportationStatus = 'Waitlist'">WTL</xsl:when>
							</xsl:choose>
						</xsl:element>
					</xsl:when>
					<xsl:when test="SailingInfo/SelectedSailing/@VendorCode = 'RCC' or SailingInfo/SelectedSailing/@VendorCode = 'CEL'">
						<xsl:element name="motStatus">AVL</xsl:element>
					</xsl:when>
				</xsl:choose>
			</xsl:element>
			<xsl:if test="SailingInfo/SelectedSailing/@VendorCode != 'RCC' and SailingInfo/SelectedSailing/@VendorCode != 'CEL'">
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
			<xsl:element name="currencyInfo">
				<xsl:element name="currencyList">
					<xsl:element name="currencyQualifier">
						<xsl:value-of select="('5')"/>
					</xsl:element>
					<xsl:element name="currencyIsoCode">
						<xsl:choose>
							<xsl:when test="SailingInfo/Currency/@CurrencyCode != ''">
								<xsl:value-of select="SailingInfo/Currency/@CurrencyCode"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:element>
				</xsl:element>
			</xsl:element>
			<xsl:apply-templates select="SailingInfo/InclusivePackageOption"/>
			<xsl:element name="fareGroup">
				<xsl:element name="fareCode">
					<xsl:element name="fareCodeId">
						<xsl:element name="cruiseFareCode">
							<xsl:value-of select="SelectedFare/@FareCode"/>
						</xsl:element>
					</xsl:element>
				</xsl:element>
				<xsl:apply-templates select="SelectedFare/@GroupCode"/>
				<xsl:element name="categoryGroup">
					<xsl:element name="categoryInfo">
						<xsl:element name="categoryId">
							<xsl:element name="pricedCategory">
								<xsl:value-of select="SailingInfo/SelectedCategory/@PricedCategoryCode"/>
							</xsl:element>
							<xsl:element name="berthedCategory">
								<xsl:value-of select="SailingInfo/SelectedCategory/@BerthedCategoryCode"/>
							</xsl:element>
						</xsl:element>
					</xsl:element>
					<xsl:if test="SailingInfo/SelectedCategory/CabinFilters/CabinFilter/@CabinFilterCode or SailingInfo/SelectedCategory/@DeckName">
						<xsl:element name="cabinInfo">
							<xsl:apply-templates select="SailingInfo/SelectedCategory/CabinFilters/CabinFilter"/>
							<xsl:apply-templates select="SailingInfo/SelectedCategory/@DeckName"/>
						</xsl:element>
					</xsl:if>					
				</xsl:element>							
			</xsl:element>
		</xsl:element>		
	</xsl:element>
</xsl:template>

<xsl:template match="@VoyageID">
	<xsl:element name="sailingId">
		<xsl:element name="cruiseVoyageNbr">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:element>
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
	<xsl:element name="passengerGroupId">
		<xsl:element name="passengerGroupInfoId">
			<xsl:element name="groupCode">
				<xsl:value-of select="."/>
			</xsl:element>
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="CabinFilter">
	<xsl:element name="cabinFilters">
		<xsl:value-of select="@CabinFilterCode"/>
	</xsl:element>
</xsl:template>

<xsl:template match="@DeckName">
	<xsl:element name="deckPlanName">
		<xsl:element name="deckId">
			<xsl:value-of select="."/>
			</xsl:element>
	</xsl:element>
</xsl:template>


</xsl:stylesheet>
