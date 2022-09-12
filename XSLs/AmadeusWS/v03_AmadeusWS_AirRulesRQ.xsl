<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- v03_AmadeusWS_AirRulesRQ.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 21 Oct 2015 - Rastko - adapted to latest version of IP wihout PNR (14.1)	 -->
  <!-- Date : 13 Oct 2015 - Rastko - added new tag trigger						-->
  <!-- Date : 14 Mar 2013 - Rastko - added stopover category (8)						-->
  <!-- Date : 19 Jul 2012 - Shashin - request only specified rules categories			-->
  <!-- Date: 14 May 2012 - Shashin - Mapped nego fare code							-->
  <!-- Date: 07 Jul 2011 - Rastko - added pax type index								-->
  <!-- Date: 17 Feb 2010 - Rastko - new version  to use informative pricing from Amadeus 		-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="user" select="OTA_AirRulesRQ/POS/TPA_Extensions/Provider/Userid"/>
  <xsl:template match="/">
    <AirRules>
      <xsl:apply-templates select="OTA_AirRulesRQ"/>
    </AirRules>
  </xsl:template>
  <xsl:template match="OTA_AirRulesRQ">
    <xsl:choose>
      <xsl:when test="$user='Downtown'or $user='Autoticket'">
        <Fare_InformativePricingWithoutPNR>
          <xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
            <passengersGroup>
              <segmentRepetitionControl>
                <segmentControlDetails>
                  <quantity>1</quantity>
                  <numberOfUnits>
                    <xsl:value-of select="@Quantity"/>
                  </numberOfUnits>
                </segmentControlDetails>
              </segmentRepetitionControl>
              <xsl:variable name="paxid">
                <xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
              </xsl:variable>
              <travellersID>
                <xsl:call-template name="create_names">
                  <xsl:with-param name="count">
                    <xsl:value-of select="@Quantity"/>
                  </xsl:with-param>
                  <xsl:with-param name="count1">
                    <xsl:value-of select="$paxid + 1"/>
                  </xsl:with-param>
                </xsl:call-template>
              </travellersID>
              <discountPtc>
                <valueQualifier>
                  <xsl:value-of select="@Code"/>
                </valueQualifier>
                <xsl:if test="@Code='INF'">
                  <fareDetails>
                    <qualifier>766</qualifier>
                  </fareDetails>
                </xsl:if>
              </discountPtc>
            </passengersGroup>
          </xsl:for-each>
          <xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption"/>
          <xsl:choose>
            <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
              <pricingOptionGroup>
                <pricingOptionKey>
                  <pricingOptionKey>RP</pricingOptionKey>
                </pricingOptionKey>
              </pricingOptionGroup>
              <pricingOptionGroup>
                <pricingOptionKey>
                  <pricingOptionKey>RU</pricingOptionKey>
                </pricingOptionKey>
              </pricingOptionGroup>
            </xsl:when>
            <xsl:when test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
              <pricingOptionGroup>
                <pricingOptionKey>
                  <pricingOptionKey>RW</pricingOptionKey>
                </pricingOptionKey>
                <optionDetail>
                  <criteriaDetails>
                    <attributeType>
                      <xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
                    </attributeType>
                  </criteriaDetails>
                </optionDetail>
              </pricingOptionGroup>
            </xsl:when>
            <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
              <pricingOptionGroup>
                <pricingOptionKey>
                  <pricingOptionKey>RU</pricingOptionKey>
                </pricingOptionKey>
              </pricingOptionGroup>
            </xsl:when>
            <xsl:otherwise>
              <pricingOptionGroup>
                <pricingOptionKey>
                  <pricingOptionKey>RP</pricingOptionKey>
                </pricingOptionKey>
              </pricingOptionGroup>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="POS/Source/@ISOCurrency!=''">
            <pricingOptionGroup>
              <pricingOptionKey>
                <pricingOptionKey>FCO</pricingOptionKey>
              </pricingOptionKey>
              <currency>
                <firstCurrencyDetails>
                  <currencyQualifier>FCO</currencyQualifier>
                  <currencyIsoCode>
                    <xsl:value-of select="POS/Source/@ISOCurrency"/>
                  </currencyIsoCode>
                </firstCurrencyDetails>
              </currency>
            </pricingOptionGroup>
          </xsl:if>
          <pricingOptionGroup>
            <pricingOptionKey>
              <pricingOptionKey>CAB</pricingOptionKey>
            </pricingOptionKey>
            <optionDetail>
              <criteriaDetails>
                <attributeType>FC</attributeType>
                <attributeDescription>
                  <xsl:choose>
                    <xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='F'">
                      <xsl:value-of select="'F'"/>
                    </xsl:when>
                    <xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='C'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                    <xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='W'">
                      <xsl:value-of select="'W'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'Y'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </attributeDescription>
              </criteriaDetails>
            </optionDetail>
          </pricingOptionGroup>
          <xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode">
            <pricingOptionGroup>
              <pricingOptionKey>
                <pricingOptionKey>VC</pricingOptionKey>
              </pricingOptionKey>
              <carrierInformation>
                <companyIdentification>
                  <otherCompany>
                    <xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/>
                  </otherCompany>
                </companyIdentification>
              </carrierInformation>
            </pricingOptionGroup>
          </xsl:if>
          <xsl:if test="POS/Source/@AirportCode != ''">
            <pricingOptionGroup>
              <pricingOptionKey>
                <pricingOptionKey>POS</pricingOptionKey>
              </pricingOptionKey>
              <locationInformation>
                <locationType>POS</locationType>
                <firstLocationDetails>
                  <code>
                    <xsl:value-of select="POS/Source/@AirportCode "/>
                  </code>
                </firstLocationDetails>
              </locationInformation>
            </pricingOptionGroup>
          </xsl:if>
          <pricingOptionGroup>
            <pricingOptionKey>
              <pricingOptionKey>RLA</pricingOptionKey>
            </pricingOptionKey>
          </pricingOptionGroup>
          <pricingOptionGroup>
            <pricingOptionKey>
              <pricingOptionKey>TKT</pricingOptionKey>
            </pricingOptionKey>
            <optionDetail>
              <criteriaDetails>
                <attributeType>ET</attributeType>
              </criteriaDetails>
            </optionDetail>
          </pricingOptionGroup>
        </Fare_InformativePricingWithoutPNR>
      </xsl:when>
      <xsl:otherwise>
        <Fare_InformativePricingWithoutPNR>
          <messageDetails>
            <messageFunctionDetails>
              <businessFunction>1</businessFunction>
              <messageFunction>741</messageFunction>
              <responsibleAgency>1A</responsibleAgency>
            </messageFunctionDetails>
          </messageDetails>
          <corporateFares>
            <corporateFareIdentifiers>
              <fareQualifier>
                <xsl:choose>
                  <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">UP</xsl:when>
                  <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">U</xsl:when>
                  <xsl:otherwise>P</xsl:otherwise>
                </xsl:choose>
              </fareQualifier>
              <xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
                <corporateID>
                  <xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
                </corporateID>
              </xsl:if>
            </corporateFareIdentifiers>
          </corporateFares>
          <xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
            <passengersGroup>
              <segmentRepetitionControl>
                <segmentControlDetails>
                  <quantity>1</quantity>
                  <numberOfUnits>
                    <xsl:value-of select="@Quantity"/>
                  </numberOfUnits>
                </segmentControlDetails>
              </segmentRepetitionControl>
              <xsl:variable name="paxid">
                <xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
              </xsl:variable>
              <travellersID>
                <xsl:call-template name="create_names">
                  <xsl:with-param name="count">
                    <xsl:value-of select="@Quantity"/>
                  </xsl:with-param>
                  <xsl:with-param name="count1">
                    <xsl:value-of select="$paxid + 1"/>
                  </xsl:with-param>
                </xsl:call-template>
              </travellersID>
              <ptcGroup>
                <discountPtc>
                  <valueQualifier>
                    <xsl:value-of select="@Code"/>
                  </valueQualifier>
                </discountPtc>
              </ptcGroup>
            </passengersGroup>
          </xsl:for-each>
          <xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode">
            <pricingOptionsGroup>
              <pricingDetails>
                <priceTicketDetails>
                  <indicators>L</indicators>
                  <indicators>E</indicators>
                  <indicators>VOA</indicators>
                </priceTicketDetails>
                <companyDetails>
                  <marketingCompany>
                    <xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/>
                  </marketingCompany>
                </companyDetails>
              </pricingDetails>
            </pricingOptionsGroup>
          </xsl:if>
          <xsl:apply-templates select="AirItinerary/OriginDestinationOptions"/>
        </Fare_InformativePricingWithoutPNR>
      </xsl:otherwise>
    </xsl:choose>
    <Fare_CheckRules>
      <msgType>
        <messageFunctionDetails>
          <messageFunction>712</messageFunction>
        </messageFunctionDetails>
      </msgType>
      <itemNumber>
        <itemNumberDetails>
          <number>0x</number>
        </itemNumberDetails>
        <itemNumberDetails>
          <number>n</number>
          <type>FC</type>
        </itemNumberDetails>
      </itemNumber>
      <fareRule>
        <tarifFareRule>
          <ruleSectionId>50</ruleSectionId>
          <ruleSectionId>16</ruleSectionId>
          <ruleSectionId>15</ruleSectionId>
          <ruleSectionId>14</ruleSectionId>
          <ruleSectionId>8</ruleSectionId>
        </tarifFareRule>
      </fareRule>
    </Fare_CheckRules>
  </xsl:template>
  <xsl:template match="OriginDestinationOptions">
    <tripsGroup>
      <originDestination>
        <origin>
          <xsl:value-of select="OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode"/>
        </origin>
        <destination>
          <xsl:value-of select="OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
        </destination>
      </originDestination>
      <xsl:apply-templates select="OriginDestinationOption"/>
    </tripsGroup>
  </xsl:template>
  <xsl:template match="OriginDestinationOption">
    <xsl:apply-templates select="FlightSegment"/>
  </xsl:template>
  <xsl:template match="FlightSegment">
    <segmentGroup>
      <segmentInformation>
        <flightDate>
          <departureDate>
            <xsl:value-of select="substring(@DepartureDateTime,9,2)"/>
            <xsl:value-of select="substring(@DepartureDateTime,6,2)"/>
            <xsl:value-of select="substring(@DepartureDateTime,3,2)"/>
          </departureDate>
        </flightDate>
        <boardPointDetails>
          <trueLocationId>
            <xsl:value-of select="DepartureAirport/@LocationCode"/>
          </trueLocationId>
        </boardPointDetails>
        <offpointDetails>
          <trueLocationId>
            <xsl:value-of select="ArrivalAirport/@LocationCode"/>
          </trueLocationId>
        </offpointDetails>
        <companyDetails>
          <marketingCompany>
            <xsl:value-of select="MarketingAirline/@Code"/>
          </marketingCompany>
        </companyDetails>
        <flightIdentification>
          <flightNumber>
            <xsl:value-of select="@FlightNumber"/>
          </flightNumber>
          <bookingClass>
            <xsl:value-of select="@ResBookDesigCode"/>
          </bookingClass>
        </flightIdentification>
      </segmentInformation>
      <xsl:if test="$user='ForestTravel' or $user='JetBrave'">
        <trigger/>
      </xsl:if>
    </segmentGroup>
  </xsl:template>
  <xsl:template name="create_names">
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:if test="$count !=0">
      <travellerDetails>
        <measurementValue>
          <xsl:value-of select="$count1"/>
        </measurementValue>
      </travellerDetails>
      <xsl:call-template name="create_names">
        <xsl:with-param name="count">
          <xsl:value-of select="$count - 1"/>
        </xsl:with-param>
        <xsl:with-param name="count1">
          <xsl:value-of select="$count1 + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
