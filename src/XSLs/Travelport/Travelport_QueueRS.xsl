<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:terminal="http://www.travelport.com/schema/terminal_v8_0">
<!-- ================================================================== -->
<!-- TravelportWS_QueueRS.xsl 																							-->
<!-- ================================================================== -->
<!-- Date: 18 Aug 2014 - Rastko - new file																			-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<OTA_QueueRS Version="1.000">
		<xsl:apply-templates select="ListQueue/terminal:TerminalRsp[1]" mode="listqueue"/>
		<xsl:apply-templates select="PlaceQueue/terminal:TerminalRsp" mode="placequeue"/>
		<xsl:apply-templates select="RemoveQueue/terminal:TerminalRsp" mode="removequeue"/>
		<xsl:apply-templates select="Queue_CountTotalReply/queueCountDisplay"/>
		<xsl:apply-templates select="Queue_CountTotalReply/errorReturn"/>
		<xsl:apply-templates select="Queue_MoveItemReply/goodResponse" mode="move"/>
		<xsl:apply-templates select="Queue_MoveItemReply/errorReturn"/>
		<xsl:apply-templates select="Queue_RemoveItemReply/goodResponse" mode="remove"/>
		<xsl:apply-templates select="Queue_RemoveItemReply/errorReturn"/>
	</OTA_QueueRS>
</xsl:template>
		
<!-- Queue List -->
<xsl:template match="terminal:TerminalRsp" mode="listqueue">
	<xsl:choose>
		<xsl:when test="count(terminal:TerminalCommandResponse/terminal:Text)=1">
			<Errors>
				<Error>
					<xsl:value-of select="terminal:TerminalCommandResponse/terminal:Text"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:otherwise>
			<Success/>
			<ListQueue>
				<xsl:attribute name="PseudoCityCode"><xsl:value-of select="substring-before(terminal:TerminalCommandResponse/terminal:Text[1],' ')"/></xsl:attribute>
				<QueueCategory>
					<xsl:attribute name="QueueNumber"><xsl:value-of select="translate(substring-after(terminal:TerminalCommandResponse/terminal:Text[1],' Q'),' ','')"/></xsl:attribute>
					<xsl:attribute name="CategoryNumber"><xsl:value-of select="translate(substring(terminal:TerminalCommandResponse/terminal:Text[position()=5],12,2),' ','')"/></xsl:attribute>
					<xsl:attribute name="TotalItems"><xsl:value-of select="count(terminal:TerminalCommandResponse/terminal:Text) - 4"/></xsl:attribute>
					<QueueItems>
						<xsl:apply-templates select="terminal:TerminalCommandResponse/terminal:Text[position()>4][not(contains(.,'UTR PNR'))][contains(.,'   C ')]" mode="items"/>
						<xsl:apply-templates select="following-sibling::terminal:TerminalRsp/terminal:TerminalCommandResponse/terminal:Text[not(contains(.,'UTR PNR'))][contains(.,'   C ')]" mode="items"/>
					</QueueItems>
				</QueueCategory>
			</ListQueue>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<!-- Queue List Queues -->
<xsl:template match="terminal:Text" mode="items">
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
				<xsl:value-of select="substring(.,1,6)"/>
			</xsl:attribute>
		</UniqueID> 
		<TravelerName><xsl:value-of select="substring(.,33)"/></TravelerName>
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

<!-- Queue Place -->
<xsl:template match="terminal:TerminalRsp" mode="placequeue">
	<xsl:choose>
		<xsl:when test="contains(terminal:TerminalCommandResponse/terminal:Text,'INVALID') or contains(terminal:TerminalCommandResponse/terminal:Text,'INVLD ADDRESS') or contains(terminal:TerminalCommandResponse/terminal:Text,'SECURED PNR')">
			<Errors>
				<Error>
					<xsl:value-of select="'Cannot retrieve PNR'"/>
				</Error>
				<Error>
					<xsl:value-of select="terminal:TerminalCommandResponse/terminal:Text"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:when test="not(contains(terminal:TerminalCommandResponse/terminal:Text,' ON QUE'))">
			<Errors>
				<Error>
					<xsl:value-of select="'Cannot place PNR on queue'"/>
				</Error>
				<Error>
					<xsl:value-of select="terminal:TerminalCommandResponse/terminal:Text"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:otherwise>
			<Success/>
			<PlaceQueue>
				<UniqueID Type="16">
					<xsl:attribute name="ID"><xsl:value-of select="substring(terminal:TerminalCommandResponse/terminal:Text,0,6)"/></xsl:attribute>
				</UniqueID>
			</PlaceQueue>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<!-- Queue Remove -->
<xsl:template match="terminal:TerminalRsp" mode="removequeue">
	<xsl:choose>
		<xsl:when test="contains(terminal:TerminalCommandResponse/terminal:Text,'INVALID') or contains(terminal:TerminalCommandResponse/terminal:Text,'INVLD ADDRESS') or contains(terminal:TerminalCommandResponse/terminal:Text,'SECURED PNR')">
			<Errors>
				<Error>
					<xsl:value-of select="'Cannot retrieve PNR'"/>
				</Error>
				<Error>
					<xsl:value-of select="terminal:TerminalCommandResponse/terminal:Text"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:when test="not(contains(terminal:TerminalCommandResponse/terminal:Text,' REMOVED FROM'))">
			<Errors>
				<Error>
					<xsl:value-of select="'Cannot remove PNR from queue'"/>
				</Error>
				<Error>
					<xsl:value-of select="terminal:TerminalCommandResponse/terminal:Text"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:otherwise>
			<Success/>
			<Remove>
				<NumberOfPNRS>1</NumberOfPNRS>
				<From>
				</From>
				<To>
				</To>
				<Text>OK processed</Text>
			</Remove>
		</xsl:otherwise>
	</xsl:choose>
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
