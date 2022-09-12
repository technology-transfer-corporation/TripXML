<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
================================================================== 
v03_AmadeusWS_TB_Errors.xsl                     								
================================================================== 
Date: 06 Jul 2021 - Kobelev - Pricing Errors Fix
Date: 29 May 2014 - Rastko - added handling of command_cryptic errors
Date: 19 Mar 2014 - Rastko - added handling of car avail errors			
Date: 26 Jan 2014 - Rastko - added handling of Errors tag					  
Date: 06 Sep 2010 - Rastko - accept air sell status code RQ as valid one	                   
Date: 08 Oct 2009 - Rastko													                   
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Air_SellFromRecommendationReply/errorAtMessageLevel/errorSegment/errorDetails/errorCode"/>
		<xsl:apply-templates select="PNR_Reply/originDestinationDetails/itineraryInfo[elementManagementItinerary/status='ERR']" mode="err" />
		<xsl:apply-templates select="PNR_Reply/originDestinationDetails/itineraryInfo[relatedProduct/status='HL']" mode="warning"/>
		<xsl:apply-templates select="PNR_Reply/originDestinationDetails/itineraryInfo[itineraryfreeFormText/text = 'WARNING - CHECK TIMES']" mode="warning1"/>
		<xsl:apply-templates select="PNR_Reply/travellerInfo/nameError[nameErrorInformation/errorDetail/errorCode='ZZZ'] | PNR_Reply/travellerInfo/nameError[nameErrorInformation/errorDetail/qualifier='EC']"/>
		<xsl:apply-templates select="PNR_Reply/generalErrorInfo[messageErrorInformation/errorDetail/qualifier='EC'][messageErrorText/freetextDetail/subjectQualifier='3']/messageErrorText/text" mode="error"/>
		<xsl:apply-templates select="PNR_Reply/generalErrorInfo[messageErrorInformation/errorDetail/qualifier='WEC'][messageErrorText/freetextDetail/subjectQualifier='3']/messageErrorText/text" mode="warning"/>
		<xsl:if test="PNR_Reply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb and not(serviceRequest/ssr/type='RQST')][elementManagementData/status='ERR']">
			<xsl:apply-templates select="PNR_Reply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb and not(serviceRequest/ssr/type='RQST')][elementManagementData/status='ERR']" mode="warning"/>
		</xsl:if>
		<xsl:if test="PNR_Reply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][elementManagementData/status='ERR']">
			<xsl:apply-templates select="PNR_Reply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type='FQTV'][elementManagementData/status='ERR']" mode="warning"/>
		</xsl:if>
		<xsl:apply-templates select="PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/status='ERR']" mode="error"/>
		<xsl:apply-templates select="Command_CrypticReply/longTextString/textStringDetails" mode="err"/>
		<xsl:apply-templates select="Car_SingleAvailabilityReply/errorSituation[applicationError/errorDetails/errorCategory = 'EC']" mode="error"/>
		<xsl:apply-templates select="Car_SellReply/errorWarning[applicationError/errorDetails/errorCategory = 'EC']" mode="error"/>
		<xsl:apply-templates select="Car_SellReply/errorWarning[applicationError/errorDetails/errorCategory = 'WEC']" mode="warning"/>
		<xsl:apply-templates select="Car_SellReply/errorSituation[applicationError/errorDetails/errorCategory = 'EC']" mode="error"/>
		<xsl:apply-templates select="Hotel_SellReply/errorWarning/messageErrorText/freeText" mode="error"/>
		<xsl:apply-templates select="Hotel_SellReply/errorGroup/errorDescription/freeText" mode="error"/>
		<xsl:apply-templates select="OTA_HotelAvailRS/Errors"/>
		<xsl:apply-templates select="Fare_PricePNRWithBookingClassReply/applicationError" mode="error"/>
		<xsl:apply-templates select="Ticket_ImagePlus_Reply/CAPI_Messages[LineType='E']" mode="error"/>
		<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages[LineType='E']" mode="error"/>
		<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages[LineType='L']" mode="error"/>
		<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages[LineType='EC']" mode="err"/>
		<xsl:apply-templates select="Cryptic_GetScreen_Reply/CAPI_Screen/Response" mode="err"/>
		<xsl:apply-templates select="PNR_CreateTSMReply/errorText" mode="warning"/>
		<xsl:apply-templates select="Ticket_UpdateTSTReply/applicationError[applicationErrorInfo/applicationErrorDetail/codeListQualifier='EC']" mode="error"/>
		<xsl:apply-templates select="Ticket_CreateManualTSTReply/applicationError[applicationErrorInfo/applicationErrorDetail/codeListQualifier='EC']/errorText" mode="err"/>
		<xsl:apply-templates select="Errors"/>
	</xsl:template>

	<xsl:template match="errorCode">
		<xsl:choose>
			<xsl:when test="//Air_SellFromRecommendationReply/itineraryDetails/segmentInformation[actionDetails/statusCode != 'OK' and actionDetails/statusCode != 'RQ']">
				<xsl:apply-templates select="//Air_SellFromRecommendationReply/itineraryDetails/segmentInformation[actionDetails/statusCode != 'OK' and actionDetails/statusCode != 'RQ']"/>
			</xsl:when>
			<xsl:otherwise>
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:text>Cannot book </xsl:text>
					<xsl:value-of select="../../../../itineraryDetails/originDestination/origin"/>
					<xsl:value-of select="' '"/>
					<xsl:value-of select="../../../../itineraryDetails/originDestination/destination"/>
				</Error>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="segmentInformation">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="actionDetails/statusCode"/>
			</xsl:attribute>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:text>Flight </xsl:text>
			<xsl:value-of select="flightDetails/companyDetails/marketingCompany"/>
			<xsl:value-of select="flightDetails/flightIdentification/flightNumber"/>
			<xsl:text> Class </xsl:text>
			<xsl:value-of select="flightDetails/flightIdentification/bookingClass"/>
			<xsl:text> Date 20</xsl:text>
			<xsl:value-of select="substring(flightDetails/flightDate/departureDate,5)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(flightDetails/flightDate/departureDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(flightDetails/flightDate/departureDate,1,2)"/>
			<xsl:text> - </xsl:text>
			<xsl:choose>
				<xsl:when test="actionDetails/statusCode = 'WL'">Wait listed</xsl:when>
				<xsl:otherwise>Unable to sell</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>

	<xsl:template match="Errors">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Error"/>
			<xsl:value-of select="Errors/Error"/>
		</Error>
	</xsl:template>

	<xsl:template match="applicationError" mode="error">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="errorText/errorFreeText"/>
			<xsl:value-of select="errorWarningDescription/freeText"/>
		</Error>
	</xsl:template>

	<xsl:template match="itineraryInfo" mode="err">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:text>Flight </xsl:text>
			<xsl:value-of select="travelProduct/companyDetail/identification"/>
			<xsl:value-of select="travelProduct/productDetails/identification"/>
			<xsl:text> Class </xsl:text>
			<xsl:value-of select="travelProduct/productDetails/classOfService"/>
			<xsl:text> Date 20</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,5)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="errorInfo/errorfreeFormText/text" />
		</Error>
	</xsl:template>

	<xsl:template match="nameError">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="nameErrorFreeText/text" />
			<xsl:value-of select="../travellerInformation/traveller/surname"/>
			<xsl:text>/</xsl:text>
			<xsl:value-of select="../travellerInformation/passenger/firstName"/>
		</Error>
	</xsl:template>

	<xsl:template match="CAPI_Messages" mode="error">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Text = 'Local error'">
					<xsl:text>Invalid XML structure - contact your site administrator</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Text" />
				</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>

	<xsl:template match="text | freeText" mode="error">
		<xsl:choose>
			<xsl:when test="contains(.,'SEAT ')">
				<Warning>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="." />
				</Warning>
			</xsl:when>
			<xsl:otherwise>
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="." />
				</Error>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="text" mode="warning">
		<Warning>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="." />
		</Warning>
	</xsl:template>

	<xsl:template match="errorText" mode="warning">
		<Warning>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="freeText" />
		</Warning>
	</xsl:template>

	<xsl:template match="errorText" mode="err">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="freeText" />
		</Error>
	</xsl:template>

	<xsl:template match="CAPI_Messages" mode="err">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>

	<xsl:template match="dataElementsIndiv" mode="error">
		<xsl:choose>
			<xsl:when test="elementManagementData/segmentName='SSR' and serviceRequest/ssr/type='RQST'">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:text>Cannot book seat </xsl:text>
					<xsl:value-of select="concat(serviceRequest/ssrb/data,'-')"/>
					<xsl:variable name="seat">
						<xsl:value-of select="serviceRequest/ssrb/data"/>
					</xsl:variable>
					<xsl:variable name="flight">
						<xsl:value-of select="../../PNR_AddMultiElements/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='STR' and seatGroup/seatRequest/special/data=$seat]/referenceForDataElement/reference[qualifier='ST']/number"/>
					</xsl:variable>
					<xsl:value-of select="concat(../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number=$flight]/travelProduct/companyDetail/identification,'-')"/>
					<xsl:value-of select="concat(../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number=$flight]/travelProduct/productDetails/identification,'-')"/>
					<xsl:value-of select="concat(../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number=$flight]/travelProduct/boardpointDetail/cityCode,'-')"/>
					<xsl:value-of select="../../originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number=$flight]/travelProduct/offpointDetail/cityCode"/>
				</Error>
			</xsl:when>
			<xsl:when test="elementManagementData/segmentName='SSR'">
				<Warning>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="elementErrorInformation/elementErrorText/text"/>
				</Warning>
			</xsl:when>
			<xsl:when test="elementManagementData/segmentName='OP'">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:text>QUEUE ELEMENT-</xsl:text>
					<xsl:value-of select="elementErrorInformation/errorWarningDescription/freeText"/>
				</Error>
			</xsl:when>
			<xsl:otherwise>
				<Error Type="Amadeus">
					<xsl:value-of select="elementErrorInformation/elementErrorText/text"/>
					<xsl:value-of select="elementErrorInformation/errorWarningDescription/freeText"/>
					<xsl:value-of select="concat(' ',elementManagementData/segmentName,' element')"/>
				</Error>
				<xsl:if test="miscellaneousRemarks">
					<Error Type="Amadeus">
						<xsl:value-of select="miscellaneousRemarks/remarks/type"/>
						<xsl:value-of select="miscellaneousRemarks/remarks/category"/>
						<xsl:value-of select="miscellaneousRemarks/remarks/freetext"/>
					</Error>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dataElementsIndiv" mode="warning">
		<Warning Type="Amadeus">
			<xsl:if test="serviceRequest/ssrb/data">
				<xsl:value-of select="serviceRequest/ssrb/data"/>
			</xsl:if>
			<xsl:if test="serviceRequest/ssr/type = 'FQTV'">
				<xsl:text>FREQUENT FLYER </xsl:text>
				<xsl:value-of select="serviceRequest/ssr/companyId"/>
			</xsl:if>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="elementErrorInformation/elementErrorText/text"/>
		</Warning>
	</xsl:template>

	<xsl:template match="itineraryInfo" mode="warning">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:text>Flight </xsl:text>
			<xsl:value-of select="travelProduct/companyDetail/identification"/>
			<xsl:value-of select="travelProduct/productDetails/identification"/>
			<xsl:text> Class </xsl:text>
			<xsl:value-of select="travelProduct/productDetails/classOfService"/>
			<xsl:text> Date 20</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,5)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
			<xsl:text> - IS WAIT LIST</xsl:text>
		</Error>
	</xsl:template>

	<xsl:template match="itineraryInfo" mode="warning1">
		<Warning>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:text>Flight </xsl:text>
			<xsl:value-of select="travelProduct/companyDetail/identification"/>
			<xsl:value-of select="travelProduct/productDetails/identification"/>
			<xsl:text> Class </xsl:text>
			<xsl:value-of select="travelProduct/productDetails/classOfService"/>
			<xsl:text> Date 20</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,5)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="itineraryfreeFormText/text[contains(.,'WARNING')]"/>
		</Warning>
	</xsl:template>

	<xsl:template match="errorWarning | errorSituation" mode="error">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:text>CAR - </xsl:text>
			<xsl:value-of select="errorFreeText/freeText"/>
		</Error>
	</xsl:template>
	<xsl:template match="errorWarning" mode="warning">
		<xsl:if test="starts-with(errorFreeText/freeText, 'INVALID RESPONSE - RETRY')">
			<Error>
				<xsl:attribute name="Type">Amadeus</xsl:attribute>
				<xsl:text>CAR - </xsl:text>
				<xsl:value-of select="errorFreeText/freeText"/>
			</Error>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Response | textStringDetails" mode="err">
		<xsl:choose>
			<xsl:when test="contains(.,'INVALID CONNECTING CITY')"></xsl:when>
			<xsl:when test="contains((.),'CHECK MINIMUM CONNECTING TIME')">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="."/>
				</Error>
			</xsl:when>
			<xsl:when test="contains(.,'INVALID')">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="."/>
				</Error>
			</xsl:when>
			<xsl:when test="contains(.,'NO FARE FOR BOOKING CODE')">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="."/>
				</Error>
			</xsl:when>
			<xsl:when test="contains(.,'RECORD LOCATOR NOT FOUND')">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="'TRAVELER PROFILE RECORD LOCATOR NOT FOUND'"/>
				</Error>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
