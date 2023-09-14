<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:air="http://www.travelport.com/schema/air_v50_0"
				xmlns:universal="http://www.travelport.com/schema/universal_v50_0"
				xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- 
	================================================================== 
	Travelport_PNRRepriceRQ.xsl															
	================================================================== 	
	Date: 11 Sep 2023 - Kobelev - Corrected "ptmarkup" template to recieve AirPriceOnfo from PNR instead of NewPrice.
	Date: 08 Sep 2023 - Kobelev - Reprice PUB PNR
	Date: 06 Sep 2023 - Kobelev - Corrected Passanger @Key reference
	Date: 30 Aug 2023 - Kobelev - Reprice PUB vs. NEGO
	Date: 18 Jul 2023 - Samokhvalov - CHD Age Fixes
	Date: 30 Mar 2023 - Kobelev - Using of PlatingCarrier in AirPricingModifiers for Validating Carrier guarantee
	Date: 01 Mar 2023 - Kobelev - Using Brand in FareInfo
	Date: 27 Feb 2023 - Kobelev - Groupping by AirPricingInfoGroup and FarePriceGroup
	Date: 03 Feb 2023 - Kobelev - universal:AirAdd and universal:AirDelete Groupping by AirPricingInfoGroup 
	Date: 03 Feb 2023 - Kobelev - Corrected AirPricing Groupping via AirPricingInfoGroup fro PUB PNR with xcluded ptc
	Date: 02 Feb 2023 - Kobelev - Fixed Equipment display and Added InventoryRequestType in AirPricingModifiers
	Date: 26 Jan 2023 - Kobelev - Air Delete based on passed Stored Fares and not passanger types int he PNR
	Date: 17 Jan 2023 - Kobelev - For Published PNR - Commissions / Endoursments /TourCode
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

	<xsl:key name="storedFareSims" match="StoredFare" use="Markup"/>
	<xsl:key name="ptclist" match="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo/air:PassengerType/@Code" use="."/>
	<xsl:key name="priceGroups" match="StoredFare/@FarePriceGroup" use="."/>

	<xsl:template match="/">
		<xsl:apply-templates select="OTA_PNRRepriceRQ"/>
	</xsl:template>

	<!-- 
	================================================================== 
	First Step. Get PNR														
	================================================================== 
    -->
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
					<xsl:call-template name="EmulateOffice" />
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

	<!-- 
	================================================================== 
	Second Step. Get Current Price														
	================================================================== 
    -->
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
			<xsl:choose>
				<xsl:when test="//StoredFare/Markup and //@StoreFare='true' and //StoredFare/@FareType='Private'" >
					<xsl:call-template name="markups">
						<xsl:with-param name="pnr" select="$PNR" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<air:AirPricingModifiers>
						<xsl:attribute name="FaresIndicator">
							<xsl:choose>
								<xsl:when test="$PNR/air:AirReservation/air:AirPricingInfo/@PricingMethod='Guaranteed'">
									<xsl:text>PublicFaresOnly</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>PrivateFaresOnly</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</air:AirPricingModifiers>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="passanger_air">
				<xsl:with-param name="action" select="'price'" />
				<xsl:with-param name="pnr" select="$PNR" />
			</xsl:call-template>
			<xsl:call-template name="EmulateOfficePrice" />
		</air:AirPriceReq>
	</xsl:template>

	<!-- 
	================================================================== 
	Third Step. Set Price														
	================================================================== 
    -->
	<xsl:template match="NewPrice">
		<xsl:variable name="PNR">
			<xsl:choose>
				<xsl:when test="//Response/universal:UniversalRecordRetrieveRsp">
					<xsl:copy-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/node()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="//Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/node()"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="priceType" select="msxsl:node-set($PNR)/air:AirReservation/air:AirPricingInfo[1]/@PricingMethod"/>
		<xsl:variable name="fareBasis" select="concat('01',msxsl:node-set($PNR)/air:AirReservation/air:AirPricingInfo/air:FareInfo[1]/@FareBasis)" />
		<xsl:variable name="Price">
			<xsl:choose>
				<xsl:when test="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution[contains(common_v50_0:HostToken, $fareBasis)]/air:AirPricingInfo">
					<xsl:copy-of select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution[contains(common_v50_0:HostToken, $fareBasis)]/air:AirPricingInfo"/>
				</xsl:when>
				<xsl:when test="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution/air:AirPricingInfo">
					<xsl:copy-of select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution/air:AirPricingInfo"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
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
				<xsl:choose>
					<xsl:when test="//Response/universal:UniversalRecordRetrieveRsp">
						<xsl:value-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/@Version" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="//Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/@Version"/>
					</xsl:otherwise>
				</xsl:choose>

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
			<xsl:call-template name="EmulateOffice" />
			<common_v50_0:ContinuityCheckOverride>Yes</common_v50_0:ContinuityCheckOverride>
			<xsl:call-template name="passanger_air">
				<xsl:with-param name="action" select="'new'" />
				<xsl:with-param name="pnr" select="msxsl:node-set($PNR)" />
				<xsl:with-param name="airprice" select="msxsl:node-set($Price)[1]/node()" />
				<xsl:with-param name="univers">
					<xsl:choose>
						<xsl:when test="//Response/universal:UniversalRecordRetrieveRsp">
							<xsl:value-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/@LocatorCode"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="//Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/@LocatorCode"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
			<common_v50_0:FileFinishingInfo/>

		</universal:UniversalRecordModifyReq>
	</xsl:template>

	<!-- 
	================================================================== 
	Fourth Step. Update Price with Commissions, TourCode and Endoursments
	================================================================== 
    -->
	<xsl:template match="UpdatePrice">
		<xsl:param name="version" />
		<xsl:variable name="PNR">
			<xsl:choose>
				<xsl:when test="//Response/universal:UniversalRecordRetrieveRsp">
					<xsl:copy-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/node()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="//Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/node()"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="fareBasis" select="concat('01',msxsl:node-set($PNR)/air:AirReservation/air:AirPricingInfo/air:FareInfo[1]/@FareBasis)" />
		<xsl:variable name="Price">
			<xsl:choose>
				<xsl:when test="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution[contains(common_v50_0:HostToken, $fareBasis)]/air:AirPricingInfo">
					<xsl:copy-of  select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution[contains(common_v50_0:HostToken, $fareBasis)]/air:AirPricingInfo"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of  select="air:AirPriceRsp/air:AirPriceResult/air:AirPricingSolution/air:AirPricingInfo"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
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
				<xsl:choose>
					<xsl:when test="$version">
						<xsl:value-of select="$version" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/@Version" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="TargetBranch">
				<xsl:value-of select="//POS/Source/RequestorID/@Instance" />
			</xsl:attribute>
			<xsl:attribute name="ReturnRecord">
				<xsl:text>true</xsl:text>
			</xsl:attribute>
			<xsl:if test="//ConversationID">
				<xsl:attribute name="SessionKey">
					<xsl:value-of select="//ConversationID" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="//UUID">
				<xsl:attribute name="TransactionId">
					<xsl:value-of select="//UUID" />
				</xsl:attribute>
			</xsl:if>

			<common_v50_0:BillingPointOfSaleInfo OriginApplication="UAPI"/>
			<xsl:call-template name="EmulateOffice" />
			<xsl:call-template name="mod_air">
				<xsl:with-param name="pnr" select="msxsl:node-set($PNR)" />
				<xsl:with-param name="airprice" select="$Price" />
				<xsl:with-param name="univers">
					<xsl:choose>
						<xsl:when test="//Response/universal:UniversalRecordRetrieveRsp">
							<xsl:value-of select="//Response/universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/@LocatorCode"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="//Response/universal:UniversalRecordModifyRsp/universal:UniversalRecord/@LocatorCode"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
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
				<xsl:choose>
					<xsl:when test="air:FlightDetails/@Equipment">
						<xsl:value-of select="air:FlightDetails/@Equipment"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@Equipment"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:attribute>
			<xsl:attribute name="ChangeOfPlane">
				<xsl:value-of select="@ChangeOfPlane"/>
			</xsl:attribute>
			<xsl:attribute name="OptionalServicesIndicator">
				<xsl:value-of select="@OptionalServicesIndicator"/>
			</xsl:attribute>
			<xsl:if test="@ParticipantLevel">
				<xsl:attribute name="ParticipantLevel">
					<xsl:value-of select="@ParticipantLevel"/>
				</xsl:attribute>
			</xsl:if>
		</air:AirSegment>
	</xsl:template>

	<xsl:template name="passanger_air">
		<xsl:param name="action" />
		<xsl:param name="pnr" select="universal:UniversalRecord" />
		<xsl:param name="airprice" />
		<xsl:param name="univers" />
		<xsl:variable name="priceType" select="//StoredFare[PassengerType/@Code]/@FareType" />
		<xsl:variable name="pcount" select="sum(//StoredFare/PassengerType/@Quantity)" />
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
					<xsl:value-of select="$univers"/>
				</xsl:attribute>
			</universal:RecordIdentifier>
		</xsl:if>
		<xsl:variable name="changePTC">
			<xsl:call-template name="getChangedPTC">
				<xsl:with-param name="pnr" select="$pnr"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="class" select="$pnr/air:AirReservation/air:AirPricingInfo[1]/air:BookingInfo" />
		<xsl:variable name="td" select="$pnr/air:AirReservation/air:AirPricingInfo[1]/air:FareInfo/air:FareTicketDesignator/@Value" />
		<xsl:variable name="brnds" >
			<xsl:call-template name="DistictList">
				<xsl:with-param name="list" select="//StoredFare/BrandedFares/FareFamily/@Code"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="sfGroups" >
			<xsl:call-template name="DistictList">
				<xsl:with-param name="list" select="//StoredFare/@FarePriceGroup"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="pnrGroups" >
			<xsl:call-template name="DistictList">
				<xsl:with-param name="list" select="$pnr/air:AirReservation/air:AirPricingInfo/@AirPricingInfoGroup"/>
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
			<xsl:when test="//StoredFare[PassengerType/@Code]/BrandedFares">
				<xsl:choose>
					<xsl:when test="$action='new'">
						<xsl:for-each select="//StoredFare">
							<xsl:variable name="optc" select="PassengerType/@Code" />
							<xsl:variable name="price" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1)=substring($optc, 1,1)]" />
							<xsl:variable name="group" select="$price/@AirPricingInfoGroup" />
							<xsl:variable name="sibgroup">
								<xsl:choose>
									<xsl:when test="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup">
										<xsl:value-of select="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$group"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="pos" select="position()"/>
							<xsl:variable name="ptc" select="$price/air:PassengerType/@Code" />
							<xsl:choose>
								<!-- According to Travelport you only need to use one AirpriceInfo Key if all of them sharing a group  and $pos=1-->
								<xsl:when test="$group = $sibgroup and $pos=1">
									<universal:UniversalModifyCmd>
										<xsl:attribute name="Key">
											<xsl:value-of select="$pos"/>
										</xsl:attribute>
										<universal:AirDelete Element="AirPricingInfo">
											<xsl:attribute name="Key">
												<xsl:value-of select="$price/@Key"/>
											</xsl:attribute>
											<xsl:attribute name="ReservationLocatorCode">
												<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
											</xsl:attribute>
										</universal:AirDelete>
									</universal:UniversalModifyCmd>
								</xsl:when>
								<xsl:when test="$group != $sibgroup">
									<universal:UniversalModifyCmd>
										<xsl:attribute name="Key">
											<xsl:value-of select="@FarePriceGroup"/>
										</xsl:attribute>
										<universal:AirDelete Element="AirPricingInfo">
											<xsl:attribute name="Key">
												<xsl:value-of select="$price/@Key"/>
											</xsl:attribute>
											<xsl:attribute name="ReservationLocatorCode">
												<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
											</xsl:attribute>
										</universal:AirDelete>
									</universal:UniversalModifyCmd>
								</xsl:when>
							</xsl:choose>
						</xsl:for-each>
						<xsl:variable name="priceList" select="//StoredFare" />
						<xsl:for-each select="msxsl:node-set($sfGroups)/elem/node()[1]">
							<xsl:variable name="g" select="." />
							<universal:UniversalModifyCmd>
								<xsl:attribute name="Key">
									<xsl:value-of select="number(msxsl:node-set($pnrGroups)/elem[last()]/node()[1]) + position()"/>
								</xsl:attribute>
								<xsl:variable name="resCode" select="$pnr/air:AirReservation/@LocatorCode"/>
								<universal:AirAdd>
									<xsl:attribute name="ReservationLocatorCode">
										<xsl:value-of select="$resCode"/>
									</xsl:attribute>
									<xsl:for-each select="$priceList[@FarePriceGroup=$g]">
										<xsl:variable name="p" select="PassengerType/@Code" />
										<xsl:variable name="ptc">
											<xsl:call-template name="price_ptc" >
												<xsl:with-param name="sptc" select="$p"/>
											</xsl:call-template>
										</xsl:variable>
										<xsl:if test="$pnr/air:AirReservation/air:AirPricingInfo/air:FareInfo[@PassengerTypeCode=$ptc]">
											<xsl:variable name="bcode" select="BrandedFares/FareFamily/@Code" />

											<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc]" mode="brandFareStore">
												<xsl:with-param name="pcount" select="$pcount" />
												<xsl:with-param name="airprice" select="$airprice[air:PassengerType/@Code=$ptc and number(air:FareInfo/air:Brand/@BrandTier) = number($bcode)]" />
												<xsl:with-param name="delete" select="'true'" />
												<xsl:with-param name="pnr" select="$pnr" />
												<xsl:with-param name="storedFare" select="." />
											</xsl:apply-templates>

										</xsl:if>
									</xsl:for-each>

								</universal:AirAdd>
							</universal:UniversalModifyCmd>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[@PricingType='TicketRecord'][1]" mode="brandFare">
							<xsl:with-param name="ptc" select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[1]/@Code" />
						</xsl:apply-templates>
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
									<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[$index]" mode="brandFareStore">
										<xsl:with-param name="pcount" select="$pcount" />
										<xsl:with-param name="airprice" select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" />
										<xsl:with-param name="delete" select="'true'" />
										<xsl:with-param name="pnr" select="$pnr" />
										<xsl:with-param name="storedFare" select="//StoredFare[PassengerType/@Code= $p]" />
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
									<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[$index]" mode="brandFareStore">
										<xsl:with-param name="pcount" select="$pcount" />
										<xsl:with-param name="airprice" select="$airprice[position() = $index]/air:AirPricingInfo[air:PassengerType/@Code=$p][1]" />
										<xsl:with-param name="delete" select="'true'" />
										<xsl:with-param name="pnr" select="$pnr" />
										<xsl:with-param name="storedFare" select="//StoredFare[PassengerType/@Code= $p]" />
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
			<xsl:otherwise>
				<xsl:if test="$airprice!=''">
					<xsl:for-each select="//StoredFare">
						<xsl:variable name="optc" select="PassengerType/@Code" />
						<xsl:variable name="price" select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$optc]" />
						<xsl:variable name="group" select="$price/@AirPricingInfoGroup" />
						<xsl:variable name="sibgroup">
							<xsl:choose>
								<xsl:when test="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup">
									<xsl:value-of select="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$group"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="pos" select="position()"/>
						<xsl:variable name="ptc" select="$price/air:PassengerType/@Code" />
						<xsl:variable name="pKey" select="$price/@Key" />
						<xsl:choose>
							<!-- According to Travelport you only need to use one AirpriceInfo Key if all of them sharing a group  and $pos=1-->
							<xsl:when test="($group = $sibgroup or contains($group,$sibgroup)) 
						    and (count(msxsl:node-set($pnrGroups)[1]/node()) = count(msxsl:node-set($sfGroups)[1]/node()))
						    and $pos=1">
								<xsl:if  test="$pKey!=''">
									<universal:UniversalModifyCmd>
										<xsl:attribute name="Key">
											<xsl:value-of select="$pos"/>
										</xsl:attribute>
										<universal:AirDelete Element="AirPricingInfo">
											<xsl:attribute name="Key">
												<xsl:value-of select="$pKey"/>
											</xsl:attribute>
											<xsl:attribute name="ReservationLocatorCode">
												<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
											</xsl:attribute>
										</universal:AirDelete>
									</universal:UniversalModifyCmd>
								</xsl:if>
							</xsl:when>
							<xsl:when test="$group != $sibgroup">
								<universal:UniversalModifyCmd>
									<xsl:attribute name="Key">
										<xsl:value-of select="$pos"/>
									</xsl:attribute>
									<universal:AirDelete Element="AirPricingInfo">
										<xsl:attribute name="Key">
											<xsl:value-of select="$pKey"/>
										</xsl:attribute>
										<xsl:attribute name="ReservationLocatorCode">
											<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
										</xsl:attribute>
									</universal:AirDelete>
								</universal:UniversalModifyCmd>
							</xsl:when>
							<xsl:otherwise>
								<xsl:if test="$pos=1">
									<xsl:for-each select="msxsl:node-set($pnrGroups)[1]/node()">
										<xsl:variable name="lgroup" select="node()[1]" />
										<xsl:variable name="aprice" select="$pnr/air:AirReservation/air:AirPricingInfo[@AirPricingInfoGroup = $lgroup][1]" />
										<xsl:variable name="lpos" select="position()"/>
										<universal:UniversalModifyCmd>
											<xsl:attribute name="Key">
												<xsl:value-of select="$lpos"/>
											</xsl:attribute>
											<universal:AirDelete Element="AirPricingInfo">
												<xsl:attribute name="Key">
													<xsl:value-of select="$aprice/@Key"/>
												</xsl:attribute>
												<xsl:attribute name="ReservationLocatorCode">
													<xsl:value-of select="$aprice[1]/../../air:AirReservation/@LocatorCode"/>
												</xsl:attribute>
											</universal:AirDelete>
										</universal:UniversalModifyCmd>
									</xsl:for-each>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
					<xsl:variable name="keyElem" select="number(msxsl:node-set($sfGroups)[1]/elem[last()]/node()[1])" />
					<xsl:variable name="priceList" select="//StoredFare" />
					<xsl:for-each select="msxsl:node-set($sfGroups)/elem/node()[1]">
						<xsl:variable name="g" select="." />
						<universal:UniversalModifyCmd>
							<xsl:attribute name="Key">
								<xsl:value-of select="$keyElem + position()"/>
							</xsl:attribute>
							<xsl:variable name="resCode" select="$pnr/air:AirReservation/@LocatorCode"/>
							<universal:AirAdd>
								<xsl:attribute name="ReservationLocatorCode">
									<xsl:value-of select="$resCode"/>
								</xsl:attribute>
								<xsl:choose>
									<xsl:when test="$priceList[@FarePriceGroup=$g]/@FareType='Private'">
										<xsl:for-each select="$priceList[@FarePriceGroup=$g]">
											<xsl:variable name="p" >
												<xsl:apply-templates select="." mode="general">
													<xsl:with-param name="pnrAirRes" select="$pnr/air:AirReservation" />
												</xsl:apply-templates>
												<!--
												<xsl:choose>
													<xsl:when test="string(number(substring(PassengerType/@Code, 2,2))) != 'NaN' ">
														<xsl:choose>
															<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=JNN]">
																<xsl:text>JNN</xsl:text>		
															</xsl:when>
															<xsl:otherwise>
																<xsl:text>CNN</xsl:text>
															</xsl:otherwise>
														</xsl:choose>														
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="PassengerType/@Code"/>
													</xsl:otherwise>
												</xsl:choose>
												-->
											</xsl:variable>
											<xsl:variable name="tprice" select="number(substring($pnr/air:AirReservation/air:AirPricingInfo[air:FareInfo/@PassengerTypeCode = $p]/@TotalPrice,4))" />
											<xsl:variable name="muPrice" select="$tprice + number(Markup/@Amount)" />
											<xsl:choose>
												<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo/air:FareInfo[@PassengerTypeCode = $p]">
													<!--<xsl:apply-templates select="$airprice[air:FareInfo/@PassengerTypeCode = $p]" mode="brandFareStore">-->
													<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$p]" mode="brandFareStore">
														<xsl:with-param name="pcount" select="$pcount" />
														<xsl:with-param name="airprice" select="$airprice[air:PassengerType/@Code=$p]" />
														<xsl:with-param name="delete" select="'true'" />
														<xsl:with-param name="pnr" select="$pnr" />
														<xsl:with-param name="storedFare" select="." />
													</xsl:apply-templates>
												</xsl:when>
												<xsl:otherwise>
													<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[air:FareInfo/air:PassengerType/@Code=$p]" mode="brandFareStore">
														<xsl:with-param name="pcount" select="$pcount" />
														<xsl:with-param name="airprice" select="$airprice[substring(air:FareInfo/@PassengerTypeCode,1,1) = substring($p,1,1)]" />
														<xsl:with-param name="delete" select="'true'" />
														<xsl:with-param name="pnr" select="$pnr" />
														<xsl:with-param name="storedFare" select="." />
													</xsl:apply-templates>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="$priceList[@FarePriceGroup=$g]">
											<xsl:variable name="p" >
												<xsl:apply-templates select="." mode="general">
													<xsl:with-param name="pnrAirRes" select="$pnr/air:AirReservation" />
												</xsl:apply-templates>												
											</xsl:variable>
											<xsl:variable name="tprice" select="number(substring($pnr/air:AirReservation/air:AirPricingInfo[air:FareInfo/@PassengerTypeCode = $p]/@TotalPrice,4))" />
											<xsl:variable name="muPrice" select="$tprice + number(Markup/@Amount)" />
											<xsl:if test="$pnr/air:AirReservation/air:AirPricingInfo[air:FareInfo/@PassengerTypeCode = $p]">
												<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[air:FareInfo/@PassengerTypeCode = $p]" mode="brandFareStore">
													<xsl:with-param name="pcount" select="$pcount" />
													<xsl:with-param name="airprice" select="$airprice[air:FareInfo/@PassengerTypeCode = $p]" />
													<xsl:with-param name="delete" select="'true'" />
													<xsl:with-param name="pnr" select="$pnr" />
													<xsl:with-param name="storedFare" select="." />
												</xsl:apply-templates>
											</xsl:if>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>

							</universal:AirAdd>
						</universal:UniversalModifyCmd>
					</xsl:for-each>
					<xsl:if test="count(msxsl:node-set($changePTC)/elem) > 0 and (count(//StoredFare/PassengerType/@Code) > count(msxsl:node-set($changePTC)/elem))">
						<xsl:variable name="sibgroup">
							<xsl:choose>
								<xsl:when test="$pnr/air:AirReservation/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup">
									<xsl:value-of select="$pnr/air:AirReservation/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$sfGroups"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:if test="count(msxsl:node-set($sfGroups)/elem/node()[1]) = 1">
							<universal:UniversalModifyCmd>
								<xsl:attribute name="Key">
									<xsl:choose>
										<xsl:when test="msxsl:node-set($changePTC)/elem">
											<xsl:value-of select="count($airprice) + 1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="count(msxsl:node-set($changePTC)/elem) + 1"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:variable name="resCode" select="$pnr/air:AirReservation/@LocatorCode"/>
								<universal:AirAdd>
									<xsl:attribute name="ReservationLocatorCode">
										<xsl:value-of select="$resCode"/>
									</xsl:attribute>
									<xsl:for-each select="$airprice">
										<xsl:variable name="p" >
											<xsl:apply-templates select="air:AirPriceResult/air:AirPricingSolution/air:AirPricingInfo" mode="reverse">
												<xsl:with-param name="pnrAirRes" select="$pnr/air:AirReservation" />
											</xsl:apply-templates>
										</xsl:variable>
										<xsl:if test="not(msxsl:node-set($changePTC)/elem[contains(node(), $p)])">
											<xsl:apply-templates select="." mode="brandFareStore">
												<xsl:with-param name="pcount" select="$pcount" />
												<xsl:with-param name="delete" select="'true'" />
												<xsl:with-param name="pnr" select="$pnr" />
												<xsl:with-param name="storedFare" select="//StoredFare[PassengerType/@Code= $p]" />
											</xsl:apply-templates>
										</xsl:if>
									</xsl:for-each>
								</universal:AirAdd>
							</universal:UniversalModifyCmd>
						</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="StoredFare" mode="general">
		<xsl:param name="pnrAirRes" />
		<xsl:choose>
			<xsl:when test="string(number(substring(PassengerType/@Code, 2,2))) != 'NaN' ">
				<xsl:choose>
					<xsl:when test="$pnrAirRes/air:AirPricingInfo[air:PassengerType/@Code='JNN']">
						<xsl:text>JNN</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>CNN</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="PassengerType/@Code"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="reverse">
		<xsl:param name="pnrAirRes" />
		<xsl:choose>
			<xsl:when test="substring(air:PassengerType/@Code, 2,2) = 'NN' ">
				<xsl:variable name="age" select="$pnrAirRes/air:AirPricingInfo[air:PassengerType/@Code=air:PassengerType/@Code]/air:PassengerType/@Age" />
				<xsl:value-of select="concat(substring(air:PassengerType/@Code, 1,1),$age)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="air:PassengerType/@Code"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
				  

	<xsl:template name="airPriceFilter">
		<xsl:param name="airprice" />
		<xsl:param name="class" />
		<xsl:param name="td" />
		<xsl:param name="brnds" />

		<xsl:variable name="brand" select="msxsl:node-set($brnds)/elem/node()[1]" />
		<xsl:variable name="fbrand" select="format-number($brand, '0000')" />

		<xsl:variable name="pos">
			<xsl:choose>
				<xsl:when test="count($airprice[air:FareInfo/air:Brand/@BrandTier = $fbrand]) = 1">
					<xsl:value-of select="1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$airprice[air:FareInfo/air:Brand/@BrandTier = $fbrand]">
						<xsl:variable name="_pos" select="position()" />
						<xsl:variable name="_xClass" select="air:BookingInfo/@BookingCode[not(.=$class/@BookingCode)]" />
						<xsl:variable name="_xTD" select="air:FareInfo/air:FareTicketDesignator/@Value[not(.=$td)]" />
						<xsl:if test="not(air:BookingInfo/@BookingCode[not(.=$class/@BookingCode)]) and not(air:FareInfo/air:FareTicketDesignator/@Value[not(.=$td)])">
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
		<xsl:param name="univers" />

		<universal:RecordIdentifier>
			<xsl:attribute name="ProviderCode">
				<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@ProviderCode"/>
			</xsl:attribute>
			<xsl:attribute name="ProviderLocatorCode">
				<xsl:value-of select="$pnr/universal:ProviderReservationInfo/@LocatorCode"/>
			</xsl:attribute>
			<xsl:attribute name="UniversalLocatorCode">
				<xsl:value-of select="$univers"/>
			</xsl:attribute>
		</universal:RecordIdentifier>

		<xsl:variable name="changed" select="//StoredFare" />

		<xsl:variable name="changePTC">
			<xsl:call-template name="getChangedPTC">
				<xsl:with-param name="pnr" select="$pnr"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="groups" >
			<xsl:call-template name="DistictList">
				<xsl:with-param name="list" select="//StoredFare/@FarePriceGroup"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="keyElem" select="count(//StoredFare[Markup or TourCode or Endorsement])" />
		<xsl:variable name="priceList" select="//StoredFare" />

		<xsl:for-each select="msxsl:node-set($groups)/elem/node()[1]">
			<xsl:variable name="g" select="." />
			<xsl:variable name="p" select="$priceList[@FarePriceGroup=$g]/PassengerType/@Code" />
			
			<xsl:variable name="ptc">
				<xsl:call-template name="price_ptc" >
					<xsl:with-param name="sptc" select="$p"/>
				</xsl:call-template>
			</xsl:variable>

			<!--<xsl:if test="msxsl:node-set($airprice)[1]/air:AirPricingInfo[air:PassengerType/@Code = msxsl:node-set($changePTC)/elem[1][substring(.,1,1) = substring($ptc,1,1)]]">-->
			<xsl:if test="msxsl:node-set($airprice)[1]/air:AirPricingInfo[air:PassengerType/@Code = $ptc]">
				<universal:UniversalModifyCmd>
					<xsl:attribute name="Key">
						<xsl:value-of select="position() + 1"/>
					</xsl:attribute>
					<universal:AirAdd>
						<xsl:attribute name="ReservationLocatorCode">
							<xsl:value-of select="$pnr/air:AirReservation/@LocatorCode"/>
						</xsl:attribute>
						<xsl:call-template name="ModStoredFareJoined" >
							<xsl:with-param name="pnr" select="$pnr" />
							<xsl:with-param name="fares" select="$priceList[@FarePriceGroup=$g]" />
							<xsl:with-param name="airprice" select="msxsl:node-set($airprice)[1]/air:AirPricingInfo[air:PassengerType/@Code = $ptc]" />
						</xsl:call-template>
					</universal:AirAdd>
				</universal:UniversalModifyCmd>
			</xsl:if>
		</xsl:for-each>

	</xsl:template>

	<xsl:template match="common_v50_0:BookingTraveler">
		<xsl:variable name="key" select="@Key" />
		<xsl:variable name="optc" select="../air:AirReservation/air:AirPricingInfo/air:PassengerType[@BookingTravelerRef=$key]/@Code" />
		<xsl:variable name="sfptc">
			<xsl:choose>
				<xsl:when test="//StoredFare[PassengerType/@Code = $optc]/PassengerType/@Code">
					<xsl:value-of select="//StoredFare[PassengerType/@Code = $optc]/PassengerType/@Code"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="//StoredFare[substring(PassengerType/@Code, 1, 1) = substring($optc,1,1) ]/PassengerType/@Code"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="ptc">
			<xsl:call-template name="private_ptc" >
				<xsl:with-param name="optc" select="$optc"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="age">
			<xsl:choose>
				<xsl:when test="//air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]/air:PassengerType/@Age!=''">
					<xsl:value-of select="format-number(//air:AirReservation/air:AirPricingInfo[air:PassengerType/@BookingTravelerRef=$key]/air:PassengerType/@Age,'000')"/>
				</xsl:when>
				<xsl:when test="substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1],1,3)='P-C'">
					<xsl:value-of select="format-number(substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1], 4, 2),'000')"/>
				</xsl:when>

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
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="brandFare">
		<xsl:param name="ptc" />
		<air:AirPricingCommand>
			<xsl:choose>
				<xsl:when test="//StoredFare[PassengerType/@Code=$ptc[1]]/BrandedFares/FareFamily">
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
		<xsl:param name="airprice" select="."/>
		<xsl:param name="delete" />
		<xsl:param name="pnr" select="universal:UniversalRecord" />
		<xsl:param name="storedFare" select="//StoredFare[1]" />
		<xsl:param name="pcount" select="sum(//StoredFare/PassengerType/@Quantity)" />
		<xsl:variable name="pos" select="position()"/>
		<xsl:variable name="ptc" select="air:PassengerType/@Code" />
		<xsl:variable name ="ptcAge" select="air:PassengerType/@Age" />
		<xsl:variable name="comCount" select="count($storedFare[Markup])"/>
		<xsl:variable name="group">
			<xsl:choose>
				<xsl:when test="$comCount > 1 and $storedFare/Markup">
					<xsl:choose>
						<xsl:when test="not($storedFare[1]/Markup/@Amount = $storedFare[$comCount]/Markup/@Amount)" >
							<xsl:value-of select="format-number($pos,'#0')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="msxsl:node-set($pnr)/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$storedFare/Markup">
					<xsl:value-of select="msxsl:node-set($pnr)/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="$storedFare[1]/Markup/@Amount = $storedFare[$comCount]/Markup/@Amount" >
							<xsl:value-of select="format-number($pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup,'#0') + 1"/>
						</xsl:when>
						<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup">
							<xsl:value-of select="format-number($pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $ptc]/@AirPricingInfoGroup,'#0')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="format-number($pos,'#0')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--
		<xsl:variable name="brandTier" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1) = substring($ptc,1,1)]/air:FareInfo[position()=$pos]/air:Brand/@BrandTier" />
		-->
		<xsl:variable name="brandTier" select="air:FareInfo[position()=$pos]/air:Brand/@BrandTier" />

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
			<xsl:attribute name="ApproximateTaxes">
				<xsl:value-of select="@ApproximateTaxes"/>
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
			<xsl:for-each select="air:FareInfo[air:Brand/@BrandTier = $brandTier]">
				<xsl:variable name="from" select="@Origin"/>
				<xsl:variable name="to" select="@Destination"/>
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
						<xsl:choose>
							<xsl:when test="@DepartureDate">
								<xsl:value-of select="@DepartureDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$pnr/air:AirReservation/air:AirSegment[@Origin=$from and @Destination=$to]/@DepartureTime"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:if test="@Amount">
						<xsl:attribute name="Amount">
							<xsl:value-of select="@Amount"/>
						</xsl:attribute>
					</xsl:if>
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
					<air:FareRuleKey>
						<xsl:attribute name="FareInfoRef">
							<xsl:choose>
								<xsl:when test="air:FareRuleKey/@FareInfoRef">
									<xsl:value-of select="air:FareRuleKey/@FareInfoRef"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@Key"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="ProviderCode">
							<xsl:choose>
								<xsl:when test="air:FareRuleKey/@ProviderCode">
									<xsl:value-of select="air:FareRuleKey/@ProviderCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="../@ProviderCode"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:attribute>
						<xsl:value-of select="air:FareRuleKey"/>
					</air:FareRuleKey>
					<xsl:if test="air:Brand">
						<air:Brand>
							<xsl:attribute name="Key">
								<xsl:value-of select="air:Brand/@Key"/>
							</xsl:attribute>
							<xsl:attribute name="BrandID">
								<xsl:value-of select="air:Brand/@BrandID"/>
							</xsl:attribute>
							<xsl:if test="air:Brand/@UpSellBrandID" >
								<xsl:attribute name="UpSellBrandID">
									<xsl:value-of select="air:Brand/@UpSellBrandID"/>
								</xsl:attribute>
							</xsl:if>
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
					</xsl:if>
				</air:FareInfo>
			</xsl:for-each>
			<xsl:variable name="bookingTrvKey">
				<xsl:choose>
					<xsl:when test="$pnr/common_v50_0:BookingTraveler[substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1],1,3)='P-C' and
						format-number(substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1], 4, 2),'0')=$ptcAge]/@Key">
						<xsl:value-of select="$pnr/common_v50_0:BookingTraveler[substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1],1,3)='P-C' and
						format-number(substring(common_v50_0:NameRemark/common_v50_0:RemarkData/node()[1], 4, 2),'0')=$ptcAge]/@Key"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo/air:PassengerType[@Code = $ptc]/@BookingTravelerRef"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:copy-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc and 
			      (($bookingTrvKey!='' and air:PassengerType/@BookingTravelerRef=substring($bookingTrvKey,1,24)) or air:PassengerType/@Age=$ptcAge or $ptcAge='' or not($ptcAge))]/air:BookingInfo"/>
			<!--
			<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc and 
				     (($bookingTrvKey!='' and air:PassengerType/@BookingTravelerRef=substring($bookingTrvKey,1,24)) or air:PassengerType/@Age=$ptcAge or $ptcAge='' or not($ptcAge))]/air:PassengerType">		
			-->

			<xsl:variable name="paxPos">
				<xsl:value-of select="position()"/>
			</xsl:variable>
			<xsl:apply-templates select="." mode="ptmarkup">
				<xsl:with-param name="pcount" select="$pcount" />
				<xsl:with-param name="storedFare" select="$storedFare"/>
				<xsl:with-param name="airPrice" select="$airprice[air:PassengerType/@Code=$ptc]"/>
			</xsl:apply-templates>
			<xsl:variable name="priceType" select="@PricingMethod" />
			<air:AirPricingModifiers>
				<xsl:attribute name="FaresIndicator">
					<xsl:choose>
						<xsl:when test="$priceType='Guaranteed'">
							<xsl:text>PublicFaresOnly</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>PrivateFaresOnly</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="@SegmentRef"/>
				</xsl:attribute>
				<xsl:attribute name="PlatingCarrier" >
					<xsl:value-of select="$airprice[air:PassengerType/@Code=$ptc]/@PlatingCarrier"/>
				</xsl:attribute>
				<xsl:apply-templates select="." mode="markup">
					<xsl:with-param name="storedFare" select="$storedFare"/>
					<xsl:with-param name="pnrPrice" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1)=substring($ptc,1,1)]"/>
				</xsl:apply-templates>
			</air:AirPricingModifiers>
		</air:AirPricingInfo>
	</xsl:template>

	<xsl:template name="ModStoredFareJoined">
		<xsl:param name="pnr" />
		<xsl:param name="airprice" />
		<xsl:param name="fares" select="//StoredFare[Markup or TourCode or Endorsement]"/>

		<xsl:variable name="priceType" select="$fares[1]/@FareType" />
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
				<xsl:when test="string(number(substring(PassengerType/@Code, 2,2))) != 'NaN'">
					<!--test="substring(air:PassengerType/@Code, 1,1) = 'C'" -->
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

		<xsl:variable name="crr" select="$fares[1]" />

		<air:AirPricingTicketingModifiers>
			<xsl:for-each select="$fares">
				<xsl:variable name="pax" select="PassengerType/@Code" />

				<xsl:variable name="p">
					<xsl:call-template name="price_ptc" >
						<xsl:with-param name="sptc" select="$pax"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $p]/@Key">
					<!--<xsl:variable name="priceKey" select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code = $p]/@Key" />-->
					<air:AirPricingInfoRef>
						<xsl:attribute name="Key">
							<xsl:value-of select="."/>
							<!--<xsl:value-of select="$priceKey"/>-->
						</xsl:attribute>
					</air:AirPricingInfoRef>
				</xsl:for-each>
			</xsl:for-each>

			<air:TicketingModifiers FreeTicket="false">
				<!--
				<xsl:attribute name="BookingTravelerRef">
					<xsl:value-of select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$optc]/air:PassengerType/@BookingTravelerRef"/>
				</xsl:attribute>
				-->

				<xsl:if test="$crr/Discount or $crr/Markup">
					<common_v50_0:Commission>
						<xsl:attribute name="Level">
							<xsl:text>Fare</xsl:text>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="$crr/Markup/@Amount">
								<xsl:attribute name="Amount">
									<xsl:value-of select="$crr/Markup/@Amount"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>Flat</xsl:text>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="$crr/Markup/@Percent">

								<xsl:attribute name="Modifier">
									<xsl:text>SupplementaryPercent</xsl:text>
								</xsl:attribute>

								<xsl:attribute name="Percentage">
									<xsl:value-of select="$crr/Markup/@Percent"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>PercentTotal</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</common_v50_0:Commission>
				</xsl:if>

				<xsl:if test="$crr/TourCode">
					<air:TourCode>
						<xsl:attribute name="Value">
							<xsl:value-of select="$crr/TourCode"/>
						</xsl:attribute>
					</air:TourCode>
				</xsl:if>

				<xsl:if test="$crr/Endorsement">
					<xsl:variable name="endors">
						<xsl:call-template name="tokenizeString">
							<xsl:with-param name="list" select="$crr/Endorsement"/>
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
			<xsl:when test="$optc='WEB'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='ITX'">
				<xsl:text>JWA</xsl:text>
			</xsl:when>
			<xsl:when test="$optc='WBC'">
				<xsl:text>JWC</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$optc"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="price_ptc">
		<xsl:param name="sptc"/>
		<xsl:choose>
			<xsl:when test="substring($sptc,1,2)='J0' or substring($sptc,1,2)='J1'">
				<xsl:text>JNN</xsl:text>
			</xsl:when>
			<xsl:when test="substring($sptc,1,2)='C0' or substring($sptc,1,2)='C1'">
				<xsl:text>CNN</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$sptc"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="markups">
		<xsl:param name="pnr" />
		<air:AirPricingModifiers>
			<xsl:attribute name="FaresIndicator">
				<xsl:choose>
					<xsl:when test="$pnr/air:AirReservation/air:AirPricingInfo/@PricingMethod='Guaranteed'">
						<xsl:text>PublicFaresOnly</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>PrivateFaresOnly</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:value-of select="@SegmentRef"/>
			</xsl:attribute>
			<xsl:for-each select="//StoredFare">
				<xsl:variable name="sptc" select="PassengerType/@Code" />
				<xsl:variable name="ptc">
					<xsl:call-template name="price_ptc" >
						<xsl:with-param name="sptc" select="$sptc"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="Markup and @FareType='Private'">
					<xsl:apply-templates select="$pnr/air:AirReservation/air:AirPricingInfo[air:PassengerType/@Code=$ptc]" mode="markup">
						<xsl:with-param name="storedFare" select="." />
						<xsl:with-param name="pnrPrice" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1)=substring($ptc,1,1)]"/>
					</xsl:apply-templates>
				</xsl:if>
			</xsl:for-each>
		</air:AirPricingModifiers>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="markup">
		<xsl:param name="storedFare"/>
		<xsl:param name="td" select="''"/>
		<xsl:param name="pnrPrice" select="." />
		<xsl:variable name="ptc" select="air:PassengerType/@Code" />
		<xsl:variable name="pax" select="air:PassengerType[@Code = $ptc]/@BookingTravelerRef" />
		<xsl:variable name="mu" select="$storedFare/Markup" />
		<xsl:if test ="$mu/@Amount and $storedFare/@FareType='Private'" >
			<xsl:for-each select="$pax">
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
						<xsl:value-of select="."/>
					</xsl:attribute>
				</air:ManualFareAdjustment>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="ptmarkup">
		<xsl:param name="pcount" />
		<xsl:param name="storedFare"/>
		<xsl:param name="airPrice" select="." />
		<xsl:variable name="ptc" select="air:PassengerType/@Code" />
		<xsl:choose>
			<xsl:when test ="$pcount > 1 or $storedFare/BrandedFares">
				<xsl:for-each select="air:PassengerType">
					<xsl:variable name ="index" select="position()" />
					<xsl:variable name ="ptcAge" select="@Age" />
					<air:PassengerType>
						<xsl:attribute name="Code">
							<xsl:value-of select="$ptc"/>
						</xsl:attribute>
						<xsl:if test="$ptcAge != ''">
							<xsl:attribute name="Age">
								<xsl:value-of select="$ptcAge"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="BookingTravelerRef">
							<xsl:value-of select="@BookingTravelerRef"/>
						</xsl:attribute>
					</air:PassengerType>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test ="$storedFare/Markup/@Amount or $storedFare/BrandedFares">				
				<xsl:for-each select="air:PassengerType">
					<xsl:variable name ="index" select="position()" />
					<xsl:variable name ="ptcAge" select="@Age" />
					<air:PassengerType>
						<xsl:attribute name="Code">
							<xsl:value-of select="$ptc"/>
						</xsl:attribute>
						<xsl:if test="$ptcAge != ''">
							<xsl:attribute name="Age">
								<xsl:value-of select="$ptcAge"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="BookingTravelerRef">
							<xsl:value-of select="@BookingTravelerRef"/>
						</xsl:attribute>
					</air:PassengerType>
				</xsl:for-each>
			</xsl:when>
		</xsl:choose>
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

	<xsl:template name="getChangedPTC">
		<xsl:param name="pnr" />
		<xsl:variable name="changes" >
			<xsl:for-each select="//StoredFare[Markup or TourCode or Endorsement]">
				<!--<xsl:for-each select="$pnr/air:AirReservation/air:AirPricingInfo">-->
				<xsl:variable name="optc" select="PassengerType/@Code" />
				<xsl:variable name="price" select="$pnr/air:AirReservation/air:AirPricingInfo[substring(air:PassengerType/@Code,1,1)=substring($optc, 1,1)]" />
				<xsl:variable name="group" select="$price/@AirPricingInfoGroup" />
				<xsl:variable name="sibgroup">
					<xsl:choose>
						<xsl:when test="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup">
							<xsl:value-of select="$price/preceding-sibling::air:AirPricingInfo/@AirPricingInfoGroup"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$group"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="pos" select="position()"/>
				<xsl:variable name="ptc" select="$price/air:PassengerType/@Code" />

				<xsl:choose>
					<xsl:when test="$group = $sibgroup">
						<xsl:value-of select="concat($ptc, ',')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat($ptc, ',')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>

		<xsl:call-template name="tokenizeString">
			<xsl:with-param name="list" select="$changes"/>
			<xsl:with-param name="delimiter" select="','"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="EmulateOfficePrice">
		<air:PCC>
			<xsl:call-template name="EmulateOffice" />
		</air:PCC>
	</xsl:template>

	<xsl:template name="EmulateOffice" >
		<common_v50_0:OverridePCC>
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
			<xsl:attribute name="PseudoCityCode">
				<xsl:value-of select="//POS/Source/@PseudoCityCode"/>
			</xsl:attribute>
		</common_v50_0:OverridePCC>
	</xsl:template>
</xsl:stylesheet>