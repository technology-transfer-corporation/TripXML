<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_PNRReadRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 13 Mar 2006 - Rastko														       -->
<!-- ================================================================== -->
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="PoweredPNR_PNRReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
	<xsl:apply-templates select="Queue_Start_Reply" />
	<xsl:apply-templates select="Queue_Count_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply | Queue_Start_Reply | Queue_Count_Reply">
	<OTA_TravelItineraryRS Version="2.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_TravelItineraryRS>
</xsl:template>
	
<xsl:template match="CAPI_Messages">
	<Error>
		<xsl:attribute name="Code">
			<xsl:value-of select="ErrorCode" />
		</xsl:attribute>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="Text" />
	</Error>
</xsl:template>

<xsl:template match="PoweredPNR_PNRReply">
	<OTA_TravelItineraryRS Version="2.000">
		<xsl:choose>
			<xsl:when test="pnrHeader/reservationInfo/reservation/controlNumber!=''">
				<Success/>
				<xsl:if test="Error or Warning">
					<Warnings>
						<xsl:apply-templates select="Error" mode="warning"/>
						<xsl:apply-templates select="Warning" mode="warning"/>
					</Warnings>
				</xsl:if>
				<TravelItinerary>	
					<xsl:apply-templates select="pnrHeader[not(reservationInfo/reservation/controlType)]" mode="header"/>
					<CustomerInfos>
						<xsl:apply-templates select="travellerInfo/travellerInformation/passenger"/>	
					</CustomerInfos>
					<ItineraryInfo>							
						<xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU']" >
							<!--  Process Air, Car, Hotel Itineraries      -->
							<ReservationItems>								
								<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']" mode="Air"/>
								<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR']" mode="Car"/>
								<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU']" mode="Car"/>								
								<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL']" mode="Hotel"/>
								<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU']" mode="Hotel"/>
								<xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] 	| originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN']" mode="Train"/> 	
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']" mode="Cruise"/> 	
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']" mode="Tour"/>  	
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']" mode="Other"/> 	
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU']" mode="Taxi"/> 	
									<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR']" mode="Land"/> 	
								</xsl:if>
								<xsl:if test="PoweredTicket_DisplayTSTReply">
									<ItemPricing>
										<xsl:apply-templates select="PoweredTicket_DisplayTSTReply"/>	
									</ItemPricing>		
								</xsl:if>
							</ReservationItems>
						</xsl:if>	
						<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='TK']" mode="ticketing"/>
						<SpecialRequestDetails>
							<xsl:if test="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]">
								<SeatRequests>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb]" mode="Seat"/>
								</SeatRequests>		
							</xsl:if>
							<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR'][not(serviceRequest/ssrb)]">
								<SpecialServiceRequests>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR'][not(serviceRequest/ssrb)]" mode="SSR"/>
								</SpecialServiceRequests>		
							</xsl:if>		
							<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']">
								<OtherServiceInformations>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='OS']" mode="OSI"/>					
								</OtherServiceInformations>
							</xsl:if>	
							<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RM']">	
								<Remarks>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RM']" mode="GenRemark"/>
								</Remarks>
							</xsl:if>	
							<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RII'] | dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIR']">
								<SpecialRemarks>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RII']" mode="InvoiceItinRemark"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIF']" mode="InvoiceRemark"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIR']" mode="ItinRemark"/>
								</SpecialRemarks>
							</xsl:if>	
						</SpecialRequestDetails>	
					</ItineraryInfo>	
					<!--xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA']" mode="ticketnumber"/-->		
					<TravelCost>
						<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
							<FormOfPayment>
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP' ]" mode="Payment"/>
							</FormOfPayment>
						</xsl:if>	
						<!--xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']">
							<TPA_Extensions>
								<AgencyCommission>
									<xsl:attribute name="Percent"><xsl:value-of select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext/longFreetext"/></xsl:attribute>
								</AgencyCommission>
							</TPA_Extensions>
						</xsl:if-->
					</TravelCost>	
					<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FB'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']">
						<TPA_Extensions>
							<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']" mode="commission"/>
							<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']" mode="accounting"/>
						</TPA_Extensions>
					</xsl:if>
				</TravelItinerary>			
			</xsl:when>			
			<xsl:when test="Error">
				<Errors>
					<xsl:apply-templates select="Error" mode="error"/>
				</Errors>
			</xsl:when>
		</xsl:choose>			
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
<!-- PNR Header Information    	                                    -->
<!-- ************************************************************** -->
<xsl:template match="pnrHeader" mode="header">
	<ItineraryRef>
			<xsl:attribute name="Type">PNR</xsl:attribute>
			<xsl:attribute name="ID"><xsl:value-of select="reservationInfo/reservation/controlNumber"/></xsl:attribute>
			<xsl:if test="../securityInformation/responsibilityInformation/officeId != ''">
				<xsl:attribute name="ID_Context"><xsl:value-of select="../securityInformation/responsibilityInformation/officeId"/></xsl:attribute>
			</xsl:if>
	</ItineraryRef> 
</xsl:template>

<!-- ************************************************************** -->
<!-- Pricing Response     	                                    -->
<!-- ************************************************************** -->
<xsl:template match="PoweredTicket_DisplayTSTReply">
	<AirFareInfo>
		<xsl:attribute name="PricingSource">
			<xsl:choose>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'H'">Private</xsl:when>
				<xsl:otherwise>Published</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<ItinTotalFare>
			<BaseFare>
				<xsl:attribute name="Amount">
					<xsl:variable name="tot">
						<xsl:value-of select="sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount)"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="contains($tot,'.')"><xsl:value-of select="translate(string($tot),'.','')"/></xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$tot"/>
							<xsl:text>00</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</BaseFare>
			<TotalFare>
				<xsl:attribute name="Amount">
					<xsl:value-of select="translate(sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount),'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</TotalFare>
		</ItinTotalFare>
		<PTC_FareBreakdowns>
			<xsl:apply-templates select="fareList"/>
		</PTC_FareBreakdowns>
	</AirFareInfo>
</xsl:template>

<xsl:template match="fareList">
	<PTC_FareBreakdown>
		<xsl:attribute name="PricingSource">
			<xsl:choose>
				<xsl:when test="fareDataInformation/fareDataMainInformation/fareDataQualifier = 'H'">Private</xsl:when>
				<xsl:otherwise>Published</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="statusInformation/firstStatusDetails/tstFlag = 'CH'">CHD</xsl:when>
					<xsl:when test="statusInformation/firstStatusDetails/tstFlag = 'CNN'">CHD</xsl:when>
					<xsl:when test="statusInformation/firstStatusDetails/tstFlag = 'YCD'">SRC</xsl:when>
					<xsl:otherwise><xsl:value-of select="statusInformation/firstStatusDetails/tstFlag"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity"><xsl:value-of select="count(paxSegReference/refDetails)"/></xsl:attribute>
		</PassengerTypeQuantity>
		<FareBasisCodes>
			<xsl:apply-templates select="segmentInformation"/>
		</FareBasisCodes>
		<PassengerFare>
			<BaseFare>
				<xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount,'.','')"/>
				</xsl:attribute>
			</BaseFare> 
			<Taxes>
				<xsl:apply-templates select="taxInformation"/>
			</Taxes>
			<TotalFare>
				<xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount,'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</TotalFare>
		</PassengerFare>
	</PTC_FareBreakdown>
</xsl:template>

<xsl:template match="segmentInformation">
	<FareBasisCode>
		<xsl:choose>
			<xsl:when test="fareQualifier/fareBasisDetails/fareBasisCode != ''">
				<xsl:value-of select="fareQualifier/fareBasisDetails/fareBasisCode"/>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="fareQualifier/fareBasisDetails/primaryCode"/></xsl:otherwise>
		</xsl:choose>
	</FareBasisCode> 
</xsl:template>

<xsl:template match="taxInformation">
	<Tax>
		<xsl:attribute name="Code">
			<xsl:value-of select="taxDetails/taxType/isoCountry"/>
		</xsl:attribute>
		<xsl:attribute name="Amount">
			<xsl:value-of select="translate(amountDetails/fareDataMainInformation/fareAmount,'.','')"/>
		</xsl:attribute>
	</Tax>
</xsl:template>
	<!-- ************************************************************** -->
	<!-- Process Names			                            -->
	<!-- ************************************************************** -->
	<xsl:template match="passenger">
	   <CustomerInfo>
		   <xsl:attribute name="RPH"><xsl:value-of select="../../elementManagementPassenger/lineNumber"/></xsl:attribute>
		   <Customer>
			   	<PersonName>
				   	<xsl:attribute name="NameType">
				   		<xsl:choose>
							<xsl:when test="type = 'CH'">CHD</xsl:when>
							<xsl:when test="type = 'CNN'">CHD</xsl:when>
							<xsl:when test="type = 'YCD'">SRC</xsl:when>
							<xsl:otherwise><xsl:value-of select="type"/></xsl:otherwise>
						</xsl:choose>
				   	</xsl:attribute>
				  	 <GivenName>
						<xsl:value-of select="firstName"/>
					</GivenName>			
					<Surname>						
						<xsl:value-of select="../traveller/surname"/>
					</Surname>			
			   	</PersonName> 
			   	<xsl:apply-templates select="../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="phone"/>	
			  	<xsl:apply-templates select="../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="email"/>	
				<xsl:apply-templates select="../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB/']" mode="Address"/>
				<xsl:apply-templates select="../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB']" mode="Address"/>
				<xsl:apply-templates select="../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM/']" mode="Address"/>
		   </Customer>
		</CustomerInfo>
	</xsl:template>
	<!-- ****************************************************************************************************************** -->
	<!-- Process Itinerary				 							                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Air Segments    				                    -->
	<!-- ************************************************************** -->
	<xsl:template match="itineraryInfo" mode="Air">
		<xsl:variable name="zeros">000</xsl:variable>
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>		
			</xsl:attribute>
			<xsl:if test="relatedProduct/status='GK'">
				<xsl:attribute name="IsPassive">Y</xsl:attribute>
			</xsl:if>
		<xsl:choose>
			
			<xsl:when test="travelProduct/productDetails/identification='ARNK'">
				<TPA_Extensions>
					<Arnk/>
				</TPA_Extensions>
			</xsl:when>			
			<xsl:otherwise>
			
			<Air>
				<xsl:attribute name="RPH"> 
					<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
				</xsl:attribute>
				
				
				
					<!--************************************************************************************-->
					<!--			Air Segments/Open Segments  						      -->
					<!--************************************************************************************-->
					<xsl:variable name="zeroes">0000</xsl:variable>
					<xsl:attribute name="NumberInParty"><xsl:value-of select="relatedProduct/quantity"/></xsl:attribute> <!--done-->
					<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="travelProduct/productDetails/classOfService"/></xsl:attribute><!--done-->
					<!--note:  valid values according to OTA for ActionCode are OK, Waitlist, or Other - need to add others to this I think-->
					<xsl:attribute name="ActionCode"> 
							<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>	
					<xsl:attribute name="DepartureDateTime">	<!--done -->
							<xsl:text>20</xsl:text>		
							<xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/depTime,1,2)"/>:<xsl:value-of select="substring(travelProduct/product/depTime,3,2)"/><xsl:text>:00</xsl:text>
					
					</xsl:attribute>	
					<xsl:attribute name="ArrivalDateTime">	<!--done -->
							<xsl:text>20</xsl:text>		
							<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring(travelProduct/product/arrTime,1,2)"/>:<xsl:value-of select="substring(travelProduct/product/arrTime,3,2)"/><xsl:text>:00</xsl:text>
						
					</xsl:attribute>
					<xsl:attribute name="FlightNumber">	<!--done-->			
						<xsl:choose>
							<xsl:when test="travelProduct/productDetails/identification='OPEN'">OPEN</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="travelProduct/productDetails/identification"/>									
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<DepartureAirport> <!--done -->
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>								
						</xsl:attribute> 						
					</DepartureAirport>
					<ArrivalAirport>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
						</xsl:attribute> 						
					</ArrivalAirport>		
					<xsl:if test="CAPI_PNR_AirSegment/Airline2 != ''">
						<OperatingAirline>
							<xsl:attribute name="Code">
											<xsl:value-of select="CAPI_PNR_AirSegment/Airline2"/>
							</xsl:attribute>
							<AirlineName></AirlineName>
						</OperatingAirline>
					</xsl:if>
					<xsl:if test="flightDetail/productDetails/equipment">
						<Equipment>
							<xsl:attribute name="AirEquipType">	
								<xsl:value-of select="flightDetail/productDetails/equipment"/>
							</xsl:attribute>
						</Equipment>
					</xsl:if>			
					<MarketingAirline>
						<xsl:attribute name="Code">
								<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</MarketingAirline>
				</Air>	
			</xsl:otherwise>	
		</xsl:choose>
	</Item>
	</xsl:template>
	
	<!--************************************************************************************-->
	<!--			Hotel Segs   						   					    -->
	<!--************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Hotel">	
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>		
			</xsl:attribute>			
			<xsl:if test="elementManagementItinerary/segmentName='HU'">			
				<xsl:attribute name="IsPassive">Y</xsl:attribute>
			</xsl:if>	
		<Hotel>
			<Reservation>
				<RoomTypes>
					<RoomType>
						<xsl:attribute name="RoomTypeCode"><xsl:value-of select="hotelProduct/hotelRoom/typeCode"/></xsl:attribute> 
						<xsl:attribute name="NumberOfUnits"><xsl:value-of select="relatedProduct/quantity"/></xsl:attribute> 
					</RoomType>	
				</RoomTypes>
				<RatePlans>
					<RatePlan>
						<xsl:attribute name="RatePlanCode"><xsl:value-of select="hotelProduct/negotiated/rateCode"/></xsl:attribute>
					</RatePlan>				
				</RatePlans>
				<RoomRates>
					<RoomRate>
						<Rates>
							<Rate>
								<Base>	
									<xsl:variable name="deci">
										<xsl:value-of select="substring-after(string(generalOption/optionDetail[type='TTL']/freetext),'.')"/>
									</xsl:variable>
									<xsl:variable name="decilgth">
										<xsl:value-of select="string-length($deci)"/>
									</xsl:variable>		
									<xsl:variable name="amount">
										<xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),4,20)"/>
									</xsl:variable>					
									<xsl:attribute name="AmountBeforeTax"><xsl:value-of select="translate(string($amount),'.','')"/></xsl:attribute> 	
									<xsl:attribute name="CurrencyCode"><xsl:value-of select="substring(string(generalOption/optionDetail[type='TTL']/freetext),1,3)"/></xsl:attribute>
									<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$decilgth"/></xsl:attribute>
								</Base>
							</Rate>							
						</Rates>
					</RoomRate>					
				</RoomRates>				
				<GuestCounts>
					<GuestCount> 
						<xsl:attribute name="Count"><xsl:value-of select="hotelProduct/hotelRoom/occupancy"/></xsl:attribute> 
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
					<xsl:attribute name="ChainCode"><xsl:value-of select="travelProduct/companyDetail/identification"/></xsl:attribute> 
					<xsl:attribute name="HotelCityCode">	<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute> 
					<xsl:attribute name="HotelCode"><xsl:value-of select="hotelProduct/property/code"/></xsl:attribute> 
					<xsl:attribute name="HotelName"><xsl:value-of select="hotelProduct/property/name"/></xsl:attribute> 
				</BasicPropertyInfo>			
			</Reservation>
			<TPA_Extensions>
				<xsl:attribute name="ConfirmationNumber"><xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/></xsl:attribute> 	
			 </TPA_Extensions>
		</Hotel>
	   </Item>	
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Car Segs				   			-->
	<!--************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Car">
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>	
			</xsl:attribute>
			<xsl:if test="elementManagementItinerary/segmentName='CU'">
				<xsl:attribute name="IsPassive">Y</xsl:attribute>
			</xsl:if>
		<Vehicle>			
			<ConfID>
				<xsl:attribute name="Type">8</xsl:attribute>	
				<xsl:attribute name="ID">
					<xsl:choose>
						<xsl:when test="generalOption/optionDetail[type='CF']/freetext != ''">
							<xsl:value-of select="generalOption/optionDetail[type='CF']/freetext"/>					
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="generalOption/optionDetail[type='BS']/freetext"/>					
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>			
			</ConfID>
  			<Vendor>
  				<xsl:attribute name="Code"><xsl:value-of select="travelProduct/companyDetail/identification"/></xsl:attribute>
  			</Vendor>   			
 			<VehRentalCore>
 				<xsl:attribute name="PickUpDateTime">
					<xsl:value-of select="typicalCarData/pickupDropoffTimes/beginDateTime/year"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/month,'00')"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/day,'00')"/>
					<xsl:text>T</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/hour,'00')"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/beginDateTime/minutes,'00')"/>
					<xsl:text>:00</xsl:text>
				</xsl:attribute> 				
 				<xsl:attribute name="ReturnDateTime">
					<xsl:value-of select="typicalCarData/pickupDropoffTimes/endDateTime/year"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/month,'00')"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/day,'00')"/>
					<xsl:text>T</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/hour,'00')"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="format-number(typicalCarData/pickupDropoffTimes/endDateTime/minutes,'00')"/>
					<xsl:text>:00</xsl:text>
				</xsl:attribute> 
 				 <PickUpLocation>
 				 	<xsl:attribute name="LocationCode">	<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute> 
 				  </PickUpLocation> 
 				 <ReturnLocation>
 					 <xsl:attribute name="LocationCode">
 						 <xsl:choose>
							<xsl:when test="travelProduct/offpointDetail/cityCode != ''">							
								<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>						
							</xsl:when>
							<xsl:when test="tgeneralOption/optionDetail[type='DO']/freetext != ''">							
								<xsl:value-of select="generalOption/optionDetail[type='DO']/freetext"/>						
							</xsl:when>
							<xsl:otherwise>						
								<xsl:value-of select="typicalCarData/locationInfo[locationType='DOL']/locationDescription/name"/>					
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute> 			 
 				 </ReturnLocation> 
 			 </VehRentalCore>
			<Veh>
				<xsl:attribute name="AirConditionInd">
					<xsl:choose>
						<xsl:when test="substring(travelProduct/productDetails/identification,4,1) = 'R'">true</xsl:when>										<xsl:otherwise>false</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute> 
				<xsl:attribute name="TransmissionType">
					<xsl:choose>
						<xsl:when test="substring(travelProduct/productDetails/identification,3,1) = 'A'">Automatic</xsl:when>									<xsl:otherwise>Manual</xsl:otherwise>
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
  			<xsl:variable name="charge">
		 		<xsl:choose>
					<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/amount = ''">
						<xsl:value-of select="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/amount"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="translate(concat(substring-before(substring(generalOption/optionDetail[type='RG']/freetext,8),'.'),'.',substring(substring-after(generalOption/optionDetail[type='RG']/freetext,'.'),1,2)),' ','')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable> 			
			 <RentalRate>
 				 <RateDistance>
 				 	<xsl:attribute name="Unlimited">
 				 		<xsl:choose>
							<xsl:when test="typicalCarData/rateInfo[tariffInfo/amountType = 'RG']/chargeDetails/description = 'UNL'">true</xsl:when>
							<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'UNL')">true</xsl:when>
							<xsl:otherwise>false</xsl:otherwise>
						</xsl:choose> 				 	
 				 	</xsl:attribute>
					<xsl:attribute name="DistUnitName">Mile</xsl:attribute>
 				  </RateDistance> 
 				  <VehicleCharges>
 				  	 <xsl:if test="$charge != '.'">
	 					<VehicleCharge> 
	 						<xsl:attribute name="TaxInclusive">false</xsl:attribute>	
	 						<xsl:attribute name="Amount">	
	 							<xsl:value-of select="translate($charge,'.','')"/>	
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
								<xsl:value-of select="string-length(substring-after($charge,'.'))"/>
							</xsl:attribute>		
							<xsl:attribute name="TaxInclusive">false</xsl:attribute> 
							<xsl:attribute name="GuaranteedInd">	
								<xsl:choose>
									<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/amount != ''">true</xsl:when>
									<xsl:when test="generalOption/optionDetail[type='RG']/freetext != ''">true</xsl:when>
									<xsl:otherwise>false</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute> 		
							<xsl:attribute name="Description">
								<xsl:choose>
									<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator != ''">
										<xsl:choose>
											<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator = 'DY'">Daily Rate</xsl:when>
											<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator = 'WY'">Weekly Rate</xsl:when>
											<xsl:otherwise>Rental Period Rate</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'DY')">Daily Rate</xsl:when>
									<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'WY')">Weekly Rate</xsl:when>
									<xsl:otherwise>Rental Period Rate</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</VehicleCharge>
					</xsl:if>
					<xsl:if test="contains(generalOption/optionDetail[type='RG']/freetext, 'XD') or typicalCarData/rateInfo/chargeDetails[V = 'XDM']">
						<xsl:variable name="ed">
							<xsl:choose>
								<xsl:when test="typicalCarData/rateInfo/chargeDetails[V = 'XDM']">
									<xsl:apply-templates select="typicalCarData/rateInfo/chargeDetails[V = 'XDM']"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),'.',substring(substring-after(substring-after(generalOption/optionDetail[type='RG']/freetext,'XD'),'.'),1,2)),' ','')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:if test="$ed != '.'">
							<VehicleCharge> 
								<xsl:attribute name="TaxInclusive">false</xsl:attribute>
								<xsl:attribute name="Amount">	
		 							<xsl:value-of select="translate($ed,'.','')"/>	
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
							<xsl:value-of select="translate(concat(substring-before(substring-after(generalOption/optionDetail[type='RG']/freetext,'XH'),'.'),'.',substring(substring-after(substring-after(generalOption/optionDetail	[type='RG']/freetext,'XH'),'.'),1,2)),' ','')"/>
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
					</xsl:if>
				</VehicleCharges>	
				<xsl:if test="typicalCarData/rateCodeInfo/fareCategories/fareType != ''">
					<RateQualifier>
						<xsl:attribute name="RateQualifier"><xsl:value-of select="typicalCarData/rateCodeInfo/fareCategories/fareType"/></xsl:attribute>
						<xsl:attribute name="RatePeriod">
							<xsl:choose>
								<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator != ''">
									<xsl:choose>
										<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator = 'DY'">Daily</xsl:when>
										<xsl:when test="typicalCarData/rateInfo/tariffInfo[amountType = 'RG']/ratePlanIndicator = 'WY'">Weekly</xsl:when>
										<xsl:otherwise>Other</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'DY')">Daily</xsl:when>
								<xsl:when test="contains(generalOption/optionDetail[type='RG']/freetext, 'WY')">Weekly</xsl:when>
								<xsl:otherwise>Other</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</RateQualifier>
				</xsl:if>
			  </RentalRate> 
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
					<xsl:attribute name="RateTotalAmount"><xsl:value-of select="$total"/></xsl:attribute>
					<xsl:attribute name="EstimatedTotalAmount"><xsl:value-of select="$total"/></xsl:attribute>
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
				</TotalCharge> 
			</xsl:if>
			<TPA_Extensions>
				<CarOptions>
					<xsl:apply-templates select="generalOption/optionDetail" mode="car" />
					<xsl:for-each select="itineraryFreetext[longFreetext != '']">
						<CarOption>
							<xsl:attribute name="Option">Marketing Text</xsl:attribute>
							<Text>
								<xsl:value-of select="longFreetext" />
							</Text>
						</CarOption>
					</xsl:for-each>
				</CarOptions>
			</TPA_Extensions>
 		</Vehicle>
	</Item>
</xsl:template>

<xsl:template match="chargeDetails">
	<xsl:value-of select="translate(preceding-sibling::chargeDetails[1]/amount,'.','')"/>
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
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RQ'">
				<CarOption>
					<xsl:attribute name="Option">Quoted Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RB'">
				<CarOption>
					<xsl:attribute name="Option">Base Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RC'">
				<CarOption>
					<xsl:attribute name="Option">Rate Code</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RG'">
				<CarOption>
					<xsl:attribute name="Option">Guaranteed Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'NM'">
				<CarOption>
					<xsl:attribute name="Option">Reservation Last and First Names</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ES'">
				<CarOption>
					<xsl:attribute name="Option">Estimated Total Rate</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ARR'">
				<CarOption>
					<xsl:attribute name="Option">Pick Up Time</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'RT'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Time</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'ID'">
				<CarOption>
					<xsl:attribute name="Option">Customer ID</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'GT'">
				<CarOption>
					<xsl:attribute name="Option">Payment Guarantee</xsl:attribute>
					<xsl:choose>
						<xsl:when test="contains(freetext, 'EXP')">
							<Text>
								<xsl:value-of select="substring(freetext, 1, 2)" />
								<xsl:value-of select="substring-before(substring(freetext, 3, string-length(freetext) - 4), 'EXP')" />
								<xsl:value-of select="substring(substring-after(freetext,'EXP'), 1, 2)" />
								<xsl:value-of select="substring(substring-after(freetext,'EXP'), 3, 2)" />
							</Text>
						</xsl:when>
					</xsl:choose>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'PU'">
				<CarOption>
					<xsl:attribute name="Option">Pick Up Location</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'AD'">
				<CarOption>
					<xsl:attribute name="Option">Customer Address</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'CD'">
				<CarOption>
					<xsl:attribute name="Option">Corporate Discount</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'SQ'">
				<CarOption>
					<xsl:attribute name="Option">Optional Equipment</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'PR'">
				<CarOption>
					<xsl:attribute name="Option">PrePayment Info</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DL'">
				<CarOption>
					<xsl:attribute name="Option">Driver License</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'FT'">
				<CarOption>
					<xsl:attribute name="Option">Frequent Traveler Number</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'SI'">
				<CarOption>
					<xsl:attribute name="Option">Additional Info</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DC'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Charge</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'VC'">
				<CarOption>
					<xsl:attribute name="Option">Merchant Currency</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'AC'">
				<CarOption>
					<xsl:attribute name="Option">Alternate Currency</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'DO'">
				<CarOption>
					<xsl:attribute name="Option">DropOff Location</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
					</Text>
				</CarOption>
			</xsl:when>
			<xsl:when test="type = 'TN'">
				<CarOption>
					<xsl:attribute name="Option">Tour Number</xsl:attribute>
					<Text>
						<xsl:value-of select="freetext" />
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
	<xsl:template match="dataElementsIndiv" mode="phone">
		<xsl:if test="otherDataFreetext/freetextDetail/type!='P02'">		
			<Telephone>
				<xsl:attribute name="PhoneUseType">
					<xsl:choose>
						<xsl:when test="otherDataFreetext/freetextDetail/type='4'">H</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='5'">O</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='P01'">F</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='6'">A</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='3'">B</xsl:when>
						<xsl:otherwise>O</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="PhoneNumber">
						<xsl:value-of select="otherDataFreetext/longFreetext"/>
				</xsl:attribute>		
		
			</Telephone>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Ticketing Element	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ticketing">
		<Ticketing>		
			<xsl:if test="ticketElement/ticket/time != ''">			
				<xsl:attribute name="TicketTimeLimit">				
							<xsl:text>20</xsl:text>		
							<xsl:value-of select="substring(ticketElement/ticket/date,5,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(ticketElement/ticket/date,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(ticketElement/ticket/date,1,2)"/>
							<xsl:text>T</xsl:text>		
							<xsl:value-of select="substring(ticketElement/ticket/time,1,2)"/>:<xsl:value-of select="substring(ticketElement/ticket/time,3,2)"/>
							<xsl:text>:00</xsl:text>		
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="TicketType">
				<xsl:choose>				
					<xsl:when test="ticketElement/ticket/electronicTicketFlag !=''">eTicket</xsl:when>
					<xsl:otherwise>Paper</xsl:otherwise>				
				</xsl:choose>	
			</xsl:attribute>	
			<xsl:if test="ticketElement/ticket/indicator = 'OK'">	
				<TicketAdvisory>
					<xsl:text>OK-</xsl:text>
					<xsl:value-of select="ticketElement/ticket/date"/>
					<xsl:text>/</xsl:text>
					<xsl:value-of select="ticketElement/ticket/officeId"/>
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
	<xsl:template match="dataElementsIndiv" mode="email"> <!--done -->
		<xsl:if test="otherDataFreetext/freetextDetail/type='P02'">
				<Email>
					<xsl:value-of select="otherDataFreetext/longFreetext"/>
				</Email>		
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Form of Payment	   		                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Payment"> 
		<xsl:choose>
			<xsl:when test="substring(otherDataFreetext/longFreetext,1,2) ='CC' or substring(otherDataFreetext/longFreetext,1,6) = 'PAX CC'">
				<xsl:attribute name="RPH"><xsl:value-of select="position()"/></xsl:attribute>
				<PaymentCard>
					<xsl:attribute name="CardCode">
						<xsl:choose>
							<xsl:when test="substring(substring-after(otherDataFreetext/longFreetext,'CC'),1,2) = 'CA'">MC</xsl:when>
							<xsl:otherwise><xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'CC'),1,2)"/></xsl:otherwise>
						</xsl:choose>					
					</xsl:attribute>
					<xsl:attribute name="CardNumber">
						<xsl:value-of select="translate(substring(substring-after(otherDataFreetext/longFreetext,'CC'),3,16),'/T','')"/>
					</xsl:attribute>
					<xsl:attribute name="ExpireDate">
						<xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'/'),1,2)"/>
						<xsl:value-of select="substring(substring-after(otherDataFreetext/longFreetext,'/'),3,2)"/>
					</xsl:attribute>		
				</PaymentCard>		
				<TPA_Extensions>
					<xsl:attribute name="FOPType">CC</xsl:attribute>
				</TPA_Extensions>
			</xsl:when>
			<xsl:when test="otherDataFreetext/longFreetext = 'CHECK'">
				<xsl:attribute name="RPH"><xsl:value-of select="position()" /></xsl:attribute>
				<TPA_Extensions>
					<xsl:attribute name="FOPType">CHECK</xsl:attribute>
				</TPA_Extensions>
			</xsl:when>
		</xsl:choose>
	</xsl:template>				
	<!-- ************************************************************** -->
	<!-- Address	   		                                   		 -->  
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Address">
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
					<xsl:apply-templates select="structuredAddress/address"/>
				</xsl:when>
				<xsl:otherwise>
					<AddressLine>
						<xsl:value-of select="otherDataFreetext/longFreetext"/>
					</AddressLine>
				</xsl:otherwise>
			</xsl:choose>
		</Address>
	</xsl:template>
	
	<xsl:template match="structuredAddress/address">
		<xsl:if test="option='A1'">
			<StreetNmbr>
				<xsl:value-of select="optionText"/>
			</StreetNmbr>
		</xsl:if>
		<xsl:if test="option='CI'">
			<CityName>
				<xsl:value-of select="optionText"/>
			</CityName>
		</xsl:if>
		<xsl:if test="option='ZP'">
			<PostalCode>
				<xsl:value-of select="optionText"/>
			</PostalCode>
		</xsl:if>
		<xsl:if test="option='ST'">
			<StateProv>
				<xsl:choose>
					<xsl:when test="string-length(optionText) = 2">
						<xsl:attribute name="StateCode"><xsl:value-of select="optionText"/></xsl:attribute>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="optionText"/></xsl:otherwise>
				</xsl:choose>
			</StateProv>
		</xsl:if>
		<xsl:if test="option='CO'">
			<CountryName>
				<xsl:choose>
					<xsl:when test="string-length(optionText) = 2">
						<xsl:attribute name="Code"><xsl:value-of select="optionText"/></xsl:attribute>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="optionText"/></xsl:otherwise>
				</xsl:choose>
			</CountryName>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- General Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="GenRemark">
		<Remark>	
			<xsl:if test="miscellaneousRemarks/remarks/category != ''">		
				<xsl:attribute name="Category"><xsl:value-of select="miscellaneousRemarks/remarks/category"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>		
		</Remark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--Itinerary Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ItinRemark">	
		<SpecialRemark>
			<xsl:attribute name="RemarkType">Itinerary</xsl:attribute>				
			<FlightRefNumber>
				<xsl:attribute name="RPH">
					<xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
				</xsl:attribute>
			</FlightRefNumber>
			<Text><xsl:value-of select="miscellaneousRemarks/remarks/freetext"/></Text>
		</SpecialRemark>		
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Invoice Remarks	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="InvoiceRemark">
		<SpecialRemark>
			<xsl:attribute name="RemarkType">Invoice</xsl:attribute>				
				<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/>
						</xsl:attribute>
				</FlightRefNumber>
				<Text><xsl:value-of select="miscellaneousRemarks/remarks/freetext"/></Text>
		</SpecialRemark>			
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Invoice and itinerary Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="InvoiceItinRemark">
		<SpecialRemark>
			<xsl:choose>
				<xsl:when test="referenceForDataElement/reference/qualifier='ST'">
					<xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
					<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:variable name="rph"><xsl:value-of select="referenceForDataElement/reference[qualifier='ST']/number"/></xsl:variable>
							<xsl:value-of select="../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number = $rph]/lineNumber"/>
						</xsl:attribute>
					</FlightRefNumber>	
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="RemarkType">Invoice</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text><xsl:value-of select="miscellaneousRemarks/remarks/freetext"/></Text>
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
	<xsl:template match="dataElementsIndiv" mode="SSR">
			<SpecialServiceRequest>
				<xsl:attribute name="SSRCode">
					<xsl:value-of select="serviceRequest/ssr/type"/>
				</xsl:attribute>	
				<Airline>
					<xsl:attribute name="Code">
						<xsl:value-of select="serviceRequest/ssr/companyId"/>
					</xsl:attribute>
				</Airline>
				<Text><xsl:value-of select="serviceRequest/ssr/freeText"/></Text>
			</SpecialServiceRequest>		
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Seat Elements 	                               		    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Seat">
		<xsl:for-each select="serviceRequest/ssrb">
			<SeatRequest>
				<xsl:if test="data != ''">
					<xsl:attribute name="SeatNumber">
						<xsl:value-of select="data"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="seatType != ''">
					<xsl:attribute name="SeatPreference">
						<xsl:value-of select="seatType"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="data = 'N'">
					<xsl:attribute name="SmokingAllowed"><xsl:text>false</xsl:text></xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='PT']" mode="SeatPassAssoc"/>
				<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
				<DepartureAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/boardpoint"/></xsl:attribute>
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/offpoint"/></xsl:attribute>
				</ArrivalAirport>
			</SeatRequest>
		</xsl:for-each>
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
		<xsl:attribute name="TravelerRefNumberRPHList">
			<xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
		</xsl:attribute>
	</xsl:template>
	
	<xsl:template match="reference" mode="SeatSegAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:attribute name="FlightRefNumberRPHList">
			<xsl:value-of select="//originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number=$Tattoo]/lineNumber"/>
		</xsl:attribute>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Form of Payment	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="fop">
		<xsl:if test="ElementType='FP'">
			<FOP>
				<xsl:if test="CAPI_PNR_NonFormattedElement/InformationType='16'">
					<ElementNumber>
						<xsl:attribute name="TattooNumber"><xsl:value-of select="elementManagementData/reference[qualifier='OT']/number"/></xsl:attribute>
						<xsl:attribute name="TattooQualifier"><xsl:value-of select="elementManagementData/reference/qualifier"/></xsl:attribute>
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
			<xsl:choose>
				<xsl:when test="substring(otherDataFreetext/longFreetext,1,3) = '*M*' or substring(otherDataFreetext/longFreetext,1,3) = '*C*'">
					<xsl:choose>
						<xsl:when test="contains(otherDataFreetext/longFreetext,'A')">
							<xsl:attribute name="Amount"><xsl:value-of select="substring-before(substring(otherDataFreetext/longFreetext,4),'A')"/></xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Percent"><xsl:value-of select="substring(otherDataFreetext/longFreetext,4)"/></xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'A')">
					<xsl:attribute name="Amount"><xsl:value-of select="substring-before(otherDataFreetext/longFreetext,'A')"/></xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Percent"><xsl:value-of select="otherDataFreetext/longFreetext"/></xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
		</AgencyCommission>
	</xsl:template>
	
	<xsl:template match="dataElementsIndiv" mode="accounting">
		<AccountingLine>
			<xsl:value-of select="accounting/account/number"/>
		</AccountingLine>
	</xsl:template>

	<!-- ********************************************************************************	-->
	<!-- Miscellaneous other,  Air Taxi, Land, Sea, Rail, Car and hotel               -->
	<!-- ********************************************************************************* -->
	<xsl:template match="itineraryInfo" mode="Other">  <!--General Segment in OTA -->
		<!-- Other Segments -->
		<Item>
			<xsl:attribute name="Status"><xsl:value-of select="relatedProduct/status"/></xsl:attribute>
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
				<!--TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions-->
			</General>
		</Item>		
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Taxi"> <!--General OTA Segments -->
		<!-- Taxi Segments -->
		<Item>
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
					<Description><xsl:value-of select="itineraryFreetext/longFreetext"/></Description>
				</xsl:if>
				<TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions>
			</General>
		</Item>				
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Land"><!--OTA Package -->
		<!-- Surface Segments -->
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
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
					<Description><xsl:value-of select="itineraryFreetext/longFreetext"/></Description>
				</xsl:if>
				<TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions>
			</Package>
		</Item>				
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Train"> <!-- OTA Rail -->
		<!-- Rail Segments -->
			<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
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
					<Description><xsl:value-of select="itineraryFreetext/longFreetext"/></Description>
				</xsl:if>
				<TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions>
			</Rail>
		</Item>				
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Cruise"> <!-- OTA Cruise-->
		<!-- Sea Segments -->
			<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
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
					<Description><xsl:value-of select="itineraryFreetext/longFreetext"/></Description>
				</xsl:if>
				<TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions>
			</Cruise>
		</Item>				
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="Tour"> <!--OTA Tour-->
		<!--Tour Segments -->
		<Item>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
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
					<Description><xsl:value-of select="itineraryFreetext/longFreetext"/></Description>
				</xsl:if>
				<TPA_Extensions>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="travelProduct/companyDetail/identification"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></OriginCityCode>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="relatedProduct/quantity"/>
					</xsl:attribute>				
				</TPA_Extensions>
			</Tour>
		</Item>				
	</xsl:template>
	<!--***************************************************************************************************-->
	<xsl:template match="itineraryInfo" mode="CarPassive">
		<!-- Car Segments -->
		<CarPassiveSegment>
			<ElementNumber>
				<xsl:attribute name="TattooNumber"><xsl:value-of select="elementManagementItinerary/reference[qualifier='ST']/number"/></xsl:attribute>
				<xsl:attribute name="TattooQualifier"><xsl:value-of select="elementManagementItinerary/reference/qualifier"/></xsl:attribute>
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
				<Date><xsl:value-of select="travelProduct/product/depDate"/></Date>
			</PickUpInfo>
			<DropOffInfo>
				<Date><xsl:value-of select="travelProduct/product/arrDate"/></Date>
			</DropOffInfo>
			<CarType/>
			<xsl:if test="itineraryFreetext/longFreetext!=''">
				<Text>
					<xsl:value-of select="itineraryFreetext/longFreetext"/>
				</Text>
			</xsl:if>
		</CarPassiveSegment>
	</xsl:template>
</xsl:stylesheet>
