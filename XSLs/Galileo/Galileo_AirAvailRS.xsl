<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_AirAvailRS.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 25 Sep 2013 - Rastko - commented out operating airline flight number			-->
	<!-- Date: 22 Aug 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="AirAvailability_12" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="AirAvailability_12">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Galileo</xsl:attribute>
			<xsl:if test="AirAvail/AvailFlt">
				<Success />
				<xsl:if test="AirAvail/MoreToken/Tok != '' and AirAvail/ErrorCode != '0020'">
					<xsl:apply-templates select="AirAvail/MoreToken" />
				</xsl:if>
				<OriginDestinationOptions>
					<xsl:apply-templates select="AirAvail" />
				</OriginDestinationOptions>
			</xsl:if>
			<xsl:if test="AirAvail/AvailSummary/ErrInd!= '0'">
				<Errors>
					<xsl:if test="AirAvail/AvailSummary/ErrInd!= '0'">
						<xsl:choose>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='20'">
								<Error Type="Galileo">No more later flights</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='21'">
								<Error Type="Galileo">No displayable flights</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='28'">
								<Error Type="Galileo">No more earlier flights</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='37'">
								<Error Type="Galileo">Flight departed</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='38'">
								<Error Type="Galileo">Flight cancelled</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='39'">
								<Error Type="Galileo">Flight boarding</Error>
							</xsl:when>
							<xsl:when test="AirAvail/AvailSummary/ErrNum='42'">
								<Error Type="Galileo">No direct service between cities (change of gauge)</Error>
							</xsl:when>
							<xsl:otherwise>
								<Error Type="Galileo">
									<xsl:attribute name="Code">
										<xsl:value-of select="AirAvail/AvailSummary/ErrNum" />
									</xsl:attribute>
									<xsl:value-of select="AirAvail/AvailSummary/ErrNum" />
								</Error>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
					<xsl:if test="AirAvail/ErrorCode!= '' and not(AirAvail/AvailFlt)">
						<Error Type="Galileo">
							<xsl:attribute name="Code">
								<xsl:value-of select="AirAvail/ErrorCode" />
							</xsl:attribute>
							<xsl:call-template name="errors">
								<xsl:with-param name="errorcode">
									<xsl:value-of select="AirAvail/ErrorCode" />
								</xsl:with-param>
							</xsl:call-template>
						</Error>
					</xsl:if>
				</Errors>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="AirAvail/ErrorCode!= '' and not(AirAvail/AvailFlt)">
					<Errors>
						<Error Type="Galileo">
							<xsl:attribute name="Code">
								<xsl:value-of select="AirAvail/ErrorCode" />
							</xsl:attribute>
							<xsl:call-template name="errors">
								<xsl:with-param name="errorcode">
									<xsl:value-of select="AirAvail/ErrorCode" />
								</xsl:with-param>
							</xsl:call-template>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="TransactionErrorCode">
					<Errors>
						<Error Type="Galileo">
							<xsl:attribute name="Code">
								<xsl:value-of select="TransactionErrorCode/Code" />
							</xsl:attribute>
							<xsl:value-of select="TransactionErrorCode/Code" />
						</Error>
					</Errors>
				</xsl:when>
			</xsl:choose>
		</OTA_AirAvailRS>
	</xsl:template>
	<!-- **************************************************************** -->
	<xsl:template match="AirAvail">
		<xsl:apply-templates select="AvailFlt[1]" mode="od">
			<xsl:with-param name="rph">1</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="MoreToken">
		<TPA_Extensions>
			<MoreIndicator>
				<xsl:value-of select="Tok" />
			</MoreIndicator>
		</TPA_Extensions>
	</xsl:template>
	<!-- **************************************************************** -->
	<xsl:template match="AvailFlt" mode="od">
		<xsl:param name="rph" />
		<xsl:choose>
			<xsl:when test="Conx='N'">
				<OriginDestinationOption>
					<xsl:apply-templates select="JrnyTm" />
					<xsl:apply-templates select="." mode="Segment">
						<xsl:with-param name="rph">
							<xsl:value-of select="$rph" />
						</xsl:with-param>
					</xsl:apply-templates>
				</OriginDestinationOption>
			</xsl:when>
			<xsl:otherwise>
				<OriginDestinationOption>
					<xsl:apply-templates select="JrnyTm" />
					<xsl:apply-templates select="." mode="Segment">
						<xsl:with-param name="rph">
							<xsl:value-of select="$rph" />
						</xsl:with-param>
					</xsl:apply-templates>
					<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="cxn">
						<xsl:with-param name="rph">
							<xsl:value-of select="$rph" />
						</xsl:with-param>
					</xsl:apply-templates>
				</OriginDestinationOption>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="od1">
			<xsl:with-param name="rph">
				<xsl:value-of select="$rph" />
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="AvailFlt" mode="od1">
		<xsl:param name="rph" />
		<xsl:choose>
			<xsl:when test="Conx='Y' and preceding-sibling::AvailFlt[1]/Conx='Y'">
				<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="od1">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="Conx='N' and preceding-sibling::AvailFlt[1]/Conx='Y'">
				<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="od1">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="od">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph + 1" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="AvailFlt" mode="cxn">
		<xsl:param name="rph" />
		<xsl:choose>
			<xsl:when test="Conx='Y'">
				<xsl:apply-templates select="." mode="Segment">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph" />
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="following-sibling::AvailFlt[1]" mode="cxn">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="Segment">
					<xsl:with-param name="rph">
						<xsl:value-of select="$rph" />
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="JrnyTm">
		<xsl:variable name="zeros">00</xsl:variable>
		<xsl:variable name="jt">
			<xsl:value-of select="." />
		</xsl:variable>
		<xsl:variable name="hours">
			<xsl:choose>
				<xsl:when test="substring-before(($jt div 60),'.')=''">
					<xsl:value-of select="$jt div 60" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(($jt div 60),'.')" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="minutes">
			<xsl:value-of select="$jt - ($hours*60)" />
		</xsl:variable>
		<TPA_Extensions>
			<TotalJourneyDuration>
				<xsl:choose>
					<xsl:when test="$minutes = 'NaN'">
						<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
						<xsl:value-of select="$hours" />
						<xsl:text>:00</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" /><xsl:value-of select="$hours" />:<xsl:value-of select="substring(string	($zeros),1,2-string-length($minutes))" /><xsl:value-of select="$minutes" />
					</xsl:otherwise>
				</xsl:choose>
			</TotalJourneyDuration>
		</TPA_Extensions>
	</xsl:template>
	<xsl:template match="FltTm">
		<xsl:variable name="zeros">00</xsl:variable>
		<xsl:variable name="jt">
			<xsl:value-of select="." />
		</xsl:variable>
		<xsl:variable name="hours">
			<xsl:choose>
				<xsl:when test="substring-before(($jt div 60),'.')=''">
					<xsl:value-of select="$jt div 60" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(($jt div 60),'.')" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="minutes">
			<xsl:value-of select="$jt - ($hours*60)" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$minutes = 'NaN'">
				<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
				<xsl:value-of select="$hours" />
				<xsl:text>:00</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" /><xsl:value-of select="$hours" />:<xsl:value-of select="substring(string($zeros),1,2-string-length($minutes))" /><xsl:value-of select="$minutes" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="AvailFlt" mode="Segment">
		<xsl:param name="rph" />
		<FlightSegment>
			<xsl:variable name="zeros">000</xsl:variable>
			<xsl:attribute name="DepartureDateTime"><xsl:value-of select="substring(StartDt,1,4)" />-<xsl:value-of select="substring(StartDt,5,2)" />-<xsl:value-of select="substring(StartDt,7,2)" />T<xsl:variable name="st">
					<xsl:value-of select="substring(string($zeros),1,4-string-length(StartTm))" />
					<xsl:value-of select="StartTm" />
				</xsl:variable><xsl:value-of select="substring($st,1,2)" />:<xsl:value-of select="substring($st,3,2)" />:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime"><xsl:value-of select="substring(EndDt,1,4)" />-<xsl:value-of select="substring(EndDt,5,2)" />-<xsl:value-of select="substring(EndDt,7,2)" />T<xsl:variable name="et">
					<xsl:value-of select="substring(string($zeros),1,4-string-length(EndTm))" />
					<xsl:value-of select="EndTm" />
				</xsl:variable><xsl:value-of select="substring($et,1,2)" />:<xsl:value-of select="substring($et,3,2)" />:00</xsl:attribute>
			<xsl:attribute name="StopQuantity">
				<xsl:value-of select="NumStops" />
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$rph" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="substring(string($zeros),1,4-string-length(FltNum))" />
				<xsl:value-of select="FltNum" />
			</xsl:attribute>
			<xsl:if test="FltTm != ''">
				<xsl:attribute name="JourneyDuration">
					<xsl:apply-templates select="FltTm" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="Perf!='' and Perf!='N' and Perf!='U'">
				<xsl:attribute name="OnTimeRate">
					<xsl:value-of select="Perf * 10" />
				</xsl:attribute>
			</xsl:if>
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
			<OperatingAirline>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="OpAirV != ''"><xsl:value-of select="OpAirV" /></xsl:when>
						<xsl:otherwise><xsl:value-of select="AirV" /></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<!--xsl:if test="OpFltDesignator != ''">
					<xsl:attribute name="FlightNumber">
						<xsl:value-of select="substring(string($zeros),1,4-string-length(OpFltDesignator))" />
						<xsl:value-of select="OpFltDesignator" />
						<xsl:if test="OpFltSuf != ''">
							<xsl:attribute name="Suffix">
								<xsl:value-of select="OpFltSuf" />
							</xsl:attribute>
						</xsl:if>
					</xsl:attribute>
				</xsl:if-->
			</OperatingAirline>
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
			<xsl:apply-templates select="following-sibling::BICAvail[1]/BICStatusAry/BICStatus" />
			<TPA_Extensions>
				<xsl:if test="AirpChg='Y'">
					<AirportChange>Y</AirportChange>
				</xsl:if>
				<xsl:if test="DayChg != '00'">
					<DayChange>
						<xsl:value-of select="DayChg" />
					</DayChange>
				</xsl:if>
				<xsl:apply-templates select="StartTerminal" />
				<xsl:apply-templates select="EndTerminal" />
				<FlightFrequency>
					<xsl:if test="substring(DaysOperates,2,1)='Y'">1</xsl:if>
					<xsl:if test="substring(DaysOperates,3,1)='Y'">2</xsl:if>
					<xsl:if test="substring(DaysOperates,4,1)='Y'">3</xsl:if>
					<xsl:if test="substring(DaysOperates,5,1)='Y'">4</xsl:if>
					<xsl:if test="substring(DaysOperates,6,1)='Y'">5</xsl:if>
					<xsl:if test="substring(DaysOperates,7,1)='Y'">6</xsl:if>
					<xsl:if test="substring(DaysOperates,1,1)='Y'">7</xsl:if>
				</FlightFrequency>
				<xsl:apply-templates select="GenTrafRestriction" />
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	<xsl:template match="BICStatus">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<BookingClassAvail>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="BIC" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigStatusCode">
				<xsl:choose>
					<xsl:when test="substring(Status,4,1)='A'">
						<xsl:value-of select="substring(Status,4,1)" />
					</xsl:when>
					<xsl:when test="substring(Status,1,1)='A'">
						<xsl:value-of select="substring(Status,1,1)" />
					</xsl:when>
					<xsl:when test="substring(Status,3,1)!=' '">
						<xsl:value-of select="substring(Status,3,1)" />
					</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
		</BookingClassAvail>
	</xsl:template>
	<xsl:template match="StartTerminal">
		<xsl:if test=". != ''">
			<DepartureTerminal>
				<xsl:value-of select="." />
			</DepartureTerminal>
		</xsl:if>
	</xsl:template>
	<xsl:template match="EndTerminal">
		<xsl:if test=". != ''">
			<ArrivalTerminal>
				<xsl:value-of select="." />
			</ArrivalTerminal>
		</xsl:if>
	</xsl:template>
	<xsl:template match="GenTrafRestriction">
		<xsl:if test=". != ''">
			<TrafficRestrictions>
				<xsl:value-of select="." />
			</TrafficRestrictions>
		</xsl:if>
	</xsl:template>
	<xsl:template name="errors">
		<xsl:param name="errorcode" />
		<xsl:choose>
			<xsl:when test="$errorcode = '0002'">
				<xsl:text>CHECK NUMBER IN PARTY</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0003'">
				<xsl:text>INVALID CLASS OF SERVICE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0004'">
				<xsl:text>INVALID DATE FORMAT</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0005'">
				<xsl:text>INVALID DEPARTURE CODE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0006'">
				<xsl:text>INVALID ARRIVAL CODE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0007'">
				<xsl:text>INVALID TIME FORMAT</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0008'">
				<xsl:text>INVALID TIME MODIFIER</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0011'">
				<xsl:text>MAXIMUM AIRLINE PREFERENCE EXCEEDED</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0012'">
				<xsl:text>INVALID AIRLINE PREFERENCE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0016'">
				<xsl:text>INVALID FLIGHT NUMBER</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0017'">
				<xsl:text>INVALID CONNECTION INDICATOR</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0018'">
				<xsl:text>INVALID MORE INDICATOR INFORMATION</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0019'">
				<xsl:text>GENERAL SYSTEM ERROR</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0020'">
				<xsl:text>NO MORE FLIGHTS</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0021'">
				<xsl:text>NO DISPLAYABLE FLIGHTS</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0022'">
				<xsl:text>INVALID CITY PAIR DATA</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0023'">
				<xsl:text>INVALID AIRLINE CODE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0024'">
				<xsl:text>SPECIFIC AVAILABILITY FOR AIRLINE NOT FOUND</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0025'">
				<xsl:text>DATE OUTSIDE RANGE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0027'">
				<xsl:text>INVALID CITY PAIR DATA</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0031'">
				<xsl:text>SPECIFIC FLIGHT NOT FOUND</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0033'">
				<xsl:text>LEGS ARE NOT CONTINUOUS</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0034'">
				<xsl:text>CHECK NUMBER IN PARTY</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0035'">
				<xsl:text>INVALID CLASS OF SERVICE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0040'">
				<xsl:text>AIRLINE NOT AVAILABLE</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0041'">
				<xsl:text>INVALID FLIGHT NUMBER</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0043'">
				<xsl:text>FLIGHT NOT OPERATING</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0044'">
				<xsl:text>INVALID HOURS BEFORE IN TIME WINDOW </xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0045'">
				<xsl:text>INVALID HOURS AFTER IN TIME WINDOW </xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0046'">
				<xsl:text>INVALID JOURNEY TOTAL HOURS</xsl:text>
			</xsl:when>
			<xsl:when test="$errorcode = '0049'">
				<xsl:text>INVALID INPUT DATA</xsl:text>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
