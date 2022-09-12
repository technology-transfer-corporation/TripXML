<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CreditValidation_2_0" />
	</xsl:template>
	<xsl:template match="CreditValidation_2_0">
		<OTA_CCValidRS>
			<xsl:apply-templates select="CCNumVerify" />
		</OTA_CCValidRS>
	</xsl:template>
	<xsl:template match="CCNumVerify">
		<xsl:choose>
			<xsl:when test=".=''">
				<Success />
				<ApprovalCode>OK</ApprovalCode>
			</xsl:when>
			<xsl:when test="ErrorCode!=''">
				<Errors>
					<Error Type="Galileo" Code="INV">Invalid Card</Error>
				</Errors>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>