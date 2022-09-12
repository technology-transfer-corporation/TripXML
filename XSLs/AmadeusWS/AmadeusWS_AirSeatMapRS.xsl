<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirSeatMapRS.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 14 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_AirSeatMapRS Version="1.000">
			<xsl:choose>
				<xsl:when test="Air_RetrieveSeatMapReply/errorDetails/errorInformation/processingLevel != ''">
					<xsl:apply-templates select="Air_RetrieveSeatMapReply/errorDetails" />
				</xsl:when>
				<xsl:when test="MessagesOnly_Reply">
					<xsl:apply-templates select="MessagesOnly_Reply" />
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<SeatMapResponses>
						<xsl:apply-templates select="Air_RetrieveSeatMapReply" />
					</SeatMapResponses>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirSeatMapRS>
	</xsl:template>
	
	<xsl:template match="MessagesOnly_Reply">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Text = 'Local error'">
					<xsl:attribute name="Type">Traveltalk</xsl:attribute>
					<xsl:attribute name="Code">TT001</xsl:attribute>
					<xsl:text>Invalid provider message structure or message content</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="Text" />
				</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>

	<!--***********************************************************************************************************-->
	<!--***            			         Process Seat Map Information          	                              ***-->
	<!--***********************************************************************************************************-->
	<xsl:template match="Air_RetrieveSeatMapReply">
		<SeatMapResponse>
			<xsl:apply-templates select="segment/flightDateInformation" />
			<xsl:apply-templates select="segment/cabin" />
		</SeatMapResponse>
	</xsl:template>
	<xsl:template match="flightDateInformation">
		<FlightSegmentInfo>
			<xsl:attribute name="DepartureDateTime">
				<xsl:text>20</xsl:text>
				<xsl:value-of select="substring(productDetails/departureDate,5,2)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(productDetails/departureDate,3,2)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(productDetails/departureDate,1,2)" />
				<xsl:text>T</xsl:text>
				<xsl:text>00:00:00</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="flightIdentification/flightNumber" />
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="boardpointDetail/departureCityCode" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="offPointDetail/arrivalCityCode" />
				</xsl:attribute>
			</ArrivalAirport>
			<xsl:if test="companyIdentification/operatingAirlineCode != ''">
				<OperatingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="companyIdentification/operatingAirlineCode" />
					</xsl:attribute>
				</OperatingAirline>
			</xsl:if>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:choose>
						<xsl:when test="../aircraftEquipementDetails/additionalEquipmentInfo/aircraftVersion != ''">
							<xsl:value-of select="../aircraftEquipementDetails/additionalEquipmentInfo/aircraftVersion" />
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="../aircraftEquipementDetails/meansOfTransport"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="companyIdentification/marketingAirlineCode" />
				</xsl:attribute>
			</MarketingAirline>
		</FlightSegmentInfo>
	</xsl:template>
	
	<xsl:template match="cabin">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<SeatMapDetails>
			<xsl:apply-templates select="cabinDetails">
				<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			</xsl:apply-templates>
		</SeatMapDetails>
	</xsl:template>
	
	<xsl:template match="cabinDetails">
		<xsl:param name="pos"/>
		<CabinClass>
			<xsl:variable name="Cabin" select="cabinClassDesignation/cabinClassDesignator" />
			<xsl:attribute name="CabinType">
				<xsl:choose>
					<xsl:when test="$Cabin='F'">
						<xsl:text>First</xsl:text>
					</xsl:when>
					<xsl:when test="$Cabin='C'">
						<xsl:text>Business</xsl:text>
					</xsl:when>
					<xsl:when test="$Cabin='J'">
						<xsl:text>Business</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Economy</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<TPA_Extensions>
				<SeatColHeadings>
					<xsl:attribute name="ColumnHeadings">
						<xsl:apply-templates select="cabinColumnDetails" mode="rows"/>
					</xsl:attribute>
				</SeatColHeadings>
			</TPA_Extensions>
			<AirRows>
				<xsl:variable name="default"><xsl:value-of select="seatOccupationDefault"/></xsl:variable>
				<xsl:variable name="startwing"><xsl:value-of select="overwingRowRange/seatRowNumber[1]"/></xsl:variable>
				<xsl:variable name="endwing"><xsl:value-of select="overwingRowRange/seatRowNumber[2]"/></xsl:variable>
				<xsl:variable name="startrow"><xsl:value-of select="cabinRangeOfRowsDetail/seatRowNumber[1]"/></xsl:variable>
				<xsl:variable name="endrow"><xsl:value-of select="cabinRangeOfRowsDetail/seatRowNumber[2]"/></xsl:variable>
				<xsl:apply-templates select="../../row/rowDetails">
					<xsl:with-param name="defocc"><xsl:value-of select="$default"/></xsl:with-param>
					<xsl:with-param name="startwing"><xsl:value-of select="$startwing - 1"/></xsl:with-param>
					<xsl:with-param name="endwing"><xsl:value-of select="$endwing + 1"/></xsl:with-param>
					<xsl:with-param name="startrow"><xsl:value-of select="$startrow - 1"/></xsl:with-param>
					<xsl:with-param name="endrow"><xsl:value-of select="$endrow + 1"/></xsl:with-param>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
				</xsl:apply-templates>
			</AirRows>
		</CabinClass>
	</xsl:template>
	
	<xsl:template match="cabinColumnDetails" mode="rows">
		<xsl:value-of select="seatColumn" />
		<xsl:if test="columnCharacteristic='A' and following-sibling::cabinColumnDetails[1]/columnCharacteristic = 'A'">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="rowDetails">
		<xsl:param name="defocc"/>
		<xsl:param name="startwing"/>
		<xsl:param name="endwing"/>
		<xsl:param name="startrow"/>
		<xsl:param name="endrow"/>
		<xsl:param name="pos"/>
		<xsl:variable name="rownum"><xsl:value-of select="seatRowNumber"/></xsl:variable>
		<xsl:if test="$rownum > $startrow and $rownum &lt; $endrow">
			<xsl:if test="not(rowCharacteristicsDetails/rowCharacteristic) or (rowCharacteristicsDetails/rowCharacteristic and rowCharacteristicsDetails/rowCharacteristic != 'Z')">
				<AirRow>
					<xsl:attribute name="RowNumber"><xsl:value-of select="$rownum" /></xsl:attribute>
					<AirSeats>
						<xsl:apply-templates select="../../cabin[position()=$pos]/cabinDetails/cabinColumnDetails" mode="seats">
							<xsl:with-param name="defocc"><xsl:value-of select="$defocc"/></xsl:with-param>
							<xsl:with-param name="rownum"><xsl:value-of select="$rownum"/></xsl:with-param>
						</xsl:apply-templates>
					</AirSeats>
					<AirRowCharacteristics>
						<xsl:attribute name="CharacteristicList">
							<xsl:choose>
								<xsl:when test="$rownum > $startwing and $rownum &lt; $endwing">Overwing</xsl:when>
								<xsl:otherwise>None</xsl:otherwise>
							</xsl:choose>
							<xsl:if test="rowCharacteristicsDetails/rowCharacteristic = 'E'">
								<xsl:text> Exit</xsl:text>
							</xsl:if>
							<xsl:if test="rowCharacteristicsDetails/rowCharacteristic = 'K'">
								<xsl:text> Bulkhead</xsl:text>
							</xsl:if>
							<xsl:if test="rowCharacteristicsDetails/rowCharacteristic = 'Z'">
								<xsl:text> BufferZone</xsl:text>
							</xsl:if>
						</xsl:attribute>
					</AirRowCharacteristics>
				</AirRow>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="cabinColumnDetails" mode="seats">
		<xsl:param name="defocc"/>
		<xsl:param name="rownum"/>
		<xsl:variable name="seatnum"><xsl:value-of select="seatColumn"/></xsl:variable>
		<AirSeat>
			<xsl:attribute name="SeatNumber">
				<xsl:value-of select="seatColumn" />
			</xsl:attribute>
			<xsl:attribute name="SeatAvailability">
				<xsl:choose>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = '1'">Restricted</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = '8'">NoSeat</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'LA'">NoSeat</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'GN'">Galley</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'O'">Preferential</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatOccupation = 'Z'">Blocked</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatOccupation = 'F'">Available</xsl:when>
					<xsl:when test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatOccupation = 'O'">Occupied</xsl:when>
					<xsl:when test="$defocc = 'F'">Available</xsl:when>
					<xsl:when test="$defocc = 'O'">Occupied</xsl:when>
					<xsl:when test="$defocc = ''">Available</xsl:when>
					<xsl:otherwise><xsl:value-of select="$defocc" /></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="SeatCharacteristics">
				<xsl:choose>
					<xsl:when test="columnCharacteristic = 'A'">Aisle</xsl:when>
					<xsl:when test="columnCharacteristic = 'W'">Window</xsl:when>
					<xsl:otherwise>Middle</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'U'">
					<xsl:text> UM</xsl:text>
				</xsl:if>
				<xsl:if test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'V'">
					<xsl:text> OfferedLast</xsl:text>
				</xsl:if>
				<xsl:if test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'I'">
					<xsl:text> AdultWithInfant</xsl:text>
				</xsl:if>
				<xsl:if test="../../../row/rowDetails[seatRowNumber = $rownum]/seatOccupationDetails[seatColumn = $seatnum]/seatCharacteristic = 'H'">
					<xsl:text> Handicapped</xsl:text>
				</xsl:if>
			</xsl:attribute>
		</AirSeat>
	</xsl:template>
	<xsl:template match="seatCharacteristic">
		<xsl:value-of select="." />
	</xsl:template>
	<xsl:template match="errorDetails">
		<xsl:variable name="ErrorText">
			<xsl:choose>
				<xsl:when test="errorInformation/errorNumber=1">Passenger surname not found</xsl:when>
				<xsl:when test="errorInformation/errorNumber=10">Location of arrival is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=100">Seat map not available, request seat at check-in</xsl:when>
				<xsl:when test="errorInformation/errorNumber=101">Seat map contains conditional seats, it may be sub</xsl:when>
				<xsl:when test="errorInformation/errorNumber=102">Unable to process</xsl:when>
				<xsl:when test="errorInformation/errorNumber=11">Departure/Arrival city pair is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=12">Unique name not found</xsl:when>
				<xsl:when test="errorInformation/errorNumber=13">Invalid seat number</xsl:when>
				<xsl:when test="errorInformation/errorNumber=14">Airline code and/or flight number invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=15">Flight cancelled</xsl:when>
				<xsl:when test="errorInformation/errorNumber=16">Flight check-in held or suspended temporarily</xsl:when>
				<xsl:when test="errorInformation/errorNumber=17">Passenger surname already checked in</xsl:when>
				<xsl:when test="errorInformation/errorNumber=18">Seating conflict - request contradicts the facilit</xsl:when>
				<xsl:when test="errorInformation/errorNumber=185">Use airline name</xsl:when>
				<xsl:when test="errorInformation/errorNumber=186">Use passenger status</xsl:when>
				<xsl:when test="errorInformation/errorNumber=187">Flight changes from smoking to non smoking</xsl:when>
				<xsl:when test="errorInformation/errorNumber=188">Flight changes from non smoking to smoking</xsl:when>
				<xsl:when test="errorInformation/errorNumber=189">Pax has pre-reserved exit seat unable to C/I</xsl:when>
				<xsl:when test="errorInformation/errorNumber=19">Baggage weight is required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=190">Pax cannot be seated together</xsl:when>
				<xsl:when test="errorInformation/errorNumber=191">Generic seat change not supported</xsl:when>
				<xsl:when test="errorInformation/errorNumber=192">Seat change-request in row change not supported</xsl:when>
				<xsl:when test="errorInformation/errorNumber=193">API pax data required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=194">Passenger surname not checked in</xsl:when>
				<xsl:when test="errorInformation/errorNumber=195">Change of equipment on this flight</xsl:when>
				<xsl:when test="errorInformation/errorNumber=196">Time out occured on host 3</xsl:when>
				<xsl:when test="errorInformation/errorNumber=197">Error in frequent flyer number</xsl:when>
				<xsl:when test="errorInformation/errorNumber=198">Class code required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=199">Check-in separately</xsl:when>
				<xsl:when test="errorInformation/errorNumber=2">Seat not available on the requested class/zone</xsl:when>
				<xsl:when test="errorInformation/errorNumber=20">Bag count conflict - weight update for non-existin</xsl:when>
				<xsl:when test="errorInformation/errorNumber=200">FQTV number not accepted</xsl:when>
				<xsl:when test="errorInformation/errorNumber=201">FQTV number already present</xsl:when>
				<xsl:when test="errorInformation/errorNumber=202">Baggage details not updated</xsl:when>
				<xsl:when test="errorInformation/errorNumber=203">SSR details not updated</xsl:when>
				<xsl:when test="errorInformation/errorNumber=204">Row invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=205">Short connection baggage</xsl:when>
				<xsl:when test="errorInformation/errorNumber=206">Seat change only supported for single passenger</xsl:when>
				<xsl:when test="errorInformation/errorNumber=207">Use generic seating only</xsl:when>
				<xsl:when test="errorInformation/errorNumber=208">Update separately</xsl:when>
				<xsl:when test="errorInformation/errorNumber=209">Flight changes from seating to openseating (frees</xsl:when>
				<xsl:when test="errorInformation/errorNumber=21">Seats not available for passenger type</xsl:when>
				<xsl:when test="errorInformation/errorNumber=210">Flight changes from openseating (freeseating) to</xsl:when>
				<xsl:when test="errorInformation/errorNumber=211">Unable to through-check - complexing/COG/codeshar</xsl:when>
				<xsl:when test="errorInformation/errorNumber=212">API pax data not supported</xsl:when>
				<xsl:when test="errorInformation/errorNumber=213">Time invalid - max/min connecting time for though</xsl:when>
				<xsl:when test="errorInformation/errorNumber=214">API date of birth required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=215">API passport number required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=217">API pax first name required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=218">API pax gender required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=22">Too many connections - need manual tags</xsl:when>
				<xsl:when test="errorInformation/errorNumber=223">API infant data required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=224">Passenger holds advance boarding pass</xsl:when>
				<xsl:when test="errorInformation/errorNumber=23">Invalid bag destination - need manual tags</xsl:when>
				<xsl:when test="errorInformation/errorNumber=24">Passenger actual weight required for this flight</xsl:when>
				<xsl:when test="errorInformation/errorNumber=25">Hand baggage details required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=26">No seat selection on this flight</xsl:when>
				<xsl:when test="errorInformation/errorNumber=27">Location of departure is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=28">Flight rescheduled - through check-in no longer al</xsl:when>
				<xsl:when test="errorInformation/errorNumber=29">Flight full in the requested class</xsl:when>
				<xsl:when test="errorInformation/errorNumber=3">Invalid seat request</xsl:when>
				<xsl:when test="errorInformation/errorNumber=30">Passenger surname off-loaded</xsl:when>
				<xsl:when test="errorInformation/errorNumber=31">Passenger surname deleted/cancelled from the fligh</xsl:when>
				<xsl:when test="errorInformation/errorNumber=32">Bag tag number invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=33">Flight gated - through check-in is not allowed</xsl:when>
				<xsl:when test="errorInformation/errorNumber=34">Time invalid - minimum connecting time for check-i</xsl:when>
				<xsl:when test="errorInformation/errorNumber=35">Flight closed</xsl:when>
				<xsl:when test="errorInformation/errorNumber=36">Passenger not accessible in the system (error/prot</xsl:when>
				<xsl:when test="errorInformation/errorNumber=37">Unique reference for passenger is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=38">Passenger party reference is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=39">Booking/Ticketing class conflict</xsl:when>
				<xsl:when test="errorInformation/errorNumber=4">Bag tag number details required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=40">Status conflict - status does not exist</xsl:when>
				<xsl:when test="errorInformation/errorNumber=41">Frequent flyer number is invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=42">Booking/Ticketing class invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=43">Passenger type conflicts with seats held</xsl:when>
				<xsl:when test="errorInformation/errorNumber=44">Too many passengers</xsl:when>
				<xsl:when test="errorInformation/errorNumber=45">Unable - group names</xsl:when>
				<xsl:when test="errorInformation/errorNumber=46">Unable to check-in partial party</xsl:when>
				<xsl:when test="errorInformation/errorNumber=47">Passenger status conflict</xsl:when>
				<xsl:when test="errorInformation/errorNumber=48">PNR locator unknown in the receiving system</xsl:when>
				<xsl:when test="errorInformation/errorNumber=49">Ticket number invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=5">Invalid flight/Date</xsl:when>
				<xsl:when test="errorInformation/errorNumber=50">Pool airline invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=51">Operating airline invalid</xsl:when>
				<xsl:when test="errorInformation/errorNumber=52">Not authorized - company level</xsl:when>
				<xsl:when test="errorInformation/errorNumber=53">Not authorized - station level</xsl:when>
				<xsl:when test="errorInformation/errorNumber=54">Not authorized - data level</xsl:when>
				<xsl:when test="errorInformation/errorNumber=55">Passenger regraded to a different class (up/down)</xsl:when>
				<xsl:when test="errorInformation/errorNumber=56">Passenger seated elsewhere than requested</xsl:when>
				<xsl:when test="errorInformation/errorNumber=57">Seat not available in the requested class</xsl:when>
				<xsl:when test="errorInformation/errorNumber=58">Seat not available in the requested zone</xsl:when>
				<xsl:when test="errorInformation/errorNumber=59">Specific seat not available</xsl:when>
				<xsl:when test="errorInformation/errorNumber=6">Too many passengers with the same Surname</xsl:when>
				<xsl:when test="errorInformation/errorNumber=60">Free seating in the requested flight</xsl:when>
				<xsl:when test="errorInformation/errorNumber=61">Too many infants</xsl:when>
				<xsl:when test="errorInformation/errorNumber=62">Smoking zone unavailable</xsl:when>
				<xsl:when test="errorInformation/errorNumber=63">Non-smoking zone unavailable</xsl:when>
				<xsl:when test="errorInformation/errorNumber=64">Indifferent zone unavailable</xsl:when>
				<xsl:when test="errorInformation/errorNumber=65">Check visa and/or documentation</xsl:when>
				<xsl:when test="errorInformation/errorNumber=66">No baggage update required for this flight</xsl:when>
				<xsl:when test="errorInformation/errorNumber=67">Gender weight is required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=68">Item conflict</xsl:when>
				<xsl:when test="errorInformation/errorNumber=69">Item weight is required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=7">Passenger type or gender conflict</xsl:when>
				<xsl:when test="errorInformation/errorNumber=70">Modification not possible</xsl:when>
				<xsl:when test="errorInformation/errorNumber=71">No common itinerary</xsl:when>
				<xsl:when test="errorInformation/errorNumber=72">Unable to give seat</xsl:when>
				<xsl:when test="errorInformation/errorNumber=73">Passenger needs initial</xsl:when>
				<xsl:when test="errorInformation/errorNumber=74">Passenger needs first name</xsl:when>
				<xsl:when test="errorInformation/errorNumber=75">Collect second flight name</xsl:when>
				<xsl:when test="errorInformation/errorNumber=76">Check smallpox vaccination</xsl:when>
				<xsl:when test="errorInformation/errorNumber=77">Check yellow fever vaccination</xsl:when>
				<xsl:when test="errorInformation/errorNumber=78">Check cholera vaccination</xsl:when>
				<xsl:when test="errorInformation/errorNumber=79">Passenger has pre-reserved seat</xsl:when>
				<xsl:when test="errorInformation/errorNumber=8">More precise gender is required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=80">Flight initiated - try again</xsl:when>
				<xsl:when test="errorInformation/errorNumber=81">Bag through labeling not allowed beyond this stati</xsl:when>
				<xsl:when test="errorInformation/errorNumber=82">Item/data not found - data not existing in process</xsl:when>
				<xsl:when test="errorInformation/errorNumber=83">Invalid format - data does not match EDIFACT rules</xsl:when>
				<xsl:when test="errorInformation/errorNumber=84">No action - Processing host can not support the fu</xsl:when>
				<xsl:when test="errorInformation/errorNumber=85">Invalid reservations booking modifier</xsl:when>
				<xsl:when test="errorInformation/errorNumber=86">Invalid compartment designator code</xsl:when>
				<xsl:when test="errorInformation/errorNumber=87">Invalid country code</xsl:when>
				<xsl:when test="errorInformation/errorNumber=88">Invalid source of business</xsl:when>
				<xsl:when test="errorInformation/errorNumber=89">Invalid agent's code</xsl:when>
				<xsl:when test="errorInformation/errorNumber=9">Flight is not open for through check-in</xsl:when>
				<xsl:when test="errorInformation/errorNumber=90">Requester identification required</xsl:when>
				<xsl:when test="errorInformation/errorNumber=91">Seat Map Display request is outside system date ra</xsl:when>
				<xsl:when test="errorInformation/errorNumber=92">Flight does not operate due to weather, mechanical</xsl:when>
				<xsl:when test="errorInformation/errorNumber=93">Flight does not operate on date requested</xsl:when>
				<xsl:when test="errorInformation/errorNumber=94">Flight does not operate between requested cities</xsl:when>
				<xsl:when test="errorInformation/errorNumber=95">Schedule change in progress</xsl:when>
				<xsl:when test="errorInformation/errorNumber=96">Repeat request updating in progress</xsl:when>
				<xsl:when test="errorInformation/errorNumber=97">Flight has departed</xsl:when>
				<xsl:when test="errorInformation/errorNumber=98">Seating closed due flight under departure control</xsl:when>
				<xsl:when test="errorInformation/errorNumber=99">Seat map not available for requested zone, seat ma</xsl:when>
				<xsl:otherwise>Unknown error</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<Errors>
			<Error Type="Amadeus">
				<xsl:attribute name="Code">
					<xsl:value-of select="errorInformation/errorNumber" />
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="errorInformation/errorText!=''">
						<xsl:value-of select="errorInformation/errorText"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ErrorText" />
					</xsl:otherwise>
				</xsl:choose>
			</Error>
		</Errors>
	</xsl:template>
</xsl:stylesheet>
