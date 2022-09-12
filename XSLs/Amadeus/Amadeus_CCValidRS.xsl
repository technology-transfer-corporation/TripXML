<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_CCValidRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 18 Jan 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_CCValidRS>
			<xsl:apply-templates select="//Cryptic_GetScreen_Reply" />
		</OTA_CCValidRS>
	</xsl:template>
	<xsl:template match="Cryptic_GetScreen_Reply">
		<xsl:choose>
			<xsl:when test="contains(CAPI_Screen/Response,'AX  ') or substring-before(CAPI_Screen/Response,' ') = ''">
				<Success />
				<ApprovalCode>
					<xsl:choose>
						<xsl:when test="contains(CAPI_Screen/Response,'AX  ')">
							<xsl:value-of select="substring-after(substring-before(CAPI_Screen/Response,'>'),'AX  ')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(CAPI_Screen/Response,'>')" />
						</xsl:otherwise>
					</xsl:choose>
				</ApprovalCode>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<Error Type="Amadeus" Code="INV">
						<xsl:value-of select="substring-before(CAPI_Screen/Response,'>')" />
					</Error>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
