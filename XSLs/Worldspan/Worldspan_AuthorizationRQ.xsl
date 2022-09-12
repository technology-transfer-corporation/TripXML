<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ==================================================================
   Worldspan_AuthorizationRQ.xsl 														
  ================================================================== 
   Date: 14 Feb 2018 - Kobelev	- Optional fields corrected usage.
   Date: 02 Feb 2018 - Kobelev	- POS Fix	
   Date: 31 Jan 2018 - Kobelev	- new file	
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_AuthorizationRQ" />
  </xsl:template>
  <xsl:template match="OTA_AuthorizationRQ">
    <OTA_AuthorizationRQ Version="1" xmlns="http://www.opentravel.org/OTA/2003/05">
      <POS>
        <xsl:choose>
          <xsl:when test="POS/Source/@AgentSine">
            <Source>
              <xsl:attribute name="AgentSine">
                <xsl:value-of select="POS/Source/@AgentSine"/>
              </xsl:attribute>
              <xsl:attribute name="ISOCountry">
                <xsl:value-of select="POS/Source/@ISOCountry"/>
              </xsl:attribute>
            </Source>
          </xsl:when>
          <xsl:otherwise>
            <Source>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="POS/Source/@PseudoCityCode"/>
              </xsl:attribute>
              <RequestorID>
                <xsl:attribute name="Type">
                  <xsl:value-of select="POS/Source/RequestorID/@Type"/>
                </xsl:attribute>
                <xsl:attribute name="ID">
                  <xsl:value-of select="POS/Source/RequestorID/@ID"/>
                </xsl:attribute>
              </RequestorID>
            </Source>            
          </xsl:otherwise>
        </xsl:choose>
      </POS>
      <AuthorizationDetail>
        <xsl:if test="AuthorizationDetail/@PrincipalCompanyCode">
          <xsl:if test="AuthorizationDetail/@PrincipalCompanyCode != ''">
            <xsl:attribute name="PrincipalCompanyCode">
              <xsl:value-of select="AuthorizationDetail/@PrincipalCompanyCode"/>
            </xsl:attribute>
          </xsl:if>
        </xsl:if>
        <CreditCardAuthorization>
          <xsl:attribute name="Amount">
            <xsl:value-of select="AuthorizationDetail/CreditCardAuthorization/@Amount"/>
          </xsl:attribute>
          <xsl:attribute name="CardPresentInd">
            <xsl:value-of select="AuthorizationDetail/CreditCardAuthorization/@CardPresentInd"/>
          </xsl:attribute>
          <CreditCard>
            <xsl:attribute name="CardCode">
              <xsl:value-of select="AuthorizationDetail/CreditCardAuthorization/CreditCard/@CardCode"/>
            </xsl:attribute>
            <xsl:attribute name="CardNumber">
              <xsl:value-of select="AuthorizationDetail/CreditCardAuthorization/CreditCard/@CardNumber"/>
            </xsl:attribute>
            <xsl:attribute name="ExpireDate">
              <xsl:value-of select="AuthorizationDetail/CreditCardAuthorization/CreditCard/@ExpireDate"/>
            </xsl:attribute>
          </CreditCard>
        </CreditCardAuthorization>
        <BookingReferenceID>
          <xsl:attribute name="ID">
            <xsl:value-of select="AuthorizationDetail/BookingReferenceID/@ID"/>
          </xsl:attribute>
          <xsl:attribute name="Type">
            <xsl:value-of select="AuthorizationDetail/BookingReferenceID/@Type"/>
          </xsl:attribute>
        </BookingReferenceID>
      </AuthorizationDetail>
    </OTA_AuthorizationRQ>
  </xsl:template>
</xsl:stylesheet>
