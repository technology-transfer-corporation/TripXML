<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinUnHoldRS.xsl 	    									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>
<!--only one occurrence of Fax?-->

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_UnholdCabinReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCabinUnholdRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCabinUnholdRS>
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

<xsl:template match="Cruise_UnholdCabinReply">
	<OTA_CruiseCabinUnholdRS>
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
					<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/>										</xsl:attribute>
				<xsl:attribute name="VendorName"></xsl:attribute>
				<xsl:attribute name="ShipCode">
					<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
				</xsl:attribute>
				<xsl:attribute name="ShipName"/>
				<xsl:choose>
					<xsl:when test="sailingGroup/cabinGroup"><xsl:apply-templates select="sailingGroup/cabinGroup"/></xsl:when>
					<xsl:otherwise>
						<SelectedCabin>
							<xsl:attribute name="CabinNumber"></xsl:attribute>
							<xsl:attribute name="HeldIndicator">false</xsl:attribute>
						</SelectedCabin>
					</xsl:otherwise>
				</xsl:choose>
			</SelectedSailing>
		</xsl:if>
	</OTA_CruiseCabinUnholdRS>
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
	</xsl:element>
</xsl:template>

<xsl:template match="cabinGroup">
	<SelectedCabin>
		<xsl:attribute name="CabinNumber">
			<xsl:value-of select="cabinInfo/cabinDetails/cabinNbr"/>	
		</xsl:attribute>
		<xsl:attribute name="HeldIndicator">
			<xsl:choose>
				<xsl:when test="pricedDataAdvisory/advisoryMessageDetails/advisoryMessageNbr ='9821'">false</xsl:when>
				<xsl:otherwise>true</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</SelectedCabin>
</xsl:template>


</xsl:stylesheet>