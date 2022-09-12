<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:air="http://www.travelport.com/schema/air_v50_0" xmlns:common_v50_0="http://www.travelport.com/schema/common_v50_0" xmlns:universal="http://www.travelport.com/schema/universal_v50_0" xmlns:SOAP="http://schemas.xmlsoap.org/soap/envelope/" version="1.0">
	<!-- 
	==================================================================
	Travelport_PNRRepriceRS.xsl 										
	================================================================== 
	Date: 30 Aug 2022 - Kobelev - Total Display and Price info Display corrected.
	Date: 19 Aug 2022 - Kobelev - Implamented Conversation ID.
	Date: 21 Mar 2022 - Kobelev - Update display.
	Date: 11 Nov 2014 - Rastko - new file								
	================================================================== 
	-->
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_PNRRepriceRS>
			<xsl:attribute name="Version">1.0</xsl:attribute>
			<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp"/>
			<xsl:apply-templates select="SOAP:Fault/detail" mode="error"/>
			<xsl:if test="universal:UniversalRecordRetrieveRsp/ConversationID!=''">
				<ConversationID>
					<xsl:value-of select="universal:UniversalRecordRetrieveRsp/ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_PNRRepriceRS>
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="detail" mode="error">
		<Errors>
			<Error>
				<xsl:value-of select="../faultstring"/>
				<xsl:value-of select="common_v50_0:ErrorInfo/common_v50_0:Description"/>
			</Error>
		</Errors>
	</xsl:template>
	<xsl:template match="universal:UniversalRecordRetrieveRsp">
		<xsl:choose>
			<xsl:when test="SOAP:Fault/detail">
				<Errors>
					<Error>
						<xsl:value-of select="SOAP:Fault/faultstring"/>
						<xsl:value-of select="SOAP:Fault/detail/common_v50_0:ErrorInfo/common_v50_0:Description"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="OTA_AirPriceRS/Error != ''">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:value-of select="OTA_AirPriceRS/Error"/>
					</Error>
				</Errors>
			</xsl:when>
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
			<xsl:otherwise>
				<Success/>
				<xsl:if test="Warning!=''">
					<Warnings>
						<xsl:apply-templates select="Warning"/>
					</Warnings>
				</xsl:if>
				<PricedItineraries>
					<xsl:apply-templates select="universal:UniversalRecord" mode="first"/>
					<xsl:apply-templates select="air:AirPriceRsp/air:AirPriceResult"/>
					<xsl:apply-templates select="universal:UniversalRecordRetrieveRsp/universal:UniversalRecord" mode="second"/>
				</PricedItineraries>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Warning">
		<Warning>
			<xsl:value-of select="normalize-space(translate(.,'Â',''))"/>
		</Warning>
	</xsl:template>
	<!--
	************************************************************************************
				PNR Retrieve Errors                                           	    
	************************************************************************************
	-->
	<xsl:template match="Err">
		<Error>
			<xsl:attribute name="Type">General</xsl:attribute>
			<xsl:value-of select="Text"/>
		</Error>
	</xsl:template>

	<!--************************************************************************************-->
	<!--					Calculate Total FareTotals	 	      			           -->
	<!--***********************************************************************************-->
	<xsl:template match="universal:UniversalRecord" mode="first">
		<xsl:call-template name="AirPricingInfo">
			<xsl:with-param name="fare" select="'stored'"/>
			<xsl:with-param name="sn" select="'1'"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="air:AirPriceResult">
		<xsl:call-template name="AirPricingInfo">
			<xsl:with-param name="fare" select="'new'"/>
			<xsl:with-param name="sn" select="2"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="universal:UniversalRecord" mode="second">
		<xsl:call-template name="AirPricingInfo">
			<xsl:with-param name="fare" select="'new'"/>
			<xsl:with-param name="sn" select="'2'"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="AirPricingInfo">
		<xsl:param name="fare"/>
		<xsl:param name="sn"/>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="$sn"/>
			</xsl:attribute>
			<xsl:apply-templates select="air:AirReservation"/>
			<xsl:apply-templates select="air:AirPricingSolution"/>
		</PricedItinerary>
	</xsl:template>
	<xsl:template match="air:AirReservation | air:AirPricingSolution">
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:variable name="bf">
				<xsl:choose>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFareQuote']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFareQuote'][1]" mode="totalbase">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFare']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFare'][1]" mode="totalbase">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="air:AirPricingInfo[1]" mode="totalbase">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="tf">
				<xsl:choose>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFareQuote']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFareQuote'][1]" mode="totalprice">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFare']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFare'][1]" mode="totalprice">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="air:AirPricingInfo[1]" mode="totalprice">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>			
				
			</xsl:variable>
			<xsl:variable name="Taxf">
				<xsl:choose>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFareQuote']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFareQuote'][1]" mode="totalTax">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFare']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFare'][1]" mode="totalTax">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="air:AirPricingInfo[1]" mode="totalTax">
							<xsl:with-param name="sum">0</xsl:with-param>
							<xsl:with-param name="pos">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="curt">
				<xsl:value-of select="substring(air:AirPricingInfo[1]/@BasePrice,1,3)"/>
			</xsl:variable>
			<xsl:variable name="dect">
				<xsl:value-of select="string-length(substring-after(substring(air:AirPricingInfo[1]/@BasePrice,4),'.'))"/>
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
				<xsl:choose>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFareQuote']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFareQuote']"/>
					</xsl:when>
					<xsl:when test="air:AirPricingInfo[@PricingType='StoredFare']">
						<xsl:apply-templates select="air:AirPricingInfo[@PricingType='StoredFare']"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="air:AirPricingInfo"/>
					</xsl:otherwise>
				</xsl:choose>
			</PTC_FareBreakdowns>
		</AirItineraryPricingInfo>
	</xsl:template>
	<xsl:template match="air:AirPricingInfo">
		<PTC_FareBreakdown>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:for-each select="air:PassengerType">

					<xsl:variable name="paxref">
						<xsl:choose>
							<xsl:when test="@BookingTravelerRef">
								<xsl:value-of select="@BookingTravelerRef"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="position()"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="@BookingTravelerRef">
							<xsl:call-template name="paxNumber">
								<xsl:with-param name="key" select="$paxref" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$paxref"/>
						</xsl:otherwise>
					</xsl:choose>
					<!--<xsl:value-of select="../../../common_v50_0:BookingTraveler[@Key=$paxref]/@Key"/>-->
				</xsl:for-each>
			</xsl:attribute>
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:for-each select="air:BookingInfo">
					<xsl:variable name="segref">
						<xsl:choose>
							<xsl:when test="@SegmentRef">
								<xsl:value-of select="@SegmentRef"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="position()"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:if test="position() > 1">
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="../../air:AirSegment">
							<xsl:call-template name="fltNumber">
								<xsl:with-param name="key" select="$segref" />
								<xsl:with-param name="segs" select="../../air:AirSegment" />
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="@SegmentRef">
							<xsl:call-template name="fltNumber">
								<xsl:with-param name="key" select="$segref" />
								<xsl:with-param name="segs" select="../../../../../air:AirPriceRsp/air:AirItinerary/air:AirSegment" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$segref"/>
						</xsl:otherwise>
					</xsl:choose>

					<!--<xsl:value-of select="../../air:AirSegment[@Key=$segref]/@Key"/>-->
				</xsl:for-each>
			</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:variable name="paxtype">
						<xsl:value-of select="air:PassengerType/@Code"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$paxtype = 'CH'">CHD</xsl:when>
						<xsl:when test="$paxtype = 'INN'">CHD</xsl:when>
						<xsl:when test="$paxtype = 'CNN'">CHD</xsl:when>
						<xsl:when test="$paxtype = 'YCD'">SRC</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$paxtype"/>
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
				<Taxes>
					<xsl:apply-templates select="air:TaxInfo">
						<xsl:with-param name="nip">
							<xsl:value-of select="$nip"/>
						</xsl:with-param>
						<xsl:with-param name="dec">
							<xsl:value-of select="$dec"/>
						</xsl:with-param>
					</xsl:apply-templates>
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
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>
	<xsl:template match="air:FareInfo" mode="fb">
		<FareBasisCode>
			<xsl:value-of select="@FareBasis"/>
		</FareBasisCode>
	</xsl:template>
	<xsl:template match="air:TaxInfo">
		<xsl:param name="nip"/>
		<xsl:param name="dec"/>
		<Tax>
			<xsl:attribute name="Code">
				<xsl:value-of select="@Category"/>
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(substring(@Amount,4),'.','') * $nip"/>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$dec"/>
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<xsl:template match="ItineraryPricing | OTA_AirPriceRS" mode="Fare">
		<xsl:param name="fare"/>
		<xsl:param name="sn"/>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="$sn"/>
			</xsl:attribute>
			<xsl:variable name="dect">
				<xsl:choose>
					<xsl:when test="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFae/EquivFare/@Amount!=''">
						<xsl:value-of select="string-length(substring-after(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="string-length(substring-after(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<AirItineraryPricingInfo>
				<xsl:attribute name="PricingSource">
					<xsl:choose>
						<xsl:when test="contains(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JCB') or contains(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNN') or contains(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@InputMessage,'JNF')">
							<xsl:value-of select="'Private'"/>
						</xsl:when>
						<xsl:when test="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JCB' or PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNN' or PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/PassengerTypeQuantity/@Code='JNF'">
							<xsl:value-of select="'Private'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Published'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<ItinTotalFare>
					<BaseFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
									<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/BaseFare">
										<xsl:with-param name="totalbf">0</xsl:with-param>
										<xsl:with-param name="pos">1</xsl:with-param>
										<xsl:with-param name="bfcount">
											<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
										</xsl:with-param>
										<xsl:with-param name="fare" select="'new'"/>
									</xsl:apply-templates>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
										<xsl:with-param name="totalbf">0</xsl:with-param>
										<xsl:with-param name="pos">1</xsl:with-param>
										<xsl:with-param name="bfcount">
											<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
										</xsl:with-param>
										<xsl:with-param name="fare" select="'stored'"/>
									</xsl:apply-templates>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dect"/>
						</xsl:attribute>
					</BaseFare>
					<Taxes>
						<Tax>
							<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
							<xsl:choose>
								<xsl:when test="$fare='new'">
									<xsl:choose>
										<xsl:when test="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
											<xsl:attribute name="Amount">
												<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/Taxes">
													<xsl:with-param name="totalbf">0</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
													<xsl:with-param name="bfcount">
														<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
													</xsl:with-param>
													<xsl:with-param name="fare" select="'new'"/>
												</xsl:apply-templates>
											</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="Amount">
												<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes">
													<xsl:with-param name="totalbf">0</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
													<xsl:with-param name="bfcount">
														<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
													</xsl:with-param>
													<xsl:with-param name="fare" select="'stored'"/>
												</xsl:apply-templates>
											</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="Amount">
										<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
											<xsl:with-param name="totalbf">0</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
											<xsl:with-param name="bfcount">
												<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
											</xsl:with-param>
										</xsl:apply-templates>
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$dect"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</Tax>
					</Taxes>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])=1 and count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo)>1">
									<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo[1]/ItinTotalFare/TotalFare">
										<xsl:with-param name="totalbf">0</xsl:with-param>
										<xsl:with-param name="pos">1</xsl:with-param>
										<xsl:with-param name="bfcount">
											<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo)+1"/>
										</xsl:with-param>
										<xsl:with-param name="fare" select="'new'"/>
									</xsl:apply-templates>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
										<xsl:with-param name="totalbf">0</xsl:with-param>
										<xsl:with-param name="pos">1</xsl:with-param>
										<xsl:with-param name="bfcount">
											<xsl:value-of select="count(PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))])+1"/>
										</xsl:with-param>
										<xsl:with-param name="fare" select="'stored'"/>
									</xsl:apply-templates>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dect"/>
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:apply-templates select="PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))]/PricedItinerary/AirItineraryPricingInfo">
						<xsl:with-param name="fare" select="$fare"/>
					</xsl:apply-templates>
				</PTC_FareBreakdowns>
			</AirItineraryPricingInfo>
		</PricedItinerary>
	</xsl:template>
	<xsl:template match="BaseFare">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:param name="fare"/>
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
			<xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
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
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$fare = 'new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]">
				<xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/BaseFare">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
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
			<xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
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
	<xsl:template match="Taxes">
		<xsl:param name="totalbf"/>
		<xsl:param name="pos"/>
		<xsl:param name="bfcount"/>
		<xsl:param name="fare"/>
		<xsl:variable name="bf1">
			<xsl:value-of select="translate(@TotalAmount,'.','')"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
		</xsl:variable>
		<xsl:variable name="bf">
			<xsl:value-of select="$bf1 * $nip"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/@TotalAmount!=''">
				<xsl:apply-templates select="../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes[@TotalAmount!='']">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$fare='new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/Taxes/@TotalAmount!=''">
				<xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/Taxes[@TotalAmount!='']">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
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
		<xsl:param name="fare"/>
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
			<xsl:when test="$fare='stored' and $pos &lt; $bfcount and ../../../../../PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][$pos + 1]">
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
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$fare='new' and $pos &lt; $bfcount and ../../../AirItineraryPricingInfo[$pos + 1]">
				<xsl:apply-templates select="../../../AirItineraryPricingInfo[$pos + 1]/ItinTotalFare/TotalFare">
					<xsl:with-param name="totalbf">
						<xsl:value-of select="$totalbf + $bf"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
					<xsl:with-param name="bfcount">
						<xsl:value-of select="$bfcount"/>
					</xsl:with-param>
					<xsl:with-param name="fare">
						<xsl:value-of select="$fare"/>
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
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="'TotalTax'"/>
			</xsl:attribute>
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
		<xsl:param name="fare"/>
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
				<xsl:choose>
					<xsl:when test="$fare='new'">
						<xsl:value-of select="position()"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="../../@RPH"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="contains(../@InputMessage,'JCB') or contains(../@InputMessage,'JNN') or contains(../@InputMessage,'JNF')">
						<xsl:value-of select="'Private'"/>
					</xsl:when>
					<xsl:when test="PassengerTypeQuantity/@Code='JCB' or PassengerTypeQuantity/@Code='JNN' or PassengerTypeQuantity/@Code='JNF'">
						<xsl:value-of select="'Private'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Published'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:choose>
					<xsl:when test="PassengerTypeQuantity/@Code='C09'">
						<xsl:attribute name="Code">CHD</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="Code">
							<xsl:value-of select="PassengerTypeQuantity/@Code"/>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="format-number(PassengerTypeQuantity/@Quantity,'#0')"/>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<xsl:choose>
				<xsl:when test="$fare='new'">
					<FareBasisCodes>
						<xsl:for-each select="PTC_FareBreakdown/FareBasis">
							<FareBasisCode>
								<xsl:value-of select="@Code"/>
							</FareBasisCode>
						</xsl:for-each>
					</FareBasisCodes>
				</xsl:when>
				<xsl:otherwise>
					<FareBasisCodes>
						<xsl:for-each select="PTC_FareBreakdown/FlightSegment/FareBasis">
							<FareBasisCode>
								<xsl:value-of select="@Code"/>
							</FareBasisCode>
						</xsl:for-each>
					</FareBasisCodes>
					<!--xsl:if test="PTC_FareBreakdown/FareBasis/@Code">
						<FareBasisCodes>
							<xsl:variable name="fbc">
								<xsl:value-of select="PTC_FareBreakdown/FareBasis/@Code"/>
							</xsl:variable>
							<xsl:call-template name="farebasis">
								<xsl:with-param name="fbc">
									<xsl:value-of select="$fbc"/>
								</xsl:with-param>
							</xsl:call-template>
						</FareBasisCodes>
					</xsl:if-->
				</xsl:otherwise>
			</xsl:choose>
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
				<xsl:if test="ItinTotalFare/BaseFare/EquivFare">
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dect1"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
						</xsl:attribute>
					</EquivFare>
				</xsl:if>
				<Taxes>
					<xsl:choose>
						<xsl:when test="$fare='new'">
							<Tax TaxCode="'TotalTax'" Amount="{translate(ItinTotalFare/Taxes/@TotalAmount,'.','')}" DecimalPlaces="{string-length(substring-after(ItinTotalFare/Taxes/@TotalAmount,'.'))}" CurrencyCode="{ItinTotalFare/BaseFare/@CurrencyCode}" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="ItinTotalFare/Taxes/Tax" mode="PTC"/>
						</xsl:otherwise>
					</xsl:choose>
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
				<xsl:if test="FareCalculation/Text!=''">
					<FareCalculation>
						<xsl:value-of select="FareCalculation/Text"/>
					</FareCalculation>
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
				<xsl:if test="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code != ''">
					<ValidatingAirlineCode>
						<xsl:value-of select="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code"/>
					</ValidatingAirlineCode>
					<!--Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="../../MiscInformation/HeaderInformation/ValidatingCarrier/@Code"/>
						</xsl:attribute>
					</Vendor-->
				</xsl:if>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>
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
						<xsl:value-of select="$bf * $nip"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
					</xsl:attribute>
				</BaseFare>
				<!--xsl:if test="PTC_FareBreakdown/PassengerFare/EquivFare">
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFare/@DecimalPlaces" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivFare/@CurrencyCode" />
						</xsl:attribute>
					</EquivFare>
				</xsl:if-->
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
						<xsl:value-of select="$bf * $nip"/>
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
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="'TotalTax'"/>
			</xsl:attribute>
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

	<xsl:template match="air:AirPricingInfo" mode="totalbase">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>

		<xsl:variable name="pax">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>

		<xsl:variable name="pq" select="@PricingType" />

		<xsl:variable name="tot">
			<xsl:value-of select="translate(substring(@BasePrice,4),'.','') * $pax"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="$pq!=''">
				<xsl:choose>
					<xsl:when test="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]">
						<xsl:apply-templates select="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]" mode="totalbase">
							<xsl:with-param name="sum">
								<xsl:value-of select="number($tot) + number($sum)"/>
							</xsl:with-param>
							<xsl:with-param name="pos">
								<xsl:value-of select="$pos + 1"/>
							</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="number($tot) + number($sum)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="../air:AirPricingInfo[$pos + 1]">
						<xsl:apply-templates select="../air:AirPricingInfo[$pos + 1]" mode="totalbase">
							<xsl:with-param name="sum">
								<xsl:value-of select="number($tot) + number($sum)"/>
							</xsl:with-param>
							<xsl:with-param name="pos">
								<xsl:value-of select="$pos + 1"/>
							</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="number($tot) + number($sum)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="air:AirPricingInfo" mode="totalTax">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(substring(@Taxes,4),'.','') * $nopt"/>
		</xsl:variable>
		<xsl:variable name="pq" select="@PricingType" />
		<xsl:choose>
			<xsl:when test="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]">
				<xsl:apply-templates select="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]" mode="totalTax">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
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
	<xsl:template match="air:AirPricingInfo" mode="totalprice">
		<xsl:param name="sum"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(air:PassengerType)"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(substring(@TotalPrice,4),'.','') * $nopt"/>
		</xsl:variable>
		
		<xsl:variable name="pq" select="@PricingType" />

		<xsl:choose>
			<xsl:when test="$pq!=''">
				<xsl:choose>
					<xsl:when test="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]">
						<xsl:apply-templates select="../air:AirPricingInfo[@PricingType=$pq][$pos + 1]" mode="totalprice">
							<xsl:with-param name="sum">
								<xsl:value-of select="$tot + $sum"/>
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
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="../air:AirPricingInfo[$pos + 1]">
						<xsl:apply-templates select="../air:AirPricingInfo[$pos + 1]" mode="totalprice">
							<xsl:with-param name="sum">
								<xsl:value-of select="number($tot) + number($sum)"/>
							</xsl:with-param>
							<xsl:with-param name="pos">
								<xsl:value-of select="$pos + 1"/>
							</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="number($tot) + number($sum)"/>
					</xsl:otherwise>
				</xsl:choose>
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
		<xsl:param name="segs" />
		<xsl:for-each select="$segs">
			<xsl:if test="@Key = $key">
				<xsl:value-of select="position()"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
