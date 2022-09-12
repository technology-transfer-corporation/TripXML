<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Aggregation_LowFareRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 09 Apr 2012 - Rastko - added Notes element to output					-->
	<!-- Date: 23 Jan 2011 - Rastko - changed sorting process of low fare search schedule		-->
	<!-- Date: 25 Nov 2010 - Rastko - changed display of SequenceNumber by position		-->
	<!-- Date: 14 Jul 2006 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="SuperRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="SuperRS">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRS">
				<OTA_AirLowFareSearchRS>
					<xsl:if test="OTA_AirLowFareSearchRS/@EchoToken != ''">
						<xsl:attribute name="EchoToken"><xsl:value-of select="OTA_AirLowFareSearchRS/@EchoToken" /></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<xsl:choose>
						<xsl:when test="OTA_AirLowFareSearchRS/Errors/Error != '' and OTA_AirLowFareSearchRS/Success">
							<Warnings>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error">
									<Warning>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:value-of select="." />
									</Warning>
								</xsl:for-each>
							</Warnings>
						</xsl:when>
						<xsl:when test="OTA_AirLowFareSearchRS/Errors/Error != ''">
							<Errors>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error">
									<Error>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@Code != ''">
													<xsl:value-of select="@Code" />
												</xsl:when>
												<xsl:otherwise>E</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:value-of select="." />
									</Error>
								</xsl:for-each>
							</Errors>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="OTA_AirLowFareSearchRS/Success">
						<xsl:copy-of select="SearchPromotionsResponse"/>
						<Success></Success>
						<PricedItineraries>
							<xsl:apply-templates select="OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary">
								<xsl:sort data-type="number" order="ascending" select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/>
							</xsl:apply-templates>
						</PricedItineraries>
					</xsl:if>
				</OTA_AirLowFareSearchRS>
			</xsl:when>
			<xsl:when test="OTA_AirLowFareSearchPlusRS">
				<OTA_AirLowFareSearchPlusRS>
					<xsl:if test="OTA_AirLowFareSearchPlusRS/@EchoToken != ''">
						<xsl:attribute name="EchoToken"><xsl:value-of select="OTA_AirLowFareSearchPlusRS/@EchoToken" /></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<xsl:choose>
						<xsl:when test="OTA_AirLowFareSearchPlusRS/Errors/Error != '' and OTA_AirLowFareSearchPlusRS/Success">
							<Warnings>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error | OTA_AirLowFareSearchPlusRS/Errors/Error">
									<Warning>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:value-of select="." />
									</Warning>
								</xsl:for-each>
							</Warnings>
						</xsl:when>
						<xsl:when test="OTA_AirLowFareSearchPlusRS/Errors/Error != ''">
							<Errors>
								<xsl:for-each select="OTA_AirLowFareSearchPlusRS/Errors/Error">
									<Error>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@Code != ''">
													<xsl:value-of select="@Code" />
												</xsl:when>
												<xsl:otherwise>E</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:value-of select="." />
									</Error>
								</xsl:for-each>
							</Errors>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="OTA_AirLowFareSearchPlusRS/Success">
						<xsl:copy-of select="SearchPromotionsResponse"/>
						<Success></Success>
						<PricedItineraries>
							<xsl:apply-templates select="OTA_AirLowFareSearchPlusRS/PricedItineraries/PricedItinerary">
								<xsl:sort data-type="number" order="ascending" select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/>
							</xsl:apply-templates>
						</PricedItineraries>
					</xsl:if>
				</OTA_AirLowFareSearchPlusRS>
			</xsl:when>
			<xsl:when test="OTA_AirLowFareSearchScheduleRS">
				<OTA_AirLowFareSearchScheduleRS>
					<xsl:if test="OTA_AirLowFareSearchScheduleRS/@EchoToken != ''">
						<xsl:attribute name="EchoToken"><xsl:value-of select="OTA_AirLowFareSearchScheduleRS/@EchoToken" /></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<xsl:choose>
						<xsl:when test="OTA_AirLowFareSearchScheduleRS/Errors/Error != '' and OTA_AirLowFareSearchScheduleRS/Success">
							<Warnings>
								<xsl:for-each select="OTA_AirLowFareSearchScheduleRS/Errors/Error | OTA_AirLowFareSearchScheduleRS/Errors/Error">
									<Warning>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:value-of select="." />
									</Warning>
								</xsl:for-each>
							</Warnings>
						</xsl:when>
						<xsl:when test="OTA_AirLowFareSearchScheduleRS/Errors/Error != ''">
							<Errors>
								<xsl:for-each select="OTA_AirLowFareSearchScheduleRS/Errors/Error">
									<Error>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@Code != ''">
													<xsl:value-of select="@Code" />
												</xsl:when>
												<xsl:otherwise>E</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:value-of select="." />
									</Error>
								</xsl:for-each>
							</Errors>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="OTA_AirLowFareSearchScheduleRS/Success">
						<xsl:copy-of select="SearchPromotionsResponse"/>
						<Success></Success>
						<PricedItineraries>
							<xsl:apply-templates select="OTA_AirLowFareSearchScheduleRS/PricedItineraries/PricedItinerary">
								<xsl:sort data-type="number"  order="ascending" select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/TPA_Extensions/FromTotalFare/@Amount"/>
							</xsl:apply-templates>
						</PricedItineraries>
					</xsl:if>
				</OTA_AirLowFareSearchScheduleRS>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="Provider"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
			<xsl:copy-of select="AirItinerary" />
			<xsl:copy-of select="AirItineraryPricingInfo" />
			<xsl:copy-of select="Notes"/>
			<xsl:copy-of select="TicketingInfo" />
		</PricedItinerary>
	</xsl:template>
	<!--*************************************************************-->
	
</xsl:stylesheet>
