<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<Table>
			<xsl:apply-templates select="Table/Airport"/>
		</Table>
	</xsl:template>
	
	<xsl:template match="Airport">
		<Airport>
			<Code><xsl:value-of select="Code"/></Code>
			<Name>
				<xsl:value-of select="substring(Name,1,1)"/>
				<xsl:value-of select="translate(substring(Name,2),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
			</Name>
		</Airport>
	</xsl:template>
</xsl:stylesheet>
