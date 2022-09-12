<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- SITA_InventoryManagementS.xsl													-->
	<!-- ================================================================== -->
	<!-- Date: 31 Jul 2102 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:template match="/">
		<TXML_InventoryManagementRS Version="1.0">
			<xsl:apply-templates select="OTA_ScreenTextRS" />
		</TXML_InventoryManagementRS>
	</xsl:template>
	<xsl:template match="OTA_ScreenTextRS">
		<xsl:choose>
			<xsl:when test="count(TextScreens/TextScreen/TextData) &lt; 4">
				<Errors>
					<xsl:apply-templates select="TextScreens/TextScreen/TextData" mode="error" />
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="resp"><xsl:value-of select="TextScreens/TextScreen/TextData"/></xsl:variable>
				<Success />
				<Inventory>
					<MarketingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="substring($resp,1,2)"/></xsl:attribute>
						<xsl:attribute name="CodeContext"><xsl:value-of select="'IATA'"/></xsl:attribute>
						<xsl:attribute name="FlightNumber"><xsl:value-of select="translate(substring($resp,3,4),' ','')"/></xsl:attribute>
					</MarketingAirline>
					<InitialDate>
						<xsl:variable name="dow"><xsl:value-of select="substring($resp,8,2)"/></xsl:variable>
						<xsl:attribute name="DayOfWeek">
							<xsl:choose>
								<xsl:when test="$dow='MO'"><xsl:value-of select="'Mon'"/></xsl:when>
								<xsl:when test="$dow='TU'"><xsl:value-of select="'Tue'"/></xsl:when>
								<xsl:when test="$dow='WE'"><xsl:value-of select="'Wed'"/></xsl:when>
								<xsl:when test="$dow='TH'"><xsl:value-of select="'Thu'"/></xsl:when>
								<xsl:when test="$dow='FR'"><xsl:value-of select="'Fri'"/></xsl:when>
								<xsl:when test="$dow='SA'"><xsl:value-of select="'Sat'"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="'Sun'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:variable name="nowyear"><xsl:value-of select="substring(NowDate,1,4)"/></xsl:variable>
						<xsl:variable name="nowmonth"><xsl:value-of select="substring(NowDate,6,2)"/></xsl:variable>
						<xsl:variable name="nowday"><xsl:value-of select="substring(NowDate,9,2)"/></xsl:variable>
						<xsl:variable name="flightmonth">
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($resp,12,3)"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="flightday"><xsl:value-of select="substring($resp,10,2)"/></xsl:variable>
						<xsl:choose>
							<xsl:when test="$flightmonth > $nowmonth">
								<xsl:value-of select="concat($nowyear,'-',$flightmonth)"/>
							</xsl:when>
							<xsl:when test="$flightmonth &lt; $nowmonth">
								<xsl:value-of select="concat(($nowyear + 1),'-',$flightmonth)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$flightday &lt; $nowday">
										<xsl:value-of select="concat(($nowyear + 1),'-',$flightmonth)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat($nowyear,'-',$flightmonth)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="concat('-',$flightday)"/>
					</InitialDate> 
				</Inventory>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="TextData" mode="error">
		<Error><xsl:value-of select="."/></Error>
	</xsl:template>
	
	<xsl:template name="month">
		<xsl:param name="month"/>
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
