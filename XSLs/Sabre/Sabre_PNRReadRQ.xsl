<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_PNRReadRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				-->
	<!-- Date: 24 Dec 2010 - Rastko - added mapping for Update message				-->
	<!-- Date: 15 Aug 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_ReadRQ | OTA_UpdateRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_ReadRQ | OTA_UpdateRQ">
		<TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
			<MessagingDetails>
				<SubjectAreas>
					<SubjectArea>FULL</SubjectArea>
				</SubjectAreas>
			</MessagingDetails>
			<xsl:if test="UniqueID/@ID!=''">
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="UniqueID/@ID" />
					</xsl:attribute>
				</UniqueID>
			</xsl:if>
		</TravelItineraryReadRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
