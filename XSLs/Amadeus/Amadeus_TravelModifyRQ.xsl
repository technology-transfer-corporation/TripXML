<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_TravelModifyRQ.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 27 Feb 2007 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="pcc" select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
	<xsl:variable name="pricer">
		<xsl:choose>
			<xsl:when test="($pcc!='FMN1S28AA' and $pcc!='LAX1S21VW' and $pcc!='HOUG32101' and $pcc!='HOUG3210A' and $pcc!='MSPG32102' and $pcc!='MSPG32103' and $pcc!='MSPG32100' and $pcc!='DCA1S2182' and $pcc!='DCA1S2117' and $pcc!='LONU123LF' and $pcc!='LAXGO3100' and $pcc!='YTOGO3100' and $pcc!='BUF1S210L' and $pcc!='ATL1S21D9' and $pcc!='ATL1S21DG' and $pcc!='AVL1S2101' and $pcc!='CHI1S211S' and $pcc!='MKE1S2142' and $pcc!='DCA1S219D' and $pcc!='MCO1S213N') and OTA_TravelItineraryRQ/OTA_AirBookRQ">MP</xsl:when>
			<xsl:otherwise>VP</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:template match="/">
		<TravelModify>
			<xsl:apply-templates select="OTA_TravelModifyRQ" />
		</TravelModify>
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="OTA_TravelModifyRQ">
		<MultiElements>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<dataElementsMaster>
					<marker1>1</marker1>
					<!--xsl:apply-templates select="TPA_Extensions/PNRData/Telephone" />
					<xsl:apply-templates select="TPA_Extensions/PNRData/Ticketing" /-->
					<xsl:apply-templates select="POS/Source" />
					<!--xsl:apply-templates select="TPA_Extensions/PNRData/Email" />
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation"/-->
					<xsl:apply-templates select="OTA_AirBookModifyRQ/AirBookModifyRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark"/>	
					<!--xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark"/-->	
					<xsl:apply-templates select="OTA_AirBookModifyRQ/AirBookModifyRQ/Fulfillment/PaymentDetails/PaymentDetail"/>
					<xsl:apply-templates select="OTA_AirBookModifyRQ/AirBookModifyRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address" mode="billing"/>
					<!--xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest"/>
					<xsl:apply-templates select="TPA_Extensions/AgencyData/Commission"/>
					<xsl:apply-templates select="TPA_Extensions/PNRData/AccountingLine" />
					<xsl:apply-templates select="TPA_Extensions/PNRData/Queue" />
					<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '']" /-->
				</dataElementsMaster>
			</PoweredPNR_AddMultiElements>
		</MultiElements>
		<ET>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
				</pnrActions>
			</PoweredPNR_AddMultiElements>
		</ET>
	</xsl:template>
	
	<xsl:template name="psaList">
		<xsl:param name="ptypes"/>
		<xsl:param name="tst"/>
		<xsl:if test="string-length($ptypes) > 0">
			<psaList>
				<itemReference>
					<referenceType>TST</referenceType>
					<uniqueReference><xsl:value-of select="$tst"/></uniqueReference>
				</itemReference>
			</psaList>
			<xsl:call-template name="psaList">
				<xsl:with-param name="ptypes"><xsl:value-of select="substring($ptypes,4)"/></xsl:with-param>
				<xsl:with-param name="tst"><xsl:value-of select="$tst + 1"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Traveler" mode="storeprice">
		<xsl:param name="pt"/>
		<xsl:variable name="pt1">
			<xsl:value-of select="$pt"/>
			<xsl:if test="not(contains($pt,@PassengerTypeCode))">
				<xsl:value-of select="@PassengerTypeCode"/>
			</xsl:if>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="following-sibling::Traveler[1]">
				<xsl:apply-templates select="following-sibling::Traveler[@PassengerTypeCode!='INF'][1]" mode="storeprice">
					<xsl:with-param name="pt"><xsl:value-of select="$pt1"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="$pt1"/></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Name Elements 							 -->
	<!---********************************************************************-->
	<xsl:template match="Traveler" mode="nameadt">
		<xsl:param name="adt"/>
		<xsl:param name="inf"/>
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<travellerInfo>
			<elementManagementPassenger>
				<reference>
					<qualifier>PR</qualifier>
					<number>
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''"><xsl:value-of select="TravelerRefNumber/@RPH" /></xsl:when>
							<xsl:otherwise><xsl:value-of select="$pos"/></xsl:otherwise>
						</xsl:choose>
					</number>
				</reference>
				<segmentName>NM</segmentName>
			</elementManagementPassenger>
			<travellerInformation>
				<traveller>
					<surname>
						<xsl:value-of select="PersonName/Surname" />
					</surname>
					<quantity>
						<xsl:choose>
							<xsl:when test="$pos > $inf">1</xsl:when>
							<xsl:otherwise>2</xsl:otherwise>
						</xsl:choose>
					</quantity>
				</traveller>
				<passenger>
					<firstName>
						<xsl:value-of select="PersonName/GivenName" />
					</firstName>
					<type>
						<xsl:choose>
							<xsl:when test="@PassengerTypeCode='SRC'">YCD</xsl:when>
							<xsl:when test="@PassengerTypeCode='SCR'">YCD</xsl:when>
							<xsl:otherwise><xsl:value-of select="@PassengerTypeCode"/></xsl:otherwise>
						</xsl:choose>
					</type>
					<xsl:if test="$pos &lt; $inf or $pos = $inf">
						<infantIndicator>3</infantIndicator>
					</xsl:if>
				</passenger>
			</travellerInformation>
			<xsl:if test="$pos &lt; $inf or $pos = $inf">
				<travellerInformation>
					<traveller>
						<surname>
							<xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF'][position() = $pos]/PersonName/Surname" />
						</surname>
					</traveller>
					<passenger>
						<firstName>
							<xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF'][position() = $pos]/PersonName/GivenName" />
						</firstName>
						<type>INF</type>
					</passenger>
				</travellerInformation>
			</xsl:if>
		</travellerInfo>
	</xsl:template>
	<xsl:template match="Traveler" mode="namechd">
		<xsl:param name="adt"/>
		<xsl:variable name="pos1"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="pos"><xsl:value-of select="$pos1 + $adt"/></xsl:variable>
		<travellerInfo>
			<elementManagementPassenger>
				<reference>
					<qualifier>PR</qualifier>
					<number>
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''"><xsl:value-of select="TravelerRefNumber/@RPH" /></xsl:when>
							<xsl:otherwise><xsl:value-of select="$pos"/></xsl:otherwise>
						</xsl:choose>
					</number>
				</reference>
				<segmentName>NM</segmentName>
			</elementManagementPassenger>
			<travellerInformation>
				<traveller>
					<surname>
						<xsl:value-of select="PersonName/Surname" />
					</surname>
					<quantity>1</quantity>
				</traveller>
				<passenger>
					<firstName>
						<xsl:value-of select="PersonName/GivenName" />
					</firstName>
					<type>
						<xsl:value-of select="@PassengerTypeCode"/>
					</type>
				</passenger>
			</travellerInformation>
		</travellerInfo>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - Value Pricer						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOption" mode="VP">
		<originDestinationDetails>
			<originDestination>
				<origin><xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode" /></origin>
				<destination><xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode" /></destination>
			</originDestination>
			<xsl:apply-templates select="FlightSegment" mode="VP"/>
		</originDestinationDetails>
	</xsl:template>
	
	<xsl:template match="FlightSegment" mode="VP">
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)" />
		</xsl:variable>
		<itineraryInfo>
			<elementManagementItinerary>
				<reference>
					<qualifier>SR</qualifier>
					<number><xsl:value-of select="@RPH"/></number>
				</reference>
				<segmentName>AIR</segmentName>
			</elementManagementItinerary>
			<airAuxItinerary>
				<travelProduct>
					<product>
						<depDate>
							<xsl:value-of select="substring(string(@DepartureDateTime),9,2)" />
							<xsl:value-of select="substring(string(@DepartureDateTime),6,2)" />
							<xsl:value-of select="substring(string(@DepartureDateTime),3,2)" />
						</depDate>
					</product>
					<boardpointDetail>
						<cityCode><xsl:value-of select="DepartureAirport/@LocationCode" /></cityCode>
					</boardpointDetail>
					<offpointDetail>
						<cityCode><xsl:value-of select="ArrivalAirport/@LocationCode" /></cityCode>
					</offpointDetail>
					<company>
						<identification><xsl:value-of select="MarketingAirline/@Code" /></identification>
					</company>
					<productDetails>
						<identification><xsl:value-of select="@FlightNumber" /></identification>
						<classOfService><xsl:value-of select="@ResBookDesigCode" /></classOfService>
					</productDetails>
				</travelProduct>
				<messageAction>
					<business>
						<function>1</function>
					</business>
				</messageAction>
				<relatedProduct>
					<quantity><xsl:value-of select="@NumberInParty" /></quantity>
					<status>NN</status>
				</relatedProduct>
				<selectionDetailsAir>
					<selection>
						<option>P10</option>
					</selection>
				</selectionDetailsAir>
			</airAuxItinerary>
		</itineraryInfo>
	</xsl:template>
	
	<xsl:template match="FlightSegment" mode="MP">
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)" />
		</xsl:variable>
		<segmentInformation>
			<travelProductInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(string(@DepartureDateTime),9,2)" />
						<xsl:value-of select="substring(string(@DepartureDateTime),6,2)" />
						<xsl:value-of select="substring(string(@DepartureDateTime),3,2)" />
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId>
						<xsl:value-of select="DepartureAirport/@LocationCode" />
					</trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId>
						<xsl:value-of select="ArrivalAirport/@LocationCode" />
					</trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="MarketingAirline/@Code" />
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="@FlightNumber" />
					</flightNumber>
					<bookingClass>
						<xsl:value-of select="@ResBookDesigCode" />
					</bookingClass>
				</flightIdentification>
			</travelProductInformation>
			<relatedproductInformation>
				<quantity><xsl:value-of select="@NumberInParty" /></quantity>
				<statusCode>NN</statusCode>
			</relatedproductInformation>
		</segmentInformation>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - Master Pricer						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOption" mode="MP">
		<itineraryDetails>
			<originDestinationDetails>
				<origin><xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode" /></origin>
				<destination><xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode" /></destination>
			</originDestinationDetails>
			<!--message>
				<messageFunctionDetails>
					<messageFunction>183</messageFunction>
				</messageFunctionDetails>
			</message-->
			<xsl:apply-templates select="FlightSegment" mode="MP"/>
		</itineraryDetails>
	</xsl:template>

	<!---**********************************************************************************-->
	<!--     Data Elements 					                    	       -->
	<!---**********************************************************************************-->
	<!--         		        Phone fields				 	       -->
	<!---**********************************************************************************-->
	<xsl:template match="Telephone">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>AP</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
					<xsl:choose>
						<xsl:when test="@PhoneLocationType='Business'">
							<type>3</type>
						</xsl:when>
						<xsl:when test="@PhoneLocationType='Home'">
							<type>4</type>
						</xsl:when>
						<xsl:when test="@PhoneLocationType='Agency'">
							<type>6</type>
						</xsl:when>
						<xsl:when test="@PhoneLocationType='Fax'">
							<type>P01</type>
						</xsl:when>
						<xsl:otherwise>
							<type>5</type>
						</xsl:otherwise>
					</xsl:choose>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="@AreaCityCode" />
					<xsl:value-of select="@PhoneNumber" />
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<!---**********************************************************************************-->
	<!--     Data Elements 					                    	       -->
	<!---**********************************************************************************-->
	<!--         		        email fields				 	       -->
	<!---**********************************************************************************-->
	<xsl:template match="Email">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>AP</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
					<type>P02</type>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="." />
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>

	<!-- *********************************************************************** -->
	<!--         		       Ticketing Fields			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Ticketing">
		<xsl:variable name="TktTime">
			<xsl:value-of select="substring-after(@TicketTimeLimit,'T')" />
		</xsl:variable>
		<xsl:variable name="TktTime2">
			<xsl:value-of select="substring(string($TktTime),1,5)" />
		</xsl:variable>
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>TK</segmentName>
			</elementManagementData>
			<ticketElement>
				<ticket>
					<xsl:choose>
						<xsl:when test ="TicketAdvisory = 'OK'">
							<indicator>OK</indicator>
						</xsl:when>
						<xsl:when test="@TicketTimeLimit">
							<indicator>
								<xsl:choose>
									<xsl:when test="TicketAdvisory = 'XL'">XL</xsl:when>
									<xsl:when test="TicketAdvisory = 'TL'">TL</xsl:when>
									<xsl:otherwise>TL</xsl:otherwise>
								</xsl:choose>
							</indicator>
							<date>
								<xsl:value-of select="substring(string(@TicketTimeLimit),9,2)" />
								<xsl:value-of select="substring(string(@TicketTimeLimit),6,2)" />
								<xsl:value-of select="substring(string(@TicketTimeLimit),3,2)" />
							</date>
							<xsl:choose>
								<xsl:when test="$TktTime2 = '0000'">
									<time>
										<xsl:text>2400</xsl:text>
									</time>
								</xsl:when>
								<xsl:otherwise>
									<time>
										<xsl:value-of select="translate(string($TktTime2),':','')" />
									</time>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>
				</ticket>
			</ticketElement>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
<!--         		       SSR			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "SpecialServiceRequest" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>SSR</segmentName> 	
		</elementManagementData>                                  
 		<serviceRequest>
			<ssr>
				<type><xsl:value-of select="@SSRCode"/></type>
				<status>NN</status>
				<quantity>
					<xsl:choose>
						<xsl:when test="@TravelerRefNumberRPHList!=''">
							<xsl:value-of select="string-length(@TravelerRefNumberRPHList)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="count(../../../../../TPA_Extensions/PNRData/Traveler)"/>
						</xsl:otherwise>
					</xsl:choose>				
				</quantity>
				<companyId><xsl:value-of select="Airline/@Code"/></companyId>
				<xsl:if test="Text!=''">
					<freetext><xsl:value-of select="Text"/></freetext>
				</xsl:if>
			</ssr>
		</serviceRequest>  
		<xsl:if test="@TravelerRefNumberRPHList != '' or @FlightRefNumberRPHList != ''">
			<referenceForDataElement>
				<xsl:if test="@TravelerRefNumberRPHList != ''">
					<xsl:call-template name="RPH">
						<xsl:with-param name="RPH"><xsl:value-of select="@TravelerRefNumberRPHList"/></xsl:with-param>
						<xsl:with-param name="Type"><xsl:text>P</xsl:text></xsl:with-param>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="@FlightRefNumberRPHList != ''">
					<xsl:call-template name="RPH">
						<xsl:with-param name="RPH"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:with-param>
						<xsl:with-param name="Type"><xsl:text>S</xsl:text></xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</referenceForDataElement>
		</xsl:if>             
	</dataElementsIndiv>
</xsl:template>

<xsl:template name="RPH">
	<xsl:param name="RPH"/>
	<xsl:param name="Type"/>
	<xsl:if test="string-length($RPH) != 0">
		<xsl:variable name="tRPH"><xsl:value-of select="substring($RPH,1,1)"/></xsl:variable>
		<reference>
			<qualifier>
				<xsl:value-of select="$Type"/>
				<xsl:choose>
					<xsl:when test="$Type='S' and $pricer='MP'">T</xsl:when>
					<xsl:otherwise>R</xsl:otherwise>
				</xsl:choose>
			</qualifier>
			<number><xsl:value-of select="$tRPH"/></number>
		</reference>
		<xsl:call-template name="RPH">
			<xsl:with-param name="RPH"><xsl:value-of select="substring($RPH,2)"/></xsl:with-param>
			<xsl:with-param name="Type"><xsl:value-of select="$Type"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		       Seat request			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "SeatRequest" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>STR</segmentName> 	
		</elementManagementData>                                  
 		<seatRequest>
 			<xsl:if test="@SeatNumber = '' and @SeatPreference=''">
				<seat>
					<type>RQST</type>
				</seat>
			</xsl:if>
			<xsl:if test="@SeatNumber != '' or @SeatPreference != ''">
				 <special>
				 	<xsl:if test="@SeatNumber != ''">
				 		<data><xsl:value-of select="@SeatNumber"/></data>
				 	</xsl:if>
				 	<xsl:if test="@SeatPreference != ''">
					 	<seatType><xsl:value-of select="@SeatPreference"/></seatType>
					 </xsl:if>
				</special> 
			</xsl:if>
		</seatRequest>  
		<xsl:if test="@TravelerRefNumberRPHList != '' or @FlightRefNumberRPHList != ''">
			<referenceForDataElement>
				<xsl:if test="@TravelerRefNumberRPHList != ''">
					<xsl:call-template name="RPH">
						<xsl:with-param name="RPH"><xsl:value-of select="@TravelerRefNumberRPHList"/></xsl:with-param>
						<xsl:with-param name="Type"><xsl:text>P</xsl:text></xsl:with-param>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="@FlightRefNumberRPHList != ''">
					<xsl:call-template name="RPH">
						<xsl:with-param name="RPH"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:with-param>
						<xsl:with-param name="Type"><xsl:text>S</xsl:text></xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</referenceForDataElement>
		</xsl:if>             
	</dataElementsIndiv>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		       OSI			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "OtherServiceInformation" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>OS</segmentName> 	
		</elementManagementData>                                  
 		<freetextData>   
			<freetextDetail>
				<subjectQualifier>3</subjectQualifier>
				<type>28</type>
				<companyId><xsl:value-of select="Airline/@Code"/></companyId>
			</freetextDetail>
			<longFreetext><xsl:value-of select="Text"/></longFreetext>
		</freetextData>               
	</dataElementsIndiv>
</xsl:template>
<!-- *********************************************************************** -->
<!--         		       General remark			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "Remark" >
	<xsl:if test=". != ''">
		 <dataElementsIndiv> 
			<elementManagementData>
				<segmentName>RM</segmentName> 	
			</elementManagementData>                                  
	 		<miscellaneousRemark>
		   		<remarks>
		   			<type>RM</type>
		   	   		<freetext><xsl:value-of select="."/></freetext> 
		   		</remarks>
		   </miscellaneousRemark>              
		</dataElementsIndiv>
	</xsl:if>
</xsl:template>
<!-- *********************************************************************** -->
<!--         		       Special remark			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "SpecialRemark">
	<xsl:if test=". != ''">
		 <dataElementsIndiv> 
			<elementManagementData>
				<segmentName>RI</segmentName> 	
			</elementManagementData>                                  
	 		<miscellaneousRemark>
		   		<remarks>
		   			<type>RI</type>
		   			<category><xsl:value-of select="@RemarkType"/></category>
		   	   		<freetext><xsl:value-of select="Text"/></freetext> 
		   		</remarks>
		   </miscellaneousRemark>              
		</dataElementsIndiv>
	</xsl:if>
</xsl:template>
<!-- *********************************************************************** -->
<!--         		      Form of payment		 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "PaymentDetail" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>FP</segmentName> 	
		</elementManagementData>                                  
 		<formOfPayment>
 			<fop>
 				<xsl:choose>
					<xsl:when test="PaymentCard">
						<identification>CC</identification>
						<creditCardCode>
							<xsl:choose>
								<xsl:when test="PaymentCard/@CardCode = 'MC'">CA</xsl:when>
								<xsl:otherwise><xsl:value-of select="PaymentCard/@CardCode" /></xsl:otherwise>
							</xsl:choose>
						</creditCardCode>
						<accountNumber>
							<xsl:value-of select="PaymentCard/@CardNumber" />
						</accountNumber>
						<expiryDate>
							<xsl:value-of select="PaymentCard/@ExpireDate" />
						</expiryDate>
					</xsl:when>
					<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
						<identification>CA</identification>
					</xsl:when>
					<xsl:otherwise>
						<identification>MS</identification>
					</xsl:otherwise>
				</xsl:choose>
			</fop>
		</formOfPayment>
	</dataElementsIndiv>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		      Billing address					 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "Address" mode="billing">
	<xsl:choose>
		<xsl:when test="AddressLine != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>ABU</segmentName>
				</elementManagementData>
				<freetextData>
					<freetextDetail>
						<subjectQualifier>3</subjectQualifier>
					</freetextDetail>
					<longFreetext>
						<xsl:value-of select="AddressLine"/>
					</longFreetext>
				</freetextData>
			</dataElementsIndiv>
		</xsl:when>
		<xsl:when test="@FormattedInd = 'true'">
			<dataElementsIndiv> 
				<elementManagementData>
					<segmentName>AB</segmentName> 	
				</elementManagementData>                                  
		 		<structuredAddress>
					<informationType>2</informationType>
					 <address>        
					 	<optionA1>A1</optionA1>         
					 	<optionTextA1><xsl:value-of select="StreetNmbr"/></optionTextA1>      
					 </address> 		
					 <optionalData>        
					 	<option>CI</option>         
					 	<optionText><xsl:value-of select="CityName"/></optionText>      
					 </optionalData> 
					 <optionalData>        
					 	<option>ZP</option>         
					 	<optionText><xsl:value-of select="PostalCode"/></optionText>      
					 </optionalData>
					 <xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
					 	<optionalData>        
						 	<option>ST</option>         
						 	<optionText>
						 		<xsl:choose>
									<xsl:when test="StateProv/@StateCode != ''"><xsl:value-of select="StateProv/@StateCode"/></xsl:when>
									<xsl:otherwise><xsl:value-of select="StateProv"/></xsl:otherwise>
								</xsl:choose>
						 	</optionText>      
						 </optionalData>
					 </xsl:if>
					 <optionalData>        
					 	<option>CO</option>         
					 	<optionText>
					 		<xsl:choose>
								<xsl:when test="CountryName != ''"><xsl:value-of select="CountryName"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="CountryName/@Code"/></xsl:otherwise>
							</xsl:choose>
					 	</optionText>      
					 </optionalData>
				</structuredAddress>
			</dataElementsIndiv>
		</xsl:when>
		<xsl:otherwise>
			<xsl:apply-templates select="." mode="pax"/>
			<xsl:apply-templates select="." mode="street"/>
			<xsl:apply-templates select="." mode="city"/>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match = "Address" mode="pax">
	<xsl:if test="../CardHolderName != ''">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>ABU</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
				</freetextDetail>
				<longFreetext><xsl:value-of select="../CardHolderName"/></longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:if>
</xsl:template>

<xsl:template match = "Address" mode="street">
	<dataElementsIndiv>
		<elementManagementData>
			<segmentName>ABU</segmentName>
		</elementManagementData>
		<freetextData>
			<freetextDetail>
				<subjectQualifier>3</subjectQualifier>
			</freetextDetail>
			<longFreetext><xsl:value-of select="StreetNmbr"/></longFreetext>
		</freetextData>
	</dataElementsIndiv>
</xsl:template>

<xsl:template match = "Address" mode="city">
	<dataElementsIndiv>
		<elementManagementData>
			<segmentName>ABU</segmentName>
		</elementManagementData>
		<freetextData>
			<freetextDetail>
				<subjectQualifier>3</subjectQualifier>
			</freetextDetail>
			<longFreetext>
				<xsl:value-of select="CityName"/>
				<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
					<xsl:text> </xsl:text>
			 		<xsl:choose>
						<xsl:when test="StateProv/@StateCode != ''"><xsl:value-of select="StateProv/@StateCode"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="StateProv"/></xsl:otherwise>
					</xsl:choose>
				 </xsl:if>
				 <xsl:text> </xsl:text>
				 <xsl:value-of select="PostalCode"/>
				 <xsl:if test="CountryName != '' or CountryName/@Code != ''">
				 	<xsl:text> </xsl:text>
		 			<xsl:choose>
						<xsl:when test="CountryName != ''"><xsl:value-of select="CountryName"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="CountryName/@Code"/></xsl:otherwise>
					</xsl:choose>
			 	</xsl:if>
			</longFreetext>
		</freetextData>
	</dataElementsIndiv>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		      Delivery address					 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "DeliveryAddress" >
	<xsl:choose>
		<xsl:when test="AddressLine != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>AMU</segmentName>
				</elementManagementData>
				<freetextData>
					<freetextDetail>
						<subjectQualifier>3</subjectQualifier>
					</freetextDetail>
					<longFreetext>
						<xsl:value-of select="AddressLine"/>
					</longFreetext>
				</freetextData>
			</dataElementsIndiv>
		</xsl:when>
		<xsl:otherwise>
			 <dataElementsIndiv> 
				<elementManagementData>
					<segmentName>AM</segmentName> 	
				</elementManagementData>                                  
		 		<structuredAddress>
					<informationType>P25</informationType>
					 <address>        
					 	<optionA1>A1</optionA1>         
					 	<optionTextA1><xsl:value-of select="StreetNmbr"/></optionTextA1>      
					 </address> 		
					 <xsl:if test="../Name != ''">
					 	 <optionalData>        
						 	<option>NA</option>         
						 	<optionText><xsl:value-of select="../Name"/></optionText>      
						 </optionalData>
					 </xsl:if>
					 <optionalData>        
					 	<option>CI</option>         
					 	<optionText><xsl:value-of select="CityName"/></optionText>      
					 </optionalData> 
					 <optionalData>        
					 	<option>ZP</option>         
					 	<optionText><xsl:value-of select="PostalCode"/></optionText>      
					 </optionalData>
					 <xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
					 	<optionalData>        
						 	<option>ST</option>         
						 	<optionText>
						 		<xsl:choose>
									<xsl:when test="StateProv/@StateCode != ''"><xsl:value-of select="StateProv/@StateCode"/></xsl:when>
									<xsl:otherwise><xsl:value-of select="StateProv"/></xsl:otherwise>
								</xsl:choose>
						 	</optionText>      
						 </optionalData>
					 </xsl:if>
					 <xsl:if test="CountryName != '' or CountryName/@Code != ''">
						 <optionalData>        
						 	<option>CO</option>         
						 	<optionText>
						 		<xsl:choose>
									<xsl:when test="CountryName != ''"><xsl:value-of select="CountryName"/></xsl:when>
									<xsl:otherwise><xsl:value-of select="CountryName/@Code"/></xsl:otherwise>
								</xsl:choose>
						 	</optionText>      
						 </optionalData>
					 </xsl:if>
				</structuredAddress>
			</dataElementsIndiv>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		     Agency commission fee		 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "Commission" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>FM</segmentName> 	
		</elementManagementData>                                  
 		<commission>
 			<commissionInfo> 
				<percentage>
					<xsl:value-of select="@Percent"/>
				</percentage>          
 			</commissionInfo>		
 		</commission>
	</dataElementsIndiv>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		     Accointing Information		 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match="AccountingLine">
	<xsl:if test=". != ''">
		<dataElementsIndiv> 
			<elementManagementData>
				<segmentName>AI</segmentName> 	
			</elementManagementData>                                  
	 		<accounting>
				<account>
					<number><xsl:value-of select="translate(string(.),'-','')"/></number>
				</account>
			</accounting>
		</dataElementsIndiv>
	</xsl:if>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		     Queue Information		 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match="Queue">
	<dataElementsIndiv> 
		<elementManagementData>
			<segmentName>OP</segmentName> 	
		</elementManagementData>                                  
 		<optionElement>
    			<optionDetail>
    				<officeId><xsl:value-of select="@PseudoCityCode" /></officeId>
    				<date></date>
    				<queue><xsl:value-of select="@QueueNumber" /></queue>
    				<category><xsl:value-of select="@QueueCategory" /></category>
    				<freetext></freetext>
    			</optionDetail>
    		</optionElement>
	</dataElementsIndiv>
</xsl:template>

<!-- *********************************************************************** -->
<!--         		     Frequent Flyer Information		 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match="CustLoyalty">
	<dataElementsIndiv> 
		<elementManagementData>
			<segmentName>SSR</segmentName> 	
		</elementManagementData>      
		<serviceRequest>
			<ssr>
				<type>FQTV</type>
				<companyId><xsl:value-of select="@ProgramID"/></companyId>
				<indicator>P01</indicator>
			</ssr>
		</serviceRequest>                            
 		<frequentTravellerData>
 			<frequentTraveller>
 				<companyId><xsl:value-of select="@ProgramID"/></companyId>
 				<membershipNumber>
 					<xsl:value-of select="@ProgramID"/>
 					<xsl:value-of select="@MembershipID"/>
 				</membershipNumber>
 			</frequentTraveller>
 		</frequentTravellerData>
 		<xsl:if test="../TravelerRefNumber/@RPH != ''">
	 		<referenceForDataElement>
	 			<reference>
	 				<qualifier>PR</qualifier>
	 				<number><xsl:value-of select="../TravelerRefNumber/@RPH"/></number>
	 			</reference>
	 		</referenceForDataElement>
	 	</xsl:if>
	</dataElementsIndiv>
</xsl:template>

	<!-- *********************************************************************** -->
	<!--         		     Received From				     -->
	<!-- *********************************************************************** -->
	<xsl:template match="Source">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>RF</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
					<type>P22</type>
				</freetextDetail>
				<longFreetext>
					<xsl:choose>
						<xsl:when test="@AgentSine != ''"><xsl:value-of select="@AgentSine"/></xsl:when>
						<xsl:otherwise>TRAVELTALK</xsl:otherwise>
					</xsl:choose>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<xsl:template match="VehResRQCore">
		<PoweredCar_Sell>
			<pnrInfo>
				<paxTattooNbr>
					<referenceDetails>
						<type>PT</type>
						<value>X</value> <!-- real value will be added by business logic -->
					</referenceDetails>
				</paxTattooNbr>
			</pnrInfo>
			<sellData>
				<companyIdentification>
					<travelSector>CAR</travelSector>
					<companyCode>
						<xsl:value-of select="VendorPref/@Code" />
					</companyCode>
					<xsl:if test="VendorPref/@CompanyShortName !=''">
						<companyName>
							<xsl:value-of select="VendorPref/@CompanyShortName" />
						</companyName>
					</xsl:if>
					<xsl:if test="VendorPref/@CodeContext !=''">
						<accessLevel>
							<xsl:choose>
								<xsl:when test="VendorPref/@CodeContext = 'Complete Access'">CA</xsl:when>
								<xsl:when test="VendorPref/@CodeContext = 'Complete Access Plus'">CP</xsl:when>
								<xsl:when test="VendorPref/@CodeContext = 'Standard Access'">SA</xsl:when>
								<xsl:otherwise><xsl:value-of select="VendorPref/@CodeContext"/></xsl:otherwise>
							</xsl:choose>
						</accessLevel>
					</xsl:if>
				</companyIdentification>
				<locationInfo>
					<locationType>176</locationType>
					<locationDescription>
						<code>1A</code>
						<name>
							<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode" />
						</name>
					</locationDescription>
				</locationInfo>
				<locationInfo>
					<locationType>DOL</locationType>
					<locationDescription>
						<code>1A</code>
						<name>
							<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode" />
						</name>
					</locationDescription>
				</locationInfo>
				<pickupDropoffTimes>
					<beginDateTime>
						<year>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),1,4)" />
						</year>
						<month>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),6,2)" />
						</month>
						<day>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),9,2)" />
						</day>
						<hour>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),12,2)" />
						</hour>
						<minutes>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),15,2)" />
						</minutes>
					</beginDateTime>
					<endDateTime>
						<year>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),1,4)" />
						</year>
						<month>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),6,2)" />
						</month>
						<day>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),9,2)" />
						</day>
						<hour>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),12,2)" />
						</hour>
						<minutes>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),15,2)" />
						</minutes>
					</endDateTime>
				</pickupDropoffTimes>
				<vehicleInformation>
					<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
					<vehicleRentalNeedType>
						<vehicleTypeOwner>ACR</vehicleTypeOwner>
						<vehicleRentalPrefType>
							<xsl:value-of select="VehPref/VehType/@VehicleCategory" />
						</vehicleRentalPrefType>
					</vehicleRentalNeedType>
				</vehicleInformation>
				<xsl:if test="../VehResRQInfo/ArrivalDetails">
					<arrivalInfo>
						<inboundCarrierDetails>
							<carrier>
								<xsl:value-of select="../VehResRQInfo/ArrivalDetails/MarketingCompany/@Code" />
							</carrier>
						</inboundCarrierDetails>
						<inboundFlightDetails>
							<flightNumber>
								<xsl:value-of select="../VehResRQInfo/ArrivalDetails/@Number" />
							</flightNumber>
						</inboundFlightDetails>
						<inboundArrivalDate>
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),1,4)" />
							<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),6,2)" />
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),9,2)" />
							<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),12,2)" />
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),15,2)" />
						</inboundArrivalDate>
					</arrivalInfo>
				</xsl:if>
				<rateCodeInfo>
					<fareCategories>
						<fareType>
							<xsl:value-of select="RateQualifier/@RateQualifier" />
						</fareType>
					</fareCategories>
				</rateCodeInfo>
				<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
					<customerInfo>
						<customerReferences>
							<referenceQualifier>CD</referenceQualifier>
							<referenceNumber>
								<xsl:value-of select="RateQualifier/@CorpDiscountNmbr" />
							</referenceNumber>
						</customerReferences>
					</customerInfo>
				</xsl:if>
				<rateInfo>
					<tariffInfo>
						<rateType>
							<xsl:value-of select="RateQualifier/@RateCategory" />
						</rateType>
						<xsl:if test="RateQualifier/@RatePeriod != ''">
							<ratePlanIndicator>
								<xsl:choose>
									<xsl:when test="RateQualifier/@RatePeriod = 'Daily'">DY</xsl:when>
									<xsl:when test="RateQualifier/@RatePeriod = 'Monthly'">MY</xsl:when>
									<xsl:when test="RateQualifier/@RatePeriod = 'Weekend'">WD</xsl:when>
									<xsl:when test="RateQualifier/@RatePeriod = 'Weekly'">WY</xsl:when>
								</xsl:choose>
							</ratePlanIndicator>
						</xsl:if>
					</tariffInfo>
				</rateInfo>
				<xsl:if test="../VehResRQInfo/RentalPaymentPref !=''">
					<payment>
						<formOfPayment>
							<type>
								<xsl:choose>
									<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode != ''">CC</xsl:when>
									<xsl:otherwise>CA</xsl:otherwise>
								</xsl:choose>
							</type>
							<vendorCode>
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode" />
							</vendorCode>
							<creditCardNumber>
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardNumber" />
							</creditCardNumber>
							<expiryDate>
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@ExpireDate" />
							</expiryDate>
							<extendedPayment>
								<xsl:choose>
									<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode != ''">GUA</xsl:when>
									<xsl:otherwise>FOP</xsl:otherwise>
								</xsl:choose>
							</extendedPayment>
						</formOfPayment>
						<xsl:if test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode  != ''">
							<otherFormOfPayment>
								<type>
									<xsl:choose>
										<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode != ''">CC</xsl:when>
										<xsl:otherwise>CA</xsl:otherwise>
									</xsl:choose>
								</type>
								<vendorCode>
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode" />
								</vendorCode>
								<creditCardNumber>
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardNumber" />
								</creditCardNumber>
								<expiryDate>
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@ExpireDate" />
								</expiryDate>
								<extendedPayment>
									<xsl:choose>
										<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode != ''">GUA</xsl:when>
										<xsl:otherwise>FOP</xsl:otherwise>
									</xsl:choose>
								</extendedPayment>
							</otherFormOfPayment>
						</xsl:if>
					</payment>
				</xsl:if>
			</sellData>
		</PoweredCar_Sell>
	</xsl:template>
	<xsl:template match="HotelReservation">
		<PoweredHotel_Sell>
			<productInformation>
				<xsl:apply-templates select="RoomStays/RoomStay" />
			</productInformation>
		</PoweredHotel_Sell>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="RoomStay">
		<hotelPropertyInfo>
			<hotelReference>
				<chainCode>
					<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
				</chainCode>
				<cityCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCityCode" />
				</cityCode>
				<hotelCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
				</hotelCode>
			</hotelReference>
		</hotelPropertyInfo>
		<requestedDates>
			<businessSemantic>CHK</businessSemantic>
			<timeMode>LT</timeMode>
			<beginDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@Start),1,4)" />
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@Start),6,2)" />
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@Start),9,2)" />
				</day>
			</beginDateTime>
			<endDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@End),1,4)" />
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@End),6,2)" />
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@End),9,2)" />
				</day>
			</endDateTime>
		</requestedDates>
		<roomRateDetails>
			<roomInformation>
				<xsl:choose>
					<xsl:when test="BasicPropertyInfo/@HotelCodeContext='SA' or BasicPropertyInfo/@HotelCodeContext='CA' or ../../../../../POS/TPA_Extensions/Provider/System = 'Test'">
						<roomRateIdentifier>
							<roomType>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),1,3)" />
							</roomType>
							<ratePlanCode>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),4,3)" />
							</ratePlanCode>
						</roomRateIdentifier>
					</xsl:when>
					<xsl:otherwise>
						<bookingCode>
							<xsl:value-of select="RoomRates/RoomRate/@BookingCode" />
						</bookingCode>
					</xsl:otherwise>
				</xsl:choose>
				<guestCountDetails>
					<numberOfUnit>
						<xsl:value-of select="GuestCounts/GuestCount/@Count" />
					</numberOfUnit>
					<unitQualifier>A</unitQualifier>
				</guestCountDetails>
			</roomInformation>
			<xsl:if test="RoomTypes/RoomType/TPA_Extensions/HotelData/ExtraAdult !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayAdult !='' or 	RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayChild !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/Crib !=''"></xsl:if>
		</roomRateDetails>
		<relatedProduct>
			<quantity>
				<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits" />
			</quantity>
		</relatedProduct>
		<selectionInformation>
			<selectionDetails>
				<option>P10</option>
			</selectionDetails>
		</selectionInformation>
		<guaranteeOrDeposit>
			<xsl:choose>
				<xsl:when test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!= ''">
					<paymentInfo>
						<paymentDetails>
							<formOfPaymentCode>1</formOfPaymentCode>
							<paymentType>1</paymentType>
							<serviceToPay>3</serviceToPay>
						</paymentDetails>
					</paymentInfo>
				</xsl:when>
				<xsl:when test="DepositPayments/RequiredPayment/AcceptedPayment/PaymentCard!=''">
					<paymentInfo>
						<paymentDetails>
							<formOfPaymentCode>ADV</formOfPaymentCode>
							<paymentType>2</paymentType>
							<serviceToPay>3</serviceToPay>
						</paymentDetails>
					</paymentInfo>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!= ''">
				<creditCardInfo>
					<formOfPayment>
						<type>CC</type>
						<vendorCode>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode" />
						</vendorCode>
						<creditCardNumber>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber" />
						</creditCardNumber>
						<expiryDate>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate" />
						</expiryDate>
					</formOfPayment>
				</creditCardInfo>
			</xsl:if>
		</guaranteeOrDeposit>
		<referenceForSegment>
			<xsl:apply-templates select="../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[1]"
				mode="hotel" />
		</referenceForSegment>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="Traveler" mode="hotel">
		<referenceDetails>
			<type>PT</type>
			<value>
				<xsl:value-of select="TravelerRefNumber/@RPH + 1" />
			</value>
		</referenceDetails>
	</xsl:template>
	
	<xsl:template match="PriceData">
		<!--FarePlus_PriceItinerary_Query>
			<TSTFlag>T</TSTFlag>
			<LowestFareOption>T</LowestFareOption>
			<xsl:if test="@PriceType = 'Private'">
				<CAPI_FXXPsgrPlus>
					<PassengerNumber>
						<xsl:for-each select="../PNRData/Traveler">
							<xsl:value-of select="position()"/>
							<xsl:if test="position() != last()">
								<xsl:text>,</xsl:text>
							</xsl:if>
						</xsl:for-each>
					</PassengerNumber>
					<PTC1><xsl:value-of select="NegoFares/PriceRequestInformation/NegotiatedFareCode"/></PTC1>
				</CAPI_FXXPsgrPlus>
			</xsl:if>
			<xsl:if test="@PriceType = 'Published'">
				<xsl:choose>
				    <xsl:when test="not(PublishedFares/FareRestrictPref/AdvResTicketing/AdvReservation)">
						<OtherRestrictions>NAP</OtherRestrictions>               
				    </xsl:when>
				    <xsl:when test="not(PublishedFares/FareRestrictPref/VoluntaryChanges/Penalty)">
						<OtherRestrictions>NPE</OtherRestrictions>           
				    </xsl:when>
				</xsl:choose>
			</xsl:if>
			<SellingCity></SellingCity>
			<TicketingCity></TicketingCity>
		    <CAPI_TicketingIndicatorsPlus>
		    		<PricingTicketingIndicator>RP</PricingTicketingIndicator>
				<PricingTicketingIndicator>RU</PricingTicketingIndicator>
				<xsl:if test="NegoFares/PriceRequestInformation/NegotiatedFareCode != ''">
					<PricingTicketingIndicator>
						<xsl:value-of select="NegoFares/PriceRequestInformation/NegotiatedFareCode"/>
					</PricingTicketingIndicator>
				</xsl:if>
		    </CAPI_TicketingIndicatorsPlus>				
		</FarePlus_PriceItinerary_Query-->
		<!--PoweredFare_PricePNRWithLowerFares-->
		<PoweredFare_PricePNRWithBookingClass> 
			<overrideInformation>
				<attributeDetails>
					<attributeType>RLO</attributeType>
				</attributeDetails>
				<!--attributeDetails>
					<attributeType>CAB</attributeType>
				</attributeDetails-->
				<attributeDetails>
					<attributeType>RP</attributeType>
				</attributeDetails>
				<attributeDetails>
					<attributeType>RU</attributeType>
				</attributeDetails>
			</overrideInformation>
			<xsl:if test="../../POS/Source/@ISOCurrency != ''">
				<currencyOverride>
   					<firstRateDetail>
  						<currencyCode><xsl:value-of select="../../POS/Source/@ISOCurrency"/></currencyCode>
      				</firstRateDetail>      
				</currencyOverride>
			</xsl:if>
		<!--/PoweredFare_PricePNRWithLowerFares-->
		</PoweredFare_PricePNRWithBookingClass> 
	</xsl:template>

</xsl:stylesheet>
