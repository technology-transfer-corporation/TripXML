<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_TravelBuild1RQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 03 Aug 2005 - New file - Rastko											-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<DocProdFareManipulation_2_0>
			<ManualFareUpdateSaveMods>
				<xsl:copy-of select="TTReprice/PNRBFManagement_7_9/DocProdDisplayStoredQuote/FareNumInfo"/>
				<xsl:apply-templates select="TTReprice/OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler" />
				<xsl:copy-of select="TTReprice/PNRBFManagement_7_9/DocProdDisplayStoredQuote/AssocPsgrs"/>
				<xsl:copy-of select="TTReprice/PNRBFManagement_7_9/DocProdDisplayStoredQuote/AssocSegs"/>
				<xsl:copy-of select="TTReprice/PNRBFManagement_7_9/DocProdDisplayStoredQuote/PlatingAirVMod"/>
			</ManualFareUpdateSaveMods>
		</DocProdFareManipulation_2_0>
	</xsl:template>
	
	<xsl:template match="Traveler">
		<xsl:variable name="travelerpos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="rph"><xsl:value-of select="TravelerRefNumber/@RPH"/></xsl:variable>
		<xsl:variable name="paxtype"><xsl:value-of select="@PassengerTypeCode"/></xsl:variable>
		<xsl:variable name="uniquekey"><xsl:value-of select="../../../../PNRBFManagement_7_9/DocProdDisplayStoredQuote/AgntEnteredPsgrDescInfo[AgntEnteredPsgrDesc = $paxtype]/UniqueKey"/></xsl:variable>
		<xsl:variable name="pos">
			<xsl:for-each select="../../PriceData/FareDiscount">
				<xsl:if test="contains(@TravelerRefNumberRPHList,$rph)"><xsl:value-of select="position()"/></xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$pos != ''">
				<xsl:apply-templates select="../../../../PNRBFManagement_7_9/DocProdDisplayStoredQuote" mode="discount">
					<xsl:with-param name="uniquekey"><xsl:value-of select="$uniquekey"/></xsl:with-param>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="../../../../PNRBFManagement_7_9/DocProdDisplayStoredQuote" mode="nodiscount">
					<xsl:with-param name="uniquekey"><xsl:value-of select="$uniquekey"/></xsl:with-param>
					<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="DocProdDisplayStoredQuote" mode="discount">
		<xsl:param name="uniquekey"/>
		<xsl:param name="pos"/>
		<xsl:param name="travelerpos"/>
		<xsl:apply-templates select="GenQuoteDetails[UniqueKey = $uniquekey]" mode="discount">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="AgntEnteredPsgrDescInfo[UniqueKey = $uniquekey]">
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="FareConstruction[UniqueKey = $uniquekey]" mode="discount">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="uniquekey"><xsl:value-of select="$uniquekey"/></xsl:with-param>
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:for-each select="SegRelatedInfo[UniqueKey = $uniquekey]">
			<SegRelatedInfo>
				<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
				<QuoteNum>1</QuoteNum>
				<RelSegNum><xsl:value-of select="RelSegNum"/></RelSegNum>
				<NotValidBeforeDt><xsl:value-of select="NotValidBeforeDt"/></NotValidBeforeDt>
				<NotValidAfterDt><xsl:value-of select="NotValidAfterDt"/></NotValidAfterDt>
				<FIC><xsl:value-of select="FIC"/></FIC>
				<xsl:variable name="discountcode"><xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/@DiscountCode"/></xsl:variable>
				<xsl:variable name="fccode"><xsl:value-of select="concat(FIC,TkDesignator,' ')"/></xsl:variable>
				<TkDesignator>
					<xsl:choose>
						<xsl:when test="contains(../FareConstruction[UniqueKey = $uniquekey]/FareConstructText,$fccode)">
							<xsl:value-of select="TkDesignator"/>
							<xsl:value-of select="$discountcode"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="$discountcode"/></xsl:otherwise>
					</xsl:choose>
				</TkDesignator>
				<BagInfo><xsl:value-of select="BagInfo"/></BagInfo>
			</SegRelatedInfo>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="AgntEnteredPsgrDescInfo">
		<xsl:param name="travelerpos"/>
		<AgntEnteredPsgrDescInfo>
			<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
			<AgntEnteredPsgrDesc><xsl:value-of select="AgntEnteredPsgrDesc"/></AgntEnteredPsgrDesc>
			<QuotedPsgrDesc><xsl:value-of select="QuotedPsgrDesc"/></QuotedPsgrDesc>
			<PFCApplies><xsl:value-of select="PFCApplies"/></PFCApplies>
			<Spare1><xsl:value-of select="Spare1"/></Spare1>
			<ApplesToAry>
				<AppliesTo>
					<LNameNum>0</LNameNum>
					<FNameNum>0</FNameNum>
					<AbsNameNum><xsl:value-of select="$travelerpos"/></AbsNameNum>
				</AppliesTo>
			</ApplesToAry>
		</AgntEnteredPsgrDescInfo>
	</xsl:template>
	
	<xsl:template match="GenQuoteDetails" mode="discount">
		<xsl:param name="pos"/>
		<xsl:param name="travelerpos"/>
		<GenQuoteDetails>
			<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
			<QuoteNum><xsl:value-of select="QuoteNum"/></QuoteNum>
			<QuoteType><xsl:value-of select="QuoteType"/></QuoteType>
			<LastTkDt><xsl:value-of select="LastTkDt"/></LastTkDt>
			<QuoteDt><xsl:value-of select="QuoteDt"/></QuoteDt>
			<IntlSaleInd><xsl:value-of select="IntlSaleInd"/></IntlSaleInd>
			<BaseFareCurrency><xsl:value-of select="BaseFareCurrency"/></BaseFareCurrency>
			<BaseFareAmt><xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount"/></BaseFareAmt>
			<LowestOrNUCFare><xsl:value-of select="LowestOrNUCFare"/></LowestOrNUCFare>
			<BaseDecPos><xsl:value-of select="BaseDecPos"/></BaseDecPos>
			<EquivCurrency><xsl:value-of select="EquivCurrency"/></EquivCurrency>
			<EquivAmt><xsl:value-of select="EquivAmt"/></EquivAmt>
			<EquivDecPos><xsl:value-of select="EquivDecPos"/></EquivDecPos>
			<TotCurrency><xsl:value-of select="TotCurrency"/></TotCurrency>
			<TotAmt><xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/TotalFare/@Amount"/></TotAmt>
			<TotDecPos><xsl:value-of select="TotDecPos"/></TotDecPos>
			<ITNum><xsl:value-of select="ITNum"/></ITNum>
			<RteBasedQuote><xsl:value-of select="RteBasedQuote"/></RteBasedQuote>
			<M0><xsl:value-of select="M0"/></M0>
			<M5><xsl:value-of select="M5"/></M5>
			<M10><xsl:value-of select="M10"/></M10>
			<M15><xsl:value-of select="M15"/></M15>
			<M20><xsl:value-of select="M20"/></M20>
			<M25><xsl:value-of select="M25"/></M25>
			<Spare1><xsl:value-of select="Spare1"/></Spare1>
			<PrivFQd><xsl:value-of select="PrivFQd"/></PrivFQd>
			<PFOverrides><xsl:value-of select="PFOverrides"/></PFOverrides>
			<FlatFQd><xsl:value-of select="FlatFQd"/></FlatFQd>
			<DirMinApplied><xsl:value-of select="DirMinApplied"/></DirMinApplied>
			<VATIncInd><xsl:value-of select="VATIncInd"/></VATIncInd>
			<PenApplies><xsl:value-of select="PenApplies"/></PenApplies>
			<QuoteBasis><xsl:value-of select="QuoteBasis"/></QuoteBasis>
			<TaxDataAry>
				<xsl:for-each select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/Taxes/Tax">
					<TaxData>
						<Country><xsl:value-of select="@TaxCode"/></Country>
						<xsl:variable name="dec"><xsl:value-of select="@DecimalPlaces"/></xsl:variable>
						<xsl:variable name="zero">0000</xsl:variable>
						<xsl:variable name="amount">
							<xsl:choose>
								<xsl:when test="string-length(@Amount) > $dec">
									<xsl:value-of select="substring(@Amount,1,string-length(@Amount) - $dec)"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="substring(@Amount,(string-length(@Amount) - $dec) + 1,$dec)"/>
								</xsl:when>
								<xsl:when test="string-length(@Amount) = $dec">
									<xsl:text>0.</xsl:text>
									<xsl:value-of select="@Amount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>0.</xsl:text>
									<xsl:value-of select="substring($zero,1,$dec - string-length(@Amount))"/>
									<xsl:value-of select="@Amount"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Amt>
							<xsl:text disable-output-escaping="yes">&lt;![CDATA[  </xsl:text>
							<xsl:value-of disable-output-escaping="yes" select="$amount" />
							<xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
						</Amt>
					</TaxData>
				</xsl:for-each>
			</TaxDataAry>
		</GenQuoteDetails>
	</xsl:template>

	<xsl:template match="GenQuoteDetails" mode="nodiscount">
		<xsl:param name="pos"/>
		<xsl:param name="travelerpos"/>
		<GenQuoteDetails>
			<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
			<QuoteNum><xsl:value-of select="QuoteNum"/></QuoteNum>
			<QuoteType><xsl:value-of select="QuoteType"/></QuoteType>
			<LastTkDt><xsl:value-of select="LastTkDt"/></LastTkDt>
			<QuoteDt><xsl:value-of select="QuoteDt"/></QuoteDt>
			<IntlSaleInd><xsl:value-of select="IntlSaleInd"/></IntlSaleInd>
			<BaseFareCurrency><xsl:value-of select="BaseFareCurrency"/></BaseFareCurrency>
			<BaseFareAmt><xsl:value-of select="BaseFareAmt"/></BaseFareAmt>
			<LowestOrNUCFare><xsl:value-of select="LowestOrNUCFare"/></LowestOrNUCFare>
			<BaseDecPos><xsl:value-of select="BaseDecPos"/></BaseDecPos>
			<EquivCurrency><xsl:value-of select="EquivCurrency"/></EquivCurrency>
			<EquivAmt><xsl:value-of select="EquivAmt"/></EquivAmt>
			<EquivDecPos><xsl:value-of select="EquivDecPos"/></EquivDecPos>
			<TotCurrency><xsl:value-of select="TotCurrency"/></TotCurrency>
			<TotAmt><xsl:value-of select="TotAmt"/></TotAmt>
			<TotDecPos><xsl:value-of select="TotDecPos"/></TotDecPos>
			<ITNum><xsl:value-of select="ITNum"/></ITNum>
			<RteBasedQuote><xsl:value-of select="RteBasedQuote"/></RteBasedQuote>
			<M0><xsl:value-of select="M0"/></M0>
			<M5><xsl:value-of select="M5"/></M5>
			<M10><xsl:value-of select="M10"/></M10>
			<M15><xsl:value-of select="M15"/></M15>
			<M20><xsl:value-of select="M20"/></M20>
			<M25><xsl:value-of select="M25"/></M25>
			<Spare1><xsl:value-of select="Spare1"/></Spare1>
			<PrivFQd><xsl:value-of select="PrivFQd"/></PrivFQd>
			<PFOverrides><xsl:value-of select="PFOverrides"/></PFOverrides>
			<FlatFQd><xsl:value-of select="FlatFQd"/></FlatFQd>
			<DirMinApplied><xsl:value-of select="DirMinApplied"/></DirMinApplied>
			<VATIncInd><xsl:value-of select="VATIncInd"/></VATIncInd>
			<PenApplies><xsl:value-of select="PenApplies"/></PenApplies>
			<QuoteBasis><xsl:value-of select="QuoteBasis"/></QuoteBasis>
			<TaxDataAry>
				<xsl:for-each select="TaxDataAry/TaxData">
					<TaxData>
						<Country><xsl:value-of select="Country"/></Country>
						<Amt><xsl:value-of select="Amt"/></Amt>
					</TaxData>
				</xsl:for-each>
			</TaxDataAry>
		</GenQuoteDetails>
	</xsl:template>

	<xsl:template match="DocProdDisplayStoredQuote" mode="nodiscount">
		<xsl:param name="uniquekey"/>
		<xsl:param name="travelerpos"/>
		<xsl:apply-templates select="GenQuoteDetails[UniqueKey = $uniquekey]" mode="nodiscount">
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="AgntEnteredPsgrDescInfo[UniqueKey = $uniquekey]">
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="FareConstruction[UniqueKey = $uniquekey]" mode="nodiscount">
			<xsl:with-param name="travelerpos"><xsl:value-of select="$travelerpos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:for-each select="SegRelatedInfo[UniqueKey = $uniquekey]">
			<SegRelatedInfo>
				<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
				<QuoteNum>1</QuoteNum>
				<RelSegNum><xsl:value-of select="RelSegNum"/></RelSegNum>
				<NotValidBeforeDt><xsl:value-of select="NotValidBeforeDt"/></NotValidBeforeDt>
				<NotValidAfterDt><xsl:value-of select="NotValidAfterDt"/></NotValidAfterDt>
				<FIC><xsl:value-of select="FIC"/></FIC>
				<TkDesignator><xsl:value-of select="TkDesignator"/></TkDesignator>
				<BagInfo><xsl:value-of select="BagInfo"/></BagInfo>
			</SegRelatedInfo>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="FareConstruction" mode="discount">
		<xsl:param name="pos"/>
		<xsl:param name="uniquekey"/>
		<xsl:param name="travelerpos"/>
		<FareConstruction>
			<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
			<QuoteNum>1</QuoteNum>
			<xsl:variable name="dec"><xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@DecimalPlaces"/></xsl:variable>
			<xsl:variable name="newfare">
				<xsl:choose>
					<xsl:when test="string-length(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount) > $dec">
						<xsl:value-of select="substring(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount,1,string-length(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount) - 2)"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="substring(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount,string-length(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount) - 1,2)"/>
					</xsl:when>
					<xsl:when test="string-length(../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount) = $dec">
						<xsl:text>0.</xsl:text>
						<xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>0.0</xsl:text>
						<xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/BaseFare/@Amount"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="oldfare">
				<xsl:choose>
					<xsl:when test="string-length(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt) > 2">
						<xsl:value-of select="substring(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt,1,string-length(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt) - 2)"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="substring(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt,string-length(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt) - 1,2)"/>
					</xsl:when>
					<xsl:when test="string-length(../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt) = 2">
						<xsl:text>0.</xsl:text>
						<xsl:value-of select="../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>0.0</xsl:text>
						<xsl:value-of select="../GenQuoteDetails[UniqueKey = $uniquekey]/BaseFareAmt"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<FareConstructText>
				<xsl:call-template name="farec">
					<xsl:with-param name="fc"><xsl:value-of select="FareConstructText"/></xsl:with-param>
					<xsl:with-param name="newfare"><xsl:value-of select="$newfare"/></xsl:with-param>
					<xsl:with-param name="oldfare"><xsl:value-of select="$oldfare"/></xsl:with-param>
					<xsl:with-param name="numSR"><xsl:value-of select="count(../SegRelatedInfo[UniqueKey = $uniquekey])"/></xsl:with-param>
					<xsl:with-param name="index">1</xsl:with-param>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					<xsl:with-param name="uniquekey"><xsl:value-of select="$uniquekey"/></xsl:with-param>
				</xsl:call-template>
			</FareConstructText>
		</FareConstruction>
	</xsl:template>
	
	<xsl:template name="farec">
		<xsl:param name="fc"/>
		<xsl:param name="newfare"/>
		<xsl:param name="oldfare"/>
		<xsl:param name="numSR"/>
		<xsl:param name="index"/>
		<xsl:param name="pos"/>
		<xsl:param name="uniquekey"/>
		<xsl:if test="$numSR > 0">
			<xsl:variable name="fccode"><xsl:value-of select="concat(../SegRelatedInfo[UniqueKey = $uniquekey][position()=$index]/FIC,../SegRelatedInfo[UniqueKey = $uniquekey][position()=$index]/TkDesignator,' ')"/></xsl:variable>
			<xsl:variable name="fic">
				<xsl:choose>
					<xsl:when test="contains($fc,$fccode)"><xsl:value-of select="$fccode"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="concat(../SegRelatedInfo[UniqueKey = $uniquekey][position()=$index]/FIC,' ')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="discountcode"><xsl:value-of select="../../../OTA_TravelItineraryRQ/TPA_Extensions/PriceData/FareDiscount[position()=$pos]/@DiscountCode"/></xsl:variable>
			<xsl:variable name="newfc">
				<xsl:value-of select="substring-before($fc,$fic)"/>
				<xsl:value-of select="translate($fic,' ','')"/>
				<xsl:value-of select="concat($discountcode,' ')"/>
				<xsl:value-of select="substring-after($fc,$fic)"/>
			</xsl:variable>
			<xsl:variable name="fcfare1">
				<xsl:value-of select="substring-before($newfc,$oldfare)"/>
				<xsl:value-of select="$newfare"/>
				<xsl:value-of select="substring-after($newfc,$oldfare)"/>
			</xsl:variable>
			<xsl:variable name="fcfare2">
				<xsl:value-of select="substring-before($fcfare1,$oldfare)"/>
				<xsl:value-of select="$newfare"/>
				<xsl:value-of select="substring-after($fcfare1,$oldfare)"/>
			</xsl:variable>
			<xsl:call-template name="farec">
				<xsl:with-param name="fc"><xsl:value-of select="$fcfare2"/></xsl:with-param>
				<xsl:with-param name="newfare"><xsl:value-of select="$newfare"/></xsl:with-param>
				<xsl:with-param name="oldfare"><xsl:value-of select="$oldfare"/></xsl:with-param>
				<xsl:with-param name="numSR"><xsl:value-of select="$numSR - 1"/></xsl:with-param>
				<xsl:with-param name="index"><xsl:value-of select="$index + 1"/></xsl:with-param>
				<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
				<xsl:with-param name="uniquekey"><xsl:value-of select="$uniquekey"/></xsl:with-param>
			</xsl:call-template>
			<xsl:if test="$numSR = 1">
				<xsl:value-of select="$fcfare2"/>
				<xsl:choose>
					<xsl:when test="contains($fcfare2,'ROE1')"></xsl:when>
					<xsl:otherwise>
						<xsl:text> ROE1</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="FareConstruction" mode="nodiscount">
		<xsl:param name="travelerpos"/>
		<FareConstruction>
			<UniqueKey>000<xsl:value-of select="$travelerpos"/></UniqueKey>
			<QuoteNum>1</QuoteNum>
			<FareConstructText>
				<xsl:value-of select="FareConstructText"/>
				<xsl:choose>
					<xsl:when test="contains(FareConstructText,'ROE1')"></xsl:when>
					<xsl:otherwise>
						<xsl:text> ROE1</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</FareConstructText>
		</FareConstruction>
	</xsl:template>
		
</xsl:stylesheet>