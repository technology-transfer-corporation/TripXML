<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- SITA_InventoryManagementRQ.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 31 Jul 2012 - Rastko	- new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="TXML_InventoryManagementRQ" />
	</xsl:template>
	<xsl:template match="TXML_InventoryManagementRQ">
		<OTA_ScreenTextRQ xmlns="http://www.opentravel.org/OTA/2003/05" TransactionIdentifier="SESSION">
		     <POS>
		          <Source>
		          	<xsl:attribute name="ERSP_UserID">
		          		<xsl:value-of select="POS/Source/@ERSP_UserID"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="AgentSine">
		          		<xsl:value-of select="POS/Source/@AgentSine"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="PseudoCityCode">
		          		<xsl:value-of select="POS/Source/@PseudoCityCode"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="AgentDutyCode">
		          		<xsl:value-of select="POS/Source/@AgentDutyCode"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="ISOCountry">
		          		<xsl:value-of select="POS/Source/@ISOCountry"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="AirlineVendorID">
		          		<xsl:value-of select="POS/Source/@AirlineVendorID"/>
		          	</xsl:attribute>
		          	<xsl:attribute name="AirportCode">
		          		<xsl:value-of select="POS/Source/@AirportCode"/>
		          	</xsl:attribute>
		          </Source> 
		     </POS>
			<ScreenEntry>
				<xsl:choose>
					<xsl:when test="FlightNumber/@InventoryAction='CloseClass'">
						<xsl:value-of select="'IM:LX/'"/>
						<xsl:value-of select="concat(FlightNumber,'/')"/>
						<xsl:value-of select="substring(InitialDate,9,2)"/>
						<xsl:call-template name="month">
							<xsl:with-param name="month">
								<xsl:value-of select="substring(InitialDate,6,2)"/>
							</xsl:with-param>
						</xsl:call-template>
						<xsl:value-of select="'/'"/>
						<xsl:if test="FinalDate!=''">
							<xsl:value-of select="substring(FinalDate,9,2)"/>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(FinalDate,6,2)"/>
								</xsl:with-param>
							</xsl:call-template>
							<xsl:value-of select="'/'"/>
						</xsl:if>
						<xsl:if test="DayOfWeek!=''">
							<xsl:choose>
								<xsl:when test="DayOfWeek='Mon'">1</xsl:when>
								<xsl:when test="DayOfWeek='Tue'">2</xsl:when>
								<xsl:when test="DayOfWeek='Wed'">3</xsl:when>
								<xsl:when test="DayOfWeek='Thu'">4</xsl:when>
								<xsl:when test="DayOfWeek='Fri'">5</xsl:when>
								<xsl:when test="DayOfWeek='Sat'">6</xsl:when>
								<xsl:otherwise>7</xsl:otherwise>
							</xsl:choose>
							<xsl:value-of select="'/'"/>
						</xsl:if>
						<xsl:value-of select="concat(DepartureAirport/@LocationCode,ArrivalAirport/@LocationCode,'/')"/>
						<xsl:for-each select="BookingClassPref">
							<xsl:value-of select="concat(@ResBookDesigCode,'/')"/>
						</xsl:for-each>
						<xsl:value-of select="'S'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'IM:L/'"/>
						<xsl:value-of select="concat(FlightNumber,'/')"/>
						<xsl:value-of select="substring(InitialDate,9,2)"/>
						<xsl:call-template name="month">
							<xsl:with-param name="month">
								<xsl:value-of select="substring(InitialDate,6,2)"/>
							</xsl:with-param>
						</xsl:call-template>
						<xsl:value-of select="'/'"/>
						<xsl:if test="FinalDate!=''">
							<xsl:value-of select="substring(FinalDate,9,2)"/>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(FinalDate,6,2)"/>
								</xsl:with-param>
							</xsl:call-template>
							<xsl:value-of select="'/'"/>
						</xsl:if>
						<xsl:if test="DayOfWeek!=''">
							<xsl:choose>
								<xsl:when test="DayOfWeek='Mon'">1</xsl:when>
								<xsl:when test="DayOfWeek='Tue'">2</xsl:when>
								<xsl:when test="DayOfWeek='Wed'">3</xsl:when>
								<xsl:when test="DayOfWeek='Thu'">4</xsl:when>
								<xsl:when test="DayOfWeek='Fri'">5</xsl:when>
								<xsl:when test="DayOfWeek='Sat'">6</xsl:when>
								<xsl:otherwise>7</xsl:otherwise>
							</xsl:choose>
							<xsl:value-of select="'/'"/>
						</xsl:if>
						<xsl:value-of select="concat(DepartureAirport/@LocationCode,ArrivalAirport/@LocationCode)"/>
						<xsl:for-each select="BookingClassPref">
							<xsl:value-of select="concat('/',@ResBookDesigCode,@NumberOfSeats)"/>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</ScreenEntry>
		</OTA_ScreenTextRQ>
	</xsl:template> 
	
	<xsl:template name="month">
		<xsl:param name="month"/>
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

