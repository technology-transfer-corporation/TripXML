<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_LowFarePlusRQ_Air.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 09 Nov 2018 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
		<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ | OTA_AirLowFareSearchScheduleRQ" />
	</xsl:template>
	<!--************************************************************************************************************ -->
	<xsl:template match="OTA_AirLowFareSearchPlusRQ | OTA_AirLowFareSearchScheduleRQ">
		<OTA_AirLowFareSearchRQ xmlns="http://www.opentravel.org/OTA/2003/05" Version="4.3.0">
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
							<xsl:otherwise><xsl:value-of select="POS/Source/@PseudoCityCode" /></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<RequestorID ID="1" Type="1">
						<CompanyName Code="SSW"></CompanyName>
					</RequestorID>
				</Source>
			</POS>
			<xsl:apply-templates select="OriginDestinationInformation"/>
			<TravelPreferences>
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
			</TravelPreferences>
			<TravelerInfoSummary SpecificPTC_Indicator="false">
				<SeatsRequested>
					<xsl:value-of select="count(TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code!='INF'])"/>
				</SeatsRequested>
				<AirTravelerAvail>
					<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Quantity!='0']">
						<PassengerTypeQuantity>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="@Code = 'CHD'">C09</xsl:when>
									<xsl:when test="@Code = 'SCR'">SRC</xsl:when>
									<xsl:when test="../../PriceRequestInformation/@PricingSource='Private' and not(../..//PriceRequestInformation/NegotiatedFareCode/@Code)">JCB</xsl:when>
									<xsl:when test="@Code = 'JCB'">ADT</xsl:when>
									<xsl:otherwise><xsl:value-of select="@Code" /></xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="Quantity">
								<xsl:value-of select="@Quantity" />
							</xsl:attribute>
						</PassengerTypeQuantity>
					</xsl:for-each>
				</AirTravelerAvail>
				<PriceRequestInformation>
					<xsl:if test="POS/Source/@ISOCurrency!=''">
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="POS/Source/@ISOCurrency"/></xsl:attribute>
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
					</TPA_Extensions>
				</PriceRequestInformation>
			</TravelerInfoSummary>
			<TPA_Extensions>
				<IntelliSellTransaction>
					<RequestType>
						<xsl:attribute name="Name">ALTDATES</xsl:attribute>
					</RequestType>
					<ServiceTag Name="XXAdVShop"/>
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
			<xsl:attribute name="RPH"><xsl:value-of select="$pos"/></xsl:attribute>
			<DepartureDateTime>
				<xsl:value-of select="DepartureDateTime" />
			</DepartureDateTime>
			<xsl:if test="DepartureDateTime/@WindowBefore != '' or DepartureDateTime/@WindowAfter != ''">
				<xsl:variable name="hh"><xsl:value-of select="substring(DepartureDateTime,12,2)"/></xsl:variable>
				<xsl:variable name="mm"><xsl:value-of select="substring(DepartureDateTime,15,2)"/></xsl:variable>
				<xsl:variable name="lrange1">
					<xsl:choose>
						<xsl:when test="DepartureDateTime/@WindowBefore != ''">
							<xsl:variable name="wb"><xsl:value-of select="DepartureDateTime/@WindowBefore"/></xsl:variable>
							<xsl:value-of select="format-number($hh - $wb,'00')"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="$hh"/></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="$mm"/>
				</xsl:variable>
				<xsl:variable name="lrange">
					<xsl:choose>
						<xsl:when test="$lrange1 &lt; '1'">0001</xsl:when>
						<xsl:otherwise><xsl:value-of select="$lrange1"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="hrange1">
					<xsl:choose>
						<xsl:when test="DepartureDateTime/@WindowAfter != ''">
							<xsl:variable name="wa"><xsl:value-of select="DepartureDateTime/@WindowAfter"/></xsl:variable>
							<xsl:value-of select="format-number($hh + $wa,'00')"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="$hh"/></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="$mm"/>
				</xsl:variable>
				<xsl:variable name="hrange">
					<xsl:choose>
						<xsl:when test="$hrange1 > '2359'">2359</xsl:when>
						<xsl:otherwise><xsl:value-of select="$hrange1"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<DepartureWindow><xsl:value-of select="concat($lrange,$hrange)"/></DepartureWindow> 
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
			<xsl:attribute name="RPH"><xsl:value-of select="$pos"/></xsl:attribute>
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
						<xsl:otherwise><xsl:value-of select="@Code" /></xsl:otherwise>
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
