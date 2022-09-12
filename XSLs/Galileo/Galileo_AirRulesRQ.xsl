<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_AirRulesRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 04 Nov 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<AirRules>
			<Session>
				<xsl:apply-templates select="OTA_AirRulesRQ/POS" mode="Open" />
			</Session>
			<xsl:apply-templates select="OTA_AirRulesRQ" />
			<EndSession>
				<xsl:apply-templates select="OTA_AirRulesRQ/POS" mode="Close" />
			</EndSession>
		</AirRules>
	</xsl:template>
	<xsl:template match="OTA_AirRulesRQ">
		<FareQuoteTariffDisplay_8_0>
			<FareDisplayMods>
				<QueryHeader>
					<UniqueKey>0000</UniqueKey>
					<LangNum>00</LangNum>
					<Action>002</Action>
					<RetCRTOutput>N</RetCRTOutput>
					<NoMsg>N</NoMsg>
					<NoTrunc>N</NoTrunc>
					<IMInd>N</IMInd>
					<FIPlus>N</FIPlus>
					<PEInd>N</PEInd>
					<HostUse16>N</HostUse16>
					<NBInd>N</NBInd>
					<ActionOnlyInd>N</ActionOnlyInd>
					<TranslatePeriod>N</TranslatePeriod>
					<GFYInd>N</GFYInd>
					<IntFrame1>N</IntFrame1>
					<SmartParsed>Y</SmartParsed>
					<PDCodes>N</PDCodes>
					<BkDtOverride>N</BkDtOverride>
					<HostUse25>N</HostUse25>
					<DefCurrency>N</DefCurrency>
					<PFPWInd>N</PFPWInd>
					<HostUse28>N</HostUse28>
					<HostUse29>N</HostUse29>
					<HostUse30>N</HostUse30>
					<HostUse31>N</HostUse31>
					<DefCurrencyLocInd>N</DefCurrencyLocInd>
					<HostUse33>N</HostUse33>
				</QueryHeader>
				<!--can have up to 7 blanks to fill in the FareReference field below -->
				<xsl:variable name="BlanksForFareRef">
					<xsl:value-of select="string('       ')" />
				</xsl:variable>
				<TravConstraints>
					<xsl:variable name="DepDate">
						<xsl:value-of select="substring-before(RuleReqInfo/DepartureDate,'T')" />
					</xsl:variable>
					<UniqueKey>0000</UniqueKey>
					<!--Left justified, blank filled. -->
					<StartPt>
						<xsl:value-of select="RuleReqInfo/DepartureAirport/@LocationCode" />
						<xsl:value-of select="string('  ')" />
					</StartPt>
					<EndPt>
						<xsl:value-of select="RuleReqInfo/ArrivalAirport/@LocationCode" />
						<xsl:value-of select="string('  ')" />
					</EndPt>
					<!--Don't know if I need OW and RT due to fact that OTA only allows one RuleReqInfo -->
					<!-- One way fares request Y or N N if does not exist (GQ2DI1) -->
					<OW>N</OW>
					<RT>N</RT>
					<!-- Long display request Y or N N if does not exist (GQ2DI1) -->
					<LongDispInd>N</LongDispInd>
					<!-- Validating display request Y or N N if does not exist (GQ2DI1) -->
					<ValidatingDispInd>N</ValidatingDispInd>
					<!-- NUC values request Y or N N if does not exist (GQ2DI1) -->
					<NUCInd>N</NUCInd>
					<!-- Return route and surcharge data with smart parsed fare display only Y or N N if does not exist (GQ2DI1) -->
					<RetDataInd>N</RetDataInd>
					<!-- With rules request Y or N N if does not exist (GQ2DI1) -->
					<RulesInd>N</RulesInd>
					<!-- Base fares request	Y or N N if does not exist (GQ2DI1) -->
					<BaseFares>N</BaseFares>
					<!-- Connection cities display request Y or N N if does not exist (GQ2DI2) -->
					<ConxPts>N	</ConxPts>
					<!-- Include domestic tax request  Y or N N if does not exist (GQ2DI2) -->
					<IncDomTax>N</IncDomTax>
					<!-- Convert advance purchase column Y or N N if does not exist (GQ2DI2) -->
					<ConvAP>N	</ConvAP>
					<!-- (FQS only) (Fare type/class identifier 8-byte field below)Y or N N if does not exist (GQ2DI2) -->
					<FQSFareType>N </FQSFareType>
					<!-- Half round trip NUC modifier 	Y or N N if does not exist (GQ2DI2) -->
					<HalfRT>N</HalfRT>
					<!-- Fare Display has been requested for Calendar Shop Y or N N if does not exist (GQ2DI2) -->
					<CalShopReq></CalShopReq>
					<!-- Reserved for future use 	NN if does not exist (GQ2DI2) -->
					<Spare1>NN</Spare1>
					<StartDt>
						<xsl:value-of select="translate(string($DepDate),'-','')" />
					</StartDt>
					<!-- Left justified, blank filled -->
					<AirV1>
						<xsl:value-of select="RuleReqInfo/FilingAirline/@Code" />
						<xsl:value-of select="string(' ')" />
					</AirV1>
					<AirV2></AirV2>
					<AirV3></AirV3>
					<GlobDir></GlobDir>
					<ConxPt1></ConxPt1>
					<ConxPt2></ConxPt2>
					<EndDt>00000000</EndDt>
					<!-- YYYYMMDD (GQ2DTT) 	Zero if does not exist -->
					<TkDt>00000000</TkDt>
					<!-- Fare type/Class identifier (GQ2FCL) 	
					FQS	-	Fare basis code or booking code IFQ	- P, F, J, C, Y, EURO, PEX, SPEX, APEX, EXCN, ALL, IATA, APEXS, PEXS, SPCLS, NORMS, RTW, 
					and Fare basis codes are all valid. Left-justified, blank filled. -->
					<FareType>
						<xsl:value-of select="RuleReqInfo/FareReference" />
						<xsl:value-of select="substring($BlanksForFareRef,1,8-string-length(RuleReqInfo/FareReference))" />
					</FareType>
					<Currency>
						<xsl:choose>
							<xsl:when test="POS/Source/@ISOCurrency !=''">
								<xsl:value-of select="POS/Source/@ISOCurrency" />
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</Currency>
					<Pt></Pt>
					<SellCurrency></SellCurrency>
					<JointFares>N</JointFares>
					<!-- Round the World fares Y or N N if does not exist (GQ2FTY) -->
					<RndWorld>N</RndWorld>
					<!-- Circle trip/Triangle fares 	Y or N N if does not exist (GQ2FTY) -->
					<CircTrip>N</CircTrip>
					<!-- Reserved for future use 	NNNNN if does not exist (GQ2FTY) -->
					<Spare2>NNNNN</Spare2>
				</TravConstraints>
				<PsgrTypes>
					<UniqueKey>0001</UniqueKey>
					<PICReq>ADT</PICReq>
					<PICPsgrs>01</PICPsgrs>
					<PsgrNum>01</PsgrNum>
				</PsgrTypes>
			</FareDisplayMods>  
		</FareQuoteTariffDisplay_8_0>
		<FareQuoteRulesDisplay_8_0>
			<FareDisplayMods>
				<QueryHeader>
					<UniqueKey>0000</UniqueKey>
					<LangNum>00</LangNum>
					<Action>023</Action>
					<RetCRTOutput>N</RetCRTOutput>
					<NoMsg>N</NoMsg>
					<NoTrunc>N</NoTrunc>
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
					<DefCurrency />
					<PFPWInd>N</PFPWInd>
					<HostUse28>N</HostUse28>
					<HostUse29>N</HostUse29>
					<HostUse30>N</HostUse30>
					<HostUse31>N</HostUse31>
					<DefCurrencyLocInd />
					<HostUse33 />
					<Spare14>NNNNNNNNNNNNNNNN</Spare14>
				</QueryHeader>
				<RulesDisplay>
					<UniqueKey>0000</UniqueKey>
					<LineNum>001</LineNum>
					<Spare1>N</Spare1>
					<AllParagraphsInd>Y</AllParagraphsInd>
					<SumRuleDispInd>N</SumRuleDispInd>
					<FullTextInd>N</FullTextInd>
					<Spare2>NNNN</Spare2>
					<AirV1></AirV1>
					<AirV2 />
					<RuleParagraphNums />
				</RulesDisplay>
			</FareDisplayMods>
		</FareQuoteRulesDisplay_8_0>
	</xsl:template>
	<xsl:template match="POS" mode="Open">
		<SessionCreateRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
		</SessionCreateRQ>
	</xsl:template>
	<xsl:template match="POS" mode="Close">
		<SessionCloseRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
		</SessionCloseRQ>
	</xsl:template>
</xsl:stylesheet>