<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<Z>
		<xsl:apply-templates select="Z/Y"/>
	</Z>
</xsl:template>

<xsl:template match="Y">
	<xsl:variable name="city" select="b"/>
	<xsl:choose>
		<xsl:when test="following-sibling::Y[1]/b != $city or position()=last()">
			<Y><a><xsl:value-of select="a"/></a><b><xsl:value-of select="b"/></b><c><xsl:value-of select="c"/></c><d><xsl:value-of select="d"/></d></Y><xsl:value-of select="'&#10;'"/>
		</xsl:when>
	</xsl:choose>
</xsl:template>  

</xsl:stylesheet>