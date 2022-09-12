<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<CrypticRS Version="1.0">
			<xsl:apply-templates select="Cryptic_GetScreen_Reply" />
			<xsl:apply-templates select="MessagesOnly_Reply" />
		</CrypticRS>
	</xsl:template>
	<xsl:template match="Cryptic_GetScreen_Reply">
		<xsl:if test="CAPI_Messages">
			<xsl:apply-templates select="CAPI_Messages" />
		</xsl:if>
		<Response>
			<xsl:value-of select="CAPI_Screen/Response" />
		</Response>
		<xsl:if test="ConversationID != ''">
			<ConversationID>
				<xsl:value-of select="ConversationID" />
			</ConversationID>
		</xsl:if>
	</xsl:template>
	<xsl:template match="CAPI_Messages">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</xsl:template>
</xsl:stylesheet>
