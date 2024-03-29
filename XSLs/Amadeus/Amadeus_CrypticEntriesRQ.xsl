<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CrypticEntriesRQ" />
	</xsl:template>
	
	<xsl:template match="CrypticEntriesRQ">
		<Cryptic>
			<xsl:apply-templates select="Entry"/>
		</Cryptic>
	</xsl:template>
	
	<xsl:template match="Entry">
		<Cryptic_GetScreen_Query>
			<Command>
				<xsl:value-of select="." />
			</Command>
		</Cryptic_GetScreen_Query>
	</xsl:template>
</xsl:stylesheet>
