<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFarePlusRQ.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 29 Mar 2012 - Shashin -POS AirportCode change						 -->	
	<!-- Date: 21 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OTA_AirLowFareSearchPlusRQ">
		<Fare_MasterPricerTravelBoardSearch>
			<numberOfUnit>
				<unitNumberDetail>
					<numberOfUnits>
						<xsl:value-of select="TravelerInfoSummary/SeatsRequested" />
					</numberOfUnits>
					<typeOfUnit>PX</typeOfUnit>
				</unitNumberDetail>
				<unitNumberDetail>
					<numberOfUnits>
						<xsl:choose>
							<xsl:when test="@MaxResponses != ''">
								<xsl:value-of select="@MaxResponses"/>
							</xsl:when>
							<xsl:when test="TravelPreferences/VendorPref or OriginDestinationInformation/ConnectionLocations or TravelPreferences/FlightTypePref">20</xsl:when>
							<xsl:when test="not(TravelPreferences/VendorPref) and not(OriginDestinationInformation/ConnectionLocations) and not(TravelPreferences/FlightTypePref)">20</xsl:when>
							<xsl:otherwise>20</xsl:otherwise>
						</xsl:choose>
					</numberOfUnits>
					<typeOfUnit>RC</typeOfUnit>
				</unitNumberDetail>
				<unitNumberDetail>
					<numberOfUnits>1</numberOfUnits>
					<typeOfUnit>SND</typeOfUnit>
				</unitNumberDetail>
			</numberOfUnit>
			<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Quantity > 0]">
				<paxReference>
					<xsl:call-template name="create_type">
						<xsl:with-param name="count">
							<xsl:value-of select="@Quantity" />
						</xsl:with-param>
						<xsl:with-param name="count1">
							<xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity) + 1" />
						</xsl:with-param>
						<xsl:with-param name="st">1</xsl:with-param>
					</xsl:call-template>
				</paxReference>
			</xsl:for-each>
			<fareOptions>
				<pricingTickInfo>
					<pricingTicketing>
						<priceType>TAC</priceType>
						<priceType>ET</priceType>
						<xsl:choose>
							<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
								<priceType>RU</priceType>
							</xsl:when>
							<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
								<priceType>RP</priceType>
								<priceType>RU</priceType>
							</xsl:when>
							<xsl:otherwise>
								<priceType>RP</priceType>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="POS/Source/@ISOCurrency!=''">
							<priceType>CUC</priceType>
						</xsl:if>
						<xsl:if test="TravelPreferences/FareRestrictPref/AdvResTicketing/@AdvResInd = 'false'">
							<priceType>NAP</priceType>
						</xsl:if>
						<xsl:if test="TravelPreferences/FareRestrictPref/StayRestrictions/@StayRestrictionInd = 'false'">
							<priceType>NR</priceType>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="TravelPreferences/FareRestrictPref/VoluntaryChanges/@VolChangeInd = 'false'">
								<priceType>NPE</priceType>
							</xsl:when>
							<xsl:when test="TravelPreferences/FareRestrictPref/VoluntaryChanges/Penalty/@PenaltyType = 'Ref'">
								<priceType>RF</priceType>
							</xsl:when>
						</xsl:choose>
					</pricingTicketing>
					<xsl:if test="POS/Source/@AirportCode != ''">
						<sellingPoint>
                                               <locationId>
                                               	<xsl:value-of select="POS/Source/@AirportCode "/>
                                               </locationId>
                               		 </sellingPoint>
                                		<ticketingPoint>
                                        	<locationId>
                                        		<xsl:value-of select="POS/Source/@AirportCode "/>
                                        	</locationId>
	                                </ticketingPoint>
					</xsl:if>
					<xsl:if test="TravelPreferences/TicketDistribPref/@TicketTime != ''">
						<ticketingDate>
							<date>
								<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,9,2)"/>
								<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,6,2)"/>
								<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,3,2)"/>
							</date>
						</ticketingDate>
					</xsl:if>
					<!--corporateId>
						<arcNumber></arcNumber>
					</corporateId-->
				</pricingTickInfo>
				<xsl:if test="POS/Source/@ISOCurrency!=''">
					<conversionRate>
						<conversionRateDetail>
							<currency><xsl:value-of select="POS/Source/@ISOCurrency"/></currency>
						</conversionRateDetail>
					</conversionRate>
				</xsl:if>
			</fareOptions>
			<xsl:if test="(TravelPreferences/CabinPref/@Cabin = 'Economy' and TravelPreferences/CabinPref/@PreferLevel = 'Only') or TravelPreferences/CabinPref/@Cabin = 'First' or TravelPreferences/CabinPref/@Cabin = 'Premium'  or TravelPreferences/CabinPref/@Cabin = 'Business' or TravelPreferences/VendorPref/@Code!='' or TravelPreferences/FlightTypePref/@FlightType != ''">
				<travelFlightInfo>
					<xsl:if test="(TravelPreferences/CabinPref/@Cabin = 'Economy' and TravelPreferences/CabinPref/@PreferLevel = 'Only') or TravelPreferences/CabinPref/@Cabin = 'First' or TravelPreferences/CabinPref/@Cabin = 'Business' or TravelPreferences/CabinPref/@Cabin = 'Premium'">
						<cabinId>
							<cabinQualifier>RC</cabinQualifier>
							<cabin>
								<xsl:choose>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'First'">F</xsl:when>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Business'">C</xsl:when>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Premium'">M</xsl:when>

									<xsl:otherwise>Y</xsl:otherwise>
								</xsl:choose>
							</cabin>
						</cabinId>
					</xsl:if>
					<xsl:if test="TravelPreferences/VendorPref/@Code!=''">
						<companyIdentity>
							<carrierQualifier>
								<xsl:choose>
									<xsl:when test="TravelPreferences/VendorPref/@PreferLevel = 'Unacceptable'">X</xsl:when>
									<xsl:otherwise>M</xsl:otherwise>
								</xsl:choose>
							</carrierQualifier>
							<xsl:for-each select="TravelPreferences/VendorPref">
								<carrierId><xsl:value-of select="@Code"/></carrierId>
							</xsl:for-each>
						</companyIdentity>
					</xsl:if>
					<xsl:if test="TravelPreferences/FlightTypePref/@FlightType != ''">
						<flightDetail>
							<xsl:if test="TravelPreferences/FlightTypePref/@FlightType = 'Nonstop'">
								<flightType>N</flightType>
							</xsl:if>
							<xsl:if test="TravelPreferences/FlightTypePref/@FlightType = 'Direct'">
								<flightType>D</flightType>
							</xsl:if>
						</flightDetail>
					</xsl:if>
				</travelFlightInfo>
			</xsl:if>
			<xsl:apply-templates select="OriginDestinationInformation" />
		</Fare_MasterPricerTravelBoardSearch>
	</xsl:template>
	<!---********************************************************************-->
	<!--   Pax type 									  	           -->
	<!---********************************************************************-->
	<xsl:template name="create_type">
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:param name="st" />
		<xsl:if test="$st = 1">
			<ptc>
				<xsl:choose>
					<xsl:when test="@Code='CHD'">CNN</xsl:when>
					<xsl:when test="@Code='SRC'">YCD</xsl:when>
					<xsl:when test="@Code='SCR'">YCD</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@Code" />
					</xsl:otherwise>
				</xsl:choose>
			</ptc>
		</xsl:if>
		<xsl:if test="$count !=0">
			<traveller>
				<xsl:choose>
					<xsl:when test="@Code='INF' or @Code='IN'">
						<ref><xsl:value-of select="$count"/></ref>
						<infantIndicator>1</infantIndicator>
					</xsl:when>
					<xsl:otherwise>
						<ref>
							<xsl:value-of select="$count1" />
						</ref>
					</xsl:otherwise>
				</xsl:choose>
			</traveller>
			<xsl:call-template name="create_type">
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1" />
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1" />
				</xsl:with-param>
				<xsl:with-param name="st">2</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!---********************************************************************-->
	<!--   Itinerary information begins here                                 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationInformation">
		<itinerary>
			<requestedSegmentRef>
				<segRef>
					<xsl:value-of select="position()" />
				</segRef>
			</requestedSegmentRef>
			<departureLocalization>
				<depMultiCity>
					<locationId>
						<xsl:value-of select="OriginLocation/@LocationCode" />
					</locationId>
				</depMultiCity>
			</departureLocalization>
			<arrivalLocalization>
				<arrivalMultiCity>
					<locationId>
						<xsl:value-of select="DestinationLocation/@LocationCode" />
					</locationId>
				</arrivalMultiCity>
			</arrivalLocalization>
			<timeDetails>
				<firstDateTimeDetail>
					<xsl:variable name="start">
						<xsl:value-of select="substring(DepartureDateTime,1,10)" />
					</xsl:variable>
					<xsl:variable name="end">
						<xsl:value-of select="substring(following-sibling::OriginDestinationInformation/DepartureDateTime,1,10)" />
					</xsl:variable>
					<xsl:variable name="end1">
						<xsl:value-of select="substring(preceding-sibling::OriginDestinationInformation/DepartureDateTime,1,10)" />
					</xsl:variable>
					<xsl:variable name="dd" select="ttVB:FctDateDuration(string($start),string($end))"/>
					<xsl:variable name="dd1" select="ttVB:FctDateDuration(string($start),string($end1))"/>
					<xsl:choose>
						<xsl:when test="$dd = 0 or $dd = 1">
							<timeQualifier>TA</timeQualifier>
						</xsl:when>
						<xsl:otherwise>
							<timeQualifier>TD</timeQualifier>
						</xsl:otherwise>
					</xsl:choose>
					<date>
						<xsl:value-of select="substring(string(DepartureDateTime),9,2)" />
						<xsl:value-of select="substring(string(DepartureDateTime),6,2)" />
						<xsl:value-of select="substring(string(DepartureDateTime),3,2)" />
					</date>
					<xsl:choose>
						<xsl:when test="$dd = 0 or $dd = 1 or $dd1 = 0 or $dd1 = -1">
							<xsl:if test="$dd = 0 or $dd = 1">
								<time>
									<xsl:choose>
										<xsl:when test="ArrivalDateTime != ''">
											<xsl:value-of select="translate(substring(ArrivalDateTime,12,5),':','')" />
										</xsl:when>
										<xsl:when test="substring(DepartureDateTime,12,5) = '00:00'">
											<xsl:choose>
												<xsl:when test="$dd = 0">1159</xsl:when>
												<xsl:otherwise>2359</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:variable name="at">
												<xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" />
											</xsl:variable>
											<xsl:value-of select="format-number($at + 200,'0000')"/>
										</xsl:otherwise>
									</xsl:choose>
								</time>
							</xsl:if>
							<xsl:if test="$dd1 = 0 or $dd1 = -1">
								<time>
									<xsl:choose>
										<xsl:when test="substring(DepartureDateTime,12,5) = '00:00'">
											<xsl:choose>
												<xsl:when test="$dd1= 0">1200</xsl:when>
												<xsl:otherwise>0000</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" />
										</xsl:otherwise>
									</xsl:choose>
								</time>
							</xsl:if>
						</xsl:when>
						<xsl:when test="(DepartureDateTime/@WindowBefore != '' and DepartureDateTime/@WindowBefore != '24') or (DepartureDateTime/@WindowAfter != '' and DepartureDateTime/@WindowAfter != '24')">
							<time>
								<xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" />
							</time>
						</xsl:when>
						<xsl:when test="substring(DepartureDateTime,12,5) != ''">
							<time>
								<xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" />
							</time>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="(DepartureDateTime/@WindowBefore != '' and DepartureDateTime/@WindowBefore != '24') or (DepartureDateTime/@WindowAfter != '' and DepartureDateTime/@WindowAfter != '24')">
						<timeWindow>
							<xsl:choose>
								<xsl:when test="DepartureDateTime/@WindowBefore != ''">
									<xsl:value-of select="DepartureDateTime/@WindowBefore"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="DepartureDateTime/@WindowAfter"/>
								</xsl:otherwise>
							</xsl:choose>
						</timeWindow>
					</xsl:if>
				</firstDateTimeDetail>
				<xsl:if test="DepartureDateTime/@WindowBefore = '24' or DepartureDateTime/@WindowAfter = '24'">
					<rangeOfDate>
						<rangeQualifier>
							<xsl:choose>
								<xsl:when test="DepartureDateTime/@WindowBefore = '24' and DepartureDateTime/@WindowAfter = '24'">C</xsl:when>
								<xsl:when test="DepartureDateTime/@WindowBefore = '24'">M</xsl:when>
								<xsl:otherwise>P</xsl:otherwise>
							</xsl:choose>
						</rangeQualifier>
						<dayInterval>1</dayInterval>
					</rangeOfDate>
				</xsl:if>
			</timeDetails>
		</itinerary>
	</xsl:template>
<!--**********************************************************************************************-->	
<msxsl:script language="VisualBasic" implements-prefix="ttVB">
<![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]>
</msxsl:script>

</xsl:stylesheet>
