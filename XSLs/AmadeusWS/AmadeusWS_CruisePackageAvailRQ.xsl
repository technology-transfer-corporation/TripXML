<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruisePackageAvailRQ.xsl 	     								       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruisePackageAvailRQ">
	<xsl:element name="Cruise_RequestPrePostPackageAvailability">
		<xsl:element name="agentEnvironment">
			<xsl:element name="agentTerminalId">
				<xsl:value-of select="('12345678')"/>
			</xsl:element>
		</xsl:element>
		<moreFeatures>
			<moreDescription>
				<maxUnitNbr>30</maxUnitNbr>
			</moreDescription>
		</moreFeatures>
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
			<packageDescription>
				<packageType>
					<xsl:choose>
						<xsl:when test="SailingInfo/PackageOption/@PackageType = 'Pre'">B</xsl:when>
						<xsl:otherwise>A</xsl:otherwise>
					</xsl:choose>
				</packageType>
				<xsl:if test="SailingInfo/PackageOption/@PackageDuration != ''">
					<packageDateTime>
						<packageDuration>
							<xsl:value-of select="SailingInfo/PackageOption/@PackageDuration"/>
						</packageDuration>
					</packageDateTime>
				</xsl:if>
			</packageDescription>
			<xsl:apply-templates select="SailingInfo/InclusivePackageOption"/>
			<xsl:apply-templates select="Guest"/>
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

<xsl:template match="Guest">
	<travellerGroup>
		<travellerInfo>
			<travellerId>
				<lastName>
					<xsl:value-of select="position()"/>
				</lastName>
			</travellerId>
			<travellerDetails>
				<age>
					<xsl:value-of select="@Age"/>
				</age>
			</travellerDetails>
		</travellerInfo>
		<modeOfTransportation>
			<transportationInfo>
				<modeOfTransport>
					<xsl:choose>
						<xsl:when test="GuestTransportation/@TransportationMode = '29'">C</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '32'">A</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '33'">O</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '30'">F</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '31'">T</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '3'">B</xsl:when>
						<xsl:when test="GuestTransportation/@TransportationMode = '21'">R</xsl:when>
						<xsl:otherwise><xsl:value-of select="GuestTransportation/@TransportationMode"/></xsl:otherwise>
					</xsl:choose>
				</modeOfTransport>
				<motCity>
					<xsl:value-of select="GuestTransportation/GatewayCity/@LocationCode"/>
				</motCity>
			</transportationInfo>
		</modeOfTransportation>	
		<xsl:choose>
			<xsl:when test="not(GuestTransportation/GuestCity/@LocationCode) or (GuestTransportation/GuestCity/@LocationCode='')">
				<xsl:element name="addressInfo">
					<xsl:element name="cityName">
						<xsl:value-of select="GuestTransportation/GatewayCity/@LocationCode"/>
					</xsl:element>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="GuestTransportation/GuestCity"/>	
			</xsl:otherwise>
		</xsl:choose>
		<fareCode>
			<fareCodeId>
				<cruiseFareCode>
					<xsl:value-of select="../SelectedFare/@FareCode"/>
				</cruiseFareCode>
			</fareCodeId>
		</fareCode>
	</travellerGroup>
</xsl:template>

</xsl:stylesheet>
