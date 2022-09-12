<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
   AmadeusWS_LowFarePlus2RS.xsl 												
   ================================================================== 
   Date: 26 Jul 2016 - Rastko - fallback of added airline code in operating airline same ....	
   Date: 18 Jul 2016 - Rastko - added AirEquipType attribute in StopInfo element		
   Date: 18 Jul 2016 - Rastko - added airline code in operating airline same as marketing when not provided by Amadeus	
   Date: 05 Jul 2016 - Rastko - corrected mapping of some params related to MTK option	
   Date: 01 Apr 2016 - Rastko - added mapping for code share airline on codeShareType=L	
   Date: 04 Feb 2016 - Rastko - added support for free baggage allowance			
   Date: 22 Jul 2015 - Rastko - mapped to the new flight number element from Amadeus	
   Date: 01 Apr 2013 - Rastko - added BreakPoint attribute in FareInfo element		
   Date: 18 Dec 2012 - Rastko - added support for option 	MKT (2 one ways in response)	
   Date: 02 Oct 2012 - Rastko - added StopInfo information						
   Date: 29 Sep 2012 - Rastko - added support for option 	LFPLight				
   Date: 25 Sep 2012 - Rastko - use InfoSource to pass back number of seats available	
   Date: 24 Sep 2012 - Rastko - added action code mapping (number of seats available)	
   Date: 14 Sep 2012 - Rastko - simplified process for spead reason				
   Date: 17 Aug 2012 - Rastko - added specific decoding for Kulula airline			
   Date: 21 Mar 2012 - Rastko - removed the filtering of DL and KL flights			
   Date: 30 Nov 2011 - Shashin - added logic to remove private, published fare combinations	
   Date: 08 Oct 2011 - Rastko - added default ticket time limit value				
   Date: 16 Aug 2011 - Rastko - corrected slice and dice mapping				
   Date: 18 Apr 2011 - Rastko - added extra test for mapping WEB fares			
   Date: 14 Feb 2011 - Rastko - added new mapping for operating airline	 		
   Date: 19 Jan 2011 - Rastko	- added W web fares mapping			 		
   Date: 28 Oct 2010 - Rastko - always display number of stops					
   Date: 28 Oct 2010 - Rastko - removed checking flight black list				
   Date: 20 Apr 2010 - Rastko - added filtering of DL and KL code share flights			
   Date: 04 Jun 2010 - Rastko - corrected evaluation of Private attribute			
   ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="LFPLight">
    <xsl:value-of select="Fare_MasterPricerTravelBoardSearchReply/LFPLight"/>
  </xsl:variable>
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="Errors">
        <OTA_AirLowFareSearchPlusRS Version="1.001">
          <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
          <Errors>
            <xsl:apply-templates select="Errors/Error" mode="Error"/>
          </Errors>
        </OTA_AirLowFareSearchPlusRS>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="Fare_MasterPricerTravelBoardSearchReply"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Error" mode="Error">
    <Error Type="Amadeus">
      <xsl:value-of select="."/>
    </Error>
  </xsl:template>
  <xsl:template match="Fare_MasterPricerTravelBoardSearchReply">
    <OTA_AirLowFareSearchPlusRS>
      <xsl:attribute name="Version">1.001</xsl:attribute>
      <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
      <xsl:choose>
        <xsl:when test="errorMessage">
          <Errors>
            <Error Type="Amadeus">
              <xsl:attribute name="Code">
                <xsl:value-of select="errorMessage/applicationError/applicationErrorDetail/error"/>
              </xsl:attribute>
              <xsl:value-of select="errorMessage/errorMessageText/description"/>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:when test="not(recommendation)">
          <Errors>
            <Error Type="Traveltalk">
              <xsl:attribute name="Code">0001</xsl:attribute>
              <xsl:text>No selection matches search criteria</xsl:text>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:otherwise>
          <!--xsl:variable name="recommendation">
						<xsl:apply-templates select="recommendation"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$recommendation!=''"-->
          <Success/>
          <PricedItineraries>
            <!--xsl:apply-templates select="recommendation[1]">
                                  				 <xsl:with-param name="pos">1</xsl:with-param>
                              				</xsl:apply-templates-->
            <xsl:apply-templates select="recommendation"/>
          </PricedItineraries>
          <!--/xsl:when>
						<xsl:otherwise>
							<Errors>
								<Error Type="TripXML">
									<xsl:attribute name="Code">0100</xsl:attribute>
									<xsl:text>ALL FLIGHTS ARE ON BLACK LIST</xsl:text>
								</Error>
							</Errors>
						</xsl:otherwise>
					</xsl:choose-->
        </xsl:otherwise>
      </xsl:choose>
    </OTA_AirLowFareSearchPlusRS>
  </xsl:template>
  <!--****************************************************************************************-->
  <!--											    -->
  <!--****************************************************************************************-->
  <xsl:template match="recommendation">
    <xsl:param name="pos"/>
    <!--xsl:variable name="fm">
			<xsl:for-each select="segmentFlightRef/OD">
				<xsl:variable name="odpos"><xsl:value-of select="position()"/></xsl:variable>
				<xsl:for-each select="flightDetails">
					<xsl:variable name="posit"><xsl:value-of select="position()"/></xsl:variable>
					<xsl:variable name="airline"><xsl:value-of select="flightInformation/companyId/marketingCarrier"/></xsl:variable>
					<xsl:variable name="flight"><xsl:value-of select="flightInformation/flightNumber"/></xsl:variable>
					<xsl:variable name="class"><xsl:value-of select="../../../paxFareProduct[1]/fareDetails[position()=$odpos]/groupOfFares[position()=$posit]/productInformation/cabinProduct/rbd"/></xsl:variable>
					<xsl:variable name="ddate">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),1,2)"/>
					</xsl:variable>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable-->
    <!--xsl:variable name="cs">
			<xsl:for-each select="segmentFlightRef/OD">
				<xsl:variable name="odpos"><xsl:value-of select="position()"/></xsl:variable>
				<xsl:for-each select="flightDetails">
					<xsl:choose>
						<xsl:when test="flightInformation/companyId/marketingCarrier='DL' and flightInformation/companyId/operatingCarrier='KL'">1</xsl:when>
						<xsl:when test="flightInformation/companyId/marketingCarrier='KL' and flightInformation/companyId/operatingCarrier='DL'">1</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable-->
    <xsl:variable name="fp">
      <xsl:apply-templates select="paxFareProduct" mode="test"/>
    </xsl:variable>
    <xsl:choose>
      <!--xsl:when test="substring($fm,1,1)='1'">
			<xsl:apply-templates select="following-sibling::recommendation[1]">
                    		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
	                    </xsl:apply-templates>

			</xsl:when-->
      <!--xsl:when test="substring($cs,1,1)='1'">
			<xsl:apply-templates select="following-sibling::recommendation[1]">
                    		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
	                    </xsl:apply-templates>

			</xsl:when-->
      <xsl:when test="$fp='1' and $fp!='1'">
        <!--xsl:apply-templates select="following-sibling::recommendation[1]">
                    		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
	                    </xsl:apply-templates-->
      </xsl:when>
      <xsl:otherwise>
        <PricedItinerary>
          <xsl:attribute name="SequenceNumber">
            <xsl:value-of select="position()"/>
          </xsl:attribute>
          <AirItinerary>
            <xsl:attribute name="DirectionInd">
              <xsl:choose>
                <xsl:when test="segmentFlightRef/OD[1]/flightDetails[1]/flightInformation/location[1]/locationId = segmentFlightRef/OD[position()=last()]/flightDetails[position()=last()]/flightInformation/location[2]/locationId">Circle</xsl:when>
                <xsl:otherwise>OneWay</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <OriginDestinationOptions>
              <xsl:apply-templates select="segmentFlightRef/OD"/>
            </OriginDestinationOptions>
          </AirItinerary>
          <AirItineraryPricingInfo>
            <xsl:choose>
              <xsl:when test="paxFareProduct/fare[contains(pricingMessage/description,'PRIVATE RATES USED')]">
                <xsl:attribute name="PricingSource">Private</xsl:attribute>
              </xsl:when>
              <xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RP'">
                <xsl:attribute name="PricingSource">Published</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="PricingSource">Private</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
            <xsl:attribute name="ValidatingAirlineCode">
              <xsl:value-of select="paxFareProduct/paxFareDetail/codeShareDetails[transportStageQualifier='V']/company"/>
            </xsl:attribute>
            <xsl:variable name="Deci" select="substring-after(string(recPriceInfo/monetaryDetail[1]/amount),'.')"/>
            <xsl:variable name="NoDeci1" select="string-length($Deci)"/>
            <xsl:variable name="TotalAmount1" select="translate(string(recPriceInfo/monetaryDetail[1]/amount),'.,','')"/>
            <xsl:variable name="TaxTotal1">
              <xsl:choose>
                <xsl:when test="recPriceInfo/monetaryDetail[2]/amount">
                  <xsl:value-of select="translate(string(recPriceInfo/monetaryDetail[2]/amount),'.','')"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="NoDeci">
              <xsl:value-of select="'2'"/>
            </xsl:variable>
            <xsl:variable name="TotalAmount">
              <xsl:choose>
                <xsl:when test="$NoDeci1='0'">
                  <xsl:value-of select="concat($TotalAmount1,'00')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$TotalAmount1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="TaxTotal">
              <xsl:choose>
                <xsl:when test="$NoDeci1='0'">
                  <xsl:value-of select="concat($TaxTotal1,'00')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$TaxTotal1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <ItinTotalFare>
              <xsl:choose>
                <xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RN'">
                  <xsl:attribute name="NegotiatedFareCode"/>
                </xsl:when>
                <xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RA'">
                  <xsl:attribute name="NegotiatedFareCode">CAT35</xsl:attribute>
                </xsl:when>
                <xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId!=''">
                  <xsl:attribute name="NegotiatedFareCode">
                    <xsl:value-of select="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId"/>
                  </xsl:attribute>
                </xsl:when>
              </xsl:choose>
              <!--xsl:if test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RN'">
								<xsl:attribute name="NegotiatedFareCode" />
							</xsl:if-->
              <BaseFare>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="$TotalAmount - $TaxTotal"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="../conversionRate/conversionRateDetail/currency"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$NoDeci"/>
                </xsl:attribute>
              </BaseFare>
              <Taxes>
                <Tax>
                  <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
                  <xsl:attribute name="Amount">
                    <xsl:value-of select="$TaxTotal"/>
                  </xsl:attribute>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="../conversionRate/conversionRateDetail/currency"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="$NoDeci"/>
                  </xsl:attribute>
                </Tax>
              </Taxes>
              <TotalFare>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="$TotalAmount"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="../conversionRate/conversionRateDetail/currency"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$NoDeci"/>
                </xsl:attribute>
              </TotalFare>
            </ItinTotalFare>
            <xsl:if test="$LFPLight=''">
              <PTC_FareBreakdowns>
                <xsl:apply-templates select="paxFareProduct"/>
              </PTC_FareBreakdowns>
              <FareInfos>
                <xsl:apply-templates select="paxFareProduct/fareDetails" mode="FareRule"/>
              </FareInfos>
            </xsl:if>
          </AirItineraryPricingInfo>
          <xsl:choose>
            <xsl:when test="segmentFlightRef/OD/flightDetails/flightInformation/addProductDetail/electronicTicketing != '' or paxFareProduct/fare/pricingMessage/freeTextQualification/textSubjectQualifier = 'LTD'">
              <TicketingInfo>
                <xsl:choose>
                  <xsl:when test="paxFareProduct/fare/pricingMessage/freeTextQualification/textSubjectQualifier = 'LTD'">
                    <xsl:attribute name="TicketTimeLimit">
                      <xsl:text>20</xsl:text>
                      <xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],6,2)"/>
                      <xsl:text>-</xsl:text>
                      <xsl:call-template name="month">
                        <xsl:with-param name="month">
                          <xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],3,3)"/>
                        </xsl:with-param>
                      </xsl:call-template>
                      <xsl:text>-</xsl:text>
                      <xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],1,2)"/>
                      <xsl:text>T23:59:00</xsl:text>
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="TicketTimeLimit">
                      <xsl:text>20</xsl:text>
                      <xsl:value-of select="substring(string(segmentFlightRef/OD[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),5,2)"/>
                      <xsl:text>-</xsl:text>
                      <xsl:value-of select="substring(string(segmentFlightRef/OD[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),3,2)"/>
                      <xsl:text>-</xsl:text>
                      <xsl:value-of select="substring(string(segmentFlightRef/OD[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),1,2)"/>
                      <xsl:text>T00:00:00</xsl:text>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
              </TicketingInfo>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="TicketTimeLimit">
                <xsl:text>20</xsl:text>
                <xsl:value-of select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),5,2)"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),3,2)"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),1,2)"/>
                <xsl:text>T00:00:00</xsl:text>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </PricedItinerary>
        <!--xsl:apply-templates select="following-sibling::recommendation[1]">
                          		<xsl:with-param name="pos"><xsl:value-of select="$pos + 1"/></xsl:with-param>
                    	</xsl:apply-templates-->
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--****************************************************************************************-->
  <!-- 											 							   -->
  <!--****************************************************************************************-->
  <xsl:template match="fareDetails" mode="FareRule">
    <xsl:variable name="ref">
      <xsl:value-of select="segmentRef/segRef"/>
    </xsl:variable>
    <xsl:apply-templates select="groupOfFares/productInformation" mode="FareRule">
      <xsl:with-param name="ref">
        <xsl:value-of select="$ref"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>
  <xsl:template match="paxFareProduct">
    <PTC_FareBreakdown>
      <xsl:variable name="TaxAmountPTC1">
        <xsl:value-of select="translate(string(paxFareDetail/totalTaxAmount),'.','')"/>
      </xsl:variable>
      <xsl:variable name="TotalAmountPTC1">
        <xsl:value-of select="translate(string(paxFareDetail/totalFareAmount),'.','')"/>
      </xsl:variable>
      <xsl:variable name="Deci" select="substring-after(string(paxFareDetail/totalFareAmount),'.')"/>
      <xsl:variable name="NoDeci1" select="string-length($Deci)"/>
      <xsl:variable name="NoDeci">
        <xsl:value-of select="'2'"/>
      </xsl:variable>
      <xsl:variable name="TotalAmountPTC">
        <xsl:choose>
          <xsl:when test="$NoDeci1='0'">
            <xsl:value-of select="concat($TotalAmountPTC1,'00')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$TotalAmountPTC1"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="TaxAmountPTC">
        <xsl:choose>
          <xsl:when test="$NoDeci1='0'">
            <xsl:value-of select="concat($TaxAmountPTC1,'00')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$TaxAmountPTC1"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="NumberOfPax">
        <xsl:value-of select="count(paxReference/traveller)"/>
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="fare[contains(pricingMessage/description,'PRIVATE RATES USED')]">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="fareDetails/groupOfFares/productInformation/fareProductDetail/fareType='RP'">
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:when>
        <!--xsl:when test="fareDetails/groupOfFares/productInformation/fareProductDetail/fareType='RV'">
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:when-->
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="paxReference/ptc = 'CH'">CHD</xsl:when>
            <xsl:when test="paxReference/ptc = 'CNN'">CHD</xsl:when>
            <xsl:when test="paxReference/ptc = 'YCD'">SRC</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="paxReference/ptc"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="count(paxReference/traveller)"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:apply-templates select="fareDetails/groupOfFares/productInformation/fareProductDetail/fareBasis" mode="FareBasis"/>
      </FareBasisCodes>
      <PassengerFare>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="($TotalAmountPTC - $TaxAmountPTC) * $NumberOfPax"/>
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <Tax>
            <xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
            <xsl:attribute name="Amount">
              <xsl:value-of select="$TaxAmountPTC * $NumberOfPax"/>
            </xsl:attribute>
          </Tax>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$TotalAmountPTC * $NumberOfPax"/>
          </xsl:attribute>
        </TotalFare>
        <xsl:if test="../referencingDetail[refQualifier='B']">
          <xsl:variable name="bref">
            <xsl:value-of select="../referencingDetail[refQualifier='B']/refNumber"/>
          </xsl:variable>
          <xsl:variable name="bag" select="../../serviceFeesGrp[serviceTypeInfo/carrierFeeDetails/type='FBA']/serviceCoverageInfoGrp[itemNumberInfo/itemNumber/number=$bref]"/>
          <xsl:variable name="paxref">
            <xsl:value-of select="paxReference/traveller/ref"/>
          </xsl:variable>
          <xsl:if test="$bag/serviceCovInfoGrp/paxRefInfo/travellerDetails/referenceNumber=$paxref">
            <xsl:for-each select="$bag/serviceCovInfoGrp/coveragePerFlightsInfo/lastItemsDetails">
              <FreeBagAllowance>
                <xsl:variable name="bagtype">
                  <xsl:value-of select="../../refInfo/referencingDetail[refQualifier='F']/refNumber"/>
                </xsl:variable>
                <xsl:variable name="baggrp" select="../../../../freeBagAllowanceGrp[itemNumberInfo/itemNumberDetails/number=$bagtype]"/>
                <xsl:choose>
                  <xsl:when test="$baggrp/freeBagAllownceInfo/baggageDetails/quantityCode='N'">
                    <xsl:attribute name="Quantity">
                      <xsl:value-of select="$baggrp/freeBagAllownceInfo/baggageDetails/freeAllowance"/>
                    </xsl:attribute>
                    <xsl:attribute name="Type">
                      <xsl:text>Piece</xsl:text>
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="Weight">
                      <xsl:value-of select="$baggrp/freeBagAllownceInfo/baggageDetails/freeAllowance"/>
                    </xsl:attribute>
                    <xsl:attribute name="Type">
                      <xsl:text>Weight</xsl:text>
                    </xsl:attribute>
                    <xsl:attribute name="Unit">
                      <xsl:value-of select="$baggrp/freeBagAllownceInfo/baggageDetails/unitQualifier"/>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:attribute name="ItinSeqNumber">
                  <xsl:value-of select="position()"/>
                </xsl:attribute>
              </FreeBagAllowance>
            </xsl:for-each>
          </xsl:if>
        </xsl:if>
      </PassengerFare>
      <TPA_Extensions>
        <PricedCode>
          <xsl:choose>
            <xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/fareType='W'">
              <xsl:value-of select="'WEB'"/>
            </xsl:when>
            <xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'CH'">CHD</xsl:when>
            <xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'CNN'">CHD</xsl:when>
            <xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'YCD'">SRC</xsl:when>
            <xsl:when test="fareDetails[1]/groupOfFares/ticketInfos/additionalFareDetails/ticketDesignator='WEB'">
              <xsl:value-of select="'WEB'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType"/>
            </xsl:otherwise>
          </xsl:choose>
        </PricedCode>
        <xsl:if test="fare/pricingMessage/freeTextQualification/textSubjectQualifier = 'APM'">
          <Text>
            <xsl:value-of select="fare/pricingMessage[freeTextQualification/textSubjectQualifier='APM']/description"/>
          </Text>
        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="fareBasis" mode="FareBasis">
    <FareBasisCode>
      <xsl:value-of select="."/>
    </FareBasisCode>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="Text" mode="diftypes">
    <xsl:choose>
      <xsl:when test="contains(string(.),'NOT FARED AT PASSENGER TYPE REQUESTED')">Y</xsl:when>
      <xsl:otherwise>N</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="OD">
    <xsl:variable name="odpos">
      <xsl:value-of select="segRef"/>
    </xsl:variable>
    <OriginDestinationOption>
      <xsl:attribute name="SectorSequence">
        <xsl:value-of select="$odpos"/>
      </xsl:attribute>
      <xsl:apply-templates select="flightDetails">
        <xsl:with-param name="pos">1</xsl:with-param>
        <xsl:with-param name="odpos">
          <xsl:value-of select="$odpos"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </OriginDestinationOption>
  </xsl:template>
  <xsl:template match="flightDetails">
    <xsl:param name="pos"/>
    <xsl:param name="odpos"/>
    <xsl:variable name="posit">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <xsl:variable name="depday">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),1,2)"/>
    </xsl:variable>
    <xsl:variable name="depmonth">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),3,2)"/>
    </xsl:variable>
    <xsl:variable name="depyear">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),5,2)"/>
    </xsl:variable>
    <xsl:variable name="arrday">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),1,2)"/>
    </xsl:variable>
    <xsl:variable name="arrmonth">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),3,2)"/>
    </xsl:variable>
    <xsl:variable name="arryear">
      <xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),5,2)"/>
    </xsl:variable>
    <FlightSegment>
      <xsl:variable name="zeros">000</xsl:variable>
      <xsl:attribute name="DepartureDateTime"><xsl:value-of select="concat('20',$depyear,'-',$depmonth,'-',$depday,'T',substring(string(flightInformation/productDateTime/timeOfDeparture),1,2),':',substring(string(flightInformation/productDateTime/timeOfDeparture),3,2),':00')"/></xsl:attribute>
      <xsl:attribute name="ArrivalDateTime"><xsl:value-of select="concat('20',$arryear,'-',$arrmonth,'-',$arrday,'T',substring(string(flightInformation/productDateTime/timeOfArrival),1,2),':',substring(string(flightInformation/productDateTime/timeOfArrival),3,2),':00')"/></xsl:attribute>
      <xsl:attribute name="StopQuantity">
        <xsl:choose>
          <xsl:when test="flightInformation/productDetail/techStopNumber!=''">
            <xsl:value-of select="flightInformation/productDetail/techStopNumber"/>
          </xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$pos"/>
      </xsl:attribute>
      <xsl:attribute name="InfoSource">
        <xsl:value-of select="../../../paxFareProduct[1]/fareDetails[segmentRef/segRef=$odpos]/groupOfFares[position()=$posit]/productInformation/cabinProduct/avlStatus"/>
      </xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="flightInformation/flightNumber"/>
        <xsl:value-of select="flightInformation/flightOrtrainNumber"/>
      </xsl:attribute>
      <xsl:variable name="classes">
        <xsl:for-each select="../../../paxFareProduct[1]/fareDetails">
          <xsl:for-each select="groupOfFares">
            <xsl:value-of select="productInformation/cabinProduct/rbd"/>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:variable>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:value-of select="../../../paxFareProduct[1]/fareDetails[segmentRef/segRef=$odpos]/groupOfFares[position()=$posit]/productInformation/cabinProduct/rbd"/>
        <!--xsl:value-of select="substring($classes,position(),1)" /-->
      </xsl:attribute>
      <xsl:attribute name="NumberInParty">
        <xsl:value-of select="count(../../../paxFareProduct/paxReference[ptc != 'INF' and ptc != 'IN']/traveller)"/>
      </xsl:attribute>
      <xsl:attribute name="E_TicketEligibility">
        <xsl:choose>
          <xsl:when test="flightInformation/addProductDetail/electronicTicketing='Y'">Eligible</xsl:when>
          <xsl:otherwise>NotEligible</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="flightInformation/location[1]/locationId"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="flightInformation/location[2]/locationId"/>
        </xsl:attribute>
      </ArrivalAirport>
      <xsl:choose>
        <xsl:when test="flightInformation/companyId/operatingCarrier != ''">
          <OperatingAirline>
            <xsl:attribute name="Code">
              <xsl:value-of select="flightInformation/companyId/operatingCarrier"/>
            </xsl:attribute>
          </OperatingAirline>
        </xsl:when>
        <xsl:when test="commercialAgreement/codeshareDetails[codeShareType='S']/flightNumber!=''">
          <OperatingAirline>
            <!--xsl:attribute name="Code"><xsl:value-of select="flightInformation/companyId/marketingCarrier"/></xsl:attribute-->
            <xsl:variable name="oa">
              <xsl:value-of select="commercialAgreement/codeshareDetails[codeShareType='S']/flightNumber"/>
            </xsl:variable>
            <xsl:value-of select="../../../../companyIdText[textRefNumber=$oa]/companyText"/>
          </OperatingAirline>
        </xsl:when>
        <xsl:when test="commercialAgreement/codeshareDetails[codeShareType='L']/flightNumber!=''">
          <OperatingAirline>
            <!--xsl:attribute name="Code"><xsl:value-of select="flightInformation/companyId/marketingCarrier"/></xsl:attribute-->
            <xsl:variable name="oa">
              <xsl:value-of select="commercialAgreement/codeshareDetails[codeShareType='L']/flightNumber"/>
            </xsl:variable>
            <xsl:value-of select="../../../../companyIdText[textRefNumber=$oa]/companyText"/>
          </OperatingAirline>
        </xsl:when>
      </xsl:choose>
      <Equipment>
        <xsl:attribute name="AirEquipType">
          <xsl:value-of select="flightInformation/productDetail/equipmentType"/>
        </xsl:attribute>
      </Equipment>
      <MarketingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="flightInformation/companyId/marketingCarrier"/>
        </xsl:attribute>
      </MarketingAirline>
      <TPA_Extensions>
        <xsl:variable name="cabin">
          <xsl:value-of select="../../../paxFareProduct[1]/fareDetails[position()=$odpos]/groupOfFares[position()=$posit]/productInformation/cabinProduct/cabin"/>
        </xsl:variable>
        <CabinType>
          <xsl:attribute name="Cabin">
            <xsl:choose>
              <xsl:when test="$cabin = 'F'">First</xsl:when>
              <xsl:when test="$cabin = 'C'">Business</xsl:when>
              <xsl:when test="$cabin = 'W'">Premium</xsl:when>
              <xsl:otherwise>Economy</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </CabinType>
        <JourneyTotalDuration>
          <xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,1,2)"/>
          <xsl:text>:</xsl:text>
          <xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,3,4)"/>
        </JourneyTotalDuration>
        <xsl:if test="../../../specificRecDetails[1]/specificProductDetails/fareContextDetails[requestedSegmentInfo/segRef=$odpos]/cnxContextDetails[position()=$posit]/fareCnxInfo/contextDetails/availabilityCnxType!=''">
          <FlightContext>
            <xsl:value-of select="../../../specificRecDetails[1]/specificProductDetails/fareContextDetails[requestedSegmentInfo/segRef=$odpos]/cnxContextDetails[position()=$posit]/fareCnxInfo/contextDetails/availabilityCnxType"/>
          </FlightContext>
        </xsl:if>
        <xsl:for-each select="technicalStop">
          <StopInfo>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="stopDetails[dateQualifier='AA']/locationId"/>
            </xsl:attribute>
            <xsl:attribute name="ArrivalDateTime">
              <xsl:value-of select="concat('20',substring(stopDetails[dateQualifier='AA']/date,5))"/>
              <xsl:value-of select="concat('-',substring(stopDetails[dateQualifier='AA']/date,3,2))"/>
              <xsl:value-of select="concat('-',substring(stopDetails[dateQualifier='AA']/date,1,2))"/>
              <xsl:value-of select="concat('T',substring(stopDetails[dateQualifier='AA']/firstTime,1,2))"/>
              <xsl:value-of select="concat(':',substring(stopDetails[dateQualifier='AA']/firstTime,3,2),':00')"/>
            </xsl:attribute>
            <xsl:attribute name="DepartureDateTime">
              <xsl:value-of select="concat('20',substring(stopDetails[dateQualifier='AD']/date,5))"/>
              <xsl:value-of select="concat('-',substring(stopDetails[dateQualifier='AD']/date,3,2))"/>
              <xsl:value-of select="concat('-',substring(stopDetails[dateQualifier='AD']/date,1,2))"/>
              <xsl:value-of select="concat('T',substring(stopDetails[dateQualifier='AD']/firstTime,1,2))"/>
              <xsl:value-of select="concat(':',substring(stopDetails[dateQualifier='AD']/firstTime,3,2),':00')"/>
            </xsl:attribute>
            <xsl:if test="stopDetails[dateQualifier='AD']/equipementType!=''">
              <xsl:attribute name="AirEquipType">
                <xsl:value-of select="stopDetails[dateQualifier='AD']/equipementType"/>
              </xsl:attribute>
            </xsl:if>
          </StopInfo>
        </xsl:for-each>
      </TPA_Extensions>
    </FlightSegment>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="productInformation" mode="FareRule">
    <xsl:param name="ref"/>
    <xsl:variable name="pos">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <FareInfo>
      <xsl:variable name="depdayfarerule">
        <xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),1,2)"/>
      </xsl:variable>
      <xsl:variable name="depmonthfarerule">
        <xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),3,2)"/>
      </xsl:variable>
      <xsl:variable name="depyearfarerule">
        <xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),5,2)"/>
      </xsl:variable>
      <xsl:attribute name="BreakPoint">
        <xsl:value-of select="breakPoint"/>
      </xsl:attribute>
      <xsl:attribute name="PassengerType">
        <xsl:choose>
          <xsl:when test="fareProductDetail/fareType='W'">
            <xsl:value-of select="'WEB'"/>
          </xsl:when>
          <xsl:when test="fareProductDetail/passengerType = 'CH'">CHD</xsl:when>
          <xsl:when test="fareProductDetail/passengerType = 'CNN'">CHD</xsl:when>
          <xsl:when test="fareProductDetail/passengerType = 'YCD'">SRC</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="fareProductDetail/passengerType"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <DepartureDate>
        <xsl:text>20</xsl:text>
        <xsl:value-of select="$depyearfarerule"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="$depmonthfarerule"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="$depdayfarerule"/>
        <xsl:text>T</xsl:text>
        <xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/timeOfDeparture),1,2)"/>
        <xsl:text>:</xsl:text>
        <xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/timeOfDeparture),3,2)"/>
        <xsl:text>:00</xsl:text>
      </DepartureDate>
      <FareReference>
        <xsl:value-of select="fareProductDetail/fareBasis"/>
      </FareReference>
      <RuleInfo>
        <xsl:apply-templates select="../../../fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']" mode="Advance"/>
        <xsl:apply-templates select="../../../fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'PEN']" mode="Penalty"/>
      </RuleInfo>
      <FilingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/companyId/marketingCarrier"/>
        </xsl:attribute>
      </FilingAirline>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/location[1]/locationId"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="../../../../segmentFlightRef/OD[segRef=$ref]/flightDetails[position()=$pos]/flightInformation/location[2]/locationId"/>
        </xsl:attribute>
      </ArrivalAirport>
    </FareInfo>
  </xsl:template>
  <!--****************************************************************************************-->
  <!-- Tax 																    -->
  <!--****************************************************************************************-->
  <xsl:template match="Details">
    <xsl:variable name="TaxDeci" select="substring-after(string(../../TotalPrice/Money[1]/Amount),'.')"/>
    <xsl:variable name="TaxDeci2" select="string-length($TaxDeci)"/>
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="Country"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(string(Amount),'.','')"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../CurrencyDetails/Currency/Code"/>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="$TaxDeci2"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <!--****************************************************************************************-->
  <!-- Fare Rules - Penalty													    -->
  <!--****************************************************************************************-->
  <xsl:template match="pricingMessage" mode="Penalty">
    <xsl:choose>
      <xsl:when test="freeTextQualification/informationType = '70' or freeTextQualification/informationType = '71' or freeTextQualification/informationType = '73'">
        <ChargesRules>
          <VoluntaryChanges>
            <Penalty>
              <xsl:attribute name="PenaltyType">
                <xsl:value-of select="description"/>
              </xsl:attribute>
            </Penalty>
          </VoluntaryChanges>
        </ChargesRules>
      </xsl:when>
      <xsl:when test="freeTextQualification/informationType = '72' and ../monetaryInformation/monetaryDetail/amount!=''">
        <ChargesRules>
          <VoluntaryChanges>
            <Penalty>
              <xsl:variable name="Deci" select="substring-after(string(../monetaryInformation/monetaryDetail/amount),'.')"/>
              <xsl:variable name="NoDeci" select="string-length($Deci)"/>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(../monetaryInformation/monetaryDetail/amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="../monetaryInformation/monetaryDetail/currency"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$NoDeci"/>
              </xsl:attribute>
            </Penalty>
          </VoluntaryChanges>
        </ChargesRules>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!--****************************************************************************************-->
  <!-- Fare Rules - Advance Purchase										    -->
  <!--****************************************************************************************-->
  <xsl:template match="pricingMessage" mode="Advance">
    <xsl:choose>
      <xsl:when test="freeTextQualification/informationType = '40'">
        <!--ResTicketingRules>
					<AdvResTicketing>
						<AdvTicketing>
							<xsl:for-each select="description">
								<xsl:value-of select="."/>
								<xsl:value-of select="string(' ')"/>
							</xsl:for-each>
						</AdvTicketing> 
					</AdvResTicketing>
				</ResTicketingRules-->
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="FlightSegment" mode="TicketingInfo">
    <xsl:if test="Description/Indicator/CodeShareAndElectronicTicket='ET'">
      <TicketingInfo>
        <xsl:attribute name="TicketType">eTicket</xsl:attribute>
      </TicketingInfo>
    </xsl:if>
    <xsl:if test="Description/Indicator/CodeShareAndElectronicTicket='EN'">
      <TicketingInfo>
        <xsl:attribute name="TicketType">eTicket</xsl:attribute>
      </TicketingInfo>
    </xsl:if>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template match="Code" mode="depterm">
    <Terminal>
      <xsl:value-of select="."/>
    </Terminal>
  </xsl:template>
  <xsl:template match="Code" mode="arrterm">
    <Terminal>
      <xsl:value-of select="."/>
    </Terminal>
  </xsl:template>
  <!--****************************************************************************************-->
  <xsl:template name="month">
    <xsl:param name="month"/>
    <xsl:choose>
      <xsl:when test="$month = 'JAN'">01</xsl:when>
      <xsl:when test="$month = 'FEB'">02</xsl:when>
      <xsl:when test="$month = 'MAR'">03</xsl:when>
      <xsl:when test="$month = 'APR'">04</xsl:when>
      <xsl:when test="$month = 'MAY'">05</xsl:when>
      <xsl:when test="$month = 'JUN'">06</xsl:when>
      <xsl:when test="$month = 'JUL'">07</xsl:when>
      <xsl:when test="$month = 'AUG'">08</xsl:when>
      <xsl:when test="$month = 'SEP'">09</xsl:when>
      <xsl:when test="$month = 'OCT'">10</xsl:when>
      <xsl:when test="$month = 'NOV'">11</xsl:when>
      <xsl:when test="$month = 'DEC'">12</xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="paxFareProduct" mode="test">
    <xsl:variable name="ft">
      <xsl:for-each select="fareDetails">
        <xsl:for-each select="groupOfFares">
          <xsl:if test="productInformation/fareProductDetail/fareType != 'RP'">
            <xsl:value-of select="productInformation/fareProductDetail/fareType"/>
          </xsl:if>
        </xsl:for-each>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="ftLen">
      <xsl:for-each select="fareDetails">
        <xsl:for-each select="groupOfFares">*</xsl:for-each>
      </xsl:for-each>
    </xsl:variable>
    <xsl:if test="not(string-length($ft)=0)">
      <xsl:if test="not(string-length($ft)=(2*(string-length($ftLen))))">1</xsl:if>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
