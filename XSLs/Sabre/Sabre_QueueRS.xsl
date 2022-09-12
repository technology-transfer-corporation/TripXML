<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Sabre_QueueRS.xsl 																-->
<!-- ================================================================== -->
<!-- Date: 28 Jan 2104 - Rastko - added Success tag								-->
<!-- Date: 10 Oct 2013 - Rastko - New file												-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<OTA_QueueRS Version="1.000">
		<xsl:apply-templates select="SabreCommandLLSRS" mode="list"/>
		<xsl:apply-templates select="QueueCountRS" mode="count"/>
		<xsl:apply-templates select="Queue_ListReply[not(errorReturn)]"/>
		<xsl:apply-templates select="QueuePlaceRS"/>
		<xsl:apply-templates select="Queue_ListReply/errorReturn"/>
		<xsl:apply-templates select="Queue_MoveItemReply/goodResponse" mode="move"/>
		<xsl:apply-templates select="Queue_MoveItemReply/errorReturn"/>
		<xsl:apply-templates select="Queue_RemoveItemReply/goodResponse" mode="remove"/>
		<xsl:apply-templates select="Queue_RemoveItemReply/errorReturn"/>
		<xsl:apply-templates select="MessagesOnly_Reply"/>
      <xsl:if test="*/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="*/ConversationID"/>
        </ConversationID>
      </xsl:if>
	</OTA_QueueRS>
</xsl:template>
		
<!-- Queue Count -->
<xsl:template match="QueueCountRS" mode="count">
	<Success/>
	<CountQueue>
		<xsl:attribute name="PseudoCityCode">
			<xsl:value-of select="QueueInfo/@PseudoCityCode"/>
		</xsl:attribute>
		<xsl:attribute name="CurrentDateTime">
			<xsl:value-of select="ApplicationResults/Success/@timeStamp"/>
		</xsl:attribute>
		<xsl:attribute name="TotalItems">
			<xsl:value-of select="translate(Totals[@Type='PNRS']/@Count,'.','')"/>
		</xsl:attribute>
		<Queues>
			<xsl:for-each select="QueueInfo/QueueIdentifier">
				<Queue>
					<xsl:attribute name="Number">
						<xsl:value-of select="@Number"/>
					</xsl:attribute>
					<xsl:attribute name="TotalQueueItems">
						<xsl:value-of select="translate(@Count,'.','')"/>
					</xsl:attribute>
				</Queue>
			</xsl:for-each>
		</Queues>
	</CountQueue>
</xsl:template>

<!-- Queue List -->
<xsl:template match="SabreCommandLLSRS" mode="list">
	<xsl:choose>
		<xsl:when test="contains(Response,'QUEUE LIST') and count(Response/Line) > 3">
			<Success/>
			<ListQueue>
				<xsl:attribute name="PseudoCityCode">
					<xsl:value-of select="substring-before(substring-after(Response,'QUEUE LIST FOR '),' ')"/>
				</xsl:attribute>
				<QueueCategory>
					<xsl:attribute name="QueueNumber">
						<xsl:value-of select="substring-before(substring-after(substring-after(Response,'QUEUE LIST FOR '),'QUEUE '),' ')"/>
					</xsl:attribute>
					<!--xsl:attribute name="CategoryNumber">
						<xsl:value-of select="queueView[1]/categoryDetails/subQueueInfoDetails/itemNumber"/>
					</xsl:attribute-->
					<!--xsl:attribute name="TotalItems"><xsl:value-of select="count(queueView/item)"/></xsl:attribute-->
					<QueueItems>
						<xsl:apply-templates select="Response/Line"/>
					</QueueItems>
				</QueueCategory>
			</ListQueue>
		</xsl:when>
		<xsl:when test="contains(Response,'QUEUE LIST') and count(Response/Line) = 3">
			<Errors>
				<Error Type="Sabre">
					<xsl:value-of select="'No PNR on the queue'"/>
				</Error>
			</Errors>
		</xsl:when>
		<xsl:otherwise>
			<Errors>
				<Error Type="Sabre">
					<xsl:value-of select="Response"/>
				</Error>
			</Errors>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<!-- Queue List Queues -->
<xsl:template match="Line">
	<xsl:variable name="a"><xsl:value-of select="translate(substring(.,1,4),' ','')"/></xsl:variable>
	<xsl:if test="$a>0">
		<Success/>
		<QueueItem>
			<xsl:attribute name="DateQueued">
				<xsl:call-template name="dateTime">
					<xsl:with-param name="dt" select="substring(.,35,14)"/>
				</xsl:call-template>
			</xsl:attribute>
			<UniqueID Type="16">
				<xsl:attribute name="ID">
					<xsl:value-of select="substring(.,7,6)"/>
				</xsl:attribute>
			</UniqueID>
		</QueueItem>
	</xsl:if>
</xsl:template>

<xsl:template name="dateTime">
	<xsl:param name="dt"/>
	<xsl:text>20</xsl:text>
	<xsl:value-of select="substring($dt,6,2)"/>
	<xsl:text>-</xsl:text>
	<xsl:call-template name="month">
		<xsl:with-param name="month" select="substring($dt,3,3)"/>
	</xsl:call-template>
	<xsl:text>-</xsl:text>
	<xsl:value-of select="substring($dt,1,2)"/>
	<xsl:text>T</xsl:text>
	<xsl:value-of select="substring($dt,11,2)"/>
	<xsl:text>:</xsl:text>
	<xsl:value-of select="substring($dt,13,2)"/>
	<xsl:text>:00</xsl:text>
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
	<Success/>
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
	<Success/>
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
<xsl:template match="Error">
	<Errors>
		<Error Type="Sabre">
			<xsl:attribute name="Code"><xsl:value-of select="SystemSpecificResults/Message/@code"/></xsl:attribute>
			<xsl:value-of select="SystemSpecificResults/Message"/>
		</Error>
		<Error Type="Sabre">
			<xsl:value-of select="SystemSpecificResults/ShortText"/>
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
<xsl:template match="QueuePlaceRS">
	<xsl:choose>
		<xsl:when test="Text='QUEUE PLACEMENT COMPLETED'">
			<Success/>
			<PlaceQueue>
				<UniqueID Type="16">
					<xsl:attribute name="ID"><xsl:value-of select="QueueInfo/UniqueID/@ID"/></xsl:attribute>
				</UniqueID>
			</PlaceQueue>
		</xsl:when>
		<xsl:otherwise>
			<Errors>
				<Error Type="Amadeus">
					<xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/Message"/>
				</Error>
			</Errors>
		</xsl:otherwise>
	</xsl:choose>
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
