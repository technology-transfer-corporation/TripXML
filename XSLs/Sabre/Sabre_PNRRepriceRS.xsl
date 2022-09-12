<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:stl="http://services.sabre.com/STL/v01">
  <!--
   ================================================================== 
   Sabre_PNRRepriceRS.xsl 															
   ================================================================== 
   Date: 13 Jul 2021 - Babin - Total Fare Amount Variable fix in sum XPath.
   Date: 08 Jul 2021 - Kobelev - Branded Fare Combined fix of Totals.
   Date: 08 Jul 2021 - Kobelev - Branded Fare Name fix on first Sequance.
   Date: 07 Jul 2021 - Kobelev - Branded Fare information fix.
   Date: 19 May 2021 - Kobelev - Branded Fare information and Segment association.
   Date: 18 May 2021 - Kobelev - Branded Fare information added.
   Date: 29 Oct 2018 - Samokhvalov - PriceQuote - filtered history records
   Date: 03 Jan 2018 - Samokhvalov - PassangeFare corrected -> amount per 1 pax 
   Date: 01 Feb 2016 - Rastko - fixd issue where pax number in party was not used in total fare calc 
   Date: 12 Apr 2014 - Rastko - modify calculation of total amounts for new fares		
   Date: 04 Apr 2014 - Rastko - placed a filter on stored fares					
   Date: 21 Mar 2014 - Rastko - added support for air price error					
   Date: 13 Feb 2014 - Rastko - mapped private fare estimation for air price response		
   Date: 10 Feb 2014 - Rastko - new file												
   ==================================================================
   -->
  <xsl:output omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <OTA_PNRRepriceRS>
      <xsl:attribute name="Version">1.0</xsl:attribute>
      <xsl:if test="TravelItineraryReadRS/EchoToken!=''">
        <xsl:attribute name="EchoToken">
          <xsl:value-of select="TravelItineraryReadRS/EchoToken"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="TravelItineraryReadRS"/>
      <xsl:if test="TravelItineraryReadRS/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="TravelItineraryReadRS/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_PNRRepriceRS>
  </xsl:template>
  <!--************************************************************************************************************-->
  <xsl:template match="TravelItineraryReadRS">
    <xsl:choose>
      <xsl:when test="OTA_AirPriceRS/ApplicationResults/Error">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:attribute name="Code">
              <xsl:choose>
                <xsl:when test="OTA_AirPriceRS/ApplicationResults/Error/SystemSpecificResults/Message/@code!= ''">
                  <xsl:value-of select="OTA_AirPriceRS/ApplicationResults/Error/SystemSpecificResults/Message/@code"/>
                </xsl:when>
                <xsl:otherwise>E</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="OTA_AirPriceRS/ApplicationResults/Error/SystemSpecificResults/Message"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="OTA_AirPriceRS/Error != ''">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:attribute name="Code">E</xsl:attribute>
            <xsl:value-of select="OTA_AirPriceRS/Error"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="Errors/Error != ''">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:attribute name="Code">
              <xsl:choose>
                <xsl:when test="Errors/Error/@ErrorCode!= ''">
                  <xsl:value-of select="Errors/Error/@ErrorCode"/>
                </xsl:when>
                <xsl:otherwise>E</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="Errors/Error"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="stl:ApplicationResults/@status='NotProcessed'">
        <Errors>
          <Error>
            <xsl:attribute name="Type">
              <xsl:value-of select="stl:ApplicationResults/stl:Error/@type"/>
            </xsl:attribute>
            <xsl:value-of select="stl:ApplicationResults/stl:Error/stl:SystemSpecificResults"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <Success/>
        <xsl:if test="Warning!=''">
          <Warnings>
            <xsl:apply-templates select="Warning"/>
          </Warnings>
        </xsl:if>
        <PricedItineraries>
          <xsl:apply-templates select="TravelItinerary/ItineraryInfo/ItineraryPricing" mode="Fare">
            <xsl:with-param name="fare" select="'stored'"/>
            <xsl:with-param name="sn" select="'1'"/>
          </xsl:apply-templates>
          <xsl:choose>
            <xsl:when test="count(OTA_AirPriceRS) > 1">
              <xsl:call-template name="multyFare">
                <xsl:with-param name="ota_airprice" select="OTA_AirPriceRS"/>
                <xsl:with-param name="fare" select="'new'"/>
                <xsl:with-param name="sn" select="'2'"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="OTA_AirPriceRS" mode="Fare">
                <xsl:with-param name="fare" select="'new'"/>
                <xsl:with-param name="sn" select="'2'"/>
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>

        </PricedItineraries>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Warning">
    <Warning>
      <xsl:value-of select="normalize-space(translate(.,'Â',''))"/>
    </Warning>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			PNR Retrieve Errors                                           	                 -->
  <!--************************************************************************************-->
  <xsl:template match="Err">
    <Error>
      <xsl:attribute name="Type">General</xsl:attribute>
      <xsl:value-of select="Text"/>
    </Error>
  </xsl:template>

  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			           -->
  <!--***********************************************************************************-->
  <xsl:template match="ItineraryPricing | OTA_AirPriceRS" mode="Fare">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:variable name="dect">
        <xsl:choose>
          <xsl:when test="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFae/EquivFare/@Amount!=''">
            <xsl:value-of select="string-length(substring-after(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="string-length(substring-after(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <AirItineraryPricingInfo>
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="contains(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JCB') or contains(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNN') or contains(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNF')">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JCB' or PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNN' or PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNF'">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Published'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <ItinTotalFare>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
                  <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/BaseFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="fare" select="'new'"/>
                  </xsl:apply-templates>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="fare" select="'stored'"/>
                  </xsl:apply-templates>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:choose>
                <xsl:when test="$fare='new'">
                  <xsl:choose>
                    <xsl:when test="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
                      <xsl:attribute name="Amount">
                        <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/Taxes">
                          <xsl:with-param name="totalbf">0</xsl:with-param>
                          <xsl:with-param name="pos">1</xsl:with-param>
                          <xsl:with-param name="bfcount">
                            <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
                          </xsl:with-param>
                          <xsl:with-param name="fare" select="'new'"/>
                        </xsl:apply-templates>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:attribute name="Amount">
                        <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes">
                          <xsl:with-param name="totalbf">0</xsl:with-param>
                          <xsl:with-param name="pos">1</xsl:with-param>
                          <xsl:with-param name="bfcount">
                            <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
                          </xsl:with-param>
                          <xsl:with-param name="fare" select="'stored'"/>
                        </xsl:apply-templates>
                      </xsl:attribute>
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare/@CurrencyCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="$dect"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Amount">
                    <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
                      <xsl:with-param name="totalbf">0</xsl:with-param>
                      <xsl:with-param name="pos">1</xsl:with-param>
                      <xsl:with-param name="bfcount">
                        <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
                      </xsl:with-param>
                    </xsl:apply-templates>
                  </xsl:attribute>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="$dect"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </Tax>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
                  <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="fare" select="'new'"/>
                  </xsl:apply-templates>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="fare" select="'stored'"/>
                  </xsl:apply-templates>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo">
            <xsl:with-param name="fare" select="$fare"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template name="multyFare">
    <xsl:param name="ota_airprice"/>
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:variable name="dect">
        <xsl:choose>
          <xsl:when test="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFae/EquivFare/@Amount!=''">
            <xsl:value-of select="string-length(substring-after($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="string-length(substring-after($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <AirItineraryPricingInfo>
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="contains($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JCB') or contains($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNN') or contains($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNF')">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JCB' or $ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNN' or $ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNF'">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Published'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <ItinTotalFare>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">

                  <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/BaseFare/@Amount)">
                  </xsl:variable>
                  <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>

                </xsl:when>
                <xsl:otherwise>
                  <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount)">                    
                  </xsl:variable>                  
                  <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:choose>
                <xsl:when test="$fare='new'">
                  <xsl:choose>
                    <xsl:when test="count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
                      <xsl:attribute name="Amount">
                      
                        <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/Taxes/@TotalAmount)">
                        </xsl:variable>
                        <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:attribute name="Amount">
                        <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/@TotalAmount)">
                        </xsl:variable>
                        <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                      </xsl:attribute>
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare/@CurrencyCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="$dect"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Amount">
                                        
                    <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount)">
                    </xsl:variable>
                    <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                    
                  </xsl:attribute>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="$dect"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </Tax>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
                  
                  <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare/@Amount)">
                    </xsl:variable>
                    <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                  
                </xsl:when>
                <xsl:otherwise>
                  
                  <xsl:variable name="bf" select="sum($ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@TotalAmount)">
                  </xsl:variable>
                  <xsl:value-of select="translate(format-number($bf, '#0.00'), '.','')"/>
                  
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="$ota_airprice/PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo">
            <xsl:with-param name="fare" select="$fare"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="BaseFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="fare"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="$fare = 'new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]">
        <xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/BaseFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="EquivFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Tax">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount!=''">
        <xsl:apply-templates select="../../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@Amount!='']">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Taxes">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="fare"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@TotalAmount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/@TotalAmount!=''">
        <xsl:apply-templates select="../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes[@TotalAmount!='']">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="$fare='new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/Taxes/@TotalAmount!=''">
        <xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/Taxes[@TotalAmount!='']">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="TotalFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="fare"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[not(contains(MiscInformation/SignatureLine/@Status,'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="$fare='new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]">
        <xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/TotalFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="fare">
            <xsl:value-of select="$fare"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="calcbf">
    <xsl:param name="bf"/>
    <xsl:param name="totalbf"/>
    <xsl:if test="$bf != ''">
      <xsl:variable name="temp">
        <xsl:value-of select="substring-before($bf,'/')"/>
      </xsl:variable>
      <xsl:call-template name="calcbf">
        <xsl:with-param name="bf">
          <xsl:value-of select="substring-after($bf,'/')"/>
        </xsl:with-param>
        <xsl:with-param name="totalbf">
          <xsl:value-of select="$totalbf + $temp"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="$totalbf"/>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Individual Tax element 	 	      			                -->
  <!--***********************************************************************************-->
  <xsl:template match="Tax" mode="TotalFare">
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="'TotalTax'"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(string(@Amount),'.','')"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../../ItinTotalFare/TotalFare/@CurrencyCode"/>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="AirItineraryPricingInfo">
    <xsl:param name="fare"/>

    <xsl:variable name="dect1">
      <xsl:choose>
        <xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
          <xsl:value-of select="string-length(substring-after(ItinTotalFare/EquivFare/@Amount,'.'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="string-length(substring-after(ItinTotalFare/BaseFare/@Amount,'.'))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="ptc" select="PassengerTypeQuantity/@Code" />

    <PTC_FareBreakdown>
      <xsl:attribute name="RPH">
        <xsl:choose>
          <xsl:when test="$fare='new'">
            <xsl:value-of select="position()"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="../../@RPH"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:attribute name="PricingSource">
        <xsl:choose>
          <xsl:when test="contains(../@InputMessage,'JCB') or contains(../@InputMessage,'JNN') or contains(../@InputMessage,'JNF')">
            <xsl:value-of select="'Private'"/>
          </xsl:when>
          <xsl:when test="PassengerTypeQuantity/@Code='JCB' or PassengerTypeQuantity/@Code='JNN' or PassengerTypeQuantity/@Code='JNF'">
            <xsl:value-of select="'Private'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'Published'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:value-of select="../../@RPH"/>
      </xsl:attribute>

      <xsl:variable name="ptcSegs" select="BaggageProvisions[last()]/Associations/PNR_Segment"/>

      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:choose>
          <xsl:when test="PTC_FareBreakdown/BrandedFareInformation">
            <xsl:for-each select="BaggageProvisions[last()]/Associations/PNR_Segment">
              <xsl:variable name="rph">
                <xsl:value-of select="@RPH"/>
              </xsl:variable>
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="$rph"/>
            </xsl:for-each>
          </xsl:when>
          <xsl:otherwise>
            <xsl:for-each select="PTC_FareBreakdown/FlightSegment">
              <xsl:variable name="rph">
                <xsl:value-of select="@SegmentNumber"/>
              </xsl:variable>
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="$rph"/>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:attribute>

      <PassengerTypeQuantity>
        <xsl:choose>
          <xsl:when test="PassengerTypeQuantity/@Code='C09'">
            <xsl:attribute name="Code">CHD</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Code">
              <xsl:value-of select="PassengerTypeQuantity/@Code"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="format-number(PassengerTypeQuantity/@Quantity,'#0')"/>
        </xsl:attribute>
      </PassengerTypeQuantity>

      <xsl:variable name="nip">
        <xsl:value-of select="PassengerTypeQuantity/@Quantity"/>
      </xsl:variable>

      <a>
        <xsl:value-of select="$nip"/>
      </a>

      <xsl:choose>
        <xsl:when test="$fare='new'">
          <FareBasisCodes>
            <xsl:for-each select="PTC_FareBreakdown/FareBasis">
              <FareBasisCode>
                <xsl:value-of select="@Code"/>
              </FareBasisCode>
            </xsl:for-each>
          </FareBasisCodes>
        </xsl:when>
        <xsl:otherwise>
          <FareBasisCodes>
            <xsl:for-each select="PTC_FareBreakdown/FlightSegment/FareBasis">
              <FareBasisCode>
                <xsl:value-of select="@Code"/>
              </FareBasisCode>
            </xsl:for-each>
          </FareBasisCodes>
          <!--xsl:if test="PTC_FareBreakdown/FareBasis/@Code">
						<FareBasisCodes>
							<xsl:variable name="fbc">
								<xsl:value-of select="PTC_FareBreakdown/FareBasis/@Code"/>
							</xsl:variable>
							<xsl:call-template name="farebasis">
								<xsl:with-param name="fbc">
									<xsl:value-of select="$fbc"/>
								</xsl:with-param>
							</xsl:call-template>
						</FareBasisCodes>
					</xsl:if-->
        </xsl:otherwise>
      </xsl:choose>

      <xsl:if test="contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '$BR') 
        or contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '*BR')
        or contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], 'BR')">
        <BrandedFares>
          <xsl:choose>
            <xsl:when test="contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '$S') or contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '*BR')">
              <xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='']" mode="ffSegmental">
                <xsl:with-param name="brID" select="substring-after(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '$')"/>
              </xsl:apply-templates>
            </xsl:when>
            <xsl:when test="contains(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], 'WPBR')">
              <xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='' and @Status]" mode="ff">
                <xsl:with-param name="brID" select="substring-after(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], 'BR')"/>
              </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:variable name="bf">
                <xsl:call-template name="string-trim">
                  <xsl:with-param name="string" select="substring-after(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER'], '$BR')" />
                </xsl:call-template>
              </xsl:variable>
              <xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='' and @Status]" mode="ff">
                <xsl:with-param name="brID" select="$bf"/>
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>
        </BrandedFares>
      </xsl:if>
      <xsl:if test="PTC_FareBreakdown/BrandedFareInformation">
        <BrandedFares>
          <xsl:for-each select="PTC_FareBreakdown">
            <xsl:variable name="fbc">
              <xsl:value-of select="FareBasis/@Code"/>
            </xsl:variable>
            <xsl:variable name="brand">
              <xsl:value-of select="../PTC_FareBreakdown[FareBasis/@Code=$fbc]/BrandedFareInformation/BrandCode"/>
            </xsl:variable>
            <xsl:variable name="td">
              <xsl:value-of select="FareBasis/@TicketDesignator"/>
            </xsl:variable>
            <xsl:variable name="index">
              <xsl:value-of select="position()"/>
            </xsl:variable>
            <xsl:variable name="seg">
              <xsl:value-of select="$ptcSegs[@RPH=$index]"/>
            </xsl:variable>

            <FareFamily>
              <xsl:choose>
                <xsl:when test="$seg">
                  <xsl:attribute name="RPH">
                    <xsl:value-of select="$seg"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="RPH">
                    <xsl:value-of select="position()"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>

              <xsl:value-of select="$brand"/>
            </FareFamily>

          </xsl:for-each>
        </BrandedFares>
      </xsl:if>

      <PassengerFare>
        <BaseFare>
          <xsl:choose>
            <xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect1"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(ItinTotalFare/BaseFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect1"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </BaseFare>
        <xsl:if test="ItinTotalFare/BaseFare/EquivFare">
          <EquivFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect1"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
            </xsl:attribute>
          </EquivFare>
        </xsl:if>
        <Taxes>
          <xsl:choose>
            <xsl:when test="$fare='new'">
              <Tax TaxCode="TotalTax" Amount="{translate(ItinTotalFare/Taxes/@TotalAmount,'.','')}" DecimalPlaces="{string-length(substring-after(ItinTotalFare/Taxes/@TotalAmount,'.'))}" CurrencyCode="{ItinTotalFare/BaseFare/@CurrencyCode}" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="ItinTotalFare/Taxes/Tax" mode="PTC">
                <xsl:with-param name="nip" select="$nip"/>
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(ItinTotalFare/TotalFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect1"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="PTC_FareBreakdown/FareCalculation/Text!=''">
          <FareCalculation>
            <xsl:value-of select="PTC_FareBreakdown/FareCalculation/Text"/>
          </FareCalculation>
        </xsl:if>
        <xsl:if test="FareCalculation/Text!=''">
          <FareCalculation>
            <xsl:value-of select="FareCalculation/Text"/>
          </FareCalculation>
        </xsl:if>
        <xsl:if test="../../PricedItinerary/@ValidatingCarrier != ''">
          <ValidatingAirlineCode>
            <xsl:value-of select="../../PricedItinerary/@ValidatingCarrier"/>
          </ValidatingAirlineCode>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="../../PricedItinerary/@ValidatingCarrier"/>
            </xsl:attribute>
          </Vendor>
        </xsl:if>
        <xsl:if test="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code != ''">
          <ValidatingAirlineCode>
            <xsl:value-of select="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code"/>
          </ValidatingAirlineCode>
          <!--Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code"/>
						</xsl:attribute>
					</Vendor-->
        </xsl:if>
        <xsl:if test="../../PricedItinerary/@InputMessage != ''">
          <SupplementalInfo>
            <xsl:value-of select="../../PricedItinerary/@InputMessage"/>
          </SupplementalInfo>
        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="AirFareInfo">
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Code"/>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:if test="PTC_FareBreakdown/FareBasisCode">
        <FareBasisCodes>
          <xsl:variable name="fbc">
            <xsl:value-of select="PTC_FareBreakdown/FareBasisCode"/>
          </xsl:variable>
          <xsl:call-template name="farebasis">
            <xsl:with-param name="fbc">
              <xsl:value-of select="$fbc"/>
            </xsl:with-param>
          </xsl:call-template>
        </FareBasisCodes>
      </xsl:if>
      <PassengerFare>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:variable name="bf">
              <xsl:choose>
                <xsl:when test="PTC_FareBreakdown/PassengerFare/EquivFare/@Amount!=''">
                  <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/BaseFare/@Amount),'.','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="nip">
              <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
            </xsl:variable>
            <xsl:value-of select="$bf * $nip"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </BaseFare>
        <!--xsl:if test="PTC_FareBreakdown/PassengerFare/EquivFare">
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFare/@DecimalPlaces" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivFare/@CurrencyCode" />
						</xsl:attribute>
					</EquivFare>
				</xsl:if-->
        <Taxes>
          <xsl:apply-templates select="PTC_FareBreakdown/PassengerFare//Taxes/Tax" mode="PTC"/>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:variable name="bf">
              <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/TotalFare/@Amount),'.','')"/>
            </xsl:variable>
            <xsl:variable name="nip">
              <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
            </xsl:variable>
            <xsl:value-of select="$bf * $nip"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
    </PTC_FareBreakdown>
  </xsl:template>
  <xsl:template name="farebasis">
    <xsl:param name="fbc"/>
    <xsl:if test="$fbc != ''">
      <FareBasisCode>
        <xsl:choose>
          <xsl:when test="contains($fbc,'/')">
            <xsl:value-of select="substring-before($fbc,'/')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$fbc"/>
          </xsl:otherwise>
        </xsl:choose>
      </FareBasisCode>
      <xsl:call-template name="farebasis">
        <xsl:with-param name="fbc">
          <xsl:value-of select="substring-after($fbc,'/')"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="FareBasisCode">
    <FareBasisCode>
      <xsl:value-of select="."/>
    </FareBasisCode>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Individual Tax element 	 	      			                -->
  <!--***********************************************************************************-->
  <xsl:template match="Tax" mode="PTC">
    <xsl:param name="nip"/>
    <b>
      <xsl:value-of select="$nip"/>
    </b>

    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="'TotalTax'"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:variable name="bf">
          <xsl:value-of select="translate(string(@Amount),'.','')"/>
        </xsl:variable>
        <!--xsl:variable name="nip">
					<xsl:value-of select="../../..//PassengerTypeQuantity/@Quantity"/>
				</xsl:variable-->
        <xsl:variable name="tottax">
          <xsl:value-of select="$bf * $nip"/>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$tottax='NaN'">0</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$tottax"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../TotalFare/@CurrencyCode"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>

  <xsl:template match="FlightSegment" mode="ffSegmental">
    <!-- 
    S1*BRECONOMY?S2*BRECOFLEX 
    S1/2*BREC?
    S1-2*BREC?
    -->
    <xsl:param name="brID"/>
    <xsl:variable name="seg">
      <xsl:value-of select="concat('S', @SegmentNumber,'*')"/>
    </xsl:variable>
    <xsl:variable name="ff">
      <xsl:choose>
        <xsl:when test="string-length(substring-before(substring-after($brID, concat($seg,'BR')), '$')) > 0">
          <xsl:call-template name="string-trim">
            <xsl:with-param name="string" select="substring-before(substring-after($brID, concat($seg,'BR')), '$')" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="string-trim">
            <xsl:with-param name="string" select="substring-after($brID, concat($seg,'BR'))" />
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="contains($brID, $seg)">
      <FareFamily>
        <xsl:attribute name="RPH">
          <xsl:value-of select="@SegmentNumber"/>
        </xsl:attribute>
        <xsl:value-of select="$ff"/>
      </FareFamily>
    </xsl:if>
  </xsl:template>

  <xsl:template match="FlightSegment" mode="ff">
    <xsl:param name="brID"/>
    <FareFamily>
      <xsl:attribute name="RPH">
        <xsl:value-of select="@SegmentNumber"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="contains($brID, '$')">
          <xsl:call-template name="string-trim">
            <xsl:with-param name="string" select="translate(substring-before($brID, '$'), '$RQ', '')" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="string-trim">
            <xsl:with-param name="string" select="$brID" />
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </FareFamily>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- Replace a string 	   	                                          -->
  <!-- ************************************************************** -->
  <xsl:template name="string-replace-all">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
      <xsl:when test="contains($text, $replace)">
        <xsl:value-of select="substring-before($text,$replace)" />
        <xsl:value-of select="$by" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text"
          select="substring-after($text,$replace)" />
          <xsl:with-param name="replace" select="$replace" />
          <xsl:with-param name="by" select="$by" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="rphFormat">
    <xsl:param name="rn" />
    <xsl:variable name="whole" select="format-number(number(substring-before($rn, '.')),'00')"/>
    <xsl:variable name="tenth" select="format-number(number(substring-after($rn, '.')),'00')"/>

    <xsl:value-of select="concat($whole,'.',$tenth)" />
  </xsl:template>

  <xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

  <!-- Strips leading whitespace characters from 'string' -->
  <xsl:template name="string-ltrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:if test="string-length($string) &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, 1, 1))">
          <xsl:call-template name="string-ltrim">
            <xsl:with-param name="string" select="substring($string, 2)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips trailing whitespace characters from 'string' -->
  <xsl:template name="string-rtrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:variable name="length" select="string-length($string)" />

    <xsl:if test="$length &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, $length, 1))">
          <xsl:call-template name="string-rtrim">
            <xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips leading and trailing whitespace characters from 'string' -->
  <xsl:template name="string-trim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />
    <xsl:call-template name="string-rtrim">
      <xsl:with-param name="string">
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="$string" />
          <xsl:with-param name="trim"   select="$trim" />
        </xsl:call-template>
      </xsl:with-param>
      <xsl:with-param name="trim"   select="$trim" />
    </xsl:call-template>
  </xsl:template>

  <!--############################################################-->
  <!--## Template to tokenize strings                           ##-->
  <!--############################################################-->
  <xsl:template name="tokenizeString">
    <!--passed template parameter -->
    <xsl:param name="list"/>
    <xsl:param name="delimiter"/>
    <xsl:choose>
      <xsl:when test="contains($list, $delimiter)">
        <elem>
          <!-- get everything in front of the first delimiter -->
          <xsl:value-of select="substring-before($list,$delimiter)"/>
        </elem>
        <xsl:call-template name="tokenizeString">
          <!-- store anything left in another variable -->
          <xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
          <xsl:with-param name="delimiter" select="$delimiter"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$list = ''">
            <xsl:text/>
          </xsl:when>
          <xsl:otherwise>
            <elem>
              <xsl:value-of select="$list"/>
            </elem>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
