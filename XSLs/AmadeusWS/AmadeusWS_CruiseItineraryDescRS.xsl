<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://www.w3.org/2004/07/xpath-functions" xmlns:xdt="http://www.w3.org/2004/07/xpath-datatypes">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseItineraryDescRS.xsl 	   									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="Cruise_DisplayItineraryDescriptionReply">
	<OTA_CruiseItineraryDescRS>
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="error"/>
				</Errors>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('4')">
				<Success/>
				<Warnings>
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="warning"/>
				</Warnings>
			</xsl:when>
			<xsl:when test="not(advisoryMessage)">
				<Success/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryMessage)) or (advisoryMessage/errorQualifierDescription/messageType=('4'))">
			<SelectedSailing>
				<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNbr"/>
				<xsl:attribute name="Start">
					<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>
					<xsl:text>-</xsl:text>	
					<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
					<xsl:text>T00:00:00</xsl:text>
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
				<xsl:attribute name="ShipName"></xsl:attribute>
			</SelectedSailing>
			<CruiseItinInfos>
				<xsl:apply-templates select="sailingGroup/marketingMessage/advisoryMessageDetails/messageDescription"/>
				<xsl:apply-templates select="sailingGroup/locationGroup"/>
			</CruiseItinInfos>
		</xsl:if>
	</OTA_CruiseItineraryDescRS>
</xsl:template>


<xsl:template match="errorQualifierDescription[messageType=('2')]" mode="error">
	<Error>
		<xsl:attribute name="Type">
			<xsl:value-of select="messageQualifier"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="messageQualifier=('2')">
				<xsl:attribute name="ShortText">
					<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="messageQualifier=('1')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>	
	</Error>
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('4')]" mode="warning">
	<Warning>
		<xsl:attribute name="Type">
			<xsl:value-of select="messageQualifier"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="messageQualifier=('2')">
				<xsl:attribute name="ShortText">
					<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="messageQualifier=('1')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>	
	</Warning>
</xsl:template>

<xsl:template match="cruiseVoyageNbr">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="messageDescription">
	<CruiseItinInfo>
		<xsl:attribute name="DockIndicator">false</xsl:attribute>
		<xsl:attribute name="ShorexIndicator">false</xsl:attribute>
		<Information>
			<xsl:attribute name="Name">
				<xsl:value-of select="('MarketingInfo')"/>	
			</xsl:attribute>
			<Text>
				<xsl:value-of select="."/>	
			</Text>
		</Information>
	</CruiseItinInfo>
</xsl:template>

<xsl:template match="locationGroup">
	<CruiseItinInfo>
		<xsl:attribute name="PortCode">
			<xsl:value-of select="locationInformation/locationDescription/portCode"/>
		</xsl:attribute>
		<xsl:attribute name="PortName">
			<xsl:value-of select="locationInformation/locationDescription/portName"/>
		</xsl:attribute>
		<xsl:if test="locationInformation/relatedLocationDetails/relatedLocationId != ''">
			<xsl:attribute name="PortCountryCode">
				<xsl:value-of select="locationInformation/relatedLocationDetails/relatedLocationId"/>	
			</xsl:attribute>
		</xsl:if>
		<xsl:attribute name="DockIndicator">
			<xsl:choose>
				<xsl:when test="locationInformation/relationType=('207')">true</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>			
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="ShorexIndicator">
			<xsl:choose>
				<xsl:when test="locationInformation/locationDescription/featureIndicator=('S')">true</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>			
			</xsl:choose>
		</xsl:attribute>
		<xsl:apply-templates select="depositDateTime"/>
		<xsl:apply-templates select="remark"/>
	</CruiseItinInfo>
</xsl:template>

<xsl:template match="depositDateTime">
	<DateTimeDescription>
		<xsl:attribute name="DateTimeQualifier">
			<xsl:choose>
				<xsl:when test="dateTimeDescription/dateTimeQualifier = 'A'">arrival</xsl:when>
				<xsl:when test="dateTimeDescription/dateTimeQualifier = 'B'">boarding</xsl:when>
				<xsl:when test="dateTimeDescription/dateTimeQualifier = 'D'">departure</xsl:when>
				<xsl:when test="dateTimeDescription/dateTimeQualifier = 'S'">stay</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="DateTimeDetails">
			<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,5,4)"/>
			<xsl:text>-</xsl:text>	
			<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,1,2)"/>
		</xsl:attribute>
	</DateTimeDescription>
</xsl:template>

<xsl:template match="remark">
	<Information>
		<xsl:attribute name="Name">
			<xsl:value-of select="('Itinerary remark')"/>	
		</xsl:attribute>
		<Text>
			<xsl:value-of select="advisoryMessageDetails/messageDescription"/>	
		</Text>
	</Information>
</xsl:template>


</xsl:stylesheet>