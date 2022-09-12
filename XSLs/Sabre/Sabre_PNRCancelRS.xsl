<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_PNRCancelRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 01 Sep 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="EndTransactionRS" />
		<xsl:apply-templates select="OTA_CancelRS"/>
		<xsl:apply-templates select="OTA_TravelItineraryRS" />
	</xsl:template>
	<!-- **************************************************************************************	-->
	<xsl:template match="EndTransactionRS">
		<OTA_CancelRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:text>END TRANSACTION ERROR</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="Success">
					<xsl:attribute name="Status">Cancelled</xsl:attribute>
					<Success />
					<UniqueID>
						<xsl:attribute name="ID">
							<xsl:value-of select="UniqueID/@ID" />
						</xsl:attribute>
					</UniqueID>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Status">Unsuccessful</xsl:attribute>
					<UniqueID>
						<xsl:attribute name="ID">
							<xsl:value-of select="TravelItinerary/ItineraryRef/@Id" />
						</xsl:attribute>
					</UniqueID>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_CancelRS>
	</xsl:template>
	
	<xsl:template match="OTA_TravelItineraryRS">
		<OTA_CancelRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:text>PNR NOT FOUND</xsl:text>
					</Error>
				</Errors>
		</OTA_CancelRS>
	</xsl:template>
	
	<xsl:template match="OTA_CancelRS">
		<OTA_CancelRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:text>NO ITINERARY TO CANCEL</xsl:text>
					</Error>
				</Errors>
		</OTA_CancelRS>
	</xsl:template>
	<!-- **************************************************************************************	-->
</xsl:stylesheet>