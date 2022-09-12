<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_LowFareRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 07 Mar 2012 - Rastko - mapped CNN to CHD								-->
	<!-- Date: 07 Aug - Rastko - Added mapping for NegotiatedFareCode				-->
	<!-- Date: 26 Mar 2007 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="zeros">000</xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="FareQuoteSuperBB_21" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="FareQuoteSuperBB_21">
		<OTA_AirLowFareSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Galileo</xsl:attribute>
			<xsl:choose>
				<xsl:when test="FareInfo/RespHeader/ErrMsg='Y' or ErrText/Err!='' or FareInfo/ErrorCode != ''">
					<Errors>
						<xsl:apply-templates select="//ErrText" />
						<xsl:apply-templates select="FareInfo/InfoMsg" mode="warning" />
					</Errors>
				</xsl:when>
				<xsl:when test="TransactionErrorCode">
					<Errors>
						<xsl:apply-templates select="TransactionErrorCode" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="FareInfo" mode="PricedItin" />
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="FareInfo" mode="PricedItin">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<AirItinerary>
				<xsl:attribute name="DirectionInd">
					<xsl:choose>
						<xsl:when test="ItinSeg[1]/StartPt  = ItinSeg[position()=last()]/EndPt">Circle</xsl:when>
						<xsl:otherwise>OneWay</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<OriginDestinationOptions>
					<xsl:variable name="nip"><xsl:value-of select="sum(PsgrTypes[PICReq != 'INF']/PICPsgrs)"/></xsl:variable>
					<xsl:apply-templates select="FlightItemCrossRef[not(ODNum = preceding-sibling::FlightItemCrossRef/ODNum)]" mode="first">
						<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
					</xsl:apply-templates>
				</OriginDestinationOptions>
			</AirItinerary>
			<xsl:apply-templates select="." mode="totalprice" />
		</PricedItinerary>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="ErrText">
		<Error Type="Fare">
			<xsl:choose>
				<xsl:when test="Err='F 000001' and KlrInErr='15DE' and Text='SYSTEM ERROR'">
					<xsl:attribute name="Code">
						<xsl:text>XXERR</xsl:text>
					</xsl:attribute>
					<xsl:text>NO NEGO/PUBLISHED FARES</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Code">
						<xsl:value-of select="Err" />
					</xsl:attribute>
					<xsl:value-of select="Text" />
				</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="InfoMsg" mode="warning">
		<Error Type="Fare">
			<xsl:attribute name="Code">
				<xsl:value-of select="AppNum" />
				<xsl:value-of select="MsgType" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="TransactionErrorCode">
		<Error Type="General">
			<xsl:attribute name="Code">
				<xsl:value-of select="Domain" />
				<xsl:value-of select="Code" />
			</xsl:attribute>
			<xsl:text>- SYSTEM ERROR</xsl:text>
		</Error>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Total Fare and PTC_FareBreakdown 				    -->
	<!-- ************************************************************** -->
	<xsl:template match="FareInfo" mode="totalprice">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<AirItineraryPricingInfo>
			<xsl:choose>
				<xsl:when test="GenQuoteDetails[1]/PrivFQd='Y'">
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<ItinTotalFare>
				<xsl:if test="../OTA_AirLowFareSearchRQ/TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode!=''">
					<xsl:attribute name="NegotiatedFareCode">
						<xsl:value-of select="../OTA_AirLowFareSearchRQ/TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:variable name="amttota">
					<xsl:apply-templates select="GenQuoteDetails[1]" mode="basefare">
						<xsl:with-param name="total">0</xsl:with-param>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="amttot"><xsl:value-of select="substring-before($amttota,'/')" /></xsl:variable>
				<xsl:variable name="amttot1a">
					<xsl:apply-templates select="GenQuoteDetails[1]" mode="totalfare">
						<xsl:with-param name="total">0</xsl:with-param>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="amttot1"><xsl:value-of select="substring-before($amttot1a,'/')" /></xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amttot" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount"><xsl:value-of select="$amttot1 - $amttot"/></xsl:attribute>
						<!--xsl:apply-templates select="GenQuoteDetails[1]" mode="TotalTax" /-->
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amttot1" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="GenQuoteDetails[1]/TotCurrency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="GenQuoteDetails[1]/TotDecPos" />
					</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="GenQuoteDetails" mode="travelergroup" />
			</PTC_FareBreakdowns>
			<FareInfos>
				<xsl:apply-templates select="SegRelatedInfo" />
			</FareInfos>
		</AirItineraryPricingInfo>
		<xsl:if test="GenQuoteDetails[1]/LastTkDt != ''">
			<TicketingInfo>
				<xsl:if test="GenQuoteDetails[1]/LastTkDt != ''">
					<xsl:attribute name="TicketTimeLimit">
						<xsl:value-of select="substring(GenQuoteDetails[1]/LastTkDt,1,4)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(GenQuoteDetails[1]/LastTkDt,5,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(GenQuoteDetails[1]/LastTkDt,7,2)"/>
						<xsl:text>T00:00:00</xsl:text>
					</xsl:attribute>
				</xsl:if>
			</TicketingInfo>
		</xsl:if>
	</xsl:template>
	<!-- ******************************************************************** -->
	<xsl:template match="GenQuoteDetails" mode="basefare">
		<xsl:param name="total" />
		<xsl:variable name="uk">
			<xsl:value-of select="format-number(UniqueKey,'0000')" />
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:choose>
				<xsl:when test="EquivAmt != '0'">
					<xsl:value-of select="EquivAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="BaseFareAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="basefare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>
	
	<xsl:template match="GenQuoteDetails" mode="basefare2">
		<xsl:param name="total" />
		<xsl:variable name="uk">
			<xsl:value-of select="format-number(UniqueKey,'0000')" />
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:value-of select="EquivAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="basefare2">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>
	<xsl:template match="GenQuoteDetails" mode="totalfare">
		<xsl:param name="total" />
		<xsl:variable name="uk">
			<xsl:value-of select="format-number(UniqueKey,'0000')" />
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:value-of select="TotAmt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="totalfare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>
	<xsl:template match="GenQuoteDetails" mode="toteachtax">
		<xsl:param name="total" />
		<xsl:param name="taxcode" />
		<xsl:variable name="uk">
			<xsl:value-of select="format-number(UniqueKey,'0000')" />
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:value-of select="TaxDataAry/TaxData[Country=$taxcode]/Amt * ../PsgrTypes[UniqueKey=$uk]/PICPsgrs" />
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:choose>
				<xsl:when test="$thistotal = 'NaN'">
					<xsl:value-of select="$total" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$total + $thistotal" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::GenQuoteDetails[1]" mode="toteachtax">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
			<xsl:with-param name="taxcode">
				<xsl:value-of select="$taxcode" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>
	<xsl:template match="GenQuoteDetails" mode="TotalTax">
		<xsl:for-each select="TaxDataAry/TaxData">
			<xsl:variable name="taxcode" select="Country" />
			<xsl:variable name="addzeros">00000</xsl:variable>
			<xsl:variable name="numberdecimals" select="../../TotDecPos" />
			<xsl:variable name="amttot">
				<xsl:apply-templates select="../../../GenQuoteDetails[1]" mode="toteachtax">
					<xsl:with-param name="total">0</xsl:with-param>
					<xsl:with-param name="taxcode">
						<xsl:value-of select="$taxcode" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="SumOfTax">
				<xsl:value-of select="substring-before($amttot,'/')" />
			</xsl:variable>
			<xsl:variable name="SumOfTaxWithoutDec">
				<xsl:value-of select="translate(string($SumOfTax),'. ','')" />
			</xsl:variable>
			<xsl:variable name="Length">
				<xsl:value-of select="string-length($SumOfTaxWithoutDec)" />
			</xsl:variable>
			<xsl:variable name="subtract">1<xsl:value-of select="substring($addzeros,1,$numberdecimals)" /></xsl:variable>
			<xsl:variable name="SumOfTaxTwo">
				<xsl:value-of select="$SumOfTax * $subtract" />
			</xsl:variable>
			<xsl:variable name="NoDecimals">
				<xsl:value-of select="substring-before(string($SumOfTaxTwo),'.')" />
			</xsl:variable>
			<Tax>
				<xsl:attribute name="TaxCode">
					<xsl:value-of select="Country" />
				</xsl:attribute>
				<xsl:attribute name="Amount">
					<xsl:choose>
						<xsl:when test="contains($SumOfTaxTwo, '.')">
							<xsl:value-of select="$NoDecimals" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$SumOfTaxTwo" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</Tax>
		</xsl:for-each>
	</xsl:template>
	<!-- ******************************************************************** -->
	<!--  OriginDestinationOption section						    -->
	<!-- ******************************************************************** -->
	<xsl:template match="FlightItemCrossRef" mode="first">
		<xsl:param name="nip"/>
		<xsl:variable name="ODNumber" select="ODNum" />
		<xsl:variable name="FirstFlight" select="FltItemAry/FltItem[1]/IndexNum" />
		<OriginDestinationOption>
			<xsl:apply-templates select="FltItemAry/FltItem[1]" mode="firstsegment">
				<xsl:with-param name="rph">1</xsl:with-param>
				<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
			</xsl:apply-templates>
			<xsl:apply-templates select="following-sibling::FlightItemCrossRef[ODNum=current()/ODNum]" mode="next">
				<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
			</xsl:apply-templates>
		</OriginDestinationOption>
	</xsl:template>
	<xsl:template match="FlightItemCrossRef" mode="next">
		<xsl:param name="nip"/>
		<xsl:apply-templates select="FltItemAry/FltItem[1]" mode="firstsegment">
			<xsl:with-param name="rph">1</xsl:with-param>
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<!-- ********************************************************** -->
	<!-- create flight for this OandD 			-->
	<!-- ********************************************************** -->
	<xsl:template match="FltItem" mode="firstsegment">
		<xsl:param name="rph" />
		<xsl:param name="nip"/>
		<xsl:variable name="Index" select="IndexNum" />
		<xsl:variable name="ODNum" select="../../ODNum" />
		<xsl:variable name="NumLegs" select="../../ODNumLegs" />
		<xsl:variable name="cl" select="translate(../../FltItemAry,'0123456789','')" />
		<xsl:apply-templates select="../../../../AirAvail[position()=$ODNum]/AvailFlt[position()=$Index]">
			<xsl:with-param name="class">
				<xsl:value-of select="$cl" />
			</xsl:with-param>
			<xsl:with-param name="pos">
				<xsl:value-of select="$rph" />
			</xsl:with-param>
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="following-sibling::FltItem[position()=$NumLegs]" mode="firstsegment">
			<xsl:with-param name="rph">
				<xsl:value-of select="$rph + 1" />
			</xsl:with-param>
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Fare Breakdown per pax type					    -->
	<!-- ************************************************************** -->
	<xsl:template match="GenQuoteDetails" mode="travelergroup">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<xsl:variable name="paxno">
			<xsl:value-of select="format-number(UniqueKey,'0000')" />
		</xsl:variable>
		<xsl:variable name="paxno1">
			<xsl:value-of select="UniqueKey" />
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/PICPsgrs" />
		</xsl:variable>
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:variable name="PsgrType">
						<xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/PICReq" />
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$PsgrType = 'AA'">ADT</xsl:when>
						<xsl:when test="$PsgrType = 'AD'">ADT</xsl:when>
						<xsl:when test="$PsgrType = 'C10'">CHD</xsl:when>
						<xsl:when test="$PsgrType = 'CNN'">CHD</xsl:when>
						<xsl:when test="$PsgrType = 'CH'">CHD</xsl:when>
						<xsl:when test="$PsgrType = 'C'">CHD</xsl:when>
						<xsl:when test="$PsgrType = 'IN'">INF</xsl:when>
						<xsl:when test="$PsgrType = 'INF'">INF</xsl:when>
						<xsl:when test="$PsgrType = 'GOV'">GOV</xsl:when>
						<xsl:when test="$PsgrType = 'MIL'">MIL</xsl:when>
						<xsl:when test="$PsgrType = 'CD'">SRC</xsl:when>
						<xsl:when test="$PsgrType = 'SC'">SRC</xsl:when>
						<xsl:when test="$PsgrType = 'STU'">STD</xsl:when>
						<xsl:when test="$PsgrType = 'YC'">YTH</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PsgrType" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="$nip" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:for-each select="../SegRelatedInfo[UniqueKey=$paxno1]">
					<FareBasisCode>
						<xsl:value-of select="FIC" />
					</FareBasisCode>
				</xsl:for-each>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:if test="PrivFQd='Y'">
					<xsl:attribute name="NegotiatedFare">1</xsl:attribute>
				</xsl:if>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="TotCurrency = BaseFareCurrency">
								<xsl:variable name="bfa"><xsl:value-of select="BaseFareAmt" /></xsl:variable>
								<xsl:value-of select="$bfa * $nip" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="ea"><xsl:value-of select="EquivAmt" /></xsl:variable>
								<xsl:value-of select="$ea * $nip" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:variable name="tta">
							<xsl:choose>
								<xsl:when test="EquivAmt != '0'">
									<xsl:value-of select="TotAmt - EquivAmt" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="TotAmt - BaseFareAmt" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:attribute name="Amount"><xsl:value-of select="$tta * $nip"/></xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:variable name="ta"><xsl:value-of select="TotAmt" /></xsl:variable>
						<xsl:value-of select="$ta * $nip" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<PricedCode>
					<xsl:variable name="PsgrTypeResp">
						<xsl:value-of select="../PsgrTypes[UniqueKey=$paxno]/RespPIC" />
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$PsgrTypeResp = 'AA'">ADT</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'AD'">ADT</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'C10'">CHD</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'CHD'">CHD</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'CH'">CHD</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'C'">CHD</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'IN'">INF</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'INF'">INF</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'GOV'">GOV</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'MIL'">MIL</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'CD'">SRC</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'SC'">SRC</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'STU'">STD</xsl:when>
						<xsl:when test="$PsgrTypeResp = 'YC'">YTH</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PsgrTypeResp" />
						</xsl:otherwise>
					</xsl:choose>
				</PricedCode>
				<xsl:if test="../PsgrTypes[UniqueKey=$paxno]/ReqReturnedPIC = 'N'">
					<Text>NOT FARED AT PASSENGER TYPE REQUESTED *5*</Text>
				</xsl:if>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Build Tax Data Group					 		   -->
	<!-- ************************************************************** -->
	<xsl:template match="TaxData" mode="tax">
		<xsl:variable name="tax" select="translate(string(Amt),'. ','')" />
		<Tax>
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="Country" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:choose>
					<xsl:when test="starts-with($tax,'00000')">
						<xsl:value-of select="substring($tax, 6)" />
					</xsl:when>
					<xsl:when test="starts-with($tax,'0000')">
						<xsl:value-of select="substring($tax, 5)" />
					</xsl:when>
					<xsl:when test="starts-with($tax,'000')">
						<xsl:value-of select="substring($tax, 4)" />
					</xsl:when>
					<xsl:when test="starts-with($tax,'00')">
						<xsl:value-of select="substring($tax, 3)" />
					</xsl:when>
					<xsl:when test="starts-with($tax,'0')">
						<xsl:value-of select="substring($tax, 2)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$tax" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<!--xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../TotCurrency"/>
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="../../TotDecPos"/>
			</xsl:attribute-->
		</Tax>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--Fare Rule Info section							    -->
	<!-- ************************************************************** -->
	<xsl:template match="SegRelatedInfo">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable >
		<FareInfo>
			<xsl:variable name="segnum" select="RelSegNum" />
			<xsl:variable name="StartTime">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(../ItinSeg[position()=$segnum]/StartTm))" />
				<xsl:value-of select="../ItinSeg[position()=$segnum]/StartTm" />
			</xsl:variable>
			<xsl:variable name="EndTime">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(../ItinSeg[position()=$segnum]/EndTm))" />
				<xsl:value-of select="../ItinSeg[position()=$segnum]/EndTm" />
			</xsl:variable>
			<DepartureDate>
				<xsl:value-of select="substring(string(../ItinSeg[position()=$segnum]/StartDt),1,4)" />-<xsl:value-of select="substring(string(../ItinSeg[position()=$segnum]/StartDt),5,2)" />-<xsl:value-of select="substring(string(../ItinSeg[position()=$segnum]/StartDt),7,2)" />T<xsl:value-of select="substring(string($StartTime),1,2)" />:<xsl:value-of select="substring(string($StartTime),3,2)" /><xsl:text>:00</xsl:text>
			</DepartureDate>
			<FareReference>
				<xsl:value-of select="FIC" />
			</FareReference>
			<RuleInfo>
				<xsl:choose>
					<xsl:when test="count(../RsvnRules) != '1'">
						<!--xsl:apply-templates select="../RsvnRules[position()=$pos]/PenFeeAry/PenFee[1]" mode="pen"/-->
						<xsl:apply-templates select="../RsvnRules[position()=$pos]" mode="advp"/>
			    			<!--xsl:apply-templates select="../RsvnRules[position()=$pos]" mode="minmax"/-->
					</xsl:when>
					<xsl:otherwise>
						<!--xsl:apply-templates select="../RsvnRules/PenFeeAry/PenFee[1]" mode="pen"/-->
						<xsl:apply-templates select="../RsvnRules" mode="advp"/>
			    			<!--xsl:apply-templates select="../RsvnRules" mode="minmax"/-->
					</xsl:otherwise>
			    	</xsl:choose>
				 <xsl:choose>
					<xsl:when test="../InfoMsg[contains(Text,'CHANGE')]">
						<ChargesRules>
					 		<VoluntaryChanges>
							  	<Penalty>
							  		<xsl:attribute name="PenaltyType"><xsl:value-of select="../InfoMsg[contains(Text,'CHANGE')]/Text"/></xsl:attribute>
							  	</Penalty>
							</VoluntaryChanges>
						</ChargesRules>
					</xsl:when>
					<xsl:when test="../InfoMsg[contains(Text,'REFUND')]">
						<ChargesRules>
					 		<VoluntaryChanges>
							  	<Penalty>
							  		<xsl:attribute name="PenaltyType"><xsl:value-of select="../InfoMsg[contains(Text,'REFUND')]/Text"/></xsl:attribute>
							  	</Penalty>
							</VoluntaryChanges>
						</ChargesRules>
					</xsl:when>
					<xsl:when test="../InfoMsg[contains(Text,'CHG')]">
						<ChargesRules>
					 		<VoluntaryChanges>
							  	<Penalty>
							  		<xsl:attribute name="PenaltyType"><xsl:value-of select="../InfoMsg[contains(Text,'CHG')]/Text"/></xsl:attribute>
							  	</Penalty>
							</VoluntaryChanges>
						</ChargesRules>
					</xsl:when>
				</xsl:choose>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="../ItinSeg[position()=$segnum]/AirV" />
				</xsl:attribute>
			</FilingAirline>
			<xsl:if test="../ItinSeg[position()=$segnum]/OpAirV != ' '">
				<MarketingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="../ItinSeg[position()=$segnum]/OpAirV" />
					</xsl:attribute>
				</MarketingAirline>
			</xsl:if>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../ItinSeg[position()=$segnum]/StartPt" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../ItinSeg[position()=$segnum]/EndPt" />
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--  		          Segment Data                           		    -->
	<!-- *************************************************************** -->
	<xsl:template match="AvailFlt">
		<xsl:param name="class" />
		<xsl:param name="pos" />
		<xsl:param name="nip"/>
		<xsl:variable name="cl">
			<xsl:value-of select="$class" />
		</xsl:variable>
		<xsl:variable name="ps">
			<xsl:value-of select="$pos" />
		</xsl:variable>
		<xsl:apply-templates select="." mode="Segment">
			<xsl:with-param name="class">
				<xsl:value-of select="$cl" />
			</xsl:with-param>
			<xsl:with-param name="pos">
				<xsl:value-of select="$ps" />
			</xsl:with-param>
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
			<xsl:with-param name="classpos">1</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="AvailFlt" mode="Segment">
		<xsl:param name="class" />
		<xsl:param name="pos" />
		<xsl:param name="nip"/>
		<xsl:param name="classpos" />
		<FlightSegment>
			<xsl:variable name="DepTime">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(StartTm))" />
				<xsl:value-of select="StartTm" />
			</xsl:variable>
			<xsl:variable name="ArrTime">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(EndTm))" />
				<xsl:value-of select="EndTm" />
			</xsl:variable>
			<xsl:attribute name="DepartureDateTime"><xsl:value-of select="substring(StartDt,1,4)" />-<xsl:value-of select="substring(StartDt,5,2)" />-<xsl:value-of select="substring(StartDt,7,2)" />T<xsl:value-of select="substring($DepTime,1,2)" />:<xsl:value-of select="substring($DepTime,3,2)" />:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime"><xsl:value-of select="substring(EndDt,1,4)" />-<xsl:value-of select="substring(EndDt,5,2)" />-<xsl:value-of select="substring(EndDt,7,2)" />T<xsl:value-of select="substring($ArrTime,1,2)" />:<xsl:value-of select="substring($ArrTime,3,2)" />:00</xsl:attribute>
			<xsl:attribute name="StopQuantity">
				<xsl:value-of select="NumStops" />
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(FltNum))" />
				<xsl:value-of select="FltNum" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="substring($class,$classpos,1)" />
			</xsl:attribute>
			<xsl:attribute name="NumberInParty">
				<xsl:value-of select="$nip"/>
			</xsl:attribute>
			<xsl:attribute name="E_TicketEligibility">
				<xsl:choose>
					<xsl:when test="ETktEligibility='E'">Eligible</xsl:when>
					<xsl:otherwise>NotEligible</xsl:otherwise>
				</xsl:choose>
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
			<xsl:choose>
				<xsl:when test="OpAirVInd = 'Y'">
					<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="OpAirV"/></xsl:attribute>
					</OperatingAirline>
				</xsl:when>
				<xsl:otherwise>
					<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="AirV"/></xsl:attribute>
					</OperatingAirline>
				</xsl:otherwise>
			</xsl:choose>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="Equip" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="AirV" />
				</xsl:attribute>
			</MarketingAirline>
			<TPA_Extensions>
				<JourneyTotalDuration>
					<xsl:call-template name="duration">
						<xsl:with-param name="journey">
							<xsl:value-of select="JrnyTm" />
						</xsl:with-param>
					</xsl:call-template>
				</JourneyTotalDuration>
				<xsl:if test="FltTm != ''">
					<JourneyDuration>
						<xsl:call-template name="duration">
							<xsl:with-param name="journey">
								<xsl:value-of select="FltTm" />
							</xsl:with-param>
						</xsl:call-template>
					</JourneyDuration>
				</xsl:if>
			</TPA_Extensions>
		</FlightSegment>
		<xsl:if test="Conx='Y'">
			<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="Segment">
				<xsl:with-param name="class">
					<xsl:value-of select="$class" />
				</xsl:with-param>
				<xsl:with-param name="pos">
					<xsl:value-of select="$pos" />
				</xsl:with-param>
				<xsl:with-param name="nip">
					<xsl:value-of select="$nip" />
				</xsl:with-param>
				<xsl:with-param name="classpos">
					<xsl:value-of select="$classpos + 1"/>
				</xsl:with-param>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- **************************************************************** -->
<xsl:template match="PenFee" mode="pen">
    <Penalty>
		<xsl:attribute name="TicketRefundable">
			<xsl:choose>
				<xsl:when test="TkNonRef='Y'">N</xsl:when>
				<xsl:otherwise>Y</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="DepositRequired"><xsl:value-of select = "DepRequired"/></xsl:attribute>
		<xsl:attribute name="DepositRefundable">
			<xsl:choose>
				<xsl:when test="DepNonRef='Y'">N</xsl:when>
				<xsl:otherwise>Y</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="count(../PenFee)=1">
				<xsl:apply-templates select="../PenFee[1]" mode="fee"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="../PenFee[2]" mode="fee"/>
			</xsl:otherwise>
		</xsl:choose>
    </Penalty>
</xsl:template>

<xsl:template match="PenFee" mode="fee">
	<FeeApplies>    
		<xsl:attribute name="FailureToConfirm"><xsl:value-of select="FailConfirmSpace"/></xsl:attribute>
		<xsl:attribute name="Cancellation"><xsl:value-of select="Cancellation"/></xsl:attribute>
		<xsl:attribute name="Carrier"><xsl:value-of select="AirVFee"/></xsl:attribute>
		<xsl:attribute name="ReplaceTicket"><xsl:value-of select="ReplaceTk"/></xsl:attribute>
		<xsl:attribute name="ChangeItinerary"><xsl:value-of select="ItinChg"/></xsl:attribute>
	</FeeApplies>
	<xsl:if test="ItinChg = 'Y' or Cancellation='Y' or FailConfirmSpace='Y' or ReplaceTk='Y'">
		<xsl:variable name="Deci" select="substring-after(translate(Amt,' ',''),'.')"/>
   	   	<xsl:variable name="NoDeci" select="string-length($Deci)"/>
		<CurrencyCode>
			<xsl:choose>
				<xsl:when test="Type = 'D'">
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci"/>				
					</xsl:attribute>
					<xsl:value-of select="Currency"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="DecimalPlaces">0</xsl:attribute>
					<xsl:value-of select="//GenQuoteDetails/TotCurrency"/>
				</xsl:otherwise>
			</xsl:choose>
		</CurrencyCode>
	</xsl:if>
	<xsl:if test="ItinChg = 'Y'">
		<Change>
			<Amount>
				<xsl:attribute name="Unit">
					<xsl:choose>
						<xsl:when test="Type = 'D'">M</xsl:when>
						<xsl:when test="Type = 'P'">P</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="translate(string(Amt),'. ','')"/>
			</Amount>
		</Change>
	</xsl:if>
	<xsl:if test="Cancellation = 'Y'">
		<Cancel>
			<Amount>
				<xsl:attribute name="Unit">
					<xsl:choose>
						<xsl:when test="Type = 'D'">M</xsl:when>
						<xsl:when test="Type = 'P'">P</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="translate(string(Amt),'. ','')"/>
			</Amount>
		</Cancel>
	</xsl:if>
	<xsl:if test="FailConfirmSpace = 'Y'">
		<FailureToConfirm>
			<Amount>
				<xsl:attribute name="Unit">
					<xsl:choose>
						<xsl:when test="Type = 'D'">M</xsl:when>
						<xsl:when test="Type = 'P'">P</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="translate(string(Amt),'. ','')"/>
			</Amount>
		</FailureToConfirm>
	</xsl:if>
	<xsl:if test="ReplaceTk = 'Y'">
		<Replace>
			<Amount>
				<xsl:attribute name="Unit">
					<xsl:choose>
						<xsl:when test="Type = 'D'">M</xsl:when>
						<xsl:when test="Type = 'P'">P</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="translate(string(Amt),'. ','')"/>
			</Amount>
		</Replace>
	</xsl:if>
</xsl:template>

<xsl:template match="RsvnRules" mode="advp">
	<xsl:if test="AdvRsvnTm!='0' or AdvTkRsvnTm!='0' or AdvTkStartTm!='0'">
		<ResTicketingRules>
			<AdvResTicketing>
				<xsl:if test="AdvRsvnTm!='0'">
					<xsl:attribute name="AdvResInd">true</xsl:attribute>
				</xsl:if>
				<xsl:if test="AdvTkRsvnTm!='0' or AdvTkStartTm!='0'">
					<xsl:attribute name="AdvTicketingInd">true</xsl:attribute>
				</xsl:if>
				<xsl:if test="AdvRsvnTm!='0'">
					<AdvReservation>
						<xsl:if test="LatestRsvnDt != ''">
							<xsl:attribute name="LatestTimeOfDay">
								<xsl:value-of select="substring(LatestRsvnDt,1,4)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(LatestRsvnDt,5,2)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(LatestRsvnDt,7,2)"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="LatestPeriod"><xsl:value-of select="AdvRsvnTm"/></xsl:attribute>
						<xsl:attribute name="LatestUnit">
							<xsl:choose>
								<xsl:when test="AdvRsvnHrs='Y'">Hours</xsl:when>
								<xsl:when test="AdvRsvnDays='Y'">Days</xsl:when>
								<xsl:otherwise>Months</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</AdvReservation>
				</xsl:if>
				<xsl:if test="AdvTkRsvnTm!='0' or AdvTkStartTm!='0'">
					<AdvTicketing>
						<xsl:if test="AdvTkRsvnTm!='0'">
							<xsl:if test="LatestTkDt != ''">
								<xsl:attribute name="FromResTimeOfDay">
									<xsl:value-of select="substring(LatestTkDt,1,4)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(LatestTkDt,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(LatestTkDt,7,2)"/>
									<xsl:text>T00:00:00</xsl:text>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="FromResPeriod"><xsl:value-of select="AdvTkRsvnTm"/></xsl:attribute>
							<xsl:attribute name="FromResUnit">
								<xsl:choose>
									<xsl:when test="AdvTkRsvnHrs='Y'">Hours</xsl:when>
									<xsl:when test="AdvTkRsvnDays='Y'">Days</xsl:when>
									<xsl:otherwise>Months</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="AdvTkStartTm!='0'">
							<xsl:if test="LatestTkDt != ''">
								<xsl:attribute name="FromDepartTimeOfDay">
									<xsl:value-of select="substring(LatestTkDt,1,4)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(LatestTkDt,5,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(LatestTkDt,7,2)"/>
									<xsl:text>T00:00:00</xsl:text>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="FromDepartPeriod"><xsl:value-of select="AdvTkStartTm"/></xsl:attribute>
							<xsl:attribute name="FromDepartUnit">
								<xsl:choose>
									<xsl:when test="AdvTkStartHrs='Y'">Hours</xsl:when>
									<xsl:when test="AdvTkStartDays='Y'">Days</xsl:when>
									<xsl:otherwise>Months</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>
						<!--xsl:if test="../InfoMsg[contains(Text,'LAST DATE TO PURCHASE TICKET')]">
							<xsl:value-of select="../InfoMsg[contains(Text,'LAST DATE TO PURCHASE TICKET')]/Text"/>
						 </xsl:if-->
					</AdvTicketing>
				</xsl:if>
			</AdvResTicketing>
		</ResTicketingRules>
	</xsl:if>
</xsl:template>

<xsl:template match="RsvnRules" mode="minmax">
	<xsl:if test="TmDOWMin!='0'">
		<MinimumStay>
			<xsl:choose>
				<xsl:when test="NumOccurMin!='0'">
					<DayOfWeek>
						<xsl:attribute name="Day">
							<xsl:choose>
								<xsl:when test="TmDOWMin=1">7</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="TmDOWMin - 1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="NumOccurMin"/>
					</DayOfWeek>
				</xsl:when>
				<xsl:otherwise>
					<Value>
						<xsl:attribute name="Unit">
							<xsl:choose>
								<xsl:when test="HoursMin='Y'">H</xsl:when>
								<xsl:when test="DaysMin='Y'">D</xsl:when>
								<xsl:otherwise>M</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="TmDOWMin"/>
					</Value>
				</xsl:otherwise>
			</xsl:choose>
		</MinimumStay>
	</xsl:if>
	<xsl:if test="TmDOWMax!='0'">
		<MaximumStay>
			<xsl:choose>
				<xsl:when test="NumOccurMax!='0'">
					<DayOfWeek>
						<xsl:attribute name="Day">
							<xsl:choose>
								<xsl:when test="TmDOWMax=1">7</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="TmDOWMax - 1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="NumOccurMax"/>
					</DayOfWeek>
				</xsl:when>
				<xsl:otherwise>
					<Value>
						<xsl:attribute name="Unit">
							<xsl:choose>
								<xsl:when test="HoursMax='Y'">H</xsl:when>
								<xsl:when test="DaysMax='Y'">D</xsl:when>
								<xsl:otherwise>M</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:value-of select="TmDOWMax"/>
					</Value>
				</xsl:otherwise>
			</xsl:choose>
		</MaximumStay>
	</xsl:if>
</xsl:template>

<xsl:template match="InfoMsg" mode="rmk">
	<xsl:if test="not(contains(Text, 'LAST DATE TO PURCHASE TICKET'))">
		<Remark><xsl:value-of select="Text"/></Remark>
	</xsl:if>
</xsl:template>

	<xsl:template name="duration">
		<xsl:param name="journey" />
		<xsl:variable name="hours">
			<xsl:choose>
				<xsl:when test="substring-before(($journey div 60),'.')=''">
					<xsl:value-of select="$journey div 60" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(($journey div 60),'.')" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="minutes">
			<xsl:value-of select="$journey - ($hours*60)" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$minutes = 'NaN'">
				<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
				<xsl:value-of select="$hours" />
				<xsl:text>:00</xsl:text>
			</xsl:when>
			<xsl:otherwise>
			<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
			<xsl:value-of select="$hours" />:<xsl:value-of select="substring(string($zeros),1,2-string-length($minutes))" />
			<xsl:value-of select="$minutes" />
		</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
