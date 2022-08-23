<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:air="http://www.travelport.com/schema/air_v50_0"
				xmlns:universal="http://www.travelport.com/schema/universal_v50_0"
				xmlns:common="http://www.travelport.com/schema/common_v50_0" version="1.0">
	<!-- 
	================================================================== 
	Travelport_PNRRepriceRQ.xsl															
	================================================================== 
	Date: 19 Aug 2022 - Kobelev - Implamented Conversation ID.
	Date: 16 Mar 2022 - Kobelev - Branded Fare in Request	
	Date: 10 Nov 2014 - Rastko - New file											
	================================================================== 
  -->

	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_PNRRepriceRQ"/>
	</xsl:template>

	<xsl:template match="OTA_PNRRepriceRQ">
		<xsl:choose>
			<xsl:when test="not(Response) and not(NewPrice)">
				<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                    xmlns:air="http://www.travelport.com/schema/air_v50_0"
									xmlns:common="http://www.travelport.com/schema/common_v50_0"
                                    xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
                                    xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                    xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                    xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
					<xsl:attribute name="TargetBranch">
						<xsl:value-of select="POS/Source/RequestorID/@Instance" />
					</xsl:attribute>

					<xsl:if test="ConversationID">
						<xsl:attribute name="SessionKey">
							<xsl:value-of select="ConversationID" />
						</xsl:attribute>
					</xsl:if>

					<common:BillingPointOfSaleInfo OriginApplication="UAPI"/>
					<ProviderReservationInfo>
						<xsl:attribute name="ProviderCode">
							<xsl:choose>
								<xsl:when test="@Target='WSP'">
									<xsl:value-of select="'1P'"/>
								</xsl:when>
								<xsl:when test="@Target='APL'">
									<xsl:value-of select="'1V'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'1G'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="ProviderLocatorCode">
							<xsl:value-of select="UniqueID/@ID"/>
						</xsl:attribute>
					</ProviderReservationInfo>
					<!--
					<UniversalRecordLocatorCode>
						<xsl:value-of select="UniqueID/@ID"/>
					</UniversalRecordLocatorCode>
					-->

				</UniversalRecordRetrieveReq>
			</xsl:when>
			<xsl:when test="NewPrice">
				<xsl:apply-templates select="NewPrice"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="Response"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="Response">
		<air:AirPriceReq>
			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="../POS/Source/RequestorID/@Instance"/>
			</xsl:attribute>
			<xsl:if test="../ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="../ConversationID" />
				</xsl:attribute>
			</xsl:if>

			<common:BillingPointOfSaleInfo xmlns:com="http://www.travelport.com/schema/common_v50_0" OriginApplication="UAPI"/>
			<air:AirItinerary>
				<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirSegment"/>
			</air:AirItinerary>

			<xsl:if test="../StoredFare/BrandedFares">
				<air:AirPricingModifiers CurrencyType="USD" ProhibitAdvancePurchaseFares="false" ProhibitNonRefundableFares="true" ProhibitRestrictedFares="false" FaresIndicator="PublicAndPrivateFares" ProhibitMaxStayFares="false" ProhibitMinStayFares="false" AccountCodeFaresOnly="false" />
			</xsl:if>

			<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/common:BookingTraveler"/>
			<!--<xsl:call-template name="passanger_air"/>-->

			<xsl:if test="../StoredFare/BrandedFares">
				<air:AirPricingCommand/>
			</xsl:if>

			<xsl:if test="../../../../@StoreFare='true'">
				<xsl:copy-of select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/common:FormOfPayment"/>
				<air:AirReservationLocatorCode>
					<xsl:value-of select="../UniqueID/@ID"/>
				</air:AirReservationLocatorCode>
			</xsl:if>
		</air:AirPriceReq>

	</xsl:template>

	<xsl:template match="NewPrice">
		<xsl:variable name="PNR" select="../Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord"/>
		<xsl:variable name="Price" select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution"/>
		<universal:UniversalRecordModifyReq
      xmlns="http://www.travelport.com/schema/universal_v50_0"
      xmlns:air="http://www.travelport.com/schema/air_v50_0"
      xmlns:common="http://www.travelport.com/schema/common_v50_0"
      xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
      xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
      xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
      xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ReturnRecord="true">

			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="POS/Source/RequestorID/@Instance" />
			</xsl:attribute>

			<xsl:if test="ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="ConversationID" />
				</xsl:attribute>
			</xsl:if>

			<common:BillingPointOfSaleInfo OriginApplication="UAPI"/>
			<common:ContinuityCheckOverride>Yes</common:ContinuityCheckOverride>
			<universal:RecordIdentifier UniversalLocatorCode="{$PNR/@LocatorCode}" ProviderCode="{$PNR/universal:ProviderReservationInfo/@ProviderCode}" ProviderLocatorCode="{$PNR/universal:ProviderReservationInfo//@LocatorCode}"/>
			<universal:UniversalModifyCmd Key="1">
				<universal:AirDelete ReservationLocatorCode="{$PNR/air:AirReservation/@LocatorCode}" Element="AirPricingInfo" Key="{$PNR/air:AirReservation/air:AirPricingInfo/@Key}"/>
			</universal:UniversalModifyCmd>
			<universal:UniversalModifyCmd Key="2" >
				<universal:AirAdd ReservationLocatorCode="{$PNR/air:AirReservation/@LocatorCode}">
					<xsl:for-each select="$Price/air:AirPricingInfo">
						<xsl:copy-of select="."/>
						<!--
						<air:AirPricingInfo Key="{@Key}" TotalPrice="USD1361.12" BasePrice="USD1009.00" ApproximateTotalPrice="USD1361.12" ApproximateBasePrice="USD1009.00" Taxes="USD352.12" LatestTicketingTime="2013-06-03T23:59:00.000+00:00" PricingMethod="Auto" PlatingCarrier="CM" ProviderCode="1P">
							<air:FareInfo Key="@Key" FareBasis="UH07F90" PassengerTypeCode="ADT" Origin="BUE" Destination="PTY" EffectiveDate="2013-05-31T16:59:34.410-03:00" Amount="NUC449.50">
								<air:FareTicketDesignator Value="SR"/>
								<air:FareRuleKey FareInfoRef="11T" ProviderCode="1P">XzDMMwsHqPXIslq/Ib7oyrF6nNWs26q/O3RFsLGklWZ4VF+9+au0cPa0xAyxURWhjbG+8ywcYF6ql2SKdQowyK5atgV7J/OD</air:FareRuleKey>
							</air:FareInfo>
							<air:FareInfo Key="13T" FareBasis="UL07F90" PassengerTypeCode="ADT" Origin="PTY" Destination="BUE" EffectiveDate="2013-05-31T14:59:34.411-05:00" Amount="NUC399.50">
								<air:FareTicketDesignator Value="SR"/>
								<air:FareRuleKey FareInfoRef="13T" ProviderCode="1P">fFzFFyvSWMnIslq/Ib7oyrF6nNWs26q/CBIoKak2Uml4VF+9+au0cPa0xAyxURWhjbG+8ywcYF6ql2SKdQowyK5atgV7J/OD</air:FareRuleKey>
							</air:FareInfo>
							<air:BookingInfo BookingCode="U" FareInfoRef="11T" SegmentRef="0T"/>
							<air:BookingInfo BookingCode="U" FareInfoRef="13T" SegmentRef="1T"/>
							<air:TaxInfo Category="AR" Amount="USD50.50" Key="4T"/>
							<air:TaxInfo Category="XR" Amount="USD37.32" Key="5T"/>
							<air:TaxInfo Category="QO" Amount="USD10.00" Key="6T"/>
							<air:TaxInfo Category="TQ" Amount="USD10.00" Key="7T"/>
							<air:TaxInfo Category="ZK" Amount="USD201.80" Key="8T"/>
							<air:TaxInfo Category="AH" Amount="USD2.50" Key="9T"/>
							<air:TaxInfo Category="FZ" Amount="USD40.00" Key="10T"/>
							<air:FareCalc>ADT BUE CM PTY Q80.00 449.50CM BUE Q80.00 399.50NUC1009.00END ROE1.00CM XT10.00QO10.00TQ201.80ZK2.50AH40.00FZ</air:FareCalc>
							<air:PassengerType Code="ADT"/>
						</air:AirPricingInfo>
						-->
					</xsl:for-each>
				</universal:AirAdd>
			</universal:UniversalModifyCmd>
			<common:FileFinishingInfo/>
		</universal:UniversalRecordModifyReq>
	</xsl:template>

	<xsl:template match="air:AirSegment">
		<air:AirSegment>
			<xsl:attribute name="Key">
				<xsl:value-of select="@Key"/>
			</xsl:attribute>
			<xsl:attribute name="Group">
				<xsl:value-of select="@Group"/>
			</xsl:attribute>
			<xsl:attribute name="ClassOfService">
				<xsl:value-of select="@ClassOfService"/>
			</xsl:attribute>
			<xsl:attribute name="Carrier">
				<xsl:value-of select="@Carrier"/>
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="@FlightNumber"/>
			</xsl:attribute>
			<xsl:attribute name="ProviderCode">
				<xsl:value-of select="@ProviderCode"/>
			</xsl:attribute>
			<xsl:attribute name="Origin">
				<xsl:value-of select="@Origin"/>
			</xsl:attribute>
			<xsl:attribute name="Destination">
				<xsl:value-of select="@Destination"/>
			</xsl:attribute>
			<xsl:attribute name="DepartureTime">
				<xsl:value-of select="@DepartureTime"/>
			</xsl:attribute>
			<xsl:attribute name="ArrivalTime">
				<xsl:value-of select="@ArrivalTime"/>
			</xsl:attribute>
			<xsl:attribute name="ETicketability">
				<xsl:value-of select="@ETicketability"/>
			</xsl:attribute>
			<xsl:attribute name="Equipment">
				<xsl:value-of select="air:FlightDetails/@Equipment"/>
			</xsl:attribute>
			<xsl:attribute name="ChangeOfPlane">
				<xsl:value-of select="@ChangeOfPlane"/>
			</xsl:attribute>
			<xsl:attribute name="OptionalServicesIndicator">
				<xsl:value-of select="@OptionalServicesIndicator"/>
			</xsl:attribute>
			<xsl:attribute name="ParticipantLevel">
				<xsl:value-of select="@ParticipantLevel"/>
			</xsl:attribute>
		</air:AirSegment>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="brandFare">
		<xsl:param name="ptc" />
		<air:AirPricingCommand>
			<!--<xsl:variable name="bn" select="air:FareInfo/air:Brand[1]/@BrandID" />-->
			<xsl:variable name="bn" select="../../../../../StoredFare[PassengerType/@Code=$ptc]/BrandedFares/FareFamily" />

			<xsl:for-each select="../air:AirSegment">
				<air:AirSegmentPricingModifiers>
					<xsl:attribute name="AirSegmentRef">
						<xsl:value-of select="@Key"/>
					</xsl:attribute>
					<xsl:attribute name="BrandTier">
						<xsl:value-of select="$bn[@RPH = position()]/@Code"/>
					</xsl:attribute>
					<air:PermittedBookingCodes>
						<air:BookingCode>
							<xsl:attribute name="Code">
								<xsl:value-of select="@ClassOfService"/>
							</xsl:attribute>
						</air:BookingCode>
					</air:PermittedBookingCodes>
				</air:AirSegmentPricingModifiers>

			</xsl:for-each>
		</air:AirPricingCommand>
	</xsl:template>

	<xsl:template name="passanger_air">
		<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/common:BookingTraveler"/>
	</xsl:template>

	<xsl:template match="common:BookingTraveler">
		<xsl:variable name="key" select="@Key" />
		<xsl:variable name="ptc" select="../air:AirReservation/air:AirPricingInfo/air:PassengerType[@BookingTravelerRef=$key]/@Code" />

		<common:SearchPassenger xmlns:common="http://www.travelport.com/schema/common_v50_0">
			<xsl:attribute name="BookingTravelerRef">
				<xsl:value-of select="@Key"/>
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="$ptc"/>
			</xsl:attribute>
			<!-- @TravelerType -->
		</common:SearchPassenger>
		<!-- -->
		<xsl:choose>
			<xsl:when test="../../../../StoredFare[PassengerType/@Code]/BrandedFares and ../../../../@StoreFare='true'">
				<xsl:apply-templates select="../air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]" mode="brandFare">
					<xsl:with-param name="ptc" select="$ptc" />
				</xsl:apply-templates>
			</xsl:when>
			<!--
			<xsl:otherwise>
				<air:AirPricingCommand/>
			</xsl:otherwise>
			-->
		</xsl:choose>

	</xsl:template>
</xsl:stylesheet>
