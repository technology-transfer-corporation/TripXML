<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseFareAvailRQ.xsl 	       									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruiseFareAvailRQ">
	<xsl:element name="Cruise_RequestFareAvailability">
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
		<xsl:if test="Guest/@Age">
			<xsl:apply-templates select="Guest/@Age"/>
			<xsl:element name="unitQualifier">
				<xsl:value-of select="('AG')"/>
			</xsl:element>
		</xsl:if>
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
			<xsl:apply-templates select="SailingInfo/InclusivePackageOption"/>
			<xsl:apply-templates select="Guest"/>
			<xsl:element name="currencyInfo">
				<xsl:element name="currencyList">
					<xsl:element name="currencyQualifier">5</xsl:element>
					<xsl:element name="currencyIsoCode">
						<xsl:value-of select="SailingInfo/Currency/@CurrencyCode"/>
					</xsl:element>
				</xsl:element>
			</xsl:element>
		</xsl:element>		
	</xsl:element>
</xsl:template>


<xsl:template match="@Age">
	<xsl:element name="nbrOfUnitsDetails">
		<xsl:element name="unitValue">
			<xsl:value-of select="."/>
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

<xsl:template match="InclusivePackageOption">
	<xsl:element name="packageDescription">
		<xsl:element name="packageType">
			<xsl:value-of select="('I')"/>
		</xsl:element>
		<xsl:element name="packageDetails">
			<xsl:element name="packageCode">
				<xsl:value-of select="@CruisePackageCode"/>
			</xsl:element>
		</xsl:element>
		<xsl:element name="packageDateTime">
			<xsl:element name="packageStartDate">
				<xsl:value-of select="@StartDate"/>
			</xsl:element>
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="Guest">
	<xsl:element name="travellerGroup">
		<xsl:element name="travellerInfo">
			<xsl:element name="travellerId">
				<xsl:element name="lastName">
					<xsl:value-of select="@GuestRPH"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
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
		<xsl:apply-templates select="GuestTransportation"/>
	</xsl:element>
</xsl:template>

<xsl:template match="GuestCity">
	<xsl:element name="addressInfo">
		<xsl:element name="cityName">
			<xsl:value-of select="@LocationCode"/>
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="GuestTransportation">
	<xsl:element name="modeOfTransportation">
		<xsl:element name="transportationInfo">
			<xsl:element name="modeOfTransport">
				<xsl:choose>
					<xsl:when test="@TransportationMode = '29'">C</xsl:when>
					<xsl:when test="@TransportationMode = '32'">A</xsl:when>
					<xsl:when test="@TransportationMode = '33'">O</xsl:when>
					<xsl:when test="@TransportationMode = '30'">F</xsl:when>
					<xsl:when test="@TransportationMode = '31'">T</xsl:when>
					<xsl:when test="@TransportationMode = '3'">B</xsl:when>
					<xsl:when test="@TransportationMode = '21'">R</xsl:when>
					<xsl:otherwise><xsl:value-of select="@TransportationMode"/></xsl:otherwise>
				</xsl:choose>
			</xsl:element>
			<xsl:element name="motCity">
				<xsl:value-of select="GatewayCity/@LocationCode"/>
			</xsl:element>
		</xsl:element>
	</xsl:element>		
</xsl:template>

</xsl:stylesheet>
