<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Worldspan_AirSeatMapRS.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 08 Mar 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_AirSeatMapRS Version="1.000">
			<xsl:choose>
				<xsl:when test="OTA_AirSeatMapRS/SeatMapResponses/SeatMapResponse/FlightSegmentInfo/Warnings/Warning/@Type = '3'">
					<xsl:apply-templates select="OTA_AirSeatMapRS/SeatMapResponses/SeatMapResponse/FlightSegmentInfo/Warnings/Warning" />
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<SeatMapResponses>
						<xsl:apply-templates select="OTA_AirSeatMapRS/SeatMapResponses/SeatMapResponse" />
					</SeatMapResponses>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirSeatMapRS>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<!--***            			         Process Seat Map Information          	                              ***-->
	<!--***********************************************************************************************************-->
	<xsl:template match="SeatMapResponse">
		<SeatMapResponse>
			<xsl:copy-of select="FlightSegmentInfo"/>
			<SeatMapDetails>
				<xsl:apply-templates select="SeatMapDetails/CabinClass"/>
			</SeatMapDetails>
		</SeatMapResponse>
	</xsl:template>
	
	<xsl:template match="CabinClass">
		<CabinClass>
			<xsl:attribute name="CabinType">
				<xsl:value-of select="@CabinType"/>
			</xsl:attribute>
			<TPA_Extensions>
				<SeatColHeadings>
					<xsl:attribute name="ColumnHeadings">
						<xsl:apply-templates select="AirRows/AirRow[position()=2]/AirSeats/AirSeat" mode="rows"/>
					</xsl:attribute>
				</SeatColHeadings>
			</TPA_Extensions>
			<AirRows>
				<xsl:apply-templates select="AirRows/AirRow"/>
			</AirRows>
		</CabinClass>
	</xsl:template>
	
	<xsl:template match="AirSeat" mode="rows">
		<xsl:value-of select="@SeatNumber" />
		<xsl:if test="substring(@SeatCharacteristics,1,1)='3' and substring(following-sibling::AirSeat[1]/@SeatCharacteristics,1,1)= '3'">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="AirRow">
		<AirRow>
			<xsl:attribute name="RowNumber"><xsl:value-of select="@RowNumber" /></xsl:attribute>
			<AirSeats>
				<xsl:apply-templates select="AirSeats/AirSeat" mode="seats"/>
			</AirSeats>
			<AirRowCharacteristics>
				<xsl:attribute name="CharacteristicList">
					<xsl:call-template name="rowType">
						<xsl:with-param name="row"><xsl:value-of select="AirRowCharacteristics/@CharacteristicList"/></xsl:with-param>
					</xsl:call-template>
				</xsl:attribute>
			</AirRowCharacteristics>
		</AirRow>
	</xsl:template>
	
	<xsl:template match="AirSeat" mode="seats">
		<AirSeat>
			<xsl:attribute name="SeatNumber">
				<xsl:value-of select="@SeatNumber" />
			</xsl:attribute>
			<xsl:attribute name="SeatAvailability">
				<xsl:choose>
					<xsl:when test="@SeatAvailability = '1'">Available</xsl:when>
					<xsl:when test="@SeatAvailability = '2'">Occupied</xsl:when>
					<xsl:when test="@SeatAvailability = '3'">Unavailable</xsl:when>
					<xsl:when test="@SeatAvailability = '16'">NoSeat</xsl:when>
					<xsl:otherwise><xsl:value-of select="@SeatAvailability" /></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="SeatCharacteristics">
				<xsl:call-template name="seatType">
					<xsl:with-param name="seat"><xsl:value-of select="@SeatCharacteristics"/></xsl:with-param>
				</xsl:call-template>
				<xsl:if test="not(contains(@SeatCharacteristics,'47'))">
					<xsl:choose>
						<xsl:when test="position()=1 or position()=last()">Window</xsl:when>
						<xsl:when test="substring(@SeatCharacteristics,1,1) ='3'">Aisle</xsl:when>
						<xsl:when test="substring(@SeatCharacteristics,1,1) !='3'">Middle</xsl:when>
					</xsl:choose>
				</xsl:if>
			</xsl:attribute>
		</AirSeat>
	</xsl:template>
	
	<xsl:template name="rowType">
		<xsl:param name="row"/>
		<xsl:if test="$row != ''">
			<xsl:variable name="rowchar">
				<xsl:choose>
					<xsl:when test="substring($row,1,1)=' '">
						<xsl:choose>
							<xsl:when test="contains(substring($row,2),' ')"><xsl:value-of select="substring-before(substring($row,2),' ')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="substring($row,2)"/></xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="contains($row,' ')"><xsl:value-of select="substring-before($row,' ')"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="$row"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$rowchar='4'">Exit</xsl:when>
				<xsl:when test="$rowchar='11'">No-smoking</xsl:when>
				<xsl:when test="$rowchar='13'">Overwing</xsl:when>
				<xsl:otherwise>None</xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="rowt">
				<xsl:choose>
					<xsl:when test="substring($row,1,1)=' '"><xsl:value-of select="substring($row,2)"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="$row"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:if test="contains($rowt,' ')">
				<xsl:text> </xsl:text>
			</xsl:if>
			<xsl:call-template name="rowType">
				<xsl:with-param name="row"><xsl:value-of select="substring-after($rowt,' ')"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="seatType">
		<xsl:param name="seat"/>
		<xsl:if test="$seat != ''">
			<xsl:variable name="seatchar">
				<xsl:choose>
					<xsl:when test="substring($seat,1,1)=' '">
						<xsl:choose>
							<xsl:when test="contains(substring($seat,2),' ')"><xsl:value-of select="substring-before(substring($seat,2),' ')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="substring($seat,2)"/></xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="contains($seat,' ')"><xsl:value-of select="substring-before($seat,' ')"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="$seat"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$seatchar='3'"></xsl:when>
				<xsl:when test="$seatchar='17'">Exit</xsl:when>
				<xsl:when test="$seatchar='47'">NoSeat</xsl:when>
				<xsl:when test="$seatchar='49'"></xsl:when>
				<xsl:when test="$seatchar='58'">Overwing</xsl:when>
				<xsl:when test="$seatchar='60'">Preferred</xsl:when>
				<xsl:when test="$seatchar='79'">Bassinet</xsl:when>
				<xsl:when test="$seatchar='80'">Handicapped</xsl:when>
				<xsl:otherwise><xsl:value-of select="$seatchar"/></xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="seatt">
				<xsl:choose>
					<xsl:when test="substring($seat,1,1)=' '"><xsl:value-of select="substring($seat,2)"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="$seat"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<!--xsl:if test="contains($seatt,' ')"-->
				<xsl:text> </xsl:text>
			<!--/xsl:if-->
			<xsl:call-template name="seatType">
				<xsl:with-param name="seat"><xsl:value-of select="substring-after($seatt,' ')"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="OTA_AirSeatMapRS/SeatMapResponses/SeatMapResponse/FlightSegmentInfo/Warnings/Warning">
		<Errors>
			<Error Type="Worldspan">
				<xsl:attribute name="Code">
					<xsl:value-of select="@Code" />
				</xsl:attribute>
				<xsl:value-of select="@ShortText" />
			</Error>
		</Errors>
	</xsl:template>
</xsl:stylesheet>
