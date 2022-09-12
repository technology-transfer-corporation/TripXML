<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
  <!-- 
  ================================================================== 
	Galileo_PNRReadRQ.xsl 															
	================================================================== 
  Date: 14 Sep 2021 - Kobelev - Intermidean Errors during Price Search.
  Date: 01 Mar 2021 - Kobelev - ARNK Segment handling.
  Date: 22 Feb 2021 - Kobelev - In case of Check include CheckFOP element
  Date: 05 Jan 2021 - Kobelev - PFQual included only if FareType is not set to "N" (Case Number: CS0653990)
  Date: 03 Apr 2020 - Kobelev - Finilizing format of different requests 
  Date: 18 Feb 2020 - Kobelev - New version of PNR RePrice 
	================================================================== 
  HELP: https://support.travelport.com/webhelp/GWS/Content/TRANSACTIONHELP/PNRBFManagement_53/PNRBFManagement_53_request.html
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="pqs" select="OTA_PNRRepriceRQ/StoredFare/@RPH" />

  <xsl:template match="/">

    <xsl:apply-templates select="OTA_PNRRepriceRQ"/>
    <xsl:choose>
      <xsl:when test="PNRBFManagement_53/OTA_PNRRepriceRQ/@StoreFare = 'true'">
        <xsl:apply-templates select="PNRBFManagement_53" mode="store" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="PNRBFManagement_53" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Read 
  ===================================================================
  -->
  <xsl:template match="OTA_PNRRepriceRQ">
    <PNRBFManagement_53>
      <PNRBFRetrieveMods>
		  <xsl:choose>
			  <xsl:when test="ConversationID!=''">
				  <CurrentPNR />
			  </xsl:when>
			  <xsl:otherwise>
				  <PNRAddr>
					  <FileAddr />
					  <CodeCheck />
					  <RecLoc>
						  <xsl:value-of select="UniqueID/@ID" />
					  </RecLoc>
				  </PNRAddr>
			  </xsl:otherwise>
		  </xsl:choose>
      </PNRBFRetrieveMods>
      <xsl:if test="$pqs">
        <xsl:for-each select="$pqs">
          <xsl:if test="generate-id() = generate-id($pqs[. = current()][1])">
            <FareRedisplayMods>
              <DisplayAction>
                <Action>D</Action>
              </DisplayAction>
              <FareNumInfo>
                <FareNumAry>
                  <xsl:if test="generate-id() = generate-id($pqs[. = current()][1])">
                    <FareNum>
                      <xsl:value-of select="."/>
                    </FareNum>
                  </xsl:if>
                </FareNumAry>
              </FareNumInfo>
            </FareRedisplayMods>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>
    </PNRBFManagement_53>
  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Price Qoute Request 
  ===================================================================
  -->
  <xsl:template match="PNRBFManagement_53">
    <xsl:if test="count(DocProdDisplayStoredQuote[ErrText]) > 0">
      <Errors>
        <xsl:for-each select="DocProdDisplayStoredQuote[ErrText]">
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:value-of select="ErrText/Text" />
          </Error>
        </xsl:for-each>
      </Errors>
    </xsl:if>
    <xsl:if test="count(DocProdDisplayStoredQuote[ErrText]) = 0">
      <FareQuoteClassSpecific_35>
        <xsl:apply-templates select="DocProdDisplayStoredQuote[not(ErrText)]" mode="compare" />
      </FareQuoteClassSpecific_35>
    </xsl:if>
  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Price Qoute And Store Request 
  ===================================================================
  -->
  <xsl:template match="PNRBFManagement_53" mode="store">

    <PNRBFManagement_53>
      <CancelStoredFareMods>
        <FareNumInfo>
          <FareNumAry>
            <xsl:for-each select="DocProdDisplayStoredQuote[not(ErrText)]">
              <FareNum>
                <xsl:value-of select="FareNumInfo/FareNumAry/FareNum"/>
              </FareNum>
            </xsl:for-each>
          </FareNumAry>
        </FareNumInfo>
      </CancelStoredFareMods>

      <!--
      <xsl:if test="OTA_PNRRepriceRQ/StoredFare/Markup">
      -->

      <xsl:if test="OTA_PNRRepriceRQ">
        <xsl:choose>
          <xsl:when test="count(OTA_PNRRepriceRQ/StoredFare[not(@RPH=preceding-sibling::StoredFare/@RPH)]) > 1">
            <xsl:apply-templates select="OTA_PNRRepriceRQ/StoredFare" mode="store" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="DocProdDisplayStoredQuote[not(ErrText)]" mode="store">
              <xsl:with-param name="fareType">
                <xsl:value-of select="OTA_PNRRepriceRQ/StoredFare[1]/@FareType" />
              </xsl:with-param>
              <xsl:with-param name="ptc">
                <xsl:value-of select="OTA_PNRRepriceRQ/StoredFare[1]/PassengerType/@Code" />
              </xsl:with-param>
            </xsl:apply-templates>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>

      <!--      -->
      <EndTransactionMods>
        <EndTransactRequest>
          <ETInd>R</ETInd>
          <RcvdFrom>TRIPXML</RcvdFrom>
        </EndTransactRequest>
      </EndTransactionMods>
    </PNRBFManagement_53>

  </xsl:template>

  <xsl:template match ="DocProdDisplayStoredQuote" mode="docs">
    <DocProdFareManipulation_29>
      <!--below given entire TicketingMods block was commented in local xsl-->
      <ManualFareUpdateSaveMods>
        <FareNumInfo>
          <FareNumAry>
            <xsl:for-each select="DocProdDisplayStoredQuote[not(ErrText)]">
              <FareNum>
                <xsl:value-of select="FareNumInfo/FareNumAry/FareNum"/>
              </FareNum>
            </xsl:for-each>
          </FareNumAry>
        </FareNumInfo>
        <InfoMsg>
          <UniqueKey>0001</UniqueKey>
          <QuoteNum>1</QuoteNum>
          <MsgNum>0</MsgNum>
          <AppNum>0</AppNum>
          <MsgType>1</MsgType>
          <Lang>0</Lang>
          <Text>NONREF/PENALTY APPLIES</Text>
        </InfoMsg>
      </ManualFareUpdateSaveMods>

      <!--==================================================================================-->
    </DocProdFareManipulation_29>
  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Store Price by PTC type 
  ===================================================================
  -->
  <xsl:template match="StoredFare" mode="store">
    <xsl:variable name="fareNum">
      <xsl:value-of select="@RPH"/>
    </xsl:variable>

    <!--    
    <xsl:apply-templates select="../../DocProdDisplayStoredQuote[not(ErrText) and FareNumInfo/FareNumAry/FareNum=$fareNum]" mode="store" >
      <xsl:with-param name="fareType">
        <xsl:value-of select="@FareType" />
      </xsl:with-param>
      <xsl:with-param name="ptc">
        <xsl:value-of select="PassengerType/@Code" />
      </xsl:with-param>
    </xsl:apply-templates>
-->

    <xsl:apply-templates select="../../DocProdDisplayStoredQuote[not(ErrText)]/GenQuoteDetails[UniqueKey=$fareNum]" mode="store" >
      <xsl:with-param name="fareType">
        <xsl:value-of select="@FareType" />
      </xsl:with-param>
      <xsl:with-param name="ptc">
        <xsl:value-of select="PassengerType/@Code" />
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Store Price for pax 
  ===================================================================
  -->
  <xsl:template match="DocProdDisplayStoredQuote" mode="compare">

    <xsl:variable name="fareNum" select="format-number(AgntEnteredPsgrDescInfo/UniqueKey, '0')" />

    <ClassSpecificMods>
      <SegInfo>
        <FltSegAry>
          <xsl:apply-templates select="../PNRBFRetrieve/AirSeg" />
        </FltSegAry>
      </SegInfo>

      <PassengerType>
        <PsgrAry>
          <xsl:apply-templates select="." />
        </PsgrAry>
      </PassengerType>

      <xsl:if test="OutPvtFareInfo">
        <xsl:variable name="fareType">
          <xsl:choose >
            <xsl:when test="GenQuoteDetails[1]/PrivFQd = 'Y'">
              <xsl:text>Private</xsl:text>
            </xsl:when>
            <xsl:otherwise>Published</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <SegSelection>
          <ReqAirVPFs>Y</ReqAirVPFs>
          <SegRangeAry>
            <xsl:variable name="qN" select="GenQuoteDetails[1]/QuoteNum"/>
            <xsl:variable name="uqN" select="GenQuoteDetails[1]/UniqueKey"/>
            <xsl:variable name="segs" select="PNRFareDetail[UniqueKey=$uqN]/SegNum"/>
            <xsl:variable name="fic" select="PNRFareDetail[UniqueKey=$uqN]/FIC"/>
            <SegRange>
              <xsl:choose>
                <xsl:when test="PNRBFRetrieve/ARNK">
                  <StartSeg>
                    <xsl:number value="00"/>
                  </StartSeg>
                  <EndSeg>
                    <xsl:number value="00"/>
                  </EndSeg>
                </xsl:when>
                <xsl:otherwise>
                  <StartSeg>
                    <xsl:number value="$segs[1]" format="01"/>
                  </StartSeg>
                  <EndSeg>
                    <xsl:number value="$segs[last()]" format="01"/>
                  </EndSeg>
                </xsl:otherwise>
              </xsl:choose>

              <xsl:variable name="ft">
                <xsl:choose>
                  <xsl:when test="$fareType='Published'">
                    <xsl:text>N</xsl:text>
                  </xsl:when>
                  <xsl:when test="$fareType='Private'">
                    <xsl:text>P</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>B</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <FareType>
                <xsl:value-of select="$ft" />
              </FareType>

              <xsl:if test="$ft='B' or $ft='F'">
                <FIC>
                  <xsl:value-of select="$fic[1]"/>
                </FIC>
              </xsl:if>

              <xsl:if test="$ft!='N'">
                <PFQual>
                  <CRSInd>1G</CRSInd>
                  <PCC>
                    <xsl:value-of select="ExtendedQuoteInformation[QuoteNum=$qN and UniqueKey=$uqN]/TkPCC[1]"/>
                  </PCC>
                  <AirV>
                    <xsl:value-of select="PlatingAirVMod/AirV"/>
                  </AirV>
                  <Acct/>
                  <Contract/>

                  <xsl:choose>
                    <xsl:when test="$fareType='Private'">
                      <PublishedFaresInd>N</PublishedFaresInd>
                    </xsl:when>
                    <xsl:otherwise>
                      <PublishedFaresInd>Y</PublishedFaresInd>
                    </xsl:otherwise>
                  </xsl:choose>
                  <Type>A</Type>
                </PFQual>
              </xsl:if>

            </SegRange>
          </SegRangeAry>
        </SegSelection>

      </xsl:if>


      <xsl:variable name="ptc" select="."/>

      <xsl:if test="../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares">
        <xsl:variable name="ffs" select="../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares/FareFamily" />
        <xsl:for-each select="$ffs">
          <xsl:if test="generate-id() = generate-id($ffs[. = current()][1])">
            <xsl:variable name="ffCode" select="./@Code" />
            <PriceByBrand>
              <PricebyBrandModifier>
                <xsl:number value="$ffCode" format="0001"/>
              </PricebyBrandModifier>
            </PriceByBrand>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>

      <xsl:if test="../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum]/BrandedFares">
        <GenQuoteInfo>
          <SuppressBrandedFares>N</SuppressBrandedFares>
          <RBDInd>M</RBDInd>
        </GenQuoteInfo>
      </xsl:if>

    </ClassSpecificMods>

  </xsl:template>

  <!--
  ===================================================================
  Prepare PNR Store Price for pax 
  ===================================================================
  -->
  <xsl:template match="GenQuoteDetails" mode="store">
    <xsl:param name="fareType" />
    <xsl:param name="ptc" />

    <xsl:variable name="ptc2">
      <xsl:choose>
        <xsl:when test="$ptc='CHD'">
          <xsl:value-of select="../AgntEnteredPsgrDescInfo[substring(AgntEnteredPsgrDesc, 1,1)='C']/AgntEnteredPsgrDesc"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ptc"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="fareNum">
      <xsl:value-of select="UniqueKey"/>
    </xsl:variable>

    <xsl:variable name="segs" select="../PNRFareDetail[UniqueKey=$fareNum]/SegNum"/>
    <xsl:variable name="fic" select="../SegRelatedInfo[UniqueKey=$fareNum]/FIC"/>

    <StorePriceMods>
      <xsl:variable name="qN" select="GenQuoteDetails[UniqueKey=$fareNum]/QuoteNum[1]"/>

      <PassengerType>
        <PsgrAry>
          <xsl:apply-templates select="../AgntEnteredPsgrDescInfo[AgntEnteredPsgrDesc=$ptc2]" >
            <xsl:with-param name="fareNum">
              <xsl:value-of select="$fareNum" />
            </xsl:with-param>
          </xsl:apply-templates>

        </PsgrAry>
      </PassengerType>

      <PlatingAirVMod>
        <AirV>
          <xsl:value-of select="../PlatingAirVMod/AirV"/>
        </AirV>
      </PlatingAirVMod>

      <SegSelection>
        <ReqAirVPFs>Y</ReqAirVPFs>
        <SegRangeAry>
          <SegRange>
            <xsl:choose>
              <xsl:when test="../../PNRBFRetrieve/ARNK">
                <StartSeg>
                  <xsl:number value="00"/>
                </StartSeg>
                <EndSeg>
                  <xsl:number value="00"/>
                </EndSeg>
              </xsl:when>
              <xsl:otherwise>
                <StartSeg>
                  <xsl:number value="$segs[1]" format="01"/>
                </StartSeg>
                <EndSeg>
                  <xsl:number value="$segs[last()]" format="01"/>
                </EndSeg>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:variable name="ft">
              <xsl:choose>
                <xsl:when test="$fareType='Published'">
                  <xsl:text>N</xsl:text>
                </xsl:when>
                <xsl:when test="$fareType='Private'">
                  <xsl:text>P</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:text>B</xsl:text>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <FareType>
              <xsl:value-of select="$ft" />
            </FareType>

            <xsl:if test="$ft='B' or $ft='F'">
              <FIC>
                <xsl:value-of select="$fic[1]"/>
              </FIC>
            </xsl:if>

            <xsl:if test="$ft!='N'">
              <PFQual>
                <CRSInd>1G</CRSInd>
                <PCC>
                  <xsl:value-of select="../ExtendedQuoteInformation[UniqueKey=$fareNum]/TkPCC[1]"/>
                </PCC>
                <AirV>
                  <xsl:value-of select="../PlatingAirVMod/AirV"/>
                </AirV>
                <Acct/>
                <Contract/>
                <xsl:choose>
                  <xsl:when test="$fareType='Private'">
                    <PublishedFaresInd>N</PublishedFaresInd>
                  </xsl:when>
                  <xsl:otherwise>
                    <PublishedFaresInd>Y</PublishedFaresInd>
                  </xsl:otherwise>
                </xsl:choose>
                <Type>A</Type>
              </PFQual>
            </xsl:if>
          </SegRange>
        </SegRangeAry>
      </SegSelection>

      <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare/@FareType='Private'">
        <BulkTicket/>
      </xsl:if>

      <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TourCode">
        <xsl:variable name="tc" select="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TourCode" />
        <TourCode>
          <Rules>
            <xsl:value-of select="$tc"/>
          </Rules>
        </TourCode>
      </xsl:if>

      <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/Endorsement">

        <xsl:variable name="endo" select="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/Endorsement" />
        <xsl:choose>
          <xsl:when test="string-length($endo) > 29">
            <EndorsementBox>
              <xsl:call-template name="multyEndorsmats">
                <xsl:with-param name="endo" select="$endo" />
              </xsl:call-template>
            </EndorsementBox>
          </xsl:when>
          <xsl:when test="contains($endo, '/')">
            <EndorsementBox>
              <xsl:call-template name="multyEndorsmats">
                <xsl:with-param name="endo" select="$endo" />
              </xsl:call-template>
            </EndorsementBox>
          </xsl:when>
          <xsl:otherwise>
            <EndorsementBox>
              <Endors1>
                <xsl:value-of select="$endo"/>
              </Endors1>
            </EndorsementBox>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:if>

      <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare/@FareType!='Private'">
        <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code = $ptc]/Markup">
          <CommissionMod>
            <xsl:choose>
              <xsl:when test="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code = $ptc]/Markup/@Amount">
                <Amt>
                  <xsl:value-of select="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code = $ptc]/Markup/@Amount"/>
                </Amt>
              </xsl:when>
              <xsl:otherwise>
                <Percent>
                  <xsl:value-of select="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code = $ptc]/Markup/@Percent"/>
                </Percent>
              </xsl:otherwise>
            </xsl:choose>
          </CommissionMod>
        </xsl:if>
      </xsl:if>

      <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares">
        <xsl:variable name="ffs" select="../../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares/FareFamily/@Code" />
        <xsl:for-each select="$ffs">
          <xsl:if test="generate-id() = generate-id($ffs[. = current()][1])">

            <PriceByBrand>
              <PricebyBrandModifier>
                <xsl:number value="$ffs" format="0001"/>
              </PricebyBrandModifier>
            </PriceByBrand>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>

    </StorePriceMods>

  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="store">
    <xsl:param name="fareType" />
    <xsl:param name="ptc" />

    <xsl:variable name="ptc2">
      <xsl:choose>
        <xsl:when test="$ptc='CHD'">
          <xsl:value-of select="../AgntEnteredPsgrDescInfo[substring(AgntEnteredPsgrDesc, 1,1)='C']/AgntEnteredPsgrDesc"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ptc"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="fareNum" select="format-number(AgntEnteredPsgrDescInfo[AgntEnteredPsgrDesc=$ptc]/UniqueKey, '0')" />
    <xsl:variable name="segs" select="PNRFareDetail[UniqueKey=$fareNum]/SegNum"/>
    <xsl:variable name="fic" select="SegRelatedInfo[UniqueKey=$fareNum]/FIC"/>

    <StorePriceMods>
      <SegSelection>
        <ReqAirVPFs>Y</ReqAirVPFs>
        <SegRangeAry>
          <xsl:variable name="qN" select="GenQuoteDetails[UniqueKey=$fareNum]/QuoteNum"/>

          <SegRange>

            <xsl:choose>
              <xsl:when test="PNRBFRetrieve/ARNK">
                <StartSeg>
                  <xsl:number value="00"/>
                </StartSeg>
                <EndSeg>
                  <xsl:number value="00"/>
                </EndSeg>
              </xsl:when>
              <xsl:otherwise>
                <StartSeg>
                  <xsl:number value="$segs[1]" format="01"/>
                </StartSeg>
                <EndSeg>
                  <xsl:number value="$segs[last()]" format="01"/>
                </EndSeg>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="ft">
              <xsl:choose>
                <xsl:when test="$fareType='Published'">
                  <xsl:text>N</xsl:text>
                </xsl:when>
                <xsl:when test="$fareType='Private'">
                  <xsl:text>P</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:text>B</xsl:text>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <FareType>
              <xsl:value-of select="$ft" />
            </FareType>

            <xsl:if test="$ft='B' or $ft='F'">
              <FIC>
                <xsl:value-of select="$fic[1]"/>
              </FIC>
            </xsl:if>

            <xsl:if test="$ft!='N'">
              <PFQual>
                <CRSInd>1G</CRSInd>
                <PCC>
                  <xsl:value-of select="ExtendedQuoteInformation[QuoteNum=$qN and UniqueKey=$fareNum]/TkPCC[1]"/>
                </PCC>
                <AirV>
                  <xsl:value-of select="PlatingAirVMod/AirV"/>
                </AirV>
                <Acct/>
                <Contract/>
                <xsl:choose>
                  <xsl:when test="$fareType='Private'">
                    <PublishedFaresInd>N</PublishedFaresInd>
                  </xsl:when>
                  <xsl:otherwise>
                    <PublishedFaresInd>Y</PublishedFaresInd>
                  </xsl:otherwise>
                </xsl:choose>
                <Type>A</Type>
              </PFQual>
            </xsl:if>
          </SegRange>
        </SegRangeAry>
      </SegSelection>

      <PassengerType>
        <PsgrAry>
          <xsl:apply-templates select="." />
        </PsgrAry>
      </PassengerType>

      <xsl:if test="../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TourCode">
        <xsl:variable name="tc" select="../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TourCode" />
        <TourCode>
          <Rules>
            <xsl:value-of select="$tc"/>
          </Rules>
        </TourCode>
      </xsl:if>

      <xsl:if test="../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/Endorsement">
        <xsl:variable name="endo" select="../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/Endorsement" />
        <xsl:choose>
          <xsl:when test="string-length($endo) > 29">
            <EndorsementBox>
              <xsl:call-template name="multyEndorsmats">
                <xsl:with-param name="endo" select="$endo" />
              </xsl:call-template>
            </EndorsementBox>
          </xsl:when>
          <xsl:when test="contains($endo, '/')">
            <EndorsementBox>
              <xsl:call-template name="multyEndorsmats">
                <xsl:with-param name="endo" select="$endo" />
              </xsl:call-template>
            </EndorsementBox>
          </xsl:when>
          <xsl:otherwise>
            <EndorsementBox>
              <Endors1>
                <xsl:value-of select="$endo"/>
              </Endors1>
            </EndorsementBox>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>

      <!--      -->
      <xsl:if test="../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares">
        <xsl:variable name="ffs" select="../OTA_PNRRepriceRQ/StoredFare[@RPH=$fareNum and PassengerType/@Code=$ptc]/BrandedFares/FareFamily" />
        <xsl:for-each select="$ffs">
          <xsl:if test="generate-id() = generate-id($ffs[. = current()][1])">
            <xsl:variable name="ffCode" select="./@Code" />
            <PriceByBrand>
              <PricebyBrandModifier>
                <xsl:number value="$ffCode" format="0001"/>
              </PricebyBrandModifier>
            </PriceByBrand>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>

      <PlatingAirVMod>
        <AirV>
          <xsl:value-of select="PlatingAirVMod/AirV"/>
        </AirV>
      </PlatingAirVMod>
    </StorePriceMods>

  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote">
    <xsl:choose>
      <xsl:when test="AssocPsgrs">
        <xsl:for-each select="AssocPsgrs[PsgrAry != '']">

          <xsl:variable name="abs">
            <xsl:value-of select="PsgrAry/Psgr/AbsNameNum"/>
          </xsl:variable>

          <xsl:variable name="ptc">
            <xsl:value-of select="../AgntEnteredPsgrDescInfo[ApplesToAry/AppliesTo/AbsNameNum = $abs]/AgntEnteredPsgrDesc"/>
          </xsl:variable>

          <Psgr>
            <LNameNum>
              <xsl:choose>
                <xsl:when test="../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]">
                  <xsl:value-of select="../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]/LNameNum"/>
                </xsl:when>
                <xsl:when test="../../PNRBFRetrieve/LNameInfo[1]">
                  <xsl:value-of select="../../PNRBFRetrieve/LNameInfo[1]/LNameNum"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="PsgrAry/Psgr/LNameNum"/>
                </xsl:otherwise>
              </xsl:choose>
            </LNameNum>
            <PsgrNum>
              <xsl:value-of select="../../PNRBFRetrieve/FNameInfo[AbsNameNum=$abs]/PsgrNum"/>
            </PsgrNum>
            <AbsNameNum>
              <xsl:value-of select="$abs"/>
            </AbsNameNum>
            <PTC>
              <xsl:value-of select="$ptc"/>
            </PTC>
            <TIC/>

            <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator">
              <xsl:variable name="tktDesg" select="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator" />
              <TkDesignator>
                <xsl:value-of select="$tktDesg"/>
              </TkDesignator>
            </xsl:if>

            <xsl:if test="../../OTA_PNRRepriceRQ/@StoreFare = 'true'">
              <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup">
                <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare/@FareType='Private'">
                  <DiscOrIncrInd>
                    <xsl:choose>
                      <xsl:when test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                        <xsl:text>IF</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>IP</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </DiscOrIncrInd>
                  <AmtOrPercent>
                    <xsl:choose>
                      <xsl:when test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                        <xsl:number value="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount" format="000000001"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:number value="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Percent" format="000000001"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </AmtOrPercent>
                </xsl:if>
              </xsl:if>
            </xsl:if>

          </Psgr>
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="AgntEnteredPsgrDescInfo">
        <xsl:for-each select="AgntEnteredPsgrDescInfo[ApplesToAry != '']">
          <xsl:variable name="abs">
            <xsl:value-of select="ApplesToAry/AppliesTo/AbsNameNum"/>
          </xsl:variable>

          <xsl:variable name="ptc">
            <xsl:value-of select="../AgntEnteredPsgrDescInfo[ApplesToAry/AppliesTo/AbsNameNum = $abs]/AgntEnteredPsgrDesc"/>
          </xsl:variable>

          <Psgr>
            <LNameNum>
              <xsl:choose>
                <xsl:when test="../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]">
                  <xsl:value-of select="../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]/LNameNum"/>
                </xsl:when>
                <xsl:when test="../../PNRBFRetrieve/LNameInfo[1]">
                  <xsl:value-of select="../../PNRBFRetrieve/LNameInfo[1]/LNameNum"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="PsgrAry/Psgr/LNameNum"/>
                </xsl:otherwise>
              </xsl:choose>

            </LNameNum>
            <PsgrNum>
              <xsl:value-of select="../../PNRBFRetrieve/FNameInfo[AbsNameNum=$abs]/PsgrNum"/>
            </PsgrNum>
            <AbsNameNum>
              <xsl:value-of select="$abs"/>
            </AbsNameNum>
            <PTC>
              <xsl:value-of select="$ptc"/>
            </PTC>
            <TIC/>

            <!-- -->
            <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator">
              <xsl:variable name="tktDesg" select="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator" />
              <TkDesignator>
                <xsl:value-of select="$tktDesg"/>
              </TkDesignator>
            </xsl:if>

            <xsl:if test="../../OTA_PNRRepriceRQ/@StoreFare = 'true'">
              <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup">
                <xsl:if test="../../OTA_PNRRepriceRQ/StoredFare/@FareType='Private'">
                  <DiscOrIncrInd>
                    <xsl:choose>
                      <xsl:when test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                        <xsl:text>IF</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>IP</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </DiscOrIncrInd>
                  <AmtOrPercent>
                    <xsl:choose>
                      <xsl:when test="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                        <xsl:number value="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount" format="000000001"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:number value="../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Percent" format="000000001"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </AmtOrPercent>
                </xsl:if>
              </xsl:if>
            </xsl:if>
          </Psgr>

        </xsl:for-each>
      </xsl:when>
    </xsl:choose>

  </xsl:template>

  <!--
   ===================================================================
  Air Segments 
  =================================================================== 
  -->
  <xsl:template match="AirSeg">
    <FltSeg>
      <AirV>
        <xsl:value-of select="AirV" />
      </AirV>
      <FltNum>
        <xsl:value-of select="FltNum" />
      </FltNum>
      <Dt>
        <xsl:value-of select="Dt" />
      </Dt>
      <StartAirp>
        <xsl:value-of select="StartAirp" />
      </StartAirp>
      <EndAirp>
        <xsl:value-of select="EndAirp" />
      </EndAirp>
      <StartTm>
        <xsl:value-of select="StartTm" />
      </StartTm>
      <EndTm>
        <xsl:value-of select="EndTm" />
      </EndTm>
      <AsBookedBIC>
        <xsl:value-of select="BIC" />
      </AsBookedBIC>
      <DayChgInd>
        <xsl:value-of select="DayChg" />
      </DayChgInd>
      <Conx>
        <xsl:value-of select="ConxInd" />
      </Conx>
    </FltSeg>
  </xsl:template>

  <!--
   ===================================================================
  Passangers 
  =================================================================== 
  -->
  <xsl:template match="AgntEnteredPsgrDescInfo">
    <xsl:param name="fareNum" />

    <xsl:variable name="ptc" >
      <xsl:value-of select="AgntEnteredPsgrDesc"/>
      <!-- <xsl:choose>
        <xsl:when test="substring(AgntEnteredPsgrDesc, 1,1) = 'C'">
          <xsl:text>CHD</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="AgntEnteredPsgrDesc"/>
        </xsl:otherwise>
      </xsl:choose>-->
    </xsl:variable>

    <xsl:for-each select="ApplesToAry/AppliesTo">
      <xsl:variable name="abs">
        <xsl:value-of select="AbsNameNum"/>
      </xsl:variable>
      <xsl:variable name="ptc2" >
        <xsl:choose>
          <xsl:when test="contains(../../../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]/NameRmk, '-')">
            <xsl:value-of select="substring-after(../../../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]/NameRmk, '-')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ptc"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <Psgr>
        <LNameNum>
          <xsl:choose>
            <xsl:when test="../../../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]">
              <xsl:value-of select="../../../../PNRBFRetrieve/NameRmkInfo[AbsNameNum=$abs]/LNameNum"/>
            </xsl:when>
            <xsl:when test="../../../../PNRBFRetrieve/LNameInfo[1]">
              <xsl:value-of select="../../../../PNRBFRetrieve/LNameInfo[1]/LNameNum"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="PsgrAry/Psgr/LNameNum"/>
            </xsl:otherwise>
          </xsl:choose>
        </LNameNum>
        <PsgrNum>
          <xsl:value-of select="../../../../PNRBFRetrieve/FNameInfo[AbsNameNum=$abs]/PsgrNum"/>
        </PsgrNum>
        <AbsNameNum>
          <xsl:value-of select="$abs"/>
        </AbsNameNum>
        <PTC>
          <xsl:value-of select="$ptc2"/>
        </PTC>
        <TIC/>

        <!-- -->
        <xsl:if test="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator">
          <xsl:variable name="tktDesg" select="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code=$ptc]/TicketDesignator" />
          <TkDesignator>
            <xsl:value-of select="$tktDesg"/>
          </TkDesignator>
        </xsl:if>

        <xsl:if test="../../../../OTA_PNRRepriceRQ/@StoreFare = 'true'">
          <xsl:if test="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup">
            <xsl:if test="../../../../OTA_PNRRepriceRQ/StoredFare/@FareType='Private'">
              <DiscOrIncrInd>
                <xsl:choose>
                  <xsl:when test="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                    <xsl:text>IF</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>IP</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </DiscOrIncrInd>
              <AmtOrPercent>
                <xsl:choose>
                  <xsl:when test="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount">
                    <xsl:number value="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Amount" format="000000001"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:number value="../../../../OTA_PNRRepriceRQ/StoredFare[PassengerType/@Code = $ptc]/Markup/@Percent" format="000000001"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AmtOrPercent>
            </xsl:if>
          </xsl:if>
        </xsl:if>

      </Psgr>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="multyEndorsmats">
    <xsl:param name="endo"/>
    <xsl:choose>
      <xsl:when test="contains($endo, '/')" >
        <xsl:variable name="elems">
          <xsl:call-template name="tokenizeString">
            <xsl:with-param name="list" select="$endo"/>
            <xsl:with-param name="delimiter" select="'/'"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:for-each select="msxsl:node-set($elems)/elem">
          <xsl:if test="not(position() > 3)">
            <xsl:element name="{concat('Endors', position())}">
              <xsl:value-of select="."/>
            </xsl:element>
          </xsl:if>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="delElems">
          <xsl:call-template name="wrap">
            <xsl:with-param name="text" select="$endo"/>
            <xsl:with-param name="line-length" select="29"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="elems">
          <xsl:call-template name="tokenizeString">
            <xsl:with-param name="list" select="$delElems"/>
            <xsl:with-param name="delimiter" select="'/'"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:for-each select="msxsl:node-set($elems)/elem">
          <xsl:if test="not(position() > 3)">
            <xsl:element name="{concat('Endors', position())}">
              <xsl:value-of select="."/>
            </xsl:element>
          </xsl:if>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
  ############################################################
  ## Template to tokenize strings with delimeter            ##
  ############################################################
  -->
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

  <!--
  ############################################################
  ## Template to tokenize strings based on the length       ##
  ############################################################
  -->
  <xsl:template name="wrap">
    <xsl:param name="text" select="."/>
    <xsl:param name="line-length" select="29"/>
    <xsl:param name="carry">
      <xsl:variable name="lengths">
        <xsl:for-each select="preceding-sibling::text()">
          <length>
            <xsl:value-of select="string-length()"/>
          </length>
        </xsl:for-each>
      </xsl:variable>
      <xsl:value-of select="sum(msxsl:node-set($lengths)/length) mod $line-length"/>
    </xsl:param>

    <xsl:value-of select="substring($text, 1, $line-length - $carry)"/>
    <xsl:text>/</xsl:text>
    <xsl:if test="$carry + string-length($text) > $line-length">
      <!-- recursive call -->
      <xsl:call-template name="wrap">
        <xsl:with-param name="text" select="substring($text, $line-length - $carry + 1)"/>
        <xsl:with-param name="carry" select="0"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
