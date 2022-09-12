<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_PNRCancelRQ.xsl 														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PNRBFManagement_17" />
	</xsl:template>
	
	<xsl:template match="PNRBFManagement_17">
		<OTA_CancelRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="EndTransaction/ErrorCode != ''">
					<xsl:apply-templates select="EndTransaction/EndTransactMessage"/>
				</xsl:when>
				<xsl:when test="PNRBFRetrieve/ErrorCode != ''">
					<xsl:apply-templates select="PNRBFRetrieve/ErrText"/>
				</xsl:when>
				<xsl:when test="PNRBFRetrieve/GenPNRInfo">
					<xsl:apply-templates select="PNRBFRetrieve/GenPNRInfo" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="EndTransaction/EndTransactResponse" />
				</xsl:otherwise>
			</xsl:choose>
		</OTA_CancelRS>
	</xsl:template>
	
	<xsl:template match="GenPNRInfo | EndTransactResponse">
		<xsl:attribute name="Status">Cancelled</xsl:attribute>
		<Success />
		<UniqueID>
			<xsl:attribute name="ID"><xsl:value-of select="RecLoc"/></xsl:attribute>
		</UniqueID>
	</xsl:template>
	
	<xsl:template match="EndTransactMessage | ErrText">
		<xsl:attribute name="Status">Unsuccessful</xsl:attribute>
		<UniqueID>
			<xsl:attribute name="ID"><xsl:value-of select="//PNRBFManagement_17/GenPNRInfo/RecLoc"/></xsl:attribute>
		</UniqueID>
		<Errors>
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:attribute name="Code">
					<xsl:value-of select="../ErrorCode" />
				</xsl:attribute>
				<xsl:value-of select="Text"/>
			</Error>
		</Errors>
	</xsl:template>
</xsl:stylesheet>