<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<a>
			<xsl:apply-templates select="a/Airport" mode="api"/>
			<xsl:apply-templates select="a/Airport" mode="city"/>
			<xsl:apply-templates select="a/Airport" mode="local"/>
		</a>
	</xsl:template>
	
	<xsl:template match="Airport" mode="api">
		<api><xsl:value-of select="concat('	0	Odyssey	',Code,'	',Code)"/></api>
	</xsl:template>
	
	<xsl:template match="Airport" mode="city">
		<city><xsl:value-of select="concat('	FR	',Code,'				1	5/24/2009	5/24/2009	4	False	True')"/></city>
	</xsl:template>

	<xsl:template match="Airport" mode="local">
		<local><xsl:value-of select="concat('	',Code,'	1	',substring-before(Name,','),'		')"/></local>
		<local><xsl:value-of select="concat('	',Code,'	2	',substring-before(Name,','),'		')"/></local>
		<local><xsl:value-of select="concat('	',Code,'	3	',substring-before(Name,','),'		')"/></local>
		<local><xsl:value-of select="concat('	',Code,'	4	',substring-before(Name,','),'		')"/></local>
	</xsl:template>
</xsl:stylesheet>
