<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Amadeus_PNRReadRS.xsl 														       -->
  <!-- ================================================================== -->
  <!-- Date: 05 Jan 2014 - Suraj  - Handled the NameType and the PassengerTypeQuantity> Code when created from a terminal.in terminal type is null.  -->
  <!-- Date: 08 Feb 2012 - Rastko  - display misc segment even when no travel segments in PNR -->
  <!-- Date: 20 May 2009 - Rastko														       -->
  <!-- ================================================================== -->
  <xsl:variable name="segcount" select="count(//fareList/segmentInformation)" />
  <xsl:variable name="tktcount" select="count(//PoweredTicket_DisplayTSTReply/fareList)" />
  <xsl:variable name="count1">
    <xsl:choose>
      <xsl:when test="$tktcount != 0">
        <xsl:value-of select="($segcount div $tktcount)"/>
      </xsl:when>
      <xsl:otherwise>0</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name="loop">
    <xsl:choose>
      <xsl:when test="//PoweredTicket_DisplayTSTReply/fareList/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
        <xsl:value-of select="(count(//PoweredTicket_DisplayTSTReply/fareList) div 2) + 1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="count(//PoweredTicket_DisplayTSTReply/fareList) + 1" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <xsl:apply-templates select="PoweredPNR_PNRReply" />
    <xsl:apply-templates select="MessagesOnly_Reply" />
    <xsl:apply-templates select="Queue_Start_Reply" />
    <xsl:apply-templates select="Queue_Count_Reply" />
  </xsl:template>

  <xsl:template match="MessagesOnly_Reply | Queue_Start_Reply | Queue_Count_Reply">
    <OTA_TravelItineraryRS Version="2.000">
      <Errors>
        <xsl:apply-templates select="CAPI_Messages" />
      </Errors>
    </OTA_TravelItineraryRS>
  </xsl:template>

  <xsl:template match="CAPI_Messages">
    <Error>
      <xsl:attribute name="Type">Amadeus</xsl:attribute>
      <xsl:attribute name="Code">
        <xsl:value-of select="ErrorCode" />
      </xsl:attribute>
      <xsl:value-of select="Text" />
    </Error>
  </xsl:template>

  <xsl:template match="PoweredPNR_PNRReply">
    <OTA_TravelItineraryRS Version="2.000">
      <xsl:choose>
        <xsl:when test="pnrHeader/reservationInfo/reservation/controlNumber!=''">
          <Success/>
          <xsl:if test="Error or Warning">
            <Warnings>
              <xsl:apply-templates select="Error" mode="warning"/>
              <xsl:apply-templates select="Warning" mode="warning"/>
            </Warnings>
          </xsl:if>
          <TravelItinerary>
            <xsl:apply-templates select="pnrHeader[not(reservationInfo/reservation/controlType)]" mode="header"/>
            <CustomerInfos>
              <xsl:apply-templates select="travellerInfo/passengerData"/>
            </CustomerInfos>
            <ItineraryInfo>
              <xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']" >
                <ReservationItems>
                  <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']" mode="Air"/>
                  <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR']" mode="Car"/>
                  <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU']" mode="Car"/>
                  <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL']" mode="Hotel"/>
                  <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU']" mode="Hotel"/>
                  <xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] 	| originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN']" mode="Train"/>
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']" mode="Cruise"/>
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']" mode="Tour"/>
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']" mode="Other"/>
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU']" mode="Taxi"/>
                    <xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR']" mode="Land"/>
                  </xsl:if>
                  <xsl:if test="PoweredTicket_DisplayTSTReply[not(applicationError)]">
                    <ItemPricing>
                      <xsl:apply-templates select="PoweredTicket_DisplayTSTReply"/>
                    </ItemPricing>
                  </xsl:if>
                </ReservationItems>
              </xsl:if>
              <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='TK']" mode="ticketing"/>
              <SpecialRequestDetails>
                <xsl:if test="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]">
                  <SeatRequests>
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]" mode="Seat"/>
                  </SeatRequests>
                </xsl:if>
                <xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR'][not(serviceRequest/ssrb)]">
                  <SpecialServiceRequests>
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR'][not(serviceRequest/ssrb)]" mode="SSR"/>
                  </SpecialServiceRequests>
                </xsl:if>
                <xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']">
                  <OtherServiceInformations>
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']" mode="OSI"/>
                  </OtherServiceInformations>
                </xsl:if>
                <xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RM']">
                  <Remarks>
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RM']" mode="GenRemark"/>
                  </Remarks>
                </xsl:if>
                <xsl:if test="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RI')] | dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE']">
                  <SpecialRemarks>
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RI')]" mode="InvoiceItinRemark"/>
                    <!--xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIF']" mode="InvoiceRemark"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIT']" mode="InvoiceRemark"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIR']" mode="ItinRemark"/-->
                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RC')]" mode="ConfRemark"/>

                    <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE']" mode="Endorsement"/>
                  </SpecialRemarks>
                </xsl:if>
              </SpecialRequestDetails>
            </ItineraryInfo>
            <!--xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA']" mode="ticketnumber"/-->
            <TravelCost>
              <xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
                <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP' ]" mode="Payment"/>
              </xsl:if>
              <!--xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']">
							<TPA_Extensions>
								<AgencyCommission>
									<xsl:attribute name="Percent"><xsl:value-of select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext/longFreetext"/></xsl:attribute>
								</AgencyCommission>
							</TPA_Extensions>
						</xsl:if-->
            </TravelCost>
            <UpdatedBy>
              <xsl:attribute name="CreateDateTime">
                <xsl:value-of select="format-number(substring(securityInformation/secondRpInformation/creationDate,5,2),'2000')"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(securityInformation/secondRpInformation/creationDate,3,2)"/>
                <xsl:text>-</xsl:text>
                <xsl:value-of select="substring(securityInformation/secondRpInformation/creationDate,1,2)"/>
                <xsl:text>T</xsl:text>
                <xsl:value-of select="substring(securityInformation/secondRpInformation/creationTime,1,2)"/>
                <xsl:text>:</xsl:text>
                <xsl:value-of select="substring(securityInformation/secondRpInformation/creationTime,1,2)"/>
                <xsl:text>:00</xsl:text>
              </xsl:attribute>
            </UpdatedBy>
            <xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FB'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']">
              <TPA_Extensions>
                <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']" mode="commission"/>
                <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']" mode="accounting"/>
              </TPA_Extensions>
            </xsl:if>
          </TravelItinerary>
        </xsl:when>
        <xsl:when test="Error">
          <Errors>
            <xsl:apply-templates select="Error" mode="error"/>
          </Errors>
        </xsl:when>
      </xsl:choose>
    </OTA_TravelItineraryRS>
  </xsl:template>

  <xsl:template match="Error" mode="error">
    <Error Type="Amadeus">
      <xsl:value-of select="."/>
    </Error>
  </xsl:template>

  <xsl:template match="Text">
    <Text>
      <xsl:value-of select="."/>
    </Text>
  </xsl:template>

  <xsl:template match="Error | Warning" mode="warning">
    <Warning Type="Amadeus">
      <xsl:value-of select="."/>
    </Warning>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- PNR Header Information    	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="pnrHeader" mode="header">
    <ItineraryRef>
      <xsl:attribute name="Type">PNR</xsl:attribute>
      <xsl:attribute name="ID">
        <xsl:value-of select="reservationInfo/reservation/controlNumber"/>
      </xsl:attribute>
      <xsl:if test="../securityInformation/responsibilityInformation/officeId != ''">
        <xsl:attribute name="ID_Context">
          <xsl:value-of select="../securityInformation/responsibilityInformation/officeId"/>
        </xsl:attribute>
      </xsl:if>
    </ItineraryRef>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- Pricing Response     	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="PoweredTicket_DisplayTSTReply">
    <AirFareInfo>
      <xsl:attribute name="PricingSource">
        <!--xsl:choose>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'H'">Private</xsl:when>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'F'">Private</xsl:when>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'BT'">Private</xsl:when>
				<xsl:otherwise>Published</xsl:otherwise>
			</xsl:choose-->
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
      <ItinTotalFare>
        <BaseFare>
          <!--xsl:attribute name="Amount">
					<xsl:variable name="base">
						<xsl:value-of select="format-number(sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount),'.00')"/>
					</xsl:variable>
					<xsl:value-of select="translate(string($base),'.','')"/>
				</xsl:attribute-->
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
          <!--xsl:attribute name="Amount">
					<xsl:variable name="tot">
						<xsl:value-of select="format-number(sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount),'.00')"/>
					</xsl:variable>
					<xsl:value-of select="translate(string($tot),'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute-->
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:apply-templates select="fareList"/>
      </PTC_FareBreakdowns>
    </AirFareInfo>
  </xsl:template>

  <xsl:template match="fareList">
    <PTC_FareBreakdown>
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
          <xsl:otherwise>Published</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:variable name="paxref">
            <xsl:value-of select="paxSegReference/refDetails[1]/refNumber"/>
          </xsl:variable>
          <xsl:variable name="paxtype">
            <xsl:choose>
              <xsl:when test="statusInformation/firstStatusDetails[1]/tstFlag='INF'">INF</xsl:when>
              <xsl:when test="not(../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger/type)">ADT</xsl:when>
              <xsl:when test="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/infantIndicator = 1 and ../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[position()=2]/type = 'INF'">ADT</xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger/type"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$paxtype = 'INN'">CHD</xsl:when>
            <xsl:when test="$paxtype = 'CNN'">CHD</xsl:when>
            <xsl:when test="$paxtype = 'YCD'">SRC</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$paxtype"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="count(paxSegReference/refDetails)"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:if test="segmentInformation/fareQualifier/fareBasisDetails">
        <FareBasisCodes>
          <!--xsl:apply-templates select="segmentInformation[connexInformation/connecDetails/routingInformation != 'ARNK']"/-->
          <xsl:apply-templates select="segmentInformation"/>
        </FareBasisCodes>
      </xsl:if>
      <xsl:variable name="nip">
        <xsl:value-of select="count(paxSegReference/refDetails)"/>
      </xsl:variable>
      <PassengerFare>
        <xsl:variable name="bfpax">
          <xsl:choose>
            <xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
              <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nip" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="tfpax">
          <xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nip" />
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
            <xsl:value-of select="$cur" />
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dec"/>
          </xsl:attribute>
        </BaseFare>
        <Taxes>
          <xsl:apply-templates select="taxInformation">
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </Taxes>
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
            <xsl:value-of select="$cur" />
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
    </PTC_FareBreakdown>
  </xsl:template>

  <xsl:template match="segmentInformation">
    <xsl:if test="not(connexInformation/connecDetails/routingInformation) or connexInformation/connecDetails/routingInformation != 'ARNK'">
      <FareBasisCode>
        <xsl:value-of select="fareQualifier/fareBasisDetails/primaryCode"/>
        <xsl:value-of select="fareQualifier/fareBasisDetails/fareBasisCode"/>
      </FareBasisCode>
    </xsl:if>
  </xsl:template>

  <xsl:template match="taxInformation">
    <xsl:param name="nip"/>
    <Tax>
      <xsl:attribute name="Code">
        <xsl:value-of select="taxDetails/taxType/isoCountry"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(amountDetails/fareDataMainInformation/fareAmount,'.','') * $nip"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Process Names			                            -->
  <!-- ************************************************************** -->
  <xsl:template match="passengerData">
    <CustomerInfo>
      <xsl:attribute name="RPH">
        <xsl:value-of select="../elementManagementPassenger/lineNumber"/>
      </xsl:attribute>
      <Customer>
        <xsl:if test="dateOfBirth/dateAndTimeDetails[qualifier='706'] and not(travellerInformation/passenger[1]/infantIndicator)">
          <xsl:attribute name="BirthDate">
            <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,5,4)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <PersonName>
          <xsl:attribute name="NameType">
            <xsl:choose>
              <xsl:when test="not(travellerInformation/passenger[1]/type)">ADT</xsl:when>
              <xsl:when test="travellerInformation/passenger[1]/type = 'CH'">CHD</xsl:when>
              <xsl:when test="travellerInformation/passenger[1]/type = 'CNN'">CHD</xsl:when>
              <xsl:when test="travellerInformation/passenger[1]/type = 'YCD'">SRC</xsl:when>
              <xsl:when test="travellerInformation/passenger[1]/infantIndicator = 1">ADT</xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="travellerInformation/passenger/type"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <GivenName>
            <xsl:value-of select="travellerInformation/passenger/firstName"/>
          </GivenName>
          <Surname>
            <xsl:value-of select="travellerInformation/traveller/surname"/>
          </Surname>
        </PersonName>
        <xsl:variable name="paxref">
          <xsl:value-of select="../elementManagementPassenger/reference/number"/>
        </xsl:variable>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="phone"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="email"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB/']" mode="Address"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB']" mode="Address"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM/']" mode="Address"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM']" mode="Address"/>
        <xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][serviceRequest/ssr/status='HK']" mode="Fqtv">
          <xsl:with-param name="paxref">
            <xsl:value-of select="$paxref"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </Customer>
    </CustomerInfo>
    <xsl:if test="travellerInformation/passenger[1]/infantIndicator = 1 and travellerInformation/passenger[position()=2]/type = 'INF'">
      <CustomerInfo>
        <xsl:attribute name="RPH">
          <xsl:value-of select="../elementManagementPassenger/lineNumber"/>
        </xsl:attribute>
        <Customer>
          <xsl:if test="dateOfBirth/dateAndTimeDetails[qualifier='706']">
            <xsl:attribute name="BirthDate">
              <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,5,4)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,3,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,1,2)"/>
            </xsl:attribute>
          </xsl:if>
          <PersonName>
            <xsl:attribute name="NameType">INF</xsl:attribute>
            <GivenName>
              <xsl:value-of select="travellerInformation/passenger[position()=2]/firstName"/>
            </GivenName>
            <Surname>
              <xsl:value-of select="travellerInformation/traveller/surname"/>
            </Surname>
          </PersonName>
        </Customer>
      </CustomerInfo>
    </xsl:if>
  </xsl:template>
  <!-- ****************************************************************************************************************** -->
  <!-- Process Itinerary				 							                -->
  <!-- ****************************************************************************************************************** -->
  <!-- Air Segments    				                    -->
  <!-- ************************************************************** -->
  <xsl:template match="itineraryInfo" mode="Air">
    <xsl:variable name="zeros">000</xsl:variable>
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
      </xsl:attribute>
      <xsl:if test="relatedProduct/status='GK'">
        <xsl:attribute name="IsPassive">true</xsl:attribute>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="travelProduct/productDetails/identification='ARNK'">
          <TPA_Extensions>
            <Arnk/>
          </TPA_Extensions>
        </xsl:when>
        <xsl:otherwise>
          <Air>
            <!--************************************************************************************-->
            <!--			Air Segments/Open Segments  						      -->
            <!--************************************************************************************-->
            <xsl:variable name="zeroes">0000</xsl:variable>
            <xsl:attribute name="DepartureDateTime">
              <xsl:text>20</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
              <xsl:text>T</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depTime,1,2)"/>:<xsl:value-of select="substring(travelProduct/product/depTime,3,2)"/><xsl:text>:00</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="ArrivalDateTime">
              <xsl:text>20</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
              <xsl:text>T</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrTime,1,2)"/>:<xsl:value-of select="substring(travelProduct/product/arrTime,3,2)"/><xsl:text>:00</xsl:text>
            </xsl:attribute>
            <xsl:if test="flightDetail/productDetails/numOfStops != ''">
              <xsl:attribute name="StopQuantity">
                <xsl:value-of select="flightDetail/productDetails/numOfStops"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="RPH">
              <xsl:value-of select="position()"/>
            </xsl:attribute>
            <xsl:attribute name="FlightNumber">
              <xsl:choose>
                <xsl:when test="travelProduct/productDetails/identification='OPEN'">OPEN</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="travelProduct/productDetails/identification"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="ResBookDesigCode">
              <xsl:value-of select="travelProduct/productDetails/classOfService"/>
            </xsl:attribute>
            <xsl:attribute name="NumberInParty">
              <xsl:value-of select="relatedProduct/quantity"/>
            </xsl:attribute>
            <xsl:attribute name="Status">
              <xsl:value-of select="relatedProduct/status"/>
            </xsl:attribute>
            <xsl:if test="travelProduct/typeDetail/detail != ''">
              <xsl:attribute name="E_TicketEligibility">
                <xsl:choose>
                  <xsl:when test="travelProduct/typeDetail/detail = 'ET'">Eligible</xsl:when>
                  <xsl:otherwise>NotEligible</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="flightDetail/productDetails/weekDay != ''">
              <xsl:attribute name="DepartureDay">
                <xsl:choose>
                  <xsl:when test="flightDetail/productDetails/weekDay = '1'">Mon</xsl:when>
                  <xsl:when test="flightDetail/productDetails/weekDay = '2'">Tue</xsl:when>
                  <xsl:when test="flightDetail/productDetails/weekDay = '3'">Wed</xsl:when>
                  <xsl:when test="flightDetail/productDetails/weekDay = '4'">Thu</xsl:when>
                  <xsl:when test="flightDetail/productDetails/weekDay = '5'">Fri</xsl:when>
                  <xsl:when test="flightDetail/productDetails/weekDay = '6'">Sat</xsl:when>
                  <xsl:otherwise>Sun</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:if>
            <DepartureAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
              </xsl:attribute>
              <xsl:if test="flightDetail/departureInformation/departTerminal != ''">
                <xsl:attribute name="Terminal">
                  <xsl:value-of select="flightDetail/departureInformation/departTerminal"/>
                </xsl:attribute>
              </xsl:if>
            </DepartureAirport>
            <ArrivalAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
              </xsl:attribute>
            </ArrivalAirport>
            <OperatingAirline>
              <xsl:choose>
                <xsl:when test="CAPI_PNR_AirSegment/Airline2 != ''">
                  <xsl:attribute name="Code">
                    <xsl:value-of select="CAPI_PNR_AirSegment/Airline2"/>
                  </xsl:attribute>
                  <AirlineName></AirlineName>
                </xsl:when>
                <xsl:when test="itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text !=''">
                  <xsl:choose>
                    <xsl:when test="contains(itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text,'OPERATED BY')">
                      <xsl:value-of select="substring-after(itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text,' BY ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="travelProduct/companyDetail/identification"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </OperatingAirline>
            <xsl:if test="flightDetail/productDetails/equipment">
              <Equipment>
                <xsl:attribute name="AirEquipType">
                  <xsl:value-of select="flightDetail/productDetails/equipment"/>
                </xsl:attribute>
              </Equipment>
            </xsl:if>
            <MarketingAirline>
              <xsl:attribute name="Code">
                <xsl:value-of select="travelProduct/companyDetail/identification"/>
              </xsl:attribute>
            </MarketingAirline>
            <TPA_Extensions>
              <xsl:attribute name="ConfirmationNumber">
                <xsl:value-of select="itineraryReservationInfo/reservation/controlNumber"/>
              </xsl:attribute>
            </TPA_Extensions>
          </Air>
        </xsl:otherwise>
      </xsl:choose>
    </Item>
  </xsl:template>

  <!--************************************************************************************-->
  <!--			Hotel Segs   						   					    -->
  <!--************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Hotel">
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
      </xsl:attribute>
      <xsl:if test="elementManagementItinerary/segmentName='HU'">
        <xsl:attribute name="IsPassive">Y</xsl:attribute>
      </xsl:if>
      <Hotel>
        <Reservation>
          <RoomTypes>
            <RoomType>
              <xsl:attribute name="RoomTypeCode">
                <xsl:value-of select="hotelProduct/hotelRoom/typeCode"/>
              </xsl:attribute>
              <xsl:attribute name="NumberOfUnits">
                <xsl:value-of select="relatedProduct/quantity"/>
              </xsl:attribute>
            </RoomType>
          </RoomTypes>
          <RatePlans>
            <RatePlan>
              <xsl:attribute name="RatePlanCode">
                <xsl:value-of select="hotelProduct/negotiated/rateCode"/>
              </xsl:attribute>
            </RatePlan>
          </RatePlans>
          <RoomRates>
            <RoomRate>
              <Rates>
                <Rate>
                  <Base>
                    <xsl:choose>
                      <xsl:when test="elementManagementItinerary/segmentName='HU'">
                        <xsl:if test="contains(itineraryFreetext/longFreetext,'/AO-')">
                          <xsl:variable name="amt">
                            <xsl:value-of select="substring-before(substring-after(itineraryFreetext/longFreetext,'/AO-'),'/')"/>
                          </xsl:variable>
                          <xsl:attribute name="AmountBeforeTax">
                            <xsl:value-of select="translate(substring($amt,4),'.','')"/>
                          </xsl:attribute>
                          <xsl:attribute name="CurrencyCode">
                            <xsl:value-of select="substring($amt,1,3)"/>
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="string-length(substring-after($amt,'.'))"/>
                          </xsl:attribute>
                        </xsl:if>
                      </xsl:when>
                      <xsl:when test="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/amount != ''">
                        <xsl:variable name="amt">
                          <xsl:value-of select="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/amount"/>
                        </xsl:variable>
                        <xsl:attribute name="AmountBeforeTax">
                          <xsl:value-of select="translate($amt,'.','')"/>
                        </xsl:attribute>
                        <xsl:attribute name="CurrencyCode">
                          <xsl:value-of select="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/currency"/>
                        </xsl:attribute>
                        <xsl:attribute name="DecimalPlaces">
                          <xsl:value-of select="string-length(substring-after($amt,'.'))"/>
                        </xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:variable name="deci">
                          <xsl:value-of select="substring-after(string(generalOption/optionDetail[type='TTL']/freetext),'.')"/>
                        </xsl:variable>
                        <xsl:variable name="decilgth">
                          <xsl:value-of select="string-length($deci)"/>
                        </xsl:variable>
                        <xsl:variable name="amount">
                          <xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),4,20)"/>
                        </xsl:variable>
                        <xsl:attribute name="AmountBeforeTax">
                          <xsl:value-of select="translate(string($amount),'.','')"/>
                        </xsl:attribute>
                        <xsl:attribute name="CurrencyCode">
                          <xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),1,3)"/>
                        </xsl:attribute>
                        <xsl:attribute name="DecimalPlaces">
                          <xsl:value-of select="$decilgth"/>
                        </xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Base>
                </Rate>
              </Rates>
            </RoomRate>
          </RoomRates>
          <GuestCounts>
            <GuestCount>
              <xsl:attribute name="Count">
                <xsl:value-of select="hotelProduct/hotelRoom/occupancy"/>
              </xsl:attribute>
            </GuestCount>
          </GuestCounts>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:text>20</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:text>20</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
            </xsl:attribute>
          </TimeSpan>
          <BasicPropertyInfo>
            <xsl:attribute name="ChainCode">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
            <xsl:attribute name="HotelCityCode">
              <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
            </xsl:attribute>
            <xsl:attribute name="HotelCode">
              <xsl:value-of select="hotelProduct/property/code"/>
            </xsl:attribute>
            <xsl:attribute name="HotelName">
              <xsl:choose>
                <xsl:when test="elementManagementItinerary/segmentName='HU'">
                  <xsl:value-of select="substring-before(itineraryFreetext/longFreetext,'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="hotelProduct/property/name"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </BasicPropertyInfo>
        </Reservation>
        <TPA_Extensions>
          <xsl:attribute name="ConfirmationNumber">
            <xsl:choose>
              <xsl:when test="elementManagementItinerary/segmentName='HU'">
                <xsl:if test="contains(itineraryFreetext/longFreetext,'/CF-')">
                  <xsl:value-of select="substring-before(substring-after(itineraryFreetext/longFreetext,'/CF-'),'/')"/>
                </xsl:if>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </TPA_Extensions>
      </Hotel>
    </Item>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Car Segs				   			-->
  <!--************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Car">
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
      </xsl:attribute>
      <xsl:if test="elementManagementItinerary/segmentName='CU'">
        <xsl:attribute name="IsPassive">Y</xsl:attribute>
      </xsl:if>
      <Vehicle>
        <ConfID>
          <xsl:attribute name="Type">8</xsl:attribute>
          <xsl:attribute name="ID">
            <xsl:choose>
              <xsl:when test="generalOption/optionDetail[type='CF']/freetext != ''">
                <xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/>
              </xsl:when>
              <xsl:when test="typicalCarData/cancelOrConfirmNbr/reservation[controlType='3' or controlType='2']/controlNumber!= ''">
                <xsl:value-of select="typicalCarData/cancelOrConfirmNbr/reservation[controlType='3' or controlType='2']/controlNumber"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="generalOption/optionDetail[type='BS']/freetext"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </ConfID>
        <Vendor>
          <xsl:attribute name="Code">
            <xsl:value-of select="travelProduct/companyDetail/identification"/>
          </xsl:attribute>
        </Vendor>
        <VehRentalCore>
          <xsl:attribute name="PickUpDateTime">
            <xsl:value-of select="typicalCarData/pickupDropoffTimes/beginDateTime/year"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/month,'00')"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/day,'00')"/>
            <xsl:text>T</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/hour,'00')"/>
            <xsl:text>:</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/minutes,'00')"/>
            <xsl:text>:00</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="ReturnDateTime">
            <xsl:value-of select="typicalCarData/pickupDropoffTimes/endDateTime/year"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/month,'00')"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/day,'00')"/>
            <xsl:text>T</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/hour,'00')"/>
            <xsl:text>:</xsl:text>
            <xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/minutes,'00')"/>
            <xsl:text>:00</xsl:text>
          </xsl:attribute>
          <PickUpLocation>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
            </xsl:attribute>
          </PickUpLocation>
          <ReturnLocation>
            <xsl:attribute name="LocationCode">
              <xsl:choose>
                <xsl:when test="travelProduct/offpointDetail/cityCode != ''">
                  <xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
                </xsl:when>
                <xsl:when test="tgeneralOption/optionDetail[type='DO']/freetext != ''">
                  <xsl:value-of select="generalOption/optionDetail[type='DO']/freetext"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="typicalCarData/locationInfo[locationType='DOL']/locationDescription/name"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </ReturnLocation>
        </VehRentalCore>
        <Veh>
          <xsl:attribute name="AirConditionInd">
            <xsl:choose>
              <xsl:when test="substring(travelProduct/productDetails/identification,4,1) = 'R'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="TransmissionType">
            <xsl:choose>
              <xsl:when test="substring(travelProduct/productDetails/identification,3,1) = 'A'">Automatic</xsl:when>
              <xsl:otherwise>Manual</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <VehType>
            <xsl:attribute name="VehicleCategory">
              <xsl:choose>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'C'">2/4 Door Car</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'B'">2-Door Car</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'D'">4-Door Car</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'W'">Wagon</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'V'">Van</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'L'">Limousine</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'S'">Sport</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'T'">Convertible</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'F'">4-Wheel Drive</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'P'">Pickup</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'J'">All-Terrain</xsl:when>
                <xsl:otherwise>Unavailable</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="travelProduct/productDetails/identification"/>
          </VehType>
          <VehClass>
            <xsl:attribute name="Size">
              <xsl:choose>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'M'">Mini</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'E'">Economy</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'C'">Compact</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'I'">Intermediate</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'S'">Standard</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'F'">Full-Size</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'P'">Premium</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'L'">Luxury</xsl:when>
                <xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'X'">Special</xsl:when>
                <xsl:otherwise>Unavailable</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </VehClass>
        </Veh>
        <RentalRate>
          <xsl:choose>
            <xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType='RG' or amountType='RQ']/amount != ''">
              <xsl:apply-templates select="typicalCarData"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:variable name="rateText">
                <xsl:choose>
                  <xsl:when test="typicalCarData/rateInfo/chargeDetails[type='RG']/comment !=''">
                    <xsl:value-of select="typicalCarData/rateInfo/chargeDetails[type='RG']/comment"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="typicalCarData/rateInfo/chargeDetails[type='RQ']/comment"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:if test="$rateText!=''">
                <RateDistance>
                  <xsl:attribute name="Unlimited">
                    <xsl:choose>
                      <xsl:when test="contains($rateText, 'UNL')">true</xsl:when>
                      <xsl:otherwise>false</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:attribute name="DistUnitName">
                    <xsl:choose>
                      <xsl:when test="contains($rateText, ' KM')">Km</xsl:when>
                      <xsl:otherwise>Mile</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </RateDistance>
                <VehicleCharges>
                  <VehicleCharge>
                    <xsl:attribute name="Amount">
                      <xsl:value-of select="substring-before(substring($rateText,8),'.')"/>
                      <xsl:value-of select="substring(substring-after(substring($rateText,8),'.'),1,2)"/>
                    </xsl:attribute>
                    <xsl:attribute name="CurrencyCode">
                      <xsl:value-of select="substring($rateText,5,3)"/>
                    </xsl:attribute>
                    <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
                    <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                    <xsl:attribute name="GuaranteedInd">
                      <xsl:choose>
                        <xsl:when test="typicalCarData/rateInfo/chargeDetails[type='RG']">true</xsl:when>
                        <xsl:otherwise>false</xsl:otherwise>
                      </xsl:choose>
                    </xsl:attribute>
                    <xsl:attribute name="Description">
                      <xsl:choose>
                        <xsl:when test="contains($rateText, 'HY')">Hourly Rate</xsl:when>
                        <xsl:when test="contains($rateText, 'DY')">Daily Rate</xsl:when>
                        <xsl:when test="contains($rateText, 'WY')">Weekly Rate</xsl:when>
                        <xsl:when test="contains($rateText, 'MY')">Monthly Rate</xsl:when>
                        <xsl:when test="contains($rateText, 'WD')">Weekend Rate</xsl:when>
                        <xsl:when test="contains($rateText, 'PK')">Package Rate</xsl:when>
                        <xsl:otherwise>Rental Period Rate</xsl:otherwise>
                      </xsl:choose>
                    </xsl:attribute>
                  </VehicleCharge>
                </VehicleCharges>
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>


          <!--xsl:if test="contains(generalOption/optionDetail[type='RG']/freetext, 'XD')">
						<xsl:variable name="ed">
							<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),'.',substring(substring-after(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),1,2)),' ','')"/>
						</xsl:variable>
						<xsl:if test="$ed != '.'">
							<VehicleCharge> 
								<xsl:attribute name="TaxInclusive">false</xsl:attribute>
								<xsl:attribute name="Amount">	
		 							<xsl:value-of select="number(translate($ed,'.',''))"/>	
								</xsl:attribute>	
								<xsl:attribute name="CurrencyCode">			
									<xsl:choose>
										<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
											<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
										</xsl:otherwise>
									</xsl:choose>		
								</xsl:attribute>	
								<xsl:attribute name="DecimalPlaces">					
									<xsl:value-of select="string-length(substring-after($ed,'.'))"/>
								</xsl:attribute>			
								<xsl:attribute name="Description">Extra Day Rate</xsl:attribute>	
							</VehicleCharge>
						</xsl:if>
					</xsl:if>
					<xsl:if test="contains(generalOption/optionDetail[type='RG']/freetext, 'XH')">
						<xsl:variable name="eh">
							<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XH'),'.'),'.',substring(substring-after(substring-after(generalOption/optionDetail	[type='RG']/freetext,'XH'),'.'),1,2)),' ','')"/>
						</xsl:variable>
						<xsl:if test="$eh != '.'">
							<VehicleCharge> 
								<xsl:attribute name="TaxInclusive">false</xsl:attribute>
		 						<xsl:attribute name="Amount">	
		 							<xsl:value-of select="translate($eh,'.','')"/>	
								</xsl:attribute>	
								<xsl:attribute name="CurrencyCode">			
									<xsl:choose>
										<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
											<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
										</xsl:otherwise>
									</xsl:choose>		
								</xsl:attribute>	
								<xsl:attribute name="DecimalPlaces">					
									<xsl:value-of select="string-length(substring-after($eh,'.'))"/>
								</xsl:attribute>			
								<xsl:attribute name="Description">Extra Hour Rate</xsl:attribute>	
							</VehicleCharge>
						</xsl:if>
					</xsl:if-->

          <xsl:if test="typicalCarData/rateCodeGroup/rateCodeInfo/fareCategories/fareType != ''">
            <RateQualifier>
              <xsl:if test="typicalCarData/rateInfo/rateInformation/category!=''">
                <xsl:attribute name="RateCategory">
                  <xsl:choose>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='2'">Inclusive</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='6'">Convention</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='7'">Corporate</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='9'">Government</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='11'">Package</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='19'">Association</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='20'">Business</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='21'">Consortium</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='22'">Credential</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='23'">Industry</xsl:when>
                    <xsl:when test="typicalCarData/rateInfo/rateInformation/category='24'">Standard</xsl:when>
                    <xsl:otherwise>General</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:if>
              <xsl:attribute name="RateQualifier">
                <xsl:value-of select="typicalCarData/rateCodeGroup/rateCodeInfo/fareCategories/fareType"/>
              </xsl:attribute>
              <xsl:attribute name="RatePeriod">
                <xsl:variable name="rateinfo" select="typicalCarData/rateInfo[tariffInfo/amountType='RG' or tariffInfo/amountType='RQ']"/>
                <xsl:choose>
                  <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator != ''">
                    <xsl:choose>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'HY'">Hourly</xsl:when>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'DY'">Daily</xsl:when>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WY'">Weekly</xsl:when>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'MY'">Monthly</xsl:when>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WD'">WeekendDay</xsl:when>
                      <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'PK'">Package</xsl:when>
                      <xsl:otherwise>Other</xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'DY')">Daily</xsl:when>
                  <xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'WY')">Weekly</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'HY')">Hourly</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'DY')">Daily</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'WY')">Weekly</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'MY')">Monthly</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'WD')">WeekendDay</xsl:when>
                  <xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'PK')">Package</xsl:when>
                  <xsl:otherwise>Other</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </RateQualifier>
          </xsl:if>
        </RentalRate>
        <xsl:variable name="total">
          <xsl:choose>
            <xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = '904']/amount != ''">
              <xsl:value-of select="translate(typicalCarData/rateInfo/tariffInfo[amountType = '904']/amount,'.','')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="translate(concat(substring-before(substring(generalOption/optionDetail[type='ES']/freetext,8),'.'),'.',substring(substring-after(generalOption/optionDetail[type='ES']/freetext,'.'),1,2)),' ','')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:if test="$total != '.'">
          <TotalCharge>
            <xsl:attribute name="RateTotalAmount">
              <xsl:value-of select="$total"/>
            </xsl:attribute>
            <xsl:attribute name="EstimatedTotalAmount">
              <xsl:value-of select="$total"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:choose>
                <xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
                  <xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
                </xsl:when>
                <xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = '904']/currency != ''">
                  <xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = '904']/currency"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </TotalCharge>
        </xsl:if>
        <TPA_Extensions>
          <CarOptions>
            <xsl:apply-templates select="generalOption/optionDetail" mode="car" />
            <xsl:if test="typicalCarData/customerInfo/customerReferences[referenceQualifier='CD']">
              <CarOption>
                <xsl:attribute name="Option">Corporate Discount</xsl:attribute>
                <Text>
                  <xsl:value-of select="typicalCarData/customerInfo/customerReferences[referenceQualifier='CD']/referenceNumber" />
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:if test="typicalCarData/customerInfo/customerReferences[referenceQualifier='1']">
              <CarOption>
                <xsl:attribute name="Option">Customer ID</xsl:attribute>
                <Text>
                  <xsl:value-of select="typicalCarData/customerInfo/customerReferences[referenceQualifier='1']/referenceNumber" />
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:if test="typicalCarData/bookingSource/originatorDetails/originatorId!=''">
              <CarOption>
                <xsl:attribute name="Option">Booking Source</xsl:attribute>
                <Text>
                  <xsl:value-of select="typicalCarData/bookingSource/originatorDetails/originatorId" />
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:if test="typicalCarData/supleInfo/remarkDetails[type='ARR']">
              <CarOption>
                <xsl:attribute name="Option">Additional Info</xsl:attribute>
                <Text>
                  <xsl:text>ARRIVES </xsl:text>
                  <xsl:value-of select="typicalCarData/supleInfo/remarkDetails[type='ARR']/freetext" />
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:if test="typicalCarData/attribute[criteriaSetType='BAT']">
              <CarOption>
                <xsl:attribute name="Option">Additional Info</xsl:attribute>
                <Text>
                  <xsl:choose>
                    <xsl:when test="typicalCarData/attribute[criteriaSetType='BAT']/criteriaDetails/attributeType='COR'">CORPORATE BOOKING</xsl:when>
                    <xsl:otherwise>LEISURE BOOKING</xsl:otherwise>
                  </xsl:choose>
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:if test="typicalCarData/supleInfo/remarkDetails[type='CNM']">
              <CarOption>
                <xsl:attribute name="Option">Reservation Last and First Names</xsl:attribute>
                <Text>
                  <xsl:value-of select="typicalCarData/supleInfo/remarkDetails[type='CNM']/freetext" />
                </Text>
              </CarOption>
            </xsl:if>
            <xsl:for-each select="itineraryFreetext[longFreetext != '']">
              <CarOption>
                <xsl:attribute name="Option">Marketing Text</xsl:attribute>
                <Text>
                  <xsl:value-of select="longFreetext" />
                </Text>
              </CarOption>
            </xsl:for-each>
            <xsl:if test="typicalCarData/marketingInfo[freetextDetail/type='MK']">
              <xsl:for-each select="typicalCarData/marketingInfo[freetextDetail/type='MK']">
                <xsl:for-each select="text">
                  <CarOption>
                    <xsl:attribute name="Option">Marketing Text</xsl:attribute>
                    <Text>
                      <xsl:value-of select="." />
                    </Text>
                  </CarOption>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:if>
          </CarOptions>
        </TPA_Extensions>
      </Vehicle>
    </Item>
  </xsl:template>
  <!-- -->
  <xsl:template match="typicalCarData">
    <xsl:variable name="rateinfo" select="rateInfo[tariffInfo/amountType='RG' or tariffInfo/amountType='RQ']"/>
    <RateDistance>
      <xsl:attribute name="Unlimited">
        <xsl:choose>
          <xsl:when test="$rateinfo/chargeDetails/description = 'UNL'">true</xsl:when>
          <xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'UNL')">true</xsl:when>
          <xsl:when test="contains($rateinfo/chargeDetails[type='RG' or type='RQ']/comment, 'UNL')">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="DistUnitName">
        <xsl:choose>
          <xsl:when test="$rateinfo[chargeDetails/type='31']">Mile</xsl:when>
          <xsl:when test="$rateinfo[chargeDetails/type='32']">Km</xsl:when>
          <xsl:otherwise>Mile</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </RateDistance>
    <VehicleCharges>
      <VehicleCharge>
        <xsl:attribute name="Amount">
          <xsl:value-of select="translate($rateinfo/tariffInfo/amount,'.','')"/>
        </xsl:attribute>
        <xsl:attribute name="CurrencyCode">
          <xsl:value-of select="$rateinfo/tariffInfo/currency"/>
        </xsl:attribute>
        <xsl:attribute name="DecimalPlaces">
          <xsl:value-of select="string-length(substring-after($rateinfo/tariffInfo/amount,'.'))"/>
        </xsl:attribute>
        <xsl:attribute name="TaxInclusive">false</xsl:attribute>
        <xsl:attribute name="Description">
          <xsl:choose>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'HY'">Hourly Rate</xsl:when>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'DY'">Daily Rate</xsl:when>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WY'">Weekly Rate</xsl:when>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'MY'">Monthly Rate</xsl:when>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WD'">Weekend Rate</xsl:when>
            <xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'PK'">Package Rate</xsl:when>
            <xsl:otherwise>Rental Period Rate</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="GuaranteedInd">
          <xsl:choose>
            <xsl:when test="$rateinfo/tariffInfo[amountType = 'RG']/amount != ''">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </VehicleCharge>
      <xsl:if test="$rateinfo/chargeDetails[type='8']">
        <VehicleCharge>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate($rateinfo/chargeDetails[type='8']/amount,'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after($rateinfo/chargeDetails[type='8']/amount,'.'))"/>
          </xsl:attribute>
          <xsl:attribute name="TaxInclusive">false</xsl:attribute>
          <xsl:attribute name="Description">Extra Day Rate</xsl:attribute>
        </VehicleCharge>
      </xsl:if>
      <xsl:if test="$rateinfo/chargeDetails[type='9']">
        <VehicleCharge>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate($rateinfo/chargeDetails[type='9']/amount,'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after($rateinfo/chargeDetails[type='9']/amount,'.'))"/>
          </xsl:attribute>
          <xsl:attribute name="TaxInclusive">false</xsl:attribute>
          <xsl:attribute name="Description">Extra Hour Rate</xsl:attribute>
        </VehicleCharge>
      </xsl:if>
      <xsl:if test="$rateinfo[chargeDetails/type='31'] or $rateinfo[chargeDetails/type='32']">
        <VehicleCharge>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate($rateinfo[chargeDetails/type='31' or chargeDetails/type='32']/chargeDetails/amount,'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after($rateinfo[chargeDetails/type='31' or chargeDetails/type='32']/chargeDetails/amount,'.'))"/>
          </xsl:attribute>
          <xsl:attribute name="TaxInclusive">false</xsl:attribute>
          <xsl:attribute name="Description">Over Allowance Rate</xsl:attribute>
        </VehicleCharge>
      </xsl:if>
    </VehicleCharges>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Car Options Fields										-->
  <!--************************************************************************************-->
  <xsl:template match="optionDetail" mode="car">
    <xsl:choose>
      <xsl:when test="type = 'BS'">
        <CarOption>
          <xsl:attribute name="Option">Booking Source</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'RQ'">
        <CarOption>
          <xsl:attribute name="Option">Quoted Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'RB'">
        <CarOption>
          <xsl:attribute name="Option">Base Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'RC'">
        <CarOption>
          <xsl:attribute name="Option">Rate Code</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'RG'">
        <CarOption>
          <xsl:attribute name="Option">Guaranteed Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'NM'">
        <CarOption>
          <xsl:attribute name="Option">Reservation Last and First Names</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'ES'">
        <CarOption>
          <xsl:attribute name="Option">Estimated Total Rate</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'ARR'">
        <CarOption>
          <xsl:attribute name="Option">Pick Up Time</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'RT'">
        <CarOption>
          <xsl:attribute name="Option">DropOff Time</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'ID'">
        <CarOption>
          <xsl:attribute name="Option">Customer ID</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'GT'">
        <CarOption>
          <xsl:attribute name="Option">Payment Guarantee</xsl:attribute>
          <xsl:choose>
            <xsl:when test="contains(freetext, 'EXP')">
              <Text>
                <xsl:value-of select="substring(freetext, 1, 2)" />
                <xsl:value-of select="substring-before(substring(freetext, 3, string-length(freetext) - 4), 'EXP')" />
                <xsl:value-of select="substring(substring-after(freetext,'EXP'), 1, 2)" />
                <xsl:value-of select="substring(substring-after(freetext,'EXP'), 3, 2)" />
              </Text>
            </xsl:when>
          </xsl:choose>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'PU'">
        <CarOption>
          <xsl:attribute name="Option">Pick Up Location</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'AD'">
        <CarOption>
          <xsl:attribute name="Option">Customer Address</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'CD'">
        <CarOption>
          <xsl:attribute name="Option">Corporate Discount</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'SQ'">
        <CarOption>
          <xsl:attribute name="Option">Optional Equipment</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'PR'">
        <CarOption>
          <xsl:attribute name="Option">PrePayment Info</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'DL'">
        <CarOption>
          <xsl:attribute name="Option">Driver License</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'FT'">
        <CarOption>
          <xsl:attribute name="Option">Frequent Traveler Number</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'SI'">
        <CarOption>
          <xsl:attribute name="Option">Additional Info</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'DC'">
        <CarOption>
          <xsl:attribute name="Option">DropOff Charge</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'VC'">
        <CarOption>
          <xsl:attribute name="Option">Merchant Currency</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'AC'">
        <CarOption>
          <xsl:attribute name="Option">Alternate Currency</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'DO'">
        <CarOption>
          <xsl:attribute name="Option">DropOff Location</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
      <xsl:when test="type = 'TN'">
        <CarOption>
          <xsl:attribute name="Option">Tour Number</xsl:attribute>
          <Text>
            <xsl:value-of select="freetext" />
          </Text>
        </CarOption>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- ****************************************************************************************************************** -->
  <!-- PNR Data Elements   	              								                -->
  <!-- ****************************************************************************************************************** -->
  <!-- Phone Fields	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="phone">
    <xsl:if test="otherDataFreetext/freetextDetail/type!='P02'">
      <Telephone>
        <xsl:attribute name="PhoneUseType">
          <xsl:choose>
            <xsl:when test="otherDataFreetext/freetextDetail/type='4'">H</xsl:when>
            <xsl:when test="otherDataFreetext/freetextDetail/type='5'">O</xsl:when>
            <xsl:when test="otherDataFreetext/freetextDetail/type='P01'">F</xsl:when>
            <xsl:when test="otherDataFreetext/freetextDetail/type='6'">A</xsl:when>
            <xsl:when test="otherDataFreetext/freetextDetail/type='3'">B</xsl:when>
            <xsl:otherwise>O</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="PhoneNumber">
          <xsl:value-of select="otherDataFreetext/longFreetext"/>
        </xsl:attribute>

      </Telephone>
    </xsl:if>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Ticketing Element	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="ticketing">
    <Ticketing>
      <xsl:if test="ticketElement/ticket/time != ''">
        <xsl:attribute name="TicketTimeLimit">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(ticketElement/ticket/date,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(ticketElement/ticket/date,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(ticketElement/ticket/date,1,2)"/>
          <xsl:text>T</xsl:text>
          <xsl:value-of select="substring(ticketElement/ticket/time,1,2)"/>:<xsl:value-of select="substring(ticketElement/ticket/time,3,2)"/>
          <xsl:text>:00</xsl:text>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="TicketType">
        <xsl:choose>
          <xsl:when test="ticketElement/ticket/electronicTicketFlag !=''">eTicket</xsl:when>
          <xsl:when test="../../originDestinationDetails/itineraryInfo/travelProduct/typeDetail/detail ='ET'">eTicket</xsl:when>
          <xsl:otherwise>Paper</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="ticketElement/ticket/indicator = 'OK'">
        <TicketAdvisory>
          <xsl:text>OK-</xsl:text>
          <xsl:value-of select="ticketElement/ticket/date"/>
          <xsl:text>/</xsl:text>
          <xsl:value-of select="ticketElement/ticket/officeId"/>
        </TicketAdvisory>
      </xsl:if>
    </Ticketing>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Ticketing Remark	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="ticketingremark">
    <xsl:if test="ticketElement/ticket/freetext">
      <UniqueRemark>
        <xsl:attribute name="RemarkType">Ticketing</xsl:attribute>
        <xsl:value-of select="normalize-space(substring(ticketElement/ticket/freetext, 2, 15))"/>
      </UniqueRemark>
    </xsl:if>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Email Address	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="email">
    <!--done -->
    <xsl:if test="otherDataFreetext/freetextDetail/type='P02'">
      <Email>
        <xsl:value-of select="otherDataFreetext/longFreetext"/>
      </Email>
    </xsl:if>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Form of Payment	   		                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="Payment">
    <FormOfPayment>
      <xsl:choose>
        <xsl:when test="substring(otherDataFreetext/longFreetext,1,2) ='CC' or substring(otherDataFreetext/longFreetext,1,6) = 'PAX CC'">
          <xsl:attribute name="RPH">
            <xsl:value-of select="position()"/>
          </xsl:attribute>
          <PaymentCard>
            <xsl:attribute name="CardCode">
              <xsl:choose>
                <xsl:when test="substring(substring-after(otherDataFreetext/longFreetext,'CC'),1,2) = 'CA'">MC</xsl:when>
                <xsl:when test="substring(substring-after(otherDataFreetext/longFreetext,'CC'),1,2) = 'DC'">DN</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'CC'),1,2)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="CardNumber">
              <xsl:value-of select="translate(substring-before(substring(otherDataFreetext/longFreetext,5),'/'),'T','')"/>
            </xsl:attribute>
            <xsl:attribute name="ExpireDate">
              <xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'/'),1,2)"/>
              <xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'/'),3,2)"/>
            </xsl:attribute>
          </PaymentCard>
          <TPA_Extensions>
            <xsl:attribute name="FOPType">CC</xsl:attribute>
            <xsl:if test="contains(substring-after(otherDataFreetext/longFreetext,'/'),'/')">
              <xsl:attribute name="ConfirmationNumber">
                <xsl:value-of select="substring-after(substring-after(otherDataFreetext/longFreetext,'/'),'/')"/>
              </xsl:attribute>
            </xsl:if>
          </TPA_Extensions>
        </xsl:when>
        <xsl:when test="contains(otherDataFreetext/longFreetext,'CHECK')">
          <xsl:attribute name="RPH">
            <xsl:value-of select="position()" />
          </xsl:attribute>
          <DirectBill>
            <xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
          </DirectBill>
        </xsl:when>
        <xsl:when test="contains(otherDataFreetext/longFreetext,'CASH')">
          <xsl:attribute name="RPH">
            <xsl:value-of select="position()" />
          </xsl:attribute>
          <DirectBill>
            <xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
          </DirectBill>
        </xsl:when>
      </xsl:choose>
    </FormOfPayment>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Address	   		                                   		 -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="Address">
    <Address>
      <xsl:attribute name="UseType">
        <xsl:choose>
          <xsl:when test="elementManagementData/segmentName = 'AB/'">Billing</xsl:when>
          <xsl:when test="elementManagementData/segmentName = 'AB'">Billing</xsl:when>
          <xsl:otherwise>Mailing</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="elementManagementData/segmentName = 'AB/' or elementManagementData/segmentName = 'AM/'">
          <xsl:apply-templates select="structuredAddress"/>
        </xsl:when>
        <xsl:otherwise>
          <AddressLine>
            <xsl:value-of select="otherDataFreetext/longFreetext"/>
          </AddressLine>
        </xsl:otherwise>
      </xsl:choose>
    </Address>
  </xsl:template>

  <xsl:template match="structuredAddress">
    <xsl:if test="address[option='A1']">
      <StreetNmbr>
        <xsl:value-of select="address[option='A1']/optionText"/>
      </StreetNmbr>
    </xsl:if>
    <xsl:if test="address[option='A2']">
      <BldgRoom>
        <xsl:value-of select="address[option='A2']/optionText"/>
      </BldgRoom>
    </xsl:if>
    <xsl:if test="address[option='CI']">
      <CityName>
        <xsl:value-of select="address[option='CI']/optionText"/>
      </CityName>
    </xsl:if>
    <xsl:if test="address[option='ZP']">
      <PostalCode>
        <xsl:value-of select="address[option='ZP']/optionText"/>
      </PostalCode>
    </xsl:if>
    <xsl:if test="address[option='ST']">
      <StateProv>
        <xsl:choose>
          <xsl:when test="string-length(address[option='ST']/optionText) = 2">
            <xsl:attribute name="StateCode">
              <xsl:value-of select="address[option='ST']/optionText"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="address[option='ST']/optionText"/>
          </xsl:otherwise>
        </xsl:choose>
      </StateProv>
    </xsl:if>
    <xsl:if test="address[option='CO']">
      <CountryName>
        <xsl:choose>
          <xsl:when test="string-length(address[option='CO']/optionText) = 2">
            <xsl:attribute name="Code">
              <xsl:value-of select="address[option='CO']/optionText"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="address[option='CO']/optionText"/>
          </xsl:otherwise>
        </xsl:choose>
      </CountryName>
    </xsl:if>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Frequent Flyer                      		     		 -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="Fqtv">
    <xsl:param name="paxref"/>
    <xsl:if test="referenceForDataElement/reference[qualifier='PT']/number = $paxref">
      <CustLoyalty>
        <xsl:attribute name="ProgramID">
          <xsl:value-of select="frequentTravellerInfo/frequentTraveler/company"/>
        </xsl:attribute>
        <xsl:attribute name="MembershipID">
          <xsl:value-of select="frequentTravellerInfo/frequentTraveler/membershipNumber"/>
        </xsl:attribute>
      </CustLoyalty>
    </xsl:if>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- General Remarks	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="GenRemark">
    <Remark>
      <xsl:if test="miscellaneousRemarks/remarks/category != ''">
        <xsl:attribute name="Category">
          <xsl:value-of select="miscellaneousRemarks/remarks/category"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
    </Remark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!--Itinerary Remarks	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="ItinRemark">
    <SpecialRemark>
      <xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
      <FlightRefNumber>
        <xsl:attribute name="RPH">
          <xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
        </xsl:attribute>
      </FlightRefNumber>
      <Text>
        <xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Invoice Remarks	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="InvoiceRemark">
    <SpecialRemark>
      <xsl:attribute name="RemarkType">Invoice</xsl:attribute>
      <FlightRefNumber>
        <xsl:attribute name="RPH">
          <xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
        </xsl:attribute>
      </FlightRefNumber>
      <Text>
        <xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Invoice and itinerary Remarks	   	                              -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="InvoiceItinRemark">
    <SpecialRemark>
      <xsl:choose>
        <xsl:when test="referenceForDataElement/reference/qualifier='ST'">
          <xsl:attribute name="RemarkType">
            <xsl:value-of select="miscellaneousRemarks/remarks/category"/>
          </xsl:attribute>
          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:variable name="rph">
                <xsl:value-of select="referenceForDataElement/reference[qualifier='ST']/number"/>
              </xsl:variable>
              <xsl:value-of select="../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
            </xsl:attribute>
          </FlightRefNumber>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">
            <xsl:value-of select="miscellaneousRemarks/remarks/category"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Confidential Remarks	   	                              -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="ConfRemark">
    <SpecialRemark>
      <xsl:attribute name="RemarkType">C</xsl:attribute>
      <Text>
        <xsl:for-each select="miscellaneousRemarks/individualSecurity">
          <xsl:if test="position() &gt; 1">
            <xsl:text>,</xsl:text>
          </xsl:if>
          <xsl:value-of select="office"/>
        </xsl:for-each>
        <xsl:text>//</xsl:text>
        <xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Endorsement Remarks	   	                              -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="Endorsement">
    <SpecialRemark>
      <xsl:choose>
        <xsl:when test="referenceForDataElement/reference/qualifier='ST' or referenceForDataElement/reference/qualifier='PT'">
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
          <xsl:if test="referenceForDataElement/reference/qualifier='PT'">
            <TravelerRefNumber>
              <xsl:attribute name="RPH">
                <xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
                  <xsl:variable name="rph">
                    <xsl:value-of select="."/>
                  </xsl:variable>
                  <xsl:if test="position() > 1">
                    <xsl:text> </xsl:text>
                  </xsl:if>
                  <xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
                </xsl:for-each>
              </xsl:attribute>
            </TravelerRefNumber>
          </xsl:if>
          <xsl:if test="referenceForDataElement/reference/qualifier='ST'">
            <FlightRefNumber>
              <xsl:attribute name="RPH">
                <xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
                  <xsl:variable name="rph">
                    <xsl:value-of select="."/>
                  </xsl:variable>
                  <xsl:if test="position() > 1">
                    <xsl:text> </xsl:text>
                  </xsl:if>
                  <xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
                </xsl:for-each>
              </xsl:attribute>
            </FlightRefNumber>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="otherDataFreetext/longFreetext"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- OSI		   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="OSI">
    <OtherServiceInformation>
      <xsl:if test="otherDataFreetext/freetextDetail/type='28'">
        <Airline>
          <xsl:attribute name="Code">
            <xsl:value-of select="otherDataFreetext/freetextDetail/companyId"/>
          </xsl:attribute>
        </Airline>
        <Text>
          <xsl:value-of select="otherDataFreetext/longFreetext"/>
        </Text>
      </xsl:if>
      <xsl:if test="otherDataFreetext/freetextDetail/type='P27'">
        <Text>xsl:value-of select="otherDataFreetext/longFreetext"/></Text>
      </xsl:if>
    </OtherServiceInformation>
  </xsl:template>
  <xsl:template match="reference" mode="OSIPassAssoc">
    <xsl:variable name="Tattoo">
      <xsl:value-of select="number"/>
    </xsl:variable>
    <xsl:attribute name="RPH">
      <xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
    </xsl:attribute>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- SSR Elements 	                               		    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="SSR">
    <SpecialServiceRequest>
      <xsl:attribute name="SSRCode">
        <xsl:value-of select="serviceRequest/ssr/type"/>
      </xsl:attribute>
      <xsl:if test="referenceForDataElement/reference[qualifier = 'PT']/number != ''">
        <xsl:variable name="ref">
          <xsl:value-of select="referenceForDataElement/reference[qualifier = 'PT']/number"/>
        </xsl:variable>
        <xsl:attribute name="TravelerRefNumberRPHList">
          <xsl:value-of select="../../travellerInfo/elementManagementPassenger[reference/number = $ref]/lineNumber"/>
        </xsl:attribute>
      </xsl:if>
      <Airline>
        <xsl:attribute name="Code">
          <xsl:value-of select="serviceRequest/ssr/companyId"/>
        </xsl:attribute>
      </Airline>
      <Text>
        <xsl:if test="serviceRequest/ssr/freeText != ''">
          <xsl:value-of select="serviceRequest/ssr/freeText"/>
        </xsl:if>
        <xsl:if test="frequentTravellerInfo/frequentTraveler/membershipNumber != ''">
          <xsl:value-of select="frequentTravellerInfo/frequentTraveler/membershipNumber"/>
        </xsl:if>
      </Text>
    </SpecialServiceRequest>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Seat Elements 	                               		    -->
  <!-- ************************************************************** -->
  <xsl:template match="dataElementsIndiv" mode="Seat">
    <xsl:choose>
      <xsl:when test="referenceForDataElement/reference[qualifier='PT']">
        <xsl:for-each select="serviceRequest/ssrb">
          <xsl:variable name="pos">
            <xsl:value-of select="position()"/>
          </xsl:variable>
          <SeatRequest>
            <xsl:if test="data != ''">
              <xsl:attribute name="SeatNumber">
                <xsl:value-of select="data"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="seatType[position()=2] != ''">
              <xsl:attribute name="SeatPreference">
                <xsl:value-of select="seatType[position()=2]"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="SmokingAllowed">false</xsl:attribute>
            <xsl:attribute name="Status">
              <xsl:value-of select="../ssr/status"/>
            </xsl:attribute>
            <xsl:attribute name="TravelerRefNumberRPHList">
              <xsl:variable name="ref">
                <xsl:value-of select="../../referenceForDataElement/reference[qualifier = 'PT'][position()=$pos]/number"/>
              </xsl:variable>
              <xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number = $ref]/lineNumber"/>
              <!--xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='PT']" mode="SeatPassAssoc"/-->
            </xsl:attribute>
            <xsl:attribute name="FlightRefNumberRPHList">
              <xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
            </xsl:attribute>
            <DepartureAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="../ssr/boardpoint"/>
              </xsl:attribute>
            </DepartureAirport>
            <ArrivalAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="../ssr/offpoint"/>
              </xsl:attribute>
            </ArrivalAirport>
          </SeatRequest>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:for-each select="serviceRequest/ssrb[data != '']">
          <SeatRequest>
            <xsl:attribute name="SeatNumber">
              <xsl:value-of select="data"/>
            </xsl:attribute>
            <xsl:if test="seatType[position()=2] != ''">
              <xsl:attribute name="SeatPreference">
                <xsl:value-of select="seatType[position()=2]"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="SmokingAllowed">false</xsl:attribute>
            <xsl:attribute name="Status">
              <xsl:value-of select="../ssr/status"/>
            </xsl:attribute>
            <xsl:attribute name="TravelerRefNumberRPHList">
              <xsl:value-of select="position()"/>
            </xsl:attribute>
            <xsl:attribute name="FlightRefNumberRPHList">
              <xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
            </xsl:attribute>
            <DepartureAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="../ssr/boardpoint"/>
              </xsl:attribute>
            </DepartureAirport>
            <ArrivalAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="../ssr/offpoint"/>
              </xsl:attribute>
            </ArrivalAirport>
          </SeatRequest>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="reference" mode="SSRPassAssoc">
    <xsl:variable name="Tattoo">
      <xsl:value-of select="number"/>
    </xsl:variable>
    <xsl:attribute name="TravelerRefNumberRPHList">
      <xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="reference" mode="SeatPassAssoc">
    <xsl:variable name="Tattoo">
      <xsl:value-of select="number"/>
    </xsl:variable>
    <xsl:if test="position() > 1">
      <xsl:value-of select="string(' ')"/>
    </xsl:if>
    <xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
  </xsl:template>

  <xsl:template match="reference" mode="SeatSegAssoc">
    <xsl:variable name="Tattoo">
      <xsl:value-of select="number"/>
    </xsl:variable>
    <xsl:if test="position() > 1">
      <xsl:value-of select="string(' ')"/>
    </xsl:if>
    <xsl:value-of select="//originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number=$Tattoo]/lineNumber"/>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Form of Payment	   	                                    -->
  <!-- ************************************************************** -->
  <xsl:template match="fop">
    <xsl:if test="ElementType='FP'">
      <FOP>
        <xsl:if test="CAPI_PNR_NonFormattedElement/InformationType='16'">
          <ElementNumber>
            <xsl:attribute name="TattooNumber">
              <xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
            </xsl:attribute>
            <xsl:attribute name="TattooQualifier">
              <xsl:value-of select="elementManagementData/reference/qualifier"/>
            </xsl:attribute>
            <xsl:value-of select="ElementNum"/>
          </ElementNumber>
          <xsl:choose>
            <xsl:when test="substring(string(CAPI_PNR_NonFormattedElement/InformationText),1,2)='CC'">
              <CC>
                <CCCode>
                  <xsl:value-of select="substring(string(CAPI_PNR_NonFormattedElement/InformationText),3,2)"/>
                </CCCode>
                <CCNo>
                  <xsl:value-of select="substring(substring-before(string(CAPI_PNR_NonFormattedElement/InformationText),'/'),5,20)"/>
                </CCNo>
                <xsl:variable name="apcode">
                  <xsl:value-of select="substring-after(string(CAPI_PNR_NonFormattedElement/InformationText),'/')"/>
                </xsl:variable>
                <CCExpDate>
                  <xsl:value-of select="substring(string($apcode),1,4)"/>
                </CCExpDate>
                <CCApprovalCode>
                  <xsl:value-of select="substring-after(string($apcode),'/N')"/>
                </CCApprovalCode>
              </CC>
            </xsl:when>
            <xsl:otherwise>
              <Other>
                <xsl:value-of select="CAPI_PNR_NonFormattedElement/InformationText"/>
              </Other>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </FOP>
    </xsl:if>
  </xsl:template>

  <xsl:template match="dataElementsIndiv" mode="commission">
    <AgencyCommission>
      <xsl:choose>
        <xsl:when test="contains(otherDataFreetext/longFreetext,'*M*') or contains(otherDataFreetext/longFreetext,'*C*') or contains(otherDataFreetext/longFreetext,'*G*') or contains(otherDataFreetext/longFreetext,'*F*')">
          <xsl:variable name="comm">
            <xsl:choose>
              <xsl:when test="contains(otherDataFreetext/longFreetext,'*M*')">
                <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*M*')"/>
              </xsl:when>
              <xsl:when test="contains(otherDataFreetext/longFreetext,'*G*')">
                <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*G*')"/>
              </xsl:when>
              <xsl:when test="contains(otherDataFreetext/longFreetext,'*F*')">
                <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*F*')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*C*')"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="contains($comm,'A')">
              <xsl:attribute name="Amount">
                <xsl:value-of select="substring-before($comm,'A')"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Percent">
                <xsl:value-of select="$comm"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="contains(otherDataFreetext/longFreetext,'*AM*')">
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(substring-after(otherDataFreetext/longFreetext,'*AM*'),'A','')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="contains(otherDataFreetext/longFreetext,'A')">
          <xsl:attribute name="Amount">
            <xsl:value-of select="substring-before(otherDataFreetext/longFreetext,'A')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="Percent">
            <xsl:value-of select="otherDataFreetext/longFreetext"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </AgencyCommission>
  </xsl:template>

  <xsl:template match="dataElementsIndiv" mode="accounting">
    <AccountingLine>
      <xsl:value-of select="accounting/account/number"/>
    </AccountingLine>
  </xsl:template>

  <!-- ********************************************************************************	-->
  <!-- Miscellaneous other,  Air Taxi, Land, Sea, Rail, Car and hotel               -->
  <!-- ********************************************************************************* -->
  <xsl:template match="itineraryInfo" mode="Other">
    <!--General Segment in OTA -->
    <!-- Other Segments -->
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="relatedProduct/status"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
      </xsl:attribute>
      <General>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <Description>
          <xsl:choose>
            <xsl:when test="elementManagementItinerary/segmentName = 'RU'">Miscellaneous</xsl:when>
          </xsl:choose>
          <xsl:if test="travelProduct/boardpointDetail/cityCode !=''">
            <xsl:text> - Board point: </xsl:text>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </xsl:if>
          <xsl:if test="itineraryFreetext/longFreetext!=''">
            <xsl:text> - </xsl:text>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </xsl:if>
        </Description>
        <TPA_Extensions>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </General>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Taxi">
    <!--General OTA Segments -->
    <!-- Taxi Segments -->
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
      </xsl:attribute>
      <General>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="itineraryFreetext/longFreetext!=''">
          <Description>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </Description>
        </xsl:if>
        <TPA_Extensions>
          <xsl:attribute name="ActionCode">
            <xsl:value-of select="relatedProduct/status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </General>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Land">
    <!--OTA Package -->
    <!-- Surface Segments -->
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
      </xsl:attribute>
      <Package>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="itineraryFreetext/longFreetext!=''">
          <Description>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </Description>
        </xsl:if>
        <TPA_Extensions>
          <xsl:attribute name="ActionCode">
            <xsl:value-of select="relatedProduct/status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </Package>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Train">
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
      </xsl:attribute>
      <Rail>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="itineraryFreetext/longFreetext!=''">
          <Description>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </Description>
        </xsl:if>
        <TPA_Extensions>
          <xsl:attribute name="ActionCode">
            <xsl:value-of select="relatedProduct/status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </Rail>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Cruise">
    <!-- OTA Cruise-->
    <!-- Sea Segments -->
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
      </xsl:attribute>
      <Cruise>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="itineraryFreetext/longFreetext!=''">
          <Description>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </Description>
        </xsl:if>
        <TPA_Extensions>
          <xsl:attribute name="ActionCode">
            <xsl:value-of select="relatedProduct/status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </Cruise>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="Tour">
    <!--OTA Tour-->
    <!--Tour Segments -->
    <Item>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
      </xsl:attribute>
      <Tour>
        <xsl:attribute name="Start">
          <xsl:text>20</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
        </xsl:attribute>
        <xsl:if test="travelProduct/product/arrDate != ''">
          <xsl:attribute name="End">
            <xsl:text>20</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="itineraryFreetext/longFreetext!=''">
          <Description>
            <xsl:value-of select="itineraryFreetext/longFreetext"/>
          </Description>
        </xsl:if>
        <TPA_Extensions>
          <xsl:attribute name="ActionCode">
            <xsl:value-of select="relatedProduct/status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="relatedProduct/quantity"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="travelProduct/companyDetail/identification"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </Tour>
    </Item>
  </xsl:template>
  <!--***************************************************************************************************-->
  <xsl:template match="itineraryInfo" mode="CarPassive">
    <!-- Car Segments -->
    <CarPassiveSegment>
      <ElementNumber>
        <xsl:attribute name="TattooNumber">
          <xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
        </xsl:attribute>
        <xsl:attribute name="TattooQualifier">
          <xsl:value-of select="elementManagementItinerary/reference/qualifier"/>
        </xsl:attribute>
        <xsl:value-of select="elementManagementItinerary/lineNumber"/>
      </ElementNumber>
      <CarVendorCode>
        <xsl:value-of select="travelProduct/companyDetail/identification"/>
      </CarVendorCode>
      <CityCode>
        <xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
      </CityCode>
      <ActionCode>
        <xsl:value-of select="relatedProduct/status"/>
      </ActionCode>
      <NumberOfCars>
        <xsl:value-of select="relatedProduct/quantity"/>
      </NumberOfCars>
      <PickUpInfo>
        <Date>
          <xsl:value-of select="travelProduct/product/depDate"/>
        </Date>
      </PickUpInfo>
      <DropOffInfo>
        <Date>
          <xsl:value-of select="travelProduct/product/arrDate"/>
        </Date>
      </DropOffInfo>
      <CarType/>
      <xsl:if test="itineraryFreetext/longFreetext!=''">
        <Text>
          <xsl:value-of select="itineraryFreetext/longFreetext"/>
        </Text>
      </xsl:if>
    </CarPassiveSegment>
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
</xsl:stylesheet>
