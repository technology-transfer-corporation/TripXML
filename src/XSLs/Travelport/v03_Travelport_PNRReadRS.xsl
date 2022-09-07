<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:air="http://www.travelport.com/schema/air_v50_0"
				xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0"
				xmlns:universal="http://www.travelport.com/schema/universal_v50_0"
				xmlns:SOAP="http://schemas.xmlsoap.org/soap/envelope/" version="1.0"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				xmlns:ttVB="urn:ttVB">
	<!-- 
	================================================================== 
	v03_Travelport_PNRReadRS.xsl 									
	==================================================================
	Date: 07 Sep 2022 - Kobelev - ARNK display fixed
	Date: 07 Sep 2022 - Samokhvalov - ARNK segments added
	Date: 18 Aug 2022 - Kobelev - Conversation ID fixed
	Date: 09 Aug 2022 - Kobelev - Price Supplemental Info Display
	Date: 08 Aug 2022 - Kobelev - Price Info Display 
	Date: 08 Mar 2022 - Kobelev - Pricing command display in SupplementalInfo 
	Date: 08 Mar 2022 - Kobelev - Controlling Carrier Identification 
	Date: 08 Mar 2022 - Kobelev - Remarks Name fields fix 	
	Date: 11 Feb 2022 - Kobelev - Implementation Changes 										
	Date: 11 Nov 2014 - Rastko 										
	================================================================== 
	-->
	<xsl:output omit-xml-declaration="yes"/>

	<xsl:key name="conCarr" match="//universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo/air:FareInfo/air:Brand/@Carrier" use="." />

	<xsl:variable name="loop">
		<xsl:choose>
			<xsl:when test="//universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo[@AirPricingInfoGroup=1]">
				<xsl:value-of select="(count(//universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo[@AirPricingInfoGroup=1]) div 2) + 1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="count(//universal:UniversalRecordRetrieveRsp/universal:UniversalRecord/air:AirReservation/air:AirPricingInfo) + 1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<!-- Identifies provider. Example: 1P - Worldspan, 1G - Galileo -->
	<xsl:variable name="provider">
		<xsl:value-of select="//universal:UniversalRecordRetrieveRsp/universal:ProviderReservationInfo/@ProviderCode"/>
	</xsl:variable>

	<xsl:template match="/">
		<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord"/>
		<xsl:apply-templates select="universal:UniversalRecordImportRsp/universal:UniversalRecord"/>
		<xsl:apply-templates select="universal:UniversalRecordModifyRsp/universal:UniversalRecord"/>
		<xsl:apply-templates select="SOAP:Fault/detail" mode="error"/>
	</xsl:template>
	<xsl:template match="detail" mode="error">
		<OTA_TravelItineraryRS Version="v03" AltLangID="Travelport">
			<Errors>
				<Error>
					<xsl:value-of select="../faultstring"/>
					<!-- Allot of time Description containes XML that will break Transformation logic 
					<xsl:value-of select="common_v50_0:ErrorInfo/common_v50_0:Description"/>
					-->
				</Error>
			</Errors>
		</OTA_TravelItineraryRS>
	</xsl:template>
	<xsl:template match="universal:UniversalRecord">
		<OTA_TravelItineraryRS Version="v03" AltLangID="Travelport">
			<!-- 
			<xsl:if test="@Version!=''">
				<xsl:attribute name="SequenceNmbr">
					<xsl:value-of select="@Version"/>
				</xsl:attribute>
			</xsl:if>
			-->
			<xsl:if test="EchoToken!=''">
				<xsl:attribute name="EchoToken">
					<xsl:value-of select="EchoToken"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="@LocatorCode!=''">
					<Success/>
					<xsl:if test="Error or Warning">
						<Warnings>
							<xsl:apply-templates select="Error" mode="warning"/>
							<xsl:apply-templates select="Warning" mode="warning"/>
						</Warnings>
					</xsl:if>
					<TravelItinerary>
						<ItineraryRef>
							<xsl:variable name="creationDate" select="ttVB:ShortDateFormat(common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Created']/@EventTime)" />

							<xsl:attribute name="Type">PNR</xsl:attribute>
							<xsl:attribute name="ID">
								<xsl:choose>
									<xsl:when test="universal:ProviderReservationInfo/@LocatorCode != ''">
										<xsl:value-of select="universal:ProviderReservationInfo/@LocatorCode"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@LocatorCode"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="ID_Context">
								<xsl:choose>
									<xsl:when test="universal:ProviderReservationInfo/@OwningPCC != ''">
										<xsl:value-of select="universal:ProviderReservationInfo/@OwningPCC"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<CompanyName>
								<xsl:attribute name="Code">
									<xsl:value-of select="concat(@LocatorCode, '|', $creationDate)"/>
								</xsl:attribute>
								<xsl:attribute name="CodeContext">
									<xsl:choose>
										<xsl:when test="universal:ProviderReservationInfo/@ProviderCode='1P'">
											<xsl:text>Worlspan</xsl:text>
										</xsl:when>
										<xsl:when test="universal:ProviderReservationInfo/@ProviderCode='1G'">
											<xsl:text>Galileo</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>Apollo</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>

								<xsl:choose>
									<xsl:when test="common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Ticketed']">
										<xsl:value-of select="concat(universal:ProviderReservationInfo/@OwningPCC,'|', common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Ticketed']/@BranchCode)"/>
									</xsl:when>
									<xsl:when test="common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Modified']">
										<xsl:value-of select="concat(universal:ProviderReservationInfo/@OwningPCC,'|', common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Modified']/@BranchCode)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(universal:ProviderReservationInfo/@OwningPCC,'|', common_v50_0:AgencyInfo/common_v50_0:AgentAction[@ActionType='Created']/@BranchCode)"/>
									</xsl:otherwise>
								</xsl:choose>

							</CompanyName>
						</ItineraryRef>
						<CustomerInfos>
							<xsl:apply-templates select="common_v50_0:BookingTraveler"/>
						</CustomerInfos>
						<ItineraryInfo>
							<xsl:variable name="refKey" select="air:AirReservation/common_v50_0:ProviderReservationInfoRef/@Key" />
							<xsl:if test="air:AirReservation/air:AirSegment | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL'] | originDestinationDetails/itineraryInf[elementManagementItinerary/segmentName='HU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] | 	originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
								<ReservationItems>
									<xsl:choose>
										<xsl:when test="common_v50_0:ProviderARNKSegment">
											<xsl:apply-templates select="air:AirReservation/air:AirSegment" mode="Air">
												<xsl:with-param name="arnk" select="common_v50_0:ProviderARNKSegment/@ProviderSegmentOrder" />
											</xsl:apply-templates>
										</xsl:when>
										<xsl:otherwise>
											<xsl:apply-templates select="air:AirReservation/air:AirSegment" mode="Air"/>
										</xsl:otherwise>
									</xsl:choose>

									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR']" mode="Car"/>
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU']" mode="Car"/>
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL']" mode="Hotel"/>
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU']" mode="Hotel"/>
									<!-- This is looks like Amadeus code -->
									<xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN']" mode="Train"/>
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']" mode="Cruise"/>
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']" mode="Tour"/>
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']" mode="Other"/>
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU']" mode="Taxi"/>
										<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR']" mode="Land"/>
									</xsl:if>

									<xsl:if test="air:AirReservation/air:AirPricingInfo">
										<ItemPricing>

											<xsl:call-template name="AirPricingInfo">
												<xsl:with-param name="ref" select="$refKey"/>
											</xsl:call-template>
										</ItemPricing>
									</xsl:if>
								</ReservationItems>
							</xsl:if>

							<xsl:apply-templates select="common_v50_0:ActionStatus[@ProviderReservationInfoRef=$refKey]" mode="ticketing"/>

							<SpecialRequestDetails>
								<xsl:if test="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]">
									<SeatRequests>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]" mode="Seat"/>
									</SeatRequests>
								</xsl:if>
								<xsl:if test="common_v50_0:SSR">
									<SpecialServiceRequests>
										<xsl:apply-templates select="common_v50_0:BookingTraveler/common_v50_0:SSR" mode="SSR"/>
										<xsl:apply-templates select="common_v50_0:SSR" mode="SSR"/>
									</SpecialServiceRequests>
								</xsl:if>
								<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']">
									<OtherServiceInformations>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']" mode="OSI"/>
									</OtherServiceInformations>
								</xsl:if>
								<xsl:if test="common_v50_0:GeneralRemark">
									<Remarks>
										<xsl:apply-templates select="common_v50_0:GeneralRemark[@TypeInGds!='Historical' ]" mode="GenRemark"/>
										<!-- and @Category='F' -->
										<xsl:apply-templates select="common_v50_0:GeneralRemark[@TypeInGds='Historical' ]" mode="HistoricalRemark"/>
									</Remarks>
								</xsl:if>
								<SpecialRemarks>
									<xsl:if test="common_v50_0:GeneralRemark[@Category='Vendor']">
										<xsl:apply-templates select="common_v50_0:GeneralRemark[@Category='Vendor']" mode="VendorRemark"/>
									</xsl:if>
									<xsl:if test="common_v50_0:AccountingRemark">
										<xsl:apply-templates select="common_v50_0:AccountingRemark" mode="AccountingRemark"/>
									</xsl:if>
									<!--
									<xsl:if test="common_v50_0:GeneralRemark[@TypeInGds='Historical']">
										<xsl:apply-templates select="common_v50_0:GeneralRemark[@TypeInGds='Historical']" mode="HistoricalRemark"/>
									</xsl:if>
									-->
									<xsl:if test="common_v50_0:ConfRemark">
										<xsl:apply-templates select="common_v50_0:ConfRemark" mode="ConfRemark"/>
									</xsl:if>
									<xsl:if test="air:AirReservation/air:AirPricingInfo[1]/air:FareInfo[1]/common_v50_0:Endorsement">
										<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[1]/air:FareInfo[1]/common_v50_0:Endorsement" mode="Endorsement"/>
									</xsl:if>
									<xsl:if test="common_v50_0:TourCode">
										<xsl:apply-templates select="common_v50_0:TourCode" mode="TourCode"/>
									</xsl:if>
									<xsl:if test="common_v50_0:TourCode">
										<xsl:apply-templates select="common_v50_0:TourCode" mode="TourCode"/>
									</xsl:if>
									<xsl:if test="air:AirReservation/air:AirPricingInfo[1]/air:FareInfo/air:Brand">
										<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[1]" mode="CC"/>
									</xsl:if>
								</SpecialRemarks>
							</SpecialRequestDetails>
							<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV' or elementManagementData/segmentName='FO' or elementManagementData/segmentName='FA' or 		elementManagementData/segmentName='FB' or elementManagementData/segmentName='FI' or substring(elementManagementData/segmentName,1,2)='FH']">
								<TPA_Extensions>
									<IssuedTickets>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA' or elementManagementData/segmentName='FB']" mode="IssuedTicket"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHA']" mode="AutomatedTicket"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHE']" mode="ElectronicTicket"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHM']" mode="ManualTicket"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FI']" mode="IssuedInvoice"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FO']" mode="ExchangeDocument"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV']" mode="TicketingCarrier"/>
									</IssuedTickets>
								</TPA_Extensions>
							</xsl:if>
						</ItineraryInfo>
						<xsl:if test="air:AirReservation/common_v50_0:FormOfPayment or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO']">
							<TravelCost>
								<xsl:apply-templates select="air:AirReservation/common_v50_0:FormOfPayment" mode="Payment"/>
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO' ]" mode="MCO"/>
							</TravelCost>
						</xsl:if>
						<xsl:if test="common_v50_0:FormOfPayment">
							<TravelCost>
								<xsl:apply-templates select="common_v50_0:FormOfPayment" mode="Payment"/>
							</TravelCost>
						</xsl:if>
						<UpdatedBy>
							<xsl:attribute name="CreateDateTime">
								<xsl:value-of select="universal:ProviderReservationInfo/@CreateDate"/>
							</xsl:attribute>
						</UpdatedBy>
						<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'] or common_v50_0:AccountingRemark">
							<TPA_Extensions>
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']" mode="commission"/>
								<xsl:apply-templates select="common_v50_0:AccountingRemark" mode="accounting"/>
							</TPA_Extensions>
						</xsl:if>
					</TravelItinerary>
				</xsl:when>
				<xsl:when test="Error">
					<Errors>
						<xsl:apply-templates select="Error" mode="error"/>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error Type="Amadeus">
							<xsl:value-of select="generalErrorInfo/messageErrorText/text"/>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="../ConversationID!=''">
				<ConversationID>
					<xsl:value-of select="../ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_TravelItineraryRS>
	</xsl:template>
	<xsl:template match="Error" mode="error">
		<Error Type="Amadeus">
			<xsl:value-of select="."/>
		</Error>
	</xsl:template>
	<xsl:template match="Text">
		<Text>
			<xsl:value-of select="."/>
		</Text>
	</xsl:template>
	<xsl:template match="Error | Warning" mode="warning">
		<Warning Type="Amadeus">
			<xsl:value-of select="."/>
		</Warning>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Pricing Response     	                                    -->
	<!-- ************************************************************** -->
	<xsl:template name="AirPricingInfo">
		<xsl:param name="ref"/>
		<xsl:variable name="priceGroup">
			<xsl:choose>
				<xsl:when test="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @PricingType = 'TicketRecord']">
					<xsl:value-of select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @PricingType = 'TicketRecord'][last()]/@AirPricingInfoGroup"/>
				</xsl:when>
				<xsl:when test="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @PricingType = 'StoredFareQuote']">
					<xsl:value-of select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @PricingType = 'StoredFareQuote']/@AirPricingInfoGroup"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @PricingType = 'StoredFare']/@AirPricingInfoGroup"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>
		<AirFareInfo>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:variable name="bf">
				<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = $priceGroup][1]" mode="totalbase">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="tf">
				<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = $priceGroup][1]" mode="totalprice">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="Taxf">
				<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = $priceGroup][1]" mode="totalTax">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="curt">
				<xsl:value-of select="substring(air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = $priceGroup]/@BasePrice,1,3)"/>
			</xsl:variable>
			<xsl:variable name="dect">
				<xsl:value-of select="string-length(substring-after(substring(air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = 2]/@BasePrice,4),'.'))"/>
			</xsl:variable>

			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$bf"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$curt"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dect"/>
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$Taxf"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$curt"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dect"/>
					</xsl:attribute>

					<!--
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$Taxf"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="$curt"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dect"/>
						</xsl:attribute>
					</Tax>
					-->
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$tf"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$curt"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dect"/>
					</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="air:AirReservation/air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup = $priceGroup]"/>
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</xsl:template>
	<xsl:template match="air:AirPricingInfo">
		<PTC_FareBreakdown>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:variable name="fcmi">
				<xsl:value-of select="@FareCalculationInd"/>
			</xsl:variable>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="$fcmi='G'">Published</xsl:when>
					<xsl:otherwise>Private</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:for-each select="air:PassengerType">
					<xsl:variable name="paxref">
						<xsl:value-of select="@BookingTravelerRef"/>
					</xsl:variable>
					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:call-template name="paxNumber">
						<xsl:with-param name="key" select="$paxref" />
					</xsl:call-template>
					<!--<xsl:value-of select="../../../common_v50_0:BookingTraveler[@Key=$paxref]/@Key"/>-->
				</xsl:for-each>
			</xsl:attribute>
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:for-each select="air:BookingInfo">
					<xsl:variable name="segref">
						<xsl:value-of select="@SegmentRef"/>
					</xsl:variable>
					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:call-template name="fltNumber">
						<xsl:with-param name="key" select="$segref" />
					</xsl:call-template>
					<!--<xsl:value-of select="../../air:AirSegment[@Key=$segref]/@Key"/>-->
				</xsl:for-each>
			</xsl:attribute>

			<xsl:attribute name="FCMI">
				<xsl:value-of select="$fcmi"/>
			</xsl:attribute>

			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:variable name="paxtype">
						<xsl:value-of select="air:PassengerType/@Code"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$paxtype = 'AA'">ADT</xsl:when>
						<xsl:when test="$paxtype = 'AD'">ADT</xsl:when>
						<xsl:when test="$paxtype = 'CH'">CHD</xsl:when>
						<xsl:when test="$paxtype = 'C'">CHD</xsl:when>
						<xsl:when test="$paxtype = 'IN'">INF</xsl:when>
						<xsl:when test="$paxtype = 'GGV'">GOV</xsl:when>
						<xsl:when test="$paxtype = 'CD'">SRC</xsl:when>
						<xsl:when test="$paxtype = 'SC'">SRC</xsl:when>
						<xsl:when test="$paxtype = 'STU'">STD</xsl:when>
						<xsl:when test="$paxtype = 'YC'">YTH</xsl:when>
						<xsl:when test="$paxtype = 'YCD'">SRC</xsl:when>
						<xsl:when test="$paxtype = ''">ADT</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$paxtype" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="count(air:PassengerType)"/>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<xsl:if test="air:FareInfo/@FareBasis">
				<FareBasisCodes>
					<xsl:apply-templates select="air:FareInfo" mode="fb"/>
				</FareBasisCodes>
			</xsl:if>
			<xsl:if test="air:FareInfo/air:Brand">
				<BrandedFares>
					<xsl:apply-templates select="air:FareInfo/air:Brand" mode="brand"/>
				</BrandedFares>
			</xsl:if>
			<xsl:variable name="nip">
				<xsl:value-of select="count(air:PassengerType)"/>
			</xsl:variable>
			<PassengerFare>
				<xsl:variable name="bfpax">
					<xsl:value-of select="translate(substring(@BasePrice,4),'.','') * $nip"/>
				</xsl:variable>
				<xsl:variable name="tfpax">
					<xsl:value-of select="translate(substring(@TotalPrice,4),'.','') * $nip"/>
				</xsl:variable>
				<xsl:variable name="cur">
					<xsl:value-of select="substring(@BasePrice,1,3)"/>
				</xsl:variable>
				<xsl:variable name="dec">
					<xsl:value-of select="string-length(substring-after(substring(@BasePrice,4),'.'))"/>
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$bfpax"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</BaseFare>

				<!--
				<Taxes>
					<xsl:apply-templates select="air:TaxInfo">
						<xsl:with-param name="nip">
							<xsl:value-of select="$nip"/>
						</xsl:with-param>
						<xsl:with-param name="dec">
							<xsl:value-of select="$dec"/>
						</xsl:with-param>
						<xsl:with-param name="cur">
							<xsl:value-of select="$cur"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</Taxes>
				-->

				<Taxes>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(substring(@Taxes, 4), '.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$tfpax"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<xsl:if test="air:FareCalc">
					<FareCalculation>
						<xsl:value-of select="air:FareCalc"/>
					</FareCalculation>
				</xsl:if>
				<xsl:if test="otherPricingInfo/attributeDetails[attributeType='PAY']">
					<PaymentRestrictions>
						<xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='PAY']/attributeDescription"/>
					</PaymentRestrictions>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="../air:TicketingModifiers/@PlatingCarrier != ''">
						<ValidatingAirlineCode>
							<xsl:value-of select="../air:TicketingModifiers/@PlatingCarrier"/>
						</ValidatingAirlineCode>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="paxassoc">
							<xsl:for-each select="paxSegReference/refDetails">
								<xsl:sort order="ascending" data-type="text" select="refNumber"/>
								<xsl:value-of select="refNumber"/>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="segassoc">
							<xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation) or connexInformation/connecDetails/routingInformation != 'ARNK']">
								<xsl:sort order="ascending" data-type="text" select="segmentReference/refDetails/refNumber"/>
								<xsl:value-of select="segmentReference/refDetails/refNumber"/>
							</xsl:for-each>
						</xsl:variable>
						<xsl:for-each select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV']">
							<xsl:variable name="paxfv">
								<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
									<xsl:sort order="ascending" data-type="text" select="number"/>
									<xsl:value-of select="number"/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="segfv">
								<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']">
									<xsl:sort order="ascending" data-type="text" select="number"/>
									<xsl:value-of select="number"/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:if test="contains($paxfv,$paxassoc) and contains($segfv,$segassoc) or ($paxfv='' and $segfv='')">
								<ValidatingAirlineCode>
									<xsl:choose>
										<xsl:when test="starts-with(otherDataFreetext/longFreetext,'PAX ')">
											<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'PAX ')"/>
										</xsl:when>
										<xsl:when test="starts-with(otherDataFreetext/longFreetext,'INF ')">
											<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'INF ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="otherDataFreetext/longFreetext"/>
										</xsl:otherwise>
									</xsl:choose>
								</ValidatingAirlineCode>
							</xsl:if>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>

				<BagAllowance>
					<xsl:attribute name="Quantity">
						<xsl:value-of select="sum(air:BaggageAllowance/@NumberOfPieces)"/>
					</xsl:attribute>
					<xsl:attribute name="Type">
						<xsl:text>Piece</xsl:text>
					</xsl:attribute>
				</BagAllowance>
				<!--
				<xsl:for-each select="air:FareInfo">
					<xsl:if test="air:BaggageAllowance">
						<BagAllowance>
							<xsl:choose>
								<xsl:when test="air:BaggageAllowance/air:NumberOfPieces!=''">
									<xsl:attribute name="Quantity">
										<xsl:value-of select="air:BaggageAllowance/air:NumberOfPieces"/>
									</xsl:attribute>
									<xsl:attribute name="Type">
										<xsl:text>Piece</xsl:text>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Weight">
										<xsl:value-of select="air:BaggageAllowance/air:MaxWeight"/>
									</xsl:attribute>
									<xsl:attribute name="Type">
										<xsl:text>Weight</xsl:text>
									</xsl:attribute>
									<xsl:attribute name="Unit">
										<xsl:value-of select="air:BaggageAllowance/air:MaxWeight"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="ItinSeqNumber">
								<xsl:value-of select="position()"/>
							</xsl:attribute>
						</BagAllowance>
					</xsl:if>
				</xsl:for-each>
				-->
				<xsl:if test="air:ActionDetails/@Text">
					<SupplementalInfo>
						<xsl:value-of select="air:ActionDetails/@Text"/>
					</SupplementalInfo>
				</xsl:if>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>
	<xsl:template match="air:FareInfo" mode="fb">
		<xsl:variable name="td" select="air:FareTicketDesignator/@Value"/>
		<FareBasisCode>
			<xsl:choose>
				<xsl:when test="air:FareTicketDesignator/@Value">
					<xsl:value-of select="concat(@FareBasis,'/',$td)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="@FareBasis"/>
				</xsl:otherwise>
			</xsl:choose>
		</FareBasisCode>
	</xsl:template>

	<xsl:template match="air:Brand" mode="brand">
		<FareFamily>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="@BrandTier"/>
			</xsl:attribute>
			<xsl:value-of select="@Name"/>
		</FareFamily>
	</xsl:template>
	<xsl:template match="air:TaxInfo">
		<xsl:param name="nip"/>
		<xsl:param name="dec"/>
		<xsl:param name="cur"/>
		<Tax>
			<xsl:attribute name="Code">
				<xsl:value-of select="@Category"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="$cur"/>
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(substring(@Amount,4),'.','') * $nip"/>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$dec"/>
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Process Names			                            -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:BookingTraveler">
		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:variable name="ref">
				<xsl:value-of select="@Key"/>
			</xsl:variable>
			<Customer>
				<!-- CHD07OCT10 -->
				<xsl:if test="contains(common_v50_0:NameRemark/common_v50_0:RemarkData, 'CHD') or contains(common_v50_0:NameRemark/common_v50_0:RemarkData, 'INF')">
					<xsl:attribute name="BirthDate">
						<xsl:call-template name="bdt">
							<xsl:with-param name="bdt">
								<!-- <NameRmk>23FEB20</NameRmk> -->
								<xsl:choose>
									<xsl:when test="contains(common_v50_0:NameRemark/common_v50_0:RemarkData, 'INF')">
										<xsl:value-of select="substring-after(common_v50_0:NameRemark/common_v50_0:RemarkData, 'INF')" />
									</xsl:when>
									<xsl:when test="contains(common_v50_0:NameRemark/common_v50_0:RemarkData, 'CHD')">
										<xsl:value-of select="substring-after(common_v50_0:NameRemark/common_v50_0:RemarkData, 'CHD')" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="common_v50_0:NameRemark/common_v50_0:RemarkData" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:attribute>
				</xsl:if>
				<PersonName>
					<xsl:variable name="ptc">
						<xsl:choose>
							<xsl:when test="@TravelerType">
								<xsl:value-of select="@TravelerType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="../air:AirReservation[common_v50_0:BookingTravelerRef/@Key=$ref]/air:AirPricingInfo/air:PassengerType[@BookingTravelerRef=$ref]/@Code"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:attribute name="NameType">

						<xsl:choose>
							<xsl:when test="$ptc = 'AA'">ADT</xsl:when>
							<xsl:when test="$ptc = 'AD'">ADT</xsl:when>
							<xsl:when test="$ptc = 'CHD'">CHD</xsl:when>
							<xsl:when test="$ptc = 'CH'">CHD</xsl:when>
							<xsl:when test="$ptc = 'C'">CHD</xsl:when>
							<xsl:when test="$ptc = 'CNN'">CHD</xsl:when>
							<xsl:when test="$ptc = 'INN'">INF</xsl:when>
							<xsl:when test="$ptc = 'IN'">INF</xsl:when>
							<xsl:when test="$ptc = 'INF'">INF</xsl:when>
							<xsl:when test="$ptc = 'GOV'">GOV</xsl:when>
							<xsl:when test="$ptc = 'GGV'">GOV</xsl:when>
							<xsl:when test="$ptc = 'MIL'">MIL</xsl:when>
							<xsl:when test="$ptc = 'CD'">SRC</xsl:when>
							<xsl:when test="$ptc = 'SC'">SRC</xsl:when>
							<xsl:when test="$ptc = 'STU'">STD</xsl:when>
							<xsl:when test="$ptc = 'YC'">YTH</xsl:when>
							<xsl:when test="$ptc = 'YCD'">SRC</xsl:when>
							<xsl:when test="$ptc = ''">ADT</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$ptc"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<GivenName>
						<xsl:value-of select="common_v50_0:BookingTravelerName/@First"/>
					</GivenName>
					<Surname>
						<xsl:value-of select="common_v50_0:BookingTravelerName/@Last"/>
					</Surname>
				</PersonName>

				<xsl:variable name="paxref">
					<xsl:value-of select="@Key"/>
				</xsl:variable>

				<xsl:apply-templates select="common_v50_0:PhoneNumber" mode="phone"/>
				<xsl:apply-templates select="common_v50_0:Email" mode="email"/>
				<xsl:apply-templates select="common_v50_0:Address" mode="Address"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][serviceRequest/ssr/status='HK']" mode="Fqtv">
					<xsl:with-param name="paxref">
						<xsl:value-of select="$paxref"/>
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:if test="../common_v50_0:AgencyContactInfo/common_v50_0:PhoneNumber">
					<xsl:apply-templates select="../common_v50_0:AgencyContactInfo/common_v50_0:PhoneNumber" mode="phone"/>
				</xsl:if>
			</Customer>
		</CustomerInfo>
		<xsl:if test="travellerInformation/passenger[1]/infantIndicator = 1 and travellerInformation/passenger[position()=2]/type = 'INF'">
			<CustomerInfo>
				<xsl:attribute name="RPH">
					<xsl:value-of select="../elementManagementPassenger/lineNumber"/>
				</xsl:attribute>
				<Customer>
					<xsl:if test="dateOfBirth/dateAndTimeDetails[qualifier='706']">
						<xsl:attribute name="BirthDate">
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,5,4)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,1,2)"/>
						</xsl:attribute>
					</xsl:if>
					<PersonName>
						<xsl:attribute name="NameType">INF</xsl:attribute>
						<GivenName>
							<xsl:choose>
								<xsl:when test="travellerInformation/passenger[position()=2]/firstName!=''">
									<xsl:value-of select="travellerInformation/passenger[position()=2]/firstName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'NONAME'"/>
								</xsl:otherwise>
							</xsl:choose>
						</GivenName>
						<Surname>
							<xsl:choose>
								<xsl:when test="travellerInformation/traveller/surname!=''">
									<xsl:value-of select="travellerInformation/traveller/surname"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="../passengerData[1]/travellerInformation/traveller/surname"/>
								</xsl:otherwise>
							</xsl:choose>
						</Surname>
					</PersonName>
				</Customer>
			</CustomerInfo>
		</xsl:if>
	</xsl:template>
	<!-- ****************************************************************************************************************** -->
	<!-- Process Itinerary				 							                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Air Segments    				                    -->
	<!-- ************************************************************** -->
	<xsl:template match="air:AirSegment" mode="Air">
		<xsl:param name="arnk"/>

		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>

		<xsl:for-each select="$arnk">
			<xsl:if test=".=$pos">
				<xsl:apply-templates select="../../common_v50_0:ProviderARNKSegment[@ProviderSegmentOrder=$pos]" />
			</xsl:if>
		</xsl:for-each>


		<Item>

			<xsl:attribute name="Status">
				<xsl:value-of select="@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="@TravelOrder"/>
			</xsl:attribute>
			<xsl:if test="@Status='GK'">
				<xsl:attribute name="IsPassive">true</xsl:attribute>
			</xsl:if>

			<Air>
				<!--************************************************************************************-->
				<!--			Air Segments/Open Segments  						      -->
				<!--************************************************************************************-->
				<xsl:variable name="zeroes">0000</xsl:variable>
				<xsl:attribute name="DepartureDateTime">
					<xsl:value-of select="@DepartureTime"/>
				</xsl:attribute>
				<xsl:attribute name="ArrivalDateTime">
					<xsl:value-of select="@ArrivalTime"/>
				</xsl:attribute>
				<xsl:attribute name="StopQuantity">
					<xsl:value-of select="count(air:FlightDetails) - 1"/>
				</xsl:attribute>
				<xsl:attribute name="RPH">
					<xsl:value-of select="position()"/>
				</xsl:attribute>
				<xsl:attribute name="FlightNumber">
					<xsl:value-of select="@FlightNumber"/>
				</xsl:attribute>
				<xsl:attribute name="ResBookDesigCode">
					<xsl:value-of select="@ClassOfService"/>
				</xsl:attribute>
				<xsl:if test="relatedProduct/quantity!=''">
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:attribute name="Status">
					<xsl:value-of select="@Status"/>
				</xsl:attribute>
				<xsl:if test="@ETicketability != ''">
					<xsl:attribute name="E_TicketEligibility">
						<xsl:choose>
							<xsl:when test="@ETicketability = 'Yes'">Eligible</xsl:when>
							<xsl:otherwise>NotEligible</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:if>
				<DepartureAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="@Origin"/>
					</xsl:attribute>
					<xsl:if test="flightDetail/departureInformation/departTerminal != ''">
						<xsl:attribute name="Terminal">
							<xsl:value-of select="flightDetail/departureInformation/departTerminal"/>
						</xsl:attribute>
					</xsl:if>
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="@Destination"/>
					</xsl:attribute>
				</ArrivalAirport>
				<OperatingAirline>
					<xsl:choose>
						<xsl:when test="itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text !=''">
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Code">
								<xsl:value-of select="@Carrier"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</OperatingAirline>
				<Equipment>
					<xsl:attribute name="AirEquipType">
						<xsl:value-of select="air:FlightDetails/@Equipment"/>
					</xsl:attribute>
				</Equipment>
				<MarketingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="@Carrier"/>
					</xsl:attribute>
				</MarketingAirline>
				<TPA_Extensions>
					<xsl:attribute name="ConfirmationNumber">
						<xsl:variable name="airline">
							<xsl:value-of select="@Carrier"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="../common_v50_0:SupplierLocator[@SupplierCode=$airline]">
								<xsl:value-of select="../common_v50_0:SupplierLocator[@SupplierCode=$airline]/@SupplierLocatorCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="../@LocatorCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:if test="air:FlightDetails/@FlightTime">
						<xsl:variable name="zeros">00</xsl:variable>
						<xsl:variable name="jt">
							<xsl:value-of select="air:FlightDetails/@FlightTime" />
						</xsl:variable>
						<xsl:variable name="hours">
							<xsl:choose>
								<xsl:when test="substring-before(($jt div 60),'.')=''">
									<xsl:value-of select="$jt div 60" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(($jt div 60),'.')" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="minutes">
							<xsl:value-of select="$jt - ($hours*60)" />
						</xsl:variable>
						<xsl:attribute name="FlightDuration">
							<xsl:choose>
								<xsl:when test="$minutes = 'NaN'">
									<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
									<xsl:value-of select="$hours" />
									<xsl:text>:00</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" /><xsl:value-of select="$hours" />:<xsl:value-of select="substring(string($zeros),1,2-string-length($minutes))" /><xsl:value-of select="$minutes" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:if>
				</TPA_Extensions>
			</Air>
		</Item>
	</xsl:template>
	<xsl:template match="common_v50_0:ProviderARNKSegment">
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="@ProviderSegmentOrder"/>
			</xsl:attribute>
			<TPA_Extensions>
				<Arnk />
			</TPA_Extensions>
		</Item>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Hotel Segs   						   					    -->
	<!--************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Hotel">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
			</xsl:attribute>
			<xsl:if test="elementManagementItinerary/segmentName='HU'">
				<xsl:attribute name="IsPassive">true</xsl:attribute>
			</xsl:if>
			<Hotel>
				<xsl:choose>
					<xsl:when test="elementManagementItinerary/segmentName='HU'">
						<Reservation>
							<GuestCounts>
								<GuestCount>
									<xsl:attribute name="Count">
										<xsl:value-of select="relatedProduct/quantity"/>
									</xsl:attribute>
								</GuestCount>
							</GuestCounts>
							<TimeSpan>
								<xsl:attribute name="Start">
									<xsl:text>20</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:attribute name="End">
									<xsl:text>20</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
								</xsl:attribute>
							</TimeSpan>
							<BasicPropertyInfo>
								<xsl:attribute name="ChainCode">
									<xsl:value-of select="travelProduct/companyDetail/identification"/>
								</xsl:attribute>
								<xsl:attribute name="HotelCityCode">
									<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
								</xsl:attribute>
								<xsl:attribute name="HotelName">
									<xsl:choose>
										<xsl:when test="elementManagementItinerary/segmentName='HU' and contains(itineraryFreetext/longFreetext,',')">
											<xsl:value-of select="substring-before(substring-after(itineraryFreetext/longFreetext,','),',')"/>
										</xsl:when>
										<xsl:when test="elementManagementItinerary/segmentName='HU' and contains(itineraryFreetext/longFreetext,'-')">
											<xsl:value-of select="substring-before(itineraryFreetext/longFreetext,'-')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-before(itineraryFreetext/longFreetext,'/')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</BasicPropertyInfo>
						</Reservation>
					</xsl:when>
					<xsl:otherwise>
						<Reservation>
							<RoomTypes>
								<RoomType>
									<xsl:attribute name="RoomTypeCode">
										<xsl:choose>
											<xsl:when test="hotelProduct/hotelRoom/typeCode!=''">
												<xsl:value-of select="hotelProduct/hotelRoom/typeCode"/>
											</xsl:when>
											<xsl:when test="hotelReservationInfo/roomRateDetails/roomInformation/roomTypeOverride!=''">
												<xsl:value-of select="hotelReservationInfo/roomRateDetails/roomInformation/roomTypeOverride"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="generalOption/optionDetail[type='RO']/freetext"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<xsl:attribute name="NumberOfUnits">
										<xsl:value-of select="relatedProduct/quantity"/>
									</xsl:attribute>
									<xsl:if test="generalOption[optionDetail/type='DES']">
										<RoomDescription>
											<xsl:element name="Text">
												<xsl:for-each select="generalOption[optionDetail/type='DES']/optionDetail/freetext">
													<xsl:value-of select="."/>
													<xsl:text>,</xsl:text>
												</xsl:for-each>
											</xsl:element>
										</RoomDescription>
									</xsl:if>
								</RoomType>
							</RoomTypes>
							<RatePlans>
								<RatePlan>
									<xsl:attribute name="RatePlanCode">
										<xsl:value-of select="hotelProduct/negotiated/rateCode"/>
									</xsl:attribute>
								</RatePlan>
							</RatePlans>
							<RoomRates>
								<RoomRate>
									<xsl:if test="rateInformations/rateInfo/ratePlan!=''">
										<xsl:attribute name="RatePlanType">
											<xsl:choose>
												<xsl:when test="rateInformations/rateInfo/ratePlan='DY'">Daily</xsl:when>
												<xsl:when test="rateInformations/rateInfo/ratePlan='PER'">Period</xsl:when>
												<xsl:otherwise>Package</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</xsl:if>
									<xsl:if test="generalOption[optionDetail/type='BC']">
										<xsl:attribute name="RatePlanID">
											<xsl:value-of select="generalOption[optionDetail/type='BC']/optionDetail/freetext"/>
										</xsl:attribute>
									</xsl:if>
									<Rates>
										<Rate>
											<xsl:if test="hotelReservationInfo/guaranteeOrDeposit/paymentInfo/paymentDetails/paymentType='1'">
												<xsl:attribute name="GuaranteedInd">true</xsl:attribute>
											</xsl:if>
											<Base>
												<xsl:choose>
													<xsl:when test="elementManagementItinerary/segmentName='HU'">
														<xsl:if test="contains(itineraryFreetext/longFreetext,'/AO-')">
															<xsl:variable name="amt">
																<xsl:value-of select="substring-before(substring-after(itineraryFreetext/longFreetext,'/AO-'),'/')"/>
															</xsl:variable>
															<xsl:attribute name="AmountBeforeTax">
																<xsl:value-of select="translate(substring($amt,4),'.','')"/>
															</xsl:attribute>
															<xsl:attribute name="CurrencyCode">
																<xsl:value-of select="substring($amt,1,3)"/>
															</xsl:attribute>
															<xsl:attribute name="DecimalPlaces">
																<xsl:value-of select="string-length(substring-after($amt,'.'))"/>
															</xsl:attribute>
														</xsl:if>
													</xsl:when>
													<xsl:when test="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/amount != ''">
														<xsl:variable name="amt">
															<xsl:value-of select="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/amount"/>
														</xsl:variable>
														<xsl:attribute name="AmountBeforeTax">
															<xsl:value-of select="translate($amt,'.','')"/>
														</xsl:attribute>
														<xsl:attribute name="CurrencyCode">
															<xsl:value-of select="hotelReservationInfo/roomRateDetails/tariffDetails/tariffInfo/currency"/>
														</xsl:attribute>
														<xsl:attribute name="DecimalPlaces">
															<xsl:value-of select="string-length(substring-after($amt,'.'))"/>
														</xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:variable name="deci">
															<xsl:value-of select="substring-after(string(generalOption/optionDetail[type='TTL']/freetext),'.')"/>
														</xsl:variable>
														<xsl:variable name="decilgth">
															<xsl:value-of select="string-length($deci)"/>
														</xsl:variable>
														<xsl:variable name="amount">
															<xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),4,20)"/>
														</xsl:variable>
														<xsl:attribute name="AmountBeforeTax">
															<xsl:value-of select="translate(string($amount),'.','')"/>
														</xsl:attribute>
														<xsl:attribute name="CurrencyCode">
															<xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),1,3)"/>
														</xsl:attribute>
														<xsl:attribute name="DecimalPlaces">
															<xsl:value-of select="$decilgth"/>
														</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</Base>
										</Rate>
									</Rates>
								</RoomRate>
							</RoomRates>
							<GuestCounts>
								<GuestCount>
									<xsl:attribute name="Count">
										<xsl:value-of select="hotelProduct/hotelRoom/occupancy"/>
									</xsl:attribute>
								</GuestCount>
							</GuestCounts>
							<TimeSpan>
								<xsl:attribute name="Start">
									<xsl:text>20</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:if test="generalOption[optionDetail/type='NGT']">
									<xsl:attribute name="Duration">
										<xsl:value-of select="generalOption[optionDetail/type='NGT']/optionDetail/freetext"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="End">
									<xsl:text>20</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
								</xsl:attribute>
							</TimeSpan>
							<xsl:choose>
								<xsl:when test="hotelReservationInfo/guaranteeOrDeposit/paymentInfo/paymentDetails/paymentType='1'">
									<Guarantee>
										<GuaranteesAccepted>
											<GuaranteeAccepted>
												<xsl:choose>
													<xsl:when test="hotelReservationInfo/guaranteeOrDeposit/paymentInfo/paymentDetails/formOfPaymentCode='1'">
														<PaymentCard>
															<xsl:attribute name="CardType">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/type"/>
															</xsl:attribute>
															<xsl:attribute name="CardCode">
																<xsl:choose>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'CA'">MC</xsl:when>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'MA'">MC</xsl:when>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'DC'">DN</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2)"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:attribute>
															<xsl:attribute name="CardNumber">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/creditCardNumber"/>
															</xsl:attribute>
															<xsl:attribute name="ExpireDate">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/expiryDate"/>
															</xsl:attribute>
														</PaymentCard>
													</xsl:when>
													<xsl:otherwise>
														<DirectBill DirectBill_ID="Other"/>
													</xsl:otherwise>
												</xsl:choose>
											</GuaranteeAccepted>
										</GuaranteesAccepted>
									</Guarantee>
								</xsl:when>
								<xsl:when test="generalOption[optionDetail/type='G']">
									<Guarantee>
										<GuaranteesAccepted>
											<GuaranteeAccepted>
												<PaymentCard>
													<xsl:attribute name="CardType">
														<xsl:value-of select="substring(generalOption[optionDetail/type='G']/optionDetail/freetext,1,2)"/>
													</xsl:attribute>
													<xsl:attribute name="CardCode">
														<xsl:choose>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'CA'">MC</xsl:when>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'MA'">MC</xsl:when>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'DC'">DN</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2)"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>
													<xsl:attribute name="CardNumber">
														<xsl:value-of select="substring-before(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,5),'EXP')"/>
													</xsl:attribute>
													<xsl:attribute name="ExpireDate">
														<xsl:value-of select="substring-after(generalOption[optionDetail/type='G']/optionDetail/freetext,'EXP')"/>
													</xsl:attribute>
												</PaymentCard>
											</GuaranteeAccepted>
										</GuaranteesAccepted>
									</Guarantee>
								</xsl:when>
								<xsl:when test="hotelReservationInfo/guaranteeOrDeposit/paymentInfo/paymentDetails/paymentType='2'">
									<DepositPayment>
										<AcceptedPayments>
											<AcceptedPayment>
												<xsl:choose>
													<xsl:when test="hotelReservationInfo/guaranteeOrDeposit/paymentInfo/paymentDetails/formOfPaymentCode='1'">
														<PaymentCard>
															<xsl:attribute name="CardType">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/type"/>
															</xsl:attribute>
															<xsl:attribute name="CardCode">
																<xsl:choose>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'CA'">MC</xsl:when>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'MA'">MC</xsl:when>
																	<xsl:when test="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2) = 'DC'">DN</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="substring(hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/vendorCode,1,2)"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:attribute>
															<xsl:attribute name="CardNumber">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/creditCardNumber"/>
															</xsl:attribute>
															<xsl:attribute name="ExpireDate">
																<xsl:value-of select="hotelReservationInfo/guaranteeOrDeposit/creditCardInfo/formOfPayment/expiryDate"/>
															</xsl:attribute>
														</PaymentCard>
													</xsl:when>
													<xsl:otherwise>
														<DirectBill DirectBill_ID="Other"/>
													</xsl:otherwise>
												</xsl:choose>
											</AcceptedPayment>
										</AcceptedPayments>
									</DepositPayment>
								</xsl:when>
								<xsl:when test="generalOption[optionDetail/type='D']">
									<DepositPayment>
										<AcceptedPayments>
											<AcceptedPayment>
												<PaymentCard>
													<xsl:attribute name="CardType">
														<xsl:value-of select="substring(generalOption[optionDetail/type='G']/optionDetail/freetext,1,2)"/>
													</xsl:attribute>
													<xsl:attribute name="CardCode">
														<xsl:choose>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'CA'">MC</xsl:when>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'MA'">MC</xsl:when>
															<xsl:when test="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2) = 'DC'">DN</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="substring(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,3,2),1,2)"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>
													<xsl:attribute name="CardNumber">
														<xsl:value-of select="substring-before(substring(generalOption[optionDetail/type='G']/optionDetail/freetext,5),'EXP')"/>
													</xsl:attribute>
													<xsl:attribute name="ExpireDate">
														<xsl:value-of select="substring-after(generalOption[optionDetail/type='G']/optionDetail/freetext,'EXP')"/>
													</xsl:attribute>
												</PaymentCard>
											</AcceptedPayment>
										</AcceptedPayments>
									</DepositPayment>
								</xsl:when>
							</xsl:choose>
							<BasicPropertyInfo>
								<xsl:attribute name="ChainCode">
									<xsl:value-of select="travelProduct/companyDetail/identification"/>
								</xsl:attribute>
								<xsl:attribute name="HotelCityCode">
									<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
								</xsl:attribute>
								<xsl:attribute name="HotelCode">
									<xsl:value-of select="hotelProduct/property/code"/>
								</xsl:attribute>
								<xsl:attribute name="HotelName">
									<xsl:choose>
										<xsl:when test="elementManagementItinerary/segmentName='HU'">
											<xsl:value-of select="substring-before(itineraryFreetext/longFreetext,'/')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="hotelProduct/property/name"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="ChainName">
									<xsl:value-of select="hotelProduct/property/providerName"/>
								</xsl:attribute>
							</BasicPropertyInfo>
						</Reservation>
						<TPA_Extensions>
							<xsl:if test="generalOption[optionDetail/type='BS']">
								<xsl:attribute name="BookingSource">
									<xsl:value-of select="generalOption[optionDetail/type='BS']/optionDetail/freetext"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="ConfirmationNumber">
								<xsl:choose>
									<xsl:when test="elementManagementItinerary/segmentName='HU'">
										<xsl:if test="contains(itineraryFreetext/longFreetext,'/CF-')">
											<xsl:value-of select="substring-before(substring-after(itineraryFreetext/longFreetext,'/CF-'),'/')"/>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</TPA_Extensions>
					</xsl:otherwise>
				</xsl:choose>
			</Hotel>
		</Item>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Car Segs				   			-->
	<!--************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Car">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
			</xsl:attribute>
			<xsl:if test="elementManagementItinerary/segmentName='CU'">
				<xsl:attribute name="IsPassive">true</xsl:attribute>
			</xsl:if>
			<Vehicle>
				<ConfID>
					<xsl:attribute name="Type">8</xsl:attribute>
					<xsl:attribute name="ID">
						<xsl:choose>
							<xsl:when test="generalOption/optionDetail[type='CF']/freetext != ''">
								<xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/>
							</xsl:when>
							<xsl:when test="typicalCarData/cancelOrConfirmNbr/reservation[controlType='3' or controlType='2']/controlNumber!= ''">
								<xsl:value-of select="typicalCarData/cancelOrConfirmNbr/reservation[controlType='3' or controlType='2']/controlNumber"/>
							</xsl:when>
							<xsl:when test="generalOption/optionDetail[type='BS']/freetext!=''">
								<xsl:value-of select="generalOption/optionDetail[type='BS']/freetext"/>
							</xsl:when>
							<xsl:when test="contains(itineraryFreetext/longFreetext,'/CF-')">
								<xsl:value-of select="substring-after(itineraryFreetext/longFreetext,'/CF-')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>UNKNOWN</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</ConfID>
				<Vendor>
					<xsl:attribute name="Code">
						<xsl:value-of select="travelProduct/companyDetail/identification"/>
					</xsl:attribute>
				</Vendor>
				<VehRentalCore>
					<xsl:attribute name="PickUpDateTime">
						<xsl:choose>
							<xsl:when test="elementManagementItinerary/segmentName='CU'">
								<xsl:text>20</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/depDate,5)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
								<xsl:text>T00:00:00</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="typicalCarData/pickupDropoffTimes/beginDateTime/year"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/month,'00')"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/day,'00')"/>
								<xsl:text>T</xsl:text>
								<xsl:choose>
									<xsl:when test="typicalCarData/pickupDropoffTimes/beginDateTime/hour != ''">
										<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/hour,'00')"/>
									</xsl:when>
									<xsl:otherwise>00</xsl:otherwise>
								</xsl:choose>
								<xsl:text>:</xsl:text>
								<xsl:choose>
									<xsl:when test="typicalCarData/pickupDropoffTimes/beginDateTime/minutes != ''">
										<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/minutes,'00')"/>
									</xsl:when>
									<xsl:otherwise>00</xsl:otherwise>
								</xsl:choose>
								<xsl:text>:00</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="ReturnDateTime">
						<xsl:choose>
							<xsl:when test="elementManagementItinerary/segmentName='CU'">
								<xsl:text>20</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/arrDate,5)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
								<xsl:text>T00:00:00</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="typicalCarData/pickupDropoffTimes/endDateTime/year"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/month,'00')"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/day,'00')"/>
								<xsl:text>T</xsl:text>
								<xsl:choose>
									<xsl:when test="typicalCarData/pickupDropoffTimes/endDateTime/hour!=''">
										<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/hour,'00')"/>
										<xsl:text>:</xsl:text>
										<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/minutes,'00')"/>
										<xsl:text>:00</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>00:00:00</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:variable name="pickup">
						<xsl:choose>
							<xsl:when test="typicalCarData/locationInfo[locationType='176']/locationDescription/code='1A'">
								<xsl:value-of select="typicalCarData/locationInfo[locationType='176']/locationDescription/name"/>
							</xsl:when>
							<xsl:when test="typicalCarData/locationInfo[locationType='176']/firstLocationDetails/code!=''">
								<xsl:value-of select="typicalCarData/locationInfo[locationType='176']/firstLocationDetails/code"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<PickUpLocation>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="$pickup"/>
						</xsl:attribute>
					</PickUpLocation>
					<ReturnLocation>
						<xsl:attribute name="LocationCode">
							<xsl:choose>
								<xsl:when test="typicalCarData/locationInfo[locationType='DOL']/locationDescription/code='1A'">
									<xsl:value-of select="typicalCarData/locationInfo[locationType='DOL']/locationDescription/name"/>
								</xsl:when>
								<xsl:when test="travelProduct/offpointDetail/cityCode != ''">
									<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
								</xsl:when>
								<xsl:when test="tgeneralOption/optionDetail[type='DO']/freetext != ''">
									<xsl:value-of select="generalOption/optionDetail[type='DO']/freetext"/>
								</xsl:when>
								<xsl:when test="travelProduct/boardpointDetail/cityCode != ''">
									<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$pickup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</ReturnLocation>
				</VehRentalCore>
				<Veh>
					<xsl:attribute name="AirConditionInd">
						<xsl:choose>
							<xsl:when test="substring(travelProduct/productDetails/identification,4,1) = 'R'">true</xsl:when>
							<xsl:otherwise>false</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="TransmissionType">
						<xsl:choose>
							<xsl:when test="substring(travelProduct/productDetails/identification,3,1) = 'A'">Automatic</xsl:when>
							<xsl:otherwise>Manual</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<VehType>
						<xsl:attribute name="VehicleCategory">
							<xsl:choose>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'C'">2/4 Door Car</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'B'">2-Door Car</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'D'">4-Door Car</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'W'">Wagon</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'V'">Van</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'L'">Limousine</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'S'">Sport</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'T'">Convertible</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'F'">4-Wheel Drive</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'P'">Pickup</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,2,1) = 'J'">All-Terrain</xsl:when>
								<xsl:otherwise>Unavailable</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="travelProduct/productDetails/identification"/>
					</VehType>
					<VehClass>
						<xsl:attribute name="Size">
							<xsl:choose>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'M'">Mini</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'E'">Economy</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'C'">Compact</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'I'">Intermediate</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'S'">Standard</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'F'">Full-Size</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'P'">Premium</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'L'">Luxury</xsl:when>
								<xsl:when test="substring(travelProduct/productDetails/identification,1,1) = 'X'">Special</xsl:when>
								<xsl:otherwise>Unavailable</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</VehClass>
				</Veh>
				<xsl:if test="elementManagementItinerary/segmentName!='CU'">
					<RentalRate>
						<xsl:choose>
							<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType='RG' or amountType='RQ']/amount != ''">
								<xsl:apply-templates select="typicalCarData"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="rateText">
									<xsl:choose>
										<xsl:when test="typicalCarData/rateInfo/chargeDetails[type='RG']/comment !=''">
											<xsl:value-of select="typicalCarData/rateInfo/chargeDetails[type='RG']/comment"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="typicalCarData/rateInfo/chargeDetails[type='RQ']/comment"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:if test="$rateText!=''">
									<RateDistance>
										<xsl:attribute name="Unlimited">
											<xsl:choose>
												<xsl:when test="contains($rateText, 'UNL')">true</xsl:when>
												<xsl:otherwise>false</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:attribute name="DistUnitName">
											<xsl:choose>
												<xsl:when test="contains($rateText, ' KM')">Km</xsl:when>
												<xsl:otherwise>Mile</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</RateDistance>
									<VehicleCharges>
										<VehicleCharge>
											<xsl:variable name="curr">
												<xsl:value-of select="typicalCarData/rateInfo[tariffInfo/amountType='904']/tariffInfo/currency"/>
											</xsl:variable>
											<xsl:attribute name="Amount">
												<xsl:choose>
													<xsl:when test="starts-with($rateText,$curr)">
														<xsl:variable name="rateText1">
															<xsl:choose>
																<xsl:when test="contains($rateText,'-')">
																	<xsl:value-of select="substring-before($rateText,'-')"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="$rateText"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:variable>
														<xsl:value-of select="translate(substring-before(substring($rateText1,4),'.'),'ABCDEFGHIJKLMNOPQRSTUVWXYZ*','')"/>
														<xsl:choose>
															<xsl:when test="number(substring(substring-after(substring($rateText1,4),'.'),2,1))">
																<xsl:value-of select="substring(substring-after(substring($rateText1,4),'.'),1,2)"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="concat(substring(substring-after(substring($rateText1,4),'.'),1,1),'0')"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:when>
													<xsl:otherwise>
														<xsl:choose>
															<xsl:when test="contains($rateText,'-')">
																<xsl:variable name="rateText1">
																	<xsl:choose>
																		<xsl:when test="contains(substring-before(substring($rateText,8),'-'),'.')">
																			<xsl:value-of select="translate(substring-before(substring($rateText,8),'-'),'.','')"/>
																		</xsl:when>
																		<xsl:otherwise>
																			<xsl:value-of select="concat(substring-before(substring($rateText,8),'-'),'00')"/>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:variable>
																<xsl:value-of select="translate($rateText1,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="substring-before(substring($rateText,8),'.')"/>
																<xsl:choose>
																	<xsl:when test="number(substring(substring-after(substring($rateText,8),'.'),2,1))">
																		<xsl:value-of select="substring(substring-after(substring($rateText,8),'.'),1,2)"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="concat(substring(substring-after(substring($rateText,8),'.'),1,1),'0')"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:attribute name="CurrencyCode">
												<xsl:choose>
													<xsl:when test="starts-with($rateText,$curr)">
														<xsl:value-of select="substring($rateText,1,3)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="substring($rateText,5,3)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
											<xsl:attribute name="TaxInclusive">false</xsl:attribute>
											<xsl:attribute name="GuaranteedInd">
												<xsl:choose>
													<xsl:when test="typicalCarData/rateInfo/chargeDetails[type='RG']">true</xsl:when>
													<xsl:otherwise>false</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:attribute name="Description">
												<xsl:choose>
													<xsl:when test="contains($rateText, 'HY')">Hourly Rate</xsl:when>
													<xsl:when test="contains($rateText, 'DY')">Daily Rate</xsl:when>
													<xsl:when test="contains($rateText, 'WY')">Weekly Rate</xsl:when>
													<xsl:when test="contains($rateText, 'MY')">Monthly Rate</xsl:when>
													<xsl:when test="contains($rateText, 'WD')">Weekend Rate</xsl:when>
													<xsl:when test="contains($rateText, 'PK')">Package Rate</xsl:when>
													<xsl:otherwise>Rental Period Rate</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</VehicleCharge>
									</VehicleCharges>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
						<!--xsl:if test="contains(generalOption/optionDetail[type='RG']/freetext, 'XD')">
							<xsl:variable name="ed">
								<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),'.',substring	(substring-after(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),1,2)),' ','')"/>
							</xsl:variable>
							<xsl:if test="$ed != '.'">
								<VehicleCharge> 
									<xsl:attribute name="TaxInclusive">false</xsl:attribute>
									<xsl:attribute name="Amount">	
			 							<xsl:value-of select="number(translate($ed,'.',''))"/>	
									</xsl:attribute>	
									<xsl:attribute name="CurrencyCode">			
										<xsl:choose>
											<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
												<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
											</xsl:otherwise>
										</xsl:choose>		
									</xsl:attribute>	
									<xsl:attribute name="DecimalPlaces">					
										<xsl:value-of select="string-length(substring-after($ed,'.'))"/>
									</xsl:attribute>			
									<xsl:attribute name="Description">Extra Day Rate</xsl:attribute>	
								</VehicleCharge>
							</xsl:if>
						</xsl:if>
						<xsl:if test="contains(generalOption/optionDetail[type='RG']/freetext, 'XH')">
							<xsl:variable name="eh">
								<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XH'),'.'),'.',substring	(substring-after(substring-after(generalOption/optionDetail	[type='RG']/freetext,'XH'),'.'),1,2)),' ','')"/>
							</xsl:variable>
							<xsl:if test="$eh != '.'">
								<VehicleCharge> 
									<xsl:attribute name="TaxInclusive">false</xsl:attribute>
			 						<xsl:attribute name="Amount">	
			 							<xsl:value-of select="translate($eh,'.','')"/>	
									</xsl:attribute>	
									<xsl:attribute name="CurrencyCode">			
										<xsl:choose>
											<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
												<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
											</xsl:otherwise>
										</xsl:choose>		
									</xsl:attribute>	
									<xsl:attribute name="DecimalPlaces">					
										<xsl:value-of select="string-length(substring-after($eh,'.'))"/>
									</xsl:attribute>			
									<xsl:attribute name="Description">Extra Hour Rate</xsl:attribute>	
								</VehicleCharge>
							</xsl:if>
						</xsl:if-->
						<xsl:if test="typicalCarData/rateCodeGroup/rateCodeInfo/fareCategories/fareType != ''">
							<RateQualifier>
								<xsl:if test="typicalCarData/rateInfo/rateInformation/category!=''">
									<xsl:attribute name="RateCategory">
										<xsl:choose>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='2'">Inclusive</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='6'">Convention</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='7'">Corporate</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='9'">Government</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='11'">Package</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='19'">Association</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='20'">Business</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='21'">Consortium</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='22'">Credential</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='23'">Industry</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/rateInformation/category='24'">Standard</xsl:when>
											<xsl:otherwise>General</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="RateQualifier">
									<xsl:value-of select="typicalCarData/rateCodeGroup/rateCodeInfo/fareCategories/fareType"/>
								</xsl:attribute>
								<xsl:attribute name="RatePeriod">
									<xsl:variable name="rateinfo" select="typicalCarData/rateInfo[tariffInfo/amountType='RG' or tariffInfo/amountType='RQ']"/>
									<xsl:choose>
										<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator != ''">
											<xsl:choose>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'HY'">Hourly</xsl:when>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'DY'">Daily</xsl:when>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WY'">Weekly</xsl:when>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'MY'">Monthly</xsl:when>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WD'">WeekendDay</xsl:when>
												<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'PK'">Package</xsl:when>
												<xsl:otherwise>Other</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'DY')">Daily</xsl:when>
										<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'WY')">Weekly</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'HY')">Hourly</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'DY')">Daily</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'WY')">Weekly</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'MY')">Monthly</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'WD')">WeekendDay</xsl:when>
										<xsl:when test="contains(typicalCarData/rateInfo/chargeDetails[type='RG']/comment, 'PK')">Package</xsl:when>
										<xsl:otherwise>Other</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</RateQualifier>
						</xsl:if>
					</RentalRate>
				</xsl:if>
				<xsl:variable name="total">
					<xsl:choose>
						<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = '904']/amount != ''">
							<xsl:value-of select="translate(typicalCarData/rateInfo/tariffInfo[amountType = '904']/amount,'.','')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(concat(substring-before(substring(generalOption/optionDetail[type='ES']/freetext,8),'.'),'.',substring(substring-after(generalOption/optionDetail[type='ES']/freetext,'.'),1,2)),' ','')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:if test="$total != '.'">
					<TotalCharge>
						<xsl:attribute name="RateTotalAmount">
							<xsl:value-of select="$total"/>
						</xsl:attribute>
						<xsl:attribute name="EstimatedTotalAmount">
							<xsl:value-of select="$total"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency != ''">
									<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RB']/currency"/>
								</xsl:when>
								<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = '904']/currency != ''">
									<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = '904']/currency"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(generalOption/optionDetail[type='RG']/freetext,5,3)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</TotalCharge>
				</xsl:if>
				<TPA_Extensions>
					<CarOptions>
						<xsl:apply-templates select="generalOption/optionDetail" mode="car"/>
						<xsl:if test="typicalCarData/customerInfo/customerReferences[referenceQualifier='CD']">
							<CarOption>
								<xsl:attribute name="Option">Corporate Discount</xsl:attribute>
								<Text>
									<xsl:value-of select="typicalCarData/customerInfo/customerReferences[referenceQualifier='CD']/referenceNumber"/>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:if test="typicalCarData/customerInfo/customerReferences[referenceQualifier='1']">
							<CarOption>
								<xsl:attribute name="Option">Customer ID</xsl:attribute>
								<Text>
									<xsl:value-of select="typicalCarData/customerInfo/customerReferences[referenceQualifier='1']/referenceNumber"/>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:if test="typicalCarData/bookingSource/originatorDetails/originatorId!=''">
							<CarOption>
								<xsl:attribute name="Option">Booking Source</xsl:attribute>
								<Text>
									<xsl:value-of select="typicalCarData/bookingSource/originatorDetails/originatorId"/>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:if test="typicalCarData/supleInfo/remarkDetails[type='ARR']">
							<CarOption>
								<xsl:attribute name="Option">Additional Info</xsl:attribute>
								<Text>
									<xsl:text>ARRIVES </xsl:text>
									<xsl:value-of select="typicalCarData/supleInfo/remarkDetails[type='ARR']/freetext"/>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:if test="typicalCarData/attribute[criteriaSetType='BAT']">
							<CarOption>
								<xsl:attribute name="Option">Additional Info</xsl:attribute>
								<Text>
									<xsl:choose>
										<xsl:when test="typicalCarData/attribute[criteriaSetType='BAT']/criteriaDetails/attributeType='COR'">CORPORATE BOOKING</xsl:when>
										<xsl:otherwise>LEISURE BOOKING</xsl:otherwise>
									</xsl:choose>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:if test="typicalCarData/supleInfo/remarkDetails[type='CNM']">
							<CarOption>
								<xsl:attribute name="Option">Reservation Last and First Names</xsl:attribute>
								<Text>
									<xsl:value-of select="typicalCarData/supleInfo/remarkDetails[type='CNM']/freetext"/>
								</Text>
							</CarOption>
						</xsl:if>
						<xsl:for-each select="itineraryFreetext[longFreetext != '']">
							<CarOption>
								<xsl:attribute name="Option">Marketing Text</xsl:attribute>
								<Text>
									<xsl:value-of select="longFreetext"/>
								</Text>
							</CarOption>
						</xsl:for-each>
						<xsl:if test="typicalCarData/marketingInfo[freetextDetail/type='MK']">
							<xsl:for-each select="typicalCarData/marketingInfo[freetextDetail/type='MK']">
								<xsl:for-each select="text">
									<CarOption>
										<xsl:attribute name="Option">Marketing Text</xsl:attribute>
										<Text>
											<xsl:value-of select="."/>
										</Text>
									</CarOption>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:if>
						<xsl:if test="typicalCarData/billingData/billingInfo[billingQualifier='902']">
							<CarOption>
								<xsl:attribute name="Option">Voucher Number</xsl:attribute>
								<Text>
									<xsl:value-of select="typicalCarData/billingData/billingInfo[billingQualifier='902']/billingDetails" />
								</Text>
							</CarOption>
						</xsl:if>
					</CarOptions>
				</TPA_Extensions>
			</Vehicle>
		</Item>
	</xsl:template>
	<!-- -->
	<xsl:template match="typicalCarData">
		<xsl:variable name="rateinfo" select="rateInfo[tariffInfo/amountType='RG' or tariffInfo/amountType='RQ']"/>
		<RateDistance>
			<xsl:attribute name="Unlimited">
				<xsl:choose>
					<xsl:when test="$rateinfo/chargeDetails/description = 'UNL'">true</xsl:when>
					<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'UNL')">true</xsl:when>
					<xsl:when test="contains($rateinfo/chargeDetails[type='RG' or type='RQ']/comment, 'UNL')">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="DistUnitName">
				<xsl:choose>
					<xsl:when test="$rateinfo[chargeDetails/type='31']">Mile</xsl:when>
					<xsl:when test="$rateinfo[chargeDetails/type='32']">Km</xsl:when>
					<xsl:otherwise>Mile</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</RateDistance>
		<VehicleCharges>
			<VehicleCharge>
				<xsl:attribute name="Amount">
					<xsl:value-of select="translate($rateinfo/tariffInfo/amount,'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="$rateinfo/tariffInfo/currency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="string-length(substring-after($rateinfo/tariffInfo/amount,'.'))"/>
				</xsl:attribute>
				<xsl:attribute name="TaxInclusive">false</xsl:attribute>
				<xsl:attribute name="Description">
					<xsl:choose>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'HY'">Hourly Rate</xsl:when>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'DY'">Daily Rate</xsl:when>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WY'">Weekly Rate</xsl:when>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'MY'">Monthly Rate</xsl:when>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'WD'">Weekend Rate</xsl:when>
						<xsl:when test="$rateinfo/tariffInfo/ratePlanIndicator = 'PK'">Package Rate</xsl:when>
						<xsl:otherwise>Rental Period Rate</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="GuaranteedInd">
					<xsl:choose>
						<xsl:when test="$rateinfo/tariffInfo[amountType = 'RG']/amount != ''">true</xsl:when>
						<xsl:otherwise>false</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</VehicleCharge>
			<xsl:if test="$rateinfo/chargeDetails[type='8']">
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate($rateinfo/chargeDetails[type='8']/amount,'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after($rateinfo/chargeDetails[type='8']/amount,'.'))"/>
					</xsl:attribute>
					<xsl:attribute name="TaxInclusive">false</xsl:attribute>
					<xsl:attribute name="Description">Extra Day Rate</xsl:attribute>
				</VehicleCharge>
			</xsl:if>
			<xsl:if test="$rateinfo/chargeDetails[type='9']">
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate($rateinfo/chargeDetails[type='9']/amount,'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after($rateinfo/chargeDetails[type='9']/amount,'.'))"/>
					</xsl:attribute>
					<xsl:attribute name="TaxInclusive">false</xsl:attribute>
					<xsl:attribute name="Description">Extra Hour Rate</xsl:attribute>
				</VehicleCharge>
			</xsl:if>
			<xsl:if test="$rateinfo[chargeDetails/type='31'] or $rateinfo[chargeDetails/type='32']">
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate($rateinfo[chargeDetails/type='31' or chargeDetails/type='32']/chargeDetails/amount,'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after($rateinfo[chargeDetails/type='31' or chargeDetails/type='32']/chargeDetails/amount,'.'))"/>
					</xsl:attribute>
					<xsl:attribute name="TaxInclusive">false</xsl:attribute>
					<xsl:attribute name="Description">Over Allowance Rate</xsl:attribute>
				</VehicleCharge>
			</xsl:if>
		</VehicleCharges>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Car Options Fields										-->
	<!--************************************************************************************-->
	<xsl:template match="optionDetail" mode="car">
		<xsl:choose>
			<xsl:when test="type = 'BS'">
				<CarOption>
					<xsl:attribute name="Option">Booking Source</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RQ'">
				<CarOption>
					<xsl:attribute name="Option">Quoted Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RB'">
				<CarOption>
					<xsl:attribute name="Option">Base Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RC'">
				<CarOption>
					<xsl:attribute name="Option">Rate Code</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RG'">
				<CarOption>
					<xsl:attribute name="Option">Guaranteed Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'NM'">
				<CarOption>
					<xsl:attribute name="Option">Reservation Last and First Names</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ES'">
				<CarOption>
					<xsl:attribute name="Option">Estimated Total Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ARR'">
				<CarOption>
					<xsl:attribute name="Option">Pick Up Time</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RT'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Time</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ID'">
				<CarOption>
					<xsl:attribute name="Option">Customer ID</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'GT'">
				<CarOption>
					<xsl:attribute name="Option">Payment Guarantee</xsl:attribute>
					<xsl:choose>
						<xsl:when test="contains(freetext, 'EXP')">
							<Text>
								<xsl:value-of select="substring(freetext, 1, 2)"/>
								<xsl:value-of select="substring-before(substring(freetext, 3, string-length(freetext) - 4), 'EXP')"/>
								<xsl:value-of select="substring(substring-after(freetext,'EXP'), 1, 2)"/>
								<xsl:value-of select="substring(substring-after(freetext,'EXP'), 3, 2)"/>
							</Text>
						</xsl:when>
					</xsl:choose>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'PU'">
				<CarOption>
					<xsl:attribute name="Option">Pick Up Location</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'AD'">
				<CarOption>
					<xsl:attribute name="Option">Customer Address</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'CD'">
				<CarOption>
					<xsl:attribute name="Option">Corporate Discount</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'SQ'">
				<CarOption>
					<xsl:attribute name="Option">Optional Equipment</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'PR'">
				<CarOption>
					<xsl:attribute name="Option">PrePayment Info</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DL'">
				<CarOption>
					<xsl:attribute name="Option">Driver License</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'FT'">
				<CarOption>
					<xsl:attribute name="Option">Frequent Traveler Number</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'SI'">
				<CarOption>
					<xsl:attribute name="Option">Additional Info</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DC'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Charge</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'VC'">
				<CarOption>
					<xsl:attribute name="Option">Merchant Currency</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'AC'">
				<CarOption>
					<xsl:attribute name="Option">Alternate Currency</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DO'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Location</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'TN'">
				<CarOption>
					<xsl:attribute name="Option">Tour Number</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext"/>
					</Text>
				</CarOption>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- ****************************************************************************************************************** -->
	<!-- PNR Data Elements   	              								                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Phone Fields	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:PhoneNumber" mode="phone">
		<xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'" />
		<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />

		<Telephone>
			<xsl:attribute name="PhoneUseType">
				<xsl:choose>
					<xsl:when test="@Type">
						<xsl:value-of select="translate(@Type, $lowercase, $uppercase)"/>
					</xsl:when>
					<xsl:otherwise>O</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="PhoneNumber">
				<xsl:value-of select="@Number"/>
			</xsl:attribute>
		</Telephone>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- Ticketing Element	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:ActionStatus" mode="ticketing">
		<Ticketing>
			<xsl:if test="@TicketDate">
				<xsl:attribute name="TicketTimeLimit">
					<xsl:value-of select="@TicketDate"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="TicketType">eTicket</xsl:attribute>
			<xsl:if test="@Type = 'TAW'">
				<TicketAdvisory>
					<xsl:text>TL-</xsl:text>
					<xsl:value-of select="@TicketDate"/>
					<xsl:text>/</xsl:text>
					<xsl:value-of select="../universal:ProviderReservationInfo/@OwningPCC"/>
				</TicketAdvisory>
			</xsl:if>
			<xsl:if test="common_v50_0:Remark">
				<TicketAdvisory>
					<xsl:text>T/</xsl:text>
					<xsl:value-of select="common_v50_0:Remark"/>
				</TicketAdvisory>
			</xsl:if>
		</Ticketing>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Ticketing Remark	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ticketingremark">
		<xsl:if test="ticketElement/ticket/freetext">
			<UniqueRemark>
				<xsl:attribute name="RemarkType">Ticketing</xsl:attribute>
				<xsl:value-of select="normalize-space(substring(ticketElement/ticket/freetext, 2, 15))"/>
			</UniqueRemark>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Email Address	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:Email" mode="email">
		<!--done -->
		<Email>
			<xsl:value-of select="@EmailID"/>
		</Email>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Form of Payment	   		                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:FormOfPayment" mode="Payment">
		<FormOfPayment>
			<xsl:attribute name="RPH">1</xsl:attribute>
			<xsl:choose>
				<xsl:when test="common_v50_0:CreditCard">
					<PaymentCard>
						<xsl:attribute name="CardCode">
							<xsl:choose>
								<xsl:when test="common_v50_0:CreditCard/@Type = 'CA'">MC</xsl:when>
								<xsl:when test="common_v50_0:CreditCard/@Type = 'DC'">DN</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="common_v50_0:CreditCard/@Type"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CardNumber">
							<xsl:value-of select="common_v50_0:CreditCard/@Number"/>
						</xsl:attribute>
						<xsl:attribute name="ExpireDate">
							<xsl:value-of select="concat(substring(common_v50_0:CreditCard/@ExpDate,6),substring(common_v50_0:CreditCard/@ExpDate,3,2))"/>
						</xsl:attribute>
					</PaymentCard>
				</xsl:when>
				<xsl:otherwise>
					<DirectBill DirectBill_ID="Check" />
					<TPA_Extensions FOPType="CHECK" />
				</xsl:otherwise>
			</xsl:choose>
		</FormOfPayment>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- MCO	   		                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="MCO">
		<FormOfPayment>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<MiscChargeOrder>
				<xsl:attribute name="TravelerRefNumber">
					<xsl:variable name="pax">
						<xsl:value-of select="referenceForDataElement/reference/number"/>
					</xsl:variable>
					<xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number = $pax]/elementManagementPassenger/lineNumber"/>
				</xsl:attribute>
				<xsl:value-of select="mcoRecord/mcoInformation/freeText"/>
			</MiscChargeOrder>
			<xsl:if test="mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FM' or (mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FA' or mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FB' or mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FI') or mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FP' and starts-with(fareElementData/freeText,'CC') and string-length(fareElementData/freeText) &gt; 10 and substring(fareElementData/freeText,3,2)!='ZZ']">
				<TPA_Extensions>
					<xsl:for-each select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FM']">
						<AgencyCommission>
							<xsl:choose>
								<xsl:when test="contains(fareElementData/freeText,'*M*') or contains(fareElementData/freeText,'*C*') or contains(fareElementData/freeText,'*G*') or contains(fareElementData/freeText,'*F*')">
									<xsl:variable name="comm1">
										<xsl:choose>
											<xsl:when test="contains(fareElementData/freeText,'*M*')">
												<xsl:value-of select="substring-after(fareElementData/freeText,'*M*')"/>
											</xsl:when>
											<xsl:when test="contains(fareElementData/freeText,'*G*')">
												<xsl:value-of select="substring-after(fareElementData/freeText,'*G*')"/>
											</xsl:when>
											<xsl:when test="contains(fareElementData/freeText,'*F*')">
												<xsl:value-of select="substring-after(fareElementData/freeText,'*F*')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring-after(fareElementData/freeText,'*C*')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="comm">
										<xsl:choose>
											<xsl:when test="contains($comm1,'XO')">
												<xsl:value-of select="substring-before($comm1,'/')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$comm1"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="contains($comm,'A')">
											<xsl:attribute name="Amount">
												<xsl:value-of select="substring-before($comm,'A')"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Percent">
												<xsl:value-of select="$comm"/>
											</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="contains(fareElementData/freeText,'*AM*')">
									<xsl:attribute name="Amount">
										<xsl:value-of select="substring-after(fareElementData/freeText,'*AM*')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(fareElementData/freeText,'A')">
									<xsl:attribute name="Amount">
										<xsl:value-of select="substring-before(fareElementData/freeText,'A')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Percent">
										<xsl:value-of select="fareElementData/freeText"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</AgencyCommission>
					</xsl:for-each>
					<xsl:if test="mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FA' or mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FB' or mcoRecord/groupOfFareElements/fareElementData/freeTextDetails/informationType='FI'">
						<IssuedTickets>
							<xsl:for-each select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FA' or fareElementData/freeTextDetails/informationType='FB']">
								<IssuedTicket>
									<xsl:value-of select="fareElementData/freeText"/>
								</IssuedTicket>
							</xsl:for-each>
							<xsl:for-each select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FI']">
								<IssuedInvoice>
									<xsl:value-of select="fareElementData/freeText"/>
								</IssuedInvoice>
							</xsl:for-each>
						</IssuedTickets>
					</xsl:if>
					<xsl:for-each select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FP' and starts-with(fareElementData/freeText,'CC') and string-length(fareElementData/freeText) &gt; 10 and substring(fareElementData/freeText,3,2)!='ZZ']">
						<PaymentCard>
							<xsl:attribute name="CardCode">
								<xsl:choose>
									<xsl:when test="substring(substring-after(fareElementData/freeText,'CC'),1,2) = 'CA'">MC</xsl:when>
									<xsl:when test="substring(substring-after(fareElementData/freeText,'CC'),1,2) = 'MA'">MC</xsl:when>
									<xsl:when test="substring(substring-after(fareElementData/freeText,'CC'),1,2) = 'DC'">DN</xsl:when>
									<xsl:when test="substring(fareElementData/freeText,4,1) = '/'">
										<xsl:variable name="cc">
											<xsl:value-of select="substring(fareElementData/freeText,2,2)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$cc='CA'">MC</xsl:when>
											<xsl:when test="$cc='MA'">MC</xsl:when>
											<xsl:when test="$cc='DC'">DN</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$cc"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="substring(fareElementData/freeText,4,1) = '-'">
										<xsl:variable name="cc">
											<xsl:value-of select="substring(fareElementData/freeText,2,2)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$cc='CA'">MC</xsl:when>
											<xsl:when test="$cc='MA'">MC</xsl:when>
											<xsl:when test="$cc='DC'">DN</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$cc"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring(substring-after(fareElementData/freeText,'CC'),1,2)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CardNumber">
								<xsl:variable name="cardnum">
									<xsl:choose>
										<xsl:when test="substring(fareElementData/freeText,4,1) = '-'">
											<xsl:value-of select="substring-before(substring-after(fareElementData/freeText,'/'),'T')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate(substring(substring-after(fareElementData/freeText,'CC'),3,16),'/','')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="$cardnum"/>
							</xsl:attribute>
							<xsl:attribute name="ExpireDate">
								<xsl:choose>
									<xsl:when test="substring(fareElementData/freeText,4,1) = '-'">
										<xsl:value-of select="substring(substring-after(fareElementData/freeText,'T/'),1,2)"/>
										<xsl:value-of select="substring(substring-after(fareElementData/freeText,'T/'),3,2)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring(substring-after(substring(fareElementData/freeText,5),'/'),1,2)"/>
										<xsl:value-of select="substring(substring-after(substring(fareElementData/freeText,5),'/'),3,2)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:if test="contains(substring-after(substring(fareElementData/freeText,5),'/'),'/N')">
								<xsl:attribute name="ConfirmationNumber">
									<xsl:variable name="cn">
										<xsl:value-of select="substring-after(substring-after(substring(fareElementData/freeText,5),'/'),'/N')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="contains($cn,'*')">
											<xsl:value-of select="concat('N',substring-before($cn,'*'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('N',$cn)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</xsl:if>
						</PaymentCard>
					</xsl:for-each>
				</TPA_Extensions>
			</xsl:if>
		</FormOfPayment>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Address	   		                                   		 -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:Address" mode="Address">
		<Address>
			<xsl:attribute name="UseType">
				<xsl:choose>
					<xsl:when test="elementManagementData/segmentName = 'AB/'">Billing</xsl:when>
					<xsl:when test="elementManagementData/segmentName = 'AB'">Billing</xsl:when>
					<xsl:otherwise>Mailing</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>

			<xsl:choose>
				<xsl:when test="elementManagementData/segmentName = 'AB/' or elementManagementData/segmentName = 'AM/'">
					<xsl:apply-templates select="structuredAddress"/>
				</xsl:when>
				<xsl:otherwise>
					<AddressLine>
						<xsl:variable name="na">
							<xsl:choose>
								<xsl:when test="contains(otherDataFreetext/longFreetext,'NA-')">
									<xsl:value-of select="substring-before(otherDataFreetext/longFreetext,'NA-')"/>
									<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'NA-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="otherDataFreetext/longFreetext"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="a1">
							<xsl:choose>
								<xsl:when test="contains($na,'A1-')">
									<xsl:value-of select="substring-before($na,'A1-')"/>
									<xsl:value-of select="substring-after($na,'A1-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$na"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="a2">
							<xsl:choose>
								<xsl:when test="contains($a1,'A2-')">
									<xsl:value-of select="substring-before($a1,'A2-')"/>
									<xsl:value-of select="substring-after($a1,'A2-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$a1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="zp">
							<xsl:choose>
								<xsl:when test="contains($a2,'ZP-')">
									<xsl:value-of select="substring-before($a2,'ZP-')"/>
									<xsl:value-of select="substring-after($a2,'ZP-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$a2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="ci">
							<xsl:choose>
								<xsl:when test="contains($zp,'CI-')">
									<xsl:value-of select="substring-before($zp,'CI-')"/>
									<xsl:value-of select="substring-after($zp,'CI-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$zp"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="st">
							<xsl:choose>
								<xsl:when test="contains($ci,'ST-')">
									<xsl:value-of select="substring-before($ci,'ST-')"/>
									<xsl:value-of select="substring-after($ci,'ST-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$ci"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="co">
							<xsl:choose>
								<xsl:when test="contains($st,'CO-')">
									<xsl:value-of select="substring-before($st,'CO-')"/>
									<xsl:value-of select="substring-after($st,'CO-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$st"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:value-of select="$co"/>
					</AddressLine>
				</xsl:otherwise>
			</xsl:choose>
		</Address>
	</xsl:template>
	<xsl:template match="structuredAddress">
		<xsl:if test="address[option='A1']">
			<StreetNmbr>
				<xsl:value-of select="address[option='A1']/optionText"/>
			</StreetNmbr>
		</xsl:if>
		<xsl:if test="address[option='A2']">
			<BldgRoom>
				<xsl:value-of select="address[option='A2']/optionText"/>
			</BldgRoom>
		</xsl:if>
		<xsl:if test="address[option='CI']">
			<CityName>
				<xsl:value-of select="address[option='CI']/optionText"/>
			</CityName>
		</xsl:if>
		<xsl:if test="address[option='ZP']">
			<PostalCode>
				<xsl:value-of select="address[option='ZP']/optionText"/>
			</PostalCode>
		</xsl:if>
		<xsl:if test="address[option='ST']">
			<StateProv>
				<xsl:choose>
					<xsl:when test="string-length(address[option='ST']/optionText) = 2">
						<xsl:attribute name="StateCode">
							<xsl:value-of select="address[option='ST']/optionText"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="address[option='ST']/optionText"/>
					</xsl:otherwise>
				</xsl:choose>
			</StateProv>
		</xsl:if>
		<xsl:if test="address[option='CO']">
			<CountryName>
				<xsl:choose>
					<xsl:when test="string-length(address[option='CO']/optionText) = 2">
						<xsl:attribute name="Code">
							<xsl:value-of select="address[option='CO']/optionText"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="address[option='CO']/optionText"/>
					</xsl:otherwise>
				</xsl:choose>
			</CountryName>
		</xsl:if>
		<xsl:if test="address[option='NA']">
			<ContactName>
				<xsl:value-of select="address[option='NA']/optionText"/>
			</ContactName>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Frequent Flyer                      		     		 -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Fqtv">
		<xsl:param name="paxref"/>
		<xsl:if test="referenceForDataElement/reference[qualifier='PT']/number = $paxref">
			<CustLoyalty>
				<xsl:attribute name="ProgramID">
					<xsl:value-of select="frequentTravellerInfo/frequentTraveler/company"/>
				</xsl:attribute>
				<xsl:attribute name="MembershipID">
					<xsl:value-of select="frequentTravellerInfo/frequentTraveler/membershipNumber"/>
				</xsl:attribute>
			</CustLoyalty>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- General Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:GeneralRemark" mode="GenRemark">
		<Remark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:if test="miscellaneousRemarks/remarks/category != ''">
				<xsl:attribute name="Category">
					<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="common_v50_0:RemarkData"/>
		</Remark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--Itinerary Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ItinRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
			<FlightRefNumber>
				<xsl:attribute name="RPH">
					<xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
				</xsl:attribute>
			</FlightRefNumber>
			<Text>
				<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Invoice Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="InvoiceRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:attribute name="RemarkType">Invoice</xsl:attribute>
			<FlightRefNumber>
				<xsl:attribute name="RPH">
					<xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
				</xsl:attribute>
			</FlightRefNumber>
			<Text>
				<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Accounting Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:AccountingRemark" mode="AccountingRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="RemarkType">Invoice</xsl:attribute>
			<Text>
				<xsl:value-of select="common_v50_0:RemarkData"/>
			</Text>
			<RemarkOrigin>Vendor</RemarkOrigin>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Invoice and itinerary Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="InvoiceItinRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="referenceForDataElement/reference/qualifier='ST'">
					<xsl:attribute name="RemarkType">
						<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
					</xsl:attribute>
					<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:variable name="rph">
								<xsl:value-of select="referenceForDataElement/reference[qualifier='ST']/number"/>
							</xsl:variable>
							<xsl:value-of select="../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
						</xsl:attribute>
					</FlightRefNumber>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="RemarkType">
						<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
					</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text>
				<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Historical Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:GeneralRemark" mode="HistoricalRemark">
		<Remark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Category">
				<xsl:text>Historical</xsl:text>
			</xsl:attribute>

			<xsl:value-of select="common_v50_0:RemarkData"/>
		</Remark>
	</xsl:template>
	<xsl:template match="common_v50_0:GeneralRemark" mode="VendorRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="RemarkType">
				<xsl:value-of select="@Category"/>
			</xsl:attribute>
			<Text>
				<xsl:value-of select="common_v50_0:RemarkData"/>
			</Text>
			<Vendor>
				<xsl:attribute name="TravelSector">
					<xsl:value-of select="@SupplierType"/>
				</xsl:attribute>
				<xsl:attribute name="Code">
					<xsl:value-of select="@SupplierCode"/>
				</xsl:attribute>
			</Vendor>
			<RemarkDateTime>
				<xsl:value-of select="@CreateDate"/>
			</RemarkDateTime>
			<RemarkOrigin>
				<xsl:value-of select="@Category"/>
			</RemarkOrigin>
		</SpecialRemark>
	</xsl:template>
	<xsl:template match="common_v50_0:GeneralRemark" mode="GenRemark">
		<Remark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:if test="miscellaneousRemarks/remarks/category != ''">
				<xsl:attribute name="Category">
					<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="common_v50_0:RemarkData"/>
		</Remark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Endorsement Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:Endorsement" mode="Endorsement">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="../../air:BookingInfo">
					<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
					<TravelerRefNumber>
						<xsl:attribute name="RPH">
							<xsl:for-each select="../../air:PassengerType">
								<xsl:variable name="paxref">
									<xsl:value-of select="@BookingTravelerRef"/>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:call-template name="paxNumber">
									<xsl:with-param name="key" select="$paxref" />
								</xsl:call-template>
							</xsl:for-each>
						</xsl:attribute>
					</TravelerRefNumber>

					<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:for-each select="../../air:BookingInfo">
								<xsl:variable name="segref">
									<xsl:value-of select="@SegmentRef"/>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:call-template name="fltNumber">
									<xsl:with-param name="key" select="$segref" />
								</xsl:call-template>
							</xsl:for-each>
						</xsl:attribute>
					</FlightRefNumber>

				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text>
				<xsl:value-of select="@Value"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Controlling Carrier  Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="air:AirPricingInfo" mode="CC">
		<xsl:variable name="cc" select="air:FareInfo/air:Brand" />
		<xsl:variable name="gqd" select="." />
		<xsl:variable name="segs" select="air:FareInfo" />

		<xsl:variable name="lCarr">
			<xsl:for-each select="$cc/@Carrier[generate-id() = generate-id(key('conCarr',.)[1])]">
				<xsl:if test=". !=''">
					<xsl:value-of select="concat(., ',')"/>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>

		<xsl:variable name="elems">
			<xsl:call-template name="tokenizeString">
				<xsl:with-param name="list" select="$lCarr"/>
				<xsl:with-param name="delimiter" select="','"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:for-each select="msxsl:node-set($elems)/elem">

			<xsl:variable name="al">
				<xsl:value-of select="text()"/>
			</xsl:variable>

			<SpecialRemark>
				<xsl:attribute name="RPH">
					<xsl:value-of select="$al"/>
				</xsl:attribute>
				<xsl:attribute name="RemarkType">CC</xsl:attribute>

				<FlightRefNumber>
					<xsl:attribute name="RPH">
						<xsl:for-each select="$segs[air:Brand/@Carrier=$al]">
							<xsl:variable name="rph">
								<xsl:value-of select="@Key"/>
							</xsl:variable>
							<xsl:if test="position() > 1">
								<xsl:text> </xsl:text>
							</xsl:if>
							<xsl:call-template name="fltNumber">
								<xsl:with-param name="key" select="../air:BookingInfo[@FareInfoRef=$rph]/@SegmentRef" />
							</xsl:call-template>
						</xsl:for-each>
					</xsl:attribute>
				</FlightRefNumber>

				<xsl:variable name="fltPath">
					<xsl:for-each select="$segs[air:Brand/@Carrier=$al]">
						<xsl:variable name="sN" select="node()[1]" />
						<xsl:variable name="port">
							<xsl:value-of select="concat(@Origin,@Destination)"/>
						</xsl:variable>

						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="$port"/>
					</xsl:for-each>
				</xsl:variable>

				<Text>
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="concat($fltPath,' -/', $al)" />
					</xsl:call-template>
				</Text>
			</SpecialRemark>

		</xsl:for-each>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- TourCode Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="TourCode">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="referenceForDataElement/reference/qualifier='ST' or referenceForDataElement/reference/qualifier='PT'">
					<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
					<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
						<TravelerRefNumber>
							<xsl:attribute name="RPH">
								<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
									<xsl:variable name="rph">
										<xsl:value-of select="."/>
									</xsl:variable>
									<xsl:if test="position() > 1">
										<xsl:text> </xsl:text>
									</xsl:if>
									<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
								</xsl:for-each>
							</xsl:attribute>
						</TravelerRefNumber>
					</xsl:if>
					<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
						<FlightRefNumber>
							<xsl:attribute name="RPH">
								<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
									<xsl:variable name="rph">
										<xsl:value-of select="."/>
									</xsl:variable>
									<xsl:if test="position() > 1">
										<xsl:text> </xsl:text>
									</xsl:if>
									<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
								</xsl:for-each>
							</xsl:attribute>
						</FlightRefNumber>
					</xsl:if>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text>
				<xsl:value-of select="otherDataFreetext/longFreetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- OSI		   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="OSI">
		<OtherServiceInformation>
			<xsl:if test="otherDataFreetext/freetextDetail/type='28'">
				<Airline>
					<xsl:attribute name="Code">
						<xsl:value-of select="otherDataFreetext/freetextDetail/companyId"/>
					</xsl:attribute>
				</Airline>
				<Text>
					<xsl:value-of select="otherDataFreetext/longFreetext"/>
				</Text>
			</xsl:if>
			<xsl:if test="otherDataFreetext/freetextDetail/type='P27'">
				<Text>xsl:value-of select="otherDataFreetext/longFreetext"/></Text>
			</xsl:if>
		</OtherServiceInformation>
	</xsl:template>
	<xsl:template match="reference" mode="OSIPassAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:attribute name="RPH">
			<xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
		</xsl:attribute>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- SSR Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="common_v50_0:SSR" mode="SSR">
		<SpecialServiceRequest>
			<xsl:attribute name="SSRCode">
				<xsl:value-of select="@Type"/>
			</xsl:attribute>
			<xsl:if test="referenceForDataElement/reference[qualifier = 'PT']/number != ''">
				<xsl:variable name="ref">
					<xsl:value-of select="referenceForDataElement/reference[qualifier = 'PT']/number"/>
				</xsl:variable>
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:value-of select="../../travellerInfo/elementManagementPassenger[reference/number = $ref]/lineNumber"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference[qualifier = 'ST']/number != ''">
				<xsl:variable name="ref">
					<xsl:value-of select="referenceForDataElement/reference[qualifier = 'ST']/number"/>
				</xsl:variable>
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:value-of select="../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $ref]/lineNumber"/>
				</xsl:attribute>
			</xsl:if>
			<Airline>
				<xsl:attribute name="Code">
					<xsl:value-of select="@Carrier"/>
				</xsl:attribute>
				<xsl:attribute name="CodeContext">
					<xsl:value-of select="@Status"/>
				</xsl:attribute>
			</Airline>
			<Text>
				<xsl:value-of select="@FreeText"/>
			</Text>
		</SpecialServiceRequest>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Seat Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Seat">
		<xsl:choose>
			<xsl:when test="referenceForDataElement/reference[qualifier='PT']">
				<xsl:for-each select="serviceRequest/ssrb">
					<xsl:variable name="pos">
						<xsl:value-of select="position()"/>
					</xsl:variable>
					<SeatRequest>
						<xsl:if test="data != ''">
							<xsl:attribute name="SeatNumber">
								<xsl:value-of select="data"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="seatType[position()=2] != ''">
							<xsl:attribute name="SeatPreference">
								<xsl:value-of select="seatType[position()=2]"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="SmokingAllowed">false</xsl:attribute>
						<xsl:attribute name="Status">
							<xsl:value-of select="../ssr/status"/>
						</xsl:attribute>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:variable name="ref">
								<xsl:value-of select="../../referenceForDataElement/reference[qualifier = 'PT'][position()=$pos]/number"/>
							</xsl:variable>
							<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number = $ref]/lineNumber"/>
							<!--xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='PT']" mode="SeatPassAssoc"/-->
						</xsl:attribute>
						<xsl:attribute name="FlightRefNumberRPHList">
							<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
						</xsl:attribute>
						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="../ssr/boardpoint"/>
							</xsl:attribute>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="../ssr/offpoint"/>
							</xsl:attribute>
						</ArrivalAirport>
					</SeatRequest>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="serviceRequest/ssrb[data != '']">
					<SeatRequest>
						<xsl:attribute name="SeatNumber">
							<xsl:value-of select="data"/>
						</xsl:attribute>
						<xsl:if test="seatType[position()=2] != ''">
							<xsl:attribute name="SeatPreference">
								<xsl:value-of select="seatType[position()=2]"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="SmokingAllowed">false</xsl:attribute>
						<xsl:attribute name="Status">
							<xsl:value-of select="../ssr/status"/>
						</xsl:attribute>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:value-of select="position()"/>
						</xsl:attribute>
						<xsl:attribute name="FlightRefNumberRPHList">
							<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
						</xsl:attribute>
						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="../ssr/boardpoint"/>
							</xsl:attribute>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="../ssr/offpoint"/>
							</xsl:attribute>
						</ArrivalAirport>
					</SeatRequest>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Issued Tickets Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="IssuedTicket">
		<IssuedTicket>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</IssuedTicket>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Automated Tickets Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="AutomatedTicket">
		<AutomatedTicket>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</AutomatedTicket>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Electronic Tickets Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ElectronicTicket">
		<ElectronicTicket>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</ElectronicTicket>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Manual Tickets Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ManualTicket">
		<ManualTicket>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</ManualTicket>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- IssuedInvoice Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="IssuedInvoice">
		<IssuedInvoice>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../travellerInfo/elementManagementPassenger[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']/number">
						<xsl:variable name="rph">
							<xsl:value-of select="."/>
						</xsl:variable>
						<xsl:if test="position() > 1">
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</IssuedInvoice>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- ExchangeDocument Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ExchangeDocument">
		<ExchangeDocument>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</ExchangeDocument>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- TicketingCarrier Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="TicketingCarrier">
		<TicketingCarrier>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</TicketingCarrier>
	</xsl:template>
	<xsl:template match="reference" mode="SSRPassAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:attribute name="TravelerRefNumberRPHList">
			<xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="reference" mode="SeatPassAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:if test="position() > 1">
			<xsl:value-of select="string(' ')"/>
		</xsl:if>
		<xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
	</xsl:template>
	<xsl:template match="reference" mode="SeatSegAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:if test="position() > 1">
			<xsl:value-of select="string(' ')"/>
		</xsl:if>
		<xsl:value-of select="//originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number=$Tattoo]/lineNumber"/>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Form of Payment	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="fop">
		<xsl:if test="ElementType='FP'">
			<FOP>
				<xsl:if test="CAPI_PNR_NonFormattedElement/InformationType='16'">
					<ElementNumber>
						<xsl:attribute name="TattooNumber">
							<xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
						</xsl:attribute>
						<xsl:attribute name="TattooQualifier">
							<xsl:value-of select="elementManagementData/reference/qualifier"/>
						</xsl:attribute>
						<xsl:value-of select="ElementNum"/>
					</ElementNumber>
					<xsl:choose>
						<xsl:when test="substring(string(CAPI_PNR_NonFormattedElement/InformationText),1,2)='CC'">
							<CC>
								<CCCode>
									<xsl:value-of select="substring(string(CAPI_PNR_NonFormattedElement/InformationText),3,2)"/>
								</CCCode>
								<CCNo>
									<xsl:value-of select="substring(substring-before(string(CAPI_PNR_NonFormattedElement/InformationText),'/'),5,20)"/>
								</CCNo>
								<xsl:variable name="apcode">
									<xsl:value-of select="substring-after(string(CAPI_PNR_NonFormattedElement/InformationText),'/')"/>
								</xsl:variable>
								<CCExpDate>
									<xsl:value-of select="substring(string($apcode),1,4)"/>
								</CCExpDate>
								<CCApprovalCode>
									<xsl:value-of select="substring-after(string($apcode),'/N')"/>
								</CCApprovalCode>
							</CC>
						</xsl:when>
						<xsl:otherwise>
							<Other>
								<xsl:value-of select="CAPI_PNR_NonFormattedElement/InformationText"/>
							</Other>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</FOP>
		</xsl:if>
	</xsl:template>
	<xsl:template match="dataElementsIndiv" mode="commission">
		<AgencyCommission>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*M*') or contains(otherDataFreetext/longFreetext,'*C*') or contains(otherDataFreetext/longFreetext,'*G*') or contains(otherDataFreetext/longFreetext,'*F*')">
					<xsl:variable name="comm1">
						<xsl:choose>
							<xsl:when test="contains(otherDataFreetext/longFreetext,'*M*')">
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*M*')"/>
							</xsl:when>
							<xsl:when test="contains(otherDataFreetext/longFreetext,'*G*')">
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*G*')"/>
							</xsl:when>
							<xsl:when test="contains(otherDataFreetext/longFreetext,'*F*')">
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*F*')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*C*')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="comm">
						<xsl:choose>
							<xsl:when test="contains($comm1,'XO')">
								<xsl:value-of select="substring-before($comm1,'/')"/>
							</xsl:when>
							<xsl:when test="contains($comm1,'/')">
								<xsl:value-of select="substring-after($comm1,'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$comm1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="contains($comm,'A')">
							<xsl:attribute name="Amount">
								<xsl:choose>
									<xsl:when test="contains(substring-after($comm,'.'),'.')">
										<xsl:choose>
											<xsl:when test="contains(substring-before(substring-after($comm,'.'),'.'),'A')">
												<xsl:value-of select="substring-before($comm,'A')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat(substring-before($comm,'.'),'.')"/>
												<xsl:value-of select="substring-before(substring-after($comm,'.'),'.')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring-before($comm,'A')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="contains($comm,'N')">
							<xsl:attribute name="Percent">
								<xsl:value-of select="translate(substring-before($comm,'N'),'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','')"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Percent">
								<xsl:value-of select="translate($comm,'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','')"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*AM*')">
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(substring-after(otherDataFreetext/longFreetext,'*AM*'),'A','')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*PR*')">
					<xsl:attribute name="Percent">
						<xsl:choose>
							<xsl:when test="contains(substring-after(otherDataFreetext/longFreetext,'*PR*'),'/')">
								<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'*PR*'),'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*PR*')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*P*')">
					<xsl:attribute name="Percent">
						<xsl:choose>
							<xsl:when test="contains(substring-after(otherDataFreetext/longFreetext,'*P*'),'/')">
								<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'*P*'),'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*P*')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*D*')">
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="contains(substring-after(otherDataFreetext/longFreetext,'*D*'),'/')">
								<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'*D*'),'/')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*D*')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'PAX ') and contains(substring-after(otherDataFreetext/longFreetext,'PAX '),'A')">
					<xsl:attribute name="Amount">
						<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'PAX '),'A')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'PAX ') and contains(substring-after(otherDataFreetext/longFreetext,'PAX '),'P')">
					<xsl:attribute name="Percent">
						<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'PAX '),'P')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'INF ') and contains(substring-after(otherDataFreetext/longFreetext,'INF '),'A')">
					<xsl:attribute name="Amount">
						<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'INF '),'A')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'INF ') and contains(substring-after(otherDataFreetext/longFreetext,'INF '),'P')">
					<xsl:attribute name="Percent">
						<xsl:value-of select="substring-before(substring-after(otherDataFreetext/longFreetext,'INF '),'P')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'PAX ')">
					<xsl:attribute name="Amount">
						<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'PAX ')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'A')">
					<xsl:attribute name="Amount">
						<xsl:value-of select="substring-before(otherDataFreetext/longFreetext,'A')"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Percent">
						<xsl:value-of select="translate(otherDataFreetext/longFreetext,'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','')"/>
					</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="starts-with(otherDataFreetext/longFreetext,'INF ') or starts-with(otherDataFreetext/longFreetext,'PAX ')">
					<xsl:attribute name="PassengerType">
						<xsl:value-of select="substring(otherDataFreetext/longFreetext,1,3)"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</AgencyCommission>
	</xsl:template>
	<xsl:template match="common_v50_0:AccountingRemark" mode="accounting">
		<AccountingLine>
			<xsl:value-of select="common_v50_0:RemarkData"/>
		</AccountingLine>
	</xsl:template>
	<!-- ********************************************************************************	-->
	<!-- Miscellaneous other,  Air Taxi, Land, Sea, Rail, Car and hotel               -->
	<!-- ********************************************************************************* -->
	<xsl:template match="itineraryInfo" mode="Other">
		<!--General Segment in OTA -->
		<!-- Other Segments -->
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
					</xsl:attribute>
				</xsl:if>
				<Description>
					<xsl:choose>
						<xsl:when test="elementManagementItinerary/segmentName = 'RU'">Miscellaneous</xsl:when>
					</xsl:choose>
					<xsl:if test="travelProduct/boardpointDetail/cityCode !=''">
						<xsl:text> - Board point: </xsl:text>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</xsl:if>
					<xsl:if test="itineraryFreetext/longFreetext!=''">
						<xsl:text> - </xsl:text>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</xsl:if>
				</Description>
				<TPA_Extensions>
					<xsl:attribute name="Status">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</General>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Taxi">
		<!--General OTA Segments -->
		<!-- Taxi Segments -->
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="itineraryFreetext/longFreetext!=''">
					<Description>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</Description>
				</xsl:if>
				<TPA_Extensions>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</General>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Land">
		<!--OTA Package -->
		<!-- Surface Segments -->
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
			</xsl:attribute>
			<Package>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="itineraryFreetext/longFreetext!=''">
					<Description>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</Description>
				</xsl:if>
				<TPA_Extensions>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</Package>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Train">
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
			</xsl:attribute>
			<Rail>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
					<xsl:text>T</xsl:text>
					<xsl:choose>
						<xsl:when test="not(travelProduct/product/depTime)">00:00:00</xsl:when>
						<xsl:when test="substring(travelProduct/product/depTime,1,2)='24'">00</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(travelProduct/product/depTime,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="travelProduct/product/depTime!=''">
						<xsl:value-of select="concat(':',substring(travelProduct/product/depTime,3,2),':00')"/>
					</xsl:if>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
						<xsl:text>T</xsl:text>
						<xsl:choose>
							<xsl:when test="not(travelProduct/product/arrTime)">00:00:00</xsl:when>
							<xsl:when test="substring(travelProduct/product/arrTime,1,2)='24'">00</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(travelProduct/product/arrTime,1,2)"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="travelProduct/product/arrTime!=''">
							<xsl:value-of select="concat(':',substring(travelProduct/product/arrTime,3,2),':00')"/>
						</xsl:if>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="itineraryFreetext/longFreetext!=''">
					<Description>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</Description>
				</xsl:if>
				<TPA_Extensions>
					<xsl:attribute name="Status">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<xsl:attribute name="TrainNumber">
						<xsl:value-of select="travelProduct/productDetails/identification"/>
					</xsl:attribute>
					<xsl:attribute name="ClassOfService">
						<xsl:value-of select="travelProduct/productDetails/classOfService"/>
					</xsl:attribute>
					<xsl:attribute name="BookingReference">
						<xsl:value-of select="itineraryReservationInfo/reservation/controlNumber"/>
					</xsl:attribute>
					<xsl:attribute name="TrainEquipment">
						<xsl:value-of select="travelProduct/typeDetail/detail"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
					<OriginRailwayStation>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
						</xsl:attribute>
						<xsl:value-of select="travelProduct/boardpointDetail/cityName"/>
					</OriginRailwayStation>
					<DestinationRailwayStation>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
						</xsl:attribute>
						<xsl:value-of select="travelProduct/offpointDetail/cityName"/>
					</DestinationRailwayStation>
				</TPA_Extensions>
			</Rail>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Cruise">
		<!-- OTA Cruise-->
		<!-- Sea Segments -->
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
			</xsl:attribute>
			<Cruise>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="itineraryFreetext/longFreetext!=''">
					<Description>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</Description>
				</xsl:if>
				<TPA_Extensions>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</Cruise>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Tour">
		<!--OTA Tour-->
		<!--Tour Segments -->
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="Status">
				<xsl:value-of select="relatedProduct/status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
			</xsl:attribute>
			<Tour>
				<xsl:attribute name="Start">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
				</xsl:attribute>
				<xsl:if test="travelProduct/product/arrDate != ''">
					<xsl:attribute name="End">
						<xsl:text>20</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="itineraryFreetext/longFreetext!=''">
					<Description>
						<xsl:value-of select="itineraryFreetext/longFreetext"/>
					</Description>
				</xsl:if>
				<TPA_Extensions>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</Tour>
		</Item>
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="CarPassive">
		<!-- Car Segments -->
		<CarPassiveSegment>
			<ElementNumber>
				<xsl:attribute name="TattooNumber">
					<xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/>
				</xsl:attribute>
				<xsl:attribute name="TattooQualifier">
					<xsl:value-of select="elementManagementItinerary/reference/qualifier"/>
				</xsl:attribute>
				<xsl:value-of select="elementManagementItinerary/lineNumber"/>
			</ElementNumber>
			<CarVendorCode>
				<xsl:value-of select="travelProduct/companyDetail/identification"/>
			</CarVendorCode>
			<CityCode>
				<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
			</CityCode>
			<ActionCode>
				<xsl:value-of select="relatedProduct/status"/>
			</ActionCode>
			<NumberOfCars>
				<xsl:value-of select="relatedProduct/quantity"/>
			</NumberOfCars>
			<PickUpInfo>
				<Date>
					<xsl:value-of select="travelProduct/product/depDate"/>
				</Date>
			</PickUpInfo>
			<DropOffInfo>
				<Date>
					<xsl:value-of select="travelProduct/product/arrDate"/>
				</Date>
			</DropOffInfo>
			<CarType/>
			<xsl:if test="itineraryFreetext/longFreetext!=''">
				<Text>
					<xsl:value-of select="itineraryFreetext/longFreetext"/>
				</Text>
			</xsl:if>
		</CarPassiveSegment>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="totalbase">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>
		<xsl:param name="loop"/>

		<xsl:variable name="nopt">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>

		<xsl:variable name="ref">
			<xsl:value-of select="@ProviderReservationInfoRef"/>
		</xsl:variable>

		<xsl:variable name="tot">
			<xsl:value-of select="format-number(translate(substring(@BasePrice,4),'.',''), '#') * $nopt"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]">
				<xsl:apply-templates select="../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]" mode="totalbase">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="totalTax">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>
		<xsl:param name="loop"/>

		<xsl:variable name="nopt">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(substring(@Taxes,4),'.','') * $nopt"/>
		</xsl:variable>

		<xsl:variable name="ref">
			<xsl:value-of select="@ProviderReservationInfoRef"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]">
				<xsl:apply-templates select="../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]" mode="totalTax">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="air:AirPricingInfo" mode="totalprice">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>
		<xsl:param name="loop"/>

		<xsl:variable name="nopt">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>

		<xsl:variable name="tot">
			<xsl:value-of select="translate(substring(@TotalPrice,4),'.','') * $nopt"/>
		</xsl:variable>

		<xsl:variable name="ref">
			<xsl:value-of select="@ProviderReservationInfoRef"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]">
				<xsl:apply-templates select="../air:AirPricingInfo[@ProviderReservationInfoRef = $ref and @AirPricingInfoGroup=1][$pos + 1]" mode="totalprice">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="paxNumber">
		<xsl:param name="key" />
		<xsl:for-each select="../../../common_v50_0:BookingTraveler">
			<xsl:if test="@Key = $key">
				<xsl:value-of select="position()"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="fltNumber">
		<xsl:param name="key" />
		<xsl:for-each select="../../air:AirSegment">
			<xsl:if test="@Key = $key">
				<xsl:value-of select="position()"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="bdt">
		<xsl:param name="bdt" />
		<xsl:variable name="bd">
			<xsl:choose>
				<xsl:when test="string-length($bdt) > 7">
					<xsl:value-of select="substring($bdt,string-length($bdt) - 6,7)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$bdt"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="substring($bd,6,2) > 89">19</xsl:when>
			<xsl:otherwise>20</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="substring($bd,6,2)" />
		<xsl:text>-</xsl:text>
		<xsl:choose>
			<xsl:when test="substring($bd,3,3) = 'JAN'">01</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'FEB'">02</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'MAR'">03</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'APR'">04</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'MAY'">05</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'JUN'">06</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'JUL'">07</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'AUG'">08</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'SEP'">09</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'OCT'">10</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'NOV'">11</xsl:when>
			<xsl:when test="substring($bd,3,3) = 'DEC'">12</xsl:when>
		</xsl:choose>
		<xsl:text>-</xsl:text>
		<xsl:value-of select="substring($bd,1,2)" />
	</xsl:template>

	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

	<!-- Strips trailing whitespace characters from 'string' -->
	<xsl:template name="string-rtrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:variable name="length" select="string-length($string)" />

		<xsl:if test="$length &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, $length, 1))">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading whitespace characters from 'string' -->
	<xsl:template name="string-ltrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:if test="string-length($string) &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, 1, 1))">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="substring($string, 2)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading and trailing whitespace characters from 'string' -->
	<xsl:template name="string-trim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />
		<xsl:call-template name="string-rtrim">
			<xsl:with-param name="string">
				<xsl:call-template name="string-ltrim">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="trim"   select="$trim" />
				</xsl:call-template>
			</xsl:with-param>
			<xsl:with-param name="trim"   select="$trim" />
		</xsl:call-template>
	</xsl:template>

	<!--############################################################-->
	<!--## Template to tokenize strings                           ##-->
	<!--############################################################-->
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

	<!--**********************************************************************************************-->
	<msxsl:script language="VisualBasic" implements-prefix="ttVB">
		<![CDATA[        
        Function ShortDateFormat(ByVal p_startDate As String) As String

            If IsDate(p_startDate) Then
                Return Convert.ToDateTime(p_startDate).ToString("yyyy-MM-d")
            Else
                Return p_startDate
            End If

        End Function
]]>
	</msxsl:script>
</xsl:stylesheet>
