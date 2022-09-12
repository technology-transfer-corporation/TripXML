<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- 
	================================================================== 
	Sabre_LowFareRS.xsl 																
	================================================================== 
	Date: 24 Aug 2022 - Kobelev - Fixed JourneyDuration Display
	Date: 20 Aug 2022 - Kobelev - Better Error Handler
	Date: 28 Aug 2011 - Rastko - corrected mapping of validating carrier
	Date: 18 Feb 2011 - Rastko - added code to get operating airline in response		
	Date: 15 Dec 2010 - Rastko - corrected display of fares						
	Date: 04 Dec 2010 - Rastko - added calculation of JourneyTotalDuration			
	Date: 31 Aug 2010 - Rastko - corrected marriage group element				
	Date: 16 Nov 2008 - Rastko													
	================================================================== 
	-->

	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="tis" select="OTA_AirLowFareSearchRS/TravelerInfoSummary"/>

	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_AirLowFareSearchRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<xsl:attribute name="TransactionIdentifier">Sabre</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_AirLowFareSearchRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchRS">
		<OTA_AirLowFareSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Sabre</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:choose>
								<xsl:when test="Errors/Error[@Type='ERR']">
									<xsl:value-of select="Errors/Error[@Type='ERR']/@ShortText"/>
								</xsl:when>
								<xsl:when test="Errors/Error[@Type='SERVER']">
									<xsl:value-of select="Errors/Error[@Type='SERVER']"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Errors/Error" />
								</xsl:otherwise>
							</xsl:choose>

						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="not(PricedItineraries/PricedItinerary) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="@SequenceNumber" />
			</xsl:attribute>
			<AirItinerary>
				<xsl:attribute name="DirectionInd">
					<xsl:choose>
						<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestination[1]/FlightSegment[1]/DepartureAirport/@LocationCode = AirItinerary/OriginDestinationOptions/OriginDestination[position()=last()]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">Circle</xsl:when>
						<xsl:otherwise>OneWay</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<OriginDestinationOptions>
					<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption" />
				</OriginDestinationOptions>
			</AirItinerary>
			<xsl:apply-templates select="AirItineraryPricingInfo" />
		</PricedItinerary>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Total Fare and PTC_FareBreakdown 				    -->
	<!-- ************************************************************** -->
	<xsl:template match="AirItineraryPricingInfo">
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="@PricingSource">
						<xsl:value-of select="@PricingSource" />
					</xsl:when>
					<xsl:when test="PTC_FareInfo/PTC_FareBreakdown/PassengerFare/TPA_Extensions/Text[contains(.,'PRIVATE FARE APPLIED')]">
						<xsl:text>Private</xsl:text>
					</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ValidatingAirlineCode">
				<xsl:choose>
					<xsl:when test="PTC_FareInfo/PTC_FareBreakdown[1]/PassengerFare/TPA_Extensions/ValidatingCarrier/@Code!=''">
						<xsl:value-of select="PTC_FareInfo/PTC_FareBreakdown[1]/PassengerFare/TPA_Extensions/ValidatingCarrier/@Code"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-after(PTC_FareInfo/PTC_FareBreakdown/PassengerFare/TPA_Extensions/Text[contains(.,'VALIDATING CARRIER - ')],' - ')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<ItinTotalFare>
				<xsl:variable name="amtbase1">
					<xsl:apply-templates select="PTC_FareInfo/PTC_FareBreakdown[1]" mode="basefare">
						<xsl:with-param name="total">0</xsl:with-param>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="amtbase">
					<xsl:value-of select="substring-before($amtbase1,'/')" />
				</xsl:variable>
				<xsl:variable name="amttot">
					<xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" />
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amtbase" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="ItinTotalFare/TotalFare/@DecimalPlaces" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$amttot - $amtbase"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="ItinTotalFare/TotalFare/@DecimalPlaces" />
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amttot" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="ItinTotalFare/TotalFare/@DecimalPlaces" />
					</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FareInfo/PTC_FareBreakdown" mode="PaxType" />
			</PTC_FareBreakdowns>
			<FareInfos>
				<xsl:for-each select="PTC_FareInfo/PTC_FareBreakdown">
					<xsl:variable name="fareref" select="FareBasisCode"/>
					<xsl:apply-templates select="../../../AirItinerary/OriginDestinationOptions/OriginDestination/FlightSegment" mode="fareinfos">
						<xsl:with-param name="fareref">
							<xsl:copy-of select="$fareref" />
						</xsl:with-param>
					</xsl:apply-templates>
				</xsl:for-each>
			</FareInfos>
		</AirItineraryPricingInfo>
		<xsl:variable name="ttl1">
			<xsl:value-of select="substring-after(PTC_FareInfo/PTC_FareBreakdown[1]/PassengerFare/TPA_Extensions/Text[contains(.,'PURCHASE')],'PURCHASE ')"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$ttl1 != ''">
				<xsl:variable name="ttl">
					<xsl:value-of select="translate($ttl1,' ','0')"/>
				</xsl:variable>
				<TicketingInfo>
					<xsl:attribute name="TicketTimeLimit">
						<xsl:variable name="mm">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($ttl,3,3)" />
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="depdate">
							<xsl:value-of select="../AirItinerary/OriginDestinationOptions/OriginDestination[1]/FlightSegment[1]/@DepartureDateTime"/>
						</xsl:variable>
						<xsl:variable name="depmm">
							<xsl:value-of select="substring($depdate,6,2)"/>
						</xsl:variable>
						<xsl:variable name="depyyyy">
							<xsl:value-of select="substring($depdate,1,4)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$depmm &lt; $mm">
								<xsl:value-of select="$depyyyy - 1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$depyyyy"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="$mm"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring($ttl,1,2)"/>
						<xsl:text>T23:59:00</xsl:text>
					</xsl:attribute>
				</TicketingInfo>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="depdate">
					<xsl:value-of select="../AirItinerary/OriginDestinationOptions/OriginDestination[1]/FlightSegment[1]/@DepartureDateTime"/>
				</xsl:variable>
				<TicketingInfo>
					<xsl:attribute name="TicketTimeLimit">
						<xsl:value-of select="substring($depdate,1,10)"/>
						<xsl:text>T00:00:00</xsl:text>
					</xsl:attribute>
				</TicketingInfo>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ******************************************************************** -->
	<xsl:template match="Tax">
		<Tax>
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="@TaxCode" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(@Amount,'.','')" />
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Fare Breakdown per pax type					    -->
	<!-- ************************************************************** -->
	<xsl:template match="PTC_FareBreakdown" mode="PaxType">
		<xsl:variable name="PaxTypeQuantity">
			<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
		</xsl:variable>
		<PTC_FareBreakdown>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="@PricingSource">
						<xsl:value-of select="@PricingSource" />
					</xsl:when>
					<xsl:when test="PassengerFare/TPA_Extensions/Text[contains(.,'PRIVATE FARE APPLIED')]">
						<xsl:text>Private</xsl:text>
					</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="PassengerTypeQuantity/@Code" />
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:choose>
					<xsl:when test="contains(FareBasisCode,'*')">
						<xsl:for-each select="FareBasisCode">
							<xsl:call-template name="parsefb">
								<xsl:with-param name="fbc">
									<xsl:value-of select="." />
								</xsl:with-param>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="FareBasisCode" mode="PaxFareBasis" />
					</xsl:otherwise>
				</xsl:choose>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:variable name="tbase">
					<xsl:choose>
						<xsl:when test="PassengerFare/EquivFare/@Amount!=''">
							<xsl:value-of select="translate(PassengerFare/EquivFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="ttot">
					<xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$tbase" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$ttot - $tbase"/>
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$ttot" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<PricedCode>
					<xsl:value-of select="PassengerFare/TPA_Extensions/FarePassengerType/@Code" />
				</PricedCode>
				<xsl:if test="PassengerFare/TPA_Extensions/WarningInfo[contains(.,'NOT APPLICABLE')]">
					<Text>
						<xsl:value-of select="PassengerFare/TPA_Extensions/WarningInfo[contains(.,'NOT APPLICABLE')]"/>
					</Text>
				</xsl:if>
			</TPA_Extensions>
		</PTC_FareBreakdown>
	</xsl:template>

	<xsl:template name="parsefb">
		<xsl:param name="fbc" />
		<xsl:choose>
			<xsl:when test="contains($fbc,'*')">
				<xsl:variable name="item">
					<xsl:value-of select="substring-before($fbc,'*')" />
				</xsl:variable>
				<FareBasisCode>
					<xsl:value-of select="$item" />
				</FareBasisCode>
				<xsl:call-template name="parsefb">
					<xsl:with-param name="fbc">
						<xsl:value-of select="substring-after($fbc,'*')" />
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<FareBasisCode>
					<xsl:value-of select="$fbc" />
				</FareBasisCode>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="FareBasisCode" mode="PaxFareBasis">
		<FareBasisCode>
			<xsl:value-of select="." />
		</FareBasisCode>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--Fare Rule Info section							    -->
	<!-- ************************************************************** -->
	<xsl:template match="FlightSegment" mode="fareinfos">
		<xsl:param name="fareref" />
		<xsl:variable name="pos">
			<xsl:value-of select="@RPH"/>
		</xsl:variable>
		<FareInfo>
			<DepartureDate>
				<xsl:value-of select="@DepartureDateTime" />
			</DepartureDate>
			<FareReference>
				<xsl:choose>
					<xsl:when test="msxsl:node-set($fareref)/FareBasisCode[position() = $pos]  = ''">NODATA</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="msxsl:node-set($fareref)/FareBasisCode[position() = $pos]" />
					</xsl:otherwise>
				</xsl:choose>
			</FareReference>
			<RuleInfo />
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code" />
				</xsl:attribute>
			</FilingAirline>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DepartureAirport/@LocationCode" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ArrivalAirport/@LocationCode" />
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
	<!-- 
******************************************************************** -->
	<!--  OriginDestination section						    -->
	<!-- 
******************************************************************** -->
	<xsl:template match="OriginDestinationOption">
		<OriginDestinationOption>
			<xsl:variable name="tjd1">
				<xsl:apply-templates select="FlightSegment[1]" mode="calcTJD">
					<xsl:with-param name="tjd">
						<xsl:value-of select="'00.00'"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:apply-templates select="FlightSegment" mode="od">
				<xsl:with-param name="tjd" select="$tjd1"/>
			</xsl:apply-templates>
		</OriginDestinationOption>
	</xsl:template>
	<!-- -->
	<xsl:template match="FlightSegment" mode="calcTJD">
		<xsl:param name="tjd"/>
		<xsl:choose>
			<xsl:when test="following-sibling::FlightSegment[1]">

				<xsl:variable name="elpsTm" select="concat(floor(@ElapsedTime div 60),'.', floor(@ElapsedTime mod 60))"/>				
				<xsl:variable name="hrs1">
					<xsl:value-of select="substring-before($elpsTm,'.')"/>					
				</xsl:variable>
				<xsl:variable name="min1">
					<xsl:value-of select="substring-after($elpsTm,'.')"/>
				</xsl:variable>

				<xsl:variable name="fltElpsTm" select="concat(floor(following-sibling::FlightSegment[1]/@ElapsedTime div 60),'.', floor(following-sibling::FlightSegment[1]/@ElapsedTime mod 60))"/>
				<xsl:variable name="hrs2">
					<xsl:value-of select="substring-before($fltElpsTm,'.')"/>
				</xsl:variable>
				<xsl:variable name="min2">
					<xsl:value-of select="substring-after($fltElpsTm,'.')"/>
				</xsl:variable>
				
				<xsl:variable name="arday">
					<xsl:value-of select="substring(@ArrivalDateTime,9,2)"/>
				</xsl:variable>
				<xsl:variable name="arhrs">
					<xsl:value-of select="substring(@ArrivalDateTime,12,2)"/>
				</xsl:variable>
				<xsl:variable name="armin">
					<xsl:value-of select="substring(@ArrivalDateTime,15,2)"/>
				</xsl:variable>
				<xsl:variable name="deday">
					<xsl:value-of select="substring(following-sibling::FlightSegment[1]/@DepartureDateTime,9,2)"/>
				</xsl:variable>
				<xsl:variable name="dehrs">
					<xsl:value-of select="substring(following-sibling::FlightSegment[1]/@DepartureDateTime,12,2)"/>
				</xsl:variable>
				<xsl:variable name="demin">
					<xsl:value-of select="substring(following-sibling::FlightSegment[1]/@DepartureDateTime,15,2)"/>
				</xsl:variable>
				<xsl:variable name="mins">
					<xsl:choose>
						<xsl:when test="$armin &gt; $demin">
							<xsl:value-of select="60 + $demin - $armin"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$demin - $armin"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="hrs">
					<xsl:variable name="chhrs">
						<xsl:choose>
							<xsl:when test="$deday != $arday">24</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$armin &gt; $demin">
							<xsl:value-of select="($chhrs + $dehrs) - $arhrs - 1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="($chhrs + $dehrs) - $arhrs"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="summin">
					<xsl:value-of select="$mins + $min1 + $min2 + substring-after($tjd,'.')"/>
				</xsl:variable>
				<xsl:variable name="totmins">
					<xsl:choose>
						<xsl:when test="$summin &gt; 59">
							<xsl:value-of select="format-number($summin mod 60,'00')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="format-number($summin,'00')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="tothrs">
					<xsl:choose>
						<xsl:when test="$summin &gt; 59">
							<xsl:variable name="hrssum">
								<xsl:variable name="a">
									<xsl:value-of select="$summin mod 60"/>
								</xsl:variable>
								<xsl:variable name="b">
									<xsl:value-of select="$summin - $a"/>
								</xsl:variable>
								<xsl:value-of select="$b div 60"/>
							</xsl:variable>
							<xsl:value-of select="$hrssum + $hrs + $hrs1 + $hrs2 + substring-before($tjd,'.')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$hrs + $hrs1 + $hrs2 + substring-before($tjd,'.')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:apply-templates select="following-sibling::FlightSegment[1]" mode="calcTJD">
					<xsl:with-param name="tjd" select="concat($tothrs,'.',$totmins)"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$tjd='00.00'">
						<xsl:value-of select="@ElapsedTime"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$tjd"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ************************************************************** -->
	<!--  		          Segment Data                           		    -->
	<!-- *************************************************************** -->
	<xsl:template match="FlightSegment" mode="od">
		<xsl:param name="tjd"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">
				<xsl:value-of select="@DepartureDateTime" />
			</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">
				<xsl:value-of select="@ArrivalDateTime"/>
			</xsl:attribute>
			<xsl:attribute name="StopQuantity">
				<xsl:value-of select="@StopQuantity" />
			</xsl:attribute>
			<xsl:attribute name="RPH">1</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="@FlightNumber" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="@ResBookDesigCode" />
			</xsl:attribute>
			<xsl:attribute name="NumberInParty">
				<xsl:value-of select="$tis/SeatsRequested" />
			</xsl:attribute>
			<xsl:attribute name="E_TicketEligibility">
				<xsl:choose>
					<xsl:when test="TPA_Extensions/eTicket/@Ind = 'true'">Eligible</xsl:when>
					<xsl:otherwise>NotEligible</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DepartureAirport/@LocationCode" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ArrivalAirport/@LocationCode" />
				</xsl:attribute>
			</ArrivalAirport>
			<xsl:choose>
				<xsl:when test="OperatingAirline/@Code !='' and OperatingAirline/@Code !='**'">
					<OperatingAirline>
						<xsl:attribute name="Code">
							<xsl:value-of select="OperatingAirline/@Code" />
						</xsl:attribute>
					</OperatingAirline>
				</xsl:when>
				<xsl:when test="contains(TPA_Extensions/Text,'OPERATED BY /')">
					<OperatingAirline>
						<xsl:attribute name="Code"/>
						<xsl:value-of select="substring-after(TPA_Extensions/Text,'OPERATED BY /')" />
					</OperatingAirline>
				</xsl:when>
				<xsl:when test="contains(TPA_Extensions/Text,'OPERATED BY ')">
					<OperatingAirline>
						<xsl:attribute name="Code"/>
						<xsl:value-of select="substring-after(TPA_Extensions/Text,'OPERATED BY ')" />
					</OperatingAirline>
				</xsl:when>
				<xsl:otherwise>
					<OperatingAirline>
						<xsl:attribute name="Code">
							<xsl:value-of select="MarketingAirline/@Code" />
						</xsl:attribute>
					</OperatingAirline>
				</xsl:otherwise>
			</xsl:choose>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="Equipment/@AirEquipType" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code" />
				</xsl:attribute>
			</MarketingAirline>
			<MarriageGrp>
				<xsl:choose>
					<xsl:when test="MarriageGrp='I'">
						<xsl:value-of select="'true'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'false'"/>
					</xsl:otherwise>
				</xsl:choose>
			</MarriageGrp>
			<TPA_Extensions>
				<JourneyDuration>
					<!--
					<xsl:value-of select="substring-before(@ElapsedTime,'.')"/>
					<xsl:text>:</xsl:text>					
					<xsl:value-of select="substring-after(@ElapsedTime,'.')"/>
					-->
					<xsl:value-of select="concat(floor(@ElapsedTime div 60),':', floor(@ElapsedTime mod 60))"/>
				</JourneyDuration>
				<JourneyTotalDuration>
					<xsl:value-of select="substring-before($tjd,'.')"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring-after($tjd,'.')"/>
				</JourneyTotalDuration>
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	<!-- ******************************************************************** -->
	<xsl:template match="PTC_FareBreakdown" mode="basefare">
		<xsl:param name="total" />
		<xsl:variable name="thistotal">
			<xsl:choose>
				<xsl:when test="PassengerFare/EquivFare/@Amount != ''">
					<xsl:value-of select="translate(PassengerFare/EquivFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::PTC_FareBreakdown[1]" mode="basefare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
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
