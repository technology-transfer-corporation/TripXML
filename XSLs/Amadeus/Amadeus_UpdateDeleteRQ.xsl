<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_UpdateDeleteRQ.xsl												  -->
	<!-- ================================================================== -->
	<!-- Date: 25 Jul 2008 - Rastko													  -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	
	<xsl:template match="UpdateDelete">	
		<UpdateDelete>
			<Cancel>	
				<PoweredPNR_Cancel>
					<xsl:apply-templates select="OTA_UpdateRQ"/>
				</PoweredPNR_Cancel>
			</Cancel>
			<RF>
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
										<xsl:when test="OTA_UpdateRQ/POS/Source/@AgentSine!=''">
											<xsl:value-of select="OTA_UpdateRQ/POS/Source/@AgentSine"/>
										</xsl:when>
										<xsl:otherwise>TRAVELTALK</xsl:otherwise>
									</xsl:choose>
								</longFreetext>
							</freetextData>
						</dataElementsIndiv>
					</dataElementsMaster>
				</PoweredPNR_AddMultiElements>
			</RF>
		</UpdateDelete>
	</xsl:template>
	
	<xsl:template match="OTA_UpdateRQ">
		<pnrActions>
			<optionCode>0</optionCode>
		</pnrActions>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='CustomerInfos']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='ReservationItems']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='CustomerInfo']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SeatRequests']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SpecialServiceRequests']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='Remarks']"/>
		<xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SpecialRemarks']"/>
	</xsl:template>
	
	<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='CustomerInfos']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="CustomerInfos/CustomerInfo"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="CustomerInfo">
	<xsl:param name="customerref"><xsl:value-of select="@RPH"/></xsl:param>		
		<element>
			<identifier>PT</identifier>
			<xsl:apply-templates select="//PoweredPNR_PNRReply/travellerInfo/elementManagementPassenger[lineNumber=$customerref]"/>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/travellerInfo/elementManagementPassenger">
		<number>
			<xsl:value-of select="reference/number"/>
		</number>
	</xsl:template>

	<!-- *********************************************************************************************************  -->

	<xsl:template match="Element[@Child='ReservationItems']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="ReservationItems/Item"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="Item">
	<xsl:param name="segmentref"><xsl:value-of select="@ItinSeqNumber"/></xsl:param>		
		<element>
			<identifier>ST</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Segment number: </xsl:text>
						<xsl:value-of select="@ItinSeqNumber"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo/elementManagementItinerary">
		<number>
			<xsl:value-of select="reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='CustomerInfo']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="CustomerInfo/Customer/Telephone"/>
			<xsl:apply-templates select="CustomerInfo/Customer/Email"/>
			<xsl:apply-templates select="CustomerInfo/Customer/Address[@UseType='Billing']"/>
			<xsl:apply-templates select="CustomerInfo/Customer/Address[@UseType='Mailing']"/>
			<xsl:apply-templates select="CustomerInfo/Customer/CustLoyalty"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="Telephone">
	<xsl:param name="phoneref"><xsl:value-of select="@PhoneNumber"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Telephone element: </xsl:text>
						<xsl:value-of select="@PhoneNumber"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="Email">
	<xsl:param name="emailref"><xsl:value-of select="."/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Email element: </xsl:text>
						<xsl:value-of select="."/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="Address[@UseType='Billing']">
	<xsl:param name="billingaddressref"><xsl:value-of select="AddressLine"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$billingaddressref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$billingaddressref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Address: </xsl:text>
						<xsl:value-of select="AddressLine"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>

	<xsl:template match="Address[@UseType='Mailing']">
	<xsl:param name="mailingaddressstreetref"><xsl:value-of select="StreetNmbr"/></xsl:param>
	<xsl:param name="mailingaddresscityref"><xsl:value-of select="CityName"/></xsl:param>
	<xsl:param name="mailingaddresszipref"><xsl:value-of select="PostalCode"/></xsl:param>
	<xsl:param name="mailingaddressstateref"><xsl:value-of select="StateProv/@StateCode"/></xsl:param>
	<xsl:param name="mailingaddresscountryref"><xsl:value-of select="CountryName/@Code"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and address/optionText=$mailingaddresscountryref)]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and address/optionText=$mailingaddresscountryref)]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Address element: </xsl:text>
						<xsl:value-of select="StreetNmbr"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="CityName"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="PostalCode"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="StateProv/@StateCode"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="CountryName/@Code"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>

		</element>
	</xsl:template>
		
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/structuredAddress">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>

<xsl:template match="CustLoyalty">
	<xsl:param name="loyaltyref"><xsl:value-of select="@MembershipID"/></xsl:param>
	<xsl:param name="programref"><xsl:value-of select="@ProgramID"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>CustLoyalty element: </xsl:text>
						<xsl:value-of select="@MembershipID"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="@ProgramID"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='SeatRequests']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="SeatRequests/SeatRequest"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="SeatRequest">
	<xsl:param name="seatref"><xsl:value-of select="@SeatPreference"/></xsl:param>
	<xsl:param name="segmentref"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$segmentref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$segmentref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Seat element: </xsl:text>
						<xsl:value-of select="@SeatPreference"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="@FlightRefNumberRPHList"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv">
		<number>
			<xsl:value-of select="elementManagementData/reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='SpecialServiceRequests']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="SpecialServiceRequests/SpecialServiceRequest"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="SpecialServiceRequest">
	<xsl:param name="ssrref"><xsl:value-of select="@SSRCode"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>SSR element: </xsl:text>
						<xsl:value-of select="@SSRCode"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv">
		<number>
			<xsl:value-of select="elementManagementData/reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='Remarks']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="Remarks/Remark"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="Remark">
	<xsl:param name="remarkref"><xsl:value-of select="."/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Remark element: </xsl:text>
						<xsl:value-of select="."/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>

<!-- *********************************************************************************************************  -->
	
	<xsl:template match="Element[@Child='SpecialRemarks']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="SpecialRemarks/SpecialRemark"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="SpecialRemark">
	<xsl:param name="specialremarkref"><xsl:value-of select="Text"/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$specialremarkref]">
					<xsl:apply-templates select="//PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$specialremarkref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Special Remark element: </xsl:text>
						<xsl:value-of select="Text"/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>

</xsl:stylesheet>
