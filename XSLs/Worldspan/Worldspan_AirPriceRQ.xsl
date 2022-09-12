<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_AirPriceRQ.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 28 Jan 2009 - Rastko														       -->
<!-- ================================================================== -->
<xsl:template match="/">
	<xsl:apply-templates select="OTA_AirPriceRQ"/>
</xsl:template>
<!--******************************************************************************************-->
<!--**                                 MAIN LINE              	                            **-->
<!--******************************************************************************************-->
<xsl:template match="OTA_AirPriceRQ">
	<BPC9>
		<MSG_VERSION>9</MSG_VERSION>
		<SEATS><xsl:value-of select="TravelerInfoSummary/SeatsRequested"/></SEATS>
		<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[1]">
			<xsl:with-param name="counter">1</xsl:with-param>
		</xsl:apply-templates>
		<PNR_DATA>                              
 				<PHONE_FIELDS>
   					<PHONE_FIELD>305-395-3933</PHONE_FIELD>             
   				</PHONE_FIELDS>
   		  		<RECEIVED_FROM>TRAVELTALK</RECEIVED_FROM>             
 		  	</PNR_DATA>
		  	<END_OPTION>I</END_OPTION>
		<PRICING_DISPLAY_OPTION>2</PRICING_DISPLAY_OPTION> 
		<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode != ''">
			<SECURATE_NUMBER><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode"/></SECURATE_NUMBER>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource = 'Published'">
				<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
				<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
				<PRICING_COMMAND></PRICING_COMMAND>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code !='' ">
				        	<PRICING_COMMAND>
				        		<xsl:text>4PQFSR/*</xsl:text>
				        		<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
				        	</PRICING_COMMAND>
						<PRICING_COMMAND>
							<xsl:text>4P*FSR/*</xsl:text>
							<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
						</PRICING_COMMAND>
						<PRICING_COMMAND>
							<xsl:text>4PQFSR/*</xsl:text>
							<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
						</PRICING_COMMAND>
					 </xsl:when>
					<xsl:otherwise> 
						<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
						<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
						<PRICING_COMMAND></PRICING_COMMAND>
						<PRICING_COMMAND>#FSR</PRICING_COMMAND>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
		<TICKET_OPTION>7T/</TICKET_OPTION>
		<LEG_COUNT>1</LEG_COUNT>
		<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"/>
	</BPC9>
</xsl:template>
<!--******************************************************************************************-->
<!--**                             PASSENGER DATA                                           **-->
<!--******************************************************************************************-->
<xsl:template match="PassengerTypeQuantity">
	<xsl:param name="counter" />
	<xsl:call-template name="create_each_pax_type">
		<xsl:with-param name="typeQ">
			<xsl:value-of select="@Quantity" />
		</xsl:with-param>
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter" />
		</xsl:with-param>
	</xsl:call-template>
	<xsl:apply-templates select="following-sibling::PassengerTypeQuantity[1]">
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter + @Quantity" />
		</xsl:with-param>
	</xsl:apply-templates>
</xsl:template>

<xsl:template name="create_each_pax_type">
	<xsl:param name="typeQ" />
	<xsl:param name="counter" />
	<xsl:if test="$typeQ != 0">
		<PASSENGER_DATA>
			<NAME_POSITION><xsl:value-of select="$counter"/></NAME_POSITION>
        		<LAST_FIRST_MIDDLE>LAST/FIRST.MIDDLE</LAST_FIRST_MIDDLE>  
			 <xsl:choose>
				<xsl:when test="@Code=CHD">
					<TYPE>CNN</TYPE>
     		   		<AGE>03</AGE>
        				<DOB>060102</DOB>
				</xsl:when>
				<xsl:when test="@Code='ADT'">
					<TYPE>ADT</TYPE>
				</xsl:when>
				<xsl:when test="@Code='YTH'">
					<TYPE>YTH</TYPE>
     		   		<AGE>15</AGE>
        				<DOB>940102</DOB>
				</xsl:when>
				<xsl:when test="@Code='INF'">
					<TYPE>INF</TYPE>
     		   		<AGE>00</AGE>
        				<DOB>090102</DOB>
				</xsl:when>
				<xsl:when test="@Code='INS'">
					<TYPE>INS</TYPE>
					<AGE>00</AGE>
        				<DOB>090102</DOB>
				</xsl:when>
				<xsl:when test="@Code='SRC'">
					<TYPE>SRC</TYPE>
				</xsl:when>
				<xsl:otherwise>
					<TYPE><xsl:value-of select="@Code"/></TYPE>
     			</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS'">
				<CUSTOM_NAME_DATA>00</CUSTOM_NAME_DATA>
			</xsl:if>
		</PASSENGER_DATA>
		<xsl:call-template name="create_each_pax_type">
			<xsl:with-param name="typeQ">
				<xsl:value-of select="$typeQ - 1" />
			</xsl:with-param>
			<xsl:with-param name="counter">
				<xsl:value-of select="$counter + 1" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:if>
</xsl:template>

<!--****************************************************************************-->
<!--   		                 Itinerary Section		                -->
<!--****************************************************************************-->
<xsl:template match="FlightSegment">
	<SEGMENT_INFO>
		<LEG_NUM>1</LEG_NUM> 
		<SEGMENT_INDICATOR>P</SEGMENT_INDICATOR> 
		<AVAIL_LOCATION>A</AVAIL_LOCATION> 
		<AIRLINE_CODE><xsl:value-of select="MarketingAirline/@Code" /></AIRLINE_CODE>
		<FLIGHT_NUM><xsl:value-of select="@FlightNumber" /></FLIGHT_NUM>
		<DEP_CLASS><xsl:value-of select="@ResBookDesigCode" /></DEP_CLASS>
	        <DEP_DATE>
	            <DEP_DAY><xsl:value-of select="substring(@DepartureDateTime,9,2)" /></DEP_DAY>   
	            <DEP_MONTH>
	            	<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
	            </DEP_MONTH>  
	        </DEP_DATE>
	        <DEP_TIME>
	            <DEP_HOUR><xsl:value-of select="substring(@DepartureDateTime,12,2)"/></DEP_HOUR>
	            <DEP_MIN><xsl:value-of select="substring(@DepartureDateTime,15,2)"/></DEP_MIN>
	        </DEP_TIME>
		<DEP_AIRPORT><xsl:value-of select="DepartureAirport/@LocationCode" /></DEP_AIRPORT>
		<ARRIVAL_DATE>
			<ARRIV_DAY><xsl:value-of select="substring(@ArrivalDateTime,9,2)" /></ARRIV_DAY>
			<ARRIV_MONTH>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(@ArrivalDateTime,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
			</ARRIV_MONTH>
		</ARRIVAL_DATE>
		<ARRIVAL_TIME>
			<ARRIV_HOUR><xsl:value-of select="substring(@ArrivalDateTime,12,2)"/></ARRIV_HOUR>
			<ARRIV_MIN><xsl:value-of select="substring(@ArrivalDateTime,15,2)"/></ARRIV_MIN>
		</ARRIVAL_TIME>
		<ARRIV_AIRPORT><xsl:value-of select="ArrivalAirport/@LocationCode" /></ARRIV_AIRPORT>
	</SEGMENT_INFO>
</xsl:template>

<xsl:template name="month">
	<xsl:param name="month" />
	<xsl:choose>
		<xsl:when test="$month = '01'">JAN</xsl:when>
		<xsl:when test="$month = '02'">FEB</xsl:when>
		<xsl:when test="$month = '03'">MAR</xsl:when>
		<xsl:when test="$month = '04'">APR</xsl:when>
		<xsl:when test="$month = '05'">MAY</xsl:when>
		<xsl:when test="$month = '06'">JUN</xsl:when>
		<xsl:when test="$month = '07'">JUL</xsl:when>
		<xsl:when test="$month = '08'">AUG</xsl:when>
		<xsl:when test="$month = '09'">SEP</xsl:when>
		<xsl:when test="$month = '10'">OCT</xsl:when>
		<xsl:when test="$month = '11'">NOV</xsl:when>
		<xsl:when test="$month = '12'">DEC</xsl:when>
	</xsl:choose>
</xsl:template>

</xsl:stylesheet> 
