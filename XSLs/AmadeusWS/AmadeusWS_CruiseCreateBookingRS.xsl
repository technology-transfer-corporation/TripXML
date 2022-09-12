<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCreateBookingRS.xsl 	     								       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_CreateBookingReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCreateBookingRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCreateBookingRS>
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

<xsl:template match="Cruise_CreateBookingReply">
	<OTA_CruiseCreateBookingRS Version="1.000">
		<xsl:choose>
			<xsl:when test="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription" mode="error"/>
				</Errors>
			</xsl:when>
			<xsl:when test="generalErrors/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="generalErrors/errorQualifierDescription" mode="error"/>
				</Errors>
			</xsl:when>
			<xsl:when test="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('4') or generalErrors/errorQualifierDescription/messageType='4'">
				<Success/>
				<Warnings>
					<xsl:apply-templates select="advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription" mode="warning"/>
					<xsl:apply-templates select="generalErrors/advisoryMessageDetails" mode="warning"/>
				</Warnings>
			</xsl:when>
			<xsl:when test="not(advisoryGroup) and not(generalErrors)">
				<Success/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="((not(advisoryGroup)) and not(generalErrors)) or (advisoryGroup/advisoryDetailsPerPax/errorQualifierDescription/messageType=('4')) or (generalErrors/advisoryMessageDetails/advisoryMessageNbr  = '9821') or (reservationInfo/reservationDetails/pnrRecordLocator != '')">
			<ReservationID>
				<xsl:attribute name="Type">
					<xsl:value-of select="('CruiseLineConfirmationNumber')"/>	
				</xsl:attribute>
				<xsl:attribute name="ID">
					<xsl:value-of select="bookingQualifier/itemDescription/value"/>	
				</xsl:attribute>
			</ReservationID>
			<ReservationID>
				<xsl:attribute name="Type">
					<xsl:value-of select="('AmadeusRecordLocator')"/>	
				</xsl:attribute>
				<xsl:attribute name="ID">
					<xsl:value-of select="reservationInfo/reservationDetails/pnrRecordLocator"/>	
				</xsl:attribute>
			</ReservationID>
			<xsl:if test="freeDescription">
				<BookingInfo>
					<PolicyInfo>
						<xsl:apply-templates select="freeDescription/description"/>
					</PolicyInfo>
				</BookingInfo>
			</xsl:if>
		</xsl:if>
	</OTA_CruiseCreateBookingRS>
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
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="advisoryMessageDetails" mode="warning">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="description">
	<xsl:element name="Text">
		<xsl:value-of select="."/>	
	</xsl:element>
</xsl:template>

</xsl:stylesheet>