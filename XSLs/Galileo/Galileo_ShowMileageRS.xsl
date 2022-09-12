<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_ShowMileageRS Version="1.001">
			<Success />
			<xsl:apply-templates select="CrypticRS/Response" />
		</OTA_ShowMileageRS>
	</xsl:template>
	<xsl:template match="Response">
		<xsl:variable name="screen1">
			<xsl:value-of select="substring-after(string(.),'REMARKS')" />
		</xsl:variable>
		<xsl:variable name="screen">
			<xsl:value-of select="substring($screen1,69)" />
		</xsl:variable>
		<FromCity>
			<xsl:value-of select="substring($screen,1,3)" />
		</FromCity>
		<xsl:call-template name="cities">
			<xsl:with-param name="screen">
				<xsl:value-of select="substring($screen,65)" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="cities">
		<xsl:param name="screen" />
		<xsl:variable name="line1">
			<xsl:value-of select="substring($screen,65,10)" />
		</xsl:variable>
		<xsl:if test="substring($screen,1,5) != '     '">
			<xsl:variable name="totm">
				<xsl:choose>
					<xsl:when test="substring($screen,22,1) = 'E'">
						<xsl:value-of select="substring($screen,17,5) + substring($screen,59,5)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring($screen,17,5)" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<ToCity>
				<xsl:attribute name="Mileage">
					<xsl:value-of select="substring($screen,10,5)" />
				</xsl:attribute>
				<xsl:attribute name="AccumulativeMileage">
					<xsl:value-of select="$totm" />
				</xsl:attribute>
				<xsl:value-of select="substring($screen,1,3)" />
			</ToCity>
			<xsl:if test="substring($line1,1,5) = '     '">
				<TotalMileage>
					<xsl:value-of select="$totm" />
				</TotalMileage>
			</xsl:if>
			<xsl:call-template name="cities">
				<xsl:with-param name="screen">
					<xsl:value-of select="substring($screen,65)" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
