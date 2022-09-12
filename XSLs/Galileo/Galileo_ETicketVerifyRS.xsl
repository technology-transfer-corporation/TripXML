<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_ETicketVerifyRS.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 06 Oct 2005 - New file - Rastko												-->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<xsl:apply-templates select="DocProdETicketCheck_1_0" />
	<xsl:apply-templates select="PNRBFManagement_7_9" />
</xsl:template>

<xsl:template match="DocProdETicketCheck_1_0">
	<OTA_ETicketVerifyRS Version="1.000">
		<xsl:choose>
			<xsl:when test="Eligibility/DPOK">
				<Success />
				<ETicket>true</ETicket>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="ErrText" />
			</xsl:otherwise>
		</xsl:choose>
	</OTA_ETicketVerifyRS>
</xsl:template>

<xsl:template match="PNRBFManagement_7_9">
	<OTA_ETicketVerifyRS Version="1.000">
		<xsl:apply-templates select="AirSegSell/ErrText" />
	</OTA_ETicketVerifyRS>
</xsl:template>

<xsl:template match="ErrText">
	<Errors>
		<Error Type="Galileo"><xsl:value-of select="Text"/></Error>
	</Errors>
</xsl:template>

</xsl:stylesheet>