<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_ShowMileageRS Version="1.001">
			<Success />
			<xsl:apply-templates select="Cryptic_GetScreen_Reply/CAPI_Screen/Response" />
		</OTA_ShowMileageRS>
	</xsl:template>
	<xsl:template match="Response">
		<xsl:variable name="screen2">
			<xsl:value-of select="translate(string(.),'&#xD;','/')" />
		</xsl:variable>
		<xsl:variable name="screen1">
			<xsl:value-of select="substring-after($screen2,'XTRA/')" />
		</xsl:variable>
		<xsl:variable name="screen">
			<xsl:value-of select="substring-before($screen1,'&gt;')" />
		</xsl:variable>
		<FromCity>
			<xsl:value-of select="substring($screen,1,3)" />
		</FromCity>
		<xsl:call-template name="cities">
			<xsl:with-param name="screen">
				<xsl:value-of select="$screen" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="cities">
		<xsl:param name="screen" />
		<xsl:if test="$screen != ''">
			<xsl:variable name="line">
				<xsl:value-of select="substring-before($screen,'/')" />
			</xsl:variable>
			<xsl:if test="string-length($line) > 9">
				<ToCity>
					<xsl:attribute name="Mileage">
						<xsl:value-of select="translate(substring($screen,10,5),' ','')" />
					</xsl:attribute>
					<xsl:attribute name="AccumulativeMileage">
						<xsl:value-of select="translate(substring($screen,16,5),' ','')" />
					</xsl:attribute>
					<xsl:value-of select="substring($screen,1,3)" />
				</ToCity>
			</xsl:if>
			<xsl:if test="substring-after($screen,'/') = ''">
				<TotalMileage>
					<xsl:value-of select="translate(substring($screen,16,5),' ','')" />
				</TotalMileage>
			</xsl:if>
			<xsl:call-template name="cities">
				<xsl:with-param name="screen">
					<xsl:value-of select="substring-after($screen,'/')" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
