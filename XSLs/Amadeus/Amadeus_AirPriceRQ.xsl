<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
  <!-- ================================================================== -->
  <!-- Amadeus_AirPriceRQ.xsl 															-->
  <!-- ================================================================== -->
  <!-- Date: 21 Jun 2013 -  Lakshitha - added <controlNumber>ABCDEF</controlNumber> in Line 431		-->
  <!-- Date: 19 Feb 2013 -  Rastko - added currency mapping in fare command			-->
  <!-- Date: 21 Apr 2011 -  Rastko - send cryptic entry when pricing IT fares			-->
  <!-- Date: 24 Jun 2009 -  Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="passengers">
    <xsl:variable name="inf">
      <xsl:choose>
        <xsl:when test="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity">
          <xsl:value-of select="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity"/>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="chd">
      <xsl:choose>
        <xsl:when test="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity">
          <xsl:value-of select="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity"/>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="adt">
      <xsl:variable name="all">
        <xsl:value-of select="sum(OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity/@Quantity)"/>
      </xsl:variable>
      <xsl:value-of select="$all - $inf - $chd"/>
    </xsl:variable>
    <xsl:variable name="adttypes">
      <xsl:for-each select="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']">
        <xsl:call-template name="gettypes">
          <xsl:with-param name="count">
            <xsl:choose>
              <xsl:when test="@Quantity != ''">
                <xsl:value-of select="@Quantity"/>
              </xsl:when>
              <xsl:otherwise>1</xsl:otherwise>
            </xsl:choose>
          </xsl:with-param>
          <xsl:with-param name="count1">1</xsl:with-param>
        </xsl:call-template>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="chdtypes">
      <xsl:for-each select="OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']">
        <xsl:call-template name="gettypes">
          <xsl:with-param name="count">
            <xsl:choose>
              <xsl:when test="@Quantity != ''">
                <xsl:value-of select="@Quantity"/>
              </xsl:when>
              <xsl:otherwise>1</xsl:otherwise>
            </xsl:choose>
          </xsl:with-param>
          <xsl:with-param name="count1">1</xsl:with-param>
        </xsl:call-template>
      </xsl:for-each>
    </xsl:variable>
    <xsl:call-template name="create_nameadt1">
      <xsl:with-param name="adt">
        <xsl:value-of select="$adt"/>
      </xsl:with-param>
      <xsl:with-param name="inf">
        <xsl:value-of select="$inf"/>
      </xsl:with-param>
      <xsl:with-param name="adttypes">
        <xsl:value-of select="$adttypes"/>
      </xsl:with-param>
      <xsl:with-param name="count">
        <xsl:value-of select="sum(OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']/@Quantity)"/>
      </xsl:with-param>
      <xsl:with-param name="count1">1</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="create_namechd1">
      <xsl:with-param name="adt">
        <xsl:value-of select="$adt"/>
      </xsl:with-param>
      <xsl:with-param name="chdtypes">
        <xsl:value-of select="$chdtypes"/>
      </xsl:with-param>
      <xsl:with-param name="count">
        <xsl:value-of select="sum(OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']/@Quantity)"/>
      </xsl:with-param>
      <xsl:with-param name="count1">1</xsl:with-param>
    </xsl:call-template>
  </xsl:variable>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_AirPriceRQ"/>
  </xsl:template>
  <xsl:template match="OTA_AirPriceRQ">
    <AirPrice>
      <AddMultiElements>
        <xsl:apply-templates select="AirItinerary"/>
      </AddMultiElements>
      <xsl:copy-of select="$passengers"/>
      <FarePlus>
        <xsl:choose>
          <xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='IIT']">
            <Cryptic_GetScreen_Query>
              <Command>
                <xsl:text>FXP/</xsl:text>
                <xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
                  <xsl:if test="position() > 1">
                    <xsl:text>//</xsl:text>
                  </xsl:if>
                  <xsl:text>P</xsl:text>
                  <xsl:choose>
                    <xsl:when test="@Code = 'IIT'">
                      <xsl:for-each select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='IIT']">
                        <xsl:if test="position() > 1">
                          <xsl:text>,</xsl:text>
                        </xsl:if>
                        <xsl:value-of select="TravelerRefNumber/@RPH"/>
                      </xsl:for-each>
                      <xsl:text>/PAX/R</xsl:text>
                      <xsl:value-of select="@Code"/>
                      <xsl:text>,U</xsl:text>
                      <xsl:if test="../../../POS/Source/@ISOCurrency != ''">
                        <xsl:text>,FC-</xsl:text>
                        <xsl:value-of select="../../../POS/Source/@ISOCurrency"/>
                      </xsl:if>
                    </xsl:when>
                    <xsl:when test="@Code = 'ITF'">
                      <xsl:for-each select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='INF']">
                        <xsl:if test="position() > 1">
                          <xsl:text>,</xsl:text>
                        </xsl:if>
                        <xsl:value-of select="TravelerRefNumber/@RPH"/>
                      </xsl:for-each>
                      <xsl:text>/INF/R</xsl:text>
                      <xsl:value-of select="@Code"/>
                    </xsl:when>
                    <xsl:when test="@Code = 'INN'">
                      <xsl:for-each select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='INN']">
                        <xsl:if test="position() > 1">
                          <xsl:text>,</xsl:text>
                        </xsl:if>
                        <xsl:value-of select="TravelerRefNumber/@RPH"/>
                      </xsl:for-each>
                      <xsl:text>/R</xsl:text>
                      <xsl:value-of select="@Code"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:for-each>
              </Command>
            </Cryptic_GetScreen_Query>
          </xsl:when>
          <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@FareQualifier='12'">
            <PoweredFare_PricePNRWithBookingClass>
              <overrideInformation>
                <attributeDetails>
                  <attributeType>RLO</attributeType>
                </attributeDetails>
                <xsl:choose>
                  <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
                    <attributeDetails>
                      <attributeType>RU</attributeType>
                    </attributeDetails>
                    <xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode!='JCB'">
                      <xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
                        <attributeDetails>
                          <attributeType>RW</attributeType>
                          <attributeDescription>
                            <xsl:value-of select="."/>
                          </attributeDescription>
                        </attributeDetails>
                      </xsl:for-each>
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <attributeDetails>
                      <attributeType>RP</attributeType>
                    </attributeDetails>
                  </xsl:otherwise>
                </xsl:choose>
              </overrideInformation>
              <xsl:if test="TravelPreferences/TicketDistribPref/@TicketTime != ''">
                <ticketingDate>
                  <date>
                    <xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,9,2)"/>
                    <xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,6,2)"/>
                    <xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,3,2)"/>
                  </date>
                </ticketingDate>
              </xsl:if>
              <xsl:if test="POS/Source/@ISOCurrency != ''">
                <currencyOverride>
                  <firstRateDetail>
                    <currencyCode>
                      <xsl:value-of select="POS/Source/@ISOCurrency"/>
                    </currencyCode>
                  </firstRateDetail>
                </currencyOverride>
              </xsl:if>
            </PoweredFare_PricePNRWithBookingClass>
          </xsl:when>
          <xsl:otherwise>
            <PoweredFare_PricePNRWithLowerFares>
              <overrideInformation>
                <attributeDetails>
                  <attributeType>RLA</attributeType>
                </attributeDetails>
                <attributeDetails>
                  <attributeType>CAB</attributeType>
                </attributeDetails>
                <attributeDetails>
                  <attributeType>RP</attributeType>
                </attributeDetails>
                <attributeDetails>
                  <attributeType>RU</attributeType>
                </attributeDetails>
                <xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
                  <attributeDetails>
                    <attributeType>RW</attributeType>
                    <attributeDescription>
                      <xsl:value-of select="@Code"/>
                    </attributeDescription>
                  </attributeDetails>
                </xsl:for-each>
              </overrideInformation>
              <xsl:if test="POS/Source/@ISOCurrency != ''">
                <currencyOverride>
                  <firstRateDetail>
                    <currencyCode>
                      <xsl:value-of select="POS/Source/@ISOCurrency"/>
                    </currencyCode>
                  </firstRateDetail>
                </currencyOverride>
              </xsl:if>
            </PoweredFare_PricePNRWithLowerFares>
          </xsl:otherwise>
        </xsl:choose>
      </FarePlus>
      <AirTravelerAvail>
        <xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity"/>
      </AirTravelerAvail>
    </AirPrice>
  </xsl:template>
  <xsl:template match="PassengerTypeQuantity">
    <PassengerTypeQuantity>
      <xsl:attribute name="Code">
        <xsl:value-of select="@Code"/>
      </xsl:attribute>
      <xsl:attribute name="Quantity">
        <xsl:value-of select="@Quantity"/>
      </xsl:attribute>
    </PassengerTypeQuantity>
  </xsl:template>
  <xsl:template match="AirItinerary">
    <PoweredPNR_AddMultiElements>
      <pnrActions>
        <optionCode>0</optionCode>
      </pnrActions>
      <xsl:variable name="inf">
        <xsl:choose>
          <xsl:when test="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity">
            <xsl:value-of select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity"/>
          </xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="chd">
        <xsl:choose>
          <xsl:when test="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity">
            <xsl:value-of select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity"/>
          </xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="adt">
        <xsl:variable name="all">
          <xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity/@Quantity)"/>
        </xsl:variable>
        <xsl:value-of select="$all - $inf - $chd"/>
      </xsl:variable>
      <xsl:variable name="adttypes">
        <xsl:for-each select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']">
          <xsl:call-template name="gettypes">
            <xsl:with-param name="count">
              <xsl:choose>
                <xsl:when test="@Quantity != ''">
                  <xsl:value-of select="@Quantity"/>
                </xsl:when>
                <xsl:otherwise>1</xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="count1">1</xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>
      </xsl:variable>
      <xsl:variable name="chdtypes">
        <xsl:for-each select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']">
          <xsl:call-template name="gettypes">
            <xsl:with-param name="count">
              <xsl:choose>
                <xsl:when test="@Quantity != ''">
                  <xsl:value-of select="@Quantity"/>
                </xsl:when>
                <xsl:otherwise>1</xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="count1">1</xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>
      </xsl:variable>
      <xsl:call-template name="create_nameadt">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="inf">
          <xsl:value-of select="$inf"/>
        </xsl:with-param>
        <xsl:with-param name="adttypes">
          <xsl:value-of select="$adttypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']/@Quantity)"/>
        </xsl:with-param>
        <xsl:with-param name="count1">1</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="create_namechd">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="chdtypes">
          <xsl:value-of select="$chdtypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']/@Quantity)"/>
        </xsl:with-param>
        <xsl:with-param name="count1">1</xsl:with-param>
      </xsl:call-template>
      <!--xsl:for-each select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
				<xsl:call-template name="create_name">
					<xsl:with-param name="count">
						<xsl:choose>
							<xsl:when test="@Quantity != ''"><xsl:value-of select="@Quantity" /></xsl:when>
							<xsl:otherwise>1</xsl:otherwise>
						</xsl:choose>
					</xsl:with-param>
					<xsl:with-param name="count1">1</xsl:with-param>
				</xsl:call-template>
			</xsl:for-each-->
      <xsl:apply-templates select="OriginDestinationOptions"/>
    </PoweredPNR_AddMultiElements>
  </xsl:template>
  <xsl:template match="OriginDestinationOptions">
    <xsl:apply-templates select="OriginDestinationOption"/>
  </xsl:template>
  <xsl:template match="OriginDestinationOption">
    <originDestinationDetails>
      <originDestination>
        <origin>
          <xsl:value-of select="FlightSegment/DepartureAirport/@LocationCode"/>
        </origin>
        <destination>
          <xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
        </destination>
      </originDestination>
      <xsl:apply-templates select="FlightSegment"/>
    </originDestinationDetails>
  </xsl:template>
  <xsl:template match="FlightSegment">
    <itineraryInfo>
      <elementManagementItinerary>
        <segmentName>AIR</segmentName>
      </elementManagementItinerary>
      <airAuxItinerary>
        <travelProduct>
          <product>
            <depDate>
              <xsl:value-of select="substring(@DepartureDateTime,9,2)"/>
              <xsl:value-of select="substring(@DepartureDateTime,6,2)"/>
              <xsl:value-of select="substring(@DepartureDateTime,3,2)"/>
            </depDate>
            <depTime>
              <xsl:value-of select="translate(substring(@DepartureDateTime,12,5),':','')"/>
            </depTime>
            <arrDate>
              <xsl:value-of select="substring(@ArrivalDateTime,9,2)"/>
              <xsl:value-of select="substring(@ArrivalDateTime,6,2)"/>
              <xsl:value-of select="substring(@ArrivalDateTime,3,2)"/>
            </arrDate>
            <arrTime>
              <xsl:value-of select="translate(substring(@ArrivalDateTime,12,5),':','')"/>
            </arrTime>
          </product>
          <boardpointDetail>
            <cityCode>
              <xsl:value-of select="DepartureAirport/@LocationCode"/>
            </cityCode>
          </boardpointDetail>
          <offpointDetail>
            <cityCode>
              <xsl:value-of select="ArrivalAirport/@LocationCode"/>
            </cityCode>
          </offpointDetail>
          <company>
            <identification>
              <xsl:value-of select="MarketingAirline/@Code"/>
            </identification>
          </company>
          <productDetails>
            <identification>
              <xsl:value-of select="@FlightNumber"/>
            </identification>
            <classOfService>
              <xsl:value-of select="@ResBookDesigCode"/>
            </classOfService>
          </productDetails>
        </travelProduct>
        <messageAction>
          <business>
            <function>1</function>
          </business>
        </messageAction>
        <relatedProduct>
          <quantity>
            <xsl:value-of select="../../../../TravelerInfoSummary/SeatsRequested"/>
          </quantity>
          <status>GK</status>
        </relatedProduct>
        <selectionDetailsAir>
          <selection>
            <option>P10</option>
          </selection>
        </selectionDetailsAir>
      </airAuxItinerary>
    </itineraryInfo>
  </xsl:template>
  <!--xsl:template name="create_name">
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:variable name="pos"><xsl:value-of select="position()" /></xsl:variable>
		<xsl:variable name="noinf"><xsl:value-of select="following-sibling::PassengerTypeQuantity/@Quantity"/></xsl:variable>
		<xsl:if test="$count !=0 and @Code!='INF'">
			<travellerInfo>
				<elementManagementPassenger>
					<reference>
						<qualifier>PR</qualifier>
						<number>
							<xsl:value-of select="$pos" />
						</number>
					</reference>
					<segmentName>NM</segmentName>
				</elementManagementPassenger>
				<travellerInformation>
					<traveller>
						<surname>SURNAME</surname>
						<quantity>
							<xsl:choose>
								<xsl:when test="@Code = 'ADT'  and following-sibling::PassengerTypeQuantity/@Code='INF' and not($count > $noinf)">2</xsl:when>
								<xsl:otherwise>1</xsl:otherwise>
							</xsl:choose>
						</quantity>
					</traveller>
					<passenger>
						<firstName>FIRSTNAME</firstName>
						<type>
							<xsl:choose>
								<xsl:when test="@Code='SRC'">YCD</xsl:when>
								<xsl:when test="@Code='SCR'">YCD</xsl:when>
								<xsl:when test="@Code='CHD'">C09</xsl:when>
								<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
							</xsl:choose>
						</type>
						<xsl:if test="@Code = 'ADT'  and following-sibling::PassengerTypeQuantity/@Code='INF' and not($count > $noinf)">
							<infantIndicator>3</infantIndicator>
						</xsl:if>
					</passenger>
				</travellerInformation>
				<xsl:if test="@Code = 'ADT'  and following-sibling::PassengerTypeQuantity/@Code='INF' and not($count > $noinf)">
					<travellerInformation>
						<traveller>
							<surname>PAX</surname>
						</traveller>
						<passenger>
							<firstName>INFANT</firstName>
							<type>INF</type>
						</passenger>
					</travellerInformation>
				</xsl:if>
			</travellerInfo>
			<xsl:call-template name="create_name">
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1" />
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template-->
  <xsl:template name="create_nameadt">
    <xsl:param name="adt"/>
    <xsl:param name="inf"/>
    <xsl:param name="adttypes"/>
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:if test="$count !=0">
      <travellerInfo>
        <elementManagementPassenger>
          <reference>
            <qualifier>PR</qualifier>
            <number>
              <xsl:value-of select="$count1"/>
            </number>
          </reference>
          <segmentName>NM</segmentName>
        </elementManagementPassenger>
        <travellerInformation>
          <traveller>
            <surname>SURNAME</surname>
            <quantity>
              <xsl:choose>
                <xsl:when test="$count1 > $inf">1</xsl:when>
                <xsl:otherwise>2</xsl:otherwise>
              </xsl:choose>
            </quantity>
          </traveller>
          <passenger>
            <firstName>FIRSTNAME</firstName>
            <type>
              <xsl:choose>
                <xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SRC'">YCD</xsl:when>
                <xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SCR'">YCD</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring($adttypes,($count1 * 3) - 2,3)"/>
                </xsl:otherwise>
              </xsl:choose>
            </type>
            <xsl:if test="$count1 &lt; $inf or $count1 = $inf">
              <infantIndicator>3</infantIndicator>
            </xsl:if>
          </passenger>
        </travellerInformation>
        <xsl:if test="$count1 &lt; $inf or $count1 = $inf">
          <travellerInformation>
            <traveller>
              <surname>INFSURNAME</surname>
            </traveller>
            <passenger>
              <firstName>INFANT</firstName>
              <type>INF</type>
            </passenger>
          </travellerInformation>
        </xsl:if>
      </travellerInfo>
      <xsl:call-template name="create_nameadt">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="inf">
          <xsl:value-of select="$inf"/>
        </xsl:with-param>
        <xsl:with-param name="adttypes">
          <xsl:value-of select="$adttypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count - 1"/>
        </xsl:with-param>
        <xsl:with-param name="count1">
          <xsl:value-of select="$count1 + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="create_namechd">
    <xsl:param name="adt"/>
    <xsl:param name="chdtypes"/>
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:variable name="pos">
      <xsl:value-of select="$count1 + $adt"/>
    </xsl:variable>
    <xsl:if test="$count !=0">
      <travellerInfo>
        <elementManagementPassenger>
          <reference>
            <qualifier>PR</qualifier>
            <number>
              <xsl:value-of select="$pos"/>
            </number>
          </reference>
          <segmentName>NM</segmentName>
        </elementManagementPassenger>
        <travellerInformation>
          <traveller>
            <surname>SURNAME</surname>
            <quantity>1</quantity>
          </traveller>
          <passenger>
            <firstName>FIRSTNAME</firstName>
            <type>
              <xsl:value-of select="substring($chdtypes,($count1 * 3) - 2,3)"/>
            </type>
          </passenger>
        </travellerInformation>
      </travellerInfo>
      <xsl:call-template name="create_namechd">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="chdtypes">
          <xsl:value-of select="$chdtypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count - 1"/>
        </xsl:with-param>
        <xsl:with-param name="count1">
          <xsl:value-of select="$count1 + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="create_nameadt1">
    <xsl:param name="adt"/>
    <xsl:param name="inf"/>
    <xsl:param name="adttypes"/>
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:if test="$count !=0">
      <Traveler>
        <xsl:attribute name="PassengerTypeCode">
          <xsl:choose>
            <xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SRC'">YCD</xsl:when>
            <xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SCR'">YCD</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring($adttypes,($count1 * 3) - 2,3)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <PersonName>
          <GivenName>FIRSTNAME</GivenName>
          <Surname>SURNAME</Surname>
        </PersonName>
        <TravelerRefNumber>
          <xsl:attribute name="RPH">
            <xsl:value-of select="$count1"/>
          </xsl:attribute>
        </TravelerRefNumber>
      </Traveler>
      <xsl:if test="$count1 &lt; $inf or $count1 = $inf">
        <Traveler>
          <xsl:attribute name="PassengerTypeCode">INF</xsl:attribute>
          <PersonName>
            <GivenName>INFANT</GivenName>
            <Surname>INFSURNAME</Surname>
          </PersonName>
          <TravelerRefNumber>
            <xsl:attribute name="RPH">
              <xsl:value-of select="$count1"/>
            </xsl:attribute>
          </TravelerRefNumber>
        </Traveler>
      </xsl:if>
      <xsl:call-template name="create_nameadt1">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="inf">
          <xsl:value-of select="$inf"/>
        </xsl:with-param>
        <xsl:with-param name="adttypes">
          <xsl:value-of select="$adttypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count - 1"/>
        </xsl:with-param>
        <xsl:with-param name="count1">
          <xsl:value-of select="$count1 + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="create_namechd1">
    <xsl:param name="adt"/>
    <xsl:param name="chdtypes"/>
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:variable name="pos">
      <xsl:value-of select="$count1 + $adt"/>
    </xsl:variable>
    <xsl:if test="$count !=0">
      <Traveler>
        <xsl:attribute name="PassengerTypeCode">
          <xsl:value-of select="substring($chdtypes,($count1 * 3) - 2,3)"/>
        </xsl:attribute>
        <PersonName>
          <GivenName>FIRSTNAME</GivenName>
          <Surname>SURNAME</Surname>
        </PersonName>
        <TravelerRefNumber>
          <xsl:attribute name="RPH">
            <xsl:value-of select="$pos"/>
          </xsl:attribute>
        </TravelerRefNumber>
      </Traveler>
      <xsl:call-template name="create_namechd1">
        <xsl:with-param name="adt">
          <xsl:value-of select="$adt"/>
        </xsl:with-param>
        <xsl:with-param name="chdtypes">
          <xsl:value-of select="$chdtypes"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count - 1"/>
        </xsl:with-param>
        <xsl:with-param name="count1">
          <xsl:value-of select="$count1 + 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="gettypes">
    <xsl:param name="count"/>
    <xsl:param name="count1"/>
    <xsl:if test="$count !=0">
      <xsl:choose>
        <xsl:when test="@Code='CH'">CHD</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@Code"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="gettypes">
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
