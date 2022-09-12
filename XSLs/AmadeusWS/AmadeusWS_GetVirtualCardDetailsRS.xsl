<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ==================================================================
   AmadeusWS_GetVirtualCardDetailsRS.xsl								
  ================================================================== 
   Date: 03 May 2021 - Kobelev - Added Conversation ID to schema	
   Date: 22 Jan 2019 - Kobelev - Adjustment for TripXML schema											
   Date: 16 Jan 2019 - Rastko - new file											
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:apply-templates select="AMA_PAY_GetVirtualCardDetailsRS"/>
  </xsl:template>
  <xsl:template match="AMA_PAY_GetVirtualCardDetailsRS">
    <PAY_GetVirtualCardDetailsRS>
      <xsl:if test="Failure">
        <Errors>
          <xsl:apply-templates select="Failure"/>
        </Errors>
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
    </PAY_GetVirtualCardDetailsRS>
  </xsl:template>
  <xsl:template match="Failure">
    <xsl:for-each select="Errors/Error">
      <Error>
        <xsl:attribute name="Type">
          <xsl:value-of select="@Type"/>  
        </xsl:attribute>
        <xsl:attribute name="ShortText">
          <xsl:value-of select="@ShortText"/>  
        </xsl:attribute>
        <xsl:attribute name="Code">
          <xsl:value-of select="@Code"/>  
        </xsl:attribute>
        <xsl:value-of select="@ShortText"/>
      </Error>
    </xsl:for-each>   
  </xsl:template>
  <xsl:template match="Warnings">
    <xsl:copy-of select="Warnings"/>
  </xsl:template>
  <xsl:template match="Success">
    <Success />
    <xsl:if test="Warnings">
      <xsl:apply-templates select="Warnings"/>
    </xsl:if>
    <xsl:if test="VirtualCard">
      <VirtualCard>
        <xsl:attribute name="LastUpdatedTime">
          <xsl:value-of select="VirtualCard/@LastUpdatedTime"/>
        </xsl:attribute>
        <xsl:attribute name="CreationTime">
          <xsl:value-of select="VirtualCard/@CreationTime"/>
        </xsl:attribute>
        <xsl:attribute name="CreationUser">
          <xsl:value-of select="VirtualCard/@CreationUser"/>
        </xsl:attribute>
        <xsl:attribute name="CreationOffice">
          <xsl:value-of select="VirtualCard/@CreationOffice"/>
        </xsl:attribute>
        <xsl:attribute name="CardStatus">
          <xsl:value-of select="VirtualCard/@CardStatus"/>
        </xsl:attribute>
        <xsl:apply-templates select="VirtualCard"/>
      </VirtualCard>
    </xsl:if>
    <xsl:copy-of select="Transactions"/>
    <xsl:copy-of select="ReportingInfo"/>
    <xsl:copy-of select="Reservation"/>
  </xsl:template>
  
  <!-- 
  ********************************************
  Virtual Card Display  
  ******************************************** 
  -->
  <xsl:template match="VirtualCard">
    <xsl:if test="Card">
      <xsl:copy-of select="Card"/>
    </xsl:if>
    <xsl:if test="References">
      <xsl:copy-of select="References"/>
    </xsl:if>
    <xsl:if test="Provider">
      <xsl:copy-of select="Provider"/>
    </xsl:if>
    <xsl:if test="Values">
      <xsl:copy-of select="Values"/>
    </xsl:if>    
    <xsl:if test="Limitations">
      <xsl:copy-of select="Limitations"/>
    </xsl:if>     
  </xsl:template>
  
</xsl:stylesheet>
