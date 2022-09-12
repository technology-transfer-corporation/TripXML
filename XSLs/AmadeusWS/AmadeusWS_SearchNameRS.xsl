<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_SearchNameRS.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 31 Mar 2015 - Rastko - added 	DepartureAirportName and ArrivalAirportName	-->
	<!-- Date: 27 Feb 2015 - Rastko - added 	DepartureAirportName and ArrivalAirportName	-->
	<!-- Date: 11 Aug 2014 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_List"/>
		<xsl:apply-templates select="PNR_Reply"/>
		<xsl:apply-templates select="Errors"/>
	</xsl:template>
	
	<xsl:template match="PNR_List | PNR_Reply | Errors">
		<TT_SearchNameRS Version="1.00">
			<xsl:choose>
				<xsl:when test="Error">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:value-of select="Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<CustomerInfos>
						<xsl:apply-templates select="citypair/travellerInformationSection"/>
						<xsl:apply-templates select="travellerInfo"/>
					</CustomerInfos>
				</xsl:otherwise>
			</xsl:choose>
		</TT_SearchNameRS>
	</xsl:template>
	
	<xsl:template match="travellerInformationSection">
		<CustomerInfo>
			<xsl:attribute name="RPH"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="ActiveItinerary">
				<xsl:choose>
					<xsl:when test="messageAction/business/function!=''">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<Surname><xsl:value-of select="travellerInformation/traveller/surname"/></Surname>
			<GivenName><xsl:value-of select="travellerInformation/passenger/firstName"/></GivenName>
			<UniqueID>
				<xsl:attribute name="ID"><xsl:value-of select="reservationInfo/reservation/controlNumber"/></xsl:attribute>
			</UniqueID>
			<xsl:if test="messageAction/business/function!=''">
				<Itinerary>
					<xsl:choose>
						<xsl:when test="messageAction/business/function='1'">
							<Air>
								<xsl:attribute name="MarketingAirline"><xsl:value-of select="travelProduct/companyDetail/identification"/></xsl:attribute>
								<xsl:attribute name="FlightNumber"><xsl:value-of select="travelProduct/productDetails/identification"/></xsl:attribute>
								<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="productInfo/product/identification"/></xsl:attribute>
								<xsl:attribute name="DepartureDate">
									<xsl:value-of select="concat('20',substring(travelProduct/product/depDate,5),'-')"/>
									<xsl:value-of select="concat(substring(travelProduct/product/depDate,3,2),'-')"/>
									<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:attribute name="DepartureAirport"><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="DepartureAirportName"><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="ArrivalAirport"><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="ArrivalAirportName"><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="NumberInParty"><xsl:value-of select="relatedProduct/quantity"/></xsl:attribute>
							</Air>
						</xsl:when>
						<xsl:when test="messageAction/business/function='32'">
							<Miscellaneous>
								<xsl:attribute name="Provider"><xsl:value-of select="travelProduct/companyDetail/identification"/></xsl:attribute>
								<xsl:attribute name="Date">
									<xsl:value-of select="concat('20',substring(travelProduct/product/depDate,5),'-')"/>
									<xsl:value-of select="concat(substring(travelProduct/product/depDate,3,2),'-')"/>
									<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:attribute name="City"><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="NumberInParty"><xsl:value-of select="relatedProduct/quantity"/></xsl:attribute>
							</Miscellaneous>
						</xsl:when>
					</xsl:choose>
				</Itinerary>
			</xsl:if>
		</CustomerInfo>
	</xsl:template>
	
	<xsl:template match="travellerInfo">
		<CustomerInfo>
			<xsl:attribute name="RPH">1</xsl:attribute>
			<xsl:attribute name="ActiveItinerary">
				<xsl:choose>
					<xsl:when test="../originDestinationDetails">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<Surname><xsl:value-of select="passengerData/travellerInformation/traveller/surname"/></Surname>
			<GivenName><xsl:value-of select="passengerData/travellerInformation/passenger/firstName"/></GivenName>
			<UniqueID>
				<xsl:attribute name="ID"><xsl:value-of select="../pnrHeader/reservationInfo/reservation/controlNumber"/></xsl:attribute>
			</UniqueID>
			<xsl:if test="../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'][not(relatedProduct/status='B')] or ../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']">
				<Itinerary>
					<xsl:choose>
						<xsl:when test="../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'][not(relatedProduct/status='B')]">
							<xsl:variable name="air" select="../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'][not(relatedProduct/status='B')]"/>
							<Air>
								<xsl:attribute name="MarketingAirline"><xsl:value-of select="$air/travelProduct/companyDetail/identification"/></xsl:attribute>
								<xsl:attribute name="FlightNumber"><xsl:value-of select="$air/travelProduct/productDetails/identification"/></xsl:attribute>
								<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="$air/travelProduct/productDetails/classOfService"/></xsl:attribute>
								<xsl:attribute name="DepartureDate">
									<xsl:value-of select="concat('20',substring($air/travelProduct/product/depDate,5),'-')"/>
									<xsl:value-of select="concat(substring($air/travelProduct/product/depDate,3,2),'-')"/>
									<xsl:value-of select="substring($air/travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:attribute name="DepartureAirport"><xsl:value-of select="$air/travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="DepartureAirportName"><xsl:value-of select="$air/travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="ArrivalAirport"><xsl:value-of select="$air/travelProduct/offpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="ArrivalAirportName"><xsl:value-of select="$air/travelProduct/offpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="NumberInParty"><xsl:value-of select="$air/relatedProduct/quantity"/></xsl:attribute>
							</Air>
						</xsl:when>
						<xsl:when test="../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']">
						<xsl:variable name="mis" select="../originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='RU']"/>
							<Miscellaneous>
								<xsl:attribute name="Provider"><xsl:value-of select="$mis/travelProduct/companyDetail/identification"/></xsl:attribute>
								<xsl:attribute name="Date">
									<xsl:value-of select="concat('20',substring($mis/travelProduct/product/depDate,5),'-')"/>
									<xsl:value-of select="concat(substring($mis/travelProduct/product/depDate,3,2),'-')"/>
									<xsl:value-of select="substring($mis/travelProduct/product/depDate,1,2)"/>
								</xsl:attribute>
								<xsl:attribute name="City"><xsl:value-of select="$mis/travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
								<xsl:attribute name="NumberInParty"><xsl:value-of select="$mis/relatedProduct/quantity"/></xsl:attribute>
							</Miscellaneous>
						</xsl:when>
					</xsl:choose>
				</Itinerary>
			</xsl:if>
		</CustomerInfo>
	</xsl:template>

</xsl:stylesheet>