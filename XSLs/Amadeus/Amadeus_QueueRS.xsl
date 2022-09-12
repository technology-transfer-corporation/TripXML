<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_QueueRS.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 30 Jul - new file - Rastko													-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<OTA_QueueRS Version="1.000">
		<xsl:apply-templates select="PoweredQueue_CountTotalReply/queueCountDisplay"/>
		<xsl:apply-templates select="PoweredQueue_ListReply[not(errorReturn)]"/>
		<xsl:apply-templates select="PoweredQueue_PlacePNRReply[recordLocator]"/>
		<xsl:apply-templates select="PoweredQueue_PlacePNRReply/errorReturn"/>
		<xsl:apply-templates select="PoweredQueue_CountTotalReply/errorReturn"/>
		<xsl:apply-templates select="PoweredQueue_ListReply/errorReturn"/>
		<xsl:apply-templates select="PoweredQueue_MoveItemReply/goodResponse" mode="move"/>
		<xsl:apply-templates select="PoweredQueue_MoveItemReply/errorReturn"/>
		<xsl:apply-templates select="PoweredQueue_RemoveItemReply/goodResponse" mode="remove"/>
		<xsl:apply-templates select="PoweredQueue_RemoveItemReply/errorReturn"/>
		<xsl:apply-templates select="MessagesOnly_Reply"/>
	</OTA_QueueRS>
</xsl:template>
		
<!-- Queue Count -->
<xsl:template match="queueCountDisplay">
	<CountQueue>
		<xsl:attribute name="PseudoCityCode">
			<xsl:value-of select="officeBeingDisplayed/originatorDetails/inHouseIdentification1"/>
		</xsl:attribute>
		<xsl:attribute name="CurrentDateTime">
			<xsl:apply-templates select="localDateAndTime/dateTime" mode="date"/>
		</xsl:attribute>
		<xsl:attribute name="TotalItems">
			<xsl:value-of select="sum(standardQueueCountDisplay/categoryAndCount/queueCount/queueDetails/numberOfItems)"/>
		</xsl:attribute>
		<Queues>
			<xsl:for-each select="standardQueueCountDisplay">
				<Queue>
					<xsl:attribute name="Number">
						<xsl:value-of select="queueNumber/queueDetails/number"/>
					</xsl:attribute>
					<xsl:attribute name="Name">
						<xsl:value-of select="queueName/referenceDetails/value"/>
					</xsl:attribute>
					<xsl:attribute name="TotalQueueItems">
						<xsl:value-of select="sum(categoryAndCount/queueCount/queueDetails/numberOfItems)"/>
					</xsl:attribute>
					<Categories>
						<xsl:for-each select="categoryAndCount">
							<Category>
								<xsl:attribute name="Number">
									<xsl:value-of select="categoryAndDateRange/subQueueInfoDetails/itemNumber"/>
								</xsl:attribute>
								<xsl:attribute name="TotalCategoryItems">
									<xsl:value-of select="queueCount/queueDetails/numberOfItems"/>
								</xsl:attribute>
							</Category>
						</xsl:for-each>
					</Categories>
				</Queue>
			</xsl:for-each>
		</Queues>
	</CountQueue>
</xsl:template>

<!-- Queue List -->
<xsl:template match="PoweredQueue_ListReply">
	<ListQueue>
		<xsl:attribute name="PseudoCityCode"><xsl:value-of select="queueView[1]/agent/originatorDetails/inHouseIdentification1"/></xsl:attribute>
		<QueueCategory>
			<xsl:attribute name="QueueNumber"><xsl:value-of select="queueView[1]/queueNumber/queueDetails/number"/></xsl:attribute>
			<!--xsl:attribute name="QueueName"></xsl:attribute-->
			<xsl:attribute name="CategoryNumber"><xsl:value-of select="queueView[1]/categoryDetails/subQueueInfoDetails/itemNumber"/></xsl:attribute>
			<xsl:attribute name="TotalItems"><xsl:value-of select="count(queueView/item)"/></xsl:attribute>
			<QueueItems>
				<xsl:apply-templates select="queueView/item"/>
			</QueueItems>
		</QueueCategory>
	</ListQueue>
</xsl:template>

<!-- Queue List Queues -->
<xsl:template match="item">
	<QueueItem>
		<xsl:if test="agent/originatorDetails/inHouseIdentification2 != ''">
			<xsl:attribute name="AgentCode"><xsl:value-of select="agent/originatorDetails/inHouseIdentification2"/></xsl:attribute>
		</xsl:if>
		<xsl:if test="pnrdates[timeMode='700']/dateTime != ''">
			<xsl:attribute name="DateQueued">
				<xsl:apply-templates select="pnrdates[timeMode='700']/dateTime" mode="date"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="pnrdates[timeMode='701']/dateTime != ''">
			<xsl:attribute name="TicketDate">
				<xsl:apply-templates select="pnrdates[timeMode='701']/dateTime" mode="date"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="pnrdates[timeMode='711']/dateTime != ''">
			<xsl:attribute name="DateTimeCreated">
				<xsl:apply-templates select="pnrdates[timeMode='711']/dateTime" mode="date"/>
			</xsl:attribute>
		</xsl:if>
		<UniqueID Type="16">
			<xsl:attribute name="ID">
				<xsl:value-of select="recLoc/reservation/controlNumber"/>
			</xsl:attribute>
		</UniqueID>
		<TravelerName><xsl:value-of select="paxName/paxDetails/surname"/></TravelerName>
		<xsl:if test="segment/flightDate/departureDate != ''">
			<Flight>
				<xsl:attribute name="DepartureDateTime">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(segment/flightDate/departureDate,6,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:call-template name="month">
						<xsl:with-param name="month">
							<xsl:value-of select="substring(segment/flightDate/departureDate,3,3)" />
						</xsl:with-param>
					</xsl:call-template>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(segment/flightDate/departureDate,1,2)"/>
					<xsl:text>T00:00:00</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="FlightNumber">
					<xsl:value-of select="segment/flightIdentification/flightNumber"/>
				</xsl:attribute>
				<DepartureAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="segment/boardPointDetails/trueLocation"/></xsl:attribute>
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="segment/offpointDetails/trueLocation"/></xsl:attribute>
				</ArrivalAirport>
				<MarketingAirline>
					<xsl:attribute name="Code"><xsl:value-of select="segment/companyDetails/marketingCompany"/></xsl:attribute>
				</MarketingAirline>
			</Flight>
		</xsl:if>
	</QueueItem>
</xsl:template>


<xsl:template match="dateTime" mode="date">
	<xsl:choose>
		<xsl:when test="string-length(year) = 4">
			<xsl:value-of select="year"/>
		</xsl:when>
		<xsl:when test="string-length(year) = 2">
			<xsl:text>20</xsl:text>
			<xsl:value-of select="year"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>200</xsl:text>
			<xsl:value-of select="year"/>
		</xsl:otherwise>	
	</xsl:choose>
	<xsl:text>-</xsl:text>
	<xsl:if test="string-length(month) = 1">
		<xsl:text>0</xsl:text>
	</xsl:if>
	<xsl:value-of select="month"/>
	<xsl:text>-</xsl:text>
	<xsl:if test="string-length(day) = 1">
		<xsl:text>0</xsl:text>
	</xsl:if>
	<xsl:value-of select="day"/>
	<xsl:if test="hour">
		<xsl:text>T</xsl:text>
		<xsl:if test="string-length(hour) = 1">
			<xsl:text>0</xsl:text>
		</xsl:if>
		<xsl:value-of select="hour"/>
		<xsl:text>:</xsl:text>
		<xsl:if test="string-length(minutes) = 1">
			<xsl:text>0</xsl:text>
		</xsl:if>
		<xsl:value-of select="minutes"/>
		<xsl:text>:00</xsl:text>
	</xsl:if>
</xsl:template>

<!-- Queue Move -->
<xsl:template match="goodResponse" mode="move">
	<Move>
		<NumberOfPNRS>
		</NumberOfPNRS>
		<From>
		</From>
		<To>
		</To>
		<Text>OK processed</Text>
	</Move>
</xsl:template>

<!-- Queue Remove -->
<xsl:template match="goodResponse" mode="remove">
	<Remove>
		<NumberOfPNRS>
		</NumberOfPNRS>
		<From>
		</From>
		<To>
		</To>
		<Text>OK processed</Text>
	</Remove>
</xsl:template>

<!-- errors -->
<xsl:template match="errorReturn">
	<Errors>
		<Error Type="Amadeus">
			<xsl:attribute name="Code"><xsl:value-of select="errorDefinition/errorDetails/errorCode"/></xsl:attribute>
			<xsl:choose>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '1'">Invalid date</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '360'">Invalid PNR file address</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '723'">Invalid category</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '727'">Invalid amount</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79A'">Invalid pseudo city code</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79B'">Already working another queue</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79C'">Not allowed to access queues for specified pseudo city code</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79D'">Queue identifier has not been assigned for specified pseudo city code</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79E'">Attempting to perform a queue function when not associated with a queue</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '79F'">Queue placement or add new queue item is not allowed for the specified pseudo city code and queue identifier</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '911'">Unable to process - system error</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '912'">Incomplete message - data missing in query</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '913'">Item/data not found or data not existing in processing host</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '914'">Invalid format/data - data does not match EDIFACT rules</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '915'">No action - processing host cannot support function</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '916'">EDIFACT version not supported</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '917'">EDIFACT message size exceeded</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '918'">Enter message in remarks</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '919'">No PNR in AAA</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '91A'">Inactive queue bank</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '91B'">Nickname not found</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '91C'">Invalid record locator</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '91D'">Invalid format</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '91F'">Invalid queue number</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '920'">Queue/date range empty</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '921'">Target not specified</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '922'">Targetted queue has wrong queue type</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '923'">Invalid time</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '924'">Invalid date range</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '925'">Queue number not specified</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '926'">Queue category empty</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '927'">No items exist</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '928'">Queue category not assigned</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '929'">No more items</xsl:when>
				<xsl:when test="errorDefinition/errorDetails/errorCode = '92A'">Queue category full</xsl:when>
			</xsl:choose>
		</Error>
	</Errors>
</xsl:template>

<!-- Queue Error -->
<xsl:template match="MessagesOnly_Reply">
	<Errors>
		<Error Type="Amadeus">
			<xsl:attribute name="Code"><xsl:value-of select="CAPI_Messages/ErrorCode"/></xsl:attribute>
			<xsl:value-of select="CAPI_Messages/Text"/>
		</Error>
	</Errors>
</xsl:template>
	
<!-- Queue Clean -->
<!--xsl:template match="">
</xsl:template-->

<!-- Queue Place -->
<xsl:template match="PoweredQueue_PlacePNRReply">
	<PlaceQueue>
		<UniqueID Type="16">
			<xsl:attribute name="ID"><xsl:value-of select="recordLocator/reservation/controlNumber"/></xsl:attribute>
		</UniqueID>
	</PlaceQueue>
</xsl:template>
	
<!-- Queue Exit -->
<!--xsl:template match="">
</xsl:template-->

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
