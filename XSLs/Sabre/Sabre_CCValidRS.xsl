<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

<xsl:output method="xml" omit-xml-declaration="yes"/>
<xsl:template match = "/">
	<OTA_CCValidRS>
		<xsl:apply-templates select="SabreCommandLLSRS/Response"/>
		<xsl:if test="ConversationID != ''">
			<ConversationID><xsl:value-of select="ConversationID"/></ConversationID>
		</xsl:if>
	</OTA_CCValidRS>
</xsl:template>

<xsl:template match="Response">
	<xsl:choose>
		<xsl:when test="string-length(string(.))=3 and substring(string(.),1,2)='* '">
			<Success/>
			<ApprovalCode>OK</ApprovalCode>
		</xsl:when>
		<xsl:otherwise>
			<Errors>
				<Error Type="Sabre" Code="INV">
					<xsl:value-of select="translate(string(.),'?','')"/>
				</Error>
			</Errors>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

</xsl:stylesheet>




