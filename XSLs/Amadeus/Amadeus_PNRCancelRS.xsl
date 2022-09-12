<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:if test="boolean(PoweredPNR_PNRReply)">
			<xsl:apply-templates select="PoweredPNR_PNRReply" />
		</xsl:if>
		<xsl:if test="boolean(MessagesOnly_Reply)">
			<xsl:apply-templates select="MessagesOnly_Reply" />
		</xsl:if>
	</xsl:template>
	<xsl:template match="PoweredPNR_PNRReply">
		<OTA_CancelRS>
			<xsl:attribute name="Version">3.14</xsl:attribute>
			<xsl:attribute name="Status">Cancelled</xsl:attribute>
			<Success />
			<UniqueID>
				<xsl:attribute name="ID">
					<xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber" />
				</xsl:attribute>
			</UniqueID>
		</OTA_CancelRS>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply">
		<OTA_CancelRS>
			<xsl:attribute name="Version">3.14</xsl:attribute>
			<xsl:attribute name="Status">Cancelled</xsl:attribute>
			<Errors>
				<xsl:apply-templates select="CAPI_Messages" />
			</Errors>
		</OTA_CancelRS>
	</xsl:template>
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Type">
				<xsl:value-of select="LineType" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
</xsl:stylesheet>
