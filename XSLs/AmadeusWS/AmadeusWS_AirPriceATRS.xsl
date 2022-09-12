<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirPriceRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 14 Jun 2009 -  Rastko														-->
	<!-- ================================================================== -->
	<xsl:variable name="segcount" select="count(//fareList/segmentInformation)" />
	<xsl:variable name="tktcount" select="count(//fareList)" />
	<xsl:variable name="count1" select="($segcount div $tktcount)" />
	<xsl:variable name="loop">
		<xsl:choose>
			<xsl:when test="//fareList/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
				<xsl:value-of select="(count(//fareList) div 2) + 1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="count(//fareList) + 1" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_Reply" />
		<xsl:apply-templates select="Fare_PricePNRWithLowerFaresReply" />
		<xsl:apply-templates select="Fare_PricePNRWithBookingClassReply" />
		<xsl:apply-templates select="MessagesOnly_Reply" />
	</xsl:template>
	
	<xsl:template match="MessagesOnly_Reply">
		<OTA_AirPriceRS>
			<xsl:attribute name="Version">2003.2</xsl:attribute>
			<Errors>
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:attribute name="Code"><xsl:value-of select="CAPI_Messages/ErrorCode"/></xsl:attribute>
					<xsl:value-of select="CAPI_Messages/Text"/>
				</Error>	
			</Errors>
		</OTA_AirPriceRS>
	</xsl:template>
	
	<xsl:template match="PNR_Reply">
		<OTA_AirPriceRS>
			<xsl:attribute name="Version">2003.2</xsl:attribute>
			<Errors>
				<xsl:for-each select="Error">
					<Error>
						<xsl:attribute name="Type">Amadeus</xsl:attribute>
						<xsl:attribute name="Code">0</xsl:attribute>
						<xsl:value-of select="."/>
					</Error>	
				</xsl:for-each>
			</Errors>
		</OTA_AirPriceRS>
	</xsl:template>
	
	<xsl:template match="Fare_PricePNRWithLowerFaresReply | Fare_PricePNRWithBookingClassReply">
		<OTA_AirPriceRS>
			<xsl:attribute name="Version">2003.2</xsl:attribute>
			<xsl:choose>
				<xsl:when test="applicationError">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:value-of select="applicationError/applicationErrorInfo/applicationErrorDetail/applicationErrorCode"/>
							</xsl:attribute>
							<xsl:value-of select="applicationError/errorText/errorFreeText"/>
						</Error>	
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<xsl:apply-templates select="fareList[1]/warningInformation[warningCode/applicationErrorDetail/applicationErrorCode = 'BBY']" />
					<PricedItineraries>
						<PricedItinerary>
							<xsl:attribute name="SequenceNumber">1</xsl:attribute>
							<AirItineraryPricingInfo>
								<ItinTotalFare>
									<xsl:variable name="bf">
										<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalbase">
											<xsl:with-param name="sum">0</xsl:with-param>
											<xsl:with-param name="loop">
												<xsl:value-of select="$loop" />
											</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
										</xsl:apply-templates>
									</xsl:variable>
									<xsl:variable name="tf">
										<xsl:apply-templates select="fareList[1]/fareDataInformation" mode="totalprice">
											<xsl:with-param name="sum">0</xsl:with-param>
											<xsl:with-param name="loop">
												<xsl:value-of select="$loop" />
											</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
										</xsl:apply-templates>
									</xsl:variable>
									<xsl:variable name="curt">
										<xsl:choose>
											<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
												<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency" />
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="dect">
										<xsl:choose>
											<xsl:when test="fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
												<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.'))" />
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="string-length(substring-after(fareList[1]/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.'))" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<BaseFare>
										<xsl:attribute name="Amount">
											<xsl:value-of select="$bf"/>
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode">
											<xsl:value-of select="$curt" />
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="$dect"/>
										</xsl:attribute>
									</BaseFare>
									<Taxes>
										<Tax>
											<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
											<xsl:attribute name="Amount">
												<xsl:value-of select="$tf - $bf"/>
											</xsl:attribute>
											<xsl:attribute name="CurrencyCode">
												<xsl:value-of select="$curt" />
											</xsl:attribute>
											<xsl:attribute name="DecimalPlaces">
												<xsl:value-of select="$dect"/>
											</xsl:attribute>
										</Tax>
		  							</Taxes>
		  							<xsl:variable name="qfees">
			  							<xsl:apply-templates select="fareList[1]/otherPricingInfo/attributeDetails[attributeType='FCA']" mode="qfees">
											<xsl:with-param name="sum">0</xsl:with-param>
											<xsl:with-param name="loop">
												<xsl:value-of select="$loop" />
											</xsl:with-param>
											<xsl:with-param name="pos">1</xsl:with-param>
										</xsl:apply-templates>
			  						</xsl:variable>
			  						<xsl:if test="$qfees>0">
			  							<Fees>
			  								<Fee>
			  									<xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
			  									<xsl:attribute name="Amount"><xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/></xsl:attribute>
			  									<xsl:attribute name="CurrencyCode">
													<xsl:value-of select="$curt" />
												</xsl:attribute>
												<xsl:attribute name="DecimalPlaces">
													<xsl:value-of select="$dect"/>
												</xsl:attribute>
			  								</Fee>
			  							</Fees>
			  						</xsl:if>
									<TotalFare>
										<xsl:attribute name="Amount">
											<xsl:value-of select="$tf"/>
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode">
											<xsl:value-of select="$curt" />
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="$dect"/>
										</xsl:attribute>
									</TotalFare>
								</ItinTotalFare>
								<PTC_FareBreakdowns>
									<xsl:for-each select="AirTravelerAvail/PassengerTypeQuantity">
										<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
										<xsl:variable name="paxtype">
											<xsl:choose>
												<xsl:when test="@Code = 'SRC'">YCD</xsl:when>
												<xsl:when test="@Code = 'IN'">INF</xsl:when>
												<xsl:when test="@Code = 'CH'">CHD</xsl:when>
												<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:variable name="paxnum">
											<xsl:value-of select="../../PNR_Reply/travellerInfo[passengerData/travellerInformation/passenger/type=$paxtype]/elementManagementPassenger/reference/number"/>
										</xsl:variable>
										<xsl:variable name="pa">
											<xsl:choose>
												<xsl:when test="@Code='INF' or @Code='IN'">PI</xsl:when>
												<xsl:otherwise>PA</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:apply-templates select="../../fareList[paxSegReference/refDetails/refNumber = $paxnum][paxSegReference/refDetails/refQualifier= $pa]" mode="paxtypes">
											<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
										</xsl:apply-templates>
									</xsl:for-each>
								</PTC_FareBreakdowns>
								<FareInfos>
									<xsl:apply-templates select="fareList[1]/segmentInformation[not(connexInformation/connecDetails/routingInformation)]" 	mode="fareinfo" />
								</FareInfos>
							</AirItineraryPricingInfo>
						</PricedItinerary>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirPriceRS>
	</xsl:template>
	
	<xsl:template match="fareDataInformation" mode="totalbase">
		<xsl:param name="sum" />
		<xsl:param name="loop" />
		<xsl:param name="pos" />
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)" />
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:choose>
				<xsl:when test="fareDataSupInformation[fareDataQualifier = 'E']">
					<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nopt" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nopt" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalbase">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum" />
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop" />
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="fareDataInformation" mode="totalprice">
		<xsl:param name="sum" />
		<xsl:param name="loop" />
		<xsl:param name="pos" />
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../paxSegReference/refDetails)" />
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nopt" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../fareList[$pos + 1]/fareDataInformation" mode="totalprice">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum" />
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop" />
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="warningInformation">
		<Warnings>
			<Warning>
				<xsl:attribute name="Type">Amadeus</xsl:attribute>
				<xsl:value-of select="warningText/errorFreeText" />
			</Warning>
		</Warnings>
	</xsl:template>
	
	<xsl:template match="fareList" mode="paxtypes">
		<xsl:param name="pos"/>
		<xsl:variable name="nip"><xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Quantity" /></xsl:variable>
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Code" />
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="../AirTravelerAvail/PassengerTypeQuantity[position() = $pos]/@Quantity" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:apply-templates select="segmentInformation[not(connexInformation/connecDetails/routingInformation)]" mode="farebasis"/>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:variable name="bfpax">
					<xsl:choose>
						<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
							<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount,'.','') * $nip" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount,'.','') * $nip" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="tfpax">
					<xsl:value-of select="translate(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.','') * $nip" />
				</xsl:variable>
				<xsl:variable name="cur">
					<xsl:choose>
						<xsl:when test="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']">
							<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareCurrency" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="dec">
					<xsl:value-of select="string-length(substring-after(fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount,'.'))"/>
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$bfpax"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$tfpax - $bfpax"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="$cur" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dec"/>
						</xsl:attribute>
					</Tax>
					<xsl:apply-templates select="taxInformation">
						<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
					</xsl:apply-templates>
				</Taxes>
				<xsl:variable name="qfees">
					<xsl:variable name="tot1">
						<xsl:choose>
							<xsl:when test="contains(otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription,' Q') and contains(substring-after(otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription,' Q'),'.')">
								<xsl:call-template name="qfeecalc">
									<xsl:with-param name="fca">
										<xsl:value-of select="otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription"/>
									</xsl:with-param>
									<xsl:with-param name="qfeesum">0</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:value-of select="$tot1 * $nip"/>
				</xsl:variable>
				<xsl:if test="$qfees>0">
					<Fees>
						<Fee>
							<xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
							<xsl:attribute name="Amount"><xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
						</Fee>
					</Fees>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$tfpax"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="$cur" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
	</xsl:template>
	
	<xsl:template match="taxInformation">
		<xsl:param name="nip"/>
		<Tax>
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="taxDetails/taxType/isoCountry" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(amountDetails/fareDataMainInformation/fareAmount,'.','') * $nip" />
			</xsl:attribute>
		</Tax>
	</xsl:template>
	
	<xsl:template match="segmentInformation" mode="farebasis">
		<FareBasisCode>
			<xsl:value-of select="fareQualifier/fareBasisDetails/primaryCode" />
			<xsl:value-of select="fareQualifier/fareBasisDetails/fareBasisCode" />
			<xsl:if test="string-length(fareQualifier/fareBasisDetails/fareBasisCode)=6 and substring(fareQualifier/fareBasisDetails/fareBasisCode,6,1) = 'C' and fareQualifier/fareBasisDetails/discTktDesignator='CH'">
				<xsl:text>H</xsl:text>
			</xsl:if>
		</FareBasisCode>
	</xsl:template>
	
	<xsl:template match="segmentInformation" mode="fareinfo">
		<FareInfo>
			<xsl:variable name="seg"><xsl:value-of select="segmentReference/refDetails/refNumber"/></xsl:variable>
			<xsl:variable name="segment" select="../../PNR_Reply/originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $seg]"/>
			<DepartureDate>
				<xsl:value-of select="format-number(substring($segment/travelProduct/product/depDate,5,2),'2000')"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring($segment/travelProduct/product/depDate,3,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring($segment/travelProduct/product/depDate,1,2)"/>
				<xsl:text>T</xsl:text>
				<xsl:value-of select="format-number($segment/travelProduct/product/depTime,'00:00')"/>
				<xsl:text>:00</xsl:text>
			</DepartureDate> 
			<FareReference><xsl:value-of select="segDetails/segmentDetail/classOfService"/></FareReference> 
			<xsl:if test="../warningInformation/warningText/errorFreeText = 'NON-REFUNDABLE'">
				<RuleInfo>
					<ChargesRules>
						<VoluntaryChanges>
							<Penalty PenaltyType="Ticket Is Non Refundable"/> 
						</VoluntaryChanges>
					</ChargesRules>
				</RuleInfo>
			</xsl:if>
			<FilingAirline><xsl:value-of select="$segment/travelProduct/companyDetail/identification"/></FilingAirline> 
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$segment/travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$segment/travelProduct/offpointDetail/cityCode"/></xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
	
	<xsl:template match="attributeDetails" mode="qfees">
		<xsl:param name="sum" />
		<xsl:param name="loop" />
		<xsl:param name="pos" />
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../../paxSegReference/refDetails)" />
		</xsl:variable>
		<xsl:variable name="tot1">
			<xsl:choose>
				<xsl:when test="contains(attributeDescription,' Q') and contains(substring-after(attributeDescription,' Q'),'.')">
					<xsl:call-template name="qfeecalc">
						<xsl:with-param name="fca"><xsl:value-of select="attributeDescription"/></xsl:with-param>
						<xsl:with-param name="qfeesum">0</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot"><xsl:value-of select="$tot1 * $nopt"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../../fareList[$pos + 1]">
				<xsl:apply-templates select="../../../fareList[$pos + 1]/otherPricingInfo/attributeDetails[attributeType='FCA']" mode="qfees">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum" />
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop" />
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="qfeecalc">
		<xsl:param name="fca"/>
		<xsl:param name="qfeesum"/>
		<xsl:variable name="q1"><xsl:value-of select="substring-after($fca,' Q')"/></xsl:variable>
		<xsl:if test="$q1!=''">
			<xsl:variable name="q2"><xsl:value-of select="substring-before($q1,'.')"/></xsl:variable>
			<xsl:if test="$q2!=''">
				<xsl:variable name="q3"><xsl:value-of select="substring(substring-after($q1,'.'),1,2)"/></xsl:variable>
				<xsl:if test="$q3!=''">
					<xsl:variable name="q4"><xsl:value-of select="$q2"/><xsl:value-of select="$q3"/></xsl:variable>
					<xsl:variable name="q5"><xsl:value-of select="translate($q4,'0123456789','')"/></xsl:variable>
					<xsl:variable name="totqfee">
						<xsl:choose>
							<xsl:when test="$q5=''">
								<xsl:value-of select="$q2"/>.<xsl:value-of select="$q3"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="newfee">
						<xsl:value-of select="$qfeesum + $totqfee"/>
					</xsl:variable>
					<xsl:call-template name="qfeecalc">
						<xsl:with-param name="fca"><xsl:value-of select="$q1"/></xsl:with-param>
						<xsl:with-param name="qfeesum"><xsl:value-of select="$newfee"/></xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
		</xsl:if>
		<xsl:if test="substring-after($fca,' Q') = ''">
			<xsl:value-of select="$qfeesum"/>
		</xsl:if>
	</xsl:template>
		
</xsl:stylesheet>