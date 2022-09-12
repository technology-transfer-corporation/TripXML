<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_AirAvailRS.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 04 Apr 2005 - Bug TT25, TT26 - Rastko									-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirAvailRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_AirAvailRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<xsl:attribute name="TransactionIdentifier">Sabre</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Traveltalk</xsl:attribute>
						<xsl:attribute name="Code">TT002</xsl:attribute>
						<xsl:text>INVALID INPUT</xsl:text>
					</Error>
				</Errors>
			</OTA_AirAvailRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirAvailRS">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Sabre</xsl:attribute>
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
				<xsl:when test="not(OriginDestinationOptions/OriginDestinationOption) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<OriginDestinationOptions>
						<xsl:apply-templates select="OriginDestinationOptions/OriginDestinationOption" />
					</OriginDestinationOptions>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirAvailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="OriginDestinationOption">
		<OriginDestinationOption>
			<xsl:apply-templates select="FlightSegment" />
		</OriginDestinationOption>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="FlightSegment">
		<FlightSegment>
			<xsl:attribute name="ArrivalDateTime">
				<xsl:value-of select="@ArrivalDateTime" />
			</xsl:attribute>
			<xsl:attribute name="DepartureDateTime">
				<xsl:value-of select="@DepartureDateTime" />
			</xsl:attribute>
			<xsl:attribute name="StopQuantity">
				<xsl:value-of select="@StopQuantity" />
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="@RPH" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="@FlightNumber" />
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="CodeContext">
					<xsl:value-of select="DepartureAirport/@CodeContext" />
				</xsl:attribute>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DepartureAirport/@LocationCode" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="CodeContext">
					<xsl:value-of select="ArrivalAirport/@CodeContext" />
				</xsl:attribute>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ArrivalAirport/@LocationCode" />
				</xsl:attribute>
			</ArrivalAirport>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="Equipment/@AirEquipType" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code" />
				</xsl:attribute>
			</MarketingAirline>
			<xsl:apply-templates select="BookingClassAvail" />
		</FlightSegment>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="BookingClassAvail">
		<BookingClassAvail>
			<!--xsl:attribute name="Availability">
				<xsl:value-of select="@Availability"/>
		</xsl:attribute-->
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="@ResBookDesigCode" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigStatusCode">
				<xsl:value-of select="@Availability" />
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="@RPH" />
			</xsl:attribute>
		</BookingClassAvail>
	</xsl:template>
	<!--*************************************************************-->
</xsl:stylesheet>
