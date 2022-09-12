<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehLocDetailRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_VehLocDetailRQ">
		<OTA_VehLocDetailRQ Version="Sabre1.01" xmlns="http://www.opentravel.org/OTA/2002/11">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
					<xsl:if test="POS/Source/@ISOCountry != ''">
						<xsl:attribute name="ISOCountry">
							<xsl:value-of select="POS/Source/@ISOCountry" />
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="POS/Source/@ISOCurrency != ''">
						<xsl:attribute name="ISOCurrency">
							<xsl:value-of select="POS/Source/@ISOCurrency" />
						</xsl:attribute>
					</xsl:if>
				</Source>
			</POS>
			<Location>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="Location/@LocationCode" />
				</xsl:attribute>
			</Location>
			<xsl:if test="Vendor/@Code !=''">
				<Vendor>
					<xsl:attribute name="Code">
						<xsl:value-of select="Vendor/@Code" />
					</xsl:attribute>
				</Vendor>
			</xsl:if>
			<TPA_Extensions>
				<DropOffDate>
					<xsl:attribute name="Date">
						<xsl:value-of select="TPA_Extensions/DropOffDate/@Date" />
					</xsl:attribute>
				</DropOffDate>
				<PickUpDate>
					<xsl:attribute name="Date">
						<xsl:value-of select="TPA_Extensions/PickUpDate/@Date" />
					</xsl:attribute>
				</PickUpDate>
			</TPA_Extensions>
		</OTA_VehLocDetailRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
