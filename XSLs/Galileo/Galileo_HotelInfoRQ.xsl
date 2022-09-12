<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_HotelInfoRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 25 Feb 2006 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelDescriptiveInfoRQ" />
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="OTA_HotelDescriptiveInfoRQ">
		<HotelDescription_5_0>
			<xsl:apply-templates select="HotelDescriptiveInfos/HotelDescriptiveInfo" />
		</HotelDescription_5_0>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelDescriptiveInfo">
		<HotelDescMods>
			<StartDt></StartDt>
			<xsl:if test="@ChainCode !='' and (not(@HotelCode) or @HotelCode='')">
				<Chain>
					<xsl:value-of select="@ChainCode" />
					<xsl:value-of select="string('  ')" />
				</Chain>
			</xsl:if>
			<xsl:if test="@HotelCode !=''">
				<PropID>
					<xsl:value-of select="@HotelCode" />
				</PropID>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="FacilityInfo and Policies and AreaInfo = ' '">
					<KeywordAry>
						<Keyword>
							<xsl:value-of select="string('     ')" />
						</Keyword>
						<Keyword>
							<xsl:value-of select="string('     ')" />
						</Keyword>
						<Keyword>
							<xsl:value-of select="string('     ')" />
						</Keyword>
					</KeywordAry>
				</xsl:when>
				<xsl:otherwise>
					<!--Note that you can only send 3 Keywords at a time on Galileo -->
					<KeywordAry>
						<xsl:if test="FacilityInfo/@SendMeetingRoom = 'true'">
							<Keyword>MEET</Keyword>
						</xsl:if>
						<xsl:if test="FacilityInfo/@SendGuestRooms = 'true'">
							<Keyword>ROOM</Keyword>
						</xsl:if>
						<xsl:if test="FacilityInfo/@SendRestaurants = 'true'">
							<Keyword>MEAL</Keyword>
						</xsl:if>
						<xsl:if test="Policies/@SendPolicies='true'">
							<Keyword>BOOK</Keyword>
						</xsl:if>
						<xsl:if test="AreaInfo/@SendRefPoints='true'">
							<Keyword>LOCA</Keyword>
						</xsl:if>
						<xsl:if test="AreaInfo/@SendAttractions='true'">
							<Keyword>RECR</Keyword>
						</xsl:if>
						<xsl:if test="ContactInfo/@SendData='true'">
							<Keyword>CONT</Keyword>
						</xsl:if>
					</KeywordAry>
				</xsl:otherwise>
			</xsl:choose>
			<WantKeyworkList>N</WantKeyworkList>
			<MoreInd>N</MoreInd>
		</HotelDescMods>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<!--Note;  this is a table of all the keywords available to Galileo - there is an 'ok' in front of the ones which I was able to map to OTA schema - perhaps we want to add the other ones?
OK   BOOKING            Booking guidelines and policies  P BOOK - 
CATEGORY           Category  A - - 
CREDITCARD         Credit Card policy  - CRED - 
COMMISSION         Commission  C COMM - 
CORPORATERATE      Corporate rates  - CORP - 
OK  CONTACTS           Contacts  - CONT - 
CANCELLATIONPOLICY Cancellation policy  - CANC - 
RATECODEDESCRIP    Rate code description  - DESC - 
DINING             Dining facilities  - - - 
DIRECTIONS         Directions to Hotel  - DIRS - 
DEPOSIT            Deposit Information  D DPST - 
EXTRACHARGES       Extra charges  E - - 
FACILITIES         Facilities  F FACI - 
FAMILY             Family plan  - FAMI - 
FREQUENT           Frequent Traveler  Q FREQ - 
GROUP              Group information  - GRPS - 
GUARANTEE          Guarantee policy  G GUAR - 
HELP               Customer Service  - HELP - 
INDEX              Retrieves promo and contact for hotel  - INDX - 
OK LOCATION           Hotel Location  L LOCA - 
OK MEAL               Meal information  N MEAL - 
ok   MEETING            Meeting facilities  M - - 
MINMAXSTAY         Minimum / maximum stay  S - - 
OTHER              OTHR Other  O OTHR - 
PROMOTIONAL        Promotional information  - PROM - 
ok  RECREATION         Recreation  - RECR - 
OK  ROOM               Room / unit types available  R ROOM - 
SAFETY             Safety features  Y - - 
SERVICES           Services available  - SERV - 
TAX                Tax information  - TAXS - 
TRANSPORTATION     Area Transportation  T TRAN - 
TRAVELINDUSTRYINFO Travel Industry information.  - TRVL  -->
</xsl:stylesheet>