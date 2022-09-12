<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CrypticRS" />
	</xsl:template>
	<xsl:template match="CrypticRS">
		<CrypticRS>
			<Success />
			<Response>
				<xsl:value-of select="Response" />
			</Response>
			<Screen>
				<xsl:apply-templates select="Screen/Line"/>
			</Screen>
			<ConversationID>
				<xsl:value-of select="ConversationID" />
			</ConversationID>
		</CrypticRS>
	</xsl:template>
	
	<xsl:template match="Line">
		<Line><xsl:value-of select="."/></Line>
	</xsl:template>
</xsl:stylesheet>
