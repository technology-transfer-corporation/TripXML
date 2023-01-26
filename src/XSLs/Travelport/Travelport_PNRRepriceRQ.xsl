<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:air="http://www.travelport.com/schema/air_v50_0"
				xmlns:universal="http://www.travelport.com/schema/universal_v50_0"
				xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- 
	================================================================== 
	Travelport_PNRRepriceRQ.xsl															
	================================================================== 
	Date: 17 Jan 2022 - Kobelev - For Published PNR - Commissions / Endoursments /TourCode
	Date: 06 Dec 2022 - Kobelev - AirPricingModifiers (Nego Price) for markups through air:ManualFareAdjustment fields.
	Date: 04 Nov 2022 - Kobelev - AirPricingModifiers (Pub/Nego Price) updated. Generalized ADT- JWA, CHD - JWC, INF - JWB
	Date: 02 Nov 2022 - Samokhvalov - AirPricingModifiers (Pub/Nego Price) added
	Date: 28 Oct 2022 - Kobelev - AirPricing Groupping via AirPricingInfoGroup
	Date: 06 Oct 2022 - Kobelev - Commissions / Endoursments /TourCode
	Date: 06 Oct 2022 - Kobelev - BookingTraveler for UniversalModifyCmd
	Date: 05 Oct 2022 - Kobelev - RePrice w/stored Fare AirPrice association.
	Date: 30 Sep 2022 - Kobelev - RePrice w/stored Fare.
	Date: 20 Sep 2022 - Kobelev - RePrice with stored fare request fix for multiple PTCs.
	Date: 09 Sep 2022 - Kobelev - Use of PNR Brand Data vs. Brand Data from Request.
	Date: 19 Aug 2022 - Kobelev - Implamented Conversation ID.
	Date: 16 Mar 2022 - Kobelev - Branded Fare in Request	
	Date: 10 Nov 2014 - Rastko - New file											
	================================================================== 
    -->

	<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:key name="ptclist" match="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo/air:PassengerType/@Code" use="."/>

	<xsl:template match="/">
		<xsl:apply-templates select="OTA_PNRRepriceRQ"/>
	</xsl:template>

	<xsl:template match="OTA_PNRRepriceRQ">
		<xsl:choose>
			<xsl:when test="not(Response) and not(NewPrice)">
				<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                    xmlns:air="http://www.travelport.com/schema/air_v50_0"
									xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
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

					<xsl:if test="UUID">
						<xsl:attribute name="TransactionId">
							<xsl:value-of select="UUID" />
						</xsl:attribute>
					</xsl:if>

					<common_v50_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>
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
			<xsl:when test="UpdatePrice">
				<xsl:apply-templates select="UpdatePrice">
					<xsl:with-param name="version" select="Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/@Version"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="Response"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="Response">
		<xsl:variable name="PNR" select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord"/>
		<air:AirPriceReq>
			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="../POS/Source/RequestorID/@Instance"/>
			</xsl:attribute>
			<xsl:if test="../ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="../ConversationID" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="../UUID">
				<xsl:attribute name="TransactionId">
					<xsl:value-of select="../UUID" />
				</xsl:attribute>
			</xsl:if>
			<common_v50_0:BillingPointOfSaleInfo xmlns:com="http://www.travelport.com/schema/common_v50_0" OriginApplication="UAPI"/>
			<air:AirItinerary>
				<xsl:apply-templates select="$PNR/air:AirReservation/air:AirSegment"/>
			</air:AirItinerary>

			<!--*****  or ( //StoredFare/FareSegments/AirSegments/@TicketDesignator !='') *****-->
			<xsl:if test="//StoredFare/Markup and //@StoreFare='true' and //StoredFare/@FareType='Private'" >
				<xsl:call-template name="markups">
					<xsl:with-param name="pnr" select="$PNR" />
				</xsl:call-template>
			</xsl:if>

			<xsl:call-template name="passanger_air">
				<xsl:with-param name="action" select="'price'" />
				<xsl:with-param name="pnr" select="$PNR" />
			</xsl:call-template>
			<!--
			<xsl:if test="../@StoreFare='true'">
				<xsl:if test="../StoredFare/BrandedFares">
					<air:AirPricingCommand/>
				</xsl:if>
				<xsl:copy-of select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/common:FormOfPayment"/>
				<air:AirReservationLocatorCode>
					<xsl:value-of select="../UniqueID/@ID"/>
				</air:AirReservationLocatorCode>
			</xsl:if>
			-->

		</air:AirPriceReq>
	</xsl:template>

	<xsl:template match="NewPrice">
		<xsl:variable name="PNR" select="../Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord"/>
		<xsl:variable name="priceType" select="$PNR/air:AirReservation/air:AirPricingInfo[1]/@PricingMethod"/>
		<xsl:variable name="Price" select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution"/>
		<!--[air:AirPricingInfo/@PricingMethod=$priceType]-->
		<universal:UniversalRecordModifyReq
xmlns="http://www.travelport.com/schema/universal_v50_0"
xmlns:air="http://www.travelport.com/schema/air_v50_0"
xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ReturnRecord="true">

			<xsl:attribute name="Version">
				<xsl:value-of select="$PNR/@Version" />
			</xsl:attribute>
			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="../POS/Source/RequestorID/@Instance" />
			</xsl:attribute>
			<xsl:if test="../ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="../ConversationID" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="../UUID">
				<xsl:attribute name="TransactionId">
					<xsl:value-of select="../UUID" />
				</xsl:attribute>
			</xsl:if>
			<common_v50_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>
			<common_v50_0:ContinuityCheckOverride>Yes</common_v50_0:ContinuityCheckOverride>
			<xsl:call-template name="passanger_air">
				<xsl:with-param name="action" select="'new'" />
				<xsl:with-param name="pnr" select="$PNR" />
				<xsl:with-param name="airprice" select="$Price" />
			</xsl:call-template>
			<common_v50_0:FileFinishingInfo/>
		</universal:UniversalRecordModifyReq>
	</xsl:template>

	<xsl:template match="UpdatePrice">
		<xsl:param name="version" />
		<xsl:variable name="PNR" select="../Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord"/>

		<xsl:variable name="fareBasis" select="concat('01',$PNR/air:AirReservation/air:AirPricingInfo/air:FareInfo[1]/@FareBasis)" />

		<xsl:variable name="Price" select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution[contains(common_v50_0:HostToken, $fareBasis)]/air:AirPricingInfo"/>
		<!--"air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution"-->
		<universal:UniversalRecordModifyReq
			xmlns="http://www.travelport.com/schema/universal_v50_0"
			xmlns:air="http://www.travelport.com/schema/air_v50_0"
			xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
			xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
			xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
			xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
			xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
			xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ReturnRecord="true">

			<xsl:attribute name="Version">
				<xsl:value-of select="$PNR[1]/@Version" />
			</xsl:attribute>
			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="../POS/Source/RequestorID/@Instance" />
			</xsl:attribute>
			<xsl:attribute name="ReturnRecord">
				<xsl:text>true</xsl:text>
			</xsl:attribute>
			<xsl:if test="../ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="../ConversationID" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="../UUID">
				<xsl:attribute name="TransactionId">
					<xsl:value-of select="../UUID" />
				</xsl:attribute>
			</xsl:if>

			<common_v50_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>

			<xsl:call-template name="mod_air">
				<xsl:with-param name="pnr" select="$PNR" />
				<xsl:with-param name="airprice" select="$Price" />
			</xsl:call-template>

			<common_v50_0:FileFinishingInfo/>
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

	<xsl:template name="passanger_air">
		<xsl:param name="action" />
		<xsl:param name="pnr" />
		<xsl:param name="airprice" />
		<xsl:variable name="priceType" select="//StoredFare[PassengerType/@Code]/@FareType" />
		<xsl:variable name="pricing">
			<xsl:choose>
				<xsl:when test="contains($priceType,'Published')">
					<xsl:text>Guaranteed</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<!--<xsl:copy-of select="$airprice[contains(@PricingMethod, 'PrivateFare')][1]"/>-->
					<xsl:text>GuaranteedUsingAirlinePrivateFare</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!--
		<xsl:if test="not (../StoredFare[PassengerType/@Code]/BrandedFares or $pnr/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFare'][1]/air:FareInfo/air:Brand or $pnr/air:AirReservation/air:AirPricingInfo[@PricingType='TicketRecord'][1]/air:FareInfo/air:Brand)">
			<xsl:choose>
				<xsl:when test="../StoredFare/@FareType = 'Private'">
					<air:AirPricingModifiers FaresIndicator="PrivateFaresOnly"/>
				</xsl:when>
				<xsl:otherwise>
					<air:AirPricingModifiers FaresIndicator="PublicFaresOnly"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		-->
		<xsl:if test="$action='price'">
			<xsl:apply-templates select="$pnr/common_v50_0:BookingTraveler"/>
		</xsl:if>
		<xsl:if test="$action='new'">
			<universal:RecordIdentifier>
				<xsl:attribute name="ProviderCode">
					<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@ProviderCode"/>
				</xsl:attribute>
				<xsl:attribute name="ProviderLocatorCode">
					<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@LocatorCode"/>
				</xsl:attribute>
				<xsl:attribute name="UniversalLocatorCode">
					<xsl:value-of select="$pnr/@LocatorCode"/>
				</xsl:attribute>
			</universal:RecordIdentifier>
		</xsl:if>

		<xsl:variable name="class" select="$pnr/air:AirReservation/air:AirPricingInfo[1]/air:BookingInfo" />
		<xsl:variable name="td" select="$pnr/air:AirReservation/air:AirPricingInfo[1]/air:FareInfo/air:FareTicketDesignator/@Value" />

		<xsl:variable name="brnds" >
			<xsl:call-template name="DistictList">
				<xsl:with-param name="list" select="//StoredFare/BrandedFares/FareFamily/@Code"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="index">
			<xsl:if test="$action='new'">
				<xsl:choose>
					<xsl:when test="count($airprice) = 1">
						<xsl:value-of select="count($airprice)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="airPriceFilter">
							<xsl:with-param name="airprice" select="$airprice" />
							<xsl:with-param name="class" select="$class" />
							<xsl:with-param name="td" select="$td" />
							<xsl:with-param name="brnds" select="$brnds" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="../StoredFare[PassengerType/@Code]/BrandedFares">
				<xsl:choose>
					<xsl:when test="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFare']">
						<xsl:choose>
							<xsl:when test="$action='new'">
								<universal:UniversalModifyCmd Key="count($pnr/air:AirReservation/air:AirPricingInfo) + 1">
									<universal:AirAdd>
										<xsl:attribute name="ReservationLocatorCode">
											<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
										</xsl:attribute>

										<xsl:variable name="elems" >
											<xsl:call-template name="DistictList">
												<xsl:with-param name="list" select="$airprice[position() = $index]/air:AirPricingInfo/air:PassengerType/@Code"/>
											</xsl:call-template>
										</xsl:variable>

										<xsl:for-each select="msxsl:node-set($elems)/elem">
											<xsl:variable name="p" select="./node()[1]" />
											<xsl:apply-templates select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" mode="brandFareStore">
												<xsl:with-param name="delete" select="'true'" />
											</xsl:apply-templates>
										</xsl:for-each>
									</universal:AirAdd>
								</universal:UniversalModifyCmd>
							</xsl:when>
							<xsl:otherwise>
								<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFare'][1]" mode="brandFare">
									<xsl:with-param name="ptc" select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
								</xsl:apply-templates>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFareQoute']">
						<xsl:choose>
							<xsl:when test="$action='new'">
								<universal:UniversalModifyCmd Key="count($pnr/air:AirReservation/air:AirPricingInfo) + 1">
									<universal:AirAdd>
										<xsl:attribute name="ReservationLocatorCode">
											<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
										</xsl:attribute>

										<xsl:variable name="elems" >
											<xsl:call-template name="DistictList">
												<xsl:with-param name="list" select="$airprice[position() = $index]/air:AirPricingInfo/air:PassengerType/@Code"/>
											</xsl:call-template>
										</xsl:variable>

										<xsl:for-each select="msxsl:node-set($elems)/elem">
											<xsl:variable name="p" select="./node()[1]" />
											<xsl:apply-templates select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" mode="brandFareStore">
												<xsl:with-param name="delete" select="'true'" />
											</xsl:apply-templates>
										</xsl:for-each>
									</universal:AirAdd>
								</universal:UniversalModifyCmd>
							</xsl:when>
							<xsl:otherwise>
								<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFareQoute'][1]" mode="brandFare">
									<xsl:with-param name="ptc" select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
								</xsl:apply-templates>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$action='new'">
								<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo">
									<xsl:variable name="pos" select="position()"/>
									<universal:UniversalModifyCmd>
										<xsl:attribute name="Key">
											<xsl:value-of select="$pos"/>
										</xsl:attribute>
										<universal:AirDelete Element="AirPricingInfo">
											<xsl:attribute name="Key">
												<xsl:value-of select="@Key"/>
											</xsl:attribute>
											<xsl:attribute name="ReservationLocatorCode">
												<xsl:value-of select="//air:AirReservation/@LocatorCode"/>
											</xsl:attribute>
										</universal:AirDelete>
									</universal:UniversalModifyCmd>
								</xsl:for-each>

								<universal:UniversalModifyCmd>
									<xsl:attribute name="Key">
										<xsl:value-of select="count($pnr/air:AirReservation/air:AirPricingInfo) + 1"/>
									</xsl:attribute>
									<xsl:variable name="resCode" select="$pnr/air:AirReservation/@LocatorCode"/>

									<universal:AirAdd>
										<xsl:attribute name="ReservationLocatorCode">
											<xsl:value-of select="$resCode"/>
										</xsl:attribute>

										<xsl:variable name="elems" >
											<xsl:call-template name="DistictList">
												<xsl:with-param name="list" select="$airprice[position() = $index]/air:AirPricingInfo/air:PassengerType/@Code"/>
											</xsl:call-template>
										</xsl:variable>

										<xsl:for-each select="msxsl:node-set($elems)/elem">
											<xsl:variable name="p" select="./node()[1]" />
											<xsl:apply-templates select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" mode="brandFareStore">
												<xsl:with-param name="delete" select="'true'" />
											</xsl:apply-templates>
										</xsl:for-each>
									</universal:AirAdd>

								</universal:UniversalModifyCmd>
							</xsl:when>
							<xsl:otherwise>
								<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='TicketRecord'][1]" mode="brandFare">
									<xsl:with-param name="ptc" select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
								</xsl:apply-templates>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFare'][1]/air:FareInfo/air:Brand">
				<xsl:choose>
					<xsl:when test="$action='new'">
						<universal:UniversalModifyCmd Key="2">
							<universal:AirAdd>
								<xsl:attribute name="ReservationLocatorCode">
									<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
								</xsl:attribute>

								<xsl:variable name="elems" >
									<xsl:call-template name="DistictList">
										<xsl:with-param name="list" select="$airprice[position() = $index]/air:AirPricingInfo/air:PassengerType/@Code"/>
									</xsl:call-template>
								</xsl:variable>

								<xsl:for-each select="msxsl:node-set($elems)/elem">
									<xsl:variable name="p" select="./node()[1]" />
									<xsl:apply-templates select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" mode="brandFareStore">
										<xsl:with-param name="delete" select="'true'" />
									</xsl:apply-templates>
								</xsl:for-each>

							</universal:AirAdd>
						</universal:UniversalModifyCmd>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='StoredFare'][1]" mode="brandFare">
							<xsl:with-param name="ptc" select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='TicketRecord'][1]/air:FareInfo/air:Brand">
				<xsl:choose>
					<xsl:when test="$action='new'">
						<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo">
							<xsl:variable name="pos" select="position()"/>
							<universal:UniversalModifyCmd>
								<xsl:attribute name="Key">
									<xsl:value-of select="$pos"/>
								</xsl:attribute>
								<universal:AirDelete Element="AirPricingInfo">
									<xsl:attribute name="Key">
										<xsl:value-of select="@Key"/>
									</xsl:attribute>
									<xsl:attribute name="ReservationLocatorCode">
										<xsl:value-of select="../../air:AirReservation/@LocatorCode"/>
									</xsl:attribute>
								</universal:AirDelete>
							</universal:UniversalModifyCmd>
						</xsl:for-each>
						<universal:UniversalModifyCmd>
							<xsl:attribute name="Key">
								<xsl:value-of select="count($pnr/air:AirReservation/air:AirPricingInfo) + 1"/>
							</xsl:attribute>
							<universal:AirAdd>
								<xsl:attribute name="ReservationLocatorCode">
									<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
								</xsl:attribute>
								<xsl:variable name="elems" >
									<xsl:call-template name="DistictList">
										<xsl:with-param name="list" select="$airprice[position() = $index]/air:AirPricingInfo/air:PassengerType/@Code"/>
									</xsl:call-template>
								</xsl:variable>

								<xsl:for-each select="msxsl:node-set($elems)/elem">
									<xsl:variable name="p" select="./node()[1]" />
									<xsl:apply-templates select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" mode="brandFareStore">
										<xsl:with-param name="delete" select="'true'" />
									</xsl:apply-templates>
								</xsl:for-each>
							</universal:AirAdd>
						</universal:UniversalModifyCmd>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='TicketRecord'][1]" mode="brandFare">
							<xsl:with-param name="ptc" select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="airPriceFilter">
		<xsl:param name="airprice" />
		<xsl:param name="class" />
		<xsl:param name="td" />
		<xsl:param name="brnds" />

		<xsl:variable name="brand" select="msxsl:node-set($brnds)/elem/node()[1]" />

		<xsl:variable name="pos">
			<xsl:choose>
				<xsl:when test="count($airprice[air:AirPricingInfo/air:FareInfo/air:Brand/@BrandTier = format-number($brand, '0000')]) = 1">
					<xsl:value-of select="1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$airprice[air:AirPricingInfo/air:FareInfo/air:Brand/@BrandTier = format-number($brand, '0000')]">
						<xsl:variable name="_pos" select="position()" />
						<xsl:variable name="_xClass" select="air:AirPricingInfo/air:BookingInfo/@BookingCode[not(.=$class/@BookingCode)]" />
						<xsl:variable name="_xTD" select="air:AirPricingInfo/air:FareInfo/air:FareTicketDesignator/@Value[not(.=$td)]" />
						<xsl:if test="not(air:AirPricingInfo/air:BookingInfo/@BookingCode[not(.=$class/@BookingCode)]) and not(air:AirPricingInfo/air:FareInfo/air:FareTicketDesignator/@Value[not(.=$td)])">
							<xsl:value-of select="$_pos"/>
						</xsl:if>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="format-number(substring($pos,1,1), '#0')"/>
	</xsl:template>

	<xsl:template name="mod_air">
		<xsl:param name="pnr" />
		<xsl:param name="airprice" />

		<universal:RecordIdentifier>
			<xsl:attribute name="ProviderCode">
				<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@ProviderCode"/>
			</xsl:attribute>
			<xsl:attribute name="ProviderLocatorCode">
				<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@LocatorCode"/>
			</xsl:attribute>
			<xsl:attribute name="UniversalLocatorCode">
				<xsl:value-of select="$pnr/@LocatorCode"/>
			</xsl:attribute>
		</universal:RecordIdentifier>

		<xsl:for-each select="$airprice">
			<universal:UniversalModifyCmd>
				<xsl:attribute name="Key">
					<xsl:value-of select="position() + 1"/>
				</xsl:attribute>
				<universal:AirAdd>
					<xsl:attribute name="ReservationLocatorCode">
						<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
					</xsl:attribute>
					<xsl:call-template name="ModStoredFare" >
						<xsl:with-param name="pnr" select="$pnr" />
						<xsl:with-param name="airprice" select="." />
					</xsl:call-template>
				</universal:AirAdd>
			</universal:UniversalModifyCmd>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="common_v50_0:BookingTraveler">
		<xsl:variable name="key" select="@Key" />
		<xsl:variable name="optc" select="../air:AirReservation/air:AirPricingInfo/air:PassengerType[@BookingTravelerRef=$key]/@Code" />
		<xsl:variable name="sfptc" select="//StoredFare[substring(PassengerType/@Code, 1, 1) = substring($optc,1,1) ]/PassengerType/@Code" />
		<xsl:variable name="ptc">
			<xsl:call-template name="private_ptc" >
				<xsl:with-param name="optc" select="$optc"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="age">
			<xsl:choose>
				<xsl:when test="substring($sfptc, 1, 1) = 'C'">
					<xsl:value-of select="concat('0',number(substring($sfptc, 2, 2)))"/>
				</xsl:when>
				<xsl:when test="concat('0',number(substring($sfptc, 2, 2))) = substring($sfptc, 2, 2)">
					<xsl:value-of select="substring($sfptc, 2, 2)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text></xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<common_v50_0:SearchPassenger>
			<xsl:attribute name="BookingTravelerRef">
				<xsl:value-of select="@Key"/>
			</xsl:attribute>
			<xsl:attribute name="Key">
				<xsl:value-of select="@Key"/>
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="$ptc"/>
			</xsl:attribute>
			<xsl:if test="$age != ''">
				<xsl:attribute name="Age">
					<xsl:value-of select="$age"/>
				</xsl:attribute>
			</xsl:if>
		</common_v50_0:SearchPassenger>
		<!--
		<xsl:choose>
			<xsl:when test="../../../../StoredFare[PassengerType/@Code]/BrandedFares and ../../../../@StoreFare='true'">
				<xsl:apply-templates select="../air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]" mode="brandFare">
					<xsl:with-param name="ptc" select="$ptc" />
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="../air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]/air:FareInfo/air:Brand and ../../../../@StoreFare='true'">
				<xsl:apply-templates select="../air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]" mode="brandFare">
					<xsl:with-param name="ptc" select="$ptc" />
				</xsl:apply-templates>
			</xsl:when>
			
			<xsl:otherwise>
				<air:AirPricingCommand/>
			</xsl:otherwise>
			
		</xsl:choose>
	    -->
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="brandFare">
		<xsl:param name="ptc" />
		<air:AirPricingCommand>
			<!--<xsl:variable name="bn" select="air:FareInfo/air:Brand[1]/@BrandID" />-->
			<xsl:attribute name="FaresIndicator">
				<xsl:choose>
					<xsl:when test="//StoredFare/@FareType = 'Private'">
						<xsl:text>PrivateFaresOnly</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>PublicFaresOnly</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="//StoredFare[PassengerType/@Code=$ptc[1]]/BrandedFares/FareFamily">
					<!--Brand Information from Request Object-->
					<xsl:variable name="bn" select="//StoredFare[PassengerType/@Code=$ptc[1]]/BrandedFares/FareFamily" />
					<xsl:for-each select="../air:AirSegment">
						<xsl:variable name="pos" select="position()" />
						<air:AirSegmentPricingModifiers>
							<xsl:attribute name="AirSegmentRef">
								<xsl:value-of select="@Key"/>
							</xsl:attribute>
							<xsl:attribute name="BrandTier">
								<xsl:value-of select="$bn[@RPH=$pos]/@Code"/>
							</xsl:attribute>
							<!--
							<xsl:if test="//StoredFare[PassengerType/@Code=$ptc[1]]/FareSegments">
								<xsl:attribute name="FareBasisCode">
									<xsl:value-of select="//StoredFare[PassengerType/@Code=$ptc[1]]/FareSegments/AirSegments[@RPH=$pos]"/>
								</xsl:attribute>
							</xsl:if>
							-->
							<air:PermittedBookingCodes>
								<air:BookingCode>
									<xsl:attribute name="Code">
										<xsl:value-of select="@ClassOfService"/>
									</xsl:attribute>
								</air:BookingCode>
							</air:PermittedBookingCodes>

						</air:AirSegmentPricingModifiers>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="air:FareInfo/air:Brand">
					<!--Brand Information from PNR Object-->
					<xsl:variable name="bn" select="air:FareInfo/air:Brand" />
					<xsl:for-each select="../air:AirSegment">
						<xsl:variable name="segkey" select="@Key"/>
						<xsl:variable name="key" select="../air:AirPricingInfo/air:BookingInfo[@SegmentRef=$segkey]/@FareInfoRef"/>
						<air:AirSegmentPricingModifiers>
							<xsl:attribute name="AirSegmentRef">
								<xsl:value-of select="@Key"/>
							</xsl:attribute>
							<xsl:if test="$bn[@Key=$key]/@BrandTier!=''">
								<xsl:attribute name="BrandTier">
									<xsl:value-of select="$bn[@Key=$key]/@BrandTier"/>
								</xsl:attribute>
							</xsl:if>
							<air:PermittedBookingCodes>
								<air:BookingCode>
									<xsl:attribute name="Code">
										<xsl:value-of select="@ClassOfService"/>
									</xsl:attribute>
								</air:BookingCode>
							</air:PermittedBookingCodes>
						</air:AirSegmentPricingModifiers>
					</xsl:for-each>
				</xsl:when>
			</xsl:choose>
		</air:AirPricingCommand>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="brandFareStore">
		<xsl:param name="delete" />
		<xsl:variable name="pos" select="position()"/>
		<xsl:variable name="pnr" select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord" />
		<xsl:variable name="ptc" select="air:PassengerType/@Code" />

		<xsl:variable name="ptcMod">
			<xsl:choose>
				<xsl:when test="$ptc='JNN'">
					<xsl:value-of select="//StoredFare[substring(PassengerType/@Code, 1,1) = substring($ptc,1,1) and not(PassengerType/@Code='JCB')]/PassengerType/@Code"/>
				</xsl:when>
				<xsl:when test="$ptc='CNN'">
					<xsl:value-of select="//StoredFare[substring(PassengerType/@Code, 1,1) = substring($ptc,1,1)]/PassengerType/@Code"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="//StoredFare[PassengerType/@Code = $ptc]/PassengerType/@Code"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="storedFare" select="//StoredFare[PassengerType/@Code = $ptcMod]" />

		<xsl:variable name="comCount" select="count($storedFare[Markup])"/>
		<xsl:variable name="group">
			<xsl:choose>
				<xsl:when test="$comCount > 1 and $storedFare/Markup">
					<xsl:choose>
						<xsl:when test="not($storedFare[1]/Markup/@Amount = $storedFare[$comCount]/Markup/@Amount)" >
							<!--<xsl:value-of select="format-number($pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup,'#0') * format-number($pos,'#0')"/>-->
							<xsl:value-of select="format-number($pos,'#0')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$storedFare/Markup">
					<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="$storedFare[1]/Markup/@Amount = $storedFare[$comCount]/Markup/@Amount" >
							<xsl:value-of select="format-number($pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup,'#0') + 1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="format-number($pos,'#0')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="brandTier" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1) = substring($ptc,1,1)]/air:FareInfo[1]/air:Brand/@BrandTier" />
		<xsl:if test="air:FareInfo/air:Brand[@BrandTier = $brandTier]">
			<air:AirPricingInfo>
				<xsl:attribute name="Key">
					<xsl:value-of select="@Key"/>
				</xsl:attribute>
				<xsl:attribute name="TotalPrice">
					<xsl:value-of select="@TotalPrice"/>
				</xsl:attribute>
				<xsl:attribute name="BasePrice">
					<xsl:value-of select="@BasePrice"/>
				</xsl:attribute>
				<xsl:attribute name="ApproximateTotalPrice">
					<xsl:value-of select="@ApproximateTotalPrice"/>
				</xsl:attribute>
				<xsl:attribute name="ApproximateBasePrice">
					<xsl:value-of select="@ApproximateBasePrice"/>
				</xsl:attribute>
				<xsl:attribute name="Taxes">
					<xsl:value-of select="@Taxes"/>
				</xsl:attribute>
				<xsl:attribute name="LatestTicketingTime">
					<xsl:value-of select="@LatestTicketingTime"/>
				</xsl:attribute>
				<xsl:attribute name="PricingMethod">
					<xsl:value-of select="@PricingMethod"/>
				</xsl:attribute>
				<xsl:attribute name="ProviderCode">
					<xsl:value-of select="@ProviderCode"/>
				</xsl:attribute>
				<xsl:attribute name="ProviderReservationInfoRef">
					<xsl:value-of select="$pnr/air:AirReservation/common_v50_0:SupplierLocator/@ProviderReservationInfoRef"/>
				</xsl:attribute>
				<xsl:attribute name="AirPricingInfoGroup">
					<xsl:value-of select="$group"/>
				</xsl:attribute>
				<xsl:for-each select="air:FareInfo">

					<air:FareInfo xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0">
						<xsl:attribute name="Key">
							<xsl:value-of select="@Key"/>
						</xsl:attribute>
						<xsl:attribute name="FareBasis">
							<xsl:value-of select="@FareBasis"/>
						</xsl:attribute>
						<xsl:attribute name="PassengerTypeCode">
							<xsl:value-of select="@PassengerTypeCode"/>
						</xsl:attribute>
						<xsl:attribute name="Origin">
							<xsl:value-of select="@Origin"/>
						</xsl:attribute>
						<xsl:attribute name="Destination">
							<xsl:value-of select="@Destination"/>
						</xsl:attribute>
						<xsl:attribute name="EffectiveDate">
							<xsl:value-of select="@EffectiveDate"/>
						</xsl:attribute>
						<xsl:attribute name="DepartureDate">
							<xsl:value-of select="@DepartureDate"/>
						</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="@Amount"/>
						</xsl:attribute>
						<xsl:attribute name="NegotiatedFare">
							<xsl:choose>
								<xsl:when test="$storedFare/@FareType = 'Private'">
									<xsl:text>true</xsl:text>
								</xsl:when>
								<xsl:when test="$storedFare/@FareType = 'Published'">
									<xsl:text>false</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@NegotiatedFare"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:attribute>
						<xsl:if test="$storedFare/@FareType = 'Private'">
							<xsl:attribute name="PrivateFare">
								<xsl:text>PrivateFare</xsl:text>
							</xsl:attribute>
						</xsl:if>

						<!-- Possible different implementation
						<xsl:if test="//StoredFare[PassengerType/@Code=$ptc and Markup or TourCode or Endorsement]/FareSegments/AirSegments[$pos]/@TicketDesignator">
							<air:FareTicketDesignator>
								<xsl:value-of select="//StoredFare[PassengerType/@Code=$ptc and Markup or TourCode or Endorsement]/FareSegments/AirSegments[$pos]/@TicketDesignator"/>
							</air:FareTicketDesignator>
						</xsl:if>
						-->
						
						<air:FareRuleKey>
							<xsl:attribute name="FareInfoRef">
								<xsl:value-of select="air:FareRuleKey/@FareInfoRef"/>
							</xsl:attribute>
							<xsl:attribute name="ProviderCode">
								<xsl:value-of select="air:FareRuleKey/@ProviderCode"/>
							</xsl:attribute>
							<xsl:value-of select="air:FareRuleKey"/>
						</air:FareRuleKey>
						<air:Brand>
							<xsl:attribute name="Key">
								<xsl:value-of select="air:Brand/@Key"/>
							</xsl:attribute>
							<xsl:attribute name="BrandID">
								<xsl:value-of select="air:Brand/@BrandID"/>
							</xsl:attribute>
							<xsl:attribute name="UpSellBrandID">
								<xsl:value-of select="air:Brand/@UpSellBrandID"/>
							</xsl:attribute>
							<xsl:attribute name="Name">
								<xsl:value-of select="air:Brand/@Name"/>
							</xsl:attribute>
							<xsl:attribute name="Carrier">
								<xsl:value-of select="air:Brand/@Carrier"/>
							</xsl:attribute>
							<xsl:attribute name="BrandTier">
								<xsl:value-of select="air:Brand/@BrandTier"/>
							</xsl:attribute>
						</air:Brand>
					</air:FareInfo>
				</xsl:for-each>
				<xsl:copy-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc]/air:BookingInfo"/>
				<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc]/air:PassengerType">
					<xsl:variable name="paxPos">
						<xsl:value-of select="position()"/>
					</xsl:variable>

					<xsl:variable name="age">
						<xsl:choose>
							<xsl:when test="@Age!=''">
								<xsl:value-of select="@Age"/>
							</xsl:when>
							<xsl:when test="@Code = 'CNN' or @Code = 'JNN'">
								<xsl:value-of select="number(substring($storedFare[position()=$paxPos]/PassengerType/@Code, 2, 2))"/>
							</xsl:when>
							<xsl:when test="substring(@Code, 1, 1) = 'C'">
								<xsl:value-of select="concat('0',number(substring(@Code, 2, 2)))"/>
							</xsl:when>
							<xsl:when test="substring(@Code, 1, 1) = 'J' and @Code != 'JCB'">
								<xsl:value-of select="concat('0',number(substring(@Code, 2, 2)))"/>
							</xsl:when>
							<xsl:when test="concat('0',number(substring(@Code, 2, 2))) = substring(@Code, 2, 2)">
								<xsl:value-of select="substring(@Code, 2, 2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text></xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<air:PassengerType>
						<xsl:attribute name="Code">
							<xsl:value-of select="@Code"/>
						</xsl:attribute>
						<xsl:if test="$age != ''">
							<xsl:attribute name="Age">
								<xsl:value-of select="$age"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="BookingTravelerRef">
							<xsl:value-of select="@BookingTravelerRef"/>
						</xsl:attribute>
					</air:PassengerType>
				</xsl:for-each>
				<xsl:variable name="priceType" select="@PricingMethod" />
				<xsl:if test="//StoredFare/Markup and //@StoreFare='true' and //StoredFare/@FareType='Private'">
					<air:AirPricingModifiers CurrencyType="USD" AccountCodeFaresOnly="false">
						<xsl:attribute name="FaresIndicator">
							<xsl:choose>
								<xsl:when test="$priceType='Guaranteed'">
									<xsl:text>PublicFares</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>PrivateFares</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:value-of select="@SegmentRef"/>
						</xsl:attribute>
						<xsl:apply-templates select="." mode="markup">
							<xsl:with-param name="mu" select="$storedFare/Markup"/>
						</xsl:apply-templates>
					</air:AirPricingModifiers>
				</xsl:if>
			</air:AirPricingInfo>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ModStoredFare">
		<xsl:param name="pnr" />
		<xsl:param name="airprice" />
		<xsl:variable name="priceType" select="@FareType" />
		<xsl:variable name="pricing">
			<xsl:choose>
				<xsl:when test="contains($priceType,'Published')">
					<xsl:text>Guaranteed</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>GuaranteedUsingAirlinePrivateFare</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="ptc">
			<xsl:choose>
				<xsl:when test="substring(air:PassengerType/@Code, 1,1) = 'C'">
					<xsl:variable name="pTemp" select="concat('C', substring(concat('0',air:PassengerType/@Age), string-length(air:PassengerType/@Age)))"/>
					<xsl:choose>
						<xsl:when test="//StoredFare[PassengerType/@Code=$pTemp]/Markup">
							<xsl:value-of select="$pTemp"/>
						</xsl:when>
						<xsl:when test="//StoredFare[PassengerType/@Code=translate($pTemp, '0', '')]/Markup">
							<xsl:value-of select="translate($pTemp, '0', '')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="air:PassengerType/@Code"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="air:PassengerType/@Code"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<air:AirPricingTicketingModifiers>
			<!--
				<xsl:for-each select="../StoredFare[Markup or TourCode or Endorsement]">
				<xsl:variable name="ptc">
					<xsl:choose>
						<xsl:when test="substring(air:PassengerType/@Code, 1,1) = 'C'">CNN</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="air:PassengerType/@Code"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				-->

			<air:AirPricingInfoRef>
				<xsl:attribute name="Key">
					<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo/@Key"/>
				</xsl:attribute>
			</air:AirPricingInfoRef>
			<!--</xsl:for-each>-->

			<air:TicketingModifiers FreeTicket="false">

				<!--<xsl:for-each select="../StoredFare[Markup]">-->
				<xsl:if test="//StoredFare[Discount or Markup]">
					<common_v50_0:Commission>
						<xsl:attribute name="Level">
							<xsl:text>Fare</xsl:text>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="//StoredFare[PassengerType/@Code=$ptc]/Markup/@Amount">

								<!--
								<xsl:attribute name="Modifier">
									<xsl:text>CommissionAmount</xsl:text>
								</xsl:attribute>
								-->

								<xsl:attribute name="Amount">
									<xsl:value-of select="//StoredFare[PassengerType/@Code=$ptc]/Markup/@Amount"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>Flat</xsl:text>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="//StoredFare[1]/Markup/@Percent">

								<xsl:attribute name="Modifier">
									<xsl:text>SupplementaryPercent</xsl:text>
								</xsl:attribute>

								<xsl:attribute name="Percentage">
									<xsl:value-of select="//StoredFare[PassengerType/@Code=$ptc]/Markup/@Percent"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>PercentTotal</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>

						<!--
							<xsl:attribute name="BookingTravelerRef">
								<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc]/air:PassengerType/@BookingTravelerRef"/>
							</xsl:attribute>
							-->

					</common_v50_0:Commission>
				</xsl:if>
				<!--</xsl:for-each>-->

				<xsl:if test="//StoredFare[PassengerType/@Code=$ptc and Markup or TourCode or Endorsement]/TourCode">
					<air:TourCode>
						<xsl:attribute name="Value">
							<xsl:value-of select="//StoredFare[PassengerType/@Code=$ptc and Markup or TourCode or Endorsement][1]/TourCode"/>
						</xsl:attribute>
					</air:TourCode>
				</xsl:if>
				<xsl:if test="//StoredFare[PassengerType/@Code=$ptc and Markup or TourCode or Endorsement]/Endorsement">
					<xsl:variable name="endors">
						<xsl:call-template name="tokenizeString">
							<xsl:with-param name="list" select="//StoredFare[Markup or TourCode or Endorsement][1]/Endorsement"/>
							<xsl:with-param name="delimiter" select="'/'"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:for-each select="msxsl:node-set($endors)/elem/node()[1]">
						<air:TicketEndorsement>
							<xsl:attribute name="Value">
								<xsl:value-of select="."/>
							</xsl:attribute>
						</air:TicketEndorsement>
					</xsl:for-each>
				</xsl:if>

				<air:DocumentSelect IssueTicketOnly="false">
					<air:Itinerary SeparateIndicator="false"/>
				</air:DocumentSelect>
				<air:DocumentOptions SuppressItineraryRemarks="false"/>
			</air:TicketingModifiers>
		</air:AirPricingTicketingModifiers>
	</xsl:template>

	<xsl:template name="private_ptc">
		<xsl:param name="optc"/>
		<xsl:choose>
			<!--
			<xsl:when test="$optc='JCB'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='ADT'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			-->
			<xsl:when test="$optc='WEB'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='ITX'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			<!--
			<xsl:when test="$optc='CHD'">
				<xsl:text>JWC</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='CNN'">
				<xsl:text>JWC</xsl:text>
			</xsl:when>
			-->
			<xsl:when test="$optc='WBC'">
				<xsl:text>JWC</xsl:text>
			</xsl:when>
			<!--
			<xsl:when test="$optc='JNN'">
				<xsl:text>JWB</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='INF'">
				<xsl:text>JWB</xsl:text>
			</xsl:when>
			-->
			<xsl:otherwise>
				<xsl:value-of select="$optc"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="markups">
		<xsl:param name="pnr" />

			<air:AirPricingModifiers>
				<xsl:for-each select="//StoredFare">
					<xsl:variable name="ptc" select="./PassengerType/@Code" />
					<!--<air:ManualFareAdjustment AppliedOn="Base" AdjustmentType="Amount" Value="+100.00" PassengerRef="bdJx9RDynDKAYThwYYAAAA==" />-->
					<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1)=substring($ptc,1,1)]" mode="markup">
						<xsl:with-param name="mu">
							<xsl:if test="Markup and @FareType='Private'">
								<xsl:value-of select="./Markup"/>
							</xsl:if>
						</xsl:with-param>
						
						<!--
						<xsl:with-param name="td">
							<xsl:if test="FareSegments/AirSegments/@TicketDesignator != ''">
								<xsl:value-of select="FareSegments/AirSegments[1]/@TicketDesignator"/>
							</xsl:if>
						</xsl:with-param>
						-->
					
					</xsl:apply-templates>
				</xsl:for-each>
			</air:AirPricingModifiers>

	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="markup">
		<xsl:param name="mu" select="''"/>
		<xsl:param name="td" select="''"/>
		<xsl:variable name="ptc" select="air:PassengerType/@Code" />
		<xsl:variable name="pax" select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/air:PassengerType/@BookingTravelerRef" />
		<xsl:for-each select="$pax">
			<xsl:variable name="pos" select="position()" />
			<xsl:variable name="key">
				<xsl:choose>
					<xsl:when test="air:PassengerType[position()=$pos]/@BookingTravelerRef">
						<xsl:value-of select="air:PassengerType/@BookingTravelerRef"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:if test ="$mu != '' and $mu/@Amount" >
				<air:ManualFareAdjustment>
					<xsl:attribute name="AppliedOn">
						<xsl:text>Base</xsl:text>
					</xsl:attribute>
					<xsl:choose>
						<xsl:when test="$mu/@Amount">
							<xsl:attribute name="AdjustmentType">
								<xsl:text>Amount</xsl:text>
							</xsl:attribute>
							<xsl:attribute name="Value">
								<xsl:value-of select="concat('+', format-number($mu/@Amount, '#0'))"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="AdjustmentType">
								<xsl:text>Percent</xsl:text>
							</xsl:attribute>
							<xsl:attribute name="Value">
								<xsl:value-of select="concat('+', $mu/@Percent)"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:attribute name="PassengerRef">
						<xsl:value-of select="$key"/>
					</xsl:attribute>
				</air:ManualFareAdjustment>
			</xsl:if>
			<!-- 
			<xsl:if test ="$td != ''" >
				<air:ManualFareAdjustment>
					<xsl:attribute name="AppliedOn">
						<xsl:text>Base</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="AdjustmentType">
						<xsl:text>Amount</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="TicketDesignator">
						<xsl:value-of select="$td"/>
					</xsl:attribute>
					<xsl:attribute name="PassengerRef">
						<xsl:value-of select="$key"/>
					</xsl:attribute>
				</air:ManualFareAdjustment>
			</xsl:if>
			-->
			
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="tokenizeString">
		<!--passed template parameter -->
		<xsl:param name="list"/>
		<xsl:param name="delimiter"/>
		<xsl:choose>
			<xsl:when test="contains($list, $delimiter)">
				<elem>
					<!-- get everything in front of the first delimiter -->
					<xsl:value-of select="substring-before($list,$delimiter)"/>
				</elem>
				<xsl:call-template name="tokenizeString">
					<!-- store anything left in another variable -->
					<xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
					<xsl:with-param name="delimiter" select="$delimiter"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$list = ''">
						<xsl:text/>
					</xsl:when>
					<xsl:otherwise>
						<elem>
							<xsl:value-of select="$list"/>
						</elem>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DistictList">
		<xsl:param name="list" />

		<xsl:variable name="lptc">
			<xsl:for-each select="$list">
				<xsl:if test="generate-id() = generate-id($list[.= current()][1])">
					<xsl:value-of select="."/>
					<xsl:value-of select="' '"/>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>

		<xsl:call-template name="tokenizeString">
			<xsl:with-param name="list" select="$lptc"/>
			<xsl:with-param name="delimiter" select="' '" />
		</xsl:call-template>

	</xsl:template>
</xsl:stylesheet>