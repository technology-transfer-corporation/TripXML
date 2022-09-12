<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_AirFareDisplayRQ.xsl 														-->
<!-- ================================================================== -->
<!-- Date: 22 Nov 2005 -  Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirFareDisplayRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_AirFareDisplayRQ">
		<FareQuoteMultiDisplay_8_2>
			<FareDisplayMods>
				<QueryHeader>
					<UniqueKey>0000</UniqueKey>
					<LangNum>00</LangNum>
					<Action>002</Action>
					<RetCRTOutput>N</RetCRTOutput>
					<NoMsg>N</NoMsg>
					<NoTrunc>Y</NoTrunc>
					<IMInd>N</IMInd>
					<FIPlus>N</FIPlus>
					<PEInd>N</PEInd>
					<HostUse16>N</HostUse16>
					<NBInd>N</NBInd>
					<ActionOnlyInd>N</ActionOnlyInd>
					<TranslatePeriod>N</TranslatePeriod>
					<GFYInd>N</GFYInd>
					<IntFrame1>Y</IntFrame1>
					<SmartParsed>Y</SmartParsed>
					<PDCodes>N</PDCodes>
					<BkDtOverride>N</BkDtOverride>
					<HostUse25>N</HostUse25>
					<DefCurrency>N</DefCurrency>
					<PFPWInd>Y</PFPWInd>
					<HostUse28>N</HostUse28>
					<HostUse29>N</HostUse29>
					<HostUse30>N</HostUse30>
					<HostUse31>N</HostUse31>
					<DefCurrencyLocInd>N</DefCurrencyLocInd>
					<HostUse33>N</HostUse33>
					<TariffQual>
						<DispYYasBlanks>N</DispYYasBlanks>
						<NoAirpSort>Y</NoAirpSort>
						<HiLo>N</HiLo>
						<PRIInd>
							<xsl:choose>
								<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Private'">Y</xsl:when>
								<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Both'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</PRIInd>
						<FormulaDisp>N</FormulaDisp>
						<NonIntegratedDisp>
							<xsl:choose>
								<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Private'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</NonIntegratedDisp>
						<PrivDisp>N</PrivDisp>
						<HMCTInd>N</HMCTInd>
						<RemvInfants>N</RemvInfants>
						<HistFaresInd>N</HistFaresInd>
						<NoCompFares>Y</NoCompFares>
						<ExcTaxes>N</ExcTaxes>
						<Spare1>N</Spare1>
					</TariffQual>
				</QueryHeader>
				<TravConstraints>
					<UniqueKey>0000</UniqueKey>
					<StartPt><xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode"/></StartPt>
					<EndPt><xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/></EndPt>
					<xsl:choose>
						<xsl:when test="count(OriginDestinationInformation) = 1">
							<OW>Y</OW>
							<RT>N</RT>
						</xsl:when>
						<xsl:otherwise>
							<OW>N</OW>
							<RT>Y</RT>
						</xsl:otherwise>
					</xsl:choose>
					<LongDispInd>N</LongDispInd>
					<ValidatingDispInd>N</ValidatingDispInd>
					<NUCInd>N</NUCInd>
					<RetDataInd>N</RetDataInd>
					<BaseFares>Y</BaseFares>
					<ConxPts>N</ConxPts>
					<IncDomTax>N</IncDomTax>
					<ConvAP>Y</ConvAP>
					<FQSFareType>N</FQSFareType>
					<HalfRT>N</HalfRT>
					<CalShopReq />
					<CheckAltCitiesExist>N</CheckAltCitiesExist>
					<RetAltCityQuote>N</RetAltCityQuote>
					<StartDt><xsl:value-of select="translate(substring(OriginDestinationInformation[1]/DepartureDateTime,1,10),'-','')"/></StartDt>
					<xsl:if test="TravelPreferences/VendorPref[1]/@Code != ''">
						<AirV1><xsl:value-of select="TravelPreferences/VendorPref[1]/@Code"/></AirV1>
					</xsl:if>
					<xsl:if test="TravelPreferences/VendorPref[2]/@Code != ''">
						<AirV2><xsl:value-of select="TravelPreferences/VendorPref[2]/@Code"/></AirV2>
					</xsl:if>
					<xsl:if test="TravelPreferences/VendorPref[3]/@Code != ''">
						<AirV3><xsl:value-of select="TravelPreferences/VendorPref[3]/@Code"/></AirV3>
					</xsl:if>
					<GlobDir />
					<xsl:if test="OriginDestinationInformation[1]/ConnectionLocations/ConnectionLocation[1]/@LocationCode != ''">
						<ConxPt1><xsl:value-of select="OriginDestinationInformation[1]/ConnectionLocations/ConnectionLocation[1]/@LocationCode"/></ConxPt1>
					</xsl:if>
					<xsl:if test="OriginDestinationInformation[1]/ConnectionLocations/ConnectionLocation[1]/@LocationCode != ''">
						<ConxPt2><xsl:value-of select="OriginDestinationInformation[1]/ConnectionLocations/ConnectionLocation[2]/@LocationCode"/></ConxPt2>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="count(OriginDestinationInformation) = 1">
							<EndDt/>
						</xsl:when>
						<xsl:otherwise>
							<EndDt><xsl:value-of select="translate(substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,1,10),'-','')"/></EndDt>
						</xsl:otherwise>
					</xsl:choose>
					<TkDt></TkDt>
					<FareType>
						<xsl:choose>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin ='First'">F</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin ='Business'">C</xsl:when>
						</xsl:choose>
					</FareType>
					<Currency />
					<Pt />
					<SellCurrency />
					<JointFares>N</JointFares>
					<RndWorld>N</RndWorld>
					<CircTrip>N</CircTrip>
					<DoubleOneWay />
					<AltDatesReq />
					<Surcharges>N</Surcharges>
					<Spare1>N</Spare1>
					<SkipEffDtProcess />
					<CabinNum>
						<xsl:choose>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin ='First'">2</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin ='Business'">3</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</CabinNum>
					<EffStartDtFilter />
					<EffEndDtFilter />
				</TravConstraints>
				<xsl:for-each select="TravelerInfoSummary/PassengerTypeQuantity">
					<PsgrTypes>
						<PICReq>
							<xsl:choose>
								<xsl:when test="@Code = 'CHD'">CNN</xsl:when>
								<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
							</xsl:choose>
						</PICReq>
						<PICPsgrs><xsl:value-of select="@Quantity"/></PICPsgrs>
					</PsgrTypes> 
				</xsl:for-each>
				<PFMods>
					<UniqueKey>0000</UniqueKey>
					<PCC><xsl:value-of select="POS/Source/@PseudoCityCode"/></PCC>
					<Acct />
					<Contract />
				</PFMods>
			</FareDisplayMods>
		</FareQuoteMultiDisplay_8_2>
	</xsl:template>

</xsl:stylesheet>