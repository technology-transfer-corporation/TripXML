<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_AirAvailRQ.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 17 Sep 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirAvailRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirAvailRQ">
		<xsl:for-each select="OriginDestinationInformation">
			<AirAvailability_12>
				<AirAvailMods>
					<xsl:choose>
						<xsl:when test="../TPA_Extensions/MoreIndicator != ''">
							<MoreAvail>
								<Tok>
									<xsl:value-of select="../TPA_Extensions/MoreIndicator" />
								</Tok>
							</MoreAvail>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode !='' ">
									<BICFilter>
										<BICFilterAry>
											<xsl:apply-templates select="../SpecificFlightInfo/BookingClassPref" />
										</BICFilterAry>
									</BICFilter>
								</xsl:when>
								<xsl:when test="../TravelPreferences/CabinPref/@Cabin !='' ">
									<BICFilter>
										<BICFilterAry>
											<xsl:apply-templates select="../TravelPreferences/CabinPref" />
										</BICFilterAry>
									</BICFilter>
								</xsl:when>
							</xsl:choose>
							<xsl:apply-templates select="../TravelPreferences" mode="VendorPref" />
							<xsl:choose>
								<xsl:when test="../SpecificFlightInfo/FlightNumber != ''">
									<xsl:apply-templates select="." mode="flight" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="." mode="Itin" />
								</xsl:otherwise>
							</xsl:choose>
							<xsl:apply-templates select="ConnectionLocations/ConnectionLocation" />
						</xsl:otherwise>
					</xsl:choose>
				</AirAvailMods>
			</AirAvailability_12>
		</xsl:for-each>
		<!-- ======================================================================= -->
		<xsl:apply-templates select="MoreFlightsToken" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="MoreFlightsToken">
		<AirAvailability_6_2>
			<AirAvailMods>
				<MoreAvail>
					<Tok>
						<xsl:value-of select="." />
					</Tok>
				</MoreAvail>
			</AirAvailMods>
		</AirAvailability_6_2>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="CabinPref">
		<BICFilterInfo>
			<BIC>
				<xsl:choose>
					<xsl:when test="@Cabin='Economy'">Y</xsl:when>
					<xsl:when test="@Cabin='Business'">C</xsl:when>
					<xsl:when test="@Cabin='First'">F</xsl:when>
				</xsl:choose>
			</BIC>
			<AllowSmlrInd>Y</AllowSmlrInd>
		</BICFilterInfo>
	</xsl:template>
	
	<xsl:template match="BookingClassPref">
		<BICFilterInfo>
			<BIC><xsl:value-of select="@ResBookDesigCode"/></BIC>
			<AllowSmlrInd>N</AllowSmlrInd>
		</BICFilterInfo>
	</xsl:template>

	<!-- ======================================================================= -->
	<xsl:template match="TravelPreferences" mode="VendorPref">
		<xsl:if test="VendorPref/@Code!=''">
			<AirVPrefInd>
				<AirVIncExcInd>
					<xsl:choose>
						<xsl:when test="VendorPref/@PreferLevel = 'Unacceptable'">E</xsl:when>
						<xsl:otherwise>O</xsl:otherwise>
					</xsl:choose>
				</AirVIncExcInd>
				<RelaxAirVPref>N</RelaxAirVPref>
				<SectorNum>0</SectorNum>
			</AirVPrefInd>
			<AirVPrefs>
				<AirVAry>
					<xsl:apply-templates select="VendorPref/@Code" mode="airline" />
				</AirVAry>
			</AirVPrefs>
		</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@Code" mode="airline">
		<AirVInfo>
			<AirV>
				<xsl:value-of select="." />
			</AirV>
		</AirVInfo>
	</xsl:template>
	
	<xsl:template match="OriginDestination" mode="Alliance">
		<xsl:if test="Preferences/AllianceCode">
			<AirVPrefInd>
				<AirVIncExcInd>I</AirVIncExcInd>
				<RelaxAirVPref>N</RelaxAirVPref>
				<SectorNum>0</SectorNum>
			</AirVPrefInd>
			<AirVPrefs>
				<AirVAry>
					<AirVInfo>
						<AirV>/<xsl:value-of select="Preferences/AllianceCode" /></AirV>
					</AirVInfo>
				</AirVAry>
			</AirVPrefs>
		</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ConnectionLocation">
		<xsl:choose>
			<xsl:when test="@PreferLevel = 'Unacceptable'">
				<ConxPrefInd>
					<IncExc>E</IncExc>
				</ConxPrefInd>
			</xsl:when>
			<xsl:otherwise>
				<ConxPrefInd>
					<IncExc>I</IncExc>
				</ConxPrefInd>
			</xsl:otherwise>
		</xsl:choose>
		<ConxPref>
			<PtAry>
				<PtInfo>
					<Pt>
						<xsl:value-of select="@LocationCode" />
					</Pt>
				</PtInfo>
			</PtAry>
		</ConxPref>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OriginDestinationInformation" mode="Itin">
		<xsl:variable name="DepDate">
			<xsl:choose>
				<xsl:when test="contains(DepartureDateTime,'T')">
					<xsl:value-of select="substring-before(DepartureDateTime,'T')" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="DepartureDateTime" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<GenAvail>
			<NumSeats>
				<xsl:choose>
					<xsl:when test="../TravelerInfoSummary/SeatsRequested!=''">
						<xsl:value-of select="../TravelerInfoSummary/SeatsRequested" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity/@Quantity)" />
					</xsl:otherwise>
				</xsl:choose>
			</NumSeats>
			<xsl:choose>
				<xsl:when test="../TravelPreferences/CabinPref/@Cabin !='' and not(../TravelPreferences/CabinPref[position() = 2])">
					<Class>
						<xsl:choose>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Economy'">Y</xsl:when>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
						</xsl:choose>
					</Class>
				</xsl:when>
				<xsl:otherwise>
					<Class>
						<xsl:value-of select="string(' ')" />
					</Class>
				</xsl:otherwise>
			</xsl:choose>
			<StartDt>
				<xsl:value-of select="translate(string($DepDate),'-','')" />
			</StartDt>
			<StartPt>
				<xsl:value-of select="OriginLocation/@LocationCode" />
			</StartPt>
			<EndPt>
				<xsl:value-of select="DestinationLocation/@LocationCode" />
			</EndPt>
			<StartTm>
				<xsl:value-of select="translate(string($DepTime2),':','')" />
			</StartTm>
			<TmWndInd>D</TmWndInd>
			<xsl:choose>
				<xsl:when test="not(DepartureDateTime/@WindowBefore) and not(DepartureDateTime/@WindowAfter)">
					<xsl:call-template name="FakeStartWin">
						<xsl:with-param name="mode">s</xsl:with-param>
					</xsl:call-template>
					<xsl:call-template name="FakeStartWin">
						<xsl:with-param name="mode">e</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="DepartureDateTime/@WindowBefore != ''">
							<xsl:apply-templates select="DepartureDateTime/@WindowBefore">
								<xsl:with-param name="time"><xsl:value-of select="translate(string($DepTime2),':','')"/></xsl:with-param>
							</xsl:apply-templates>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="FakeStartWin">
								<xsl:with-param name="mode">s</xsl:with-param>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:choose>
						<xsl:when test="DepartureDateTime/@WindowAfter != ''">
							<xsl:apply-templates select="DepartureDateTime/@WindowAfter">
								<xsl:with-param name="time"><xsl:value-of select="translate(string($DepTime2),':','')"/></xsl:with-param>
							</xsl:apply-templates>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="FakeStartWin">
								<xsl:with-param name="mode">e</xsl:with-param>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:apply-templates select="../TravelPreferences" mode="MaxStops" />
			<xsl:if test="DepartureDateTime/@WindowBefore or DepartureDateTime/@WindowAfter">
				<SeqInd>P</SeqInd>
			</xsl:if>
		</GenAvail>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OriginDestinationInformation" mode="flight">
		<xsl:variable name="DepDate">
			<xsl:value-of select="substring-before(DepartureDateTime,'T')" />
		</xsl:variable>
		<SpecificAvailMods>
			<NumSeats>
				<xsl:value-of select="../TravelerInfoSummary/SeatsRequested" />
			</NumSeats> 
			<xsl:choose>
				<xsl:when test="../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode !=''">
					<Class>
						<xsl:value-of select="../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode" />
					</Class>
				</xsl:when>
				<xsl:otherwise>
					<Class>
						<xsl:value-of select="string(' ')" />
					</Class>
				</xsl:otherwise>
			</xsl:choose>
		</SpecificAvailMods>
		<SpecificAvailFlts>
			<FltAry>
				<Flt>
					<xsl:variable name="zeros">000</xsl:variable>
					<StartDt>
						<xsl:value-of select="translate(string($DepDate),'-','')" />
					</StartDt>
					<StartAirp>
						<xsl:value-of select="OriginLocation/@LocationCode" />
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="DestinationLocation/@LocationCode" />
					</EndAirp>
					<FltNum>
						<xsl:value-of select="substring(string($zeros),1,4-string-length(../SpecificFlightInfo/FlightNumber))" />
						<xsl:value-of select="../SpecificFlightInfo/FlightNumber" />
					</FltNum>
					<OpSuf>
						<xsl:value-of select="Preferences/FlightSpecific/FlightNumber/@Suffix" />
					</OpSuf> 
					<AirV>
						<xsl:value-of select="../SpecificFlightInfo/Airline/@Code" />
					</AirV>
					<Conx>N</Conx>
				</Flt>
			</FltAry>
		</SpecificAvailFlts>
	</xsl:template>

	<!--**********************************************************************************************-->
	<!-- If no Time Window input create one				  -->
	<!--**********************************************************************************************-->
	<xsl:template name="FakeStartWin">
		<xsl:param name="mode"/>
		<xsl:if test="$mode='s'">
			<StartTmWnd>
				<xsl:value-of select="0000" />
			</StartTmWnd>
		</xsl:if>
		<xsl:if test="$mode='e'">
			<EndTmWnd>
				<xsl:value-of select="2400" />
			</EndTmWnd>
		</xsl:if>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!-- If Time Window input, use input values							  -->
	<!--**********************************************************************************************-->
	<xsl:template match="@WindowBefore">
		<xsl:param name="time"/>
		<StartTmWnd>
			<xsl:variable name="begin" select="$time - (. * 100)" />
			<xsl:call-template name="CheckLeadingZero">
				<xsl:with-param name="in">
					<xsl:choose>
						<xsl:when test="$begin >= 0">
							<xsl:value-of select="$begin" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0000" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</StartTmWnd>
	</xsl:template>
	
	<xsl:template match="@WindowAfter">
		<xsl:param name="time"/>
		<EndTmWnd>
			<xsl:variable name="end" select="$time + (. * 100)" />
			<xsl:call-template name="CheckLeadingZero">
				<xsl:with-param name="in">
					<xsl:choose>
						<xsl:when test="$end &lt;= 2400">
							<xsl:value-of select="$end" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="2400" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</EndTmWnd>
	</xsl:template>
	
	<xsl:template match="TravelPreferences" mode="MaxStops">
		<xsl:variable name="VendorPosition2">
			<xsl:value-of select="VendorPref[position()=2]" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="@MaxStopsQuantity = '3' and VendorPref[position()=2] != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>Y</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="@MaxStopsQuantity = '2' and VendorPref[position()=2] != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="@MaxStopsQuantity = '1' and VendorPref[position()=2] != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="@MaxStopsQuantity = '3' and $VendorPosition2 =  ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="@MaxStopsQuantity = '2' and $VendorPosition2 =  ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="@MaxStopsQuantity = '1' and $VendorPosition2 =  ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="FlightTypePref/@FlightType = 'Direct'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>N</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test="FlightTypePref/@FlightType = 'Nonstop'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>N</IncStopDirectsInd>
				<IncSingleOnlineConxInd>N</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="HoursTotal">
		<JrnyTm>
			<xsl:value-of select="." />
		</JrnyTm>
	</xsl:template>
	<xsl:template name="CheckLeadingZero">
		<xsl:param name="in" />
		<xsl:param name="length">4</xsl:param>
		<xsl:choose>
			<xsl:when test="string-length(string($in)) &lt; $length">
				<xsl:call-template name="CheckLeadingZero">
					<xsl:with-param name="in">
						<xsl:text>0</xsl:text>
						<xsl:value-of select="$in" />
					</xsl:with-param>
					<xsl:with-param name="length" select="$length" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$in" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>