<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_TimeDiffRS Version="1.001">
			<Success />
			<xsl:apply-templates select="Cryptic_GetScreen_Reply/CAPI_Screen" />
		</OTA_TimeDiffRS>
	</xsl:template>
	<xsl:template match="CAPI_Screen">
		<LocalInfo>
			<Time>
			<xsl:variable name="time">
					<xsl:value-of select="substring-after(Response,'IS ')" />
				</xsl:variable>	
			<xsl:variable name="time1">
					<xsl:value-of select="substring-before($time,'/')" />
				</xsl:variable>	
			<xsl:value-of select="substring($time1,1,2)" />:<xsl:value-of select="substring($time1,3,2)" />
		</Time>
			<xsl:variable name="date">
				<xsl:value-of select="substring-after(Response,'ON')" />
			</xsl:variable>
			<xsl:variable name="date1">
				<xsl:value-of select="substring($date,5,7)" />
			</xsl:variable>
			<Date>
				<xsl:text>20</xsl:text>
				<xsl:value-of select="substring($date1,6,2)" />
				<xsl:text>-</xsl:text>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring($date1,3,3)" />
					</xsl:with-param>
				</xsl:call-template>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring($date1,1,2)" />
			</Date>
			<CityCode>
				<xsl:value-of select="substring(Response,11,3)" />
			</CityCode>
		</LocalInfo>
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
