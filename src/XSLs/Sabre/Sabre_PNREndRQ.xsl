<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
  Sabre_PNREndRQ.xsl 										
  ================================================================== 
  Date: 12 Dec 2024 - Kobelev - Created								
  ================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes" />

	<xsl:template match="/">
		<PNREnd>
			<xsl:apply-templates select="OTA_PNREndRQ" />
		</PNREnd>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_PNREndRQ">
		<Read>
			<TravelItineraryReadRQ Version="3.10.0" xmlns="http://services.sabre.com/res/tir/v3_10">
				<MessagingDetails>
					<SubjectAreas>
						<SubjectArea>FULL</SubjectArea>
					</SubjectAreas>
				</MessagingDetails>
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="UniqueID/@ID" />
					</xsl:attribute>
				</UniqueID>
			</TravelItineraryReadRQ>
		</Read>
		<ET>
			<EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
				<EndTransaction Ind="true" />
				<Source ReceivedFrom="6P"/>
			</EndTransactionRQ>
		</ET>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
