<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
	================================================================== 
	Sabre_LowFareRQ.xsl 												
	================================================================== 
	Date: 07 Jun 2022 - Kobelev - Updated version of the message as welll fix formating.
	Date: 06 May 2014 - Rastko - added mapping for no penalty fares	
	Date: 30 Dec 2011 - Rastko - added negotiated code mapping			
	Date: 18 Feb 2011 - Rastko - added code to get operating airline in response		
	Date: 16 Sep 2010 - Rastko - removed mapping of connections as not supported by Sabre
	Date: 9 Oct 2008 - Rastko															
	================================================================== 
	-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ" />
	</xsl:template>
	<!--************************************************************************************************************ -->
	<xsl:template match="OTA_AirLowFareSearchRQ">
		<OTA_AirLowFareSearchRQ xmlns="http://www.opentravel.org/OTA/2003/05" AvailableFlightsOnly="true" Version="6.5.0" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
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
			<TravelPreferences>
				<xsl:apply-templates select="TravelPreferences/FlightTypePref" />
				<xsl:for-each select="OriginDestinationInformation">
					<xsl:variable name="pos">
						<xsl:value-of select="position()"/>
					</xsl:variable>
					<xsl:apply-templates select="../TravelPreferences/VendorPref" mode="include">
						<xsl:with-param name="pos">
							<xsl:value-of select="$pos"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</xsl:for-each>
				<xsl:choose>
					<xsl:when test="TravelPreferences/CabinPref/@Cabin !='' ">
						<xsl:for-each select="OriginDestinationInformation">
							<xsl:variable name="pos">
								<xsl:value-of select="position()"/>
							</xsl:variable>
							<xsl:apply-templates select="../TravelPreferences/CabinPref">
								<xsl:with-param name="pos">
									<xsl:value-of select="$pos"/>
								</xsl:with-param>
							</xsl:apply-templates>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:for-each select="OriginDestinationInformation">
							<CabinPref>
								<xsl:attribute name="Code">Y</xsl:attribute>
								<xsl:attribute name="RPH">
									<xsl:value-of select="position()"/>
								</xsl:attribute>
							</CabinPref>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
				<TPA_Extensions>
					<xsl:apply-templates select="TravelPreferences/VendorPref" mode="exclude"/>
					<NumTrips Number="19" />
					<!--<ReturnMaxData Ind="true" />-->
				</TPA_Extensions>
			</TravelPreferences>
			<TravelerInfoSummary>
				<SeatsRequested>
					<xsl:value-of select="sum(TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity/@Quantity)"/>
				</SeatsRequested>
				<AirTravelerAvail>
					<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity" />
				</AirTravelerAvail>
				<PriceRequestInformation>
					<xsl:if test="POS/Source/@ISOCurrency!=''">
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="POS/Source/@ISOCurrency"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
						<NegotiatedFareCode>
							<xsl:attribute name="Code">
								<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
							</xsl:attribute>
						</NegotiatedFareCode>
					</xsl:if>
					<TPA_Extensions>
						<!--
						<FareCalc Ind="true">
							<FareBasis SegmentsOnly="true" WithFareCalc="false" />
						</FareCalc>
						-->
						<xsl:choose>
							<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
								<PublicFare Ind="false" />
								<PrivateFare Ind="true" />
							</xsl:when>
							<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Published'">
								<PublicFare Ind="true" />
								<PrivateFare Ind="false" />
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
					</TPA_Extensions>
				</PriceRequestInformation>
			</TravelerInfoSummary>
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
				<xsl:variable name="lrange">
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
						<xsl:when test="$hrange1 = '2400'">2359</xsl:when>
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
		<xsl:if test="DestinationLocation/@LocationCode != following-sibling::OriginDestinationInformation[1]/OriginLocation/@LocationCode">
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
		</xsl:if>
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
			<VendorPref>
				<xsl:attribute name="Code">
					<xsl:value-of select="@Code" />
				</xsl:attribute>
				<xsl:attribute name="RPH">
					<xsl:value-of select="$pos"/>
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
			<xsl:attribute name="Cabin">
				<xsl:choose>
					<xsl:when test="@Cabin = 'First'">F</xsl:when>
					<xsl:when test="@Cabin = 'Business'">C</xsl:when>
					<xsl:otherwise>Y</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<!--
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos"/>
			</xsl:attribute>
			-->
			<xsl:attribute name="PreferLevel">
				<xsl:text>Preferred</xsl:text>
			</xsl:attribute>
		</CabinPref>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="PassengerTypeQuantity">
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="@Code = 'CHD'">C09</xsl:when>
					<xsl:when test="@Code = 'SCR'">SRC</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@Code" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity">
				<xsl:value-of select="@Quantity" />
			</xsl:attribute>
		</PassengerTypeQuantity>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
