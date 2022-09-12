<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
  <!-- 
  ================================================================== 
   Galileo_PNRRepriceRS.xsl 														
  ==================================================================
  Date: 14 Sep 2021 - Kobelev - Intermidean Errors during Price Search.
  Date: 25 Nov 2020 - Kobelev - RePriced and stored multiple FQ.
  Date: 12 Nov 2020 - Kobelev - FQ index for RePriced and stored RPH.
  Date: 03 Nov 2020 - Kobelev - Multy FQ vs. Single FQ with multy Quotes.
  Date: 02 Nov 2020 - Kobelev - Fare Quote number for respected PTC
  Date: 03 Apr 2020 - Kobelev - Multi Fare Info (TST) for multiple PTCs 
  Date: 27 Feb 2020 - Kobelev - New price display 
  Date: 24 Feb 2020 - Kobelev - Creating RePrice XSL
  ================================================================== 
  -->
  <xsl:output omit-xml-declaration="yes" />
  <xsl:template match="/">
    <OTA_PNRRepriceRS>
      <xsl:attribute name="Version">1.0</xsl:attribute>
      <xsl:apply-templates select="PNRBFManagement_53" mode="qoute" />

      <xsl:if test="PNRBFManagement_53/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="PNRBFManagement_53/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_PNRRepriceRS>
  </xsl:template>

  <xsl:template match="PNRBFManagement_53 | PNRBFManagement_17" mode="qoute">
    <xsl:choose>
      <xsl:when test="ErrorFaultList/ErrorFault">
        <Errors>
          <xsl:apply-templates select="ErrorFaultList/ErrorFault" mode="error" />
        </Errors>
      </xsl:when>
      <xsl:when test="PNRBFRetrieve/ErrText">
        <Errors>
          <xsl:apply-templates select="PNRBFRetrieve/ErrText" mode="rtv" />
        </Errors>
      </xsl:when>
      <xsl:when test="Error">
        <Errors>
          <xsl:copy-of select="Error"/>
        </Errors>
      </xsl:when>
      <xsl:when test="HostApplicationError">
        <Errors>
          <xsl:apply-templates select="HostApplicationError" />
        </Errors>
      </xsl:when>
      <xsl:when test="AirSegSell/ErrorCode != ''">
        <Errors>
          <xsl:apply-templates select="AirSegSell/ErrText" mode="airsell"/>
        </Errors>
      </xsl:when>
      <xsl:when test="AirSegSell/ErrText/Text != ''">
        <Errors>
          <xsl:apply-templates select="AirSegSell/ErrText" mode="airsell"/>
        </Errors>
      </xsl:when>
      <xsl:when test="CarSegSell/ErrorCode != ''">
        <Errors>
          <xsl:apply-templates select="CarSegSell/TypeEWFQual" mode="carsell"/>
        </Errors>
      </xsl:when>
      <xsl:when test="HtlSegSell/ErrorCode != ''">
        <Errors>
          <xsl:apply-templates select="HtlSegSell/SellErrorQual" mode="htlsell"/>
        </Errors>
      </xsl:when>
      <xsl:when test="PNRBFPrimaryBldChg/RecID = 'EROR'">
        <Errors>
          <xsl:apply-templates select="PNRBFPrimaryBldChg" mode="error"/>
        </Errors>
      </xsl:when>
      <xsl:when test="PNRBFSecondaryBldChg/RecID = 'EROR'">
        <Errors>
          <xsl:apply-templates select="PNRBFSecondaryBldChg" mode="error"/>
        </Errors>
      </xsl:when>
      <xsl:when test="EndTransaction/ErrorCode!= ''">
        <Errors>
          <xsl:apply-templates select="EndTransaction/EndTransactMessage"/>
        </Errors>
      </xsl:when>
      <xsl:when test="OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo/OutputMsg">
        <Errors>
          <xsl:apply-templates select="OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo/OutputMsg" mode="error"/>
        </Errors>
      </xsl:when>
      <xsl:when test="TransactionErrorCode/Code!= ''">
        <Errors>
          <xsl:apply-templates select="TransactionErrorCode"/>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo/ErrText/Text">
            <Errors>
              <xsl:apply-templates select="OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo/ErrText" mode="error">
                <xsl:with-param name="title" select="'FareInfo'"/>
              </xsl:apply-templates>
            </Errors>
          </xsl:when>
          <xsl:when test="OTA_AirPriceRS/PNRBFManagement_53/CancelStoredFare/ErrText">
            <Errors>
              <xsl:apply-templates select="OTA_AirPriceRS/PNRBFManagement_53/CancelStoredFare/ErrText" mode="error">
                <xsl:with-param name="title" select="'CancelStoredFare'"/>
              </xsl:apply-templates>
            </Errors>
          </xsl:when>
          <xsl:when test="OTA_AirPriceRS/PNRBFManagement_53/TransactionErrorCode">
            <Errors>
              <xsl:apply-templates select="OTA_AirPriceRS/PNRBFManagement_53/TransactionErrorCode" mode="error"/>
            </Errors>
          </xsl:when>
          <xsl:when test="OTA_AirPriceRS/Errors">
            <Errors>
              <Error Type="Galileo">
                  <xsl:value-of select="OTA_AirPriceRS/Errors/Error"/>
              </Error>
            </Errors>
          </xsl:when>
          <xsl:otherwise>
            <Success />
          </xsl:otherwise>
        </xsl:choose>
        <xsl:if test="(DocProdDisplayStoredQuote/ErrText/Text != '' and not(DocProdDisplayStoredQuote/FareNumInfo)) or PNRBFPrimaryBldChg/RecID = 'EROR' or PNRBFSecondaryBldChg/RecID = 'EROR' or DocProdFareManipulation_4_0/TransactionErrorCode or FareInfo/RespHeader/ErrMsg = 'Y' or Warning!=''">
          <Warnings>
            <xsl:choose>
              <xsl:when test="DocProdDisplayStoredQuote/ErrText/Text != '' and not(DocProdDisplayStoredQuote/FareNumInfo)">
                <Warning Type="Galileo">
                  <xsl:value-of select="DocProdDisplayStoredQuote/ErrText/Text"/>
                </Warning>
              </xsl:when>
              <!--xsl:when test="DocProdDisplayStoredQuote and PNRBFRetrieve/GenPNRInfo/FareDataExistsInd = 'N'">
								<Warning Type="Galileo">
									<xsl:text>Store price failed</xsl:text>
								</Warning>
							</xsl:when-->
            </xsl:choose>
            <xsl:if test="PNRBFPrimaryBldChg/RecID = 'EROR'">
              <Warning Type="Galileo">
                <xsl:value-of select="PNRBFPrimaryBldChg/Text"/>
                <xsl:choose>
                  <xsl:when test="contains(PNRBFPrimaryBldChg/DataBlkInd,'M')">
                    <xsl:text> IN FREQUENT FLYER (CustLoyalty) ELEMENT</xsl:text>
                  </xsl:when>
                </xsl:choose>
              </Warning>
            </xsl:if>
            <xsl:if test="PNRBFSecondaryBldChg/RecID = 'EROR'">
              <Warning Type="Galileo">
                <xsl:value-of select="PNRBFSecondaryBldChg/Text"/>
                <xsl:choose>
                  <xsl:when test="contains(PNRBFPrimaryBldChg/DataBlkInd,'O')">
                    <xsl:text> IN OSI ELEMENT</xsl:text>
                  </xsl:when>
                </xsl:choose>
              </Warning>
            </xsl:if>
            <xsl:if test="DocProdFareManipulation_4_0/Ticketing/ErrText/Text != ''">
              <xsl:for-each select="DocProdFareManipulation_4_0/Ticketing/ErrText">
                <Warning Type="Galileo">
                  <xsl:value-of select="Text"/>
                </Warning>
              </xsl:for-each>
            </xsl:if>
            <xsl:if test="FareInfo/RespHeader/ErrMsg = 'Y'">
              <Warning Type="Galileo">
                <xsl:value-of select="FareInfo/OutputMsg/Text"/>
              </Warning>
            </xsl:if>
            <xsl:if test="Warning!=''">
              <Warning Type="TripXML">
                <xsl:value-of select="Warning"/>
              </Warning>
            </xsl:if>
          </Warnings>
        </xsl:if>
        <PricedItineraries>
          <xsl:apply-templates select="./DocProdDisplayStoredQuote[not(ErrText)]" mode="Fare">
            <xsl:with-param name="fare" select="'stored'"/>
            <xsl:with-param name="sn" select="'1'"/>
          </xsl:apply-templates>
          <xsl:choose>
            <xsl:when test="./OTA_AirPriceRS/PNRBFManagement_53/FareInfo">
              <xsl:choose>
                <xsl:when test="count(./OTA_AirPriceRS/PNRBFManagement_53/FareInfo) > 1">
                  <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53" mode="Fare">
                    <xsl:with-param name="fare" select="'new'"/>
                    <xsl:with-param name="sn" select="'2'"/>
                  </xsl:apply-templates>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53/FareInfo" mode="Fare">
                    <xsl:with-param name="fare" select="'new'"/>
                    <xsl:with-param name="sn" select="'2'"/>
                    <xsl:with-param name="fareType" select="./OTA_AirPriceRS/PNRBFManagement_53/FareInfo/GenQuoteDetails/QuoteType"/>
                  </xsl:apply-templates>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:when test="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote">
              <xsl:choose>
                <xsl:when test="count(./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]) > 1">

                  <PricedItinerary>
                    <xsl:attribute name="SequenceNumber">
                      <xsl:value-of select="2"/>
                    </xsl:attribute>
                    <AirItineraryPricingInfo>
                      <xsl:choose>
                        <xsl:when test="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails/QuoteType='P'">
                          <xsl:attribute name="PricingSource">Private</xsl:attribute>
                        </xsl:when>
                        <xsl:when test="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails/QuoteType='A'">
                          <xsl:attribute name="PricingSource">Private</xsl:attribute>
                        </xsl:when>
                        <xsl:when test="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails/PrivFQd='Y'">
                          <xsl:attribute name="PricingSource">Private</xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="PricingSource">Published</xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                      <ItinTotalFare>
                        
                        <xsl:variable name="baseFareAmt">
                          <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53" mode="multyBaseFare" />
                        </xsl:variable>

                        <xsl:variable name="totalFareAmt">
                          <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53" mode="multyTotalFare" />
                        </xsl:variable>

                        <BaseFare>
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$baseFareAmt" />
                          </xsl:attribute>
                          <xsl:attribute name="CurrencyCode">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails/TotCurrency" />
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails/TotDecPos" />
                          </xsl:attribute>
                        </BaseFare>

                        <Taxes>
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$totalFareAmt - $baseFareAmt" />
                          </xsl:attribute>
                          <xsl:attribute name="CurrencyCode">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails[1]/TotCurrency" />
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails[1]/TotDecPos" />
                          </xsl:attribute>
                        </Taxes>
                        <TotalFare>
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$totalFareAmt" />
                          </xsl:attribute>
                          <xsl:attribute name="CurrencyCode">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails[1]/TotCurrency" />
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails[1]/TotDecPos" />
                          </xsl:attribute>
                        </TotalFare>
                      </ItinTotalFare>
                      <PTC_FareBreakdowns>
                        <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]" mode="multy" />
                      </PTC_FareBreakdowns>
                    </AirItineraryPricingInfo>
                  </PricedItinerary>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:apply-templates select="./OTA_AirPriceRS/PNRBFManagement_53/DocProdDisplayStoredQuote[not(ErrText)]" mode="Fare">
                    <xsl:with-param name="fare" select="'new'"/>
                    <xsl:with-param name="sn" select="'2'"/>
                  </xsl:apply-templates>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:when>
          </xsl:choose>
          <xsl:apply-templates select="./OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo" mode="Fare">
            <xsl:with-param name="fare" select="'new'"/>
            <xsl:with-param name="sn" select="'2'"/>
            <xsl:with-param name="fareType" select="./OTA_AirPriceRS/FareQuoteClassSpecific_35/FareInfo/GenQuoteDetails/QuoteType"/>
          </xsl:apply-templates>
        </PricedItineraries>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			PNR General Info - PNRBFRetrieve                                              -->
  <!--************************************************************************************-->
  <xsl:template match="DocProdDisplayStoredQuote | FareInfo" mode="Fare">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <xsl:if test="FareNumInfo">
        <xsl:apply-templates select="." />
      </xsl:if>
      <xsl:if test=". = ../FareInfo">
        <xsl:apply-templates select="." />
      </xsl:if>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="multy">
    <xsl:apply-templates select="." />
  </xsl:template>

  <xsl:template match="PNRBFManagement_53" mode="Fare">
    <xsl:param name="fare"/>
    <xsl:param name="sn"/>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">
        <xsl:value-of select="$sn"/>
      </xsl:attribute>
      <AirItineraryPricingInfo>
        <xsl:choose>
          <xsl:when test="FareInfo[1]/GenQuoteDetails[1]/QuoteType='P'">
            <xsl:attribute name="PricingSource">Private</xsl:attribute>
          </xsl:when>
          <xsl:when test="FareInfo[1]/GenQuoteDetails[1]/QuoteType='A'">
            <xsl:attribute name="PricingSource">Private</xsl:attribute>
          </xsl:when>
          <xsl:when test="FareInfo[1]/GenQuoteDetails[1]/PrivFQd='Y'">
            <xsl:attribute name="PricingSource">Private</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="PricingSource">Published</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <ItinTotalFare>
          <xsl:variable name="amttota">
            <xsl:apply-templates select="FareInfo/GenQuoteDetails" mode="basefare2">
              <xsl:with-param name="total">0</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:variable name="amttot">
            <xsl:value-of select="substring-before($amttota,'/')" />
          </xsl:variable>
          <xsl:variable name="amttot1a">
            <xsl:apply-templates select="FareInfo/GenQuoteDetails" mode="totalfare2">
              <xsl:with-param name="total">0</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:variable name="amttot1">
            <xsl:value-of select="substring-before($amttot1a,'/')" />
          </xsl:variable>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$amttot" />
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotCurrency" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotDecPos" />
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$amttot1 - $amttot" />
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotCurrency" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotDecPos" />
            </xsl:attribute>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$amttot1" />
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotCurrency" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="FareInfo/GenQuoteDetails[1]/TotDecPos" />
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="FareInfo/GenQuoteDetails" mode="Details" >
            <xsl:with-param name="FQType" select="'newFQ'" />
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			PNR Retrieve Errors                                           	              -->
  <!--************************************************************************************-->
  <xsl:template match="ErrText" mode="rtv">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="Text" />
    </Error>
  </xsl:template>

  <xsl:template match="TypeEWFQual" mode="carsell">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="ErrMsg" />
      <xsl:if test="../../CarSegSellText/LineDescAry">
        <xsl:for-each select="../../CarSegSellText/LineDescAry/LineDescInfo">
          <xsl:text> - </xsl:text>
          <xsl:value-of select="Txt"/>
        </xsl:for-each>
      </xsl:if>
    </Error>
  </xsl:template>

  <xsl:template match="SellErrorQual" mode="htlsell">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="ErrMsg" />
      <xsl:if test="../../HtlSegSellText/LineDescAry">
        <xsl:for-each select="../../HtlSegSellText/LineDescAry/LineDescInfo">
          <xsl:text> - </xsl:text>
          <xsl:value-of select="Txt"/>
        </xsl:for-each>
      </xsl:if>
    </Error>
  </xsl:template>

  <xsl:template match="ErrText" mode="airsell">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="Text" />
      <xsl:text> - </xsl:text>
      <xsl:value-of select="preceding-sibling::AirSell[1]/Vnd"/>
      <xsl:value-of select="preceding-sibling::AirSell[1]/FltNum"/>
    </Error>
  </xsl:template>

  <xsl:template match="EndTransactMessage">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="Text" />
    </Error>
    <xsl:if test="../../PNRBFPrimaryBldChg/RecID = 'EROR'">
      <Error>
        <xsl:attribute name="Type">Galileo</xsl:attribute>
        <xsl:value-of select="../../PNRBFPrimaryBldChg/Text" />
      </Error>
    </xsl:if>
  </xsl:template>

  <xsl:template match="TransactionErrorCode">
    <xsl:choose>
      <xsl:when test="../CustomCheckRuleExecute/ErrorCode != ''">
        <xsl:for-each select="../CustomCheckRuleExecute/CCRuleExecute">
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:if test="Code">
              <xsl:attribute name="Code">
                <xsl:value-of select="Code"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:value-of select="Msg"/>
          </Error>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <Error>
          <xsl:attribute name="Type">Galileo</xsl:attribute>
          <xsl:if test="Code">
            <xsl:attribute name="Code">
              <xsl:value-of select="Code"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="../PNRBFPrimaryBldChg/RecID = 'EROR'">
              <xsl:value-of select="../PNRBFPrimaryBldChg/Text"/>
            </xsl:when>
            <xsl:when test="../CustomCheckRuleExecute/ErrorCode != ''">
              <xsl:value-of select="../CustomCheckRuleExecute/CCRuleExecute/Msg"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>Processing error - </xsl:text>
              <xsl:value-of select="Domain"/>
            </xsl:otherwise>
          </xsl:choose>
        </Error>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="ErrorFaultList/ErrorFault" mode="error">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:choose>
        <xsl:when test="Text != ''">
          <xsl:value-of select="concat(Id,' - ', normalize-space(Text))" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>GALILEO ERROR </xsl:text>
          <xsl:value-of select="ErrorCode"/>
          <xsl:text> - STRUCTURED DATA ERROR</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </Error>
  </xsl:template>

  <xsl:template match="TransactionErrorCode" mode="error">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:choose>
        <xsl:when test="Domain != ''">
          <xsl:value-of select="concat(Code,' - ', normalize-space(Domain))" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>GALILEO ERROR </xsl:text>
          <xsl:value-of select="ErrorCode"/>
          <xsl:text> - ERROR</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </Error>
  </xsl:template>

  <xsl:template match="PNRBFSecondaryBldChg" mode="error">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:choose>
        <xsl:when test="Text != ''">
          <xsl:value-of select="Text" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>GALILEO ERROR </xsl:text>
          <xsl:value-of select="ErrorCode"/>
          <xsl:text> - STRUCTURED DATA ERROR</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </Error>
  </xsl:template>

  <xsl:template match="PNRBFPrimaryBldChg" mode="error">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="Text" />
      <xsl:choose>
        <xsl:when test="contains(DataBlkInd,'T')">
          <xsl:text> IN TICKETING ELEMENT</xsl:text>
        </xsl:when>
      </xsl:choose>
    </Error>
  </xsl:template>

  <xsl:template match="OutputMsg" mode="error">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>

      <xsl:choose>
        <xsl:when test="contains(Text, 'CDATA[')">
          <xsl:value-of select="substring-after(substring-before(Text, ']]'), '[CDATA[')" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="Text" />
        </xsl:otherwise>
      </xsl:choose>
    </Error>
  </xsl:template>

  <xsl:template match="ErrText" mode="error">
    <xsl:param name="title" />
    <Error>
      <xsl:attribute name="Type">
        <xsl:value-of select="$title" />
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="Text != ''">
          <xsl:value-of select="concat(Err,' - ', normalize-space(Text))" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>GALILEO ERROR </xsl:text>
          <xsl:value-of select="ErrorCode"/>
          <xsl:value-of select="../ErrorCode" />
        </xsl:otherwise>
      </xsl:choose>
    </Error>
  </xsl:template>

  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			                                -->
  <!--************************************************************************************-->
  <xsl:template match="DocProdDisplayStoredQuote">
    <xsl:variable name="fareNum" select="FareNumInfo/FareNumAry/FareNum" />

    <AirItineraryPricingInfo>
      <xsl:choose>
        <xsl:when test="GenQuoteDetails/QuoteType='P'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="GenQuoteDetails/QuoteType='A'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="GenQuoteDetails/PrivFQd='Y'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <ItinTotalFare>
        <xsl:variable name="amttota">
          <xsl:apply-templates select="GenQuoteDetails" mode="basefare">
            <xsl:with-param name="total">0</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="amttot">
          <xsl:value-of select="substring-before($amttota,'/')" />
        </xsl:variable>
        <xsl:variable name="amttot1a">
          <xsl:apply-templates select="GenQuoteDetails" mode="totalfare">
            <xsl:with-param name="total">0</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="amttot1">
          <xsl:value-of select="substring-before($amttot1a,'/')" />
        </xsl:variable>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails/TotDecPos" />
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <!--  
            <xsl:apply-templates select="GenQuoteDetails[1]" mode="TotalTax" />
            -->
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot1 - $amttot" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
          </xsl:attribute>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot1" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
          </xsl:attribute>
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:choose>
          <xsl:when test="count(GenQuoteDetails) > 1">
            <xsl:apply-templates select="GenQuoteDetails" mode="Details">
              <xsl:with-param name="FQType" select="'oneFQ'" />
              <xsl:with-param name="FQNum" select="$fareNum" />
            </xsl:apply-templates>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="GenQuoteDetails" mode="Details" >
              <xsl:with-param name="FQType" select="'multyFQ'" />
              <xsl:with-param name="FQNum" select="$fareNum" />
            </xsl:apply-templates>
          </xsl:otherwise>
        </xsl:choose>
      </PTC_FareBreakdowns>
    </AirItineraryPricingInfo>
  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="multy">
    <xsl:variable name="fareNum" select="FareNumInfo/FareNumAry/FareNum" />


    <xsl:choose>
      <xsl:when test="count(GenQuoteDetails) > 1">
        <xsl:apply-templates select="GenQuoteDetails" mode="Details">
          <xsl:with-param name="FQType" select="'oneFQ'" />
          <xsl:with-param name="FQNum" select="$fareNum" />
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="GenQuoteDetails" mode="Details" >
          <xsl:with-param name="FQType" select="'multyFQ'" />
          <xsl:with-param name="FQNum" select="$fareNum" />
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>


  </xsl:template>

  <xsl:template match="FareInfo">
    <AirItineraryPricingInfo>
      <xsl:choose>
        <xsl:when test="GenQuoteDetails[1]/QuoteType='P'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="GenQuoteDetails[1]/QuoteType='A'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="GenQuoteDetails[1]/PrivFQd='Y'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <ItinTotalFare>
        <xsl:variable name="amttota">
          <xsl:apply-templates select="GenQuoteDetails" mode="basefare2">
            <xsl:with-param name="total">0</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="amttot">
          <xsl:value-of select="substring-before($amttota,'/')" />
        </xsl:variable>
        <xsl:variable name="amttot1a">
          <xsl:apply-templates select="GenQuoteDetails" mode="totalfare2">
            <xsl:with-param name="total">0</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="amttot1">
          <xsl:value-of select="substring-before($amttot1a,'/')" />
        </xsl:variable>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails/TotDecPos" />
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot1 - $amttot" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
          </xsl:attribute>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$amttot1" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
          </xsl:attribute>
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:apply-templates select="GenQuoteDetails" mode="Details" >
          <xsl:with-param name="FQType" select="'newFQ'" />
          <xsl:with-param name="FQNum" select="position()" />
        </xsl:apply-templates>
      </PTC_FareBreakdowns>
    </AirItineraryPricingInfo>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                                  -->
  <!--************************************************************************************-->
  <xsl:template match="GenQuoteDetails" mode="basefare">
    <xsl:param name="total" />
    <xsl:param name="sep" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$uk]/ApplesToAry/AppliesTo)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:choose>
        <xsl:when test="EquivAmt != '0'">
          <xsl:value-of select="EquivAmt * $totpax" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="BaseFareAmt * $totpax" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:value-of select="$total + $thistotal" />
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="basefare">
      <xsl:with-param name="total">
        <xsl:value-of select="$bigtotal" />
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:value-of select="$bigtotal" />
    <xsl:if test="$sep=''">
      <xsl:text>/</xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="basefare2">
    <xsl:param name="total" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../PsgrTypes[UniqueKey=$uk]/PsgrNumAry/PsgrNum)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:choose>
        <xsl:when test="EquivAmt != '0'">
          <xsl:value-of select="EquivAmt * $totpax" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="BaseFareAmt * $totpax" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:value-of select="$total + $thistotal" />
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="basefare2">
      <xsl:with-param name="total">
        <xsl:value-of select="$bigtotal" />
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:value-of select="$bigtotal" />
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="totalfare">
    <xsl:param name="total" />
    <xsl:param name="sep" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$uk]/ApplesToAry/AppliesTo)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="TotAmt * $totpax" />
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:value-of select="$total + $thistotal" />
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="totalfare">
      <xsl:with-param name="total">
        <xsl:value-of select="$bigtotal" />
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:value-of select="$bigtotal" />
    <xsl:if test="$sep=''">
      <xsl:text>/</xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="totalfare2">
    <xsl:param name="total" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../PsgrTypes[UniqueKey=$uk]/PsgrNumAry/PsgrNum)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="TotAmt * $totpax" />
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:value-of select="$total + $thistotal" />
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="totalfare2">
      <xsl:with-param name="total">
        <xsl:value-of select="$bigtotal" />
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:value-of select="$bigtotal" />
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="toteachtax">
    <xsl:param name="total" />
    <xsl:param name="taxcode" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$uk]/ApplesToAry/AppliesTo)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="TaxDataAry/TaxData[Country=$taxcode]/Amt * $totpax" />
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:choose>
        <xsl:when test="$thistotal = 'NaN'">
          <xsl:value-of select="$total" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$total + $thistotal" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="toteachtax">
      <xsl:with-param name="total">
        <xsl:value-of select="$bigtotal" />
      </xsl:with-param>
      <xsl:with-param name="taxcode">
        <xsl:value-of select="$taxcode" />
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:value-of select="$bigtotal" />
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="TotalTax">
    <xsl:for-each select="TaxDataAry/TaxData">
      <xsl:variable name="taxcode" select="Country" />
      <xsl:variable name="addzeros">00000</xsl:variable>
      <xsl:variable name="numberdecimals" select="../../TotDecPos" />
      <xsl:variable name="amttot">
        <xsl:apply-templates select="../../../GenQuoteDetails[1]" mode="toteachtax">
          <xsl:with-param name="total">0</xsl:with-param>
          <xsl:with-param name="taxcode">
            <xsl:value-of select="$taxcode" />
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:variable>
      <xsl:variable name="SumOfTax">
        <xsl:value-of select="substring-before($amttot,'/')" />
      </xsl:variable>
      <xsl:variable name="SumOfTaxWithoutDec">
        <xsl:value-of select="translate(string($SumOfTax),'. ','')" />
      </xsl:variable>
      <xsl:variable name="Length">
        <xsl:value-of select="string-length($SumOfTaxWithoutDec)" />
      </xsl:variable>
      <xsl:variable name="subtract">
        <xsl:value-of select="concat(1,substring($addzeros,1,$numberdecimals))" />
      </xsl:variable>
      <xsl:variable name="SumOfTaxTwo">
        <xsl:value-of select="$SumOfTax * $subtract" />
      </xsl:variable>
      <xsl:variable name="NoDecimals">
        <xsl:value-of select="substring-before(string($SumOfTaxTwo),'.')" />
      </xsl:variable>
      <Tax>
        <xsl:attribute name="Code">
          <xsl:value-of select="Country" />
        </xsl:attribute>
        <xsl:attribute name="Amount">
          <xsl:choose>
            <xsl:when test="contains($SumOfTaxTwo, '.')">
              <xsl:value-of select="$NoDecimals" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$SumOfTaxTwo" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="CurrencyCode">
          <xsl:value-of select="../../TotCurrency" />
        </xsl:attribute>
        <xsl:attribute name="DecimalPlaces">
          <xsl:value-of select="../../TotDecPos" />
        </xsl:attribute>
      </Tax>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="Details">
    <xsl:param name="FQType" />
    <xsl:param name="FQNum" />

    <xsl:variable name="pos">
      <xsl:value-of select="position()" />
    </xsl:variable>
    <xsl:variable name="paxno">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>

    <xsl:variable name="paxno2">
      <xsl:value-of select="format-number(UniqueKey,'0')" />
    </xsl:variable>

    <PTC_FareBreakdown>
      <xsl:attribute name="PricingSource">
        <xsl:choose>
          <xsl:when test="QuoteType='P'">Private</xsl:when>
          <xsl:when test="QuoteType='A'">Private</xsl:when>
          <xsl:when test="PrivFQd='Y'">Private</xsl:when>
          <xsl:otherwise>Published</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:attribute name="FCMI">
        <xsl:value-of select="QuoteType"/>
      </xsl:attribute>

      <xsl:variable name="fqN">
        <xsl:choose>
          <xsl:when test="$FQType='newFQ'">
            <xsl:value-of select="UniqueKey"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="../FareNumInfo/FareNumAry/FareNum"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="../AssocSegs/SegNumAry/SegNum != ''">
          <xsl:attribute name="FlightRefNumberRPHList">
            <xsl:for-each select="../AssocSegs/SegNumAry">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="SegNum"/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="../SegRelatedInfo[UniqueKey=$paxno2]/RelSegNum != ''">
          <xsl:attribute name="FlightRefNumberRPHList">
            <xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno2]/RelSegNum">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="."/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]">
          <xsl:attribute name="TravelerRefNumberRPHList">
            <xsl:for-each select="../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]/ApplesToAry/AppliesTo">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="AbsNameNum"/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="../AssocPsgrs/PsgrAry/Psgr/AbsNameNum != ''">
          <xsl:attribute name="TravelerRefNumberRPHList">
            <xsl:for-each select="../AssocPsgrs/PsgrAry/Psgr">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="AbsNameNum"/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="../AgntEnteredPsgrDescInfo/ApplesToAry/AppliesTo/AbsNameNum != ''">
          <xsl:attribute name="TravelerRefNumberRPHList">
            <xsl:for-each select="../AgntEnteredPsgrDescInfo/ApplesToAry/AppliesTo">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="AbsNameNum"/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="../PsgrTypes[UniqueKey=$paxno]/PsgrNumAry/PsgrNum != ''">
          <xsl:attribute name="TravelerRefNumberRPHList">
            <xsl:for-each select="../PsgrTypes[UniqueKey=$paxno]/PsgrNumAry/PsgrNum">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="."/>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>

      <xsl:attribute name="RPH">
        <xsl:choose>
          <!--
          <xsl:when test="../GrandFeeTotal">
            <xsl:value-of select="concat(QuoteNum,'.',UniqueKey)"/>
          </xsl:when>
          -->
          <xsl:when test="$FQType='newFQ'">
            <xsl:value-of select="concat($FQNum,'.',QuoteNum)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($FQNum,'.',UniqueKey)"/>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:attribute>

      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:variable name="PsgrType">
            <xsl:choose>
              <xsl:when test="../PsgrTypes[UniqueKey=$paxno]/RespPIC">
                <xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/RespPIC"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]/AgntEnteredPsgrDesc"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$PsgrType = 'AA'">ADT</xsl:when>
            <xsl:when test="$PsgrType = 'AD'">ADT</xsl:when>
            <xsl:when test="$PsgrType = 'CHD'">CHD</xsl:when>
            <xsl:when test="$PsgrType = 'CH'">CHD</xsl:when>
            <xsl:when test="$PsgrType = 'C'">CHD</xsl:when>
            <xsl:when test="$PsgrType = 'IN'">INF</xsl:when>
            <xsl:when test="$PsgrType = 'INF'">INF</xsl:when>
            <xsl:when test="$PsgrType = 'GOV'">GOV</xsl:when>
            <xsl:when test="$PsgrType = 'MIL'">MIL</xsl:when>
            <xsl:when test="$PsgrType = 'CD'">SRC</xsl:when>
            <xsl:when test="$PsgrType = 'SC'">SRC</xsl:when>
            <xsl:when test="$PsgrType = 'STU'">STD</xsl:when>
            <xsl:when test="$PsgrType = 'YC'">YTH</xsl:when>
            <xsl:when test="$PsgrType = ''">ADT</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$PsgrType" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:choose>
            <xsl:when test="../GrandFeeTotal">
              <xsl:value-of select="count(../PsgrTypes[UniqueKey=$paxno]/PsgrNumAry/PsgrNum)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]/ApplesToAry/AppliesTo)"/>
            </xsl:otherwise>
          </xsl:choose>

        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno2]">
          <FareBasisCode>
            <xsl:value-of select="../SegRelatedInfo[UniqueKey=$paxno2]/FIC" />
            <xsl:if test="string-length(../SegRelatedInfo[UniqueKey=$paxno2]/FIC) = 8">
              <xsl:variable name="fic">
                <xsl:choose>
                  <xsl:when test="contains(../SegRelatedInfo[UniqueKey=$paxno2]/TkDesignator, '/')">
                    <xsl:value-of select="concat(../SegRelatedInfo[UniqueKey=$paxno2]/FIC,../SegRelatedInfo[UniqueKey=$paxno2]/TkDesignator)" />
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(../SegRelatedInfo[UniqueKey=$paxno2]/FIC,'/',../SegRelatedInfo[UniqueKey=$paxno2]/TkDesignator)" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:if test="../FareConstruction/FareConstructText[contains(.,$fic)]">
                <xsl:value-of select="concat('/',../SegRelatedInfo[UniqueKey=$paxno2]/TkDesignator)"/>
              </xsl:if>
            </xsl:if>
          </FareBasisCode>
        </xsl:for-each>
      </FareBasisCodes>
      <PassengerFare>
        <xsl:if test="PrivFQd='Y'">
          <xsl:attribute name="NegotiatedFare">true</xsl:attribute>
        </xsl:if>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="TotCurrency = BaseFareCurrency">
                <xsl:value-of select="BaseFareAmt" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="EquivAmt" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="TotDecPos" />
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <xsl:apply-templates select="TaxDataAry/TaxData" mode="tax" />
          <xsl:if test="not(TaxDataAry/TaxData/Amt)">
            <Tax Amount="000" />
          </xsl:if>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="TotAmt" />
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="TotCurrency" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="TotDecPos" />
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="../FareConstruction[UniqueKey=$paxno]/FareConstructText">
          <FareCalculation>
            <xsl:value-of select="../FareConstruction[UniqueKey=$paxno]/FareConstructText"/>
          </FareCalculation>
        </xsl:if>
        <!-- 
        <xsl:if test="bankerRates/firstRateDetail">
          <BSR>
            <xsl:value-of select="bankerRates/firstRateDetail/amount"/>
          </BSR>
        </xsl:if>
        <xsl:if test="otherPricingInfo/attributeDetails[attributeType='PAY']">
          <PaymentRestrictions>
            <xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='PAY']/attributeDescription"/>
          </PaymentRestrictions>
        </xsl:if>        
        -->
        <ValidatingAirlineCode>
          <xsl:choose>
            <xsl:when test="../GrandFeeTotal">
              <xsl:value-of select="../GrandFeeTotal/PlatingAirV"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="../PlatingAirVMod/AirV"/>
            </xsl:otherwise>
          </xsl:choose>
        </ValidatingAirlineCode>

        <xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno2]">
          <xsl:if test="BagInfo">
            <BagAllowance>
              <xsl:choose>
                <xsl:when test="contains(BagInfo, 'PC')">
                  <xsl:attribute name="Quantity">
                    <xsl:value-of select="substring-before(BagInfo,'PC')"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Piece</xsl:text>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Weight">
                    <xsl:value-of select="BagInfo"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Weight</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="Unit">
                    <xsl:value-of select="BagInfo"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:attribute name="ItinSeqNumber">
                <xsl:value-of select="RelSegNum"/>
              </xsl:attribute>
            </BagAllowance>
          </xsl:if>
        </xsl:for-each>

      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>

  <!-- 
  *******************************************************************
        Get Total Base Fare with Multy FQ
  *******************************************************************
  -->
  <xsl:template match="PNRBFManagement_53" mode="multyBaseFare">
    <xsl:variable name="valuesXml">
      <values>
        <xsl:for-each select="DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails">
          <value>
            <xsl:apply-templates select="." mode="basefare">
              <xsl:with-param name="total">0</xsl:with-param>
              <xsl:with-param name="sep">1</xsl:with-param>
            </xsl:apply-templates>
          </value>
        </xsl:for-each>
      </values>
    </xsl:variable>

    <xsl:variable name="values" select="msxsl:node-set($valuesXml)/values/value" />
    <xsl:value-of select="sum($values)" />
  </xsl:template>


  <!-- 
  *******************************************************************
        Get Total Total Fare with Multy FQ
  *******************************************************************
  -->
  <xsl:template match="PNRBFManagement_53" mode="multyTotalFare">

    <xsl:variable name="valuesXml">
      <values>
        <xsl:for-each select="DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails">
          <value>
            <xsl:apply-templates select="." mode="totalfare">
              <xsl:with-param name="total">0</xsl:with-param>
              <xsl:with-param name="sep">1</xsl:with-param>
            </xsl:apply-templates>
          </value>
        </xsl:for-each>
      </values>
    </xsl:variable>

    <xsl:variable name="values" select="msxsl:node-set($valuesXml)/values/value" />
    <xsl:value-of select="sum($values)" />
  </xsl:template>

  <!-- **************************************************************-->
  <!--      Get individual Tax info  	                               -->
  <!-- **************************************************************-->
  <xsl:template match="TaxData" mode="tax">
    <xsl:variable name="tax" select="translate(string(Amt),'. ','')" />
    <Tax>
      <xsl:attribute name="Code">
        <xsl:value-of select="Country" />
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:choose>
          <xsl:when test="starts-with($tax,'00000')">
            <xsl:value-of select="substring($tax, 6)" />
          </xsl:when>
          <xsl:when test="starts-with($tax,'0000')">
            <xsl:value-of select="substring($tax, 5)" />
          </xsl:when>
          <xsl:when test="starts-with($tax,'000')">
            <xsl:value-of select="substring($tax, 4)" />
          </xsl:when>
          <xsl:when test="starts-with($tax,'00')">
            <xsl:value-of select="substring($tax, 3)" />
          </xsl:when>
          <xsl:when test="starts-with($tax,'0')">
            <xsl:value-of select="substring($tax, 2)" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$tax" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../TotCurrency" />
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="../../TotDecPos" />
      </xsl:attribute>
    </Tax>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			Form of Payment       						                                            -->
  <!--************************************************************************************-->
  <xsl:template match="CreditCardFOP">
    <xsl:attribute name="RPH">
      <xsl:value-of select="position()" />
    </xsl:attribute>
    <PaymentCard>
      <xsl:attribute name="CardCode">
        <xsl:choose>
          <xsl:when test="Vnd = 'CA'">MC</xsl:when>
          <xsl:when test="Vnd = 'DC'">DN</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Vnd" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="CardNumber">
        <xsl:variable name="cardnum">
          <xsl:value-of select="translate(string(Acct),' ','')" />
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="string-length($cardnum) = 14">
            <xsl:text>xxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,11)"/>
          </xsl:when>
          <xsl:when test="string-length($cardnum) = 15">
            <xsl:text>xxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,12)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>xxxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,13)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="ExtTxt != ''">
        <xsl:attribute name="SeriesCode">

        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="ExpireDate">
        <xsl:variable name="zeroes">000</xsl:variable>
        <xsl:variable name="expdate">
          <xsl:value-of select="substring(string($zeroes),1,4-string-length(ExpDt))" />
          <xsl:value-of select="ExpDt" />
        </xsl:variable>
        <xsl:value-of select="substring($expdate, 1, 2)" />
        <xsl:text>/20</xsl:text>
        <xsl:value-of select="substring($expdate, 3, 2)" />
      </xsl:attribute>
    </PaymentCard>
    <TPA_Extensions>
      <xsl:attribute name="FOPType">CC</xsl:attribute>
    </TPA_Extensions>
  </xsl:template>

  <xsl:template match="OtherFOP">
    <xsl:attribute name="RPH">
      <xsl:value-of select="position()" />
    </xsl:attribute>
    <TPA_Extensions>
      <xsl:attribute name="FOPType">
        <xsl:choose>
          <xsl:when test="ID='1'">CASH</xsl:when>
          <xsl:when test="ID='3'">NONREF</xsl:when>
          <xsl:when test="ID='4'">MISCELLENEOUS</xsl:when>
          <xsl:when test="ID='5'">INVOICE</xsl:when>
          <xsl:when test="ID='7'">GOVERNMENT</xsl:when>
          <xsl:when test="ID='8'">PSEUDOCASH</xsl:when>
          <xsl:when test="ID='9'">CREDIT</xsl:when>
          <xsl:when test="ID='11'">AKBONS</xsl:when>
          <xsl:when test="ID='12'">PAYLATE</xsl:when>
          <xsl:when test="ID='13'">EXCHANGE</xsl:when>
          <xsl:when test="ID='14'">MONEYORDER</xsl:when>
          <xsl:when test="ID='15'">TRAVELERSCHECK</xsl:when>
          <xsl:otherwise>OTHER</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </TPA_Extensions>
  </xsl:template>

  <xsl:template match="CheckFOP">
    <xsl:attribute name="RPH">
      <xsl:value-of select="position()" />
    </xsl:attribute>
    <TPA_Extensions>
      <xsl:attribute name="FOPType">CHECK</xsl:attribute>
    </TPA_Extensions>
  </xsl:template>

</xsl:stylesheet>
