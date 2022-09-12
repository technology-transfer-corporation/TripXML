<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinHoldRS.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_HoldCabinReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCabinHoldRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCabinHoldRS>
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

<xsl:template match="Cruise_HoldCabinReply">
	<OTA_CruiseCabinHoldRS>
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
			<xsl:when test="not(advisoryMessage)">
				<xsl:element name="Success"/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryMessage)) or (advisoryMessage/errorQualifierDescription/messageType=('4'))">
			<SelectedSailing Start="" Duration="" VendorCode="" VendorName="" ShipCode="" ShipName="">
				<SelectedCabin>
					<xsl:attribute name="CabinNumber">
						<xsl:if test="cabinGroup/cabinInfo/cabinDetails/cabinNbr != ''">
							<xsl:value-of select="cabinGroup/cabinInfo/cabinDetails/cabinNbr"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:apply-templates select="cabinGroup/cabinProfileInfo/attributeInfo"/>
				</SelectedCabin>
				<xsl:apply-templates select="diningDetails/diningIdentification"/>
				<xsl:if test="diningDetails[not(diningIdentification)]/guestTableSize">
					<xsl:for-each select="diningDetails/guestTableSize">
						<Dining>
							<xsl:attribute name="TableSize">
								<xsl:value-of select="."/>
							</xsl:attribute>
						</Dining>
					</xsl:for-each>
				</xsl:if>
				<xsl:apply-templates select="insuranceInfo/insuranceList[insuranceCode != 'NON']"/>
				<xsl:apply-templates select="profileInfo"/>
				<xsl:apply-templates select="paymentInfo"/>
				<xsl:apply-templates select="faxInfo"/>
			</SelectedSailing>
		</xsl:if>
	</OTA_CruiseCabinHoldRS>
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

<xsl:template match="cabinProfileInfo/attributeInfo">
	<xsl:attribute name="MaxOccupancy">
			<xsl:value-of select="attribute"/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningIdentification">
	<Dining>
		<xsl:apply-templates select="../roomDetails/smokingIndicator"/>
		<xsl:apply-templates select="../roomDetails/diningRoomId"/>
		<xsl:apply-templates select="../guestTableSize"/>
		<xsl:apply-templates select="../diningGuestAge"/>
		<xsl:apply-templates select="../roomDetails/preferedIsoLanguage"/>
		<xsl:attribute name="Sitting">
			<xsl:value-of select="diningLabel"/>
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="diningStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="diningStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="diningStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="diningStatus"/>	
				</xsl:otherwise>
			</xsl:choose>	
		</xsl:attribute>
	</Dining>
</xsl:template>

<xsl:template match="smokingIndicator">
	<xsl:attribute name="SmokingCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningRoomId">
	<xsl:attribute name="DiningRoom">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="guestTableSize">
	<xsl:attribute name="TableSize">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningGuestAge">
	<xsl:attribute name="AgeCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="preferedIsoLanguage">
	<xsl:attribute name="Language">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="insuranceList">
	<Insurance>
		<xsl:attribute name="InsuranceCode">
			<xsl:value-of select="insuranceCode"/>
		</xsl:attribute>
		<xsl:if test="defaultIndicator != ''">
			<xsl:attribute name="DefaultIndicator">
				<xsl:choose>
					<xsl:when test="defaultIndicator = 'Y'">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:if>
	</Insurance>
</xsl:template>

<xsl:template match="profileInfo">
	<CruiseProfiles>
		<xsl:attribute name="ProfileTypeIdentifier">
			<xsl:choose>
				<xsl:when test="profileType = 'MAN'">Mandatory</xsl:when>
				<xsl:when test="profileType = 'MAX'">Maximums</xsl:when>
				<xsl:when test="profileType = 'MOD'">Modifiable</xsl:when>
				<xsl:otherwise><xsl:value-of select="profileType"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:apply-templates select="attributeInfo"/>
	</CruiseProfiles>
</xsl:template>

<xsl:template match="attributeInfo">
	<CruiseProfile>
		<xsl:attribute name="Code">
			<xsl:value-of select="attributeType"/>
		</xsl:attribute>
		<xsl:apply-templates select="attributeDescription"/>
	</CruiseProfile>
</xsl:template>

<xsl:template match="attributeDescription">
	<xsl:attribute name="MaxQuantity">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="paymentInfo">
	<AcceptedPayments>
		<xsl:apply-templates select="paymentDetails"/>
		<xsl:apply-templates select="creditCardInfo"/>
	</AcceptedPayments>
</xsl:template>

<xsl:template match="paymentDetails">
	<AcceptedPayment>
		<xsl:choose>
			<xsl:when test="formOfPaymentCode = 'CCD'">
				<PaymentCard CardType="1"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'AGC'">
				<DirectBill DirectBill_ID="Agency Check"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'CAG'">
				<DirectBill DirectBill_ID="Credited Agency"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'WTR'">
				<DirectBill DirectBill_ID="Wire Transfer"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'BAG'">
				<DirectBill DirectBill_ID="Billing Agency"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'CHK'">
				<DirectBill DirectBill_ID="Check"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'GRO'">
				<DirectBill DirectBill_ID="Giro"/>
			</xsl:when>
		</xsl:choose>
	</AcceptedPayment>
</xsl:template>

<xsl:template match="creditCardInfo">
	<AcceptedPayment>
		<PaymentCard>
			<!--xsl:attribute name="FormOfPayment">
				<xsl:value-of select="('CCD')"/>
			</xsl:attribute-->
			<xsl:attribute name="CardCode">
					<xsl:value-of select="creditCardCode"/>
			</xsl:attribute>
		</PaymentCard>
	</AcceptedPayment>
</xsl:template>

<xsl:template match="faxInfo">
	<FaxNotification>
		<xsl:attribute name="PhoneNumber">
			<xsl:choose>
				<xsl:when test="not(faxNbr)">TBA</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="faxNbr"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:apply-templates select="faxDetails"/>
	</FaxNotification>
</xsl:template>

<xsl:template match="faxDetails">
	<xsl:element name="NotificationType">
		<xsl:value-of select="faxType"/>
	</xsl:element>
</xsl:template>


</xsl:stylesheet>