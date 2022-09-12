<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!--
   ================================================================== 
   v03_AmadeusWS_PNRReadRS.xsl 												       
   ================================================================== 
   Date: 09 Aug 2022 - Kobelev - Confirmation Number for FOP as part of CC.
   Date: 09 Aug 2022 - Kobelev - Better Pax Birthday handler.
   Date: 19 Jul 2022 - Kobelev - Error node for failed FOP change.
   Date: 29 Jun 2022 - Kobelev - FareBasis Codes were getting cut off for CHD and INF.
   Date: 03 Jun 2022 - Kobelev - Birth Date of passanger from SSR DOCS.
   Date: 29 Apr 2022 - Kobelev - EMD Exchange and EMD Service Fee display fix.
   Date: 28 Apr 2022 - Kobelev - Handle of FOP Errors in case if GDS regected a card.
   Date: 25 Mar 2022 - Kobelev - Added to SupplementalInfo information on criteriaDetails.
   Date: 06 Jan 2022 - Kobelev - Passanger Information display ADT/CHLD vs. INF.
   Date: 21 Oct 2021 - Kobelev - Added handling of Corporate special remarks RX.
   Date: 21 Oct 2021 - Kobelev - Change Controlling Carrier RemarkType from "Z" to "CC".
   Date: 16 Aug 2021 - Kobelev - Controlling Carrier Identification.
   Date: 15 Jul 2021 - Samokhvalov - Fix Branded Fares display
   Date: 21 Apr 2021 - Kobelev - Added Quantity to SSR HK flag.
   Date: 09 Oct 2020 - Samokhvalov - Fix Branded Fares display
   Date: 05 Oct 2020 - Kobelev - OperatingAirline identification when there are two Operated By lines in one airsegment.
   Date: 28 Sep 2020 - Kobelev - Removed IataCode from condition during CompanyName build process.
   Date: 16 Jul 2020 - Samokhvalov - Displaying Branded Fares
   Date: 10 Jul 2020 - Kobelev - Multiple Pricing Option Key display.
   Date: 07 Jul 2020 - Kobelev - Pricing Option Key display.
   Date: 12 Mar 2020 - Kobelev - Fix Bag Allowed display.
   Date: 21 Jan 2020 - Kobelev - MCO Voided in totalCost node. FIX BUG 1173 Amadeus - Identifying Void MCOs.
   Date: 19 Nov 2019 - Kobelev - PNR with No TST and/or with No Air Segments.
   Date: 11 Nov 2019 - Kobelev - Air Segment - OperatingAirline fix.
   Date: 16 Oct 2019 - Kobelev - Air Segment - OperatingAirline fix for complex Operated By cases
   Date: 25 Sep 2019 - Samokhvalov - Air Segment - ARNK filter fixes
   Date: 21 Aug 2019 - Kobelev - Formating Flight Number to 4 digit code
   Date: 23 May 2019 - Kobelev - Start using AltLangID as GDS idetifier
   Date: 07 Sep 2018 - Samokhvalov - Added FOPType to MCO payment template.
   Date: 05 May 2019 - Kobelev - Added FCMI code to PTC_FareBreakdown
   Date: 07 Sep 2018 - Kobelev - Tour Code from TST.
   Date: 07 Aug 2018 - Kobelev - Added RPH to Telephone field PIN property will be populated with this value.
   Date: 14 Jun 2018 - Kobelev - CompanyName object with Creation Date Time added.
   Date: 10 Jan 2018 - Kobelev - Air Segment with train or other ARNK segments.
   Date: 12 Dec 2017 - Kobelev - Displaying lastagent information in the Company value.
   Date: 17 Nov 2017 - Samokhvalov - Removed EquivFare from display	for awad only morqua - beacuse this is old code. PassangeFare corrected -> amount per 1 pax.
   Date: 17 Nov 2017 - Kobelev - Removed EquivFare from display	for awad only morqua - beacuse this is old code.
   Date: 28 Apr 2017 - Kobelev - Added Trim functionality.
   Date: 21 Mar 2017 - Kobelev - Added APX identifing process.
   Date: 15 Mar 2017 - Kobelev - Added UHP read in Special Remarks.
   Date: 20 Dec 2016 - Kobelev - Added PaymentAmount in case of FOP CHECK.
   Date: 16 Dec 2016 - Kobelev - Added Ticket Designator to Fare Bases Code with "/"
   Date: 22 Nov 2016 - Kobelev - TravelerRefNumberRPHList in Agency Commission FM element.
   Date: 10 Nov 2016 - Samokhvalov - corrected ItinSeqNumber of ARNK ReservationItems records.
   Date: 07 Nov 2016 - Kobelev - New client (EROS) uses *V* in FM line.Exch
   Date: 24 Oct 2016 - Kobelev - ARNK FareBases code marked as VOID. 
   Date: 24 Oct 2016 - Kobelev - FOP traveler reference number and PTC identifier. 
   Date: 08 Jun 2016 - Rastko	- corrected mapping of operating airline to add code FIX BUG: 316
   Date: 24 May 2016 - Rastko	- corrected mapping private/published fares			
   Date: 21 Apr 2016 - Rastko	- corrected mapping of commission in MCO
   Date: 15 Mar 2016 - Kobelev - FOP re-approval code				
   Date: 24 Nov 2015 - Rastko	- mapped OrginDestType info				
   Date: 17 Jun 2015 - Rastko	- mapped segment marriage info				
   Date: 16 Jun 2015 - Rastko	- mapped Chargeable seat					
   Date: 31 Mar 2015 - Rastko - added passenger and segment association to remarks	
   Date: 23 Feb 2015 - Tkachenko - added CompanyName to the PNR header information	
   Date: 05 Feb 2015 - Rastko - added Code attribute to Error when available				
   Date: 23 Jan 2015 - Rastko - added reference to record locator from split PNR				
   Date: 11 Dec 2014 - Rastko - corrected mapping of commission in form of payment				
   Date: 10 Dec 2014 - Rastko - changed logic to determine flight operating airline					
   Date: 01 Dec 2014 - Rastko - added test for error in returned PNR						
   Date: 29 Oct 2014 - Rastko - map TST fcmi=G parameter to Private fare tag					
   Date: 29 Jul 2014 - Rastko - added display of car voucher						
   Date: 12 Jun 2014 - Rastko - added RPH attribute to Remark elements			
   Date: 21 May 2014 - Rastko - corrected RPH maping of FOP CC				
   Date: 20 May 2014 - Rastko - corrected mapping of pax and seg association in validating carrier	
   Date: 16 May 2014 - Rastko - corrected mapping of pax and seg association in validating carrier	
   Date: 08 May 2014 - Rastko - when INF does not have surname, take one from adult	
   Date: 15 Apr 2014 - Rastko - added support for passenger types in commission element	
   Date: 28 Mar 2014 - Rastko - corrected mapping of car locationa for car passive segs	
   Date: 19 Mar 2014 - Rastko - corrected mapping of car locationa to show amadeus codes	
   Date: 16 Jan 2014 - Rastko - mapped accounting info and future price info and PTC Breakdown RPH	
   Date: 26 Dec 2013 - Rastko - added mapping of validating airline in fare structure		
   Date: 31 Oct 2013 - Rastko - corrected FOP containing original CC info			
   Date: 17 Apr 2013 - Rastko - corrected ticket status process					
   Date: 21 Mar 2013 - Rastko - corrected flight duration when flight includes stops	 	
   Date: 18 Mar 2013 - Rastko - removed EquivFare from display	for awad only (morqua) 	
   Date: 18 Mar 2013 - Rastko - removed EquivFare from display				 	
   Date: 14 Mar 2013 - Rastko - added case when there is no flight info for flight duration 	
   Date: 01 Mar 2013 - Rastko - added mapping of flight duration in air segment		
   Date: 14 Feb 2012 - Rastko - corrected pax type mapping in fare list response		
   Date: 24 Jan 2012 - Rastko - improved mapping of rail segment				
   Date: 15 Dec 2012 - Rastko - added support for getting only record locator back		 
   Date: 14 Dec 2012 - Laleen - Fixed the tax fare getting Minus values Issue			 
   Date: 28 Nov 2012 - Rastko - corrected mapping of FT tour code element			 
   Date: 27 Nov 2012 - Rastko - added mapping of fare rules						 
   Date: 26 Nov 2012 - Rastko - mapped airline fee in total and passenger fares		 
   Date: 12 Nov 2012 - Rastko - added CouponStatus to Air segment				 
   Date: 09 Nov 2012 - Rastko - improved telephone type mapping				 
   Date: 06 Nov 2012 - Rastko - corrected decimal number bug with EquivFare mapping	 
   Date: 05 Nov 2012 - Rastko - added EquivFare mapping						 
   Date: 15 Oct 2012 - Rastko - corrected number of decimals mapping in passenger base fare	 
   Date: 11 Oct 2012 - Rastko - corrected parsing of FOP Cash				 
   Date: 31 Aug 2012 - Rastko - corrected missing CC codes in FOP list		 
   Date: 07 Jul 2012 - Rastko - corrected equivalent base fare amount and currency 
   Date: 30 May 2012 - Shashin  -fixed the issue displya ADT with INF				
   Date: 30 May 2012 - Rastko - corrected equivalent base fare amount and currency	
   Date: 10 May 2012 - Shashin - fixed the issue when equivalent fare does not come	
   Date: 30 Apr 2012 - Shashin - calculate total tax when total comes as 0		
   Date: 04 Apr 2012 - Rastko - added attribute Status to Item element 			
   Date: 15 Mar 2012 - Shashin - Fixed minus tax value and NaN when no amount is available for E 
   Date: 14 Mar 2012 - Rastko - in FOP check that card code is valid 			
   Date: 09 Mar 2012 - Rastko - added a catch all FOP (misc charge order)		
   Date: 09 Mar 2012 - Rastko - fixed FOP display for finnish market			
   Date: 08 Mar 2012 - Rastko - fixed FOP display for spanish and swedish markets	
   Date: 08 Mar 2012 - Rastko - fixed issue in FOP change display				
   Date: 08 Mar 2012 - Rastko - added misc payment in FOP element			
   Date: 06 Mar 2012 - Rastko - fixed amount in cash FOP and car drop off time		
   Date: 22 Feb 2012 - Rastko - added suplemental info to FOP element			
   Date: 21 Feb 2012 - Rastko - changed FOP to support amounts and multiple FOP line	
   Date: 08 Feb 2012 - Rastko - display misc segment even when no travel segments in PNR 
   Date: 07 Feb 2012 - Rastko - corrected mapping of reservation number		
   Date: 05 Dec 2011 - Rastko - corrected mapping of SSR FQTV				
   Date: 24 Nov 2011 - Rastko - display BagAllowance only when bag type present in xml	
   Date: 14 Nov 2011 - Shashin - Added FQTV number in SSR section				
   Date: 30 Aug 2011 - Rastko - made ADT default pax type						
   Date: 09 Aug 2011 - Rastko - added mapping for BagAllowance				
   Date: 08 Aug 2011 - Rastko - corrected PNR creation date time info			
   Date: 02 Aug 2011 - Shashin - fixed invalid PickUpDateTime value
   Date: 01 Jul 2011 - Rastko - fixed CC parsing for different CC type entries		       
   Date: 30 Jun 2011 - Shashin - fixed invalid CardCode value
   Date: 25 Jun 2011 - Rastko - fixed commission parsing when percent in it		       
   Date: 23 Jun 2011 - Shashin - fixed invalid format in Percentage
   Date: 12 May 2011 - Shashin - added XL to TicketAdvisory	       
   Date: 04 Apr 2011 - Rastko - added another Amadeus code to test as private fare	       
   Date: 29 Mar 2011 - Rastko - added NONAME for passengers without names		       
   Date: 14 Mar 2011 - Rastko - set infant surname to NONAME when not in response	       
   Date: 01 Mar 2011 - Rastko - corrected parsing of credit card	and commission		       
   Date: 28 Feb 2011 - Rastko - corrected parsing of MCO credit card			       
   Date: 16 Feb 2011 - Rastko - added parsing of operating airline				       
   Date: 01 Feb 2011 - Rastko - fixed parsing error in car VehicleCharge			       
   Date: 31 Jan 2011 - Rastko - fixed parsing of CC info for air mauritius			       
   Date: 14 Jan 2010 - Rastko - fixed parsing of MCO CC info					       
   Date: 12 Jan 2010 - Rastko - fixed commission parsing when *D* in it			       
   Date: 06 Jan 2010 - Rastko - fixed CC parsing in MCO element				       
   Date: 19 Dec 2010 - Rastko - fixed commission parsing						       
   Date: 15 Dec 2010 - Rastko - fixed CC parsing in MCO element				       
   Date: 10 Dec 2010 - Rastko - fixed commission parsing when INF in it			       
   Date: 05 Dec 2010 - Rastko - added mappimg for Errors						       
   Date: 17 Nov 2010 - Rastko - fixed parsing error in car VehicleCharge			       
   Date: 09 Nov 2010 - Rastko - fixed various parsing errors in FOP and commission	       
   Date: 25 Oct 2010 - Rastko - fixed commission parsing						       
   Date: 20 Oct 2010 - Rastko - fixed commission percent parsing				       
   Date: 01 Oct 2010 - Rastko - added mapping for mobile phone				       
   Date: 29 Sep 2010 - Rastko - fixed parsing of car charges rate				       
   Date: 22 Sep 2010 - Rastko - fixed few schema validation errors				       
   Date: 15 Sep 2010 - Rastko - fixed few schema validation errors				       
   Date: 06 Sep 2010 - Rastko - correced few schema validation errors			       
   Date: 23 Aug 2010 - Rastko - correced time sent by Amadeus from 24:00 to 00:00	       
   Date: 17 Aug 2010 - Rastko - fixed commission amount parsing				       
   Date: 13 Aug 2010 - Rastko - mapped MC and CA credit card vendors			       
   Date: 06 Jul 2010 - Rastko - fixed commission amount parsing				       
   Date: 26 Feb 2010 - Rastko - fixed mapping of hotel segment					       
   ================================================================== 
   -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="userid" select="//POS/TPA_Extensions/Provider/Userid"/>
	<xsl:variable name="segcount" select="count(//fareList/segmentInformation)"/>
	<xsl:variable name="tktcount" select="count(//Ticket_DisplayTSTReply/fareList)"/>
	<xsl:variable name="count1">
		<xsl:choose>
			<xsl:when test="$tktcount != 0">
				<xsl:value-of select="($segcount div $tktcount)"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="loop">
		<xsl:choose>
			<xsl:when test="//Ticket_DisplayTSTReply/fareList/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
				<xsl:value-of select="(count(//Ticket_DisplayTSTReply/fareList) div 2) + 1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="count(//Ticket_DisplayTSTReply/fareList) + 1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_RetrieveByRecLocReply"/>
		<xsl:apply-templates select="PNR_Reply"/>
		<xsl:apply-templates select="MessagesOnly_Reply"/>
		<xsl:apply-templates select="Errors"/>
		<xsl:apply-templates select="Queue_Start_Reply"/>
		<xsl:apply-templates select="Queue_Count_Reply"/>
	</xsl:template>

	<xsl:template match="MessagesOnly_Reply | Queue_Start_Reply | Queue_Count_Reply">
		<OTA_TravelItineraryRS Version="v03" AltLangID="Amadeus">
			<Errors>
				<xsl:apply-templates select="CAPI_Messages"/>
			</Errors>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="Errors">
		<OTA_TravelItineraryRS Version="v03" AltLangID="Amadeus">
			<Errors>
				<xsl:apply-templates select="Error" mode="error"/>
			</Errors>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode"/>
			</xsl:attribute>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Text"/>
		</Error>
	</xsl:template>

	<xsl:template match="PNR_RetrieveByRecLocReply | PNR_Reply">
		<OTA_TravelItineraryRS>
			<xsl:attribute name="Version">
				<xsl:value-of select="'v03'"/>
			</xsl:attribute>
			<xsl:attribute name="AltLangID">
				<xsl:value-of select="'Amadeus'"/>
			</xsl:attribute>
			<xsl:if test="EchoToken!=''">
				<xsl:attribute name="EchoToken">
					<xsl:value-of select="EchoToken"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="generalErrorInfo/messageErrorInformation[errorDetail/qualifier='EC']">
					<Errors>
						<xsl:for-each select="generalErrorInfo/messageErrorText/text">
							<Error Type="Amadeus">
								<xsl:value-of select="."/>
							</Error>
						</xsl:for-each>
					</Errors>
				</xsl:when>
				<xsl:when test="pnrHeader/reservationInfo/reservation/controlNumber!='' or dataElementsMaster/dataElementsIndiv/elementErrorInformation">

					<xsl:choose>
						<xsl:when test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/elementManagementData/status = 'ERR' or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO']/elementManagementData/status = 'ERR'">
							<Errors>
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP' ]" mode="FOPerror"/>
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO' ]" mode="FOPerror"/>
							</Errors>
						</xsl:when>
						<!--
						<xsl:when test="originDestinationDetails/itineraryInfo[not (elementManagementItinerary/segmentName='AIR')]">
							<Errors>
								<Error Type="Amadeus">
									No Air Segments in PNR
								</Error>
							</Errors>
						</xsl:when>
						-->
						<xsl:otherwise>
							<Success/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:choose>
						<xsl:when test="Ticket_DisplayTSTReply[(applicationError)]">
							<Warnings>
								<xsl:if test="originDestinationDetails/itineraryInfo[not (elementManagementItinerary/segmentName='AIR')]">
									<Warning Type="Amadeus">NO AIR SEGMENTS IN PNR</Warning>
								</xsl:if>
								<xsl:apply-templates select="Ticket_DisplayTSTReply/applicationError/errorText/errorFreeText" mode="warning"/>
							</Warnings>
						</xsl:when>
						<xsl:when test="Error">
							<Errors>
								<xsl:apply-templates select="Error" mode="error"/>								
							</Errors>
						</xsl:when>
						<xsl:when test="Warning">
							<Warnings>
								<xsl:apply-templates select="Warning" mode="warning"/>
							</Warnings>
						</xsl:when>
					</xsl:choose>

					<TravelItinerary>
						<xsl:choose>
							<xsl:when test="pnrHeader[not(reservationInfo/reservation/controlType) and reservationInfo/reservation/companyId='1A' and not(reservationInfo/reservation/date)] and not(travellerInfo)">
								<xsl:apply-templates select="pnrHeader[not(reservationInfo/reservation/controlType) and reservationInfo/reservation/companyId='1A' and not(reservationInfo/reservation/date)]" mode="header"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:apply-templates select="pnrHeader[not(reservationInfo/reservation/controlType) and reservationInfo/reservation/companyId='1A' and reservationInfo/reservation/date!='']" mode="header"/>
								<CustomerInfos>
									<!--
									  <xsl:apply-templates select="travellerInfo/passengerData"/>
									-->
									<xsl:apply-templates select="travellerInfo/enhancedPassengerData"/>
								</CustomerInfos>
								<ItineraryInfo>
									<xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL'] | originDestinationDetails/itineraryInf[elementManagementItinerary/segmentName='HU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] | 	originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
										<ReservationItems>
											<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']" mode="Air"/>
											<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CCR']" mode="Car"/>
											<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CU']" mode="Car"/>
											<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HHL']" mode="Hotel"/>
											<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='HU']" mode="Hotel"/>
											<xsl:if test="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU'] | originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']">
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TRN']" mode="Train"/>
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']" mode="Cruise"/>
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='TU']" mode="Tour"/>
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']" mode="Other"/>
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AU']" mode="Taxi"/>
												<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='SUR']" mode="Land"/>
											</xsl:if>
											<xsl:if test="Ticket_DisplayTSTReply[not(applicationError)]">
												<ItemPricing>
													<xsl:apply-templates select="Ticket_DisplayTSTReply"/>
												</ItemPricing>
											</xsl:if>
										</ReservationItems>
									</xsl:if>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='TK'][1]" mode="ticketing"/>
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
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RX']" mode="Corporate"/>
											</Remarks>
										</xsl:if>
										<xsl:if test="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RI')] or dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RC')] 	or 	dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT'] or  dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RX']">
											<SpecialRemarks>
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RI')]" mode="InvoiceItinRemark"/>
												<!--
                        <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIF']" mode="InvoiceRemark"/>
											  <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIT']" mode="InvoiceRemark"/>
											  <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIR']" mode="ItinRemark"/>
                        -->
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RC')]" mode="ConfRemark"/>
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE']" mode="Endorsement"/>

												<xsl:choose>
													<xsl:when test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT']">
														<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT']" mode="TourCode"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:apply-templates select="Ticket_DisplayTSTReply/fareList/otherPricingInfo/attributeDetails[attributeType='PAY']" mode="TourCode"/>
													</xsl:otherwise>
												</xsl:choose>

												<xsl:if test="pricingRecordGroup/productPricingQuotationRecord/documentDetailsGroup/fareComponentDetailsGroup[fareFamilyOwner]">
													<xsl:apply-templates select="pricingRecordGroup/productPricingQuotationRecord/documentDetailsGroup/fareComponentDetailsGroup" mode="controllingCarrier"/>
												</xsl:if>

											</SpecialRemarks>
										</xsl:if>
										<xsl:if test="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'PL')]">
											<UniqueRemarks>
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='PL']" mode="UHP"/>
											</UniqueRemarks>
										</xsl:if>
									</SpecialRequestDetails>
									<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV' or elementManagementData/segmentName='FO' or elementManagementData/segmentName='FA' or 		elementManagementData/segmentName='FB' or elementManagementData/segmentName='FI' or substring(elementManagementData/segmentName,1,2)='FH']">
										<TPA_Extensions>
											<IssuedTickets>
												<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[(elementManagementData/segmentName='FA' or elementManagementData/segmentName='FB')]" mode="IssuedTicket"/>
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
								<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SP']">
									<AssociatedContent>
										<AssocItems>
											<xsl:for-each select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SP']">
												<AssocItem RPH="{position()}" Type="21">
													<AssocItemRef Type="21">
														<xsl:attribute name="ID">
															<xsl:value-of select="referencedRecord/referencedReservationInfo/reservation/controlNumber"/>
														</xsl:attribute>
														<xsl:attribute name="ID_Context">
															<xsl:value-of select="'AXR'"/>
														</xsl:attribute>
													</AssocItemRef>
												</AssocItem>
											</xsl:for-each>
										</AssocItems>
									</AssociatedContent>
								</xsl:if>
								<!--xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA']" mode="ticketnumber"/-->
								<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO']">
									<TravelCost>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP' ]" mode="Payment"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='MCO' ]" mode="MCO"/>
									</TravelCost>
								</xsl:if>
								<UpdatedBy>
									<xsl:attribute name="CreateDateTime">
										<xsl:value-of select="format-number(substring(securityInformation/secondRpInformation/creationDate,5,2),'2000')"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring		(securityInformation/secondRpInformation/creationDate,3,2)"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(securityInformation/secondRpInformation/creationDate,1,2)"/>
										<xsl:text>T</xsl:text>
										<xsl:value-of select="substring		(securityInformation/secondRpInformation/creationTime,1,2)"/>
										<xsl:text>:</xsl:text>
										<xsl:value-of select="substring(securityInformation/secondRpInformation/creationTime,3,2)"/>
										<xsl:text>:00</xsl:text>
									</xsl:attribute>
								</UpdatedBy>
								<xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FB'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'] or dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']">
									<TPA_Extensions>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']" mode="commission"/>
										<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']" mode="accounting"/>
									</TPA_Extensions>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</TravelItinerary>
				</xsl:when>
				<xsl:when test="Error">
					<Errors>
						<xsl:apply-templates select="Error" mode="error"/>
					</Errors>
				</xsl:when>
				<xsl:when test="dataElementsMaster/dataElementsIndiv/elementErrorInformation">
					<Errors>
						<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementErrorInformation]" mode="FOPerror"/>
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
			<xsl:if test="ConversationID!=''">
				<ConversationID>
					<xsl:value-of select="ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="Error" mode="error">
		<Error Type="Amadeus">
			<xsl:if test="@Code!=''">
				<xsl:attribute name="Code">
					<xsl:value-of select="@Code"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="."/>
		</Error>
	</xsl:template>

	<xsl:template match="dataElementsIndiv" mode="FOPerror">
		<Error Type="Amadeus">
			<xsl:if test="elementErrorInformation">
				<xsl:attribute name="Code">
					<xsl:value-of select="concat(elementErrorInformation/errorOrWarningCodeDetails/errorDetails/errorCode, '-', elementErrorInformation/errorOrWarningCodeDetails/errorDetails/errorCategory)"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="elementErrorInformation/errorWarningDescription/freeText"/>
		</Error>
		<Error Type="Amadeus">
			<xsl:if test="elementErrorInformation">
				<xsl:attribute name="Code">
					<xsl:value-of select="concat(elementErrorInformation/errorOrWarningCodeDetails/errorDetails/errorCode, '-', elementErrorInformation/errorOrWarningCodeDetails/errorDetails/errorCategory)"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</Error>
	</xsl:template>

	<xsl:template match="Text">
		<Text>
			<xsl:value-of select="."/>
		</Text>
	</xsl:template>

	<xsl:template match="Error | Warning | errorFreeText | text()" mode="warning">
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
			<xsl:attribute name="ID">
				<xsl:value-of select="reservationInfo/reservation/controlNumber"/>
			</xsl:attribute>
			<xsl:if test="../securityInformation/responsibilityInformation/officeId != ''">
				<xsl:attribute name="ID_Context">
					<xsl:value-of select="../securityInformation/responsibilityInformation/officeId"/>
				</xsl:attribute>
			</xsl:if>
			<!-- /iataCode -->
			<xsl:if test="../securityInformation/responsibilityInformation != ''">
				<CompanyName>
					<!-- 140618 -->
					<xsl:variable name="CreationDate">
						<xsl:choose>
							<xsl:when test="string-length(../securityInformation/secondRpInformation/creationDate) > 6">
								<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationDate,5,4)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationDate,3,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationDate,1,2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(20, substring(../securityInformation/secondRpInformation/creationDate,5,2))"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationDate,3,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationDate,1,2)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<!-- 1611 -->
					<xsl:variable name="CreationTime">
						<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationTime,1,2)"/>
						<xsl:text>:</xsl:text>
						<xsl:value-of select="substring(../securityInformation/secondRpInformation/creationTime,3,2)"/>
					</xsl:variable>

					<xsl:attribute name="Code">
						<xsl:value-of select="concat(../securityInformation/responsibilityInformation/iataCode, '|',$CreationDate, 'T', $CreationTime)"/>
					</xsl:attribute>

					<xsl:attribute name="CodeContext">IATACode</xsl:attribute>
					<xsl:value-of select="concat(../securityInformation/secondRpInformation/creationOfficeId, '/', ../securityInformation/secondRpInformation/agentSignature)"/>
				</CompanyName>
			</xsl:if>
		</ItineraryRef>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Pricing Response     	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="Ticket_DisplayTSTReply">
		<AirFareInfo>


			<xsl:variable name="OptP">
				<xsl:for-each select="../Ticket_GetPricingOptionsReply/documentInformation[1]/pricingOptionsGroup/pricingOptionKey/pricingOptionKey">
					<xsl:if test="position() > 1">
						<xsl:text>,</xsl:text>
					</xsl:if>
					<xsl:value-of select="text()"/>
				</xsl:for-each>
			</xsl:variable>


			<xsl:attribute name="PricingSource">
				<!--xsl:choose>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'H'">Private</xsl:when>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'F'">Private</xsl:when>
				<xsl:when test="fareList[1]/fareDataInformation/fareDataMainInformation/fareDataQualifier = 'BT'">Private</xsl:when>
				<xsl:otherwise>Published</xsl:otherwise>
			</xsl:choose-->
				<xsl:choose>
					<xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'F'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'I'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'M'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'N'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'R'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = '7'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = '9'">Private</xsl:when>
					<xsl:when test="fareList[1]/pricingInformation/fcmi = 'G'">Private</xsl:when>

					<xsl:when test="contains($OptP,'RP')">Published</xsl:when>
					<xsl:when test="contains($OptP,'RU')">Private</xsl:when>
					<xsl:when test="contains($OptP,'RC')">Private</xsl:when>
					<xsl:when test="contains($OptP,'RN')">Private</xsl:when>
					<xsl:when test="contains($OptP,'RW')">Private</xsl:when>

					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="FCMI">
				<xsl:value-of select="concat(fareList[1]/pricingInformation/tstInformation/tstIndicator,fareList[1]/pricingInformation/fcmi)"/>
			</xsl:attribute>
			<xsl:variable name="bf">
				<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalbase">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="tf">
				<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalprice">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="Taxf">
				<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalTax">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="Feef">
				<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalFee">
					<xsl:with-param name="sum">0</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">1</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="curt">
				<xsl:choose>
					<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
						<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="dect">
				<xsl:choose>
					<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
						<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<ItinTotalFare>
				<BaseFare>
					<!--xsl:attribute name="Amount">
					<xsl:variable name="base">
						<xsl:value-of select="format-number(sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount),'.00')"/>
					</xsl:variable>
					<xsl:value-of select="translate(string($base),'.','')"/>
				</xsl:attribute-->
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="$bf != ''">
								<xsl:value-of select="$bf"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
								<xsl:choose>
									<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
										<xsl:value-of select="$curt"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$curt"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
								<xsl:choose>
									<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
										<xsl:value-of select="$dect"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$dect"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</BaseFare>
				<!-- and $userid!='morqua' -->
				<xsl:if test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E'] ">
					<xsl:variable name="ef">
						<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalequiv">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="loop">
								<xsl:value-of select="$loop"/>
							</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:variable>
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$ef != ''">
									<xsl:value-of select="$ef"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
						</xsl:attribute>
					</EquivFare>
				</xsl:if>
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
					<!--<Tax>
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
          </Tax>-->
				</Taxes>
				<xsl:if test="fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'TOF']">
					<Fees>
						<Fee>
							<xsl:attribute name="Amount">
								<xsl:choose>
									<xsl:when test="$Feef!= ''">
										<xsl:value-of select="$Feef"/>
									</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="$curt"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Fee>
					</Fees>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="$tf != ''">
								<xsl:value-of select="$tf"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$curt"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dect"/>
					</xsl:attribute>
					<!--
			<xsl:attribute name="Amount">
				<xsl:variable name="tot">
					<xsl:value-of select="format-number(sum(fareList/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount),'.00')"/>
					</xsl:variable>
					<xsl:value-of select="translate(string($tot),'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			-->
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="fareList"/>
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</xsl:template>

	<xsl:template match="fareList">
		<PTC_FareBreakdown>
			<xsl:variable name="tqtNum">
				<xsl:value-of select="fareReference/uniqueReference"/>
			</xsl:variable>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$tqtNum"/>
			</xsl:attribute>
			<xsl:attribute name="FCMI">
				<xsl:value-of select="concat(pricingInformation/tstInformation/tstIndicator,pricingInformation/fcmi)"/>
			</xsl:attribute>

			<xsl:variable name="priceOpt">
				<xsl:for-each select="../../Ticket_GetPricingOptionsReply/documentInformation[documentSelection/uniqueReference = $tqtNum]/pricingOptionsGroup">
					<xsl:if test="position() > 1">
						<xsl:text>,</xsl:text>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="optionDetail/criteriaDetails/attributeType!=''">
							<xsl:value-of select="concat(pricingOptionKey/pricingOptionKey, '*', optionDetail/criteriaDetails/attributeType)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="pricingOptionKey/pricingOptionKey"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>

			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
					<xsl:when test="pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when>
					<xsl:when test="pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'F'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'I'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'M'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'N'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'R'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = '7'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = '9'">Private</xsl:when>
					<xsl:when test="pricingInformation/fcmi = 'G'">Private</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="pricingInformation/fcmi = '5'">
								<xsl:variable name="adttype">
									<xsl:variable name="adultref">
										<xsl:value-of select="../../travellerInfo[passengerData[1]/travellerInformation/passenger/type='ADT' or not(passengerData[1]/travellerInformation/passenger/type)]/elementManagementPassenger/reference/number"/>
									</xsl:variable>
									<xsl:variable name="adttst">
										<xsl:value-of select="../fareList[paxSegReference/refDetails[refNumber=$adultref]]/pricingInformation/tstInformation/tstIndicator "/>
									</xsl:variable>
									<xsl:variable name="adtfcmi">
										<xsl:value-of select="../fareList[paxSegReference/refDetails[refNumber=$adultref]]/pricingInformation/fcmi"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$adttst = 'B'">Private</xsl:when>
										<xsl:when test="$adttst = 'F'">Private</xsl:when>
										<xsl:when test="$adttst = 'G'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'F'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'I'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'M'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'N'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'R'">Private</xsl:when>
										<xsl:when test="$adtfcmi = '7'">Private</xsl:when>
										<xsl:when test="$adtfcmi = '9'">Private</xsl:when>
										<xsl:when test="$adtfcmi = 'G'">Private</xsl:when>

										<xsl:when test="contains($priceOpt,'RP')">Published</xsl:when>
										<xsl:when test="contains($priceOpt,'RU')">Private</xsl:when>
										<xsl:when test="contains($priceOpt,'RC')">Private</xsl:when>
										<xsl:when test="contains($priceOpt,'RN')">Private</xsl:when>
										<xsl:when test="contains($priceOpt,'RW')">Private</xsl:when>

										<xsl:otherwise>Published</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="$adttype"/>
							</xsl:when>
							<!-- -->
							<xsl:when test="contains($priceOpt,'RP')">Published</xsl:when>
							<xsl:when test="contains($priceOpt,'RU')">Private</xsl:when>
							<xsl:when test="contains($priceOpt,'RC')">Private</xsl:when>
							<xsl:when test="contains($priceOpt,'RN')">Private</xsl:when>
							<xsl:when test="contains($priceOpt,'RW')">Private</xsl:when>

							<xsl:otherwise>Published</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>

			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:for-each select="paxSegReference/refDetails">
					<xsl:variable name="paxref">
						<xsl:value-of select="refNumber"/>
					</xsl:variable>
					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:value-of select="../../../../travellerInfo[elementManagementPassenger/reference/number = $paxref]/elementManagementPassenger/lineNumber"/>
				</xsl:for-each>
			</xsl:attribute>
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]">
					<xsl:variable name="segref">
						<xsl:value-of select="segmentReference/refDetails/refNumber"/>
					</xsl:variable>
					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:value-of select="../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
				</xsl:for-each>
			</xsl:attribute>
			<xsl:variable name="paxref">
				<xsl:value-of select="paxSegReference/refDetails[1]/refNumber"/>
			</xsl:variable>
			<xsl:variable name="paxtype">
				<xsl:choose>
					<xsl:when test="statusInformation/firstStatusDetails[1]/tstFlag='INF'">INF</xsl:when>
					<xsl:when test="count(../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData)>1">
						<xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData[1]/travellerInformation/passenger/type"/>
					</xsl:when>
					<xsl:when test="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type=''">ADT</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger[1]/type"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="paxcode">
				<xsl:choose>
					<xsl:when test="$paxtype = ''">ADT</xsl:when>
					<xsl:when test="$paxtype = 'CH'">CHD</xsl:when>
					<xsl:when test="$paxtype = 'INN'">CHD</xsl:when>
					<xsl:when test="$paxtype = 'CNN'">CHD</xsl:when>
					<xsl:when test="$paxtype = 'YCD'">SRC</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$paxtype"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="$paxcode"/>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="count(paxSegReference/refDetails)"/>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<xsl:if test="segmentInformation/fareQualifier/fareBasisDetails">
				<FareBasisCodes>
					<!--xsl:apply-templates select="segmentInformation[connexInformation/connecDetails/routingInformation != 'ARNK']"/-->
					<xsl:apply-templates select="segmentInformation"/>
				</FareBasisCodes>
			</xsl:if>
			<xsl:if test="../../pricingRecordGroup/productPricingQuotationRecord/documentDetailsGroup/fareComponentDetailsGroup/fareFamilyDetails/fareFamilyname">
				<BrandedFares>
					<xsl:apply-templates select="../../pricingRecordGroup/productPricingQuotationRecord[pricingRecordId/uniqueReference = $tqtNum]/documentDetailsGroup/fareComponentDetailsGroup" mode="FareFamily" />
				</BrandedFares>
			</xsl:if>
			<xsl:variable name="nip">
				<xsl:value-of select="1"/>
				<!--<xsl:value-of select="count(paxSegReference/refDetails)"/>-->
			</xsl:variable>
			<PassengerFare>
				<xsl:variable name="bfpax">
					<xsl:choose>
						<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
							<xsl:choose>
								<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
									<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nip"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="tfpax">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nip"/>
				</xsl:variable>
				<xsl:variable name="feefpax">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'TOF']/fareAmount,'.','') * $nip"/>
				</xsl:variable>
				<xsl:variable name="cur">
					<xsl:choose>
						<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
							<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="dec">
					<xsl:choose>
						<xsl:when test="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') > 0">
							<xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>
				<BaseFare>
					<!--xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount,'.','')"/>
				</xsl:attribute-->
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="$bfpax != 'NaN'">
								<xsl:value-of select="$bfpax"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
								<xsl:choose>
									<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
										<xsl:value-of select="$cur"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$cur"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
								<xsl:choose>
									<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
										<xsl:value-of select="$dec"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$dec"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</BaseFare>
				<!--and $userid!='morqua'-->
				<xsl:if test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
					<xsl:variable name="efpax">
						<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip"/>
					</xsl:variable>
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$efpax != 'NaN'">
									<xsl:value-of select="$efpax"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
						</xsl:attribute>
					</EquivFare>
				</xsl:if>
				<Taxes>
					<xsl:apply-templates select="taxInformation">
						<xsl:with-param name="nip">
							<xsl:value-of select="$nip"/>
						</xsl:with-param>
						<xsl:with-param name="dec">
							<xsl:value-of select="$dec"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</Taxes>
				<xsl:if test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'TOF']">
					<Fees>
						<Fee>
							<xsl:attribute name="Amount">
								<xsl:choose>
									<xsl:when test="$feefpax!= 'NaN'">
										<xsl:value-of select="$feefpax"/>
									</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="$cur"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dec"/>
							</xsl:attribute>
						</Fee>
					</Fees>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="$tfpax != 'NaN'">
								<xsl:value-of select="$tfpax"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
					<!--xsl:attribute name="Amount">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount,'.','')"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareCurrency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute-->
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<xsl:if test="otherPricingInfo/attributeDetails[attributeType='FCA']">
					<FareCalculation>
						<xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription"/>
					</FareCalculation>
				</xsl:if>
				<xsl:if test="bankerRates/firstRateDetail">
					<BSR>
						<xsl:value-of select="bankerRates/firstRateDetail/amount"/>
					</BSR>
				</xsl:if>
				<xsl:if test="otherPricingInfo/attributeDetails[attributeType='PAY']">
					<PaymentRestrictions>
						<xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='PAY']/attributeDescription"/>
					</PaymentRestrictions>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="validatingCarrier/carrierInformation/carrierCode != ''">
						<ValidatingAirlineCode>
							<xsl:value-of select="validatingCarrier/carrierInformation/carrierCode"/>
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

				<!--
        <xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]">
          <xsl:if test="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity or bagAllowanceInformation/bagAllowanceDetails/baggageWeight">
            <BagAllowance>
              <xsl:choose>
                <xsl:when test="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity!=''">
                  <xsl:attribute name="Quantity">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/baggageQuantity"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Piece</xsl:text>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Weight">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/baggageWeight"/>
                  </xsl:attribute>
                  <xsl:attribute name="Type">
                    <xsl:text>Weight</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="Unit">
                    <xsl:value-of select="bagAllowanceInformation/bagAllowanceDetails/measureUnit"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:attribute name="FlightRefNumberRPHList">
                <xsl:variable name="segref">
                  <xsl:value-of select="segmentReference/refDetails/refNumber"/>
                </xsl:variable>
                <xsl:value-of select="../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
              </xsl:attribute>
            </BagAllowance>
          </xsl:if>
        </xsl:for-each>
        -->

				<xsl:if test="segmentInformation[not(connexInformation/connecDetails/routingInformation)][1]/bagAllowanceInformation/bagAllowanceDetails/baggageQuantity or bagAllowanceInformation/bagAllowanceDetails/baggageWeight">
					<BagAllowance>
						<xsl:choose>
							<xsl:when test="segmentInformation[not(connexInformation/connecDetails/routingInformation)][1]/bagAllowanceInformation/bagAllowanceDetails/baggageQuantity!=''">
								<xsl:attribute name="Quantity">
									<xsl:value-of select="segmentInformation[not(connexInformation/connecDetails/routingInformation)][1]/bagAllowanceInformation/bagAllowanceDetails/baggageQuantity"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>Piece</xsl:text>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Weight">
									<xsl:value-of select="segmentInformation[not(connexInformation/connecDetails/routingInformation)][1]/bagAllowanceInformation/bagAllowanceDetails/baggageWeight"/>
								</xsl:attribute>
								<xsl:attribute name="Type">
									<xsl:text>Weight</xsl:text>
								</xsl:attribute>
								<xsl:attribute name="Unit">
									<xsl:value-of select="segmentInformation[not(connexInformation/connecDetails/routingInformation)][1]/bagAllowanceInformation/bagAllowanceDetails/measureUnit"/>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<!--
            <xsl:attribute name="ItinSeqNumber">
              <xsl:for-each select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]">
                <xsl:variable name="segref">
                  <xsl:value-of select="segmentReference/refDetails/refNumber"/>
                </xsl:variable>                
                <xsl:if test="position() > 1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
              </xsl:for-each>
            </xsl:attribute>
            -->
					</BagAllowance>
				</xsl:if>

				<xsl:if test="../../OTA_AirRulesRS">
					<FareRules>
						<xsl:for-each select="../../OTA_AirRulesRS/FareRuleResponseInfo[FareRuleInfo/@PassengerType=$paxcode][1]/FareRules/SubSection[@SubCode='PE']/Paragraph">
							<Text>
								<xsl:value-of select="Text"/>
							</Text>
						</xsl:for-each>
					</FareRules>
				</xsl:if>

				<xsl:if test="$priceOpt != ''">
					<SupplementalInfo>
						<xsl:value-of select="$priceOpt"/>
					</SupplementalInfo>
				</xsl:if>

			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>

	<xsl:template match="fareComponentDetailsGroup" mode="FareFamily">
		<xsl:for-each select="couponDetailsGroup">
			<xsl:if test="../fareFamilyDetails/fareFamilyname">
				<FareFamily>
					<xsl:variable name="segref">
						<xsl:value-of select="productId/referenceDetails/value"/>
					</xsl:variable>
					<xsl:attribute name="RPH">
						<xsl:value-of select="../../../../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
						<!--<xsl:value-of select="productId/referenceDetails/value"/>-->
					</xsl:attribute>
					<xsl:variable name="tstref">
						<xsl:value-of select="../../../pricingRecordId/uniqueReference"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="../fareFamilyDetails/fareFamilyname">
							<xsl:value-of select="../fareFamilyDetails/fareFamilyname"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="../../../../../Ticket_GetPricingOptionsReply[documentInformation/documentSelection/uniqueReference=$tstref]/documentInformation[pricingOptionsGroup/paxSegTstReference/referenceDetails/value=$segref]/pricingOptionsGroup[pricingOptionKey/pricingOptionKey='PFF' and paxSegTstReference/referenceDetails/value=$segref]/optionDetail/criteriaDetails/attributeDescription"/>
						</xsl:otherwise>
					</xsl:choose>
				</FareFamily>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="segmentInformation">
		<xsl:variable name="fcl" select="../otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription" />
		
		<xsl:choose>
			<xsl:when test="(not(connexInformation/connecDetails/routingInformation) or connexInformation/connecDetails/routingInformation != 'ARNK') and fareQualifier/fareBasisDetails">
				<xsl:variable name="fb">
					<xsl:choose>
						<xsl:when test="string-length(concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode)) = 9 
						and substring(fareQualifier/fareBasisDetails/fareBasisCode, string-length(fareQualifier/fareBasisDetails/fareBasisCode), 1) = 'C' 
					    and contains($fcl, concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode))">
							<xsl:value-of select="concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode,'H')"/>
						</xsl:when>
						<xsl:when test="string-length(concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode)) = 9 
						and substring(fareQualifier/fareBasisDetails/fareBasisCode, string-length(fareQualifier/fareBasisDetails/fareBasisCode), 1) = 'I' 
					    and contains($fcl, concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode))">
							<xsl:value-of select="concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode,'N')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat(fareQualifier/fareBasisDetails/primaryCode,fareQualifier/fareBasisDetails/fareBasisCode)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				<FareBasisCode>
					<xsl:choose>
						<xsl:when test="fareQualifier/fareBasisDetails/ticketDesignator">
							<xsl:value-of select="concat($fb,'/',fareQualifier/fareBasisDetails/ticketDesignator)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$fb"/>
						</xsl:otherwise>
					</xsl:choose>
				</FareBasisCode>
				
			</xsl:when>
			<xsl:otherwise>
				<FareBasisCode>VOID</FareBasisCode>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="taxInformation">
		<xsl:param name="nip"/>
		<xsl:param name="dec"/>
		<Tax>
			<xsl:attribute name="Code">
				<xsl:value-of select="taxDetails/taxType/isoCountry"/>
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(amountDetails/fareDataMainInformation/fareAmount,'.','') * $nip"/>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$dec"/>
			</xsl:attribute>
		</Tax>
	</xsl:template>

	<!-- 
  **************************************************************
   Process Names			                            
  ************************************************************** 
  -->

	<xsl:template match="passengerData">
		<xsl:variable name="paxNum" select="../elementManagementPassenger/lineNumber" />
		<xsl:variable name="paxref" select="../elementManagementPassenger/reference/number"/>
		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$paxNum"/>
			</xsl:attribute>
			<Customer>
				<xsl:choose>
					<xsl:when test="dateOfBirth/dateAndTimeDetails[qualifier='706']">
						<xsl:attribute name="BirthDate">
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,5,4)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,1,2)"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR' and referenceForDataElement/reference[qualifier='PT']/number = $paxNum]">

						<xsl:call-template name="birthDate">
							<xsl:with-param name="lookupName" select="concat(enhancedTravellerInformation/otherPaxNamesDetails/surname,'/',enhancedTravellerInformation/otherPaxNamesDetails/givenName)" />
							<xsl:with-param name="paxref" select="$paxref" />
						</xsl:call-template>

					</xsl:when>
				</xsl:choose>
				<PersonName>
					<xsl:attribute name="NameType">
						<xsl:choose>
							<xsl:when test="not(travellerInformation/passenger[1]/type)">ADT</xsl:when>
							<xsl:when test="travellerInformation/passenger[1]/type = 'CH'">CHD</xsl:when>
							<xsl:when test="travellerInformation/passenger[1]/type = 'CNN'">CHD</xsl:when>
							<xsl:when test="travellerInformation/passenger[1]/type = 'YCD'">SRC</xsl:when>
							<xsl:when test="travellerInformation/passenger[1]/infantIndicator = 1">ADT</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="travellerInformation/passenger/type"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<GivenName>
						<xsl:choose>
							<xsl:when test="../elementManagementPassenger/lineNumber='0'">GROUPPNR</xsl:when>
							<xsl:when test="not(travellerInformation/passenger/firstName)">NONAME</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="travellerInformation/passenger/firstName"/>
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

				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="phone"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="email"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB/']" mode="Address"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB']" mode="Address"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM/']" mode="Address"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM']" mode="Address"/>
				<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][serviceRequest/ssr/status='HK']" mode="Fqtv">
					<xsl:with-param name="paxref">
						<xsl:value-of select="$paxref"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</Customer>
		</CustomerInfo>

		<xsl:if test="travellerInformation/passenger[1]/infantIndicator = 1 and travellerInformation/passenger[position()=2]/type = 'INF'">
			<CustomerInfo>
				<xsl:attribute name="RPH">
					<xsl:value-of select="$paxNum"/>
				</xsl:attribute>
				<Customer>
					<xsl:choose>
						<xsl:when test="dateOfBirth/dateAndTimeDetails[qualifier='706']">
							<xsl:attribute name="BirthDate">
								<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,5,4)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,3,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(dateOfBirth/dateAndTimeDetails/date,1,2)"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR' and serviceRequest/ssr/type='DOCS' and referenceForDataElement/reference[qualifier='PT']/number = $paxref]">

							<xsl:call-template name="birthDate">
								<xsl:with-param name="lookupName" select="concat(enhancedTravellerInformation/otherPaxNamesDetails/surname,'/',enhancedTravellerInformation/otherPaxNamesDetails/givenName)" />
								<xsl:with-param name="paxref" select="$paxref" />
							</xsl:call-template>

						</xsl:when>
					</xsl:choose>
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

	<xsl:template match="enhancedPassengerData">
		<xsl:variable name="paxNum" select="../elementManagementPassenger/lineNumber" />
		<xsl:variable name="paxref" select="../elementManagementPassenger/reference/number"/>

		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$paxNum"/>
			</xsl:attribute>

			<xsl:variable name="ptc">
				<xsl:choose>
					<xsl:when test="not(enhancedTravellerInformation/travellerNameInfo/type)">ADT</xsl:when>
					<xsl:when test="enhancedTravellerInformation/travellerNameInfo/type = 'CH'">CHD</xsl:when>
					<xsl:when test="enhancedTravellerInformation/travellerNameInfo/type = 'CNN'">CHD</xsl:when>
					<xsl:when test="enhancedTravellerInformation/travellerNameInfo/type = 'YCD'">SRC</xsl:when>
					<xsl:when test="enhancedTravellerInformation/travellerNameInfo/infantIndicator = 1">ADT</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="enhancedTravellerInformation/travellerNameInfo/type"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<Customer>
				<xsl:choose>
					<xsl:when test="dateOfBirthInEnhancedPaxData/dateAndTimeDetails[qualifier='706']">
						<xsl:attribute name="BirthDate">
							<xsl:value-of select="substring(dateOfBirthInEnhancedPaxData/dateAndTimeDetails/date,5,4)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirthInEnhancedPaxData/dateAndTimeDetails/date,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(dateOfBirthInEnhancedPaxData/dateAndTimeDetails/date,1,2)"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR' and serviceRequest/ssr/type='DOCS' and referenceForDataElement/reference[qualifier='PT']/number = $paxref]">

						<xsl:call-template name="birthDate">
							<xsl:with-param name="lookupName" select="concat(enhancedTravellerInformation/otherPaxNamesDetails/surname,'/',enhancedTravellerInformation/otherPaxNamesDetails/givenName)" />
							<xsl:with-param name="paxref" select="$paxref" />
						</xsl:call-template>

					</xsl:when>
				</xsl:choose>

				<PersonName>
					<xsl:attribute name="NameType">
						<xsl:value-of select="$ptc"/>
					</xsl:attribute>

					<GivenName>
						<xsl:choose>
							<xsl:when test="../elementManagementPassenger/lineNumber='0'">GROUPPNR</xsl:when>
							<xsl:when test="not(enhancedTravellerInformation/otherPaxNamesDetails/givenName)">NONAME</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="enhancedTravellerInformation/otherPaxNamesDetails/givenName"/>
							</xsl:otherwise>
						</xsl:choose>
					</GivenName>
					<Surname>
						<xsl:value-of select="enhancedTravellerInformation/otherPaxNamesDetails/surname"/>
					</Surname>
				</PersonName>
				<xsl:if test="$ptc != 'INF'">
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="phone"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AP']" mode="email"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB/']" mode="Address"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AB']" mode="Address"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM/']" mode="Address"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AM']" mode="Address"/>
					<xsl:apply-templates select="../../dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][serviceRequest/ssr/status='HK']" mode="Fqtv">
						<xsl:with-param name="paxref">
							<xsl:value-of select="$paxref"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</xsl:if>
			</Customer>
		</CustomerInfo>

		<!--
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
		-->
	</xsl:template>

	<xsl:template name="birthDate">
		<xsl:param name="lookupName" />
		<xsl:param name="paxref" />

		<xsl:variable name="ssr" select="../../dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SSR' and serviceRequest/ssr/type='DOCS' and referenceForDataElement/reference[qualifier='PT']/number = $paxref and contains(serviceRequest/ssr/freeText, $lookupName)]/serviceRequest/ssr/freeText" />

		<xsl:variable name="elems">
			<xsl:call-template name="tokenizeString">
				<xsl:with-param name="list" select="$ssr"/>
				<xsl:with-param name="delimiter" select="'/'"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="dob">
			<xsl:for-each select="msxsl:node-set($elems)/elem/node()[1]">
									
				<xsl:if test="string-length(.) = 7">
					<xsl:variable name="nNum">
						<xsl:call-template name="month">
							<xsl:with-param name="month">
								<xsl:value-of select="substring(.,3,3)"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					
					<xsl:if test="$nNum != ''">
						<xsl:value-of select="."/>
					</xsl:if>
					
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>

		<xsl:if test="string-length($dob) > 0">
			<xsl:attribute name="BirthDate">
				<xsl:call-template name="year">
					<xsl:with-param name="year">
						<xsl:value-of select="substring($dob,6,2)"/>
					</xsl:with-param>
				</xsl:call-template>

				<xsl:text>-</xsl:text>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring($dob,3,3)"/>
					</xsl:with-param>
				</xsl:call-template>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring($dob,1,2)"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>


	<!-- ****************************************************************************************************************** -->
	<!-- Process Itinerary				 							                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Air Segments    				                    -->
	<!-- ************************************************************** -->
	<xsl:template match="itineraryInfo" mode="Air">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="tstRef">
			<xsl:value-of select="$pos+1"/>
		</xsl:variable>

		<xsl:variable name="zeros">000</xsl:variable>
		<xsl:choose>
			<!--<xsl:when test="../../Ticket_DisplayTSTReply and (flightDetail/productDetails/equipment='TRN' or flightDetail/productDetails/equipment='CCR' or flightDetail/productDetails/equipment='CU' or flightDetail/productDetails/equipment='AU' or flightDetail/productDetails/equipment='SUR' or flightDetail/productDetails/equipment='CRU' or flightDetail/productDetails/equipment='TU') and $pos > 1">-->
			<xsl:when test="../../Ticket_DisplayTSTReply and (flightDetail/productDetails/equipment='CU' or flightDetail/productDetails/equipment='AU' or flightDetail/productDetails/equipment='TU') and ../../Ticket_DisplayTSTReply/fareList[1]/segmentInformation[sequenceInformation/sequenceSection/sequenceNumber=$tstRef]/connexInformation/connecDetails/routingInformation='ARNK'">
				<!--<xsl:if test="../../Ticket_DisplayTSTReply/fareList[1]/segmentInformation[sequenceInformation/sequenceSection/sequenceNumber=$tstRef]/connexInformation/connecDetails/routingInformation='ARNK'">-->
				<Item Status="">
					<xsl:attribute name="ItinSeqNumber">
						<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
					</xsl:attribute>
					<!--ItinSeqNumber="0">-->
					<TPA_Extensions>
						<Arnk/>
					</TPA_Extensions>
				</Item>
				<!--</xsl:if>-->
			</xsl:when>
			<xsl:otherwise>
				<Item>
					<xsl:attribute name="Status">
						<xsl:value-of select="relatedProduct/status"/>
					</xsl:attribute>
					<xsl:attribute name="ItinSeqNumber">
						<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/lineNumber"/>
					</xsl:attribute>
					<xsl:if test="relatedProduct/status='GK'">
						<xsl:attribute name="IsPassive">true</xsl:attribute>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="travelProduct/productDetails/identification='ARNK'">
							<TPA_Extensions>
								<Arnk/>
							</TPA_Extensions>
						</xsl:when>
						<xsl:otherwise>
							<Air>
								<!--************************************************************************************-->
								<!--			Air Segments/Open Segments  						      -->
								<!--************************************************************************************-->
								<xsl:variable name="zeroes">0000</xsl:variable>
								<xsl:if test="travelProduct/product/depDate!=''">
									<xsl:attribute name="DepartureDateTime">
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
								</xsl:if>
								<xsl:if test="travelProduct/product/arrDate!=''">
									<xsl:attribute name="ArrivalDateTime">
										<xsl:text>20</xsl:text>
										<xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/>
										<xsl:text>T</xsl:text>
										<xsl:choose>
											<xsl:when test="not(travelProduct/product/depTime)">00:00:00</xsl:when>
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
								<xsl:if test="flightDetail/productDetails/numOfStops != ''">
									<xsl:attribute name="StopQuantity">
										<xsl:value-of select="flightDetail/productDetails/numOfStops"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="RPH">
									<xsl:value-of select="position()"/>
								</xsl:attribute>
								<xsl:attribute name="FlightNumber">
									<xsl:choose>
										<xsl:when test="travelProduct/productDetails/identification='OPEN'">OPEN</xsl:when>
										<xsl:otherwise>
											<xsl:number value="travelProduct/productDetails/identification" format="0001"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="ResBookDesigCode">
									<xsl:value-of select="travelProduct/productDetails/classOfService"/>
								</xsl:attribute>
								<xsl:if test="relatedProduct/quantity!=''">
									<xsl:attribute name="NumberInParty">
										<xsl:value-of select="relatedProduct/quantity"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="Status">
									<xsl:value-of select="relatedProduct/status"/>
								</xsl:attribute>
								<xsl:if test="travelProduct/typeDetail/detail != ''">
									<xsl:attribute name="E_TicketEligibility">
										<xsl:choose>
											<xsl:when test="travelProduct/typeDetail/detail = 'ET'">Eligible</xsl:when>
											<xsl:otherwise>NotEligible</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:if>
								<xsl:if test="flightDetail/productDetails/weekDay != ''">
									<xsl:attribute name="DepartureDay">
										<xsl:choose>
											<xsl:when test="flightDetail/productDetails/weekDay = '1'">Mon</xsl:when>
											<xsl:when test="flightDetail/productDetails/weekDay = '2'">Tue</xsl:when>
											<xsl:when test="flightDetail/productDetails/weekDay = '3'">Wed</xsl:when>
											<xsl:when test="flightDetail/productDetails/weekDay = '4'">Thu</xsl:when>
											<xsl:when test="flightDetail/productDetails/weekDay = '5'">Fri</xsl:when>
											<xsl:when test="flightDetail/productDetails/weekDay = '6'">Sat</xsl:when>
											<xsl:otherwise>Sun</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:if>
								<xsl:if test="../../Ticket_DisplayTSTReply">
									<xsl:variable name="segref">
										<xsl:value-of select="elementManagementItinerary[reference/qualifier='ST']/reference/number"/>
									</xsl:variable>
									<xsl:variable name="segcnx">
										<xsl:value-of select="../../Ticket_DisplayTSTReply/fareList[1]/segmentInformation[segmentReference/refDetails/refNumber=$segref]/connexInformation/connecDetails/connexType"/>
									</xsl:variable>
									<xsl:attribute name="OrginDestType">
										<xsl:choose>
											<xsl:when test="$segcnx='O'">First</xsl:when>
											<xsl:otherwise>Intermediate</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:if>
								<xsl:if test="../../Ticket_ProcessETicketReply">
									<xsl:variable name="seg" select="elementManagementItinerary/reference/number"/>
									<xsl:variable name="cs">
										<xsl:value-of select="../../Ticket_ProcessETicketReply/documentGroup/ticketInfoGroup/couponInfoGroup[flightInfo/itemNumber=$seg]/couponInfo/couponDetails/cpnStatus"/>
									</xsl:variable>
									<xsl:attribute name="CouponStatus">
										<xsl:choose>
											<xsl:when test="$cs='AL'">
												<xsl:value-of select="'AirportControl'"/>
											</xsl:when>
											<xsl:when test="$cs='B'">
												<xsl:value-of select="'FlownUsed'"/>
											</xsl:when>
											<xsl:when test="$cs='BD'">
												<xsl:value-of select="'Boarded'"/>
											</xsl:when>
											<xsl:when test="$cs='CK'">
												<xsl:value-of select="'CheckedIn'"/>
											</xsl:when>
											<xsl:when test="$cs='E'">
												<xsl:value-of select="'ExchangedReissued'"/>
											</xsl:when>
											<xsl:when test="$cs='I'">
												<xsl:value-of select="'OriginalIssue'"/>
											</xsl:when>
											<xsl:when test="$cs='IO'">
												<xsl:value-of select="'IrregularOperations'"/>
											</xsl:when>
											<xsl:when test="$cs='OK'">
												<xsl:value-of select="'Confirmed'"/>
											</xsl:when>
											<xsl:when test="$cs='PE'">
												<xsl:value-of select="'PrintExchange'"/>
											</xsl:when>
											<xsl:when test="$cs='PR'">
												<xsl:value-of select="'Printed'"/>
											</xsl:when>
											<xsl:when test="$cs='RF'">
												<xsl:value-of select="'Refunded'"/>
											</xsl:when>
											<xsl:when test="$cs='RQ'">
												<xsl:value-of select="'Requested'"/>
											</xsl:when>
											<xsl:when test="$cs='V'">
												<xsl:value-of select="'Void'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="'NotAvailable'"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:if>
								<DepartureAirport>
									<xsl:attribute name="LocationCode">
										<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
									</xsl:attribute>
									<xsl:if test="flightDetail/departureInformation/departTerminal != ''">
										<xsl:attribute name="Terminal">
											<xsl:value-of select="flightDetail/departureInformation/departTerminal"/>
										</xsl:attribute>
									</xsl:if>
								</DepartureAirport>
								<ArrivalAirport>
									<xsl:attribute name="LocationCode">
										<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
									</xsl:attribute>
								</ArrivalAirport>
								<OperatingAirline>
									<xsl:variable name="citypair">
										<xsl:value-of select="concat(travelProduct/boardpointDetail/cityCode,travelProduct/offpointDetail/cityCode,' OPERATED BY ')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="../../RTSVI/Line[contains(.,$citypair)]">
											<xsl:variable name="oa">
												<xsl:value-of select="substring-before(substring-after(../../RTSVI/Line[contains(.,$citypair)],$citypair), ' FOR ')"/>
											</xsl:variable>
											<xsl:choose>
												<xsl:when test="contains($oa,'  ')">
													<xsl:variable name="aircode">
														<xsl:choose>
															<xsl:when test="string-length(translate(substring-after($oa,'  '),' 0123456789',''))=1">

																<xsl:variable name="alcode">
																	<!--                   B2  857-->
																	<xsl:call-template name="string-trim">
																		<xsl:with-param name="string" select="substring-after($oa,'  ')" />
																	</xsl:call-template>
																</xsl:variable>

																<xsl:value-of select="substring-before($alcode,' ')"/>
															</xsl:when>
															<xsl:when test="string-length(translate(substring-after($oa,'  '),' 0123456789',''))=0">
																<xsl:value-of select="substring(../../RTSVI/Line[contains(.,$citypair)],46,2)"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="translate(substring-after($oa,'  '),' 0123456789','')"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:variable>

													<xsl:attribute name="Code">
														<xsl:choose>
															<xsl:when test="$aircode!=''">
																<xsl:value-of select="$aircode"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="substring-before($oa,'  ')"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:attribute>

													<xsl:value-of select="substring-before($oa,'  ')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:variable name="aircode">
														<xsl:value-of select="substring(../../RTSVI/Line[contains(.,$citypair)],46,2)"/>
													</xsl:variable>

													<xsl:if test="$aircode!=''">
														<xsl:attribute name="Code">
															<xsl:value-of select="$aircode"/>
														</xsl:attribute>
													</xsl:if>

													<xsl:value-of select="$oa"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="CAPI_PNR_AirSegment/Airline2 != ''">
											<xsl:attribute name="Code">
												<xsl:value-of select="CAPI_PNR_AirSegment/Airline2"/>
											</xsl:attribute>
											<AirlineName/>
										</xsl:when>
										<xsl:when test="itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text !=''">
											<xsl:choose>
												<xsl:when test="contains(itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text,'OPERATED BY')">
													<xsl:choose>
														<xsl:when test="substring-after(itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text,' BY ') = 'SUBSIDIARY/FRANCHISE'">
															<xsl:variable name="fn">
																<xsl:value-of select="travelProduct/productDetails/identification"/>
															</xsl:variable>
															<xsl:variable name="fn1">
																<xsl:value-of select="substring-after(substring-after(../../Command_CrypticReply/longTextString/textStringDetails,$fn),'OPERATED BY ')"/>
															</xsl:variable>
															<xsl:variable name="fn2">
																<xsl:value-of select="translate(substring-before($fn1,'&#13;'),' ','')"/>
															</xsl:variable>
															<xsl:variable name="fn3">
																<xsl:choose>
																	<xsl:when test="$fn2!=''">
																		<xsl:value-of select="substring-before($fn1,'&#13;')"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="substring-before(substring-after($fn1,'&#13;'),'&#13;')"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:variable>
															<xsl:value-of select="normalize-space($fn3)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:variable name="carrier">
																<xsl:value-of select="substring-after(itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text,' BY ')"/>
															</xsl:variable>

															<xsl:if test="../../RTSVI/Line[contains(.,$carrier)]">
																<xsl:variable name="aircode">
																	<xsl:value-of select="substring(../../RTSVI/Line[contains(.,$carrier)],46,2)"/>
																</xsl:variable>

																<xsl:if test="$aircode!=''">
																	<xsl:attribute name="Code">
																		<xsl:value-of select="$aircode"/>
																	</xsl:attribute>
																</xsl:if>
															</xsl:if>

															<xsl:value-of select="$carrier"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="itineraryfreeFormText[freetextDetail/subjectQualifier='3']/text"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(itineraryfreeFormText/freeText, 'OPERATED BY')">
											<xsl:variable name="opBy">
												<!--     OPERATED BY BRUSSELS AIRLINES-->
												<xsl:call-template name="string-trim">
													<xsl:with-param name="string" select="itineraryfreeFormText/freeText" />
												</xsl:call-template>
											</xsl:variable>

											<xsl:variable name="carrier">
												<xsl:value-of select="substring-after($opBy,'BY ')"/>
											</xsl:variable>

											<xsl:if test="../../RTSVI/Line[contains(.,$opBy)]">
												<xsl:variable name="aircode">
													<xsl:value-of select="substring(../../RTSVI/Line[contains(.,$opBy)],46,2)"/>
												</xsl:variable>

												<xsl:if test="$aircode!=''">
													<xsl:attribute name="Code">
														<xsl:value-of select="$aircode"/>
													</xsl:attribute>
												</xsl:if>
											</xsl:if>
											<xsl:value-of select="$carrier"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Code">
												<xsl:value-of select="travelProduct/companyDetail/identification"/>
											</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
								</OperatingAirline>
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
								<xsl:variable name="segnum">
									<xsl:value-of select="elementManagementItinerary/reference/number"/>
								</xsl:variable>
								<xsl:if test="../../segmentGroupingInfo[groupingCode!='CNX'][marriageDetail/tatooNum=$segnum]">
									<xsl:variable name="marnum">
										<xsl:for-each select="../../segmentGroupingInfo[groupingCode!='CNX']">
											<xsl:if test="marriageDetail/tatooNum=$segnum">
												<xsl:value-of select="position()"/>
											</xsl:if>
										</xsl:for-each>
									</xsl:variable>
									<MarriageGrp>
										<xsl:value-of select="concat(../../segmentGroupingInfo[groupingCode!='CNX'][marriageDetail/tatooNum=$segnum]/groupingCode,$marnum)"/>
									</MarriageGrp>
								</xsl:if>
								<TPA_Extensions>
									<xsl:attribute name="ConfirmationNumber">
										<xsl:value-of select="itineraryReservationInfo/reservation/controlNumber"/>
									</xsl:attribute>
									<xsl:if test="../../Air_FlightInfoReply">
										<xsl:variable name="dep">
											<xsl:value-of select="travelProduct/boardpointDetail/cityCode"/>
										</xsl:variable>
										<xsl:variable name="arr">
											<xsl:value-of select="travelProduct/offpointDetail/cityCode"/>
										</xsl:variable>
										<xsl:variable name="duration">
											<xsl:value-of select="../../Air_FlightInfoReply[flightScheduleDetails/boardPointAndOffPointDetails/generalFlightInfo/boardPointDetails/trueLocationId=$dep and flightScheduleDetails/boardPointAndOffPointDetails/generalFlightInfo/offPointDetails/trueLocationId=$arr]/flightScheduleDetails/additionalProductDetails/facilitiesInformation/description"/>
										</xsl:variable>
										<xsl:if test="$duration!=''">
											<xsl:attribute name="FlightDuration">
												<xsl:value-of select="substring($duration,1,2)"/>
												<xsl:value-of select="string(':')"/>
												<xsl:value-of select="substring($duration,3,2)"/>
											</xsl:attribute>
										</xsl:if>
									</xsl:if>
								</TPA_Extensions>
							</Air>
						</xsl:otherwise>
					</xsl:choose>
				</Item>
			</xsl:otherwise>
		</xsl:choose>
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
	<xsl:template match="dataElementsIndiv" mode="phone">
		<xsl:if test="otherDataFreetext/freetextDetail/type!='P02'">
			<Telephone>
				<xsl:attribute name="PhoneUseType">
					<xsl:choose>
						<xsl:when test="otherDataFreetext/freetextDetail/type='4'">H</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='P01'">F</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='6'">A</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='3'">B</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail/type='7'">M</xsl:when>
						<xsl:when test="otherDataFreetext/freetextDetail[type='5' and status='X']">X</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="phonetype">
								<xsl:value-of select="translate(otherDataFreetext/longFreetext,' ','')"/>
							</xsl:variable>
							<xsl:variable name="pt">
								<xsl:value-of select="substring($phonetype,string-length($phonetype) - 1)"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="substring($pt,1,1) = '-'">
									<xsl:value-of select="substring($pt,2)"/>
								</xsl:when>
								<xsl:otherwise>O</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="PhoneNumber">
					<xsl:value-of select="otherDataFreetext/longFreetext"/>
				</xsl:attribute>
				<xsl:attribute name="PIN">
					<xsl:value-of select="elementManagementData/lineNumber"/>
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
					<xsl:text>20</xsl:text><xsl:value-of select="substring(ticketElement/ticket/date,5,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(ticketElement/ticket/date,3,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(ticketElement/ticket/date,1,2)"/><xsl:text>T</xsl:text><xsl:value-of select="substring(ticketElement/ticket/time,1,2)"/>:<xsl:value-of select="substring(ticketElement/ticket/time,3,2)"/><xsl:text>:00</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="TicketType">
				<xsl:choose>
					<xsl:when test="ticketElement/ticket/electronicTicketFlag !=''">eTicket</xsl:when>
					<xsl:when test="../../originDestinationDetails/itineraryInfo/travelProduct/typeDetail/detail ='ET'">eTicket</xsl:when>
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
			<xsl:if test="ticketElement/ticket/indicator = 'XL'">
				<TicketAdvisory>
					<xsl:text>XL-</xsl:text>
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
	<xsl:template match="dataElementsIndiv" mode="email">
		<!--done -->
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
		<xsl:variable name="payment">
			<xsl:value-of select="concat(otherDataFreetext/longFreetext,'+')"/>
		</xsl:variable>
		<xsl:call-template name="ProcessPayment">
			<xsl:with-param name="payment">
				<xsl:value-of select="$payment"/>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="ProcessPayment">
		<xsl:param name="payment"/>
		<xsl:if test="$payment!=''">
			<xsl:variable name="paytext">
				<xsl:choose>
					<xsl:when test="substring($payment,1,1)='/'">
						<xsl:value-of select="substring-before(substring($payment,2),'+')"/>
					</xsl:when>
					<xsl:when test="substring($payment,1,2)='AX' and string-length($payment) > 16">
						<xsl:value-of select="substring-before(concat('CC',$payment),'+')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before($payment,'+')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="substring($paytext,1,2) ='CC' or substring($paytext,1,6) = 'PAX CC' or substring($paytext,1,6) = 'INF CC' or substring($paytext,1,8) = 'PAX O/CC' or substring($paytext,1,8) = 'INF O/CC' or  substring($paytext,1,4) = 'O/CC'">
					<xsl:variable name="validCC">
						<xsl:value-of select="'AU/AX/BC/BL/CA/CB/CG/CX/DC/DN/DK/DS/EC/GK/JC/MC/TC/TP/VI/XS'"/>
					</xsl:variable>
					<xsl:if test="string-length(substring-after($paytext,'CC')) > 2">
						<xsl:variable name="cc">
							<xsl:choose>
								<xsl:when test="substring($paytext,8,1)='/'">
									<xsl:value-of select="concat(substring($paytext,1,5),'C')"/>
									<xsl:value-of select="substring($paytext,6,2)"/>
									<xsl:value-of select="substring($paytext,9)"/>
								</xsl:when>
								<xsl:when test="substring($paytext,4,1)='/'">
									<xsl:value-of select="concat(substring($paytext,1,1),'C')"/>
									<xsl:value-of select="substring($paytext,2,2)"/>
									<xsl:value-of select="substring($paytext,5)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$paytext"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="cardCode">
							<xsl:value-of select="substring(substring-after($cc,'CC'),1,2)"/>
						</xsl:variable>
						<xsl:if test="contains($validCC,$cardCode)">
							<FormOfPayment>
								<xsl:attribute name="RPH">
									<xsl:value-of select="elementManagementData/lineNumber"/>
								</xsl:attribute>

								<xsl:variable name="card">
									<xsl:choose>
										<xsl:when test="contains($cc,'+')">
											<xsl:value-of select="substring-before(substring-after(substring-after($cc,'/'),'/'),'+')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(substring-after($cc,'/'),'/')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="conf">
									<xsl:if test="$card != ''">
										<xsl:choose>
											<xsl:when test="contains($card,'/N')">
												<xsl:value-of select="concat('N',substring-after($card,'/N'))"/>
											</xsl:when>
											<xsl:when test="contains($card,'/A')">
												<xsl:value-of select="concat('N',substring-after($card,'/A'))"/>
											</xsl:when>
											<xsl:when test="contains($card,'/') and not(contains($card,'.'))">
												<xsl:value-of select="$card"/>
											</xsl:when>
											<xsl:when test="contains($card,'/')">
												<xsl:value-of select="substring-after($card,'/')"/>
											</xsl:when>
											<xsl:when test="substring($card,1,1)='N' and not(contains($card,'.'))">
												<xsl:value-of select="$card"/>
											</xsl:when>
											<xsl:when test="substring($card,1,1)='A' and not(contains($card,'.'))">
												<xsl:value-of select="$card"/>
											</xsl:when>
											<xsl:when test="not(contains($card,'.'))">
												<xsl:value-of select="$card"/>
											</xsl:when>
										</xsl:choose>
									</xsl:if>
								</xsl:variable>
								
								<PaymentCard>
									<xsl:attribute name="CardCode">
										<xsl:choose>
											<xsl:when test="$cardCode = 'CA'">MC</xsl:when>
											<xsl:when test="$cardCode = 'DC'">DN</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$cardCode"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>									
									<xsl:if test="$conf!=''">
										<xsl:attribute name="ConfirmationNumber">
											<xsl:choose>
												<xsl:when test="contains($conf,'+')">
													<xsl:value-of select="substring-before($conf,'+')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$conf"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</xsl:if>									
									<xsl:choose>
										<xsl:when test="substring($cc,5,1)='/'">
											<xsl:attribute name="CardNumber">
												<xsl:value-of select="substring-before(substring($cc,6),'D')"/>
											</xsl:attribute>
											<xsl:attribute name="ExpireDate">
												<xsl:value-of select="substring(substring-after(substring($cc,6),'D'),1,2)"/>
												<xsl:value-of select="substring(substring-after(substring($cc,6),'D'),3,2)"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="contains(substring-after($cc,'CC'),'/X')">
											<xsl:attribute name="CardNumber">
												<xsl:value-of select="substring-before(substring(substring-after($cc,'CC'),3),'/X')"/>
											</xsl:attribute>
											<xsl:attribute name="ExpireDate">
												<xsl:value-of select="substring(substring-after(substring(substring-after($cc,'CC'),3),'/X'),1,2)"/>
												<xsl:value-of select="substring(substring-after(substring(substring-after($cc,'CC'),3),'/X'),3,2)"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="substring($cc,1,6) = 'PAX CC' or substring($cc,1,6) = 'INF CC'">
											<xsl:variable name="sep">
												<xsl:choose>
													<xsl:when test="contains(substring-before(substring($cc,9),'/'),'D')">
														<xsl:value-of select="'D'"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="'/'"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:attribute name="CardNumber">
												<xsl:value-of select="substring-before(substring($cc,9),$sep)"/>
											</xsl:attribute>
											<xsl:attribute name="ExpireDate">
												<xsl:value-of select="substring(substring-after($cc,$sep),1,2)"/>
												<xsl:value-of select="substring(substring-after($cc,$sep),3,2)"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="substring($cc,1,8) = 'PAX O/CC' or substring($cc,1,8) = 'INF O/CC'">
											<xsl:variable name="sep">
												<xsl:choose>
													<xsl:when test="contains(substring-before(substring($cc,11),'/'),'D')">
														<xsl:value-of select="'D'"/>
													</xsl:when>
													<xsl:when test="contains(substring($cc,11),'/')">
														<xsl:value-of select="'/'"/>
													</xsl:when>
												</xsl:choose>
											</xsl:variable>
											<xsl:attribute name="CardNumber">
												<xsl:choose>
													<xsl:when test="$sep!=''">
														<xsl:value-of select="substring-before(substring($cc,11),$sep)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="substring($cc,11)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:if test="$sep!=''">
												<xsl:attribute name="ExpireDate">
													<xsl:value-of select="substring(substring-after($cc,$sep),1,2)"/>
													<xsl:value-of select="substring(substring-after($cc,$sep),3,2)"/>
												</xsl:attribute>
											</xsl:if>
										</xsl:when>
										<xsl:when test="substring($cc,1,2) = 'O/'">
											<xsl:variable name="sep">
												<xsl:choose>
													<xsl:when test="contains(substring($cc,7),'EXPT')">
														<xsl:value-of select="'EXPT'"/>
													</xsl:when>
													<xsl:when test="contains(substring($cc,7),'EXP')">
														<xsl:value-of select="'EXP'"/>
													</xsl:when>
													<xsl:when test="contains(substring($cc,7),'/')">
														<xsl:value-of select="'/'"/>
													</xsl:when>
												</xsl:choose>
											</xsl:variable>
											<xsl:attribute name="CardNumber">
												<xsl:choose>
													<xsl:when test="$sep!=''">
														<xsl:value-of select="substring-before(substring($cc,7),$sep)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="substring($cc,7)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:if test="$sep!=''">
												<xsl:attribute name="ExpireDate">
													<xsl:value-of select="substring(substring-after(substring($cc,7),$sep),1,4)"/>
												</xsl:attribute>
											</xsl:if>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="CardNumber">
												<xsl:value-of select="substring-before(substring($cc,5),'/')"/>
											</xsl:attribute>
											<xsl:attribute name="ExpireDate">
												<xsl:choose>
													<xsl:when test="substring(substring-after($cc,'/'),1,1) = 'X'">
														<xsl:value-of select="substring(substring-after($cc,'/'),2,2)"/>
														<xsl:value-of select="substring(substring-after($cc,'/'),4,2)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="substring(substring-after($cc,'/'),1,2)"/>
														<xsl:value-of select="substring(substring-after($cc,'/'),3,2)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>									
								</PaymentCard>
								
								<xsl:variable name="amt">
									<xsl:if test="$card != ''">
										<xsl:choose>
											<xsl:when test="contains($card,'/N')">
												<xsl:value-of select="substring-before($card,'/N')"/>
											</xsl:when>
											<xsl:when test="contains($card,'/A')">
												<xsl:value-of select="substring-before($card,'/A')"/>
											</xsl:when>
											<xsl:when test="substring($card,1,1)='N' and string(number(substring($card,2,1)))!='NaN'"/>
											<xsl:when test="substring($card,1,1)='A' and string(number(substring($card,2,1)))!='NaN'"/>
											<xsl:otherwise>
												<xsl:value-of select="$card"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:if>
								</xsl:variable>
								
								<TPA_Extensions>
									<xsl:attribute name="FOPType">CC</xsl:attribute>
									<xsl:if test="$conf!=''">
										<xsl:attribute name="ConfirmationNumber">
											<xsl:choose>
												<xsl:when test="contains($conf,'+')">
													<xsl:value-of select="substring-before($conf,'+')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$conf"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</xsl:if>
									<xsl:if test="referenceForDataElement/reference[qualifier='PT']">
										<xsl:attribute name="RPH">
											<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
												<xsl:if test="position() > 1">
													<xsl:value-of select="','"/>
												</xsl:if>
												<xsl:variable name="number">
													<xsl:value-of select="number"/>
												</xsl:variable>
												<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
											</xsl:for-each>
										</xsl:attribute>
									</xsl:if>
									<xsl:if test="contains(otherDataFreetext/longFreetext,'INF ')">INF</xsl:if>
								</TPA_Extensions>
								
								<xsl:if test="$amt!='' and (string(number($amt))!='NaN' or string(number(substring($amt,4)))!='NaN')">
									<PaymentAmount>
										<xsl:attribute name="Amount">
											<xsl:choose>
												<xsl:when test="string(number(substring($amt,1,1)))!='NaN'">
													<xsl:value-of select="translate($amt,'.','')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="translate(substring($amt,4),'.','')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:if test="not(number(substring($amt,1,1)))">
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="substring($amt,1,3)"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="DecimalPlaces">
											<xsl:choose>
												<xsl:when test="contains($amt,'.')">2</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentAmount>
								</xsl:if>
							</FormOfPayment>
						</xsl:if>
					</xsl:if>
				</xsl:when>
				<xsl:when test="contains($paytext,'CHECK')">
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="elementManagementData/lineNumber"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
						</DirectBill>
						<xsl:if test="contains(substring-after($paytext,'CHECK'),'/') and substring($paytext,1,2)!= 'O/'">
							<xsl:variable name="amt">
								<xsl:value-of select="substring-after(substring-after($paytext,'CHECK'),'/')"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="number(substring($amt,1,1))">
									<PaymentAmount>
										<xsl:attribute name="Amount">
											<xsl:value-of select="translate($amt,'.','')"/>
										</xsl:attribute>
										<xsl:if test="not(number(substring($amt,1,1)))">
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="substring($amt,1,3)"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="DecimalPlaces">
											<xsl:choose>
												<xsl:when test="contains($amt,'.')">2</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentAmount>
								</xsl:when>
								<xsl:when test="number(substring($amt,4,1))">
									<PaymentAmount>
										<xsl:attribute name="Amount">
											<xsl:value-of select="translate(substring($amt,4),'.','')"/>
										</xsl:attribute>
										<xsl:if test="not(number(substring($amt,1,1)))">
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="substring($amt,1,3)"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="DecimalPlaces">
											<xsl:choose>
												<xsl:when test="contains($amt,'.')">2</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentAmount>
								</xsl:when>
							</xsl:choose>
						</xsl:if>

						<TPA_Extensions>
							<xsl:attribute name="FOPType">Check</xsl:attribute>
							<xsl:if test="referenceForDataElement/reference[qualifier='PT']">
								<xsl:attribute name="RPH">
									<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
										<xsl:if test="position() > 1">
											<xsl:value-of select="','"/>
										</xsl:if>
										<xsl:variable name="number">
											<xsl:value-of select="number"/>
										</xsl:variable>
										<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
									</xsl:for-each>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="contains(otherDataFreetext/longFreetext,'INF ')">INF</xsl:if>
						</TPA_Extensions>
					</FormOfPayment>
				</xsl:when>
				<xsl:when test="contains($paytext,'CASH')">
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="elementManagementData/lineNumber"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
						</DirectBill>
						<xsl:if test="contains(substring-after($paytext,'CASH'),'/') and substring($paytext,1,2)!= 'O/'">
							<xsl:variable name="amt">
								<xsl:value-of select="substring-after(substring-after($paytext,'CASH'),'/')"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="number(substring($amt,1,1))">
									<PaymentAmount>
										<xsl:attribute name="Amount">
											<xsl:value-of select="translate($amt,'.','')"/>
										</xsl:attribute>
										<xsl:if test="not(number(substring($amt,1,1)))">
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="substring($amt,1,3)"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="DecimalPlaces">
											<xsl:choose>
												<xsl:when test="contains($amt,'.')">2</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentAmount>
								</xsl:when>
								<xsl:when test="number(substring($amt,4,1))">
									<PaymentAmount>
										<xsl:attribute name="Amount">
											<xsl:value-of select="translate(substring($amt,4),'.','')"/>
										</xsl:attribute>
										<xsl:if test="not(number(substring($amt,1,1)))">
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="substring($amt,1,3)"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="DecimalPlaces">
											<xsl:choose>
												<xsl:when test="contains($amt,'.')">2</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PaymentAmount>
								</xsl:when>
							</xsl:choose>
						</xsl:if>
						<TPA_Extensions>
							<xsl:attribute name="FOPType">Cash</xsl:attribute>
							<xsl:if test="referenceForDataElement/reference[qualifier='PT']">
								<xsl:attribute name="RPH">
									<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
										<xsl:if test="position() > 1">
											<xsl:value-of select="','"/>
										</xsl:if>
										<xsl:variable name="number">
											<xsl:value-of select="number"/>
										</xsl:variable>
										<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
									</xsl:for-each>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="contains(otherDataFreetext/longFreetext,'INF ')">INF</xsl:if>
						</TPA_Extensions>
					</FormOfPayment>
				</xsl:when>
				<xsl:otherwise>
					<FormOfPayment>
						<MiscChargeOrder>
							<xsl:value-of select="$paytext"/>
						</MiscChargeOrder>
					</FormOfPayment>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="ProcessPayment">
				<xsl:with-param name="payment">
					<xsl:value-of select="substring-after($payment,'+')"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- MCO	   		                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="MCO">
		<FormOfPayment>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:variable name="payment">
				<xsl:value-of select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FP']/fareElementData/freeText"/>
			</xsl:variable>
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
					<xsl:attribute name="FOPType">
						<xsl:choose>
							<xsl:when test="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FP' and starts-with(fareElementData/freeText,'CC')]">СС</xsl:when>
							<!--<xsl:when test="elementManagementData/segmentName = 'AB'">Check</xsl:when>-->
							<!--<xsl:when test="elementManagementData/segmentName = 'AB'">Cash</xsl:when>-->
							<xsl:otherwise></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<!--<xsl:attribute name="FOPType">CC</xsl:attribute>-->
					<xsl:for-each select="mcoRecord/groupOfFareElements[fareElementData/freeTextDetails/informationType='FM']">
						<AgencyCommission>
							<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
								<xsl:attribute name="TravelerRefNumberRPHList">
									<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
										<xsl:if test="position() > 1">
											<xsl:value-of select="' '"/>
										</xsl:if>
										<xsl:variable name="number">
											<xsl:value-of select="number"/>
										</xsl:variable>
										<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
									</xsl:for-each>
								</xsl:attribute>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="contains(fareElementData/freeText,'*M*') or contains(fareElementData/freeText,'*C*') or contains(fareElementData/freeText,'*G*') or contains(fareElementData/freeText,'*D*') or contains(fareElementData/freeText,'*F*')">
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
											<xsl:when test="contains(fareElementData/freeText,'*D*')">
												<xsl:value-of select="substring-after(fareElementData/freeText,'*D*')"/>
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
										<xsl:value-of select="translate(substring-after(fareElementData/freeText,'*AM*'),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
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
								<xsl:choose>
									<xsl:when test="substring(substring-after(fareElementData/freeText,'/'),2,1)='V'">
										<IssuedTicket>
											<!-- PAX 230-5058181319/PVCM/USD26.25/14JAN20/FLL1S212V/10729143 -->
											<xsl:choose>
												<xsl:when test="fareElementData/freeTextDetails/source='M'">
													<xsl:value-of select="concat('MCO ', substring-after(concat(fareElementData/freeText, ' *VOID* '), ' '))"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="concat(fareElementData/freeText, ' *VOID* ')"/>
												</xsl:otherwise>
											</xsl:choose>
										</IssuedTicket>
									</xsl:when>
									<xsl:otherwise>
										<IssuedTicket>
											<xsl:choose>
												<xsl:when test="fareElementData/freeTextDetails/source='M'">
													<xsl:value-of select="concat('MCO ', substring-after(fareElementData/freeText, ' '))"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="fareElementData/freeText"/>
												</xsl:otherwise>
											</xsl:choose>
										</IssuedTicket>
									</xsl:otherwise>

								</xsl:choose>

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
	<xsl:template match="dataElementsIndiv" mode="GenRemark">
		<Remark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:if test="miscellaneousRemarks/remarks/category != ''">
				<xsl:attribute name="Category">
					<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='PT'">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
						<xsl:if test="position() > 1">
							<xsl:value-of select="' '"/>
						</xsl:if>
						<xsl:variable name="number">
							<xsl:value-of select="number"/>
						</xsl:variable>
						<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="referenceForDataElement/reference/qualifier='ST'">
				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:for-each select="referenceForDataElement/reference[qualifier='ST']">
						<xsl:if test="position() > 1">
							<xsl:value-of select="' '"/>
						</xsl:if>
						<xsl:variable name="number">
							<xsl:value-of select="number"/>
						</xsl:variable>
						<xsl:value-of select="../../../../originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number=$number]/lineNumber"/>
					</xsl:for-each>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
		</Remark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Miscellaneous Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Corporate">
		<Remark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:attribute name="Category">Hidden Remark</xsl:attribute>

			<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
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
	<!-- Confidential Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="ConfRemark">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:attribute name="RemarkType">C</xsl:attribute>
			<Text>
				<xsl:for-each select="miscellaneousRemarks/individualSecurity">
					<xsl:if test="position() &gt; 1">
						<xsl:text>,</xsl:text>
					</xsl:if>
					<xsl:value-of select="office"/>
				</xsl:for-each>
				<xsl:text>//</xsl:text>
				<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- Endorsement Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="Endorsement">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="referenceForDataElement/reference/qualifier='ST' or referenceForDataElement/reference/qualifier='PT'">
					<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
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
					<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text>
				<xsl:value-of select="otherDataFreetext/longFreetext"/>
			</Text>
		</SpecialRemark>
	</xsl:template>

	<xsl:template match="fareComponentDetailsGroup" mode="controllingCarrier">

		<xsl:if test="fareFamilyDetails/fareFamilyname">
			<SpecialRemark>

				<xsl:attribute name="RPH">
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="fareFamilyOwner" />
					</xsl:call-template>
				</xsl:attribute>
				<xsl:attribute name="RemarkType">CC</xsl:attribute>

				<FlightRefNumber>
					<xsl:attribute name="RPH">
						<xsl:for-each select="couponDetailsGroup/productId/referenceDetails[type='ST']/value">
							<xsl:variable name="segref">
								<xsl:value-of select="."/>
							</xsl:variable>
							<xsl:variable name="rph">
								<xsl:value-of select="/PNR_Reply/originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $segref]/elementManagementItinerary/lineNumber"/>
							</xsl:variable>
							<xsl:if test="position() > 1">
								<xsl:text> </xsl:text>
							</xsl:if>
							<xsl:value-of select="$rph"/>
						</xsl:for-each>
					</xsl:attribute>
				</FlightRefNumber>

				<Text>
					<xsl:value-of select="concat(marketFareComponent/boardPointDetails/trueLocationId,marketFareComponent/offpointDetails/trueLocationId, ' -',fareFamilyDetails/fareFamilyname, '/', fareFamilyOwner/companyIdentification/otherCompany)"/>
				</Text>
			</SpecialRemark>
		</xsl:if>

	</xsl:template>
	<!-- ************************************************************** -->
	<!-- UHP                        	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="dataElementsIndiv" mode="UHP">
		<UniqueRemark>
			<xsl:attribute name="RemarkType">UHP</xsl:attribute>
			<xsl:value-of select="otherDataFreetext/longFreetext"/>
		</UniqueRemark>
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
	<xsl:template match="attributeDetails" mode="TourCode">
		<SpecialRemark>
			<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
			<Text>
				<xsl:value-of select="substring-before(./attributeDescription, '*')"/>
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
				<Text><xsl:value-of select="otherDataFreetext/longFreetext"/></Text>
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
					<xsl:value-of select="serviceRequest/ssr/companyId"/>
				</xsl:attribute>
			</Airline>
			<Text>
				<xsl:text>Status:</xsl:text>
				<xsl:value-of select="concat(serviceRequest/ssr/status,serviceRequest/ssr/quantity)"/>
				<xsl:if test="serviceRequest/ssr/freeText != ''">
					<xsl:text>-</xsl:text>
					<xsl:value-of select="serviceRequest/ssr/freeText"/>
				</xsl:if>
				<xsl:if test="frequentFlyerInformationGroup/frequentTravellerInfo/frequentTraveler/membershipNumber != ''">
					<xsl:text>-</xsl:text>
					<xsl:value-of select="frequentFlyerInformationGroup/frequentTravellerInfo/frequentTraveler/membershipNumber"/>
				</xsl:if>
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
						</xsl:attribute>
						<xsl:attribute name="FlightRefNumberRPHList">
							<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
						</xsl:attribute>
						<xsl:if test="../../seatPaxInfo/seatPaxIndicator/statusDetails/indicator='C0'">
							<xsl:attribute name="Chargeable">true</xsl:attribute>
						</xsl:if>
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
						<xsl:if test="../../seatPaxInfo/seatPaxIndicator/statusDetails/indicator='C0'">
							<xsl:attribute name="Chargeable">true</xsl:attribute>
						</xsl:if>
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
		<xsl:variable name="tktType">
			<xsl:choose>
				<xsl:when test="contains(otherDataFreetext/longFreetext, '/DT')">
					<xsl:value-of select="concat('EMD', ' ', substring-before(otherDataFreetext/longFreetext, ' '))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(otherDataFreetext/longFreetext, ' ')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
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
			<xsl:if test="referenceForDataElement/reference/qualifier='ST' and not(starts-with($tktType, 'EMD'))">
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
			<xsl:value-of select="concat($tktType, ' ',substring-after(otherDataFreetext/longFreetext, ' '))"/>
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
	<xsl:template match="dataElementsIndiv" mode="EMD">
		<xsl:variable name="tktType">
			<xsl:value-of select="substring-before(otherDataFreetext/longFreetext, ' ')"/>
		</xsl:variable>

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
			<xsl:value-of select="concat('MCO ', $tktType, ' ',substring-after(otherDataFreetext/longFreetext, ' '))"/>
		</IssuedTicket>
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

			<xsl:variable name="paxRef" select="referenceForDataElement/reference/number" />
			<xsl:choose>
				<xsl:when test="referenceForDataElement/reference/qualifier='PT'">
					<xsl:attribute name="TravelerRefNumberRPHList">
						<xsl:for-each select="referenceForDataElement/reference[qualifier='PT']">
							<xsl:if test="position() > 1">
								<xsl:value-of select="' '"/>
							</xsl:if>
							<xsl:variable name="number">
								<xsl:value-of select="number"/>
							</xsl:variable>
							<xsl:value-of select="../../../../travellerInfo/elementManagementPassenger[reference/number=$number]/lineNumber"/>
						</xsl:for-each>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="not($paxRef)">
					<xsl:attribute name="TravelerRefNumberRPHList">
						<xsl:for-each select="../../travellerInfo[passengerData/travellerInformation/passenger/type='INF']/elementManagementPassenger/lineNumber">
							<xsl:if test="position() > 1">
								<xsl:value-of select="' '"/>
							</xsl:if>
							<xsl:variable name="number">
								<xsl:value-of select="."/>
							</xsl:variable>
							<xsl:value-of select="$number" />
						</xsl:for-each>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>


			<xsl:attribute name="RPH">
				<xsl:value-of select="elementManagementData/lineNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="contains(otherDataFreetext/longFreetext,'*M*') or contains(otherDataFreetext/longFreetext,'*C*') or contains(otherDataFreetext/longFreetext,'*G*') or contains(otherDataFreetext/longFreetext,'*F*') or contains(otherDataFreetext/longFreetext,'*V*') ">
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
							<xsl:when test="contains(otherDataFreetext/longFreetext,'*V*')">
								<xsl:value-of select="substring-after(otherDataFreetext/longFreetext,'*V*')"/>
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
							<xsl:if test="translate(substring-before($comm,'N'),'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','') != ''">
								<xsl:attribute name="Percent">
									<xsl:value-of select="translate(substring-before($comm,'N'),'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','')"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:if test="translate($comm,'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','') != ''">
								<xsl:attribute name="Percent">
									<xsl:value-of select="translate($comm,'ABCDEFGHIJKLMNOPQRSTUVWXYZ* ','')"/>
								</xsl:attribute>
							</xsl:if>
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

	<xsl:template match="dataElementsIndiv" mode="accounting">
		<AccountingLine>
			<xsl:value-of select="accounting/account/number"/>
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

	<xsl:template match="fareDataInformation" mode="totalbase">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:choose>
				<xsl:when test="fareDataSupInformation[fareDataQualifier = 'E']">
					<xsl:choose>
						<xsl:when test="fareDataSupInformation[fareDataQualifier = 'E']/fareAmount != ''">
							<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nopt"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nopt"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nopt"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalbase">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="fareDataInformation" mode="totalequiv">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nopt"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalequiv">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="fareDataInformation" mode="totalTax">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:choose>
				<xsl:when test="../taxInformation[1] != ''">
					<xsl:value-of select="translate(../taxInformation[1]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot2">
			<xsl:choose>
				<xsl:when test="../taxInformation[2] != ''">
					<xsl:value-of select="translate(../taxInformation[2]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot3">
			<xsl:choose>
				<xsl:when test="../taxInformation[3] != ''">
					<xsl:value-of select="translate(../taxInformation[3]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot4">
			<xsl:choose>
				<xsl:when test="../taxInformation[4] != ''">
					<xsl:value-of select="translate(../taxInformation[4]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot5">
			<xsl:choose>
				<xsl:when test="../taxInformation[5] != ''">
					<xsl:value-of select="translate(../taxInformation[5]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot6">
			<xsl:choose>
				<xsl:when test="../taxInformation[6] != ''">
					<xsl:value-of select="translate(../taxInformation[6]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot7">
			<xsl:choose>
				<xsl:when test="../taxInformation[7] != ''">
					<xsl:value-of select="translate(../taxInformation[7]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot8">
			<xsl:choose>
				<xsl:when test="../taxInformation[8] != ''">
					<xsl:value-of select="translate(../taxInformation[8]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot9">
			<xsl:choose>
				<xsl:when test="../taxInformation[9] != ''">
					<xsl:value-of select="translate(../taxInformation[9]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot10">
			<xsl:choose>
				<xsl:when test="../taxInformation[10] != ''">
					<xsl:value-of select="translate(../taxInformation[10]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot11">
			<xsl:choose>
				<xsl:when test="../taxInformation[11] != ''">
					<xsl:value-of select="translate(../taxInformation[11]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="tot12">
			<xsl:choose>
				<xsl:when test="../taxInformation[12] != ''">
					<xsl:value-of select="translate(../taxInformation[12]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="tot13">
			<xsl:choose>
				<xsl:when test="../taxInformation[13] != ''">
					<xsl:value-of select="translate(../taxInformation[13]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="tot14">
			<xsl:choose>
				<xsl:when test="../taxInformation[14] != ''">
					<xsl:value-of select="translate(../taxInformation[14]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="tot15">
			<xsl:choose>
				<xsl:when test="../taxInformation[15] != ''">
					<xsl:value-of select="translate(../taxInformation[15]/amountDetails/fareDataMainInformation/fareAmount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalTax">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $tot2 + $tot3 + $tot4 + $tot5 +$tot6 +$tot7 +$tot8 +$tot9 +$tot10  +$tot11  +$tot12  +$tot13 +$tot14 +$tot15  + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $tot2 + $tot3 + $tot4+ $tot5 +$tot6 +$tot7 +$tot8 +$tot9 +$tot10  +$tot11  +$tot12  +$tot13 +$tot14 +$tot15  + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="fareDataInformation" mode="totalprice">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nopt"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalprice">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="fareDataInformation" mode="totalFee">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'TOF']/fareAmount,'.','') * $nopt"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalprice">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
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

	<xsl:template name="month">
		<xsl:param name="month"/>
		<xsl:choose>
			<xsl:when test="$month = 'JAN'">01</xsl:when>
			<xsl:when test="$month = 'FEB'">02</xsl:when>
			<xsl:when test="$month = 'MAR'">03</xsl:when>
			<xsl:when test="$month = 'APR'">04</xsl:when>
			<xsl:when test="$month = 'MAY'">05</xsl:when>
			<xsl:when test="$month = 'JUN'">06</xsl:when>
			<xsl:when test="$month = 'JUL'">07</xsl:when>
			<xsl:when test="$month = 'AUG'">08</xsl:when>
			<xsl:when test="$month = 'SEP'">09</xsl:when>
			<xsl:when test="$month = 'OCT'">10</xsl:when>
			<xsl:when test="$month = 'NOV'">11</xsl:when>
			<xsl:when test="$month = 'DEC'">12</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="year">
		<xsl:param name="year"/>
		<xsl:choose>
			<xsl:when test="$year > '40' and not($year > '99')">
				<xsl:value-of select="concat('19', $year)" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat('20', $year)" />
			</xsl:otherwise>
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

</xsl:stylesheet>
