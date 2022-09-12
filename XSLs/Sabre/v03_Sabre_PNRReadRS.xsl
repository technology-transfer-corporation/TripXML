<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:stl="http://services.sabre.com/STL/v01"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ext="http://exslt.org/common" exclude-result-prefixes="ext">
	<!-- 
  ================================================================== 
  v03_Sabre_PNRReadRS.xsl 														
  ==================================================================
  Date: 28 Jul 2022 - Kobelev - PNR Read with unmasked CC
  Date: 19 Jul 2022 - Samokhvalov - QueueRead - Pax Type Fixes
  Date: 08 Jul 2022 - Samokhvalov - Controlling Carrier Remark reworked. Added GI to Air Segments.
  Date: 23 May 2022 - Kobelev - Ticket Designator fix.
  Date: 18 May 2022 - Kobelev - Tour Code FlightRefNumberRPHList fix.
  Date: 06 May 2022 - Kobelev - Tour Code from Future Pricing line.
  Date: 29 Apr 2022 - Kobelev - EMD Exchange and EMD Service Fee display fix.
  Date: 01 Apr 2022 - Samokhvalov - Fixed Branded Fare object(ARNK)
  Date: 21 Oct 2021 - Kobelev - MCO identification fix.
  Date: 21 Oct 2021 - Kobelev - Change Controlling Carrier RemarkType from "Z" to "CC".
  Date: 02 Sep 2021 - Kobelev - Multi FlightSegment in Item for Controling Carrier process.
  Date: 16 Aug 2021 - Kobelev - Controlling Carrier Identification.
  Date: 13 Aug 2021 - Kobelev - Controling Carrier in Special Remarks Remark Type "Endorsements".
  Date: 08 Jul 2021 - Kobelev - Branded Fare Name different for different Segments.
  Date: 08 Jul 2021 - Kobelev - Branded Fare Name fix.
  Date: 01 Jul 2021 - Kobelev - Branded Fare when price command starts with WPBR.
  Date: 27 May 2021 - Kobelev - Branded Fare when information paired by Segments.
  Date: 10 Mar 2021 - Kobelev - Price Quote Reference FIX.
  Date: 04 Mar 2021 - Kobelev - Air Segments Referencies For Issued Tickets Fix.
  Date: 04 Mar 2021 - Kobelev - Air Segments Display Fix (with or without ARNK).
  Date: 03 Mar 2021 - Kobelev - Side Trip Fix.
  Date: 17 Feb 2021 - Kobelev - BSR Calculations
  Date: 03 Feb 2021 - Kobelev - Better implamentation of Exchange PNR with two PQ (one correct one not, both active) vs. regular PNR  
  Date: 29 Dec 2020 - Kobelev - Fixed Exchange PNR with two PQ (one correct one not, both active)
  Date: 05 Dec 2020 - Samokhvalov - Fixed overnight flight 31 Dec arrival date BUG: 1366
  Date: 02 Dec 2020 - Samokhvalov - Fixed PTC_FareBreakdown/@PricingSource BUG: 1357
  Date: 26 Aug 2020 - Kobelev - Corrected display of Owner and Creator of PNR
  Date: 17 Jul 2020 - Kobelev - Displaying Segmental and non segmental Branded Fares
  Date: 16 Jul 2020 - Kobelev - Displaying Branded Fares
  Date: 18 Jun 2020 - Babin -   Fixed FlightRefNumberRPHList. Added RPH variable from the correct node BUG: 1267
  Date: 09 Apr 2020 - Kobelev - Display of simple address.
  Date: 05 Mar 2020 - Kobelev - Display of Flight Segment Display when PNR has air segments with same flight Number.
  Date: 06 Feb 2020 - Kobelev - Display of Flight Segment Display when PNR has Voided / ARNK / Exchanges segments.
  Date: 05 Feb 2020 - Kobelev - Display of Flight Segment Display when PNR has voided segments.
  Date: 16 Dec 2019 - Kobelev - Incorrect Flight Segment Display when PNR has ARNK. BUG: 1154.
  Date: 21 Nov 2019 - Kobelev - In AccountingInfo Document Number Fix.
  Date: 14 Nov 2019 - Kobelev - MCO indicator when eTicketNumber starts with 'TK'
  Date: 22 Oct 2019 - Kobelev - IssuedTicket VOID indicator
  Date: 04 Oct 2019 - Kobelev - Added in AccountingInfo attribute TravelerRefNumberRPHList instead of field TravelerRefNumber
  Date: 03 Oct 2019 - Kobelev - Exchange display when exchange idetified with leter "e" as part of Ticket Number
  Date: 24 Sep 2019 - Kobelev - Fixed MCO vs. SPLIT MCO vs. Exchange display
  Date: 06 Sep 2019 - Kobelev - Fixed FlightRefNumberRPHList itentification for Issued Tickets
  Date: 23 May 2019 - Kobelev - Start using AltLangID as GDS idetifier
  Date: 03 apr 2019 - Samokhvalov - Added processing airsegments with not HK status
  Date: 29 mar 2019 - Kobelev - In CompanyName/Code we are writing PCC of the the office who Priced this PNR
  Date: 28 mar 2019 - Kobelev - Fixed repeative Air segments. In case when Airline changes pax class of service PNR gets HX air segment.
  Date: 27 sep 2018 - Kobelev - Fixed FOP display. Customers inject PNR with not FOP related information which leads to error in reading it.
  Date: 09 Jul 2018 - Samokhvalov - Fixed PAX type identification by PQS data.
  Date: 14 Jun 2018 - Kobelev - CompanyName object with Creation Date Time added.
  Date: 08 Jun 2018 - Kobelev - Fix Identification of Private or Published Fare. ...ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]
  Date: 23 Feb 2018 - Samokhvalov - Empty remarks changed to '.'.
  Date: 07 Feb 2018 - Samokhvalov - Fixed Endorsement remarks if no endorsement in PNR.
  Date: 05 Feb 2018 - Samokhvalov - Added Endorsement remarks.
  Date: 11 Jan 2018 - Kobelev - Fixed Flights with same Flight Number and Flightnumber formating to 3 or 4 characters.
  Date: 01 Dec 2017 - Kobelev - Fixed FlightRefNumberRPHList to IssuedTickets for side trip PNRs
  Date: 30 Nov 2017 - Samokhvalov - Added FlightRefNumberRPHList to IssuedTickets
  Date: 16 Nov 2017 - Samokhvalov - PTC=PFA added to Private Fare List, PassangeFare corrected -> amount per 1 pax.
  Date: 03 Nov 2017 - Kobelev - Multiple FOPs Line 3360.
  Date: 04 Oct 2017 - Kobelev - Correct Identification of PQ with Passangers.
  Date: 29 Sep 2017 - Samokhvalov - ItinSeqNumber FIX.
  Date: 20 Sep 2017 - Kobelev - In case C09 for PTC it has to use CNN instead of CHD. line 2803.
  Date: 19 Sep 2017 - Kobelev - Added original Sabre customer number to ProfileRefRS ID="01.03"
  Date: 06 Jul 2017 - Samokhvalov - Added TPA_Extentions to TravelCosts/FormOfPayment=CC
  Date: 06 Jun 2017 - Samokhvalov - Remarks moved to SpecialRequestDetails like as in Amadeus
  Date: 29 Aug 2017 - Kobelev - Added MCO indicator.
  Date: 23 May 2017 - Kobelev - Added RPH number formating function. Turns 1.1 into 01.01.
  Date: 05 May 2017 - Kobelev - Added FlightRefNumberRPHList to PTC_FareBreakDown.
  Date: 05 Apr 2017 - Kobelev - Fix for passenger Type. It should always go by PQS value.
  Date: 05 Apr 2017 - Kobelev - Fix for credit card with only 3 digit for Expiration Date. ex: *VI4XXXXXXXXXXX1111?1/19.
  Date: 21 Feb 2017 - Kobelev - ReservationItems has to take last element from PriceQuote.
  Date: 20 Feb 2017 - Kobelev - EquivFare fix. First step to BUG 529
  Date: 06 Feb 2017 - Kobelev - Form Of Payment check for CC number. 
  Date: 06 Jan 2017 - Kobelev - Correction on passanger RPH for Issued Tickets. 
  Date: 22 Dec 2016 - Kobelev - Expiration Date parsing FIX. 
  Date: 21 Dec 2016 - Kobelev - Added TravelerReferenceRPHList to PTC_FareBreakdown in ItemPricing. Also, added TourCode to SpecailRemarks. 
  Date: 10 Oct 2016 - Kobelev - Credit Card number can have leading "*" or not. Thats creates problem during parsing. 
  Date: 09 Sep 2016 - Kobelev - New Infant identifier. PNR 'HSPTHB'. 
  Date: 06 Sep 2016 - Kobelev - Canceled Air segments display. PNR 'WXQMTH'. 
  Date: 22 Aug 2016 - Kobelev - Price Quote did not display. Bug Fix of #349
  Date: 19 Aug 2016 - Kobelev - Correct display of Secured PNR.
  Date: 19 Aug 2016 - Kobelev - Canceled Air Segemnts just not getting displayed.
  Date: 12 Aug 2016 - Kobelev - Flight Number repiets in the one PNR. bug fix of #345
  Date: 10 Aug 2016 - Kobelev - Added new DQB ticket information collection. bug fix of #343
  Date: 25 Jul 2016 - Samokhvalov - AirSegments filled from AirItineraryPricingInfo/PTC_FareBreakdown instead of ItineraryInfo/ReservationItems
  Date: 12 Jyl 2016 - Kobelev - Fix check for attribute existance ItinTotalFare when PriceQuoteTotals absent. bug fix of #317
  Date: 22 Jun 2016 - Samokhvalov - Set Amount=0 for ItinTotalFare when PriceQuoteTotals absent. bug fix #317
  Date: 11 May 2016 - Kobelev - Total Price correct price quote. bug fix #290 
  Date: 21 Apr 2016 - Kobelev - MiscSegment vs. Misc element. after implementation of new version of messages. bug fix #269	
  Date: 19 Apr 2016 - Kobelev - Price Qoute Siaplaying correct price quote bug fix #267	
  Date: 12 Apr 2016 - Kobelev - Passanger type passanger identification bug fix	
  Date: 06 Apr 2016 - Rastko - corrected credit card mapping for verson 3.6.0		
  Date: 18 Mar 2016 - Rastko - corrected text error display		
  Date: 18 Dec 2015 - Rastko - change to car version 2.0.1		
  Date: 17 Dec 2015 - Rastko	- parse *PQS instead of PD for pax types					
  Date: 21 Jul 2015 - Rastko	- corrected mapping of EquivFare						
  Date: 17 Jun 2015 - Rastko	- mapped segment marriage info						
  Date: 28 Feb 2015 - Tkachenko - added DQB parser template
  Date: 26 Feb 2015 - Rastko - 	take pax type from Sabre if in response		
  Date: 16 Sep 2014 - Rastko - do not display hotel rate if no numeric value in Sabre response	
  Date: 20 May 2014 - Rastko - corrected tax amount when no tax returned in response	
  Date: 01 May 2014 - Rastko - mapped DisplayPriceQuoteRS to get private fare info		
  Date: 17 Apr 2014 - Rastko - added TransactionStatusCode attribute when PNR ticketed	
  Date: 15 Apr 2014 - Rastko - corrected RPH value of stored fare				
  Date: 10 Apr 2014 - Rastko - fixed parsing of foprm of payment CK, CHEQUE and CC	
  Date: 07 Apr 2014 - Rastko - made corrections in parsing manual hotel segment		
  Date: 03 Apr 2014 - Rastko - change test to identify if there are stored fares or not		
  Date: 02 Apr 2014 - Rastko - do not try to display stored fare priced starting with WS	
  Date: 02 Apr 2014 - Rastko - fixed FOP parsing when it is in first text line			
  Date: 01 Apr 2014 - Rastko - fixed car and form of payment free flow text			
  Date: 01 Apr 2014 - Rastko - fixed car and hotel segments parsiing issues			
  Date: 22 Mar 2014 - Rastko - fixed parsing issue on stored fares				
  Date: 21 Mar 2014 - Rastko - do not try to display stored fare priced as WS			
  Date: 19 Mar 2014 - Rastko - do not try to display stored fare if not any exist		
  Date: 18 Mar 2014 - Rastko - corrected fares calculation to include number of pax in it	
  Date: 25 Feb 2014 - Rastko - connect mapping of fare basis code				
  Date: 24 Feb 2014 - Rastko - if remark does not have a text return a space in text field	
  Date: 21 Feb 2014 - Rastko - do not show RPH in IssueTicket if it does not exist		
  Date: 05 Feb 2014 - Rastko - formatted RPH number in Payment and Remarks elements	
  Date: 05 Feb 2014 - Rastko - corrected Remark Type attribute into Category attribute	
  Date: 24 Jan 2014 - Rastko - corrected parsing of accounting line				
  Date: 18 Jan 2014 - Rastko - corrected display of Warning to be put after Success 		
  Date: 16 Jan 2014 - Rastko - mapped accounting info and future price info and PTC Breakdown RPH	
  Date: 13 Jan 2014 - Rastko - mapped Calc line									
  Date: 08 Jan 2014 - Rastko - mapped Cash and Check FOP					
  Date: 08 Jan 2014 - Rastko - set ReservationItems optional and corrected tkt time limit	
  Date: 08 Jan 2014 - Rastko - added mapping of pax types, accounting line and fare type	
  Date: 07 Jan 2014 - Rastko - added mapping of misc segments				
  Date: 02 Jan 2014 - Rastko - added mapping of arunk segments				
  Date: 01 Aug 2012 - Rastko - corrected FOP and baggage allowance mapping		
  Date: 06 Jun 2012 - Kasun - include Validating Airline Code					
  Date: 24 May 2012 - Kasun - include RPH in Issued Tickets					
  Date: 17 May 2012 - Kasun - corrected Arrival date format						
  Date: 25 Apr 2012 - Kasun - corrected error mapping							
  Date: 22 Mar 2012 - Kasun - mapped cash and check form of payments			
  Date: 11 Mar 2012 - Shashin - corrected year part in ticketingtimelimit			
  Date: 14 Feb 2012 - Rastko - corrected misc segment mapping				
  Date: 13 Jan 2012 - Rastko - added mapping of accounting line				
  Date: 24 Oct 2010 - Rastko - Made FOP multiple in response					
  Date: 26 Sep 2010 - Rastko - mapped CustLoyalty	  							
  Date: 08 Apr 2010 - Rastko - fixed total fares calculation 						
  Date: 29 Apr 2010 - Rastko - fixed parsing of fares when hostorical fares exist		
  Date: 20 May 2010 - Rastko - fixed ticket time limit parsing					
  Date: 25 May 2010 - Rastko - fixed ticket time limit parsing when code is 7TAW/		
  Date: 02 Jul 2010 - Rastko - fixed missing remarks in display					
  Date: 16 Jul 2010 - Rastko - fixed ticket time limit parsing when code is 7TAW/		
  Date: 06 Sep 2010 - Rastko - corrected calculation of base and total fares			
  Date: 17 Sep 2010 - Rastko - added support for warning messages				
  ================================================================== 
  -->

	<xsl:output omit-xml-declaration="yes"/>

	<xsl:variable name="PNR">
		<xsl:value-of select="TravelItineraryReadRS/TravelItinerary/ItineraryRef/@ID"/>
	</xsl:variable>

	<xsl:template match="/">
		<OTA_TravelItineraryRS>
			<xsl:attribute name="Version">v03</xsl:attribute>
			<xsl:attribute name="AltLangID">Sabre</xsl:attribute>
			<xsl:if test="TravelItineraryReadRS/EchoToken!=''">
				<xsl:attribute name="EchoToken">
					<xsl:value-of select="TravelItineraryReadRS/EchoToken"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="TravelItineraryReadRS"/>
			<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</xsl:if>
			<xsl:if test="PNRReply">
				<xsl:if test="PNRReply/EchoToken!=''">
					<xsl:attribute name="EchoToken">
						<xsl:value-of select="PNRReply/EchoToken"/>
					</xsl:attribute>
				</xsl:if>
				<Errors>
					<xsl:for-each select="PNRReply/Error">
						<Error>
							<xsl:attribute name="Type">
								<xsl:value-of select="@Type"/>
							</xsl:attribute>
							<xsl:value-of select="."/>
						</Error>
					</xsl:for-each>
				</Errors>
			</xsl:if>
			<xsl:if test="EndTransactionRS/Errors">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:value-of select="EndTransactionRS/Errors/Error"/>
					</Error>
				</Errors>
			</xsl:if>
			<xsl:if test="TravelItineraryReadRS/ConversationID!=''">
				<ConversationID>
					<xsl:value-of select="TravelItineraryReadRS/ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<!--************************************************************************************-->
	<!--			                                                                        -->
	<!--************************************************************************************-->
	<xsl:template match="TravelItineraryReadRS">
		<xsl:if test="Ticketed">
			<xsl:attribute name="TransactionStatusCode">Ticketed</xsl:attribute>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="Errors/Error != ''">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">
							<xsl:choose>
								<xsl:when test="Errors/Error/@ErrorCode!= ''">
									<xsl:value-of select="Errors/Error/@ErrorCode"/>
								</xsl:when>
								<xsl:otherwise>E</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="Errors/Error"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="ApplicationResults/@status='NotProcessed'">
				<Errors>
					<Error>
						<xsl:choose>
							<xsl:when test="ApplicationResults/Error/SystemSpecificResults/ShortText">
								<xsl:attribute name="Type">
									<xsl:value-of select="ApplicationResults/Error/@type"/>
								</xsl:attribute>
								<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/ShortText"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Type">
									<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message/@code"/>
								</xsl:attribute>
								<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message"/>
							</xsl:otherwise>
						</xsl:choose>
					</Error>
				</Errors>
			</xsl:when>

			<!--
	   <xsl:when test="not(TravelItinerary/ItineraryRef) and not(Errors/Error)">			
						<Errors>
								<Error>
									<xsl:attribute name="Type">Sabre</xsl:attribute>
									<xsl:attribute name="Code">E</xsl:attribute>
									<xsl:text>INVALID INPUT FILE</xsl:text>
								</Error>
						</Errors>
		</xsl:when>
	  -->
			<xsl:otherwise>
				<Success/>
				<xsl:choose>
					<xsl:when test="Warning!=''">
						<Warnings>
							<xsl:apply-templates select="Warning"/>
						</Warnings>
					</xsl:when>
					<xsl:when test="ApplicationResults/@status='Complete'">
						<xsl:if test="ApplicationResults/Error">
							<Warnings>
								<Warning>
									<xsl:choose>
										<xsl:when test="ApplicationResults/Error/SystemSpecificResults/ShortText">
											<xsl:attribute name="Type">
												<xsl:value-of select="ApplicationResults/Error/@type"/>
											</xsl:attribute>
											<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/ShortText"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Type">
												<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message/@code"/>
											</xsl:attribute>
											<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message"/>
										</xsl:otherwise>
									</xsl:choose>
								</Warning>
							</Warnings>
						</xsl:if>
					</xsl:when>
				</xsl:choose>
				<TravelItinerary>
					<ItineraryRef>
						<xsl:attribute name="Type">PNR</xsl:attribute>
						<xsl:attribute name="ID">
							<xsl:value-of select="TravelItinerary/ItineraryRef/@ID"/>
						</xsl:attribute>

						<xsl:attribute name="ID_Context">
							<xsl:value-of select="TravelItinerary/ItineraryRef/Source/@PseudoCityCode"/>
						</xsl:attribute>
						<xsl:if test="TravelItinerary/ItineraryRef/Source/@AAA_PseudoCityCode">
							<CompanyName>
								<xsl:attribute name="Code">
									<xsl:choose>
										<xsl:when test="TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[@RPH=1]/MiscInformation/SignatureLine/Text">
											<xsl:value-of select="concat(substring-before(TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[@RPH=1]/MiscInformation/SignatureLine/Text, ' '), '|',  TravelItinerary/ItineraryRef/Source/@CreateDateTime)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(TravelItinerary/ItineraryRef/Source/@HomePseudoCityCode, '|',  TravelItinerary/ItineraryRef/Source/@CreateDateTime)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="CodeContext">
									<xsl:choose>
										<xsl:when test="TravelItinerary/ItineraryRef/OfficeStationCode">
											<xsl:value-of select="TravelItinerary/ItineraryRef/OfficeStationCode"/>
										</xsl:when>
										<xsl:otherwise>IATACode</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:choose>
									<xsl:when test="PNR_HDK/Line">
										<xsl:value-of select="concat(PNR_HDK/Line[position()=last()]/@PCC, '/', TravelItinerary/ItineraryRef/Source/@CreationAgent)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(TravelItinerary/ItineraryRef/Source/@HomePseudoCityCode, '/', TravelItinerary/ItineraryRef/Source/@CreationAgent)"/>
									</xsl:otherwise>
								</xsl:choose>
							</CompanyName>
						</xsl:if>
					</ItineraryRef>
					<xsl:choose>
						<xsl:when test="../../ItineraryInfo/ReservationItems/Item/FlightSegment/@Status!='HK'">
							<xsl:apply-templates select="TravelItinerary" >
								<xsl:with-param name="paramSegMode">woPQ</xsl:with-param>
							</xsl:apply-templates>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="TravelItinerary">
								<xsl:with-param name="paramSegMode">wPQ</xsl:with-param>
							</xsl:apply-templates>
						</xsl:otherwise>
					</xsl:choose>
				</TravelItinerary>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Warning">
		<Warning>
			<xsl:value-of select="normalize-space(translate(.,'Â',''))"/>
		</Warning>
	</xsl:template>

	<!--************************************************************************************-->
	<!--			TravelItinerary detail Information                                      -->
	<!--************************************************************************************-->
	<xsl:template match="TravelItinerary">
		<!--******************************************************-->
		<!--				Names                                      -->
		<!--******************************************************-->
		<xsl:param name="paramSegMode"/>
		<CustomerInfos>
			<xsl:variable name="pd">
				<xsl:if test="../SabreCommandLLSRS/Response">
					<xsl:value-of select="../SabreCommandLLSRS/Response"/>
				</xsl:if>
			</xsl:variable>
			<!--<xsl:variable name="pd">
				<xsl:value-of select="../SabreCommandLLSRS/Response"/>
			</xsl:variable>-->

			<xsl:apply-templates select="CustomerInfo//PersonName">
				<xsl:with-param name="pd">
					<xsl:value-of select="$pd"/>
				</xsl:with-param>
			</xsl:apply-templates>
		</CustomerInfos>
		<!--******************************************************-->
		<!--			Air Itinerary                                      -->
		<!--******************************************************-->
		<ItineraryInfo>
			<xsl:choose>
				<xsl:when test="$paramSegMode='woPQ'">
					<xsl:if test="//ItineraryInfo/ReservationItems/Item/FlightSegment">
						<ReservationItems>
							<!--<xsl:for-each select="//ItineraryInfo/ReservationItems/Item/FlightSegment[not(./@SegmentNumber=preceding::*/@SegmentNumber)]">-->
							<xsl:for-each select="//ItineraryInfo/ReservationItems/Item">
								<!--<Item>
                  <xsl:attribute name="ItinSeqNumber">
                    <xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
                  </xsl:attribute>-->
								<xsl:variable name="ItinSeqNumber">
									<xsl:value-of select="@RPH"/>
								</xsl:variable>
								<xsl:variable name="tFltNum">
									<xsl:value-of select='format-number(FlightSegment/@FlightNumber,"#000")'/>
								</xsl:variable>
								<xsl:variable name="fltNum">
									<xsl:choose>
										<xsl:when test="FlightSegment[@FlightNumber = $tFltNum]">
											<xsl:value-of select='$tFltNum'/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select='format-number(FlightSegment/@FlightNumber,"#0000")'/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:variable>

								<xsl:variable name="segNum">
									<xsl:value-of select='format-number(FlightSegment/@SegmentNumber,"#0000")'/>
								</xsl:variable>

								<xsl:variable name="originLoc">
									<xsl:value-of select="FlightSegment/OriginLocation/@LocationCode"/>
								</xsl:variable>

								<xsl:variable name="sqCount">
									<!--<xsl:value-of select='count(//ItineraryInfo/ReservationItems/Item/FlightSegment[@FlightNumber = $fltNum and @SegmentNumber = $segNum])'/>-->
									<xsl:value-of select='count(FlightSegment)'/>
								</xsl:variable>
								<!--<Air>-->
								<xsl:choose>
									<xsl:when test="$sqCount &lt; 2">
										<xsl:call-template name="FillAirSegment">
											<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
											<xsl:with-param name="parmSegNumber" select='$segNum'/>
											<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:apply-templates select='.'  mode="FillAirSegmentStopOver">
											<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
											<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
											<xsl:with-param name="parmSegmentNumber" select='$ItinSeqNumber'/>
										</xsl:apply-templates>
									</xsl:otherwise>
								</xsl:choose>
								<!--</Air>-->
								<!--</xsl:when>-->
								<!--</Item>-->
							</xsl:for-each>
							<xsl:choose>
								<xsl:when test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount">
									<ItemPricing>
										<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
									</ItemPricing>
								</xsl:when>
								<xsl:when test="ItineraryInfo/ItineraryPricing!=''">
									<ItemPricing>
										<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
									</ItemPricing>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment" mode ="Air">
										<xsl:sort data-type="number" order="ascending" select="@SegmentNumber"/>
									</xsl:apply-templates>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Vehicle" mode="Car"/>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Hotel" mode="Hotel"/>
							<xsl:if test="ItineraryInfo/ReservationItems/Item/TPA_Extensions/Line[@Type='OTH']">
								<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/TPA_Extensions" mode="Other"/>
							</xsl:if>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Misc" mode="Misc"/>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/MiscSegment" mode="Misc2"/>

							<xsl:if test="ItineraryInfo/ItineraryPricing!=''">
								<ItemPricing>
									<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
								</ItemPricing>
							</xsl:if>
						</ReservationItems>
					</xsl:if>
				</xsl:when>
				<xsl:when test="$paramSegMode='wPQ'">
					<xsl:if test="ItineraryInfo">
						<ReservationItems>
							<xsl:choose>
								<xsl:when test="ItineraryInfo/ReservationItems/Item/FlightSegment">

									<xsl:choose>
										<xsl:when test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo]">
											<!-- We don't need it now, BUT it may help us later 
                      <xsl:variable name="hasARNK" select="count(ItineraryInfo/ItineraryPricing/PriceQuote[@RPH=$pqRPH]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment[@SegmentNumber!='0' and FareBasis/@Code[1]='VOID'])" />
                      -->
											<xsl:for-each select="ItineraryInfo/ReservationItems/Item">
												<!--<Item>-->

												<xsl:variable name="ItinSeqNumber">
													<xsl:value-of select="@RPH"/>
												</xsl:variable>
												<xsl:variable name="sqCount">
													<xsl:value-of select='count(FlightSegment)'/>
												</xsl:variable>
												<xsl:variable name="tFltNum">
													<xsl:value-of select='format-number(FlightSegment/@FlightNumber,"#000")'/>
												</xsl:variable>
												<xsl:variable name="fltNum">
													<xsl:choose>
														<xsl:when test="FlightSegment[@FlightNumber = $tFltNum]">
															<xsl:value-of select='$tFltNum'/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select='format-number(@FlightNumber,"#0000")'/>
														</xsl:otherwise>
													</xsl:choose>

												</xsl:variable>
												<xsl:variable name="segNum">
													<xsl:value-of select='format-number(FlightSegment/@SegmentNumber,"#0000")'/>
												</xsl:variable>
												<xsl:variable name="originLoc">
													<xsl:value-of select="FlightSegment/OriginLocation/@LocationCode"/>
												</xsl:variable>

												<!--<Air>-->
												<xsl:choose>
													<xsl:when test="$sqCount &lt; 2">
														<xsl:apply-templates select="." mode="FillAirSegment">
															<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
															<xsl:with-param name="parmSegNumber" select='$ItinSeqNumber'/>
															<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
														</xsl:apply-templates>
													</xsl:when>
													<xsl:otherwise>
														<Item>
															<xsl:attribute name="ItinSeqNumber">
																<xsl:value-of select="format-number(FlightSegment/@SegmentNumber,'#0')"/>
															</xsl:attribute>
															<Air>
																<xsl:apply-templates select='.' mode="FillAirSegmentStopOver">
																	<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
																	<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
																	<xsl:with-param name="parmSegmentNumber" select='$ItinSeqNumber'/>
																	<!--  -->
																</xsl:apply-templates>
															</Air>
														</Item>
													</xsl:otherwise>
												</xsl:choose>
												<!--</Air>-->

												<!--</Item>-->
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<!--<xsl:for-each select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus][*//@SegmentNumber!='0'][last()]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment">-->
											<xsl:for-each select="ItineraryInfo/ReservationItems/Item">
												<xsl:sort data-type="number" order="ascending" select="FlightSegment/@SegmentNumber"/>
												<xsl:variable name="ItinSeqNumber">
													<xsl:value-of select="FlightSegment/@SegmentNumber"/>
												</xsl:variable>
												<xsl:if test="FlightSegment/FareBasis/@Code">
													<xsl:choose>
														<xsl:when test="FlightSegment/FareBasis/@Code = 'VOID'">
															<Item>
																<xsl:attribute name="ItinSeqNumber">
																	<xsl:value-of select="format-number(FlightSegment/@SegmentNumber,'#0')"/>
																</xsl:attribute>
																<TPA_Extensions>
																	<Arnk/>
																</TPA_Extensions>
															</Item>
														</xsl:when>
														<xsl:when test="FlightSegment/FareBasis/@Code">
															<xsl:variable name="tFltNum">
																<xsl:value-of select='format-number(FlightSegment/@FlightNumber,"#000")'/>
															</xsl:variable>
															<xsl:variable name="fltNum">
																<xsl:choose>
																	<xsl:when test="FlightSegment[@FlightNumber = $tFltNum]">
																		<xsl:value-of select='$tFltNum'/>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select='format-number(FlightSegment/@FlightNumber,"#0000")'/>
																	</xsl:otherwise>
																</xsl:choose>

															</xsl:variable>

															<xsl:variable name="segNum">
																<xsl:value-of select='format-number(FlightSegment/@SegmentNumber,"#0000")'/>
															</xsl:variable>

															<xsl:variable name="originLoc">
																<xsl:value-of select="FlightSegment/OriginLocation/@LocationCode"/>
															</xsl:variable>

															<xsl:variable name="sqCount">
																<xsl:value-of select='count(FlightSegment)'/>
															</xsl:variable>
															<!--<Air>-->
															<xsl:choose>
																<xsl:when test="$sqCount &lt; 2">
																	<xsl:call-template name="FillAirSegment">
																		<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
																		<xsl:with-param name="parmSegNumber" select='$segNum'/>
																		<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
																	</xsl:call-template>
																</xsl:when>
																<xsl:otherwise>
																	<Item>
																		<xsl:attribute name="ItinSeqNumber">
																			<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
																		</xsl:attribute>
																		<Air>
																			<xsl:apply-templates select='.' mode="FillAirSegmentStopOver">
																				<xsl:with-param name="parmFlightNumber" select='$fltNum'/>
																				<xsl:with-param name="parmOriginLocationCode" select="OriginLocation/@LocationCode[1]"/>
																				<xsl:with-param name="parmSegmentNumber" select='$ItinSeqNumber'/>
																			</xsl:apply-templates>
																		</Air>
																	</Item>
																</xsl:otherwise>
															</xsl:choose>
															<!--</Air>-->
														</xsl:when>
														<xsl:otherwise>
														</xsl:otherwise>
													</xsl:choose>
													<!--</Item>-->
												</xsl:if>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/FlightSegment | ItineraryInfo/ReservationItems/Item/Arunk" mode ="Air">
										<xsl:sort data-type="number" order="ascending" select="@SegmentNumber"/>
									</xsl:apply-templates>
								</xsl:otherwise>
							</xsl:choose>
							<!--
          <xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo]/MiscInformation/SignatureLine[@PQR_Ind='Y']">
            <ItemPricing>
              <xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][MiscInformation/SignatureLine/@PQR_Ind='Y']/PricedItinerary" mode="Exch">
              </xsl:apply-templates>
            </ItemPricing>
          </xsl:if>
          -->

							<!-- 
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/FlightSegment | ItineraryInfo/ReservationItems/Item/Arunk" mode="Air">
            <xsl:sort data-type="number" order="ascending" select="@SegmentNumber"/>
          </xsl:apply-templates>
         -->

							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Vehicle" mode="Car"/>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Hotel" mode="Hotel"/>
							<xsl:if test="ItineraryInfo/ReservationItems/Item/TPA_Extensions/Line[@Type='OTH']">
								<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/TPA_Extensions" mode="Other"/>
							</xsl:if>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Misc" mode="Misc"/>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/MiscSegment" mode="Misc2"/>

							<!--
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/General" mode="Other" />          
          <xsl:if test="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo">
						<ItemPricing>
							<xsl:apply-templates select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo" mode="Air" />
						</ItemPricing>
					</xsl:if>          
          <xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount">

          <xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount">
            <ItemPricing>
              <xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
            </ItemPricing>
          </xsl:if>
          -->
							<xsl:variable name="fPAX" select="ItineraryInfo/ItineraryPricing/PriceQuote[position()=1]/PriceQuotePlus/PassengerInfo/PassengerData/@NameNumber" />
							<xsl:variable name="multiPQ" >
								<xsl:for-each select="ItineraryInfo/ItineraryPricing/PriceQuote[MiscInformation/SignatureLine/@Status='ACTIVE']">
									<xsl:variable name="pax" select="PriceQuotePlus/PassengerInfo/PassengerData/@NameNumber"/>
									<xsl:if test="$fPAX=$pax and position()!=1">
										<xsl:text>yes </xsl:text>
									</xsl:if>
								</xsl:for-each>
							</xsl:variable>

							<xsl:if test="ItineraryInfo/ItineraryPricing!=''">
								<xsl:choose>

									<xsl:when test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount">
										<ItemPricing>
											<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
										</ItemPricing>
									</xsl:when>
									<xsl:when test="ItineraryInfo/ItineraryPricing!=''">
										<ItemPricing>
											<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
										</ItemPricing>
									</xsl:when>
									<xsl:when test="count(ItineraryInfo/ItineraryPricing/PriceQuote) > 1 and contains($multiPQ,'yes')">
										<xsl:for-each select="ItineraryInfo/ItineraryPricing/PriceQuote[MiscInformation/SignatureLine/@Status='ACTIVE']">
											<xsl:variable name="pqN" select="@RPH"/>
											<xsl:if test="../../../../DisplayPriceQuoteRS/PriceQuote[@RPH=$pqN]">
												<ItemPricing>
													<xsl:apply-templates select="." mode="Fare"/>
												</ItemPricing>
											</xsl:if>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<ItemPricing>
											<xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
										</ItemPricing>
									</xsl:otherwise>


								</xsl:choose>
							</xsl:if>

						</ReservationItems>
					</xsl:if>
				</xsl:when>
			</xsl:choose>
			<xsl:apply-templates select="ItineraryInfo/Ticketing[@TicketTimeLimit!='']" mode="Ticketing"/>
			<!--<xsl:if test="SpecialServiceInfo/Service/@SSR_Code!= ''">-->
			<SpecialRequestDetails>
				<xsl:if test="SpecialServiceInfo/Service/@SSR_Code= 'OSI'">
					<OtherServiceInformations>
						<xsl:apply-templates select="SpecialServiceInfo/Service[@SSR_Code= 'OSI']" mode="OSI"/>
					</OtherServiceInformations>
				</xsl:if>

				<xsl:if test="SpecialServiceInfo/Service/@SSR_Code!= 'OSI'">
					<SpecialServiceRequests>
						<xsl:apply-templates select="SpecialServiceInfo/Service[@SSR_Code= 'SSR']" mode="SSR"/>
					</SpecialServiceRequests>
				</xsl:if>

				<xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/TourCode/Text or ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='SYSTEM_ENDORSEMENT']">
					<SpecialRemarks>

						<!--              
              <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RI')]" mode="InvoiceItinRemark"/>
              <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIF']" mode="InvoiceRemark"/>
						  <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIT']" mode="InvoiceRemark"/>
						  <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RIR']" mode="ItinRemark"/>
              <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RC')]" mode="ConfRemark"/>
              <xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE']" mode="Endorsement"/>              
              -->

						<xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo]" mode="TourCode"/>
						<xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo]" mode="Endorsement"/>
						<xsl:if test="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown">
							<xsl:apply-templates select="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown" mode="controllingCarrier"/>
						</xsl:if>
						<!--<xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='DOT_BAGGAGE']">
							<xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='DOT_BAGGAGE'][contains(Text, 'P/') or contains(Text, 'KG/') or contains(Text, 'LB/')]" mode="controllingCarrier"/>
						</xsl:if>-->
					</SpecialRemarks>
				</xsl:if>
				<!--<xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/TourCode/Text or ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='DOT_BAGGAGE']">
					<SpecialRemarks>
						<xsl:if test="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='DOT_BAGGAGE']">
							<xsl:apply-templates select="ItineraryInfo/ItineraryPricing/PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='DOT_BAGGAGE'][contains(Text, 'P/') or contains(Text, 'KG/') or contains(Text, 'LB/')]" mode="controllingCarrier"/>
						</xsl:if>
					</SpecialRemarks>
				</xsl:if>-->
				<xsl:if test="RemarkInfo/Remark[@Type!='Itinerary']">
					<Remarks>
						<xsl:for-each select="RemarkInfo/Remark[@Type!='Itinerary']">
							<xsl:if test="@Type!='Alpha-Coded'">
								<Remark>
									<xsl:attribute name="RPH">
										<xsl:value-of select="format-number(@RPH,'#0')"/>
									</xsl:attribute>
									<xsl:attribute name="Category">
										<xsl:value-of select="@Type"/>
									</xsl:attribute>
									<xsl:choose>
										<xsl:when test="Text!=''">
											<xsl:value-of select="Text"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>.</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</Remark>
							</xsl:if>
						</xsl:for-each>
					</Remarks>
				</xsl:if>

			</SpecialRequestDetails>
			<!--</xsl:if>-->
			<xsl:variable name="dqb">
				<xsl:call-template name="ParseDQBLines">
					<xsl:with-param name="LostText" select="../SabreCommandLLSRS/Response[contains(.,'SALES AUDIT REPORT')]"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:if test="ItineraryInfo/Ticketing[@eTicketNumber!=''] or $dqb!=''">
				<TPA_Extensions>
					<IssuedTickets>
						<xsl:apply-templates select="ItineraryInfo/Ticketing[@eTicketNumber !='']" mode="IssuedTicket"/>
						<xsl:if test="$dqb!=''">
							<xsl:copy-of select="$dqb"/>
						</xsl:if>
					</IssuedTickets>
				</TPA_Extensions>
			</xsl:if>
		</ItineraryInfo>
		<!--******************************************************-->
		<!--			Form of Payment                               -->
		<!--******************************************************-->

		<xsl:choose>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CHECK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CHEQUE'">
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=1]/@RPH,'#0')"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
						</DirectBill>
					</FormOfPayment>
				</TravelCost>
			</xsl:when>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=2] = 'CHECK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CHEQUE'">
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=2]/@RPH,'#0')"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
						</DirectBill>
					</FormOfPayment>
				</TravelCost>
			</xsl:when>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CASH'">
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=1]/@RPH,'#0')"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
						</DirectBill>
					</FormOfPayment>
				</TravelCost>
			</xsl:when>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=2] = 'CASH'">
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=2]/@RPH,'#0')"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
						</DirectBill>
					</FormOfPayment>
				</TravelCost>
			</xsl:when>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form/PaymentCard/@CardType != '' or CustomerInfos/CustomerInfo[1]/Customer[1]/PaymentForm/PaymentCard">
				<TravelCost>
					<xsl:if test="CustomerInfo/PaymentInfo/Payment/Form">
						<FormOfPayment>
							<xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form"/>
						</FormOfPayment>
					</xsl:if>
					<xsl:if test="CustomerInfo/PaymentInfo/Payment/Form/PaymentCard">
						<FormOfPayment>
							<xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form"/>
						</FormOfPayment>
					</xsl:if>
				</TravelCost>
			</xsl:when>
			<xsl:when test="contains(CustomerInfo/PaymentInfo/Payment/Form/Text,'XXXXX')">
				<TravelCost>
					<xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form" mode="Text">
						<xsl:with-param name="accMCO" select="AccountingInfo"></xsl:with-param>
					</xsl:apply-templates>
				</TravelCost>
			</xsl:when>
			<xsl:when test="contains(CustomerInfo/PaymentInfo/Payment/Form/TPA_Extensions/Text,'XXXXX')">
				<xsl:variable name="card">
					<xsl:value-of select="CustomerInfo/PaymentInfo/Payment/Form/TPA_Extensions/Text"/>
				</xsl:variable>
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form/@RPH,'#0')"/>
						</xsl:attribute>
						<PaymentCard>
							<xsl:attribute name="CardCode">
								<xsl:choose>
									<xsl:when test="substring($card,2,2)='CA'">MC</xsl:when>
									<xsl:when test="substring($card,2,2)='DI'">DS</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring($card,2,2)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CardNumber">
								<xsl:value-of select="substring-before(substring($card,4),'Â')"/>
							</xsl:attribute>
							<xsl:attribute name="ExpireDate">XXXX</xsl:attribute>
						</PaymentCard>
					</FormOfPayment>
				</TravelCost>
			</xsl:when>
			<xsl:when test="substring(CustomerInfo/PaymentInfo/Payment/Form[position()=1],1,1) = '*' or substring(CustomerInfo/PaymentInfo/Payment/Form[position()=2],1,1) = '*'">
				<xsl:variable name="card">
					<xsl:choose>
						<xsl:when test="substring(CustomerInfo/PaymentInfo/Payment/Form[position()=2],1,1) = '*'">
							<xsl:value-of select="substring(CustomerInfo/PaymentInfo/Payment/Form[position()=2],2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(CustomerInfo/PaymentInfo/Payment/Form[position()=1],2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:if test="contains($card,'')">
					<TravelCost>
						<FormOfPayment>
							<xsl:attribute name="RPH">
								<xsl:choose>
									<xsl:when test="substring(CustomerInfo/PaymentInfo/Payment/Form[position()=2],1,1) = '*'">
										<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=2]/@RPH,'#0')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=1]/@RPH,'#0')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<PaymentCard>
								<xsl:attribute name="CardCode">
									<xsl:choose>
										<xsl:when test="substring($card,1,2)='CA'">MC</xsl:when>
										<xsl:when test="substring($card,1,2)='DI'">DS</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring($card,1,2)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="CardNumber">
									<xsl:value-of select="substring-before(substring($card,3),'')"/>
								</xsl:attribute>
								<xsl:attribute name="ExpireDate">
									<xsl:value-of select="translate(substring-after($card,''),'/','')"/>
								</xsl:attribute>
							</PaymentCard>
						</FormOfPayment>
					</TravelCost>
				</xsl:if>
			</xsl:when>
			<xsl:when test="CustomerInfo/PaymentInfo/Payment/Form/Text">
				<TravelCost>
					<xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form" mode="Text">
						<xsl:with-param name="accMCO" select="AccountingInfo"></xsl:with-param>
					</xsl:apply-templates>
				</TravelCost>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="RemarkInfo/Remark[@Type='Itinerary']">
			<Remarks>
				<xsl:for-each select="RemarkInfo/Remark[@Type='Itinerary']">
					<xsl:if test="@Type!='Alpha-Coded'">
						<Remark>
							<xsl:attribute name="RPH">
								<xsl:value-of select="format-number(@RPH,'#0')"/>
							</xsl:attribute>
							<xsl:attribute name="Category">
								<xsl:value-of select="@Type"/>
							</xsl:attribute>
							<xsl:choose>
								<xsl:when test="Text!=''">
									<xsl:value-of select="Text"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>.</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</Remark>
					</xsl:if>
				</xsl:for-each>
			</Remarks>
		</xsl:if>
		<UpdatedBy>

			<xsl:attribute name="CreateDateTime">
				<xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,1,4)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,6,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,9,2)"/>
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,12,2)"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,15,2)"/>
				<xsl:text>:00</xsl:text>
			</xsl:attribute>
		</UpdatedBy>
		<xsl:if test="CustomerInfos/CustomerInfo/Customer/TPA_Extensions/CustomerIdentifier/@Identifier!='' or ItineraryRef/@CustomerIdentifier!='' or AccountingInfo or ItineraryInfo/ItineraryPricing/FuturePriceInfo">
			<TPA_Extensions>
				<xsl:if test="CustomerInfos/CustomerInfo/Customer/TPA_Extensions/CustomerIdentifier/@Identifier!='' or ItineraryRef/@CustomerIdentifier!=''">
					<AccountingLine>
						<xsl:choose>
							<xsl:when test="CustomerInfos/CustomerInfo/Customer/TPA_Extensions/CustomerIdentifier/@Identifier!=''">
								<xsl:value-of select="CustomerInfos/CustomerInfo/Customer/TPA_Extensions/CustomerIdentifier/@Identifier"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="ItineraryRef/@CustomerIdentifier"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountingLine>
				</xsl:if>
				<xsl:if test="AccountingInfo">
					<xsl:for-each select="AccountingInfo">
						<AccountingInfo RPH="{position()}">
							<xsl:if test="PersonName">
								<xsl:variable name="paxnum">
									<xsl:value-of select="concat('0',substring(PersonName/@NameNumber,1,2),'0',substring(PersonName/@NameNumber,3))"/>
								</xsl:variable>
								<xsl:attribute name="TravelerRefNumberRPHList">
									<xsl:value-of select="../CustomerInfo/PersonName[@NameNumber=$paxnum]/@RPH"/>
								</xsl:attribute>
							</xsl:if>

							<xsl:if test="@LinkCode!=''">
								<xsl:attribute name="LinkCode">
									<xsl:value-of select="@LinkCode"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="@Type!=''">
								<xsl:attribute name="Type">
									<xsl:value-of select="@Type"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="AccountVendor/@Code!=''">
								<AccountVendor Code="{AccountVendor/@Code}"/>
							</xsl:if>
							<xsl:if test="Airline/@Code!=''">
								<AccountVendor Code="{Airline/@Code}"/>
							</xsl:if>
							<BaseFare>
								<xsl:choose>
									<xsl:when test="BaseFare/@Amount='0.00'">
										<xsl:attribute name="Amount">0</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="'0'"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:when test="contains(BaseFare/@Amount,'.')">
										<xsl:attribute name="Amount">
											<xsl:value-of select="translate(BaseFare/@Amount,'.','')"/>
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="'2'"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:attribute name="Amount">
											<xsl:value-of select="BaseFare/@Amount"/>
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="'0'"/>
										</xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
							</BaseFare>
							<xsl:if test="DocumentInfo/Document/@Number!=''">
								<DocumentInfo Number="{DocumentInfo/Document/@Number}"/>
							</xsl:if>
							<xsl:if test="ChargeCategory!=''">
								<ChargeCategory>
									<xsl:value-of select="ChargeCategory"/>
								</ChargeCategory>
							</xsl:if>
							<xsl:if test="FareApplication!=''">
								<FareApplication>
									<xsl:value-of select="FareApplication"/>
								</FareApplication>
							</xsl:if>
							<PaymentInfo>
								<xsl:if test="PaymentInfo/Commission/@Amount!=''">
									<Commission>
										<xsl:choose>
											<xsl:when test="PaymentInfo/Commission/@Amount='0.00'">
												<xsl:attribute name="Amount">0</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'0'"/>
												</xsl:attribute>
											</xsl:when>
											<xsl:when test="contains(PaymentInfo/Commission/@Amount,'.')">
												<xsl:attribute name="Amount">
													<xsl:value-of select="translate(PaymentInfo/Commission/@Amount,'.','')"/>
												</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'2'"/>
												</xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="Amount">
													<xsl:value-of select="PaymentInfo/Commission/@Amount"/>
												</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'0'"/>
												</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
									</Commission>
								</xsl:if>
								<FormOfPayment>
									<xsl:choose>
										<xsl:when test="PaymentInfo/Payment/CC_Info">
											<PaymentCard>

												<xsl:variable name="card">
													<xsl:choose>
														<xsl:when test="contains(PaymentInfo/Payment/CC_Info/PaymentCard/@Number, 'XXXX')">

															<xsl:variable name="cardID" select="substring(translate(PaymentInfo/Payment/CC_Info/PaymentCard/@Number, 'X',''), 2)">
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="../../PNR_HDK_FOP[contains(text(), $cardID)]">
																	<xsl:variable name="hFOP" select="../../PNR_HDK_FOP[contains(text(), $cardID)]"  />
																	<xsl:value-of select="concat('*', $hFOP/@CCType, $hFOP/text(),'?', substring($hFOP/@Exp,1,2), '/', substring($hFOP/@Exp,3,2))"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="PaymentInfo/Payment/CC_Info/PaymentCard/@Number"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="PaymentInfo/Payment/CC_Info/PaymentCard/@Number"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>

												<xsl:attribute name="CardCode">
													<xsl:choose>
														<xsl:when test="PaymentInfo/Payment/CC_Info/PaymentCard/@Code='CA'">MC</xsl:when>
														<xsl:when test="PaymentInfo/Payment/CC_Info/PaymentCard/@Code='DI'">DS</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="PaymentInfo/Payment/CC_Info/PaymentCard/@Code"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:attribute>
												
												<xsl:attribute name="CardNumber">
													<xsl:value-of select="$card"/>
												</xsl:attribute>
											
											</PaymentCard>
										</xsl:when>
										<xsl:otherwise>
											<DirectBill DirectBill_ID="CHECK"/>
										</xsl:otherwise>
									</xsl:choose>
								</FormOfPayment>
							</PaymentInfo>

							<Taxes>
								<xsl:for-each select="Taxes/Tax">
									<Tax>
										<xsl:choose>
											<xsl:when test="@Amount='0.00'">
												<xsl:attribute name="Amount">0</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'0'"/>
												</xsl:attribute>
											</xsl:when>
											<xsl:when test="contains(@Amount,'.')">
												<xsl:attribute name="Amount">
													<xsl:value-of select="translate(@Amount,'.','')"/>
												</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'2'"/>
												</xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="Amount">
													<xsl:value-of select="@Amount"/>
												</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="'0'"/>
												</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
									</Tax>
								</xsl:for-each>
							</Taxes>
							<xsl:if test="TicketingInfo">
								<TicketingInfo>
									<xsl:if test="TicketingInfo/Exchange">
										<xsl:attribute name="ExchangeInd">
											<xsl:value-of select="TicketingInfo/Exchange/@Ind"/>
										</xsl:attribute>
									</xsl:if>
									<xsl:if test="TicketingInfo/OriginalInvoice">
										<xsl:attribute name="ExchangeInd">
											<xsl:value-of select="TicketingInfo/OriginalInvoice/@Number"/>
										</xsl:attribute>
									</xsl:if>
									<xsl:if test="TicketingInfo/TarriffBasis">
										<xsl:attribute name="TarriffBasis">
											<xsl:value-of select="TicketingInfo/TarriffBasis"/>
										</xsl:attribute>
									</xsl:if>
								</TicketingInfo>
							</xsl:if>
						</AccountingInfo>
					</xsl:for-each>
				</xsl:if>
				<xsl:for-each select="ItineraryInfo/ItineraryPricing/FuturePriceInfo">
					<FuturePriceInfo RPH="{format-number(@RPH,'#0')}">
						<xsl:value-of select="Text"/>
					</FuturePriceInfo>
				</xsl:for-each>
			</TPA_Extensions>
		</xsl:if>
	</xsl:template>

	<!--************************************************************************************-->
	<!--			PNR Retrieve Errors                                           	        -->
	<!--************************************************************************************-->
	<xsl:template match="Err">
		<Error>
			<xsl:attribute name="Type">General</xsl:attribute>
			<xsl:value-of select="Text"/>
		</Error>
	</xsl:template>
	<xsl:template match="Error" mode="error">
		<Error>
			<xsl:value-of select="ErrorInfo/Message"/>
			<xsl:value-of select="SystemSpecificResults/Message"/>
		</Error>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- Issued Tickets Elements 	                               		-->
	<!-- ************************************************************** -->
	<xsl:template match="Ticketing" mode="IssuedTicket">

		<xsl:if test="@RPH!=''">
			<xsl:variable name="elems">
				<xsl:call-template name="tokenizeString">
					<xsl:with-param name="list" select="@eTicketNumber"/>
					<xsl:with-param name="delimiter" select="' '"/>
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="tkn">
				<xsl:choose>
					<xsl:when  test="contains(substring-before(msxsl:node-set($elems)/elem[2], '-'), '/')">
						<xsl:value-of select="substring-before(substring-before(msxsl:node-set($elems)/elem[2], '-'), '/')" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before(msxsl:node-set($elems)/elem[2], '-')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="subtkt">
				<xsl:choose>
					<xsl:when  test="contains(substring-before(msxsl:node-set($elems)/elem[2], '-'), '/')">
						<xsl:variable name="lst" select="substring-after(substring-before(msxsl:node-set($elems)/elem[2], '-'), '/')" />
						<xsl:value-of select="concat(substring( $tkn, 1, string-length($tkn) - string-length($lst)), $lst)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$tkn"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="mco">
				<xsl:choose>
					<xsl:when test="../../AccountingInfo/DocumentInfo/Document[@Number=substring($tkn,4)] and ../../AccountingInfo/TicketingInfo/OriginalTicketNumber=$tkn">MCO</xsl:when>
					<xsl:when test="substring($tkn, 1,3) = 890">MCO</xsl:when>
					<xsl:when test="substring(@eTicketNumber, 1,2) = 'TK'">MCO</xsl:when>
					<xsl:otherwise>EX</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:choose>
				<xsl:when test="$mco='MCO'">
					<ExchangeDocument>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:variable name="rph">
								<xsl:value-of select="format-number(@RPH,'000')-1"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="PersonName/@NameNumber">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="../../AccountingInfo[DocumentInfo/Document/@Number=substring($tkn,4)]/PersonName/@NameNumber">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="../../AccountingInfo[DocumentInfo/Document/@Number=substring($tkn,4)]/PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$rph"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="concat('MCO', substring(@eTicketNumber,3))"/>
					</ExchangeDocument>
				</xsl:when>
				<xsl:when test="../../AccountingInfo/TicketingInfo/OriginalTicketNumber = $tkn">
					<ExchangeDocument>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:variable name="rph">
								<xsl:value-of select="format-number(@RPH,'000')-1"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="PersonName/@NameNumber">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="../../AccountingInfo/DocumentInfo/Document[@Number=substring($tkn,4)]">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="../../AccountingInfo[DocumentInfo/Document/@Number=substring($tkn,4)]/PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(@RPH,'0')-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="concat('EX', substring(@eTicketNumber,3))"/>
					</ExchangeDocument>
				</xsl:when>
				<xsl:when test="//AccountingInfo/Text[contains(text(),$tkn)]">
					<ExchangeDocument>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:variable name="rph">
								<xsl:value-of select="format-number(@RPH,'000')-1"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="PersonName/@NameNumber">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="../../AccountingInfo/DocumentInfo/Document[@Number=substring($tkn,4)]">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="../../AccountingInfo[DocumentInfo/Document/@Number=substring($tkn,4)]/PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(@RPH,'0')-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="concat('EX', substring(@eTicketNumber,3))"/>
					</ExchangeDocument>
				</xsl:when>
				<xsl:otherwise>
					<IssuedTicket>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:variable name="rph">
								<xsl:value-of select="format-number(@RPH,'000')-1"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="../../SpecialServiceInfo/Service[@RPH=$rph]">
									<xsl:variable name="paxRPH" select="../../SpecialServiceInfo/Service[@RPH=$rph]/PersonName/@NameNumber" />
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="PersonName/@NameNumbe">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="PersonName/@NameNumbe"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="../../AccountingInfo/DocumentInfo/Document[@Number=substring($tkn,4)]">
									<xsl:variable name="paxRPH">
										<xsl:call-template name="rphFormat">
											<xsl:with-param name="rn" select="../../AccountingInfo[DocumentInfo/Document/@Number=substring($tkn,4)]/PersonName/@NameNumber"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:when test="../../SpecialServiceInfo/Service[contains(Text, $tkn)]">
									<xsl:variable name="paxRPH" select="../../SpecialServiceInfo/Service[contains(Text, $tkn)]/PersonName/@NameNumber" />
									<xsl:value-of select="../../CustomerInfo/PersonName[@NameNumber=$paxRPH]/@RPH"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(@RPH,'0')-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>

						<xsl:if test="string-length($tkn) > 0">
							<xsl:variable name="fltRPH">
								<xsl:for-each select="../../SpecialServiceInfo/Service[(contains(Text, $tkn) or contains(Text, $subtkt))and @SSR_Type='TKNE']">
									<!-- Get City Pair  -->
									<xsl:variable name="cityPair" select="substring(substring-after(Text, 'HK1 '), 1, 6)" />
									<xsl:variable name="from" select="substring($cityPair, 1, 3)" />
									<xsl:variable name="to" select="substring($cityPair, 4, 3)" />
									<xsl:variable name="flightNum" select="format-number(../../ItineraryInfo/ReservationItems/Item[FlightSegment/OriginLocation/@LocationCode=$from and FlightSegment/DestinationLocation/@LocationCode=$to]/FlightSegment/@FlightNumber, '#0')" />

									<!--
                  <xsl:variable name="fltRPH" select="../../../ItineraryInfo/ReservationItems/Item[FlightSegment/OriginLocation/@LocationCode=$from and FlightSegment/DestinationLocation/@LocationCode=$to]/@RPH" />
                   <xsl:variable name="fltRPH" select="format-number(../../../FareFamily/PriceQuoteInfo/Details/SegmentInfo[Flight/Departure/CityCode=$from and Flight/MarketingFlight/@number=$flightNum]/@number, '#0')"/>
                  -->


									<xsl:variable name="itemNum">
										<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
									</xsl:variable>
									<xsl:variable name="sqCount">
										<xsl:value-of select="count(../../ItineraryInfo/ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flightNum,'#0000')])"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$sqCount > 1">
											<xsl:value-of select="format-number(../../ItineraryInfo/ReservationItems/Item[@RPH=$itemNum]/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flightNum,'#0000')]/@SegmentNumber[not(../preceding-sibling::FlightSegment/@SegmentNumber = .)],'#0')"/>
										</xsl:when>
										<xsl:when test="../../ItineraryInfo/ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flightNum,'#0000')]">
											<xsl:value-of select="format-number(../../ItineraryInfo/ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flightNum,'#0000')]/@SegmentNumber[not(../preceding-sibling::FlightSegment/@SegmentNumber = .)],'#0')"/>
											<xsl:if test="FareBasis/@Code='VOID'">
												<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
											</xsl:if>
										</xsl:when>
										<xsl:when test="FareBasis/@Code='VOID'">
											<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
										</xsl:when>
									</xsl:choose>
									<!--
                  <xsl:variable name="fltRPH" select="format-number(../../../DisplayPriceQuoteRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment[OriginLocation/@LocationCode=$from and @FlightNumber=$flightNum]/@RPH, '#0')"/>
                  -->
									<xsl:text> </xsl:text>
								</xsl:for-each>
							</xsl:variable>

							<xsl:if test="string-length($fltRPH) > 0">
								<xsl:attribute name="FlightRefNumberRPHList">
									<xsl:call-template name="string-trim">
										<xsl:with-param name="string" select="$fltRPH" />
									</xsl:call-template>
								</xsl:attribute>
							</xsl:if>

						</xsl:if>

						<xsl:variable name="tkt">
							<xsl:choose>
								<xsl:when test="../../ItineraryInfo/Ticketing[contains(@eTicketNumber,$tkn) and contains(@eTicketNumber,'VOID')]/@eTicketNumber">
									<xsl:value-of select="../../ItineraryInfo/Ticketing[contains(@eTicketNumber,$tkn) and contains(@eTicketNumber,'VOID')]/@eTicketNumber"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@eTicketNumber"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="starts-with($tkt, 'MV')">
								<xsl:value-of select="concat('EMD', ' ', $tkt)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$tkt"/>
							</xsl:otherwise>
						</xsl:choose>

					</IssuedTicket>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- Issued Tickets from DQB lines                                  -->
	<!-- ************************************************************** -->
	<xsl:template name="ParseDQBLines">
		<xsl:param name="LostText"/>
		<xsl:choose>
			<xsl:when test="substring-after($LostText,$PNR)!='' and $PNR != ''">
				<xsl:variable name="lpnr">
					<xsl:value-of select="translate(substring-before(substring-after($LostText,$PNR),'PNR-'), '&#x9;&#xa;&#xd;', '')"/>
				</xsl:variable>
				<xsl:variable name="ls">
					<xsl:value-of select="translate(substring-before($lpnr,'    F '), '&#x9;&#xa;&#xd;', '')"/>
				</xsl:variable>
				<xsl:variable name="tn">
					<xsl:value-of select="translate(substring($ls,string-length($ls)-15),' ','')"/>
				</xsl:variable>
				<xsl:variable name="tn1">
					<xsl:value-of select="translate(translate(substring($ls,string-length($ls)-15),' ',''),'-','/')"/>
				</xsl:variable>
				<xsl:variable name="ca">
					<xsl:value-of select="translate(translate(substring-after(substring-before(substring-after($LostText,$PNR),'TICKET AMOUNT - '), 'COMMISSION AMOUNT-'), '&#x9;&#xa;&#xd;', ''), '&#09;&#10;&#13; ','')"/>
				</xsl:variable>
				<xsl:variable name="ta">
					<xsl:value-of select="translate(translate(substring-before(substring-after(substring-after($LostText,$PNR),'TICKET AMOUNT - '), $tn), '&#x9;&#xa;&#xd;', ''), '&#09;&#10;&#13; ','')"/>
				</xsl:variable>

				<xsl:variable name="mcoLine">
					<xsl:value-of select="translate(substring-before($lpnr,'    E '), '&#x9;&#xa;&#xd;', '')"/>
				</xsl:variable>
				<xsl:variable name="mco">
					<xsl:value-of select="translate(substring($mcoLine,string-length($mcoLine)-15),' ','')"/>
				</xsl:variable>
				<xsl:variable name="mcoa">
					<xsl:value-of select="translate(translate(substring-before(substring-after($mcoLine,'TICKET AMOUNT - '), $mco), '&#x9;&#xa;&#xd;', ''), '&#09;&#10;&#13; ','')"/>
				</xsl:variable>

				<xsl:variable name="pax">
					<xsl:value-of select="translate(translate(substring-before(substring-after($LostText,$PNR),'AGT SINE-'), '&#x9;&#xa;&#xd;', ''), '&#09;&#10;&#13; ','')"/>
				</xsl:variable>

				<xsl:if test="count(ItineraryInfo/Ticketing[contains(@eTicketNumber,$tn) and @RPH!='']) = 0 and count(ItineraryInfo/Ticketing[contains(@eTicketNumber,$tn1) and @RPH!='']) = 0">
					<IssuedTicket>
						TE <xsl:value-of select="concat($tn,' *DQB*')"/> CA-<xsl:value-of select="$ca"/> TA-<xsl:value-of select="$ta"/> P-<xsl:value-of select="$pax"/>
					</IssuedTicket>
				</xsl:if>

				<xsl:if test="$mco != ''">
					<IssuedTicket>
						TE <xsl:value-of select="concat($mco,' *DQB*MCO*')"/> CA-<xsl:value-of select="$ca"/> TA-<xsl:value-of select="$mcoa"/> P-<xsl:value-of select="$pax"/>
					</IssuedTicket>
				</xsl:if>

				<xsl:call-template name="ParseDQBLines">
					<xsl:with-param name="LostText" select="substring-after($LostText,$PNR)"/>
				</xsl:call-template>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!--************************************************************************************-->
	<!--						 Passenger Information         		                        -->
	<!--************************************************************************************-->
	<xsl:template match="PersonName">
		<xsl:param name="pd"/>

		<xsl:variable name="paxref">
			<xsl:value-of select="concat(' ',translate(@NameNumber,'0',''),' ')"/>
		</xsl:variable>

		<xsl:variable name="paxRPH">
			<xsl:value-of select="@RPH"/>
		</xsl:variable>

		<xsl:variable name="vDigits" select="'0123456789'"/>
		<xsl:variable name="alpha" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
		<xsl:variable name="paxtype">
			<xsl:choose>
				<xsl:when test="$pd != ''">
					<xsl:choose>
						<xsl:when test="translate(substring(substring-after($pd,$paxref),8,3),(translate(substring(substring-after($pd,$paxref),9,3), $alpha,'')), '') = ''">ADT</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(substring-after($pd,$paxref),8,3)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="//DisplayPriceQuoteRS/PriceQuoteSummary/PTC_FareBreakdown/PassengerTypeQuantity[number(@NameNumber)=number($paxref)]/@Code"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="@RPH"/>
			</xsl:attribute>
			<Customer>
				<PersonName>

					<xsl:choose>
						<xsl:when test="@PassengerType!='' and @PassengerType=$paxtype">
							<xsl:attribute name="NameType">
								<xsl:value-of select="@PassengerType"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="@PassengerType!=$paxtype">
							<xsl:attribute name="NameType">
								<xsl:value-of select="$paxtype"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="@Infant='true'">
							<xsl:attribute name="NameType">INF</xsl:attribute>
						</xsl:when>
						<xsl:when test="@WithInfant='true'">
							<xsl:attribute name="NameType">INF</xsl:attribute>
						</xsl:when>
						<!-- this is for parsing *PQS response -->
						<xsl:when test="//DisplayPriceQuoteRS/PriceQuoteSummary/PTC_FareBreakdown/PassengerTypeQuantity[number(@NameNumber)=number($paxref)]/@Code or contains($pd,$paxref)">
							<xsl:attribute name="NameType">
								<xsl:choose>
									<xsl:when test="substring($paxtype,1,1)!=' '">
										<xsl:choose>
											<xsl:when test="substring($paxtype,1,2)='CH'">
												<xsl:value-of select="'CHD'"/>
											</xsl:when>
											<xsl:when test="substring($paxtype,1,1)='C' and string-length(translate($paxtype, $vDigits,'')) &gt; 0">
												<xsl:value-of select="'CHD'"/>
											</xsl:when>
											<xsl:when test="$paxtype='CNN'">
												<xsl:value-of select="'CHD'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$paxtype"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>ADT</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="NameType">ADT</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="Remarks != ''">
						<xsl:attribute name="NameType">
							<xsl:value-of select="substring(string(Remarks),23,3)"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="NamePrefix!=''">
						<NamePrefix>
							<xsl:value-of select="NamePrefix"/>
						</NamePrefix>
					</xsl:if>
					<GivenName>
						<xsl:value-of select="GivenName"/>
					</GivenName>
					<Surname>
						<xsl:value-of select="Surname"/>
					</Surname>
				</PersonName>
				<xsl:apply-templates select="Email"/>
				<xsl:variable name="custref">
					<xsl:value-of select="$paxref"/>
				</xsl:variable>
				<xsl:for-each select="CustLoyalty[@NameNumber=$custref]">
					<CustLoyalty>
						<xsl:attribute name="ProgramID">
							<xsl:value-of select="@ProgramID"/>
						</xsl:attribute>
						<xsl:attribute name="MembershipID">
							<xsl:value-of select="@MembershipID"/>
						</xsl:attribute>
						<xsl:attribute name="RPH">
							<xsl:value-of select="@RPH"/>
						</xsl:attribute>
					</CustLoyalty>
				</xsl:for-each>
				<xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Client Address']"/>
				<xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Delivery']"/>
				<xsl:apply-templates select="../Address"/>
			</Customer>
			<ProfileRefRS>
				<xsl:attribute name="ID">
					<xsl:value-of select="@NameNumber"/>
				</xsl:attribute>
			</ProfileRefRS>
		</CustomerInfo>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			      Air Itinerary Section				                              	            -->
	<!--************************************************************************************-->

	<xsl:template match="Item" mode="FillAirSegment">
		<xsl:param name="parmFlightNumber"/>
		<xsl:param name="parmSegNumber"/>
		<xsl:param name="parmOriginLocationCode"/>
		<xsl:for-each select="FlightSegment">
			<!--  and @SegmentNumber = $parmSegNumber OriginLocation/@LocationCode = $parmOriginLocationCode and-->
			<xsl:if test="@Status !='SC'">
				<Item>
					<xsl:attribute name="ItinSeqNumber">
						<xsl:choose>
							<xsl:when test="$parmSegNumber='0000'">
								<xsl:value-of select="format-number(@SegmentNumber,'#0') + 1 "/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($parmSegNumber,'#0')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<Air>
						<!--<xsl:if test="@FlightNumber = $parmFlightNumber and @SegmentNumber = $parmSegNumber and OriginLocation/@LocationCode = $parmOriginLocationCode and @Status !='SC'">-->
						<xsl:if test="@DepartureDateTime">
							<xsl:attribute name="DepartureDateTime">
								<xsl:value-of select="concat(@DepartureDateTime,':00')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ArrivalDateTime">
							<xsl:attribute name="ArrivalDateTime">
								<xsl:choose>
									<xsl:when test="starts-with(@ArrivalDateTime,'20')">
										<xsl:value-of select="@ArrivalDateTime"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="substring(@DepartureDateTime,6,2) > substring(@ArrivalDateTime,1,2)">
												<xsl:value-of select="substring(@DepartureDateTime,1,4)+1"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:value-of select="':00'"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@StopQuantity != ''">
							<xsl:attribute name="StopQuantity">
								<xsl:value-of select="format-number(@StopQuantity,'#0')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@SegmentNumber">
							<xsl:attribute name="RPH">
								<xsl:choose>
									<xsl:when test="$parmSegNumber='0000'">
										<xsl:value-of select="format-number(@SegmentNumber,'#0') + 1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($parmSegNumber,'#0')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@FlightNumber">
							<xsl:attribute name="FlightNumber">
								<xsl:value-of select="@FlightNumber"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ResBookDesigCode">
							<xsl:attribute name="ResBookDesigCode">
								<xsl:value-of select="@ResBookDesigCode"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@NumberInParty">
							<xsl:attribute name="NumberInParty">
								<xsl:value-of select="translate(@NumberInParty,'0','')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@Status">
							<xsl:attribute name="Status">
								<xsl:value-of select="@Status"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@eTicket">
							<xsl:attribute name="E_TicketEligibility">
								<xsl:choose>
									<xsl:when test="@eTicket= 'true'">Eligible</xsl:when>
									<xsl:otherwise>NotEligible</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>

						<xsl:variable name="orig">
							<xsl:value-of select="OriginLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:variable name="dest">
							<xsl:value-of select="DestinationLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:if test="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd">
							<xsl:attribute name="GI">
								<xsl:value-of select="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd"/>
							</xsl:attribute>
						</xsl:if>

						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="OriginLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="OriginLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="OriginLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DestinationLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="DestinationLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DestinationLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</ArrivalAirport>
						<OperatingAirline>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="OperatingAirline/@Code!=''">
										<xsl:value-of select="OperatingAirline/@Code"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="MarketingAirline/@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</OperatingAirline>
						<xsl:if test="Equipment/@AirEquipType != ''">
							<Equipment>
								<xsl:attribute name="AirEquipType">
									<xsl:value-of select="Equipment/@AirEquipType"/>
								</xsl:attribute>
							</Equipment>
						</xsl:if>
						<MarketingAirline>
							<xsl:attribute name="Code">
								<xsl:value-of select="MarketingAirline/@Code"/>
							</xsl:attribute>
						</MarketingAirline>
						<xsl:if test="MarriageGrp/@Group!=''">
							<MarriageGrp>
								<xsl:value-of select="MarriageGrp/@Group"/>
							</MarriageGrp>
						</xsl:if>
						<TPA_Extensions>
							<xsl:attribute name="ConfirmationNumber">
								<xsl:value-of select="../../../../ItineraryRef/@ID"/>
							</xsl:attribute>
						</TPA_Extensions>
					</Air>
				</Item>
			</xsl:if>
		</xsl:for-each>
		<xsl:if test="Arunk">
			<Item>
				<xsl:attribute name="ItinSeqNumber">
					<xsl:choose>
						<xsl:when test="$parmSegNumber='0000'">
							<xsl:value-of select="format-number(@SegmentNumber,'#0') + 1 "/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="format-number($parmSegNumber,'#0')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<TPA_Extensions>
					<Arnk/>
				</TPA_Extensions>
			</Item>
		</xsl:if>
	</xsl:template>

	<xsl:template name="FillAirSegment">
		<xsl:param name="parmFlightNumber"/>
		<xsl:param name="parmSegNumber"/>
		<xsl:param name="parmOriginLocationCode"/>
		<xsl:for-each select="//ItineraryInfo/ReservationItems/Item/FlightSegment[@FlightNumber = $parmFlightNumber and OriginLocation/@LocationCode=$parmOriginLocationCode]">
			<!--  and @SegmentNumber = $parmSegNumber OriginLocation/@LocationCode = $parmOriginLocationCode and-->
			<xsl:if test="@Status !='SC'">
				<Item>
					<xsl:attribute name="ItinSeqNumber">
						<xsl:choose>
							<xsl:when test="$parmSegNumber='0000'">
								<xsl:value-of select="format-number(@SegmentNumber,'#0') + 1 "/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($parmSegNumber,'#0')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<Air>
						<!--<xsl:if test="@FlightNumber = $parmFlightNumber and @SegmentNumber = $parmSegNumber and OriginLocation/@LocationCode = $parmOriginLocationCode and @Status !='SC'">-->
						<xsl:if test="@DepartureDateTime">
							<xsl:attribute name="DepartureDateTime">
								<xsl:value-of select="concat(@DepartureDateTime,':00')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ArrivalDateTime">
							<xsl:attribute name="ArrivalDateTime">
								<xsl:choose>
									<xsl:when test="starts-with(@ArrivalDateTime,'20')">
										<xsl:value-of select="@ArrivalDateTime"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="substring(@DepartureDateTime,6,2) > substring(@ArrivalDateTime,1,2)">
												<xsl:value-of select="substring(@DepartureDateTime,1,4)+1"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:value-of select="':00'"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@StopQuantity != ''">
							<xsl:attribute name="StopQuantity">
								<xsl:value-of select="format-number(@StopQuantity,'#0')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@SegmentNumber">
							<xsl:attribute name="RPH">
								<xsl:choose>
									<xsl:when test="$parmSegNumber='0000'">
										<xsl:value-of select="format-number(@SegmentNumber,'#0') + 1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($parmSegNumber,'#0')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@FlightNumber">
							<xsl:attribute name="FlightNumber">
								<xsl:value-of select="@FlightNumber"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ResBookDesigCode">
							<xsl:attribute name="ResBookDesigCode">
								<xsl:value-of select="@ResBookDesigCode"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@NumberInParty">
							<xsl:attribute name="NumberInParty">
								<xsl:value-of select="translate(@NumberInParty,'0','')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@Status">
							<xsl:attribute name="Status">
								<xsl:value-of select="@Status"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@eTicket">
							<xsl:attribute name="E_TicketEligibility">
								<xsl:choose>
									<xsl:when test="@eTicket= 'true'">Eligible</xsl:when>
									<xsl:otherwise>NotEligible</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>

						<xsl:variable name="orig">
							<xsl:value-of select="OriginLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:variable name="dest">
							<xsl:value-of select="DestinationLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:if test="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd">
							<xsl:attribute name="GI">
								<xsl:value-of select="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd"/>
							</xsl:attribute>
						</xsl:if>

						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="OriginLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="OriginLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="OriginLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DestinationLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="DestinationLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DestinationLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</ArrivalAirport>
						<OperatingAirline>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="OperatingAirline/@Code!=''">
										<xsl:value-of select="OperatingAirline/@Code"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="MarketingAirline/@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</OperatingAirline>
						<xsl:if test="Equipment/@AirEquipType != ''">
							<Equipment>
								<xsl:attribute name="AirEquipType">
									<xsl:value-of select="Equipment/@AirEquipType"/>
								</xsl:attribute>
							</Equipment>
						</xsl:if>
						<MarketingAirline>
							<xsl:attribute name="Code">
								<xsl:value-of select="MarketingAirline/@Code"/>
							</xsl:attribute>
						</MarketingAirline>
						<xsl:if test="MarriageGrp/@Group!=''">
							<MarriageGrp>
								<xsl:value-of select="MarriageGrp/@Group"/>
							</MarriageGrp>
						</xsl:if>
						<TPA_Extensions>
							<xsl:attribute name="ConfirmationNumber">
								<xsl:value-of select="../../../../ItineraryRef/@ID"/>
							</xsl:attribute>
						</TPA_Extensions>
					</Air>
				</Item>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="Item" mode="FillAirSegmentStopOver">
		<xsl:param name="parmFlightNumber"/>
		<xsl:param name="parmOriginLocationCode"/>
		<xsl:param name="parmSegmentNumber"/>
		<xsl:for-each select="FlightSegment">
			<!-- //ItineraryInfo/ReservationItems/Item/FlightSegment[@FlightNumber = $parmFlightNumber and @SegmentNumber = $parmSegmentNumber] -->
			<xsl:if test="position() = 1">
				<xsl:attribute name="DepartureDateTime">
					<xsl:value-of select="concat(@DepartureDateTime,':00')"/>
				</xsl:attribute>

				<xsl:attribute name="ArrivalDateTime">
					<xsl:choose>
						<xsl:when test="UpdatedArrivalTime">
							<!-- //ItineraryInfo/ReservationItems/Item/FlightSegment[@FlightNumber = $parmFlightNumber and @SegmentNumber = $parmSegmentNumber]/ -->
							<xsl:value-of select="translate(translate((descendant::FlightSegment[@FlightNumber = $parmFlightNumber and @SegmentNumber = $parmSegmentNumber]/@ArrivalDateTime)[last()], '&#x9;&#xa;&#xd;', ''), ' ', '')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="starts-with(@ArrivalDateTime,'20')">
									<xsl:value-of select="@ArrivalDateTime"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="substring(@DepartureDateTime,6,2) > substring(@ArrivalDateTime,1,2)">
											<xsl:value-of select="substring(@DepartureDateTime,1,4)+1"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="@ArrivalDateTime"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="@ArrivalDateTime"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="':00'"/>
				</xsl:attribute>

				<xsl:if test="@StopQuantity != ''">
					<xsl:attribute name="StopQuantity">
						<xsl:value-of select="format-number(@StopQuantity,'#0')"/>
					</xsl:attribute>
				</xsl:if>

				<xsl:attribute name="RPH">
					<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
				</xsl:attribute>

				<xsl:attribute name="FlightNumber">
					<xsl:value-of select="@FlightNumber"/>
				</xsl:attribute>

				<xsl:attribute name="ResBookDesigCode">
					<xsl:value-of select="@ResBookDesigCode"/>
				</xsl:attribute>

				<xsl:attribute name="NumberInParty">
					<xsl:value-of select="translate(@NumberInParty,'0','')"/>
				</xsl:attribute>

				<xsl:attribute name="Status">
					<xsl:value-of select="@Status"/>
				</xsl:attribute>

				<xsl:if test="@eTicket">
					<xsl:attribute name="E_TicketEligibility">
						<xsl:choose>
							<xsl:when test="@eTicket= 'true'">Eligible</xsl:when>
							<xsl:otherwise>NotEligible</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:if>

				<DepartureAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="OriginLocation/@LocationCode"/>
					</xsl:attribute>
					<xsl:if test="OriginLocation/@TerminalCode!=''">
						<xsl:attribute name="Terminal">
							<xsl:value-of select="OriginLocation/@TerminalCode"/>
						</xsl:attribute>
					</xsl:if>
				</DepartureAirport>

				<OperatingAirline>
					<xsl:attribute name="Code">
						<xsl:choose>
							<xsl:when test="OperatingAirline/@Code!=''">
								<xsl:value-of select="OperatingAirline/@Code"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="MarketingAirline/@Code"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</OperatingAirline>

				<xsl:if test="Equipment/@AirEquipType != ''">
					<Equipment>
						<xsl:attribute name="AirEquipType">
							<xsl:value-of select="Equipment/@AirEquipType"/>
						</xsl:attribute>
					</Equipment>
				</xsl:if>

				<MarketingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="MarketingAirline/@Code"/>
					</xsl:attribute>
				</MarketingAirline>

				<xsl:if test="MarriageGrp/@Group!=''">
					<MarriageGrp>
						<xsl:value-of select="MarriageGrp/@Group"/>
					</MarriageGrp>
				</xsl:if>

				<TPA_Extensions>
					<xsl:attribute name="ConfirmationNumber">
						<xsl:value-of select="../../../../ItineraryRef/@ID"/>
					</xsl:attribute>
				</TPA_Extensions>

			</xsl:if>

			<xsl:if test="count(../FlightSegment) > 1">
				<xsl:choose>
					<xsl:when test="position() = 1">
						<StopInfo>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DestinationLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="DestinationLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DestinationLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="ArrivalDateTime">
								<xsl:value-of select="@ArrivalDateTime"/>
							</xsl:attribute>
							<xsl:attribute name="DepartureDateTime">
								<xsl:value-of select="translate(translate(../FlightSegment[last()]/@DepartureDateTime, '&#x9;&#xa;&#xd;', ''), ' ', '')"/>
							</xsl:attribute>
							<xsl:if test="stopDetails[dateQualifier='AD']/equipementType!=''">
								<xsl:attribute name="AirEquipType">
									<xsl:value-of select="stopDetails[dateQualifier='AD']/equipementType"/>
								</xsl:attribute>
							</xsl:if>
						</StopInfo>
					</xsl:when>
					<xsl:otherwise>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DestinationLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="DestinationLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DestinationLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</ArrivalAirport>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>

			<xsl:if test="count(FlightSegment) = 1">
				<xsl:attribute name="ArrivalDateTime">
					<xsl:choose>
						<xsl:when test="starts-with(@ArrivalDateTime,'20')">
							<xsl:value-of select="@ArrivalDateTime"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="substring(@DepartureDateTime,6,2) > substring(@ArrivalDateTime,1,2)">
									<xsl:value-of select="substring(@DepartureDateTime,1,4)+1"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="@ArrivalDateTime"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="@ArrivalDateTime"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="':00'"/>
				</xsl:attribute>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="DestinationLocation/@LocationCode"/>
					</xsl:attribute>
					<xsl:if test="DestinationLocation/@TerminalCode!=''">
						<xsl:attribute name="Terminal">
							<xsl:value-of select="DestinationLocation/@TerminalCode"/>
						</xsl:attribute>
					</xsl:if>
				</ArrivalAirport>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="FlightSegment | Arunk" mode="Air">
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:choose>
					<xsl:when test="@SegmentNumber">
						<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="name(.) = 'FlightSegment' and @SegmentNumber">
					<xsl:if test="@ActionCode = 'GK'">
						<xsl:attribute name="IsPassive">Y</xsl:attribute>
					</xsl:if>
					<Air>
						<xsl:if test="@DepartureDateTime">
							<xsl:attribute name="DepartureDateTime">
								<xsl:value-of select="concat(@DepartureDateTime,':00')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ArrivalDateTime">
							<xsl:attribute name="ArrivalDateTime">
								<xsl:choose>
									<xsl:when test="starts-with(@ArrivalDateTime,'20')">
										<xsl:value-of select="@ArrivalDateTime"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="substring(@DepartureDateTime,6,2) > substring(@ArrivalDateTime,1,2)">
												<xsl:value-of select="substring(@DepartureDateTime,1,4)+1"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="@ArrivalDateTime"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:value-of select="':00'"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@StopQuantity != ''">
							<xsl:attribute name="StopQuantity">
								<xsl:value-of select="format-number(@StopQuantity,'#0')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@SegmentNumber">
							<xsl:attribute name="RPH">
								<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@FlightNumber">
							<xsl:attribute name="FlightNumber">
								<xsl:value-of select="@FlightNumber"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@ResBookDesigCode">
							<xsl:attribute name="ResBookDesigCode">
								<xsl:value-of select="@ResBookDesigCode"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@NumberInParty">
							<xsl:attribute name="NumberInParty">
								<xsl:value-of select="translate(@NumberInParty,'0','')"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@Status">
							<xsl:attribute name="Status">
								<xsl:value-of select="@Status"/>
							</xsl:attribute>
						</xsl:if>

						<xsl:if test="@eTicket">
							<xsl:attribute name="E_TicketEligibility">
								<xsl:choose>
									<xsl:when test="@eTicket= 'true'">Eligible</xsl:when>
									<xsl:otherwise>NotEligible</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>

						<xsl:variable name="orig">
							<xsl:value-of select="OriginLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:variable name="dest">
							<xsl:value-of select="DestinationLocation/@LocationCode"/>
						</xsl:variable>
						<xsl:if test="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd">
							<xsl:attribute name="GI">
								<xsl:value-of select="//OTA_AirPriceRS/PriceQuote/PricedItinerary/AirItineraryPricingInfo/FareCalculationBreakdown[Departure/@AirportCode=$orig and Departure/@ArrivalAirportCode=$dest]/FareBasis/@GlobalInd"/>
							</xsl:attribute>
						</xsl:if>

						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="OriginLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="OriginLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="OriginLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DestinationLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="DestinationLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DestinationLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</ArrivalAirport>
						<OperatingAirline>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="OperatingAirline/@Code!=''">
										<xsl:value-of select="OperatingAirline/@Code"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="MarketingAirline/@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</OperatingAirline>
						<xsl:if test="Equipment/@AirEquipType != ''">
							<Equipment>
								<xsl:attribute name="AirEquipType">
									<xsl:value-of select="Equipment/@AirEquipType"/>
								</xsl:attribute>
							</Equipment>
						</xsl:if>
						<MarketingAirline>
							<xsl:attribute name="Code">
								<xsl:value-of select="MarketingAirline/@Code"/>
							</xsl:attribute>
						</MarketingAirline>
						<xsl:if test="MarriageGrp/@Group!=''">
							<MarriageGrp>
								<xsl:value-of select="MarriageGrp/@Group"/>
							</MarriageGrp>
						</xsl:if>
						<TPA_Extensions>
							<xsl:attribute name="ConfirmationNumber">
								<xsl:value-of select="../../../../ItineraryRef/@ID"/>
							</xsl:attribute>
						</TPA_Extensions>
					</Air>
				</xsl:when>
				<xsl:when test="OriginLocation/@LocationCode !=''">
					<Air>
						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="OriginLocation/@LocationCode"/>
							</xsl:attribute>
							<xsl:if test="OriginLocation/@TerminalCode!=''">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="OriginLocation/@TerminalCode"/>
								</xsl:attribute>
							</xsl:if>
						</DepartureAirport>
					</Air>
				</xsl:when>
				<xsl:otherwise>
					<TPA_Extensions>
						<Arnk/>
					</TPA_Extensions>
				</xsl:otherwise>
			</xsl:choose>
		</Item>
	</xsl:template>

	<xsl:template match="Misc" mode="Misc">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:variable name="curdate">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,1,10)"/>
					</xsl:variable>
					<xsl:variable name="curyear">
						<xsl:value-of select="substring($curdate,1,4)"/>
					</xsl:variable>
					<xsl:variable name="curmonth">
						<xsl:value-of select="substring($curdate,6,2)"/>
					</xsl:variable>
					<xsl:variable name="curday">
						<xsl:value-of select="substring($curdate,9)"/>
					</xsl:variable>
					<xsl:variable name="miscmonth">
						<xsl:value-of select="substring(@DepartureDateTime,1,2)"/>
					</xsl:variable>
					<xsl:variable name="miscday">
						<xsl:value-of select="substring(@DepartureDateTime,4,2)"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$miscmonth > $curmonth">
							<xsl:value-of select="$curyear"/>
							<xsl:value-of select="concat('-',@DepartureDateTime)"/>
						</xsl:when>
						<xsl:when test="$curmonth > $miscmonth">
							<xsl:value-of select="$curyear + 1"/>
							<xsl:value-of select="concat('-',@DepartureDateTime)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="$miscday >= $curday">
									<xsl:value-of select="$curyear"/>
									<xsl:value-of select="concat('-',@DepartureDateTime)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$curyear + 1"/>
									<xsl:value-of select="concat('-',@DepartureDateTime)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<Description>
					<xsl:text>Miscellaneous</xsl:text>
					<xsl:value-of select="concat(' - Board point: ',OriginLocation/@LocationCode)"/>
					<xsl:if test="Text!=''">
						<xsl:value-of select="concat(' - ',Text)"/>
					</xsl:if>
				</Description>
				<TPA_Extensions Status="{@Status}" NumberInParty="{format-number(@NumberInParty,'#0')}">
					<Vendor Code="{Vendor/@Code}"/>
					<OriginCityCode>
						<xsl:value-of select="OriginLocation/@LocationCode"/>
					</OriginCityCode>
				</TPA_Extensions>

			</General>
		</Item>
	</xsl:template>

	<xsl:template match="MiscSegment" mode="Misc2">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:variable name="curdate">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,1,10)"/>
					</xsl:variable>
					<xsl:variable name="curyear">
						<xsl:value-of select="substring($curdate,1,4)"/>
					</xsl:variable>
					<xsl:variable name="curmonth">
						<xsl:value-of select="substring($curdate,6,2)"/>
					</xsl:variable>
					<xsl:variable name="curday">
						<xsl:value-of select="substring($curdate,9)"/>
					</xsl:variable>
					<xsl:variable name="miscmonth">
						<xsl:value-of select="substring(@DepartureDateTime,1,2)"/>
					</xsl:variable>
					<xsl:variable name="miscday">
						<xsl:value-of select="substring(@DepartureDateTime,4,2)"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$miscmonth > $curmonth">
							<xsl:value-of select="$curyear"/>
							<xsl:value-of select="concat('-',@DepartureDateTime)"/>
						</xsl:when>
						<xsl:when test="$curmonth > $miscmonth">
							<xsl:value-of select="$curyear + 1"/>
							<xsl:value-of select="concat('-',@DepartureDateTime)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="$miscday >= $curday">
									<xsl:value-of select="$curyear"/>
									<xsl:value-of select="concat('-',@DepartureDateTime)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$curyear + 1"/>
									<xsl:value-of select="concat('-',@DepartureDateTime)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<Description>
					<xsl:text>Miscellaneous</xsl:text>
					<xsl:value-of select="concat(' - Board point: ',OriginLocation/@LocationCode)"/>
					<xsl:if test="Text!=''">
						<xsl:value-of select="concat(' - ',Text)"/>
					</xsl:if>
				</Description>
				<TPA_Extensions Status="{@Status}" NumberInParty="{format-number(@NumberInParty,'#0')}">
					<Vendor Code="{Vendor/@Code}"/>
					<OriginCityCode>
						<xsl:value-of select="OriginLocation/@LocationCode"/>
					</OriginCityCode>
				</TPA_Extensions>

			</General>
		</Item>
	</xsl:template>

	<!--*****************************************************************-->
	<!--			Car Segs						   		     -->
	<!--*****************************************************************-->
	<xsl:template match="Vehicle" mode="Car">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
			</xsl:attribute>
			<Vehicle>
				<ConfID>
					<xsl:attribute name="Type">C</xsl:attribute>
					<xsl:attribute name="ID">
						<xsl:value-of select="ConfId/@Id"/>
						<xsl:value-of select="ConfirmationNumber"/>
					</xsl:attribute>
				</ConfID>
				<Vendor>
					<xsl:attribute name="Code">
						<xsl:value-of select="Vendor/@Code"/>
						<xsl:value-of select="VehVendorAvail/Vendor/@Code"/>
					</xsl:attribute>
				</Vendor>
				<VehRentalCore>
					<xsl:variable name="year">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,1,4)"/>
					</xsl:variable>
					<xsl:variable name="month">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,6,2)"/>
					</xsl:variable>
					<xsl:variable name="day">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,9,2)"/>
					</xsl:variable>
					<xsl:variable name="cMonth">
						<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,1,2)"/>
					</xsl:variable>
					<xsl:variable name="cDay">
						<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,4,2)"/>
					</xsl:variable>
					<xsl:variable name="rMonth">
						<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,1,2)"/>
					</xsl:variable>
					<xsl:variable name="rDay">
						<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,4,2)"/>
					</xsl:variable>
					<xsl:variable name="cYear">
						<xsl:choose>
							<xsl:when test="$cMonth &lt; $month">
								<xsl:value-of select="$year + 1"/>
							</xsl:when>
							<xsl:when test="$cDay &lt; $day">
								<xsl:value-of select="$year + 1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$year"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="rYear">
						<xsl:choose>
							<xsl:when test="$rMonth &lt; $month">
								<xsl:value-of select="$year + 1"/>
							</xsl:when>
							<xsl:when test="$rDay &lt; $day">
								<xsl:value-of select="$year + 1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$year"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:attribute name="PickUpDateTime">
						<xsl:value-of select="concat($cYear,'-',VehRentalCore/@PickUpDateTime,':00')" />
					</xsl:attribute>
					<xsl:attribute name="ReturnDateTime">
						<xsl:value-of select="concat($rYear,'-',VehRentalCore/@ReturnDateTime,':00')" />
					</xsl:attribute>
					<PickUpLocation>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode"/>
							<xsl:value-of select="VehRentalCore/LocationDetails/@LocationCode"/>
						</xsl:attribute>
					</PickUpLocation>
					<ReturnLocation>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode"/>
							<xsl:value-of select="VehRentalCore/LocationDetails/@LocationCode"/>
						</xsl:attribute>
					</ReturnLocation>
				</VehRentalCore>
				<Vehicle>
					<xsl:attribute name="AirConditionInd">

						<xsl:choose>
							<xsl:when test="substring(VehVendorAvail/VehResCore/RentalRate/Vehicle/VehType,4,1) = 'R'">true</xsl:when>
							<xsl:otherwise>false</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="TransmissionType">

						<xsl:choose>
							<xsl:when test="substring(VehVendorAvail/VehResCore/RentalRate/Vehicle/VehType,3,1) = 'A'">Automatic</xsl:when>
							<xsl:otherwise>Manual</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<VehType>
						<xsl:attribute name="VehicleCategory">
							<xsl:value-of select="Vehicle/VehType/@VehicleCategory"/>
							<xsl:value-of select="VehVendorAvail/VehResCore/RentalRate/Vehicle/VehType"/>
						</xsl:attribute>
					</VehType>
				</Vehicle>
				<xsl:if test="RentalRate/VehicleCharges/VehicleCharge/@Amount!=''">
					<RentalRate>
						<RateDistance>
							<xsl:attribute name="Unlimited">
								<xsl:choose>
									<xsl:when test="RentalRate/RateDistance/@Unlimited='true'">1</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="DistUnitName">
								<xsl:choose>
									<xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@UnitOfMeasure='K'">Km</xsl:when>
									<xsl:otherwise>Mile</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</RateDistance>
						<VehicleCharges>
							<VehicleCharge>
								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(string(RentalRate/VehicleCharges/VehicleCharge/@Amount),'.','')"/>
									<xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/ChargeDetails/ApproximateTotalCharge	[@RateType='APPROXIMATE TOTAL PRICE']/@Amount),'.','')"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@CurrencyCode"/>
									<xsl:value-of select="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@CurrencyCode"/>
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
								<xsl:attribute name="TaxInclusive">
									<xsl:choose>
										<xsl:when test="RentalRate/VehicleCharges/VehicleCharge/@TaxInclusive='true'">1</xsl:when>
										<xsl:otherwise>0	</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="GuaranteedInd">
									<xsl:choose>
										<xsl:when test="VehVendorAvail/VehResCore/RentalRate/@GuaranteedQuoted='G'">1</xsl:when>
										<xsl:otherwise>0	</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</VehicleCharge>
						</VehicleCharges>
					</RentalRate>
					<TotalCharge>
						<xsl:attribute name="RateTotalAmount">
							<xsl:value-of select="translate(string(TotalCharge/@RateTotalAmount),'.','')"/>
							<xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/ChargeDetails/ApproximateTotalCharge[@RateType='APPROXIMATE 	TOTAL PRICE']/@Amount),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="EstimatedTotalAmount">
							<xsl:value-of select="translate(string(TotalCharge/@EstimatedTotalAmount),'.','')"/>


							<xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/ChargeDetails/ApproximateTotalCharge[@RateType='APPROXIMATE 	TOTAL PRICE']/@Amount),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="TotalCharge/@CurrencyCode"/>
							<xsl:value-of select="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@CurrencyCode"/>
						</xsl:attribute>
					</TotalCharge>
				</xsl:if>
			</Vehicle>
		</Item>
	</xsl:template>
	<!--*****************************************************************-->
	<!--			Hotel Segs								    -->
	<!--*****************************************************************-->
	<xsl:template match="Hotel" mode="Hotel">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
			</xsl:attribute>
			<Hotel>
				<Reservation>
					<RoomTypes>
						<RoomType>
							<xsl:attribute name="RoomTypeCode">
								<xsl:value-of select="Reservation/RoomTypes/RoomType/@RoomTypeCode"/>
								<xsl:value-of select="RoomRates/Rate/@RoomTypeCode"/>
							</xsl:attribute>
							<xsl:if test="Reservation/RoomTypes/RoomType/@NumberOfUnits!='' or @NumberOfUnits!=''">
								<xsl:attribute name="NumberOfUnits">
									<xsl:value-of select="Reservation/RoomTypes/RoomType/@NumberOfUnits"/>
									<xsl:value-of select="@NumberOfUnits"/>
								</xsl:attribute>
							</xsl:if>
						</RoomType>
					</RoomTypes>
					<xsl:if test="translate(string(Reservation/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')!='' or translate(string(RoomRates/Rate/@Amount),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')!=''">
						<RoomRates>
							<RoomRate>
								<Rates>
									<Rate>
										<Base>
											<xsl:attribute name="AmountBeforeTax">
												<xsl:value-of select="translate(string(Reservation/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
												<xsl:value-of select="translate(string(RoomRates/Rate/@Amount),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
											</xsl:attribute>
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="Reservation/RoomRates/RoomRate/Rates/Rate/Base/@CurrencyCode"/>
												<xsl:value-of select="RoomRates/Rate/@CurrencyCode"/>
											</xsl:attribute>
										</Base>
									</Rate>
								</Rates>
							</RoomRate>
						</RoomRates>
					</xsl:if>
					<GuestCounts>
						<GuestCount>
							<xsl:attribute name="AgeQualifyingCode">
								<xsl:choose>
									<xsl:when test="Reservation/GuestCounts/GuestCount/@AgeQualifyingCode">
										<xsl:value-of select="Reservation/GuestCounts/GuestCount/@AgeQualifyingCode"/>
									</xsl:when>
									<xsl:otherwise>10</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="Count">
								<xsl:value-of select="Reservation/GuestCounts/GuestCount/@Count"/>
								<xsl:value-of select="@NumberInParty"/>
							</xsl:attribute>
						</GuestCount>
					</GuestCounts>
					<xsl:variable name="ny">
						<xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,1,4)"/>
					</xsl:variable>
					<TimeSpan>
						<xsl:attribute name="Start">
							<xsl:value-of select="Reservation/TimeSpan/@Start"/>
							<xsl:value-of select="concat($ny,'-',TimeSpan/@Start)"/>
						</xsl:attribute>
						<xsl:attribute name="Duration">
							<xsl:value-of select="Reservation/TimeSpan/@Duration"/>
							<xsl:value-of select="TimeSpan/@Duration"/>
						</xsl:attribute>
						<xsl:attribute name="End">
							<xsl:value-of select="Reservation/TimeSpan/@End"/>
							<xsl:value-of select="concat($ny,'-',TimeSpan/@End)"/>
						</xsl:attribute>
					</TimeSpan>
					<BasicPropertyInfo>
						<xsl:attribute name="ChainCode">
							<xsl:value-of select="Reservation/BasicPropertyInfo/@ChainCode"/>
							<xsl:value-of select="BasicPropertyInfo/@ChainCode"/>
						</xsl:attribute>
						<xsl:attribute name="HotelCityCode">
							<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCityCode"/>
							<xsl:value-of select="BasicPropertyInfo/@HotelCityCode"/>
						</xsl:attribute>
						<xsl:if test="Reservation/BasicPropertyInfo/@HotelCode!= '' or BasicPropertyInfo/@HotelCode!=''">
							<xsl:attribute name="HotelCode">
								<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCode"/>
								<xsl:value-of select="BasicPropertyInfo/@HotelCode"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="HotelName">
							<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelName"/>
							<xsl:value-of select="BasicPropertyInfo/@HotelName"/>
						</xsl:attribute>
					</BasicPropertyInfo>
				</Reservation>
				<TPA_Extensions>
					<xsl:attribute name="ConfirmationNumber">
						<xsl:value-of select="TPA_Extensions/ConfirmationNumber"/>
						<xsl:value-of select="BasicPropertyInfo/ConfirmationNumber"/>
					</xsl:attribute>
				</TPA_Extensions>
			</Hotel>
		</Item>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Calculate Total FareTotals	 	      			           -->
	<!--***********************************************************************************-->
	<xsl:template match="AirFareInfo" mode="Air">
		<AirFareInfo>
			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(string(ItinTotalFare/BaseFare/@Amount),'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</BaseFare>
				<EquivFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</EquivFare>
				<Taxes>
					<xsl:variable name="TaxAmountNoDec">
						<xsl:value-of select="translate(sum(ItinTotalFare/Taxes/Tax/@Amount),'.','')"/>
					</xsl:variable>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TaxAmountNoDec"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
					<xsl:apply-templates select="ItinTotalFare/Taxes/Tax" mode="TotalFare"/>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(string(ItinTotalFare/TotalFare/@Amount),'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FareInfo"/>
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Calculate Total FareTotals	 	      			           -->
	<!--***********************************************************************************-->
	<xsl:template match="ItineraryPricing" mode="Fare">
		<xsl:variable name="dect">
			<xsl:choose>
				<xsl:when test="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
					<xsl:value-of select="string-length(substring-after(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="string-length(substring-after(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="PriceQuote/PriceQuotePlus/PassengerInfo">
				<AirFareInfo>
					<xsl:attribute name="PricingSource">
						<xsl:choose>
							<xsl:when test="../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">
								<!--<xsl:when test="../../../FareFamily/PriceQuoteInfo/Details/MessageInfo/Message[contains(text(),'PRIVATE FARE APPLIED')]">-->
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:when test="contains(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/@InputMessage,'PFA') or contains(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/@InputMessage,'JCB') or contains(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/@InputMessage,'JNN') or contains(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/@InputMessage,'JNF')">
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Published'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/EquivFare">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/EquivFare/@Amount,'.','')"/>
										<!--
                      <xsl:apply-templates select="PriceQuoteTotals/EquivFare">
                        <xsl:with-param name="totalbf">0</xsl:with-param>
                        <xsl:with-param name="pos">1</xsl:with-param>
                        <xsl:with-param name="bfcount">
                          <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                        </xsl:with-param>
                      </xsl:apply-templates>
                    -->
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="PriceQuoteTotals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(PriceQuoteTotals/BaseFare/@Amount,'.','')"/>
												<!--
                <xsl:apply-templates select="PriceQuoteTotals/BaseFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                  </xsl:with-param>
                </xsl:apply-templates>
                -->
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount,'.','')"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Amount">0.00</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<xsl:if test="PriceQuoteTotals/EquivFare">
							<EquivFare>
								<xsl:variable name="eqdect">
									<xsl:choose>
										<xsl:when test="PriceQuote[PriceQuotePlus/PassengerInfo]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount!=''">
											<xsl:value-of select="string-length(substring-after(PriceQuote[PriceQuotePlus/PassengerInfo]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="string-length(substring-after(PriceQuoteTotals/BaseFare/@Amount,'.'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(string(PriceQuote[PriceQuotePlus/PassengerInfo]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount),'.','')" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$eqdect"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode" />
								</xsl:attribute>
							</EquivFare>
						</xsl:if>
						<Taxes>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/Taxes/Tax/@Amount,'.','')"/>
										<!--
                    <xsl:variable name="tax">
                      <xsl:apply-templates select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
                        <xsl:with-param 	name="totalbf">0</xsl:with-param>
                        <xsl:with-param name="pos">1</xsl:with-param>
                        <xsl:with-param name="bfcount">
                          <xsl:value-of 	select="count(PriceQuote)+1"/>
                        </xsl:with-param>
                      </xsl:apply-templates>
                    </xsl:variable>            
                    <xsl:choose>
                      <xsl:when test="$tax=''">0</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$tax"/>
                      </xsl:otherwise>
                    </xsl:choose>
                    -->
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/TotalFare/@Amount,'.','')"/>
										<!--
            <xsl:apply-templates select="PriceQuoteTotals/TotalFare">
              <xsl:with-param name="totalbf">0</xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
              <xsl:with-param name="bfcount">
                <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
              </xsl:with-param>
            </xsl:apply-templates>
            -->
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<!--        
        <xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][PricedItinerary/@InputMessage!='M-A']"/>
        -->
						<xsl:apply-templates select="../../../DisplayPriceQuoteRS/PriceQuote">
							<xsl:with-param name="pqh" select="PriceQuote[PriceQuotePlus/PassengerInfo][MiscInformation/SignatureLine/@Status='ACTIVE']"/>
						</xsl:apply-templates>

					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:when>
			<xsl:otherwise>
				<AirFareInfo>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<Taxes>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<xsl:apply-templates select="PriceQuote/PricedItinerary/AirItineraryPricingInfo"/>
					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="PriceQuote" mode="Fare">
		<xsl:variable name="pqN" select="@RPH" />
		<xsl:variable name="dect">
			<xsl:choose>
				<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
					<xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="PriceQuotePlus/PassengerInfo">
				<AirFareInfo>
					<xsl:attribute name="PricingSource">
						<xsl:choose>
							<xsl:when test="../../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:when test="contains(PricedItinerary/@InputMessage,'PFA') or contains(PricedItinerary/@InputMessage,'JCB') or contains(PricedItinerary/@InputMessage,'JNN') or contains(PricedItinerary/@InputMessage,'JNF')">
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Published'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/EquivFare">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/EquivFare/@Amount,'.','')"/>
										<!--
                      <xsl:apply-templates select="PriceQuoteTotals/EquivFare">
                        <xsl:with-param name="totalbf">0</xsl:with-param>
                        <xsl:with-param name="pos">1</xsl:with-param>
                        <xsl:with-param name="bfcount">
                          <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                        </xsl:with-param>
                      </xsl:apply-templates>
                    -->
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="PriceQuoteTotals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(PriceQuoteTotals/BaseFare/@Amount,'.','')"/>
												<!--
                <xsl:apply-templates select="PriceQuoteTotals/BaseFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                  </xsl:with-param>
                </xsl:apply-templates>
                -->
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount,'.','')"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Amount">0.00</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<xsl:if test="PriceQuoteTotals/EquivFare">
							<EquivFare>
								<xsl:variable name="eqdect">
									<xsl:choose>
										<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount!=''">
											<xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="string-length(substring-after(PriceQuoteTotals/BaseFare/@Amount,'.'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(string(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount),'.','')" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$eqdect"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode" />
								</xsl:attribute>
							</EquivFare>
						</xsl:if>
						<Taxes>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/Taxes/Tax/@Amount,'.','')"/>

									</xsl:attribute>
								</xsl:when>
								<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/TotalFare/@Amount,'.','')"/>

									</xsl:attribute>
								</xsl:when>
								<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>

						<!--<xsl:apply-templates select="../../../../DisplayPriceQuoteRS/PriceQuote[@RPH=$pqN]">-->
						<xsl:apply-templates select=".">
							<xsl:with-param name="pqh" select="."/>
						</xsl:apply-templates>

					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:when>
			<xsl:otherwise>
				<AirFareInfo>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<Taxes>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo"/>
					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="PricedItinerary" mode="Exch">
		<xsl:variable name="dect">
			<xsl:choose>
				<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
					<xsl:value-of select="string-length(substring-after(AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="string-length(substring-after(AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="PriceQuote/PriceQuotePlus/PassengerInfo">
				<AirFareInfo>
					<xsl:attribute name="PricingSource">
						<xsl:choose>
							<xsl:when test="../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">
								<!-- <xsl:when test="../../../FareFamily/PriceQuoteInfo/Details/MessageInfo/Message[contains(text(),'PRIVATE FARE APPLIED')]"> -->
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:when test="contains(@InputMessage,'PFA') or contains(@InputMessage,'JCB') or contains(@InputMessage,'JNN') or contains(@InputMessage,'JNF')">
								<xsl:value-of select="'Private'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Published'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/EquivFare">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/EquivFare/@Amount,'.','')"/>
										<!--
                      <xsl:apply-templates select="PriceQuoteTotals/EquivFare">
                        <xsl:with-param name="totalbf">0</xsl:with-param>
                        <xsl:with-param name="pos">1</xsl:with-param>
                        <xsl:with-param name="bfcount">
                          <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                        </xsl:with-param>
                      </xsl:apply-templates>
                    -->
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="PriceQuoteTotals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(PriceQuoteTotals/BaseFare/@Amount,'.','')"/>
												<!--
                <xsl:apply-templates select="PriceQuoteTotals/BaseFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
                  </xsl:with-param>
                </xsl:apply-templates>
                -->
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount">
											<xsl:attribute name="Amount">
												<xsl:value-of select="translate(AirItineraryPricingInfo/ItinTotalFare/Totals/BaseFare/@Amount,'.','')"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Amount">0.00</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<xsl:if test="PriceQuoteTotals/EquivFare">
							<EquivFare>
								<xsl:variable name="eqdect">
									<xsl:choose>
										<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount!=''">
											<xsl:value-of select="string-length(substring-after(AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="string-length(substring-after(PriceQuoteTotals/BaseFare/@Amount,'.'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(string(AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount),'.','')" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$eqdect"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode" />
								</xsl:attribute>
							</EquivFare>
						</xsl:if>
						<Taxes>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/Taxes/Tax/@Amount,'.','')"/>
										<!--
                    <xsl:variable name="tax">
                      <xsl:apply-templates select="PriceQuote[PriceQuotePlus/PassengerInfo][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
                        <xsl:with-param 	name="totalbf">0</xsl:with-param>
                        <xsl:with-param name="pos">1</xsl:with-param>
                        <xsl:with-param name="bfcount">
                          <xsl:value-of 	select="count(PriceQuote)+1"/>
                        </xsl:with-param>
                      </xsl:apply-templates>
                    </xsl:variable>            
                    <xsl:choose>
                      <xsl:when test="$tax=''">0</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$tax"/>
                      </xsl:otherwise>
                    </xsl:choose>
                    -->
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(AirItineraryPricingInfo/ItinTotalFare/Totals/Taxes/Tax/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:choose>
								<xsl:when test="PriceQuoteTotals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(PriceQuoteTotals/TotalFare/@Amount,'.','')"/>
										<!--
            <xsl:apply-templates select="PriceQuoteTotals/TotalFare">
              <xsl:with-param name="totalbf">0</xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
              <xsl:with-param name="bfcount">
                <xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
              </xsl:with-param>
            </xsl:apply-templates>
            -->
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount">
									<xsl:attribute name="Amount">
										<xsl:value-of select="translate(AirItineraryPricingInfo/ItinTotalFare/Totals/TotalFare/@Amount,'.','')"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">0.00</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<!--        
        <xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][PricedItinerary/@InputMessage!='M-A']"/>
        -->
						<xsl:apply-templates select="../../../DisplayPriceQuoteRS/PriceQuote">
							<xsl:with-param name="pqh" select="PriceQuote[PriceQuotePlus/PassengerInfo]"/>
						</xsl:apply-templates>

					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:when>
			<xsl:otherwise>
				<AirFareInfo>
					<ItinTotalFare>
						<BaseFare>
							<xsl:choose>
								<xsl:when test="AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="AirItineraryPricingInfo/ItinTotalFare/EquivFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="AirItineraryPricingInfo/ItinTotalFare/BaseFare">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote)+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</BaseFare>
						<Taxes>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</Taxes>
						<TotalFare>
							<xsl:attribute name="Amount">
								<xsl:apply-templates select="AirItineraryPricingInfo/ItinTotalFare/TotalFare">
									<xsl:with-param name="totalbf">0</xsl:with-param>
									<xsl:with-param name="pos">1</xsl:with-param>
									<xsl:with-param name="bfcount">
										<xsl:value-of select="count(PriceQuote)+1"/>
									</xsl:with-param>
								</xsl:apply-templates>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect"/>
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<xsl:apply-templates select="AirItineraryPricingInfo"/>
					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="PriceQuote">
		<xsl:param name="pqh"/>
		<xsl:variable name="pos" select="position()"/>
		<xsl:variable name="rph" select="@RPH"/>
		<xsl:variable name="pq" select="$pqh[@RPH=$rph]"/>

		<!-- PNR can be priced differently than Airline alows. That is why we need two variables in order to find correct PQ. -->
		<xsl:variable name="ptc" select="$pq/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code"/>
		<xsl:variable name="ptc2" select="substring(substring-after($pq/PricedItinerary/@InputMessage, 'P'), 1,3)"/>

		<xsl:variable name="pqPAX">
			<xsl:for-each select="$pq/PriceQuotePlus/PassengerInfo/PassengerData/@NameNumber">
				<xsl:variable name="pn" select="." />
				<xsl:if test="position() > 1">
					<xsl:value-of select="' '"/>
				</xsl:if>
				<xsl:value-of select="/TravelItineraryReadRS/TravelItinerary/CustomerInfo/PersonName[@NameNumber=$pn]/@RPH"/>
			</xsl:for-each>
		</xsl:variable>

		<xsl:choose>
			<!-- [PassengerTypeQuantity/@Code=$ptc] -->
			<xsl:when test="$pq/PricedItinerary/AirItineraryPricingInfo">
				<xsl:apply-templates select="$pq/PricedItinerary/AirItineraryPricingInfo">
					<xsl:with-param name="pos" select="$pos"/>
					<xsl:with-param name="pqPAX" select="$pqPAX"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$pq/PricedItinerary/AirItineraryPricingInfo">
					<xsl:with-param name="pos" select="$pos"/>
					<xsl:with-param name="pqPAX" select="$pqPAX"/>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="BaseFare">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:variable name="bf1">
			<xsl:value-of select="translate(@Amount,'.','')"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
		</xsl:variable>
		<xsl:variable name="bf">
			<xsl:value-of select="$bf1 * $nip"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
				<xsl:apply-templates select="../../../../..//PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$totalbf + $bf"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="EquivFare">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:variable name="bf1">
			<xsl:value-of select="translate(@Amount,'.','')"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
		</xsl:variable>
		<xsl:variable name="bf">
			<xsl:value-of select="$bf1 * $nip"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
				<xsl:apply-templates select="../../../../..//PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$totalbf + $bf"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="Tax">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:variable name="bf1">
			<xsl:value-of select="translate(@Amount,'.','')"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/>
		</xsl:variable>
		<xsl:variable name="bf">
			<xsl:value-of select="$bf1 * $nip"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$pos &lt; $bfcount and ../../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount!=''">
				<xsl:apply-templates select="../../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@Amount!='']">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$totalbf + $bf"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="TotalFare">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:variable name="bf1">
			<xsl:value-of select="translate(@Amount,'.','')"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
		</xsl:variable>
		<xsl:variable name="bf">
			<xsl:value-of select="$bf1 * $nip"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
				<xsl:apply-templates select="../../../../..//PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$totalbf + $bf"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="calcbf">
		<xsl:param name="bf"/>
		<xsl:param name="totalbf"/>
		<xsl:if test="$bf != ''">
			<xsl:variable name="temp">
				<xsl:value-of select="substring-before($bf,'/')"/>
			</xsl:variable>
			<xsl:call-template name="calcbf">
				<xsl:with-param name="bf">
					<xsl:value-of select="substring-after($bf,'/')"/>
				</xsl:with-param>
				<xsl:with-param name="totalbf">
					<xsl:value-of select="$totalbf + $temp"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="$totalbf"/>
	</xsl:template>

	<!--************************************************************************************-->
	<!--					Individual Tax element 	 	      			                -->
	<!--***********************************************************************************-->
	<xsl:template match="Tax" mode="TotalFare">
		<Tax>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(string(@Amount),'.','')"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../../ItinTotalFare/TotalFare/@CurrencyCode"/>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Calculate Fare Totals per Passenger Type	 	                 -->
	<!--************************************************************************************-->
	<xsl:template match="AirItineraryPricingInfo">
		<xsl:param name="pos"/>
		<xsl:param name="pqPAX"/>
		<xsl:variable name="dect1">
			<xsl:choose>
				<xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
					<xsl:value-of select="string-length(substring-after(ItinTotalFare/EquivFare/@Amount,'.'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="string-length(substring-after(ItinTotalFare/BaseFare/@Amount,'.'))"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<PTC_FareBreakdown>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos"/>
			</xsl:attribute>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<!--<xsl:when test="../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">-->
					<xsl:when test="../../ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">
						<xsl:value-of select="'Private'"/>
					</xsl:when>
					<!--<xsl:when test="contains(../@InputMessage,'PFA') or contains(../@InputMessage,'JCB') or contains(../@InputMessage,'JNN') or contains(../@InputMessage,'JNF')">-->
					<xsl:when test="PrivateFareInformation/@PrivateFareType">
						<xsl:value-of select="'Private'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Published'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:value-of select="$pqPAX"/>
			</xsl:attribute>

			<xsl:variable name="pqRPH">
				<xsl:value-of select="../../@RPH"/>
			</xsl:variable>

			<xsl:variable name="rph">
				<xsl:for-each select="PTC_FareBreakdown/FlightSegment">
					<xsl:variable name="flt">
						<xsl:value-of select="@FlightNumber"/>
					</xsl:variable>
					<xsl:variable name="posFLT">
						<xsl:value-of select="position()"/>
					</xsl:variable>

					<xsl:variable name="itemNum">
						<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
					</xsl:variable>

					<xsl:variable name="sqCount">
						<xsl:value-of select="count(../../../../../../ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flt,'#0000')])"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$sqCount > 1">
							<xsl:value-of select="format-number(../../../../../../ReservationItems/Item[@RPH=$itemNum]/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flt,'#0000')]/@SegmentNumber[not(../preceding-sibling::FlightSegment/@SegmentNumber = .)],'#0')"/>
						</xsl:when>
						<xsl:when test="../../../../../../ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flt,'#0000')]">
							<xsl:value-of select="format-number(../../../../../../ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flt,'#0000')]/@SegmentNumber[not(../preceding-sibling::FlightSegment/@SegmentNumber = .)],'#0')"/>

							<xsl:if test="FareBasis/@Code='VOID'">
								<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
							</xsl:if>
						</xsl:when>

						<!--
            <xsl:when test="../../../../../../ReservationItems/Item/FlightSegment[format-number(@FlightNumber,'#0000')=format-number($flt,'#0000')]">
              <xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
            </xsl:when>
           -->

						<xsl:when test="FareBasis/@Code='VOID'">
							<xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
						</xsl:when>
					</xsl:choose>

					<!--<xsl:if test="position() > 1">-->
					<xsl:text> </xsl:text>
					<!--</xsl:if>-->
				</xsl:for-each>


			</xsl:variable>

			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:call-template name="string-trim">
					<xsl:with-param name="string" select="$rph" />
				</xsl:call-template>
			</xsl:attribute>

			<PassengerTypeQuantity>
				<!--
        <xsl:choose>
          <xsl:when test="PassengerTypeQuantity/@Code='C09'">
            <xsl:attribute name="Code">CNN</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
        -->
				<xsl:attribute name="Code">
					<xsl:value-of select="PassengerTypeQuantity/@Code"/>
				</xsl:attribute>
				<!--
          </xsl:otherwise>
        </xsl:choose>
        -->
				<xsl:attribute name="Quantity">
					<xsl:value-of select="PassengerTypeQuantity/@Quantity"/>
				</xsl:attribute>
			</PassengerTypeQuantity>

			<FareBasisCodes>
				<xsl:for-each select="PTC_FareBreakdown/FlightSegment/FareBasis">
					<FareBasisCode>
						<xsl:value-of select="@Code"/>
					</FareBasisCode>
				</xsl:for-each>
			</FareBasisCodes>

			<xsl:variable name="brcmd">
				<xsl:choose>
					<xsl:when test="starts-with(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER']/Text, 'WPBR')">
						<xsl:value-of select="translate(PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER']/Text, 'WPBR','$BR')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="PTC_FareBreakdown/Endorsements/Endorsement[@type='PRICING_PARAMETER']/Text"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>



			<xsl:if test="contains($brcmd, '$BR') or contains($brcmd, '*BR')">
				<BrandedFares>
					<xsl:choose>
						<xsl:when test="contains($brcmd, '$S') or contains($brcmd, '*BR') or contains($brcmd, 'WPBR')">

							<xsl:choose>
								<xsl:when test="contains($brcmd,'$S1')">
									<xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='' and FareBasis/@Code!='VOID']" mode="ffSegmental">
										<xsl:with-param name="brID" select="substring-after($brcmd, '$')"/>
									</xsl:apply-templates>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="ffSegs">
										<xsl:call-template name="tokenizeString">
											<!-- store anything left in another variable -->
											<xsl:with-param name="list" select="substring-before(translate(concat('S1',substring-after($brcmd, 'S1')), '/','-'), '$P')"/>
											<xsl:with-param name="delimiter" select="'$'"/>
										</xsl:call-template>
									</xsl:variable>

									<xsl:variable name="flSeg" select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='']" />

									<xsl:for-each select="msxsl:node-set($ffSegs)/elem/node()[1]">
										<xsl:if test="contains(.,'*BR')">
											<xsl:apply-templates select="$flSeg" mode="ffSegmental">
												<xsl:with-param name="brID" select="."/>
											</xsl:apply-templates>
										</xsl:if>
									</xsl:for-each>

									<!--
                  <xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='']" mode="ffSegmental">
                    <xsl:with-param name="brID" select="substring-before(translate(concat('S1',substring-after($brcmd, 'S1')), '/','-'), '$P')"/>
                  </xsl:apply-templates>
                  -->
								</xsl:otherwise>
							</xsl:choose>

						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="PTC_FareBreakdown/FlightSegment[@SegmentNumber!='' and @Status]" mode="ff">
								<xsl:with-param name="brID" select="substring-after($brcmd, '$BR')"/>
							</xsl:apply-templates>
						</xsl:otherwise>
					</xsl:choose>
				</BrandedFares>
			</xsl:if>

			<!--
      <xsl:if test="../../../../../../FareFamily/PriceQuoteInfo/Details[@number=$pos]/SegmentInfo/BrandedFare/@description">
        <BrandedFares>
          <xsl:apply-templates select="../../../../../../FareFamily/PriceQuoteInfo/Details[@number=$pos]/SegmentInfo" mode="FareFamily" />
        </BrandedFares>
      </xsl:if>
      -->

			<xsl:variable name="nip">
				<xsl:value-of select="PassengerTypeQuantity/@Quantity"/>
			</xsl:variable>
			<PassengerFare>
				<BaseFare>
					<xsl:choose>
						<xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect1"/>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(string(ItinTotalFare/BaseFare/@Amount),'.','')"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dect1"/>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</BaseFare>
				<Taxes>
					<xsl:apply-templates select="ItinTotalFare/Taxes/Tax" mode="PTC"/>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(string(ItinTotalFare/TotalFare/@Amount),'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dect1"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<xsl:if test="PTC_FareBreakdown/FareCalculation/Text!=''">
					<FareCalculation>
						<xsl:value-of select="PTC_FareBreakdown/FareCalculation/Text"/>
					</FareCalculation>
				</xsl:if>

				<xsl:if test="ItinTotalFare/EquivFare/@Amount!=''">
					<BSR>
						<xsl:variable name="bsr">
							<xsl:value-of select="format-number(number(ItinTotalFare/EquivFare/@Amount) div number(ItinTotalFare/BaseFare/@Amount),'#0.000000')"/>
						</xsl:variable>
						<xsl:value-of select="$bsr"/>
					</BSR>
				</xsl:if>

				<xsl:if test="../../PricedItinerary/@ValidatingCarrier != ''">
					<ValidatingAirlineCode>
						<xsl:value-of select="../../PricedItinerary/@ValidatingCarrier"/>
					</ValidatingAirlineCode>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="../../PricedItinerary/@ValidatingCarrier"/>
						</xsl:attribute>
					</Vendor>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="contains(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'P')">
						<BagAllowance>
							<xsl:attribute name="Quantity">
								<xsl:value-of select="translate(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'PW','')"/>
							</xsl:attribute>
							<xsl:attribute name="Type">
								<xsl:value-of select="'Piece'"/>
							</xsl:attribute>
						</BagAllowance>
					</xsl:when>
					<xsl:when test="contains(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'W')">
						<BagAllowance>
							<xsl:attribute name="Weight">
								<xsl:value-of select="translate(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'PW','')"/>
							</xsl:attribute>
							<xsl:attribute name="Type">
								<xsl:value-of select="'Weight'"/>
							</xsl:attribute>
						</BagAllowance>
					</xsl:when>
				</xsl:choose>

				<xsl:if test="../@InputMessage">
					<SupplementalInfo>
						<xsl:value-of select="../@InputMessage"/>
					</SupplementalInfo>
				</xsl:if>

			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>

	<xsl:template match="FlightSegment" mode="ff">
		<xsl:param name="brID"/>
		<FareFamily>
			<xsl:attribute name="RPH">
				<xsl:value-of select="@SegmentNumber"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="contains($brID, '$')">
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="translate(substring-before($brID, '$'), '$RQ', '')" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="$brID" />
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>

		</FareFamily>
	</xsl:template>

	<xsl:template match="FlightSegment" mode="ffSegmental">
		<!-- 
    S1*BRECONOMY?S2*BRECOFLEX 
    S1/2$S3-5*BRMAIN
    -->
		<xsl:param name="brID"/>
		<xsl:variable name="seg">
			<xsl:value-of select="concat('S', @SegmentNumber,'*')"/>
		</xsl:variable>
		<xsl:variable name="segN">
			<xsl:value-of select="@SegmentNumber"/>
		</xsl:variable>

		<xsl:variable name="pStart" select="substring-after(substring-before($brID, '-'), 'S')" />
		<xsl:variable name="pEnd" select="substring-before(substring-after($brID, '-'), '*BR')" />

		<xsl:variable name="ssSegs">
			<xsl:call-template name="displayNumbers">
				<xsl:with-param name="pStart" select="$pStart"/>
				<xsl:with-param name="pEnd" select="$pEnd"/>
				<xsl:with-param name="delem" select="','"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="ff">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(substring-after($brID, concat($seg,'BR')), '$')) > 0">
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="substring-before(substring-after($brID, concat($seg,'BR')), '$')" />
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="contains(substring-before($brID, '$'), '/') or contains(substring-before($brID, '$'), '-')">
					<xsl:choose>
						<xsl:when test="contains(substring-before($brID, '$'), @SegmentNumber)">
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="substring-before(substring-after($brID, 'BR'), '$')" />
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="contains(substring-after($brID, '$'), @SegmentNumber)">
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="substring-after(substring-after(substring-after($brID, 'BR'), '$'), '*BR')" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="substring-before(substring-after($brID, concat($seg,'BR')), '$')" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="not(contains($brID, '$'))">
					<!-- S1-4*BRECOSTAND -->

					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="substring-after($brID, '*BR')" />
					</xsl:call-template>

				</xsl:when>
				<xsl:when test="contains(substring-after($brID, '$'), '/') or contains(substring-after($brID, '$'), '-')">
					<xsl:choose>
						<xsl:when test="contains(substring-after($brID, '$'), @SegmentNumber)">

							<xsl:variable name="ssFFs">
								<xsl:call-template name="tokenizeString">
									<!-- store anything left in another variable -->
									<xsl:with-param name="list" select="$brID"/>
									<xsl:with-param name="delimiter" select="'$'"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:for-each select="ext:node-set($ssFFs)/*">
								<xsl:if test="contains(., 'S')">
									<xsl:if test="contains(., $segN)">
										<xsl:call-template name="string-trim">
											<xsl:with-param name="string" select="substring-after(./text(), 'BR')" />
										</xsl:call-template>
									</xsl:if>
								</xsl:if>
							</xsl:for-each>

						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="substring-after(substring-after($brID, concat($seg,'BR')), '$')" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="substring-after($brID, concat($seg,'BR'))" />
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:if test="contains($brID, @SegmentNumber) or contains($ssSegs, @SegmentNumber)">
			<FareFamily>
				<xsl:attribute name="RPH">
					<xsl:value-of select="@SegmentNumber"/>
				</xsl:attribute>
				<xsl:value-of select="$ff"/>
			</FareFamily>
		</xsl:if>
	</xsl:template>

	<!-- This Template if FareRQ were used 
  <xsl:template match="SegmentInfo" mode="FareFamily">
    <xsl:if test="BrandedFare/@description">
      <FareFamily>
        <xsl:attribute name="RPH">
          <xsl:value-of select="@number"/>
        </xsl:attribute>
        <xsl:value-of select="BrandedFare/@description"/>
      </FareFamily>
    </xsl:if>
  </xsl:template>
  -->
	<!--************************************************************************************-->
	<!--			Calculate Fare Totals per Passenger Type	 	                 -->
	<!--************************************************************************************-->
	<xsl:template match="AirFareInfo">
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Code"/>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<xsl:if test="PTC_FareBreakdown/FareBasisCode">
				<FareBasisCodes>
					<xsl:variable name="fbc">
						<xsl:value-of select="PTC_FareBreakdown/FareBasisCode"/>
					</xsl:variable>
					<xsl:call-template name="farebasis">
						<xsl:with-param name="fbc">
							<xsl:value-of select="$fbc"/>
						</xsl:with-param>
					</xsl:call-template>
				</FareBasisCodes>
			</xsl:if>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:variable name="bf">
							<xsl:choose>
								<xsl:when test="PTC_FareBreakdown/PassengerFare/EquivFare/@Amount!=''">
									<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/BaseFare/@Amount),'.','')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="nip">
							<xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
						</xsl:variable>
						<xsl:value-of select="$bf"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
				</BaseFare>
				<xsl:if test="PTC_FareBreakdown/PassengerFare/EquivFare">
					<EquivFare>
						<xsl:apply-templates select="PriceQuoteTotals/EquivFare">
							<xsl:with-param name="totalbf">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
							<xsl:with-param name="bfcount">
								<xsl:value-of select="count(PriceQuote[PriceQuotePlus/PassengerInfo])+1"/>
							</xsl:with-param>
						</xsl:apply-templates>

						<!--
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFare/@DecimalPlaces" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivFare/@CurrencyCode" />
						</xsl:attribute>
            -->
					</EquivFare>
				</xsl:if>
				<Taxes>
					<xsl:apply-templates select="PTC_FareBreakdown/PassengerFare//Taxes/Tax" mode="PTC"/>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:variable name="bf">
							<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/TotalFare/@Amount),'.','')"/>
						</xsl:variable>
						<xsl:variable name="nip">
							<xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
						</xsl:variable>
						<xsl:value-of select="$bf"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
	</xsl:template>
	<xsl:template name="farebasis">
		<xsl:param name="fbc"/>
		<xsl:if test="$fbc != ''">
			<FareBasisCode>
				<xsl:choose>
					<xsl:when test="contains($fbc,'/')">
						<xsl:value-of select="substring-before($fbc,'/')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$fbc"/>
					</xsl:otherwise>
				</xsl:choose>
			</FareBasisCode>
			<xsl:call-template name="farebasis">
				<xsl:with-param name="fbc">
					<xsl:value-of select="substring-after($fbc,'/')"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="FareBasisCode">
		<FareBasisCode>
			<xsl:value-of select="."/>
		</FareBasisCode>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Individual Tax element 	 	      			                -->
	<!--***********************************************************************************-->
	<xsl:template match="Tax" mode="PTC">
		<Tax>
			<xsl:attribute name="Amount">
				<xsl:variable name="bf">
					<xsl:value-of select="translate(string(@Amount),'.','')"/>
				</xsl:variable>
				<xsl:variable name="nip">
					<xsl:value-of select="../../..//PassengerTypeQuantity/@Quantity"/>
				</xsl:variable>
				<xsl:variable name="tottax">
					<xsl:value-of select="$bf * $nip"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$tottax='NaN'">0</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$tottax"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../TotalFare/@CurrencyCode"/>
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--************************************************************************************-->
	<!-- 						Telephone									    -->
	<!--************************************************************************************-->
	<xsl:template match="Telephone">
		<Telephone>
			<xsl:if test="@PhoneUseType != ''">
				<xsl:attribute name="PhoneUseType">
					<xsl:value-of select="@PhoneUseType"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@AreaCityCode!=''">
				<xsl:attribute name="AreaCityCode">
					<xsl:value-of select="@AreaCityCode"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="PhoneNumber">
				<xsl:value-of select="@PhoneNumber"/>
			</xsl:attribute>
		</Telephone>
	</xsl:template>
	<!--************************************************************************************-->
	<!--		EmailAddress  Processing									    -->
	<!--************************************************************************************-->
	<xsl:template match="Email">
		<xsl:if test=".  != ''">
			<Email>
				<xsl:value-of select="translate(.,'Â','')"/>
			</Email>
		</xsl:if>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Address/Delivery Addres information						    -->
	<!--************************************************************************************-->
	<xsl:template match="Line">
		<Address>
			<xsl:attribute name="UseType">
				<xsl:choose>
					<xsl:when test="@Type = 'Client Address'">Billing</xsl:when>
					<xsl:otherwise>Mailing</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="contains(Text,',')">
					<StreetNmbr>
						<xsl:value-of select="substring-before(Text,',')"/>
					</StreetNmbr>
					<xsl:variable name="txt">
						<xsl:value-of select="substring-after(Text,',')"/>
					</xsl:variable>
					<CityName>
						<xsl:value-of select="substring-before($txt,',')"/>
					</CityName>
					<xsl:variable name="txt1">
						<xsl:value-of select="substring-after($txt,',')"/>
					</xsl:variable>
					<PostalCode>
						<xsl:value-of select="substring-before($txt1,',')"/>
					</PostalCode>
					<xsl:variable name="txt2">
						<xsl:value-of select="substring-after($txt1,',')"/>
					</xsl:variable>
					<StateProv>
						<xsl:value-of select="substring-before($txt2,',')"/>
					</StateProv>
					<xsl:variable name="txt3">
						<xsl:value-of select="substring-after($txt2,',')"/>
					</xsl:variable>
					<CountryName>
						<xsl:attribute name="Code">
							<xsl:value-of select="$txt3"/>
						</xsl:attribute>
					</CountryName>
				</xsl:when>
				<xsl:otherwise>
					<AddressLine>
						<xsl:value-of select="Text"/>
					</AddressLine>
				</xsl:otherwise>
			</xsl:choose>
		</Address>
	</xsl:template>

	<xsl:template match="Address">
		<Address>
			<xsl:attribute name="UseType">
				<xsl:choose>
					<xsl:when test="AddressLine[1]/@type = 'B'">Billing</xsl:when>
					<xsl:otherwise>Mailing</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>

			<xsl:for-each select="AddressLine">
				<xsl:choose>
					<xsl:when test="contains(.,',')">
						<StreetNmbr>
							<xsl:value-of select="substring-before(.,',')"/>
						</StreetNmbr>
						<xsl:variable name="txt">
							<xsl:value-of select="substring-after(.,',')"/>
						</xsl:variable>
						<CityName>
							<xsl:value-of select="substring-before($txt,',')"/>
						</CityName>
						<xsl:variable name="txt1">
							<xsl:value-of select="substring-after($txt,',')"/>
						</xsl:variable>
						<PostalCode>
							<xsl:value-of select="substring-before($txt1,',')"/>
						</PostalCode>
						<xsl:variable name="txt2">
							<xsl:value-of select="substring-after($txt1,',')"/>
						</xsl:variable>
						<StateProv>
							<xsl:value-of select="substring-before($txt2,',')"/>
						</StateProv>
						<xsl:variable name="txt3">
							<xsl:value-of select="substring-after($txt2,',')"/>
						</xsl:variable>
						<CountryName>
							<xsl:attribute name="Code">
								<xsl:value-of select="$txt3"/>
							</xsl:attribute>
						</CountryName>
					</xsl:when>
					<xsl:otherwise>
						<AddressLine>
							<xsl:value-of select="."/>
						</AddressLine>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>

		</Address>
	</xsl:template>
	<!-- ***********************************************************-->
	<!--  				Ticketing info      			     -->
	<!-- ********************************************************** -->
	<xsl:template match="Ticketing" mode="Ticketing">
		<Ticketing>
			<xsl:variable name="ttl1">
				<xsl:value-of select="substring-after(../ItineraryPricing/TPA_Extensions/TicketRecordInfo/Text[contains(.,'LAST DAY TO PURCHASE')],'LAST DAY TO PURCHASE ')"/>
				<xsl:value-of select="substring-after(../ItineraryPricing/PriceQuote/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/ResTicketingRestrictions[contains(.,'LAST DAY TO PURCHASE')],'LAST DAY TO PURCHASE ')"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="substring(@TicketTimeLimit,1,3)='TAW' and string-length(@TicketTimeLimit)>4">
					<xsl:attribute name="TicketTimeLimit">
						<xsl:variable name="ttl">
							<xsl:value-of select="substring(@TicketTimeLimit,8,5)"/>
						</xsl:variable>
						<xsl:variable name="ttld">
							<xsl:value-of select="substring($ttl,1,2)"/>
						</xsl:variable>
						<xsl:variable name="ttlm">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($ttl,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="nd">
							<xsl:value-of select="substring(../../../ApplicationResults/Success/@timeStamp,9,2)"/>
						</xsl:variable>
						<xsl:variable name="nm">
							<xsl:value-of select="substring(../../../ApplicationResults/Success/@timeStamp,6,2)"/>
						</xsl:variable>
						<xsl:variable name="ny">

							<xsl:value-of select="substring(../ReservationItems/Item[1]/FlightSegment/@DepartureDateTime,1,4)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$ttlm &lt; $nm">
								<xsl:value-of select="$ny + 1"/>
								<xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:when>
							<xsl:when test="$ttlm = $nm and $ttld &lt; ($nd - 2)">
								<xsl:value-of select="$ny + 1"/>
								<xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@TicketTimeLimit='TAW/' and $ttl1!=''">
					<xsl:attribute name="TicketTimeLimit">
						<xsl:variable name="nd">
							<xsl:value-of select="substring($ttl1,1,2)"/>
						</xsl:variable>
						<xsl:variable name="nm">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($ttl1,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="ny">
							<xsl:value-of select="substring(../ReservationItems/Item/FlightSegment[1]/@DepartureDateTime,1,4)"/>
						</xsl:variable>
						<xsl:value-of select="concat($ny,'-',$nm,'-',$nd,'T23:59:00')"/>
					</xsl:attribute>
				</xsl:when>
				<!--xsl:when test="@TicketTimeLimit='TAW/' and $ttl1=''">
					<xsl:attribute name="TicketTimeLimit"><xsl:variable name="nd"><xsl:value-of select="substring(../ReservationItems/Item[1]/FlightSegment/@DepartureDateTime,9,2)"/></xsl:variable><xsl:variable name="nm"><xsl:value-of select="substring(../ReservationItems/Item[1]/Air/@DepartureDateTime,6,2)"/></xsl:variable><xsl:variable name="ny"><xsl:value-of select="substring(../ReservationItems/Item/FlightSegment[1]/@DepartureDateTime,1,4)"/></xsl:variable><xsl:value-of select="concat($ny,'-',$nm,'-',$nd,'T23:59:00')"/></xsl:attribute>
				</xsl:when-->
				<xsl:when test="substring(@TicketTimeLimit,1,2)='TL'">
					<xsl:attribute name="TicketTimeLimit">
						<xsl:variable name="ttl">
							<xsl:value-of select="substring-before(substring-after(@TicketTimeLimit,'/'),'-')"/>
						</xsl:variable>
						<xsl:variable name="ttld">
							<xsl:value-of select="substring($ttl,1,2)"/>
						</xsl:variable>
						<xsl:variable name="ttlm">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($ttl,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="nd">
							<xsl:value-of select="substring(../../../ApplicationResults/Success/@timeStamp,9,2)"/>
						</xsl:variable>
						<xsl:variable name="nm">
							<xsl:value-of select="substring(../../../ApplicationResults/Success/@timeStamp,6,2)"/>
						</xsl:variable>
						<xsl:variable name="ny">
							<xsl:value-of select="substring(../../../ApplicationResults/Success/@timeStamp,1,4)"/>
						</xsl:variable>
						<xsl:variable name="tm1">
							<xsl:value-of select="substring-before(substring(@TicketTimeLimit,3),'/')"/>
						</xsl:variable>
						<xsl:variable name="tm">
							<xsl:choose>
								<xsl:when test="contains($tm1,'P')">
									<xsl:variable name="tm2">
										<xsl:value-of select="substring-before($tm1,'P')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="substring($tm2,1,2)='12' and string-length($tm2)=4">
											<xsl:value-of select="$tm2"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$tm2 + 1200"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="contains($tm1,'A')">
									<xsl:variable name="tm2">
										<xsl:value-of select="substring-before($tm1,'A')"/>
									</xsl:variable>
									<xsl:value-of select="format-number($tm2,'0000')"/>
								</xsl:when>
								<xsl:when test="contains($tm1,'N')">
									<xsl:variable name="tm2">
										<xsl:value-of select="substring-before($tm1,'N')"/>
									</xsl:variable>
									<xsl:value-of select="format-number($tm2,'0000')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$ttlm &lt; $nm">
								<xsl:value-of select="$ny + 1"/>
								<xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
							</xsl:when>
							<xsl:when test="$ttlm = $nm and $ttld &lt; ($nd - 2)">
								<xsl:value-of select="$ny + 1"/>
								<xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
			<xsl:attribute name="TicketType">
				<xsl:choose>
					<xsl:when test="../ReservationItems/Item[1]/FlightSegment/@eTicket='true'">eTicket</xsl:when>
					<xsl:otherwise>Paper</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@TicketTimeLimit='TAW/'">
					<TicketingAdvisory>OK</TicketingAdvisory>
				</xsl:when>
				<xsl:when test="substring(@TicketTimeLimit,1,2)='T-' and substring(@TicketTimeLimit,8,1)='-'">
					<TicketAdvisory>
						<xsl:text>OK-</xsl:text>
						<xsl:variable name="ttl">
							<xsl:value-of select="substring(@TicketTimeLimit,3,5)"/>
						</xsl:variable>
						<xsl:variable name="ttld">
							<xsl:value-of select="substring($ttl,1,2)"/>
						</xsl:variable>
						<xsl:variable name="ttlm">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($ttl,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="nd">
							<xsl:value-of select="substring(../../../TimeStamp,9,2)"/>


						</xsl:variable>
						<xsl:variable name="nm">
							<xsl:value-of select="substring(../../../TimeStamp,6,2)"/>


						</xsl:variable>
						<xsl:variable name="ny">
							<xsl:value-of select="substring(../../../TimeStamp,1,4)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$ttlm &lt; $nm">
								<xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:when>
							<xsl:when test="$ttlm = $nm and $ttld &lt; $nd">
								<xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$ny - 1"/>
								<xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</TicketAdvisory>
				</xsl:when>
			</xsl:choose>
		</Ticketing>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Form of Payment Credit Card        						                               -->
	<!--************************************************************************************-->
	<xsl:template match="Payment/Form" mode="Text">
		<xsl:param name="accMCO"/>

		<xsl:if test="starts-with(Text, '*')">
			<xsl:variable name="cardText">
				<xsl:value-of select="translate(Text, translate(Text, '0123456789', ''), '')"/>
			</xsl:variable>

			<xsl:variable name="cardNum">
				<xsl:value-of select="substring($cardText, (substring($cardText,1,1)='1') +1)"/>
			</xsl:variable>

			<FormOfPayment>
				<xsl:attribute name="RPH">
					<xsl:value-of select="@RPH"/>
				</xsl:attribute>

				<xsl:if test="count($accMCO) > 0">
					<xsl:if test="$accMCO[Airline/@Code='XD']">
						<xsl:apply-templates select="$accMCO[Airline/@Code='XD']" mode="mco">
						</xsl:apply-templates>
					</xsl:if>
				</xsl:if>

				<xsl:choose>
					<xsl:when test="(Text='CHECK') or (Text='CASH')">
						<xsl:attribute name="RPH">
							<xsl:value-of select="format-number(@RPH,'#0')"/>
						</xsl:attribute>
						<DirectBill>
							<xsl:attribute name="DirectBill_ID">
								<xsl:value-of select="Text"/>
							</xsl:attribute>
						</DirectBill>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="string-length($cardNum) >= 8 and string-length(Text) > 15">
							
							<xsl:variable name="card">
								<xsl:choose>
									<xsl:when test="contains(Text, 'XXXX')">

										<xsl:variable name="cardID" select="substring(translate(substring-before(Text, '?'), 'X',''), string-length(translate(substring-before(Text, '?'), 'X','')) - 3)">
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="../../../../../PNR_HDK_FOP[contains(text(), $cardID)]">
												<xsl:variable name="hFOP" select="../../../../../PNR_HDK_FOP[contains(text(), $cardID)]"  />
												<xsl:value-of select="concat('*', $hFOP/@CCType, $hFOP/text(),'?', substring($hFOP/@Exp,1,2), '/', substring($hFOP/@Exp,3,2))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="Text"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Text"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>		

							<PaymentCard>
								<xsl:attribute name="CardCode">
									<xsl:choose>
										<xsl:when test="starts-with($card, '*') or starts-with($card, '-')">
											<xsl:choose>
												<xsl:when test="substring($card,2,2)='CA'">MC</xsl:when>
												<xsl:when test="substring($card,2,2)='DI'">DS</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring($card,2,2)"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="substring($card,1,2)='CA'">MC</xsl:when>
												<xsl:when test="substring($card,1,2)='DI'">DS</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring($card,1,2)"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="CardNumber">
									<xsl:choose>
										<xsl:when test="starts-with($card, '*') or starts-with($card, '-')">
											<xsl:choose>
												<xsl:when test="contains($card, 'Â')">
													<xsl:value-of select="substring-before(substring($card,4),'Â')"/>
												</xsl:when>
												<xsl:when test="contains($card, '¥')">
													<xsl:value-of select="substring-before(substring($card,4),'¥')"/>
												</xsl:when>
												<xsl:when test="contains($card, '?')">
													<xsl:value-of select="substring-before(substring($card,4),'?')"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:if test="contains($card, 'Â')">
												<xsl:value-of select="substring-before(substring($card,3),'Â')"/>
											</xsl:if>
											<xsl:if test="contains($card, '¥')">
												<xsl:value-of select="substring-before(substring($card,3),'¥')"/>
											</xsl:if>
											<xsl:if test="contains($card, '?')">
												<xsl:value-of select="substring-before(substring($card,3),'?')"/>
											</xsl:if>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="ExpireDate">
									<!-- 
                  *VI4XXXXXXXXXXX8882?03/20-XN
                  *VI4XXXXXXXXXXX1111?1/19 
                  *CA5XXXXXXXXXXX2120¥11/22
                  -->
									<xsl:variable name="exp">
										<xsl:choose>
											<xsl:when test="contains($card, '¥')">
												<xsl:value-of select="translate(substring(substring-after(substring($card,4),'¥'),0,6),'/','')"/>
											</xsl:when>
											<xsl:when test="contains($card, 'Â')">
												<xsl:value-of select="translate(substring(substring-after(substring($card,4),'Â'),0,6),'/','')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="translate(substring(substring-after(substring($card,4),'?'),0,6),'/','')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>

									<xsl:choose>
										<xsl:when test="string-length($exp)=3">
											<xsl:value-of select="concat(0,$exp)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$exp"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:attribute>
							</PaymentCard>

							<xsl:choose>
								<xsl:when test="contains($card,'/P')">
									<!--<xsl:if test="contains($card,'/P')">-->
									<xsl:variable name="pax" select="translate(substring-after($card, '/P'),'/P',',')" />
									<TPA_Extensions>
										<xsl:attribute name="FOPType">CC</xsl:attribute>
										<xsl:attribute name="RPH">
											<xsl:value-of select="$pax"/>
										</xsl:attribute>
										<!--
          ****************************************************************
          * May not work since Sabre sometimes returns someting like this:
          * *VI4444333322221111?10/17/P1.2-P1.4 - WILL NOT work
          * *VI4XXXXXXXXXXX8882?03/20-XN  - WILL WORK
          ****************************************************************
          <xsl:if test="contains($card,'-')">
            <xsl:attribute name="ConfirmationNumber">
              <xsl:value-of select="substring-after($card,'-')"/>
            </xsl:attribute>
          </xsl:if>
          -->
									</TPA_Extensions>
								</xsl:when>
								<xsl:otherwise>
									<TPA_Extensions>
										<xsl:attribute name="FOPType">CC</xsl:attribute>
										<xsl:attribute name="RPH">
											<xsl:value-of select="1"/>
										</xsl:attribute>
									</TPA_Extensions>
								</xsl:otherwise>
								<!--</xsl:if>-->
							</xsl:choose>

						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>

			</FormOfPayment>
		</xsl:if>
	</xsl:template>

	<xsl:template match="FormOfPayment" >
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()"/>
		</xsl:attribute>
		<PaymentCard>
			<xsl:attribute name="CardCode">
				<xsl:choose>
					<xsl:when test="PaymentCard/@CardType='CA'">MC</xsl:when>
					<xsl:when test="PaymentCard/@CardType='DI'">DS</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="PaymentCard/@CardType"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="CardNumber">
				<xsl:variable name="cardnum">
					<xsl:value-of select="PaymentCard/@CardNumber"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="string-length($cardnum) = 14">
						<xsl:text>xxxxxxxxxx</xsl:text>
						<xsl:value-of select="substring($cardnum,11)"/>
					</xsl:when>
					<xsl:when test="string-length($cardnum) = 15">
						<xsl:text>xxxxxxxxxxx</xsl:text>
						<xsl:value-of select="substring($cardnum,12)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>xxxxxxxxxxxx</xsl:text>
						<xsl:value-of select="substring($cardnum,13)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ExpireDate">
				<xsl:value-of select="PaymentCard/@ExpireDate"/>
			</xsl:attribute>
		</PaymentCard>
		<!--Note:  TPA_Extensions/@FOPType needs to be put on the schema so that it validates -->
		<TPA_Extensions>
			<xsl:attribute name="FOPType">CC</xsl:attribute>
		</TPA_Extensions>
	</xsl:template>

	<xsl:template match="PaymentForm">
		<FormOfPayment>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<PaymentCard>
				<xsl:attribute name="CardCode">
					<xsl:choose>
						<xsl:when test="substring(TPA_Extensions/Text,2,2)='CA'">MC</xsl:when>
						<xsl:when test="substring(TPA_Extensions/Text,2,2)='DI'">DS</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(TPA_Extensions/Text,2,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="CardNumber">
					<xsl:variable name="cardnum">
						<xsl:choose>
							<xsl:when test="substring(TPA_Extensions/Text,2,2) = 'AX'">
								<xsl:value-of select="substring(TPA_Extensions/Text,4,15)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(TPA_Extensions/Text,4,16)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="string-length($cardnum) = 14">
							<xsl:text>xxxxxxxxxx</xsl:text>
							<xsl:value-of select="substring($cardnum,11)"/>
						</xsl:when>
						<xsl:when test="string-length($cardnum) = 15">
							<xsl:text>xxxxxxxxxxx</xsl:text>
							<xsl:value-of select="substring($cardnum,12)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>xxxxxxxxxxxx</xsl:text>
							<xsl:value-of select="substring($cardnum,13)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="ExpireDate">
					<xsl:variable name="temp">
						<xsl:value-of select="substring-before(TPA_Extensions/Text,'/')"/>
					</xsl:variable>
					<xsl:variable name="templ">
						<xsl:value-of select="string-length($temp)"/>
					</xsl:variable>
					<xsl:value-of select="substring(TPA_Extensions/Text,$templ - 1,2)"/>
					<xsl:value-of select="substring-after(TPA_Extensions/Text,'/')"/>
				</xsl:attribute>
			</PaymentCard>
			<!--Note:  TPA_Extensions/@FOPType needs to be put on the schema so that it validates -->
			<TPA_Extensions>
				<xsl:attribute name="FOPType">CC</xsl:attribute>
			</TPA_Extensions>
		</FormOfPayment>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Other Service Information (OSI) Processing				    -->
	<!--************************************************************************************-->
	<xsl:template match="Service" mode="OSI">
		<OtherServiceInformation>
			<Airline>
				<xsl:choose>
					<xsl:when test="Airline/@Code">
						<xsl:value-of select="Airline/@Code"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(Text,1,2)"/>
					</xsl:otherwise>
				</xsl:choose>
			</Airline>
			<Text>
				<xsl:value-of select="Text"/>
			</Text>
		</OtherServiceInformation>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Special Service Request (SSR) Processing				    -->
	<!--************************************************************************************-->
	<xsl:template match="Service" mode="SSR">
		<SpecialServiceRequest>
			<xsl:attribute name="SSRCode">
				<xsl:value-of select="@SSR_Type"/>
			</xsl:attribute>
			<xsl:if test="PersonName/@NameNumber != ''">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:value-of select="PersonName/@NameNumber"/>
				</xsl:attribute>
			</xsl:if>
			<Airline>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="Airline/@Code">
							<xsl:value-of select="Airline/@Code"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(Text,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</Airline>
			<Text>
				<xsl:value-of select="Text"/>
			</Text>
		</SpecialServiceRequest>
	</xsl:template>
	<!--************************************************************************************-->
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
		</xsl:choose>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- TourCode Remarks	   	                                          -->
	<!-- ************************************************************** -->
	<xsl:template match="PriceQuote" mode="TourCode">
		<SpecialRemark>
			<xsl:variable name="pq">
				<xsl:value-of select="@RPH"/>
			</xsl:variable>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pq"/>
			</xsl:attribute>
			<xsl:variable name="tc">
				<xsl:choose>
					<xsl:when test="PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/TourCode/Text">
						<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/TourCode/Text"/>
					</xsl:when>
					<xsl:when test="../../../ItineraryInfo/ItineraryPricing/FuturePriceInfo/Text">
						<xsl:variable name="futurePricing">
							<xsl:value-of select="../../../ItineraryInfo/ItineraryPricing/FuturePriceInfo[contains(Text, concat('PQ',$pq))]/Text"/>
						</xsl:variable>

						<xsl:if test="contains($futurePricing, 'UN*')">
							<xsl:choose>
								<xsl:when test="contains($futurePricing, 'Â') or contains($futurePricing, '‡')">
									<xsl:variable name="tk" select="substring-after($futurePricing, '‡UN*')" />
									<xsl:choose>
										<xsl:when test="contains($tk, 'Â‡')">
											<xsl:value-of select="substring-before(substring-after($futurePricing, '‡UN*'), 'Â‡')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after($futurePricing, '‡UN*')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="futurePriceElement">
										<xsl:with-param name="string" select="$futurePricing" />
										<xsl:with-param name="elem" select="'UN*'" />
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>

					</xsl:when>
				</xsl:choose>
			</xsl:variable>

			<xsl:choose>
				<xsl:when test="string-length($tc) > 0">
					<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
					<TravelerRefNumber>
						<xsl:attribute name="RPH">
							<xsl:for-each select="PriceQuotePlus/PassengerInfo/PassengerData">
								<xsl:variable name="rph">
									<xsl:value-of select="@NameNumber"/>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:value-of select="/TravelItineraryReadRS/TravelItinerary/CustomerInfo/PersonName[@NameNumber=$rph]/@RPH"/>
							</xsl:for-each>
						</xsl:attribute>
					</TravelerRefNumber>

					<xsl:variable name="fltRef">
						<xsl:for-each select="PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment">
							<xsl:variable name="rph">
								<xsl:choose>
									<xsl:when test="@SegmentNumber">
										<xsl:value-of select="format-number(@SegmentNumber,'#')"/>
									</xsl:when>
									<xsl:when test="@RPH">
										<xsl:value-of select="format-number(@RPH,'#')"/>
									</xsl:when>
								</xsl:choose>

							</xsl:variable>
							<xsl:if test="position() > 1">
								<xsl:text> </xsl:text>
							</xsl:if>
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="$rph" />
							</xsl:call-template>
						</xsl:for-each>
					</xsl:variable>

					<FlightRefNumber>
						<xsl:attribute name="FlightRefNumber">
							<xsl:call-template name="string-trim">
								<xsl:with-param name="string" select="$fltRef" />
							</xsl:call-template>
						</xsl:attribute>
					</FlightRefNumber>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<Text>
				<xsl:value-of select="translate($tc, '*', '')"/>
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Endorsement Remarks	   	                              -->
	<!-- ************************************************************** -->
	<xsl:template match="PriceQuote" mode="Endorsement">
		<SpecialRemark>
			<xsl:attribute name="RPH">
				<xsl:value-of select="@RPH"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='SYSTEM_ENDORSEMENT']/Text">
					<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>

					<TravelerRefNumber>
						<xsl:attribute name="RPH">
							<xsl:for-each select="PriceQuotePlus/PassengerInfo/PassengerData">
								<xsl:variable name="rph">
									<xsl:value-of select="@NameNumber"/>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:value-of select="/TravelItineraryReadRS/TravelItinerary/CustomerInfo/PersonName[@NameNumber=$rph]/@RPH"/>
							</xsl:for-each>
						</xsl:attribute>
					</TravelerRefNumber>

					<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:for-each select="../../../../DisplayPriceQuoteRS/PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment">
								<xsl:variable name="rph">
									<xsl:if test="@RPH">
										<xsl:value-of select="format-number(@RPH,'#')"/>
									</xsl:if>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:call-template name="string-trim">
									<xsl:with-param name="string" select="$rph" />
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
				<xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/Endorsements/Endorsement[@type='SYSTEM_ENDORSEMENT']/Text"/>
			</Text>
		</SpecialRemark>
	</xsl:template>

	<xsl:template match="FareCalculationBreakdown" mode="controllingCarrier">
		<xsl:variable select="FareBasis/@FilingCarrier" name="airline" />
		<xsl:if test="string-length($airline) = 2">
			<xsl:variable name="flts">
				<xsl:value-of select="concat(Departure/@AirportCode,Departure/@ArrivalAirportCode)" />
			</xsl:variable>

			<SpecialRemark>
				<xsl:attribute name="RPH">
					<xsl:value-of select="$airline"/>
				</xsl:attribute>
				<xsl:attribute name="RemarkType">CC</xsl:attribute>
				<FlightRefNumber>
					<xsl:attribute name="RPH">
						<xsl:for-each select="//TravelItinerary/ItineraryInfo/ReservationItems/Item[FlightSegment]">
							<xsl:variable name="flt">
								<xsl:choose>
									<xsl:when test="count(FlightSegment) > 1">
										<xsl:value-of select="concat(FlightSegment[1]/OriginLocation/@LocationCode,FlightSegment[last()]/DestinationLocation/@LocationCode)" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(FlightSegment/OriginLocation/@LocationCode,FlightSegment/DestinationLocation/@LocationCode)" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:if test="contains($flts, $flt)">
								<xsl:variable name="rph">
									<xsl:if test="@RPH">
										<xsl:value-of select="format-number(./@RPH,'#')"/>
									</xsl:if>
								</xsl:variable>
								<!--<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>-->
								<xsl:call-template name="string-trim">
									<xsl:with-param name="string" select="$rph" />
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:attribute>
				</FlightRefNumber>
				<Text>
					<xsl:value-of select="concat($flts,'/',FareBasis/@FilingCarrier)"/>
				</Text>
			</SpecialRemark>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Endorsement" mode="controllingCarrier">
		<xsl:variable select="substring-after(Text, '/')" name="airline" />
		<xsl:if test="string-length($airline) = 2">

			<xsl:variable name="elems">
				<xsl:call-template name="tokenizeString">
					<xsl:with-param name="list" select="substring-before(Text, '-')"/>
					<xsl:with-param name="delimiter" select="' '"/>
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="flts">
				<xsl:call-template name="ListToString">
					<xsl:with-param name="list" select="msxsl:node-set($elems)/elem/node()[1]"/>
					<xsl:with-param name="delem" select="','" />
				</xsl:call-template>
			</xsl:variable>

			<SpecialRemark>
				<xsl:attribute name="RPH">
					<xsl:value-of select="$airline"/>
				</xsl:attribute>
				<xsl:attribute name="RemarkType">CC</xsl:attribute>
				<FlightRefNumber>
					<xsl:attribute name="RPH">
						<xsl:for-each select="../../../../../../../ReservationItems/Item[FlightSegment]">
							<xsl:variable name="flt">
								<xsl:choose>
									<xsl:when test="count(FlightSegment) > 1">
										<xsl:value-of select="concat(FlightSegment[1]/OriginLocation/@LocationCode,FlightSegment[last()]/DestinationLocation/@LocationCode)" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(FlightSegment/OriginLocation/@LocationCode,FlightSegment/DestinationLocation/@LocationCode)" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:if test="contains($flts, $flt)">
								<xsl:variable name="rph">
									<xsl:if test="@RPH">
										<xsl:value-of select="format-number(./@RPH,'#')"/>
									</xsl:if>
								</xsl:variable>
								<xsl:if test="position() > 1">
									<xsl:text> </xsl:text>
								</xsl:if>
								<xsl:call-template name="string-trim">
									<xsl:with-param name="string" select="$rph" />
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:attribute>
				</FlightRefNumber>
				<Text>
					<xsl:value-of select="Text"/>
				</Text>
			</SpecialRemark>
		</xsl:if>
	</xsl:template>

	<!-- ********************************************************************************* -->
	<!-- Miscellaneous other               -->
	<!-- ********************************************************************************* -->
	<xsl:template match="TPA_Extensions" mode="Other">
		<!--General Segment in OTA -->
		<!-- Other Segments -->
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="Line/@Status"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:choose>
					<xsl:when test="substring(Line/@Number,1,3)='000'">
						<xsl:value-of select="substring(Line/@Number,4)"/>
					</xsl:when>
					<xsl:when test="substring(Line/@Number,1,2)='00'">
						<xsl:value-of select="substring(Line/@Number,3)"/>
					</xsl:when>
					<xsl:when test="substring(Line/@Number,1,1)='0'">
						<xsl:value-of select="substring(Line/@Number,2)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="Line/@Number"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:value-of select="substring(DateTime,1,10)"/>
				</xsl:attribute>
				<Description>
					<xsl:value-of select="Text"/>
				</Description>
				<TPA_Extensions>
					<xsl:attribute name="Status">
						<xsl:value-of select="Line/@Status"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="substring(NumberInParty,2)"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="Vendor/@Code"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>
						<xsl:value-of select="Location/@LocationCode"/>
					</OriginCityCode>
				</TPA_Extensions>
			</General>
		</Item>
	</xsl:template>

	<!-- ********************************************************************************* -->
	<!-- Miscellaneous Charges               -->
	<!-- ********************************************************************************* -->
	<xsl:template match="AccountingInfo" mode="mco">
		<!--General Segment in OTA -->
		<xsl:for-each select=".">
			<xsl:variable name="tktNum">
				<xsl:value-of select="DocumentInfo/Document/@Number"/>
			</xsl:variable>
			<MiscChargeOrder>
				<xsl:attribute name="TicketNumber">
					<xsl:value-of  select="$tktNum" />
				</xsl:attribute>
				<xsl:value-of  select="../ItineraryInfo/Ticketing[contains(@eTicketNumber,$tktNum)]/@eTicketNumber" />
			</MiscChargeOrder>
		</xsl:for-each>
	</xsl:template>

	<!-- ************************************************************** -->
	<!-- Replace a string 	   	                                          -->
	<!-- ************************************************************** -->
	<xsl:template name="string-replace-all">
		<xsl:param name="text" />
		<xsl:param name="replace" />
		<xsl:param name="by" />
		<xsl:choose>
			<xsl:when test="contains($text, $replace)">
				<xsl:value-of select="substring-before($text,$replace)" />
				<xsl:value-of select="$by" />
				<xsl:call-template name="string-replace-all">
					<xsl:with-param name="text"
					select="substring-after($text,$replace)" />
					<xsl:with-param name="replace" select="$replace" />
					<xsl:with-param name="by" select="$by" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$text" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="rphFormat">
		<xsl:param name="rn" />
		<xsl:variable name="whole" select="format-number(number(substring-before($rn, '.')),'00')"/>
		<xsl:variable name="tenth" select="format-number(number(substring-after($rn, '.')),'00')"/>

		<xsl:value-of select="concat($whole,'.',$tenth)" />
	</xsl:template>

	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

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

	<xsl:template name="displayNumbers">
		<xsl:param name="pStart"/>
		<xsl:param name="pEnd"/>
		<xsl:param name="delem"/>

		<xsl:if test="not($pStart > $pEnd)">
			<xsl:choose>
				<xsl:when test="$pStart = $pEnd">
					<xsl:value-of select="$pStart"/>
					<xsl:choose>
						<xsl:when test="$delem!=''" >
							<xsl:value-of select="$delem"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>&#xA;</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="vMid" select=
           "floor(($pStart + $pEnd) div 2)"/>
					<xsl:call-template name="displayNumbers">
						<xsl:with-param name="pStart" select="$pStart"/>
						<xsl:with-param name="pEnd" select="$vMid"/>
						<xsl:with-param name="delem" select="$delem"/>
					</xsl:call-template>
					<xsl:call-template name="displayNumbers">
						<xsl:with-param name="pStart" select="$vMid+1"/>
						<xsl:with-param name="pEnd" select="$pEnd"/>
						<xsl:with-param name="delem" select="$delem"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ListToString">
		<xsl:param name="list"/>
		<xsl:param name="delem"/>
		<xsl:for-each select="$list">
			<xsl:value-of select="concat(., substring($delem, 2 - (position() != last())))"/>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="futurePriceElement">
		<xsl:param name="string" />
		<xsl:param name="elem" />

		<xsl:choose>
			<xsl:when test="contains($string, '/')">
				<xsl:variable name="value">
					<xsl:call-template name="string-trim">
						<xsl:with-param name="string" select="substring-before(substring-after($string, $elem), '/')" />
					</xsl:call-template>
				</xsl:variable>
				<xsl:value-of select="substring($value, 0, string-length($value) - 2)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-after($string, $elem)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>



</xsl:stylesheet>
