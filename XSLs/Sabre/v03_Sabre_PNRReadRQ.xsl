<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- v03_Sabre_PNRReadRQ.xsl 															-->
  <!-- ================================================================== -->
  <!-- Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				-->
  <!-- Date: 17 Feb 2014 - Rastko - made ID optional to so it can do a redisplay			-->
  <!-- Date: 15 Aug 2008 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_ReadRQ" />
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="OTA_ReadRQ">
    <TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
      <MessagingDetails>
        <SubjectAreas>
          <SubjectArea>FULL</SubjectArea>
        </SubjectAreas>
      </MessagingDetails>
      <xsl:if test="UniqueID/@ID!=''">
        <UniqueID>
          <xsl:attribute name="ID">
            <xsl:value-of select="UniqueID/@ID" />
          </xsl:attribute>
        </UniqueID>
      </xsl:if>
      <xsl:apply-templates select="VerifyTickets"/>
     
    </TravelItineraryReadRQ>
  </xsl:template>
  <!--
  ************************************************************************************************************	
    Verify Tickets
  ************************************************************************************************************	
  -->
  <xsl:template match="VerifyTickets">
    <VerifyTickets>
      <xsl:for-each select="TicketDate">
        <xsl:variable name="dt" select="."/>
        <xsl:for-each select="../PseudoCityCode">
          <DailySalesReportRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0">
            <SalesReport StartDate="{substring($dt,1,10)}" PseudoCityCode="{.}" />
          </DailySalesReportRQ>
        </xsl:for-each>
      </xsl:for-each>
    </VerifyTickets>
  </xsl:template>
  
</xsl:stylesheet>
