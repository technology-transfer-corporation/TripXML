<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_LowFareFlightsRS.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 21 June 2013 - Lakshitha - Added priceTariffType='N' for Private fare			-->
  <!-- Date: 12 May 2011 - Rastko - corrected calculation of base and total fare			-->
  <!-- Date: 21 Mar 2011 - Rastko - corrected ticket time limit mapping				-->
  <!-- Date: 21 Mar 2011 - Rastko - corrected NumberInParty to be sum instead of count		-->
  <!-- Date: 13 Mar 2011 - Rastko - corrected e-ticket mapping						-->
  <!-- Date: 12 Mar 2011 - Rastko - added one extra test for possible errors returned		-->
  <!-- Date: 21 Jan 2011 - Rastko - corrected mapping of SectorSequence			-->
  <!-- Date: 03 Jan 2011 - Rastko - new file												-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="not(FS/AirAvail/Air_MultiAvailabilityReply/singleCityPairInfo)">
        <OTA_AirLowFareSearchFlightsRS Version="1.001">
          <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
          <Errors>
            <xsl:apply-templates select="FS/AirAvail/Air_MultiAvailabilityReply/errorOrWarningSection/textInformation" mode="Error"/>
          </Errors>
        </OTA_AirLowFareSearchFlightsRS>
      </xsl:when>
      <xsl:when test="FS/AirAvail/Air_MultiAvailabilityReply/singleCityPairInfo/cityPairErrorOrWarning/cityPairErrorOrWarningText[freeTextQualification/codedIndicator='3']">
        <OTA_AirLowFareSearchFlightsRS Version="1.001">
          <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
          <Errors>
            <xsl:apply-templates select="FS/AirAvail/Air_MultiAvailabilityReply/singleCityPairInfo/cityPairErrorOrWarning/cityPairErrorOrWarningText[freeTextQualification/codedIndicator='3']" mode="Error"/>
          </Errors>
        </OTA_AirLowFareSearchFlightsRS>
      </xsl:when>
      <xsl:when test="FS/Fare_InformativePricingWithoutPNRReply/errorGroup/errorMessage/freeText!=''">
        <OTA_AirLowFareSearchFlightsRS Version="1.001">
          <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
          <Errors>
            <xsl:apply-templates select="FS/Fare_InformativePricingWithoutPNRReply/errorGroup/errorMessage" mode="Error"/>
          </Errors>
        </OTA_AirLowFareSearchFlightsRS>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="FS"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="cityPairErrorOrWarningText" mode="Error">
    <Error Type="Amadeus">
      <xsl:value-of select="concat(../../locationDetails/origin,' ')"/>
      <xsl:value-of select="concat(../../locationDetails/destination,' ')"/>
      <xsl:value-of select="freeText"/>
    </Error>
  </xsl:template>
  <xsl:template match="textInformation | errorMessage" mode="Error">
    <Error Type="Amadeus">
      <xsl:value-of select="freeText"/>
    </Error>
  </xsl:template>
  <xsl:template match="FS">
    <OTA_AirLowFareSearchFlightsRS>
      <xsl:attribute name="Version">1.001</xsl:attribute>
      <xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
      <Success/>
      <PricedItineraries>
        <PricedItinerary>
          <xsl:attribute name="SequenceNumber">1</xsl:attribute>
          <AirItinerary>
            <xsl:attribute name="DirectionInd">
              <xsl:choose>
                <xsl:when test="AirAvail/Air_MultiAvailabilityReply[1]/singleCityPairInfo/locationDetails/origin = AirAvail/Air_MultiAvailabilityReply[position()=last()]/singleCityPairInfo/locationDetails/destination">Circle</xsl:when>
                <xsl:otherwise>OneWay</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <OriginDestinationOptions>
              <xsl:apply-templates select="AirAvail/Air_MultiAvailabilityReply[1]">
                <xsl:with-param name="pot">1</xsl:with-param>
                <xsl:with-param name="pos">2</xsl:with-param>
                <xsl:with-param name="posOD">1</xsl:with-param>
              </xsl:apply-templates>
            </OriginDestinationOptions>
          </AirItinerary>
          <xsl:apply-templates select="Fare_InformativePricingWithoutPNRReply"/>
        </PricedItinerary>
        <xsl:if test="count(AirAvail/Air_MultiAvailabilityReply[1]/singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D']) > 1 or count(AirAvail/Air_MultiAvailabilityReply[position()=2]/singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D']) > 1 or count(AirAvail/Air_MultiAvailabilityReply[position()=3]/singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D']) > 1">
          <PricedItinerary>
            <xsl:attribute name="SequenceNumber">2</xsl:attribute>
            <AirItinerary>
              <OriginDestinationOptions>
                <xsl:apply-templates select="AirAvail/Air_MultiAvailabilityReply[1]">
                  <xsl:with-param name="pot">1</xsl:with-param>
                  <xsl:with-param name="pos">99</xsl:with-param>
                  <xsl:with-param name="posOD">1</xsl:with-param>
                </xsl:apply-templates>
              </OriginDestinationOptions>
            </AirItinerary>
          </PricedItinerary>
        </xsl:if>
      </PricedItineraries>
    </OTA_AirLowFareSearchFlightsRS>
  </xsl:template>
  <!--****************************************************************************************-->
  <!--											    -->
  <!--****************************************************************************************-->
  <xsl:template match="Air_MultiAvailabilityReply">
    <xsl:param name="pot"/>
    <xsl:param name="pos"/>
    <xsl:param name="posOD"/>
    <xsl:variable name="flights">
      <OriginDestinationOption>
        <xsl:attribute name="SectorSequence">
          <xsl:value-of select="$posOD"/>
        </xsl:attribute>
        <xsl:for-each select="singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='S' or basicFlightInfo/productTypeDetail/productIndicators='D'][position() &lt; $pos]">
          <xsl:apply-templates select="." mode="leg">
            <xsl:with-param name="pot">
              <xsl:value-of select="$pot"/>
            </xsl:with-param>
            <xsl:with-param name="rph">
              <xsl:value-of select="position()"/>
            </xsl:with-param>
            <xsl:with-param name="pos" select="$pos"/>
          </xsl:apply-templates>
          <xsl:if test="basicFlightInfo/productTypeDetail/productIndicators = 'S'">
            <xsl:apply-templates select="following-sibling::flightInfo[1]" mode="conx">
              <xsl:with-param name="pot">
                <xsl:value-of select="$pot + 1"/>
              </xsl:with-param>
              <xsl:with-param name="rph">
                <xsl:value-of select="position()"/>
              </xsl:with-param>
              <xsl:with-param name="pos" select="$pos"/>
            </xsl:apply-templates>
          </xsl:if>
        </xsl:for-each>
      </OriginDestinationOption>
    </xsl:variable>
    <xsl:copy-of select="$flights"/>
    <xsl:apply-templates select="following-sibling::Air_MultiAvailabilityReply[1]">
      <xsl:with-param name="pot">
        <xsl:value-of select="$pot + count(msxsl:node-set($flights)/OriginDestinationOption/FlightSegment)"/>
      </xsl:with-param>
      <xsl:with-param name="pos" select="$pos"/>
      <xsl:with-param name="posOD">
        <xsl:value-of select="$posOD + 1"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>
  <xsl:template match="flightInfo" mode="conx">
    <xsl:param name="pot"/>
    <xsl:param name="rph"/>
    <xsl:param name="pos"/>
    <xsl:variable name="ind">
      <xsl:value-of select="basicFlightInfo/productTypeDetail/productIndicators[1]"/>
    </xsl:variable>
    <xsl:if test="$ind != 'S'">
      <xsl:apply-templates select="." mode="leg">
        <xsl:with-param name="pot">
          <xsl:value-of select="$pot"/>
        </xsl:with-param>
        <xsl:with-param name="rph">
          <xsl:value-of select="$rph"/>
        </xsl:with-param>
        <xsl:with-param name="pos" select="$pos"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="following-sibling::flightInfo[1]" mode="conx">
        <xsl:with-param name="pot">
          <xsl:value-of select="$pot + 1"/>
        </xsl:with-param>
        <xsl:with-param name="rph">
          <xsl:value-of select="$rph"/>
        </xsl:with-param>
        <xsl:with-param name="pos" select="$pos"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <xsl:template match="flightInfo" mode="leg">
    <xsl:param name="pot"/>
    <xsl:param name="rph"/>
    <xsl:param name="pos"/>
    <xsl:variable name="posit">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <xsl:variable name="depday">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),1,2)"/>
    </xsl:variable>
    <xsl:variable name="depmonth">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),3,2)"/>
    </xsl:variable>
    <xsl:variable name="depyear">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),5,2)"/>
    </xsl:variable>
    <xsl:variable name="arrday">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),1,2)"/>
    </xsl:variable>
    <xsl:variable name="arrmonth">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),3,2)"/>
    </xsl:variable>
    <xsl:variable name="arryear">
      <xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),5,2)"/>
    </xsl:variable>
    <FlightSegment>
      <xsl:variable name="zeros">000</xsl:variable>
      <xsl:attribute name="DepartureDateTime">
        20<xsl:value-of select="$depyear"/>-<xsl:value-of select="$depmonth"/>-<xsl:value-of select="$depday"/>T<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureTime),1,2)"/>:<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureTime),3,2)"/>:00
      </xsl:attribute>
      <xsl:attribute name="ArrivalDateTime">
        20<xsl:value-of select="$arryear"/>-<xsl:value-of select="$arrmonth"/>-<xsl:value-of select="$arrday"/>T<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalTime),1,2)"/>:<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalTime),3,2)"/>:00
      </xsl:attribute>
      <xsl:attribute name="StopQuantity">
        <xsl:choose>
          <xsl:when test="additionalFlightInfo/flightDetails/numberOfStops!=''">
            <xsl:value-of select="additionalFlightInfo/flightDetails/numberOfStops"/>
          </xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$rph"/>
      </xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="basicFlightInfo/flightIdentification/number"/>
      </xsl:attribute>
      <xsl:variable name="classes">
        <xsl:for-each select="../../../paxFareProduct[1]/fareDetails">
          <xsl:for-each select="groupOfFares">
            <xsl:value-of select="productInformation/cabinProduct/rbd"/>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:variable>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:choose>
          <xsl:when test="$pos='2'">
            <xsl:value-of select="../../../../Fare_InformativePricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[position()=last()]/fareInfoGroup/segmentLevelGroup[position()=$pot]/segmentInformation/flightIdentification/bookingClass"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="../../../../Fare_InformativePricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[position()=1]/fareInfoGroup/segmentLevelGroup[position()=1]/segmentInformation/flightIdentification/bookingClass"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="NumberInParty">
        <xsl:value-of select="sum(../../../../Fare_InformativePricingWithoutPNRReply/AirTravelerAvail/PassengerTypeQuantity[@Code!= 'INF' and @Code!= 'IN']/@Quantity)"/>
      </xsl:attribute>
      <xsl:attribute name="E_TicketEligibility">
        <xsl:choose>
          <xsl:when test="../../../../Fare_InformativePricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[position()=1]/fareInfoGroup/textData[contains(freeText,'FARE VALID FOR E TICKET ONLY')]">Eligible</xsl:when>
          <xsl:when test="basicFlightInfo/productTypeDetail/productIndicators='ET'">Eligible</xsl:when>
          <xsl:otherwise>NotEligible</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="basicFlightInfo/departureLocation/cityAirport"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="basicFlightInfo/arrivalLocation/cityAirport"/>
        </xsl:attribute>
      </ArrivalAirport>
      <OperatingAirline>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="basicFlightInfo/operatingCompany/identifier!= ''">
              <xsl:value-of select="basicFlightInfo/operatingCompany/identifier"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="basicFlightInfo/marketingCompany/identifier"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </OperatingAirline>
      <Equipment>
        <xsl:attribute name="AirEquipType">
          <xsl:value-of select="additionalFlightInfo/flightDetails/typeOfAircraft"/>
        </xsl:attribute>
      </Equipment>
      <MarketingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="basicFlightInfo/marketingCompany/identifier"/>
        </xsl:attribute>
      </MarketingAirline>
      <TPA_Extensions>
        <xsl:variable name="cabin">
          <xsl:value-of select="cabinClassInfo/cabinInfo/cabinDesignation/cabinClassOfService"/>
        </xsl:variable>
        <CabinType>
          <xsl:attribute name="Cabin">
            <xsl:choose>
              <xsl:when test="$cabin = '1'">First</xsl:when>
              <xsl:when test="$cabin = '2'">Business</xsl:when>
              <xsl:otherwise>Economy</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </CabinType>
        <JourneyTotalDuration>
          <xsl:choose>
            <xsl:when test="additionalFlightInfo/flightDetails/legDuration!=''">
              <xsl:value-of select="substring(additionalFlightInfo/flightDetails/legDuration,1,2)"/>
              <xsl:text>:</xsl:text>
              <xsl:value-of select="substring(additionalFlightInfo/flightDetails/legDuration,3,4)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(following-sibling::flightInfo[additionalFlightInfo/flightDetails/legDuration!='']/additionalFlightInfo/flightDetails/legDuration,1,2)"/>
              <xsl:text>:</xsl:text>
              <xsl:value-of select="substring(following-sibling::flightInfo[additionalFlightInfo/flightDetails/legDuration!='']/additionalFlightInfo/flightDetails/legDuration,3,4)"/>
            </xsl:otherwise>
          </xsl:choose>
        </JourneyTotalDuration>
        <xsl:if test="../../../specificRecDetails[1]/specificProductDetails/fareContextDetails[position()=1]/cnxContextDetails[position()=$posit]/fareCnxInfo/contextDetails/availabilityCnxType!=''">
          <FlightContext>
            <xsl:value-of select="../../../specificRecDetails[1]/specificProductDetails/fareContextDetails[position()=1]/cnxContextDetails[position()=$posit]/fareCnxInfo/contextDetails/availabilityCnxType"/>
          </FlightContext>
        </xsl:if>
      </TPA_Extensions>
    </FlightSegment>
  </xsl:template>
  <xsl:template match="Fare_InformativePricingWithoutPNRReply">
    <xsl:variable name="nip" select="sum(AirTravelerAvail/PassengerTypeQuantity[@Code!='INF']/@Quantity)"/>
    <AirItineraryPricingInfo>
      <xsl:choose>
        <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'PRIVATE RATES USED')]">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/priceTariffType='N'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/priceTariffType='P'">
          <xsl:attribute name="PricingSource">Private</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="PricingSource">Published</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:attribute name="ValidatingAirlineCode">
        <xsl:value-of select="substring-before(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[starts-with(freeText,'PRICED WITH VALIDATING CARRIER')]/freeText,'PRICED WITH VALIDATING CARRIER '),' ')"/>
      </xsl:attribute>
      <ItinTotalFare>
        <xsl:if test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/segmentLevelGroup[1]/additionalInformation/idNumber!=''">
          <xsl:attribute name="NegotiatedFareCode">
            <xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/segmentLevelGroup[1]/additionalInformation/idNumber"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:variable name="bf">
          <xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup" mode="totalbase">
            <xsl:with-param name="sum">0</xsl:with-param>
            <xsl:with-param name="pos">1</xsl:with-param>
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="tf">
          <xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup" mode="totalprice">
            <xsl:with-param name="sum">0</xsl:with-param>
            <xsl:with-param name="pos">1</xsl:with-param>
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="curt">
          <xsl:choose>
            <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
              <xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/currency"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="dect">
          <xsl:choose>
            <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
              <xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$bf"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$curt"/>
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
              <xsl:value-of select="$curt"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </Tax>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$tf"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$curt"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect"/>
          </xsl:attribute>
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:for-each select="AirTravelerAvail/PassengerTypeQuantity">
          <xsl:variable name="pos">
            <xsl:value-of select="position()"/>
          </xsl:variable>
          <xsl:variable name="paxtype">
            <xsl:choose>
              <xsl:when test="@Code = 'SRC'">YCD</xsl:when>
              <xsl:when test="@Code = 'ITF'">INF</xsl:when>
              <xsl:when test="@Code = 'ITS'">INS</xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@Code"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:apply-templates select="../../mainGroup/pricingGroupLevelGroup[position()=$pos]" mode="paxtypes">
            <xsl:with-param name="paxtype">
              <xsl:value-of select="$paxtype"/>
            </xsl:with-param>
            <xsl:with-param name="nip">
              <xsl:value-of select="@Quantity"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:for-each>
      </PTC_FareBreakdowns>
      <!--FareInfos>
				<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup/fareInfoGroup/segmentLevelGroup" mode="fareinfo" />
			</FareInfos-->
    </AirItineraryPricingInfo>
    <TicketingInfo>
      <xsl:attribute name="TicketTimeLimit">
        <xsl:text>20</xsl:text>
        <xsl:choose>
          <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate!=''">
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,1,2)"/>
          </xsl:when>
          <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]">
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,6,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:call-template name="month">
              <xsl:with-param name="month">
                <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,3,3)"/>
              </xsl:with-param>
            </xsl:call-template>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,1,2)"/>
          </xsl:when>
          <xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'DATE OF ORIGIN')]">
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,5)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,3,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,1,2)"/>
          </xsl:when>
        </xsl:choose>
        <xsl:text>T23:59:00</xsl:text>
      </xsl:attribute>
    </TicketingInfo>
  </xsl:template>
  <xsl:template match="fareInfoGroup" mode="totalbase">
    <xsl:param name="sum"/>
    <xsl:param name="pos"/>
    <xsl:param name="nip"/>
    <xsl:variable name="nopt">
      <xsl:value-of select="../numberOfPax/segmentControlDetails/numberOfUnits"/>
    </xsl:variable>
    <xsl:variable name="tot">
      <xsl:choose>
        <xsl:when test="fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
          <xsl:value-of select="translate(fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.','') * $nopt"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="translate(fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.','') * $nopt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="../../pricingGroupLevelGroup[$pos + 1]">
        <xsl:apply-templates select="../../pricingGroupLevelGroup[$pos + 1]/fareInfoGroup" mode="totalbase">
          <xsl:with-param name="sum">
            <xsl:value-of select="$tot + $sum"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="nip" select="$nip"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tot + $sum"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="fareInfoGroup" mode="totalprice">
    <xsl:param name="sum"/>
    <xsl:param name="pos"/>
    <xsl:param name="nip"/>
    <xsl:variable name="nopt">
      <xsl:value-of select="../numberOfPax/segmentControlDetails/numberOfUnits"/>
    </xsl:variable>
    <xsl:variable name="tot">
      <xsl:value-of select="translate(fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.','') * $nopt"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="../../pricingGroupLevelGroup[$pos + 1]">
        <xsl:apply-templates select="../../pricingGroupLevelGroup[$pos + 1]/fareInfoGroup" mode="totalprice">
          <xsl:with-param name="sum">
            <xsl:value-of select="$tot + $sum"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="nip" select="$nip"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tot + $sum"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="warningInformation">
    <Warnings>
      <Warning>
        <xsl:attribute name="Type">Amadeus</xsl:attribute>
        <xsl:value-of select="warningText/errorFreeText"/>
      </Warning>
    </Warnings>
  </xsl:template>
  <xsl:template match="pricingGroupLevelGroup" mode="paxtypes">
    <xsl:param name="paxtype"/>
    <xsl:param name="nip"/>
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="$paxtype"/>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="$nip"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <FareBasisCodes>
        <xsl:apply-templates select="fareInfoGroup/segmentLevelGroup/fareBasis/additionalFareDetails/rateClass" mode="farebasis"/>
      </FareBasisCodes>
      <PassengerFare>
        <xsl:variable name="bfpax">
          <xsl:choose>
            <xsl:when test="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
              <xsl:value-of select="translate(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.','') * $nip"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="translate(fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.','') * $nip"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="tfpax">
          <xsl:value-of select="translate(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.','') * $nip"/>
        </xsl:variable>
        <xsl:variable name="cur">
          <xsl:choose>
            <xsl:when test="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
              <xsl:value-of select="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/currency"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="dec">
          <xsl:value-of select="string-length(substring-after(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.'))"/>
        </xsl:variable>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$bfpax"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur"/>
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
              <xsl:value-of select="$cur"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dec"/>
            </xsl:attribute>
          </Tax>
          <xsl:apply-templates select="fareInfoGroup/surchargesGroup/taxesAmount/taxDetails">
            <xsl:with-param name="nip">
              <xsl:value-of select="$nip"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$tfpax"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="$cur"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dec"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <PricedCode>
          <xsl:value-of select="fareInfoGroup/segmentLevelGroup[1]/ptcSegment/quantityDetails/unitQualifier"/>
        </PricedCode>
        <xsl:if test="fareInfoGroup/textData[contains(freeText,'NOT FARED AT PASSENGER TYPE REQUESTED')]">
          <Text>NOT FARED AT PASSENGER TYPE REQUESTED *5*</Text>
        </xsl:if>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>
  <xsl:template match="taxDetails">
    <xsl:param name="nip"/>
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="countryCode"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(rate,'.','') * $nip"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <xsl:template match="rateClass" mode="farebasis">
    <FareBasisCode>
      <xsl:value-of select="."/>
    </FareBasisCode>
  </xsl:template>
  <xsl:template match="segmentLevelGroup" mode="fareinfo">
    <xsl:variable name="pos">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <FareInfo>
      <xsl:variable name="segment" select="../../../../FlightSegments/FlightSegment[position()=$pos]"/>
      <DepartureDate>
        <xsl:value-of select="$segment/@DepartureDateTime"/>
      </DepartureDate>
      <FareReference>
        <xsl:value-of select="fareBasis/additionalFareDetails/secondRateClass"/>
      </FareReference>
      <xsl:if test="../textData/freeText = 'NON-REFUNDABLE'">
        <RuleInfo>
          <ChargesRules>
            <VoluntaryChanges>
              <Penalty PenaltyType="Ticket Is Non Refundable"/>
            </VoluntaryChanges>
          </ChargesRules>
        </RuleInfo>
      </xsl:if>
      <FilingAirline>
        <xsl:value-of select="$segment/MarketingAirline/@Code"/>
      </FilingAirline>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$segment/DepartureAirport/@LocationCode"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="$segment/ArrivalAirport/@LocationCode"/>
        </xsl:attribute>
      </ArrivalAirport>
    </FareInfo>
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
</xsl:stylesheet>
