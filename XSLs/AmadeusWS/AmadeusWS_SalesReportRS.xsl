<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_SalesReportRS.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 18 Oct 2012 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<SalesReportRS Version="1.0">
			<Screen>
				<xsl:apply-templates select="SalesReportRS/Screen"/>
			</Screen>
		</SalesReportRS>
	</xsl:template>
	<xsl:template match="Screen">
		<xsl:apply-templates select="Line[1]"></xsl:apply-templates>
	</xsl:template>
	<xsl:template match="Line">
		<xsl:variable name="line"><xsl:value-of select="."/></xsl:variable>
			<xsl:choose>
				<xsl:when test="following-sibling::Line[. = $line]"></xsl:when>
				<xsl:when test=". = '&amp;gt;'"></xsl:when>
				<xsl:when test=". = '&gt;'"></xsl:when>
				<xsl:when test="contains(.,'------------')"></xsl:when>
				<xsl:otherwise>
					<Line>
						<xsl:value-of select="."/>
					</Line>
				</xsl:otherwise>
			</xsl:choose>
		<xsl:apply-templates select="following-sibling::Line[1]"></xsl:apply-templates>
	</xsl:template>
</xsl:stylesheet>
