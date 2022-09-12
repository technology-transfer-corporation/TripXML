<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<CrypticRS Version="1.0">
			<xsl:apply-templates select="SabreCommandLLSRS" />
		</CrypticRS>
	</xsl:template>
	<xsl:template match="SabreCommandLLSRS">
    <xsl:choose>
      <xsl:when test="ErrorRS/Errors">
        <Errors>
          <xsl:for-each select="ErrorRS/Errors/Error">
            <Error>
              <xsl:value-of select="ErrorInfo/Message" />
            </Error>
          </xsl:for-each>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <Success/>
        <Response>
          <xsl:value-of select="Response" />
        </Response>        
      </xsl:otherwise>
    </xsl:choose>   
    
		<xsl:if test="ConversationID != ''">
			<ConversationID>
				<xsl:value-of select="ConversationID" />
			</ConversationID>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
