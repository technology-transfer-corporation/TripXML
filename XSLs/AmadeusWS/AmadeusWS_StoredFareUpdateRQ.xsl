<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_UpdateTSTRQ.xsl													  -->
<!-- ================================================================== -->
<!-- Date: 04 Dec 2012 - Rastko - added TST number in cryptic entry			    	-->
<!-- Date: 05 Nov 2012 - Rastko - send cryptic entry for OneTwoTrip			    	-->
<!-- Date: 20 Nov 2010	- Rastko														  -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="username" select="OTA_StoredFareUpdateRQ/POS/TPA_Extensions/Provider/Userid"/>
	<xsl:variable name="tstRPH"><xsl:value-of select="OTA_StoredFareUpdateRQ/Fare/@RPH"/></xsl:variable>
	<xsl:variable name="cnx">
		<xsl:variable name="cnx1">
			<xsl:for-each select="OTA_StoredFareUpdateRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption">
				<xsl:for-each select="FlightSegment">
					<xsl:choose>
						<xsl:when test="position()=1">O</xsl:when>
						<xsl:otherwise>X</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:for-each select="OTA_StoredFareUpdateRQ/Segments">
			<xsl:choose>
				<xsl:when test="@RPH!='0'">
					<xsl:variable name="cnx2"><xsl:value-of select="@RPH"/></xsl:variable>
					<xsl:value-of select="substring($cnx1,$cnx2,1)"/>
				</xsl:when>
				<xsl:otherwise>O</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:variable>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="$username='OneTwoTrip'">
				<Command_Cryptic>
					<xsl:apply-templates select="OTA_StoredFareUpdateRQ" mode="cryptic"/>
				</Command_Cryptic>
			</xsl:when>
			<xsl:otherwise>
				<Ticket_UpdateTST>
					<xsl:apply-templates select="OTA_StoredFareUpdateRQ" mode="sfu"/>
				</Ticket_UpdateTST>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="OTA_StoredFareUpdateRQ" mode="cryptic">
		<messageAction>
			<messageFunctionDetails>
				<messageFunction>M</messageFunction>
			</messageFunctionDetails>
		</messageAction>
		<longTextString>
			<textStringDetails>
				<xsl:value-of select="'TTK/'" />
				<xsl:if test="Fare/@RPH!=''">
					<xsl:value-of select="concat('T',Fare/@RPH,'/')"/>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="Fare/EquivFare/@Amount!=''">
						<xsl:value-of select="'I'"/>
						<xsl:value-of select="Fare/EquivFare/@CurrencyCode"/>
						<xsl:choose>
							<xsl:when test="Fare/EquivFare/@DecimalPlaces='2'">
								<xsl:value-of select="substring(Fare/EquivFare/@Amount,1,string-length(Fare/EquivFare/@Amount) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring(Fare/EquivFare/@Amount,string-length(Fare/EquivFare/@Amount) - 1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Fare/EquivFare/@Amount"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="'/E'"/>
						<xsl:value-of select="Fare/BaseFare/@CurrencyCode"/>
						<xsl:choose>
							<xsl:when test="Fare/BaseFare/@DecimalPlaces='2'">
								<xsl:value-of select="substring(Fare/BaseFare/@Amount,1,string-length(Fare/BaseFare/@Amount) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring(Fare/BaseFare/@Amount,string-length(Fare/BaseFare/@Amount) - 1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Fare/BaseFare/@Amount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'U'"/>
						<xsl:choose>
							<xsl:when test="Fare/BaseFare/@DecimalPlaces='2'">
								<xsl:value-of select="substring(Fare/BaseFare/@Amount,1,string-length(Fare/BaseFare/@Amount) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring(Fare/BaseFare/@Amount,string-length(Fare/BaseFare/@Amount) - 1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Fare/BaseFare/@Amount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</textStringDetails>
		</longTextString>
	</xsl:template>
	
	<xsl:template match="OTA_StoredFareUpdateRQ" mode="sfu">
		<xsl:apply-templates select="@SalesIndicator"/>
		<fareReference>
			<referenceType>TST</referenceType>
			<uniqueReference>
				<xsl:value-of select="Fare/@RPH"/>
			</uniqueReference>
			<xsl:variable name="tst"><xsl:value-of select="Fare/@RPH"/></xsl:variable>
			<iDDescription>
				<iDSequenceNumber>
					<xsl:value-of select="Ticket_DisplayTSTReply/fareList[fareReference/uniqueReference=$tst]/fareReference/iDDescription/iDSequenceNumber"/>
				</iDSequenceNumber>
			</iDDescription>
		</fareReference>
		<xsl:apply-templates select="Fare"/>
		<xsl:if test="BankerRates">
			<BankerRates>
				<xsl:apply-templates select="BankerRates[Price/@PriceTypeCode='FirstRate']"/>
				<xsl:apply-templates select="BankerRates[Price/@PriceTypeCode='SecondRate']"/>
			</BankerRates>
		</xsl:if>
		<xsl:if test="Segments">
			<xsl:variable name="depRPH"><xsl:value-of select="Segments[1]/@RPH"/></xsl:variable>
			<xsl:variable name="arrRPH"><xsl:value-of select="Segments[position()=last()]/@RPH"/></xsl:variable>
			<originDestination>
				<cityCode>
					<xsl:value-of select="PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo[position()=$depRPH]/travelProduct/boardpointDetail/cityCode"/>
				</cityCode>
				<cityCode>
					<xsl:value-of select="PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo[position()=$arrRPH]/travelProduct/offpointDetail/cityCode"/>
				</cityCode>
			</originDestination>
		</xsl:if>
		<xsl:apply-templates select="Segments"/>
		<xsl:if test="FareCalcLine">
			<otherPricingInfo>
				<xsl:apply-templates select="FareCalcLine"/>
			</otherPricingInfo>
		</xsl:if>
		<xsl:apply-templates select="PassengerType"/>
		<xsl:apply-templates select="AirlineFees"/>
		<xsl:apply-templates select="Mileage"/>
	</xsl:template>
	
	<!-- *********************************************************************************************************  -->
	
	<xsl:template match="@SalesIndicator">
		<pricingInformation>
			<salesIndicator>
				<xsl:value-of select="."/>
			</salesIndicator>
		</pricingInformation>
	</xsl:template>
	
	<xsl:template match="Fare">
		<xsl:if test="BaseFare">
			<fareDataInformation>
				<fareDataMainInformation>
					<fareDataQualifier>
						<xsl:choose>
							<xsl:when test="@FareIdentifier!=''">
								<xsl:value-of select="@FareIdentifier"/>
							</xsl:when>
							<xsl:otherwise>F</xsl:otherwise>
						</xsl:choose>
					</fareDataQualifier>
				</fareDataMainInformation>
				<xsl:variable name="dec"><xsl:value-of select="BaseFare/@DecimalPlaces"/></xsl:variable>
				<xsl:variable name="basefare">
					<xsl:choose>
						<xsl:when test="$dec='0'">
							<xsl:value-of select="BaseFare/@Amount"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(BaseFare/@Amount,1,string-length(BaseFare/@Amount) - $dec)"/>
							<xsl:value-of select="substring(BaseFare/@Amount,string-length(BaseFare/@Amount) - ($dec - 1),2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<fareDataSupInformation>
					<fareDataQualifier>B</fareDataQualifier>
					<fareAmount>
						<xsl:choose>
							<xsl:when test="$dec='0'">
								<xsl:value-of select="$basefare"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring($basefare,1,string-length($basefare) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring($basefare,string-length($basefare) - 1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</fareAmount>
					<xsl:if test="BaseFare/@CurrencyCode!=''">
						<fareCurrency>
							<xsl:value-of select="BaseFare/@CurrencyCode"/>
						</fareCurrency>
					</xsl:if>
					<xsl:if test="@Location!=''">
						<fareLocation>
							<xsl:value-of select="@Location"/>
						</fareLocation>
					</xsl:if>
				</fareDataSupInformation>
				<fareDataSupInformation>
					<fareDataQualifier>712</fareDataQualifier>
					<fareAmount>
						<xsl:variable name="taxes"><xsl:value-of select="sum(Taxes/Tax/@Amount)"/></xsl:variable>
						<xsl:variable name="total"><xsl:value-of select="$taxes + $basefare"/></xsl:variable>
						<xsl:choose>
							<xsl:when test="$dec='0'">
								<xsl:value-of select="$total"/>
							</xsl:when>	
							<xsl:otherwise>
								<xsl:value-of select="substring($total,1,string-length($total) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring($total,string-length($total) - 1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</fareAmount>
					<fareCurrency><xsl:value-of select="BaseFare/@CurrencyCode"/></fareCurrency>
				</fareDataSupInformation>
			</fareDataInformation>
		</xsl:if>
		<xsl:apply-templates select="Taxes/Tax"/>
	</xsl:template>
	
	<xsl:template match="Tax">
		<taxInformation>
			<taxDetails>
				<taxQualifier>7</taxQualifier>
				<taxIdentification>
					<taxIdentifier>X</taxIdentifier>
				</taxIdentification>
				<taxType>
					<isoCountry>
						<xsl:value-of select="@TaxCode"/>
					</isoCountry>
				</taxType>
				<xsl:if test="@Nature!=''">
					<taxNature>
						<xsl:value-of select="@Nature"/>
					</taxNature>
				</xsl:if>
				<xsl:if test="@ExemptIndicator!=''">
					<taxExempt>
						<xsl:value-of select="@ExemptIndicator"/>
					</taxExempt>
				</xsl:if>
			</taxDetails>
			<amountDetails>
				<fareDataMainInformation>
					<fareDataQualifier>TAX</fareDataQualifier>
					<fareAmount>
						<xsl:choose>
							<xsl:when test="contains(@Amount,'.')">
								<xsl:value-of select="substring(@Amount,1,string-length(@Amount) - 3)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring(@Amount,string-length(@Amount) - 2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(@Amount,1,string-length(@Amount) - 2)"/>
								<xsl:text>.</xsl:text>
								<xsl:value-of select="substring(@Amount,string-length(@Amount) - 1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</fareAmount>
					<fareCurrency><xsl:value-of select="@CurrencyCode"/></fareCurrency>
				</fareDataMainInformation>
			</amountDetails>
		</taxInformation>
	</xsl:template>
	
	<xsl:template match="BankerRates[Price/@PriceTypeCode='FirstRate']">
		<firstRateDetail>
			<amount>
				<xsl:value-of select="Price/@Amount"/>
			</amount>
		</firstRateDetail>
	</xsl:template>
	
	<xsl:template match="BankerRates[Price/@PriceTypeCode='SecondRate']">
		<secondRateDetail>
			<currencyCode>
				<xsl:value-of select="Currency/@CurrencyCode"/>
			</currencyCode>
			<amount>
				<xsl:value-of select="Price/@Amount"/>
			</amount>
		</secondRateDetail>
	</xsl:template>
	
	<xsl:template match="Segments">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<segmentInformation>
			<xsl:choose>
				<xsl:when test="@RPH='0'">
					<connexInformation>
						<connecDetails>
							<routingInformation>ARNK</routingInformation>
							<connexType>O</connexType>
						</connecDetails>
					</connexInformation>
					<sequenceInformation>
						<sequenceSection>
							<sequenceNumber><xsl:value-of select="$pos"/></sequenceNumber>
						</sequenceSection>
					</sequenceInformation>
				</xsl:when>
				<xsl:otherwise>
					<connexInformation>
						<connecDetails>
							<connexType>
								<!--xsl:value-of select="substring($cnx,$pos,1)"/-->
								<xsl:choose>
						<xsl:when test="$pos=1">O</xsl:when>
						<xsl:otherwise>X</xsl:otherwise>
					</xsl:choose>
							</connexType>
						</connecDetails>
					</connexInformation>
					<xsl:apply-templates select="FareBasis"/>
					<xsl:apply-templates select="FareValidity"/>
					<xsl:apply-templates select="BagAllowance"/>
					<sequenceInformation>
						<sequenceSection>
							<sequenceNumber>
								<xsl:value-of select="$pos"/>
							</sequenceNumber>
						</sequenceSection>
					</sequenceInformation>
				</xsl:otherwise>
			</xsl:choose>
		</segmentInformation>
	</xsl:template>
	
	<xsl:template match="FareBasis">
		<fareQualifier>
			<xsl:if test="@MovementType!=''">
				<movementType>
					<xsl:value-of select="@MovementType"/>
				</movementType>
			</xsl:if>
			<fareBasisDetails>
				<xsl:choose>
					<xsl:when test="string-length(@FareBasisCode) > 9">
						<primaryCode>
							<xsl:value-of select="substring(@FareBasisCode,1,3)"/>
						</primaryCode>
						<fareBasisCode>
							<xsl:value-of select="substring(@FareBasisCode,4,6)"/>
						</fareBasisCode>
						<ticketDesignator>
							<xsl:value-of select="substring(@FareBasisCode,10)"/>
						</ticketDesignator>
					</xsl:when>
					<xsl:when test="string-length(@FareBasisCode) > 6">
						<primaryCode>
							<xsl:value-of select="substring(@FareBasisCode,1,3)"/>
						</primaryCode>
						<fareBasisCode>
							<xsl:value-of select="substring(@FareBasisCode,4)"/>
						</fareBasisCode>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="@PrimaryCode!=''">
							<primaryCode>
								<xsl:value-of select="@PrimaryCode"/>
							</primaryCode>
						</xsl:if>
						<fareBasisCode>
							<xsl:value-of select="@FareBasisCode"/>
						</fareBasisCode>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="@TicketDesignator!=''">
					<ticketDesignator>
						<xsl:value-of select="@TicketDesignator"/>
					</ticketDesignator>
				</xsl:if>
				<xsl:if test="@DiscountTicketDesignator!=''">
					<discTktDesignator>
						<xsl:value-of select="@DiscountTicketDesignator"/>
					</discTktDesignator>
				</xsl:if>
			</fareBasisDetails>
			<xsl:if test="@DiscountType!=''">
				<zapOffDetails>
					<zapOffType>
						<xsl:value-of select="@DiscountType"/>
					</zapOffType>
					<zapOffAmount>
						<xsl:value-of select="@DiscountAmount"/>
					</zapOffAmount>
					<zapOffPercentage>
						<xsl:value-of select="@DiscountPecentage"/>
					</zapOffPercentage>
				</zapOffDetails>
			</xsl:if>
		</fareQualifier>
	</xsl:template>
	
	<xsl:template match="FareValidity">
		<validityInformation>
			<businessSemantic>
				<xsl:value-of select="substring(@ValidityReason,1,1)"/>
			</businessSemantic>
			<dateTime>
				<year>
					<xsl:value-of select="substring(@ValidityDate,1,4)"/>
				</year>
				<month>
					<xsl:value-of select="substring(@ValidityDate,6,2)"/>
				</month>
				<day>
					<xsl:value-of select="substring(@ValidityDate,9,2)"/>
				</day>
			</dateTime>
		</validityInformation>
	</xsl:template>
	
	<xsl:template match="BagAllowance">
		<bagAllowanceInformation>
			<bagAllowanceDetails>
				<baggageQuantity>
					<xsl:value-of select="@Quantity"/>
				</baggageQuantity>
				<xsl:if test="@Weight!=''">
					<baggageWeight>
						<xsl:value-of select="@Weight"/>
					</baggageWeight>
				</xsl:if>
				<baggageType>
					<xsl:choose>
						<xsl:when test="@Type='Piece'">N</xsl:when>
						<xsl:otherwise>W</xsl:otherwise>
					</xsl:choose>
				</baggageType>
				<xsl:if test="@Unit!=''">
					<measureUnit>
						<xsl:value-of select="@Unit"/>
					</measureUnit>
				</xsl:if>
			</bagAllowanceDetails>
		</bagAllowanceInformation>
	</xsl:template>
	
	<xsl:template match="FareCalcLine">
		<attributeDetails>
			<attributeType>FCA</attributeType>
			<attributeDescription>
				<xsl:value-of select="."/>
			</attributeDescription>
		</attributeDetails>
	</xsl:template>

	<xsl:template match="PassengerType">
		<statusInformation>
			<firstStatusDetails>
				<tstFlag>
					<xsl:value-of select="@Code"/>
				</tstFlag>
			</firstStatusDetails>
		</statusInformation>
	</xsl:template>
		
	<xsl:template match="AirlineFees">
		<carrierFeesGroup>
			<carrierFeeType>
				<selectionDetails>
					<option>
						<xsl:value-of select="@FeeType"/>
					</option>
				</selectionDetails>
			</carrierFeeType>
			<xsl:apply-templates select="FeeInformation"/>
		</carrierFeesGroup>
	</xsl:template>

	<xsl:template match="FeeInformation">
		<carrierFeesInfo>
			<carrierFeeSubcode>
				<dataTypeInformation>
					<type>
						<xsl:value-of select="FeeProperties/@Type"/>
					</type>
				</dataTypeInformation>
				<xsl:apply-templates select="FeeApplication"/>				
			</carrierFeeSubcode>
			<commercialName>
				<freeTextQualification>
					<textSubjectQualifier>
						<xsl:value-of select="FeeName/@Qualifier"/>
					</textSubjectQualifier>
				</freeTextQualification>
				<freeText>
					<xsl:value-of select="FeeName/@Name"/>
				</freeText>
			</commercialName>
			<feeAmount>
				<monetaryDetails>
					<typeQualifier>
						<xsl:value-of select="Amount/@Qualifier"/>
					</typeQualifier>
					<amount>
						<xsl:value-of select="Amount/Price/@Amount"/>	
					</amount>
					<currency>
						<xsl:value-of select="Amount/Currency/@CurrencyCode"/>
					</currency>
				</monetaryDetails>
			</feeAmount>
			<xsl:apply-templates select="Taxes"/>
		</carrierFeesInfo>
	</xsl:template>

	<xsl:template match="FeeApplication">
		<dataInformation>
			<indicator>
				<xsl:value-of select="@Code"/>
			</indicator>
		</dataInformation>
	</xsl:template>
	
	<xsl:template match="Taxes">
		<feeTax>
			<taxCategory>
				<xsl:value-of select="@TaxCategory"/>
			</taxCategory>
			<feeTaxDetails>
				<rate>
					<xsl:value-of select="Details/@Rate"/>
				</rate>
				<currencyCode>
					<xsl:value-of select="Details/@CurrencyCode"/>
				</currencyCode>
				<type>
					<xsl:value-of select="Details/@Type"/>
				</type>
			</feeTaxDetails> 
		</feeTax>
	</xsl:template>

	<xsl:template match="Mileage">
		<mileage>
			<mileageTimeDetails>
				<totalMileage>
					<xsl:value-of select="."/>
				</totalMileage>
			</mileageTimeDetails>
		</mileage>
	</xsl:template>


		
</xsl:stylesheet>
