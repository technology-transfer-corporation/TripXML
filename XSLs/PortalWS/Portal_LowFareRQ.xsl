<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Portal_LowFareRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 04 Jun 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">		
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ"/>	
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OTA_AirLowFareSearchRQ">
		<SearchForPackages xmlns="http://odyssey.com/webservices/">
			<requestObj xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
				<MaxSearchResults>0</MaxSearchResults>
				<InternationalFlight>false</InternationalFlight>
				<InternationalPublishedMarkup>0</InternationalPublishedMarkup>
				<InternationalPrivateMarkup>0</InternationalPrivateMarkup>
				<DomesticPublishedMarkup>0</DomesticPublishedMarkup>
				<DomesticPrivateMarkup>0</DomesticPrivateMarkup>
				<InternationalPublishedMarkupPercent>0</InternationalPublishedMarkupPercent>
				<InternationalPrivateMarkupPercent>0</InternationalPrivateMarkupPercent>
				<DomesticPublishedMarkupPercent>0</DomesticPublishedMarkupPercent>
				<DomesticPrivateMarkupPercent>0</DomesticPrivateMarkupPercent>
				<DirectFlight>false</DirectFlight>
				<AirSearchObj>
					<xsl:apply-templates select="OriginDestinationInformation"/>
				</AirSearchObj>
				<AirPackage>
					<internationalFlight>false</internationalFlight>
					<DiscountApplied>0</DiscountApplied>
					<AirJourneyType>
						<ID>0</ID>
						<SupplierID>0</SupplierID>
						<LanguageID>0</LanguageID>
						<PseudoCode>Circle</PseudoCode>
						<Active>false</Active>
					</AirJourneyType>
					<PaperTicketRequested>false</PaperTicketRequested>
					<SequenceNumber>0</SequenceNumber>
					<Flights/>
					<ClassTypeChanged>false</ClassTypeChanged>
					<CommissionPercent>0</CommissionPercent>
					<TicketIssueDate>0001-01-01T00:00:00</TicketIssueDate>
					<PaymentSchedule/>
					<DepartureDateTime>0001-01-01T00:00:00.0000000-05:00</DepartureDateTime>
					<ArrivalDateTime>0001-01-01T00:00:00</ArrivalDateTime>
					<Duration>0</Duration>
				</AirPackage>
				<ClassType>
					<xsl:choose>
						<xsl:when test="TravelPreferences/CabinPref/@Cabin!=''">
							<xsl:value-of select="TravelPreferences/CabinPref/@Cabin"/>
						</xsl:when>
						<xsl:otherwise>Economy</xsl:otherwise>
					</xsl:choose>
				</ClassType>
				<RefundableTicket>false</RefundableTicket>
				<UserID>0</UserID>
				<ApplicationID>0</ApplicationID>
				<AdultCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</AdultCount>
				<ChildCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</ChildCount>
				<YouthCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</YouthCount>
				<SeniorCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</SeniorCount>
				<InfantCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</InfantCount>
				<SeatedInfantCount>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity!=''">
							<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity"/>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</SeatedInfantCount>
				<CurrencyID>
					<xsl:choose>
						<xsl:when test="POS/Source/@ISOCurrency!=''">
							<xsl:value-of select="POS/Source/@ISOCurrency"/>
						</xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</CurrencyID>
				<SiteItemID>0</SiteItemID>
				<SiteItemTypeID>0</SiteItemTypeID>
				<ParentSiteItemID>0</ParentSiteItemID>
				<GrandParentSiteItemID>0</GrandParentSiteItemID>
				<LanguageID>1</LanguageID>
			</requestObj>
		</SearchForPackages>
	</xsl:template>	
	<!---********************************************************************-->
	<!--   Itinerary information begins here                                 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationInformation">
		<AirSearchObject>
			<DepartureDateTime><xsl:value-of select="DepartureDateTime"/></DepartureDateTime>
			<SearchFromDepDateTime>0001-01-01T00:00:00</SearchFromDepDateTime>
			<SearchAfterDepDateTime>0001-01-01T00:00:00</SearchAfterDepDateTime>
			<ArrivalDateTime>0001-01-01T00:00:00</ArrivalDateTime>
			<DepartureCity><xsl:value-of select="OriginLocation/@LocationCode"/></DepartureCity>
			<ArrivalCity><xsl:value-of select="DestinationLocation/@LocationCode"/></ArrivalCity>
		</AirSearchObject>
	</xsl:template>
</xsl:stylesheet>
