<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
================================================================== 
 Worldspan_PNRReadRS.xsl 					     								       
================================================================== 
Date: 14 Jun 2018 - Kobelev - CompanyName object with Creation Date Time added.
Date: 20 Nov 2009 - Rastko														       
================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<xsl:apply-templates select="DPW8"/>
	<xsl:apply-templates select="XXW"/>
	<xsl:apply-templates select="BPW9"/>
</xsl:template>

<xsl:template match="XXW">
	<OTA_TravelItineraryRS Version="2.000">
		<Errors>
			<Error>
				<xsl:attribute name="Type">Worldspan</xsl:attribute>
				<xsl:attribute name="Code"><xsl:value-of select="ERROR/CODE"/></xsl:attribute>
				<xsl:value-of select="ERROR/TEXT"/>
			</Error>
		</Errors>
	</OTA_TravelItineraryRS>
</xsl:template>

<xsl:template match="BPW9">
	<OTA_TravelItineraryRS Version="2.000">
		<xsl:if test="not(PNR_RLOC) and TIME_PRICE_DIF != ''">
			<Errors>
				<Error>
					<xsl:attribute name="Type">Worldspan</xsl:attribute>
					<xsl:choose>
						<xsl:when test="TIME_PRICE_DIF = 'T'">Time difference detected</xsl:when>
						<xsl:when test="TIME_PRICE_DIF = 'P'">Price difference detected</xsl:when>
						<xsl:when test="TIME_PRICE_DIF = 'B'">Time and Price difference detected</xsl:when>
						<xsl:when test="TIME_PRICE_DIF = 'V'">Price variance detected</xsl:when>
						<xsl:when test="TIME_PRICE_DIF = 'L'">Lower price detected</xsl:when>
					</xsl:choose>
				</Error>
			</Errors>
		</xsl:if>
	</OTA_TravelItineraryRS>
</xsl:template>

<xsl:template match="DPW8">
	<OTA_TravelItineraryRS Version="2.000">
		<Success/>
		<xsl:if test="WARNING_INFO/WRN_ITEM/WRN_TEXT != ''">
			<Warnings>
				<xsl:for-each select="WARNING_INFO/WRN_ITEM">
					<Warning Type="Worldspan"><xsl:value-of select="WRN_TEXT"/></Warning>
				</xsl:for-each>
			</Warnings>
		</xsl:if>
		<TravelItinerary>
			<ItineraryRef>
				<xsl:attribute name="Type">PNR</xsl:attribute>
				<xsl:attribute name="ID">
					<xsl:value-of select="REC_LOC" />
				</xsl:attribute>
				<xsl:attribute name="ID_Context"><xsl:value-of select="PNR_INF/TEL_INF/TEL_NUM[1]/SID"/></xsl:attribute>
			  <CompanyName>
			    <!-- 13OCT2017 | 2017-3OC-13-->
			    <xsl:variable name="CreationDate">
			      <xsl:choose>
			        <xsl:when test="string-length(CRE_DAT) > 7">
			          <xsl:value-of select="substring(CRE_DAT,6,4)"/>
			          <xsl:text>-</xsl:text>
			          <xsl:value-of select="substring(CRE_DAT,3,3)"/>
			          <xsl:text>-</xsl:text>
			          <xsl:value-of select="substring(CRE_DAT,1,2)"/>
			        </xsl:when>
			        <xsl:otherwise>
			          <xsl:value-of select="concat(20, substring(CRE_DAT,6,2))"/>
			          <xsl:text>-</xsl:text>
			          <xsl:value-of select="substring(CRE_DAT,3,3)"/>
			          <xsl:text>-</xsl:text>
			          <xsl:value-of select="substring(CRE_DAT,1,2)"/>
			        </xsl:otherwise>
			      </xsl:choose>
			    </xsl:variable>
          
			    <xsl:attribute name="Code">
			      <xsl:value-of select="concat(PNR_INF/TEL_INF/TEL_NUM[2]/SID, '|', $CreationDate)"/>
			    </xsl:attribute>
			    <xsl:attribute name="CodeContext">IATACode</xsl:attribute>
			    <xsl:value-of select="PNR_INF/TEL_INF/TEL_NUM[3]/SID"/>
			  </CompanyName>
			</ItineraryRef>
			<CustomerInfos>
				<xsl:apply-templates select="PAX_INF/NME_ITM"/>	
			</CustomerInfos>
			<ItineraryInfo>
				<ReservationItems>
					<xsl:apply-templates select="AIR_SEG_INF/AIR_ITM"/>
					<xsl:apply-templates select="PRC_INF/PRC_QUO" />
				</ReservationItems>
				<xsl:apply-templates select="PNR_INF/TIC_INF"/>
				<xsl:if test="SSR_INF/SSR_ITM">
					<SpecialRequestDetails>
						<xsl:if test="SSR_INF/SSR_ITM">
							<SpecialServiceRequests>
								<xsl:apply-templates select="SSR_INF/SSR_ITM" mode="SSR"/>
							</SpecialServiceRequests>		
						</xsl:if>
					</SpecialRequestDetails>
				</xsl:if>
			</ItineraryInfo>
			<TravelCost>
				<xsl:apply-templates select="PNR_INF/PMT_INF"/>
			</TravelCost>
			<xsl:if test="RMK_INF/GEN_RMK_INF/RMK_ITM">
				<Remarks>
					<xsl:for-each select="RMK_INF/GEN_RMK_INF/RMK_ITM">
						<Remark><xsl:value-of select="RMK_TXT"/></Remark>
					</xsl:for-each>
				</Remarks>
			</xsl:if>
		</TravelItinerary>
	</OTA_TravelItineraryRS>
</xsl:template>

<xsl:template match="PRC_QUO">
	<ItemPricing>
		<AirFareInfo>
			<xsl:choose>
				<xsl:when test="PRC_QUO_CMD != ''">
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<ItinTotalFare>
				<!--xsl:variable name="totbase">
					<xsl:variable name="amttot">
						<xsl:apply-templates select="FARE_INFO[1]" mode="basefare">
							<xsl:with-param name="total">0</xsl:with-param>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:value-of select="substring-before($amttot,'/')" />
				</xsl:variable>
				<xsl:variable name="tottax">
					<xsl:variable name="amttot">
						<xsl:apply-templates select="FARE_INFO[1]" mode="TotalTax">
							<xsl:with-param name="total">0</xsl:with-param>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:value-of select="substring-before($amttot,'/')" />
				</xsl:variable-->
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_AMT!=''">
								<xsl:value-of select="translate(TTL_BAS_FAR_AMT,'.','')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="translate(EQV_BAS_FAR_AMT,'.','')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="TTL_BAS_FAR_CUR_COD"/></xsl:when>
							<xsl:when test="EQV_BAS_FAR_CUR_COD != ''"><xsl:value-of select="EQV_BAS_FAR_CUR_COD"/></xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</BaseFare>
				<Taxes>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(TTL_TAX_AMT,'.','')"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="TTL_BAS_FAR_CUR_COD"/></xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(TTL_PRC_AMT,'.','')"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="TTL_BAS_FAR_CUR_COD"/></xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FAR_DTL" mode="Details" />
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</ItemPricing>
</xsl:template>
	
<xsl:template match="FARE_INFO" mode="basefare">
	<xsl:param name="total" />
	<xsl:variable name="totpax">
		<xsl:value-of select="NUMBER_OF_PEOPLE"/>
	</xsl:variable>
	<xsl:variable name="thistotal">
		<xsl:value-of select="translate(FARE,'.','') * $totpax" />
	</xsl:variable>
	<xsl:variable name="bigtotal">
		<xsl:value-of select="$total + $thistotal" />
	</xsl:variable>
	<xsl:apply-templates select="following-sibling::FARE_INFO[1]" mode="basefare">
		<xsl:with-param name="total">
			<xsl:value-of select="$bigtotal" />
		</xsl:with-param>
	</xsl:apply-templates>
	<xsl:value-of select="$bigtotal" />
	<xsl:text>/</xsl:text>
</xsl:template>

<xsl:template match="FARE_INFO" mode="TotalTax">
	<xsl:param name="total" />
	<xsl:variable name="totpax">
		<xsl:value-of select="NUMBER_OF_PEOPLE"/>
	</xsl:variable>
	<xsl:variable name="thistotal">
		<xsl:value-of select="translate(TAX,'.','') * $totpax" />
	</xsl:variable>
	<xsl:variable name="bigtotal">
		<xsl:value-of select="$total + $thistotal" />
	</xsl:variable>
	<xsl:apply-templates select="following-sibling::FARE_INFO[1]" mode="basefare">
		<xsl:with-param name="total">
			<xsl:value-of select="$bigtotal" />
		</xsl:with-param>
	</xsl:apply-templates>
	<xsl:value-of select="$bigtotal" />
	<xsl:text>/</xsl:text>
</xsl:template>

<xsl:template match="PTC_FAR_DTL" mode="Details">
	<xsl:variable name="base">
		<xsl:choose>
			<xsl:when test="BAS_FAR_AMT!=''">
				<xsl:value-of select="translate(BAS_FAR_AMT,'.','')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="translate(EQV_BAS_FAR_AMT,'.','')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="tax"><xsl:value-of select="translate(TAX_AMT,'.','')" /></xsl:variable>
	<xsl:variable name="paxtype"><xsl:value-of select="PTC"/></xsl:variable>
	<xsl:variable name="nip"><xsl:value-of select="count(../../../PAX_INF/NME_ITM[PTC=$paxtype])"/></xsl:variable> 
	<xsl:variable name="totbase"><xsl:value-of select="$base * $nip"/></xsl:variable>
	<xsl:variable name="tottax"><xsl:value-of select="$tax * $nip"/></xsl:variable>
	<xsl:variable name="totfare"><xsl:value-of select="$totbase + $tottax"/></xsl:variable>
	<PTC_FareBreakdown>
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="PTC = 'CNN'">CHD</xsl:when>
					<xsl:when test="PTC = 'GGV'">GOV</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="PTC" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity"><xsl:value-of select="$nip"/></xsl:attribute>
		</PassengerTypeQuantity>
		<FareBasisCodes>
			<xsl:for-each select="FAR_BAS_COD">
				<FareBasisCode><xsl:value-of select="." /></FareBasisCode>
			</xsl:for-each>
		</FareBasisCodes>
		<PassengerFare>
			<BaseFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$totbase"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="../TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/></xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</BaseFare>
			<Taxes>
				<xsl:attribute name="Amount"><xsl:value-of select="$tottax"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="../TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/></xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</Taxes>
			<TotalFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$totfare" /></xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="../TTL_BAS_FAR_CUR_COD != ''"><xsl:value-of select="../TTL_BAS_FAR_CUR_COD"/></xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</TotalFare>
		</PassengerFare>
	</PTC_FareBreakdown>
</xsl:template>
	
<xsl:template match="NME_ITM">
   <CustomerInfo>
	   <xsl:attribute name="RPH"><xsl:value-of select="NME_POS"/></xsl:attribute>
	   <Customer>
	   		<xsl:if test="CST_NME_INF != ''">
	   			<xsl:attribute name="BirthDate">
	   				<xsl:text>20</xsl:text>
	   				<xsl:value-of select="substring(CST_NME_INF,1,2)"/>
	   				<xsl:text>-</xsl:text>
	   				<xsl:value-of select="substring(CST_NME_INF,3,2)"/>
					<xsl:text>-</xsl:text>
	   				<xsl:value-of select="substring(CST_NME_INF,5,2)"/>
	   			</xsl:attribute>
	   		</xsl:if>
		   	<PersonName>
			   	<xsl:attribute name="NameType"><xsl:value-of select="PTC"/></xsl:attribute>
			  	 <GivenName><xsl:value-of select="substring-after(string(PAX_NME),'/')"/></GivenName>			
				<Surname><xsl:value-of select="substring-before(string(PAX_NME),'/')"/></Surname>			
		   	</PersonName> 
		   	<xsl:apply-templates select="../../PNR_INF/TEL_INF/TEL_NUM"/>	
	   </Customer>
	</CustomerInfo>
</xsl:template>

<xsl:template match="TEL_NUM">
	<Telephone>
		<xsl:attribute name="PhoneNumber">
			<xsl:value-of select="NUM"/>
		</xsl:attribute>		
	</Telephone>
</xsl:template>

<xsl:template match="PMT_INF">
	<FormOfPayment>
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()" />
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="PMT_COD = 'CA'">
				<TPA_Extensions>
					<xsl:attribute name="FOPType">Cash</xsl:attribute>
				</TPA_Extensions>
			</xsl:when>
			<xsl:when test="PMT_COD = 'CK'">
				<TPA_Extensions>
					<xsl:attribute name="FOPType">Check</xsl:attribute>
				</TPA_Extensions>
			</xsl:when>
			<xsl:otherwise>
				<PaymentCard>
					<xsl:attribute name="CardCode">
						<xsl:choose>
							<xsl:when test="CC_TYP = 'CA'">MC</xsl:when>
							<xsl:when test="CC_TYP = 'DC'">DN</xsl:when>
							<xsl:otherwise><xsl:value-of select="CC_TYP"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CardNumber">
						<xsl:value-of select="CC_NUM"/>
					</xsl:attribute>
					<xsl:attribute name="ExpireDate"><xsl:value-of select="CC_EXP_DAT"/></xsl:attribute>
				</PaymentCard>
				<TPA_Extensions>
					<xsl:attribute name="FOPType">CC</xsl:attribute>
				</TPA_Extensions>
			</xsl:otherwise>
		</xsl:choose>
	</FormOfPayment>
</xsl:template>
		
<xsl:template match="TIC_INF">
	<Ticketing>
		<xsl:attribute name="TicketType">
			<xsl:choose>
				<xsl:when test="//AIR_SEG_INF/AIR_ITM/E_TIC_ELI='Y'">eTicket</xsl:when>
				<xsl:otherwise>Paper</xsl:otherwise>			
			</xsl:choose>
		</xsl:attribute>
		<TicketAdvisory><xsl:value-of select="."/></TicketAdvisory> 
	</Ticketing>
</xsl:template>

<xsl:template match="Text">
	<Text><xsl:value-of select="."/></Text>
</xsl:template>


<xsl:template match="AIR_ITM"> 
	<Item>
		<xsl:attribute name="ItinSeqNumber">
			<xsl:choose>
				<xsl:when test="SEG_NUM!=''"><xsl:value-of select="SEG_NUM" /></xsl:when>
				<xsl:otherwise><xsl:value-of select="position()" /></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<Air>
			<xsl:attribute name="RPH">
				<xsl:choose>
					<xsl:when test="SEG_NUM!=''"><xsl:value-of select="SEG_NUM" /></xsl:when>
					<xsl:otherwise><xsl:value-of select="position()" /></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="../../PAX_INF/ITM_COU"/></xsl:attribute>
			<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="CLA_COD"/></xsl:attribute>
			<xsl:attribute name="Status"><xsl:value-of select="SEG_STA"/></xsl:attribute>
			<xsl:attribute name="DepartureDateTime">
				<xsl:value-of select="substring(FLI_DAT,6)"/>
				<xsl:text>-</xsl:text>
				<xsl:call-template name="month">
					<xsl:with-param name="month"><xsl:value-of select="substring(FLI_DAT,3,3)"/></xsl:with-param>
				</xsl:call-template>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(FLI_DAT,1,2)"/>
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring(DEP_TIM,1,2)"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring(DEP_TIM,3,4)"/>
				<xsl:text>:00</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">
				<xsl:value-of select="substring(ARR_DAT,6)"/>
				<xsl:text>-</xsl:text>
				<xsl:call-template name="month">
					<xsl:with-param name="month"><xsl:value-of select="substring(ARR_DAT,3,3)"/></xsl:with-param>
				</xsl:call-template>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(ARR_DAT,1,2)"/>
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring(ARR_TIM,1,2)"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring(ARR_TIM,3,4)"/>
				<xsl:text>:00</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="FLI_NUM"/></xsl:attribute>
			<xsl:if test="E_TIC_ELI != ''">
				<xsl:attribute name="E_TicketEligibility">
					<xsl:choose>
						<xsl:when test="E_TIC_ELI = 'Y'">Eligible</xsl:when>
						<xsl:otherwise>NotEligible</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="NUM_STO != ''">
				<xsl:attribute name="StopQuantity">
					<xsl:value-of select="NUM_STO"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="FLI_DAY != ''">
				<xsl:attribute name="DepartureDay">
					<xsl:choose>
						<xsl:when test="FLI_DAY = 'Monday'">Mon</xsl:when>
						<xsl:when test="FLI_DAY = 'Tuesday'">Tue</xsl:when>
						<xsl:when test="FLI_DAY = 'Wednesday'">Wed</xsl:when>
						<xsl:when test="FLI_DAY = 'Thursday'">Thu</xsl:when>
						<xsl:when test="FLI_DAY = 'Friday'">Fri</xsl:when>
						<xsl:when test="FLI_DAY = 'Saturday'">Sat</xsl:when>
						<xsl:otherwise>Sun</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:if>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="DEP_ARP"/></xsl:attribute>
				<xsl:if test="DEP_TER != '' and DEP_TER != '*'">
					<xsl:attribute name="Terminal"><xsl:value-of select="DEP_TER"/></xsl:attribute>
				</xsl:if>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="ARR_ARP"/></xsl:attribute>
				<xsl:if test="ARR_TER != '' and ARR_TER != '*'">
					<xsl:attribute name="Terminal"><xsl:value-of select="ARR_TER"/></xsl:attribute>
				</xsl:if>
			</ArrivalAirport>
			<Equipment>
				<xsl:attribute name="AirEquipType"><xsl:value-of select="ADD_FLI_SVC/EQP_TYP"/></xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="ARL_COD"/></xsl:attribute>
			</MarketingAirline>
		</Air>
	</Item>
</xsl:template>

<xsl:template match="ADDRESS_INFO"> 
<xsl:if test="BILLING_LINE_INFO!=''">
<AddressGroup>
<xsl:attribute name="Type">O</xsl:attribute>
<xsl:if test="BILLING_LINE_INFO!=''">
<xsl:attribute name="Use">B</xsl:attribute>
</xsl:if>
	<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
	<AddressText>
	<xsl:value-of select="BILLING_LINE_INFO[position()=1]"/><xsl:value-of select="BILLING_LINE_INFO[position()=2]"/><xsl:value-of select="BILLING_LINE_INFO[position()=3]"/><xsl:value-of 	select="BILLING_LINE_INFO[position()=4]"/><xsl:value-of select="BILLING_LINE_INFO[position()=5]"/><xsl:value-of select="BILLING_LINE_INFO[position()=6]"/></AddressText>
</AddressGroup>
</xsl:if>
		<xsl:if test="MAILING_LINE_INFO!=''">
			<AddressGroup>
				<xsl:attribute name="Type">O</xsl:attribute>
				<xsl:attribute name="Use">D</xsl:attribute>
				<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
				<AddressText>
					<xsl:value-of select="MAILING_LINE_INFO[position()=1]"/><xsl:value-of select="MAILING_LINE_INFO[position()=2]"/><xsl:value-of select="MAILING_LINE_INFO[position()=3]"/><xsl:value-of select="MAILING_LINE_INFO[position()=4]"/><xsl:value-of select="MAILING_LINE_INFO[position()=5]"/><xsl:value-of select="MAILING_LINE_INFO[position()=6]"/>
				</AddressText>
			</AddressGroup>			
	</xsl:if>
</xsl:template>

<xsl:template match="HTL_ITEM"> 
	<HotelSegment>
		<ElementNumber><xsl:value-of select="SEG_NUM"/></ElementNumber>
		<TravelerElementNumber>!func:GetGlobal(HTravelerIDRef)</TravelerElementNumber>
		<Hotel>
		<CheckInDate>(<xsl:value-of select="concat(string(CITY_IN_OUT/IN_DATE/IN_DAY), string(CITY_IN_OUT/IN_DATE/IN_MONTH))"/>)</CheckInDate> 
		<CheckOutDate>(<xsl:value-of select="concat(string(CITY_IN_OUT/OUT_DATE/OUT_DAY), string(CITY_IN_OUT/OUT_DATE/OUT_MONTH))"/>)</CheckOutDate> 
		<NumberOfNights>!func:Math( SubDate , <xsl:value-of select="CITY_IN_OUT/OUT_DATE/OUT_DAY"/><xsl:value-of select="CITY_IN_OUT/OUT_DATE/OUT_MONTH"/>,<xsl:value-of select="CITY_IN_OUT/IN_DATE/IN_DAY"/><xsl:value-of select="CITY_IN_OUT/IN_DATE/IN_MONTH"/>, ddMMM, ddMMM)</NumberOfNights>
		<NumberOfPersons><xsl:value-of select="NUMBER_OF_PERSONS"/></NumberOfPersons>
		<ChainCode><xsl:value-of select="HTL_CHAIN_CODE"/></ChainCode>
		<ChainName>(Hotels,<xsl:value-of select="HTL_CHAIN_CODE"/>)</ChainName>
		<PropertyCode><xsl:value-of select="HTL_PROP_CODE"/></PropertyCode>
		<PropertyName><xsl:value-of select="HTL_PROP_NAME"/></PropertyName>
		<CityCode><xsl:value-of select="CITY_IN_OUT/HTL_CITY_CODE"/></CityCode>		
		<CityName> (Airports, <xsl:value-of select="CITY_IN_OUT/HTL_CITY_CODE" />)</CityName>
		<CurrencyCode>
			<xsl:variable name="HtlDeci" select="substring-after(string(//Collects/Collect/HBW/ROOM_RATE),'.')"/>
			<xsl:variable name="HtlNoDeci" select="string-length($HtlDeci)"/>
			<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$HtlNoDeci" /></xsl:attribute>			
			<xsl:value-of select="RATE_CUR_CODE"/>
		</CurrencyCode>
		</Hotel>
		<Rooms>
			<BookingCode><xsl:value-of select="ROOM_TYPE"/></BookingCode>
			<RateCategory>!func:GetGlobal(RateCategory)</RateCategory>
			<RoomType><xsl:value-of select="ROOM_TYPE"/></RoomType>
			<RoomTypeDescription>(RoomTypes,<xsl:value-of select="ROOM_TYPE"/>)</RoomTypeDescription>		
			<ActionCode><xsl:value-of select="HTL_STATUS_ROOM/HTL_STATUS_CODE"/></ActionCode>
			<NumberOfRooms><xsl:value-of select="HTL_STATUS_ROOM/NUMBER_OF_ROOMS"/></NumberOfRooms>
			<RateCode>!func:GetGlobal(RateCode)</RateCode>
			<xsl:choose>
				<xsl:when test="RATE_DESCRIPTION!=''">
				<RateCodeDescription><xsl:value-of select="RATE_DESCRIPTION"/></RateCodeDescription>
				</xsl:when>
				<xsl:otherwise><RateCodeDescription>!func:GetGlobal(RateCode)</RateCodeDescription></xsl:otherwise>
				</xsl:choose>
			<xsl:if test="//Collects/Collect/HBW/ROOM_RATE!=''">
			<RateAmount><xsl:value-of select="translate(string(//Collects/Collect/HBW/ROOM_RATE),'.','')" /></RateAmount></xsl:if>
			<xsl:choose>
				<xsl:when test="Rate_CHG_MAY_APPLY!=''">
						<RateChange><xsl:value-of select="RATE-CHG-MAY-APPLY"/></RateChange>
				</xsl:when>
				<xsl:otherwise>
					<RateChange>N</RateChange>
				</xsl:otherwise>
			</xsl:choose>
		</Rooms>
		<xsl:if test="EXTRA_CHARGES">
		<RoomOptions>
			<ExtraAdult><xsl:value-of select="EXTRA_CHARGES/EA_AMOUNT"/></ExtraAdult>
			<ExtraChild><xsl:value-of select="EXTRA_CHARGES/EC_AMOUNT"/></ExtraChild>	
			<RollawayAdult><xsl:value-of select="EXTRA_CHARGES/RA_AMOUNT"/></RollawayAdult>
			<RollawayChild><xsl:value-of select="EXTRA_CHARGES/RC_AMOUNT"/></RollawayChild>	
			<Crib><xsl:value-of select="EXTRA_CHARGES/CR_AMOUNT"/></Crib>
		</RoomOptions>
		</xsl:if>
		<xsl:if test="HTL_CONF_NBR!=''">
		<ConfirmationNumber><xsl:value-of select="HTL_CONF_NBR"/></ConfirmationNumber>
		</xsl:if>
		<xsl:if test="HTL_GUARANTEE_INFO!=''">
		<SupplementalInformation>
			<PaymentGuarantee>
			<CreditCard>
			<CCCode><xsl:value-of select="substring(string(HTL_GUARANTEE_INFO),3,2)"/></CCCode>
			<xsl:variable name="CCHotelOne"><xsl:value-of select="substring(string(HTL_GUARANTEE_INFO),5,35)"/></xsl:variable>
			<xsl:variable name="CCHotelTwo"><xsl:value-of select="substring-before(string($CCHotelOne),'E')"/></xsl:variable>					
			<CCNumber><xsl:value-of select="$CCHotelTwo"/></CCNumber>
			<xsl:variable name="CCExpMonth"><xsl:value-of select="substring-after(string(HTL_GUARANTEE_INFO),'P')"/></xsl:variable>
			<xsl:variable name="CCExpMonthTwo"><xsl:value-of select="substring-before(string($CCExpMonth),'-')"/></xsl:variable>
			<xsl:variable name="CCExpYear"><xsl:value-of select="substring-after(string($CCExpMonth),'-')"/></xsl:variable>
			<CCExpiration>
				<Month><xsl:value-of select="$CCExpMonthTwo"/></Month>
				<Year>	<xsl:value-of select="$CCExpYear"/></Year>
			</CCExpiration>
			</CreditCard>
		</PaymentGuarantee>		
		</SupplementalInformation>
		</xsl:if>
	</HotelSegment>
</xsl:template>

<xsl:template match="PSG_ITEM" mode="TravelerElementNumber">
<xsl:if test="CUSTOM_NAME_DATA!=''">
<xsl:if test="CUSTOM_NAME_DATA!='*'">
<TravelerElementNumber><xsl:value-of select="CUSTOM_NAME_DATA"/></TravelerElementNumber>
</xsl:if>
</xsl:if>
</xsl:template>
	
<xsl:template match="CAR_ITEM">
	<CarSegment>
		<xsl:variable name="SegNum"><xsl:value-of select="SEG_NUM"/></xsl:variable>
		<ElementNumber><xsl:value-of select="SEG_NUM"/></ElementNumber>		
		<xsl:variable name="Passenger"><xsl:value-of select="RENTER_NAME"/></xsl:variable>
		<TravelerElementNumber>!func:GetGlobal(TravelerIDRef)</TravelerElementNumber>
		<NumberOfCars><xsl:value-of select="VENDOR_INFO/NUMBER_OF_CARS"/></NumberOfCars>
		<PickUp>
		<AirportCode><xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/></AirportCode>
		<AirportName> (Airports, <xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE" />)</AirportName>
<xsl:variable name="Date"><xsl:value-of select="concat(string(VENDOR_INFO/PICKUP_DATE/PICKUP_DAY), string (VENDOR_INFO/PICKUP_DATE/PICKUP_MONTH))"/></xsl:variable>
<Date>(<xsl:value-of select="$Date"/>)</Date>
<xsl:variable name="PickUpTime">
<xsl:value-of select="substring-after(string(ARR_FLIGHT_INFO),'-')"/></xsl:variable>
<Time><xsl:value-of select="substring(string($PickUpTime),1,2)"/>:<xsl:value-of select="substring(string($PickUpTime),3,2)"/></Time>
<xsl:variable name="Flight"><xsl:value-of select="substring(string(ARR_FLIGHT_INFO),3,10)"/></xsl:variable>
		<xsl:variable name="Flighttwo"><xsl:value-of select="substring-before(string($Flight),'-')"/></xsl:variable>
			<FlightArrival>
				<AirlineCode><xsl:value-of select="substring(string(ARR_FLIGHT_INFO),1,2)"/></AirlineCode>
				<AirlineName> (Airlines, <xsl:value-of select="substring(string(ARR_FLIGHT_INFO),1,2)"/>)</AirlineName>
				<FlightNumber><xsl:value-of select="$Flighttwo"/></FlightNumber>
			</FlightArrival>
		</PickUp>
		<DropOff>
			<AirportCode><xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/></AirportCode>
			<AirportName> (Airports, <xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE" />)</AirportName>
			<Date>(<xsl:value-of select="concat(string(VENDOR_INFO/DROP_OFF_DATE/DROP_OFF_DAY), string(VENDOR_INFO/DROP_OFF_DATE/DROP_OFF_MONTH))"/>)</Date>
<xsl:if test="DROP_OFF_TIME!=''"> 
		<Time><xsl:value-of select="substring(string(DROP_OFF_TIME),1,2)"/>:<xsl:value-of select="substring(string(DROP_OFF_TIME),3,2)"/></Time></xsl:if>
		</DropOff>
		<CarData>
			<CarVendorCode><xsl:value-of select="VENDOR_INFO/CAR_VENDOR_CODE"/></CarVendorCode>
		<CarVendorName>(Cars,<xsl:value-of select="VENDOR_INFO/CAR_VENDOR_CODE"/>)</CarVendorName>
		<Location>
				<CityCode><xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/></CityCode>
				<Category><xsl:value-of select="LOCATION/LOCATION_CODE"/></Category>
				<Number><xsl:value-of select="LOCATION/LOCATION_CODE_EXT"/></Number>  
		</Location>
		<CarType><xsl:value-of select="CAR_FLAGS/CAR_CLASS_CODE"/><xsl:value-of select="CAR_FLAGS/CAR_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/SHIFT_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/AIR_COND_CODE"/></CarType>
		<CarTypeDescription>(CarTypes,<xsl:value-of select="CAR_FLAGS/CAR_CLASS_CODE"/><xsl:value-of select="CAR_FLAGS/CAR_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/SHIFT_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/AIR_COND_CODE"/>)</CarTypeDescription>
		<ActionCode><xsl:value-of select="VENDOR_INFO/CAR_STATUS_CODE"/></ActionCode>
		<Rate>
			<xsl:apply-templates select="RATE_INFO/RATE_PLAN_CODE"/>	
			<xsl:if test="RATE_INFO/RATE_CATEGORY">
			<xsl:attribute name="Category"><xsl:value-of select="RATE_INFO/RATE_CATEGORY"/></xsl:attribute></xsl:if>
				<RateCode><xsl:value-of select="RATE_CODE"/></RateCode>
				<RateAmount><xsl:value-of select="translate(string(RATE_INFO/RATE_AMAOUNT),'.','')" /></RateAmount> 
<xsl:variable name="CarDeci" select="substring-after(string(RATE_INFO/RATE_AMAOUNT),'.')"/>
			<xsl:variable name="CarNoDeci" select="string-length($CarDeci)"/>
				<CurrencyCode>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$CarNoDeci" /></xsl:attribute>
					<xsl:value-of select="RATE_INFO/CURR_CODE"/>
				</CurrencyCode>
				<xsl:if test="RATE_INFO/MILEAGE_INFO/MILEAGE_CHARGE!='' or RATE_INFO/MILEAGE_INFO/FREE_MILEAGE!=''">
				<MileKmRate><xsl:value-of select="RATE_INFO/MILEAGE_INFO/MILEAGE_CHARGE"/><xsl:value-of select="RATE_INFO/MILEAGE_INFO/FREE_MILEAGE"/></MileKmRate> 
				</xsl:if>
		</Rate>
		<xsl:if test="RATE_INFO/EXTRA_DAY_RATE!='' or RATE_INFO/EXTRA_HOUR_RATE!=''">
		<ExtraCharges>
		<xsl:if test="RATE_INFO/EXTRA_DAY_RATE!=''">
		<xsl:attribute name="Type">D</xsl:attribute>
		</xsl:if>
		<ExtraChargesAmount>xsl:value-of select="RATE_INFO/EXTRA_DAY_RATE"/></ExtraChargesAmount>		
		<MileKmLimit><xsl:value-of select="RATE_INFO/EXTRA_DAY_MILEAGE/FREE_MILEAGE"/></MileKmLimit>
		<xsl:if test="RATE_INFO/EXTRA_HOUR_RATE!=''">
		<xsl:attribute name="Type">H</xsl:attribute>
		<ExtraChargesAmount><xsl:value-of select="RATE_INFO/EXTRA_HOUR_RATE"/></ExtraChargesAmount>		<MileKmLimit><xsl:value-of select="RATE_INFO/EXTRA_HOUR_MILEAGE/FREE_MILEAGE"/></MileKmLimit>
		</xsl:if>
		<xsl:if test="RATE_INFO/EXTRA_WEEK_RATE!=''">
		<xsl:attribute name="Type">H</xsl:attribute>
		<ExtraChargesAmount><xsl:value-of select="RATE_INFO/EXTRA_WEEK_RATE"/></ExtraChargesAmount>	
	</xsl:if>
		</ExtraCharges>
		</xsl:if>
			<xsl:if test="SPECIAL_EQUIP_CODES!=''">
		<OptionalEquipment>
			<EquipmentType><xsl:value-of select="SPECIAL_EQUIP_CODES"/></EquipmentType>
		</OptionalEquipment>
		</xsl:if>
	</CarData>
	<xsl:if test="CONFIRMATION_NUM!=''"><ConfirmationNumber><xsl:value-of select="CONFIRMATION_NUM"/></ConfirmationNumber></xsl:if>
	<SupplementalInformation>
				<BookingSource><xsl:value-of select="BS_IATA_NUM"/></BookingSource>
				<xsl:if test="CORP_DISCOUNT_NUM!=''">
				<CorporateDiscountNumber><xsl:value-of select="CORP_DISCOUNT_NUM"/></CorporateDiscountNumber></xsl:if>
				<xsl:if test="GUARANTEE_INFO">
				<PaymentGuarantee>
					<CreditCard>
						<CCCode><xsl:value-of select="substring(string(GUARANTEE_INFO),3,2)"/></CCCode>
<xsl:variable name="CCOne"><xsl:value-of select="substring(string(GUARANTEE_INFO),5,35)"/></xsl:variable>
<xsl:variable name="CCTwo"><xsl:value-of select="substring-before(string($CCOne),'E')"/></xsl:variable>					
					<CCNumber><xsl:value-of select="$CCTwo"/></CCNumber>
<xsl:variable name="CCExpMonth"><xsl:value-of select="substring-after(string(GUARANTEE_INFO),'P')"/></xsl:variable>
<xsl:variable name="CCExpMonthTwo"><xsl:value-of select="substring-before(string($CCExpMonth),'-')"/></xsl:variable>
<xsl:variable name="CCExpYear"><xsl:value-of select="substring-after(string($CCExpMonth),'-')"/></xsl:variable>
					<CCExpiration>
							<Month><xsl:value-of select="$CCExpMonthTwo"/></Month>
							<Year><xsl:value-of select="$CCExpYear"/></Year>
					</CCExpiration>
				</CreditCard>
			</PaymentGuarantee>
			</xsl:if>
			<xsl:if test="GUARANTEE_INFO"></xsl:if>
			<xsl:if test="SUPPLE_INFO!=''">
			<AdditionalInformation><xsl:value-of select="SUPPLE_INFO"/></AdditionalInformation></xsl:if>
	</SupplementalInformation>	
	</CarSegment>
</xsl:template>

<xsl:template match="RATE_PLAN_CODE">
<xsl:attribute name="Type">
				<xsl:choose>
				<xsl:when test=".='DY'">D</xsl:when>
				<xsl:when test=".='WK'">W</xsl:when>
				<xsl:when test=".='MY'">M</xsl:when>
				<xsl:when test=".='WD'">E</xsl:when>
				<xsl:when test=".='HR'">H</xsl:when>
				</xsl:choose>
</xsl:attribute>
</xsl:template>

<xsl:template match="AIR_ITEM" mode="SEAT"> 
	<AirlineCode><xsl:value-of select="AIRLINE_CODE"/></AirlineCode> 
	<AirlineName>(Airlines,<xsl:value-of select="AIRLINE_CODE"/>)</AirlineName> 
	<FlightNumber><xsl:value-of select="FLIGHT_NUM"/></FlightNumber>	
	<Date>(<xsl:value-of select="concat(string(DEP_DATE/DEP_DAY), string(DEP_DATE/DEP_MONTH))"/>)</Date>
	<DepartureAirportCode><xsl:value-of select="DEP_AIRPORT"/></DepartureAirportCode>
	<DepartureAirportName>(Airlines,<xsl:value-of select="DEP_AIRPORT"/>)</DepartureAirportName>
	<ArrivalAirportCode><xsl:value-of select="ARR_AIRPORT"/></ArrivalAirportCode>
	<ArrivalAirportName>>(Airlines,<xsl:value-of select="ARR_AIRPORT"/>)</ArrivalAirportName>
	<ClassOfService><xsl:value-of select="DEP_CLASS"/></ClassOfService>	
</xsl:template>

<xsl:template match="AIR_ITEM" mode="SEATTWO"> <!-- SD Done -->
	<AirlineCode><xsl:value-of select="AIRLINE_CODE"/></AirlineCode> 
	<AirlineName>(Airlines,<xsl:value-of select="AIRLINE_CODE"/>)</AirlineName> 
	<FlightNumber><xsl:value-of select="FLIGHT_NUM"/></FlightNumber>	
	<Date>(<xsl:value-of select="concat(string(DEP_DATE/DEP_DAY), string(DEP_DATE/DEP_MONTH))"/>)</Date>
	<DepartureAirportCode><xsl:value-of select="DEP_AIRPORT"/></DepartureAirportCode>
	<DepartureAirportName>(Airlines,<xsl:value-of select="DEP_AIRPORT"/>)</DepartureAirportName>
	<ArrivalAirportCode><xsl:value-of select="ARR_AIRPORT"/></ArrivalAirportCode>
	<ArrivalAirportName>>(Airlines,<xsl:value-of select="ARR_AIRPORT"/>)</ArrivalAirportName>
	<ClassOfService><xsl:value-of select="DEP_CLASS"/></ClassOfService>	
</xsl:template>

<xsl:template match="OSI_INFO">
	<OtherServiceInformation>   
		<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
	 	<AirlineCode><xsl:value-of select="substring(string(.),4,2)"/></AirlineCode> 
		<Text><xsl:value-of select="substring(string(.),6,15)"/></Text>
	</OtherServiceInformation>
</xsl:template>

<xsl:template match="SSR_TEXT" mode="SSRONE"> 
<xsl:variable name="Airl"><xsl:value-of select="substring(string(.),8,2)"/></xsl:variable>
<xsl:variable name="Seat"><xsl:value-of select="substring(string(.),4,4)"/></xsl:variable>
<xsl:variable name="FlightNo"><xsl:value-of select="substring(string(translate(string(.),' ','')),19,4)"/></xsl:variable>
<xsl:variable name="Name"><xsl:value-of select="substring-after(string(.),'-')"/></xsl:variable>
<xsl:variable name="Nametwo"><xsl:value-of select="substring-before(string($Name),' .')"/></xsl:variable>

<xsl:if test="$Seat!='SEAT'">
		<SpecialServiceRequest>
		<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
		<xsl:if test="$Seat!='SEAT' and $Seat!='FQTV'">
				<xsl:apply-templates select="//AIR_SEGMENT_INFO/AIR_ITEM[FLIGHT_NUM=$FlightNo]" mode="SegmentElementNumber"/>					

			<xsl:choose>
				<xsl:when test="translate ($Nametwo, ' ','') != ''">
					<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Nametwo]" mode="Association"/>					
				</xsl:when>
				<xsl:when test="translate ($Nametwo, ' ','') = ''">
					<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Name]" mode="Association"/>
				</xsl:when>				
			<xsl:otherwise>	
				<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM" mode="SEATTWO"/>
			</xsl:otherwise>
		</xsl:choose>
				<SSRCode><xsl:value-of select="substring(string(.),4,4)"/></SSRCode> 
				<AirlineCode><xsl:value-of select="$Airl"/></AirlineCode>
				<AirlineName>(Airlines,<xsl:value-of select="$Airl"/>)</AirlineName>		
				<Text><xsl:value-of select="substring(string(.),1,100)"/></Text> 
				</xsl:if>
			</SpecialServiceRequest>
			</xsl:if>
     </xsl:template>

<xsl:template match="AIR_ITEM" mode="SegmentElementNumber">
<SegmentElementNumber><xsl:value-of select="SEG_NUM"/></SegmentElementNumber>
</xsl:template>

<xsl:template match="SSR_TEXT" mode="SSRTWO">
<xsl:variable name="Seat"><xsl:value-of select="substring(string(.),4,4)"/></xsl:variable>
<xsl:variable name="Unconf"><xsl:value-of select="substring(string(.),10,2)"/></xsl:variable>
<xsl:variable name="FlightNo"><xsl:value-of select="substring(string(.),19,4)"/></xsl:variable>
<xsl:variable name="Airl"><xsl:value-of select="substring(string(.),8,2)"/></xsl:variable>
<xsl:variable name="Nametemp"><xsl:value-of select="substring-after(string(.),'-')"/></xsl:variable>
<xsl:variable name="Name"><xsl:value-of select="substring(string($Nametemp),3,99)"/></xsl:variable>
<xsl:variable name="Namethree"><xsl:value-of select="substring(string(.),36,100)"/></xsl:variable>
<xsl:variable name="Namefour"><xsl:value-of select="translate($Namethree,' ','')"/></xsl:variable>	
<xsl:variable name="SegNum"><xsl:value-of select="//AIR_ITEM/SEG_NUM"/></xsl:variable>

	<xsl:if test="$Seat='SEAT'">
	<xsl:if test="$Unconf='NN' or $Unconf='UC'">
	<SpecialServiceRequest>
		<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
		<xsl:apply-templates select="//AIR_SEGMENT_INFO/AIR_ITEM[FLIGHT_NUM=$FlightNo]" mode="SegmentElementNumber"/>	
<xsl:choose>
<xsl:when test="contains(string(.),'-')">
<Association>
				<TravelerElementNumber><xsl:value-of select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Name]/NAME_POSITION"/></TravelerElementNumber>
</Association>	
</xsl:when>
<xsl:otherwise>
<Association>
				<TravelerElementNumber><xsl:value-of select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Namefour]/NAME_POSITION"/></TravelerElementNumber>
</Association>	
</xsl:otherwise>	
</xsl:choose>	
			<SSRCode><xsl:value-of select="substring(string(.),4,4)"/></SSRCode> 
			<AirlineCode><xsl:value-of select="$Airl"/></AirlineCode>
			<AirlineName>(Airlines,<xsl:value-of select="$Airl"/>)</AirlineName>
			<Text><xsl:value-of select="substring(string(.),1,100)"/></Text> 		
		</SpecialServiceRequest>
	</xsl:if>
	</xsl:if>
</xsl:template>

	
<xsl:template match="SET_ITEM"> 
	<Seat>		
		<ElementNumber><xsl:value-of select="position()"/></ElementNumber>
		<SegmentElementNumber><xsl:value-of select="SEG_NUM"/></SegmentElementNumber>
		<SeatStatus>R</SeatStatus>
		<Assignment>
			<TravelerElementNumber><xsl:value-of select="PSG_NUM"/></TravelerElementNumber>
			<SeatLocation><xsl:value-of select="SEAT_DETAIL/SEAT_NUM"/></SeatLocation>
			<Characteristic><xsl:value-of select="SEAT_DETAIL/SEAT_TYPE"/></Characteristic>
		</Assignment>
	</Seat>		
</xsl:template>


<xsl:template match="PSG_ITEM" mode="Assignment">
	<TravelerElementNumber><xsl:value-of select="NAME_POSITION"/></TravelerElementNumber>
</xsl:template>

<xsl:template match="PSG_ITEM" mode="Association">
<Association>
	<TravelerElementNumber><xsl:value-of select="NAME_POSITION"/></TravelerElementNumber>
</Association>
</xsl:template>

<xsl:template match="PSG_ITEM" mode="SEATTWO">
<Association>
	<TravelerElementNumber><xsl:value-of select="NAME_POSITION"/></TravelerElementNumber>
</Association>
</xsl:template>

<xsl:template match="RMK_ITEM" mode="ITINRMKS">
	<xsl:variable name="Rmk" select="substring(string(RMK_TEXT),1,2)"/>
	<xsl:if test="$Rmk='RM'">
	<xsl:variable name="Type" select="substring(string(RMK_TEXT),4,3)"/>
	<ItineraryRemarks>
	<xsl:if test="$Type='AIR'">
		<AirRemark>
			<ElementNumber><xsl:value-of select="RMK_NUMBER"/></ElementNumber>
			<SegmentElementNumber><xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/></SegmentElementNumber>
			<Text><xsl:value-of select="RMK_TEXT"/></Text>
		</AirRemark>
		</xsl:if>
	<xsl:if test="$Type='CAR'">
		<CarRemark>
			<ElementNumber><xsl:value-of select="RMK_NUMBER"/></ElementNumber>
			<SegmentElementNumber><xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/></SegmentElementNumber>
			<Text><xsl:value-of select="RMK_TEXT"/></Text>
		</CarRemark>
		</xsl:if>
	<xsl:if test="$Type='HTL'">
		<HotelRemark>
			<ElementNumber><xsl:value-of select="RMK_NUMBER"/></ElementNumber>
			<SegmentElementNumber><xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/></SegmentElementNumber>
			<Text><xsl:value-of select="RMK_TEXT"/></Text>
		</HotelRemark>
		</xsl:if>
	</ItineraryRemarks>
	</xsl:if>
</xsl:template>

<xsl:template match="RMK_ITEM" mode="OTHER">
	<xsl:variable name="Rmk" select="substring(string(RMK_TEXT),1,1)"/>
	<xsl:variable name="Rmk2" select="substring(string(RMK_TEXT),1,2)"/>
	<xsl:variable name="Rmk3" select="substring(string(RMK_CATEGORY),1,2)"/>
	<xsl:if test="$Rmk!='/' and $Rmk2!='RM' and $Rmk3!='MT' and $Rmk3!='MN'">
		<GeneralRemark>
			<ElementNumber><xsl:value-of select="RMK_NUMBER"/></ElementNumber>
			<Text><xsl:value-of select="RMK_TEXT"/></Text>
		</GeneralRemark>
	</xsl:if>
</xsl:template>

<xsl:template match="RMK_ITEM" mode="TKTRMK">
	<xsl:variable name="Rmk3" select="substring(string(RMK_CATEGORY),1,2)"/>
	<xsl:if test="$Rmk3='MT'">
		<TktRmks>
			<ElemNo><xsl:value-of select="RMK_NUMBER"/></ElemNo>
			<Text><xsl:value-of select="RMK_TEXT"/></Text>
		</TktRmks>
	</xsl:if>
</xsl:template>

<xsl:template match="AIR_ITEM" mode="RLOC">
	<xsl:if test="DIRECT_RLOC!='' and not(preceding-sibling::AIR_ITEM/DIRECT_RLOC = current()/DIRECT_RLOC)"> 
		<VendorRecLocs>
			<VendorCode><xsl:value-of select="AIRLINE_CODE"/></VendorCode>
			<RecLoc><xsl:value-of select="DIRECT_RLOC"/></RecLoc>
		</VendorRecLocs>
	</xsl:if>
</xsl:template>

<xsl:template match="SSR_ITM" mode="SSR">
	<SpecialServiceRequest>
		<xsl:attribute name="SSRCode">
			<xsl:value-of select="SSR_COD"/>
		</xsl:attribute>	
		<Airline>
			<xsl:variable name="rph"><xsl:value-of select="SEG_NUM"/></xsl:variable>
			<xsl:attribute name="Code">
				<xsl:value-of select="../../AIR_SEG_INF/AIR_ITM[SEG_NUM = $rph]/ARL_COD"/>
			</xsl:attribute>
		</Airline>
		<Text>
			<xsl:value-of select="SSR_TXT"/>
		</Text>
	</SpecialServiceRequest>		
</xsl:template>

<xsl:template name="strip-zeroes">
	<xsl:param name="in"/>
	<xsl:choose>
		<xsl:when test="substring($in, 1, 1) = '0' and string-length($in) > 1">
			<xsl:call-template name="strip-zeroes">
				<xsl:with-param name="in" select="substring($in, 2, string-length($in) - 1)"/>
			</xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$in"/>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="month">
		<xsl:param name="month" />
		<xsl:choose>
			<xsl:when test="$month = 'JAN'">01</xsl:when>
			<xsl:when test="$month = 'FEB'">02</xsl:when>
			<xsl:when test="$month = 'MAR'">03</xsl:when>
			<xsl:when test="$month = 'APR'">04</xsl:when>
			<xsl:when test="$month = 'MAY'">05</xsl:when>
			<xsl:when test="$month = 'JUN'">06</xsl:when>
			<xsl:when test="$month = 'JUL'">07</xsl:when>
			<xsl:when test="$month = 'AUG'">08</xsl:when>
			<xsl:when test="$month = 'SEP'">09</xsl:when>
			<xsl:when test="$month = 'OCT'">10</xsl:when>
			<xsl:when test="$month = 'NOV'">11</xsl:when>
			<xsl:when test="$month = 'DEC'">12</xsl:when>
		</xsl:choose>
</xsl:template>

</xsl:stylesheet>