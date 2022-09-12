<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<OTA_ShowMileageRS Version="1.0">
		<Success/>
		<FromCity><xsl:value-of select="MileageRS/OriginInfo/OriginLocation/@LocationCode"/></FromCity>
		<xsl:apply-templates select="MileageRS/LineNumber"/>
		<xsl:if test="ConversationID != ''">
			<ConversationID><xsl:value-of select="ConversationID"/></ConversationID>
		</xsl:if>
	</OTA_ShowMileageRS>
</xsl:template>

<xsl:template match="LineNumber">
	<ToCity>
		<xsl:attribute name="Mileage"><xsl:value-of select="TicketedPointMileage/@Code"/></xsl:attribute>
		<xsl:attribute name="AccumulativeMileage"><xsl:value-of select="CumulativeTicketedPointMileage/@Code"/></xsl:attribute>
		<xsl:value-of select="DestinationLocation/@LocationCode"/>
	</ToCity>
	<xsl:if test="position() = last()">
		<TotalMileage>
			<xsl:value-of select="CumulativeTicketedPointMileage/@Code"/>
		</TotalMileage>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>




