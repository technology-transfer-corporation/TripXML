<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ==================================================================
   AmadeusWS_ListVirtualCardsRS.xsl									
  ================================================================== 
   Date: 03 May 2021 - Kobelev - Added Conversation ID to schema
   Date: 21 Jan 2019 - Kobelev - Adjustment for TripXML schema											
   Date: 16 Jan 2019 - Rastko - new file											
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:apply-templates select="AMA_PAY_ListVirtualCardsRS"/>
  </xsl:template>
  <xsl:template match="AMA_PAY_ListVirtualCardsRS">
    <PAY_ListVirtualCardsRS>
      <xsl:if test="Errors">
        <xsl:apply-templates select="Errors"/>
      </xsl:if>
      <xsl:if test="Warnings">
        <xsl:apply-templates select="Warnings"/>
      </xsl:if>
      <xsl:if test="Success">
        <xsl:apply-templates select="Success"/>
      </xsl:if>
      <xsl:if test="ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:if>
    </PAY_ListVirtualCardsRS>
  </xsl:template>
  <xsl:template match="Errors">
    <xsl:copy-of select="Errors"/>
  </xsl:template>
  <xsl:template match="Warnings">
    <xsl:copy-of select="Warnings"/>
  </xsl:template>
  <xsl:template match="Success">
    <Success />
    <xsl:if test="Warnings">
      <xsl:apply-templates select="Warnings"/>
    </xsl:if>
    <VirtualCards>
      <xsl:apply-templates select="VirtualCard"/>
    </VirtualCards>
  </xsl:template>
  <xsl:template match="VirtualCard">
    <xsl:copy-of select="."/>
  </xsl:template>

</xsl:stylesheet>
