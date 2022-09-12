<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ======================================================================= -->
	<!-- Galileo_AirSeatMapRS.xsl            			            	    						     -->
	<!-- ======================================================================= -->
	<!-- 09 Jun 2009 	- Rastko															     -->
	<!-- ======================================================================= -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="TypeDisp"><xsl:value-of select="SeatMap_5_0/SeatMap/SeatMapQual/TypeDisp"/></xsl:variable>
	<xsl:template match="/">
		<OTA_AirSeatMapRS Version="1.000">
			<xsl:choose>
				<xsl:when test="SeatMap_5_0/SeatMap/Status='0'">
					<Success />
					<SeatMapResponses>
						<xsl:apply-templates select="SeatMap_5_0/SeatMap" />
					</SeatMapResponses>
				</xsl:when>
				<xsl:when test="SeatMap_5_0/SeatMap/ErrMsg">
					<xsl:apply-templates select="SeatMap_5_0/SeatMap/ErrMsg" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="SeatMap_5_0/SeatMap/ErrorCode" />
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirSeatMapRS>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<!--***            			         Process Seat Map Information          	                              ***-->
	<!--***********************************************************************************************************-->
	<xsl:template match="SeatMap">
		<SeatMapResponse>
			<xsl:apply-templates select="FlightQual" />
			<xsl:apply-templates select="SeatMapQual" />
		</SeatMapResponse>
	</xsl:template>
	<xsl:template match="FlightQual ">
		<FlightSegmentInfo>
			<xsl:attribute name="DepartureDateTime">
				<xsl:value-of select="substring(StartDt,1,4)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(StartDt,5,2)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(StartDt,7,2)" />
				<xsl:text>T</xsl:text>
				<xsl:text>00:00:00</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="FltNbr" />
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="StartCity" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="EndCity" />
				</xsl:attribute>
			</ArrivalAirport>
			<OperatingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="AirV" />
				</xsl:attribute>
			</OperatingAirline>
			<xsl:if test="translate(AircraftType, ' ·', '')!=''">
				<Equipment>
					<xsl:attribute name="AirEquipType">
						<xsl:value-of select="translate(AircraftType, ' ·', '')" />
					</xsl:attribute>
				</Equipment>
			</xsl:if>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="AirV" />
				</xsl:attribute>
			</MarketingAirline>
		</FlightSegmentInfo>
	</xsl:template>
	<xsl:template match="SeatMapQual">
		<SeatMapDetails>
			<xsl:apply-templates select="SectionsAry/Sections" />
		</SeatMapDetails>
	</xsl:template>
	<xsl:template match="Sections">
		<CabinClass>
			<xsl:variable name="Cabin" select="../../../FlightQual/BIC" />
			<xsl:attribute name="CabinType">
				<xsl:choose>
					<xsl:when test="$Cabin='F'">
						<xsl:text>First</xsl:text>
					</xsl:when>
					<xsl:when test="$Cabin='C'">
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
						<xsl:value-of select="translate(ColLabel,'=',' ')" />
					</xsl:attribute>
				</SeatColHeadings>
			</TPA_Extensions>
			<xsl:variable name="seats"><xsl:value-of select="translate(ColLabel,'=','')" /></xsl:variable>
			<AirRows>
				<xsl:apply-templates select="RowAry/Row">
					<xsl:with-param name="seats"><xsl:value-of select="$seats"/></xsl:with-param>
				</xsl:apply-templates>
			</AirRows>
		</CabinClass>
	</xsl:template>
	<xsl:template name="createSeats">
		<xsl:param name="seats"/>
		<xsl:if test="$seats != ''">
			<xsl:variable name="seat"><xsl:value-of select="substring($seats,1,1)"/></xsl:variable>
			<xsl:choose>
				<xsl:when test="SeatsAry/Seats[ColID=$seat]">
					<xsl:apply-templates select="SeatsAry/Seats[ColID=$seat]" />
				</xsl:when>
				<xsl:otherwise>
					<AirSeat>
						<xsl:attribute name="SeatNumber">
							<xsl:value-of select="$seat" />
						</xsl:attribute>
						<xsl:attribute name="SeatAvailability">
							<xsl:choose>
								<xsl:when test="$TypeDisp='N'">NoSeat</xsl:when>
								<xsl:when test="$TypeDisp='O'">Occupied</xsl:when>
								<xsl:when test="$TypeDisp='A'">Available</xsl:when>
								<xsl:when test="$TypeDisp='B'">Blocked</xsl:when>
								<xsl:when test="$TypeDisp='C'">Conditional</xsl:when>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="SeatCharacteristics">None</xsl:attribute>
					</AirSeat>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="createSeats">
				<xsl:with-param name="seats"><xsl:value-of select="substring($seats,2)"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Row">
		<xsl:param name="seats"/>
		<xsl:variable name="RownNumber" select="Num" />
		<xsl:if test="$RownNumber!='0'">
			<AirRow>
				<xsl:attribute name="RowNumber">
					<xsl:value-of select="Num" />
				</xsl:attribute>
				<AirSeats>
					<xsl:call-template name="createSeats">
						<xsl:with-param name="seats"><xsl:value-of select="$seats"/></xsl:with-param>
					</xsl:call-template>
					<!--xsl:choose>
						<xsl:when test="SeatsAry/Seats">
							<xsl:apply-templates select="SeatsAry/Seats" />
						</xsl:when>
						<xsl:otherwise>
							<AirSeat SeatNumber="N/A" SeatCharacteristics="N/A" />
						</xsl:otherwise>
					</xsl:choose-->
				</AirSeats>
				<AirRowCharacteristics>
					<xsl:attribute name="CharacteristicList">
						<xsl:choose>
							<xsl:when test="AttribAry/Attrib">
								<xsl:apply-templates select="AttribAry/Attrib" mode="row"/>
							</xsl:when>
							<xsl:otherwise>None</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</AirRowCharacteristics>
			</AirRow>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Seats">
		<AirSeat>
			<xsl:attribute name="SeatNumber">
				<xsl:value-of select="ColID" />
			</xsl:attribute>
			<xsl:attribute name="SeatAvailability">
				<xsl:choose>
					<xsl:when test="Status='N'">NoSeat</xsl:when>
					<xsl:when test="Status='O'">Occupied</xsl:when>
					<xsl:when test="Status='A'">Available</xsl:when>
					<xsl:when test="Status='B'">Blocked</xsl:when>
					<xsl:when test="Status='C'">Conditional</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="SeatCharacteristics">
				<xsl:choose>
					<xsl:when test="AttribAry/Attrib">
						<xsl:apply-templates select="AttribAry/Attrib" mode="seat"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>U</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</AirSeat>
	</xsl:template>
	<xsl:template match="Attrib" mode="row">
		<xsl:if test="position() > 1">
			<xsl:text><![CDATA[ ]]></xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test=". = 'K'">Overwing</xsl:when>
			<xsl:when test=". = 'EL'">ExitLeft</xsl:when>
			<xsl:when test=". = 'ER'">ExitRight</xsl:when>
			<xsl:when test=". = 'E'">Exit</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Attrib" mode="seat">
		<xsl:if test="position() > 1">
			<xsl:text><![CDATA[ ]]></xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test=". = 'A'">Aisle</xsl:when>
			<xsl:when test=". = 'B'">Bassinet</xsl:when>
			<xsl:when test=". = 'BK'">Blocked</xsl:when>
			<xsl:when test=". = 'N'">NoSmoking</xsl:when>
			<xsl:when test=". = 'W'">Window</xsl:when>
			<xsl:when test=". = 'OW'">Overwing</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="ErrMsg">
		<xsl:variable name="ErrorText">
			<xsl:choose>
				<xsl:when test=".=100">Invalid board point</xsl:when>
				<xsl:when test=".=101">Invalid off point</xsl:when>
				<xsl:when test=".=102">Invalid date</xsl:when>
				<xsl:when test=".=104">Invalid class code</xsl:when>
				<xsl:when test=".=107">Invalid airline code</xsl:when>
				<xsl:when test=".=114">Invalid flight number</xsl:when>
				<xsl:when test=".=122">Seating suspended</xsl:when>
				<xsl:when test=".=197">No seats available</xsl:when>
				<xsl:when test=".=200">No seating this flight</xsl:when>
				<xsl:when test=".=201">No seating this class</xsl:when>
				<xsl:when test=".=281">Generic seating only</xsl:when>
				<xsl:otherwise>Unknown error</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<Errors>
			<Error Type="Galileo">
				<xsl:attribute name="Code">
					<xsl:value-of select="." />
				</xsl:attribute>
				<xsl:value-of select="$ErrorText" />
			</Error>
		</Errors>
	</xsl:template>
	<xsl:template match="ErrorCode">
		<xsl:variable name="ErrorText">
			<xsl:choose>
				<xsl:when test=".='0013'">No seating this flight</xsl:when>
				<xsl:otherwise>Unknown error</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<Errors>
			<Error Type="Galileo">
				<xsl:attribute name="Code">
					<xsl:value-of select="." />
				</xsl:attribute>
				<xsl:value-of select="$ErrorText" />
			</Error>
		</Errors>
	</xsl:template>
</xsl:stylesheet>