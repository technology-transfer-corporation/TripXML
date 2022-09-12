<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="boolean(Sabre_OTA_ProfileCreateRS/ResponseMessage/Errors)">
        <xsl:apply-templates select="Sabre_OTA_ProfileCreateRS/ResponseMessage/Errors" />
      </xsl:when>
      <xsl:when test="boolean(Sabre_OTA_ProfileCreateRS)">
        <xsl:apply-templates select="Sabre_OTA_ProfileCreateRS" />
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Sabre_OTA_ProfileCreateRS">
    <OTA_ProfileCreateRS>
      <xsl:attribute name="Version">3.14</xsl:attribute>
      <Success />
      <UniqueID>
        <xsl:attribute name="ID">
          <xsl:value-of select="Profile/@UniqueID" />
        </xsl:attribute>
      </UniqueID>
    </OTA_ProfileCreateRS>
  </xsl:template>
  <xsl:template match="Errors">
    <OTA_ProfileCreateRS>
      <xsl:attribute name="Status">Error</xsl:attribute>
      <Errors>
        <xsl:for-each select="ErrorMessage">
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:value-of select="." />
          </Error>
        </xsl:for-each>
      </Errors>
    </OTA_ProfileCreateRS>
  </xsl:template>
</xsl:stylesheet>
