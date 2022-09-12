<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_TravelBuildRQ.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 20 Nov 2009 - Rastko														       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TravelBuild>
		<xsl:apply-templates select="OTA_TravelItineraryRQ"/>
	</TravelBuild>
</xsl:template>

<xsl:template match="OTA_TravelItineraryRQ">
	<TTBPC>
		<BPC9>
			<MSG_VERSION>9</MSG_VERSION>
			<SEATS><xsl:value-of select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment/@NumberInParty"/></SEATS>
			<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='ADT']" mode="adt"/>
			<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode!='ADT' and @PassengerTypeCode!='INF']" mode="pax"/>
			<PNR_DATA>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Telephone"/>
				<xsl:apply-templates select="POS/Source"/>
				<xsl:apply-templates select="TPA_Extensions/PNRData/CustomerIdentification"/>
				<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail"/>
				<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest"/>
			</PNR_DATA>
			<xsl:choose>
				<xsl:when test="TPA_Extensions/PNRData/Queue/@QueueNumber !='' ">
					<END_OPTION>Q</END_OPTION>
					<QUEUE_DATA>
						<QUEUE_LOCATION><xsl:value-of select="TPA_Extensions/PNRData/Queue/@PseudoCityCode"/></QUEUE_LOCATION>
						<QUEUE_NUMBER><xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueNumber"/></QUEUE_NUMBER>
						<QUEUE_CATEGORY><xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueCategory"/></QUEUE_CATEGORY>
					</QUEUE_DATA>
				</xsl:when>
				<xsl:otherwise>
					<END_OPTION>E</END_OPTION>
				</xsl:otherwise>
			</xsl:choose>
			<PRICING_DISPLAY_OPTION>2</PRICING_DISPLAY_OPTION> 
			<xsl:if test="TPA_Extensions/PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@SupplierCode != ''">
				<SECURATE_NUMBER>
					<xsl:value-of select="TPA_Extensions/PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@SupplierCode"/>
				</SECURATE_NUMBER>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="TPA_Extensions/PriceData/@PriceType = 'Published'">
					<xsl:apply-templates select="TPA_Extensions/PriceData/PublishedFares/FareRestrictPref"/> 
				</xsl:when>
				<xsl:when test="TPA_Extensions/PriceData/@PriceType = 'Private'">
					<xsl:choose>
						<xsl:when test="TPA_Extensions/PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/SupplierCode !='' ">
				          	<!--PRICING_COMMAND>4PLFBFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND-->
				          	<PRICING_COMMAND>4PQFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
							<PRICING_COMMAND>4P*FSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
							<PRICING_COMMAND>4PQFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
				     	</xsl:when>
						<xsl:otherwise> 
							<!--PRICING_COMMAND>4PLFBFSR</PRICING_COMMAND-->
							<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
							<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
							<PRICING_COMMAND></PRICING_COMMAND>
							<!--PRICING_COMMAND>4PQFSR</PRICING_COMMAND-->
							<PRICING_COMMAND>#FSR</PRICING_COMMAND>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<!--PRICING_COMMAND>4PLFBFSR</PRICING_COMMAND-->
					<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
					<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
					<!--PRICING_COMMAND>4PQFSR</PRICING_COMMAND-->
					<PRICING_COMMAND></PRICING_COMMAND>
					<PRICING_COMMAND>#FSR</PRICING_COMMAND>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="OTA_AirBookRQ/Ticketing">
					<xsl:apply-templates select="OTA_AirBookRQ/Ticketing"/>
				</xsl:when>
				<xsl:when test="TPA_Extensions/PNRData/Ticketing">
					<xsl:apply-templates select="TPA_Extensions/PNRData/Ticketing"/>
				</xsl:when>
				<xsl:otherwise>
					<TICKET_OPTION>7T/</TICKET_OPTION>
				</xsl:otherwise>
			</xsl:choose>
			<LEG_COUNT>1</LEG_COUNT>
			<!--LEG_COUNT><xsl:value-of select="count(OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment)"/>	</LEG_COUNT-->
			<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark[substring(.,1,5) = '*ST9 ']">
				<CK_TIME_PRICE_DIF>B</CK_TIME_PRICE_DIF>
				<PRICE_CK_AMOUNT>
					<xsl:variable name="amt">
						<xsl:value-of select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark[substring(.,1,5) = '*ST9 ']"/>
					</xsl:variable>
					<xsl:variable name="amt1"><xsl:value-of select="translate(substring-after($amt,'*ST9 '),'.','')"/></xsl:variable>
					<xsl:value-of select="$amt1"/>
				</PRICE_CK_AMOUNT>
			<PRICE_CK_NOD>2</PRICE_CK_NOD>
			</xsl:if>
			<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"/>
			<PRICING_PATH_SW>DN</PRICING_PATH_SW>
		</BPC9>
	</TTBPC>
	<xsl:if test="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS']">
		<TTUPC>
			<UPC7>
				<MSG_VERSION>7</MSG_VERSION>
				<VARIABLE_COUNT>
					<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS'])"/>
				</VARIABLE_COUNT>
				<PNR_RLOC></PNR_RLOC>
				<END_OPTION>E</END_OPTION>
				<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
				<VAR_DATA>
					<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS']" mode="ssr"/>
				</VAR_DATA>
			</UPC7>
		</TTUPC>
	</xsl:if>
	<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark != ''">
		<TTRMC>
			<RMC2>
				<MSG_VERSION>2</MSG_VERSION>
				<VARIABLE_COUNT><xsl:value-of select="count(OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark)"/></VARIABLE_COUNT>
				<PNR_RLOC></PNR_RLOC>
				<IGNORE_RMK_ERROR>I</IGNORE_RMK_ERROR>
				<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
				<END_OPTION>E</END_OPTION>
				<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark[. != '']">
					<REMARK>
						<REMARK_LINE><xsl:value-of select="."/></REMARK_LINE>
					</REMARK>
				</xsl:for-each>
			</RMC2>
		</TTRMC>
	</xsl:if>
	<TTDPC>
		<DPC8>
			<MSG_VERSION>8</MSG_VERSION>
			<REC_LOC></REC_LOC>
			<ETR_INF>Y</ETR_INF> 
  			<ALL_PNR_INF>Y</ALL_PNR_INF> 
  			<PRC_INF>Y</PRC_INF>
		</DPC8>
	</TTDPC>
</xsl:template>

<xsl:template match="Traveler" mode="adt">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<PASSENGER_DATA>
		<NAME_POSITION><xsl:value-of select="position()"/></NAME_POSITION>
		<LAST_FIRST_MIDDLE><xsl:value-of select="PersonName/Surname"/>/<xsl:value-of select="PersonName/GivenName"/></LAST_FIRST_MIDDLE>
		<TYPE><xsl:value-of select="@PassengerTypeCode"/></TYPE>
		<xsl:if test="Age != ''">
			<AGE><xsl:value-of select="Age"/></AGE>
		</xsl:if> 
		<xsl:if test="@BirthDate != ''">
			<DOB><xsl:value-of select="substring(@BirthDate,3,2)"/><xsl:value-of select="substring(@BirthDate,6,2)"/><xsl:value-of select="substring(@BirthDate,9,10)"/></DOB>
		</xsl:if>
		<xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS'">
			<CUSTOM_NAME_DATA>00</CUSTOM_NAME_DATA>
		</xsl:if>
	</PASSENGER_DATA>
	<xsl:if test="../Traveler[@PassengerTypeCode='INF']">
		<xsl:apply-templates select="../Traveler[@PassengerTypeCode='INF']" mode="inf">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:if>
</xsl:template>

<xsl:template match="Traveler" mode="inf">
	<xsl:param name="pos"/>
	<xsl:variable name="infpos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:if test="$infpos = $pos">
		<PASSENGER_DATA>
			<NAME_POSITION><xsl:value-of select="position()"/></NAME_POSITION>
			<LAST_FIRST_MIDDLE><xsl:value-of select="PersonName/Surname"/>/<xsl:value-of select="PersonName/GivenName"/></LAST_FIRST_MIDDLE>
			<TYPE>INF</TYPE>
			<AGE><xsl:value-of select="Age"/></AGE>
			<DOB>
				<xsl:value-of select="substring(@BirthDate,3,2)"/>
				<xsl:value-of select="substring(@BirthDate,6,2)"/>
				<xsl:value-of select="substring(@BirthDate,9,10)"/>
			</DOB>
			<CUSTOM_NAME_DATA>00</CUSTOM_NAME_DATA>
		</PASSENGER_DATA>
	</xsl:if>
</xsl:template>

<xsl:template match="Traveler" mode="pax">
	<PASSENGER_DATA>
		<NAME_POSITION><xsl:value-of select="position()"/></NAME_POSITION>
		<LAST_FIRST_MIDDLE><xsl:value-of select="PersonName/Surname"/>/<xsl:value-of select="PersonName/GivenName"/></LAST_FIRST_MIDDLE>
		<TYPE><xsl:value-of select="@PassengerTypeCode"/></TYPE>
		<xsl:if test="Age != ''">
			<AGE><xsl:value-of select="Age"/></AGE>
		</xsl:if> 
		<xsl:if test="@BirthDate != ''">
			<DOB><xsl:value-of select="substring(@BirthDate,3,2)"/><xsl:value-of select="substring(@BirthDate,6,2)"/><xsl:value-of select="substring(@BirthDate,9,10)"/></DOB>
		</xsl:if>
		<xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'INS'">
			<CUSTOM_NAME_DATA>00</CUSTOM_NAME_DATA>
		</xsl:if>
	</PASSENGER_DATA>
</xsl:template>

<xsl:template match="Traveler" mode="ssr">
	<SEGMENT_INFO>
		<SSR_DATA>
			<SSR_SEG_NUM>00</SSR_SEG_NUM>
			<PASSENGER_SSR_DATA>
				<SSR_NAME_NUM>N<xsl:value-of select="TravelerRefNumber/@RPH"/></SSR_NAME_NUM>
			</PASSENGER_SSR_DATA>
			<SSR_CODE>INFT</SSR_CODE>
			<SSR_FREEFORM>
				<xsl:text>-2.1/</xsl:text>
				<xsl:value-of select="Age"/>
				<xsl:text>MTHS</xsl:text>
			</SSR_FREEFORM>
		</SSR_DATA>
	</SEGMENT_INFO>
</xsl:template>

<xsl:template match="CustomerProfile">
	<xsl:if test="BusinessAccountRecord!=''">
		<WF_LEVEL1>N</WF_LEVEL1>
	</xsl:if>
	<WF_LEVEL2><xsl:value-of select="BusinessAccountRecord"/></WF_LEVEL2>
</xsl:template>

<xsl:template match="Telephone">
	<PHONE_FIELDS>
		<PHONE_FIELD><xsl:value-of select="@AreaCityCode" /><xsl:value-of select="@PhoneNumber" /></PHONE_FIELD>
	</PHONE_FIELDS>
</xsl:template>

<xsl:template match="Source">
	<RECEIVED_FROM>
		<xsl:choose>
			<xsl:when test="@AgentSine != ''"><xsl:value-of select="@AgentSine"/></xsl:when>
			<xsl:otherwise>TRAVELTALK</xsl:otherwise>
		</xsl:choose>
	</RECEIVED_FROM>
</xsl:template>

<xsl:template match="CustomerIdentification"> 
	<CUST_REF_NUM><xsl:value-of select="Text"/></CUST_REF_NUM>
</xsl:template>

<xsl:template match="PaymentDetail">
	<xsl:choose>
		<xsl:when test="PaymentCard">
			<FOP>CC</FOP>
			<CREDIT_CARD_DATA>
				<CREDIT_CARD_NO>
					<xsl:choose>
						<xsl:when test="PaymentCard/@CardCode = 'MC'">CA</xsl:when>
						<xsl:when test="PaymentCard/@CardCode = 'DN'">DC</xsl:when>
						<xsl:otherwise><xsl:value-of select="PaymentCard/@CardCode" /></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="PaymentCard/@CardNumber" />
				</CREDIT_CARD_NO>
				<EXPIRATION_DATE><xsl:value-of select="PaymentCard/@ExpireDate" /></EXPIRATION_DATE>
				<xsl:if test="PaymentCard/@SeriesCode">
					<CARD_ID><xsl:value-of select="PaymentCard/@SeriesCode"/></CARD_ID>
				</xsl:if>
			</CREDIT_CARD_DATA>		
		</xsl:when>
		<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
			<FOP>CA</FOP>
		</xsl:when>
		<xsl:when test="DirectBill/@DirectBill_ID='Check'">
			<FOP>CK</FOP>
		</xsl:when>
	</xsl:choose>
</xsl:template>

<xsl:template match="Remark">
	<REMARK><xsl:value-of select="."/></REMARK>
</xsl:template>

<xsl:template match="SpecialServiceRequest">
	<SSR_GENERIC>
		<SSR_CODE><xsl:value-of select="@SSRCode" /></SSR_CODE>
		<SSR_AIRLINE>
			<xsl:choose>
				<xsl:when test="Airline/@Code != ''"><xsl:value-of select="Airline/@Code" /></xsl:when>
				<xsl:otherwise>YY</xsl:otherwise>
			</xsl:choose>
		</SSR_AIRLINE>
		<xsl:if test="Text != ''">
			<SSR_FREEFORM><xsl:value-of select="Text"/></SSR_FREEFORM>
		</xsl:if>
	</SSR_GENERIC>
</xsl:template>

<xsl:template match="FareRestrictPref">
	<xsl:choose>
		<xsl:when test="AdvResTicketing/AdvReservation and not(VoluntaryChanges/Penalty)">
			<!--PRICING_COMMAND>4PLFBFSRNP</PRICING_COMMAND-->
			<PRICING_COMMAND>4PQFSRNP</PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSRNP</PRICING_COMMAND>
			<PRICING_COMMAND></PRICING_COMMAND>
			<!--PRICING_COMMAND>4PQFSRNP</PRICING_COMMAND-->
		</xsl:when> 
		<xsl:when test="VoluntaryChanges/Penalty and not(AdvResTicketing/AdvReservation)">
			<!--PRICING_COMMAND>4PLFBFSRNA</PRICING_COMMAND-->
			<PRICING_COMMAND>4PQFSRNA</PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSRNA</PRICING_COMMAND>
			<PRICING_COMMAND></PRICING_COMMAND>
			<!--PRICING_COMMAND>4PQFSRNA</PRICING_COMMAND-->
		</xsl:when> 
		<xsl:when test="not(VoluntaryChanges/Penalty) and not(AdvResTicketing/AdvReservation)">
			<!--PRICING_COMMAND>4PLFBFSRNANP</PRICING_COMMAND-->
			<PRICING_COMMAND>4PQFSRNANP</PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSRNANP</PRICING_COMMAND>
			<PRICING_COMMAND></PRICING_COMMAND>
			<!--PRICING_COMMAND>4PQFSRNANP</PRICING_COMMAND-->
		</xsl:when>
		<xsl:otherwise>
			<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
			<PRICING_COMMAND></PRICING_COMMAND>
			<!--PRICING_COMMAND>4PQFSR</PRICING_COMMAND-->
		</xsl:otherwise> 
	</xsl:choose>
</xsl:template>

<xsl:template match="NegotiatedFareCode">
	<xsl:choose>
		<xsl:when test="SupplierCode !='' ">
          	<!--PRICING_COMMAND>4PLFBFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND-->
          	<PRICING_COMMAND>4PQFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
			<PRICING_COMMAND>4PQFSR/*<xsl:value-of select="SupplierCode"/></PRICING_COMMAND>
     	</xsl:when>
		<xsl:otherwise> 
			<!--PRICING_COMMAND>4PLFBFSR</PRICING_COMMAND-->
			<PRICING_COMMAND>4PQFSR</PRICING_COMMAND>
			<PRICING_COMMAND>4P*FSR</PRICING_COMMAND>
			<PRICING_COMMAND></PRICING_COMMAND>
			<!--PRICING_COMMAND>4PQFSR</PRICING_COMMAND-->
			<PRICING_COMMAND>#FSR</PRICING_COMMAND>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="Ticketing">
		<TICKET_OPTION>7TAW</TICKET_OPTION>
		<TICKET_CAT>00</TICKET_CAT>
		<TICKET_DATE>
			<xsl:value-of select="substring(@TicketTimeLimit,9,2)" />
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(@TicketTimeLimit,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
		</TICKET_DATE> 
		<TICKET_FREEFORM><xsl:value-of select="substring(@TicketTimeLimit,12,2)"/><xsl:value-of select="substring(@TicketTimeLimit,15,2)"/></TICKET_FREEFORM>
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

<xsl:template match="FlightSegment">
	<SEGMENT_INFO>
		<LEG_NUM>1</LEG_NUM> 
		<SEGMENT_INDICATOR>P</SEGMENT_INDICATOR> 
		<AVAIL_LOCATION>A</AVAIL_LOCATION> 
		<!--AVAIL_INFO>
			<AVAIL_DEP_AIRPORT>
				<CITY_CODE><xsl:value-of select="DepartureAirport/@LocationCode" /></CITY_CODE>
			</AVAIL_DEP_AIRPORT>
			<AVAIL_AIRLINE><xsl:value-of select="MarketingAirline/@Code" /></AVAIL_AIRLINE>
			<AVAIL_DEP_CLASS><xsl:value-of select="@ResBookDesigCode" /></AVAIL_DEP_CLASS>
			<AVAIL_ARRIVAL_AIRPORT>
				<CITY_CODE><xsl:value-of select="ArrivalAirport/@LocationCode" /></CITY_CODE>
			</AVAIL_ARRIVAL_AIRPORT>
			<AVAIL_DEP_DAY><xsl:value-of select="substring(@DepartureDateTime,9,2)" /></AVAIL_DEP_DAY>
			<AVAIL_DEP_MONTH>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
			</AVAIL_DEP_MONTH>
			<AVAIL_DEP_TIME><xsl:value-of select="translate(substring(@DepartureDateTime,12,5),':','')"/></AVAIL_DEP_TIME>
			<AVAIL_ARRIVAL_TIME><xsl:value-of select="translate(substring(@ArrivalDateTime,12,5),':','')"/></AVAIL_ARRIVAL_TIME>
		</AVAIL_INFO-->
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
 
</xsl:stylesheet>


