<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<LocationTable>
			<xsl:apply-templates select="AmadeusAirports/PoweredInfo_EncodeDecodeCityReply"/>
		</LocationTable>
	</xsl:template>
	
	<xsl:template match="PoweredInfo_EncodeDecodeCityReply">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:apply-templates select="mainLocation/relatedLocation" mode="names">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="relatedLocation" mode="names">
		<xsl:param name="pos"/>
		<xsl:variable name="code"><xsl:value-of select="sublocationInformation/locationDescription/code"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="../../../PoweredInfo_EncodeDecodeCityReply[position()&lt;$pos][mainLocation/relatedLocation/sublocationInformation/locationDescription/code=$code]"></xsl:when>
			<xsl:otherwise>
				<Location>
					<xsl:attribute name="Type">
						<xsl:choose>
							<xsl:when test="sublocationInformation/locationType='A'">Airport</xsl:when>
							<xsl:when test="sublocationInformation/locationType='H'">Heliport</xsl:when>
							<xsl:when test="sublocationInformation/locationType='R'">Railway</xsl:when>
							<xsl:when test="sublocationInformation/locationType='B'">Bus</xsl:when>
							<xsl:otherwise>Other</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<Code><xsl:value-of select="$code"/></Code>
					<xsl:variable name="name"><xsl:value-of select="sublocationInformation/locationDescription/name"/></xsl:variable>
					<Name>
						<xsl:call-template name="buildName">
							<xsl:with-param name="name"><xsl:value-of select="$name"/></xsl:with-param>
							<xsl:with-param name="finalname"></xsl:with-param>
						</xsl:call-template>
					</Name>
					<City>
						<xsl:attribute name="Code">
							<xsl:value-of select="../locationInformation/locationDescription/code"/>
						</xsl:attribute>
						<xsl:call-template name="buildName">
							<xsl:with-param name="name"><xsl:value-of select="../locationInformation/locationDescription/name"/></xsl:with-param>
							<xsl:with-param name="finalname"></xsl:with-param>
						</xsl:call-template>
					</City>
					<xsl:if test="countryStateInformation/countryIdentification/stateCode!=''">
						<xsl:element name="State">
							<xsl:value-of select="countryStateInformation/countryIdentification/stateCode"/>
						</xsl:element>
					</xsl:if>
					<xsl:element name="Country">
						<xsl:value-of select="countryStateInformation/countryIdentification/countryCode"/>
					</xsl:element>
				</Location>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="buildName">
		<xsl:param name="name"/>
		<xsl:param name="words"/>
		<xsl:param name="finalname"/>
		<xsl:variable name="name1">
			<xsl:choose>
				<xsl:when test="contains($name,' ')">
					<xsl:value-of select="substring-before($name,' ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$name"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$name1 != ''">
				<xsl:variable name="name2">
					<xsl:value-of select="substring($name1,1,1)"/>
					<xsl:if test="string-length($name1) > 1">
						<xsl:value-of select="translate(substring($name1,2),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="name3">
					<xsl:choose>
						<xsl:when test="$finalname = ''">
							<xsl:value-of select="$name2"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($finalname,' ',$name2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:call-template name="buildName">
					<xsl:with-param name="name"><xsl:value-of select="substring-after($name,' ')"/></xsl:with-param>
					<xsl:with-param name="finalname"><xsl:value-of select="$name3"/></xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$finalname"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
