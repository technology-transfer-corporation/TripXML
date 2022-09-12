<?xml version="1.0" ?>
<!-- ================================================================== -->
<!-- Galileo_LowFareRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 07 Mar 2012 - Rastko - CHD should be sent as CHD now to Galileo		-->
<!-- Date: 19 Apr 2011 - Rastko - made changes for Galileo certification			-->
<!-- Date: 29 Jul 2008 - Rastko														-->
<!-- ================================================================== -->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ" />
	</xsl:template>
	<!-- **********************************************************************************************************  -->
	<xsl:template match="OTA_AirLowFareSearchRQ">
		<FareQuoteSuperBB_21>
			<xsl:apply-templates select="OriginDestinationInformation" />
			<SuperBBMods>
				<Optimize>
					<RecType>1001</RecType>
					<KlrIDAry>
						<KlrID>AAFH</KlrID>
						<KlrID>AAFI</KlrID>
					</KlrIDAry>
				</Optimize>
				<Optimize>
					<RecType>1425</RecType>
					<KlrIDAry>
						<KlrID>EROR</KlrID>
						<KlrID>GFRH</KlrID>
						<KlrID>GFIS</KlrID>
						<KlrID>GFSR</KlrID>
						<KlrID>GFGQ</KlrID>
						<KlrID>GFMM</KlrID>
						<KlrID>GFPI</KlrID>
						<KlrID>GFRR</KlrID>
						<KlrID>GFXI</KlrID>
					</KlrIDAry>
				</Optimize>
				<xsl:if test="OriginDestinationInformation/TravelPreferences/CabinPref/@Cabin !=''">
					<ClassPrefs>
						<ODPairAry>
							<xsl:apply-templates select="OriginDestinationInformation/TravelPreferences/CabinPref" />
						</ODPairAry>
					</ClassPrefs>
				</xsl:if>
				<xsl:apply-templates select="TravelPreferences/FareRestrictPref" mode="restrictions" />
				<PassengerType>
					<PsgrAry>
						<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[1][@Quantity!='']">
							<xsl:with-param name="counter">1</xsl:with-param>
						</xsl:apply-templates>
					</PsgrAry>
				</PassengerType>
				<xsl:if test="TravelerInfoSummary/PriceRequestInformation[@PricingSource != 'Published']">
					<PFInfo>
						<ReqAirVPFs>Y</ReqAirVPFs>
						<PFAry>
							<xsl:choose>
								<xsl:when test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code != '' or TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode != ''">
									<xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
										<PF>
											<xsl:if test="../@PricingSource='Private' or ../@PricingSource='Both'">	
												<CRS>
													<xsl:choose>
														<xsl:when test="../../../POS/TPA_Extensions/Provider/Name = 'Apollo'">1V</xsl:when>
														<xsl:otherwise>1G</xsl:otherwise>
													</xsl:choose>
												</CRS>
												<PCC>
													<xsl:choose>
														<xsl:when test="@SupplierCode !='' and @SupplierCode !='1G' and 	@SupplierCode !='1V'">
															<xsl:value-of select="@SupplierCode" />
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="../../../POS/Source/@PseudoCityCode" />
														</xsl:otherwise>
													</xsl:choose>
												</PCC>
											</xsl:if>
											<xsl:if test="@Code != '' or 	@SecondaryCode != ''">
												<xsl:apply-templates select="." mode="fares" />
											</xsl:if>
											<PublishedFaresInd>
												<xsl:choose>
													<xsl:when test="../@PricingSource='Private'">N</xsl:when>
													<xsl:otherwise>Y</xsl:otherwise>
												</xsl:choose>
											</PublishedFaresInd>
											<Type>V</Type>
										</PF>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<PF>
										<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private' or 	TravelerInfoSummary/PriceRequestInformation/	@PricingSource='Both'">	
											<CRS>
												<xsl:choose>
													<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'">1V</xsl:when>
													<xsl:otherwise>1G</xsl:otherwise>
												</xsl:choose>
											</CRS>
											<PCC>
												<xsl:choose>
													<xsl:when test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SupplierCode !='' and 		TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SupplierCode !='1G' and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/		@SupplierCode !='1V'">
														<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SupplierCode" />
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="POS/Source/@PseudoCityCode" />
													</xsl:otherwise>
												</xsl:choose>
											</PCC>
										</xsl:if>
										<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code != '' or 		TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode != ''">
											<xsl:apply-templates select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode" mode="fares" />
										</xsl:if>
										<PublishedFaresInd>
											<xsl:choose>
												<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">N</xsl:when>
												<xsl:otherwise>Y</xsl:otherwise>
											</xsl:choose>
										</PublishedFaresInd>
										<Type>V</Type>
									</PF>
								</xsl:otherwise>
							</xsl:choose>
						</PFAry>
					</PFInfo>
				</xsl:if>
				<xsl:if test="POS/Source/@AirportCode != '' or POS/Source/@ISOCurrency != ''">
					<GenQuoteInfo>
						<SellCity>
							<xsl:value-of select="POS/Source/@AirportCode"/>
						</SellCity>
						<TktCity>
							<!--xsl:value-of select="POS/Source/@AirportCode"/-->
						</TktCity>
						<EquivCurrency>
							<xsl:value-of select="POS/Source/@ISOCurrency" />
						</EquivCurrency>
						<TkDt />
						<BkDtOverride />
					</GenQuoteInfo>
				</xsl:if>
			</SuperBBMods>
		</FareQuoteSuperBB_21>
	</xsl:template>
	<xsl:template match="PassengerTypeQuantity">
		<xsl:param name="counter" />
		<xsl:call-template name="create_each_pax_type">
			<xsl:with-param name="typeQ">
				<xsl:value-of select="@Quantity" />
			</xsl:with-param>
			<xsl:with-param name="counter">
				<xsl:value-of select="$counter" />
			</xsl:with-param>
		</xsl:call-template>
		<xsl:apply-templates select="following-sibling::PassengerTypeQuantity[1][@Quantity!='']">
			<xsl:with-param name="counter">
				<xsl:value-of select="$counter + @Quantity" />
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template name="create_each_pax_type">
		<xsl:param name="typeQ" />
		<xsl:param name="counter" />
		<xsl:if test="$typeQ != 0">
			<Psgr>
				<LNameNum>
					<xsl:value-of select="$counter" />
				</LNameNum>
				<PsgrNum>
					<xsl:value-of select="$counter" />
				</PsgrNum>
				<AbsNameNum>
					<xsl:value-of select="$counter" />
				</AbsNameNum>
				<PTC>
					<xsl:choose>
						<xsl:when test="@Code='ADT'">
							<xsl:value-of select="string(' ')" />
						</xsl:when>
						<!--xsl:when test="@Code='CHD'">
							<xsl:text>C10</xsl:text>
						</xsl:when-->
						<xsl:when test="@Code='INF'">
							<xsl:text>INF</xsl:text>
						</xsl:when>
						<xsl:when test="@Code='GOV'">
							<xsl:text>GOV</xsl:text>
						</xsl:when>
						<xsl:when test="@Code='MIL'">
							<xsl:text>MIL</xsl:text>
						</xsl:when>
						<xsl:when test="@Code='SRC'">
							<xsl:text>SC66</xsl:text>
						</xsl:when>
						<xsl:when test="@Code='STD'">
							<xsl:text>STU</xsl:text>
						</xsl:when>
						<xsl:when test="@Code='YTH'">
							<xsl:text>YC16</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="@Code" />
						</xsl:otherwise>
					</xsl:choose>
				</PTC>
				<Age>
					<![CDATA[     ]]> 
  				</Age>
				<PricePTCOnly>N</PricePTCOnly>
			</Psgr>
			<xsl:call-template name="create_each_pax_type">
				<xsl:with-param name="typeQ">
					<xsl:value-of select="$typeQ - 1" />
				</xsl:with-param>
				<xsl:with-param name="counter">
					<xsl:value-of select="$counter + 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!--Air Availability 							 							    	    -->
	<!--**********************************************************************************************-->
	<xsl:template match="OriginDestinationInformation">
		<xsl:variable name="DepDate">
			<xsl:value-of select="substring-before(DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<AirAvailMods>
			<!--xsl:if test="../TravelPreferences/CabinPref[@Cabin!='']">
				<BICPrefs>
					<BICPrefsAry>
						<BICPrefsInfo>
							<BIC>
								<xsl:choose>
									<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Economy'">Y</xsl:when>
									<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
									<xsl:when test="../TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
								</xsl:choose>
							</BIC>
							<AllowSmlrInd>Y</AllowSmlrInd>
						</BICPrefsInfo>
					</BICPrefsAry>
				</BICPrefs>
			</xsl:if-->
			<xsl:if test="../TravelPreferences/VendorPref/@Code!=''">
				<xsl:choose>
					<xsl:when test="../TravelPreferences/VendorPref/@PreferLevel = 'Unacceptable'">
						<AirVPrefInd>
							<AirVIncExcInd>E</AirVIncExcInd>
							<RelaxAirVPref>N</RelaxAirVPref>
							<SectorNum>0</SectorNum>
						</AirVPrefInd>
					</xsl:when>
					<xsl:when test="../TravelPreferences/VendorPref/@PreferLevel = 'Only'">
						<AirVPrefInd>
							<AirVIncExcInd>O</AirVIncExcInd>
							<RelaxAirVPref>N</RelaxAirVPref>
							<SectorNum>0</SectorNum>
						</AirVPrefInd>
					</xsl:when>
					<xsl:otherwise>
						<AirVPrefInd>
							<AirVIncExcInd>I</AirVIncExcInd>
							<RelaxAirVPref>N</RelaxAirVPref>
							<SectorNum>0</SectorNum>
						</AirVPrefInd>
					</xsl:otherwise>
				</xsl:choose>
				<AirVPrefs>
					<AirVAry>
						<xsl:apply-templates select="../TravelPreferences/VendorPref" />
					</AirVAry>
				</AirVPrefs>
			</xsl:if>
			<GenAvail>
				<NumSeats>
					<xsl:value-of select="../TravelerInfoSummary/SeatsRequested" />
				</NumSeats>
				<xsl:if test="../TravelPreferences/CabinPref/@Cabin !=''">
					<Class>
						<xsl:choose>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Economy'">Y</xsl:when>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
							<xsl:when test="../TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
						</xsl:choose>
					</Class>
				</xsl:if>
				<StartDt>
					<xsl:value-of select="translate(string($DepDate),'-','')" />
				</StartDt>
				<StartPt>
					<xsl:value-of select="OriginLocation/@LocationCode" />
				</StartPt>
				<EndPt>
					<xsl:value-of select="DestinationLocation/@LocationCode" />
				</EndPt>
				<StartTm>
					<xsl:value-of select="translate(string($DepTime2),':','')" />
				</StartTm>
				<!-- Start here going over the time window scenarios-->
				<TmWndInd>D</TmWndInd>
				<xsl:choose>
					<xsl:when test="DepartureDateTime/@WindowBefore !=''">
						<xsl:apply-templates select="DepartureDateTime/@WindowBefore"/>
					</xsl:when>
					<xsl:otherwise>
						<StartTmWnd>0000</StartTmWnd>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="DepartureDateTime/@WindowAfter!=''">
						<xsl:apply-templates select="DepartureDateTime/@WindowAfter"/>
					</xsl:when>
					<xsl:otherwise>
						<EndTmWnd>2359</EndTmWnd>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="../TravelPreferences/@MaxStopsQuantity">
						<xsl:apply-templates select="../TravelPreferences/@MaxStopsQuantity" />
					</xsl:when>
					<xsl:when test="../TravelPreferences/FlightTypePref/@FlightType != ''">
						<xsl:apply-templates select="../TravelPreferences/FlightTypePref/@FlightType" />
					</xsl:when>
					<xsl:when test="../TravelPreferences/VendorPref/@PreferLevel = 'Only'">
						<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
						<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
						<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
						<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
						<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
						<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
					</xsl:when>
				</xsl:choose>
				<!--xsl:choose>
					<xsl:when test="../TravelPreferences/@MaxStopsQuantity != ''">
						<SeqInd>P</SeqInd>
					</xsl:when>
					<xsl:when test="TravelPreferences/@Sort = 'ARRIVAL'">
						<SeqInd>O</SeqInd>
					</xsl:when>
					<xsl:when test="TravelPreferences/@Sort = 'ELAPSED'">
						<SeqInd>J</SeqInd>
					</xsl:when>
					<xsl:otherwise>
						<SeqInd>P</SeqInd>
					</xsl:otherwise>
				</xsl:choose-->
				<!--End here going over all these time and sort items -->
			</GenAvail>
			<xsl:apply-templates select="ConnectionLocations/ConnectionLocation" />
		</AirAvailMods>
	</xsl:template>
	<!--**********************************************************************************************-->
	<xsl:template match="VendorPref">
		<AirVInfo>
			<AirV>
				<xsl:value-of select="@Code" />
			</AirV>
		</AirVInfo>
	</xsl:template>
	<!--**********************************************************************************************-->
	<xsl:template match="ConnectionLocation">
		<xsl:choose>
			<xsl:when test="@PreferLevel = 'Only'">
				<ConxPrefInd>
					<xsl:choose>
						<xsl:when test="@PreferLevel = 'Only'">
							<IncExc>I</IncExc>
						</xsl:when>
						<xsl:otherwise>
							<IncExc>R</IncExc>
						</xsl:otherwise>
					</xsl:choose>
				</ConxPrefInd>
				<ConxPref>
					<PtAry>
						<PtInfo>
							<Pt>
								<xsl:value-of select="@LocationCode" />
							</Pt>
						</PtInfo>
					</PtAry>
				</ConxPref>
			</xsl:when>
			<xsl:when test="@PreferLevel = 'Unacceptable'">
				<ConxPrefInd>
					<IncExc>E</IncExc>
				</ConxPrefInd>
				<ConxPref>
					<PtAry>
						<PtInfo>
							<Pt>
								<xsl:value-of select="@LocationCode" />
							</Pt>
						</PtInfo>
					</PtAry>
				</ConxPref>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!--**********************************************************************************************-->
	<xsl:template match="CabinPref" mode="classpref">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<ODPair>
			<ODNum>
				<xsl:value-of select="$pos" />
			</ODNum>
			<xsl:if test="@Cabin!=''">
				<ClassPref>
					<xsl:choose>
						<xsl:when test="@Cabin='Economy'">Y</xsl:when>
						<xsl:when test="@Cabin='Business'">C</xsl:when>
						<xsl:when test="@Cabin='First'">F</xsl:when>
					</xsl:choose>
				</ClassPref>
			</xsl:if>
		</ODPair>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!-- Get PAX Penalty Tolerance									  -->
	<!--**********************************************************************************************-->
	<xsl:template match="FareRestrictPref" mode="restrictions">
		<GenFarePrefs>
			<xsl:choose>
				<xsl:when test="VoluntaryChanges/Penalty[not(@PenaltyType)]">
					<Pen>
						<xsl:value-of select="string('  ')" />
					</Pen>
				</xsl:when>
				<xsl:when test="VoluntaryChanges/Penalty/@PenaltyType = 'NonRef'">
					<Pen>99</Pen>
				</xsl:when>
				<xsl:when test="VoluntaryChanges/Penalty/@PenaltyType = 'Ref'">
					<Pen>00</Pen>
				</xsl:when>
				<xsl:otherwise>
					<Pen>00</Pen>
				</xsl:otherwise>
			</xsl:choose>
			<MinStay>
				<xsl:choose>
					<xsl:when test="StayRestrictions/MinimumStay">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MinStay>
			<MaxStay>
				<xsl:choose>
					<xsl:when test="StayRestrictions/MaximumStay">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MaxStay>
			<AP>
				<xsl:choose>
					<xsl:when test="AdvResTicketing/AdvReservation">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</AP>
		</GenFarePrefs>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!--Negotiated Fares 						  								  -->
	<!--**********************************************************************************************-->
	<xsl:template match="NegotiatedFareCode" mode="fares">
		<xsl:if test="@SecondaryCode != ''">
			<Acct>
				<xsl:value-of select="@SecondaryCode" />
			</Acct>
		</xsl:if>
		<xsl:if test="@Code != ''">
			<Contract>
				<xsl:value-of select="@Code" />
			</Contract>
		</xsl:if>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!-- Time Window						  -->
	<!--**********************************************************************************************-->
	<xsl:template match="@WindowBefore | @WindowAfter">
		<xsl:variable name="time"><xsl:value-of select="translate(substring(../../DepartureDateTime,12,5),':','')"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="name(.)='WindowBefore'">
				<StartTmWnd>
	                	<xsl:variable name="begin" select="($time - (. * 100))"/>
	                	<xsl:choose>
	                		<xsl:when test="$begin >= 0"><xsl:value-of select="format-number($begin,'0000')"/></xsl:when>
	                		<xsl:otherwise><xsl:value-of select="0000"/></xsl:otherwise>
	                	</xsl:choose>
	                </StartTmWnd>
			</xsl:when>
			<xsl:otherwise>
				<EndTmWnd>
	                	<xsl:variable name="end" select="($time + (. * 100))"/>
	                	<xsl:choose>
	                		<xsl:when test="$end &lt;= 2400"><xsl:value-of select="format-number($end,'0000')"/></xsl:when>
	                		<xsl:otherwise>2359</xsl:otherwise>
	                	</xsl:choose>
	                </EndTmWnd>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--**********************************************************************************************-->
	<xsl:template match="@MaxStopsQuantity">
		<xsl:variable name="VendorPosition2">
			<xsl:value-of select="../VendorPref[position()=2]/@Code" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test=". = '3' and $VendorPosition2  != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>Y</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = '3'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = '2' and $VendorPosition2 != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = '2'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = '1' and $VendorPosition2  != ''">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = '1'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="@FlightType">
		<xsl:choose>
			<xsl:when test=". = 'Direct'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>N</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = 'Nonstop'">
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>N</IncStopDirectsInd>
				<IncSingleOnlineConxInd>N</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>N</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>N</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>N</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>N</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>N</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:when test=". = 'Connection'">
				<IncNonStopDirectsInd>N</IncNonStopDirectsInd>
				<IncStopDirectsInd>N</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>Y</IncTripleInterlineConxInd>
			</xsl:when>
			<xsl:otherwise>
				<IncNonStopDirectsInd>Y</IncNonStopDirectsInd>
				<IncStopDirectsInd>Y</IncStopDirectsInd>
				<IncSingleOnlineConxInd>Y</IncSingleOnlineConxInd>
				<IncDoubleOnlineConxInd>Y</IncDoubleOnlineConxInd>
				<IncTripleOnlineConxInd>Y</IncTripleOnlineConxInd>
				<IncSingleInterlineConxInd>Y</IncSingleInterlineConxInd>
				<IncDoubleInterlineConxInd>Y</IncDoubleInterlineConxInd>
				<IncTripleInterlineConxInd>Y</IncTripleInterlineConxInd>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
