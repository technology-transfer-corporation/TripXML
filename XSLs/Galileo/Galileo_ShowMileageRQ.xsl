<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<ShowMileage>
			<Session>
				<xsl:apply-templates select="OTA_ShowMileageRQ/POS" mode="Open" />
			</Session>
			<xsl:apply-templates select="OTA_ShowMileageRQ" />
			<EndSession>
				<xsl:apply-templates select="OTA_ShowMileageRQ/POS" mode="Close" />
			</EndSession>
		</ShowMileage>
	</xsl:template>
	<xsl:template match="OTA_ShowMileageRQ">
		<CrypticRQ Version="Galileo1.01">
			<Entry>
				<xsl:text>$LM/</xsl:text>
				<xsl:value-of select="FromCity" />
				<xsl:apply-templates select="ToCity" />
			</Entry>
		</CrypticRQ>
	</xsl:template>
	<xsl:template match="ToCity">
		<xsl:text>/</xsl:text>
		<xsl:value-of select="." />
	</xsl:template>
	<xsl:template match="POS" mode="Open">
		<SessionCreateRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
		</SessionCreateRQ>
	</xsl:template>
	<xsl:template match="POS" mode="Close">
		<SessionCloseRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
		</SessionCloseRQ>
	</xsl:template>
</xsl:stylesheet>
