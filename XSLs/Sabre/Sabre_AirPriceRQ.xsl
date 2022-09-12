<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Sabre_AirPriceRQ.xsl 																-->
  <!-- ================================================================== -->
  <!-- Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				-->
  <!-- Date: 02 Mar 2016 - Rastko - added mapping for negotiated code				-->
  <!-- Date: 31 Aug 2010 - Rastko - corrected marriage group element			       -->
  <!-- Date: 26 Aug 2010 - Rastko - added marriage element							-->
  <!-- Date: 16 Dec 2008 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="PCC">
    <xsl:value-of select="OTA_AirPriceRQ/POS/Source/@PseudoCityCode"/>
  </xsl:variable>
  <xsl:variable name="NIP">
    <xsl:value-of select="OTA_AirPriceRQ/TravelerInfoSummary/SeatsRequested"/>
  </xsl:variable>
  <xsl:template match="/">
    <AirPrice>
      <xsl:apply-templates select="OTA_AirPriceRQ" />
    </AirPrice>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="OTA_AirPriceRQ">
    <AirBook>
      <xsl:apply-templates select="AirItinerary"/>
    </AirBook>
    <Read>
      <TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
        <MessagingDetails>
          <SubjectAreas>
            <SubjectArea>FULL</SubjectArea>
          </SubjectAreas>
        </MessagingDetails>
      </TravelItineraryReadRQ>
    </Read>
    <Pricing>
      <xsl:apply-templates select="TravelerInfoSummary"/>
    </Pricing>
    <Ignore>
      <IgnoreTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
        <POS>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
          </xsl:attribute>
        </POS>
        <IgnoreTransaction Ind="true"/>
      </IgnoreTransactionRQ>
    </Ignore>
  </xsl:template>

  <xsl:template match="AirItinerary">
    <OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
          </xsl:attribute>
        </Source>
      </POS>
      <AirItinerary>
        <xsl:attribute name="DirectionInd">
          <xsl:choose>
            <xsl:when test="@DirectionInd = 'Oneway'">Oneway</xsl:when>
            <xsl:otherwise>Return</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <OriginDestinationOptions>
          <OriginDestinationOption>
            <xsl:apply-templates select="OriginDestinationOptions/OriginDestinationOption" />
          </OriginDestinationOption>
        </OriginDestinationOptions>
      </AirItinerary>
    </OTA_AirBookRQ>
  </xsl:template>

  <xsl:template match="TravelerInfoSummary">
    <OTA_AirPriceRQ  xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.12.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
          </xsl:attribute>
        </Source>
      </POS>
      <TravelerInfoSummary>
        <xsl:if test="../POS/Source/@ISOCurrency!='' or PriceRequestInformation/NegotiatedFareCode/@Code!=''">
          <PriceRequestInformation>
            <xsl:if test="../POS/Source/@ISOCurrency!=''">
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="../POS/Source/@ISOCurrency"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="PriceRequestInformation/NegotiatedFareCode/@Code!=''">
              <TPA_Extensions>
                <Account Code="{PriceRequestInformation/NegotiatedFareCode/@Code}"/>
              </TPA_Extensions>
            </xsl:if>
          </PriceRequestInformation>
        </xsl:if>
        <TPA_Extensions>
          <BargainFinder>
            <xsl:attribute name="Ind">
              <xsl:choose>
                <xsl:when test="PriceRequestInformation/@FareQualifier = '12'">false</xsl:when>
                <xsl:otherwise>true</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </BargainFinder>
          <xsl:for-each select="AirTravelerAvail/PassengerTypeQuantity">
            <PassengerType>
              <xsl:attribute name="Quantity">
                <xsl:value-of select="@Quantity"/>
              </xsl:attribute>
              <xsl:attribute name="Code">
                <xsl:choose>
                  <xsl:when test="@Code='CHD'">C09</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="@Code"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="AlternatePassengerType">true</xsl:attribute>
            </PassengerType>
          </xsl:for-each>
          <xsl:choose>
            <xsl:when test="PriceRequestInformation/@PricingSource ='Private'">
              <PrivateFare Ind="true" />
            </xsl:when>
            <xsl:otherwise>
              <PublicFare Ind="true" />
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="PriceRequestInformation/NegotiatedFareCode/@CodeContext = 'FareBase'">
            <CommandPricing RPH="1">
              <FareBasisCode>
                <xsl:value-of select="PriceRequestInformation/NegotiatedFareCode"/>
              </FareBasisCode>
            </CommandPricing>
          </xsl:if>
        </TPA_Extensions>
      </TravelerInfoSummary>
    </OTA_AirPriceRQ>
  </xsl:template>

  <xsl:template match="OriginDestinationOption">
    <!--OriginDestinationOption-->
    <xsl:apply-templates select="FlightSegment" />
    <!--/OriginDestinationOption-->
  </xsl:template>

  <xsl:template match="FlightSegment">
    <FlightSegment>
      <xsl:attribute name="ActionCode">NN</xsl:attribute>
      <xsl:attribute name="DepartureDateTime">
        <xsl:value-of select="@DepartureDateTime" />
      </xsl:attribute>
      <xsl:attribute name="ArrivalDateTime">
        <xsl:value-of select="@ArrivalDateTime" />
      </xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="@FlightNumber" />
      </xsl:attribute>
      <xsl:attribute name="NumberInParty">
        <xsl:value-of select="$NIP" />
      </xsl:attribute>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:value-of select="@ResBookDesigCode" />
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="DepartureAirport/@LocationCode" />
        </xsl:attribute>
        <xsl:attribute name="CodeContext">IATA</xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="ArrivalAirport/@LocationCode" />
        </xsl:attribute>
        <xsl:attribute name="CodeContext">IATA</xsl:attribute>
      </ArrivalAirport>
      <OperatingAirline>
        <xsl:choose>
          <xsl:when test="OperatingAirline">
            <xsl:attribute name="Code">
              <xsl:value-of select="OperatingAirline/@Code" />
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Code">
              <xsl:value-of select="MarketingAirline/@Code" />
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </OperatingAirline>
      <xsl:if test="Equipment">
        <Equipment>
          <xsl:attribute name="AirEquipType">
            <xsl:value-of select="Equipment/@AirEquipType" />
          </xsl:attribute>
        </Equipment>
      </xsl:if>
      <MarketingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="MarketingAirline/@Code" />
        </xsl:attribute>
      </MarketingAirline>
      <xsl:if test="MarriageGrp!=''">
        <MarriageGrp>
          <xsl:attribute name="Ind">
            <xsl:value-of select="MarriageGrp"/>
          </xsl:attribute>
        </MarriageGrp>
      </xsl:if>
    </FlightSegment>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="AirTravelerAvail">
    <xsl:apply-templates select="PassengerTypeQuantity" />
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="PassengerTypeQuantity">
    <PassengerTypeQuantity>
      <xsl:attribute name="Code">
        <xsl:value-of select="@Code" />
      </xsl:attribute>
      <xsl:attribute name="Quantity">
        <xsl:value-of select="@Quantity" />
      </xsl:attribute>
    </PassengerTypeQuantity>
  </xsl:template>
  <!--************************************************************************************************************	-->
</xsl:stylesheet>
