<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!--************************************************************************************************************-->
	<!--  		This PNR processes the response to a PNR Read Request                                    -->
	<!--************************************************************************************************************-->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_TravelItineraryRS Version="2.000">
			<Success />
			<xsl:apply-templates select="AgencyPNRBFDisplay_7_0" />
		</OTA_TravelItineraryRS>
	</xsl:template>
	<xsl:template match="AgencyPNRBFDisplay_7_0">
		<xsl:choose>
			<xsl:when test="PNRBFRetrieve">
				<TravelItinerary>
					<ItineraryRef>
						<xsl:attribute name="Type">PNR</xsl:attribute>
						<xsl:attribute name="ID">
							<xsl:value-of select="PNRBFRetrieve/GenPNRInfo/RecLoc" />
						</xsl:attribute>
					</ItineraryRef>
					<xsl:apply-templates select="PNRBFRetrieve" />
					<!-- <xsl:if test="CarSeg">
							<xsl:apply-templates select="CarSeg"/>						               Process Car Segements   	
						</xsl:if> -->
					<xsl:if test="HtlSeg">
						<xsl:apply-templates select="HtlSeg" />
						<!--  Process Car Segements      -->
					</xsl:if>
					<!-- <xsl:if test="NonAirSeg">						
						<MiscellaneousGroup>
							<xsl:apply-templates select="NonAirSeg"/>					   `          Process NonAir Segments   
						</MiscellaneousGroup>
					</xsl:if> -->
				</TravelItinerary>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<xsl:apply-templates select="Err" />
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- *******************************-->
	<!--    Templates - Tier1          -->
	<!-- *******************************-->
	<!--************************************************************************************-->
	<!--			PNR General Info - PNRBFRetrieve                                          -->
	<!--************************************************************************************-->
	<xsl:template match="PNRBFRetrieve">
		<!--******************************************************-->
		<!--				PNR Info                                     -->
		<!--******************************************************-->
		<CustomerInfos>
			<xsl:apply-templates select="LNameInfo" mode="pax" />
		</CustomerInfos>
		<ItineraryInfo>
			<ReservationItems>
				<Item>
					<!--******************************************************-->
					<!--			Air Itinerary                                      -->
					<!--******************************************************-->
					<xsl:if test="AirSeg or OpenAirSeg or ARNK">
						<!--  Process Air Itinerary	      -->
						<xsl:apply-templates select="AirSeg | OpenAirSeg | ARNK" mode="Segment" />
					</xsl:if>
				</Item>
				<xsl:if test="../DocProdDisplayStoredQuote">
					<ItemPricing>
						<xsl:apply-templates select="../DocProdDisplayStoredQuote" />
					</ItemPricing>
				</xsl:if>
			</ReservationItems>
			<xsl:if test="GenPNRInfo">
				<xsl:apply-templates select="GenPNRInfo" />
				<!--  Process Ticketing Info   -->
			</xsl:if>
			<!--******************************************************-->
			<!--			Special Request Details                    -->
			<!--******************************************************-->
			<xsl:if test="SeatSeg or ProgramaticSSR or NonProgramaticSSR or OSI or GenRmkInfo or  ItinRmk or InvoiceRmk">
				<SpecialRequestDetails>
					<xsl:if test="SeatSeg">
						<SeatRequests>
							<xsl:apply-templates select="SeatSeg" />
						</SeatRequests>
					</xsl:if>
					<xsl:if test="ProgramaticSSR or NonProgramaticSSR">
						<SpecialServiceRequests>
							<xsl:if test="ProgramaticSSR">
								<xsl:apply-templates select="ProgramaticSSR" />
							</xsl:if>
							<xsl:if test="NonProgramaticSSR">
								<xsl:apply-templates select="NonProgramaticSSR" />
							</xsl:if>
						</SpecialServiceRequests>
					</xsl:if>
					<xsl:if test="OSI">
						<OtherServiceInformations>
							<xsl:apply-templates select="OSI" />
						</OtherServiceInformations>
					</xsl:if>
					<xsl:if test="GenRmkInfo">
						<Remarks>
							<xsl:attribute name="RemarkType">General</xsl:attribute>
							<xsl:apply-templates select="GenRmkInfo" />
						</Remarks>
					</xsl:if>
					<xsl:if test="ItinRmk or InvoiceRmk">
						<SpecialRemarks>
							<xsl:if test="ItinRmk">
								<xsl:apply-templates select="ItinRmk" />
							</xsl:if>
							<xsl:if test="InvoiceRmk">
								<xsl:apply-templates select="InvoiceRmk" />
							</xsl:if>
						</SpecialRemarks>
					</xsl:if>
				</SpecialRequestDetails>
			</xsl:if>
		</ItineraryInfo>
		<!--******************************************************-->
		<!--			Form of Payment                               -->
		<!--******************************************************-->
		<TravelCost>
			<xsl:if test="CreditCardFOP">
				<FormOfPayment>
					<xsl:apply-templates select="CreditCardFOP" />
				</FormOfPayment>
			</xsl:if>
			<xsl:if test="OtherFOP">
				<FormOfPayment>
					<xsl:apply-templates select="OtherFOP" />
				</FormOfPayment>
			</xsl:if>
			<xsl:if test="CheckFOP">
				<FormOfPayment>
					<xsl:apply-templates select="CheckFOP" />
				</FormOfPayment>
			</xsl:if>
		</TravelCost>
		<!-- <xsl:apply-templates select="TAUTkArrangement"/>
	<xsl:apply-templates select="TAWTkArrangement"/>
	<xsl:apply-templates select="TkArrangement"/>
	<xsl:apply-templates select="TLTkArrangement"/>
	<xsl:apply-templates select="TAMTkArrangement"/>		
	 <xsl:apply-templates select="FreqCustInfo[LNameNum != '']" mode="FrequentTravelerGroup"/>		
	<xsl:apply-templates select="ProfileClientFileAssoc"/>
	<xsl:apply-templates select="CustID"/>
	<xsl:apply-templates select="DuePaidInfo"/>	-->
	</xsl:template>
	<!--************************************************************************************-->
	<!--			PNR Retrieve Errors                                           	                 -->
	<!--************************************************************************************-->
	<xsl:template match="Err">
		<Error>
			<xsl:attribute name="Type">General</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Calculate Total FareTotals	 	      			           -->
	<!--***********************************************************************************-->
	<xsl:template match="DocProdDisplayStoredQuote">
		<AirFareInfo>
			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">0</xsl:attribute>
					<xsl:attribute name="CurrencyCode">USD</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">0</xsl:attribute>
				</BaseFare>
				<Taxes>
					<xsl:attribute name="Amount">0</xsl:attribute>
					<xsl:attribute name="CurrencyCode">USD</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">0</xsl:attribute>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">0</xsl:attribute>
					<xsl:attribute name="CurrencyCode">USD</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">0</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="../DocProdDisplayStoredQuote/GenQuoteDetails" mode="Details" />
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Calculate Fare Totals per Passenger Type	 	                 -->
	<!--************************************************************************************-->
	<xsl:template match="GenQuoteDetails" mode="Details">
		<PTC_FareBreakdown>
			<xsl:choose>
				<xsl:when test="PrivFQd='Y'">
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<PassengerTypeQuantity>
				<xsl:attribute name="Quantity">
					<xsl:variable name="PaxKey">
						<xsl:value-of select="UniqueKey" />
					</xsl:variable>
					<xsl:for-each select="//NameRmkInfo">
						<xsl:variable name="LastNameNo">000<xsl:value-of select="LNameNum" /></xsl:variable>
						<xsl:variable name="PaxTot">
							<xsl:if test="$LastNameNo=$PaxKey">
								<xsl:variable name="PaxType">
									<xsl:value-of select="NameRmk" />
								</xsl:variable>
								<xsl:value-of select="count(../NameRmkInfo[NameRmk=$PaxType])" />
							</xsl:if>
						</xsl:variable>
						<xsl:value-of select="$PaxTot" />
					</xsl:for-each>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:variable name="ItemNo">
					<xsl:value-of select="UniqueKey" />
				</xsl:variable>
				<xsl:apply-templates select="following-sibling::SegRelatedInfo[$ItemNo=UniqueKey]" />
				<!-- <xsl:apply-templates select="following-sibling::SegRelatedInfo/BagInfo[$ItemNo=UniqueKey]"/> -->
			</FareBasisCodes>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="BaseFareAmt" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="BaseDecPos" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="BaseFareCurrency" />
					</xsl:attribute>
				</BaseFare>
				<xsl:choose>
					<xsl:when test="EquivAmt != '0'">
						<EquivFare>
							<xsl:attribute name="Amount">
								<xsl:value-of select="EquivAmt" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="EquivDecPos" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="EquivCurrency" />
							</xsl:attribute>
						</EquivFare>
					</xsl:when>
				</xsl:choose>
				<Taxes>
					<xsl:attribute name="Amount">
						<xsl:value-of select="TotAmt - BaseFareAmt" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="BaseFareCurrency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="BaseDecPos" />
					</xsl:attribute>
					<xsl:apply-templates select="TaxDataAry/TaxData" />
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="TotAmt" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="BaseDecPos" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="BaseFareCurrency" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
		<!--<FareRules>		
			<xsl:variable name="UKey"><xsl:value-of select="UniqueKey"/></xsl:variable>
			<xsl:apply-templates select="following-sibling::SegRelatedInfo[$UKey=UniqueKey]"/>
			<xsl:variable name="penvar" select="//PenMod/Pen"/>
			<Penalty>
			<xsl:if test="//PenMod/Pen!=''">
				<xsl:attribute name = "TicketRefundable">
					<xsl:choose>
						<xsl:when test="contains(//PenMod/Pen,'99')">N</xsl:when>
						<xsl:when test="contains(//PenMod/Pen,'00')">Y</xsl:when>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
				<CurrencyCode NumberOfDecimals = "{BaseDecPos}">><xsl:value-of select="BaseFareCurrency"/></CurrencyCode>
			</Penalty>
			<TicketByDate>!func:ConvertDate(<xsl:value-of select="LastTkDt"/>,yyyymmdd,yyyy-mm-dd)</TicketByDate>
			<Remark><xsl:apply-templates select="../FareConstruction[$UKey=UniqueKey]/FareConstructText"/></Remark>
		</FareRules> -->
	</xsl:template>
	<xsl:template match="SegRelatedInfo">
		<FareBasisCode>
			<xsl:value-of select="FIC" />
		</FareBasisCode>
		<!-- <BagInformation><xsl:value-of select="BagInfo"/></BagInformation>	-->
	</xsl:template>
	<!-- ************************************************************** -->
	<!--      Get individual Tax info  	                                    -->
	<!-- ************************************************************** -->
	<xsl:template match="TaxData">
		<xsl:variable name="TaxAmount" select="translate(string(Amt),'.','')" />
		<Tax>
			<xsl:attribute name="Code">
				<xsl:value-of select="Country" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:choose>
					<xsl:when test="starts-with($TaxAmount,'00000')">
						<xsl:value-of select="substring($TaxAmount, 6)" />
					</xsl:when>
					<xsl:when test="starts-with($TaxAmount,'0000')">
						<xsl:value-of select="substring($TaxAmount, 5)" />
					</xsl:when>
					<xsl:when test="starts-with($TaxAmount,'000')">
						<xsl:value-of select="substring($TaxAmount, 4)" />
					</xsl:when>
					<xsl:when test="starts-with($TaxAmount,'00')">
						<xsl:value-of select="substring($TaxAmount, 3)" />
					</xsl:when>
					<xsl:when test="starts-with($TaxAmount,'0')">
						<xsl:value-of select="substring($TaxAmount, 2)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$TaxAmount" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../BaseFareCurrency" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="../../BaseDecPos" />
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--*****************************************************************-->
	<!--			Hotel Segs								    -->
	<!--*****************************************************************-->
	<xsl:template match="HtlSeg">
		<HotelSegment>
			<ElementNumber>
				<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>H_</xsl:text><xsl:value-of select="HtlV" /><xsl:text>_</xsl:text><xsl:value-of select="HtlPropNum" /><xsl:text>_</xsl:text><xsl:value-of select="RateCode" /><xsl:text>_</xsl:text><xsl:value-of select="StartDt" />)</xsl:attribute>
				<xsl:value-of select="SegNum" />
			</ElementNumber>
			<TravelerElementNumber>
				<xsl:variable name="LNam" select="../HtlSegOptFlds/FldAry/Fld[ID = 'NL']/Contents" />
				<xsl:variable name="FNam" select="../HtlSegOptFlds/FldAry/Fld[ID = 'NF']/Contents" />
				<xsl:for-each select="//LNameInfo">
					<xsl:if test="$LNam=LName">
						<xsl:variable name="LNamPos" select="position()" />
						<xsl:if test="../FNameInfo[position()=$LNamPos]/FName=$FNam">
							<xsl:value-of select="position()" />
						</xsl:if>
					</xsl:if>
				</xsl:for-each>
			</TravelerElementNumber>
			<Hotel>
				<CheckInDate>!func:ConvertDate(<xsl:value-of select="StartDt" />, yyyymmdd, yyyy-mm-dd)</CheckInDate>
				<CheckOutDate>!func:ConvertDate(<xsl:value-of select="EndDt" />, yyyymmdd, yyyy-mm-dd)</CheckOutDate>
				<NumberOfNights>
					<xsl:value-of select="NumNights" />
				</NumberOfNights>
				<NumberOfPersons>
					<xsl:value-of select="NumPersons" />
				</NumberOfPersons>
				<ChainCode>
					<xsl:value-of select="HtlV" />
				</ChainCode>
				<ChainName>!func:Decode(Hotels,<xsl:value-of select="HtlV" />)</ChainName>
				<PropertyCode>
					<xsl:value-of select="HtlPropNum" />
				</PropertyCode>
				<PropertyName>
					<xsl:value-of select="PropName" />
				</PropertyName>
				<CityCode>
					<xsl:value-of select="Pt" />
				</CityCode>
				<CityName>!func:Decode(Airports, <xsl:value-of select="Pt" />)</CityName>
				<CurrencyCode DecimalPlaces="{RateDecPos}">
					<xsl:value-of select="Currency" />
				</CurrencyCode>
			</Hotel>
			<Rooms>
				<RateCategory>
					<xsl:value-of select="substring(string(RateCode),4,3)" />
				</RateCategory>
				<RoomType>
					<xsl:value-of select="substring(string(RateCode),1,3)" />
				</RoomType>
				<RoomTypeDescription>!func:Decode(RoomTypes,<xsl:value-of select="substring(string(RateCode),1,3)" />)</RoomTypeDescription>
				<ActionCode>
					<xsl:value-of select="Status" />
				</ActionCode>
				<NumberOfRooms>
					<xsl:value-of select="NumRooms" />
				</NumberOfRooms>
				<RateCode>
					<xsl:value-of select="substring(string(RateCode),4,3)" />
				</RateCode>
				<RateCodeDescription>
					<xsl:value-of select="substring(string(RateCode),4,3)" />
				</RateCodeDescription>
				<RateAmount>
					<xsl:value-of select="RateAmt" />
				</RateAmount>
				<RateChange>
					<xsl:value-of select="RateChgInd" />
				</RateChange>
				<RoomOptions>
					<xsl:apply-templates select="../HtlSegOptFlds/FldAry/Fld[ID = 'EX']" mode="Htl" />
					<xsl:apply-templates select="../HtlSegOptFlds/FldAry/Fld[ID = 'EC']" mode="Htl" />
					<xsl:apply-templates select="../HtlSegOptFlds/FldAry/Fld[ID = 'RA']" mode="Htl" />
					<xsl:apply-templates select="../HtlSegOptFlds/FldAry/Fld[ID = 'RD']" mode="Htl" />
					<xsl:apply-templates select="../HtlSegOptFlds/FldAry/Fld[ID = 'CR']" mode="Htl" />
				</RoomOptions>
			</Rooms>
			<ConfirmationNumber>
				<xsl:value-of select="ConfNum" />
			</ConfirmationNumber>
			<SupplementalInformation>
				<xsl:apply-templates select="../HtlSegOptFlds" />
			</SupplementalInformation>
		</HotelSegment>
	</xsl:template>
	<!-- *******************************-->
	<!--    Templates - Tier 2         -->
	<!-- *******************************-->
	<!--************************************************************************************-->
	<!--			      Air Itinerary Section				                              	    -->
	<!--************************************************************************************-->
	<xsl:template match="AirSeg | OpenAirSeg" mode="Segment">
		<Air>
			<xsl:attribute name="RPH">
				<xsl:value-of select="SegNum" />
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="self::OpenAirSeg">
					<xsl:apply-templates select="." />
				</xsl:when>
				<xsl:otherwise>
					<!--************************************************************************************-->
					<!--			Air Segments/Passive  segments  						 -->
					<!--************************************************************************************-->
					<xsl:variable name="zeroes">0000</xsl:variable>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="NumPsgrs" />
					</xsl:attribute>
					<xsl:attribute name="ResBookDesigCode">
						<xsl:value-of select="BIC" />
					</xsl:attribute>
					<xsl:attribute name="ActionCode">
						<xsl:value-of select="Status" />
					</xsl:attribute>
					<xsl:attribute name="DepartureDateTime">
						<xsl:variable name="deptime">
							<xsl:value-of select="substring(string($zeroes),1,4-string-length(StartTm))" />
							<xsl:value-of select="StartTm" />
						</xsl:variable>
						<xsl:value-of select="substring(Dt,1,4)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(Dt,5,2)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(Dt,7,2)" />
						<xsl:text>T</xsl:text>
						<xsl:value-of select="substring($deptime,1,2)" />
						<xsl:text>:</xsl:text>
						<xsl:value-of select="substring($deptime,3,2)" />
						<xsl:text>:00</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="ArrivalDateTime">
						<xsl:variable name="arrtime">
							<xsl:value-of select="substring(string($zeroes),1,4-string-length(EndTm))" />
							<xsl:value-of select="EndTm" />
						</xsl:variable>
						<xsl:value-of select="substring(Dt,1,4)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(Dt,5,2)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(Dt,7,2)" />
						<xsl:text>T</xsl:text>
						<xsl:value-of select="substring($arrtime,1,2)" />
						<xsl:text>:</xsl:text>
						<xsl:value-of select="substring($arrtime,3,2)" />
						<xsl:text>:00</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="FlightNumber">
						<xsl:value-of select="FltNum" />
					</xsl:attribute>
					<DepartureAirport>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="StartAirp" />
						</xsl:attribute>
					</DepartureAirport>
					<ArrivalAirport>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="EndAirp" />
						</xsl:attribute>
					</ArrivalAirport>
					<xsl:apply-templates select="following-sibling::AirSegOpAirV[1]" />
					<!--Equipment>
						<xsl:attribute name="AirEquipType"/>					
						<xsl:choose>
							<xsl:when test="COG = 'Y'">
								<xsl:attribute name="ChangeofGauge">1</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="ChangeofGauge">0</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</Equipment-->
					<MarketingAirline>
						<xsl:attribute name="Code">
							<xsl:value-of select="AirV" />
						</xsl:attribute>
					</MarketingAirline>
					<TPA_Extensions>
						<xsl:attribute name="DateChange">
							<xsl:value-of select="DayChg" />
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="FlownInd = 'Y'">
								<xsl:attribute name="FlownIndicator">1</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="FlownIndicator">0</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</TPA_Extensions>
				</xsl:otherwise>
			</xsl:choose>
		</Air>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Open Air  Segments										    -->
	<!--************************************************************************************-->
	<xsl:template match="OpenAirSeg">
		<xsl:attribute name="NumberInParty">
			<xsl:value-of select="NumPsgrs" />
		</xsl:attribute>
		<xsl:attribute name="ResBookDesigCode">
			<xsl:value-of select="BIC" />
		</xsl:attribute>
		<xsl:attribute name="DepartureDateTime">
			<xsl:value-of select="substring(Dt,1,4)" />
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(Dt,5,2)" />
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(Dt,7,2)" />
			<xsl:text>T</xsl:text>
			<xsl:value-of select="substring(StartTm,1,2)" />
			<xsl:text>:</xsl:text>
			<xsl:value-of select="substring(StartTm,3,2)" />
			<xsl:text>:00</xsl:text>
		</xsl:attribute>
		<xsl:attribute name="FlightNumber">
			<xsl:value-of select="FltNum" />
		</xsl:attribute>
		<DepartureAirport>
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="StartAirp" />
			</xsl:attribute>
		</DepartureAirport>
		<ArrivalAirport>
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="EndAirp" />
			</xsl:attribute>
		</ArrivalAirport>
		<xsl:apply-templates select="following-sibling::AirSegOpAirV[1]" />
		<MarketingAirline>
			<xsl:attribute name="Code">
				<xsl:value-of select="AirV" />
			</xsl:attribute>
		</MarketingAirline>
	</xsl:template>
	<xsl:template match="AirSegOpAirV">
		<OperatingAirline>
			<xsl:if test="OpAirVInfoAry/OpAirVInfo/AirV !=''">
				<xsl:attribute name="Code">
					<xsl:value-of select="OpAirVInfoAry/OpAirVInfo[position()=1]/AirV" />
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Name">
				<xsl:value-of select="OpAirVInfoAry/OpAirVInfo[position()=1]/AirVName" />
			</xsl:attribute>
		</OperatingAirline>
	</xsl:template>
	<!--************************************************************************************-->
	<!--						 Passenger Information         		                        -->
	<!--************************************************************************************-->
	<xsl:template match="LNameInfo" mode="pax">
		<xsl:call-template name="buildnames">
			<xsl:with-param name="PsgrsTot">
				<xsl:value-of select="NumPsgrs" />
			</xsl:with-param>
			<xsl:with-param name="PsgrsNum">1</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!--************************************************************************************-->
	<!-- 						Telephone									    -->
	<!--************************************************************************************-->
	<xsl:template match="PhoneInfo" mode="Phone">
		<xsl:if test="substring(Phone,4,1)!='E'">
			<Telephone>
				<!-- <xsl:attribute name="RPH"><xsl:value-of select="PhoneFldNum"/></xsl:attribute> -->
				<xsl:choose>
					<xsl:when test="Pt != ''">
						<xsl:attribute name="PhoneUseType">
							<xsl:choose>
								<xsl:when test="Type = 'R'">HOME</xsl:when>
								<xsl:when test="Type = 'B'">BUSINESS</xsl:when>
								<xsl:when test="Type = 'A'">AGENCY</xsl:when>
								<xsl:when test="Type = 'F'">FAX</xsl:when>
								<xsl:otherwise>OTHER</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="PhoneUseType">
							<xsl:choose>
								<xsl:when test="substring(Phone,4,1)='R'">HOME</xsl:when>
								<xsl:when test="substring(Phone,4,1)='B'">BUSINESS</xsl:when>
								<xsl:when test="substring(Phone,4,1)='A'">AGENCY</xsl:when>
								<xsl:when test="substring(Phone,4,1)='F'">FAX</xsl:when>
								<xsl:otherwise>OTHER</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="Pt != ''">
						<xsl:choose>
							<xsl:when test="Type = 'X'">
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="substring(Phone,2,30)" />
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="Phone" />
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<!-- <CityCode><xsl:value-of select="Pt"/></CityCode> -->
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="Type = 'X'">
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="substring(Phone,5,30)" />
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="Type = ''">
								<xsl:variable name="email">
									<xsl:choose>
										<xsl:when test="contains(Phone,'--')">
											<xsl:value-of select="substring-before(Phone,'--')" />
											<xsl:text>_</xsl:text>
											<xsl:value-of select="substring-after(Phone,'--')" />
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="Phone" />
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:variable name="email1">
									<xsl:choose>
										<xsl:when test="contains($email,'//')">
											<xsl:value-of select="substring-before($email,'//')" />
											<xsl:text>@</xsl:text>
											<xsl:value-of select="substring-after($email,'//')" />
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="." />
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="substring($email1,6,30)" />
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="substring(Phone,6,30)" />
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<!-- <CityCode><xsl:value-of select="substring(Phone,1,3)"/></CityCode> -->
					</xsl:otherwise>
				</xsl:choose>
			</Telephone>
		</xsl:if>
	</xsl:template>
	<!--************************************************************************************-->
	<!--		EmailAddress  Processing									    -->
	<!--************************************************************************************-->
	<xsl:template match="PhoneInfo" mode="Email">
		<xsl:if test="substring(Phone,4,1)='E'">
			<Email>
				<!-- <xsl:attribute name="RPH"><xsl:value-of select="PhoneFldNum"/></xsl:attribute> -->
				<xsl:variable name="PEmail">
					<xsl:value-of select="substring(Phone,6,30)" />
				</xsl:variable>
				<xsl:variable name="email">
					<xsl:choose>
						<xsl:when test="contains($PEmail,'--')">
							<xsl:value-of select="substring-before($PEmail,'--')" />
							<xsl:text>_</xsl:text>
							<xsl:value-of select="substring-after($PEmail,'--')" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PEmail" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:value-of select="substring-before($email,'//')" />
				<xsl:text>@</xsl:text>
				<xsl:value-of select="substring-after($email,'//')" />
			</Email>
		</xsl:if>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Address/Delivery Addres information						    -->
	<!--************************************************************************************-->
	<xsl:template match="AddrInfo">
		<Address>
			<xsl:attribute name="Type">Home</xsl:attribute>
			<xsl:attribute name="UseType">Mailing</xsl:attribute>
			<StreetNmbr>
				<xsl:value-of select="Addr" />
			</StreetNmbr>
			<CityName />
			<PostalCode />
			<StateProv />
			<CountryName />
		</Address>
	</xsl:template>
	<xsl:template match="DeliveryAddrInfo">
		<Address>
			<xsl:attribute name="Type">Home</xsl:attribute>
			<xsl:attribute name="UseType">Delivery</xsl:attribute>
			<StreetNmbr>
				<xsl:value-of select="DeliveryAddr" />
			</StreetNmbr>
			<CityName />
			<PostalCode />
			<StateProv />
			<CountryName />
		</Address>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Form of Payment       						                       -->
	<!--************************************************************************************-->
	<xsl:template match="CreditCardFOP">
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()" />
		</xsl:attribute>
		<PaymentCard>
			<xsl:attribute name="CardCode">
				<xsl:value-of select="Vnd" />
			</xsl:attribute>
			<xsl:attribute name="CardNumber">
				<xsl:value-of select="translate(string(Acct),' ','')" />
			</xsl:attribute>
			<xsl:attribute name="ExpireDate">
				<xsl:variable name="zeroes">000</xsl:variable>
				<xsl:variable name="expdate">
					<xsl:value-of select="substring(string($zeroes),1,4-string-length(ExpDt))" />
					<xsl:value-of select="ExpDt" />
				</xsl:variable>
				<xsl:value-of select="substring($expdate, 1, 2)" />
				<xsl:text>/20</xsl:text>
				<xsl:value-of select="substring($expdate, 3, 2)" />
			</xsl:attribute>
		</PaymentCard>
		<TPA_Extensions>
			<xsl:attribute name="FOPType">CC</xsl:attribute>
		</TPA_Extensions>
	</xsl:template>
	<xsl:template match="OtherFOP">
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()" />
		</xsl:attribute>
		<TPA_Extensions>
			<xsl:attribute name="FOPType">
				<xsl:choose>
					<xsl:when test="ID='1'">CASH</xsl:when>
					<xsl:when test="ID='3'">NONREF</xsl:when>
					<xsl:when test="ID='4'">MISCELLENEOUS</xsl:when>
					<xsl:when test="ID='5'">INVOICE</xsl:when>
					<xsl:when test="ID='7'">GOVERNMENT</xsl:when>
					<xsl:when test="ID='8'">PSEUDOCASH</xsl:when>
					<xsl:when test="ID='9'">CREDIT</xsl:when>
					<xsl:when test="ID='11'">AKBONS</xsl:when>
					<xsl:when test="ID='12'">PAYLATE</xsl:when>
					<xsl:when test="ID='13'">EXCHANGE</xsl:when>
					<xsl:when test="ID='14'">MONEYORDER</xsl:when>
					<xsl:when test="ID='15'">TRAVELERSCHECK</xsl:when>
					<xsl:otherwise>OTHER</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</TPA_Extensions>
	</xsl:template>
	<xsl:template match="CheckFOP">
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()" />
		</xsl:attribute>
		<TPA_Extensions>
			<xsl:attribute name="FOPType">CHECK</xsl:attribute>
		</TPA_Extensions>
	</xsl:template>
	<!--************************************************************************************-->
	<!--						Seats Processing		  				           -->
	<!--************************************************************************************-->
	<xsl:template match="SeatSeg">
		<SeatRequest>
			<!-- <xsl:attribute name="RPH"><xsl:text>0</xsl:text></xsl:attribute> -->
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:value-of select="FltSegNum" />
			</xsl:attribute>
			<xsl:apply-templates select="following-sibling::SeatProcessing[not(. = current()/following-sibling::SeatSeg/following-sibling::SeatProcessing)]" />
			<xsl:variable name="SegmentNo" select="FltSegNum" />
			<xsl:variable name="PaxNum" select="NumPsgrs" />
			<xsl:for-each select="following-sibling::SeatAssignment[position() &lt;= $PaxNum]">
				<xsl:if test="$PaxNum != '0'">
					<xsl:call-template name="SeatProcessing">
						<xsl:with-param name="fltnum" select="$SegmentNo" />
						<xsl:with-param name="pcount" select="$PaxNum" />
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</SeatRequest>
	</xsl:template>
	<!--************************************************************************************-->
	<!--							Seats   									    -->
	<!--************************************************************************************-->
	<xsl:template name="SeatProcessing">
		<xsl:if test="Locn != ''">
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:value-of select="AbsNameNum" />
			</xsl:attribute>
			<xsl:attribute name="SeatLocation">
				<xsl:value-of select="Locn" />
			</xsl:attribute>
			<xsl:attribute name="Characteristic">
				<xsl:apply-templates select="AttribAry/Attrib" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="AttribAry/Attrib">
		<xsl:value-of select="." />
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Special Service Request (SSR) Processing				    -->
	<!--************************************************************************************-->
	<xsl:template match="ProgramaticSSR">
		<SpecialServiceRequest>
			<!-- <xsl:attribute name="RPH"><xsl:value-of select="GFAXNum"/></xsl:attribute> -->
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:value-of select="SegNum" />
			</xsl:attribute>
			<xsl:apply-templates select="AppliesToAry/AppliesTo" />
			<xsl:attribute name="SSRCode">
				<xsl:value-of select="SSRCode" />
			</xsl:attribute>
			<Airline>
				<xsl:value-of select="../AirSeg[SegNum=current()/SegNum]/AirV" />
			</Airline>
			<xsl:if test="following-sibling::ProgramaticSSRText[1]/Text!=''">
				<Text>
					<xsl:value-of select="following-sibling::ProgramaticSSRText[1]/Text" />
				</Text>
			</xsl:if>
		</SpecialServiceRequest>
	</xsl:template>
	<xsl:template match="AppliesTo">
		<xsl:attribute name="TravelerRefNumberRPHList">
			<xsl:value-of select="LNameNum" />
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="NonProgramaticSSR">
		<SpecialServiceRequest>
			<!-- <xsl:attribute name="RPH"><xsl:value-of select="GFAXNum"/></xsl:attribute> -->
			<xsl:attribute name="SSRCode">
				<xsl:value-of select="SSRCode" />
			</xsl:attribute>
			<xsl:if test="Vnd!=''">
				<Airline>
					<xsl:value-of select="Vnd" />
				</Airline>
			</xsl:if>
			<Text>
				<xsl:value-of select="SSRText" />
			</Text>
		</SpecialServiceRequest>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Other Service Requests (OSI) Processing			           -->
	<!--************************************************************************************-->
	<xsl:template match="OSI">
		<OtherServiceInformation>
			<!-- <xsl:attribute name="RPH"><xsl:value-of select="GFAXNum"/></xsl:attribute> -->
			<Airline>
				<xsl:value-of select="OSIV" />
			</Airline>
			<Text>
				<xsl:value-of select="OSIMsg" />
			</Text>
		</OtherServiceInformation>
	</xsl:template>
	<!--************************************************************************************-->
	<!--						General Remarks			                               -->
	<!--************************************************************************************-->
	<xsl:template match="GenRmkInfo">
		<Remark>
			<xsl:value-of select="GenRmk" />
		</Remark>
	</xsl:template>
	<!--************************************************************************************-->
	<!--						 Itinerary Remarks							    -->
	<!--************************************************************************************-->
	<xsl:template match="ItinRmk">
		<SpecialRemark>
			<xsl:choose>
				<xsl:when test="../AirSeg[SegNum=current()/SegNum]">
					<!-- <xsl:attribute name="RPH"><xsl:value-of select="RmkNum"/>"/></xsl:attribute> -->
					<xsl:attribute name="RemarkType">Itinerary</xsl:attribute>
					<FlightRefNumber>
						<xsl:attribute name="RPH">
							<xsl:value-of select="SegNum" />
						</xsl:attribute>
					</FlightRefNumber>
					<Text>
						<xsl:value-of select="Rmk" />
					</Text>
				</xsl:when>
				<!-- <xsl:when test="../HtlSeg[SegNum=current()/SegNum]">
					<xsl:attribute name="RPH"><xsl:value-of select="RmkNum"/>"/></xsl:attribute>
					<xsl:attribute name="RemarkType">Hotel</xsl:attribute>
					<xsl:attribute name="HotelRefNumber"><xsl:value-of select="SegNum"/></xsl:attribute>
					<Text>
						<xsl:value-of select="Rmk"/>
					</Text>
				</xsl:when>
				<xsl:when test="../CarSeg[SegNum=current()/SegNum]">
					<xsl:attribute name="RPH"><xsl:value-of select="RmkNum"/>"/></xsl:attribute>
					<xsl:attribute name="RemarkType">Car</xsl:attribute>
					<xsl:attribute name="CarRefNumber"><xsl:value-of select="SegNum"/></xsl:attribute>
					<Text>
						<xsl:value-of select="Rmk"/>
					</Text>
				</xsl:when> -->
			</xsl:choose>
		</SpecialRemark>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Document Invoice	Remarks							    -->
	<!--************************************************************************************-->
	<xsl:template match="InvoiceRmk">
		<SpecialRemark>
			<!-- <xsl:attribute name="RPH"><xsl:value-of select="ItemNum"/></xsl:attribute> -->
			<xsl:attribute name="RemarkType">Invoice</xsl:attribute>
			<!-- <Key>
				<xsl:choose>
					<xsl:when test="Keyword='3001'">DY</xsl:when>
					<xsl:when test="Keyword='3002'">DS</xsl:when>
					<xsl:when test="Keyword='3003'">CR</xsl:when>
					<xsl:when test="Keyword='3004'">PF</xsl:when>
					<xsl:when test="Keyword='3005'">NP</xsl:when>
					<xsl:when test="Keyword='3006'">TK</xsl:when>
					<xsl:when test="Keyword='3007'">CA</xsl:when>
					<xsl:when test="Keyword='3008'">BB</xsl:when>
					<xsl:when test="Keyword='3009'">AC</xsl:when>
					<xsl:when test="Keyword='3010'">FT</xsl:when>
					<xsl:when test="Keyword='3011'">DF</xsl:when>
					<xsl:when test="Keyword='3012'">PT</xsl:when>
					<xsl:when test="Keyword='3013'">SF</xsl:when>
					<xsl:when test="Keyword='3014'">SA</xsl:when>
					<xsl:when test="Keyword='3015'">SD</xsl:when>
					<xsl:when test="Keyword='3016'">CD</xsl:when>
					<xsl:when test="Keyword='3017'">RC</xsl:when>
					<xsl:when test="Keyword='3018'">FA</xsl:when>
					<xsl:when test="Keyword='3019'">PH</xsl:when>
					<xsl:otherwise>FT</xsl:otherwise>
				</xsl:choose>
			</Key> -->
			<Text>
				<xsl:value-of select="Rmk" />
			</Text>
		</SpecialRemark>
	</xsl:template>
	<!-- ***********************************************************-->
	<!--  Vendor Record locators/Ticketing info      	     -->
	<!-- ********************************************************** -->
	<xsl:template match="GenPNRInfo">
		<xsl:if test="../../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo/TkNum">
			<Ticketing>
				<xsl:choose>
					<xsl:when test="ETkDataExistInd = 'Y'">
						<xsl:attribute name="TicketType">eTicket</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="TicketType">Paper</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="TkNumExistInd='Y'">
					<xsl:attribute name="eTicketNumber">
						<xsl:value-of select="../../DocProdDisplayStoredQuote/AdditionalPsgrFareInfo/TkNum" />
					</xsl:attribute>
				</xsl:if>
			</Ticketing>
		</xsl:if>
		<!-- <xsl:if test="../VndRecLocs/RecLocInfoAry/RecLocInfo!=''">
			<VendorRecLocators>
				<xsl:apply-templates select="../VndRecLocs/RecLocInfoAry/RecLocInfo"/>
			</VendorRecLocators>
		</xsl:if>-->
	</xsl:template>
	<!-- ***********************************************************-->
	<!-- ***                               Tier 3                                  *-->
	<!-- ***********************************************************-->
	<!--************************************************************************************-->
	<!--						Passenger 	Names 							    -->
	<!--************************************************************************************-->
	<xsl:template name="buildnames">
		<xsl:param name="PsgrsTot" />
		<xsl:param name="PsgrsNum" />
		<xsl:if test="$PsgrsTot > 0">
			<xsl:variable name="ItemNo">
				<xsl:value-of select="LNameNum" />
			</xsl:variable>
			<CustomerInfo>
				<xsl:attribute name="RPH">
					<xsl:value-of select="LNameNum" />
				</xsl:attribute>
				<Customer>
					<PersonName>
						<xsl:attribute name="NameType">
							<xsl:value-of select="../NameRmkInfo[LNameNum=$ItemNo]/NameRmk" />
						</xsl:attribute>
						<GivenName>
							<xsl:value-of select="following-sibling::FNameInfo[position()=$PsgrsNum]/FName" />
						</GivenName>
						<Surname>
							<xsl:value-of select="LName" />
						</Surname>
					</PersonName>
					<xsl:apply-templates select="../PhoneInfo" mode="Phone" />
					<xsl:apply-templates select="../PhoneInfo" mode="Email" />
					<xsl:apply-templates select="../AddrInfo" />
					<xsl:apply-templates select="../DeliveryAddrInfo" />
				</Customer>
			</CustomerInfo>
			<xsl:call-template name="buildnames">
				<xsl:with-param name="PsgrsTot">
					<xsl:value-of select="$PsgrsTot - 1" />
				</xsl:with-param>
				<xsl:with-param name="PsgrsNum">
					<xsl:value-of select="$PsgrsNum+ 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="LNameInfo" mode="Inf">
		<CustomerInfos>
			<xsl:attribute name="RPH">
				<xsl:value-of select="LNameNum" />
			</xsl:attribute>
			<Customer>
				<PersonName>
					<xsl:attribute name="NameType">INF</xsl:attribute>
					<Surname>
						<xsl:value-of select="LName" />
					</Surname>
					<GivenName>
						<xsl:value-of select="following-sibling::FNameInfo[1]/FName" />
					</GivenName>
				</PersonName>
			</Customer>
		</CustomerInfos>
	</xsl:template>
	<!-- ***********************************************************-->
	<!--  Vendor Record locators        	                         -->
	<!-- ********************************************************** -->
	<xsl:template match="RecLocInfo">
		<RecordLocator>
			<xsl:variable name="zeroes">000</xsl:variable>
			<xsl:variable name="Airline">
				<xsl:value-of select="Vnd" />
			</xsl:variable>
			<xsl:for-each select="//AirSeg">
				<xsl:if test="AirV=$Airline">
					<xsl:attribute name="RPH">
						<xsl:value-of select="SegNum" />
					</xsl:attribute>
				</xsl:if>
			</xsl:for-each>
			<xsl:attribute name="BookingReferenceID">
				<xsl:value-of select="RecLoc" />
			</xsl:attribute>
			<xsl:attribute name="DateBooked">
				<xsl:variable name="timestmp">
					<xsl:value-of select="substring(string($zeroes),1,4-string-length(TmStamp))" />
					<xsl:value-of select="TmStamp" />
				</xsl:variable>
				<xsl:value-of select="substring(DtStamp,1,4)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(DtStamp,5,2)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(DtStamp,7,2)" />
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring($timestmp,1,2)" />
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring($timestmp,3,2)" />
				<xsl:text>:00</xsl:text>
			</xsl:attribute>
			<MarketingAirline>
				<xsl:value-of select="Vnd" />
			</MarketingAirline>
		</RecordLocator>
	</xsl:template>
	<!-- ***********************************************************-->
	<!--           Hotel Processing           	                         -->
	<!-- ********************************************************** -->
	<xsl:template match="HtlSegOptFlds">
		<xsl:apply-templates select="FldAry/Fld[ID = 'BS']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'CD']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'RT']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'RG' or ID = 'RQ' or ID = 'RR'][position() = 1]" mode="RateInformation" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'DP']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'VC']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'AC']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'GT']" mode="Htl" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'TN' or ID = 'RL' or ID = 'MP'][position() = 1]" mode="TourInformation" />
		<xsl:apply-templates select="FldAry/Fld[ID = 'SI']" mode="Htl" />
	</xsl:template>
	<!--*****************************************************************-->
	<!--			Rate Information					    -->
	<!--*****************************************************************-->
	<!-- <xsl:template match="Fld" mode="RateInformation">
	<RateInformation>
		<xsl:apply-templates select="../Fld[ID = 'RR']" mode="Htl"/>
		<xsl:apply-templates select="../Fld[ID = 'RQ']" mode="Htl"/>
		<xsl:apply-templates select="../Fld[ID = 'RG']" mode="Htl"/>
	</RateInformation>
</xsl:template>-->
	<!-- <xsl:template match="Fld" mode="TourInformation">
	<TourInformation>
		<xsl:apply-templates select="../Fld[ID = 'TN']" mode="Htl"/>
		<xsl:apply-templates select="../Fld[ID = 'RL']" mode="Htl"/>
		<xsl:apply-templates select="../Fld[ID = 'MP']" mode="Htl"/>
	</TourInformation>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Hotel Options Processing					-->
	<!--************************************************************************************-->
	<xsl:template match="Fld" mode="Htl">
		<xsl:choose>
			<xsl:when test="ID = 'AC'">
				<AlternateCurrencyCode>
					<xsl:value-of select="Contents" />
				</AlternateCurrencyCode>
			</xsl:when>
			<xsl:when test="ID = 'AV'">
				<AdvDpo>
					<xsl:value-of select="Contents" />
				</AdvDpo>
			</xsl:when>
			<xsl:when test="ID = 'RG'">
				<Guaranteed>
					<xsl:value-of select="Contents" />
				</Guaranteed>
			</xsl:when>
			<xsl:when test="ID = 'RQ'">
				<Quoted>
					<xsl:value-of select="Contents" />
				</Quoted>
			</xsl:when>
			<xsl:when test="ID = 'RR'">
				<Requested>
					<xsl:value-of select="Contents" />
				</Requested>
			</xsl:when>
			<xsl:when test="ID = 'RA'">
				<RollawayAdult>
					<xsl:value-of select="Contents" />
				</RollawayAdult>
			</xsl:when>
			<xsl:when test="ID = 'RD'">
				<RollawayChild>
					<xsl:value-of select="Contents" />
				</RollawayChild>
			</xsl:when>
			<xsl:when test="ID = 'CR'">
				<Cribs>
					<xsl:value-of select="Contents" />
				</Cribs>
			</xsl:when>
			<xsl:when test="ID = 'EX'">
				<ExtraAdult>
					<xsl:value-of select="Contents" />
				</ExtraAdult>
			</xsl:when>
			<xsl:when test="ID = 'EC'">
				<ExtraChild>
					<xsl:value-of select="Contents" />
				</ExtraChild>
			</xsl:when>
			<xsl:when test="ID = 'VC'">
				<MerchantCurrencyCode>
					<xsl:value-of select="Contents" />
				</MerchantCurrencyCode>
			</xsl:when>
			<xsl:when test="ID = 'BS'">
				<BookingSource>
					<xsl:value-of select="Contents" />
				</BookingSource>
			</xsl:when>
			<xsl:when test="ID = 'RT'">
				<OverrideCorporateRate>
					<xsl:value-of select="Contents" />
				</OverrideCorporateRate>
			</xsl:when>
			<xsl:when test="ID = 'GT'">
				<PaymentGuarantee>
					<xsl:choose>
						<xsl:when test="contains(Contents, 'EXP')">
							<CreditCard>
								<CCCode>
									<xsl:value-of select="substring(Contents, 1, 2)" />
								</CCCode>
								<CCNumber>
									<xsl:value-of select="substring-before(substring(Contents, 3, string-length(Contents) - 4), 'EXP')" />
								</CCNumber>
								<CCExpiration>
									<Month>
										<xsl:value-of select="substring(substring-after(Contents,'EXP'), 3, 2)" />
									</Month>
									<Year>
										<xsl:value-of select="substring(substring-after(Contents,'EXP'), 1, 2)" />
									</Year>
									<Month>
										<xsl:value-of select="substring(substring-after(Contents,'EXP'), 1, 2)" />
									</Month>
									<Year>
										<xsl:value-of select="substring(substring-after(Contents,'EXP'), 3, 2)" />
									</Year>
								</CCExpiration>
							</CreditCard>
						</xsl:when>
						<xsl:otherwise>
							<Other>
								<xsl:value-of select="Contents" />
							</Other>
						</xsl:otherwise>
					</xsl:choose>
				</PaymentGuarantee>
			</xsl:when>
			<xsl:when test="ID = 'DP'">
				<Deposit>
					<xsl:value-of select="Contents" />
				</Deposit>
			</xsl:when>
			<xsl:when test="ID = 'AD'">
				<CustAddr>
					<xsl:value-of select="Contents" />
				</CustAddr>
			</xsl:when>
			<xsl:when test="ID = 'TN'">
				<Number>
					<xsl:value-of select="Contents" />
				</Number>
			</xsl:when>
			<xsl:when test="ID = 'RL'">
				<RoomLocation>
					<xsl:value-of select="Contents" />
				</RoomLocation>
			</xsl:when>
			<xsl:when test="ID = 'MP'">
				<MealPlan>
					<xsl:value-of select="Contents" />
				</MealPlan>
			</xsl:when>
			<xsl:when test="ID = 'CD'">
				<CorporateDiscountCode>
					<xsl:value-of select="Contents" />
				</CorporateDiscountCode>
			</xsl:when>
			<xsl:when test="ID = 'FG'">
				<FreqtGuestNo>
					<xsl:value-of select="Contents" />
				</FreqtGuestNo>
			</xsl:when>
			<xsl:when test="ID = 'FT'">
				<FreqtAirNo>
					<xsl:value-of select="Contents" />
				</FreqtAirNo>
			</xsl:when>
			<xsl:when test="ID = 'SI'">
				<AdditionalInformation>
					<xsl:value-of select="Contents" />
				</AdditionalInformation>
			</xsl:when>
			<xsl:when test="ID = 'NF'">
				<ResFirstName>
					<xsl:value-of select="Contents" />
				</ResFirstName>
			</xsl:when>
			<xsl:when test="ID = 'NL'">
				<ResLastName>
					<xsl:value-of select="Contents" />
				</ResLastName>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Text">
		<Text>
			<xsl:value-of select="." />
		</Text>
	</xsl:template>
	<!-- <xsl:template match="FreqCustInfo" mode="FrequentTravelerGroup">
	<FrequentTravelerGroup>
	<ElementNumber>
		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>FF</xsl:text>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="LNameNum"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="FreqCustV"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="FreqCustNum"/>)</xsl:attribute>
		<xsl:value-of select="LNameNum"/>
	</ElementNumber>
		<FFCompanyCode><xsl:value-of select="FreqCustV"/></FFCompanyCode>
		<FFNumber><xsl:value-of select="FreqCustNum"/></FFNumber>
		<FFProgramName>!func:Decode(Airlines, <xsl:value-of select="FreqCustV"/>)</FFProgramName> 
		<xsl:if test="FreqCustStatus!=''">
			<FFLoyaltyLevel><xsl:value-of select="FreqCustStatus"/></FFLoyaltyLevel>
		</xsl:if>
	</FrequentTravelerGroup>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Customer ID									    -->
	<!--************************************************************************************-->
	<!-- <xsl:template match="CustID">
	<CustomerIdentification>
		<ElementNumber>
			<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>CI</xsl:text>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="Text"/>)</xsl:attribute>
		<xsl:text>0</xsl:text>
		</ElementNumber>
		<Text><xsl:value-of select="Text"/></Text>
	</CustomerIdentification>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Customer Profiles								    -->
	<!--************************************************************************************-->
	<!-- <xsl:template match="ProfileClientFileAssoc">
	<CustomerProfiles>
		<ElementNumber>
			<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:value-of select="ItemNum"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="PAR"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="BAR"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="MAR"/>)</xsl:attribute>
			<xsl:value-of select="ItemNum"/>
		</ElementNumber>
            <PassengerAccountRecord><xsl:value-of select="PAR"/></PassengerAccountRecord>
            <BusinessAccountRecord><xsl:value-of select="BAR"/></BusinessAccountRecord>
            <MasterAccountRecord><xsl:value-of select="MAR"/></MasterAccountRecord>
	</CustomerProfiles>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Due Paid text											    -->
	<!--************************************************************************************-->
	<!-- <xsl:template match="DuePaidInfo">
	<DuePaidText>
	 	<xsl:attribute name="Indicator">
	 		<xsl:value-of select="DuePaidTextInd"/>
	 	</xsl:attribute>
		<ElementNumber>
			<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:value-of select="SegNum"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="Type"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="Dt"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="Price"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
	 	<Type><xsl:value-of select="Type"/></Type>
	 	<Date>!func:ConvertDate(<xsl:value-of select="Dt"/>, yyyymmdd, yyyy-mm-dd)</Date>
	 	<Amount><xsl:value-of select="Price"/></Amount>
	 	<CurrencyCode><xsl:value-of select="Currency"/></CurrencyCode>
		<Text><xsl:value-of select="Rmk"/></Text>
	</DuePaidText>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Ticketing Processing													-->
	<!--************************************************************************************-->
	<!-- <xsl:template match="TkArrangement">
	<Ticketing>
		<xsl:choose>
			<xsl:when test=". != '' ">
	    			<Action>TL</Action>
			</xsl:when>
			<xsl:otherwise>
	    			<Action>TI</Action>
			</xsl:otherwise>
		</xsl:choose>
	</Ticketing>
</xsl:template> -->
	<!--*************************************************************-->
	<!--			Prepaid							-->
	<!--*************************************************************-->
	<!-- <xsl:template match="TLTkArrangement">
	 <xsl:variable name="zeroes">000</xsl:variable>
	<Ticketing>
		<xsl:choose>
			<xsl:when test="Text='PTA'">
	    			<Action>PP</Action>
				<xsl:variable name="time">
					<xsl:value-of select="substring(string($zeroes),1,4-string-length(../AirSeg[1]/StartTm))"/>
					<xsl:value-of select="../AirSeg[1]/StartTm"/>
				</xsl:variable>
				<xsl:variable name="minutes">
					<xsl:value-of select="substring($time,3,2)"/>
				</xsl:variable>
				<xsl:variable name="hour">
					<xsl:value-of select="substring($time,1,2)"/>
				</xsl:variable>
				<xsl:choose>
				<xsl:when test="$minutes &lt; 30">
					<xsl:choose>
						<xsl:when test = "$hour = '00'">
	    						<Date>!func:Math(SubDate, (<xsl:value-of select="TkIssueDt"/>, 1, ddMMM, yyyy-mm-dd)</Date>
							<Time><xsl:value-of select="../AirSeg[1]/StartTm + 2330"/></Time>
						</xsl:when>
						<xsl:otherwise>
	    						<Date>!func:ConvertDate(<xsl:value-of select="TkIssueDt"/>, ddMMM, yyyy-mm-dd)</Date>
							<Time>
								<xsl:if test="$hour &lt; 10"><xsl:text>0</xsl:text></xsl:if>
								<xsl:value-of select="$hour - 01"/>:<xsl:value-of select="$minutes + 30"/>
							</Time>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$minutes = 30">
					<Date>!func:ConvertDate(<xsl:value-of select="TkIssueDt"/>, ddMMM, yyyy-mm-dd)</Date>
					<Time><xsl:value-of select="$hour"/>:00</Time>
				</xsl:when>
				<xsl:otherwise>
					<Date>!func:ConvertDate(<xsl:value-of select="TkIssueDt"/>, ddMMM, yyyy-mm-dd)</Date>
					<Time><xsl:value-of select="$hour"/>:<xsl:value-of select="$minutes - 30"/></Time>
				</xsl:otherwise>
				</xsl:choose> 
			</xsl:when> -->
	<!--*************************************************************-->
	<!--			TimeLimit							-->
	<!--************************************************************-->
	<!-- 	<xsl:otherwise>
	    			<Action>TL</Action>
				<xsl:choose>
					<xsl:when test = "TkIssueTm != ''">
	    					<Date>!func:ConvertDate(<xsl:value-of select="TkIssueDt"/>, ddMMM, yyyy-mm-dd)</Date>
						<xsl:variable name="time">
							<xsl:value-of select="substring(string($zeroes),1,4-string-length(TkIssueTm))"/>
							<xsl:value-of select="TkIssueTm"/>
						</xsl:variable>
						<Time><xsl:value-of select="substring($time,1,2)"/>:<xsl:value-of select="substring($time,3,2)"/></Time>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="//CarSeg[SegNum='1']">
							<Date>!func:ConvertDate(<xsl:value-of select="//CarSeg[position()= '1']/StartDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
						</xsl:if>
						<xsl:if test="//HtlSeg[SegNum='1']">
							<Date>!func:ConvertDate(<xsl:value-of select="//HtlSeg[position()= '1']/StartDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
						</xsl:if>
						<xsl:if test="//AirSeg[SegNum='1']">
							<xsl:variable name="time">
								<xsl:value-of select="substring(string($zeroes),1,4-string-length(//AirSeg[position()= '1']/StartTm))"/>
								<xsl:value-of select="//AirSeg[position()= '1']/StartTm"/>
							</xsl:variable>
							<xsl:variable name="minutes">
								<xsl:value-of select="substring($time,3,2)"/>
							</xsl:variable>
							<xsl:variable name="hour">
								<xsl:value-of select="substring($time,1,2)"/>
							</xsl:variable>
							<xsl:choose>
							<xsl:when test="$minutes &lt; 30">
								<xsl:choose>
								<xsl:when test = "$hour = '00'">
	   	 							<Date>!func:Math(SubDate, (<xsl:value-of select="//AirSeg[position()= '1']/Dt"/>, 1,yyyymmdd, yyyy-mm-dd)</Date>
									<Time><xsl:value-of select="../AirSeg[1]/StartTm + 2330"/></Time>
								</xsl:when>
								<xsl:otherwise>
									<Date>!func:ConvertDate(<xsl:value-of select="//AirSeg[position()= '1']/Dt"/>, yyyymmdd, yyyy-mm-dd)</Date>
									<Time>
										<xsl:if test="$hour &lt; 10"><xsl:text>0</xsl:text></xsl:if>
										<xsl:value-of select="$hour - 01"/>:<xsl:value-of select="$minutes + 30"/>
									</Time>
								</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="$minutes = 30">
								<Date>!func:ConvertDate(<xsl:value-of select="//AirSeg[position()= '1']/Dt"/>, yyyymmdd, yyyy-mm-dd)</Date>
								<Time><xsl:value-of select="$hour"/>:00</Time>
							</xsl:when>
							<xsl:otherwise>
								<Date>!func:ConvertDate(<xsl:value-of select="//AirSeg[position()= '1']/Dt"/>, yyyymmdd, yyyy-mm-dd)</Date>
								<Time><xsl:value-of select="$hour"/>:<xsl:value-of select="$minutes - 30"/></Time>
							</xsl:otherwise>
							</xsl:choose> 
						</xsl:if>
					</xsl:otherwise>
 					</xsl:choose>
				      <OfficeIdentification><xsl:value-of select="Airp"/></OfficeIdentification>
				</xsl:otherwise>
				</xsl:choose>
			</Ticketing>
</xsl:template>

<xsl:template match="TAUTkArrangement">	
	<Ticketing>
	    <Action>TL</Action>
            <Date>!func:ConvertDate(<xsl:value-of select="QTAUDt"/>, ddMMM, yyyy-mm-dd)</Date>
            <OfficeIdentification><xsl:value-of select="BranchPCC"/></OfficeIdentification>
	</Ticketing>
</xsl:template>

<xsl:template match="TAWTkArrangement">
	 <xsl:variable name="zeroes">000</xsl:variable>	
	<Ticketing>
	    <Action>TAW/</Action>
            <Date>!func:ConvertDate(<xsl:value-of select="QTAWDt"/>, ddMMM, yyyy-mm-dd)</Date>
		<xsl:variable name="time">
			<xsl:value-of select="substring(string($zeroes),1,4-string-length(QTAWTm))"/>
			<xsl:value-of select="QTAWTm"/>
		</xsl:variable>
		<Time>
			<xsl:value-of select="substring($time,1,2)"/>:<xsl:value-of select="substring($time,3,2)"/>
		</Time>
            <OfficeIdentification><xsl:value-of select="BranchPCC"/></OfficeIdentification>
	</Ticketing>
</xsl:template>

<xsl:template match="TAMTkArrangement">	
	<Ticketing>
	    <Action>TAM/</Action>
            <Date>!func:ConvertDate(<xsl:value-of select="TkMailDt"/>, ddMMM, yyyy-mm-dd)</Date>
	</Ticketing>
</xsl:template> -->
	<!--*******************************************************************************-->
	<!--			Process Car Segments								    -->
	<!--*******************************************************************************-->
	<!-- <xsl:template match="CarSeg">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<CarSegment>
		<ElementNumber>
	    		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>C_</xsl:text>
	    			<xsl:value-of select="CarV"/>
	    			<xsl:text>_</xsl:text>
				<xsl:value-of select="CarType"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="Airp"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartDt"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
		<xsl:variable name="zeroes">000</xsl:variable>
		<TravelerElementNumber>
			<xsl:variable name="LNam" select="../CarSegOptFlds/FldAry/Fld[ID = 'NL']/Contents"/>
			<xsl:variable name="FNam" select="../CarSegOptFlds/FldAry/Fld[ID = 'NF']/Contents"/>
			<xsl:for-each select="//LNameInfo">
				<xsl:if test="$LNam=LName">
				<xsl:variable name="LNamPos" select="position()"/>
					<xsl:if test="../FNameInfo[position()=$LNamPos]/FName=$FNam">
						<xsl:value-of select="position()"/>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</TravelerElementNumber>
		<NumberOfCars><xsl:value-of select="NumCars"/></NumberOfCars>
		<PickUp>
			<xsl:choose>
		    		<xsl:when test="ActualStartPt !=''">
	    				<AirportCode><xsl:value-of select="ActualStartPt"/></AirportCode>
	    				<AirportName>!func:Decode(Airports, <xsl:value-of select="ActualStartPt"/>)</AirportName>
		    		</xsl:when>
		    		<xsl:otherwise>
					<AirportCode><xsl:value-of select="Airp"/></AirportCode>
					<AirportName>!func:Decode(Airports, <xsl:value-of select="Airp"/>)</AirportName>
			    </xsl:otherwise>
			</xsl:choose>	
			<Date>!func:ConvertDate(<xsl:value-of select="StartDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
			<xsl:variable name="time">
				<xsl:value-of select="substring(string($zeroes),1,4-string-length(StartTm))"/>
				<xsl:value-of select="StartTm"/>
			</xsl:variable>
			<Time>
				<xsl:value-of select="substring($time,1,2)"/>:<xsl:value-of select="substring($time,3,2)"/>
			</Time>
			<xsl:if test="StartAirV != ''">
				<FlightArrival>
					<AirlineCode><xsl:value-of select="StartAirV"/></AirlineCode>
					<AirlineName>!func:Decode(Airlines, <xsl:value-of select="StartAirV"/>)</AirlineName>
					<FlightNumber>
						<xsl:if test="OpSuf !=''">
	    						<xsl:attribute name="Suffix">
	    							<xsl:value-of select="OpSuf"/>
	    						</xsl:attribute>
						</xsl:if >
					<xsl:value-of select="StartFltNum"/></FlightNumber>
				</FlightArrival>
			</xsl:if>
		</PickUp>
		<DropOff>
			<xsl:choose>
				<xsl:when test="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']">
					<xsl:choose>
						<xsl:when test="string-length(following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents) = '3'">	
							<AirportCode><xsl:value-of select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents"/></AirportCode>
							<AirportName>!func:Decode(Airports, <xsl:value-of select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents"/>)</AirportName>
						</xsl:when>
						<xsl:otherwise>
							<Location><xsl:value-of select="substring(following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents, 1, 1)"/></Location>
							<LocationNumber><xsl:value-of select="substring(following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DO']/Contents, 2, string-length(Contents) - 1)"/></LocationNumber>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
		    				<xsl:when test="ActualStartPt !=''">
		    					<AirportCode><xsl:value-of select="ActualStartPt"/></AirportCode>
		    					<AirportName>!func:Decode(Airports, <xsl:value-of select="ActualStartPt"/>)</AirportName>
				    		</xsl:when>
				    		<xsl:otherwise>
							<AirportCode><xsl:value-of select="Airp"/></AirportCode>
							<AirportName>!func:Decode(Airports, <xsl:value-of select="Airp"/>)</AirportName>
						</xsl:otherwise>
					</xsl:choose>	
				</xsl:otherwise>
			</xsl:choose>
			<Date>!func:ConvertDate(<xsl:value-of select="EndDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
			<xsl:variable name="time">
				<xsl:value-of select="substring(string($zeroes),1,4-string-length(EndTm))"/>
				<xsl:value-of select="EndTm"/>
			</xsl:variable>
			<Time>
				<xsl:value-of select="substring($time,1,2)"/>:<xsl:value-of select="substring($time,3,2)"/>
			</Time>
		</DropOff>
		<CarData>
			<xsl:if test="RateType != ''">
				<xsl:attribute name="Type">
					<xsl:value-of select="RateType"/>
				</xsl:attribute>
			</xsl:if>
			<CarVendorCode><xsl:value-of select="CarV"/></CarVendorCode>
			<CarVendorName>!func:Decode(Cars,<xsl:value-of select="CarV"/>)</CarVendorName>
			<Location>
				<xsl:choose>
			    		<xsl:when test="ActualStartPt !=''">
		    				<CityCode><xsl:value-of select="ActualStartPt"/></CityCode>
			    		</xsl:when>
			    		<xsl:otherwise>
						<CityCode><xsl:value-of select="Airp"/></CityCode>
				      </xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="LocnCat !=''">
						<Category><xsl:value-of select="LocnCat"/></Category>
					</xsl:when>
					<xsl:when test="LocnNum !=''">
						<Number><xsl:value-of select="LocnNum"/></Number>
					</xsl:when>
					<xsl:when test="//Collect/Cars[position()=$pos]/LocnCat!=''">
						<Category><xsl:value-of select="//Collect/Cars[position()=$pos]/LocnCat"/></Category>
						<Number><xsl:value-of select="//Collect/Cars[position()=$pos]/LocnNum"/></Number>
					</xsl:when>
				</xsl:choose>
			</Location>
			<CarType><xsl:value-of select="CarType"/></CarType>
			<CarTypeDescription>!func:Decode(CarTypes,<xsl:value-of select="CarType"/>)</CarTypeDescription>
			<ActionCode><xsl:value-of select="Status"/></ActionCode>
			<xsl:if test="MilesOrKm != ''">
				<DistanceUnit><xsl:value-of select="MilesOrKm"/></DistanceUnit>
			</xsl:if>
			<Rate>
				<xsl:if test="RateType != ''">
					<xsl:attribute name="Type">
						<xsl:value-of select="RateType"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="RateGuarInd = 'G'">
					<xsl:attribute name="Guarantee">Y</xsl:attribute>
				</xsl:if>
				<xsl:if test="RateCat != ''">
					<xsl:attribute name="Category">!func:Decode(GxsCarRateCtgR,<xsl:value-of select="RateCat"/>,<xsl:value-of select="RateCat"/>)</xsl:attribute>
				</xsl:if>
				<RateCode><xsl:value-of select="RateCode"/></RateCode>
				<RateAmount><xsl:value-of select="RateAmt"/></RateAmount>		
				<CurrencyCode>
					<xsl:attribute name="NumberOfDecimals"><xsl:value-of select="DecPos"/></xsl:attribute>
					<xsl:value-of select="Currency"/>
				</CurrencyCode>
				<xsl:if test="MileRateAmt != '0'">
					<MileKmRate><xsl:value-of select="MileRateAmt"/></MileKmRate>
				</xsl:if>
				<xsl:if test="MileAllow!= ''">
					<MileKmLimit><xsl:value-of select="MileAllow"/></MileKmLimit>
				</xsl:if>
			</Rate>
			<xsl:if test="ExtraHourMileAllow !=''">
				<ExtraCharges>
					<xsl:attribute name="Type">H</xsl:attribute>
					<xsl:choose>
						<xsl:when test="ExtraHourMileRateAmt!='0'">
							<ExtraChargesAmount><xsl:value-of select="ExtraHourMileRateAmt"/></ExtraChargesAmount>
						</xsl:when>
						<xsl:otherwise>
							<ExtraChargesAmount><xsl:value-of select="ExtraHourRateAmt"/></ExtraChargesAmount>
						</xsl:otherwise>
					</xsl:choose>
					<MileKmLimit><xsl:value-of select="ExtraHourMileAllow"/></MileKmLimit>
				</ExtraCharges>
			</xsl:if>
			<xsl:if test="ExtraDayMileAllow !=''">
				<ExtraCharges>
					<xsl:attribute name="Type">D</xsl:attribute>
					<xsl:choose>
						<xsl:when test="ExtraHourMileRateAmt!='0'">
							<ExtraChargesAmount><xsl:value-of select="ExtraHourMileRateAmt"/></ExtraChargesAmount>
						</xsl:when>
						<xsl:otherwise>
							<ExtraChargesAmount><xsl:value-of select="ExtraDayRateAmt"/></ExtraChargesAmount>
						</xsl:otherwise>
					</xsl:choose>
					<MileKmLimit><xsl:value-of select="ExtraDayMileAllow"/></MileKmLimit>
				</ExtraCharges>
			</xsl:if>
			<xsl:if test = "following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DC']">
				<ExtraCharges>
					<xsl:attribute name="Type">F</xsl:attribute>
					<ExtraChargesAmount><xsl:value-of select="translate(substring(following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'DC']/Contents,4,8),'.','')"/></ExtraChargesAmount>
				</ExtraCharges>
			</xsl:if>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'SQ']"/>
		</CarData>
 		<ConfirmationNumber><xsl:value-of select="ConfNum"/></ConfirmationNumber> 
		<SupplementalInformation>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'BS']" mode="car"/>			
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'RT']" mode="car"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'CD']" mode="car"/>
			<xsl:apply-templates select="following-sibling::CarsegOptFlds/FldAry/Fld[ID = 'RG' or ID = 'RQ' or ID = 'RR'][position() = 1]" mode="RateInformation"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'VC']" mode="car"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'AC']" mode="car"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'GT']" mode="car"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'TN' or ID = 'RL' or ID = 'MP'][position() = 1]" mode="TourInformation"/>
			<xsl:apply-templates select="following-sibling::CarSegOptFlds/FldAry/Fld[ID = 'SI']" mode="car"/>
		</SupplementalInformation>
		<xsl:variable name="car"><xsl:value-of select="CarV"/></xsl:variable>
		<xsl:variable name="date"><xsl:value-of select="StartDt"/></xsl:variable>
		<xsl:variable name="type"><xsl:value-of select="CarType"/></xsl:variable>
		<xsl:variable name="city"><xsl:value-of select="Airp"/></xsl:variable>
		<xsl:if test="//Collect/CarInformation[Vendor = $car][Date = $date][City = $city][Type = $type]">
			<Information>
				<xsl:apply-templates select="//Collect/CarInformation[Vendor = $car][Date = $date][City = $city][Type = $type]/Text" mode="carinfo"/>
			</Information>
		</xsl:if>
	</CarSegment>
</xsl:template>

<xsl:template match="Text" mode="carinfo">
	<Text><xsl:value-of select="."/></Text>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			Car Options Processing						-->
	<!--************************************************************************************-->
	<!-- <xsl:choose>
		<xsl:when test="ID = 'BS'">
			<BookingSource><xsl:value-of select="Contents"/></BookingSource>
		</xsl:when>
		<xsl:when test="ID = 'RT'">
			<CorporateRateOverride><xsl:value-of select="Contents"/></CorporateRateOverride>
		</xsl:when>
		<xsl:when test="ID = 'ID'">	
			<CustomerID><xsl:value-of select="Contents"/></CustomerID>
		</xsl:when>
		<xsl:when test="ID = 'GT'">
			<PaymentGuarantee>
				<xsl:choose>
					<xsl:when test="contains(Contents, 'EXP')">
						<CreditCard>
							<CCCode><xsl:value-of select="substring(Contents, 1, 2)"/></CCCode>
							<CCNumber><xsl:value-of select="substring-before(substring(Contents, 3, string-length(Contents) - 4), 'EXP')"/></CCNumber>
							<CCExpiration>								
								<Month><xsl:value-of select="substring(substring-after(Contents,'EXP'), 1, 2)"/></Month>
								<Year><xsl:value-of select="substring(substring-after(Contents,'EXP'), 3, 2)"/></Year>
							</CCExpiration>
						</CreditCard>
					</xsl:when>
					<xsl:otherwise>
						<Other><xsl:value-of select="Contents"/></Other>
					</xsl:otherwise>
				</xsl:choose>
			</PaymentGuarantee>
		</xsl:when>
		<xsl:when test="ID = 'PU'">	
			<PickUpLoc><xsl:value-of select="Contents"/></PickUpLoc>
		</xsl:when>
		<xsl:when test="ID = 'AD'">	
			<CustAddr><xsl:value-of select="Contents"/></CustAddr>
		</xsl:when>
		<xsl:when test="ID = 'CD'">	
			<CorporateDiscountCode><xsl:value-of select="Contents"/></CorporateDiscountCode>
		</xsl:when>
		<xsl:when test="ID = 'SQ'">	
			<OptionalEquipment><Text><xsl:value-of select="Contents"/></Text></OptionalEquipment>
		</xsl:when>
		<xsl:when test="ID = 'PR'">	
			<PrePmtInfo><xsl:value-of select="Contents"/></PrePmtInfo>
		</xsl:when>
		<xsl:when test="ID = 'DL'">	
			<DrivLicense><xsl:value-of select="Contents"/></DrivLicense>
		</xsl:when>
		<xsl:when test="ID = 'FT'">	
			<FreqtAirNo><xsl:value-of select="Contents"/></FreqtAirNo>
		</xsl:when>
		<xsl:when test="ID = 'SI'">	
			<AdditionalInformation><xsl:value-of select="Contents"/></AdditionalInformation>
		</xsl:when>
		<xsl:when test="ID = 'DC'">	
			<DropCharge><xsl:value-of select="Contents"/></DropCharge>
		</xsl:when>
		<xsl:when test="ID = 'DO'">
			<xsl:choose>
				<xsl:when test="string-length(Contents) = 3">	
					<AirportCode><xsl:value-of select="Contents"/></AirportCode>
					<AirportName>!func:Decode(Airports, <xsl:value-of select="Contents"/>)</AirportName>
				</xsl:when>
				<xsl:otherwise>
					<Location><xsl:value-of select="substring(Contents, 1, 1)"/></Location>
					<LocationNumber><xsl:value-of select="substring(Contents, 2, string-length(Contents) - 1)"/></LocationNumber>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:when>
		<xsl:when test="ID = 'VC'">	
			<MerchantCurrencyCode><xsl:value-of select="Contents"/></MerchantCurrencyCode>
		</xsl:when>
		<xsl:when test="ID = 'AC'">	
			<AlternateCurrencyCode><xsl:value-of select="Contents"/></AlternateCurrencyCode>
		</xsl:when>
	</xsl:choose>
</xsl:template> -->
	<!--************************************************************************************-->
	<!--			NonAir Segs				   			-->
	<!--************************************************************************************-->
	<!-- <xsl:template match="NonAirSeg">
<xsl:if test="Type ='ATX'">
	<AirTaxiSegment>
		<ElementNumber>
	    		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>C_</xsl:text>
	    			<xsl:value-of select="Vnd"/>
	    			<xsl:text>_</xsl:text>
				<xsl:value-of select="substring(Text,1,4)"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartPt"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartDt"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
		<VendorCode><xsl:value-of select="Vnd"/></VendorCode>
		<ServiceDate>!func:ConvertDate(<xsl:value-of select="StartDt"/>, yyyymmdd, yyyy-mm-dd)</ServiceDate>
		<OriginCityCode><xsl:value-of select="StartPt"/></OriginCityCode>
		<DestinationCityCode><xsl:value-of select="EndPt"/></DestinationCityCode>
		<BookingStatus><xsl:value-of select="Status"/></BookingStatus>
		<NumberInParty><xsl:value-of select="NumPersons"/></NumberInParty>
		<xsl:if test="Text !=''">
			<xsl:apply-templates select="Text"/>
		</xsl:if>
	</AirTaxiSegment>
</xsl:if>
<xsl:if test="Type ='TUR'">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<TourSegment>
		<ElementNumber>
	    		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>C_</xsl:text>
	    			<xsl:value-of select="Vnd"/>
	    			<xsl:text>_</xsl:text>
				<xsl:value-of select="substring(Text,1,4)"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartPt"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartDt"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
		<VendorCode><xsl:value-of select="Vnd"/></VendorCode>
		<StartDate>!func:ConvertDate(<xsl:value-of select="StartDt"/>, yyyymmdd, yyyy-mm-dd)</StartDate>
		<CityCode><xsl:value-of select="StartPt"/></CityCode>
		<BookingStatus><xsl:value-of select="Status"/></BookingStatus>
		<NumberInParty><xsl:value-of select="NumPersons"/></NumberInParty>
		<xsl:if test="contains(Text,'-TN-')">
			<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'-TN-'),'-')!=''">
					<TourName>
						<xsl:value-of select="substring-before(substring-after(Text,'-TN-'),'-')"/>
					</TourName>
				</xsl:when>
				<xsl:otherwise>
					<TourName>
						<xsl:value-of select="substring-after(Text,'-TN-')"/>
					</TourName>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'-TC-')">
			<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'-TC-'),'-')!=''">
					<TourCode>
						<xsl:value-of select="substring-before(substring-after(Text,'-TC-'),'-')"/>
					</TourCode>
				</xsl:when>
				<xsl:otherwise>
					<TourCode>
						<xsl:value-of select="substring-after(Text,'-TC-')"/>
					</TourCode>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'-RT-')">
			<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'-RT-'),'-')!=''">
					<RoomCode>
						<xsl:value-of select="substring-before(substring-after(Text,'-RT-'),'-')"/>
					</RoomCode>
				</xsl:when>
				<xsl:otherwise>
					<RoomCode>
						<xsl:value-of select="substring-after(Text,'-RT-')"/>
					</RoomCode>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'-SI-')">
			<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'-SI-'),'-')!=''">
					<ServiceInformation>
						<xsl:value-of select="substring-before(substring-after(Text,'-SI-'),'-')"/>
					</ServiceInformation>
				</xsl:when>
				<xsl:otherwise>
					<ServiceInformation>
						<xsl:value-of select="substring-after(Text,'-SI-')"/>
					</ServiceInformation>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'-CF-')">
			<ConfirmationNumber><xsl:value-of select="substring-after(Text,'-CF-')"/></ConfirmationNumber>
		</xsl:if>
		<xsl:if test="Text!=''">
			<xsl:apply-templates select="Text"/>
		</xsl:if>
	</TourSegment>
</xsl:if>
<xsl:if test="Type ='CAR'">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<CarPassiveSegment>
		<ElementNumber>
	    		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>C_</xsl:text>
	    			<xsl:value-of select="Vnd"/>
	    			<xsl:text>_</xsl:text>
				<xsl:value-of select="substring(Text,1,4)"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartPt"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartDt"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
		<CarVendorCode><xsl:value-of select="Vnd"/></CarVendorCode>
		<PickUpInfo>
			<Date>!func:ConvertDate(<xsl:value-of select="StartDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
			<xsl:if test="contains(Text,'/ARR-')">
				<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'/ARR-'),'/')!=''">
					<Time>
						<xsl:value-of select="substring(substring-before(substring-after(Text,'/ARR-'),'/'),1,2)"/>:<xsl:value-of select="substring(substring-before(substring-after(Text,'/ARR-'),'/'),3,2)"/>
					</Time>
				</xsl:when>
				<xsl:otherwise>
					<Time>
						<xsl:value-of select="substring(substring-after(Text,'/ARR-'),1,2)"/>:<xsl:value-of select="substring(substring-after(Text,'/ARR-'),3,2)"/>
					</Time>
				</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<xsl:if test="contains(Text,'/PUP-')">
				<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'/PUP-'),'/')!=''">
					<CityCode>
						<xsl:value-of select="substring-before(substring-after(Text,'/PUP-'),'/')"/>
					</CityCode>
				</xsl:when>
				<xsl:otherwise>
					<CityCode>
						<xsl:value-of select="substring-after(Text,'/PUP-')"/>
					</CityCode>
				</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
 		</PickUpInfo>
		<DropOffInfo>
			<Date>!func:ConvertDate(<xsl:value-of select="EndDt"/>, yyyymmdd, yyyy-mm-dd)</Date>
			<xsl:if test="contains(Text,'/DT-')">
				<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'/DT-'),'/')!=''">
					<Time>
						<xsl:value-of select="substring(substring-before(substring-after(Text,'/DT-'),'/'),1,2)"/>:<xsl:value-of select="substring(substring-before(substring-after(Text,'/DT-'),'/'),3,2)"/>
					</Time>
				</xsl:when>
				<xsl:otherwise>
					<Time>
						<xsl:value-of select="substring(substring-after(Text,'/DT-'),1,2)"/>:<xsl:value-of select="substring(substring-after(Text,'/DT-'),3,2)"/>
					</Time>
				</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<xsl:if test="contains(Text,'/DO-')">
				<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'/DO-'),'/')!=''">
					<CityCode>
						<xsl:value-of select="substring-before(substring-after(Text,'/DO-'),'/')"/>
					</CityCode>
				</xsl:when>
				<xsl:otherwise>
					<CityCode>
						<xsl:value-of select="substring-after(Text,'/DO-')"/>
					</CityCode>
				</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</DropOffInfo>
		<CityCode><xsl:value-of select="StartPt"/></CityCode>
		<BookingStatus><xsl:value-of select="Status"/></BookingStatus>
		<NumberOfCars><xsl:value-of select="NumPersons"/></NumberOfCars>
		<CarType><xsl:value-of select="substring(Text,1,4)"/></CarType>
		<xsl:if test="contains(Text,'/BS-')">
			<xsl:choose>
			<xsl:when test="substring-before(substring-after(Text,'/BS-'),'/')!=''">
				<BookingSource>
					<xsl:value-of select="substring-before(substring-after(Text,'/BS-'),'/')"/>
				</BookingSource>
			</xsl:when>
			<xsl:otherwise>
				<BookingSource>
					<xsl:value-of select="substring-after(Text,'/BS-')"/>
				</BookingSource>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'/SI-')">
			<xsl:choose>
			<xsl:when test="substring-before(substring-after(Text,'/SI-'),'/')!=''">
				<ServiceInformation>
					<xsl:value-of select="substring-before(substring-after(Text,'/SI-'),'/')"/>
				</ServiceInformation>
			</xsl:when>
			<xsl:otherwise>
				<ServiceInformation>
					<xsl:value-of select="substring-after(Text,'/SI-')"/>
				</ServiceInformation>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="contains(Text,'/CF-')">
			<ConfirmationNumber>
				<xsl:value-of select="substring-after(Text,'/CF-')"/>
			</ConfirmationNumber>
		</xsl:if>
		<xsl:if test="Text!=''">
			<xsl:apply-templates select="Text"/>
		</xsl:if>
	</CarPassiveSegment>
</xsl:if>
<xsl:if test="Type ='HTL'">
	<HotelPassiveSegment>
		<ElementNumber>
	    		<xsl:attribute name="TattooNumber">!func:AddTattoo(<xsl:text>C_</xsl:text>
	    			<xsl:value-of select="Vnd"/>
	    			<xsl:text>_</xsl:text>
				<xsl:value-of select="substring(Text,1,4)"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartPt"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="StartDt"/>)</xsl:attribute>
			<xsl:value-of select="SegNum"/>
		</ElementNumber>
		<CarrierCode><xsl:value-of select="Vnd"/></CarrierCode>
		<CheckInDate>!func:ConvertDate(<xsl:value-of select="StartDt"/>, yyyymmdd, yyyy-mm-dd)</CheckInDate>
		<CheckOutDate>!func:ConvertDate(<xsl:value-of select="EndDt"/>, yyyymmdd, yyyy-mm-dd)</CheckOutDate>
		<CityCode><xsl:value-of select="StartPt"/></CityCode>
		<BookingStatus><xsl:value-of select="Status"/></BookingStatus>
		<NumberOfRooms><xsl:value-of select="NumPersons"/></NumberOfRooms>
		<xsl:if test="contains(Text,'/W-')">
			<HotelInformation>
				<xsl:choose>
				<xsl:when test="substring-before(substring-after(Text,'/W-'),'*')!=''">
					<HotelName><xsl:value-of select="substring-before(substring-after(Text,'/W-'),'*')"/></HotelName>
				</xsl:when>
				<xsl:when test="substring-before(substring-after(Text,'/W-'),'/')!=''">
					<HotelName><xsl:value-of select="substring-before(substring-after(Text,'/W-'),'/')"/></HotelName>
				</xsl:when>
				<xsl:otherwise>
					<HotelName><xsl:value-of select="substring-after(Text,'/W-')"/></HotelName>
				</xsl:otherwise>
				</xsl:choose>
				Address1
				<xsl:choose>
					<xsl:when test="substring-before(substring-after(Text,'*'),'*')!=''">
						<xsl:variable name="Text1">
							<xsl:value-of select="substring-before(substring-after(Text,'*'),'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text1)=12 and substring($Text1,4,1)='-' and substring($Text1,8,1)='-'">
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text1"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="substring-before(substring-after(Text,'*'),'/')!=''">
						<xsl:variable name="Text1">
							<xsl:value-of select="substring-before(substring-after(Text,'*'),'/')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text1)=12 and substring($Text1,4,1)='-' and substring($Text1,8,1)='-'">
								<TelephoneNumber><xsl:value-of select="$Text1"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text1"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					
					<xsl:when test="substring-after(Text,'*')!=''">
						<xsl:variable name="Text1">
							<xsl:value-of select="substring-after(Text,'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text1)=12 and substring($Text1,4,1)='-' and substring($Text1,8,1)='-'">
									<TelephoneNumber><xsl:value-of select="$Text1"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text1"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
				</xsl:choose>
								
				Address2
				<xsl:choose>
					<xsl:when test="substring-before(substring-after(substring-after(Text,'*'),'*'),'*')!=''">
						<xsl:variable name="Text2">
							<xsl:value-of select="substring-before(substring-after(substring-after(Text,'*'),'*'),'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text2)=12 and substring($Text2,4,1)='-' and substring($Text2,8,1)='-'">
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text2"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="substring-before(substring-after(substring-after(Text,'*'),'*'),'/')!=''">
						<xsl:variable name="Text2">
							<xsl:value-of select="substring-before(substring-after(substring-after(Text,'*'),'*'),'/')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text2)=12 and substring($Text2,4,1)='-' and substring($Text2,8,1)='-'">
									<TelephoneNumber><xsl:value-of select="$Text2"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text2"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="substring-after(substring-after(Text,'*'),'*')!=''">
						<xsl:variable name="Text2">
							<xsl:value-of select="substring-after(substring-after(Text,'*'),'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text2)=12 and substring($Text2,4,1)='-' and substring($Text2,8,1)='-'">
									<TelephoneNumber><xsl:value-of select="$Text2"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text2"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
				</xsl:choose>
				
				Address3			
				<xsl:choose>
					<xsl:when test="substring-before(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*')!=''">
						<xsl:variable name="Text3">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text3)=12 and substring($Text3,4,1)='-' and substring($Text3,8,1)='-'">
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text3"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="substring-before(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'/')!=''">
						<xsl:variable name="Text3">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'/')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text3)=12 and substring($Text3,4,1)='-' and substring($Text3,8,1)='-'">
									<TelephoneNumber><xsl:value-of select="$Text3"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text3"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="substring-after(substring-after(substring-after(Text,'*'),'*'),'*')!=''">
						<xsl:variable name="Text3">
							<xsl:value-of select="substring-after(substring-after(substring-after(Text,'*'),'*'),'*')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length($Text3)=12 and substring($Text3,4,1)='-' and substring($Text3,8,1)='-'">
									<TelephoneNumber><xsl:value-of select="$Text3"/></TelephoneNumber>
							</xsl:when>
							<xsl:otherwise>
								<Address><xsl:value-of select="$Text3"/></Address>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="substring-before(substring-after(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*'),'/')!=''">
						<xsl:variable name="Text4">
							<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*'),'/')"/>
						</xsl:variable>
						<xsl:if test="string-length($Text4)=12 and substring($Text4,4,1)='-' and substring($Text4,8,1)='-'">
								<TelephoneNumber><xsl:value-of select="$Text4"/></TelephoneNumber>
						</xsl:if>
					</xsl:when>
					<xsl:when test="substring-after(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*')!=''">
						<xsl:variable name="Text4">
							<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(Text,'*'),'*'),'*'),'*')"/>
						</xsl:variable>
						<xsl:if test="string-length($Text4)=12 and substring($Text4,4,1)='-' and substring($Text4,8,1)='-'">
								<TelephoneNumber><xsl:value-of select="$Text4"/></TelephoneNumber>
						</xsl:if>
					</xsl:when>
				</xsl:choose>
			</HotelInformation>
		</xsl:if>
		
		<xsl:if test="contains(Text,'/CF-')">
			<ConfirmationNumber><xsl:value-of select="substring-after(Text,'/CF-')"/></ConfirmationNumber>
		</xsl:if>
		<xsl:if test="Text !=''">
			<xsl:apply-templates select="Text"/>
		</xsl:if>
	</HotelPassiveSegment>
</xsl:if>
</xsl:template> -->
</xsl:stylesheet>
