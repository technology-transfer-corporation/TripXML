<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:air="http://www.travelport.com/schema/air_v28_0" xmlns:universal="http://www.travelport.com/schema/universal_v28_0" xmlns:common_v28_0="http://www.travelport.com/schema/common_v28_0" version="1.0">
	<!-- 
  ================================================================== 
	Travelport_PNRRepriceRQ.xsl															
	================================================================== 
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
				<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v28_0" 
                                    xmlns:air_v28_0="http://www.travelport.com/schema/air_v28_0" 
																		xmlns:common_v28_0="http://www.travelport.com/schema/common_v28_0" 
                                    xmlns:hotel="http://www.travelport.com/schema/hotel_v28_0" 
                                    xmlns:passive="http://www.travelport.com/schema/passive_v28_0" 
                                    xmlns:rail="http://www.travelport.com/schema/rail_v28_0" 
                                    xmlns:vehicle="http://www.travelport.com/schema/vehicle_v28_0" 
                                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" TargetBranch="{POS/Source/@PseudoCityCode}">
					<common_v28_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>
					<UniversalRecordLocatorCode>
						<xsl:value-of select="UniqueID/@ID"/>
					</UniversalRecordLocatorCode>
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
				<xsl:value-of select="../POS/Source/@PseudoCityCode"/>
			</xsl:attribute>
			<common_v28_0:BillingPointOfSaleInfo xmlns:com="http://www.travelport.com/schema/common_v28_0" OriginApplication="UAPI"/>
			<air:AirItinerary>
				<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirSegment"/>
			</air:AirItinerary>
			<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/common_v28_0:BookingTraveler"/>
			<air:AirPricingCommand/>
		</air:AirPriceReq>
	</xsl:template>
	
	<xsl:template match="NewPrice">
		<xsl:variable name="PNR" select="../Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord"/>
		<xsl:variable name="Price" select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution"/>
		<universal:UniversalRecordModifyReq 
      xmlns="http://www.travelport.com/schema/universal_v28_0" 
      xmlns:air_v28_0="http://www.travelport.com/schema/air_v28_0" 
      xmlns:common_v28_0="http://www.travelport.com/schema/common_v28_0" 
      xmlns:hotel="http://www.travelport.com/schema/hotel_v28_0" 
      xmlns:passive="http://www.travelport.com/schema/passive_v28_0" 
      xmlns:rail="http://www.travelport.com/schema/rail_v28_0" 
      xmlns:vehicle="http://www.travelport.com/schema/vehicle_v28_0" 
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" TargetBranch="{POS/Source/@PseudoCityCode}" ReturnRecord="true">
			<common_v28_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>
			<universal:RecordIdentifier UniversalLocatorCode="{$PNR/@LocatorCode}" ProviderCode="{$PNR/universal:ProviderReservationInfo/@ProviderCode}" ProviderLocatorCode="{$PNR/universal:ProviderReservationInfo//@LocatorCode}"/>
			<universal:UniversalModifyCmd Key="1">
				<universal:AirDelete ReservationLocatorCode="{$PNR/air:AirReservation/@LocatorCode}" Element="AirPricingInfo" Key="{$PNR/air:AirReservation/air:AirPricingInfo/@Key}"/>
			</universal:UniversalModifyCmd>
			<universal:UniversalModifyCmd Key="2" >
				<universal:AirAdd ReservationLocatorCode="{$PNR/air:AirReservation/@LocatorCode}">
					<xsl:for-each select="$Price/air:AirPricingInfo">
						<xsl:copy-of select="."/>
						<!--air:AirPricingInfo Key="{@Key}" TotalPrice="USD1361.12" BasePrice="USD1009.00" ApproximateTotalPrice="USD1361.12" ApproximateBasePrice="USD1009.00" Taxes="USD352.12" LatestTicketingTime="2013-06-03T23:59:00.000+00:00" PricingMethod="Auto" PlatingCarrier="CM" ProviderCode="1P">
							<air:FareInfo Key="11T" FareBasis="UH07F90" PassengerTypeCode="ADT" Origin="BUE" Destination="PTY" EffectiveDate="2013-05-31T16:59:34.410-03:00" Amount="NUC449.50">
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
						</air:AirPricingInfo-->
					</xsl:for-each>
				</universal:AirAdd>
			</universal:UniversalModifyCmd>
			<common_v28_0:FileFinishingInfo/>
		</universal:UniversalRecordModifyReq>
	</xsl:template>
	
	<xsl:template match="air:AirSegment">
		<air:AirSegment>
			<xsl:attribute name="Key"><xsl:value-of select="@Key"/></xsl:attribute>
			<xsl:attribute name="Group"><xsl:value-of select="@Group"/></xsl:attribute>
			<xsl:attribute name="ClassOfService"><xsl:value-of select="@ClassOfService"/></xsl:attribute>
			<xsl:attribute name="Carrier"><xsl:value-of select="@Carrier"/></xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="@FlightNumber"/></xsl:attribute>
			<xsl:attribute name="ProviderCode"><xsl:value-of select="@ProviderCode"/></xsl:attribute>
			<xsl:attribute name="Origin"><xsl:value-of select="@Origin"/></xsl:attribute>
			<xsl:attribute name="Destination"><xsl:value-of select="@Destination"/></xsl:attribute>
			<xsl:attribute name="DepartureTime"><xsl:value-of select="@DepartureTime"/></xsl:attribute>
			<xsl:attribute name="ArrivalTime"><xsl:value-of select="@ArrivalTime"/></xsl:attribute>
			<xsl:attribute name="ETicketability"><xsl:value-of select="@ETicketability"/></xsl:attribute>
			<xsl:attribute name="Equipment"><xsl:value-of select="air:FlightDetails/@Equipment"/></xsl:attribute>
			<xsl:attribute name="ChangeOfPlane"><xsl:value-of select="@ChangeOfPlane"/></xsl:attribute>
			<xsl:attribute name="OptionalServicesIndicator"><xsl:value-of select="@OptionalServicesIndicator"/></xsl:attribute>
			<xsl:attribute name="ParticipantLevel"><xsl:value-of select="@ParticipantLevel"/></xsl:attribute>
		</air:AirSegment>
	</xsl:template>
		
	<xsl:template match="common_v28_0:BookingTraveler">
		<common_v28_0:SearchPassenger xmlns:com="http://www.travelport.com/schema/common_v28_0">
			<xsl:attribute name="BookingTravelerRef"><xsl:value-of select="@Key"/></xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="@TravelerType"/></xsl:attribute>
		</common_v28_0:SearchPassenger>
	</xsl:template>
</xsl:stylesheet>
