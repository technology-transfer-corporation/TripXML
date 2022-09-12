<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_PNRCancelRS.xsl 													-->
  <!-- ================================================================== -->
  <!-- Date: 03 Nov 2104 -  Rastko - added support for retruned errors	-->
  <!-- Date: 11 May 2012 - Shashin - display informations in error sections as warnings	-->
  <!-- Date: 21 Jun 2009 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:if test="boolean(PNR_Reply)">
      <xsl:apply-templates select="PNR_Reply" />
    </xsl:if>
    <xsl:if test="boolean(Error)">
      <xsl:apply-templates select="Error" />
    </xsl:if>
  </xsl:template>
  <xsl:template match="PNR_Reply">
    <OTA_CancelRS>
      <xsl:attribute name="Version">3.14</xsl:attribute>
      <xsl:attribute name="Status">Cancelled</xsl:attribute>
      <Success />
      <xsl:if test="generalErrorInfo/messageErrorInformation/errorDetail/qualifier='INF'">
        <xsl:apply-templates select="generalErrorInfo"></xsl:apply-templates>
      </xsl:if>
      <UniqueID>
        <xsl:attribute name="ID">
          <xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber" />
        </xsl:attribute>
      </UniqueID>
    </OTA_CancelRS>
  </xsl:template>
  <xsl:template match="Error">
    <OTA_CancelRS>
      <xsl:attribute name="Version">3.14</xsl:attribute>
      <xsl:attribute name="Status">Error</xsl:attribute>
      <Errors>
        <Error>
          <xsl:value-of select="." />
        </Error>
      </Errors>
    </OTA_CancelRS>
  </xsl:template>
  <xsl:template match="generalErrorInfo">
    <Warnings>
      <Warning>
        <xsl:attribute name="Type">Amadeus</xsl:attribute>
        <xsl:value-of select="messageErrorText/text" />
      </Warning>
    </Warnings>
  </xsl:template>
</xsl:stylesheet>
