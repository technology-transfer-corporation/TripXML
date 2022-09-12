<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirFlifoRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 14 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Air_FlightInfoReply" />
		<xsl:apply-templates select="Fault" />
		<xsl:apply-templates select="MessagesOnly_Reply" />
		<xsl:apply-templates select="Errors" />
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply">
		<OTA_AirFlifoRS Version="1.001">
			<Errors>
				<xsl:apply-templates select="CAPI_Messages[LineType='E']" />
				<xsl:apply-templates select="CAPI_Messages[LineType='EC']" />
			</Errors>
		</OTA_AirFlifoRS>
	</xsl:template>
	<xsl:template match="Fault">
		<OTA_AirFlifoRS Version="1.001">
			<Errors>
				<Error>
					<xsl:attribute name="Code">
						<xsl:value-of select="substring-before(faultstring,'|')" />
					</xsl:attribute>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="substring-after(substring-after(faultstring,'|'),'|')" />
				</Error>
			</Errors>
		</OTA_AirFlifoRS>
	</xsl:template>
	<xsl:template match="Errors">
		<OTA_AirFlifoRS Version="1.001">
			<Errors>
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="Error" />
				</Error>
			</Errors>
		</OTA_AirFlifoRS>
	</xsl:template>
	<xsl:template match="Air_FlightInfoReply">
		<OTA_AirFlifoRS Version="1.001">
			<FlightInfoDetails>
				<xsl:attribute name="TotalTripTime">
					<xsl:value-of select="flightScheduleDetails/additionalProductDetails/facilitiesInformation/description" />
				</xsl:attribute>
				<xsl:variable name="airline">
					<xsl:value-of select="flightScheduleDetails/generalFlightInfo/companyDetails/marketingCompany" />
				</xsl:variable>
				<xsl:variable name="flight">
					<xsl:value-of select="flightScheduleDetails/generalFlightInfo/productIdDetails/flightNumber" />
				</xsl:variable>
				<xsl:variable name="status">
					<xsl:choose>
						<xsl:when test="flightScheduleDetails/interactiveFreeText/freeText = 'FLIGHT CANCELLED'">Departure Canceled</xsl:when>
						<xsl:otherwise><xsl:value-of select="flightScheduleDetails/boardPointAndOffPointDetails[position()=last()]/interactiveFreeText[position()=last()]/freeText"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:apply-templates select="flightScheduleDetails/boardPointAndOffPointDetails[additionalProductDetails/legDetails/equipment != '']">
					<xsl:with-param name="airline">
						<xsl:value-of select="$airline" />
					</xsl:with-param>
					<xsl:with-param name="flight">
						<xsl:value-of select="$flight" />
					</xsl:with-param>
					<xsl:with-param name="status">
						<xsl:value-of select="$status" />
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:if test="flightScheduleDetails/interactiveFreeText/freeText">
					<xsl:apply-templates select="flightScheduleDetails/interactiveFreeText/freeText" mode="comment" />
				</xsl:if>
			</FlightInfoDetails>
		</OTA_AirFlifoRS>
	</xsl:template>
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	
	<xsl:template match="boardPointAndOffPointDetails">
		<xsl:param name="airline" />
		<xsl:param name="flight" />
		<xsl:param name="status" />
		<FlightLegInfo>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="$flight" />
			</xsl:attribute>
			<xsl:attribute name="JourneyDuration"><xsl:value-of select="substring(string(additionalProductDetails/legDetails/duration),1,2)" />:<xsl:value-of select="substring(string(additionalProductDetails/legDetails/duration),3,2)" /></xsl:attribute>
			<xsl:if test="additionalProductDetails/facilitiesInformation/description != ''">
				<xsl:attribute name="GroundDuration"><xsl:value-of select="substring(additionalProductDetails/facilitiesInformation/description,1,2)" />:<xsl:value-of select="substring(additionalProductDetails/facilitiesInformation/description,3,2)" /></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="FlightStatus">
				<xsl:choose>
					<xsl:when test="substring($status,1,2)='AD' and substring($status,9)='TOOK OFF'">
						<xsl:text>Took off</xsl:text>
					</xsl:when>
					<xsl:when test="substring($status,1,2)='AD' and substring($status,9)='LEFT THE GATE'">
						<xsl:text>Left gate</xsl:text>
					</xsl:when>
					<xsl:when test="substring($status,1,2)='AA' and substring($status,9)='ARRIVED'">
						<xsl:text>Arrived</xsl:text>
					</xsl:when>
					<xsl:when test="substring($status,1,2)='AA' and substring($status,9)='AIRCRAFT LANDED'">
						<xsl:text>Landed</xsl:text>
					</xsl:when>
					<xsl:when test="substring($status,1,2)='EA' and substring($status,9)='ESTIMATED TIME OF ARRIVAL'">
						<xsl:text>In flight</xsl:text>
					</xsl:when>
					<xsl:when test="substring($status,1,2)='ED' and substring($status,9)='ESTIMATED TIME OF DEPARTURE'">
						<xsl:text>On ground</xsl:text>
					</xsl:when>
					<xsl:when test="../../flightScheduleDetails/interactiveFreeText/freeText = 'FLIGHT CANCELLED'">Departure Canceled</xsl:when>
					<xsl:otherwise>
						<xsl:text>On Schedule</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="generalFlightInfo/boardPointDetails/trueLocationId" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="following-sibling::boardPointAndOffPointDetails[1]/generalFlightInfo/offPointDetails/trueLocationId" />
				</xsl:attribute>
				<xsl:attribute name="Diversion">0</xsl:attribute>
			</ArrivalAirport>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="$airline" />
				</xsl:attribute>
			</MarketingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="additionalProductDetails/legDetails/equipment" />
				</xsl:attribute>
			</Equipment>
			<xsl:variable name="arrd">
				<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/generalFlightInfo/flightDate/arrivalDate" />
			</xsl:variable>
			<xsl:variable name="depd">
				<xsl:apply-templates select="generalFlightInfo/flightDate/departureDate" />
			</xsl:variable>
			<DepartureDateTime>
				<xsl:attribute name="Scheduled">
					<xsl:apply-templates select="generalFlightInfo/flightDate/departureDate" />
					<xsl:apply-templates select="generalFlightInfo/flightDate/departureTime" />
				</xsl:attribute>
				<xsl:apply-templates select="interactiveFreeText" mode="depest">
					<xsl:with-param name="dep">
						<xsl:value-of select="$depd" />
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="interactiveFreeText" mode="depact">
					<xsl:with-param name="dep">
						<xsl:value-of select="$depd" />
					</xsl:with-param>
				</xsl:apply-templates>
			</DepartureDateTime>
			<ArrivalDateTime>
				<xsl:attribute name="Scheduled">
					<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/generalFlightInfo/flightDate/arrivalDate" />
					<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/generalFlightInfo/flightDate/arrivalTime" />
				</xsl:attribute>
				<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/interactiveFreeText" mode="arrest">
					<xsl:with-param name="arr">
						<xsl:value-of select="$arrd" />
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/interactiveFreeText" mode="arract">
					<xsl:with-param name="arr">
						<xsl:value-of select="$arrd" />
					</xsl:with-param>
				</xsl:apply-templates>
			</ArrivalDateTime>
			<xsl:if test="string-length(interactiveFreeText/freeText) > 7">
				<OperationTimes>
					<xsl:apply-templates select="interactiveFreeText" mode="all">
						<xsl:with-param name="dep">
							<xsl:value-of select="$depd" />
						</xsl:with-param>
					</xsl:apply-templates>
					<xsl:apply-templates select="following-sibling::boardPointAndOffPointDetails[1]/interactiveFreeText" mode="allarr">
						<xsl:with-param name="arr">
							<xsl:value-of select="$arrd" />
						</xsl:with-param>
					</xsl:apply-templates>
				</OperationTimes>
			</xsl:if>
		</FlightLegInfo>
	</xsl:template>
	
	<xsl:template match="CAPI_ClassesInfo">
		<Meals>
			<xsl:attribute name="ClassOfService">
				<xsl:value-of select="Class" />
			</xsl:attribute>
			<xsl:value-of select="MealService" />
		</Meals>
	</xsl:template>
	
	<xsl:template match="departureDate| arrivalDate">
		<xsl:text>20</xsl:text>
		<xsl:value-of select="substring(string(.),5,2)" />
		<xsl:text>-</xsl:text>
		<xsl:value-of select="substring(string(.),3,2)" />
		<xsl:text>-</xsl:text>
		<xsl:value-of select="substring(string(.),1,2)" />
	</xsl:template>
	
	<xsl:template match="departureTime | arrivalTime">
	<xsl:text>T</xsl:text>
	<xsl:value-of select="substring(string(.),1,2)" />:<xsl:value-of select="substring(string(.),3,2)" />
	<xsl:text>:00</xsl:text>
	</xsl:template>
	
	<xsl:template match="freeText" mode="times">
	<xsl:value-of select="substring(string(.),4,2)" />:<xsl:value-of select="substring(string(.),6,2)" />
	<xsl:text>:00</xsl:text>
	</xsl:template>
	
	<xsl:template match="CAPI_Comments" mode="arr">
		<xsl:param name="airport" />
		<xsl:variable name="to">
			<xsl:value-of select="string('TO')" />
			<xsl:value-of select="$airport" />
		</xsl:variable>
		<xsl:if test="contains(Comment,'ARRIVES TERMINAL') and (contains(Comment,$to) or substring(Comment,4,4)=$airport)">
			<Terminal>
				<xsl:value-of select="substring-after(Comment,'ARRIVES TERMINAL ')" />
			</Terminal>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="CAPI_Comments" mode="dep">
		<xsl:param name="airport" />
		<xsl:variable name="from">
			<xsl:value-of select="string('FROM ')" />
			<xsl:value-of select="$airport" />
		</xsl:variable>
		<xsl:if test="contains(Comment,'DEPARTS TERMINAL') and (contains(Comment,$from) or substring(Comment,1,4)=$airport)">
			<Terminal>
				<xsl:value-of select="substring-after(Comment,'DEPARTS TERMINAL ')" />
			</Terminal>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="CAPI_Comments" mode="flt">
		<xsl:if test="contains(Comment,'ENTIRE FLT')">
			<MessageText>
				<xsl:value-of select="Comment" />
				<xsl:if test="substring(following-sibling::CAPI_Comments/Comment,1,12)='            '">
					<xsl:value-of select="substring(following-sibling::CAPI_Comments/Comment,12)" />
				</xsl:if>
			</MessageText>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="CAPI_Comments" mode="opc">
		<xsl:param name="cpair" />
		<xsl:if test="contains(Comment,'OPERATED BY') and (contains(Comment,$cpair) or contains(Comment,'ENTIRE FLT'))">
			<xsl:variable name="opc">
				<xsl:choose>
					<xsl:when test="boolean(substring-after(Comment,'OPERATED BY'))">
						<xsl:value-of select="substring-after(Comment,'OPERATED BY ')" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(following-sibling::CAPI_Comments/Comment,13)" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<CodeSharing>
				<AirlineCode></AirlineCode>
				<AirlineName>
					<xsl:value-of select="$opc" />
				</AirlineName>
			</CodeSharing>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="CAPI_Comments" mode="etk">
		<xsl:param name="airport" />
		<xsl:variable name="from">
			<xsl:value-of select="string('FROM ')" />
			<xsl:value-of select="$airport" />
		</xsl:variable>
		<xsl:if test="contains(Comment,'ELECTRONIC TKT CANDIDATE') and (contains(Comment,$from) or substring(Comment,1,4)=$airport)">
			<ElectronicTicketing>
				<xsl:choose>
					<xsl:when test="contains(Comment,'ET/')">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</ElectronicTicketing>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="freeText" mode="comment">
		<Comment>
			<xsl:value-of select="." />
		</Comment>
	</xsl:template>
	<xsl:template match="interactiveFreeText" mode="depest">
		<xsl:param name="dep" />
		<xsl:if test="substring(freeText,1,2)='ED' and substring(freeText,9)='ESTIMATED TIME OF DEPARTURE'">
			<xsl:attribute name="Estimated">
				<xsl:value-of select="$dep" />
				<xsl:text>T</xsl:text>
				<xsl:apply-templates select="freeText" mode="times" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="interactiveFreeText" mode="depact">
		<xsl:param name="dep" />
		<xsl:if test="substring(freeText,1,2)='AD' and substring(freeText,9)='LEFT THE GATE'">
			<xsl:attribute name="Actual">
				<xsl:value-of select="$dep" />
				<xsl:text>T</xsl:text>
				<xsl:apply-templates select="freeText" mode="times" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="interactiveFreeText" mode="all">
		<xsl:param name="dep" />
		<xsl:choose>
			<xsl:when test="substring(freeText,1,2)='ED' and substring(freeText,9)='ESTIMATED TIME OF DEPARTURE'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$dep" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Estimated</xsl:attribute>
				</OperationTime>
			</xsl:when>
			<xsl:when test="substring(freeText,1,2)='AD' and substring(freeText,9)='LEFT THE GATE'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$dep" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Actual</xsl:attribute>
				</OperationTime>
			</xsl:when>
			<xsl:when test="substring(freeText,1,2)='AD' and substring(freeText,9)='TOOK OFF'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$dep" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Actual</xsl:attribute>
				</OperationTime>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="substring(freeText,1,2)='DL' and substring(freeText,9,13)='PLANE IS LATE'">
			<OperationTime>
				<xsl:attribute name="Time"><xsl:value-of select="$dep" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
				<xsl:attribute name="OperationType">
					<xsl:value-of select="substring(freeText,9)" />
				</xsl:attribute>
				<xsl:attribute name="TimeType">Actual</xsl:attribute>
			</OperationTime>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="interactiveFreeText" mode="allarr">
		<xsl:param name="arr" />
		<xsl:choose>
			<xsl:when test="substring(freeText,1,2)='EA' and substring(freeText,9)='ESTIMATED TIME OF ARRIVAL'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$arr" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Estimated</xsl:attribute>
				</OperationTime>
			</xsl:when>
			<xsl:when test="substring(freeText,1,2)='AA' and substring(freeText,9)='ARRIVED'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$arr" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Actual</xsl:attribute>
				</OperationTime>
			</xsl:when>
			<xsl:when test="substring(freeText,1,2)='AA' and substring(freeText,9)='AIRCRAFT LANDED'">
				<OperationTime>
					<xsl:attribute name="Time"><xsl:value-of select="$arr" />T<xsl:apply-templates select="freeText" mode="times" /></xsl:attribute>
					<xsl:attribute name="OperationType">
						<xsl:value-of select="substring(freeText,9)" />
					</xsl:attribute>
					<xsl:attribute name="TimeType">Actual</xsl:attribute>
				</OperationTime>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="interactiveFreeText" mode="arrest">
		<xsl:param name="arr" />
		<xsl:if test="substring(freeText,1,2)='EA' and substring(freeText,9)='ESTIMATED TIME OF ARRIVAL'">
			<xsl:attribute name="Estimated">
				<xsl:value-of select="$arr" />
				<xsl:text>T</xsl:text>
				<xsl:apply-templates select="freeText" mode="times" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="interactiveFreeText" mode="arract">
		<xsl:param name="arr" />
		<xsl:if test="substring(freeText,1,2)='AA' and substring(freeText,9)='ARRIVED'">
			<xsl:attribute name="Actual">
				<xsl:value-of select="$arr" />
				<xsl:text>T</xsl:text>
				<xsl:apply-templates select="freeText" mode="times" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>