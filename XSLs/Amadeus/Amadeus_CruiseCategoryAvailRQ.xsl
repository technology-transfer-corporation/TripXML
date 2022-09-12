<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruiseCategoryAvailRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 3 Jan 2006 - Rastko										        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruiseCategoryAvailRQ">
	<CruiseByPass_CategoryAvailability>
		<processingInfo>
			<processingDetails>
				<businessType>
					<xsl:value-of select="('5')"/>
				</businessType>
				<function>
					<xsl:value-of select="('56')"/>
				</function>
			</processingDetails>
		</processingInfo>
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
		<xsl:choose>
			<xsl:when test="GuestCounts/GuestCount/@Code != ''">
				<numberOfUnitsDescription>
					<xsl:apply-templates select="GuestCounts/GuestCount/@Code"/>
				</numberOfUnitsDescription>
			</xsl:when>
			<xsl:when test="Guest/@Age != ''">
				<numberOfUnitsDescription>
					<xsl:apply-templates select="Guest/@Age"/>
				</numberOfUnitsDescription>
			</xsl:when>
		</xsl:choose>
		<sailingGroup>
			<sailingDescription>
				<providerDetails>
					<shipCode>
						<xsl:value-of select="SailingInfo/SelectedSailing/@ShipCode"/>
					</shipCode>
					<cruiselineCode>
						<xsl:value-of select="SailingInfo/SelectedSailing/@VendorCode"/>
					</cruiselineCode>
				</providerDetails>
				<sailingDateTime>
					<sailingDepartureDate>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,1,4)"/>
					</sailingDepartureDate>
					<sailingDuration>
						<xsl:value-of select="SailingInfo/SelectedSailing/@Duration"/>
					</sailingDuration>
				</sailingDateTime>
				<xsl:apply-templates select="SailingInfo/SelectedSailing/@VoyageID"/>
			</sailingDescription>
			<currencyInfo>
				<currencyList>
					<currencyQualifier>
						<xsl:value-of select="('5')"/>
					</currencyQualifier>
					<currencyIsoCode>
						<xsl:choose>
							<xsl:when test="SailingInfo/Currency/@CurrencyCode != ''">
								<xsl:value-of select="SailingInfo/Currency/@CurrencyCode"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</currencyIsoCode>
				</currencyList>
			</currencyInfo>
			<xsl:choose>
				<xsl:when test="not(Guest[1]/GuestTransportation/GuestCity/@LocationCode) or (Guest[1]/GuestTransportation/GuestCity/@LocationCode='')">
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
			<xsl:apply-templates select="SailingInfo/InclusivePackageOption"/>
			<xsl:apply-templates select="SelectedFare"/>
		</sailingGroup>		
	</CruiseByPass_CategoryAvailability>
</xsl:template>

<xsl:template match="@Age">
	<nbrOfUnitsDetails>
		<unitValue>
			<xsl:value-of select="."/>
		</unitValue>
			<unitQualifier>
				<xsl:value-of select="('AG')"/>
			</unitQualifier>
	</nbrOfUnitsDetails>
</xsl:template>

<xsl:template match="@Code">
	<xsl:variable name="nip"><xsl:value-of select="../@Quantity"/></xsl:variable>
	<xsl:call-template name="nipcode">
		<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
	</xsl:call-template>
</xsl:template>
	
<xsl:template name="nipcode">
	<xsl:param name="nip"/>
	<xsl:if test="$nip > 0 ">
		<nbrOfUnitsDetails>
			<unitValue>
				<xsl:choose>
					<xsl:when test=". = '10'">30</xsl:when>
					<xsl:when test=". = '8'">10</xsl:when>
					<xsl:when test=". = '7'">01</xsl:when>
					<xsl:when test=". = '11'">65</xsl:when>
					<xsl:otherwise><xsl:text>30</xsl:text></xsl:otherwise>
				</xsl:choose>
			</unitValue>
				<unitQualifier>
					<xsl:value-of select="('AG')"/>
				</unitQualifier>
		</nbrOfUnitsDetails>
		<xsl:call-template name="nipcode">
			<xsl:with-param name="nip"><xsl:value-of select="$nip - 1"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>
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

<xsl:template match="SelectedFare">
	<fareGroup>
		<fareCode>
			<fareCodeId>
				<cruiseFareCode>
					<xsl:value-of select="@FareCode"/>
				</cruiseFareCode>
			</fareCodeId>
		</fareCode>
		<xsl:apply-templates select="@GroupCode"/>
	</fareGroup>
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


</xsl:stylesheet>
