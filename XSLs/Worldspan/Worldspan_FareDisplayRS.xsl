<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_FareDisplayRS.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 27 Apr 2006 - Rastko								 						      -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<xsl:apply-templates select="FAW5" />
		<xsl:apply-templates select="XXW" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="FAW5">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="FareInfo/RespHeader/ErrMsg='Y'">
					<Errors>
						<Error Type="Galileo">
							<xsl:value-of select="FareInfo/InfoMsg/Text"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<FareDisplayInfos>
						<xsl:variable name="start"><xsl:value-of select="Request/OTA_AirFareDisplayRQ/OriginDestinationInformation[1]/DepartureDateTime"/></xsl:variable>
						<xsl:variable name="end"><xsl:value-of select="Request/OTA_AirFareDisplayRQ/OriginDestinationInformation[position()=last()]/DepartureDateTime"/></xsl:variable>
						<xsl:apply-templates select="FAR_INF[DEP_AIR != 'JL']">
							<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
							<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
						</xsl:apply-templates>
					</FareDisplayInfos>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFareDisplayRS>
	</xsl:template>

	<xsl:template match="FAR_INF">
		<xsl:param name="start"/>
		<xsl:param name="end"/>
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="RND_TRP_IND = 'R'">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:choose>
					<xsl:when test="BKG_COD != ''"><xsl:value-of select="BKG_COD"/></xsl:when>
					<xsl:when test="DEP_AIR = 'CO'"><xsl:value-of select="substring(FAR_BAS_COD,string-length(FAR_BAS_COD),1)"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="substring(FAR_BAS_COD,1,1)"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:attribute name="ArrivalDate"><xsl:value-of select="$end"/></xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="FAR_BAS_COD"/></FareReference>
			<RuleInfo>
				<xsl:if test="ADV_PUR = '##'">
					<ResTicketingRules>
						<AdvResTicketing AdvResInd="true">
							<xsl:if test="LST_TKT != ''">
								<xsl:attribute name="AdvTicketingInd">true</xsl:attribute>
								<xsl:attribute name="RequestedTicketingDate">
									<xsl:variable name="travarr"><xsl:value-of select="substring(LST_TKT,3,3)"/></xsl:variable>
									<xsl:value-of select="substring(LST_TKT,1,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="$travarr" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-20</xsl:text>
									<xsl:value-of select="substring(LST_TKT,6,2)"/>
								</xsl:attribute>
							</xsl:if>
							<AdvReservation>
							</AdvReservation>
						</AdvResTicketing>
					</ResTicketingRules>
				</xsl:if>
				<xsl:if test="MIN_STY != ''">
					<LengthOfStayRules>
						<MinimumStay>
							<xsl:attribute name="MinStay"><xsl:value-of select="MIN_STY"/></xsl:attribute>
						</MinimumStay>
						<MaximumStay>
							<xsl:attribute name="MaxStay"><xsl:value-of select="MAX_STY"/></xsl:attribute>
						</MaximumStay>
					</LengthOfStayRules>
				</xsl:if>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="DEP_AIR"/></xsl:attribute>
			</FilingAirline>
			<DepartureLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="HED_APT"/></xsl:attribute>
			</DepartureLocation>
			<ArrivalLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="SDE_APT"/></xsl:attribute>
			</ArrivalLocation>
			<xsl:if test="FRT_TVL != '' or LST_TVL != ''">
				<Restrictions>
					<Restriction>
						<DateRestriction>
							<xsl:if test="FRT_TVL != ''">
								<xsl:attribute name="StartDate">
									<xsl:variable name="travdep"><xsl:value-of select="substring(FRT_TVL,3,3)"/></xsl:variable>
									<xsl:value-of select="substring(FRT_TVL,1,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="$travdep" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-20</xsl:text>
									<xsl:value-of select="substring(FRT_TVL,6,2)"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="LST_TVL != ''">
								<xsl:attribute name="EndDate">
									<xsl:variable name="travarr"><xsl:value-of select="substring(LST_TVL,3,3)"/></xsl:variable>
									<xsl:value-of select="substring(LST_TVL,1,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="$travarr" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-20</xsl:text>
									<xsl:value-of select="substring(LST_TVL,6,2)"/>
								</xsl:attribute>
							</xsl:if>
						</DateRestriction>
					</Restriction>
				</Restrictions>
			</xsl:if>
			<PricingInfo>
				<xsl:choose>
					<xsl:when test="FAR_SRC = 'C'">
						<xsl:attribute name="NegotiatedFare">true</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="NegotiatedFare">false</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<!--xsl:attribute name="PassengerTypeCode">
					<xsl:choose>
						<xsl:when test="HasFreeForm = 'Y'">
							<xsl:value-of select="substring-after(FreeForm,'PTC:')"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="PIC"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute-->
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:variable name="bf">
							<xsl:value-of select="translate(CUR_AMT,'.','')"/>
						</xsl:variable>
						<xsl:variable name="qs">
							<!--xsl:choose>
								<xsl:when test="Q_SUR != '' and Q_SUR != 'N' and Q_SUR != 'Y'">
									<xsl:value-of select="translate(Q_SUR,'.','')"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose-->
							<xsl:text>0</xsl:text>
						</xsl:variable>
						<xsl:value-of select="$bf + $qs"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(CUR_AMT,'.'))"/></xsl:attribute>
				</BaseFare>
				<xsl:if test="Q_SUR != '' and Q_SUR != 'N' and Q_SUR != 'Y'">
					<Fees>
						<Fee>
							<xsl:attribute name="FeeCode">
								<xsl:text>Q-Surchage</xsl:text>
							</xsl:attribute>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(Q_SUR,'.','')"/>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="string-length(substring-after(Q_SUR,'.'))"/>
							</xsl:attribute>
						</Fee> 
					</Fees>
				</xsl:if>
			</PricingInfo>
		</FareDisplayInfo>
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
			<xsl:when test="$month = 'Jan'">01</xsl:when>
			<xsl:when test="$month = 'Feb'">02</xsl:when>
			<xsl:when test="$month = 'Mar'">03</xsl:when>
			<xsl:when test="$month = 'Apr'">04</xsl:when>
			<xsl:when test="$month = 'May'">05</xsl:when>
			<xsl:when test="$month = 'Jun'">06</xsl:when>
			<xsl:when test="$month = 'Jul'">07</xsl:when>
			<xsl:when test="$month = 'Aug'">08</xsl:when>
			<xsl:when test="$month = 'Sep'">09</xsl:when>
			<xsl:when test="$month = 'Oct'">10</xsl:when>
			<xsl:when test="$month = 'Nov'">11</xsl:when>
			<xsl:when test="$month = 'Dec'">12</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="XXW">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<Errors>
				<Error>
					<xsl:attribute name="Type">Worldspan</xsl:attribute>
					<xsl:attribute name="Code">
						<xsl:value-of select="ERROR/CODE"/>
					</xsl:attribute>
					<xsl:value-of select="ERROR/TEXT"/>
				</Error>
			</Errors>
		</OTA_AirFareDisplayRS>
	</xsl:template>


</xsl:stylesheet>
