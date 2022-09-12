<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseTransferAvailRS.xsl 	     								       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_RequestTransferAvailabilityReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseTransferAvailRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseTransferAvailRS>
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

<xsl:template match="Cruise_RequestTransferAvailabilityReply">
	<xsl:element name="OTA_CruiseTransferAvailRS">
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('1')">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/advisoryMessageDetails/messageDescription"/>
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
			<xsl:if test="sailingGroup/transferGroup">
				<TransferOptions>
					<xsl:apply-templates select="sailingGroup/transferGroup"/>
				</TransferOptions>
			</xsl:if>
		</xsl:if>
	</xsl:element>
</xsl:template>

<xsl:template match="transferGroup">
	<TransferOption>
		<xsl:attribute name="TransferType">
			<xsl:choose>
				<xsl:when test="transferInfo/transferDetails/transferType = 'A'">Post</xsl:when>
				<xsl:when test="transferInfo/transferDetails/transferType = 'R'">Round</xsl:when>
				<xsl:otherwise>Pre</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="TransferCode">
			<xsl:value-of select="transferInfo/transferDetails/transferCode"/>
		</xsl:attribute>
		<xsl:attribute name="Description">
			<xsl:value-of select="transferInfo/transferDetails/transferDescription"/>
		</xsl:attribute>
		<xsl:if test="transferInfo/transferDetails/remark != ''">
			<xsl:attribute name="Remark">
				<xsl:value-of select="transferInfo/transferDetails/remark"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo/dateTime/transferDate != ''">
			<xsl:attribute name="StartDateTime">
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,5,4)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,3,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,1,2)"/>
				<xsl:text>T</xsl:text>
				<xsl:choose>
					<xsl:when test="transferInfo/dateTime/transferTime != ''">
						<xsl:value-of select="substring(transferInfo/dateTime/transferTime,1,2)"/>
						<xsl:text>:</xsl:text>
						<xsl:value-of select="substring(transferInfo/dateTime/transferTime,3,2)"/>
						<xsl:text>:00</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>00:00:00</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo/transferLocation/boardOffPoints[1] != ''">
			<xsl:attribute name="BoardPoint">
				<xsl:value-of select="transferInfo/transferLocation/boardOffPoints[1]"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo/transferLocation/boardOffPoints[position()=2] != ''">
			<xsl:attribute name="OffPoint">
				<xsl:value-of select="transferInfo/transferLocation/boardOffPoints[position()=2]"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="priceInfo">
			<TransferPrices>
				<xsl:apply-templates select="priceInfo"/>
			</TransferPrices>
		</xsl:if>
	</TransferOption>
</xsl:template>

<xsl:template match="priceInfo">
	<TransferPrice>
		<xsl:attribute name="PriceTypeCode">
			<xsl:value-of select="amountDetail/amountQualifierCode"/>
		</xsl:attribute>
		<xsl:attribute name="Amount">
			<xsl:value-of select="amountDetail/amount"/>
		</xsl:attribute>
		<xsl:attribute name="CodeDetail"></xsl:attribute>
		<xsl:attribute name="AgeQualifyingCode">
			<xsl:choose>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'ADT'">10</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'INF'">7</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'CHD'">8</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'SRC'">11</xsl:when>
				<xsl:when test="not(amountDetail/breakdownQualifierCode)">10</xsl:when>
				<xsl:otherwise><xsl:value-of select="amountDetail/breakdownQualifierCode"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="BreakdownType">
			<xsl:value-of select="amountDetail/breakdownCode"/>
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="priceStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="priceStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="priceStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="priceStatus = 'WTL'">Waitlist</xsl:when>
				<xsl:otherwise>Available</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</TransferPrice>
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

<xsl:template match="messageDescription">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="."/>
	</xsl:element>
</xsl:template>

<xsl:template match="sailingInformation">
	<Remark>
		<xsl:value-of select="textDetails"/>	
	</Remark>
</xsl:template>

</xsl:stylesheet>