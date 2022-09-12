<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Galileo_AirPriceRS.xsl 																-->
  <!-- ================================================================== -->
  <!-- Date: 27 Dec 2006 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="FareQuoteFlightSpecific_18" />
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="FareQuoteFlightSpecific_18">
    <OTA_AirPriceRS>
      <xsl:attribute name="Version">2003.2</xsl:attribute>
      <xsl:choose>
        <xsl:when test="FareInfo/RespHeader/ErrMsg='Y' or FareInfo/ErrText/Err!=''">
          <Errors>
            <xsl:choose>
              <xsl:when test="FareInfo/RespHeader/ErrMsg='Y'">
                <xsl:apply-templates select="FareInfo/InfoMsg" mode="error" />
              </xsl:when>
            </xsl:choose>
            <xsl:apply-templates select="FareInfo/ErrText" />
          </Errors>
        </xsl:when>
        <xsl:when test="FareInfo/RespHeader/CRTOutput='Y'">
          <Errors>
            <Error Type="Galileo">
              <xsl:value-of select="FareInfo/OutputMsg/Text" />
            </Error>
          </Errors>
        </xsl:when>
        <xsl:when test="TransactionErrorCode">
          <Errors>
            <Error Type="Galileo">
              <xsl:if test="AirAvail/ErrorCode != ''">
                <xsl:attribute name="Code">
                  <xsl:value-of select="AirAvail/ErrorCode" />
                </xsl:attribute>
              </xsl:if>
              <xsl:call-template name="errors">
                <xsl:with-param name="errorcode">
                  <xsl:value-of select="AirAvail/ErrorCode" />
                </xsl:with-param>
              </xsl:call-template>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:otherwise>
          <Success></Success>
          <PricedItineraries>
            <PricedItinerary>
              <xsl:attribute name="SequenceNumber">1</xsl:attribute>
              <!--AirItinerary>
								<OriginDestinationOptions>
									<xsl:apply-templates select="FareInfo/ItinSeg[1]" mode="OandDInfo"/>									
	         						</OriginDestinationOptions>
         						</AirItinerary-->
              <AirItineraryPricingInfo>
                <xsl:apply-templates select="FareInfo" mode="totalprice" />
                <!--xsl:apply-templates select="FareInfo" mode="faregroup"/-->
              </AirItineraryPricingInfo>
              <xsl:if test="FareInfo/GenQuoteDetails[1]/LastTkDt != ''">
                <TicketingInfo>
                  <xsl:if test="FareInfo/GenQuoteDetails[1]/LastTkDt != ''">
                    <xsl:attribute name="TicketTimeLimit">
                      <xsl:value-of select="substring(FareInfo/GenQuoteDetails[1]/LastTkDt,1,4)"/>
                      <xsl:text>-</xsl:text>
                      <xsl:value-of select="substring(FareInfo/GenQuoteDetails[1]/LastTkDt,5,2)"/>
                      <xsl:text>-</xsl:text>
                      <xsl:value-of select="substring(FareInfo/GenQuoteDetails[1]/LastTkDt,7,2)"/>
                      <xsl:text>T00:00:00</xsl:text>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:attribute name="TicketType">eTicket</xsl:attribute>
                </TicketingInfo>
              </xsl:if>
              <xsl:apply-templates select="InfoMsg" mode="info" />
            </PricedItinerary>
          </PricedItineraries>
        </xsl:otherwise>
      </xsl:choose>
    </OTA_AirPriceRS>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="ErrText">
    <Error Type="Gen">
      <xsl:attribute name="Code">
        <xsl:value-of select="Err" />
      </xsl:attribute>
      <xsl:value-of select="Text" />
    </Error>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="ItinSeg" mode="OandDInfo">
    <OriginDestinationOption>
      <xsl:apply-templates select="." mode="seg"/>
    </OriginDestinationOption>
    <xsl:apply-templates select="following-sibling::ItinSeg[NoStopAtBoardPt='Y'][1]" mode="nextOD"/>
  </xsl:template>

  <xsl:template match="ItinSeg" mode="nextOD">
    <xsl:apply-templates select="following-sibling::ItinSeg[1]" mode="OandDInfo"/>
  </xsl:template>

  <xsl:template match="ItinSeg" mode="seg">
    <FlightSegment>
      <xsl:variable name="segnum" select="position()"/>
      <xsl:variable name="DepTime">
        <xsl:value-of select="format-number(StartTm,'0000')"/>
      </xsl:variable>
      <xsl:attribute name="DepartureDateTime">
        <xsl:value-of select="substring(StartDt,1,4)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(StartDt,5,2)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(StartDt,7,2)"/>
        <xsl:text>T</xsl:text>
        <xsl:value-of select="substring($DepTime,1,2)"/>
        <xsl:text>:</xsl:text>
        <xsl:value-of select="substring($DepTime,3,2)"/>
        <xsl:text>:00</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="RPH">1</xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="format-number(FltNum,'0000')"/>
      </xsl:attribute>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:value-of select="BIC"/>
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="StartPt"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:choose>
            <xsl:when test="following-sibling::ItinSeg/AirV='' and following-sibling::ItinSeg/FltNum=''">
              <xsl:value-of select="following-sibling::ItinSeg/EndPt"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="EndPt"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </ArrivalAirport>
      <MarketingAirline>
        <xsl:value-of select="AirV"/>
      </MarketingAirline>
    </FlightSegment>
    <xsl:if test="NoStopAtBoardPt='N'">
      <xsl:apply-templates select="following-sibling::ItinSeg[AirV!='][FltNum!='][1]" mode="seg"/>
    </xsl:if>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="GenQuoteDetails" mode="travelergroup">
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="pos">
      <xsl:value-of select="position()" />
    </xsl:variable>
    <xsl:variable name="paxno">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="paxno1">
      <xsl:value-of select="UniqueKey"/>
    </xsl:variable>
    <PTC_FareBreakdown>
      <xsl:attribute name="PricingSource">
        <xsl:choose>
          <xsl:when test="PrivFQd='Y'">Private</xsl:when>
          <xsl:otherwise>Published</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:variable name="PsgrType">
            <xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/PICReq" />
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
            <xsl:otherwise>
              <xsl:value-of select="$PsgrType" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/PICPsgrs" />
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:apply-templates select="../SegRelatedInfo[UniqueKey=$paxno1]" mode="FIC" />
      </FareBasisCodes>
      <PassengerFare>
        <xsl:attribute name="NegotiatedFare">
          <xsl:choose>
            <xsl:when test="PrivFQd='Y'">1</xsl:when>
            <xsl:otherwise>0</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="EquivAmt != '' and EquivAmt != '0'">
                <xsl:value-of select="EquivAmt" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="BaseFareAmt" />
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
    </PTC_FareBreakdown>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Build Tax Data Group					 		   -->
  <!-- ************************************************************** -->
  <xsl:template match="TaxData" mode="tax">
    <xsl:variable name="tax" select="translate(string(Amt),'. ','')" />
    <Tax>
      <xsl:attribute name="TaxCode">
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
  <!--***********************************************************************************************************-->
  <xsl:template match="SegRelatedInfo" mode="FIC">
    <FareBasisCode>
      <xsl:value-of select="FIC" />
    </FareBasisCode>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="FareInfo" mode="totalprice">
    <xsl:variable name="pos">
      <xsl:value-of select="position()" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="GenQuoteDetails[1]/PrivFQd='Y'">
        <xsl:attribute name="PricingSource">Private</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="PricingSource">Published</xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
    <ItinTotalFare>
      <BaseFare>
        <xsl:attribute name="Amount">
          <xsl:variable name="amttot">
            <xsl:apply-templates select="GenQuoteDetails[1]" mode="basefare">
              <xsl:with-param name="total">0</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:value-of select="substring-before($amttot,'/')" />
        </xsl:attribute>
        <xsl:attribute name="CurrencyCode">
          <xsl:value-of select="GenQuoteDetails[1]/TotCurrency"/>
        </xsl:attribute>
        <xsl:attribute name="DecimalPlaces">
          <xsl:value-of select="GenQuoteDetails[1]/TotDecPos"/>
        </xsl:attribute>
      </BaseFare>
      <Taxes>
        <xsl:apply-templates select="GenQuoteDetails[1]" mode="TotalTax" />
      </Taxes>
      <xsl:if test="Surcharge">
        <Fees>
          <Fee>
            <xsl:attribute name="FeeCode">
              <xsl:text>Q-Surcharges</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="Amount">
              <xsl:variable name="amtfee1">
                <xsl:apply-templates select="Surcharge[1]" mode="totalfee">
                  <xsl:with-param name="total">0</xsl:with-param>
                </xsl:apply-templates>
              </xsl:variable>
              <xsl:value-of select="substring-before($amtfee1,'/')" />
            </xsl:attribute>
          </Fee>
        </Fees>
      </xsl:if>
      <TotalFare>
        <xsl:attribute name="Amount">
          <xsl:variable name="amttot1">
            <xsl:apply-templates select="GenQuoteDetails[1]" mode="totalfare">
              <xsl:with-param name="total">0</xsl:with-param>
            </xsl:apply-templates>
          </xsl:variable>
          <xsl:value-of select="substring-before($amttot1,'/')" />
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
      <xsl:apply-templates select="GenQuoteDetails" mode="travelergroup" />
    </PTC_FareBreakdowns>
    <FareInfos>
      <!--xsl:apply-templates select="RulesInfo" mode="FareInfo" /-->
      <!--xsl:apply-templates select="RsvnRules"/-->
      <xsl:apply-templates select="ItinSeg[AirV!=''][FltNum!='']" mode="FareInfo"/>
    </FareInfos>
    <!--xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:for-each select="GenQuoteDetails">
		<xsl:variable name="uk"><xsl:value-of select="UniqueKey"/></xsl:variable>
		!func:SetGlobal(tp_<xsl:value-of select="$pos"/>,<xsl:value-of select="TotAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs"/>,Add)
	</xsl:for-each>
	!func:SetGlobal(tp_<xsl:value-of select="$pos"/>,0)-->
  </xsl:template>
  <!-- ********************************************************************************************************-->
  <xsl:template match="RulesInfo" mode="FareInfo">
    <FareInfo>
      <FareReference>
        <xsl:value-of select="substring(string(FIC),1,1)" />
      </FareReference>
      <xsl:apply-templates select="../RsvnRules[1]" />
      <FilingAirline>
        <xsl:value-of select="AirV" />
      </FilingAirline>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="StartPt" />
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="EndPt" />
        </xsl:attribute>
      </ArrivalAirport>
    </FareInfo>
  </xsl:template>

  <xsl:template match="ItinSeg" mode="FareInfo">
    <FareInfo>
      <xsl:variable name="DepTime">
        <xsl:value-of select="format-number(StartTm,'0000')"/>
      </xsl:variable>
      <DepartureDate>
        <xsl:value-of select="substring(StartDt,1,4)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(StartDt,5,2)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(StartDt,7,2)"/>
        <xsl:text>T</xsl:text>
        <xsl:value-of select="substring($DepTime,1,2)"/>
        <xsl:text>:</xsl:text>
        <xsl:value-of select="substring($DepTime,3,2)"/>
        <xsl:text>:00</xsl:text>
      </DepartureDate>
      <FareReference>
        <xsl:value-of select="BIC" />
      </FareReference>
      <xsl:apply-templates select="../RsvnRules[1]" />
      <FilingAirline>
        <xsl:value-of select="AirV" />
      </FilingAirline>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="StartPt" />
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:choose>
            <xsl:when test="following-sibling::ItinSeg[1]/AirV='' and following-sibling::ItinSeg[1]/FltNum=''">
              <xsl:value-of select="following-sibling::ItinSeg[1]/EndPt" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="EndPt" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </ArrivalAirport>
    </FareInfo>
  </xsl:template>

  <!-- ********************************************************************************************************-->
  <xsl:template match="GenQuoteDetails" mode="basefare">
    <xsl:param name="total" />
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="uk">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:choose>
        <xsl:when test="EquivAmt != '' and EquivAmt != '0'">
          <xsl:value-of select="EquivAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="BaseFareAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
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
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="basefare2">
    <xsl:param name="total" />
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="uk">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="EquivAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
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
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="uk">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="TotAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
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
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="Surcharge" mode="totalfee">
    <xsl:param name="total" />
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="uk">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="Amt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
    </xsl:variable>
    <xsl:variable name="bigtotal">
      <xsl:value-of select="$total + $thistotal" />
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::Surcharge[1]" mode="totalfee">
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
    <xsl:variable name="zeros">0000</xsl:variable>
    <xsl:variable name="uk">
      <xsl:value-of select="concat(substring($zeros,1,4 - string-length(UniqueKey)),UniqueKey)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="TaxDataAry/TaxData[Country=$taxcode]/Amt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
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
  <!--***********************************************************************************************************-->
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
        1<xsl:value-of select="substring($addzeros,1,$numberdecimals)" />
      </xsl:variable>
      <xsl:variable name="SumOfTaxTwo">
        <xsl:value-of select="$SumOfTax * $subtract" />
      </xsl:variable>
      <xsl:variable name="NoDecimals">
        <xsl:value-of select="substring-before(string($SumOfTaxTwo),'.')" />
      </xsl:variable>
      <Tax>
        <xsl:attribute name="TaxCode">
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
          <xsl:value-of select="../../TotCurrency"/>
        </xsl:attribute>
        <xsl:attribute name="DecimalPlaces">
          <xsl:value-of select="../../TotDecPos"/>
        </xsl:attribute>
      </Tax>
    </xsl:for-each>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="BICInfoAry">
    <xsl:param name="segnum" />
    <xsl:param name="pbic" />
    <xsl:variable name="nbic">
      <xsl:for-each select="BICInfo">
        <xsl:variable name="bics">
          <xsl:value-of select="substring(string(.),2)" />
        </xsl:variable>
        <xsl:if test="substring($bics,$segnum,1)='Y'">
          <xsl:value-of select="BIC" />
        </xsl:if>
      </xsl:for-each>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$nbic=''">
        <xsl:value-of select="$pbic" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$nbic" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="RsvnRules">
    <!--FareInfo>
		<FareReference><xsl:value-of select="../SegRelatedInfo/FIC"/></FareReference-->
    <RuleInfo>
      <xsl:if test="AdvTkRsvnTm!='N' or AdvTkStartTm!='N'">
        <ResTicketingRules>
          <AdvResTicketing>
            <AdvReservation>
              <xsl:attribute name="LatestPeriod">
                <xsl:value-of select="AdvTkRsvnTm" />
              </xsl:attribute>
              <xsl:attribute name="LatestUnit">
                <xsl:choose>
                  <xsl:when test="AdvTkRsvnHrs='Y'">Hours</xsl:when>
                  <xsl:when test="AdvTkRsvnDays='Y'">Days</xsl:when>
                  <xsl:otherwise>Minutes</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </AdvReservation>
            <AdvTicketing>
              <xsl:attribute name="FromResPeriod">
                <xsl:value-of select="AdvTkStartTm" />
              </xsl:attribute>
              <xsl:attribute name="FromResUnit">
                <xsl:choose>
                  <xsl:when test="AdvTkRsvnHrs='Y'">Hours</xsl:when>
                  <xsl:when test="AdvTkRsvnDays='Y'">Days</xsl:when>
                  <xsl:otherwise>Minutes</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </AdvTicketing>
          </AdvResTicketing>
        </ResTicketingRules>
      </xsl:if>
      <xsl:if test="TmDOWMin!='0' or TmDOWMax!='0'">
        <LengthOfStayRules>
          <xsl:if test="TmDOWMin!='0'">
            <MinimumStay>
              <xsl:attribute name="MinStay">
                <xsl:choose>
                  <xsl:when test="TmDOWMin='1'">7</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="TmDOWMin - 1" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="StayUnit">
                <xsl:choose>
                  <xsl:when test="HoursMin='Y'">Hours</xsl:when>
                  <xsl:when test="DaysMin='Y'">Days</xsl:when>
                  <xsl:when test="MonthsMin='Y'">Months</xsl:when>
                  <xsl:otherwise>Minutes</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </MinimumStay>
          </xsl:if>
          <xsl:if test="TmDOWMax!='0'">
            <MaximumStay>
              <xsl:attribute name="MaxStay">
                <xsl:choose>
                  <xsl:when test="TmDOWMax='1'">7</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="TmDOWMax - 1" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="StayUnit">
                <xsl:choose>
                  <xsl:when test="HoursMax='Y'">Hours</xsl:when>
                  <xsl:when test="DaysMax='Y'">Days</xsl:when>
                  <xsl:when test="MonthsMax='Y'">Months</xsl:when>
                  <xsl:otherwise>Minutes</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </MaximumStay>
          </xsl:if>
        </LengthOfStayRules>
      </xsl:if>
      <ChargesRules>
        <VoluntaryChanges>
          <xsl:choose>
            <xsl:when test="count(PenFeeAry/PenFee)=1">
              <xsl:apply-templates select="PenFeeAry/PenFee[1]" mode="fee" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="PenFeeAry/PenFee[2]" mode="fee" />
            </xsl:otherwise>
          </xsl:choose>
        </VoluntaryChanges>
      </ChargesRules>
    </RuleInfo>
    <!--Note:  I don't think the position of the PenFee tag co-relates to the segment number in Galileo - this is why I only have one occurrence of FareInfo tag and rules under it - OTA does allow multiple FareInfo tags -->
    <!--FilingAirline><xsl:value-of select="../ItinSeg/AirV"/></FilingAirline>
		<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="../ItinSeg/StartPt"/></xsl:attribute>
		</DepartureAirport>	
		<ArrivalAirport>
			<xsl:attribute name="LocationCode"><xsl:value-of select="../ItinSeg/EndPt"/></xsl:attribute>
		</ArrivalAirport>
	</FareInfo-->
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="PenFee" mode="fee">
    <xsl:if test="ItinChg = 'Y' or Cancellation='Y' or FailConfirmSpace='Y' or ReplaceTk='Y' or TkNonRef = 'Y'">
      <Penalty>
        <xsl:variable name="Deci" select="substring-after(translate(Amt,' ',''),'.')" />
        <xsl:variable name="NoDeci" select="string-length($Deci)" />
        <xsl:if test="ItinChg = 'Y'">
          <xsl:attribute name="PenaltyType">ItinChange</xsl:attribute>
          <xsl:if test="Type='D'">
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(Amt),'. ','')" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Currency!=''">
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="Currency" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='D'">
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$NoDeci" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='P'">
            <xsl:attribute name="Percent">
              <xsl:value-of select="Amt" />
            </xsl:attribute>
          </xsl:if>
        </xsl:if>
        <xsl:if test="Cancellation = 'Y'">
          <xsl:attribute name="PenaltyType">Cancellation</xsl:attribute>
          <xsl:if test="Type='D'">
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(Amt),'. ','')" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Currency!=''">
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="Currency" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='D'">
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$NoDeci" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='P'">
            <xsl:attribute name="Percent">
              <xsl:value-of select="Amt" />
            </xsl:attribute>
          </xsl:if>
        </xsl:if>
        <xsl:if test="FailConfirmSpace = 'Y'">
          <xsl:attribute name="PenaltyType">FailConfirmSpace</xsl:attribute>
          <xsl:if test="Type='D'">
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(Amt),'. ','')" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Currency!=''">
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="Currency" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='D'">
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$NoDeci" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='P'">
            <xsl:attribute name="Percent">
              <xsl:value-of select="Amt" />
            </xsl:attribute>
          </xsl:if>
        </xsl:if>
        <xsl:if test="ReplaceTk = 'Y'">
          <xsl:attribute name="PenaltyType">ReplaceTicket</xsl:attribute>
          <xsl:if test="Type='D'">
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(Amt),'. ','')" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Currency!=''">
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="Currency" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='D'">
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$NoDeci" />
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="Type='P'">
            <xsl:attribute name="Percent">
              <xsl:value-of select="Amt" />
            </xsl:attribute>
          </xsl:if>
        </xsl:if>
        <xsl:if test="TkNonRef = 'Y'">
          <xsl:attribute name="PenaltyType">Ticket Is Non Refundable</xsl:attribute>
          <xsl:choose>
            <xsl:when test="Type = 'D'">
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(Amt),'. ','')" />
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$NoDeci" />
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:if test="Type!=''">
                <xsl:attribute name="Percent">
                  <xsl:value-of select="Amt" />
                </xsl:attribute>
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </Penalty>
    </xsl:if>
  </xsl:template>
  <!--***********************************************************************************************************-->
  <xsl:template match="InfoMsg" mode="info">
    <xsl:if test="not(contains(Text, 'LAST DATE TO PURCHASE TICKET'))">
      <Notes>
        <xsl:value-of select="Text" />
      </Notes>
    </xsl:if>
  </xsl:template>
  <xsl:template match="InfoMsg" mode="error">
    <Error Type="Galileo" Code="{MsgNum}">
      <xsl:value-of select="Text" />
    </Error>
  </xsl:template>
  <xsl:template name="errors">
    <xsl:param name="errorcode" />
    <xsl:choose>
      <xsl:when test="$errorcode = '0002'">
        <xsl:text>CHECK NUMBER IN PARTY</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0003'">
        <xsl:text>INVALID CLASS OF SERVICE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0004'">
        <xsl:text>INVALID DATE FORMAT</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '	0005'">
        <xsl:text>INVALID DEPARTURE CODE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '	0006'">
        <xsl:text>INVALID ARRIVAL CODE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '	0007'">
        <xsl:text>INVALID TIME FORMAT</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '	0008'">
        <xsl:text>INVALID TIME MODIFIER</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '	0011'">
        <xsl:text>MAXIMUM AIRLINE PREFERENCE EXCEEDED</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0012'">
        <xsl:text>INVALID AIRLINE PREFERENCE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0016'">
        <xsl:text>INVALID FLIGHT NUMBER</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0017'">
        <xsl:text>INVALID CONNECTION INDICATOR</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0019'">
        <xsl:text>GENERAL SYSTEM ERROR</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0021'">
        <xsl:text>NO DISPLAYABLE FLIGHTS</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0022'">
        <xsl:text>INVALID CITY PAIR DATA</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0023'">
        <xsl:text>INVALID AIRLINE CODE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0024'">
        <xsl:text>SPECIFIC AVAILABILITY FOR AIRLINE NOT FOUND</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0025'">
        <xsl:text>DATE OUTSIDE RANGE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0027'">
        <xsl:text>INVALID CITY PAIR DATA</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0031'">
        <xsl:text>SPECIFIC FLIGHT NOT FOUND</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0033'">
        <xsl:text>LEGS ARE NOT CONTINUOUS</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0034'">
        <xsl:text>CHECK NUMBER IN PARTY</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0035'">
        <xsl:text>INVALID CLASS OF SERVICE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0040'">
        <xsl:text>AIRLINE NOT AVAILABLE</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0041'">
        <xsl:text>INVALID FLIGHT NUMBER</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0043'">
        <xsl:text>FLIGHT NOT OPERATING</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0044'">
        <xsl:text>INVALID HOURS BEFORE IN TIME WINDOW </xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0045'">
        <xsl:text>INVALID HOURS AFTER IN TIME WINDOW </xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0046'">
        <xsl:text>INVALID JOURNEY TOTAL HOURS</xsl:text>
      </xsl:when>
      <xsl:when test="$errorcode = '0049'">
        <xsl:text>INVALID INPUT DATA</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>INVALID INPUT DATA</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--***********************************************************************************************************-->
</xsl:stylesheet>
