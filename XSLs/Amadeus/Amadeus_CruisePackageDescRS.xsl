<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruisePackageDescRS.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 15 Mar 2006 - Rastko											        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="CruiseByPass_PrePostPackageDescriptionReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruisePackageDescRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruisePackageDescRS>
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

<xsl:template match="CruiseByPass_PrePostPackageDescriptionReply">
	<xsl:element name="OTA_CruisePackageDescRS">
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
					<xsl:attribute name="VoyageID">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNumber"/>	
					</xsl:attribute>
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
			</SailingInfo>
			<xsl:if test="sailingGroup/packageGroup">
				<xsl:apply-templates select="sailingGroup/packageGroup"/>
			</xsl:if>
		</xsl:if>
	</xsl:element>
</xsl:template>

<xsl:template match="packageGroup">
	<PackageDescription>
		<xsl:attribute name="PackageType">
			<xsl:choose>
				<xsl:when test="packageDescription/packageType = 'A'">Post</xsl:when>
				<xsl:when test="packageDescription/packageType = 'Z'">Bus</xsl:when>
				<xsl:when test="packageDescription/packageType = 'S'">Shore</xsl:when>
				<xsl:otherwise>Pre</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="PackageCode">
			<xsl:value-of select="packageDescription/packageDetails/packageCode"/>
		</xsl:attribute>
		<xsl:apply-templates select="freeDescription/description"/>
	</PackageDescription>
</xsl:template>

<xsl:template match="description">
	<Text><xsl:value-of select="."/></Text>
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

</xsl:stylesheet>