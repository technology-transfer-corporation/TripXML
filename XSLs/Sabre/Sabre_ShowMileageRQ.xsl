<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	
	<xsl:template match="/">
		<MileageRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07">
			<xsl:apply-templates select="OTA_ShowMileageRQ" />
		</MileageRQ>
	</xsl:template>
	
	<xsl:template match="OTA_ShowMileageRQ">
		<POS xmlns="http://webservices.sabre.com/sabreXML/2003/07">
			<Source xmlns="http://webservices.sabre.com/sabreXML/2003/07">
				<xsl:attribute name="PseudoCityCode">
					<xsl:value-of select="POS/Source/@PseudoCityCode" />
				</xsl:attribute>
			</Source>
		</POS>
		<OriginLocation xmlns="http://webservices.sabre.com/sabreXML/2003/07">
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="FromCity" />
			</xsl:attribute>
		</OriginLocation>
		<xsl:apply-templates select="ToCity" />
	</xsl:template>
	
	<xsl:template match="ToCity">
		<DestinationLocation xmlns="http://webservices.sabre.com/sabreXML/2003/07">
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="." />
			</xsl:attribute>
		</DestinationLocation>
	</xsl:template>
	
</xsl:stylesheet>
