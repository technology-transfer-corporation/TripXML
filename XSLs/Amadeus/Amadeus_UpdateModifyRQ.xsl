<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_PNRUpdateRQ.xsl													  -->
	<!-- ================================================================== -->
	<!-- Date: 04 Feb 2009																  -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">	
		<ChangeElement>	
			<xsl:apply-templates select="UpdateModify/OTA_UpdateRQ"/>
		</ChangeElement>
	</xsl:template>	
	
	<xsl:template match="OTA_UpdateRQ">
		<xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='CustomerInfos']"/>
		<!--xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/ReservationItems']"/-->
		<xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='CustomerInfo']"/>
		<!--xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SeatRequests']"/>
		<xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SpecialServiceRequests']"/-->
		<xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='Remarks']"/>
		<xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='SpecialRemarks']"/>
		<xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='FormOfPayment']"/>
		<PoweredPNR_AddMultiElements>
			<pnrActions>
				<optionCode>0</optionCode>
			</pnrActions>
			<dataElementsMaster>
				<marker1/>
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>RF</segmentName>
					</elementManagementData>
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
							<type>P22</type>
						</freetextDetail>
						<longFreetext>
							<xsl:choose>
								<xsl:when test="POS/Source/@AgentSine!=''">
									<xsl:value-of select="POS/Source/@AgentSine"/>
								</xsl:when>
								<xsl:otherwise>TRAVELTALK</xsl:otherwise>
							</xsl:choose>
						</longFreetext>
					</freetextData>
				</dataElementsIndiv>
			</dataElementsMaster>
		</PoweredPNR_AddMultiElements>
	</xsl:template>
	
	<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Position[Element/@Operation='modify' and Element/@Child='CustomerInfos']">
		<xsl:apply-templates select="Element[1][CustomerInfos]" mode="name"/>
	</xsl:template>

	<xsl:template match="Element" mode="name">
		<xsl:variable name="repName" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfos/CustomerInfo">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="firstname"><xsl:value-of select="Customer/PersonName/GivenName"/></xsl:variable>
			<xsl:variable name="lastname"><xsl:value-of select="Customer/PersonName/Surname"/></xsl:variable>
			<xsl:variable name="paxtype"><xsl:value-of select="Customer/PersonName/@NameType"/></xsl:variable>
			<PoweredPNR_NameUpdate>
				<recordLocator>
					<reservation>
						<controlNumber><xsl:value-of select="../../../../UniqueID/@ID"/></controlNumber>
					</reservation>
				</recordLocator>
				<xsl:apply-templates select="//PoweredPNR_PNRReply/travellerInfo[passengerData/travellerInformation/passenger/firstName = $firstname and passengerData/travellerInformation/passenger/type = $paxtype and passengerData/travellerInformation/traveller/surname = $lastname]">	
					<xsl:with-param name="repName" select="$repName"/>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
				</xsl:apply-templates>
			</PoweredPNR_NameUpdate>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="travellerInfo">
		<xsl:param name="repName"/>
		<xsl:param name="pos"/>		
		<passenger>
			<passengerDetails>
				<paxDetails>
					<surname>
						<xsl:value-of select="$repName/CustomerInfos/CustomerInfo[position()=$pos]/Customer/PersonName/Surname"/>
					</surname>
					<quantity>1</quantity>
					<status>C</status>
				</paxDetails>
				<otherPaxDetails>
					<givenName>
						<xsl:value-of select="$repName/CustomerInfos/CustomerInfo[position()=$pos]/Customer/PersonName/GivenName"/>				
					</givenName>
					<uniqueCustomerIdentifier>
						<xsl:value-of select="elementManagementPassenger/reference/number"/>
					</uniqueCustomerIdentifier>
				</otherPaxDetails>
			</passengerDetails>	
		</passenger>
	</xsl:template>
	
	<!-- *********************************************************************************************************  -->

	<xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/ReservationItems']">
		<PoweredPNR_Cancel>
			<reservationInfo>		
				<reservation>
					<controlNumber>
						<xsl:value-of select="../UniqueID/@ID"/>
					</controlNumber>
				</reservation>
			</reservationInfo>
			<pnrActions>
				<optionCode>11</optionCode>
			</pnrActions>
			<cancelElements>
				<entryType>I</entryType>
				<xsl:apply-templates select="Element/ReservationItems/Item[../../@Operation='delete']"/>
			</cancelElements>
		</PoweredPNR_Cancel>
		<PoweredPNR_AddMultiElements>
			<reservationInfo>		
				<reservation>
					<controlNumber>
						<xsl:value-of select="../UniqueID/@ID"/>
					</controlNumber>
				</reservation>
			</reservationInfo>
			<pnrActions>
				<optionCode>0</optionCode>
			</pnrActions>
			<xsl:apply-templates select="Element/ReservationItems/Item[../../@Operation='modify']"/>
		</PoweredPNR_AddMultiElements>
	</xsl:template>
	
	<xsl:template match="Item[../../@Operation='delete']">
		<xsl:param name="segmentref"><xsl:value-of select="@ItinSeqNumber"/></xsl:param>		
		<element>
			<identifier>ST</identifier>
			<xsl:apply-templates select="../../../../../PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]" mode="delete"/>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary" mode="delete">
		<number>
			<xsl:value-of select="reference/number"/>
		</number>
	</xsl:template>
	
	
	<xsl:template match="Item[../../@Operation='modify']">
		<xsl:param name="segmentref"><xsl:value-of select="@ItinSeqNumber"/></xsl:param>		
		<originDestinationDetails>
			<originDestination>
				<origin>
					<xsl:value-of select="Air/DepartureAirport/@LocationCode" />
				</origin>
				<destination>
					<xsl:value-of select="Air/ArrivalAirport/@LocationCode" />
				</destination>
			</originDestination>
			<itineraryInfo>
				<elementManagementItinerary>
					<reference>
						<qualifier>SR</qualifier>
						<number>
							<xsl:value-of select="Air/@RPH"/>
						</number>
					</reference>
					<segmentName>AIR</segmentName>
				</elementManagementItinerary>
				<airAuxItinerary>
					<travelProduct>
						<product>
							<depDate>
								<xsl:value-of select="substring(string(Air/@DepartureDateTime),9,2)" />
								<xsl:value-of select="substring(string(Air/@DepartureDateTime),6,2)" />
								<xsl:value-of select="substring(string(Air/@DepartureDateTime),3,2)" />
							</depDate>
						</product>
						<boardpointDetail>
							<cityCode><xsl:value-of select="Air/DepartureAirport/@LocationCode" /></cityCode>
						</boardpointDetail>
						<offpointDetail>
							<cityCode><xsl:value-of select="Air/ArrivalAirport/@LocationCode" /></cityCode>
						</offpointDetail>
						<company>
							<identification><xsl:value-of select="Air/MarketingAirline/@Code" /></identification>
						</company>
						<productDetails>
							<identification><xsl:value-of select="Air/@FlightNumber" /></identification>
							<classOfService><xsl:value-of select="Air/@ResBookDesigCode" /></classOfService>
						</productDetails>
					</travelProduct>
					<messageAction>
						<business>
							<function>1</function>
						</business>
					</messageAction>
					<relatedProduct>
						<quantity><xsl:value-of select="Air/@NumberInParty" /></quantity>
						<status>NN</status>
					</relatedProduct>
					<selectionDetailsAir>
						<selection>
							<option>P10</option>
						</selection>
					</selectionDetailsAir>
				</airAuxItinerary>
			</itineraryInfo>
		</originDestinationDetails>

	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary" mode="modify">
		<number>
			<xsl:value-of select="reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->

	<xsl:template match="Position[Element/@Operation='modify' and Element/@Child='CustomerInfo'] | Position[Element/@Operation='modify' and Element/@Child='FormOfPayment'] | Position[Element/@Operation='modify' and Element/@Child='Remarks'] | Position[Element/@Operation='modify' and Element/@Child='SpecialRemarks']">
		<xsl:apply-templates select="Element[1][CustomerInfo/Customer/Telephone]" mode="phone"/>
		<xsl:apply-templates select="Element[1][CustomerInfo/Customer/Email]" mode="email"/>
		<xsl:apply-templates select="Element[1][CustomerInfo/Customer/Address[@UseType='Billing']/AddressLine]" mode="billing"/>	
		<xsl:apply-templates select="Element[1][CustomerInfo/Customer/Address[@UseType='Mailing']]" mode="mailing"/>
		<xsl:apply-templates select="Element[1][CustomerInfo/Customer/CustLoyalty]" mode="custloyalty"/>
		<xsl:apply-templates select="Element[1][Remarks/Remark]" mode="remark"/>
		<xsl:apply-templates select="Element[1][SpecialRemarks/SpecialRemark]" mode="remark"/>
		<xsl:apply-templates select="Element[1][FormOfPayment]" mode="fop"/>
	</xsl:template>
	
	<xsl:template match="Element" mode="phone">
		<xsl:variable name="repPhone" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfo/Customer/Telephone">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="phoneref"><xsl:value-of select="@PhoneNumber"/></xsl:variable>
				<PoweredPNR_ChangeElement>				
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]" 		mode="phone">
							<xsl:with-param name="repPhone" select="$repPhone"/>
							<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
						</xsl:apply-templates>
					</longTextString>
				</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="phone">
		<xsl:param name="repPhone"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repPhone/CustomerInfo/Customer/Telephone[position()=$pos]/@PhoneNumber)"/>
		</textStringDetails>
	</xsl:template>
	
	<xsl:template match="Element" mode="email">
		<xsl:variable name="repPhone" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfo/Customer/Email">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="emailref"><xsl:value-of select="."/></xsl:variable>
			<PoweredPNR_ChangeElement>				
				<messageAction>
					<messageFunctionDetails>
						<messageFunction>M</messageFunction>
					</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]" mode="email">
						<xsl:with-param name="repPhone" select="$repPhone"/>
						<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					</xsl:apply-templates>
				</longTextString>
			</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>			
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="email">
		<xsl:param name="repPhone"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repPhone/CustomerInfo/Customer/Email[position()=$pos])"/>
		</textStringDetails>
	</xsl:template>
		
	<xsl:template match="Element" mode="billing">
		<xsl:variable name="repAddress" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfo/Customer/Address[@UseType='Billing']">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="billingaddressref"><xsl:value-of select="."/></xsl:variable>
			<PoweredPNR_ChangeElement>				
				<messageAction>
					<messageFunctionDetails>
						<messageFunction>M</messageFunction>
					</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=	$billingaddressref]" mode="billingaddress">
						<xsl:with-param name="repAddress" select="$repAddress"/>
						<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					</xsl:apply-templates>
				</longTextString>			
			</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="billingaddress">
		<xsl:param name="repAddress"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repAddress/CustomerInfo/Customer/Address[position()=$pos and @UseType='Billing']/AddressLine)"/>
		</textStringDetails>
	</xsl:template>

	<xsl:template match="Element" mode="mailing">
		<xsl:variable name="repAddress" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfo/Customer/Address[@UseType='Mailing']">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="mailingaddressstreetref"><xsl:value-of select="StreetNmbr"/></xsl:variable>
			<xsl:variable name="mailingaddresscityref"><xsl:value-of select="CityName"/></xsl:variable>
			<xsl:variable name="mailingaddresszipref"><xsl:value-of select="PostalCode"/></xsl:variable>
			<xsl:variable name="mailingaddressstateref"><xsl:value-of select="StateProv/@StateCode"/></xsl:variable>
			<xsl:variable name="mailingaddresscountryref"><xsl:value-of select="CountryName/@Code"/></xsl:variable>
			<PoweredPNR_ChangeElement>				
				<messageAction>
					<messageFunctionDetails>
						<messageFunction>M</messageFunction>
					</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and  address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and 	address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and 	address/optionText=$mailingaddresscountryref)]">
						<xsl:with-param name="repAddress" select="$repAddress"/>
						<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					</xsl:apply-templates>
				</longTextString>			
			</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/structuredAddress">
		<xsl:param name="repAddress"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:value-of select="concat(../elementManagementData/lineNumber, '/A1-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/StreetNmbr, '/CI-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/CityName, '/ZP-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/PostalCode, '/ST-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/StateProv/@StateCode, '/CO-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/CountryName/@Code)"/>
		</textStringDetails>
	</xsl:template>
	
	<xsl:template match="Element" mode="custloyalty">
		<xsl:variable name="repCL" select="following-sibling::Element[1]"/>
		<xsl:for-each select="CustomerInfo/Customer/CustLoyalty">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="loyaltyref"><xsl:value-of select="@MembershipID"/></xsl:variable>
			<xsl:variable name="programref"><xsl:value-of select="@ProgramID"/></xsl:variable>
			<PoweredPNR_ChangeElement>				
				<messageAction>
					<messageFunctionDetails>
						<messageFunction>M</messageFunction>
					</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo	[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]">
						<xsl:with-param name="repCL" select="$repCL"/>
						<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					</xsl:apply-templates>
				</longTextString>			
			</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo">
		<xsl:param name="repCL"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repCL/CustomerInfo/Customer/CustLoyalty/@MembershipID)"/>
		</textStringDetails>
	</xsl:template>			
					
	<xsl:template match="Element" mode="fop">
		<xsl:variable name="repFOP" select="following-sibling::Element[1]"/>
		<xsl:variable name="fopref1">
			<xsl:value-of select="concat('CC',FormOfPayment/PaymentCard/@CardCode,translate(FormOfPayment/PaymentCard/@CardNumber,'x','X'),'/',FormOfPayment/PaymentCard/@ExpireDate)"/>
			<xsl:if test="FormOfPayment/PaymentAmount/@Amount!= ''">
				<xsl:text>/</xsl:text>
				<xsl:value-of select="FormOfPayment/PaymentAmount/@CurrencyCode"/>
				<xsl:variable name="dec"><xsl:value-of select="FormOfPayment/PaymentAmount/@DecimalPlaces"/></xsl:variable>
				<xsl:value-of select="substring(FormOfPayment/PaymentAmount/@Amount,1,string-length(FormOfPayment/PaymentAmount/@Amount) - $dec)"/>
				<xsl:text>.</xsl:text>
				<xsl:value-of select="substring(FormOfPayment/PaymentAmount/@Amount,string-length(FormOfPayment/PaymentAmount/@Amount) - ($dec - 1))"/>
			</xsl:if>
			<xsl:if test="FormOfPayment/TPA_Extensions/@ConfirmationNumber!= ''">
				<xsl:text>/</xsl:text>
				<xsl:value-of select="FormOfPayment/TPA_Extensions/@ConfirmationNumber"/>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="fopref"><xsl:value-of select="$fopref1"/></xsl:variable>
		<PoweredPNR_ChangeElement>				
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<xsl:variable name="confChange">
				<xsl:choose>
					<xsl:when test="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber!=''">
						<xsl:choose>
							<xsl:when test="FormOfPayment/PaymentCard/@CardCode = $repFOP/FormOfPayment/PaymentCard/@CardCode and FormOfPayment/PaymentCard/@CardNumber = $repFOP/FormOfPayment/PaymentCard/@CardNumber and FormOfPayment/PaymentCard/@ExpireDate = $repFOP/FormOfPayment/PaymentCard/@ExpireDate">
								<xsl:choose>
									<xsl:when test="not(FormOfPayment/PaymentAmount) and not($repFOP/FormOfPayment/PaymentAmount)">Y</xsl:when>
									<xsl:when test="FormOfPayment/PaymentAmount/@Amount != '' and FormOfPayment/PaymentAmount/@Amount = $repFOP/FormOfPayment/PaymentAmount/@Amount">Y</xsl:when>
									<xsl:otherwise>N</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>N</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<longTextString>
				<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fop">
					<xsl:with-param name="repFOP" select="$repFOP"/>
					<xsl:with-param name="fopref" select="$fopref"/>
					<xsl:with-param name="confChange" select="$confChange"/>
				</xsl:apply-templates>
			</longTextString>
		</PoweredPNR_ChangeElement>
	</xsl:template>
	
	<xsl:template match="otherDataFreetext" mode="fop">
		<xsl:param name="repFOP"/>
		<xsl:param name="fopref"/>
		<xsl:param name="confChange"/>
		<xsl:variable name="fop"><xsl:value-of select="translate(longFreetext,' ','')"/></xsl:variable>
		<xsl:variable name="fopref2">
			<xsl:choose>
				<xsl:when test="substring($fopref,3,2)='MC'"><xsl:value-of select="concat('CCCA',substring($fopref, 5))"/></xsl:when>
				<xsl:when test="substring($fopref,3,2)='DN'"><xsl:value-of select="concat('CCDC',substring($fopref, 5))"/></xsl:when>
				<xsl:otherwise><xsl:value-of select="$fopref"/></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="$fop=$fopref2">
			<textStringDetails>
				<xsl:variable name="cc">
					<xsl:choose>
						<xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='MC'">CA</xsl:when>
						<xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='DN'">DC</xsl:when>
						<xsl:otherwise><xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardCode"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="cn"><xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardNumber"/></xsl:variable>
				<xsl:variable name="ed"><xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@ExpireDate"/></xsl:variable>
				<xsl:variable name="amt"><xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@Amount"/></xsl:variable>
				<xsl:variable name="conf">
					<xsl:if test="substring($repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber,1,1)!='N'">N</xsl:if>
					<xsl:value-of select="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$confChange = 'Y'">
						<xsl:value-of select="concat(../elementManagementData/lineNumber, '//', $conf)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="newcard">
							<xsl:value-of select="concat(../elementManagementData/lineNumber, '/CC', $cc,$cn,'/',$ed)"/>
							<xsl:if test="$amt != ''">
								<xsl:text>/</xsl:text>
								<xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@CurrencyCode"/>
								<xsl:variable name="amt1">
									<xsl:variable name="dec"><xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@DecimalPlaces"/></xsl:variable>
									<xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,1,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - $dec)"/>
									<xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - ($dec - 1),2)"/>
								</xsl:variable>
								<xsl:value-of select="substring($amt1,1,string-length($amt) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring($amt1,string-length($amt) - 1)"/>
							</xsl:if>
							<xsl:if test="$conf != ''">
								<xsl:text>/</xsl:text>
								<xsl:value-of select="$conf"/>
							</xsl:if>
						</xsl:variable>
						<xsl:value-of select="$newcard"/>
					</xsl:otherwise>
				</xsl:choose>
			</textStringDetails>
		</xsl:if>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SeatRequests']">
		<ChangeElement> <!-- this block is sent for any element update-->
			<xsl:apply-templates select="Element/SeatRequests/SeatRequest[../../@Operation='delete']"/>
		</ChangeElement>
	</xsl:template>

	<xsl:template match="SeatRequest">
	<xsl:param name="seatref"><xsl:value-of select="@SeatPreference"/></xsl:param>
	<xsl:param name="segmentref"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:param>
		<PoweredPNR_ChangeElement>				
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<xsl:apply-templates select="../../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$segmentref]" mode="seat"/>
			</longTextString>
		</PoweredPNR_ChangeElement>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv" mode="seat">
		<textStringDetails>
			<xsl:value-of select="concat(elementManagementData/reference/number, '/', ../../../OTA_UpdateRQ/Position/Element/SeatRequests/SeatRequest/@SeatPreference[../../../@Operation='modify'])"/>
		</textStringDetails>
	</xsl:template>
	
	
<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SpecialServiceRequests']">
		<ChangeElement> <!-- this block is sent for any element update-->
			<xsl:apply-templates select="Element/SpecialServiceRequests/SpecialServiceRequest[../../@Operation='delete']"/>
		</ChangeElement>
	</xsl:template>

	<xsl:template match="SpecialServiceRequest">
	<xsl:param name="ssrref"><xsl:value-of select="@SSRCode"/></xsl:param>
		<PoweredPNR_ChangeElement>				
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<xsl:apply-templates select="../../../../../PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref]" mode="ssr"/>
			</longTextString>
		</PoweredPNR_ChangeElement>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv" mode="ssr">
		<textStringDetails>
			<xsl:value-of select="concat(elementManagementData/reference/number, '/', ../../../OTA_UpdateRQ/Position/Element/SpecialServiceRequests/SpecialServiceRequest/@SSRCode[../../../@Operation='modify'])"/>
		</textStringDetails>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element" mode="remark">
		<xsl:variable name="repRemark" select="following-sibling::Element[1]"/>
		<xsl:for-each select="Remarks/Remark">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="rmkref"><xsl:value-of select="."/></xsl:variable>
				<PoweredPNR_ChangeElement>				
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$rmkref]" 	mode="remark">
							<xsl:with-param name="repRemark" select="$repRemark"/>
							<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
						</xsl:apply-templates>
					</longTextString>
				</PoweredPNR_ChangeElement>
		</xsl:for-each>
		<xsl:for-each select="SpecialRemarks/SpecialRemark">
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="rmkref"><xsl:value-of select="."/></xsl:variable>
				<PoweredPNR_ChangeElement>				
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$rmkref]" 	mode="remark">
							<xsl:with-param name="repRemark" select="$repRemark"/>
							<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
						</xsl:apply-templates>
					</longTextString>
				</PoweredPNR_ChangeElement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks" mode="remark">
		<xsl:param name="repRemark"/>
		<xsl:param name="pos"/>
		<textStringDetails>
			<xsl:choose>
				<xsl:when test="$repRemark/Remarks">
					<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/Remarks/Remark[position()=$pos])"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/SpecialRemarks/SpecialRemark[position()=$pos])"/>
				</xsl:otherwise>
			</xsl:choose>
		</textStringDetails>
	</xsl:template>

</xsl:stylesheet>
