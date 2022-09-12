<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
  AmadeusWS_PNRRepriceRS.xsl 												
  ================================================================== 
  Date: 03 Jan 2018 -  Samokhvalov - PassangeFare corrected -> amount per 1 pax 
  Date: 03 Feb 2016 -  Rastko - added extra error check in Fare_PricePNRWithBookingClassReply 
  Date: 20 Jan 2016 -  Rastko - added mapping of tour code				
  Date: 01 Dec 2015 -  Rastko - corrected identification of pax type INF				
  Date: 15 Sep 2015 -  Rastko - corrected fareList RPH numbering				
  Date: 24 Aug 2015 -  Rastko - added support for TTK reply						
  Date: 04 Jun 2014 -  Rastko - added mapping of Fare_PricePNRWithLowerFaresReply	
  Date: 16 May 2014 -  Rastko - corrected mapping of passenger type for repriced fare	
  Date: 13 May 2014 -  Rastko - made correction to return right TST number when fare stored	
  Date: 05 May 2014 -  Rastko - added PTC_FareBreakdown mapping			
  Date: 02 May 2014 -  Alex - added mapping of ConversationID					
  Date: 30 Apr 2103 -  Rastko - added support for pricing by Fare Number (TST number)	
  Date: 22 Mar 2010 -  Rastko														
  ================================================================== 
  -->
  <xsl:variable name="segcount" select="count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithBookingClassReply/fareList/segmentInformation)" />
  <xsl:variable name="tktcount" select="count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithBookingClassReply/fareList)" />
  <xsl:variable name="count1" select="($segcount div $tktcount)" />
  <xsl:variable name="loop">
    <xsl:choose>
      <xsl:when test="PNR_RetrieveByRecLocReply/OTA_PNRRepriceRQ/FareNumber">
        <xsl:value-of select="'1'"/>
      </xsl:when>
      <xsl:when test="PNR_RetrieveByRecLocReply/Fare_PricePNRWithBookingClassReply/fareList/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
        <xsl:value-of select="(count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithBookingClassReply/fareList) div 2) + 1" />
      </xsl:when>
      <xsl:when test="PNR_RetrieveByRecLocReply/Fare_PricePNRWithLowerFaresReply/fareList/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
        <xsl:value-of select="(count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithLowerFaresReply/fareList) div 2) + 1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithBookingClassReply/fareList) + 1" />
        <xsl:value-of select="count(PNR_RetrieveByRecLocReply/Fare_PricePNRWithLowerFaresReply/fareList) + 1" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="PNR_RetrieveByRecLocReply" />
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply">
    <OTA_PNRRepriceRS>
      <xsl:attribute name="Version">1.0</xsl:attribute>
      <xsl:choose>
        <xsl:when test="Fare_PricePNRWithBookingClassReply/applicationError">
          <Errors>
            <Error>
              <xsl:attribute name="Type">Amadeus</xsl:attribute>
              <xsl:attribute name="Code">
                <xsl:value-of select="Fare_PricePNRWithBookingClassReply/applicationError/applicationErrorInfo/applicationErrorDetail/applicationErrorCode"/>
              </xsl:attribute>
              <xsl:value-of select="Fare_PricePNRWithBookingClassReply/applicationError/errorText/errorFreeText"/>
              <xsl:value-of select="Fare_PricePNRWithBookingClassReply/applicationError/errorWarningDescription/freeText"/>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:when test="Fare_PricePNRWithLowerFaresReply/applicationError">
          <Errors>
            <Error>
              <xsl:attribute name="Type">Amadeus</xsl:attribute>
              <xsl:attribute name="Code">
                <xsl:value-of select="Fare_PricePNRWithLowerFaresReply/applicationError/applicationErrorInfo/applicationErrorDetail/applicationErrorCode"/>
              </xsl:attribute>
              <xsl:value-of select="Fare_PricePNRWithLowerFaresReply/applicationError/errorText/errorFreeText"/>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:when test="not(Fare_PricePNRWithBookingClassReply) and not(Fare_PricePNRWithLowerFaresReply) and not(strResponseReprice)">
          <Errors>
            <Error>
              <xsl:attribute name="Type">Amadeus</xsl:attribute>
              <xsl:value-of select="'NO STORED FARE EXIST'"/>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:otherwise>
          <Success/>
          <xsl:apply-templates select="fareList[1]/warningInformation[warningCode/applicationErrorDetail/applicationErrorCode = 'BBY']" />
          <PricedItineraries>
            <xsl:apply-templates select="Ticket_DisplayTSTReply" mode="pnr"/>
            <xsl:apply-templates select="strResponseReprice/Ticket_DisplayTSTReply" mode="reprice"/>
            <xsl:apply-templates select="Fare_PricePNRWithBookingClassReply" mode="reprice"/>
            <xsl:apply-templates select="Fare_PricePNRWithLowerFaresReply" mode="reprice"/>
          </PricedItineraries>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:if test="ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_PNRRepriceRS>
  </xsl:template>

  <xsl:template match="Fare_PricePNRWithBookingClassReply | Ticket_DisplayTSTReply | Fare_PricePNRWithLowerFaresReply" mode="reprice">
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">2</xsl:attribute>
      <AirItineraryPricingInfo>
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/fcmi = 'F'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/fcmi = 'I'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/fcmi = 'M'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/fcmi = 'N'">Private</xsl:when>
            <xsl:when test="fareList[1]/pricingInformation/fcmi = 'R'">Private</xsl:when>
            <xsl:otherwise>Published</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="PricingOrigin">Repriced Fare</xsl:attribute>
        <ItinTotalFare>
          <xsl:variable name="bf">
            <xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalbase">
              <xsl:with-param name="sum">0</xsl:with-param>
              <xsl:with-param name="loop">
                <xsl:value-of select="$loop" />
              </xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:variable name="tf">
            <xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalprice">
              <xsl:with-param name="sum">0</xsl:with-param>
              <xsl:with-param name="loop">
                <xsl:value-of select="$loop" />
              </xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:variable name="curt">
            <xsl:choose>
              <xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
                <xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:variable name="dect">
            <xsl:choose>
              <xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
                <xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.'))" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$bf"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$curt" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:attribute name="Amount">
                <xsl:value-of select="$tf - $bf"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$curt" />
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </Tax>
          </Taxes>
          <xsl:variable name="qfees">
            <xsl:apply-templates select="fareList[1]/otherPricingInfo/attributeDetails[attributeType='FCA']" mode="qfees">
              <xsl:with-param name="sum">0</xsl:with-param>
              <xsl:with-param name="loop">
                <xsl:value-of select="$loop" />
              </xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:if test="$qfees>0">
            <Fees>
              <Fee>
                <xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="$curt" />
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect"/>
                </xsl:attribute>
              </Fee>
            </Fees>
          </xsl:if>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$tf"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$curt" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="fareList">
            <xsl:with-param name="msg" select="name()"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="Ticket_DisplayTSTReply" mode="pnr">
    <xsl:variable name="pos">
      <xsl:choose>
        <xsl:when test="../OTA_PNRRepriceRQ/FareNumber">
          <xsl:value-of select="../OTA_PNRRepriceRQ/FareNumber"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="fareList[1]/fareReference/uniqueReference"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <PricedItinerary>
      <xsl:attribute name="SequenceNumber">1</xsl:attribute>
      <AirItineraryPricingInfo>
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/fcmi = 'F'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/fcmi = 'I'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/fcmi = 'M'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/fcmi = 'N'">Private</xsl:when>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/pricingInformation/fcmi = 'R'">Private</xsl:when>
            <xsl:otherwise>Published</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="PricingOrigin">Stored Fare</xsl:attribute>
        <xsl:variable name="bf">
          <xsl:apply-templates select="fareList[fareReference/uniqueReference = $pos]/fareDataInformation" mode="totalbase">
            <xsl:with-param name="sum">0</xsl:with-param>
            <xsl:with-param name="loop">
              <xsl:value-of select="$loop" />
            </xsl:with-param>
            <xsl:with-param name="pos">1</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="tf">
          <xsl:apply-templates select="fareList[fareReference/uniqueReference = $pos]/fareDataInformation" mode="totalprice">
            <xsl:with-param name="sum">0</xsl:with-param>
            <xsl:with-param name="loop">
              <xsl:value-of select="$loop" />
            </xsl:with-param>
            <xsl:with-param name="pos">1</xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="curt">
          <xsl:choose>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="dect">
          <xsl:choose>
            <xsl:when test="fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="string-length(substring-after(fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.'))" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="string-length(substring-after(fareList[fareReference/uniqueReference = $pos]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <ItinTotalFare>
          <BaseFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="$bf != ''">
                  <xsl:value-of select="$bf"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$curt" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </BaseFare>
          <Taxes>
            <Tax>
              <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
              <xsl:attribute name="Amount">
                <xsl:choose>
                  <xsl:when test="$bf != ''">
                    <xsl:value-of select="$tf - $bf"/>
                  </xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$curt" />
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </Tax>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="$tf != ''">
                  <xsl:value-of select="$tf"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$curt" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="fareList">
            <xsl:with-param name="msg" select="name()"/>
          </xsl:apply-templates>
        </PTC_FareBreakdowns>
      </AirItineraryPricingInfo>
    </PricedItinerary>
  </xsl:template>

  <xsl:template match="fareDataInformation" mode="totalbase">
    <xsl:param name="sum" />
    <xsl:param name="loop" />
    <xsl:param name="pos" />
    <xsl:variable name="nopt">
      <xsl:value-of select="count(../paxSegReference/refDetails)" />
    </xsl:variable>
    <xsl:variable name="tot">
      <xsl:choose>
        <xsl:when test="fareDataSupInformation[fareDataQualifier = 'E']">
          <xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nopt" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nopt" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
        <xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalbase">
          <xsl:with-param name="sum">
            <xsl:value-of select="$tot + $sum" />
          </xsl:with-param>
          <xsl:with-param name="loop">
            <xsl:value-of select="$loop" />
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1" />
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tot + $sum" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="fareDataInformation" mode="totalprice">
    <xsl:param name="sum" />
    <xsl:param name="loop" />
    <xsl:param name="pos" />
    <xsl:variable name="nopt">
      <xsl:value-of select="count(../paxSegReference/refDetails)" />
    </xsl:variable>
    <xsl:variable name="tot">
      <xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nopt" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
        <xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalprice">
          <xsl:with-param name="sum">
            <xsl:value-of select="$tot + $sum" />
          </xsl:with-param>
          <xsl:with-param name="loop">
            <xsl:value-of select="$loop" />
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1" />
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tot + $sum" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="warningInformation">
    <Warnings>
      <Warning>
        <xsl:attribute name="Type">Amadeus</xsl:attribute>
        <xsl:value-of select="warningText/errorFreeText" />
      </Warning>
    </Warnings>
  </xsl:template>

  <xsl:template match="fareList" mode="paxtypes">
    <xsl:param name="pos"/>
    <xsl:variable name="nip">
      <xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Quantity" />
    </xsl:variable>
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Code" />
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Quantity" />
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:apply-templates select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]" mode="farebasis"/>
      </FareBasisCodes>
      <PassengerFare>
        <xsl:variable name="bfpax">
          <xsl:choose>
            <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','')" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','')" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="tfpax">
          <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','')" />
        </xsl:variable>
        <xsl:variable name="cur">
          <xsl:choose>
            <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="dec">
          <xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.'))"/>
        </xsl:variable>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$bfpax"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dec"/>
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <Tax>
            <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$tfpax - $bfpax"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="$cur" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dec"/>
            </xsl:attribute>
          </Tax>
          <xsl:apply-templates select="taxInformation">
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </Taxes>
        <xsl:variable name="qfees">
          <xsl:variable name="tot1">
            <xsl:choose>
              <xsl:when test="contains(otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription,' Q') and contains(substring-after(otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription,' Q'),'.')">
                <xsl:call-template name="qfeecalc">
                  <xsl:with-param name="fca">
                    <xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription"/>
                  </xsl:with-param>
                  <xsl:with-param name="qfeesum">0</xsl:with-param>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>0</xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:value-of select="$tot1 * $nip"/>
        </xsl:variable>
        <xsl:if test="$qfees>0">
          <Fees>
            <Fee>
              <xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$cur" />
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dec"/>
              </xsl:attribute>
            </Fee>
          </Fees>
        </xsl:if>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$tfpax"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dec"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
    </PTC_FareBreakdown>
  </xsl:template>

  <xsl:template match="taxInformation">
    <xsl:param name="nip"/>
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="taxDetails/taxType/isoCountry" />
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(amountDetails/fareDataMainInformation/fareAmount,'.','') * $nip" />
      </xsl:attribute>
    </Tax>
  </xsl:template>

  <xsl:template match="segmentInformation" mode="farebasis">
    <xsl:if test="(not(connexInformation/connecDetails/routingInformation) or connexInformation/connecDetails/routingInformation != 'ARNK') and fareQualifier/fareBasisDetails">
      <FareBasisCode>
        <xsl:value-of select="fareQualifier/fareBasisDetails/primaryCode" />
        <xsl:value-of select="fareQualifier/fareBasisDetails/fareBasisCode" />
        <xsl:if test="string-length(fareQualifier/fareBasisDetails/fareBasisCode)=6 and substring(fareQualifier/fareBasisDetails/fareBasisCode,6,1) = 'C' and fareQualifier/fareBasisDetails/discTktDesignator='CH'">
          <xsl:text>H</xsl:text>
        </xsl:if>
      </FareBasisCode>
    </xsl:if>
  </xsl:template>

  <xsl:template match="segmentInformation" mode="fareinfo">
    <FareInfo>
      <xsl:variable name="seg">
        <xsl:value-of select="segmentReference/refDetails/refNumber"/>
      </xsl:variable>
      <xsl:variable name="segment" select="../../PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $seg]"/>
      <DepartureDate>
        <xsl:value-of select="format-number(substring($segment/travelProduct/product/depDate,5,2),'2000')"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring($segment/travelProduct/product/depDate,3,2)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring($segment/travelProduct/product/depDate,1,2)"/>
        <xsl:text>T</xsl:text>
        <xsl:value-of select="format-number($segment/travelProduct/product/depTime,'00:00')"/>
        <xsl:text>:00</xsl:text>
      </DepartureDate>
      <FareReference>
        <xsl:value-of select="segDetails/segmentDetail/classOfService"/>
      </FareReference>
      <xsl:if test="../warningInformation/warningText/errorFreeText = 'NON-REFUNDABLE'">
        <RuleInfo>
          <ChargesRules>
            <VoluntaryChanges>
              <Penalty PenaltyType="Ticket Is Non Refundable"/>
            </VoluntaryChanges>
          </ChargesRules>
        </RuleInfo>
      </xsl:if>
      <FilingAirline>
        <xsl:value-of select="$segment/travelProduct/companyDetail/identification"/>
      </FilingAirline>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$segment/travelProduct/boardpointDetail/cityCode"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$segment/travelProduct/offpointDetail/cityCode"/>
        </xsl:attribute>
      </ArrivalAirport>
    </FareInfo>
  </xsl:template>

  <xsl:template match="attributeDetails" mode="qfees">
    <xsl:param name="sum" />
    <xsl:param name="loop" />
    <xsl:param name="pos" />
    <xsl:variable name="nopt">
      <xsl:value-of select="count(../../paxSegReference/refDetails)" />
    </xsl:variable>
    <xsl:variable name="tot1">
      <xsl:choose>
        <xsl:when test="contains(attributeDescription,' Q') and contains(substring-after(attributeDescription,' Q'),'.')">
          <xsl:call-template name="qfeecalc">
            <xsl:with-param name="fca">
              <xsl:value-of select="attributeDescription"/>
            </xsl:with-param>
            <xsl:with-param name="qfeesum">0</xsl:with-param>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="tot">
      <xsl:value-of select="$tot1 * $nopt"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="($pos &lt; $loop) and ../../../fareList[$pos + 1]">
        <xsl:apply-templates select="../../../fareList[$pos + 1]/otherPricingInfo/attributeDetails[attributeType='FCA']" mode="qfees">
          <xsl:with-param name="sum">
            <xsl:value-of select="$tot + $sum" />
          </xsl:with-param>
          <xsl:with-param name="loop">
            <xsl:value-of select="$loop" />
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1" />
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tot + $sum" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="qfeecalc">
    <xsl:param name="fca"/>
    <xsl:param name="qfeesum"/>
    <xsl:variable name="q1">
      <xsl:value-of select="substring-after($fca,' Q')"/>
    </xsl:variable>
    <xsl:if test="$q1!=''">
      <xsl:variable name="q2">
        <xsl:value-of select="substring-before($q1,'.')"/>
      </xsl:variable>
      <xsl:if test="$q2!=''">
        <xsl:variable name="q3">
          <xsl:value-of select="substring(substring-after($q1,'.'),1,2)"/>
        </xsl:variable>
        <xsl:if test="$q3!=''">
          <xsl:variable name="q4">
            <xsl:value-of select="$q2"/>
            <xsl:value-of select="$q3"/>
          </xsl:variable>
          <xsl:variable name="q5">
            <xsl:value-of select="translate($q4,'0123456789','')"/>
          </xsl:variable>
          <xsl:variable name="totqfee">
            <xsl:choose>
              <xsl:when test="$q5=''">
                <xsl:value-of select="$q2"/>.<xsl:value-of select="$q3"/>
              </xsl:when>
              <xsl:otherwise>0</xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:variable name="newfee">
            <xsl:value-of select="$qfeesum + $totqfee"/>
          </xsl:variable>
          <xsl:call-template name="qfeecalc">
            <xsl:with-param name="fca">
              <xsl:value-of select="$q1"/>
            </xsl:with-param>
            <xsl:with-param name="qfeesum">
              <xsl:value-of select="$newfee"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
      </xsl:if>
    </xsl:if>
    <xsl:if test="substring-after($fca,' Q') = ''">
      <xsl:value-of select="$qfeesum"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="fareList">
    <xsl:param name="msg"/>
    <PTC_FareBreakdown>
      <xsl:attribute name="RPH">
        <xsl:choose>
          <xsl:when test="$msg='Ticket_DisplayTSTReply'">
            <xsl:value-of select="fareReference/uniqueReference"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="position()"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="PricingSource">
        <!--xsl:choose>
				<xsl:when test="fareDataInformation/fareDataMainInformation/fareDataQualifier = 'H'">Private</xsl:when>
				<xsl:when test="fareDataInformation/fareDataMainInformation/fareDataQualifier = 'F'">Private</xsl:when>
				<xsl:when test="fareDataInformation/fareDataMainInformation/fareDataQualifier = 'BT'">Private</xsl:when>
				<xsl:otherwise>Published</xsl:otherwise>
			</xsl:choose-->
        <xsl:choose>
          <xsl:when test="pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
          <xsl:when test="pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when>
          <xsl:when test="pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = 'F'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = 'I'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = 'M'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = 'N'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = 'R'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = '7'">Private</xsl:when>
          <xsl:when test="pricingInformation/fcmi = '9'">Private</xsl:when>
          <xsl:otherwise>Published</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:for-each select="paxSegReference/refDetails">
          <xsl:variable name="paxref">
            <xsl:value-of select="refNumber"/>
          </xsl:variable>
          <xsl:if test="position() > 1">
            <xsl:text> </xsl:text>
          </xsl:if>
          <xsl:value-of select="../../../../travellerInfo[elementManagementPassenger/reference/number = $paxref]/elementManagementPassenger/lineNumber"/>
          <xsl:value-of select="../../../../../travellerInfo[elementManagementPassenger/reference/number = $paxref]/elementManagementPassenger/lineNumber"/>
        </xsl:for-each>
      </xsl:attribute>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]">
          <xsl:variable name="segref">
            <xsl:value-of select="segmentReference/refDetails/refNumber"/>
          </xsl:variable>
          <xsl:if test="position() > 1">
            <xsl:text> </xsl:text>
          </xsl:if>
          <xsl:value-of select="../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
          <xsl:value-of select="../../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
        </xsl:for-each>
      </xsl:attribute>
      <xsl:variable name="paxref">
        <xsl:value-of select="paxSegReference/refDetails[1]/refNumber"/>
      </xsl:variable>
      <xsl:variable name="paxtype">
        <xsl:choose>
          <xsl:when test="statusInformation/firstStatusDetails[1]/tstFlag='INF'">INF</xsl:when>
          <xsl:when test="paxSegReference/refDetails[1]/refQualifier='PI'">INF</xsl:when>
          <xsl:when test="count(../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData)>1">
            <xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData[1]/travellerInformation/passenger/type"/>
          </xsl:when>
          <xsl:when test="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type=''">ADT</xsl:when>
          <xsl:when test="count(../../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData)>1">
            <xsl:value-of select="../../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData[1]/travellerInformation/passenger/type"/>
          </xsl:when>
          <xsl:when test="../../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type=''">ADT</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type"/>
            <xsl:value-of select="../../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="paxcode">
        <xsl:choose>
          <xsl:when test="$paxtype = ''">ADT</xsl:when>
          <xsl:when test="$paxtype = 'CH'">CHD</xsl:when>
          <xsl:when test="$paxtype = 'INN'">CHD</xsl:when>
          <xsl:when test="$paxtype = 'CNN'">CHD</xsl:when>
          <xsl:when test="$paxtype = 'YCD'">SRC</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$paxtype"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="$paxcode"/>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="count(paxSegReference/refDetails)"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:if test="segmentInformation/fareQualifier/fareBasisDetails">
        <FareBasisCodes>
          <!--xsl:apply-templates select="segmentInformation[connexInformation/connecDetails/routingInformation != 'ARNK']"/-->
          <xsl:apply-templates select="segmentInformation" mode="farebasis"/>
        </FareBasisCodes>
      </xsl:if>
      <xsl:variable name="nip">
        <xsl:value-of select="count(paxSegReference/refDetails)"/>
      </xsl:variable>
      <PassengerFare>
        <xsl:variable name="bfpax">
          <xsl:choose>
            <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:choose>
                <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
                  <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="tfpax">
          <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','')"/>
        </xsl:variable>
        <xsl:variable name="feefpax">
          <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'TOF']/fareAmount,'.','')"/>
        </xsl:variable>
        <xsl:variable name="cur">
          <xsl:choose>
            <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="dec">
          <xsl:choose>
            <xsl:when test="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') > 0">
              <xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
            </xsl:otherwise>
          </xsl:choose>

        </xsl:variable>
        <BaseFare>
          <!--xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount,'.','')"/>
				</xsl:attribute-->
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="$bfpax != 'NaN'">
                <xsl:value-of select="$bfpax"/>
              </xsl:when>
              <xsl:otherwise>0</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:choose>
              <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
                <xsl:choose>
                  <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
                    <xsl:value-of select="$cur"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$cur"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:choose>
              <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
                <xsl:choose>
                  <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
                    <xsl:value-of select="$dec"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$dec"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </BaseFare>
        <xsl:if test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
          <xsl:variable name="efpax">
            <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip"/>
          </xsl:variable>
          <EquivFare>
            <xsl:attribute name="Amount">
              <xsl:choose>
                <xsl:when test="$efpax != 'NaN'">
                  <xsl:value-of select="$efpax"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
            </xsl:attribute>
          </EquivFare>
        </xsl:if>
        <Taxes>
          <xsl:apply-templates select="taxInformation">
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
            <xsl:with-param name="dec">
              <xsl:value-of select="$dec"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </Taxes>
        <xsl:if test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'TOF']">
          <Fees>
            <Fee>
              <xsl:attribute name="Amount">
                <xsl:choose>
                  <xsl:when test="$feefpax!= 'NaN'">
                    <xsl:value-of select="$feefpax"/>
                  </xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="$cur"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dec"/>
              </xsl:attribute>
            </Fee>
          </Fees>
        </xsl:if>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="$tfpax != 'NaN'">
                <xsl:value-of select="$tfpax"/>
              </xsl:when>
              <xsl:otherwise>0</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dec"/>
          </xsl:attribute>
          <!--xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount,'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute-->
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="otherPricingInfo/attributeDetails[attributeType='FCA']">
          <FareCalculation>
            <xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription"/>
          </FareCalculation>
        </xsl:if>
        <xsl:if test="otherPricingInfo/attributeDetails[attributeType='PAY'] or otherPricingInfo/attributeDetails[attributeType='TOU']">
          <PaymentRestrictions>
            <xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='PAY']/attributeDescription"/>
            <xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='TOU']/attributeDescription"/>
          </PaymentRestrictions>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="validatingCarrier/carrierInformation/carrierCode != ''">
            <ValidatingAirlineCode>
              <xsl:value-of select="validatingCarrier/carrierInformation/carrierCode"/>
            </ValidatingAirlineCode>
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="paxassoc">
              <xsl:for-each select="paxSegReference/refDetails">
                <xsl:value-of select="refNumber"/>
              </xsl:for-each>
            </xsl:variable>
            <xsl:variable name="segassoc">
              <xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation) or connexInformation/connecDetails/routingInformation != 'ARNK']">
                <xsl:value-of select="segmentReference/refDetails/refNumber"/>
              </xsl:for-each>
            </xsl:variable>
            <xsl:for-each select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV'] | ../../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV']">
              <xsl:variable name="paxfv">
                <xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
                  <xsl:value-of select="number"/>
                </xsl:for-each>
              </xsl:variable>
              <xsl:variable name="segfv">
                <xsl:for-each select="referenceForDataElement/reference[qualifier='ST']">
                  <xsl:value-of select="number"/>
                </xsl:for-each>
              </xsl:variable>
              <xsl:if test="$paxassoc = $paxfv and $segassoc = $segfv">
                <ValidatingAirlineCode>
                  <xsl:choose>
                    <xsl:when test="starts-with(otherDataFreetext/longFreetext,'PAX ')">
                      <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'PAX ')"/>
                    </xsl:when>
                    <xsl:when test="starts-with(otherDataFreetext/longFreetext,'INF ')">
                      <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'INF ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="otherDataFreetext/longFreetext"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ValidatingAirlineCode>
              </xsl:if>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]">
          <xsl:if test="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity or bagAllowanceInformation/bagAllowanceDetails/baggageWeight">
            <BagAllowance>
              <xsl:choose>
                <xsl:when test="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity!=''">
                  <xsl:attribute name="Quantity">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Piece</xsl:text>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Weight">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/baggageWeight"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Weight</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="Unit">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/measureUnit"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:attribute name="ItinSeqNumber">
                <xsl:variable name="segref">
                  <xsl:value-of select="segmentReference/refDetails/refNumber"/>
                </xsl:variable>
                <xsl:value-of select="../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
                <xsl:value-of select="../../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
              </xsl:attribute>
            </BagAllowance>
          </xsl:if>
        </xsl:for-each>
        <xsl:if test="../../OTA_AirRulesRS">
          <FareRules>
            <xsl:for-each select="../../OTA_AirRulesRS/FareRuleResponseInfo[FareRuleInfo/@PassengerType=$paxcode][1]/FareRules/SubSection[@SubCode='PE']/Paragraph">
              <Text>
                <xsl:value-of select="Text"/>
              </Text>
            </xsl:for-each>
          </FareRules>
        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>

</xsl:stylesheet>