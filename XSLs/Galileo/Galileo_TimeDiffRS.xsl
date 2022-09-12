<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_TimeDiffRS Version="1.001">
			<xsl:apply-templates select="LocalDateTimeCT_6_0" />
		</OTA_TimeDiffRS>
	</xsl:template>
	<xsl:template match="LocalDateTimeCT_6_0">
		<xsl:variable name="zeros">000</xsl:variable>
		<xsl:variable name="time">
			<xsl:value-of select="substring(string($zeros),1,4-string-length(LocalDateTime/Tm))" />
			<xsl:value-of select="LocalDateTime/Tm" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="TransactionErrorCode or LocalDateTime/ErrorCode!=''">
				<Errors>
					<Error Type="Galileo" Code="{LocalDateTime/ErrorCode}">
						<xsl:choose>
							<xsl:when test="LocalDateTime/ErrorCode='0002'">INVALID TRANSACTION</xsl:when>
							<xsl:when test="LocalDateTime/ErrorCode='0005'">INCOMPLETE DATA</xsl:when>
							<xsl:when test="LocalDateTime/ErrorCode='0006'">INVALID CITY CODE</xsl:when>
							<xsl:otherwise>HOST ERROR</xsl:otherwise>
						</xsl:choose>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success />
				<LocalInfo>
					<Time><xsl:value-of select="substring($time,1,2)" />:<xsl:value-of select="substring($time,3,2)" /></Time>
					<Date>
						<xsl:value-of select="substring(LocalDateTime/Dt,1,4)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(LocalDateTime/Dt,5,2)" />
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(LocalDateTime/Dt,7,2)" />
					</Date>
				</LocalInfo>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
