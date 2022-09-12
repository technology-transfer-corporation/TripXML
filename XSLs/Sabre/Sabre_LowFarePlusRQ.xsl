<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
   Sabre_LowFarePlusRQ.xsl 														
  ================================================================== 
  Date: 11 Mar 2020 - Kobelev - added branded fares			
  Date: 06 May 2014 - Rastko - added mapping for no penalty fares			
  Date: 29 Apr 2014 - Rastko - mapped refundable fares option					
  Date: 30 Dec 2011 - Rastko - added negotiated code mapping				
  Date: 18 Feb 2011 - Rastko - added code to get operating airline in response		
  Date: 16 Sep 2010 - Rastko - removed mapping of connections as not supported by Sabre	
  Date: 18 Feb 2010 - Rastko														
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="requestor" select="OTA_AirLowFareSearchPlusRQ/POS/Source/RequestorID/@ID | OTA_AirLowFareSearchRQ/POS/Source/RequestorID/@ID| OTA_AirLowFareSearchMatrixRQ/POS/Source/RequestorID/@ID"/>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ | OTA_AirLowFareSearchMatrixRQ" />
  </xsl:template>
  <!--************************************************************************************************************ -->
  <xsl:template match="OTA_AirLowFareSearchPlusRQ | OTA_AirLowFareSearchMatrixRQ">
    <OTA_AirLowFareSearchRQ xmlns="http://www.opentravel.org/OTA/2003/05" AvailableFlightsOnly="true" Version="6.2.0">
      <xsl:attribute name="DirectFlightsOnly">
        <xsl:choose>
          <xsl:when test="TravelPreferences/FlightTypePref/@FlightType='Nonstop'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:choose>
              <xsl:when test="POS/TPA_Extensions/Provider/Name[. = 'Sabre']/@PseudoCityCode != ''">
                <xsl:value-of select="POS/TPA_Extensions/Provider/Name[. = 'Sabre']/@PseudoCityCode" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="POS/Source/@PseudoCityCode" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <RequestorID ID="1" Type="1">
            <CompanyName Code="TN">TN</CompanyName>
          </RequestorID>
        </Source>
      </POS>
      <xsl:apply-templates select="OriginDestinationInformation"/>
      <TravelPreferences ETicketDesired="true">
        <xsl:apply-templates select="TravelPreferences/VendorPref" mode="include"/>
        <xsl:if test="TravelPreferences/CabinPref/@Cabin !='' ">
          <CabinPref PreferLevel="Only">
            <xsl:attribute name="Cabin">
              <xsl:choose>
                <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'First'">F</xsl:when>
                <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Business'">C</xsl:when>
                <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Premium'">S</xsl:when>
                <xsl:otherwise>Y</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </CabinPref>
        </xsl:if>
        <xsl:if test="not(TravelerInfoSummary/TPA_Extensions/FareFamilies/FareFamily/@Hierarchy='0')">
          <TPA_Extensions>
            <!--OnlineIndicator Ind="true"/-->
            <xsl:apply-templates select="TravelPreferences/VendorPref" mode="exclude"/>
            <KeepSameCabin Enabled="true"/>
            <xsl:if test="./TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
              <FlexibleFares>
                <FareParameters>
                  <PrivateFare Ind="true"/>
                  <xsl:if test="TravelPreferences/CabinPref/@Cabin !='' ">
                    <Cabin>
                      <xsl:attribute name="Type">
                        <xsl:choose>
                          <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'First'">F</xsl:when>
                          <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Business'">C</xsl:when>
                          <xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Premium'">S</xsl:when>
                          <xsl:otherwise>Y</xsl:otherwise>
                        </xsl:choose>
                      </xsl:attribute>
                    </Cabin>
                  </xsl:if>
                  <PassengerTypeQuantity Code="JCB">
                    <xsl:attribute name="Quantity">
                      <xsl:choose>
                        <xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity != ''">
                          <xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='JCB']/@Quantity"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:attribute>
                  </PassengerTypeQuantity>
                  <xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Quantity!='0' and @Code!='ADT']">
                    <PassengerTypeQuantity>
                      <xsl:attribute name="Code">
                        <xsl:choose>
                          <xsl:when test="@Code = 'CHD'">JNN</xsl:when>
                          <xsl:when test="@Code = 'INF'">JNF</xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="@Code" />
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:attribute>
                      <xsl:attribute name="Quantity">
                        <xsl:value-of select="@Quantity" />
                      </xsl:attribute>
                    </PassengerTypeQuantity>
                  </xsl:for-each>
                </FareParameters>
              </FlexibleFares>
            </xsl:if>
          </TPA_Extensions>
        </xsl:if>
      </TravelPreferences>
      <TravelerInfoSummary>
        <AirTravelerAvail>
          <xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Quantity!='0']">
            <PassengerTypeQuantity>
              <xsl:attribute name="Code">
                <xsl:choose>
                  <xsl:when test="@Code = 'CHD'">C09</xsl:when>
                  <xsl:when test="@Code = 'SCR'">SRC</xsl:when>
                  <xsl:when test="../../PriceRequestInformation/@PricingSource='Private' and not(../..//PriceRequestInformation/NegotiatedFareCode/@Code)">JCB</xsl:when>
                  <xsl:when test="@Code = 'JCB'">ADT</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="@Code" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="Quantity">
                <xsl:value-of select="@Quantity" />
              </xsl:attribute>
              <xsl:if test="$requestor='CanadaOne'">
                <TPA_Extensions>
                  <VoluntaryChanges Match="All">
                    <Penalty Type="Exchange"/>
                  </VoluntaryChanges>
                </TPA_Extensions>
              </xsl:if>
            </PassengerTypeQuantity>
          </xsl:for-each>
        </AirTravelerAvail>
        <PriceRequestInformation>
          <xsl:if test="POS/Source/@ISOCurrency!=''">
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="POS/Source/@ISOCurrency"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode[string-length(@Code) =5]">
            <NegotiatedFareCode>
              <xsl:attribute name="Code">
                <xsl:value-of select="@Code"/>
              </xsl:attribute>
            </NegotiatedFareCode>
          </xsl:for-each>
          <TPA_Extensions>
            <xsl:choose>
              <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
                <PublicFare Ind="false" />
                <PrivateFare Ind="true" />
              </xsl:when>
              <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Published'">
                <PublicFare Ind="true" />
                <PrivateFare Ind="false" />
              </xsl:when>
              <xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
                <PublicFare Ind="true" />
              </xsl:when>
            </xsl:choose>
            <xsl:choose>
              <xsl:when test="OriginDestinationInformation/DepartureDateTime/@WindowBefore !='' or OriginDestinationInformation/DepartureDateTime/@WindowAfter !=''">
                <Priority>
                  <Price Priority="2" />
                  <DirectFlights Priority="3" />
                  <Time Priority="1" />
                  <Vendor Priority="4" />
                </Priority>
              </xsl:when>
              <xsl:otherwise>
                <Priority>
                  <Price Priority="1"/>
                  <DirectFlights Priority="2"/>
                  <Time Priority="3"/>
                  <Vendor Priority="4"/>
                </Priority>
              </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="TravelPreferences/FareRestrictPref/VoluntaryChanges/Penalty/@PenaltyType='Ref'">
              <Indicators>
                <RetainFare Ind="false" />
                <MinMaxStay Ind="true" />
                <RefundPenalty Ind="false" />
                <ResTicketing Ind="true" />
                <TravelPolicy Ind="true" />
              </Indicators>
            </xsl:if>
            <xsl:if test="TravelerInfoSummary/TPA_Extensions/FareFamilies/FareFamily/@Hierarchy='0'">
              <BrandedFareIndicators SingleBrandedFare="true" MultipleBrandedFares="true" UpsellLimit="3" />
            </xsl:if>
          </TPA_Extensions>
        </PriceRequestInformation>
      </TravelerInfoSummary>
      <TPA_Extensions>
        <IntelliSellTransaction>
          <RequestType>
            <xsl:attribute name="Name">
              <xsl:choose>
                <xsl:when test="../OTA_AirLowFareSearchMatrixRQ">
                  <xsl:choose>
                    <xsl:when test="../OTA_AirLowFareSearchMatrixRQ/OriginDestinationInformation/DepartureDateTime/@WindowBefore > 0 and ../OTA_AirLowFareSearchMatrixRQ/OriginDestinationInformation/DepartureDateTime/@WindowBefore &lt; 4">
                      <xsl:value-of select="concat('AD',../OTA_AirLowFareSearchMatrixRQ/OriginDestinationInformation/DepartureDateTime/@WindowBefore)"/>
                    </xsl:when>
                    <xsl:otherwise>AD3</xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <!--xsl:when test="OriginDestinationInformation/DepartureDateTime/@WindowBefore!=''">
									<xsl:choose>
										<xsl:when test="OriginDestinationInformation/DepartureDateTime/@WindowBefore > 0 and OriginDestinationInformation/DepartureDateTime/@WindowBefore &lt; 4">
											<xsl:value-of select="concat('AD',OriginDestinationInformation/DepartureDateTime/@WindowBefore)"/>
										</xsl:when>
										<xsl:otherwise>AD3</xsl:otherwise>
									</xsl:choose>
								</xsl:when-->
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$requestor='CheapFlightsFreak'">200</xsl:when>
                    <xsl:when test="$requestor='Corpodia' or $requestor='UpAndAway'">50</xsl:when>
                    <xsl:when test="@MaxResponses!=''">
                      <xsl:choose>
                        <xsl:when test="@MaxResponses &gt; 200">50</xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@MaxResponses"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>50</xsl:otherwise>
                  </xsl:choose>
                  <xsl:value-of select="string('ITINS')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </RequestType>
        </IntelliSellTransaction>
      </TPA_Extensions>
    </OTA_AirLowFareSearchRQ>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="OriginDestinationInformation">
    <xsl:variable name="pos">
      <xsl:value-of select="position()" />
    </xsl:variable>
    <OriginDestinationInformation>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$pos"/>
      </xsl:attribute>
      <DepartureDateTime>
        <xsl:value-of select="DepartureDateTime" />
      </DepartureDateTime>
      <xsl:if test="DepartureDateTime/@WindowBefore != '' or DepartureDateTime/@WindowAfter != ''">
        <xsl:variable name="hh">
          <xsl:value-of select="substring(DepartureDateTime,12,2)"/>
        </xsl:variable>
        <xsl:variable name="mm">
          <xsl:value-of select="substring(DepartureDateTime,15,2)"/>
        </xsl:variable>
        <xsl:variable name="lrange1">
          <xsl:choose>
            <xsl:when test="DepartureDateTime/@WindowBefore != ''">
              <xsl:variable name="wb">
                <xsl:value-of select="DepartureDateTime/@WindowBefore"/>
              </xsl:variable>
              <xsl:value-of select="format-number($hh - $wb,'00')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$hh"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:value-of select="$mm"/>
        </xsl:variable>
        <xsl:variable name="lrange">
          <xsl:choose>
            <xsl:when test="$lrange1 &lt; '1'">0001</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$lrange1"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="hrange1">
          <xsl:choose>
            <xsl:when test="DepartureDateTime/@WindowAfter != ''">
              <xsl:variable name="wa">
                <xsl:value-of select="DepartureDateTime/@WindowAfter"/>
              </xsl:variable>
              <xsl:value-of select="format-number($hh + $wa,'00')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$hh"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:value-of select="$mm"/>
        </xsl:variable>
        <xsl:variable name="hrange">
          <xsl:choose>
            <xsl:when test="$hrange1 > '2359'">2359</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$hrange1"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <DepartureWindow>
          <xsl:value-of select="concat($lrange,$hrange)"/>
        </DepartureWindow>
      </xsl:if>
      <OriginLocation>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="OriginLocation/@LocationCode" />
        </xsl:attribute>
      </OriginLocation>
      <DestinationLocation>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="DestinationLocation/@LocationCode" />
        </xsl:attribute>
      </DestinationLocation>
      <!--xsl:if test="ConnectionLocations/ConnectionLocation">
				<ConnectionLocations>
					<xsl:apply-templates select="ConnectionLocations/ConnectionLocation" />
				</ConnectionLocations>
			</xsl:if-->
    </OriginDestinationInformation>
    <!--xsl:if test="DestinationLocation/@LocationCode != following-sibling::OriginDestinationInformation[1]/OriginLocation/@LocationCode">
			<OriginDestinationInformation>
				<xsl:attribute name="RPH">0</xsl:attribute>
				<OriginLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="DestinationLocation/@LocationCode" />
					</xsl:attribute>
				</OriginLocation>
				<DestinationLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="following-sibling::OriginDestinationInformation[1]/OriginLocation/@LocationCode" />
					</xsl:attribute>
				</DestinationLocation>
				<TPA_Extensions>
					<SegmentType Code="ARUNK" /> 
				</TPA_Extensions>
			</OriginDestinationInformation>
		</xsl:if-->
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="ConnectionLocation">
    <ConnectionLocation>
      <xsl:attribute name="LocationCode">
        <xsl:value-of select="@LocationCode" />
      </xsl:attribute>
    </ConnectionLocation>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="VendorPref" mode="include">
    <xsl:param name="pos"/>
    <xsl:if test="@PreferLevel !='Unacceptable' or not(@PreferLevel)">
      <VendorPref PreferLevel="Preferred">
        <xsl:attribute name="Code">
          <xsl:value-of select="@Code" />
        </xsl:attribute>
      </VendorPref>
    </xsl:if>
  </xsl:template>
  <xsl:template match="VendorPref" mode="exclude">
    <xsl:if test="@PreferLevel ='Unacceptable'">
      <ExcludeVendorPref>
        <xsl:attribute name="Code">
          <xsl:value-of select="@Code" />
        </xsl:attribute>
      </ExcludeVendorPref>
    </xsl:if>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="FlightTypePref">
    <xsl:attribute name="MaxStopsQuantity">
      <xsl:choose>
        <xsl:when test="@FlightType='Nonstop'">0</xsl:when>
        <xsl:when test="@FlightType='Direct'">0</xsl:when>
        <xsl:when test="@FlightType='Connection'">3</xsl:when>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="CabinPref">
    <xsl:param name="pos"/>
    <CabinPref>
      <xsl:attribute name="Code">
        <xsl:choose>
          <xsl:when test="@Cabin = 'First'">F</xsl:when>
          <xsl:when test="@Cabin = 'Business'">C</xsl:when>
          <xsl:otherwise>Y</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$pos"/>
      </xsl:attribute>
    </CabinPref>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="PassengerTypeQuantity">
    <xsl:if test="@Quantity!='0'">
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="@Code = 'CHD'">C09</xsl:when>
            <xsl:when test="@Code = 'SCR'">SRC</xsl:when>
            <xsl:when test="@Code = 'JCB'">ADT</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@Code" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="@Quantity" />
        </xsl:attribute>
      </PassengerTypeQuantity>
    </xsl:if>
  </xsl:template>
  <!--************************************************************************************************************	-->
</xsl:stylesheet>
