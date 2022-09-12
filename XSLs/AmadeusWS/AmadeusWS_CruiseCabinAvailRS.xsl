<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinAvailRS.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_RequestCabinAvailabilityReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCabinAvailRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCabinAvailRS>
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

<xsl:template match="Cruise_RequestCabinAvailabilityReply">
	<xsl:element name="OTA_CruiseCabinAvailRS">
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('4')">
				<xsl:element name="Success"/>
				<xsl:element name="Warnings">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="not(advisoryMessage)">
				<xsl:element name="Success"/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryMessage)) or (advisoryMessage/errorQualifierDescription/messageType=('4'))">
			<SailingInfo>
				<SelectedSailing>
					<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNumber"/>
					<xsl:attribute name="Start">
						<xsl:if test="sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate != ''">
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
					<xsl:attribute name="VendorCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/>											</xsl:attribute>
					<xsl:attribute name="VendorName"></xsl:attribute>
					<xsl:attribute name="ShipCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
					</xsl:attribute>
					<xsl:attribute name="ShipName"/>
				</SelectedSailing>
				<xsl:apply-templates select="sailingGroup/packageDescription"/>
				<xsl:element name="SelectedCategory">
					<xsl:attribute name="BerthedCategoryCode">
						<xsl:value-of select="sailingGroup/fareGroup/categoryGroup/categoryInfo/categoryId/berthedCategory"/>	
					</xsl:attribute>
					<xsl:attribute name="PricedCategoryCode">
						<xsl:value-of select="sailingGroup/fareGroup/categoryGroup/categoryInfo/categoryId/pricedCategory"/>	
					</xsl:attribute>
				</xsl:element>
			</SailingInfo>
			<xsl:element name="SelectedFare">
				<xsl:attribute name="FareCode">
					<xsl:value-of select="sailingGroup/fareGroup/fareCode/fareCodeId/cruiseFareCode"/>	
				</xsl:attribute>
				<xsl:apply-templates select="sailingGroup/fareGroup/passengerGroupId/passengerGroupInfoId/groupCode"/>
			</xsl:element>
			<xsl:element name="CabinOptions">
				<xsl:apply-templates select="sailingGroup/fareGroup/categoryGroup/cabinGroup"/>
			</xsl:element>
			<xsl:apply-templates select="sailingGroup/sailingInformation"/>
			<xsl:apply-templates select="sailingGroup/marketingMessage/advisoryMessageDetails/messageDescription"/>
		</xsl:if>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('2')]">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:if test="messageQualifier=('1')">
			<xsl:attribute name="Code">
				<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('4')]">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:if test="messageQualifier=('1')">
			<xsl:attribute name="Code">
				<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="cruiseVoyageNumber">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="sailingInformation">
	<Remark>
		<xsl:value-of select="textDetails"/>	
	</Remark>
</xsl:template>

<xsl:template match="packageDescription">
	<InclusivePackageOption>
		<xsl:attribute name="CruisePackageCode">
			<xsl:value-of select="packageDetails/packageCode"/>	
		</xsl:attribute>
		<xsl:attribute name="StartDate">
			<xsl:value-of select="packageDateTime/packageStartDate"/>	
		</xsl:attribute>
	</InclusivePackageOption>
</xsl:template>

<xsl:template match="messageDescription">
	<Information>
		<xsl:attribute name="Name">
			<xsl:value-of select="('MarketingMessage')"/>	
		</xsl:attribute>
		<Text>
			<xsl:value-of select="."/>	
		</Text>
	</Information>
</xsl:template>

<xsl:template match="groupCode">
	<xsl:attribute name="GroupCode">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="cabinGroup">
	<xsl:element name="CabinOption">
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="cabinInfo/cabinStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="cabinInfo/cabinStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="cabinInfo/cabinStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:otherwise>Available</xsl:otherwise>
			</xsl:choose>	
		</xsl:attribute>
		<xsl:apply-templates select="cabinInfo/cabinDescription/categoryLocation"/>
		<xsl:apply-templates select="cabinInfo/cabinDescription/cabinSide"/>
		<xsl:apply-templates select="cabinInfo/cabinDescription/positionInShip"/>
		<xsl:apply-templates select="cabinInfo/cabinDescription/maxOccupancy"/>
		<xsl:apply-templates select="cabinInfo/deckPlanName/deckId"/>
		<xsl:attribute name="CabinNumber">
			<xsl:value-of select="cabinInfo/cabinDetails/cabinNbr"/>
		</xsl:attribute>
		<xsl:if test="cabinInfo/bedDetails">
			<xsl:attribute name="BedType">
				<xsl:value-of select="cabinInfo/bedDetails/bedType"/>	
			</xsl:attribute>
			<xsl:apply-templates select="cabinInfo/bedDetails/bedConfiguration"/>
		</xsl:if>
		<xsl:apply-templates select="cabinInfo/cabinDetails/remarkCode"/>
		<xsl:apply-templates select="cabinMeasurement"/>
		<xsl:apply-templates select="cabinInfo/cabinDetails/remarkText"/>
	</xsl:element>
</xsl:template>

<xsl:template match="categoryLocation">
	<xsl:attribute name="CategoryLocation">
		<xsl:choose>
			<xsl:when test=". = 'O'">Outside</xsl:when>
			<xsl:when test=". = 'I'">Inside</xsl:when>
			<xsl:when test=". = 'B'">Both</xsl:when>
			<xsl:when test=". = 'S'">Suite</xsl:when>
			<xsl:when test=". = 'V'">Balcony</xsl:when>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="cabinSide">
	<xsl:attribute name="ShipSide">
		<xsl:choose>
			<xsl:when test=". = 'S'">Starboard</xsl:when>
			<xsl:when test=". = 'P'">Port</xsl:when>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="positionInShip">
	<xsl:attribute name="PositionInShip">
		<xsl:choose>
			<xsl:when test=". = 'A'">After</xsl:when>
			<xsl:when test=". = 'F'">Forward</xsl:when>
			<xsl:when test=". = 'M'">Middle</xsl:when>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="maxOccupancy">
	<xsl:attribute name="MaxOccupancy">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="remarkText">
	<xsl:element name="Information">
		<xsl:element name="Text">
			<xsl:value-of select="."/>	
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="deckId">
	<xsl:attribute name="DeckName">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="bedConfiguration">
	<CabinConfiguraton>
		<xsl:attribute name="BedConfigurationCode">
			<xsl:value-of select="."/>	
		</xsl:attribute>
	</CabinConfiguraton>
</xsl:template>

<xsl:template match="cabinMeasurement">
	<xsl:element name="MeasurementInfo">
		<xsl:attribute name="UnitOfMeasure">
			<xsl:value-of select="measurementDescription/unit"/>
		</xsl:attribute>
		<xsl:attribute name="CabinArea">
			<xsl:value-of select="measurementDescription/cabinSize"/>
		</xsl:attribute>
	</xsl:element>
</xsl:template>


</xsl:stylesheet>