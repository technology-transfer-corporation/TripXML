<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ==================================================================
   Worldspan_PNRRepriceRQ.xsl															
  ================================================================== 
   Date: 23 May 2022 - Kobelev - Ticket Designator fix.
   Date: 24 May 2019 - Kobelev - Added .SR for NEGO fares with markup (BUG 994)
   Date: 11 Sep 2018 - Samokhvalov - Added Fare Qualifier 										
   Date: 08 Apr 2016 - Kobelev - Private Fare identifier											
   Date: 10 Feb 2015 - Rastko - New file											
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_PNRRepriceRQ"/>
  </xsl:template>
  <xsl:template match="OTA_PNRRepriceRQ">
    <xsl:choose>
      <xsl:when test="StoredFare/Markup">
        <Reprice>
          <ScreenEntry>
            <xsl:value-of select="concat('*',UniqueID/@ID)" />
          </ScreenEntry>
          <xsl:for-each select="StoredFare">
            <ScreenEntry>
              <xsl:value-of select="'4P*'"/>
              <xsl:if test="@FareType='Private'">
                <xsl:value-of select="'FSR'"/>
                <xsl:if test="Markup/@Amount or Markup/@Percent">
                  <xsl:value-of select="'.SR'"/>
                </xsl:if>
              </xsl:if>
              <xsl:value-of select="'/-'"/>
              <xsl:choose>
                <xsl:when test="Markup/@Amount">
                  <xsl:value-of select="concat('$P',Markup/@Amount)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat('P',Markup/@Percent)"/>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:value-of select="'/#TR'"/>
            </ScreenEntry>
          </xsl:for-each>
          <ScreenEntry>
            <xsl:value-of select="'4PQCTRALL'" />
          </ScreenEntry>
          <ScreenEntry>6.TRIPXML</ScreenEntry>
          <ScreenEntry>ER</ScreenEntry>
        </Reprice>
      </xsl:when>
      <xsl:otherwise>
        <OTA_AirPriceRQ xmlns="http://www.opentravel.org/OTA/2003/05" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Version="1">
          <xsl:attribute name="Type">
            <xsl:value-of select="'AutoPrice'"/>
          </xsl:attribute>
          <POS>
            <Source ISOCountry="US"/>
          </POS>
          <TravelerInfoSummary>
            <AirTravelerAvail>
              <xsl:for-each select="StoredFare">
                <xsl:call-template name="paxType">
                  <xsl:with-param name="paxNum">0</xsl:with-param>
                </xsl:call-template>
              </xsl:for-each>
            </AirTravelerAvail>
            <xsl:if test="count(StoredFare/FareSegments/AirSegments/@TicketDesignator) > 0 or StoredFare/Discount or StoredFare/@FareType='Private'">
              <PriceRequestInformation>
                <xsl:if test="StoredFare/@FareType='Private'">
                  <xsl:attribute name="FareQualifier">
                    <xsl:value-of select="'SR'"/>
                  </xsl:attribute>
                  <xsl:attribute name="NegotiatedFaresOnly">
                    <xsl:value-of select="'true'"/>
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="StoredFare/@FareQualifier and StoredFare/@FareType!='Private'">
                  <xsl:attribute name="FareQualifier">
                    <xsl:value-of select="StoredFare/@FareQualifier"/>
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="count(StoredFare/FareSegments/AirSegments/@TicketDesignator) > 0 or StoredFare/Discount or StoredFare/Markup">
                  <DiscountPricing>
                    <xsl:if test="StoredFare/FareSegments/AirSegments/@TicketDesignator!=''">
                      <xsl:attribute name="TicketDesignatorCode">
                        <xsl:value-of select="StoredFare/FareSegments/AirSegments/@TicketDesignator[1]"/>
                      </xsl:attribute>
                    </xsl:if>
                    <xsl:if test="StoredFare/Discount">
                      <xsl:attribute name="Discount">
                        <xsl:value-of select="StoredFare/Discount/@Amount"/>
                        <xsl:value-of select="StoredFare/Discount/@Percent"/>
                      </xsl:attribute>
                      <xsl:attribute name="Type">
                        <xsl:choose>
                          <xsl:when test="StoredFare/Discount/@Amount!=''">Amount</xsl:when>
                          <xsl:otherwise>Percentage</xsl:otherwise>
                        </xsl:choose>
                      </xsl:attribute>
                      <xsl:attribute name="Usage">Plus</xsl:attribute>
                    </xsl:if>
                    <xsl:if test="StoredFare/Markup">
                      <xsl:attribute name="Discount">
                        <xsl:value-of select="StoredFare/Markup/@Amount"/>
                        <xsl:value-of select="StoredFare/Markup/@Percent"/>
                      </xsl:attribute>
                      <xsl:attribute name="Type">
                        <xsl:choose>
                          <xsl:when test="StoredFare/Markup/@Amount!=''">Amount</xsl:when>
                          <xsl:otherwise>Percentage</xsl:otherwise>
                        </xsl:choose>
                      </xsl:attribute>
                      <xsl:attribute name="Usage">Plus</xsl:attribute>
                    </xsl:if>
                  </DiscountPricing>
                </xsl:if>
              </PriceRequestInformation>
            </xsl:if>
            <!--PricingPref Type="1" Qualifier="5" ExcludeInd="false" />
						
						<PricingPref Type="20" /-->
            <xsl:if test="@StoreFare='true'">
              <PricingPref Type="12" />
            </xsl:if>
            <xsl:if test="StoredFare/Markup">
              <PricingPref Type="19" />
            </xsl:if>
            <xsl:if test="StoredFare/@FareType='Private'">
              <PricingPref Type="21" />
            </xsl:if>
          </TravelerInfoSummary>
          <BookingReferenceID Type="14" ID="{UniqueID/@ID}"/>
          <xsl:for-each select="FlightReference">
            <FlightReference FlightRefNumber="{@RPH}"/>
          </xsl:for-each>
        </OTA_AirPriceRQ>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="paxType">
    <xsl:param name="paxNum"/>
    <xsl:if test="PassengerType/@Quantity > $paxNum">
      <PassengerTypeQuantity Code="{PassengerType/@Code}" Quantity="1"/>
      <xsl:call-template name="paxType">
        <xsl:with-param name="paxNum">
          <xsl:value-of select="$paxNum + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
