<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CCValidRQ/CreditCard" />
	</xsl:template>
	<xsl:template match="CreditCard">
		<CreditValidation_2_0>
			<CCNumVerifyMods>
				<Cmd>JV</Cmd>
				<CCData>
				<xsl:choose>
						<xsl:when test="Code='MC'">CA</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="Code" />
						</xsl:otherwise>
					</xsl:choose>
				<xsl:value-of select="Number" />/V1.00USD</CCData>
			</CCNumVerifyMods>
		</CreditValidation_2_0>
	</xsl:template>
</xsl:stylesheet>
