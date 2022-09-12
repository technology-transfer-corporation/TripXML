<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="ref">0</xsl:variable>
	<xsl:variable name="refdate">12/05/09</xsl:variable>
	<xsl:template match="/">
		<SellRQ>
			<xsl:apply-templates select="Booking/TripXML/OTA_TravelItineraryRQ/OTA_AirBookRQ/AirItinerary/OriginDestinationOptions"/>
		</SellRQ>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOptions">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:apply-templates select="OriginDestinationOption">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOption">
		<xsl:param name="pos"/>
		<xsl:variable name="posod"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:apply-templates select="FlightSegment">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="posod"><xsl:value-of select="$posod"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="FlightSegment">
		<xsl:param name="pos"/>
		<xsl:param name="posod"/>
		<xsl:variable name="posfl"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="prevf">
			<xsl:value-of select="count(../../OriginDestinationOption[position() &lt; $posod]/FlightSegment)"/>
		</xsl:variable>
		<xsl:variable name="prevfl"><xsl:value-of select="$prevf + $posfl"/></xsl:variable>
		<Sell>
			<xsl:if test="$posod = 1 and $posfl = 1">
				<xsl:value-of select="../../../../../../../ConversationID"/>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:if test="$posod = 1 and $posfl = 1">
				<xsl:value-of select="$ref + $pos"/>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="MarketingAirline/@Code"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="@FlightNumber"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="DepartureAirport/@LocationCode"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="ArrivalAirport/@LocationCode"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="substring(@DepartureDateTime,6,2)"/>
			<xsl:text>/</xsl:text>
			<xsl:value-of select="substring(@DepartureDateTime,9,2)"/>
			<xsl:text>/</xsl:text>
			<xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
			<xsl:text>,</xsl:text>
			<xsl:variable name="ts">
				<xsl:value-of select="../../../../../../../TimeStamp"/>
			</xsl:variable>
			<xsl:if test="$posod = 1 and $posfl = 1">
				<xsl:value-of select="substring-before(substring-after(substring-after($ts,'/'),'/'),' ')"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(substring-before(substring-after($ts,'/'),'/'),'00')"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="format-number(substring-before($ts,'/'),'00')"/>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:if test="$posod = 1 and $posfl = 1">
				<xsl:value-of select="substring-after($ts,' ')"/>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:if test="$posod = 1 and $posfl = 1">
				<xsl:value-of select="../../../../../POS/Source/@PseudoCityCode"/>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:if test="$posfl = 1">
				<xsl:value-of select="DepartureAirport/@LocationCode"/>
				<xsl:text>-</xsl:text>
				<xsl:choose>
					<xsl:when test="count(../FlightSegment) > 1">
						<xsl:value-of select="following-sibling::FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="ArrivalAirport/@LocationCode"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<xsl:text>,,</xsl:text>
			<xsl:value-of select="@NumberInParty"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="@ResBookDesigCode"/>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="../../../../../../../Native/Air_SellFromRecommendationReply/itineraryDetails[position() = $posod]/segmentInformation[position() = $posfl]/actionDetails/statusCode"/>
		</Sell>
	</xsl:template>
</xsl:stylesheet>
