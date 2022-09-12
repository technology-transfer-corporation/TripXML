<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<CrypticEntriesRS Version="1.0">
			<xsl:apply-templates select="CrypticRS"/>
		</CrypticEntriesRS>
	</xsl:template>
	
	<xsl:template match="CrypticRS">
		<Success/>
		<xsl:apply-templates select="Cryptic_GetScreen_Reply" />
		<xsl:apply-templates select="MessagesOnly_Reply[1]" mode="first"/>
	</xsl:template>
	
	<xsl:template match="Cryptic_GetScreen_Reply">
		<xsl:if test="CAPI_Messages">
			<xsl:apply-templates select="CAPI_Messages" />
		</xsl:if>
		<Response>
			<xsl:value-of select="CAPI_Screen/Response" />
		</Response>
	</xsl:template>
	
	<xsl:template match="MessagesOnly_Reply" mode="first">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
			<xsl:apply-templates select="following-sibling::MessagesOnly_Reply" mode="next"/>
		</Errors>
	</xsl:template>
	
	<xsl:template match="MessagesOnly_Reply" mode="next">
		<xsl:apply-templates select="CAPI_Messages" />
	</xsl:template>

	<xsl:template match="CAPI_Messages">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
</xsl:stylesheet>
