<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ======================================================================= -->
	<!-- Author  : Rastko Ilic										                                           -->
	<!-- Date    : 16 Sep 2004									                                                         -->
	<!-- ======================================================================= -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PoweredLowestFare_SearchReply" />
		<xsl:apply-templates select="MessagesOnly_Reply" />
	</xsl:template>

	<xsl:template match="MessagesOnly_Reply">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="PoweredLowestFare_SearchReply">
		<PoweredLowestFare_SearchReply>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<xsl:copy-of select="errorMessage"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="replyStatus"/>
					<xsl:copy-of select="conversionRate"/>
					<xsl:apply-templates select="recommendation">
						<xsl:with-param name="ods"><xsl:value-of select="count(flightIndex)"/></xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</PoweredLowestFare_SearchReply>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="recommendation">
		<xsl:param name="ods"/>
		<recommendation>
			<xsl:copy-of select="itemNumber"/>
			<xsl:copy-of select="recPriceInfo"/>
			<xsl:copy-of select="paxFareProduct"/>
			<xsl:call-template name="ods">
				<xsl:with-param name="ods"><xsl:value-of select="$ods"/></xsl:with-param>
				<xsl:with-param name="rank">1</xsl:with-param>
			</xsl:call-template>
		</recommendation>
	</xsl:template>
	
	<xsl:template name="ods">
		<xsl:param name="ods"/>
		<xsl:param name="rank"/>
		<xsl:if test="$ods > 0">
			<segmentFlightRef>
				<xsl:apply-templates select="segmentFlightRef/referencingDetail[position() = $rank]">
					<xsl:with-param name="rank"><xsl:value-of select="$rank"/></xsl:with-param>
				</xsl:apply-templates>
			</segmentFlightRef>	
			<xsl:call-template name="ods">
				<xsl:with-param name="ods"><xsl:value-of select="$ods - 1"/></xsl:with-param>
				<xsl:with-param name="rank"><xsl:value-of select="$rank + 1"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="referencingDetail">
		<xsl:param name="rank"/>
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="ref"><xsl:value-of select="refNumber"/></xsl:variable>
		<xsl:variable name="prevref">
			<xsl:choose>
				<xsl:when test="position() = 1">0</xsl:when>
				<xsl:otherwise>
					<!--xsl:value-of select="../../segmentFlightRef[1]/referencingDetail[position() = $rank]/refNumber"/-->
					<xsl:for-each select="../../segmentFlightRef[position() &lt; $pos]/referencingDetail[position() = $rank]">
						<xsl:value-of select="refNumber"/>
						<xsl:text>/</xsl:text>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="not(contains($prevref,$ref))">
			<RPH>
				<xsl:copy-of select="../../../flightIndex[position() = $rank]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]/flightDetails"/>
			</RPH>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
