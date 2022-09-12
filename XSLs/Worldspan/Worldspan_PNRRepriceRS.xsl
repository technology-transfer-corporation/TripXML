<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- 
  ================================================================== 
   Worldspan_PNRRepriceRS.xsl 												
  ================================================================== 
   Date: 05 Aug 2021 - Kobelev - BUG 1464 correct display of Total Fare per PTC.
   Date: 27 Jul 2021 - Kobelev - Price Qoutes for each PTC will have in RPH reference TR it belong.
   Date: 06 Jun 2019 - Kobelev - Adjutment of price compare display. 
   Date: 08 Jan 2019 - Kobelev - PricingSource Identifier Corrected
   Date: 08 Feb 2018 - Samokhvalov - Added error object  						
   Date: 10 Feb 2015 - Rastko														
  ================================================================== 
  -->
  <xsl:output omit-xml-declaration="yes"/>
  <xsl:include href="worldspan_faultstring.xsl" />
  <xsl:key name="trPTC" match="//DPW8/PNR_4_INF/Line/@TR" use="." />

  <xsl:template match="/">
    <OTA_PNRRepriceRS>
      <xsl:attribute name="Version">1.0</xsl:attribute>
      <xsl:apply-templates select="OTA_AirPriceRS"/>
      <xsl:apply-templates select="DPW8"/>
      <xsl:call-template name="processFaultstring"/>
    </OTA_PNRRepriceRS>
  </xsl:template>

  <!--************************************************************************************************************-->
  <xsl:template match="OTA_AirPriceRS | DPW8">
    <xsl:choose>
      <xsl:when test="Errors/Error != ''">
        <Errors>
          <Error>
            <xsl:attribute name="Type">
              <xsl:value-of select="Errors/Error/@Type"/>
            </xsl:attribute>
            <xsl:attribute name="Code">
              <xsl:value-of select="Errors/Error/@Code"/>
            </xsl:attribute>
            <xsl:value-of select="Errors/Error"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="not(PricedItineraries) and not(PRC_INF)">
        <Errors>
          <xsl:for-each select="Warnings/Warning">
            <Error>
              <xsl:value-of select="."/>
            </Error>
          </xsl:for-each>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <Success/>
        <xsl:if test="Warnings!=''">
          <Warnings>
            <xsl:apply-templates select="Warnings/Warning"/>
          </Warnings>
        </xsl:if>
        <PricedItineraries>
          <xsl:choose>
            <xsl:when test="PricedItineraries/PricedItinerary">
              <xsl:apply-templates select="PricedItineraries/PricedItinerary/AirItineraryPricingInfo" mode="Fare">
                <xsl:with-param name="fare" select="'stored'"/>
                <xsl:with-param name="sn" select="'1'"/>
              </xsl:apply-templates>

              <xsl:apply-templates select="PricingOverview" mode="Fare">
                <xsl:with-param name="fare" select="'new'"/>
                <xsl:with-param name="sn" select="'2'"/>
                <xsl:with-param name="fareType" select="PricedItineraries/PricedItinerary/AirItineraryPricingInfo/@PricingSource"/>
              </xsl:apply-templates>
            </xsl:when>

            <xsl:otherwise>
              <xsl:variable name="pq" select="PRC_INF/TIC_REC_PRC_QUO"></xsl:variable>
              <xsl:variable name="tr" select="PNR_4_INF/Line[@ID]"></xsl:variable>

              <xsl:variable name="lTR">
                <xsl:for-each select="$tr/@TR[generate-id() = generate-id(key('trPTC',.)[1])]">
                  <xsl:if test=". !=''">
                    <xsl:value-of select="concat(., ',')"/>
                  </xsl:if>
                </xsl:for-each>
              </xsl:variable>

              <xsl:variable name="elems">
                <xsl:call-template name="tokenizeString">
                  <xsl:with-param name="list" select="$lTR"/>
                  <xsl:with-param name="delimiter" select="','"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:apply-templates select="PRC_INF/PRC_QUO">
                <xsl:with-param name="sn" select="'1'"/>
              </xsl:apply-templates>

              <xsl:choose>
                <xsl:when test="count(msxsl:node-set($elems)/elem) > 1">
                  <xsl:call-template name="Joined">
                    <xsl:with-param name="PQs" select="PRC_INF/TIC_REC_PRC_QUO[contains(concat(',', $lTR), concat(',', TIC_REC_NUM, ','))]" />
                    <xsl:with-param name="sn" select="'2'"/>
                  </xsl:call-template>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="PRC_INF/TIC_REC_PRC_QUO[last()]" >
                    <xsl:with-param name="sn" select="'2'"/>
                  </xsl:apply-templates>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:otherwise>
          </xsl:choose>
        </PricedItineraries>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="ConversationID">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:when>
      <xsl:otherwise>
        <ConversationID>NONE</ConversationID>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Warning">
    <Warning>
      <xsl:value-of select="normalize-space(translate(.,'Â',''))"/>
    </Warning>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			PNR Retrieve Errors                                           	              -->
  <!--************************************************************************************-->
  <xsl:template match="Err">
    <Error>
      <xsl:attribute name="Type">General</xsl:attribute>
      <xsl:value-of select="Text"/>
    </Error>
  </xsl:template>

  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			                                -->
  <!--***********************************************************************************-->
  <xsl:template match="AirItineraryPricingInfo" mode="Fare">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:variable name="cur">
        <xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
      </xsl:variable>
      <xsl:variable name="dect">
        <xsl:value-of select="string-length(substring-after(ItinTotalFare/BaseFare/@Amount,'.'))"/>
      </xsl:variable>

      <AirItineraryPricingInfo>

        <xsl:variable name="base">
          <xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')"/>
        </xsl:variable>
        <xsl:variable name="taxes">
          <xsl:value-of select="translate(ItinTotalFare/Taxes/@Amount,'.','')"/>
        </xsl:variable>
        <xsl:attribute name="PricingSource" />
        <!-- See explinations at the top of the page 6/6/19
         Worldpan in this webservice only returns current price. So we are putting a placing holder in order to replace it with stored price from PNR Read. 
         
        <xsl:attribute name="PricingSource">
          <xsl:value-of select="@PricingSource"/>
        </xsl:attribute> 
        <ItinTotalFare>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$base"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$cur"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <xsl:if test="ItinTotalFare/EquivFare">
            <xsl:variable name="dect2">
              <xsl:value-of select="string-length(substring-after(ItinTotalFare/EquivFare/@Amount,'.'))"/>
            </xsl:variable>
            <EquivFare>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(ItinTotalFare/EquivFare/@Amount,'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect2"/>
              </xsl:attribute>
            </EquivFare>
          </xsl:if>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:attribute name="Amount">
                <xsl:value-of select="$taxes"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$cur"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </Tax>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$base + $taxes"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$cur"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="PTC_FareBreakdowns/PTC_FareBreakdown">
            <xsl:with-param name="cur" select="$cur"/>
            <xsl:with-param name="dect" select="$dect"/>
            <xsl:with-param name="sn" select="'1'"/>
            <xsl:with-param name="fareType" select="@PricingSource"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      -->
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="PricingOverview" mode="Fare">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <xsl:param name ="fareType"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:variable name="cur">
        <xsl:value-of select="FareInfo/BaseFare/@CurrencyCode"/>
      </xsl:variable>
      <xsl:variable name="dect">
        <xsl:value-of select="string-length(substring-after(FareInfo/BaseFare/@Amount,'.'))"/>
      </xsl:variable>

      <AirItineraryPricingInfo>
        <xsl:variable name="base">
          <xsl:value-of select="translate(FareInfo/BaseFare/@Amount,'.','')"/>
        </xsl:variable>
        <xsl:variable name="total">
          <xsl:value-of select="translate(FareInfo/TotalFare/@Amount,'.','')"/>
        </xsl:variable>
        <xsl:attribute name="PricingSource">
          <xsl:value-of select="$fareType"/>
        </xsl:attribute>
        <ItinTotalFare>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$base"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="FareInfo/BaseFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:attribute name="Amount">
                <xsl:value-of select="$total - $base"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$cur"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </Tax>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$total"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="FareInfo/TotalFare/@CurrencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="PTC_FareBreakdowns/PTC_FareBreakdown">
            <xsl:with-param name="cur" select="$cur"/>
            <xsl:with-param name="dect" select="$dect"/>
            <xsl:with-param name="sn" select="'2'"/>
            <xsl:with-param name="fareType" select="$fareType"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="PTC_FareBreakdown">
    <xsl:param name="cur"/>
    <xsl:param name="dect"/>
    <xsl:param name="sn"/>
    <xsl:param name="fareType"/>
    <xsl:variable name="pos" select="position()"/>
    <PTC_FareBreakdown>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="PricingSource">
        <xsl:value-of select="$fareType"/>
        <!--
        <xsl:choose>
          <xsl:when test="Private">
            <xsl:value-of select="'Private'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'Published'"/>
          </xsl:otherwise>
        </xsl:choose>
        -->
      </xsl:attribute>
      <PassengerTypeQuantity>
        <xsl:choose>
          <xsl:when test="PassengerTypeQuantity/@Code='CNN'">
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
      <FareBasisCodes>
        <xsl:choose>
          <xsl:when test="$sn='1'">
            <xsl:for-each select="PricingUnit/FareComponent/FlightLeg">
              <FareBasisCode>
                <xsl:value-of select="@FareBasisCode"/>
              </FareBasisCode>
            </xsl:for-each>
          </xsl:when>
          <xsl:otherwise>
            <xsl:for-each select="FareInfo">
              <FareBasisCode>
                <xsl:value-of select="FareInfo[position()=$pos]/@FareBasisCode"/>
              </FareBasisCode>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
      </FareBasisCodes>
      <PassengerFare>
        <xsl:variable name="base">
          <xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')"/>
        </xsl:variable>
        <xsl:variable name="total">
          <xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')"/>
        </xsl:variable>

        <xsl:variable name="eqCurr">
          <xsl:value-of select="/OTA_AirPriceRS/PricedItineraries/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
        </xsl:variable>

        <BaseFare>
          <xsl:choose>
            <xsl:when test="PassengerFare/EquivFare/@Amount!=''">
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(PassengerFare/EquivFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$eqCurr"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(PassengerFare/BaseFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$cur"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </BaseFare>
        <xsl:if test="ItinTotalFare/EquivFare">
          <EquivFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(PassengerFare/EquivFare/@Amount),'.','')"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$eqCurr"/>
            </xsl:attribute>
          </EquivFare>
        </xsl:if>
        <Taxes>
          <Tax>
            <xsl:attribute name="TaxCode">
              <xsl:value-of select="'TotalTax'"/>
            </xsl:attribute>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="$sn='1'">
                  <xsl:value-of select="translate(PassengerFare/Taxes/@Amount,'.','')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$total - $base"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$cur"/>
            </xsl:attribute>
          </Tax>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(PassengerFare/TotalFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="PassengerFare/UnstructuredFareCalc!=''">
          <FareCalculation>
            <xsl:value-of select="PassengerFare/UnstructuredFareCalc"/>
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
            <xsl:value-of select="$bf"/> <!--  * $nip -->
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
          <xsl:variable name="tx">
            <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare//Taxes/Tax/@Amount),'.','')"/>
          </xsl:variable>
          <xsl:attribute name="Amount">
            <xsl:variable name="bf">
              <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/TotalFare/@Amount),'.','')"/>
            </xsl:variable>
            <xsl:variable name="nip">
              <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
            </xsl:variable>
            <xsl:value-of select="$bf + $tx"/> <!--  * $nip -->
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
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="'TotalTax'"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:variable name="bf">
          <xsl:value-of select="translate(string(@Amount),'.','')"/>
        </xsl:variable>
        <xsl:variable name="nip">
          <xsl:value-of select="../../..//PassengerTypeQuantity/@Quantity"/>
        </xsl:variable>
        <xsl:variable name="tottax">
          <xsl:value-of select="$bf "/> <!-- * $nip -->
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

  <xsl:template match="PRC_QUO | TIC_REC_PRC_QUO">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:attribute name="Type">
        <xsl:value-of select="$fare"/>
      </xsl:attribute>
      <AirItineraryPricingInfo>
        <xsl:choose>
          <xsl:when test="PTC_FAR_DTL[1]/FAR_SHE_ORI[contains(text(), ' SR')]">
            <xsl:attribute name="PricingSource">Private</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="PricingSource">Published</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <ItinTotalFare>
          <!--
          xsl:variable name="totbase">
						<xsl:variable name="amttot">
							<xsl:apply-templates select="FARE_INFO[1]" mode="basefare">
								<xsl:with-param name="total">0</xsl:with-param>
							</xsl:apply-templates>
						</xsl:variable>
						<xsl:value-of select="substring-before($amttot,'/')" />
					</xsl:variable>
					<xsl:variable name="tottax">
						<xsl:variable name="amttot">
							<xsl:apply-templates select="FARE_INFO[1]" mode="TotalTax">
								<xsl:with-param name="total">0</xsl:with-param>
							</xsl:apply-templates>
						</xsl:variable>
						<xsl:value-of select="substring-before($amttot,'/')" />
					</xsl:variable
          -->
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="TTL_BAS_FAR_AMT!=''">
                  <xsl:value-of select="translate(TTL_BAS_FAR_AMT,'.','')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(EQV_BAS_FAR_AMT,'.','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:when test="EQV_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="EQV_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
          </BaseFare>
          <Taxes>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(TTL_TAX_AMT,'.','')"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(TTL_PRC_AMT,'.','')"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>          
          <xsl:variable name="trNum">
            <xsl:choose>
              <xsl:when test="./TIC_REC_NUM">
                <xsl:value-of select="TIC_REC_NUM"/>                
              </xsl:when>            
            <xsl:otherwise>
              <xsl:value-of select="1"/>              
            </xsl:otherwise>
          </xsl:choose>
          
          </xsl:variable>
          <xsl:apply-templates select="PTC_FAR_DTL" mode="Details">
            <xsl:with-param name="trN" select="$trNum" />          
        </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template name="Joined">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <xsl:param name="PQs" />
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:attribute name="Type">
        <xsl:value-of select="$fare"/>
      </xsl:attribute>
      <AirItineraryPricingInfo>
        <xsl:choose>
          <xsl:when test="$PQs/PTC_FAR_DTL[1]/FAR_SHE_ORI[contains(text(), ' SR')]">
            <xsl:attribute name="PricingSource">Private</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="PricingSource">Published</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <ItinTotalFare>
          <BaseFare>

            <xsl:variable name="amount">
              <xsl:choose>
                <xsl:when test="$PQs[1]/PTC_FAR_DTL!=''">
                  <!--<xsl:value-of select="format-number(sum($PQs/TTL_BAS_FAR_AMT),'#.00')"/>-->
                  <xsl:for-each select="$PQs/PTC_FAR_DTL">
                    <xsl:variable name="ptc" select="../PTC_FAR_DTL/PTC"/>
                    <xsl:value-of select="concat(format-number(BAS_FAR_AMT * count(../../../PAX_INF/NME_ITM[PTC=$ptc]),'#.00'), ',')"/>
                  </xsl:for-each>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:for-each select="$PQs/PTC_FAR_DTL">
                    <xsl:variable name="ptc" select="../PTC_FAR_DTL/PTC"/>
                    <xsl:value-of select="concat(format-number(EQV_BAS_FAR_AMT * count(../../../PAX_INF/NME_ITM[PTC=$ptc]),'#.00'), ',')"/>
                  </xsl:for-each>
                  <!--<xsl:value-of select="format-number(sum($PQs/EQV_BAS_FAR_AMT),'#.00')"/>-->
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="bElems">
              <xsl:call-template name="tokenizeString">
                <xsl:with-param name="list" select="$amount"/>
                <xsl:with-param name="delimiter" select="','"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:attribute name="Amount">
              <xsl:variable name="tamount" select="format-number(sum(msxsl:node-set($bElems)/elem),'#.00')"/>
              <xsl:value-of select="translate($tamount,'.','')"/>
            </xsl:attribute>

            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:when test="$PQs[last()]/EQV_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="$PQs[last()]/EQV_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>

            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="string-length(substring-after(msxsl:node-set($bElems)/elem[last()],'.'))"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>

            <xsl:variable name="taxAmount">
              <xsl:for-each select="$PQs/PTC_FAR_DTL">
                <xsl:variable name="ptc" select="../PTC_FAR_DTL/PTC"/>
                <xsl:value-of select="concat(format-number(TAX_AMT * count(../../../PAX_INF/NME_ITM[PTC=$ptc]),'#.00'), ',')"/>
              </xsl:for-each>
            </xsl:variable>

            <xsl:variable name="taxElems">
              <xsl:call-template name="tokenizeString">
                <xsl:with-param name="list" select="$taxAmount"/>
                <xsl:with-param name="delimiter" select="','"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:attribute name="Amount">
              <xsl:variable name="tamount" select="format-number(sum(msxsl:node-set($taxElems)/elem),'#.00')"/>
              <xsl:value-of select="translate($tamount,'.','')"/>
            </xsl:attribute>

            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="string-length(substring-after($PQs[last()]/TTL_TAX_AMT,'.'))"/>
            </xsl:attribute>
          </Taxes>
          <TotalFare>
            <xsl:variable name="ttAmount">
              <xsl:for-each select="$PQs/PTC_FAR_DTL">
                <xsl:variable name="ptc" select="PTC"/>
                <xsl:value-of select="concat(format-number(TTL_AMT * count(../../../PAX_INF/NME_ITM[PTC=$ptc]),'#.00'), ',')"/>
              </xsl:for-each>
            </xsl:variable>

            <xsl:variable name="tElems">
              <xsl:call-template name="tokenizeString">
                <xsl:with-param name="list" select="$ttAmount"/>
                <xsl:with-param name="delimiter" select="','"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(sum(msxsl:node-set($tElems)/elem),'.','')"/>
            </xsl:attribute>

            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
                  <xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="string-length(substring-after($PQs[last()]/TTL_PRC_AMT,'.'))"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <!--<xsl:apply-templates select="PTC_FAR_DTL" mode="Details" />-->

          <xsl:for-each select="msxsl:node-set($PQs)">
            <xsl:variable name="prc_cmd">
              <xsl:value-of select="PRC_QUO_CMD"/>
            </xsl:variable>
            <xsl:variable name="prc_txt">
              <xsl:value-of select="DOC_INS/DOC_INS_TXT"/>
            </xsl:variable>
            <xsl:variable name="tr_num">
              <xsl:value-of select="TIC_REC_NUM"/>
            </xsl:variable>
            <xsl:apply-templates select="PTC_FAR_DTL" mode="single">
              <xsl:with-param name="tr_num" select="$tr_num" />
            </xsl:apply-templates>
          </xsl:for-each>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="PTC_FAR_DTL" mode="Details">
    <xsl:param name="trN"/>
    <xsl:variable name="base">
      <xsl:choose>
        <xsl:when test="BAS_FAR_AMT!=''">
          <xsl:value-of select="translate(BAS_FAR_AMT,'.','')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="translate(EQV_BAS_FAR_AMT,'.','')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="tax">
      <xsl:value-of select="translate(TAX_AMT,'.','')" />
    </xsl:variable>
    <xsl:variable name="paxtype">
      <xsl:value-of select="PTC"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../../PAX_INF/NME_ITM[PTC=$paxtype])"/>
    </xsl:variable>
    <!-- 
    <xsl:variable name="totbase">
      <xsl:value-of select="$base * $nip"/>
    </xsl:variable>
    <xsl:variable name="tottax">
      <xsl:value-of select="$tax * $nip"/>
    </xsl:variable>
    -->
    <xsl:variable name="totfare">
      <xsl:value-of select="$base + $tax"/>
    </xsl:variable>
    <PTC_FareBreakdown>
      <xsl:attribute name="RPH">
        <xsl:choose>
          <xsl:when test="$trN=''">
            <xsl:value-of select="position()"/>          
          </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$trN"/>                  
        </xsl:otherwise>
        </xsl:choose>        
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="FAR_SHE_ORI[contains(text(), ' SR')]">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="PTC = 'CNN'">CHD</xsl:when>
            <xsl:when test="PTC = 'GGV'">GOV</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="PTC" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="$nip"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:for-each select="FAR_BAS_COD">
          <FareBasisCode>
            <xsl:value-of select="." />
          </FareBasisCode>
        </xsl:for-each>
      </FareBasisCodes>
      <PassengerFare>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$base"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="../TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
        </BaseFare>
        <Taxes>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$tax"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="../TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$totfare" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="../TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="FAR_LDR!=''">
          <FareCalculation>
            <xsl:value-of select="FAR_LDR"/>
          </FareCalculation>
          <xsl:variable name="vc">
            <xsl:value-of select="substring(substring-after(substring-after(FAR_LDR,'ROE'),' '),1,2)"/>
          </xsl:variable>
          <xsl:if test="$vc != ''">
            <ValidatingAirlineCode>
              <xsl:value-of select="$vc"/>
            </ValidatingAirlineCode>
          </xsl:if>
          <xsl:if test="../PRC_QUO_CMD">
            <SupplementalInfo>
              <xsl:value-of select="../PRC_QUO_CMD"/>
            </SupplementalInfo>
          </xsl:if>
        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>

  <xsl:template match="PTC_FAR_DTL" mode="single">
    <xsl:param name="tr_num" />
    <xsl:variable name="base">
      <xsl:choose>
        <xsl:when test="BAS_FAR_AMT!=''">
          <xsl:value-of select="BAS_FAR_AMT"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="EQV_BAS_FAR_AMT"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="tax">
      <xsl:value-of select="TAX_AMT" />
    </xsl:variable>
    <xsl:variable name="paxtype">
      <xsl:value-of select="PTC"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../../PAX_INF/NME_ITM[PTC=$paxtype])"/>
    </xsl:variable>
    <xsl:variable name="totbase">
      <xsl:choose>
        <xsl:when test="contains($base, '.')">
          <xsl:value-of select="format-number($base * $nip, '#0.00')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="format-number($base * $nip, '#0')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="tottax">
      <xsl:choose>
        <xsl:when test="contains($tax, '.')">
          <xsl:value-of select="format-number($tax * $nip, '#0.00')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="format-number($tax * $nip, '#0')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="totfare">
      <xsl:choose>
        <xsl:when test="contains($base, '.')">
          <xsl:value-of select="format-number($totbase + $tottax, '#0.00')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="format-number($totbase + $tottax, '#0')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="pnrPax">
      <xsl:for-each select="//PAX_INF/NME_ITM[PTC=$paxtype]">
        <xsl:value-of select="concat(NME_POS, ' ')"/>
      </xsl:for-each>
    </xsl:variable>

    <PTC_FareBreakdown>
      <xsl:attribute name="RPH">
        <xsl:choose>
          <xsl:when test="$tr_num!=''">
            <xsl:value-of select="$tr_num"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="position()"/>    
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="FAR_SHE_ORI[contains(text(), ' SR')]">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:call-template name="string-rtrim">
          <xsl:with-param name="string" select="$pnrPax" />
        </xsl:call-template>
      </xsl:attribute>
      <xsl:variable name="p" select="//PNR_4_INF/Line[1]/text()" />

      <xsl:variable name="segs">
        <xsl:choose>
          <xsl:when test="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')">
            <xsl:value-of select="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')"/>
          </xsl:when>
          <xsl:when test="contains($p,'*S')">
            <xsl:value-of select="translate(translate(substring-after($p,'*S'),'/',' '),'-',' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:for-each select="//AIR_SEG_INF/AIR_ITM">
              <xsl:call-template name="string-ltrim">
                <xsl:with-param name="string" select="concat(SEG_NUM, ' ')" />
              </xsl:call-template>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:call-template name="string-rtrim">
          <xsl:with-param name="string" select="translate($segs, '-', ' ')" />
        </xsl:call-template>
      </xsl:attribute>

      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="$paxtype = 'GGV'">GOV</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$paxtype" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="$nip"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:for-each select="FAR_BAS_COD">
          <xsl:variable name="seg">
            <xsl:value-of select="position()" />
          </xsl:variable>
          <xsl:variable name="arnk">
            <xsl:value-of select="//AIR_SEG_INF/AIR_ITM[SEG_NUM=$seg]/FLI_NUM" />
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$arnk!='ARNK'">
              <FareBasisCode>
                <xsl:value-of select="." />
              </FareBasisCode>
            </xsl:when>
            <xsl:otherwise>
              <FareBasisCode>VOID</FareBasisCode>
              <FareBasisCode>
                <xsl:value-of select="." />
              </FareBasisCode>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
      </FareBasisCodes>
      <PassengerFare>
        <xsl:attribute name="FareType">
          <xsl:value-of select="//FAR_TYP" />
        </xsl:attribute>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate($base, '.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after($base,'.'))"/>
          </xsl:attribute>
        </BaseFare>
        <xsl:if test="PTC_FAR_DTL/EQV_BAS_FAR_AMT">
          <xsl:variable name="ef">
            <xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equiv" />
          </xsl:variable>
          <xsl:variable name="cur">
            <xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
          </xsl:variable>
          <EquivFare>

            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="$ef != ''">
                  <xsl:value-of select="format-number(translate($ef,'.',''), '#0')"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="$cur != ''">
                  <xsl:value-of select="$cur"/>
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="string-length(substring-after($ef,'.'))"/>
            </xsl:attribute>
          </EquivFare>
        </xsl:if>
        <Taxes>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate($tax, '.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after($tax,'.'))"/>
          </xsl:attribute>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(format-number($base + $tax, '#0.00'), '.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
                <xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
              </xsl:when>
              <xsl:otherwise>USD</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after(format-number($base + $tax, '#0.00'),'.'))"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="FAR_LDR!=''">
          <FareCalculation>
            <xsl:value-of select="FAR_LDR"/>
          </FareCalculation>
          <xsl:if test="EQV_BAS_FAR_AMT">
            <xsl:variable name="cur">
              <xsl:apply-templates select="//PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
            </xsl:variable>
            <xsl:if test="//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1]">
              <BSR>
                <xsl:value-of select="substring-after(substring-before(//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1], 'USD'), '/')" />
              </BSR>
            </xsl:if>
          </xsl:if>

          <xsl:variable name="vc">
            <xsl:choose>
              <xsl:when test="contains(FAR_LDR, 'ROE')">
                <xsl:value-of select="substring(substring-after(substring-after(FAR_LDR,'ROE'),' '),1,2)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring(substring-after(substring-after(FAR_LDR,'END'),' '),1,2)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:if test="$vc != ''">
            <ValidatingAirlineCode>
              <xsl:value-of select="$vc"/>
            </ValidatingAirlineCode>
          </xsl:if>

          <!-- <xsl:if test="contains(../PRC_QUO_CMD,'-$P')"> -->
          <xsl:if test="../PRC_QUO_CMD">
            <SupplementalInfo>
              <xsl:value-of select="../PRC_QUO_CMD"/>
            </SupplementalInfo>
          </xsl:if>

        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>

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
  <xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

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
</xsl:stylesheet>
