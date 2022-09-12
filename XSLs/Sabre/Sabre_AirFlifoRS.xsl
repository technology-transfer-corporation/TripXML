<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Sabre_AirFlifoRS.xsl 																-->
<!-- ================================================================== -->
<!-- Date: 16 Feb 2014 - Rastko - remapped the message to new Sabre structure		-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirFlifoRS" />
	</xsl:template>
	<!--**********************************************************************************************-->
	<xsl:template match="OTA_AirFlifoRS ">
		<OTA_AirFlifoRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="ApplicationResults/Error">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<FlightInfoDetails>
						<xsl:apply-templates select="FlightInfo/ScheduledInfo/FlightLeg[DepartureDateTime/@Scheduled != '']" />
					</FlightInfoDetails>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFlifoRS>
	</xsl:template>
	<!--**********************************************************************************************-->
	<xsl:template match="FlightLeg">
		<xsl:variable name="dte" select="substring(../../../ApplicationResults/Success/@timeStamp,1,5)"/>
		<FlightLegInfo>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="../../@FlightNumber" />
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="@LocationCode" />
				</xsl:attribute>
				<xsl:if test="DepartureDateTime/@Gate!=''">
					<xsl:attribute name="Gate">
						<xsl:value-of select="DepartureDateTime/@Gate" />
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="DepartureAirport/Comment != ''">
					<Comment>
						<xsl:value-of select="DepartureAirport/Comment" />
					</Comment>
				</xsl:if>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="following-sibling::FlightLeg[1]/@LocationCode" />
				</xsl:attribute>
				<xsl:if test="following-sibling::FlightLeg[1]/ArrivalDateTime/@Gate!=''">
					<xsl:attribute name="Gate">
						<xsl:value-of select="following-sibling::FlightLeg[1]/ArrivalDateTime/@Gate" />
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="following-sibling::FlightLeg[1]/ArrivalDateTime/@BaggageClaim!=''">
					<xsl:attribute name="BaggageClaim">
						<xsl:value-of select="following-sibling::FlightLeg[1]/ArrivalDateTime/@BaggageClaim" />
					</xsl:attribute>
				</xsl:if>
				<xsl:attribute name="Diversion">0</xsl:attribute>
				<xsl:if test="ArrivalAirport/Comment != ''">
					<Comment>
						<xsl:value-of select="ArrivalAirport/Comment" />
					</Comment>
				</xsl:if>
			</ArrivalAirport>
			<xsl:variable name="dep" select="@LocationCode"/>
			<xsl:variable name="arr" select="following-sibling::FlightLeg[1]/@LocationCode"/>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="../../@AirlineCode" />
				</xsl:attribute>
			</MarketingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="Equipment/@AirEquipType" />
				</xsl:attribute>
			</Equipment>
			<DepartureDateTime>
				<xsl:attribute name="Scheduled">
					<xsl:value-of select="concat($dte,DepartureDateTime/@Scheduled,':00')" />
				</xsl:attribute>
			</DepartureDateTime>
			<ArrivalDateTime>
				<xsl:attribute name="Scheduled">
					<xsl:value-of select="concat($dte,following-sibling::FlightLeg[1]/ArrivalDateTime/@Scheduled,':00')" />
				</xsl:attribute>
			</ArrivalDateTime>
			<xsl:if test="../../ActualInfo/FlightLeg[@LocationCode=$dep][following-sibling::FlightLeg[1][@LocationCode=$arr]]">
				<OperationTimes>
					<xsl:variable name="flt" select="../../ActualInfo/FlightLeg[@LocationCode=$dep][following-sibling::FlightLeg[1][@LocationCode=$arr]]"/>
					<xsl:variable name="fltn" select="$flt/following-sibling::FlightLeg[1][@LocationCode=$arr]"/>
					<xsl:if test="$flt/DepartureDateTime/@Estimated!=''">
						<OperationTime Time="{concat($dte,$flt/DepartureDateTime/@Estimated,':00')}" OperationType="ESTIMATED TIME OF DEPARTURE" TimeType="Estimated"/>
					</xsl:if>
					<xsl:if test="$flt/DepartureDateTime/@OffGate!=''">
						<OperationTime Time="{concat($dte,$flt/DepartureDateTime/@OffGate,':00')}" OperationType="LEFT THE GATE" TimeType="Actual"/>
					</xsl:if>
					<xsl:if test="$flt/DepartureDateTime/@OffRunway!=''">
						<OperationTime Time="{concat($dte,$flt/DepartureDateTime/@OffRunway,':00')}" OperationType="TOOK OFF" TimeType="Actual"/>
					</xsl:if>
					<xsl:if test="$fltn/ArrivalDateTime/@Estimated!=''">
						<OperationTime Time="{concat($dte,$fltn/ArrivalDateTime/@Estimated,':00')}" OperationType="ESTIMATED TIME OF ARRIVAL" TimeType="Estimated"/>
					</xsl:if>
					<xsl:if test="$fltn/ArrivalDateTime/@OnRunway!=''">
						<OperationTime Time="{concat($dte,$fltn/ArrivalDateTime/@OnRunway,':00')}" OperationType="AIRCRAFT LANDED" TimeType="Actual"/>
					</xsl:if>
					<xsl:if test="$fltn/ArrivalDateTime/@OnGate!=''">
						<OperationTime Time="{concat($dte,$fltn/ArrivalDateTime/@OnGate,':00')}" OperationType="ARRIVED" TimeType="Actual"/>
					</xsl:if>
				</OperationTimes>
			</xsl:if>
		</FlightLegInfo>
	</xsl:template>

</xsl:stylesheet>
