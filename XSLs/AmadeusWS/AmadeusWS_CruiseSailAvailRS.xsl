<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseSailAvailRS.xsl 	     										       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>
<xsl:variable name="mottable" select="document('..\..\tables\decoding\ttCruiseMot.xml')"/>
<xsl:template match="/">
	<xsl:apply-templates select="Cruise_RequestSailingAvailabilityReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseSailAvailRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseSailAvailRS>
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

<xsl:template match="Cruise_RequestSailingAvailabilityReply">
	<xsl:element name="OTA_CruiseSailAvailRS">
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="not(sailingGroup)">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="error"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="sailingGroup">
				<xsl:element name="Success"/>
				<xsl:if test="advisoryMessage">
					<xsl:element name="Warnings">
						<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="warning"/>
					</xsl:element>	
				</xsl:if>
				<xsl:element name="SailingOptions">
					<xsl:apply-templates select="sailingGroup"/>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription" mode="error">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription" mode="warning">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">
			<xsl:value-of select="messageQualifier"/>
		</xsl:attribute>
		<xsl:attribute name="ShortText">
			<xsl:value-of select="itemType"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="messageQualifier=('2')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="messageQualifier=('1')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>	
	</xsl:element>
</xsl:template>

<xsl:template match="sailingGroup">
	<xsl:element name="SailingOption">
		<xsl:if test="sailingDescription/sailingIndicators != ''">
			<xsl:attribute name="ListOfSailingDescriptionCode">
				<xsl:apply-templates select="sailingDescription/sailingIndicators"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:attribute name="DepartureDate">
			<xsl:value-of select="substring(sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>
			<xsl:text>-</xsl:text>	
			<xsl:value-of select="substring(sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
			<xsl:text>T00:00:00</xsl:text>
		</xsl:attribute>
		<xsl:attribute name="Duration">
			<xsl:value-of select="sailingDescription/sailingDateTime/sailingDuration"/>	
		</xsl:attribute>
		<xsl:apply-templates select="sailingDescription/sailingId/cruiseVoyageNbr"/>
		<xsl:attribute name="PortsOfCallQuantity">
			<xsl:value-of select="sailingDescription/sailingDetails/numberOfPorts"/>	
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="sailingDescription/sailingDetails/sailingStatusCode = 'AVL'">Available</xsl:when>
				<xsl:when test="sailingDescription/sailingDetails/sailingStatusCode = 'WTL'">OnRequest</xsl:when>
				<xsl:when test="sailingDescription/sailingDetails/sailingStatusCode = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="sailingDescription/sailingDetails/sailingStatusCode = 'CLO'">Unavailable</xsl:when>
				<xsl:otherwise><xsl:value-of select="sailingDescription/sailingDetails/sailingStatusCode"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="MaxCabinOccupancy">
			<xsl:value-of select="sailingDescription/sailingDetails/maxNumberOfGuest"/>	
		</xsl:attribute>
		<xsl:apply-templates select="sailingDescription/sailingDetails/categoryLocation"/>
		<xsl:apply-templates select="currencyInfo"/>
		<xsl:element name="CruiseLine">
			<xsl:attribute name="VendorCode">
				<xsl:value-of select="sailingDescription/providerDetails/cruiselineCode"/>	
			</xsl:attribute>
			<xsl:attribute name="VendorName"></xsl:attribute>
			<xsl:attribute name="ShipCode">
				<xsl:value-of select="sailingDescription/providerDetails/shipCode"/>	
			</xsl:attribute>
			<xsl:attribute name="ShipName"/>
		</xsl:element>
		<xsl:apply-templates select="sailingDescription/regionDetails/cruiseRegionCode"/>
		<xsl:element name="DeparturePort">
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="sailingDescription/arrivalAndDeparturePort[1]/portCode"/>	
			</xsl:attribute>
		</xsl:element>
		<xsl:element name="ArrivalPort">
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="sailingDescription/arrivalAndDeparturePort[2]/portCode"/>	
			</xsl:attribute>
		</xsl:element>		
		<xsl:apply-templates select="diningDetails/diningIdentification"/>
		<xsl:choose>
			<xsl:when test="modeOfTransportation">
				<xsl:apply-templates select="modeOfTransportation"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="mot">
					<xsl:with-param name="vendor"><xsl:value-of select="sailingDescription/providerDetails/cruiselineCode"/></xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:apply-templates select="packageDescription"/>
		<xsl:apply-templates select="sailingInformation"/>
		<xsl:apply-templates select="../registrationInformation"/>
	</xsl:element>
</xsl:template>

<xsl:template match="sailingIndicators">
	<xsl:if test="position() > 1"><xsl:text> </xsl:text></xsl:if>
	<xsl:choose>
		<xsl:when test=". = 'O'">5</xsl:when>
		<xsl:when test=". = 'P'">4</xsl:when>
		<xsl:when test=". = 'Z'">6</xsl:when>
		<xsl:when test=". = 'S'">7</xsl:when>
		<xsl:when test=". = 'K'">2</xsl:when>
		<xsl:when test=". = 'G'">1</xsl:when>
		<xsl:when test=". = 'I'">3</xsl:when>
		<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="cruiseVoyageNbr">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="categoryLocation">
	<xsl:attribute name="CategoryLocation">
		<xsl:choose>
			<xsl:when test=". = 'I'">Inside</xsl:when>
			<xsl:when test=". = 'O'">Outside</xsl:when>
			<xsl:when test=". = 'B'">Both</xsl:when>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="currencyInfo">
	<xsl:attribute name="CurrencyCode">
		<xsl:value-of select="currencyList[1]/currencyIsoCode"/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="cruiseRegionCode">
	<xsl:element name="Region">
		<xsl:attribute name="RegionCode">
			<xsl:choose>
				<xsl:when test="contains(../../../sailingInformation/textDetails,'BERMUDA')">BER</xsl:when>
				<xsl:when test="contains(../../../sailingInformation/textDetails,'BAHAMAS')">BAH</xsl:when>
				<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="RegionName"/>
	</xsl:element>
</xsl:template>

<xsl:template match="diningIdentification">
	<xsl:element name="Dining">
		<xsl:attribute name="Sitting">
			<xsl:value-of select="diningLabel"/>	
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="diningStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="diningStatus = 'WTL'">OnRequest</xsl:when>
				<xsl:when test="diningStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="diningStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:otherwise><xsl:value-of select="diningStatus"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="modeOfTransportation">
	<xsl:for-each select="transportationInfo">
		<xsl:element name="Transportation">
			<xsl:attribute name="Mode">
				<xsl:choose>
					<xsl:when test="modeOfTransport ='C'">29</xsl:when>
					<xsl:when test="modeOfTransport ='A'">32</xsl:when>
					<xsl:when test="modeOfTransport ='O'">33</xsl:when>
					<xsl:when test="modeOfTransport ='F'">30</xsl:when>
					<xsl:when test="modeOfTransport ='T'">31</xsl:when>
					<xsl:when test="modeOfTransport ='B'">3</xsl:when>
					<xsl:when test="modeOfTransport ='R'">21</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Status">
				<xsl:choose>
					<xsl:when test="../motStatus = 'AVL'">Available</xsl:when>
					<xsl:when test="../motStatus = 'ONR'">OnRequest</xsl:when>
					<xsl:when test="../motStatus = 'WTL'">OnRequest</xsl:when>
					<xsl:when test="../motStatus = 'CLO'">Unavailable</xsl:when>
					<xsl:otherwise><xsl:value-of select="../motStatus"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:element>
	</xsl:for-each>
</xsl:template>

<xsl:template name="mot">
	<xsl:param name="vendor"/>
	<xsl:for-each select="$mottable/ttCruiseMot/R[@cruise = $vendor]">
		<xsl:element name="Transportation">
			<xsl:attribute name="Mode">
				<xsl:value-of select="@code"/>
			</xsl:attribute>
			<xsl:attribute name="Status">Available</xsl:attribute>
		</xsl:element>
	</xsl:for-each>
</xsl:template>

<xsl:template match="packageDescription">
	<xsl:element name="InclusivePackageOption">
		<xsl:attribute name="CruisePackageCode">
			<xsl:value-of select="packageDetails/packageCode"/>	
		</xsl:attribute>
		<xsl:attribute name="StartDate">
			<xsl:value-of select="packageDateTime/packageStartDate"/>	
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="sailingInformation">
	<xsl:element name="Information">
		<xsl:attribute name="Name">
			<xsl:value-of select="('SailingInfo')"/>	
		</xsl:attribute>
		<xsl:element name="Text">
			<xsl:value-of select="textDetails"/>	
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="registrationInformation">
	<xsl:element name="Information">
		<xsl:attribute name="Name">
			<xsl:value-of select="('RegistrationInfo')"/>	
		</xsl:attribute>
		<xsl:apply-templates select="description"/>
	</xsl:element>
</xsl:template>

<xsl:template match="description">
	<xsl:element name="Text">
		<xsl:value-of select="."/>	
	</xsl:element>
</xsl:template>


</xsl:stylesheet>