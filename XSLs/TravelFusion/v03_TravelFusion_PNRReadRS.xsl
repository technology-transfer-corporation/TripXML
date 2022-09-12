<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ================================================================== -->
<!-- v03_TravelFusion_PNRReadRS.xsl 												       -->
<!-- ================================================================== -->
<!-- Date: 21 Nov 2012 - Rastko  - display airline record locator if returned in response	-->
<!-- Date: 30 Oct 2012 - Rastko  - mapped ID_Context								-->
<!-- Date: 09 Apr 2012 - Rastko  - new file											-->
<!-- ================================================================== -->
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="PNR_Reply" />
	<xsl:apply-templates select="Errors" />
</xsl:template>

<xsl:template match="etext">
	<OTA_TravelItineraryRS Version="v03">
		<Errors>
			<Error Type="TravelFusion">
				<xsl:value-of select="."/>
			</Error>
		</Errors>
	</OTA_TravelItineraryRS>
</xsl:template>

<xsl:template match="Errors">
	<OTA_TravelItineraryRS Version="v03">
		<Errors>
			<Error Type="TravelFusion">
				<xsl:value-of select="Error"/>
			</Error>
		</Errors>
	</OTA_TravelItineraryRS>
</xsl:template>
	
<xsl:template match="PNR_Reply">
	<OTA_TravelItineraryRS>
		<xsl:attribute name="Version"><xsl:value-of select="'v03'"/></xsl:attribute>
		<xsl:if test="EchoToken!=''">
			<xsl:attribute name="EchoToken"><xsl:value-of select="EchoToken"/></xsl:attribute>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="CommandList/GetBookingDetails">
				<Success/>
				<xsl:if test="Error or Warning">
					<Warnings>
						<xsl:apply-templates select="Error" mode="warning"/>
						<xsl:apply-templates select="Warning" mode="warning"/>
					</Warnings>
				</xsl:if>
				<TravelItinerary>	
					<ItineraryRef>
						<xsl:attribute name="Type">PNR</xsl:attribute>
						<xsl:attribute name="ID">
							<xsl:choose>
								<xsl:when test="CommandList/GetBookingDetails/SupplierReference!=''">
									<xsl:value-of select="CommandList/GetBookingDetails/SupplierReference"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="REQPNR"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="ID_Context">
							<xsl:choose>
								<xsl:when test="CommandList/GetBookingDetails/Status">
									<xsl:value-of select="CommandList/GetBookingDetails/Status"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'Unknown'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</ItineraryRef> 
					<CustomerInfos>
						<xsl:apply-templates select="CommandList/GetBookingDetails/BookingProfile/TravellerList/Traveller"/>	
					</CustomerInfos>
					<ItineraryInfo>							
						<ReservationItems>	
							<xsl:variable name="reqpnr">
								<xsl:choose>
									<xsl:when test="CommandList/GetBookingDetails/SupplierReference!=''">
										<xsl:value-of select="CommandList/GetBookingDetails/SupplierReference"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="REQPNR"/></xsl:otherwise>
								</xsl:choose>
							</xsl:variable>							
							<xsl:apply-templates select="CommandList/GetBookingDetails/RouterHistory/TermsRouter/GroupList/Group/OutwardList/Outward/SegmentList/Segment" mode="Air">
								<xsl:with-param name="pnrref" select="$reqpnr"/>
								<xsl:with-param name="nip"><xsl:value-of select="count(CommandList/GetBookingDetails/BookingProfile/TravellerList/Traveller[Age!='0'])"/></xsl:with-param>
								<xsl:with-param name="rph"><xsl:value-of select="0"/></xsl:with-param>
							</xsl:apply-templates>
							<xsl:apply-templates select="CommandList/GetBookingDetails/RouterHistory/TermsRouter/GroupList/Group/ReturnList/Return/SegmentList/Segment" mode="Air">
								<xsl:with-param name="pnrref" select="$reqpnr"/>
								<xsl:with-param name="nip"><xsl:value-of select="count(CommandList/GetBookingDetails/BookingProfile/TravellerList/Traveller[Age!='0'])"/></xsl:with-param>
								<xsl:with-param name="rph"><xsl:value-of select="count(CommandList/GetBookingDetails/RouterHistory/TermsRouter/GroupList/Group/OutwardList/Outward/SegmentList/Segment)"/></xsl:with-param>
							</xsl:apply-templates>
							<ItemPricing>
								<xsl:variable name="priceditin">
									<xsl:choose>
										<xsl:when test="CommandList/GetBookingDetails/RouterHistory/SearchRouter/GroupList/Group/ReturnList/Return">
											<xsl:apply-templates select="CommandList/GetBookingDetails/RouterHistory/SearchRouter/GroupList" mode="circle"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:apply-templates select="CommandList/GetBookingDetails/RouterHistory/SearchRouter/GroupList" mode="oneway"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:apply-templates select="msxsl:node-set($priceditin)/AirFareInfo" mode="final">
									<xsl:sort data-type="number" order="ascending" select="AirFareInfo/ItinTotalFare/TotalFare/@Amount"/>
								</xsl:apply-templates>
							</ItemPricing>		
						</ReservationItems>
						<xsl:apply-templates select="CommandList/GetBookingDetails/RouterHistory/TermsRouter/GroupList/Group/OutwardList/Outward/SegmentList" mode="ticketing"/>
						<!--SpecialRequestDetails>
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
						</SpecialRequestDetails-->	
						<!--xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV' or elementManagementData/segmentName='FO' or elementManagementData/segmentName='FA' or elementManagementData/segmentName='FB' or elementManagementData/segmentName='FI' or substring(elementManagementData/segmentName,1,2)='FH']">
							<TPA_Extensions>
								<IssuedTickets>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA' or elementManagementData/segmentName='FB']" mode="IssuedTicket"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHA']" mode="AutomatedTicket"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHE']" mode="ElectronicTicket"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHM']" mode="ManualTicket"/>
									<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV']" mode="TicketingCarrier"/>
								</IssuedTickets>
							</TPA_Extensions>
						</xsl:if-->
					</ItineraryInfo>	
					<xsl:if test="CommandList/GetBookingDetails/BookingProfile/BillingDetails/CreditCard/Number!=''">
						<TravelCost>
							<xsl:apply-templates select="CommandList/GetBookingDetails/BookingProfile/BillingDetails/CreditCard" mode="Payment"/>
						</TravelCost>
					</xsl:if>		
					<UpdatedBy>
						<xsl:attribute name="CreateDateTime">
							<xsl:value-of select="format-number(substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,7,2),'2000')"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,4,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,1,2)"/>
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,10,2)"/>
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,13,2)"/>
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring(CommandList/GeneralInfoItemList/GeneralInfoItem[Name='StartTime']/Value,16,2)"/>
						</xsl:attribute>
					</UpdatedBy>
					<!--xsl:if test="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']">
						<TPA_Extensions>
							<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']" mode="accounting"/>
						</TPA_Extensions>
					</xsl:if-->
				</TravelItinerary>			
			</xsl:when>			
			<xsl:when test="CommandList/CommandExecutionFailure/GetBookingDetails/@etext!=''">
				<Errors>
					<Error Type="TravelFusion">
						<xsl:value-of select="CommandList/CommandExecutionFailure/GetBookingDetails/@etext"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<Error Type="TravelFusion">
						<xsl:value-of select="Error"/>
					</Error>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</OTA_TravelItineraryRS>		
</xsl:template>	

<xsl:template match="AirFareInfo" mode="final">
	<xsl:copy-of select="." />
</xsl:template>

<xsl:template match="Error" mode="error">
	<Error Type="TravelFusion">
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
	<!-- Process Names			                            -->
	<!-- ************************************************************** -->
	<xsl:template match="Traveller">
	   <CustomerInfo>
		   <xsl:attribute name="RPH"><xsl:value-of select="position()"/></xsl:attribute>
		   <Customer>
		   		<xsl:if test="CustomSupplierParameterList/CustomSupplierParameter/Value!=''">
			   		<xsl:attribute name="BirthDate">
						<xsl:value-of select="substring(CustomSupplierParameterList/CustomSupplierParameter/Value,7)"/>
						<xsl:value-of select="'-'"/>
						<xsl:value-of select="substring(CustomSupplierParameterList/CustomSupplierParameter/Value,4,2)"/>
						<xsl:value-of select="'-'"/>
						<xsl:value-of select="substring(CustomSupplierParameterList/CustomSupplierParameter/Value,4,2)"/>
			   		</xsl:attribute>
			   	</xsl:if>
			   	<PersonName>
				   	<xsl:attribute name="NameType">
				   		<xsl:choose>
							<xsl:when test="Age='0'">INF</xsl:when>
							<xsl:when test="Age='7'">CHD</xsl:when>
							<xsl:otherwise>ADT</xsl:otherwise>
						</xsl:choose>
				   	</xsl:attribute>
				  	 <GivenName>
		  	 			<xsl:value-of select="Name/NamePartList/NamePart[1]"/>
					</GivenName>			
					<Surname>						
						<xsl:value-of select="Name/NamePartList/NamePart[position()=2]"/>
					</Surname>			
			   	</PersonName> 
			   	<xsl:if test="../../ContactDetails/HomePhone/Number!=''">
			   		<Telephone>
						<xsl:attribute name="PhoneUseType">H</xsl:attribute>
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="../../ContactDetails/HomePhone/InternationalCode"/>
							<xsl:value-of select="../../ContactDetails/HomePhone/AreaCode"/>
							<xsl:value-of select="../../ContactDetails/HomePhone/Number"/>
							<xsl:if test="../../ContactDetails/HomePhone/Extension !=''">
								<xsl:value-of select="concat('-',../../ContactDetails/HomePhone/Extension)"/>
							</xsl:if>
						</xsl:attribute>		
					</Telephone>
			   	</xsl:if>
			   	<xsl:if test="../../ContactDetails/WorkPhone/Number!=''">
			   		<Telephone>
						<xsl:attribute name="PhoneUseType">B</xsl:attribute>
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="../../ContactDetails/WorkPhone/InternationalCode"/>
							<xsl:value-of select="../../ContactDetails/WorkPhone/AreaCode"/>
							<xsl:value-of select="../../ContactDetails/WorkPhone/Number"/>
							<xsl:if test="../../ContactDetails/WorkPhone/Extension !=''">
								<xsl:value-of select="concat('-',../../ContactDetails/WorkPhone/Extension)"/>
							</xsl:if>
						</xsl:attribute>		
					</Telephone>
			   	</xsl:if>
			   	<xsl:if test="../../ContactDetails/MobilePhone/Number!=''">
			   		<Telephone>
						<xsl:attribute name="PhoneUseType">M</xsl:attribute>
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="../../ContactDetails/MobilePhone/InternationalCode"/>
							<xsl:value-of select="../../ContactDetails/MobilePhone/AreaCode"/>
							<xsl:value-of select="../../ContactDetails/MobilePhone/Number"/>
							<xsl:if test="../../ContactDetails/MobilePhone/Extension !=''">
								<xsl:value-of select="concat('-',../../ContactDetails/MobilePhone/Extension)"/>
							</xsl:if>
						</xsl:attribute>		
					</Telephone>
			   	</xsl:if>
			   	<xsl:if test="../../ContactDetails/Fax/Number!=''">
			   		<Telephone>
						<xsl:attribute name="PhoneUseType">F</xsl:attribute>
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="../../ContactDetails/Fax/InternationalCode"/>
							<xsl:value-of select="../../ContactDetails/Fax/AreaCode"/>
							<xsl:value-of select="../../ContactDetails/Fax/Number"/>
							<xsl:if test="../../ContactDetails/Fax/Extension !=''">
								<xsl:value-of select="concat('-',../../ContactDetails/Fax/Extension)"/>
							</xsl:if>
						</xsl:attribute>		
					</Telephone>
			   	</xsl:if>
			  	<Email><xsl:value-of select="../../ContactDetails/Email"/></Email>
			  	<xsl:if test="position()=1">
			  		<xsl:if test="../../ContactDetails/Address/Street!=''">
			  			<Address>
							<xsl:attribute name="UseType">Mailing</xsl:attribute>
							<StreetNmbr>
								<xsl:value-of select="../../ContactDetails/Address/Street"/>
							</StreetNmbr>
							<CityName>
								<xsl:value-of select="../../ContactDetails/Address/City"/>
							</CityName>
							<PostalCode>
								<xsl:value-of select="../../ContactDetails/Address/Postcode"/>
							</PostalCode>
							<xsl:if test="../../ContactDetails/Address/Province!=''">
								<StateProv>
									<xsl:choose>
										<xsl:when test="string-length(../../ContactDetails/Address/Province)=2">
											<xsl:attribute name="StateCode">
												<xsl:value-of select="../../ContactDetails/Address/Province"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../ContactDetails/Address/Province"/></xsl:otherwise>
									</xsl:choose>
								</StateProv>
							</xsl:if>
							<CountryName>
								<xsl:attribute name="Code"><xsl:value-of select="../../ContactDetails/Address/CountryCode"/></xsl:attribute>
							</CountryName>
						</Address>
			  		</xsl:if>
					<Address>
						<xsl:attribute name="UseType">Billing</xsl:attribute>
						<StreetNmbr>
								<xsl:value-of select="../../BillingDetails/Address/Street"/>
							</StreetNmbr>
							<CityName>
								<xsl:value-of select="../../BillingDetails/Address/City"/>
							</CityName>
							<PostalCode>
								<xsl:value-of select="../../BillingDetails/Address/Postcode"/>
							</PostalCode>
							<xsl:if test="../../BillingDetails/Address/Province!=''">
								<StateProv>
									<xsl:choose>
										<xsl:when test="string-length(../../BillingDetails/Address/Province)=2">
											<xsl:attribute name="StateCode">
												<xsl:value-of select="../../BillingDetails/Address/Province"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../BillingDetails/Address/Province"/></xsl:otherwise>
									</xsl:choose>
								</StateProv>
							</xsl:if>
							<CountryName>
								<xsl:attribute name="Code"><xsl:value-of select="../../BillingDetails/Address/CountryCode"/></xsl:attribute>
							</CountryName>
					</Address>
				</xsl:if>
		   </Customer>
		</CustomerInfo>
	</xsl:template>
	<!-- ****************************************************************************************************************** -->
	<!-- Process Itinerary				 							                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Air Segments    				                    -->
	<!-- ************************************************************** -->
	<xsl:template match="Segment" mode="Air">
		<xsl:param name="pnrref"/>
		<xsl:param name="nip"/>
		<xsl:param name="rph"/>
		<Item>
			<xsl:attribute name="Status"><xsl:value-of select="'HK'"/></xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:value-of select="position() + $rph"/>		
			</xsl:attribute>
			<Air>			
				<xsl:attribute name="DepartureDateTime">	
					<xsl:value-of select="substring(DepartDate,7,4)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(DepartDate,4,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(DepartDate,1,2)"/>
					<xsl:text>T</xsl:text>
					<xsl:value-of select="substring(DepartDate,12,5)"/>
					<xsl:text>:00</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="ArrivalDateTime">
					<xsl:value-of select="substring(ArriveDate,7,4)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(ArriveDate,4,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(ArriveDate,1,2)"/>
					<xsl:text>T</xsl:text>
					<xsl:value-of select="substring(ArriveDate,12,5)"/>
					<xsl:text>:00</xsl:text>
				</xsl:attribute>
				<xsl:if test="SegmentMayEndWithAStop= 'true'">
					<xsl:attribute name="StopQuantity">1</xsl:attribute>
				</xsl:if>
				<xsl:attribute name="RPH"> 
					<xsl:value-of select="position() + $rph"/>
				</xsl:attribute>
				<xsl:attribute name="FlightNumber">				
					<xsl:value-of select="FlightId/Number"/>
				</xsl:attribute>
				<xsl:attribute name="ResBookDesigCode">
					<xsl:value-of select="TravelClass/SupplierClass"/>
				</xsl:attribute>
				<xsl:attribute name="NumberInParty">
					<xsl:value-of select="$nip"/>
				</xsl:attribute>
				<xsl:attribute name="Status"> 
						<xsl:value-of select="'HK'"/>
				</xsl:attribute>
				<xsl:attribute name="E_TicketEligibility">Eligible</xsl:attribute>
				<DepartureAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="Origin/Code"/>								
					</xsl:attribute> 		
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="Destination/Code"/>
					</xsl:attribute> 						
				</ArrivalAirport>		
				<OperatingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="Operator/Code"/>
					</xsl:attribute>
					<xsl:value-of select="Operator/Name"/>
				</OperatingAirline>
				<Equipment>
					<xsl:attribute name="AirEquipType">JET</xsl:attribute>
				</Equipment>
				<MarketingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="VendingOperator/Code"/>
					</xsl:attribute>
					<xsl:value-of select="VendingOperator/Name"/>
				</MarketingAirline>
				<TPA_Extensions>
					<xsl:attribute name="ConfirmationNumber"><xsl:value-of select="$pnrref"/></xsl:attribute>
				</TPA_Extensions>
			</Air>	
		</Item>
	</xsl:template>
	
	
	<!-- ****************************************************************************************************************** -->
	<!-- PNR Data Elements   	              								                -->
	<!-- ****************************************************************************************************************** -->
	<!-- Ticketing Element	   	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="SegmentList" mode="ticketing">
		<Ticketing>		
			<xsl:attribute name="TicketTimeLimit">				
				<xsl:variable name="depday">
					<xsl:value-of select="substring(Segment[1]/DepartDate,1,2)" />
				</xsl:variable>
				<xsl:variable name="depmonth">
					<xsl:value-of select="substring(Segment[1]/DepartDate,4,2)" />
				</xsl:variable>
				<xsl:variable name="depyear">
					<xsl:value-of select="substring(Segment[1]/DepartDate,7,4)" />
				</xsl:variable>
				<xsl:value-of select="$depyear" />-<xsl:value-of select="$depmonth" />-<xsl:value-of select="$depday" />T<xsl:value-of select="substring-after(Segment[1]/DepartDate,'-')" />
				<xsl:text>:00</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="TicketType">eTicket</xsl:attribute>	
		</Ticketing>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Form of Payment	   		                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="CreditCard" mode="Payment">
		<FormOfPayment>
			<PaymentCard>
				<xsl:attribute name="CardCode">
					<xsl:choose>
						<xsl:when test="CardType='Visa Credit'">VI</xsl:when>
						<xsl:when test="CardType='MasterCard'">MC</xsl:when>
						<xsl:when test="CardType='American Express'">AX</xsl:when>
						<xsl:when test="CardType='Visa Electron'">VE</xsl:when>
						<xsl:when test="CardType='Air Plus'">TP</xsl:when>
						<xsl:when test="CardType='Diners Club'">DN</xsl:when>
						<xsl:when test="CardType='MasterCard Debit'">MD</xsl:when>
						<xsl:when test="CardType='MasterCard Prepaid'">MP</xsl:when>
						<xsl:when test="CardType='Visa Delta'">VT</xsl:when>
						<xsl:when test="CardType='Visa Debit'">VD</xsl:when>
						<xsl:when test="CardType='Connect'">CN</xsl:when>
						<xsl:when test="CardType='Solo'">SO</xsl:when>
						<xsl:when test="CardType='Maestro'">MO</xsl:when>
						<xsl:when test="CardType='Switch'">SW</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="CardNumber">
					<xsl:value-of select="Number"/>
				</xsl:attribute>
				<xsl:attribute name="ExpireDate">
					<xsl:value-of select="translate(ExpiryDate,'/','')"/>
				</xsl:attribute>	
				<xsl:attribute name="SeriesCode">
					<xsl:value-of select="SecurityCode"/>
				</xsl:attribute>
			</PaymentCard>	
			<TPA_Extensions>
				<xsl:attribute name="FOPType">CC</xsl:attribute>
				<xsl:if test="IssueNumber!=''">
					<xsl:attribute name="ConfirmationNumber">
						<xsl:value-of select="IssueNumber"/>
					</xsl:attribute>
				</xsl:if>
			</TPA_Extensions>
			<PaymentAmount>
				<xsl:attribute name="Amount">
					<xsl:value-of select="../../../RouterHistory/TermsRouter/GroupList/Group/Price/Amount"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="../../../RouterHistory/TermsRouter/GroupList/Group/Price/Currency"/>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:choose>
						<xsl:when test="contains(../../../RouterHistory/TermsRouter/GroupList/Group/Price/Amount,'.')">2</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</PaymentAmount>
		</FormOfPayment>
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
			<xsl:if test="miscellaneousRemarks/remarks/category != ''">		
				<xsl:attribute name="Category"><xsl:value-of select="miscellaneousRemarks/remarks/category"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>		
		</Remark>
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
					<xsl:value-of select="serviceRequest/ssr/status"/>
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
					<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
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
						<xsl:attribute name="Status"><xsl:value-of select="../ssr/status"/></xsl:attribute>
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
							<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/boardpoint"/></xsl:attribute>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/offpoint"/></xsl:attribute>
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
						<xsl:attribute name="Status"><xsl:value-of select="../ssr/status"/></xsl:attribute>
						<xsl:attribute name="TravelerRefNumberRPHList">
							<xsl:value-of select="position()"/>
						</xsl:attribute>
						<xsl:attribute name="FlightRefNumberRPHList">
							<xsl:apply-templates select="../../referenceForDataElement/reference[qualifier='ST']" mode="SeatSegAssoc"/>
						</xsl:attribute>
						<DepartureAirport>
							<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/boardpoint"/></xsl:attribute>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode"><xsl:value-of select="../ssr/offpoint"/></xsl:attribute>
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
						<xsl:variable name="rph"><xsl:value-of select="."/></xsl:variable>
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
						<xsl:variable name="rph"><xsl:value-of select="."/></xsl:variable>
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
		<xsl:if test="position() > 1"><xsl:value-of select="string(' ')"/></xsl:if>
		<xsl:value-of select="//travellerInfo/elementManagementPassenger[reference/number=$Tattoo]/lineNumber"/>
	</xsl:template>
	
	<xsl:template match="reference" mode="SeatSegAssoc">
		<xsl:variable name="Tattoo">
			<xsl:value-of select="number"/>
		</xsl:variable>
		<xsl:if test="position() > 1"><xsl:value-of select="string(' ')"/></xsl:if>
		<xsl:value-of select="//originDestinationDetails/itineraryInfo/elementManagementItinerary[reference/number=$Tattoo]/lineNumber"/>
	</xsl:template>
		
	<xsl:template match="dataElementsIndiv" mode="accounting">
		<AccountingLine>
			<xsl:value-of select="accounting/account/number"/>
		</AccountingLine>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Pricing Response     	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="GroupList" mode="circle">
		<xsl:for-each select="Group/OutwardList/Outward">
			<xsl:variable name="outward" select="."/>
			<xsl:for-each select="../../ReturnList/Return">
				<AirFareInfo>
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
					<xsl:variable name="Deci" select="substring-after(string(Price/Amount),'.')" />
					<xsl:variable name="NoDeci" select="string-length($Deci)" />
					<xsl:variable name="TotalAmount" select="translate(string(Price/Amount),'.,','')" />
					<xsl:variable name="TotOutward" select="translate(string($outward/Price/Amount),'.','')"/>
					<xsl:variable name="TaxTotal">
						<xsl:variable name="tax">
							<xsl:choose>
								<xsl:when test="Price/TaxItemList/TaxItem/Amount!=''">
									<xsl:value-of select="sum(Price/TaxItemList/TaxItem/Amount)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="sum(Price/PassengerPriceList/PassengerPrice/TaxItemList/TaxItem/Amount)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="TaxTotalOutward">
						<xsl:variable name="tax">
							<xsl:choose>
								<xsl:when test="$outward/Price/TaxItemList/TaxItem/Amount!=''">
									<xsl:value-of select="sum($outward/Price/TaxItemList/TaxItem/Amount)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="sum($outward/Price/PassengerPriceList/PassengerPrice/TaxItemList/TaxItem/Amount)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="cur"><xsl:value-of select="Price/Currency"/></xsl:variable>
					<xsl:variable name="fee1">
						<xsl:choose>
							<xsl:when test="../../Price/Amount != ''">
								<xsl:value-of select="translate(../../Price/Amount,'.','')"/>
							</xsl:when>
							<xsl:when test="SegmentList/Segment/VendingOperator/Code='U2'">
								<xsl:choose>
									<xsl:when test="$cur = 'GBP'"><xsl:value-of select="'900'"/></xsl:when>
									<xsl:when test="$cur = 'USD'"><xsl:value-of select="'1500'"/></xsl:when>
									<xsl:when test="$cur = 'CHF'"><xsl:value-of select="'1600'"/></xsl:when>
									<xsl:when test="$cur = 'DKK'"><xsl:value-of select="'7900'"/></xsl:when>
									<xsl:when test="$cur = 'CZK'"><xsl:value-of select="'27000'"/></xsl:when>
									<xsl:when test="$cur = 'HUF'"><xsl:value-of select="'290000'"/></xsl:when>
									<xsl:when test="$cur = 'PLN'"><xsl:value-of select="'4500'"/></xsl:when>
									<xsl:when test="$cur = 'MAD'"><xsl:value-of select="'12500'"/></xsl:when>
									<xsl:when test="$cur = 'SEK'"><xsl:value-of select="'10000'"/></xsl:when>
									<xsl:otherwise><xsl:value-of select="'1100'"/></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="fee2">
						<xsl:choose>
							<xsl:when test="../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Amount!=''">
								<xsl:value-of select="translate(../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Amount,'.','')"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="fee">
						<xsl:value-of select="$fee1 + $fee2"/>
					</xsl:variable>
					<ItinTotalFare>
						<BaseFare>
							<xsl:attribute name="Amount">
								<xsl:value-of select="$TotalAmount + $TotOutward + $fee - $TaxTotal - $TaxTotalOutward" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="Price/Currency" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$NoDeci" />
							</xsl:attribute>
						</BaseFare>
						<Taxes>
							<Tax>
								<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TaxTotal + $TaxTotalOutward" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="Price/Currency" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</Tax>
						</Taxes>
						<xsl:if test="$fee!='0'">
							<Fees>
								<xsl:if test="$fee1!='0'">
	  								<Fee>
	  									<xsl:attribute name="FeeCode"><xsl:value-of select="'Fee'"/></xsl:attribute>
	  									<xsl:attribute name="FeeType"><xsl:value-of select="../../Price/TaxItemList/TaxItem/Name"/></xsl:attribute>
	  									<xsl:attribute name="Amount">
											<xsl:value-of select="$fee1" />
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode">
											<xsl:value-of select="$cur" />
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="$NoDeci" />
										</xsl:attribute>
	  								</Fee> 
	  							</xsl:if>
	  							<xsl:if test="$fee2!='0'">
	  								<Fee>
	  									<xsl:attribute name="FeeCode"><xsl:value-of select="'Fee'"/></xsl:attribute>
	  									<xsl:attribute name="FeeType"><xsl:value-of select="../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Name"/></xsl:attribute>
	  									<xsl:attribute name="Amount">
											<xsl:value-of select="$fee2" />
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode">
											<xsl:value-of select="$cur" />
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="$NoDeci" />
										</xsl:attribute>
	  								</Fee> 
	  							</xsl:if>
  							</Fees>
						</xsl:if>
						<TotalFare>
							<xsl:attribute name="Amount">
								<xsl:value-of select="$TotalAmount + $TotOutward + $fee" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="Price/Currency" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$NoDeci" />
							</xsl:attribute>
						</TotalFare>
					</ItinTotalFare>
					<PTC_FareBreakdowns>
						<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='30'][1]" mode="circle">
							<xsl:with-param name="age">30</xsl:with-param>
							<xsl:with-param name="outward" select="$outward"/>
							<xsl:with-param name="tottax" select="$TaxTotal + $TaxTotalOutward"/>
						</xsl:apply-templates> 
						<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='7'][1]" mode="circle">
							<xsl:with-param name="age">7</xsl:with-param>
							<xsl:with-param name="outward" select="$outward"/>
							<xsl:with-param name="tottax" select="$TaxTotal + $TaxTotalOutward"/>
						</xsl:apply-templates>
						<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='0'][1]" mode="circle">
							<xsl:with-param name="age">0</xsl:with-param>
							<xsl:with-param name="outward" select="$outward"/>
							<xsl:with-param name="tottax" select="$TaxTotal + $TaxTotalOutward"/>
						</xsl:apply-templates>
					</PTC_FareBreakdowns>
				</AirFareInfo>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="GroupList" mode="oneway">
		<xsl:for-each select="Group/OutwardList/Outward">
			<AirFareInfo>
				<xsl:attribute name="PricingSource">Published</xsl:attribute>
				<xsl:attribute name="ValidatingAirlineCode">
					<xsl:value-of select="SegmentList/Segment[1]/VendingOperator/Code"/>
				</xsl:attribute>
				<xsl:variable name="Deci" select="substring-after(string(Price/Amount),'.')" />
				<xsl:variable name="NoDeci" select="string-length($Deci)" />
				<xsl:variable name="TotalAmount" select="translate(string(Price/Amount),'.,','')" />
				<xsl:variable name="TaxTotal">
					<xsl:variable name="tax">
						<xsl:choose>
							<xsl:when test="Price/TaxItemList/TaxItem/Amount!=''">
								<xsl:value-of select="sum(Price/TaxItemList/TaxItem/Amount)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="sum(Price/PassengerPriceList/PassengerPrice/TaxItemList/TaxItem/Amount)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="cur"><xsl:value-of select="Price/Currency"/></xsl:variable>
				<xsl:variable name="fee1">
					<xsl:choose>
						<xsl:when test="../../Price/Amount != ''">
							<xsl:value-of select="translate(../../Price/Amount,'.','')"/>
						</xsl:when>
						<xsl:when test="SegmentList/Segment/VendingOperator/Code='U2'">
							<xsl:choose>
								<xsl:when test="$cur = 'GBP'"><xsl:value-of select="'900'"/></xsl:when>
								<xsl:when test="$cur = 'USD'"><xsl:value-of select="'1500'"/></xsl:when>
								<xsl:when test="$cur = 'CHF'"><xsl:value-of select="'1600'"/></xsl:when>
								<xsl:when test="$cur = 'DKK'"><xsl:value-of select="'7900'"/></xsl:when>
								<xsl:when test="$cur = 'CZK'"><xsl:value-of select="'27000'"/></xsl:when>
								<xsl:when test="$cur = 'HUF'"><xsl:value-of select="'290000'"/></xsl:when>
								<xsl:when test="$cur = 'PLN'"><xsl:value-of select="'4500'"/></xsl:when>
								<xsl:when test="$cur = 'MAD'"><xsl:value-of select="'12500'"/></xsl:when>
								<xsl:when test="$cur = 'SEK'"><xsl:value-of select="'10000'"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="'1100'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="fee2">
					<xsl:choose>
						<xsl:when test="../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Amount!=''">
							<xsl:value-of select="translate(../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Amount,'.','')"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="fee">
					<xsl:value-of select="$fee1 + $fee2"/>
				</xsl:variable>
				<ItinTotalFare>
					<BaseFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TotalAmount - $TaxTotal + $fee" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Price/Currency" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</BaseFare>
					<Taxes>
						<Tax>
							<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
							<xsl:attribute name="Amount">
								<xsl:value-of select="$TaxTotal" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="Price/Currency" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$NoDeci" />
							</xsl:attribute>
						</Tax>
					</Taxes>
					<xsl:if test="$fee!='0'">
						<Fees>
							<xsl:if test="$fee1!='0'">
  								<Fee>
  									<xsl:attribute name="FeeCode"><xsl:value-of select="'Fee'"/></xsl:attribute>
  									<xsl:attribute name="FeeType"><xsl:value-of select="../../Price/TaxItemList/TaxItem/Name"/></xsl:attribute>
  									<xsl:attribute name="Amount">
										<xsl:value-of select="$fee1" />
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="$cur" />
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$NoDeci" />
									</xsl:attribute>
  								</Fee> 
  							</xsl:if>
  							<xsl:if test="$fee2!='0'">
  								<Fee>
  									<xsl:attribute name="FeeCode"><xsl:value-of select="'Fee'"/></xsl:attribute>
  									<xsl:attribute name="FeeType"><xsl:value-of select="../../../../../TermsRouter/GroupList/Group/Price/TaxItemList/TaxItem/Name"/></xsl:attribute>
  									<xsl:attribute name="Amount">
										<xsl:value-of select="$fee2" />
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="$cur" />
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$NoDeci" />
									</xsl:attribute>
  								</Fee> 
  							</xsl:if>
 						</Fees>
					</xsl:if>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TotalAmount + $fee" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Price/Currency" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='30'][1]" mode="oneway">
						<xsl:with-param name="age">30</xsl:with-param>
					</xsl:apply-templates> 
					<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='7'][1]" mode="oneway">
						<xsl:with-param name="age">7</xsl:with-param>
					</xsl:apply-templates>
					<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='0'][1]" mode="oneway">
						<xsl:with-param name="age">0</xsl:with-param>
					</xsl:apply-templates>
				</PTC_FareBreakdowns>
			</AirFareInfo>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PassengerPrice" mode="circle">
		<xsl:param name="age"/>
		<xsl:param name="outward"/>
		<xsl:param name="tottax"/>
		<PTC_FareBreakdown>
			<xsl:variable name="Deci" select="substring-after(string(Amount),'.')" />
			<xsl:variable name="NoDeci" select="string-length($Deci)" />
			<xsl:variable name="TaxAmountPTC">
				<xsl:variable name="tax">
					<xsl:value-of select="sum(../PassengerPrice[Age=$age]/TaxItemList/TaxItem/Amount)"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TaxAmountPTCOutward">
				<xsl:variable name="tax">
					<xsl:value-of select="sum($outward/Price/PassengerPriceList/PassengerPrice[Age=$age]/TaxItemList/TaxItem/Amount)"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TotalAmountPTC">
				<xsl:value-of select="translate(format-number(sum(../PassengerPrice[Age=$age]/Amount),'#.00'),'.','')" />
			</xsl:variable>
			<xsl:variable name="TotalAmountPTCOutward">
				<xsl:value-of select="translate(format-number(sum($outward/Price/PassengerPriceList/PassengerPrice[Age=$age]/Amount),'#.00'),'.','')" />
			</xsl:variable>
			<xsl:variable name="NumberOfPax">
				<xsl:value-of select="count(../PassengerPrice[Age=$age])" />
			</xsl:variable>
			<xsl:attribute name="PricingSource">Published</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="$NumberOfPax" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC + $TotalAmountPTCOutward - $TaxAmountPTC - $TaxAmountPTCOutward" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="Currency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TaxAmountPTC + $TaxAmountPTCOutward" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Currency" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC + $TotalAmountPTCOutward" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="Currency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<PricedCode>
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</PricedCode>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>
	
	<xsl:template match="PassengerPrice" mode="oneway">
		<xsl:param name="age"/>
		<PTC_FareBreakdown>
			<xsl:variable name="Deci" select="substring-after(string(Amount),'.')" />
			<xsl:variable name="NoDeci" select="string-length($Deci)" />
			<xsl:variable name="TaxAmountPTC">
				<xsl:variable name="tax">
					<xsl:value-of select="sum(../PassengerPrice[Age=$age]/TaxItemList/TaxItem/Amount)"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TotalAmountPTC">
				<xsl:value-of select="translate(format-number(sum(../PassengerPrice[Age=$age]/Amount),'#.00'),'.','')" />
			</xsl:variable>
			<xsl:variable name="NumberOfPax">
				<xsl:value-of select="count(../PassengerPrice[Age=$age])" />
			</xsl:variable>
			<xsl:attribute name="PricingSource">Published</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="$NumberOfPax" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC - $TaxAmountPTC" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="Currency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TaxAmountPTC" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Currency" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="Currency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<PricedCode>
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</PricedCode>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>

</xsl:stylesheet>
