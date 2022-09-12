<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
  v03_Sabre_PNRReadRQ.xsl 															
  ================================================================== 
  Date: 31 May 2021 - Kobelev - upgraded ReadRQ to GetReservationRQ version 1.19.0			
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_ReadRQ" />
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="OTA_ReadRQ">
    <GetReservationRQ Version="1.19.0" xmlns="http://webservices.sabre.com/pnrbuilder/v1_19">
      <Locator>
        <xsl:value-of select="UniqueID/@ID" />
      </Locator>
      <RequestType>Stateful</RequestType>
      <ReturnOptions PriceQuoteServiceVersion="4.0.0">
        <SubjectAreas>
          <SubjectArea>PRICE_QUOTE</SubjectArea>
        </SubjectAreas>
      </ReturnOptions>
    </GetReservationRQ>
  </xsl:template>
  <!--************************************************************************************************************	-->
</xsl:stylesheet>