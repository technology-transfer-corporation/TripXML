<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- v03_AmadeusWS_AirRulesRS.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 03 Mar 2021 - Samokvalov - Fare_CheckRulesReply reworked to get FareRules for passengers per fare segments -->
  <!-- Date: 11 Aug 2011 - Rastko - mapped requested passenger types				-->
  <!-- Date: 25 Jul 2011 - Rastko - mapped real airports								-->
  <!-- Date: 07 Jul 2011 - Rastko - added pax type attribute							-->
  <!-- Date: 23 Mar 2010 - Rastko - corrected mapping of root element				-->
  <!-- Date: 17 Feb 2010 - Rastko - new version  to use informative pricing from Amadeus 		-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="Fare_InformativePricingWithoutPNRReply" />
    <xsl:apply-templates select="Fare_CheckRulesReplies"/>
  </xsl:template>
  <xsl:template match="Fare_InformativePricingWithoutPNRReply | Fare_CheckRulesReplies">
    <OTA_AirRulesRS>
      <xsl:attribute name="Version">03</xsl:attribute>
      <xsl:choose>
        <xsl:when test="errorGroup">
          <Errors>
            <Error>
              <xsl:attribute name="Type">Amadeus</xsl:attribute>
              <xsl:attribute name="Code">
                <xsl:value-of select="errorGroup/errorCode/errorDetails/errorCode" />
              </xsl:attribute>
              <xsl:value-of select="errorGroup/errorMessage/freeText" />
            </Error>
          </Errors>
        </xsl:when>
        <xsl:otherwise>
          <Success></Success>
          <xsl:for-each select="Fare_CheckRulesReply">
            <xsl:variable name="pos">
              <xsl:value-of select="position()"/>
            </xsl:variable>
            <xsl:for-each select="flightDetails">
              <FareRuleResponseInfo>
                <FareRuleInfo>
                  <xsl:attribute name="PassengerType">
                    <xsl:value-of select="qualificationFareDetails/fareDetails/qualifier" />
                  </xsl:attribute>
                  <!--<xsl:if test="../DepartureDates/DepartureDate">
                  <DepartureDate>
                    <xsl:text>20</xsl:text>
                    <xsl:value-of select="substring(../DepartureDates/DepartureDate[position()=$pos],5)" />
                    <xsl:text>-</xsl:text>
                    <xsl:value-of select="substring(../DepartureDates/DepartureDate[position()=$pos],3,2)" />
                    <xsl:text>-</xsl:text>
                    <xsl:value-of select="substring(../DepartureDates/DepartureDate[position()=$pos],1,2)" />
                  </DepartureDate>
                </xsl:if>-->
                  <FareReference>
                    <xsl:value-of select="qualificationFareDetails/additionalFareDetails/rateClass" />
                  </FareReference>
                  <RuleInfo />
                  <FilingAirline Code="{transportService/companyIdentification/marketingCompany}" />
                  <DepartureAirport>
                    <xsl:attribute name="LocationCode">
                      <xsl:value-of select="odiGrp/originDestination/origin" />
                    </xsl:attribute>
                  </DepartureAirport>
                  <ArrivalAirport>
                    <xsl:attribute name="LocationCode">
                      <xsl:value-of select="odiGrp/originDestination/destination" />
                    </xsl:attribute>
                  </ArrivalAirport>
                </FareRuleInfo>
                <FareRules>
                  <xsl:variable name="ruleId">
                    <xsl:value-of select="travellerGrp/fareRulesDetails/ruleSectionId"/>
                  </xsl:variable>
                  <xsl:apply-templates select="../tariffInfo[fareRuleInfo/ruleSectionLocalId = $ruleId]" mode="section" />
                </FareRules>
              </FareRuleResponseInfo>
            </xsl:for-each>
          </xsl:for-each>
        </xsl:otherwise>
      </xsl:choose>
    </OTA_AirRulesRS>
  </xsl:template>
  <xsl:template match="tariffInfo" mode="section">
    <SubSection>
      <xsl:attribute name="SubTitle">
        <xsl:value-of select="substring-after(fareRuleText[1]/freeText,'.')"/>
      </xsl:attribute>
      <xsl:attribute name="SubCode">
        <xsl:value-of select="substring-before(fareRuleText[1]/freeText,'.')"/>
      </xsl:attribute>
      <xsl:attribute name="SubSectionNumber">
        <xsl:value-of select="fareRuleInfo/ruleSectionLocalId" />
      </xsl:attribute>
      <xsl:apply-templates select="fareRuleText[position()>1]" mode="text" />
    </SubSection>
  </xsl:template>
  <xsl:template match="fareRuleText" mode="text">
    <xsl:if test="freeText != ''">
      <Paragraph>
        <Text>
          <xsl:value-of select="freeText" />
        </Text>
      </Paragraph>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Fare_DisplayFaresForCityPairReply">
    <OTA_AirRulesRS>
      <xsl:attribute name="Version">1.001</xsl:attribute>
      <Errors>
        <Error>
          <xsl:attribute name="Type">Amadeus</xsl:attribute>
          <xsl:choose>
            <xsl:when test="errorInfo">
              <xsl:attribute name="Code">
                <xsl:value-of select="errorInfo/rejectErrorCode/errorDetails/errorCode" />
              </xsl:attribute>
              <xsl:for-each select="errorInfo/errorFreeText/freeText">
                <xsl:if test="position()>1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="." />
              </xsl:for-each>
            </xsl:when>
            <xsl:when test="flightDetails/itemGrp">
              <xsl:attribute name="Code">0</xsl:attribute>
              <xsl:text>FARE REFERENCE NOT FOUND</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Code">
                <xsl:value-of select="infoText[position()=last()]/freeTextQualification/informationType"/>
              </xsl:attribute>
              <xsl:value-of select="infoText[position()=last()]/freeText"/>
            </xsl:otherwise>
          </xsl:choose>
        </Error>
      </Errors>
    </OTA_AirRulesRS>
  </xsl:template>
</xsl:stylesheet>
