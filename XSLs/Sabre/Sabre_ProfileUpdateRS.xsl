<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="boolean(Sabre_OTA_ProfileReadRS/ResponseMessage/Errors)">
        <xsl:apply-templates select="Sabre_OTA_ProfileReadRS/ResponseMessage/Errors" />
      </xsl:when>
      <xsl:when test="boolean(Sabre_OTA_ProfileUpdateRS/ResponseMessage/Errors)">
        <xsl:apply-templates select="Sabre_OTA_ProfileUpdateRS/ResponseMessage/Errors" />
      </xsl:when>
      <xsl:when test="boolean(Sabre_OTA_ProfileDeleteRS/ResponseMessage/Errors)">
        <xsl:apply-templates select="Sabre_OTA_ProfileDeleteRS/ResponseMessage/Errors" />
      </xsl:when>
      <xsl:when test="boolean(Sabre_OTA_ProfileUpdateRS)">
        <xsl:apply-templates select="Sabre_OTA_ProfileUpdateRS" />
      </xsl:when>
      <xsl:when test="boolean(Sabre_OTA_ProfileDeleteRS)">
        <xsl:apply-templates select="Sabre_OTA_ProfileDeleteRS" />
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Sabre_OTA_ProfileUpdateRS">
    <OTA_ProfileUpdateRS>
      <Success />
      <UniqueID>
        <xsl:attribute name="ID">
          <xsl:value-of select="Profile/@UniqueID" />
        </xsl:attribute>
      </UniqueID>
    </OTA_ProfileUpdateRS>
  </xsl:template>
  <xsl:template match="Sabre_OTA_ProfileDeleteRS">
    <OTA_ProfileUpdateRS>
      <Success />
      <UniqueID>
        <xsl:attribute name="ID">
          <xsl:value-of select="Profile/TPA_Identity/@UniqueID" />
        </xsl:attribute>
      </UniqueID>
    </OTA_ProfileUpdateRS>
  </xsl:template>
  <xsl:template match="Errors">
    <OTA_ProfileUpdateRS>
      <xsl:attribute name="Status">Error</xsl:attribute>
      <Errors>
        <xsl:for-each select="ErrorMessage">
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:value-of select="." />
          </Error>
        </xsl:for-each>
      </Errors>
    </OTA_ProfileUpdateRS>
  </xsl:template>
</xsl:stylesheet>
