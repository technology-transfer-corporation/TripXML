<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CCValidRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_CCValidRS>
			<xsl:apply-templates select="Command_CrypticReply" />
		</OTA_CCValidRS>
	</xsl:template>
	<xsl:template match="Command_CrypticReply">
		<xsl:choose>
			<xsl:when test="contains(longTextString/textStringDetails,'AX  ') or (starts-with(longTextString/textStringDetails,'/') and contains(longTextString/textStringDetails,'VI')) or substring-before(longTextString/textStringDetails,' ') = ''">
				<Success />
				<ApprovalCode>
					<xsl:choose>
						<xsl:when test="contains(longTextString/textStringDetails,'AX  ')">
							<xsl:value-of select="substring-after(substring-before(longTextString/textStringDetails,'>'),'AX  ')" />
						</xsl:when>
						<xsl:when test="contains(longTextString/textStringDetails,'VI')">
							<xsl:value-of select="substring-after(longTextString/textStringDetails,'VI')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(longTextString/textStringDetails,'>')" />
						</xsl:otherwise>
					</xsl:choose>
				</ApprovalCode>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<Error Type="Amadeus" Code="INV">
						<xsl:value-of select="substring-before(longTextString/textStringDetails,'>')" />
					</Error>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
