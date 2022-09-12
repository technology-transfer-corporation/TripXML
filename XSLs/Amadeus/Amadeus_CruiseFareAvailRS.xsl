<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruiseFareAvailRS.xsl 	     											       -->
<!-- ================================================================== -->
<!-- Date: 05 Apr 2007 - Rastko								        					       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="CruiseByPass_FareAvailabilityReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseFareAvailRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseFareAvailRS>
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
	
<xsl:template match="CruiseByPass_FareAvailabilityReply">
	<xsl:element name="OTA_CruiseFareAvailRS">
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="error"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('4')">
				<xsl:element name="Success"/>
				<xsl:element name="Warnings">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="warning"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageQualifier=('1')">
				<xsl:element name="Success"/>
				<xsl:element name="Warnings">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription/messageQualifier" mode="warning"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="not(advisoryMessage)">
				<xsl:element name="Success"/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="not(advisoryMessage) or (advisoryMessage/errorQualifierDescription/messageType!=('2')) or sailingGroup/fareGroup">
			<xsl:element name="SailingInfo">
				<xsl:apply-templates select="sailingGroup/sailingDescription/sailingIndicators"/>
				<xsl:attribute name="DepartureDate">
					<xsl:if test="sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate !=''">
						<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>
						<xsl:text>-</xsl:text>	
						<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
					</xsl:if>
				</xsl:attribute>
				<xsl:attribute name="Duration">
					<xsl:value-of select="sailingGroup/sailingDescription/sailingDateTime/sailingDuration"/>	
				</xsl:attribute>
				<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNbr"/>
				<xsl:if test="sailingGroup/sailingDescription/sailingDetails/numberOfPorts != ''">
					<xsl:attribute name="PortsOfCallQuantity">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingDetails/numberOfPorts"/>	
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode != ''">
					<xsl:attribute name="Status">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode"/>	
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="sailingGroup/sailingDescription/sailingDetails/maxNumberOfGuest != ''">
					<xsl:attribute name="MaxCabinOccupancy">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingDetails/maxNumberOfGuest"/>	
					</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="sailingGroup/sailingDescription/sailingDetails/categoryLocation"/>
				<xsl:element name="CruiseLine">
					<xsl:attribute name="VendorCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/></xsl:attribute>
					<xsl:attribute name="VendorName"></xsl:attribute>
					<xsl:attribute name="ShipCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
					</xsl:attribute>
					<xsl:attribute name="ShipName"></xsl:attribute>
				</xsl:element>
				<xsl:apply-templates select="sailingGroup/sailingDescription/regionDetails/cruiseRegionCode"/>
				<xsl:if test="sailingGroup/sailingDescription/arrivalAndDeparturePort[1]/portCode != ''">
					<xsl:element name="DeparturePort">
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="sailingGroup/sailingDescription/arrivalAndDeparturePort[1]/portCode"/>	
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
				<xsl:if test="sailingGroup/sailingDescription/arrivalAndDeparturePort[2]/portCode">
					<xsl:element name="ArrivalPort">
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="sailingGroup/sailingDescription/arrivalAndDeparturePort[2]/portCode"/>	
						</xsl:attribute>
					</xsl:element>		
				</xsl:if>
				<xsl:apply-templates select="sailingGroup/diningDetails/diningIdentification"/>
				<xsl:apply-templates select="sailingGroup/modeOfTransportation"/>
				<xsl:apply-templates select="sailingGroup/packageDescription"/>
				<xsl:apply-templates select="sailingGroup/sailingInformation"/>
				<xsl:apply-templates select="sailingGroup/marketingMessage/advisoryMessageDetails/messageDescription"/>
			</xsl:element>
			<xsl:apply-templates select="sailingGroup/currencyInfo"/>
			<xsl:apply-templates select="currencyAlternateList"/>
			<xsl:element name="FareCodeOptions">
				<xsl:apply-templates select="sailingGroup/fareGroup"/>
			</xsl:element>
		</xsl:if>
	</xsl:element>
</xsl:template>


<xsl:template match="errorQualifierDescription[messageType=('2')]" mode="error">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>


<xsl:template match="errorQualifierDescription[messageType=('4')]" mode="warning">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:choose>
			<xsl:when test="messageQualifier=('2')">
				<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
			</xsl:when>
			<xsl:when test="messageQualifier=('1')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>	
	</xsl:element>
</xsl:template>

<xsl:template match="messageQualifier" mode="warning">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="sailingIndicators">
	<xsl:attribute name="ListOfSailingDescriptionCode">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="cruiseVoyageNbr">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="categoryLocation">
	<xsl:attribute name="CategoryLocation">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="currencyInfo">
	<xsl:element name="Currency">
		<xsl:attribute name="CurrencyCode">
			<xsl:value-of select="currencyList/currencyIsoCode"/>	
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="cruiseRegionCode">
	<xsl:element name="Region">
		<xsl:attribute name="Code">
			<xsl:value-of select="."/>	
		</xsl:attribute>
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
				<xsl:when test="diningStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="diningStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="diningStatus = ''">Unavailable</xsl:when>
				<xsl:when test="not(diningStatus)">Unavailable</xsl:when>
				<xsl:otherwise><xsl:value-of select="diningStatus"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="modeOfTransportation">
	<xsl:element name="Transportation">
		<xsl:attribute name="Mode">
			<xsl:choose>
				<xsl:when test="transportationInfo/modeOfTransport ='C'">29</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='A'">32</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='O'">33</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='F'">30</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='T'">31</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='B'">3</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='R'">21</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="motStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="motStatus = 'CNF'">Available</xsl:when>
				<xsl:when test="motStatus = 'WTL'">Waitlist</xsl:when>
				<xsl:when test="motStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="not(motStatus)">Available</xsl:when>
				<xsl:otherwise><xsl:value-of select="motStatus"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:element>
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

<xsl:template match="currencyAlternateList">
	<xsl:element name="AlternateCurrencyList">
		<xsl:attribute name="CurrencyCode">
			<xsl:value-of select="currencyList/currencyIsoCode"/>
		</xsl:attribute>
	</xsl:element>
</xsl:template>

<xsl:template match="messageDescription">
	<xsl:element name="Information">
		<xsl:attribute name="Name">
			<xsl:value-of select="('MarketingInfo')"/>	
		</xsl:attribute>
		<xsl:element name="Text">
			<xsl:value-of select="."/>	
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="fareGroup">
	<xsl:element name="FareCodeOption">
		<xsl:apply-templates select="fareCode/fareIndicator"/>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="fareCode/statusCode = 'AVL'">Available</xsl:when>
				<xsl:when test="fareCode/statusCode = 'WTL'">Waitlist</xsl:when>
				<xsl:when test="fareCode/statusCode = 'CLO'">Unavailable</xsl:when>
				<xsl:otherwise><xsl:value-of select="fareCode/statusCode"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:if test="passengerGroupId/groupCode != ''">
			<xsl:apply-templates select="passengerGroupId"/>	
		</xsl:if>	
		<xsl:attribute name="FareCode">
			<xsl:value-of select="fareCode/fareCodeId/cruiseFareCode"/>	
		</xsl:attribute>
		<xsl:if test="fareCode/fareCodeId/fareDescription != ''">
			<xsl:attribute name="FareDescription">
				<xsl:value-of select="fareCode/fareCodeId/fareDescription"/>	
			</xsl:attribute>
		</xsl:if>
		<!--xsl:apply-templates select="fareCode/fareCodeId/remarkCode"/-->
		<xsl:apply-templates select="fareCode/fareCodeId/remarkText"/>
	</xsl:element>
</xsl:template>

<!--xsl:template match="remarkCode">
	<xsl:element name="FareRemark">
		<xsl:value-of select="."/>	
	</xsl:element>
</xsl:template-->

<xsl:template match="remarkText">
	<xsl:element name="FareRemark">
		<xsl:value-of select="."/>	
	</xsl:element>
</xsl:template>


<xsl:template match="fareIndicator">
	<xsl:attribute name="ListOfFareQualifierCode">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>


<xsl:template match="passengerGroupId">
	<xsl:attribute name="GroupCode">
		<xsl:value-of select="passengerGroupInfoId/groupCode"/>	
	</xsl:attribute>
</xsl:template>

</xsl:stylesheet>