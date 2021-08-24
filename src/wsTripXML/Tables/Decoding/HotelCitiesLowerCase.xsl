<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<Z>
		<xsl:apply-templates select="Z/Y"/>
	</Z>
</xsl:template>

<xsl:template match="Y">
	<xsl:variable name="city">
		<xsl:call-template name="lowercase">
			<xsl:with-param name="city" select="b"/>
		</xsl:call-template>
	</xsl:variable>
	<Y><a><xsl:value-of select="a"/></a><b><xsl:value-of select="$city"/></b><c><xsl:value-of select="c"/></c><d><xsl:value-of select="d"/></d></Y><xsl:value-of select="'&#10;'"/>
</xsl:template>  

<xsl:template name="lowercase">
	<xsl:param name="city"/>
	<xsl:if test="$city!=''">
		<xsl:variable name="ci">
			<xsl:value-of select="substring($city,1,1)"/>
			<xsl:choose>
				<xsl:when test="contains($city,' ')">
					<xsl:variable name="name" select="substring-before($city,' ')"/>
					<xsl:value-of select="translate(substring($name,2),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
					<xsl:value-of select="' '"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(substring($city,2),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$ci"/>
		<xsl:variable name="cit">
			<xsl:if test="contains($city,' ')">
				<xsl:value-of select="substring-after($city,' ')"/>
			</xsl:if>
		</xsl:variable>
		<xsl:call-template name="lowercase">
			<xsl:with-param name="city" select="$cit"/>
		</xsl:call-template>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>