<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >

 <xsl:template name="processFaultstring">
  <xsl:if test="*/*/*/faultstring!= ''">
  <Errors>
    <Error>
      <xsl:attribute name="Code">
        <xsl:value-of select="*/*/*/faultcode"/>
      </xsl:attribute>
      <xsl:value-of select="*/*/*/faultstring"/>
    </Error>
  </Errors>
</xsl:if>
</xsl:template>

</xsl:stylesheet>