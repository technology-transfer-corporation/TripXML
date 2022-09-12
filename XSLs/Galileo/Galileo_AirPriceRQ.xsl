<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_AirPriceRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 25 Apr 2007 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<FareQuoteFlightSpecific_18>
			<FlightSpecificBestBuyMods>
				<xsl:apply-templates select="OTA_AirPriceRQ" />
			</FlightSpecificBestBuyMods>
		</FareQuoteFlightSpecific_18>
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="OTA_AirPriceRQ">
		<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@FareQualifier != '12' or not(TravelerInfoSummary/PriceRequestInformation/@FareQualifier)">
			<GenFarePrefs>
				<Pen>
					<xsl:value-of select="string('  ')" />
				</Pen>
				<MinStay>Y</MinStay>
				<MaxStay>Y</MaxStay>
				<AP>Y</AP>
			</GenFarePrefs>
		</xsl:if>
		<SegInfo>
			<FltSegAry>
				<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption" />
			</FltSegAry>
		</SegInfo>
		<PsgrMods>
			<PsgrAry>
				<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[1]">
					<xsl:with-param name="counter">1</xsl:with-param>
				</xsl:apply-templates>
			</PsgrAry>
		</PsgrMods>
		<xsl:if test="POS/TPA_Extensions/Provider/Name = 'Galileo'">
			<PlatingAirVMods>
	  			<PlatingAirV>
	  				<xsl:value-of select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/MarketingAirline/@Code"/>
	  			</PlatingAirV> 
	 		 </PlatingAirVMods>
	 	</xsl:if>
		<GenQuoteInfo>
			<SellCity>
				<!--xsl:value-of select="POS/Source/@PseudoCityCode"/-->
			</SellCity>
			<TktCity>
				<!--xsl:value-of select="POS/Source/@PseudoCityCode"/-->
			</TktCity>
			<EquivCurrency>
			</EquivCurrency>
			<TkDt />
			<BkDtOverride />
		</GenQuoteInfo>
		<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@FareQualifier = '12'">
			<BookingCode>
				<SegAry>
					<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment" mode="class"/>
				</SegAry>
			</BookingCode>
		</xsl:if>
		<xsl:apply-templates select="TravelerInfoSummary/PriceRequestInformation" />
	</xsl:template>
	<!--************************************************************************************************************-->
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
		<xsl:apply-templates select="following-sibling::PassengerTypeQuantity[1]">
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
				<PIC>
					<xsl:choose>
						<xsl:when test="@Code='ADT'">
							<xsl:value-of select="string(' ')" />
						</xsl:when>
						<xsl:when test="@Code='CHD'">
							<xsl:text>CHD</xsl:text>
						</xsl:when>
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
				</PIC>
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
	
	<!--************************************************************************************************************-->
	<xsl:template match="OriginDestinationOption">
		<xsl:apply-templates select="FlightSegment" mode="flight"/>
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="FlightSegment" mode="flight">
		<xsl:variable name="DepDate">
			<xsl:value-of select="substring-before(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<xsl:variable name="ArrDate">
			<xsl:value-of select="substring-before(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)" />
		</xsl:variable>
		<FltSeg>
			<ClassPref>
				<xsl:choose>
					<xsl:when test="Cabin='F' or Cabin='C' or Cabin='Y'">
						<xsl:value-of select="Cabin" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="string(' ')" />
					</xsl:otherwise>
				</xsl:choose>
			</ClassPref>
			<AirV>
				<xsl:value-of select="MarketingAirline/@Code" />
			</AirV>
			<FltNum>
				<xsl:value-of select="@FlightNumber" />
			</FltNum>
			<Dt>
				<xsl:value-of select="translate(string($DepDate),'-','')" />
			</Dt>
			<StartAirp>
				<xsl:value-of select="DepartureAirport/@LocationCode" />
			</StartAirp>
			<EndAirp>
				<xsl:value-of select="ArrivalAirport/@LocationCode" />
			</EndAirp>
			<StartTm>
				<xsl:value-of select="translate(string($DepTime2),':','')" />
			</StartTm>
			<EndTm>
				<xsl:value-of select="translate(string($ArrTime2),':','')" />
			</EndTm>
			<AsBookedBIC>
				<xsl:value-of select="@ResBookDesigCode" />
			</AsBookedBIC>
			<xsl:variable name="dc" select="ttVB:FctDateDuration(string($DepDate),string($ArrDate))"/>
			<DayChgInd>
				<xsl:choose>
					<xsl:when test="$dc &lt; 0">
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring-after($dc,'-')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>0</xsl:text>
						<xsl:value-of select="$dc"/>
					</xsl:otherwise>
				</xsl:choose>
			</DayChgInd>
			<xsl:variable name="Pos" select="position()" />
			<xsl:variable name="CountPos" select="count(../FlightSegment)" />
			<xsl:choose>
				<xsl:when test="$Pos=$CountPos">
					<Conx>N</Conx>
				</xsl:when>
				<xsl:otherwise>
					<Conx>Y</Conx>
				</xsl:otherwise>
			</xsl:choose>
		</FltSeg>
	</xsl:template>
	
	<xsl:template match="FlightSegment" mode="class">
		<Seg>
			<SegNum>0<xsl:value-of select="position()"/></SegNum>
			<BIC1><xsl:value-of select="@ResBookDesigCode" /></BIC1>
		</Seg>
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="AirTravelerAvail">
		<Psgr>
			<LNameNum>
				<xsl:value-of select="AirTraveler/TravelerRefNumber/@RPH" />
			</LNameNum>
			<PsgrNum>
				<xsl:value-of select="AirTraveler/TravelerRefNumber/@RPH" />
			</PsgrNum>
			<AbsNameNum>
				<xsl:value-of select="AirTraveler/TravelerRefNumber/@RPH" />
			</AbsNameNum>
			<PIC>
				<xsl:choose>
					<xsl:when test="AirTraveler/@PassengerTypeCode='ADT'">
						<xsl:value-of select="string(' ')" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="AirTraveler/@PassengerTypeCode" />
					</xsl:otherwise>
				</xsl:choose>
			</PIC>
		</Psgr>
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="PriceRequestInformation">
		<SegSelection>
			<xsl:choose>
				<xsl:when test="@PricingSource='Published'">
					<ReqAirVPFs>N</ReqAirVPFs>
				</xsl:when>
				<xsl:otherwise>
					<ReqAirVPFs>Y</ReqAirVPFs>
				</xsl:otherwise>
			</xsl:choose>
			<SegRangeAry>
				<SegRange>
					<StartSeg>0</StartSeg>
					<EndSeg>0</EndSeg>
					<xsl:choose>
						<xsl:when test="@PricingSource='Published'">
							<FareType>N</FareType>
						</xsl:when>
						<xsl:otherwise>
							<FareType>P</FareType>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="@PricingSource!='Published'">
						<PFQual>
							<CRSInd>
								<xsl:choose>
									<xsl:when test="../../POS/TPA_Extensions/Provider/Name = 'Apollo'">1V</xsl:when>
									<xsl:otherwise>1G</xsl:otherwise>
								</xsl:choose>
							</CRSInd>
							<PCC>
								<xsl:value-of select="../../POS/Source/@PseudoCityCode" />
							</PCC>
							<Acct>
								<xsl:value-of select="NegotiatedFareCode/@Code" />
							</Acct>
							<Contract>
								<xsl:value-of select="NegotiatedFareCode/@SecondaryCode" />
							</Contract>
							<xsl:choose>
								<xsl:when test="@PricingSource='Both'">
									<PublishedFaresInd>Y</PublishedFaresInd>
								</xsl:when>
								<xsl:otherwise>
									<PublishedFaresInd>N</PublishedFaresInd>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:choose>
								<xsl:when test="@PricingSource='Both'">
									<Type>V</Type>
								</xsl:when>
								<xsl:otherwise>
									<Type>A</Type>
								</xsl:otherwise>
							</xsl:choose>
						</PFQual>
					</xsl:if>
				</SegRange>
			</SegRangeAry>
		</SegSelection>
	</xsl:template>
	<!--************************************************************************************************************-->

<msxsl:script language="VisualBasic" implements-prefix="ttVB">
<![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]>
</msxsl:script>

</xsl:stylesheet>
