<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirAvailRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 25 May 2011 - Shashin - changed to handle 'nonstop' requests             -->
	<!-- Date: 21 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirAvailRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirAvailRQ">
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
						<actionCode>
							<xsl:choose>
								<xsl:when test="../@ScheduleOnly = 'Y'">48</xsl:when>
								<xsl:otherwise>44</xsl:otherwise>
							</xsl:choose>
						</actionCode>
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
					<xsl:if test="../TravelPreferences/CabinPref/@Cabin != ''">
						<cabinOption>
							 <cabinDesignation>        
							 	<xsl:for-each select="../TravelPreferences/CabinPref">
									<cabinClassOfServiceList>
										<xsl:choose>
											<xsl:when test="@Cabin='Economy'">3</xsl:when>
											<xsl:when test="@Cabin='Business'">2</xsl:when>
											<xsl:when test="@Cabin='First'">1</xsl:when>
										</xsl:choose>
									</cabinClassOfServiceList>
								</xsl:for-each>
							 </cabinDesignation>      
							<orderClassesByCabin>702</orderClassesByCabin> 
						</cabinOption>
					</xsl:if>
					<availabilityOptions>
						<productTypeDetails>
							<typeOfRequest>
								<xsl:choose>
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
						<xsl:if test="../TravelPreferences/FlightTypePref/@FlightType != ''">
							<xsl:if test="../TravelPreferences/FlightTypePref/@FlightType = 'Nonstop'">
								<optionInfo>
									<type>FLO</type>
									<arguments>ON</arguments>
								</optionInfo>
							</xsl:if>
						</xsl:if>
						<!--xsl:if test="../SpecificFlightInfo/BookingClassPref/@ResBookDesigCode != ''">
							<optionInfo>
								<type>DIR</type>
								<arguments>
								</arguments>
							</optionInfo>
						</xsl:if-->
						<!--productAvailability>
							<discountCode />
							<numberOfSeats />
						</productAvailability>
						<typeOfAircraft /-->
					</availabilityOptions>
				</requestSection>
			</Air_MultiAvailability>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
