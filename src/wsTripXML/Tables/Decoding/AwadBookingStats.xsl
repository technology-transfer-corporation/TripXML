<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<AwadStats>
			<xsl:apply-templates select="awad/Booking[1]">
				<xsl:with-param name="count">0</xsl:with-param>
			</xsl:apply-templates>
		</AwadStats>
	</xsl:template>
	<xsl:template match="Booking">
		<xsl:param name="count"/>
		<xsl:variable name="count1">
			<xsl:choose>
				<xsl:when test="following-sibling::Booking[1]/@Start != @Start or not(following-sibling::Booking[1])">
					<xsl:value-of select="0"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$count + 1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="following-sibling::Booking[1]/@Start != @Start or not(following-sibling::Booking[1])">
			<CP>
				<xsl:attribute name="DC"><xsl:value-of select="substring(@Start,1,3)"/></xsl:attribute>
				<xsl:attribute name="AC"><xsl:value-of select="substring(@Start,4)"/></xsl:attribute>
				<xsl:attribute name="Count"><xsl:value-of select="$count + 1"/></xsl:attribute>
			</CP>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::Booking[1]">
			<xsl:with-param name="count" select="$count1"/>
		</xsl:apply-templates>
	</xsl:template>
	<!--xsl:template match="Booking">
		<xsl:param name="count"/>
		<xsl:variable name="count1">
			<xsl:choose>
				<xsl:when test="following-sibling::Booking[1]/@Code != @Code or not(following-sibling::Booking[1])">
					<xsl:value-of select="0"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$count + 1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="following-sibling::Booking[1]/@Code != @Code or not(following-sibling::Booking[1])">
			<AL>
				<xsl:attribute name="AC"><xsl:value-of select="@Code"/></xsl:attribute>
				<xsl:attribute name="Count"><xsl:value-of select="$count + 1"/></xsl:attribute>
			</AL>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::Booking[1]">
			<xsl:with-param name="count" select="$count1"/>
		</xsl:apply-templates>
	</xsl:template-->
</xsl:stylesheet>
