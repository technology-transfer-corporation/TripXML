<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCancelRS.xsl 	     										       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_CancelBookingReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCancelRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCancelRS>
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

<xsl:template match="Cruise_CancelBookingReply">
	<OTA_CruiseCancelRS>
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="generalErrors/errorQualifierDescription[messageQualifier = '3']/messageType = '3'">
				<xsl:attribute name="Status">Cancelled</xsl:attribute>
				<Success />
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="reservationInfo/reservationDetails/pnrRecordLocator"/>
					</xsl:attribute>
				</UniqueID>
				<xsl:if test="freeDescription/description != ''">
					<CancelInfoRS>
						<CancelRules>
							<CancelRule><xsl:value-of select="freeDescription/description"/></CancelRule>
						</CancelRules>
					</CancelInfoRS>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="Status">Unsuccessful</xsl:attribute>
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="reservationInfo/reservationDetails/pnrRecordLocator"/>
					</xsl:attribute>
				</UniqueID>
				<xsl:if test="generalErrors/errorQualifierDescription[messageQualifier = '3']/messageType = '2'">
					<CancelInfoRS>
						<CancelRules>
							<CancelRule><xsl:value-of select="generalErrors/advisoryMessageDetails/messageDescription"/></CancelRule>
						</CancelRules>
					</CancelInfoRS>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</OTA_CruiseCancelRS>
</xsl:template>

</xsl:stylesheet>