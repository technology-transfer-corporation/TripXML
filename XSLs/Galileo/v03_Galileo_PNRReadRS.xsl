<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" version="1.0">
  <!-- 
  ================================================================== 
   Galileo_PNRReadRS.xsl - v03														
  ==================================================================
  Date: 11 Feb 2022 - Kobelev - Bate Birth for CHD and INF.
  Date: 21 Oct 2021 - Kobelev - Change Controlling Carrier RemarkType from "Z" to "CC".
  Date: 14 Sep 2021 - Kobelev - Security SSR CTCR,CTCE,CTCM with passanger association.
  Date: 16 Aug 2021 - Kobelev - Controlling Carrier Identification.
  Date: 01 Mar 2021 - Kobelev - ARNK Segments added Segment Number.
  Date: 18 Feb 2021 - Kobelev - Ticketing Date Time.
  Date: 20 Jan 2021 - Kobelev - Removed ADT,CHD,INF from NameRmkInfo in order to create Birthday tag corectly.
  Date: 17 Nov 2020 - Kobelev - Removed Credit Card masking.
  Date: 12 Nov 2020 - Kobelev - Unified FareBasisCode display.
  Date: 03 Nov 2020 - Kobelev - Multy FQ vs. Single FQ with multy Quotes. and FOP CASH display fix.
  Date: 14 Jul 2020 - Kobelev - Barnded Family Segment Association Display.
  Date: 31 Mar 2020 - Kobelev - RPH for Remarks and Special Remarks
  Date: 18 Mar 2020 - Kobelev - Barnded Family Base Fare Price Display
  Date: 12 Mar 2020 - Kobelev - Better Allowed Bag display, also pricing command display in SupplementalInfo
  Date: 03 Mar 2020 - Kobelev - Added CompanyName element
  Date: 17 Feb 2020 - Kobelev - TPA_Extensions added to Itinerary Item Pricing.
  Date: 14 Feb 2020 - Kobelev - Tour Code, Flight Reference Numbers in Item Pricing.
  Date: 20 Jan 2020 - Kobelev - Unifying Price Quote display with other GDS.
  Date: 23 May 2019 - Kobelev - Start using AltLangID as GDS idetifier
  Date: 13 Mar 2019 - Kobelev - Default identification of AIN Number
  Date: 01 Mar 2019 - Kobelev - Default Operating Airline
  Date: 28 Feb 2019 - Kobelev - New version of PNR Read (51)
  Date: 30 Sep 2014 - Rastko - added mapping to passenger association in stored fares		
  Date: 10 Sep 2014 - Rastko - added support for ConversationID			
  Date: 25 Aug 2014 - Rastko - added support for warning messages			
  Date: 03 Oct 2012 - Rastko - corrected mappping for car extra miles calculation		
  Date: 23 Aug 2012 - Rastko - corrected mappping ot air confirmation number		
  Date: 23 Aug 2012 - Rastko - corrected parsing of passenger birth date			
  Date: 24 Jul 2012 - Rastko - corrected error processing for custom check rule		
  Date: 17 Feb 2011 - Rastko - corrected operating carrier mapping 				
  Date: 18 Jan 2011 - Rastko - corrected FOP mapping in hotel segment			
  Date: 21 Oct 2009 - Rastko - added RPH attribute to Item element				
  Date: 11 Oct 2009 - Rastko - added RPH attribite to ItemPricing element			
  Date: 30 Sep 2009 - Rastko - corrected parsing of name remark to check for birth date	
  Date: 30 Sep 2009 - Rastko - corrected parsing of Agency Commission			
  Date: 22 Sep 2009 - Rastko - added ArrivalDateTime calculation				
  Date: 18 Sep 2009 - Rastko - added support for vendor remarks				
  Date: 14 Feb 2009 - Rastko - added support for due/paid/text					
  ================================================================== 
  -->
  <xsl:output omit-xml-declaration="yes" />

  <xsl:key name="conCarr" match="//PNRBFManagement_53/DocProdDisplayStoredQuote/PNRFareDetail/FareOwnCarrier" use="." />

  <xsl:template match="/">
    <OTA_TravelItineraryRS Version="v03" AltLangID="Galileo">
      <xsl:apply-templates select="PNRBFManagement_17" />
      <xsl:apply-templates select="PNRBFManagement_53" />
      <xsl:apply-templates select="QueueProcessing_16" />
      <xsl:if test="PNRBFManagement_17/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="PNRBFManagement_17/ConversationID"/>
        </ConversationID>
      </xsl:if>
      <xsl:if test="PNRBFManagement_53/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="PNRBFManagement_53/ConversationID"/>
        </ConversationID>
      </xsl:if>
      <xsl:if test="QueueProcessing_16/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="QueueProcessing_16/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_TravelItineraryRS>
  </xsl:template>

  <xsl:template match="PNRBFManagement_17 | PNRBFManagement_53 | QueueProcessing_16">
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
      <xsl:when test="TransactionErrorCode/Code!= '' and not(PNRBFRetrieve)">
        <Errors>
          <xsl:apply-templates select="TransactionErrorCode"/>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <Success />
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
        <TravelItinerary>
          <ItineraryRef>
            <xsl:attribute name="Type">PNR</xsl:attribute>
            <xsl:attribute name="ID">
              <xsl:value-of select="PNRBFRetrieve/GenPNRInfo/RecLoc" />
            </xsl:attribute>
            <xsl:attribute name="ID_Context">
              <xsl:value-of select="PNRBFRetrieve/GenPNRInfo/CurAgncyPCC"/>
            </xsl:attribute>
            <CompanyName>
              <!-- 03MAR2020 | 20200227-->
              <xsl:variable name="CreationDate">
                <xsl:value-of select="substring(PNRBFRetrieve/GenPNRInfo/CreationDt,1,4)"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(PNRBFRetrieve/GenPNRInfo/CreationDt,5,2)"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(PNRBFRetrieve/GenPNRInfo/CreationDt,7,2)"/>
              </xsl:variable>
              <xsl:attribute name="Code">
                <xsl:value-of select="concat(PNRBFRetrieve/GenPNRInfo/CreatingAgncyIATANum, '|', $CreationDate)"/>
              </xsl:attribute>
              <xsl:attribute name="CodeContext">IATACode</xsl:attribute>
              <xsl:value-of select="concat(translate(PNRBFRetrieve/GenPNRInfo/OwningAgncyPCC, ' ',''), '/', translate(PNRBFRetrieve/GenPNRInfo/CreatingAgntSignOn, ' ',''), PNRBFRetrieve/GenPNRInfo/CreatingAgntDuty)"/>
              <!--  
                <xsl:value-of select="PNR_HI_INF/Line[@PCC != ''][last()]/@PCC"/>
                <xsl:variable name="agent">
                  <xsl:value-of select="concat(ETR_INF/MIS_INF/MIS_TIC_INF[last()]/SID, '/', ETR_INF/MIS_INF/MIS_TIC_INF[last()]/TIC_AGT_ID, ' ')" />
                </xsl:variable>
                <xsl:call-template name="string-trim">
                  <xsl:with-param name="string" select="$agent" />
                </xsl:call-template>
                -->
            </CompanyName>
          </ItineraryRef>
          <xsl:apply-templates select="PNRBFRetrieve" />
        </TravelItinerary>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			PNR General Info - PNRBFRetrieve                                          -->
  <!--************************************************************************************-->
  <xsl:template match="PNRBFRetrieve">
    <!--******************************************************-->
    <!--				PNR Info                                     -->
    <!--******************************************************-->
    <CustomerInfos>
      <xsl:apply-templates select="LNameInfo" mode="pax" />
    </CustomerInfos>
    <ItineraryInfo>
      <ReservationItems>
        <!--  Process Air Itinerary	      -->
        <xsl:if test="AirSeg or OpenAirSeg or ARNK">
          <xsl:apply-templates select="AirSeg | OpenAirSeg | ARNK" mode="Segment" />
        </xsl:if>
        <!--  Process Car Segments	      -->
        <xsl:if test="CarSeg">
          <xsl:apply-templates select="CarSeg" />
        </xsl:if>
        <!--  Process Hotel Segments      -->
        <xsl:if test="HtlSeg">
          <xsl:apply-templates select="HtlSeg" />
        </xsl:if>
        <xsl:if test="NonAirSeg">
          <xsl:apply-templates select="NonAirSeg"/>
        </xsl:if>
        <xsl:if test="../DocProdDisplayStoredQuote/FareNumInfo">
          <xsl:apply-templates select="../DocProdDisplayStoredQuote[not(ErrText)]" />
        </xsl:if>
      </ReservationItems>
      <xsl:if test="GenPNRInfo">
        <xsl:apply-templates select="GenPNRInfo" />
        <!--  Process Ticketing Info   -->
      </xsl:if>
      <!--******************************************************-->
      <!--			Special Request Details                    -->
      <!--******************************************************-->
      <xsl:if test="SeatSeg or ProgramaticSSR or NonProgramaticSSR or OSI or GenRmkInfo or  ItinRmk or InvoiceRmk[Keyword != '3007'] or VndRmk">
        <SpecialRequestDetails>
          <xsl:if test="SeatSeg">
            <SeatRequests>
              <xsl:apply-templates select="SeatSeg" />
            </SeatRequests>
          </xsl:if>
          <xsl:if test="ProgramaticSSR or NonProgramaticSSR">
            <SpecialServiceRequests>
              <xsl:if test="ProgramaticSSR">
                <xsl:apply-templates select="ProgramaticSSR" />
              </xsl:if>
              <xsl:if test="NonProgramaticSSR">
                <xsl:apply-templates select="NonProgramaticSSR" />
              </xsl:if>
            </SpecialServiceRequests>
          </xsl:if>
          <xsl:if test="OSI">
            <OtherServiceInformations>
              <xsl:apply-templates select="OSI" />
            </OtherServiceInformations>
          </xsl:if>
          <xsl:if test="GenRmkInfo">
            <Remarks>
              <xsl:apply-templates select="GenRmkInfo" />
            </Remarks>
          </xsl:if>
          <xsl:if test="DuePaidInfo">
            <UniqueRemarks>
              <xsl:apply-templates select="DuePaidInfo" />
            </UniqueRemarks>
          </xsl:if>
          <xsl:if test="ItinRmk or InvoiceRmk[Keyword != '3007'] or VndRmk">
            <SpecialRemarks>
              <xsl:if test="ItinRmk">
                <xsl:apply-templates select="ItinRmk" />
              </xsl:if>
              <xsl:if test="InvoiceRmk[Keyword != '3007']">
                <xsl:apply-templates select="InvoiceRmk[Keyword != '3007']" mode="invrmk"/>
              </xsl:if>
              <xsl:if test="VndRmk">
                <xsl:apply-templates select="VndRmk" />
              </xsl:if>
              <xsl:if test="../DocProdDisplayStoredQuote/TourCode">
                <xsl:apply-templates select="../DocProdDisplayStoredQuote[TourCode]" mode="tourcode"/>
              </xsl:if>
              <xsl:if test="../DocProdDisplayStoredQuote/InfoMsg[MsgType='1']">
                <xsl:apply-templates select="../DocProdDisplayStoredQuote[InfoMsg[MsgType='1']]" mode="EndorsementInfo"/>
              </xsl:if>
              <xsl:if test="../DocProdDisplayStoredQuote/EndorsementBox">
                <xsl:apply-templates select="../DocProdDisplayStoredQuote[EndorsementBox]" mode="Endorsement"/>
              </xsl:if>

              <xsl:if test="../DocProdDisplayStoredQuote/PNRFareDetail/FareOwnCarrier">
                <xsl:apply-templates select="../DocProdDisplayStoredQuote[PNRFareDetail/FareOwnCarrier]" mode="Endorsement"/>
              </xsl:if>

            </SpecialRemarks>
          </xsl:if>
        </SpecialRequestDetails>
      </xsl:if>
      <xsl:if test="../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo">
        <TPA_Extensions>
          <IssuedTickets>
            <xsl:apply-templates select="../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo[TkNum != '']" mode="IssuedTicket" />
          </IssuedTickets>
        </TPA_Extensions>
      </xsl:if>
    </ItineraryInfo>
    <!--******************************************************-->
    <!--			Form of Payment                               -->
    <!--******************************************************-->
    <TravelCost>
      <xsl:if test="CreditCardFOP">
        <FormOfPayment>
          <xsl:apply-templates select="CreditCardFOP" />
        </FormOfPayment>
      </xsl:if>
      <xsl:if test="OtherFOP">
        <FormOfPayment>
          <xsl:apply-templates select="OtherFOP" />
        </FormOfPayment>
      </xsl:if>
      <xsl:if test="CheckFOP">
        <FormOfPayment>
          <xsl:apply-templates select="CheckFOP" />
        </FormOfPayment>
      </xsl:if>
    </TravelCost>

    <UpdatedBy>
      <xsl:attribute name="CreateDateTime">
        <xsl:value-of select="substring(GenPNRInfo/CreationDt,1,4)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(GenPNRInfo/CreationDt,5,2)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(GenPNRInfo/CreationDt,7,2)"/>
        <xsl:text>T00:00:00</xsl:text>
      </xsl:attribute>
    </UpdatedBy>

    <xsl:if test="../DocProdDisplayStoredQuote/CommissionMod or InvoiceRmk[Keyword = '3007'] or InvoiceRmk[Keyword = '3010' and contains(Rmk,'SF-')] or InvoiceRmk[Keyword = '3010' and (contains(Rmk,'FT-') or contains(Rmk,'AN'))]">
      <TPA_Extensions>
        <xsl:apply-templates select="../DocProdDisplayStoredQuote/CommissionMod"/>
        <xsl:apply-templates select="InvoiceRmk[Keyword = '3010' and contains(Rmk,'SF-')]" mode="serfee"/>
        <xsl:apply-templates select="InvoiceRmk[Keyword = '3007' or (contains(Rmk,'FT-') or contains(Rmk,'AN'))]" mode="accline"/>
      </TPA_Extensions>
    </xsl:if>
  </xsl:template>

  <xsl:template match="CommissionMod">
    <xsl:if test="Percent != '' or Amt != ''">
      <AgencyCommission>
        <xsl:choose>
          <xsl:when test="Percent != ''">
            <xsl:attribute name="Percent">
              <xsl:value-of select="Percent"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Amount">
              <xsl:value-of select="Amt"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </AgencyCommission>
    </xsl:if>
  </xsl:template>

  <xsl:template match="InvoiceRmk" mode="serfee">
    <AgencyServiceFee>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(substring-before(substring-after(Rmk,'SF-'),'-'),'@','.')"/>
      </xsl:attribute>
    </AgencyServiceFee>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			PNR Retrieve Errors                                           	                 -->
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
            <xsl:value-of select="Msg"/>
          </Error>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <Error>
          <xsl:attribute name="Type">Galileo</xsl:attribute>
          <xsl:choose>
            <xsl:when test="../PNRBFPrimaryBldChg/RecID = 'EROR'">
              <xsl:value-of select="../PNRBFPrimaryBldChg/Text"/>
            </xsl:when>
            <xsl:when test="../CustomCheckRuleExecute/ErrorCode != ''">
              <xsl:value-of select="../CustomCheckRuleExecute/CCRuleExecute/Msg"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>Processing error - </xsl:text>
              <xsl:value-of select="Code"/>
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

  <xsl:template match="ErrText" mode="error">
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

  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			           -->
  <!--***********************************************************************************-->
  <xsl:template match="DocProdDisplayStoredQuote">
    <ItemPricing>
      <AirFareInfo>
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
              <xsl:value-of select="GenQuoteDetails/TotCurrency" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="GenQuoteDetails/TotDecPos" />
            </xsl:attribute>
          </Taxes>
          <TotalFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$amttot1" />
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="GenQuoteDetails/TotCurrency" />
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="GenQuoteDetails/TotDecPos" />
            </xsl:attribute>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:choose>
            <xsl:when test="count(GenQuoteDetails) > 1">
              <xsl:apply-templates select="GenQuoteDetails" mode="Details">
                <xsl:with-param name="FQType" select="'oneFQ'" />
              </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="GenQuoteDetails" mode="Details" >
                <xsl:with-param name="FQType" select="'multyFQ'" />
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>
        </PTC_FareBreakdowns>
      </AirFareInfo>
    </ItemPricing>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="GenQuoteDetails" mode="basefare">
    <xsl:param name="total" />
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
    <xsl:text>/</xsl:text>
  </xsl:template>

  <xsl:template match="GenQuoteDetails" mode="basefare2">
    <xsl:param name="total" />
    <xsl:variable name="uk">
      <xsl:value-of select="format-number(UniqueKey,'0000')" />
    </xsl:variable>
    <xsl:variable name="totpax">
      <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$uk]/ApplesToAry/AppliesTo)"/>
    </xsl:variable>
    <xsl:variable name="thistotal">
      <xsl:value-of select="EquivAmt * $totpax" />
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

      <xsl:if test="../AssocSegs/SegNumAry/SegNum != ''">
        <xsl:attribute name="FlightRefNumberRPHList">
          <xsl:for-each select="../AssocSegs/SegNumAry">
            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="SegNum"/>
          </xsl:for-each>
        </xsl:attribute>
      </xsl:if>

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
      </xsl:choose>

      <xsl:variable name="fqN">
        <xsl:choose>
          <xsl:when test="$FQType='oneFQ'">
            <xsl:value-of select="UniqueKey"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="../FareNumInfo/FareNumAry/FareNum"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:attribute name="RPH">
        <xsl:value-of select="concat(../FareNumInfo/FareNumAry/FareNum,'.',UniqueKey)"/>
      </xsl:attribute>

      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:variable name="PsgrType">
            <xsl:value-of select="../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]/AgntEnteredPsgrDesc"/>
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
          <xsl:value-of select="count(../AgntEnteredPsgrDescInfo[UniqueKey=$paxno]/ApplesToAry/AppliesTo)"/>
        </xsl:attribute>
      </PassengerTypeQuantity>

      <FareBasisCodes>
        <xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno2]">
          <FareBasisCode>
            <xsl:variable name="fic">
              <!--<xsl:value-of select="concat(FIC,'/',translate(TkDesignator, 'CH/', ''))" />-->
              <xsl:value-of select="concat(FIC,'/',TkDesignator)" />
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="string-length(FIC) = 8">
                <!--<xsl:value-of select="concat(FIC,'/', translate(TkDesignator, 'CH/', ''))" />-->
                <xsl:value-of select="concat(FIC,'/',TkDesignator)" />
              </xsl:when>
              <!--
              <xsl:when test="contains(../FareConstruction/FareConstructText,$fic) and TkDesignator">
                <xsl:value-of select="TkDesignator"/>
              </xsl:when>
              -->
              <xsl:otherwise>
                <xsl:value-of select="FIC" />
              </xsl:otherwise>
            </xsl:choose>
          </FareBasisCode>
        </xsl:for-each>
      </FareBasisCodes>

      <xsl:if test="../BrandInformation">
        <BrandedFares>
          <xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno2]">

            <xsl:variable name="fic">
              <xsl:choose>
                <xsl:when test="contains(FIC, '/')">
                  <xsl:value-of select="substring-before(FIC, '/')" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="FIC" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="td">
              <xsl:choose>
                <xsl:when test="contains(TkDesignator, '/')">
                  <xsl:value-of select="substring-after(TkDesignator, '/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="TkDesignator" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="seg">
              <xsl:value-of select="format-number(RelSegNum, '00')"/>
            </xsl:variable>

            <xsl:if test="../BrandInformation[FIC=$fic and TkDesignator=$td and contains(SegNumList,$seg)]/SegNumList">
              <FareFamily>
                <xsl:attribute name="RPH">
                  <xsl:value-of select="format-number($seg, '0')"/>
                </xsl:attribute>
                <xsl:attribute name="Code">
                  <xsl:value-of select="../BrandInformation[FIC=$fic and TkDesignator=$td and contains(SegNumList,$seg)]/PricebyBrandModifier"/>
                </xsl:attribute>
                <xsl:value-of select="../BrandInformation[FIC=$fic and TkDesignator=$td and contains(SegNumList,$seg)]/BrandName"/>
              </FareFamily>
            </xsl:if>
          </xsl:for-each>

        </BrandedFares>
      </xsl:if>

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
          <xsl:value-of select="../PlatingAirVMod/AirV"/>
        </ValidatingAirlineCode>

        <xsl:if test="../SegRelatedInfo/BagInfo">
          <BagAllowance>
            <xsl:choose>
              <xsl:when test="contains(../SegRelatedInfo/BagInfo[1], 'PC')">
                <xsl:attribute name="Quantity">
                  <xsl:value-of select="substring-before(../SegRelatedInfo/BagInfo, 'PC')"/>
                </xsl:attribute>
                <xsl:attribute name="Type">
                  <xsl:text>Piece</xsl:text>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="Weight">
                  <xsl:value-of select="substring(../SegRelatedInfo/BagInfo, 1,string-length(../SegRelatedInfo/BagInfo) - 1)"/>
                </xsl:attribute>
                <xsl:attribute name="Type">
                  <xsl:text>Weight</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="Unit">
                  <xsl:value-of select="substring(../SegRelatedInfo/BagInfo, string-length(../SegRelatedInfo/BagInfo))"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            <!--
            <xsl:attribute name="ItinSeqNumber">
              <xsl:for-each select="../SegRelatedInfo">
                <xsl:if test="position() > 1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="RelSegNum"/>
              </xsl:for-each>
            </xsl:attribute>
            -->
          </BagAllowance>
        </xsl:if>

        <xsl:if test="../StoredQuoteModifier/StoredQuoteModifierdata">
          <SupplementalInfo>
            <xsl:value-of select="../StoredQuoteModifier/StoredQuoteModifierdata"/>
          </SupplementalInfo>
        </xsl:if>

      </TPA_Extensions>

    </PTC_FareBreakdown>
  </xsl:template>

  <!-- ************************************************************** -->
  <!--      Get individual Tax info  	                                    -->
  <!-- ************************************************************** -->
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
  <!--*****************************************************************-->
  <!--			Hotel Segs								    -->
  <!--*****************************************************************-->
  <xsl:template match="HtlSeg">
    <xsl:variable name="pos">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="SegNum" />
      </xsl:attribute>
      <xsl:attribute name="Status">
        <xsl:value-of select="Status" />
      </xsl:attribute>
      <xsl:if test="Status!='HK'">
        <xsl:attribute name="IsPassive">true</xsl:attribute>
      </xsl:if>
      <Hotel>
        <Reservation>
          <xsl:if test="RateChgInd='Y'">
            <RatePlans>
              <RatePlan>
                <xsl:attribute name="RateIndicator">
                  <xsl:text>ChangeDuringStay</xsl:text>
                </xsl:attribute>
              </RatePlan>
            </RatePlans>
          </xsl:if>
          <xsl:if test="RateTypeSold!=''">
            <RatePlans>
              <RatePlan>
                <xsl:attribute name="RatePlanType">
                  <xsl:value-of select="RateTypeSold"/>
                </xsl:attribute>
              </RatePlan>
            </RatePlans>
          </xsl:if>
          <RoomRates>
            <RoomRate>
              <xsl:attribute name="NumberOfUnits">
                <xsl:value-of select="NumRooms" />
              </xsl:attribute>
              <xsl:attribute name="RatePlanCode">
                <xsl:value-of select="RateCode" />
              </xsl:attribute>
              <Rates>
                <Rate>
                  <xsl:attribute name="GuaranteedInd">
                    <xsl:choose>
                      <xsl:when test="RateTypeSold = 'RG'">true</xsl:when>
                      <xsl:otherwise>false</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <Base>
                    <xsl:attribute name="AmountAfterTax">
                      <xsl:value-of select="RateAmt" />
                    </xsl:attribute>
                    <xsl:attribute name="CurrencyCode">
                      <xsl:value-of select="Currency" />
                    </xsl:attribute>
                    <xsl:attribute name="DecimalPlaces">
                      <xsl:value-of select="RateDecPos" />
                    </xsl:attribute>
                  </Base>
                </Rate>
              </Rates>
            </RoomRate>
          </RoomRates>
          <GuestCounts>
            <GuestCount>
              <xsl:value-of select="NumPersons" />
            </GuestCount>
          </GuestCounts>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:value-of select="substring(StartDt,1,4)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(StartDt,5,2)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(StartDt,7,2)" />
            </xsl:attribute>
            <xsl:attribute name="Duration">
              <xsl:value-of select="NumNights" />
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:value-of select="substring(EndDt,1,4)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(EndDt,5,2)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(EndDt,7,2)" />
            </xsl:attribute>
          </TimeSpan>
          <xsl:if test="following-sibling::HtlSegOptFlds[1]/FldAry/Fld/ID = 'GT'">
            <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'GT']" mode="Htl" />
          </xsl:if>
          <xsl:if test="following-sibling::HtlSegOptFlds[1]/FldAry/Fld/ID = 'DP'">
            <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'DP']" mode="Htl" />
          </xsl:if>
          <BasicPropertyInfo>
            <xsl:attribute name="ChainCode">
              <xsl:value-of select="HtlV" />
            </xsl:attribute>
            <xsl:attribute name="HotelCode">
              <xsl:choose>
                <xsl:when test="HtlPropNum">
                  <xsl:value-of select="HtlPropNum" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="RoomMasterID" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="HotelCityCode">
              <xsl:value-of select="Pt" />
            </xsl:attribute>
            <xsl:attribute name="HotelName">
              <xsl:value-of select="PropName" />
            </xsl:attribute>
          </BasicPropertyInfo>
        </Reservation>
        <TPA_Extensions>
          <xsl:attribute name="RPH">
            <xsl:value-of select="SegNum" />
          </xsl:attribute>
          <xsl:attribute name="BookingCode">
            <xsl:value-of select="RateCode" />
          </xsl:attribute>
          <xsl:attribute name="Status">
            <xsl:value-of select="Status" />
          </xsl:attribute>
          <xsl:if test="ConfNum!=''">
            <xsl:attribute name="ConfirmationNumber">
              <xsl:value-of select="ConfNum" />
            </xsl:attribute>
          </xsl:if>
          <!-- Customer address     -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'AD']" mode="Htl" />
          <!-- Booking source     -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'BS']" mode="Htl" />
          <!-- Supplemental info     -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'SI']" mode="Htl" />
          <!-- Last name    -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'NL']" mode="Htl" />
          <!-- First name     -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'NF']" mode="Htl" />
          <!-- Extra Adult     -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'EX']" mode="Htl" />
          <!-- Extra Child      -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'EC']" mode="Htl" />
          <!-- Crib                -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'CR']" mode="Htl" />
          <!-- Rollaway Adult   -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RA']" mode="Htl" />
          <!-- Rollaway Child   -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RD']" mode="Htl" />
          <!-- Alternate Currency   -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'AC']" mode="Htl" />
          <!-- Merchant currency -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'VC']" mode="Htl" />
          <!-- Rate Quoted   -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RQ']" mode="Htl" />
          <!-- Rate Requested  -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RR']" mode="Htl" />
          <!-- Overwrite Corporate Rate -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RT']" mode="Htl" />
          <!-- Tour Number -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'TN']" mode="Htl" />
          <!-- Room Location  -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'RL']" mode="Htl" />
          <!-- Meal Plan -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'MP']" mode="Htl" />
          <!-- Corporate Discount  -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'CD']" mode="Htl" />
          <!-- Advanced Deposit  -->
          <xsl:apply-templates select="following-sibling::HtlSegOptFlds[1]/FldAry/Fld[ID = 'AV']" mode="Htl" />
          <xsl:if test="../../HtlSegSellText[position() = $pos]/RuleLineAry">
            <Rules>
              <xsl:apply-templates select="../../HtlSegSellText[position() = $pos]/RuleLineAry"/>
            </Rules>
          </xsl:if>
        </TPA_Extensions>
      </Hotel>
    </Item>
  </xsl:template>

  <xsl:template match="RuleLineAry">
    <xsl:for-each select="RuleLine">
      <xsl:if test=". != ''">
        <Text>
          <xsl:value-of select="."/>
        </Text>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			      Air Itinerary Section				                              	    -->
  <!--************************************************************************************-->
  <xsl:template match="AirSeg | OpenAirSeg" mode="Segment">
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="SegNum" />
      </xsl:attribute>
      <Air>
        <xsl:attribute name="RPH">
          <xsl:value-of select="SegNum" />
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="self::OpenAirSeg">
            <xsl:apply-templates select="." />
          </xsl:when>
          <xsl:otherwise>
            <!--************************************************************************************-->
            <!--			Air Segments/Passive  segments  						 -->
            <!--************************************************************************************-->
            <xsl:variable name="zeroes">0000</xsl:variable>
            <xsl:attribute name="NumberInParty">
              <xsl:value-of select="NumPsgrs" />
            </xsl:attribute>
            <xsl:attribute name="ResBookDesigCode">
              <xsl:value-of select="BIC" />
            </xsl:attribute>
            <xsl:attribute name="Status">
              <xsl:value-of select="Status" />
            </xsl:attribute>
            <xsl:attribute name="DepartureDateTime">
              <xsl:variable name="deptime">
                <xsl:value-of select="format-number(StartTm,'0000')" />
              </xsl:variable>
              <xsl:value-of select="substring(Dt,1,4)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(Dt,5,2)" />
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(Dt,7,2)" />
              <xsl:text>T</xsl:text>
              <xsl:value-of select="substring($deptime,1,2)" />
              <xsl:text>:</xsl:text>
              <xsl:value-of select="substring($deptime,3,2)" />
              <xsl:text>:00</xsl:text>
            </xsl:attribute>
            <xsl:variable name="arrtime">
              <xsl:value-of select="format-number(EndTm,'0000')" />
            </xsl:variable>
            <xsl:variable name="start">
              <xsl:value-of select="substring(Dt,1,4)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(Dt,5,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(Dt,7,2)"/>
            </xsl:variable>
            <xsl:variable name="dayc">
              <xsl:value-of select="DayChg"/>
            </xsl:variable>
            <xsl:variable name="dc" select="ttVB:FctArrDate(string($start),$dayc)"/>
            <xsl:attribute name="ArrivalDateTime">
              <xsl:value-of select="substring($dc,1,10)"/>
              <xsl:text>T</xsl:text>
              <xsl:value-of select="substring($arrtime,1,2)" />
              <xsl:text>:</xsl:text>
              <xsl:value-of select="substring($arrtime,3,2)" />
              <xsl:text>:00</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="FlightNumber">
              <xsl:value-of select="FltNum" />
            </xsl:attribute>
            <xsl:if test="TklessInd != ''">
              <xsl:attribute name="E_TicketEligibility">
                <xsl:choose>
                  <xsl:when test="TklessInd = 'Y'">Eligible</xsl:when>
                  <xsl:otherwise>NotEligible</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:if>
            <DepartureAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="StartAirp" />
              </xsl:attribute>
            </DepartureAirport>
            <ArrivalAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="EndAirp" />
              </xsl:attribute>
            </ArrivalAirport>
            <xsl:choose>
              <xsl:when test="name(following-sibling::*[1])='AirSegOpAirV'">
                <xsl:apply-templates select="following-sibling::AirSegOpAirV[1]"/>
              </xsl:when>
              <xsl:otherwise>
                <OperatingAirline>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="AirV" />
                  </xsl:attribute>
                </OperatingAirline>
              </xsl:otherwise>
            </xsl:choose>
            <Equipment>
              <xsl:attribute name="AirEquipType" />
              <xsl:choose>
                <xsl:when test="COG = 'Y'">
                  <xsl:attribute name="ChangeofGauge">true</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="ChangeofGauge">false</xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </Equipment>
            <MarketingAirline>
              <xsl:attribute name="Code">
                <xsl:value-of select="AirV" />
              </xsl:attribute>
            </MarketingAirline>
            <TPA_Extensions>
              <xsl:attribute name="DateChange">
                <xsl:value-of select="DayChg" />
              </xsl:attribute>
              <xsl:choose>
                <xsl:when test="FltFlownInd = 'Y'">
                  <xsl:attribute name="FlownIndicator">true</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="FlownIndicator">false</xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:attribute name="ConfirmationNumber">
                <xsl:choose>
                  <xsl:when test="count(../VndRecLocs/RecLocInfoAry/RecLocInfo) = 1">
                    <xsl:value-of select="../VndRecLocs/RecLocInfoAry/RecLocInfo/RecLoc"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:variable name="air">
                      <xsl:value-of select="AirV"/>
                    </xsl:variable>
                    <xsl:choose>
                      <xsl:when test="../VndRecLocs/RecLocInfoAry/RecLocInfo[Vnd = $air]/RecLoc!=''">
                        <xsl:value-of select="../VndRecLocs/RecLocInfoAry/RecLocInfo[Vnd = $air]/RecLoc"/>
                      </xsl:when>
                      <xsl:when test="../VndRecLocs/RecLocInfoAry/RecLocInfo[Vnd = '1A']/RecLoc!=''">
                        <xsl:value-of select="../VndRecLocs/RecLocInfoAry/RecLocInfo[Vnd = '1A']/RecLoc"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </TPA_Extensions>
          </xsl:otherwise>
        </xsl:choose>
      </Air>
    </Item>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			Open Air  Segments										    -->
  <!--************************************************************************************-->
  <xsl:template match="OpenAirSeg">
    <xsl:attribute name="NumberInParty">
      <xsl:value-of select="NumPsgrs" />
    </xsl:attribute>
    <xsl:attribute name="ResBookDesigCode">
      <xsl:value-of select="BIC" />
    </xsl:attribute>
    <xsl:attribute name="DepartureDateTime">
      <xsl:variable name="zeroes">0000</xsl:variable>
      <xsl:variable name="deptime">
        <xsl:value-of select="substring(string($zeroes),1,4-string-length(StartTm))" />
        <xsl:value-of select="EndTm" />
      </xsl:variable>
      <xsl:value-of select="substring(Dt,1,4)" />
      <xsl:text>-</xsl:text>
      <xsl:value-of select="substring(Dt,5,2)" />
      <xsl:text>-</xsl:text>
      <xsl:value-of select="substring(Dt,7,2)" />
      <xsl:text>T</xsl:text>
      <xsl:value-of select="substring($deptime,1,2)" />
      <xsl:text>:</xsl:text>
      <xsl:value-of select="substring($deptime,3,2)" />
      <xsl:text>:00</xsl:text>
    </xsl:attribute>
    <xsl:attribute name="FlightNumber">
      <xsl:value-of select="FltNum" />
    </xsl:attribute>
    <DepartureAirport>
      <xsl:attribute name="LocationCode">
        <xsl:value-of select="StartAirp" />
      </xsl:attribute>
    </DepartureAirport>
    <ArrivalAirport>
      <xsl:attribute name="LocationCode">
        <xsl:value-of select="EndAirp" />
      </xsl:attribute>
    </ArrivalAirport>
    <xsl:choose>
      <xsl:when test="name(following-sibling::*[1])='AirSegOpAirV'">
        <xsl:apply-templates select="following-sibling::AirSegOpAirV[1]"/>
      </xsl:when>
      <xsl:otherwise>
        <OperatingAirline>
          <xsl:attribute name="Code">
            <xsl:value-of select="AirV" />
          </xsl:attribute>
        </OperatingAirline>
      </xsl:otherwise>
    </xsl:choose>
    <MarketingAirline>
      <xsl:attribute name="Code">
        <xsl:value-of select="AirV" />
      </xsl:attribute>
    </MarketingAirline>
  </xsl:template>
  <xsl:template match="AirSegOpAirV">
    <xsl:if test="OpAirVInfoAry/OpAirVInfo/AirV !='' or OpAirVInfoAry/OpAirVInfo/AirVName !=''">
      <OperatingAirline>
        <xsl:if test="OpAirVInfoAry/OpAirVInfo/AirV !=''">
          <xsl:attribute name="Code">
            <xsl:value-of select="OpAirVInfoAry/OpAirVInfo[position()=1]/AirV" />
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="OpAirVInfoAry/OpAirVInfo/AirVName !=''">
          <xsl:value-of select="OpAirVInfoAry/OpAirVInfo[position()=1]/AirVName" />
        </xsl:if>
      </OperatingAirline>
    </xsl:if>
  </xsl:template>
  <xsl:template match="ARNK" mode="Segment">
    <Item Status="">
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="SegNum" />
      </xsl:attribute>
      <TPA_Extensions>
        <Arnk/>
      </TPA_Extensions>
    </Item>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						 Passenger Information         		                        -->
  <!--************************************************************************************-->
  <xsl:template match="LNameInfo" mode="pax">
    <xsl:call-template name="buildnames">
      <xsl:with-param name="PsgrsTot">
        <xsl:value-of select="NumPsgrs" />
      </xsl:with-param>
      <xsl:with-param name="PsgrsNum">1</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!--************************************************************************************-->
  <!-- 						Telephone									    -->
  <!--************************************************************************************-->
  <xsl:template match="PhoneInfo" mode="Phone">
    <!--xsl:if test="substring(Phone,4,1)!='E'"-->
    <Telephone>
      <xsl:choose>
        <xsl:when test="Pt != ''">
          <xsl:attribute name="PhoneUseType">
            <xsl:choose>
              <xsl:when test="Type = 'R'">HOME</xsl:when>
              <xsl:when test="Type = 'B'">BUSINESS</xsl:when>
              <xsl:when test="Type = 'A'">AGENCY</xsl:when>
              <xsl:when test="Type = 'F'">FAX</xsl:when>
              <xsl:when test="Type = 'H'">HOTEL</xsl:when>
              <xsl:when test="Type = 'M'">MOBILE</xsl:when>
              <xsl:otherwise>OTHER</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="PhoneNumber">
            <xsl:value-of select="Phone" />
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PhoneUseType">OTHER</xsl:attribute>
          <xsl:attribute name="PhoneNumber">
            <xsl:value-of select="Phone" />
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </Telephone>
    <!--/xsl:if-->
  </xsl:template>
  <!--************************************************************************************-->
  <!--		Email  Processing									    -->
  <!--************************************************************************************-->
  <xsl:template match="Email">
    <Email>
      <xsl:value-of select="translate(Data,'¤','@')"/>
    </Email>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Address/Delivery Addres information						    -->
  <!--************************************************************************************-->
  <xsl:template match="AddrInfo">
    <Address>
      <xsl:attribute name="Type">Home</xsl:attribute>
      <xsl:attribute name="UseType">Mailing</xsl:attribute>
      <xsl:apply-templates select="Addr" />
    </Address>
  </xsl:template>
  <xsl:template match="DeliveryAddrInfo">
    <Address>
      <xsl:attribute name="Type">Home</xsl:attribute>
      <xsl:attribute name="UseType">Delivery</xsl:attribute>
      <xsl:apply-templates select="DeliveryAddr" />
    </Address>
  </xsl:template>
  <xsl:template match="Addr | DeliveryAddr">
    <xsl:variable name="addr">
      <xsl:value-of select="translate(string(.),'¤','@')"/>
    </xsl:variable>
    <xsl:variable name="FieldName" select="substring-before($addr,'@')" />
    <!--name -->
    <xsl:variable name="Field2Temp" select="substring-after($addr,'@')" />
    <xsl:variable name="FieldStreet" select="substring-before($Field2Temp,'@')" />
    <!--street -->
    <xsl:variable name="Field3Temp" select="substring-after($Field2Temp,'@')" />
    <xsl:variable name="FieldCity" select="substring-before($Field3Temp,'@')" />
    <!--City -->
    <xsl:variable name="Field4Temp" select="substring-after($Field3Temp,'@')" />
    <xsl:variable name="FieldStzip" select="substring-before($Field4Temp,'@')" />
    <!--State and zip -->
    <xsl:variable name="FieldCountry" select="substring-after($Field4Temp,'@')" />
    <!--Country -->
    <StreetNmbr>
      <xsl:value-of select="$FieldStreet" />
    </StreetNmbr>
    <CityName>
      <xsl:value-of select="$FieldCity" />
    </CityName>
    <PostalCode>
      <xsl:choose>
        <xsl:when test="contains($FieldStzip,'P/')">
          <xsl:value-of select="substring-after($FieldStzip,' P/')" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-after($FieldStzip,' Z/')" />
        </xsl:otherwise>
      </xsl:choose>
    </PostalCode>
    <xsl:if test="substring($FieldStzip,1,1) != ' '">
      <StateProv>
        <xsl:choose>
          <xsl:when test="substring($FieldStzip,3,1) = ' '">
            <xsl:attribute name="StateCode">
              <xsl:value-of select="substring-before($FieldStzip,' ')"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring-before($FieldStzip,' ')"/>
          </xsl:otherwise>
        </xsl:choose>
      </StateProv>
    </xsl:if>
    <CountryName>
      <xsl:value-of select="$FieldCountry" />
    </CountryName>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Form of Payment       						                       -->
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
        <xsl:value-of select="$cardnum"/>
        <!-- 
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
        -->
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
    <DirectBill>
      <xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
    </DirectBill>
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
    <DirectBill>
      <xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
    </DirectBill>

    <xsl:if test="Amt != ''">
      <xsl:variable name="amt">
        <xsl:value-of select="Amt"/>
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="number(substring($amt,1,1))">
          <PaymentAmount>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate($amt,'.','')"/>
            </xsl:attribute>
            <xsl:if test="not(number(substring($amt,1,1)))">
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="substring($amt,1,3)"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="DecimalPlaces">
              <xsl:choose>
                <xsl:when test="contains($amt,'.')">2</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </PaymentAmount>
        </xsl:when>
        <xsl:when test="number(substring($amt,4,1))">
          <PaymentAmount>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(substring($amt,4),'.','')"/>
            </xsl:attribute>
            <xsl:if test="not(number(substring($amt,1,1)))">
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="substring($amt,1,3)"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="DecimalPlaces">
              <xsl:choose>
                <xsl:when test="contains($amt,'.')">2</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </PaymentAmount>
        </xsl:when>
      </xsl:choose>
    </xsl:if>

    <TPA_Extensions>
      <xsl:attribute name="FOPType">CHECK</xsl:attribute>
    </TPA_Extensions>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						Seats Processing		  				           -->
  <!--************************************************************************************-->
  <xsl:template match="SeatSeg">
    <xsl:variable name="FltSegNum">
      <xsl:value-of select="FltSegNum"/>
    </xsl:variable>
    <xsl:variable name="StartAirp">
      <xsl:value-of select="StartAirp"/>
    </xsl:variable>
    <xsl:variable name="EndAirp">
      <xsl:value-of select="EndAirp"/>
    </xsl:variable>
    <xsl:apply-templates select="following-sibling::SeatAssignment[1]">
      <xsl:with-param name="FltSegNum">
        <xsl:value-of select="$FltSegNum"/>
      </xsl:with-param>
      <xsl:with-param name="StartAirp">
        <xsl:value-of select="$StartAirp"/>
      </xsl:with-param>
      <xsl:with-param name="EndAirp">
        <xsl:value-of select="$EndAirp"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="SeatAssignment">
    <xsl:param name="FltSegNum"/>
    <xsl:param name="StartAirp"/>
    <xsl:param name="EndAirp"/>
    <SeatRequest>
      <xsl:if test="Locn != ''">
        <xsl:attribute name="SeatNumber">
          <xsl:value-of select="Locn"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="AttribAry/Attrib='A'">
          <xsl:attribute name="SeatPreference">A</xsl:attribute>
        </xsl:when>
        <xsl:when test="AttribAry/Attrib='W'">
          <xsl:attribute name="SeatPreference">W</xsl:attribute>
        </xsl:when>
        <xsl:when test="AttribAry/Attrib!=''">
          <xsl:attribute name="SeatPreference">
            <xsl:value-of select="AttribAry/Attrib"/>
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <xsl:attribute name="SmokingAllowed">false</xsl:attribute>
      <xsl:attribute name="Status">
        <xsl:value-of select="Status"/>
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:value-of select="AbsNameNum"/>
      </xsl:attribute>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:value-of select="$FltSegNum" />
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$StartAirp"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$EndAirp"/>
        </xsl:attribute>
      </ArrivalAirport>
    </SeatRequest>
    <xsl:apply-templates select="following-sibling::*[1][AbsNameNum != '']">
      <xsl:with-param name="FltSegNum">
        <xsl:value-of select="$FltSegNum"/>
      </xsl:with-param>
      <xsl:with-param name="StartAirp">
        <xsl:value-of select="$StartAirp"/>
      </xsl:with-param>
      <xsl:with-param name="EndAirp">
        <xsl:value-of select="$EndAirp"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Special Service Request (SSR) Processing				    -->
  <!--************************************************************************************-->
  <xsl:template match="ProgramaticSSR">
    <SpecialServiceRequest>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:value-of select="SegNum" />
      </xsl:attribute>
      <xsl:apply-templates select="AppliesToAry/AppliesTo" />
      <xsl:attribute name="SSRCode">
        <xsl:value-of select="SSRCode" />
      </xsl:attribute>
      <xsl:if test="../AirSeg[SegNum=current()/SegNum]/AirV">
        <Airline>
          <xsl:attribute name="Code">
            <xsl:value-of select="../AirSeg[SegNum=current()/SegNum]/AirV" />
          </xsl:attribute>
          <xsl:attribute name="CodeContext">
            <xsl:value-of select="Status"/>
          </xsl:attribute>
        </Airline>
      </xsl:if>
      <xsl:if test="following-sibling::*[1]/Text!=''">
        <Text>
          <xsl:value-of select="following-sibling::ProgramaticSSRText[1]/Text" />
        </Text>
      </xsl:if>
    </SpecialServiceRequest>
  </xsl:template>

  <xsl:template match="AppliesTo">
    <xsl:attribute name="TravelerRefNumberRPHList">
      <xsl:value-of select="LNameNum" />
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="NonProgramaticSSR">
    <SpecialServiceRequest>
      <xsl:attribute name="SSRCode">
        <xsl:value-of select="SSRCode" />
      </xsl:attribute>
      
      <xsl:choose>
        <xsl:when test="SSRCode = 'CTCR' or SSRCode = 'CTCE' or SSRCode = 'CTCM'">
          <xsl:variable name="paxFull" select="substring-after(SSRText,'-')" />
          <xsl:variable name="pax" select="substring-after($paxFull,'/')" />
          <xsl:variable name="paxNum" select="/PNRBFManagement_53/PNRBFRetrieve/FNameInfo[FName = $pax]/AbsNameNum" />
          <xsl:attribute name="TravelerRefNumberRPHList">
            <xsl:value-of select="$paxNum" />
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>
      
      <xsl:if test="Vnd!=''">
        <Airline>
          <xsl:attribute name="Code">
            <xsl:value-of select="Vnd" />
          </xsl:attribute>
          <xsl:attribute name="CodeContext">
            <xsl:value-of select="Status"/>
          </xsl:attribute>
        </Airline>
      </xsl:if>
      
      
      
      <Text>
        <xsl:value-of select="SSRText" />
      </Text>
    </SpecialServiceRequest>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Other Service Requests (OSI) Processing			           -->
  <!--************************************************************************************-->
  <xsl:template match="OSI">
    <OtherServiceInformation>
      <Airline>
        <xsl:attribute name="Code">
          <xsl:value-of select="OSIV" />
        </xsl:attribute>
      </Airline>
      <Text>
        <xsl:value-of select="OSIMsg" />
      </Text>
    </OtherServiceInformation>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						General Remarks			                               -->
  <!--************************************************************************************-->
  <xsl:template match="GenRmkInfo">
    <Remark>
      <xsl:attribute name="Category">
        <xsl:choose>
          <xsl:when test="contains(GenlRmkQual, 'H')">Historical</xsl:when>
          <xsl:when test="contains(GenlRmkQual, 'C')">Confidential</xsl:when>
          <xsl:otherwise>General</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="GenRmkNum" />
      </xsl:attribute>
      <!-- <xsl:value-of select="GenlRmkQual"/> -->
      <xsl:variable name="rmk1">
        <xsl:choose>
          <xsl:when test="contains(GenRmk,'*40')">
            <xsl:value-of select="substring-before(GenRmk,'*40')"/>
            <xsl:text>@</xsl:text>
            <xsl:value-of select="substring-after(GenRmk,'*40')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="GenRmk"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:value-of select="$rmk1"/>
    </Remark>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						 Itinerary Remarks							    -->
  <!--************************************************************************************-->
  <xsl:template match="ItinRmk">
    <SpecialRemark>
      <xsl:choose>
        <xsl:when test="../AirSeg[SegNum=current()/SegNum]">
          <xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:value-of select="SegNum" />
            </xsl:attribute>
          </FlightRefNumber>
          <Text>
            <xsl:value-of select="Rmk" />
          </Text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
          <Text>
            <xsl:value-of select="Rmk" />
          </Text>
        </xsl:otherwise>
      </xsl:choose>
    </SpecialRemark>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						 Vendor Remarks							    -->
  <!--************************************************************************************-->
  <xsl:template match="VndRmk">
    <SpecialRemark>
      <xsl:attribute name="RemarkType">Vendor</xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="RmkNum"/>
      </xsl:attribute>
      <Text>
        <xsl:value-of select="Rmk" />
      </Text>
      <Vendor>
        <xsl:attribute name="TravelSector">
          <xsl:choose>
            <xsl:when test="VType='A'">Airline</xsl:when>
            <xsl:when test="VType='H'">Hotel</xsl:when>
            <xsl:when test="VType='C'">Car</xsl:when>
            <xsl:when test="VType='L'">Leisure</xsl:when>
            <xsl:otherwise>Other</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Code">
          <xsl:value-of select="Vnd"/>
        </xsl:attribute>
      </Vendor>
      <RemarkDateTime>
        <xsl:value-of select="substring(DtStamp,1,4)"/>
        <xsl:value-of select="concat('-',substring(DtStamp,5,2))"/>
        <xsl:value-of select="concat('-',substring(DtStamp,7),'T')"/>
        <xsl:value-of select="substring(TmStamp,1,2)"/>
        <xsl:value-of select="concat(':',substring(TmStamp,3),':00')"/>
      </RemarkDateTime>
      <RemarkOrigin>
        <xsl:choose>
          <xsl:when test="RmkType='I'">Vendor</xsl:when>
          <xsl:otherwise>Agent</xsl:otherwise>
        </xsl:choose>
      </RemarkOrigin>
    </SpecialRemark>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Document Invoice	Remarks							    -->
  <!--************************************************************************************-->
  <xsl:template match="InvoiceRmk"  mode="invrmk">
    <SpecialRemark>
      <xsl:attribute name="RemarkType">Invoice</xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="ItemNum"/>
      </xsl:attribute>
      <Text>
        <xsl:value-of select="Rmk" />
      </Text>
    </SpecialRemark>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			Accounting line							    -->
  <!--************************************************************************************-->
  <xsl:template match="InvoiceRmk" mode="accline">
    <AccountingLine>
      <xsl:choose>
        <xsl:when test="contains(Rmk,'FT-')">
          <xsl:value-of select="translate(substring-before(substring-after(substring-after(Rmk,'FT-'),'AN'),'-'),'@','.')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="translate(substring-after(Rmk,'AN'),'@','.')"/>
        </xsl:otherwise>
      </xsl:choose>
    </AccountingLine>
  </xsl:template>

  <!-- ***********************************************************-->
  <!--  Vendor Record locators/Ticketing info      	     -->
  <!-- ********************************************************** -->
  <xsl:template match="GenPNRInfo">
    <xsl:if test="../TAUTkArrangement or ../TkArrangement or ../../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo/TkNum or TkNumExistInd='Y'">
      <Ticketing>
        <xsl:choose>
          <xsl:when test="../TAUTkArrangement">
            <xsl:attribute name="TicketType">
              <xsl:choose>
                <xsl:when test="../../DocProdDisplayStoredQuote/DocumentSelect/ETInd = 'P'">Paper</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/DocumentSelect/ETInd = 'Y'">eTicket</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/ExtendedQuoteInformation/ETkInd= 'N'">Paper</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/ExtendedQuoteInformation/ETkInd= 'Y'">eTicket</xsl:when>
                <xsl:when test="ETkDataExistInd = 'Y'">eTicket</xsl:when>
                <xsl:otherwise>Paper</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <TicketAdvisory>
              <xsl:value-of select="../TAUTkArrangement/QTAUDt"/>
              <xsl:text>/</xsl:text>
              <xsl:value-of select="../TAUTkArrangement/Text"/>
            </TicketAdvisory>
          </xsl:when>
          <xsl:when test="../TkArrangement">
            <xsl:attribute name="TicketType">
              <xsl:choose>
                <xsl:when test="../../DocProdDisplayStoredQuote/DocumentSelect/ETInd = 'P'">Paper</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/DocumentSelect/ETInd = 'Y'">eTicket</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/ExtendedQuoteInformation/ETkInd= 'N'">Paper</xsl:when>
                <xsl:when test="../../DocProdDisplayStoredQuote/ExtendedQuoteInformation/ETkInd= 'Y'">eTicket</xsl:when>
                <xsl:when test="ETkDataExistInd = 'Y'">eTicket</xsl:when>
                <xsl:otherwise>Paper</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <TicketAdvisory>
              <xsl:value-of select="../TkArrangement/Text"/>
            </TicketAdvisory>
          </xsl:when>
          <xsl:when test="../../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo/TkNum != ''">
            <xsl:choose>
              <xsl:when test="ETkDataExistInd = 'Y'">
                <xsl:attribute name="TicketType">eTicket</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="TicketType">Paper</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="TkNumExistInd='Y'">
              <xsl:attribute name="eTicketNumber">
                <xsl:value-of select="../../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo/TkNum" />
              </xsl:attribute>
            </xsl:if>
          </xsl:when>
        </xsl:choose>
      </Ticketing>
    </xsl:if>
  </xsl:template>
  <!-- ***********************************************************-->
  <!-- ***                               Tier 3                                  *-->
  <!-- ***********************************************************-->
  <!--************************************************************************************-->
  <!--						Passenger 	Names 							    -->
  <!--************************************************************************************-->
  <xsl:template name="buildnames">
    <xsl:param name="PsgrsTot" />
    <xsl:param name="PsgrsNum" />
    <xsl:if test="$PsgrsTot > 0">
      <xsl:variable name="ItemNo">
        <xsl:value-of select="LNameNum" />
      </xsl:variable>

      <xsl:variable name="absNum">
        <xsl:value-of select="../NameRmkInfo[LNameNum=$ItemNo and PsgrNum=$PsgrsNum]/AbsNameNum" />
      </xsl:variable>

      <CustomerInfo>
        <xsl:attribute name="RPH">
          <xsl:choose>
            <xsl:when test="../FNameInfo[PsgrNum=$PsgrsNum and AbsNameNum=$absNum]">
              <xsl:value-of select="$absNum" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$PsgrsNum" />
            </xsl:otherwise>
          </xsl:choose>

        </xsl:attribute>
        <Customer>
          <xsl:if test="../GenPNRInfo/OwningCRS = '1G'">
            <xsl:if test="../NameRmkInfo[LNameNum=$ItemNo]/NameRmk != ''">
              <xsl:if test="string-length(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk) > 5">
                <xsl:if test="contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'JAN') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'FEB') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'MAR') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'APR') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'MAY') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'JUN') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'JUL') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'AUG') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'SEP') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'OCT') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'NOV') or contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk,'DEC')">
                  <xsl:attribute name="BirthDate">
                    <xsl:call-template name="bdt">
                      <xsl:with-param name="bdt">
                        <!-- <NameRmk>23FEB20</NameRmk> -->
                        <xsl:choose>
                          <xsl:when test="contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk, 'INF')">
                            <xsl:value-of select="substring-after(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk, 'INF')" />
                          </xsl:when>
                          <xsl:when test="contains(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk, 'CHD')">
                            <xsl:value-of select="substring-after(../NameRmkInfo[LNameNum=$ItemNo]/NameRmk, 'CHD')" />
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="../NameRmkInfo[LNameNum=$ItemNo]/NameRmk" />
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:with-param>
                    </xsl:call-template>
                  </xsl:attribute>
                </xsl:if>
              </xsl:if>
            </xsl:if>
          </xsl:if>
          <PersonName>
            <xsl:choose>
              <xsl:when test="../GenPNRInfo/OwningCRS = '1V'">
                <xsl:attribute name="NameType">
                  <xsl:value-of select="../NameRmkInfo[LNameNum=$ItemNo]/NameRmk" />
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="NameType">
                  <xsl:choose>
                    <xsl:when test="../NameRmkInfo[LNameNum=$ItemNo and PsgrNum=$PsgrsNum]/NameRmk != '' and NameType = ''">CHD</xsl:when>
                    <xsl:when test="../NameRmkInfo[LNameNum=$ItemNo and PsgrNum=$PsgrsNum]/NameRmk != '' and NameType = 'I'">INF</xsl:when>
                    <xsl:otherwise>ADT</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            <GivenName>
              <xsl:value-of select="following-sibling::FNameInfo[position()=$PsgrsNum]/FName" />
            </GivenName>
            <Surname>
              <xsl:value-of select="LName" />
            </Surname>
          </PersonName>
          <xsl:apply-templates select="../PhoneInfo" mode="Phone" />
          <xsl:apply-templates select="../Email" />
          <xsl:apply-templates select="../AddrInfo" />
          <xsl:apply-templates select="../DeliveryAddrInfo" />
          <xsl:variable name="lnamenum">
            <xsl:value-of select="LNameNum"/>
          </xsl:variable>
          <xsl:apply-templates select="../FreqCustInfo[LNameNum = $lnamenum]" />
        </Customer>
      </CustomerInfo>
      <xsl:call-template name="buildnames">
        <xsl:with-param name="PsgrsTot">
          <xsl:value-of select="$PsgrsTot - 1" />
        </xsl:with-param>
        <xsl:with-param name="PsgrsNum">
          <xsl:value-of select="$PsgrsNum + 1" />
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="bdt">
    <xsl:param name="bdt" />
    <xsl:variable name="bd">
      <xsl:choose>
        <xsl:when test="string-length($bdt) > 7">
          <xsl:value-of select="substring($bdt,string-length($bdt) - 6,7)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$bdt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="substring($bd,6,2) > 89">19</xsl:when>
      <xsl:otherwise>20</xsl:otherwise>
    </xsl:choose>
    <xsl:value-of select="substring($bd,6,2)" />
    <xsl:text>-</xsl:text>
    <xsl:choose>
      <xsl:when test="substring($bd,3,3) = 'JAN'">01</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'FEB'">02</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'MAR'">03</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'APR'">04</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'MAY'">05</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'JUN'">06</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'JUL'">07</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'AUG'">08</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'SEP'">09</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'OCT'">10</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'NOV'">11</xsl:when>
      <xsl:when test="substring($bd,3,3) = 'DEC'">12</xsl:when>
    </xsl:choose>
    <xsl:text>-</xsl:text>
    <xsl:value-of select="substring($bd,1,2)" />
  </xsl:template>

  <xsl:template match="FreqCustInfo">
    <CustLoyalty>
      <xsl:attribute name="ProgramID">
        <xsl:value-of select="FreqCustV"/>
      </xsl:attribute>
      <xsl:if test="FreqCustStatus != ''">
        <xsl:attribute name="LoyalLevel">
          <xsl:value-of select="FreqCustStatus"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="MembershipID">
        <xsl:value-of select="FreqCustNum"/>
      </xsl:attribute>
    </CustLoyalty>
  </xsl:template>

  <xsl:template match="LNameInfo" mode="Inf">
    <CustomerInfos>
      <xsl:attribute name="RPH">
        <xsl:value-of select="LNameNum" />
      </xsl:attribute>
      <Customer>
        <PersonName>
          <xsl:attribute name="NameType">INF</xsl:attribute>
          <Surname>
            <xsl:value-of select="LName" />
          </Surname>
          <GivenName>
            <xsl:value-of select="following-sibling::FNameInfo[1]/FName" />
          </GivenName>
        </PersonName>
      </Customer>
    </CustomerInfos>
  </xsl:template>
  <!-- ***********************************************************-->
  <!--  Vendor Record locators        	                         -->
  <!-- ********************************************************** -->
  <xsl:template match="RecLocInfo">
    <RecordLocator>
      <xsl:variable name="zeroes">000</xsl:variable>
      <xsl:variable name="Airline">
        <xsl:value-of select="Vnd" />
      </xsl:variable>
      <xsl:for-each select="//AirSeg">
        <xsl:if test="AirV=$Airline">
          <xsl:attribute name="RPH">
            <xsl:value-of select="SegNum" />
          </xsl:attribute>
        </xsl:if>
      </xsl:for-each>
      <xsl:attribute name="BookingReferenceID">
        <xsl:value-of select="RecLoc" />
      </xsl:attribute>
      <xsl:attribute name="DateBooked">
        <xsl:variable name="timestmp">
          <xsl:value-of select="substring(string($zeroes),1,4-string-length(TmStamp))" />
          <xsl:value-of select="TmStamp" />
        </xsl:variable>
        <xsl:value-of select="substring(DtStamp,1,4)" />
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(DtStamp,5,2)" />
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(DtStamp,7,2)" />
        <xsl:text>T</xsl:text>
        <xsl:value-of select="substring($timestmp,1,2)" />
        <xsl:text>:</xsl:text>
        <xsl:value-of select="substring($timestmp,3,2)" />
        <xsl:text>:00</xsl:text>
      </xsl:attribute>
      <MarketingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="Vnd" />
        </xsl:attribute>
      </MarketingAirline>
    </RecordLocator>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- TourCode Remarks	   	                                          -->
  <!-- ************************************************************** -->
  <xsl:template match="DocProdDisplayStoredQuote" mode="tourcode">
    <SpecialRemark>
      <xsl:attribute name="RPH">
        <xsl:value-of select="FareNumInfo/FareNumAry/FareNum"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="TourCode/Rules">
          <xsl:attribute name="RemarkType">TourCode</xsl:attribute>
          <TravelerRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="AgntEnteredPsgrDescInfo/ApplesToAry/AppliesTo/AbsNameNum">
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </TravelerRefNumber>

          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="SegRelatedInfo">
                <xsl:variable name="rph">
                  <xsl:value-of select="RelSegNum"/>
                </xsl:variable>
                <xsl:if test="position() > 1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="$rph"/>
              </xsl:for-each>
            </xsl:attribute>
          </FlightRefNumber>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">TourCode</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="TourCode/Rules"/>
      </Text>
    </SpecialRemark>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- Endorsement Remarks	   	                                          -->
  <!-- ************************************************************** -->
  <xsl:template match="DocProdDisplayStoredQuote" mode="Endorsement">

    <xsl:if test="EndorsementBox/Endors1!=''">
      <xsl:apply-templates select="." mode="endo">
        <xsl:with-param name="endo" select="EndorsementBox/Endors1"></xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>

    <xsl:if test="EndorsementBox/Endors2!=''">
      <xsl:apply-templates select="." mode="endo">
        <xsl:with-param name="endo" select="EndorsementBox/Endors2"></xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>

    <xsl:if test="EndorsementBox/Endors3!=''">
      <xsl:apply-templates select="." mode="endo">
        <xsl:with-param name="endo" select="EndorsementBox/Endors3"></xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>

    <xsl:if test="PNRFareDetail/FareOwnCarrier">
      <xsl:apply-templates select="." mode="controllingCarrier" />
    </xsl:if>

  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="EndorsementInfo">
    <xsl:variable name="trav" >
      <xsl:for-each select="AgntEnteredPsgrDescInfo/ApplesToAry/AppliesTo/AbsNameNum">
        <xsl:if test="position() > 1">
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="text()" />
        </xsl:call-template>
      </xsl:for-each>

      <!--
      <xsl:for-each select="../AssocSegs/SegNumAry">
        <xsl:if test="position() > 1">
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="SegNum" />
        </xsl:call-template>
      </xsl:for-each>
      -->

    </xsl:variable>
    <xsl:variable name="segs">
      <!--
      <xsl:for-each select="SegRelatedInfo">
        <xsl:variable name="rph">
          <xsl:value-of select="RelSegNum"/>
        </xsl:variable>
        <xsl:if test="position() > 1">
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:value-of select="$rph"/>
      </xsl:for-each>
      -->

      <xsl:for-each select="SegRelatedInfo[UniqueKey='1']">
        <xsl:if test="position() > 1">
          <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="RelSegNum" />
        </xsl:call-template>
      </xsl:for-each>
    </xsl:variable>

    <xsl:for-each select="InfoMsg[MsgType='1' and UniqueKey='0001']">
      <SpecialRemark>
        <xsl:attribute name="RPH">
          <xsl:value-of select="FareNumInfo/FareNumAry/FareNum"/>
        </xsl:attribute>

        <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
        <TravelerRefNumber>
          <xsl:attribute name="RPH" >
            <xsl:value-of select="$trav"/>
          </xsl:attribute>
        </TravelerRefNumber>

        <FlightRefNumber>
          <xsl:attribute name="RPH">
            <xsl:value-of select="$segs"/>
          </xsl:attribute>
        </FlightRefNumber>

        <Text>
          <xsl:value-of select="Text"/>
        </Text>
      </SpecialRemark>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="endo">
    <xsl:param name="endo" />
    <SpecialRemark>
      <xsl:attribute name="RPH">
        <xsl:value-of select="FareNumInfo/FareNumAry/FareNum"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="EndorsementBox/Endors1!=''">
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
          <TravelerRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="AgntEnteredPsgrDescInfo/ApplesToAry/AppliesTo/AbsNameNum">
                <xsl:value-of select="text()"/>
              </xsl:for-each>
            </xsl:attribute>
          </TravelerRefNumber>

          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="SegRelatedInfo">
                <xsl:variable name="rph">
                  <xsl:value-of select="RelSegNum"/>
                </xsl:variable>
                <xsl:if test="position() > 1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="$rph"/>
              </xsl:for-each>
            </xsl:attribute>
          </FlightRefNumber>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="$endo"/>
      </Text>
    </SpecialRemark>
  </xsl:template>

  <xsl:template match="DocProdDisplayStoredQuote" mode="controllingCarrier">
    <xsl:variable name="cc" select="PNRFareDetail" />
    <xsl:variable name="gqd" select="GenQuoteDetails[1]" />
    <xsl:variable name="segs" select="SegRelatedInfo[UniqueKey=$gqd/UniqueKey]" />
    <xsl:variable name="air" select="//PNRBFRetrieve/AirSeg" />

    <xsl:variable name="lCarr">
      <xsl:for-each select="$cc/FareOwnCarrier[generate-id() = generate-id(key('conCarr',.)[1])]">
        <xsl:if test=". !=''">
          <xsl:value-of select="concat(., ',')"/>
        </xsl:if>
      </xsl:for-each>
    </xsl:variable>

    <xsl:variable name="elems">
      <xsl:call-template name="tokenizeString">
        <xsl:with-param name="list" select="$lCarr"/>
        <xsl:with-param name="delimiter" select="','"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:for-each select="msxsl:node-set($elems)/elem">

      <xsl:variable name="al">
        <xsl:value-of select="text()"/>
      </xsl:variable>

      <SpecialRemark>
        <xsl:attribute name="RPH">
          <xsl:value-of select="$al"/>
        </xsl:attribute>
        <xsl:attribute name="RemarkType">CC</xsl:attribute>
        
        <FlightRefNumber>
          <xsl:attribute name="RPH">
            <xsl:for-each select="$segs">
              <xsl:variable name="rph">
                <xsl:value-of select="RelSegNum"/>
              </xsl:variable>
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:value-of select="$rph"/>
            </xsl:for-each>
          </xsl:attribute>
        </FlightRefNumber>
        
        <xsl:variable name="fltPath">
          <xsl:for-each select="$segs/RelSegNum">
            <xsl:variable name="sN" select="node()[1]" />
            <xsl:variable name="bfInfo" select="$air[SegNum=$sN]" />
            <xsl:variable name="port">
              <xsl:value-of select="concat($bfInfo/StartAirp,$bfInfo/EndAirp)"/>
            </xsl:variable>

            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="$port"/>
          </xsl:for-each>
        </xsl:variable>

        <Text>
          <xsl:call-template name="string-trim">
            <xsl:with-param name="string" select="concat($fltPath,' -/', $al)" />
          </xsl:call-template>
        </Text>
      </SpecialRemark>

    </xsl:for-each>
  </xsl:template>

  <!--************************************************************************************-->
  <!--					Hotel Optional Fields				   	                        -->
  <!--************************************************************************************-->
  <xsl:template match="Fld" mode="Htl">
    <xsl:choose>
      <xsl:when test="ID = 'BS'">
        <BookingSource>
          <xsl:value-of select="Contents" />
        </BookingSource>
      </xsl:when>
      <xsl:when test="ID = 'SI'">
        <SupplementalInfo>
          <xsl:value-of select="Contents" />
        </SupplementalInfo>
      </xsl:when>
      <xsl:when test="ID = 'AC'">
        <AlternateCurrency>
          <xsl:value-of select="Contents" />
        </AlternateCurrency>
      </xsl:when>
      <xsl:when test="ID = 'AV'">
        <AdvancedDeposit>
          <xsl:value-of select="Contents" />
        </AdvancedDeposit>
      </xsl:when>
      <xsl:when test="ID = 'RQ'">
        <RateQuoted>
          <xsl:value-of select="Contents" />
        </RateQuoted>
      </xsl:when>
      <xsl:when test="ID = 'RR'">
        <RateRequested>
          <xsl:value-of select="Contents" />
        </RateRequested>
      </xsl:when>
      <xsl:when test="ID = 'RA'">
        <RollawayAdult>
          <xsl:value-of select="Contents" />
        </RollawayAdult>
      </xsl:when>
      <xsl:when test="ID = 'RD'">
        <RollawayChild>
          <xsl:value-of select="Contents" />
        </RollawayChild>
      </xsl:when>
      <xsl:when test="ID = 'CR'">
        <Crib>
          <xsl:value-of select="Contents" />
        </Crib>
      </xsl:when>
      <xsl:when test="ID = 'EX'">
        <ExtraAdult>
          <xsl:value-of select="Contents" />
        </ExtraAdult>
      </xsl:when>
      <xsl:when test="ID = 'EC'">
        <ExtraChild>
          <xsl:value-of select="Contents" />
        </ExtraChild>
      </xsl:when>
      <xsl:when test="ID = 'VC'">
        <MerchantCurrency>
          <xsl:value-of select="Contents" />
        </MerchantCurrency>
      </xsl:when>
      <xsl:when test="ID = 'RT'">
        <OverrideCorporateRate>
          <xsl:value-of select="Contents" />
        </OverrideCorporateRate>
      </xsl:when>
      <xsl:when test="ID = 'GT'">
        <Guarantee>
          <xsl:if test="contains(Contents, 'EXP')">
            <GuaranteesAccepted>
              <GuaranteeAccepted>
                <xsl:variable name="Contents">
                  <xsl:choose>
                    <xsl:when test="substring(Contents,1,4)='DPST'">
                      <xsl:value-of select="substring(Contents,5)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring(Contents,1)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <PaymentCard>
                  <xsl:attribute name="CardCode">
                    <xsl:value-of select="substring($Contents, 1, 2)" />
                  </xsl:attribute>
                  <xsl:attribute name="CardNumber">
                    <xsl:value-of select="substring-before(substring($Contents, 3, string-length($Contents) - 4), 'EXP')" />
                  </xsl:attribute>
                  <xsl:attribute name="ExpireDate">
                    <xsl:value-of select="substring(substring-after($Contents,'EXP'), 3, 2)" />
                    <xsl:value-of select="substring(substring-after($Contents,'EXP'), 1, 2)" />
                  </xsl:attribute>
                </PaymentCard>
              </GuaranteeAccepted>
            </GuaranteesAccepted>
          </xsl:if>
          <xsl:if test="contains(Contents, 'AGT')">
            <GuaranteeDescription>
              <Text>
                <xsl:value-of select="Contents"/>
              </Text>
            </GuaranteeDescription>
          </xsl:if>
        </Guarantee>
      </xsl:when>
      <xsl:when test="ID = 'DP'">
        <DepositPayment>
          <AcceptedPayments>
            <AcceptedPayment>
              <xsl:if test="contains(Contents, 'EXP')">
                <PaymentCard>
                  <xsl:attribute name="CardCode">
                    <xsl:value-of select="substring(Contents, 1, 2)" />
                  </xsl:attribute>
                  <xsl:attribute name="CardNumber">
                    <xsl:value-of select="substring-before(substring(Contents, 3, string-length(Contents) - 4), 'EXP')" />
                  </xsl:attribute>
                  <xsl:attribute name="ExpireDate">
                    <xsl:value-of select="substring(substring-after(Contents,'EXP'), 3, 2)" />
                    <xsl:value-of select="substring(substring-after(Contents,'EXP'), 1, 2)" />
                  </xsl:attribute>
                </PaymentCard>
              </xsl:if>
            </AcceptedPayment>
          </AcceptedPayments>
        </DepositPayment>
      </xsl:when>
      <xsl:when test="ID = 'TN'">
        <TourNumber>
          <xsl:value-of select="Contents" />
        </TourNumber>
      </xsl:when>
      <xsl:when test="ID = 'RL'">
        <RoomLocation>
          <xsl:value-of select="Contents" />
        </RoomLocation>
      </xsl:when>
      <xsl:when test="ID = 'MP'">
        <MealPlan>
          <xsl:value-of select="Contents" />
        </MealPlan>
      </xsl:when>
      <xsl:when test="ID = 'CD'">
        <CorporateDiscount>
          <xsl:value-of select="Contents" />
        </CorporateDiscount>
      </xsl:when>
      <xsl:when test="ID = 'NL'">
        <GuestLastName>
          <xsl:value-of select="Contents" />
        </GuestLastName>
      </xsl:when>
      <xsl:when test="ID = 'NF'">
        <GuestFirstName>
          <xsl:value-of select="Contents" />
        </GuestFirstName>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="ErrText | HostApplicationError">
    <Error>
      <xsl:attribute name="Type">Galileo</xsl:attribute>
      <xsl:value-of select="Text" />
    </Error>
  </xsl:template>
  <!--*******************************************************************************-->
  <!--			Process Car Segments								    -->
  <!--*******************************************************************************-->
  <xsl:template match="CarSeg">
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="SegNum" />
      </xsl:attribute>
      <xsl:attribute name="Status">
        <xsl:value-of select="Status" />
      </xsl:attribute>
      <xsl:if test="Status!='HK'">
        <xsl:attribute name="IsPassive">true</xsl:attribute>
      </xsl:if>
      <Vehicle>
        <xsl:if test="ConfNum!=''">
          <ConfID>
            <xsl:attribute name="Type">8</xsl:attribute>
            <xsl:attribute name="ID">
              <xsl:value-of select="ConfNum" />
            </xsl:attribute>
          </ConfID>
        </xsl:if>
        <Vendor>
          <xsl:attribute name="Code">
            <xsl:value-of select="CarV" />
          </xsl:attribute>
        </Vendor>
        <VehRentalCore>
          <xsl:variable name="zeroes">0000</xsl:variable>
          <xsl:attribute name="PickUpDateTime">
            <xsl:variable name="deptime">
              <xsl:value-of select="format-number(StartTm,'0000')" />
            </xsl:variable>
            <xsl:value-of select="substring(StartDt,1,4)" />
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(StartDt,5,2)" />
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(StartDt,7,2)" />
            <xsl:text>T</xsl:text>
            <xsl:value-of select="substring($deptime,1,2)" />
            <xsl:text>:</xsl:text>
            <xsl:value-of select="substring($deptime,3,2)" />
            <xsl:text>:00</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="ReturnDateTime">
            <xsl:variable name="arrtime">
              <xsl:value-of select="format-number(EndTm,'0000')" />
            </xsl:variable>
            <xsl:value-of select="substring(EndDt,1,4)" />
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(EndDt,5,2)" />
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(EndDt,7,2)" />
            <xsl:text>T</xsl:text>
            <xsl:value-of select="substring($arrtime,1,2)" />
            <xsl:text>:</xsl:text>
            <xsl:value-of select="substring($arrtime,3,2)" />
            <xsl:text>:00</xsl:text>
          </xsl:attribute>
          <PickUpLocation>
            <xsl:choose>
              <xsl:when test="ActualStartPt !=''">
                <xsl:attribute name="LocationCode">
                  <xsl:value-of select="ActualStartPt" />
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="LocationCode">
                  <xsl:value-of select="Airp" />
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </PickUpLocation>
          <ReturnLocation>
            <xsl:choose>
              <xsl:when test="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']">
                <xsl:choose>
                  <xsl:when test="string-length(following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents) = '3'">
                    <xsl:attribute name="LocationCode">
                      <xsl:value-of select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents" />
                    </xsl:attribute>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="ActualStartPt !=''">
                    <xsl:attribute name="LocationCode">
                      <xsl:value-of select="ActualStartPt" />
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="LocationCode">
                      <xsl:value-of select="Airp" />
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </ReturnLocation>
        </VehRentalCore>
        <Veh>
          <xsl:attribute name="AirConditionInd">
            <xsl:choose>
              <xsl:when test="substring(CarType,4,1)='R'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="TransmissionType">
            <xsl:choose>
              <xsl:when test="substring(CarType,3,1)='A'">Automatic</xsl:when>
              <xsl:otherwise>Manual</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <VehType>
            <xsl:attribute name="VehicleCategory">
              <xsl:choose>
                <xsl:when test="substring(CarType,2,1)='B'">2-Door</xsl:when>
                <xsl:when test="substring(CarType,2,1)='C'">2 or 4-Door</xsl:when>
                <xsl:when test="substring(CarType,2,1)='D'">4-Door</xsl:when>
                <xsl:when test="substring(CarType,2,1)='S'">Sport</xsl:when>
                <xsl:when test="substring(CarType,2,1)='T'">Convertible</xsl:when>
                <xsl:when test="substring(CarType,2,1)='X'">Special</xsl:when>
                <xsl:when test="substring(CarType,2,1)='W'">Wagon</xsl:when>
                <xsl:when test="substring(CarType,2,1)='V'">Van</xsl:when>
                <xsl:when test="substring(CarType,2,1)='F'">Four- Wheel Drive</xsl:when>
                <xsl:when test="substring(CarType,2,1)='J'">All Terrain</xsl:when>
                <xsl:when test="substring(CarType,2,1)='P'">Pick-Up</xsl:when>
                <xsl:when test="substring(CarType,2,1)='L'">Limo</xsl:when>
                <xsl:when test="substring(CarType,2,1)='K'">Truck</xsl:when>
                <xsl:when test="substring(CarType,2,1)='R'">Recreational</xsl:when>
                <xsl:otherwise>4-Door</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:if test="substring(CarType,2,1)='B'">
              <xsl:attribute name="DoorCount">2</xsl:attribute>
            </xsl:if>
            <xsl:if test="substring(CarType,2,1)='C'">
              <xsl:attribute name="DoorCount">4</xsl:attribute>
            </xsl:if>
            <xsl:if test="substring(CarType,2,1)='D'">
              <xsl:attribute name="DoorCount">4</xsl:attribute>
            </xsl:if>
            <xsl:value-of select="CarType"/>
          </VehType>
          <VehClass>
            <xsl:attribute name="Size">
              <xsl:choose>
                <xsl:when test="substring(CarType,1,1)='M'">Mini</xsl:when>
                <xsl:when test="substring(CarType,1,1)='E'">Economy</xsl:when>
                <xsl:when test="substring(CarType,1,1)='C'">Compact</xsl:when>
                <xsl:when test="substring(CarType,1,1)='I'">Intermediate</xsl:when>
                <xsl:when test="substring(CarType,1,1)='S'">Standard</xsl:when>
                <xsl:when test="substring(CarType,1,1)='F'">Full Sizel</xsl:when>
                <xsl:when test="substring(CarType,1,1)='P'">Premium</xsl:when>
                <xsl:when test="substring(CarType,1,1)='L'">Luxury</xsl:when>
                <xsl:when test="substring(CarType,1,1)='X'">Special</xsl:when>
                <xsl:otherwise>All</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </VehClass>
        </Veh>
        <RentalRate>
          <xsl:if test="MileAllow !='' or DistUnitName!='' or RateType!=''">
            <RateDistance>
              <xsl:choose>
                <xsl:when test="MileAllow = 'UNL'">
                  <xsl:attribute name="Unlimited">true</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Unlimited">false</xsl:attribute>
                  <xsl:attribute name="Quantity">
                    <xsl:value-of select="MileAllow" />
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:if test="DistUnitName!=''">
                <xsl:attribute name="DistUnitName">
                  <xsl:choose>
                    <xsl:when test="MilesOrKm = 'K'">Km</xsl:when>
                    <xsl:when test="MilesOrKm = 'M'">Mile</xsl:when>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:if>
              <xsl:if test="RateType!=''">
                <xsl:attribute name="VehiclePeriodUnitName">
                  <xsl:choose>
                    <xsl:when test="RateType='H'">Hour</xsl:when>
                    <xsl:when test="RateType='E'">Weekend</xsl:when>
                    <xsl:when test="RateType='W'">Week</xsl:when>
                    <xsl:when test="RateType='M'">Month</xsl:when>
                    <xsl:otherwise>Day</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:if>
            </RateDistance>
          </xsl:if>
          <VehicleCharges>
            <xsl:if test="RateAmt !='' and RateAmt !='0' ">
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="RateAmt" />
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:choose>
                    <xsl:when test="Currency!=''">
                      <xsl:value-of select="Currency" />
                    </xsl:when>
                    <xsl:otherwise>USD</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="DecPos" />
                </xsl:attribute>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="GuaranteedInd">
                  <xsl:choose>
                    <xsl:when test="RateGuarInd = 'G'">true</xsl:when>
                    <xsl:otherwise>false</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="Description">
                  <xsl:choose>
                    <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'DY')">Daily Rate</xsl:when>
                    <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'WY')">Weekly Rate</xsl:when>
                    <xsl:otherwise>Rental Period Rate</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraHourRateAmt!=''">
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="ExtraHourRateAmt" />
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="Currency" />
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="DecPos" />
                </xsl:attribute>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Hour Rate</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraHourMileRateAmt!=''">
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="ExtraHourMileRateAmt" />
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="Currency" />
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="DecPos" />
                </xsl:attribute>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Hour Mile/Km Rate</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
                <Calculation>
                  <xsl:attribute name="UnitName">
                    <xsl:choose>
                      <xsl:when test="MilesOrKm = 'K'">Km</xsl:when>
                      <xsl:otherwise>Mile</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </Calculation>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraHourMileAllow !=''">
              <VehicleCharge>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Day Mile/Km Allowance</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
                <Calculation>
                  <xsl:attribute name="UnitName">
                    <xsl:choose>
                      <xsl:when test="MilesOrKm = 'K'">Km</xsl:when>
                      <xsl:otherwise>Mile</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:attribute name="Quantity">
                    <xsl:choose>
                      <xsl:when test="ExtraHourMileAllow='UNL'">99999</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ExtraHourMileAllow" />
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </Calculation>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraDayRateAmt!=''">
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="ExtraDayRateAmt" />
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="Currency" />
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="DecPos" />
                </xsl:attribute>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Day Rate</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraDayMileRateAmt!=''">
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="ExtraDayMileRateAmt" />
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="Currency" />
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="DecPos" />
                </xsl:attribute>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Day Mile/Km Rate</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
                <Calculation>
                  <xsl:attribute name="UnitName">
                    <xsl:choose>
                      <xsl:when test="MilesOrKm = 'K'">Km</xsl:when>
                      <xsl:otherwise>Mile</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </Calculation>
              </VehicleCharge>
            </xsl:if>
            <xsl:if test="ExtraDayMileAllow!=''">
              <VehicleCharge>
                <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                <xsl:attribute name="Description">Extra Day Mile/Km Allowance</xsl:attribute>
                <xsl:attribute name="IncludedInRate">false</xsl:attribute>
                <Calculation>
                  <xsl:attribute name="UnitName">
                    <xsl:choose>
                      <xsl:when test="MilesOrKm = 'K'">Km</xsl:when>
                      <xsl:otherwise>Mile</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:attribute name="Quantity">
                    <xsl:choose>
                      <xsl:when test="ExtraDayMileAllow='UNL'">99999</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ExtraDayMileAllow" />
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </Calculation>
              </VehicleCharge>
            </xsl:if>
          </VehicleCharges>
          <xsl:if test="RateCat!='' or RateType!=''">
            <RateQualifier>
              <xsl:if test="RateCat!=''">
                <xsl:attribute name="RateCategory">
                  <xsl:choose>
                    <xsl:when test="RateCat='A'">Association</xsl:when>
                    <xsl:when test="RateCat='B'">Business Standard</xsl:when>
                    <xsl:when test="RateCat='C'">Corporate</xsl:when>
                    <xsl:when test="RateCat='G'">Government</xsl:when>
                    <xsl:when test="RateCat='I'">Industry</xsl:when>
                    <xsl:when test="RateCat='K'">Package</xsl:when>
                    <xsl:when test="RateCat='P'">Promotional</xsl:when>
                    <xsl:when test="RateCat='R'">Credential</xsl:when>
                    <xsl:when test="RateCat='S'">Standard</xsl:when>
                    <xsl:when test="RateCat='U'">Consortium</xsl:when>
                    <xsl:when test="RateCat='V'">Convention</xsl:when>
                    <xsl:otherwise>No Rate Category</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:if>
              <xsl:attribute name="RateQualifier">
                <xsl:value-of select="RateCode" />
              </xsl:attribute>
              <xsl:attribute name="RatePeriod">
                <xsl:choose>
                  <xsl:when test="RateType='H'">Hourly</xsl:when>
                  <xsl:when test="RateType='E'">WeekendDay</xsl:when>
                  <xsl:when test="RateType='W'">Weekly</xsl:when>
                  <xsl:when test="RateType='M'">Monthly</xsl:when>
                  <xsl:when test="RateType='D'">Daily</xsl:when>
                  <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'DY')">Daily</xsl:when>
                  <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'WY')">Weekly</xsl:when>
                  <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'HY')">Hourly</xsl:when>
                  <xsl:when test="contains(following-sibling::CarSegOptFlds[1]/FldAry/Fld[ID = 'RG']/Contents, 'MY')">Monthly</xsl:when>
                  <xsl:otherwise>Other</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </RateQualifier>
          </xsl:if>
        </RentalRate>
        <xsl:if test="RateAmt != ''">
          <TotalCharge>
            <xsl:attribute name="RateTotalAmount">
              <xsl:value-of select="RateAmt" />
            </xsl:attribute>
            <xsl:attribute name="EstimatedTotalAmount">
              <xsl:value-of select="RateAmt" />
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="Currency!=''">
                  <xsl:value-of select="Currency" />
                </xsl:when>
                <xsl:otherwise>USD</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </TotalCharge>
        </xsl:if>
        <TPA_Extensions>
          <xsl:if test="../../CarSegSellText">
            <Rules>
              <xsl:for-each select="../../CarSegSellText/LineDescAry/LineDescInfo">
                <Text>
                  <xsl:value-of select="Txt"/>
                </Text>
              </xsl:for-each>
            </Rules>
          </xsl:if>
          <CarOptions>
            <xsl:for-each select="following-sibling::CarSegOptFlds[1]/FldAry/Fld">
              <xsl:apply-templates select="." mode="car" />
            </xsl:for-each>
          </CarOptions>
        </TPA_Extensions>
      </Vehicle>
    </Item>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Car Options Fields										-->
  <!--************************************************************************************-->
  <xsl:template match="Fld" mode="car">
    <xsl:choose>
      <xsl:when test="ID = 'BS'">
        <CarOption>
          <xsl:attribute name="Option">Booking Source</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'RQ'">
        <CarOption>
          <xsl:attribute name="Option">Quoted Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'RR'">
        <CarOption>
          <xsl:attribute name="Option">Requested Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'RG'">
        <CarOption>
          <xsl:attribute name="Option">Guaranteed Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'NL'">
        <CarOption>
          <xsl:attribute name="Option">Reservation Last Name</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'NF'">
        <CarOption>
          <xsl:attribute name="Option">Reservation First Name</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'RT'">
        <CarOption>
          <xsl:attribute name="Option">Corporate Rate Override</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'ID'">
        <CarOption>
          <xsl:attribute name="Option">Customer ID</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'GT'">
        <CarOption>
          <xsl:attribute name="Option">Payment Guarantee</xsl:attribute>
          <xsl:choose>
            <xsl:when test="contains(Contents, 'EXP')">
              <Text>
                <xsl:value-of select="substring(Contents, 1, 2)" />
                <xsl:value-of select="substring-before(substring(Contents, 3, string-length(Contents) - 4), 'EXP')" />
                <xsl:value-of select="substring(substring-after(Contents,'EXP'), 1, 2)" />
                <xsl:value-of select="substring(substring-after(Contents,'EXP'), 3, 2)" />
              </Text>
            </xsl:when>
          </xsl:choose>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'PU'">
        <CarOption>
          <xsl:attribute name="Option">Pick Up Location</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'AD'">
        <CarOption>
          <xsl:attribute name="Option">Customer Address</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'CD'">
        <CarOption>
          <xsl:attribute name="Option">Corporate Discount</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'SQ'">
        <CarOption>
          <xsl:attribute name="Option">Optional Equipment</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'PR'">
        <CarOption>
          <xsl:attribute name="Option">PrePayment Info</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'DL'">
        <CarOption>
          <xsl:attribute name="Option">Driver License</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'FT'">
        <CarOption>
          <xsl:attribute name="Option">Frequent Traveler Number</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'SI'">
        <CarOption>
          <xsl:attribute name="Option">Additional Info</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'DC'">
        <CarOption>
          <xsl:attribute name="Option">DropOff Charge</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'VC'">
        <CarOption>
          <xsl:attribute name="Option">Merchant Currency</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'AC'">
        <CarOption>
          <xsl:attribute name="Option">Alternate Currency</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'DO'">
        <CarOption>
          <xsl:attribute name="Option">DropOff Location</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="ID = 'TN'">
        <CarOption>
          <xsl:attribute name="Option">Tour Number</xsl:attribute>
          <Text>
            <xsl:value-of select="Contents" />
          </Text>
        </CarOption>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="NonAirSeg">
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="SegNum"/>
      </xsl:attribute>
      <xsl:attribute name="Status">
        <xsl:value-of select="Status"/>
      </xsl:attribute>
      <xsl:if test="Status!='HK'">
        <xsl:attribute name="IsPassive">true</xsl:attribute>
      </xsl:if>
      <General>
        <xsl:attribute name="Type">Unknown</xsl:attribute>
        <xsl:if test="StartDt!=''">
          <xsl:attribute name="Start">
            <xsl:value-of select="substring(StartDt,1,4)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(StartDt,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(StartDt,7,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="EndDt!=''">
          <xsl:attribute name="End">
            <xsl:value-of select="substring(EndDt,1,4)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(EndDt,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(EndDt,7,2)"/>
          </xsl:attribute>
        </xsl:if>
        <Description>
          <xsl:value-of select="Type"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Vnd"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="NumPersons"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="StartPt"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Text"/>
        </Description>
      </General>
    </Item>
  </xsl:template>
  <!-- -->
  <xsl:template match="DuePaidInfo">
    <UniqueRemark>
      <xsl:attribute name="RemarkType">
        <xsl:choose>
          <xsl:when test="Type='D'">DUE</xsl:when>
          <xsl:when test="Type='P'">PAID</xsl:when>
          <xsl:otherwise>TEXT</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="SegNum"/>
      </xsl:attribute>
      <xsl:value-of select="DuePaidTextInd"/>
      <xsl:text>/</xsl:text>
      <xsl:value-of select="substring(Dt,1,4)"/>
      <xsl:text>-</xsl:text>
      <xsl:value-of select="substring(Dt,5,2)"/>
      <xsl:text>-</xsl:text>
      <xsl:value-of select="substring(Dt,7,2)"/>
      <xsl:text>/</xsl:text>
      <xsl:value-of select="Text"/>
    </UniqueRemark>
  </xsl:template>
  <!--**********************************************************************************************-->
  <msxsl:script language="VisualBasic" implements-prefix="ttVB">
    <![CDATA[
Function FctArrDate(byval p_startDate as string, byval p_DateChange as double) as date
   	
    If IsDate(p_startDate) Then
        FctArrDate = DateAdd("d", p_DateChange, p_startDate)
    Else
        FctArrDate = p_startDate
    End If

End Function
]]>
  </msxsl:script>

  <!--
  ************************************************************************************
  					Issued Tickets				   	                        
  ************************************************************************************
  -->
  <xsl:template match="AdditionalPsgrFareInfo" mode="IssuedTicket">
    <xsl:variable name="tktPax" select="AbsNameNum" />
    <xsl:variable name="tkt" select="TkNum" />
    <xsl:variable name="tktType" select="TkType" />
    <xsl:variable name="tktStatus" select="Status" />
    <!-- 
     <xsl:variable name="tktInfo" select="concat(../TkArrangement/Text, ' I')" />
    -->

    <IssuedTicket>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:value-of select="AbsNameNum"/>
      </xsl:attribute>

      <xsl:if test="../AssocSegs/SegNumAry/SegNum != ''">
        <xsl:attribute name="FlightRefNumberRPHList">
          <xsl:for-each select="../AssocSegs/SegNumAry">
            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:call-template name="string-ltrim">
              <xsl:with-param name="string" select="SegNum" />
            </xsl:call-template>
          </xsl:for-each>
        </xsl:attribute>
      </xsl:if>

      <xsl:variable name="tktTime">
        <xsl:if test="../../PNRBFRetrieve/TkArrangement/Text != ''">
          <xsl:value-of select="substring-after(../../PNRBFRetrieve/TkArrangement/Text, ' ')"/>
        </xsl:if>
      </xsl:variable>

      <xsl:variable name="tktPCC">
        <xsl:if test="../ExtendedQuoteInformation/TkPCC != ''">
          <xsl:value-of select="../ExtendedQuoteInformation/TkPCC"/>
        </xsl:if>
      </xsl:variable>

      <xsl:variable name="tStatus">
        <xsl:if test="$tktStatus = 'T'">
          <xsl:text>OK</xsl:text>
        </xsl:if>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="$tktType = 'V'">
          <xsl:value-of select="concat('TV ', $tkt, '-', $tStatus, '/', $tktTime, '/', $tktPCC, '/', ' *VOID* ' , '/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat('TE ', $tkt, '-', $tStatus, '/', $tktTime, '/', $tktPCC, '/')"/>
        </xsl:otherwise>
      </xsl:choose>

    </IssuedTicket>

    <xsl:if test="//MIS_INF[contains(ORI_TIC_NUM,$tkt)]">
      <ExchangeDocument>
        <xsl:attribute name="TravelerRefNumberRPHList">
          <xsl:value-of select="AbsNameNum"/>
        </xsl:attribute>

        <xsl:if test="../AssocSegs/SegNumAry/SegNum != ''">
          <xsl:attribute name="FlightRefNumberRPHList">
            <xsl:for-each select="../AssocSegs/SegNumAry">
              <xsl:if test="position() > 1">
                <xsl:text> </xsl:text>
              </xsl:if>
              <xsl:call-template name="string-ltrim">
                <xsl:with-param name="string" select="SegNum" />
              </xsl:call-template>
            </xsl:for-each>
          </xsl:attribute>
        </xsl:if>

        <xsl:variable name="segs">
          <xsl:for-each select="../AssocSegs/SegNumAry">
            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:call-template name="string-ltrim">
              <xsl:with-param name="string" select="SegNum" />
            </xsl:call-template>
          </xsl:for-each>
        </xsl:variable>

        <xsl:attribute name="FlightRefNumberRPHList">
          <xsl:call-template name="string-rtrim">
            <xsl:with-param name="string" select="translate($segs, '-', ' ')" />
          </xsl:call-template>
        </xsl:attribute>

        <xsl:variable name="mcoN">
          <xsl:value-of select="$tkt"/>
          <!--<xsl:value-of select="MIS_INF/EXG_REI_INF/TIC_NUM"/>-->
        </xsl:variable>

        <xsl:variable name="mco">
          <xsl:choose>
            <xsl:when test="//MIS_INF/ORI_TIC_NUM">
              <xsl:value-of select="//MIS_INF/ORI_TIC_NUM"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="//MIS_INF/EXG_REI_INF/TIC_NUM"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="mcoFlag">
          <xsl:choose>
            <xsl:when test="//PNR_DH_INF/Line[@TicketNumber=concat('M',$mcoN)]">MCO</xsl:when>
            <xsl:otherwise>EX</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="//PNR_DHV_INF/Line[@TicketNumber=concat('M',$mcoN)]">
            <xsl:value-of select="concat($mcoFlag,' ', substring($mco,1,3),'-',substring($mco,4,20), '/',substring($mco,24), '-*VOID*')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($mcoFlag,' ', substring($mco,1,3),'-',substring($mco,4,20), '/',substring($mco,24), '-OK')"/>
          </xsl:otherwise>
        </xsl:choose>
      </ExchangeDocument>
    </xsl:if>

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
