<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_LowFareFlightsRQ.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 11 Oct 2013 - Dharshan - Added TR for the office id RUHS2214J    	-->
  <!-- Date: 11 Oct 2013 - Dharshan - Added Direct and Nonstop critierias	      -->
  <!-- Date: 12 Aug 2011 - Rastko - corrected test for classes and cabins				-->
  <!-- Date: 25 Mar 2011 - Rastko - added specific code for Sita-travel pcc			-->
  <!-- Date: 13 Mar 2011 - Rastko - added specific code for Sita-travel pcc			-->
  <!-- Date: 02 Jan 2010 - Rastko - New file												-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="pcc" select="OTA_AirLowFareSearchFlightsRQ/POS/Source/@PseudoCityCode"/>
  <xsl:template match="/">
    <AirAvail>
      <xsl:apply-templates select="OTA_AirLowFareSearchFlightsRQ" />
    </AirAvail>
  </xsl:template>
  <xsl:template match="OTA_AirLowFareSearchFlightsRQ">
    <xsl:for-each select="OriginDestinationInformation">
      <xsl:variable name="DepDate">
        <xsl:value-of select="substring-before(DepartureDateTime,'T')" />
      </xsl:variable>
      <xsl:variable name="DepDateYear">
        <xsl:value-of select="substring(string($DepDate),3,2)" />
      </xsl:variable>
      <xsl:variable name="DepDateMonth">
        <xsl:value-of select="substring(string($DepDate),6,2)" />
      </xsl:variable>
      <xsl:variable name="DepDateDay">
        <xsl:value-of select="substring(string($DepDate),9,2)" />
      </xsl:variable>
      <xsl:variable name="DepTime">
        <xsl:value-of select="substring-after(DepartureDateTime,'T')" />
      </xsl:variable>
      <xsl:variable name="DepTime2">
        <xsl:value-of select="substring(string($DepTime),1,5)" />
      </xsl:variable>
      <Air_MultiAvailability>
        <messageActionDetails>
          <functionDetails>
            <actionCode>44</actionCode>
          </functionDetails>
        </messageActionDetails>
        <requestSection>
          <availabilityProductInfo>
            <availabilityDetails>
              <departureDate>
                <xsl:value-of select="$DepDateDay" />
                <xsl:value-of select="$DepDateMonth" />
                <xsl:value-of select="$DepDateYear" />
              </departureDate>
              <departureTime>
                <xsl:value-of select="translate(string($DepTime2),':','')" />
              </departureTime>
              <arrivalDate />
              <arrivalTime />
            </availabilityDetails>
            <departureLocationInfo>
              <cityAirport>
                <xsl:value-of select="OriginLocation/@LocationCode" />
              </cityAirport>
              <modifier />
            </departureLocationInfo>
            <arrivalLocationInfo>
              <cityAirport>
                <xsl:value-of select="DestinationLocation/@LocationCode" />
              </cityAirport>
              <modifier />
            </arrivalLocationInfo>
          </availabilityProductInfo>
          <xsl:if test="../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode != ''">
            <optionClass>
              <xsl:for-each select="../SpecificFlightInfo/BookingClassPref">
                <productClassDetails>
                  <serviceClass>
                    <xsl:value-of select="@ResBookDesigCode" />
                  </serviceClass>
                  <nightModifierOption />
                </productClassDetails>
              </xsl:for-each>
            </optionClass>
          </xsl:if>
          <xsl:if test="ConnectionLocations/ConnectionLocation">
            <connectionOption>
              <xsl:for-each select="ConnectionLocations/ConnectionLocation">
                <xsl:variable name="pos">
                  <xsl:value-of select="position()" />
                </xsl:variable>
                <xsl:if test="$pos='1'">
                  <firstConnection>
                    <location>
                      <xsl:value-of select="@LocationCode" />
                    </location>
                    <time />
                    <xsl:if test="@PrefLevel  = 'Unacceptable'">
                      <indicatorList>700</indicatorList>
                    </xsl:if>
                  </firstConnection>
                </xsl:if>
                <xsl:if test="$pos='2'">
                  <secondConnection>
                    <location>
                      <xsl:value-of select="@LocationCode" />
                    </location>
                    <time />
                    <xsl:if test="@PreferLevel  = 'Unacceptable'">
                      <indicatorList>700</indicatorList>
                    </xsl:if>
                  </secondConnection>
                </xsl:if>
              </xsl:for-each>
            </connectionOption>
          </xsl:if>
          <numberOfSeatsInfo>
            <numberOfPassengers>
              <xsl:value-of select="../TravelerInfoSummary/SeatsRequested" />
            </numberOfPassengers>
          </numberOfSeatsInfo>
          <xsl:choose>
            <xsl:when test="../SpecificFlightInfo/Airline">
              <airlineOrFlightOption>
                <flightIdentification>
                  <airlineCode>
                    <xsl:value-of select="../SpecificFlightInfo/Airline/@Code" />
                  </airlineCode>
                  <number>
                    <xsl:value-of select="../SpecificFlightInfo/FlightNumber" />
                  </number>
                  <suffix></suffix>
                </flightIdentification>
              </airlineOrFlightOption>
            </xsl:when>
            <xsl:otherwise>
              <airlineOrFlightOption>
                <xsl:for-each select="../TravelPreferences/VendorPref">
                  <flightIdentification>
                    <airlineCode>
                      <xsl:value-of select="@Code" />
                    </airlineCode>
                    <number />
                    <suffix />
                  </flightIdentification>
                </xsl:for-each>
                <xsl:if test="../TravelPreferences/VendorPref/@PreferLevel='Unacceptable'">
                  <excludeAirlineIndicator>701</excludeAirlineIndicator>
                </xsl:if>
              </airlineOrFlightOption>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="not(../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode)">
            <cabinOption>
              <cabinDesignation>
                <xsl:choose>
                  <xsl:when test="../TravelPreferences/CabinPref/@Cabin!=''">
                    <xsl:for-each select="../TravelPreferences/CabinPref">
                      <cabinClassOfServiceList>
                        <xsl:choose>
                          <xsl:when test="@Cabin='Premium'">4</xsl:when>
                          <xsl:when test="@Cabin='Economy'">3</xsl:when>
                          <xsl:when test="@Cabin='Business'">2</xsl:when>
                          <xsl:when test="@Cabin='First'">1</xsl:when>
                        </xsl:choose>
                      </cabinClassOfServiceList>
                    </xsl:for-each>
                  </xsl:when>
                  <xsl:otherwise>
                    <cabinClassOfServiceList>3</cabinClassOfServiceList>
                  </xsl:otherwise>
                </xsl:choose>
              </cabinDesignation>
              <orderClassesByCabin>702</orderClassesByCabin>
            </cabinOption>
          </xsl:if>
          <availabilityOptions>
            <productTypeDetails>
              <typeOfRequest>
                <xsl:choose>
                  <xsl:when test="$pcc = 'RUHS2214J'">TR</xsl:when>
                  <xsl:when test="$pcc='RUHS22213'">TT</xsl:when>
                  <xsl:when test="Preferences/@Sort = 'DEPARTURE'">TD</xsl:when>
                  <xsl:when test="Preferences/@Sort = 'ARRIVAL'">TA</xsl:when>
                  <xsl:when test="Preferences/@Sort = 'ELAPSED'">TE</xsl:when>
                  <xsl:otherwise>TN</xsl:otherwise>
                </xsl:choose>
              </typeOfRequest>
            </productTypeDetails>
            <xsl:if test="DepartureDateTime/@WindowBefore!='' or DepartureDateTime/@WindowAfter!=''">
              <optionInfo>
                <type>
                  <xsl:if test="DepartureDateTime/@WindowBefore!='' or DepartureDateTime/@WindowAfter!=''">TIW</xsl:if>
                </type>
                <arguments>
                  <xsl:choose>
                    <xsl:when test="DepartureDateTime/@WindowBefore!=''">
                      <xsl:value-of select="DepartureDateTime/@WindowBefore"/>
                    </xsl:when>
                    <xsl:when test="DepartureDateTime/@WindowAfter!=''">
                      <xsl:value-of select="DepartureDateTime/@WindowAfter"/>
                    </xsl:when>
                  </xsl:choose>
                </arguments>
              </optionInfo>
            </xsl:if>
            <xsl:if test="../TravelPreferences/FlightTypePref/@FlightType = 'Nonstop'" >
              <optionInfo>
                <type>FLO</type>
                <xsl:choose>
                  <xsl:when test="../TravelPreferences/FlightTypePref/@FlightType='Nonstop'">
                    <arguments>ON</arguments>
                  </xsl:when>
                  <xsl:otherwise>
                    <arguments>OD</arguments>
                  </xsl:otherwise>
                </xsl:choose>
              </optionInfo>
            </xsl:if>
            <xsl:if test="../TravelPreferences/FlightTypePref/@FlightType = 'Direct'" >
              <optionInfo>
                <type>FLO</type>
                <xsl:choose>
                  <xsl:when test="../TravelPreferences/FlightTypePref/@FlightType='Nonstop' ">
                    <arguments>ON</arguments>
                  </xsl:when>
                  <xsl:otherwise>
                    <arguments>OD</arguments>
                  </xsl:otherwise>
                </xsl:choose>
              </optionInfo>
            </xsl:if>
          </availabilityOptions>
        </requestSection>
      </Air_MultiAvailability>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>